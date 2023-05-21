using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myProductLedger : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        public Decimal tmpVal1, tmpVal2;
        public string ProjectID
        {
            get { return hdn_ProjectID.Value; }
            set { hdn_ProjectID.Value = value; }
        }
        public string ProductID
        {
            get { return hdn_ProductID.Value; }
            set { hdn_ProductID.Value = value; }
        }
        public string LocationFlag
        {
            get { return hdn_LocationFlag.Value; }
            set { hdn_LocationFlag.Value = value; }
        }

        public string LocationID
        {
            get { return hdn_LocationID.Value; }
            set { hdn_LocationID.Value = value; }
        }
        public string OpeningBalance
        {
            get { return hdn_Closing.Value; }
            set
            {
                hdn_Opening.Value = value;
                hdn_Closing.Value = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        public void BindInwardOutwardLedger()
        {
            if (!String.IsNullOrEmpty(hdn_ProductID.Value) && hdn_ProductID.Value != "0")
            {
                hdn_ProjectID.Value = (!String.IsNullOrEmpty(hdn_ProjectID.Value)) ? Convert.ToInt64(hdn_ProjectID.Value).ToString() : "0";
                // -----------------------------------------------------------------------------------
                List<Entity.InwardOutwardLedger> lstEntity1 = new List<Entity.InwardOutwardLedger>();
                if (hdn_LocationFlag.Value == "yes")
                {
                    Int64 tmpVal = (!String.IsNullOrEmpty(hdn_LocationID.Value)) ? Convert.ToInt64(hdn_LocationID.Value) : 0;
                    lstEntity1 = BAL.ProductMgmt.GetInwardOutwardLedger(Convert.ToInt64(hdn_ProjectID.Value), tmpVal, Convert.ToInt64(hdn_ProductID.Value), "inout");
                }
                else
                    lstEntity1 = BAL.ProductMgmt.GetInwardOutwardLedger(Convert.ToInt64(hdn_ProjectID.Value), 0, Convert.ToInt64(hdn_ProductID.Value), "inout");

                rptProductLedger.DataSource = lstEntity1;
                rptProductLedger.DataBind();
            }
        }
        protected void rptProductLedger_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlTableCell tdHeaderInward = (HtmlTableCell)e.Item.FindControl("tdHeaderInward");
                HtmlTableCell tdHeaderOutward = (HtmlTableCell)e.Item.FindControl("tdHeaderOutward");
                HtmlTableCell tdHeaderClosing = (HtmlTableCell)e.Item.FindControl("tdHeaderClosing");

                if (Convert.ToDecimal(hdn_Closing.Value) >= 0)
                    tdHeaderInward.InnerText = hdn_Closing.Value;
                else if (Convert.ToDecimal(hdn_Closing.Value) < 0)
                    tdHeaderOutward.InnerText = hdn_Closing.Value;

                tdHeaderClosing.InnerText = hdn_Closing.Value;
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnTransType = ((HiddenField)e.Item.FindControl("hdnTransType"));
                HiddenField hdnQuantity = ((HiddenField)e.Item.FindControl("hdnQuantity"));
                //HtmlTableCell tdOpening = (HtmlTableCell)e.Item.FindControl("tdOpening");
                HtmlTableCell tdClosing = (HtmlTableCell)e.Item.FindControl("tdClosing");

                HtmlTableCell tdInwardQty = (HtmlTableCell)e.Item.FindControl("tdInwardQty");
                HtmlTableCell tdOutwardQty = (HtmlTableCell)e.Item.FindControl("tdOutwardQty");
                // -------------------------------------------------------------------
                //tdOpening.InnerText = hdn_Closing.Value;
                decimal tmpVal1 = Convert.ToDecimal(hdnQuantity.Value);
                if (hdnTransType.Value.ToLower() == "in" || hdnTransType.Value.ToLower() == "inward")
                {
                    tdInwardQty.InnerText = hdnQuantity.Value;
                    hdn_Inward.Value = (Convert.ToDecimal(hdn_Inward.Value) + tmpVal1).ToString();
                    hdn_Closing.Value = (Convert.ToDecimal(hdn_Closing.Value) + tmpVal1).ToString();
                }
                else
                {
                    tdOutwardQty.InnerText = hdnQuantity.Value;
                    hdn_Outward.Value = (Convert.ToDecimal(hdn_Outward.Value) + tmpVal1).ToString();
                    hdn_Closing.Value = (Convert.ToDecimal(hdn_Closing.Value) - tmpVal1).ToString();
                }
                // -------------------------------------------------------------------

                tdClosing.InnerText = hdn_Closing.Value;

                if (Convert.ToDecimal(hdn_Closing.Value) > 0)
                {
                    tdClosing.Attributes.Add("style", "color: Red; text-align:right; font-size:14px;");
                }
                else if (Convert.ToDecimal(hdn_Closing.Value) < 0)
                {
                    tdClosing.Attributes.Add("style", "color: Navy; text-align:right; font-size:14px;");
                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                HtmlTableCell tdFooterInward = (HtmlTableCell)e.Item.FindControl("tdFooterInward");
                HtmlTableCell tdFooterOutward = (HtmlTableCell)e.Item.FindControl("tdFooterOutward");
                HtmlTableCell tdFooterClosing = (HtmlTableCell)e.Item.FindControl("tdFooterClosing");
                // ---------------------------------------------------------
                Decimal tmpInOpening = 0, tmpOutOpening = 0;
                tmpInOpening = (Convert.ToDecimal(hdn_Opening.Value) >= 0) ? Convert.ToDecimal(hdn_Opening.Value) : 0;
                tmpOutOpening = (Convert.ToDecimal(hdn_Opening.Value) >= 0) ? 0 : Convert.ToDecimal(hdn_Opening.Value);
                tdFooterInward.InnerText = (tmpInOpening + Convert.ToDecimal(hdn_Inward.Value)).ToString("0.00");
                tdFooterOutward.InnerText = (tmpOutOpening + Convert.ToDecimal(hdn_Outward.Value)).ToString("0.00");
                tdFooterClosing.InnerText = hdn_Closing.Value;
            }
        }

    }
}