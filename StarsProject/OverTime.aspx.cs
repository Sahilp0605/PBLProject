using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class OverTime : System.Web.UI.Page
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
            txtReasonForOT.ReadOnly = true;
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
                List<Entity.OverTime> lstEntity = new List<Entity.OverTime>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.OverTimeMgmt.GetOverTimeList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), 0, 0, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtReasonForOT.Text = lstEntity[0].ReasonForOT.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                txtFromDate.Text = lstEntity[0].FromDate.ToString("yyyy-MM-dd");
                txtToDate.Text = lstEntity[0].ToDate.ToString("yyyy-MM-dd");
                txtFromTime.Text = lstEntity[0].FromDate.ToString("HH:mm tt");
                txtToTime.Text = lstEntity[0].ToDate.ToString("HH:mm tt");
                txtFromTime.Text = (txtFromTime.Text == "00:00 AM" || txtFromTime.Text == "00:00 PM") ? "" : txtFromTime.Text;
                txtToTime.Text = (txtToTime.Text == "00:00 AM" || txtToTime.Text == "00:00 PM") ? "" : txtToTime.Text;

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

            _pageValid = true;

            string strErr = "";

            if (String.IsNullOrEmpty(txtReasonForOT.Text) || String.IsNullOrEmpty(txtFromDate.Text) || String.IsNullOrEmpty(txtFromTime.Text) || String.IsNullOrEmpty(txtToDate.Text) || String.IsNullOrEmpty(txtToTime.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtReasonForOT.Text))
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
                Entity.OverTime objEntity = new Entity.OverTime();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.EmployeeID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0;
                objEntity.ReasonForOT = txtReasonForOT.Text;

                if (!String.IsNullOrEmpty(txtFromDate.Text) && !String.IsNullOrEmpty(txtFromTime.Text))
                    objEntity.FromDate = Convert.ToDateTime(Convert.ToDateTime(txtFromDate.Text + " " + txtFromTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                if (!String.IsNullOrEmpty(txtToDate.Text) && !String.IsNullOrEmpty(txtToTime.Text))
                    objEntity.ToDate = Convert.ToDateTime(Convert.ToDateTime(txtToDate.Text + " " + txtToTime.Text).ToString("yyyy-MM-dd HH:mm tt"));


                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.OverTimeMgmt.AddUpdateOverTime(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";
                // --------------------------------------------------------------
                // Sending EMAIL 
                // --------------------------------------------------------------
                //if (paraSaveAndEmail)
                //{
                //    Entity.Authenticate objAuth = new Entity.Authenticate();
                //    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                //    String sendEmailFlag = BAL.CommonMgmt.GetConstant("LEAVE-EMAIL", 0, objAuth.CompanyID).ToLower();
                //    if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                //    {
                //        try
                //        {
                //            if (String.IsNullOrEmpty(hdnEmpEmailAddress.Value) && objEntity.EmployeeID > 0)
                //            {
                //                hdnEmpEmailAddress.Value = objAuth.EmailAddress;
                //            }
                //            // -------------------------------------------------------
                //            if (!String.IsNullOrEmpty(hdnEmpEmailAddress.Value) && hdnEmpEmailAddress.Value.ToUpper() != "NULL")
                //            {
                //                String respVal = "";
                //                respVal = BAL.CommonMgmt.SendLeaveNotification("OverTime", objEntity);
                //            }
                //            strErr += "<li>" + ReturnMsg + " and Email Sent Successfully !" + "</li>";
                //        }
                //        catch (Exception ex)
                //        {
                //            strErr += "<li>" + ReturnMsg + " and Sending Email Failed !" + "</li>";
                //        }
                //    }
                //}

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    btnSaveEmail.Disabled = true;
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
            txtReasonForOT.Text = String.Empty;
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
        }


        [System.Web.Services.WebMethod]
        public static string DeleteOverTime(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.OverTimeMgmt.DeleteOverTime(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
   
}