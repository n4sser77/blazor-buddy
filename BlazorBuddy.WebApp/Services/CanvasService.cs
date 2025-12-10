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

        public async Task<Canvas> CreateCanvasAsync(string name, string canvasData, string userId)
        {
            var owner = await _userRepo.GetUserById(userId);
            if (owner == null)
                throw new ArgumentException("User not found");

            return await _canvasRepo.CreateCanvasAsync(name, canvasData, owner);
        }

        public async Task<Canvas?> GetCanvasByIdAsync(Guid id)
        {
            return await _canvasRepo.GetCanvasByIdAsync(id);
        }

        public async Task<List<Canvas>> GetCanvasesForUserAsync(string userId)
        {
            return await _canvasRepo.GetCanvasesForUserAsync(userId);
        }

        public async Task<bool> UpdateCanvasAsync(Guid id, string name, string canvasData)
        {
            return await _canvasRepo.UpdateCanvasAsync(id, name, canvasData);
        }

        public async Task<bool> DeleteCanvasAsync(Guid id, string userId)
        {
            var canvas = await _canvasRepo.GetCanvasByIdAsync(id);
            if (canvas == null || canvas.Owner.Id != userId)
                return false;

            return await _canvasRepo.DeleteCanvasAsync(id);
        }

        public async Task<bool> AddUserToCanvasAsync(Guid canvasId, string userId)
        {
            var user = await _userRepo.GetUserById(userId);
            if (user == null)
                return false;

            return await _canvasRepo.AddUserToCanvasAsync(canvasId, user);
        }
    }
}
