using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Core.Other;
using Core.Repositories;

[Authorize(Policy = AuthorizePolicy.UserAccess)]
public class NotificationHub : Hub
{
    private readonly IBugNotificationRepository _repository;

    public NotificationHub(IBugNotificationRepository repository) : base()
    {
        _repository = repository;
    }

    public override async Task OnConnectedAsync()
    {
        string userId = Context.UserIdentifier!;

        var notifs = await _repository.GetUnRead(userId);

        foreach (var notif in notifs)
        {
            await Clients.User(notif.AssigneeId).SendAsync("new-assigned-bug", notif);
        }

        await base.OnConnectedAsync();
    }
}
