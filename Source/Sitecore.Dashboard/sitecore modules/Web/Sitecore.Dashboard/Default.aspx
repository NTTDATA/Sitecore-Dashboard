<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sitecore.Dashboard.Web.Default" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.WebControls" TagPrefix="sc" %>
<%@ Register Src="~/sitecore modules/Web/Sitecore.Dashboard/UI/Header.ascx" TagName="Header" TagPrefix="dashboard" %>
<%@ Register Src="~/sitecore modules/Web/Sitecore.Dashboard/UI/Links.ascx" TagName="Links" TagPrefix="dashboard" %>
<%@ Register Src="~/sitecore modules/Web/Sitecore.Dashboard/UI/WidgetContainer.ascx" TagName="WidgetContainer" TagPrefix="dashboard" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
	<title>CMS Dashboard</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <link rel="stylesheet" href="/sitecore modules/Web/Sitecore.Dashboard/css/base.css" />
    <link rel="stylesheet" href="/sitecore modules/Web/Sitecore.Dashboard/css/grid.css" />
    <link rel="stylesheet" href="/sitecore modules/Web/Sitecore.Dashboard/css/dashboard.css" />
    <asp:PlaceHolder ID="phStylesheets" runat="server" />

    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script>
        window.jQuery || document.write('<script src="/Scripts/jquery-1.9.1.min.js"><\/script>');
        $j = jQuery.noConflict();
    </script>
    <script src="/Scripts/modernizr-2.6.2.js"></script>
    <script src="/Scripts/knockout-2.3.0.js"></script>
    <script src="/Scripts/json2.min.js"></script>
    <script src="/Scripts/jquery.signalR-1.1.3.min.js"></script>
    <script src="/signalr/hubs"></script>
    <script src="/sitecore modules/Web/Sitecore.Dashboard/js/dashboard.hub.js"></script>
    <script src="/sitecore modules/Web/Sitecore.Dashboard/js/dashboard.widgets.js"></script>
    <script>
        var Dashboard = Dashboard || {
            ViewModel: {}
        };
    </script>
</head>

<body id="body" class="dashboard">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
        <sc:AjaxScriptManager ID="AjaxScriptManager1" runat="server"/>

        <div id="container">
            <header id="header">
	            <div class="container">
		            <div class="site-nav">
			            <div class="row">
				            <dashboard:Header ID="Header1" runat="server" />
			            </div>
		            </div>			
	            </div>
            </header>
            <div id="main">
                <div class="container">
                    <div class="row">
                        <div class="content-main">
                            <div class="row">
                                <div class="page-tools offset-by-nine three text-right">
                                    <asp:LinkButton ID="lbRefresh" runat="server" CssClass="btn-refresh">
                                        <img src="/sitecore modules/Web/Sitecore.Dashboard/img/i-refresh.png" width="17" height="17" alt="Refresh" />
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <dashboard:WidgetContainer ID="WidgetContainer1" runat="server" />
                        </div>
                        <div class="sidebar">
                            <dashboard:Links ID="Links1" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        $j(document).ready(function () {
            ko.applyBindings(Dashboard.ViewModel);
        });
    </script>
</body>
</html>
