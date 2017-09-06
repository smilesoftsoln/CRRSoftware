<%@ Page Title="" Language="C#" MasterPageFile="Site2.Master" AutoEventWireup="true" CodeBehind="ConfirmationEntry.aspx.cs" Inherits="InvestmentSummary.confirmation.ConfirmationEntry" %>


<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
 



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            color: #FF3300;
            background-color: #FFFF00;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="tkit" runat="server"></ajaxToolkit:ToolkitScriptManager>


<table style="width:100%;">
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td colspan="2"> <asp:LinkButton ID="TodaysLinkButton3" runat="server" CausesValidation="False" 
                    onclick="LinkButton3_Click">Todays Entry</asp:LinkButton>
                <asp:LinkButton ID="YesterdaysLinkButton2" runat="server" CausesValidation="False" 
                    onclick="LinkButton2_Click">Previous Days Pending</asp:LinkButton>
               
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="User Name:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="UserNameLabel13" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Dept/Branch:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="Dept_BranchLabel14" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Terminal No:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="TerminalNoTextBox1" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="Required Field" 
                    ControlToValidate="TerminalNoTextBox1"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ControlToValidate="TerminalNoTextBox1" ValidationExpression="^[a-zA-Z0-9]+$" ID="RegularExpressionValidator2" runat="server" ErrorMessage="Only Numbers Allowed..!"></asp:RegularExpressionValidator>   </td>

            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Client Code:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="ClientCodeTextBox2" runat="server" AutoPostBack="True" 
                    ontextchanged="TextBox2_TextChanged"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="Required Field" 
                    ControlToValidate="ClientCodeTextBox2"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ControlToValidate="ClientCodeTextBox2" ValidationExpression="^[a-zA-Z0-9]+$" ID="RegularExpressionValidator1" runat="server" ErrorMessage="Space Not Allowed..!"></asp:RegularExpressionValidator>   </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Client Name:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="ClientNameLabel15" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Contact No:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:RadioButtonList ID="ContactRadioButtonList1" runat="server" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ContactRadioButtonList1_SelectedIndexChanged" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem Text="123" Value="Landine1" ></asp:ListItem>
  <asp:ListItem Text="123" Value="Landine2" ></asp:ListItem>
   <asp:ListItem Text="123" Value="Mobile" ></asp:ListItem>
    <asp:ListItem Text="Other" Value="Other" ></asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="ContactRadioButtonList1" 
                    ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label10" runat="server" Text="Called No:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="UnRegisteredTextBox3" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ErrorMessage="Required Field" 
                    ControlToValidate="UnRegisteredTextBox3"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label11" runat="server" Text="Confirmation Date/Time:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="ConfirmationDateTextBox4" runat="server"></asp:TextBox>
                         <asp:TextBox ID="ConfirmationDateTextBoxTextBox1" Visible="false" runat="server"></asp:TextBox>
                       
                <asp:DropDownList ID="HH_DropDownList1" runat="server">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="MM_DropDownList2" runat="server">
                    <asp:ListItem>0</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>31</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>33</asp:ListItem>
                    <asp:ListItem>34</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>36</asp:ListItem>
                    <asp:ListItem>37</asp:ListItem>
                    <asp:ListItem>38</asp:ListItem>
                    <asp:ListItem>39</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>41</asp:ListItem>
                    <asp:ListItem>42</asp:ListItem>
                    <asp:ListItem>43</asp:ListItem>
                    <asp:ListItem>44</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>46</asp:ListItem>
                    <asp:ListItem>47</asp:ListItem>
                    <asp:ListItem>48</asp:ListItem>
                    <asp:ListItem>49</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>51</asp:ListItem>
                    <asp:ListItem>52</asp:ListItem>
                    <asp:ListItem>53</asp:ListItem>
                    <asp:ListItem>54</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>56</asp:ListItem>
                    <asp:ListItem>57</asp:ListItem>
                    <asp:ListItem>58</asp:ListItem>
                    <asp:ListItem>59</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="AM_PM_DropDownList3" runat="server">
                    <asp:ListItem>PM</asp:ListItem>
                    <asp:ListItem>AM</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label12" runat="server" Text="Segment:-"></asp:Label>
            </td>
            <td>
                <%--<asp:DropDownList ID="SegmentDropDownList4" runat="server">
                    <asp:ListItem>--Select--</asp:ListItem>
                    <asp:ListItem>BSE/NSE</asp:ListItem>
                    <asp:ListItem>NSECD</asp:ListItem>
                    <asp:ListItem>FONSE</asp:ListItem>
                </asp:DropDownList>--%>
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="CheckBoxList1_SelectedIndexChanged" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>BSE</asp:ListItem>  
                    <asp:ListItem>NSE</asp:ListItem>
                    <asp:ListItem>NSECD</asp:ListItem>
                    <asp:ListItem>FONSE</asp:ListItem>
                     <asp:ListItem>Commodity</asp:ListItem>
                </asp:CheckBoxList>
             </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label13" runat="server" Text="You Have Selected:-"></asp:Label>

                <asp:Label ID="SegmentLabel8" runat="server" Text="" style="font-weight: 700"></asp:Label>   

             </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
          <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Confirmation Given to:-"></asp:Label>
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ConfirmationToDropDownList5" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList5_SelectedIndexChanged">
                    <asp:ListItem>--Select--</asp:ListItem>
                    <asp:ListItem>Self</asp:ListItem>
                    <asp:ListItem>Wife</asp:ListItem>
                     <asp:ListItem>Husband</asp:ListItem>
                       <asp:ListItem>Son</asp:ListItem>
                       <asp:ListItem>Daughter</asp:ListItem>
                    <asp:ListItem>Other</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="ConfirmationToTextBox5" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="ConfirmationToTextBox5" 
                    ErrorMessage="Required Field"></asp:RequiredFieldValidator>
              </td>
            <td>
 </td>
        </tr>
          <tr>
            <td>
                &nbsp;</td>
            <td>
                Other Remark:-</td>
            <td colspan="2">
                <asp:TextBox ID="OtherRemarkTextBox1" TextMode="MultiLine"  runat="server" 
                    Height="44px" Width="205px"></asp:TextBox>
              </td>
            <td>
                &nbsp;</td>
        </tr>
          <tr runat="server" id="ReasonForPendingRow"  >
            <td>
                &nbsp;</td>
            <td>
                Reason For Pending</td>
            <td colspan="2">
                <asp:TextBox ID="ReasonForPendingTextBox1" runat="server"></asp:TextBox>
              </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
               </td>
            <td>
                </td>
            <td colspan="2">
                <asp:Button ID="Button1" runat="server" Text="Save" onclick="Button1_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td colspan="3">
              <div style="overflow :scroll; width:927px; height: 230px;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="ID" DataSourceID="SqlDataSource1" BackColor="White" 
                    BorderColor="#000000" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
                    Font-Size="Small" ForeColor="Black" Font-Names="Calibri">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                            ReadOnly="True" SortExpression="ID" />
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
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
                </div>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    
                    SelectCommand="SELECT * FROM [Confirmation] WHERE (([UserName] = @UserName) AND ([ConfirmationDate] &lt;= @ConfirmationDate) AND ([ConfirmationDate] &gt;= @ConfirmationDate2)) ORDER BY [ConfirmationDate]">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="UserNameLabel13" Name="UserName" 
                            PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="ConfirmationDateTextBoxTextBox1" 
                            Name="ConfirmationDate" PropertyName="Text" Type="DateTime" />
                        <asp:ControlParameter ControlID="ConfirmationDateTextBox4" 
                            Name="ConfirmationDate2" PropertyName="Text" Type="DateTime" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

 <div id="pendingDiv" runat="server"  
        
        
        
        style="width:580px; background-color:White; text-align:center; border: thick outset #FFFFFF; position: absolute; z-index: auto; top: 137px; left: 218px; height: 480px;"  >
     <span class="style2"><strong>Previous Days Pending
    </strong></span>
    <div style="overflow :scroll; width:580px;height: 430px;">
        <asp:GridView ID="PendingGridView2" runat="server" 
            BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" 
            CellPadding="2" ForeColor="Black" GridLines="None">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <FooterStyle BackColor="Tan" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <SortedAscendingCellStyle BackColor="#FAFAE7" />
            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
            <SortedDescendingCellStyle BackColor="#E1DB9C" />
            <SortedDescendingHeaderStyle BackColor="#C2A47B" />

        </asp:GridView>
              
   

    </div>
    
      <asp:Button ID="Button2" runat="server" Text="OK"  CausesValidation="false"
            onclick="Button2_Click" />
    </div>








</asp:Content>
