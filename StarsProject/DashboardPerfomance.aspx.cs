using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Globalization;
using iTextSharp.text.html;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Net;
namespace StarsProject
{
    public partial class DashboardPerfomance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();

                BindMonthYear();
                // -------------------------------------------------------
                BindEmployee();
            }
        }
        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));

            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpDailyYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            drpDailyYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        public void BindEmployee()
        {
            // ---------------- Employee List  -------------------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();

            divCustToDO.Visible = true;
            drpEmployeeToDOCustomer.DataSource = lstEmployee;
            drpEmployeeToDOCustomer.DataValueField = "LoginUserID";
            drpEmployeeToDOCustomer.DataTextField = "EmployeeName";
            drpEmployeeToDOCustomer.DataBind();
            drpEmployeeToDOCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", ""));
            drpEmployeeToDOCustomer.SelectedValue = Session["LoginUserID"].ToString();

            // --------------------------------------------------------------------------
            List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
            lstCustomer = BAL.CustomerMgmt.GetCustomerBySalesOrder();
            drpToDOCustomer.DataSource = lstCustomer;
            drpToDOCustomer.DataValueField = "CustomerID";
            drpToDOCustomer.DataTextField = "CustomerName";
            drpToDOCustomer.DataBind();
            drpToDOCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
            drpToDOCustomer.SelectedValue = Session["LoginUserID"].ToString();
        }
        protected void drpDailyMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindCustomerTaskList();
        }
        // --------------------------------------------------------------
        // Binding : Customer TODO
        // --------------------------------------------------------------
        public void BindCustomerTaskList()
        {
            myToDOCustomer.pageView = "dashboarddaily";
            myToDOCustomer.pageMonth = drpDailyMonth.SelectedValue;
            myToDOCustomer.pageYear = drpDailyYear.SelectedValue;
            myToDOCustomer.pageCustomerID = drpToDOCustomer.SelectedValue;
            myToDOCustomer.BindCustomerTaskList(drpEmployeeToDOCustomer.SelectedValue, drpStatusToDOCustomer.SelectedValue);
            spnCustToDOCount.InnerText = "Count : " + myToDOCustomer.ToDoCount.ToString() + " ";
        }

        protected void drpStatusToDOCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomerTaskList();
        }
        protected void drpEmployeeToDOCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomerTaskList();
        }
        protected void drpToDOCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCustomerTaskList();
        }

    }
}