using System.Collections.Generic;
using System.Linq;
using Sitecore.Jobs;
using Sitecore.Security.Accounts;

namespace Sitecore.Dashboard.Models
{
    public class JobViewer : WidgetModel
    {
        public string[] JobCategoryFilters
        {
            get
            {
                string filters = this.Parameters["JobCategoryFilters"];
                return !string.IsNullOrEmpty(filters) ? filters.Split('|') : null;
            }
        }
        
        public List<JobEntity> Jobs;

        public override void Initialize()
        {
            string[] jobCategoryFilters = JobCategoryFilters ?? new string[0];

            Jobs = new List<JobEntity>();

            foreach (Job job in JobManager.GetJobs().OrderBy(j => j.QueueTime))
            {
                if (jobCategoryFilters.Length > 0 &&
                    !jobCategoryFilters.Any(filter => job.Category.ToLower().Equals(filter.ToLower())))
                {
                    continue;
                }

                JobEntity item = new JobEntity
                {
                    Name = job.Name,
                    Status = job.Status.State.ToString(),
                    Owner = "Anonymous",
                    Time = job.QueueTime.ToString("h:mmtt").ToLower(),
                    Date = job.QueueTime.ToString("dd-MMM-yyyy")
                };
                if (job.Options.ContextUser != null)
                {
                    User user = User.FromName(job.Options.ContextUser.Name, true);
                    if (user != null)
                    {
                        item.Owner = StringUtil.GetString(user.Profile.FullName, user.Name);
                    }
                }
                Jobs.Add(item);
            }
        }
    }

    public class JobEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
    }
}