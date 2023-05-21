using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProductBrand : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 15;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

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
            txtBrandName.ReadOnly = true;
            txtBrandAlias.ReadOnly = true;
            chkActive.Enabled = false;

            btnSave.Visible = false;
            btnReset.Visible = false;

        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Brand> lstEntity = new List<Entity.Brand>();
                // ----------------------------------------------------
                lstEntity = BAL.BrandMgmt.GetBrandList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtBrandName.Text = lstEntity[0].BrandName;
                txtBrandAlias.Text = lstEntity[0].BrandAlias;
                chkActive.Checked = lstEntity[0].ActiveFlag;
                txtBrandName.Focus();
                // -------------------------------------------------------------------------
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
             _pageValid = true;
            divErrorMessage.InnerHtml = "";

            if (String.IsNullOrEmpty(txtBrandName.Text) || String.IsNullOrEmpty(txtBrandAlias.Text)) 
            {
                _pageValid = false;

                divErrorMessage.Style.Remove("color");
                divErrorMessage.Style.Add("color", "red");

                if (String.IsNullOrEmpty(txtBrandName.Text))
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Brand Name is required." + "</li>"));

                if (String.IsNullOrEmpty(txtBrandAlias.Text))
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Brand Alias is required." + "</li>"));

            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";

                Entity.Brand objEntity = new Entity.Brand();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.BrandName = txtBrandName.Text;
                objEntity.BrandAlias = txtBrandAlias.Text;
                objEntity.ActiveFlag = chkActive.Checked;


                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.BrandMgmt.AddUpdateBrand(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                divErrorMessage.InnerHtml = ReturnMsg;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtBrandName.Text = "";
            txtBrandAlias.Text = "";
            chkActive.Checked = true;
            txtBrandName.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProductBrand(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.BrandMgmt.DeleteBrand(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}