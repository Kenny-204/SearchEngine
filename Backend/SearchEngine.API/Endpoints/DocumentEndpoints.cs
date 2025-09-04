using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SearchEngine.API.Core;
using SearchEngine.Core.Types;
using SearchEngine.Dtos;
using SearchEngine.Filters;
using SearchEngine.Mappings;
using SearchEngine.Services;
using System;

namespace SearchEngine.Enpoints;

public static class DocumentEndpoints
{
  const string PostDocumentEndpointName = "PostDocument";

  public static RouteGroupBuilder MapDocumentEndpoint(this WebApplication app)
  {
    var group = app.MapGroup("documents");

    // GET /documents - List all documents with pagination and filtering
    group
      .MapGet(
        "/",
        async (
          MongoDbContext db,
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 10,
          [FromQuery] string? searchTerm = null,
          [FromQuery] string? fileType = null,
          [FromQuery] string? sortBy = "createdAt",
          [FromQuery] string? sortOrder = "desc"
        ) =>
        {
          try
          {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            // Build filter
            var filter = Builders<DocumentModel>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
              var searchFilter = Builders<DocumentModel>.Filter.Or(
                Builders<DocumentModel>.Filter.Regex("title", new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<DocumentModel>.Filter.AnyIn("keywords", new[] { searchTerm })
              );
              filter = Builders<DocumentModel>.Filter.And(filter, searchFilter);
            }

            if (!string.IsNullOrWhiteSpace(fileType))
            {
              filter = Builders<DocumentModel>.Filter.And(filter, 
                Builders<DocumentModel>.Filter.Eq("fileType", fileType));
            }

            // Build sort
            var sort = sortOrder?.ToLower() == "asc" 
              ? Builders<DocumentModel>.Sort.Ascending(sortBy)
              : Builders<DocumentModel>.Sort.Descending(sortBy);

            // Get total count for pagination
            var totalCount = await db.Documents.CountDocumentsAsync(filter);

            // Get documents with pagination
            var documents = await db.Documents
              .Find(filter)
              .Sort(sort)
              .Skip((page - 1) * pageSize)
              .Limit(pageSize)
              .ToListAsync();

            // Convert to DTOs
            var documentDtos = documents.Select(DocumentMapping.ToDto).ToList();

            // Calculate pagination info
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasNextPage = page < totalPages;
            var hasPreviousPage = page > 1;

            var response = new DocumentListResponse
            {
              Documents = documentDtos,
              Pagination = new PaginationInfo
              {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = (int)totalCount,
                TotalPages = totalPages,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
              }
            };

            return Results.Ok(response);
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            return Results.Problem("An error occurred while fetching documents");
          }
        }
      )
      .Produces<DocumentListResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError)
      .WithName("GetDocuments")
      .WithOpenApi(operation =>
        new(operation)
        {
          Summary = "Get list of uploaded documents",
          Description = "Retrieves a paginated list of uploaded documents with optional filtering and sorting.",
        }
      );

    // GET /documents/{id} - Get a specific document by ID
    group
      .MapGet(
        "/{id}",
        async (
          string id,
          MongoDbContext db
        ) =>
        {
          try
          {
            if (!MongoDB.Bson.ObjectId.TryParse(id, out var objectId))
            {
              return Results.BadRequest("Invalid document ID format");
            }

            var document = await db.Documents.Find(d => d.Id == objectId).FirstOrDefaultAsync();
            
            if (document == null)
            {
              return Results.NotFound("Document not found");
            }

            var documentDto = DocumentMapping.ToDto(document);
            return Results.Ok(documentDto);
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            return Results.Problem("An error occurred while fetching the document");
          }
        }
      )
      .Produces<DocumentResponseDto>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status400BadRequest)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError)
      .WithName("GetDocumentById")
      .WithOpenApi(operation =>
        new(operation)
        {
          Summary = "Get document by ID",
          Description = "Retrieves a specific document by its unique identifier.",
        }
      );

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
            var originalFileName = file.FileName; // Always use the original file name with extension
            var displayName = (uploadDTO.FileName is null || uploadDTO.FileName == string.Empty)
                ? originalFileName
                : uploadDTO.FileName;
            var ext = Path.GetExtension(originalFileName);

            if (file == null)
              return Results.BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();

            var processor = new DocumentProcessor();
            var (meta, terms, keywords) = processor.ParseDocument(stream, originalFileName, ext);

            var uploadResult = await cloudinaryService.UploadFileAsync(
              stream,
              originalFileName,
              publicId: $"docs/{originalFileName}",
              useFileName: false
            );
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

            // Use the best available URL from Cloudinary upload result
            var filePath = !string.IsNullOrEmpty(uploadResult.SecureUrl?.ToString()) ? uploadResult.SecureUrl?.ToString() :
                          !string.IsNullOrEmpty(uploadResult.Url?.ToString()) ? uploadResult.Url?.ToString() :
                          $"https://res.cloudinary.com/dpfuj9km4/raw/upload/{uploadResult.PublicId}";

            var newDocument = new DocumentModel()
            {
              Title = displayName,
              FilePath = filePath,
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

    // DELETE /documents/{id} - Delete a specific document
    group
      .MapDelete(
        "/{id}",
        async (
          string id,
          MongoDbContext db,
          CloudinaryService cloudinaryService
        ) =>
        {
          try
          {
            if (!MongoDB.Bson.ObjectId.TryParse(id, out var objectId))
            {
              return Results.BadRequest("Invalid document ID format");
            }

            // Find the document first to get the Cloudinary public ID
            var document = await db.Documents.Find(d => d.Id == objectId).FirstOrDefaultAsync();
            
            if (document == null)
            {
              return Results.NotFound("Document not found");
            }

            // Delete from Cloudinary first
            try
            {
              var cloudinaryDeleteResult = await cloudinaryService.DeleteFileAsync(document.FilePath);
              if (cloudinaryDeleteResult.Error != null)
              {
                Console.WriteLine($"Warning: Failed to delete from Cloudinary: {cloudinaryDeleteResult.Error.Message}");
                // Continue with database deletion even if Cloudinary deletion fails
              }
            }
            catch (Exception cloudinaryEx)
            {
              Console.WriteLine($"Warning: Cloudinary deletion failed: {cloudinaryEx.Message}");
              // Continue with database deletion even if Cloudinary deletion fails
            }

            // Delete from MongoDB
            var mongoDeleteResult = await db.Documents.DeleteOneAsync(d => d.Id == objectId);
            
            if (mongoDeleteResult.DeletedCount == 0)
            {
              return Results.NotFound("Document not found or already deleted");
            }

            return Results.Ok(new { message = "Document deleted successfully", deletedId = id });
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            return Results.Problem("An error occurred while deleting the document");
          }
        }
      )
      .Produces<object>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status400BadRequest)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError)
      .WithName("DeleteDocument")
      .WithOpenApi(operation =>
        new(operation)
        {
          Summary = "Delete document by ID",
          Description = "Deletes a specific document by its unique identifier from both MongoDB and Cloudinary.",
        }
      );

