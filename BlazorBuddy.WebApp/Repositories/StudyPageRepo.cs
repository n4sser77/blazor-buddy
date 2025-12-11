using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Repositories
{

    public class StudyPageRepo : IStudyPageRepo
    {
        private readonly ApplicationDbContext _context;

        public StudyPageRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudyPage>> GetUserStudyPagesAsync(string userId)
        {
            return await _context.StudyPages
                .Include(sp => sp.Notes)
                .Include(sp => sp.Users)
                .Where(sp => sp.Owner.Id == userId || sp.Users.Any(u => u.Id.ToString() == userId))
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

        public async Task<StudyPage> CreateStudyPageAsync(string title, string description, UserProfile user)
        {
            // Check if the user is already being tracked
            var trackedUser = _context.ChangeTracker.Entries<UserProfile>()
                .FirstOrDefault(e => e.Entity.Id == user.Id)?.Entity;

            var studyPage = new StudyPage()
            {
                Owner = trackedUser ?? user,
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
            var studyPage = await _context.StudyPages
                    .Include(s => s.Owner)
                    .FirstOrDefaultAsync(s => s.Id == id);
            if (studyPage == null || studyPage.Owner.Id != userId)
                return false;

            _context.StudyPages.Remove(studyPage);
            await _context.SaveChangesAsync();
            return true;
        }


        //add more methods below for share studyPage with another user etc.
    }
}
