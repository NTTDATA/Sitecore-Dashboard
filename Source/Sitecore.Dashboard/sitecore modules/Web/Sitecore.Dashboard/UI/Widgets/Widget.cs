using System.Collections.Specialized;

namespace Sitecore.Dashboard.Web.UI.Widgets
{
    public abstract class Widget : System.Web.UI.UserControl
    {
        public string ApiRoute;
        public NameValueCollection Parameters;
    }
}