using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

namespace StarsProject
{
    public partial class myFollowup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)
            {
                // BindFollowupList(Session["LoginUserID"].ToString(), FollowupStatus);
            }
        }

        public void BindFollowupList(string loginuserid, String pFollowupStatus)
        {
            int TotRec;
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            FollowupStatus = pFollowupStatus;
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            lstEntity = BAL.FollowupMgmt.GetDashboardFollowupList(pFollowupStatus, pMon, pYear, loginuserid, 1, 100000, out TotRec);
            DataTable dtTemp = new DataTable();
            dtTemp = PageBase.ConvertListToDataTable(lstEntity);
            if(dtTemp != null)
            {
                if (dtTemp.Rows.Count>0)
                {
                    if (HttpContext.Current.Session["SerialKey"].ToString() == "ECO3-2G21-TECH-3MRT")
                    {
                        DataView dv = dtTemp.DefaultView;
                        dv.Sort = "pkID Desc";
                        dtTemp.AcceptChanges();
                    }
                }
                // -------------------------------------------------
                rptFollowup.DataSource = dtTemp;
                rptFollowup.DataBind();
            }
            
        }
        
       
        public Int64 FollowupCount
        {
            get
            {
                return Convert.ToInt64(rptFollowup.Items.Count);
            }
        }
        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
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

        public string FollowupStatus
        {
            get { return hdnFollowupStatus.Value; }
            set { hdnFollowupStatus.Value = value; }
        }

        protected void rptFollowup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdncustid = ((HiddenField)e.Item.FindControl("hdnCustID"));
                HtmlGenericControl liBase = ((HtmlGenericControl)e.Item.FindControl("liBase"));
                if (hdncustid.Value == "0")
                    liBase.Attributes.CssStyle.Add("background-color", "#e9f7ff");
                else
                    liBase.Attributes.CssStyle.Add("background-color", "beige");
                // ------------------------------------------------------------
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnNextFollowup"));
                HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("ltrNextFollowup"));
                if (String.IsNullOrEmpty(hdn1.Value))
                {
                    dv.InnerText = "Not Applied";
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(hdn1.Value);
                    if (dt.Year < 2000)
                        dv.InnerText = "Not Applied";
                }
                // ------------------------------------------------------------
            }
        }
    }
}