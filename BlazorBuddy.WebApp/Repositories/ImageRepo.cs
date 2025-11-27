using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Repositories
{
    public class ImageRepo : IImageRepo
    {
        private readonly ApplicationDbContext _context;

        public ImageRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Image> CreateImageAsync(byte[] imageData, string fileName, string contentType, UserProfile owner, Guid noteId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Id == noteId);

            var image = new Image()
            {
                Title = fileName,
                ImageData = imageData,
                Type = contentType,
                Owner = owner
            };

            note.Images.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<Image?> GetImageByIdAsync(Guid imageId)
        {
            return await _context.Images
                .Include(i => i.Owner)
                .FirstOrDefaultAsync(i => i.Id == imageId);
        }

        public async Task<List<Image>> GetImagesForNoteAsync(Guid noteId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Images)
                .FirstOrDefaultAsync(n => n.Id == noteId);

            return note?.Images ?? new List<Image>();
        }

        public async Task<bool> DeleteImageAsync(Guid imageId, string userId)
        {
            var image = await _context.Images
                .Include(i => i.Owner)
                .FirstOrDefaultAsync(i => i.Id == imageId);

            if (image == null || image.Owner.Id != userId)
                return false;

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
