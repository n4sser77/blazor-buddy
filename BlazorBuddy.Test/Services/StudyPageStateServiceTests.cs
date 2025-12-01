using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services;
using Xunit;

namespace BlazorBuddy.Test.Services;


public class StudyPageStateServiceTests
{
    [Fact]
    public void IsCacheValid_WhenCacheIsEmpty_ReturnsFalse()
    {
        // Given
        var Services = new StudyPageStateService();    
        // When
        var result = Services.IsCacheValid();
        // Then
        Assert.False(result);

    }   
    [Fact]
    public void IsCacheValid_WhenCacheIsSet_ReturnsTrue()
    {
        // Give
        var service = new StudyPageStateService();
        var studyPages = new List<StudyPage> { new StudyPage { Owner = new UserProfile { Id = "test_owner" } } };

        // When
        service.SetStudyPages (studyPages);
        var result = service.IsCacheValid();
        // Then
        Assert.True (result);
    }

}