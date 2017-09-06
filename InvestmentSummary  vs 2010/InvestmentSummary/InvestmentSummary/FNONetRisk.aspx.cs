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

namespace InvestmentSummary
{
    public partial class FNONetRisk : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        DataTable dt;
     
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FileUpload1.SaveAs(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName));
            StreamReader sr = new StreamReader(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName));

            string strline = "";
            string[] _values = null;
            strline = sr.ReadLine();
            strline = sr.ReadLine();
            strline = sr.ReadLine();
            dt = new DataTable();
            dt.Columns.Add("ClientCode");
            dt.Columns.Add("ClientName");
            dt.Columns.Add("Cash");
            dt.Columns.Add("A_HairCut");
            dt.Columns.Add("LedgerBill");
       dt.Columns.Add("ExchangeMargin");
            dt.Columns.Add("NetRisk");
            while (strline != null)
            {

                
                 
                    _values = strline.Split(',');
                    strline = sr.ReadLine();
                    DataRow drow = dt.NewRow();
                drow["ClientCode"]=_values[3];
                drow["ClientName"]=_values[4];
                drow["Cash"]=_values[7];
                drow["B_HairCut"]=_values[9];
                drow["LedgerBill"]=_values[14];
              drow["ExchangeMargin"]=_values[23];
                drow["NetRisk"] = Convert.ToDecimal(_values[7]) + Convert.ToDecimal(_values[9]) + Convert.ToDecimal(_values[14])+Convert.ToDecimal(_values[23]);
                dt.Rows.Add(drow);


                 
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            if (GridView1.Rows.Count != 0)
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "Delete    from FNODetails";
                cmd.ExecuteNonQuery();
                conn.Close();



            foreach (GridViewRow gr in GridView1.Rows)
            {
                conn.Open();


                cmd = conn.CreateCommand();
                cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + gr.Cells[0].Text.Trim() + "' and branch!='RETAILKOLH'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    string family = dr[0].ToString();
                    string branch = dr[1].ToString();
                    string clientname = dr[2].ToString();
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + gr.Cells[0].Text.Trim() + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM)values('" + gr.Cells[0].Text.Trim() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "')";

                        cmd.ExecuteNonQuery();
                    }
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "insert into FNODetails(ClientCode,Cash,Non_Cash_A_HairCut,Leg_Bal_With_Bill,Total_Margin_Reporting,FNO_Total)values(@ClientCode,@Cash,@Non_Cash_A_HairCut,@Leg_Bal_With_Bill,@Total_Margin_Reporting,@FNO_Total)";
//                    FNOID
//ClientCode
//Cash
//Non_Cash_A_HairCut
//Leg_Bal_With_Bill
//Total_Margin_Reporting

                    cmd.Parameters.AddWithValue("ClientCode", gr.Cells[0].Text.Trim());
                    cmd.Parameters.AddWithValue("Cash", gr.Cells[2].Text.Trim());
                    cmd.Parameters.AddWithValue("Non_Cash_A_HairCut", gr.Cells[3].Text.Trim());
                    cmd.Parameters.AddWithValue("Leg_Bal_With_Bill", gr.Cells[4].Text.Trim());
                    cmd.Parameters.AddWithValue("Total_Margin_Reporting", gr.Cells[5].Text.Trim());
                    cmd.Parameters.AddWithValue("FNO_Total", gr.Cells[6].Text.Trim());
                    


                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            foreach (GridViewRow gr in GridView1.Rows)
            {
                conn.Open();


                cmd = conn.CreateCommand();
                cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + gr.Cells[0].Text.Trim() + "'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    string family = dr[0].ToString();
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + gr.Cells[0].Text.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,FNO)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr.Cells[0].Text.Trim() + "','" + family.Trim() + "'," + gr.Cells[6].Text.Trim() + ")";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "update INVESTMENTSUMMARY set FNO='" + gr.Cells[6].Text.Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr.Cells[0].Text.Trim() + "'";

                        cmd.ExecuteNonQuery();

                    }
                }
                dr.Close();
                conn.Close();

            }
            MessageBox.Show("Updation Done Successfully..!");
            conn.Open();


            cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('FNO.csv','" + DateTime.Today.ToString() + "')";

            cmd.ExecuteNonQuery();

            conn.Close();
        }
        else 
    {
    MessageBox.Show("First Upload the File..!");
    }
            GridView1.DataSource = null;
            GridView1.DataBind();
        
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
    }
}
