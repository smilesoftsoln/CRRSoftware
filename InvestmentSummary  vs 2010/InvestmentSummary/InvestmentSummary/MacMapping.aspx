<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MacMapping.aspx.cs" Inherits="InvestmentSummary.MacMapping" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table width="100%">
<tr>
<td class="style1"></td>
<td>
    &nbsp;</td>
<td>
    &nbsp;</td>
<td>
    &nbsp;</td>
<td>
    <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" />
    </td>
</tr>
<tr>
<td colspan="5">
<center>
    <asp:ListView style=" font-size:small" ID="ListView1" runat="server" DataKeyNames="macidid" 
        DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
        <ItemTemplate>
            <tr style="color:#333333; background-color:#E0FFFF; ">
                <td >
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="macididLabel" runat="server" Text='<%# Eval("macidid") %>' />
                </td>
                <td>
                    <asp:Label ID="macLabel" runat="server" Text='<%# Eval("mac") %>' />
                </td>
                <td  >
                    <asp:Label ID="PC_NameLabel" runat="server" Text='<%# Eval("PC_Name") %>' />
                </td>
                <td>
                    <asp:Label ID="BranchLabel" runat="server" Text='<%# Eval("Branch") %>' />
                </td>
                <td >
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="color:#284775; background-color:#FFFFFF; ">
                <td >
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td   >
                    <asp:Label ID="macididLabel" runat="server" Text='<%# Eval("macidid") %>' />
                </td>
                <td  >
                    <asp:Label ID="macLabel" runat="server" Text='<%# Eval("mac") %>' />
                </td>
                <td  >
                    <asp:Label ID="PC_NameLabel" runat="server" Text='<%# Eval("PC_Name") %>' />
                </td>
                <td>
                    <asp:Label ID="BranchLabel" runat="server" Text='<%# Eval("Branch") %>' />
                </td>
                <td  >
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
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
                    <asp:TextBox ID="macTextBox" runat="server" Text='<%# Bind("mac") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PC_NameTextBox" runat="server" Text='<%# Bind("PC_Name") %>' />
                </td>
                <td>
                    <asp:TextBox ID="BranchTextBox" runat="server" Text='<%# Bind("Branch") %>' />
                </td>
                <td>
                    <asp:TextBox ID="UserNameTextBox" runat="server" 
                        Text='<%# Bind("UserName") %>' />
                </td>
            </tr>
        </InsertItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table ID="itemPlaceholderContainer" runat="server" border="1" 
                            style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                            <tr runat="server" style="background-color: #E0FFFF;color: #333333;">
                                <th runat="server">
                                </th>
                                <th runat="server">
                                    ID</th>
                                <th runat="server">
                                   MAC ID</th>
                                <th runat="server">
                                    PC Name</th>
                                <th runat="server">
                                    Branch</th>
                                <th runat="server">
                                    User Name</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" 
                        style="text-align: center;background-color: #5D7B9D;font-family: Verdana, Arial, Helvetica, sans-serif;color: #FFFFFF">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <EditItemTemplate>
            <tr style="background-color: #999999;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Cancel" />
                </td>
                <td>
                    <asp:Label ID="macididLabel1" runat="server" Text='<%# Eval("macidid") %>' />
                </td>
                <td>
                    <asp:TextBox ID="macTextBox" runat="server" Text='<%# Bind("mac") %>' />
                </td>
                <td>
                    <asp:TextBox ID="PC_NameTextBox" runat="server" Text='<%# Bind("PC_Name") %>' />
                </td>
                <td>
                    <asp:TextBox ID="BranchTextBox" runat="server" Text='<%# Bind("Branch") %>' />
                </td>
                <td>
                    <asp:TextBox ID="UserNameTextBox" runat="server" 
                        Text='<%# Bind("UserName") %>' />
                </td>
            </tr>
        </EditItemTemplate>
        <SelectedItemTemplate>
            <tr style="background-color: #E2DED6;font-weight: bold;color: #333333;">
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" 
                        Text="Delete" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="macididLabel" runat="server" Text='<%# Eval("macidid") %>' />
                </td>
                <td>
                    <asp:Label ID="macLabel" runat="server" Text='<%# Eval("mac") %>' />
                </td>
                <td>
                    <asp:Label ID="PC_NameLabel" runat="server" Text='<%# Eval("PC_Name") %>' />
                </td>
                <td>
                    <asp:Label ID="BranchLabel" runat="server" Text='<%# Eval("Branch") %>' />
                </td>
                <td>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView></center>
    
    </td>
</tr>
</table>
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
        DeleteCommand="DELETE FROM [macmapping] WHERE [macidid] = @macidid" 
        InsertCommand="INSERT INTO [macmapping] ([mac], [PC_Name], [Branch], [UserName]) VALUES (@mac, @PC_Name, @Branch, @UserName)" 
        SelectCommand="SELECT * FROM [macmapping]" 
        
        UpdateCommand="UPDATE [macmapping] SET [mac] = @mac, [PC_Name] = @PC_Name, [Branch] = @Branch, [UserName] = @UserName WHERE [macidid] = @macidid">
        <DeleteParameters>
            <asp:Parameter Name="macidid" Type="Int64" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="mac" Type="String" />
            <asp:Parameter Name="PC_Name" Type="String" />
            <asp:Parameter Name="Branch" Type="String" />
            <asp:Parameter Name="UserName" Type="String" />
            <asp:Parameter Name="macidid" Type="Int64" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="mac" Type="String" />
            <asp:Parameter Name="PC_Name" Type="String" />
            <asp:Parameter Name="Branch" Type="String" />
            <asp:Parameter Name="UserName" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>


</asp:Content>
