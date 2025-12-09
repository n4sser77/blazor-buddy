using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.Models;
using BlazorBuddy.WebApp.Data;
using Microsoft.Build.Tasks;
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
        
        // Reload with navigation properties to ensure complete entity graph
        return await GetChatGroupById(chat.Id) ?? chat;
    }



    public async Task<ChatGroup?> GetChatGroupById(Guid id)
    {
        var chat = await _ctx.ChatGroups
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .ThenInclude(m => m.FromUser)
            .FirstOrDefaultAsync(c => c.Id == id);
        return chat;
    }

    public async Task<List<ChatGroup>> GetChatGroupsByUserId(string userId)
    {
        var chats = await _ctx.ChatGroups.Where(c => c.Users.Any(u => u.Id == userId)).Include(c => c.Users).Include(c => c.Messages).ThenInclude(m => m.FromUser).ToListAsync();

        return chats ?? [];
    }

    public async Task<ChatGroup?> UpdateChatGroup(ChatGroup newChatGroup)
    {
        var chat = await _ctx.ChatGroups
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .ThenInclude(m => m.FromUser)
            .FirstOrDefaultAsync(c => c.Id == newChatGroup.Id);
        
        if (chat is null)
        {
            return null;
        }
        
        chat.Users = newChatGroup.Users;
        chat.Title = newChatGroup.Title;

        await _ctx.SaveChangesAsync();

        return chat;
    }

    public async Task<ChatGroup?> AddMessage(Guid chatId, string msg, UserProfile user)
    {
        // Critical: Include Messages to ensure the collection is loaded before adding
        // Without this, the new message won't appear in the returned chat.Messages
        var chat = await _ctx.ChatGroups
            .Include(c => c.Messages)
            .ThenInclude(m => m.FromUser)
            .FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat is null)
        {
            return null;
        }
        
        chat.AddMessage(user, msg);
        await _ctx.SaveChangesAsync();

        return chat;
    }
}

