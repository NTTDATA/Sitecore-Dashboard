<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkflowState.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.Widgets.WorkflowState" %>

<asp:ListView ID="lvItems" runat="server" OnItemDataBound="lvItems_ItemDataBound">
    <LayoutTemplate>
        <table>
            <thead>
                <tr>
                    <th>
                        Item
                    </th>
                    <th>
                        Modified By
                    </th>
                    <th>
                        Date
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </tbody>
        </table>
        <p class="more">
            <asp:HyperLink ID="lnkWorkbox" runat="server">View All</asp:HyperLink>
        </p>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HyperLink ID="lnkItem" runat="server" />
                <p class="path"><asp:Literal ID="litPath" runat="server" /></p>
            </td>
            <td>
                <asp:Literal ID="litModifiedBy" runat="server" />
            </td>
            <td>
                <asp:Literal ID="litModifiedDate" runat="server" />
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <p>No items in this workflow state</p>
    </EmptyDataTemplate>
</asp:ListView>