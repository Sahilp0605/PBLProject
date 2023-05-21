using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myClientDetailLedger : System.Web.UI.UserControl
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

        public string CustomerName
        {
            get { return hdnCustomerName.Value; }
            set { hdnCustomerName.Value = value; }
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

        public void BindClientLedger(Int64 pCustomerID, string loginuserid)
        {
            int totrec;
            hdnClosing.Value = "0";
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            lstCust = BAL.CustomerMgmt.GetCustomerList(pCustomerID, Session["LoginUserID"].ToString(), 1, 10000, out totrec);
            hdnClosing.Value = lstCust[0].OpeningAmount.ToString();
            // -----------------------------------------------
            rptLedgerDetail.DataSource = null;
            rptLedgerDetail.DataSource = BAL.CustomerMgmt.GetCustomerDetailLedgerList(pCustomerID, loginuserid);
            rptLedgerDetail.DataBind();
        }


        protected void rptLedgerDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                HiddenField hdnRemarks = ((HiddenField)e.Item.FindControl("hdnRemarks"));
                HtmlTableRow trRemarks = (HtmlTableRow)e.Item.FindControl("trRemarks");
                trRemarks.Visible = (!String.IsNullOrEmpty(hdnRemarks.Value)) ? true : false;
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