<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedIn.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.Widgets.LoggedIn" %>

<!-- ko with: <%= ClientID %> -->
<ul data-bind="foreach: Users, visible: Users.length > 0">
    <li>
        <a data-bind="attr: { href: 'mailto:' + Email }, text: Name"></a><br />
	    <b class="time" data-bind="text: Time"></b> <span data-bind="text: Date"></span>
    </li>
</ul>
<p data-bind="visible: Users.length > 0">
    <a href="/sitecore/shell/Applications/Login/Users/Kick.aspx" target="_blank">Kick Users</a>
</p>
<p data-bind="visible: Users.length == 0">
    No users currently logged in
</p>
<!-- /ko -->

<script>
    $j(document).ready(function () {
        // Constructor takes ID of widget to auto-refresh and URL of Web API call
        var widget = new Widget('<%= ClientID %>', '<%= ApiRoute %>');
        // Bind update event handler to Sitecore security events
        $j(document).bind('security:loggedIn security:loggedOut', function() {
            setTimeout(function() {
                widget.update();
            }, 1000);
        });
    });
</script>