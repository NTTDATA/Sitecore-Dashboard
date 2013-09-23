<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Links.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.Links" %>

<asp:ListView ID="lvLinkSections" runat="server" OnItemDataBound="lvLinkSections_ItemDataBound">
    <LayoutTemplate>
        <nav>
	        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </nav>
    </LayoutTemplate>
    <ItemTemplate>
        <h4><asp:Literal ID="litHeader" runat="server" /></h4>
        <asp:ListView ID="lvLinks" runat="server" OnItemDataBound="lvLinks_ItemDataBound">
            <LayoutTemplate>
                <ul>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <asp:PlaceHolder ID="phLink" runat="server" />
                </li>
            </ItemTemplate>
        </asp:ListView>
    </ItemTemplate>
</asp:ListView>