<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="BM_RM_Page.aspx.cs" Inherits="InvestmentSummary.BM_RM_Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
<script type = "text/javascript">
$(document).ready(function () {
    $('#<%=GridView1.ClientID %>').Scrollable({
        ScrollHeight: 300
    });

});
$(document).ready(function () {
    $('#<%=GridView2.ClientID %>').Scrollable({
        ScrollHeight: 300
    });

});
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
 



    <table style="width: 100%;">
        <tr>
            <td  >
                &nbsp;
                Logged In As:&nbsp;&nbsp; <asp:Label ID="Label2" runat="server" Text=""> </asp:Label></td>
            <td >
                &nbsp;
               
                Total Families:<asp:Label ID="CountLabel4" runat="server" 
                    style="font-weight: 700"></asp:Label>
               
            </td>
            <td >
                 Visits Done:<asp:Label ID="VisitDoneLabel1" runat="server" 
                     style="font-weight: 700"></asp:Label>
          </td>
            <td >
                             Remaining   Visits:<asp:Label 
                                 ID="RemainigLabel1" runat="server" style="font-weight: 700"></asp:Label>
</td>
            <td  >
                Branch: <asp:Label ID="Label3" runat="server" Text=""></asp:Label></td>
            <td  >
                <asp:Label ID="Label4" runat="server" Text="RM"></asp:Label>
            </td>
            <td>
                &nbsp;
               
               <asp:DropDownList ID="RMDropDownList1" runat="server" AutoPostBack="True" 
                      DataTextField="username" DataValueField="username" 
                    onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                </asp:DropDownList>
              <%--   <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [username] FROM [UserMaster] WHERE ([Branch] = @Branch) ">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label3" Name="Branch" PropertyName="Text" 
                            Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>--%>
               
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
                <%--Search Name:---%></td>
            <td class="style2" colspan="3">
                &nbsp;
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td class="style3" colspan="2">
                <asp:Button ID="Button1" runat="server" Text="Get Clients" 
                    onclick="Button1_Click" />
                <asp:Button ID="Button2" runat="server" Text="Search Clients" 
                    PostBackUrl="~/RM_Family_PDF.aspx" />
            </td>
            <td>
                <asp:Button ID="Button3" runat="server" Text="Add Reminders/Visit Remarks" 
                    PostBackUrl="~/ReminderSetting.aspx" onclick="Button3_Click" />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td   class="style1" colspan="7" >
                  <asp:GridView ID="GridView1" 
                        runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                      onrowediting="GridView1_RowEditing">
                    <RowStyle BackColor="#F7F7DE" />
                    <Columns>
                        <asp:CommandField HeaderText="Reminder/PDF" ShowHeader="True"  SelectText="PDF"
                            ShowSelectButton="True" ShowEditButton="true" EditText="Reminder" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView> 
&nbsp;&nbsp;
             
          
                <asp:SqlDataSource ID="ReminderDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    
                    
                      SelectCommand="SELECT RemID, RemDate,UPPER(BM_RM_Name) AS BM_RM_Name , ClientCode, ClientName, Remark, Status, Branch FROM Reminder WHERE (BM_RM_Name = @BM_RM_Name) AND (Status = N'Later On')">
                    <SelectParameters>
                        <asp:SessionParameter Name="BM_RM_Name" SessionField="login" Type="String" />
                         
                    </SelectParameters>
                </asp:SqlDataSource>
                 
            </td>
        </tr>
        <tr>
            <td class="style1">
                Reminders</td>
            <td class="style2" colspan="3">
                &nbsp;</td>
            <td class="style3" colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1" colspan="7">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="RemID" DataSourceID="ReminderDataSource1" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" 
                    onselectedindexchanged="GridView2_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" />
                 <Columns>
                     <asp:CommandField ShowSelectButton="True" />
                     <asp:BoundField DataField="RemID" HeaderText="RemID" InsertVisible="False" 
                         ReadOnly="True" SortExpression="RemID" />
                     <asp:BoundField DataField="RemDate" HeaderText="RemDate" 
                         SortExpression="RemDate" />
                     <asp:BoundField DataField="BM_RM_Name" HeaderText="BM_RM_Name" 
                         SortExpression="BM_RM_Name" />
                     <asp:BoundField DataField="ClientCode" HeaderText="FamilyCode" 
                         SortExpression="ClientCode" />
                     <asp:BoundField DataField="ClientName" HeaderText="ClientName" 
                         SortExpression="ClientName" />
                     <asp:BoundField DataField="Remark" HeaderText="Remark" 
                         SortExpression="Remark" />
                     <asp:BoundField DataField="Status" HeaderText="Status" 
                         SortExpression="Status" />
                     <asp:BoundField DataField="Branch" HeaderText="Branch" 
                         SortExpression="Branch" />
                 </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                  </asp:GridView>&nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2" colspan="3">
                &nbsp;</td>
            <td class="style3" colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2" colspan="3">
                &nbsp;</td>
            <td class="style3" colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2" colspan="3">
                &nbsp;</td>
            <td class="style3" colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2" colspan="3">
                &nbsp;</td>
            <td class="style3" colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>






</asp:Content>
