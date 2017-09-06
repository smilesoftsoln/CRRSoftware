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
using System.Data.SqlClient;
using System.Web.Configuration;

namespace InvestmentSummary
{
    public partial class NewUser : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        static int userid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["page"] = "User Management";

            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            if (!IsPostBack)
            {
                Button4.Enabled = false;
                Button3.Enabled = false;
            }
           //Button1.Text = "New";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Button1.Text == "New")
            {
                Button1.Text = "Save";
                TextBox1.Text = "";
                TextBox2.Text = "";
                EmailTextbox.Text = "";
               // DropDownList1.SelectedItem.Text = "User";
            }
            else if (TextBox2.Text != "" && TextBox1.Text != "" && EmailTextbox.Text != "")
            {
              
                conn.Open();
                cmd = new SqlCommand("Select * from UserMaster where username='" + TextBox1.Text.Trim() + "'", conn);
                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    cmd = new SqlCommand("insert into UserMaster values('" + TextBox1.Text.Trim() + "','" + TextBox2.Text.Trim() + "','" + DropDownList1.SelectedItem.Text + "','"+BranchDropDownList2.Text+"','false','"+EmailTextbox.Text+"')", conn);
                    cmd.ExecuteNonQuery();
                    if (DropDownList1.SelectedValue.Equals("Mentor"))
                    {
                        foreach (ListItem chk in BranchCheckBoxList1.Items)
                        {
                            if (chk.Selected == true)
                            {
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "insert into MentorMaster(Mentor,Branch) values(@Mentor,@Branch)";
                                cmd.Parameters.AddWithValue("Mentor", TextBox1.Text.Trim());
                                cmd.Parameters.AddWithValue("Branch", chk.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    Label4.Text = "User Created Successfully..!";
                    Button1.Text = "New";
                }
                else
                {
                    Label4.Text = "User Allready Exists..!";

                }
                dr.Close();
                conn.Close();
            }
            else
            {
                Label4.Text = "Fill All Values..!";
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Label6.Text = "";
            TextBox1.Text = "";
            TextBox2.Text = "";
            EmailTextbox.Text = "";
            //DropDownList1.SelectedItem.Text = "User";
            Button1.Text = "Save";
            Label4.Text = "";
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = new SqlCommand("Select * from UserMaster where userid=1", conn);
            userid = 1;
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Label6.Text = dr[0].ToString();
                    TextBox1.Text = dr[1].ToString();
                    TextBox2.Text = dr[2].ToString();
                    DropDownList1.SelectedValue = dr[3].ToString();
                    BranchDropDownList2.Text=dr[4].ToString();
                 //   ChekMF.Checked = (dr[4].ToString() == "True");
                  //  ChekMF.Checked = dr[4];
                    EmailTextbox.Text = dr[6].ToString();
                    if (DropDownList1.SelectedValue.Equals("Mentor"))
                    {
                        dr.Close();
                        BranchCheckBoxList1.Visible = true;
                        BranchDropDownList2.Visible = false;
                        BranchCheckBoxList1.Items.Clear();
                        BranchCheckBoxList1.DataBind();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select Branch from MentorMaster where Mentor='" + TextBox1.Text.Trim() + "'";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ListItem chk = BranchCheckBoxList1.Items.FindByValue(dr[0].ToString());
                                chk.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        BranchDropDownList2.Visible = true;
                        BranchCheckBoxList1.Visible = false;
                        BranchDropDownList2.Text = dr[4].ToString();
                    }
                    Label4.Text = "First User Entry..!";

                    Button3.Enabled = true;
                    Button6.Enabled = true;
                    Button5.Enabled = false;
                    Button4.Enabled = false;
                }
            }
            else
            {
                Label4.Text = "User Entry Not Found..!";
            }
            dr.Close();
            conn.Close();
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = new SqlCommand("Select max(userid) from UserMaster", conn);
            int max = (int)cmd.ExecuteScalar();//.ExecuteReader();
            userid = max;
            cmd = new SqlCommand("Select * from UserMaster where userid=" + max, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Label6.Text = dr[0].ToString();
                    TextBox1.Text = dr[1].ToString();
                    TextBox2.Text = dr[2].ToString();
                    DropDownList1.SelectedValue = dr[3].ToString();
                    BranchDropDownList2.Text = dr[4].ToString();
                    EmailTextbox.Text = dr[6].ToString();
                  //  ChekMF.Checked =(bool) dr[4];
                    Label4.Text = "Last User Entry..!";
                    if (DropDownList1.SelectedValue.Equals("Mentor"))
                    {
                        dr.Close();
                        BranchCheckBoxList1.Visible = true;
                        BranchDropDownList2.Visible = false;
                        BranchCheckBoxList1.Items.Clear();
                        BranchCheckBoxList1.DataBind();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select Branch from MentorMaster where Mentor='" + TextBox1.Text.Trim() + "'";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ListItem chk = BranchCheckBoxList1.Items.FindByValue(dr[0].ToString());
                                chk.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        BranchDropDownList2.Visible = true;
                        BranchCheckBoxList1.Visible = false;
                        BranchDropDownList2.Text = dr[4].ToString();
                    }

                    Button3.Enabled = false;
                    Button6.Enabled = false;
                    Button5.Enabled = true;
                    Button4.Enabled = true;
                }
            }
            else
            {
                Label4.Text = "User Entry Not Found..!";
            }
            dr.Close();
            conn.Close();
            Button1.Text = "New";
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            conn.Open();

            cmd = new SqlCommand("Select max(userid) from UserMaster", conn);
            int max = (int)cmd.ExecuteScalar();
            if (userid < max)
            {
                userid++;
                cmd = new SqlCommand("Select * from UserMaster where userid=" + userid, conn);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Label6.Text = dr[0].ToString();
                        TextBox1.Text = dr[1].ToString();
                        TextBox2.Text = dr[2].ToString();
                        DropDownList1.SelectedValue = dr[3].ToString();
                        BranchDropDownList2.Text = dr[4].ToString();
                        EmailTextbox.Text = dr[6].ToString();
                        if (DropDownList1.SelectedValue.Equals("Mentor"))
                        {
                            dr.Close();
                            BranchCheckBoxList1.Visible = true;
                            BranchDropDownList2.Visible = false;
                            BranchCheckBoxList1.Items.Clear();
                            BranchCheckBoxList1.DataBind();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select Branch from MentorMaster where Mentor='" + TextBox1.Text.Trim() + "'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    ListItem chk = BranchCheckBoxList1.Items.FindByValue(dr[0].ToString());
                                    chk.Selected = true;
                                }
                            }
                        }
                        else
                        {
                            BranchDropDownList2.Visible = true;
                            BranchCheckBoxList1.Visible = false;
                            BranchDropDownList2.Text = dr[4].ToString();
                        }

