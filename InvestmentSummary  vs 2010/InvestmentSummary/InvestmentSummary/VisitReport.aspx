<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="VisitReport.aspx.cs" Inherits="InvestmentSummary.VisitReport" %>
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







    <style type="text/css">
        .style2
        {
            width: 170px;
        }
        .style3
        {
            width: 94px;
        }
    </style>







</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<center>
    <table style="width:100%;">
        <tr>
            <td class="style3">
                Total Familes:-<asp:Label ID="lblTotalFamily" runat="server"></asp:Label>
            </td>
            <td class="style2">
                Visits Done:-<asp:Label ID="lblVisitDone" runat="server"></asp:Label>
            </td>
            <td>
                Visits Pending:-<asp:Label ID="lblPending" runat="server"></asp:Label>
            </td>
            <td>
 <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" />

            </td>
        </tr>
        <tr>
            <td colspan="4">

<asp:GridView ID="GridView1" runat="server" BackColor="White" 
        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;</td>
            <td class="style2">
    <asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
        Text="Export To Excel" />
            </td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
    </table>
    <br />
    </center>

    




</asp:Content>
