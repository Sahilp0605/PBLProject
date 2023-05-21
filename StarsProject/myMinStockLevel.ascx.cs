using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myMinStockLevel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindMinimumStock()
        {
            rptMinStock.DataSource = BAL.ReportMgmt.ProductMinStockList("","", Session["LoginUserID"].ToString());
            rptMinStock.DataBind();
        }
    }
}