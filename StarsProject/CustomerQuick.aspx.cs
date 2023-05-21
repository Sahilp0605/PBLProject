using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlTypes;

namespace StarsProject
{
    public partial class CustomerQuick : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        //private DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            string pageMode = (!String.IsNullOrEmpty(Request.QueryString["mode"])) ? Request.QueryString["mode"] : "";
            hdnPageMode.Value = pageMode;
            // -----------------------------------------
            if (!IsPostBack)
            {
                DataTable dtCustomer = new DataTable();
                Session.Add("dtCustomer", dtCustomer);
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 10;
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnCustomerID.Value = Request.QueryString["id"].ToString();

                    if (hdnCustomerID.Value == "0" || hdnCustomerID.Value == "")
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

        public void OnlyViewControls()
        {
            txtCustomerName.ReadOnly = true;
            drpCustomerType.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");
            txtAddress.ReadOnly = true;
            txtArea.ReadOnly = true;
            txtPincode.ReadOnly = true;
            txtContactNo1.ReadOnly = true;
            txtContactNo2.ReadOnly = true;
            txtEmailAddress.ReadOnly = true;
            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;
            drpCustomerSource.Attributes.Add("disabled", "disabled");
        }

        public void BindDropDown()
        {
            drpCountry.ClearSelection();
            List<Entity.Country> lstCountry = new List<Entity.Country>();
            lstCountry = BAL.CountryMgmt.GetCountryList();
            drpCountry.DataSource = lstCountry;
            drpCountry.DataValueField = "CountryCode";
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataBind();
            drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));
            // ---------------- Customer Category List -------------------------------------
            List<Entity.CustomerCategory> lstCustCat = new List<Entity.CustomerCategory>();
            lstCustCat = BAL.CustomerCategoryMgmt.GetCustomerCategoryList();
            drpCustomerType.DataSource = lstCustCat;
            drpCustomerType.DataValueField = "CategoryName";
            drpCustomerType.DataTextField = "CategoryName";
            drpCustomerType.DataBind();
            drpCustomerType.Items.Insert(0, new ListItem("-- All Category --", ""));

            // ---------------- Customer Source  -------------------------------------
            List<Entity.InquiryStatus> lstSource = new List<Entity.InquiryStatus>();
            lstSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquirySource");
            drpCustomerSource.DataSource = lstSource;
            drpCustomerSource.DataValueField = "pkID";
            drpCustomerSource.DataTextField = "InquiryStatusName";
            drpCustomerSource.DataBind();
            drpCustomerSource.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpCustomerType.SelectedValue = lstEntity[0].CustomerType;
                txtAddress.Text = lstEntity[0].Address;
                txtArea.Text = lstEntity[0].Area;
                txtPincode.Text = lstEntity[0].Pincode;
                txtContactNo1.Text = lstEntity[0].ContactNo1;
                txtContactNo2.Text = lstEntity[0].ContactNo2;
                txtEmailAddress.Text = lstEntity[0].EmailAddress;
                drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();
                drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                drpCustomerSource.SelectedValue = lstEntity[0].CustomerSourceID.ToString();

                if (!String.IsNullOrEmpty(lstEntity[0].CountryCode))
                {
                    drpState.Enabled = true;
                    drpCountry_SelectedIndexChanged(null, null);
                    drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                }

