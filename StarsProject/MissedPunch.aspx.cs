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
    public partial class MissedPunch : System.Web.UI.Page
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
            
            drpEmployee.Attributes.Add("disabled", "disabled");
            txtPresenceDate.ReadOnly = true;
            txtTimeIn.ReadOnly = true;
            txtTimeOut.ReadOnly = true;
            txtNotes.ReadOnly = true;
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
                List<Entity.MissedPunch> lstEntity = new List<Entity.MissedPunch>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.AttendanceMgmt.GetMissedPunchList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), "", Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();

                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                txtPresenceDate.Text = lstEntity[0].PresenceDate.ToString("yyyy-MM-dd");
                txtTimeIn.Text = lstEntity[0].TimeIn.ToString();
                txtTimeOut.Text = lstEntity[0].TimeOut.ToString();
                txtNotes.Text = lstEntity[0].Notes.ToString();
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

            if (String.IsNullOrEmpty(txtPresenceDate.Text) || String.IsNullOrEmpty(txtTimeIn.Text) || String.IsNullOrEmpty(txtTimeOut.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtPresenceDate.Text))
                    strErr += "<li>" + "Missed Punch Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtTimeIn.Text))
                    strErr += "<li>" + "In Time is required." + "</li>";

                if (String.IsNullOrEmpty(txtTimeOut.Text))
                    strErr += "<li>" + "Out Time is required." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.MissedPunch objEntity = new Entity.MissedPunch();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.EmployeeID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0;

                if (!String.IsNullOrEmpty(txtPresenceDate.Text))
                    objEntity.PresenceDate = Convert.ToDateTime(txtPresenceDate.Text);

                objEntity.TimeIn = txtTimeIn.Text;
                objEntity.TimeOut = txtTimeOut.Text;
                objEntity.Notes = txtNotes.Text;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.AttendanceMgmt.AddUpdateMissedPunch(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

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
            drpEmployee.SelectedValue = objAuth.EmployeeID.ToString();
            txtPresenceDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtTimeIn.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtTimeOut.Text = DateTime.Today.ToString("HH:mm tt");
            txtNotes.Text = String.Empty;
            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
        }


        [System.Web.Services.WebMethod]
        public static string DeleteMissedPunch(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.AttendanceMgmt.DeleteMissedPunch(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}