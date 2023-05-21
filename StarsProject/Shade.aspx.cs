﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Shade : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
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
        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            _pageErrMsg = "";

            if (String.IsNullOrEmpty(txtShadeName.Text))
            {
                _pageErrMsg += "<li>" + "Shade Name is required." + "</li>";
                _pageValid = false;


            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.Shade objEntity = new Entity.Shade();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.ShadeName = (!String.IsNullOrEmpty(txtShadeName.Text)) ? txtShadeName.Text.Trim() : "";

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ShadeMgmt.AddUpdateShade(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        public void ClearAllField()
        {
            txtShadeName.Text = "";
        }
        public void OnlyViewControls()
        {
            txtShadeName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Shade> lstEntity = new List<Entity.Shade>();
                lstEntity = BAL.ShadeMgmt.GetShade(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                txtShadeName.Text = (!String.IsNullOrEmpty(lstEntity[0].ShadeName)) ? lstEntity[0].ShadeName.Trim() : "";
                txtShadeName.Focus();
            }
        }
        [System.Web.Services.WebMethod]
        public static string DeleteShade(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ShadeMgmt.DeleteShade(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);
            return serializer.Serialize(rows);
        }
    }
}