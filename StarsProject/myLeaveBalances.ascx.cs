using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myLeaveBalances : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();
            // -----------------------------------------------------------------------
        }
        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }
        public string pageEmployeeID
        {
            get { return hdnEmployeeID.Value; }
            set { hdnEmployeeID.Value = value; }
        }
        public string pageLeaveTypeID
        {
            get { return hdnLeaveTypeID.Value; }
            set { hdnLeaveTypeID.Value = value; }
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
        public void BindLeaveBalances()
        {
            Int64 pEmployeeID, pLeaveTypeID;
            pEmployeeID = (!String.IsNullOrEmpty(hdnEmployeeID.Value)) ? Convert.ToInt64(hdnEmployeeID.Value) : 0;
            pLeaveTypeID = (!String.IsNullOrEmpty(hdnLeaveTypeID.Value)) ? Convert.ToInt64(hdnLeaveTypeID.Value) : 0;

            rptBalances.DataSource = BAL.LeaveRequestMgmt.GetEmployeeLeaveBalance(pEmployeeID, pLeaveTypeID, Session["LoginUserID"].ToString());
            rptBalances.DataBind();
        }
        protected void rptBalances_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                //HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                //if (!String.IsNullOrEmpty(tmpField.Value))
                //    ddl.SelectedValue = tmpField.Value;

                //DropDownList ddlPU = ((DropDownList)e.Item.FindControl("drpPaidUnpaid"));
                //HiddenField tmpFieldPU = ((HiddenField)e.Item.FindControl("hdnPaidUnpaid"));
                //if (!String.IsNullOrEmpty(tmpFieldPU.Value))
                //    ddlPU.SelectedValue = tmpFieldPU.Value;

                //HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
            }
        }

    }
}