using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myAMCContract : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        public void BindContractList(string loginuserid, string ComplaintStatus, Int64 pMon, Int64 pYear)
        {
            int TotRec;
            rptContract.DataSource = null;
            List<Entity.ContractInfo> lstEntity = new List<Entity.ContractInfo>();
            lstEntity = BAL.ContractInfoMgmt.GetContractInfoList(0, Session["LoginUserID"].ToString(), "", ComplaintStatus, pMon, pYear, 1, 10000, out TotRec);
            rptContract.DataSource = lstEntity;
            rptContract.DataBind();

        }
        protected void rptContract_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnRenewDays = ((HiddenField)e.Item.FindControl("hdnRenewDays"));
                HtmlGenericControl lblRenewDays = ((HtmlGenericControl)e.Item.FindControl("lblRenewDays"));
                if (String.IsNullOrEmpty(hdnRenewDays.Value))
                {
                    lblRenewDays.InnerText = "Under AMC";

                    lblRenewDays.Style.Add("border-radius", "8px");
                    lblRenewDays.Style.Add("background-color", "silver");
                    lblRenewDays.Style.Add("color", "navy");
                    lblRenewDays.Style.Add("padding", "3px");
                }
                else
                {
                    lblRenewDays.InnerText = (Convert.ToInt64(hdnRenewDays.Value)<=0 ) ? hdnRenewDays.Value + " Days Left" : hdnRenewDays.Value + " Days OverDue";
                    if (Convert.ToInt64(hdnRenewDays.Value) <= 0)
                    {
                        lblRenewDays.Style.Add("border-radius", "8px");
                        lblRenewDays.Style.Add("background-color", "green");
                        lblRenewDays.Style.Add("color", "white");
                        lblRenewDays.Style.Add("padding", "3px");
                    }
                    else
                    {
                        lblRenewDays.Style.Add("border-radius", "8px");
                        lblRenewDays.Style.Add("background-color", "red");
                        lblRenewDays.Style.Add("color", "white");
                        lblRenewDays.Style.Add("padding", "3px");
                    }
                }
            }
        }
        public Int64 ContractCount
        {
            get
            {
                return Convert.ToInt64(rptContract.Items.Count);
            }
        }
    }
}