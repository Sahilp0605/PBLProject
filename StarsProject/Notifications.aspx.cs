using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                myNotifications.loadNotifications();
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void UpdateUserTimeStamp()
        {
            //List<Entity.CommonParameters> lstEntity = new List<Entity.FinancialTrans>();
            ////// --------------------------------------------------------------------------
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //// --------------------------------------------d------------------------------
            //lstEntity = BAL.FinancialTransMgmt.GetFinancialTransList(Session["LoginUserID"].ToString());
            BAL.CommonMgmt.UpdateUserTimeStamp(HttpContext.Current.Session["LoginUserID"].ToString(), HttpContext.Current.Session["CompanyID"].ToString());

            //return serializer.Serialize(lstEntity);
        }
       

    }
}