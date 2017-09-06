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

namespace InvestmentSummary.confirmation
{
    public partial class ContactModification : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            conn.Open();

            foreach (GridViewRow grw in GridView1.Rows)
            {

                string modification = "";

                string landline1 = grw.Cells[1].Text.Replace("&nbsp;","").Trim();
                string landline2 = grw.Cells[2].Text.Replace("&nbsp;", "").Trim();
                string mobileno = grw.Cells[3].Text.Replace("&nbsp;", "").Trim(); 

                if (!string.IsNullOrEmpty(landline1.Trim()))
                {
                    modification = modification + " landline1='" + landline1 + "',";
                
                
                }

                if (!string.IsNullOrEmpty(landline2.Trim()))
                {
                    modification = modification + " landline2='" + landline2 + "',";


                }
                if (!string.IsNullOrEmpty(mobileno.Trim()))
                {
                    modification = modification + " mobileno='" + mobileno + "',";


                }

                if(!string.IsNullOrEmpty(modification.Trim()))
                {
                    modification = modification.Substring(0, modification.Length - 1);

                string query ="update Cust_Client_Master set "+modification+" where clientcode='"+grw.Cells[0].Text.ToUpper()+"'";



 cmd = conn.CreateCommand();
 cmd.CommandText = query;
 int no= cmd.ExecuteNonQuery();
 if (no != 0)
 {

     cmd = conn.CreateCommand();
     cmd.CommandText = "insert into ContactModification(UpdateDate,ClientCode,Landline1,Landline2,Mobile)values(@UpdateDate,@ClientCode,@Landline1,@Landline2,@Mobile)";
     //@UpdateDate,@ClientCode,@Landline1,@Landline2,@Mobile
     cmd.Parameters.AddWithValue("@UpdateDate",DateTime.Today);
     cmd.Parameters.AddWithValue("@ClientCode",grw.Cells[0].Text.ToUpper()); 
     cmd.Parameters.AddWithValue("@Landline1",landline1);
     cmd.Parameters.AddWithValue("@Landline2",landline2);
     cmd.Parameters.AddWithValue("@Mobile",mobileno);
     cmd.ExecuteNonQuery();
 }
                }
            
            }





            MessageBox.Show("Modifications Done..!");
            GridView1.DataSource = null;
            GridView1.DataBind();



            conn.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataSet result = new DataSet();
            FileUpload1.SaveAs(System.IO.Path.Combine(Server.MapPath("/"), FileUpload1.FileName));
            FileStream stream = File.Open(System.IO.Path.Combine(Server.MapPath("/"), FileUpload1.FileName), FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
         excelReader.IsFirstRowAsColumnNames = true;  
            result = excelReader.AsDataSet();
            excelReader.Close();
            
         GridView1.DataSource=   result.Tables[0];//.Columns.Add("Net Risk");
         GridView1.DataBind();
        
        }
    }
}