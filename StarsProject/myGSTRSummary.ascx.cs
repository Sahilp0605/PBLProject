using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myGSTRSummary : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {

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
        public void BindGSTRSummary()
        {
            Int64 pMon, pYear;
            pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
            pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
            rptGSTR.DataSource = BAL.CommonMgmt.GSTRSummary(pageView, pMon, pYear, 0, Session["LoginUserID"].ToString());
            rptGSTR.DataBind();
        }
        protected void rptGSTR_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "gstrDetail")
            {

                Int64 pMon, pYear;
                pMon = (!String.IsNullOrEmpty(hdnMonth.Value)) ? Convert.ToInt64(hdnMonth.Value) : 0;
                pYear = (!String.IsNullOrEmpty(hdnYear.Value)) ? Convert.ToInt64(hdnYear.Value) : 0;
                
                HtmlTableRow trDetail = (HtmlTableRow)e.Item.FindControl("trDetail");
                if (trDetail.Visible == false)
                {
                    
                    Repeater detRepeater = ((Repeater)e.Item.FindControl("rptGSTRDetail"));
                    List<Entity.GSTR> lstGSTR = new List<Entity.GSTR>();
                    lstGSTR = BAL.CommonMgmt.GSTRSummary(e.CommandArgument.ToString(), pMon, pYear, 0, Session["LoginUserID"].ToString());
                    if (lstGSTR.Count > 0)
                    {
                        trDetail.Visible = true;
                        detRepeater.DataSource = lstGSTR;
                        detRepeater.DataBind();
                    }
                }
                else
                {
                    trDetail.Visible = false;
                }
            }

        }
        protected void rptGSTR_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnModule = (HiddenField)e.Item.FindControl("hdnModule");
                if (hdnModule.Value == "GSTR1TOTAL" || hdnModule.Value == "GSTR2TOTAL")
                {
                    HtmlTableRow trDataRow = (HtmlTableRow)e.Item.FindControl("trDataRow");
                    trDataRow.Style.Add("background-color", "#e8e8c0");
                }
            }
        }
    }
}