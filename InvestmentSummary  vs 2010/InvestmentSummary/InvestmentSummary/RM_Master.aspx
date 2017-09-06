<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RM_Master.aspx.cs" Inherits="InvestmentSummary.RM_Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" /> <asp:ListView ID="ListView1" runat="server" DataKeyNames="RMID" 
        DataSourceID="SqlDataSource1" InsertItemPosition="LastItem">
        <ItemTemplate>
            <tr style="background-color: #DCDCDC; color: #000000;">
                <td>
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" 
                        Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="RMIDLabel" runat="server" Text='<%# Eval("RMID") %>' />
                </td>
                <td>
                    <asp:Label ID="RMLabel" runat="server" Text='<%# Eval("RM") %>' />
                </td>
                <td>
                    <asp:Label ID="MobileNoLabel" runat="server" Text='<%# Eval("MobileNo") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color: #FFF8DC;">
                <td>
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" 
                        Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="RMIDLabel" runat="server" Text='<%# Eval("RMID") %>' />
                </td>
                <td>
                    <asp:Label ID="RMLabel" runat="server" Text='<%# Eval("RM") %>' />
                </td>
                <td>
                    <asp:Label ID="MobileNoLabel" runat="server" Text='<%# Eval("MobileNo") %>' />
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
                    <asp:TextBox ID="RMTextBox" runat="server" Text='<%# Bind("RM") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MobileNoTextBox" runat="server" 
                        Text='<%# Bind("MobileNo") %>' />
                </td>
            </tr>
        </InsertItemTemplate>
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
                                    RMID</th>
                                <th runat="server">
                                    RM</th>
                                <th runat="server">
                                    MobileNo</th>
                            </tr>
                            <tr ID="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" 
                        
                        style="text-align: center;background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif;color: #000000">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <EditItemTemplate>
            <tr style="background-color: #008A8C; color: #FFFFFF;">
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" 
                        Text="Update" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" 
                        Text="Cancel" />
                </td>
                <td>
                    <asp:Label ID="RMIDLabel1" runat="server" Text='<%# Eval("RMID") %>' />
                </td>
                <td>
                    <asp:TextBox ID="RMTextBox" runat="server" Text='<%# Bind("RM") %>' />
                </td>
                <td>
                    <asp:TextBox ID="MobileNoTextBox" runat="server" 
                        Text='<%# Bind("MobileNo") %>' />
                </td>
            </tr>
        </EditItemTemplate>
        <SelectedItemTemplate>
            <tr style="background-color: #008A8C; font-weight: bold;color: #FFFFFF;">
                <td>
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" 
                        Text="Edit" />
                </td>
                <td>
                    <asp:Label ID="RMIDLabel" runat="server" Text='<%# Eval("RMID") %>' />
                </td>
                <td>
                    <asp:Label ID="RMLabel" runat="server" Text='<%# Eval("RM") %>' />
                </td>
                <td>
                    <asp:Label ID="MobileNoLabel" runat="server" Text='<%# Eval("MobileNo") %>' />
                </td>
            </tr>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
        DeleteCommand="DELETE FROM [RM_Master] WHERE [RMID] = @RMID" 
        InsertCommand="INSERT INTO [RM_Master] ([RM], [MobileNo]) VALUES (@RM, @MobileNo)" 
        SelectCommand="SELECT * FROM [RM_Master]" 
        
        UpdateCommand="UPDATE [RM_Master] SET [RM] = @RM, [MobileNo] = @MobileNo WHERE [RMID] = @RMID">
        <DeleteParameters>
            <asp:Parameter Name="RMID" Type="Int64" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="RM" Type="String" />
            <asp:Parameter Name="MobileNo" Type="String" />
            <asp:Parameter Name="RMID" Type="Int64" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="RM" Type="String" />
            <asp:Parameter Name="MobileNo" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
