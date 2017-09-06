<%@ Page Title="" Language="C#" MasterPageFile="~/confirmation/Site2.Master" AutoEventWireup="true" CodeBehind="UserEntriesExport.aspx.cs" Inherits="InvestmentSummary.confirmation.UserEntriesExport" %>




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

    <table style="width:100%;">
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
         

                <asp:Label ID="DateLabel2" runat="server"></asp:Label>
            </td>
            <%--<td class="style6">
                &nbsp;Select User&nbsp;</td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server" 
                    DataSourceID="USerDataSource1" DataTextField="UserName" 
                    DataValueField="UserName" ondatabound="DropDownList1_DataBound">
                </asp:DropDownList>
                <asp:SqlDataSource ID="USerDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [UserName] FROM [Confirmation]">
                </asp:SqlDataSource>
            </td>--%>
            <td>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Get Report" />
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Export" />
            </td>
        </tr>
        <tr>
            <td runat="server" class="style2" colspan="6">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="ReportDataSource1">
                    <Columns>
                        <asp:BoundField DataField="UserName" HeaderText="UserName" 
                            SortExpression="UserName" />
                        <asp:BoundField DataField="Dept_Branch" HeaderText="Dept_Branch" 
                            SortExpression="Dept_Branch" />
                        <asp:BoundField DataField="TerminalNo" HeaderText="TerminalNo" 
                            SortExpression="TerminalNo" />
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
                              <asp:BoundField DataField="ReasonForPending" HeaderText="ReasonForPending" 
                            SortExpression="ReasonForPending" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="ReportDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [UserName], [Dept_Branch], [TerminalNo], [ClientCode], [ClientName], [ContactNo], [ContactType], [ConfirmationDate], [Segment], [GivenTo], [OtherRemark],[ReasonForPending] FROM [Confirmation] WHERE (([ConfirmationDate] &lt;= @ConfirmationDate) AND ([ConfirmationDate] &gt;= @ConfirmationDate2) ) ORDER BY [ConfirmationDate]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DateLabel2" Name="ConfirmationDate" 
                            PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter ControlID="TextBox1" Name="ConfirmationDate2" 
                            PropertyName="Text" Type="DateTime" />
                       
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
