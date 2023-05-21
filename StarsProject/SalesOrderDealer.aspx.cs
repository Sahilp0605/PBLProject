using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SalesOrderDealer : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount, totDiscAmt, totAddTaxAmt, totSGST, totCGST, totIGST;
        //private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            // ----------------------------------------------------
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

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();

                hdnQuotationSpecification.Value = BAL.CommonMgmt.GetConstant("QuotationSpecification", 0, 1);
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
                                GetInquiryProducts();
                            }
                        }
                    }
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            hdnMode.Value = Request.QueryString["mode"].ToString();
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }

                //-------------------------------------------------------
                // Loading Product Specification 
                //-------------------------------------------------------
                if (hdnQuotationSpecification.Value.ToLower() == "yes")
                {
                    if (hdnMode.Value.ToLower() == "edit")
                    {
                        DataTable dtSpecs = new DataTable();
                        List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
                        if (!String.IsNullOrEmpty(txtOrderNo.Text) || txtOrderNo.Text != "")
                        {
                            lst = BAL.ProductMgmt.GetSalesOrderDealerProductSpecList(txtOrderNo.Text, 0, Session["LoginUserID"].ToString());
                            if (lst.Count > 0)
                            {
                                dtSpecs = PageBase.ConvertListToDataTable(lst);
                                Session["dtSpecs"] = dtSpecs;
                            }
                        }
                    }
                }
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];

                if (requestTarget.ToLower() == "drpreferenceno")
                {
                    if (rdblOption.SelectedItem.Value == "Inquiry")
                    {
                        GetInquiryProducts();
                    }

                    if (rdblOption.SelectedItem.Value == "Quotation")
                    {
                        if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                        {
                            DataTable dtDetail = new DataTable();
                            dtDetail = (DataTable)Session["dtDetail"];

                            dtDetail.Clear();
                            dtDetail = BAL.QuotationDetailMgmt.GetQuotationProductForSalesOrder(drpReferenceNo.SelectedValue, txtOrderNo.Text);
                            rptOrderDetail.DataSource = dtDetail;
                            rptOrderDetail.DataBind();

                            Session.Add("dtDetail", dtDetail);

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

                            txtHeadDiscount_TextChanged(null, null);
                        }
                    }
                }
                // ------------------------------------------------
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:retainTabPosition();", true);
            }
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            hdnCustomerID.Value = objAuth.EmployeeID.ToString();
            txtCustomerName.Text = objAuth.EmployeeName;
        }

        public void GetInquiryProducts()
        {
            if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
            {
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                dtDetail.Clear();
                dtDetail = BAL.InquiryInfoMgmt.GetInquiryProductForQuotation(drpReferenceNo.SelectedValue, txtOrderNo.Text);
                Session.Add("dtDetail", dtDetail);

                foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                foreach (DataRow row in dtDetail.Rows)
                {
                    Decimal a = (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                    Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);
                    Decimal q = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                    Decimal ur = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                    Decimal dp = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                    Decimal dpa = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                    Decimal nr = (!String.IsNullOrEmpty(row["NetRate"].ToString())) ? Convert.ToDecimal(row["NetRate"]) : 0;
                    Decimal tr = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                    Decimal at = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;

                    decimal TaxAmt = 0;
                    decimal CGSTPer = 0, CGSTAmt = 0;
                    decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
                    BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

                    row.SetField("HeaderDiscAmt", 0);
                    row.SetField("NetRate", NetRate);
                    row.SetField("Amount", BasicAmt);
                    row.SetField("CGSTPer", CGSTPer);
                    row.SetField("SGSTPer", SGSTPer);
                    row.SetField("IGSTPer", IGSTPer);
                    row.SetField("CGSTAmt", CGSTAmt);
                    row.SetField("SGSTAmt", SGSTAmt);
                    row.SetField("IGSTAmt", IGSTAmt);
                    row.SetField("TaxAmount", CGSTAmt + SGSTAmt + IGSTAmt);
                    row.SetField("AddTaxAmt", AddTaxAmt);
                    row.SetField("NetAmt", NetAmt);
                    row.SetField("NetAmount", NetAmt);
                }
                DataTable dtDetail1 = new DataTable();
                dtDetail1 = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
                rptOrderDetail.DataSource = dtDetail1;
                rptOrderDetail.DataBind();
                Session.Add("dtDetail", dtDetail);
            }
        }

        public void OnlyViewControls()
        {
            txtOrderNo.ReadOnly = true;
            txtOrderDate.ReadOnly = true;
            txtTermsCondition.ReadOnly = true;
            //drpCustomer.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            drpReferenceNo.Attributes.Add("disabled", "disabled");
            drpSalesPerson.Attributes.Add("disabled", "disabled");
            drpApprovalStatus.Attributes.Add("disabled", "disabled");
            drpTNC.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;

            drpOthChrg1.Attributes.Add("disabled", "disabled");
            txtOthChrgAmt1.ReadOnly = true;
            drpOthChrg2.Attributes.Add("disabled", "disabled");
            txtOthChrgAmt2.ReadOnly = true;
            drpOthChrg3.Attributes.Add("disabled", "disabled");
            txtOthChrgAmt3.ReadOnly = true;
            drpOthChrg4.Attributes.Add("disabled", "disabled");
            txtOthChrgAmt4.ReadOnly = true;
            drpOthChrg5.Attributes.Add("disabled", "disabled");
            txtOthChrgAmt5.ReadOnly = true;

            txtTotBasicAmt.ReadOnly = true;
            txtTotOthChrgBeforeGST.ReadOnly = true;
            txtTotGST.ReadOnly = true;
            txtTotOthChrgAfterGST.ReadOnly = true;
            txtRoff.ReadOnly = true;
            txtTotNetAmt.ReadOnly = true;

            pnlDetail.Enabled = false;
            rdblOption.Enabled = false;
        }

        public void BindDropDown()
        {
            //// ---------------- Customers List -------------------------------------
            //List<Entity.Customer> lstOrgDept2 = new List<Entity.Customer>();
            //lstOrgDept2 = BAL.CustomerMgmt.GetCustomerList();
            //drpCustomer.DataSource = lstOrgDept2;
            //drpCustomer.DataValueField = "CustomerID";
            //drpCustomer.DataTextField = "CustomerName";
            //drpCustomer.DataBind();
            //drpCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));

            // ---------------- Terms & Condition -------------------------------------
            List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            drpTNC.DataSource = lstList2;
            drpTNC.DataValueField = "pkID";
            drpTNC.DataTextField = "TNC_Header";
            drpTNC.DataBind();
            drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

            // ---------------- Employee (Sales Person) List----------------------------
            List<Entity.OrganizationEmployee> lstList3 = new List<Entity.OrganizationEmployee>();
            lstList3 = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpSalesPerson.DataSource = lstList3;
            drpSalesPerson.DataValueField = "pkID";
            drpSalesPerson.DataTextField = "EmployeeName";
            drpSalesPerson.DataBind();
            drpSalesPerson.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Sales Person --", ""));

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
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
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

        // ----------------------------------------------------------------------------------
        // Sales Order Dealer Item Detail List 
        // ----------------------------------------------------------------------------------
        public void BindSalesOrderDealerDetailList(string pOrderNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerDetail(pOrderNo);
            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
            // -----------------------------------------------------------
            BindPayScheduleList(0, pOrderNo, Session["LoginUserID"].ToString());
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
                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + ((HiddenField)e.Item.FindControl("hdnProductID")).Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "<li>'Duplicate Item Not Allowed..!!')", true);
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
                        string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                        string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                        string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                        string disper = ((TextBox)e.Item.FindControl("txtDiscountPercent")).Text;
                        string disamt = ((TextBox)e.Item.FindControl("txtDiscountAmt")).Text;
                        string netrate = ((TextBox)e.Item.FindControl("txtNetRate")).Text;
                        string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;
                        string taxrate = ((TextBox)e.Item.FindControl("txtTaxRate")).Text;
                        string taxamt = ((TextBox)e.Item.FindControl("txtTaxAmount")).Text;
                        //string addtaxper = ((TextBox)e.Item.FindControl("txtAddTaxPer")).Text;
                        //string addtaxamt = ((TextBox)e.Item.FindControl("txtAddTaxAmt")).Text;
                        string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;

                        string cgstper = ((HiddenField)e.Item.FindControl("hdnCGSTPer")).Value;
                        string sgstper = ((HiddenField)e.Item.FindControl("hdnSGSTPer")).Value;
                        string igstper = ((HiddenField)e.Item.FindControl("hdnIGSTPer")).Value;

                        string cgstamt = ((HiddenField)e.Item.FindControl("hdnCGSTAmt")).Value;
                        string sgstamt = ((HiddenField)e.Item.FindControl("hdnSGSTAmt")).Value;
                        string igstamt = ((HiddenField)e.Item.FindControl("hdnIGSTAmt")).Value;
                        string headdiscamt = ((TextBox)e.Item.FindControl("txtHeaderDiscAmt")).Text;
                        string boxweight = "", boxsqft = "", boxsqmt = "";

                        dr["OrderNo"] = txtOrderNo.Text;
                        dr["QuotationNo"] = "";
                        dr["InquiryNo"] = "";
                        dr["InvoiceNo"] = "";

                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["TaxType"] = (!String.IsNullOrEmpty(taxtype)) ? Convert.ToInt16(taxtype) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Qty"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                        dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["UnitPrice"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["Rate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["DiscountPercent"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                        dr["DiscountPer"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
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
                        dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                        dr["NetAmt"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                        dr["AddTaxPer"] = 0;
                        dr["AddTaxAmt"] = 0;
                        dr["HeaderDiscAmt"] = 0;
                        dr["BundleID"] = 0;
                        dr["Box_Weight"] = (!String.IsNullOrEmpty(boxweight)) ? Convert.ToInt64(boxweight) : 0;
                        dr["Box_SQFT"] = (!String.IsNullOrEmpty(boxsqft)) ? Convert.ToInt64(boxsqft) : 0;
                        dr["Box_SQMT"] = (!String.IsNullOrEmpty(boxsqmt)) ? Convert.ToInt64(boxsqmt) : 0;

                        dtDetail.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptOrderDetail.DataSource = dtDetail;
                        rptOrderDetail.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtDetail", dtDetail);
                    }
                }
                // -------------------------------------------------

                //txtHeadDiscount_TextChanged(null, null);

                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
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
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
        }

        protected void rptOrderDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2, v3, v4, v5, v6, v7, v8;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "DiscountAmt"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Amount"));
                v3 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaxAmount"));
                //v4 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "AddTaxAmt"));
                v5 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmount"));

                v6 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "CGSTAmt"));
                v7 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "SGSTAmt"));
                v8 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "IGSTAmt"));

                totDiscAmt += v1;
                totAmount += v2;
                totTaxAmount += v3;
                //totAddTaxAmt += v4;
                totNetAmount += v5;

                totCGST += v6;
                totSGST += v7;
                totIGST += v8;

                ViewState["totAmount"] = (!String.IsNullOrEmpty(totAmount.ToString())) ? Convert.ToDecimal(totAmount) : 0;
                ViewState["totTaxAmount"] = (!String.IsNullOrEmpty(totTaxAmount.ToString())) ? Convert.ToDecimal(totTaxAmount) : 0;
                ViewState["totNetAmount"] = (!String.IsNullOrEmpty(totNetAmount.ToString())) ? Convert.ToDecimal(totNetAmount) : 0;
                ViewState["totDiscAmt"] = (!String.IsNullOrEmpty(totDiscAmt.ToString())) ? Convert.ToDecimal(totDiscAmt) : 0;
                //ViewState["totAddTaxAmt"] = (!String.IsNullOrEmpty(totAddTaxAmt.ToString())) ? Convert.ToDecimal(totAddTaxAmt) : 0;
                ViewState["totSGST"] = (!String.IsNullOrEmpty(totSGST.ToString())) ? Convert.ToDecimal(totSGST) : 0;
                ViewState["totCGST"] = (!String.IsNullOrEmpty(totCGST.ToString())) ? Convert.ToDecimal(totCGST) : 0;
                ViewState["totIGST"] = (!String.IsNullOrEmpty(totIGST.ToString())) ? Convert.ToDecimal(totIGST) : 0;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotalDiscAmt = (Label)e.Item.FindControl("lblTotalDiscAmt");
                Label lblTotalAmt = (Label)e.Item.FindControl("lblTotalAmt");
                Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
                //Label lblAddTaxAmt = (Label)e.Item.FindControl("lblAddTaxAmt");
                Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");

                lblTotalDiscAmt.Text = totDiscAmt.ToString("0.00");
                lblTotalAmt.Text = totAmount.ToString("0.00");
                lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
                //lblAddTaxAmt.Text = totAddTaxAmt.ToString("0.00");
                lblTotalNetAmount.Text = totNetAmount.ToString("0.00");


                funCalculateTotal();
            }
            else
            {
                ViewState["totAmount"] = 0;
                ViewState["totTaxAmount"] = 0;
                ViewState["totNetAmount"] = 0;
                ViewState["totDiscAmt"] = 0;
                //ViewState["totAddTaxAmt"] = 0;
                ViewState["totSGST"] = 0;
                ViewState["totCGST"] = 0;
                ViewState["totIGST"] = 0;

                totAmount = 0;
                totTaxAmount = 0;
                totNetAmount = 0;
                totDiscAmt = 0;
                //totAddTaxAmt = 0;
                totCGST = 0;
                totSGST = 0;
                totIGST = 0;

            }
        }

        // ----------------------------------------------------------------------------------
        // Payment Schedule 
        // ----------------------------------------------------------------------------------
        public void BindPayScheduleList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SalesOrderDealerMgmt.GetPayScheduleList(pkID, OrderNo, LoginUserID);
            rptPaySchedule.DataSource = dtDetail1;
            rptPaySchedule.DataBind();
            Session.Add("dtSchedule", dtDetail1);
        }

        protected void rptPaySchedule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString().ToLower() == "addpayment")
            {
                _pageValid = true;

                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtSchedule = new DataTable();
                    dtSchedule = (DataTable)Session["dtSchedule"];

                    DataRow dr = dtSchedule.NewRow();

                    dr["pkID"] = 0;
                    string payAmt = ((TextBox)e.Item.FindControl("ftPayAmount")).Text;
                    string payDate = ((TextBox)e.Item.FindControl("ftDueDate")).Text;

                    dr["OrderNo"] = txtOrderNo.Text;
                    dr["PayAmount"] = (!String.IsNullOrEmpty(payAmt)) ? Convert.ToDecimal(payAmt) : 0;
                    dr["DueDate"] = (!String.IsNullOrEmpty(payDate)) ? payDate : "";
                    dtSchedule.Rows.Add(dr);
                    // ---------------------------------------------------------------
                    rptPaySchedule.DataSource = dtSchedule;
                    rptPaySchedule.DataBind();
                    // ---------------------------------------------------------------
                    Session.Add("dtSchedule", dtSchedule);
                }
                btnSave.Focus();

            }
            if (e.CommandName.ToString().ToLower() == "delpayment")
            {
                DataTable dtSchedule = new DataTable();
                dtSchedule = (DataTable)Session["dtSchedule"];
                // --------------------------------- Delete Record
                string payamt = ((TextBox)e.Item.FindControl("edPayAmount")).Text;
                string duedate = ((TextBox)e.Item.FindControl("edDueDate")).Text;
                foreach (DataRow dr in dtSchedule.Rows)
                {
                    if (dr["PayAmount"].ToString() == payamt && ((DateTime)dr["DueDate"]).ToString("yyyy-MM-dd") == duedate)
                    {
                        dtSchedule.Rows.Remove(dr);
                        break;
                    }
                }

                rptPaySchedule.DataSource = dtSchedule;
                rptPaySchedule.DataBind();

                Session.Add("dtSchedule", dtSchedule);
            }

        }

        // ----------------------------------------------------------------------------------
        // Setting Up Design Layout 
        // ----------------------------------------------------------------------------------
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.SalesOrderDealer> lstEntity = new List<Entity.SalesOrderDealer>();
                // ----------------------------------------------------
                lstEntity = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtOrderNo.Text = lstEntity[0].OrderNo;
                txtOrderDate.Text = lstEntity[0].OrderDate.ToString("yyyy-MM-dd");
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                txtCustomerName_TextChanged(null, null);

                drpSalesPerson.SelectedValue = (lstEntity[0].EmployeeID > 0) ? lstEntity[0].EmployeeID.ToString() : "";
                drpApprovalStatus.SelectedValue = lstEntity[0].ApprovalStatus;
                // ------------------------------------------------------------

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
                rdblOption_SelectedIndexChanged(null, null);
                // ------------------------------------------------------------
                txtTermsCondition.Text = lstEntity[0].TermsCondition;

                // -------------------------------------------------------------------------

                txtTotBasicAmt.Text = lstEntity[0].BasicAmt.ToString();
                txtHeadDiscount.Text = lstEntity[0].DiscountAmt.ToString();
                txtTotGST.Text = lstEntity[0].BasicAmt.ToString();
                txtRoff.Text = lstEntity[0].ROffAmt.ToString();
                txtTotNetAmt.Text = lstEntity[0].NetAmt.ToString();

                txtAdvPer.Text = lstEntity[0].AdvPer.ToString();
                txtAdvAmt.Text = lstEntity[0].AdvAmt.ToString();

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

                BindSalesOrderDealerDetailList(lstEntity[0].OrderNo);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            // ---------------------------------------------------
            // Resetting Temporary Table for Products
            // ---------------------------------------------------
            Session.Remove("dtDetail");
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerDetail("-1");
            Session.Add("dtDetail", dtDetail1);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnOrderNo = "", ReturnMsg1 = "";
            string strErr = "";
            _pageValid = true;

            // --------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtSchedule = new DataTable();
            dtSchedule = (DataTable)Session["dtSchedule"];

            if (String.IsNullOrEmpty(txtOrderDate.Text) || (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0"))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtOrderDate.Text))
                    strErr += "<li>" + "Order Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Name is required." + "</li>";
            }
            //---------------------------------------------------------------
            if ((Convert.ToDecimal(txtOthChrgAmt1.Text) > 0 && drpOthChrg1.SelectedValue == "")
                || (Convert.ToDecimal(txtOthChrgAmt2.Text) > 0 && drpOthChrg2.SelectedValue == "")
                || (Convert.ToDecimal(txtOthChrgAmt3.Text) > 0 && drpOthChrg3.SelectedValue == "")
                || (Convert.ToDecimal(txtOthChrgAmt4.Text) > 0 && drpOthChrg4.SelectedValue == "")
                || (Convert.ToDecimal(txtOthChrgAmt5.Text) > 0 && drpOthChrg5.SelectedValue == ""))
            {
                _pageValid = false;
                strErr += "<li>" + "Please select Other Charge Type when you are selecting Amount" + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {

                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);


                        objEntity.OrderNo = txtOrderNo.Text;
                        objEntity.OrderDate = Convert.ToDateTime(txtOrderDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.TermsCondition = txtTermsCondition.Text;
                        objEntity.EmployeeID = (!String.IsNullOrEmpty(drpSalesPerson.SelectedValue)) ? Convert.ToInt64(drpSalesPerson.SelectedValue) : 0;
                        objEntity.ApprovalStatus = drpApprovalStatus.SelectedValue;

                        objEntity.BasicAmt = (!String.IsNullOrEmpty(txtTotBasicAmt.Text)) ? Convert.ToDecimal(txtTotBasicAmt.Text) : 0;
                        //objEntity.DiscountAmt = (!String.IsNullOrEmpty(txtHeadDiscount.Text)) ? Convert.ToDecimal(txtHeadDiscount.Text) : 0;
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
                        objEntity.AdvPer = (!String.IsNullOrEmpty(txtAdvPer.Text)) ? Convert.ToDecimal(txtAdvPer.Text) : 0;
                        objEntity.AdvAmt = (!String.IsNullOrEmpty(txtAdvAmt.Text)) ? Convert.ToDecimal(txtAdvAmt.Text) : 0;

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

                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.SalesOrderDealerMgmt.AddUpdateSalesOrderDealer(objEntity, out ReturnCode, out ReturnMsg, out ReturnOrderNo);
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnOrderNo) && !String.IsNullOrEmpty(txtOrderNo.Text))
                        {
                            ReturnOrderNo = txtOrderNo.Text;
                        }
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnOrderNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                        BAL.SalesOrderDealerMgmt.DeleteSalesOrderDealerDetailByOrderNo(ReturnOrderNo, out ReturnCode1, out ReturnMsg1);
                        // --------------------------------------------------------------
                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnOrderNo))
                        {
                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            Entity.SalesOrderDealerDetail objQuotDet = new Entity.SalesOrderDealerDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.OrderNo = ReturnOrderNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.TaxType = Convert.ToInt32(dr["TaxType"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.UnitRate = Convert.ToDecimal(dr["UnitRate"]);
                                objQuotDet.DiscountPercent = Convert.ToDecimal(dr["DiscountPercent"]);
                                objQuotDet.DiscountAmt = Convert.ToDecimal(dr["DiscountAmt"]);
                                objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                                objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);

                                objQuotDet.SGSTPer = Convert.ToDecimal(dr["SGSTPer"]);
                                objQuotDet.SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]);
                                objQuotDet.CGSTPer = Convert.ToDecimal(dr["CGSTPer"]);
                                objQuotDet.CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]);
                                objQuotDet.IGSTPer = Convert.ToDecimal(dr["IGSTPer"]);
                                objQuotDet.IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]);

                                objQuotDet.TaxRate = Convert.ToDecimal(dr["TaxRate"]);
                                objQuotDet.TaxAmount = Convert.ToDecimal(dr["TaxAmount"]);
                                objQuotDet.NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.SalesOrderDealerMgmt.AddUpdateSalesOrderDealerDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            BAL.SalesOrderDealerMgmt.DeleteSalesOrderDealerPaySchedule(ReturnOrderNo, out ReturnCode1, out ReturnMsg1);
                            if (ReturnCode1 > 0)
                            {
                                Entity.SalesOrderDealer objPaySch = new Entity.SalesOrderDealer();
                                foreach (DataRow dr in dtSchedule.Rows)
                                {
                                    objPaySch.pkID = 0;
                                    objPaySch.OrderNo = ReturnOrderNo;
                                    objPaySch.PayAmount = Convert.ToDecimal(dr["PayAmount"]);
                                    objPaySch.DueDate = Convert.ToDateTime(dr["DueDate"]);
                                    objPaySch.LoginUserID = Session["LoginUserID"].ToString();
                                    BAL.SalesOrderDealerMgmt.AddUpdateSalesOrderDealerPaySchedule(objPaySch, out ReturnCode1, out ReturnMsg1);
                                }
                            }


                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            // SAVE - Product Detail Specification 
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            int ReturnCode91 = 0;
                            string ReturnMsg91 = "";
                            if (ReturnCode > 0)
                            {
                                DataTable dtSpecs = new DataTable();
                                dtSpecs = (DataTable)Session["dtSpecs"];
                                if (dtSpecs != null)
                                {
                                    foreach (DataRow dr in dtSpecs.Rows)
                                    {
                                        if (dr.RowState.ToString() != "Deleted")
                                        {
                                            Entity.ProductDetailCard lstObject = new Entity.ProductDetailCard();

                                            if (!String.IsNullOrEmpty(dr["pkID"].ToString()) && dr["pkID"].ToString() != "0")
                                                lstObject.pkID = Convert.ToInt64(dr["pkID"].ToString());
                                            else
                                                lstObject.pkID = 0;
                                            lstObject.OrderNo = ReturnOrderNo;
                                            lstObject.FinishProductID = Convert.ToInt64(dr["FinishProductID"].ToString());
                                            lstObject.GroupHead = dr["GroupHead"].ToString();
                                            lstObject.MaterialHead = dr["MaterialHead"].ToString();
                                            lstObject.MaterialSpec = dr["MaterialSpec"].ToString();
                                            lstObject.ItemOrder = dr["ItemOrder"].ToString();
                                            lstObject.LoginUserID = Session["LoginUserID"].ToString();
                                            // -------------------------------------------------------------- Insert/Update Record
                                            BAL.ProductMgmt.AddUpdateSalesOrderProductSpec(lstObject, out ReturnCode91, out ReturnMsg91);
                                        }
                                    }
                                }
                                // ---------------------------------------------------
                                // Removing Unwanted Specification
                                // ---------------------------------------------------
                                BAL.ProductMgmt.DeleteUnwantedSalesOrderSpec(ReturnOrderNo);
                            }
                            // --------------------------------------------------------------
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                            }
                        }
                        // --------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            if (!String.IsNullOrEmpty(txtOrderNo.Text))
                            {

                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnOrderNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
                            else
                            {
                                txtOrderNo.Text = ReturnOrderNo;
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnOrderNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
                        }
                    }
                    else
                    {
                        strErr += "<li>Atleast One Item is required to save Sales Order Information !</li>";
                    }
                }
                else
                {
                    strErr += "<li>Atleast One Item is required to save Sales Order Information !</li>";
                }
            }
            // ------------------------------------------------------
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
            txtOrderDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtOrderNo.Text = ""; // BAL.CommonMgmt.GetSalesOrderNo(txtOrderDate.Text);
            txtTermsCondition.Text = "";
            //drpCustomer.SelectedValue = "";
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpReferenceNo.Items.Clear();
            drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            divEmployee.Visible = false;

            hdnCustStateID.Value = "";

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

            txtTotBasicAmt.Text = "0";
            txtTotOthChrgBeforeGST.Text = "0";
            hdnTotCGSTAmt.Value = "0";
            hdnTotSGSTAmt.Value = "0";
            hdnTotIGSTAmt.Value = "0";
            hdnTotItemGST.Value = "0";
            txtTotGST.Text = "0";
            txtTotOthChrgAfterGST.Text = "0";
            txtRoff.Text = "0";
            txtTotNetAmt.Text = "0";
            txtAdvAmt.Text = "0";
            txtAdvPer.Text = "0";

            ViewState["totAmount"] = 0;
            ViewState["totTaxAmount"] = 0;
            ViewState["totNetAmount"] = 0;
            ViewState["totDiscAmt"] = 0;
            //ViewState["totAddTaxAmt"] = 0;
            ViewState["totSGST"] = 0;
            ViewState["totCGST"] = 0;
            ViewState["totIGST"] = 0;

            totAmount = 0;
            totTaxAmount = 0;
            totNetAmount = 0;
            totDiscAmt = 0;
            //totAddTaxAmt = 0;
            totCGST = 0;
            totSGST = 0;
            totIGST = 0;


            BindSalesOrderDealerDetailList("");
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptOrderDetail.Controls[rptOrderDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
            //TextBox txtAddTaxPer = ((TextBox)rptFootCtrl.FindControl("txtAddTaxPer"));
            //TextBox txtAddTaxAmt = ((TextBox)rptFootCtrl.FindControl("txtAddTaxAmt"));
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

            if (lstEntity.Count > 0)
            {
                txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
                // ----------------------------------------------------------
                // Fetching UnitPrice from ... PriceList 
                // ----------------------------------------------------------
                decimal plUnitRate = 0, plDiscount = 0;
                hdnCustomerID.Value = (String.IsNullOrEmpty(hdnCustomerID.Value)) ? "0" : hdnCustomerID.Value;
                hdnProductID.Value = (String.IsNullOrEmpty(hdnProductID.Value)) ? "0" : hdnProductID.Value;
                BAL.CommonMgmt.GetProductPriceListRate(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), out plUnitRate, out plDiscount);


                txtUnitRate.Text = plUnitRate.ToString();   // (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
                txtDiscountPercent.Text = plDiscount.ToString();
                txtDiscountAmt.Text = "0";
                txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
                hdnTaxType.Value = (lstEntity.Count > 0) ? lstEntity[0].TaxType.ToString() : "0";

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

                editItem_TextChanged1();
                txtQuantity.Focus();
            }
            else
            {
                String strErr = "";
                strErr += "<li> Select Proper Item From List !</li>";
            }
        }


        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();

            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && String.IsNullOrEmpty(txtCustomerName.Text))       //For Order generation from inquiry no - dashboard
                txtCustomerName.Text = (lstEntity.Count > 0) ? lstEntity[0].CustomerName : "";

            hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";
            DataTable dtDetail = new DataTable();
            dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail);

            ////////////////////////////////////////////////////////

            bindInqQuotation();
        }

        private void bindInqQuotation()
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
            else if (String.IsNullOrEmpty(drpTNC.SelectedValue))
            {
                txtTermsCondition.Text = "";
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteSalesOrderDealer(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.SalesOrderDealerMgmt.DeleteSalesOrderDealer(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string GetSalesOrderDealerNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetSalesOrderDealerNo(pkID);
            return tempVal;
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            HiddenField edTaxType = (HiddenField)item.FindControl("edTaxType");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
            TextBox edDiscountPercent = (TextBox)item.FindControl("edDiscountPercent");
            TextBox edDiscountAmt = (TextBox)item.FindControl("edDiscountAmt");
            TextBox edNetRate = (TextBox)item.FindControl("edNetRate");
            TextBox edAmount = (TextBox)item.FindControl("edAmount");
            TextBox edTaxRate = (TextBox)item.FindControl("edTaxRate");
            TextBox edTaxAmount = (TextBox)item.FindControl("edTaxAmount");
            //TextBox edAddTaxPer = (TextBox)item.FindControl("edAddTaxPer");
            //TextBox edAddTaxAmt = (TextBox)item.FindControl("edAddTaxAmt");
            TextBox edNetAmount = (TextBox)item.FindControl("edNetAmount");

            HiddenField edhdnCGSTPer = ((HiddenField)item.FindControl("edhdnCGSTPer"));
            HiddenField edhdnSGSTPer = ((HiddenField)item.FindControl("edhdnSGSTPer"));
            HiddenField edhdnIGSTPer = ((HiddenField)item.FindControl("edhdnIGSTPer"));

            HiddenField edhdnCGSTAmt = ((HiddenField)item.FindControl("edhdnCGSTAmt"));
            HiddenField edhdnSGSTAmt = ((HiddenField)item.FindControl("edhdnSGSTAmt"));
            HiddenField edhdnIGSTAmt = ((HiddenField)item.FindControl("edhdnIGSTAmt"));
            // --------------------------------------------------------------------------

            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            Decimal ur = (!String.IsNullOrEmpty(edUnitRate.Text)) ? Convert.ToDecimal(edUnitRate.Text) : 0;
            Decimal dp = (!String.IsNullOrEmpty(edDiscountPercent.Text)) ? Convert.ToDecimal(edDiscountPercent.Text) : 0;
            Decimal dpa = (!String.IsNullOrEmpty(edDiscountAmt.Text)) ? Convert.ToDecimal(edDiscountAmt.Text) : 0;
            Decimal nr = (!String.IsNullOrEmpty(edNetRate.Text)) ? Convert.ToDecimal(edNetRate.Text) : 0;
            Decimal a = (!String.IsNullOrEmpty(edAmount.Text)) ? Convert.ToDecimal(edAmount.Text) : 0;
            Decimal tr = (!String.IsNullOrEmpty(edTaxRate.Text)) ? Convert.ToDecimal(edTaxRate.Text) : 0;
            Decimal ta = (!String.IsNullOrEmpty(edTaxAmount.Text)) ? Convert.ToDecimal(edTaxAmount.Text) : 0;
            //Decimal at = (!String.IsNullOrEmpty(edAddTaxPer.Text)) ? Convert.ToDecimal(edAddTaxPer.Text) : 0;
            //Decimal ata = (!String.IsNullOrEmpty(edAddTaxAmt.Text)) ? Convert.ToDecimal(edAddTaxAmt.Text) : 0;
            Decimal na = (!String.IsNullOrEmpty(edNetAmount.Text)) ? Convert.ToDecimal(edNetAmount.Text) : 0;
            Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(edTaxType.Value)) ? Convert.ToInt16(edTaxType.Value) : 0);

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

            edDiscountPercent.Text = ItmDiscPer1.ToString();
            edDiscountAmt.Text = ItmDiscAmt1.ToString();
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

            // --------------------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductID"].ToString() == edProductID.Value)
                {
                    row.SetField("Quantity", edQuantity.Text);
                    row.SetField("TaxType", edTaxType.Value);
                    row.SetField("UnitRate", edUnitRate.Text);
                    row.SetField("DiscountPercent", edDiscountPercent.Text);
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
                    row.SetField("NetAmount", edNetAmount.Text);
                }
                else
                {
                    if (!String.IsNullOrEmpty(txtHeadDiscount.Text))
                    {
                        Decimal a1 = (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                        Decimal q1 = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                        Decimal ur1 = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                        Decimal dp1 = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                        Decimal dpa1 = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                        Decimal tr1 = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                        Decimal at1 = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;
                        Int16 taxtype1 = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);

                        TaxAmt = 0;
                        CGSTPer = 0; CGSTAmt = 0;
                        SGSTPer = 0; SGSTAmt = 0; IGSTPer = 0; IGSTAmt = 0; NetRate = 0; BasicAmt = 0; NetAmt = 0; ItmDiscPer1 = 0; ItmDiscAmt1 = 0; AddTaxAmt = 0; HeadDiscAmt1 = 0;
                        BAL.CommonMgmt.funCalculate(taxtype1, q1, ur1, dp1, dpa1, tr1, at1, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

                        row.SetField("NetRate", NetRate);
                        row.SetField("Amount", BasicAmt);
                        row.SetField("CGSTPer", CGSTPer);
                        row.SetField("SGSTPer", SGSTPer);
                        row.SetField("IGSTPer", IGSTPer);
                        row.SetField("CGSTAmt", CGSTAmt);
                        row.SetField("SGSTAmt", SGSTAmt);
                        row.SetField("IGSTAmt", IGSTAmt);
                        row.SetField("TaxAmount", CGSTAmt + SGSTAmt + IGSTAmt);
                        row.SetField("AddTaxAmt", AddTaxAmt);
                        row.SetField("NetAmt", NetAmt);
                        row.SetField("NetAmount", NetAmt);
                    }
                }
            }
            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
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
        protected void txtTaxRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
        }

        protected void drpReferenceNo_TextChanged(object sender, EventArgs e)
        {
            if (rdblOption.SelectedValue == "Quotation")
            {
                int TotalCount = 0;
                if (!String.IsNullOrEmpty(drpReferenceNo.SelectedValue) && drpReferenceNo.SelectedValue != "")
                {
                    List<Entity.QuotationDetail> lstEntity = new List<Entity.QuotationDetail>();
                    // ----------------------------------------------------
                    lstEntity = BAL.QuotationDetailMgmt.GetQuotationDetailListByQuotationNo(drpReferenceNo.SelectedValue, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                    if (lstEntity.Count > 0)
                    {
                        txtTermsCondition.Text = lstEntity[0].QuotationFooter.ToString();
                        //rptOrderDetail.DataSource = lstEntity;
                        //rptOrderDetail.DataBind();
                    }
                }
            }
        }

        private static byte[] showQRCode(string pQuotationNo, string pCustomerName)
        {

            byte[] returnValue;
            PageBase pb = new PageBase();
            string code = pQuotationNo + "," + pCustomerName;
            //string encCode = pb.Encrypt(code, "r0b1nr0y");
            string encCode = code;

            qrGenerator = new QRCodeGenerator();

            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(encCode, QRCodeGenerator.ECCLevel.Q);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

            imgBarCode.Style.Add("top", "-15px !important");
            imgBarCode.Style.Add("height", "150px !important");
            imgBarCode.Style.Add("width", "150px !important");
            imgBarCode.Style.Add("margin-top", "10px !important");
            imgBarCode.Style.Add("float", "right");
            imgBarCode.Style.Add("position", "relative");
            imgBarCode.Style.Add("padding-right", "10px");

            using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    returnValue = byteImage;
                }
            }
            return returnValue;
        }

        private static QRCodeGenerator qrGenerator;

        public static iTextSharp.text.Image imgQRCode;

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Sales Order Dealer
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        [WebMethod(EnableSession = true)]
        public static void GenerateSalesOrderDealer(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // -----------------------------------------------------------------------
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            // ----------------------------------------------
            tmpSerialKey = "SI08-SB94-MY45-RY1X";
            // ----------------------------------------------
            if (tmpSerialKey == "SI08-SB94-MY45-RY15")          // Sharvaya Infotech
                GenerateSalesOrderDealer_Sharvaya(pkID);
        }

        public static void GenerateSalesOrderDealer_Sharvaya(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(8);
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

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "SalesOrderDealer");

            ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                }
            }

            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Dealer Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesOrderDealer> lstQuot = new List<Entity.SalesOrderDealer>();
            lstQuot = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerDetail(lstQuot[0].OrderNo);
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstCust.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 9999, out TotalCount);
            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(lstQuot[0].CompanyID, 1, 1000, out totrec);

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
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 20, 33, 22 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("Buyer :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstQuot[0].Address.Trim() + lstQuot[0].Area.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstQuot[0].Address + "," + lstQuot[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstQuot[0].City.Trim() + lstQuot[0].PinCode.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstQuot[0].City + "," + lstQuot[0].PinCode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstQuot[0].ContactNo1))
                    tblNested20.AddCell(pdf.setCell("Contact No: " + lstQuot[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstQuot[0].EmailAddress))
                    tblNested20.AddCell(pdf.setCell("Email     : " + lstQuot[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 26, 44, 10, 20 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Order No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].OrderNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].OrderDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));

                tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell(": " + ((dtContact.Rows.Count > 0) ? dtContact.Rows[0]["ContactPerson1"].ToString() : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell("GSTIN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell(": " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell("PAN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblNested2.AddCell(pdf.setCell(": " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));


                tblMember.AddCell(pdf.setCell("Dealer Sales Order", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                //tblMember.AddCell(pdf.setCell("We acknowledge with thanks the receipt of your above started inquiry. We are pleased to offer our most competitive rates as", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 10, 33, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 10, 39, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Discount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

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
                    tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }

                //if (ProdDetail_Lines > dtItem.Rows.Count)
                //{
                //    for (int i = 1; i <= (ProdDetail_Lines - dtItem.Rows.Count ); i++)
                //    {
                //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                //        if (sumDis > 0)
                //        {
                //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                //        }
                //        else
                //        {
                //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                //        }
                //    }
                //}

                if (ProdDetail_Lines > dtItem.Rows.Count)
                {
                    for (int i = 1; i <= (ProdDetail_Lines - dtItem.Rows.Count); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (sumDis > 0)
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                    }
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(1);
                int[] column_tblTNC = { 100 };
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // ---------------------------------------------------------------------------------------------------------
                PdfPTable tblAmount = new PdfPTable(2);
                int[] column_tblAmount = { 60, 40 };
                tblAmount.SetWidths(column_tblAmount);
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                totAmount = lstQuot[0].BasicAmt;
                totRNDOff = lstQuot[0].ROffAmt;
                totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt1;
                    befGST += lstQuot[0].ChargeGSTAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                //if (befAmt > 0)
                //{
                //    tblAmount.AddCell(setCell("Total     :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(setCell((totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                {
                    if (lstQuot[0].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        else
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                    else
                    {
                        if ((lstQuot[0].CGSTAmt + lstQuot[0].SGSTAmt) > 0)
                        {
                            tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            else
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                            tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            else
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        }
                    }
                }
                //tblAmount.AddCell(setCell("Total     :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                //tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST)).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));

                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt4;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt5;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //tblAmount.AddCell(setCell("Grand Total  :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }

                // ****************************************************************
                //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
                netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Total Amount ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                #endregion
                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);
                string tmp = "Thank you for your above stated purchase order. We are pleased to confirm having registerd your order subject to above Terms & Conditions. If we do not get any information from your side within 3 days. We will consider above order Acceptance is accepted to you.";
                tblFootDetail.AddCell(pdf.setCell(tmp, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstBank[0].BankName + " - Branch : " + lstBank[0].BranchName, pdf.fnCalibriBold8));
                phrase1.Add(new Chunk(", A/c No : " + lstBank[0].BankAccountNo + ", IFSC: " + lstBank[0].BankIFSC, pdf.fnCalibri8));


                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell(phrase1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
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
                else
                {
                    tblESignature.AddCell(pdf.setCell("For " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("We hope you will find above rate in line with your requirement. We assure you of our best services with maximum technical", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblSignOff.AddCell(pdf.setCell("supports at all times", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                tblSignOff.AddCell(pdf.setCell("Thanking you and awaiting for your valed offer.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                tblSignOff.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].OrderNo.Replace("/", "-").ToString() + ".pdf";
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

            // >>>>>> Adding Organization Name 
            //tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //pdfDoc.Add(tableHeader);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

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

            // >>>>>> Adding Quotation Footer
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);

            // >>>>>> Adding Quotation Header
            tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSignOff);

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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Dealer Performa
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        [WebMethod(EnableSession = true)]
        public static void GenerateDealerSalesPerfoma(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // -----------------------------------------------------------------------
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

            tmpSerialKey = "SI08-SB94-MY45-RY15";

            if (tmpSerialKey == "SI08-SB94-MY45-RY15")               // Sharvaya Infotech
                GenerateSalesPerfoma_Sharvaya(pkID);
        }

        public static void GenerateSalesPerfoma_Sharvaya(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(8);
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

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Proforma");

            ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                }
            }

            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);

            //Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            //pdfDoc.SetMargins(30, 30, 40, 0);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
            // -------------------------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesOrderDealer> lstQuot = new List<Entity.SalesOrderDealer>();
            lstQuot = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesOrderDealerMgmt.GetSalesOrderDealerDetail(lstQuot[0].OrderNo);
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
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 20, 33, 22 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("Buyer :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].Address.Trim() + lstCust[0].Area.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.Trim() + lstCust[0].Pincode.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName + "," + lstCust[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 30, 40, 10, 20 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Order No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].OrderNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].OrderDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("P.O. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": Verbally", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].OrderDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //lstQuot[0].TransporterName
                tblNested2.AddCell(pdf.setCell("Transport ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                PdfPTable tblNested202 = new PdfPTable(1);
                int[] column_tblNested202 = { 100 };
                tblNested202.SetWidths(column_tblNested202);

                tblNested202.AddCell(pdf.setCell("Consignee :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstOrg[0].Address))
                    tblNested202.AddCell(pdf.setCell(lstOrg[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstOrg[0].CityName + lstOrg[0].Pincode))
                    tblNested202.AddCell(pdf.setCell(lstOrg[0].CityName + " - " + lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstOrg[0].StateName))
                    tblNested202.AddCell(pdf.setCell(lstOrg[0].StateName + ",India", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell("Phone : " + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblNested22 = new PdfPTable(4);
                int[] column_tblNested22 = { 30, 40, 10, 20 };
                tblNested22.SetWidths(column_tblNested22);
                tblNested22.AddCell(pdf.setCell("Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell(": " + "", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell("GSTIN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell(": " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell("CIN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell(": " + objAuth.CINNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell("PAN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell(": " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));

                tblMember.AddCell(pdf.setCell("Perfoma Invoice", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested202, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested22, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 10, 33, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 10, 39, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Discount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

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
                    tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));

                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }

                //for (int i = 1; i < (13 - dtItem.Rows.Count); i++)
                //{
                if (ProdDetail_Lines > dtItem.Rows.Count)
                {
                    for (int i = 1; i <= (ProdDetail_Lines - dtItem.Rows.Count); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (sumDis > 0)
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                    }
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(1);
                int[] column_tblTNC = { 100 };
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // ---------------------------------------------------------------------------------------------------------
                PdfPTable tblAmount = new PdfPTable(2);
                int[] column_tblAmount = { 60, 40 };
                tblAmount.SetWidths(column_tblAmount);
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                totAmount = lstQuot[0].BasicAmt;
                totRNDOff = lstQuot[0].ROffAmt;
                totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt1;
                    befGST += lstQuot[0].ChargeGSTAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                //if (befAmt > 0)
                //{
                //    tblAmount.AddCell(setCell("Total     :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(setCell((totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                {
                    if (lstQuot[0].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        else
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                    else
                    {
                        if ((lstQuot[0].CGSTAmt + lstQuot[0].SGSTAmt) > 0)
                        {
                            tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            else
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                            tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            else
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        }
                    }
                }
                //tblAmount.AddCell(setCell("Total     :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                //tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST)).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));

                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt4;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt5;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //tblAmount.AddCell(setCell("Grand Total  :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }

                // ****************************************************************
                //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
                netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Total Amount ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                //var ph1 = new Phrase();
                //ph1.Add(new Chunk("GSTIN No : ", pdf.fnCalibriBold8));
                //ph1.Add(new Chunk("24BIQPK4338E1Z8", pdf.fnCalibri8));
                //tblDetail.AddCell(pdf.setCell(ph1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);
                string tmp = "We hope you will find above rates in line with your requirement. We assure you of our best services with maximum technical supports at all times.";
                tblFootDetail.AddCell(pdf.setCell(tmp, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //var phrase1 = new Phrase();
                //phrase1.Add(new Chunk("1. ", pdf.fnCalibri8));
                //phrase1.Add(new Chunk("Kotak Bank", pdf.fnCalibriBold8));
                //phrase1.Add(new Chunk(", A/c No : 0911539231, IFSC: KKBK0000159, Swift Code: KKBKINBB, Maninagar, Ahmedabad.", pdf.fnCalibri8));

                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstBank[0].BankName + " -   Branch : " + lstBank[0].BranchName, pdf.fnCalibriBold8));
                phrase1.Add(new Chunk(", A/c No : " + lstBank[0].BankAccountNo + ", IFSC: " + lstBank[0].BankIFSC, pdf.fnCalibri8));

                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell(phrase1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
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
                else
                {
                    tblESignature.AddCell(pdf.setCell("For " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURISDICTION", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].OrderNo.Replace("/", "-").ToString() + ".pdf";
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

            // >>>>>> Adding Organization Name 
            //tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //pdfDoc.Add(tableHeader);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

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

            // >>>>>> Adding Quotation Footer
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);

            // >>>>>> Adding Quotation Header
            tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSignOff);

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

        protected void editItem_TextChanged1()
        {
            Control rptFootCtrl = rptOrderDetail.Controls[rptOrderDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
            TextBox txtNetRate = ((TextBox)rptFootCtrl.FindControl("txtNetRate"));

            TextBox txtAmount = ((TextBox)rptFootCtrl.FindControl("txtAmount"));
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
            TextBox txtTaxAmount = ((TextBox)rptFootCtrl.FindControl("txtTaxAmount"));

            //TextBox txtAddTaxPer = ((TextBox)rptFootCtrl.FindControl("txtAddTaxPer"));
            //TextBox txtAddTaxAmt = ((TextBox)rptFootCtrl.FindControl("txtAddTaxAmt"));

            TextBox txtNetAmount = ((TextBox)rptFootCtrl.FindControl("txtNetAmount"));
            HiddenField hdnTaxType = ((HiddenField)rptFootCtrl.FindControl("hdnTaxType"));

            HiddenField hdnCGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTPer"));
            HiddenField hdnSGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTPer"));
            HiddenField hdnIGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTPer"));

            HiddenField hdnCGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTAmt"));
            HiddenField hdnSGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTAmt"));
            HiddenField hdnIGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTAmt"));
            // --------------------------------------------------------------------------
            Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);
            Decimal dp = String.IsNullOrEmpty(txtDiscountPercent.Text) ? 0 : Convert.ToDecimal(txtDiscountPercent.Text);
            Decimal dpa = String.IsNullOrEmpty(txtDiscountAmt.Text) ? 0 : Convert.ToDecimal(txtDiscountAmt.Text);
            Decimal nr = String.IsNullOrEmpty(txtNetRate.Text) ? 0 : Convert.ToDecimal(txtNetRate.Text);
            Decimal a = String.IsNullOrEmpty(txtAmount.Text) ? 0 : Convert.ToDecimal(txtAmount.Text);
            Decimal tr = String.IsNullOrEmpty(txtTaxRate.Text) ? 0 : Convert.ToDecimal(txtTaxRate.Text);
            Decimal ta = String.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
            //Decimal at = String.IsNullOrEmpty(txtAddTaxPer.Text) ? 0 : Convert.ToDecimal(txtAddTaxPer.Text);
            //Decimal ata = String.IsNullOrEmpty(txtAddTaxAmt.Text) ? 0 : Convert.ToDecimal(txtAddTaxAmt.Text);
            Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            Int16 taxtype = Convert.ToInt16(String.IsNullOrEmpty(hdnTaxType.Value) ? 0 : Convert.ToInt16(hdnTaxType.Value));

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

            txtDiscountPercent.Text = ItmDiscPer1.ToString();
            txtDiscountAmt.Text = ItmDiscAmt1.ToString();
            //txtAddTaxAmt.Text = AddTaxAmt.ToString();
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

        protected void txtOthChrgAmt1_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg1.SelectedValue)) ? Convert.ToInt64(drpOthChrg1.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt1.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt1.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

            hdnOthChrgGST1.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic1.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void txtOthChrgAmt2_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg2.SelectedValue)) ? Convert.ToInt64(drpOthChrg2.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt2.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt2.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

            hdnOthChrgGST2.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic2.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void txtOthChrgAmt3_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg3.SelectedValue)) ? Convert.ToInt64(drpOthChrg3.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt3.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt3.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

            hdnOthChrgGST3.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic3.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void txtOthChrgAmt4_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg4.SelectedValue)) ? Convert.ToInt64(drpOthChrg4.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt4.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt4.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

            hdnOthChrgGST4.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic4.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
        }

        protected void txtOthChrgAmt5_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg5.SelectedValue)) ? Convert.ToInt64(drpOthChrg5.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt5.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt5.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

            hdnOthChrgGST5.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic5.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
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
            //txtTotAddTaxAmt.Text = Convert.ToDecimal(ViewState["totAddTaxAmt"]).ToString("0.00");
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
            NetAmt = (Convert.ToDecimal(ViewState["totNetAmount"]) + (Convert.ToDecimal(hdnOthChrgGST1.Value))
                + (Convert.ToDecimal(hdnOthChrgGST2.Value)) + (Convert.ToDecimal(hdnOthChrgGST3.Value)) + (Convert.ToDecimal(hdnOthChrgGST4.Value))
                + (Convert.ToDecimal(hdnOthChrgGST5.Value)) + (Convert.ToDecimal(hdnOthChrgBasic1.Value)) + (Convert.ToDecimal(hdnOthChrgBasic2.Value))
                + (Convert.ToDecimal(hdnOthChrgBasic3.Value)) + (Convert.ToDecimal(hdnOthChrgBasic4.Value)) + (Convert.ToDecimal(hdnOthChrgBasic5.Value))
                    );

            txtTotNetAmt.Text = Math.Round(NetAmt, 0).ToString("0.00");
            txtRoff.Text = (Math.Round(NetAmt, 0) - Math.Round(NetAmt, 2)).ToString("0.00");
        }

        protected void drpOthChrg1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt1_TextChanged(sender, e);
            if (drpOthChrg1.SelectedValue == "")
            {
                txtOthChrgAmt1.Text = "0";
            }
        }
        protected void drpOthChrg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt2_TextChanged(sender, e);
            if (drpOthChrg2.SelectedValue == "")
            {
                txtOthChrgAmt2.Text = "0";
            }
        }
        protected void drpOthChrg3_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt3_TextChanged(sender, e);
            if (drpOthChrg3.SelectedValue == "")
            {
                txtOthChrgAmt3.Text = "0";
            }
        }
        protected void drpOthChrg4_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt4_TextChanged(sender, e);
            if (drpOthChrg4.SelectedValue == "")
            {
                txtOthChrgAmt4.Text = "0";
            }

        }
        protected void drpOthChrg5_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt5_TextChanged(sender, e);
            if (drpOthChrg5.SelectedValue == "")
            {
                txtOthChrgAmt5.Text = "0";
            }
        }

        protected void rdblOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindInqQuotation();
        }

        protected void txtHeadDiscount_TextChanged(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

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
                    HeaderDiscItemWise = 0;
                    Decimal a = (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                    Decimal q = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                    Decimal ur = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                    Decimal dp = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                    Decimal dpa = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                    Decimal tr = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                    Decimal at = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;
                    Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);

                    Decimal nr = (!String.IsNullOrEmpty(row["NetRate"].ToString())) ? Convert.ToDecimal(row["NetRate"]) : 0;

                    HeaderDiscItemWise = Math.Round((HeaderDiscAmt * a) / TotalAmt, 2);

                    decimal TaxAmt = 0;
                    decimal CGSTPer = 0, CGSTAmt = 0;
                    decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
                    BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, HeaderDiscItemWise, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

                    row.SetField("HeaderDiscAmt", HeaderDiscItemWise);
                    row.SetField("NetRate", NetRate);
                    row.SetField("Amount", BasicAmt);
                    row.SetField("CGSTPer", CGSTPer);
                    row.SetField("SGSTPer", SGSTPer);
                    row.SetField("IGSTPer", IGSTPer);
                    row.SetField("CGSTAmt", CGSTAmt);
                    row.SetField("SGSTAmt", SGSTAmt);
                    row.SetField("IGSTAmt", IGSTAmt);
                    row.SetField("TaxAmount", CGSTAmt + SGSTAmt + IGSTAmt);
                    row.SetField("AddTaxAmt", AddTaxAmt);
                    row.SetField("NetAmt", NetAmt);
                    row.SetField("NetAmount", NetAmt);
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


            txtHeadDiscount.Focus();
        }

        protected void txtAdvPer_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAdvPer.Text))
            {
                txtAdvAmt.Text = (Math.Round((Convert.ToDouble(txtTotNetAmt.Text) * Convert.ToDouble(txtAdvPer.Text)) / 100, 2)).ToString();
            }
        }
    }

    class RoundedBorder2 : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] canvas)
        {
            PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.RoundRectangle(
              rect.Left + 1.5f,
              rect.Bottom + 1.5f,
              rect.Width - 3,
              rect.Height - 3, 4
            );
            cb.Stroke();
        }
    }
}