using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DashboardDailySummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSummaryCount();
            }
        }
        public void BindSummaryCount()
        {
            List<Entity.CRMSummary> lstEntity = new List<Entity.CRMSummary>();
            lstEntity = BAL.CommonMgmt.GetDashboardDailySummary("", 0, 0, Session["LoginUserID"].ToString());

            spnDueSalesOrder.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DueDispatch.ToString() : "0";
            spnDuePurchaseOrder.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DuePurchase.ToString() : "0";
            spnDuePayable.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DuePayable.ToString() : "0";
            spnDueReceivable.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DueReceivable.ToString() : "0";
            spnPurchasePaySch.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DuePurchasePaySch.ToString() : "0";
            spnSalesPaySch.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DueSalesPaySch.ToString() : "0";

            spnAppSalesOrder.InnerText = (lstEntity.Count > 0) ? lstEntity[0].AppSalesOrder.ToString() : "0";
            spnAppPurchaseOrder.InnerText = (lstEntity.Count > 0) ? lstEntity[0].AppPurchaseOrder.ToString() : "0";

            spnDocInquiry.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DocInquiry.ToString() : "0";
            spnDocQuotation.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DocQuotation.ToString() : "0";
            spnDocSalesOrder.InnerText = (lstEntity.Count > 0) ? lstEntity[0].DocSalesOrder.ToString() : "0";
        }

    }
}