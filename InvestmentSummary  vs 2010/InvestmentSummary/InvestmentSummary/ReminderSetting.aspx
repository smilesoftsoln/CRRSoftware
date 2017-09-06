<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReminderSetting.aspx.cs" Inherits="InvestmentSummary.ReminderSetting" %>

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
    $(document).ready(function () {
        $('#<%=GridView2.ClientID %>').Scrollable({
            ScrollHeight: 100
        });

    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    
 
 <ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>


    <table style="width: 100%;">
        <tr>
            <td class="style1">
                &nbsp;
                Logged In As:&nbsp; <asp:Label ID="Label2" runat="server" Text=""> </asp:Label></td>
            <td class="style2">
                &nbsp;
               
            </td>
            <td class="style3">
                Branch: <asp:Label ID="Label3" runat="server" Text=""></asp:Label></td>
            <td>
                &nbsp;
               
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
                <%--Search Name:---%>Search Client:-</td>
            <td class="style2">
                &nbsp;
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td class="style3">
                <asp:Button ID="Button1" runat="server" Text="Get Clients" 
                    onclick="Button1_Click" CausesValidation="False" />
            </td>
            <td>
                <asp:Button ID="Button2" runat="server" PostBackUrl="~/BM_RM_Page.aspx" 
                    Text="Back" CausesValidation="False" />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" class="style1" colspan="4">
                  <asp:GridView ID="GridView1" 
                        runat="server" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged">
                    <RowStyle BackColor="#F7F7DE" />
                    <Columns>
                        <asp:CommandField HeaderText="Select" ShowHeader="True" 
                            ShowSelectButton="True" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView> 
&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" class="style1" colspan="4">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="RemID" DataSourceID="remcliSqlDataSource1" CellPadding="4" 
                    ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="RemID" HeaderText="RemID" InsertVisible="False" 
                            ReadOnly="True" SortExpression="RemID" />
                        <asp:BoundField DataField="RemDate" HeaderText="RemDate"  
                            SortExpression="RemDate" />
                        <asp:BoundField DataField="BM_RM_Name"  HeaderText="BM_RM_Name" 
                            SortExpression="BM_RM_Name" />
                        <asp:BoundField DataField="ClientCode" HeaderText="FamilyCode" 
                            SortExpression="ClientCode" />
                        <asp:BoundField DataField="ClientName" HeaderText="ClientName" 
                            SortExpression="ClientName" />
                        <asp:BoundField DataField="Remark" HeaderText="Remark" 
                            SortExpression="Remark" />
                        <asp:BoundField DataField="Status" HeaderText="Status" 
                            SortExpression="Status" />
                        <asp:BoundField DataField="Branch" HeaderText="Branch" 
                            SortExpression="Branch" />
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
                <asp:SqlDataSource ID="remcliSqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT * FROM [Reminder] WHERE ([ClientCode] = @ClientCode)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblClientCode" Name="ClientCode" 
                            PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
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
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style1">
                Client Code:<asp:Label ID="lblClientCode" runat="server"></asp:Label>
            </td>
            <td class="style2">
                Client Name:<br />
                <asp:Label ID="lblClientName" runat="server"></asp:Label>
            </td>
            <td class="style3">
              <asp:Label ID="remdatelabel" runat="server" Text="Reminder Date" ></asp:Label>  </td>
            <td>
                <asp:TextBox ID="DateTextBox2" runat="server"></asp:TextBox>
           <ajaxToolkit:CalendarExtender  ID="remdate" runat ="server" TargetControlID="DateTextBox2" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                <asp:DropDownList ID="DropDownList1" runat="server" Height="29px" Width="49px">
                 <asp:ListItem>00</asp:ListItem>
                <asp:ListItem>01</asp:ListItem>
                 <asp:ListItem>02</asp:ListItem>
                 <asp:ListItem>03</asp:ListItem>
                 <asp:ListItem>04</asp:ListItem>
                 <asp:ListItem>05</asp:ListItem>
                 <asp:ListItem>06</asp:ListItem>
                 <asp:ListItem>07</asp:ListItem>
                 <asp:ListItem>08</asp:ListItem>
                 <asp:ListItem>09</asp:ListItem>
                 <asp:ListItem>10</asp:ListItem>
                 <asp:ListItem>11</asp:ListItem>
                 <asp:ListItem>12</asp:ListItem>
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
                Remark:-</td>
            <td class="style2">
                <asp:TextBox ID="RemarkTextBox3" runat="server" Height="76px" TextMode="MultiLine" 
                    Width="208px"></asp:TextBox>
            </td>
            <td class="style3">
                Status:-</td>
            <td>
                <asp:Label ID="StatusDropDownList11" Text="Later On" runat="server" />
                    
                   
                
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;</td>
            <td class="style2">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="RemarkTextBox3" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
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
              <asp:Label ID="visitloc" runat="server" Text="Visit Location:"></asp:Label>  </td>
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
                &nbsp;</td>
            <td class="style2">
                <asp:Button ID="Button3" runat="server" Text="Save" onclick="Button3_Click" />
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
                &nbsp;</td>
        </tr>
    </table>






</asp:Content>
