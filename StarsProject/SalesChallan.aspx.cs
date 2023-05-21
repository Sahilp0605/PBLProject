using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Reflection;

namespace StarsProject
{
    public partial class SalesChallan : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount, totDiscAmt, totAddTaxAmt, totSGST, totCGST, totIGST;
        public decimal DomesticPer = 0, InternationalPer = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();
            // ----------------------------------------------------
            String TempConstant = BAL.CommonMgmt.GetConstant("Domestic", 0, 1).ToString();
            DomesticPer = (!String.IsNullOrEmpty(TempConstant)) ? Convert.ToDecimal(TempConstant) : 0;
            TempConstant = BAL.CommonMgmt.GetConstant("International", 0, 1);
            InternationalPer = (!String.IsNullOrEmpty(TempConstant)) ? Convert.ToDecimal(TempConstant) : 0;
            TempConstant = BAL.CommonMgmt.GetConstant("SalesWithInquiry", 0, 1);

            if (!IsPostBack)
            {
                ViewState["totAmount"] = 0;
                ViewState["totTaxAmount"] = 0;
                ViewState["totNetAmount"] = 0;
                ViewState["totDiscAmt"] = 0;
                ViewState["totAddTaxAmt"] = 0;
                ViewState["totSGST"] = 0;
                ViewState["totCGST"] = 0;
                ViewState["totIGST"] = 0;

                totAmount = 0;
                totTaxAmount = 0;
                totNetAmount = 0;
                totDiscAmt = 0;
                totAddTaxAmt = 0;
                totCGST = 0;
                totSGST = 0;
                totIGST = 0;

                DataTable dtDetail = new DataTable();
                Session.Add("dtDetail", dtDetail);
                Session.Remove("mySpecs");

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnStockOutward.Value = BAL.CommonMgmt.GetConstant("StockOutward", 0, 1).ToLower();
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
                BindDropDown();
                // --------------------------------------------------------
                string manualInvoiceNo = BAL.CommonMgmt.GetConstant("SalesManualInvoiceNo", 0, 1);
                if (manualInvoiceNo == "yes")      // Steelman Gases
                    txtInvoiceNo.ReadOnly = false;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        if (!String.IsNullOrEmpty(Request.QueryString["CustomerId"]))
                        {
                            hdnCustomerID.Value = (!String.IsNullOrEmpty(Request.QueryString["CustomerId"])) ? Request.QueryString["CustomerId"] : "";

                            txtCustomerName_TextChanged(null, null);
                            if (!String.IsNullOrEmpty(Request.QueryString["InquiryNo"]))
                            {
                                rdblOption.SelectedIndex = rdblOption.Items.IndexOf(rdblOption.Items.FindByText("Inquiry"));
                                rdblOption_SelectedIndexChanged(null, null);
                                drpReferenceNo.SelectedValue = (!String.IsNullOrEmpty(Request.QueryString["InquiryNo"])) ? Request.QueryString["InquiryNo"] : "";
                                drpReferenceNo_SelectedIndexChanged(null, null);
                            }
                        }
                    }
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && String.IsNullOrEmpty(txtCustomerName.Text))       //For Bill generation from inquiry no - dashboard
                    txtCustomerName.Text = (lstEntity.Count > 0) ? lstEntity[0].CustomerName : "";

                if (lstEntity.Count > 0)
                {
                    hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";

                    //if (lstEntity[0].BlockCustomer == true)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, GetType(), "alertMessage", @"alert('Please Unblock Customer First ')", true);
                    //    txtCustomerName.Focus();
                    //}
                }

                if (!String.IsNullOrEmpty(hdnCustStateID.Value))
                {
                    if (Convert.ToInt64(hdnCustStateID.Value) > 0)
                    {
                        drpTerminationOfDelivery.SelectedValue = hdnCustStateID.Value;
                    }
                }
                drpTerminationOfDelivery_SelectedIndexChanged(null, null);

                if (!String.IsNullOrEmpty(lstEntity[0].CityCode))
                {
                    if (Convert.ToInt64(lstEntity[0].CityCode) > 0)
                    {
                        drpTerminationOfDeliveryCity.SelectedValue = lstEntity[0].CityCode;
                    }
                }
                // ------------------------------------------
                BindSalesBillDetailList("");
                // ------------------------------------------            
                bindInqQuotationSO();
            }
            else
            {
                strErr += "<li>" + "Select Proper Customer From List." + "</li>";
                txtCustomerName.Focus();
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void rdblOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
            {
                if (Request.QueryString["mode"].ToString() == "edit")
                    hdnInvBasedOn.Value = "salesinvoice";
                else
                    hdnInvBasedOn.Value = rdblOption.SelectedValue.ToLower();

            }
            bindInqQuotationSO();
        }
        private void bindInqQuotationSO()
        {
            if (rdblOption.SelectedValue == "Quotation")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    BindQuotationList(Convert.ToInt64(hdnCustomerID.Value));
                else
                    BindQuotationList(0);

                lblReferenceType.InnerText = "Quotation No";
            }
            else if (rdblOption.SelectedValue == "Inquiry")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    BindInquiryList(Convert.ToInt64(hdnCustomerID.Value));
                else
                    BindInquiryList(0);

                lblReferenceType.InnerText = "Inquiry No";
            }
            else if (rdblOption.SelectedValue == "SalesOrder")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    BindSalesOrderList(Convert.ToInt64(hdnCustomerID.Value));
                else
                    BindSalesOrderList(0);

                lblReferenceType.InnerText = "Order No";
            }
            else if (rdblOption.SelectedValue == "Complaint")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    BindComplaintList(Convert.ToInt64(hdnCustomerID.Value));

                lblReferenceType.InnerText = "Complaint No";
            }
        }
        protected void drpReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];


            if (rdblOption.SelectedItem.Value == "Inquiry")
            {
                if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                {
                    dtDetail.Clear();
                    dtDetail = BAL.InquiryInfoMgmt.GetInquiryProductForQuotation(drpReferenceNo.SelectedValue, "");

                    foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                    foreach (DataRow row in dtDetail.Rows)
                    {
                        Decimal uq = (!String.IsNullOrEmpty(row["UnitQuantity"].ToString())) ? Convert.ToDecimal(row["UnitQuantity"]) : 1;
                        Decimal a = (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                        Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);
                        Decimal q = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                        Decimal ur = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                        Decimal dp = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                        Decimal dpa = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                        Decimal nr = (!String.IsNullOrEmpty(row["NetRate"].ToString())) ? Convert.ToDecimal(row["NetRate"]) : 0;
                        Decimal tr = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                        Decimal at = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;

                        // -----------------------------------------------------
                        if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")    // ShaktiPet
                        {
                            row.SetField("UnitQty", q);
                            row.SetField("Qty", q * uq);
                            row.SetField("Quantity", q * uq);
                            q = (q * uq);
                        }
                        // -----------------------------------------------------
                        decimal TaxAmt = 0;
                        decimal CGSTPer = 0, CGSTAmt = 0;
                        decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
                        BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);

                        row.SetField("HeaderDiscAmt", 0);
                        row.SetField("NetRate", NetRate);
                        row.SetField("Amount", BasicAmt);
                        row.SetField("CGSTPer", CGSTPer);
                        row.SetField("SGSTPer", SGSTPer);
                        row.SetField("IGSTPer", IGSTPer);
                        row.SetField("CGSTAmt", CGSTAmt);
                        row.SetField("SGSTAmt", SGSTAmt);
                        row.SetField("IGSTAmt", IGSTAmt);
                        row.SetField("TaxRate", CGSTPer + SGSTPer + IGSTPer);
                        row.SetField("TaxAmount", CGSTAmt + SGSTAmt + IGSTAmt);
                        row.SetField("AddTaxAmt", AddTaxAmt);
                        row.SetField("NetAmt", NetAmt);
                        row.SetField("NetAmount", NetAmt);
                    }
                }
            }
            else if (rdblOption.SelectedItem.Value == "Quotation")
            {
                if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                {
                    dtDetail.Clear();
                    dtDetail = BAL.QuotationDetailMgmt.GetQuotationProductForSalesOrder(drpReferenceNo.SelectedValue, "");

                    long QtPKID = 0;
                    if (dtDetail.Rows.Count > 0)
                        QtPKID = Convert.ToInt64(dtDetail.Rows[0]["QtPKID"]);

                    if (QtPKID > 0)
                    {
                        int TotalCount = 0;
                        List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
                        // ----------------------------------------------------
                        lstEntity = BAL.QuotationMgmt.GetQuotationList(QtPKID, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                        if (lstEntity.Count > 0)
                        {
                            txtHeadDiscount.Text = lstEntity[0].DiscountAmt.ToString();

                            drpOthChrg1.SelectedValue = (lstEntity[0].ChargeID1 > 0) ? lstEntity[0].ChargeID1.ToString() : "";
                            drpOthChrg2.SelectedValue = (lstEntity[0].ChargeID2 > 0) ? lstEntity[0].ChargeID2.ToString() : "";
                            drpOthChrg3.SelectedValue = (lstEntity[0].ChargeID3 > 0) ? lstEntity[0].ChargeID3.ToString() : "";
                            drpOthChrg4.SelectedValue = (lstEntity[0].ChargeID4 > 0) ? lstEntity[0].ChargeID4.ToString() : "";
                            drpOthChrg5.SelectedValue = (lstEntity[0].ChargeID5 > 0) ? lstEntity[0].ChargeID5.ToString() : "";

                            txtOthChrgAmt1.Text = (lstEntity[0].ChargeAmt1 > 0) ? lstEntity[0].ChargeAmt1.ToString("0.00") : "0.00";
                            txtOthChrgAmt2.Text = (lstEntity[0].ChargeAmt2 > 0) ? lstEntity[0].ChargeAmt2.ToString("0.00") : "0.00";
                            txtOthChrgAmt3.Text = (lstEntity[0].ChargeAmt3 > 0) ? lstEntity[0].ChargeAmt3.ToString("0.00") : "0.00";
                            txtOthChrgAmt4.Text = (lstEntity[0].ChargeAmt4 > 0) ? lstEntity[0].ChargeAmt4.ToString("0.00") : "0.00";
                            txtOthChrgAmt5.Text = (lstEntity[0].ChargeAmt5 > 0) ? lstEntity[0].ChargeAmt5.ToString("0.00") : "0.00";

                            hdnOthChrgBasic1.Value = (lstEntity[0].ChargeBasicAmt1 > 0) ? lstEntity[0].ChargeBasicAmt1.ToString("0.00") : "0";
                            hdnOthChrgBasic2.Value = (lstEntity[0].ChargeBasicAmt2 > 0) ? lstEntity[0].ChargeBasicAmt2.ToString("0.00") : "0";
                            hdnOthChrgBasic3.Value = (lstEntity[0].ChargeBasicAmt3 > 0) ? lstEntity[0].ChargeBasicAmt3.ToString("0.00") : "0";
                            hdnOthChrgBasic4.Value = (lstEntity[0].ChargeBasicAmt4 > 0) ? lstEntity[0].ChargeBasicAmt4.ToString("0.00") : "0";
                            hdnOthChrgBasic5.Value = (lstEntity[0].ChargeBasicAmt5 > 0) ? lstEntity[0].ChargeBasicAmt5.ToString("0.00") : "0";

                            hdnOthChrgGST1.Value = (lstEntity[0].ChargeGSTAmt1 > 0) ? lstEntity[0].ChargeGSTAmt1.ToString("0.00") : "0";
                            hdnOthChrgGST2.Value = (lstEntity[0].ChargeGSTAmt2 > 0) ? lstEntity[0].ChargeGSTAmt2.ToString("0.00") : "0";
                            hdnOthChrgGST3.Value = (lstEntity[0].ChargeGSTAmt3 > 0) ? lstEntity[0].ChargeGSTAmt3.ToString("0.00") : "0";
                            hdnOthChrgGST4.Value = (lstEntity[0].ChargeGSTAmt4 > 0) ? lstEntity[0].ChargeGSTAmt4.ToString("0.00") : "0";
                            hdnOthChrgGST5.Value = (lstEntity[0].ChargeGSTAmt5 > 0) ? lstEntity[0].ChargeGSTAmt5.ToString("0.00") : "0";
                            
                        }
                    }
                 }
            }
            else if (rdblOption.SelectedItem.Value == "SalesOrder")
            {
                if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                {
                    dtDetail.Clear();
                    dtDetail = BAL.SalesOrderMgmt.GetSalesOrderDetailForSale(drpReferenceNo.SelectedValue);
                    long SOPKID = 0;
                    //if (dtDetail.Rows.Count > 0)
                    //    SOPKID = Convert.ToInt64(dtDetail.Rows[0]["QtPKID"]);
                    SOPKID = BAL.CommonMgmt.GetSalesOrderPrimaryID(drpReferenceNo.SelectedValue);

                    if (SOPKID > 0)
                    {
                        int TotalCount = 0;
                        List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
                        // ----------------------------------------------------
                        lstEntity = BAL.SalesOrderMgmt.GetSalesOrderList(SOPKID, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                        if (lstEntity.Count > 0)
                        {
                            txtHeadDiscount.Text = lstEntity[0].DiscountAmt.ToString();

                            drpOthChrg1.SelectedValue = (lstEntity[0].ChargeID1 > 0) ? lstEntity[0].ChargeID1.ToString() : "";
                            drpOthChrg2.SelectedValue = (lstEntity[0].ChargeID2 > 0) ? lstEntity[0].ChargeID2.ToString() : "";
                            drpOthChrg3.SelectedValue = (lstEntity[0].ChargeID3 > 0) ? lstEntity[0].ChargeID3.ToString() : "";
                            drpOthChrg4.SelectedValue = (lstEntity[0].ChargeID4 > 0) ? lstEntity[0].ChargeID4.ToString() : "";
                            drpOthChrg5.SelectedValue = (lstEntity[0].ChargeID5 > 0) ? lstEntity[0].ChargeID5.ToString() : "";

                            txtOthChrgAmt1.Text = (lstEntity[0].ChargeAmt1 > 0) ? lstEntity[0].ChargeAmt1.ToString("0.00") : "0.00";
                            txtOthChrgAmt2.Text = (lstEntity[0].ChargeAmt2 > 0) ? lstEntity[0].ChargeAmt2.ToString("0.00") : "0.00";
                            txtOthChrgAmt3.Text = (lstEntity[0].ChargeAmt3 > 0) ? lstEntity[0].ChargeAmt3.ToString("0.00") : "0.00";
                            txtOthChrgAmt4.Text = (lstEntity[0].ChargeAmt4 > 0) ? lstEntity[0].ChargeAmt4.ToString("0.00") : "0.00";
                            txtOthChrgAmt5.Text = (lstEntity[0].ChargeAmt5 > 0) ? lstEntity[0].ChargeAmt5.ToString("0.00") : "0.00";

                            hdnOthChrgBasic1.Value = (lstEntity[0].ChargeBasicAmt1 > 0) ? lstEntity[0].ChargeBasicAmt1.ToString("0.00") : "0";
                            hdnOthChrgBasic2.Value = (lstEntity[0].ChargeBasicAmt2 > 0) ? lstEntity[0].ChargeBasicAmt2.ToString("0.00") : "0";
                            hdnOthChrgBasic3.Value = (lstEntity[0].ChargeBasicAmt3 > 0) ? lstEntity[0].ChargeBasicAmt3.ToString("0.00") : "0";
                            hdnOthChrgBasic4.Value = (lstEntity[0].ChargeBasicAmt4 > 0) ? lstEntity[0].ChargeBasicAmt4.ToString("0.00") : "0";
                            hdnOthChrgBasic5.Value = (lstEntity[0].ChargeBasicAmt5 > 0) ? lstEntity[0].ChargeBasicAmt5.ToString("0.00") : "0";

                            hdnOthChrgGST1.Value = (lstEntity[0].ChargeGSTAmt1 > 0) ? lstEntity[0].ChargeGSTAmt1.ToString("0.00") : "0";
                            hdnOthChrgGST2.Value = (lstEntity[0].ChargeGSTAmt2 > 0) ? lstEntity[0].ChargeGSTAmt2.ToString("0.00") : "0";
                            hdnOthChrgGST3.Value = (lstEntity[0].ChargeGSTAmt3 > 0) ? lstEntity[0].ChargeGSTAmt3.ToString("0.00") : "0";
                            hdnOthChrgGST4.Value = (lstEntity[0].ChargeGSTAmt4 > 0) ? lstEntity[0].ChargeGSTAmt4.ToString("0.00") : "0";
                            hdnOthChrgGST5.Value = (lstEntity[0].ChargeGSTAmt5 > 0) ? lstEntity[0].ChargeGSTAmt5.ToString("0.00") : "0";

                        }
                    }
                }
            }

            Session.Add("dtDetail", dtDetail);
            dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
            Session.Add("dtDetail", dtDetail);

            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            txtHeadDiscount_TextChanged(null, null);
        }
        public void OnlyViewControls()
        {
            txtInvoiceNo.ReadOnly = true;
            txtInvoiceDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtSupplierRef.ReadOnly = true;
            txtSupplierRefDate.ReadOnly = true;
            drpTerminationOfDelivery.Attributes.Add("disabled", "disabled");
            drpTerminationOfDeliveryCity.Attributes.Add("disabled", "disabled");
            drpLocation.Attributes.Add("disabled", "disabled");
            txtOtherRef.ReadOnly = true;
            txtDisDocNo.ReadOnly = true;
            txtTotBasicAmt.ReadOnly = true;
            txtHeadDiscount.ReadOnly = true;
            txtTotGST.ReadOnly = true;
            txtRoff.ReadOnly = true;
            txtTotNetAmt.ReadOnly = true;

            txtOthChrgAmt1.ReadOnly = true;
            txtOthChrgAmt2.ReadOnly = true;
            txtOthChrgAmt3.ReadOnly = true;
            txtOthChrgAmt4.ReadOnly = true;
            txtOthChrgAmt5.ReadOnly = true;
            drpOthChrg1.Attributes.Add("disabled", "disabled");
            drpOthChrg2.Attributes.Add("disabled", "disabled");
            drpOthChrg3.Attributes.Add("disabled", "disabled");
            drpOthChrg4.Attributes.Add("disabled", "disabled");
            drpOthChrg5.Attributes.Add("disabled", "disabled");

            txtTotOthChrgBeforeGST.ReadOnly = true;
            txtTotOthChrgAfterGST.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
            pnlDetail.Enabled = false;
            rdblOption.Enabled = false;
            drpReferenceNo.Attributes.Add("disabled", "disabled");
        }
        public void BindDropDown()
        {
            // ---------------- OtherCharge List -------------------------------------
            List<Entity.OtherCharge> lstOthChrg = new List<Entity.OtherCharge>();
            lstOthChrg = BAL.OtherChargeMgmt.GetOtherChargeList();

            drpOthChrg1.DataSource = lstOthChrg;
            drpOthChrg1.DataValueField = "pkID";
            drpOthChrg1.DataTextField = "ChargeName";
            drpOthChrg1.DataBind();
            drpOthChrg1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", ""));

            drpOthChrg2.DataSource = lstOthChrg;
            drpOthChrg2.DataValueField = "pkID";
            drpOthChrg2.DataTextField = "ChargeName";
            drpOthChrg2.DataBind();
            drpOthChrg2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", ""));

            drpOthChrg3.DataSource = lstOthChrg;
            drpOthChrg3.DataValueField = "pkID";
            drpOthChrg3.DataTextField = "ChargeName";
            drpOthChrg3.DataBind();
            drpOthChrg3.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", ""));

            drpOthChrg4.DataSource = lstOthChrg;
            drpOthChrg4.DataValueField = "pkID";
            drpOthChrg4.DataTextField = "ChargeName";
            drpOthChrg4.DataBind();
            drpOthChrg4.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", ""));

            drpOthChrg5.DataSource = lstOthChrg;
            drpOthChrg5.DataValueField = "pkID";
            drpOthChrg5.DataTextField = "ChargeName";
            drpOthChrg5.DataBind();
            drpOthChrg5.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", ""));

            // ---------------- State List -------------------------------------
            List<Entity.State> lstEvents = new List<Entity.State>();
            lstEvents = BAL.StateMgmt.GetStateList();
            drpTerminationOfDelivery.DataSource = lstEvents;
            drpTerminationOfDelivery.DataValueField = "StateCode";
            drpTerminationOfDelivery.DataTextField = "StateName";
            drpTerminationOfDelivery.DataBind();
            drpTerminationOfDelivery.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All State --", ""));

            // ---------------- Terms & Condition -------------------------------------
            List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            drpTNC.DataSource = lstList2;
            drpTNC.DataValueField = "pkID";
            drpTNC.DataTextField = "TNC_Header";
            drpTNC.DataBind();
            drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

            // ---------------- Fixed Ledger -------------------------------------
            List<Entity.Customer> lstLedger = new List<Entity.Customer>();
            lstLedger = BAL.CustomerMgmt.GetFixedLedgerForDropdown();
            drpLedger.DataSource = lstLedger;
            drpLedger.DataValueField = "CustomerID";
            drpLedger.DataTextField = "CustomerName";
            drpLedger.DataBind();
            drpLedger.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Sales A/c --", ""));

            //---------------------------Bank Details-------------------------------
            int totrec = 0;
            List<Entity.OrganizationBankInfo> lstOrgDept23 = new List<Entity.OrganizationBankInfo>();
            lstOrgDept23 = BAL.CommonMgmt.GetBankInfo(0, "", 1, 50000, out totrec);
            drpBank.DataSource = lstOrgDept23;
            drpBank.DataValueField = "pkID";
            drpBank.DataTextField = "BankName";
            drpBank.DataBind();
            drpBank.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            //---------------------------Location Details-------------------------------
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            lstLocation = BAL.CommonMgmt.GetLocationList();
            drpLocation.DataSource = lstLocation;
            drpLocation.DataValueField = "LocationID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }
        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        public void BindSalesBillDetailList(string pInvoiceNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SalesChallanMgmt.GetSalesChallanDetail(pInvoiceNo);
            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        [System.Web.Services.WebMethod]
        public static string GetSalesChallanNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetSalesChallanNo(pkID);
            return tempVal;
        }

        public void BindQuotationList(Int64 pCustomerID)
        {
            List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
            drpReferenceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.QuotationMgmt.GetQuotationListByCustomer(pCustomerID);

                drpReferenceNo.DataValueField = "QuotationNo";
                drpReferenceNo.DataTextField = "QuotationNo";
                if (lstEntity.Count > 0)
                {
                    drpReferenceNo.DataSource = lstEntity;
                    drpReferenceNo.DataBind();
                }
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        public void BindInquiryList(Int64 pCustomerID)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            drpReferenceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoListByCustomer(pCustomerID);
                // --------------------------------------------------
                lstEntity = lstEntity.Where(e => !(e.InquiryStatus == "Close - Lost" && e.InquiryStatus == "Close - Success")).ToList();
                // --------------------------------------------------
                drpReferenceNo.DataValueField = "InquiryNo";
                drpReferenceNo.DataTextField = "InquiryNo";
                if (lstEntity.Count > 0)
                {
                    drpReferenceNo.DataSource = lstEntity;
                    drpReferenceNo.DataBind();
                }
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        public void BindSalesOrderList(Int64 pCustomerID)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            drpReferenceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", pCustomerID, "Approved", 0, 0);
                // --------------------------------------------------

                drpReferenceNo.DataValueField = "OrderNo";
                drpReferenceNo.DataTextField = "OrderNo";
                if (lstEntity.Count > 0)
                {
                    drpReferenceNo.DataSource = lstEntity;
                    drpReferenceNo.DataBind();
                }
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        public List<Entity.SalesOrder> BindSalesOrderList(Int64 pCustomerID, Int64 pProductID)
        {
            List<Entity.SalesOrder> lstPurcOrder = new List<Entity.SalesOrder>();
            lstPurcOrder = BAL.SalesOrderMgmt.GetSalesOrderListByCustomerProduct(pCustomerID, pProductID, "", Session["LoginUserID"].ToString());
            return lstPurcOrder;
        }

        public List<Entity.Outward> BindOutwardList(Int64 pCustomerID, Int64 pProductID)
        {
            List<Entity.Outward> lstInward = new List<Entity.Outward>();
            lstInward = BAL.OutwardMgmt.GetOutwardListByCustomerProduct(pCustomerID, pProductID, Session["LoginUserID"].ToString());
            return lstInward;
        }
        public void BindComplaintList(Int64 pCustomerID)
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            drpReferenceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.ComplaintMgmt.GetComplaintList(pCustomerID, "", Session["LoginUserID"].ToString());
                // --------------------------------------------------
                drpReferenceNo.DataValueField = "pkID";
                drpReferenceNo.DataTextField = "pkID";
                if (lstEntity.Count > 0)
                {
                    drpReferenceNo.DataSource = lstEntity;
                    drpReferenceNo.DataBind();
                }
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        protected void drpTerminationOfDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCustStateID.Value = drpTerminationOfDelivery.SelectedValue;
            if (!String.IsNullOrEmpty(hdnCustStateID.Value))
            {
                List<Entity.City> lstEvents = new List<Entity.City>();
                lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(hdnCustStateID.Value));
                drpTerminationOfDeliveryCity.DataSource = lstEvents;
                drpTerminationOfDeliveryCity.DataValueField = "CityCode";
                drpTerminationOfDeliveryCity.DataTextField = "CityName";
                drpTerminationOfDeliveryCity.DataBind();

            }
            drpTerminationOfDeliveryCity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All City --", ""));
            drpTerminationOfDeliveryCity.Enabled = true;
            // ------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());

            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail1);
        }

        protected void rptOrderDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlTableCell tdOutward = ((HtmlTableCell)e.Item.FindControl("tdOutwardNo"));
                tdOutward.InnerText = (hdnStockOutward.Value == "sale") ? "S.O #" : "Outward #";
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2, v3, v4, v5, v6, v7, v8;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "DiscountAmt"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Amount"));
                v3 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaxAmount"));
                v4 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "AddTaxAmt"));
                //v5 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmt"));
                v5 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmount"));

                v6 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "CGSTAmt"));
                v7 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "SGSTAmt"));
                v8 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "IGSTAmt"));

                totDiscAmt += v1;
                totAmount += v2;
                totTaxAmount += v3;
                totAddTaxAmt += v4;
                totNetAmount += v5;

                totCGST += v6;
                totSGST += v7;
                totIGST += v8;

                ViewState["totAmount"] = (!String.IsNullOrEmpty(totAmount.ToString())) ? Convert.ToDecimal(totAmount) : 0;
                ViewState["totTaxAmount"] = (!String.IsNullOrEmpty(totTaxAmount.ToString())) ? Convert.ToDecimal(totTaxAmount) : 0;
                ViewState["totNetAmount"] = (!String.IsNullOrEmpty(totNetAmount.ToString())) ? Convert.ToDecimal(totNetAmount) : 0;
                ViewState["totDiscAmt"] = (!String.IsNullOrEmpty(totDiscAmt.ToString())) ? Convert.ToDecimal(totDiscAmt) : 0;
                ViewState["totAddTaxAmt"] = (!String.IsNullOrEmpty(totAddTaxAmt.ToString())) ? Convert.ToDecimal(totAddTaxAmt) : 0;
                ViewState["totSGST"] = (!String.IsNullOrEmpty(totSGST.ToString())) ? Convert.ToDecimal(totSGST) : 0;
                ViewState["totCGST"] = (!String.IsNullOrEmpty(totCGST.ToString())) ? Convert.ToDecimal(totCGST) : 0;
                ViewState["totIGST"] = (!String.IsNullOrEmpty(totIGST.ToString())) ? Convert.ToDecimal(totIGST) : 0;
                // ------------------------------------------------
                if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")        // ShaktiPet    
                {
                    TextBox txt1 = (TextBox)e.Item.FindControl("edQuantity");
                    txt1.Enabled = false;
                }

            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotalDiscAmt = (Label)e.Item.FindControl("lblTotalDiscAmt");
                Label lblTotalAmt = (Label)e.Item.FindControl("lblTotalAmt");
                Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
                Label lblAddTaxAmt = (Label)e.Item.FindControl("lblAddTaxAmt");
                Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");

                lblTotalDiscAmt.Text = totDiscAmt.ToString("0.00");
                lblTotalAmt.Text = totAmount.ToString("0.00");
                lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
                lblAddTaxAmt.Text = totAddTaxAmt.ToString("0.00");
                lblTotalNetAmount.Text = totNetAmount.ToString("0.00");

                funCalculateTotal();
                // ------------------------------------------------
                if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")        // ShaktiPet    
                {
                    TextBox txt1 = (TextBox)e.Item.FindControl("txtQuantity");
                    txt1.Enabled = false;
                }
                // -----------------------------------------------------
                // Binding Customer's Purchase Order
                // -----------------------------------------------------
                DropDownList drpForOrderNo = (DropDownList)e.Item.FindControl("drpForOrderNo");
                drpForOrderNo.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    drpForOrderNo.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", Convert.ToInt64(hdnCustomerID.Value), "Approved", 0, 0);
                else
                    drpForOrderNo.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", -1, "Approved", 0, 0);
                drpForOrderNo.DataTextField = "OrderNo";
                drpForOrderNo.DataValueField = "OrderNo";
                drpForOrderNo.DataBind();
                drpForOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SO # --", ""));

            }
            else
            {
                ViewState["totAmount"] = 0;
                ViewState["totTaxAmount"] = 0;
                ViewState["totNetAmount"] = 0;
                ViewState["totDiscAmt"] = 0;
                ViewState["totAddTaxAmt"] = 0;
                ViewState["totSGST"] = 0;
                ViewState["totCGST"] = 0;
                ViewState["totIGST"] = 0;

                totAmount = 0;
                totTaxAmount = 0;
                totNetAmount = 0;
                totDiscAmt = 0;
                totAddTaxAmt = 0;
                totCGST = 0;
                totSGST = 0;
                totIGST = 0;
            }
        }

        protected void rptOrderDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;

                if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0"
                    || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                        strErr += "<li>" + "Product Selection is required." + "</li>";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                        strErr += "<li>" + "Quantity is required." + "</li>";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                        strErr += "<li>" + "Unit Rate is required." + "</li>";

                }
                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

                    if (dtDetail != null)
                    {
                        foreach (System.Data.DataColumn col in dtDetail.Columns) col.AllowDBNull = true;

                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + ((HiddenField)e.Item.FindControl("hdnProductID")).Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                            return;
                        }
                        DataRow dr = dtDetail.NewRow();
                        dr["pkID"] = 0;
                        //string icode = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedValue;
                        //string iname = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedItem.Text;
                        string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                        string taxtype = ((HiddenField)e.Item.FindControl("hdnTaxType")).Value;
                        string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                        string untqty = ((TextBox)e.Item.FindControl("txtUnitQty")).Text;
                        string unitqty = ((HiddenField)e.Item.FindControl("hdnUnitQuantity")).Value;
                        string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                        string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                        string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                        string disper = ((TextBox)e.Item.FindControl("txtDiscountPercent")).Text;
                        string disamt = ((TextBox)e.Item.FindControl("txtDiscountAmt")).Text;
                        string netrate = ((TextBox)e.Item.FindControl("txtNetRate")).Text;
                        string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;
                        string taxrate = ((TextBox)e.Item.FindControl("txtTaxRate")).Text;
                        string taxamt = ((TextBox)e.Item.FindControl("txtTaxAmount")).Text;
                        string addtaxper = ((TextBox)e.Item.FindControl("txtAddTaxPer")).Text;
                        string addtaxamt = ((TextBox)e.Item.FindControl("txtAddTaxAmt")).Text;
                        string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;

                        string cgstper = ((HiddenField)e.Item.FindControl("hdnCGSTPer")).Value;
                        string sgstper = ((HiddenField)e.Item.FindControl("hdnSGSTPer")).Value;
                        string igstper = ((HiddenField)e.Item.FindControl("hdnIGSTPer")).Value;

                        string cgstamt = ((HiddenField)e.Item.FindControl("hdnCGSTAmt")).Value;
                        string sgstamt = ((HiddenField)e.Item.FindControl("hdnSGSTAmt")).Value;
                        string igstamt = ((HiddenField)e.Item.FindControl("hdnIGSTAmt")).Value;

                        string headdiscamt = ((TextBox)e.Item.FindControl("txtHeaderDiscAmt")).Text;
                        string fororderno = ((DropDownList)e.Item.FindControl("drpForOrderNo")).SelectedValue;

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        DataTable mySpecs = new DataTable();
                        List<Entity.Products> ProdSpec = new List<Entity.Products>();

                        ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList("", Convert.ToInt64(icode), Session["LoginUserID"].ToString());
                        if (Session["mySpecs"] != null)
                        {
                            mySpecs = (DataTable)Session["mySpecs"];

                            DataRow drTemp = mySpecs.NewRow();
                            drTemp["pkID"] = Convert.ToInt64(icode);
                            drTemp["ProductSpecification"] = (ProdSpec.Count > 0) ? ProdSpec[0].ProductSpecification : "";
                            mySpecs.Rows.Add(drTemp);
                        }
                        else
                            mySpecs = PageBase.ConvertListToDataTable(ProdSpec);


                        mySpecs.AcceptChanges();
                        Session.Add("mySpecs", mySpecs);
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                        dr["ChallanNo"] = txtInvoiceNo.Text;
                        dr["QuotationNo"] = "";
                        dr["InquiryNo"] = "";
                        dr["OrderNo"] = "";
                        //dr["BundleID"] = 0;
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["TaxType"] = (!String.IsNullOrEmpty(taxtype)) ? Convert.ToInt16(taxtype) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["UnitQuantity"] = (!String.IsNullOrEmpty(unitqty)) ? Convert.ToInt64(unitqty) : 0;
                        dr["UnitQty"] = (!String.IsNullOrEmpty(untqty)) ? Convert.ToDecimal(untqty) : 0;
                        dr["Qty"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                        dr["Rate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["UnitPrice"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["DiscountPer"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                        dr["DiscountPercent"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                        dr["DiscountAmt"] = (!String.IsNullOrEmpty(disamt)) ? Convert.ToDecimal(disamt) : 0;
                        dr["NetRate"] = (!String.IsNullOrEmpty(netrate)) ? Convert.ToDecimal(netrate) : 0;
                        dr["Amount"] = (!String.IsNullOrEmpty(amt)) ? Convert.ToDecimal(amt) : 0;
                        dr["CGSTPer"] = (!String.IsNullOrEmpty(cgstper)) ? Convert.ToDecimal(cgstper) : 0;
                        dr["SGSTPer"] = (!String.IsNullOrEmpty(sgstper)) ? Convert.ToDecimal(sgstper) : 0;
                        dr["IGSTPer"] = (!String.IsNullOrEmpty(igstper)) ? Convert.ToDecimal(igstper) : 0;
                        dr["CGSTAmt"] = (!String.IsNullOrEmpty(cgstamt)) ? Convert.ToDecimal(cgstamt) : 0;
                        dr["SGSTAmt"] = (!String.IsNullOrEmpty(sgstamt)) ? Convert.ToDecimal(sgstamt) : 0;
                        dr["IGSTAmt"] = (!String.IsNullOrEmpty(igstamt)) ? Convert.ToDecimal(igstamt) : 0;

                        dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
                        dr["TaxAmount"] = (!String.IsNullOrEmpty(taxamt)) ? Convert.ToDecimal(taxamt) : 0;

                        dr["AddTaxPer"] = (!String.IsNullOrEmpty(addtaxper)) ? Convert.ToDecimal(addtaxper) : 0;
                        dr["AddTaxAmt"] = (!String.IsNullOrEmpty(addtaxamt)) ? Convert.ToDecimal(addtaxamt) : 0;
                        dr["NetAmt"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                        dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                        dr["HeaderDiscAmt"] = (!String.IsNullOrEmpty(headdiscamt)) ? Convert.ToDecimal(headdiscamt) : 0;
                        dr["ForOrderNo"] = (!String.IsNullOrEmpty(fororderno)) ? fororderno : "";

                        dtDetail.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptOrderDetail.DataSource = dtDetail;
                        rptOrderDetail.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtDetail", dtDetail);
                        strErr += "<li>" + "Item Added Successfully !" + "</li>";
                    }
                }

            }
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                // --------------------------------- Delete Record
                string iname = ((HiddenField)e.Item.FindControl("edProductName")).Value;

                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (dr["ProductName"].ToString() == iname)
                    {
                        dtDetail.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }
                rptOrderDetail.DataSource = dtDetail;
                rptOrderDetail.DataBind();
                Session.Add("dtDetail", dtDetail);
                strErr += "<li>" + "Item Deleted Successfully !" + "</li>";
            }
            // -------------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        }
        public void funCalculateTotal()
        {

            hdnOthChrgGST1.Value = (String.IsNullOrEmpty(hdnOthChrgGST1.Value) ? 0 : Convert.ToDecimal(hdnOthChrgGST1.Value)).ToString("0.00");
            hdnOthChrgGST2.Value = (String.IsNullOrEmpty(hdnOthChrgGST2.Value) ? 0 : Convert.ToDecimal(hdnOthChrgGST2.Value)).ToString("0.00");
            hdnOthChrgGST3.Value = (String.IsNullOrEmpty(hdnOthChrgGST3.Value) ? 0 : Convert.ToDecimal(hdnOthChrgGST3.Value)).ToString("0.00");
            hdnOthChrgGST4.Value = (String.IsNullOrEmpty(hdnOthChrgGST4.Value) ? 0 : Convert.ToDecimal(hdnOthChrgGST4.Value)).ToString("0.00");
            hdnOthChrgGST5.Value = (String.IsNullOrEmpty(hdnOthChrgGST5.Value) ? 0 : Convert.ToDecimal(hdnOthChrgGST5.Value)).ToString("0.00");

            hdnOthChrgBasic1.Value = (String.IsNullOrEmpty(hdnOthChrgBasic1.Value) ? 0 : Convert.ToDecimal(hdnOthChrgBasic1.Value)).ToString("0.00");
            hdnOthChrgBasic2.Value = (String.IsNullOrEmpty(hdnOthChrgBasic2.Value) ? 0 : Convert.ToDecimal(hdnOthChrgBasic2.Value)).ToString("0.00");
            hdnOthChrgBasic3.Value = (String.IsNullOrEmpty(hdnOthChrgBasic3.Value) ? 0 : Convert.ToDecimal(hdnOthChrgBasic3.Value)).ToString("0.00");
            hdnOthChrgBasic4.Value = (String.IsNullOrEmpty(hdnOthChrgBasic4.Value) ? 0 : Convert.ToDecimal(hdnOthChrgBasic4.Value)).ToString("0.00");
            hdnOthChrgBasic5.Value = (String.IsNullOrEmpty(hdnOthChrgBasic5.Value) ? 0 : Convert.ToDecimal(hdnOthChrgBasic5.Value)).ToString("0.00");

            hdnTotCGSTAmt.Value = Convert.ToDecimal(ViewState["totCGST"]).ToString("0.00");
            hdnTotSGSTAmt.Value = Convert.ToDecimal(ViewState["totSGST"]).ToString("0.00");
            hdnTotIGSTAmt.Value = Convert.ToDecimal(ViewState["totIGST"]).ToString("0.00");

            txtTotOthChrgBeforeGST.Text = (
                                            ((Convert.ToDecimal(hdnOthChrgGST1.Value) > 0) ? (Convert.ToDecimal(hdnOthChrgBasic1.Value)) : 0) +
                                            ((Convert.ToDecimal(hdnOthChrgGST2.Value) > 0) ? (Convert.ToDecimal(hdnOthChrgBasic2.Value)) : 0) +
                                            ((Convert.ToDecimal(hdnOthChrgGST3.Value) > 0) ? (Convert.ToDecimal(hdnOthChrgBasic3.Value)) : 0) +
                                            ((Convert.ToDecimal(hdnOthChrgGST4.Value) > 0) ? (Convert.ToDecimal(hdnOthChrgBasic4.Value)) : 0) +
                                            ((Convert.ToDecimal(hdnOthChrgGST5.Value) > 0) ? (Convert.ToDecimal(hdnOthChrgBasic5.Value)) : 0)
                                            ).ToString("0.00");

            txtTotBasicAmt.Text = Convert.ToDecimal(ViewState["totAmount"]).ToString("0.00");
            txtTotAddTaxAmt.Text = Convert.ToDecimal(ViewState["totAddTaxAmt"]).ToString("0.00");
            hdnTotItemGST.Value = (Convert.ToDecimal(hdnTotCGSTAmt.Value) + Convert.ToDecimal(hdnTotSGSTAmt.Value) + Convert.ToDecimal(hdnTotIGSTAmt.Value)).ToString("0.00");
            txtTotGST.Text = (Convert.ToDecimal(hdnTotCGSTAmt.Value) + Convert.ToDecimal(hdnTotSGSTAmt.Value) + Convert.ToDecimal(hdnTotIGSTAmt.Value) + Convert.ToDecimal(hdnOthChrgGST1.Value) + Convert.ToDecimal(hdnOthChrgGST2.Value) + Convert.ToDecimal(hdnOthChrgGST3.Value) + Convert.ToDecimal(hdnOthChrgGST4.Value) + Convert.ToDecimal(hdnOthChrgGST5.Value)).ToString("0.00");

            txtTotOthChrgAfterGST.Text = (
                                            ((Convert.ToDecimal(hdnOthChrgGST1.Value) > 0) ? 0 : (Convert.ToDecimal(hdnOthChrgBasic1.Value))) +
                                            ((Convert.ToDecimal(hdnOthChrgGST2.Value) > 0) ? 0 : (Convert.ToDecimal(hdnOthChrgBasic2.Value))) +
                                            ((Convert.ToDecimal(hdnOthChrgGST3.Value) > 0) ? 0 : (Convert.ToDecimal(hdnOthChrgBasic3.Value))) +
                                            ((Convert.ToDecimal(hdnOthChrgGST4.Value) > 0) ? 0 : (Convert.ToDecimal(hdnOthChrgBasic4.Value))) +
                                            ((Convert.ToDecimal(hdnOthChrgGST5.Value) > 0) ? 0 : (Convert.ToDecimal(hdnOthChrgBasic5.Value)))
                                            ).ToString("0.00");

            decimal NetAmt = 0;
            NetAmt = (Convert.ToDecimal(ViewState["totNetAmount"]) + (Convert.ToDecimal(hdnOthChrgGST1.Value)) - (((!String.IsNullOrEmpty(txtHeadDiscount.Text)) ? Convert.ToDecimal(txtHeadDiscount.Text) : 0))
                + (Convert.ToDecimal(hdnOthChrgGST2.Value)) + (Convert.ToDecimal(hdnOthChrgGST3.Value)) + (Convert.ToDecimal(hdnOthChrgGST4.Value))
                + (Convert.ToDecimal(hdnOthChrgGST5.Value)) + (Convert.ToDecimal(hdnOthChrgBasic1.Value)) + (Convert.ToDecimal(hdnOthChrgBasic2.Value))
                + (Convert.ToDecimal(hdnOthChrgBasic3.Value)) + (Convert.ToDecimal(hdnOthChrgBasic4.Value)) + (Convert.ToDecimal(hdnOthChrgBasic5.Value))
                    );

            txtTotNetAmt.Text = Math.Round(NetAmt, 0).ToString("0.00");
            txtRoff.Text = (Math.Round(NetAmt, 0) - Math.Round(NetAmt, 2)).ToString("0.00");
        }
        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            Control rptFootCtrl = rptOrderDetail.Controls[rptOrderDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
                HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));
                TextBox txtUnitQty = ((TextBox)rptFootCtrl.FindControl("txtUnitQty"));
                TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
                TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
                TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
                TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
                TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
                TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
                TextBox txtAddTaxPer = ((TextBox)rptFootCtrl.FindControl("txtAddTaxPer"));
                TextBox txtAddTaxAmt = ((TextBox)rptFootCtrl.FindControl("txtAddTaxAmt"));
                HiddenField hdnTaxType = ((HiddenField)rptFootCtrl.FindControl("hdnTaxType"));

                HiddenField hdnCGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTPer"));
                HiddenField hdnSGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTPer"));
                HiddenField hdnIGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTPer"));

                HiddenField hdnCGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTAmt"));
                HiddenField hdnSGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTAmt"));
                HiddenField hdnIGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTAmt"));

                List<Entity.Products> lstEntity = new List<Entity.Products>();

                if (!String.IsNullOrEmpty(hdnProductID.Value))
                    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
                txtUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
                txtDiscountPercent.Text = "0";
                txtDiscountAmt.Text = "0";
                hdnUnitQuantity.Value = (lstEntity.Count > 0) ? ((!String.IsNullOrEmpty(lstEntity[0].UnitQuantity.ToString())) ? lstEntity[0].UnitQuantity.ToString() : "1") : "1";

                //if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                //{
                txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
                hdnTaxType.Value = (lstEntity.Count > 0) ? lstEntity[0].TaxType.ToString() : "0";
                txtAddTaxPer.Text = (lstEntity.Count > 0) ? lstEntity[0].AddTaxPer.ToString() : "0";
                hdnProductUnitQty.Value = (lstEntity.Count > 0) ? lstEntity[0].UnitQuantity.ToString() : "0";
                txtAddTaxAmt.Text = "0";
                //}

                if (BAL.CommonMgmt.isIGST(hdnCustStateID.Value, Session["CompanyStateCode"].ToString()))
                {
                    hdnIGSTPer.Value = txtTaxRate.Text;
                    hdnCGSTPer.Value = "0";
                    hdnSGSTPer.Value = "0";
                }
                else
                {
                    hdnCGSTPer.Value = (Convert.ToDecimal(txtTaxRate.Text) / 2).ToString();
                    hdnSGSTPer.Value = (Convert.ToDecimal(txtTaxRate.Text) / 2).ToString();
                    hdnIGSTPer.Value = "0";
                }
                if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                    txtUnitQty.Focus();
                else
                    txtQuantity.Focus();
                editItem_TextChanged1();

                // ----------------------------------------------------------------------------
                // Binding Customer's Purchase Order
                // -----------------------------------------------------
                DropDownList drpForOrderNo = (DropDownList)rptFootCtrl.FindControl("drpForOrderNo");
                drpForOrderNo.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                {
                    if (!String.IsNullOrEmpty(hdnProductID.Value))
                    {
                        if (hdnStockOutward.Value == "sale")
                        {
                            drpForOrderNo.DataSource = BindSalesOrderList(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value));
                            drpForOrderNo.DataTextField = "OrderNo";
                            drpForOrderNo.DataValueField = "OrderNo";
                            drpForOrderNo.DataBind();
                            drpForOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SO # --", ""));
                        }
                        else
                        {
                            drpForOrderNo.DataSource = BindOutwardList(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value));
                            drpForOrderNo.DataTextField = "OutwardNo";
                            drpForOrderNo.DataValueField = "OutwardNo";
                            drpForOrderNo.DataBind();
                            drpForOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Outward # --", ""));
                        }
                    }

                }


                // ----------------------------------------------------------------------------
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtQuantity);", true);
            }
            else
            {
                strErr += "<li> Select Proper Item From List !</li>";
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }
        [System.Web.Services.WebMethod]
        public static string DeleteSalesChallan(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.SalesChallanMgmt.DeleteSalesChallan(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
        protected void txtUnitQty_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void txtUnitRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void txtDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void txtDiscountAmt_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void txtTaxRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void txtAddTaxPer_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }
        protected void editItem_TextChanged1()
        {
            Control rptFootCtrl = rptOrderDetail.Controls[rptOrderDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
            HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));
            TextBox txtUnitQty = ((TextBox)rptFootCtrl.FindControl("txtUnitQty"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
            TextBox txtNetRate = ((TextBox)rptFootCtrl.FindControl("txtNetRate"));

            TextBox txtAmount = ((TextBox)rptFootCtrl.FindControl("txtAmount"));
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
            TextBox txtTaxAmount = ((TextBox)rptFootCtrl.FindControl("txtTaxAmount"));

            TextBox txtAddTaxPer = ((TextBox)rptFootCtrl.FindControl("txtAddTaxPer"));
            TextBox txtAddTaxAmt = ((TextBox)rptFootCtrl.FindControl("txtAddTaxAmt"));

            TextBox txtNetAmount = ((TextBox)rptFootCtrl.FindControl("txtNetAmount"));
            HiddenField hdnTaxType = ((HiddenField)rptFootCtrl.FindControl("hdnTaxType"));

            HiddenField hdnCGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTPer"));
            HiddenField hdnSGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTPer"));
            HiddenField hdnIGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTPer"));

            HiddenField hdnCGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTAmt"));
            HiddenField hdnSGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTAmt"));
            HiddenField hdnIGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTAmt"));
            //// --------------------------------------------------------------------------
            //Decimal q = 0;
            //if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
            //    q = String.IsNullOrEmpty(txtUnitQty.Text) ? 0 : (Convert.ToDecimal(txtUnitQty.Text) * Convert.ToDecimal(hdnProductUnitQty.Value));
            //else
            //    q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);

            ////Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            //Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);

            // --------------------------------------------------------------------------
            Decimal uq = 0, txtUQ = 0;
            uq = String.IsNullOrEmpty(hdnUnitQuantity.Value) ? 1 : Convert.ToDecimal(hdnUnitQuantity.Value);    // 50

            if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")        // ShaktiPet    
            {
                txtUQ = String.IsNullOrEmpty(txtUnitQty.Text) ? 1 : Convert.ToDecimal(txtUnitQty.Text); // 2
                txtQuantity.Text = (uq * txtUQ).ToString();
                uq = 1;
            }

            Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);
            // ------------------------------------------------------------------
            Decimal dp = String.IsNullOrEmpty(txtDiscountPercent.Text) ? 0 : Convert.ToDecimal(txtDiscountPercent.Text);
            Decimal dpa = String.IsNullOrEmpty(txtDiscountAmt.Text) ? 0 : Convert.ToDecimal(txtDiscountAmt.Text);
            Decimal nr = String.IsNullOrEmpty(txtNetRate.Text) ? 0 : Convert.ToDecimal(txtNetRate.Text);
            Decimal a = String.IsNullOrEmpty(txtAmount.Text) ? 0 : Convert.ToDecimal(txtAmount.Text);
            Decimal tr = String.IsNullOrEmpty(txtTaxRate.Text) ? 0 : Convert.ToDecimal(txtTaxRate.Text);
            Decimal ta = String.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
            Decimal at = String.IsNullOrEmpty(txtAddTaxPer.Text) ? 0 : Convert.ToDecimal(txtAddTaxPer.Text);
            Decimal ata = String.IsNullOrEmpty(txtAddTaxAmt.Text) ? 0 : Convert.ToDecimal(txtAddTaxAmt.Text);
            Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            Int16 taxtype = Convert.ToInt16(String.IsNullOrEmpty(hdnTaxType.Value) ? 0 : Convert.ToInt16(hdnTaxType.Value));

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
            //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(), 0, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

            txtQuantity.Text = q.ToString();
            txtDiscountPercent.Text = ItmDiscPer1.ToString();
            txtDiscountAmt.Text = ItmDiscAmt1.ToString();
            txtAddTaxAmt.Text = AddTaxAmt.ToString();
            txtTaxRate.Text = (CGSTPer + SGSTPer + IGSTPer).ToString();
            txtTaxAmount.Text = (CGSTAmt + SGSTAmt + IGSTAmt).ToString();
            txtNetRate.Text = NetRate.ToString();
            txtAmount.Text = BasicAmt.ToString();
            txtNetAmount.Text = NetAmt.ToString();

            hdnCGSTPer.Value = CGSTPer.ToString();
            hdnSGSTPer.Value = SGSTPer.ToString();
            hdnIGSTPer.Value = IGSTPer.ToString();

            hdnCGSTAmt.Value = CGSTAmt.ToString();
            hdnSGSTAmt.Value = SGSTAmt.ToString();
            hdnIGSTAmt.Value = IGSTAmt.ToString();

        }
        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            //For Footer Section

            //For repeater other section
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            HiddenField edUnitQuantity = (HiddenField)item.FindControl("edUnitQuantity");
            HiddenField edTaxType = (HiddenField)item.FindControl("edTaxType");
            TextBox edUnitQty = (TextBox)item.FindControl("edUnitQty");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
            TextBox edDiscountPercent = (TextBox)item.FindControl("edDiscountPercent");
            TextBox edDiscountAmt = (TextBox)item.FindControl("edDiscountAmt");
            TextBox edNetRate = (TextBox)item.FindControl("edNetRate");
            TextBox edAmount = (TextBox)item.FindControl("edAmount");
            TextBox edTaxRate = (TextBox)item.FindControl("edTaxRate");
            TextBox edTaxAmount = (TextBox)item.FindControl("edTaxAmount");
            TextBox edAddTaxPer = (TextBox)item.FindControl("edAddTaxPer");
            TextBox edAddTaxAmt = (TextBox)item.FindControl("edAddTaxAmt");
            TextBox edNetAmount = (TextBox)item.FindControl("edNetAmount");


            HiddenField edhdnCGSTPer = ((HiddenField)item.FindControl("edhdnCGSTPer"));
            HiddenField edhdnSGSTPer = ((HiddenField)item.FindControl("edhdnSGSTPer"));
            HiddenField edhdnIGSTPer = ((HiddenField)item.FindControl("edhdnIGSTPer"));

            HiddenField edhdnCGSTAmt = ((HiddenField)item.FindControl("edhdnCGSTAmt"));
            HiddenField edhdnSGSTAmt = ((HiddenField)item.FindControl("edhdnSGSTAmt"));
            HiddenField edhdnIGSTAmt = ((HiddenField)item.FindControl("edhdnIGSTAmt"));
            // --------------------------------------------------------------------------
            Decimal uq = 0, txtUQ = 0;
            uq = (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? Convert.ToDecimal(edUnitQuantity.Value) : 1;

            if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")        // ShaktiPet    
            {
                txtUQ = String.IsNullOrEmpty(edUnitQty.Text) ? 1 : Convert.ToDecimal(edUnitQty.Text); // 2
                edQuantity.Text = (uq * txtUQ).ToString();
                uq = 1;
            }
            // --------------------------------------------------------------------------
            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            Decimal ur = (!String.IsNullOrEmpty(edUnitRate.Text)) ? Convert.ToDecimal(edUnitRate.Text) : 0;
            Decimal dp = (!String.IsNullOrEmpty(edDiscountPercent.Text)) ? Convert.ToDecimal(edDiscountPercent.Text) : 0;
            Decimal dpa = (!String.IsNullOrEmpty(edDiscountAmt.Text)) ? Convert.ToDecimal(edDiscountAmt.Text) : 0;
            Decimal nr = (!String.IsNullOrEmpty(edNetRate.Text)) ? Convert.ToDecimal(edNetRate.Text) : 0;
            Decimal a = (!String.IsNullOrEmpty(edAmount.Text)) ? Convert.ToDecimal(edAmount.Text) : 0;
            Decimal tr = (!String.IsNullOrEmpty(edTaxRate.Text)) ? Convert.ToDecimal(edTaxRate.Text) : 0;
            Decimal ta = (!String.IsNullOrEmpty(edTaxAmount.Text)) ? Convert.ToDecimal(edTaxAmount.Text) : 0;
            Decimal at = (!String.IsNullOrEmpty(edAddTaxPer.Text)) ? Convert.ToDecimal(edAddTaxPer.Text) : 0;
            Decimal ata = (!String.IsNullOrEmpty(edAddTaxAmt.Text)) ? Convert.ToDecimal(edAddTaxAmt.Text) : 0;
            Decimal na = (!String.IsNullOrEmpty(edNetAmount.Text)) ? Convert.ToDecimal(edNetAmount.Text) : 0;
            Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(edTaxType.Value)) ? Convert.ToInt16(edTaxType.Value) : 0);
            // --------------------------------------------------------------------------
            //nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            //a = Math.Round((q * nr), 2);
            //ta = Math.Round(((a * tr) / 100), 2);
            //na = Math.Round((a + ta), 2);

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
            //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(),0, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

            edDiscountPercent.Text = ItmDiscPer1.ToString();
            edDiscountAmt.Text = ItmDiscAmt1.ToString();
            edAddTaxAmt.Text = AddTaxAmt.ToString();
            edTaxRate.Text = (CGSTPer + SGSTPer + IGSTPer).ToString();
            edTaxAmount.Text = (CGSTAmt + SGSTAmt + IGSTAmt).ToString();
            edNetRate.Text = NetRate.ToString();
            edAmount.Text = BasicAmt.ToString();
            edNetAmount.Text = NetAmt.ToString();

            edhdnCGSTPer.Value = CGSTPer.ToString();
            edhdnSGSTPer.Value = SGSTPer.ToString();
            edhdnIGSTPer.Value = IGSTPer.ToString();

            edhdnCGSTAmt.Value = CGSTAmt.ToString();
            edhdnSGSTAmt.Value = SGSTAmt.ToString();
            edhdnIGSTAmt.Value = IGSTAmt.ToString();

            //edNetRate.Text = nr.ToString();
            //edAmount.Text = a.ToString();
            //edTaxAmount.Text = ta.ToString();
            //edNetAmount.Text = na.ToString();

            // --------------------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductID"].ToString() == edProductID.Value)
                {
                    row.SetField("UnitQty", edUnitQty.Text);
                    //if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                    //{
                    //    hdnProductUnitQty.Value = (!String.IsNullOrEmpty(hdnProductUnitQty.Value)) ? hdnProductUnitQty.Value : "1";
                    //    row.SetField("Qty", (Convert.ToInt64(edUnitQty.Text)) * (Convert.ToInt64(hdnProductUnitQty.Value)));
                    //}
                    //else
                    //{
                    //    row.SetField("Qty", edQuantity.Text);
                    //}
                    row.SetField("UnitQuantity", (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? edUnitQuantity.Value : "0");
                    row.SetField("UnitQty", edUnitQty.Text);
                    if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")    // ShaktiPet
                    {
                        hdnProductUnitQty.Value = (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? edUnitQuantity.Value : "1";
                        row.SetField("Quantity", (Convert.ToDecimal(edUnitQty.Text)) * (Convert.ToDecimal(edUnitQuantity.Value)));
                        row.SetField("Qty", (Convert.ToDecimal(edUnitQty.Text)) * (Convert.ToDecimal(edUnitQuantity.Value)));
                    }
                    else
                    {       
                        row.SetField("Quantity", edQuantity.Text);
                        row.SetField("Qty", edQuantity.Text);
                    }
                    row.SetField("TaxType", edTaxType.Value);
                    row.SetField("Rate", edUnitRate.Text);
                    row.SetField("DiscountPer", edDiscountPercent.Text);
                    row.SetField("DiscountAmt", edDiscountAmt.Text);
                    row.SetField("NetRate", edNetRate.Text);
                    row.SetField("Amount", edAmount.Text);
                    row.SetField("CGSTPer", edhdnCGSTPer.Value);
                    row.SetField("SGSTPer", edhdnSGSTPer.Value);
                    row.SetField("IGSTPer", edhdnIGSTPer.Value);
                    row.SetField("CGSTAmt", edhdnCGSTAmt.Value);
                    row.SetField("SGSTAmt", edhdnSGSTAmt.Value);
                    row.SetField("IGSTAmt", edhdnIGSTAmt.Value);
                    row.SetField("TaxRate", edTaxRate.Text);
                    row.SetField("TaxAmount", edTaxAmount.Text);
                    row.SetField("AddTaxPer", edAddTaxPer.Text);
                    row.SetField("AddTaxAmt", edAddTaxAmt.Text);
                    row.SetField("NetAmt", edNetAmount.Text);
                    row.AcceptChanges();
                }
            }
            dtDetail.AcceptChanges();
            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
        }
        protected void drpOthChrg1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt1_TextChanged(sender, e);
            if (drpOthChrg1.SelectedValue == "")
            {
                txtOthChrgAmt1.Text = "0";
            }
        }

        protected void txtOthChrgAmt1_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg1.SelectedValue)) ? Convert.ToInt64(drpOthChrg1.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt1.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt1.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out OthChrgGSTAmt, out OthChrgBasicAmt);

            hdnOthChrgGST1.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic1.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void drpOthChrg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt2_TextChanged(sender, e);
            if (drpOthChrg2.SelectedValue == "")
            {
                txtOthChrgAmt2.Text = "0";
            }
        }

        protected void txtOthChrgAmt2_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg2.SelectedValue)) ? Convert.ToInt64(drpOthChrg2.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt2.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt2.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out OthChrgGSTAmt, out OthChrgBasicAmt);

            hdnOthChrgGST2.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic2.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void drpOthChrg3_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt3_TextChanged(sender, e);
            if (drpOthChrg3.SelectedValue == "")
            {
                txtOthChrgAmt3.Text = "0";
            }
        }

        protected void txtOthChrgAmt3_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg3.SelectedValue)) ? Convert.ToInt64(drpOthChrg3.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt3.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt3.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out OthChrgGSTAmt, out OthChrgBasicAmt);

            hdnOthChrgGST3.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic3.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void drpOthChrg4_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt4_TextChanged(sender, e);
            if (drpOthChrg4.SelectedValue == "")
            {
                txtOthChrgAmt4.Text = "0";
            }
        }

        protected void txtOthChrgAmt4_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg4.SelectedValue)) ? Convert.ToInt64(drpOthChrg4.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt4.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt4.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out OthChrgGSTAmt, out OthChrgBasicAmt);

            hdnOthChrgGST4.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic4.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void drpOthChrg5_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt5_TextChanged(sender, e);
            if (drpOthChrg5.SelectedValue == "")
            {
                txtOthChrgAmt5.Text = "0";
            }
        }

        protected void txtOthChrgAmt5_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg5.SelectedValue)) ? Convert.ToInt64(drpOthChrg5.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt5.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt5.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out OthChrgGSTAmt, out OthChrgBasicAmt);

            hdnOthChrgGST5.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic5.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void txtHeadDiscount_TextChanged(object sender, EventArgs e)
        {
            //this.rptOrderDetail.ItemDataBound  -= rptOrderDetail_ItemDataBound;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            //decimal TotalAmt = Convert.ToDecimal(txtTotBasicAmt.Text) + Convert.ToDecimal(txtTotGST.Text) + Convert.ToDecimal(txtTotAddTaxAmt.Text);
            decimal TotalAmt = 0;
            decimal HeaderDiscAmt = (!String.IsNullOrEmpty(txtHeadDiscount.Text)) ? Convert.ToDecimal(txtHeadDiscount.Text) : 0;
            decimal HeaderDiscItemWise = 0;

            if (dtDetail != null)
            {
                foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                foreach (DataRow row in dtDetail.Rows)
                {
                    TotalAmt += (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                }

                foreach (DataRow row in dtDetail.Rows)
                {
                    //Convert.ToDecimal(row["CGSTPer"])
                    HeaderDiscItemWise = 0;
                    Decimal a = (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                    Decimal q = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                    Decimal ur = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                    Decimal dp = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                    Decimal dpa = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                    Decimal tr = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                    Decimal at = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;
                    Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);

                    HeaderDiscItemWise = Math.Round((HeaderDiscAmt * a) / TotalAmt, 2);

                    decimal TaxAmt = 0;
                    decimal CGSTPer = 0, CGSTAmt = 0;
                    decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
                    BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, HeaderDiscItemWise, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
                    //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(), HeaderDiscItemWise, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

                    row.SetField("HeaderDiscAmt", HeaderDiscItemWise);
                    row.SetField("NetRate", NetRate);
                    row.SetField("Amount", BasicAmt);
                    row.SetField("CGSTPer", CGSTPer);
                    row.SetField("SGSTPer", SGSTPer);
                    row.SetField("IGSTPer", IGSTPer);
                    row.SetField("CGSTAmt", CGSTAmt);
                    row.SetField("SGSTAmt", SGSTAmt);
                    row.SetField("IGSTAmt", IGSTAmt);
                    row.SetField("TaxRate", CGSTPer + SGSTPer + IGSTPer);
                    row.SetField("TaxAmount", CGSTAmt + SGSTAmt + IGSTAmt);
                    row.SetField("AddTaxAmt", AddTaxAmt);
                    row.SetField("NetAmt", NetAmt);
                }

                rptOrderDetail.DataSource = dtDetail;
                rptOrderDetail.DataBind();
                funCalculateTotal();
                Session.Add("dtDetail", dtDetail);
            }
            else
            {
                txtHeadDiscount.Text = "0";
            }
        }

        protected void drpTNC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpTNC.SelectedValue) && drpTNC.SelectedValue != "0")
            {
                string tmpval = drpTNC.SelectedValue;
                List<Entity.Contents> lstList2 = new List<Entity.Contents>();
                lstList2 = BAL.CommonMgmt.GetContentList(Convert.ToInt64(drpTNC.SelectedValue), "TNC");
                if (lstList2.Count > 0)
                    txtTermsCondition.Text = lstList2[0].TNC_Content;
            }
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.SalesChallan> lstEntity = new List<Entity.SalesChallan>();
                // ----------------------------------------------------
                lstEntity = BAL.SalesChallanMgmt.GetSalesChallanList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtInvoiceNo.Text = lstEntity[0].ChallanNo;
                txtInvoiceDate.Text = lstEntity[0].ChallanDate.ToString("yyyy-MM-dd");
                txtSupplierRef.Text = lstEntity[0].SupplierRef;
                txtSupplierRefDate.Text = lstEntity[0].ChallanDate.ToString("yyyy-MM-dd");
                txtOtherRef.Text = lstEntity[0].OtherRef;
                txtDisDocNo.Text = lstEntity[0].DispatchDocNo;
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                drpLedger.SelectedValue = lstEntity[0].FixedLedgerID.ToString();
                drpBank.SelectedValue = lstEntity[0].BankID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpLocation.SelectedValue = lstEntity[0].LocationID.ToString();

                drpTerminationOfDelivery.SelectedValue = (lstEntity[0].TerminationOfDeliery > 0) ? lstEntity[0].TerminationOfDeliery.ToString() : "";
                drpTerminationOfDelivery_SelectedIndexChanged(null, null);
                drpTerminationOfDeliveryCity.SelectedValue = (lstEntity[0].TerminationOfDelieryCity > 0) ? lstEntity[0].TerminationOfDelieryCity.ToString() : "";
                txtTermsCondition.Text = lstEntity[0].TermsCondition;

                if (!String.IsNullOrEmpty(lstEntity[0].QuotationNo))
                {
                    BindQuotationList(lstEntity[0].CustomerID);
                    drpReferenceNo.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].QuotationNo)) ? lstEntity[0].QuotationNo : "";
                    rdblOption.SelectedIndex = rdblOption.Items.IndexOf(rdblOption.Items.FindByText("Quotation"));
                }
                else if (!String.IsNullOrEmpty(lstEntity[0].InquiryNo))
                {
                    BindInquiryList(lstEntity[0].CustomerID);
                    drpReferenceNo.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].InquiryNo)) ? lstEntity[0].InquiryNo : "";
                    rdblOption.SelectedIndex = rdblOption.Items.IndexOf(rdblOption.Items.FindByText("Inquiry"));
                }
                else if (!String.IsNullOrEmpty(lstEntity[0].OrderNo))
                {
                    BindSalesOrderList(lstEntity[0].CustomerID);
                    drpReferenceNo.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].OrderNo)) ? lstEntity[0].OrderNo : "";
                    rdblOption.SelectedIndex = rdblOption.Items.IndexOf(rdblOption.Items.FindByText("SalesOrder"));
                }
                else if (!String.IsNullOrEmpty(lstEntity[0].ComplaintNo))
                {
                    BindComplaintList(lstEntity[0].CustomerID);
                    drpReferenceNo.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].ComplaintNo)) ? lstEntity[0].ComplaintNo : "";
                    rdblOption.SelectedIndex = rdblOption.Items.IndexOf(rdblOption.Items.FindByText("Complaint"));
                }
                rdblOption_SelectedIndexChanged(null, null);

                txtTotBasicAmt.Text = lstEntity[0].BasicAmt.ToString();
                txtHeadDiscount.Text = lstEntity[0].DiscountAmt.ToString();
                txtTotGST.Text = lstEntity[0].BasicAmt.ToString();
                txtRoff.Text = lstEntity[0].ROffAmt.ToString();
                txtTotNetAmt.Text = lstEntity[0].NetAmt.ToString();

                drpOthChrg1.SelectedValue = (lstEntity[0].ChargeID1 > 0) ? lstEntity[0].ChargeID1.ToString() : "";
                drpOthChrg2.SelectedValue = (lstEntity[0].ChargeID2 > 0) ? lstEntity[0].ChargeID2.ToString() : "";
                drpOthChrg3.SelectedValue = (lstEntity[0].ChargeID3 > 0) ? lstEntity[0].ChargeID3.ToString() : "";
                drpOthChrg4.SelectedValue = (lstEntity[0].ChargeID4 > 0) ? lstEntity[0].ChargeID4.ToString() : "";
                drpOthChrg5.SelectedValue = (lstEntity[0].ChargeID5 > 0) ? lstEntity[0].ChargeID5.ToString() : "";

                txtOthChrgAmt1.Text = (lstEntity[0].ChargeAmt1 > 0) ? lstEntity[0].ChargeAmt1.ToString("0.00") : "0.00";
                txtOthChrgAmt2.Text = (lstEntity[0].ChargeAmt2 > 0) ? lstEntity[0].ChargeAmt2.ToString("0.00") : "0.00";
                txtOthChrgAmt3.Text = (lstEntity[0].ChargeAmt3 > 0) ? lstEntity[0].ChargeAmt3.ToString("0.00") : "0.00";
                txtOthChrgAmt4.Text = (lstEntity[0].ChargeAmt4 > 0) ? lstEntity[0].ChargeAmt4.ToString("0.00") : "0.00";
                txtOthChrgAmt5.Text = (lstEntity[0].ChargeAmt5 > 0) ? lstEntity[0].ChargeAmt5.ToString("0.00") : "0.00";

                hdnOthChrgBasic1.Value = (lstEntity[0].ChargeBasicAmt1 > 0) ? lstEntity[0].ChargeBasicAmt1.ToString("0.00") : "0";
                hdnOthChrgBasic2.Value = (lstEntity[0].ChargeBasicAmt2 > 0) ? lstEntity[0].ChargeBasicAmt2.ToString("0.00") : "0";
                hdnOthChrgBasic3.Value = (lstEntity[0].ChargeBasicAmt3 > 0) ? lstEntity[0].ChargeBasicAmt3.ToString("0.00") : "0";
                hdnOthChrgBasic4.Value = (lstEntity[0].ChargeBasicAmt4 > 0) ? lstEntity[0].ChargeBasicAmt4.ToString("0.00") : "0";
                hdnOthChrgBasic5.Value = (lstEntity[0].ChargeBasicAmt5 > 0) ? lstEntity[0].ChargeBasicAmt5.ToString("0.00") : "0";

                hdnOthChrgGST1.Value = (lstEntity[0].ChargeGSTAmt1 > 0) ? lstEntity[0].ChargeGSTAmt1.ToString("0.00") : "0";
                hdnOthChrgGST2.Value = (lstEntity[0].ChargeGSTAmt2 > 0) ? lstEntity[0].ChargeGSTAmt2.ToString("0.00") : "0";
                hdnOthChrgGST3.Value = (lstEntity[0].ChargeGSTAmt3 > 0) ? lstEntity[0].ChargeGSTAmt3.ToString("0.00") : "0";
                hdnOthChrgGST4.Value = (lstEntity[0].ChargeGSTAmt4 > 0) ? lstEntity[0].ChargeGSTAmt4.ToString("0.00") : "0";
                hdnOthChrgGST5.Value = (lstEntity[0].ChargeGSTAmt5 > 0) ? lstEntity[0].ChargeGSTAmt5.ToString("0.00") : "0";

                drpModeOfTransport.SelectedValue = lstEntity[0].ModeOfTransport;
                txtTransporterName.Text = lstEntity[0].TransporterName;
                txtVehicleNo.Text = lstEntity[0].VehicleNo;
                txtDelMote.Text = lstEntity[0].DeliveryNote;
                txtLRNo.Text = lstEntity[0].LRNo;
                txtLRDate.Text = (lstEntity[0].LRDate != null) ? lstEntity[0].LRDate.Value.ToString("yyyy-MM-dd") : null;
                txteWay.Text = lstEntity[0].EwayBillNo;
                txtModePay.Text = lstEntity[0].ModeOfPayment;
                txtTransportRemark.Text = lstEntity[0].TransportRemark;
                txtDeliverTo.Text = lstEntity[0].DeliverTo;
                // -------------------------------------------------------------------------
                BindSalesBillDetailList(lstEntity[0].ChallanNo);
            }
            // -------------------------------------------------------------------------
            txtCustomerName.Enabled = (pMode.ToLower() == "add") ? true : false;
            txtCustomerName.Focus();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }
        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnChallanNo = "", ReturnMsg1 = "";
            string strErr = "";

            if (String.IsNullOrEmpty(drpLedger.SelectedValue) || String.IsNullOrEmpty(hdnCustomerID.Value) || String.IsNullOrEmpty(drpBank.SelectedValue))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(drpLedger.SelectedValue))
                    strErr += "<li>" + "Ledger Selection is required. " + "</li>";

                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Select Proper Customer From List. " + "</li>";

                if (String.IsNullOrEmpty(drpBank.SelectedValue))
                    strErr += "<li>" + "Bank Selection is required. " + "</li>";


            }
            // --------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            Entity.SalesChallan objEntity = new Entity.SalesChallan();
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        Int64 intLocation = (!String.IsNullOrEmpty(drpLocation.SelectedValue)) ? Convert.ToInt64(drpLocation.SelectedValue) : 0;
                        objEntity.ChallanNo = txtInvoiceNo.Text;
                        objEntity.ChallanDate = Convert.ToDateTime(txtInvoiceDate.Text);
                        objEntity.FixedLedgerID = Convert.ToInt64(drpLedger.SelectedValue);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.BankID = Convert.ToInt64(drpBank.SelectedValue);
                        objEntity.LocationID = intLocation;
                        objEntity.TerminationOfDeliery = (!String.IsNullOrEmpty(drpTerminationOfDelivery.SelectedValue)) ? Convert.ToInt64(drpTerminationOfDelivery.SelectedValue) : 0;
                        objEntity.TerminationOfDelieryCity = (!String.IsNullOrEmpty(drpTerminationOfDeliveryCity.SelectedValue)) ? Convert.ToInt64(drpTerminationOfDeliveryCity.SelectedValue) : 0;
                        objEntity.TermsCondition = txtTermsCondition.Text;
                        objEntity.SupplierRef = txtSupplierRef.Text;
                        //if (!string.IsNullOrEmpty(txtSupplierRefDate.Text))
                        objEntity.SupplierRefDate = Convert.ToDateTime(txtSupplierRefDate.Text);
                        objEntity.OtherRef = txtOtherRef.Text;
                        objEntity.DispatchDocNo = txtDisDocNo.Text;
                        
                        if (rdblOption.SelectedValue == "Quotation")
                        {
                            if (!String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                            {
                                objEntity.QuotationNo = drpReferenceNo.SelectedValue;
                                objEntity.RefType = rdblOption.SelectedValue;
                            }
                        }
                        else if (rdblOption.SelectedValue == "Inquiry")
                        {
                            if (!String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                            {
                                objEntity.InquiryNo = drpReferenceNo.SelectedValue;
                                objEntity.RefType = rdblOption.SelectedValue;
                            }
                        }
                        else if (rdblOption.SelectedValue == "SalesOrder")
                        {
                            if (!String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                            {
                                objEntity.OrderNo = drpReferenceNo.SelectedValue;
                                objEntity.RefType = rdblOption.SelectedValue;
                            }
                        }
                        else if (rdblOption.SelectedValue == "Complaint")
                        {
                            if (!String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                            {
                                objEntity.ComplaintNo = drpReferenceNo.SelectedValue;
                                objEntity.RefType = rdblOption.SelectedValue;
                            }
                        }
                        objEntity.BasicAmt = (!String.IsNullOrEmpty(txtTotBasicAmt.Text)) ? Convert.ToDecimal(txtTotBasicAmt.Text) : 0;
                        objEntity.DiscountAmt = (!String.IsNullOrEmpty(txtHeadDiscount.Text)) ? Convert.ToDecimal(txtHeadDiscount.Text) : 0;
                        objEntity.CGSTAmt = (!String.IsNullOrEmpty(hdnTotCGSTAmt.Value)) ? Convert.ToDecimal(hdnTotCGSTAmt.Value) : 0; ;
                        objEntity.SGSTAmt = (!String.IsNullOrEmpty(hdnTotSGSTAmt.Value)) ? Convert.ToDecimal(hdnTotSGSTAmt.Value) : 0;
                        objEntity.IGSTAmt = (!String.IsNullOrEmpty(hdnTotIGSTAmt.Value)) ? Convert.ToDecimal(hdnTotIGSTAmt.Value) : 0;
                        objEntity.ROffAmt = (!String.IsNullOrEmpty(txtRoff.Text)) ? Convert.ToDecimal(txtRoff.Text) : 0;

                        objEntity.ChargeID1 = (!String.IsNullOrEmpty(drpOthChrg1.SelectedValue)) ? Convert.ToInt64(drpOthChrg1.SelectedValue) : 0;
                        objEntity.ChargeAmt1 = (!String.IsNullOrEmpty(txtOthChrgAmt1.Text)) ? Convert.ToDecimal(txtOthChrgAmt1.Text) : 0;
                        objEntity.ChargeBasicAmt1 = (!String.IsNullOrEmpty(hdnOthChrgBasic1.Value)) ? Convert.ToDecimal(hdnOthChrgBasic1.Value) : 0;
                        objEntity.ChargeGSTAmt1 = (!String.IsNullOrEmpty(hdnOthChrgGST1.Value)) ? Convert.ToDecimal(hdnOthChrgGST1.Value) : 0;

                        objEntity.ChargeID2 = (!String.IsNullOrEmpty(drpOthChrg2.SelectedValue)) ? Convert.ToInt64(drpOthChrg2.SelectedValue) : 0;
                        objEntity.ChargeAmt2 = (!String.IsNullOrEmpty(txtOthChrgAmt2.Text)) ? Convert.ToDecimal(txtOthChrgAmt2.Text) : 0;
                        objEntity.ChargeBasicAmt2 = (!String.IsNullOrEmpty(hdnOthChrgBasic2.Value)) ? Convert.ToDecimal(hdnOthChrgBasic2.Value) : 0;
                        objEntity.ChargeGSTAmt2 = (!String.IsNullOrEmpty(hdnOthChrgGST2.Value)) ? Convert.ToDecimal(hdnOthChrgGST2.Value) : 0;

                        objEntity.ChargeID3 = (!String.IsNullOrEmpty(drpOthChrg3.SelectedValue)) ? Convert.ToInt64(drpOthChrg3.SelectedValue) : 0;
                        objEntity.ChargeAmt3 = (!String.IsNullOrEmpty(txtOthChrgAmt3.Text)) ? Convert.ToDecimal(txtOthChrgAmt3.Text) : 0;
                        objEntity.ChargeBasicAmt3 = (!String.IsNullOrEmpty(hdnOthChrgBasic3.Value)) ? Convert.ToDecimal(hdnOthChrgBasic3.Value) : 0;
                        objEntity.ChargeGSTAmt3 = (!String.IsNullOrEmpty(hdnOthChrgGST3.Value)) ? Convert.ToDecimal(hdnOthChrgGST3.Value) : 0;

                        objEntity.ChargeID4 = (!String.IsNullOrEmpty(drpOthChrg4.SelectedValue)) ? Convert.ToInt64(drpOthChrg4.SelectedValue) : 0;
                        objEntity.ChargeAmt4 = (!String.IsNullOrEmpty(txtOthChrgAmt4.Text)) ? Convert.ToDecimal(txtOthChrgAmt4.Text) : 0;
                        objEntity.ChargeBasicAmt4 = (!String.IsNullOrEmpty(hdnOthChrgBasic4.Value)) ? Convert.ToDecimal(hdnOthChrgBasic4.Value) : 0;
                        objEntity.ChargeGSTAmt4 = (!String.IsNullOrEmpty(hdnOthChrgGST4.Value)) ? Convert.ToDecimal(hdnOthChrgGST4.Value) : 0;

                        objEntity.ChargeID5 = (!String.IsNullOrEmpty(drpOthChrg5.SelectedValue)) ? Convert.ToInt64(drpOthChrg5.SelectedValue) : 0;
                        objEntity.ChargeAmt5 = (!String.IsNullOrEmpty(txtOthChrgAmt5.Text)) ? Convert.ToDecimal(txtOthChrgAmt5.Text) : 0;
                        objEntity.ChargeBasicAmt5 = (!String.IsNullOrEmpty(hdnOthChrgBasic5.Value)) ? Convert.ToDecimal(hdnOthChrgBasic5.Value) : 0;
                        objEntity.ChargeGSTAmt5 = (!String.IsNullOrEmpty(hdnOthChrgGST5.Value)) ? Convert.ToDecimal(hdnOthChrgGST5.Value) : 0;

                        objEntity.NetAmt = (!String.IsNullOrEmpty(txtTotNetAmt.Text)) ? Convert.ToDecimal(txtTotNetAmt.Text) : 0;

                        objEntity.ModeOfTransport = (!String.IsNullOrEmpty(drpModeOfTransport.SelectedValue)) ? Convert.ToString(drpModeOfTransport.SelectedValue) : "";
                        objEntity.TransporterName = txtTransporterName.Text;
                        objEntity.VehicleNo = txtVehicleNo.Text;
                        objEntity.DeliveryNote = txtDelMote.Text;
                        objEntity.LRNo = txtLRNo.Text;
                        objEntity.LRDate = (!String.IsNullOrEmpty(txtLRDate.Text)) ? Convert.ToDateTime(txtLRDate.Text) : (DateTime?)null;
                        objEntity.EwayBillNo = txteWay.Text;
                        objEntity.ModeOfPayment = txtModePay.Text;
                        objEntity.TransportRemark = txtTransportRemark.Text;
                        objEntity.DeliverTo = txtDeliverTo.Text;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.SalesChallanMgmt.AddUpdateSalesChallan(objEntity, out ReturnCode, out ReturnMsg, out ReturnChallanNo);
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnChallanNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnChallanNo) && !String.IsNullOrEmpty(txtInvoiceNo.Text))
                        {
                            ReturnChallanNo = txtInvoiceNo.Text;
                        }
                        BAL.SalesChallanMgmt.DeleteSalesChallanDetailByChallanNo(ReturnChallanNo, out ReturnCode1, out ReturnMsg1);
                        // --------------------------------------------------------------
                        if (ReturnCode1 > 0 && !String.IsNullOrEmpty(ReturnChallanNo))
                        {
                            //DataTable dtDetail = new DataTable();
                            //dtDetail = (DataTable)Session["dtDetail"];
                            // --------------------------------------------------------------

                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            Entity.SalesChallanDetail objQuotDet = new Entity.SalesChallanDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.ChallanNo = ReturnChallanNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.LocationID = intLocation;
                                objQuotDet.TaxType = Convert.ToInt16(dr["TaxType"]);
                                objQuotDet.UnitQty = Convert.ToDecimal(dr["UnitQty"]);
                                objQuotDet.Qty = Convert.ToDecimal(dr["Qty"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.Rate = Convert.ToDecimal(dr["Rate"]);
                                objQuotDet.DiscountPer = Convert.ToDecimal(dr["DiscountPer"]);
                                objQuotDet.DiscountAmt = Convert.ToDecimal(dr["DiscountAmt"]);
                                objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                                objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);

                                if (Session["mySpecs"] != null)
                                {
                                    Boolean itemAdded = false;
                                    DataTable mySpecs = new DataTable();
                                    mySpecs = (DataTable)Session["mySpecs"];
                                    foreach (DataRow row in mySpecs.Rows)
                                    {
                                        if (row["pkID"].ToString() == dr["ProductID"].ToString())
                                        {
                                            objQuotDet.ProductSpecification = row["ProductSpecification"].ToString();
                                            itemAdded = true;
                                        }
                                    }

                                    if (!itemAdded)
                                    {
                                        objQuotDet.ProductSpecification = dr["ProductSpecification"].ToString();
                                    }
                                }
                                else
                                {
                                    objQuotDet.ProductSpecification = dr["ProductSpecification"].ToString();
                                }

                                objQuotDet.SGSTPer = Convert.ToDecimal(dr["SGSTPer"]);
                                objQuotDet.SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]);
                                objQuotDet.CGSTPer = Convert.ToDecimal(dr["CGSTPer"]);
                                objQuotDet.CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]);
                                objQuotDet.IGSTPer = Convert.ToDecimal(dr["IGSTPer"]);
                                objQuotDet.IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]);
                                objQuotDet.AddTaxPer = Convert.ToDecimal(dr["AddTaxPer"]);
                                objQuotDet.AddTaxAmt = Convert.ToDecimal(dr["AddTaxAmt"]);
                                objQuotDet.NetAmt = Convert.ToDecimal(dr["NetAmount"]);
                                objQuotDet.HeaderDiscAmt = Convert.ToDecimal(dr["HeaderDiscAmt"]);
                                objQuotDet.ForOrderNo = dr["ForOrderNo"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.SalesChallanMgmt.AddUpdateSalesChallanDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);

                            }
                            // --------------------------------------------------------------
                            //if (ReturnCode > 0)
                            //{
                            //    Session.Remove("dtDetail");
                            //}
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("mySpecs");
                            }
                        }
                        // --------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            if (!String.IsNullOrEmpty(txtInvoiceNo.Text))
                            {
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnChallanNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
                            else
                            {
                                txtInvoiceNo.Text = ReturnChallanNo;
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnChallanNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }

                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                        }
                    }
                }
                else
                {
                    strErr += "<li>Atleast One Item is required to save Sales Bill Information !</li>";
                }
            }
            // ---------------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnInvBasedOn.Value = "";
            txtInvoiceDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtLRDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtSupplierRefDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txteWay.Text = "";
            txtModePay.Text = "";
            txtInvoiceNo.Text = ""; // BAL.CommonMgmt.GetSalesOrderNo(txtOrderDate.Text);
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtSupplierRef.Text = "";
            txtOtherRef.Text = "";
            txtDisDocNo.Text = "";
            txtDelMote.Text = "";
            drpTerminationOfDelivery.SelectedValue = "0";

            drpReferenceNo.ClearSelection();

            txtHeadDiscount.Text = "";
            drpOthChrg1.SelectedValue = "";
            txtOthChrgAmt1.Text = "0";
            hdnOthChrgGST1.Value = "0";
            hdnOthChrgBasic1.Value = "0";

            drpOthChrg2.SelectedValue = "";
            txtOthChrgAmt2.Text = "0";
            hdnOthChrgGST2.Value = "0";
            hdnOthChrgBasic2.Value = "0";

            drpOthChrg3.SelectedValue = "";
            txtOthChrgAmt3.Text = "0";
            hdnOthChrgGST3.Value = "0";
            hdnOthChrgBasic3.Value = "0";

            drpOthChrg4.SelectedValue = "";
            txtOthChrgAmt4.Text = "0";
            hdnOthChrgGST4.Value = "0";
            hdnOthChrgBasic4.Value = "0";

            drpOthChrg5.SelectedValue = "";
            txtOthChrgAmt5.Text = "0";
            hdnOthChrgGST5.Value = "0";
            hdnOthChrgBasic5.Value = "0";

            txtTotBasicAmt.Text = "";
            txtTotOthChrgBeforeGST.Text = "";
            hdnTotCGSTAmt.Value = "";
            hdnTotSGSTAmt.Value = "";
            hdnTotIGSTAmt.Value = "";
            hdnTotItemGST.Value = "";
            txtTotGST.Text = "";
            txtTotOthChrgAfterGST.Text = "";
            txtRoff.Text = "";
            txtTotNetAmt.Text = "";

            ViewState["totAmount"] = 0;
            ViewState["totTaxAmount"] = 0;
            ViewState["totNetAmount"] = 0;
            ViewState["totDiscAmt"] = 0;
            ViewState["totAddTaxAmt"] = 0;
            ViewState["totSGST"] = 0;
            ViewState["totCGST"] = 0;
            ViewState["totIGST"] = 0;

            totAmount = 0;
            totTaxAmount = 0;
            totNetAmount = 0;
            totDiscAmt = 0;
            totAddTaxAmt = 0;
            totCGST = 0;
            totSGST = 0;
            totIGST = 0;

            BindSalesBillDetailList("");
        }


        [WebMethod(EnableSession = true)]
        public static void GenerateSalesChallan(Int64 pkID)
        {
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
             
            // ---------------------------------------------
            if (tmpSerialKey == "8YWQ-DDRO-V98V-LDN2")     // FIELDMASTER Innovation Limited
                GenerateSalesBillChallan_FIELDMASTER(pkID);
            else if (tmpSerialKey == "TJ7S-06Q2-8R2U-KJWG")     // Gautam
                GenerateSalesBillChallan_Gautam(pkID);
            else if (tmpSerialKey == "D33J-H872-3545-71A1")     // SuperTech
                GenerateSalesBillChallan_Supertech(pkID);
            else if (tmpSerialKey == "D33J-H872-3545-71A1")     // SuperTech
                GenerateSalesBillChallan_mudra(pkID);
            else if (tmpSerialKey == "6GZP-BW7W-78DF-HG88")     // Dishachi
                GenerateSalesBillChallan_mudra(pkID);
            else
                GenerateSalesBillChallan_FIELDMASTER(pkID);
        }
       
        public static void GenerateSalesBillChallan_Dishachi(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "no";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(5);
            PdfPTable tblDetail = new PdfPTable(5);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30, ProdDetail_Lines = 0;
            pdf.GetPrinterMargins("SalesChallan", flagPrintHeader, out TopMargin, out BottomMargin, out ProdDetail_Lines);


            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesChallan> lstQuot = new List<Entity.SalesChallan>();
            lstQuot = BAL.SalesChallanMgmt.GetSalesChallanList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesChallanMgmt.GetSalesChallanDetail(lstQuot[0].ChallanNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 20, 20, 30, 15, 15 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("DC NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("DELIVER TO ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].DeliverTo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("TRANSPORT ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].TransporterName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell("GST NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                // Adding to Parent Table
                // -------------------------------------------------------------------------------------


                //--------------------------Header--------------------------------
                PdfPTable tblBanner = new PdfPTable(1);
                int[] column_tblBanner = { 100 };
                tblBanner.SetWidths(column_tblBanner);
                int fileCount = 0;
                //string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\QuotationHeader.png";
                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        //int[] column_tblSign = { 30 };
                        //tblSign.SetWidths(column_tblSign);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(520, 90);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblBanner.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                tblMember.AddCell(pdf.setCell(tblBanner, pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 5, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell("Delivery Note", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 5, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(pdf.Quotation_CustomerInfo(lstCust), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                int[] colstruc111 = { 6, 60, 10, 10, 14 };
                tblDetail.SetWidths(colstruc111);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                decimal netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Net Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                string currNetAmt = ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencySymbol.Trim() : "") + " " + netAmount.ToString("0.00");
                tblAmount1.AddCell(pdf.setCell(currNetAmt, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                if (sumDis > 0)
                {
                    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" }, { "Description", "ProductName", "2", "1", "4", "12" }, { "Quantity", "Quantity", "2", "1", "4", "12" },
                                        { "UnitRate", "UnitRate", "2", "1", "4", "12" }, { "Amount", "Amount", "2", "1", "4", "12" }};
                    int[] column_tblDetailNested99 = { 6, 60, 10, 10, 14 };
                    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("saleschallan", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri7), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    // ------------------------------------------
                    tblDetail.AddCell(pdf.setCell(pdf.BankDetails(lstBank, 0, 1), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                }
                else
                {
                    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" }, { "Description", "ProductName", "2", "1", "4", "12" }, { "Quantity", "Quantity", "2", "1", "4", "12" },
                                        { "UnitRate", "UnitRate", "2", "1", "4", "12" }, { "Amount", "Amount", "2", "1", "4", "12" }};
                    int[] column_tblDetailNested99 = { 6, 60, 10, 10, 14 };
                    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("saleschallan", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri8), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    // ------------------------------------------
                    tblDetail.AddCell(pdf.setCell(pdf.BankDetails(lstBank, 0, 1), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    //List<Entity.PrintDocument> targetList = new List<Entity.PrintDocument>(lstQuot.Cast<Entity.PrintDocument>());
                    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                tblDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Bank Detail & Signature
                // -------------------------------------------------------------------------------------
                PdfPTable tblTNCBank = new PdfPTable(1);
                int[] column_tblTNCBank = { 100 };
                tblTNCBank.SetWidths(column_tblTNCBank);
                tblTNCBank.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].TermsCondition, 0, true), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                //tblTNCBank.AddCell(pdf.setCell(pdf.BankDetails(lstBank, 0, 1), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                tblDetail.AddCell(pdf.setCell(tblTNCBank, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 4), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                #region Section >>>> Terms & Condition
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].ChallanNo.Replace("/", "-").ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }

        public static void GenerateSalesBillChallan_FIELDMASTER(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(5);
            PdfPTable tblDetail = new PdfPTable(5);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30, ProdDetail_Lines = 0;
            pdf.GetPrinterMargins("SalesChallan", flagPrintHeader, out TopMargin, out BottomMargin, out ProdDetail_Lines);


            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesChallan> lstQuot = new List<Entity.SalesChallan>();
            lstQuot = BAL.SalesChallanMgmt.GetSalesChallanList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesChallanMgmt.GetSalesChallanDetail(lstQuot[0].ChallanNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 20, 20, 30, 15, 15 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("DC NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("DELIVER TO ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("TRANSPORT ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("GST NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                // Adding to Parent Table
                // -------------------------------------------------------------------------------------
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 13));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 6));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 2));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 2));
                tblMember.AddCell(pdf.setCell("  Delivery Note  ", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 0));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 10));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 14));

                tblMember.AddCell(pdf.setCell(pdf.Quotation_CustomerInfo(lstCust), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                int[] colstruc111 = { 6, 60, 10, 10, 14 };
                tblDetail.SetWidths(colstruc111);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                decimal netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Net Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                string currNetAmt = ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencySymbol.Trim() : "") + " " + netAmount.ToString("0.00");
                tblAmount1.AddCell(pdf.setCell(currNetAmt, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                if (sumDis > 0)
                {
                    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" }, { "Description", "ProductName", "2", "1", "4", "12" }, { "Quantity", "Quantity", "2", "1", "4", "12" },
                                        { "UnitRate", "UnitRate", "2", "1", "4", "12" }, { "Amount", "Amount", "2", "1", "4", "12" }};
                    int[] column_tblDetailNested99 = { 6, 60, 10, 10, 14 };
                    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("salesbill", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri7), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    // ------------------------------------------
                    tblDetail.AddCell(pdf.setCell("Note : ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                }
                else
                {
                    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" }, { "Description", "ProductName", "2", "1", "4", "12" }, { "Quantity", "Quantity", "2", "1", "4", "12" },
                                        { "UnitRate", "UnitRate", "2", "1", "4", "12" }, { "Amount", "Amount", "2", "1", "4", "12" }};
                    int[] column_tblDetailNested99 = { 6, 60, 10, 10, 14 };
                    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("saleschallan", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri8), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    // ------------------------------------------
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    //List<Entity.PrintDocument> targetList = new List<Entity.PrintDocument>(lstQuot.Cast<Entity.PrintDocument>());
                    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                tblDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Bank Detail & Signature
                // -------------------------------------------------------------------------------------
                PdfPTable tblTNCBank = new PdfPTable(1);
                int[] column_tblTNCBank = { 100 };
                tblTNCBank.SetWidths(column_tblTNCBank);
                tblTNCBank.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].TermsCondition, 0, true), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblTNCBank.AddCell(pdf.setCell(pdf.BankDetails(lstBank, 0, 1), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                tblDetail.AddCell(pdf.setCell(tblTNCBank, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 2), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                #region Section >>>> Terms & Condition
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].ChallanNo.Replace("/", "-").ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }


        public static void GenerateSalesBillChallan_Gautam(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(5);
            PdfPTable tblDetail = new PdfPTable(6);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30, ProdDetail_Lines = 0;
            pdf.GetPrinterMargins("SalesChallan", flagPrintHeader, out TopMargin, out BottomMargin, out ProdDetail_Lines);


            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesChallan> lstQuot = new List<Entity.SalesChallan>();
            lstQuot = BAL.SalesChallanMgmt.GetSalesChallanList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesChallanMgmt.GetSalesChallanDetail(lstQuot[0].ChallanNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 20, 20, 30, 15, 15 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("DC NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("DELIVER TO ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("TRANSPORT ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("GST NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                // Adding to Parent Table
                // -------------------------------------------------------------------------------------
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 13));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 6));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 2));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 2));
                tblMember.AddCell(pdf.setCell("  Delivery Note  ", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 10));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 14));

                tblMember.AddCell(pdf.setCell(pdf.Quotation_CustomerInfo(lstCust), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                int[] colstruc111 = { 6, 50, 10, 10, 10, 14 };
                tblDetail.SetWidths(colstruc111);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                decimal netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Net Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                string currNetAmt = ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencySymbol.Trim() : "") + " " + netAmount.ToString("0.00");
                tblAmount1.AddCell(pdf.setCell(currNetAmt, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                if (sumDis > 0)
                {
                    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" },
                        { "Description", "ProductName", "2", "1", "4", "12" },
                        { "HSN Code", "HSNCode", "2", "1", "4", "12" },
                        { "Quantity", "Quantity", "2", "1", "4", "12" },
                        { "UnitRate", "UnitRate", "2", "1", "4", "12" },
                        { "Amount", "Amount", "2", "1", "4", "12" }};
                    int[] column_tblDetailNested99 = { 6, 50, 10, 10, 10, 14 };
                    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("salesbill", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri7), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    // ------------------------------------------
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                }
                else
                {
                    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" },
                        { "Description", "ProductName", "2", "1", "4", "12" },
                        { "HSN Code", "HSNCode", "2", "1", "4", "12" },
                        { "Quantity", "Quantity", "2", "1", "4", "12" },
                        { "UnitRate", "UnitRate", "2", "1", "4", "12" },
                        { "Amount", "Amount", "2", "1", "4", "12" }};
                    int[] column_tblDetailNested99 = { 6, 50, 10, 10, 10, 14 };
                    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("saleschallan", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri8), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    // ------------------------------------------
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    //List<Entity.PrintDocument> targetList = new List<Entity.PrintDocument>(lstQuot.Cast<Entity.PrintDocument>());
                    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                tblDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf4, 6, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Bank Detail & Signature
                // -------------------------------------------------------------------------------------

                #region Section >>>> Terms & Condition
                PdfPTable tblTNCBank = new PdfPTable(1);
                int[] column_tblTNCBank = { 100 };
                tblTNCBank.SetWidths(column_tblTNCBank);
                tblTNCBank.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].TermsCondition, 0, true), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblTNCBank.AddCell(pdf.setCell(pdf.BankDetails(lstBank, 0, 3), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                tblDetail.AddCell(pdf.setCell(tblTNCBank, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 2), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].ChallanNo.Replace("/", "-").ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }

        public static void GenerateSalesBillChallan_mudra(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(3);
            PdfPTable tblHead = new PdfPTable(3);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblTNC = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30, ProdDetail_Lines = 0;
            pdf.GetPrinterMargins("SalesChallan", flagPrintHeader, out TopMargin, out BottomMargin, out ProdDetail_Lines);


            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesChallan> lstQuot = new List<Entity.SalesChallan>();
            lstQuot = BAL.SalesChallanMgmt.GetSalesChallanList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // ------------------------------------------------------------------------------
            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
            //-------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesChallanMgmt.GetSalesChallanDetail(lstQuot[0].ChallanNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information


                int[] column_tableHeader = { 40, 40, 30 };
                tblHead.SetWidths(column_tableHeader);
                tblHead.SpacingBefore = 0f;
                tblHead.LockedWidth = true;
                //tableHeader.AddCell(pdf.setCell("Original For Recipient", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                //tableHeader.AddCell(pdf.setCell("Debit Memo", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                int[] column_tblMember = { 35, 35, 30 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                //------------------Buyes's Details 1 Details----------------------
                PdfPTable tblInvoiceTo = new PdfPTable(4);
                int[] column_tblConsigneeD = { 15, 15, 40, 30 };
                tblInvoiceTo.SetWidths(column_tblConsigneeD);

                string temp = lstOrg[0].Address + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : "") + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? " Cell - " + lstOrg[0].Landline1 : "") + (!String.IsNullOrEmpty(lstOrg[0].Landline2) ? ", " + lstOrg[0].Landline2 : "");

                tblInvoiceTo.AddCell(pdf.setCell("DELIVERY ADDRESS", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell(lstOrg[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell(lstCust[0].StateName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                //------------------Buyers details 2 Details----------------------
                PdfPTable tblDispatchedTo = new PdfPTable(4);
                int[] column_tblDispatchedTo = { 15, 15, 40, 30 };
                tblDispatchedTo.SetWidths(column_tblDispatchedTo);

                string address1 = (lstCust.Count > 0 ? lstCust[0].Address1.ToUpper() : " ") + ", " + (lstCust.Count > 0 ? lstCust[0].Area1.ToUpper() : " ") + ", " + (lstCust.Count > 0 ? lstCust[0].CityName1.ToUpper() : " ") + " - " + (lstCust.Count > 0 ? lstCust[0].Pincode1.ToUpper() : " ");
                tblDispatchedTo.AddCell(pdf.setCell("KIND ATTN", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblDispatchedTo.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                tblDispatchedTo.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDispatchedTo.AddCell(pdf.setCell("PHONE NO", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP,2));
                tblDispatchedTo.AddCell(pdf.setCell(" " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                tblDispatchedTo.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDispatchedTo.AddCell(pdf.setCell("E MAIL ADDRESS", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblDispatchedTo.AddCell(pdf.setCell(" " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //------------------Invoice Details----------------------
                PdfPTable tblInvoiceD = new PdfPTable(4);
                int[] column_tblInvoiceD = { 40, 25, 15, 20 };
                tblInvoiceD.SetWidths(column_tblInvoiceD);

                // tblInvoiceD.AddCell(pdf.setCell("Invoice Details", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell("DC NO", pdf.DarkGrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].DispatchDocNo, pdf.DarkGrayBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell("DC DATE", pdf.DarkGrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].SupplierRefDate.ToString("dd-MMM-yyyy"), pdf.DarkGrayBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell("CUSTOMER PO NO & Date", pdf.DarkGrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].RefNo + lstQuot[0].SupplierRefDate, pdf.DarkGrayBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblInvoiceD.AddCell(pdf.setCell("Dated", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblInvoiceD.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell("Delivery Type", pdf.DarkGrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].DeliveryNote, pdf.DarkGrayBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell("DELIVERY BY", pdf.DarkGrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].DeliverTo, pdf.DarkGrayBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
              
                tblMember.AddCell(pdf.setCell("Delivery Challan", pdf.DarkGrayBaseColor, pdf.fnCalibriBold12, pdf.paddingOf2, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblInvoiceTo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblDispatchedTo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblInvoiceD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                //  tblMember.AddCell(pdf.setCell(tblTransportD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                sumDis = Convert.ToDecimal(sumDis) + Convert.ToDecimal(dtItem.AsEnumerable().Sum(x => x.Field<decimal>("HeaderDiscAmt")));

                int[] column_tblNested = { 10, 60, 15, 15 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("SR.No.", pdf.LightBlueBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.LightBlueBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("HSN CODE", pdf.LightBlueBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("QTY", pdf.LightBlueBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));



                decimal totAmount = 0, taxAmount = 0, netAmount = 0, totalQty = 0;

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    //tmpAmount = (Convert.ToDecimal(dtItem.Rows[i]["Quantity"]) * Convert.ToDecimal(dtItem.Rows[i]["NetRate"]));
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    //netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    totalQty += Convert.ToDecimal(dtItem.Rows[i]["UnitQty"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["HSNCode"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));


                }

                //for (int i = 1; i < (13 - dtItem.Rows.Count); i++)
                //{
                if (ProdDetail_Lines > dtItem.Rows.Count)
                {
                    for (int i = 1; i <= (ProdDetail_Lines - dtItem.Rows.Count); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    }
                }
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                tblDetail.AddCell(pdf.setCell("TOTAL", pdf.WhiteBaseColor, pdf.fnCalibriBold8,  pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                tblDetail.AddCell(pdf.setCell(Convert.ToInt32(totalQty).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8,  pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));

                #endregion
                // -------------------------------------------------------------------------------------
                //  Defining : Bank Detail & Signature
                // -------------------------------------------------------------------------------------


                int[] column_tblTnC = { 20, 80 };
                tblTNC.SetWidths(column_tblTnC);
                tblTNC.SpacingBefore = 15f;
                tblTNC.LockedWidth = true;

                tblTNC.AddCell(pdf.setCell("Terms & Conditions :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));

                lstQuot[0].TermsCondition.Split('\n').ToList().ForEach(line =>
                {
                    if (!String.IsNullOrEmpty(line.Trim()))
                    {
                        string[] strArr = null;
                        strArr = line.Split(':');

                        if (strArr.Length > 1)
                        {
                            tblTNC.AddCell(pdf.setCell(" " + strArr[0].Trim(), pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                            tblTNC.AddCell(pdf.setCell(": " + strArr[1].Trim(), pdf.DarkGrayBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        }
                        else
                        {
                            tblTNC.AddCell(pdf.setCell(strArr[0].Trim(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        }
                    }
                });

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        //int[] column_tblSign = { 30 };
                        //tblSign.SetWidths(column_tblSign);

                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(80, 50);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                #region Section >>>> Terms & Condition
                PdfPTable tblTNCBank = new PdfPTable(1);
                int[] column_tblTNCBank = { 100 };
                tblTNCBank.SetWidths(column_tblTNCBank);
                    tblTNCBank.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].TermsCondition, 0, true), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
           
                tblDetail.AddCell(pdf.setCell(tblTNCBank, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 2), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].ChallanNo.Replace("/", "-").ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }


        public static void GenerateSalesBillChallan_Supertech(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(5);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30, ProdDetail_Lines = 0;
            pdf.GetPrinterMargins("SalesChallan", flagPrintHeader, out TopMargin, out BottomMargin, out ProdDetail_Lines);


            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesChallan> lstQuot = new List<Entity.SalesChallan>();
            lstQuot = BAL.SalesChallanMgmt.GetSalesChallanList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesChallanMgmt.GetSalesChallanDetail(lstQuot[0].ChallanNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 20, 20, 30, 15, 15 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("DC NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].ChallanDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("DELIVER TO ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("TRANSPORT ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("GST NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                // Adding to Parent Table
                // -------------------------------------------------------------------------------------
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 13));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 6));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 2));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 2));
                tblMember.AddCell(pdf.setCell("  Delivery Note  ", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 10));
                tblMember.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 14));

                tblMember.AddCell(pdf.setCell(pdf.Quotation_CustomerInfo(lstCust), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                #region Product Specification
                Boolean specInstance = false;
                PdfPTable tblSpec = new PdfPTable(2);
                int[] column_tblNested22 = { 45, 55 };
                tblSpec.SetWidths(column_tblNested22);
                //tblSpec.SpacingBefore = 8f;
                //tblSpec.LockedWidth = true;


                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].QuotationNo.ToString(), Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), "");
                    lstSpec = lstSpec.Where(e => (e.MaterialSpec.Trim() != "")).ToList();

                    if (lstSpec.Count > 0)
                    {
                        specInstance = true;
                        //tblSpec.AddCell(pdf.setCell("Parameter", pdf.WhiteBaseColor, pdf.fnTimesBold10, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE));
                        //tblSpec.AddCell(pdf.setCell("Value", pdf.WhiteBaseColor, pdf.fnTimesBold10, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE));
                        ////tblSpec.AddCell(setCell("Technical Specification", pdf.LightBlueBaseColor, pdf.objContentFontTitleBlack, pdf.paddingOf3, 3, Element.ALIGN_CENTER));
                        // ----------------------------------------------------------------------------
                        String tmpGroup = "";
                        int rowCount = 1;
                        for (int j = 0; j < lstSpec.Count; j++)
                        {
                            //if (j == 0)
                            //{
                            //    tblSpec.AddCell(setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack, pdf.paddingOf3, 4, Element.ALIGN_CENTER));
                            //    tblSpec.AddCell(setCell("Product Name : " + dtItem.Rows[i]["ProductName"].ToString(), pdf.LightBlueBaseColor, pdf.objContentFontTitleBlack, pdf.paddingOf3, 4, Element.ALIGN_CENTER));
                            //}
                            if (String.IsNullOrEmpty(tmpGroup) || tmpGroup != lstSpec[j].GroupHead.ToString())
                            {
                                tblSpec.AddCell(pdf.setCell(lstSpec[j].GroupHead.ToString(), pdf.WhiteBaseColor, pdf.fnTimesBold10, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                                rowCount = 1;
                            }
                            // ----------------------------------
                            if (!String.IsNullOrEmpty(lstSpec[j].MaterialSpec.ToString().Trim()))
                            {
                                tblSpec.AddCell(pdf.setCell(lstSpec[j].MaterialHead.ToString(), pdf.WhiteBaseColor, pdf.fnTimes8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                                tblSpec.AddCell(pdf.setCell(lstSpec[j].MaterialSpec.ToString(), pdf.WhiteBaseColor, pdf.fnTimes8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                                rowCount = rowCount + 1;
                            }
                            tmpGroup = lstSpec[j].GroupHead.ToString();
                        }
                    }
                }
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                //var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                //int[] colstruc111 = { 6, 50, 10, 10, 10, 14 };
                //tblDetail.SetWidths(colstruc111);
                //tblDetail.SpacingBefore = 0f;
                //tblDetail.LockedWidth = true;
                //tblDetail.SplitLate = false;
                //tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //decimal netAmount = lstQuot[0].NetAmt;
                //PdfPTable tblAmount1 = new PdfPTable(2);
                //int[] column_tblAmount1 = { 60, 40 };
                //tblAmount1.SetWidths(column_tblAmount1);
                //string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                //tblAmount1.AddCell(pdf.setCell("Net Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //string currNetAmt = ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencySymbol.Trim() : "") + " " + netAmount.ToString("0.00");
                //tblAmount1.AddCell(pdf.setCell(currNetAmt, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //if (sumDis > 0)
                //{
                //    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" },
                //        { "Description", "ProductName", "2", "1", "4", "12" },
                //        { "HSN Code", "HSNCode", "2", "1", "4", "12" },
                //        { "Quantity", "Quantity", "2", "1", "4", "12" },
                //        { "UnitRate", "UnitRate", "2", "1", "4", "12" },
                //        { "Amount", "Amount", "2", "1", "4", "12" }};
                //    int[] column_tblDetailNested99 = { 6, 50, 10, 10, 10, 14 };
                //    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("deliverychallan", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri7), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                //    // ------------------------------------------
                //    tblDetail.AddCell(pdf.setCell("Note : ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                //    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                //    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //}
                //else
                //{
                //    string[,] myColStruc = {{ "Sr.No", "counter", "2", "1", "4", "12" },
                //        { "Description", "ProductName", "2", "1", "4", "12" },
                //        { "HSN Code", "HSNCode", "2", "1", "4", "12" },
                //        { "Quantity", "Quantity", "2", "1", "4", "12" },
                //        { "UnitRate", "UnitRate", "2", "1", "4", "12" },
                //        { "Amount", "Amount", "2", "1", "4", "12" }};
                //    int[] column_tblDetailNested99 = { 6, 50, 10, 10, 10, 14 };
                //    tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("saleschallan", dtItem, myColStruc, column_tblDetailNested99, ProdDetail_Lines, 12, pdf.fnCalibri8,1,"",2), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                //    // ------------------------------------------
                //    tblDetail.AddCell(pdf.setCell("Note : ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    //List<Entity.PrintDocument> targetList = new List<Entity.PrintDocument>(lstQuot.Cast<Entity.PrintDocument>());
                //    tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                //    string currInWords = "Amount In Words: " + ((!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim())) ? lstQuot[0].CurrencyName.Trim().ToUpper() : "RUPEES") + " " + NetAmtInWords;
                //    tblDetail.AddCell(pdf.setCell(currInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //tblDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf4, 6, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));

                #endregion

                #region Section >>>> Quotation Product Detail

                //var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                //if (sumDis > 0)
                //{
                int[] column_tblNested = { 6, 64, 15, 15 };
                tblDetail.SetWidths(column_tblNested);
                //}
                //else
                //{
                //    int[] column_tblNested = { 6, 64, 15, 15 };
                //    tblDetail.SetWidths(column_tblNested);
                //}
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Unit Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    //tmpAmount = (Convert.ToDecimal(dtItem.Rows[i]["Quantity"]) * Convert.ToDecimal(dtItem.Rows[i]["NetRate"]));
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    //netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //tblDetail.AddCell(pdf.setCell(tmpProdAlias, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 4));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString() + tmpProdAlias, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //if (sumDis > 0)
                    //{
                    //    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}
                    //else
                    //{
                    //    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}

                    //----Product Alias Printing-----------
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(tmpProdAlias, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //if (sumDis > 0)
                    //{
                    //     tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}
                    //else
                    //{
                    //    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}

                    //----------Product Specificatio Table Printing--------
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(tblSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //if (sumDis > 0)
                    //{
                    //    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}
                    //else
                    //{
                    //    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}

                }
                for (int i = 1; i < (ProdDetail_Lines - dtItem.Rows.Count); i++)
                {
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //if (sumDis > 0)
                    //{
                    //    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}
                    //else
                    //{
                    //    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //}

                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-


                #region ------ Other charges
                // ---------------------------------------------------------------------------------------------------------
                //PdfPTable tblAmount = new PdfPTable(2);
                //int[] column_tblAmount = { 60, 40 };
                //tblAmount.SetWidths(column_tblAmount);
                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                //totAmount = lstQuot[0].BasicAmt;
                //totRNDOff = lstQuot[0].ROffAmt;
                //totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                //tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    befAmt += lstQuot[0].ChargeAmt1;
                //    befGST += lstQuot[0].ChargeGSTAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    befAmt += lstQuot[0].ChargeAmt2;
                //    befGST += lstQuot[0].ChargeGSTAmt2;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    befAmt += lstQuot[0].ChargeAmt3;
                //    befGST += lstQuot[0].ChargeGSTAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    befAmt += lstQuot[0].ChargeAmt4;
                //    befGST += lstQuot[0].ChargeGSTAmt4;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    befAmt += lstQuot[0].ChargeAmt5;
                //    befGST += lstQuot[0].ChargeGSTAmt5;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                ////if (befAmt > 0)
                ////{
                ////    tblAmount.AddCell(setCell("Total     :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ////    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                ////        tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ////    else
                ////        tblAmount.AddCell(setCell((totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ////}

                //if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                //{
                //    if (lstQuot[0].IGSTAmt > 0)
                //    {
                //        List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
                //        lstTaxList = BAL.CommonMgmt.GetTaxSummary("salesorder", "igst", lstQuot[0].OrderNo);
                //        for (int i = 0; i < lstTaxList.Count; i++)
                //        {
                //            tblAmount.AddCell(pdf.setCell("IGST @ " + lstTaxList[i].IGSTPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstTaxList[i].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            else
                //                tblAmount.AddCell(pdf.setCell(lstTaxList[i].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //        }
                //    }
                //    else
                //    {
                //        List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
                //        lstTaxList = BAL.CommonMgmt.GetTaxSummary("salesorder", "cgst", lstQuot[0].OrderNo);
                //        for (int i = 0; i < lstTaxList.Count; i++)
                //        {
                //            if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
                //            {
                //                tblAmount.AddCell(pdf.setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //                if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //                    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstTaxList[i].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //                else
                //                    tblAmount.AddCell(pdf.setCell(lstTaxList[i].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //                tblAmount.AddCell(pdf.setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //                if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //                    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstTaxList[i].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //                else
                //                    tblAmount.AddCell(pdf.setCell(lstTaxList[i].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            }
                //        }
                //    }
                //}

                ////tblAmount.AddCell(setCell("Total     :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                ////tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST)).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));

                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    aftAmt += lstQuot[0].ChargeAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    aftAmt += lstQuot[0].ChargeAmt2;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    aftAmt += lstQuot[0].ChargeAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    aftAmt += lstQuot[0].ChargeAmt4;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    aftAmt += lstQuot[0].ChargeAmt5;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ROffAmt > 0)
                //{
                //    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}

                //tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ////tblAmount.AddCell(setCell("Grand Total  :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ////if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                ////    tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ////else
                ////    tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                ////if (sumDis > 0)
                ////{
                ////    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                ////    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                ////}
                ////else
                ////{
                #endregion
                //tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}

                // ****************************************************************
                //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
                //netAmount = lstQuot[0].NetAmt;
                //PdfPTable tblAmount1 = new PdfPTable(2);
                //int[] column_tblAmount1 = { 60, 40 };
                //tblAmount1.SetWidths(column_tblAmount1);
                //string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                //tblAmount1.AddCell(pdf.setCell("Total Amount ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //var ph1 = new Phrase();
                //ph1.Add(new Chunk("GSTIN No : ", pdf.fnCalibriBold8));
                //ph1.Add(new Chunk("24BIQPK4338E1Z8", pdf.fnCalibri8));
                //tblDetail.AddCell(pdf.setCell(ph1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                #endregion


                // -------------------------------------------------------------------------------------
                //  Defining : Bank Detail & Signature
                // -------------------------------------------------------------------------------------

                #region Section >>>> Terms & Condition

                PdfPTable tblTNCBank = new PdfPTable(1);
                int[] column_tblTNCBank = { 100 };
                tblTNCBank.SetWidths(column_tblTNCBank);
                tblTNCBank.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].TermsCondition, 0, true), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblTNCBank.AddCell(pdf.setCell(pdf.BankDetails(lstBank, 0, 1), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                int[] column_tblFooter = { 70, 30 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.AddCell(pdf.setCell(tblTNCBank, pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 2), pdf.WhiteBaseColor, pdf.objContentFontTitleBlack8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].ChallanNo.Replace("/", "-").ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Detail Table
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblFooter);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }

       
    }
}