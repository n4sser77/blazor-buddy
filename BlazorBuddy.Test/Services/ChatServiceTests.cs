using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories;
using BlazorBuddy.WebApp.Services;
using BlazorBuddy.WebApp.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Test.Services
{
    public class ChatServiceTests
    {

        private readonly ChatService _sut;
        private readonly Mock<IChatEventBroker> _brokerMock = new Mock<IChatEventBroker>();
        private readonly Mock<IChatRepo> _chatRepoMock = new Mock<IChatRepo>();
        private readonly Mock<IUserRepo> _userRepoMock = new Mock<IUserRepo>();
        public ChatServiceTests()
        {
            _sut = new ChatService(_chatRepoMock.Object, _userRepoMock.Object, _brokerMock.Object);

        }

        [Fact]
        public async Task AddUserToChatGroup_AddsUserIfNotAlreadyMemberAndNotifes()
        {
            // arrange
            var chatId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            ChatGroup chat = new() { Id = chatId, Title = "Test-chat" };
            UserProfile user = new() { Id = userId, DisplayName = "Test-user" };

            _chatRepoMock
                .Setup(x => x.GetChatGroupById(chatId))
                .ReturnsAsync(chat);

            _userRepoMock
                .Setup(x => x.GetUserById(userId))
                .ReturnsAsync(user);


            _chatRepoMock
                .Setup(x => x.UpdateChatGroup(It.IsAny<ChatGroup>()))
                .ReturnsAsync((ChatGroup g) => g);

            // act
            await _sut.AddUserToChatGroup(chatId, userId);

            // assert 
            Assert.Contains(chat.Users, u => u.Id == userId);


            _chatRepoMock.Verify(
                x => x.UpdateChatGroup(
                    It.Is<ChatGroup>(
                        g => g.Users
                            .Any(u => u.Id == userId))
                ), Times.Once);


            _brokerMock.Verify(
                x => x.NotifyStateChanged(
                    It.Is<ChatGroup>(g => g.Id == chatId 
                        && g.Users.Any(u => u.Id == userId))
                ),
                Times.Once);
        }

        [Fact]
        public async Task SendMessage_CreatesAndSendsMessageToChat()
        {
            // arrange 
            var chatId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var msg = "Test message";

            ChatGroup chat = new() { Id = chatId };
            UserProfile user = new() { Id = userId };

            _chatRepoMock
                .Setup(x => x.GetChatGroupById(chatId))
                .ReturnsAsync(chat);

            _userRepoMock
                .Setup(x => x.GetUserById(userId))
                .ReturnsAsync(user);


            _chatRepoMock
                .Setup(x => x.AddMessage(chat.Id, It.IsAny<string>(), user))
                .ReturnsAsync(chat);

            // act
            await _sut.SendMessage(chatId, userId, msg);

            // assert 
            _chatRepoMock.Verify(
                x => x.AddMessage(chat.Id, msg, user),
                Times.Once);

            _brokerMock.Verify(
                x => x.NotifyStateChanged(chat),
                Times.Once);

        }


    }
}
