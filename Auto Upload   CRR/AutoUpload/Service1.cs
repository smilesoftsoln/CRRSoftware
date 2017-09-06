using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Reflection;
using excelapp= Microsoft.Office.Interop.Excel;
namespace BrokerageMail
{
    public partial class Service1 : ServiceBase
    {

        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        System.Timers.Timer mailTimer;
        SqlCommand sqlcmd;
        SqlDataReader sqldr;
        String   sortby   ;
        String query ; 
        String   data  ;
        String     groupby   ;
        DataTable dtnew;
        int row_no = 0;
        String       where   ;
        public Service1()
        {
            InitializeComponent();
  //conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=CRR;Integrated Security=True");
            conn = new SqlConnection(@"Server=10.56.65.10\MSSQLSERVER1;Initial Catalog=CRR;user id=sa;password=sa123");//on 22 sep 2016
      
//            conn = new SqlConnection(@"Data Source=10.56.65.10\SQLEXPRESS;Initial Catalog=CRR;Integrated Security=True");
//on laptop            conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename='D:\Softwares Under Development\Changes Since In Pune\Auto Upload 15 MAR 2016\InvestmentSummary.mdf';Integrated Security=True;Connect Timeout=30;User Instance=True");
        
        }

        protected override void OnStart(string[] args)
        {

            mailTimer = new System.Timers.Timer();
            mailTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetSummaryMail);
            mailTimer.Interval = 60000;//60000=1 min
            mailTimer.Enabled = true;
            mailTimer.AutoReset = true;
            mailTimer.Start();
             

        }

        public void GetSummaryMail(object sender, System.Timers.ElapsedEventArgs arg)
        {
            mailTimer.Stop();
            #region Uncomment the code in this region to run the TimedAccess class
            // TimedAccess timedAccess = new TimedAccess();
            // timedAccess.Read();
            // return;
            #endregion
            excelapp.Application app = new excelapp.Application();
           excelapp. Workbook book = null;
           excelapp.Worksheet sheet = null;
           excelapp.Range range = null;
           object[,] values;
           conn.Open();
           mail("Started To Upload Files....", "....","");
            try
            {

               
                int duplicates = 0;
                int newrecords = 0;
                string[] update;
                DataTable dt=new DataTable();
                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from UploadLog";//  where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' ";

                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from EqutyDetails where  IDate<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from FNODetails where  IDate<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from INVESTMENTSUMMARY  where  IS_date<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from MarginFundingDetails  where  IDate<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from MFDetails  where  Idate<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from FDDetails  where  Idate<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "delete from PMSDetails  where  PMSDate<='" + DateTime.Today.AddDays(-40).ToString("dd-MMM-yyyy") + "' ";
                cmd.ExecuteNonQuery();

                /**********************
                 * 
                 * Equity Clients Listing upload
                 * 
                 * **************/
                if (File.Exists(@"D:\\CC\\EquityClients.xls"))
                {
                    try
                    {
                        app.Visible = false;
                        app.ScreenUpdating = false;
                        app.DisplayAlerts = false;




                        book = app.Workbooks.Open(@"D:\\CC\\EquityClients.xls", Missing.Value, Missing.Value, Missing.Value
                                                          , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                         , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                        , Missing.Value, Missing.Value, Missing.Value);
                        sheet = (excelapp.Worksheet)book.Worksheets[1];

                        range = sheet.get_Range("A1", "af11000");

                        values = (object[,])range.Value2;

                        dt = new DataTable();
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            dt.Columns.Add(values[1, j].ToString());


                        }

                        for (int i = 2; i <= values.GetLength(0); i++)
                        {
                            DataRow drw = dt.NewRow();

                            for (int j = 1; j <= values.GetLength(1); j++)
                            {
                                try
                                {
                                    string str = values[i, j].ToString();
                                    drw[j - 1] = str;
                                }
                                catch (Exception ex)
                                {
                                    drw[j - 1] = "";

                                }
                            }

                            dt.Rows.Add(drw);
                        }
                        /****************Client Master Equity*****************/

                        update = new string[dt.Rows.Count];

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            if (!string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                            {
                                string qry1 = "select clientid from Cust_Client_Master where clientcode='" + dt.Rows[i][1].ToString() + "'";
                                cmd = new SqlCommand(qry1, conn);
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    duplicates++;
                                    update[i] = dr[0].ToString();



                                    dr.Close();
                                }
                                else
                                {
                                    dr.Close();
                                    string temp = dt.Rows[i][0].ToString();
                                    cmd = new SqlCommand("insert into Cust_Client_Master(clientcode,family,clientname,shortname,branch,subbrokercode,trader,landline1,landline2,mobileno,emailid,panno,activefrom,inactivefrom,approvedby,introducer,bankcode,bankname,paymentmode,defaultdpld,clientdpld,clientaccno,clientbankname,address1,address2,address3,city,state,nation,zip,insdate,update1) values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25,@26,@27,@28,@29,@30,'" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.AddHours(2).ToShortDateString() + "')", conn);

                                    cmd.Parameters.AddWithValue("@1", dt.Rows[i][1].ToString());
                                    cmd.Parameters.AddWithValue("@2", dt.Rows[i][2].ToString());
                                    cmd.Parameters.AddWithValue("@3", dt.Rows[i][3].ToString());
                                    cmd.Parameters.AddWithValue("@4", dt.Rows[i][4].ToString());
                                    cmd.Parameters.AddWithValue("@5", dt.Rows[i][6].ToString());
                                    cmd.Parameters.AddWithValue("@6", dt.Rows[i][7].ToString());
                                    cmd.Parameters.AddWithValue("@7", dt.Rows[i][8].ToString());
                                    cmd.Parameters.AddWithValue("@8", dt.Rows[i][9].ToString());
                                    cmd.Parameters.AddWithValue("@9", dt.Rows[i][10].ToString());
                                    cmd.Parameters.AddWithValue("@10", dt.Rows[i][11].ToString());
                                    cmd.Parameters.AddWithValue("@11", dt.Rows[i][12].ToString());
                                    cmd.Parameters.AddWithValue("@12", dt.Rows[i][13].ToString());
                                    cmd.Parameters.AddWithValue("@13", dt.Rows[i][14].ToString());
                                    cmd.Parameters.AddWithValue("@14", dt.Rows[i][15].ToString());
                                    cmd.Parameters.AddWithValue("@15", dt.Rows[i][16].ToString());
                                    cmd.Parameters.AddWithValue("@16", dt.Rows[i][17].ToString());
                                    cmd.Parameters.AddWithValue("@17", dt.Rows[i][22].ToString());
                                    cmd.Parameters.AddWithValue("@18", dt.Rows[i][18].ToString());
                                    cmd.Parameters.AddWithValue("@19", dt.Rows[i][19].ToString());
                                    cmd.Parameters.AddWithValue("@20", dt.Rows[i][20].ToString());
                                    cmd.Parameters.AddWithValue("@21", dt.Rows[i][21].ToString());
                                    cmd.Parameters.AddWithValue("@22", dt.Rows[i][22].ToString());
                                    cmd.Parameters.AddWithValue("@23", dt.Rows[i][23].ToString());
                                    cmd.Parameters.AddWithValue("@24", dt.Rows[i][23].ToString());
                                    cmd.Parameters.AddWithValue("@25", dt.Rows[i][24].ToString());
                                    cmd.Parameters.AddWithValue("@26", dt.Rows[i][25].ToString());
                                    cmd.Parameters.AddWithValue("@27", dt.Rows[i][26].ToString());
                                    cmd.Parameters.AddWithValue("@28", dt.Rows[i][27].ToString());
                                    cmd.Parameters.AddWithValue("@29", dt.Rows[i][28].ToString());
                                    cmd.Parameters.AddWithValue("@30", dt.Rows[i][29].ToString());
                                    newrecords++;
                                    cmd.ExecuteNonQuery();

                                }
                            }

                        }
                        if (newrecords != 0)
                        {
                            cmd = new SqlCommand();
                            cmd.Connection = conn;

                            cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_LEADER' where update1='" + DateTime.Today + "'";
                            cmd.ExecuteNonQuery();
                            cmd = new SqlCommand(); //TILL DEMO remaining
                            cmd.Connection = conn;

                            cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_MEMBER' where clientcode!=family ";
                            cmd.ExecuteNonQuery();

                        }

                        if (duplicates != 0)
                        {

                            cmd = new SqlCommand();



                            cmd.Connection = conn;

                            cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_LEADER' where update1='" + DateTime.Today + "'";
                            cmd.ExecuteNonQuery();
                            cmd = new SqlCommand();
                            cmd.Connection = conn;

                            cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_MEMBER' where clientcode!=family ";
                            cmd.ExecuteNonQuery();


                        }



                        cmd = new SqlCommand();
                        cmd.CommandText = "Select Subbroker,BranchName from SBCODE order by BranchName desc";
                        cmd.Connection = conn;
                        dr = cmd.ExecuteReader();
                        DataTable dtbr = new DataTable();

                        if (dr.HasRows)
                        {
                            dtbr.Load(dr);
                        }
                        dr.Close();
                        foreach (DataRow dtr in dtbr.Rows)
                        {
                            cmd.CommandText = "update Cust_Client_Master set branch='" + dtr["BranchName"].ToString() + "' where subbrokercode='" + dtr["Subbroker"].ToString() + "'";
                            cmd.ExecuteNonQuery();
                        }
                        duplicates = 0;

                        /********update equity clients***************/

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(update[i]))
                            {
                                string qry = "update Cust_Client_Master set family=@family,update1=@update1,trader=@trader,address2=@address2,city=@city,landline2=@landline2,panno=@panno,emailid=@emailid,mobileno=@mobileno,landline1=@landline1,clientname=@clientname,subbrokercode=@subbrokercode" + "  where clientid=" + update[i] + ""; //,subbrokercode=@subbrokercode;
                                cmd = new SqlCommand(qry, conn);

                                string landline1 = dt.Rows[i][9].ToString();
                                string landline2 = dt.Rows[i][10].ToString();


                                cmd.Parameters.AddWithValue("@clientname", dt.Rows[i][3].ToString());
                                cmd.Parameters.AddWithValue("@family", dt.Rows[i][2].ToString());
                                cmd.Parameters.AddWithValue("@landline1", dt.Rows[i][9].ToString());
                                cmd.Parameters.AddWithValue("@landline2", dt.Rows[i][10].ToString());
                                cmd.Parameters.AddWithValue("@mobileno", dt.Rows[i][11].ToString());
                                cmd.Parameters.AddWithValue("@emailid", dt.Rows[i][12].ToString());
                                cmd.Parameters.AddWithValue("@panno", dt.Rows[i][13].ToString());
                                cmd.Parameters.AddWithValue("@address2", dt.Rows[i][24].ToString());
                                cmd.Parameters.AddWithValue("@city", dt.Rows[i][26].ToString());
                                cmd.Parameters.AddWithValue("@trader", dt.Rows[i][8].ToString());
                                cmd.Parameters.AddWithValue("@update1", DateTime.Now.ToShortDateString());
                                cmd.Parameters.AddWithValue("@subbrokercode", dt.Rows[i][7].ToString());
                                duplicates++;
                                cmd.ExecuteNonQuery();
                            }

                        }

                        cmd = new SqlCommand();
                        cmd.Connection = conn;

                        cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_LEADER' where update1='" + DateTime.Today + "'";
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand();
                        cmd.Connection = conn;

                        cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_MEMBER' where clientcode!=family ";
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand();
                        cmd.CommandText = "Select Subbroker,BranchName from SBCODE order by BranchName desc";
                        cmd.Connection = conn;
                        dr = cmd.ExecuteReader();
                        dt = new DataTable();

                        if (dr.HasRows)
                        {
                            dt.Load(dr);
                        }
                        dr.Close();
                        foreach (DataRow dtr in dt.Rows)
                        {
                            cmd.CommandText = "update Cust_Client_Master set branch='" + dtr["BranchName"].ToString() + "' where subbrokercode='" + dtr["Subbroker"].ToString() + "'";
                            cmd.ExecuteNonQuery();
                        }

                        dr.Close();
                        //cmd.Connection = conn;
                        //cmd.CommandText = "update [ClientMaster]  set ClientMaster.Branch=(select Cust_Client_Master.branch from Cust_Client_Master where ClientMaster.[ClientCode]=[Cust_Client_Master].[clientcode]) where  ClientMaster.[ClientCode] in (select distinct Cust_Client_Master.ClientCode from Cust_Client_Master) ";
                        //cmd.ExecuteNonQuery();



