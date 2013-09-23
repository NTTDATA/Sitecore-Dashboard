using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace Sitecore.Dashboard.Pipelines.HttpRequestBegin
{
    public class TransferRoutedRequest : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            HttpContextWrapper httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
            if (routeData != null)
            {
                RouteValueDictionary dictionary = (routeData.Route as Route).ValueOrDefault<Route, RouteValueDictionary>(r => r.Defaults);
                if ((dictionary == null) || !dictionary.ContainsKey("scIsFallThrough"))
                {
                    args.AbortPipeline();
                }
            }
        }
    }
}