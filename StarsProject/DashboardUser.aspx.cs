using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using StarsProject.Common;

namespace StarsProject
{
    public partial class DashboardUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, typeof(string), "opload", "javascript:setTimeout(openLoader,3000);", true);
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                // ----------------------------------------
                drpToDo.Items.Add(new ListItem("Pending", "Pending", true));
                drpToDo.Items.Add(new ListItem("Overdue", "Overdue"));
                drpToDo.Items.Add(new ListItem("Completed", "Completed"));
                drpToDo.Items.Add(new ListItem("Completed-Overdue", "Completed-Overdue"));
                // ----------------------------------------
                BindMonthYear();
                // ----------------------------------------
                BindInquirySummary();

                BindTaskList();
                BindFollowupList();

                BindComplaintList();

                BindSalesOrderApproval();

                BindLeaveControl();
                BindUserControl();

                BindSalesTarget();

                BindCustomerLedger();
                // ----------------------------------------
                String tmpVal = drpSummaryMode.SelectedItem.Text;
                ScriptManager.RegisterStartupScript(this, typeof(string), "grph", "javascript:initGraphLayout('" + tmpVal + "');", true);
                // ----------------------------------------
                //ScriptManager.RegisterStartupScript(this, typeof(string), "grphext", "javascript:initExternalGraph('" + tmpVal + "');", true);
                // ----------------------------------------
                ScriptManager.RegisterStartupScript(this, typeof(string), "lay", "javascript:reloadLayout();", true);
                // ----------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                String tmpStr = "Hello User Welcome to E-OFFICE-DESK";
                ScriptManager.RegisterStartupScript(this, typeof(string), "spkwrds", "javascript:speakMyWords('" + tmpStr + "');", true);
            }
            // -----------------------------------------
            //Page.PreRenderComplete += new EventHandler(Page_LoadComplete);
        }

        public void Page_LoadComplete(object sender, EventArgs e)
        {
            //SpeechSynthesizer sp = new SpeechSynthesizer();
            //sp.Volume = 100;
            //sp.SpeakAsync("Hello world. Welcome to Sharvaya E-OFFICE-DESK");
        }

        protected void drpDashboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpDashboard.SelectedValue == "DashboardUser")
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "lay1", "javascript:setMyDashboard('111');", true);
            }
            else if (drpDashboard.SelectedValue == "DashboardBussiness")
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "lay2", "javascript:setMyDashboard('222');", true);
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
        }

        // --------------------------------------------------------------
        // Binding Inquiry Status 
        // -------------------------------------------------------------- 
        public void BindInquirySummary()
        {
            List<Entity.DashboardInquirySummary> lstEntity = new List<Entity.DashboardInquirySummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardInquiryStatusSummary(Session["LoginUserID"].ToString(), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
            aOpen.Text = "Open<span id='aSuccess1' class='badge badge-light marginleft10'>" + lstEntity[0].Open.ToString() + "</span>";
            aWorkProg.Text = "Work-In-Progress<span id='aSuccess2' class='badge badge-light marginleft10'>" + lstEntity[0].WorkInProgress.ToString() + "</span>";
            aHold.Text = "On Hold<span id='aSuccess3' class='badge badge-light marginleft10'>" + lstEntity[0].OnHold.ToString() + "</span>";
            aLost.Text = "Close-Lost<span id='aSuccess4' class='badge badge-light marginleft10'>" + lstEntity[0].CloseLost.ToString() + "</span>";
            aSuccess.Text = "Close-Success<span id='aSuccess5' class='badge badge-light marginleft10'>" + lstEntity[0].CloseSuccess.ToString() + "</span>";
            aUnknown.Text = "Others<span id='aSuccess6' class='badge badge-light marginleft10'>" + lstEntity[0].Unknown.ToString() + "</span>";
        }

        // --------------------------------------------------------------
        // Binding TODO
        // --------------------------------------------------------------
        public void BindTaskList()
        {
            int TotRec;
            List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
            lstEntity = BAL.ToDoMgmt.GetDashboardToDoList(drpToDo.SelectedValue, Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue), Session["LoginUserID"].ToString(), 1, 100000, out TotRec);
            rptTODO.DataSource = lstEntity;
            rptTODO.DataBind();
            lblToDOCount.Text = "  Total Count : " + lstEntity.Count.ToString() + "   ";
        }

        // --------------------------------------------------------------
        // Binding FollowUp
        // --------------------------------------------------------------
        public void BindFollowupList()
        {
            int TotRec;
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            lstEntity = BAL.FollowupMgmt.GetDashboardFollowupList(drpFollowup.SelectedValue, Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue), Session["LoginUserID"].ToString(), 1, 100000, out TotRec);
            rptFollowup.DataSource = lstEntity;
            rptFollowup.DataBind();
            lblFollowUpCount.Text = "  Total Count : " + lstEntity.Count.ToString() + "   ";
        }

        // --------------------------------------------------------------
        // Binding SalesTarget
        // --------------------------------------------------------------
        public void BindSalesTarget()
        {
            mySalesTarget.pageView = "dashboard";
            mySalesTarget.pageMonth = drpSummaryMonth.SelectedValue;
            mySalesTarget.pageYear = drpSummaryYear.SelectedValue;
            mySalesTarget.BindSalesTarget(Session["LoginUserID"].ToString(), drpTargetType.SelectedValue);
        }

        // --------------------------------------------------------------
        // Binding User Activity
        // --------------------------------------------------------------
        public void BindUserControl()
        {
            myUserActivity.pageView = "dashboard";
            myUserActivity.pageMonth = drpSummaryMonth.SelectedValue;
            myUserActivity.pageYear = drpSummaryYear.SelectedValue;
            myUserActivity.BindUserActivity(Session["LoginUserID"].ToString());
            lblUserCount.Text = " Total Count : " + myUserActivity.UserCount.ToString() + " ";
        }

        // --------------------------------------------------------------
        // Binding SalesTarget
        // --------------------------------------------------------------
        public void BindLeaveControl()
        {
            myLeaveRequest.pageView = "dashboard";
            myLeaveRequest.pageMonth = drpSummaryMonth.SelectedValue;
            myLeaveRequest.pageYear = drpSummaryYear.SelectedValue;
            myLeaveRequest.BindLeaveRequest(drpLeaveStatus.SelectedValue);
            lblLeaveCount.Text = " Total Count : " + myLeaveRequest.LeaveCount.ToString() + " ";
        }

        // --------------------------------------------------------------
        // Binding Sales Order Approval
        // --------------------------------------------------------------
        public void BindSalesOrderApproval()
        {
            myOrderApproval.pageView = "dashboard";
            myOrderApproval.BindOrders(drpApprovalStatus.SelectedValue);
        }

        // --------------------------------------------------------------
        // Binding Customer Ledger
        // --------------------------------------------------------------
        public void BindCustomerLedger()
        {
            myClientLedger.pageMonth = drpSummaryMonth.SelectedValue;
            myClientLedger.pageYear = drpSummaryYear.SelectedValue;
            myClientLedger.BindClientLedger(0, Session["LoginUserID"].ToString());
            lblDebitAmount.Text = "Net Payable : " + myClientLedger.GetDebitAmount.ToString();
            lblCreditAmount.Text = "Net Receivable : " + myClientLedger.GetCreditAmount.ToString();
        }

        // --------------------------------------------------------------
        // Binding Complaint
        // --------------------------------------------------------------
        public void BindComplaintList()
        {
            int TotRec;
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0, 0, drpComplaintStatus.SelectedValue, Session["LoginUserID"].ToString());
            rptComplaint.DataSource = lstEntity;
            rptComplaint.DataBind();
            lblComplaintCount.Text = "Total " + drpComplaintStatus.SelectedValue + " Complaints : " + lstEntity.Count.ToString() + " ";
        }


        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        protected void EventTimer_OnTick(object sender, EventArgs e)
        {
            BindInquirySummary();
        }

        protected void EventTimerResTeam_OnTick(object sender, EventArgs e)
        {
        }

        // ------------------------------------------------------------------
        // Action : This method is used to convert datatable to json string
        // ------------------------------------------------------------------
        //public string ConvertDataTabletoString()
        //{
        //    //int TotalCount = 0;
        //    //List<Entity.HelpLog> lstEntity = new List<Entity.HelpLog>();
        //    //lstEntity = BAL.HelpLogMgmt.GetHelpLogList(Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), 5000, out TotalCount);
        //    // -------------------------------------------------
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> row;
        //    // -------------------------------------------------
        //    return serializer.Serialize(rows);
        //}

        protected void drpSummaryMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            String tmpVal = drpSummaryMode.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(this, typeof(string), "grph", "javascript:initGraphLayout('" + tmpVal + "');", true);
            // ----------------------------------------------
            BindInquirySummary();
            BindTaskList();         // ToDO
            BindFollowupList();     // FollowUp
            BindLeaveControl();     // Leave Approval
            BindUserControl();      // User Activity
            BindSalesTarget();      // Sales Target
            BindCustomerLedger();   // Customer Ledger
        }

        protected void drpSummaryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            String tmpVal = drpSummaryMode.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(this, typeof(string), "grph", "javascript:initGraphLayout('" + tmpVal + "');", true);
        }

        protected void drpToDO_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTaskList();
        }
        protected void drpFollowup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFollowupList();
        }
        protected void drpApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            myOrderApproval.BindOrders(drpApprovalStatus.SelectedValue);
        }

        protected void drpLeaveStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLeaveControl();
        }

        protected void drpTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSalesTarget();
        }

        protected void drpComplaintStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindComplaintList();
        }

        protected void rptTODO_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnCompletionDate"));
                HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("ltrCompletion"));
                if (String.IsNullOrEmpty(hdn1.Value))
                {
                    dv.InnerText = "Not Applied";
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(hdn1.Value);
                    if (dt.Year < 2000)
                        dv.InnerText = "Not Applied";
                }

            }
        }

        protected void rptFollowup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnNextFollowup"));
                HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("ltrNextFollowup"));
                if (String.IsNullOrEmpty(hdn1.Value))
                {
                    dv.InnerText = "Not Applied";
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(hdn1.Value);
                    if (dt.Year < 2000)
                        dv.InnerText = "Not Applied";
                }
                // ------------------------------------------------------------
                //HtmlGenericControl dv1 = ((HtmlGenericControl)e.Item.FindControl("divFollInqNo"));
                //HiddenField hdn2 = ((HiddenField)e.Item.FindControl("hdnFollInqNo"));
                //dv1.Visible = (!String.IsNullOrEmpty(hdn2.Value)) ? true : false;

            }
        }

        protected void rptComplaint_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
            }
        }

    }
}