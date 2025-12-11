using BlazorBuddy.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface IHomePageService
{
    Task<List<StudyPage>> GetUserStudyPagesAsync(string userId);

    Task<List<NoteDocument>> GetUserNotesAsync(List<StudyPage> studyPages);

    Task<List<NoteDocument>> SearchAsync(
        string query,
        List<StudyPage> studyPages,
        List<NoteDocument> allNotes,
        CancellationToken token);

    List<NoteDocument> LoadRecentlyViewed(StudyPage studyPage, List<NoteDocument> recentlyViewedNotes);

    string GetContentPreview(string content);
    string GetTimeAgo(DateTime viewedAt);

}