                if (!String.IsNullOrEmpty(lstEntity[0].StateCode) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                }
                txtCustomerName.Focus();
            }
            else if (pMode == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------- Delete Record
                BAL.CustomerMgmt.DeleteCustomer(Convert.ToInt64(hdnCustomerID.Value), out ReturnCode, out ReturnMsg);
                if (ReturnCode == 0)
                {
                    string title = "Delete Action Failed";
                    string body = ReturnMsg;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "ErrorPopup", "ShowErrorPopup('" + title + "', '" + body + "');", true);
                }

            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtCustomer");
        }


        public void ClearAllField()
        {
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpCustomerType.SelectedValue = "";
            txtAddress.Text = "";
            txtArea.Text = "";
            // ------------------------------------------------
            drpCity.Items.Clear();
            drpState.Items.Clear();

            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);
            }
            txtPincode.Text = "";
            txtContactNo1.Text = "";
            txtContactNo2.Text = "";
            txtEmailAddress.Text = "";
            // ------------------------------------------------------------
            drpCustomerSource.SelectedValue = "";
            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
            txtCustomerName.Focus();
        }

        [WebMethod]
        public static string DeleteCustomer(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CustomerMgmt.DeleteCustomer(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [WebMethod]
        public static List<Entity.State> FillState(string pCountryCode)
        {
            List<Entity.State> lstState = new List<Entity.State>();
            lstState = BAL.StateMgmt.GetStateList(pCountryCode);
            return lstState;
        }

        [WebMethod]
        public static List<Entity.City> FillCity(string pStateCode)
        {
            List<Entity.City> lstCity = new List<Entity.City>();
            lstCity = BAL.CityMgmt.GetCityByState(Convert.ToInt64(pStateCode));
            return lstCity;
        }


        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            String strErr = "";

            int ReturnCode = 0;
            string ReturnMsg = "";

            _pageValid = true;

            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(txtContactNo1.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer/Company Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtContactNo1.Text))
                    strErr += "<li>" + "Contact #1 is required." + "</li>";

                //if (String.IsNullOrEmpty(drpCountry.SelectedValue))
                //    strErr += "<li>" + "Country Selection is required." + "</li>";
            }
            // ----------------------------------------------------------------
            Entity.Customer objEntity = new Entity.Customer();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);

                objEntity.CustomerName = txtCustomerName.Text;
                objEntity.CustomerType = drpCustomerType.SelectedValue.Trim();

                objEntity.Address = txtAddress.Text;
                objEntity.Area = txtArea.Text;
                objEntity.CountryCode = drpCountry.SelectedValue;
                objEntity.StateCode = drpState.SelectedValue;
                objEntity.CityCode = drpCity.SelectedValue;
                objEntity.Pincode = txtPincode.Text;

                objEntity.ContactNo1 = txtContactNo1.Text;
                objEntity.ContactNo2 = txtContactNo2.Text;
                objEntity.EmailAddress = txtEmailAddress.Text;

                objEntity.CustomerSourceID = (!String.IsNullOrEmpty(drpCustomerSource.SelectedValue)) ? Convert.ToInt64(drpCustomerSource.SelectedValue) : 0;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CustomerMgmt.AddUpdateCustomerQuick(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    btnSaveEmail.Disabled = true;
                }
                // --------------------------------------------------------------
                if (paraSaveAndEmail)
                {
                    Entity.Authenticate objAuth = new Entity.Authenticate();
                    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                    String sendEmailFlag = BAL.CommonMgmt.GetConstant("COMPANYPROFILE", 0, objAuth.CompanyID).ToLower();
                    if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(txtEmailAddress.Text) && txtEmailAddress.Text.ToUpper() != "NULL")
                            {
                                String respVal = "";
                                respVal = BAL.CommonMgmt.SendEmailNotifcation("COMPANYPROFILE", Session["LoginUserID"].ToString(), 0, txtEmailAddress.Text);
                            }
                            strErr += "<li>" + @ReturnMsg + " and Email Sent Successfully !" + "</li>";
                        }
                        catch (Exception ex)
                        {
                            strErr += "<li>" + @ReturnMsg + " and Sending Email Failed !" + "</li>";
                        }

                    }
                }
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
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
                    drpState.Items.Insert(0, new ListItem("-- All State --", "0"));
                    //drpState.Enabled = true;
                }

            }
        }
        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpState.SelectedValue))
            {
                if (Convert.ToInt64(drpState.SelectedValue) > 0)
                {
                    List<Entity.City> lstEvents = new List<Entity.City>();
                    lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpState.SelectedValue));
                    drpCity.DataSource = lstEvents;
                    drpCity.DataValueField = "CityCode";
                    drpCity.DataTextField = "CityName";
                    drpCity.DataBind();
                    drpCity.Items.Insert(0, new ListItem("-- All City --", "0"));
                    //drpCity.Enabled = true;
                }

            }
        }
    }
}