using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class GeneralTemplate : System.Web.UI.Page
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
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (String.IsNullOrEmpty(hdnpkID.Value))
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
            hdnpkID.Value = "";
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
                lstEntity = BAL.EmailTemplateMgmt.GetGeneralTemplate(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), 1, Convert.ToInt32(Session["PageSize"]), out TotalRecord);
                if (lstEntity.Count > 0 && Convert.ToInt64(hdnpkID.Value) != 0)
                {
                    txtSubject.Text = lstEntity[0].Subject;
                    txtEditor.Text = HttpUtility.HtmlDecode(lstEntity[0].ContentData);
                    hdnEditor.Value = HttpUtility.HtmlDecode(lstEntity[0].ContentData);
                 }
                else
                {
                    txtSubject.Text = "";
                    txtEditor.Text = "";
                    hdnEditor.Value = "";
                }
            }
        }
        public void OnlyViewControls()
        {
            txtSubject.ReadOnly = true;
            txtEditor.ReadOnly = true;
        }
       
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            //btnReset.Attributes.Add("OnClick", "javascript: myfun()");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "", strErr = "";

            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtSubject.Text))
            {

                _pageValid = false;

                if (String.IsNullOrEmpty(txtSubject.Text))
                    strErr += "<li>" + "Template Subject is required." + "</li>";

            }
            // -----------------------------------------------------------------
            Entity.EmailTemplate objEntity = new Entity.EmailTemplate();
            if (_pageValid)
            {
                objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.Subject = txtSubject.Text.Trim();
                objEntity.ContentData = HttpUtility.HtmlEncode(txtEditor.Text);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.EmailTemplateMgmt.AddUpdateGeneralTemplate(objEntity, out ReturnCode, out ReturnMsg);
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

        [System.Web.Services.WebMethod]
        public static string DeleteGenTemplate(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.EmailTemplateMgmt.DeleteGeneralTemplate(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}