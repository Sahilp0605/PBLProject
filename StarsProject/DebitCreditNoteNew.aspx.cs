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
    public partial class DebitCreditNoteNew : System.Web.UI.Page
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
                hdnDBC.Value = Request.QueryString["DBC"].ToString();
                BindDropDown();
                // --------------------------------------------------------
                string manualInvoiceNo = BAL.CommonMgmt.GetConstant("SalesManualInvoiceNo", 0, 1);
                if (manualInvoiceNo == "yes")      // Steelman Gases
                    txtVoucherNo.ReadOnly = false;
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }
        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnInvoiceNo = "", ReturnMsg1 = "";
            string strErr = "";

            if ( String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Select Proper Customer From List. " + "</li>";
            }
            // --------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            Entity.DBNote objEntity = new Entity.DBNote();
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.VoucherNo = txtVoucherNo.Text;
                        objEntity.InvoiceNo = drpBillNo.SelectedValue;
                        objEntity.VoucherDate = Convert.ToDateTime(txtVoucherDate.Text);
                        if (hdnDBC.Value == "DBNT")
                            objEntity.DBCustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        else if (hdnDBC.Value == "CRNT")
                            objEntity.CRCustomerID = Convert.ToInt64(hdnCustomerID.Value);

                        objEntity.DBC = hdnDBC.Value;
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
                        objEntity.VoucherAmount = (!String.IsNullOrEmpty(txtTotNetAmt.Text)) ? Convert.ToDecimal(txtTotNetAmt.Text) : 0;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.FinancialTransMgmt.AddUpdateDBCRNote(objEntity, out ReturnCode, out ReturnMsg, out ReturnInvoiceNo);
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnInvoiceNo) && !String.IsNullOrEmpty(txtVoucherNo.Text))
                        {
                            ReturnInvoiceNo = txtVoucherNo.Text;
                        }
                        BAL.FinancialTransMgmt.DeleteDBCRNoteDetailByVoucherNo(ReturnInvoiceNo, out ReturnCode1, out ReturnMsg1);
                        // --------------------------------------------------------------
                        if (ReturnCode1 > 0 && !String.IsNullOrEmpty(ReturnInvoiceNo))
                        {
                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;

                            //DataTable dtDetail = new DataTable();
                            //dtDetail = (DataTable)Session["dtDetail"];
                            // --------------------------------------------------------------

                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            Entity.DBNote_Detail objQuotDet = new Entity.DBNote_Detail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.VoucherNo = ReturnInvoiceNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.LocationID = 0;
                                objQuotDet.TaxType = Convert.ToInt16(dr["TaxType"]);
                                objQuotDet.DBC = hdnDBC.Value;
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
                                objQuotDet.NetAmt = Convert.ToDecimal(dr["NetAmt"]);
                                objQuotDet.HeaderDiscAmt = Convert.ToDecimal(dr["HeaderDiscAmt"]);
                                objQuotDet.ForOrderNo = "";
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.FinancialTransMgmt.AddUpdateDBCRNoteDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);

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
                            if (!String.IsNullOrEmpty(txtVoucherNo.Text))
                            {

                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
                            else
                            {
                                txtVoucherNo.Text = ReturnInvoiceNo;
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
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
            txtVoucherDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            
            txtVoucherNo.Text = ""; // BAL.CommonMgmt.GetSalesOrderNo(txtOrderDate.Text);
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            
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

            BindDBCRNoteDetailList("");
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

                }
                // -----------------------------------------------------------------------------------
                // Binding Sales/Purchase Bill For CRNT/DBNT
                // -----------------------------------------------------------------------------------
                int recNo;
                string Customer = txtCustomerName.Text;
                string StrCustomerName = (Customer.IndexOf(" -")>0) ? Customer.Substring(0, Customer.IndexOf(" -")) : txtCustomerName.Text;

                drpBillNo.Items.Clear();
                if (hdnDBC.Value == "DBNT")
                {
                    //List<Entity.PurchaseBill> lstPurchase = new List<Entity.PurchaseBill>();
                    //lstPurchase = BAL.PurchaseBillMgmt.GetPurchasePendingBillsByCustomerID(Convert.ToInt64(hdnCustomerID.Value));

                    //drpBillNo.DataSource = lstPurchase;
                    drpBillNo.DataSource = BAL.PurchaseBillMgmt.GetPurchasePendingBillsByCustomerID(Convert.ToInt64(hdnCustomerID.Value));
                    drpBillNo.DataValueField = "InvoiceNo";
                    drpBillNo.DataTextField = "InvoiceNo";
                    drpBillNo.DataBind();
                    //drpBillNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Bill No --", ""));

                }
                else if (hdnDBC.Value == "CRNT")
                {
                    //List<Entity.SalesBill> lstPurchase = new List<Entity.SalesBill>();
                    //lstPurchase = BAL.SalesBillMgmt.GetSalesPendingBillsByCustomerID(Convert.ToInt64(hdnCustomerID.Value));

                    //drpBillNo.DataSource = lstPurchase;

                    drpBillNo.DataSource = BAL.SalesBillMgmt.GetSalesPendingBillsByCustomerID(Convert.ToInt64(hdnCustomerID.Value));
                    drpBillNo.DataValueField = "InvoiceNo";
                    drpBillNo.DataTextField = "InvoiceNo";
                    drpBillNo.DataBind();
                    
                }
                drpBillNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Bill No --", ""));
                // ------------------------------------------
                BindDBCRNoteDetailList("");
                // ------------------------------------------            
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
                        //string fororderno = ((DropDownList)e.Item.FindControl("drpForOrderNo")).SelectedValue;

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

                        //dr["InvoiceNo"] = "";
                        //dr["QuotationNo"] = "";
                        //dr["InquiryNo"] = "";
                        //dr["OrderNo"] = "";
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
                        //dr["ForOrderNo"] = (!String.IsNullOrEmpty(fororderno)) ? fororderno : "";

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

        protected void rptOrderDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                //HtmlTableCell tdOutward = ((HtmlTableCell)e.Item.FindControl("tdOutwardNo"));
                //tdOutward.InnerText = (hdnStockOutward.Value == "sale") ? "S.O #" : "Outward #";
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2, v3, v4, v5, v6, v7, v8;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "DiscountAmt"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Amount"));
                v3 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaxAmount"));
                v4 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "AddTaxAmt"));
                v5 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmt"));

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
                // -----------------------------------------------------
                // Binding Customer's Purchase Order
                // -----------------------------------------------------
                //DropDownList drpForOrderNo = (DropDownList)e.Item.FindControl("drpForOrderNo");
                //drpForOrderNo.Items.Clear();
                //if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                //    drpForOrderNo.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", Convert.ToInt64(hdnCustomerID.Value), "Approved", 0, 0);
                //else
                //    drpForOrderNo.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", -1, "Approved", 0, 0);
                //drpForOrderNo.DataTextField = "OrderNo";
                //drpForOrderNo.DataValueField = "OrderNo";
                //drpForOrderNo.DataBind();
                //drpForOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SO # --", ""));

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
        public static string DeleteDBCRNote(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FinancialTransMgmt.DeleteDBCRNote(pkID, out ReturnCode, out ReturnMsg);
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
            Decimal q = 0;
            if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                q = String.IsNullOrEmpty(txtUnitQty.Text) ? 0 : (Convert.ToDecimal(txtUnitQty.Text) * Convert.ToDecimal(hdnProductUnitQty.Value));
            else
                q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);

            //Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
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

            //Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
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
                    if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")
                    {
                        hdnProductUnitQty.Value = (!String.IsNullOrEmpty(hdnProductUnitQty.Value)) ? hdnProductUnitQty.Value : "1";
                        row.SetField("Qty", (Convert.ToDecimal(edUnitQty.Text)) * (Convert.ToDecimal(hdnProductUnitQty.Value)));
                    }
                    else
                    {
                        row.SetField("Qty", edQuantity.Text);
                    }
                    row.SetField("UnitQuantity", (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? edUnitQuantity.Value : "0");
                    row.SetField("UnitQty", edUnitQty.Text);
                    if (hdnSerialKey.Value == "SA98-6HY9-HU67-LORF")    // ShaktiPet
                    {
                        hdnProductUnitQty.Value = (!String.IsNullOrEmpty(edUnitQuantity.Value)) ? edUnitQuantity.Value : "1";
                        row.SetField("Quantity", (Convert.ToDecimal(edUnitQty.Text)) * (Convert.ToDecimal((!String.IsNullOrEmpty(edUnitQuantity.Value)? Convert.ToDecimal(edUnitQuantity.Value):1))));
                        row.SetField("Qty", (Convert.ToDecimal(edUnitQty.Text)) * (Convert.ToDecimal((!String.IsNullOrEmpty(edUnitQuantity.Value) ? Convert.ToDecimal(edUnitQuantity.Value) : 1))));
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

        protected void drpBillNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

                if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpBillNo.SelectedValue))
                {
                    dtDetail.Clear();
                if (hdnDBC.Value == "DBNT")
                {
                    int TotalCount = 0;
                    List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
                    Int64 purPKID = BAL.CommonMgmt.GetPurchaseBillPrimaryID(drpBillNo.SelectedValue);
                    // ----------------------------------------------------
                    lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillList(purPKID, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

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
                
                txtHeadDiscount_TextChanged(null, null);

                dtDetail = BAL.PurchaseBillMgmt.GetPurchaseBillDetailWithDisc(drpBillNo.SelectedValue);
                    foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;
                    foreach (DataRow row in dtDetail.Rows)
                    {
                        Decimal uq = (!String.IsNullOrEmpty(row["UnitQuantity"].ToString())) ? Convert.ToDecimal(row["UnitQuantity"]) : 1;
                        Decimal a = (!String.IsNullOrEmpty(row["NetAmt"].ToString())) ? Convert.ToDecimal(row["NetAmt"]) : 0;
                        Decimal q = (!String.IsNullOrEmpty(row["Qty"].ToString())) ? Convert.ToDecimal(row["Qty"]) : 0;
                        Decimal ur = (!String.IsNullOrEmpty(row["Rate"].ToString())) ? Convert.ToDecimal(row["Rate"]) : 0;
                        Decimal dp = (!String.IsNullOrEmpty(row["DiscountPer"].ToString())) ? Convert.ToDecimal(row["DiscountPer"]) : 0;
                        Decimal dpa = (!String.IsNullOrEmpty(row["DiscountAmt"].ToString())) ? Convert.ToDecimal(row["DiscountAmt"]) : 0;
                        Decimal tr = (!String.IsNullOrEmpty(row["TaxRate"].ToString())) ? Convert.ToDecimal(row["TaxRate"]) : 0;
                        Decimal at = (!String.IsNullOrEmpty(row["AddTaxPer"].ToString())) ? Convert.ToDecimal(row["AddTaxPer"]) : 0;
                        Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(row["TaxType"].ToString())) ? Convert.ToInt16(row["TaxType"]) : 0);

                        //HeaderDiscItemWise = Math.Round((HeaderDiscAmt * a) / TotalAmt, 2);
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
                        //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(), HeaderDiscItemWise, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

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
                    }
                }
                else if (hdnDBC.Value == "CRNT")
                {
                    int TotalCount = 0;
                    List<Entity.SalesBill> lstEntity = new List<Entity.SalesBill>();
                    Int64 purPKID = BAL.CommonMgmt.GetSalesBillPrimaryID(drpBillNo.SelectedValue);
                    // ----------------------------------------------------
                    lstEntity = BAL.SalesBillMgmt.GetSalesBillList(purPKID, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

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

                    txtHeadDiscount_TextChanged(null, null);
                    dtDetail = BAL.SalesBillMgmt.GetSalesBillDetailWithDisc(drpBillNo.SelectedValue);

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
            
            //Session.Add("dtDetail", dtDetail);
            //dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
            Session.Add("dtDetail", dtDetail);

            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            txtHeadDiscount_TextChanged(null, null);
        }

        public void OnlyViewControls()
        {
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
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
            
        }
        public void BindDBCRNoteDetailList(string pVoucherNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.FinancialTransMgmt.GetDBCRNoteDetail(pVoucherNo);
            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        [System.Web.Services.WebMethod]
        public static string GetSalesBillNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetSalesBillNo(pkID);
            return tempVal;
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

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
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
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.DBNote> lstEntity = new List<Entity.DBNote>();
                // ----------------------------------------------------
                lstEntity = BAL.FinancialTransMgmt.GetDBCRList(Convert.ToInt64(hdnpkID.Value),hdnDBC.Value, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtVoucherNo.Text = lstEntity[0].VoucherNo;
                txtVoucherDate.Text = lstEntity[0].VoucherDate.ToString("yyyy-MM-dd");

                if (lstEntity[0].DBC == "DBNT")
                { 
                    hdnCustomerID.Value = lstEntity[0].DBCustomerID.ToString();
                    txtCustomerName.Text = lstEntity[0].DBCustomerName;
                }
                if (lstEntity[0].DBC == "CRNT")
                {
                    hdnCustomerID.Value = lstEntity[0].CRCustomerID.ToString();
                    txtCustomerName.Text = lstEntity[0].CRCustomerName;
                }
                

                txtTotBasicAmt.Text = lstEntity[0].BasicAmt.ToString();
                txtHeadDiscount.Text = lstEntity[0].DiscountAmt.ToString();
                txtTotGST.Text = (lstEntity[0].SGSTAmt.ToString() + lstEntity[0].CGSTAmt.ToString() + lstEntity[0].IGSTAmt.ToString());
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
                // -----------------------------------------------------------------------------------
                // Binding Sales/Purchase Bill For CRNT/DBNT
                // -----------------------------------------------------------------------------------
                int recNo;
                string Customer = txtCustomerName.Text;
                string StrCustomerName = (Customer.IndexOf(" -") > 0) ? Customer.Substring(0, Customer.IndexOf(" -")) : txtCustomerName.Text;
                if (hdnDBC.Value == "DBNT")
                {
                    List<Entity.PurchaseBill> lstPurchase = new List<Entity.PurchaseBill>();
                    lstPurchase = BAL.PurchaseBillMgmt.GetPurchaseBillList(0, Session["LoginUserID"].ToString(), StrCustomerName, 0, 10, out recNo);
                    drpBillNo.DataSource = lstPurchase;
                }
                else if (hdnDBC.Value == "CRNT")
                {
                    List<Entity.SalesBill> lstPurchase = new List<Entity.SalesBill>();
                    lstPurchase = BAL.SalesBillMgmt.GetSalesBillList(0, Session["LoginUserID"].ToString(), StrCustomerName, 0, 10, out recNo);
                    drpBillNo.DataSource = lstPurchase;
                }
                drpBillNo.DataValueField = "InvoiceNo";
                drpBillNo.DataTextField = "InvoiceNo";
                drpBillNo.DataBind();
                drpBillNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Bill No --", ""));
                
                if (!String.IsNullOrEmpty(lstEntity[0].InvoiceNo))
                {
                    drpBillNo.ClearSelection();
                    drpBillNo.SelectedValue = lstEntity[0].InvoiceNo;
                }
                
                // -------------------------------------------------------------------------
                BindDBCRNoteDetailList(lstEntity[0].VoucherNo);
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
    }
}