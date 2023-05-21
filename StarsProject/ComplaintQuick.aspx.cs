using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;
//using RestSharp;

namespace StarsProject
{
    public partial class ComplaintQuick : System.Web.UI.Page
    {

        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                ClearAllField();
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
        }

        public void OnlyViewControls()
        {
            txtComplaintDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpStatus.Attributes.Add("disabled", "disabled");
            txtComplaintNotes.ReadOnly = true;
            drpEmployee.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;

            drpComplaintType.Attributes.Add("disabled", "disabled");
            txtClosingRemarks.ReadOnly = true;
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

            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);                
            }
            // ---------------- Designation List  -------------------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Assigned To --", "0"));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnpkID.Value), 0, "", Session["LoginUserID"].ToString());
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                lblComplaintNo.Text = lstEntity[0].ComplaintNo;
                txtComplaintDate.Text = lstEntity[0].ComplaintDate.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();
                drpStatus.SelectedValue = lstEntity[0].ComplaintStatus.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                drpComplaintType.SelectedValue = lstEntity[0].ComplaintType.ToString();
                txtClosingRemarks.Text = lstEntity[0].ClosingRemarks.ToString();
                drpComplaintType_SelectedIndexChanged(null,null);

                if (!String.IsNullOrEmpty(lstEntity[0].ScheduleDate.ToString()) && lstEntity[0].ScheduleDate.Value.Year > 1900)
                    txtScheduleDate.Text = lstEntity[0].ScheduleDate.Value.ToString("yyyy-MM-dd");
                else
                    txtScheduleDate.Text = null;
                // ---------------------------------------
                //if (drpComplaintType.SelectedValue == "Online")
                //    divClosingRemarks.Visible = true;
                BindPriorComplaintList();
                BindCustomerPrimaryAddress();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnComplaintNo = "";
            string strErr = "";

            _pageValid = true;

            if (String.IsNullOrEmpty(txtComplaintDate.Text) || 
               //((String.IsNullOrEmpty(txtContactNo1.Text) || drpCountry.SelectedValue == "0" || drpState.SelectedValue == "0" || drpCity.SelectedValue == "0")) ||
                drpEmployee.SelectedValue == "0")
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtComplaintDate.Text))
                    strErr += "<li>" + "Complaint Date is required." + "</li>";

                ////if (String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0" || drpCountry.SelectedValue == "0" || drpState.SelectedValue == "0" || drpCity.SelectedValue == "0")
                //if (drpCountry.SelectedValue == "0" || drpState.SelectedValue == "0" || drpCity.SelectedValue == "0")
                //{
                //        strErr += "<li>" + "Customer City Selection is required." + "</li>";
                //}

                if (drpEmployee.SelectedValue == "0")
                    strErr += "<li>" + "Assigned To Name is required." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Customer objCustomer = new Entity.Customer();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    objCustomer.CustomerID = Convert.ToInt64(hdnCustomerID.Value);

                string[] strCustomerName = txtCustomerName.Text.Split('-');

                objCustomer.CustomerName = strCustomerName[0].ToString();
                objCustomer.Address = txtAddress.Text;
                objCustomer.Area = txtArea.Text;
                objCustomer.CountryCode = drpCountry.SelectedValue;
                objCustomer.StateCode = drpState.SelectedValue;
                objCustomer.CityCode = drpCity.SelectedValue;
                objCustomer.Pincode = txtPincode.Text;
                objCustomer.ContactNo1 = txtContactNo1.Text;
                objCustomer.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CustomerMgmt.AddUpdateCustomerQuick(objCustomer, out ReturnCode1, out ReturnMsg1);
                // ------------------------------------------------------
                Entity.Complaint objEntity = new Entity.Complaint();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.ComplaintNo = lblComplaintNo.Text;
                objEntity.ComplaintDate = Convert.ToDateTime(txtComplaintDate.Text);
                objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0") ? Convert.ToInt64(hdnCustomerID.Value) : ReturnCode1;
                objEntity.ComplaintNotes = txtComplaintNotes.Text;
                objEntity.ComplaintStatus = drpStatus.SelectedValue;
                objEntity.ComplaintType = drpComplaintType.SelectedValue;
                objEntity.ClosingRemarks = txtClosingRemarks.Text;
                objEntity.ScheduleDate = String.IsNullOrWhiteSpace(txtScheduleDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtScheduleDate.Text);
                if (drpEmployee.SelectedValue != "0")
                    objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ComplaintMgmt.AddUpdateComplaintQuick(objEntity, out ReturnCode, out ReturnMsg, out ReturnComplaintNo);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    lblComplaintNo.Text = ReturnComplaintNo;
                    btnSave.Disabled = true;
                    string tmpval = "http://chat.chatmybot.in/whatsapp/api/v1/sendmessage?access-token=4197-35YW4IZVOETDQT0MDI&phone=91-[pMobile]&content=[pMessage]&contentType=1&fileName&caption";

                    //SendWhatsApp("Your Complaint " + ReturnComplaintNo + " is registered with us and our technical team will contact you shortly.", "91-9898621973");
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            lblComplaintNo.Text = "";
            txtComplaintDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            // ------------------------------------------------
            txtContactNo1.Text = "";
            txtAddress.Text = "";

            drpCity.Items.Clear();
            drpState.Items.Clear();

            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);
                DivCountry.Visible = false;
            }
            // ------------------------------------------------
            txtComplaintNotes.Text = "";
            drpEmployee.SelectedValue = "0";
            drpStatus.SelectedValue = "Open";
            btnSave.Disabled = false;
            txtCustomerName.Focus();
            drpComplaintType_SelectedIndexChanged(null, null);
            txtClosingRemarks.Text = "";
            txtScheduleDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            BindCustomerPrimaryAddress();
        }

        public void BindCustomerPrimaryAddress()
        {
            int totrec;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                BindPriorComplaintList();
                List<Entity.Customer> lstCust = new List<Entity.Customer>();
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 10000, out totrec);
                if (lstCust.Count > 0)
                {
                    txtContactNo1.Text = lstCust[0].ContactNo1;
                    txtAddress.Text = lstCust[0].Address;
                    txtArea.Text = lstCust[0].Area;
                    txtPincode.Text = lstCust[0].Pincode;
                    if (lstCust[0].CountryCode != "0")
                    {
                        drpCountry.Items.FindByValue(lstCust[0].CountryCode).Selected = true;
                        drpCountry_SelectedIndexChanged(null, null);

                        drpState.Items.FindByValue(lstCust[0].StateCode).Selected = true;
                        drpState_SelectedIndexChanged(null, null);

                        if (lstCust[0].StateCode != "0")
                            drpCity.Items.FindByValue(lstCust[0].CityCode).Selected = true;
                    }
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteComplaint(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ComplaintMgmt.DeleteComplaint(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        public void BindPriorComplaintList()
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnCustomerID.Value), "", Session["LoginUserID"].ToString());
            rptFollowupTrail.DataSource = lstEntity;
            rptFollowupTrail.DataBind();
            lnkPriorFollowup.Visible = (lstEntity.Count > 0) ? true : false;
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
                }

            }
        }

        protected void drpComplaintType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(drpComplaintType.SelectedValue =="Online")
                divClosingRemarks.Visible = true;
            else
                divClosingRemarks.Visible = false;
        }

        //public void SendWhatsApp(string pMessage, string pMobileNo)
        //{
        //    string strMessageUrl = "";
        //    strMessageUrl = "http://chat.chatmybot.in/whatsapp/api/v1/sendmessage?access-token=4197-35YW4IZVOETDQT0MDI&phone=[pMobileNo]&content=[pMessage]&contentType=1&fileName&caption";
        //    strMessageUrl = strMessageUrl.Replace("[pMobileNo]", pMobileNo);
        //    strMessageUrl = strMessageUrl.Replace("[pMessage]", pMessage);
        //    var client = new RestClient(strMessageUrl);
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //    IRestResponse response = client.Execute(request);
        //}
    }
}