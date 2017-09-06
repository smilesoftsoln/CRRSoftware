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
    public partial class ImportLog : System.Web.UI.Page
    {
         SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

         

            Label2.Text = DateTime.Today.ToString("dd-MMM-yyyy");
            conn.Open();
            cmd = conn.CreateCommand();
            cmd.CommandText = " SELECT     FILEMASTER.FileName FROM         FILEMASTER where        FILEMASTER.FileName not in (select FileName from  UploadLog where UploadDate='"+Convert.ToDateTime(Label2.Text)+"')";
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            if (dr.HasRows)
            {
                dt.Load(dr);
            }
            GridView2.DataSource = dt;
            GridView2.DataBind();
            dr.Close();
            conn.Close();

        }
    }
}
