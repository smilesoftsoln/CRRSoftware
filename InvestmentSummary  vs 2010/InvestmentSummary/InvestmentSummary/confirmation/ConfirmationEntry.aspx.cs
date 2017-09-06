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

namespace InvestmentSummary.confirmation
{
    public partial class ConfirmationEntry : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        DateTime maxDate;
        SqlDataReader dr;
        string yday;
       static string staff_name ;//= Request.QueryString["staffname"];
       static string Dept_Branch;//= Request.QueryString["deptbranch"];
        protected void Page_Load(object sender, EventArgs e)
        {
           
           
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open();
            cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "select max(TradeDate) from OFLReport";
                maxDate = (DateTime)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                maxDate = DateTime.Today;
            }
            conn.Close();
            ReasonForPendingRow.Visible = false;
            //if (!IsPostBack)
            //{ 
                
                
                ConfirmationDateTextBox4.Text = DateTime.Today.ToString("dd-MMM-yyyy");
            ConfirmationDateTextBoxTextBox1.Text = DateTime.Today.AddDays(1).ToString("dd-MMM-yyyy");
            ConfirmationDateTextBox4.Enabled = false;

                staff_name = Request.QueryString["staffname"];
                Dept_Branch = Request.QueryString["deptbranch"];
                  yday = Request.QueryString["yday"];
              
                if (!string.IsNullOrEmpty(staff_name))
                {
                    UserNameLabel13.Text = staff_name;
                }
                else
                {
                    UserNameLabel13.Text = "xyz";
                }
                if (!string.IsNullOrEmpty(Dept_Branch))
                {
                    Dept_BranchLabel14.Text = Dept_Branch;
                }
                else
                {
                    Dept_BranchLabel14.Text = "DEALING";
                }
               
                UnRegisteredTextBox3.Enabled = false;
                ConfirmationToTextBox5.Enabled = false;
                if (!string.IsNullOrEmpty(yday))
                {
                    ReasonForPendingRow.Visible = true;
                    pendingDiv.Visible = false; 
                    YesterdaysLinkButton2.Font.Bold = true;
                    TodaysLinkButton3.Font.Bold = false;
                    ConfirmationDateTextBox4.Text =Convert.ToDateTime(yday).ToString("dd-MMM-yyyy");
                    ConfirmationDateTextBoxTextBox1.Text = Convert.ToDateTime(yday).AddDays(1).ToString("dd-MMM-yyyy");
                }
                else
                {
                    ReasonForPendingRow.Visible = false;
                    pendingDiv.Visible = false; 
                    YesterdaysLinkButton2.Font.Bold = false;
                    TodaysLinkButton3.Font.Bold = true;
                  
                    conn.Open();

                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select Subbroker from SBCODE where BranchName='" + Dept_BranchLabel14.Text + "'";
                    string sbcode1 = (string)cmd.ExecuteScalar();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select TD.ClientCode,CCL.clientname as ClientName,TD.Terminal,TD.Segment,CONVERT(VARCHAR(11),TD.TradeDate,106) as  TradeDate  from OFLReport TD,Cust_Client_Master CCL where Matched!='Confirmed'    and SubBroker='" + sbcode1 + "' and CCL.clientcode=TD.ClientCode and TD.TradeDate='" + maxDate + "'";
                    dr = cmd.ExecuteReader();
                    DataTable dtAll = new DataTable();
                    if (dr.HasRows)
                    {
                        dtAll.Load(dr);
                    }
                    dr.Close();
                    if (dtAll.Rows.Count > 0)
                    {
                        pendingDiv.Visible = true; 
                        PendingGridView2.DataSource = dtAll;
                        PendingGridView2.DataBind();
                    }







                    conn.Close();



                }
            
