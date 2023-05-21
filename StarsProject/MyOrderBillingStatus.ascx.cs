using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MyOrderBillingStatus : System.Web.UI.UserControl
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
            }
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public void BindOrdersByBillStatus(String pBillingStatus)
        {
            int totrec;
            rptApproval.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListByBillStatus(pBillingStatus, Session["LoginUserID"].ToString(),1,9999,out totrec);
            rptApproval.DataBind();
        }        

        protected void rptApproval_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                //HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));

                //if (!String.IsNullOrEmpty(tmpField.Value))
                //    ddl.SelectedValue = tmpField.Value;
                // -----------------------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // -----------------------------------------------------------------------

                //if  (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                //    ddl.Attributes["disabled"] = "disabled";
                    
            }
        }
    }
}


