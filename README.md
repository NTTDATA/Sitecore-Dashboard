Sitecore-Dashboard
==================

Sitecore app that aggregates real-time data about your site into a customizable widget-based view.


**Requirements:**

- .NET 4.0+
- IIS 7+
- Json.NET 4.5 (contained in Sitecore package and will overwrite existing version of Newtonsoft.Json.dll)
- Tested on Sitecore 6.4.1, 6.5, and 6.6


**Installation:**

1. Install Sitecore package

2. Add to `<runtime>/<assemblyBinding>` in Web.config:

        
        <dependentAssembly>
          <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" />
          <bindingRedirect oldVersion="1.0.0.0-3.6.0.0" newVersion="4.5.0.0" />
        </dependentAssembly>

  
**Contact Info:**  
<valerie.concepcion@nttdata.com>  
<http://www.nttdatasitecore.com>