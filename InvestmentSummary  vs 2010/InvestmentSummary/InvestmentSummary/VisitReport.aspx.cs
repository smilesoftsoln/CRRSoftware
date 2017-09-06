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
    public partial class VisitReport : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "Select RM,Branch,count(distinct FamilyCode) as FamilyCount from ClientMaster group by RM,Branch";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dt.Columns.Add("Visit Done");
            dt.Columns.Add("Visit Pending");
            dr.Close();
            int familytotal = 0;
            int visittotal = 0;
            foreach (DataRow drw in dt.Rows)
            {
                familytotal = familytotal + Convert.ToInt32(drw["FamilyCount"]);
                drw["Visit Done"] = 0;
                drw["Visit Pending"] = drw["FamilyCount"].ToString();
                cmd = conn.CreateCommand();
                cmd.CommandText = "Select RM, count(distinct FamilyCode) from ClientMaster where VisitStatus='Visit Done' and RM='" + drw[0].ToString() + "' group by RM";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    
                    dr.Read();
                    drw["Visit Done"] = dr[1].ToString();
                    if (drw["Visit Done"] != null)
                    {
                        visittotal =visittotal+ Convert.ToInt32(drw["Visit Done"]);
                        drw["Visit Pending"] = Convert.ToInt64(drw["FamilyCount"]) - Convert.ToInt64(drw["Visit Done"]);
                    }
                    
                }
                dr.Close();
            }

            lblTotalFamily.Text = familytotal.ToString();
            lblVisitDone.Text = visittotal.ToString();
            lblPending.Text = (familytotal - visittotal).ToString();
            GridView1.DataSource = dt;
            GridView1.DataBind();

            conn.Close();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = "Select RM,Branch,count(distinct FamilyCode) as FamilyCount from ClientMaster group by RM,Branch";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dt.Columns.Add("Visit Done");
            dt.Columns.Add("Visit Pending");
            dr.Close();
            foreach (DataRow drw in dt.Rows)
            {
                drw["Visit Done"] = 0;
                drw["Visit Pending"] = drw["FamilyCount"].ToString();
                cmd = conn.CreateCommand();
                cmd.CommandText = "Select RM, count(distinct FamilyCode) from ClientMaster where VisitStatus='Visit Done' and RM='" + drw[0].ToString() + "' group by RM";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    drw["Visit Done"] = dr[1].ToString();
                    if (drw["Visit Done"] != null)
                    {
                        drw["Visit Pending"] = Convert.ToInt64(drw["FamilyCount"]) - Convert.ToInt64(drw["Visit Done"]);
                    }
                     
                }
                dr.Close();
            }



         //   GridView1.DataSource = dt;
            if (System.IO.File.Exists(Server.MapPath("/") + "Book2.xls")) //It checks if file exists then it delete that file.
            {
                System.IO.File.Delete(Server.MapPath("/") + "Book2.xls");
            }
            Export1(dt, Server.MapPath("/") + "Book2.xls");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + "Book2.xls");
            Response.ContentType = "application/excel";
            Response.WriteFile(Server.MapPath("/") + "Book2.xls");
            //GridView1.DataBind();
            conn.Close();
        }
        public static void Export1(DataTable dt, string filepath)
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
                    mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).WrapText = true;

                    mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Font.Bold = true;
                    mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Font.Size = 10;
                    mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    ExcelApp.Borders borders = mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]).Borders;
                    borders[ExcelApp.XlBordersIndex.xlEdgeLeft].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    borders[ExcelApp.XlBordersIndex.xlEdgeTop].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    borders[ExcelApp.XlBordersIndex.xlEdgeBottom].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    borders[ExcelApp.XlBordersIndex.xlEdgeRight].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                    borders.Color = 0;
                    borders[ExcelApp.XlBordersIndex.xlInsideVertical].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    borders[ExcelApp.XlBordersIndex.xlInsideHorizontal].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    borders[ExcelApp.XlBordersIndex.xlDiagonalUp].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    borders[ExcelApp.XlBordersIndex.xlDiagonalDown].LineStyle = ExcelApp.XlLineStyle.xlLineStyleNone;
                    borders = null;
                    //  mysheet.Columns.WrapText = mysheet.get_Range(myExcel.Cells[rowIndex + 1, colIndex], myExcel.Cells[rowIndex + 1, colIndex]);
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
                    //string str = drow[3].ToString();

                    //if ((!string.IsNullOrEmpty(drow[3].ToString())))
                    //{
                    //    decimal sum = Convert.ToDecimal(str);
                    //    if (sum <= 0)
                    //    {
                    //        break;
                    //    }
                    //}
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        myExcel.Cells[rowIndex + 1, colIndex] = drow[dcol.ColumnName];
                    }


                    //    //if (sum != 0)
                    //    //{
                    //    int col = colIndex;

                    //    for (int i = 0; i <= col; i++)
                    //    {
                    //        ExcelApp.Range rng = (ExcelApp.Range)mysheet.Cells[rowIndex + 1, col];
                    //        //  mysheet.Columns.WrapText = rng;
                    //        //  rng.WrapText = true;

                    //        rng.Style = style1;
                    //        ExcelApp.Borders borders = mysheet.get_Range(myExcel.Cells[rowIndex + 1, col], myExcel.Cells[rowIndex + 1, col]).Borders;

                    //        borders[ExcelApp.XlBordersIndex.xlEdgeBottom].LineStyle = ExcelApp.XlLineStyle.xlContinuous;

                    //    }
                    //    //}
                    //}
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
    }
}