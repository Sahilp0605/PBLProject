using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProductHexagon : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        hdnpkID.Value = "0";
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

        public void OnlyViewControls()
        {
            txtProductName.ReadOnly = true;
            chkActive.Enabled = false;
            txtProductAlias.ReadOnly = true;
            txtBrandName.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                // ----------------------------------------------------
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProductName.Text = lstEntity[0].ProductName;
                txtProductAlias.Text = lstEntity[0].ProductAlias;
                txtBrandName.Text = lstEntity[0].BrandName;
                chkActive.Checked = lstEntity[0].ActiveFlag;

                txtProductName.Focus();
            }
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtProductName.Text = "";
            txtProductAlias.Text = "";
            txtBrandName.Text = "";
            chkActive.Checked = true;

            btnSave.Disabled = false;
            txtProductName.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            String strErr = "";
            int ReturnCode = 0;
            string ReturnMsg = "";
            long ReturnProductId = 0;

            if (String.IsNullOrEmpty(txtProductName.Text) || String.IsNullOrEmpty(txtProductAlias.Text) || String.IsNullOrEmpty(txtBrandName.Text))
                
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtProductName.Text))
                    strErr += "<li>" + "Product Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtProductAlias.Text))
                    strErr += "<li>" + "Product Alias is required." + "</li>";

                if (String.IsNullOrEmpty(txtBrandName.Text))
                    strErr += "<li>" + "Brand Name is required." + "</li>";
            }
            //---------------------------------------------------------------------------------------
            if(_pageValid == true)
            {
                Entity.Products objEntity = new Entity.Products();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.ProductName = txtProductName.Text;
                objEntity.ProductAlias = txtProductAlias.Text;
                objEntity.BrandName = txtBrandName.Text;
                objEntity.ActiveFlag = chkActive.Checked;

                // -------------------------------------------------------------- Insert/Update Record
                BAL.ProductMgmt.AddUpdateProduct(objEntity, out ReturnCode, out ReturnMsg, out ReturnProductId);
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProduct(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProductMgmt.DeleteProduct(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);
            return serializer.Serialize(rows);
        }
        
    }
}