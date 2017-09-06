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
    public partial class DP919 : System.Web.UI.Page
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
            DataSet result = new DataSet();
            FileUpload1.SaveAs(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName));
            FileStream stream = File.Open(System.IO.Path.Combine(Server.MapPath("Data"), FileUpload1.FileName), FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            result = excelReader.AsDataSet();
            excelReader.Close();
            string a = "";
            int row_no = 1;
            // result.Tables[0].Columns.Add("SubBroker");
            result.Tables[0].Columns.Add("Net Risk");

            DataTable dtnew = new DataTable();
            dtnew.Columns.Add("ClientCode");
            dtnew.Columns.Add("ClientName");
            dtnew.Columns.Add("Value");
            dtnew.Columns.Add("DematCode");
            for (int i = 1; i < result.Tables[0].Rows.Count; i++)
            {
                DataRow drw = dtnew.NewRow();
                drw["ClientCode"] = result.Tables[0].Rows[i][3].ToString();
                drw["ClientName"] = result.Tables[0].Rows[i][1].ToString();
                drw["Value"] = result.Tables[0].Rows[i][2].ToString();
                drw["DematCode"] = result.Tables[0].Rows[i][0].ToString();
              
                
                dtnew.Rows.Add(drw);
                
            }
            GridView1.DataSource = dtnew;
            GridView1.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            conn.Open();
            //cmd = conn.CreateCommand();
            //cmd.CommandText ="delete from POA" ;
            //cmd.ExecuteNonQuery();
            foreach (GridViewRow gr in GridView1.Rows)
            {
                cmd = conn.CreateCommand();

                cmd.CommandText = "select * from POA where DematCode=@DematCode and ClientCode=@ClientCode and type='DP919' and uploadDate='" + DateTime.Today.ToString() + "' ";
                // cmd.Parameters.AddWithValue("Value", Convert.ToDecimal(gr.Cells[2].Text));
                cmd.Parameters.AddWithValue("ClientCode", gr.Cells[0].Text);
                cmd.Parameters.AddWithValue("DematCode", gr.Cells[3].Text);

                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    cmd = conn.CreateCommand();

                    cmd.CommandText = "insert into POA(DematCode,Value,ClientCode,type,uploadDate) values(@DematCode,@Value,@ClientCode,'DP919','" + DateTime.Today.ToString() + "') ";
                    cmd.Parameters.AddWithValue("Value", Convert.ToDecimal(gr.Cells[2].Text));
                    cmd.Parameters.AddWithValue("ClientCode", gr.Cells[0].Text);
                    cmd.Parameters.AddWithValue("DematCode", gr.Cells[3].Text);

                    cmd.ExecuteNonQuery();
                }
                dr.Close();
            }

            
            conn.Close();
            MessageBox.Show("Updated Successfully");
            conn.Open();


            cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('DP919.xls','" + DateTime.Today.ToString() + "')";

            cmd.ExecuteNonQuery();

            conn.Close();

            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
    }
}
