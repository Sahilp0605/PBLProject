using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myProjectStatus : System.Web.UI.UserControl
    {

        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnSerialKey.Value = Session["SerialKey"].ToString();
            hdnRole.Value = objAuth.RoleCode.ToLower();
            hdnLoginUserID.Value = objAuth.LoginUserID;
            hdnClientUrl.Value = Request.Url.AbsoluteUri.ToLower();
            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                BindOrders(pageApprovalStatus, pageProjectStage);
            }

        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }
        public string pageMonth
        {
            get { return hdnMonth.Value; }
            set { hdnMonth.Value = value; }
        }

        public string pageYear
        {
            get { return hdnYear.Value; }
            set { hdnYear.Value = value; }
        }

        public String pageApprovalStatus
        {
            get { return hdn_ApprovalStatus.Value; }
            set { hdn_ApprovalStatus.Value = value; }
        }

        public String pageProjectStage
        {
            get { return hdn_ProjectStage.Value; }
            set { hdn_ProjectStage.Value = value; }
        }

        public void BindOrders(String pApprovalStatus, String pProjectStage)
        {

            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            rptApproval.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListBYProjectStatus(Session["LoginUserID"].ToString(), pProjectStage, pApprovalStatus);
            rptApproval.DataBind();
        }

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                ddl.DataSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("SOApproval");
                ddl.DataValueField = "InquiryStatusName";
                ddl.DataTextField = "InquiryStatusName";
                ddl.DataBind();
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                // ---------------------------------------------------------
                DropDownList dd2 = ((DropDownList)e.Item.FindControl("drpProjectStage"));
                dd2.DataSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("ProjectStatus");
                dd2.DataValueField = "InquiryStatusName";
                dd2.DataTextField = "InquiryStatusName";
                dd2.DataBind();
                dd2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                // ---------------------------------------------------------
                HiddenField tmpField11 = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                HiddenField tmpField12 = ((HiddenField)e.Item.FindControl("hdnProjectStage"));

                if (!String.IsNullOrEmpty(tmpField11.Value))
                    ddl.SelectedValue = tmpField11.Value;

                if (!String.IsNullOrEmpty(tmpField12.Value))
                    dd2.SelectedValue = tmpField12.Value;

                //HiddenField hdnremarks = ((HiddenField)e.Item.FindControl("hdnStatusRemarks"));
                //TextBox remarks = ((TextBox)e.Item.FindControl("txtRemarks"));

                //remarks.Text = hdnremarks.Value;
                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];

                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));
                HiddenField hdnStatusRemarks = ((HiddenField)e.Item.FindControl("hdnStatusRemarks"));

                if (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                    ddl.Attributes["disabled"] = "disabled";
                if (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                    dd2.Attributes["disabled"] = "disabled";
            }
        }

        protected void rptApproval_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "SaveLog")
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                HiddenField hdnID = (HiddenField)e.Item.FindControl("hdnpkID");
                HiddenField hdnOrderNo = (HiddenField)e.Item.FindControl("hdnOrderNo");
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                DropDownList dd2 = ((DropDownList)e.Item.FindControl("drpProjectStage"));
                TextBox txtRemarks = ((TextBox)e.Item.FindControl("txtRemarks"));
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnID.Value);
                    objEntity.OrderNo = hdnOrderNo.Value;
                    objEntity.ApprovalStatus = ddl.SelectedValue;
                    objEntity.ProjectStage = dd2.SelectedValue;
                    objEntity.StatusRemarks = txtRemarks.Text;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
                // --------------------------------------------------------
                BindOrders(pageApprovalStatus, pageProjectStage);
            }
            else
            {
          //  javascript: viewSalesOrderLog('<%# Eval("OrderNo")%>'); 
            }
        }
    }
}