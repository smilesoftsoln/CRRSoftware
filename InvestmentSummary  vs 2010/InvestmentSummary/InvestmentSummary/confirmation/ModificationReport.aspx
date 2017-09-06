<%@ Page Title="" Language="C#" MasterPageFile="~/confirmation/Site2.Master" AutoEventWireup="true" CodeBehind="ModificationReport.aspx.cs" Inherits="InvestmentSummary.confirmation.ModificationReport" %>


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
            width: 134px;
        }
        .style4
        {
            width: 47px;
        }
        .style5
        {
            width: 105px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
    <ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>

    <table style="width:100%;" align="center">
        <tr>
            <td class="style2">
                Select Date From</td>
            <td class="style3">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <ajaxToolkit:CalendarExtender ID="from" runat="server" Format="dd-MMM-yyyy" TargetControlID="TextBox1" ></ajaxToolkit:CalendarExtender>

            </td>
            <td class="style4">
                To</td>
            <td class="style3">
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                   <ajaxToolkit:CalendarExtender ID="to" runat="server" Format="dd-MMM-yyyy" TargetControlID="TextBox2" ></ajaxToolkit:CalendarExtender>

            </td>
            <td class="style5">
                <asp:Button ID="GetReportButton1" runat="server" Text="Get Report" 
                    onclick="GetReportButton1_Click" />
            </td>
            <td>
                <asp:Button ID="ExportButton2" runat="server" Text="Export" />
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="6">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="ID" DataSourceID="SqlDataSource1" style="text-align: center">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                            ReadOnly="True" SortExpression="ID" />
                        <asp:BoundField DataField="UpdateDate" HeaderText="UpdateDate" 
                            SortExpression="UpdateDate" />
                        <asp:BoundField DataField="ClientCode" HeaderText="ClientCode" 
                            SortExpression="ClientCode" />
                        <asp:BoundField DataField="Landline1" HeaderText="Landline1" 
                            SortExpression="Landline1" />
                        <asp:BoundField DataField="Landline2" HeaderText="Landline2" 
                            SortExpression="Landline2" />
                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" 
                            SortExpression="Mobile" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT * FROM [ContactModification] WHERE (([UpdateDate] &lt;= @UpdateDate) AND ([UpdateDate] &gt;= @UpdateDate2))">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="TextBox2" DbType="Date" Name="UpdateDate" 
                            PropertyName="Text" />
                        <asp:ControlParameter ControlID="TextBox1" DbType="Date" Name="UpdateDate2" 
                            PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style5">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
