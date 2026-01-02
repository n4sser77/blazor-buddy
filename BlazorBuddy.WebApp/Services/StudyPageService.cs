using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services.Interfaces;

namespace BlazorBuddy.WebApp.Services
{
    public class StudyPageService : IStudyPageService
    {
        private readonly IStudyPageRepo _studyPageRepo;
        private readonly StudyPageStateService _stateService;

        public StudyPageService(IStudyPageRepo studyPageRepo, StudyPageStateService stateService)
        {
            _studyPageRepo = studyPageRepo;
            _stateService = stateService;
        }

        public async Task<List<StudyPage>> GetUserStudyPagesAsync(string userId)
        {
            // Check cache first
            if (_stateService.IsCacheValid())
            {
                return _stateService.CachedStudyPages ?? new List<StudyPage>();
            }

            // Fetch from repository and cache
            var studyPages = await _studyPageRepo.GetUserStudyPagesAsync(userId);
            _stateService.SetStudyPages(studyPages);
            return studyPages;
        }

        public async Task<StudyPage?> GetStudyPageByIdAsync(Guid id)
        {
            return await _studyPageRepo.GetStudyPageByIdAsync(id);
        }

        public async Task<StudyPage> CreateStudyPageAsync(string title, string description, UserProfile user)
        {
            var studyPage = await _studyPageRepo.CreateStudyPageAsync(title, description, user);
            
            // Invalidate cache when creating new study page
            _stateService.InvalidateCache();
            
            return studyPage;
        }

        public async Task<bool> UpdateStudyPageAsync(Guid id, string title, string description)
        {
            var result = await _studyPageRepo.UpdateStudyPageAsync(id, title, description);
            
            // Invalidate cache when updating
            _stateService.InvalidateCache();
            
            return result;
        }

        public async Task<bool> DeleteStudyPageAsync(Guid id, string userId)
        {
            var result = await _studyPageRepo.DeleteStudyPageAsync(id, userId);
            
            // Invalidate cache when deleting
            _stateService.InvalidateCache();
            
            return result;
        }
    }
}
