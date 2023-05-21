using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SearchCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BindCustomer();
                myClientDetailLedger.BindClientLedger(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString());
            }
        }
        public void BindCustomer()
        {
            int TotalCount = 0;
            // ----------------------------------------------------
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                // ----------------------------------------------------
                lblContact.Text = String.Concat(lstEntity[0].ContactNo1, (!String.IsNullOrEmpty(lstEntity[0].ContactNo2) ? ", " + lstEntity[0].ContactNo2 : ""));
                lblEmail.Text = lstEntity[0].EmailAddress;
                lblWebsite.Text = lstEntity[0].WebsiteAddress;

                lblGST.Text = lstEntity[0].GSTNo;
                lblPAN.Text = lstEntity[0].PANNo;
                lblCIN.Text = lstEntity[0].CINNo;
                lblBirthDate.Text = lstEntity[0].BirthDate != SqlDateTime.MinValue.Value && (lstEntity[0].BirthDate).Year > 1900 ? lstEntity[0].BirthDate.ToShortDateString() : "";
                lblAnniversaryDate.Text = lstEntity[0].AnniversaryDate != SqlDateTime.MinValue.Value && (lstEntity[0].AnniversaryDate).Year > 1900 ? lstEntity[0].AnniversaryDate.ToShortDateString() : "";

                if (!String.IsNullOrEmpty(lstEntity[0].Address.Trim()) && lstEntity[0].Address.Trim() != "" && lstEntity[0].Address.Trim() != "NULL")
                {
                    lblAddress11.Text = lstEntity[0].Address;
                    lblAddress12.Text = lstEntity[0].Area + (!String.IsNullOrEmpty(lstEntity[0].CityName) ? ", " + lstEntity[0].CityName : "") + (!String.IsNullOrEmpty(lstEntity[0].Pincode) ? ", " + lstEntity[0].Pincode : "");
                    lblAddress13.Text = (!String.IsNullOrEmpty(lstEntity[0].StateName) ? ", " + lstEntity[0].StateName : "INDIA");
                }

                if (!String.IsNullOrEmpty(lstEntity[0].Address1.Trim()) && lstEntity[0].Address1.Trim() != "" && lstEntity[0].Address1.Trim() != "NULL")
                {
                    lblAddress21.Text = lstEntity[0].Address1;
                    lblAddress22.Text = lstEntity[0].Area1 + (!String.IsNullOrEmpty(lstEntity[0].CityName1) ? ", " + lstEntity[0].CityName1 : "") + (!String.IsNullOrEmpty(lstEntity[0].Pincode1) ? ", " + lstEntity[0].Pincode1 : "");
                    lblAddress23.Text = (!String.IsNullOrEmpty(lstEntity[0].StateName1) ? ", " + lstEntity[0].StateName1 : "INDIA");

                }
                // --------------------------------
                BindContacts();

                // --------------------------------
                BindInquiry();
                BindQuotation();
                BindSalesOrders();
                BindOutward();
                BindSalesBill();
                BindPurchaseOrder();
                BindInward();
                BindPurchaseBill();
                BindFollowup();
                BindUserFollowupSummary();
            }
        }

        public void BindContacts()
        {
            int TotalCount = 0;
            List<Entity.CustomerContacts> lstEntity1 = new List<Entity.CustomerContacts>();
            lstEntity1 = BAL.CustomerContactsMgmt.GetCustomerContactsList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            rptContacts.DataSource = lstEntity1;
            rptContacts.DataBind();
        }

        public void BindSalesOrders()
        {
            int TotalCount;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();
                lstOrder = BAL.SalesOrderMgmt.GetSalesOrderListByStatus("", Session["LoginUserID"].ToString(), 1, 99000, out TotalCount);
                lstOrder = lstOrder.Where(e => (e.CustomerID == Convert.ToInt64(hdnCustomerID.Value))).ToList();
                rptApproval.DataSource = lstOrder;
                rptApproval.DataBind();
            }
        }
        public void BindOutward()
        {
            int TotalCount;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                List<Entity.Outward> lstOutward = new List<Entity.Outward>();
                lstOutward = BAL.OutwardMgmt.GetOutwardListByCustomerProduct(Convert.ToInt64(hdnCustomerID.Value), 0, Session["LoginUserID"].ToString());
                rptOutward.DataSource = lstOutward;
                rptOutward.DataBind();
            }
        }

        public void BindSalesBill()
        {
            int TotalCount;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                List<Entity.SalesBill> lstOrder = new List<Entity.SalesBill>();
                lstOrder = BAL.SalesBillMgmt.GetSalesBillList(0, Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                lstOrder = lstOrder.Where(e => (e.CustomerID == Convert.ToInt64(hdnCustomerID.Value))).ToList();
                rptSalesBill.DataSource = lstOrder;
                rptSalesBill.DataBind();
            }
        }

        public void BindPurchaseOrder()
        {
            int TotalCount;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                List<Entity.PurchaseOrder> lstOrder = new List<Entity.PurchaseOrder>();
                lstOrder = BAL.PurchaseOrderMgmt.GetPurchaseOrderList(0, Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                lstOrder = lstOrder.Where(e => (e.CustomerID == Convert.ToInt64(hdnCustomerID.Value))).ToList();
                rptPurcOrder.DataSource = lstOrder;
                rptPurcOrder.DataBind();
            }
        }

        public void BindInward()
        {
            int TotalCount;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                List<Entity.Inward> lstInward = new List<Entity.Inward>();
                lstInward = BAL.InwardMgmt.GetInwardListByCustomer(Convert.ToInt64(hdnCustomerID.Value), 0, Session["LoginUserID"].ToString(), 0, 0);
                rptInward.DataSource = lstInward;
                rptInward.DataBind();
            }
        }
        public void BindPurchaseBill()
        {
            int TotalCount;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                List<Entity.PurchaseBill> lstOrder = new List<Entity.PurchaseBill>();
                lstOrder = BAL.PurchaseBillMgmt.GetPurchaseBillList(0, Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                lstOrder = lstOrder.Where(e => (e.CustomerID == Convert.ToInt64(hdnCustomerID.Value))).ToList();
                rptPurcBill.DataSource = lstOrder;
                rptPurcBill.DataBind();
            }
        }
        public void BindInquiry()
        {
            List<Entity.InquiryInfo> lstEntity1 = new List<Entity.InquiryInfo>();
            lstEntity1 = BAL.InquiryInfoMgmt.GetInquiryInfoListByCustomer(Convert.ToInt64(hdnCustomerID.Value));
            rptInquiry.DataSource = lstEntity1;
            rptInquiry.DataBind();
        }

        public void BindQuotation()
        {
            List<Entity.Quotation> lstEntity1 = new List<Entity.Quotation>();
            lstEntity1 = BAL.QuotationMgmt.GetQuotationListByCustomer(Convert.ToInt64(hdnCustomerID.Value));
            rptQuotation.DataSource = lstEntity1;
            rptQuotation.DataBind();
        }

        public void BindFollowup()
        {
            int TotalCount = 0;
            List<Entity.Followup> lstEntity1 = new List<Entity.Followup>();
            lstEntity1 = BAL.FollowupMgmt.GetDashboardFollowupTimeline(Convert.ToInt64(hdnCustomerID.Value), "admin", 1, 1000, out TotalCount);
            rptFollowup.DataSource = lstEntity1;
            rptFollowup.DataBind();
        }

        public void BindUserFollowupSummary()
        {
            int TotalCount = 0;
            List<Entity.DashboardCountSummary> lstEntity1 = new List<Entity.DashboardCountSummary>();
            lstEntity1 = BAL.CommonMgmt.GetEmployeeFollowupCount(Session["LoginUserID"].ToString());
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        // --------------------------------------------------------------------------
        // Section : Regenerate Trial Balance
        // --------------------------------------------------------------------------
        public void ReGenTrialBalance()
        {
            List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
            lstCustomer = BAL.CommonMgmt.ReGenerateTrialBalance();
        }
        protected void btnSaveTrial_Click(object sender, EventArgs e)
        {
            ReGenTrialBalance();
        }
    }
}