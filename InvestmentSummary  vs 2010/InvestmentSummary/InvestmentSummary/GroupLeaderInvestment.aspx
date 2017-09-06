<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="GroupLeaderInvestment.aspx.cs" Inherits="InvestmentSummary.GroupLeaderInvestment" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%;">
            <tr>
                <td class="style1">
                    Select Date</td>
                <td class="style2">
                    <asp:DropDownList ID="DateDropDownList1" runat="server" AutoPostBack="True" 
                        DataSourceID="dateSqlDataSource1" DataTextField="IS_date" 
                        DataValueField="IS_date" Height="20px" Width="125px" 
                        onselectedindexchanged="DateDropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dateSqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                        SelectCommand="SELECT DISTINCT [IS_date] FROM [INVESTMENTSUMMARY]">
                    </asp:SqlDataSource>
                </td>
                <td>
                    Select Branch:<asp:DropDownList ID="BranchDropDownList1" runat="server" 
                        DataSourceID="SqlDataSource1" DataTextField="BranchName" 
                        DataValueField="BranchName" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                        SelectCommand="SELECT DISTINCT [BranchName] FROM [SBCODE]">
                    </asp:SqlDataSource>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                        Text="Get Report" />
                </td>
                <td>
                    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Export" />
                </td>
                <td>
                 <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" onclick="Button3_Click" /></td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                        ForeColor="Black" GridLines="Vertical">
                        <RowStyle BackColor="#F7F7DE" />
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
</asp:Content>
