using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ClientLedger : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
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

        public Decimal GetDebitAmount
        {
            get { return Convert.ToDecimal(hdnDebit.Value); }
        }

        public Decimal GetCreditAmount
        {
            get { return Convert.ToDecimal(hdnCredit.Value); }
        }


        public void BindClientLedger(Int64 pCustomerID, string loginuserid)
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            int totrec;
            rptClientLedger.DataSource = BAL.CustomerMgmt.GetCustomerLedgerList(pCustomerID, loginuserid, pMon, pYear);
            rptClientLedger.DataBind();
        }

        public void BindClientLedgerByDBCR(Int64 pCustomerID, string loginuserid, string pCategory)
        {
            int totrec;
            Int64 pMon, pYear;

            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;

            List<Entity.Customer> lstEntity = new List<Entity.Customer>();
            lstEntity = BAL.CustomerMgmt.GetCustomerLedgerList(pCustomerID, loginuserid, pMon, pYear);

            if (pCategory == "R")
            {
                lstEntity = lstEntity.Where(x => Convert.ToDecimal(x.ClosingAmount) > 0).ToList();
            }
            else
            {
                lstEntity = lstEntity.Where(x => Convert.ToDecimal(x.ClosingAmount) < 0).ToList();
            }
            // -------------------------------------------------------------
            rptClientLedger.DataSource = lstEntity;
            rptClientLedger.DataBind();

        }

        protected void rptClientLedger_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnIDebit = ((HiddenField)e.Item.FindControl("hdnItemDebit"));
                HiddenField hdnICredit = ((HiddenField)e.Item.FindControl("hdnItemCredit"));
                // --------------------------------------
                Decimal tmpVal1 = Convert.ToDecimal(hdnIDebit.Value);
                Decimal tmpVal2 = Convert.ToDecimal(hdnICredit.Value);
                hdnDebit.Value = (Convert.ToDecimal(hdnDebit.Value) + tmpVal1).ToString();
                hdnCredit.Value = (Convert.ToDecimal(hdnCredit.Value) + tmpVal2).ToString();
            }
        }
    }
}