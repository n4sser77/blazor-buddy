using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Test.Integration
{
    public class StudyPageIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly ApplicationDbContext _context;
        private readonly INoteRepo _noteRepo;
        private readonly IStudyPageRepo _studyPageRepo;

        public StudyPageIntegrationTests(CustomWebApplicationFactory<Program> factory)
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


        // GetUserStudyPagesAsync Tests

        [Fact]
        public async Task GetUserStudyPagesAsync_ShouldReturnOwnedStudyPages()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-1", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            await _studyPageRepo.CreateStudyPageAsync("Study page 1", "Description 1", testUser);
            await _studyPageRepo.CreateStudyPageAsync("Study page 2", "Description 2", testUser);

            // Act
            var studyPages = await _studyPageRepo.GetUserStudyPagesAsync(testUser.Id);

            // Assert
            Assert.NotNull(studyPages);
            Assert.Equal(2, studyPages.Count);
            Assert.All(studyPages, sp => Assert.Equal(testUser.Id, sp.Owner.Id));
        }

        [Fact]
        public async Task GetUserStudyPagesAsync_ShouldReturnEmptyList_WhenUserHasNoStudyPages()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-2", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            // Act
            var studyPages = await _studyPageRepo.GetUserStudyPagesAsync(testUser.Id);

            // Assert
            Assert.NotNull(studyPages);
            Assert.Empty(studyPages);
        }

        // GetStudyPageByIdAsync Tests

        [Fact]
        public async Task GetStudyPageByIdAsync_ShouldReturnStudyPage_WhenItExists()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-sp-5", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var createdStudyPage = await _studyPageRepo.CreateStudyPageAsync("Test Study Page", "Description", testUser);

            // Act
            var studyPage = await _studyPageRepo.GetStudyPageByIdAsync(createdStudyPage.Id);

            // Assert
            Assert.NotNull(studyPage);
            Assert.Equal(createdStudyPage.Id, studyPage.Id);
            Assert.Equal("Test Study Page", studyPage.Title);
            Assert.Equal("Description", studyPage.Description);

        }

        [Fact]
        public async Task GetStudyPageByIdAsync_ShouldReturnNull_WhenDoesNotExist()
        {
            // Act
            var studyPage = await _studyPageRepo.GetStudyPageByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(studyPage);
        }

        // CreateStudyPageAsync Tests

        [Fact]
        public async Task CreateStudyPageAsync_ShouldCreateStudyPage_WithValidData()
        {
            var testUser = new UserProfile { Id = "test-user-sp-6", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            // Act
            var studyPage = await _studyPageRepo.CreateStudyPageAsync("New Study Page", "New Description", testUser);

            // Assert
            Assert.NotNull(studyPage);
            Assert.Equal("New Study Page", studyPage.Title);
            Assert.Equal("New Description", studyPage.Description);
            Assert.Equal(testUser.Id, studyPage.Owner.Id);

        }


        // UpdateStudyPagesAsync Test

        [Fact]
        public async Task UpdateStudyPagesAsync_ShouldUpdateStudyPage_WhenExists()
        {
            // Arrange

            var testUser = new UserProfile { Id = "test-user-sp-7", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Original Title", "Original Description", testUser);

            // Act
            var result = await _studyPageRepo.UpdateStudyPageAsync(studyPage.Id, "Updated Title", "Updated Description");

            // Assert
            Assert.True(result);
            var updatedStudyPage = await _context.StudyPages.FindAsync(studyPage.Id);
            Assert.Equal("Updated Title", updatedStudyPage.Title);
            Assert.Equal("Updated Description", updatedStudyPage.Description);
        }

        [Fact]
        public async Task updateStudyPageAsync_ShouldReturnFalse_WhenDoesNotExist()
        {

            // Act
            var result = await _studyPageRepo.UpdateStudyPageAsync(Guid.NewGuid(), "Title", "Description");

            // Assert
            Assert.False(result);
        }

        // DeleteStudyPageAsync Tests

        [Fact]
        public async Task DeleteStudyPageAsync_ShouldDeleteStudyPage_WhenUserIsOwner()
        {
            // Arrange
            var testUser = new UserProfile { Id = "test-user-sp-8", DisplayName = "Test User" };
            _context.Add(testUser);
            await _context.SaveChangesAsync();

            var studyPage = await _studyPageRepo.CreateStudyPageAsync("Study Page To Delete", "Description", testUser);

            // Act
            var result = await _studyPageRepo.DeleteStudyPageAsync(studyPage.Id, testUser.Id);

            // Assert 
            Assert.True(result);
            var deletedStudyPage = await _context.StudyPages.FindAsync(studyPage.Id);
            Assert.Null(deletedStudyPage);
        }

        [Fact]
        public async Task DeleteStudyPageAsync_ShouldReturnFalse_WhenStudyPageDoesNotExist()
        {
            // Act
            var result = await _studyPageRepo.DeleteStudyPageAsync(Guid.NewGuid(), "any-user-id");

            // Assert
            Assert.False(result);
        }


    }
}
