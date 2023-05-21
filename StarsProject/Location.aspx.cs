using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace StarsProject
{
    public partial class Location : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkId.Value = Request.QueryString["id"].ToString();
                    if (hdnpkId.Value == "0" || hdnpkId.Value == "")
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
            txtLocationName.Text = "";
        }
        public void OnlyViewControls()
        {
            txtLocationName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Location> lstEntity = new List<Entity.Location>();
                lstEntity = BAL.LocationMgmt.GetLocation(Convert.ToInt64(hdnpkId.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkId.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                txtLocationName.Text = (!String.IsNullOrEmpty(lstEntity[0].LocationName)) ? lstEntity[0].LocationName.Trim() : "";
                txtLocationName.Focus();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            _pageErrMsg = "";
            if (String.IsNullOrEmpty(txtLocationName.Text))
            {
                _pageErrMsg += "<li>" + "Location is required." + "</li>";
                _pageValid = false;
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.Location objEntity = new Entity.Location();
                if (!String.IsNullOrEmpty(hdnpkId.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkId.Value);
                objEntity.LocationName = (!String.IsNullOrEmpty(txtLocationName.Text)) ? txtLocationName.Text.Trim() : "";
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.LocationMgmt.AddUpdateLocation(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                _pageErrMsg += "<li>" + ReturnMsg + "</li>";
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteLocation(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.LocationMgmt.DeleteLocation(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);
            return serializer.Serialize(rows);
        }
    }
}