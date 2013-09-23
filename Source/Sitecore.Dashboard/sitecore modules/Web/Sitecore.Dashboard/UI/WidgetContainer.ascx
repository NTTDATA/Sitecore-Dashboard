<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WidgetContainer.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.WidgetContainer" %>

<div class="row">
    <asp:ListView ID="Left" runat="server" OnItemDataBound="lvWidgets_ItemDataBound">
        <LayoutTemplate>
            <div class="column four">
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <section class="content <%# Eval("CssClass") %>">
                <h2><asp:Literal ID="litTitle" runat="server" /></h2>
                <asp:PlaceHolder ID="phWidgetContent" runat="server" />
            </section>
        </ItemTemplate>
    </asp:ListView>
    <asp:ListView ID="Right" runat="server" OnItemDataBound="lvWidgets_ItemDataBound">
        <LayoutTemplate>
            <div class="column eight">
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <section class="content <%# Eval("CssClass") %>">
                <h2><asp:Literal ID="litTitle" runat="server" /></h2>
                <asp:PlaceHolder ID="phWidgetContent" runat="server" />
            </section>
        </ItemTemplate>
    </asp:ListView>
</div>