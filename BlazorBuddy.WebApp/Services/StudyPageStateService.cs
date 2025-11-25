using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services
{
    public class StudyPageStateService
    {
        private List<StudyPage>? _cachedStudyPages;
        private DateTime? _lastFetched;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);
        private bool _hasSeeded = false;

        public event Action? OnChange;

        public List<StudyPage>? CachedStudyPages => _cachedStudyPages;

        public bool IsCacheValid()
        {
            return _cachedStudyPages != null && 
                   _lastFetched.HasValue && 
                   DateTime.UtcNow - _lastFetched.Value < _cacheExpiration;
        }

        public void SetStudyPages(List<StudyPage> studyPages)
        {
            _cachedStudyPages = studyPages;
            _lastFetched = DateTime.UtcNow;
            NotifyStateChanged();
        }

        public void InvalidateCache()
        {
            _cachedStudyPages = null;
            _lastFetched = null;
            NotifyStateChanged();
        }

        public bool HasSeeded()
        {
            return _hasSeeded;
        }

        public void MarkAsSeeded()
        {
            _hasSeeded = true;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
