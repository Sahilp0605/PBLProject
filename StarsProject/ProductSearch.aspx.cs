using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProductSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
                myProductLedger.LocationFlag = hdnLocationStock.Value;
            }
        }
        public void BindDropDown()
        {
            //---------------------------Location Details-------------------------------
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            lstLocation = BAL.CommonMgmt.GetLocationList();
            drpLocation.DataSource = lstLocation;
            drpLocation.DataValueField = "LocationID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }
        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            if (!String.IsNullOrEmpty(hdnProductID.Value) && hdnProductID.Value != "0")
            {
                List<Entity.Products> lstProduct = new List<Entity.Products>();
                lstProduct = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);
                if (lstProduct.Count > 0)
                {

                    lblProductAlias.Text = lstProduct[0].ProductAlias;
                    lblBrandName.Text = lstProduct[0].BrandName;
                    lblCategoryName.Text = lstProduct[0].ProductGroupName;
                    lblProductType.Text = lstProduct[0].ProductType;

                    lblUnit.Text = lstProduct[0].Unit;
                    lblHSNCode.Text = lstProduct[0].HSNCode;
                    lblUnitPrice.Text = lstProduct[0].UnitPrice.ToString();
                    lblGST.Text = lstProduct[0].TaxRate.ToString("0.00") + " %   " + ((lstProduct[0].TaxType == 0) ? "Inclusive" : "Exclusive");

                    lblMinStock.Text = lstProduct[0].MinQuantity.ToString();
                    lblMinUnitPrice.Text = lstProduct[0].Min_UnitPrice.ToString();
                    lblMaxUnitPrice.Text = lstProduct[0].Max_UnitPrice.ToString();

                    lblOpeningSTK.Text = lstProduct[0].OpeningSTK.ToString();
                    lblInward.Text = lstProduct[0].InwardSTK.ToString();
                    lblOutward.Text = lstProduct[0].OutwardSTK.ToString();
                    lblClosingSTK.Text = lstProduct[0].ClosingSTK.ToString();
                    // --------------------------------------------------------------
                    hdnClosing.Value = lstProduct[0].OpeningSTK.ToString();
                    // --------------------------------------------------------------
                    BindStockLedger();
                }
            }
        }

        // --------------------------------------------------------------
        // Product Stock Ledger
        // --------------------------------------------------------------
        public void BindStockLedger()
        {
            myProductLedger.ProductID = hdnProductID.Value;
            myProductLedger.OpeningBalance = hdnClosing.Value;
            myProductLedger.LocationFlag = hdnLocationStock.Value;
            myProductLedger.LocationID = drpLocation.SelectedValue;
            myProductLedger.BindInwardOutwardLedger();
        }
        protected void drpLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStockLedger();
        }
        // --------------------------------------------------------------
        // Production FLOOR : Stock Movement
        // --------------------------------------------------------------

    }
}
