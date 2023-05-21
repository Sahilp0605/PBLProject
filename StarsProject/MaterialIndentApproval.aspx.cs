using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MaterialIndentApproval : System.Web.UI.Page
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                myMaterialIndent.BindMaterialIndent(drpApprovalStatus.SelectedValue);
        }

        protected void drpApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            myMaterialIndent.BindMaterialIndent(drpApprovalStatus.SelectedValue);
        }
    }
}