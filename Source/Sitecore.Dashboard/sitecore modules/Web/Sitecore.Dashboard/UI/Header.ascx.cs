using System;

namespace Sitecore.Dashboard.Web.UI
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetWelcomeMessage();
        }

        protected void SetWelcomeMessage()
        {
            lnkCurrentUser.Attributes["href"] = string.Format("mailto:{0}", Sitecore.Context.User.Profile.Email);
            lnkCurrentUser.Text = StringUtil.GetString(Sitecore.Context.User.Profile.FullName, Sitecore.Context.User.Name);
        }
    }
}