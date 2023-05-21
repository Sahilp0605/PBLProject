using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class RuleEngineSetup : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    /* BindValidation(); */
        //    if (!IsPostBack)
        //    {

        //        Session["PageNo"] = 1;
        //        Session["PageSize"] = System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString();
        //        //Session["PageSize"] = 5;
        //        Session["OldUserID"] = "0";
        //        hdnID.Value = "0";
        //        BindDropdown();
        //        BindUserData();

        //        ClearAllField();
        //    }
        //}

        //public void BindDropdown()
        //{
        //    List<Entity.Category> lstCategory = new List<Entity.Category>();
        //    lstCategory = BAL.CategoryMgmt.GetCategoryListForDropdown();
        //    drpCategory.DataSource = lstCategory ;
        //    drpCategory.DataValueField = "CategoryID";
        //    drpCategory.DataTextField = "Description";
        //    drpCategory.DataBind();
        //    drpCategory.Items.Insert(0, new ListItem("-- Select --", ""));

        //    drpCategoryMaster.DataSource = lstCategory;
        //    drpCategoryMaster.DataValueField = "CategoryID";
        //    drpCategoryMaster.DataTextField = "Description";
        //    drpCategoryMaster.DataBind();
        //    drpCategoryMaster.Items.Insert(0, new ListItem("-- Select --", ""));   
        //    // =================================================================
            
        //    List<Entity.Events> lstEvents = new List<Entity.Events>();
        //    lstEvents = BAL.EventsMgmt.GetEventListForDropdown();
        //    drpEvent.DataSource = lstEvents;
        //    drpEvent.DataValueField = "EventID";
        //    drpEvent.DataTextField = "Description";
        //    drpEvent.DataBind();
        //    drpEvent.Items.Insert(0, new ListItem("-- Select --", ""));

        //    drpsrcEvent.DataSource = lstEvents;
        //    drpsrcEvent.DataValueField = "EventID";
        //    drpsrcEvent.DataTextField = "Description";
        //    drpsrcEvent.DataBind();
        //    drpsrcEvent.Items.Insert(0, new ListItem("-- Select --", ""));

        //    // =================================================================
        //    List<Entity.Exam> lstExam = new List<Entity.Exam>();
        //    lstExam = BAL.ExamMgmt.GetExamListForDropdown();
        //    drpExam.DataSource = lstExam;
        //    drpExam.DataValueField = "ExamID";
        //    drpExam.DataTextField = "Description";
        //    drpExam.DataBind();
        //    drpExam.Items.Insert(0, new ListItem("-- Select --", ""));   

        //    drpExamMaster.DataSource = lstExam;
        //    drpExamMaster.DataValueField = "ExamID";
        //    drpExamMaster.DataTextField = "Description";
        //    drpExamMaster.DataBind();
        //    drpExamMaster.Items.Insert(0, new ListItem("-- Select --", ""));

        //    List<Entity.RuleEngine> lstRules = new List<Entity.RuleEngine>();
        //    lstRules = BAL.RuleEngineMgmt.GetDomicileList();
        //    drpDomicile.DataSource = lstRules;
        //    drpDomicile.DataValueField = "Domicial";
        //    drpDomicile.DataTextField = "Domicial";
        //    drpDomicile.DataBind();
        //    drpDomicile.Items.Insert(0, new ListItem("-- Select --", ""));

        //    drpSrcDomicile.DataSource = lstRules;
        //    drpSrcDomicile.DataValueField = "Domicial";
        //    drpSrcDomicile.DataTextField = "Domicial";
        //    drpSrcDomicile.DataBind();
        //    drpSrcDomicile.Items.Insert(0, new ListItem("-- Select --", ""));
        //}

        //public void BindUserData()
        //{
        //    List<Entity.RuleEngine> lstRuleEngine = new List<Entity.RuleEngine>();
        //    int TotalCount = 0;
        //    lstRuleEngine = BAL.RuleEngineMgmt.GetRuleEngineList(0, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
        //    rptUser.DataSource = lstRuleEngine;
        //    rptUser.DataBind();
        //    pageGrid.BindPageing(TotalCount);
        //}

        //public void BindUserDataList()
        //{
        //    List<Entity.RuleEngine> lstRuleEngine = new List<Entity.RuleEngine>();
        //    int TotalCount = 0;
        //    Entity.RuleEngineSearch objSearch = new Entity.RuleEngineSearch();
        //    objSearch.ExamID = drpExamMaster.SelectedItem.Value.ToString();
        //    objSearch.CategoryID = drpCategoryMaster.SelectedItem.Value.ToString();
        //    objSearch.Domicial = drpSrcDomicile.SelectedValue;
        //    objSearch.Gender = drpSrcGender.SelectedValue;
        //    objSearch.EventID = drpsrcEvent.SelectedValue;
        //    objSearch.PageNo = Convert.ToInt32(Session["PageNo"]);
        //    objSearch.PageSize = Convert.ToInt32(Session["PageSize"]);
        //    lstRuleEngine = BAL.RuleEngineMgmt.GetRuleEngineListOnExamID(objSearch, out TotalCount);
        //    rptUser.DataSource = lstRuleEngine;
        //    rptUser.DataBind();
        //    pageGrid.BindPageing(TotalCount);
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    Entity.RuleEngine objRuleEngine = new Entity.RuleEngine();
        //    objRuleEngine.ID = Convert.ToInt32(Session["OldUserID"]);
        //    objRuleEngine.Gender = drpGender.SelectedItem.Value;
        //    objRuleEngine.CategoryID = drpCategory.SelectedItem.Value;
        //    objRuleEngine.ExamID = drpExam.SelectedItem.Value;
        //    objRuleEngine.EventID = drpEvent.SelectedItem.Value;
        //    objRuleEngine.Eligibility = Convert.ToDecimal(txtEligibility.Text.Trim() == "" ? "0" : txtEligibility.Text.Trim());
        //    objRuleEngine.Eligibility_To = Convert.ToDecimal(txtEligibility_To.Text.Trim() == "" ? "0" : txtEligibility_To.Text.Trim());
        //    objRuleEngine.Domicial = drpDomicile.SelectedValue;
        //    int ReturnCode = 0;
        //    string ReturnMsg = "";
        //    BAL.RuleEngineMgmt.AddUpdateRuleEngine(objRuleEngine, out ReturnCode, out ReturnMsg);
        //    ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
        //    ClearAllField();
        //    BindUserData();
        //}

        //public void ClearAllField()
        //{
        //    txtEligibility.Text = "";
        //    txtEligibility_To.Text = "";
        //    drpGender.SelectedIndex = 0;
        //    drpCategory.SelectedIndex = 0;
        //    drpEvent.SelectedIndex = 0;
        //    drpExam.SelectedIndex = 0;
        //    hdnID.Value = "";
        //    Session["OldUserID"] = "0";
        //}

        //protected void btnReset_Click(object sender, EventArgs e)
        //{
        //    ClearAllField();
        //    Session["OldUserID"] = "0";
        //}

        //protected void rptUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName.ToString() == "Edit")
        //    {
        //        int TotalCount = 0;
        //        List<Entity.RuleEngine> lstRuleEngine = new List<Entity.RuleEngine>();

        //        lstRuleEngine = BAL.RuleEngineMgmt.GetRuleEngineList(Convert.ToInt32(e.CommandArgument.ToString()), 0, 0, out TotalCount);

        //        Session["OldUserID"] = e.CommandArgument.ToString();
        //        drpGender.SelectedValue = lstRuleEngine[0].Gender;
        //        drpCategory.SelectedValue = lstRuleEngine[0].CategoryID ;
        //        drpExam.SelectedValue = lstRuleEngine[0].ExamID ;
        //        drpEvent.SelectedValue = lstRuleEngine[0].EventID;
        //        txtEligibility.Text = lstRuleEngine[0].Eligibility.ToString("0.00");
        //        txtEligibility_To.Text = lstRuleEngine[0].Eligibility_To.ToString("0.00");
        //    }
        //    else if (e.CommandName.ToString() == "Delete")
        //    {
        //        int ReturnCode = 0;
        //        string ReturnMsg = "";
        //        BAL.RuleEngineMgmt.DeleteCategory(Convert.ToInt32(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
        //        ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
        //        BindUserData();
        //    }
        //}

        //protected void Pager_Changed(object sender, PagerEventArgs e)
        //{
        //    //BindUserData();
        //    BindUserDataList();
        //    ScriptManager.RegisterStartupScript(this, typeof(string), "color", "javascript:changecolor();", true);
        //}

        ////protected void drpExamMaster_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    Session["PageNo"] = 1;
        ////    ClearAllField();
        ////    BindUserDataList();
        ////}

        ////protected void drpCategoryMaster_SelectedIndexChanged(object sender, EventArgs e)
        ////{
        ////    Session["PageNo"] = 1;
        ////    ClearAllField();
        ////    BindUserData(drpExamMaster.SelectedItem.Value.ToString(), drpCategoryMaster.SelectedItem.Value.ToString());
        ////}
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    Session["PageNo"] = 1;
        //    Session["PageSize"] = System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString();
        //    ClearAllField();
        //    BindUserDataList();
        //}
        //protected void bthSearchReset_Click(object sender, EventArgs e)
        //{
        //    BindDropdown();
        //    Session["PageNo"] = 1;
        //    Session["PageSize"] = System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString();
        //    BindUserDataList();
        //}
    }
}