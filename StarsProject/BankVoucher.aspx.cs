using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
//using iTextSharp.text;
//using DocumentFormat.OpenXml.Spreadsheet;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;
using System.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace StarsProject
{
    public partial class BankVoucher : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session.Remove("dtTransaction");
                // --------------------------------------------------------
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    // -----------------------------------------------------------
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
                // --------------------------------------------
                lblAccountName.InnerText = "Bank A/c";
                string tmpStr = "Customer A/c";
                tmpStr += "<small class='text-muted font-italic'>&nbsp;(Min 3 chars to search)</small><span class='materialize-red-text font-weight-800'>*</span>";
                tmpStr += "<a href='javascript:openCustomerInfo(\'view\');'><img src='images/registration.png' width='30' height='20' alt='Preview Customer Info' title='Preview Customer Info' style='display: inline-block;' Tabindex='3' /></a>";
                tmpStr += "<a href='javascript:openCustomerInfo(\'add\');'><img src='images/addCustomer.png' width='30' height='20' alt='Add New Customer' title='Add New Customer' /></a>";
                lblCustomerName.InnerHtml = tmpStr;
            }
        }

        public void OnlyViewControls()
        {
            drpVoucherType.Attributes.Add("disabled", "disabled");
            drpRecPay.Attributes.Add("disabled", "disabled");
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.ReadOnly = true;
            txtAccountName.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            rdblTransType.Attributes.Add("disabled", "disabled");
            drpTransMode.Attributes.Add("disabled", "disabled");
            txtTransID.ReadOnly = true;
            txtTransDate.ReadOnly = true;
            txtVoucherAmount.ReadOnly = true;
            txtBankName.ReadOnly = true;
            txtTransRemark.ReadOnly = true;
            drpTerminationOfDelivery.Attributes.Add("disabled", "disabled");
            rdblRDURD.Attributes.Add("disabled", "disabled");

            drpTaxPer.Attributes.Add("disabled", "disabled");
            txtTotBasicAmt.ReadOnly = true;
            txtTotGST.ReadOnly = true;
            txtTotNetAmt.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            List<Entity.Wallet> lstCustomer = new List<Entity.Wallet>();
            lstCustomer = BAL.WalletMgmt.GetWalletList();
            drpTransMode.DataSource = lstCustomer;
            drpTransMode.DataValueField = "pkID";
            drpTransMode.DataTextField = "WalletName";
            drpTransMode.DataBind();
            drpTransMode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select TransactionMode --", ""));

            // ---------------- State List -------------------------------------
            List<Entity.State> lstEvents = new List<Entity.State>();
            lstEvents = BAL.StateMgmt.GetStateList();
            drpTerminationOfDelivery.DataSource = lstEvents;
            drpTerminationOfDelivery.DataValueField = "StateCode";
            drpTerminationOfDelivery.DataTextField = "StateName";
            drpTerminationOfDelivery.DataBind();
            drpTerminationOfDelivery.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All State --", ""));

            List<Entity.OrganizationEmployee> lstOrg = new List<Entity.OrganizationEmployee>();
            lstOrg = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstOrg;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Employee --", ""));

        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.FinancialTrans> lstEntity = new List<Entity.FinancialTrans>();
                // -----------------------------------------------------------------------------------

                lstEntity = BAL.FinancialTransMgmt.GetFinancialTransList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();

                drpVoucherType.SelectedValue = lstEntity[0].VoucherType.ToString();
                drpRecPay.SelectedValue = lstEntity[0].RecPay.ToString();
                txtVoucherNo.Text = lstEntity[0].VoucherNo.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                //txtVoucherDate.Text = lstEntity[0].VoucherDate.ToString("dd-MM-yyyy");
                txtVoucherDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].VoucherDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnAccountID.Value = lstEntity[0].AccountID.ToString();
                txtAccountName.Text = lstEntity[0].AccountName.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                hdnTDSAccountID.Value = lstEntity[0].TDSAccountID.ToString();
                txtTDSAccountID.Text = lstEntity[0].TDSAccountName.ToString();
                rdblTransType.SelectedValue = lstEntity[0].TransType.ToString();
                drpTransMode.SelectedValue = lstEntity[0].TransModeID.ToString();
                txtTransID.Text = lstEntity[0].TransID.ToString();
                //txtTransDate.Text = lstEntity[0].TransDate.ToString("dd-MM-yyyy");
                txtTransDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].TransDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                txtVoucherAmount.Text = lstEntity[0].VoucherAmount.ToString();
                txtTDSAmount.Text = lstEntity[0].TDSAmount.ToString();
                txtBankName.Text = lstEntity[0].BankName.ToString();
                txtTransRemark.Text = lstEntity[0].Remark.ToString();
                if (drpRecPay.SelectedValue == "Payable")
                {
                    drpTerminationOfDelivery.SelectedValue = lstEntity[0].TerminationOfDelivery.ToString();
                    rdblRDURD.SelectedValue = lstEntity[0].RDURD.ToString();

                    drpTaxPer.SelectedValue = lstEntity[0].TaxPer.ToString();

                    hdnCGSTPer.Value = lstEntity[0].CGSTPer.ToString();
                    hdnSGSTPer.Value = lstEntity[0].SGSTPer.ToString();
                    hdnIGSTPer.Value = lstEntity[0].IGSTPer.ToString();

                    txtTotBasicAmt.Text = lstEntity[0].BasicAmt.ToString();
                    txtTotGST.Text = lstEntity[0].GSTAmt.ToString();

                    hdnCGSTAmt.Value = lstEntity[0].CGSTAmt.ToString();
                    hdnSGSTAmt.Value = lstEntity[0].SGSTAmt.ToString();
                    hdnIGSTAmt.Value = lstEntity[0].IGSTAmt.ToString();

                    txtTotNetAmt.Text = lstEntity[0].NetAmt.ToString();


                }
                // -----------------------------------
                BindPartialTransaction();
                // -----------------------------------
                drpRecPay_SelectedIndexChanged(null, null);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            string strErr = "";
            _pageValid = true;

            DataTable dtTransaction = new DataTable();
            dtTransaction = (DataTable)Session["dtTransaction"];

            if ((String.IsNullOrEmpty(txtVoucherDate.Text) || String.IsNullOrEmpty(drpRecPay.SelectedValue) || String.IsNullOrEmpty(txtVoucherAmount.Text) ? 0 : Convert.ToDecimal(txtVoucherAmount.Text)) == 0 || String.IsNullOrEmpty(hdnAccountID.Value) || String.IsNullOrEmpty(hdnCustomerID.Value) || String.IsNullOrEmpty(txtTransRemark.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtVoucherDate.Text))
                    strErr += "<li>" + "Voucher Date is required. " + "</li>";

                if (String.IsNullOrEmpty(hdnAccountID.Value))
                    strErr += "<li>" + "Please Select Proper Debit Account " + "</li>";

                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Please Select Proper Credit Account " + "</li>";

                if ((String.IsNullOrEmpty(txtVoucherAmount.Text) ? 0 : Convert.ToDecimal(txtVoucherAmount.Text)) == 0)
                    strErr += "<li>" + "Voucher amount must be greater than zero" + "</li>";

                if (String.IsNullOrEmpty(drpTransMode.SelectedValue))
                    strErr += "<li>" + "Please Select Transaction Mode " + "</li>";

                if (String.IsNullOrEmpty(txtTransDate.Text))
                    strErr += "<li>" + "Transaction Date/Cheque Date should not be blank " + "</li>";

                if (String.IsNullOrEmpty(txtTransRemark.Text))
                    strErr += "<li>" + "Transaction Notes is required. " + "</li>";
            }

            if (!String.IsNullOrEmpty(txtVoucherDate.Text) && !String.IsNullOrEmpty(txtTransDate.Text))
            {
                if (Convert.ToDateTime(txtTransDate.Text) < Convert.ToDateTime(txtVoucherDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Transaction Date/Cheque Date should be greater than Voucher Date." + "</li>";
                }

            }

            if (drpRecPay.SelectedValue == "Payable")
            {
                if (String.IsNullOrEmpty(drpTerminationOfDelivery.SelectedValue))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Please Select Delivery State. " + "</li>";
                }

            }
            // --------------------------------------------------------------
            if (dtTransaction != null)
            {
                if (dtTransaction.Rows.Count > 0)
                {
                    Decimal VouAmt = 0, BillVouAmt = 0;
                    VouAmt = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;
                    BillVouAmt = dtTransaction.AsEnumerable().Sum(r => r.Field<decimal>("Amount"));
                    if (VouAmt != BillVouAmt)
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Distributed Amount is Not Matching with Payment Amount !" + "</li>";
                    }
                }
            }

            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtVoucherDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtVoucherDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Voucher Date is Not Valid." + "</li>";
                }
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.FinancialTrans objEntity = new Entity.FinancialTrans();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.VoucherType = (!String.IsNullOrEmpty(drpVoucherType.SelectedValue)) ? drpVoucherType.SelectedValue : "";
                objEntity.RecPay = (!String.IsNullOrEmpty(drpRecPay.SelectedValue)) ? drpRecPay.SelectedValue : "";
                objEntity.VoucherNo = (!String.IsNullOrEmpty(txtVoucherNo.Text)) ? txtVoucherNo.Text : "";
                objEntity.VoucherDate = Convert.ToDateTime(txtVoucherDate.Text);
                objEntity.AccountID = (!String.IsNullOrEmpty(hdnAccountID.Value)) ? Convert.ToInt64(hdnAccountID.Value) : 0;
                objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                objEntity.TDSAccountID = (!String.IsNullOrEmpty(hdnTDSAccountID.Value)) ? Convert.ToInt64(hdnTDSAccountID.Value) : 0;
                objEntity.EmployeeID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0;
                objEntity.TransType = (!String.IsNullOrEmpty(rdblTransType.SelectedValue)) ? rdblTransType.SelectedValue : "";
                objEntity.TransModeID = (!String.IsNullOrEmpty(drpTransMode.SelectedValue)) ? Convert.ToInt64(drpTransMode.SelectedValue) : 0;
                objEntity.TransID = (!String.IsNullOrEmpty(txtTransID.Text)) ? txtTransID.Text : "";
                objEntity.TransDate = Convert.ToDateTime(txtTransDate.Text);
                objEntity.TDSAmount = (!String.IsNullOrEmpty(txtTDSAmount.Text)) ? Convert.ToDecimal(txtTDSAmount.Text) : 0;
                objEntity.VoucherAmount = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;
                objEntity.BankName = (!String.IsNullOrEmpty(txtBankName.Text)) ? txtBankName.Text : "";
                objEntity.Remark = (!String.IsNullOrEmpty(txtTransRemark.Text)) ? txtTransRemark.Text : "";
                objEntity.NetAmt = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;
                objEntity.BasicAmt = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;
                if (drpRecPay.SelectedValue == "Payable")
                {
                    objEntity.TerminationOfDelivery = (!String.IsNullOrEmpty(drpTerminationOfDelivery.SelectedValue)) ? Convert.ToInt64(drpTerminationOfDelivery.SelectedValue) : 0;
                    objEntity.RDURD = (!String.IsNullOrEmpty(rdblRDURD.SelectedValue)) ? rdblRDURD.SelectedValue : "";
                    //objEntity.TaxPer = (!String.IsNullOrEmpty(drpTaxPer.SelectedValue)) ? Convert.ToDecimal(drpTaxPer.SelectedValue) : 0;
                    objEntity.CGSTPer = (!String.IsNullOrEmpty(hdnCGSTPer.Value)) ? Convert.ToDecimal(hdnCGSTPer.Value) : 0;
                    objEntity.SGSTPer = (!String.IsNullOrEmpty(hdnSGSTPer.Value)) ? Convert.ToDecimal(hdnSGSTPer.Value) : 0;
                    objEntity.IGSTPer = (!String.IsNullOrEmpty(hdnIGSTPer.Value)) ? Convert.ToDecimal(hdnIGSTPer.Value) : 0;
                    objEntity.CGSTAmt = (!String.IsNullOrEmpty(hdnCGSTAmt.Value)) ? Convert.ToDecimal(hdnCGSTAmt.Value) : 0;
                    objEntity.SGSTAmt = (!String.IsNullOrEmpty(hdnSGSTAmt.Value)) ? Convert.ToDecimal(hdnSGSTAmt.Value) : 0; ;
                    objEntity.IGSTAmt = (!String.IsNullOrEmpty(hdnIGSTAmt.Value)) ? Convert.ToDecimal(hdnIGSTAmt.Value) : 0; ;
                    objEntity.BasicAmt = (!String.IsNullOrEmpty(txtTotBasicAmt.Text)) ? Convert.ToDecimal(txtTotBasicAmt.Text) : 0;
                    //objEntity.GSTAmt = (!String.IsNullOrEmpty(txtTotGST.Text)) ? Convert.ToDecimal(txtTotGST.Text) : 0;
                    objEntity.NetAmt = (!String.IsNullOrEmpty(txtTotNetAmt.Text)) ? Convert.ToDecimal(txtTotNetAmt.Text) : 0;
                }
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.FinancialTransMgmt.AddUpdateFinancialTrans(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    int ReturnCode1 = 0;
                    string ReturnMsg1 = "";
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    // SAVE - Bank Partial Transaction 
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                    // --------------------------------------------------------------
                    if (dtTransaction != null)
                    {
                        BAL.FinancialTransMgmt.DeleteFinancialTransDetailByParentID(ReturnCode, out ReturnCode1, out ReturnMsg1);

                        Entity.FinancialTransDetail objEntity1 = new Entity.FinancialTransDetail();
                        foreach (DataRow dr in dtTransaction.Rows)
                        {
                            objEntity1.pkID = 0;
                            objEntity1.ParentID = Convert.ToInt64(ReturnCode);
                            objEntity1.InvoiceNo = dr["InvoiceNo"].ToString();
                            objEntity1.Amount = Convert.ToDecimal(dr["Amount"].ToString());
                            objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.FinancialTransMgmt.AddUpdateFinancialTransDetail(objEntity1, out ReturnCode1, out ReturnMsg1);
                        }
                        if (ReturnCode > 0)
                        {
                            strErr += "<li>" + ReturnMsg1 + "</li>";
                            Session.Remove("dtTransaction");
                        }
                    }
                    btnSave.Disabled = true;
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            Session.Remove("dtTransaction");
            // ------------------------------------------
            hdnpkID.Value = "";
            hdnAccountID.Value = "";
            hdnCustomerID.Value = "";
            hdnCustStateID.Value = "";
            drpRecPay.SelectedValue = "Receivable";
            pnlPayment.Visible = false;

            drpVoucherType.SelectedValue = "Bank";
            txtVoucherNo.Text = "";
            txtVoucherDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtAccountName.Text = "";
            txtCustomerName.Text = "";
            rdblTransType.SelectedValue = "";
            drpTransMode.SelectedValue = "";
            drpEmployee.SelectedValue = "";
            txtTransID.Text = "";
            txtTransDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtVoucherAmount.Text = "";
            txtBankName.Text = "";
            txtTransRemark.Text = "";
            drpTerminationOfDelivery.SelectedValue = "";
            //rdblRDURD.SelectedValue = "";

            txtTotBasicAmt.Text = "";
            txtTotGST.Text = "";
            txtTotNetAmt.Text = "";
            hdnCGSTPer.Value = "";
            hdnSGSTPer.Value = "";
            hdnIGSTPer.Value = "";
            hdnCGSTAmt.Value = "";
            hdnSGSTAmt.Value = "";
            hdnIGSTAmt.Value = "";
            // -------------------------------------------------
            BindPartialTransaction();

            btnSave.Disabled = false;
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (drpRecPay.SelectedValue == "Payable")
            {

                int totalrecord;
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);
                if (lstEntity.Count > 0)
                {
                    hdnCustStateID.Value = (!String.IsNullOrEmpty(lstEntity[0].StateCode)) ? Convert.ToInt64(lstEntity[0].StateCode).ToString() : "0";
                    if ((!String.IsNullOrEmpty(hdnCustStateID.Value)) && Convert.ToInt64(hdnCustStateID.Value) > 0)
                    {
                        drpTerminationOfDelivery.SelectedValue = hdnCustStateID.Value;
                    }
                }
            }
            // --------------------------------------
            Int64 CustID = !String.IsNullOrEmpty(hdnCustomerID.Value) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
            Int64 AccID = !String.IsNullOrEmpty(hdnAccountID.Value) ? Convert.ToInt64(hdnAccountID.Value) : 0;
            // --------------------------------------
            Control myTemp = rptPartialTrans.Controls[rptPartialTrans.Controls.Count - 1].Controls[0];
            DropDownList drpInvoiceNo = (DropDownList)myTemp.FindControl("drpInvoiceNo");
            drpInvoiceNo.Items.Clear();
            if (drpRecPay.SelectedValue == "Payable")
                drpInvoiceNo.DataSource = BAL.PurchaseBillMgmt.GetPurchasePendingBillsByCustomerID(CustID);
            else
                drpInvoiceNo.DataSource = BAL.SalesBillMgmt.GetSalesPendingBillsByCustomerID(CustID);

            drpInvoiceNo.DataTextField = "InvoiceNo";
            drpInvoiceNo.DataValueField = "InvoiceNo";
            drpInvoiceNo.DataBind();
            drpInvoiceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }

        [System.Web.Services.WebMethod]
        public static string DeleteFinancialTrans(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FinancialTransMgmt.DeleteFinancialTrans(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpRecPay_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnPayRec.Value = drpRecPay.SelectedValue;
            pnlPayment.Visible = (drpRecPay.SelectedValue == "Payable") ? true : false;
            funCalcTot();
            // -------------------------------------------------
            string tmpStr = "Customer A/c";
            tmpStr += "<small class='text-muted font-italic'>&nbsp;(Min 3 chars to search)</small><span class='materialize-red-text font-weight-800'>*</span>";
            tmpStr += "<a href='javascript:openCustomerInfo(\'view\');'><img src='images/registration.png' width='30' height='20' alt='Preview Customer Info' title='Preview Customer Info' style='display: inline-block;' Tabindex='3' /></a>";
            tmpStr += "<a href='javascript:openCustomerInfo(\'add\');'><img src='images/addCustomer.png' width='30' height='20' alt='Add New Customer' title='Add New Customer' /></a>";
            // -------------------------------------------------
            lblCustomerName.InnerHtml = tmpStr;
            lblAccountName.InnerHtml = "Bank A/c";
            txtCustomerName_TextChanged(null, null);
        }

        [System.Web.Services.WebMethod]
        public static string FilterBank(string pBankName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.FinancialTransMgmt.GetBankListByName(pBankName).Select(sel => new { sel.BankName });
            return serializer.Serialize(rows);
        }

        protected void drpTerminationOfDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCustStateID.Value = drpTerminationOfDelivery.SelectedValue;
            funCalcTot();
        }

        private void funCalcTot()
        {
            Int16 taxtype = 0;
            if (rdblRDURD.SelectedValue == "rd")
                taxtype = 0;
            else if (rdblRDURD.SelectedValue == "urd")
                taxtype = 1;

            Decimal taxper = (!String.IsNullOrEmpty(drpTaxPer.SelectedValue)) ? Convert.ToDecimal(drpTaxPer.SelectedValue) : 0;
            decimal basicval = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;

            decimal TaxAmt = 0;
            decimal CGSTPer = 0, CGSTAmt = 0;
            decimal SGSTPer = 0, SGSTAmt = 0, IGSTPer = 0, IGSTAmt = 0, NetRate = 0, BasicAmt = 0, NetAmt = 0, ItmDiscPer1 = 0, ItmDiscAmt1 = 0, AddTaxAmt = 0, HeadDiscAmt1 = 0;
            BAL.CommonMgmt.funCalculate(taxtype, 1, basicval, 0, 0, taxper, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out AddTaxAmt);

            txtTotBasicAmt.Text = BasicAmt.ToString("0.00");
            txtTotGST.Text = (CGSTAmt + SGSTAmt + IGSTAmt).ToString("0.00");
            txtTotNetAmt.Text = (BasicAmt + CGSTAmt + SGSTAmt + IGSTAmt).ToString("0.00");

            hdnIGSTPer.Value = IGSTPer.ToString();
            hdnIGSTAmt.Value = IGSTAmt.ToString();
            hdnCGSTPer.Value = CGSTPer.ToString();
            hdnSGSTPer.Value = SGSTPer.ToString();
            hdnCGSTAmt.Value = CGSTAmt.ToString();
            hdnSGSTAmt.Value = SGSTAmt.ToString();

        }

        protected void txtVoucherAmount_TextChanged(object sender, EventArgs e)
        {
            funCalcTot();
        }

        protected void rdblRDURD_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCalcTot();
        }

        protected void drpTaxPer_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCalcTot();
        }

        public void BindPartialTransaction()
        {
            int TotalRecord = 0;
            hdnpkID.Value = (!String.IsNullOrEmpty(hdnpkID.Value)) ? hdnpkID.Value : "-1";

            DataTable dtTransaction = new DataTable();
            List<Entity.FinancialTransDetail> lstDetail = new List<Entity.FinancialTransDetail>();
            lstDetail = BAL.FinancialTransMgmt.GetFinancialTransDetailList(Convert.ToInt64(hdnpkID.Value), "", Session["LoginUserID"].ToString(), 1, 100000, out TotalRecord);
            dtTransaction = PageBase.ConvertListToDataTable(lstDetail);
            Session.Add("dtTransaction", dtTransaction);
            rptPartialTrans.DataSource = lstDetail;
            rptPartialTrans.DataBind();

        }

        protected void rptPartialTrans_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";

            if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // -------------------------------------------------------------- Delete Record
                BAL.FinancialTransMgmt.DeleteFinancialTransDetail(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                // -------------------------------------------------------------------------
                BindPartialTransaction();

            }
            else if (e.CommandName.ToString() == "Save")
            {
                DataTable dtTransaction = new DataTable();
                dtTransaction = (DataTable)Session["dtTransaction"];

                if (dtTransaction == null)
                {
                    int TotalRecord = 0;
                    List<Entity.FinancialTransDetail> lstDetail = new List<Entity.FinancialTransDetail>();
                    lstDetail = BAL.FinancialTransMgmt.GetFinancialTransDetailList(Convert.ToInt64(hdnpkID.Value), "", Session["LoginUserID"].ToString(), 1, 100000, out TotalRecord);
                    dtTransaction = PageBase.ConvertListToDataTable(lstDetail);
                    Session.Add("dtTransaction", dtTransaction);
                }
                DropDownList drpInvoiceNo = ((DropDownList)e.Item.FindControl("drpInvoiceNo"));
                TextBox txtAmount = ((TextBox)e.Item.FindControl("txtAmount"));

                bool _Valid = true;
                // ----------------------------------------------------------------
                if ((String.IsNullOrEmpty(drpInvoiceNo.SelectedValue) || drpInvoiceNo.SelectedValue == "0"))
                {
                    _Valid = false;
                    strErr += "<li>" + "Invoice No is Mandatory." + "</li>";
                }
                if (String.IsNullOrEmpty(txtAmount.Text) || txtAmount.Text == "0")
                {
                    _Valid = false;
                    strErr += "<li>" + "Amount is Mandatory." + "</li>";
                }
                // ----------------------------------------------------------------
                for (int i = 0; i < dtTransaction.Rows.Count; i++)
                {
                    if (drpInvoiceNo.SelectedValue.ToString() == dtTransaction.Rows[i]["InvoiceNo"].ToString())
                    {
                        _Valid = false;
                        strErr += "<li>" + "Same Invoice No can not add for bill wise payment for the same entry." + "</li>";
                        break;
                    }
                }

                if (_Valid)
                {
                    DataRow dr = dtTransaction.NewRow();
                    dr["pkID"] = 0;
                    dr["ParentID"] = (!String.IsNullOrEmpty(hdnpkID.Value)) ? Convert.ToInt64(hdnpkID.Value) : 0;
                    dr["InvoiceNo"] = drpInvoiceNo.SelectedValue;
                    dr["Amount"] = Convert.ToDecimal(txtAmount.Text);
                    dtTransaction.Rows.Add(dr);
                    dtTransaction.AcceptChanges();
                }
                Session.Add("dtTransaction", dtTransaction);
                // ------------------------------------------------
                rptPartialTrans.DataSource = dtTransaction;
                rptPartialTrans.DataBind();
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);

        }

        protected void rptPartialTrans_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                {
                    DropDownList ddl = ((DropDownList)e.Item.FindControl("drpInvoiceNo"));
                    ddl.DataSource = BAL.SalesBillMgmt.GetSalesBillNoByCustomerID(Convert.ToInt64(hdnCustomerID.Value));
                    if (drpRecPay.SelectedValue == "Payable")
                        ddl.DataSource = BAL.PurchaseBillMgmt.GetPurchasePendingBillsByCustomerID(Convert.ToInt64(hdnCustomerID.Value));
                    else
                        ddl.DataSource = BAL.SalesBillMgmt.GetSalesPendingBillsByCustomerID(Convert.ToInt64(hdnCustomerID.Value));

                    ddl.DataValueField = "InvoiceNo";
                    ddl.DataTextField = "InvoiceNo";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                }
            }
        }

        protected void drpInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rptPartialTrans.Controls.Count > 0)
            {
                Int64 CustID = !String.IsNullOrEmpty(hdnCustomerID.Value) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                // ----------------------------------------------------------------------------------
                Control FooterTemplate = rptPartialTrans.Controls[rptPartialTrans.Controls.Count - 1].Controls[0];
                DropDownList drpInvoiceNo = (DropDownList)FooterTemplate.FindControl("drpInvoiceNo");
                TextBox txtAmount = (TextBox)FooterTemplate.FindControl("txtAmount");
                // ----------------------------------------------------------------------------------
                DataTable dtTemp = new DataTable();
                if (drpRecPay.SelectedValue == "Payable")
                    dtTemp = BAL.PurchaseBillMgmt.GetPurchasePendingBillsByCustomerID(CustID);
                else
                    dtTemp = BAL.SalesBillMgmt.GetSalesPendingBillsByCustomerID(CustID);
                // -----------------------------------------------------
                if (dtTemp != null)
                {
                    if (drpRecPay.SelectedValue == "Payable")
                        txtAmount.Text = BAL.PurchaseBillMgmt.GetPurchasePendingBillsAmount(drpInvoiceNo.SelectedValue).ToString();
                    else
                        txtAmount.Text = BAL.SalesBillMgmt.GetSalesPendingBillsAmount(drpInvoiceNo.SelectedValue).ToString();
                }
            }
        }
        [WebMethod]
        public static string GetVoucherNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetVoucherNoForPDF(pkID);
            return tempVal;
        }

        [WebMethod(EnableSession = true)]
        public static void GenerateCashVoucher(Int64 pkID)
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



            if (tmpSerialKey == "SI08-SB94-MY45-RY15")          // Sharvaya 
            {
                GenerateCashVoucherPdf(pkID);
            }
            else
            {
                GenerateCashVoucherPdf(pkID);
            }

        }

        public static void GenerateCashVoucherPdf(Int64 pkID)
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
            PdfPTable tblDetail = new PdfPTable(3);
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
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "BankVoucher");

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
                        LeftMargin = Convert.ToInt64(tmpary[2].ToString());
                        RightMargin = Convert.ToInt64(tmpary[3].ToString());
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                    {
                        String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                        TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                        BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                        LeftMargin = Convert.ToInt64(tmpary[2].ToString());
                        RightMargin = Convert.ToInt64(tmpary[3].ToString());
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
            List<Entity.FinancialTrans> lstin = new List<Entity.FinancialTrans>();
            lstin = BAL.FinancialTransMgmt.GetFinancialTransList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //--------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            List<Entity.FinancialTransDetail> lstDetail = new List<Entity.FinancialTransDetail>();
            lstDetail = BAL.FinancialTransMgmt.GetFinancialTransDetailList(pkID,"", HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            dtItem = PageBase.ConvertListToDataTable(lstDetail);
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

                PdfPTable tblAddress = new PdfPTable(4);
                int[] column_tblAddress = {15,35,15,35};
                tblAddress.SetWidths(column_tblAddress);

                tblAddress.AddCell(pdf.setCell("Voucher No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(": " + lstin[0].VoucherNo, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("Voucher Date", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(": " + lstin[0].VoucherDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblMember.AddCell(pdf.setCell(objAuth.OrgName, pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell((lstin[0].RecPay == "Payable") ? "Payment Voucher" : "Receipt Voucher" , pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblAddress, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Material Movement Detail
                // -------------------------------------------------------------------------------------
                int[] column_tblDetail1 = { 10,60, 30 };
                tblDetail.SetWidths(column_tblDetail1);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                tblDetail.AddCell(pdf.setCell("Particulars", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7));

                tblDetail.AddCell(pdf.setCell("Account :", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));

                if (dtItem.Rows.Count > 0)
                {
                    Decimal Amount = 0;

                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                    tblDetail.AddCell(pdf.setCell((lstin[0].RecPay == "Payable") ? lstin[0].CustomerName : lstin[0].AccountName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));

                    for (int i = 0; i < dtItem.Rows.Count; i++)
                    {
                        Amount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["InvoiceNo"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 8));
                        tblDetail.AddCell(pdf.setCell(Convert.ToDecimal(dtItem.Rows[i]["Amount"]).ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    }
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                    tblDetail.AddCell(pdf.setCell((lstin[0].RecPay == "Payable") ? lstin[0].CustomerName : lstin[0].AccountName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(lstin[0].VoucherAmount.ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

                for (int i = 0; i <= (ProdDetail_Lines - dtItem.Rows.Count); i++)
                {
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                }
                tblDetail.AddCell(pdf.setCell("Through :", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));


                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                tblDetail.AddCell(pdf.setCell((lstin[0].RecPay == "Payable") ? lstin[0].AccountName : lstin[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));

                tblDetail.AddCell(pdf.setCell("Amount (in Words) :", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                tblDetail.AddCell(pdf.setCell("INR "+BAL.CommonMgmt.ConvertNumbertoWordsinDecimalNew(lstin[0].VoucherAmount), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                tblDetail.AddCell(pdf.setCell(lstin[0].VoucherAmount.ToString("##,##0.00"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 7));

                int[] Column_tblFooter = { 50, 50 };
                tblFooter.SetWidths(Column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.SplitLate = false;

                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                tblFooter.AddCell(pdf.setCell("Receiver's Signature", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.OrgName,4), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));


            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstin[0].VoucherNo.Replace("/", "-").ToString() + ".pdf";
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
            //pdfDoc.Add(tblDetail);
            PdfPTable tblOuterDetail = new PdfPTable(1);
            int[] column_tblNestedOuter = { 100 };
            tblOuterDetail.SetWidths(column_tblNestedOuter);
            tblOuterDetail.SpacingBefore = 0f;
            tblOuterDetail.LockedWidth = true;
            tblOuterDetail.SplitLate = false;
            tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            tblOuterDetail.AddCell(pdf.setCell(tblDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
            tblOuterDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblOuterDetail);

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


    }
}
