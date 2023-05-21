using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myPurchaseApproval : System.Web.UI.UserControl
    {

        int ReturnCode = 0;
        string ReturnMsg = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();
            hdnLoginUserID.Value = objAuth.LoginUserID;
            hdnClientUrl.Value = Request.Url.AbsoluteUri.ToLower();
            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                btnApproveReject.Visible = (hdnView.Value == "dashboard") ? false : true;
                divApprovalStatus.Visible = (hdnView.Value == "dashboard") ? false : true;
                BindOrders(drpApprovalStatus1.SelectedValue);
                BindPOStatus();
            }
        }
        void BindPOStatus()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("POApproval");
            drpApprovalStatus1.DataSource = lstDesig;
            drpApprovalStatus1.DataValueField = "InquiryStatusName";
            drpApprovalStatus1.DataTextField = "InquiryStatusName";
            drpApprovalStatus1.DataBind();
            drpApprovalStatus1_SelectedIndexChanged(null, null);

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

        public void BindOrders(String pStatus)
        {

            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            rptApproval.DataSource = BAL.PurchaseOrderMgmt.GetPurchaseOrderList(pStatus, Session["LoginUserID"].ToString(), pMon, pYear);
            rptApproval.DataBind();
        }

        protected void drpApprovalStatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrders(drpApprovalStatus1.SelectedValue);
        }

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));

                //DropDownList selectList = e.Item.FindControl("drpApprovalStatus") as DropDownList;
                ddl.DataSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("POApproval");
                ddl.DataValueField = "InquiryStatusName";
                ddl.DataTextField = "InquiryStatusName";
                ddl.DataBind();

                if (!String.IsNullOrEmpty(tmpField.Value))
                    ddl.SelectedValue = tmpField.Value;
                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // -----------------------------------------------------------------------
                //if ((hdnView.Value == "dashboard" || objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()) && (objAuth.RoleCode.ToLower() != "admin" && objAuth.RoleCode.ToLower() != "bradmin" && objAuth.RoleCode.ToLower() != "supervisor"))
                //    ddl.Attributes["disabled"] = "disabled";
                if (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                    ddl.Attributes["disabled"] = "disabled";
            }
        }

        protected void btnApproveReject_Click(object sender, EventArgs e)
        {
            SendApprovalStatus();
        }
        

        public void SendApprovalStatus()
        {
            foreach (RepeaterItem i in rptApproval.Items)
            {
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

                HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
                HiddenField hdnOrderNo = (HiddenField)i.FindControl("hdnOrderNo");
                DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnID.Value);
                    objEntity.OrderNo = hdnOrderNo.Value;
                    objEntity.ApprovalStatus = ddl.SelectedValue;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    if (hdnOrderNo.Value.Contains("DL") == true)    // Dealer Sales Order Approval 
                        BAL.PurchaseOrderMgmt.UpdatePurchaseOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                    else                                            // General Sales Order Approval 
                        BAL.PurchaseOrderMgmt.UpdatePurchaseOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
            }
            BindOrders(drpApprovalStatus1.SelectedValue);
        }
    }
}