using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.IO;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using System.Data.OleDb;

namespace InvestmentSummary
{
    public partial class RM_Mapping : System.Web.UI.Page
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
            if (!Directory.Exists(Server.MapPath("/" + Session["login"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("/" + Session["login"].ToString()));
            }
            if (FileUpload1.HasFile)
            {
                try
                {
                    FileUpload1.SaveAs(Server.MapPath("/" + Session["login"].ToString()) + "/RM.xls");


                    string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                      "Data Source=" + Server.MapPath("/" + Session["login"].ToString() + "/" + "RM.xls") + ";" + "Extended Properties=Excel 8.0;";


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
                    Session["dt"] = objDataSet.Tables[0];

                    // Bind the data to the GridView
                    GridView1.DataSource = objDataSet.Tables[0].DefaultView;
                    GridView1.DataBind();
                    //if (GridView1.HeaderRow.Cells[0].Text == "NAME" && GridView1.HeaderRow.Cells[1].Text == "CLIENT_ALIAS" && GridView1.HeaderRow.Cells[2].Text == "MOBILE" && GridView1.HeaderRow.Cells[3].Text == "PHONE" && GridView1.HeaderRow.Cells[4].Text == "EMAIL1" && GridView1.HeaderRow.Cells[5].Text == "EMAIL2" && GridView1.HeaderRow.Cells[6].Text == "ADDRESS1" && GridView1.HeaderRow.Cells[7].Text == "ADDRESS2" && GridView1.HeaderRow.Cells[8].Text == "ADDRESS3" && GridView1.HeaderRow.Cells[9].Text == "CITY" && GridView1.HeaderRow.Cells[10].Text == "PAN" && GridView1.HeaderRow.Cells[11].Text == "DOB" && GridView1.HeaderRow.Cells[12].Text == "GROUPNAME" && GridView1.HeaderRow.Cells[13].Text == "GROUP_ALIAS" && GridView1.HeaderRow.Cells[14].Text == "SUBBROK" && GridView1.HeaderRow.Cells[15].Text == "RM" && GridView1.HeaderRow.Cells[16].Text == "FILENO" && GridView1.HeaderRow.Cells[17].Text == "LOCK" && GridView1.HeaderRow.Cells[18].Text == "EQUITY" && GridView1.HeaderRow.Cells[19].Text == "DEBT" && GridView1.HeaderRow.Cells[20].Text == "EQUITYCODE1" && GridView1.HeaderRow.Cells[21].Text == "EQUITYCODE2")
                    //{
                    //    Label7.Text = "";
                    //    //checkduplicates();
                    //}
                    //else
                    //{
                    //    GridView1.DataSource = null;
                    //    GridView1.DataBind();
                    //    MessageBox.Show(" Invalid Column Name/Sequence..!  ");
                    //    Label7.Text = "Column Sequence: 1.NAME	2.CLIENT_ALIAS	3.MOBILE	4.PHONE	5.EMAIL1	6.EMAIL2	7.ADDRESS1	8.ADDRESS2	9.ADDRESS3	10.CITY	11.PAN	12.DOB	13.GROUPNAME	14.GROUP_ALIAS	15.SUBBROK	16.RM	17.FILENO	18.LOCK	19.EQUITY	20.DEBT	21.EQUITYCODE1	22.EQUITYCODE2";

                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            foreach (GridViewRow gr in GridView1.Rows)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "update ClientMaster set RM='"+gr.Cells[5].Text.Trim()+"' where  FamilyCode='"+gr.Cells[3].Text.Trim()+"'";
                cmd.ExecuteNonQuery();
            
            
            }
            MessageBox.Show("Update Done..!");
            conn.Close();
        }
       
    }
}
