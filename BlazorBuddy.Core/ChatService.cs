using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.Core
{
    public class ChatService : IChatService
    {
        public List<ChatGroup> ActiveChatGroups { get; set; } = [];
        private readonly IChatRepo _chatRepo;
        private readonly IUserRepo _userRepo;
        public ChatService(IChatRepo chatRepo, IUserRepo userRepo)
        {
            _chatRepo = chatRepo;
            _userRepo = userRepo;

        }



        public event Action<ChatGroup>? OnChatGroupUpdate;

        public async Task AddUserToChatGroup(Guid groupId, string userId)
        {
            var chat = await _chatRepo.GetChatGroupById(groupId);
            var newUser = await _userRepo.GetUserById(userId);

            if (chat == null)
            {
                throw new ArgumentNullException(nameof(chat));
            }
            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser));
            }

            var updatedGroup = new ChatGroup
            {
                Id = chat.Id,
                Messages = chat.Messages,
                Title = chat.Title,
                Users = [.. chat.Users, newUser]
            };
            var fetchedGroup = await _chatRepo.UpdateChatGroup(updatedGroup);
            if (fetchedGroup == null)
            {
                throw new Exception("Failed to update group");
            }
            NotifyStateChanged(fetchedGroup);

        }

        public async Task SendMessage(Guid groupId, string userId, string message)
        {
            var user = await _userRepo.GetUserById(userId);
            var chat = await _chatRepo.GetChatGroupById(groupId);
            if (user == null)
            {
                throw new Exception("Failed to send message,Sender user is not found ");
            }
            if (chat == null)
            {
                throw new Exception("Chat group does not exist");
            }
            var newMessage = new ChatMessage
            {
                Content = message,
                FromUser = user,
            };

            NotifyStateChanged(chat);
        }
        private void NotifyStateChanged(ChatGroup group)
        {
            // This checks if anyone is listening, then fires the event
            OnChatGroupUpdate?.Invoke(group);
        }
    }
}
