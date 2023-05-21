using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class UserMgmt : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["OldUserID"] = "";
               
                // ==========================================================
                BindDropdown();
                ClearAllField();
                // ----------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnUserID.Value = Request.QueryString["id"].ToString();

                    if (hdnUserID.Value == "0" || hdnUserID.Value == "")
                    {
                        //==================license User Verification==========================
                        Int64 license_user = Convert.ToInt64(Session["LicenseUsers"]);
                        Int64 existing_user = Convert.ToInt64(Session["ExistingUsers"]);

                        if (existing_user + 1 <= license_user)
                        {
                            ClearAllField();
                        }
                        else
                        {
                            divUserMgmt.Visible = false;
                            divLicenseUser.Visible = true;
                            lblWarning.Text = "Sorry, You reached maxmimum Licensed User Limit !";
                        }
                    }
                    else
                    {
                        setLayout("Edit");
                    }
                    // -------------------------------------------------
                    if (!String.IsNullOrEmpty(Request.QueryString["type"]))
                    {
                        if (Request.QueryString["type"].ToString() == "profile1")
                        {
                            divAccount.Visible = true;
                            divOther.Visible = false;
                            divAccount.Attributes["class"] = "col m12 padding-2";
                        }
                        if (Request.QueryString["type"].ToString() == "profile2")
                        {
                            divAccount.Visible = false;
                            divOther.Visible = true;
                            divOther.Attributes["class"] = "col m12 padding-2";
                        }
                    }
                }              
            }

            txtPassword.Attributes["value"] = txtPassword.Text;
        }

        public void BindDropdown()
        {
            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = BAL.CommonMgmt.GetCompanyProfileList(0, "");
            drpCompany.DataSource = lstCompany;
            drpCompany.DataValueField = "CompanyID";
            drpCompany.DataTextField = "CompanyName";
            drpCompany.DataBind();
            drpCompany.Items.Insert(0, new ListItem("-- Select --", ""));
            drpCompany.SelectedIndex = 0;

            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            lstRole = BAL.RolesMgmt.GetRoleList();
            drpRole.DataSource = lstRole;
            drpRole.DataValueField = "RoleCode";
            drpRole.DataTextField = "Description";
            drpRole.DataBind();
            drpRole.Items.Insert(0, new ListItem("-- Select --", ""));
            drpRole.SelectedIndex = 0;

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList();
            drpOrganization.DataSource = lstOrg;
            drpOrganization.DataValueField = "OrgCode";
            drpOrganization.DataTextField = "OrgName";
            drpOrganization.DataBind();
            drpOrganization.Items.Insert(0, new ListItem("-- Select --", ""));
            drpOrganization.SelectedIndex = 0;

        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.Users> lstUser = new List<Entity.Users>();
                
                lstUser = BAL.UserMgmt.GetLoginUserList(hdnUserID.Value, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnpkID.Value = lstUser[0].pkID.ToString();
                txtScreenFullName.Text = lstUser[0].ScreenFullName;
                drpRole.SelectedValue = lstUser[0].RoleCode;
                drpOrganization.SelectedValue = lstUser[0].OrgCode;
                drpCompany.SelectedValue = lstUser[0].CompanyID.ToString();

                if (drpRole.SelectedValue != "dealer")
                {
                    dvEmployee.Visible = true;
                    dvCustomer.Visible = false;
                    BindEmployeeList();
                    drpEmployee.SelectedValue = lstUser[0].EmployeeID.ToString();
                }
                else
                {
                    dvEmployee.Visible = false;
                    dvCustomer.Visible = true;
                }

                txtUserID.Text = lstUser[0].UserID;
                txtPassword.Text = lstUser[0].UserPassword;
                chkActive.Checked = lstUser[0].ActiveFlag;
                // ------------------------------------------------------------
                BindMenuAction();
                // ------------------------------------------------------------
                txtScreenFullName.Focus();
                txtUserID.Enabled = false;
                txtUserID.CssClass = "form-control";

            }
        }
        public void BindMenuAction()
        {
            if (!String.IsNullOrEmpty(txtUserID.Text))
            {
                List<Entity.ApplicationMenu> lstMenu = new List<Entity.ApplicationMenu>();
                lstMenu = BAL.CommonMgmt.GetMenuAddEditDelList(0, txtUserID.Text);
                rptAddEditDel.DataSource = lstMenu;
                rptAddEditDel.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtScreenFullName.Text) || String.IsNullOrEmpty(txtUserID.Text) || String.IsNullOrEmpty(txtPassword.Text) ||
                String.IsNullOrEmpty(drpRole.SelectedValue) || String.IsNullOrEmpty(drpCompany.SelectedValue) || 
                (drpRole.SelectedValue != "dealer" && (String.IsNullOrEmpty(drpOrganization.SelectedValue) || String.IsNullOrEmpty(drpEmployee.SelectedValue))) ||
                (drpRole.SelectedValue == "dealer" && String.IsNullOrEmpty(hdnCustomerID.Value))
                ) 
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtUserID.Text))
                    strErr += "<li>" + "User ID is required." + "</li>";

                if (String.IsNullOrEmpty(txtPassword.Text))
                    strErr += "<li>" + "Password is required." + "</li>";

                if (String.IsNullOrEmpty(txtScreenFullName.Text))
                    strErr += "<li>" + "User ScreenName is required." + "</li>";

                if (String.IsNullOrEmpty(drpRole.SelectedValue))
                    strErr += "<li>" + "Role Selection is required." + "</li>";

                if (String.IsNullOrEmpty(drpCompany.SelectedValue))
                    strErr += "<li>" + "Company/Branch Selection is required." + "</li>";

                if (drpRole.SelectedValue != "dealer" && String.IsNullOrEmpty(drpOrganization.SelectedValue))
                    strErr += "<li>" + "Organization Selection is required." + "</li>";

                if (drpRole.SelectedValue != "dealer" && String.IsNullOrEmpty(drpEmployee.SelectedValue))
                    strErr += "<li>" + "Employee Selection is required." + "</li>";

                if (drpRole.SelectedValue == "dealer" && String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Dealer Selection is required." + "</li>";
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                //string textPassword = EncryptedPassword(txtPassword.Text.Trim());

                Entity.Users objUser = new Entity.Users();
                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objUser.pkID = Convert.ToInt64(hdnpkID.Value);

                objUser.ScreenFullName = txtScreenFullName.Text.Trim();
                objUser.Description = txtScreenFullName.Text.Trim();
                objUser.UserID = txtUserID.Text.Trim();
                objUser.UserPassword = txtPassword.Text;
                objUser.ActiveFlag = chkActive.Checked;
                objUser.RoleCode = drpRole.SelectedValue;
                objUser.CompanyID = Convert.ToInt64(drpCompany.SelectedValue);
                objUser.OrgCode = drpOrganization.SelectedValue;
                if (drpRole.SelectedValue != "dealer")
                {
                    objUser.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                }
                else
                {
                    objUser.CustomerID = Convert.ToInt64(hdnCustomerID.Value); 
                }
                objUser.LoginUserID = Session["LoginUserID"].ToString();
                // ----------------------------------------------- Calling Procedure to Insert/Update Record
                BAL.UserMgmt.AddUpdateUserManagement(objUser, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                // --------------------------------------------------------------//Registration Database User Insert
                if (ReturnCode > 0)
                {
                    BAL.UserMgmt.AddUpdateUserManagementRegistration(objUser, Session["SerialKey"].ToString(), out ReturnCode1, out ReturnMsg1);
                    // ---------------------------------------------------------
                    foreach (RepeaterItem i in rptAddEditDel.Items)
                    {
                        Entity.ApplicationMenu objEntity = new Entity.ApplicationMenu();

                        HiddenField hdnMenuID = (HiddenField)i.FindControl("hdnMenuID");
                        CheckBox chkAddFlag = (CheckBox)i.FindControl("chkAddFlag");
                        CheckBox chkEditFlag = (CheckBox)i.FindControl("chkEditFlag");
                        CheckBox chkDelFlag = (CheckBox)i.FindControl("chkDelFlag");

                        // --------------------------------------------------------
                        if (!String.IsNullOrEmpty(hdnMenuID.Value) && hdnMenuID.Value != "0")
                        {
                            objEntity.pkID = Convert.ToInt64(hdnMenuID.Value);
                            objEntity.UserID = txtUserID.Text;
                            objEntity.AddFlag = chkAddFlag.Checked;
                            objEntity.EditFlag = chkEditFlag.Checked;
                            objEntity.DelFlag = chkDelFlag.Checked;

                            BAL.CommonMgmt.AddUpdateUserAction(objEntity, out ReturnCode, out ReturnMsg);
                        }
                    }
                    // ---------------------------------------------------------
                    btnSave.Disabled = true;
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
            hdnpkID.Value = "";
            hdnUserID.Value = "";
            txtScreenFullName.Text = "";
            drpRole.SelectedIndex = 0;
            drpOrganization.SelectedIndex = 0;
            drpCompany.SelectedIndex = 0;
            txtUserID.Text = "";
            txtPassword.Text = "";
            chkActive.Checked = true;
            txtScreenFullName.Focus();

            if (drpRole.SelectedValue != "dealer")
                BindEmployeeList();

            btnSave.Disabled = false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteUsers(string UserID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.UserMgmt.DeleteLoginUser(UserID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpRole.SelectedValue != "dealer")
                BindEmployeeList();
        }

        public void BindEmployeeList()
        {
            int totrec;

            drpEmployee.Items.Clear();                
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            if (Session["RoleCode"].ToString() == "admin" || Session["RoleCode"].ToString() == "bradmin")
                lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), 1, 10000, out totrec); 
            else
                lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(drpOrganization.SelectedValue, Session["LoginUserID"].ToString(), 1, 10000, out totrec); 
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Select --", ""));
            drpEmployee.SelectedIndex = 0;

        }

        public string EncryptedPassword(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void drpRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvEmployee.Visible = (drpRole.SelectedValue != "dealer") ? true : false;
            dvCustomer.Visible = (drpRole.SelectedValue != "dealer") ? false : true;
        }
    }
}