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

namespace InvestmentSummary
{

    //****Visit Mailer
    public partial class VisitMailer : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open(); cmd = conn.CreateCommand();
            cmd.CommandText = "Select distinct Branch from  UserMaster";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            string html = "";
            html = "<table><tr><td style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse' >ClientCode</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>ClientName</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Remark</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>RM NAME</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Location</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>DateTime(MM/DD/YYYY)</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Branch</td></tr>";
            int flag = 0;
            foreach (DataRow drw in dt.Rows)
            {
                //cmd = conn.CreateCommand();
                //cmd.CommandText = "select username,MailID,role from UserMaster where Branch='" + drw[0].ToString() + "'  and  role!='Admin' order by role";
                //dr = cmd.ExecuteReader();
                //DataTable dtmail = new DataTable();
                //dtmail.Load(dr);
                //dr.Close();
                //string bmmail = "";
                //string rmmail = "";
                //foreach (DataRow drw1 in dtmail.Rows)
                //{

                //    if (drw1[2].ToString().Equals("BM"))
                //    {
                //        bmmail = drw1[1].ToString();
                //        rmmail = drw1[1].ToString();
                //    }
                //    else if (drw1[2].ToString().Equals("RM"))
                //    {
                //        rmmail = drw1[1].ToString();
                //    }
               
                cmd = conn.CreateCommand();
                cmd.CommandText = "select distinct ClientCode,ClientName,Remark,BM_RM_Name,Location,RemDate as VisitDate,Branch from Reminder where Branch='" + drw[0].ToString() + "' and    RemDate>='" + DateTime.Today.AddDays(-2).ToString() + "' and  RemDate<='" + DateTime.Today.ToString() + "' and Status='Visit Done' ";
                dr = cmd.ExecuteReader();
                DataTable dtrem = new DataTable();
                dtrem.Load(dr);
                dr.Close();
                if (dtrem.Rows.Count != 0)
                {
                    foreach (DataRow dtrowrem in dtrem.Rows)
                    {
                        flag = 1;
                        html = html + "<tr>";
                        foreach (DataColumn dtcol in dtrem.Columns)
                        {
                            html = html + "<td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>" + dtrowrem[dtcol].ToString() + "</td>";

                        }
                        html = html + "</tr>";
                    }
                   
                }
                else
                {
                    //visitmail("", "samir@tradenetstockbroking.in", "No visits for Today", DateTime.Today, "For Branch " + drw[0].ToString());
                }

                



            }
            html = html + "</table>";
            if (flag == 1)
            {
                visitmail("", "samir@tradenetstockbroking.in", html, DateTime.Today.AddDays(-1), "");
            }
            else
            {
                visitmail("", "samir@tradenetstockbroking.in", "No Visits Done", DateTime.Today.AddDays(-1), "");
            }
            conn.Close();

        }
        public   void visitmail(string BmAddress, string rmaddress, string body, DateTime day,string subject)
        {
            /****/
            /**********Mail Sender************/
            MailMessage msgMail = new MailMessage();

            MailMessage myMessage = new MailMessage();
            myMessage.From = new MailAddress("techsupport2@tradenetstockbroking.in", "CRR Software");
            myMessage.Bcc.Add("techsupport2@tradenetstockbroking.in");
          
            myMessage.To.Add(rmaddress);

            myMessage.Subject = "CRR Visit Report " + day.AddDays(-1).ToString("dd-MMM-yyyy") + "-" + day.ToString("dd-MMM-yyyy") + subject;
            string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>PLEASE FIND THE Visit Done FOR THE CRR SOFTWARE AS FOLLOWS FOR YOUR KIND PERUSAL.</h3></br></br>" + body + "</br></br><h4>THANKING YOU,</h4></br><h4>TECHSUPPORT TEAM.</h4>";


            myMessage.IsBodyHtml = true;
            myMessage.Body = msgbody;
            //Attachment attch = new Attachment(Server.MapPath("~/") + "//Dat//CallCenterSummary" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
            //    myMessage.Attachments.Add(attch);
            //Attachment attch1 = new Attachment(Server.MapPath("~/") + "//Reports//MeetingReport" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
            // myMessage.Attachments.Add(attch1);
            SmtpClient mySmtpClient = new SmtpClient();
            System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential("techsupport2@tradenetstockbroking.in", "tech123");
            mySmtpClient.Host = "10.53.251.9";
            mySmtpClient.Port = 25;
            mySmtpClient.UseDefaultCredentials = false;
            mySmtpClient.Credentials = myCredential;
            mySmtpClient.ServicePoint.MaxIdleTime = 1;
            string day1 = DateTime.Today.DayOfWeek.ToString();
            //if (!day1.Equals("Saturday"))
            //{
            mySmtpClient.Send(myMessage);
            //}
            // MessageBox.Show("Mail sent to " + manemail + " and " + tlemail);
            myMessage.Dispose();



            /************/
        }
    }
}