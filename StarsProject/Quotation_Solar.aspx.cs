using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using iTextSharp.text.html;

namespace StarsProject
{
    public partial class Quotation_Solar : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount, totDiscAmt, totAddTaxAmt, totSGST, totCGST, totIGST;
        public static Int64 companyid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                Session["dtSpecs"] = null;
                Session["ProductShortRemark"] = "";
                Session.Remove("mySpecs");

                // --------------------------------------------------------
                BindDropDown();

                hdnSerialKey.Value = Session["SerialKey"].ToString();
                hdnQuotationVersion.Value = BAL.CommonMgmt.GetConstant("QuotationVersion", 0, 1);
                hdnQuotationCurrency.Value = BAL.CommonMgmt.GetConstant("QuotationCurrency", 0, 1);
                hdnQuotationSpecification.Value = BAL.CommonMgmt.GetConstant("QuotationSpecification", 0, 1);
                hdnPriceRangeValidation.Value = BAL.CommonMgmt.GetConstant("PriceRangeValidation", 0, 1);
                hdnApplicationIndustry.Value = BAL.CommonMgmt.GetConstant("ApplicationIndustry", 0, 1);
                hdnCustWisePro.Value = BAL.CommonMgmt.GetConstant("CustomerWiseProducts", 0, 1);
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
                            if (!String.IsNullOrEmpty(Request.QueryString["InquiryNo"]))
                            {
                                drpInquiry.SelectedValue = (!String.IsNullOrEmpty(Request.QueryString["InquiryNo"])) ? Request.QueryString["InquiryNo"] : "";
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
                            if (hdnMode.Value.ToLower() == "view")
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
                        if (!String.IsNullOrEmpty(txtQuotationNo.Text) || txtQuotationNo.Text != "")
                        {
                            lst = BAL.ProductMgmt.GetQuotationProductSpecList(txtQuotationNo.Text, 0, Session["LoginUserID"].ToString());
                            if (lst.Count > 0)
                            {
                                dtSpecs = PageBase.ConvertListToDataTable(lst);
                                Session["dtSpecs"] = dtSpecs;
                            }
                        }
                    }
                }

            }
            // ------------------------------------------------
            if (hdnMode.Value.ToLower() == "edit")
            {
                divLoad1.Style.Add("display", "none");
                divLoad2.Style.Add("display", "none");
            }
            // ------------------------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:retainTabPosition();", true);
        }
        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            GetInquiryProducts();
        }
        public void GetInquiryProducts()
        {
            if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(hdnSelectedReference.Value))
            {
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                dtDetail.Clear();
                dtDetail = BAL.InquiryInfoMgmt.GetInquiryProductForQuotation(hdnSelectedReference.Value, txtQuotationNo.Text);
                Session.Add("dtDetail", dtDetail);

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


                DataTable dtDetail1 = new DataTable();
                dtDetail1 = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
                rptQuotationDetail.DataSource = dtDetail1;
                rptQuotationDetail.DataBind();
                Session.Add("dtDetail", dtDetail1);
            }
            txtQuotationDate.Focus();
        }

        public void OnlyViewControls()
        {
            //divFollowUp.Visible = false;

            txtQuotationNo.ReadOnly = true;
            txtQuotationDate.ReadOnly = true;
            txtQuotationHeader.ReadOnly = true;
            txtQuotationFooter.ReadOnly = true;
            txtQuotationSubject.ReadOnly = true;
            //drpCustomer.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            drpInquiry.Attributes.Add("disabled", "disabled");
            drpQuotationKindAttn.Attributes.Add("disabled", "disabled");
            drpProjects.Attributes.Add("disabled", "disabled");
            drpBankID.Attributes.Add("disabled", "disabled");
            drpTNC.Attributes.Add("disabled", "disabled");

            txtNextFollowupDate.ReadOnly = true;
            txtMeetingNotes.ReadOnly = true;
            drpFollowupType.Attributes.Add("disabled", "disabled");

            btnQSave.Visible = false;
            btnQSaveEmail.Visible = false;
            btnQSaveEmail1.Visible = false;
            btnQSaveEmail2.Visible = false;
            btnQReset.Visible = false;

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
            txtHeadDiscount.ReadOnly = true;
            txtTotOthChrgBeforeGST.ReadOnly = true;
            txtTotGST.ReadOnly = true;
            txtTotOthChrgAfterGST.ReadOnly = true;
            txtRoff.ReadOnly = true;
            txtTotNetAmt.ReadOnly = true;
            txtSubsidy.ReadOnly = true;
            txtNetPayble.ReadOnly = true;

            if (hdnQuotationCurrency.Value.ToLower() == "yes")
            {
                drpCurrency.Visible = true;
                txtExchangeRate.Visible = true;
                drpCurrency.Attributes.Add("disabled", "disabled");
                txtExchangeRate.ReadOnly = true;
            }
            else
            {
                drpCurrency.Visible = false;
                txtExchangeRate.Visible = false;
            }

            pnlDetail.Enabled = false;

        }

        public void GetCurrenciesList()
        {
            List<Entity.Currency> lstEntity = new List<Entity.Currency>();
            lstEntity = BAL.CommonMgmt.GetCurrencyList();
            lstEntity = lstEntity.Where(e => !(e.ActiveFlag == true)).ToList();
            drpCurrency.DataTextField = "CurrencyName";
            drpCurrency.DataValueField = "CurrencySymbol";
            if (lstEntity.Count > 0)
            {
                drpCurrency.DataSource = lstEntity;
                drpCurrency.DataBind();
            }
            drpCurrency.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }

        public void BindDropDown()
        {
            // ---------------- Report To List -------------------------------------
            GetCurrenciesList();

            // ---------------- Report To List -------------------------------------
            List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            drpTNC.DataSource = lstList2;
            drpTNC.DataValueField = "pkID";
            drpTNC.DataTextField = "TNC_Header";
            drpTNC.DataBind();
            drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

            // ---------------- Report To List -------------------------------------
            int totrec = 0;
            List<Entity.Projects> lstList3 = new List<Entity.Projects>();
            lstList3 = BAL.ProjectsMgmt.GetProjectsList(0, "", 1, 50000, out totrec);
            drpProjects.DataSource = lstList3;
            drpProjects.DataValueField = "ProjectName";
            drpProjects.DataTextField = "ProjectName";
            drpProjects.DataBind();
            drpProjects.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Project --", ""));

            // ---------------- Report To List -------------------------------------
            List<Entity.InquiryStatus> lstOrgDept22 = new List<Entity.InquiryStatus>();
            lstOrgDept22 = BAL.InquiryStatusMgmt.GetInquiryStatusList("Followup");
            drpFollowupType.DataSource = lstOrgDept22;
            drpFollowupType.DataValueField = "pkID";
            drpFollowupType.DataTextField = "InquiryStatusName";
            drpFollowupType.DataBind();
            drpFollowupType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            //---------------------------Bank Details-------------------------------
            List<Entity.OrganizationBankInfo> lstOrgDept23 = new List<Entity.OrganizationBankInfo>();
            lstOrgDept23 = BAL.CommonMgmt.GetBankInfo(0, "", 1, 50000, out totrec);
            drpBankID.DataSource = lstOrgDept23;
            drpBankID.DataValueField = "pkID";
            drpBankID.DataTextField = "BankName";
            drpBankID.DataBind();
            drpBankID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

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

            // ---------------- Inquiry List -------------------------------------
            BindInquiryList(0);
            BindEmailCategory();
        }

        public void BindEmailCategory()
        {
            //---------------------------Email Category Details-------------------------------
            int TotalRecord = 0;
            List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
            lstEntity = BAL.EmailTemplateMgmt.GetGeneralTemplate(0, Session["LoginUserID"].ToString(), 1, Convert.ToInt32(Session["PageSize"]), out TotalRecord);
            drpQuotationCategory.DataSource = lstEntity;
            drpQuotationCategory.DataValueField = "pkID";
            drpQuotationCategory.DataTextField = "Subject";
            drpQuotationCategory.DataBind();
            drpQuotationCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }

        public void BindCustomerContacts()
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                try
                {
                    // ---------------- Report To List -------------------------------------
                    drpQuotationKindAttn.Items.Clear();
                    int TotalCount = 0;
                    List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
                    lstObject = BAL.CustomerContactsMgmt.GetCustomerContactsList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                    drpQuotationKindAttn.DataSource = lstObject;
                    drpQuotationKindAttn.DataValueField = "ContactPerson1";
                    drpQuotationKindAttn.DataTextField = "ContactPerson1";
                    drpQuotationKindAttn.DataBind();
                }
                catch (System.Exception ex)
                {

                }
                drpQuotationKindAttn.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Contact --", ""));
            }
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        public void BindInquiryList(Int64 pCustomerID)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            drpInquiry.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoListByCustomer(pCustomerID);
                // --------------------------------------------------
                lstEntity = lstEntity.Where(e => !(e.InquiryStatus == "Close - Lost" && e.InquiryStatus == "Close - Success")).ToList();
                // --------------------------------------------------
                drpInquiry.DataValueField = "InquiryNo";
                drpInquiry.DataTextField = "InquiryNo";
                if (lstEntity.Count > 0)
                {
                    drpInquiry.DataSource = lstEntity;
                    drpInquiry.DataBind();
                }
                drpInquiry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            }
            else
            {
                drpInquiry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));
            }
        }

        public void BindQuotationDetailList(string pQuotationNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.QuotationDetailMgmt.GetQuotationDetail(pQuotationNo);
            rptQuotationDetail.DataSource = dtDetail1;
            rptQuotationDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);

            int TotalRecord = 0;
            List<Entity.QuotationSubsidy> lstSub = new List<Entity.QuotationSubsidy>();
            lstSub = BAL.QuotationDetailMgmt.GetQuotationSubsidyListByQuotationNo(pQuotationNo, out TotalRecord);
            DataTable dtSubsidy = new DataTable();
            dtSubsidy = PageBase.ConvertListToDataTable(lstSub);
            Session.Add("dtSubsidy", dtSubsidy);

            //if (dtSubsidy != null)
            //{
            //    DataTable dtSummary = new DataTable();
            //    dtSummary = BAL.QuotationDetailMgmt.GetQuotationSubsidySummmary(pQuotationNo);

            //    if (dtSummary.AsEnumerable().Any(x => x.Field<string>("Type") == "upto"))
            //    {
            //        DataRow subsidyUpto = dtSummary.AsEnumerable().Where(x => x.Field<string>("Type") == "upto").FirstOrDefault();
            //        txtSubsidyQty.Text = subsidyUpto["Quantity"].ToString();
            //        txtSubsidyPer.Text = subsidyUpto["SubsidyPer"].ToString();
            //        txtSubsidyAmt.Text = subsidyUpto["SubsidyAmt"].ToString();
            //    }
            //    if (dtSummary.AsEnumerable().Any(x => x.Field<string>("Type") == "remaining"))
            //    {
            //        DataRow subsidyRem = dtSummary.AsEnumerable().Where(x => x.Field<string>("Type") == "remaining").FirstOrDefault();
            //        txtSubsidyPer1.Text = subsidyRem["SubsidyPer"].ToString();
            //        txtSubsidyAmt1.Text = subsidyRem["SubsidyAmt"].ToString();
            //    }
            //    funCalculateTotal();
            //}
        }

        protected void rptQuotationDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || Convert.ToDecimal(((TextBox)e.Item.FindControl("txtQuantity")).Text) <= 0
                        || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || Convert.ToDecimal(((TextBox)e.Item.FindControl("txtUnitRate")).Text) <= 0)
                    {
                        _pageValid = false;


                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                        {
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        }
                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || Convert.ToDecimal(((TextBox)e.Item.FindControl("txtQuantity")).Text) <= 0)
                            strErr += "<li>" + "Quantity is required." + "</li>";


                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || Convert.ToDecimal(((TextBox)e.Item.FindControl("txtUnitRate")).Text) <= 0)
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
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                                return;
                            }


                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = 0;
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
                            string bundleid = "";
                            string boxweight = "", boxsqft = "";

                            //Boolean SubsidyApplicable = ((CheckBox)e.Item.FindControl("edchkSubsidy")).Checked;

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
                            {
                                mySpecs = PageBase.ConvertListToDataTable(ProdSpec);
                            }

                            mySpecs.AcceptChanges();
                            Session.Add("mySpecs", mySpecs);
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            dr["QuotationNo"] = txtQuotationNo.Text;
                            dr["OrderNo"] = "";
                            dr["InquiryNo"] = "";
                            dr["InvoiceNo"] = "";
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["TaxType"] = (!String.IsNullOrEmpty(taxtype)) ? Convert.ToInt16(taxtype) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["UnitQty"] = (!String.IsNullOrEmpty(untqty)) ? Convert.ToDecimal(untqty) : 1;
                            dr["UnitQuantity"] = (!String.IsNullOrEmpty(unitqty)) ? Convert.ToInt64(unitqty) : 1;
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
                            //dr["HeaderDiscAmt"] = 0;
                            dr["HeaderDiscAmt"] = (!String.IsNullOrEmpty(headdiscamt)) ? Convert.ToDecimal(headdiscamt) : 0;

                            dr["BundleId"] = (!String.IsNullOrEmpty(bundleid)) ? Convert.ToInt64(bundleid) : 0;

                            dr["Box_Weight"] = (!String.IsNullOrEmpty(boxweight)) ? Convert.ToInt64(boxweight) : 0;
                            dr["Box_SQFT"] = (!String.IsNullOrEmpty(boxsqft)) ? Convert.ToInt64(boxsqft) : 0;
                            dr["Box_SQMT"] = (!String.IsNullOrEmpty(boxsqft)) ? Convert.ToInt64(boxsqft) : 0;
                            dr["ForOrderNo"] = " ";
                            dr["ProductSpecification"] = (ProdSpec.Count > 0) ? ProdSpec[0].ProductSpecification : "";
                            //dr["SubsidyApplicable"] = SubsidyApplicable;
                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptQuotationDetail.DataSource = dtDetail;
                            rptQuotationDetail.DataBind();
                            // ---------------------------------------------------------------0.00
                            Session.Add("dtDetail", dtDetail);

                            //if (SubsidyApplicable)
                            //{

                            TextBox edtxtSubsidyQty = ((TextBox)e.Item.FindControl("edtxtSubsidyQty"));
                            TextBox edtxtSubsidyPer = ((TextBox)e.Item.FindControl("edtxtSubsidyPer"));
                            TextBox edtxtSubsidyPer1 = ((TextBox)e.Item.FindControl("edtxtSubsidyPer1"));

                            Decimal QuantityUpto = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyQty.Text) ? "0" : edtxtSubsidyQty.Text);
                            Decimal SubsidyPer = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer.Text) ? "0" : edtxtSubsidyPer.Text);
                            Decimal SubsidyPer1 = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer1.Text) ? "0" : edtxtSubsidyPer1.Text);

                            Decimal subsidyUpto =0, subsidyUpto1=0;
                            funSetSubsidy(Convert.ToInt64(icode), QuantityUpto, SubsidyPer, SubsidyPer1,out subsidyUpto,out subsidyUpto1);

                            ((TextBox)e.Item.FindControl("edtxtSubsidyAmt")).Text = subsidyUpto.ToString();
                            ((TextBox)e.Item.FindControl("edtxtSubsidyAmt1")).Text = subsidyUpto1.ToString();
                            //}
                        }
                    }
                    // Comment By : Mrunal .. Due to prevent Double Postback
                    txtHeadDiscount_TextChanged(null, null);

                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showErrorPopup('" + strErr + "');", true);
                    ((TextBox)e.Item.FindControl("txtProductName")).Focus();
                }

            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];
                    // --------------------------------- Delete Record
                    string iname = ((HiddenField)e.Item.FindControl("edProductName")).Value;
                    string icode = ((HiddenField)e.Item.FindControl("edProductID")).Value;

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (dr["ProductName"].ToString() == iname)
                        {
                            dtDetail.Rows.Remove(dr);
                            break;
                        }
                    }
                    rptQuotationDetail.DataSource = dtDetail;
                    rptQuotationDetail.DataBind();

                    Session.Add("dtDetail", dtDetail);

                    string ProductID = ((HiddenField)e.Item.FindControl("edProductID")).Value;
                    Repeater myControl = ((Repeater)e.Item.FindControl("rptSubsidyDetail"));

                    DataTable dtSubsidy = new DataTable();
                    dtSubsidy = (DataTable)Session["dtSubsidy"];

                    foreach (DataRow dr in dtSubsidy.Rows)
                    {
                        if (dr["ProductID"].ToString() == iname)
                            dtSubsidy.Rows.Remove(dr);
                    }
                    dtSubsidy.AcceptChanges();
                    funCalculateTotal();
                    // ---------------------------------
                    int ReturnCode22 = 0;
                    string ReturnMsg22 = "";
                    DataTable dtSpecs = new DataTable();
                    dtSpecs = (DataTable)Session["dtSpecs"];
                    DataRow[] drr;
                    if (dtSpecs != null)
                    {
                        if (!String.IsNullOrEmpty(txtQuotationNo.Text))
                            drr = dtSpecs.Select("QuotationNo = '" + txtQuotationNo.Text + "' AND FinishProductID = " + icode);
                        else
                            drr = dtSpecs.Select("FinishProductID = " + icode);
                        foreach (var row in drr)
                            row.Delete();
                        dtSpecs.AcceptChanges();
                        Session.Add("dtSpecs", dtSpecs);
                        BAL.QuotationDetailMgmt.DeleteQuotationSpecByProduct(txtQuotationNo.Text, Convert.ToInt64(icode), out ReturnCode22, out ReturnMsg22);
                    }
                }
                else if (e.CommandName.ToString() == "Subsidy")
                {
                    foreach (RepeaterItem item in rptQuotationDetail.Items)
                    {
                        Repeater tmpControl = ((Repeater)item.FindControl("rptSubsidyDetail"));
                        tmpControl.DataSource = null;
                        tmpControl.DataBind();
                        tmpControl.Visible = false;
                    }
                    // -------------------------------------------------------------------------
                    string ProductID = ((HiddenField)e.Item.FindControl("edProductID")).Value; 
                    Repeater myControl = ((Repeater)e.Item.FindControl("rptSubsidyDetail"));
                    DataTable dtsummary = new DataTable();
                    dtsummary = FunSubsidySummary(Convert.ToInt64(ProductID));
                    if (dtsummary.Rows.Count > 0)
                    {
                        myControl.Visible = true;
                        myControl.DataSource = dtsummary;
                        myControl.DataBind();
                    }
                }
            }
        }

        public DataTable FunSubsidySummary(Int64 ProductID)
        {
            DataTable dtSummary = new DataTable();

            List <Entity.QuotationSubsidySummary > lstSummary = new List<Entity.QuotationSubsidySummary>();
            dtSummary = PageBase.ConvertListToDataTable(lstSummary);

            DataTable dtSubsidy = new DataTable();
            dtSubsidy = (DataTable)(Session["dtSubsidy"]);

            DataTable dtTemp = new DataTable();

            if (dtSubsidy != null)
            {
                try
                {
                    dtTemp = dtSubsidy.AsEnumerable().Where(x => x.Field<Int64>("ProductID") == ProductID).CopyToDataTable();
                    dtSummary = dtSummary.AsEnumerable().Where(x => x.Field<Int64>("ProductID") != ProductID).CopyToDataTable();
                }
                catch (Exception Ex)
                {
                    dtSummary.Rows.Clear();
                }

                if(dtTemp != null)
                { 
                    if(dtTemp.Rows.Count > 0)
                    {
                        var subsidyUpto = dtTemp.AsEnumerable().Where(x => x.Field<string>("Type") == "upto").CopyToDataTable();
                        var subsidyRemaining = dtTemp.AsEnumerable().Where(x => x.Field<string>("Type") == "remaining").CopyToDataTable();

                        DataRow Dr = dtSummary.NewRow();
                        Dr["ProductID"] = ProductID;
                        //Dr["Quantity"] = subsidyUpto.Rows[0]["Quantity"];
                        //Dr["SubsidyPer"] = subsidyUpto.Rows[0]["SubsidyPer"];
                        Dr["Quantity"] = subsidyUpto.Rows[0]["SlabQty"];
                        Dr["SubsidyPer"] = subsidyUpto.Rows[0]["SlabPer"];
                        Dr["SubsidyAmt"] = subsidyUpto.Rows[0]["SubsidyAmt"];
                        Dr["QuantityRem"] = subsidyRemaining.Rows[0]["Quantity"];
                        //Dr["SubsidyPer1"] = subsidyRemaining.Rows[0]["SubsidyPer"];
                        Dr["SubsidyPer1"] = subsidyRemaining.Rows[0]["SlabPer"];
                        Dr["SubsidyAmt1"] = subsidyRemaining.Rows[0]["SubsidyAmt"];
                        Dr["TotalSubsidyAmt"] = Convert.ToDecimal(subsidyUpto.Rows[0]["SubsidyAmt"]) + Convert.ToDecimal(subsidyRemaining.Rows[0]["SubsidyAmt"]);
                        dtSummary.Rows.Add(Dr);
                    }
                }
            }

            return dtSummary;
        }

        protected void rptQuotationDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
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
                //Label lblAddTaxAmt = (Label)e.Item.FindControl("lblAddTaxAmt");
                Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");

                lblTotalDiscAmt.Text = totDiscAmt.ToString("0.00");
                lblTotalAmt.Text = totAmount.ToString("0.00");
                lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
                //lblAddTaxAmt.Text = totAddTaxAmt.ToString("0.00");
                lblTotalNetAmount.Text = totNetAmount.ToString("0.00");


                funCalculateTotal();
                // ------------------------------------------------
                if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")        // ShaktiPet    
                {
                    TextBox txt1 = (TextBox)e.Item.FindControl("txtQuantity");
                    txt1.Enabled = false;
                }
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

        public List<Entity.Bundle> BindBundleList()
        {
            // ---------------- Assign Employee ------------------------
            List<Entity.Bundle> lstEmployee = new List<Entity.Bundle>();
            lstEmployee = BAL.BundleMgmt.GetBundleList();
            return lstEmployee;
        }

        // ----------------------------------------------------------------------------------
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
                // ----------------------------------------------------
                lstEntity = BAL.QuotationMgmt.GetQuotationList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtQuotationNo.Text = lstEntity[0].QuotationNo;
                txtQuotationDate.Text = lstEntity[0].QuotationDate.ToString("yyyy-MM-dd");
                drpProjects.SelectedValue = lstEntity[0].ProjectName.ToString();
                drpBankID.SelectedValue = lstEntity[0].BankId.ToString();
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                // Comment By : Mrunal .. Due to prevent Double Postback
                txtCustomerName_TextChanged(null, null);
                BindCustomerContacts();
                // ------------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                {
                    drpQuotationKindAttn.Items.Clear();
                    List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
                    lstObject = BAL.CustomerContactsMgmt.GetCustomerContactsList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                    drpQuotationKindAttn.DataSource = lstObject;
                    drpQuotationKindAttn.DataValueField = "ContactPerson1";
                    drpQuotationKindAttn.DataTextField = "ContactPerson1";
                    drpQuotationKindAttn.DataBind();
                    drpQuotationKindAttn.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Contact --", ""));
                    if (!String.IsNullOrEmpty(lstEntity[0].QuotationKindAttn.ToString()))
                        drpQuotationKindAttn.SelectedValue = lstEntity[0].QuotationKindAttn.ToString();
                }
                // ------------------------------------------------------------
                txtQuotationSubject.Text = lstEntity[0].QuotationSubject;
                drpQuotationKindAttn.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].QuotationKindAttn)) ? lstEntity[0].QuotationKindAttn : "";
                drpInquiry.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].InquiryNo)) ? lstEntity[0].InquiryNo : "";
                txtQuotationHeader.Text = lstEntity[0].QuotationHeader;
                txtQuotationFooter.Text = lstEntity[0].QuotationFooter;

                txtAssumptionRemark.Text = lstEntity[0].AssumptionRemark;
                txtAdditionalRemark.Text = lstEntity[0].AdditionalRemark;

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

                if (hdnQuotationCurrency.Value.ToLower() == "yes")
                {
                    drpCurrency.SelectedValue = lstEntity[0].CurrencySymbol;
                    txtExchangeRate.Text = lstEntity[0].ExchangeRate.ToString();
                }
                // -------------------------------------------------------------------------
                BindQuotationDetailList(lstEntity[0].QuotationNo);
                txtCustomerName.Focus();
            }
        }

        protected void btnQSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false, sender);
        }

        protected void btnQSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true, sender);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            // ---------------------------------------------------
            // Resetting Temporary Table for Products
            // ---------------------------------------------------
            Session.Remove("dtDetail");
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.QuotationDetailMgmt.GetQuotationDetail("-1");
            Session.Add("dtDetail", dtDetail1);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail, object sender)
        {

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            // --------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnQuotationNo = "";
            Int64 ReturnFollowupPKID = 0;
            Int64 ppkID = 0;
            // --------------------------------------------------------------
            _pageValid = true;
            string strError = "";
            if (String.IsNullOrEmpty(txtQuotationDate.Text) || drpBankID.SelectedIndex == 0 || (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0"))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    strError += "<li>" + "Select Proper Customer From List." + "</li>";
                txtCustomerName.Focus();


                if (String.IsNullOrEmpty(txtQuotationDate.Text))
                    strError += "<li>" + "Quotation Date is required." + "</li>";
                txtQuotationDate.Focus();


                if (drpBankID.SelectedIndex == 0)
                    strError += "<li>" + "Bank Selection is required." + "</li>";
                drpBankID.Focus();

            }

            if (divFollowUp.Visible == true)
            {
                Boolean cc = false;

                String val1 = txtNextFollowupDate.Text.Trim();
                String val2 = txtMeetingNotes.Text.Trim();
                String val3 = drpFollowupType.SelectedValue;
                if (val1 == "" && val2 == "" && val3 == "")
                    cc = true;
                else if (val1 != "" && val2 != "" && val3 != "")
                    cc = true;
                else
                    cc = false;

                if (cc == false)
                {
                    _pageValid = false;
                    strError += "<li>" + "All Information required to Auto Generate Followup" + "</li>";
                }

            }
            // ------------------------------------------------------------------
            if (hdnPriceRangeValidation.Value.ToLower() == "yes")
            {
                if (Session["RoleCode"].ToString().ToLower() != "admin" && Session["RoleCode"].ToString().ToLower() != "bradmin")
                {
                    if (dtDetail != null)
                    {
                        if (dtDetail.Rows.Count > 0)
                        {
                            List<Entity.Products> lstProduct1 = new List<Entity.Products>();
                            lstProduct1 = BAL.ProductMgmt.GetProductList();
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                Int64 tmpID = Convert.ToInt64(dr["ProductID"]);
                                Decimal tmpRate = Convert.ToInt64(dr["UnitRate"]);
                                if (lstProduct1.Where(x => (x.pkID == tmpID && (tmpRate < x.Min_UnitPrice || tmpRate > x.Max_UnitPrice))).ToList().Count() > 0)
                                {
                                    _pageValid = false;
                                    strError += "<li>" + "You are giving Price outside define range" + "</li>";
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Quotation objEntity = new Entity.Quotation();
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                        objEntity.InquiryNo = drpInquiry.SelectedValue;
                        objEntity.QuotationNo = txtQuotationNo.Text;
                        objEntity.QuotationDate = Convert.ToDateTime(txtQuotationDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.ProjectName = drpProjects.SelectedValue;
                        objEntity.BankId = Convert.ToInt64(drpBankID.SelectedValue);
                        objEntity.QuotationSubject = txtQuotationSubject.Text;
                        objEntity.QuotationKindAttn = drpQuotationKindAttn.SelectedValue;
                        objEntity.QuotationHeader = txtQuotationHeader.Text;
                        objEntity.QuotationFooter = txtQuotationFooter.Text;

                        objEntity.AssumptionRemark = txtAssumptionRemark.Text;
                        objEntity.AdditionalRemark = txtAdditionalRemark.Text;

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


                        objEntity.CurrencyName = (hdnQuotationCurrency.Value.ToLower() == "yes") ? ((!String.IsNullOrEmpty(drpCurrency.SelectedItem.Value)) ? drpCurrency.SelectedItem.Text : "") : "";
                        objEntity.CurrencySymbol = (hdnQuotationCurrency.Value.ToLower() == "yes") ? ((!String.IsNullOrEmpty(drpCurrency.SelectedItem.Value)) ? drpCurrency.SelectedItem.Value : "") : "";
                        objEntity.ExchangeRate = (hdnQuotationCurrency.Value.ToLower() == "yes") ? ((!String.IsNullOrEmpty(txtExchangeRate.Text)) ? Convert.ToDecimal(txtExchangeRate.Text) : 0) : 0;


                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.QuotationMgmt.AddUpdateQuotation(objEntity, out ReturnCode, out ReturnMsg, out ReturnQuotationNo);

                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnQuotationNo) && !String.IsNullOrEmpty(txtQuotationNo.Text))
                        {
                            ReturnQuotationNo = txtQuotationNo.Text;
                        }
                        strError += "<li>" + ((ReturnCode > 0) ? ReturnQuotationNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                        // --------------------------------------------------------------
                        #region >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnQuotationNo))
                        {
                            try
                            {
                                string notificationMsg = "";
                                if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
                                {
                                    ppkID = Convert.ToInt64(hdnpkID.Value);
                                    notificationMsg = "Quotation " + ReturnQuotationNo + " Updated For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                }
                                else
                                {
                                    ppkID = BAL.CommonMgmt.GetQuotationNoPrimaryID(ReturnQuotationNo);
                                    notificationMsg = "Quotation " + ReturnQuotationNo + " Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                }

                                BAL.CommonMgmt.SendNotification_Firebase("Quotation", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                BAL.CommonMgmt.SendNotificationToDB("Quotation", ppkID, notificationMsg, Session["LoginUserID"].ToString(), 0);
                            }
                            catch (Exception ex)
                            { }

                            txtQuotationNo.Text = ReturnQuotationNo;
                            btnQSave.Disabled = true;
                            btnQSaveEmail.Disabled = true;
                            btnQSaveEmail1.Disabled = true;
                            btnQSaveEmail2.Disabled = true;
                            // ------------------------------------------------------------------
                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            BAL.QuotationDetailMgmt.DeleteQuotationDetailByQuotationNo(ReturnQuotationNo, out ReturnCode1, out ReturnMsg1);
                            List<Entity.Products> ProdSpec = new List<Entity.Products>();
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                Entity.QuotationDetail objQuotDet = new Entity.QuotationDetail();

                                objQuotDet.pkID = 0;
                                objQuotDet.QuotationNo = ReturnQuotationNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);

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
                                objQuotDet.TaxType = Convert.ToInt32(dr["TaxType"]);
                                objQuotDet.UnitQty = !String.IsNullOrEmpty(dr["UnitQty"].ToString()) ? Convert.ToDecimal(dr["UnitQty"]) : Convert.ToDecimal(1);
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
                                objQuotDet.HeaderDiscAmt = Convert.ToDecimal(dr["HeaderDiscAmt"]);
                                //objQuotDet.SubsidyApplicable = Convert.ToBoolean(dr["SubsidyApplicable"]);

                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.QuotationDetailMgmt.AddUpdateQuotationDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                                Session["ProductShortRemark"] = "";
                            }
                            // --------------------------------------------------------------
                            if (ReturnCode1 > 0)
                                Session.Remove("dtDetail");
                        }

                        #endregion

                        // --------------------------------------------------------------
                        // Adding FollowUp from Quotation
                        // --------------------------------------------------------------
                        if (ReturnCode > 0 && objEntity.pkID != 0)
                        {
                            if (!String.IsNullOrEmpty(txtNextFollowupDate.Text) && !String.IsNullOrEmpty(txtMeetingNotes.Text) && !String.IsNullOrEmpty(drpFollowupType.SelectedValue))
                            {
                                Entity.Followup objFollow = new Entity.Followup();
                                objFollow.FollowupDate = System.DateTime.Now;
                                objFollow.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                                objFollow.InquiryNo = drpInquiry.SelectedValue;
                                objFollow.MeetingNotes = txtMeetingNotes.Text;
                                objFollow.NextFollowupDate = Convert.ToDateTime(txtNextFollowupDate.Text);
                                objFollow.InquiryStatusID = (!String.IsNullOrEmpty(drpFollowupType.SelectedValue)) ? Convert.ToInt64(drpFollowupType.SelectedValue) : Convert.ToInt64("0");
                                objFollow.Rating = 1;
                                objFollow.LoginUserID = Session["LoginUserID"].ToString();
                                BAL.FollowupMgmt.AddUpdateFollowup(objFollow, out ReturnCode1, out ReturnMsg1, out ReturnFollowupPKID);
                                strError += "<li>" + ReturnMsg1 + "</li>";
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
                                        lstObject.QuotationNo = ReturnQuotationNo;
                                        lstObject.FinishProductID = Convert.ToInt64(dr["FinishProductID"].ToString());
                                        lstObject.GroupHead = dr["GroupHead"].ToString();
                                        lstObject.MaterialHead = dr["MaterialHead"].ToString();
                                        lstObject.MaterialSpec = dr["MaterialSpec"].ToString();
                                        lstObject.ItemOrder = dr["ItemOrder"].ToString();
                                        lstObject.MaterialRemarks = dr["MaterialRemarks"].ToString();
                                        lstObject.LoginUserID = Session["LoginUserID"].ToString();
                                        // -------------------------------------------------------------- Insert/Update Record
                                        BAL.ProductMgmt.AddUpdateQuotationProductSpec(lstObject, out ReturnCode91, out ReturnMsg91);
                                    }
                                }
                            }
                            // ---------------------------------------------------
                            // Removing Unwanted Specification
                            // ---------------------------------------------------
                            BAL.ProductMgmt.DeleteUnwantedSpec(ReturnQuotationNo);

                            Session.Remove("dtSpecs");
                            Session.Remove("mySpecs");

                            BAL.QuotationDetailMgmt.DeleteQuotationSubsidyByQuotationNo(ReturnQuotationNo, out ReturnCode1, out ReturnMsg1);

                            DataTable dtSubsidy = new DataTable();
                            dtSubsidy = (DataTable)Session["dtSubsidy"];
                            if (dtSubsidy != null)
                            {
                                foreach (DataRow dr in dtSubsidy.Rows)
                                {
                                    if (dr.RowState.ToString() != "Deleted")
                                    {
                                        Entity.QuotationSubsidy lstSubsidy = new Entity.QuotationSubsidy();
                                        if (!String.IsNullOrEmpty(dr["pkID"].ToString()) && dr["pkID"].ToString() != "0")
                                            lstSubsidy.pkID = Convert.ToInt64(dr["pkID"].ToString());
                                        else
                                            lstSubsidy.pkID = 0;
                                        lstSubsidy.QuotationNo = ReturnQuotationNo;
                                        lstSubsidy.Type = dr["Type"].ToString();
                                        lstSubsidy.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                                        lstSubsidy.Quantity = Convert.ToDecimal(dr["Quantity"].ToString());
                                        lstSubsidy.SlabQty = Convert.ToDecimal(dr["SlabQty"].ToString());
                                        lstSubsidy.SlabPer = Convert.ToDecimal(dr["SlabPer"].ToString());
                                        lstSubsidy.SubsidyPer = Convert.ToDecimal(dr["SubsidyPer"].ToString());
                                        lstSubsidy.SubsidyAmt = Convert.ToDecimal(dr["SubsidyAmt"].ToString());
                                        lstSubsidy.LoginUserID = Session["LoginUserID"].ToString();

                                        BAL.QuotationDetailMgmt.AddUpdateQuotationSubsidy(lstSubsidy, out ReturnCode91, out ReturnMsg91);
                                    }
                                }
                            }

                            BAL.ProductMgmt.DeleteUnwantedSubsidy(ReturnQuotationNo);
                        }
                        // ==============================================================


                        if (ReturnCode > 0)
                        {
                            // Generating Quotation PDF file ...
                            Quotation.GenerateQuotation(ppkID);
                        }

                        // --------------------------------------------------------------
                        if (paraSaveAndEmail)
                        {
                            Entity.Authenticate objAuth = new Entity.Authenticate();
                            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                            String sendEmailFlag = BAL.CommonMgmt.GetConstant("QT-EMAIL", 0, objAuth.CompanyID).ToLower();
                            if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                            {
                                try
                                {
                                    // Sending Email Notification ...
                                    String respVal = "";
                                    if (ppkID > 0)
                                    {
                                        String tmpEmailAddress = "";
                                        tmpEmailAddress = (objEntity.CustomerID > 0) ? BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID) : "";
                                        if (!String.IsNullOrEmpty(tmpEmailAddress) && tmpEmailAddress.ToUpper() != "NULL")
                                            //respVal = BAL.QuotationMgmt.SendQuotationEmail("QUOTATION", Session["LoginUserID"].ToString(), ppkID, "");
                                            respVal = BAL.CommonMgmt.SendEmailNotifcation("QUOTATION", Session["LoginUserID"].ToString(), ppkID, tmpEmailAddress);
                                    }
                                    strError += "<li>" + ((ReturnCode > 0) ? ReturnQuotationNo + " " + ReturnMsg : ReturnMsg) + " and Email Sent Successfully !" + "</li>";
                                }
                                catch (Exception ex)
                                {
                                    strError += "<li>" + ((ReturnCode > 0) ? ReturnQuotationNo + " " + ReturnMsg : ReturnMsg) + " and Sending Email Failed !" + "</li>";
                                }
                            }
                        }
                    }
                    else
                    {
                        strError = "<li>" + "Atleast One Item is required to save Quotation Information !" + "</li>";
                    }
                }
                else
                {
                    strError = "<li>" + "Atleast One Item is required to save Quotation Information !" + "</li>";
                }
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strError))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-danger');", true);
            }
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtQuotationDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtQuotationNo.Text = ""; // BAL.CommonMgmt.GetQuotationNo(txtQuotationDate.Text);
            txtQuotationHeader.Text = "";
            txtQuotationFooter.Text = "";
            drpTNC.SelectedValue = "";
            txtQuotationSubject.Text = "";
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpQuotationKindAttn.SelectedValue = "";
            drpProjects.SelectedValue = "";
            drpBankID.SelectedValue = "";
            drpInquiry.Items.Clear();
            drpInquiry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            txtAdditionalRemark.Text = "";
            txtAssumptionRemark.Text = "";

            txtNextFollowupDate.Text = String.Empty;
            txtMeetingNotes.Text = "";
            drpFollowupType.SelectedValue = "";

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
            txtSubsidy.Text = "0";

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

            if (hdnQuotationCurrency.Value.ToLower() == "yes")
            {
                drpCurrency.Visible = true;
                txtExchangeRate.Visible = true;
                drpCurrency.SelectedItem.Text = "";
                txtExchangeRate.Text = "";
            }
            else
            {
                drpCurrency.Visible = false;
                txtExchangeRate.Visible = false;
            }

            BindQuotationDetailList("");
            txtCustomerName.Focus();
            btnQSave.Disabled = false;
            btnQSaveEmail.Disabled = false;
            btnQSaveEmail1.Disabled = false;
            btnQSaveEmail2.Disabled = false;
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;

            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

                HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));

                TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
                TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
                TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
                HiddenField hdnOrgUnitRate = ((HiddenField)rptFootCtrl.FindControl("hdnOrgUnitRate"));
                TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
                TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
                TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
                //TextBox txtAddTaxPer = ((TextBox)rptFootCtrl.FindControl("txtAddTaxPer"));
                //TextBox txtAddTaxAmt = ((TextBox)rptFootCtrl.FindControl("txtAddTaxAmt"));
                HiddenField hdnTaxType = ((HiddenField)rptFootCtrl.FindControl("hdnTaxType"));
                HiddenField hdnBox_SQFT = ((HiddenField)rptFootCtrl.FindControl("hdnBox_SQFT"));
                HiddenField hdnBox_SQMT = ((HiddenField)rptFootCtrl.FindControl("hdnBox_SQMT"));

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
                    txtUnit.Text = lstEntity[0].Unit;
                    // ----------------------------------------------------------
                    // Fetching UnitPrice from ... PriceList 
                    // ----------------------------------------------------------
                    Decimal plUnitRate = 0, plDiscount = 0;
                    hdnCustomerID.Value = (String.IsNullOrEmpty(hdnCustomerID.Value)) ? "0" : hdnCustomerID.Value;
                    hdnProductID.Value = (String.IsNullOrEmpty(hdnProductID.Value)) ? "0" : hdnProductID.Value;
                    BAL.CommonMgmt.GetProductPriceListRate(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), out plUnitRate, out plDiscount);

                    hdnOrgUnitRate.Value = plUnitRate.ToString();
                    txtUnitRate.Text = plUnitRate.ToString();

                    if (hdnApplicationIndustry.Value.ToLower() == "ceramic")
                    {
                        txtUnitRate.Text = (lstEntity.Count > 0) ? calcCeremicRate(lstEntity[0].Unit, lstEntity[0].Box_SQFT, plUnitRate).ToString() : "0";
                        hdnOrgUnitRate.Value = txtUnitRate.Text;
                    }

                    hdnBox_SQFT.Value = (lstEntity.Count > 0) ? lstEntity[0].Box_SQFT.ToString() : "0";
                    hdnBox_SQMT.Value = (lstEntity.Count > 0) ? lstEntity[0].Box_SQMT.ToString() : "0";
                    // ------------------------------------------------------------------
                    hdnUnitQuantity.Value = (lstEntity.Count > 0) ? ((!String.IsNullOrEmpty(lstEntity[0].UnitQuantity.ToString())) ? lstEntity[0].UnitQuantity.ToString() : "1") : "1";
                    hdnProductUnitQty.Value = (lstEntity.Count > 0) ? ((!String.IsNullOrEmpty(lstEntity[0].UnitQuantity.ToString())) ? lstEntity[0].UnitQuantity.ToString() : "1") : "1";
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
                    // -----------------------------------------------------
                    // Fetching Product Specificaiton for New Added Item
                    // -----------------------------------------------------
                    BindProductSpecs(hdnProductID.Value);
                }
                editItem_TextChanged1();
                txtUnit.Focus();
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

        public void BindProductSpecs(string pFinishProductID)
        {
            DataTable dtSpecs = new DataTable();
            List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
            if (Session["dtSpecs"] != null)
            {
                dtSpecs = (DataTable)Session["dtSpecs"];
                DataRow[] drr = dtSpecs.Select("QuotationNo = '" + txtQuotationNo.Text + "' AND FinishProductID = " + pFinishProductID);
                foreach (var row in drr)
                    row.Delete();
                dtSpecs.AcceptChanges();
            }

            if (!String.IsNullOrEmpty(pFinishProductID) || pFinishProductID != "0")
            {
                lst = BAL.ProductMgmt.GetQuotationProductSpecList(txtQuotationNo.Text, Convert.ToInt64(pFinishProductID), Session["LoginUserID"].ToString());
                // -----------------------------------------
                DataTable dtSpecsNew = new DataTable();
                dtSpecsNew = PageBase.ConvertListToDataTable(lst);
                if (Session["dtSpecs"] != null)
                {
                    foreach (DataRow row in dtSpecsNew.Rows)
                    {
                        DataRow dr = dtSpecs.NewRow();
                        dr["QuotationNo"] = txtQuotationNo.Text;
                        dr["FinishProductID"] = Convert.ToInt64(pFinishProductID);
                        dr["GroupHead"] = row["GroupHead"].ToString();
                        dr["MaterialHead"] = row["MaterialHead"].ToString();
                        dr["MaterialSpec"] = row["MaterialSpec"].ToString();
                        dr["MaterialRemarks"] = row["MaterialRemarks"].ToString();
                        dr["ItemOrder"] = row["ItemOrder"].ToString();
                        dtSpecs.Rows.Add(dr);
                    }
                    dtSpecs.AcceptChanges();
                    Session["dtSpecs"] = dtSpecs;
                }
                if (Session["dtSpecs"] == null)
                {
                    Session["dtSpecs"] = dtSpecsNew;
                }
            }
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

            List<Entity.Products> ProdSpec = new List<Entity.Products>();
            if (!String.IsNullOrEmpty(pFinishProductID) || pFinishProductID != "0")
            {
                ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList("", Convert.ToInt64(pFinishProductID), Session["LoginUserID"].ToString());
                if (Session["mySpecs"] == null)
                {
                    Session["mySpecs"] = PageBase.ConvertListToDataTable(ProdSpec);
                }
            }
        }
        public decimal calcCeremicRate(string pUnit, decimal pBoxSQFT, decimal pUnitPrice)
        {
            Decimal tmpUnitPrice = pUnitPrice;
            if (pUnit.ToLower() == "sqft" && pBoxSQFT > 0 && pUnitPrice > 0)
            {
                tmpUnitPrice = (tmpUnitPrice / ((String.IsNullOrEmpty(pBoxSQFT.ToString()) || pBoxSQFT.ToString() == "0") ? Convert.ToDecimal(1) : Convert.ToDecimal(pBoxSQFT)));
            }
            return Math.Round(tmpUnitPrice, 2);
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

                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && String.IsNullOrEmpty(txtCustomerName.Text))       //For quotation generation from inquiry no - dashboard
                    txtCustomerName.Text = (lstEntity.Count > 0) ? lstEntity[0].CustomerName : "";

                hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";
                DataTable dtDetail = new DataTable();
                dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
                rptQuotationDetail.DataSource = dtDetail;
                rptQuotationDetail.DataBind();

                Session.Add("dtDetail", dtDetail);
                ////////////////////////////////////////////////////////

                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    BindInquiryList(Convert.ToInt64(hdnCustomerID.Value));
                else
                    BindInquiryList(0);
                // ---------------------------------------------
                BindCustomerContacts();
                // ---------------------------------------------
                drpInquiry.Focus();
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
                    Int64 uq = (!String.IsNullOrEmpty(row["UnitQuantity"].ToString())) ? Convert.ToInt64(row["UnitQuantity"]) : 0;
                    Decimal q = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                    Decimal unitqty = (!String.IsNullOrEmpty(row["UnitQty"].ToString())) ? Convert.ToDecimal(row["UnitQty"]) : 0;
                    Decimal ur = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                    Decimal dp = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                    Decimal dpa = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                    Decimal tr = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                    Decimal at = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;
                    Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);

                    HeaderDiscItemWise = (TotalAmt > 0) ? Math.Round((HeaderDiscAmt * a) / TotalAmt, 2) : 0;

                    decimal TaxAmt = 0;
                    decimal CGSTPer = 0, CGSTAmt = 0;
                    decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
                    if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")        // ShaktiPet    
                    {
                        BAL.CommonMgmt.funCalculateSteel(uq, taxtype, unitqty, ur, dp, dpa, tr, at, HeaderDiscItemWise, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
                    }
                    else
                    {
                        BAL.CommonMgmt.funCalculateSteel(uq, taxtype, q, ur, dp, dpa, tr, at, HeaderDiscItemWise, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
                    }

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
                    row.SetField("NetAmount", NetAmt);

                }

                rptQuotationDetail.DataSource = dtDetail;
                rptQuotationDetail.DataBind();
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
                    txtQuotationFooter.Text = lstList2[0].TNC_Content;
            }
            else if (String.IsNullOrEmpty(drpTNC.SelectedValue))
            {
                txtQuotationFooter.Text = "";
            }
            txtQuotationFooter.Focus();

        }

        protected void editItemSubsidy_TextChanged(object sender, EventArgs e)
        {
            if (sender.ToString() == "System.Web.UI.WebControls.TextBox")
            {
                TextBox edSender = (TextBox)sender;
                var item = (RepeaterItem)edSender.NamingContainer;

                HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
                TextBox edtxtSubsidyQty = ((TextBox)item.FindControl("txtSubsidyQty"));
                TextBox edtxtSubsidyPer = ((TextBox)item.FindControl("txtSubsidyPer"));
                TextBox edtxtSubsidyAmt = ((TextBox)item.FindControl("txtSubsidyAmt"));
                TextBox edtxtSubsidyPer1 = ((TextBox)item.FindControl("txtSubsidyPer1"));
                TextBox edtxtSubsidyAmt1 = ((TextBox)item.FindControl("txtSubsidyAmt1"));
                TextBox edtxtTotalSubsidy = ((TextBox)item.FindControl("txtTotalSubsidy"));

                Decimal QuantityUpto = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyQty.Text) ? "0" : edtxtSubsidyQty.Text);
                Decimal SubsidyPer = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer.Text) ? "0" : edtxtSubsidyPer.Text);
                Decimal SubsidyPer1 = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer1.Text) ? "0" : edtxtSubsidyPer1.Text);

                if (edProductID.Value != "")
                {
                    Decimal subsidyUpto = 0, subsidyUpto1 = 0;
                    funSetSubsidy(Convert.ToInt64(edProductID.Value), QuantityUpto, SubsidyPer, SubsidyPer1, out subsidyUpto, out subsidyUpto1);

                    edtxtSubsidyAmt.Text = subsidyUpto.ToString();
                    edtxtSubsidyAmt1.Text = subsidyUpto1.ToString();
                    edtxtTotalSubsidy.Text = (subsidyUpto + subsidyUpto1).ToString();
                }
            }
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            if (sender.ToString() == "System.Web.UI.WebControls.DropDownList")
            {
                DropDownList edSender = (DropDownList)sender;

                var item = (RepeaterItem)edSender.NamingContainer;
                HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];

                foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                rptQuotationDetail.DataSource = dtDetail;
                rptQuotationDetail.DataBind();

                Session.Add("dtDetail", dtDetail);
            }
            else if (sender.ToString() == "System.Web.UI.WebControls.TextBox")
            {
                TextBox edSender = (TextBox)sender;
                var item = (RepeaterItem)edSender.NamingContainer;

                HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
                HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
                HiddenField edUnitQuantity = (HiddenField)item.FindControl("edUnitQuantity");
                HiddenField edTaxType = (HiddenField)item.FindControl("edTaxType");
                HiddenField edBox_SQFT = (HiddenField)item.FindControl("edBox_SQFT");
                HiddenField edBox_SQMT = (HiddenField)item.FindControl("edBox_SQMT");

                TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
                TextBox edUnitQty = (TextBox)item.FindControl("edUnitQty");
                TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
                TextBox edUnit = (TextBox)item.FindControl("edUnit");

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
                Decimal uq = 1, txtUQ = 0;
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
                //Decimal at = (!String.IsNullOrEmpty(edAddTaxPer.Text)) ? Convert.ToDecimal(edAddTaxPer.Text) : 0;
                //Decimal ata = (!String.IsNullOrEmpty(edAddTaxAmt.Text)) ? Convert.ToDecimal(edAddTaxAmt.Text) : 0;
                Decimal na = (!String.IsNullOrEmpty(edNetAmount.Text)) ? Convert.ToDecimal(edNetAmount.Text) : 0;
                Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(edTaxType.Value)) ? Convert.ToInt16(edTaxType.Value) : 0);
                // --------------------------------------------------------------------------
                ur = calcCeremicRate(edUnit.Text, Convert.ToDecimal(edBox_SQFT.Value), ur);
                // --------------------------------------------------------------------------
                decimal TaxAmt = 0;
                decimal CGSTPer = 0, CGSTAmt = 0;
                decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;

                BAL.CommonMgmt.funCalculateSteel(uq, taxtype, q, ur, dp, dpa, tr, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);

                edDiscountPercent.Text = ItmDiscPer1.ToString();
                edDiscountAmt.Text = ItmDiscAmt1.ToString();
                //edAddTaxAmt.Text = AddTaxAmt.ToString();
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
                        row.SetField("UnitQuantity", (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? edUnitQuantity.Value : "1");
                        row.SetField("UnitQty", (!String.IsNullOrEmpty(edUnitQty.Text)) ? edUnitQty.Text : "1");
                        if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")    // ShaktiPet
                        {
                            hdnProductUnitQty.Value = (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? edUnitQuantity.Value : "1";
                            row.SetField("Quantity", (Convert.ToDecimal(edUnitQty.Text)) * (Convert.ToDecimal(edUnitQuantity.Value)));
                        }
                        else
                        {
                            row.SetField("Quantity", edQuantity.Text);
                        }
                        //row.SetField("Quantity", edQuantity.Text);
                        row.SetField("Unit", edUnit.Text);
                        row.SetField("Qty", edQuantity.Text);
                        row.SetField("TaxType", edTaxType.Value);
                        row.SetField("UnitRate", edUnitRate.Text);
                        row.SetField("UnitPrice", edUnitRate.Text);
                        row.SetField("Rate", edUnitRate.Text);
                        row.SetField("DiscountPercent", edDiscountPercent.Text);
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
                        row.SetField("NetAmount", edNetAmount.Text);
                        row.SetField("NetAmt", edNetAmount.Text);

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
                            BAL.CommonMgmt.funCalculate(taxtype1, q1, ur1, dp1, dpa1, tr1, at1, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);

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
                rptQuotationDetail.DataSource = dtDetail;
                rptQuotationDetail.DataBind();

                Session.Add("dtDetail", dtDetail);

                Repeater myControl = ((Repeater)item.FindControl("rptSubsidyDetail"));

                foreach (RepeaterItem items in myControl.Items)
                {
                    if (items.ItemType == ListItemType.Item || items.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox edtxtSubsidyQty = ((TextBox)items.FindControl("txtSubsidyQty"));
                        TextBox edtxtSubsidyPer = ((TextBox)items.FindControl("txtSubsidyPer"));
                        TextBox edtxtSubsidyAmt = ((TextBox)items.FindControl("txtSubsidyAmt"));
                        TextBox edtxtSubsidyPer1 = ((TextBox)items.FindControl("txtSubsidyPer1"));
                        TextBox edtxtSubsidyAmt1 = ((TextBox)items.FindControl("txtSubsidyAmt1"));
                        TextBox edtxtTotalSubsidy = ((TextBox)items.FindControl("txtTotalSubsidy"));

                        Decimal QuantityUpto = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyQty.Text) ? "0" : edtxtSubsidyQty.Text);
                        Decimal SubsidyPer = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer.Text) ? "0" : edtxtSubsidyPer.Text);
                        Decimal SubsidyPer1 = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer1.Text) ? "0" : edtxtSubsidyPer1.Text);

                        if (edProductID.Value != "")
                        {
                            Decimal subsidyUpto = 0, subsidyUpto1 = 0;
                            funSetSubsidy(Convert.ToInt64(edProductID.Value), QuantityUpto, SubsidyPer, SubsidyPer1, out subsidyUpto, out subsidyUpto1);

                            edtxtSubsidyAmt.Text = subsidyUpto.ToString();
                            edtxtSubsidyAmt1.Text = subsidyUpto1.ToString();
                            edtxtTotalSubsidy.Text = (subsidyUpto + subsidyUpto1).ToString();
                        }
                    }
                }

 
                //editItemSubsidy_TextChanged(null, null);

                if (Convert.ToDouble(string.IsNullOrEmpty(txtHeadDiscount.Text) ? "0" : txtHeadDiscount.Text) > 0)
                {
                    // Comment By : Mrunal .. Due to prevent Double Postback
                    txtHeadDiscount_TextChanged(null, null);
                }
            }
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            txtUnitRate.Focus();
        }
        protected void txtUnit_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            HiddenField hdnOrgUnitRate = ((HiddenField)rptFootCtrl.FindControl("hdnOrgUnitRate"));

            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));

            txtUnitRate.Text = hdnOrgUnitRate.Value;
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            HiddenField hdnBox_SQFT = ((HiddenField)rptFootCtrl.FindControl("hdnBox_SQFT"));
            HiddenField hdnBox_SQMT = ((HiddenField)rptFootCtrl.FindControl("hdnBox_SQMT"));

            if (!String.IsNullOrEmpty(hdnProductID.Value) && hdnProductID.Value != "0")
            {
                txtUnitRate.Text = calcCeremicRate(txtUnit.Text, Convert.ToDecimal(hdnBox_SQFT.Value), Convert.ToDecimal(txtUnitRate.Text)).ToString();
                editItem_TextChanged1();
            }
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            txtQuantity.Focus();
        }
        protected void txtUnitRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            txtDiscountPercent.Focus();
        }
        protected void txtDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
            txtTaxRate.Focus();

        }
        protected void txtTaxRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            ImageButton imgBtnSave = ((ImageButton)rptFootCtrl.FindControl("imgBtnSave"));
            imgBtnSave.Focus();
            imgBtnSave.BackColor = System.Drawing.Color.Red;
        }
        protected void editItem_TextChanged1()
        {
            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));
            HiddenField hdnBox_SQFT = ((HiddenField)rptFootCtrl.FindControl("hdnBox_SQFT"));
            HiddenField hdnBox_SQMT = ((HiddenField)rptFootCtrl.FindControl("hdnBox_SQMT"));

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
            //Decimal at = String.IsNullOrEmpty(txtAddTaxPer.Text) ? 0 : Convert.ToDecimal(txtAddTaxPer.Text);
            //Decimal ata = String.IsNullOrEmpty(txtAddTaxAmt.Text) ? 0 : Convert.ToDecimal(txtAddTaxAmt.Text);
            Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            Int16 taxtype = Convert.ToInt16(String.IsNullOrEmpty(hdnTaxType.Value) ? 0 : Convert.ToInt16(hdnTaxType.Value));

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculateSteel(uq, taxtype, q, ur, dp, dpa, tr, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);

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
            funSetSubsidy();
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
            drpOthChrg2.Focus();
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
            drpOthChrg3.Focus();
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
            drpOthChrg4.Focus();
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
            drpOthChrg5.Focus();
        }

        protected void drpQuotationCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int TotalRecord = 0;
            List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
            // -------------------------------------------------------------------------
            lstEntity = BAL.EmailTemplateMgmt.GetGeneralTemplate(Convert.ToInt64(drpQuotationCategory.SelectedValue), Session["LoginUserID"].ToString(), 1, Convert.ToInt32(Session["PageSize"]), out TotalRecord);
            if (lstEntity.Count > 0)
            {
                txtQuotationSubject.Text = lstEntity[0].Subject;
                txtQuotationHeader.Text = lstEntity[0].ContentData;
            }
        }

        protected void txtUnitQty_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
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
            txtOthChrgAmt5.Focus();
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
            NetAmt = (Convert.ToDecimal(ViewState["totNetAmount"]) + (Convert.ToDecimal(hdnOthChrgGST1.Value)) - (((!String.IsNullOrEmpty(txtHeadDiscount.Text)) ? Convert.ToDecimal(txtHeadDiscount.Text) : 0))
                + (Convert.ToDecimal(hdnOthChrgGST2.Value)) + (Convert.ToDecimal(hdnOthChrgGST3.Value)) + (Convert.ToDecimal(hdnOthChrgGST4.Value))
                + (Convert.ToDecimal(hdnOthChrgGST5.Value)) + (Convert.ToDecimal(hdnOthChrgBasic1.Value)) + (Convert.ToDecimal(hdnOthChrgBasic2.Value))
                + (Convert.ToDecimal(hdnOthChrgBasic3.Value)) + (Convert.ToDecimal(hdnOthChrgBasic4.Value)) + (Convert.ToDecimal(hdnOthChrgBasic5.Value))
                    );

            txtTotNetAmt.Text = Math.Round(NetAmt, 0).ToString("0.00");
            txtRoff.Text = (Math.Round(NetAmt, 0) - Math.Round(NetAmt, 2)).ToString("0.00");

            //txtSubsidy.Text = (Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyAmt.Text) ? "0" : txtSubsidyAmt.Text) +
            //                Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyAmt1.Text) ? "0" : txtSubsidyAmt1.Text)).ToString();

            DataTable dtSubsidy = new DataTable();
            dtSubsidy = (DataTable)Session["dtSubsidy"];

            if(dtSubsidy != null)
            {
                if(dtSubsidy.Rows.Count > 0)
                {
                    var TotalSubsidy = dtSubsidy.AsEnumerable().Sum(x => x.Field<Decimal>("SubsidyAmt"));
                    txtSubsidy.Text = Convert.ToDecimal(TotalSubsidy).ToString();
                }
            }

            txtNetPayble.Text = (Convert.ToDecimal(string.IsNullOrEmpty(txtTotNetAmt.Text) ? "0" : txtTotNetAmt.Text) -
                            Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidy.Text) ? "0" : txtSubsidy.Text)).ToString();
        }
        protected void drpOthChrg1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt1_TextChanged(sender, e);
            if (drpOthChrg1.SelectedValue == "")
            {
                txtOthChrgAmt1.Text = "0";
            }
            txtOthChrgAmt1.Focus();
        }
        protected void drpOthChrg2_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt2_TextChanged(sender, e);
            if (drpOthChrg2.SelectedValue == "")
            {
                txtOthChrgAmt2.Text = "0";
            }
            txtOthChrgAmt2.Focus();
        }

        protected void txtSubsidyPer1_TextChanged(object sender, EventArgs e)
        {
            funSetSubsidy();
        }

        protected void txtSubsidyPer_TextChanged(object sender, EventArgs e)
        {
            funSetSubsidy();
        }

        public void funSetSubsidy(Int64 ProductID, Decimal QuantityUpto, Decimal SubsidyPer, Decimal SubsidyPer1, out Decimal SubsidyAmt, out Decimal SubsidyAmt1)
        {
            //Decimal QuantityUpto = Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyQty.Text) ? "0" : txtSubsidyQty.Text);
            //Decimal SubsidyPer = Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyPer.Text) ? "0" : txtSubsidyPer.Text);
            //Decimal SubsidyPer1 = Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyPer1.Text) ? "0" : txtSubsidyPer1.Text);

            SubsidyAmt = 0;
            SubsidyAmt1 = 0;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtSubsidy = new DataTable();
            dtSubsidy = (DataTable)Session["dtSubsidy"];
            //dtSubsidy.Rows.Clear();

            try
            {
                dtSubsidy = dtSubsidy.AsEnumerable().Where(x => x.Field<Int64>("ProductID") != ProductID).CopyToDataTable();
            }
            catch (Exception ex)
            {
                dtSubsidy.Rows.Clear();
            }

            if (dtDetail != null && dtSubsidy != null)
            {
                if (dtDetail.Rows.Count > 0)
                {

                    string find = "ProductID = " + ProductID;
                    DataRow[] foundRows = dtDetail.Select(find);
                    foreach (DataRow dr in foundRows)
                    {
                        if (Convert.ToDecimal(dr["Quantity"]) > QuantityUpto)
                        {
                            DataRow drsub = dtSubsidy.NewRow();
                            drsub["Type"] = "upto";
                            drsub["ProductID"] = dr["ProductID"];
                            drsub["Quantity"] = QuantityUpto;
                            drsub["SlabQty"] = QuantityUpto;
                            drsub["SlabPer"] = SubsidyPer;
                            drsub["SubsidyPer"] = SubsidyPer;
                            dtSubsidy.Rows.Add(drsub);
                            dtSubsidy.AcceptChanges();

                            DataRow drsub2 = dtSubsidy.NewRow();
                            drsub2["Type"] = "remaining";
                            drsub2["ProductID"] = dr["ProductID"];
                            drsub2["Quantity"] = (Convert.ToDecimal(dr["Quantity"]) - QuantityUpto);
                            drsub2["SlabQty"] = 0;
                            drsub2["SlabPer"] = SubsidyPer1;
                            drsub2["SubsidyPer"] = SubsidyPer1;
                            dtSubsidy.Rows.Add(drsub2);
                            dtSubsidy.AcceptChanges();
                        }
                        else
                        {
                            DataRow drsub = dtSubsidy.NewRow();
                            drsub["Type"] = "upto";
                            drsub["ProductID"] = dr["ProductID"];
                            drsub["Quantity"] = Convert.ToDecimal(dr["Quantity"]);
                            drsub["SlabQty"] = QuantityUpto;
                            drsub["SlabPer"] = SubsidyPer;
                            drsub["SubsidyPer"] = SubsidyPer;
                            dtSubsidy.Rows.Add(drsub);
                            dtSubsidy.AcceptChanges();

                            DataRow drsub2 = dtSubsidy.NewRow();
                            drsub2["Type"] = "remaining";
                            drsub2["ProductID"] = dr["ProductID"];
                            drsub2["Quantity"] = 0;
                            drsub2["SubsidyPer"] = 0;
                            drsub2["SlabQty"] = 0;
                            drsub2["SlabPer"] = SubsidyPer1;
                            dtSubsidy.Rows.Add(drsub2);
                            dtSubsidy.AcceptChanges();
                        }
                    }

                    dtSubsidy = funCalculateSubsidy(dtSubsidy, dtDetail);
                    Session.Add("dtSubsidy", dtSubsidy);

                    var subsidyUpto = dtSubsidy.AsEnumerable().Where(x => x.Field<string>("Type") == "upto" && x.Field<Int64>("ProductID") == ProductID).Sum(X => X.Field<Decimal>("SubsidyAmt"));
                    SubsidyAmt = subsidyUpto;

                    var subsidyRemaining = dtSubsidy.AsEnumerable().Where(x => x.Field<string>("Type") == "remaining" && x.Field<Int64>("ProductID") == ProductID).Sum(X => X.Field<Decimal>("SubsidyAmt"));
                    SubsidyAmt1 = subsidyRemaining;

                    funCalculateTotal();
                }
            }
        }

        public void funSetSubsidy()
        {

            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));

            if(hdnProductID.Value != "")
            {
                TextBox edtxtSubsidyQty = ((TextBox)rptFootCtrl.FindControl("edtxtSubsidyQty"));
                TextBox edtxtSubsidyPer = ((TextBox)rptFootCtrl.FindControl("edtxtSubsidyPer"));
                TextBox edtxtSubsidyAmt = ((TextBox)rptFootCtrl.FindControl("edtxtSubsidyAmt"));
                TextBox edtxtSubsidyPer1 = ((TextBox)rptFootCtrl.FindControl("edtxtSubsidyPer1"));
                TextBox edtxtSubsidyAmt1 = ((TextBox)rptFootCtrl.FindControl("edtxtSubsidyAmt1"));
                TextBox edtxtTotalSubsidy = ((TextBox)rptFootCtrl.FindControl("edtxtTotalSubsidy"));

                HiddenField edTaxType = (HiddenField)rptFootCtrl.FindControl("hdnTaxType");
                TextBox edNetRate = (TextBox)rptFootCtrl.FindControl("txtNetRate");
                TextBox edTaxRate = (TextBox)rptFootCtrl.FindControl("txtTaxRate");                
                TextBox edQuantity = (TextBox)rptFootCtrl.FindControl("txtQuantity");

                Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
                Decimal Rate = (!String.IsNullOrEmpty(edNetRate.Text)) ? Convert.ToDecimal(edNetRate.Text) : 0;
                Decimal TaxRate = (!String.IsNullOrEmpty(edTaxRate.Text)) ? Convert.ToDecimal(edTaxRate.Text) : 0;
                Int16 TaxType = Convert.ToInt16((!String.IsNullOrEmpty(edTaxType.Value)) ? Convert.ToInt16(edTaxType.Value) : 0);

                Decimal QuantityUpto = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyQty.Text) ? "0" : edtxtSubsidyQty.Text);
                Decimal SubsidyPer = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer.Text) ? "0" : edtxtSubsidyPer.Text);
                Decimal SubsidyPer1 = Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer1.Text) ? "0" : edtxtSubsidyPer1.Text);

                Decimal subsidyUpto = 0, subsidyUpto1 = 0;
                //funSetSubsidy(Convert.ToInt64(hdnProductID.Value), Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyQty.Text) ? "0" : edtxtSubsidyQty.Text), Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer.Text) ? "0" : edtxtSubsidyPer.Text), Convert.ToDecimal(string.IsNullOrEmpty(edtxtSubsidyPer1.Text) ? "0" : edtxtSubsidyPer1.Text), out subsidyUpto, out subsidyUpto1);

                if(q > 0 && Rate > 0)
                {
                    if (Convert.ToDecimal(q) > QuantityUpto)
                    {
                        Decimal qty = 0, BasicAmt = 0, GSTAmt = 0;
                        qty = QuantityUpto;

                        BasicAmt = qty * Rate;

                        if (TaxType == 1)
                        {
                            GSTAmt = Math.Round(((BasicAmt * TaxRate) / 100), 2);
                            BasicAmt = BasicAmt + GSTAmt;
                        }
                        else
                            BasicAmt = BasicAmt;

                        subsidyUpto = Math.Round(((BasicAmt * SubsidyPer) / 100), 2);


                        qty = (q - QuantityUpto);

                        BasicAmt = qty * Rate;

                        if (TaxType == 1)
                        {
                            GSTAmt = Math.Round(((BasicAmt * TaxRate) / 100), 2);
                            BasicAmt = BasicAmt + GSTAmt;
                        }
                        else
                            BasicAmt = BasicAmt;

                        subsidyUpto1 = Math.Round(((BasicAmt * SubsidyPer1) / 100), 2);

                    }
                    else
                    {
                        Decimal qty = 0, BasicAmt = 0, GSTAmt = 0;
                        qty = q;

                        BasicAmt = qty * Rate;

                        if (TaxType == 1)
                        {
                            GSTAmt = Math.Round(((BasicAmt * TaxRate) / 100), 2);
                            BasicAmt = BasicAmt + GSTAmt;
                        }
                        else
                            BasicAmt = BasicAmt;

                        subsidyUpto = Math.Round(((BasicAmt * SubsidyPer) / 100), 2);
                    }

                }

                edtxtSubsidyAmt.Text = subsidyUpto.ToString();
                edtxtSubsidyAmt1.Text = subsidyUpto1.ToString();
                edtxtTotalSubsidy.Text = (subsidyUpto + subsidyUpto1).ToString();
            }
        }


        //public void funSetSubsidy()
        //{
        //    Decimal QuantityUpto = Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyQty.Text) ? "0" : txtSubsidyQty.Text);
        //    Decimal SubsidyPer = Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyPer.Text) ? "0" : txtSubsidyPer.Text);
        //    Decimal SubsidyPer1 = Convert.ToDecimal(string.IsNullOrEmpty(txtSubsidyPer1.Text) ? "0" : txtSubsidyPer1.Text);

        //    DataTable dtDetail = new DataTable();
        //    dtDetail = (DataTable)Session["dtDetail"];

        //    DataTable dtSubsidy = new DataTable();
        //    dtSubsidy = (DataTable)Session["dtSubsidy"];
        //    dtSubsidy.Rows.Clear();

        //    if (dtDetail != null && dtSubsidy != null)
        //    {
        //        if (dtDetail.Rows.Count > 0)
        //        {
        //            if (dtDetail.AsEnumerable().Any(x => x.Field<Boolean>("SubsidyApplicable") == true))
        //            {
        //                dtDetail = dtDetail.AsEnumerable().Where(x => x.Field<Boolean>("SubsidyApplicable") == true).CopyToDataTable();

        //                foreach (DataRow dr in dtDetail.Rows)
        //                {
        //                    if (Convert.ToDecimal(dr["Quantity"]) > QuantityUpto)
        //                    {
        //                        DataRow drsub = dtSubsidy.NewRow();
        //                        drsub["Type"] = "upto";
        //                        drsub["ProductID"] = dr["ProductID"];
        //                        drsub["Quantity"] = QuantityUpto;
        //                        drsub["SubsidyPer"] = SubsidyPer;
        //                        dtSubsidy.Rows.Add(drsub);
        //                        dtSubsidy.AcceptChanges();

        //                        DataRow drsub2 = dtSubsidy.NewRow();
        //                        drsub2["Type"] = "remaining";
        //                        drsub2["ProductID"] = dr["ProductID"];
        //                        drsub2["Quantity"] = (Convert.ToDecimal(dr["Quantity"]) - QuantityUpto);
        //                        drsub2["SubsidyPer"] = SubsidyPer1;
        //                        dtSubsidy.Rows.Add(drsub2);
        //                        dtSubsidy.AcceptChanges();
        //                    }
        //                    else
        //                    {
        //                        DataRow drsub = dtSubsidy.NewRow();
        //                        drsub["Type"] = "upto";
        //                        drsub["ProductID"] = dr["ProductID"];
        //                        drsub["Quantity"] = Convert.ToDecimal(dr["Quantity"]);
        //                        drsub["SubsidyPer"] = SubsidyPer;
        //                        dtSubsidy.Rows.Add(drsub);
        //                        dtSubsidy.AcceptChanges();
        //                    }
        //                }
        //                dtSubsidy = funCalculateSubsidy(dtSubsidy, dtDetail);
        //                Session.Add("dtSubsidy", dtSubsidy);

        //                var subsidyUpto = dtSubsidy.AsEnumerable().Where(x => x.Field<string>("Type") == "upto").Sum(X => X.Field<Decimal>("SubsidyAmt"));
        //                txtSubsidyAmt.Text = subsidyUpto.ToString();

        //                var subsidyRemaining = dtSubsidy.AsEnumerable().Where(x => x.Field<string>("Type") == "remaining").Sum(X => X.Field<Decimal>("SubsidyAmt"));
        //                txtSubsidyAmt1.Text = subsidyRemaining.ToString();

        //                funCalculateTotal();
        //            }
        //        }
        //    }
        //}

        public DataTable funCalculateSubsidy(DataTable dtSubsidy, DataTable dtDetail)
        {
            Decimal qty = 0, Rate = 0, TaxType = 0, TaxRate = 0, BasicAmt = 0, GSTAmt = 0, SubsidyPer = 0, SubsidyAmt = 0;

            foreach (DataRow dr in dtSubsidy.Rows)
            {
                DataRow[] drProductDetail = dtDetail.Select("ProductID = " + Convert.ToInt64(dr["ProductID"]));
                qty = 0; Rate = 0; TaxType = 0; TaxRate = 0; BasicAmt = 0; GSTAmt = 0; SubsidyPer = 0; SubsidyAmt = 0;

                qty = Convert.ToDecimal(dr["Quantity"]);
                Rate = Convert.ToDecimal(drProductDetail[0]["NetRate"]);
                TaxType = Convert.ToDecimal(drProductDetail[0]["TaxType"]);
                TaxRate = Convert.ToDecimal(drProductDetail[0]["TaxRate"]);
                SubsidyPer = Convert.ToDecimal(dr["SubsidyPer"]);

                BasicAmt = qty * Rate;

                if (TaxType == 1)
                {
                    GSTAmt = Math.Round(((BasicAmt * TaxRate) / 100), 2);
                    BasicAmt = BasicAmt + GSTAmt;
                }
                else
                    BasicAmt = BasicAmt;

                SubsidyAmt = Math.Round(((BasicAmt * SubsidyPer) / 100), 2);
                dr["SubsidyAmt"] = SubsidyAmt;
            }

            return dtSubsidy;
        }

        //protected void edchkSubsidy_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox edSender = (CheckBox)sender;
        //    var item = (RepeaterItem)edSender.NamingContainer;

        //    HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
        //    CheckBox chkSubsidy = ((CheckBox)item.FindControl("edchkSubsidy"));
        //    Boolean SubsidyApplicable = chkSubsidy.Checked;

        //    DataTable dtDetail = new DataTable();
        //    dtDetail = (DataTable)Session["dtDetail"];

        //    foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

        //    foreach (DataRow row in dtDetail.Rows)
        //    {
        //        if (row["ProductID"].ToString() == edProductID.Value)
        //        {
        //            row.SetField("SubsidyApplicable", SubsidyApplicable);
        //            row.AcceptChanges();
        //        }
        //        dtDetail.AcceptChanges();
        //        rptQuotationDetail.DataSource = dtDetail;
        //        rptQuotationDetail.DataBind();
        //        Session.Add("dtDetail", dtDetail);
        //    }

        //    funSetSubsidy();
        //}

        protected void drpOthChrg3_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt3_TextChanged(sender, e);
            if (drpOthChrg3.SelectedValue == "")
            {
                txtOthChrgAmt3.Text = "0";
            }
            txtOthChrgAmt3.Focus();
        }
        protected void drpOthChrg4_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt4_TextChanged(sender, e);
            if (drpOthChrg4.SelectedValue == "")
            {
                txtOthChrgAmt4.Text = "0";
            }
            txtOthChrgAmt4.Focus();
        }
        protected void drpOthChrg5_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOthChrgAmt5_TextChanged(sender, e);
            if (drpOthChrg5.SelectedValue == "")
            {
                txtOthChrgAmt5.Text = "0";
            }
            txtOthChrgAmt5.Focus();
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // https://cjhaas.com/2012/01/06/how-to-recompress-images-in-a-pdf-using-itextsharp/
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static void RecompressPDF(string largePDF, string smallPDF)
        {
            //Bind a reader to our large PDF
            PdfReader reader = new PdfReader(largePDF);
            //Create our output PDF
            using (FileStream fs = new FileStream(smallPDF, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //Bind a stamper to the file and our reader
                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {
                    //NOTE: This code only deals with page 1, you'd want to loop more for your code
                    //Get page 1
                    PdfDictionary page = reader.GetPageN(1);
                    //Get the xobject structure
                    PdfDictionary resources = (PdfDictionary)PdfReader.GetPdfObject(page.Get(PdfName.RESOURCES));
                    PdfDictionary xobject = (PdfDictionary)PdfReader.GetPdfObject(resources.Get(PdfName.XOBJECT));
                    if (xobject != null)
                    {
                        PdfObject obj;
                        //Loop through each key
                        foreach (PdfName name in xobject.Keys)
                        {
                            obj = xobject.Get(name);
                            if (obj.IsIndirect())
                            {
                                //Get the current key as a PDF object
                                PdfDictionary imgObject = (PdfDictionary)PdfReader.GetPdfObject(obj);
                                //See if its an image
                                if (imgObject.Get(PdfName.SUBTYPE).Equals(PdfName.IMAGE))
                                {
                                    //NOTE: There's a bunch of different types of filters, I'm only handing the simplest one here which is basically raw JPG, you'll have to research others
                                    if (imgObject.Get(PdfName.FILTER).Equals(PdfName.DCTDECODE))
                                    {
                                        //Get the raw bytes of the current image
                                        byte[] oldBytes = PdfReader.GetStreamBytesRaw((PRStream)imgObject);
                                        //Will hold bytes of the compressed image later
                                        byte[] newBytes;
                                        //Wrap a stream around our original image
                                        using (MemoryStream sourceMS = new MemoryStream(oldBytes))
                                        {
                                            //Convert the bytes into a .Net image
                                            using (System.Drawing.Image oldImage = System.Drawing.Bitmap.FromStream(sourceMS))
                                            {
                                                //Shrink the image to 90% of the original
                                                using (System.Drawing.Image newImage = ShrinkImage(oldImage, 0.9f))
                                                {
                                                    //Convert the image to bytes using JPG at 85%
                                                    newBytes = ConvertImageToBytes(newImage, 85);
                                                }
                                            }
                                        }
                                        //Create a new iTextSharp image from our bytes
                                        iTextSharp.text.Image compressedImage = iTextSharp.text.Image.GetInstance(newBytes);
                                        //Kill off the old image
                                        PdfReader.KillIndirect(obj);
                                        //Add our image in its place
                                        stamper.Writer.AddDirectImageSimple(compressedImage, (PRIndirectReference)obj);
                                    }
                                }
                            }
                        }
                    }
                }

                fs.Close();
                fs.Dispose();
            }
            reader.Close();
        }
        //Standard image save code from MSDN, returns a byte array
        private static byte[] ConvertImageToBytes(System.Drawing.Image image, long compressionLevel)
        {
            if (compressionLevel < 0)
            {
                compressionLevel = 0;
            }
            else if (compressionLevel > 100)
            {
                compressionLevel = 100;
            }
            System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, compressionLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jgpEncoder, myEncoderParameters);
                return ms.ToArray();
            }

        }
        //standard code from MSDN
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        //Standard high quality thumbnail generation from http://weblogs.asp.net/gunnarpeipman/archive/2009/04/02/resizing-images-without-loss-of-quality.aspx
        private static System.Drawing.Image ShrinkImage(System.Drawing.Image sourceImage, float scaleFactor)
        {
            int newWidth = Convert.ToInt32(sourceImage.Width * scaleFactor);
            int newHeight = Convert.ToInt32(sourceImage.Height * scaleFactor);

            var thumbnailBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnailBitmap))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                g.DrawImage(sourceImage, imageRectangle);
            }
            return thumbnailBitmap;
        }

    }
}