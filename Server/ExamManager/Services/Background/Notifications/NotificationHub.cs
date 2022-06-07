using Microsoft.AspNetCore.SignalR;

namespace ExamManager.Services;
public class NotificationHub : Hub
{
    public async Task SendNotification(Guid userId, string message)
    {
        await Clients.All.SendAsync("Notify", userId, message);
    }
}