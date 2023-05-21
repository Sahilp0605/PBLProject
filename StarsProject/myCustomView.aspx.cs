using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myCustomView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnLoginUserID.Value = Session["LoginUserID"].ToString();
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();
            // --------------------------------------------------------------------------
            // Fetching Querystring Parameters
            // --------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(Request.QueryString["view"]))
                hdnView.Value = Request.QueryString["view"].ToString().Trim();

            if (!String.IsNullOrEmpty(Request.QueryString["para1"]))
                hdnPara1.Value = Request.QueryString["para1"].ToString().Trim();
            
            if (!String.IsNullOrEmpty(Request.QueryString["para2"]))
                hdnPara2.Value = Request.QueryString["para2"].ToString().Trim();

            if (!String.IsNullOrEmpty(Request.QueryString["para3"]))
                hdnPara3.Value = Request.QueryString["para3"].ToString().Trim();

            if (!String.IsNullOrEmpty(Request.QueryString["para4"]))
                hdnPara4.Value = Request.QueryString["para4"].ToString().Trim();

            if (!String.IsNullOrEmpty(Request.QueryString["para5"]))
                hdnPara5.Value = Request.QueryString["para5"].ToString().Trim();
            // --------------------------------------------------------------------------
            // Fetching Constant Values 
            // --------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("InquiryShare", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                hdnInquiryShare.Value = BAL.CommonMgmt.GetConstant("InquiryShare", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

        }
    }
}