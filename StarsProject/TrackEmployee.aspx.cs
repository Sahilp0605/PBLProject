using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class TrackEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();
                txtFollowupDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ScriptManager.RegisterStartupScript(this, typeof(string), "empmap", "javascript:initTackMap();", true);
            }
        }

        public void BindDropDown()
        {
            // ---------------- ReportTo List  -------------------------------------
            List<Entity.OrganizationEmployee> lstReportTo = new List<Entity.OrganizationEmployee>();
            lstReportTo = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            drpEmployee.DataSource = lstReportTo;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            //drpEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
        }

        public void BindFollowup()
        {
            List<Entity.Followup> lstEntity1 = new List<Entity.Followup>();
            lstEntity1 = BAL.ReportMgmt.Report_followup("followupbylocation", Convert.ToDateTime(txtFollowupDate.Text), Convert.ToDateTime(txtFollowupDate.Text), "", "", drpEmployee.SelectedValue);
            rptFollowup.DataSource = lstEntity1.OrderBy(o => o.FollowupDate).ToList(); ;
            rptFollowup.DataBind();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "empmap", "javascript:initTackMap();", true);
            BindFollowup();
        }
        // ------------------------------------------------------------------
        // Action : This method is used to convert datatable to json string
        // ------------------------------------------------------------------
        public string ConvertDataTabletoString()
        {
            //int TotalCount = 0;
            //List<Entity.HelpLog> lstEntity = new List<Entity.HelpLog>();
            //lstEntity = BAL.HelpLogMgmt.GetHelpLogList(Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), 5000, out TotalCount);
            //// -------------------------------------------------
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row;
            //// -------------------------------------------------
            //foreach (Entity.HelpLog point in lstEntity)
            //{
            //    row = new Dictionary<string, object>();
            //    if (point.LogStatus != "Closed")
            //    {
            //        row.Add("HelpLogID", point.pkID.ToString());
            //        row.Add("LogDateTime", point.LogDateTime.ToString());
            //        row.Add("MemberName", point.MemberName);
            //        row.Add("DriverName", point.DriverName);
            //        row.Add("Longitude", point.Longitude.ToString());
            //        row.Add("Latitude", point.Latitude.ToString());
            //        row.Add("LogStatus", point.LogStatus);
            //        rows.Add(row);
            //    }
            //}
            //return serializer.Serialize(rows);
            return "";
        }

        protected void rptFollowup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var liFollowupCard1 = e.Item.FindControl("liFollowupCard") as HtmlGenericControl;
            var hdnLat = e.Item.FindControl("hdnLatitude") as HiddenField;

            if (String.IsNullOrEmpty(hdnLat.Value) || hdnLat.Value == "0")
            {
                liFollowupCard1.Style.Add("background-color", "#ffeaea");
            }
        }


    }
}