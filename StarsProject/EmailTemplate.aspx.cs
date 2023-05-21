using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
//using AjaxControlToolkit.HTMLEditor;

namespace StarsProject
{
    // Need to add <httpRuntime requestValidationMode = "2.0" /> in web.config.
    public partial class EmailTemplate : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;  
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["CurrentRole"] = "";
                Session["Access"] = "";
                Session["NoAccess"] = "";
                Session["OldUserID"] = "";
                // ----------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnTemplateID.Value = Request.QueryString["id"].ToString();

                    if (String.IsNullOrEmpty(hdnTemplateID.Value))
                    {
                        ClearAllField();
                    }
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }

        }


        public void ClearAllField()
        {
            Session["OldUserID"] = "";
            hdnTemplateID.Value = "";
            txtTemplateID.Text = "";
            txtDescription.Text = "";
            txtSubject.Text = "";
            txtEditor.Text = "";
            hdnEditor.Value = "";
        }

        public void setLayout(string mode)
        {
            if (mode == "Edit")
            {
                int TotalRecord = 0;
                List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.EmailTemplateMgmt.GetEmailTemplate(hdnTemplateID.Value, "", 1, Convert.ToInt32(Session["PageSize"]), out TotalRecord);
                if (lstEntity.Count>0)
                {
                    txtTemplateID.Text = lstEntity[0].TemplateID;
                    drpCategory.SelectedValue = lstEntity[0].Category;
                    txtDescription.Text = lstEntity[0].Description;
                    txtSubject.Text = lstEntity[0].Subject;
                    txtEditor.Text = HttpUtility.HtmlDecode(lstEntity[0].ContentData);
                    hdnEditor.Value =  HttpUtility.HtmlDecode(lstEntity[0].ContentData);
                    hdnImageAttachment1.Value = lstEntity[0].ImageAttachment1;
                    hdnImageAttachment2.Value = lstEntity[0].ImageAttachment2;
                    if (lstEntity[0].ActiveFlag == true)
                        drpActive.SelectedValue = "1";
                    else
                        drpActive.SelectedValue = "0";
                }
            }
        }

        public void OnlyViewControls()
        {
            txtTemplateID.ReadOnly = true;
            txtDescription.ReadOnly = true;
            txtSubject.ReadOnly = true;
            txtEditor.ReadOnly = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "", strErr = "";

            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtTemplateID.Text) || String.IsNullOrEmpty(txtDescription.Text) || String.IsNullOrEmpty(txtSubject.Text))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtTemplateID.Text))
                    strErr = "<li>" + "Template ID is required." + "</li>";

                if (String.IsNullOrEmpty(txtDescription.Text))
                    strErr += "<li>" + "Template Description is required." + "</li>";

                if (String.IsNullOrEmpty(txtSubject.Text))
                    strErr += "<li>" + "Template Subject is required." + "</li>";

            }
            // -----------------------------------------------------------------
            Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
            if (_pageValid)
            {
                objEntity.TemplateID = txtTemplateID.Text.Trim();
                objEntity.Category = drpCategory.SelectedValue.ToUpper();
                objEntity.Description = txtDescription.Text.Trim();
                objEntity.Subject = txtSubject.Text.Trim();
                objEntity.ContentData = HttpUtility.HtmlEncode(txtEditor.Text);
                objEntity.ImageAttachment1 = hdnImageAttachment1.Value;
                objEntity.ImageAttachment2 = hdnImageAttachment2.Value;
                if (drpActive.SelectedValue == "1")
                    objEntity.ActiveFlag = true;
                else
                    objEntity.ActiveFlag = false;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.EmailTemplateMgmt.AddUpdateEmailTemplate(objEntity, out ReturnCode, out ReturnMsg);
                // -------------------------------------------------------------- 
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0) 
                    ClearAllField(); 
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            btnReset.Attributes.Add("OnClick", "javascript: myfun()");
        }

        [System.Web.Services.WebMethod]
        public static string DeleteEmailTemplate(string pTemplateID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.EmailTemplateMgmt.DeleteEmailTemplate(pTemplateID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}