using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace StarsProject
{
    public partial class CompanyRegistration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["CurrentRole"] = "";
                Session["Access"] = "";
                Session["NoAccess"] = "";
                Session["OldUserID"] = "";

                bindGrid();
            }
            //bindGrid();
        }
        private void bindGrid()
        {
            int TotalCount = 0;
            List<Entity.CompanyRegistration> lstEntity = new List<Entity.CompanyRegistration>();
            lstEntity = BAL.CompanyRegistrationMgmt.GetCompanyRegistrationList(Session["LoginUserID"].ToString(),Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]),out TotalCount);
            rptRegistration.DataSource = lstEntity;
            rptRegistration.DataBind();
            pageGrid.BindPageing(TotalCount);
        }
        protected void rptRegistration_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Edit")
            {
                hdnpkID.Value = e.CommandArgument.ToString();                
                ScriptManager.RegisterStartupScript(this, GetType(), "RegPage", "javascript:openCompanyRegistration(" + hdnpkID.Value + "); ", true);
            }
            //else if (e.CommandName.ToString() == "Delete")
            //{
            //    int ReturnCode = 0;
            //    string ReturnMsg = "";
            //    // -------------------------------------------------------------- Delete Record
            //    BAL.EmailTemplateMgmt.DeleteEmailTemplate(e.CommandArgument.ToString(), out ReturnCode, out ReturnMsg);
            //    ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
            //    // -------------------------------------------------------------------------
            //    bindGrid();
            //}
        }
        protected void rptRegistration_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            
        }
        protected void Pager_Changed(object sender, PagerEventArgs e)
        {           
            bindGrid();
            ScriptManager.RegisterStartupScript(this, typeof(string), "color", "javascript:changecolor();", true);
        }     
    }
}