            //}


        }
        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "select clientname,landline1,landline2,mobileno from Cust_Client_Master where clientcode='" + ClientCodeTextBox2.Text.Trim() + "'";

            dr = cmd.ExecuteReader();


   

            ContactRadioButtonList1.Items[0].Enabled = true;
            ContactRadioButtonList1.Items[1].Enabled = true;
            ContactRadioButtonList1.Items[2].Enabled = true;

            if (dr.HasRows)
            {
                dr.Read();
                ClientNameLabel15.Text = dr["clientname"].ToString();
                ContactRadioButtonList1.Items[0].Text = dr["landline1"].ToString();
                ContactRadioButtonList1.Items[1].Text = dr["landline2"].ToString();
                ContactRadioButtonList1.Items[2].Text = dr["mobileno"].ToString();
            }

            dr.Close();

            if (string.IsNullOrEmpty(ContactRadioButtonList1.Items[0].Text.Trim()))
            {
                ContactRadioButtonList1.Items[0].Text = "Not Available";

                ContactRadioButtonList1.Items[0].Enabled = false;
            }

            if (string.IsNullOrEmpty(ContactRadioButtonList1.Items[1].Text.Trim()))
            {
                ContactRadioButtonList1.Items[1].Text = "Not Available";
                ContactRadioButtonList1.Items[1].Enabled = false;

            }
            if (string.IsNullOrEmpty(ContactRadioButtonList1.Items[2].Text.Trim()))
            {
                ContactRadioButtonList1.Items[2].Enabled = false;
                ContactRadioButtonList1.Items[2].Text = "Not Available";
            }














            conn.Close();
        }

        protected void ContactRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ContactRadioButtonList1.SelectedItem.Text.Trim() == "Other")
            {
                UnRegisteredTextBox3.Enabled = true;
                UnRegisteredTextBox3.Text = "";
            }
            else
            {
                UnRegisteredTextBox3.Text = ContactRadioButtonList1.SelectedItem.Text;
                UnRegisteredTextBox3.Enabled = false;
            
            }
        }

        protected void DropDownList5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConfirmationToDropDownList5.SelectedItem.Text.Trim() == "Other")
            {
                ConfirmationToTextBox5.Enabled = true;
                ConfirmationToTextBox5.Text = "";
            }
            else
            {
                if (ConfirmationToDropDownList5.SelectedIndex != 0)
                {
                    ConfirmationToTextBox5.Text = ConfirmationToDropDownList5.Text;
                    ConfirmationToTextBox5.Enabled = false;
                }
                else
                {
                    ConfirmationToTextBox5.Text = "";
                    ConfirmationToTextBox5.Enabled = false;

                }

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
 //            ID
//UserName
//Dept_Branch
//TerminalNo
//ClientCode
//ClientName
//ContactNo
//ContactType
//ConfirmationDate
//Segment
//GivenTo
            if (!string.IsNullOrEmpty(ClientNameLabel15.Text.Trim()))
            {
            if (!string.IsNullOrEmpty(SegmentLabel8.Text.Trim()))
            {

                if ((!string.IsNullOrEmpty(yday)) && string.IsNullOrEmpty(ReasonForPendingTextBox1.Text.Trim()))
               
            {

                MessageBox.Show("Please Enter Reason For Pending Confirmation..");
            
            }
                 else
                {
               
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "insert into Confirmation(UserName,Dept_Branch,TerminalNo,ClientCode,ClientName,ContactNo,ContactType,ConfirmationDate,Segment,GivenTo,OtherRemark,ReasonForPending)values(@UserName,@Dept_Branch,@TerminalNo,@ClientCode,@ClientName,@ContactNo,@ContactType,@ConfirmationDate,@Segment,@GivenTo,@OtherRemark,@ReasonForPending)";
                //@UserName,@Dept_Branch,@TerminalNo,@ClientCode,@ClientName,@ContactNo,@ContactType,@ConfirmationDate,@Segment,@GivenTo
                cmd.Parameters.AddWithValue("@UserName", UserNameLabel13.Text.Trim());
                cmd.Parameters.AddWithValue("@Dept_Branch", Dept_BranchLabel14.Text.Trim());
                cmd.Parameters.AddWithValue("@TerminalNo", TerminalNoTextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@ClientCode", ClientCodeTextBox2.Text.ToUpper().Trim());
                cmd.Parameters.AddWithValue("@ClientName", ClientNameLabel15.Text.Trim());
                if (ContactRadioButtonList1.SelectedItem.Text.Trim() == "Other")
                {
                    cmd.Parameters.AddWithValue("@ContactNo", UnRegisteredTextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactType", "UnRegistered");

                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContactNo", ContactRadioButtonList1.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactType", "Registered");

                }

                cmd.Parameters.AddWithValue("@ConfirmationDate", Convert.ToDateTime(ConfirmationDateTextBox4.Text + " " + HH_DropDownList1.SelectedValue + ":" + MM_DropDownList2.SelectedValue + " " + AM_PM_DropDownList3.SelectedValue));
                //            cmd.Parameters.AddWithValue("@Segment", SegmentDropDownList4.SelectedValue);
                cmd.Parameters.AddWithValue("@Segment", SegmentLabel8.Text.Trim());

                if (ConfirmationToDropDownList5.SelectedItem.Text.Trim() == "Other")
                {
                    cmd.Parameters.AddWithValue("@GivenTo", ConfirmationToTextBox5.Text.Trim());

                }
                else
                {
                    cmd.Parameters.AddWithValue("@GivenTo", ConfirmationToDropDownList5.Text.Trim());


                }

                cmd.Parameters.AddWithValue("@OtherRemark", OtherRemarkTextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@ReasonForPending", ReasonForPendingTextBox1.Text.Trim());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Saved Successfully");
                conn.Close();
                GridView1.DataBind();
                ReasonForPendingTextBox1.Text = "";
                TerminalNoTextBox1.Text = "";
                ClientCodeTextBox2.Text = "";
                ClientNameLabel15.Text = "";
                ContactRadioButtonList1.ClearSelection();
                CheckBoxList1.ClearSelection();
                UnRegisteredTextBox3.Text = "";
                ConfirmationToTextBox5.Text = "";
                ConfirmationToDropDownList5.SelectedIndex = 0;

                OtherRemarkTextBox1.Text = "";

                SegmentLabel8.Text = "";

                /********/


                DataTable dtOFL = new DataTable();
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select * from OFLReport where TradeDate='" + ConfirmationDateTextBox4.Text + "'";
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
                    cmd.CommandText = "select * from Confirmation where TerminalNo='" + DRW["Terminal"].ToString().Trim() + "' and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' and  ClientCode='" + DRW["ClientCode"].ToString().Trim() + "' and  ConfirmationDate>='" + ConfirmationDateTextBox4.Text.Trim() + "' and ConfirmationDate<='" + Convert.ToDateTime(ConfirmationDateTextBox4.Text.Trim()).AddDays(1).ToString("dd-MMM-yyyy") + "' Order by ID desc";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string query = "update OFLReport set  ReasonForPending=@ReasonForPending, OtherRemark=@OtherRemark, UserName= @UserName,Dept_Branch=@Dept_Branch,CallTime=@CallTime,ContactNo=@ContactNo,ContactType=@ContactType,GivenTo=@GivenTo,  Matched='Confirmed' where  TradeDate='" + ConfirmationDateTextBox4.Text + "' and ClientCode= @ClientCode and Terminal= @Terminal and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' ";
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

                    //    cmd.Parameters.AddWithValue("@Segment", DRW["Segment"].ToString().Trim());

                        dr.Close();
                          

                        cmd.ExecuteNonQuery();

                    }
                    dr.Close();
                }
                conn.Close();

                /*********/
            }
                
            }
            else
            {

                MessageBox.Show("Select Segment..!");
            }
            }
            else
            {

                MessageBox.Show("Client Name Blank Not Allowed");
            }
        }

        protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SegmentLabel8.Text = "";
            foreach (ListItem lst in CheckBoxList1.Items)
            {

                if (lst.Selected == true)
                {
                    SegmentLabel8.Text = SegmentLabel8.Text + " " + lst.Text;
                }

            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfirmationEntry.aspx?staffname=" + UserNameLabel13.Text + "&deptbranch=" + Dept_BranchLabel14.Text + "&yday=" + maxDate.ToString("dd-MMM-yyyy"));

        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfirmationEntry.aspx?staffname=" + UserNameLabel13.Text + "&deptbranch=" + Dept_BranchLabel14.Text);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConfirmationEntry.aspx?staffname=" + UserNameLabel13.Text + "&deptbranch=" + Dept_BranchLabel14.Text + "&yday=" + maxDate.ToString("dd-MMM-yyyy"));

        }
    }
}