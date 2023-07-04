using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using ShoeShop.Helper;
using ShoeShop.Interfaces;

namespace ShoeShop.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;

        public PhotoService(IOptions<CloudinarySetting> config)
        {
            var acc = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
               );
            cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParam = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Height(400).Width(500).Crop("fill").Gravity(Gravity.Center)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParam);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhoto(string id)
        {
            var deleteParam = new DeletionParams(id);
            var result = await cloudinary.DestroyAsync(deleteParam);
            return result;
        }
    }
}
