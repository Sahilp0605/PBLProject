using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlTypes;

namespace StarsProject
{
    public partial class WebComplaintNew : System.Web.UI.Page
    {
        bool _pageValid = true;
        int totrec = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["LoginUserID"] = "admin";
                BindDropDown();
                // --------------------------------------------------------
                myAttachDefect.ResetSession("ComplaintAcu-Panel");
                myAttachPanel.ResetSession("ComplaintAcu-Defect");
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
                //myModuleAttachment.ModuleName = "WebComplaintAcupanel";
                myAttachPanel.ModuleName = "ComplaintAcu-Panel";
                myAttachPanel.KeyValue = lblComplaintNo.Text;
                myAttachPanel.ManageLibraryDocs();

                //myModuleAttachment1.ModuleName = "WebComplaintAcuDefect";
                myAttachDefect.ModuleName = "ComplaintAcu-Defect";
                myAttachDefect.KeyValue = lblComplaintNo.Text;
                myAttachDefect.ManageLibraryDocs();
            }
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
            drpCountry.Items.Insert(0, new ListItem("-- Select Country --", ""));

            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("ComplaintStatus");
            drpStatus.DataSource = lstDesig;
            drpStatus.DataValueField = "InquiryStatusName";
            drpStatus.DataTextField = "InquiryStatusName";
            drpStatus.DataBind();
            drpStatus.Items.Insert(0, new ListItem("--Select Status --", "0"));

            //List<Entity.State> lstState = new List<Entity.State>();
            //lstState = BAL.StateMgmt.GetStateList();
            //drpState.DataSource = lstState;
            //drpState.DataValueField = "StateCode";
            //drpState.DataTextField = "StateName";
            //drpState.DataBind();
            //drpState.Items.Insert(0, new ListItem("-- All State --", ""));

