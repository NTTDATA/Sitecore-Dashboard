using System;
using System.Web.UI.HtmlControls;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework;
using Sitecore.Text;
using Sitecore.Web.UI.XamlSharp.Ajax;

namespace Sitecore.Dashboard.Web
{
    public partial class Default : System.Web.UI.Page
	{
        protected Database Database
        {
            get
            {
                return Client.Site.ContentDatabase;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            Client.AjaxScriptManager.OnExecute += Current_OnExecute;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            Database coreDb = Client.CoreDatabase;
            Item dashboardsFolder = coreDb.GetItem(Constants.ItemIDs.DashboardsFolder);
            if (dashboardsFolder != null)
            {
                string dashboardPath = string.Format("{0}/{1}", dashboardsFolder.Paths.FullPath, Settings.GetSetting("Dashboard.DefaultDashboard", "Standard"));
                Item dashboard = coreDb.GetItem(dashboardPath);
                if (dashboard != null && dashboard.TemplateID.ToString() == Constants.TemplateIDs.Dashboard)
                {
                    // Theme
                    string themeId = dashboard.GetFieldValueOrDefault(Constants.FieldIDs.DashboardTheme);
                    if (!string.IsNullOrEmpty(themeId))
                    {
                        Item theme = coreDb.GetItem(themeId);
                        if (theme != null)
                        {
                            ApplyThemeCss(theme);
                        }
                    }

                    // Widgets
                    string widgetsFolderPath = dashboardPath + Constants.Paths.WidgetsPath;
                    Item widgetsFolder = coreDb.GetItem(widgetsFolderPath);
                    if (widgetsFolder != null)
                    {
                        this.WidgetContainer1.RootFolder = widgetsFolder;
                    }

                    // Links
                    string linksFolderPath = dashboardPath + Constants.Paths.LinksPath;
                    Item linksFolder = coreDb.GetItem(linksFolderPath);
                    if (linksFolder != null)
                    {
                        this.Links1.RootFolder = linksFolder;
                    }
                }
            }
		}

        private void ApplyThemeCss(Item theme)
        {
            Assert.ArgumentNotNull(theme, "theme");
            string cssFilePath = theme.GetFieldValueOrDefault(Constants.FieldIDs.ThemeCssFilePath);
            if (!string.IsNullOrEmpty(cssFilePath))
            {
                var link = new HtmlGenericControl("link");
                link.Attributes["rel"] = "stylesheet";
                link.Attributes["href"] = cssFilePath;
                phStylesheets.Controls.Add(link);
            }
        }

        private void Current_OnExecute(object sender, AjaxCommandEventArgs args)
        {
            switch (args.Command.Name)
            {
                case "item:open":
                    string itemid = args.Parameters["itemid"];
                    Item item = this.Database.GetItem(itemid);
                    var str2 = new UrlString();
                    str2.Append("fo", itemid);
                    str2.Append("id", itemid);
                    str2.Append("la", item.Language.ToString());
                    str2.Append("vs", item.Version.ToString());
                    str2.Append("sc_content", this.Database.Name);
                    Windows.RunApplication("Content Editor", str2.ToString());
                    break;
                case "appshortcut:open":
                    ID appId;
                    if (Sitecore.Data.ID.TryParse(args.Parameters["appid"], out appId))
                    {
                        Windows.RunShortcut(appId);
                    }
                    break;
            }
        }
    }
}