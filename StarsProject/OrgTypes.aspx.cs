using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

namespace StarsProject
{
    public partial class OrgTypes : System.Web.UI.Page
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
                Session["OldUserID"] = "";
                // ----------------------------------------
                ClearAllField();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        ClearAllField();
                    else
                    {
                        setLayout("Edit");
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
            txtOrgType.ReadOnly = true;
            chkActive.Enabled  = false;
           
            btnSave.Visible = false;
            btnReset.Visible = false;

        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.OrgTypeMgmt.GetOrgType(Convert.ToInt64(hdnpkID.Value), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtOrgType.Text = lstEntity[0].OrgType;
                chkActive.Checked = lstEntity[0].ActiveFlag;

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtOrgType.Text))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtOrgType.Text))
                    _pageErrMsg += "Organizaiton Type is required.<br>";

            }
            // -----------------------------------------------------------------
            
            if (_pageValid)
            {
                Entity.OrgTypes objEntity = new Entity.OrgTypes();
                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.OrgType = txtOrgType.Text.Trim();
                objEntity.ActiveFlag = chkActive.Checked;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------
                BAL.OrgTypeMgmt.AddUpdateOrgType(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtOrgType.Text = "";
            chkActive.Checked = true;
            txtOrgType.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteOrgType(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.OrgTypeMgmt.DeleteOrgType(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

    }
}