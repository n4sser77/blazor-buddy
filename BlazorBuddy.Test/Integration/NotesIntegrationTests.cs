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

        //GetNotesForStudyPageAsync Tests

        [Fact]
        public async Task GetNotesForStudyPageAsync_ShouldReturnAllNotes_WhenStudyPageExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-2", DisplayName = "Test User 2" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Another Study Page", "Description", testUser);
            await _noteRepo.CreateNoteAsync("Note 1", "Content 1", testUser, studyPage.Id);
            await _noteRepo.CreateNoteAsync("Note 2", "Content 2", testUser, studyPage.Id);

            // Act
            var notes = await _noteRepo.GetNotesForStudyPageAsync(studyPage.Id);

            // Assert
            Assert.NotNull(notes);
            Assert.Equal(2, notes.Count);
            Assert.Contains(notes, n => n.Title == "Note 1");
            Assert.Contains(notes, n => n.Title == "Note 2");

        }

        [Fact]
        public async Task GetNotesForStudyPageAsync_ShouldReturnEmptyList_WhenStudyPageDoesNotExist()
        {
            // Arrange 
            var notes = await _noteRepo.GetNotesForStudyPageAsync(Guid.NewGuid());

            // Assert
            Assert.NotNull(notes);
            Assert.Empty(notes);
        }

        [Fact]
        public async Task GetNotesForStudyPageAsync_ShouldIncludeTagsAndLinks()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-notes-2", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);
            var note = await _noteRepo.CreateNoteAsync("Note with extras", "Content", testUser, studyPage.Id);

            var tag = new Tag { Title = "TestTag", Color = "#FF0000" };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            await _noteRepo.AddTagToNoteAsync(note.Id, tag.Id);
            await _noteRepo.AddLinkToNoteAsync(note.Id, "Test Link", "https://example.com");

            // Act
            var notes = await _noteRepo.GetNotesForStudyPageAsync(studyPage.Id);

            // Assert
            Assert.Single(notes);
            Assert.NotEmpty(notes[0].Tags);
            Assert.NotEmpty(notes[0].Links);
        }

        // GetNoteByIdAsync Tests

        [Fact]
        public async Task GetNoteByIdAsync_ShouldReturnNote_WhenNoteExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-4", DisplayName = "Test User 4" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();
            
            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Study Page for GetNoteById", "Description", testUser);
            var createdNote = await _noteRepo.CreateNoteAsync("Test Note", "Test Content", testUser, studyPage.Id);

            // Act
            var note = await _noteRepo.GetNoteByIdAsync(createdNote.Id);

            // Assert
            Assert.NotNull(note);
            Assert.Equal("Test Note", note.Title);
            Assert.Equal("Test Content", note.Content);


        }

        [Fact]
        public async Task GetNoteByIdAsync_ShouldReturnNull_WhenNoteDoesNotExist()
        {
            // Act 
            var note = await _noteRepo.GetNoteByIdAsync(Guid.NewGuid());

            // Assert 
            Assert.Null(note);
        }

        [Fact]
        public async Task GetNoteByIdAsync_ShouldIncludeTagsLinksAndUsers()
        {
            // Arrange
            var testUser = new UserProfile { Id = "Test-User-Notes-4", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);
            var note = await _noteRepo.CreateNoteAsync("Note", "Content", testUser, studyPage.Id);

            // Act
            var retrievedNote = await _noteRepo.GetNoteByIdAsync(note.Id);

            // Assert
            Assert.NotNull(retrievedNote);
            Assert.NotNull(retrievedNote.Tags);
            Assert.NotNull(retrievedNote.Links);
            Assert.NotNull(retrievedNote.Users);


        }

        // CreateNoteAsync Tests

        [Fact]
        public async Task CreateNoteAsync_ShouldCreateNote_WithValidData()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-notes-5", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();


            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);

            // Act 
            var note = await _noteRepo.CreateNoteAsync("New Note", "New Content", testUser, studyPage.Id);

            // Assert
            Assert.NotNull(note);
            Assert.Equal("New Note", note.Title);
            Assert.Equal("New Content", note.Content);
            Assert.Equal(testUser.Id, note.Owner.Id);

            var savedNote = await _context.NoteDocuments.FindAsync(note.Id);
            Assert.NotNull(savedNote);
        }

        // UpdateNoteAsync Tests

        [Fact]
        public async Task UpdateNoteAsync_ShouldUpdateNote_WhenNoteExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-notes-7", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);
            var note = await _noteRepo.CreateNoteAsync("Original Title", "Original Content", testUser, studyPage.Id);

            // Act
            var result = await _noteRepo.UpdateNoteAsync(note.Id, "Updated Title", "Updated Content");

            // Assert 
            Assert.True(result);
            var updatedNote = await _context.NoteDocuments.FindAsync(note.Id);
            Assert.Equal("Updated Title", updatedNote.Title);
            Assert.Equal("Updated Content", updatedNote.Content);
        }

        [Fact]
        public async Task UpdateNoteAsync_ShoudReturnFasle_WhenNoteDoesNotExist()
        {
            var result = await _noteRepo.UpdateNoteAsync(Guid.NewGuid(), "Title", "Content");

            Assert.False(result);
        }

        // DeleteNoteAsync Tests

        [Fact]
        public async Task DeleteNoteAsync_ShouldDeleteNote_WhenUserIsOwner()
        {

            // Arrange
            var testUser = new UserProfile { Id = "test-user-notes-8", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);
            var note = await _noteRepo.CreateNoteAsync("Note to Delete", "Content", testUser, studyPage.Id);

            // Act
            var result = await _noteRepo.DeleteNoteAsync(note.Id, testUser.Id);

            // Assert 
            Assert.True(result);
            var deletedNote = await _context.NoteDocuments.FindAsync(note.Id);
            Assert.Null(deletedNote);

        }
    }
}
