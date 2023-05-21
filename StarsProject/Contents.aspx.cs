using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Contents : System.Web.UI.Page
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
            
            drpCategory.Attributes.Add("disabled", "disabled");
            txtHeader.ReadOnly = true;
            txtContents.ReadOnly = true;
            
            btnSave.Visible = false;
            btnReset.Visible = false;

        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                List<Entity.Contents> lstEntity = new List<Entity.Contents>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.CommonMgmt.GetContentList(Convert.ToInt64(hdnpkid.Value), "");

                hdnpkid.Value = lstEntity[0].pkID.ToString();
                drpCategory.SelectedValue = lstEntity[0].Category;
                txtHeader.Text = lstEntity[0].TNC_Header;
                txtContents.Text = lstEntity[0].TNC_Content;
                drpCategory.Focus();
                // -------------------------------------------------------------------------
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            string strErr = "";
            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(drpCategory.SelectedValue) || String.IsNullOrEmpty(txtHeader.Text) || String.IsNullOrEmpty(txtContents.Text))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtHeader.Text))
                    strErr += "<li>" + "Header is required." + "</li>";

                if (String.IsNullOrEmpty(txtContents.Text))
                    strErr += "<li>" + "Contents is required." + "</li>";

            }
            // -----------------------------------------------------------------
            Entity.Contents objEntity = new Entity.Contents();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnpkid.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkid.Value);
                
                objEntity.Category = drpCategory.SelectedValue;
                objEntity.TNC_Header = txtHeader.Text;
                objEntity.TNC_Content = txtContents.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CommonMgmt.AddUpdateContents(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkid.Value = "";
            drpCategory.SelectedValue = "";
            txtHeader.Text = "";
            txtContents.Text = "";
            drpCategory.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteContents(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CommonMgmt.DeleteContents(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

    }
}