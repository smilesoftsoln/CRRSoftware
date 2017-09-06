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
    public partial class ImportOFLReport : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
         
                
        }

        protected void GetDataButton1_Click(object sender, EventArgs e)
        {
DataSet result = new DataSet();
            FileUpload1.SaveAs(System.IO.Path.Combine(Server.MapPath(@"..\Data"), FileUpload1.FileName));
            FileStream stream = File.Open(System.IO.Path.Combine(Server.MapPath(@"..\Data"), FileUpload1.FileName), FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            result = excelReader.AsDataSet();
            excelReader.Close();
            result.Tables[0].Columns["TradeDate"].ReadOnly=false;
           //result.Tables[0].Columns["TradeDate"].DataType = DateTime.Now.GetType();
            foreach (DataRow drw in result.Tables[0].Rows)
            {
                drw["TradeDate"] = DateTime.FromOADate(Convert.ToDouble( drw["TradeDate"].ToString())).ToString("dd-MMM-yyyy");
            }
            GridView1.DataSource = result.Tables[0];
            GridView1.DataBind();
        }

        protected void ImportButton1_Click(object sender, EventArgs e)
        {
            conn.Open();

            cmd = conn.CreateCommand();
            GridViewRow grw = GridView1.Rows[0];
            string strdate = grw.Cells[1].Text;
            DateTime date1 = Convert.ToDateTime(strdate);

            cmd.CommandText = "delete from OFLReport where TradeDate='" + strdate + "'";
            cmd.ExecuteNonQuery();
            
            
            foreach (GridViewRow gr in GridView1.Rows)
            {

                cmd = conn.CreateCommand();
                cmd.CommandText = "insert into OFLReport(ClientCode,Terminal,Segment,TradeDate,SubBroker,Matched,GivenTo,ContactType,OtherRemark)values(@ClientCode,@Terminal,@Segment,@TradeDate,@SubBroker,'UnConfirmed','NotGiven','No','No')";

                cmd.Parameters.AddWithValue("@ClientCode", gr.Cells[5].Text.Trim());
                cmd.Parameters.AddWithValue("@Terminal", gr.Cells[2].Text.Trim());
                cmd.Parameters.AddWithValue("@Segment", gr.Cells[4].Text.Trim());
                cmd.Parameters.AddWithValue("@TradeDate", gr.Cells[1].Text.Trim());
                cmd.Parameters.AddWithValue("@SubBroker", gr.Cells[0].Text.Trim());
               
                if (!gr.Cells[5].Text.Trim().Equals(gr.Cells[2].Text))
                { cmd.ExecuteNonQuery(); }
            
            }
            cmd = conn.CreateCommand();

            //cmd.CommandText = "select distinct ClientCode,Terminal,Segment,TradeDate,SubBroker  from OFLReport where TradeDate='" + strdate + "' and SubBroker in('RKOLHHNI','RTRADENET','RKOLHRATNA','KOLHMANGAL','RBHARTIOSW','RKOLHVIDYA','RVIDYACHIP','RTRDEKUDAL','RVBSPOWAIN','RKOLHNIPCG','RKUSHALHS','RKHCHANDRA','KOLHFRDESK','RKOLHRAJAR')";
            cmd.CommandText = "select distinct ClientCode,Terminal,Segment,TradeDate,SubBroker  from OFLReport where TradeDate='" + strdate + "' and SubBroker in(select Subbroker from  SBCODE)";
            
            dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd = conn.CreateCommand();
           
            cmd.CommandText = "delete from OFLReport where TradeDate='" + strdate + "'";
            cmd.ExecuteNonQuery();

            foreach (DataRow dtrw in dt.Rows)
            {

                cmd = conn.CreateCommand();
              //  cmd.CommandText = "insert into OFLReport(ClientCode,Terminal,Segment,TradeDate,SubBroker)values(@ClientCode,@Terminal,@Segment,@TradeDate,@SubBroker)";
                cmd.CommandText = "insert into OFLReport(ClientCode,Terminal,Segment,TradeDate,SubBroker,Matched,GivenTo,ContactType,OtherRemark)values(@ClientCode,@Terminal,@Segment,@TradeDate,@SubBroker,'UnConfirmed','NotGiven','No','No')";

                cmd.Parameters.AddWithValue("@ClientCode", dtrw[0].ToString().Trim());
                cmd.Parameters.AddWithValue("@Terminal", dtrw[1].ToString().Trim());
                cmd.Parameters.AddWithValue("@Segment", dtrw[2].ToString().Trim());
                cmd.Parameters.AddWithValue("@TradeDate", dtrw[3].ToString().Trim());
                cmd.Parameters.AddWithValue("@SubBroker", dtrw[4].ToString().Trim());


                cmd.ExecuteNonQuery();

            }

            conn.Close();
            MessageBox.Show("Upload Done");
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    }
}