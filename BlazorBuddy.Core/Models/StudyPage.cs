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
<<<<<<< HEAD
        public List<UserProfile> ListUser { get; set; } = [];
=======
        public List<AppUser> Users { get; set; } = [];
>>>>>>> main
        public string Owner { get; set; }

        public StudyPage(string owner)
        {
            Id = Guid.NewGuid();
            Owner = owner;

        }
    }
}
