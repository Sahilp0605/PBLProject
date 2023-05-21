using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class BankDetails : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                //BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    hdnCompanyID.Value = Session["CompanyID"].ToString().ToString();
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
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.OrganizationBankInfo> lstEntity = new List<Entity.OrganizationBankInfo>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.CommonMgmt.GetBankInfo(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtBankName.Text = lstEntity[0].BankName;
                txtAccountName.Text = lstEntity[0].BankAccountName;
                txtBranchName.Text = lstEntity[0].BranchName;
                txtBankAccountNo.Text = lstEntity[0].BankAccountNo;
                txtBankIFSC.Text = lstEntity[0].BankIFSC;
                txtBankSWIFT.Text = lstEntity[0].BankSWIFT;
                txtBankName.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtBankName.Text) || String.IsNullOrEmpty(txtBankAccountNo.Text) || String.IsNullOrEmpty(txtBranchName.Text) || String.IsNullOrEmpty(txtBankIFSC.Text) || String.IsNullOrEmpty(txtBankSWIFT.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtBankName.Text))
                {
                    strErr += "<li>" + "Bank Name is mandatory !" + "</li>";
                }
                if (String.IsNullOrEmpty(txtAccountName.Text))
                {
                    strErr += "<li>" + "Account Name is mandatory !" + "</li>";
                }
                if (String.IsNullOrEmpty(txtBankAccountNo.Text))
                    strErr += "<li>" + "Bank Account is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtBranchName.Text))
                    strErr += "<li>" + "Branch Name is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtBankIFSC.Text))
                    strErr += "<li>" + "Bank IFSC Code is mandatory !" + "</li>";

                if (String.IsNullOrEmpty(txtBankSWIFT.Text))
                    strErr += "<li>" + "SWIFT Code is mandatory !" + "</li>";

            }
            

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.OrganizationBankInfo objEntity = new Entity.OrganizationBankInfo();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.CompanyID = Convert.ToInt64(hdnCompanyID.Value);
                objEntity.BankName = txtBankName.Text;
                objEntity.BankAccountName = txtAccountName.Text;
                objEntity.BankAccountNo = txtBankAccountNo.Text;
                objEntity.BranchName = txtBranchName.Text;
                objEntity.BankIFSC = txtBankIFSC.Text;
                objEntity.BankSWIFT = txtBankSWIFT.Text;


                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CommonMgmt.AddUpdateBankInfo(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                }
            }
            if (ReturnCode > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        }

        public void OnlyViewControls()
        {
            txtBankName.ReadOnly = true;
            txtAccountName.ReadOnly = true;
            txtBankAccountNo.ReadOnly = true;
            txtBranchName.ReadOnly = true;
            txtBankIFSC.ReadOnly = true;
            txtBankSWIFT.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void ClearAllField()
        {
            txtBankName.Text = "";
            txtAccountName.Text = "";
            txtBranchName.Text = "";
            txtBankAccountNo.Text = "";
            txtBankIFSC.Text = "";
            txtBankSWIFT.Text = "";

            txtBankName.Focus();
            btnSave.Disabled = false;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteBankDetails(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CommonMgmt.DeleteBankDetails(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}