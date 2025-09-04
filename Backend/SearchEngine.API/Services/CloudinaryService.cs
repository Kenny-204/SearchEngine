using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace SearchEngine.Services;

public class CloudinarySettings
{
  public required string CloudName { get; set; }
  public required string ApiKey { get; set; }
  public required string ApiSecret { get; set; }
}

public class CloudinaryService
{
  private readonly Cloudinary _cloudinary;
  private readonly CloudinarySettings _cloudinarySettings;

  public CloudinaryService(IOptions<CloudinarySettings> options)
  {
    _cloudinarySettings = options.Value;

    if (
      string.IsNullOrWhiteSpace(_cloudinarySettings.CloudName)
      || string.IsNullOrWhiteSpace(_cloudinarySettings.ApiKey)
      || string.IsNullOrWhiteSpace(_cloudinarySettings.ApiSecret)
    )
    {
      throw new ArgumentException("Cloudinary settings are missing or invalid.");
    }
    Account account = new Account(
      _cloudinarySettings.CloudName,
      _cloudinarySettings.ApiKey,
      _cloudinarySettings.ApiSecret
    );
    _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
  }

  public async Task<ImageUploadResult> UploadImageAsync(
    Stream fileStream,
    string fileName,
    string? publicId,
    Transformation? transformation = null
  )
  {
    fileStream.Position = 0;

    var uploadParam = new ImageUploadParams()
    {
      File = new FileDescription(fileName, fileStream),
      Transformation = transformation,
      PublicId = publicId,
    };
    return await _cloudinary.UploadAsync(uploadParam);
  }

  public async Task<RawUploadResult> UploadFileAsync(
    Stream fileStream,
    string fileName,
    string? publicId,
    bool? useFileName = false
  )
  {
    fileStream.Position = 0;

    var uploadParams = new RawUploadParams
    {
      File = new FileDescription(fileName, fileStream),
      PublicId = publicId,
      UseFilename = useFileName,
      UniqueFilename = useFileName,
      AccessMode = "public",
    };
    return await _cloudinary.UploadAsync(uploadParams);
  }

  public async Task<DeletionResult> DeleteFileAsync(string publicId)
  {
    var deleteParams = new DeletionParams(publicId)
    {
      ResourceType = ResourceType.Raw
    };

    return await _cloudinary.DestroyAsync(deleteParams);
  }
}
