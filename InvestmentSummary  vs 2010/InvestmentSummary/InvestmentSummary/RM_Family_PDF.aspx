<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RM_Family_PDF.aspx.cs" Inherits="InvestmentSummary.RM_Family_PDF" %>
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


    
 



    <table style="width: 100%;">
        <tr>
            <td class="style1">
                &nbsp;
                Logged In As:&nbsp; <asp:Label ID="Label2" runat="server" Text=""> </asp:Label></td>
            <td class="style2">
                &nbsp;
               
            </td>
            <td class="style3">
                Branch: <asp:Label ID="Label3" runat="server" Text=""></asp:Label></td>
            <td>
                &nbsp;
               
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
                <%--Search Name:---%>Search Client:-</td>
            <td class="style2">
                &nbsp;
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td class="style3">
                <asp:Button ID="Button1" runat="server" Text="Get Clients" 
                    onclick="Button1_Click" />
            </td>
            <td>
                <asp:Button ID="Button2" runat="server" PostBackUrl="~/BM_RM_Page.aspx" 
                    Text="Back" />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" class="style1" colspan="4">
                  <asp:GridView ID="GridView1" 
                        runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged">
                    <RowStyle BackColor="#F7F7DE" />
                    <Columns>
                        <asp:CommandField HeaderText="Select" ShowHeader="True" 
                            ShowSelectButton="True" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView> 
&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>






</asp:Content>
