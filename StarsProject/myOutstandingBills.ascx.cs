using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myOutstandingBills : System.Web.UI.UserControl
    {
        int totrec;
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
        public string pageCategory
        {
            get { return hdnCategory.Value; }
            set { hdnCategory.Value = value; }
        }

        public string pageAgeing
        {
            get { return hdnAgeing.Value; }
            set { hdnAgeing.Value = value; }
        }
        public string pageAge1
        {
            get { return hdnAge1.Value; }
            set { hdnAge1.Value = value; }
        }
        public string pageAge2
        {
            get { return hdnAge2.Value; }
            set { hdnAge2.Value = value; }
        }
        public string pageAge3
        {
            get { return hdnAge3.Value; }
            set { hdnAge3.Value = value; }
        }
        public string pageAge4
        {
            get { return hdnAge4.Value; }
            set { hdnAge4.Value = value; }
        }
        public string pageAge5
        {
            get { return hdnAge5.Value; }
            set { hdnAge5.Value = value; }
        }
        public string pageAge6
        {
            get { return hdnAge6.Value; }
            set { hdnAge6.Value = value; }
        }
        public void BindOutstandingBills()
        {
            rptOutstanding.DataSource = BAL.SalesOrderMgmt.GetOutstandingBills(pageView, pageCategory, pageAgeing, DateTime.Now, pageAge1, pageAge2, pageAge3, pageAge4, pageAge5, pageAge6);
            rptOutstanding.DataBind();
        }

        protected void rptOutstanding_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                //HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));

                ////if (!String.IsNullOrEmpty(tmpField.Value))
                ////    ddl.SelectedValue = tmpField.Value;
                //// -----------------------------------------------------------------------
                //Entity.Authenticate objAuth = new Entity.Authenticate();
                //objAuth = (Entity.Authenticate)Session["logindetail"];
                //// -----------------------------------------------------------------------

                ////if  (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                ////    ddl.Attributes["disabled"] = "disabled";

            }
        }
    }
}