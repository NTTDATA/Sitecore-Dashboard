using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Sitecore.Dashboard.Pipelines.Initialize
{
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
            // Register route for Widgets Web API service
            RouteTable.Routes.MapHttpRoute(
                name: "Widgets API",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "widgets" }
            );

            // Register default route for SignalR hubs
            RouteTable.Routes.MapHubs();
        }
    }
}