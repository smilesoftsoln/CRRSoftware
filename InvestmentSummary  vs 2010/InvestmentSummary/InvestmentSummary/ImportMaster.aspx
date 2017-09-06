<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ImportMaster.aspx.cs" Inherits="InvestmentSummary.ImportMaster" Title="Untitled Page" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %><%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <ajaxToolkit:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
      


    <div align="center">
<table align="center"><tr><td class="style1">
    <asp:Label ID="Label1" runat="server" Text="Select Master ">
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList1" runat="server">
        <asp:ListItem Selected="True">Customer Care</asp:ListItem>
        <asp:ListItem>Mutual Fund</asp:ListItem>
    </asp:RadioButtonList></td><td class="style1" colspan="3">
    
        <asp:Label ID="Label2" runat="server" Text="Select XLS File: "></asp:Label>
        <asp:FileUpload CssClass="button" ID="FileUpload1" runat="server" />
        <asp:Label ID="Label3" runat="server"></asp:Label>
    </td><td class="style1">
            <asp:Button CssClass="button" ID="ButtonGetData" runat="server" Text="Get Data" 
                onclick="ButtonGetData_Click" />
        </td>
    
    
    
    </tr>
    
    <tr><td class="style1">
        &nbsp;</td><td class="style1" colspan="3">
    
            <asp:Label ID="Label7" runat="server" Font-Bold="False" ForeColor="Black"></asp:Label>
    </td><td class="style1">
<%--            <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/UploadLog.aspx">Upload Log</asp:LinkButton>
--%>        </td>
    
    
    
    </tr>
    
    <tr align="center">
        <td align="center" colspan="5">
          
    <div style="border-style:none; border-color:Black; border-width: 0px; padding: 0px; vertical-align: middle; height: 300px; margin-left: 0px; overflow: auto; text-align: justify; width: 750px;" 
                align="center">
    
        <asp:GridView style=" font-size:small" ID="GridView1"    runat="server" BackColor="#CCCCCC" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" 
            CellSpacing="2" ForeColor="Black" >
            <RowStyle BackColor="White" />
            <FooterStyle BackColor="#CCCCCC" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </div>
    </td>
    
    </tr>
    <tr align="center">
    <td colspan="2">
        <asp:Button CssClass="button" ID="Button1" runat="server" Text="Import" onclick="Yes_Click" 
           />
        
           </td>
        <td>
            <asp:Button CssClass="button" ID="btnUpdate" runat="server" Text="Update" 
                onclick="BtnUpdate_Click" Visible="False" /></td>
        <td>
        <% if (GridView1.Rows.Count > 0)
           {%>
            <asp:Button CssClass="button" ID="Button2" runat="server" Text="Cancel" 
                onclick="Button2_Click" />
                <%} %>
                </td>
        <td>
            <asp:Button CssClass="button" ID="Button3" runat="server" 
                onclick="Button3_Click" Text="Back" />
        </td>
    </tr>
    
</table>
</div>
</asp:Content>


