Sitecore-Dashboard
==================

Sitecore app that aggregates real-time data about your site into a customizable widget-based view.


**Requirements:**

- .NET 4.0+
- IIS 7+
- Tested on Sitecore 6.4.1, 6.5, and 6.6


**Installation:**

1. Install Sitecore package

2. Add to `<runtime>/<assemblyBinding>` in Web.config:

        
        <dependentAssembly>
          <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" />
          <bindingRedirect oldVersion="1.0.0.0-3.6.0.0" newVersion="4.5.0.0" />
        </dependentAssembly>

3. Add to Global.asax:

        <%@ Import Namespace="System.Web.Http" %>
        <%@ Import Namespace="System.Web.Routing" %>
        ...
          public void Application_Start() {
            RouteTable.Routes.MapHttpRoute(
              name: "Widgets API",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { controller = "widgets" }
            );
            RouteTable.Routes.MapHubs();
          }
        ...
  
**Contact Info:**  
<valerie.concepcion@nttdata.com>  
<http://www.nttdatasitecore.com>