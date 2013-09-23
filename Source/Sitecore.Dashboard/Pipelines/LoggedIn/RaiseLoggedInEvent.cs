using Sitecore.Events;
using Sitecore.Pipelines.LoggedIn;

namespace Sitecore.Dashboard.Pipelines.LoggedIn
{
    public class RaiseLoggedInEvent : LoggedInProcessor
    {
        public override void Process(LoggedInArgs args)
        {
            Event.RaiseEvent("security:loggedIn", new object[] { });
        }
    }
}