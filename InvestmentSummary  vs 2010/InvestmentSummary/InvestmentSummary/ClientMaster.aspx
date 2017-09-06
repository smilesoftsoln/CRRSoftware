<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ClientMaster.aspx.cs" Inherits="InvestmentSummary.ClientMaster" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
<script type = "text/javascript">
$(document).ready(function () {
    $('#<%=GridView1.ClientID %>').Scrollable({
        ScrollHeight: 300
    });
    $('#<%=GridView2.ClientID %>').Scrollable({
        ScrollHeight: 300
    });
});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <table width="100%">
<tr>
<td class="style1">Select Branch:-</td>
<td class="style2">
    <asp:DropDownList ID="BranchDropDownList1" runat="server" DataSourceID="BranchSource" 
        DataTextField="BranchName" DataValueField="BranchName" 
        onselectedindexchanged="BranchDropDownList1_SelectedIndexChanged">
    </asp:DropDownList>
    </td>
<td class="style5">
    <asp:Button ID="Button4" runat="server" Text="Get Data" 
        onclick="Button4_Click" />
    </td>
<td class="style5">
    <asp:FileUpload ID="FileUpload1" runat="server" />
    </td>
<td>
    <asp:Button ID="Button7" runat="server" Text="Import" style="width: 56px" onclick="Button7_Click" 
         />
    </td>
<td  >
    <asp:Button ID="Button8" runat="server" Text="Upload" onclick="Button8_Click" />
    <asp:Button ID="Button3" runat="server" PostBackUrl="~/Admin.aspx" 
        Text="Back" />
    
    </td>
</tr>
<tr>
<td colspan="6" class="style3">
    
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
            CellPadding="4" DataKeyNames="ID" DataSourceID="CliientSqlDataSource1" 
            ForeColor="Black" GridLines="Vertical" 
            onselectedindexchanged="GridView1_SelectedIndexChanged">
            <RowStyle BackColor="#F7F7DE" />
            <Columns>
                <asp:CommandField HeaderText="Select"  SelectText="Edit" ShowSelectButton="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="ID" />
                <asp:BoundField DataField="ClientCode" HeaderText="ClientCode" 
                    SortExpression="ClientCode" />
                <asp:BoundField DataField="ClientName" HeaderText="ClientName" 
                    SortExpression="ClientName" />
                <asp:BoundField DataField="FamilyCode" HeaderText="FamilyCode" 
                    SortExpression="FamilyCode" />
                <asp:BoundField DataField="Branch" HeaderText="Branch" 
                    SortExpression="Branch" />
                <asp:BoundField DataField="RM" HeaderText="RM" SortExpression="RM" />
                 <asp:BoundField DataField="AddedDate" HeaderText="AddedDate" SortExpression="AddedDate" />
            </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
        <asp:SqlDataSource ID="CliientSqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
            SelectCommand="SELECT ID,ClientCode,ClientName,FamilyCode,Branch,UPPER(RM) as RM ,AddedDate FROM [ClientMaster] WHERE ([Branch] = @Branch)  order by AddedDate desc">
            <SelectParameters>
                <asp:ControlParameter ControlID="BranchDropDownList1" Name="Branch" 
                    PropertyName="SelectedValue" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    
   
        <asp:GridView ID="GridView2" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None">
            <RowStyle BackColor="#EFF3FB" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    
   
    </td>
</tr>
<tr>
<td class="style4">


    &nbsp;</td>
<td class="style4">


    ClientCode:</td>
<td class="style6" colspan="2">


    <asp:Label ID="lbbCliencodeLabel2" runat="server"></asp:Label>


</td>
<td class="style4">


    Branch:</td>
<td class="style4">


    <asp:DropDownList ID="Branch2DropDownList2" runat="server" DataSourceID="BranchSource" 
        DataTextField="BranchName" DataValueField="BranchName" AutoPostBack="True" 
        onselectedindexchanged="Branch2DropDownList2_SelectedIndexChanged" 
        ondatabound="Branch2DropDownList2_DataBound">
    </asp:DropDownList>


</td>
</tr>
<tr>
<td class="style4">


</td>
<td class="style4">


    ClientName:</td>
<td class="style6" colspan="2">


    <asp:Label ID="lblnameLabel2" runat="server"></asp:Label>


</td>
<td class="style4">


    RM:</td>
<td class="style4">


    <asp:DropDownList ID="RMDropDownList3" runat="server" 
        DataSourceID="RMSqlDataSource1" DataTextField="RM" DataValueField="RM" 
        ondatabound="RMDropDownList3_DataBound" 
        onselectedindexchanged="RMDropDownList3_SelectedIndexChanged">
    </asp:DropDownList>


    <asp:SqlDataSource ID="RMSqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
        SelectCommand="SELECT DISTINCT [RM] FROM [RM_Master]">
    </asp:SqlDataSource>


</td>
</tr>
<tr>
<td class="style4">


</td>
<td class="style4">


    FamilyCode:</td>
<td class="style6" colspan="2">


    <asp:TextBox ID="FamilyCodeTextBox1txt" runat="server"></asp:TextBox>


</td>
<td class="style4">


</td>
<td class="style4">


</td>
</tr>
<tr>
<td class="style4">


</td>
<td class="style4">


</td>
<td class="style6" colspan="2">


    <asp:Button ID="Button5" runat="server" Text="Save" onclick="Button5_Click" />


</td>
<td class="style4">


    <asp:Button ID="Button6" runat="server" Text="Export" onclick="Button6_Click" style="width: 56px" 
         />


</td>
<td class="style4">


</td>
</tr>
</table>
    <asp:SqlDataSource ID="BranchSource" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
        SelectCommand="SELECT DISTINCT [BranchName] FROM [SBCODE]">
    </asp:SqlDataSource>
    </asp:Content>
