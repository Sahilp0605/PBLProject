using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardAccount : System.Web.UI.Page
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
                BindGSTRSummary();
                // -------------------------------------------------------
                BindSalesOrderApproval();
                BindOrderBillingStatus();
                BindSalesTarget();
                BindCustomerLedger();
                BindSOStatus();
                BindOutstandingBills();
            }
        }

        void BindGSTRSummary()
        {
            myGSTRSummary.pageView = drpGSTR.SelectedValue;
            myGSTRSummary.pageMonth = drpDailyMonth.SelectedValue;
            myGSTRSummary.pageYear = drpDailyYear.SelectedValue;
            myGSTRSummary.BindGSTRSummary();
        }
        void BindSOStatus()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("SOApproval");
            drpApprovalStatus.DataSource = lstDesig;
            drpApprovalStatus.DataValueField = "InquiryStatusName";
            drpApprovalStatus.DataTextField = "InquiryStatusName";
            drpApprovalStatus.DataBind();
            //drpApprovalStatus_SelectedIndexChanged(null, null);

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
            drpDailyYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpDailyYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        // --------------------------------------------------------------
        // Binding SalesTarget
        // --------------------------------------------------------------
        public void BindSalesTarget()
        {
            mySalesTarget.pageView = "dashboard";
            mySalesTarget.pageMonth = drpDailyMonth.SelectedValue;
            mySalesTarget.pageYear = drpDailyYear.SelectedValue;
            mySalesTarget.BindSalesTarget(Session["LoginUserID"].ToString(), drpTargetType.SelectedValue);
        }

        protected void drpTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSalesTarget();
        }
        // --------------------------------------------------------------
        // Binding Sales Order Approval
        // --------------------------------------------------------------
        public void BindSalesOrderApproval()
        {
            myOrderApproval.pageView = "dashboard";
            myOrderApproval.pageMonth = drpDailyMonth.SelectedValue;
            myOrderApproval.pageYear = drpDailyYear.SelectedValue;
            myOrderApproval.BindOrders(drpApprovalStatus.SelectedValue);
        }
        
        protected void drpApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            myOrderApproval.BindOrders(drpApprovalStatus.SelectedValue);
        }

        // --------------------------------------------------------------
        // Binding Billing Status
        // --------------------------------------------------------------
        public void BindOrderBillingStatus()
        {
            MyOrderBillingStatus.pageView = "dashboard";
            MyOrderBillingStatus.BindOrdersByBillStatus(drpBillStatus.SelectedValue);
        }

        protected void drpBillStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyOrderBillingStatus.BindOrdersByBillStatus(drpBillStatus.SelectedValue);
        }
        // --------------------------------------------------------------
        // Binding Customer Ledger
        // --------------------------------------------------------------
        public void BindCustomerLedger()
        {
            myLedgerRec.pageMonth = drpDailyMonth.SelectedValue;
            myLedgerRec.pageYear = drpDailyMonth.SelectedValue;
            myLedgerRec.BindClientLedgerByDBCR(0, Session["LoginUserID"].ToString(), "R");
            // ---------------------------------------------
            myLedgerPay.pageMonth = drpDailyMonth.SelectedValue;
            myLedgerPay.pageYear = drpDailyMonth.SelectedValue;
            myLedgerPay.BindClientLedgerByDBCR(0, Session["LoginUserID"].ToString(), "P");

            //lblDebitAmount.Text = "Net Payable : " + myLedgerRec.GetDebitAmount.ToString();
            //lblCreditAmount.Text = "Net Receivable : " + myLedgerRec.GetCreditAmount.ToString();
        }

        protected void drpDailyMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGSTRSummary();
            BindCustomerLedger();         
            BindOrderBillingStatus();     
            BindSalesOrderApproval();     
            BindSalesTarget();
            BindOutstandingBills();
        }

        protected void drpGSTR_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGSTRSummary();
        }

        // --------------------------------------------------------------
        // Binding : Outstanding Billing Status
        // --------------------------------------------------------------
        public void BindOutstandingBills()
        {
            myOutstandingBills.pageView = drpBillingCategory.SelectedValue;
            myOutstandingBills.pageCategory = drpPendingStatus.SelectedValue;
            myOutstandingBills.pageAgeing = drpAging.SelectedValue;
            myOutstandingBills.pageAge1 = (!String.IsNullOrEmpty(drpAging.SelectedValue)) ? txtAge1.Text : "0";
            myOutstandingBills.pageAge2 = (!String.IsNullOrEmpty(drpAging.SelectedValue)) ? txtAge2.Text : "0";
            myOutstandingBills.pageAge3 = (!String.IsNullOrEmpty(drpAging.SelectedValue)) ? txtAge3.Text : "0";
            myOutstandingBills.pageAge4 = (!String.IsNullOrEmpty(drpAging.SelectedValue)) ? txtAge4.Text : "0";
            myOutstandingBills.pageAge5 = (!String.IsNullOrEmpty(drpAging.SelectedValue)) ? txtAge5.Text : "0";
            myOutstandingBills.pageAge6 = (!String.IsNullOrEmpty(drpAging.SelectedValue)) ? txtAge6.Text : "0";
            myOutstandingBills.BindOutstandingBills();
        }
        protected void drpPendingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOutstandingBills();
        }

        protected void drpBillingCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOutstandingBills();
        }

        protected void drpAging_SelectedIndexChanged(object sender, EventArgs e)
        {
            divAging.Visible = (!String.IsNullOrEmpty(drpAging.SelectedValue.ToLower())) ? true : false;
            BindOutstandingBills();
        }

        protected void txtAge_TextChanged(object sender, EventArgs e)
        {
            BindOutstandingBills();
        }
    }
}