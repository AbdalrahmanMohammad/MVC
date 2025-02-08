using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using TeddySmith.helpers;
using TeddySmith.Interfaces;

namespace TeddySmith.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloundinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloundinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloundinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloundinary.DestroyAsync(deleteParams);
            return result;
        }
        public string GetPublicIdFromUrl(string imageUrl)
        {
            var parts = imageUrl.Split('/');
            if (parts.Length < 2) return "sss";

            var filename = parts[^1]; // Get last part: "lytz5vljrlnlpj8ehphk.jpg"
            var versionIndex = Array.FindIndex(parts, p => p.StartsWith("v")); // Find "v1739019547"

            if (versionIndex > 0 && versionIndex < parts.Length - 1)
            {
                return filename.Split('.')[0]; // Remove file extension, return "lytz5vljrlnlpj8ehphk"
            }

            return "sss";
        }

    }
}
