<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Margin_Funding.aspx.cs" Inherits="InvestmentSummary.Margin_Funding" %>
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
<td class="style1">Select File For Margin Funding Net Risk:</td>
<td>
    <asp:FileUpload ID="FileUpload1" runat="server" />
    </td>
<td>
    <asp:Button ID="Button1" runat="server" Text="Upload" onclick="Button1_Click" />
    </td>
<td>
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Update" />
    </td>
<td>
    <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" />
    </td>
</tr>
<tr>
<td colspan="5">
    <asp:GridView ID="GridView1" runat="server" BackColor="White" style=" font-size:small"
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
