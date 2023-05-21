using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace StarsProject
{
    public partial class myDealerOrderApproval : System.Web.DynamicData.FieldTemplateUserControl
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
            // -----------------------------------------------------------------------
            if (!IsPostBack)
            {
                //btnApproveReject.Visible = (hdnView.Value == "dashboard") ? false : true;
                divApprovalStatus.Visible = (hdnView.Value == "dashboard") ? false : true;
                BindOrders(drpApprovalStatus1.SelectedValue);
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

        protected void drpApprovalStatus1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrders(drpApprovalStatus1.SelectedValue);
        }

        public void BindOrders(String pStatus)
        {

            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            int totrec;
            rptApproval.DataSource = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerList(pStatus, Session["LoginUserID"].ToString(), pMon, pYear);
            rptApproval.DataBind();
        }

        //protected void btnApproveReject_Click(object sender, EventArgs e)
        //{
        //    SendApprovalStatus();
        //}

        //public void SendApprovalStatus()
        //{
        //    foreach (RepeaterItem i in rptApproval.Items)
        //    {
        //        Entity.SalesOrder objEntity = new Entity.SalesOrder();

        //        HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
        //        DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
        //        // --------------------------------------------------------
        //        if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
        //        {
        //            objEntity.pkID = Convert.ToInt64(hdnID.Value);
        //            objEntity.ApprovalStatus = ddl.SelectedValue;
        //            objEntity.LoginUserID = Session["LoginUserID"].ToString();
        //            // -------------------------------------------------------------- Insert/Update Record
        //            BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
        //        }
        //    }
        //    BindOrders(drpApprovalStatus1.SelectedValue);
        //}

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                HiddenField hdnApprovalStatus = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));

                //if (!String.IsNullOrEmpty(tmpField.Value))
                //    ddl.SelectedValue = tmpField.Value;
                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // -----------------------------------------------------------------------
                //if ((hdnView.Value == "dashboard" || objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()) && (objAuth.RoleCode.ToLower() != "admin" && objAuth.RoleCode.ToLower() != "bradmin" && objAuth.RoleCode.ToLower() != "supervisor"))
                //    ddl.Attributes["disabled"] = "disabled";
                //if (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                //    ddl.Attributes["disabled"] = "disabled";
            }
        }
    }
}
