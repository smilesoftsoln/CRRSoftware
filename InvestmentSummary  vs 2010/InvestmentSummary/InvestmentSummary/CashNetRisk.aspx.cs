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
    public partial class CashNetRisk : System.Web.UI.Page
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
            int k = 0;
            int j = 0;
            conn.Open();
            cmd = conn.CreateCommand();

            cmd.CommandText = "select * from POA where type='DP900' and uploadDate='" + DateTime.Today.ToString() + "' ";
            // cmd.Parameters.AddWithValue("Value", Convert.ToDecimal(gr.Cells[2].Text));
            //cmd.Parameters.AddWithValue("ClientCode", gr.Cells[0].Text);
            //cmd.Parameters.AddWithValue("DematCode", gr.Cells[3].Text);

            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                k = 1;
            }
            dr.Close();
            cmd = conn.CreateCommand();

            cmd.CommandText = "select * from POA where type='DP919' and uploadDate='" + DateTime.Today.ToString() + "' ";
            // cmd.Parameters.AddWithValue("Value", Convert.ToDecimal(gr.Cells[2].Text));
            //cmd.Parameters.AddWithValue("ClientCode", gr.Cells[0].Text);
            //cmd.Parameters.AddWithValue("DematCode", gr.Cells[3].Text);

            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                j= 1;
            }
            dr.Close();

            if (k == 1 && j == 1)
            {
                DataSet result = new DataSet();
                FileUpload1.SaveAs(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName));
                FileStream stream = File.Open(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName), FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                result = excelReader.AsDataSet();
                excelReader.Close();
                string a = "";
                int row_no = 3;
                // result.Tables[0].Columns.Add("SubBroker");
                result.Tables[0].Columns.Add("Net Risk");

                DataTable dtnew = new DataTable();
                for (int i = 0; i < result.Tables[0].Columns.Count - 1; i++)
                {
                    string colmname = result.Tables[0].Rows[row_no - 2][i].ToString();
                    dtnew.Columns.Add(colmname);
                }
                //  
                dtnew.Columns.Add("Net Risk");
                dtnew.Columns.Add("Sub Broker");
            //   conn.Open();   // 
                while (row_no < result.Tables[0].Rows.Count)
                {
                  
                    decimal value = 0;
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select sum(value)  from POA where ClientCode='" + result.Tables[0].Rows[row_no][1].ToString().Trim() + "' and uploadDate='" + DateTime.Today.ToString() + "'  ";
                    //decimal value = (decimal)cmd.ExecuteScalar();
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string val = dr[0].ToString();
                        if (!string.IsNullOrEmpty(val))
                        {
                            value = Convert.ToDecimal(dr[0].ToString());
                        }
                        //   value = Convert.ToDecimal(dr[0].ToString());

                    }
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select branch from Cust_Client_Master where clientcode='" + result.Tables[0].Rows[row_no][1].ToString().Trim() + "' and branch!='RETAILKOLH' ";

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
                            if (i >= 3)
                            {
                                string number = result.Tables[0].Rows[row_no][i].ToString();

                                if (!string.IsNullOrEmpty(number.Trim()))
                                {
                                    if (i == 9)
                                    {
                                        number = "-" + number;
                                    }
                                    if (i == 11)
                                    {
                                        number = value.ToString();
                                    }
                                    if (i != 11)
                                    {
                                        netrisk = netrisk + (Convert.ToDecimal(number) * 100000);
                                    }
                                    else
                                    {
                                        netrisk = netrisk + Convert.ToDecimal(number);
                                    }
                                }
                            }
                            if (i == 11)
                            {
                                drow[i] = value.ToString();
                            }
                            else if (i >= 3)
                            {
                                drow[i] = (Convert.ToDecimal(result.Tables[0].Rows[row_no][i].ToString()) * 100000);// result.Tables[0].Rows[row_no][i].ToString();
                            }
                            else
                            {
                                drow[i] = result.Tables[0].Rows[row_no][i].ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(result.Tables[0].Rows[row_no][4].ToString().Trim()))
                        {
                            netrisk = netrisk - (Convert.ToDecimal(result.Tables[0].Rows[row_no][4].ToString()) * 100000);
                            //  netrisk = netrisk * 100000;
                        }
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
                //string output = System.IO.Path.Combine(Server.MapPath("Data"), "ccbackup.csv");
                //StreamWriter csv = new StreamWriter(@output, false);
                //csv.Write(a);
                //csv.Close();
                //  StreamReader sr = new StreamReader(System.IO.Path.Combine(Server.MapPath("Data"), "ccbackup.csv"));
            }
            else
            {
                if (k == 0)
                {
                    MessageBox.Show("Please Import DP900 File");
                }
                if (j == 0)
                {
                    MessageBox.Show("Please Import DP919 File");
                }
            }
       conn.Close();  }

        protected void Button2_Click(object sender, EventArgs e)
        {

            if (GridView1.Rows.Count != 0)
            {
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "Delete    from EqutyDetails";
                cmd.ExecuteNonQuery();
                conn.Close();
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    conn.Open();


                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + gr.Cells[1].Text.Trim() + "' and branch!='RETAILKOLH'";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        string family = dr[0].ToString();
                        string branch = dr[1].ToString();
                        string clientname = dr[2].ToString();
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + gr.Cells[1].Text.Trim() + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM)values('" + gr.Cells[1].Text.Trim() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "')";

                            cmd.ExecuteNonQuery();
                        }
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "insert into EqutyDetails(ClientCode,LegBal,CashColl,NonCashColl,DebitStock,POToday,ShrtValue,FutPOValue,POAValue,Total)values(@ClientCode,@LegBal,@CashColl,@NonCashColl,@DebitStock,@POToday,@ShrtValue,@FutPOValue,@POAValue,@Total)";

                        cmd.Parameters.AddWithValue("ClientCode", gr.Cells[1].Text.Trim());
                        cmd.Parameters.AddWithValue("LegBal", gr.Cells[3].Text.Trim());
                        cmd.Parameters.AddWithValue("CashColl", gr.Cells[5].Text.Trim());
                        cmd.Parameters.AddWithValue("NonCashColl", gr.Cells[6].Text.Trim());
                        cmd.Parameters.AddWithValue("DebitStock", gr.Cells[7].Text.Trim());
                        cmd.Parameters.AddWithValue("POToday", gr.Cells[8].Text.Trim());
                        cmd.Parameters.AddWithValue("ShrtValue", gr.Cells[9].Text.Trim());
                        cmd.Parameters.AddWithValue("FutPOValue", gr.Cells[10].Text.Trim());
                        cmd.Parameters.AddWithValue("POAValue", gr.Cells[11].Text.Trim());
                        cmd.Parameters.AddWithValue("Total", gr.Cells[12].Text.Trim());


                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }

              
                foreach (GridViewRow gr in GridView1.Rows)
                {
                    conn.Open();


                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + gr.Cells[1].Text.Trim() + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        string family = dr[0].ToString();
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + gr.Cells[1].Text.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,CASH)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr.Cells[1].Text.Trim() + "','" + family.Trim() + "'," + gr.Cells[12].Text.Trim() + ")";

                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "update INVESTMENTSUMMARY set CASH='" + gr.Cells[12].Text.Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr.Cells[1].Text.Trim() + "'";

                            cmd.ExecuteNonQuery();

                        }

                    }
                    dr.Close();
