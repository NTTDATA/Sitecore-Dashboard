using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.Publish;
using Sitecore.Workflows;
using Sitecore.Security.Accounts;

namespace Sitecore.Dashboard.Web.UI.Widgets
{
    public partial class PublishQueueViewer : Widget
    {
        private Database _sourceDb;
        private Database _targetDb;
        private int? _pageSize;
        protected const int DefaultPageSize = 10;

        protected Database SourceDb
        {
            get
            {
                if (_sourceDb == null)
                {
                    _sourceDb = Database.GetDatabase("master");
                }
                return _sourceDb;
            }
        }

        protected Database TargetDb
        {
            get
            {
                if (_targetDb == null)
                {
                    _targetDb = Database.GetDatabase("web");
                }
                return _targetDb;
            }
        }

        public int? PageSize
        {
            get
            {
                if (_pageSize == null)
                {
                    int pageSize;
                    _pageSize = (!string.IsNullOrEmpty(this.Parameters["PageSize"]) && Int32.TryParse(this.Parameters["PageSize"], out pageSize)) ? pageSize : DefaultPageSize;
                }
                return _pageSize;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lvItems.DataBind();
        }

        protected void lvItems_DataBound(object sender, EventArgs e)
        {
            Control ctl = lvItems.FindControl("pgrResults");
            if (ctl != null)
            {
                DataPager pager = (DataPager)ctl;
                pager.Attributes["class"] = "pagination";
                pager.PageSize = PageSize.GetValueOrDefault(DefaultPageSize);
                Control tfoot = lvItems.FindControl("tfoot");
                if (tfoot != null)
                {
                    tfoot.Visible = pager.TotalRowCount > pager.PageSize;
                }
            }
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

                    Literal litApprover = e.Item.FindControl("litApprover") as Literal;
                    Literal litApproved = e.Item.FindControl("litApproved") as Literal;
                    ApprovalInfo info = GetApprovalInfo(item);
                    if (!string.IsNullOrEmpty(info.Approver) && User.Exists(info.Approver))
                    {
                        User user = User.FromName(info.Approver, false);
                        litApprover.Text = Sitecore.StringUtil.GetString(user.Profile.FullName, user.DisplayName, user.Name);
                    }
                    litApproved.Text = info.Approved.ToString("dd-MMM-yyyy");
                }
            }
        }

        protected void dsPublishQueue_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["pageSize"] = PageSize;
            e.InputParameters["sourceDb"] = SourceDb;
            e.InputParameters["targetDb"] = TargetDb;
        }

        protected virtual ApprovalInfo GetApprovalInfo(Item item)
        {
            IWorkflowProvider workflowProvider = SourceDb.WorkflowProvider;
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
                            return new ApprovalInfo()
                            {
                                Approver = wfEvent.User,
                                Approved = wfEvent.Date
                            };
                        }
                    }
                }
            }

            // fallback on last updated by (e.g., if no workflow assigned)
            return new ApprovalInfo()
            {
                Approver = item.Statistics.UpdatedBy,
                Approved = item.Statistics.Updated
            };
        }

        public class ApprovalInfo
        {
            public string Approver { get; set; }
            public DateTime Approved { get; set; }
        }
    }

    [DataObject(true)]
    public class PublishQueueDataSource
    {
        private static int _rowCount;

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<Item> GetRows(Database sourceDb, Database targetDb, int maximumRows, int startRowIndex, int pageSize)
        {
            List<Item> queuedItems = new List<Item>();
            // TODO: dynamically set language
            DateTime publishDate = DateTime.Now;
            PublishOptions options = new PublishOptions(sourceDb, targetDb, PublishMode.Incremental, Language.Parse("en"), publishDate);
            IEnumerable<PublishingCandidate> candidates = PublishQueue.GetPublishQueue(options);
            foreach (PublishingCandidate candidate in candidates)
            {
                Item item = sourceDb.GetItem(candidate.ItemId);
                if (item != null)
                {
                    item = item.Publishing.GetValidVersion(publishDate, true);
                    if (item != null && item.Access.CanRead() && item.Access.CanReadLanguage() && item.Access.CanWriteLanguage() && ((Sitecore.Context.IsAdministrator || item.Locking.CanLock()) || item.Locking.HasLock()))
                    {
                        queuedItems.Add(item);
                    }
                }
            }
            _rowCount = queuedItems.Count;
            return queuedItems.Skip(startRowIndex).Take(pageSize).ToList();
        }

        public static int GetCount(Database sourceDb, Database targetDb, int pageSize)
        {
            return _rowCount;
        }
    }
}