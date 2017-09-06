<%@ Page Title="" Language="C#" MasterPageFile="~/confirmation/Site2.Master" AutoEventWireup="true" CodeBehind="ContactModification.aspx.cs" Inherits="InvestmentSummary.confirmation.ContactModification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            text-align: center;
        }
        .style3
        {
            width: 217px;
        }
        .style4
        {
            width: 76px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:100%; text-align: center;">
        <tr>
            <td class="style2">
                Select File:-</td>
            <td class="style3">
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td class="style4">
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Get Data" />
            </td>
            <td>
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Upload" />
            </td>
        </tr>
        <tr align="center"  >
            <td   colspan="4">
                <asp:GridView ID="GridView1" runat="server" style="text-align: center">
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td class="style4">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
