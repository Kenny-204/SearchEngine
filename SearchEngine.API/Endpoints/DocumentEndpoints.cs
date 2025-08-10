using Microsoft.AspNetCore.Mvc;
using SearchEngine.Core.Types;
using SearchEngine.Dtos;
using SearchEngine.Filters;
using SearchEngine.Mappings;
using SearchEngine.Parser;
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
          CloudinaryService cloudinaryService
        ) =>
        {
          try
          {
            var file = uploadDTO.File;
            var fileName =
              (uploadDTO.FileName == string.Empty) ? file.FileName : uploadDTO.FileName;
            if (file == null)
              return Results.BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();

            var processor = new DocumentProcessor();
            var (meta, terms, keywords) = processor.ParseDocument(stream, "test.txt");

            var uploadResult = await cloudinaryService.UploadFileAsync(
              stream,
              file.FileName,
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
              FileType = meta.Extension,
              Keywords = keywords,
              Metadata = metaData,
            };
            await db.Documents.InsertOneAsync(newDocument);

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

    return group;
  }
}
