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
    public partial class Margin_Funding : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "select * from UploadLog where FileName='CASH.xls' and UploadDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {

                DataSet result = new DataSet();
                FileUpload1.SaveAs(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName));
                FileStream stream = File.Open(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName), FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                result = excelReader.AsDataSet();
                excelReader.Close();
                string a = "";
                int row_no = 0;
                // result.Tables[0].Columns.Add("SubBroker");
                result.Tables[0].Columns.Add("Net Risk");

                DataTable dtnew = new DataTable();
                for (int i = 0; i < result.Tables[0].Columns.Count - 1; i++)
                {
                    string colmname = result.Tables[0].Rows[2][i].ToString();
                    dtnew.Columns.Add(colmname);
                }
                //  
                dtnew.Columns.Add("Net Risk");
                dtnew.Columns.Add("Sub Broker");
                dr.Close();
                while (row_no < result.Tables[0].Rows.Count)
                {

                    decimal value = 0;
                    //cmd = conn.CreateCommand();
                    //cmd.CommandText = "Select sum(value)  from POA where ClientCode='" + result.Tables[0].Rows[row_no][1].ToString().Trim() + "'";
                    ////decimal value = (decimal)cmd.ExecuteScalar();
                    //dr = cmd.ExecuteReader();
                    //if (dr.HasRows)
                    //{
                    //    dr.Read();
                    //    string val = dr[0].ToString();
                    //    if (!string.IsNullOrEmpty(val))
                    //    {
                    //        value = Convert.ToDecimal(dr[0].ToString());
                    //    }
                    //    //   value = Convert.ToDecimal(dr[0].ToString());

                    //}
                   
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select branch from Cust_Client_Master where clientcode='" + result.Tables[0].Rows[row_no][0].ToString().Trim() + "' and branch!='RETAILKOLH' ";

                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        DataRow drow = dtnew.NewRow();

                        decimal netrisk = 0;
                        int last = 0;
                        for (int i = 0; i < result.Tables[0].Columns.Count - 1; i++)
                        {
                            // a += result.Tables[0].Rows[row_no][i].ToString() + ",";
                            if (i >= 2 && i <= 7)
                            {
                                try
                                {
                                    string number = result.Tables[0].Rows[row_no][i].ToString();

                                    if (!string.IsNullOrEmpty(number.Trim()))
                                    {
                                        if (i == 7)
                                        {
                                            netrisk = netrisk - Convert.ToDecimal(number);
                                        }
                                        else if (i == 2 || i == 3 || i == 4)
                                        {
                                            netrisk = netrisk + Convert.ToDecimal(number);
                                        }
                                        //if (i == 11)
                                        //{
                                        //    number = value.ToString();
                                        //}
                                        //if (i != 11)
                                        //{
                                        //    netrisk = netrisk + (Convert.ToDecimal(number) * 100000);
                                        //}
                                        //else
                                        //{

                                        //}
                                    }
                                }
                                catch (Exception ex)
                                { 
                                
                                }
                            }//if (i == 11)
                            //{
                            //    drow[i] = value.ToString();
                            //}
                            //else
                            if (i >= 2)
                            {
                                drow[i] = result.Tables[0].Rows[row_no][i].ToString();
                                // drow[i] = (Convert.ToDecimal(result.Tables[0].Rows[row_no][i].ToString()) * 100000);// result.Tables[0].Rows[row_no][i].ToString();
                            }
                            else
                            {
                                drow[i] = result.Tables[0].Rows[row_no][i].ToString();
                            }
                        }
                        //if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][4].ToString().Trim()))
                        //{
                        //    netrisk = netrisk - (Convert.ToDecimal(result.Tables[0].Rows[row_no][4].ToString()) * 100000);
                        //    //  netrisk = netrisk * 100000;
                        //}
                        result.Tables[0].Rows[row_no][result.Tables[0].Columns.Count - 1] = netrisk;
                        drow[result.Tables[0].Columns.Count - 1] = netrisk;

                        drow[dtnew.Columns.Count - 1] = dr["branch"].ToString();
                        //netrisk = 0;
                        dtnew.Rows.Add(drow);
                    }
                    dr.Close();


                    row_no++;

                    // -(2 * Convert.ToDecimal(result.Tables[0].Rows[row_no][4].ToString()));
                    //    a += "\n";
                }

                GridView1.DataSource = dtnew;//result.Tables[0];

                GridView1.DataBind();
            }
            else
            {
                MessageBox.Show("First Upload Cash Net Risk File...!");
            }




            dr.Close();
            conn.Close();




        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows.Count != 0)
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "Delete    from MarginFundingDetails";
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
                        cmd.CommandText = "insert into MarginFundingDetails(ClientCode,UnApprovedMktValue,ApprovedMktValue,Odd_LotMktValue,LedgerBal,NetRisk)values(@ClientCode,@UnApprovedMktValue,@ApprovedMktValue,@Odd_LotMktValue,@LedgerBal,@NetRisk)";

                        cmd.Parameters.AddWithValue("ClientCode", gr.Cells[0].Text.Trim());
                        cmd.Parameters.AddWithValue("UnApprovedMktValue", gr.Cells[3].Text.Trim());
                        cmd.Parameters.AddWithValue("ApprovedMktValue", gr.Cells[2].Text.Trim());
                        cmd.Parameters.AddWithValue("Odd_LotMktValue", gr.Cells[4].Text.Trim());
                        cmd.Parameters.AddWithValue("LedgerBal", gr.Cells[7].Text.Trim());
                        cmd.Parameters.AddWithValue("NetRisk", gr.Cells[19].Text.Trim());
                       


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
                            cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,CASH)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr.Cells[0].Text.Trim() + "','" + family.Trim() + "'," + gr.Cells[19].Text.Trim() + ")";

                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "update INVESTMENTSUMMARY set CASH='" + gr.Cells[19].Text.Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr.Cells[0].Text.Trim() + "'";

                            cmd.ExecuteNonQuery();

                        }
                    }
                    dr.Close();
                    conn.Close();

                }
                MessageBox.Show("Updation Done Successfully..!");
                conn.Open();


                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('MarginFunding.xls','" + DateTime.Today.ToString() + "')";

                cmd.ExecuteNonQuery();

                conn.Close();



                GridView1.DataSource = null;
                GridView1.DataBind();
            }
            else
            {
                MessageBox.Show("First Upload the File..!");
            }
        }
    }
}
