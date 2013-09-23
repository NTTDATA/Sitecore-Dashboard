<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.Header" %>

<div class="column six">
    <a href="/" target="_blank">
        <i class="brand-logo"></i>
	</a>
</div>
<div class="column six text-right">
	<p class="user">
        Welcome <asp:HyperLink ID="lnkCurrentUser" runat="server" />
    </p>
</div>