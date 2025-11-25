using BlazorBuddy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Core.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public byte[] ImageData { get; set; } = [];
        public string Type { get; set; } = "";
        public required UserProfile Owner { get; set; }

    }
}
