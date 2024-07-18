using CloudinaryDotNet.Actions;

namespace API.Interfaces;

public interface IPhoteService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}
