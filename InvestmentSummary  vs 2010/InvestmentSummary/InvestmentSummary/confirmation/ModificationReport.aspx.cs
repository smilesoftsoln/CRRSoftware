using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InvestmentSummary.confirmation
{
    public partial class ModificationReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetReportButton1_Click(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }
    }
}