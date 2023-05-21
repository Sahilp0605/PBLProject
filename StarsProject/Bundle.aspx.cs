using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Bundle : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnBundleID.Value = Request.QueryString["id"].ToString();

                    if (hdnBundleID.Value == "0" || hdnBundleID.Value == "")
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
            txtBundleName.Text = "";
            
            
        }
        public void OnlyViewControls()
        {
            txtBundleName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
          
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Bundle> lstEntity = new List<Entity.Bundle>();

                lstEntity = BAL.BundleMgmt.GetBundle(Convert.ToInt64(hdnBundleID.Value));
                hdnBundleID.Value = (!String.IsNullOrEmpty(lstEntity[0].BundleId.ToString())) ? lstEntity[0].BundleId.ToString() : "";
                txtBundleName.Text = (!String.IsNullOrEmpty(lstEntity[0].BundleName)) ? lstEntity[0].BundleName.Trim() : "";

                txtBundleName.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            _pageErrMsg = "";

            if (String.IsNullOrEmpty(txtBundleName.Text))
            {
                _pageErrMsg += "<li>" + "Bundle Name is required." + "</li>";
                _pageValid = false;


            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.Bundle objEntity = new Entity.Bundle();

                if (!String.IsNullOrEmpty(hdnBundleID.Value))
                    objEntity.BundleId  = Convert.ToInt64(hdnBundleID.Value);

                objEntity.BundleName = (!String.IsNullOrEmpty(txtBundleName.Text)) ? txtBundleName.Text.Trim() : "";
                
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.BundleMgmt.AddUpdateBundle(objEntity, out ReturnCode, out ReturnMsg);
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
        public static string DeleteBundle(string BundleID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.BundleMgmt.DeleteBundle(Convert.ToInt64(BundleID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}