using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SearchProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BindProduct();
                hdnClientERPIntegration.Value = BAL.CommonMgmt.GetConstant("ClientERPIntegration", 0, 1);
            }
        }

        public void BindProduct()
        {
            int TotalCount = 0;
            // ----------------------------------------------------
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                // ----------------------------------------------------
                lblProductAlias.InnerText = lstEntity[0].ProductAlias;
                lblProductGroup.InnerText = lstEntity[0].ProductGroupName;
                lblProductBrand.InnerText = lstEntity[0].BrandName;
                lblUnit.InnerText = lstEntity[0].Unit;
                lblUnitPrice.InnerText = lstEntity[0].UnitPrice.ToString("#.##");
                lblTaxRate.InnerText = lstEntity[0].TaxRate.ToString("#.## %");
                lblSpecification.InnerText = lstEntity[0].ProductSpecification;
                lblClosingStock.InnerText = lstEntity[0].ClosingSTK.ToString();
            }
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }
        
        // --------------------------------------------------------------------------
        // Section : Regenerate Stock
        // --------------------------------------------------------------------------
        public void ReGenStock()
        {
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.CommonMgmt.ReGenerateStock(false);
        }
        protected void btnSaveStock_Click(object sender, EventArgs e)
        {
            ReGenStock();
        }
    }
}