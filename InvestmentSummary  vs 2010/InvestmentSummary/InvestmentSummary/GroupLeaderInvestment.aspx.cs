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
    public partial class GroupLeaderInvestment : System.Web.UI.Page
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
            cmd.CommandText = "select distinct invsum.FAMILYCODE as GroupLeaderCode from  INVESTMENTSUMMARY invsum,Cust_Client_Master ccm where invsum.IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' ";  //and ccm.branch='" + BranchDropDownList1.Text.Trim() + "' 
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dt.Columns.Add("Branch");
            dt.Columns.Add("Total");
      dr.Close();
      decimal total = 0;
      foreach (DataRow drw in dt.Rows)
      {
          cmd = conn.CreateCommand();
          cmd.CommandText = "select  MF , CLILENTCODE     from  INVESTMENTSUMMARY   where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and  FAMILYCODE='" + drw["GroupLeaderCode"] + "'   ";
          dr = cmd.ExecuteReader();
          if (dr.HasRows)
          {
              while (dr.Read())
              {
                  string mf = dr[0].ToString();
                  //string cash = dr[1].ToString();
                  //string pms = dr[2].ToString();
                  //string FNO = dr[3].ToString();

                  if (!string.IsNullOrEmpty(mf))
                  {
                      total = total + Convert.ToDecimal(dr[0].ToString());
                  }
                  //if (!string.IsNullOrEmpty(cash))
                  //{
                  //    total = total + Convert.ToDecimal(dr[1].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(pms))
                  //{
                  //    total = total + Convert.ToDecimal(dr[2].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(FNO))
                  //{
                  //    total = total + Convert.ToDecimal(dr[3].ToString());
                  //}
              }
               

          }

          dr.Close();
          cmd = conn.CreateCommand();
          cmd.CommandText = "select  FNO ,CLILENTCODE   from  INVESTMENTSUMMARY   where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and  FAMILYCODE='" + drw["GroupLeaderCode"] + "' ";
          dr = cmd.ExecuteReader();
          if (dr.HasRows)
          {
              while (dr.Read())
              {
                  string mf = dr[0].ToString();
                  //string cash = dr[1].ToString();
                  //string pms = dr[2].ToString();
                  //string FNO = dr[3].ToString();

                  if (!string.IsNullOrEmpty(mf))
                  {
                      total = total + Convert.ToDecimal(dr[0].ToString());
                  }
                  //if (!string.IsNullOrEmpty(cash))
                  //{
                  //    total = total + Convert.ToDecimal(dr[1].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(pms))
                  //{
                  //    total = total + Convert.ToDecimal(dr[2].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(FNO))
                  //{
                  //    total = total + Convert.ToDecimal(dr[3].ToString());
                  //}
              }


          }

          dr.Close();
          cmd = conn.CreateCommand();
          cmd.CommandText = "select  CASH ,CLILENTCODE  from  INVESTMENTSUMMARY   where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and  FAMILYCODE='" + drw["GroupLeaderCode"] + "' ";
          dr = cmd.ExecuteReader();
          if (dr.HasRows)
          {
              while (dr.Read())
              {
                  string mf = dr[0].ToString();
                  //string cash = dr[1].ToString();
                  //string pms = dr[2].ToString();
                  //string FNO = dr[3].ToString();

                  if (!string.IsNullOrEmpty(mf))
                  {
                      total = total + Convert.ToDecimal(dr[0].ToString());
                  }
                  //if (!string.IsNullOrEmpty(cash))
                  //{
                  //    total = total + Convert.ToDecimal(dr[1].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(pms))
                  //{
                  //    total = total + Convert.ToDecimal(dr[2].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(FNO))
                  //{
                  //    total = total + Convert.ToDecimal(dr[3].ToString());
                  //}
              }


          }

          dr.Close();
          cmd = conn.CreateCommand();
          cmd.CommandText = "select  PMS ,CLILENTCODE   from  INVESTMENTSUMMARY   where IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and  FAMILYCODE='" + drw["GroupLeaderCode"].ToString() + "'  ";
          dr = cmd.ExecuteReader();
          if (dr.HasRows)
          {
              while (dr.Read())
              {
                  string mf = dr[0].ToString();
                  //string cash = dr[1].ToString();
                  //string pms = dr[2].ToString();
                  //string FNO = dr[3].ToString();

                  if (!string.IsNullOrEmpty(mf))
                  {
                      total = total + Convert.ToDecimal(dr[0].ToString());
                  }
                  //if (!string.IsNullOrEmpty(cash))
                  //{
                  //    total = total + Convert.ToDecimal(dr[1].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(pms))
                  //{
                  //    total = total + Convert.ToDecimal(dr[2].ToString());
                  //}
                  //if (!string.IsNullOrEmpty(FNO))
                  //{
                  //    total = total + Convert.ToDecimal(dr[3].ToString());
                  //}
              }


          }

          dr.Close();
          drw["Total"] = total;
          total = 0;
      }
            DataRow drowtemp = dt.NewRow();
            for (int pass = 1; pass <= dt.Rows.Count - 2; pass++)
            {
                for (int i = 0; i <= dt.Rows.Count - 2; i++)
                {
                    if (Convert.ToDecimal(dt.Rows[i][2].ToString())> Convert.ToDecimal(dt.Rows[i + 1][2].ToString()))
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            drowtemp[j] = dt.Rows[i + 1][j];
                            dt.Rows[i + 1][j] = dt.Rows[i][j];
                            dt.Rows[i][j] = drowtemp[j];
                        }
                    }

                }

            }
            foreach (DataRow drw in dt.Rows)
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "select   branch    from  Cust_Client_Master   where clientcode='" + drw["GroupLeaderCode"].ToString() + "'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        drw["Branch"] = dr["branch"].ToString();
                    }
                }
                dr.Close();
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
            conn.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void DateDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
