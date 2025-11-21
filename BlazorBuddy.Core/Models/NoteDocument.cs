using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class NoteDocument
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Owner { get; set; }
        public List<AppUser> ListUser { get; set; } = [];
        public List<Tag> ListTag { get; set; } = [];
        public List<Link> ListLink { get; set; } = [];
        public bool IsVisible { get; set; } = false;

        public NoteDocument(string owner)
        {
            Id = Guid.NewGuid();
            Owner = owner;
        }
    }
}
