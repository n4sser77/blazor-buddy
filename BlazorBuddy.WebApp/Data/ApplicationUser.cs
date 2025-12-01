using BlazorBuddy.Models;
using Microsoft.AspNetCore.Identity;

namespace BlazorBuddy.WebApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        // Navigation property to the profile
        public virtual UserProfile UserProfile { get; set; } = new();



    }
}
