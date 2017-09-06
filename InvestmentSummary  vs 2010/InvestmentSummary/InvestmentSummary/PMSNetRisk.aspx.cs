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
    public partial class PMSNetRisk : System.Web.UI.Page
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


            if (File.Exists("/"+FileUpload1.FileName))
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
            OleDbCommand objCommand = new OleDbCommand("SELECT * FROM [Sheet1$]", objXConn);
            //StreamReader streamread = new StreamReader(Server.MapPath("/temp/" + Session["login"].ToString()) + "/" + MasterFileUpload1.FileName);

            OleDbDataAdapter objDataAdapter = new OleDbDataAdapter();

            // retrieve the Select command for the Spreadsheet
            objDataAdapter.SelectCommand = objCommand;

            // Create a DataSet
            DataSet objDataSet = new DataSet();
            // Populate the DataSet with the spreadsheet worksheet data
            objDataAdapter.Fill(objDataSet);
            DataColumn dcol = new DataColumn("ClientCode", typeof(System.String));

            objDataSet.Tables[0].Columns.Add(dcol);
                dcol = new DataColumn("Branch", typeof(System.String));
            objDataSet.Tables[0].Columns.Add(dcol);
            // Bind the data to the GridView
            GridView1.DataSource = objDataSet.Tables[0].DefaultView;
            GridView1.DataBind();
            //BoundField colmn = new BoundField();
            //colmn.HeaderText = "xyz";
            //GridView1.Columns.Add(colmn);
            foreach (GridViewRow gr in GridView1.Rows)
            {
                string pan = gr.Cells[7].Text.Trim();
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = "select clientcode,subbrokercode  from Cust_Client_Master   where panno='" + pan + "' ";//and branch!='RETAILKOLH'
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    gr.Cells[9].Text = dr[0].ToString();
                    gr.Cells[10].Text = dr[1].ToString();
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
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            conn.Open();
        foreach(GridViewRow gr in GridView1.Rows)
        {
            cmd = conn.CreateCommand();
            cmd.CommandText = "select PANNO from PMSDetails where PANNO='" + gr.Cells[7].Text.Trim() + "' and Scheme='" + gr.Cells[3].Text.Trim() + "' and PMSDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
            dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT   INTO PMSDetails(PMSDate,PANNO,Scheme,Valuation)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr.Cells[7].Text.Trim() + "','" + gr.Cells[3].Text.Trim() + "'," + gr.Cells[8].Text.Replace("&nbsp;","0").Trim() + ")";

                cmd.ExecuteNonQuery();
            }
            else
            {
                dr.Close();
                cmd = conn.CreateCommand();
                cmd.CommandText = "update PMSDetails set Valuation=" + gr.Cells[8].Text.Replace("&nbsp;","0").Trim() + " where  PMSDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and PANNO='" + gr.Cells[7].Text.Trim() + "' and Scheme='" + gr.Cells[3].Text.Trim() + "'";

                cmd.ExecuteNonQuery();

            }
            // See whether Dictionary contains this string.
            if (dictionary.ContainsKey(gr.Cells[7].Text.Trim()))
            {
                decimal value =Convert.ToDecimal( dictionary[gr.Cells[7].Text.Trim()]);
                value = value + Convert.ToDecimal(gr.Cells[8].Text.Replace("&nbsp;", "0").Trim());

                dictionary[gr.Cells[7].Text.Trim()] = value; 
            }

            // See whether Dictionary contains this string.
            if (!dictionary.ContainsKey(gr.Cells[7].Text.Trim()))
            {
                dictionary.Add(gr.Cells[7].Text.Trim(), Convert.ToDecimal(gr.Cells[8].Text.Replace("&nbsp;","0").Trim()));
            }

        }
        

        foreach (KeyValuePair<string, decimal> item in dictionary)
        {
          
                cmd = conn.CreateCommand();
                cmd.CommandText = "Select family,clientcode,branch,clientname from Cust_Client_Master where panno='" + item.Key + "' and branch!='RETAILKOLH'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();

                    string family = dr[0].ToString();
                    string clientcode = dr[1].ToString();
                    string branch = dr[2].ToString();
                    string clientname = dr[3].ToString();
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select ClientCode,FamilyCode from ClientMaster where ClientCode='" + clientcode + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM)values('" + clientcode + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "')";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dr.Read();
                        family = dr[1].ToString();
                        dr.Close();
                    }
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                    dr = cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,PMS)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + item.Value + ")";

                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "update INVESTMENTSUMMARY set PMS='" + item.Value + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                        cmd.ExecuteNonQuery();

                    }
                }
                else
                {
                    dr.Close();
                    cmd = conn.CreateCommand();
                    string subbroker = "";
                    string family = "";// = dr[0].ToString();
                    string clientcode = "";// = dr[0].ToString();
                    string clientname = "";
                    cmd.CommandText = "Select PMSCODE,SUBBROKER,NAME from PMSMASTER where PAN='" + item.Key + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        family = dr[0].ToString();
                        clientcode = dr[0].ToString();
                        subbroker = dr[1].ToString();
                        clientname = dr[2].ToString();
                    }
                    dr.Close();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select BranchName from SBCODE where Subbroker='" + subbroker + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string branch = dr[0].ToString();
                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select ClientCode,FamilyCode from ClientMaster where ClientCode='" + clientcode + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM)values('" + clientcode + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "')";

                            cmd.ExecuteNonQuery();
                        }

                        dr.Close();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,PMS)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + item.Value + ")";

                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "update INVESTMENTSUMMARY set PMS='" + item.Value + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                            cmd.ExecuteNonQuery();

                        }
                    }
                }
                dr.Close();
 
        }

        conn.Close();
        MessageBox.Show("Updation Done Successfully..!");
        conn.Open();


        cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('PMS.xls','" + DateTime.Today.ToString() + "')";

        cmd.ExecuteNonQuery();

        conn.Close();
        GridView1.DataSource = null;
        GridView1.DataBind();
        }

//        protected void Button2_Click(object sender, EventArgs e)
//        {
//            foreach (GridViewRow gr in GridView1.Rows)
//            {
//                conn.Open();

//                if (gr.Visible == true)
//                {
//                    cmd = conn.CreateCommand();
//                    cmd.CommandText = "Select family from Cust_Client_Master where clientcode='" + gr.Cells[6].Text.Trim() + "'";
//                    dr = cmd.ExecuteReader();
//                    if (dr.HasRows)
//                    {
//                        dr.Read();

//                        string family = dr[0].ToString();
//                        dr.Close();
//                        cmd = conn.CreateCommand();
//                        cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + gr.Cells[6].Text.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
//                        dr = cmd.ExecuteReader();
//                        if (!dr.HasRows)
//                        {
//                            dr.Close();
//                            cmd = conn.CreateCommand();
//                            cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,PMS)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr.Cells[6].Text.Trim() + "','" + family.Trim() + "'," + gr.Cells[5].Text.Trim() + ")";

//                            cmd.ExecuteNonQuery();
//                        }
//                        else
//                        {
//                            dr.Close();
//                            cmd = conn.CreateCommand();
//                            cmd.CommandText = "update INVESTMENTSUMMARY set PMS='" + gr.Cells[5].Text.Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr.Cells[6].Text.Trim() + "'";

//                            cmd.ExecuteNonQuery();

//                        }
//                    }
//                    dr.Close();
                 
//                }
//conn.Close();
//            }   

//        }
    }
}
