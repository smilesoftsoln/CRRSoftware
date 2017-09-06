/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InvestmentSummary
{
    public partial class ReportOnPan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}*/
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;
namespace InvestmentSummary
{
    public partial class ReportOnPan : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        DataTable dt;
        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            //Label2.Text = Session["role"].ToString();
            //Label3.Text = Session["Branch"].ToString();
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            //if (Label2.Text.Equals("Admin"))
            //{
            //    conn.Close();conn.Open();
            //    cmd = conn.CreateCommand();
            //    cmd.CommandText = "select * from ClientMaster where  ClientCode=FamilyCode";

            //    dr = cmd.ExecuteReader();
            //    if (dr.HasRows)
            //    {
            //       //TextBox1.Visible = false;
            //        //Button1.Visible = false;
            //        dt = new DataTable();
            //        dt.Load(dr);
            //        GridView1.DataSource = null;
            //        GridView1.DataBind();
            //        GridView1.DataSource = dt;
            //        GridView1.DataBind();
            //    }

            //    dr.Close();
            //    conn.Close();
            //}


        }
        */
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            DataTable dtCl;
            conn.Close();conn.Open();
            string PAN = Request.QueryString["PAN"];
            string ClientFromPAN="";
            string FamilyCodeFromPAN="";
            if (!string.IsNullOrEmpty(PAN))
            {
                cmd = conn.CreateCommand();
                try
                {
                    cmd.CommandText = "select clientcode from Cust_Client_Master where panno like '%" + PAN + "%'";
                    dr = cmd.ExecuteReader();
                    dtCl = new DataTable();
                    dtCl.Load(dr);
                    dr.Close();
                    if (dtCl.Rows.Count == 0)
                    {
                        cmd.CommandText = "select clientalias from MF_Client_Master where panno like '%" + PAN + "%'";
                        dr = cmd.ExecuteReader();
                        dtCl = new DataTable();
                        dtCl.Load(dr);
                        dr.Close();
                        if (dtCl.Rows.Count == 0)
                        {
                            cmd.CommandText = "select PMSCODE from PMSMASTER where PAN like '%" + PAN + "%'";
                            dr = cmd.ExecuteReader();
                            dtCl = new DataTable();
                            dtCl.Load(dr);
                            dr.Close();
                        }
                    }

                    if (dtCl.Rows.Count > 0)
                    {
                        ClientFromPAN = dtCl.Rows[0][0].ToString();
                        cmd.CommandText = "select FamilyCode from ClientMaster where ClientCode='" + ClientFromPAN + "'";

                        dr = cmd.ExecuteReader();
                        dtCl = new DataTable();
                        dtCl.Load(dr);
                        dr.Close();
                        if (dtCl.Rows.Count > 0)
                        {
                            FamilyCodeFromPAN = dtCl.Rows[0][0].ToString();
                        }

                    }
                }
                catch (Exception ex)
                { 
                
                }

                /*  if (Label2.Text.Equals("BM"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%' and Branch='" + Label3.Text + "'";
                }
                else if (Label2.Text.Equals("RM"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%' and RM='" + Session["login"].ToString() + "'";

                }
                else if (Label2.Text.Equals("Admin"))
                {
                    cmd.CommandText = "select * from ClientMaster where ClientName like '%" + TextBox1.Text.Trim() + "%'";

                }*/
                
                dr.Close();
                conn.Close();conn.Open();
                //GridViewRow gr = GridView1.SelectedRow;
                string familycode1 = FamilyCodeFromPAN;
                cmd = conn.CreateCommand();
                cmd.CommandText = "select max(IS_date) from INVESTMENTSUMMARY";
                DateTime date1 = (DateTime)cmd.ExecuteScalar();

                cmd = conn.CreateCommand();
                cmd.CommandText = "select invsum.CLILENTCODE as ClientCode,ccm.FamilyCode as Family,ccm.ClientName as ClientName,ccm.branch as Branch,invsum.CASH as Equity,invsum.FNO,invsum.PMS,invsum.MF,invsum.FD from  INVESTMENTSUMMARY invsum,ClientMaster ccm where IS_date='" + date1 + "' and ccm.FamilyCode= '" + familycode1 + "'  and invsum.CLILENTCODE=ccm.ClientCode     order by ccm.ClientCode ";

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
                    decimal fd = 0;
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
                    if (!(dt.Rows[i]["FD"] is DBNull))
                    {
                        fd = Convert.ToDecimal(dt.Rows[i]["FD"]);
                    }
                    if (!(dt.Rows[i]["Equity"] is DBNull))
                    {
                        cash = Convert.ToDecimal(dt.Rows[i]["Equity"]);
                    }
                    // drw["PMS"].ToString()drw["MF"].ToString()drw["CASH"].ToString()
                    dt.Rows[i]["Total"] = fno + pms + cash + mf + fd;
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
                        familytotal = familytotal + fno + pms + cash + mf + fd;
                        dt.Rows[i]["FamilyTotal"] = familytotal;
                        //familytotal = 0;
                    }
                    else
                    {
                        familytotal = familytotal + fno + pms + cash + mf + fd;

                    }

                }

                dt.Columns.Remove("FamilyTotal");

                /******Working code for EXPORT TO PDF******/

                DataGrid dgall = new DataGrid();
                dgall.DataSource = dt;
                dgall.DataBind();

                StringWriter swall = new StringWriter();

                HtmlTextWriter hwall = new HtmlTextWriter(swall);

                dgall.RenderControl(hwall);

                cmd = conn.CreateCommand();
                cmd.CommandText = "Select CM.ClientCode,CM.ClientName from ClientMaster CM where CM.FamilyCode='" + familycode1 + "'";
                dr.Close();
                dr = cmd.ExecuteReader();
                DataTable dtclients = new DataTable();
                dtclients.Load(dr);
                dr.Close(); string strpms = "";

                foreach (DataRow drclients in dtclients.Rows)
                {
                    string clientcd = drclients[0].ToString();
                    string clientnm = drclients[1].ToString();
                    strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                    /************equity Details   **/
                    cmd = conn.CreateCommand();
                    // cmd.CommandText = "Select CM.ClientCode,CM.ClientName  from ClientMaster CM,EqutyDetails ED where CM.FamilyCode='" + familycode1 + "' and CM.ClientCode=ED.ClientCode and CM.ClientCode not in (select   ClientCode from MarginFundingDetails ) ";
                    cmd.CommandText = "Select CM.ClientCode,CM.ClientName  from ClientMaster CM,EqutyDetails ED where   ED.IDate='" + date1 + "' and  CM.FamilyCode='" + familycode1 + "' and CM.ClientCode='" + drclients[0].ToString() + "'  and CM.ClientCode=ED.ClientCode and CM.ClientCode not in (select   distinct ClientCode from MarginFundingDetails where IDate='" + date1 + "') order by CM.ClientCode ";

                    dr.Close();
                    dr = cmd.ExecuteReader();
                    DataTable dtcli = new DataTable();

                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        // string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientCode, LegBal, CashColl, NonCashColl, DebitStock ,POToday ,ShrtValue, FutPOValue, POAValue ,Total  from EqutyDetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {
                            //cmd = conn.CreateCommand();
                            //cmd.CommandText = "Select  sum(Valuation) as PMS_Valuation from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";

                            //DataRow drnew = dtpms.NewRow();
                            //drnew[0] = "<b>Total----</b>";
                            //drnew[1] = (decimal)cmd.ExecuteScalar();
                            //dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            // strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center ><b><u>Equity Details</u></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select max(uploadDate) from POA";
                        DateTime datemax = (DateTime)cmd.ExecuteScalar();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientCode, DematCode,Value from POA where ClientCode='" + clientcd + "' and uploadDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select  sum(Value) as Value from POA where ClientCode='" + clientcd + "' and uploadDate='" + date1 + "'";

                            DataRow drnew = dtpms.NewRow();
                            drnew[1] = "<b>Total----</b>";
                            drnew[2] = (decimal)cmd.ExecuteScalar();
                            dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            //  strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + swpms.ToString();
                            strpms = strpms + "<br/>";
                        }
                    }


                    /********FNO*******/
                    cmd = conn.CreateCommand();
                    // cmd.CommandText = "Select CM.ClientCode,CM.ClientName  from ClientMaster CM,EqutyDetails ED where CM.FamilyCode='" + familycode1 + "' and CM.ClientCode=ED.ClientCode and CM.ClientCode not in (select   ClientCode from MarginFundingDetails ) ";
                    cmd.CommandText = "Select distinct CM.ClientCode,CM.ClientName  from ClientMaster CM,FNODetails ED where CM.FamilyCode='" + familycode1 + "' and CM.ClientCode='" + drclients[0].ToString() + "'  and CM.ClientCode=ED.ClientCode ";

                    dr.Close();
                    dr = cmd.ExecuteReader();
                    dtcli = new DataTable();

                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        // string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientCode,Cash,Non_Cash_A_HairCut as B_HairCut, Leg_Bal_With_Bill,Total_Margin_Reporting as TotalMargin,FNO_Total from FNODetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        //strpms = "";
                        if (dtpms.Rows.Count > 0)
                        {
                            //cmd = conn.CreateCommand();
                            //cmd.CommandText = "Select  sum(Valuation) as PMS_Valuation from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";

                            //DataRow drnew = dtpms.NewRow();
                            //drnew[0] = "<b>Total----</b>";
                            //drnew[1] = (decimal)cmd.ExecuteScalar();
                            //dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            // strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center><b><u>FNO Details</u ></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }
                    }
                    /************Margin Funding Details   **/
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select CM.ClientCode,CM.ClientName  from ClientMaster CM,MarginFundingDetails ED where CM.FamilyCode='" + familycode1 + "' and CM.ClientCode=ED.ClientCode and ED.IDate='" + date1 + "' and CM.ClientCode='" + drclients[0].ToString() + "' and CM.ClientCode in (select clientcode as ClientCode from Cust_Client_Master ) ";
                    dr.Close();
                    dr = cmd.ExecuteReader();
                    dtcli = new DataTable();
                    //  strpms = "";

                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        // string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientCode,UnApprovedMktValue  as UnApprovedStkValue,ApprovedMktValue as ApprovedStkValue, Odd_LotMktValue as ZGroupStkValue, LedgerBal,DP_Valuation ,NetRisk  from MarginFundingDetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {
                            //cmd = conn.CreateCommand();
                            //cmd.CommandText = "Select  sum(Valuation) as PMS_Valuation from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";

                            //DataRow drnew = dtpms.NewRow();
                            //drnew[0] = "<b>Total----</b>";
                            //drnew[1] = (decimal)cmd.ExecuteScalar();
                            //dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            // strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center ><b><u>Equity Details[Marging Funding]</u></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select max(uploadDate) from POA";
                        DateTime datemax = (DateTime)cmd.ExecuteScalar();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientCode, DematCode,Value from POA where ClientCode='" + clientcd + "' and uploadDate='" + date1 + "' and type='DP919'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select  sum(Value) as Value from POA where ClientCode='" + clientcd + "' and uploadDate='" + date1 + "' and type='DP919'";

                            DataRow drnew = dtpms.NewRow();
                            drnew[1] = "<b>Total----</b>";
                            drnew[2] = (decimal)cmd.ExecuteScalar();
                            dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            //  strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + swpms.ToString();
                            strpms = strpms + "<br/>";
                        }











                    }


                    /***************/
                    /************PMS Details for equity client code **/
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select CM.ClientCode,CM.ClientName,CCM.panno from ClientMaster CM,Cust_Client_Master CCM where CM.FamilyCode='" + familycode1 + "' and CM.ClientCode='" + drclients[0].ToString() + "' and CM.ClientCode=CCM.clientcode and CM.ClientCode in (select clientcode as ClientCode from Cust_Client_Master ) ";
                    dr.Close();
                    dr = cmd.ExecuteReader();
                    dtcli = new DataTable();
                    // strpms = "";

                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select Scheme,Valuation as PMS_Valuation from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select  sum(Valuation) as PMS_Valuation from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";

                            DataRow drnew = dtpms.NewRow();
                            drnew[0] = "<b>Total----</b>";
                            drnew[1] = (decimal)cmd.ExecuteScalar();
                            dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            // strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center ><b><u>PMS Details</u></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }
                    }


                    /***************/
                    /************PMS Details for PMS client code **/
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select CM.ClientCode,CM.ClientName,PMSM.PAN from ClientMaster CM,PMSMASTER PMSM where CM.FamilyCode='" + familycode1 + "' and CM.ClientCode='" + drclients[0].ToString() + "'   and CM.ClientCode=PMSM.PMSCODE  and CM.ClientCode in (select PMSCODE as ClientCode from PMSMASTER ) ";
                    dr.Close();
                    dr = cmd.ExecuteReader();
                    dtcli = new DataTable();
                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select Scheme,Valuation as PMS_Valuation  from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select  sum(Valuation) as PMS_Valuation from PMSDetails where  PANNO='" + clientpan + "' and PMSDate='" + date1 + "'";

                            DataRow drnew = dtpms.NewRow();
                            drnew[0] = "<b>Total----</b>";
                            drnew[1] = (decimal)cmd.ExecuteScalar();
                            dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            //strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center><b> <u>PMS Details</u></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }
                    }


                    /***************/
                    //           /************MF Details  **/
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select distinct CM.ClientCode,CM.ClientName from ClientMaster CM,MFDetails MFD  where  CM.FamilyCode='" + familycode1 + "' and CM.ClientCode=MFD.ClientCode and CM.ClientCode='" + drclients[0].ToString() + "' and MFD.IDate='" + date1 + "' order by  CM.ClientCode";
                    dr.Close();
                    dr = cmd.ExecuteReader();
                    dtcli = new DataTable();
                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();
                    // string strpms = "";

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        //   string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientName,Value  as MF_Valuation  from MFDetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {


                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select sum(Value)  as MF_Valuation  from MFDetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";

                            DataRow drnew = dtpms.NewRow();
                            drnew[0] = "<b>Total----</b>";
                            drnew[1] = (decimal)cmd.ExecuteScalar();
                            dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            //strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center ><b><u>MF Details</u></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }
                    }


                    /***************/
                    //           /************FD Details  **/
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "Select distinct CM.ClientCode,CM.ClientName from ClientMaster CM,FDDetails FDD  where  CM.FamilyCode='" + familycode1 + "' and CM.ClientCode=FDD.ClientCode and CM.ClientCode='" + drclients[0].ToString() + "' and FDD.IDate='" + date1 + "' order by  CM.ClientCode";
                    dr.Close();
                    dr = cmd.ExecuteReader();
                    dtcli = new DataTable();
                    if (dr.HasRows)
                    {

                        dtcli.Load(dr);

                    }

                    dr.Close();
                    // string strpms = "";

                    foreach (DataRow dtclir in dtcli.Rows)
                    {
                        clientcd = dtclir[0].ToString();
                        clientnm = dtclir[1].ToString();
                        //   string clientpan = dtclir[2].ToString();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select ClientName,Value  as FD from FDDetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";
                        dr.Close();
                        dr = cmd.ExecuteReader();
                        DataTable dtpms = new DataTable();
                        dtpms.Load(dr);
                        if (dtpms.Rows.Count > 0)
                        {


                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select sum(Value)  as FD  from FDDetails where ClientCode='" + clientcd + "' and IDate='" + date1 + "'";

                            DataRow drnew = dtpms.NewRow();
                            drnew[0] = "<b>Total----</b>";
                            drnew[1] = (decimal)cmd.ExecuteScalar();
                            dtpms.Rows.Add(drnew);
                            DataGrid dgpms = new DataGrid();
                            dgpms.DataSource = dtpms;
                            dgpms.DataBind();

                            StringWriter swpms = new StringWriter();

                            HtmlTextWriter hwpms = new HtmlTextWriter(swpms);
                            //strpms = strpms + "<br/><br/><b> Client Code:-</b>" + clientcd + "<b> Client Name:- </b> " + clientnm + " <br/><br/>";

                            dgpms.RenderControl(hwpms);
                            strpms = strpms + "<center ><b><u>FD Details</u></b></center><br/><br/>" + swpms.ToString(); strpms = strpms + "<br/>";
                        }
                    }


                    /***************/


                }



                string groupleader = "";
                cmd = conn.CreateCommand();
                cmd.CommandText = " select ClientName from  ClientMaster where  ClientCode='" + ClientFromPAN + "'";
                groupleader = cmd.ExecuteScalar().ToString();
                string outputuptobodystart = "<!DOCTYPE html><html lang='en'><title></title><meta charset='utf-8'><!--[if lt IE 9]><script src='http://html5shiv.googlecode.com/svn/trunk/html5.js'></script><![endif]--><style>body {    font-family:Verdana,sans-serif;font-size:0.8em;}header,footer,section,article {    border:1px solid grey;   margin:5px;margin-bottom:15px;padding:8px;    background-color:white;}header,footer {    color:white;background-color:#444;margin-bottom:5px;}section {    background-color:#ddd;}nav ul {    margin:0;padding:0;}nav ul li {    display:inline; margin:5px;}</style>";
                string body = "<body><center> <table cellspacing='0' rules='all'  ><tr ><td style=' text-align: center;font-size: 20px' >TRADENET WEALTH MANAGERS PVT LTD,KOLHAPUR</td></tr><td style=' text-align: center;font-size: 15px' >Indicative Familywise Segmentwise Summary Report<sup><b>*</b></sup></td></tr><tr><td style=' text-align: center;' >Client Name:-" + groupleader.ToUpper() + "</td></tr></table></center></br></br> " + swall.ToString() + "</br><table cellspacing='0' rules='all'  ><tr ><td style=' text-align: right;border-style: solid; border-width: 1px' > <b> FAMILY TOTAL:   " + familytotal + "</b></td></tr><tr><td  style=' text-align: right;' ><sup>*</sup>As on Date:- " + date1.ToString("dd-MMM-yyyy") + "</td></tr></table></br><b>Disclaimer:-</b><p style='font-size: 10px'> MF Valuation includes minor applicants valuation also.</p><p style='font-size: 10px' >This report is for information and should not construed as final investment summary for legal and taxation purpose.</p><p style='font-size: 10px'>Please check out individual balances from various segments before acting on this report.</p><p style='font-size: 10px'>Tradenet will not be responsible for any lapses that may occur because of error in this report.</p>" + strpms + "</body>";
                string end = "</html>";
                
                // string output = "<center> <table cellspacing='0' rules='all'  ><tr ><td style=' text-align: center;font-size: 20px' >TRADENET WEALTH MANAGERS PVT LTD,KOLHAPUR</td></tr><td style=' text-align: center;font-size: 15px' >Indicative Familywise Segmentwise Summary Report<sup><b>*</b></sup></td></tr><tr><td style=' text-align: center;' >Client Name:-" + groupleader.ToUpper() + "</td></tr></table></center></br></br> " + swall.ToString() + "</br><table cellspacing='0' rules='all'  ><tr ><td style=' text-align: right;border-style: solid; border-width: 1px' > <b> FAMILY TOTAL:   " + familytotal + "</b></td></tr><tr><td  style=' text-align: right;' ><sup>*</sup>As on Date:- " + date1.ToString("dd-MMM-yyyy") + "</td></tr></table></br><b>Disclaimer:-</b><p style='font-size: 10px'> MF Valuation includes minor applicants valuation also.</p><p style='font-size: 10px' >This report is for information and should not construed as final investment summary for legal and taxation purpose.</p><p style='font-size: 10px'>Please check out individual balances from various segments before acting on this report.</p><p style='font-size: 10px'>Tradenet will not be responsible for any lapses that may occur because of error in this report.</p>" + strpms;
                
                //StringReader sr = new StringReader("<center> <table cellspacing='0' rules='all'  ><tr ><td style=' text-align: center;font-size: 20px' >TRADENET WEALTH MANAGERS PVT LTD,KOLHAPUR</td></tr><td style=' text-align: center;font-size: 15px' >Indicative Familywise Segmentwise Summary Report<sup><b>*</b></sup></td></tr><tr><td style=' text-align: center;' >Client Name:-" + groupleader.ToUpper() + "</td></tr></table></center></br></br> " + swall.ToString() + "</br><table cellspacing='0' rules='all'  ><tr ><td style=' text-align: right;border-style: solid; border-width: 1px' > <b> FAMILY TOTAL:   " + familytotal + "</b></td></tr><tr><td  style=' text-align: right;' ><sup>*</sup>As on Date:- " + date1.ToString("dd-MMM-yyyy") + "</td></tr></table></br><b>Disclaimer:-</b><p style='font-size: 10px'> MF Valuation includes minor applicants valuation also.</p><p style='font-size: 10px' >This report is for information and should not construed as final investment summary for legal and taxation purpose.</p><p style='font-size: 10px'>Please check out individual balances from various segments before acting on this report.</p><p style='font-size: 10px'>Tradenet will not be responsible for any lapses that may occur because of error in this report.</p>" + strpms);
                //Document pdfDoc = new Document(PageSize.A3, 10f, 10f, 10f, 0f);
              //  string imageFilePath = Server.MapPath(".") + @"\pdf_logo.jpg";
                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);

                //Resize image depend upon your need
                //jpg.ScaleToFit(100f, 80f);
                // jpg.SetAbsolutePosition(1f, 1f);

                //Give space before image
                // jpg.SpacingBefore = 30f;

                //Give some space after the image
                // jpg.SpacingAfter = 1f;
                // jpg.Alignment = Element.ALIGN_CENTER;

                //Response.ContentType = "application/pdf";

                ///Response.AddHeader("content-disposition", "attachment;filename=Summary.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
               // HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
               // pdfDoc.Open();
                //pdfDoc.Add(jpg);
                //pdfDoc.NewPage();
                //htmlparser.Parse(sr);
               // pdfDoc.Close();
                Response.Write(outputuptobodystart+body+end);
                Response.End();
                conn.Close();
            }
            
            //cmd.CommandText="
            conn.Close();

        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
    }
}
/***
 * Physical Address: 00-40-F4-BC-26-95
IP Address: 10.56.65.45
Subnet Mask: 255.255.255.0
Default Gateway: 10.56.65.211
DNS Servers: 202.149.208.11, 202.149.208.92
WINS Server: 
 * ***************/
