using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class OrgStructure : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            // ----------------------------------------------
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnOrgCodeEmp.Value = Request.QueryString["id"].ToString();

                    if (hdnOrgCodeEmp.Value == "0" || hdnOrgCodeEmp.Value == "")
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
            txtOrgCode.ReadOnly = true;
            txtOrgName.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtPincode.ReadOnly = true;
            txtFax1.ReadOnly = true;
            txtLandline1.ReadOnly = true;
            drpCity.Attributes.Add("disabled", "disabled");
            txtEmailAddress.ReadOnly = true;
            drpOrgType.Attributes.Add("disabled", "disabled");
            drpReportTo.Attributes.Add("disabled", "disabled");
            chkActive.Enabled = false;
            txtPANNo.Text = "";
            txtCINNo.Text = "";
            txtGSTNo.Text = "";

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {

            // ---------------- City List -------------------------------------
            List<Entity.City> lstEvents = new List<Entity.City>();
            lstEvents = BAL.CityMgmt.GetCityList();
            drpCity.DataSource = lstEvents;
            drpCity.DataValueField = "CityCode";
            drpCity.DataTextField = "CityName";
            drpCity.DataBind();
            drpCity.Items.Insert(0, new ListItem("-- All City --", ""));

            // ---------------- Org.Type List -------------------------------------
            List<Entity.OrgTypes> lstOrgType = new List<Entity.OrgTypes>();
            lstOrgType = BAL.OrgTypeMgmt.GetOrgTypeList();
            drpOrgType.DataSource = lstOrgType;
            drpOrgType.DataValueField = "pkID";
            drpOrgType.DataTextField = "OrgType";
            drpOrgType.DataBind();
            drpOrgType.Items.Insert(0, new ListItem("-- Select Type --", "0"));

            //// ---------------- Report To List -------------------------------------
            List<Entity.OrganizationStructure> lstReportto = new List<Entity.OrganizationStructure>();
            lstReportto = BAL.OrganizationStructureMgmt.GetOrganizationStructureList();
            drpReportTo.DataSource = lstReportto;
            drpReportTo.DataValueField = "OrgCode";
            drpReportTo.DataTextField = "OrgName";
            drpReportTo.DataBind();
            drpReportTo.Items.Insert(0, new ListItem("-- Select Org --", ""));

            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpOrgHead.DataSource = lstEmployee;
            drpOrgHead.DataValueField = "pkID";
            drpOrgHead.DataTextField = "EmployeeName";
            drpOrgHead.DataBind();
            drpOrgHead.Items.Insert(0, new ListItem("-- Select Employee --", ""));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
                // -------------------------------------------------------------------------
                lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList(hdnOrgCodeEmp.Value, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnOrgCodeEmp.Value = lstEntity[0].OrgCode;
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtOrgCode.Text = lstEntity[0].OrgCode;
                txtOrgName.Text = lstEntity[0].OrgName;
                drpOrgHead.SelectedValue = lstEntity[0].OrgHead.ToString();
                drpOrgType.SelectedValue = lstEntity[0].OrgTypeCode.ToString();
                hdnOrgType.Value = lstEntity[0].OrgTypeCode.ToString();
                txtAddress.Text = lstEntity[0].Address;
                txtPincode.Text = lstEntity[0].Pincode;
                txtLandline1.Text = lstEntity[0].Landline1;
                txtFax1.Text = lstEntity[0].Fax1;
                drpCity.SelectedValue = lstEntity[0].CityCode;
                txtEmailAddress.Text = lstEntity[0].EmailAddress;
                drpReportTo.SelectedValue = lstEntity[0].ReportTo_OrgCode;
                chkActive.Checked = lstEntity[0].ActiveFlag;
                txtGSTNo.Text = lstEntity[0].GSTIN;
                txtPANNo.Text = lstEntity[0].PANNO;
                txtCINNo.Text = lstEntity[0].CINNO;
                // --------------------------------------------------------
                int totrec = 0;
                List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
                lstObject = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(hdnOrgCodeEmp.Value, Session["LoginUserID"].ToString(), 1, 50000, out totrec);

                rptOrgEmployee.DataSource = lstObject;
                rptOrgEmployee.DataBind();
                // --------------------------------------------------------
                txtOrgCode.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtOrgCode.Text) || String.IsNullOrEmpty(txtOrgName.Text) ||
                String.IsNullOrEmpty(drpOrgType.SelectedValue) || String.IsNullOrEmpty(drpReportTo.SelectedValue))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtOrgCode.Text))
                    strErr += "<li>" + "Org.Code is required." + "</li>";

                if (String.IsNullOrEmpty(txtOrgName.Text))
                    strErr += "<li>" + "Org.Name is required." + "</li>";

                if (drpOrgType.SelectedValue == "0")
                    strErr += "<li>" + "Org.Type is required." + "</li>";

                if (String.IsNullOrEmpty(drpReportTo.SelectedValue))
                    strErr += "<li>" + "Report To Org. is required." + "</li>";

            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                Entity.OrganizationStructure objEntity = new Entity.OrganizationStructure();
                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.OrgCode = txtOrgCode.Text;
                objEntity.OrgName = txtOrgName.Text;
                if (!String.IsNullOrEmpty(drpOrgType.SelectedValue) && drpOrgType.SelectedValue != "0")
                    objEntity.OrgTypeCode = Convert.ToInt64(drpOrgType.SelectedValue);
                objEntity.Address = txtAddress.Text;
                objEntity.Landline1 = txtLandline1.Text;
                objEntity.Fax1 = txtFax1.Text;
                objEntity.CityCode = drpCity.SelectedValue;
                objEntity.Pincode = txtPincode.Text;
                objEntity.EmailAddress = txtEmailAddress.Text;
                objEntity.ReportTo_OrgCode = drpReportTo.SelectedValue;
                objEntity.ActiveFlag = chkActive.Checked;
                objEntity.OrgHead = !String.IsNullOrEmpty(drpOrgHead.SelectedValue) ? Convert.ToInt64(drpOrgHead.SelectedValue) : 0;
                objEntity.GSTIN = txtGSTNo.Text;
                objEntity.PANNO = txtPANNo.Text;
                objEntity.CINNO = txtCINNo.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.OrganizationStructureMgmt.AddUpdateOrganizationStructure(objEntity, out ReturnCode, out ReturnMsg);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnOrgCodeEmp.Value = "";
            txtOrgCode.Text = "";
            txtOrgName.Text = "";
            txtAddress.Text = "";
            txtPincode.Text = "";
            txtFax1.Text = "";
            txtLandline1.Text = "";
            drpOrgHead.SelectedValue = "";
            drpCity.SelectedValue = "";
            txtEmailAddress.Text = "";
            drpOrgType.SelectedValue = "0";
            drpReportTo.SelectedValue = "";
            chkActive.Checked = true;
            btnSave.Disabled = false;
            txtGSTNo.Text = "";
            txtPANNo.Text = "";
            txtCINNo.Text = "";
            // --------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            lstObject = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("-1", Session["LoginUserID"].ToString(), 1, 50000, out totrec);

            rptOrgEmployee.DataSource = lstObject;
            rptOrgEmployee.DataBind();
            // --------------------------------------------------------
            txtOrgCode.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteOrgStructure(string OrgCode)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.OrganizationStructureMgmt.DeleteOrganizationStructure(OrgCode, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void txtGSTNo_TextChanged(object sender, EventArgs e)
        {
            txtPANNo.Text = "";
            string strErr = "";
            if (!String.IsNullOrEmpty(txtGSTNo.Text))
            {
                if (txtGSTNo.Text.ToString().Length == 15)
                {
                    txtPANNo.Text = txtGSTNo.Text.Substring(2, 10);
                }
                else
                {
                    strErr += "<li>" + "Enter correct 15 digit GST number." + "</li>";
                }
            }

            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }
    }
}