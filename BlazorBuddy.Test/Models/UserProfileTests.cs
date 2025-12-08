using BlazorBuddy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Test.Models;

public class UserProfileTests
{
    [Fact]
    public void LeaveChat_ShouldThrowExIfUserIsNotInGroup()
    {
        // arrange
        var user = new UserProfile()
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "test user"
        };

        var chat = new ChatGroup
        {
            Id = Guid.NewGuid(),
            Title = "Test chat",
        };

        // act & assert
        Assert.Throws<ArgumentException>(
            () => user.LeaveChat(chat));
        Assert.Empty(user.ChatGroups);
        Assert.Empty(chat.Users);
    }
    [Fact]
    public void LeaveChat_ShouldRemoveUserFromChat()
    {
        // arrange
        var user = new UserProfile()
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test user",
        };

        var chat = new ChatGroup
        {
            Id = Guid.NewGuid(),
            Title = "Test chatg"
        };
        chat.AddUser(user);

        // act
        user.LeaveChat(chat);

        // assert
        Assert.Empty(user.ChatGroups);
        Assert.Empty(chat.Users);
    }

}

