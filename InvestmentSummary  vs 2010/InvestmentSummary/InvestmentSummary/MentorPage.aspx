<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MentorPage.aspx.cs" Inherits="InvestmentSummary.MentorPage" %>
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
<center>

    <table style="width: 100%;">
        <tr>
            <td class="style2">
                &nbsp;
                Select Branch:-<asp:SqlDataSource ID="BranchSource" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [Branch] FROM [MentorMaster] WHERE ([Mentor] = @Mentor) ORDER BY [Branch]">
                    <SelectParameters>
                        <asp:SessionParameter Name="Mentor" SessionField="login" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                    DataSourceID="BranchSource" DataTextField="Branch" DataValueField="Branch">
                </asp:DropDownList>
            </td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;
                Select RM:-<asp:SqlDataSource ID="RMSqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [username] FROM [UserMaster] WHERE (([role] &lt;&gt; @role) AND ([role] &lt;&gt; @role2) AND ([Branch] = @Branch))">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Admin" Name="role" Type="String" />
                        <asp:Parameter DefaultValue="MNG" Name="role2" Type="String" />
                        <asp:ControlParameter ControlID="DropDownList1" Name="Branch" 
                            PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
                    DataSourceID="RMSqlDataSource1" DataTextField="username" 
                    DataValueField="username" ondatabound="DropDownList2_DataBound" 
                    onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="style3">
                <asp:Button ID="Button1" runat="server" Text="Export" onclick="Button1_Click" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2">
                Total Families:<asp:Label ID="CountLabel4" runat="server" 
                    style="font-weight: 700"></asp:Label>
               
            </td>
            <td class="style2">
                Visits Done:<asp:Label ID="VisitDoneLabel1" runat="server" 
                     style="font-weight: 700"></asp:Label>
            </td>
            <td class="style3">
                Remaining:<asp:Label 
                                 ID="RemainigLabel1" runat="server" style="font-weight: 700"></asp:Label>
            </td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2" colspan="5">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4"  DataSourceID="VisitSqlDataSource1" 
                    ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                         <asp:BoundField DataField="VisitDate" HeaderText="VisitDate" 
                            SortExpression="VisitDate" />
                        <asp:BoundField DataField="BM_RM_Name" HeaderText="BM_RM_Name" 
                            SortExpression="BM_RM_Name" />
                        <asp:BoundField DataField="ClientCode" HeaderText="ClientCode" 
                            SortExpression="ClientCode" />
                        <asp:BoundField DataField="ClientName" HeaderText="ClientName" 
                            SortExpression="ClientName" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" 
                            SortExpression="Remark" />
                        <asp:BoundField DataField="Status" HeaderText="Status" 
                            SortExpression="Status" />
                        <asp:BoundField DataField="Branch" HeaderText="Branch" 
                            SortExpression="Branch" />
                        <asp:BoundField DataField="Location" HeaderText="Location" 
                            SortExpression="Location" />
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
                </asp:GridView>
                <asp:SqlDataSource ID="VisitSqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT RemDate as VisitDate,ClientCode,ClientName,BM_RM_Name,Remark,Location,Branch,Status FROM [Reminder] WHERE (([Status] = @Status) AND ([BM_RM_Name] = @BM_RM_Name))">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="Visit Done" Name="Status" Type="String" />
                        <asp:ControlParameter ControlID="DropDownList2" Name="BM_RM_Name" 
                            PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                &nbsp;
                &nbsp;
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="2">
                &nbsp;
            </td>
            <td class="style3" colspan="2">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>


</center>
</asp:Content>
