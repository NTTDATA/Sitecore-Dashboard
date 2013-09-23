using System;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;

namespace Sitecore.Dashboard.Web.UI
{
    public partial class Links : System.Web.UI.UserControl
    {
        public Item RootFolder
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RootFolder != null)
            {
                lvLinkSections.DataSource = RootFolder.Children;
                lvLinkSections.DataBind();
            }
        }

        protected void lvLinkSections_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var item = e.Item.DataItem as Item;
                var litHeader = e.Item.FindControl("litHeader") as Literal;
                litHeader.Text = StringUtil.GetString(item.Fields[Constants.FieldIDs.LinkSectionHeaderText].Value, item.DisplayName);
                if (item.HasChildren)
                {
                    var lvLinks = e.Item.FindControl("lvLinks") as ListView;
                    lvLinks.DataSource = item.Children;
                    lvLinks.DataBind();
                }
            }
        }

        protected void lvLinks_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var item = e.Item.DataItem as Item;
                var phLink = e.Item.FindControl("phLink") as PlaceHolder;
                switch(item.TemplateID.ToString())
                {
                    case Constants.TemplateIDs.ApplicationShortcut:
                        var lnkItem = new HyperLink();
                        lnkItem.Text = StringUtil.GetString(item.Fields[Constants.FieldIDs.ApplicationShortcutDisplayName].Value, item.DisplayName);
                        lnkItem.Attributes["href"] = "#";
                        lnkItem.Attributes["onclick"] = string.Format("javascript:scForm.postRequest('', '', '', 'appshortcut:open(appid={0})'); return false;", item.ID);
                        phLink.Controls.Add(lnkItem);
                        break;
                    case Constants.TemplateIDs.Link:
                        var link = new Sitecore.Web.UI.WebControls.Link()
                        {
                            Item = item,
                            Field = Constants.FieldIDs.LinkLink
                        };
                        if (string.IsNullOrEmpty(link.Text))
                        {
                            link.Text = item.DisplayName;
                        }
                        phLink.Controls.Add(link);
                        break;
                }

            }
        }
    }
}