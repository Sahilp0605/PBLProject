using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
namespace StarsProject
{
    public partial class Inward : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount, totDiscAmt, totAddTaxAmt, totSGST, totCGST, totIGST;
        private static DataTable dtDetail;

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

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------
                Session.Remove("dtOrderProduct");
                Session.Remove("dtDetail");
                // --------------------------------------------
                BindDropDown();
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
                hdnSerialKey.Value = Session["SerialKey"].ToString().Replace("\r\n","");
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
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];

                //if (requestTarget.ToLower() == "drpquotation")
                //{
                //    if ((hdnpkID.Value == "0" || hdnpkID.Value == ""))
                //    {
                //        dtDetail.Clear();
                //        //dtDetail = BAL.QuotationDetailMgmt.GetQuotationProductForSalesOrder(drpQuotation.SelectedValue, txtOrderNo.Text);
                //        rptInwardDetail.DataSource = dtDetail;
                //        rptInwardDetail.DataBind();
                //    }
                //}
            }
        }

        public void OnlyViewControls()
        {
            txtInwardNo.ReadOnly = true;
            txtInwardDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtTotBasicAmt.ReadOnly = true;
            txtTotGST.ReadOnly = true;
            txtRoff.ReadOnly = true;
            txtTotNetAmt.ReadOnly = true;
            txtTotAddTaxAmt.ReadOnly = true;
            drpLocation.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            //---------------------------Location Details-------------------------------
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            lstLocation = BAL.CommonMgmt.GetLocationList();
            drpLocation.DataSource = lstLocation;
            drpLocation.DataValueField = "LocationID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            // ---------------- Report To List -------------------------------------
            //List<Entity.Customer> lstOrgDept2 = new List<Entity.Customer>();
            //lstOrgDept2 = BAL.CustomerMgmt.GetCustomerList();
            //drpCustomer.DataSource = lstOrgDept2;
            //drpCustomer.DataValueField = "CustomerID";
            //drpCustomer.DataTextField = "CustomerName";
            //drpCustomer.DataBind();
            //drpCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));

            // ---------------- Report To List -------------------------------------
            //List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            //lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            //drpTNC.DataSource = lstList2;
            //drpTNC.DataValueField = "pkID";
            //drpTNC.DataTextField = "TNC_Header";
            //drpTNC.DataBind();
            //drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

            // ---------------- Quotation List -------------------------------------
            // BindQuotationList(0);
        }

        public void BindPurchaseOrderList(string pModule, Int64 pCustomerID)
        {
            drpReferenceNo.Items.Clear();
            if (pModule == "PurchaseOrder" && pCustomerID != 0)
            {
                List<Entity.PurchaseOrder> lstEntity = new List<Entity.PurchaseOrder>();
                lstEntity = BAL.PurchaseOrderMgmt.GetPurchaseOrderListByCust(pCustomerID);
                drpReferenceNo.DataValueField = "OrderNo";
                drpReferenceNo.DataTextField = "OrderNo";
                if (lstEntity.Count > 0)
                {
                    drpReferenceNo.DataSource = lstEntity;
                    drpReferenceNo.DataBind();
                }
            }
            drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }

        public void BindPurchaseOrderProductList()
        {
            lstOrderProduct.Items.Clear();
            if (!String.IsNullOrEmpty(hdnSelectedReference.Value))
            {
                string selectedOrder = string.Empty;
                string[] values = hdnSelectedReference.Value.Split(',');
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        string tmpVal = values[i];
                        selectedOrder += string.IsNullOrEmpty(selectedOrder) ? "'" + values[i] + "'" : ",'" + values[i] + "'";
                    }

                    DataTable dtProduct = new DataTable();
                    dtProduct = BAL.PurchaseOrderMgmt.GetPurchaseOrderDetailForInward(selectedOrder);
                    if (dtProduct.Rows.Count > 0)
                    {
                        lstOrderProduct.DataValueField = "OrdProductID";
                        lstOrderProduct.DataTextField = "DisplayProductName";
                        lstOrderProduct.DataSource = dtProduct;
                        lstOrderProduct.DataBind();
                        Session.Add("dtOrderProduct", dtProduct);
                    }
                }
            }
        }


        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        public void BindInwardDetailList(string pInwardNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.InwardMgmt.GetInwardDetail(pInwardNo);
            rptInwardDetail.DataSource = dtDetail1;
            rptInwardDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void rptInwardDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;


                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                            strErr += "<li>" + "Quantity is required." + "</li>";

                        //if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                        //    strErr += "<li>" + "Unit Rate is required." + "</li>";

                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        if (dtDetail != null)
                        {
                            //----Check For Duplicate Item----//
                            string find = "";
                            HiddenField fld1 = ((HiddenField)e.Item.FindControl("hdnProductID"));
                            DropDownList fld2 = ((DropDownList)e.Item.FindControl("drpForOrderNo"));
                            if (!String.IsNullOrEmpty(fld1.Value))
                                find = "ProductID = " + fld1.Value;

                            if (!String.IsNullOrEmpty(fld2.SelectedValue))
                                find += " And OrderNo = '" + ((DropDownList)e.Item.FindControl("drpForOrderNo")).SelectedValue + "'";

                            DataRow[] foundRows = dtDetail.Select(find);
                            if (foundRows.Length > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                                return;
                            }

                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = 0;
                            //string icode = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedValue;
                            //string iname = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedItem.Text;
                            string taxtype = ((HiddenField)e.Item.FindControl("hdnTaxType")).Value;
                            string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
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
                            string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;

                            string cgstper = ((HiddenField)e.Item.FindControl("hdnCGSTPer")).Value;
                            string sgstper = ((HiddenField)e.Item.FindControl("hdnSGSTPer")).Value;
                            string igstper = ((HiddenField)e.Item.FindControl("hdnIGSTPer")).Value;

                            string cgstamt = ((HiddenField)e.Item.FindControl("hdnCGSTAmt")).Value;
                            string sgstamt = ((HiddenField)e.Item.FindControl("hdnSGSTAmt")).Value;
                            string igstamt = ((HiddenField)e.Item.FindControl("hdnIGSTAmt")).Value;
                            string fororderno = ((DropDownList)e.Item.FindControl("drpForOrderNo")).SelectedValue;

                            dr["InwardNo"] = txtInwardNo.Text;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
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
                            dr["TaxType"] = (!String.IsNullOrEmpty(taxtype)) ? Convert.ToInt64(taxtype) : 1;
                            dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
                            dr["TaxAmount"] = (!String.IsNullOrEmpty(taxamt)) ? Convert.ToDecimal(taxamt) : 0;
                            dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                            dr["OrderNo"] = (!String.IsNullOrEmpty(fororderno)) ? fororderno : "";
                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptInwardDetail.DataSource = dtDetail;
                            rptInwardDetail.DataBind();
                            //----------------------------------------------------------------
                            Session.Add("dtDetail", dtDetail);

                        }
                    }
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                }
            }
            if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                Int64 tmpRow;

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

                rptInwardDetail.DataSource = dtDetail;
                rptInwardDetail.DataBind();

                Session.Add("dtDetail", dtDetail);
            }
        }
        protected void rptInwardDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2, v3, v4, v5, v6, v7, v8;
                //v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "DiscountAmt"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Amount"));
                v3 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaxAmount"));
                //v4 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "AddTaxAmt"));
                v5 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmount"));

                v6 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "CGSTAmt"));
                v7 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "SGSTAmt"));
                v8 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "IGSTAmt"));

                //totDiscAmt += v1;
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
                //ViewState["totDiscAmt"] = (!String.IsNullOrEmpty(totDiscAmt.ToString())) ? Convert.ToDecimal(totDiscAmt) : 0;
                //ViewState["totAddTaxAmt"] = (!String.IsNullOrEmpty(totAddTaxAmt.ToString())) ? Convert.ToDecimal(totAddTaxAmt) : 0;
                ViewState["totSGST"] = (!String.IsNullOrEmpty(totSGST.ToString())) ? Convert.ToDecimal(totSGST) : 0;
                ViewState["totCGST"] = (!String.IsNullOrEmpty(totCGST.ToString())) ? Convert.ToDecimal(totCGST) : 0;
                ViewState["totIGST"] = (!String.IsNullOrEmpty(totIGST.ToString())) ? Convert.ToDecimal(totIGST) : 0;

            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                //    //Label lblTotalDiscAmt = (Label)e.Item.FindControl("lblTotalDiscAmt");
                //    //Label lblTotalAmt = (Label)e.Item.FindControl("lblTotalAmt");
                //    //Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
                //    //Label lblAddTaxAmt = (Label)e.Item.FindControl("lblAddTaxAmt");
                //    //Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");


                //    //lblTotalDiscAmt.Text = totDiscAmt.ToString("0.00");
                //    //lblTotalAmt.Text = totAmount.ToString("0.00");
                //    //lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
                //    //lblAddTaxAmt.Text = totAddTaxAmt.ToString("0.00");
                //    //lblTotalNetAmount.Text = totNetAmount.ToString("0.00");

                funCalculateTotal();

            }
        }
        // ----------------------------------------------------------------------------------
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Inward> lstEntity = new List<Entity.Inward>();
                // ----------------------------------------------------
                lstEntity = BAL.InwardMgmt.GetInwardList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtInwardNo.Text = lstEntity[0].InwardNo;
                //txtInwardDate.Text = lstEntity[0].InwardDate.ToString("dd-MM-yyyy");
                txtInwardDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].InwardDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpLocation.SelectedValue = lstEntity[0].LocationID.ToString();

                txtTotBasicAmt.Text = lstEntity[0].BasicAmt.ToString();
                txtTotGST.Text = lstEntity[0].BasicAmt.ToString();
                txtRoff.Text = lstEntity[0].ROffAmt.ToString();
                txtTotNetAmt.Text = lstEntity[0].NetAmt.ToString();

                drpModeOfTransport.SelectedValue = lstEntity[0].ModeOfTransport;
                txtTransporterName.Text = lstEntity[0].TransporterName;
                txtVehicleNo.Text = lstEntity[0].VehicleNo;
                txtLRNo.Text = lstEntity[0].LRNo;
                txtLRDate.Text = lstEntity[0].LRDate.ToString("yyyy-MM-dd");
                txtTransportRemark.Text = lstEntity[0].TransportRemark;
                txtManualInwardNo.Text = lstEntity[0].ManuaLInwardNo;
                // ------------------------------------------------------------
                //BindQuotationList(lstEntity[0].CustomerID);
                // ------------------------------------------------------------
                //txtTermsCondition.Text = lstEntity[0].TermsCondition;
                //drpQuotation.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].QuotationNo)) ? lstEntity[0].QuotationNo : "";
                //drpEmployee.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].EmployeeID)) ? lstEntity[0].EmployeeID : "";
                // -------------------------------------------------------------------------
                BindInwardDetailList(lstEntity[0].InwardNo);
                txtInwardDate.Focus();
            }
            // ------------------------------------------------------
            txtCustomerName.Enabled = (pMode.ToLower() == "add") ? true : false;
            txtInwardDate.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            //----------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnInwardNo = "";

            string strErr = "";
            _pageValid = true;


            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(txtInwardDate.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtInwardDate.Text))
                    strErr += "<li>" + "Inward Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Selection is required." + "</li>";
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtInwardDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtInwardDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Job Card Inward Date is Not Valid." + "</li>";
                }
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        Entity.Inward objEntity = new Entity.Inward();

                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        Int64 intLocation = (!String.IsNullOrEmpty(drpLocation.SelectedValue)) ? Convert.ToInt64(drpLocation.SelectedValue) : 0;
                        objEntity.InwardNo = txtInwardNo.Text;
                        objEntity.InwardDate = Convert.ToDateTime(txtInwardDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.LocationID = intLocation;
                        objEntity.BasicAmt = (!String.IsNullOrEmpty(txtTotBasicAmt.Text)) ? Convert.ToDecimal(txtTotBasicAmt.Text) : 0;
                        objEntity.CGSTAmt = (!String.IsNullOrEmpty(hdnTotCGSTAmt.Value)) ? Convert.ToDecimal(hdnTotCGSTAmt.Value) : 0; ;
                        objEntity.SGSTAmt = (!String.IsNullOrEmpty(hdnTotSGSTAmt.Value)) ? Convert.ToDecimal(hdnTotSGSTAmt.Value) : 0;
                        objEntity.IGSTAmt = (!String.IsNullOrEmpty(hdnTotIGSTAmt.Value)) ? Convert.ToDecimal(hdnTotIGSTAmt.Value) : 0;
                        objEntity.ROffAmt = (!String.IsNullOrEmpty(txtRoff.Text)) ? Convert.ToDecimal(txtRoff.Text) : 0;

                        objEntity.ModeOfTransport = drpModeOfTransport.SelectedValue;
                        objEntity.TransporterName = txtTransporterName.Text;
                        objEntity.VehicleNo = txtVehicleNo.Text;
                        objEntity.LRNo = txtLRNo.Text;
                        if (!String.IsNullOrEmpty(txtLRDate.Text))
                            objEntity.LRDate = Convert.ToDateTime(txtLRDate.Text);
                        objEntity.TransportRemark = txtTransportRemark.Text;

                        objEntity.ManuaLInwardNo = (String.IsNullOrEmpty(txtManualInwardNo.Text)?"": txtManualInwardNo.Text);

                        objEntity.NetAmt = (!String.IsNullOrEmpty(txtTotNetAmt.Text)) ? Convert.ToDecimal(txtTotNetAmt.Text) : 0;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.InwardMgmt.AddUpdateInward(objEntity, out ReturnCode, out ReturnMsg, out ReturnInwardNo);
                        strErr += "<li>" + ReturnMsg + "</li>";

                        // ------------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        // ------------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            BAL.InwardMgmt.DeleteInwardDetailByInwardNo(txtInwardNo.Text, out ReturnCode, out ReturnMsg);

                            Entity.InwardDetail objQuotDet = new Entity.InwardDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.InwardNo = ReturnInwardNo.Trim();
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.LocationID = intLocation;
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.UnitRate = Convert.ToDecimal(dr["UnitRate"]);
                                objQuotDet.DiscountPercent = Convert.ToDecimal(dr["DiscountPercent"]);
                                objQuotDet.DiscountAmt = Convert.ToDecimal(dr["DiscountAmt"]);
                                objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                                objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);
                                objQuotDet.TaxType = Convert.ToInt64(dr["TaxType"]);
                                objQuotDet.TaxRate = Convert.ToDecimal(dr["TaxRate"]);
                                objQuotDet.TaxAmount = Convert.ToDecimal(dr["TaxAmount"]);
                                objQuotDet.SGSTPer = Convert.ToDecimal(dr["SGSTPer"]);
                                objQuotDet.SGSTAmt = Convert.ToDecimal(dr["SGSTAmt"]);
                                objQuotDet.CGSTPer = Convert.ToDecimal(dr["CGSTPer"]);
                                objQuotDet.CGSTAmt = Convert.ToDecimal(dr["CGSTAmt"]);
                                objQuotDet.IGSTPer = Convert.ToDecimal(dr["IGSTPer"]);
                                objQuotDet.IGSTAmt = Convert.ToDecimal(dr["IGSTAmt"]);
                                objQuotDet.NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                                objQuotDet.OrderNo = dr["OrderNo"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.InwardMgmt.AddUpdateInwardDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtOrderProduct");
                                Session.Remove("dtDetail");
                            }
                            btnSave.Disabled = true;
                        }
                    }
                    else
                    {
                        strErr = "<li>" + "Atleast One Item is required to save Inward Information !" + "</li>";
                    }
                }
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            Session.Remove("dtOrderProduct");
            Session.Remove("dtDetail");

            hdnpkID.Value = "";
            txtInwardNo.Text = "";
            txtInwardDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";
            txtCustomerName.Enabled = true;
            BindInwardDetailList("");

            drpModeOfTransport.Attributes.Add("disabled", "disabled");
            txtTransporterName.Text = "";
            txtVehicleNo.Text = "";
            txtLRNo.Text = "";
            txtLRDate.Text = "";
            txtTransportRemark.Text = "";
            txtTotBasicAmt.Text = "";
            hdnTotItemGST.Value = "";
            txtTotAddTaxAmt.ReadOnly = true;
            txtTotGST.Text = "";
            txtRoff.Text = "";
            txtTotNetAmt.Text = "";

            txtManualInwardNo.Text = "";
            btnSave.Disabled = false;
            dvLoadItems.Visible = true;
            txtInwardDate.Focus();
        }
        public void funCalculateTotal()
        {
            hdnTotCGSTAmt.Value = Convert.ToDecimal(ViewState["totCGST"]).ToString("0.00");
            hdnTotSGSTAmt.Value = Convert.ToDecimal(ViewState["totSGST"]).ToString("0.00");
            hdnTotIGSTAmt.Value = Convert.ToDecimal(ViewState["totIGST"]).ToString("0.00");

            txtTotBasicAmt.Text = Convert.ToDecimal(ViewState["totAmount"]).ToString("0.00");
            txtTotAddTaxAmt.Text = Convert.ToDecimal(ViewState["totAddTaxAmt"]).ToString("0.00");
            hdnTotItemGST.Value = (Convert.ToDecimal(hdnTotCGSTAmt.Value) + Convert.ToDecimal(hdnTotSGSTAmt.Value) + Convert.ToDecimal(hdnTotIGSTAmt.Value)).ToString("0.00");
            txtTotGST.Text = (Convert.ToDecimal(hdnTotCGSTAmt.Value) + Convert.ToDecimal(hdnTotSGSTAmt.Value) + Convert.ToDecimal(hdnTotIGSTAmt.Value)).ToString("0.00");

            decimal NetAmt = 0;
            NetAmt = Convert.ToDecimal(ViewState["totNetAmount"]);

            txtTotNetAmt.Text = Math.Round(NetAmt, 0).ToString("0.00");
            txtRoff.Text = (Math.Round(NetAmt, 0) - Math.Round(NetAmt, 2)).ToString("0.00");
        }
        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptInwardDetail.Controls[rptInwardDetail.Controls.Count - 1].Controls[0];
            string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            TextBox txUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            if (!String.IsNullOrEmpty(ctrl1))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            txUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            txtDiscountPercent.Text = "0";
            txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
        }

        protected void drpCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!String.IsNullOrEmpty(drpCustomer.SelectedValue))
            //    //BindQuotationList(Convert.ToInt64(drpCustomer.SelectedValue));
            //else
            //    //BindQuotationList(0);
        }

        //protected void drpTNC_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(drpTNC.SelectedValue) && drpTNC.SelectedValue != "0")
        //    {
        //        string tmpval = drpTNC.SelectedValue;
        //        List<Entity.Contents> lstList2 = new List<Entity.Contents>();
        //        lstList2 = BAL.CommonMgmt.GetContentList(Convert.ToInt64(drpTNC.SelectedValue), "TNC");
        //        if (lstList2.Count > 0)
        //            txtTermsCondition.Text = lstList2[0].TNC_Content;

        //    }
        //}

        [System.Web.Services.WebMethod]
        public static string DeleteInward(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.InwardMgmt.DeleteInward(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
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

        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;
            HiddenField edTaxType = (HiddenField)item.FindControl("edTaxType");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
            TextBox edDiscountPercent = (TextBox)item.FindControl("edDiscountPercent");
            TextBox edDiscountAmt = (TextBox)item.FindControl("edDiscountAmt");
            TextBox edNetRate = (TextBox)item.FindControl("edNetRate");
            TextBox edAmount = (TextBox)item.FindControl("edAmount");
            TextBox edTaxRate = (TextBox)item.FindControl("edTaxRate");
            TextBox edTaxAmount = (TextBox)item.FindControl("edTaxAmount");
            TextBox edNetAmount = (TextBox)item.FindControl("edNetAmount");
            TextBox edForOrderNo = (TextBox)item.FindControl("edForOrderNo");

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
            Decimal na = (!String.IsNullOrEmpty(edNetAmount.Text)) ? Convert.ToDecimal(edNetAmount.Text) : 0;
            Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(edTaxType.Value)) ? Convert.ToInt16(edTaxType.Value) : 0);
            // --------------------------------------------------------------------------
            //nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            //a = Math.Round((q * nr), 2);
            //ta = Math.Round(((a * tr) / 100), 2);
            //na = Math.Round((a + ta), 2);
            //Int16 taxtype = Convert.ToInt16((!String.IsNullOrEmpty(edTaxType.Value)) ? Convert.ToInt16(edTaxType.Value) : 0);
            // --------------------------------------------------------------------------

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
            //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(),0, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

            edTaxType.Value = taxtype.ToString();
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

            if (dtDetail != null)
            {
                foreach (System.Data.DataColumn col in dtDetail.Columns)
                {
                    col.AllowDBNull = true;
                    col.ReadOnly = false;
                }

                foreach (DataRow row in dtDetail.Rows)
                {
                    if (row["ProductID"].ToString() == edProductID.Value && row["OrderNo"].ToString() == edForOrderNo.Text)
                    {
                        if (!String.IsNullOrEmpty(edQuantity.Text) && !String.IsNullOrEmpty(edUnitRate.Text))
                        {
                            row.SetField("TaxType", edTaxType.Value);
                            row.SetField("Quantity", edQuantity.Text);
                            //row.SetField("Unit", edUnit.Text);
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
                    }
                }
                rptInwardDetail.DataSource = dtDetail;
                rptInwardDetail.DataBind();
            }
            Session.Add("dtDetail", dtDetail);
        }

        protected void editItem_TextChanged1()
        {
            Control rptFootCtrl = rptInwardDetail.Controls[rptInwardDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            TextBox txtQuantity = (TextBox)rptFootCtrl.FindControl("txtQuantity");
            TextBox txtUnitRate = (TextBox)rptFootCtrl.FindControl("txtUnitRate");
            TextBox txtDiscountPercent = (TextBox)rptFootCtrl.FindControl("txtDiscountPercent");
            TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
            TextBox txtNetRate = (TextBox)rptFootCtrl.FindControl("txtNetRate");
            TextBox txtAmount = (TextBox)rptFootCtrl.FindControl("txtAmount");
            TextBox txtTaxRate = (TextBox)rptFootCtrl.FindControl("txtTaxRate");
            TextBox txtTaxAmount = (TextBox)rptFootCtrl.FindControl("txtTaxAmount");
            TextBox txtNetAmount = (TextBox)rptFootCtrl.FindControl("txtNetAmount");
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
            Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            Int16 taxtype = Convert.ToInt16(String.IsNullOrEmpty(hdnTaxType.Value) ? 0 : Convert.ToInt16(hdnTaxType.Value));
            // --------------------------------------------------------------------------
            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, q, ur, dp, dpa, tr, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);
            //funCalculate(taxtype, q, ur, dp, dpa, tr, at, isIGST(), 0, out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

            txtDiscountPercent.Text = ItmDiscPer1.ToString();
            txtDiscountAmt.Text = ItmDiscAmt1.ToString();
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

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptInwardDetail.Controls[rptInwardDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            //HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));

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

            txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            txtUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //hdnUnitQuantity.Value = (lstEntity.Count > 0) ? ((!String.IsNullOrEmpty(lstEntity[0].UnitQuantity.ToString())) ? lstEntity[0].UnitQuantity.ToString() : "1") : "1";
            txtDiscountPercent.Text = "0";
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

            // -----------------------------------------------------
            // Binding Customer's Purchase Order
            // -----------------------------------------------------
            DropDownList drpForOrderNo = (DropDownList)rptFootCtrl.FindControl("drpForOrderNo");
            drpForOrderNo.Items.Clear();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && !String.IsNullOrEmpty(hdnProductID.Value))
            {
                //drpForOrderNo.DataSource = BAL.InwardMgmt.GetInwardListByCustomer(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), Session["LoginUserID"].ToString(), 0, 0);
                drpForOrderNo.DataSource = BAL.PurchaseOrderMgmt.GetPurchaseOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), "", 0, 0);
                drpForOrderNo.DataTextField = "OrderNo";
                drpForOrderNo.DataValueField = "OrderNo";
                drpForOrderNo.DataBind();
                drpForOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- PO # --", ""));
            }
            // -----------------------------------------------------
            txtQuantity.Focus();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();

            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (lstEntity.Count > 0)
                {
                    hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";
                }
                BindPurchaseOrderList("PurchaseOrder", Convert.ToInt64(hdnCustomerID.Value));
            }
            else
            {
                strErr += "<li>" + "Select Proper Customer From List." + "</li>";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                txtCustomerName.Focus();
            }
        }

        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnSelectedReference.Value))
            {
                string selectedValues = string.Empty;
                Int64 ProductCount = 0;

                DataTable dtProduct = new DataTable();
                dtProduct = (DataTable)Session["dtOrderProduct"];

                DataTable dtTemp = new DataTable();

                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];

                foreach (System.Web.UI.WebControls.ListItem li in lstOrderProduct.Items)
                {
                    if (li.Selected == true)
                    {
                        ProductCount = ProductCount + 1;
                        selectedValues += li.Value + ",";
                        dtTemp = dtProduct.AsEnumerable().Where(r => (r.Field<string>("OrdProductID") == li.Value)).CopyToDataTable();
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (dtDetail != null)
                            {
                                string checkDuplicate = "";
                                checkDuplicate = "";
                                if (!string.IsNullOrEmpty(dr["ProductID"].ToString()))
                                    checkDuplicate = "ProductID = " + dr["ProductID"].ToString();

                                if (!string.IsNullOrEmpty(dr["OrderNo"].ToString()))
                                    checkDuplicate += " And OrderNo = '" + dr["OrderNo"].ToString() + "'";

                                DataRow[] FoundRows = dtDetail.Select(checkDuplicate);

                                if (FoundRows.Length > 0)
                                    continue;
                            }
                            dtDetail.ImportRow(dr);
                        }
                    }
                }

                if (ProductCount <= 0)
                {
                    string selectedOrder = string.Empty;
                    string[] values = hdnSelectedReference.Value.Split(',');
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            string tmpVal = values[i];
                            selectedOrder += string.IsNullOrEmpty(selectedOrder) ? "'" + values[i] + "'" : ",'" + values[i] + "'";
                        }

                        DataTable dtProduct1 = new DataTable();
                        dtProduct1 = BAL.PurchaseOrderMgmt.GetPurchaseOrderDetailForInward(selectedOrder);
                        foreach (DataRow dr in dtProduct1.Rows)
                        {
                            if (dtDetail != null)
                            {
                                string checkDuplicate = "";
                                checkDuplicate = "";
                                if (!string.IsNullOrEmpty(dr["ProductID"].ToString()))
                                    checkDuplicate = "ProductID = " + dr["ProductID"].ToString();

                                if (!string.IsNullOrEmpty(dr["OrderNo"].ToString()))
                                    checkDuplicate += " And OrderNo = '" + dr["OrderNo"].ToString() + "'";

                                DataRow[] FoundRows = dtDetail.Select(checkDuplicate);

                                if (FoundRows.Length > 0)
                                    continue;
                            }

                            dtDetail.ImportRow(dr);
                        }
                    }
                }


                //if (ProductCount <= 0)
                //{
                //    string checkDuplicate = "";
                //    // ========================================================
                //    string[] values = hdnSelectedReference.Value.Split(',');
                //    for (int i = 0; i < values.Length; i++)
                //    {
                //        string tmpVal = values[i];
                //        if (!String.IsNullOrEmpty(tmpVal))
                //        {
                //            DataTable dtTable = new DataTable();
                //            dtTable = BAL.PurchaseOrderMgmt.GetPurchaseOrderDetailForInward("'" + tmpVal + "'");
                //            foreach (DataRow myrow in dtTable.Rows)
                //            {
                //                checkDuplicate = "";
                //                if (!string.IsNullOrEmpty(myrow["ProductID"].ToString()))
                //                    checkDuplicate = "ProductID = " + myrow["ProductID"].ToString();

                //                if (!string.IsNullOrEmpty(myrow["OrderNo"].ToString()))
                //                    checkDuplicate += "And OrderNo = '" + myrow["OrderNo"].ToString() + "'";

                //                DataRow[] FoundRows = dtDetail.Select(checkDuplicate);

                //                if (FoundRows.Length > 0)
                //                {
                //                    continue;
                //                }

                //                DataRow dr = dtDetail.NewRow();
                //                dr["pkID"] = 0;
                //                dr["InwardNo"] = txtInwardNo.Text;
                //                dr["ProductID"] = myrow["ProductID"].ToString();
                //                dr["ProductName"] = myrow["ProductName"].ToString();
                //                dr["ProductNameLong"] = myrow["ProductNameLong"].ToString();
                //                dr["Quantity"] = Convert.ToDecimal(myrow["Quantity"].ToString());
                //                dr["Unit"] = myrow["Unit"].ToString();
                //                dr["UnitRate"] = Convert.ToDecimal(myrow["UnitRate"].ToString());
                //                dr["DiscountPercent"] = Convert.ToDecimal(myrow["DiscountPercent"].ToString());
                //                dr["DiscountAmt"] = Convert.ToDecimal(myrow["DiscountAmt"].ToString());
                //                dr["NetRate"] = Convert.ToDecimal(myrow["NetRate"].ToString());
                //                dr["Amount"] = Convert.ToDecimal(myrow["Amount"].ToString());
                //                dr["CGSTPer"] = Convert.ToDecimal(myrow["CGSTPer"].ToString());
                //                dr["SGSTPer"] = Convert.ToDecimal(myrow["SGSTPer"].ToString());
                //                dr["IGSTPer"] = Convert.ToDecimal(myrow["IGSTPer"].ToString());
                //                dr["CGSTAmt"] = Convert.ToDecimal(myrow["CGSTAmt"].ToString());
                //                dr["SGSTAmt"] = Convert.ToDecimal(myrow["SGSTAmt"].ToString());
                //                dr["IGSTAmt"] = Convert.ToDecimal(myrow["IGSTAmt"].ToString());
                //                dr["TaxType"] = Convert.ToInt64(myrow["TaxType"].ToString());
                //                dr["TaxRate"] = Convert.ToDecimal(myrow["TaxRate"].ToString());
                //                dr["TaxAmount"] = Convert.ToDecimal(myrow["TaxAmount"].ToString());
                //                dr["NetAmount"] = Convert.ToDecimal(myrow["NetAmount"].ToString());
                //                dr["OrderNo"] = myrow["OrderNo"].ToString();
                //                dtDetail.Rows.Add(dr);
                //            }
                //            dtDetail.AcceptChanges();
                //        }
                //    }
                //}
                Session.Add("dtDetail", dtDetail);
                rptInwardDetail.DataSource = dtDetail;
                rptInwardDetail.DataBind();
                // drpReferenceNo.Items.RemoveAt(index);
            }


        }

        protected void drpReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:getSelectedOrders();", true);

            BindPurchaseOrderProductList();
        }


        [WebMethod]
        public static string GetInwardNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetInwardNo(pkID);
            return tempVal;
        }

        [WebMethod(EnableSession = true)]
        public static void GenerateInward(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // ----------------------------------------------------------------------- 

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            string LoginUserID = HttpContext.Current.Session["LoginUserID"].ToString();
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
            Int32 CompanyId = 0;

            StarsProject.QuotationEagle.serialkey = tmpSerialKey;
            StarsProject.QuotationEagle.LoginUserID = LoginUserID;
            StarsProject.QuotationEagle.printheader = flagPrintHeader;
            StarsProject.QuotationEagle.path = Path;
            StarsProject.QuotationEagle.imagepath = imagepath;
            StarsProject.QuotationEagle.companyid = CompanyId;
            //tmpSerialKey = "HONP-MEDF-9RTS-FG10";
            if (tmpSerialKey == "HONP-MEDF-9RTS-FG10")          // HNMED
            {
                GenerateInward_HonmedSimple(pkID);
            }
            else
            {
                GenerateInward_HonmedSimple(pkID);
            }

        }

        public static void GenerateInward_HonmedSimple(Int64 pkID)
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

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Inward");

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
            List<Entity.Inward> lstin = new List<Entity.Inward>();
            lstin = BAL.InwardMgmt.GetInwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //--------------------------------------------------------------------------------------------
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.InwardMgmt.GetInwardDetail(lstin[0].InwardNo);

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstin.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 30, 20, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                //----------------------Customer Details Table---------------------------//

                PdfPTable tblAddress = new PdfPTable(2);
                int[] column_tblAddress = { 30, 70 };
                tblAddress.SetWidths(column_tblAddress);

                tblAddress.AddCell(pdf.setCell("Inward No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(": " + lstin[0].InwardNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("Inward Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(": " + lstin[0].InwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("Customer Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(": " + lstin[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblAddress.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell("Inward Slip", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblAddress, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblMember.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Material Movement Detail
                // -------------------------------------------------------------------------------------
                var sumDis = dtDetail1.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblDetail1 =  { 10,35, 20,15,20};
                    tblDetail.SetWidths(column_tblDetail1);
                }
                else
                {
                    int[] column_tblDetail1 = { 10, 35, 25, 1, 29 };
                    tblDetail.SetWidths(column_tblDetail1);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                tblDetail.AddCell(pdf.setCell("Sr.No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Product Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Quantity", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Discount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
                else
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));

                for (int i = 0; i < dtDetail1.Rows.Count; i++)
                {

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtDetail1.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtDetail1.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtDetail1.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtDetail1.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                        tblDetail.AddCell(pdf.setCell(dtDetail1.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    }
                    else
                        tblDetail.AddCell(pdf.setCell(dtDetail1.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));

                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 9, Element.ALIGN_CENTER, Element.ALIGN_TOP, 1));

                if (ProdDetail_Lines > (dtDetail1.Rows.Count))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtDetail1.Rows.Count)); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        //tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (sumDis > 0)
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));

                    }
                }

            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstin[0].InwardNo.Replace("/", "-").ToString() + ".pdf";
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



        [WebMethod(EnableSession = true)]
        public static void GenerateInwardhnmed(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // ----------------------------------------------------------------------- 

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            string LoginUserID = HttpContext.Current.Session["LoginUserID"].ToString();
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
            Int32 CompanyId = 0;

            StarsProject.QuotationEagle.serialkey = tmpSerialKey;
            StarsProject.QuotationEagle.LoginUserID = LoginUserID;
            StarsProject.QuotationEagle.printheader = flagPrintHeader;
            StarsProject.QuotationEagle.path = Path;
            StarsProject.QuotationEagle.imagepath = imagepath;
            StarsProject.QuotationEagle.companyid = CompanyId;
            //tmpSerialKey = "HONP-MEDF-9RTS-FG10";
            if (tmpSerialKey == "HONP-MEDF-9RTS-FG10")          // HNMED
            {
                GenerateInward_Honmed(pkID);
            }
            else
            {
                GenerateInward_Honmed(pkID);
            }

        }

        public static void GenerateInward_Honmed(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblDetail = new PdfPTable(10);
            //PdfPTable tblCylDetail = new PdfPTable(4);

            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
            PdfPTable tblSignOff = new PdfPTable(2);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // ===========================================================================================


            float paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf10 = 10, lengthmm = 2;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            float length = (float)((lengthmm) / 25.4 * 72);

            Int64 TopMargin = Convert.ToInt64(length), BottomMargin = Convert.ToInt64(length), LeftMargin = Convert.ToInt64(length), RightMargin = Convert.ToInt64(length);
            Int64 ProdDetail_Lines = 0;

            // =========================================================================
            // PDF Document Object Instance Creation
            // =========================================================================
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            //pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            float Widthmm = 100, Heightmm = 71;
            //Double Width = (((Convert.ToDouble(Widthmm)) / 25.4) * 72);
            float Width = (float)((Widthmm) / 25.4 * 72);
            float Height = (float)((Heightmm) / 25.4 * 72);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(Width, Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Inward> lstQuot = new List<Entity.Inward>();
            lstQuot = BAL.InwardMgmt.GetInwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.InwardMgmt.GetInwardDetail(lstQuot[0].InwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
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
                int[] column_tblMember = { 100 };
                tblMember.SetWidths(column_tblMember);
                //tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;


                var empty = new Phrase();
                empty.Add(new Chunk(" ", pdf.fnCalibri5));

                string address = (!String.IsNullOrEmpty(lstCust[0].Address) ? lstCust[0].Address : "") +
                               (!String.IsNullOrEmpty(lstCust[0].Area) ? " " + lstCust[0].Area : "") +
                               (!String.IsNullOrEmpty(lstCust[0].CityName) ? ", " + lstCust[0].CityName : "") +
                               (!String.IsNullOrEmpty(lstCust[0].StateName) ? ", " + lstCust[0].StateName : "") +
                                (!String.IsNullOrEmpty(lstCust[0].Pincode) ? " - " + lstCust[0].Pincode : "");

                string Org_Address = lstOrg[0].Address + " , " + lstOrg[0].CityName + " - " + lstOrg[0].Pincode + " , " + lstOrg[0].StateName;

                // -------------------------------------------------------------------------------------
                //  Defining : Customer  Detail

                Phrase CustomerName = new Phrase();
                Chunk c1 = new Chunk(lstQuot[0].CustomerName, pdf.fnCalibriBold18);
                c1.SetUnderline(1, -1);
                CustomerName.Add(c1);

                Phrase Address = new Phrase();
                Chunk c2 = new Chunk("Add:", pdf.fnCalibriBold14);
                c2.SetUnderline(1, -1);
                Address.Add(c2);

                Phrase Mo = new Phrase();
                Chunk c3 = new Chunk("MO:", pdf.fnCalibriBold14);
                c3.SetUnderline(1, -1);
                Mo.Add(c3);

                PdfPTable tblCustomerD = new PdfPTable(2);
                int[] column_tblNested20 = { 14, 86 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(Address, pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(address, pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(Mo, pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // -------------------------------------------------------------------------------------
                //  Defining : Organization  Detail

                PdfPTable tblorgadd = new PdfPTable(2);
                int[] column_tblorgadd = { 50, 50 };
                tblorgadd.SetWidths(column_tblorgadd);
                tblorgadd.AddCell(pdf.setCell("From,", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblorgadd.AddCell(pdf.setCell("MO." + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblorgadd.AddCell(pdf.setCell(lstOrg[0].OrgName.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblorgadd.AddCell(pdf.setCell(Org_Address, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));


                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblMember.AddCell(pdf.setCell(tblorgadd, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BASELINE, 15));




                // ---------------------------------------------------
                int[] column_tblFooter = { 100 };
                tblFooter.SetWidths(column_tblFooter);
                //tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_BOTTOM;
                tblFooter.AddCell(pdf.setCell(tblorgadd, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 15));
                // -------------------------------------------------------------------------------------

                #endregion


                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].InwardNo.Replace("/", "-").ToString() + ".pdf";
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


                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

                    if (i != 0)
                        pdfDoc.NewPage();

                    PdfPTable tblOuterDetail = new PdfPTable(2);
                    int[] column_tblNestedOuter = { 60,40};
                    tblOuterDetail.SetWidths(column_tblNestedOuter);
                    tblOuterDetail.SpacingBefore = 0f;
                    tblOuterDetail.LockedWidth = true;
                    tblOuterDetail.SplitLate = false;
                    tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";

                    Phrase p = new Phrase();
                    Chunk c = new Chunk(" Product", pdf.fnCalibri14);
                    Chunk cc = new Chunk(dtItem.Rows[i]["ProductName"].ToString(), pdf.fnCalibriBold14);
                    cc.SetUnderline(-1, -2);
                    Chunk ccc = new Chunk(" : ", pdf.fnCalibriBold14);
                    p.Add(c);
                    p.Add(ccc);
                    p.Add(cc);


                    Phrase pm = new Phrase();
                    Chunk cm = new Chunk("Model", pdf.fnCalibri14);
                    Chunk ccm = new Chunk(tmpProdAlias, pdf.fnCalibriBold14);
                    ccm.SetUnderline(-1, -2);
                    pm.Add(cm);
                    pm.Add(ccc);
                    pm.Add(ccm);

                    Phrase ps = new Phrase();
                    Chunk cs = new Chunk("SN", pdf.fnCalibriBold14);
                    cs.SetUnderline(-1, -2);
                    Chunk ccs = new Chunk("____________", pdf.fnCalibriBold14);
                    ps.Add(cs);
                    ps.Add(ccc);
                    ps.Add(ccs);

                    tblOuterDetail.AddCell(pdf.setCell(p , pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                    PdfPTable image = new PdfPTable(1);
                    int[] column_Image = { 100 };
                    image.SetWidths(column_Image);
                    image.AddCell(pdf.setCell(pm, pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    image.AddCell(pdf.setCell(ps, pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                    PdfPTable tblESignature = new PdfPTable(1);
                    int[] column_tblESignature = { 100 };
                    tblESignature.SetWidths(column_tblESignature);
                    int fileCount = 0;
                    string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo.png";
                    //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";

                    if (File.Exists(tmpFile))
                    {
                        if (File.Exists(tmpFile))   //Signature print
                        {
                            PdfPTable tblSign = new PdfPTable(1);
                            iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                            eSign.ScaleAbsolute(75, 55);
                            tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                            tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                            fileCount = fileCount + 1;
                        }
                    }

                    PdfPTable tblAdd = new PdfPTable(2);
                    int[] column_tblAdd = { 50,50};
                    tblAdd.SetWidths(column_tblAdd);

                    tblAdd.AddCell(pdf.setCell(lstOrg[0].OrgName, pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    tblAdd.AddCell(pdf.setCellBoldUnbold("Ph. : ", lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.fnCalibri12, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    tblAdd.AddCell(pdf.setCellBoldUnbold("Address : ",Org_Address, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.fnCalibri10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    tblAdd.AddCell(pdf.setCellBoldUnbold("Email : ", lstOrg[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.fnCalibri10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    tblAdd.AddCell(pdf.setCellBoldUnbold("WebSite : ", lstOrg[0].Fax1, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.fnCalibri10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));


                    tblOuterDetail.AddCell(pdf.setCell(image, pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    tblOuterDetail.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    tblOuterDetail.AddCell(pdf.setCell(tblAdd, pdf.WhiteBaseColor, pdf.fnCalibriBold7, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblOuterDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                    tblOuterDetail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    pdfDoc.Add(tblOuterDetail);
                    // >>>>>> Adding Quotation Header
                    //tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                    //tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    //pdfDoc.Add(tblHeader);



                    //// >>>>>> Adding Quotation Master Information Table
                    //tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                    //tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
                    //pdfDoc.Add(tblMember);


                    //// >>>>>> Adding Quotation Footer
                    //tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                    //tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    //pdfDoc.Add(tblFooter);

                    
                }
                // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
                pdfDoc.Close();
                pdfDoc.Dispose();
                string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
                byte[] content = ms.ToArray();
                FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
                fs.Write(content, 0, (int)content.Length);
                fs.Close();
                fs.Dispose();
                string pdfFileName = "";
                pdfFileName = sPath + sFileName;
                // ------------------------------------------------
                RecompressPDF(sPath + smallFileName, pdfFileName);
            }
        }

        public static void GenerateInwardhnmed_Sharvaya(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblDetail = new PdfPTable(10);
            //PdfPTable tblCylDetail = new PdfPTable(4);

            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
            PdfPTable tblSignOff = new PdfPTable(2);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // ===========================================================================================


            float paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf10 = 10, lengthmm = 2;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            float length = (float)((lengthmm) / 25.4 * 72);

            Int64 TopMargin = Convert.ToInt64(length), BottomMargin = Convert.ToInt64(length), LeftMargin = Convert.ToInt64(length), RightMargin = Convert.ToInt64(length);
            Int64 ProdDetail_Lines = 0;

            // =========================================================================
            // PDF Document Object Instance Creation
            // =========================================================================
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            //pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            float Widthmm = 100, Heightmm = 71;
            //Double Width = (((Convert.ToDouble(Widthmm)) / 25.4) * 72);
            float Width = (float)((Widthmm) / 25.4 * 72);
            float Height = (float)((Heightmm) / 25.4 * 72);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(Width, Height));
            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Inward> lstQuot = new List<Entity.Inward>();
            lstQuot = BAL.InwardMgmt.GetInwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.InwardMgmt.GetInwardDetail(lstQuot[0].InwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
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
                int[] column_tblMember = { 100 };
                tblMember.SetWidths(column_tblMember);
                //tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;


                var empty = new Phrase();
                empty.Add(new Chunk(" ", pdf.fnCalibri5));

                string address = (!String.IsNullOrEmpty(lstCust[0].Address) ? lstCust[0].Address : "") +
                               (!String.IsNullOrEmpty(lstCust[0].Area) ? " " + lstCust[0].Area : "") +
                               (!String.IsNullOrEmpty(lstCust[0].CityName) ? ", " + lstCust[0].CityName : "") +
                               (!String.IsNullOrEmpty(lstCust[0].StateName) ? ", " + lstCust[0].StateName : "") +
                                (!String.IsNullOrEmpty(lstCust[0].Pincode) ? " - " + lstCust[0].Pincode : "");

                string Org_Address = lstOrg[0].Address + " , " + lstOrg[0].CityName + " - " + lstOrg[0].Pincode + " , " + lstOrg[0].StateName;

                // -------------------------------------------------------------------------------------
                //  Defining : Customer  Detail

                Phrase CustomerName = new Phrase();
                Chunk c1 = new Chunk(lstQuot[0].CustomerName, pdf.fnCalibriBold18);
                c1.SetUnderline(1, -1);
                CustomerName.Add(c1);

                Phrase Address = new Phrase();
                Chunk c2 = new Chunk("Add:", pdf.fnCalibriBold14);
                c2.SetUnderline(1, -1);
                Address.Add(c2);

                Phrase Mo = new Phrase();
                Chunk c3 = new Chunk("MO:", pdf.fnCalibriBold14);
                c3.SetUnderline(1, -1);
                Mo.Add(c3);

                PdfPTable tblCustomerD = new PdfPTable(2);
                int[] column_tblNested20 = { 14, 86 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(Address, pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(address, pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(Mo, pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // -------------------------------------------------------------------------------------
                //  Defining : Organization  Detail

                PdfPTable tblorgadd = new PdfPTable(2);
                int[] column_tblorgadd = { 50, 50 };
                tblorgadd.SetWidths(column_tblorgadd);
                tblorgadd.AddCell(pdf.setCell("From,", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblorgadd.AddCell(pdf.setCell("MO." + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblorgadd.AddCell(pdf.setCell(lstOrg[0].OrgName.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblorgadd.AddCell(pdf.setCell(Org_Address, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));


                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblMember.AddCell(pdf.setCell(tblorgadd, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BASELINE, 15));




                // ---------------------------------------------------
                int[] column_tblFooter = { 100 };
                tblFooter.SetWidths(column_tblFooter);
                //tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_BOTTOM;
                tblFooter.AddCell(pdf.setCell(tblorgadd, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 15));
                // -------------------------------------------------------------------------------------

                #endregion


                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].InwardNo.Replace("/", "-").ToString() + ".pdf";
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

                // >>>>>> Adding Quotation Header
                tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                pdfDoc.Add(tblHeader);



                // >>>>>> Adding Quotation Master Information Table
                tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfDoc.Add(tblMember);


                // >>>>>> Adding Quotation Footer
                tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                pdfDoc.Add(tblFooter);

                // >>>>>> Closing : HTML & BODY
                htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
                // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                pdfDoc.Close();
                pdfDoc.Dispose();
                string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
                byte[] content = ms.ToArray();
                FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
                fs.Write(content, 0, (int)content.Length);
                fs.Close();
                fs.Dispose();
                string pdfFileName = "";
                pdfFileName = sPath + sFileName;
                // ------------------------------------------------
                RecompressPDF(sPath + smallFileName, pdfFileName);
            }
        }
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