﻿using System;
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
    public partial class ReminderUpdate : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            string dayofweek = DateTime.Today.DayOfWeek.ToString();
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
          
            Label2.Text = Session["role"].ToString();
            Label3.Text = Session["Branch"].ToString();
            LinkButton lnb = (LinkButton)Master.FindControl("LinkButton2");
            lnb.Visible = false;
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            if (!IsPostBack)
            {
                DropDownList1.Visible = false;
                DropDownList2.Visible = false;
                DropDownList3.Visible = false;
                visitloc.Visible = false;
                DropDownList4.Visible = false;
                OtherTextBox2.Visible = false;
            }
            if (GridView1.Rows.Count == 0)
            {
                Response.Redirect("BM_RM_Page.aspx");
            }
           
        }

        protected void StatusDropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StatusDropDownList2.SelectedIndex == 0)
            {
                DropDownList1.Visible = false;
                DropDownList2.Visible = false;
                DropDownList3.Visible = false;
                visitloc.Visible = false;
                DropDownList4.Visible = false;
                OtherTextBox2.Visible = false;
                remdatelabel.Text = "Reminder Date";
                StatusLabel.Text = "Later On";
                remdatelabel0.Visible = false;
                DateTextBox3.Visible = false;
            }
            if (StatusDropDownList2.SelectedIndex == 1)
            {
                DropDownList1.Visible = true;
                DropDownList2.Visible = true;
                DropDownList3.Visible = true;
                visitloc.Visible = true;
                DropDownList4.Visible = true;
                //OtherTextBox2.Visible = true;
                remdatelabel.Text = "Visit Done Date";
                StatusLabel.Text = "Visit Done";
                remdatelabel0.Visible = true;
                DateTextBox3.Visible = true;
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

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblremid.Text.Trim()))
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "update Reminder set Status='Postponded' where RemID=" + lblremid.Text;
                cmd.ExecuteNonQuery();
                string location = "";
                if (DropDownList4.SelectedIndex == 3)
                {
                    location = OtherTextBox2.Text;
                }
                else
                {
                    location = DropDownList4.SelectedValue;
                }
                if (!string.IsNullOrEmpty(location.Trim()))
                {
                    if (!string.IsNullOrEmpty(lblClientCode.Text.Trim()))
                    {
                        //conn.Open();
                        cmd = conn.CreateCommand();

                        cmd.CommandText = "insert into Reminder(RemDate,BM_RM_Name,ClientCode,ClientName,Remark,Status,Branch,Location)values(@RemDate,@BM_RM_Name,@ClientCode,@ClientName,@Remark,@Status,@Branch,@Location) ";

                        //   cmd.CommandText = "insert into Reminder(RemDate,BM_RM_Name,ClientCode,ClientName,Remark,Status,Branch)values(@RemDate,@BM_RM_Name,@ClientCode,@ClientName,@Remark,@Status,@Branch) ";
                        cmd.Parameters.AddWithValue("RemDate", Convert.ToDateTime(DateTextBox2.Text + " " + DropDownList1.SelectedValue.Trim() + ":" + DropDownList2.SelectedValue + ":00 " + DropDownList3.SelectedValue));

                        cmd.Parameters.AddWithValue("BM_RM_Name", Session["login"].ToString());
                        cmd.Parameters.AddWithValue("ClientCode", lblClientCode.Text);
                        cmd.Parameters.AddWithValue("ClientName", lblClientName.Text);
                        cmd.Parameters.AddWithValue("Remark", RemarkTextBox1.Text);
                        cmd.Parameters.AddWithValue("Status", StatusLabel.Text);
                        cmd.Parameters.AddWithValue("Branch", Label3.Text);
                        cmd.Parameters.AddWithValue("Location", location);
                        cmd.ExecuteNonQuery();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "update ClientMaster set  VisitStatus='" + StatusLabel.Text + "' where FamilyCode='" + lblClientCode.Text + "'";
                        cmd.ExecuteNonQuery();
                        if (StatusLabel.Text.Equals("Visit Done"))
                        {

                            string datetime = DateTextBox3.Text;
                            cmd.CommandText = "insert into Reminder(RemDate,BM_RM_Name,ClientCode,ClientName,Remark,Status,Branch,Location)values(@RemDate,@BM_RM_Name,@ClientCode,@ClientName,@Remark,@Status,@Branch,@Location) ";
                            cmd.Parameters.AddWithValue("RemDate", Convert.ToDateTime(datetime));
                            cmd.Parameters.AddWithValue("BM_RM_Name", Session["login"].ToString());
                            cmd.Parameters.AddWithValue("ClientCode", lblClientCode.Text);
                            cmd.Parameters.AddWithValue("ClientName", lblClientName.Text);
                            cmd.Parameters.AddWithValue("Remark", "Next Review");
                            cmd.Parameters.AddWithValue("Status", "Later On");
                            cmd.Parameters.AddWithValue("Branch", Label3.Text);
                            cmd.Parameters.AddWithValue("Location", "");
                            cmd.ExecuteNonQuery();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "update ClientMaster set NextReviewDate='"+datetime+"',  VisitStatus='" + StatusLabel.Text + "' where FamilyCode='" + lblClientCode.Text + "'";
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Saved..!");
                        StatusDropDownList2.SelectedIndex = 0;
                        lblClientCode.Text = "";
                        lblClientName.Text = "";
                        RemarkTextBox1.Text = "";
                        lblremid.Text = "";
                        lblremark.Text = "";
                        DateTextBox2.Text = "";
                        GridView1.DataBind();
                        if (GridView1.Rows.Count == 0)
                        {
                            Response.Redirect("BM_RM_Page.aspx");
                        }


                        //Response.Redirect("BM_RM_Page.aspx");

                        // GridView2.DataBind();
                        //conn.Close();
                    }
                    else
                    {
                        MessageBox.Show("Select Reminder First");
                    }
                }
                else
                {
                    MessageBox.Show("Enter Location..!");
                
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("Select Reminder First");
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
               
               // remdate.StartDate = DateTime.Today;
                Label2.Text = Session["role"].ToString();
                Label3.Text = Session["Branch"].ToString();
                lblremid.Text = GridView1.SelectedRow.Cells[1].Text;
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select * from Reminder where   RemID='" + lblremid.Text + "' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lblClientCode.Text = dr["ClientCode"].ToString();
                        lblClientName.Text = dr["ClientName"].ToString();
                        lblremark.Text = dr["Remark"].ToString();
                        StatusLabel.Text = dr["Status"].ToString().Trim();

                        DateTextBox2.Text = "";
                        // DateTextBox2.Text = Convert.ToDateTime(dr["RemDate"].ToString()).ToString("dd-MMM-yyyy");
                    }

                }
                dr.Close();
                conn.Close();
            }
        
    }
}