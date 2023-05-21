using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myFollowupTimeline : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string timelineCustomerID
        {
            get { return tlCustomerID.Value; }
            set { tlCustomerID.Value = value; }
        }

        public void BindFollowupList()
        {
            int TotRec;
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            if (!String.IsNullOrEmpty(tlCustomerID.Value))
            {
                lstEntity = BAL.FollowupMgmt.GetDashboardFollowupTimeline(Convert.ToInt64(tlCustomerID.Value), Session["LoginUserID"].ToString(), 1, 100000, out TotRec);
                rptTimeline.DataSource = lstEntity;
                rptTimeline.DataBind();
            }
        }

        public void BindFollowupExtList()
        {
            int TotRec;
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            if (!String.IsNullOrEmpty(tlCustomerID.Value))
            {
                lstEntity = BAL.FollowupMgmt.GetDashboardFollowupExtTimeline(Convert.ToInt64(tlCustomerID.Value), Session["LoginUserID"].ToString(), 1, 100000, out TotRec);
                rptTimeline.DataSource = lstEntity;
                rptTimeline.DataBind();
            }
        }

        protected void rptTimeline_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnInquiryNo"));
                //HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("divInquiryNo"));

                //dv.Visible = (!String.IsNullOrEmpty(tmpField.Value)) ? true: false;
            }
        }
    }
}