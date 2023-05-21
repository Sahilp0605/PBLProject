using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SignOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int ReturnCode;
            string ReturnMsg;
            Entity.UserLog objUserLog = new Entity.UserLog();
            objUserLog.pkID = 0;
            objUserLog.UserID = Session["LoginUserID"].ToString();
            objUserLog.INOUT = "OUT";
            objUserLog.MacID = System.Environment.MachineName;
            BAL.CommonMgmt.AddUpdateUserLog(objUserLog, out ReturnCode, out ReturnMsg);
            // -----------------------------------------------------------------------------
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();

            Response.Redirect("Default.aspx", false);
        }
    }
}