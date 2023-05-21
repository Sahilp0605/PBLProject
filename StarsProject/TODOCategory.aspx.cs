﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace StarsProject
{
    public partial class TODOCategory : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["OldUserID"] = "";
                // ----------------------------------------
                ClearAllField();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        ClearAllField();
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
            txtCategoryName.Text = "";
            txtCategoryName.Focus();
        }

        public void OnlyViewControls()
        {
            txtCategoryName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.ToDoCategory> lstEntity = new List<Entity.ToDoCategory>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.ToDoCategoryMgmt.GetTODOCategoryList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(),"", Convert.ToInt32(Session["PageNo"].ToString()), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtCategoryName.Text = lstEntity[0].TaskCategoryName;
                txtCategoryName.Focus();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(txtCategoryName.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCategoryName.Text))
                    _pageErrMsg = "<li>Category Name is required.</li>";
            }
            // -----------------------------------------------------------------
            Entity.ToDoCategory objEntity = new Entity.ToDoCategory();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.Category = "TODO";
                objEntity.TaskCategoryName = txtCategoryName.Text.Trim();
                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.ToDoCategoryMgmt.AddUpdateTODOCategory(objEntity, out ReturnCode, out ReturnMsg);
                _pageErrMsg = "<li>" + ReturnMsg + "</li>";
            }
            // -------------------------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);

        }
        [System.Web.Services.WebMethod]
        public static string DeleteTODOCategory(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ToDoCategoryMgmt.DeleteTODOCategory(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}