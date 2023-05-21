using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace StarsProject
{
    public partial class CheckList : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
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
                    BindCheckList("");
                }
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.CheckList> lstEntity = new List<Entity.CheckList>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.CheckListMgmt.GetCheckList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtCheckHead.Text = lstEntity[0].CheckHead;
                txtCheckDesc.Text = lstEntity[0].CheckDesc;
                txtCheckHead.Focus();
            }
        }
        public void OnlyViewControls()
        {
            txtCheckHead.ReadOnly = true;
            txtCheckDesc.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            txtCheckHead.Text = "";
            txtCheckDesc.Text = "";
            txtCheckHead.Focus();
            btnSave.Disabled = false;
        }
        public void BindDropDown()
        {
            // ---------------- Assign Check Head ------------------------
            List<Entity.CheckList> lstCheckHead = new List<Entity.CheckList>();
            lstCheckHead = BAL.CheckListMgmt.GetCheckHead();
            drpCheckType.DataSource = lstCheckHead;
            drpCheckType.DataValueField = "CheckHead";
            drpCheckType.DataTextField = "CheckHead";
            drpCheckType.DataBind();
            drpCheckType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
        }
        public void BindCheckList(String CheckHead)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.CheckListMgmt.GetCheckList(CheckHead);
            rptCheckListDetail.DataSource = dtDetail1;
            rptCheckListDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtCheckHead.Text) || String.IsNullOrEmpty(txtCheckDesc.Text) )
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCheckHead.Text))
                {
                    strErr += "<li>" + "CheckList Head is mandatory !" + "</li>";
                }
                if (String.IsNullOrEmpty(txtCheckDesc.Text))
                {
                    strErr += "<li>" + "CheckList Description is mandatory !" + "</li>";
                }

            }
          
            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.CheckList objEntity = new Entity.CheckList();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.CheckHead = txtCheckHead.Text;
                objEntity.CheckDesc= txtCheckDesc.Text;
               

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CheckListMgmt.AddUpdateCheckList(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    BindDropDown();
                }
            }
            if (ReturnCode > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        }

    
        [System.Web.Services.WebMethod]
        public static string DeleteCheckList(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CheckListMgmt.DeleteCheckList(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpCheckType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpCheckType.Text != "--Select--")
            {
                BindCheckList(drpCheckType.SelectedValue);
                if (drpCheckType.Text == "0")
                {
                    txtCheckHead.Text = "";
                    txtCheckDesc.Text = "";
                }
                else
                    txtCheckHead.Text = drpCheckType.Text;
            }
            else
            {
                txtCheckHead.Text = "";
                txtCheckDesc.Text = "";
            }
        }
    }
}