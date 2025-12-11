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

            // Act
            var canvas = await _canvasRepo.CreateCanvasAsync("Test Canvas", "canvas-data-123", testUser);

            // Assert
            var savedCanvas = await _context.Canvases.FindAsync(canvas.Id);
            Assert.NotNull(savedCanvas);
            Assert.Equal("Test Canvas", savedCanvas.Title);
            Assert.Equal(testUser.Id, savedCanvas.Owner.Id);
            Assert.Equal("canvas-data-123", savedCanvas.CanvasData);
        }
        [Fact]
        public async Task GetCanvasesForUserAsync_ShouldReturnAllCanvases_WhenUserHasCanvases()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-canvas-2", DisplayName = "Anna" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();
            var canvas1 = await _canvasRepo.CreateCanvasAsync("Canvas 1", "data-1", testUser);
            var canvas2 = await _canvasRepo.CreateCanvasAsync("Canvas 2", "data-2", testUser);
            // Act
            var canvases = await _canvasRepo.GetCanvasesForUserAsync(testUser.Id);
            // Assert
            Assert.NotNull(canvases);
            Assert.Equal(2, canvases.Count);
            Assert.Contains(canvases, c => c.Title == "Canvas 1");
            Assert.Contains(canvases, c => c.Title == "Canvas 2");
        }
        [Fact]
        public async Task GetCanvasesForUserAsync_ShouldReturnEmptyList_WhenUserHasNoCanvases()
        {
            // Arrange
            var nonExistentUserId = "non-existent-user";
            // Act
            var canvases = await _canvasRepo.GetCanvasesForUserAsync(nonExistentUserId);
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
            var canvas = await _canvasRepo.CreateCanvasAsync("Canvas to Delete", "data", testUser);
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
            var canvas = await _canvasRepo.CreateCanvasAsync("Original Canvas Title", "original-data", testUser);
            // Act
            var updateResult = await _canvasRepo.UpdateCanvasAsync(canvas.Id, "Updated Canvas Title", "updated-data");
            var updatedCanvas = await _context.Canvases.FindAsync(canvas.Id);
            // Assert
            Assert.True(updateResult);
            Assert.NotNull(updatedCanvas);
            Assert.Equal("Updated Canvas Title", updatedCanvas.Title);
            Assert.Equal("updated-data", updatedCanvas.CanvasData);
        }
        [Fact]
        public async Task UpdateCanvasAsync_ShouldReturnFalse_WhenCanvasDoesNotExist()
        {
            // Arrange
            var nonExistentCanvasId = Guid.NewGuid();
            // Act
            var updateResult = await _canvasRepo.UpdateCanvasAsync(nonExistentCanvasId, "Non-Existent Canvas", "data");
            // Assert
            Assert.False(updateResult);
        }

    }
}
