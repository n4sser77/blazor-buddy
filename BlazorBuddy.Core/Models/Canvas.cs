using BlazorBuddy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Core.Models
{
    public class Canvas
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string CanvasData { get; set; } = "";
        public List<NoteDocument> NoteDocuments { get; set; } = [];
        public List<UserProfile> Users { get; set; } = [];
        public required UserProfile Owner { get; set; }
    }
}
