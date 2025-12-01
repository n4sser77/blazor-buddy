using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Test.Integration
{
    public class NotesIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;
        private readonly INoteRepo _noteRepo;
        private readonly IStudyPageRepo _studyPageRepo;

        public NotesIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _noteRepo = _scope.ServiceProvider.GetRequiredService<INoteRepo>();
            _studyPageRepo = _scope.ServiceProvider.GetRequiredService<IStudyPageRepo>();
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }

        [Fact]
        public async Task CreateNote_ShouldPersistToDatabase()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-1", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);

            // Act
            var note = await _noteRepo.CreateNoteAsync("Test Note", "This is a test note.", testUser, studyPage.Id);

            // Assert
            var savedNote = await _context.NoteDocuments.FindAsync(note.Id);
            Assert.NotNull(savedNote);
            Assert.Equal("Test Note", savedNote.Title);
            Assert.Equal("This is a test note.", savedNote.Content);
            Assert.Equal(testUser.Id, savedNote.Owner.Id);
        }
    }
}
