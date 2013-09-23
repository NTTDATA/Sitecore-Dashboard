<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobViewer.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.Widgets.JobViewer" %>

<!-- ko with: <%= ClientID %> -->
<ul data-bind="foreach: Jobs, visible: Jobs.length > 0">
    <li>
        <span data-bind="text: Name"></span><br />
        <em data-bind="text: Status"></em><br />
        <span data-bind="text: Owner"></span><br />
        <b class="time" data-bind="text: Time"></b> <span data-bind="    text: Date"></span>
    </li>
</ul>
<p data-bind="visible: Jobs.length == 0">
    No jobs running
</p>
<!-- /ko -->

<script>
    $j(document).ready(function() {
        // Constructor takes ID of widget to auto-refresh and URL of Web API call
        var widget = new Widget('<%= ClientID %>', '<%= ApiRoute %>');
        // Bind update event handler to Sitecore security events
        $j(document).bind('job:started job:ended', function() {
            widget.update();
        });
    });
</script>