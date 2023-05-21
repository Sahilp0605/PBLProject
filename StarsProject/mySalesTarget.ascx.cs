using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class mySalesTarget : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnApproveReject.Visible = (hdnView.Value == "dashboard") ? false : true;
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
        public void BindSalesTarget(string loginuserid,String TargetType)
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            int totrec;
            rptApproval.DataSource = BAL.SalesTargetMgmt.GetSalesTargetListByTargetType(loginuserid, 0, pMon, pYear, TargetType, 1, 99000, out totrec);
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
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
                DropDownList ddl = ((DropDownList)i.FindControl("drpTargetType"));
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnID.Value);
                    objEntity.ApprovalStatus = ddl.SelectedValue;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
            }
        }

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("drpTargetType"));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnTargetType"));
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));

                //if (!String.IsNullOrEmpty(tmpField.Value))
                //    ddl.SelectedValue = tmpField.Value;
                // -----------------------------------------------------------------------
                //Entity.Authenticate objAuth = new Entity.Authenticate();
                //objAuth = (Entity.Authenticate)Session["logindetail"];
                //// -----------------------------------------------------------------------
                //if (hdnView.Value == "dashboard" || objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower())
                //    ddl.Attributes["disabled"] = "disabled";
            }
        }
    }
}