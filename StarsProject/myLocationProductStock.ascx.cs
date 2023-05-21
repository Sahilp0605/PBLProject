using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myLocationProductStock : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public string pageLocation
        {
            get { return hdnLocation.Value; }
            set { hdnLocation.Value = value; }
        }

        public string pageProduct
        {
            get { return hdnProduct.Value; }
            set { hdnProduct.Value = value; }
        }

        public void BindLocationProductList()
        {
            Int64 pLocation, pProduct;
            pLocation = (!String.IsNullOrEmpty(hdnLocation.Value)) ? Convert.ToInt64(hdnLocation.Value) : 0;
            pProduct = (!String.IsNullOrEmpty(hdnProduct.Value)) ? Convert.ToInt64(hdnProduct.Value) : 0;
            // ------------------------------------------------------------------
            List<Entity.Location> lstObject = new List<Entity.Location>();
            if (hdnView.Value.ToLower() == "summary")
            {
                rptLocationDetail.Visible = false;
                rptLocationSummary.Visible = true;
                lstObject = BAL.ReportMgmt.LocationProductStockList(hdnView.Value, pLocation, pProduct, Session["LoginUserID"].ToString());
                rptLocationSummary.DataSource = lstObject;
                rptLocationSummary.DataBind();
            }
            else if (hdnView.Value.ToLower() == "detail")
            {
                rptLocationSummary.Visible = false;
                rptLocationDetail.Visible = true;
                lstObject = BAL.ReportMgmt.LocationProductStockList(hdnView.Value, pLocation, pProduct, Session["LoginUserID"].ToString());
                rptLocationDetail.DataSource = lstObject;
                rptLocationDetail.DataBind();
            }
        }

        protected void rptMaterialStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Header)
            //{
            //    if (hdnViewType.Value.ToLower() == "summary")
            //    {
            //        HtmlTableCell headOrd1 = ((HtmlTableCell)e.Item.FindControl("headOrd1"));
            //        HtmlTableCell headOrd2 = ((HtmlTableCell)e.Item.FindControl("headOrd2"));
            //        if (headOrd1 != null)
            //            headOrd1.Visible = false;
            //        if (headOrd2 != null)
            //            headOrd2.Visible = false;

            //        HtmlTableCell headINOUT = ((HtmlTableCell)e.Item.FindControl("headINOUT"));
            //        headINOUT.InnerText = (hdnView.Value.ToLower() == "purchase") ? "Recd.Qty" : "Disp.Qty";

            //    }
            //}
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    if (hdnViewType.Value.ToLower() == "summary")
            //    {
            //        HtmlTableCell itemOrd1 = ((HtmlTableCell)e.Item.FindControl("itemOrd1"));
            //        HtmlTableCell itemOrd2 = ((HtmlTableCell)e.Item.FindControl("itemOrd2"));
            //        if (itemOrd1 != null)
            //            itemOrd1.Visible = false;
            //        if (itemOrd2 != null)
            //            itemOrd2.Visible = false;
            //    }
            //    HiddenField hdnStatus = ((HiddenField)e.Item.FindControl("hdnStatus"));
            //    Label td = ((Label)e.Item.FindControl("tdStatus"));
            //    if (hdnStatus.Value == "Pending")
            //        td.Attributes.Add("class", "pill red white-text padding-2");
            //    else if (hdnStatus.Value == "Access")
            //        td.Attributes.Add("class", "pill green white-text padding-2");
            //    else
            //        td.Attributes.Add("class", "pill blue black-text padding-2");
            //    // ------------------------------------------------------------
            //    //HtmlGenericControl dv1 = ((HtmlGenericControl)e.Item.FindControl("divFollInqNo"));
            //    //HiddenField hdn2 = ((HiddenField)e.Item.FindControl("hdnFollInqNo"));
            //    //dv1.Visible = (!String.IsNullOrEmpty(hdn2.Value)) ? true : false;

            //}
        }
    }
}