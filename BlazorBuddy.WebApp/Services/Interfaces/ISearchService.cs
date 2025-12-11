using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services
{
    public interface ISearchService
    {
        Task<List<StudyPage>> SearchStudyPagesAsync(string query);
        Task<List<NoteDocument>> SearchNotesAsync(string query);
    }
}

