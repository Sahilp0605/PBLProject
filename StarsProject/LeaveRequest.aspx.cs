using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class LeaveRequest : System.Web.UI.Page
    {
        bool _pageValid = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;

                BindDropDown();
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

        public void BindDropDown()
        {
            // ---------------- Leave Types ------------------------
            List<Entity.LeaveRequest> lstLeaveTypes = new List<Entity.LeaveRequest>();
            lstLeaveTypes = BAL.LeaveRequestMgmt.GetLeaveTypes();
            drpLeaveType.DataSource = lstLeaveTypes;
            drpLeaveType.DataValueField = "LeaveTypeID";
            drpLeaveType.DataTextField = "LeaveType";
            drpLeaveType.DataBind();
            drpLeaveType.Items.Insert(0, new ListItem("-- Select Leave Type --", ""));

            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
        }

        public void OnlyViewControls()
        {
            txtReasonForLeave.ReadOnly = true;
            drpEmployee.Attributes.Add("disabled", "disabled");
            txtFromDate.ReadOnly = true;
            txtToDate.ReadOnly = true;
            txtFromTime.ReadOnly = true;
            txtToTime.ReadOnly = true;
            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.LeaveRequestMgmt.GetLeaveRequestList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), 0, 0, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtReasonForLeave.Text = lstEntity[0].ReasonForLeave.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                txtFromDate.Text = lstEntity[0].FromDate.ToString("yyyy-MM-dd");
                txtToDate.Text = lstEntity[0].ToDate.ToString("yyyy-MM-dd");
                txtFromTime.Text = lstEntity[0].FromDate.ToString("HH:mm tt");
                txtToTime.Text = lstEntity[0].ToDate.ToString("HH:mm tt");
                txtFromTime.Text = (txtFromTime.Text == "00:00 AM" || txtFromTime.Text == "00:00 PM") ? "" : txtFromTime.Text;
                txtToTime.Text = (txtToTime.Text == "00:00 AM" || txtToTime.Text == "00:00 PM") ? "" : txtToTime.Text;
                drpLeaveType.SelectedValue = lstEntity[0].LeaveTypeID.ToString();

                // ----------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // ----------------------------------------------------------
                if (objAuth.RoleCode.ToLower() != "admin")
                {
                    drpEmployee.Enabled = false;
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
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            Int64 ReturnLeavePKID = 0;

            _pageValid = true;

            string strErr = "";

            if (String.IsNullOrEmpty(txtReasonForLeave.Text) || String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtFromTime.Text) || String.IsNullOrEmpty(txtToDate.Text) || String.IsNullOrEmpty(txtToTime.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtReasonForLeave.Text))
                    strErr += "<li>" + "Reason For Leave is required." + "</li>";

                if (String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtFromTime.Text))
                    strErr += "<li>" + "Start Date & Time is required." + "</li>";

                if (String.IsNullOrEmpty(txtToDate.Text) || String.IsNullOrEmpty(txtToTime.Text))
                    strErr += "<li>" + "To Date & Time is required." + "</li>";
                
            }

            if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtToDate.Text))
            {
                if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
                {
                    strErr += "<li>" + "From Date should be less than To Date." + "</li>";
                    _pageValid = false;
                }

            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.EmployeeID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0;
                objEntity.ReasonForLeave = txtReasonForLeave.Text;

                if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtFromTime.Text))
                    objEntity.FromDate = Convert.ToDateTime(Convert.ToDateTime(txtFromDate.Text + " " + txtFromTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                if (!String.IsNullOrEmpty(txtToDate.Text) && !String.IsNullOrEmpty(txtToTime.Text))
                    objEntity.ToDate = Convert.ToDateTime(Convert.ToDateTime(txtToDate.Text + " " + txtToTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                objEntity.LeaveTypeID = (!String.IsNullOrEmpty(drpLeaveType.SelectedValue)) ? Convert.ToInt64(drpLeaveType.SelectedValue) : 0;
               
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.LeaveRequestMgmt.AddUpdateLeaveRequest(objEntity, out ReturnCode, out ReturnMsg, out ReturnLeavePKID);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";
                // --------------------------------------------------------------
                // Sending EMAIL 
                // --------------------------------------------------------------
                if (paraSaveAndEmail)
                {
                    Entity.Authenticate objAuth = new Entity.Authenticate();
                    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                    String sendEmailFlag = BAL.CommonMgmt.GetConstant("LEAVE-EMAIL", 0, objAuth.CompanyID).ToLower();
                    if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(hdnEmpEmailAddress.Value) && objEntity.EmployeeID > 0)
                            {
                                hdnEmpEmailAddress.Value = objAuth.EmailAddress;
                            }
                            // -------------------------------------------------------
                            if (!String.IsNullOrEmpty(hdnEmpEmailAddress.Value) && hdnEmpEmailAddress.Value.ToUpper() != "NULL")
                            {
                                String respVal = "";
                                respVal = BAL.CommonMgmt.SendLeaveNotification("LEAVEREQUEST", objEntity);
                            }
                            strErr += "<li>" + ReturnMsg + " and Email Sent Successfully !" + "</li>";
                        }
                        catch (Exception ex)
                        {
                            strErr += "<li>" + ReturnMsg + " and Sending Email Failed !" + "</li>";
                        }
                    }
                }

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    btnSaveEmail.Disabled = true;

                    string notificationMsg = "";
                    if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
                        notificationMsg = "Leave Request Updated For " + BAL.CommonMgmt.GetEmployeeNameByEmployeeID(Convert.ToInt64((!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0)) + " From " + Convert.ToDateTime(txtFromDate.Text).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(txtToDate.Text.ToString()).ToString("dd-MM-yyyy");
                    else
                        notificationMsg = "Leave Request Created For  " + BAL.CommonMgmt.GetEmployeeNameByEmployeeID(Convert.ToInt64((!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0)) + " From " + Convert.ToDateTime(txtFromDate.Text).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(txtToDate.Text.ToString()).ToString("dd-MM-yyyy");
                    BAL.CommonMgmt.SendNotification_Firebase("Leave Request", notificationMsg, BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64((!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0)), 0);
                    BAL.CommonMgmt.SendNotificationToDB("Leave Request", ReturnLeavePKID, notificationMsg, BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64((!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0)), 0);
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

        public void ClearAllField()
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            // ----------------------------------------------------------
            hdnpkID.Value = String.Empty;
            txtReasonForLeave.Text = String.Empty;
            txtFromDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFromTime.Text = DateTime.Today.ToString("HH:mm tt");
            txtToTime.Text = DateTime.Today.ToString("HH:mm tt");
            drpEmployee.SelectedValue = objAuth.EmployeeID.ToString();
            
            if (objAuth.RoleCode.ToLower() != "admin")
            {
                drpEmployee.Enabled = false;
            }

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
            // -----------------------------------------------
            myLeaveBalances.pageView = "";
            myLeaveBalances.pageEmployeeID = objAuth.EmployeeID.ToString();
            myLeaveBalances.pageLeaveTypeID = "0";
            myLeaveBalances.BindLeaveBalances();

        }


        [System.Web.Services.WebMethod]
        public static string DeleteLeaveRequest(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.LeaveRequestMgmt.DeleteLeaveRequest(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

    }
}