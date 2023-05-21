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
    public partial class JobCardInward : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session.Remove("dtDetail");

                BindDropDown();
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
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
        }
        protected void edQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptInwardDetail.Controls[rptInwardDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));

            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            //TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            //TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            //TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            //TextBox txtDiscountAmt = ((TextBox)rptFootCtrl.FindControl("txtDiscountAmt"));
            //TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));
            ////TextBox txtAddTaxPer = ((TextBox)rptFootCtrl.FindControl("txtAddTaxPer"));
            ////TextBox txtAddTaxAmt = ((TextBox)rptFootCtrl.FindControl("txtAddTaxAmt"));
            //HiddenField hdnTaxType = ((HiddenField)rptFootCtrl.FindControl("hdnTaxType"));

            //HiddenField hdnCGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTPer"));
            //HiddenField hdnSGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTPer"));
            //HiddenField hdnIGSTPer = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTPer"));

            //HiddenField hdnCGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnCGSTAmt"));
            //HiddenField hdnSGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnSGSTAmt"));
            //HiddenField hdnIGSTAmt = ((HiddenField)rptFootCtrl.FindControl("hdnIGSTAmt"));

            //List<Entity.Products> lstEntity = new List<Entity.Products>();

            //if (!String.IsNullOrEmpty(hdnProductID.Value))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            //txtUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            ////hdnUnitQuantity.Value = (lstEntity.Count > 0) ? ((!String.IsNullOrEmpty(lstEntity[0].UnitQuantity.ToString())) ? lstEntity[0].UnitQuantity.ToString() : "1") : "1";
            //txtDiscountPercent.Text = "0";
            ////txtDiscountAmt.Text = "0";
            //txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
            //hdnTaxType.Value = (lstEntity.Count > 0) ? lstEntity[0].TaxType.ToString() : "0";

            //if (BAL.CommonMgmt.isIGST(hdnCustStateID.Value, Session["CompanyStateCode"].ToString()))
            //{
            //    hdnIGSTPer.Value = txtTaxRate.Text;
            //    hdnCGSTPer.Value = "0";
            //    hdnSGSTPer.Value = "0";
            //}
            //else
            //{
            //    hdnCGSTPer.Value = (Convert.ToDecimal(txtTaxRate.Text) / 2).ToString();
            //    hdnSGSTPer.Value = (Convert.ToDecimal(txtTaxRate.Text) / 2).ToString();
            //    hdnIGSTPer.Value = "0";
            //}
            // ----------------------------------------------------------------------------
            // Binding Customer's Purchase Order
            // -----------------------------------------------------
            DropDownList drpOutwardNo = (DropDownList)rptFootCtrl.FindControl("drpOutwardNo");
            drpOutwardNo.Items.Clear();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && !String.IsNullOrEmpty(hdnProductID.Value))
                drpOutwardNo.DataSource = BindOutwardList(Convert.ToInt64(hdnCustomerID.Value),Convert.ToInt64(hdnProductID.Value));
            else
                drpOutwardNo.DataSource = BindOutwardList(Convert.ToInt64("-1"), Convert.ToInt64("-1"));
            drpOutwardNo.DataTextField = "OutwardNo";
            drpOutwardNo.DataValueField = "OutwardNo";
            drpOutwardNo.DataBind();
            drpOutwardNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Outward No. # --", ""));
            //---------------------------------------------------
            txtQuantity.Focus();
            editItem_TextChanged1();
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            editItem_TextChanged1();
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


                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || 
                        String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0" ||
                        String.IsNullOrEmpty(((DropDownList)e.Item.FindControl("drpOutwardNo")).Text))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                            strErr += "<li>" + "Select Proper Product From List." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                            strErr += "<li>" + "Quantity is required." + "</li>";

                        if (String.IsNullOrEmpty(((DropDownList)e.Item.FindControl("drpOutwardNo")).Text))
                            strErr += "<li>" + "Outward No is mandatory For Product Inward" + "</li>";
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
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                                return;
                            }

                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = 0;
                            //string icode = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedValue;
                            //string iname = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedItem.Text;

                            string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                            string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                            string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                            string outwardno = ((DropDownList)e.Item.FindControl("drpOutwardNo")).SelectedValue;
                            //string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                            //string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                            //string disper = ((TextBox)e.Item.FindControl("txtDiscountPercent")).Text;
                            //string netrate = ((TextBox)e.Item.FindControl("txtNetRate")).Text;
                            //string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;
                            //string taxrate = ((TextBox)e.Item.FindControl("txtTaxRate")).Text;
                            //string taxamt = ((TextBox)e.Item.FindControl("txtTaxAmount")).Text;
                            //string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;

                            dr["InwardNo"] = txtInwardNo.Text;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["OutwardNo"] = (!String.IsNullOrEmpty(outwardno)) ? outwardno : "";
                            //dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            //dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                            //dr["DiscountPercent"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                            //dr["NetRate"] = (!String.IsNullOrEmpty(netrate)) ? Convert.ToDecimal(netrate) : 0;
                            //dr["Amount"] = (!String.IsNullOrEmpty(amt)) ? Convert.ToDecimal(amt) : 0;
                            //dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
                            //dr["TaxAmount"] = (!String.IsNullOrEmpty(taxamt)) ? Convert.ToDecimal(taxamt) : 0;
                            //dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;

                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptInwardDetail.DataSource = dtDetail;
                            rptInwardDetail.DataBind();
                            //----------------------------------------------------------------
                            Session.Add("dtDetail", dtDetail);

                        }
                    }
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
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

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
        }
        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            {

                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                //----------------------------------------------------------------
                int ReturnCode = 0, ReturnCode1 = 0;
                string ReturnMsg = "", ReturnMsg1 = "", ReturnInwardNo = "";

                string strErr = "";
                _pageValid = true;


                if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(txtInwardDate.Text) || hdnCustomerID.Value == "0")
                {
                    _pageValid = false;
                    if (String.IsNullOrEmpty(txtInwardDate.Text))
                        strErr += "<li>" + "Job Card Inward Date is required." + "</li>";

                    if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                        strErr += "<li>" + "Select Proper Customer From lIst." + "</li>";
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
                            Entity.JobCardInward objEntity = new Entity.JobCardInward();

                            if (!String.IsNullOrEmpty(hdnpkID.Value))
                                objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                            Int64 intLocation = (!String.IsNullOrEmpty(drpLocation.SelectedValue)) ? Convert.ToInt64(drpLocation.SelectedValue) : 0;
                            objEntity.InwardNo = txtInwardNo.Text;
                            objEntity.InwardDate = Convert.ToDateTime(txtInwardDate.Text);
                            objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                            objEntity.LocationID = intLocation;
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.JobCardInwardMgmt.AddUpdateJobCardInward(objEntity, out ReturnCode, out ReturnMsg, out ReturnInwardNo);
                            strErr += "<li>" + ReturnMsg + "</li>";

                            // --------------------------------------------------------------
                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            // --------------------------------------------------------------
                            if (ReturnCode > 0)
                            {
                                BAL.JobCardInwardMgmt.DeleteJobCardInwardDetailByInwardNo(ReturnInwardNo, out ReturnCode, out ReturnMsg);
                                Entity.JobCardInwardDetail objQuotDet = new Entity.JobCardInwardDetail();

                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    objQuotDet.pkID = 0;
                                    objQuotDet.InwardNo = ReturnInwardNo.Trim();
                                    objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                    objQuotDet.LocationID = intLocation;
                                    objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                    objQuotDet.OutwardNo = Convert.ToString(dr["OutwardNo"]);
                                    //objQuotDet.Unit = dr["Unit"].ToString();
                                    //objQuotDet.UnitRate = Convert.ToDecimal(dr["UnitRate"]);
                                    //objQuotDet.DiscountPercent = Convert.ToDecimal(dr["DiscountPercent"]);
                                    //objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                                    //objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);
                                    //objQuotDet.TaxRate = Convert.ToDecimal(dr["TaxRate"]);
                                    //objQuotDet.TaxAmount = Convert.ToDecimal(dr["TaxAmount"]);
                                    //objQuotDet.NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                                    objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                    BAL.JobCardInwardMgmt.AddUpdateJobCardInwardDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                                }
                                if (ReturnCode1 > 0)
                                {
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);

            }
        }
        public void ClearAllField()
        {
            Session.Remove("dtDetail");

            hdnpkID.Value = "";
            txtInwardNo.Text = "";
            txtInwardDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";
            BindInwardDetailList("");
            txtInwardDate.Focus();
            btnSave.Disabled = false;
        }
        public void BindInwardDetailList(string pInwardNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.JobCardInwardMgmt.GetJobCardInwardDetail(pInwardNo);
            rptInwardDetail.DataSource = dtDetail1;
            rptInwardDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.JobCardInward> lstEntity = new List<Entity.JobCardInward>();
                // ----------------------------------------------------
                lstEntity = BAL.JobCardInwardMgmt.GetJobCardInwardList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtInwardNo.Text = lstEntity[0].InwardNo;
                //txtInwardDate.Text = lstEntity[0].InwardDate.ToString("dd-MM-yyyy");
                txtInwardDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].InwardDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpLocation.SelectedValue = lstEntity[0].LocationID.ToString();
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
        }

        public void OnlyViewControls()
        {
            drpLocation.Attributes.Add("disabled", "disabled");
            txtInwardNo.ReadOnly = true;
            txtInwardDate.ReadOnly = true;
            //txtTermsCondition.ReadOnly = true;
            //drpCustomer.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            //drpQuotation.Attributes.Add("disabled", "disabled");
            //drpTNC.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            //TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
            //TextBox edDiscountPercent = (TextBox)item.FindControl("edDiscountPercent");
            //TextBox edNetRate = (TextBox)item.FindControl("edNetRate");
            //TextBox edAmount = (TextBox)item.FindControl("edAmount");
            //TextBox edTaxRate = (TextBox)item.FindControl("edTaxRate");
            //TextBox edTaxAmount = (TextBox)item.FindControl("edTaxAmount");
            //TextBox edNetAmount = (TextBox)item.FindControl("edNetAmount");
            // --------------------------------------------------------------------------
            Decimal q = Convert.ToDecimal(edQuantity.Text);
            //Decimal ur = Convert.ToDecimal(edUnitRate.Text);
            //Decimal dp = Convert.ToDecimal(edDiscountPercent.Text);
            //Decimal nr = Convert.ToDecimal(edNetRate.Text);
            //Decimal a = Convert.ToDecimal(edAmount.Text);
            //Decimal tr = Convert.ToDecimal(edTaxRate.Text);
            //Decimal ta = Convert.ToDecimal(edTaxAmount.Text);
            //Decimal na = Convert.ToDecimal(edNetAmount.Text);
            // --------------------------------------------------------------------------
            //nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            //a = Math.Round((q * nr), 2);
            //ta = Math.Round(((a * tr) / 100), 2);
            //na = Math.Round((a + ta), 2);

            //edNetRate.Text = nr.ToString();
            //edAmount.Text = a.ToString();
            //edTaxAmount.Text = ta.ToString();
            //edNetAmount.Text = na.ToString();
            // --------------------------------------------------------------------------

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductName"].ToString() == edProductName.Value)
                {
                    row.SetField("Quantity", edQuantity.Text);
                    //row.SetField("UnitRate", edUnitRate.Text);
                    //row.SetField("DiscountPercent", edDiscountPercent.Text);
                    //row.SetField("NetRate", edNetRate.Text);
                    //row.SetField("Amount", edAmount.Text);
                    //row.SetField("TaxRate", edTaxRate.Text);
                    //row.SetField("TaxAmount", edTaxAmount.Text);
                    //row.SetField("NetAmount", edNetAmount.Text);
                }
            }
            rptInwardDetail.DataSource = dtDetail;
            rptInwardDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
        }
        protected void editItem_TextChanged1()
        {
            Control rptFootCtrl = rptInwardDetail.Controls[rptInwardDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            TextBox txtQuantity = (TextBox)rptFootCtrl.FindControl("txtQuantity");
            //TextBox txtUnitRate = (TextBox)rptFootCtrl.FindControl("txtUnitRate");
            //TextBox txtDiscountPercent = (TextBox)rptFootCtrl.FindControl("txtDiscountPercent");
            //TextBox txtNetRate = (TextBox)rptFootCtrl.FindControl("txtNetRate");
            //TextBox txtAmount = (TextBox)rptFootCtrl.FindControl("txtAmount");
            //TextBox txtTaxRate = (TextBox)rptFootCtrl.FindControl("txtTaxRate");
            //TextBox txtTaxAmount = (TextBox)rptFootCtrl.FindControl("txtTaxAmount");
            //TextBox txtNetAmount = (TextBox)rptFootCtrl.FindControl("txtNetAmount");
            // --------------------------------------------------------------------------
            Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            //Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);
            //Decimal dp = String.IsNullOrEmpty(txtDiscountPercent.Text) ? 0 : Convert.ToDecimal(txtDiscountPercent.Text);
            //Decimal nr = String.IsNullOrEmpty(txtNetRate.Text) ? 0 : Convert.ToDecimal(txtNetRate.Text);
            //Decimal a = String.IsNullOrEmpty(txtAmount.Text) ? 0 : Convert.ToDecimal(txtAmount.Text);
            //Decimal tr = String.IsNullOrEmpty(txtTaxRate.Text) ? 0 : Convert.ToDecimal(txtTaxRate.Text);
            //Decimal ta = String.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
            //Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            // --------------------------------------------------------------------------
            //nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            //a = Math.Round((q * nr), 2);
            //ta = Math.Round(((a * tr) / 100), 2);
            //na = Math.Round((a + ta), 2);

            //txtNetRate.Text = nr.ToString();
            //txtAmount.Text = a.ToString();
            //txtTaxAmount.Text = ta.ToString();
            //txtNetAmount.Text = na.ToString();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteJobCardInward(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.JobCardInwardMgmt.DeleteJobCardInward(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void rptInwardDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList drpOutwardNo = (DropDownList)e.Item.FindControl("drpOutwardNo");
                HiddenField hdnProductID = ((HiddenField)e.Item.FindControl("hdnProductID"));
                drpOutwardNo.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && !String.IsNullOrEmpty(hdnProductID.Value))
                    drpOutwardNo.DataSource = BindOutwardList(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value));
                else
                    drpOutwardNo.DataSource = BindOutwardList(Convert.ToInt64("-1"), Convert.ToInt64("-1"));
                drpOutwardNo.DataTextField = "OutwardNo";
                drpOutwardNo.DataValueField = "OutwardNo";
                drpOutwardNo.DataBind();
                drpOutwardNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Outward No # --", ""));
            }
        }
        public List<Entity.JobCardOutward> BindOutwardList(Int64 pCustomerID, Int64 pProductID)
        {
            List<Entity.JobCardOutward> lstPurcOrder = new List<Entity.JobCardOutward>();
            lstPurcOrder = BAL.JobCardOutwardMgmt.GetJobCardOutwardList(pCustomerID, pProductID, Session["LoginUserID"].ToString(),  0, 0);
            return lstPurcOrder;
        }
    }
}