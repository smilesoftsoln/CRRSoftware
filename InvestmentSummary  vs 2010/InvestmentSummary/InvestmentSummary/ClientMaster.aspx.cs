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
using ExcelApp = Microsoft.Office.Interop.Excel;
namespace InvestmentSummary
{
    public partial class ClientMaster : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                GridView1.DataSource = null;
                GridView1.DataBind();
            //}
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gr = GridView1.SelectedRow;
            lbbCliencodeLabel2.Text = gr.Cells[2].Text;
            lblnameLabel2.Text = gr.Cells[3].Text;
            FamilyCodeTextBox1txt.Text = gr.Cells[4].Text;
            Branch2DropDownList2.Text = gr.Cells[5].Text;
            RMDropDownList3.DataBind();
            RMDropDownList3.Text = gr.Cells[6].Text;

        }

        protected void Button5_Click(object sender, EventArgs e)
        {

            if (Branch2DropDownList2.SelectedIndex != 0)
            {
                if (RMDropDownList3.SelectedIndex != 0)
                {
                    if (!string.IsNullOrEmpty(FamilyCodeTextBox1txt.Text.Trim()))
                    {
                        conn.Open();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select * from ClientMaster  where ClientCode='" + FamilyCodeTextBox1txt.Text.ToUpper().Trim() + "'";
                        dr = cmd.ExecuteReader();
                        if (!dr.HasRows)
                        {
                            MessageBox.Show("Invalid Family Code..!");
                        }
                        else
                        {
                            dr.Close();

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "update ClientMaster set  FamilyCode=@FamilyCode,Branch=@Branch,RM=@RM where ClientCode='" + lbbCliencodeLabel2.Text.ToUpper().Trim() + "'";
                            cmd.Parameters.AddWithValue("FamilyCode", FamilyCodeTextBox1txt.Text.ToUpper().Trim());
                            cmd.Parameters.AddWithValue("Branch", Branch2DropDownList2.Text.Trim());
                            cmd.Parameters.AddWithValue("RM", RMDropDownList3.Text.Trim());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Update Successfull..!");
                            GridView1.DataBind();

                        }
                        dr.Close();
                        conn.Close();

                    }
                    else
                    {
                        MessageBox.Show("Enter Family Code");
                    }
                }
                else
                {
                    MessageBox.Show("Select RM...!");
                }

            }
            else
            {
                MessageBox.Show("Select Branch...!");
            }
        }

        protected void Branch2DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BranchDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM [ClientMaster] WHERE ([Branch] = @Branch) order by AddedDate desc";
            cmd.Parameters.AddWithValue("@Branch", BranchDropDownList1.SelectedValue.Trim());
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dt = new DataTable();
                
                dt.Load(dr);

                if (System.IO.File.Exists(Server.MapPath("/") + "Book3.xls")) //It checks if file exists then it delete that file.
                {
                    System.IO.File.Delete(Server.MapPath("/") + "Book3.xls");
                }
                Export(dt, Server.MapPath("/") + "Book3.xls");
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + "Book3.xls");
                Response.ContentType = "application/excel";
                Response.WriteFile(Server.MapPath("/") + "Book3.xls");
            }
            dr.Close();
            conn.Close();
        }

        public static void Export(DataTable dt, string filepath)
        {

            String strFileName = "";
            strFileName = filepath;

            // Server File Path Where you want to save excel file.

            ExcelApp.Application myExcel = new ExcelApp.Application();
            //Create a New file
            ExcelApp._Workbook mybook = myExcel.Workbooks.Add(System.Reflection.Missing.Value);
            //Open the exist file
            //ExcelApp._Workbook mybook = myExcel.Workbooks.Open(filepath,
            //          Type.Missing, Type.Missing, Type.Missing,
            //    Type.Missing,Type.Missing, Type.Missing, Type.Missing,
            //    Type.Missing, Type.Missing, Type.Missing,
            //    Type.Missing, Type.Missing,Type.Missing, Type.Missing);
            //ExcelApp._Workbook mybook = myExcel.Workbooks.Open(Filename: filepath);
            myExcel.Visible = false;
            try
            {
                mybook.Activate();
                ExcelApp._Worksheet mysheet = (ExcelApp._Worksheet)mybook.ActiveSheet;
                int colIndex = 0;///********////
                int rowIndex = 0;
                //foreach (DataColumn dcol in dt.Columns)
                //{
                //    colIndex = colIndex + 1;
                //    myExcel.Cells[1, colIndex] = dcol.ColumnName;

                //}
                foreach (DataColumn dcol in dt.Columns)
                {
                    colIndex = colIndex + 1;
                    myExcel.Cells[rowIndex + 1, colIndex] = dcol.ColumnName;
                    //mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).WrapText = true;

                    //mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Font.Bold = true;
                    //mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Font.Size = 10;
                    //mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    //ExcelApp.Borders borders = mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Borders;
                    //borders[ExcelApp.XlBordersIndex.xlEdgeLeft].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    //borders[ExcelApp.XlBordersIndex.xlEdgeTop].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    //borders[ExcelApp.XlBordersIndex.xlEdgeBottom].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    //borders[ExcelApp.XlBordersIndex.xlEdgeRight].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    //borders.Color = 0;
                    //borders[ExcelApp.XlBordersIndex.xlInsideVertical].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    //borders[ExcelApp.XlBordersIndex.xlInsideHorizontal].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    //borders[ExcelApp.XlBordersIndex.xlDiagonalUp].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    //borders[ExcelApp.XlBordersIndex.xlDiagonalDown].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    //borders = null;
                    ////  mysheet.Columns.WrapText = mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]);
                    //mysheet.get_Range(myExcel.Cells[1, colIndex], myExcel.Cells[1, colIndex]).

                }
                ExcelApp.Style style1 = myExcel.ActiveWorkbook.Styles.Add("Content", Type.Missing);
                //style1.Borders.Color = Color.Black;
                style1.Font.Name = "Verdana";
                // style1.WrapText = true;
                style1.Font.Size = 10;

                style1.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                style1.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);

                foreach (DataRow drow in dt.Rows)
                {
                    rowIndex = rowIndex + 1;
                    colIndex = 0;
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        myExcel.Cells[rowIndex + 1, colIndex] = drow[dcol.ColumnName];
                    }
                     
                }
                mysheet.Columns.AutoFit();






                //For Saving excel file on Server
                mybook.SaveCopyAs(strFileName);

            }
            catch (Exception wzx)
            {
                MessageBox.Show(wzx.Message);
            }
            finally
            {
                mybook.Close(false, false, System.Reflection.Missing.Value);

                myExcel.Quit();

                GC.Collect();
            }

        }

        protected void Button7_Click(object sender, EventArgs e)
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
                    GridView2.DataSource = objDataSet.Tables[0].DefaultView;
                    GridView2.DataBind();
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

        protected void Button8_Click(object sender, EventArgs e)
        {
            conn.Open();
            foreach (GridViewRow gr in GridView2.Rows)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "update ClientMaster set FamilyCode='" + gr.Cells[3].Text.Trim() + "'    where  ClientCode='" + gr.Cells[1].Text.Trim() + "'";
                cmd.ExecuteNonQuery();


            }
            MessageBox.Show("Update Done..!");
            conn.Close();
        }

        protected void Branch2DropDownList2_DataBound(object sender, EventArgs e)
        {
            Branch2DropDownList2.Items.Insert(0, "<--Select-->");
        }

        protected void RMDropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void RMDropDownList3_DataBound(object sender, EventArgs e)
        {
            RMDropDownList3.Items.Insert(0, "<--Select-->");
        }

    }
}
