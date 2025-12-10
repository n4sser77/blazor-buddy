using BlazorBuddy.Core.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface ICanvasService
    {
        Task<Canvas> CreateCanvasAsync(string name, string canvasData, string userId);
        Task<Canvas?> GetCanvasByIdAsync(Guid id);
        Task<List<Canvas>> GetCanvasesForUserAsync(string userId);
        Task<bool> UpdateCanvasAsync(Guid id, string name, string canvasData);
        Task<bool> DeleteCanvasAsync(Guid id, string userId);
        Task<bool> AddUserToCanvasAsync(Guid canvasId, string userId);
    }
}
