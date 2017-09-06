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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;
namespace InvestmentSummary
{
    public partial class ReminderSetting : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            string dayofweek = DateTime.Today.DayOfWeek.ToString();
            if (StatusDropDownList2.SelectedIndex == 0)
            {

                DateTextBox3_CalendarExtender.StartDate = DateTime.Today.AddDays(0);
                if (dayofweek.Equals("Monday"))
                {
                    remdate.StartDate = DateTime.Today.AddDays(0);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                else
                {
                    remdate.StartDate = DateTime.Today.AddDays(0);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }

                DropDownList1.Visible = false;
                DropDownList2.Visible = false;
                DropDownList3.Visible = false;
                visitloc.Visible = false;
                DropDownList4.Visible = false;
                OtherTextBox2.Visible = false;
                remdatelabel0.Visible = false;
                DateTextBox3.Visible = false;
                remdatelabel.Text = "Reminder Date";
                StatusDropDownList11.Text = "Later On";
            }
            if (StatusDropDownList2.SelectedIndex == 1)
            {

                DateTextBox3_CalendarExtender.StartDate = DateTime.Today.AddDays(-1);
                if (dayofweek.Equals("Monday"))
                {
                    remdate.StartDate = DateTime.Today.AddDays(-2);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                else
                {
                    remdate.StartDate = DateTime.Today.AddDays(-1);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                DropDownList1.Visible = true;
                DropDownList2.Visible = true;
                DropDownList3.Visible = true;
                visitloc.Visible = true;
                DropDownList4.Visible = true;
                remdatelabel0.Visible = true;
                DateTextBox3.Visible = true;
                // OtherTextBox2.Visible = true;
                remdatelabel.Text = "Visit Done Date";
                StatusDropDownList11.Text = "Visit Done";
            }
            
                Label2.Text = Session["role"].ToString();
            Label3.Text = Session["Branch"].ToString();
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

            if (!IsPostBack)
            {
                DropDownList1.Visible = false;
                DropDownList2.Visible = false;
                DropDownList3.Visible = false;
                visitloc.Visible = false;
                DropDownList4.Visible = false;
                OtherTextBox2.Visible = false;  
                lblClientCode.Text = Request.QueryString["ClientCode"];
                lblClientName.Text = Request.QueryString["ClientName"];
                if (!string.IsNullOrEmpty(lblClientCode.Text))
                {

                    GridView2.DataBind();
                }
            }

          
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
           
            conn.Open();
            if (!string.IsNullOrEmpty(TextBox1.Text.Trim()))
            {
                cmd = conn.CreateCommand();
                if (Label2.Text.Equals("BM"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%' and Branch='" + Label3.Text + "'";
                }
                else if (Label2.Text.Equals("RM"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%' and RM='" + Session["login"].ToString() + "'";

                }
                else if (Label2.Text.Equals("Admin"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%'";

                }
                else if (Label2.Text.Equals("MNG"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%'";

                }
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dt = new DataTable();
                    dt.Load(dr);
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    MessageBox.Show("No Data Found..!");
                }
                dr.Close();
            }
            else
            {
                MessageBox.Show("Enter Client Name..!");
            }
            //cmd.CommandText="
            conn.Close();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gr = GridView1.SelectedRow;
            lblClientCode.Text = gr.Cells[4].Text;
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "select ClientName from ClientMaster where ClientCode='" + lblClientCode.Text + "'";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                lblClientName.Text = dr[0].ToString();

            }
            else
            {
                MessageBox.Show("Family Leader Name Not Found..!");
            }

            conn.Close();
            

        }

        protected void Button3_Click(object sender, EventArgs e)
        { string location = "";
            if (DropDownList4.SelectedIndex == 3)
            {
                location = OtherTextBox2.Text;
            }
            else
            {
                location = DropDownList4.SelectedValue;
            }
            if (!string.IsNullOrEmpty(location))
            {
                if (!string.IsNullOrEmpty(lblClientCode.Text.Trim()))
                {
                    conn.Open();
                    cmd = conn.CreateCommand();

                    if (DropDownList4.SelectedIndex == 3)
                    {
                        location = OtherTextBox2.Text;
                    }
                    else
                    {
                        location = DropDownList4.SelectedValue;
                    }
                    string datetime = DateTextBox2.Text + " " + DropDownList1.SelectedValue.Trim() + ":" + DropDownList2.SelectedValue + ":00 " + DropDownList3.SelectedValue;
                    cmd.CommandText = "insert into Reminder(RemDate,BM_RM_Name,ClientCode,ClientName,Remark,Status,Branch,Location)values(@RemDate,@BM_RM_Name,@ClientCode,@ClientName,@Remark,@Status,@Branch,@Location) ";
                    cmd.Parameters.AddWithValue("RemDate", Convert.ToDateTime(datetime));
                    cmd.Parameters.AddWithValue("BM_RM_Name", Session["login"].ToString());
                    cmd.Parameters.AddWithValue("ClientCode", lblClientCode.Text);
                    cmd.Parameters.AddWithValue("ClientName", lblClientName.Text);
                    cmd.Parameters.AddWithValue("Remark", RemarkTextBox3.Text);
                    cmd.Parameters.AddWithValue("Status", StatusDropDownList11.Text);
                    cmd.Parameters.AddWithValue("Branch", Label3.Text);
                    cmd.Parameters.AddWithValue("Location", location);
                    cmd.ExecuteNonQuery();

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "update ClientMaster set VisitStatus='" + StatusDropDownList11.Text + "' where FamilyCode='" + lblClientCode.Text + "'";
                    cmd.ExecuteNonQuery();
                    if (StatusDropDownList11.Text.Equals("Visit Done"))
                    {

                         datetime = DateTextBox3.Text;
                        cmd.CommandText = "insert into Reminder(RemDate,BM_RM_Name,ClientCode,ClientName,Remark,Status,Branch,Location)values(@RemDate,@BM_RM_Name,@ClientCode,@ClientName,@Remark,@Status,@Branch,@Location) ";
                        cmd.Parameters.AddWithValue("RemDate", Convert.ToDateTime(datetime));
                        cmd.Parameters.AddWithValue("BM_RM_Name", Session["login"].ToString());
                        cmd.Parameters.AddWithValue("ClientCode", lblClientCode.Text);
                        cmd.Parameters.AddWithValue("ClientName", lblClientName.Text);
                        cmd.Parameters.AddWithValue("Remark","Next Review");
                        cmd.Parameters.AddWithValue("Status", "Later On");
                        cmd.Parameters.AddWithValue("Branch", Label3.Text);
                        cmd.Parameters.AddWithValue("Location", "");
                        cmd.ExecuteNonQuery();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "update ClientMaster set NextReviewDate='" + datetime + "',  VisitStatus='" + StatusDropDownList11.Text + "' where FamilyCode='" + lblClientCode.Text + "'";
                        cmd.ExecuteNonQuery();
                    }

                    GridView2.DataBind();
                    conn.Close();
                    MessageBox.Show("Saved");
                    StatusDropDownList2.SelectedIndex = 0;
                    Response.Redirect("BM_RM_Page.aspx");
                }
                else
                {
                    MessageBox.Show("Select Client First");
                }
            }
            else
            {
                MessageBox.Show("Add Location");
            }
        
        }

        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList4.SelectedIndex == 3)
            {
                OtherTextBox2.Visible = true;
            }
            else
            {
                OtherTextBox2.Visible = false;
            }
        }

        protected void StatusDropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
             string dayofweek = DateTime.Today.DayOfWeek.ToString();
            if (StatusDropDownList2.SelectedIndex == 0)
            {
               
                DateTextBox3_CalendarExtender.StartDate = DateTime.Today.AddDays(1);
                if (dayofweek.Equals("Monday"))
                {
                    remdate.StartDate = DateTime.Today.AddDays(1);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                else
                {
                    remdate.StartDate = DateTime.Today.AddDays(1);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                
                DropDownList1.Visible = false;
                DropDownList2.Visible = false;
                DropDownList3.Visible = false;
                visitloc.Visible = false;
                DropDownList4.Visible = false;
                OtherTextBox2.Visible = false;
                remdatelabel0.Visible = false;
                DateTextBox3.Visible = false;
                remdatelabel.Text = "Reminder Date";
                StatusDropDownList11.Text = "Later On";
            }
            if (StatusDropDownList2.SelectedIndex == 1)
            {

                DateTextBox3_CalendarExtender.StartDate = DateTime.Today.AddDays(-1);
                if (dayofweek.Equals("Monday"))
                {
                    remdate.StartDate = DateTime.Today.AddDays(-2);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                else
                {
                    remdate.StartDate = DateTime.Today.AddDays(-1);
                    DateTextBox3.Text = DateTime.Today.AddDays(90).ToString("dd-MMM-yyyy");
                }
                DropDownList1.Visible = true  ;
                DropDownList2.Visible = true;
                DropDownList3.Visible = true;
                visitloc.Visible = true;
                DropDownList4.Visible = true;
                remdatelabel0.Visible = true;
                DateTextBox3.Visible = true;
               // OtherTextBox2.Visible = true;
                remdatelabel.Text = "Visit Done Date";
                StatusDropDownList11.Text = "Visit Done";
            }
            
        }
    }
}