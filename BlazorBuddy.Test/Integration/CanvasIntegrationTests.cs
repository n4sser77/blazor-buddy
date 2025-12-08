using BlazorBuddy.WebApp.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.Core.Models;
using Xunit;


namespace BlazorBuddy.Test.Integration
{
    public class CanvasIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {

        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;
        private readonly ICanvasRepo _canvasRepo;
        private readonly INoteRepo _noteRepo;
        private readonly IStudyPageRepo _studyPageRepo;

        public CanvasIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _canvasRepo = _scope.ServiceProvider.GetRequiredService<ICanvasRepo>();
            _noteRepo = _scope.ServiceProvider.GetRequiredService<INoteRepo>();
            _studyPageRepo = _scope.ServiceProvider.GetRequiredService<IStudyPageRepo>();
        }
        public void Dispose()
        {
            _scope?.Dispose();

        }
        [Fact]
        public async Task CreateCanvas_ShouldPersistToDatabase()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-canvas", DisplayName = "Erik" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note", "Content", testUser, studyPage.Id);

            // Act
            var canvas = await _canvasRepo.CreateCanvasAsync("Test Canvas", testUser, note.Id);

            // Assert
            var savedCanvas = await _context.Canvases.FindAsync(canvas.Id);
            Assert.NotNull(savedCanvas);
            Assert.Equal("Test Canvas", savedCanvas.Title);
            Assert.Equal(testUser.Id, savedCanvas.Owner.Id);
        }
        [Fact]
        public async Task GetCanvasesForNoteAsync_ShouldReturnAllCanvases_WhenNoteExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-canvas-2", DisplayName = "Anna" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();
            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page 2", "Description 2", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note 2", "Content 2", testUser, studyPage.Id);
            var canvas1 = await _canvasRepo.CreateCanvasAsync("Canvas 1", testUser, note.Id);
            var canvas2 = await _canvasRepo.CreateCanvasAsync("Canvas 2", testUser, note.Id);
            // Act
            var canvases = await _canvasRepo.GetCanvasesForNoteAsync(note.Id);
            // Assert
            Assert.NotNull(canvases);
            Assert.Equal(2, canvases.Count);
            Assert.Contains(canvases, c => c.Title == "Canvas 1");
            Assert.Contains(canvases, c => c.Title == "Canvas 2");
        }
        [Fact]
        public async Task GetCanvasesForNoteAsync_ShouldReturnEmptyList_WhenNoteDoesNotExist()
        {
            // Arrange
            var nonExistentNoteId = Guid.NewGuid();
            // Act
            var canvases = await _canvasRepo.GetCanvasesForNoteAsync(nonExistentNoteId);
            // Assert
            Assert.NotNull(canvases);
            Assert.Empty(canvases);
        }
        [Fact]
        public async Task DeleteCanvasAsync_ShouldRemoveCanvasFromDatabase()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-canvas-3", DisplayName = "Mike" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();
            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page 3", "Description 3", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note 3", "Content 3", testUser, studyPage.Id);
            var canvas = await _canvasRepo.CreateCanvasAsync("Canvas to Delete", testUser, note.Id);
            // Act
            var deleteResult = await _canvasRepo.DeleteCanvasAsync(canvas.Id);
            var deletedCanvas = await _context.Canvases.FindAsync(canvas.Id);
            // Assert
            Assert.True(deleteResult);
            Assert.Null(deletedCanvas);

        }
        [Fact]
        public async Task DeleteCanvasAsync_ShouldReturnFalse_WhenCanvasDoesNotExist()
        {
            // Arrange
            var nonExistentCanvasId = Guid.NewGuid();
            // Act
            var deleteResult = await _canvasRepo.DeleteCanvasAsync(nonExistentCanvasId);
            // Assert
            Assert.False(deleteResult);
        }
        [Fact]
        public async Task UpdateCanvasAsync_ShouldModifyCanvasInDatabase()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-canvas-4", DisplayName = "Sara" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();
            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page 4", "Description 4", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note 4", "Content 4", testUser, studyPage.Id);
            var canvas = await _canvasRepo.CreateCanvasAsync("Original Canvas Title", testUser, note.Id);
            // Act
            var updateResult = await _canvasRepo.UpdateCanvasAsync(canvas.Id, "Updated Canvas Title");
            var updatedCanvas = await _context.Canvases.FindAsync(canvas.Id);
            // Assert
            Assert.True(updateResult);
            Assert.NotNull(updatedCanvas);
            Assert.Equal("Updated Canvas Title", updatedCanvas.Title);
        }
        [Fact]
        public async Task UpdateCanvasAsync_ShouldReturnFalse_WhenCanvasDoesNotExist()
        {
            // Arrange
            var nonExistentCanvas = new Canvas
            {
                Id = Guid.NewGuid(),
                Title = "Non-Existent Canvas",
                Owner = new UserProfile { Id = "test-user-canvas-5", DisplayName = "Tom" }
            };
            // Act
            var updateResult = await _canvasRepo.UpdateCanvasAsync(nonExistentCanvas.Id, "Non-Existent Canvas");
            // Assert
            Assert.False(updateResult);
        }

    }
}
