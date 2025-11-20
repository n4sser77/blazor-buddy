using System;

namespace BlazorBuddy.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }

        public AppUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
