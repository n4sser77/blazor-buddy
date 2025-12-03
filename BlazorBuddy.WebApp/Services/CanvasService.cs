using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class CanvasService : ICanvasService
    {
        private readonly ICanvasRepo _canvasRepo;
        private readonly IUserRepo _userRepo;

        public CanvasService(ICanvasRepo canvasRepo, IUserRepo userRepo)
        {
            _canvasRepo = canvasRepo;
            _userRepo = userRepo;
        }

        public async Task<Canvas> CreateCanvasAsync(string name, string userId, Guid noteId)
        {
            try
            {
                var owner = await _userRepo.GetUserById(userId);
                if (owner == null)
                    throw new ArgumentException("User not found");

                return await _canvasRepo.CreateCanvasAsync(name, owner, noteId);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Canvas?> GetCanvasByIdAsync(Guid id)
        {
            try
            {
                return await _canvasRepo.GetCanvasByIdAsync(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Canvas>> GetCanvasesForNoteAsync(Guid noteId)
        {
            try
            {
                return await _canvasRepo.GetCanvasesForNoteAsync(noteId);
            }
            catch
            {
                return new List<Canvas>();
            }
        }

        public async Task<bool> UpdateCanvasAsync(Guid id, string name)
        {
            try
            {
                return await _canvasRepo.UpdateCanvasAsync(id, name);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCanvasAsync(Guid id, string userId)
        {
            try
            {
                var canvas = await _canvasRepo.GetCanvasByIdAsync(id);
                if (canvas == null || canvas.Owner.Id != userId)
                    return false;

                return await _canvasRepo.DeleteCanvasAsync(id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddUserToCanvasAsync(Guid canvasId, string userId)
        {
            try
            {
                var user = await _userRepo.GetUserById(userId);
                if (user == null)
                    return false;

                return await _canvasRepo.AddUserToCanvasAsync(canvasId, user);
            }
            catch
            {
                return false;
            }
        }
    }
}
