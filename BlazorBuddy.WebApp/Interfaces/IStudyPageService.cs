
using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services
{
    public interface IStudyPageService
    {
        Task<List<StudyPage>> GetUserStudyPagesAsync(string userId);
        Task<StudyPage?> GetStudyPageByIdAsync(Guid id);
        Task<StudyPage> CreateStudyPageAsync(string title, string description, UserProfile user);
        Task<bool> UpdateStudyPageAsync(Guid id, string title, string description);
        Task<bool> DeleteStudyPageAsync(Guid id, string userId);
    }
}