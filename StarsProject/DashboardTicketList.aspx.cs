using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace StarsProject
{
    public partial class DashboardTicketList : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    // --------------------------------------------------------------------------------------------------
        //    Entity.Authenticate objAuth = new Entity.Authenticate();
        //    objAuth = (Entity.Authenticate)Session["logindetail"];
        //    // --------------------------------------------------------------------------------------------------
        //    Session["PageNo"] = 1;
        //    Session["PageSize"] = 10;
        //    Session["OldUserID"] = "";
        //    // ----------------------------------------
        //    BindHelpLogData();
        //}

        //public void BindHelpLogData()
        //{
        //    List<Entity.HelpLog> lstEntity = new List<Entity.HelpLog>();
        //    int TotalCount = 0;

        //    if (!String.IsNullOrEmpty(Request.QueryString["LogStatus"]))
        //    {
        //        hdnMode.Value = "LS";
        //        lstEntity = BAL.HelpLogMgmt.GetHelpLogListByStatus(Request.QueryString["LogStatus"].ToString(), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

        //        rptHelpLogCrime.Visible = false;
        //        rptHelpLog.Visible = true;
        //        rptHelpLog.DataSource = lstEntity;
        //        rptHelpLog.DataBind();
        //    }
        //    else if (!String.IsNullOrEmpty(Request.QueryString["CrimeTypeDesc"]))
        //    {
        //        hdnMode.Value = "CD";

        //        lstEntity = BAL.HelpLogMgmt.GetHelpLogListByCrimeTypeDesc(Request.QueryString["CrimeTypeDesc"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

        //        rptHelpLog.Visible = false;
        //        rptHelpLogCrime.Visible = true;
        //        rptHelpLogCrime.DataSource = lstEntity;
        //        rptHelpLogCrime.DataBind();
        //    }
        //    pageGrid.BindPageing(TotalCount);

        //}
        //protected void rptHelpLog_ItemDataBound(object source, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        Entity.Authenticate objAuth = new Entity.Authenticate();
        //        objAuth = (Entity.Authenticate)Session["logindetail"];
        //        // -----------------------------------------------------------------------------------
        //        HtmlTableCell tdLogStatus = (HtmlTableCell)e.Item.FindControl("tdLogStatus");
        //        Label lblFlag = e.Item.FindControl("lblLogStatus") as Label;
        //        ImageButton ImgbtnSelect = e.Item.FindControl("ImgbtnSelect") as ImageButton;
        //        // -----------------------------------------------------------------------------------
        //        lblFlag.Font.Bold = true;
        //        if (lblFlag.Text == "Initiated")
        //        {
        //            tdLogStatus.Attributes.Add("style", "background-color: White; text-align:center;");
        //            lblFlag.ForeColor = System.Drawing.Color.Red;
        //        }
        //        else if (lblFlag.Text == "Dispatched")
        //        {
        //            tdLogStatus.Attributes.Add("style", "background-color: Orange; text-align:center;");
        //            lblFlag.ForeColor = System.Drawing.Color.Black;
        //            if (objAuth.RoleCode.ToUpper() != "ADMIN")
        //                ImgbtnSelect.Visible = false;
        //        }
        //        else if (lblFlag.Text == "Accepted")
        //        {
        //            tdLogStatus.Attributes.Add("style", "background-color: Green; text-align:center;");
        //            lblFlag.ForeColor = System.Drawing.Color.Black;
        //            if (objAuth.RoleCode.ToUpper() != "ADMIN")
        //                ImgbtnSelect.Visible = false;
        //        }
        //        else if (lblFlag.Text == "Rejected")
        //        {
        //            tdLogStatus.Attributes.Add("style", "background-color: Red; text-align:center;");
        //            lblFlag.ForeColor = System.Drawing.Color.White;
        //        }
        //        else if (lblFlag.Text == "Closed")
        //        {
        //            tdLogStatus.Attributes.Add("style", "background-color: Gray; text-align:center;");
        //            lblFlag.ForeColor = System.Drawing.Color.White;
        //            if (objAuth.RoleCode.ToUpper() != "ADMIN")
        //                ImgbtnSelect.Visible = false;

        //        }
        //    }
        //}

        //protected void Pager_Changed(object sender, PagerEventArgs e)
        //{
        //    BindHelpLogData();
        //    ScriptManager.RegisterStartupScript(this, typeof(string), "color", "javascript:changecolor();", true);
        //}
    }
}