// GET /documents/search - Search documents
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
            .SelectMany(s =>
              docLookup[s.Id]
                .Select(d => new
                {
                  Document = d,
                  s.Score,
                  s.Matches,
                })
            )
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
              Items = orderedDocs.Select(d =>
                DocumentMapping.ToRankDto(d.Document, d.Score, d.Matches)
              ),
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

    // GET /documents/count - Get total document count
    group
      .MapGet(
        "/count",
        async (MongoDbContext db) =>
        {
          try
          {
            var count = await db.Documents.CountDocumentsAsync(FilterDefinition<DocumentModel>.Empty);
            return Results.Ok(new { count = (int)count });
          }
          catch (Exception e)
          {
            return Results.Problem("An error occurred while getting document count");
          }
        }
      )
      .Produces<object>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError)
      .WithName("GetDocumentCount")
      .WithOpenApi(operation =>
        new(operation)
        {
          Summary = "Get document count",
          Description = "Returns the total number of documents in the system",
        }
      );

          // TEMPORARY: Force indexing for testing - COMMENT OUT WHEN DONE TESTING
     group
       .MapPost(
         "/force-index",
         async (MongoDbContext db, Indexer indexer) =>
         {
           try
           {
             // Get all documents that need indexing (IndexedAt is null)
             var documentsToIndex = await db.Documents
               .Find(d => d.IndexedAt == null)
               .ToListAsync();

             if (!documentsToIndex.Any())
             {
               return Results.Ok(new { 
                 message = "No documents need indexing",
                 indexedCount = 0
               });
             }

                           // For each document, we need to get its terms and process them
              // Since we don't have the original terms, we'll need to re-process the documents
              var docTermsBatch = new List<(MongoDB.Bson.ObjectId DocId, Dictionary<string, int> TermFrequencies)>();
             
             foreach (var doc in documentsToIndex)
             {
               try
               {
                 // For now, we'll create a simple term frequency dictionary
                 // In a real implementation, you'd want to re-process the document
                 var terms = new Dictionary<string, int>();
                 
                 // Add keywords as terms
                 if (doc.Keywords != null)
                 {
                   foreach (var keyword in doc.Keywords)
                   {
                     var normalizedKeyword = keyword.ToLowerInvariant();
                     if (terms.ContainsKey(normalizedKeyword))
                       terms[normalizedKeyword]++;
                     else
                       terms[normalizedKeyword] = 1;
                   }
                 }
                 
                 // Add title words as terms
                 if (!string.IsNullOrEmpty(doc.Title))
                 {
                   var titleWords = doc.Title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                   foreach (var word in titleWords)
                   {
                     var normalizedWord = word.ToLowerInvariant();
                     if (terms.ContainsKey(normalizedWord))
                       terms[normalizedWord]++;
                     else
                       terms[normalizedWord] = 1;
                   }
                 }

                 if (terms.Count > 0)
                 {
                   docTermsBatch.Add((doc.Id, terms));
                 }
               }
               catch (Exception ex)
               {
                 Console.WriteLine($"Failed to process document {doc.Id}: {ex.Message}");
               }
             }

             if (docTermsBatch.Count > 0)
             {
               // Use the batch indexing method
               await indexer.BatchUpdateGroupedInvertedIndexAsync(docTermsBatch);
             }

             return Results.Ok(new {
               message = "Forced indexing completed",
               totalDocuments = documentsToIndex.Count,
               indexedCount = docTermsBatch.Count
             });
           }
           catch (Exception e)
           {
             return Results.Problem($"Force index error: {e.Message}");
           }
         }
       )
       .Produces<object>(StatusCodes.Status200OK)
       .Produces(StatusCodes.Status500InternalServerError)
       .WithName("ForceIndex")
       .WithOpenApi(operation =>
         new(operation)
         {
           Summary = "Force indexing for testing",
           Description = "Temporarily forces indexing of all unindexed documents",
         }
       );


    return group;
  }
}

// Response models for the document list endpoint
public record class DocumentListResponse
{
  public required List<DocumentResponseDto> Documents { get; set; }
  public required PaginationInfo Pagination { get; set; }
}

public record class PaginationInfo
{
  public required int CurrentPage { get; set; }
  public required int PageSize { get; set; }
  public required int TotalCount { get; set; }
  public required int TotalPages { get; set; }
  public required bool HasNextPage { get; set; }
  public required bool HasPreviousPage { get; set; }
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
