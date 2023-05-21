using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myMaterialIndent : System.Web.UI.UserControl
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
            btnApproveReject.Visible = (hdnClientUrl.Value.Contains("dashboarddaily") || hdnClientUrl.Value.Contains("dashboardinventory")) ? false : true;
        }
        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }
        public string pageApprovalStatus
        {
            get { return hdnStatus.Value; }
            set { hdnStatus.Value = value; }
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


        public void BindMaterialIndent(String pStatus)
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            rptApproval.DataSource = BAL.MaterialIndentMgmt.GetMaterialIndentList(pStatus, Session["LoginUserID"].ToString());
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
                Entity.MaterialIndent objEntity = new Entity.MaterialIndent();

                HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
                DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnID.Value);
                    objEntity.ApprovalStatus = ddl.SelectedValue;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.MaterialIndentMgmt.UpdateIndentApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
            }
            BindMaterialIndent(pageApprovalStatus);
        }

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));
                HiddenField hdnApprovalStatus = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));

                if (!String.IsNullOrEmpty(hdnApprovalStatus.Value))
                    ddl.SelectedValue = hdnApprovalStatus.Value;
                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // -----------------------------------------------------------------------
                if (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && (objAuth.RoleCode.ToLower() == "hradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower())))
                    ddl.Attributes["disabled"] = "disabled";
            }
        }
    }
}