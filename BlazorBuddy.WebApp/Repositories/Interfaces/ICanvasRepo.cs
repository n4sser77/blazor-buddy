using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Repositories.Interfaces
{
    public interface ICanvasRepo
    {
        Task<Canvas> CreateCanvasAsync(string name, UserProfile owner, Guid noteId);
        Task<Canvas?> GetCanvasByIdAsync(Guid id);
        Task<List<Canvas>> GetCanvasesForNoteAsync(Guid noteId);
        Task<bool> UpdateCanvasAsync(Guid id, string name);
        Task<bool> DeleteCanvasAsync(Guid id);
        Task<bool> AddUserToCanvasAsync(Guid canvasId, UserProfile user);
    }
}
