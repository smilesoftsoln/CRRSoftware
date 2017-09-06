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
using System.Collections.Generic;
namespace InvestmentSummary
{
    public partial class MFNetRisk : System.Web.UI.Page
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
            if (File.Exists("/" + FileUpload1.FileName))
            {
                File.Delete("/" + FileUpload1.FileName);

            }
            FileUpload1.SaveAs("/" + FileUpload1.FileName);

            string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                         "Data Source=" + "/" + FileUpload1.FileName + ";" + "Extended Properties=Excel 8.0;";


            // create your excel connection object using the connection string
            OleDbConnection objXConn = new OleDbConnection(xConnStr);
            //objXConn.Open();

            // use a SQL Select command to retrieve the data from the Excel Spreadsheet
            // the "table name" is the name of the worksheet within the spreadsheet
            // in this case, the worksheet name is "Members" and is coded as: [Members$]
            OleDbCommand objCommand = new OleDbCommand("SELECT * FROM [AUM wise data$]", objXConn);
            //StreamReader streamread = new StreamReader(Server.MapPath("/temp/" + Session["login"].ToString()) + "/" + MasterFileUpload1.FileName);

            OleDbDataAdapter objDataAdapter = new OleDbDataAdapter();

            // retrieve the Select command for the Spreadsheet
            objDataAdapter.SelectCommand = objCommand;

            // Create a DataSet
            DataSet objDataSet = new DataSet();
            // Populate the DataSet with the spreadsheet worksheet data
            objDataAdapter.Fill(objDataSet);
            DataColumn dcol = new DataColumn("ClientCodeFromEquityDB", typeof(System.String));

