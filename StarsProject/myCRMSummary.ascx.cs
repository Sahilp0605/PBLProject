using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myCRMSummary : System.Web.UI.UserControl
    {
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

        public string pageUserID
        {
            get { return hdnLoginUserID.Value; }
            set { hdnLoginUserID.Value = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnLoginUserID.Value = Session["LoginUserID"].ToString();
        }

        public void BindCRMSummaryCount()
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            List<Entity.CRMSummary> lstEntity = new List<Entity.CRMSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardCRMSummary("", pMon, pYear, Session["LoginUserID"].ToString());

            spnLeads.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalLeads.ToString() : "0";
            spnQuotation.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalQuoatation.ToString() : "0";
            spnQuotationAmt.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalQuoatationAmt.ToString() : "0";

            spnSalesOrder.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalSalesOrder.ToString() : "0";
            spnSalesOrderAmt.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalSalesOrderAmt.ToString() : "0";

            spnSalesBill.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalSalesBill.ToString() : "0";
            spnSalesBillAmt.InnerText = (lstEntity.Count > 0) ? lstEntity[0].TotalSalesBillAmt.ToString() : "0";
        }
    }
}