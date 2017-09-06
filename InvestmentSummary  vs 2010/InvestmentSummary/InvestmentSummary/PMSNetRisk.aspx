﻿<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PMSNetRisk.aspx.cs" Inherits="InvestmentSummary.PMSNetRisk" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
<script type = "text/javascript">
$(document).ready(function () {
    $('#<%=GridView1.ClientID %>').Scrollable({
        ScrollHeight: 300
    });
     
});
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
<tr>
<td class="style1">Select File For PMS Net Risk: </td>
<td>
    <asp:FileUpload ID="FileUpload1" runat="server" />
    </td>
<td>
    <asp:Button ID="Button1" runat="server" Text="Upload" onclick="Button1_Click" />
    </td>
<td>
    <asp:Button ID="Button2" runat="server" Text="Update" onclick="Button2_Click" />
    </td>
    <td>
    <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" />
    </td>
</tr>
<tr>
<td colspan="4">
    <asp:GridView style=" font-size:small" ID="GridView1" runat="server" BackColor="White" 
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
</table>
</asp:Content>