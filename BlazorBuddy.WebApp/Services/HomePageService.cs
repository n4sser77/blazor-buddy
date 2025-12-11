
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories.Interfaces;

public class HomePageService : IHomePageService
{
    private readonly IStudyPageRepo _studyPageRepo;
    private readonly INoteRepo _noteRepo;

    public HomePageService(IStudyPageRepo studyPageRepo, INoteRepo noteRepo)
    {
        _studyPageRepo = studyPageRepo;
        _noteRepo = noteRepo;
    }

    public async Task<List<StudyPage>> GetUserStudyPagesAsync(string userId)
    {
        return await _studyPageRepo.GetUserStudyPagesAsync(userId);
    }

    public async Task<List<NoteDocument>> GetUserNotesAsync(List<StudyPage> studyPages)
    {
        var notes = new List<NoteDocument>();
        foreach (var sp in studyPages)
        {
            var spNotes = await _noteRepo.GetNotesForStudyPageAsync(sp.Id);
            notes.AddRange(spNotes);
        }
        return notes;
    }

    public async Task<List<NoteDocument>> SearchAsync(string query, List<StudyPage> allStudyPages, List<NoteDocument> allNotes, CancellationToken token)
    {
        await Task.Delay(100, token);

        var resultNotes = allNotes
            .Where(n => n.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                     || n.Content.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return resultNotes;
    }

    public List<NoteDocument> LoadRecentlyViewed(StudyPage studyPage, List<NoteDocument> recentlyViewedNotes)
    {
        recentlyViewedNotes.AddRange(studyPage.RecentNotes());
        return recentlyViewedNotes;
    }

    public string GetContentPreview(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return "No content yet...";

        return content.Length > 100 ? content.Substring(0, 100) + "..." : content;
    }

    public string GetTimeAgo(DateTime viewedAt)
    {
        var timeSpan = DateTime.Now - viewedAt;

        if (timeSpan.TotalMinutes < 1)
            return "Just now";
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes}m ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours}h ago";
        if (timeSpan.TotalDays < 7)
            return $"{(int)timeSpan.TotalDays}d ago";

        return viewedAt.ToString("MMM dd");
    }
}
