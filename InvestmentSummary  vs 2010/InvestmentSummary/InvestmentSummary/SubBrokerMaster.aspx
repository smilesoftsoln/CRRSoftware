<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SubBrokerMaster.aspx.cs" Inherits="InvestmentSummary.SubBrokerMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="SubBrokerID" 
        DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
        <AlternatingItemTemplate>
            <tr style="background-color: #FAFAD2;color: #284775;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="SubBrokerIDLabel" runat="server" 
                        Text='<%# Eval("SubBrokerID") %>' />
                </td>
                <td>
                    <asp:Label ID="SubbrokerLabel" runat="server" Text='<%# Eval("Subbroker") %>' />
                </td>
                <td>
                    <asp:Label ID="BranchNameLabel" runat="server" 
                        Text='<%# Eval("BranchName") %>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <tr style="background-color: #FFCC66;color: #000080;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Cancel" />
                </td>
                <td>
                    <asp:Label ID="SubBrokerIDLabel1" runat="server" 
                        Text='<%# Eval("SubBrokerID") %>' />
                </td>
                <td>
                    <asp:TextBox ID="SubbrokerTextBox" runat="server" 
                        Text='<%# Bind("Subbroker") %>' />
                </td>
                <td>
                    <asp:TextBox ID="BranchNameTextBox" runat="server" 
                        Text='<%# Bind("BranchName") %>' />
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
                    <asp:TextBox ID="SubbrokerTextBox" runat="server" 
                        Text='<%# Bind("Subbroker") %>' />
                </td>
                <td>
                    <asp:TextBox ID="BranchNameTextBox" runat="server" 
                        Text='<%# Bind("BranchName") %>' />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr style="background-color: #FFFBD6;color: #333333;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="SubBrokerIDLabel" runat="server" 
                        Text='<%# Eval("SubBrokerID") %>' />
                </td>
                <td>
                    <asp:Label ID="SubbrokerLabel" runat="server" Text='<%# Eval("Subbroker") %>' />
                </td>
                <td>
                    <asp:Label ID="BranchNameLabel" runat="server" 
                        Text='<%# Eval("BranchName") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="1" 
                            style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                            <tr runat="server" style="background-color: #FFFBD6;color: #333333;">
                                <th runat="server">
                                </th>
                                <th runat="server">
                                    SubBrokerID</th>
                                <th runat="server">
                                    Subbroker</th>
                                <th runat="server">
                                    BranchName</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" 
                        style="text-align: center;background-color: #FFCC66;font-family: Verdana, Arial, Helvetica, sans-serif;color: #333333;">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <tr style="background-color: #FFCC66;font-weight: bold;color: #000080;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="SubBrokerIDLabel" runat="server" 
                        Text='<%# Eval("SubBrokerID") %>' />
                </td>
                <td>
                    <asp:Label ID="SubbrokerLabel" runat="server" Text='<%# Eval("Subbroker") %>' />
                </td>
                <td>
                    <asp:Label ID="BranchNameLabel" runat="server" 
                        Text='<%# Eval("BranchName") %>' />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
        DeleteCommand="DELETE FROM [SBCODE] WHERE [SubBrokerID] = @SubBrokerID" 
        InsertCommand="INSERT INTO [SBCODE] ([Subbroker], [BranchName]) VALUES (@Subbroker, @BranchName)" 
        SelectCommand="SELECT * FROM [SBCODE]" 
        UpdateCommand="UPDATE [SBCODE] SET [Subbroker] = @Subbroker, [BranchName] = @BranchName WHERE [SubBrokerID] = @SubBrokerID">
        <DeleteParameters>
            <asp:Parameter Name="SubBrokerID" Type="Int64" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Subbroker" Type="String" />
            <asp:Parameter Name="BranchName" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Subbroker" Type="String" />
            <asp:Parameter Name="BranchName" Type="String" />
            <asp:Parameter Name="SubBrokerID" Type="Int64" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
