using Sitecore.Events;
using Sitecore.Jobs;

namespace Sitecore.Dashboard.Pipelines.Jobs
{
    public class JobRunner
    {
        public static void RaiseJobStartedEvent(JobArgs args)
        {
            Event.RaiseEvent("job:started", new object[] { });
        }

        public static void RaiseJobEndedEvent(JobArgs args)
        {
            Event.RaiseEvent("job:ended", new object[] { });
        }
    }
}