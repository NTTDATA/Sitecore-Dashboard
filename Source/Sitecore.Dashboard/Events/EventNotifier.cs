using System;
using Microsoft.AspNet.SignalR;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Dashboard.Hubs;

namespace Sitecore.Dashboard.Events
{
    public static class EventNotifier
    {
        public static void OnDashboardEventRaised(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            if (Settings.GetBoolSetting("Dashboard.EnableLiveUpdates", false))
            {
                var scArgs = (SitecoreEventArgs)args;
                var context = GlobalHost.ConnectionManager.GetHubContext<DashboardHub>();
                context.Clients.All.raiseServerEvent(scArgs.EventName);
            }
        }
    }
}