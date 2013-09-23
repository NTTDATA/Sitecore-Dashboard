using System.Collections.Generic;
using Sitecore.Security.Accounts;
using Sitecore.Web.Authentication;

namespace Sitecore.Dashboard.Models
{
    public class LoggedIn : WidgetModel
    {
        public List<LoggedInUser> Users;

        public override void Initialize()
        {
            Users = new List<LoggedInUser>();
            foreach (DomainAccessGuard.Session session in DomainAccessGuard.Sessions)
            {
                User user = Sitecore.Security.Accounts.User.FromName(session.UserName, true);
                if (user != null)
                {
                    Users.Add(new LoggedInUser()
                    {
                        Name = StringUtil.GetString(user.Profile.FullName, user.Name),
                        Email = string.Format("mailto:{0}", user.Profile.Email),
                        Time = session.LastRequest.ToString("h:mmtt").ToLower(),
                        Date = session.LastRequest.ToString("dd-MMM-yyyy")
                    });
                }
            }
        }
    }

    public class LoggedInUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
    }
}