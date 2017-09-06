<%@ Page Title="" Language="C#" MasterPageFile="~/confirmation/Site2.Master" AutoEventWireup="true" CodeBehind="UserEntriesExportUser.aspx.cs" Inherits="InvestmentSummary.confirmation.UserEntriesExportUser" %>




<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
 









<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
        }
        .style3
        {
            width: 139px;
        }
        .style5
        {
            width: 129px;
        }
        .style6
        {
            width: 95px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>

    <table style="width:100%;" align="center">
        <tr>
            <td class="style2">
                Select Date</td>
            <td class="style5">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                  
           <ajaxToolkit:CalendarExtender ID="cal1" runat="server" Format="dd-MMM-yyyy" TargetControlID="TextBox1" />
         
            </td>
            <td class="style3">
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>  
           <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="TextBox2" />
         

                <asp:Label ID="DateLabel2" runat="server" Visible="false"></asp:Label>
            </td>
            <td class="style6">
                &nbsp;User:-&nbsp;<asp:Label ID="USerLabel2" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Get Report" />
                <asp:Button ID="Button2" runat="server" Text="Export" onclick="Button2_Click" />
            </td>
        </tr>
        <tr>
            <td runat="server"  class="style2" colspan="6">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="ReportDataSource1" style="text-align: center">
                    <Columns>
                      <%--  <asp:BoundField DataField="UserName" HeaderText="UserName" 
                            SortExpression="UserName" />
                        <asp:BoundField DataField="Dept_Branch" HeaderText="Dept_Branch" 
                            SortExpression="Dept_Branch" />
                        <asp:BoundField DataField="TerminalNo" HeaderText="TerminalNo" 
                            SortExpression="TerminalNo" />--%>
                        <asp:BoundField DataField="ClientCode" HeaderText="ClientCode" 
                            SortExpression="ClientCode" />
                        <asp:BoundField DataField="ClientName" HeaderText="ClientName" 
                            SortExpression="ClientName" />
                        <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" 
                            SortExpression="ContactNo" />
                        <asp:BoundField DataField="ContactType" HeaderText="ContactType" 
                            SortExpression="ContactType" />
                        <asp:BoundField DataField="ConfirmationDate" HeaderText="ConfirmationDate" 
                            SortExpression="ConfirmationDate" />
                        <asp:BoundField DataField="Segment" HeaderText="Segment" 
                            SortExpression="Segment" />
                        <asp:BoundField DataField="GivenTo" HeaderText="GivenTo" 
                            SortExpression="GivenTo" />
                        <asp:BoundField DataField="OtherRemark" HeaderText="OtherRemark" 
                            SortExpression="OtherRemark" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="ReportDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [UserName], [Dept_Branch], [TerminalNo], [ClientCode], [ClientName], [ContactNo], [ContactType], [ConfirmationDate], [Segment], [GivenTo], [OtherRemark] FROM [Confirmation] WHERE (([ConfirmationDate] &lt;= @ConfirmationDate) AND ([ConfirmationDate] &gt;= @ConfirmationDate2) AND ([UserName] = @UserName)) ORDER BY [ConfirmationDate]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DateLabel2" Name="ConfirmationDate" 
                            PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter ControlID="TextBox1" Name="ConfirmationDate2" 
                            PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter ControlID="USerLabel2" Name="UserName" 
                            PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style6">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
