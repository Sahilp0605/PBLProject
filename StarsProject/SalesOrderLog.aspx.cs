using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SalesOrderLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnHeaderID.Value = Request.QueryString["id"].ToString();

                    List<Entity.SalesOrder> lstEntityLog = new List<Entity.SalesOrder>();
                    lstEntityLog = BAL.SalesOrderMgmt.GetSalesOrderLogList(hdnHeaderID.Value);
                    rptSalesOrderLog.DataSource = lstEntityLog;
                    rptSalesOrderLog.DataBind();
                }
            }
        }

        protected void rptSalesOrderLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnpkID = (HiddenField)e.Item.FindControl("hdnpkID");
                Repeater rptControl = (Repeater)e.Item.FindControl("rptOrderDocs");
                DataTable dtDetail1 = new DataTable();
                List<Entity.SalesorderDocuments> lst = BAL.SalesOrderMgmt.GetSalesOrderDocumentsList(0, Convert.ToInt64(hdnpkID.Value), "");
                dtDetail1 = PageBase.ConvertListToDataTable(lst);
                rptControl.DataSource = dtDetail1;
                rptControl.DataBind();
            }
        }
    }
}