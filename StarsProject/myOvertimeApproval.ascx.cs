using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myOvertimeApproval : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();
            hdnClientUrl.Value = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                drpApprovalStatusMain.Visible = (hdnClientUrl.Value.Contains("dashboard.aspx") || hdnClientUrl.Value.Contains("dashboarddaily.aspx") || hdnClientUrl.Value.Contains("dashboardhr.aspx")) ? false : true;
                btnApproveReject.Visible = (hdnClientUrl.Value.Contains("dashboard.aspx") || hdnClientUrl.Value.Contains("dashboarddaily.aspx") || hdnClientUrl.Value.Contains("dashboardhr.aspx")) ? false : true;
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

        public void BindOverTime(String pStatus)
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            int totrec;
            rptApproval.DataSource = BAL.OverTimeMgmt.GetOverTimeListByStatus(pStatus, Session["LoginUserID"].ToString(), pMon, pYear, 1, 99000, out totrec);
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
                Entity.OverTime objEntity = new Entity.OverTime();

                HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
                DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnID.Value);
                    objEntity.ApprovalStatus = ddl.SelectedValue;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.OverTimeMgmt.UpdateOverTimeApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
            }
        }

        protected void drpApprovalStatusMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOverTime(drpApprovalStatusMain.SelectedValue);
        }

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                if (!String.IsNullOrEmpty(tmpField.Value))
                    ddl.SelectedValue = tmpField.Value;
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