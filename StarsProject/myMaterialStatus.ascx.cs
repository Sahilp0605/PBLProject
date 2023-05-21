using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myMaterialStatus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public string pageViewType
        {
            get { return hdnViewType.Value; }
            set { hdnViewType.Value = value; }
        }
        public string pageStatus
        {
            get { return hdnStatus.Value; }
            set { hdnStatus.Value = value; }
        }

        public string pageMonth
        {
            get { return hdnMonth.Value; }
            set { hdnMonth.Value = value; }
        }

        public string pageYear
        {
            get { return hdnYear.Value; }
            set { hdnYear.Value = value; }
        }

        public void BindMaterialStatus()
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            // ------------------------------------------------------------------
            List<Entity.DispatchStatus> lstObject = new List<Entity.DispatchStatus>();
            if (hdnView.Value.ToLower() == "purchase")
                lstObject = BAL.ReportMgmt.SupplierMaterialStatusList(hdnViewType.Value, pMon, pYear, Session["LoginUserID"].ToString());
            else if (hdnView.Value.ToLower() == "sales")
                lstObject = BAL.ReportMgmt.DispatchStatusList(hdnViewType.Value, pMon, pYear, Session["LoginUserID"].ToString());
            else
                lstObject = null;
            // --------------------------------------------------------
            if (!String.IsNullOrEmpty(pageStatus))
                lstObject = lstObject.Where(it => (it.RequestStatus.ToLower() == pageStatus.ToLower())).ToList();
            // --------------------------------------------------------
            rptMaterialStatus.DataSource = lstObject;
            rptMaterialStatus.DataBind();
        }

        protected void rptMaterialStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (hdnViewType.Value.ToLower() == "summary")
                {
                    HtmlTableCell headOrd1 = ((HtmlTableCell)e.Item.FindControl("headOrd1"));
                    HtmlTableCell headfpdf = ((HtmlTableCell)e.Item.FindControl("headfpdf"));
                    HtmlTableCell headOrd2 = ((HtmlTableCell)e.Item.FindControl("headOrd2"));
                    if (headOrd1 != null)
                    {
                        headOrd1.Visible = false;
                        headfpdf.Visible = false;
                    }
                    if (headOrd2 != null)
                        headOrd2.Visible = false;

                    HtmlTableCell headINOUT = ((HtmlTableCell)e.Item.FindControl("headINOUT"));
                    headINOUT.InnerText = (hdnView.Value.ToLower() == "purchase") ? "Recd.Qty" : "Disp.Qty";

                }
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnRequestStatus = ((HiddenField)e.Item.FindControl("hdnRequestStatus"));
                HtmlTableCell itempdf = ((HtmlTableCell)e.Item.FindControl("itempdf"));
                if (hdnViewType.Value.ToLower() == "summary")
                {
                    HtmlTableCell itemOrd1 = ((HtmlTableCell)e.Item.FindControl("itemOrd1"));
                    HtmlTableCell itemOrd2 = ((HtmlTableCell)e.Item.FindControl("itemOrd2"));
                    if (itemOrd1 != null)
                    {
                        itemOrd1.Visible = false;
                        itempdf.Visible = false;
                    }
                    if (itemOrd2 != null)
                        itemOrd2.Visible = false;

                }
                // -----------------------------------------------------------------------------------
                if (hdnViewType.Value.ToLower() == "detail")
                {

                    HiddenField hdnApprovalStatus = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                    HtmlTableCell tdApproval = ((HtmlTableCell)e.Item.FindControl("tdApproval"));
                    if (hdnApprovalStatus.Value.ToLower() == "pending")
                    {
                        tdApproval.InnerHtml = "<i class='material-icons red-text' style='font-size: 20px; padding: 5px;'>do_not_disturb_alt</i>";
                    }
                    else
                    {
                        tdApproval.InnerHtml = "<i class='material-icons green-text' style='font-size: 20px; padding: 5px;'>check</i>";
                    }
                }
                // -----------------------------------------------------------------------------------
                
                Label td = ((Label)e.Item.FindControl("tdStatus"));
                if (hdnRequestStatus.Value == "Pending")
                    td.Attributes.Add("class", "pill red white-text padding-2");
                else if (hdnRequestStatus.Value == "Access")
                    td.Attributes.Add("class", "pill green white-text padding-2");
                else
                    td.Attributes.Add("class", "pill blue black-text padding-2");

            }
        }
    }
}