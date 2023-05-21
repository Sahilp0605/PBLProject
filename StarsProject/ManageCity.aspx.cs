using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace StarsProject
{
    public partial class ManageCity : System.Web.UI.Page
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
                    hdnCityID.Value = Request.QueryString["id"].ToString();

                    if (hdnCityID.Value == "0" || hdnCityID.Value == "")
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
            txtCityName.Text = "";
            drpState.SelectedValue = "";

            txtNewCountry.Text = "";
            txtNewCountryCode.Text = "";
            txtNewCountryISO.Text = "";
            txtNewState.Text = "";
        }

        public void OnlyViewControls()
        {
            txtCityName.ReadOnly = true; 
            drpState.Attributes.Add("disabled", "disabled");

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
              
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.City> lstEntity = new List<Entity.City>();

                lstEntity = BAL.CityMgmt.GetCity(Convert.ToInt64(hdnCityID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnCityID.Value = lstEntity[0].CityCode.ToString();
                txtCityName.Text = lstEntity[0].CityName;
                drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();
                // -----------------------------------------------
                if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
                {
                    if (!string.IsNullOrEmpty(drpCountry.SelectedValue))
                    {
                        List<Entity.State> lstEvents = new List<Entity.State>();
                        lstEvents = BAL.StateMgmt.GetStateList((drpCountry.SelectedValue).ToString());
                        drpState.DataSource = lstEvents;
                        drpState.DataValueField = "StateCode";
                        drpState.DataTextField = "StateName";
                        drpState.DataBind();
                        drpState.Items.Insert(0, new ListItem("-- All State --", ""));
                        drpState.Enabled = true;
                    }
                    else
                    {
                        List<Entity.State> lstEvents = new List<Entity.State>();
                        drpState.DataSource = lstEvents;
                        drpState.DataValueField = "StateCode";
                        drpState.DataTextField = "StateName";
                        drpState.DataBind();
                    }
                }
                // ----------------------------------------------------------
                drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                txtCityName.Focus();
            }
        }

        public void BindDropDown()
        {
            // ---------------- Country List -------------------------------------
            List<Entity.Country> lstCountry = new List<Entity.Country>();
            lstCountry = BAL.CountryMgmt.GetCountryList();
            drpCountry.DataSource = lstCountry;
            drpCountry.DataValueField = "CountryCode";
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataBind();
            drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

            drpStateCountry.DataSource = lstCountry;
            drpStateCountry.DataValueField = "CountryCode";
            drpStateCountry.DataTextField = "CountryName";
            drpStateCountry.DataBind();

            //// ---------------- City List -------------------------------------
            //List<Entity.State> lstEvents = new List<Entity.State>();
            //lstEvents = BAL.StateMgmt.GetStateList();
            //drpState.DataSource = lstEvents;
            //drpState.DataValueField = "StateCode";
            //drpState.DataTextField = "StateName";
            //drpState.DataBind();
            //drpState.Items.Insert(0, new ListItem("-- All State --", ""));
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
            {
                if (!string.IsNullOrEmpty(drpCountry.SelectedValue))
                {
                    List<Entity.State> lstEvents = new List<Entity.State>();
                    lstEvents = BAL.StateMgmt.GetStateList((drpCountry.SelectedValue).ToString());
                    drpState.DataSource = lstEvents;
                    drpState.DataValueField = "StateCode";
                    drpState.DataTextField = "StateName";
                    drpState.DataBind();
                    drpState.Items.Insert(0, new ListItem("-- All State --", ""));
                    drpState.Enabled = true;
                }

            }
            drpState.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            String errMsg = "";

            if ((String.IsNullOrEmpty(txtCityName.Text) || String.IsNullOrEmpty(drpState.SelectedValue)))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCityName.Text))
                {
                    errMsg += "<li>" + "City Name is required." + "</li>";
                }

                if (String.IsNullOrEmpty(drpState.SelectedValue))
                {
                    errMsg += "<li>" + "State Selection is required." + "</li>";
                }
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------------------------------------
                Entity.City objEntity = new Entity.City();

                if (!String.IsNullOrEmpty(hdnCityID.Value))
                    objEntity.CityCode = Convert.ToInt64(hdnCityID.Value);

                objEntity.CityName  = txtCityName.Text;
                objEntity.StateCode = Convert.ToInt64(drpState.SelectedValue);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CityMgmt.AddUpdateCity(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                errMsg += "<li>" + ReturnMsg + "</li>";
            }
            // ------------------------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + errMsg + "');", true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        protected void btnAddCountry_Click(object sender, EventArgs e)
        {
            string mymessage = "";
            int ReturnCode;
            string ReturnMsg;
            if (!String.IsNullOrEmpty(txtNewCountryCode.Text) && !String.IsNullOrEmpty(txtNewCountryISO.Text) && !String.IsNullOrEmpty(txtNewCountry.Text))
            {
                Entity.Country lstNewCountry = new Entity.Country();
                lstNewCountry.CountryCode = txtNewCountryCode.Text;
                lstNewCountry.CountryISO = txtNewCountryISO.Text;
                lstNewCountry.CountryName = txtNewCountry.Text;
                lstNewCountry.LoginUserID = Session["LoginUserID"].ToString();
                BAL.CountryMgmt.AddUpdateCountry(lstNewCountry, out ReturnCode, out ReturnMsg);
                mymessage = "<li>" + ReturnMsg + "</li>";
            }
            // ------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + mymessage + "');", true);
            // ------------------------------
            BindDropDown();


        }

        protected void btnAddState_Click(object sender, EventArgs e)
        {
            string mymessage = "";
            int ReturnCode;
            string ReturnMsg;

            if (!String.IsNullOrEmpty(drpStateCountry.SelectedValue) && !String.IsNullOrEmpty(txtNewState.Text) && !String.IsNullOrEmpty(txtStateCode.Text))
            {
                Entity.State lstNewState = new Entity.State();
                lstNewState.CountryCode = drpStateCountry.SelectedValue;
                lstNewState.StateCode = 0;
                lstNewState.StateName = txtNewState.Text;
                lstNewState.GSTStateCode = Convert.ToInt64(txtStateCode.Text);
                lstNewState.LoginUserID = Session["LoginUserID"].ToString();
                BAL.StateMgmt.AddUpdateState(lstNewState, out ReturnCode, out ReturnMsg);
                mymessage = "<li>" + ReturnMsg + "</li>";
                // ------------------------------------------
                drpCountry.SelectedValue = drpStateCountry.SelectedValue.ToString();
            }
            // ------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + mymessage + "');", true);
        }

        [System.Web.Services.WebMethod]
        public static string DeleteCity(string CityCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CityMgmt.DeleteCity(Convert.ToInt64(CityCode), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [System.Web.Services.WebMethod]
        public static string FilterCity(string CityName, Int64 StateCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CityMgmt.GetCityListForDropdown(CityName, StateCode);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterState(string StateName, Int64 CountryCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CityMgmt.GetStateListForDropdown(StateName, CountryCode);
            return serializer.Serialize(rows);
        }

        protected void txtNewState_TextChanged(object sender, EventArgs e)
        {
            String errMsg = "";
            if (System.Text.RegularExpressions.Regex.IsMatch(txtStateCode.Text, "[^0-9]"))
            {
                //MessageBox.Show("Please enter only numbers.");
                txtStateCode.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "Please enter only numbers in State Code." + "');", true);

            }
        }
    }
}