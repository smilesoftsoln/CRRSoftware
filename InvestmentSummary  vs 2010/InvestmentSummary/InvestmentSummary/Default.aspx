<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InvestmentSummary._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script id="clientEventHandlersJS" type="text/javascript">
      <!--
     function btnGo_onClick() {
        // Connect to WMI
	            var locator = new ActiveXObject("WbemScripting.SWbemLocator");
	            var service = locator.ConnectServer(".");
	             
            // Get the info
            var properties = service.ExecQuery("SELECT * FROM Win32_NetworkAdapterConfiguration  WHERE IPEnabled = True");
	            var e = new Enumerator (properties);
	             
	            // Output info
	         //   document.write("<table id='macid' border=1>");
	             
	            for (;!e.atEnd();e.moveNext ())
	            {
	                var p = e.item ();
 
 
    // document.getElementById("txtIPAdress").value = unescape(ipAddress);
  //  document.getElementById("txtComputerName").value = unescape(computerName);

 	             //document.write("<tr>");
	              // document.write("<td>" + p.Caption + "</td>");
	              document.getElementById("txtuser0").value=p.MACAddress;
	           //  document.write("<input id='macid' type='text' value='" + p.MACAddress + "'/>");
	               // document.write("</tr>");
 
	            }
//            document.write("</table>");
//alert( document.getElementById("Label4").value );
	        }
 
	        //-->
    </script>

    <style type="text/css">
        .button
        {
            background-image: url(  '/img/btn.JPG' );
        }
        .button:hover
        {
            font-family: Arial Black;
            background-image: url(  '/img/btnhover.JPG' );
        }
    </style>
</head>
<body background="img/paperTextureNo2254_preview.jpg" style="height: 450px"  >
    <form id="form1" runat="server" dir="ltr" style="height: 450px"
    >
    <div align="center">
        <table width="100%">
            <tr>
                <td>
                    <center>
                    <asp:Image ID="logo" runat="server" Height="58px" ImageUrl="~/pdf_logo.jpg" 
                            Width="92px"   />    <b style="font-size: xx-large; color: #800000;">Trade Net Wealth Managers Pvt Ltd,Kolhapur</b></center>
                </td>
            </tr>
            <tr>
                <td>
                    <center style="font-family: 'Arial Black'; color: #003366">
                        Investment Summary Software</center>
                </td>
            </tr>
        </table>
    </div>
    <center> 
        <div style="text-align: center; vertical-align: middle; background-repeat: inherit;  height: 450px; width: 778px;" 
        align="center">
        <br />
        <br />
        <table   style="height: 221px; width: 493px; position: absolute; z-index: auto; top: 165px; left: 7px;">
            <tr>
                <td style="color: #000">
                <div style="position: absolute; top: -82px; left: 293px; width: 487px;">
                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
              
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <b>User Name:-</b> 
                    
                    <asp:TextBox ID="txtuser" runat="server" Style="text-align: left" OnTextChanged="txtuser_TextChanged"
                        Height="23px" Width="155px" Font-Size="Medium"></asp:TextBox>
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp; &nbsp; <b>Password:-&nbsp;&nbsp;&nbsp;&nbsp; </b>
                    <asp:TextBox ID="txtpassword" runat="server" Style="text-align: left" TextMode="Password"
                        Height="23px" Width="155px" Font-Size="Medium"></asp:TextBox>
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;
                    <asp:Button CssClass="button" ID="cmdlogin" runat="server" Text="Login" OnClick="cmdlogin_Click"
                        Height="31px" Font-Bold="True" Width="81px" />
                    <br />
                    <br />
                    <asp:Label ID="Label4" runat="server" ForeColor="#CC0000" Font-Bold="True"></asp:Label>
          
                </div>
                        </td>
            </tr>
        </table>
        <br />
        <asp:TextBox  ID="txtuser0" Visible="true"  
                        runat="server" Style="text-align: left"  
                        Height="0px" Width="0px" Font-Size="Medium"   ></asp:TextBox>
    </div></center>
    
    
    <script type="text/javascript" >
     btnGo_onClick();
    </script>
    </form>
    <div style="position: absolute; top: 536px; left: 471px; width: 389px; color: #0000CC; font-weight: 700; background-color: #FFFF66;">
        Developed and Maintained @ Department of IT &amp; EDP</div>
</body>
</html>
