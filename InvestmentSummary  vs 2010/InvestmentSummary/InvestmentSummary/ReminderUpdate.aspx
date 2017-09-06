<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReminderUpdate.aspx.cs" Inherits="InvestmentSummary.ReminderUpdate" %>

<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
<script type = "text/javascript">
    $(document).ready(function () {
        $('#<%=GridView1.ClientID %>').Scrollable({
            ScrollHeight: 100
        });

    });
    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table width="100%">
 <tr>
<th colspan="4">Over Due Reminders</th>
</tr>
  <tr>
            <td class="style1">
                &nbsp;
                Logged In As:&nbsp; <asp:Label ID="Label2" runat="server" Text=""> </asp:Label></td>
            <td class="style2">
                &nbsp;
               
            <ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>

            </td>
            <td class="style3">
                Branch: <asp:Label ID="Label3" runat="server" Text=""></asp:Label></td>
            <td>
                
            </td>
        </tr>
 <tr >
<td colspan="4">
<center><asp:GridView ID="GridView1" runat="server" CellPadding="4" 
        DataSourceID="ReminderDataSource1" ForeColor="#333333" GridLines="None" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:CommandField ShowSelectButton="True" />
    </Columns>
    <EditRowStyle BackColor="#2461BF" />
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#EFF3FB" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#F5F7FB" />
    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
    <SortedDescendingCellStyle BackColor="#E9EBEF" />
    <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:GridView>
    <asp:SqlDataSource ID="ReminderDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    
                    
                      SelectCommand="SELECT RemID, RemDate,UPPER(BM_RM_Name) AS BM_RM_Name , ClientCode, ClientName, Remark, Status, Branch FROM Reminder WHERE (BM_RM_Name = @BM_RM_Name) AND (Status = N'Later On') and RemDate<getdate()">
                    <SelectParameters>
                        <asp:SessionParameter Name="BM_RM_Name" SessionField="login" Type="String" />
                         
                    </SelectParameters>
                </asp:SqlDataSource>
</center>
    
     </td></tr>
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
              ReminderID:-<asp:Label ID="lblremid" runat="server"></asp:Label>  &nbsp;</td>
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
                <asp:Label ID="StatusLabel" runat="server"  Text=""                 />
                    
               
                
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
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
<tr ><td colspan="4">
<center></center>
    
    </td></tr>
</table>



</asp:Content>
