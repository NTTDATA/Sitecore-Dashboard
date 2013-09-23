<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublishQueueViewer.ascx.cs" Inherits="Sitecore.Dashboard.Web.UI.Widgets.PublishQueueViewer" %>
<%@ Register Assembly="Sitecore.Dashboard" Namespace="Sitecore.Dashboard.Web.UI.Controls" TagPrefix="uc1" %>

<asp:UpdatePanel ID="pnlRefresh" runat="server">
    <ContentTemplate>
        <asp:ListView ID="lvItems" runat="server" DataSourceID="dsPublishQueue" OnDataBound="lvItems_DataBound" OnItemDataBound="lvItems_ItemDataBound">
            <LayoutTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                Item
                            </th>
                            <th>
                                Approver
                            </th>
                            <th>
                                Approval Date
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </tbody>
                    <tfoot id="tfoot" runat="server">
                        <tr>
                            <td colspan="3">
                                <uc1:UnorderedListDataPager ID="pgrResults" runat="server">
                                    <Fields>
                                        <asp:NextPreviousPagerField
                                            ShowPreviousPageButton="true" PreviousPageText="Prev" ButtonCssClass="prev"
                                            ShowNextPageButton="false"
                                            ShowFirstPageButton="false" FirstPageText="First"
                                            ShowLastPageButton="false" />
                                        <asp:NumericPagerField />
                                        <asp:NextPreviousPagerField
                                            ShowPreviousPageButton="false"
                                            ShowNextPageButton="true" NextPageText="Next" ButtonCssClass="next"
                                            ShowFirstPageButton="false"
                                            ShowLastPageButton="false" LastPageText="Last" />
                                    </Fields>
                                </uc1:UnorderedListDataPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:HyperLink ID="lnkItem" runat="server" />
                        <p class="path"><asp:Literal ID="litPath" runat="server" /></p>
                    </td>
                    <td>
                        <asp:Literal ID="litApprover" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="litApproved" runat="server" />
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <p>No items scheduled for publication</p>
            </EmptyDataTemplate>
        </asp:ListView>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:ObjectDataSource ID="dsPublishQueue" runat="server" EnablePaging="true"
    TypeName="Sitecore.Dashboard.Web.UI.Widgets.PublishQueueDataSource" 
    SelectMethod="GetRows" SelectCountMethod="GetCount" OnSelecting="dsPublishQueue_Selecting"
    StartRowIndexParameterName="startRowIndex" MaximumRowsParameterName="maximumRows">
    <SelectParameters>
        <asp:Parameter Name="sourceDb" Type="Object"/>
        <asp:Parameter Name="targetDb" Type="Object"/>
        <asp:Parameter Name="pageSize" Type="Int32"/>
    </SelectParameters>
</asp:ObjectDataSource>