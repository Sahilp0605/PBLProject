using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ManageState : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnStateID.Value = Request.QueryString["id"].ToString();

                    if (hdnStateID.Value == "0" || hdnStateID.Value == "")
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
            txtStateName.Text = "";
            drpCountry.SelectedValue = "";
            txtGSTStateCode.Text = "";
        }
        public void OnlyViewControls()
        {
            txtStateName.ReadOnly = true;
            txtGSTStateCode.ReadOnly = true;
            drpCountry.Attributes.Add("disabled", "disabled");

            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.State> lstEntity = new List<Entity.State>();
                lstEntity = BAL.StateMgmt.GetState(Convert.ToInt64(hdnStateID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnStateID.Value = lstEntity[0].StateCode.ToString();
                txtStateName.Text = lstEntity[0].StateName;
                drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();
                txtGSTStateCode.Text = lstEntity[0].GSTStateCode.ToString();
                txtStateName.Focus();
            }
        }
        public void BindDropDown()
        {
            // ---------------- City List -------------------------------------
            List<Entity.Country> lstEvents = new List<Entity.Country>();
            lstEvents = BAL.CountryMgmt.GetCountryList();
            drpCountry.DataSource = lstEvents;
            drpCountry.DataValueField = "CountryCode";
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataBind();
            drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            divErrorMessage.InnerHtml = "";


            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.State objEntity = new Entity.State();

                if (!String.IsNullOrEmpty(hdnStateID.Value))
                    objEntity.StateCode = Convert.ToInt64(hdnStateID.Value);

                objEntity.StateName = txtStateName.Text;
                objEntity.CountryCode  = drpCountry.SelectedValue;
                objEntity.GSTStateCode = (!String.IsNullOrEmpty(txtGSTStateCode.Text)) ? Convert.ToInt64(txtGSTStateCode.Text) : 0;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.StateMgmt.AddUpdateState(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------

                divErrorMessage.InnerHtml = ReturnMsg;

            }

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteState(string StateCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.StateMgmt.DeleteState(Convert.ToInt64(StateCode), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}