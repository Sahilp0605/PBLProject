using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Color : System.Web.UI.Page
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

            if (String.IsNullOrEmpty(txtColorName.Text))
            {
                _pageErrMsg += "<li>" + "Color Name is required." + "</li>";
                _pageValid = false;


            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.Color objEntity = new Entity.Color();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.ColorName = (!String.IsNullOrEmpty(txtColorName.Text)) ? txtColorName.Text.Trim() : "";

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ColorMgmt.AddUpdateColor(objEntity, out ReturnCode, out ReturnMsg);
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
            txtColorName.Text = "";
        }
        public void OnlyViewControls()
        {
            txtColorName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Color> lstEntity = new List<Entity.Color>();

                lstEntity = BAL.ColorMgmt.GetColor(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                txtColorName.Text = (!String.IsNullOrEmpty(lstEntity[0].ColorName)) ? lstEntity[0].ColorName.Trim() : "";

                txtColorName.Focus();
            }
        }
        [System.Web.Services.WebMethod]
        public static string DeleteColor(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ColorMgmt.DeleteColor(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}
