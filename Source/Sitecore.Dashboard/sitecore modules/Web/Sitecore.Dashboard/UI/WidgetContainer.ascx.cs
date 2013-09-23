using System;
using System.Linq;
using System.Web.UI.WebControls;
using Sitecore.Dashboard.Web.UI.Widgets;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Sitecore.Dashboard.Web.UI
{
    public partial class WidgetContainer : System.Web.UI.UserControl
    {
        public Item RootFolder
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RootFolder == null) return;

            // Iterate through positions
            foreach (Item positionItem in RootFolder.Children)
            {
                Assert.IsTrue(positionItem.TemplateID.ToString() == Constants.TemplateIDs.WidgetPosition, "Invalid dashboard position " + positionItem.Name);
                string positionName = positionItem.GetFieldValueOrDefault(Constants.FieldIDs.WidgetPositionPlaceholderName, positionItem.Name);
                // Get ListView matching position name and bind child widgets
                var listView = FindControl(positionName) as ListView;
                if (listView != null)
                {
                    listView.DataSource = positionItem.Children.Select(item => new WidgetItem(item));
                    listView.DataBind();
                }
            }
        }

        protected void lvWidgets_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var widget = e.Item.DataItem as WidgetItem;
                if (widget == null) return;
                Assert.IsNotNullOrEmpty(widget.Type, "Widget type");

                // Display widget title
                var litTitle = e.Item.FindControl("litTitle") as Literal;
                if (litTitle != null)
                {
                    litTitle.Text = widget.Name;
                }

                // Get and validate widget type
                Database coreDb = Client.CoreDatabase;
                Item widgetType = coreDb.GetItem(widget.Type);
                Assert.IsNotNull(widgetType, "widgetType");
                Assert.IsTrue(widgetType.TemplateID.ToString() == Constants.TemplateIDs.WidgetType, "");

                // Get user control path
                string path = widgetType.GetFieldValueOrDefault(Constants.FieldIDs.WidgetTypeControlPath);
                Assert.IsNotNullOrEmpty(path, "path");

                // Load widget user control
                var widgetControl = LoadControl(path) as Widget;
                var phWidgetContent = e.Item.FindControl("phWidgetContent") as PlaceHolder;
                if (widgetControl != null && phWidgetContent != null)
                {
                    // Pass API route and parameters to control
                    widgetControl.ApiRoute = "/api/widgets/" + widget.Id.ToGuid();
                    widgetControl.Parameters = WebUtil.ParseUrlParameters(widget.Parameters);
                    phWidgetContent.Controls.Add(widgetControl);
                }
            }
        }
    }

    public class WidgetItem
    {
        public ID Id { get; protected set; }
        public string Name { get; protected set; }
        public string Type { get; protected set; }
        public string Position { get; protected set; }
        public string CssClass { get; protected set; }
        public string Parameters { get; protected set; }

        public WidgetItem(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.IsTrue(item.TemplateID.ToString() == Constants.TemplateIDs.Widget, "Invalid widget item " + item.Name);
            Id = item.ID;
            Name = item.GetFieldValueOrDefault(Constants.FieldIDs.WidgetName);
            Type = item.GetFieldValueOrDefault(Constants.FieldIDs.WidgetType);
            CssClass = item.GetFieldValueOrDefault(Constants.FieldIDs.WidgetCssClass);
            Parameters = item.GetFieldValueOrDefault(Constants.FieldIDs.WidgetParameters);
        }
    }
}