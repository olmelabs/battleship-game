using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.SignalRHubs
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //string name = Context.User.Identity.Name;
            //_connections.Add(name, Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("AcquireConnectionId", Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //string name = Context.User.Identity.Name;
            //_connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
