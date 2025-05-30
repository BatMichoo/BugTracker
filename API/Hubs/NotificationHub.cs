using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Core.Other;

[Authorize(Policy = AuthorizePolicy.UserAccess)]
public class NotificationHub : Hub
{
    // public override async Task OnConnectedAsync()
    // {
    //     await base.OnConnectedAsync();
    // }
    //
    // public override async Task OnDisconnectedAsync(Exception? exception)
    // {
    //     await base.OnDisconnectedAsync(exception);
    // }
}
