using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Repositories.Interfaces
{
    public interface IImageRepo
    {
        Task<Image> UploadImageAsync(byte[] imageData, string fileName, string contentType, UserProfile owner, Guid noteId);
        Task<Image?> GetImageByIdAsync(Guid imageId);
        Task<List<Image>> GetImagesForNoteAsync(Guid noteId);
        Task<bool> DeleteImageAsync(Guid imageId, string userId);
    }
}
