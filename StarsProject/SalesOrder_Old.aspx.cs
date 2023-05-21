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
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace StarsProject
{
    public partial class SalesOrder_Old : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount;
        //private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                totAmount = 0;
                totTaxAmount = 0;
                totNetAmount = 0;

                DataTable dtDetail = new DataTable();
                Session.Add("dtDetail", dtDetail);

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
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
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];

                if (requestTarget.ToLower() == "drpquotation")
                {
                    if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpQuotation.SelectedValue))
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        dtDetail.Clear();
                        dtDetail = BAL.QuotationDetailMgmt.GetQuotationProductForSalesOrder(drpQuotation.SelectedValue, txtOrderNo.Text);
                        rptOrderDetail.DataSource = dtDetail;
                        rptOrderDetail.DataBind();

                        Session.Add("dtDetail", dtDetail);
                    }
                }
            }
        }

        public void OnlyViewControls()
        {
            txtOrderNo.ReadOnly = true;
            txtOrderDate.ReadOnly = true;
            txtTermsCondition.ReadOnly = true;
            //drpCustomer.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            drpQuotation.Attributes.Add("disabled", "disabled");
            drpSalesPerson.Attributes.Add("disabled", "disabled");
            drpApprovalStatus.Attributes.Add("disabled", "disabled");
            drpTNC.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;
            pnlDetail.Enabled = false;
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
            drpQuotation.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.QuotationMgmt.GetQuotationListByCustomer(pCustomerID);

                drpQuotation.DataValueField = "QuotationNo";
                drpQuotation.DataTextField = "QuotationNo";
                if (lstEntity.Count > 0)
                {
                    drpQuotation.DataSource = lstEntity;
                    drpQuotation.DataBind();
                }
                drpQuotation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpQuotation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        // ----------------------------------------------------------------------------------
        // Sales Order Item Detail List 
        // ----------------------------------------------------------------------------------
        public void BindSalesOrderDetailList(string pOrderNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SalesOrderMgmt.GetSalesOrderDetail(pOrderNo);
            rptOrderDetail.DataSource = dtDetail1;
            rptOrderDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
            // -----------------------------------------------------------
            BindPayScheduleList(0, pOrderNo, Session["LoginUserID"].ToString());
        }

        protected void rptOrderDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;
                divErrorMessage.InnerHtml = "";

                if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0"
                    || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                {
                    _pageValid = false;

                    divErrorMessage.Style.Remove("color");
                    divErrorMessage.Style.Add("color", "red");

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                        divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Product Selection is required." + "</li>"));

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                        divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Quantity is required." + "</li>"));

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                        divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Unit Rate is required." + "</li>"));

                }
                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

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
                    string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                    string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                    string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                    string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                    string disper = ((TextBox)e.Item.FindControl("txtDiscountPercent")).Text;
                    string netrate = ((TextBox)e.Item.FindControl("txtNetRate")).Text;
                    string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;
                    string taxrate = ((TextBox)e.Item.FindControl("txtTaxRate")).Text;
                    string taxamt = ((TextBox)e.Item.FindControl("txtTaxAmount")).Text;
                    string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;

                    dr["OrderNo"] = txtOrderNo.Text;
                    dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                    dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                    dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                    dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                    dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                    dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                    dr["DiscountPercent"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                    dr["NetRate"] = (!String.IsNullOrEmpty(netrate)) ? Convert.ToDecimal(netrate) : 0;
                    dr["Amount"] = (!String.IsNullOrEmpty(amt)) ? Convert.ToDecimal(amt) : 0;
                    dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
                    dr["TaxAmount"] = (!String.IsNullOrEmpty(taxamt)) ? Convert.ToDecimal(taxamt) : 0;
                    dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;

                    dtDetail.Rows.Add(dr);
                    // ---------------------------------------------------------------
                    rptOrderDetail.DataSource = dtDetail;
                    rptOrderDetail.DataBind();
                    // ---------------------------------------------------------------
                    Session.Add("dtDetail", dtDetail);
                }
                btnSave.Focus();

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
            }
        }

        protected void rptOrderDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2, v3;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Amount"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaxAmount"));
                v3 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmount"));

                totAmount += v1;
                totTaxAmount += v2;
                totNetAmount += v3;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotalGrossAmount = (Label)e.Item.FindControl("lblTotalGrossAmount");
                Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
                Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");

                lblTotalGrossAmount.Text = totAmount.ToString("0.00");
                lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
                lblTotalNetAmount.Text = totNetAmount.ToString("0.00");
            }
        }
        // ----------------------------------------------------------------------------------
        // Payment Schedule 
        // ----------------------------------------------------------------------------------
        public void BindPayScheduleList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SalesOrderMgmt.GetPayScheduleList(pkID, OrderNo, LoginUserID);
            rptPaySchedule.DataSource = dtDetail1;
            rptPaySchedule.DataBind();
            Session.Add("dtSchedule", dtDetail1);
        }

        protected void rptPaySchedule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString().ToLower() == "update")
            {
                _pageValid = true;
                divErrorMessage.InnerHtml = "";
                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtSchedule"];

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
                    string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                    string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                    string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                    string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                    string disper = ((TextBox)e.Item.FindControl("txtDiscountPercent")).Text;
                    string netrate = ((TextBox)e.Item.FindControl("txtNetRate")).Text;
                    string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;
                    string taxrate = ((TextBox)e.Item.FindControl("txtTaxRate")).Text;
                    string taxamt = ((TextBox)e.Item.FindControl("txtTaxAmount")).Text;
                    string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;

                    dr["OrderNo"] = txtOrderNo.Text;
                    dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                    dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                    dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                    dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                    dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                    dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                    dr["DiscountPercent"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                    dr["NetRate"] = (!String.IsNullOrEmpty(netrate)) ? Convert.ToDecimal(netrate) : 0;
                    dr["Amount"] = (!String.IsNullOrEmpty(amt)) ? Convert.ToDecimal(amt) : 0;
                    dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
                    dr["TaxAmount"] = (!String.IsNullOrEmpty(taxamt)) ? Convert.ToDecimal(taxamt) : 0;
                    dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;

                    dtDetail.Rows.Add(dr);
                    // ---------------------------------------------------------------
                    rptOrderDetail.DataSource = dtDetail;
                    rptOrderDetail.DataBind();
                    // ---------------------------------------------------------------
                    Session.Add("dtDetail", dtDetail);
                }
                btnSave.Focus();

            }
            if (e.CommandName.ToString().ToLower() == "delete")
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
                List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
                // ----------------------------------------------------
                lstEntity = BAL.SalesOrderMgmt.GetSalesOrderList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtOrderNo.Text = lstEntity[0].OrderNo;
                txtOrderDate.Text = lstEntity[0].OrderDate.ToString("dd-MM-yyyy");
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpSalesPerson.SelectedValue = (lstEntity[0].EmployeeID > 0) ? lstEntity[0].EmployeeID.ToString() : "";
                drpApprovalStatus.SelectedValue = lstEntity[0].ApprovalStatus;
                // ------------------------------------------------------------
                BindQuotationList(lstEntity[0].CustomerID);
                // ------------------------------------------------------------
                txtTermsCondition.Text = lstEntity[0].TermsCondition;
                drpQuotation.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].QuotationNo)) ? lstEntity[0].QuotationNo : "";
                // -------------------------------------------------------------------------
                BindSalesOrderDetailList(lstEntity[0].OrderNo);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "", ReturnOrderNo = "";
            // --------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            Entity.SalesOrder objEntity = new Entity.SalesOrder();
            if (dtDetail != null)
            {
                if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    if (!String.IsNullOrEmpty(hdnpkID.Value))
                        objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                    objEntity.QuotationNo = drpQuotation.SelectedValue;
                    objEntity.OrderNo = txtOrderNo.Text;
                    objEntity.OrderDate = Convert.ToDateTime(txtOrderDate.Text);
                    objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                    objEntity.TermsCondition = txtTermsCondition.Text;
                    objEntity.EmployeeID = (!String.IsNullOrEmpty(drpSalesPerson.SelectedValue)) ? Convert.ToInt64(drpSalesPerson.SelectedValue) : 0;
                    objEntity.ApprovalStatus = drpApprovalStatus.SelectedValue;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.SalesOrderMgmt.AddUpdateSalesOrder(objEntity, out ReturnCode, out ReturnMsg, out ReturnOrderNo);
                    // --------------------------------------------------------------
                    if (String.IsNullOrEmpty(ReturnOrderNo) && !String.IsNullOrEmpty(txtOrderNo.Text))
                    {
                        ReturnOrderNo = txtOrderNo.Text;
                    }
                    BAL.SalesOrderMgmt.DeleteSalesOrderDetailByOrderNo(ReturnOrderNo, out ReturnCode, out ReturnMsg);
                    // --------------------------------------------------------------
                    if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnOrderNo))
                    {
                        //DataTable dtDetail = new DataTable();
                        //dtDetail = (DataTable)Session["dtDetail"];
                        // --------------------------------------------------------------
                        btnSave.Enabled = false;
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        Entity.SalesOrderDetail objQuotDet = new Entity.SalesOrderDetail();

                        foreach (DataRow dr in dtDetail.Rows)
                        {
                            objQuotDet.pkID = 0;
                            objQuotDet.OrderNo = ReturnOrderNo;
                            objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                            objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                            objQuotDet.Unit = dr["Unit"].ToString();
                            objQuotDet.UnitRate = Convert.ToDecimal(dr["UnitRate"]);
                            objQuotDet.DiscountPercent = Convert.ToDecimal(dr["DiscountPercent"]);
                            objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                            objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);
                            objQuotDet.TaxRate = Convert.ToDecimal(dr["TaxRate"]);
                            objQuotDet.TaxAmount = Convert.ToDecimal(dr["TaxAmount"]);
                            objQuotDet.NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                            objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                            BAL.SalesOrderMgmt.AddUpdateSalesOrderDetail(objQuotDet, out ReturnCode, out ReturnMsg);
                        }

                        #region Direct_Control_Values
                        // -------------------------------------------------------
                        //Control rptFootCtrl = rptOrderDetail.Controls[rptOrderDetail.Controls.Count - 1].Controls[0];
                        //string ctrl1 = ((TextBox)rptFootCtrl.FindControl("txtQuantity")).Text;
                        //string ctrl2 = ((TextBox)rptFootCtrl.FindControl("txtNetAmount")).Text;
                        //string ctrl3 = ((TextBox)rptFootCtrl.FindControl("txtProductName")).Text;
                        //string ctrl4 = ((HiddenField)rptFootCtrl.FindControl("hdnProductID")).Value;

                        //if (!String.IsNullOrEmpty(ctrl1) && !String.IsNullOrEmpty(ctrl2) && !String.IsNullOrEmpty(ctrl3) && (!String.IsNullOrEmpty(ctrl4) && ctrl4 != "0"))
                        //{
                        //    objQuotDet.pkID = 0;
                        //    objQuotDet.OrderNo = ReturnOrderNo;
                        //    objQuotDet.ProductID = Convert.ToInt64(((HiddenField)rptFootCtrl.FindControl("hdnProductID")).Value);

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtQuantity")).Text))
                        //        objQuotDet.Quantity = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtQuantity")).Text);
                        //    else
                        //        objQuotDet.Quantity = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtUnit")).Text))
                        //        objQuotDet.Unit = ((TextBox)rptFootCtrl.FindControl("txtUnit")).Text;
                        //    else
                        //        objQuotDet.Unit = "";

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtUnitRate")).Text))
                        //        objQuotDet.UnitRate = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtUnitRate")).Text);
                        //    else
                        //        objQuotDet.UnitRate = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtDiscountPercent")).Text))
                        //        objQuotDet.DiscountPercent = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtDiscountPercent")).Text);
                        //    else
                        //        objQuotDet.DiscountPercent = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtNetRate")).Text))
                        //        objQuotDet.NetRate = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtNetRate")).Text);
                        //    else
                        //        objQuotDet.NetRate = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtAmount")).Text))
                        //        objQuotDet.Amount = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtAmount")).Text);
                        //    else
                        //        objQuotDet.Amount = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtTaxRate")).Text))
                        //        objQuotDet.TaxRate = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtTaxRate")).Text);
                        //    else
                        //        objQuotDet.TaxRate = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtTaxAmount")).Text))
                        //        objQuotDet.TaxAmount = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtTaxAmount")).Text);
                        //    else
                        //        objQuotDet.TaxAmount = 0;

                        //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtNetAmount")).Text))
                        //        objQuotDet.NetAmount = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtNetAmount")).Text);
                        //    else
                        //        objQuotDet.NetAmount = 0;

                        //    objQuotDet.LoginUserID = Session["LoginUserID"].ToString();


                        //    BAL.SalesOrderMgmt.AddUpdateSalesOrderDetail(objQuotDet, out ReturnCode, out ReturnMsg);
                        //}
                        #endregion
                        // --------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            Session.Remove("dtDetail");
                        }
                    }
                    // --------------------------------------------------------------
                    if (ReturnCode > 0)
                    {
                        if (!String.IsNullOrEmpty(txtOrderNo.Text))
                        {
                            divErrorMessage.InnerHtml = ReturnMsg;
                        }
                        else
                        {
                            txtOrderNo.Text = ReturnOrderNo;
                            divErrorMessage.InnerHtml = ((ReturnCode > 0) ? ReturnOrderNo + " " + ReturnMsg : ReturnMsg);
                        }
                    }
                }
            }
            else
            {
                divErrorMessage.InnerHtml = "Atleast One Item is required to save Sales Order Information !";
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {

        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtOrderDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            txtOrderNo.Text = ""; // BAL.CommonMgmt.GetSalesOrderNo(txtOrderDate.Text);
            txtTermsCondition.Text = "";
            //drpCustomer.SelectedValue = "";
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpQuotation.Items.Clear();
            drpQuotation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            divEmployee.Visible = false;
            BindSalesOrderDetailList("");
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
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            if (!String.IsNullOrEmpty(hdnProductID.Value))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            txtUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            txtDiscountPercent.Text = "0";
            txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
            txtQuantity.Focus();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                BindQuotationList(Convert.ToInt64(hdnCustomerID.Value));
            else
                BindQuotationList(0);
        
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

        [System.Web.Services.WebMethod]
        public static string DeleteSalesOrder(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.SalesOrderMgmt.DeleteSalesOrder(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
            TextBox edDiscountPercent = (TextBox)item.FindControl("edDiscountPercent");
            TextBox edNetRate = (TextBox)item.FindControl("edNetRate");
            TextBox edAmount = (TextBox)item.FindControl("edAmount");
            TextBox edTaxRate = (TextBox)item.FindControl("edTaxRate");
            TextBox edTaxAmount = (TextBox)item.FindControl("edTaxAmount");
            TextBox edNetAmount = (TextBox)item.FindControl("edNetAmount");
            // --------------------------------------------------------------------------
            Decimal q = Convert.ToDecimal(edQuantity.Text);
            Decimal ur = Convert.ToDecimal(edUnitRate.Text);
            Decimal dp = Convert.ToDecimal(edDiscountPercent.Text);
            Decimal nr = Convert.ToDecimal(edNetRate.Text);
            Decimal a = Convert.ToDecimal(edAmount.Text);
            Decimal tr = Convert.ToDecimal(edTaxRate.Text);
            Decimal ta = Convert.ToDecimal(edTaxAmount.Text);
            Decimal na = Convert.ToDecimal(edNetAmount.Text);
            // --------------------------------------------------------------------------
            nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            a = Math.Round((q * nr), 2);
            ta = Math.Round(((a * tr) / 100), 2);
            na = Math.Round((a + ta), 2);

            edNetRate.Text = nr.ToString();
            edAmount.Text = a.ToString();
            edTaxAmount.Text = ta.ToString();
            edNetAmount.Text = na.ToString();
            // --------------------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductName"].ToString() == edProductName.Value)
                {
                    row.SetField("Quantity", edQuantity.Text);
                    row.SetField("UnitRate", edUnitRate.Text);
                    row.SetField("DiscountPercent", edDiscountPercent.Text);
                    row.SetField("NetRate", edNetRate.Text);
                    row.SetField("Amount", edAmount.Text);
                    row.SetField("TaxRate", edTaxRate.Text);
                    row.SetField("TaxAmount", edTaxAmount.Text);
                    row.SetField("NetAmount", edNetAmount.Text);
                }
            }
            rptOrderDetail.DataSource = dtDetail;
            rptOrderDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
        }

        protected void drpQuotation_TextChanged(object sender, EventArgs e)
        {
            int TotalCount = 0;
            if (!String.IsNullOrEmpty(drpQuotation.SelectedValue) && drpQuotation.SelectedValue != "") 
            {
                List<Entity.QuotationDetail> lstEntity = new List<Entity.QuotationDetail>();
                // ----------------------------------------------------
                lstEntity = BAL.QuotationDetailMgmt.GetQuotationDetailListByQuotationNo(drpQuotation.SelectedValue, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                txtTermsCondition.Text = lstEntity[0].QuotationFooter.ToString();
            }
        }
        // ====================================================================================
        // PDF Generation .....
        // ====================================================================================
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

        [WebMethod(EnableSession=true)]
        public static void GenerateSalesOrder(Int64 pkID)
        {
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(10);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // ===========================================================================================
            float paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf10 = 10;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var GreenBaseColor = new BaseColor(60, 179, 113);
            var WhiteBaseColor = new BaseColor(255, 255, 255);
            var BlackBaseColor = new BaseColor(0, 0, 0);
            var GrayBaseColor = new BaseColor(238, 238, 224);
            var NavyBaseColor = new BaseColor(0, 0, 128);
            var LightBlueBaseColor = new BaseColor(236, 240, 241);
            var OrangeBaseColor = new BaseColor(243, 156, 18);

            iTextSharp.text.Font objHeaderFont12 = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objHeaderFont14 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objHeaderFont16 = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objHeaderFont18 = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            iTextSharp.text.Font objHeaderFontWhite14 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font objHeaderFontWhite16 = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font objHeaderFontWhite18 = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

            iTextSharp.text.Font objFooterFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);

            iTextSharp.text.Font objContentFontTitleBlack = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objContentFontDataBlack = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font objContentFontTitleWhite = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font objContentFontDataWhite = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
            iTextSharp.text.Font objContentFontTitleNavy = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, NavyBaseColor);
            iTextSharp.text.Font objContentFontDataNavy = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, NavyBaseColor);
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
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.SalesOrder> lstQuot = new List<Entity.SalesOrder>();
            lstQuot = BAL.SalesOrderMgmt.GetSalesOrderList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.SalesOrderMgmt.GetSalesOrderDetail(lstQuot[0].OrderNo);
            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(lstQuot[0].CompanyID, 1, 1000, out totrec);
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Section : QRCode
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            //if (!String.IsNullOrEmpty(lstQuot[0].QuotationNo))
            //{

            //    byte[] tmpVal = showQRCode(lstQuot[0].QuotationNo, lstQuot[0].CustomerName);
            //    MemoryStream ms1 = new MemoryStream(tmpVal);
            //    System.Drawing.Image sdi = System.Drawing.Image.FromStream(ms1);
            //    imgQRCode = iTextSharp.text.Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Png);
            //    //imgQRCode = new iTextSharp.text.Image.GetInstance(tmpVal);
            //}
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Header Table 
                // -------------------------------------------------------------------------------------

                tableHeader.DefaultCell.Border = PdfPCell.NO_BORDER;
                tableHeader.DefaultCell.CellEvent = new RoundedBorder1();
                tableHeader.SpacingBefore = 15f;
                tableHeader.LockedWidth = true;
                tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPTable tblNested1 = new PdfPTable(1);
                int[] column_tblNested1 = { 100 };
                tblNested1.SetWidths(column_tblNested1);
                // --------------------------------------------------------------------
                List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();

                lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
                String x1 = lstEntity[0].Address;
                String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                            ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                            ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + ", India";

                String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") +
                            ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? " Email Us : " + lstEntity[0].EmailAddress : "");
                // --------------------------------------------------------------------
                cell = new PdfPCell(new Paragraph(lstEntity[0].OrgName, objHeaderFont18));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingLeft = 2;
                cell.PaddingTop = 8;
                cell.Border = 0;
                tblNested1.AddCell(cell);

                tblNested1.AddCell(setCell(x1, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblNested1.AddCell(setCell(x2, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblNested1.AddCell(setCell(x3, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));

                //tblNested1.AddCell(setCell("412, Sahajanand Arcade, 132ft Ring Road,", WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //tblNested1.AddCell(setCell("Helmet Cross Road, Memnagar, Ahmedabad-380052", WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //tblNested1.AddCell(setCell("Telefax : 079-2792737, Email : sonata251@yahoo.com", WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));

                cell = new PdfPCell(tblNested1);
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = PdfPCell.BOTTOM_BORDER;
                cell.PaddingBottom = 5;
                tableHeader.AddCell(cell);
                // ---------------------------------------------------------------------------
                //imgQRCode.SetAbsolutePosition(pdfDoc.GetRight(80), pdfDoc.GetTop(90));
                //imgQRCode.ScaleToFit(80f, 80f);
                //imgQRCode.Alignment = Element.ALIGN_RIGHT;
                //cell = new PdfPCell(imgQRCode);
                //cell.Border = PdfPCell.BOTTOM_BORDER;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.PaddingBottom = 5;
                //tableHeader.AddCell(cell);
                // -------------------------------------------------------------------------------------
                //  Defining : Sales Order Data Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Sales Order Master Information
                tblMember.SpacingBefore = 0f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(setCell("To,", WhiteBaseColor, objContentFontTitleBlack, paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(setCell(lstQuot[0].CustomerName, WhiteBaseColor, objContentFontDataBlack, paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(setCell(lstQuot[0].Address + "," + lstQuot[0].Area + ",", WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblNested20.AddCell(setCell(lstQuot[0].City + "," + lstQuot[0].PinCode, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(2);
                int[] column_tblNested2 = { 40, 60 };
                tblNested2.SetWidths(column_tblNested2);
                
                tblNested2.AddCell(setCell("Order No. ", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                tblNested2.AddCell(setCell(lstQuot[0].OrderNo, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                tblNested2.AddCell(setCell("Order Date ", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                tblNested2.AddCell(setCell(lstQuot[0].OrderDate.ToString("dd-MMM-yyyy"), WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                tblNested2.AddCell(setCell("Quotation No ", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
                tblNested2.AddCell(setCell(lstQuot[0].QuotationNo, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));

                tblMember.AddCell(setCell(tblNested20, WhiteBaseColor, objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(setCell(tblNested2, WhiteBaseColor, objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Sales Order Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Sales Order Detail

                int[] column_tblNested = { 4, 25, 6, 10, 7, 9, 10, 7, 9, 12 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(setCell("No", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                tblDetail.AddCell(setCell("Product Name", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                tblDetail.AddCell(setCell("Qty", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Unit Price", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Dis.%", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Net Rate", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Amount", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Tax %", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Tax Amt", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Net Amount", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    //tmpAmount = (Convert.ToDecimal(dtItem.Rows[i]["Quantity"]) * Convert.ToDecimal(dtItem.Rows[i]["NetRate"]));
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    //Entity.Authenticate objAuth = new Entity.Authenticate();
                    //objAuth = (Entity.Authenticate)Session["logindetail"];
                    //string URL = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
                    // ------------------------------------------------------------------
                    tblDetail.AddCell(setCell((i + 1).ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                    if (objAuth.CompanyName.ToUpper().Contains("DALIA MOTORS"))
                    {
                        tblDetail.AddCell(setCell(dtItem.Rows[i]["ProductNameLong"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                    }
                    else
                    {
                        tblDetail.AddCell(setCell(dtItem.Rows[i]["ProductName"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                    }
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["Quantity"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["UnitRate"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["NetRate"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["Amount"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["TaxRate"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["TaxAmount"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["NetAmount"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    if (!String.IsNullOrEmpty(dtItem.Rows[i]["ProductSpecification"].ToString()))
                    {
                        tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                        tblDetail.AddCell(setCell(dtItem.Rows[i]["ProductSpecification"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf5, 6, Element.ALIGN_LEFT));
                        tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                        tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                        tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                    }
                }

                //for (int i = 1; i < (10-dtItem.Rows.Count); i++)
                //{
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //    tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                //}
                //PageBase pb = new PageBase();
                //string amountinwords = pb.ConvertToWords(totAmount.ToString("0.##"));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblBank = new PdfPTable(2);
                int[] column_tblBank = { 20, 80 };
                tblBank.SetWidths(column_tblBank);
                string xCompany = "", xBankName = "", xBranch = "", xAccountNo = "", xIFSC = "", xGSTNo = "";

                xCompany = (lstBank.Count > 0) ? (!String.IsNullOrEmpty(lstBank[0].OrgName) ? lstBank[0].OrgName : "") : "";
                xBankName = (lstBank.Count > 0) ? (!String.IsNullOrEmpty(lstBank[0].BankName) ? lstBank[0].BankName : "") : "";
                xBranch = (lstBank.Count > 0) ? (!String.IsNullOrEmpty(lstBank[0].BranchName) ? lstBank[0].BranchName : "") : "";
                xAccountNo = (lstBank.Count > 0) ? (!String.IsNullOrEmpty(lstBank[0].BankAccountNo) ? lstBank[0].BankAccountNo : "") : "";
                xIFSC = (lstBank.Count > 0) ? (!String.IsNullOrEmpty(lstBank[0].BankIFSC) ? lstBank[0].BankIFSC : "") : "";
                xGSTNo = (lstBank.Count > 0) ? (!String.IsNullOrEmpty(lstBank[0].GSTNo) ? lstBank[0].GSTNo : "") : "";
                // ---------------------------------------------------------------------------------------------------------
                tblBank.AddCell(setCell("Company", WhiteBaseColor, objContentFontTitleBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell(xCompany, WhiteBaseColor, objContentFontDataBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell("Bank Name", WhiteBaseColor, objContentFontTitleBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell(xBankName, WhiteBaseColor, objContentFontDataBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell("Branch", WhiteBaseColor, objContentFontTitleBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell(xBranch, WhiteBaseColor, objContentFontDataBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell("Bank A/c No", WhiteBaseColor, objContentFontTitleBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell(xAccountNo, WhiteBaseColor, objContentFontDataBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell("IFSC Code", WhiteBaseColor, objContentFontTitleBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell(xIFSC, WhiteBaseColor, objContentFontDataBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell("GST #", WhiteBaseColor, objContentFontTitleBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(setCell(xGSTNo, WhiteBaseColor, objContentFontDataBlack, paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // ---------------------------------------------------------------------------------------------------------

                PdfPTable tblAmount = new PdfPTable(2);
                int[] column_tblAmount = { 60, 40 };
                tblAmount.SetWidths(column_tblAmount);
                tblAmount.AddCell(setCell("Gross Amount ", WhiteBaseColor, objContentFontTitleBlack, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                tblAmount.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                tblAmount.AddCell(setCell("Tax Amount ", WhiteBaseColor, objContentFontTitleBlack, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                tblAmount.AddCell(setCell(taxAmount.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                tblAmount.AddCell(setCell("Total Amount ", WhiteBaseColor, objContentFontTitleBlack, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                tblAmount.AddCell(setCell(netAmount.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                tblDetail.AddCell(setCell(tblBank, WhiteBaseColor, objContentFontDataBlack, 0, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(setCell(tblAmount, WhiteBaseColor, objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                //tblDetail.AddCell(setCell("Rs. :" + amountinwords, WhiteBaseColor, objContentFontDataBlack, paddingOf3, 6, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Footer
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;


                tblFooter.AddCell(setCell("Terms & Conditions:", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                tblFooter.AddCell(setCell(lstQuot[0].TermsCondition, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                tblSignOff.AddCell(setCell("For, " + lstEntity[0].OrgName, WhiteBaseColor, objContentFontTitleBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 13));

                Phrase xPhrase = new Phrase("Authorised Signatory");
                xPhrase.Font = objContentFontDataBlack;
               
                cell = new PdfPCell(xPhrase);
                cell.Border = 14;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.PaddingTop = 25f;
                cell.PaddingBottom = 5f;
                cell.PaddingRight = 2f;
                cell.Colspan = 1;
                cell.BackgroundColor = WhiteBaseColor;
                tblSignOff.AddCell(cell);


                #endregion
            }

            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";



            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = objHeaderFont18;
            clHeaderFooter.FooterFont = objFooterFont;
            //clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
            //clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
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
            //objStyle.LoadTagStyle("div", "border", "1");
            //objStyle.LoadTagStyle("div", "color", "black");
            //objStyle.LoadTagStyle("div", "margin-top", "10");
            //objStyle.LoadTagStyle("div", "margin-bottom", "10");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Organization Name 
            tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            pdfDoc.Add(tableHeader);

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);

            // >>>>>> Adding Quotation Header
            tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSignOff);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // -------------------------------------------------------------------------------
            //pdfw.DirectContent.MoveTo(0, pdfDoc.PageSize.GetTop(35));
            //pdfw.DirectContent.LineTo(pdfDoc.PageSize.Width, pdfDoc.PageSize.GetTop(35));
            //pdfw.DirectContent.SetLineWidth(2);
            //pdfw.DirectContent.Stroke();

            // >>>>>> Adding Wallpaper
            //addWallPaper(pdfDoc, "images/Sanpar_Logo.png", 0, 0, 100, 100);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }

        private static PdfPCell setCell(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }

        private static PdfPCell setCell(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(objTable);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            return tmpCell;
        }



    }

    //class RoundedBorder1 : IPdfPCellEvent
    //{
    //    public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] canvas)
    //    {
    //        PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
    //        cb.RoundRectangle(
    //          rect.Left + 1.5f,
    //          rect.Bottom + 1.5f,
    //          rect.Width - 3,
    //          rect.Height - 3, 4
    //        );
    //        cb.Stroke();
    //    }
    //}

    //class ITextEvents : PdfPageEventHelper
    //{
    //    PdfContentByte cb;
    //    PdfTemplate pageNoTemplate;
    //    //BaseFont bf = null;
    //    iTextSharp.text.Image watermarkImage;

    //    #region Properties
    //    private string _Title;
    //    public string Title
    //    {
    //        get { return _Title; }
    //        set { _Title = value; }
    //    }

    //    private string _HeaderLeft;
    //    public string HeaderLeft
    //    {
    //        get { return _HeaderLeft; }
    //        set { _HeaderLeft = value; }
    //    }

    //    private string _HeaderRight;
    //    public string HeaderRight
    //    {
    //        get { return _HeaderRight; }
    //        set { _HeaderRight = value; }
    //    }

    //    private iTextSharp.text.Font _HeaderFont;
    //    public iTextSharp.text.Font HeaderFont
    //    {
    //        get { return _HeaderFont; }
    //        set { _HeaderFont = value; }
    //    }
    //    // ------------------------------------------------------
    //    private string _FooterText;
    //    public string FooterText
    //    {
    //        get { return _FooterText; }
    //        set { _FooterText = value; }
    //    }

    //    private iTextSharp.text.Font _FooterFont;
    //    public iTextSharp.text.Font FooterFont
    //    {
    //        get { return _FooterFont; }
    //        set { _FooterFont = value; }
    //    }

    //    private string _WallPaper;
    //    public string WallPaper
    //    {
    //        get { return _WallPaper; }
    //        set { _WallPaper = value; }
    //    }

    //    #endregion


    //    public override void OnOpenDocument(PdfWriter writer, Document document)
    //    {
    //        try
    //        {
    //            //watermarkImage = iTextSharp.text.Image.GetInstance(WallPaper);

    //            cb = writer.DirectContent;
    //            pageNoTemplate = cb.CreateTemplate(50, 50);
    //        }
    //        catch (DocumentException de) { }
    //        catch (System.IO.IOException ioe) { }
    //    }

    //    public override void OnStartPage(PdfWriter writer, Document document)
    //    {
    //        //base.OnStartPage(writer, document);
    //        //iTextSharp.text.Rectangle pageSize = document.PageSize;


    //        //float lmar = document.LeftMargin;
    //        //float rmar = document.RightMargin;

    //        //if (!String.IsNullOrEmpty(Title))
    //        //{
    //        //    cb.BeginText();
    //        //    cb.SetFontAndSize(HeaderFont.BaseFont , HeaderFont.Size);
    //        //    cb.SetColorFill(HeaderFont.Color);

    //        //    cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(25));
    //        //    cb.ShowText(Title);
    //        //    cb.EndText();

    //        //    cb.MoveTo(0, document.PageSize.GetTop(35));
    //        //    cb.LineTo(document.PageSize.Width, document.PageSize.GetTop(35));
    //        //    cb.SetLineWidth(2);
    //        //    cb.Stroke();
    //        //}

    //        //if (!String.IsNullOrEmpty(HeaderLeft + HeaderRight))
    //        //{
    //        //    PdfPTable HeaderTable = new PdfPTable(2);
    //        //    HeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
    //        //    HeaderTable.TotalWidth = pageSize.Width - (lmar + rmar);
    //        //    HeaderTable.SetWidthPercentage(new float[] { 50, 50 }, pageSize);

    //        //    PdfPCell HeaderLeftCell = new PdfPCell(new Phrase(8, HeaderLeft, HeaderFont));
    //        //    HeaderLeftCell.Padding = 5;
    //        //    HeaderLeftCell.PaddingBottom = 8;
    //        //    HeaderLeftCell.BorderWidthRight = 0;
    //        //    HeaderTable.AddCell(HeaderLeftCell);

    //        //    PdfPCell HeaderRightCell = new PdfPCell(new Phrase(8, HeaderRight, HeaderFont));
    //        //    HeaderRightCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
    //        //    HeaderRightCell.Padding = 5;
    //        //    HeaderRightCell.PaddingBottom = 8;
    //        //    HeaderRightCell.BorderWidthLeft = 0;
    //        //    HeaderTable.AddCell(HeaderRightCell);
    //        //    cb.SetColorFill(HeaderFont.Color);

    //        //    //cb.SetRGBColorFill(0, 0, 0);

    //        //    HeaderTable.WriteSelectedRows(0, -1, pageSize.GetLeft(lmar), pageSize.GetTop(50), cb);
    //        //}
    //    }

    //    public override void OnEndPage(PdfWriter writer, Document document)
    //    {
    //        base.OnEndPage(writer, document);

    //        //String strPageNo = "Page " + writer.PageNumber + " of ";
    //        //String strPrintDate = "Printed On " + DateTime.Now.ToString();

    //        //float lenPageNo = FooterFont.BaseFont.GetWidthPoint(strPageNo, 8);
    //        //float lenPrintDate = FooterFont.BaseFont.GetWidthPoint(strPrintDate, 8);
    //        //float lenFooterText = FooterFont.BaseFont.GetWidthPoint(FooterText, 8);
    //        //iTextSharp.text.Rectangle pageSize = document.PageSize;

    //        ////watermarkImage.SetAbsolutePosition(100, 100);
    //        ////cb.AddImage(watermarkImage);

    //        ////cb.SetRGBColorFill(255, 0, 0);    // Setting up font color
    //        //cb.SetColorFill(FooterFont.Color);

    //        //cb.BeginText();
    //        //cb.SetFontAndSize(FooterFont.BaseFont, 8);
    //        //cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(40));
    //        //cb.ShowText(strPageNo);
    //        //cb.EndText();
    //        //cb.AddTemplate(pageNoTemplate, document.LeftMargin + lenPageNo, pageSize.GetBottom(40));

    //        //cb.BeginText();
    //        //cb.SetFontAndSize(FooterFont.BaseFont, 8);
    //        //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strPrintDate, (document.PageSize.Width - (document.RightMargin + lenPrintDate)), pageSize.GetBottom(40), 0);
    //        //cb.EndText();

    //        //if (!String.IsNullOrEmpty(FooterText))
    //        //{
    //        //    float xPos = ((document.PageSize.Width - lenFooterText)/2);
    //        //    cb.BeginText();
    //        //    cb.SetFontAndSize(FooterFont.BaseFont, FooterFont.Size);
    //        //    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, FooterText, xPos, pageSize.GetBottom(15), 0);
    //        //    cb.EndText();
    //        //}

    //        ////Move the pointer and draw line to separate footer section from rest of page
    //        //cb.MoveTo(0, document.PageSize.GetBottom(30));
    //        //cb.LineTo(document.PageSize.Width, document.PageSize.GetBottom(30));
    //        //cb.Stroke();
    //    }

    //    public override void OnCloseDocument(PdfWriter writer, Document document)
    //    {
    //        base.OnCloseDocument(writer, document);
    //        pageNoTemplate.BeginText();
    //        pageNoTemplate.SetFontAndSize(FooterFont.BaseFont, 8);
    //        pageNoTemplate.SetTextMatrix(0, 0);
    //        pageNoTemplate.ShowText("" + (writer.PageNumber - 1));
    //        pageNoTemplate.EndText();
    //    }
    //}
}