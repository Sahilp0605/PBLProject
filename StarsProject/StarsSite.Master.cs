using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;

namespace StarsProject
{
    public partial class StarsSite : System.Web.UI.MasterPage
    {
        DataTable dt = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["logindetail"] == null)
                Response.Redirect("Default.aspx");
            // ------------------------------------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:show_loader();", true);
            if (!IsPostBack)
            {
                setLicenseInformation();
            }

            loadBroadcastMessage(); 
            loadActivity();
            loadNotifications();
            //BindChatUserList();
            
            // -----------------------------------------------------------------------
            if (Session["logindetail"] == null)
                Response.Redirect("Default.aspx");
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            //--------------------------------------------------------------------------
            if (objAuth.RoleCode.ToLower() != "admin")
               lnkCompProfile.Visible = false;
            //-------------------------------------------------------------------------
            if (objAuth.RoleCode == null)
                Response.Redirect("Default.aspx");
            // -----------------------------------------------------------------------
            string URL = Request.Url.AbsoluteUri.ToLower();
            ltrLocationName.InnerText = objAuth.CompanyType + " - " + objAuth.CompanyName;
            lblUser.Text = objAuth.UserID;
            lblUserTitle.InnerText = objAuth.ScreenFullName;
            lblRoleName.Text = objAuth.RoleName;
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:setTimeout(hide_loader(), 50000);", true);
            // ----------------------------------------------------------------------------
            if (hdnMasterSerialKey.Value == "AIRI-G3Y5-2T9E-YN9W")      // For Ambani
            {
                if (objAuth.RoleCode.ToLower() != "admin" && objAuth.RoleCode.ToLower() != "bradmin" && objAuth.RoleCode.ToLower() != "nsm")
                {
                    //lnkFooterDashboardDaily.Visible = false;
                    //lnkFooterDashboardLead .Visible = false;
                    //lnkFooterDashboardAccount.Visible = false;
                    //lnkFooterDashboardInventory.Visible = false;
                    //lnkFooterDashboardSupport.Visible = false;
                    //lnkFooterTrackEmployee.Visible = false;
                    //lnkFooterSearchCustomer.Visible = false;
                    //lnkFooterSearchEmployee.Visible = false;
                }
            }
        }

        //protected void Page_LoadComplete(object sender, EventArgs e)
        //{
        //    //Work and It will assign the values to label.  
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:hide_loader()", true);
        //}

        //protected override void OnPreRender(EventArgs e)
        //{
        //    //Work and It will assign the values to label.  
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:hide_loader()", true);
        //}

        //protected void Page_UnLoad(object sender, EventArgs e)
        //{
        //    //Work and it will not effect label contrl, view stae and post back data.  
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:hide_loader()", true);
        //}
        
        // ==========================================================

        public void loadBroadcastMessage()
        {
            String tmpVal = BAL.CommonMgmt.GetBroadcastMessage(Session["LoginUserID"].ToString());
            spnBroadcastMessage.InnerHtml = tmpVal;
            divBroadcast.Visible = (tmpVal.Trim().Length > 0) ? true : false;
        }

        // ==========================================================
        public void loadNotifications()
        {
            Int64 totCount = 0;
            StringBuilder sb = new StringBuilder();
            List<Entity.DashboardNotification> lstEntity = new List<Entity.DashboardNotification>();
            lstEntity = BAL.CommonMgmt.GetNotificationList(Session["LoginUserID"].ToString(), "notification");

            try
            {
                DateTime lastNotificationTimestamp = Convert.ToDateTime(BAL.CommonMgmt.lastNotificationTimestamp(Session["LoginUserID"].ToString()));
            }
            catch (Exception e)
            {

            }
            Int64 tot = 0;
            tot = lstEntity.Count;
            spnNotificationCount.InnerText = tot.ToString();

            sb.Append("<li><h6>NOTIFICATIONS<span id='spnNotificationCount' runat='server' class='new badge'>" + tot.ToString() + "</span></h6></li><li class='divider'></li>");
            foreach (var noti in lstEntity)
            {
                //sb.Append("<li><a class='black-text' href='#!'><span class='material-icons icon-bg-circle red small'>stars</span>" + noti.ModuleName.ToString() + "</a>");
                //sb.Append("<span class='media-meta grey-text darken-2' >" + noti.Description + "</span>");
                //sb.Append("</li>");
                sb.Append("<li class='timeline-items timeline-icon-brown active' Style='padding-left: 10px;padding-right: 10px;'>");
                sb.Append("<h6 class='timeline-title'><span class='material-icons icon-bg-circle red small'>stars</span>&nbsp;&nbsp;<span style='vertical-align:text-top; color:navy;'>" + noti.ModuleName + "</span>");
                sb.Append("<span class='timeline-time float-right' style='font-size: .75rem; color:maroon;'>" + noti.CreatedDate + "</span></h6>");
                sb.Append("<a class='black-text' href=" + "\"javascript:openNotification(\'" + noti.ModuleName + "\'," + noti.ModulePkID + ")\"" + ">");
                sb.Append("<p class='timeline-text' style='margin-left:35px; font-size: .75rem; color:blue;'>" + noti.Description + "</p>");
                sb.Append("</a>");
                sb.Append("</li>");
            }
            
            // ----------------------------------------------------------------
            if (tot > 0)
            {
                notificationsdropdown.InnerHtml = sb.ToString();
                
            }
            sb.Clear();
        }
        // ==========================================================
        
        // ==========================================================
        public void loadActivity()
        {
            Int64 totCount = 0;
            StringBuilder sbActivity = new StringBuilder();
            List<Entity.DashboardNotification> lstEntity = new List<Entity.DashboardNotification>();
            lstEntity = BAL.CommonMgmt.GetNotificationList(Session["LoginUserID"].ToString(), "activity");

            foreach (var actItem in lstEntity)
            {
                if (actItem.ModuleName.ToLower() == "leave request")
                    sbActivity.Append("<li class='timeline-items timeline-icon-red active'>");
                else if (actItem.ModuleName.ToLower() == "quotation")
                    sbActivity.Append("<li class='timeline-items timeline-icon-amber active'>");
                else if (actItem.ModuleName.ToLower() == "sales order")
                    sbActivity.Append("<li class='timeline-items timeline-icon-blue active'>");
                else if (actItem.ModuleName.ToLower() == "sales bill")
                    sbActivity.Append("<li class='timeline-items timeline-icon-green active'>");
                else
                    sbActivity.Append("<li class='timeline-items timeline-icon-brown active'>");

                sbActivity.Append("<div class='timeline-time'>" + actItem.CreatedDate + "</div>");
                sbActivity.Append("<h6 class='timeline-title'>" + actItem.ModuleName + "</h6>");
                sbActivity.Append("<a class='black-text' href=" + "\"javascript:openNotification(\'" + actItem.ModuleName + "\'," + actItem.ModulePkID + ")\"" + ">");
                sbActivity.Append("<p class='timeline-text'>" + actItem.Description + "</p>");
                sbActivity.Append("</a>");
                //sbActivity.Append("<div class='timeline-content orange-text'>Important</div>");
                sbActivity.Append("</li>");
            }
            // ----------------------------------------------------------------
            if (lstEntity.Count>0)
            {
                ulActivitySection.InnerHtml = sbActivity.ToString();
            }
            sbActivity.Clear();
        }

        // ==========================================================
        void setLicenseInformation()
        {
            String tmpSerialKey = Session["SerialKey"].ToString();
            List<Entity.CompanyRegistration> lstEntity = new List<Entity.CompanyRegistration>();
            lstEntity = BAL.CompanyRegistrationMgmt.GetCompanyRegistrationBySerialKey(tmpSerialKey);
            // -----------------------------------------------------------
            txtSerialKey.Text = tmpSerialKey;
            hdnMasterSerialKey.Value = tmpSerialKey;
            txtInstallationDate.Text = lstEntity[0].InstallationDate.ToString("yyyy-MM-dd");
            txtExpiryDate.Text = lstEntity[0].ExpiryDate.ToString("yyyy-MM-dd");
            txtLicenseUsers.Text = lstEntity[0].NoOfUsers.ToString();
            txtExistingUsers.Text = BAL.CommonMgmt.GetNoOfUsers().ToString();

            Session["LicenseUsers"] = txtLicenseUsers.Text;
            Session["ExistingUsers"] = txtExistingUsers.Text;
        }

        public void BindChatUserList()
        {
            //List<Entity.Chat> lstUserList = new List<Entity.Chat>();
            //lstUserList = BAL.CommonMgmt.GetChatBoxUserList(Session["LoginUserID"].ToString());
            //rptMasterUserList.DataSource = lstUserList;
            //rptMasterUserList.DataBind();
        }

        //protected void rptMasterUserList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        HtmlGenericControl spnUnread = ((HtmlGenericControl)e.Item.FindControl("spnUnread"));
        //        if (spnUnread.InnerText == "" || spnUnread.InnerText == "0")
        //        {
        //            spnUnread.Attributes.Remove("class");
        //            spnUnread.Attributes.Add("class", "badge pill white");
        //        }
        //        else
        //        {
        //            spnUnread.Attributes.Remove("class");
        //            spnUnread.Attributes.Add("class", "badge pill red");
        //        }

        //    }
        //}

    }
}