using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Document_Type : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                //BindDropDown();
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
                List<Entity.Document_Type> lstEntity = new List<Entity.Document_Type>();
                // ----------------------------------------------------
                //lstEntity.LoginUserID = Session["LoginUserID"].ToString();

                lstEntity = BAL.Document_TypeMgmt.GetDocument_Type(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtDocumentType.Text = lstEntity[0].DocumentType;
                txtDocumentType.Focus();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtDocumentType.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtDocumentType.Text))
                {
                    strErr += "<li>" + "Document Type is mandatory !" + "</li>";
                }

            }
            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.Document_Type objEntity = new Entity.Document_Type();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.DocumentType = txtDocumentType.Text;
                //objEntity.FromDate = Convert.ToDateTime(txtFromDate.Text);
                //objEntity.ToDate = Convert.ToDateTime((Convert.ToDateTime(txtToDate.Text)).ToString("yyyy-MM-dd 23:59:59"));
                //if (!String.IsNullOrEmpty(txtDocumentType.Text) && !String.IsNullOrEmpty(txtDocumentType.Text))
                //    objEntity.DocumentType = Convert.ToDateTime(Convert.ToDateTime(txtDocumentType.Text + " " + txtDocumentType.Text));



                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.Document_TypeMgmt.AddUpdateDocument_Type(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                }
            }
            if (ReturnCode > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);

        }
        public void OnlyViewControls()
        {
            txtDocumentType.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            txtDocumentType.Text = "";

            txtDocumentType.Focus();
            btnSave.Disabled = false;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteDocument_Type(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.Document_TypeMgmt.DeleteDocument_Type(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}