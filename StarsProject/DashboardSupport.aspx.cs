using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardSupport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                hdnWelcomeGreet.Value = BAL.CommonMgmt.GetConstant("WelcomeGreet", 0, 1);
                // ----------------------------------------
                BindMonthYear();
                BindEmployee();
                BindDropDown();
            }
            // ----------------------------------------
            ReloadComplaintData();
            BindContractList();
        }
        // --------------------------------------------------------
        // General Bindings 
        // --------------------------------------------------------
        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpSuppMonth.Items.Add(new ListItem("-- All --", "0"));
            drpSuppMonth.Items.Add(new ListItem("January", "1"));
            drpSuppMonth.Items.Add(new ListItem("February", "2"));
            drpSuppMonth.Items.Add(new ListItem("March", "3"));
            drpSuppMonth.Items.Add(new ListItem("April", "4"));
            drpSuppMonth.Items.Add(new ListItem("May", "5"));
            drpSuppMonth.Items.Add(new ListItem("June", "6"));
            drpSuppMonth.Items.Add(new ListItem("July", "7"));
            drpSuppMonth.Items.Add(new ListItem("August", "8"));
            drpSuppMonth.Items.Add(new ListItem("September", "9"));
            drpSuppMonth.Items.Add(new ListItem("October", "10"));
            drpSuppMonth.Items.Add(new ListItem("November", "11"));
            drpSuppMonth.Items.Add(new ListItem("December", "12"));

            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpSuppYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            drpSuppYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
        }

        public void BindEmployee()
        {
            // ---------------- Employee List  -------------------------------------
            //List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            //lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            //drpSuppEmployee.DataSource = lstEmployee;
            //drpSuppEmployee.DataValueField = "LoginUserID";
            //drpSuppEmployee.DataTextField = "EmployeeName";
            //drpSuppEmployee.DataBind();
            //drpSuppEmployee.Items.Insert(0, new ListItem("-- All --", "admin"));
            //drpSuppEmployee.SelectedValue = Session["LoginUserID"].ToString();
        }

        public void BindDropDown()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("ComplaintStatus");
            drpSuppCategory.DataSource = lstDesig;
            drpSuppCategory.DataValueField = "InquiryStatusName";
            drpSuppCategory.DataTextField = "InquiryStatusName";
            drpSuppCategory.DataBind();
            drpSuppCategory.Items.Insert(0, new ListItem("-- Select --", ""));
        }
        // --------------------------------------------------------
        // Complaint Section
        // --------------------------------------------------------
        protected void drpSuppMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadComplaintData();
            BindContractList();
        }
        // -------------------------------------------------
        // Section : Complaint User Control 
        // -------------------------------------------------
        public void BindComplaintList()
        {
            myComplaint.BindComplaintList(Session["LoginUserID"].ToString(), drpSuppCategory.SelectedValue, Convert.ToInt64(drpSuppMonth.SelectedValue), Convert.ToInt64(drpSuppYear.SelectedValue));
            myComplaint.BindComplaintSummary(Session["LoginUserID"].ToString(), Convert.ToInt64(drpSuppMonth.SelectedValue), Convert.ToInt64(drpSuppYear.SelectedValue));
            spnOpen.InnerText = myComplaint.openTickets;
            spnClose.InnerText = myComplaint.closeTickets;
        }
        public void ReloadComplaintData()
        {
            BindComplaintList();
        }

        protected void drpSuppCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadComplaintData();
        }
        // -------------------------------------------------
        // Section : AMC Contract User Control 
        // -------------------------------------------------
        public void BindContractList()
        {
            myAMCContract.BindContractList(Session["LoginUserID"].ToString(), drpContractType.SelectedValue, Convert.ToInt64(drpSuppMonth.SelectedValue), Convert.ToInt64(drpSuppYear.SelectedValue));
        }
        protected void drpContractType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindContractList();
        }
    }
}