//                    EquityID
//ClientCode 1
//LegBal 3
//CashColl 5
//NonCashColl 6
//DebitStock 7 
//POToday 8 
//ShrtValue 9
//FutPOValue 10
//POAValue 11
                    
                   

                    conn.Close();
                }
                    conn.Open();
                    /******************
                     * For records present in DP900 and DP919 file but not in Equity Net Risk file 
                     * 
                     * 
                     * 
                     * 
                     * 
                     * ***/
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select p.ClientCode from POA p   WHERE NOT EXISTS (SELECT CM.ClientCode  FROM ClientMaster CM  WHERE CM.ClientCode = p.ClientCode) and p.uploadDate='"+DateTime.Today.ToString()+"'";
                    dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    if (dr.HasRows)
                    {

                        //while (dr.Read())
                        //{
                        //    string clientcode = dr[0].ToString();
                        
                        //}

                        dt.Load(dr);
                    
                    }
                    dr.Close();

                    foreach (DataRow dtr in dt.Rows)
                    {

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + dtr[0].ToString() + "' and branch!='RETAILKOLH'";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();

                            string family = dr[0].ToString();
                            string branch = dr[1].ToString();
                            string clientname = dr[2].ToString();
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + dtr[0].ToString() + "'";
                            dr = cmd.ExecuteReader();
                            if (!dr.HasRows)
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM)values('" + dtr[0].ToString() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "')";

                                cmd.ExecuteNonQuery();
                            }
                            
                        }dr.Close();
                    }


                    conn.Close();
                
                    foreach (DataRow dtr in dt.Rows)
                    {

                        conn.Open();
                        if( !dtr[0].ToString().Equals("&nbsp;"))
                        {
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select sum(value)  from POA where ClientCode='" + dtr[0].ToString() + "' and uploadDate='" + DateTime.Today.ToString() + "'  ";
                        decimal cash = (decimal)cmd.ExecuteScalar();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + dtr[0].ToString() + "'";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            string family = dr[0].ToString();
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + dtr[0].ToString() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                            dr = cmd.ExecuteReader();
                            if (!dr.HasRows)
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,CASH)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + dtr[0].ToString() + "','" + family.Trim() + "'," + cash + ")";

                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "update INVESTMENTSUMMARY set CASH='" + cash + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + dtr[0].ToString() + "'";

                                cmd.ExecuteNonQuery();

                            }
                        }
                        
                        }
                        dr.Close();
                        conn.Close();
                    }
                   
                MessageBox.Show("Updation Done Successfully..!");
                conn.Open();


                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('CASH.xls','" + DateTime.Today.ToString() + "')";

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
