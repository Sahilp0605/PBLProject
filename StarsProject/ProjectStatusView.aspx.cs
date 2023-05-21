using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProjectStatusView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnSerialKey.Value = Session["SerialKey"].ToString();
                BindApprovalStatus();
                BindProjectStatus();
                // -----------------------------------------------------
                ucProjectStatus.pageApprovalStatus = drpApprovalStatus.SelectedValue;
                ucProjectStatus.pageProjectStage = drpProjectStage.SelectedValue;
                // -----------------------------------------------------
                ucProjectStatus.BindOrders("", "");
            }
        }

        // ------------------------------------------------------------------------------
        // Sales Order Approval Status - SOApproval
        // ------------------------------------------------------------------------------
        void BindApprovalStatus()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("SOApproval");
            drpApprovalStatus.DataSource = lstDesig;
            drpApprovalStatus.DataValueField = "InquiryStatusName";
            drpApprovalStatus.DataTextField = "InquiryStatusName";
            drpApprovalStatus.DataBind();
            drpApprovalStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            drpApprovalStatus_SelectedIndexChanged(null, null);

        }

        protected void drpApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucProjectStatus.BindOrders(drpApprovalStatus.SelectedValue, drpProjectStage.SelectedValue);
        }

        // ------------------------------------------------------------------------------
        // Govt. Project Status - ProjectStatus
        // ------------------------------------------------------------------------------
        void BindProjectStatus()
        {
            List<Entity.InquiryStatus> lstDesig1 = new List<Entity.InquiryStatus>();
            lstDesig1 = BAL.InquiryStatusMgmt.GetInquiryStatusList("ProjectStatus");
            drpProjectStage.DataSource = lstDesig1;
            drpProjectStage.DataValueField = "InquiryStatusName";
            drpProjectStage.DataTextField = "InquiryStatusName";
            drpProjectStage.DataBind();
            drpProjectStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            drpProjectStage_SelectedIndexChanged(null, null);

        }
        protected void drpProjectStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucProjectStatus.BindOrders(drpApprovalStatus.SelectedValue, drpProjectStage.SelectedValue);
        }
    }
}