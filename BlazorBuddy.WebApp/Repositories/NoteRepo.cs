using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Repositories
{
    public class NoteRepo : INoteRepo
    {
        private readonly ApplicationDbContext _context;

        public NoteRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NoteDocument>> GetNotesForStudyPageAsync(Guid studyPageId)
        {
            var studyPage = await _context.StudyPages
                .Include(sp => sp.Notes)
                .ThenInclude(n => n.Tags)
                .Include(sp => sp.Notes)
                .ThenInclude(n => n.Links)
                .FirstOrDefaultAsync(sp => sp.Id == studyPageId);

            return studyPage?.Notes.ToList() ?? new List<NoteDocument>();
        }

        public async Task<NoteDocument?> GetNoteByIdAsync(Guid noteId)
        {
            return await _context.NoteDocuments
                .Include(n => n.Tags)
                .Include(n => n.Links)
                .Include(n => n.Users)
                .FirstOrDefaultAsync(n => n.Id == noteId);
        }

        public async Task<NoteDocument> CreateNoteAsync(string title, string content, UserProfile owner, Guid studyPageId)
        {
            var studyPage = await _context.StudyPages
                .Include(sp => sp.Notes)
                .Include(sp => sp.Owner)
                .FirstOrDefaultAsync(sp => sp.Id == studyPageId);

            if (studyPage == null)
                throw new Exception("Study page not found");

            // Use the existing owner from the study page to avoid tracking conflicts
            var existingOwner = studyPage.Owner;

            var note = new NoteDocument()
            {
                Owner = existingOwner,
                Title = title,
                Content = content
            };

            studyPage.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note;
        }
        public async Task<bool> UpdateNoteAsync(Guid noteId, string title, string content)
        {
            var note = await _context.NoteDocuments.FindAsync(noteId);
            if (note == null)
                return false;

            note.Title = title;
            note.Content = content;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNoteAsync(Guid noteId, string userId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Owner)
                .FirstOrDefaultAsync(n => n.Id == noteId);
            if (note == null || note.Owner.Id != userId)
                return false;

            _context.NoteDocuments.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddLinkToNoteAsync(Guid noteId, string title, string url, string previewImage = "")
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Links)
                .FirstOrDefaultAsync(n => n.Id == noteId);

            if (note == null)
                return false;

            var link = new Link
            {
                Title = title,
                LinkUrl = url,
                PreviewImage = previewImage
            };

            note.Links.Add(link);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveLinkFromNoteAsync(Guid noteId, Guid linkId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Links)
                .FirstOrDefaultAsync(n => n.Id == noteId);

            if (note == null)
                return false;

            var link = note.Links.FirstOrDefault(l => l.Id == linkId);
            if (link != null)
            {
                note.Links.Remove(link);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> AddTagToNoteAsync(Guid noteID, Guid tagId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.Id == noteID);
            var tag = await _context.Tags.FindAsync(tagId);
            if (note == null || tag == null)
                return false;
            if (!note.Tags.Any(t => t.Id == tagId))
            {
                note.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> RemoveTagFromNoteAsync(Guid noteID, Guid tagId)
        {
            var note = await _context.NoteDocuments
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.Id == noteID);

            if (note == null)
                return false;

            var tag = note.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag != null)
            {
                note.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task UpdateVisitedAtAsync(Guid noteId, DateTime newDate)
        {
            var note = await _context.NoteDocuments.FindAsync(noteId);
            if (note != null)
            {
                note.VisitedAt = newDate;
                await _context.SaveChangesAsync();
            }
        }
    }
}
