using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myLeaveRequest : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();

            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                btnApproveReject.Visible = (hdnView.Value == "dashboard" || hdnView.Value == "dashboarddaily" || hdnView.Value == "dashboardhr") ? false : true;
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

        public Int64 LeaveCount
        {
            get { 
                return Convert.ToInt64(rptApproval.Items.Count); 
            }
        }

        public void BindLeaveRequest(String pStatus)
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            int totrec;
            rptApproval.DataSource = BAL.LeaveRequestMgmt.GetLeaveRequestListByStatus(pStatus, Session["LoginUserID"].ToString(), pMon, pYear, 1, 99000, out totrec);
            rptApproval.DataBind();
        }

        protected void btnApproveReject_Click(object sender, EventArgs e)
        {
            SendApprovalStatus();
        }

        public void SendApprovalStatus()
        {
            foreach (RepeaterItem i in rptApproval.Items)
            {
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();

                HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
                HiddenField ApprovalStatus = (HiddenField)i.FindControl("hdnApprovalStatus");
                DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
                DropDownList ddlPU = ((DropDownList)i.FindControl("drpPaidUnpaid"));
                HiddenField hdnEmployeeName = (HiddenField)i.FindControl("hdnEmployeeName");
                HiddenField hdnEmployeeID = (HiddenField)i.FindControl("hdnEmployeeID");

                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    if (ApprovalStatus.Value != ddl.SelectedValue)
                    {
                        objEntity.pkID = Convert.ToInt64(hdnID.Value);
                        objEntity.ApprovalStatus = ddl.SelectedValue;
                        objEntity.PaidUnpaid = ddlPU.SelectedValue;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.LeaveRequestMgmt.UpdateLeaveRequestApproval(objEntity, out ReturnCode, out ReturnMsg);

                        string notificationMsg = "";
                        notificationMsg = "Leave Request " + ddl.SelectedValue + " For " + hdnEmployeeName.Value + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                        BAL.CommonMgmt.SendNotification_Firebase("Leave Request", notificationMsg, BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64(hdnEmployeeID.Value.ToString())), Convert.ToInt64(hdnEmployeeID.Value.ToString()));
                        BAL.CommonMgmt.SendNotificationToDB("Leave Request Status", Convert.ToInt64(hdnID.Value), notificationMsg, BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64(hdnEmployeeID.Value.ToString())), Convert.ToInt64(hdnEmployeeID.Value.ToString()));
                    }
                }
            }
        }

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                if (!String.IsNullOrEmpty(tmpField.Value))
                    ddl.SelectedValue = tmpField.Value;

                DropDownList ddlPU = ((DropDownList)e.Item.FindControl("drpPaidUnpaid"));
                HiddenField tmpFieldPU = ((HiddenField)e.Item.FindControl("hdnPaidUnpaid"));
                if (!String.IsNullOrEmpty(tmpFieldPU.Value))
                    ddlPU.SelectedValue = tmpFieldPU.Value;

                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));

                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // -----------------------------------------------------------------------
                if ((hdnView.Value == "dashboard" || objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()) && (objAuth.RoleCode.ToLower() != "admin" && objAuth.RoleCode.ToLower() != "bradmin" && objAuth.RoleCode.ToLower() != "supervisor"))
                    ddl.Attributes["disabled"] = "disabled";
            }
        }

    }
}