using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class WebComplaint : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        Session["PageNo"] = 1;
        //        Session["PageSize"] = 10;
        //        Session["LoginUserID"] = "admin";
        //        ClearAllField();
        //        BindDropDown();
        //        // --------------------------------------------------------
        //        if (!String.IsNullOrEmpty(Request.QueryString["id"]))
        //        {
        //            hdnpkID.Value = Request.QueryString["id"].ToString();

        //            if (hdnpkID.Value == "0" || hdnpkID.Value == "")
        //                ClearAllField();
        //            else
        //            {
        //                setLayout("Edit");
        //                // -------------------------------------
        //                if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
        //                {
        //                    if (Request.QueryString["mode"].ToString() == "view")
        //                        OnlyViewControls();
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        myModuleAttachment.ModuleName = "complaint";
        //        myModuleAttachment.KeyValue = lblComplaintNo.Text;
        //        myModuleAttachment.ManageLibraryDocs();
        //    }
        //}

        //public void BindDropDown()
        //{
        //    drpCountry.ClearSelection();
        //    List<Entity.Country> lstCountry = new List<Entity.Country>();
        //    lstCountry = BAL.CountryMgmt.GetCountryList();
        //    drpCountry.DataSource = lstCountry;
        //    drpCountry.DataValueField = "CountryCode";
        //    drpCountry.DataTextField = "CountryName";
        //    drpCountry.DataBind();
        //    drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

        //    List<Entity.State> lstState = new List<Entity.State>();
        //    lstState = BAL.StateMgmt.GetStateList();
        //    drpState.DataSource = lstCountry;
        //    drpState.DataValueField = "StateCode";
        //    drpState.DataTextField = "StateName";
        //    drpState.DataBind();
        //    drpState.Items.Insert(0, new ListItem("-- All State --", ""));

        //    List<Entity.City> lstCity = new List<Entity.City>();
        //    lstCity = BAL.CityMgmt.GetCityList();
        //    drpCity.DataSource = lstCity;
        //    drpCity.DataValueField = "CityCode";
        //    drpCity.DataTextField = "CityName";
        //    drpCity.DataBind();
        //    drpCity.Items.Insert(0, new ListItem("-- All City --", ""));
        //}

        //public void OnlyViewControls()
        //{
        //    txtComplaintDate.ReadOnly = true;
        //    txtCustomerName.ReadOnly = true;
        //    txtReferenceNo.ReadOnly = true;
        //    txtComplaintNotes.ReadOnly = true;
        //    txtContactNo1.ReadOnly = true;

        //    btnSave.Visible = false;
        //    btnReset.Visible = false;
        //}

        //public void setLayout(string v)
        //{

        //}

        //public void ClearAllField()
        //{
        //    Session.Remove("dtModuleDoc");

        //    hdnpkID.Value = "";
        //    lblComplaintNo.Text = "";
        //    txtComplaintDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        //    hdnCustomerID.Value = "";
        //    txtCustomerName.Text = "";
        //    txtReferenceNo.Text = "";
        //    txtComplaintNotes.Text = "";
        //    txtContactNo1.Text = "";
        //    txtAddress.Text = "";
        //    txtArea.Text = "";
        //    drpCountry.Items.Clear();
        //    drpCity.Items.Clear();
        //    drpState.Items.Clear();
        //    txtPincode.Text = "";
        //    txtCustomerName.Focus();
        //    btnSave.Disabled = false;
        //    // ------------------------------------------------------------
        //    myModuleAttachment.ModuleName = "complaint";
        //    myModuleAttachment.KeyValue = lblComplaintNo.Text;
        //    myModuleAttachment.BindModuleDocuments();
        //}

        //protected void btnReset_ServerClick(object sender, EventArgs e)
        //{
        //    ClearAllField();
        //}

        //protected void btnSaveEmail_ServerClick(object sender, EventArgs e)
        //{
        //    SendAndSaveData(true);
        //}

        //protected void btnSave_ServerClick(object sender, EventArgs e)
        //{
        //    SendAndSaveData(false);
        //}
        //public void SendAndSaveData(Boolean v)
        //{
        //    int ReturnCode = 0, ReturnCode1 = 0;
        //    string ReturnMsg = "", ReturnMsg1 ="", ReturnComplaintNo = "";
        //    string strErr = "";

        //    _pageValid = true;

        //    if (String.IsNullOrEmpty(txtComplaintDate.Text) || (String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0"))
        //    {
        //        _pageValid = false;

        //        if (String.IsNullOrEmpty(txtComplaintDate.Text))
        //            strErr += "<li>" + "Complaint Date is required." + "</li>";

        //        if (String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value == "0")
        //            strErr += "<li>" + "Your Mobile # is Not Registered, Please Contact Your Company !" + "</li>";
        //    }
        //    // --------------------------------------------------------------
        //    if (_pageValid)
        //    {
        //        Entity.Complaint objEntity = new Entity.Complaint();

        //        if (!String.IsNullOrEmpty(hdnpkID.Value))
        //            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
        //        objEntity.ComplaintNo = lblComplaintNo.Text;
        //        objEntity.ComplaintDate = Convert.ToDateTime(txtComplaintDate.Text);
        //        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
        //        objEntity.ReferenceNo = txtReferenceNo.Text;
        //        objEntity.ComplaintNotes = txtComplaintNotes.Text;
        //        objEntity.LoginUserID = "admin";
        //        // -------------------------------------------------------------- Insert/Update Record
        //        BAL.ComplaintMgmt.AddUpdateComplaint(objEntity, out ReturnCode, out ReturnMsg, out ReturnComplaintNo);
        //        strErr += "<li>" + ReturnMsg + "</li>";

        //        if (ReturnCode > 0)
        //        {

        //            string[] RetComplaintDetail = ReturnComplaintNo.Split(',');
        //            ReturnComplaintNo = RetComplaintDetail[0];
        //            Int64 ReturntPKID = Convert.ToInt64(RetComplaintDetail[1]);

        //            lblComplaintNo.Text = ReturnComplaintNo;
        //            btnSave.Disabled = true;
        //            // ------------------------------------------------------------
        //            myModuleAttachment.KeyValue = lblComplaintNo.Text;
        //            myModuleAttachment.SaveModuleDocs();

        //            try
        //            {
        //                string notificationMsg = "";
        //                if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
        //                    notificationMsg = "Portal Complaint Updated By " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
        //                else
        //                    notificationMsg = "Portal Complaint Initiated By " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());

        //                //BAL.CommonMgmt.SendNotification_Firebase("Complaint", notificationMsg, Session["LoginUserID"].ToString(), EmployeeID);
        //                //BAL.CommonMgmt.SendNotificationToDB("Complaint", ReturntPKID, notificationMsg, Session["LoginUserID"].ToString(), EmployeeID);
        //            }
        //            catch (Exception)
        //            { }
        //        }
        //        // --------------------------------------------------------------
        //        //if (v)
        //        //{
        //        //    Entity.Authenticate objAuth = new Entity.Authenticate();
        //        //    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

        //        //    String sendEmailFlag = BAL.CommonMgmt.GetConstant("INQ-EMAIL", 0, objAuth.CompanyID).ToLower();
        //        //    if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
        //        //    {
        //        //        try
        //        //        {
        //        //            if (String.IsNullOrEmpty(hdnCustEmailAddress.Value) && objEntity.CustomerID > 0)
        //        //            {
        //        //                hdnCustEmailAddress.Value = BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID);
        //        //            }
        //        //            // -------------------------------------------------------
        //        //            if (!String.IsNullOrEmpty(hdnCustEmailAddress.Value) && hdnCustEmailAddress.Value.ToUpper() != "NULL")
        //        //            {
        //        //                String respVal = "";
        //        //                respVal = BAL.CommonMgmt.SendEmailNotifcation("INQUIRY-WELCOME", Session["LoginUserID"].ToString(), ((!String.IsNullOrEmpty(hdnpkID.Value)) ? Convert.ToInt64(hdnpkID.Value) : 0), hdnCustEmailAddress.Value);
        //        //            }
        //        //            strErr += "<li>" + "Email Notification Sent Successfully !" + "</li>";
        //        //        }
        //        //        catch (Exception ex)
        //        //        {
        //        //            strErr += "<li>" + "Email Notification Failed !" + "</li>";
        //        //        }
        //        //    }
        //        //}
        //    }
        //    // ------------------------------------------------------
        //    if (!String.IsNullOrEmpty(strErr))
        //    {
        //        if (ReturnCode > 0)
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        //        else
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        //    }
        //}

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

        //protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(drpState.SelectedValue))
        //    {
        //        if (Convert.ToInt64(drpState.SelectedValue) > 0)
        //        {
        //            List<Entity.City> lstEvents = new List<Entity.City>();
        //            lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpState.SelectedValue));
        //            drpCity.DataSource = lstEvents;
        //            drpCity.DataValueField = "CityCode";
        //            drpCity.DataTextField = "CityName";
        //            drpCity.DataBind();
        //            drpCity.Items.Insert(0, new ListItem("-- All City --", "0"));
        //            //drpCity.Enabled = true;
        //            drpCity.Focus();
        //        }

        //    }
        //    if (drpState.SelectedValue == "0" || drpState.SelectedValue == "")
        //    {
        //        drpCity.Items.Clear();
        //    }
        //}

        //protected void txtContactNo1_TextChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(txtContactNo1.Text))
        //    {
        //        List<Entity.Customer> lstCust = new List<Entity.Customer>();
        //        lstCust = BAL.CustomerMgmt.GetCustomerListByMobileNo(txtContactNo1.Text);
        //        if (lstCust.Count>0)
        //        {
        //            hdnCustomerID.Value = lstCust[0].CustomerID.ToString();
        //            txtCustomerName.Text = lstCust[0].CustomerName.Trim();
        //        }
        //        else
        //        {
        //            hdnCustomerID.Value = "";
        //            txtCustomerName.Text = "";
        //        }
        //    }
        //}
    }
}