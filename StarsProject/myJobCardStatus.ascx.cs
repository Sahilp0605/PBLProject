using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myJobCardStatus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
        public void BindJobCardStatus()
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            // ------------------------------------------------------------------
            List<Entity.JobCardStatus> lstObject = new List<Entity.JobCardStatus>();
            lstObject = BAL.ReportMgmt.JobCardStatusList(hdnViewType.Value, pMon, pYear, Session["LoginUserID"].ToString());
            
            // --------------------------------------------------------
            if (!String.IsNullOrEmpty(pageStatus))
                lstObject = lstObject.Where(it => (it.Status == pageStatus)).ToList();
            // --------------------------------------------------------
            rptJobCardStatus.DataSource = lstObject;
            rptJobCardStatus.DataBind();
        }
        protected void rptJobCardStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (hdnViewType.Value.ToLower() == "summary")
                {
                    HtmlTableCell headOrd1 = ((HtmlTableCell)e.Item.FindControl("headOrd1"));
                    HtmlTableCell headOrd2 = ((HtmlTableCell)e.Item.FindControl("headOrd2"));
                    HtmlTableCell headOrd3 = ((HtmlTableCell)e.Item.FindControl("headOrd3"));
                    if (headOrd1 != null)
                        headOrd1.Visible = false;
                    if (headOrd2 != null)
                        headOrd2.Visible = false;
                    if (headOrd3 != null)
                        headOrd3.Visible = false;
                }
                
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (hdnViewType.Value.ToLower() == "summary")
                {
                    HtmlTableCell itemOrd1 = ((HtmlTableCell)e.Item.FindControl("itemOrd1"));
                    HtmlTableCell itemOrd2 = ((HtmlTableCell)e.Item.FindControl("itemOrd2"));
                    HtmlTableCell itemOrd3 = ((HtmlTableCell)e.Item.FindControl("itemOrd3"));
                    if (itemOrd1 != null)
                        itemOrd1.Visible = false;
                    if (itemOrd2 != null)
                        itemOrd2.Visible = false;
                    if (itemOrd3 != null)
                        itemOrd3.Visible = false;

                }

                HiddenField hdnStatus = ((HiddenField)e.Item.FindControl("hdnStatus"));
                Label td = ((Label)e.Item.FindControl("tdStatus"));
                if (hdnStatus.Value == "Pending")
                    td.Attributes.Add("class", "pill red white-text padding-2");
                else if (hdnStatus.Value == "Access")
                    td.Attributes.Add("class", "pill green white-text padding-2");
                else
                    td.Attributes.Add("class", "pill blue black-text padding-2");
                
            }
        }
    }
}