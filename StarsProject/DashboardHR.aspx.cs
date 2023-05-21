using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardHR : System.Web.UI.Page
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
                //BindEmployee();
                // -------------------------------------------------------
                BindLeaveControl();     // Leave Request

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:setUserActivityInterface();", true);
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
            drpDailyYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpDailyYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        protected void drpDailyMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLeaveControl();         // Leave Request
            BindOverTimeControl();      // OverTime
        }

        // --------------------------------------------------------------
        // Binding Leave Request
        // --------------------------------------------------------------
        public void BindLeaveControl()
        {
            myLeaveRequest.pageView = "dashboarddaily";
            myLeaveRequest.pageMonth = drpDailyMonth.SelectedValue;
            myLeaveRequest.pageYear = drpDailyYear.SelectedValue;
            myLeaveRequest.BindLeaveRequest(drpLeaveStatus.SelectedValue);
            //lblLeaveCount.Text = " Total Count : " + myLeaveRequest.LeaveCount.ToString() + " ";
        }
        protected void drpLeaveStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLeaveControl();
        }
        // --------------------------------------------------------------
        // Binding Overtime
        // --------------------------------------------------------------
        public void BindOverTimeControl()
        {
            myOvertimeApproval.pageView = "dashboarddaily";
            myOvertimeApproval.pageMonth = drpDailyMonth.SelectedValue;
            myOvertimeApproval.pageYear = drpDailyYear.SelectedValue;
            myOvertimeApproval.BindOverTime(drpOTStatus.SelectedValue);
        }

        protected void drpOTStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOverTimeControl();
        }
    }
}