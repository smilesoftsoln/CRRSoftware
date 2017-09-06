<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="InvestmentSummary.NewUser" Title="Untitled Page" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <table class="style1" align="center">
        <tr>
            <td class="style12" colspan="11">
                <asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3" style="height: 23px">
                <asp:Label ID="Label5" runat="server" Text="User Id" Visible="False"></asp:Label>
            </td>
            <td class="style14" colspan="5" style="height: 23px">
                </td>
            <td class="style8" colspan="3" style="height: 23px">
                <asp:Label ID="Label6" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3">
                <asp:Label ID="Label1" runat="server" Text="User Name:"></asp:Label>
            </td>
            <td class="style14" colspan="5">
                &nbsp;</td>
            <td class="style8" colspan="3">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3">
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            </td>
            <td class="style14" colspan="5">
                &nbsp;</td>
            <td class="style8" colspan="3">
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3">
                <asp:Label ID="Label3" runat="server" Text="Role:"></asp:Label>
            </td>
            <td class="style14" colspan="5">
                &nbsp;</td>
            <td class="style8" colspan="3">
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
           onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem>--Select--</asp:ListItem>
                  <asp:ListItem>MNG</asp:ListItem>
                 <asp:ListItem>RM</asp:ListItem>
                    
                    <asp:ListItem>BM</asp:ListItem>
                  
                    <asp:ListItem>Admin</asp:ListItem>  
                    <asp:ListItem>Mentor</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td   colspan="3">
                Branch/es</td>
            <td   colspan="5">
                &nbsp;</td>
            <td  colspan="3">
                <asp:DropDownList ID="BranchDropDownList2" runat="server"     
                    DataSourceID="SqlDataSource1" DataTextField="BranchName" 
                    DataValueField="BranchName">
                   
                </asp:DropDownList>
                <asp:CheckBoxList ID="BranchCheckBoxList1" runat="server"   Visible="false"   
                    DataSourceID="SqlDataSource1" DataTextField="BranchName" 
                    DataValueField="BranchName">
                </asp:CheckBoxList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [BranchName] FROM [SBCODE]">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3">
                E Mail ID</td>
            <td class="style14" colspan="5">
                &nbsp;</td>
            <td class="style8" colspan="3">
                <asp:TextBox ID="EmailTextbox" runat="server" Width="186px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3">
                &nbsp;</td>
            <td class="style14" colspan="5">
                &nbsp;</td>
            <td class="style8" colspan="3">
                <asp:CheckBox Visible="false"  ID="ChekMF" runat="server" Text="Is MF Executive" />
            </td>
        </tr>
        <tr>
            <td class="style12" colspan="3">
                &nbsp;</td>
            <td class="style14" colspan="5">
                &nbsp;</td>
            <td class="style8" colspan="3">
                &nbsp;</td>
        </tr>
        <tr align="center">
            <td class="style13">
                &nbsp;</td>
            <td class="style13" colspan="4">
                <asp:Button CssClass="button" ID="Button5"  runat="server" onclick="Button5_Click" Text="First" 
                    Height="26px" Width="60px" />
            </td>
            <td class="style13">
                <asp:Button CssClass="button" ID="Button3" runat="server" onclick="Button3_Click" Text="Next" 
                    Height="26px" Width="60px" />
            </td>
            <td class="style13" colspan="4">
                <asp:Button CssClass="button" ID="Button4" runat="server" onclick="Button4_Click" 
                    style="width: 71px" Text="Previous" Height="26px" 
                    Width="60px" />
            </td>
            <td class="style13">
                <asp:Button CssClass="button" ID="Button6" runat="server" onclick="Button6_Click" Text="Last" 
                    Height="26px" Width="60px" />
            </td>
        </tr>
        <tr align="center">
            <td class="style13" colspan="2">
                &nbsp;</td>
            <td class="style13" colspan="2">
                <asp:Button CssClass="button" ID="Button1" runat="server" Text="Save" onclick="Button1_Click" 
                    Height="26px" Width="60px" />
            </td>
            <td class="style13" colspan="3">
                <asp:Button CssClass="button" ID="Button7" runat="server" onclick="Button7_Click" Text="Search" 
                    Height="26px" Width="60px" />
            </td>
            <td class="style13" colspan="2">
                <asp:Button CssClass="button" ID="Button8" runat="server" onclick="Button8_Click" Text="Modify" 
                    Height="26px" Width="60px" />
            </td>
            <td class="style13">
                <asp:Button CssClass="button" ID="Button2" runat="server" Text="Cancel" onclick="Button2_Click" 
                    Height="25px" Width="60px" />
            </td>
            <td class="style13">
                <asp:Button CssClass="button" ID="Button9" runat="server" Text="Back" Height="26px" Width="60px" 
                    onclick="Button9_Click" />
            </td>
        </tr>
        </table>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .button
        {}
    </style>

</asp:Content>

