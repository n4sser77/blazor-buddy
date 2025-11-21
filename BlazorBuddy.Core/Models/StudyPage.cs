using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class StudyPage
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<NoteDocument> ListNote { get; set; } = [];
        public string Description { get; set; } = "";
        public List<UserProfile> ListUser { get; set; } = [];
        public string Owner { get; set; }

        public StudyPage(string owner)
        {
            Id = Guid.NewGuid();
            Owner = owner;

        }
    }
}
