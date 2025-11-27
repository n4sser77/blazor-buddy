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

        public async Task<Canvas> CreateCanvasAsync(string name, UserProfile owner, Guid noteId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Canvases)
                .FirstOrDefaultAsync(n => n.Id == noteId);

            var canvas = new Canvas()
            {
                Title = name,
                Owner = owner,
                CanvasData = ""
            };

            note.Canvases.Add(canvas);
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

        public async Task<List<Canvas>> GetCanvasesForNoteAsync(Guid noteId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Canvases)
                .FirstOrDefaultAsync(n => n.Id == noteId);

            return note?.Canvases ?? new List<Canvas>();
        }

        public async Task<bool> UpdateCanvasAsync(Guid id, string name)
        {
            var canvas = await _context.Canvases.FindAsync(id);

            if (canvas == null)
                return false;

            canvas.Title = name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCanvasAsync(Guid id, string userId)
        {
            var canvas = await _context.Canvases
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (canvas == null || canvas.Owner.Id != userId)
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
