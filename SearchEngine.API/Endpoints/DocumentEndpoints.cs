using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SearchEngine.API.Core;
using SearchEngine.Core.Types;
using SearchEngine.Dtos;
using SearchEngine.Filters;
using SearchEngine.Mappings;
using SearchEngine.Services;

namespace SearchEngine.Enpoints;

public static class DocumentEndpoints
{
  const string PostDocumentEndpointName = "PostDocument";

  public static RouteGroupBuilder MapDocumentEndpoint(this WebApplication app)
  {
    var group = app.MapGroup("documents");

    // POST /documents
    group
      .MapPost(
        "/",
        async (
          [FromForm] UploadDocDto uploadDTO,
          MongoDbContext db,
          CloudinaryService cloudinaryService,
          IBackgroundTaskQueue taskQueue
        ) =>
        {
          try
          {
            var file = uploadDTO.File;
            var fileName =
              (uploadDTO.FileName is null || uploadDTO.FileName == string.Empty)
                ? file.FileName
                : uploadDTO.FileName;
            var ext = Path.GetExtension(file.FileName);
            Console.WriteLine(ext);
            if (file == null)
              return Results.BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();

            var processor = new DocumentProcessor();
            var (meta, terms, keywords) = processor.ParseDocument(stream, fileName, ext);

            var uploadResult = await cloudinaryService.UploadFileAsync(
              stream,
              fileName,
              publicId: $"docs/{Path.GetFileNameWithoutExtension(fileName)}",
              useFileName: true
            );
            ;
            if (uploadResult.Error != null)
              return Results.Json(
                new { error = uploadResult.Error.Message },
                statusCode: StatusCodes.Status500InternalServerError
              );

            var metaData = new FileMetadata()
            {
              Author = meta.Author,
              PageCount = meta.PageCount,
              WordCount = meta.WordCount,
              FileSize = meta.FileSizeBytes,
            };
            var newDocument = new DocumentModel()
            {
              Title = meta.Title,
              FilePath = uploadResult.PublicId,
              FileType = ext,
              Keywords = keywords,
              Metadata = metaData,
            };
            await db.Documents.InsertOneAsync(newDocument);

            taskQueue.Enqueue((newDocument.Id, terms));

            return Results.Ok(DocumentMapping.ToDto(newDocument));
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            return Results.BadRequest("Something went wrong");
          }
        }
      )
      .Accepts<UploadDocDto>("multipart/form-data") // Tells Swagger this is a file upload
      .Produces<DocumentResponseDto>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status400BadRequest)
      .WithName("UploadDocument")
      .WithOpenApi(operation =>
        new(operation)
        {
          Summary = "Uploads a document",
          Description = "Uploads a document file and returns its public URL and ID.",
        }
      )
      .AddEndpointFilter<ValidationFilter<UploadDocDto>>()
      .DisableAntiforgery();

    group
      .MapGet(
        "/search",
        async (
          HttpRequest req,
          DocMatcher docMatcher,
          MongoDbContext db,
          [FromQuery] string q,
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 20
        ) =>
        {
          var matchedDocs = await docMatcher.MatchQueryAsync(q);
          var paginatedDocs = matchedDocs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

          var ids = paginatedDocs.Select(d => d.Id);

          var totalCount = matchedDocs.Count;
          var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

          var results = await db
            .Documents.Find(Builders<DocumentModel>.Filter.In(d => d.Id, ids))
            .ToListAsync();
          var docLookup = results.ToLookup(d => d.Id);
          var orderedDocs = paginatedDocs
            .SelectMany(s => docLookup[s.Id].Select(d => new { Document = d, s.Score }))
            .ToList();

          var baseUrl = $"{req.Scheme}://{req.Host}{req.Path}";

          // Keep existing query params except page
          var queryParams = req
            .Query.Where(q => !string.Equals(q.Key, "page", StringComparison.OrdinalIgnoreCase))
            .SelectMany(q => q.Value.Select(v => $"{q.Key}={Uri.EscapeDataString(v ?? "")}"))
            .ToList();

          string BuildUrl(int targetPage) =>
            $"{baseUrl}?page={targetPage}&pageSize={pageSize}"
            + (queryParams.Any() ? "&" + string.Join("&", queryParams) : "");

          var nextPageUrl = page < totalPages ? BuildUrl(page + 1) : null;
          var prevPageUrl = page > 1 ? BuildUrl(page - 1) : null;

          return Results.Ok(
            new
            {
              Page = page,
              PageSize = pageSize,
              TotalCount = totalCount,
              TotalPages = totalPages,
              Items = orderedDocs.Select(d => DocumentMapping.ToRankDto(d.Document, d.Score)),
              PrevPageUrl = prevPageUrl,
              NextPageUrl = nextPageUrl,
            }
          );
        }
      )
      .Produces<SearchResponseDto>(StatusCodes.Status200OK)
      .WithName("QueryDocument")
      .WithOpenApi(operation =>
        new(operation)
        {
          Summary = "Queries a document",
          Description = "Matches query with uploaded documents file and returns relevant documents",
        }
      );

    return group;
  }
}

record class SearchResponseDto
{
  public required int Page { get; set; }
  public required int PageSize { get; set; }
  public required int TotalCount { get; set; }
  public required int TotalPages { get; set; }
  public required List<DocumentRankResponseDto> Items { get; set; }
  public string? PrevPageUrl { get; set; }
  public string? NextPageUrl { get; set; }
}
