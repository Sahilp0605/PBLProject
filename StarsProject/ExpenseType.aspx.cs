
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ExpenseType : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnExpenseTypeID.Value = Request.QueryString["id"].ToString();

                    if (hdnExpenseTypeID.Value == "0" || hdnExpenseTypeID.Value == "")
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
            txtExpenseTypeName.Text = "";
            //chkLocationRequired.Checked = true;
            
        }
        public void OnlyViewControls()
        {
            txtExpenseTypeName.ReadOnly = true;
            //chkLocationRequired.Enabled = false;
            btnSave.Visible = false;
            btnReset.Visible = false;
          
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ExpenseType> lstEntity = new List<Entity.ExpenseType>();

                lstEntity = BAL.ExpenseTypeMgmt.GetExpenseTypeList(Convert.ToInt64(hdnExpenseTypeID.Value));
                hdnExpenseTypeID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                txtExpenseTypeName.Text = (!String.IsNullOrEmpty(lstEntity[0].ExpenseTypeName )) ? lstEntity[0].ExpenseTypeName.Trim() : "";
                //chkLocationRequired.Checked = lstEntity[0].IsLocationRequired;
                txtExpenseTypeName.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            String strErr = "";
            if (String.IsNullOrEmpty(txtExpenseTypeName.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtExpenseTypeName.Text))
                    strErr += "<li>" + "Expense Type is required." + "</li>";
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.ExpenseType objEntity = new Entity.ExpenseType();

                if (!String.IsNullOrEmpty(hdnExpenseTypeID.Value))
                    objEntity.pkID  = Convert.ToInt64(hdnExpenseTypeID.Value);

                objEntity.ExpenseTypeName = (!String.IsNullOrEmpty(txtExpenseTypeName.Text)) ? txtExpenseTypeName.Text.Trim() : "";
                objEntity.IsLocationRequired = true;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ExpenseTypeMgmt.AddUpdateExpenseType(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>"; 
                // --------------------------------------------------------------
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteExpenseType(string ExpenseTypeID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ExpenseTypeMgmt.DeleteExpenseType(Convert.ToInt64(ExpenseTypeID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}