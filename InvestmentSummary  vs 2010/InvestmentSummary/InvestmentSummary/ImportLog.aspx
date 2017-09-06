<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ImportLog.aspx.cs" Inherits="InvestmentSummary.ImportLog" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
                Files Imported Today:-<asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                    <asp:Button ID="Button3" runat="server" 
                    PostBackUrl="~/Admin.aspx" Text="Back" />
                                    </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
            <center> <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1">
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString1 %>" 
                    SelectCommand="SELECT DISTINCT [FileName] FROM [UploadLog] WHERE ([UploadDate] = @UploadDate)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="Label2" DbType="DateTime" Name="UploadDate" 
                            PropertyName="Text" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </center>
               
            </td>
        </tr>
        <tr>
            <td>
                Files Remaing:-</td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
              
                 <center> <asp:GridView ID="GridView2" runat="server" >
                </asp:GridView></center>
            </td>
        </tr>
    </table>
    


</asp:Content>
