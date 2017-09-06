<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Snooze.aspx.cs" Inherits="InvestmentSummary.Snooze" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table style="width: 100%;">
        <tr>
            <td class="style1">
                &nbsp;<ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>

                Logged In As:&nbsp; <asp:Label ID="Label2" runat="server" Text=""> </asp:Label></td>
            <td class="style2">
                &nbsp;
               
            </td>
            <td class="style3">
                Branch: <asp:Label ID="Label3" runat="server" Text=""></asp:Label></td>
            <td>
                &nbsp;
               
                ReminderID:-<asp:Label ID="lblremid" runat="server"></asp:Label>
               
            </td>
        </tr>
           <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
           <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
           <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
           <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
           <tr>
            <td class="style1">
                Client Code:<asp:Label ID="lblClientCode" runat="server" 
                    style="font-weight: 700; color: #0000CC"></asp:Label>
            </td>
            <td class="style2">
                Client Name:<br />
                <asp:Label ID="lblClientName" runat="server" 
                    style="font-weight: 700; color: #0000FF"></asp:Label>
            </td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                &nbsp;</td>
            <td class="style3">
                &nbsp;</td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="DateTextBox2" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style1">
                Select
            </td>
            <td class="style2">
                <asp:DropDownList ID="StatusDropDownList2" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="StatusDropDownList2_SelectedIndexChanged">
                       <asp:ListItem>InComplete</asp:ListItem>
                    <asp:ListItem>Complete</asp:ListItem>
                   
                </asp:DropDownList>
            </td>
            <td class="style3">
                <asp:Label ID="remdatelabel" runat="server" Text="Reminder Date" ></asp:Label></td>
            <td>
                <asp:TextBox ID="DateTextBox2" runat="server"></asp:TextBox>
           <ajaxToolkit:CalendarExtender  ID="remdate" runat ="server" TargetControlID="DateTextBox2" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                           <asp:DropDownList ID="DropDownList1" runat="server" Height="29px" Width="49px">
                <asp:ListItem>1
                </asp:ListItem>
                 <asp:ListItem>2
                </asp:ListItem>
                 <asp:ListItem>3
                </asp:ListItem>
                 <asp:ListItem>4
                </asp:ListItem>
                 <asp:ListItem>5
                </asp:ListItem>
                 <asp:ListItem>6
                </asp:ListItem>
                 <asp:ListItem>7
                </asp:ListItem>
                 <asp:ListItem>8
                </asp:ListItem>
                 <asp:ListItem>9
                </asp:ListItem>
                 <asp:ListItem>10
                </asp:ListItem>
                 <asp:ListItem>11
                </asp:ListItem>
                 <asp:ListItem>12
                </asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2" runat="server" Height="29px" Width="49px">
               <asp:ListItem>00</asp:ListItem>
               <asp:ListItem>15</asp:ListItem>
               <asp:ListItem>30</asp:ListItem>
               <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>60</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList3" runat="server" Height="29px" Width="49px">
                <asp:ListItem>PM</asp:ListItem>
                  <asp:ListItem>AM</asp:ListItem>
                </asp:DropDownList>
           
           
           
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Label ID="visitloc" runat="server" Text="Visit Location:"></asp:Label>&nbsp;</td>
            <td class="style2">
                <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList4_SelectedIndexChanged">
                    <asp:ListItem>Clients Residence</asp:ListItem>
                    <asp:ListItem>Clients Office</asp:ListItem>
                    <asp:ListItem>At Branch Office</asp:ListItem>
                    <asp:ListItem>Other</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:TextBox ID="OtherTextBox2" runat="server" Visible="False"></asp:TextBox>
            </td>
            <td class="style3">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>

        <tr>
            <td class="style1">
                Current
                Remark:-</td>
       Remark:-</td>
            <td class="style2">
                <asp:Label ID="lblremark" runat="server" 
                    style="font-weight: 700; color: #0000FF"></asp:Label>
            </td>
            <td class="style3">
                Status-->
                <asp:DropDownList ID="StatusDropDownList1" runat="server" AutoPostBack="True" 
                    Enabled="False">
                    <asp:ListItem>Later On</asp:ListItem>
                    <asp:ListItem>Visit Done</asp:ListItem>
               
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style1">
                New Remark<br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="RemarkTextBox1" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            </td>
            <td class="style2">
                <asp:TextBox ID="RemarkTextBox1" runat="server" Height="44px" 
                    TextMode="MultiLine" Width="209px"></asp:TextBox>
            </td>
            <td class="style3">
              <asp:Label ID="remdatelabel0" runat="server" Text="Next Review Date" 
                    Visible="False" ></asp:Label>  </td>
            <td>
                <asp:TextBox ID="DateTextBox3" runat="server" Visible="False"></asp:TextBox>
           <ajaxToolkit:CalendarExtender  ID="DateTextBox3_CalendarExtender" runat ="server" 
                    TargetControlID="DateTextBox3" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="DateTextBox3" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                <asp:Button ID="Button3" runat="server" Text="Save" onclick="Button3_Click" />
            </td>
            <td class="style3">
                <asp:Button ID="Button4" runat="server" Text="Back" 
                    PostBackUrl="~/BM_RM_Page.aspx"   />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>

</asp:Content>
