using System;
using System.Collections.Generic;

namespace BlazorBuddy.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public List<NoteDocument> Notes { get; set; } = [];
        public string Color { get; set; } = "";

        public Tag()
        {
            Id = Guid.NewGuid();
        }
    }
}
