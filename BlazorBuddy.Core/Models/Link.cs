using System;

namespace BlazorBuddy.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string LinkUrl { get; set; } = "";
        public string PreviewImage { get; set; } = "";

        public Link()
        {
            Id = Guid.NewGuid();
        }
    }
}
