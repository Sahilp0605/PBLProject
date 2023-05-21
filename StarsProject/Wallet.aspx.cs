using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Wallet : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnWalletID.Value = Request.QueryString["id"].ToString();

                    if (hdnWalletID.Value == "0" || hdnWalletID.Value == "")
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
            txtWalletName.Text = "";
            
        }
        public void OnlyViewControls()
        {
            txtWalletName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
          
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Wallet> lstEntity = new List<Entity.Wallet>();

                lstEntity = BAL.WalletMgmt.GetWallet(Convert.ToInt64(hdnWalletID.Value));
                hdnWalletID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                txtWalletName.Text = (!String.IsNullOrEmpty(lstEntity[0].WalletName)) ? lstEntity[0].WalletName.Trim() : "";

                txtWalletName.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            divErrorMessage.InnerHtml = "";

            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.Wallet objEntity = new Entity.Wallet();

                if (!String.IsNullOrEmpty(hdnWalletID.Value))
                    objEntity.pkID  = Convert.ToInt64(hdnWalletID.Value);

                objEntity.WalletName = (!String.IsNullOrEmpty(txtWalletName.Text)) ? txtWalletName.Text.Trim() : "";
                
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.WalletMgmt.AddUpdateWallet(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                divErrorMessage.InnerHtml = ReturnMsg;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteWallet(string WalletID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.WalletMgmt.DeleteWallet(Convert.ToInt64(WalletID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}