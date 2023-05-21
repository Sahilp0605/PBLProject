using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ExternalLeadsRegions : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public string objAuthEmployeeName;
        string LoginUserID;
        int flag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 100000;
                LoginUserID = Session["LoginUserID"].ToString();

                BindDropdown();
                // ----------------------------------------------------------
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                objAuthEmployeeName = objAuth.EmployeeName;
                // ----------------------------------------------------------
                BindGrid();
            }
        }

        void BindGrid()
        {
            try
            {
                int TotalCount = 0;
                rptExternalLeadRegion.DataSource = BAL.ExternalLeadsMgmt.GetExternalLeadsRegionList(0, 0, 0, 0, 0, LoginUserID, out TotalCount);
                rptExternalLeadRegion.DataBind();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void BindDropdown()
        {
            drpEmployee.DataSource = BindEmployee();
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));

            List<Entity.Country> lstEvents = new List<Entity.Country>();
            lstEvents = BAL.CountryMgmt.GetCountryList();
            drpCountryCode.DataSource = lstEvents;
            drpCountryCode.DataValueField = "CountryCode";
            drpCountryCode.DataTextField = "CountryName";
            drpCountryCode.DataBind();
            drpCountryCode.Items.Insert(0, new ListItem("-- All Country --", "0"));

            //drpStateCode.DataSource = BindState();
            //drpStateCode.DataTextField = "StateName";
            //drpStateCode.DataValueField = "StateCode";
            //drpStateCode.DataBind();
            //drpStateCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select State--", "0"));

        }

        List<Entity.OrganizationEmployee> BindEmployee()
        {
            LoginUserID = Session["LoginUserID"].ToString();
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(LoginUserID);
            return lstEmployee;
        }

        List<Entity.State> BindState()
        {
            List<Entity.State> lstState = new List<Entity.State>();
            lstState = BAL.StateMgmt.GetStateList("IND");
            return lstState;
        }

        protected void oldrptExternalLeadRegion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hdnEmployee = (HiddenField)e.Item.FindControl("hdnEmpID");
                    HiddenField hdnStateID = (HiddenField)e.Item.FindControl("hdnStateID");
                    DropDownList drpEmployee = (DropDownList)e.Item.FindControl("drpEmployee");
                    DropDownList drpState = (DropDownList)e.Item.FindControl("drpStateCode");
                    Label lblEmployeeName = (Label)e.Item.FindControl("lblEmployeeName");
                    // lblEmployeeName.Text = objAuthEmployeeName;

                    drpEmployee.DataSource = BindEmployee();
                    drpEmployee.DataTextField = "EmployeeName";
                    drpEmployee.DataValueField = "pkID";
                    drpEmployee.DataBind();
                    drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));
                    drpEmployee.SelectedValue = hdnEmployee.Value;

                    drpState.DataSource = BindState();
                    drpState.DataTextField = "StateName";
                    drpState.DataValueField = "StateCode";
                    drpState.DataBind();
                    drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select State--", "0"));
                    drpState.SelectedValue = hdnStateID.Value;


                }
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    HiddenField hdnEmployee = (HiddenField)e.Item.FindControl("hdnEmpID");
                    HiddenField hdnStateID = (HiddenField)e.Item.FindControl("hdnStateID");
                    DropDownList drpEmployee = (DropDownList)e.Item.FindControl("drpEmployee");
                    DropDownList drpState = (DropDownList)e.Item.FindControl("drpStateCode");
                    Label lblEmployeeName = (Label)e.Item.FindControl("lblEmployeeName1");
                    lblEmployeeName.Text = objAuthEmployeeName;

                    drpEmployee.DataSource = BindEmployee();
                    drpEmployee.DataTextField = "EmployeeName";
                    drpEmployee.DataValueField = "pkID";
                    drpEmployee.DataBind();
                    drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));


                    drpState.DataSource = BindState();
                    drpState.DataTextField = "StateName";
                    drpState.DataValueField = "StateCode";
                    drpState.DataBind();
                    drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select State--", "0"));



                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void rptExternalLeadRegion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                string strErr = "";
                int ReturnCode;
                string ReturnMsg;
                Int64 EmpId = Convert.ToInt64(e.CommandArgument.ToString());
                string CountryID = ((HiddenField)e.Item.FindControl("hdnCountryID")).Value;
                Int64 StateID = Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnStateID")).Value);
                string CityList = ((HiddenField)e.Item.FindControl("hdnCityList")).Value;
                // --------------------------------------------------------                
                if (e.CommandName.ToString() == "Save")
                {
                    drpEmployee.SelectedValue = e.CommandArgument.ToString();
                    drpCountryCode.SelectedValue = CountryID;
                    // --------------------------------------
                    drpCountryCode_SelectedIndexChanged(this, EventArgs.Empty);
                    drpStateCode.SelectedValue = StateID.ToString();
                    // --------------------------------------
                    drpStateCode_SelectedIndexChanged(this, EventArgs.Empty);

                    for (int i = 0; i < chkCity.Items.Count; i++)
                    {
                        if (CityList.Contains(chkCity.Items[i].Value + ","))
                        {
                            chkCity.Items[i].Selected = true;
                        }
                    }

                }
                if (e.CommandName.ToString() == "Delete")
                {
                    // -------------------------------------------------------------- Delete Record
                    BAL.ExternalLeadsMgmt.DeleteExternalLedasRegion(EmpId, CountryID, StateID, out ReturnCode, out ReturnMsg);
                    // -------------------------------------------------------------------------
                    strErr += "<li>" + ReturnMsg + "</li>";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                    // -------------------------------------------------------------------------
                    BindGrid();
                    ClearAllField();
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode;
            string ReturnMsg;
            Int64 EmpId = Convert.ToInt64(drpEmployee.SelectedValue);
            string CountryID = drpCountryCode.SelectedValue != "0" ? drpCountryCode.SelectedValue : "";
            Int64 StateID = (drpStateCode.SelectedValue != "0" && drpStateCode.SelectedValue != "") ? Convert.ToInt64(drpStateCode.SelectedValue) : 0;
            // -------------------------------------------------------------- Delete Record
            BAL.ExternalLeadsMgmt.DeleteExternalLedasRegion(EmpId, CountryID, StateID, out ReturnCode, out ReturnMsg);
            // -------------------------------------------------------------- 
            SaveData();
        }

        protected void drpCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpStateCode.Items.Clear();
            List<Entity.State> lstEvents = new List<Entity.State>();
            if (!String.IsNullOrEmpty(drpCountryCode.SelectedValue))
            {
                lstEvents = BAL.StateMgmt.GetStateList(Convert.ToString(drpCountryCode.SelectedValue));
            }
            else
            {
                lstEvents = BAL.StateMgmt.GetStateList();
            }

            drpStateCode.DataSource = lstEvents;
            drpStateCode.DataValueField = "StateCode";
            drpStateCode.DataTextField = "StateName";
            drpStateCode.DataBind();
            drpStateCode.Items.Insert(0, new ListItem("-- All State --", ""));
            drpStateCode.Focus();
        }

        protected void drpStateCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Entity.City> lstCity = new List<Entity.City>();
            if (!String.IsNullOrEmpty(drpStateCode.SelectedValue))
            {
                lstCity = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpStateCode.SelectedValue));

                chkCity.DataSource = lstCity;
                chkCity.DataValueField = "CityCode";
                chkCity.DataTextField = "CityName";
                chkCity.DataBind();
            }
            else
            {
                chkCity.Items.Clear();
            }
            chkCity.Focus();
        }

        public void SaveData()
        {
            string strErr = "";
            _pageValid = true;
            int ReturnCode = 0;
            string ReturnMsg;
            Int64 EmpId = Convert.ToInt64(drpEmployee.SelectedValue);
            string CountryID = (!String.IsNullOrEmpty(drpCountryCode.SelectedValue)) ? drpCountryCode.SelectedValue : "0";
            Int64 StateID = (!String.IsNullOrEmpty(drpStateCode.SelectedValue)) ? Convert.ToInt64(drpStateCode.SelectedValue) : 0;

            if ((drpEmployee.SelectedValue == "0") || drpCountryCode.SelectedValue == "0" || String.IsNullOrEmpty(drpStateCode.SelectedValue))
            {
                _pageValid = false;

                if (drpEmployee.SelectedValue == "0")
                    strErr += "<li>" + "Employee Selection is Required." + "</li>";

                if (drpCountryCode.SelectedValue == "0")
                    strErr += "<li>" + "Country Selection is Required." + "</li>";

                if (String.IsNullOrEmpty(drpStateCode.SelectedValue))
                    strErr += "<li>" + "State Selection is Required." + "</li>";

            }

            if (_pageValid)
            {
                int selCityCount = 0;
                // Loop & count
                for (int i = 0; i < chkCity.Items.Count; i++)
                {
                    if (chkCity.Items[i].Selected)
                    {
                        selCityCount += 1;
                    }
                }
                // -------------------------------------------------
                if (selCityCount == 0)
                {
                    Entity.ExternalLeadsRegion objEntity = new Entity.ExternalLeadsRegion();
                    flag = 1;
                    objEntity.pkID = 0;
                    objEntity.EmployeeID = EmpId;
                    objEntity.CountryCode = CountryID;
                    objEntity.StateCode = StateID;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();

                    BAL.ExternalLeadsMgmt.AddUpdateExternalLeadsRegion(objEntity, out ReturnCode, out ReturnMsg);
                    strErr += "<li>" + ReturnMsg + "</li>";
                }
                else
                {
                    for (int i = 0; i < chkCity.Items.Count; i++)
                    {
                        if (chkCity.Items[i].Selected)
                        {
                            Entity.ExternalLeadsRegion objEntity = new Entity.ExternalLeadsRegion();
                            flag = 1;
                            objEntity.pkID = 0;
                            objEntity.EmployeeID = EmpId;
                            objEntity.CountryCode = CountryID;
                            objEntity.StateCode = StateID;
                            objEntity.CityCode = Convert.ToInt64(chkCity.Items[i].Value);
                            objEntity.LoginUserID = Session["LoginUserID"].ToString();

                            BAL.ExternalLeadsMgmt.AddUpdateExternalLeadsRegion(objEntity, out ReturnCode, out ReturnMsg);
                            strErr += "<li>" + ReturnMsg + "</li>";

                        }
                    }


                }
                if (ReturnCode > 0)
                {
                    //btnSave.Attributes.Add("disabled", "disabled");
                    btnSave.Disabled = true;
                }
            }
            BindGrid();

            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
        }

        private void ClearAllField()
        {
            drpEmployee.SelectedValue = "0";

            //drpCountryCode.SelectedIndex = drpCountryCode.Items.IndexOf(drpCountryCode.Items.FindByText("India"));
            //drpCountryCode_SelectedIndexChanged(null,null);
            //drpStateCode.SelectedIndex = drpStateCode.Items.IndexOf(drpStateCode.Items.FindByValue("0"));
            //drpStateCode.SelectedIndex = 0;
            drpCountryCode.SelectedValue = "0";
            chkCity.Items.Clear();
            btnSave.Disabled = false;
        }
    }
}