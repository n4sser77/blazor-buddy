using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Services.Interfaces;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorBuddy.WebApp.Services;

public class ChatService : IChatService
{
    public List<ChatGroup> ActiveChatGroups { get; set; } = [];
    private readonly IChatRepo _chatRepo;
    private readonly IUserRepo _userRepo;
    private readonly IChatEventBroker _broker;
    public ChatService(IChatRepo chatRepo, IUserRepo userRepo, IChatEventBroker broker)
    {
        _chatRepo = chatRepo;
        _userRepo = userRepo;
        _broker = broker;
    }


    public async Task AddUserToChatGroup(Guid groupId, string userId)
    {
        var group = await _chatRepo.GetChatGroupById(groupId);
        if (group is null) throw new ArgumentNullException(nameof(group));

        var user = await _userRepo.GetUserById(userId);
        if (user is null) throw new ArgumentNullException(nameof(user));


        var updatedGroup = group.AddUser(user);

        updatedGroup = await _chatRepo.UpdateChatGroup(updatedGroup);
        if (updatedGroup is null) throw new Exception("Failed to update group");

        _broker.NotifyStateChanged(updatedGroup);
    }


    public async Task SendMessage(Guid groupId, string userId, string message)
    {
        var group = await _chatRepo.GetChatGroupById(groupId)
            ?? throw new ArgumentNullException("Group is null");

        var user = await _userRepo.GetUserById(userId)
            ?? throw new ArgumentNullException("User is null");

        var updatedGroup = group.AddMessage(user, message);

        updatedGroup = await _chatRepo.UpdateChatGroup(updatedGroup)
                         ?? throw new Exception("Failed to update group");

        _broker.NotifyStateChanged(updatedGroup);
    }

    public async Task<ChatGroup> CreateChatGroup(string title, List<UserProfile> members)
    {
        var chat = await _chatRepo.CreateChatGroup(title, members)
            ?? throw new ArgumentNullException("Group is null");

        List<UserProfile> fetchedMembers = [];
        foreach (var user in members)
        {
            var fetchedUser = await _userRepo.GetUserById(user.Id) ??
                throw new ArgumentNullException("User does not exist");
            fetchedMembers.Add(fetchedUser);
        }
        _broker.NotifyStateChanged(chat);
        return chat;
    }

    public async Task<ChatGroup> UpdateChatGroup(ChatGroup updatedChatgroup)
    {
        var chat = await _chatRepo.UpdateChatGroup(updatedChatgroup)
            ?? throw new ArgumentNullException("Group is null");
        _broker.NotifyStateChanged(chat);
        return chat;
    }
}
