using BlazorBuddy.Core.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface IImageService
    {
        Task<Image> CreateImageAsync(byte[] imageData, string fileName, string contentType, string userId, Guid noteId);
        Task<Image?> GetImageByIdAsync(Guid imageId);
        Task<List<Image>> GetImagesForNoteAsync(Guid noteId);
        Task<bool> DeleteImageAsync(Guid imageId, string userId);
    }
}
