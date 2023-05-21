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
    public partial class Designations : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        

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
                    hdnDesigCode.Value = Request.QueryString["id"].ToString();

                    if (hdnDesigCode.Value == "0" || hdnDesigCode.Value == "")
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
            chkActive.Enabled = true;
            txtDesignation.ReadOnly = true;
            txtDesigCode.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;

        }
        public void ClearAllField()
        {
            Session["OldUserID"] = "";
            txtDesigCode.Text = "";
            txtDesignation.Text = "";
            chkActive.Checked = true;
            txtDesigCode.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int ReturnCode = 0;
            string ReturnMsg = "";

            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtDesigCode.Text) || String.IsNullOrEmpty(txtDesignation.Text))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtDesigCode.Text))
                    _pageErrMsg = "<li>Designation Code is required.</li>";

                if (String.IsNullOrEmpty(txtDesignation.Text))
                    _pageErrMsg += "<li>Designation Name is required.</li>";
                    
            }
            // -----------------------------------------------------------------
            Entity.Designations objEntity = new Entity.Designations();
            if (_pageValid)
            {
                objEntity.DesigCode = txtDesigCode.Text.Trim();
                objEntity.Designation = txtDesignation.Text.Trim();
                objEntity.ActiveFlag = chkActive.Checked;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.DesignationMgmt.AddUpdateDesignation(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.Designations> lstEntity = new List<Entity.Designations>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.DesignationMgmt.GetDesignation(hdnDesigCode.Value, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                txtDesigCode.Text = lstEntity[0].DesigCode;
                txtDesignation.Text = lstEntity[0].Designation;
                chkActive.Checked = lstEntity[0].ActiveFlag;
                txtDesigCode.Enabled = false;
                txtDesigCode.CssClass = "form-control";
            }
            else
            {
                txtDesigCode.Enabled = true;
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteDesignation(string DesigCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.DesignationMgmt.DeleteDesignation(DesigCode, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
 
    }
}