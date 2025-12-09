using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Repositories;
using BlazorBuddy.WebApp.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Test.Services
{
    public class ChatServiceTests
    {

        [Fact]
        public async Task AddUserToChatGroup()
        {
            var chatId = Guid.NewGuid();
            var chatMock = new Mock<IChatRepo>();
            chatMock.Setup(
                x => x.GetChatGroupById(chatId))
                .ReturnsAsync(new ChatGroup
                {
                    Id = chatId,
                    Title = "Test",
                });

        }
    }
}
