using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Net;
namespace InvestmentSummary
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button6_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportMaster.aspx");
        }

        protected void Button9_Click(object sender, EventArgs e)
        {

        }

        protected void Button16_Click(object sender, EventArgs e)
        {

        }

        protected void Button19_Click(object sender, EventArgs e)
        {


            #region Variables
            string userId = "tnsbpl";
            string pwd = "tns123";
            string postURL = "http://mobi1.blogdns.com/httpmsgid/SMSSenders.aspx?";

            StringBuilder postData = new StringBuilder();
            string responseMessage = string.Empty;
            HttpWebRequest request = null;
            #endregion

            try
            {
                // Prepare POST data 
            //    postData.Append("action=send");
                postData.Append("&UserID=" + userId);
                postData.Append("&UserPass=" + pwd);
                postData.Append("&MobileNo=" + "09762764597");
                postData.Append("&Message=" + "test");
                postData.Append("&GSMID=" + "TRDNET");
                byte[] data = new System.Text.ASCIIEncoding().GetBytes(postData.ToString());

                // Prepare web request
                request = (HttpWebRequest)WebRequest.Create(postURL);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                // Write data to stream
                using (Stream newStream = request.GetRequestStream())
                {
                    newStream.Write(data, 0, data.Length);
                }

                // Send the request and get a response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Read the response
                    using (StreamReader srResponse = new StreamReader(response.GetResponseStream()))
                    {
                        responseMessage = srResponse.ReadToEnd();
                    }

                    // Logic to interpret response from your gateway goes here
                    //Response.Write(String.Format("Response from gateway: {0}", responseMessage));
                }
            }
            catch (Exception objException)
            {
                //Response.Write(objException.ToString());
            }            

        
            }
    }
}
