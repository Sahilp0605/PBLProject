using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ZoneCluster : System.Web.UI.Page
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
        //    try
           // {
                int TotalCount = 0;
                rptExternalLeadRegion.DataSource = BAL.ExternalLeadsMgmt.GetExternalLeadsRegionList(0, 0, 0, 0, 0, LoginUserID, out TotalCount);
                rptExternalLeadRegion.DataBind();
           // }
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        private void BindDropdown()
        {


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

        //protected void oldrptExternalLeadRegion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //        {
        //            HiddenField hdnEmployee = (HiddenField)e.Item.FindControl("hdnEmpID");
        //            HiddenField hdnStateID = (HiddenField)e.Item.FindControl("hdnStateID");
        //            //DropDownList drpEmployee = (DropDownList)e.Item.FindControl("drpEmployee");
        //            DropDownList drpState = (DropDownList)e.Item.FindControl("drpStateCode");
        //            Label lblEmployeeName = (Label)e.Item.FindControl("lblEmployeeName");
        //            // lblEmployeeName.Text = objAuthEmployeeName;

        //            //drpEmployee.DataSource = BindEmployee();
        //            //drpEmployee.DataTextField = "EmployeeName";
        //            //drpEmployee.DataValueField = "pkID";
        //            //drpEmployee.DataBind();
        //            //drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));
        //            //drpEmployee.SelectedValue = hdnEmployee.Value;

        //            drpState.DataSource = BindState();
        //            drpState.DataTextField = "StateName";
        //            drpState.DataValueField = "StateCode";
        //            drpState.DataBind();
        //            drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select State--", "0"));
        //            drpState.SelectedValue = hdnStateID.Value;


        //        }
        //        if (e.Item.ItemType == ListItemType.Footer)
        //        {
        //            HiddenField hdnEmployee = (HiddenField)e.Item.FindControl("hdnEmpID");
        //            HiddenField hdnStateID = (HiddenField)e.Item.FindControl("hdnStateID");
        //            //DropDownList drpEmployee = (DropDownList)e.Item.FindControl("drpEmployee");
        //            DropDownList drpState = (DropDownList)e.Item.FindControl("drpStateCode");
        //            Label lblEmployeeName = (Label)e.Item.FindControl("lblEmployeeName1");
        //            lblEmployeeName.Text = objAuthEmployeeName;

        //            //drpEmployee.DataSource = BindEmployee();
        //            //drpEmployee.DataTextField = "EmployeeName";
        //            //drpEmployee.DataValueField = "pkID";
        //            //drpEmployee.DataBind();
        //            //drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));


        //            drpState.DataSource = BindState();
        //            drpState.DataTextField = "StateName";
        //            drpState.DataValueField = "StateCode";
        //            drpState.DataBind();
        //            drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select State--", "0"));



        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        //protected void rptExternalLeadRegion_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    try
        //    {
        //        string strErr = "";
        //        if (e.CommandName.ToString() == "Save")
        //        {
        //            _pageValid = true;
        //            int ReturnCode;
        //            string ReturnMsg;


        //            //Int64 EmpId = Convert.ToInt64(((DropDownList)e.Item.FindControl("drpEmployee")).SelectedValue);
        //            Int64 StateID = Convert.ToInt64(((DropDownList)e.Item.FindControl("drpStateCode")).SelectedValue);
        //            if (StateID == 0)
        //            {
        //                _pageValid = false;

        //                //if (EmpId == 0)
        //                //    strErr += "<li>" + "Employee Selection Required" + "</li>";

        //                if (StateID == 0)
        //                    strErr += "<li>" + "State Selection Required" + "</li>";
        //            }

        //            if (_pageValid)
        //            {
        //                Entity.ExternalLeadsRegion objEntity = new Entity.ExternalLeadsRegion();
        //                if (e.CommandName.ToString() == "Save")
        //                {
        //                    string hdnpkid = ((HiddenField)e.Item.FindControl("hdnpkID")).Value;
        //                    objEntity.pkID = Convert.ToInt64(hdnpkid);
        //                }
        //                //objEntity.EmployeeID = EmpId;
        //                objEntity.StateCode = StateID;
        //                objEntity.LoginUserID = Session["LoginUserID"].ToString();
        //                BAL.ExternalLeadsMgmt.AddUpdateExternalLeadsRegion(objEntity, out ReturnCode, out ReturnMsg);
        //                strErr += "<li>" + ReturnMsg + "</li>";
        //            }
        //            // -------------------------------------------------------------------------
        //            BindGrid();
        //            // -------------------------------------------------------------------------
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);


        //        }
        //        if (e.CommandName.ToString() == "Delete")
        //        {
        //            int ReturnCode = 0;
        //            string ReturnMsg = "";
        //            // -------------------------------------------------------------- Delete Record
        //            BAL.ExternalLeadsMgmt.DeleteExternalLedasRegion(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
        //            strErr += "<li>" + ReturnMsg + "</li>";
        //            // -------------------------------------------------------------------------
        //            BindGrid();
        //            // -------------------------------------------------------------------------
        //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strErr = "";
            _pageValid = true;
            int ReturnCode = 0, ReturnClusterID = 0;
            string ReturnMsg;
            //Int64 EmpId = Convert.ToInt64(drpEmployee.SelectedValue);
            string CountryID = (!String.IsNullOrEmpty(drpCountryCode.SelectedValue)) ? drpCountryCode.SelectedValue : "0";
            Int64 StateID = (!String.IsNullOrEmpty(drpStateCode.SelectedValue)) ? Convert.ToInt64(drpStateCode.SelectedValue) : 0;

            if (String.IsNullOrEmpty(txtClusterName.Text) || 
                (String.IsNullOrEmpty(drpCountryCode.SelectedValue) || drpCountryCode.SelectedValue=="0") ||
                (String.IsNullOrEmpty(drpStateCode.SelectedValue) || drpStateCode.SelectedValue == "0"))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtClusterName.Text))
                    strErr += "<li>" + "Cluster Name is Required." + "</li>";

                if (String.IsNullOrEmpty(drpCountryCode.SelectedValue) || drpCountryCode.SelectedValue == "0")
                    strErr += "<li>" + "Country Selection is Required." + "</li>";

                if (String.IsNullOrEmpty(drpStateCode.SelectedValue) || drpStateCode.SelectedValue == "0")
                    strErr += "<li>" + "State Selection is Required." + "</li>";

            }

            if (_pageValid)
            {
                Entity.ZoneCluster objEntity = new Entity.ZoneCluster();
                objEntity.pkID = (String.IsNullOrEmpty(hdnpkID.Value)) ? 0 : Convert.ToInt64(hdnpkID.Value);
                objEntity.ClusterName = txtClusterName.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                BAL.ZoneClusterMgmt.AddUpdateCluster(objEntity, out ReturnCode, out ReturnMsg, out ReturnClusterID);
                if (ReturnCode>0)
                {
                    strErr += "<li>" + strErr + "</li>";

                    for (int i = 0; i < chkCity.Items.Count; i++)
                    {
                        if (chkCity.Items[i].Selected)
                        {
                            Entity.ZoneCluster objDetail = new Entity.ZoneCluster();
                            objDetail.pkID = (String.IsNullOrEmpty(hdnpkID.Value)) ? 0 : Convert.ToInt64(hdnpkID.Value);
                            objDetail.ClusterID = ReturnClusterID;
                            objDetail.CountryCode = drpCountryCode.SelectedValue;
                            objDetail.StateCode = Convert.ToInt64(drpStateCode.SelectedValue);
                            objDetail.LoginUserID = Session["LoginUserID"].ToString();
                            // ----------------------------------
                            objDetail.CityCode = Convert.ToInt64(chkCity.SelectedValue);
                            BAL.ZoneClusterMgmt.AddUpdateClusterDetail(objDetail, out ReturnCode, out ReturnMsg);
                        }
                    }                      
                }

                
            }
            BindGrid();

            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            }

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
            drpStateCode.Items.Insert(0, new ListItem("-- All State --", "0"));
        }

        protected void drpStateCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Entity.City> lstCity = new List<Entity.City>();
            lstCity = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpStateCode.SelectedValue));

            chkCity.DataSource = lstCity;
            chkCity.DataValueField = "CityCode";
            chkCity.DataTextField = "CityName";
            chkCity.DataBind();
        }


    }
}