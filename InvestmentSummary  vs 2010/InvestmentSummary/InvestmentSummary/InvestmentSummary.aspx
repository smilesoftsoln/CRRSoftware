<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="InvestmentSummary.aspx.cs" Inherits="InvestmentSummary.InvestmentSummary" Title="Untitled Page" %>
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
       /*.datagrid {
width: 100%;
height: 399px;
overflow: auto;
 
}
 .theader {
display:none;
 	}*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




 
        <table >
            <tr>
                <td  >
                    Select Date</td>
                <td  >
                    <asp:DropDownList ID="DateDropDownList1" runat="server" AutoPostBack="True" 
                        DataSourceID="dateSqlDataSource1" DataTextField="IS_date" 
                        DataValueField="IS_date" Height="20px" Width="125px">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dateSqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                        SelectCommand="SELECT DISTINCT [IS_date] FROM [INVESTMENTSUMMARY] order by IS_date desc">
                    </asp:SqlDataSource>
                </td>
                <td>
                    Select Branch:<asp:DropDownList ID="BranchDropDownList1" runat="server" 
                        DataSourceID="SqlDataSource1" DataTextField="BranchName" 
                        DataValueField="BranchName" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
                        ondatabound="BranchDropDownList1_DataBound">
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
                    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Export All" />
                </td>
                <td>
                 <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" onclick="Button3_Click" /></td>
                 <td>
                    <asp:Button ID="Button4" runat="server"   Text="Export Summary" 
                         onclick="Button4_Click" />
                         <a runat="server" ID="lblBook3" href="http://10.56.65.45:81/Book3.xlsx"
>First Click on Export Summary to  Open Latest Summary File</a>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="8">
          <%-- <table cellspacing="0" cellpadding="4" rules="cols" border="1" 
                        id="ctl00_ContentPlaceHolder1_GridView1" 
                        style="border: 0px None #DEDFDE; color:White; background-color:Black; border-collapse:collapse; font-size:small;  ">
		<tr >
			<th style="width:70px" >ClientCode</th><th style="width:70px">Family</th><th style="width:300px">ClientName</th><th style="width:70px">Branch</th><th style="width:70px">FNO</th><th style="width:70px">CASH</th><th style="width:70px">PMS</th><th style="width:70px">MF</th><th style="width:70px">Total</th><th style="width:70px">FamilyTotal</th>
		</tr></table>--%>
       <div  >
                  <asp:GridView style=" font-size:small" ID="GridView1" runat="server" BackColor="White" 
                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" Width="100%" 
                        ForeColor="Black" GridLines="Vertical">
                        <RowStyle Wrap="true"  BackColor="#F7F7DE" Width="70px"  />
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
 <HeaderStyle  BackColor="Black" ForeColor="White"   />                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    </div>
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
