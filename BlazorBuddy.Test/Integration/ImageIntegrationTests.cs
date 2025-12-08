using BlazorBuddy.Core.Models;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;




namespace BlazorBuddy.Test.Integration
{
    public class ImageIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;
        private readonly IImageRepo _imageRepo;
        private readonly INoteRepo _noteRepo;
        private readonly IStudyPageRepo _studyPageRepo;

        public ImageIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = _factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _imageRepo = _scope.ServiceProvider.GetRequiredService<IImageRepo>();
            _noteRepo = _scope.ServiceProvider.GetRequiredService<INoteRepo>();
            _studyPageRepo = _scope.ServiceProvider.GetRequiredService<IStudyPageRepo>();

        }
        public void Dispose()
        {
            _scope?.Dispose();
        }
        [Fact]
        public async Task CreateImage_ShouldPersistToDatabase()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-image", DisplayName = "Fiona" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note", "Content", testUser, studyPage.Id);

            // Act
            var image = await _imageRepo.CreateImageAsync(new byte[] { 0x1, 0x2, 0x3 }, "image.png", "image/png", testUser, note.Id);

            // Assert
            var savedImage = await _context.Images.FindAsync(image.Id);
            Assert.NotNull(savedImage);
            Assert.Equal("image.png", savedImage.Title);
            Assert.Equal(testUser.Id, savedImage.Owner.Id);
        }
        [Fact]
        public async Task GetImagesForNote_ShouldReturnAllImages_WhenNoteExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-image-2", DisplayName = "George" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page 2", "Description 2", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note 2", "Content 2", testUser, studyPage.Id);

            var image1 = await _imageRepo.CreateImageAsync(new byte[] { 0x1, 0x2 }, "image1.png", "image/png", testUser, note.Id);
            var image2 = await _imageRepo.CreateImageAsync(new byte[] { 0x3, 0x4 }, "image2.png", "image/png", testUser, note.Id);

            // Act
            var images = await _imageRepo.GetImagesForNoteAsync(note.Id);

            // Assert
            Assert.Equal(2, images.Count);
            Assert.Contains(images, img => img.Title == "image1.png");
            Assert.Contains(images, img => img.Title == "image2.png");
        }
        [Fact]
        public async Task DeleteImage_ShouldRemoveFromDatabase()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-image-3", DisplayName = "Hannah" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page 3", "Description 3", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note 3", "Content 3", testUser, studyPage.Id);

            var image = await _imageRepo.CreateImageAsync(new byte[] { 0x5, 0x6 }, "image_to_delete.png", "image/png", testUser, note.Id);

            // Act
            var deleteResult = await _imageRepo.DeleteImageAsync(image.Id);

            // Assert
            Assert.True(deleteResult);
            var deletedImage = await _context.Images.FindAsync(image.Id);
            Assert.Null(deletedImage);
        }
        [Fact]
        public async Task GetImageById_ShouldReturnImage_WhenExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-image-4", DisplayName = "Ian" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page 4", "Description 4", testUser);
            var note = await _noteRepo.CreateNoteAsync("Test Note 4", "Content 4", testUser, studyPage.Id);

            var image = await _imageRepo.CreateImageAsync(new byte[] { 0x7, 0x8 }, "test_image.png", "image/png", testUser, note.Id);

            // Act
            var fetchedImage = await _imageRepo.GetImageByIdAsync(image.Id);

            // Assert
            Assert.NotNull(fetchedImage);
            Assert.Equal(image.Id, fetchedImage.Id);
            Assert.Equal("test_image.png", fetchedImage.Title);
        }
    }
}
