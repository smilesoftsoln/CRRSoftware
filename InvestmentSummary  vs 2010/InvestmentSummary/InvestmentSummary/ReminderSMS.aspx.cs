using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using System.Net.Mail;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Configuration;
using System.Text;
using System.Net;
using System.IO;
namespace InvestmentSummary
{
    public partial class ReminderSMS : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open(); 
            
            cmd = conn.CreateCommand();
            cmd.CommandText = "select ClientCode,ClientName,Remark,BM_RM_Name from Reminder where   Status!='Postponded' and  RemDate>='" + DateTime.Today.ToString() + "' and  RemDate<='" + DateTime.Today.AddDays(1).ToString() + "'";
            dr = cmd.ExecuteReader();
            DataTable dtrem = new DataTable();

            dtrem.Load(dr);
            dr.Close();
            foreach (DataRow drw in dtrem.Rows)
            {
                try
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select MobileNo from RM_Master where RM='" + drw["BM_RM_Name"].ToString().ToUpper() + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            if (! (dr[0] is DBNull))
                            {
                                string mobileno = dr[0].ToString();
                                string userId = "tnsbpl";//ConfigurationManager.AppSettings["SMSGatewayUserID"];
                                string pwd = "tns123";///ConfigurationManager.AppSettings["SMSGatewayPassword"];
                                string postURL = "http://mobi1.blogdns.com/httpmsgid/SMSSenders.aspx";//ConfigurationManager.AppSettings["SMSGatewayPostURL"];

                                StringBuilder postData = new StringBuilder();
                                string responseMessage = string.Empty;
                                HttpWebRequest request = null;

                                try
                                {
                                    // Prepare POST data 
                                    postData.Append("action=send");
                                    postData.Append("&UserID=" + userId);
                                    postData.Append("&UserPass=" + pwd);
                                    postData.Append("&MobileNo="+mobileno);
                                    postData.Append("&Message=" + drw["ClientCode"].ToString() + " " + drw["ClientName"].ToString() + " " + drw["Remark"].ToString());
                                    postData.Append("&GSMID=TRDCRR");
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
                                        // MessageBox.Show(String.Format("Response from gateway: {0}", responseMessage)); 
                                    }
                                }
                                catch (Exception objException)
                                {
                                    //MessageBox.Show(objException.ToString());
                                } 


                            }
                        
                        }
                    }
                    dr.Close();

                }
                catch (Exception ex)
                {
                    dr.Close();
                }
                dr.Close();
            }
            conn.Close();

        }
    }
}