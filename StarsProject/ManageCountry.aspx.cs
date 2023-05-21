using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace StarsProject
{
    public partial class ManageCountry : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCurrenciesList();
                // -----------------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnCountryID.Value = Request.QueryString["id"].ToString();

                    if (hdnCountryID.Value == "0" || hdnCountryID.Value == "")
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

        public void GetCurrenciesList()
        {
            foreach (CultureInfo item in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (item.IsNeutralCulture != true)
                {
                    RegionInfo region = new RegionInfo(item.LCID);
                    string CurrencyName = region.CurrencyEnglishName;
                    string ISOCurrenctSymbol = region.ISOCurrencySymbol;
                    string CurrencySymbol = region.CurrencySymbol;
                    ListItem li = new ListItem(CurrencyName + "(" + CurrencySymbol + ")", CurrencySymbol);
                    //** To check whether the Currency has already been added to the list or not ***//
                    if (ddlCurrency.Items.Count > 0)
                    {
                        int i = 0;
                        foreach (ListItem Curr in ddlCurrency.Items)
                        {
                            if (Curr.Value.Trim().ToLower() == li.Value.Trim().ToLower())
                            {
                                i++;
                            }
                        }
                        if (i == 0)
                        {
                            ddlCurrency.Items.Add(li);
                        }
                    }
                    //***********************************************************************//
                    else
                    {
                        ddlCurrency.Items.Add(li);
                    }
                }
            }
            //*************** To sort the dropdownlist items alphabatically *************//
            List<ListItem> listCopy = new List<ListItem>();
            foreach (ListItem item in ddlCurrency.Items)
            {
                listCopy.Add(item);
            }
            ddlCurrency.Items.Clear();
            foreach (ListItem item in listCopy.OrderBy(item => item.Text))
            {
                ddlCurrency.Items.Add(item);
            }
            //**************************************************************************//
            ddlCurrency.Items.Insert(0, "Select");
        }

        public void ClearAllField()
        {
            txtCountryName.Text = "";
        }
        public void OnlyViewControls()
        {
            txtCountryName.ReadOnly = true;
            ddlCurrency.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Country> lstEntity = new List<Entity.Country>();

                lstEntity = BAL.CountryMgmt.GetCountry(hdnCountryID.Value, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnCountryID.Value = lstEntity[0].CountryCode.ToString();
                txtCountryISO.Text = lstEntity[0].CountryISO.ToString();
                txtCountryName.Text = lstEntity[0].CountryName;
                ddlCurrency.SelectedValue = lstEntity[0].CurrencySymbol;

                txtCountryName.Focus();
            }
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
                Entity.Country objEntity = new Entity.Country();

                if (!String.IsNullOrEmpty(hdnCountryID.Value))
                    objEntity.CountryCode = hdnCountryID.Value;
                objEntity.CountryName = txtCountryName.Text;
                objEntity.CurrencyName = ddlCurrency.SelectedItem.Text;
                objEntity.CurrencySymbol = ddlCurrency.SelectedItem.Value;
                objEntity.CountryISO = txtCountryISO.Text; 
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CountryMgmt.AddUpdateCountry(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------

                divErrorMessage.InnerHtml = ReturnMsg;
            }

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteCountry(string CountryCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CountryMgmt.DeleteCountry(CountryCode, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}