using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Globalization;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace StarsProject
{
    public partial class PurchaseBill : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount, totDiscAmt, totAddTaxAmt, totSGST, totCGST, totIGST;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SaveClick"] = "no";
                Session.Remove("dtModuleDoc");
                Session.Remove("dtSpecs");
                Session.Remove("mySpecs");
                Session.Remove("dtDetail");
                Session.Remove("dtSchedule");
                Session.Remove("dtAssembly");
                // ---------------------------------------------------------------
                // Checking Duplicate
                // ---------------------------------------------------------------
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
                hdnStockInward.Value = BAL.CommonMgmt.GetConstant("StockInward", 0, 1).ToLower();
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
                hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();
                // --------------------------------------------------------
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        ClearAllField();
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

                txtInvoiceNo.Focus();
                //decimal TaxAmt = 0;
                //decimal CGSTPer = 0, CGSTAmt = 0;
                //decimal SGSTPer = 0, SGSTAmt = 0,IGSTPer = 0, IGSTAmt = 0,NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;

                //funCalculate(1, 2, 100, 5, 0, 5, 5, false,out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(),
                                                        "<link href='" +
                                                        ResolveUrl("~") +
                                                        "app-assets/vendors/select2/select2-materialize.css' rel='stylesheet' type='text/css' />",
                                                        false);
            }
            else
            {
                myModuleAttachment.ModuleName = "purcbill";
                myModuleAttachment.KeyValue = txtInvoiceNo.Text;
                myModuleAttachment.ManageLibraryDocs();
            }
        }
        //public Boolean isIGST()
        //{
        //    if (string.Equals(hdnCustStateID.Value, Session["CompanyStateCode"].ToString()))
        //        return false;
        //    else
        //        return true;             
        //}
        protected void drpTerminationOfDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCustStateID.Value = drpTerminationOfDelivery.SelectedValue;
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());

            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail1);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtProductName);", true);
        }

        //public void funOnChangeTermination()
        //{
        //    DataTable dtDetail = new DataTable();
        //    dtDetail = (DataTable)Session["dtDetail"];

        //    foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

        //    foreach (DataRow row in dtDetail.Rows)
        //    {
        //        if(isIGST())
        //        {
        //            row.SetField("IGSTPer", Convert.ToDecimal(row["CGSTPer"]) + Convert.ToDecimal(row["SGSTPer"]) + Convert.ToDecimal(row["IGSTPer"]));
        //            row.SetField("SGSTPer",0);
        //            row.SetField("CGSTPer", 0);
        //            row.SetField("IGSTAmt", Convert.ToDecimal(row["CGSTAmt"]) + Convert.ToDecimal(row["SGSTAmt"]) + Convert.ToDecimal(row["IGSTAmt"]));
        //            row.SetField("SGSTAmt", 0);
        //            row.SetField("CGSTAmt", 0);
        //        }
        //        else
        //        {
        //            row.SetField("CGSTPer", (Convert.ToDecimal(row["CGSTPer"]) + Convert.ToDecimal(row["SGSTPer"]) + Convert.ToDecimal(row["IGSTPer"]))/2);
        //            row.SetField("SGSTPer", Convert.ToDecimal(row["CGSTPer"])) ;
        //            row.SetField("IGSTPer", 0);
        //            row.SetField("CGSTAmt", Math.Round((Convert.ToDecimal(row["CGSTAmt"]) + Convert.ToDecimal(row["SGSTAmt"]) + Convert.ToDecimal(row["IGSTAmt"])) / 2,2));
        //            row.SetField("SGSTAmt", Convert.ToDecimal(row["CGSTAmt"]) );
        //            row.SetField("IGSTAmt", 0);
        //        }
        //    }
        //    rptOrderDetail.DataSource = dtDetail;
        //    rptOrderDetail.DataBind();

        //    Session.Add("dtDetail", dtDetail);
        //}

        public void OnlyViewControls()
        {
            txtInvoiceNo.ReadOnly = true;
            txtInvoiceDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtTotBasicAmt.ReadOnly = true;
            txtHeadDiscount.ReadOnly = true;
            txtTotGST.ReadOnly = true;
            txtRoff.ReadOnly = true;
            txtTotNetAmt.ReadOnly = true;
            txtBillNo.ReadOnly = true;
            txtTermsCondition.ReadOnly = true;
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

            drpModeOfTransport.Attributes.Add("disabled", "disabled");
            drpLocation.Attributes.Add("disabled", "disabled");
            txtTransporterName.ReadOnly = true;
            txtVehicleNo.ReadOnly = true;
            txtLRNo.ReadOnly = true;
            txtLRDate.ReadOnly = true;
            txtTransportRemark.ReadOnly = true;

            txtTotOthChrgBeforeGST.ReadOnly = true;
            txtTotOthChrgAfterGST.ReadOnly = true;
            txtConsumer.ReadOnly = true;
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
            drpOthChrg1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", "", true));

            drpOthChrg2.DataSource = lstOthChrg;
            drpOthChrg2.DataValueField = "pkID";
            drpOthChrg2.DataTextField = "ChargeName";
            drpOthChrg2.DataBind();
            drpOthChrg2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select OtherCharge --", "", true));

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

            // ---------------- Fixed Ledger -------------------------------------
            List<Entity.Customer> lstLedger = new List<Entity.Customer>();
            lstLedger = BAL.CustomerMgmt.GetFixedLedgerForDropdown();
            drpLedger.DataSource = lstLedger;
            drpLedger.DataValueField = "CustomerID";
            drpLedger.DataTextField = "CustomerName";
            drpLedger.DataBind();
            drpLedger.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Purc. A/c --", ""));
            // ---------------- State List -------------------------------------
            List<Entity.State> lstEvents = new List<Entity.State>();
            lstEvents = BAL.StateMgmt.GetStateList();
            drpTerminationOfDelivery.DataSource = lstEvents;
            drpTerminationOfDelivery.DataValueField = "StateCode";
            drpTerminationOfDelivery.DataTextField = "StateName";
            drpTerminationOfDelivery.DataBind();
            drpTerminationOfDelivery.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All State --", ""));

            //----------------------Terms and conditions------------------------
            List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            drpTNC.DataSource = lstList2;
            drpTNC.DataValueField = "pkID";
            drpTNC.DataTextField = "TNC_Header";
            drpTNC.DataBind();
            drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

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
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }
        public void BindPurchaseBillDetailList(string pInvoiceNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(pInvoiceNo);
            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }
        public List<Entity.PurchaseOrder> BindPurchaseOrderList(Int64 pCustomerID, Int64 pProductID)
        {
            List<Entity.PurchaseOrder> lstPurcOrder = new List<Entity.PurchaseOrder>();
            lstPurcOrder = BAL.PurchaseOrderMgmt.GetPurchaseOrderListByCustomer(Session["LoginUserID"].ToString(), pCustomerID, pProductID, "", 0, 0);
            return lstPurcOrder;
        }

        public List<Entity.Inward> BindInwardList(Int64 pCustomerID, Int64 pProductID)
        {
            List<Entity.Inward> lstInward = new List<Entity.Inward>();
            lstInward = BAL.InwardMgmt.GetInwardListByCustomer(pCustomerID, pProductID, Session["LoginUserID"].ToString(), 0, 0);
            return lstInward;
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
                    string orderno = ((DropDownList)e.Item.FindControl("drpOrderNo")).SelectedValue;

                    dr["InvoiceNo"] = txtInvoiceNo.Text;
                    dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                    dr["TaxType"] = (!String.IsNullOrEmpty(taxtype)) ? Convert.ToInt16(taxtype) : 0;
                    dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                    dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                    dr["Qty"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                    dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                    dr["Rate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
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

                    dr["AddTaxPer"] = (!String.IsNullOrEmpty(addtaxper)) ? Convert.ToDecimal(addtaxper) : 0;
                    dr["AddTaxAmt"] = (!String.IsNullOrEmpty(addtaxamt)) ? Convert.ToDecimal(addtaxamt) : 0;
                    dr["NetAmt"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                    
                    dr["HeaderDiscAmt"] = (!String.IsNullOrEmpty(headdiscamt)) ? Convert.ToDecimal(headdiscamt) : 0;
                    dr["OrderNo"] = (!String.IsNullOrEmpty(orderno)) ? orderno : "";

                    dtDetail.Rows.Add(dr);
                    // ---------------------------------------------------------------
                    rptOrderDetail.DataSource = dtDetail;
                    rptOrderDetail.DataBind();
                    // ---------------------------------------------------------------
                    Session.Add("dtDetail", dtDetail);
                    strErr += "<li>" + "Item Added Successfully !" + "</li>";
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        }

        protected void rptOrderDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlTableCell tdInward = ((HtmlTableCell)e.Item.FindControl("tdInwardNo"));
                tdInward.InnerText = (hdnStockInward.Value == "purchase") ? "P.O #" : "GRN #";
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
            if (e.Item.ItemType == ListItemType.Footer)
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
            // -------------------------------------------------------------------------
            // Check Tax Applied
            // -------------------------------------------------------------------------
            setTaxApplied();
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

        protected void txtOthChrgAmt1_TextChanged(object sender, EventArgs e)
        {
            decimal txtOthChrgAmt = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;

            Int64 chrgid = (!String.IsNullOrEmpty(drpOthChrg1.SelectedValue)) ? Convert.ToInt64(drpOthChrg1.SelectedValue) : 0;
            txtOthChrgAmt = String.IsNullOrEmpty(txtOthChrgAmt1.Text) ? 0 : Convert.ToDecimal(txtOthChrgAmt1.Text);

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

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

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

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

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

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

            BAL.CommonMgmt.funOthChrgTextChange(chrgid, txtOthChrgAmt, out  OthChrgGSTAmt, out  OthChrgBasicAmt);

            hdnOthChrgGST4.Value = OthChrgGSTAmt.ToString();
            hdnOthChrgBasic4.Value = OthChrgBasicAmt.ToString();

            funCalculateTotal();
            drpOthChrg5.Focus();
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
            txtHeadDiscount.Focus();
        }
        public void setGSTNo(string pCustomerID)
        {
            string pGSTNO = BAL.CommonMgmt.checkGSTNO(pCustomerID);
            if (!String.IsNullOrEmpty(pGSTNO))
            {
                isGSTAvailable.InnerText = "GST # : Available";
                isGSTAvailable.Attributes.Add("class", "badge green white-text");
            }
            else
            {
                isGSTAvailable.InnerText = "GST # : Missing";
                isGSTAvailable.Attributes.Add("class", "badge red white-text");
            }
        }
        public void setTaxApplied()
        {
            spnTaxApplied.InnerText = "";
            spnTaxApplied.Attributes.Add("class", "");
            if (Convert.ToDecimal(hdnTotIGSTAmt.Value) > 0)
            {
                spnTaxApplied.InnerText = "IGST Applied >>>";
                spnTaxApplied.Attributes.Add("class", "badge red white-text");
            }
            else if ((Convert.ToDecimal(hdnTotCGSTAmt.Value) + Convert.ToDecimal(hdnTotSGSTAmt.Value)) > 0)
            {
                spnTaxApplied.InnerText = "CGST/SGST Applied >>>";
                spnTaxApplied.Attributes.Add("class", "badge navy white-text");
            }
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
                // ----------------------------------------------------
                lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtInvoiceNo.Text = lstEntity[0].InvoiceNo;
                txtInvoiceDate.Text = lstEntity[0].InvoiceDate.ToString("yyyy-MM-dd");
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                drpLedger.SelectedValue = lstEntity[0].FixedLedgerID.ToString();
                drpBank.SelectedValue = lstEntity[0].BankID.ToString();
                // -------------------------------------------------------------
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                setGSTNo(hdnCustomerID.Value);
                // -------------------------------------------------------------
                txtCRDays.Text = lstEntity[0].CRDays.ToString();
                txtDueDate.Text = lstEntity[0].DueDate.ToString("yyyy-MM-dd");

                drpLocation.SelectedValue = lstEntity[0].LocationID.ToString();
                drpTerminationOfDelivery.SelectedValue = (lstEntity[0].TerminationOfDeliery > 0) ? lstEntity[0].TerminationOfDeliery.ToString() : "";
                txtTermsCondition.Text = lstEntity[0].TermsCondition;
                txtBillNo.Text = lstEntity[0].BillNo;
                txtConsumer.Text = lstEntity[0].ForCoustmerID;
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
                txtLRNo.Text = lstEntity[0].LRNo;
                txtLRDate.Text = lstEntity[0].LRDate.ToString("yyyy-MM-dd");
                txtTransportRemark.Text = lstEntity[0].TransportRemark;
                // -------------------------------------------------------------------------
                BindPurchaseBillDetailList(lstEntity[0].InvoiceNo);
            }
            // -------------------------------------------------------------------------
            myModuleAttachment.ModuleName = "purcbill";
            myModuleAttachment.KeyValue = txtInvoiceNo.Text;
            myModuleAttachment.BindModuleDocuments();
            // -------------------------------------------------------------------------
            txtCustomerName.Enabled = (pMode.ToLower() == "add") ? true : false;
            txtCustomerName.Focus();
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
            // ---------------------------------------------------------------
            // Checking Duplicate
            // ---------------------------------------------------------------
            if (Session["SaveClick"].ToString() == "yes")
                Session["SaveClick"] = "no";


            ClearAllField();
            Session.Remove("dtDetail");
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnInvoiceNo = "", ReturnMsg1 = "";
            string strErr = "";
            // ---------------------------------------------------------------
            // Checking Duplicate
            // ---------------------------------------------------------------
            if (Session["SaveClick"].ToString() == "yes")
            {
                _pageValid = false;
                strErr += "<li>" + "Saving Record is Under Process. Please Wait..." + "</li>";
                btnSave.Disabled = true;
                btnSaveEmail.Disabled = true;
            }
            else
            {
                Session["SaveClick"] = "yes";
            }

            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(drpLedger.SelectedValue) || String.IsNullOrEmpty(hdnCustomerID.Value) || String.IsNullOrEmpty(drpBank.SelectedValue) ||
                (String.IsNullOrEmpty(txtInvoiceDate.Text)) || (String.IsNullOrEmpty(txtDueDate.Text))) 
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(drpLedger.SelectedValue))
                    strErr += "<li>" + "Ledger Selection is required. " + "</li>";

                if (String.IsNullOrEmpty(drpBank.SelectedValue))
                    strErr += "<li>" + "Bank Selection is required. " + "</li>";

                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Select Proper Customer From List. " + "</li>";

                if (String.IsNullOrEmpty(txtInvoiceDate.Text))
                    strErr += "<li>" + "Invoice Date is required. " + "</li>";

                if (String.IsNullOrEmpty(txtDueDate.Text))
                    strErr += "<li>" + "Due Date is required. " + "</li>";

            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation 
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtInvoiceDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtInvoiceDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Future Invoice Date is Not Valid." + "</li>";
                }
            }

            //--------------------------------------------------------------- 
            // --------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            Entity.PurchaseBill objEntity = new Entity.PurchaseBill();
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        Int64 intLocation = (!String.IsNullOrEmpty(drpLocation.SelectedValue)) ? Convert.ToInt64(drpLocation.SelectedValue) : 0;
                        objEntity.InvoiceNo = txtInvoiceNo.Text;
                        objEntity.InvoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
                        objEntity.FixedLedgerID = Convert.ToInt64(drpLedger.SelectedValue);
                        objEntity.BankID = Convert.ToInt64(drpBank.SelectedValue);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.LocationID = intLocation;
                        objEntity.TerminationOfDeliery = (!String.IsNullOrEmpty(drpTerminationOfDelivery.SelectedValue)) ? Convert.ToInt64(drpTerminationOfDelivery.SelectedValue) : 0;
                        objEntity.TermsCondition = txtTermsCondition.Text;
                        objEntity.BillNo = txtBillNo.Text;
                        objEntity.ForCoustmerID = txtConsumer.Text;

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

                        objEntity.ModeOfTransport = drpModeOfTransport.SelectedValue;
                        objEntity.TransporterName = txtTransporterName.Text;
                        objEntity.VehicleNo = txtVehicleNo.Text;
                        objEntity.LRNo = txtLRNo.Text;
                        if (!String.IsNullOrEmpty(txtLRDate.Text))
                            objEntity.LRDate = Convert.ToDateTime(txtLRDate.Text);
                        objEntity.TransportRemark = txtTransportRemark.Text;

                        objEntity.CRDays = (!String.IsNullOrEmpty(txtCRDays.Text)) ? Convert.ToInt64(txtCRDays.Text) : 0; 
                        objEntity.DueDate = Convert.ToDateTime(txtDueDate.Text);

                        objEntity.NetAmt = (!String.IsNullOrEmpty(txtTotNetAmt.Text)) ? Convert.ToDecimal(txtTotNetAmt.Text) : 0;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.PurchaseBillMgmt.AddUpdatePurchaseBill(objEntity, out ReturnCode, out ReturnMsg, out ReturnInvoiceNo);
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnInvoiceNo) && !String.IsNullOrEmpty(txtInvoiceNo.Text))
                        {
                            ReturnInvoiceNo = txtInvoiceNo.Text;
                        }
                        
                        // --------------------------------------------------------------
                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnInvoiceNo))
                        {
                            BAL.PurchaseBillMgmt.DeletePurchaseBillDetailByInvoiceNo(ReturnInvoiceNo, out ReturnCode1, out ReturnMsg1);

                            myModuleAttachment.KeyValue = ReturnInvoiceNo;
                            myModuleAttachment.SaveModuleDocs();
                            // ------------------------------------------------------------
                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            Entity.PurchaseBillDetail objQuotDet = new Entity.PurchaseBillDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.InvoiceNo = ReturnInvoiceNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.LocationID = intLocation;
                                objQuotDet.TaxType = Convert.ToInt16(dr["TaxType"]);
                                objQuotDet.Qty = Convert.ToDecimal(dr["Qty"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.Rate = Convert.ToDecimal(dr["Rate"]);
                                objQuotDet.DiscountPer = Convert.ToDecimal(dr["DiscountPer"]);
                                objQuotDet.DiscountAmt = Convert.ToDecimal(dr["DiscountAmt"]);
                                objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                                objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);

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
                                objQuotDet.OrderNo = dr["OrderNo"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.PurchaseBillMgmt.AddUpdatePurchaseBillDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            // --------------------------------------------------------------
                            //if (ReturnCode > 0)
                            //{
                            //    Session.Remove("dtDetail");
                            //}
                        }
                        // --------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            if (!String.IsNullOrEmpty(txtInvoiceNo.Text))
                            {
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
                            else
                            {
                                txtInvoiceNo.Text = ReturnInvoiceNo;
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            }
                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                        }
                    }
                }
                else
                {
                    strErr += "<li>Atleast One Item is required to save Purchase Bill Information !</li>";
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
            Session.Remove("dtModuleDoc");
            Session.Remove("dtSpecs");
            Session.Remove("mySpecs");
            Session.Remove("dtDetail");
            Session.Remove("dtSchedule");
            Session.Remove("dtAssembly");
            //---------------------------------------------------------------------------------------
            hdnpkID.Value = "";
            txtInvoiceDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtInvoiceNo.Text = ""; // BAL.CommonMgmt.GetSalesOrderNo(txtOrderDate.Text);
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtBillNo.Text = "";
            txtTermsCondition.Text = "";

            txtHeadDiscount.Text = "";
            txtOthChrgAmt1.Text = "";
            txtOthChrgAmt2.Text = "";
            txtOthChrgAmt3.Text = "";
            txtOthChrgAmt4.Text = "";
            txtOthChrgAmt5.Text = "";
            drpOthChrg1.SelectedValue = "";
            drpOthChrg2.SelectedValue = "";
            drpOthChrg3.SelectedValue = "";
            drpOthChrg4.SelectedValue = "";
            drpOthChrg5.SelectedValue = "";

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

            drpModeOfTransport.SelectedValue = "";
            txtTransporterName.Text = "";
            txtVehicleNo.Text = "";
            txtLRNo.Text = "";
            txtLRDate.Text = "";
            txtTransportRemark.Text = "";
            dvLoadItems.Visible = true;

            txtCRDays.Text = "0";
            txtDueDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            BindPurchaseBillDetailList("");

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
            // ------------------------------------------------------------
            myModuleAttachment.ModuleName = "purcbill";
            myModuleAttachment.KeyValue = txtInvoiceNo.Text;
            myModuleAttachment.BindModuleDocuments();
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

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            if(!String.IsNullOrEmpty(hdnCustomerID.Value))
            { 
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";


                if (!String.IsNullOrEmpty(hdnCustStateID.Value))
                {
                    if (Convert.ToInt64(hdnCustStateID.Value) > 0)
                    {
                        drpTerminationOfDelivery.SelectedValue = hdnCustStateID.Value;
                    }
                }
                // ------------------------------------------
                if (lstEntity.Count>0)
                {
                    txtCRDays.Text = (!String.IsNullOrEmpty(lstEntity[0].CR_Days.ToString())) ? lstEntity[0].CR_Days.ToString() : "0";
                    calculateDueDate();
                }
                // ---------------------------------
                setGSTNo(hdnCustomerID.Value);
                // ---------------------------------
                BindPOInward();
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

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            Control rptFootCtrl = rptOrderDetail.Controls[rptOrderDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            if(!String.IsNullOrEmpty(hdnProductID.Value))
            { 
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
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
                //if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                //{
                txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
                hdnTaxType.Value = (lstEntity.Count > 0) ? lstEntity[0].TaxType.ToString() : "0";
                txtAddTaxPer.Text = (lstEntity.Count > 0) ? lstEntity[0].AddTaxPer.ToString() : "0";
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
                // ----------------------------------------------------------------------------
                // Binding Customer's Purchase Order
                // -----------------------------------------------------
                DropDownList drpOrderNo = (DropDownList)rptFootCtrl.FindControl("drpOrderNo");
                drpOrderNo.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                {
                    if (!String.IsNullOrEmpty(hdnProductID.Value))
                    {
                        if (hdnStockInward.Value == "purchase")
                        {
                            drpOrderNo.DataSource = BindPurchaseOrderList(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value));
                            drpOrderNo.DataTextField = "OrderNo";
                            drpOrderNo.DataValueField = "OrderNo";
                            drpOrderNo.DataBind();
                            drpOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- PO # --", ""));
                        }
                        else
                        {
                            drpOrderNo.DataSource = BindInwardList(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value));
                            drpOrderNo.DataTextField = "InwardNo";
                            drpOrderNo.DataValueField = "InwardNo";
                            drpOrderNo.DataBind();
                            drpOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- GRN # --", ""));
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
        public static string DeletePurchaseBill(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0, totrec = 0;
            string ReturnMsg = "";
            // -----------------------------------------------------------------------------------
            List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
            lstEntity = BAL.PurchaseBillMgmt.GetPurchaseBillList(Convert.ToInt64(pkID), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 10000, out totrec);
            if (lstEntity.Count > 0)
            {
                myModuleAttachment mya = new myModuleAttachment();
                mya.DeleteModuleEntry("purcbill", lstEntity[0].InvoiceNo.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
            }
            // --------------------------------- Delete Record
            BAL.PurchaseBillMgmt.DeletePurchaseBill(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProduct(string pProductName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(pProductName);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProduct(string pProductName, string pSearchModule)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            String SerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(pProductName, pSearchModule);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProductCust(string pProductName, string pSearchModule, Int64 CustomerID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            String SerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(SerialKey, pProductName, pSearchModule, CustomerID);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterSerialNo(Int64 ProductID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetSerialNoForDropdown(ProductID);
            return serializer.Serialize(rows);
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtUnit);", true);
        }
        protected void txtUnit_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtUnitRate);", true);
        }
        protected void txtUnitRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtDiscountPercent);", true);
        }
        protected void txtDiscountPercent_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtDiscountAmt);", true);
        }
        protected void txtDiscountAmt_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtTaxRate);", true);
        }
        protected void txtTaxRate_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtAddTaxPer);", true);
        }
        protected void txtAddTaxPer_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(editItem);", true);
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
            // --------------------------------------------------------------------------
            Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            String u = String.IsNullOrEmpty(txtUnit.Text) ? " " : txtUnit.Text;
            Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);
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
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);
            //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(), 0, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

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
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtProductName);", true);

        }
        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            //For Footer Section

            //For repeater other section
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            HiddenField edTaxType = (HiddenField)item.FindControl("edTaxType");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");
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
            //DropDownList edOrderNo = ((DropDownList)item.FindControl("edOrderNo"));
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
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);
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
                    row.SetField("Qty", edQuantity.Text);
                    row.SetField("Unit", edUnit.Text);
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
                }
            }
            dtDetail.AcceptChanges();

            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtProductName);", true);
        }

        //private void funCalculate(Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, bool isIGST, decimal HdDiscAmt, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        //{
        //    //Out
        //    TaxAmt = 0;
        //    CGSTPer = 0; CGSTAmt = 0;
        //    SGSTPer = 0; SGSTAmt = 0;
        //    IGSTPer = 0; IGSTAmt = 0;
        //    NetRate = 0; BasicAmt = 0; NetAmt = 0; ItmDiscPer1 = 0; ItmDiscAmt1 = 0; AddTaxAmt = 0;

        //    decimal BasicVal = 0,GSTAmt = 0;
        //    //In
        //    //TaxType = 0; Qty = 2; Rate = 100; ItmDiscPer = 5; ItmDiscAmt = 0; TaxPer = 5; AddTaxPer = 5; isIGST = false;HeadDiscAmt=0;

        //    if (Rate > 0)
        //    {
        //        if (ItmDiscPer > 0)
        //        {
        //            ItmDiscAmt = ((ItmDiscPer * Rate) / 100);
        //        }
        //        else
        //        {
        //            ItmDiscPer = Math.Round((ItmDiscAmt * 100) / Rate, 2);
        //        }

        //        NetRate = Math.Round(Rate - ItmDiscAmt, 2);
        //        ItmDiscPer1 = ItmDiscPer;
        //        ItmDiscAmt1 = ItmDiscAmt;
        //        BasicVal = Math.Round(Qty * NetRate, 2);

        //        if (TaxType == 0)
        //        {
        //            NetAmt = BasicVal;
        //            BasicVal = BasicVal - HdDiscAmt;
        //            decimal taxamt1 = Math.Round((((TaxPer + AddTaxPer) * BasicVal) / (100 + TaxPer + AddTaxPer)), 2);
        //            BasicAmt = Math.Round(BasicVal - taxamt1, 2);
        //            GSTAmt = Math.Round((((BasicAmt) * TaxPer) / 100), 2);
        //            AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
        //            BasicAmt = Math.Round(BasicVal - (AddTaxAmt + GSTAmt), 2);
        //        }
        //        else if (TaxType == 1)
        //        {
        //            BasicAmt = BasicVal - HdDiscAmt;
        //            GSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);
        //            AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
        //            NetAmt = Math.Round(BasicVal + GSTAmt + AddTaxAmt, 2);
        //        }
        //        else
        //        {
        //            BasicAmt = BasicVal;
        //            NetAmt = BasicVal;
        //        }

        //        if (isIGST)
        //        {
        //            IGSTPer = TaxPer;
        //            IGSTAmt = GSTAmt;
        //        }
        //        else
        //        {
        //            CGSTPer = Math.Round(TaxPer / 2, 2);
        //            SGSTPer = Math.Round(TaxPer / 2, 2);
        //            CGSTAmt = Math.Round(GSTAmt / 2, 2);
        //            SGSTAmt = Math.Round(GSTAmt / 2, 2);
        //        }
        //    }
        //}

        protected void txtHeadDiscount_TextChanged(object sender, EventArgs e)
        {
            //this.rptOrderDetail.ItemDataBound  -= rptOrderDetail_ItemDataBound;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            //decimal TotalAmt = Convert.ToDecimal(txtTotBasicAmt.Text) + Convert.ToDecimal(txtTotGST.Text) + Convert.ToDecimal(txtTotAddTaxAmt.Text);
            decimal TotalAmt = 0;
            decimal HeaderDiscAmt = (!String.IsNullOrEmpty(txtHeadDiscount.Text)) ? Convert.ToDecimal(txtHeadDiscount.Text) : 0;
            decimal HeaderDiscItemWise = 0;

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
                BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, at, HeaderDiscItemWise, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);
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
            txtTotBasicAmt.Focus();
            //this.rptOrderDetail.ItemDataBound += rptOrderDetail_ItemDataBound;

        }

        [WebMethod(EnableSession = true)]
        public static string GetPurchaseBillNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetPurchaseBillNo(pkID);
            return tempVal;
        }

        [WebMethod(EnableSession = true)]
        public static void GeneratePurchaseBill(Int64 pkID)
        {
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

            // ---------------------------------------------

            //tmpSerialKey ="6GZP-BW7W-39R8-T73F";
            if (tmpSerialKey == "SI08-SB94-MY45-RY15" || tmpSerialKey == "SOL4-PB94-KY45-TY15" || tmpSerialKey == "6CTR-6KWG-3TQV-3WU0") //Solnce // Demo Sharvaya
                GeneratePurchaseBill_Sharvaya(pkID);
            else if (tmpSerialKey == "6GZP-BW7W-78DF-HG88")     // Dishachi 
                GeneratePurchaseBill_Dishachi(pkID);
            else if (tmpSerialKey == "J63H-F8LX-B4B2-GYVZ")      //  HI-TECH
                GeneratePurchaseBill_HiTech(pkID);
            else if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")      // PerfectRoto
                GeneratePurchaseBill_Sharvaya(pkID);
            else if (tmpSerialKey == "QFOJ-9X7N-WGI0-RLSS")      // Devendra
                GeneratePurchaseBill_Devendra(pkID);
            else if (tmpSerialKey == "LVK4-MN01-K121-NGVL")      // MN Rubber
                GeneratePurchaseBill_Sharvaya(pkID);
            else if (tmpSerialKey == "TJ7S-06Q2-8R2U-KJWG")      // Gautam
                GeneratePurchaseBill_Gautam(pkID);
            else if (tmpSerialKey == "JME9-EI90-IKP9-K89I")      // JM Eletcricals
                GeneratePurchaseBill_JME(pkID);
            else if (tmpSerialKey == "COL1-AKL9-TEC9-SJ99")      // ColdTech
                GeneratePurchaseBill_ColdTech(pkID);
            else
                GeneratePurchaseBill_General(pkID);


        }

        public static void GeneratePurchaseBill_ColdTech(Int64 pkID)
        {
            HttpContext.Current.Session["printModule"] = "purchasebill";
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
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
           
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "SalesBill");
            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30, ProdDetail_Lines = 0;
            ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                    LeftMargin = (Int64)Convert.ToDouble(tmpary[2].ToString());
                    RightMargin = (Int64)Convert.ToDouble(tmpary[3].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                    LeftMargin = (Int64)Convert.ToDouble(tmpary[2].ToString());
                    RightMargin = (Int64)Convert.ToDouble(tmpary[3].ToString());
                }
            }
            pdfDoc.SetMargins(LeftMargin,RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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

                tblNested20.AddCell(pdf.setCell("Supplier / Vendor :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].Address.Trim() + lstCust[0].Area.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.Trim() + lstCust[0].Pincode.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName + "," + lstCust[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                //    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                //    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 30, 30, 10, 30 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("O.A. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("P.O. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": Verbally", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                tblNested2.AddCell(pdf.setCell("Transport " + lstQuot[0].TransporterName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                //  Defining : Company Master Information
                // -------------------------------------------------------------------------------------
                List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 10000, out totrec);

                PdfPTable tblNested202 = new PdfPTable(1);
                int[] column_tblNested202 = { 100 };
                tblNested202.SetWidths(column_tblNested202);

                tblNested202.AddCell(pdf.setCell("Company :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].OrgName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].CityName + " - " + lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].StateName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell("Phone : " + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblNested22 = new PdfPTable(4);
                int[] column_tblNested22 = { 30, 40, 10, 20 };
                tblNested22.SetWidths(column_tblNested22);
                tblNested22.AddCell(pdf.setCell("Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell(": " + "", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell("Buyer's GSTIN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell(": " + lstCust[0].GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));

                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
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
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));

                    Decimal Quantity = Convert.ToDecimal(dtItem.Rows[i]["Qty"]);
                    if (Quantity % 1 != 0)
                    {
                        tblDetail.AddCell(pdf.setCell(Convert.ToDecimal(dtItem.Rows[i]["Qty"]).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(Convert.ToDecimal(dtItem.Rows[i]["Quantity"]).ToString("0"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    }

                    tblDetail.AddCell(pdf.setCell(Convert.ToDecimal(dtItem.Rows[i]["Rate"]).ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(Convert.ToDecimal(dtItem.Rows[i]["Amount"]).ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(Convert.ToDecimal(dtItem.Rows[i]["Amount"]).ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }
                for (int i = 1; i < (13 - dtItem.Rows.Count); i++)
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
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(2);
                int[] column_tblTNC = { 6,94};
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                int K = 1;
                lstQuot[0].TermsCondition.Split('\n').ToList().ForEach(line =>
                {
                    if (!String.IsNullOrEmpty(line.Trim()))
                    {
                        tblTNC.AddCell(pdf.setCellTransparant(K.ToString() + ". ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                        tblTNC.AddCell(pdf.setCellTransparant(line, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    }
                    else
                    {
                        tblTNC.AddCell(pdf.setCellTransparant(" ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    }
                    K++;
                });
                // ---------------------------------------------------------------------------------------------------------
                PdfPTable tblAmount = new PdfPTable(2);
                int[] column_tblAmount = { 60, 40 };
                tblAmount.SetWidths(column_tblAmount);
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                totAmount = lstQuot[0].BasicAmt + lstQuot[0].DiscountAmt;
                totRNDOff = lstQuot[0].ROffAmt;
                totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                if (lstQuot[0].DiscountAmt > 0)
                    tblAmount.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                else
                    tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                tblAmount.AddCell(pdf.setCell(totAmount.ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                if (lstQuot[0].DiscountAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Discount Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].DiscountAmt.ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                    tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].BasicAmt.ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt1;
                    befGST += lstQuot[0].ChargeGSTAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeGSTAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeGSTAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeGSTAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeGSTAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeGSTAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                /* ---------------------------------------------------------- */
                tblAmount.AddCell(pdf.setCell(pdf.Table_MultipleGST(lstQuot[0].InvoiceNo,"purchasebill",column_tblAmount,pdf.fnCalibriBold8, pdf.fnCalibri8,0,pdf.paddingOf3), pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }

                //tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //tblAmount.AddCell(setCell("Grand Total  :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }

                // ****************************************************************
                //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
                netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Total Amount ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell(lstQuot[0].NetAmt.ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
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


                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name  : " + ((lstQuot.Count > 0) ? lstQuot[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch     : " + ((lstQuot.Count > 0) ? lstQuot[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No     : " + ((lstQuot.Count > 0) ? lstQuot[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code  : " + ((lstQuot.Count > 0) ? lstQuot[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("SWIFT Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankSwift : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                //int fileCount1 = 0;
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                //if (File.Exists(tmpFile1))
                //{
                //    if (File.Exists(tmpFile1))   //Signature print
                //    {
                //        PdfPTable tblSign = new PdfPTable(1);
                //        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile1);
                //        eSign.ScaleAbsolute(90, 70);
                //        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //        fileCount1 = fileCount1 + 1;
                //    }
                //}
                //else
                //{
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell("Authorized Signatory", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //}

                // ---------------------------------------------------
                int[] column_tblFooter = { 60, 40};
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName,4), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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
        public static void GeneratePurchaseBill_Devendra(Int64 pkID)
        {
            

            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(9);
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
            Int64 ProdDetail_Lines = 20;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "PurchaseBill");
            //ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (lstPrinter.Count > 0)
            {
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
            }


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
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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
                int[] column_tblMember = { 30, 25, 20, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].Address.TrimStart('.') + lstCust[0].Area.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.TrimStart('.') + lstCust[0].StateName.TrimStart('.') + lstCust[0].Pincode.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName.TrimStart('.') + " - " + lstCust[0].Pincode + ", " + lstCust[0].StateName.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + ((dtContact.Rows.Count > 0) ? dtContact.Rows[0]["ContactPerson1"].ToString() : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Executive Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 13, 20,10, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 13, 26,10, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("HSN Code", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));

                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["HSNCode"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    else
                    {
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    // ----------------------------------------
                    //List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    //lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].QuotationNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    //if (lstSpec.Count > 0)
                    //{
                    //    string tmpSpec = lstSpec[0].MaterialSpec.ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        if (sumDis > 0)
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        else
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    //        totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                    //    }
                    //}
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
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
                tblTNC.AddCell(pdf.setCell("Terms & Conditions : ", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

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
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt1;
                //    befGST += lstQuot[0].ChargeGSTAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt2;
                //    befGST += lstQuot[0].ChargeGSTAmt2;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt3;
                //    befGST += lstQuot[0].ChargeGSTAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt4;
                //    befGST += lstQuot[0].ChargeGSTAmt4;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt5;
                //    befGST += lstQuot[0].ChargeGSTAmt5;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                //{
                //    if (lstQuot[0].IGSTAmt > 0)
                //    {
                //        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //    else
                //    {
                //        tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //        tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //}

                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt2;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt4;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt5;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ROffAmt > 0 || lstQuot[0].ROffAmt < 0)
                //{
                //    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}

                //tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}

                //// ****************************************************************
                //netAmount = lstQuot[0].NetAmt;
                //PdfPTable tblAmount1 = new PdfPTable(2);
                //int[] column_tblAmount1 = { 60, 40 };
                //tblAmount1.SetWidths(column_tblAmount1);
                //string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                //tblAmount1.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount1.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //// -----------------------------------------------
                //if (sumDis > 0)
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //}
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-                
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);

                //tblFootDetail.AddCell(pdf.setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                //tblFootDetail.AddCell(pdf.setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo + "        " + "PAN No  : " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstBank.Count > 0) ? lstBank[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstBank.Count > 0) ? lstBank[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstBank.Count > 0) ? lstBank[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstBank.Count > 0) ? lstBank[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GeneratePurchaseBill_Sharvaya(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "yes";

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
            Int64 ProdDetail_Lines = 20;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "PurchaseBill");
            //ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (lstPrinter.Count > 0)
            {
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
            }


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
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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
                int[] column_tblMember = { 30, 25, 20, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].Address.TrimStart('.') + lstCust[0].Area.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.TrimStart('.') + lstCust[0].StateName.TrimStart('.') + lstCust[0].Pincode.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName.TrimStart('.') + " - " + lstCust[0].Pincode + ", " + lstCust[0].StateName.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + ((dtContact.Rows.Count > 0) ? dtContact.Rows[0]["ContactPerson1"].ToString() : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Executive Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 13, 30, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 13, 36, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    else
                    {
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    // ----------------------------------------
                    //List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    //lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].QuotationNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    //if (lstSpec.Count > 0)
                    //{
                    //    string tmpSpec = lstSpec[0].MaterialSpec.ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        if (sumDis > 0)
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        else
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    //        totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                    //    }
                    //}
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
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
                tblTNC.AddCell(pdf.setCell("Terms & Conditions : ", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

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
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt1;
                //    befGST += lstQuot[0].ChargeGSTAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt2;
                //    befGST += lstQuot[0].ChargeGSTAmt2;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt3;
                //    befGST += lstQuot[0].ChargeGSTAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt4;
                //    befGST += lstQuot[0].ChargeGSTAmt4;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt5;
                //    befGST += lstQuot[0].ChargeGSTAmt5;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                //{
                //    if (lstQuot[0].IGSTAmt > 0)
                //    {
                //        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //    else
                //    {
                //        tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //        tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //}

                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt2;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt4;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt5;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ROffAmt > 0 || lstQuot[0].ROffAmt < 0)
                //{
                //    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}

                //tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}

                //// ****************************************************************
                //netAmount = lstQuot[0].NetAmt;
                //PdfPTable tblAmount1 = new PdfPTable(2);
                //int[] column_tblAmount1 = { 60, 40 };
                //tblAmount1.SetWidths(column_tblAmount1);
                //string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                //tblAmount1.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount1.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //// -----------------------------------------------
                //if (sumDis > 0)
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //}
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-                
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);

                //tblFootDetail.AddCell(pdf.setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                //tblFootDetail.AddCell(pdf.setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo + "        " + "PAN No  : " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstBank.Count > 0) ? lstBank[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstBank.Count > 0) ? lstBank[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstBank.Count > 0) ? lstBank[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstBank.Count > 0) ? lstBank[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("SWIFT Code : " + ((lstBank.Count > 0) ? lstBank[0].BankSWIFT : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GeneratePurchaseBill_JME(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "yes";

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
            Int64 ProdDetail_Lines = 20;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "PurchaseBill");
            //ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (lstPrinter.Count > 0)
            {
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
            }


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
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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
                int[] column_tblMember = { 30, 25, 20, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].Address.TrimStart('.') + lstCust[0].Area.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.TrimStart('.') + lstCust[0].StateName.TrimStart('.') + lstCust[0].Pincode.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName.TrimStart('.') + " - " + lstCust[0].Pincode + ", " + lstCust[0].StateName.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 31, 28, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + ((dtContact.Rows.Count > 0) ? dtContact.Rows[0]["ContactPerson1"].ToString() : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Executive Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 13, 30, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 13, 36, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    else
                    {
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    // ----------------------------------------

                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(tmpProdAlias, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }

                    //string tmpSpec = dtItem.Rows[i]["ProductSpecification"].ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        if (sumDis > 0)
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        else
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    //        totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                    //    }
                    
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
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
                tblTNC.AddCell(pdf.setCell("Terms & Conditions : ", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

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
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt1;
                //    befGST += lstQuot[0].ChargeGSTAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt2;
                //    befGST += lstQuot[0].ChargeGSTAmt2;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt3;
                //    befGST += lstQuot[0].ChargeGSTAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt4;
                //    befGST += lstQuot[0].ChargeGSTAmt4;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt5;
                //    befGST += lstQuot[0].ChargeGSTAmt5;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                //{
                //    if (lstQuot[0].IGSTAmt > 0)
                //    {
                //        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //    else
                //    {
                //        tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //        tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //}

                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt2;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt4;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt5;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ROffAmt > 0 || lstQuot[0].ROffAmt < 0)
                //{
                //    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}

                //tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}

                //// ****************************************************************
                //netAmount = lstQuot[0].NetAmt;
                //PdfPTable tblAmount1 = new PdfPTable(2);
                //int[] column_tblAmount1 = { 60, 40 };
                //tblAmount1.SetWidths(column_tblAmount1);
                //string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                //tblAmount1.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount1.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //// -----------------------------------------------
                //if (sumDis > 0)
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //}
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-                
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);

                //tblFootDetail.AddCell(pdf.setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                //tblFootDetail.AddCell(pdf.setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo + "        " + "PAN No  : " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstBank.Count > 0) ? lstBank[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstBank.Count > 0) ? lstBank[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstBank.Count > 0) ? lstBank[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstBank.Count > 0) ? lstBank[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO RAJKOT JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GeneratePurchaseBill_Gautam(Int64 pkID)
        {
            //HttpContext.Current.Session["PrintHeader"] = "yes";

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
            Int64 ProdDetail_Lines = 20;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "PurchaseBill");
            //ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (lstPrinter.Count > 0)
            {
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
            }


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
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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
                int[] column_tblMember = { 30, 25, 20, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].Address.TrimStart('.') + lstCust[0].Area.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.TrimStart('.') + lstCust[0].StateName.TrimStart('.') + lstCust[0].Pincode.TrimStart('.')))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName.TrimStart('.') + " - " + lstCust[0].Pincode + ", " + lstCust[0].StateName.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(2);
                int[] column_tblNested2 = { 30,70};
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + ((dtContact.Rows.Count > 0) ? dtContact.Rows[0]["ContactPerson1"].ToString() : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell("Executive Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 13, 30, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 13, 36, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    else
                    {
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblDetail.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                        //else
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    }
                    // ----------------------------------------
                    //List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    //lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].QuotationNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    //if (lstSpec.Count > 0)
                    //{
                    //    string tmpSpec = lstSpec[0].MaterialSpec.ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        if (sumDis > 0)
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        else
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    //        totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                    //    }
                    //}
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
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
                tblTNC.AddCell(pdf.setCell("Terms & Conditions : ", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //---------------------------------------------------------------------------------------------------------
                PdfPTable tblAmount = new PdfPTable(2);
                int[] column_tblAmount = { 60, 40 };
                tblAmount.SetWidths(column_tblAmount);
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                totAmount = lstQuot[0].BasicAmt;
                totRNDOff = lstQuot[0].ROffAmt;
                totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                    tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    befAmt += lstQuot[0].ChargeAmt1;
                    befGST += lstQuot[0].ChargeGSTAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                   if (lstQuot[0].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //else
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                    else
                    {
                        tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //else
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                        tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //else
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                

                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    aftAmt += lstQuot[0].ChargeAmt4;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                    aftAmt += lstQuot[0].ChargeAmt5;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0 || lstQuot[0].ROffAmt < 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }

                // ****************************************************************
                netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount1.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                    tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                // -----------------------------------------------
                if (sumDis > 0)
                {
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    //else
                        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    //else
                        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

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

                //tblFootDetail.AddCell(pdf.setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                //tblFootDetail.AddCell(pdf.setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo + "        " + "PAN No  : " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstBank.Count > 0) ? lstBank[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstBank.Count > 0) ? lstBank[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstBank.Count > 0) ? lstBank[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstBank.Count > 0) ? lstBank[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName,6), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GeneratePurchaseBill_Dishachi(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "no";

            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(2);
            PdfPTable tblDetail = new PdfPTable(12);
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
            Int64 ProdDetail_Lines = 20;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "PurchaseBill");
            //ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (lstPrinter.Count > 0)
            {
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
            }


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
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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
                int[] column_tblMember = { 60, 40 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblBuyerD = new PdfPTable(2);
                int[] column_tblBuyerD = { 15, 85 };
                tblBuyerD.SetWidths(column_tblBuyerD);

                tblBuyerD.AddCell(pdf.setCell("M/s.       : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBuyerD.AddCell(pdf.setCell(lstCust[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBuyerD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].Address.Trim() + lstCust[0].Area.Trim()))
                    tblBuyerD.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblBuyerD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBuyerD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].CityName.Trim() + lstCust[0].Pincode.Trim()))
                    tblBuyerD.AddCell(pdf.setCell(lstCust[0].CityName + "," + lstCust[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblBuyerD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].GSTNo))
                {
                    tblBuyerD.AddCell(pdf.setCell("GST No : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblBuyerD.AddCell(pdf.setCell(lstCust[0].GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
                // --------------------------------------------------------
                PdfPTable tblOrderD = new PdfPTable(2);
                int[] column_tblOrderD = { 45, 55 };
                tblOrderD.SetWidths(column_tblOrderD);

                tblOrderD.AddCell(pdf.setCell("Order No.", pdf.GrayBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell("Date", pdf.GrayBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblOrderD.AddCell(pdf.setCell("Reference Bill No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].BillNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell("Transport", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].TransporterName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell("Mode Of Transport", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].ModeOfTransport, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell("L.R. No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].LRNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell("L.R. Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOrderD.AddCell(pdf.setCell(": " + lstQuot[0].LRDate.ToString("dd-MM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

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
                //else
                //{
                //    tblBanner.AddCell(pdf.setCell("For " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //}
                //----------------------------------------------------------------


                //string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                tblMember.AddCell(pdf.setCell(tblBanner, pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblBuyerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblOrderD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                //tblMember.AddCell(pdf.setCell("We acknowledge with thanks the receipt of your above started inquiry. We are pleased to offer our most competitive rates as", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));

                if (sumDis > 0)
                {
                    int[] column_tblNested = { 3, 28, 8, 5, 7, 7, 5, 9, 5, 7, 7, 9 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {

                    int[] column_tblNested = { 3, 28, 8, 5, 7, 7, 5, 9, 5, 7, 7, 9 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr." + "\n" + "No", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblDetail.AddCell(pdf.setCell("Product Name", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblDetail.AddCell(pdf.setCell("HSN/SAC" + "\n" + "CODE", pdf.WhiteBaseColor, pdf.fnCalibriBold5, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Discount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("Taxable" + "\n" + "Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("GST%", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("Tax Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));

                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Taxable Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("GST %", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("Tax Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                    tblDetail.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                }

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                    if (lstQuot[0].IGSTAmt == 0)
                    {
                        tblDetail.AddCell(pdf.setCell("Crental", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                        tblDetail.AddCell(pdf.setCell("State/UT", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell("Integrated", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    }
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));

                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                    if (lstQuot[0].IGSTAmt == 0)
                    {
                        tblDetail.AddCell(pdf.setCell("Crental", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                        tblDetail.AddCell(pdf.setCell("State/UT", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell("Integrated", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    }
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                }

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;
                decimal cgst = 0, sgst = 0, igst = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    decimal tmpAmount = (Convert.ToDecimal(dtItem.Rows[i]["Quantity"]) * Convert.ToDecimal(dtItem.Rows[i]["NetRate"]));
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += tmpAmount;
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmt"]);
                    cgst += Convert.ToDecimal(dtItem.Rows[i]["CGSTAmt"]);
                    sgst += Convert.ToDecimal(dtItem.Rows[i]["SGSTAmt"]);
                    igst += Convert.ToDecimal(dtItem.Rows[i]["IGSTAmt"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["HSNCode"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));


                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(tmpAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (Convert.ToDecimal(dtItem.Rows[i]["CGSTAmt"]) + Convert.ToDecimal(dtItem.Rows[i]["SGSTAmt"]) != 0)
                        {
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["CGSTAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["SGSTAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["IGSTAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        }
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));

                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(tmpAmount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (Convert.ToDecimal(dtItem.Rows[i]["CGSTAmt"]) + Convert.ToDecimal(dtItem.Rows[i]["SGSTAmt"]) != 0)
                        {
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["CGSTAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["SGSTAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["IGSTAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        }
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));

                    }
                }


                Int64 totalRowCount = 0;
                if (ProdDetail_Lines > dtItem.Rows.Count + totalRowCount)
                {
                    for (int i = 1; i <= (ProdDetail_Lines - dtItem.Rows.Count); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (sumDis > 0)
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            if (lstQuot[0].IGSTAmt == 0)
                            {
                                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            }
                            else
                            {
                                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                            }
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            if (lstQuot[0].IGSTAmt == 0)
                            {
                                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            }
                            else
                            {
                                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                            }
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                    }
                }

                //----------------------------------------------
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblDetail.AddCell(pdf.setCell("Total", pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 11));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(taxAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                    tblDetail.AddCell(pdf.setCell(taxAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                if (lstQuot[0].IGSTAmt == 0)
                {
                    tblDetail.AddCell(pdf.setCell(cgst.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(sgst.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(igst.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                tblDetail.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));


                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(1);
                int[] column_tblTNC = { 100 };
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions :", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell("GSTIN No.:  ", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(objAuth.GSTNo, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


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
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */

                //if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                //{
                //    if (lstQuot[0].IGSTAmt > 0)
                //    {
                //        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //    else
                //    {
                //        if ((lstQuot[0].CGSTAmt + lstQuot[0].SGSTAmt) > 0)
                //        {
                //            tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            else
                //                tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //            tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //            else
                //                tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        }
                //    }
                //}

                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt4;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt5;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                    //    tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 6, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}

                // ****************************************************************
                //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
                netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 20, 80 };
                tblAmount1.SetWidths(column_tblAmount1);
                decimal totalTax = 0;
                if (lstQuot[0].IGSTAmt == 0)
                {
                    totalTax = cgst + sgst;
                }
                else
                {
                    totalTax = igst;
                }
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                string GSTAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)totalTax);
                tblAmount1.AddCell(pdf.setCell("Total GST : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell(GSTAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell("Bill Amount : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell(NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));

                PdfPTable tblAmount2 = new PdfPTable(2);
                int[] column_tblAmount2 = { 60, 40 };
                tblAmount2.SetWidths(column_tblAmount2);
                if (lstQuot[0].ROffAmt != 0)
                {
                    tblAmount2.AddCell(pdf.setCell("Round Off", pdf.GrayBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 0));
                    tblAmount2.AddCell(pdf.setCell(lstQuot[0].ROffAmt.ToString("0.00"), pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf6, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 11));
                }
                else
                {
                    tblAmount2.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 0));
                }
                tblAmount2.AddCell(pdf.setCell("Grand Total", pdf.GrayBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblAmount2.AddCell(pdf.setCell(lstQuot[0].NetAmt.ToString("0.00"), pdf.GrayBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 11));


                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount2, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount2, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 5, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
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
                if (lstBank.Count > 0)
                {
                    phrase1.Add(new Chunk(lstQuot[0].BankName + " - Branch : " + lstQuot[0].BranchName, pdf.fnCalibri8));
                    phrase1.Add(new Chunk(", A/c No : " + lstQuot[0].BankAccountNo + ", IFSC: " + lstQuot[0].BankIFSC, pdf.fnCalibri8));
                }

                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell(phrase1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount1 = 0;
                string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";

                if (File.Exists(tmpFile1))
                {
                    if (File.Exists(tmpFile1))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        //int[] column_tblSign = { 30 };
                        //tblSign.SetWidths(column_tblSign);

                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile1);
                        eSign.ScaleAbsolute(80, 50);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount1 = fileCount1 + 1;
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
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblFooter.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 3), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                //tblSignOff.AddCell(pdf.setCell("We hope you will find above rate in line with your requirement. We assure you of our best services with maximum technical", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                //tblSignOff.AddCell(pdf.setCell("supports at all times", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                //tblSignOff.AddCell(pdf.setCell("Thanking you and awaiting for your valed offer.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                //tblSignOff.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                #endregion
            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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
        public static void GeneratePurchaseBill_General(Int64 pkID)
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
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(30, 30, 40, 0);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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

                tblNested20.AddCell(pdf.setCell("Spplier / Vendor :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].Address.Trim() + lstCust[0].Area.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.Trim() + lstCust[0].Pincode.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName + "," + lstCust[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                //    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                //    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 30, 40, 10, 20 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("O.A. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " , pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("P.O. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": Verbally", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                //tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                tblNested2.AddCell(pdf.setCell("Transport " + lstQuot[0].TransporterName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                //  Defining : Company Master Information
                // -------------------------------------------------------------------------------------
                List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 10000, out totrec);

                PdfPTable tblNested202 = new PdfPTable(1);
                int[] column_tblNested202 = { 100 };
                tblNested202.SetWidths(column_tblNested202);

                tblNested202.AddCell(pdf.setCell("Company :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].OrgName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].CityName + " - " + lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].StateName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell("Phone : " + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblNested22 = new PdfPTable(4);
                int[] column_tblNested22 = { 30, 40, 10, 20 };
                tblNested22.SetWidths(column_tblNested22);
                tblNested22.AddCell(pdf.setCell("Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell(": " + "", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell("Buyer's GSTIN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell(": " + lstCust[0].GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));

                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
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
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(tmpProdAlias, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 4));
                    if (objAuth.CompanyName.ToUpper().Contains("DALIA MOTORS"))
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductNameLong"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    }
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Qty"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Rate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
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
                for (int i = 1; i < (13 - dtItem.Rows.Count); i++)
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
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(1);
                int[] column_tblTNC = { 100 };
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblTNC.AddCell(pdf.setCell("GSTIN No : 24BIQPK4338E1Z8 ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
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

                //if (lstQuot[0].IGSTAmt > 0)
                //{
                //    tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //else
                //{
                //    tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //    tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
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
                tblAmount.AddCell(pdf.setCell(pdf.Table_MultipleGST(lstQuot[0].InvoiceNo,"purchasebill", column_tblAmount,pdf.fnCalibriBold8,pdf.fnCalibri8), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

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


                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstQuot.Count > 0) ? lstQuot[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstQuot.Count > 0) ? lstQuot[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstQuot.Count > 0) ? lstQuot[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("SWIFT Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankSwift : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount1 = 0;
                string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile1))
                {
                    if (File.Exists(tmpFile1))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile1);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount1 = fileCount1 + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GeneratePurchaseBill_HiTech(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "no";
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
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(30, 30, 40, 0);
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

            //----------------------------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.PurchaseBill> lstQuot = new List<Entity.PurchaseBill>();
            lstQuot = BAL.PurchaseBillMgmt.GetPurchaseBillList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.PurchaseBillMgmt.GetPurchaseBillDetail(lstQuot[0].InvoiceNo);
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
                int[] column_tblMember = { 20, 20, 33, 27 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                ////--------------------Header Image--------------------------//
                //PdfPTable tblLocation = new PdfPTable(1);
                //int[] column_tblLocation = { 100 };
                //tblLocation.SetWidths(column_tblLocation);
                //int fileCount1 = 0;
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\headerforall.jpg";


                //if (File.Exists(tmpFile1))
                //{
                //    if (File.Exists(tmpFile1))   //Signature print
                //    {
                //        PdfPTable tblSymbol = new PdfPTable(1);
                //        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                //        eLoc.ScaleAbsolute(530, 80);


                //        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                //        tblLocation.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //        fileCount1 = fileCount1 + 1;
                //    }
                //}


                //-----------------------Hitech Logo---------------------------------------
                PdfPTable tblLogo = new PdfPTable(1);
                int[] column_tblLogo = { 100 };
                tblLogo.SetWidths(column_tblLogo);
                int fileCount1 = 0;
                string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\Hitech.png";


                if (File.Exists(tmpFile1))
                {
                    if (File.Exists(tmpFile1))   //Signature print
                    {
                        PdfPTable tblSymbol = new PdfPTable(1);
                        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                        eLoc.ScaleAbsolute(211, 73);


                        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                        tblLogo.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount1 = fileCount1 + 1;
                    }
                }
                //------------------------------------Location Image-----------------------------
                PdfPTable tblLocation = new PdfPTable(1);
                int[] column_tblLocation = { 100 };
                tblLocation.SetWidths(column_tblLocation);
                int fileCount2 = 0;
                string tmpFile2 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/") + "\\location.png";


                if (File.Exists(tmpFile2))
                {
                    if (File.Exists(tmpFile2))   //Signature print
                    {
                        PdfPTable tblSymbol = new PdfPTable(1);
                        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile2);
                        eLoc.ScaleAbsolute(86, 35);


                        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                        tblLocation.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount2 = fileCount2 + 1;
                    }
                }

                PdfPTable tblName = new PdfPTable(1);
                int[] column_tblName = { 100 };
                tblName.SetWidths(column_tblName);

                //tblName.AddCell(pdf.setCell("HI-TECH AVENUE", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblName.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].Address.TrimStart('.')))
                    tblName.AddCell(pdf.setCell(lstOrg[0].Address.ToUpper() + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].CityName.TrimStart('.') + lstOrg[0].StateName.TrimStart('.') + lstOrg[0].Pincode.TrimStart('.')))
                    tblName.AddCell(pdf.setCell(lstOrg[0].CityName.ToUpper().TrimStart('.') + " - " + lstOrg[0].Pincode + ", " + lstOrg[0].StateName.ToUpper().Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblName.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblAddress = new PdfPTable(2);
                int[] column_tblAddress = { 32, 68 };
                tblAddress.SetWidths(column_tblAddress);

                tblAddress.AddCell(pdf.setCell(tblLocation, pdf.WhiteBaseColor, pdf.fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(tblName, pdf.WhiteBaseColor, pdf.fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(" GST NO : 24AAMFH4110Q1Z7", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("MOBLIE NO :- +91 90990 67240, +91 70969 61311, T-(079) 25855240", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("EMAIL :- info@hi-techscrewbarrel.com, dpshitech@gmail.com", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));






                tblNested20.AddCell(pdf.setCell("Supplier / Vendor :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].Address.Trim() + lstCust[0].Area.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.Trim() + lstCust[0].Pincode.Trim()))
                    tblNested20.AddCell(pdf.setCell(lstCust[0].CityName + "," + lstCust[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNested20.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                //    tblNested20.AddCell(pdf.setCell("Contact No: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //if (!String.IsNullOrEmpty(lstCust[0].EmailAddress))
                //    tblNested20.AddCell(pdf.setCell("Email     : " + lstCust[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 30, 40, 10, 20 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Invoice No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].InvoiceDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("P.O. No." , pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].BillNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell("Transport " , pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].TransporterName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                // -------------------------------------------------------------------------------------
                //  Defining : Company Master Information
                // -------------------------------------------------------------------------------------
               

                PdfPTable tblNested202 = new PdfPTable(1);
                int[] column_tblNested202 = { 100 };
                tblNested202.SetWidths(column_tblNested202);

                tblNested202.AddCell(pdf.setCell("Company :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].OrgName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].CityName + " - " + lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell(lstOrg[0].StateName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested202.AddCell(pdf.setCell("Phone : " + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblNested22 = new PdfPTable(4);
                int[] column_tblNested22 = { 30, 40, 10, 20 };
                tblNested22.SetWidths(column_tblNested22);
                tblNested22.AddCell(pdf.setCell("Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell(": " + lstQuot[0].TerminationOfDeliery, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblNested22.AddCell(pdf.setCell("Buyer's GSTIN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblNested22.AddCell(pdf.setCell(": " + lstCust[0].GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));

                tblMember.AddCell(pdf.setCell("Purchase Bill", pdf.WhiteBaseColor, pdf.fnCalibriBold20, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                //tblMember.AddCell(pdf.setCell(tblLocation, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblLogo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblAddress, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
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
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(tmpProdAlias, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 4));
                    if (objAuth.CompanyName.ToUpper().Contains("DALIA MOTORS"))
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductNameLong"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    }
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Qty"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Rate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
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
                for (int i = 1; i < (13 - dtItem.Rows.Count); i++)
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
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(1);
                int[] column_tblTNC = { 100 };
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell("GSTIN No : " + lstOrg[0].GSTIN, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 0));
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
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
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

                if (lstQuot[0].IGSTAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                else
                {
                    tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                    tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                //tblAmount.AddCell(setCell("Total     :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                //tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST)).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));

                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt4;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt5;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                    //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //else
                        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount.AddCell(pdf.setCell("Grand Total  :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                    tblAmount.AddCell(pdf.setCell(((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

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


                tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstQuot.Count > 0) ? lstQuot[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstQuot.Count > 0) ? lstQuot[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstQuot.Count > 0) ? lstQuot[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount3 = 0;
                string tmpFile3 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile3))
                {
                    if (File.Exists(tmpFile3))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile3);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount3 = fileCount3 + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName,5), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 15));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                //tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].InvoiceNo.Replace("/", "-").ToString() + ".pdf";
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

        protected void rdblOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPOInward();
        }

        public void BindPOInward()
        {
            if (rdblOption.SelectedValue == "GRN")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    List<Entity.Inward> lstEntity = new List<Entity.Inward>();
                    lstEntity = BAL.InwardMgmt.GetInwardListByCustomer(Convert.ToInt64(hdnCustomerID.Value), 0, Session["LoginUserID"].ToString(), 0, 0);
                    var tttEntity = lstEntity.GroupBy(x => x.InwardNo).Select(y => y.First());
                    drpReferenceNo.DataValueField = "InwardNo";
                    drpReferenceNo.DataTextField = "InwardNo";
                    if (lstEntity.Count > 0)
                    {
                        drpReferenceNo.DataSource = tttEntity;
                        drpReferenceNo.DataBind();
                    }
                }
            }
            else if (rdblOption.SelectedValue == "PO")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    List<Entity.PurchaseOrder> lstEntity = new List<Entity.PurchaseOrder>();
                    lstEntity = BAL.PurchaseOrderMgmt.GetPurchaseOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), "", 0, 0);
                    var tttEntity = lstEntity.GroupBy(x => x.OrderNo).Select(y => y.First());
                    drpReferenceNo.DataValueField = "OrderNo";
                    drpReferenceNo.DataTextField = "OrderNo";
                    if (lstEntity.Count > 0)
                    {
                        drpReferenceNo.DataSource = tttEntity;
                        drpReferenceNo.DataBind();
                    }
                }
            }
        }

        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnSelectedReference.Value))
            {
                

                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                dtDetail.Rows.Clear();
                // ========================================================
                string[] values = hdnSelectedReference.Value.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    string tmpVal = values[i];
                    if (!String.IsNullOrEmpty(tmpVal))
                    {
                        DataTable dtTable = new DataTable();
                        if (rdblOption.SelectedValue == "GRN")
                            dtTable = BAL.InwardMgmt.GetInwardDetail(tmpVal);
                        else
                            dtTable = BAL.PurchaseOrderMgmt.GetPurchaseOrderDetail(tmpVal);

                        foreach (DataRow myrow in dtTable.Rows)
                        {
                            DataRow dr = dtDetail.NewRow();
                            dr["pkID"] = 0;
                            dr["InvoiceNo"] = txtInvoiceNo.Text;
                            dr["ProductID"] = myrow["ProductID"].ToString();
                            dr["ProductName"] = myrow["ProductName"].ToString();
                            dr["ProductGroupName"] = myrow["ProductGroupName"].ToString();
                            dr["ProductNameLong"] = myrow["ProductNameLong"].ToString();
                            dr["Qty"] = Convert.ToDecimal(myrow["Quantity"].ToString());
                            dr["Unit"] = myrow["Unit"].ToString();
                            dr["Rate"] = Convert.ToDecimal(myrow["UnitRate"].ToString());
                            dr["DiscountPer"] = Convert.ToDecimal(myrow["DiscountPercent"].ToString());
                            dr["DiscountAmt"] = (!String.IsNullOrEmpty(myrow["DiscountAmt"].ToString())) ? Convert.ToDecimal(myrow["DiscountAmt"].ToString()) : 0;
                            dr["NetRate"] = (!String.IsNullOrEmpty(myrow["NetRate"].ToString())) ? Convert.ToDecimal(myrow["NetRate"].ToString()) : 0;
                            dr["Amount"] = (!String.IsNullOrEmpty(myrow["Amount"].ToString())) ? Convert.ToDecimal(myrow["Amount"].ToString()) : 0;
                            dr["CGSTPer"] = (!String.IsNullOrEmpty(myrow["CGSTPer"].ToString())) ? Convert.ToDecimal(myrow["CGSTPer"].ToString()) : 0;
                            dr["SGSTPer"] = (!String.IsNullOrEmpty(myrow["SGSTPer"].ToString())) ? Convert.ToDecimal(myrow["SGSTPer"].ToString()) : 0;
                            dr["IGSTPer"] = (!String.IsNullOrEmpty(myrow["IGSTPer"].ToString())) ? Convert.ToDecimal(myrow["IGSTPer"].ToString()) : 0;
                            dr["CGSTAmt"] = (!String.IsNullOrEmpty(myrow["CGSTAmt"].ToString())) ? Convert.ToDecimal(myrow["CGSTAmt"].ToString()) : 0;
                            dr["SGSTAmt"] = (!String.IsNullOrEmpty(myrow["SGSTAmt"].ToString())) ? Convert.ToDecimal(myrow["SGSTAmt"].ToString()) : 0;
                            dr["IGSTAmt"] = (!String.IsNullOrEmpty(myrow["IGSTAmt"].ToString())) ? Convert.ToDecimal(myrow["IGSTAmt"].ToString()) : 0;
                            dr["TaxType"] = (!String.IsNullOrEmpty(myrow["TaxType"].ToString())) ? Convert.ToInt64(myrow["TaxType"].ToString()) : 0;
                            dr["TaxRate"] = (!String.IsNullOrEmpty(myrow["TaxRate"].ToString())) ? Convert.ToDecimal(myrow["TaxRate"].ToString()) : 0;
                            dr["TaxAmount"] = (!String.IsNullOrEmpty(myrow["TaxAmount"].ToString())) ? Convert.ToDecimal(myrow["TaxAmount"].ToString()) : 0;
                            if (rdblOption.SelectedValue == "GRN")
                            {
                                dr["AddTaxPer"] = 0;
                                dr["AddTaxAmt"] = 0;
                                dr["HeaderDiscAmt"] = 0;
                                dr["NetAmt"] = (!String.IsNullOrEmpty(myrow["NetAmount"].ToString())) ? Convert.ToDecimal(myrow["NetAmount"].ToString()) : 0;
                                dr["OrderNo"] = myrow["InwardNo"].ToString();
                            }
                            else
                            {
                                dr["AddTaxPer"] = (!String.IsNullOrEmpty(myrow["AddTaxPer"].ToString())) ? Convert.ToDecimal(myrow["AddTaxPer"].ToString()) : 0;
                                dr["AddTaxAmt"] = (!String.IsNullOrEmpty(myrow["AddTaxAmt"].ToString())) ? Convert.ToDecimal(myrow["AddTaxAmt"].ToString()) : 0;
                                dr["HeaderDiscAmt"] = (!String.IsNullOrEmpty(myrow["HeaderDiscAmt"].ToString())) ? Convert.ToDecimal(myrow["HeaderDiscAmt"].ToString()) : 0;
                                dr["NetAmt"] = (!String.IsNullOrEmpty(myrow["NetAmt"].ToString())) ? Convert.ToDecimal(myrow["NetAmt"].ToString()) : 0;
                                dr["OrderNo"] = myrow["OrderNo"].ToString();
                            }
                            
                            
                            
                            dtDetail.Rows.Add(dr);

                        }
                        dtDetail.AcceptChanges();
                        Session.Add("dtDetail", dtDetail);

                        rptOrderDetail.DataSource = dtDetail;
                        rptOrderDetail.DataBind();
                    }
                }
            }
        }

        protected void txtCRDays_TextChanged(object sender, EventArgs e)
        {
            calculateDueDate();
        }

        protected void txtInvoiceDate_TextChanged(object sender, EventArgs e)
        {
            calculateDueDate();
        }
        void calculateDueDate()
        {
            if (txtInvoiceDate.Text != "" && txtCRDays.Text != "")
            {
                txtDueDate.ReadOnly = false;
                txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(Convert.ToInt64(txtCRDays.Text)).ToString("yyyy-MM-dd");
                txtDueDate.ReadOnly = true;
            }
                
        }
    }
}