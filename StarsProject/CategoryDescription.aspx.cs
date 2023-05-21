using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class CategoryDescription : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
          
            // ----------------------------------------------
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["OldUserID"] = "";

                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkid.Value = Request.QueryString["id"].ToString();

                    if (hdnpkid.Value == "0" || hdnpkid.Value == "")
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

            txtDescription.ReadOnly = true;
            drpCategory.Attributes.Add("disabled", "disabled");

            btnSave.Visible = false;
            btnReset.Visible = false;

        }
        public void ClearAllField()
        {
            hdnpkid.Value = "";
            txtDescription.Text = "";
            drpCategory.SelectedValue = "";
            txtDescription.Focus();
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
                List<Entity.CategoryDescription> lstEntity = new List<Entity.CategoryDescription>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.CategoryDescriptionMgmt.GetCategoryDescriptionList(Convert.ToInt64(hdnpkid.Value), "", Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnpkid.Value = lstEntity[0].pkID.ToString();
                txtDescription.Text = lstEntity[0].Description;
                drpCategory.SelectedValue = lstEntity[0].Category;
                txtDescription.Focus();
                // -------------------------------------------------------------------------
                //categoryStatusCaption();

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtDescription.Text) || String.IsNullOrEmpty(drpCategory.SelectedValue))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtDescription.Text))
                    _pageErrMsg = "<li>Inquiry Status is required.</li>";

            }
            // -----------------------------------------------------------------
            Entity.CategoryDescription objEntity = new Entity.CategoryDescription();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnpkid.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkid.Value);
                objEntity.Description = txtDescription.Text;
                objEntity.Category = drpCategory.SelectedValue;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.CategoryDescriptionMgmt.AddUpdateCategoryDescription(objEntity, out ReturnCode, out ReturnMsg);
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }
        [System.Web.Services.WebMethod]
        public static string DeleteCategoryDescription(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CategoryDescriptionMgmt.DeleteCategoryDescription(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}