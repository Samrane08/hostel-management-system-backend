using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Service;
public class NotifyHub : Hub
{
    public Task Notify(string Id)
    {
        return Clients.All.SendAsync("Notification", Id);
    }
}
