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
    public partial class TeleCaller : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnSource.Value = Request.QueryString["source"].ToString();
                hdnMode.Value = Request.QueryString["mode"].ToString();
                hdnSerialKey.Value = Session["SerialKey"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {

                        BindDisQualifiedDropDown();
                        
                        List<Entity.Country> lstEvents = new List<Entity.Country>();
                        lstEvents = BAL.CountryMgmt.GetCountryList();
                        drpCountry.DataSource = lstEvents;
                        drpCountry.DataValueField = "CountryCode";
                        drpCountry.DataTextField = "CountryName";
                        drpCountry.DataBind();
                        drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

                        ClearAllField();
                    }
                    else
                    {
                        BindDropDown();
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

        private void ClearAllField()
        {
            txtQueryDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            hdnpkID.Value = "";
            txtLeadID.Text = "";
            txtSenderName.Text = "";

            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtCompanyName.Text = "";

            txtAddress.Text = "";
            txtPinCode.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtContact1.Text = "";

            hdnCountryFlagURL.Value = "";
            hdnCountryISO.Value = "";
            hdnCountryCode.Value = "";

            txtMessage.Text = "";
            drpInquiryStatus.SelectedValue = "---Select Status---";
            divDisQualified.Visible = false;
            divDisQualifiedRemarks.Visible = false;

            divQualified.Visible = false;
            divAssignTo.Visible = false;

            hdnProductID.Value = "";
            txtProductName.Text = "";
            // ------------------------------------------------
            drpCity.Items.Clear();
            drpState.Items.Clear();
            drpCountry.ClearSelection();
            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);
            }

            //if (drpState.Items.FindByText("Gujarat") != null)
            //{
            //    drpState.Items.FindByText("Gujarat").Selected = true;
            //    drpState_SelectedIndexChanged(null, null);
            //}
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            List<Entity.CompanyProfile> lstCSC = new List<Entity.CompanyProfile>();
            lstCSC = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

            drpState.ClearSelection();
            if (drpState.Items.FindByText(lstCSC[0].StateName) != null)
            {
                drpState.Items.FindByText(lstCSC[0].StateName).Selected = true;
                drpState_SelectedIndexChanged(null, null);
            }
            drpCity.ClearSelection();
            if (drpCity.Items.FindByText(lstCSC[0].CityName) != null)
            {
                drpCity.Items.FindByText(lstCSC[0].CityName).Selected = true;
                drpCity_SelectedIndexChanged(null, null);
            }

            hdnInquiryNopkID.Value = "";
            spnInquiryNo.Text = "";

            txtFollowupNotes.Text = "";
            txtFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtPreferredTime.Text = "";

            btnGenerateInquiry.Disabled = false;

            txtQueryDate.Focus();
        }

        public void OnlyViewControls()
        {
            txtQueryDate.ReadOnly = true;
            txtMessage.ReadOnly = true;
            txtSenderName.ReadOnly = true;
            txtCompanyName.ReadOnly = true;
            drpInquiryStatus.Attributes.Add("disabled", "disabled");
            txtEmail.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtContact.ReadOnly = true;
            txtContact1.ReadOnly = true;
            drpAssignTo.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");
            drpCountry.Attributes.Add("disabled", "disabled");
            drpState.Attributes.Add("disabled", "disabled");
            txtProductName.ReadOnly = true;
            txtPinCode.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            divFollowup.Visible = (drpInquiryStatus.SelectedValue.ToLower() == "qualified" && (String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0")) ? true : false;
            drpDisQualifiedReason.Attributes.Add("disabled", "disabled");
            txtDisQualifiedRemarks.ReadOnly = true;

            btnGenerateInquiry.Visible = false;
        }

        public void BindDropDown()
        {
            drpAssignTo.Items.Clear();
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpAssignTo.DataSource = lstEmployee;
            drpAssignTo.DataValueField = "pkID";
            drpAssignTo.DataTextField = "EmployeeName";
            drpAssignTo.DataBind();
            drpAssignTo.Items.Insert(0, new ListItem("-- Select --", ""));

        }

        public void BindRegionEmployeeDropDown()
        {
            Int64 pState = 0, pCity = 0;
            pState = (!String.IsNullOrEmpty(drpState.SelectedValue) && drpState.SelectedValue != "0") ? Convert.ToInt64(drpState.SelectedValue) : 0;
            pCity = (!String.IsNullOrEmpty(drpCity.SelectedValue) && drpCity.SelectedValue != "0") ? Convert.ToInt64(drpCity.SelectedValue) : 0;

            // ----------------------------------------
            
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrgEmployeeByRegion(pState, pCity, Session["LoginUserID"].ToString());
            
            if (lstEmployee.Count > 0)
            {
                drpAssignTo.Items.Clear();
                drpAssignTo.DataSource = lstEmployee;
                drpAssignTo.DataValueField = "pkID";
                drpAssignTo.DataTextField = "EmployeeName";
                drpAssignTo.DataBind();
                drpAssignTo.Items.Insert(0, new ListItem("-- Select --", ""));
            }
            


        }

        public void BindDisQualifiedDropDown()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("DisQualifiedReason");
            drpDisQualifiedReason.DataSource = lstDesig;
            drpDisQualifiedReason.DataValueField = "pkID";
            drpDisQualifiedReason.DataTextField = "InquiryStatusName";
            drpDisQualifiedReason.DataBind();
            drpDisQualifiedReason.Items.Insert(0, new ListItem("-- Select Disqualified Reason --", "0"));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                BindRegionEmployeeDropDown();
                // ---------------------------------------------------
                List<Entity.Country> lstEvents = new List<Entity.Country>();
                lstEvents = BAL.CountryMgmt.GetCountryList();
                drpCountry.DataSource = lstEvents;
                drpCountry.DataValueField = "CountryCode";
                drpCountry.DataTextField = "CountryName";
                drpCountry.DataBind();
                drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ExternalLeads> lstEntity = new List<Entity.ExternalLeads>();
                // ----------------------------------------------------
                lstEntity = BAL.ExternalLeadsMgmt.GetExternalLeadList(Convert.ToInt64(hdnpkID.Value), "", hdnSource.Value, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();

                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtLeadID.Text = lstEntity[0].pkID.ToString();
                txtQueryDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].QueryDatetime.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                if (hdnMode.Value.ToLower() == "edit")
                {
                    txtQueryDate.Enabled = false;
                }
                txtSenderName.Text = lstEntity[0].SenderName.ToString();

                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                txtCompanyName.Text = lstEntity[0].CompanyName.ToString();
                
                txtAddress.Text = lstEntity[0].Address.ToString();
                txtPinCode.Text = lstEntity[0].Pincode.ToString();
                txtEmail.Text = lstEntity[0].SenderMail.ToString();
                txtContact.Text = lstEntity[0].PrimaryMobileNo.ToString();
                txtContact1.Text = lstEntity[0].SecondaryMobileNo.ToString();

                hdnCountryFlagURL.Value = lstEntity[0].CountryFlagURL.ToString();
                hdnCountryISO.Value = lstEntity[0].CountryISO.ToString();
                hdnCountryCode.Value = lstEntity[0].CountryCode.ToString();

                String tmpMessage = "";
                tmpMessage = RemoveHTMLTags(lstEntity[0].Message.ToString());
                txtMessage.Text = tmpMessage;

                drpInquiryStatus.SelectedValue = lstEntity[0].LeadStatus.ToString();
                drpInquiryStatus_SelectedIndexChanged(null, null);

                hdnProductID.Value = lstEntity[0].ProductID.ToString();
                txtProductName.Text = lstEntity[0].ProductName.ToString();

                drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();

                if (lstEntity[0].EmployeeID.ToString() != "" && lstEntity[0].EmployeeID.ToString() != "0")
                    drpAssignTo.SelectedValue = lstEntity[0].EmployeeID.ToString();

                if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                {
                    hdnInquiryNopkID.Value = lstEntity[0].InquiryNopkID.ToString();
                    spnInquiryNo.Text = lstEntity[0].InquiryNo.ToString();
                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "disqualified")
                {
                    drpDisQualifiedReason.SelectedValue = lstEntity[0].ExLeadClosure.ToString();
                    txtDisQualifiedRemarks.Text = lstEntity[0].DisqualifedRemarks.ToString();
                }
                // -----------------------------------------------------------------
                // Resetting Country -> State -> City
                // -----------------------------------------------------------------
                if (!String.IsNullOrEmpty(lstEntity[0].CountryCode.ToString()))
                {
                    drpState.Enabled = true;
                    drpCountry_SelectedIndexChanged(null, null);
                    drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                }

                if (!String.IsNullOrEmpty(lstEntity[0].StateCode.ToString()) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                }

                // -------------------------------------------------------------
                // Disable Controls On Qualified Status
                // -------------------------------------------------------------
                string tmpVal = drpInquiryStatus.SelectedValue.ToLower();

                divQualified.Visible = (tmpVal == "qualified") ? true : false;
                divDisQualified.Visible = (tmpVal == "disqualified") ? true : false;
                divDisQualifiedRemarks.Visible = (tmpVal == "disqualified") ? true : false;

                divAssignTo.Visible = (tmpVal == "qualified" || tmpVal == "inprocess") ? true : false;

                if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                {
                    txtQueryDate.Attributes.Add("disabled", "disabled");
                    txtProductName.Attributes.Add("disabled", "disabled");
                    txtMessage.Attributes.Add("disabled", "disabled");
                    txtSenderName.Attributes.Add("disabled", "disabled");
                    //txtCompanyName.Attributes.Add("disabled", "disabled");
                    txtEmail.Attributes.Add("disabled", "disabled");
                    txtAddress.Attributes.Add("disabled", "disabled");
                    drpCity.Attributes.Add("disabled", "disabled");
                    drpState.Attributes.Add("disabled", "disabled");
                    drpCountry.Attributes.Add("disabled", "disabled");
                    txtContact.Attributes.Add("disabled", "disabled");
                    txtContact1.Attributes.Add("disabled", "disabled");
                    txtPinCode.Attributes.Add("disabled", "disabled");

                    drpInquiryStatus.Attributes.Add("disabled", "disabled");
                    drpAssignTo.Attributes.Add("disabled", "disabled");
                    txtCustomerName.Attributes.Add("disabled", "disabled");
                    txtFollowupNotes.Attributes.Add("disabled", "disabled");
                    txtFollowupDate.Attributes.Add("disabled", "disabled");
                    txtPreferredTime.Attributes.Add("disabled", "disabled");

                    //btnGenerateInquiry.Attributes.Add("disabled", "disabled");
                }
                txtQueryDate.Focus();

            }
        }

        public string RemoveHTMLTags(string HTMLCode)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLCode, "<[^>]*>", "");
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            Int64 ReturnFollowupPKID = 0, ReturnInquiryPkID = 0;
            _pageValid = true;
            string strErr = "";
            if (btnGenerateInquiry.Disabled == true)
            {
                return;
            }
            //btnGenerateInquiry.Disabled = true;
            if (String.IsNullOrEmpty(txtQueryDate.Text))
            {
                strErr += "<li>" + "Query Date Selection is required." + "</li>";
                _pageValid = false;
            }

            if (String.IsNullOrEmpty(txtProductName.Text) || String.IsNullOrEmpty(hdnProductID.Value) || Convert.ToInt64(hdnProductID.Value) == 0)
            {
                strErr += "<li>" + "Select Proper Product Name From List." + "</li>";
                _pageValid = false;
            }

            if (String.IsNullOrEmpty(txtMessage.Text))
            {
                strErr += "<li>" + "Detail Description is required." + "</li>";
                _pageValid = false;
            }

            if (String.IsNullOrEmpty(txtSenderName.Text.Trim()))
            {
                strErr += "<li>" + "Contact Person is required." + "</li>";
                _pageValid = false;
            }

            if (String.IsNullOrEmpty(txtCompanyName.Text.Trim()))
            {
                strErr += "<li>" + "Select Proper Company Name From List." + "</li>";
                _pageValid = false;
            }

            if (String.IsNullOrEmpty(txtContact.Text.Trim()))
            {
                strErr += "<li>" + "Primary Contact is required." + "</li>";
                _pageValid = false;
            }

            if ((String.IsNullOrEmpty(drpCountry.SelectedValue) || drpCountry.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
            {
                strErr += "<li>" + "Country Selection is required." + "</li>";
                _pageValid = false;
            }

            if ((String.IsNullOrEmpty(drpState.SelectedValue) || drpState.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
            {
                strErr += "<li>" + "State Selection is required." + "</li>";
                _pageValid = false;
            }

            if ((String.IsNullOrEmpty(drpCity.SelectedValue) || drpCity.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
            {
                strErr += "<li>" + "City Selection is required." + "</li>";
                _pageValid = false;
            }

            if (drpInquiryStatus.SelectedValue.ToLower() == "disqualified")
            {
                if (String.IsNullOrEmpty(drpDisQualifiedReason.SelectedValue) || drpDisQualifiedReason.SelectedValue == "0")
                {
                    strErr += "<li>" + "DisQualified Reason is required." + "</li>";
                    _pageValid = false;
                }
            }
            if (drpInquiryStatus.SelectedValue.ToLower() == "inprocess")
            {
                if (String.IsNullOrEmpty(drpAssignTo.SelectedValue))
                {
                    strErr += "<li>" + "Assign To : Employee Selection is required." + "</li>";
                    _pageValid = false;
                }
            }
            if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
            {
                if (String.IsNullOrEmpty(drpAssignTo.SelectedValue))
                {
                    strErr += "<li>" + "Assign To : Employee Selection is required." + "</li>";
                    _pageValid = false;
                }

                if (!String.IsNullOrEmpty(txtFollowupNotes.Text) && !String.IsNullOrEmpty(txtFollowupDate.Text))
                {
                    if (String.IsNullOrEmpty(txtFollowupNotes.Text))
                    {
                        strErr += "<li>" + "FollowUp Notes is required." + "</li>";
                        _pageValid = false;
                    }

                    if (String.IsNullOrEmpty(txtFollowupDate.Text))
                    {
                        strErr += "<li>" + "FollowUp Date Selection is required." + "</li>";
                        _pageValid = false;
                    }
                }

                if (!String.IsNullOrEmpty(txtFollowupNotes.Text) && !String.IsNullOrEmpty(txtFollowupDate.Text))
                {
                    if (!String.IsNullOrEmpty(txtQueryDate.Text) && (hdnpkID.Value == "" || hdnpkID.Value == "0"))
                    {
                        if (Convert.ToDateTime(txtFollowupDate.Text) < Convert.ToDateTime(txtQueryDate.Text))
                        {
                            _pageValid = false;
                            strErr += "<li>" + "Next FollowUp Date should be greater than Query Date." + "</li>";
                        }
                    }
                }
            }
            // ------------------------------------------------------------
            if (_pageValid)
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.LeadID = (hdnSource.Value.ToLower() != "indiamart") ? txtLeadID.Text : hdnpkID.Value;
                objEntity.QueryDatetime = Convert.ToDateTime(txtQueryDate.Text);
                objEntity.SenderName = txtSenderName.Text;
                objEntity.CompanyName = txtCompanyName.Text;
                objEntity.CountryFlagURL = hdnCountryFlagURL.Value;
                objEntity.CountryISO = hdnCountryISO.Value;
                objEntity.Address = txtAddress.Text;
                objEntity.ProductID = Convert.ToInt64(hdnProductID.Value) ;
                objEntity.ForProduct = txtProductName.Text;
                objEntity.City = !String.IsNullOrEmpty(drpCity.SelectedValue) ? drpCity.SelectedItem.Text : "";
                objEntity.State = !String.IsNullOrEmpty(drpState.SelectedValue) ? drpState.SelectedItem.Text : "";
                objEntity.SenderMail = txtEmail.Text;
                objEntity.PrimaryMobileNo = txtContact.Text;
                objEntity.SecondaryMobileNo = txtContact1.Text;
                objEntity.Pincode = txtPinCode.Text.Trim();
                objEntity.StateCode = !String.IsNullOrEmpty(drpState.SelectedValue) ? Convert.ToInt64(drpState.SelectedValue) : 0;
                objEntity.CityCode = !String.IsNullOrEmpty(drpCity.SelectedValue) ? Convert.ToInt64(drpCity.SelectedValue) : 0;
                objEntity.CountryCode = Convert.ToString(drpCountry.SelectedValue);
                objEntity.CustomerID = (!String.IsNullOrEmpty(txtCompanyName.Text) && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0") ? Convert.ToInt64(hdnCustomerID.Value) : 0;

                objEntity.LeadSource = "TeleCaller";

                objEntity.Message = txtMessage.Text;
                objEntity.LeadStatus = drpInquiryStatus.SelectedValue.ToString();
                if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                {
                    objEntity.EmployeeID = Convert.ToInt64(drpAssignTo.SelectedValue);
                    objEntity.FollowupNotes = (!String.IsNullOrEmpty(txtFollowupNotes.Text)) ? txtFollowupNotes.Text : "";
                    objEntity.FollowupDate = (!String.IsNullOrEmpty(txtFollowupDate.Text)) ? Convert.ToDateTime(txtFollowupDate.Text) : SqlDateTime.MinValue.Value;
                    objEntity.PreferredTime = txtPreferredTime.Text;
                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "inprocess")
                {
                    if (!String.IsNullOrEmpty(drpAssignTo.SelectedValue) && drpAssignTo.SelectedValue != "0")
                        objEntity.EmployeeID = Convert.ToInt64(drpAssignTo.SelectedValue);
                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "disqualified")
                {
                    objEntity.ExLeadClosure = Convert.ToInt64(drpDisQualifiedReason.SelectedValue);
                    objEntity.DisqualifedRemarks = txtDisQualifiedRemarks.Text;
                }
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ExternalLeadsMgmt.AddUpdateExternalLeads(objEntity, out ReturnCode, out ReturnMsg,out ReturnInquiryPkID, out ReturnFollowupPKID);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnGenerateInquiry.Attributes.Add("disabled", "disabled");
                }
                //btnGenerateInquiry.Disabled = true;
            }
            else
            {
                btnGenerateInquiry.Disabled = false;
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

        protected void btnGenerateInquiry_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }
        
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }


        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpState.Items.Clear();
            List<Entity.State> lstEvents = new List<Entity.State>();
            if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
            {
                lstEvents = BAL.StateMgmt.GetStateList(Convert.ToString(drpCountry.SelectedValue));
            }
            else
            {
                lstEvents = BAL.StateMgmt.GetStateList();
            }

            drpState.DataSource = lstEvents;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new ListItem("-- All State --", ""));
            drpState.Focus();
            if (drpCountry.SelectedValue == "0" || drpCountry.SelectedValue == "")
            {
                drpState.Items.Clear();
                drpCity.Items.Clear();
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
                    drpCity.Items.Insert(0, new ListItem("-- All City --", ""));
                    drpCity.Enabled = true;
                    drpCity.Focus();
                }

            }
            if (drpState.SelectedValue == "0" || drpState.SelectedValue == "")
            {
                drpCity.Items.Clear();
            }
            // -----------------------------------
            BindRegionEmployeeDropDown();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteExternalLeads(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ExternalLeadsMgmt.DeleteExternalLeads(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpInquiryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpVal = drpInquiryStatus.SelectedValue.ToLower();

            divQualified.Visible = (tmpVal == "qualified") ? true : false;
            divDisQualified.Visible = (tmpVal == "disqualified") ? true : false;
            divDisQualifiedRemarks.Visible = (tmpVal == "disqualified") ? true : false;

            divAssignTo.Visible = (tmpVal == "qualified" || tmpVal == "inprocess") ? true : false; 

            if (tmpVal == "qualified")
            {
                BindRegionEmployeeDropDown();
                drpAssignTo.Focus();
            }
            else if (tmpVal == "disqualified")
            {
                BindDisQualifiedDropDown();
                drpDisQualifiedReason.Focus();
            }
        }

        protected void drpCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRegionEmployeeDropDown();
            txtPinCode.Focus();
        }

        protected void txtCompanyName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && String.IsNullOrEmpty(txtCustomerName.Text))       //For Order generation from inquiry no - dashboard
                    txtCustomerName.Text = (lstEntity.Count > 0) ? lstEntity[0].CustomerName : "";
                // -------------------------------------
                setLayout("edit");
                // -------------------------------------
                if (lstEntity.Count>0 && (hdnpkID.Value=="" || hdnpkID.Value == "0"))
                {
                    txtAddress.Text = lstEntity[0].Address.ToString();
                    txtContact.Text = lstEntity[0].ContactNo1.ToString();
                    txtContact1.Text = lstEntity[0].ContactNo2.ToString();

                    drpCity.Items.Clear();
                    drpState.Items.Clear();
                    drpCountry.Items.Clear();
                    // -----------------------------------------------------
                    List<Entity.Country> lstEvents = new List<Entity.Country>();
                    lstEvents = BAL.CountryMgmt.GetCountryList();
                    drpCountry.DataSource = lstEvents;
                    drpCountry.DataValueField = "CountryCode";
                    drpCountry.DataTextField = "CountryName";
                    drpCountry.DataBind();
                    drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));
                    // -----------------------------------------------------
                    if (!String.IsNullOrEmpty(lstEntity[0].CountryCode.ToString()) && lstEntity[0].CountryCode.ToString() != "0")
                        drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();

                    if (!String.IsNullOrEmpty(lstEntity[0].CountryCode))
                    {
                        drpState.Enabled = true;
                        drpCountry_SelectedIndexChanged(null, null);
                        if (!String.IsNullOrEmpty(lstEntity[0].StateCode.ToString()) && lstEntity[0].StateCode.ToString() != "0")
                            drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                    }

                    if (!String.IsNullOrEmpty(lstEntity[0].StateCode) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                    {
                        drpCity.Enabled = true;
                        drpState_SelectedIndexChanged(null, null);
                        if (!String.IsNullOrEmpty(lstEntity[0].CityCode.ToString()) && lstEntity[0].CityCode.ToString() != "0")
                            drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                    }
                }
                txtEmail.Focus();
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }

        }
    }
}