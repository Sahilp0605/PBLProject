using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Dealer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:show_loader();", true);
            if (!IsPostBack)
            {
                //setLicenseInformation();
            }
            //loadBroadcastMessage();
            //loadActivity();
            //loadNotifications();
            //BindChatUserList();
            //// -----------------------------------------------------------------------
            if (Session["logindetail"] == null)
                Response.Redirect("Default.aspx");
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            //--------------------------------------------------------------------------
            //if (objAuth.RoleCode.ToLower() != "admin")
            //    lnkCompProfile.Visible = false;
            //-------------------------------------------------------------------------
            if (objAuth.RoleCode == null)
                Response.Redirect("Default.aspx");
            // -----------------------------------------------------------------------
            string URL = Request.Url.AbsoluteUri.ToLower();
            //ltrLocationName.InnerText = objAuth.CompanyType + " - " + objAuth.CompanyName;
            lblUser.Text = objAuth.UserID;
            lblUserTitle.InnerText = objAuth.EmployeeName;
            lblRoleName.Text = objAuth.RoleName;
            //imgProfile.Src = (!String.IsNullOrEmpty(objAuth.EmployeeImage)) ? objAuth.EmployeeImage : "images/customer.png";
            //imgProfileSmall.Src = (!String.IsNullOrEmpty(objAuth.EmployeeImage)) ? objAuth.EmployeeImage : "images/customer.png";

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:setTimeout(hide_loader(), 50000);", true);
        }
    }
}