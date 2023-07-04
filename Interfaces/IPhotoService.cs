using CloudinaryDotNet.Actions;

namespace ShoeShop.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhoto(IFormFile file);
        Task<DeletionResult> DeletePhoto(string id);
    }
}
