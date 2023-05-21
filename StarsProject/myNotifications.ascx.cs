using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myNotifications : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack )
            {
                loadNotifications();
            }
        }
        public void loadNotifications()
        {
            Int64 TotCount = 0;
            //StringBuilder sb = new StringBuilder();
            List<Entity.DashboardNotification> lstEntity = new List<Entity.DashboardNotification>();
            lstEntity = BAL.CommonMgmt.GetNotificationList(Session["LoginUserID"].ToString(),"days");
            rptNotification.DataSource = lstEntity;
            rptNotification.DataBind();
            Int64 tot = 0;
            tot = lstEntity.Count;
           

        }
    }
}