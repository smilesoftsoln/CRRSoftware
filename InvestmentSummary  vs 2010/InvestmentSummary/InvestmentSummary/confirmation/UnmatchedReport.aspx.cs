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
using System.Data.Sql;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using Excel;
namespace InvestmentSummary.confirmation
{
    public partial class UnmatchedReport : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);


        }

        protected void ReportButton1_Click(object sender, EventArgs e)
        {
            if (WhereRadioButtonList1.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(DateTextBox1.Text.Trim()))
                {
                    DataTable dtOFL = new DataTable();
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select * from OFLReport where TradeDate='" + Convert.ToDateTime( DateTextBox1.Text).ToString("dd-MMM-yyyy") + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dtOFL.Load(dr);
                        dr.Close();

                    }
                    else
                    {
                     //   MessageBox.Show("OFL/ONL Report File Not Imported");
                    }
                    dr.Close();

                    foreach (DataRow DRW in dtOFL.Rows)
                    {
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select * from Confirmation where TerminalNo='" + DRW["Terminal"].ToString().Trim() + "' and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' and  ClientCode='" + DRW["ClientCode"].ToString().Trim() + "' and  ConfirmationDate>='" + Convert.ToDateTime(DateTextBox1.Text).ToString("dd-MMM-yyyy") + "' and ConfirmationDate<='" + Convert.ToDateTime(DateTextBox1.Text.Trim()).AddDays(1).ToString("dd-MMM-yyyy") + "' Order by ID desc";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            string query = "update OFLReport set ReasonForPending=@ReasonForPending, OtherRemark=@OtherRemark, UserName= @UserName,Dept_Branch=@Dept_Branch,CallTime=@CallTime,ContactNo=@ContactNo,ContactType=@ContactType,GivenTo=@GivenTo,  Matched='Confirmed' where  TradeDate=@TradeDate and ClientCode= @ClientCode and Terminal= @Terminal and  Segment like '%" + DRW["Segment"].ToString().Trim() + "%' ";
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
                            cmd.Parameters.AddWithValue("@TradeDate", Convert.ToDateTime(DateTextBox1.Text).ToString("dd-MMM-yyyy") );
                            cmd.Parameters.AddWithValue("@ClientCode", DRW["ClientCode"].ToString().Trim());
                            cmd.Parameters.AddWithValue("@Terminal", DRW["Terminal"].ToString().Trim());

                           // cmd.Parameters.AddWithValue("@Segment", DRW["Segment"].ToString().Trim());

                            dr.Close();
                          



                            cmd.ExecuteNonQuery();

                        }
                        dr.Close();
                    }

                    string where = "and ";
                    if (WhereRadioButtonList1.SelectedIndex == 0)
                    {
                        where += " Matched!='Confirmed' ";
                    }
                    if (WhereRadioButtonList1.SelectedIndex == 1)
                    {
                        where += " Matched='Confirmed' and GivenTo!='Self' ";
                    }
                    if (WhereRadioButtonList1.SelectedIndex == 2)
                    {
                        where += "Matched='Confirmed' and OtherRemark!='' ";
                    }
                    if (WhereRadioButtonList1.SelectedIndex == 3)
                    {
                        where += "Matched='Confirmed' and ContactType='UnRegistered' ";
                    }
                    if (WhereRadioButtonList1.SelectedIndex == 4)
                    {
                        where += " Matched='Confirmed' ";
                    }
                    dtOFL = new DataTable();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select TD.ClientCode,CCL.clientname as ClientName, TD.Terminal, TD.Segment,CONVERT(VARCHAR(11),TD.TradeDate,106) as  TradeDate, TD.SubBroker, TD.Matched, TD.UserName, TD.Dept_Branch ,CONVERT(VARCHAR(19),TD.CallTime) as CallTime ,TD.ContactNo ,TD.ContactType ,TD.GivenTo ,TD.OtherRemark,TD.ReasonForPending from OFLReport TD,Cust_Client_Master CCL where TradeDate>='" + Convert.ToDateTime(DateTextBox1.Text).ToString("dd-MMM-yyyy") + "' and TradeDate<='" + Convert.ToDateTime(DateTextBox2.Text).ToString("dd-MMM-yyyy") + "' and CCL.clientcode=TD.ClientCode " + where + " order by TradeDate";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dtOFL.Load(dr);
                        dr.Close();

                    }

                    GridView1.DataSource = dtOFL;
                    GridView1.DataBind();
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Select Date");
                }
            }
            else
            {

                MessageBox.Show("Select Report Type");
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        } 
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ExportData.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            StringWriter StringWriter = new System.IO.StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);

            GridView1.RenderControl(HtmlTextWriter);
            Response.Write(StringWriter.ToString());
            Response.End();
        }

        protected void DateTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}