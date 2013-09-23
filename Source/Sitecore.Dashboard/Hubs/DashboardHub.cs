using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Sitecore.Dashboard.Hubs
{
    public class DashboardHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.All.connected(Context.ConnectionId, DateTime.Now.ToString());
        }

        public override Task OnDisconnected()
        {
            return Clients.All.disconnected(Context.ConnectionId, DateTime.Now.ToString());
        }

        public void Send(string message)
        {
            Clients.All.broadcastMessage(message);
        }
    }
}