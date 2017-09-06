using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.IO;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using System.Data.OleDb;


namespace InvestmentSummary
{
    public partial class ImportMaster : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
      int duplicates = 0;
    int newrecords = 0;
 static string[] update;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["page"] = "Import Master";
          
            

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);

        }

        protected void ButtonGetData_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Server.MapPath("/" + Session["login"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("/" + Session["login"].ToString()));
            }
            if (FileUpload1.HasFile)
            {
                try
                {
                    FileUpload1.SaveAs(Server.MapPath("/" + Session["login"].ToString()) + "/Master1.xls");
                    if (RadioButtonList1.SelectedIndex == 0)
                    {
                        string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + Server.MapPath("/" + Session["login"].ToString() + "/" + "Master1.xls") + ";" + "Extended Properties=Excel 8.0;";

                        // create your excel connection object using the connection string
                        OleDbConnection objXConn = new OleDbConnection(xConnStr);
                        objXConn.Open();

                        // use a SQL Select command to retrieve the data from the Excel Spreadsheet
                        // the "table name" is the name of the worksheet within the spreadsheet
                        // in this case, the worksheet name is "Members" and is coded as: [Members$]
                        OleDbCommand objCommand = new OleDbCommand("SELECT * FROM [Customer care$]", objXConn);
                        //StreamReader streamread = new StreamReader(Server.MapPath("/temp/" + Session["login"].ToString()) + "/" + MasterFileUpload1.FileName);
                        OleDbDataReader dr = objCommand.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(dr);

                        Session["dt"] = dt;


                        // Bind the data to the GridView
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        objXConn.Close();
                        // int count= dt.Columns.Count;

                        if (GridView1.HeaderRow.Cells[0].Text == "Sr" && GridView1.HeaderRow.Cells[1].Text == "Code" && GridView1.HeaderRow.Cells[2].Text == "Family" && GridView1.HeaderRow.Cells[3].Text == "LongName" && GridView1.HeaderRow.Cells[4].Text == "ShortName" && GridView1.HeaderRow.Cells[6].Text == "BR" && GridView1.HeaderRow.Cells[7].Text == "SB" && GridView1.HeaderRow.Cells[8].Text == "Trader" && GridView1.HeaderRow.Cells[9].Text == "Phone(R)" && GridView1.HeaderRow.Cells[10].Text == "Phone(O)" && GridView1.HeaderRow.Cells[11].Text == "mobile_pager" && GridView1.HeaderRow.Cells[12].Text == "Email" && GridView1.HeaderRow.Cells[13].Text == "PanGirNo" && GridView1.HeaderRow.Cells[14].Text == "ActiveFrom" && GridView1.HeaderRow.Cells[15].Text == "InActiveFrom" && GridView1.HeaderRow.Cells[16].Text == "Approved_By" && GridView1.HeaderRow.Cells[17].Text == "Introducer" && GridView1.HeaderRow.Cells[18].Text == "Default_DpId" && GridView1.HeaderRow.Cells[19].Text == "Client_DpId" && GridView1.HeaderRow.Cells[20].Text == "Client_AccNo" && GridView1.HeaderRow.Cells[21].Text == "Client_BankName" && GridView1.HeaderRow.Cells[23].Text == "Address1" && GridView1.HeaderRow.Cells[24].Text == "Address2" && GridView1.HeaderRow.Cells[25].Text == "Address3" && GridView1.HeaderRow.Cells[26].Text == "City" && GridView1.HeaderRow.Cells[27].Text == "State" && GridView1.HeaderRow.Cells[28].Text == "Nation" && GridView1.HeaderRow.Cells[29].Text == "Zip")
                        {
                            //Sr	Code	Family	LongName	ShortName	Region	BR	SB	Trader	Phone(R)	Phone(R)	mobile_pager	Email	PanGirNo	ActiveFrom	InActiveFrom	Approved_By	Introducer	Default_DpId	Client_DpId	Client_AccNo	Client_BankName	IFSCCODE	Address1	Address2	Address3	City	State	Nation	Zip

                            Label7.Text = " Sr	Code	Family	LongName	ShortName	Region	BR	SB	Trader	Phone(R)	Phone(O)	mobile_pager	Email	PanGirNo	ActiveFrom	InActiveFrom	Approved_By	Introducer	Default_DpId	Client_DpId	Client_AccNo	Client_BankName	IFSCCODE	Address1	Address2	Address3	City	State	Nation	Zip";
                            checkduplicates();
                        }
                        else
                        {
                            GridView1.DataSource = null;
                            GridView1.DataBind();
                            MessageBox.Show(" Invalid Column Name/Sequence..!  ");
                         //   Label7.Text = "Column Sequence: 1.Sr 2.Code 3.Family	4.LongName	5.ShortName	6.BR	7.SB	8.Trader	9.Phone(R)	10.Phone(O)	11.mobile_pager	12.Email	13.PanGirNo	14.ActiveFrom	15.InActiveFrom	16.Approved_By	17.Introducer	18.BankCode	19.BankName	20.PaymentMode	21.Default_DpId	22.Client_DpId	23.Client_AccNo	24.Client_BankName	25.Address1	26.Address2	27.Address3	28.City	29.State	30.Nation	31.Zip";

                        
                        }


                    }
                    if (RadioButtonList1.SelectedIndex == 1)
                    {
                        string xConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                          "Data Source=" + Server.MapPath("/" + Session["login"].ToString() + "/" + "Master1.xls") + ";" + "Extended Properties=Excel 8.0;";


                        // create your excel connection object using the connection string
                        OleDbConnection objXConn = new OleDbConnection(xConnStr);
                        //objXConn.Open();

                        // use a SQL Select command to retrieve the data from the Excel Spreadsheet
                        // the "table name" is the name of the worksheet within the spreadsheet
                        // in this case, the worksheet name is "Members" and is coded as: [Members$]
                        OleDbCommand objCommand = new OleDbCommand("SELECT * FROM [Mutul Fund$]", objXConn);
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
                        GridView1.DataSource = objDataSet.Tables[0].DefaultView;
                        GridView1.DataBind();
                        if (GridView1.HeaderRow.Cells[0].Text == "NAME" && GridView1.HeaderRow.Cells[1].Text == "CLIENT_ALIAS" && GridView1.HeaderRow.Cells[2].Text == "MOBILE" && GridView1.HeaderRow.Cells[3].Text == "PHONE" && GridView1.HeaderRow.Cells[4].Text == "EMAIL1" && GridView1.HeaderRow.Cells[5].Text == "EMAIL2" && GridView1.HeaderRow.Cells[6].Text == "ADDRESS1" && GridView1.HeaderRow.Cells[7].Text == "ADDRESS2" && GridView1.HeaderRow.Cells[8].Text == "ADDRESS3" && GridView1.HeaderRow.Cells[9].Text == "CITY" && GridView1.HeaderRow.Cells[10].Text == "PAN" && GridView1.HeaderRow.Cells[11].Text == "DOB" && GridView1.HeaderRow.Cells[12].Text == "GROUP" && GridView1.HeaderRow.Cells[13].Text == "GROUP_ALIAS" && GridView1.HeaderRow.Cells[14].Text == "SUBBROK" && GridView1.HeaderRow.Cells[15].Text == "RM" && GridView1.HeaderRow.Cells[16].Text == "FILENO" && GridView1.HeaderRow.Cells[17].Text == "LOCK" &&  GridView1.HeaderRow.Cells[18].Text == "EQUITYCODE1" && GridView1.HeaderRow.Cells[19].Text == "EQUITYCODE2")
                        {
                            Label7.Text = "";
                            //checkduplicates();
                        }
                        else
                        {
                            GridView1.DataSource = null;
                            GridView1.DataBind();
                            MessageBox.Show(" Invalid Column Name/Sequence..!  ");
                    //     Label7.Text="Column Sequence: 1.NAME	2.CLIENT_ALIAS	3.MOBILE	4.PHONE	5.EMAIL1	6.EMAIL2	7.ADDRESS1	8.ADDRESS2	9.ADDRESS3	10.CITY	11.PAN	12.DOB	13.GROUPNAME	14.GROUP_ALIAS	15.SUBBROK	16.RM	17.FILENO	18.LOCK	19.EQUITY	20.DEBT	21.EQUITYCODE1	22.EQUITYCODE2";

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" Check sheet names in XLS 1.Customer care 2.Mutul Fund  '" + ex.Message + " ");
                }
            }

        }
        void checkduplicates()
        {
          
            if (GridView1.Rows.Count != 0)
            {
                if (RadioButtonList1.SelectedIndex == 1)
                {
                    DataTable dt = (DataTable)Session["dt"];
                    int ccmf = 0;
                    int panno = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        conn.Open();
                        string qry2 = "";
                        //if (string.IsNullOrEmpty(dt.Rows[i][20].ToString()))
                        //{
                            if (!string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                            {
                                qry2 = "select * from MF_Client_Master where clientalias='" + dt.Rows[i][1].ToString() + "'";
                            }
                            //else
                            //{
                            //    qry2 = "select * from MF_Client_Master where clientname='" + dt.Rows[i][0].ToString() + "'";
                            //}
                            cmd = new SqlCommand(qry2, conn);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                 
                                    duplicates++;



                                
                                dr.Close();
                            }
                            else
                            {

                                dr.Close();
                                if (!string.IsNullOrEmpty(dt.Rows[i][10].ToString()))
                            {
                                /***********PAN NO CHECK**********************/
                                qry2 = "Select clientcode from  Cust_Client_Master where panno='" + dt.Rows[i][10].ToString().Trim()+"'";
                                cmd = new SqlCommand(qry2, conn);
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    panno++;
                                    dr.Read();
                                    dt.Rows[i][20] = dr[0].ToString();
                                }
                                dr.Close();
                                /*********************************/
                            
                            }
                                

                            }
                            

                        //}                             

                        //else
                        //{
                        //    //cmd = new SqlCommand("Update table Cust_Client_Master set mf='true' where clientcode='" + dt.Rows[i][20].ToString() + "'", conn);
                        //   // cmd.ExecuteNonQuery();
                        //    ccmf++;

                        //}

                        
                        conn.Close();
                    
                    }
                    
                    if (ccmf != 0)
                        {
                           // MessageBox.Show(ccmf+" Clients are having Equity Code , will be skipped...!");
                        }
                   if (panno != 0)
                    {
                     //   MessageBox.Show(panno + " Clients are in Equity based on PAN NO  ,will be skipped...!");
                    
                    }
                 
                }
                if (RadioButtonList1.SelectedIndex == 0)
                {
                    DataTable dt = (DataTable)Session["dt"];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        conn.Open();
                        if (!string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                        {
                            string qry1 = "select clientid from Cust_Client_Master where clientcode='" + dt.Rows[i][1].ToString() + "'";
                            cmd = new SqlCommand(qry1, conn);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {

                                duplicates++;




                                dr.Close();
                            }
                            else
                            {
                                dr.Close();

                            }
                        }
                        conn.Close();
                    }
                   

                }

         
                

            }
           
        
        }
        protected void Yes_Click(object sender, EventArgs e)
        {
            

            if (GridView1.Rows.Count != 0)
            {
                if (RadioButtonList1.SelectedIndex == 1)
                {
                    //duplicates = 0;
                    DataTable dt = (DataTable)Session["dt"];
                     update = new string[dt.Rows.Count];
                     int ccmf1 = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string branch = "";
                        conn.Open();
                        string qry2 = "";
                        //if (string.IsNullOrEmpty(dt.Rows[i][20].ToString()))
                        //{
                            //if (!string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                            //{
                                qry2 = "select * from MF_Client_Master where clientalias='" + dt.Rows[i][1].ToString() + "'";
                            //}
                            //else
                            //{
                            //    qry2 = "select * from MF_Client_Master where clientname='" + dt.Rows[i][0].ToString() + "'";
                            //}
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
                                     //
                                     //  cmd.Parameters.AddWithValue("@0", dt.Rows[i][0].ToString());
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
                                     cmd.Parameters.AddWithValue("@11", dt.Rows[i][10].ToString());
                                     cmd.Parameters.AddWithValue("@12", dt.Rows[i][11].ToString());
                                     cmd.Parameters.AddWithValue("@13", dt.Rows[i][12].ToString());
                                     cmd.Parameters.AddWithValue("@14", dt.Rows[i][13].ToString());
                                     cmd.Parameters.AddWithValue("@15", dt.Rows[i][14].ToString());
                                     cmd.Parameters.AddWithValue("@16", dt.Rows[i][15].ToString());
                                     cmd.Parameters.AddWithValue("@17", dt.Rows[i][16].ToString());
                                     cmd.Parameters.AddWithValue("@18", dt.Rows[i][17].ToString());
                                     cmd.Parameters.AddWithValue("@19", dt.Rows[i][18].ToString());
                                     cmd.Parameters.AddWithValue("@20", dt.Rows[i][19].ToString());
                                     cmd.Parameters.AddWithValue("@21", dt.Rows[i][18].ToString());
                                     cmd.Parameters.AddWithValue("@22", dt.Rows[i][19].ToString());

                                     cmd.ExecuteNonQuery();
                                     newrecords++;
                                 
                            }
conn.Close();
                        }
                        //else
                        //{
                        //    cmd = new SqlCommand("Update  Cust_Client_Master set mf='true' where clientcode='" + dt.Rows[i][20].ToString() + "'", conn);
                        //     cmd.ExecuteNonQuery();
                        //     ccmf1++;
                            
                        //}
                        
                    }
                    if (newrecords != 0)
                    {
                        MessageBox.Show(" " + newrecords + " client record newly added..! ");
                    }
                    //if (ccmf1 != 0)
                    //{
                    //   // MessageBox.Show(" " + ccmf1 + " client record marked as MF clients in Equity..! ");
                    //}
                    if (duplicates != 0)
                    {
                       // btnUpdate.Visible = true;
                        MessageBox.Show(" " + duplicates + " duplicate client records found..! ");

                    }
                    conn.Open();
                    string quer = "Select subbroker,branch from MFBranch";
                    cmd = new SqlCommand(quer, conn);
                    dr = cmd.ExecuteReader();
                    DataTable dt12 = new DataTable();
                    dt12.Load(dr);
                    foreach (DataRow drow in dt12.Rows)
                    {
                        cmd = new SqlCommand("update MF_Client_Master set branch='" + drow[1].ToString().Trim() + "' where subbroker='" + drow[0].ToString().Trim() + "'", conn);
                        cmd.ExecuteNonQuery();
                    }
                   
                    //cmd = new SqlCommand();
                    //cmd.Connection = conn;
                    //cmd.CommandText = "insert into MFUpload(MFUploadDate,New,Duplicate)values(getdate()," + newrecords + "," + duplicates + ")";
                    //cmd.ExecuteNonQuery();
                    conn.Close();

                }



                if (RadioButtonList1.SelectedIndex == 0)
                {
                   // duplicates = 0;
                    DataTable dt = (DataTable)Session["dt"];
          update = new string[dt.Rows.Count];
                   
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        conn.Open();
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
                                // cmd.Parameters.AddWithValue("@0", dt.Rows[i][0].ToString());
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
                        conn.Close();
                    }
                    if (newrecords != 0)
                    {
                        conn.Open();
                        //cmd = new SqlCommand();
                        //cmd.Connection = conn;

                        //cmd.CommandText = "update Cust_Client_Master set inactivefrom='INACTIVE' where update1!='" + DateTime.Today + "'";
                        //cmd.ExecuteNonQuery();
                        cmd = new SqlCommand();
                        cmd.Connection = conn;

                        cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_LEADER' where update1='" + DateTime.Today + "'";
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand(); //TILL DEMO remaining
                        cmd.Connection = conn;

                        cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_MEMBER' where clientcode!=family ";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show(" " + newrecords + " client record newly added..! ");
                    }
          
                    if (duplicates != 0)
                    {
                        conn.Open();
                        cmd = new SqlCommand();
                        //cmd.Connection = conn;

                        //cmd.CommandText = "update Cust_Client_Master set inactivefrom='INACTIVE' where update1!='" + DateTime.Today + "'";
                        //cmd.ExecuteNonQuery();
                        //cmd = new SqlCommand();
                        cmd.Connection = conn;

                        cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_LEADER' where update1='" + DateTime.Today + "'";
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand();//TILL DEMO
                        cmd.Connection = conn;

                        cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_MEMBER' where clientcode!=family ";
                        cmd.ExecuteNonQuery();
                        conn.Close();
                       // btnUpdate.Visible = true;
                        MessageBox.Show(" " + duplicates + " duplicate client records found..! ");

                    }


                    conn.Open();
                    //cmd = new SqlCommand();
                    //cmd.Connection = conn;
                    //cmd.CommandText = "insert into Uploads(CCImportDate,New,Duplicate)values(getdate()," + newrecords + "," + duplicates + ")";
                    //cmd.ExecuteNonQuery();


                    cmd = new SqlCommand();
                    cmd.CommandText = "Select Subbroker,BranchName from SBCODE";
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
                    conn.Close();

                }

            
            if (GridView1.Rows.Count != 0)
            {
                duplicates = 0;
                if (RadioButtonList1.SelectedIndex == 1)
                {
                    DataTable dt = (DataTable)Session["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(update[i]))
                        {
                            string qry = "update MF_Client_Master set panno=@panno, mobileno=@mobileno,emailid1=@emailid1,emailid2=@emailid2,address1=@address1,city=@city,dob=@dob,subbroker=@subbroker,landline=@landline,update1=@update1" + "  where clientid=" + update[i];
                            duplicates++;
                            // string qry = "update MF_Client_Master set panno='" + dt.Rows[i][10].ToString() + "',mobileno='" + dt.Rows[i][2].ToString() + "',emailid1='" + dt.Rows[i][4].ToString() + "',emailid2='" + dt.Rows[i][5].ToString() + "',address1='@address1',city='" + dt.Rows[i][9].ToString() + "',dob='" + dt.Rows[i][11].ToString() + "',subbroker='" + dt.Rows[i][14].ToString() + "',landline='" + dt.Rows[i][3].ToString() + "',update1='" + DateTime.Now.ToShortDateString() + "' where clientid='" + update[i] + "'";
                            cmd = new SqlCommand(qry, conn);
                            cmd.Parameters.AddWithValue("@panno", dt.Rows[i][10].ToString());
                            cmd.Parameters.AddWithValue("@mobileno", dt.Rows[i][2].ToString());
                            cmd.Parameters.AddWithValue("@emailid1", dt.Rows[i][4].ToString());
                            cmd.Parameters.AddWithValue("@emailid2", dt.Rows[i][5].ToString());
                            cmd.Parameters.AddWithValue("@address1", dt.Rows[i][6].ToString());
                            cmd.Parameters.AddWithValue("@city", dt.Rows[i][9].ToString());
                            cmd.Parameters.AddWithValue("@dob", dt.Rows[i][11].ToString());
                            cmd.Parameters.AddWithValue("@subbroker", dt.Rows[i][14].ToString());
                            cmd.Parameters.AddWithValue("@landline", dt.Rows[i][3].ToString());
                            cmd.Parameters.AddWithValue("@update1", DateTime.Now);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                    }
                  //  btnUpdate.Visible = false;
                    conn.Open();
                    string quer = "Select subbroker,branch from MFBranch";
                    cmd = new SqlCommand(quer, conn);
                    dr = cmd.ExecuteReader();
                    DataTable dt12 = new DataTable();
                    dt12.Load(dr);
                    foreach (DataRow drow in dt12.Rows)
                    {
                        cmd = new SqlCommand("update MF_Client_Master set branch='" + drow[1].ToString().Trim() + "' where subbroker='" + drow[0].ToString().Trim() + "'", conn);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    MessageBox.Show(" " + duplicates + " duplicate client records updated..! ");


                }
                else
                {
                    duplicates = 0;
                    DataTable dt = (DataTable)Session["dt"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(update[i]))
                        {
                            string qry = "update Cust_Client_Master set family=@family,update1=@update1,trader=@trader,address2=@address2,city=@city,landline2=@landline2,panno=@panno,emailid=@emailid,mobileno=@mobileno,landline1=@landline1,clientname=@clientname" + "  where clientid=" + update[i] + ""; //,subbrokercode=@subbrokercode;
                            cmd = new SqlCommand(qry, conn);
                           // cmd.Parameters.AddWithValue("@clientname", dt.Rows[i][3].ToString());
                           // cmd.Parameters.AddWithValue("@family", dt.Rows[i][2].ToString());
                           //// cmd.Parameters.AddWithValue("@subbrokercode", dt.Rows[i][6].ToString());
                           // cmd.Parameters.AddWithValue("@landline1", dt.Rows[i][8].ToString());
                           // cmd.Parameters.AddWithValue("@landline2", dt.Rows[i][9].ToString());
                           // cmd.Parameters.AddWithValue("@mobileno", dt.Rows[i][10].ToString());
                           // cmd.Parameters.AddWithValue("@emailid", dt.Rows[i][11].ToString());
                           // cmd.Parameters.AddWithValue("@panno", dt.Rows[i][12].ToString());
                           // cmd.Parameters.AddWithValue("@address2", dt.Rows[i][25].ToString());
                           // cmd.Parameters.AddWithValue("@city", dt.Rows[i][27].ToString());
                           // cmd.Parameters.AddWithValue("@trader", dt.Rows[i][7].ToString());
                           // cmd.Parameters.AddWithValue("@update1", DateTime.Now.ToShortDateString());

                            string landline1 = dt.Rows[i][9].ToString();
                            string landline2=dt.Rows[i][10].ToString();


                            cmd.Parameters.AddWithValue("@clientname", dt.Rows[i][3].ToString());
                            cmd.Parameters.AddWithValue("@family", dt.Rows[i][2].ToString());
                            //cmd.Parameters.AddWithValue("@subbrokercode", dt.Rows[i][7].ToString());
                            cmd.Parameters.AddWithValue("@landline1", dt.Rows[i][9].ToString());
                            cmd.Parameters.AddWithValue("@landline2", dt.Rows[i][10].ToString());
                            cmd.Parameters.AddWithValue("@mobileno", dt.Rows[i][11].ToString());
                            cmd.Parameters.AddWithValue("@emailid", dt.Rows[i][12].ToString());
                            cmd.Parameters.AddWithValue("@panno", dt.Rows[i][13].ToString());
                            cmd.Parameters.AddWithValue("@address2", dt.Rows[i][24].ToString());
                            cmd.Parameters.AddWithValue("@city", dt.Rows[i][26].ToString());
                            cmd.Parameters.AddWithValue("@trader", dt.Rows[i][8].ToString());
                            cmd.Parameters.AddWithValue("@update1", DateTime.Now.ToShortDateString());

                            duplicates++;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                    }
                    //btnUpdate.Visible = false;
                    MessageBox.Show(" " + duplicates + " duplicate client records updated..! ");
                    conn.Open();
                    //cmd = new SqlCommand();
                    //cmd.Connection = conn;

                    //cmd.CommandText = "update Cust_Client_Master set inactivefrom='INACTIVE' where update1!='"+DateTime.Today+"'";
                    //cmd.ExecuteNonQuery();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_LEADER' where update1='" + DateTime.Today + "'";
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand();//TILL DEMO 
                    cmd.Connection = conn;

                    cmd.CommandText = "update Cust_Client_Master set inactivefrom='GR_MEMBER' where clientcode!=family ";
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    conn.Open();
                    cmd = new SqlCommand();
                    cmd.CommandText = "Select Subbroker,BranchName from SBCODE";
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
                    conn.Close();

                }
            }

            GridView1.DataSource = null;
            GridView1.DataBind();
            if (duplicates == 0)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }


        }
  

        protected void Button2_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin.aspx");

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
           
        }
    }
}
