using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Workflows;

namespace Sitecore.Dashboard.Web.UI.Widgets
{
    public partial class WorkflowState : Widget
    {
        private Database _database;
        private int? _maxItems;

        protected Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = Database.GetDatabase("master");
                }
                return _database;
            }
        }

        public string WorkflowId
        {
            get
            {
                return this.Parameters["WorkflowID"];
            }
        }

        public string WorkflowStateId
        {
            get
            {
                return this.Parameters["WorkflowStateID"];
            }
        }

        public int? MaxItems
        {
            get
            {
                if (_maxItems == null)
                {
                    int maxItems;
                    _maxItems = (!string.IsNullOrEmpty(this.Parameters["MaxItems"]) && Int32.TryParse(this.Parameters["MaxItems"], out maxItems)) ? maxItems : 10;
                }
                return _maxItems;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Assert.IsNotNullOrEmpty(WorkflowId, "WorkflowId widget parameter");
            Assert.IsNotNullOrEmpty(WorkflowStateId, "WorkflowStateId widget parameter");

            IWorkflowProvider workflowProvider = Database.WorkflowProvider;
            if (workflowProvider != null && !string.IsNullOrEmpty(WorkflowId))
            {
                IWorkflow workflow = workflowProvider.GetWorkflow(WorkflowId);
                if (workflow != null)
                {
                    lvItems.DataSource = GetItems(workflow);
                    lvItems.DataBind();

                    HyperLink lnkWorkbox = lvItems.FindControl("lnkWorkbox") as HyperLink;
                    if (lnkWorkbox != null)
                    {
                        string str = "P" + Regex.Replace(WorkflowId, @"\W", string.Empty);
                        Sitecore.Web.UI.HtmlControls.Registry.SetString("/Current_User/Panes/" + str, "visible");
                        lnkWorkbox.Attributes["href"] = "#";
                        lnkWorkbox.Attributes["onclick"] = string.Format("javascript:scForm.postRequest('', '', '', 'appshortcut:open(appid={0})'); return false;", Constants.ItemIDs.WorkboxShortcut);
                    }
                }
            }
        }

        protected virtual IEnumerable<Item> GetItems(IWorkflow workflow)
        {
            Assert.ArgumentNotNull(workflow, "workflow");

            List<Item> items = new List<Item>();
            DataUri[] uris = workflow.GetItems(WorkflowStateId);
            {
                foreach (DataUri uri in uris)
                {
                    if (items.Count >= MaxItems.Value)
                    {
                        break;
                    }
                    Item item = Database.Items[uri];
                    if ((((item != null) && item.Access.CanRead()) && (item.Access.CanReadLanguage() && item.Access.CanWriteLanguage())) && ((Sitecore.Context.IsAdministrator || item.Locking.CanLock()) || item.Locking.HasLock()))
                    {
                        items.Add(item);
                    }
                }
            }
            return items;
        }

        protected virtual void lvItems_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Item item = e.Item.DataItem as Item;
                if (item != null)
                {
                    HyperLink lnkItem = e.Item.FindControl("lnkItem") as HyperLink;
                    lnkItem.Text = string.Format("{0} ({1}, v{2})", item.Name, item.Language.CultureInfo.DisplayName, item.Version.Number);
                    lnkItem.Attributes["href"] = "#";
                    lnkItem.Attributes["onclick"] = string.Format("javascript:scForm.postRequest('', '', '', 'item:open(itemid={0})'); return false;", item.ID);

                    Literal litPath = e.Item.FindControl("litPath") as Literal;
                    litPath.Text = item.Paths.FullPath;

                    Literal litModifiedBy = e.Item.FindControl("litModifiedBy") as Literal;
                    Literal litModifiedDate = e.Item.FindControl("litModifiedDate") as Literal;
                    ItemInfo info = GetModifiedInfo(item);
                    if (User.Exists(info.ModifiedBy))
                    {
                        User user = User.FromName(info.ModifiedBy, false);
                        litModifiedBy.Text = Sitecore.StringUtil.GetString(user.Profile.FullName, user.DisplayName, user.Name);
                    }
                    litModifiedDate.Text = info.ModifiedDate.ToString("dd-MMM-yyyy");
                }
            }
        }

        protected virtual ItemInfo GetModifiedInfo(Item item)
        {
            IWorkflowProvider workflowProvider = Database.WorkflowProvider;
            if (workflowProvider != null)
            {
                string workflowField = item[FieldIDs.Workflow];
                if (!string.IsNullOrEmpty(workflowField))
                {
                    IWorkflow workflow = workflowProvider.GetWorkflow(workflowField);
                    if (workflow != null)
                    {
                        WorkflowEvent[] history = workflow.GetHistory(item);
                        if (history.Length > 0)
                        {
                            WorkflowEvent wfEvent = history[history.Length - 1];
                            return new ItemInfo()
                            {
                                ModifiedBy = wfEvent.User,
                                ModifiedDate = wfEvent.Date
                            };
                        }
                    }
                }
            }

            // fallback on last updated by
            return new ItemInfo()
            {
                ModifiedBy = item.Statistics.UpdatedBy,
                ModifiedDate = item.Statistics.Updated
            };
        }

        public class ItemInfo
        {
            public Item Item { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime ModifiedDate { get; set; }
        }
    }
}