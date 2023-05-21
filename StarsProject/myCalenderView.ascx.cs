using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myCalenderView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMonthYear();
                BindEmployee();
                drpSummaryMonth.SelectedValue = (DateTime.Now.Month - 1).ToString();
                drpSummaryYear.SelectedValue = DateTime.Now.Year.ToString();
                List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
                BindEmployee();
            }
        }
        public void BindEmployee()
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            // ---------------- Employee List  -------------------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            if (Session["LoginUserID"].ToString().ToLower() == "admin" || Session["LoginUserID"].ToString().ToLower() == "bradmin")
                drpEmployee.Items.Insert(0, new ListItem("-- All --", "admin"));
            drpEmployee.SelectedValue = objAuth.EmployeeID.ToString();
        }
        public void BindMonthYear()
        {
            drpSummaryMonth.Items.Clear();
            drpSummaryYear.Items.Clear();
            // -----------------------------------------------------------------        
            drpSummaryMonth.Items.Add(new ListItem("January", "0"));
            drpSummaryMonth.Items.Add(new ListItem("February", "1"));
            drpSummaryMonth.Items.Add(new ListItem("March", "2"));
            drpSummaryMonth.Items.Add(new ListItem("April", "3"));
            drpSummaryMonth.Items.Add(new ListItem("May", "4"));
            drpSummaryMonth.Items.Add(new ListItem("June", "5"));
            drpSummaryMonth.Items.Add(new ListItem("July", "6"));
            drpSummaryMonth.Items.Add(new ListItem("August", "7"));
            drpSummaryMonth.Items.Add(new ListItem("September", "8"));
            drpSummaryMonth.Items.Add(new ListItem("October", "9"));
            drpSummaryMonth.Items.Add(new ListItem("November", "10"));
            drpSummaryMonth.Items.Add(new ListItem("December", "11"));
            // -----------------------------------------------------------------
            for (int i = 2015; i <= 2030; i++)
            {
                drpSummaryYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

    }
}