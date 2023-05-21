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
    public partial class PettyCash : System.Web.UI.Page
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
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.ReadOnly = true;
            txtDBCustomerName.ReadOnly = true;
            txtCRCustomerName.ReadOnly = true;
            txtVoucherAmount.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
                // -----------------------------------------------------------------------------------

                lstEntity = BAL.FinancialTransMgmt.GetPettyCashList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();

                txtVoucherNo.Text = lstEntity[0].VoucherNo.ToString();
                txtVoucherDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].VoucherDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnDBCustomerID.Value = lstEntity[0].DBCustomerID.ToString();
                txtDBCustomerName.Text = lstEntity[0].DBCustomerName.ToString();
                hdnCRCustomerID.Value = lstEntity[0].CRCustomerID.ToString();
                txtCRCustomerName.Text = lstEntity[0].CRCustomerName.ToString();
                txtVoucherAmount.Text = lstEntity[0].VoucherAmount.ToString();
                txtRemarks.Text = lstEntity[0].Remarks.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "", ReturnVoucherNo = "";
            string strErr = "";
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtVoucherDate.Text) || String.IsNullOrEmpty(txtVoucherAmount.Text) ? 0 : Convert.ToDecimal(txtVoucherAmount.Text)) == 0 ||
                String.IsNullOrEmpty(hdnDBCustomerID.Value) || String.IsNullOrEmpty(hdnCRCustomerID.Value))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtVoucherDate.Text))
                    strErr += "<li>" + "Voucher Date is required. " + "</li>";

                if (String.IsNullOrEmpty(hdnDBCustomerID.Value))
                    strErr += "<li>" + "Please Select Proper Debit Account " + "</li>";

                if (String.IsNullOrEmpty(hdnCRCustomerID.Value))
                    strErr += "<li>" + "Please Select Proper Credit Account " + "</li>";

                if ((String.IsNullOrEmpty(txtVoucherAmount.Text) ? 0 : Convert.ToDecimal(txtVoucherAmount.Text)) == 0)
                    strErr += "<li>" + "Voucher amount must be greater than zero" + "</li>";

                if (String.IsNullOrEmpty(txtRemarks.Text))
                    strErr += "<li>" + "Transaction Notes is required. " + "</li>";
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
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.VoucherNo = (!String.IsNullOrEmpty(txtVoucherNo.Text)) ? txtVoucherNo.Text : "";
                objEntity.VoucherDate = Convert.ToDateTime(txtVoucherDate.Text);
                objEntity.DBCustomerID = (!String.IsNullOrEmpty(hdnDBCustomerID.Value)) ? Convert.ToInt64(hdnDBCustomerID.Value) : 0;
                objEntity.CRCustomerID = (!String.IsNullOrEmpty(hdnCRCustomerID.Value)) ? Convert.ToInt64(hdnCRCustomerID.Value) : 0;
                objEntity.VoucherAmount = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;
                objEntity.Remarks = (!String.IsNullOrEmpty(txtRemarks.Text)) ? txtRemarks.Text : "";
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.FinancialTransMgmt.AddUpdatePettyCash(objEntity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
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
            hdnDBCustomerID.Value = "";
            hdnCRCustomerID.Value = "";
            txtVoucherNo.Text = "";
            txtVoucherDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDBCustomerName.Text = "";
            txtCRCustomerName.Text = "";
            txtVoucherAmount.Text = "";
            txtRemarks.Text = "";
            btnSave.Disabled = false;
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string DeletePettyCash(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FinancialTransMgmt.DeletePettyCash(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}