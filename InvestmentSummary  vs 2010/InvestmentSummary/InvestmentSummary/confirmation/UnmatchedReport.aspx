<%@ Page Title="" EnableEventValidation="false"           Language="C#" MasterPageFile="Site2.Master" AutoEventWireup="true" CodeBehind="UnmatchedReport.aspx.cs" Inherits="InvestmentSummary.confirmation.UnmatchedReport" %>


<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
 



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 139px;
        }
        .style4
        {
            width: 102px;
        }
        .style5
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>
  <table style="width:100%;">
        <tr>
            <td class="style5">
                Select Date  From  <asp:TextBox ID="DateTextBox1" runat="server" 
                    ontextchanged="DateTextBox1_TextChanged"></asp:TextBox> 
                </td>
            <td class="style5">
                To <asp:TextBox ID="DateTextBox2" runat="server"></asp:TextBox>   <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" TargetControlID="DateTextBox2" />
         
               </td>
            <td class="style5">
                <asp:RadioButtonList ID="WhereRadioButtonList1" runat="server" Font-Size="Smaller" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>UnConfirmed</asp:ListItem>
                    <asp:ListItem>Confirmation To Other Than Self</asp:ListItem>
                    <asp:ListItem>Other Remark</asp:ListItem>
                    <asp:ListItem>UnRegistered Number</asp:ListItem>
                    
                    <asp:ListItem>All Confirmed</asp:ListItem>
                    
                </asp:RadioButtonList>
            </td>
            <td class="style3">
              
           
           
           <ajaxToolkit:CalendarExtender ID="cal1" runat="server" Format="dd-MMM-yyyy" TargetControlID="DateTextBox1" />
           
           
           
                <asp:Button ID="ReportButton1" runat="server" Text="Get Report" 
                    onclick="ReportButton1_Click" />
           
           
           
            </td>
            <td class="style4">
               
                <asp:Button ID="Button1" runat="server" Text="Export" onclick="Button1_Click" />
               
            </td>
        </tr>
        
        <tr>
            <td class="style5" colspan="5">
                <asp:GridView ID="GridView1" runat="server" >
                    <RowStyle BorderWidth="1px" BorderStyle="Solid" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="style5" colspan="3">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5" colspan="5">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5" colspan="3">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5" colspan="3">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style5" colspan="3">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
