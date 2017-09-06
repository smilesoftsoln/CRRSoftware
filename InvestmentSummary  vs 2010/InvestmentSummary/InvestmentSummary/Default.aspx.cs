using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Configuration;
namespace InvestmentSummary
{
    public partial class _Default : System.Web.UI.Page
    {
        SqlConnection conn;
        static string role = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

        }
        private bool ValidateUser(string userName, string passWord)
        {

           
          SqlCommand cmd;
            string lookupPassword = null;

            // Check for invalid userName.
            // userName must not be null and must be between 1 and 15 characters.
            if ((null == userName) || (0 == userName.Length) || (userName.Length > 50))
            {
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of userName failed.");
                return false;
            }
            // Check for invalid passWord.
            // passWord must not be null and must be between 1 and 25 characters.
            if ((null == passWord) || (0 == passWord.Length) || (passWord.Length > 50))
            {
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of passWord failed.");
                return false;
            }

            try
            {
                // Consult with your SQL Server administrator for an appropriate connection
                // string to use to connect to your local SQL Server.
               // conn.Open();

                //// Create SqlCommand to select pwd field from users table given supplied userName.
                cmd = new SqlCommand("Select username,password,Branch from UserMaster where username=@userName", conn);
                cmd.Parameters.AddWithValue("@userName", txtuser.Text);
             

                //// Execute command and fetch pwd field into lookupPassword string.
              SqlDataReader dr=cmd.ExecuteReader();
              if (dr.HasRows)
              {
                  dr.Read();
                  lookupPassword = dr["password"].ToString();
                  Session["login"] = dr["username"].ToString();
                  Session["Branch"] = dr["Branch"].ToString();
              }
              dr.Close();
                //// Cleanup command and connection objects.
                cmd.Dispose();
                cmd = new SqlCommand("Select role from UserMaster where username=@userName", conn);
                cmd.Parameters.AddWithValue("@userName", txtuser.Text);



                //// Execute command and fetch pwd field into lookupPassword string.
                role = (string)cmd.ExecuteScalar();
                Session["role"] = role;
                //// Cleanup command and connection objects.
                cmd.Dispose();
                //conn.Dispose();


            }
            catch (Exception ex)
            {
                // Add error handling here for debugging.
                // This error message should not be sent back to the caller.
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
            }

            // If no password found, return false.
            if (null == lookupPassword)
            {
                // You could write failed login attempts here to event log for additional security.
                return false;
            }

            // Compare lookupPassword and input passWord, using a case-sensitive comparison.
            return (0 == string.Compare(lookupPassword, passWord, false));

            //Label4.Visible = true;
            //Label4.Text=("Login sucessfully...");

        }
        protected void cmdlogin_Click(object sender, EventArgs e)
        {
            string macid = txtuser0.Text.ToUpper().Replace(':', '-');// "40-61-86-0C-FA-DD";
            if ( ! string.IsNullOrEmpty(macid.Trim()))//change to ! when deploy
            {

              //  MessageBox.Show("MAC ID:" + macid);
                conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

                SqlCommand cmd = new SqlCommand("select mac from macmapping  where mac='" + macid.Trim().ToUpper() + "'", conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if ( ! dr.HasRows)//change to ! when deploy
                {
                   MessageBox.Show("Access Denied Contact Administrator");
                   //string strmacid = (string)cmd.ExecuteScalar();
                   dr.Close();
                }  
                  
                else
                {
                /******************/
                //FormsAuthenticationTicket tkt;
                //string cookiestr;
                //HttpCookie ck;

                //tkt = new FormsAuthenticationTicket(1, txtuser.Text, DateTime.Now,
                //DateTime.Now.AddMinutes(180), false, "your custom data");
                //cookiestr = FormsAuthentication.Encrypt(tkt);
                //ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);

                //ck.Path = FormsAuthentication.FormsCookiePath;
                //Session.Add("login", txtuser.Text);
                //Response.Cookies.Add(ck);
                /*********************/
                //if (logged.Trim() != "Online" )
                //{
                    dr.Close();
                if (ValidateUser(txtuser.Text, txtpassword.Text))
                {
                    FormsAuthenticationTicket tkt;
                    string cookiestr;
                    HttpCookie ck;

                    tkt = new FormsAuthenticationTicket(1, txtuser.Text, DateTime.Now,
                    DateTime.Now.AddMinutes(180), true, "your custom data");
                    cookiestr = FormsAuthentication.Encrypt(tkt);
                    ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);

                    ck.Path = FormsAuthentication.FormsCookiePath;
                    //Session.Add("login", txtuser.Text);
                    Response.Cookies.Add(ck);
                    conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

                    //SqlCommand cmd = new SqlCommand("select loggedin from UserMaster  where username='" + Session["login"].ToString() + "'", conn);
                    //conn.Open();
                    //  string logged = (string)cmd.ExecuteScalar();


                    //if (txtuser.Text.Trim() != "Admin")
                    //{
                    //    cmd = new SqlCommand("Update UserMaster set loggedin='Online' where username='" + Session["login"].ToString() + "'", conn);


                    //    cmd.ExecuteNonQuery();
                    //}
                    //conn.Close();
                    string strRedirect;
                    strRedirect = Request["ReturnUrl"];
                    if (strRedirect == null)
                    {
                        if (role.Equals("BM"))
                        {
                            Session["role"] = "BM";
                            strRedirect = "~/ReminderUpdate.aspx";
                        }
                        else if (role.Equals("RM"))
                        {
                            Session["role"] = "RM";
                            strRedirect = "~/ReminderUpdate.aspx";
                        }
                        else if (role.Equals("MNG"))
                        {
                            Session["role"] = "MNG";
                            strRedirect = "~/ReminderUpdate.aspx";
                        }
                        else if (role.Equals("Mentor"))
                        {
                            Session["role"] = "Team Leader";
                            strRedirect = "~/MentorPage.aspx";
                        }
                        ////else if (role.Equals("Management"))
                        ////{
                        ////    Session["role"] = "Management";
                        ////    strRedirect = "~/TL_OutBoundCC_MapUser.aspx";
                        ////}
                        //else if (role.Equals("Management"))
                        //{
                        //    Session["role"] = "Management";
                        //    strRedirect = "~/Management_OutBoundCC.aspx";
                        //}
                        //else if (role.Equals("Manager"))
                        //{
                        //    Session["role"] = "Manager";
                        //    strRedirect = "~/Manager_OutBoundCC.aspx";
                        //}
                        else
                        {
                            Session["role"] = "Admin";
                            strRedirect = "~/Admin.aspx";
                        }

                    }
                    Response.Redirect(strRedirect);
                }
                else
                {

                    //Label5.Visible = true;
                    Label4.Text = ("Please re-enter your valid Username and Password... ");

                }
                //}
                //else
                //{
                //    Label4.Text = ("Multiple login not allowed... ");


                //}
            }

                conn.Close();

            }
            else
            {
                MessageBox.Show("Enable Active X Settings  IN INTERNET EXPLORER");
            }
        }

        protected void txtuser_TextChanged(object sender, EventArgs e)
        {

        }

        protected void cmdlogin0_Click(object sender, EventArgs e)
        {

        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close(); </script>");
        }
    }
}
