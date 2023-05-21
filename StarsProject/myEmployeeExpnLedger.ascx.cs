using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myEmployeeExpnLedger : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        public Decimal tmpVal1, tmpVal2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public string EmployeeName
        {
            get { return hdnEmployeeName.Value; }
            set { hdnEmployeeName.Value = value; }
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

        public string EmployeeID
        {
            get { return hdnEmployeeID.Value; }
            set { hdnEmployeeID.Value = value; }
        }
        public List<Entity.OrganizationEmployee> lstExpnSummary = new List<Entity.OrganizationEmployee>();

        public void BindEmployeeExpnLedger()
        {

            hdnClosing.Value = "0";
            Int64 pMon = 0, pYear = 0;

            if (!String.IsNullOrEmpty(hdnMonth.Value))
                pMon = Convert.ToInt64(hdnMonth.Value);

            if (!String.IsNullOrEmpty(hdnYear.Value))
                pYear = Convert.ToInt64(hdnYear.Value);
            // -----------------------------------------------
            rptExpnLedgerDetail.DataSource = null;
            List<Entity.OrganizationEmployee> lstSummary = new List<Entity.OrganizationEmployee>();
            lstSummary = BAL.OrganizationEmployeeMgmt.GetEmployeeExpnLedgerList(Convert.ToInt64(hdnEmployeeID.Value), pMon, pYear, Session["LoginUserID"].ToString());
            rptExpnLedgerDetail.DataSource = lstSummary;
            rptExpnLedgerDetail.DataBind();
        }

        public List<Entity.OrganizationEmployee> BindEmployeeExpnLedgerSummary()
        {

            hdnClosing.Value = "0";
            Int64 pMon = 0, pYear = 0;

            if (!String.IsNullOrEmpty(hdnMonth.Value))
                pMon = Convert.ToInt64(hdnMonth.Value);

            if (!String.IsNullOrEmpty(hdnYear.Value))
                pYear = Convert.ToInt64(hdnYear.Value);
            // -----------------------------------------------
            rptExpnLedgerDetail.DataSource = null;

            List<Entity.OrganizationEmployee> lstSummary = new List<Entity.OrganizationEmployee>();
            lstSummary = BAL.OrganizationEmployeeMgmt.GetEmployeeExpnLedgerList(Convert.ToInt64(hdnEmployeeID.Value), pMon, pYear, Session["LoginUserID"].ToString());
            
            return lstSummary;
        }

        protected void rptExpnLedgerDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                HtmlTableCell tdHeaderDebit = (HtmlTableCell)e.Item.FindControl("tdHeaderDebit");
                HtmlTableCell tdHeaderCredit = (HtmlTableCell)e.Item.FindControl("tdHeaderCredit");
                HtmlTableCell tdHeaderClosing = (HtmlTableCell)e.Item.FindControl("tdHeaderClosing");

                if (Convert.ToDecimal(hdnClosing.Value) >= 0)
                    tdHeaderDebit.InnerText = hdnClosing.Value;
                else if (Convert.ToDecimal(hdnClosing.Value) < 0)
                    tdHeaderCredit.InnerText = hdnClosing.Value;

                tdHeaderClosing.InnerText = hdnClosing.Value;
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnDbAmount = ((HiddenField)e.Item.FindControl("hdnDbAmount"));
                HiddenField hdnCrAmount = ((HiddenField)e.Item.FindControl("hdnCrAmount"));
                ///HtmlTableCell tdOpening = (HtmlTableCell)e.Item.FindControl("tdOpening");
                HtmlTableCell tdClosing = (HtmlTableCell)e.Item.FindControl("tdClosing");
                // --------------------------------------
                //tdOpening.InnerText = hdnClosing.Value;
                tmpVal1 = Convert.ToDecimal(hdnDbAmount.Value);
                tmpVal2 = Convert.ToDecimal(hdnCrAmount.Value);
                hdnClosing.Value = (Convert.ToDecimal(hdnClosing.Value) + (tmpVal1 - tmpVal2)).ToString();
                tdClosing.InnerText = hdnClosing.Value;

                if (Convert.ToDecimal(hdnClosing.Value) > 0)
                {
                    tdClosing.Attributes.Add("style", "color: Red; text-align:right;");
                }
                else if (Convert.ToDecimal(hdnClosing.Value) < 0)
                {
                    tdClosing.Attributes.Add("style", "color: Navy; text-align:right;");
                }
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                HtmlTableCell tdFooterDebit = (HtmlTableCell)e.Item.FindControl("tdFooterDebit");
                HtmlTableCell tdFooterCredit = (HtmlTableCell)e.Item.FindControl("tdFooterCredit");
                HtmlTableCell tdFooterClosing = (HtmlTableCell)e.Item.FindControl("tdFooterClosing");

                if (Convert.ToDecimal(hdnClosing.Value) >= 0)
                    tdFooterDebit.InnerText = hdnClosing.Value;
                else if (Convert.ToDecimal(hdnClosing.Value) < 0)
                    tdFooterCredit.InnerText = hdnClosing.Value;

                //tdFooterClosing.InnerText = hdnClosing.Value;
            }

        }

    }
}