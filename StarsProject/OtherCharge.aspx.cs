using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class OtherCharge : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnChargeID.Value = Request.QueryString["id"].ToString();

                    if (hdnChargeID.Value == "0" || hdnChargeID.Value == "")
                    {
                        ClearAllField();
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
        public void ClearAllField()
        {
            txtChargeName.Text = "";
            drpTaxType.SelectedValue = "2";
            txtGSTPer.Text = "";
            txtHSNCode.Text = "";
            chkBeforeGST.Checked = false;
        }
        public void OnlyViewControls()
        {
            txtChargeName.ReadOnly = true;
            drpTaxType.Attributes.Add("disabled", "disabled");
            txtGSTPer.ReadOnly = true;
            txtHSNCode.ReadOnly = true;
            chkBeforeGST.Enabled = false;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                List<Entity.OtherCharge> lstEntity = new List<Entity.OtherCharge>();

                lstEntity = BAL.OtherChargeMgmt.GetOtherChargeList(Convert.ToInt64(hdnChargeID.Value));
                hdnChargeID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                txtChargeName.Text = (!String.IsNullOrEmpty(lstEntity[0].ChargeName)) ? lstEntity[0].ChargeName.Trim() : "";
                drpTaxType.SelectedValue = (lstEntity[0].TaxType > 0) ? lstEntity[0].TaxType.ToString() : "";
                txtGSTPer.Text = (!String.IsNullOrEmpty(lstEntity[0].GST_Per.ToString())) ? lstEntity[0].GST_Per.ToString() : "";
                txtHSNCode.Text = (!String.IsNullOrEmpty(lstEntity[0].HSNCODE.ToString())) ? lstEntity[0].HSNCODE.ToString() : "";
                chkBeforeGST.Checked = (lstEntity[0].BeforeGST == true) ? chkBeforeGST.Checked = true : chkBeforeGST.Checked = false;
                txtChargeName.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            string ReturnMsg = "";

            if (String.IsNullOrEmpty(txtChargeName.Text) || String.IsNullOrEmpty(drpTaxType.SelectedValue))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtChargeName.Text))
                    _pageErrMsg += "<li>" + "Charge Name is required." + "</li>";

                if (drpTaxType.SelectedItem.Value == "2")
                    _pageErrMsg += "<li>" + "Tax Type Selection is required." + "</li>";

                if(drpTaxType.SelectedItem.Value != "2")
                    _pageErrMsg += "<li>" + "GST Charges is required." + "</li>";
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;

                // --------------------------------------------------------------
                Entity.OtherCharge objEntity = new Entity.OtherCharge();

                if (!String.IsNullOrEmpty(hdnChargeID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnChargeID.Value);

                objEntity.ChargeName = (!String.IsNullOrEmpty(txtChargeName.Text)) ? txtChargeName.Text.Trim() : "";
                objEntity.TaxType = (!String.IsNullOrEmpty(drpTaxType.SelectedValue)) ? Convert.ToInt32(drpTaxType.SelectedValue) : 0;
                objEntity.GST_Per = (!String.IsNullOrEmpty(txtGSTPer.Text)) ? Convert.ToDecimal(txtGSTPer.Text) : 0;
                objEntity.HSNCODE = (!String.IsNullOrEmpty(txtHSNCode.Text)) ? txtHSNCode.Text.Trim() : "";
                objEntity.BeforeGST = chkBeforeGST.Checked ? true : false;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.OtherChargeMgmt.AddUpdateOtherCharge(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteOtherCharge(string ChargeID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.OtherChargeMgmt.DeleteOtherCharge(Convert.ToInt64(ChargeID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}