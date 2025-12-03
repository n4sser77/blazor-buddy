using BlazorBuddy.Core.Models;

namespace BlazorBuddy.WebApp.Services.Interfaces
{
    public interface ICanvasService
    {
        Task<Canvas> CreateCanvasAsync(string name, string userId, Guid noteId);
        Task<Canvas?> GetCanvasByIdAsync(Guid id);
        Task<List<Canvas>> GetCanvasesForNoteAsync(Guid noteId);
        Task<bool> UpdateCanvasAsync(Guid id, string name);
        Task<bool> DeleteCanvasAsync(Guid id, string userId);
        Task<bool> AddUserToCanvasAsync(Guid canvasId, string userId);
    }
}
