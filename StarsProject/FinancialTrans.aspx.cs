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

namespace StarsProject
{
    public partial class FinancialTrans : System.Web.UI.Page
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
            drpTransMode.Items.Insert(0, new ListItem("-- Select TransactionMode --", ""));

            // ---------------- State List -------------------------------------
            List<Entity.State> lstEvents = new List<Entity.State>();
            lstEvents = BAL.StateMgmt.GetStateList();
            drpTerminationOfDelivery.DataSource = lstEvents;
            drpTerminationOfDelivery.DataValueField = "StateCode";
            drpTerminationOfDelivery.DataTextField = "StateName";
            drpTerminationOfDelivery.DataBind();
            drpTerminationOfDelivery.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All State --", ""));

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
                //txtVoucherDate.Text = lstEntity[0].VoucherDate.ToString("dd-MM-yyyy");
                txtVoucherDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].VoucherDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnAccountID.Value = lstEntity[0].AccountID.ToString();
                txtAccountName.Text = lstEntity[0].AccountName.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                rdblTransType.SelectedValue = lstEntity[0].TransType.ToString();
                drpTransMode.SelectedValue = lstEntity[0].TransModeID.ToString();
                txtTransID.Text = lstEntity[0].TransID.ToString();
                //txtTransDate.Text = lstEntity[0].TransDate.ToString("dd-MM-yyyy");
                txtTransDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].TransDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                txtVoucherAmount.Text = lstEntity[0].VoucherAmount.ToString();
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

                    drpRecPay_SelectedIndexChanged(null, null);
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            string strErr = "";
            _pageValid = true;

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
                objEntity.TransType = (!String.IsNullOrEmpty(rdblTransType.SelectedValue)) ? rdblTransType.SelectedValue : "";
                objEntity.TransModeID = (!String.IsNullOrEmpty(drpTransMode.SelectedValue)) ? Convert.ToInt64(drpTransMode.SelectedValue) : 0;
                objEntity.TransID = (!String.IsNullOrEmpty(txtTransID.Text)) ? txtTransID.Text : "";
                objEntity.TransDate = Convert.ToDateTime(txtTransDate.Text);
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
            hdnpkID.Value = "";
            hdnAccountID.Value = "";
            hdnCustomerID.Value = "";
            hdnCustStateID.Value = "";
            drpRecPay.SelectedValue = "Receivable";
            pnlPayment.Visible = false;

            drpVoucherType.SelectedValue = "";
            txtVoucherNo.Text = "";
            txtVoucherDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtAccountName.Text = "";
            txtCustomerName.Text = "";
            rdblTransType.SelectedValue = "";
            drpTransMode.SelectedValue = "";
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
            if (drpRecPay.SelectedValue == "Payable")
            {
                pnlPayment.Visible = true;
                txtCustomerName_TextChanged(null, null);
                funCalcTot();

            }
            else
            {
                pnlPayment.Visible = false;
            }
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
            BAL.CommonMgmt.funCalculate(taxtype, 1, basicval, 0, 0, taxper, 0, 0, hdnCustStateID.Value, Session["CompanyStateCode"].ToString(), out TaxAmt, out CGSTPer, out CGSTAmt, out SGSTPer, out SGSTAmt, out IGSTPer, out IGSTAmt, out NetRate, out  BasicAmt, out NetAmt, out ItmDiscPer1, out ItmDiscAmt1, out  AddTaxAmt);

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
    }
}
