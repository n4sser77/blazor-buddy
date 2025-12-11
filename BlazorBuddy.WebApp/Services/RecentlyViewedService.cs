using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BlazorBuddy.WebApp.Services
{
    public class RecentlyViewedService : IRecentlyViewedService
    {
        private readonly List<NoteDocument> _recentNotes = new();

        public void Add(NoteDocument note)
        {
            
            _recentNotes.RemoveAll(n => n.Id == note.Id);

            
            note.VisitedAt = DateTime.Now;

            
            _recentNotes.Insert(0, note);

            
            if (_recentNotes.Count > 10)
                _recentNotes.RemoveAt(_recentNotes.Count - 1);
        }

        public IEnumerable<NoteDocument> GetRecent() => _recentNotes;

        public void Clear() => _recentNotes.Clear();
    }
}