            objDataSet.Tables[0].Columns.Add(dcol);
            dcol = new DataColumn("Branch", typeof(System.String));
            objDataSet.Tables[0].Columns.Add(dcol);
            dcol = new DataColumn("ClientCodeFromMFDB", typeof(System.String));
            objDataSet.Tables[0].Columns.Add(dcol);
            dcol = new DataColumn("ClientAliasFromMFDB", typeof(System.String));
            objDataSet.Tables[0].Columns.Add(dcol);
            // Bind the data to the GridView
            GridView1.DataSource = objDataSet.Tables[0].DefaultView;
            GridView1.DataBind();
            //BoundField colmn = new BoundField();
            //colmn.HeaderText = "xyz";
            //GridView1.Columns.Add(colmn);
            foreach (GridViewRow gr in GridView1.Rows)
            {
                string pan = gr.Cells[10].Text.Trim();
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select clientcode,subbrokercode  from Cust_Client_Master   where panno='" + pan + "' ";//and branch!='RETAILKOLH'
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    gr.Cells[16].Text = dr[0].ToString();
                    gr.Cells[17].Text = dr[1].ToString();
                }
                else
                {
                    // gr.Visible = false;
                }
                dr.Close();
                conn.Close();

            }
            foreach (GridViewRow gr in GridView1.Rows)
            {
                string name = gr.Cells[0].Text.Trim();
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select equitycode1,clientalias   from MF_Client_Master   where clientname='" + name + "' ";//and branch!='RETAILKOLH'
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    gr.Cells[18].Text = dr[0].ToString();
                    gr.Cells[19].Text = dr[1].ToString();
                }
                else
                {
                    // gr.Visible = false;
                }
                dr.Close();
                conn.Close();

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = " delete from MFDetails";//(ClientCode ,Value)values('" + toconsider + "'," + total + ")";
            cmd.ExecuteNonQuery();

            conn.Close();
            foreach (GridViewRow gr in GridView1.Rows)
            {
                // string name = gr.Cells[0].Text.Trim();
                string equitycode = gr.Cells[16].Text.Replace("&nbsp;","").Trim();
                string mfequitycode = gr.Cells[18].Text.Replace("&nbsp;", "").Trim();
                string clientaliascode = gr.Cells[19].Text.Replace("&nbsp;", "").Trim();
                string total = gr.Cells[14].Text.Replace("&nbsp;", "0").Trim();
                string name = gr.Cells[0].Text.Replace("&nbsp;", "0").Trim();
                string toconsider = "";

                if (!(string.IsNullOrEmpty(equitycode.Trim()) && string.IsNullOrEmpty(mfequitycode.Trim())))
                {



                    if (equitycode.Equals(mfequitycode))
                    {
                        toconsider = equitycode;
                    }
                    else if (string.IsNullOrEmpty(equitycode.Trim()) && (!string.IsNullOrEmpty(mfequitycode.Trim())))
                    {
                        toconsider = mfequitycode;
                        conn.Open();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select * from Cust_Client_Master where clientcode='" + mfequitycode + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            toconsider = clientaliascode;
                        }
                        dr.Close();
                        conn.Close();

                    }
                    else if (string.IsNullOrEmpty(mfequitycode.Trim()) && (!string.IsNullOrEmpty(equitycode.Trim())))
                    {
                        toconsider = equitycode;


                    }
                    else if ((!string.IsNullOrEmpty(mfequitycode.Trim())) && (!string.IsNullOrEmpty(equitycode.Trim())))
                    {
                        toconsider = equitycode;


                    }





                }
                else if (string.IsNullOrEmpty(equitycode.Trim()) && string.IsNullOrEmpty(mfequitycode.Trim()))
                {
                    toconsider = clientaliascode;
                }

                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "insert into  MFDetails(ClientCode ,Value,ClientName)values('" + toconsider + "'," + total + ",'" + name + "')";
                cmd.ExecuteNonQuery();

                conn.Close();



            }
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = " select ClientCode,sum(Value) from MFDetails group by ClientCode";//(ClientCode ,Value)values('" + toconsider + "'," + total + ")";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
            }
            foreach (DataRow drw in dt.Rows)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "Select family,clientcode from Cust_Client_Master where clientcode='" + drw[0].ToString() + "' and branch!='RETAILKOLH'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    string family = dr[0].ToString();
                    string clientcode = dr[1].ToString();
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,MF)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + drw[1].ToString() + ")";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "update INVESTMENTSUMMARY set MF='" + drw[1].ToString() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                        cmd.ExecuteNonQuery();

                    }
                }
                else
                {
                    //will be for pure mf
                    dr.Close();
                   // conn.Open();
                    //*******************//
                    if(!string.IsNullOrEmpty(drw[0].ToString().Trim()))
                    {
                    string family ="";//= dr[0].ToString();
                    string branch = "";//= dr[1].ToString();
                    string clientname = "";// = dr[2].ToString();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select  groupalias,branch,clientname from MF_Client_Master where clientalias='" + drw[0].ToString() + "' ";
                    
                    //cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + gr.Cells[1].Text.Trim() + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                         family = dr[0].ToString();
                         branch = dr[1].ToString();
                         clientname = dr[2].ToString();
                        dr.Close();

                        if(!string.IsNullOrEmpty(branch.Trim()))
                        { cmd = conn.CreateCommand();
                        cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + drw[0].ToString() + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM)values('" + drw[0].ToString() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "')";

                            cmd.ExecuteNonQuery();
                        }
                       
                    }
                        dr.Close();
                    }

                   // conn.Close();
                    //*********************************/
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select  FamilyCode from ClientMaster where ClientCode='" + drw[0].ToString() + "' and branch!='RETAILKOLH'";
                    dr = cmd.ExecuteReader();
                    string clientcode = drw[0].ToString();
               //   family = drw[0].ToString();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        family = dr[0].ToString();
                    }
                  
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,MF)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + drw[1].ToString() + ")";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "update INVESTMENTSUMMARY set MF='" + drw[1].ToString() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                        cmd.ExecuteNonQuery();

                    }
                }
                }
                dr.Close();
 
            }
            MessageBox.Show("updation done successfully");
            //conn.Open();


            cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('MF.xls','" + DateTime.Today.ToString() + "')";

            cmd.ExecuteNonQuery();

            conn.Close();
            GridView1.DataSource = null;
            GridView1.DataBind();
           // conn.Close();

        }
    }
}
