<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="InvestmentSummary.Admin" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
         
    .style2
    {
        background-color:Yellow;
    }
         
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center> <br /><br /> <br /> <br />   
    <table style="border-style: outset; >
    
   <tr style=" border-style: ridge; border-collapse: collapse;">
    <td class="style2"  >
        <asp:Label ID="Label1" runat="server" Text="Master Management" Font-Bold="True" 
            Font-Size="Medium"></asp:Label>
 
   </td>
   <td class="style2"  >
   <asp:Label ID="Label2" Font-Bold="True" 
            Font-Size="Medium"  runat="server" Text="Import Files"></asp:Label>
  
   </td>
<td class="style2"  > <asp:Label ID="Label3" Font-Bold="True" 
            Font-Size="Medium" runat="server" Text="Get Report"></asp:Label>
   </td>
   </tr>
    <tr>
         <td>
        <asp:Button ID="Button21" runat="server" Text="MF SubBroker Master" 
            PostBackUrl="~/MFSubBrokerMaster.aspx" />
        </td>
   <td>
   
        <asp:Button ID="Button2" runat="server" Width="150px"
            Text="Client Details" onclick="Button2_Click" />
   </td>
    </tr>
    <tr>
   <td>
    
        <asp:Button ID="Button1" Width="150px" runat="server" PostBackUrl="~/UserManagement.aspx" 
            Text="User Management" />
   </td>
<td>
        <asp:Button ID="Button3" runat="server" PostBackUrl="~/CashNetRisk.aspx" Width="150px"
            Text="Cash Net Risk" />
   </td>
   <td>
             <asp:Button ID="Button6" runat="server" PostBackUrl="~/InvestmentSummary.aspx" Width="150px"
            Text="Investment Summary" onclick="Button6_Click" />
           
     
   </td>
   </tr>
    <tr>
   <td>
        <asp:Button ID="Button9" runat="server" Text="File Import Details" 
            Width="150px" onclick="Button9_Click" PostBackUrl="~/ImportLog.aspx"/>
        </td>
<td>
            <asp:Button ID="Button4" runat="server" PostBackUrl="~/PMSNetRisk.aspx" Width="150px"
            Text="PMS Net Risk" />
   </td>
   <td>
             <asp:Button ID="Button16" runat="server" 
             Width="150px"
            Text="Family Summary PDF" onclick="Button16_Click" 
                 PostBackUrl="~/Family_PDF.aspx"   />
           
     
   </td>
   
   </tr>
   
   
    <tr>
   <td>
        <asp:Button ID="Button11" runat="server" Text="MAC ID Mapping" 
            Width="150px" onclick="Button9_Click" PostBackUrl="~/MacMapping.aspx"/>
   </td>
<td>
               <asp:Button ID="Button5" runat="server" PostBackUrl="~/FNONetRisk.aspx" Width="150px"
            Text="FNO Net Risk" />
   </td>
   <td>
             <asp:Button ID="Button17" runat="server" 
             Width="150px"
            Text="Client/RM  Count " onclick="Button16_Click" 
                 PostBackUrl="~/Client_Count_RM.aspx"   />
           
     
   </td>
   </tr>
   
   
    <tr>
   <td>
        <asp:Button ID="Button12" runat="server" Text="Client Master" 
            Width="150px" onclick="Button9_Click" PostBackUrl="~/ClientMaster.aspx"/>
        </td>
<td>
               <asp:Button ID="Button10" runat="server" PostBackUrl="~/MFNetRisk.aspx" Width="150px"
            Text="MF Net Risk" />
   </td>
   <td>
             <asp:Button ID="Button18" runat="server" 
             Width="150px"
            Text="Visit Report " onclick="Button16_Click" 
                 PostBackUrl="~/VisitReport.aspx"   />
           
     
        </td>
   </tr>
   
    <tr>
   <td>
        <asp:Button ID="Button13" runat="server" Text="RM Master" 
            Width="150px" onclick="Button9_Click" PostBackUrl="~/RM_Master.aspx"/>
   </td>
<td>
             <asp:Button ID="Button7" runat="server" PostBackUrl="~/DP900.aspx" Width="150px"
            Text="DP 900"   />
   </td>
   <td>
   </td>
   </tr>
    <tr>
   <td>
        <asp:Button ID="Button14" runat="server" Text="RM Mapping" 
            Width="150px" onclick="Button9_Click" PostBackUrl="~/RM_Mapping.aspx"/>
   </td>
<td>
             <asp:Button ID="Button8" runat="server" PostBackUrl="~/DP919.aspx" Width="150px"
            Text="DP 919"   />
   </td>
   <td>
       <asp:Button ID="Button19" runat="server" onclick="Button19_Click" 
           Text="Button" Visible="False" />
   </td>
   </tr>
   
    <tr>
   <td>
        <asp:Button ID="Button20" runat="server" Text="Equity SubBroker Master" 
            PostBackUrl="~/SubBrokerMaster.aspx" />
        </td>
      
<td>
             <asp:Button ID="Button15" runat="server"   Width="150px"
            Text="Margin Funding" PostBackUrl="~/Margin_Funding.aspx"   />
   </td>
   <td>
       &nbsp;</td>
   </tr>
   
   </table></center>
    
             
</asp:Content>
