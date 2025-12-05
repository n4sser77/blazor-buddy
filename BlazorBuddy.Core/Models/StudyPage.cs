using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class StudyPage
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<NoteDocument> Notes { get; set; } = [];
        public string Description { get; set; } = "";
        public List<UserProfile> Users { get; set; } = [];
        public required UserProfile Owner { get; set; }

        public DateTime VisitedAt { get; set; } = DateTime.UtcNow;

        public List<NoteDocument> RecentNotes()
        {
            var sortedNotes =

            Notes
                 .OrderByDescending(n => n.VisitedAt)
                 .Take(5)
                 .ToList();

            return sortedNotes;

        }



    }


}