            //List<Entity.City> lstCity = new List<Entity.City>();
            //lstCity = BAL.CityMgmt.GetCityList();
            //drpCity.DataSource = lstCity;
            //drpCity.DataValueField = "CityCode";
            //drpCity.DataTextField = "CityName";
            //drpCity.DataBind();
            //drpCity.Items.Insert(0, new ListItem("-- All City --", ""));
        }

        public void OnlyViewControls()
        {
            txtName.ReadOnly = true;
            txtMobileNo.ReadOnly = true;
            txtDesignation.ReadOnly = true;
            //txtCustomerName.ReadOnly = true;
            txtCompanyEmailID.ReadOnly = true;
            txtWorkOdrNo.ReadOnly = true;
            txtdtPurchase.ReadOnly = true;
            txtPanelSRNo.ReadOnly = true;
            txtProductSRNO.ReadOnly = true;
            txtSiteAdd.ReadOnly = true;
            drpState.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");
            txtPinCode.ReadOnly = true;
            txtSiteCoordinatorName.ReadOnly = true;
            txtsiteMobileNo.ReadOnly = true;
            txtConvinientDate.ReadOnly = true;
            txtConvinientTimeSlot.ReadOnly = true;
            txtComplaintNotes.ReadOnly = true;
            drpStatus.Attributes.Add("disabled", "disabled");
            txtNameOfCustomer.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string v)
        {

            if (v == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnpkID.Value), 0, "", Session["LoginUserID"].ToString());
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                lblComplaintNo.Text = lstEntity[0].ComplaintNo;
                //txtCustomerName.Text = lstEntity[0].CustomerName;
                txtCompanyEmailID.Text = lstEntity[0].EmailId;
                txtMobileNo.Text = lstEntity[0].CustmoreMobileNo.ToString();
                txtName.Text = lstEntity[0].CustomerEmpName;
                txtDesignation.Text = lstEntity[0].Designation;

                txtWorkOdrNo.Text = lstEntity[0].WorkOderNo;
                txtdtPurchase.Text = (lstEntity[0].DateOfPurchase.Year <= 1900) ? "" : lstEntity[0].DateOfPurchase.ToString("yyyy-MM-dd");
                txtPanelSRNo.Text = lstEntity[0].PanelSRNo;
                txtProductSRNO.Text = lstEntity[0].ProductSRNo;

                txtSiteAdd.Text = lstEntity[0].SiteAddress;
                txtPinCode.Text = lstEntity[0].Pincode;
                drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();
                if (!String.IsNullOrEmpty(Convert.ToString(lstEntity[0].CountryCode)))
                {
                    drpState.Enabled = true;
                    drpCountry_SelectedIndexChanged(null, null);
                    drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                }

                if (!String.IsNullOrEmpty(Convert.ToString(lstEntity[0].StateCode)) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                }

                txtSiteCoordinatorName.Text = lstEntity[0].SiteCoordinatorName;
                txtsiteMobileNo.Text = lstEntity[0].SiteMobileNo;
                txtConvinientDate.Text = (lstEntity[0].ConvinientDate.Year <= 1900) ? "" : lstEntity[0].ConvinientDate.ToString("yyyy-MM-dd");
                txtConvinientTimeSlot.Text = lstEntity[0].ConvinientTimeSlot.ToString();
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();
                drpStatus.SelectedValue = lstEntity[0].ComplaintStatus.ToString();

                //PhotoOfDefectProductFile.ImageUrl = lstEntity[0].PhotoOfDefectProduct;
                //PhotoOfPanelFile.ImageUrl = lstEntity[0].PhotoOfPanel;

                if (!String.IsNullOrEmpty(lstEntity[0].StateCode.ToString()) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                }

                txtNameOfCustomer.Text = lstEntity[0].NameOfCustomer;

                //if (!String.IsNullOrEmpty(lstEntity[0].PreferredDate.ToString()) && lstEntity[0].PreferredDate.Value.Year > 1900)
                //    txtPreferredDate.Text = lstEntity[0].PreferredDate.Value.ToString("yyyy-MM-dd");
                //else
                //    txtPreferredDate.Text = null;

                //txtTimeFrom.Text = lstEntity[0].TimeFrom.ToString();
                //txtTimeTo.Text = lstEntity[0].TimeTo.ToString();

            }
            //// ------------------------------------------------------------
            //myModuleAttachment.ModuleName = "WebComplaintAcupanel";
            myAttachPanel.ModuleName = "ComplaintAcu-Panel";
            myAttachPanel.KeyValue = lblComplaintNo.Text;
            myAttachPanel.BindModuleDocuments();

            //myModuleAttachment1.ModuleName = "WebComplaintAcuDefect";
            myAttachDefect.ModuleName = "ComplaintAcu-Defect";
            myAttachDefect.KeyValue = lblComplaintNo.Text;
            myAttachDefect.BindModuleDocuments();
        }

        public void ClearAllField()
        {
            // --------------------------------------------------------
            myAttachDefect.ResetSession("ComplaintAcu-Panel");
            myAttachPanel.ResetSession("ComplaintAcu-Defect");
            // --------------------------------------------------------
            hdnpkID.Value = "";
            lblComplaintNo.Text = "";
            txtName.Text = "";
            txtMobileNo.Text = "";
            txtDesignation.Text = "";
            hdnCustomerID.Value = "";
            //txtCustomerName.Text = "";
            txtCompanyEmailID.Text = "";
            txtWorkOdrNo.Text = "";
            txtdtPurchase.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtPanelSRNo.Text = "";
            txtProductSRNO.Text = "";
            txtSiteAdd.Text = "";
            drpState.Items.Clear();
            drpCity.Items.Clear();
            txtPinCode.Text = "";
            txtSiteCoordinatorName.Text = "";
            txtsiteMobileNo.Text = "";
            txtConvinientDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtConvinientTimeSlot.Text = "";
            txtComplaintNotes.Text = "";
            drpStatus.SelectedValue = "0";
            txtNameOfCustomer.Text = "";

            txtName.Focus();
            btnSave.Disabled = false;

            // ------------------------------------------------------------
            //myModuleAttachment.ModuleName = "WebComplaintAcupanel";
            myAttachPanel.ModuleName = "ComplaintAcu-Panel";
            myAttachPanel.KeyValue = lblComplaintNo.Text;
            myAttachPanel.BindModuleDocuments();

            //myModuleAttachment1.ModuleName = "WebComplaintAcuDefect";
            myAttachDefect.ModuleName = "ComplaintAcu-Defect";
            myAttachDefect.KeyValue = lblComplaintNo.Text;
            myAttachDefect.BindModuleDocuments();
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSaveEmail_ServerClick(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }
        public void SendAndSaveData(Boolean v)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnComplaintNo = "";
            string strErr = "";

            _pageValid = true;

            if ((String.IsNullOrEmpty(txtNameOfCustomer.Text)) ||
                (String.IsNullOrEmpty(txtMobileNo.Text)) || (String.IsNullOrEmpty(txtCompanyEmailID.Text)) ||
                (String.IsNullOrEmpty(txtWorkOdrNo.Text)) || (String.IsNullOrEmpty(txtdtPurchase.Text)) ||
                (String.IsNullOrEmpty(txtSiteCoordinatorName.Text)) || (String.IsNullOrEmpty(txtsiteMobileNo.Text)) || (String.IsNullOrEmpty(txtComplaintNotes.Text)))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtNameOfCustomer.Text))
                    strErr += "<li>" + "Customer Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtMobileNo.Text))
                    strErr += "<li>" + "Primary Contact No is required." + "</li>";

                if (String.IsNullOrEmpty(txtCompanyEmailID.Text))
                    strErr += "<li>" + "Email Address is required." + "</li>";

                if (String.IsNullOrEmpty(txtWorkOdrNo.Text))
                    strErr += "<li>" + "Work Order # is required." + "</li>";

                if (String.IsNullOrEmpty(txtdtPurchase.Text))
                    strErr += "<li>" + "Purchase Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtSiteCoordinatorName.Text))
                    strErr += "<li>" + "Site Coordinator Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtsiteMobileNo.Text))
                    strErr += "<li>" + "Site Incharge Mobile No is required." + "</li>";

                if (String.IsNullOrEmpty(txtComplaintNotes.Text))
                    strErr += "<li>" + "Complaint Description is required." + "</li>";

            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Complaint objEntity = new Entity.Complaint();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                objEntity.ComplaintDate = DateTime.Now;
                objEntity.CustmoreMobileNo = txtMobileNo.Text;
                objEntity.ComplaintNo = lblComplaintNo.Text;

                objEntity.CustomerEmpName = txtName.Text;
                objEntity.Designation = txtDesignation.Text;
                objEntity.WorkOderNo = txtWorkOdrNo.Text;
                objEntity.EmailId = txtCompanyEmailID.Text;
                objEntity.PanelSRNo = txtPanelSRNo.Text;
                objEntity.ProductSRNo = txtProductSRNO.Text;

                objEntity.SiteAddress = txtSiteAdd.Text;
                objEntity.CountryCode = (!String.IsNullOrEmpty(drpCountry.SelectedValue)) ? drpCountry.SelectedValue : "";
                objEntity.StateCode = (!String.IsNullOrEmpty(drpState.SelectedValue)) ? Convert.ToInt64(drpState.SelectedValue) : 0;
                objEntity.CityCode = (!String.IsNullOrEmpty(drpCity.SelectedValue)) ? Convert.ToInt64(drpCity.SelectedValue) : 0;
                objEntity.Pincode = txtPinCode.Text;

                objEntity.SiteCoordinatorName = txtSiteCoordinatorName.Text;
                objEntity.SiteMobileNo = txtsiteMobileNo.Text;


                if (!String.IsNullOrEmpty(txtdtPurchase.Text))
                {
                    if (Convert.ToDateTime(txtdtPurchase.Text).Year > 1900)
                        objEntity.DateOfPurchase = Convert.ToDateTime(txtdtPurchase.Text);
                }

                if (!String.IsNullOrEmpty(txtConvinientDate.Text))
                {
                    if (Convert.ToDateTime(txtConvinientDate.Text).Year > 1900)
                        objEntity.ConvinientDate = Convert.ToDateTime(txtConvinientDate.Text);
                }
                objEntity.ConvinientTimeSlot = txtConvinientTimeSlot.Text;
                objEntity.ComplaintNotes = txtComplaintNotes.Text;
                objEntity.ComplaintStatus = drpStatus.SelectedValue;
                objEntity.NameOfCustomer = txtNameOfCustomer.Text;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ComplaintMgmt.AddUpdateComplaint(objEntity, out ReturnCode, out ReturnMsg, out ReturnComplaintNo);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {

                    string[] RetComplaintDetail = ReturnComplaintNo.Split(',');
                    ReturnComplaintNo = RetComplaintDetail[0];
                    Int64 ReturntPKID = Convert.ToInt64(RetComplaintDetail[1]);

                    lblComplaintNo.Text = ReturnComplaintNo;
                    btnSave.Disabled = true;
                    // ------------------------------------------------------------
                    myAttachPanel.KeyValue = ReturnComplaintNo;
                    myAttachPanel.SaveModuleDocs();
                    //------------------------------------------------------------
                    myAttachDefect.KeyValue = ReturnComplaintNo;
                    myAttachDefect.SaveModuleDocs();

                    try
                    {
                        string notificationMsg = "";
                        if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
                            notificationMsg = "Portal Complaint Updated By " + txtNameOfCustomer.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                        else
                            notificationMsg = "Portal Complaint Initiated By " + txtNameOfCustomer.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());

                        //BAL.CommonMgmt.SendNotification_Firebase("Complaint", notificationMsg, Session["LoginUserID"].ToString(), EmployeeID);
                        //BAL.CommonMgmt.SendNotificationToDB("Complaint", ReturntPKID, notificationMsg, Session["LoginUserID"].ToString(), EmployeeID);
                    }
                    catch (Exception)
                    { }
                }
                //--------------------------------------------------------------
                if (v)
                {
                    Entity.Authenticate objAuth = new Entity.Authenticate();
                    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                    String sendEmailFlag = BAL.CommonMgmt.GetConstant("INQ-EMAIL", 0, objAuth.CompanyID).ToLower();
                    if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(hdnCustEmailAddress.Value) && objEntity.CustomerID > 0)
                            {
                                hdnCustEmailAddress.Value = BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID);
                            }
                            // -------------------------------------------------------
                            if (!String.IsNullOrEmpty(hdnCustEmailAddress.Value) && hdnCustEmailAddress.Value.ToUpper() != "NULL")
                            {
                                String respVal = "";
                                respVal = BAL.CommonMgmt.SendEmailNotifcation("INQUIRY-WELCOME", Session["LoginUserID"].ToString(), ((!String.IsNullOrEmpty(hdnpkID.Value)) ? Convert.ToInt64(hdnpkID.Value) : 0), hdnCustEmailAddress.Value);
                            }
                            strErr += "<li>" + "Email Notification Sent Successfully !" + "</li>";
                        }
                        catch (Exception ex)
                        {
                            strErr += "<li>" + "Email Notification Failed !" + "</li>";
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

        //protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
        //    {
        //        if (!string.IsNullOrEmpty(drpCountry.SelectedValue))
        //        {
        //            List<Entity.State> lstEvents = new List<Entity.State>();
        //            lstEvents = BAL.StateMgmt.GetStateList((drpCountry.SelectedValue).ToString());
        //            drpState.DataSource = lstEvents;
        //            drpState.DataValueField = "StateCode";
        //            drpState.DataTextField = "StateName";
        //            drpState.DataBind();
        //            drpState.Items.Insert(0, new ListItem("-- All State --", "0"));
        //            //drpState.Enabled = true;
        //            drpState.Focus();
        //        }

        //    }
        //    if (drpCountry.SelectedValue == "0" || drpCountry.SelectedValue == "")
        //    {
        //        drpState.Items.Clear();
        //        drpCity.Items.Clear();
        //    }
        //}

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            
        }

        // ------------------------------------------------------------------------------------
        // Country / State Change Event 
        // ------------------------------------------------------------------------------------
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
                    drpState.Focus();
                }

            }
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
                    drpCity.Items.Insert(0, new ListItem("-- All City --", "0"));
                    //drpCity.Enabled = true;
                    drpCity.Focus();
                }

            }
            if (drpState.SelectedValue == "0" || drpState.SelectedValue == "")
            {
                drpCity.Items.Clear();
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteComplaint(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0, totrec = 0;
            string ReturnMsg = "";
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(pkID), 0, "", HttpContext.Current.Session["LoginUserID"].ToString());
            if (lstEntity.Count > 0)
            {
                myModuleAttachment mya = new myModuleAttachment();
                //mya.DeleteModuleEntry("WebComplaintAcupanel", lstEntity[0].ComplaintNo.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
                //mya.DeleteModuleEntry("WebComplaintAcuDefect", lstEntity[0].ComplaintNo.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));

                mya.DeleteModuleEntry("ComplaintAcu-Panel", lstEntity[0].ComplaintNo.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
                mya.DeleteModuleEntry("ComplaintAcu-Defect", lstEntity[0].ComplaintNo.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));

            }
            // --------------------------------- Delete Record
            BAL.ComplaintMgmt.DeleteComplaint(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        //protected void txtMobileNo_TextChanged(object sender, EventArgs e)
        //{
        //    string strErr = "";

        //    if (!String.IsNullOrEmpty(txtMobileNo.Text))
        //    {
        //        List<Entity.Customer> lstCust = new List<Entity.Customer>();
        //        lstCust = BAL.CustomerMgmt.GetCustomerListByMobileNo(txtMobileNo.Text);
        //        if (lstCust.Count > 0)
        //        {
        //            hdnCustomerID.Value = lstCust[0].CustomerID.ToString();
        //            //txtCustomerName.Text = lstCust[0].CustomerName.Trim();
        //            txtSiteAdd.Text = lstCust[0].Address;
        //            txtPinCode.Text = lstCust[0].Pincode;
        //            drpCountry.SelectedValue = lstCust[0].CountryCode;
        //            drpCountry_SelectedIndexChanged(null,null);
        //            drpState.SelectedValue = lstCust[0].StateCode;
        //            drpState_SelectedIndexChanged(null, null);
        //            drpCity.SelectedValue = lstCust[0].CityCode;
        //            txtCompanyEmailID.Text = lstCust[0].EmailAddress;
        //        }
        //        else
        //        {
        //            hdnCustomerID.Value = "";
        //            //txtCustomerName.Text = "";
        //            strErr += "<li>" + "Customer Not Found" + "</li>";
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        //        }
        //    }
        //    else
        //    {
        //        strErr += "<li>" + "Customer Not Found" + "</li>";
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        //    }

        //}
    }
}