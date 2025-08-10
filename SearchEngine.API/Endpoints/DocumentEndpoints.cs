using Microsoft.AspNetCore.Mvc;
using SearchEngine.Core.Types;
using SearchEngine.Dtos;
using SearchEngine.Filters;
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
        async ([FromForm] UploadDocDto uploadDTO, CloudinaryService cloudinaryService) =>
        {
          try
          {
            var file = uploadDTO.File;
            var fileName =
              (uploadDTO.FileName == string.Empty) ? file.FileName : uploadDTO.FileName;
            if (file == null)
              return Results.BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            // var parsedDoc = await ProcessDocument(stream);
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

            return Results.Ok(
              new UploadDocDtoRes()
              {
                Url = uploadResult.SecureUrl,
                PublicId = uploadResult.PublicId,
              }
            );
          }
          catch (Exception e)
          {
            Console.WriteLine(e);
            return Results.BadRequest("Something went wrong");
          }
        }
      )
      .Accepts<UploadDocDto>("multipart/form-data") // Tells Swagger this is a file upload
      .Produces<UploadDocDtoRes>(StatusCodes.Status200OK)
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
