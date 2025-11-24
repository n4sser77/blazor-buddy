using BlazorBuddy.Core.Models;
using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class NoteDocument
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public required UserProfile Owner { get; set; }
        public List<UserProfile> Users { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
        public List<Link> Links { get; set; } = [];
        public List<Image> Images { get; set; } = [];
        public bool IsVisible { get; set; } = false;


        public NoteDocument()
        {

        }

    }
}
