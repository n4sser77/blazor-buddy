using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepo _noteRepo;
        private readonly NotesStateService _stateService;

        public NoteService(INoteRepo noteRepo, NotesStateService stateService)
        {
            _noteRepo = noteRepo;
            _stateService = stateService;
        }

        public async Task<List<NoteDocument>> GetNotesForStudyPageAsync(Guid studyPageId)
        {
            // Check cache first
            if (_stateService.IsCacheValid(studyPageId))
            {
                return _stateService.GetCachedNotes(studyPageId) ?? new List<NoteDocument>();
            }

            // Fetch from repository and cache
            var notes = await _noteRepo.GetNotesForStudyPageAsync(studyPageId);
            _stateService.SetNotes(studyPageId, notes);
            return notes;
        }

        public async Task<NoteDocument?> GetNoteByIdAsync(Guid noteId)
        {
            return await _noteRepo.GetNoteByIdAsync(noteId);
        }

        public async Task<NoteDocument> CreateNoteAsync(string title, string content, UserProfile owner, Guid studyPageId)
        {
            var note = await _noteRepo.CreateNoteAsync(title, content, owner, studyPageId);
            
            // Invalidate cache when creating new note
            _stateService.InvalidateCache(studyPageId);
            
            return note;
        }

        public async Task<bool> UpdateNoteAsync(Guid noteId, string title, string content)
        {
            var result = await _noteRepo.UpdateNoteAsync(noteId, title, content);
            
            // Note: We can't invalidate cache here without knowing studyPageId
            // Pages should invalidate cache explicitly after calling this
            
            return result;
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId, string userId)
        {
            var result = await _noteRepo.DeleteNoteAsync(noteId, userId);
            
            // Note: We can't invalidate cache here without knowing studyPageId
            // Pages should invalidate cache explicitly after calling this
            
            return result;
        }

        public async Task<bool> AddLinkToNoteAsync(Guid noteId, string title, string url, string previewImage = "")
        {
            return await _noteRepo.AddLinkToNoteAsync(noteId, title, url, previewImage);
        }

        public async Task<bool> RemoveLinkFromNoteAsync(Guid noteId, Guid linkId)
        {
            return await _noteRepo.RemoveLinkFromNoteAsync(noteId, linkId);
        }

        public async Task<bool> AddTagToNoteAsync(Guid noteID, Guid tagId)
        {
            return await _noteRepo.AddTagToNoteAsync(noteID, tagId);
        }

        public async Task<bool> RemoveTagFromNoteAsync(Guid noteID, Guid tagId)
        {
            return await _noteRepo.RemoveTagFromNoteAsync(noteID, tagId);
        }

        public async Task UpdateVisitedAtAsync(Guid noteId, DateTime newDate)
        {
            await _noteRepo.UpdateVisitedAtAsync(noteId, newDate);
        }
    }
}
