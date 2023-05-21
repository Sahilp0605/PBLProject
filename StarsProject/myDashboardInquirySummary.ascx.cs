using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using StarsProject.Common;
using System.IO;

namespace StarsProject
{
    public partial class myDashboardInquirySummary : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        public void BindGraph(string loginuserid, String Category)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "grphcontrol", "javascript:initInqGraphLayout('" + Category + "');", true);
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }
        public string pageMonth
        {
            get { return hdnMonth.Value; }
            set { hdnMonth.Value = value; }
        }
        public string pageYear
        {
            get { return hdnYear.Value; }
            set { hdnYear.Value = value; }
        }

    }
}