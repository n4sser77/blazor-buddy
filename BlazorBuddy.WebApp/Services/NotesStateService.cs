using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services
{
    public class NotesStateService
    {
        private Dictionary<Guid, List<NoteDocument>> _cachedNotesByStudyPage = new();
        private Dictionary<Guid, DateTime> _lastFetchedByStudyPage = new();
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public event Action? OnChange;

        public List<NoteDocument>? GetCachedNotes(Guid studyPageId)
        {
            if (_cachedNotesByStudyPage.ContainsKey(studyPageId))
                return _cachedNotesByStudyPage[studyPageId];
            return null;
        }

        public bool IsCacheValid(Guid studyPageId)
        {
            return _cachedNotesByStudyPage.ContainsKey(studyPageId) &&
                   _lastFetchedByStudyPage.ContainsKey(studyPageId) &&
                   DateTime.UtcNow - _lastFetchedByStudyPage[studyPageId] < _cacheExpiration;
        }

        public void SetNotes(Guid studyPageId, List<NoteDocument> notes)
        {
            _cachedNotesByStudyPage[studyPageId] = notes;
            _lastFetchedByStudyPage[studyPageId] = DateTime.UtcNow;
            NotifyStateChanged();
        }

        public void InvalidateCache(Guid studyPageId)
        {
            _cachedNotesByStudyPage.Remove(studyPageId);
            _lastFetchedByStudyPage.Remove(studyPageId);
            NotifyStateChanged();
        }

        public void InvalidateAllCache()
        {
            _cachedNotesByStudyPage.Clear();
            _lastFetchedByStudyPage.Clear();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
