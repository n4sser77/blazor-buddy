using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Repositories
{
    public class CanvasRepo : ICanvasRepo
    {
        private readonly ApplicationDbContext _context;

        public CanvasRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Canvas> CreateCanvasAsync(string name, string canvasData, UserProfile owner)
        {
            var canvas = new Canvas()
            {
                Title = name,
                Owner = owner,
                CanvasData = canvasData
            };

            _context.Canvases.Add(canvas);
            await _context.SaveChangesAsync();

            return canvas;
        }

        public async Task<Canvas?> GetCanvasByIdAsync(Guid id)
        {
            return await _context.Canvases
                .Include(c => c.Owner)
                .Include(c => c.Users)
                .Include(c => c.NoteDocuments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Canvas>> GetCanvasesForUserAsync(string userId)
        {
            return await _context.Canvases
                .Where(c => c.Owner.Id == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateCanvasAsync(Guid id, string name, string canvasData)
        {
            var canvas = await _context.Canvases.FindAsync(id);

            if (canvas == null)
                return false;

            canvas.Title = name;
            canvas.CanvasData = canvasData;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCanvasAsync(Guid id)
        {
            var canvas = await _context.Canvases.FindAsync(id);

            if (canvas == null)
                return false;

            _context.Canvases.Remove(canvas);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserToCanvasAsync(Guid canvasId, UserProfile user)
        {
            var canvas = await _context.Canvases
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == canvasId);

            if (canvas == null)
                return false;

            canvas.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
