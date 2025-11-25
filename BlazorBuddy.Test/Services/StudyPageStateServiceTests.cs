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
}