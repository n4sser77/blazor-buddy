using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorBuddy.WebApp.Repositories;

public class ChatRepo : IChatRepo
{
    private readonly ApplicationDbContext _ctx;
    public ChatRepo(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<ChatGroup> CreateChatGroup(string title, List<UserProfile> users)
    {
        var chat = new ChatGroup { Title = title, Users = users };
        await _ctx.ChatGroups.AddAsync(chat);

        await _ctx.SaveChangesAsync();
        return chat;
    }



    public async Task<ChatGroup?> GetChatGroupById(Guid id)
    {
        var chat = await _ctx.ChatGroups.FirstOrDefaultAsync(c => c.Id == id);
        return chat;
    }

    public async Task<List<ChatGroup>> GetChatGroupsByUserId(string userId)
    {
        var chats = await _ctx.ChatGroups.Where(c => c.Users.Any(u => u.Id == userId)).Include(c => c.Messages).ThenInclude(m => m.FromUser).ToListAsync();

        return chats ?? [];
    }

    public async Task<ChatGroup?> UpdateChatGroup(ChatGroup newChatGroup)
    {
        var chat = await _ctx.ChatGroups
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == newChatGroup.Id);
        if (chat is null)
        {
            return null;
        }

        // Update Users
        // Remove users not in newChatGroup
        var usersToRemove = chat.Users.Where(u => !newChatGroup.Users.Any(nu => nu.Id == u.Id)).ToList();
        foreach (var user in usersToRemove)
        {
            chat.Users.Remove(user);
        }
        // Add new users
        var usersToAdd = newChatGroup.Users.Where(nu => !chat.Users.Any(u => u.Id == nu.Id)).ToList();
        foreach (var user in usersToAdd)
        {
            // Attach user if not tracked
            if (_ctx.Entry(user).State == EntityState.Detached)
            {
                _ctx.UserProfiles.Attach(user);
            }
            chat.Users.Add(user);
        }

        // Update Messages
        // Remove messages not in newChatGroup
        var messagesToRemove = chat.Messages.Where(m => !newChatGroup.Messages.Any(nm => nm.Id == m.Id)).ToList();
        foreach (var msg in messagesToRemove)
        {
            chat.Messages.Remove(msg);
            _ctx.Messages.Remove(msg); // If you want to delete from DB
        }
        // Add new messages
        var messagesToAdd = newChatGroup.Messages.Where(nm => !chat.Messages.Any(m => m.Id == nm.Id)).ToList();
        foreach (var msg in messagesToAdd)
        {
            chat.Messages.Add(msg);
        }
        // Update existing messages (if needed)
        foreach (var msg in chat.Messages)
        {
            var newMsg = newChatGroup.Messages.FirstOrDefault(nm => nm.Id == msg.Id);
            if (newMsg != null)
            {
                msg.Content = newMsg.Content;
                msg.Timestamp = newMsg.Timestamp;
                msg.FromUserId = newMsg.FromUserId;
                // Add other properties as needed
            }
        }

        // Update Title
        chat.Title = newChatGroup.Title;

        await _ctx.SaveChangesAsync();

        return chat;
    }
}

