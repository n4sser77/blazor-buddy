using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepo _imageRepo;
        private readonly IUserRepo _userRepo;

        public ImageService(IImageRepo imageRepo, IUserRepo userRepo)
        {
            _imageRepo = imageRepo;
            _userRepo = userRepo;
        }

        public async Task<Image> CreateImageAsync(byte[] imageData, string fileName, string contentType, string userId, Guid noteId)
        {
            var owner = await _userRepo.GetUserById(userId);
            if (owner == null)
                throw new ArgumentException("User not found");

            return await _imageRepo.CreateImageAsync(imageData, fileName, contentType, owner, noteId);
        }

        public async Task<Image?> GetImageByIdAsync(Guid imageId)
        {
            return await _imageRepo.GetImageByIdAsync(imageId);
        }

        public async Task<List<Image>> GetImagesForNoteAsync(Guid noteId)
        {
            return await _imageRepo.GetImagesForNoteAsync(noteId);
        }

        public async Task<bool> DeleteImageAsync(Guid imageId, string userId)
        {
            var image = await _imageRepo.GetImageByIdAsync(imageId);
            if (image == null || image.Owner.Id != userId)
                return false;

            return await _imageRepo.DeleteImageAsync(imageId);
        }
    }
}
