using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace StarsProject
{
    public partial class myAllLeads : System.Web.UI.UserControl
    {
        #region Start Properties
        public Int64 pageeadCount
        {
            get
            {
                return Convert.ToInt64(rptAllLead.Items.Count);
            }
        }
        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }

        public string pageMonth
        {
            get { return ((!String.IsNullOrEmpty(hdnMonth.Value)) ? hdnMonth.Value : "0"); }
            set { hdnMonth.Value = value; }
        }

        public string pageYear
        {
            get { return ((!String.IsNullOrEmpty(hdnYear.Value)) ? hdnYear.Value : "0"); }
            set { hdnYear.Value = value; }
        }
        public string pageEmployeeID
        {
            get { return hdnSelEmpID.Value; }
            set { hdnSelEmpID.Value = value; }
        }
        public string pageUserID
        {
            get { return hdnSelUserID.Value; }
            set { hdnSelUserID.Value = value; }
        }

        public string pageLeadStatus
        {
            get { return hdnLeadStatus.Value; }
            set { hdnLeadStatus.Value = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindAssignLeadList();
            }
        }

        public void BindAssignLeadList()
        {
            int TotRec;
            Int64 pMon, pYear, pFromDays = 0, pToDays = 0;
            // -----------------------------------------------------------------
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            // -----------------------------------------------------------------
            pFromDays = (!String.IsNullOrEmpty(hdnFromDays.Value)) ? Convert.ToInt64(hdnFromDays.Value) : 0;
            pToDays = (!String.IsNullOrEmpty(hdnToDays.Value)) ? Convert.ToInt64(hdnToDays.Value) : 365;
            // -----------------------------------------------------------------
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            lstEntity = BAL.InquiryInfoMgmt.GetDashboardAllLeads(pageLeadStatus, pageUserID, pMon, pYear, pFromDays, pToDays);
            rptAllLead.DataSource = lstEntity;
            rptAllLead.DataBind();
            
        }
        protected void btnTemp_Click(object sender, EventArgs e)
        {
            BindAssignLeadList();
            //ScriptManager.RegisterStartupScript(this, typeof(string), "spkwrds", "javascript:initLeadBar();", true);
        }
        protected void rptAllLead_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //HiddenField hdn0 = ((HiddenField)e.Item.FindControl("hdnDueDate"));
                //HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnCompletionDate"));
                //HtmlGenericControl dv = ((HtmlGenericControl)e.Item.FindControl("ltrCompletion"));
                //DateTime dtdue = DateTime.Now;
                //DateTime dtcurr = DateTime.Now;

                //if (String.IsNullOrEmpty(hdn0.Value))
                //    dtdue = Convert.ToDateTime(hdn1.Value);

                //if (String.IsNullOrEmpty(hdn1.Value))
                //{
                //    dv.InnerText = (dtdue < dtcurr) ? "OverDue" : "Not Applied";
                //}
                //else
                //{
                //    DateTime dt = Convert.ToDateTime(hdn1.Value);
                //    if (dt.Year < 2000)
                //        dv.InnerText = (dtdue < dtcurr) ? "OverDue" : "Not Applied";
                //}
            }
        }

    }
}