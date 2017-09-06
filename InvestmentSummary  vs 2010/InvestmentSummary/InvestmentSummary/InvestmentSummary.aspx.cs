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
    public partial class InvestmentSummary : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
          // lblBook3.Visible = false;

            if (System.IO.File.Exists(Server.MapPath("/") + "Book3.xlsx")) //It checks if file exists then it delete that file.
            {
                System.IO.File.Delete(Server.MapPath("/") + "Book3.xlsx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();
            if (BranchDropDownList1.SelectedIndex != 0)
            {
                cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,UPPER(ccm.RM) as RM ,invsum.CASH as Equity,invsum.FNO,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToString("dd-MMM-yyyy") + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.ClientCode not like 'KB12%'  and ccm.Branch='" + BranchDropDownList1.Text.Trim() + "' order by ccm.FamilyCode desc";
            }
            else
            {
                cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,UPPER(ccm.RM) as RM ,invsum.CASH as Equity,invsum.FNO,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToString("dd-MMM-yyyy") + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.ClientCode not like 'KB12%'   order by ccm.FamilyCode desc";
            
            }
            
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            //cmd = conn.CreateCommand();
            //cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,invsum.FAMILYCODE as Family,mfm.groupalias as Status,mfm.clientname as ClientName,mfm.branch as Branch,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,MF_Client_Master mfm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=mfm.clientalias and mfm.branch='" + BranchDropDownList1.Text.Trim() + "' order by invsum.FAMILYCODE";
            //dr = cmd.ExecuteReader();
            //dt.Load(dr);
            //dr.Close();
            dt.Columns.Add("Total");
            dt.Columns.Add("FamilyTotal"); 
           decimal familytotal = 0;
           string family = "";
           string family1 = "";
            for(int i=0;i< dt.Rows.Count;i++)
            {

                decimal fno = 0;
                decimal mf = 0;
                decimal pms = 0;
                decimal cash = 0;

                if (!(dt.Rows[i]["FNO"] is DBNull))
{
    fno = Convert.ToDecimal(dt.Rows[i]["FNO"]);
}
                if (!(dt.Rows[i]["PMS"] is DBNull))
{
    pms = Convert.ToDecimal(dt.Rows[i]["PMS"]);
}
                if (!(dt.Rows[i]["MF"] is DBNull))
{
    mf = Convert.ToDecimal(dt.Rows[i]["MF"]);
}
                if (!(dt.Rows[i]["Equity"] is DBNull))
{
    cash = Convert.ToDecimal(dt.Rows[i]["Equity"]);
}
               // drw["PMS"].ToString()drw["MF"].ToString()drw["CASH"].ToString()
                dt.Rows[i]["Total"] = fno + pms + cash + mf;
                family = dt.Rows[i]["family"].ToString();
                if (i < dt.Rows.Count-1)
                {
                    family1 = dt.Rows[i + 1]["family"].ToString();
                }
                else
                {
                    family1 = "";
                }
             if (!family1.Equals(family))
             {
                 familytotal = familytotal + fno + pms + cash + mf;
                 dt.Rows[i]["FamilyTotal"] = familytotal;
                 familytotal = 0;
             }
             else
             {
                 familytotal = familytotal + fno + pms + cash + mf;

             }
                 
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
            //MessageBox.Show("DATA EXPORTING STARTED");
            //if (System.IO.File.Exists(Server.MapPath("/") + "Book2.xlsx")) //It checks if file exists then it delete that file.
            //{
            //    System.IO.File.Delete(Server.MapPath("/") + "Book2.xlsx");
            //}
            //Export(dt, Server.MapPath("/") + "Book2.xlsx");
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment; filename=" + "Book2.xlsx");
            //Response.ContentType = "application/excel";
            //Response.WriteFile(Server.MapPath("/") + "Book2.xlsx");
            //MessageBox.Show("DATA EXPORTING COMPLETED");
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
                    foreach (DataColumn dcol in dt.Columns)
                    {
                        colIndex = colIndex + 1;
                        myExcel.Cells[rowIndex + 1, colIndex] = drow[dcol.ColumnName];
                    }
                    string str = drow["FamilyTotal"].ToString();
                   
                    if((!string.IsNullOrEmpty(str)) )
                    {
                        decimal sum = Convert.ToDecimal(str);
                        //if (sum != 0)
                        //{
                        int col = colIndex;

                        for (int i = 0; i <= col; i++)
                        {
                            //ExcelApp.Range rng = (ExcelApp.Range)mysheet.Cells[rowIndex + 1, col];
                            //  mysheet.Columns.WrapText = rng;
                            //  rng.WrapText = true;

                            //rng.Style = style1;
                            //ExcelApp.Borders borders = mysheet.get_Range(myExcel.Cells[rowIndex + 1, col], myExcel.Cells[rowIndex + 1, col]).Borders;

                            //borders[ExcelApp.XlBordersIndex.xlEdgeBottom].LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                
                        }
                        //}
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();
            if (BranchDropDownList1.SelectedIndex != 0)
            {
                cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,UPPER(ccm.RM) as RM ,invsum.CASH as Equity,invsum.FNO,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToString("dd-MMM-yyyy") + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.ClientCode not like 'KB12%'  and ccm.Branch='" + BranchDropDownList1.Text.Trim() + "' order by ccm.FamilyCode desc";
            }
            else
            {
                cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,UPPER(ccm.RM) as RM ,invsum.CASH as Equity,invsum.FNO,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToString("dd-MMM-yyyy") + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.ClientCode not like 'KB12%'   order by ccm.FamilyCode desc";

            }
            //if (BranchDropDownList1.SelectedIndex != 0)
            //{
            //    cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,ccm.RM,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.ClientCode not like 'KB12%'  and ccm.Branch='" + BranchDropDownList1.SelectedItem.Value.Trim() + "' order by ccm.FamilyCode desc";
            //}
            //else
            //{
            //    cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,ccm.RM,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.ClientCode not like 'KB12%' order by ccm.FamilyCode desc";
            
            //}
            //cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.family as Family,ccm.inactivefrom as Status,ccm.clientname as ClientName,ccm.branch as Branch,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,Cust_Client_Master ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=ccm.clientcode and ccm.branch='" + BranchDropDownList1.Text.Trim() + "' order by ccm.family";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            //cmd = conn.CreateCommand();
            //cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,invsum.FAMILYCODE as Family,mfm.groupalias as Status,mfm.clientname as ClientName,mfm.branch as Branch,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,MF_Client_Master mfm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=mfm.clientalias and mfm.branch='" + BranchDropDownList1.Text.Trim() + "' order by invsum.FAMILYCODE";
            //dr = cmd.ExecuteReader();
            //dt.Load(dr);
            //dr.Close();
            dt.Columns.Add("Total");
            dt.Columns.Add("FamilyTotal");
            decimal familytotal = 0;
            string family = "";
            string family1 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                decimal fno = 0;
                decimal mf = 0;
                decimal pms = 0;
                decimal cash = 0;

                if (!(dt.Rows[i]["FNO"] is DBNull))
                {
                    fno = Convert.ToDecimal(dt.Rows[i]["FNO"]);
                }
                if (!(dt.Rows[i]["PMS"] is DBNull))
                {
                    pms = Convert.ToDecimal(dt.Rows[i]["PMS"]);
                }
                if (!(dt.Rows[i]["MF"] is DBNull))
                {
                    mf = Convert.ToDecimal(dt.Rows[i]["MF"]);
                }
                if (!(dt.Rows[i]["Equity"] is DBNull))
                {
                    cash = Convert.ToDecimal(dt.Rows[i]["Equity"]);
                }
                // drw["PMS"].ToString()drw["MF"].ToString()drw["CASH"].ToString()
                dt.Rows[i]["Total"] = fno + pms + cash + mf;
                family = dt.Rows[i]["family"].ToString();
                if (i < dt.Rows.Count - 1)
                {
                    family1 = dt.Rows[i + 1]["family"].ToString();
                }
                else
                {
                    family1 = "";
                }
                if (!family1.Equals(family))
                {
                    familytotal = familytotal + fno + pms + cash + mf;
                    dt.Rows[i]["FamilyTotal"] = familytotal;
                    familytotal = 0;
                }
                else
                {
                    familytotal = familytotal + fno + pms + cash + mf;

                }

            }
           // GridView1.DataSource = dt;
            //GridView1.DataBind();
          //  MessageBox.Show("DATA EXPORTING STARTED");
            if (System.IO.File.Exists(Server.MapPath("/") + "Book2.xlsx")) //It checks if file exists then it delete that file.
            {
                System.IO.File.Delete(Server.MapPath("/") + "Book2.xlsx");
            }
            Export(dt, Server.MapPath("/") + "Book2.xlsx");
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + "Book2.xlsx");
      // Response.ContentType = "application/excel";
         Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
          Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (DateTime.Now.Year < 18)
            {
              //  Response.Redirect("http://10.56.65.45:81/Book2.xlsx");
                Response.Redirect("~/Summary.aspx");
            }
//MessageBox.Show("DATA EXPORTING COMPLETED");
            conn.Close();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = conn.CreateCommand();
            if (BranchDropDownList1.SelectedIndex != 0)
            {
                cmd.CommandText = "select invsum.FAMILYCODE as Family, sum(  invsum.CASH ) as Equity,sum( invsum.FNO) as FNO ,Sum(invsum.PMS) as PMS,Sum( invsum.MF ) as MF from  INVESTMENTSUMMARY invsum ,ClientMaster cm where invsum.IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToString("dd-MMM-yyyy") + "' and cm.FamilyCode=invsum.FAMILYCODE  and cm.Branch='" + BranchDropDownList1.SelectedItem.Text + "'     group by invsum.FAMILYCODE order by invsum.FAMILYCODE desc";
            }
            else
            {
                cmd.CommandText = "select invsum.FAMILYCODE as Family, sum(  invsum.CASH ) as Equity,sum( invsum.FNO) as FNO ,Sum(invsum.PMS) as PMS,Sum( invsum.MF ) as MF from  INVESTMENTSUMMARY invsum  where invsum.IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToString("dd-MMM-yyyy") + "'  group by invsum.FAMILYCODE order by invsum.FAMILYCODE desc";

            }
            //if (BranchDropDownList1.SelectedIndex != 0)
            //{
            //    cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=ccm.ClientCode and ccm.Branch='" + BranchDropDownList1.SelectedItem.Value.Trim() + "' order by ccm.FamilyCode";
            //}
            //else
            //{
            //    cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,invsum.FNO,invsum.CASH,invsum.PMS,invsum.MF from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and invsum.CLILENTCODE=ccm.ClientCode  order by ccm.FamilyCode";
            
            //}
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            //dt.Columns.Add("FNO");

            //dt.Columns.Add("MF");
            //dt.Columns.Add("PMS");




            //dt.Columns.Add("Cash");
            dt.Columns.Add("Total");
            dt.Columns.Add("FamilyTotal");
            decimal familytotal = 0;
            string family = "";
            string family1 = "";
            decimal fno = 0;
            decimal mf = 0;
            decimal pms = 0;
            decimal cash = 0;
            decimal fnototal = 0;
            decimal mftotal = 0;
            decimal pmstotal = 0;
            decimal cashtotal = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

             
                if (!(dt.Rows[i]["FNO"] is DBNull))
                {
                    fno = Convert.ToDecimal(dt.Rows[i]["FNO"]);

                }
                if (!(dt.Rows[i]["PMS"] is DBNull))
                {
                    pms = Convert.ToDecimal(dt.Rows[i]["PMS"]);
                }
                if (!(dt.Rows[i]["MF"] is DBNull))
                {
                    mf = Convert.ToDecimal(dt.Rows[i]["MF"]);
                }
                if (!(dt.Rows[i]["Equity"] is DBNull))
                {
                    cash = Convert.ToDecimal(dt.Rows[i]["Equity"]);
                }
                // drw["PMS"].ToString()drw["MF"].ToString()drw["CASH"].ToString()
                
                family = dt.Rows[i]["family"].ToString();
                if (i < dt.Rows.Count - 1)
                {
                    family1 = dt.Rows[i + 1]["family"].ToString();
                }
                else
                {
                    family1 = "";
                }
                if (!family1.Equals(family))
                {
                    fnototal = fnototal + fno;
                    mftotal = mftotal + mf;
                    pmstotal = pmstotal + pms;
                    cashtotal = cashtotal + cash;
                    familytotal = familytotal + fno + pms + cash + mf;
                    dt.Rows[i]["FamilyTotal"] = familytotal;
                    //dt.Rows[i]["FNO"] = fnototal;
                    //dt.Rows[i]["PMS"] = pmstotal;
                    //dt.Rows[i]["MF"] = mftotal;
                    //dt.Rows[i]["Equity"] = cashtotal;
                    //dt.Rows[i]["Total"] = fnototal + pmstotal + cash + mftotal;
                    familytotal = 0;
                    fnototal = 0;
                    mftotal = 0;
                    pmstotal = 0;
                    cashtotal = 0;
                      fno = 0;
                      mf = 0;
                      pms = 0;
                      cash = 0;
                     
                }
                else
                {
                    familytotal = familytotal + fno + pms + cash + mf;
                    fnototal = fnototal + fno;
                    mftotal = mftotal + mf;
                    pmstotal = pmstotal + pms;
                    cashtotal = cashtotal + cash;
                }

            }
            // GridView1.DataSource = dt;
            //GridView1.DataBind();
            //  MessageBox.Show("DATA EXPORTING STARTED");
            if (System.IO.File.Exists(Server.MapPath("/") + "Book3.xlsx")) //It checks if file exists then it delete that file.
            {
                System.IO.File.Delete(Server.MapPath("/") + "Book3.xlsx");
            }

            DataTable dtsumm = new DataTable();
            dtsumm.Columns.Add("Branch");

            dtsumm.Columns.Add("FamilyCode");
            dtsumm.Columns.Add("GroupLeader");
            dtsumm.Columns.Add("FNO");

            dtsumm.Columns.Add("MF");
            dtsumm.Columns.Add("PMS");




            dtsumm.Columns.Add("Equity");



            dtsumm.Columns.Add("Total");
            dtsumm.Columns.Add("RM");

            foreach (DataRow drt in dt.Rows)
            {
                if (!string.IsNullOrEmpty(drt["FamilyTotal"].ToString()))
                {
                    DataRow drnew = dtsumm.NewRow();
                    //drnew["Branch"]=drt["Branch"];
                        drnew["FamilyCode"]=drt["family"];
                        drnew["FNO"] = drt["FNO"];
                        drnew["PMS"] = drt["PMS"];
                        drnew["MF"] = drt["MF"];
                        drnew["Equity"] = drt["Equity"];
                        drnew["Total"] = drt["FamilyTotal"];
                        cmd = conn.CreateCommand();
                        if (BranchDropDownList1.SelectedIndex == 0)
                        {
                            cmd.CommandText = "select   ClientName, RM ,Branch  from  ClientMaster     where ClientCode='" + drt["family"].ToString() + "'";
                        }
                        else
                        {
                            cmd.CommandText = "select   ClientName, RM ,Branch  from  ClientMaster     where ClientCode='" + drt["family"].ToString() + "' and Branch='" + BranchDropDownList1.SelectedItem.Text + "'";

                        }
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                drnew["GroupLeader"] = dr["clientname"].ToString();
                                drnew["RM"] = dr["RM"].ToString().ToUpper();
                                drnew["Branch"] = dr["Branch"].ToString().ToUpper();
                            }
                            dr.Close();
                        }
                        else
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            if (BranchDropDownList1.SelectedIndex == 0)
                            {
                                cmd.CommandText = "select   cc.ClientName,cm.RM,cc.branch   from  ClientMaster cm,Cust_Client_Master cc  where cc.clientcode='" + drt["family"].ToString() + "' and cc.clientcode=cm.FamilyCode";
                            }
                            else
                            {
                                cmd.CommandText = "select   cc.ClientName,cm.RM,cc.branch   from  ClientMaster cm,Cust_Client_Master cc where cc.branch='" + BranchDropDownList1.SelectedItem.Text + "' and  cc.clientcode='" + drt["family"].ToString() + "' and cc.clientcode=cm.FamilyCode";
                            
                            }
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {

                                    drnew["GroupLeader"] = dr["clientname"].ToString();
                                    drnew["RM"] = dr["RM"].ToString().ToUpper();
                                    drnew["Branch"] = dr["branch"].ToString().ToUpper();
                                }
                                dr.Close();
                            }
                        }
                        dr.Close();



                    if(!string.IsNullOrEmpty(drnew["Branch"].ToString()))
                    {
                    dtsumm.Rows.Add(drnew);
                    
                    }
                }
            
            }

            DataRow drowtemp = dtsumm.NewRow();
            for (int pass = 1; pass <= dtsumm.Rows.Count - 2; pass++)
            {
                for (int i = 0; i <= dtsumm.Rows.Count - 2; i++)
                {
                    if (Convert.ToDecimal(dtsumm.Rows[i][7].ToString()) < Convert.ToDecimal(dtsumm.Rows[i + 1][7].ToString()))
                    {
                        for (int j = 0; j < dtsumm.Columns.Count; j++)
                        {
                            drowtemp[j] = dtsumm.Rows[i + 1][j];
                            dtsumm.Rows[i + 1][j] = dtsumm.Rows[i][j];
                            dtsumm.Rows[i][j] = drowtemp[j];
                        }
                    }

                }

            }
            if (System.IO.File.Exists(Server.MapPath("/") + "Book3.xlsx")) //It checks if file exists then it delete that file.
            {
                System.IO.File.Delete(Server.MapPath("/") + "Book3.xlsx");
            }
            Export1(dtsumm, Server.MapPath("/") + "Book3.xlsx");
          
            
            //MessageBox.Show("DATA EXPORTING COMPLETED");
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
                    string str = drow[7].ToString();

                  if ((!string.IsNullOrEmpty(drow[7].ToString())))
                    {
                          decimal sum = Convert.ToDecimal(str);
                          if (sum <= 0)
                          {
                               break;
                          }
                    }  
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
                MessageBox.Show("DATA EXPORTING COMPLETED open Link");
                //lblBook3.Visible = true;
          

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

        protected void BranchDropDownList1_DataBound(object sender, EventArgs e)
        {
           // ListItem lst = new ListItem("ALL", "%%");
            BranchDropDownList1.Items.Insert(0, "ALL");
        }

    }
}
