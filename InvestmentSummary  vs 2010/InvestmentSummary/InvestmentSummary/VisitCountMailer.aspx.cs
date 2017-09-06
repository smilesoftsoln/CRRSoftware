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
    public partial class VisitCountMailer : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "Select RM,count(distinct FamilyCode) as FamilyCount from ClientMaster group by RM";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dt.Columns.Add("Visit Done");
            dt.Columns.Add("Visit Pending");
            dt.Columns.Add("Percent");
            dr.Close();
            int familytotal = 0;
            int visittotal = 0;
            foreach (DataRow drw in dt.Rows)
            {
                familytotal = familytotal + Convert.ToInt32(drw["FamilyCount"]);
                //drw["Visit Done"] = 0;
                //drw["Visit Pending"] = drw["FamilyCount"].ToString();
                cmd = conn.CreateCommand();
                cmd.CommandText = "Select RM, count(distinct FamilyCode) from ClientMaster where VisitStatus='Visit Done' and RM='" + drw[0].ToString() + "' group by RM";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    drw["Visit Done"] = dr[1].ToString();
                    dr.Close();
                    if (drw["Visit Done"] != null)
                    {
                        visittotal = visittotal + Convert.ToInt32(drw["Visit Done"]);
                        drw["Visit Pending"] = Convert.ToInt64(drw["FamilyCount"]) - Convert.ToInt64(drw["Visit Done"]);

                         //drw["Percent"]=Math.Round(((Convert.ToInt64(drw["Visit Done"])/Convert.ToInt64(drw["FamilyCount"]))*100.00),2);
                        double visitdone = Convert.ToDouble(drw["Visit Done"]);
                        double familycount = Convert.ToDouble(drw["FamilyCount"]);
                        double percent = visitdone / familycount;
                        drw["Percent"]=Math.Round(((visitdone/familycount)*100.00),2);


                    }
                   //else 
                   // {
                   //     visittotal = visittotal + Convert.ToInt32(drw["Visit Done"]);
                   //     drw["Visit Pending"] = Convert.ToInt64(drw["FamilyCount"]) - Convert.ToInt64(drw["Visit Done"]);

                   //     drw["Percent"] = 0;// Math.Round(((Convert.ToInt64(drw["Visit Done"]) / Convert.ToInt64(drw["FamilyCount"])) * 100.00), 2);


                   // }

                }
                dr.Close();
            }

            string html = "Total Families:-" + familytotal + "<br/>" + "Visits Done:-" + visittotal + "<br/>" + "Remaining:-" + (familytotal - visittotal).ToString();
            html = html + "<br/><table><tr><td style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse' >RM Name</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Total Families</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Visits Done</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Remaining</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Percentage Visits Done</td></tr>";

            foreach (DataRow drw in dt.Rows)
            {
                
                html = html + "<tr>";
                foreach (DataColumn dtcol in dt.Columns)
                {
                    html = html + "<td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>" + drw[dtcol].ToString() + "</td>";

                }
                html = html + "</tr>";
              
            }
            html = html + "</table>";

            visitmail("", "samir@tradenetstockbroking.in", html, DateTime.Today.AddDays(-1), "");
            foreach (DataRow drw in dt.Rows)
            {
                html = "<br/><table><tr><td style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse' >RM Name</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Total Families</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Visits Done</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Remaining</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Percentage Visits Done</td></tr>";
                html = html + "<tr>";
                foreach (DataColumn dtcol in dt.Columns)
                {
                    html = html + "<td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>" + drw[dtcol].ToString() + "</td>";

                }
                html = html + "</tr>";
                html = html + "</table>";
                cmd = conn.CreateCommand();
                cmd.CommandText = "select MailID from UserMaster where username='" + drw["RM"] + "'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    try
                    {
                        if (!string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            visitmail("", dr[0].ToString(), html, DateTime.Today.AddDays(-1), "");
                            dr.Close();
                        }
                    }
                    catch (Exception ex)
                    { 
                    
                    }
                    dr.Close();
                }
                dr.Close();
            }


           // }
          

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
            myMessage.CC.Add("ccare03@tradenetstockbroking.in");
            myMessage.To.Add(rmaddress);

            myMessage.Subject = "CRR Visit Count Report As On "+DateTime.Today.ToString("dd-MMM-yyyy")
                ;
            string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>PLEASE FIND THE Visit Count FOR THE CRR SOFTWARE AS FOLLOWS FOR YOUR KIND PERUSAL.</h3></br></br>" + body + "</br></br><h4>THANKING YOU,</h4></br><h4>TECHSUPPORT TEAM.</h4>";


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
            //if (DateTime.Today.DayOfWeek.ToString().Equals("Friday"))
            //{
                mySmtpClient.Send(myMessage);
            //}
            //}
            // MessageBox.Show("Mail sent to " + manemail + " and " + tlemail);
            myMessage.Dispose();



            /************/
        }
    }
}