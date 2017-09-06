<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MFSubBrokerMaster.aspx.cs" Inherits="InvestmentSummary.MFSubBrokerMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="mf_br_id" 
        DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
        <AlternatingItemTemplate>
            <tr style="background-color: #FFF8DC;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="mf_br_idLabel" runat="server" Text='<%# Eval("mf_br_id") %>' />
                </td>
                <td>
                    <asp:Label ID="subbrokerLabel" runat="server" Text='<%# Eval("subbroker") %>' />
                </td>
                <td>
                    <asp:Label ID="branchLabel" runat="server" Text='<%# Eval("branch") %>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <tr style="background-color: #008A8C; color: #FFFFFF;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Cancel" />
                </td>
                <td>
                    <asp:Label ID="mf_br_idLabel1" runat="server" Text='<%# Eval("mf_br_id") %>' />
                </td>
                <td>
                    <asp:TextBox ID="subbrokerTextBox" runat="server" 
                        Text='<%# Bind("subbroker") %>' />
                </td>
                <td>
                    <asp:TextBox ID="branchTextBox" runat="server" Text='<%# Bind("branch") %>' />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" 
                style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;">
                <tr>
                    <td>
                        No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr style="">
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" 
                        Text="Insert" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Clear" />
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:TextBox ID="subbrokerTextBox" runat="server" 
                        Text='<%# Bind("subbroker") %>' />
                </td>
                <td>
                    <asp:TextBox ID="branchTextBox" runat="server" Text='<%# Bind("branch") %>' />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style="background-color: #DCDCDC; color: #000000;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="mf_br_idLabel" runat="server" Text='<%# Eval("mf_br_id") %>' />
                </td>
                <td>
                    <asp:Label ID="subbrokerLabel" runat="server" Text='<%# Eval("subbroker") %>' />
                </td>
                <td>
                    <asp:Label ID="branchLabel" runat="server" Text='<%# Eval("branch") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="1" 
                            style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                            <tr runat="server" style="background-color: #DCDCDC; color: #000000;">
                                <th runat="server">
                                </th>
                                <th runat="server">
                                    mf_br_id</th>
                                <th runat="server">
                                    subbroker</th>
                                <th runat="server">
                                    branch</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" 
                        
                        style="text-align: center;background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif;color: #000000;">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <tr style="background-color: #008A8C; font-weight: bold;color: #FFFFFF;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="mf_br_idLabel" runat="server" Text='<%# Eval("mf_br_id") %>' />
                </td>
                <td>
                    <asp:Label ID="subbrokerLabel" runat="server" Text='<%# Eval("subbroker") %>' />
                </td>
                <td>
                    <asp:Label ID="branchLabel" runat="server" Text='<%# Eval("branch") %>' />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
        DeleteCommand="DELETE FROM [MFBranch] WHERE [mf_br_id] = @mf_br_id" 
        InsertCommand="INSERT INTO [MFBranch] ([subbroker], [branch]) VALUES (@subbroker, @branch)" 
        SelectCommand="SELECT * FROM [MFBranch]" 
        
    UpdateCommand="UPDATE [MFBranch] SET [subbroker] = @subbroker, [branch] = @branch WHERE [mf_br_id] = @mf_br_id">
        <DeleteParameters>
            <asp:Parameter Name="mf_br_id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="subbroker" Type="String" />
            <asp:Parameter Name="branch" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="subbroker" Type="String" />
            <asp:Parameter Name="branch" Type="String" />
            <asp:Parameter Name="mf_br_id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
