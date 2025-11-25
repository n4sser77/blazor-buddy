using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services
{
    public interface INotesPageService
    {
        Task<List<NoteDocument>> GetNotesForStudyPageAsync(Guid studyPageId);
        Task<NoteDocument?> GetNoteByIdAsync(Guid noteId);
        Task<NoteDocument> CreateNoteAsync(string title, string content, UserProfile owner, Guid studyPageId);
        Task<bool> UpdateNoteAsync(Guid noteId, string title, string content);
        Task<bool> DeleteNoteAsync(Guid noteId, string userId);
        Task<bool> AddLinkToNoteAsync(Guid noteId, string title, string url, string previewImage = "");
        Task<bool> RemoveLinkFromNoteAsync(Guid noteId, Guid linkId);
        Task<bool> AddTagToNoteAsync(Guid noteID, Guid tagId);
        Task<bool> RemoveTagFromNoteAsync(Guid noteID, Guid tagId);
    }
}