using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Core.Other;
using Core.Repositories;

[Authorize(Policy = AuthorizePolicy.UserAccess)]
public class NotificationHub : Hub
{
    private readonly INotifRepository _repository;

    public NotificationHub(INotifRepository repository) : base() 
    {
        _repository = repository;
    }

    public override async Task OnConnectedAsync()
    {
        string userId = Context.UserIdentifier!;

        var notifs = await _repository.GetUnRead(userId);

        foreach (var notif in notifs) {
            await Clients.User(notif.AssigneeId).SendAsync("new-assigned-bug", notif);
        }

        await base.OnConnectedAsync();
    }

    // public override async Task OnDisconnectedAsync(Exception? exception)
    // {
    //     await base.OnDisconnectedAsync(exception);
    // }
}
