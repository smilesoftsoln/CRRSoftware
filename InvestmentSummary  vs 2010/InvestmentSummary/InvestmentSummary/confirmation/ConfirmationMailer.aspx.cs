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
namespace InvestmentSummary.confirmation
{
    public partial class ConfirmationMailer : System.Web.UI.Page
    {

        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        DateTime maxDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "select max(TradeDate) from OFLReport";
            maxDate = (DateTime)cmd.ExecuteScalar();
            DataTable dtOFL = new DataTable();
            
            cmd = conn.CreateCommand();
            cmd.CommandText = "select * from OFLReport where TradeDate='" + maxDate.ToString("dd-MMM-yyyy") + "'";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dtOFL.Load(dr);
                dr.Close();

            }
            
            dr.Close();

            foreach (DataRow DRW in dtOFL.Rows)
            {
                cmd = conn.CreateCommand();
              //cmd.CommandText = "select * from Confirmation where TerminalNo='" + DRW["Terminal"].ToString().Trim() + "' and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' and  ClientCode='" + DRW["ClientCode"].ToString().Trim() + "' and  ConfirmationDate>='" + DateTextBox1.Text.Trim() + "' and ConfirmationDate<='" + Convert.ToDateTime(DateTextBox1.Text.Trim()).AddDays(1).ToString("dd-MMM-yyyy") + "' Order by ID desc";
                cmd.CommandText = "select * from Confirmation where TerminalNo='" + DRW["Terminal"].ToString().Trim() + "' and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' and  ClientCode='" + DRW["ClientCode"].ToString().Trim() + "' and  ConfirmationDate>='" + maxDate.ToString("dd-MMM-yyyy") + "' and ConfirmationDate<='" + Convert.ToDateTime(maxDate.ToString("dd-MMM-yyyy")).AddDays(1).ToString("dd-MMM-yyyy") + "' Order by ID desc ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string query = "update OFLReport set ReasonForPending=@ReasonForPending, OtherRemark=@OtherRemark, UserName= @UserName,Dept_Branch=@Dept_Branch,CallTime=@CallTime,ContactNo=@ContactNo,ContactType=@ContactType,GivenTo=@GivenTo,  Matched='Confirmed' where  TradeDate='" + maxDate.ToString("dd-MMM-yyyy") + "' and ClientCode= @ClientCode and Terminal= @Terminal and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' ";
                    cmd = conn.CreateCommand();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@ReasonForPending", dr["ReasonForPending"].ToString().Trim());

                    cmd.Parameters.AddWithValue("@OtherRemark", dr["OtherRemark"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@UserName", dr["UserName"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Dept_Branch", dr["Dept_Branch"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@CallTime", dr["ConfirmationDate"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@ContactNo", dr["ContactNo"].ToString().Trim());

                    cmd.Parameters.AddWithValue("@ContactType", dr["ContactType"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@GivenTo", dr["GivenTo"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@ClientCode", DRW["ClientCode"].ToString().Trim());
                    cmd.Parameters.AddWithValue("@Terminal", DRW["Terminal"].ToString().Trim());

                    //cmd.Parameters.AddWithValue("@Segment", DRW["Segment"].ToString().Trim());

                    dr.Close();
                         

                    cmd.ExecuteNonQuery();

                }
                dr.Close();
            }
            dr.Close();
            cmd = conn.CreateCommand();
            cmd.CommandText = "select distinct MailID,Branch from UserMaster where role='BM' and Branch!='Tradecenter Branch'";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
            }
            dr.Close();
            dt.Columns.Add("Subbroker");
            dt.Columns["Subbroker"].ReadOnly = false;
            foreach (DataRow drw in dt.Rows)
            {

                cmd = conn.CreateCommand();
                cmd.CommandText = "select Subbroker from SBCODE where BranchName='"+drw["Branch"].ToString()+"'";
                string sbcode = (string)cmd.ExecuteScalar();

                drw["Subbroker"] = sbcode;
            
            }


            GridView1.DataSource = dt;
            GridView1.DataBind();
        //    visitmail("techsupport2@tradenetstockbroking.in", ConvertDataTableToHTML(dt), DateTime.Today, "All Table");
            foreach (DataRow drw in dt.Rows)
            {
                
 

                cmd = conn.CreateCommand();
                cmd.CommandText = "select TD.ClientCode,CCL.clientname as ClientName,TD.Terminal,TD.Segment,CONVERT(VARCHAR(11),TD.TradeDate,106) as  TradeDate  from OFLReport TD,Cust_Client_Master CCL where Matched!='Confirmed' and SubBroker='" + drw["Subbroker"].ToString() + "' and CCL.clientcode=TD.ClientCode and TD.TradeDate='" + maxDate + "'";
                dr = cmd.ExecuteReader();
                DataTable dtAll = new DataTable();
                if (dr.HasRows)
                {
                    dtAll.Load(dr);
                    visitmail(drw["MailID"].ToString(), ConvertDataTableToHTML(dtAll), maxDate, drw["Branch"].ToString());
           }
                dr.Close();
               
            
            }
            conn.Close();
        }

        public   string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table  border='1' style='border-collapse:collapse;'>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td><b>" + dt.Columns[i].ColumnName + "</b></td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }


        public void visitmail(string BmAddress, string body, DateTime day, string subject)
        {
            /****/
            /**********Mail Sender************/
            MailMessage msgMail = new MailMessage();

            MailMessage myMessage = new MailMessage();
            myMessage.From = new MailAddress("ConfirmationSoftware@tradenetstockbroking.in", "Confirmation Software");
           myMessage.Bcc.Add("techsupport2@tradenetstockbroking.in");
           myMessage.To.Add(BmAddress);
           myMessage.To.Add("bo02@tradenetstockbroking.in");
           myMessage.To.Add("surveillance@tradenetstockbroking.in");
           myMessage.Subject = "Unconfirmed Trades Report " + day.ToString("dd-MMM-yyyy") + subject;
              string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>PLEASE FIND THE Unconfirmed Trades Report FOR THE Day "+maxDate .ToString("dd-MMM-yyyy")+" AS FOLLOWS FOR YOUR KIND PERUSAL.</h3></br></br>" + body + "</br></br><h4>THANKING YOU,</h4></br><h4>TECHSUPPORT TEAM.</h4>";


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