                        mail("Equity Clients", "Successfully Uploaded the File", " ");
                    }
                    catch (Exception ex)
                    {





                        mail("Equity Clients", " Successfully Not Uploaded the File", "ccare06");





                    }





                }
                else
                {
                    /********************/
                    mail("Equity Clients", "Not Found the File", "ccare06");
                    /***************/
                }

                /**********************
                * 
                * Equity Clients Listing upload End
                * 
                * **************/
                /**********************
              * 
              * Mutual Fund Clients Listing upload
              * 
              * **************/



                if (File.Exists(@"D:\\MF\\MFClients.xls"))
                {
                    try
                    {
                        duplicates = 0;
                        newrecords = 0;
                        app.Visible = false;
                        app.ScreenUpdating = false;
                        app.DisplayAlerts = false;


                        book = app.Workbooks.Open(@"D:\\MF\\MFClients.xls", Missing.Value, Missing.Value, Missing.Value
                                                          , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                         , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                        , Missing.Value, Missing.Value, Missing.Value);
                        sheet = (excelapp.Worksheet)book.Worksheets[1];

                        range = sheet.get_Range("A1", "af11000");

                        values = (object[,])range.Value2;


                        dt = new DataTable();
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            dt.Columns.Add(values[3, j].ToString());

                        }
                        for (int i = 4; i <= values.GetLength(0); i++)
                        {
                            DataRow drw = dt.NewRow();
                            for (int j = 1; j <= values.GetLength(1); j++)
                            {
                                try
                                {
                                    string str = values[i, j].ToString();
                                    drw[j - 1] = str;
                                }
                                catch (Exception ex)
                                {
                                    drw[j - 1] = "";

                                }
                            }
                            dt.Rows.Add(drw);
                        }



                        /*************************/
                        update = new string[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            try
                            {
                                string branch = "";
                                string qry2 = "";
                                if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                                {
                                    qry2 = "select * from MF_Client_Master where clientalias='" + dt.Rows[i][1].ToString() + "'";
                                }

                                cmd = new SqlCommand(qry2, conn);
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    duplicates++;
                                    update[i] = dr[0].ToString();



                                    dr.Close();
                                }
                                else
                                {
                                    dr.Close();


                                    cmd = new SqlCommand("insert into MF_Client_Master(clientname,clientalias,mobileno,landline,emailid1,emailid2,address1,address2,address3,city,panno,dob,groupname,groupalias,subbroker,rm,fileno,lock,equity,debt,equitycode1,equitycode2,insdate,update1) values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22," + DateTime.Now.ToShortDateString() + "," + DateTime.Now.AddHours(2).ToShortDateString() + ")", conn);// + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','" + dt.Rows[i][2].ToString() + "','" + dt.Rows[i][3].ToString() + "','" + dt.Rows[i][4].ToString() + "','" + dt.Rows[i][5].ToString() + "','" + dt.Rows[i][6].ToString() + "','" + dt.Rows[i][7].ToString() + "','" + dt.Rows[i][8].ToString() + "','" + dt.Rows[i][9].ToString() + "','" + dt.Rows[i][10].ToString() + "','" + dt.Rows[i][11].ToString() + "','" + dt.Rows[i][12].ToString() + "','" + dt.Rows[i][13].ToString() + "','" + dt.Rows[i][14].ToString() + "','" + dt.Rows[i][15].ToString() + "','" + dt.Rows[i][16].ToString() + "','" + dt.Rows[i][17].ToString() + "','" + dt.Rows[i][18].ToString() + "','" + dt.Rows[i][19].ToString() + "','" + dt.Rows[i][20].ToString() + "','" + dt.Rows[i][21].ToString() + "','"+DateTime.Now.ToShortDateString()+"','')", conn);
                                    cmd.Parameters.AddWithValue("@1", dt.Rows[i][0].ToString());
                                    cmd.Parameters.AddWithValue("@2", dt.Rows[i][1].ToString());
                                    cmd.Parameters.AddWithValue("@3", dt.Rows[i][2].ToString());
                                    cmd.Parameters.AddWithValue("@4", dt.Rows[i][3].ToString());
                                    cmd.Parameters.AddWithValue("@5", dt.Rows[i][4].ToString());
                                    cmd.Parameters.AddWithValue("@6", dt.Rows[i][5].ToString());
                                    cmd.Parameters.AddWithValue("@7", dt.Rows[i][6].ToString());
                                    cmd.Parameters.AddWithValue("@8", dt.Rows[i][7].ToString());
                                    cmd.Parameters.AddWithValue("@9", dt.Rows[i][8].ToString());
                                    cmd.Parameters.AddWithValue("@10", dt.Rows[i][9].ToString());
                                    cmd.Parameters.AddWithValue("@11", dt.Rows[i][11].ToString());
                                    cmd.Parameters.AddWithValue("@12", dt.Rows[i][12].ToString());
                                    cmd.Parameters.AddWithValue("@13", dt.Rows[i][13].ToString());
                                    cmd.Parameters.AddWithValue("@14", dt.Rows[i][14].ToString());
                                    cmd.Parameters.AddWithValue("@15", dt.Rows[i][15].ToString());
                                    cmd.Parameters.AddWithValue("@16", dt.Rows[i][16].ToString());
                                    cmd.Parameters.AddWithValue("@17", dt.Rows[i][17].ToString());
                                    cmd.Parameters.AddWithValue("@18", dt.Rows[i][18].ToString());
                                    cmd.Parameters.AddWithValue("@19", dt.Rows[i][19].ToString());
                                    cmd.Parameters.AddWithValue("@20", dt.Rows[i][20].ToString());
                                    cmd.Parameters.AddWithValue("@21", dt.Rows[i][19].ToString());
                                    cmd.Parameters.AddWithValue("@22", dt.Rows[i][20].ToString());
                                    if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                                    {
                                        cmd.ExecuteNonQuery();
                                        newrecords++;
                                    }
                                }
                            }
                            catch (Exception eee)
                            { }

                        }


                        string quer = "Select subbroker,branch from MFBranch order by branch desc"; 
                        cmd = new SqlCommand(quer, conn);
                        dr = cmd.ExecuteReader();
                        DataTable dt12 = new DataTable();
                        dt12.Load(dr);
                        foreach (DataRow drow in dt12.Rows)
                        {
                            cmd = new SqlCommand("update MF_Client_Master set branch='" + drow[1].ToString().Trim() + "' where subbroker='" + drow[0].ToString().Trim() + "'", conn);
                            cmd.ExecuteNonQuery();
                        }
                        /*************UPDATE data********/
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(update[i]))
                            {
                                string qry = "update MF_Client_Master set panno=@panno, mobileno=@mobileno,emailid1=@emailid1,emailid2=@emailid2,address1=@address1,city=@city,dob=@dob,subbroker=@subbroker,landline=@landline,update1=@update1" + "  where clientid=" + update[i];
                                duplicates++;
                                cmd = new SqlCommand(qry, conn);
                                cmd.Parameters.AddWithValue("@panno", dt.Rows[i][11].ToString());
                                cmd.Parameters.AddWithValue("@mobileno", dt.Rows[i][2].ToString());
                                cmd.Parameters.AddWithValue("@emailid1", dt.Rows[i][4].ToString());
                                cmd.Parameters.AddWithValue("@emailid2", dt.Rows[i][5].ToString());
                                cmd.Parameters.AddWithValue("@address1", dt.Rows[i][6].ToString());
                                cmd.Parameters.AddWithValue("@city", dt.Rows[i][9].ToString());
                                cmd.Parameters.AddWithValue("@dob", dt.Rows[i][12].ToString());
                                cmd.Parameters.AddWithValue("@subbroker", dt.Rows[i][15].ToString());
                                cmd.Parameters.AddWithValue("@landline", dt.Rows[i][3].ToString());
                                cmd.Parameters.AddWithValue("@update1", DateTime.Now);

                                cmd.ExecuteNonQuery();
                            }

                        }
                        mail("Mutual Fund Clients", "Uploaded Successfully the File", " ");
                    }
                    catch (Exception ex)
                    {
                        mail("Mutual Fund Clients", "Not Uploaded Successfully the File", "ccare02");
                    }
                }

                else
                {

                    mail("Mutual Fund Clients", "Not Found the File", "ccare02");
                }
            //    /************************/

            //    /**********************
            //     * 
            //     * Mutual Fund Clients Listing upload
            //     * 
            //     * **************/
            //    /**********
            //     * 
            //     * 
            //     * DP919 upload
            //     * 
            //     * 
            //     * 
            //     * **********************************************/

            //    if (File.Exists(@"D:\\CC\\DP919.xls"))
               if (File.Exists(@"D:\\CC\\DP919.csv"))
                  {
                    try
                    {
                        duplicates = 0;
                        newrecords = 0;
                        app.Visible = false;
                        app.ScreenUpdating = false;
                        app.DisplayAlerts = false;


                        //book = app.Workbooks.Open(@"D:\\CC\\DP919.xls", Missing.Value, Missing.Value, Missing.Value
                        //                                  , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                        //                                 , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                        //                                , Missing.Value, Missing.Value, Missing.Value);
                        //sheet = (excelapp.Worksheet)book.Worksheets[1];

                        //range = sheet.get_Range("A1", "d11000");
                        //values = (object[,])range.Value2;

                        //dt = new DataTable();
                        //dt.Columns.Add("DematCode");
                        //dt.Columns.Add("ClientName");
                        //dt.Columns.Add("Value");
                        //dt.Columns.Add("ClientCode");

                        //for (int i = 2; i <= values.GetLength(0); i++)
                        //{
                        //    DataRow drw = dt.NewRow();
                        //    for (int j = 1; j <= values.GetLength(1); j++)
                        //    {
                        //        try
                        //        {
                        //            string str = values[i, j].ToString();
                        //            drw[j - 1] = str;
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            drw[j - 1] = "";
                        //            continue;
                        //        }
                        //    }
                        //    dt.Rows.Add(drw);
                        //}

                        StreamReader sr = new StreamReader(@"D:\\CC\\DP919.csv");

                        string strline = "";
                        string[] _values = null;
                        strline = sr.ReadLine();
                        strline = sr.ReadLine();
                        //strline = sr.ReadLine();

                        dt = new DataTable();
                        dt.Columns.Add("DematCode");
                        dt.Columns.Add("ClientName");
                        dt.Columns.Add("Value");

                        dt.Columns.Add("ClientCode");

                        int dateflag = 0;
                        _values = strline.Split(',');
                        string datefile = "";// _values[0].ToString().Substring(47, 11);
                        //string filedate = values[1, 1].ToString().Substring(40);
                        string actualdate = DateTime.Today.AddDays(0).ToString("MMM  d yyyy");

                        if (DateTime.Today.AddDays(0).Day < 10)
                        {
                            datefile = _values[0].ToString().Substring(48, 11);
                            actualdate = DateTime.Today.AddDays(0).ToString("MMM  d yyyy");
                        }
                        else
                        {
                            datefile = _values[0].ToString().Substring(48, 11);
                            actualdate = DateTime.Today.AddDays(0).ToString("MMM d yyyy");
                        }
                        strline = sr.ReadLine();
                        strline = sr.ReadLine();
                        while (strline != null)
                        {
                          
                            if (datefile.Equals(actualdate))
                            {
                                dateflag = 0;
                                _values = strline.Split(',');

                                DataRow drow = dt.NewRow();
                                drow["DematCode"] = _values[0];
                                drow["ClientName"] = _values[1];
                                drow["Value"] = _values[2];
                                drow["ClientCode"] = _values[3];
                                
                                dt.Rows.Add(drow);
                                strline = sr.ReadLine();
                            }
                            else
                            {
                                dateflag = 1;
                                break;
                            }



                        }




                        if (dateflag == 0)
                        {

                        foreach (DataRow gr in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(gr[3].ToString()))
                            {
                                cmd = conn.CreateCommand();

                                cmd.CommandText = "select * from POA where DematCode=@DematCode and ClientCode=@ClientCode and type='DP919' and uploadDate='" + DateTime.Today.ToString() + "' ";
                                cmd.Parameters.AddWithValue("ClientCode", gr[3].ToString());
                                cmd.Parameters.AddWithValue("DematCode", gr[0].ToString());

                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();

                                    cmd.CommandText = "insert into POA(DematCode,Value,ClientCode,type,uploadDate) values(@DematCode,@Value,@ClientCode,'DP919','" + DateTime.Today.ToString() + "') ";
                                    cmd.Parameters.AddWithValue("Value", Convert.ToDecimal(gr[2].ToString()));
                                    cmd.Parameters.AddWithValue("ClientCode", gr[3].ToString());
                                    cmd.Parameters.AddWithValue("DematCode", gr[0].ToString());

                                    cmd.ExecuteNonQuery();
                                }
                                dr.Close();
                            }
                        }
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('DP919.csv','" + DateTime.Today.ToString() + "')";

                        cmd.ExecuteNonQuery();
                        mail("DP919", "File Uploaded Successfully", " ");
                        }
                        else
                        {
                            mail("DP919", "Wrong Date File ", "ccare06");
                        }
                    }
                    catch (Exception ex)
                    {
                        mail("DP919", "File Not  Uploaded Successfully", "ccare06");
                    }
                }
                else
                {
                    mail("DP919", "File Not Found", "ccare06");

                }
            //    /**********
            //    * 
            //    * 
            //    * DP919 upload end
            //    * 
            //    * 
            //    * 
            //    * **********************************************/

            //    /**********
            //   * 
            //   * 
            //   * DP900 upload
            //   * 
            //   * 
            //   * 
            //   * **********************************************/
                //if (File.Exists(@"D:\\CC\\DP900.xls"))
                    if (File.Exists(@"D:\\CC\\DP900.csv"))
                {

                    try
                    {
                        duplicates = 0;
                        newrecords = 0;
                        app.Visible = false;
                        app.ScreenUpdating = false;
                        app.DisplayAlerts = false;


                        //book = app.Workbooks.Open(@"D:\\CC\\DP900.xls", Missing.Value, Missing.Value, Missing.Value
                        //                                  , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                        //                                 , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                        //                                , Missing.Value, Missing.Value, Missing.Value);
                        //sheet = (excelapp.Worksheet)book.Worksheets[1];

                        //range = sheet.get_Range("A1", "d11000");
                        //values = (object[,])range.Value2;


                        //dt = new DataTable();
                        //dt.Columns.Add("DematCode");
                        //dt.Columns.Add("ClientName");
                        //dt.Columns.Add("Value");

                        //dt.Columns.Add("ClientCode");

                        //for (int i = 2; i <= values.GetLength(0); i++)
                        //{
                        //    DataRow drw = dt.NewRow();
                        //    for (int j = 1; j <= values.GetLength(1); j++)
                        //    {
                        //        try
                        //        {
                        //            string str = values[i, j].ToString();
                        //            drw[j - 1] = str;
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            drw[j - 1] = "";
                        //            continue;
                        //        }
                        //    }
                        //    dt.Rows.Add(drw);
                        //}




                        StreamReader sr = new StreamReader(@"D:\\CC\\DP900.csv");

                        string strline = "";
                        string[] _values = null;
                        strline = sr.ReadLine();
                       strline = sr.ReadLine();
                        //strline = sr.ReadLine();

                        dt = new DataTable();
                        dt.Columns.Add("DematCode");
                        dt.Columns.Add("ClientName");
                        dt.Columns.Add("Value");

                        dt.Columns.Add("ClientCode");

                        int dateflag = 0;
                        _values = strline.Split(',');
                        string datefile = "";// _values[0].ToString().Substring(47, 11);
                        //string filedate = values[1, 1].ToString().Substring(40);
                        string actualdate = DateTime.Today.AddDays(-1).ToString("MMM  d yyyy");

                        if (DateTime.Today.AddDays(0).Day < 10)
                        {
                            datefile = _values[0].ToString().Substring(48, 11);
                            actualdate = DateTime.Today.AddDays(0).ToString("MMM  d yyyy");
                        }
                        else
                        {
                            datefile = _values[0].ToString().Substring(48, 11);
                            actualdate = DateTime.Today.AddDays(0).ToString("MMM d yyyy");
                        }
                        strline = sr.ReadLine();
                        strline = sr.ReadLine();
                        while (strline != null)
                        {
                          
                            if (datefile.Equals(actualdate))
                            {
                                dateflag = 0;

                                _values = strline.Split(',');
                                DataRow drow = dt.NewRow();
                                drow["DematCode"] = _values[0];
                                drow["ClientName"] = _values[1];
                                drow["Value"] = _values[2];
                                drow["ClientCode"] = _values[3];
                                
                                dt.Rows.Add(drow);
                                strline = sr.ReadLine();
                            }
                            else
                            {
                                dateflag = 1;
                                break;
                            }



                        }




                        if (dateflag == 0)
                        {

                            foreach (DataRow gr in dt.Rows)
                            {
                                if (!string.IsNullOrEmpty(gr[3].ToString()))
                                {
                                    cmd = conn.CreateCommand();

                                    cmd.CommandText = "select * from POA where DematCode=@DematCode and ClientCode=@ClientCode and type='DP900' and uploadDate='" + DateTime.Today.ToString() + "' ";
                                    cmd.Parameters.AddWithValue("ClientCode", gr[3].ToString());
                                    cmd.Parameters.AddWithValue("DematCode", gr[0].ToString());

                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();

                                        cmd.CommandText = "insert into POA(DematCode,Value,ClientCode,type,uploadDate) values(@DematCode,@Value,@ClientCode,'DP900','" + DateTime.Today.ToString() + "') ";
                                        cmd.Parameters.AddWithValue("Value", Convert.ToDecimal(gr[2].ToString()));
                                        cmd.Parameters.AddWithValue("ClientCode", gr[3].ToString());
                                        cmd.Parameters.AddWithValue("DematCode", gr[0].ToString());

                                        cmd.ExecuteNonQuery();
                                    }
                                    dr.Close();
                                }
                            }
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('DP900.csv','" + DateTime.Today.ToString() + "')";

                            cmd.ExecuteNonQuery();
                            mail("DP900", "File Uploaded Successfully", " ");

                        }
                        else
                        {
                            mail("DP900", "Wrong Date File ", "ccare06");
                        }
                    }
                    catch (Exception ex)
                    {
                        mail("DP900", "File Not  Uploaded Successfully", "ccare06");
                    }
                }
                else
                {
                    mail("DP900", "File Not Found", "ccare06");

                }
            //    /**********
            //  * 
            //  * 
            //  * DP900 upload end 
            //  * 
            //  * 
            //  * 
            //  * **********************************************/

                /*****************************
                 * 
                 * 
                 * cash/ equity file start
                 * 
                 * 
                 * *************************/

                if (File.Exists(@"D:\\RMS\\EquityPOA.xls"))
                {

                    try
                    {
                        app.Visible = false;
                        app.ScreenUpdating = false;
                        app.DisplayAlerts = false;


                        book = app.Workbooks.Open(@"D:\\RMS\\EquityPOA.xls", Missing.Value, Missing.Value, Missing.Value
                                                          , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                         , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                        , Missing.Value, Missing.Value, Missing.Value);
                        sheet = (excelapp.Worksheet)book.Worksheets[1];

                        range = sheet.get_Range("A1", "L11000");
                        values = (object[,])range.Value2;

                        string filedate = values[1, 1].ToString().Substring(40);
                        string dateactual = DateTime.Today.ToString("MMM d yyyy");

                        if (filedate.Equals(dateactual))
                        {

                            dt = new DataTable();
                            for (int j = 1; j <= values.GetLength(1); j++)
                            {
                                dt.Columns.Add(values[2, j].ToString());
                                string str = values[2, j].ToString();
                            }
                            for (int i = 4; i <= values.GetLength(0); i++)
                            {
                                DataRow drw = dt.NewRow();
                                for (int j = 1; j <= values.GetLength(1); j++)
                                {
                                    try
                                    {
                                        string str = values[i, j].ToString();
                                        drw[j - 1] = str;
                                    }
                                    catch (Exception ex)
                                    {
                                        drw[j - 1] = "";
                                        continue;
                                    }
                                }
                                dt.Rows.Add(drw);
                            }

                            int row_no = 0;
                            dt.Columns.Add("Net Risk").ReadOnly = false;

                            DataTable dtnew = new DataTable();
                            for (int i = 0; i < dt.Columns.Count - 1; i++)
                            {
                                string colmname = dt.Columns[i].ColumnName.ToString();
                                dtnew.Columns.Add(colmname);
                            }

                            dtnew.Columns.Add("Net Risk");
                            dtnew.Columns.Add("Sub Broker").ReadOnly = false;
                            while (row_no < dt.Rows.Count)
                            {

                                decimal value = 0;
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select sum(value)  from POA where ClientCode='" + dt.Rows[row_no][1].ToString().Trim() + "' and uploadDate='" + DateTime.Today.ToString() + "'  ";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    string val = dr[0].ToString();
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        value = Convert.ToDecimal(dr[0].ToString());
                                    }

                                }
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select branch from Cust_Client_Master where clientcode='" + dt.Rows[row_no][1].ToString().Trim() + "' and branch!='RETAILKOLH' ";

                                dr = cmd.ExecuteReader();

                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    DataRow drow = dtnew.NewRow();

                                    decimal netrisk = 0;
                                    int last = 0;
                                    for (int i = 0; i < dt.Columns.Count - 1; i++)
                                    {
                                        if (i >= 3)
                                        {
                                            string number = dt.Rows[row_no][i].ToString();

                                            if (!string.IsNullOrEmpty(number.Trim()))
                                            {
                                                if (i == 9)
                                                {
                                                    number = "-" + number;
                                                }
                                                if (i == 11)
                                                {
                                                    number = value.ToString();
                                                }
                                                if (i != 11)
                                                {
                                                    netrisk = netrisk + (Convert.ToDecimal(number) * 100000);
                                                }
                                                else
                                                {
                                                    netrisk = netrisk + Convert.ToDecimal(number);
                                                }
                                            }
                                        }
                                        if (i == 11)
                                        {
                                            drow[i] = value.ToString();
                                        }
                                        else if (i >= 3)
                                        {
                                            drow[i] = (Convert.ToDecimal(dt.Rows[row_no][i].ToString()) * 100000);// result.Tables[0].Rows[row_no][i].ToString();
                                        }
                                        else
                                        {
                                            drow[i] = dt.Rows[row_no][i].ToString();
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(dt.Rows[row_no][4].ToString().Trim()))
                                    {
                                        netrisk = netrisk - (Convert.ToDecimal(dt.Rows[row_no][4].ToString()) * 100000);
                                    }
                                    dt.Rows[row_no][dt.Columns.Count - 1] = netrisk;
                                    drow[dt.Columns.Count - 1] = netrisk;

                                    drow[dtnew.Columns.Count - 1] = netrisk;

                                    dtnew.Rows.Add(drow);
                                }
                                dr.Close();


                                row_no++;

                            }

            //            /***************************
            //             * 
            //             * 
            //             *  //calculation start
            //             * 
            //             * 
            //             * **********/

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Delete    from EqutyDetails where IDate='"+DateTime.Today.ToString("dd-MMM-yyyy")+"'";
                            cmd.ExecuteNonQuery();
                        foreach (DataRow gr in dtnew.Rows)
                        {


                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + gr[1].ToString().Trim() + "' and branch!='RETAILKOLH'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();

                                string family = dr[0].ToString();
                                string branch = dr[1].ToString();
                                string clientname = dr[2].ToString();
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + gr[1].ToString().Trim() + "'";
                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + gr[1].ToString().Trim() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                    cmd.ExecuteNonQuery();
                                }
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "insert into EqutyDetails(ClientCode,LegBal,CashColl,NonCashColl,DebitStock,POToday,ShrtValue,FutPOValue,POAValue,Total,IDate)values(@ClientCode,@LegBal,@CashColl,@NonCashColl,@DebitStock,@POToday,@ShrtValue,@FutPOValue,@POAValue,@Total,'"+DateTime.Today.ToString("dd-MMM-yyyy")+"')";
                                string str = gr[1].ToString().Trim();
                                cmd.Parameters.AddWithValue("ClientCode", gr[1].ToString().Trim());
                                str = gr[3].ToString().Trim();
                                cmd.Parameters.AddWithValue("LegBal", gr[3].ToString().Trim());
                                str = gr[5].ToString().Trim();
                                cmd.Parameters.AddWithValue("CashColl", gr[5].ToString().Trim());
                                str = gr[6].ToString().Trim();
                                cmd.Parameters.AddWithValue("NonCashColl", gr[6].ToString().Trim());
                                str = gr[7].ToString().Trim();
                                cmd.Parameters.AddWithValue("DebitStock", gr[7].ToString().Trim());
                                str = gr[8].ToString().Trim();
                                cmd.Parameters.AddWithValue("POToday", gr[8].ToString().Trim());
                                str = gr[9].ToString().Trim();
                                cmd.Parameters.AddWithValue("ShrtValue", gr[9].ToString().Trim());
                                str = gr[10].ToString().Trim();
                                cmd.Parameters.AddWithValue("FutPOValue", gr[10].ToString().Trim());
                                str = gr[11].ToString().Trim();
                                cmd.Parameters.AddWithValue("POAValue", gr[11].ToString().Trim());
                                str = gr[12].ToString().Trim();
                                cmd.Parameters.AddWithValue("Total", gr[12].ToString().Trim());


                                cmd.ExecuteNonQuery();
                            }

                        }


                        foreach (DataRow gr in dtnew.Rows)
                        {


                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + gr[1].ToString().Trim() + "'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();

                                string family = dr[0].ToString();
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + gr[1].ToString().Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,CASH)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr[1].ToString().Trim() + "','" + family.Trim() + "'," + gr[12].ToString().Trim() + ")";

                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "update INVESTMENTSUMMARY set CASH='" + gr[12].ToString().Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr[1].ToString().Trim() + "'";

                                    cmd.ExecuteNonQuery();

                                }

                            }
                            dr.Close();




                        }
            //            /******************
            //             * For records present in DP900 and DP919 file but not in Equity Net Risk file 
            //             * 
            //             * 
            //             * 
            //             * 
            //             * 
            //             * ***/
                        cmd = conn.CreateCommand();
                       // cmd.CommandText = "Select p.ClientCode from POA p ,EqutyDetails CM WHERE CM.ClientCode != p.ClientCode and CM.IDate='" + DateTime.Today.ToString() + "' and p.uploadDate='" + DateTime.Today.ToString() + "'";

                            // cmd.CommandText = "Select p.ClientCode from POA p   WHERE NOT EXISTS (SELECT CM.ClientCode  FROM EqutyDetails CM  WHERE CM.ClientCode = p.ClientCode and CM.IDate='" + DateTime.Today.ToString() + "') and p.uploadDate='" + DateTime.Today.ToString() + "'";
                        cmd.CommandText = "Select distinct (p.ClientCode) from POA p  where p.uploadDate='" + DateTime.Today.ToString() + "'";
                 
                            dr = cmd.ExecuteReader();
                        dt = new DataTable();
                        if (dr.HasRows)
                        {


                            dt.Load(dr);

                        }
                        dr.Close();

                        foreach (DataRow dtr in dt.Rows)
                        {
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "SELECT CM.ClientCode  FROM EqutyDetails CM  WHERE CM.ClientCode = '" + dtr[0].ToString() + "' and CM.IDate='" + DateTime.Today.ToString() + "'";
                            dr = cmd.ExecuteReader();
                            DataTable dtcliexist = new DataTable();
                            dtcliexist.Load(dr);
                            dr.Close();
                             
                         if(dtcliexist.Rows.Count==0)
                         {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + dtr[0].ToString() + "' and branch!='RETAILKOLH'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();

                                string family = dr[0].ToString();
                                string branch = dr[1].ToString();
                                string clientname = dr[2].ToString();
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + dtr[0].ToString() + "'";
                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + dtr[0].ToString() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                    cmd.ExecuteNonQuery();
                                }

                            } dr.Close();
                        
                        }
                        }



                        foreach (DataRow dtr in dt.Rows)
                        {

                            if (!string.IsNullOrEmpty(dtr[0].ToString()))
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "SELECT CM.ClientCode  FROM EqutyDetails CM  WHERE CM.ClientCode = '" + dtr[0].ToString() + "' and CM.IDate='" + DateTime.Today.ToString() + "'";
                                dr = cmd.ExecuteReader();
                                DataTable dtcliexist = new DataTable();
                                dtcliexist.Load(dr);
                                dr.Close();



                                if (dtcliexist.Rows.Count== 0)
                                {
                        
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select sum(value)  from POA where ClientCode='" + dtr[0].ToString() + "' and uploadDate='" + DateTime.Today.ToString() + "'  ";
                                decimal cash = (decimal)cmd.ExecuteScalar();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + dtr[0].ToString() + "'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    string family = dr[0].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + dtr[0].ToString() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,CASH)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + dtr[0].ToString() + "','" + family.Trim() + "'," + cash + ")";

                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "update INVESTMENTSUMMARY set CASH='" + cash + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + dtr[0].ToString() + "'";

                                        cmd.ExecuteNonQuery();

                                    }
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "insert into EqutyDetails(ClientCode,LegBal,CashColl,NonCashColl,DebitStock,POToday,ShrtValue,FutPOValue,POAValue,Total,IDate)values(@ClientCode,@LegBal,@CashColl,@NonCashColl,@DebitStock,@POToday,@ShrtValue,@FutPOValue,@POAValue,@Total,'" + DateTime.Today.ToString("dd-MMM-yyyy") + "')";

                                    cmd.Parameters.AddWithValue("ClientCode", dtr[0].ToString());
                                    
                                    cmd.Parameters.AddWithValue("LegBal", 0.ToString().Trim());
                                   
                                    cmd.Parameters.AddWithValue("CashColl", 0.ToString().Trim());
                                   
                                    cmd.Parameters.AddWithValue("NonCashColl",0.ToString().Trim());
                                    
                                    cmd.Parameters.AddWithValue("DebitStock", 0.ToString().Trim());
                                    
                                    cmd.Parameters.AddWithValue("POToday", 0.ToString().Trim());
                                     
                                    cmd.Parameters.AddWithValue("ShrtValue", 0.ToString().Trim());
                                     
                                    cmd.Parameters.AddWithValue("FutPOValue", 0.ToString().Trim());

                                    cmd.Parameters.AddWithValue("POAValue", cash.ToString().Trim());

                                    cmd.Parameters.AddWithValue("Total", cash.ToString().Trim());
                                    cmd.ExecuteNonQuery();
                                
                                }

                            }
                            dr.Close();
                        }
                            dr.Close();
                        }



                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('EquityPOA.xls','" + DateTime.Today.ToString() + "')";

                        cmd.ExecuteNonQuery();




                        //calculation end




                        mail("Equity POA", "File Uploaded Successfully", " ");



                    }
                    else
                    {
                        mail("Equity POA", "File Wrong Date ", "bo02");
                    }




                }catch(Exception ex)
            {
                mail("Equity POA", "File Not  Uploaded Successfully","bo02");
                }
                }
                else
                {
                    mail("Equity POA", "File Not Found","bo02");
                
                }


                /*****************************
                 * 
                 * cash/ Equity file end
                 * 
                 * 
                 * 
                 * *****/
                /************
                 * 
                 * FNO start
                 * 
                 * ******************/

                if (File.Exists(@"D:\\RMS\\FNO.csv"))
                {
                    try
                    {

                        StreamReader sr = new StreamReader(@"D:\\RMS\\FNO.csv");

                        string strline = "";
                        string[] _values = null;
                        strline = sr.ReadLine();
                        strline = sr.ReadLine();
                        strline = sr.ReadLine();
                        dt = new DataTable();
                        dt.Columns.Add("ClientCode");
                        dt.Columns.Add("ClientName");
                        dt.Columns.Add("Cash");
                        dt.Columns.Add("A_HairCut");
                        dt.Columns.Add("LedgerBill");
                        dt.Columns.Add("ExchangeMargin");
                        dt.Columns.Add("NetRisk");
                        int dateflag = 0;
                        while (strline != null)
                        {
                            _values = strline.Split(',');
                            string datefile = _values[0].ToString();

                            string actualdate = DateTime.Today.AddDays(-1).ToString("MMM  d yyyy");

                            if (DateTime.Today.AddDays(0).Day < 10)
                            {
                                actualdate = DateTime.Today.AddDays(-1).ToString("MMM  d yyyy");
                            }
                            else
                            {
                                actualdate = DateTime.Today.AddDays(-1).ToString("MMM d yyyy");
                            }

                            if (datefile.Equals(actualdate))
                            {
                                dateflag = 0;


                                DataRow drow = dt.NewRow();
                                drow["ClientCode"] = _values[3];
                                drow["ClientName"] = _values[4];
                                drow["Cash"] = _values[7];
                                drow["A_HairCut"] = _values[9];
                                drow["LedgerBill"] = _values[14];
                                drow["ExchangeMargin"] = _values[23];
                                //drow["NetRisk"] = Convert.ToDecimal(_values[7]) + Convert.ToDecimal(_values[8]) + Convert.ToDecimal(_values[14]) - Convert.ToDecimal(_values[24]);
                                //on 8 8 2015
                                drow["NetRisk"] = Convert.ToDecimal(_values[7]) + Convert.ToDecimal(_values[9]) + Convert.ToDecimal(_values[14]) + Convert.ToDecimal(_values[23]);

                                dt.Rows.Add(drow);
                                strline = sr.ReadLine();
                            }
                            else
                            {
                                dateflag = 1;
                                break;
                            }



                        }
                        if (dateflag == 0)
                        {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Delete    from FNODetails where IDate='"+DateTime.Today.ToString("dd-MMM-yyyy")+"'";
                            cmd.ExecuteNonQuery();



                            foreach (DataRow gr in dt.Rows)
                            {


                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + gr[0].ToString().Trim() + "' and branch!='RETAILKOLH'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    string family = dr[0].ToString();
                                    string branch = dr[1].ToString();
                                    string clientname = dr[2].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + gr[0].ToString().Trim() + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + gr[0].ToString().Trim() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                        cmd.ExecuteNonQuery();
                                    }
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "insert into FNODetails(ClientCode,Cash,Non_Cash_A_HairCut,Leg_Bal_With_Bill,Total_Margin_Reporting,FNO_Total,IDate)values(@ClientCode,@Cash,@Non_Cash_A_HairCut,@Leg_Bal_With_Bill,@Total_Margin_Reporting,@FNO_Total,'" + DateTime.Today.ToString("dd-MMM-yyyy") + "')";

                                    cmd.Parameters.AddWithValue("ClientCode", gr[0].ToString().Trim());
                                    cmd.Parameters.AddWithValue("Cash", gr[2].ToString().Trim());
                                    cmd.Parameters.AddWithValue("Non_Cash_A_HairCut", gr[3].ToString().Trim());
                                    cmd.Parameters.AddWithValue("Leg_Bal_With_Bill", gr[4].ToString().Trim());
                                    cmd.Parameters.AddWithValue("Total_Margin_Reporting", gr[5].ToString().Trim());
                                    cmd.Parameters.AddWithValue("FNO_Total", gr[6].ToString().Trim());



                                    cmd.ExecuteNonQuery();
                                    dr.Close();
                                }
                                dr.Close();
                            }
                            foreach (DataRow gr in dt.Rows)
                            {


                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + gr[0].ToString().Trim() + "'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    string family = dr[0].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + gr[0].ToString().Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,FNO)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr[0].ToString().Trim() + "','" + family.Trim() + "'," + gr[6].ToString().Trim() + ")";

                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "update INVESTMENTSUMMARY set FNO='" + gr[6].ToString().Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr[0].ToString().Trim() + "'";

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                dr.Close();

                            }


                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('FNO.csv','" + DateTime.Today.ToString() + "')";

                            cmd.ExecuteNonQuery();

                            mail("FNO", "File Uploaded Successfully", " ");
                        }
                        else
                        {
                            mail("FNO", "Wrong Date File", "bo02");
                        }
                    }
                    catch (Exception ex)
                    {
                        mail("FNO", "File Not  Uploaded Successfully", "bo02");
                    }
                }
                else
                {
                    mail("FNO", "File Not Found", "bo02");

                }
                //                 /************
                //              * 
                //              * FNO end
                //              * 
                //              * ******************/
                /****************
                 * 
                 * Margin funding start
                 * 
            //     * *****************/
                if (File.Exists(@"D:\\RMS\\MarginFunding.xls"))
                {

                    try
                    {
                        app.Visible = false;
                        app.ScreenUpdating = false;
                        app.DisplayAlerts = false;


                        book = app.Workbooks.Open(@"D:\\RMS\\MarginFunding.xls", Missing.Value, Missing.Value, Missing.Value
                                                          , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                         , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                        , Missing.Value, Missing.Value, Missing.Value);
                        sheet = (excelapp.Worksheet)book.Worksheets[1];

                        range = sheet.get_Range("A1", "S1000");
                        values = (object[,])range.Value2;

                        dt = new DataTable();
                        int k = 0;
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            try
                            {
                                dt.Columns.Add(values[1, j].ToString());
                                k++;
                            }
                            catch (Exception ex)
                            {
                                break;
                            }

                        }
                        int dateflag = 0;
                        string filedate = DateTime.FromOADate(Convert.ToDouble(values[2, 19].ToString())).ToString("dd-MMM-yy");
                        string dateactual = DateTime.Today.AddDays(-1).ToString("dd-MMM-yy");

                        if (filedate.Equals(dateactual))
                        {
                            dateflag = 0;
                        }
                        else
                        {
                            dateflag = 1;
                        }
                        if (dateflag == 0)
                        {

                        for (int i = 2; i <= values.GetLength(0); i++)
                        {
                            
                            if (filedate.Equals(dateactual))
                            {
                                dateflag = 0;
                                DataRow drw = dt.NewRow();
                                for (int j = 1; j <= k; j++)
                                {
                                    try
                                    {
                                        string str = values[i, j].ToString();
                                        drw[j - 1] = str;
                                    }
                                    catch (Exception ex)
                                    {
                                        drw[j - 1] = "";

                                    }
                                }
                                dt.Rows.Add(drw);
                            }
                            else
                            {
                                dateflag = 1;
                                break;
                            }
                        }
                      
                            row_no = 0;
                            dt.Columns.Add("Net Risk").ReadOnly = false;

                            dtnew = new DataTable();
                            for (int i = 0; i < dt.Columns.Count - 1; i++)
                            {
                                try
                                {
                                    string colmname = dt.Columns[i].ColumnName;
                                    dtnew.Columns.Add(colmname);
                                }
                                catch (Exception e)
                                {

                                }
                            }

                            dtnew.Columns.Add("Net Risk");

                            while (row_no < dt.Rows.Count)
                            {


                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select branch from Cust_Client_Master where clientcode='" + dt.Rows[row_no][0].ToString().Trim() + "' and branch!='RETAILKOLH' ";

                                dr = cmd.ExecuteReader();

                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    DataRow drow = dtnew.NewRow();

                                    decimal netrisk = 0;
                                    int last = 0;
                                    for (int i = 0; i < dt.Columns.Count - 1; i++)
                                    {
                                        if (i >= 2 && i <= 7)
                                        {
                                            try
                                            {
                                                string number = dt.Rows[row_no][i].ToString();

                                                if (!string.IsNullOrEmpty(number.Trim()))
                                                {
                                                    if (i == 7)
                                                    {
                                                        netrisk = netrisk + Convert.ToDecimal(number);
                                                    }
                                                    else if (i == 2 || i == 3 || i == 4)
                                                    {
                                                        netrisk = netrisk + Convert.ToDecimal(number);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        if (i >= 2)
                                        {
                                            drow[i] = dt.Rows[row_no][i].ToString();
                                        }
                                        else
                                        {
                                            drow[i] = dt.Rows[row_no][i].ToString();
                                        }
                                    }
                                    dt.Rows[row_no][dt.Columns.Count - 1] = netrisk;
                                    drow[dt.Columns.Count - 1] = netrisk;

                                    drow[dtnew.Columns.Count - 1] = netrisk;
                                    dtnew.Rows.Add(drow);
                                }
                                dr.Close();


                                row_no++;

                            }

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Delete    from MarginFundingDetails where IDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                            cmd.ExecuteNonQuery();


                            foreach (DataRow gr in dtnew.Rows)
                            {


                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select family,branch,clientname from Cust_Client_Master where clientcode='" + gr[0].ToString().Trim() + "' and branch!='RETAILKOLH'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    string family = dr[0].ToString();
                                    string branch = dr[1].ToString();
                                    string clientname = dr[2].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + gr[0].ToString().Trim() + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + gr[0].ToString().Trim() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                        cmd.ExecuteNonQuery();
                                    }
                                    dr.Close();

                                    decimal value = 0;
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "Select sum(value)  from POA where ClientCode='" + gr[0].ToString().Trim() + "' and uploadDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and type='DP919' ";
                                    dr = cmd.ExecuteReader();
                                    if (dr.HasRows)
                                    {
                                        dr.Read();
                                        string val = dr[0].ToString();
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            value = Convert.ToDecimal(dr[0].ToString());
                                          gr[19]=  (Convert.ToDecimal( gr[19].ToString().Trim())+ value).ToString();
                                        }

                                    }
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "insert into MarginFundingDetails(ClientCode,UnApprovedMktValue,ApprovedMktValue,Odd_LotMktValue,LedgerBal,NetRisk,IDate,DP_Valuation)values(@ClientCode,@UnApprovedMktValue,@ApprovedMktValue,@Odd_LotMktValue,@LedgerBal,@NetRisk,'" + DateTime.Today.ToString("dd-MMM-yyyy") + "',@DP_Valuation)";

                                    cmd.Parameters.AddWithValue("ClientCode", gr[0].ToString().Trim());
                                    cmd.Parameters.AddWithValue("UnApprovedMktValue", gr[3].ToString().Trim());
                                    cmd.Parameters.AddWithValue("ApprovedMktValue", gr[2].ToString().Trim());
                                    cmd.Parameters.AddWithValue("Odd_LotMktValue", gr[4].ToString().Trim());
                                    cmd.Parameters.AddWithValue("LedgerBal", gr[7].ToString().Trim());
                                    cmd.Parameters.AddWithValue("NetRisk", gr[19].ToString());

                                    cmd.Parameters.AddWithValue("DP_Valuation", value.ToString());

                                    cmd.ExecuteNonQuery();
                                }

                            }





                            foreach (DataRow gr in dtnew.Rows)
                            {


                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select FamilyCode from ClientMaster where ClientCode='" + gr[0].ToString().Trim() + "'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    string family = dr[0].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + gr[0].ToString().Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,CASH)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr[0].ToString().Trim() + "','" + family.Trim() + "'," + gr[19].ToString().Trim() + ")";

                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "update INVESTMENTSUMMARY set CASH='" + gr[19].ToString().Trim() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + gr[0].ToString().Trim() + "'";

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                dr.Close();

                            }


                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('MarginFunding.xls','" + DateTime.Today.ToString() + "')";

                            cmd.ExecuteNonQuery();

                            mail("Margin Funding", "File Uploaded Successfully", " ");

                        }
                        else
                        {
                            mail("Margin Funding", "Wrong Date File", "bo02");
                        }
                    }
                    catch (Exception ex)
                    {
                        mail("Margin Funding", "File Not  Uploaded Successfully", "bo02");
                    }
                }
                else
                {
                    mail("Margin Funding", "File Not Found", "bo02");

                }

                /****************
                * 
                * Margin funding end
                * 
                * *****************/
                /********************
                 * 
                 * Mutual Fund AUM start
                 * 
                 * ***************/
                if (File.Exists(@"D:\\MF\\MF.xls"))
                {
                   
                    try
                    {
                    book = app.Workbooks.Open(@"D:\\MF\\MF.xls", Missing.Value, Missing.Value, Missing.Value
                                                     , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                    , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                   , Missing.Value, Missing.Value, Missing.Value);
                    sheet = (excelapp.Worksheet)book.Worksheets[1];

                    range = sheet.get_Range("A1", "Z11000");

                    values = (object[,])range.Value2;

                    string filedate = values[2, 1].ToString().Substring(12);
                    string actualdate = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
                    int dateflag = 0;
                     
                    if (filedate.Equals(actualdate))
                    {
                        dateflag = 0;
                        dt = new DataTable();
                        for (int j = 1; j <= values.GetLength(1); j++)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(values[3, j].ToString()))
                                {
                                    dt.Columns.Add(values[3, j].ToString());
                                }
                            }
                            catch (Exception eee)
                            { 
                            
                            }
                        }

                        for (int i = 4; i <= values.GetLength(0); i++)
                        {
                            DataRow drw = dt.NewRow();

                            for (int j = 1; j <= values.GetLength(1); j++)
                            {
                                try
                                {
                                    string str = values[i, j].ToString();
                                    drw[j - 1] = str;
                                }
                                catch (Exception ex)
                                {
                                   // drw[j - 1] = "";

                                }
                            }

                            dt.Rows.Add(drw);
                        }

                        DataColumn dcol = new DataColumn("ClientCodeFromEquityDB", typeof(System.String));

                        dt.Columns.Add(dcol);
                        dcol = new DataColumn("Branch", typeof(System.String));
                        dt.Columns.Add(dcol);
                        dcol = new DataColumn("ClientCodeFromMFDB", typeof(System.String));
                        dt.Columns.Add(dcol);
                        dcol = new DataColumn("ClientAliasFromMFDB", typeof(System.String));
                        dt.Columns.Add(dcol);

                        foreach (DataRow gr in dt.Rows)
                        {
                            try
                            {
                                string pan = gr[10].ToString().Trim();
                                if (!string.IsNullOrEmpty(pan.Trim()))
                                {
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select clientcode,subbrokercode  from Cust_Client_Master   where panno='" + pan + "' ";//and branch!='RETAILKOLH'
                                    dr = cmd.ExecuteReader();
                                    if (dr.HasRows)
                                    {
                                        dr.Read();
                                        gr[22] = dr[0].ToString();
                                        gr[23] = dr[1].ToString();
                                    }
                                    else
                                    {
                                    }
                                    dr.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                dr.Close();
                            }
                        }
                        foreach (DataRow gr in dt.Rows)
                        {
                            try
                            {
                                string name = gr[0].ToString().Trim();
                                if (!string.IsNullOrEmpty(name.Trim()))
                                {
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select equitycode1,clientalias   from MF_Client_Master   where clientname='" + name + "' ";//and branch!='RETAILKOLH'
                                    dr = cmd.ExecuteReader();
                                    if (dr.HasRows)
                                    {
                                        dr.Read();
                                        gr[24] = dr[0].ToString();
                                        gr[25] = dr[1].ToString();
                                    }
                                    else
                                    {
                                    }
                                    dr.Close();
                                }
                            }
                            catch (Exception EX)
                            {
                                dr.Close();
                            }
                        }
                        cmd = conn.CreateCommand();
                        cmd.CommandText = " delete from MFDetails where IDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";//(ClientCode ,Value)values('" + toconsider + "'," + total + ")";
                        cmd.ExecuteNonQuery();


                        foreach (DataRow gr in dt.Rows)
                        {
                            try
                            {
                                string equitycode = Convert.ToString(gr[22]);

                                string mfequitycode = Convert.ToString(gr[24]);
                                string clientaliascode = Convert.ToString(gr[25]);
                                string total = Convert.ToString(gr[20]);// gr[19].ToString().Replace("", "0").Trim();
                                string name = Convert.ToString(gr[0]);// gr[0].ToString().Replace("", "0").Trim();
                                string toconsider = "";

                                if (!(string.IsNullOrEmpty(equitycode.Trim()) && string.IsNullOrEmpty(mfequitycode.Trim())))
                                {



                                    if (equitycode.Equals(mfequitycode))
                                    {
                                        toconsider = equitycode;
                                    }
                                    else if (string.IsNullOrEmpty(equitycode.Trim()) && (!string.IsNullOrEmpty(mfequitycode.Trim())))
                                    {
                                        toconsider = mfequitycode;
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "select * from Cust_Client_Master where clientcode='" + mfequitycode + "'";
                                        dr = cmd.ExecuteReader();
                                        if (!dr.HasRows)
                                        {
                                            toconsider = clientaliascode;
                                        }
                                        dr.Close();

                                    }
                                    else if (string.IsNullOrEmpty(mfequitycode.Trim()) && (!string.IsNullOrEmpty(equitycode.Trim())))
                                    {
                                        toconsider = equitycode;


                                    }
                                    else if ((!string.IsNullOrEmpty(mfequitycode.Trim())) && (!string.IsNullOrEmpty(equitycode.Trim())))
                                    {
                                        toconsider = equitycode;


                                    }





                                }
                                else if (string.IsNullOrEmpty(equitycode.Trim()) && string.IsNullOrEmpty(mfequitycode.Trim()))
                                {
                                    toconsider = clientaliascode;
                                }
                                if (!string.IsNullOrEmpty(toconsider))
                                {
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "insert into  MFDetails(ClientCode ,Value,ClientName,IDate)values('" + toconsider + "'," + total + ",'" + name + "','" + DateTime.Today.ToString("dd-MMM-yyyy") + "')";
                                    cmd.ExecuteNonQuery();

                                }
                            }
                            catch (Exception e)
                            {
                            }

                        }
                        cmd = conn.CreateCommand();
                        cmd.CommandText = " select ClientCode,sum(Value) from MFDetails where IDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' group by ClientCode";//(ClientCode ,Value)values('" + toconsider + "'," + total + ")";
                        dr = cmd.ExecuteReader();
                        DataTable dt1 = new DataTable();
                        if (dr.HasRows)
                        {
                            dt1.Load(dr);
                        }
                        foreach (DataRow drw in dt1.Rows)
                        {
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select family,clientcode from Cust_Client_Master where clientcode='" + drw[0].ToString() + "' and branch!='RETAILKOLH'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();

                                string family = dr[0].ToString();
                                string clientcode = dr[1].ToString();
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,MF)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + drw[1].ToString() + ")";

                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "update INVESTMENTSUMMARY set MF='" + drw[1].ToString() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                    cmd.ExecuteNonQuery();

                                }
                            }
                            else
                            {
                                dr.Close();
                                //*******************//
                                if (!string.IsNullOrEmpty(drw[0].ToString().Trim()))
                                {
                                    string family = "";//= dr[0].ToString();
                                    string branch = "";//= dr[1].ToString();
                                    string clientname = "";// = dr[2].ToString();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "Select  groupalias,branch,clientname from MF_Client_Master where clientalias='" + drw[0].ToString() + "' ";

                                    dr = cmd.ExecuteReader();
                                    if (dr.HasRows)
                                    {
                                        dr.Read();

                                        family = dr[0].ToString();
                                        branch = dr[1].ToString();
                                        clientname = dr[2].ToString();
                                        dr.Close();

                                        if (!string.IsNullOrEmpty(branch.Trim()))
                                        {
                                            cmd = conn.CreateCommand();
                                            cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + drw[0].ToString() + "'";
                                            dr = cmd.ExecuteReader();
                                            if (!dr.HasRows)
                                            {
                                                dr.Close();
                                                cmd = conn.CreateCommand();
                                                cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + drw[0].ToString() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                                cmd.ExecuteNonQuery();
                                            }

                                        }
                                        dr.Close();
                                    }


                                    //*********************************/
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "Select  FamilyCode from ClientMaster where ClientCode='" + drw[0].ToString() + "' and branch!='RETAILKOLH'";
                                    dr = cmd.ExecuteReader();
                                    string clientcode = drw[0].ToString();
                                    if (dr.HasRows)
                                    {
                                        dr.Read();
                                        family = dr[0].ToString();
                                    }

                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,MF)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + drw[1].ToString() + ")";

                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "update INVESTMENTSUMMARY set MF='" + drw[1].ToString() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }
                            dr.Close();

                        }

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('MF.xls','" + DateTime.Today.ToString() + "')";

                        cmd.ExecuteNonQuery();

                        mail("MF AUM", "File Uploaded Successfully", " ");
                    }
                    else
                    {
                        dateflag = 1;
                        mail("MF AUM", "Wrong Date File", "ccare02");
                    }
                }catch(Exception ex)
            {
                mail("MF AUM", "File Not  Uploaded Successfully","ccare02");
                }
                }
                else
                {
                    mail("MF AUM", "File Not Found","ccare02");
                
                }


                    /************* * 
                     * 
                     * Mutual Fund AUM end
                     * 
                     * ********************/


                /********************
            * 
            * FD start
            * 
            * ***************/
                if (File.Exists(@"D:\\MF\\FD.xls"))
                {

                    try
                    {
                        book = app.Workbooks.Open(@"D:\\MF\\FD.xls", Missing.Value, Missing.Value, Missing.Value
                                                         , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                        , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                       , Missing.Value, Missing.Value, Missing.Value);
                        sheet = (excelapp.Worksheet)book.Worksheets[1];

                        range = sheet.get_Range("A1", "Z11000");

                        values = (object[,])range.Value2;

                        string filedate = values[2, 1].ToString().Substring(12);
                        string actualdate = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
                        int dateflag = 0;

                        if (filedate.Equals(actualdate))
                        {
                            dateflag = 0;
                            dt = new DataTable();
                            for (int j = 1; j <= values.GetLength(1); j++)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(values[3, j].ToString()))
                                    {
                                        dt.Columns.Add(values[3, j].ToString());
                                    }
                                }
                                catch (Exception eee)
                                {

                                }
                            }

                            for (int i = 4; i <= values.GetLength(0); i++)
                            {
                                DataRow drw = dt.NewRow();

                                for (int j = 1; j <= values.GetLength(1); j++)
                                {
                                    try
                                    {
                                        string str = values[i, j].ToString();
                                        drw[j - 1] = str;
                                    }
                                    catch (Exception ex)
                                    {
                                        // drw[j - 1] = "";

                                    }
                                }

                                dt.Rows.Add(drw);
                            }

                            DataColumn dcol = new DataColumn("ClientCodeFromEquityDB", typeof(System.String));

                            dt.Columns.Add(dcol);
                            dcol = new DataColumn("Branch", typeof(System.String));
                            dt.Columns.Add(dcol);
                            dcol = new DataColumn("ClientCodeFromMFDB", typeof(System.String));
                            dt.Columns.Add(dcol);
                            dcol = new DataColumn("ClientAliasFromMFDB", typeof(System.String));
                            dt.Columns.Add(dcol);

                            foreach (DataRow gr in dt.Rows)
                            {
                                try
                                {
                                    string pan = gr[10].ToString().Trim();
                                    if (!string.IsNullOrEmpty(pan.Trim()))
                                    {
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "select clientcode,subbrokercode  from Cust_Client_Master   where panno='" + pan + "' ";//and branch!='RETAILKOLH'
                                        dr = cmd.ExecuteReader();
                                        if (dr.HasRows)
                                        {
                                            dr.Read();
                                            gr[22] = dr[0].ToString();
                                            gr[23] = dr[1].ToString();
                                        }
                                        else
                                        {
                                        }
                                        dr.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    dr.Close();
                                }
                            }
                            foreach (DataRow gr in dt.Rows)
                            {
                                try
                                {
                                    string name = gr[0].ToString().Trim();
                                    if (!string.IsNullOrEmpty(name.Trim()))
                                    {
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "select equitycode1,clientalias   from MF_Client_Master   where clientname='" + name + "' ";//and branch!='RETAILKOLH'
                                        dr = cmd.ExecuteReader();
                                        if (dr.HasRows)
                                        {
                                            dr.Read();
                                            gr[24] = dr[0].ToString();
                                            gr[25] = dr[1].ToString();
                                        }
                                        else
                                        {
                                        }
                                        dr.Close();
                                    }
                                }
                                catch (Exception EX)
                                {
                                    dr.Close();
                                }
                            }
                            cmd = conn.CreateCommand();
                            cmd.CommandText = " delete from FDDetails where IDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";//(ClientCode ,Value)values('" + toconsider + "'," + total + ")";
                            cmd.ExecuteNonQuery();


                            foreach (DataRow gr in dt.Rows)
                            {
                                try
                                {
                                    string equitycode = Convert.ToString(gr[22]);

                                    string mfequitycode = Convert.ToString(gr[24]);
                                    string clientaliascode = Convert.ToString(gr[25]);
                                    string total = Convert.ToString(gr[16]);// gr[19].ToString().Replace("", "0").Trim();
                                    string name = Convert.ToString(gr[0]);// gr[0].ToString().Replace("", "0").Trim();
                                    string toconsider = "";

                                    if (!(string.IsNullOrEmpty(equitycode.Trim()) && string.IsNullOrEmpty(mfequitycode.Trim())))
                                    {



                                        if (equitycode.Equals(mfequitycode))
                                        {
                                            toconsider = equitycode;
                                        }
                                        else if (string.IsNullOrEmpty(equitycode.Trim()) && (!string.IsNullOrEmpty(mfequitycode.Trim())))
                                        {
                                            toconsider = mfequitycode;
                                            cmd = conn.CreateCommand();
                                            cmd.CommandText = "select * from Cust_Client_Master where clientcode='" + mfequitycode + "'";
                                            dr = cmd.ExecuteReader();
                                            if (!dr.HasRows)
                                            {
                                                toconsider = clientaliascode;
                                            }
                                            dr.Close();

                                        }
                                        else if (string.IsNullOrEmpty(mfequitycode.Trim()) && (!string.IsNullOrEmpty(equitycode.Trim())))
                                        {
                                            toconsider = equitycode;


                                        }
                                        else if ((!string.IsNullOrEmpty(mfequitycode.Trim())) && (!string.IsNullOrEmpty(equitycode.Trim())))
                                        {
                                            toconsider = equitycode;


                                        }





                                    }
                                    else if (string.IsNullOrEmpty(equitycode.Trim()) && string.IsNullOrEmpty(mfequitycode.Trim()))
                                    {
                                        toconsider = clientaliascode;
                                    }
                                    if (!string.IsNullOrEmpty(toconsider))
                                    {
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "insert into  FDDetails(ClientCode ,Value,ClientName,IDate)values('" + toconsider + "'," + total + ",'" + name + "','" + DateTime.Today.ToString("dd-MMM-yyyy") + "')";
                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                catch (Exception e)
                                {
                                }

                            }
                            cmd = conn.CreateCommand();
                            cmd.CommandText = " select ClientCode,sum(Value) from FDDetails where IDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' group by ClientCode";//(ClientCode ,Value)values('" + toconsider + "'," + total + ")";
                            dr = cmd.ExecuteReader();
                            DataTable dt1 = new DataTable();
                            if (dr.HasRows)
                            {
                                dt1.Load(dr);
                            }
                            foreach (DataRow drw in dt1.Rows)
                            {
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select family,clientcode from Cust_Client_Master where clientcode='" + drw[0].ToString() + "' and branch!='RETAILKOLH'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();

                                    string family = dr[0].ToString();
                                    string clientcode = dr[1].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,FD)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + drw[1].ToString() + ")";

                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "update INVESTMENTSUMMARY set FD='" + drw[1].ToString() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                        cmd.ExecuteNonQuery();

                                    }
                                }
                                else
                                {
                                    dr.Close();
                                    //*******************//
                                    if (!string.IsNullOrEmpty(drw[0].ToString().Trim()))
                                    {
                                        string family = "";//= dr[0].ToString();
                                        string branch = "";//= dr[1].ToString();
                                        string clientname = "";// = dr[2].ToString();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "Select  groupalias,branch,clientname from MF_Client_Master where clientalias='" + drw[0].ToString() + "' ";

                                        dr = cmd.ExecuteReader();
                                        if (dr.HasRows)
                                        {
                                            dr.Read();

                                            family = dr[0].ToString();
                                            branch = dr[1].ToString();
                                            clientname = dr[2].ToString();
                                            dr.Close();

                                            if (!string.IsNullOrEmpty(branch.Trim()))
                                            {
                                                cmd = conn.CreateCommand();
                                                cmd.CommandText = "select ClientCode from ClientMaster where ClientCode='" + drw[0].ToString() + "'";
                                                dr = cmd.ExecuteReader();
                                                if (!dr.HasRows)
                                                {
                                                    dr.Close();
                                                    cmd = conn.CreateCommand();
                                                    cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + drw[0].ToString() + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                                    cmd.ExecuteNonQuery();
                                                }

                                            }
                                            dr.Close();
                                        }


                                        //*********************************/
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "Select  FamilyCode from ClientMaster where ClientCode='" + drw[0].ToString() + "' and branch!='RETAILKOLH'";
                                        dr = cmd.ExecuteReader();
                                        string clientcode = drw[0].ToString();
                                        if (dr.HasRows)
                                        {
                                            dr.Read();
                                            family = dr[0].ToString();
                                        }

                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                        dr = cmd.ExecuteReader();
                                        if (!dr.HasRows)
                                        {
                                            dr.Close();
                                            cmd = conn.CreateCommand();
                                            cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,FD)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + drw[1].ToString() + ")";

                                            cmd.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            dr.Close();
                                            cmd = conn.CreateCommand();
                                            cmd.CommandText = "update INVESTMENTSUMMARY set FD='" + drw[1].ToString() + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                            cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                                dr.Close();

                            }

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('FD.xls','" + DateTime.Today.ToString() + "')";

                            cmd.ExecuteNonQuery();

                            mail("FD", "File Uploaded Successfully", " ");
                        }
                        else
                        {
                            dateflag = 1;
                            mail("FD", "Wrong Date File", "ccare02");
                        }
                    }
                    catch (Exception ex)
                    {
                        mail("FD", "File Not  Uploaded Successfully", "ccare02");
                    }
                }
                else
                {
                    mail("FD", "File Not Found", "ccare02");

                }


                /************* * 
                 * 
                 * FD end
                 * 
                 * ********************/





































                /************
                 * 
                 * 
                 * ****PMS start****
                 * 
                 * 
                 * 
                 * ************/

                if (File.Exists(@"D:\\CC\\PMS.xls"))
                {
                    try
                    {
                    app.Visible = false;
                    app.ScreenUpdating = false;
                    app.DisplayAlerts = false;


                    book = app.Workbooks.Open(@"D:\\CC\\PMS.xls", Missing.Value, Missing.Value, Missing.Value
                                                      , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                     , Missing.Value, Missing.Value, Missing.Value, Missing.Value
                                                    , Missing.Value, Missing.Value, Missing.Value);
                    sheet = (excelapp.Worksheet)book.Worksheets[1];

                    range = sheet.get_Range("A1", "I11000");

                    values = (object[,])range.Value2;


                    dt = new DataTable();
                    for (int j = 1; j <= values.GetLength(1); j++)
                    {
                        dt.Columns.Add(values[1, j].ToString());


                    }

                    for (int i = 2; i <= values.GetLength(0); i++)
                    {
                        try
                        {
                            DataRow drw = dt.NewRow();

                            for (int j = 1; j <= values.GetLength(1); j++)
                            {
                                try
                                {
                                    string str = values[i, j].ToString();
                                    drw[j - 1] = str;
                                }
                                catch (Exception ex)
                                {
                                    drw[j - 1] = "";

                                }
                            }
                            if (!string.IsNullOrEmpty(values[i, 1].ToString()))
                            {
                                dt.Rows.Add(drw);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }


                DataColumn     dcol = new DataColumn("ClientCode", typeof(System.String));

                    dt.Columns.Add(dcol);
                    dcol = new DataColumn("Branch", typeof(System.String));
                    dt.Columns.Add(dcol);
                    // Bind the data to the GridView
                    //dt.DataSource = objDataSet.Tables[0].DefaultView;
                    //GridView1.DataBind();
                    //BoundField colmn = new BoundField();
                    //colmn.HeaderText = "xyz";
                    //GridView1.Columns.Add(colmn);
                    foreach (DataRow gr in dt.Rows)
                    {
                        string pan = gr[7].ToString().Trim();
                        if (!string.IsNullOrEmpty(pan))
                        {
                            //conn.Open();
                            try
                            {
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select clientcode,subbrokercode  from Cust_Client_Master   where panno='" + pan + "' ";//and branch!='RETAILKOLH'
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    gr[9] = dr[0].ToString();
                                    gr[10] = dr[1].ToString();
                                }
                                else
                                {
                                    // gr.Visible = false;
                                }
                                dr.Close();
                            }
                            catch (Exception ex)
                            {
                                dr.Close();
                            }
                        }
                    }
                    /***********************/
                    Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
                        //PAN No and Value
                    // conn.Open();
                    foreach (DataRow gr in dt.Rows)
                    {
                        try
                        {
                            string valuation = gr[8].ToString();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select PANNO from PMSDetails where PMSCode='" + gr[1].ToString().Trim() + "' and PANNO='" + gr[7].ToString().Trim() + "' and Scheme='" + gr[3].ToString().Trim() + "' and PMSDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                            dr = cmd.ExecuteReader();
                            if (!dr.HasRows)
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();

                                cmd.CommandText = "INSERT   INTO PMSDetails(PMSDate,PANNO,Scheme,Valuation,PMSCode)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + gr[7].ToString().Trim() + "','" + gr[3].ToString().Trim() + "'," + valuation + ",'" + gr[1].ToString().Trim() + "')";
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "update PMSDetails set Valuation=" + valuation + " where PMSCode='" + gr[1].ToString().Trim() + "' and PMSDate='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and PANNO='" + gr[7].ToString().Trim() + "' and Scheme='" + gr[3].ToString().Trim() + "'";

                                cmd.ExecuteNonQuery();

                            }
                            // See whether Dictionary contains this string.
                            if (dictionary.ContainsKey(gr[7].ToString().Trim()))
                            {
                                decimal value = Convert.ToDecimal(dictionary[gr[7].ToString().Trim()]);
                                value = value + Convert.ToDecimal(valuation);

                                dictionary[gr[7].ToString().Trim()] = value;
                            }

                            // See whether Dictionary contains this string.
                            if (!dictionary.ContainsKey(gr[7].ToString().Trim()))
                            {
                                dictionary.Add(gr[7].ToString().Trim(), Convert.ToDecimal(gr[8].ToString()));
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }


                    foreach (KeyValuePair<string, decimal> item in dictionary)
                    {

                        cmd = conn.CreateCommand();
                        cmd.CommandText = "Select family,clientcode,branch,clientname from Cust_Client_Master where panno='" + item.Key + "' and branch!='RETAILKOLH'";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();

                            string family = dr[0].ToString();
                            string clientcode = dr[1].ToString();
                            string branch = dr[2].ToString();
                            string clientname = dr[3].ToString();
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select ClientCode,FamilyCode from ClientMaster where ClientCode='" + clientcode + "'";
                            dr = cmd.ExecuteReader();
                            if (!dr.HasRows)
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + clientcode + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                dr.Read();
                                family = dr[1].ToString();
                                dr.Close();
                            }
                            dr.Close();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                            dr = cmd.ExecuteReader();
                            if (!dr.HasRows)
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,PMS)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + item.Value + ")";

                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "update INVESTMENTSUMMARY set PMS='" + item.Value + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                cmd.ExecuteNonQuery();

                            }
                        }
                        else
                        {


                            dr.Close();

                            

                            ///////////////////
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "Select [groupalias] as family,[clientalias] as clientcode,branch,[clientname] from [MF_Client_Master] where [panno]='" + item.Key + "' and branch!='RETAILKOLH'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();

                                string family = dr[0].ToString();
                                string clientcode = dr[1].ToString();
                                string branch = dr[2].ToString();
                                string clientname = dr[3].ToString();
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select ClientCode,FamilyCode from ClientMaster where ClientCode='" + clientcode + "'";
                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + clientcode + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    dr.Read();
                                    family = dr[1].ToString();
                                    dr.Close();
                                }
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                dr = cmd.ExecuteReader();
                                if (!dr.HasRows)
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,PMS)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + item.Value + ")";

                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "update INVESTMENTSUMMARY set PMS='" + item.Value + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                    cmd.ExecuteNonQuery();

                                }
                                //////////////////
                            }
                            else
                            {


                                dr.Close();
                                cmd = conn.CreateCommand();
                                string subbroker = "";
                                string family = "";// = dr[0].ToString();
                                string clientcode = "";// = dr[0].ToString();
                                string clientname = "";
                                cmd.CommandText = "Select PMSCODE,SUBBROKER,NAME from PMSMASTER where PAN='" + item.Key + "'";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    family = dr[0].ToString();
                                    clientcode = dr[0].ToString();
                                    subbroker = dr[1].ToString();
                                    clientname = dr[2].ToString();
                                }
                                dr.Close();
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "Select BranchName from SBCODE where Subbroker='" + subbroker + "' ";
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    string branch = dr[0].ToString();
                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select ClientCode,FamilyCode from ClientMaster where ClientCode='" + clientcode + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO ClientMaster(ClientCode,ClientName,FamilyCode,Branch,RM,AddedDate)values('" + clientcode + "','" + clientname + "','" + family.Trim() + "','" + branch + "','" + branch + "',getdate())";

                                        cmd.ExecuteNonQuery();
                                    }

                                    dr.Close();
                                    cmd = conn.CreateCommand();
                                    cmd.CommandText = "select CLILENTCODE from INVESTMENTSUMMARY where CLILENTCODE='" + clientcode.Trim() + "' and IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "'";
                                    dr = cmd.ExecuteReader();
                                    if (!dr.HasRows)
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "INSERT   INTO INVESTMENTSUMMARY(IS_date,CLILENTCODE,FAMILYCODE,PMS)values('" + DateTime.Today.ToString("dd-MMM-yyyy") + "','" + clientcode.Trim() + "','" + family.Trim() + "'," + item.Value + ")";

                                        cmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dr.Close();
                                        cmd = conn.CreateCommand();
                                        cmd.CommandText = "update INVESTMENTSUMMARY set PMS='" + item.Value + "' where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' and CLILENTCODE='" + clientcode.Trim() + "'";

                                        cmd.ExecuteNonQuery();

                                    }
                                } dr.Close();
                            }
                            dr.Close();
                        }
                    }

                    //conn.Close();
                    //MessageBox.Show("Updation Done Successfully..!");
                    //conn.Open();


                    cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT   INTO UploadLog(FileName,UploadDate)values('PMS.xls','" + DateTime.Today.ToString() + "')";

                    cmd.ExecuteNonQuery();

                    /*********************/










                 mail("PMS", "File Uploaded Successfully"," ");
                }catch(Exception ex)
            {
                mail("PMS", "File Not  Uploaded Successfully","ccare06");
                }
                }
                else
                {
                    mail("PMS", "File Not Found","ccare06");
                
                }


                    /************
                  * 
                  * 
                  * ****PMS end****
                  * 
                  * 
                  * 
                  * ************/
                /**********BRANCH UPDATE**************/
               // dr.Close();
                //cmd.Connection = conn;
                //cmd.CommandText = "update [ClientMaster]  set ClientMaster.Branch=(select Cust_Client_Master.branch from Cust_Client_Master where ClientMaster.[ClientCode]=[Cust_Client_Master].[clientcode]) where  ClientMaster.[ClientCode] in (select distinct Cust_Client_Master.ClientCode from Cust_Client_Master) ";
                //cmd.ExecuteNonQuery();
                /***********************/
                cmd = conn.CreateCommand();
                cmd.CommandText = " SELECT     FILEMASTER.FileName FROM         FILEMASTER where        FILEMASTER.FileName not in (select FileName from  UploadLog where UploadDate='" + DateTime.Today.ToString() + "')";
                dr = cmd.ExecuteReader();
             DataTable      dt1231 = new DataTable();
                if (dr.HasRows)
                {
                    dt1231.Load(dr);
                }
           
                dr.Close();
                cmd = conn.CreateCommand();
                cmd.CommandText = " SELECT DISTINCT [FileName] FROM [UploadLog] WHERE ([UploadDate] = '" + DateTime.Today.ToString() + "') ";
                dr = cmd.ExecuteReader();
              DataTable   dt123  = new DataTable();
                if (dr.HasRows)
                {
                    dt123 .Load(dr);
                }
                
                dr.Close();
                /*******************Family Leaderwise Summary*******************************/


                cmd = conn.CreateCommand();
                //if (BranchDropDownList1.SelectedIndex != 0)
                //{
                //    cmd.CommandText = "select invsum.FAMILYCODE as Family, sum(  invsum.CASH ) as Equity,sum( invsum.FNO) as FNO ,Sum(invsum.PMS) as PMS,Sum( invsum.MF ) as MF from  INVESTMENTSUMMARY invsum ,ClientMaster cm where invsum.IS_date='" + Convert.ToDateTime(DateDropDownList1.Text).ToShortDateString() + "' and cm.FamilyCode=invsum.FAMILYCODE  and cm.Branch='" + BranchDropDownList1.SelectedItem.Text + "'     group by invsum.FAMILYCODE order by invsum.FAMILYCODE desc";
                //}
                //else
                //{
                    cmd.CommandText = "select invsum.FAMILYCODE as Family, sum(  invsum.CASH ) as Equity,sum( invsum.FNO) as FNO ,Sum(invsum.PMS) as PMS,Sum( invsum.MF ) as MF from  INVESTMENTSUMMARY invsum  where invsum.IS_date='" + DateTime.Today.ToShortDateString() + "'   group by invsum.FAMILYCODE order by invsum.FAMILYCODE desc";

                //}
                
                dr = cmd.ExecuteReader();
                  dt = new DataTable();
                dt.Load(dr);
                //dt.Columns.Add("FNO");

                //dt.Columns.Add("MF");
                //dt.Columns.Add("PMS");




                //dt.Columns.Add("Cash");
                dt.Columns.Add("Total");
                dt.Columns.Add("FamilyTotal");
                decimal familytotal = 0;
                string family_ = "";
                string family1 = "";
                decimal fno = 0;
                decimal mf = 0;
                decimal pms = 0;
                decimal cash_ = 0;
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
                        cash_ = Convert.ToDecimal(dt.Rows[i]["Equity"]);
                    }
                    // drw["PMS"].ToString()drw["MF"].ToString()drw["CASH"].ToString()

                    family_ = dt.Rows[i]["family"].ToString();
                    if (i < dt.Rows.Count - 1)
                    {
                        family1 = dt.Rows[i + 1]["family"].ToString();
                    }
                    else
                    {
                        family1 = "";
                    }
                    if (!family1.Equals(family_))
                    {
                        fnototal = fnototal + fno;
                        mftotal = mftotal + mf;
                        pmstotal = pmstotal + pms;
                        cashtotal = cashtotal + cash_;
                        familytotal = familytotal + fno + pms + cash_ + mf;
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
                        cash_ = 0;

                    }
                    else
                    {
                        familytotal = familytotal + fno + pms + cash_ + mf;
                        fnototal = fnototal + fno;
                        mftotal = mftotal + mf;
                        pmstotal = pmstotal + pms;
                        cashtotal = cashtotal + cash_;
                    }

                }
                // GridView1.DataSource = dt;
                //GridView1.DataBind();
                //  MessageBox.Show("DATA EXPORTING STARTED");
                if (System.IO.File.Exists(@"Summary.xls")) //It checks if file exists then it delete that file.
                {
                    System.IO.File.Delete(@"Summary.xls");
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
                        drnew["FamilyCode"] = drt["family"];
                        drnew["FNO"] = drt["FNO"];
                        drnew["PMS"] = drt["PMS"];
                        drnew["MF"] = drt["MF"];
                        drnew["Equity"] = drt["Equity"];
                        drnew["Total"] = drt["FamilyTotal"];
                        cmd = conn.CreateCommand();
                        //if (BranchDropDownList1.SelectedIndex == 0)
                        //{
                            cmd.CommandText = "select   ClientName, RM ,Branch  from  ClientMaster     where ClientCode='" + drt["family"].ToString() + "'";
                        //}
                        //else
                        //{
                        //    cmd.CommandText = "select   ClientName, RM ,Branch  from  ClientMaster     where ClientCode='" + drt["family"].ToString() + "' and Branch='" + BranchDropDownList1.SelectedItem.Text + "'";

                        //}
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
                            //if (BranchDropDownList1.SelectedIndex == 0)
                            //{
                                cmd.CommandText = "select   cc.ClientName,cm.RM,cc.branch   from  ClientMaster cm,Cust_Client_Master cc  where cc.clientcode='" + drt["family"].ToString() + "' and cc.clientcode=cm.FamilyCode";
                            //}
                            //else
                            //{
                            //    cmd.CommandText = "select   cc.ClientName,cm.RM,cc.branch   from  ClientMaster cm,Cust_Client_Master cc where cc.branch='" + BranchDropDownList1.SelectedItem.Text + "' and  cc.clientcode='" + drt["family"].ToString() + "' and cc.clientcode=cm.FamilyCode";

                            //}
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



                        if (!string.IsNullOrEmpty(drnew["Branch"].ToString()))
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
               // if (System.IO.File.Exists(@"D:\summary\Summary.xls")) //It checks if file exists then it delete that file.
              //  {
                   // System.IO.File.Delete(@"D:\summary\Summary.xls");
              //  }
               // Export1(dtsumm, @"D:\summary\Summary.xls");








                /*************************************/
               

                /****************************/
                if (dt1231.Rows.Count > 0)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from INVESTMENTSUMMARY  where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' ";

                    cmd.ExecuteNonQuery();
                    mailsummary(ExportDatatableToHtml(dt1231), ExportDatatableToHtml(dt123), "");
                }
                else
                {
                    mailsummary(ExportDatatableToHtml(dt1231), ExportDatatableToHtml(dt123), @"D:\summary\Summary.xls");
                }

                    conn.Close();
            }
            catch (Exception e)
            {
                dr.Close();
                cmd = conn.CreateCommand();
                cmd.CommandText = " SELECT     FILEMASTER.FileName FROM         FILEMASTER where        FILEMASTER.FileName not in (select FileName from  UploadLog where UploadDate='" + DateTime.Today.ToString() + "')";
                dr = cmd.ExecuteReader();
               DataTable  dt1231 = new DataTable();
                if (dr.HasRows)
                {
                    dt1231.Load(dr);
                }
             
                dr.Close();
                cmd = conn.CreateCommand();
                cmd.CommandText = " SELECT DISTINCT [FileName] FROM [UploadLog] WHERE ([UploadDate] = '" + DateTime.Today.ToString() + "') ";
                dr = cmd.ExecuteReader();
                DataTable dt123 = new DataTable();
                if (dr.HasRows)
                {
                    dt123.Load(dr);
                }
            
                dr.Close();
                mailsummary(ExportDatatableToHtml(dt1231), ExportDatatableToHtml(dt123), "");

                /****************************/
                if (dt1231.Rows.Count > 0)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "delete from INVESTMENTSUMMARY  where  IS_date='" + DateTime.Today.ToString("dd-MMM-yyyy") + "' ";

                    cmd.ExecuteNonQuery();

                }
                else
                {
                    mailsummary(ExportDatatableToHtml(dt1231), ExportDatatableToHtml(dt123), @"D:\summary\Summary.xls");
                }
                conn.Close();
            }
            finally
            {
                range = null;
                sheet = null;
                if (book != null)
                    book.Close(false, Missing.Value, Missing.Value);
                book = null;
                if (app != null)
                    app.Quit();
                app = null;
                conn.Close();
            }
        }







        /*********************************/

        public static void Export1(DataTable dt, string filepath)
        {

            String strFileName = "";
            strFileName = filepath;

            // Server File Path Where you want to save excel file.

            excelapp.Application myExcel = new excelapp.Application();
            //Create a New file
            excelapp._Workbook mybook = myExcel.Workbooks.Add(System.Reflection.Missing.Value);
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
                excelapp._Worksheet mysheet = (excelapp._Worksheet)mybook.ActiveSheet;
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
                //excelapp.Style style1 = myExcel.ActiveWorkbook.Styles.Add("Content", Type.Missing);
                ////style1.Borders.Color = Color.Black;
                //style1.Font.Name = "Verdana";
                //// style1.WrapText = true;
                //style1.Font.Size = 10;

                //style1.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                //style1.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);

                foreach (DataRow drow in dt.Rows)
                {
                    rowIndex = rowIndex + 1;
                    colIndex = 0;
                    ////string str = drow[7].ToString();

                    ////if ((!string.IsNullOrEmpty(drow[7].ToString())))
                    ////{
                    ////    decimal sum = Convert.ToDecimal(str);
                    ////    if (sum <= 0)
                    ////    {
                    ////        break;
                    ////    }
                    ////}
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
              //  MessageBox.Show(wzx.Message);
            }
            finally
            {
                mybook.Close(false, false, System.Reflection.Missing.Value);

                myExcel.Quit();

                GC.Collect();
            }

        }
        /**********************************/









      public string ExportDatatableToHtml(DataTable dt)   
{   
StringBuilder strHTMLBuilder = new StringBuilder();   
strHTMLBuilder.Append("<html >");   
strHTMLBuilder.Append("<head>");   
strHTMLBuilder.Append("</head>");   
strHTMLBuilder.Append("<body>");   
strHTMLBuilder.Append("<table border='1px' cellpadding='1' cellspacing='1' bgcolor='lightyellow' style='font-family:Garamond; font-size:smaller'>");   
   
strHTMLBuilder.Append("<tr >");   
foreach (DataColumn myColumn in dt.Columns)   
{   
strHTMLBuilder.Append("<td >");   
strHTMLBuilder.Append(myColumn.ColumnName);   
strHTMLBuilder.Append("</td>");   
   
}   
strHTMLBuilder.Append("</tr>");   
   
   
foreach (DataRow myRow in dt.Rows)   
{   
   
strHTMLBuilder.Append("<tr >");   
foreach (DataColumn myColumn in dt.Columns)   
{   
strHTMLBuilder.Append("<td >");   
strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());   
strHTMLBuilder.Append("</td>");   
   
}   
strHTMLBuilder.Append("</tr>");   
}   
   
