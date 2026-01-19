using _1_Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace _3_Infrastructure.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var cloudName = config["Cloudinary:CloudName"];
        var apiKey = config["Cloudinary:ApiKey"];
        var apiSecret = config["Cloudinary:ApiSecret"];

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<(string Url, string PublicId)> UploadImageAsync(Stream fileStream, string fileName)
    {
        var upload = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream),
            Folder = "gallery",
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(upload);

        if (result.Error != null)
            throw new Exception(result.Error.Message);

        return (result.SecureUrl.ToString(), result.PublicId);
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var delete = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(delete);

        if (result.Error != null)
            throw new Exception(result.Error.Message);
    }
}