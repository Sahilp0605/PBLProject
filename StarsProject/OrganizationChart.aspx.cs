using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;


namespace StarsProject
{
    public partial class OrganizationChart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString();
                Session["OldUserID"] = "";
                // ----------------------------------------
                if (String.IsNullOrEmpty(Request.QueryString["keyCode"]) == false)
                {
                    hdnSelectedChartType.Value = Request.QueryString["keyType"].ToString();
                    hdnSelectedDeptCode.Value = Request.QueryString["keyCode"].ToString();
                }
                else
                {
                    hdnSelectedChartType.Value = "DEPT";
                    hdnSelectedDeptCode.Value = "100";
                }
                // ----------------------------------------
            }
        }

        public string ConvertOrgChart()
        {
            List<Entity.OrgChart> lstEntity = new List<Entity.OrgChart>();

            lstEntity = BAL.CommonMgmt.GetOrgChartList(Session["LoginUserID"].ToString());
            // -------------------------------------------------
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            // -------------------------------------------------
            foreach (Entity.OrgChart point in lstEntity)
            {
                row = new Dictionary<string, object>();
                row.Add("id", point.id);
                row.Add("name", point.name);
                row.Add("parent", point.parent);
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

    }
}