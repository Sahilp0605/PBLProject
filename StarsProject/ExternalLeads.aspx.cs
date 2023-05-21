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
    public partial class ExternalLeads : System.Web.UI.Page
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
            txtQueryDate.ReadOnly = true;
            txtLeadSource.ReadOnly = true;
            txtForProduct.ReadOnly = true;
            txtMessage.ReadOnly = true;
            //txtSenderName.ReadOnly = true;
            //txtCompanyName.ReadOnly = true;
            drpInquiryStatus.Attributes.Add("disabled", "disabled");
            //txtEmail.ReadOnly = true;
            //txtAddress.ReadOnly = true;
            //txtCity.ReadOnly = true;
            //txtState.ReadOnly = true;
            //txtCountry.ReadOnly = true;
            //txtContact.ReadOnly = true;
            //txtContact1.ReadOnly = true;
            drpAssignTo.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");
            txtProductName.ReadOnly = true;
            txtPinCode.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpDisQualifiedReason.Attributes.Add("disabled", "disabled");

            btnGenerateInquiry.Visible = false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        public void ClearAllField()
        {
            //drpInquiryStatus.SelectedValue = "";
            txtQueryDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            hdnCustomerID.Value = "";
            hdnProductID.Value = "";
            txtProductName.Text = "";
            drpAssignTo.SelectedValue = "";
            drpCity.SelectedValue = "";
            drpState.SelectedValue = "";
            txtPinCode.Text = "";
            txtCustomerName.Text = "";
            txtFollowupNotes.Text = "";
            txtFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtPreferredTime.Text = "";
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

        public void BindDisQualifiedDropDown()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("DisQualifiedReason");
            drpDisQualifiedReason.DataSource = lstDesig;
            drpDisQualifiedReason.DataValueField = "pkID";
            drpDisQualifiedReason.DataTextField = "InquiryStatusName";
            drpDisQualifiedReason.DataBind();
            drpDisQualifiedReason.Items.Insert(0, new ListItem("-- Select Disqualified Reason --", ""));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                //// ---------------- State List -------------------------------------
                //List<Entity.State> lstEvents = new List<Entity.State>();
                //lstEvents = BAL.StateMgmt.GetStateList();
                //drpState.DataSource = lstEvents;
                //drpState.DataValueField = "StateCode";
                //drpState.DataTextField = "StateName";
                //drpState.DataBind();
                //drpState.Items.Insert(0, new ListItem("-- All State --", ""));

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
                txtLeadID.Text = lstEntity[0].LeadID.ToString();
                txtQueryDate.Text = lstEntity[0].QueryDatetime.ToString("yyyy-MM-dd");
                txtSenderName.Text = lstEntity[0].SenderName.ToString();
                txtCompanyName.Text = lstEntity[0].CompanyName.ToString();
                txtAddress.Text = lstEntity[0].Address.ToString();
                txtCity.Text = lstEntity[0].City.ToString();
                txtPinCode.Text = lstEntity[0].Pincode.ToString();
                txtState.Text = lstEntity[0].State.ToString();
                txtCountry.Text = lstEntity[0].CountryName.ToString();
                txtEmail.Text = lstEntity[0].SenderMail.ToString();
                txtContact.Text = lstEntity[0].PrimaryMobileNo.ToString();
                txtContact1.Text = lstEntity[0].SecondaryMobileNo.ToString();
                txtLeadSource.Text = lstEntity[0].LeadSource.ToString();
                txtForProduct.Text = lstEntity[0].ForProduct.ToString();
                hdnCountryFlagURL.Value = lstEntity[0].CountryFlagURL.ToString();
                hdnCountryISO.Value = lstEntity[0].CountryISO.ToString();
                hdnCountryCode.Value = lstEntity[0].CountryCode.ToString();

                String tmpMessage = "";
                tmpMessage = RemoveHTMLTags(lstEntity[0].Message.ToString());
                txtMessage.Text = tmpMessage;

                drpInquiryStatus.SelectedValue = lstEntity[0].LeadStatus.ToString();
                String currStatus = lstEntity[0].LeadStatus.ToString().ToLower();
                divQualified.Visible = (currStatus == "qualified") ? true : false;
                divDisQualified.Visible = (currStatus == "disqualified") ? true : false;
                divAssignTo.Visible = (currStatus == "qualified" || currStatus == "inprocess") ? true : false;
                divFollowup.Visible = (currStatus == "qualified") ? true : false;
                drpInquiryStatus_SelectedIndexChanged(null, null);

                if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                {
                    hdnInquiryNopkID.Value = lstEntity[0].InquiryNopkID.ToString();
                    spnInquiryNo.Text = "Inquiry # : " + lstEntity[0].InquiryNo.ToString();

                    drpAssignTo.SelectedValue = lstEntity[0].EmployeeID.ToString();
                    hdnProductID.Value = lstEntity[0].ProductID.ToString();
                    txtProductName.Text = lstEntity[0].ProductName.ToString();
                    hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                    txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                    drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();
                    drpState.SelectedValue = lstEntity[0].StateCode.ToString();

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

                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "inprocess")
                {
                    if (!String.IsNullOrEmpty(lstEntity[0].EmployeeID.ToString()) && lstEntity[0].EmployeeID.ToString()!="0")
                        drpAssignTo.SelectedValue = lstEntity[0].EmployeeID.ToString();
                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "disqualified")
                {
                    drpDisQualifiedReason.SelectedValue = lstEntity[0].ExLeadClosure.ToString();

                    btnGenerateInquiry.Enabled = false;
                }

                txtQueryDate.Attributes.Add("disabled", "disabled");
                //txtSenderName.Attributes.Add("disabled", "disabled");
                //txtCompanyName.Attributes.Add("disabled", "disabled");
                //txtEmail.Attributes.Add("disabled", "disabled");
                //txtAddress.Attributes.Add("disabled", "disabled");
                //txtCity.Attributes.Add("disabled", "disabled");
                //txtState.Attributes.Add("disabled", "disabled");
                //txtCountry.Attributes.Add("disabled", "disabled");
                //txtContact.Attributes.Add("disabled", "disabled");
                //txtContact1.Attributes.Add("disabled", "disabled");

                if(hdnSerialKey.Value== "HONP-MEDF-9RTS-FG10")
                {
                    // -------------------------------------------------------------
                    // Disable Controls On Qualified Status
                    // -------------------------------------------------------------
                    string tmpVal = drpInquiryStatus.SelectedValue.ToLower();

                    divQualified.Visible = (tmpVal == "qualified") ? true : false;
                    divDisQualified.Visible = (tmpVal == "disqualified") ? true : false;
                    //divDisQualifiedRemarks.Visible = (tmpVal == "disqualified") ? true : false;

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
                }
                else
                {
                    if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                    {
                        drpInquiryStatus.Attributes.Add("disabled", "disabled");
                        drpAssignTo.Attributes.Add("disabled", "disabled");
                        txtProductName.Attributes.Add("disabled", "disabled");
                        txtCustomerName.Attributes.Add("disabled", "disabled");
                        drpState.Attributes.Add("disabled", "disabled");
                        drpCountry.Attributes.Add("disabled", "disabled");
                        drpCity.Attributes.Add("disabled", "disabled");
                        txtPinCode.Attributes.Add("disabled", "disabled");
                        txtFollowupNotes.Attributes.Add("disabled", "disabled");
                        txtFollowupDate.Attributes.Add("disabled", "disabled");
                        txtPreferredTime.Attributes.Add("disabled", "disabled");

                        btnGenerateInquiry.Attributes.Add("disabled", "disabled");
                    }
                    else if (drpInquiryStatus.SelectedValue.ToLower() == "disqualified")
                    {
                        drpInquiryStatus.Attributes.Add("disabled", "disabled");
                        drpDisQualifiedReason.Attributes.Add("disabled", "disabled");

                        btnGenerateInquiry.Attributes.Add("disabled", "disabled");
                    }
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
            _pageValid = true;
            string strErr = "";
            Int64 ReturnInquiryPkID = 0, ReturnFollowupPkID = 0;

            //if (String.IsNullOrEmpty(txtSenderName.Text.Trim()))
            //{
            //    strErr += "<li>" + "Sender : Sender is required." + "</li>";
            //    _pageValid = false;
            //}

            if (String.IsNullOrEmpty(txtCompanyName.Text.Trim()))
            {
                strErr += "<li>" + "Company Name is Required." + "</li>";
                _pageValid = false;
            }
            //if (String.IsNullOrEmpty(txtCity.Text.Trim()))
            //{
            //    strErr += "<li>" + "City is required." + "</li>";
            //    _pageValid = false;
            //}
            //if (String.IsNullOrEmpty(txtState.Text.Trim()))
            //{
            //    strErr += "<li>" + "State is required." + "</li>";
            //    _pageValid = false;
            //}
            //if (String.IsNullOrEmpty(txtCountry.Text.Trim()))
            //{
            //    strErr += "<li>" + "Country is required." + "</li>";
            //    _pageValid = false;
            //}

            if (String.IsNullOrEmpty(txtContact.Text.Trim()))
            {
                strErr += "<li>" + "Primary Contact is required." + "</li>";
                _pageValid = false;
            }

            if (String.IsNullOrEmpty(drpInquiryStatus.SelectedValue))
            {
                strErr += "<li>" + "Lead Status is required." + "</li>";
                _pageValid = false;
            }


            if (drpInquiryStatus.SelectedValue == "Disqualified")
            {
                if (String.IsNullOrEmpty(drpDisQualifiedReason.SelectedValue))
                {
                    strErr += "<li>" + "DisQualified Reason is required." + "</li>";
                    _pageValid = false;
                }
            }

            if (drpInquiryStatus.SelectedValue == "Qualified")
            {
                if (String.IsNullOrEmpty(drpAssignTo.SelectedValue))
                {
                    strErr += "<li>" + "Assign To : Employee Selection is required." + "</li>";
                    _pageValid = false;
                }

                if (String.IsNullOrEmpty(hdnProductID.Value))
                {
                    strErr += "<li>" + "Product Selection is required." + "</li>";
                    _pageValid = false;
                }

                if ((String.IsNullOrEmpty(drpCountry.SelectedValue) || drpCountry.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
                {
                    strErr += "<li>" + "Country is required." + "</li>";
                    _pageValid = false;
                }

                if ((String.IsNullOrEmpty(drpState.SelectedValue) || drpState.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
                {
                    strErr += "<li>" + "State is required." + "</li>";
                    _pageValid = false;
                }

                if ((String.IsNullOrEmpty(drpCity.SelectedValue) || drpCity.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
                {
                    strErr += "<li>" + "City is required." + "</li>";
                    _pageValid = false;
                }

                //if (String.IsNullOrEmpty(txtCustomerName.Text))
                //{
                //    strErr += "<li>" + "Customer Selection is required." + "</li>";
                //    _pageValid = false;
                //}



                if (!String.IsNullOrEmpty(txtFollowupNotes.Text) && !String.IsNullOrEmpty(txtFollowupDate.Text))
                {
                    if (String.IsNullOrEmpty(txtFollowupNotes.Text))
                    {
                        strErr += "<li>" + "Followup Notes is required." + "</li>";
                        _pageValid = false;
                    }

                    if (String.IsNullOrEmpty(txtFollowupDate.Text))
                    {
                        strErr += "<li>" + "Next FollowUp Selection is required." + "</li>";
                        _pageValid = false;
                    }

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
                //funCheckCustomerExistance();

                // --------------------------------------------------------------
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.LeadID = (hdnSource.Value.ToLower() != "telecaller") ? txtLeadID.Text : hdnpkID.Value;
                objEntity.QueryDatetime = Convert.ToDateTime(txtQueryDate.Text);
                objEntity.SenderName = txtSenderName.Text;
                objEntity.CompanyName = txtCompanyName.Text;
                objEntity.CountryFlagURL = hdnCountryFlagURL.Value;
                objEntity.CountryISO = hdnCountryISO.Value;
                objEntity.Address = txtAddress.Text;
                objEntity.City = txtCity.Text;
                objEntity.State = txtState.Text;
                objEntity.CountryCode = txtCountry.Text;
                objEntity.SenderMail = txtEmail.Text;
                objEntity.PrimaryMobileNo = txtContact.Text;
                objEntity.SecondaryMobileNo = txtContact1.Text;
                //objEntity.LeadSource = txtLeadSource.Text;
                objEntity.LeadSource = "IndiaMart";
                objEntity.ForProduct = txtForProduct.Text;
                objEntity.Message = txtMessage.Text;
                objEntity.LeadStatus = drpInquiryStatus.SelectedValue.ToString();

                if (drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                {
                    objEntity.EmployeeID = Convert.ToInt64(drpAssignTo.SelectedValue);
                    objEntity.ProductID = Convert.ToInt64(hdnProductID.Value);
                    objEntity.Pincode = txtPinCode.Text.Trim();
                    objEntity.StateCode = !String.IsNullOrEmpty(drpState.SelectedValue) ? Convert.ToInt64(drpState.SelectedValue) : 0;
                    objEntity.CityCode = !String.IsNullOrEmpty(drpCity.SelectedValue) ? Convert.ToInt64(drpCity.SelectedValue) : 0;
                    objEntity.CountryCode = Convert.ToString(drpCountry.SelectedValue);
                    objEntity.CustomerID = (!String.IsNullOrEmpty(txtCustomerName.Text) && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0") ? Convert.ToInt64(hdnCustomerID.Value) : 0;

                    objEntity.FollowupNotes = txtFollowupNotes.Text;
                    objEntity.FollowupDate = (!String.IsNullOrEmpty(txtFollowupDate.Text)) ? Convert.ToDateTime(txtFollowupDate.Text) : SqlDateTime.MinValue.Value;
                    objEntity.PreferredTime = txtPreferredTime.Text;
                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "inprocess")
                {
                    if (!String.IsNullOrEmpty(drpAssignTo.SelectedValue))
                        objEntity.EmployeeID = Convert.ToInt64(drpAssignTo.SelectedValue);
                }
                else if (drpInquiryStatus.SelectedValue.ToLower() == "disqualified")
                {
                    objEntity.ExLeadClosure = Convert.ToInt64(drpDisQualifiedReason.SelectedValue);
                }

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ExternalLeadsMgmt.AddUpdateExternalLeads(objEntity, out ReturnCode, out ReturnMsg, out ReturnInquiryPkID, out ReturnFollowupPkID);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnGenerateInquiry.Attributes.Add("disabled", "disabled");
                }
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                {
                    if(drpInquiryStatus.SelectedValue.ToLower() == "qualified")
                    { 
                        try
                        {
                            string notificationMsg = "";
                            string customerName = "";
                            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && Convert.ToInt64(hdnCustomerID.Value) > 0)
                                customerName = txtCustomerName.Text;
                            else
                                customerName = !String.IsNullOrEmpty(txtCompanyName.Text) ? txtCompanyName.Text : txtSenderName.Text;
                            notificationMsg = "Inquiry Created From PortalLead For " + customerName + " And Assign To " + BAL.CommonMgmt.GetEmployeeNameByEmployeeID(Convert.ToInt64(drpAssignTo.SelectedValue)) + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                            BAL.CommonMgmt.SendNotification_Firebase("Inquiry", notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64(drpAssignTo.SelectedValue));
                            BAL.CommonMgmt.SendNotificationToDB("Inquiry",0, notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64(drpAssignTo.SelectedValue));

                            if ((((!String.IsNullOrEmpty(txtFollowupDate.Text)) ? Convert.ToDateTime(txtFollowupDate.Text) : SqlDateTime.MinValue.Value) != SqlDateTime.MinValue.Value) && (!String.IsNullOrEmpty(txtFollowupNotes.Text)))
                            {
                                notificationMsg = "";
                                notificationMsg = "FollowUp Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                BAL.CommonMgmt.SendNotification_Firebase("FollowUp", notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64(drpAssignTo.SelectedValue));
                                BAL.CommonMgmt.SendNotificationToDB("FollowUp",0, notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64(drpAssignTo.SelectedValue));
                            }

                        }
                        catch (Exception)
                        {}
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void btnGenerateInquiry_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
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
            String currStatus = drpInquiryStatus.SelectedValue.ToLower();
            divQualified.Visible = (currStatus == "qualified") ? true : false;
            divDisQualified.Visible = (currStatus == "disqualified") ? true : false;
            divAssignTo.Visible = (currStatus == "qualified" || currStatus == "inprocess") ? true : false;
            divFollowup.Visible = (currStatus == "qualified") ? true : false;
            // -------------------------------------------------------------------------
            if (currStatus == "qualified")
            {
                BindDropDown();
                drpAssignTo.Focus();

                try
                {
                    if (!string.IsNullOrEmpty(txtCountry.Text.ToString()))
                    {
                        drpCountry.Enabled = true;
                        drpCountry.SelectedValue = drpCountry.Items.FindByText(txtCountry.Text.ToString()).Value;
                        drpCountry_SelectedIndexChanged(null, null);
                    }

                    if (!String.IsNullOrEmpty(drpCountry.SelectedValue.ToString()) && !string.IsNullOrEmpty(txtState.Text.ToString()))
                    {
                        drpState.Enabled = true;
                        drpState.SelectedValue = drpState.Items.FindByText(txtState.Text.ToString()).Value;
                        drpState_SelectedIndexChanged(null, null);
                    }

                    if (!String.IsNullOrEmpty(drpState.SelectedValue.ToString()) && !string.IsNullOrEmpty(txtCity.Text.ToString()))
                    {
                        drpCity.Enabled = true;
                        drpCity.SelectedValue = drpCity.Items.FindByText(txtCity.Text.ToString()).Value;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            else if(currStatus == "inprocess")
            {
                BindDropDown();
                drpAssignTo.Focus();
            }
            else if (currStatus == "disqualified")
            {
                BindDisQualifiedDropDown();
                drpDisQualifiedReason.Focus();
            }
            else if (currStatus == "")
                drpAssignTo.Focus();
        }

        

        //public void funCheckCustomerExistance()
        //{

        //    // ----------------------------------------------------
        //    List<Entity.Customer> lstEntity = new List<Entity.Customer>();

        //    lstEntity = BAL.CustomerMgmt.GetCustomerList(txtCompanyName.Text.ToString());
        //    if (lstEntity.Count >0)
        //    {
        //        hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();                
        //    }
        //    else
        //    {
        //        List<Entity.City> lstCity = new List<Entity.City>();
        //        lstCity = BAL.ExternalLeadsMgmt.GetCityCodeByName(txtCity.Text.ToString(), drpState.SelectedValue, Session["LoginUserID"].ToString());
        //        if (lstCity.Count > 0)
        //        {
        //            hdnCityCode.Value = lstCity[0].CityCode.ToString();
        //        }

        //        int ReturnCode = 0;
        //        string ReturnMsg = "";

        //        Entity.Customer objEntity = new Entity.Customer();
        //        objEntity.CustomerID = 0;
        //        objEntity.CustomerType = "0";
        //        objEntity.CustomerName = txtCompanyName.Text.ToString();
        //        objEntity.Pincode = txtPinCode.Text.ToString();
        //        objEntity.ContactNo1 = txtContact.Text.ToString();
        //        objEntity.ContactNo2 = txtContact1.Text.ToString();
        //        objEntity.EmailAddress = txtEmail.Text.ToString();
        //        objEntity.BirthDate = SqlDateTime.MinValue.Value;
        //        objEntity.AnniversaryDate = SqlDateTime.MinValue.Value;
        //        objEntity.CityCode = hdnCityCode.Value;
        //        objEntity.StateCode = drpState.SelectedValue;
        //        objEntity.LoginUserID = Session["LoginUserID"].ToString();

        //        BAL.CustomerMgmt.AddUpdateCustomer(objEntity, out ReturnCode, out ReturnMsg);
        //        divErrorMessage.InnerHtml = @ReturnMsg;
        //        hdnCustomerID.Value = @ReturnCode.ToString();
        //    }

        //}

    }
}