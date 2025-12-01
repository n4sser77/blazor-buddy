using Azure;
using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(a => a.UserProfile)
                .WithOne()
                .HasForeignKey<UserProfile>(p => p.Id);

            builder.Entity<UserProfile>()
                   .Property(p => p.Id)
                   .ValueGeneratedNever();

            builder.Entity<StudyPage>()
                .HasOne(s => s.Owner)
                .WithMany(s => s.StudyPages);
                
        }


        public DbSet<StudyPage> StudyPages { get; set; }
        public DbSet<NoteDocument> NoteDocuments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Canvas> Canvases { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FriendList> FriendLists { get; set; }
      
    }
}
