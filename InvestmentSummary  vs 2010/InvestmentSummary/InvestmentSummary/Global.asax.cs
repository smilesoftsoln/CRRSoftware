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
    public class Global : System.Web.HttpApplication
    {
       static  SqlConnection conn;
       static  SqlCommand cmd;
       static  SqlDataReader dr;
        protected void Application_Start(object sender, EventArgs e)
        {
        //    Timer mailtimer = new Timer();
        //    mailtimer.Interval = 60000 * 1;
        //mailtimer.Enabled = true;
        //mailtimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        //mailtimer.Stop();
        //mailtimer.Start();
        //Application["ActiveUsers"] = 0;
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            string fourhfpm = DateTime.Today.AddHours(17).AddMinutes(30).ToString("HH:mm");
            string sixpm = DateTime.Today.AddHours(18).ToString("HH:mm");
            string nineam = DateTime.Today.AddHours(9).ToString("HH:mm");
            string now = DateTime.Now.ToString("HH:mm");
           // nineammail();
            //sixpmmail();
            if (now.Equals(sixpm))
            {
               // sixpmmail();
                visitsixpmmail();
            }
            if (now.Equals(fourhfpm))
            {
          fourpmmail();
                //visitsixpmmail();
            }
            if (now.Equals(nineam))
            {
                nineammail();
            }
        }
        public static void visitsixpmmail()
        {

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open(); cmd = conn.CreateCommand();
            cmd.CommandText = "Select distinct Branch from  UserMaster";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
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
                    cmd.CommandText = "select ClientCode,ClientName,Remark,BM_RM_Name from Reminder where    RemDate='" + DateTime.Today.AddDays(1).ToString() + "' and Status='Visit Done' ";
                    dr = cmd.ExecuteReader();
                    DataTable dtrem = new DataTable();
                    dtrem.Load(dr);
                    dr.Close();
                    string html = "<table><tr><td style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse' >ClientCode</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>ClientName</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Remark</td></tr>";
                    if (dtrem.Rows.Count != 0)
                    {
                        foreach (DataRow dtrowrem in dtrem.Rows)
                        {
                            html = html + "<tr>";
                            foreach (DataColumn dtcol in dtrem.Columns)
                            {
                                html = html + "<td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>" + dtrowrem[dtcol].ToString() + "</td>";

                            }
                            html = html + "</tr>";
                        }
                        html = html + "</table>";
                      visitmail("", "samir@tradenetstockbroking.in", html, DateTime.Today);
                    }
                    else
                    {
                        visitmail("", "samir@tradenetstockbroking.in", "No visits for Today", DateTime.Today);
                    }

                




            }

            conn.Close();
        }
        public static void fourpmmail()
        {
        
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
               conn.Open(); cmd = conn.CreateCommand();
            cmd.CommandText = "Select distinct Branch from  UserMaster";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            foreach (DataRow drw in dt.Rows)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "select username,MailID,role from UserMaster where Branch='" + drw[0].ToString() + "'  and  role!='Admin' order by role";
                dr = cmd.ExecuteReader();
                DataTable dtmail = new DataTable();
                dtmail.Load(dr);
                dr.Close();
                string bmmail="";
                string rmmail = "";
                foreach (DataRow drw1 in dtmail.Rows)
                {

                    if (drw1[2].ToString().Equals("BM"))
                    {
                        bmmail = drw1[1].ToString();
                        rmmail = drw1[1].ToString();
                    }
                    else if (drw1[2].ToString().Equals("RM"))
                    {
                        rmmail = drw1[1].ToString();
                    }
                    else if (drw1[2].ToString().Equals("MNG"))
                    {
                        bmmail = drw1[1].ToString();
                        rmmail = drw1[1].ToString();
                    }
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select ClientCode,ClientName,Remark,BM_RM_Name from Reminder where  BM_RM_Name='" + drw1[0].ToString() + "' and RemDate='" + DateTime.Today.AddDays(1).ToString() + "'";
                    dr = cmd.ExecuteReader();
                    DataTable dtrem = new DataTable();
                    dtrem.Load(dr);
                    dr.Close();
                    string html = "<table><tr><td style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse' >ClientCode</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>ClientName</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Remark</td></tr>";
                    if (dtrem.Rows.Count != 0)
                    {
                        foreach (DataRow dtrowrem in dtrem.Rows)
                        {
                            html = html + "<tr>";
                            foreach (DataColumn dtcol in dtrem.Columns)
                            {
                                html = html + "<td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>" + dtrowrem[dtcol].ToString() + "</td>";

                            }
                            html = html + "</tr>";
                        }
                        html = html + "</table>";
                        mail(bmmail, rmmail, html, DateTime.Today.AddDays(1));
                    }
                    else
                    {
                        mail(bmmail, rmmail, "No Reminders For Tomorrow", DateTime.Today.AddDays(1));
                    }
                
                }




            }

            conn.Close();
        }
        public static void nineammail()
        {

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open(); cmd = conn.CreateCommand();
            cmd.CommandText = "Select distinct Branch from  UserMaster";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            foreach (DataRow drw in dt.Rows)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "select username,MailID,role from UserMaster where Branch='" + drw[0].ToString() + "'  and  role!='Admin' order by role";
                dr = cmd.ExecuteReader();
                DataTable dtmail = new DataTable();
                dtmail.Load(dr);
                dr.Close();
                string bmmail = "";
                string rmmail = "";
                foreach (DataRow drw1 in dtmail.Rows)
                {

                    if (drw1[2].ToString().Equals("BM"))
                    {
                        bmmail = drw1[1].ToString();
                        rmmail = drw1[1].ToString();
                    }
                    else if (drw1[2].ToString().Equals("RM"))
                    {
                        rmmail = drw1[1].ToString();
                    }
                    else if (drw1[2].ToString().Equals("MNG"))
                    {
                        bmmail = drw1[1].ToString();
                        rmmail = drw1[1].ToString();
                    }
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select ClientCode,ClientName,Remark,BM_RM_Name from Reminder where  BM_RM_Name='" + drw1[0].ToString() + "' and RemDate='" + DateTime.Today.ToString() + "'";
                    dr = cmd.ExecuteReader();
                    DataTable dtrem = new DataTable();
                    dtrem.Load(dr);
                    dr.Close();
                    string html = "<table><tr><td style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse' >ClientCode</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>ClientName</td><td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>Remark</td></tr>";
                    if (dtrem.Rows.Count != 0)
                    {
                        foreach (DataRow dtrowrem in dtrem.Rows)
                        {
                            html = html + "<tr>";
                            foreach (DataColumn dtcol in dtrem.Columns)
                            {
                                html = html + "<td  style='color: #000; border-style: solid; border-width: 1px; border-collapse: collapse'>" + dtrowrem[dtcol].ToString() + "</td>";

                            }
                            html = html + "</tr>";
                        }
                        html = html + "</table>";
                        mail(bmmail, rmmail, html, DateTime.Today);
                    }
                    else
                    {
                        mail(bmmail, rmmail, "No Reminders For Today", DateTime.Today.AddDays(1));
                    }

                }




            }

            conn.Close();
        }
        public static void visitmail(string BmAddress, string rmaddress, string body, DateTime day)
        {
            /****/
            /**********Mail Sender************/
            MailMessage msgMail = new MailMessage();

            MailMessage myMessage = new MailMessage();
            myMessage.From = new MailAddress("techsupport2@tradenetstockbroking.in", "CRR Software");
            myMessage.Bcc.Add("techsupport2@tradenetstockbroking.in");
          //  myMessage.To.Add(BmAddress);
            myMessage.To.Add(rmaddress);
            //myMessage.To.Add("co-ordination@tradenetstockbroking.in");
            //myMessage.To.Add("teamleader@tradenetstockbroking.in");
            //myMessage.To.Add("techsupport2@tradenetstockbroking.in");
            myMessage.Subject = "CRR Visit Report " + day.ToString("dd-MM-yyyy");
            string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>PLEASE FIND THE Visit Done FOR THE CRR SOFWARE AS FOLLOWS FOR YOUR KIND PERUSAL.</h3></br></br>" + body + "</br></br><h4>THANKING YOU,</h4></br><h4>TECHSUPPORT TEAM.</h4>";


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
        public static void mail(string BmAddress,string rmaddress,string body,DateTime day)
        { 
            /****/
            /**********Mail Sender************/
            MailMessage msgMail = new MailMessage();

            MailMessage myMessage = new MailMessage();
            myMessage.From = new MailAddress("techsupport2@tradenetstockbroking.in", "CRR Software");
            myMessage.Bcc.Add("techsupport2@tradenetstockbroking.in");
            myMessage.CC.Add(BmAddress);
            myMessage.To.Add(rmaddress);
            //myMessage.To.Add("co-ordination@tradenetstockbroking.in");
            //myMessage.To.Add("teamleader@tradenetstockbroking.in");
            //myMessage.To.Add("techsupport2@tradenetstockbroking.in");
            myMessage.Subject = "CRR Reminders " + day.ToString("dd-MM-yyyy");
            string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>PLEASE FIND THE Reminders FOR THE CRR SOFWARE AS FOLLOWS FOR YOUR KIND PERUSAL.</h3></br></br>" + body + "</br></br><h4>THANKING YOU,</h4></br><h4>TECHSUPPORT TEAM.</h4>";


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
        protected void Session_Start(object sender, EventArgs e)
        {
             Application.Lock();
             Application["ActiveUsers"] =Convert.ToInt64(  Application["ActiveUsers"] )+ 1;
        Application.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveUsers"] = Convert.ToInt64(Application["ActiveUsers"]) - 1;
            Application.UnLock();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

    /********
     * Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
          
        Dim escaltimer As New System.Timers.Timer()
        escaltimer.Interval = 60000 * 1
        escaltimer.Enabled = True
        ' Add handler for Elapsed event
        AddHandler escaltimer.Elapsed, New System.Timers.ElapsedEventHandler(AddressOf HandlerSub)
        escaltimer.Stop()
        escaltimer.Start()
 
        Application("ActiveUsers") = 0
    End Sub
    Sub HandlerSub(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        somman_functions_NM.commen_funcs.Escalate_query()
        Dim str1 As String = DateTime.Now.ToString("HH:mm")
        Dim str2 As String = DateTime.Today.AddHours(18).AddMinutes(30).ToString("HH:mm")
        
        If (str1.Equals(str2)) Then
            somman_functions_NM.commen_funcs.auto_mail()
        End If
      
        
    End Sub
     * ****/
}