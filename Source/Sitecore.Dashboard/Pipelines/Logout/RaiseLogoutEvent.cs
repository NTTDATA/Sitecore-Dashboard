using Sitecore.Events;
using Sitecore.Pipelines.Logout;

namespace Sitecore.Dashboard.Pipelines.Logout
{
    public class RaiseLogoutEvent
    {
        public void Process(LogoutArgs args)
        {
            Event.RaiseEvent("security:loggedOut", new object[] { });
        }
    }
}