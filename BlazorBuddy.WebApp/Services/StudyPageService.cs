using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Services
{
    public class StudyPageService
    {
        private readonly ApplicationDbContext _context;

        public StudyPageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudyPage>> GetUserStudyPagesAsync(string userId)
        {
            return await _context.StudyPages
                .Include(sp => sp.Notes)
                .Include(sp => sp.Users)
                .Where(sp => sp.Owner == userId || sp.Users.Any(u => u.Id.ToString() == userId))
                .OrderByDescending(sp => sp.Title)
                .ToListAsync();
        }

        public async Task<StudyPage?> GetStudyPageByIdAsync(Guid id)
        {
            return await _context.StudyPages
                .Include(sp => sp.Notes)
                .ThenInclude(sp => sp.Tags)
                .Include(sp => sp.Notes)
                .ThenInclude(sp => sp.Links)
                .Include(sp => sp.Users)
                .FirstOrDefaultAsync(sp => sp.Id == id);
        }

        public async Task<StudyPage> CreateStudyPageAsync(string title, string description, string owner)
        {
            var studyPage = new StudyPage(owner)
            {
                Title = title,
                Description = description
            };

            _context.StudyPages.Add(studyPage);
            await _context.SaveChangesAsync();
            return studyPage;
                

        }

        public async Task<bool> UpdateStudyPageAsync(Guid id, string title, string description)
        {
            var studyPage = await _context.StudyPages.FindAsync(id);
                if (studyPage == null)
                return false;

            studyPage.Title = title;
            studyPage.Description = description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudyPageAsync(Guid id, string userId)
        {
            var studyPage = await _context.StudyPages.FindAsync(id);
            if (studyPage == null || studyPage.Owner != userId)
                return false;

            _context.StudyPages.Remove(studyPage);
            await _context.SaveChangesAsync();
            return true;
        }


        //add more methods below for share studyPage with another user etc.
    }
}
