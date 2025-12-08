
using BlazorBuddy.Models;

namespace BlazorBuddy.Test.Models;

public class ChatGroupTests
{
    [Fact]
    public void AddUser_ThrowsExGivenUserIsAlreadyInChat()
    {
        // arrange
        var user = new UserProfile
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test-user",
        };
        var chat_sut = new ChatGroup
        {
            Id = Guid.NewGuid(),
            Title = "Test Group",
            Users = { user }

        };

        // act & assert 
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            chat_sut.AddUser(user);
        });

        // assert
        Assert.Contains("User already in group", ex.Message);
        Assert.Single(chat_sut.Users);

    }

    [Fact]
    public void AddUser_ShouldAddUserIfTheyAreNotAMember()
    {
        // arrange
        var user = new UserProfile
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test-user"
        };

        var chat_sut = new ChatGroup()
        {
            Id = Guid.NewGuid(),
            Title = "Test Group",

        };
        // act
        chat_sut.AddUser(user);

        // assert
        Assert.Contains(user, chat_sut.Users);
        Assert.Single(chat_sut.Users);
        Assert.Single(user.ChatGroups, chat_sut);
    }

    [Fact]
    public void AddMessage_ShouldAddMessageToChat()
    {
        // arrange
        var user = new UserProfile()
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test User"
        };
        var chat_sut = new ChatGroup()
        {
            Id = Guid.NewGuid(),

        };
        chat_sut.AddUser(user);


        // act
        chat_sut.AddMessage(user, "Hello from test user");

        // assert
        Assert.Single(chat_sut.Messages,
            (m) => m.Content == "Hello from test user");
        Assert.Contains(chat_sut.Messages,
            (m) => m.FromUser.Id.Equals(user.Id));
    }

    [Fact]
    public void IsUserInGroup_ReturnsTrueIfUserIsInGroup()
    {
        // arrange
        var user = new UserProfile()
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test User"
        };
        var chat_sut = new ChatGroup()
        {
            Id = Guid.NewGuid(),

        };
        chat_sut.AddUser(user);

        // act
        var res = chat_sut.IsUserInGroup(user.Id);

        // assert
        Assert.True(res);
    }
    [Fact]
    public void IsUserInGroup_ReturnsFalseIfUserIsNotInGroup()
    {
        // arrange
        var user = new UserProfile()
        {
            Id = Guid.NewGuid().ToString(),
            DisplayName = "Test User"
        };
        var chat_sut = new ChatGroup()
        {
            Id = Guid.NewGuid(),

        };

        // act
        var res = chat_sut.IsUserInGroup(user.Id);
        
        // assert
        Assert.True(!res);
    }
}