//Close tags.   
strHTMLBuilder.Append("</table>");   
strHTMLBuilder.Append("</body>");   
strHTMLBuilder.Append("</html>");   
   
string Htmltext = strHTMLBuilder.ToString();   
   
return Htmltext;   
   
}

      void mailsummary(    string msg1 ,string msg2,string filepath)
      {

          /********************/
          MailMessage msgMail = new MailMessage();

          MailMessage myMessage = new MailMessage();
          myMessage.From = new MailAddress("techsupport2@tradenetstockbroking.in", "CRR Software");
          myMessage.To.Add("techsupport2@tradenetstockbroking.in");
          myMessage.To.Add("techsupport@tradenetstockbroking.in");
       /////////////////////////////   myMessage.To.Add("samir@tradenetstockbroking.in");////////////////////
          myMessage.To.Add("ccare03@tradenetstockbroking.in");
        
          myMessage.Subject =  "CRR files Upload Details For " + DateTime.Today.ToString("dd-MM-yyyy");
          

          myMessage.IsBodyHtml = true;
          string findfile = "For the day";
          if (!string.IsNullOrEmpty(filepath))
          {
              Attachment attch = new Attachment(filepath);
              myMessage.Attachments.Add(attch);
              findfile = "PFA the Familywise summary for the day ";
              
          }
          string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>  Files Uploaded Successfully:-</br> " + msg2 + "</br> Files Not Uploaded SuccessFully:-</br> " + msg1 + "</br></br></br></br></br> " + findfile + DateTime.Today.ToString("dd-MM-yyyy") + "  </h3></br></br><h4>THANKING YOU,</h4></br><h4>TechSupport TEAM.</h4>";
          myMessage.Body = msgbody;
          //Attachment attch1 = new Attachment(Server.MapPath("~/") + "//Reports//StatusCenterSummary" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
          //myMessage.Attachments.Add(attch1);
          SmtpClient mySmtpClient = new SmtpClient();
          System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential("techsupport2@tradenetstockbroking.in", "tech123");
          mySmtpClient.Host = "10.53.251.9";
          mySmtpClient.Port = 25;
          mySmtpClient.UseDefaultCredentials = false;
          mySmtpClient.Credentials = myCredential;
          mySmtpClient.ServicePoint.MaxIdleTime = 1;
          string day1 = DateTime.Today.DayOfWeek.ToString();
          //if (!day1.Equals("Saturday"))
          //{
   mySmtpClient.Send(myMessage);
          //}
          // MessageBox.Show("Mail sent to " + manemail + " and " + tlemail);
          myMessage.Dispose();
          //dg.Dispose();

          /***************/


      }
        void mail(string file, string msg,string mailid)
        {

            /********************/
            MailMessage msgMail = new MailMessage();

            MailMessage myMessage = new MailMessage();
            myMessage.From = new MailAddress("techsupport2@tradenetstockbroking.in", "CRR Software");
            myMessage.To.Add("techsupport2@tradenetstockbroking.in");
            myMessage.To.Add("techsupport@tradenetstockbroking.in");
            myMessage.To.Add("ccare03@tradenetstockbroking.in");
            if (!string.IsNullOrEmpty(mailid.Trim()))
            {
                myMessage.To.Add(mailid + "@tradenetstockbroking.in");
            }
            myMessage.Subject =  msg+" " +file +" "  + DateTime.Today.ToString("dd-MM-yyyy");
            string msgbody = "<h2>Dear Sir,</h2> </br> <h3>GREETINGS FOR THE DAY!!!</h3></br><h3>  " + msg + " " + file + " " + DateTime.Today.ToString("dd-MM-yyyy") + "  </h3></br></br><h4>THANKING YOU,</h4></br><h4>TechSupport TEAM.</h4>";


            myMessage.IsBodyHtml = true;
            myMessage.Body = msgbody;
            //Attachment attch = new Attachment(Server.MapPath("~/") + "//Reports//CallCenterSummary" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
            //myMessage.Attachments.Add(attch);
            //Attachment attch1 = new Attachment(Server.MapPath("~/") + "//Reports//StatusCenterSummary" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
            //myMessage.Attachments.Add(attch1);
            SmtpClient mySmtpClient = new SmtpClient();
            System.Net.NetworkCredential myCredential = new System.Net.NetworkCredential("techsupport2@tradenetstockbroking.in", "tech123");
            mySmtpClient.Host = "10.53.251.9";
            mySmtpClient.Port = 25;
            mySmtpClient.UseDefaultCredentials = false;
            mySmtpClient.Credentials = myCredential;
            mySmtpClient.ServicePoint.MaxIdleTime = 1;
            string day1 = DateTime.Today.DayOfWeek.ToString();
            //if (!day1.Equals("Saturday"))
            //{
   mySmtpClient.Send(myMessage);
            //}
            // MessageBox.Show("Mail sent to " + manemail + " and " + tlemail);
            myMessage.Dispose();
            //dg.Dispose();

            /***************/

        
        }



        protected override void OnStop()
        {
        }
    }


}