                       // ChekMF.Checked = (bool)dr[4];
                        if (userid == max)
                        {
                            Label4.Text = "Last User Entry..!";
                            Button3.Enabled = false;
                            Button6.Enabled = false;
                            Button5.Enabled = true;
                            Button4.Enabled = true;
                        }
                        else
                        {
                            Button3.Enabled = true;
                            Button6.Enabled = true;
                            Button5.Enabled = true;
                            Button4.Enabled = true;
                        }
                    }
                }
                else
                {
                    Label4.Text = "User Entry Not Found..!";
                }
            }
            else
            {
                Label4.Text = "Last User Entry..!";

                Button3.Enabled = false;
                Button6.Enabled = false;
                Button5.Enabled = true;
                Button4.Enabled = true;
            }

            dr.Close();
            conn.Close();
            Button1.Text = "New";
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            conn.Open();

            cmd = new SqlCommand("Select min(userid) from UserMaster", conn);
            int min = (int)cmd.ExecuteScalar();
            if (userid > min)
            {
                userid--;
                cmd = new SqlCommand("Select * from UserMaster where userid=" + userid, conn);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Label6.Text = dr[0].ToString();
                        TextBox1.Text = dr[1].ToString();
                        TextBox2.Text = dr[2].ToString();
                        DropDownList1.SelectedValue = dr[3].ToString();
                        BranchDropDownList2.Text = dr[4].ToString();

                        EmailTextbox.Text = dr[6].ToString();
                        if (DropDownList1.SelectedValue.Equals("Mentor"))
                        {
                            dr.Close();
                            BranchCheckBoxList1.Visible = true;
                            BranchDropDownList2.Visible = false;
                            BranchCheckBoxList1.Items.Clear();
                            BranchCheckBoxList1.DataBind();
                            cmd = conn.CreateCommand();
                            cmd.CommandText = "select Branch from MentorMaster where Mentor='" + TextBox1.Text.Trim() + "'";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    ListItem chk = BranchCheckBoxList1.Items.FindByValue(dr[0].ToString());
                                    chk.Selected = true;
                                }
                            }
                        }
                        else
                        {
                            BranchDropDownList2.Visible = true;
                            BranchCheckBoxList1.Visible = false;
                            BranchDropDownList2.Text = dr[4].ToString();
                        }

                      //  ChekMF.Checked = (bool)dr[4];
                        if (userid == min)
                        {

                            Label4.Text = "First User Entry..!";

                            Button3.Enabled = true;
                            Button6.Enabled = true;
                            Button5.Enabled = false;
                            Button4.Enabled = false;

                        }
                        else
                        {
                            Button3.Enabled = true;
                            Button6.Enabled = true;
                            Button5.Enabled = true;
                            Button4.Enabled = true;
                        }
                    }
                }
                else
                {
                    Label4.Text = "User Entry Not Found..!";
                }

                dr.Close();


            }
            else
            {
                Label4.Text = "First User Entry..!";

                Button3.Enabled = true;
                Button6.Enabled = true;
                Button5.Enabled = false;
                Button4.Enabled = false;
            }

            conn.Close();
            Button1.Text = "New";
        }

        protected void Button7_Click(object sender, EventArgs e)
        {if(TextBox1.Text!="")
        {
            conn.Open();
            cmd = new SqlCommand("Select min(userid) from UserMaster", conn);
            int min = (int)cmd.ExecuteScalar();
            cmd = new SqlCommand("Select max(userid) from UserMaster", conn);
            int max = (int)cmd.ExecuteScalar();
            cmd = new SqlCommand("Select * from UserMaster where username='" + TextBox1.Text+"'", conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Label6.Text = dr[0].ToString();
                    userid = (int)dr[0];
                    TextBox1.Text = dr[1].ToString();
                    TextBox2.Text = dr[2].ToString();
                    DropDownList1.SelectedValue = dr[3].ToString();
                    EmailTextbox.Text = dr[6].ToString();
                    if (DropDownList1.SelectedValue.Equals("Mentor"))
                    {
                        dr.Close();
                        BranchCheckBoxList1.Visible = true;
                        BranchDropDownList2.Visible = false;
                        BranchCheckBoxList1.Items.Clear();
                        BranchCheckBoxList1.DataBind();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "select Branch from MentorMaster where Mentor='" + TextBox1.Text.Trim() + "'";
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ListItem chk = BranchCheckBoxList1.Items.FindByValue(dr[0].ToString());
                                chk.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        BranchDropDownList2.Visible = true;
                        BranchCheckBoxList1.Visible = false;
                    BranchDropDownList2.Text = dr[4].ToString();
                    }

                   
                  
                    if (userid == min)
                    {

                        Label4.Text = "First User Entry..!";

                        Button3.Enabled = true;
                        Button6.Enabled = true;
                        Button5.Enabled = false;
                        Button4.Enabled = false;

                    }
                    else if (userid == max)
                    {
                        Label4.Text = "Last User Entry..!";

                        Button3.Enabled = false;
                        Button6.Enabled = false;
                        Button5.Enabled = true;
                        Button4.Enabled = true;
                    }

                }
            }
            else
            {
                Label4.Text = "User Entry Not Found..!";
            }
            dr.Close();
            conn.Close();
        }
        else
    {
        Label4.Text = "Enetr User Name..!";
    }
        Button1.Text = "New";
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            conn.Open();
            cmd = new SqlCommand("Select * from UserMaster where userid="+Label6.Text, conn);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {

                dr.Close();
                if (TextBox1.Text != "" && TextBox2.Text != "" && EmailTextbox.Text!="")
                {
                    cmd = new SqlCommand("update UserMaster set password='" + TextBox2.Text.Trim() + "',role='" + DropDownList1.SelectedItem.Text + "' , username='" + TextBox1.Text + "',Branch='" + BranchDropDownList2.Text + "', MailID='"+EmailTextbox.Text+"' where userid=" + Label6.Text, conn);
                    cmd.ExecuteNonQuery();
                  /************/
                    if (DropDownList1.SelectedValue.Equals("Mentor"))
                    {
                       
                        //BranchCheckBoxList1.Visible = true;
                        //BranchDropDownList2.Visible = false;
                       // BranchCheckBoxList1.Items.Clear();
                      //  BranchCheckBoxList1.DataBind();
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "delete   from MentorMaster where Mentor='" + TextBox1.Text.Trim() + "'";
                        cmd.ExecuteNonQuery();
                        foreach (ListItem chk in BranchCheckBoxList1.Items)
                        {
                            if (chk.Selected == true)
                            {
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "insert into MentorMaster(Mentor,Branch) values(@Mentor,@Branch)";
                                cmd.Parameters.AddWithValue("Mentor", TextBox1.Text.Trim());
                                cmd.Parameters.AddWithValue("Branch", chk.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        //if (dr.HasRows)
                        //{
                        //    while (dr.Read())
                        //    {
                        //        ListItem chk = BranchCheckBoxList1.Items.FindByValue(dr[0].ToString());
                        //        chk.Selected = true;
                        //    }
                        //}
                    }
                    //else
                    //{
                    //    BranchDropDownList2.Visible = true;
                    //    BranchCheckBoxList1.Visible = false;
                    //    BranchDropDownList2.Text = dr[4].ToString();
                    //}

                    /***/
                    Label4.Text = "User Entry Modified Successfully..!";
                }
                else
                {
                    Label4.Text = "Fill All Values";
                }
            }
            else
            {
                Label4.Text = "User Entry Not Found..!";

            }
            dr.Close();
            conn.Close(); 
            Button1.Text = "New";
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin.aspx");
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedValue.Equals("Mentor"))
            {
                BranchCheckBoxList1.Visible = true;
                BranchDropDownList2.Visible = false;
            }
            else
            {
                BranchCheckBoxList1.Visible = false;
                BranchDropDownList2.Visible = true;
            }

        }
    }
}
