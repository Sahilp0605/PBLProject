using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SiteSurveyReport : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindSSRRoofDetails("");
                BindEquipmentLocation("");
                BindSSRSysAvailablity("");
                BindSSRRequiredEngDetail("");
                //BindDropDown();
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
                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }
        public void OnlyViewControls()
        {
            //------------Pre-Fisibility Report----------------
            txtVisitDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtReq.ReadOnly = true;
            txtbuildingRemarks.ReadOnly = true;
            txtRoofRemarks.ReadOnly = true;
            txtPosRemarks.ReadOnly = true;
            drpBuilding.Attributes.Add("disabled", "disabled");
            drpPosition.Attributes.Add("disabled", "disabled");
            drpRoof.Attributes.Add("disabled", "disabled");
            //-------------General Details----------------
            txtSLoad.ReadOnly = true;
            txtConsumption.ReadOnly = true;
            txtinstallation.ReadOnly = true;
            chkBlock.Enabled = false;
            drpConnection.Attributes.Add("disabled", "disabled");
            txtConnRemarks.ReadOnly = true;
            //--------------------------------------------
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            //--------------------Pre-Fesibility Report-------
            txtReq.Text = "";
            txtVisitDate.Text = "";
            txtCustomerName.Text = "";
            txtSurveyID.Text = "";
            txtbuildingRemarks.Text = "";
            txtRoofRemarks.Text = "";
            txtPosRemarks.Text = "";
            drpBuilding.SelectedValue = "";
            drpRoof.SelectedValue = "";
            drpPosition.SelectedValue = "";
            txtConnRemarks.Text = "";
            //-------------General Details----------------
            txtSLoad.Text = "";
            txtConsumption.Text= "";
            txtinstallation.Text = "";
            chkBlock.Checked = true;
            drpConnection.SelectedValue = "";
            txtConnRemarks.Text = "";
            //--------------------------------------------
            btnSave.Disabled = false;
            btnReset.Disabled = false;
            BindSSRRoofDetails("");
            BindEquipmentLocation("");
            BindSSRSysAvailablity("");
            BindSSRRequiredEngDetail("");
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
        }
        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtEquipLoc = new DataTable();
            dtEquipLoc = (DataTable)Session["dtEquipLoc"];

            DataTable dtSysAvail = new DataTable();
            dtSysAvail = (DataTable)Session["dtSysAvail"];

            DataTable dtReq = new DataTable();
            dtReq = (DataTable)Session["dtReq"];

            int ReturnCode = 0, ReturnCode1 = 0, ReturnCode2 = 0, ReturnCode3 = 0, ReturnCode4 = 0;
            string ReturnMsg = "", ReturnSiteSurvayNo = "", ReturnMsg1 = "", ReturnMsg2 = "", ReturnMsg3 = "", ReturnMsg4 = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(txtVisitDate.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                {
                    strErr += "<li>" + "Customer Name is mandatory !" + "</li>";
                }
                txtCustomerName.Focus();
                if (String.IsNullOrEmpty(txtVisitDate.Text))
                {
                    strErr += "<li>" + "Visit Date is mandatory !" + "</li>";
                }
                txtVisitDate.Focus();
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.SiteSurveyReport objEntity = new Entity.SiteSurveyReport();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                //----------------Pre-Fesibility Report----------------
                objEntity.SurveyID = txtSurveyID.Text;
                objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.VisitDate = Convert.ToDateTime(txtVisitDate.Text);
                objEntity.SolarPosition = drpPosition.SelectedValue;
                objEntity.SolarPositionRemarks = txtPosRemarks.Text;
                objEntity.BuildType = drpBuilding.SelectedValue;
                objEntity.BuildTypeRemarks = txtbuildingRemarks.Text;
                objEntity.RoofType = drpRoof.SelectedValue;
                objEntity.RoofTypeRemarks = txtRoofRemarks.Text;
                objEntity.ClientReq = txtReq.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                //--------------------General Details-------------------
                objEntity.SanctionLoad = txtSLoad.Text;
                objEntity.MonthlyConsumption = txtConsumption.Text;
                objEntity.TotalArea = txtinstallation.Text;
                objEntity.LeaseProperty = chkBlock.Checked;
                objEntity.ExistingPhase = drpConnection.SelectedValue;
                objEntity.ExistingPhaseRemarks = txtConnRemarks.Text;

                //objEntity.LoginUserID = Convert.ToInt64(Session["LoginUserID"]);
                // -------------------------------------------------------------- Insert/Update Record
                BAL.SiteSurveyReportMgmt.AddUpdateGetSiteSurveyReport(objEntity, out ReturnCode, out ReturnMsg, out ReturnSiteSurvayNo);
                // --------------------------------------------------------------
                //strErr += "<li>" + ReturnMsg + "</li>";
                if (String.IsNullOrEmpty(ReturnSiteSurvayNo) && !String.IsNullOrEmpty(txtSurveyID.Text))
                {
                    ReturnSiteSurvayNo = txtSurveyID.Text;
                }
                strErr += "<li>" + ((ReturnCode > 0) ? ReturnSiteSurvayNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                //--------------------------------------------------------------------------
                //------------------------------Roof Details--------------------------------
                //--------------------------------------------------------------------------
                BAL.SiteSurveyReportMgmt.DeleteSSRRoofDetailsBySurveyID(txtSurveyID.Text, out ReturnCode, out ReturnMsg);
                if (ReturnCode > 0)
                {
                    Entity.SSRRoofDetails objQuotDet = new Entity.SSRRoofDetails();

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (dr.RowState.ToString() != "Deleted")
                        {
                            //objQuotDet.pkID = 0;
                            objQuotDet.SurveyID = ReturnSiteSurvayNo;
                            objQuotDet.BuildingName = (!String.IsNullOrEmpty(dr["BuildingName"].ToString())) ? dr["BuildingName"].ToString() : "";
                            objQuotDet.RoofType = (!String.IsNullOrEmpty(dr["RoofType"].ToString())) ? dr["RoofType"].ToString() : "";
                            objQuotDet.RoofArea = (!String.IsNullOrEmpty(dr["RoofArea"].ToString())) ? dr["RoofArea"].ToString() : "";
                            objQuotDet.CapacityOfBuilding = (!String.IsNullOrEmpty(dr["CapacityOfBuilding"].ToString())) ? dr["CapacityOfBuilding"].ToString() : "";
                            objQuotDet.LoginUserID = Session["LoginUserID"].ToString();
                        }
                        BAL.SiteSurveyReportMgmt.AddUpdateSSRRoofingDetails(objQuotDet, out ReturnCode1, out ReturnMsg1);
                    }
                    if (ReturnCode1 > 0)
                    {
                        Session.Remove("dtDetail");
                        btnSave.Disabled = true;
                    }
                }
                //--------------------------------------------------------------------------
                //------------------------------Equipement Location Details--------------------------------
                //--------------------------------------------------------------------------
                BAL.SiteSurveyReportMgmt.DeleteSSREquipmentLocationBySurveyID(txtSurveyID.Text, out ReturnCode, out ReturnMsg);
                if (dtEquipLoc != null)
                {
                    Entity.SSREquipmentLocation objQuotDet = new Entity.SSREquipmentLocation();

                    foreach (DataRow dr in dtEquipLoc.Rows)
                    {
                        if (dr.RowState.ToString() != "Deleted")
                        {
                            //objQuotDet.pkID = 0;
                            objQuotDet.SurveyID = ReturnSiteSurvayNo;
                            objQuotDet.Equipment = (!String.IsNullOrEmpty(dr["Equipment"].ToString())) ? dr["Equipment"].ToString() : "";
                            objQuotDet.Distance = (!String.IsNullOrEmpty(dr["Distance"].ToString())) ? dr["Distance"].ToString() : "";
                            objQuotDet.ConnPossibility = (!String.IsNullOrEmpty(dr["ConnPossibility"].ToString())) ? dr["ConnPossibility"].ToString() : "";
                            objQuotDet.ClientRating = (!String.IsNullOrEmpty(dr["ClientRating"].ToString())) ? dr["ClientRating"].ToString() : "";
                            objQuotDet.LoginUserID = Session["LoginUserID"].ToString();
                        }

                        BAL.SiteSurveyReportMgmt.AddUpdateSSREquipmentLocation(objQuotDet, out ReturnCode2, out ReturnMsg2);
                    }
                    if (ReturnCode2 > 0)
                    {
                        Session.Remove("dtEquipLoc");
                        btnSave.Disabled = true;
                    }
                }
                //--------------------------------------------------------------------------
                //------------------------------System Availablity--------------------------------
                //--------------------------------------------------------------------------
                BAL.SiteSurveyReportMgmt.DeleteSSRSysAvailablityBySurveyID(txtSurveyID.Text, out ReturnCode, out ReturnMsg);
                if (ReturnCode > 0)
                {
                    Entity.SSRSysAvailablity objQuotDet = new Entity.SSRSysAvailablity();

                    foreach (DataRow dr in dtSysAvail.Rows)
                    {
                        if (dr.RowState.ToString() != "Deleted")
                        {
                            //objQuotDet.pkID = 0;
                            objQuotDet.SurveyID = ReturnSiteSurvayNo;
                            objQuotDet.LoadDesc = (!String.IsNullOrEmpty(dr["LoadDesc"].ToString())) ? dr["LoadDesc"].ToString() : "";
                            objQuotDet.Capacity = (!String.IsNullOrEmpty(dr["Capacity"].ToString())) ? dr["Capacity"].ToString() : "";
                            objQuotDet.Voltage = (!String.IsNullOrEmpty(dr["Voltage"].ToString())) ? dr["Voltage"].ToString() : "";
                            objQuotDet.Quantity = (!String.IsNullOrEmpty(dr["Quantity"].ToString())) ? dr["Quantity"].ToString() : "";
                            objQuotDet.LoginUserID = Session["LoginUserID"].ToString();
                        }
                        BAL.SiteSurveyReportMgmt.AddUpdateSSRSysAvailablity(objQuotDet, out ReturnCode3, out ReturnMsg3);
                    }
                    if (ReturnCode3 > 0)
                    {
                        Session.Remove("dtSysAvail");
                        btnSave.Disabled = true;
                    }
                }
                //--------------------------------------------------------------------------
                //------------------------------Requirement Engineering Details--------------------------------
                //--------------------------------------------------------------------------
                BAL.SiteSurveyReportMgmt.DeleteSSRRequiredDetailsBySurveyID(txtSurveyID.Text, out ReturnCode, out ReturnMsg);
                if (ReturnCode > 0)
                {
                    Entity.SSRRequiredDetails objQuotDet = new Entity.SSRRequiredDetails();

                    foreach (DataRow dr in dtReq.Rows)
                    {
                        if (dr.RowState.ToString() != "Deleted")
                        {
                            //objQuotDet.pkID = 0;
                            objQuotDet.SurveyID = ReturnSiteSurvayNo;
                            objQuotDet.Description = (!String.IsNullOrEmpty(dr["Description"].ToString())) ? dr["Description"].ToString() : "";
                            objQuotDet.Remarks = (!String.IsNullOrEmpty(dr["Remarks"].ToString())) ? dr["Remarks"].ToString() : "";
                            objQuotDet.LoginUserID = Session["LoginUserID"].ToString();
                        }
                        BAL.SiteSurveyReportMgmt.AddUpdateSSRRequiredDetails(objQuotDet, out ReturnCode4, out ReturnMsg4);
                    }
                    if (ReturnCode4 > 0)
                    {
                        Session.Remove("dtReq");
                        btnSave.Disabled = true;
                    }
                }

                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.SiteSurveyReport> lstEntity = new List<Entity.SiteSurveyReport>();
                // ----------------------------------------------------
                lstEntity = BAL.SiteSurveyReportMgmt.GetSiteSurveyReport(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                //-----------------Pre-Feibility Report------------------
                txtSurveyID.Text = lstEntity[0].SurveyID;

                txtVisitDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].VisitDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"); ;
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                drpBuilding.SelectedValue = lstEntity[0].BuildType;
                drpPosition.SelectedValue = lstEntity[0].SolarPosition;
                drpRoof.SelectedValue = lstEntity[0].RoofType;
                txtbuildingRemarks.Text = lstEntity[0].BuildTypeRemarks;
                txtPosRemarks.Text = lstEntity[0].SolarPositionRemarks;
                txtRoofRemarks.Text = lstEntity[0].RoofTypeRemarks;
                txtReq.Text = lstEntity[0].ClientReq;
                //----------------General Details-----------------
                txtSLoad.Text = lstEntity[0].SanctionLoad;
                txtConsumption.Text = lstEntity[0].MonthlyConsumption;
                txtinstallation.Text = lstEntity[0].TotalArea;
                chkBlock.Checked = lstEntity[0].LeaseProperty;
                drpConnection.SelectedValue = lstEntity[0].ExistingPhase;
                txtConnRemarks.Text = lstEntity[0].ExistingPhaseRemarks;
                //---------------------------
                BindSSRRoofDetails(txtSurveyID.Text);
                BindEquipmentLocation(txtSurveyID.Text);
                BindSSRSysAvailablity(txtSurveyID.Text);
                BindSSRRequiredEngDetail(txtSurveyID.Text);
                //---------------------------
                txtCustomerName.Focus();
            }
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }
        [System.Web.Services.WebMethod]
        public static string DeleteSiteSurveyReport(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.SiteSurveyReportMgmt.DeleteGetSiteSurveyReport(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }


        //-----------------------Bind Roof Details for Survey Report----------------
        public void BindSSRRoofDetails(string SurveyID)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.SiteSurveyReportMgmt.GetSSRRoofDetails(SurveyID);
            rptRoofDetails.DataSource = dtDetail1;
            rptRoofDetails.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }
        protected void rptRoofDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;
                
                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

                    if (dtDetail != null)
                    {

                        foreach (System.Data.DataColumn col in dtDetail.Columns) col.AllowDBNull = true;

                        Int64 cntRow = dtDetail.Rows.Count + 1;
                        DataRow dr = dtDetail.NewRow();

                        dr["pkID"] = cntRow;
                        string bname = ((TextBox)e.Item.FindControl("txtBuildingType")).Text;
                        string rooftype = ((TextBox)e.Item.FindControl("txtRoofType")).Text;
                        string roofarea = ((TextBox)e.Item.FindControl("txtRoofArea")).Text;
                        string capacity = ((TextBox)e.Item.FindControl("txtCapacity")).Text;
                    
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                        //dr["SurveyID"] = txtSurveyID.Text;
                        dr["BuildingName"] = (!String.IsNullOrEmpty(bname)) ? Convert.ToString(bname) : "";
                        dr["RoofArea"] = (!String.IsNullOrEmpty(roofarea)) ? Convert.ToString(roofarea) : "";
                        dr["RoofType"] = (!String.IsNullOrEmpty(rooftype)) ? Convert.ToString(rooftype) : "";
                        dr["CapacityOfBuilding"] = (!String.IsNullOrEmpty(capacity)) ?  Convert.ToString(capacity) : "";
                        dtDetail.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptRoofDetails.DataSource = dtDetail;
                        rptRoofDetails.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtDetail", dtDetail);
                    }
                }
                // -------------------------------------------------

                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];

                string iname = ((TextBox)e.Item.FindControl("edBuildingName")).Text;

                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (dr["BuildingName"].ToString() == iname)
                    {
                        dtDetail.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }
                //DataRow[] rows;
                //rows = dtDetail.Select("pkID=" + e.CommandArgument.ToString());
                //foreach (DataRow r in rows)
                //    r.Delete();

                rptRoofDetails.DataSource = dtDetail;
                rptRoofDetails.DataBind();

                Session.Add("dtDetail", dtDetail);
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
        }

        protected void rptRoofDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void editRoofDetail_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edpkID = (HiddenField)item.FindControl("edRoofID");
            TextBox edBuildingName = (TextBox)item.FindControl("edBuildingName");
            TextBox edRoofType = (TextBox)item.FindControl("edRoofType");
            TextBox edRoofArea = (TextBox)item.FindControl("edRoofArea");
            TextBox edCapacity = (TextBox)item.FindControl("edCapacity");
            // --------------------------------------------------------------------------

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;
            foreach (DataRow row in dtDetail.Rows)
            {
                if (row.RowState.ToString() != "Deleted")
                {
                    if (row["pkID"].ToString() == edpkID.Value)
                    {
                        row.SetField("BuildingName", edBuildingName.Text);
                        row.SetField("RoofType", edRoofType.Text);
                        row.SetField("RoofArea", edRoofArea.Text);
                        row.SetField("CapacityOfBuilding", edCapacity.Text);
                        row.AcceptChanges();
                    }
                }
            }
            rptRoofDetails.DataSource = dtDetail;
            rptRoofDetails.DataBind();

            Session.Add("dtDetail", dtDetail);
        }

        //-----------------------Bind Equipement Location Details for Survey Report----------------
        public void BindEquipmentLocation(string SurveyID)
        {
            DataTable dtEquipLoc = new DataTable();
            dtEquipLoc = BAL.SiteSurveyReportMgmt.GetSSREquipmentLocation(SurveyID);
            rptEquipmentLocationDetails.DataSource = dtEquipLoc;
            rptEquipmentLocationDetails.DataBind();
            Session.Add("dtEquipLoc", dtEquipLoc);
        }
        protected void rptEquipmentLocationDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;

                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtEquipLoc = new DataTable();
                    dtEquipLoc = (DataTable)Session["dtEquipLoc"];

                    if (dtEquipLoc != null)
                    {

                        foreach (System.Data.DataColumn col in dtEquipLoc.Columns) col.AllowDBNull = true;

                        Int64 cntRow = dtEquipLoc.Rows.Count + 1;
                        DataRow dr = dtEquipLoc.NewRow();

                        dr["pkID"] = cntRow;
                        string equip = ((TextBox)e.Item.FindControl("txtEquipment")).Text;
                        string distance = ((TextBox)e.Item.FindControl("txtDistance")).Text;
                        string conn = ((TextBox)e.Item.FindControl("txtConnPossibility")).Text;
                        string rating = ((TextBox)e.Item.FindControl("txtClientRating")).Text;

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                        //dr["SurveyID"] = txtSurveyID.Text;
                        dr["Equipment"] = (!String.IsNullOrEmpty(equip)) ? Convert.ToString(equip) : "";
                        dr["Distance"] = (!String.IsNullOrEmpty(distance)) ? Convert.ToString(distance) : "";
                        dr["ConnPossibility"] = (!String.IsNullOrEmpty(conn)) ? Convert.ToString(conn) : "";
                        dr["ClientRating"] = (!String.IsNullOrEmpty(rating)) ? Convert.ToString(rating) : "";
                        dtEquipLoc.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptRoofDetails.DataSource = dtEquipLoc;
                        rptRoofDetails.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtEquipLoc", dtEquipLoc);
                    }
                }
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtEquipLoc = new DataTable();
                dtEquipLoc = (DataTable)Session["dtEquipLoc"];

                string iname = ((TextBox)e.Item.FindControl("edEquipment")).Text;

                foreach (DataRow dr in dtEquipLoc.Rows)
                {
                    if (dr["Equipment"].ToString() == iname)
                    {
                        dtEquipLoc.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }

                //DataRow[] rows;
                //rows = dtEquipLoc.Select("pkID=" + e.CommandArgument.ToString());
                //foreach (DataRow r in rows)
                //    r.Delete();

                rptEquipmentLocationDetails.DataSource = dtEquipLoc;
                rptEquipmentLocationDetails.DataBind();

                Session.Add("dtEquipLoc", dtEquipLoc);
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
        }

        protected void rptEquipmentLocationDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
        protected void ediEquipmentLocation_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edpkID = (HiddenField)item.FindControl("edEquipID");
            TextBox edEquipment = (TextBox)item.FindControl("edEquipment");
            TextBox edDistance = (TextBox)item.FindControl("edDistance");
            TextBox edConnPossibility = (TextBox)item.FindControl("edConnPossibility");
            TextBox edClientRating = (TextBox)item.FindControl("edClientRating");
            // --------------------------------------------------------------------------

            DataTable dtEquipLoc = new DataTable();
            dtEquipLoc = (DataTable)Session["dtEquipLoc"];
            foreach (System.Data.DataColumn col in dtEquipLoc.Columns) col.ReadOnly = false;
            foreach (DataRow row in dtEquipLoc.Rows)
            {
                if (row.RowState.ToString() != "Deleted")
                {
                    if (row["pkID"].ToString() == edpkID.Value)
                    {
                        row.SetField("Equipment",  Convert.ToString(edEquipment.Text));
                        row.SetField("Distance", Convert.ToString(edDistance.Text));
                        row.SetField("ConnPossibility",  Convert.ToString(edConnPossibility.Text));
                        row.SetField("ClientRating",  Convert.ToString(edClientRating.Text));
                        row.AcceptChanges();
                    }
                }
            }
            rptEquipmentLocationDetails.DataSource = dtEquipLoc;
            rptEquipmentLocationDetails.DataBind();

            Session.Add("dtEquipLoc", dtEquipLoc);
        }
        [System.Web.Services.WebMethod]

        //-----------------------Bind System Availablity Details for Survey Report----------------
        public void BindSSRSysAvailablity(string SurveyID)
        {
            DataTable dtSysAvail = new DataTable();
            dtSysAvail = BAL.SiteSurveyReportMgmt.GetSSRSysAvailablity(SurveyID);
            rptSysAvailablityDetails.DataSource = dtSysAvail;
            rptSysAvailablityDetails.DataBind();
            Session.Add("dtSysAvail", dtSysAvail);
        }

        protected void rptSysAvailablityDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;

                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtSysAvail = new DataTable();
                    dtSysAvail = (DataTable)Session["dtSysAvail"];

                    if (dtSysAvail != null)
                    {

                        foreach (System.Data.DataColumn col in dtSysAvail.Columns) col.AllowDBNull = true;

                        Int64 cntRow = dtSysAvail.Rows.Count + 1;
                        DataRow dr = dtSysAvail.NewRow();

                        dr["pkID"] = cntRow;
                        string load = ((TextBox)e.Item.FindControl("txtLoadDesc")).Text;
                        string Cap = ((TextBox)e.Item.FindControl("txtCapacity")).Text;
                        string Val = ((TextBox)e.Item.FindControl("txtVoltage")).Text;
                        string Qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                        //dr["SurveyID"] = txtSurveyID.Text;
                        dr["LoadDesc"] = (!String.IsNullOrEmpty(load)) ? Convert.ToString(load) : "";
                        dr["Capacity"] = (!String.IsNullOrEmpty(Cap)) ? Convert.ToString(Cap) : "";
                        dr["Voltage"] = (!String.IsNullOrEmpty(Val)) ? Convert.ToString(Val) : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(Qty)) ? Convert.ToString(Qty) : "";
                        dtSysAvail.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptSysAvailablityDetails.DataSource = dtSysAvail;
                        rptSysAvailablityDetails.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtSysAvail", dtSysAvail);
                    }
                }
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtSysAvail = new DataTable();
                dtSysAvail = (DataTable)Session["dtSysAvail"];

                string iname = ((TextBox)e.Item.FindControl("edLoadDesc")).Text;

                foreach (DataRow dr in dtSysAvail.Rows)
                {
                    if (dr["LoadDesc"].ToString() == iname)
                    {
                        dtSysAvail.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }

                rptSysAvailablityDetails.DataSource = dtSysAvail;
                rptSysAvailablityDetails.DataBind();

                Session.Add("dtSysAvail", dtSysAvail);
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
        }

        protected void rptSysAvailablityDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
        protected void editSystemAvailablity_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edpkID = (HiddenField)item.FindControl("edLoadID");
            TextBox edLoadDesc = (TextBox)item.FindControl("edLoadDesc");
            TextBox edCapacity = (TextBox)item.FindControl("edCapacity");
            TextBox edVoltage = (TextBox)item.FindControl("edVoltage");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            // --------------------------------------------------------------------------

            DataTable dtSysAvail = new DataTable();
            dtSysAvail = (DataTable)Session["dtSysAvail"];
            foreach (System.Data.DataColumn col in dtSysAvail.Columns) col.ReadOnly = false;
            foreach (DataRow row in dtSysAvail.Rows)
            {
                if (row.RowState.ToString() != "Deleted")
                {
                    if (row["pkID"].ToString() == edpkID.Value)
                    {
                        row.SetField("LoadDesc", Convert.ToString(edLoadDesc.Text));
                        row.SetField("Capacity", Convert.ToString(edCapacity.Text));
                        row.SetField("Voltage", Convert.ToString(edVoltage.Text));
                        row.SetField("Quantity", Convert.ToString(edQuantity.Text));
                        row.AcceptChanges();
                    }
                }
            }
            rptSysAvailablityDetails.DataSource = dtSysAvail;
            rptSysAvailablityDetails.DataBind();

            Session.Add("dtSysAvail", dtSysAvail);
        }

       
        
        //-----------------------Bind Required Engineering Details for Survey Report----------------
        public void BindSSRRequiredEngDetail(string SurveyID)
        {
            DataTable dtReq = new DataTable();
            dtReq = BAL.SiteSurveyReportMgmt.GetSSRRequiredDetails(SurveyID);
            rptRequiredEngDetails.DataSource = dtReq;
            rptRequiredEngDetails.DataBind();
            Session.Add("dtReq", dtReq);
        }

        protected void rptRequiredEngDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;

                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtReq = new DataTable();
                    dtReq = (DataTable)Session["dtReq"];

                    if (dtReq != null)
                    {

                        foreach (System.Data.DataColumn col in dtReq.Columns) col.AllowDBNull = true;

                        Int64 cntRow = dtReq.Rows.Count + 1;
                        DataRow dr = dtReq.NewRow();

                        dr["pkID"] = cntRow;
                        string desc = ((TextBox)e.Item.FindControl("txtDesc")).Text;
                        string Rem = ((TextBox)e.Item.FindControl("txtRemarks")).Text;

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        //dr["SurveyID"] = txtSurveyID.Text;
                        dr["Description"] = (!String.IsNullOrEmpty(desc)) ? Convert.ToString(desc) : "";
                        dr["Remarks"] = (!String.IsNullOrEmpty(Rem)) ? Convert.ToString(Rem) : "";
                        dtReq.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptRequiredEngDetails.DataSource = dtReq;
                        rptRequiredEngDetails.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtReq", dtReq);
                    }
                }
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtReq = new DataTable();
                dtReq = (DataTable)Session["dtReq"];

                string iname = ((TextBox)e.Item.FindControl("edDesc")).Text;

                foreach (DataRow dr in dtReq.Rows)
                {
                    if (dr["Description"].ToString() == iname)
                    {
                        dtReq.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }

                rptRequiredEngDetails.DataSource = dtReq;
                rptRequiredEngDetails.DataBind();

                Session.Add("dtReq", dtReq);
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
        }

        protected void rptRequiredEngDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }
        protected void editRequiredEng_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edpkID = (HiddenField)item.FindControl("edDescID");
            TextBox edDesc = (TextBox)item.FindControl("edDesc");
            TextBox edRemarks = (TextBox)item.FindControl("edRemarks");
            // --------------------------------------------------------------------------

            DataTable dtReq = new DataTable();
            dtReq = (DataTable)Session["dtReq"];
            foreach (System.Data.DataColumn col in dtReq.Columns) col.ReadOnly = false;
            foreach (DataRow row in dtReq.Rows)
            {
                if (row.RowState.ToString() != "Deleted")
                {
                    if (row["pkID"].ToString() == edpkID.Value)
                    {
                        row.SetField("Description", Convert.ToString(edDesc.Text));
                        row.SetField("Remarks", Convert.ToString(edRemarks.Text));
                        row.AcceptChanges();
                    }
                }
            }
            rptRequiredEngDetails.DataSource = dtReq;
            rptRequiredEngDetails.DataBind();

            Session.Add("dtReq", dtReq);
        }

    }
}