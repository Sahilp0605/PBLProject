using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class LeaveApprovalView : System.Web.UI.Page
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                myLeaveRequest.BindLeaveRequest("");
        }
    }
}