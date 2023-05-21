using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class viewTaxSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["module"]))
                {
                    hdnMainModule.Value = Request.QueryString["module"].ToString();
                    myTaxSummary.Module = Request.QueryString["module"].ToString();
                }


                if (!String.IsNullOrEmpty(Request.QueryString["keyid"]))
                {
                    hdnMainKeyID.Value = Request.QueryString["keyid"].ToString();
                    myTaxSummary.KeyID = Request.QueryString["keyid"].ToString();
                }
                myTaxSummary.BindTaxSummaryWidget("", "");
            }
        }
        protected void drpHSNFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            myTaxSummary.Module = hdnMainModule.Value;
            myTaxSummary.KeyID = hdnMainKeyID.Value;
            myTaxSummary.HSNFlag = drpHSNFlag.SelectedValue;
            myTaxSummary.BindTaxSummaryWidget("", "");
        }
    }
}