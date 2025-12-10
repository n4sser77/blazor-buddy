using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Repositories.Interfaces
{
    public interface ICanvasRepo
    {
        Task<Canvas> CreateCanvasAsync(string name, string canvasData, UserProfile owner);
        Task<Canvas?> GetCanvasByIdAsync(Guid id);
        Task<List<Canvas>> GetCanvasesForUserAsync(string userId);
        Task<bool> UpdateCanvasAsync(Guid id, string name, string canvasData);
        Task<bool> DeleteCanvasAsync(Guid id);
        Task<bool> AddUserToCanvasAsync(Guid canvasId, UserProfile user);
    }
}
