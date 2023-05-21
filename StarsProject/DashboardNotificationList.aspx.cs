using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Globalization;

namespace StarsProject
{
    public partial class DashboardNotificationList : System.Web.UI.Page
    {
        //bool _pageValid = true;
        //string _pageErrMsg;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        // --------------------------------------------------------------------------------------------------
        //        Session["PageNo"] = 1;
        //        Session["PageSize"] = 100;
        //        Session["OldUserID"] = "";
        //        // ----------------------------------------------------------------------
        //        // Retrieving Dispatch Notification list of Email & Mobile
        //        // ----------------------------------------------------------------------
        //        hdnHelpOrgCode.Value = Request.QueryString["OrgCodeList"].ToString();
        //        hdnHelpPkID.Value = Request.QueryString["TicketID"].ToString();
        //        //List<Entity.DispatchNotification> lstEntity = new List<Entity.DispatchNotification>();
        //        //lstEntity = BAL.HelpLogMgmt.GetDispatchNotificationInfo(hdnHelpOrgCode.Value, Convert.ToInt64(hdnHelpPkID.Value));

        //        rptNotification.DataSource = lstEntity;
        //        rptNotification.DataBind();

        //        // ----------------------------------------------------------------------
        //        // Storing HiddenField values for Single Help Log ID & Other Information 
        //        // ----------------------------------------------------------------------
        //        //List<Entity.HelpLog> lstEntity1 = new List<Entity.HelpLog>();
        //        //if (!String.IsNullOrEmpty(hdnHelpPkID.Value))
        //        //{
        //        //    lstEntity1 = BAL.HelpLogMgmt.GetHelpLogListByPkID(Convert.ToInt64(hdnHelpPkID.Value));

        //        //    hdnHelpRegistrationNo.Value = lstEntity1[0].RegistrationNo;
        //        //    hdnHelpMemberName.Value = lstEntity1[0].MemberName;
        //        //    hdnHelpLatitude.Value = lstEntity1[0].Latitude.ToString();
        //        //    hdnHelpLongitude.Value = lstEntity1[0].Longitude.ToString();
        //        //}
        //    }
        //}
        //protected void rptNotification_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName.ToString() == "SendNotification")
        //    {
        //        int ReturnCode = 0;
        //        string ReturnMsg = "";

        //        // ----------------------------------------------------------------------
        //        // Retrieving Dispatch Notification list of Email & Mobile
        //        // ----------------------------------------------------------------------
        //        foreach (RepeaterItem repeaterItem in rptNotification.Items)
        //        {
        //            HiddenField hdnHelpLogID1 = repeaterItem.FindControl("hdn_HelpLogID") as HiddenField;
        //            HiddenField hdnKeyCode1 = repeaterItem.FindControl("hdn_KeyCode") as HiddenField;
        //            HiddenField hdnOrgCode1 = repeaterItem.FindControl("hdn_OrgCode") as HiddenField;
        //            HiddenField hdnOrgName1 = repeaterItem.FindControl("hdn_OrgName") as HiddenField;
        //            HiddenField hdnOrgDepartmentCode1 = repeaterItem.FindControl("hdn_OrgDepartmentCode") as HiddenField;
        //            HiddenField hdnOrgDepartmentName1 = repeaterItem.FindControl("hdn_OrgDepartmentName") as HiddenField;
        //            Label lblRegistrationNo1 = repeaterItem.FindControl("lbl_RegistrationNo") as Label;
        //            Label lblMemberName1 = repeaterItem.FindControl("lbl_MemberName") as Label;
        //            Label lblEmailAddress1 = repeaterItem.FindControl("lbl_EmailAddress") as Label;
        //            Label lblMobileNo1 = repeaterItem.FindControl("lbl_MobileNo") as Label;
        //            Label lblLandline1 = repeaterItem.FindControl("lbl_Landline") as Label;
        //            Label lblRelationType1 = repeaterItem.FindControl("lbl_RelationType") as Label;
        //            Label lblDispatchCategory1 = repeaterItem.FindControl("lbl_DispatchCategory") as Label;

        //            Entity.DispatchNotification lstEntity = new Entity.DispatchNotification();

        //            lstEntity.HelpLogID = Convert.ToInt64(hdnHelpLogID1.Value);
        //            lstEntity.keyCode = hdnKeyCode1.Value;
        //            lstEntity.RegistrationNo = lblRegistrationNo1.Text;
        //            lstEntity.MemberName = lblMemberName1.Text;
        //            lstEntity.EmailAddress = lblEmailAddress1.Text;
        //            lstEntity.MobileNo = lblMobileNo1.Text;
        //            lstEntity.Landline = lblLandline1.Text;
        //            lstEntity.DispatchCategory = lblDispatchCategory1.Text;
        //            lstEntity.OrgCode = hdnOrgCode1.Value;
        //            lstEntity.OrgName = hdnOrgName1.Value;
        //            lstEntity.OrgDepartmentCode = hdnOrgDepartmentCode1.Value;
        //            lstEntity.OrgDepartmentName = hdnOrgDepartmentName1.Value;
        //            // ------------------------------------------------------------------------------------------
        //            // Below code is used to Send Email Notification 
        //            // ------------------------------------------------------------------------------------------
        //            string retMsg;
        //            if (!String.IsNullOrEmpty(lstEntity.EmailAddress))
        //            {
        //                string vPlace;
        //                vPlace = BAL.CommonMgmt.RetrieveFormattedAddress(hdnHelpLatitude.Value, hdnHelpLongitude.Value);

        //                retMsg = BAL.CommonMgmt.SendAlertNotificationEmail("103", lstEntity.EmailAddress, hdnHelpOrgCode.Value, lblRelationType1.Text, lstEntity.OrgCode,  lstEntity.OrgName, lstEntity.OrgDepartmentCode, lstEntity.OrgDepartmentName, lstEntity.MemberName, hdnHelpPkID.Value, hdnHelpMemberName.Value, hdnHelpLatitude.Value, hdnHelpLongitude.Value, vPlace);
        //                divMessage.InnerText = retMsg;
        //            }
        //            // ------------------------------------------------------------------------------------------
        //            BAL.HelpLogMgmt.AddUpdateDispatchNotification(lstEntity, out ReturnCode, out ReturnMsg);
        //            divMessage.InnerText = ReturnMsg;
        //        }
        //        //// ---------------------------------------------------------------------------------------------------------------
        //        //ScriptManager.RegisterStartupScript(this, typeof(string), "popDiv1", "javascript:showMyGoogleMap(" + hdnHelpLatitude.Value + "," + hdnHelpLongitude.Value + ");", true);
        //        //// ---------------------------------------------------------------------------------------------------------------
        //    }
        //}


    }
}