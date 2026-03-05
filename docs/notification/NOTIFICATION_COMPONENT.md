# Notification Toast Component

The notification toast component provides a clean and user-friendly way to display notifications to users in the BlazorBuddy application.

## Features

- **Toast-style notifications** that appear in the top-right corner
- **Auto-dismiss** after 5 seconds
- **Multiple notification types**: Chat Messages, Chat Group Invites, and Friend Requests
- **Animated entrance and exit** for a smooth user experience
- **Event-driven architecture** using the singleton NotificationService
- **Maximum 5 notifications** displayed at once (older ones are removed)

## Architecture

The notification system consists of:

1. **Notification Model** (`BlazorBuddy.Core/Models/Notification.cs`)
   - Defines the structure of a notification
   - Supports three types: `ChatMessage`, `ChatGroupInvite`, `FriendRequest`

2. **NotificationService** (`BlazorBuddy.WebApp/Services/NotificationService.cs`)
   - Singleton service that manages notification events
   - Provides `ShowNotification()` method to trigger notifications
   - Uses event-based pattern similar to `ChatEventBroker`

3. **ToastContainer Component** (`BlazorBuddy.WebApp/Components/Modules/ToastContainer.razor`)
   - Subscribes to notification events
   - Manages the display of multiple notifications
   - Handles auto-dismissal

4. **ToastNotification Component** (`BlazorBuddy.WebApp/Components/Modules/ToastNotification.razor`)
   - Individual notification toast UI
   - Displays title, description, and type-specific icon
   - Supports manual dismissal

## Usage

### Basic Usage

To show a notification from any component, inject the `INotificationService` and call `ShowNotification()`:

```csharp
@inject INotificationService NotificationService

// In your code
NotificationService.ShowNotification(
    title: "New Message",
    description: "You have a new message from John Doe",
    type: NotificationType.ChatMessage
);
```

### Example: Friend Request Notification

```csharp
@inject INotificationService NotificationService
@inject IFriendShipService FriendShipService

private async Task SendFriendRequest(string userId)
{
    await FriendShipService.SendFriendRequest(userId);
    
    NotificationService.ShowNotification(
        title: "Friend Request Sent",
        description: $"Your friend request has been sent",
        type: NotificationType.FriendRequest
    );
}
```

### Example: Chat Group Invite

```csharp
@inject INotificationService NotificationService
@inject IChatService ChatService

private async Task InviteToGroup(Guid chatGroupId, string userId)
{
    await ChatService.AddUserToChatGroup(chatGroupId, userId);
    
    NotificationService.ShowNotification(
        title: "Invited to Group",
        description: $"You've been invited to join a chat group",
        type: NotificationType.ChatGroupInvite
    );
}
```

## Notification Types

| Type | Icon | Use Case |
|------|------|----------|
| `ChatMessage` | 💬 | New chat messages |
| `ChatGroupInvite` | 👥 | Chat group invitations |
| `FriendRequest` | 🤝 | Friend requests |

## Customization

### Auto-Dismiss Duration

To change the auto-dismiss duration, modify the `AutoDismissMs` constant in `ToastContainer.razor`:

```csharp
private const int AutoDismissMs = 5000; // Change to desired milliseconds
```

### Maximum Notifications

To change the maximum number of visible notifications, modify the `MaxNotifications` constant:

```csharp
private const int MaxNotifications = 5; // Change to desired number
```

### Styling

The toast appearance can be customized by editing:
- `ToastNotification.razor.css` - Individual toast styling
- `ToastContainer.razor.css` - Container positioning

## Testing

The notification system includes unit tests in `BlazorBuddy.Test/Services/NotificationServiceTests.cs`.

To run the tests:
```bash
dotnet test
```

## Demo

A demo is available on the Home page (when logged in) with three test buttons that trigger different notification types.
