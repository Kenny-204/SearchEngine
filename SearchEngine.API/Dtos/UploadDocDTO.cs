using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SearchEngine.Dtos;

public record class UploadDocDto
{
  [Required(ErrorMessage = "A file must be provided.")]
  [DataType(DataType.Upload)]
  [FromForm]
  public required IFormFile File { get; set; }

  [StringLength(255, ErrorMessage = "File name must not exceed 255 characters.")]
  [FromForm]
  public string? FileName { get; set; }

  [FromForm]
  public List<string>? Tags { get; set; }
};

public record class UploadDocDtoRes
{
  public required Uri Url { get; set; }

  public required string PublicId { get; set; }
};
