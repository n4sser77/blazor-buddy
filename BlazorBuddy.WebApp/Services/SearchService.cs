using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class SearchService : ISearchService
    {
        private readonly IStudyPageRepo _studyPageRepo;
        private readonly string _userId;

        public SearchService(IStudyPageRepo studyPageRepo, string userId)
        {
            _studyPageRepo = studyPageRepo;
            _userId = userId;
        }

        public async Task<List<StudyPage>> SearchStudyPagesAsync(string query)
        {
            
            var allPages = await _studyPageRepo.GetUserStudyPagesAsync(_userId);

            if (string.IsNullOrWhiteSpace(query))
                return allPages;

            return allPages
                .Where(p => p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            p.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<NoteDocument>> SearchNotesAsync(string query)
        {
            var allPages = await _studyPageRepo.GetUserStudyPagesAsync(_userId);
            var allNotes = allPages.SelectMany(p => p.Notes).ToList();

            if (string.IsNullOrWhiteSpace(query))
                return allNotes;

            return allNotes
                .Where(n => n.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            n.Content.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
