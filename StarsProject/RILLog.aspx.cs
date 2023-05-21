using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class RILLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtprice = new DataTable();
            dtprice = BAL.CommonMgmt.GetRILPrice();
            rptRILPrice.DataSource = dtprice;
            rptRILPrice.DataBind();
        }

        protected void rptRILPrice_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
    }
}