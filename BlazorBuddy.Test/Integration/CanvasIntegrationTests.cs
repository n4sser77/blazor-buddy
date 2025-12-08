using BlazorBuddy.WebApp.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.Models;
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
    }
}
