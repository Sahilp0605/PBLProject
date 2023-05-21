using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardDealer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                BindMonthYear();
            }
            // -------------------------------------------------------
            BindDealerSalesOrderApproval();
        }

        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpDailyMonth.Items.Add(new ListItem("-- All --", "0"));
            drpDailyMonth.Items.Add(new ListItem("January", "1"));
            drpDailyMonth.Items.Add(new ListItem("February", "2"));
            drpDailyMonth.Items.Add(new ListItem("March", "3"));
            drpDailyMonth.Items.Add(new ListItem("April", "4"));
            drpDailyMonth.Items.Add(new ListItem("May", "5"));
            drpDailyMonth.Items.Add(new ListItem("June", "6"));
            drpDailyMonth.Items.Add(new ListItem("July", "7"));
            drpDailyMonth.Items.Add(new ListItem("August", "8"));
            drpDailyMonth.Items.Add(new ListItem("September", "9"));
            drpDailyMonth.Items.Add(new ListItem("October", "10"));
            drpDailyMonth.Items.Add(new ListItem("November", "11"));
            drpDailyMonth.Items.Add(new ListItem("December", "12"));

            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpDailyYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            drpDailyYear.Items.Insert(0, new ListItem("-- All --", "0"));
            drpDailyYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        // --------------------------------------------------------------
        // Binding Dealer Sales Order Approval
        // --------------------------------------------------------------
        public void BindDealerSalesOrderApproval()
        {
            myDealerOrderApproval.pageView = "dashboard";
            myDealerOrderApproval.pageMonth = drpDailyMonth.SelectedValue;
            myDealerOrderApproval.pageYear = drpDailyYear.SelectedValue;
            myDealerOrderApproval.BindOrders(drpApprovalStatus.SelectedValue);
        }

        protected void drpApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            myDealerOrderApproval.BindOrders(drpApprovalStatus.SelectedValue);
        }

        protected void drpDailyMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDealerSalesOrderApproval();
        }
    }
}