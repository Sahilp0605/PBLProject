using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpDashboard.SelectedValue = "Dashboard-Inventory";

                Session["PageNo"] = 1;
                Session["PageSize"] = System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString();
                Session["OldUserID"] = "";
                // ----------------------------------------
                BindMonthYear();
                // ----------------------------------------
            }
        }

        protected void drpDashboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDashboard.SelectedValue == "Dashboard")
            {
                drpDashboard.SelectedItem.Value = "Dashboard";
                Response.Redirect("Dashboard.aspx");
            }
            if (drpDashboard.SelectedValue == "Dashboard-Lead")
            {
                drpDashboard.SelectedItem.Value = "Dashboard-Lead";
                Response.Redirect("DashboardLead.aspx");
            }
            if (drpDashboard.SelectedValue == "Dashboard-Inventory")
            {
                drpDashboard.SelectedItem.Value = "Dashboard-Inventory";
                Response.Redirect("DashboardAdmin.aspx");
            }
        }
        
        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpSummaryMonth.Items.Add(new ListItem("-- All --", "0"));
            drpSummaryMonth.Items.Add(new ListItem("January", "1"));
            drpSummaryMonth.Items.Add(new ListItem("February", "2"));
            drpSummaryMonth.Items.Add(new ListItem("March", "3"));
            drpSummaryMonth.Items.Add(new ListItem("April", "4"));
            drpSummaryMonth.Items.Add(new ListItem("May", "5"));
            drpSummaryMonth.Items.Add(new ListItem("June", "6"));
            drpSummaryMonth.Items.Add(new ListItem("July", "7"));
            drpSummaryMonth.Items.Add(new ListItem("August", "8"));
            drpSummaryMonth.Items.Add(new ListItem("September", "9"));
            drpSummaryMonth.Items.Add(new ListItem("October", "10"));
            drpSummaryMonth.Items.Add(new ListItem("November", "11"));
            drpSummaryMonth.Items.Add(new ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpSummaryYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            drpSummaryYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        protected void drpSummaryMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void drpSummaryMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}