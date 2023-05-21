using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myComplaint : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }
        public string openTickets
        {
            get { return hdnOpenTicket.Value; }
            set { hdnOpenTicket.Value = value; }
        }
        public string closeTickets
        {
            get { return hdnCloseTicket.Value; }
            set { hdnCloseTicket.Value = value; }
        }

        public void BindComplaintList(string loginuserid, string ComplaintStatus, Int64 pMon, Int64 pYear)
        {
            int TotRec;
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0, 0, ComplaintStatus, pMon, pYear, Session["LoginUserID"].ToString(), "", 1, 10000, out TotRec);
            rptComplaint.DataSource = lstEntity;
            rptComplaint.DataBind();
            
        }
        public void BindComplaintSummary(string loginuserid, Int64 pMon, Int64 pYear)
        {
            int TotRec;
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(0, 0, "", pMon, pYear, Session["LoginUserID"].ToString(), "", 1, 10000, out TotRec);
            hdnOpenTicket.Value = lstEntity.Where(it => (it.ComplaintStatus.ToLower() == "open")).Count().ToString();
            hdnCloseTicket.Value = lstEntity.Where(it => (it.ComplaintStatus.ToLower() == "close")).Count().ToString();
        }

        protected void rptComplaint_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnComplaintDays = ((HiddenField)e.Item.FindControl("hdnComplaintDays"));
                HtmlGenericControl lblComplaintDays = ((HtmlGenericControl)e.Item.FindControl("lblComplaintDays"));
                if (String.IsNullOrEmpty(hdnComplaintDays.Value))
                {
                    lblComplaintDays.InnerText = "Under AMC";

                    lblComplaintDays.Style.Add("border-radius", "8px");
                    lblComplaintDays.Style.Add("background-color", "silver");
                    lblComplaintDays.Style.Add("color", "navy");
                    lblComplaintDays.Style.Add("padding", "3px");
                }
                else
                {
                    lblComplaintDays.InnerText = (Convert.ToInt64(hdnComplaintDays.Value) <= 0) ? hdnComplaintDays.Value + " Days Left" : hdnComplaintDays.Value + " Days OverDue";
                    if (Convert.ToInt64(hdnComplaintDays.Value) <= 0)
                    {
                        lblComplaintDays.Style.Add("border-radius", "8px");
                        lblComplaintDays.Style.Add("background-color", "green");
                        lblComplaintDays.Style.Add("color", "white");
                        lblComplaintDays.Style.Add("padding", "3px");
                    }
                    else
                    {
                        lblComplaintDays.Style.Add("border-radius", "8px");
                        lblComplaintDays.Style.Add("background-color", "red");
                        lblComplaintDays.Style.Add("color", "white");
                        lblComplaintDays.Style.Add("padding", "3px");
                    }
                }
            }
        }
        public Int64 ComplaintCount
        {
            get
            {
                return Convert.ToInt64(rptComplaint.Items.Count);
            }
        }
    }
}