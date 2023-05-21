using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProjectSheet : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageMode = (!String.IsNullOrEmpty(Request.QueryString["mode"])) ? Request.QueryString["mode"] : "";
            // -----------------------------------------
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 10;
                Session["dtAss"] = null;
                hdnSerialKey.Value = Session["SerialKey"].ToString();
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
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

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ProjectSheet> lstEntity = new List<Entity.ProjectSheet>();
                // ----------------------------------------------------
                lstEntity = BAL.ProjectSheetMgmt.GetProjectSheetList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProjectNo.Text = lstEntity[0].ProjectSheetNo;
                txtProjectDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].ProjectSheetDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                txtSiteNo.Text = lstEntity[0].SiteNo;
                txtAddress.Text = lstEntity[0].SiteAddress;
                txtArea.Text = lstEntity[0].SiteArea;
                drpCity.SelectedValue = lstEntity[0].SiteCityID.ToString();
                drpState.SelectedValue = lstEntity[0].SiteStateID.ToString();
                drpCountry.SelectedValue = lstEntity[0].SiteCountryID.ToString();
                txtPincode.Text = lstEntity[0].SitePincode;
                txtRemarks.Text = lstEntity[0].Remarks;

                txtSiteNo.Focus();

                BindProjectProductDetail(txtProjectNo.Text);
                BindProjectAssemblyDetail(txtProjectNo.Text);
            }
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value),Session["LoginUserID"].ToString(),1,111,out totrec);
            if (lstCust.Count > 0)
            {
                txtAddress.Text = lstCust[0].Address;
                txtArea.Text = lstCust[0].Area;
                drpCountry.SelectedValue = lstCust[0].CountryCode != "0" ? lstCust[0].CountryCode : "IND";
                drpCountry_SelectedIndexChanged(null, null);
                drpState.SelectedValue = lstCust[0].StateCode;
                drpState_SelectedIndexChanged(null, null);
                drpCity.SelectedValue = lstCust[0].CityCode;
                txtPincode.Text = lstCust[0].Pincode;
            }
        }
        public void ClearAllField()
        {
            hdnCustomerID.Value = "";
            Session["dtAss"]=null;
            txtCustomerName.Text = "";
            txtProjectDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtSiteNo.Text = "";
            txtAddress.Text = "";
            txtArea.Text = "";
            txtRemarks.Text = "";
            drpCity.Items.Clear();
            drpState.Items.Clear();
            drpCountry.ClearSelection();
            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);
            }
            //Entity.Authenticate objAuth = new Entity.Authenticate();
            //objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            //List<Entity.CompanyProfile> lstCSC = new List<Entity.CompanyProfile>();
            //lstCSC = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

            //drpState.ClearSelection();
            //if (drpState.Items.FindByText(lstCSC[0].StateName) != null)
            //{
            //    drpState.Items.FindByText(lstCSC[0].StateName).Selected = true;
            //    drpState_SelectedIndexChanged(null, null);
            //}
            //drpCity.ClearSelection();
            //if (drpCity.Items.FindByText(lstCSC[0].CityName) != null)
            //{
            //    drpCity.Items.FindByText(lstCSC[0].CityName).Selected = true;
            //}

            BindProjectProductDetail("");
            BindProjectAssemblyDetail("");


            txtPincode.Text = "";
            btnSave.Disabled = false;
            txtCustomerName.Focus();

            
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
            drpCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Country --", ""));

            drpState.ClearSelection();
            List<Entity.State> lstState= new List<Entity.State>();
            lstState = BAL.StateMgmt.GetStateList();
            drpState.DataSource = lstState;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Country --", ""));

            drpCity.ClearSelection();
            List<Entity.City> lstCity = new List<Entity.City>();
            lstCity = BAL.CityMgmt.GetCityList();
            drpCity.DataSource = lstCity;
            drpCity.DataValueField = "CityCode";
            drpCity.DataTextField = "CityName";
            drpCity.DataBind();
            drpCity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Country --", ""));
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
                    drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All State --", "0"));
                    drpState.Focus();
                }

            }
            if (drpCountry.SelectedValue == "0" || drpCountry.SelectedValue == "")
            {
                drpState.Items.Clear();
                drpCity.Items.Clear();
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
                    drpCity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All City --", "0"));
                    drpCity.Focus();
                }

            }
            if (drpState.SelectedValue == "0" || drpState.SelectedValue == "")
            {
                drpCity.Items.Clear();
            }
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0, ReturnCode2 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnMsg2 = "", ReturnProjectNo = "";

            string strErr = "";
            _pageValid = true;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];

            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(txtProjectDate.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtProjectDate.Text))
                    strErr += "<li>" + "Inward Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Selection is required." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        Entity.ProjectSheet objEntity = new Entity.ProjectSheet();
                        objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.ProjectSheetNo = txtProjectNo.Text;
                        objEntity.ProjectSheetDate = Convert.ToDateTime(txtProjectDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.SiteNo = (!String.IsNullOrEmpty(txtSiteNo.Text) ? txtSiteNo.Text : "");
                        objEntity.SiteAddress = (!String.IsNullOrEmpty(txtAddress.Text) ? txtAddress.Text : "");
                        objEntity.SiteArea = (!String.IsNullOrEmpty(txtArea.Text) ? txtArea.Text : "");
                        objEntity.SiteCityID = (!String.IsNullOrEmpty(drpCity.SelectedValue) ? Convert.ToInt64(drpCity.SelectedValue) : 0);
                        objEntity.SiteStateID = (!String.IsNullOrEmpty(drpState.SelectedValue) ? Convert.ToInt64(drpState.SelectedValue) : 0);
                        objEntity.SiteCountryID = (!String.IsNullOrEmpty(drpCountry.SelectedValue) ? drpCountry.SelectedValue : "");
                        objEntity.SitePincode = (!String.IsNullOrEmpty(txtPincode.Text) ? txtPincode.Text : "");
                        objEntity.Remarks = (!String.IsNullOrEmpty(txtRemarks.Text) ? txtRemarks.Text : "");
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.ProjectSheetMgmt.AddUpdateProjectSheet(objEntity, out ReturnCode, out ReturnMsg, out ReturnProjectNo);
                        if (String.IsNullOrEmpty(ReturnProjectNo) && !String.IsNullOrEmpty(txtProjectNo.Text))
                        {
                            txtProjectNo.Text = ReturnProjectNo;
                        }
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnProjectNo + " " + ReturnMsg : ReturnMsg) + "</li>";

                        BAL.ProjectSheetMgmt.DeleteProjectDetailsBySheetNo(txtProjectNo.Text, out ReturnCode, out ReturnMsg);
                        if (ReturnCode > 0)
                        {
                            Entity.Project_Detail objQuotDet = new Entity.Project_Detail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.ProjectSheetNo = ReturnProjectNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.SysCapacity = Convert.ToDecimal(dr["SysCapacity"]);
                                objQuotDet.PanalWattage = Convert.ToString(dr["PanalWattage"]);
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.ProjectSheetMgmt.AddUpdateProject_Detail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                            }
                            
                        }

                        BAL.ProjectSheetMgmt.DeleteAssemblyByProjectNo(txtProjectNo.Text, out ReturnCode, out ReturnMsg);
                        if (ReturnCode > 0)
                        {
                            Entity.ProjectAssembly objQuotDet = new Entity.ProjectAssembly();

                            foreach (DataRow dr in dtAssembly.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.ProjectSheetNo = ReturnProjectNo;
                                objQuotDet.FinishedProductID = Convert.ToInt64(dr["FinishProductID"]);
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.Remarks = (!String.IsNullOrEmpty(txtRemarks.Text) ? txtRemarks.Text : "");
                                objQuotDet.ProductMake = Convert.ToInt64(dr["BrandID"].ToString());
                                objQuotDet.ProductMakeName = dr["BrandName"].ToString();
                               
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.ProjectSheetMgmt.AddUpdateProject_Assembly(objQuotDet, out ReturnCode2, out ReturnMsg2);
                            }
                            if (ReturnCode2 > 0)
                            {
                                Session.Remove("dtAssembly");
                            }

                        }
                        btnSave.Disabled = true;
                    }
                    else
                    {
                        strErr = "<li>" + "Atleast One Item is required to save Project Information !" + "</li>";
                    }
                }
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
            }
        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
        }
        public void OnlyViewControls()
        {
            txtCustomerName.ReadOnly = true;
            drpCity.Attributes.Add("disabled", "disabled");
            drpState.Attributes.Add("disabled", "disabled");
            drpCountry.Attributes.Add("disabled", "disabled");
            txtAddress.ReadOnly = true;
            txtArea.ReadOnly = true;
            txtPincode.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        protected void rptProject_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    HiddenField hdnProductID = (HiddenField)e.Item.FindControl("hdnProductIDNew");
                    TextBox txtProductName = (TextBox)e.Item.FindControl("txtProductName");
                    TextBox txtUnit = (TextBox)e.Item.FindControl("txtUnit");
                    TextBox txtSysCapacity = (TextBox)e.Item.FindControl("txtSysCapacity");
                    TextBox txtPanalWattage = (TextBox)e.Item.FindControl("txtPanalWattage");



                    if (String.IsNullOrEmpty(hdnProductID.Value) || String.IsNullOrEmpty(txtPanalWattage.Text)
                        || String.IsNullOrEmpty(txtSysCapacity.Text) || String.IsNullOrEmpty(txtUnit.Text))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnProductID.Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(txtSysCapacity.Text))
                            strErr += "<li>" + "Capacity is required." + "</li>";

                        if (String.IsNullOrEmpty(txtPanalWattage.Text))
                            strErr += "<li>" + "Panal Wattage is required." + "</li>";

                        if (String.IsNullOrEmpty(txtUnit.Text))
                            strErr += "<li>" + "Unit is required." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {



                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + hdnProductID.Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                            return;
                        }

                        Int64 cntRow = dtDetail.Rows.Count + 1;
                        DataRow dr = dtDetail.NewRow();
                        string icode = hdnProductID.Value;
                        string iname = txtProductName.Text;
                        string syscap = ((TextBox)e.Item.FindControl("txtSysCapacity")).Text;
                        string panalwatt = ((TextBox)e.Item.FindControl("txtPanalWattage")).Text;
                        string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;

                        dr["pkID"] = cntRow;
                        dr["ProjectSheetNo"] = txtProjectNo.Text;
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                        dr["SysCapacity"] = (!String.IsNullOrEmpty(syscap)) ? Convert.ToDecimal(syscap) : 0;
                        dr["PanalWattage"] = (!String.IsNullOrEmpty(panalwatt)) ? panalwatt : "";
                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetail", dtDetail);
                        // ---------------------------------------------------------------
                        rptProject.DataSource = dtDetail;
                        rptProject.DataBind();
                        // ---------------------------------------------------------------
                    }
                    btnSave.Focus();
                }
                if (!string.IsNullOrEmpty(strErr))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                }
            }
            // --------------------------------------------------------------------------
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int ReturnCode = 0;
                    string ReturnMsg = "";
                    Int64 tmpRow;

                    //DataTable dtDetail = new DataTable();
                    //dtDetail = (DataTable)Session["dtDetail"];
                    // --------------------------------- Delete Record
                    Int64 iname = Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnProductID_Grid")).Value);

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (Convert.ToInt64(dr["pkID"]) == iname)
                        {
                            dtDetail.Rows.Remove(dr);
                            break;
                        }
                    }

                    rptProject.DataSource = dtDetail;
                    rptProject.DataBind();

                    Session.Add("dtDetail", dtDetail);
                }
            }
        }
        public void BindProjectProductDetail(string pProjectNo)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = BAL.ProjectSheetMgmt.GetProjectProductDetail(pProjectNo);
            rptProject.DataSource = dtDetail;
            rptProject.DataBind();
            Session.Add("dtDetail", dtDetail);
        }

        public void BindProjectAssemblyDetail(string pProjectNo)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            
            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtDetail"];
            Int64 ProductId = 0;
            if (dtDetail != null)
                if(dtDetail.Rows.Count > 0)
                    ProductId = Convert.ToInt64(dtDetail.Rows[0]["ProductID"]);
            dtAssembly = BAL.ProjectSheetMgmt.GetProjectAssemblyDetail(pProjectNo,ProductId);
            rptAssembly.DataSource = dtAssembly;
            rptAssembly.DataBind();
            Session.Add("dtAssembly", dtAssembly);
        }

        protected void rptProject_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (dtDetail != null)
                {
                    if(dtDetail.Rows.Count < 1)
                        e.Item.Visible = true;
                    else
                        e.Item.Visible = false;
                }
            }

        }

        protected void rptAssembly_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];

            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    HiddenField hdnProductID = (HiddenField)e.Item.FindControl("hdnProductID2");
                    TextBox txtProductName = (TextBox)e.Item.FindControl("txtProductName2");
                    TextBox txtUnit = (TextBox)e.Item.FindControl("txtUnit2");
                    TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity2");
                    DropDownList drpBrandName = (DropDownList)e.Item.FindControl("drpBrandName");



                    if (String.IsNullOrEmpty(hdnProductID.Value) || String.IsNullOrEmpty(txtQuantity.Text) || String.IsNullOrEmpty(txtUnit.Text) || String.IsNullOrEmpty(drpBrandName.SelectedValue))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnProductID.Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(txtUnit.Text))
                            strErr += "<li>" + "Unit is required." + "</li>";

                        if (String.IsNullOrEmpty(txtQuantity.Text))
                            strErr += "<li>" + "Quantity is required." + "</li>";

                        if (String.IsNullOrEmpty(drpBrandName.SelectedValue))
                            strErr += "<li>" + "Brand Selection is required." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + hdnProductID.Value + "";
                        DataRow[] foundRows = dtAssembly.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                            return;
                        }

                        Int64 cntRow = dtAssembly.Rows.Count + 1;
                        DataRow dr = dtAssembly.NewRow();
                        string icode = hdnProductID.Value;
                        string iname = txtProductName.Text;
                        Decimal quantity = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtQuantity2")).Text);
                        Int64 brand = Convert.ToInt64(((DropDownList)e.Item.FindControl("drpBrandName")).SelectedValue);
                        string brandName = ((DropDownList)e.Item.FindControl("drpBrandName")).SelectedItem.ToString();
                        string unit = ((TextBox)e.Item.FindControl("txtUnit2")).Text;

                        dr["pkID"] = cntRow;
                        //dr["ProjectSheetNo"] = txtProjectNo.Text;
                        dr["FinishProductID"] = (!String.IsNullOrEmpty(dtDetail.Rows[0]["ProductID"].ToString())) ? Convert.ToInt64(dtDetail.Rows[0]["ProductID"].ToString()) : 0;
                        dr["FinishProductName"] = (!String.IsNullOrEmpty(dtDetail.Rows[0]["ProductName"].ToString())) ? dtDetail.Rows[0]["ProductName"].ToString() : "";
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(quantity.ToString())) ? Convert.ToDecimal(quantity) : 0;
                        dr["BrandID"] = (!String.IsNullOrEmpty(brand.ToString())) ? brand : 0;
                        dr["BrandName"] = (!String.IsNullOrEmpty(brand.ToString())) ? brand : 0;
                        dtAssembly.Rows.Add(dr);
                        Session.Add("dtAssembly", dtAssembly);
                        // ---------------------------------------------------------------
                        rptAssembly.DataSource = dtAssembly;
                        rptAssembly.DataBind();
                        // ---------------------------------------------------------------
                    }
                    btnSave.Focus();
                }
                if (!string.IsNullOrEmpty(strErr))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                }
            }
            // --------------------------------------------------------------------------
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int ReturnCode = 0;
                    string ReturnMsg = "";
                    Int64 tmpRow;

                    //DataTable dtDetail = new DataTable();
                    //dtDetail = (DataTable)Session["dtDetail"];
                    // --------------------------------- Delete Record
                    Int64 iname = Convert.ToInt64(((HiddenField)e.Item.FindControl("hdnProductIDNew")).Value);

                    foreach (DataRow dr in dtAssembly.Rows)
                    {
                        if (Convert.ToInt64(dr["pkID"]) == iname)
                        {
                            dtAssembly.Rows.Remove(dr);
                            break;
                        }
                    }

                    rptAssembly.DataSource = dtAssembly;
                    rptAssembly.DataBind();

                    Session.Add("dtAssembly", dtAssembly);
                }
            }
        }

        protected void rptAssembly_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Control edSender = (Control)sender;
            //var item = (RepeaterItem)edSender.NamingContainer;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpBrandName"));
                ddl.DataSource = BAL.BrandMgmt.GetBrandList();
                ddl.DataValueField = "pkId";
                ddl.DataTextField = "BrandName";
                ddl.DataBind();
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Brand --", ""));

                if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnBrandID"));
                    if (!String.IsNullOrEmpty(tmpField.Value))
                        ddl.SelectedValue = tmpField.Value;
                }
            }
            
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            Int32 totalrecord = 0;
            DataTable dtAss = new DataTable();
            Control rptFootCtrl = rptProject.Controls[rptProject.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductIDNew"));
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
                TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));

                List<Entity.Products> lstEntity = new List<Entity.Products>();

                if (!String.IsNullOrEmpty(hdnProductID.Value))
                    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (lstEntity.Count > 0)
                {
                    txtUnit.Text = lstEntity[0].Unit;

                    List<Entity.ProductDetailCard> lstAssEntity = new List<Entity.ProductDetailCard>();
                    lstAssEntity = BAL.ProductMgmt.GetProductDetailList(0, Convert.ToInt64(hdnProductID.Value), Session["LoginUserID"].ToString());

                    dtAss = PageBase.ConvertListToDataTable(lstAssEntity);

                    if(dtAss != null)
                    {
                        Session.Add("dtAssembly", dtAss);
                        rptAssembly.DataSource = dtAss;
                        rptAssembly.DataBind();
                    }


                    //if(lstAssEntity.Count > 0)
                    //{ 
                    //    if (Session["dtAss"] != null)
                    //    {
                    //        dtAss = (DataTable)Session["dtAss"];

                    //        DataRow drTemp = dtAss.NewRow();
                    //        drTemp["FinishProductID"] = lstAssEntity[0].FinishProductID ;
                    //        drTemp["FinishProductName"] = !String.IsNullOrEmpty(lstAssEntity[0].FinishProductName) ? lstAssEntity[0].FinishProductName : "";
                    //        drTemp["AssemblyID"] = lstAssEntity[0].AssemblyID;
                    //        drTemp["AssemblyName"] = !String.IsNullOrEmpty(lstAssEntity[0].AssemblyName) ? lstAssEntity[0].AssemblyName : "";
                    //        drTemp["AssemblyQty"] = lstAssEntity[0].AssemblyQty;
                    //        drTemp["RequiredQty"] = lstAssEntity[0].RequiredQty;
                    //        drTemp["ClosingSTK"] = lstAssEntity[0].ClosingSTK;
                    //        drTemp["BalanceQty"] = lstAssEntity[0].BalanceQty;
                    //        drTemp["BalanceStatus"] = !String.IsNullOrEmpty(lstAssEntity[0].BalanceStatus) ? lstAssEntity[0].BalanceStatus : "";
                    //        dtAss.Rows.Add(drTemp);
                    //    }
                    //    else
                    //    {
                    //        dtAss = PageBase.ConvertListToDataTable(lstAssEntity);
                    //    }

                    //    dtAss.AcceptChanges();
                    //    Session.Add("dtAss", dtAss);

                    //    Control rptFootCtrlAss = rptAssembly.Controls[rptAssembly.Controls.Count - 1].Controls[0];
                    //    HiddenField hdnProductIDAss = ((HiddenField)rptFootCtrlAss.FindControl("hdnProductID2"));
                    //    TextBox ProductNameAss = ((TextBox)rptFootCtrlAss.FindControl("txtProductName2"));
                    //    TextBox ProductQtyAss = ((TextBox)rptFootCtrlAss.FindControl("txtQuantity2"));

                    //    for (int i = 0; i <= dtAss.Rows.Count; i++)
                    //    {
                    //        hdnProductIDAss.Value = dtAss.Rows[i]["AssemblyID"].ToString();
                    //        ProductNameAss.Text = dtAss.Rows[i]["AssemblyName"].ToString();
                    //        ProductQtyAss.Text = dtAss.Rows[i]["AssemblyQty"].ToString();
                    //    }
                    //}
                }
            }
        }
        [System.Web.Services.WebMethod]
        public static string DeleteProjectSheet(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProjectSheetMgmt.DeleteProjectSheet(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
        [System.Web.Services.WebMethod]
        public static string GetProjectNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetProjectSheetNo(pkID);
            return tempVal;
        }
        [System.Web.Services.WebMethod]
        public static void GenerateProjectSheet(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblDetail = new PdfPTable(5);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            //PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Proforma");

            ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                }
            }

            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);

            //Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            //pdfDoc.SetMargins(30, 30, 40, 0);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
            // -------------------------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.ProjectSheet> lstQuot = new List<Entity.ProjectSheet>();
            lstQuot = BAL.ProjectSheetMgmt.GetProjectSheetList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //--------------------------------------------------------------------------------------------------------------
            //List<Entity.PurchaseOrder> lstExp = new List<Entity.PurchaseOrder>();
            //lstExp = BAL.PurchaseOrderMgmt.GetSalesOrderExportList(pkID, lstQuot[0].OrderNo, HttpContext.Current.Session["LoginUserID"].ToString());
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.ProjectSheetMgmt.GetProjectProductDetail(lstQuot[0].ProjectSheetNo);
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItemAss = new DataTable();
            if (lstQuot.Count > 0)
                dtItemAss = BAL.ProjectSheetMgmt.GetProjectAssemblyDetail(lstQuot[0].ProjectSheetNo,Convert.ToInt64(dtItem.Rows[0]["ProductID"]));
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);
            //-------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                
                //------------------Invoice Details----------------------

                PdfPTable tblInvoiceD = new PdfPTable(4);
                int[] column_tblInvoiceD = { 20, 35, 15, 30 };
                tblInvoiceD.SetWidths(column_tblInvoiceD);

                tblInvoiceD.AddCell(pdf.setCell("CLIENT NAME", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell("SITE ADDRESS", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].SiteAddress, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                tblInvoiceD.AddCell(pdf.setCell("SITE NUMBER", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].SiteNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell("SYSTEM CAPACITY", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(dtItem.Rows[0]["SysCapacity"].ToString() + " KW ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell("DATE", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].ProjectSheetDate.ToString("dd/MM/yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell("PANNEL WATTAGE", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(dtItem.Rows[0]["PanalWattage"].ToString() + " WP ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell("SHEET NO", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].ProjectSheetNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));



                tblMember.SpacingBefore = 0f;
                tblMember.LockedWidth = true;
                tblMember.HorizontalAlignment = Element.ALIGN_CENTER;

                tblMember.AddCell(pdf.setCell(tblInvoiceD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail


                //var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                int[] column_tblNested = { 10, 40, 16, 18, 16 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.KeepTogether = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("SR" + "\n" + "NO", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("PRODUCT", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Quantity", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("UNIT", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("MAKE", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));

                int totalSpecLines = 0;
                decimal totAmount = 0, taxAmount = 0, netAmount = 0, amount = 0;
                int totalRowCount = 0;
                Decimal TotalQty = 0;
                for (int i = 0; i < dtItemAss.Rows.Count; i++)
                {
                   
                    ////-------------------------------------------------------------------
                    //string tmpHSNCode = "";
                    //List<Entity.Products> lstProd = new List<Entity.Products>();
                    ////if (lstProd.Count > 0)
                    ////{
                    //Int64 tmpIcode = Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString());
                    //lstProd = BAL.ProductMgmt.GetProductList(tmpIcode, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                    //if (lstProd.Count > 0)
                    //    tmpHSNCode = lstProd[0].HSNCode.ToString();
                    ////}
                    //// ------------------------------------------------------------------

                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItemAss.Rows[i]["FinishProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItemAss.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItemAss.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItemAss.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItemAss.Rows[i]["BrandName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                    {

                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));

                    }
                }



                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                #endregion


                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(1);
                int[] column_tblFootDetail = { 100 };
                tblFootDetail.SetWidths(column_tblFootDetail);


                //----------------------Dynamic Esignature---------------------
                PdfPTable tblESignature = new PdfPTable(3);
                int[] column_tblESignature = { 30,30,40 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                
                tblESignature.AddCell(pdf.setCell("Prepared By", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell("Checked By", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell("Client Sign", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblESignature.AddCell(pdf.setCell(lstQuot[0].CreatedEmployeeName , pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                //PdfPTable tblsign = new PdfPTable(2);
                //int[] column_tblsign = { 50, 50 };
                //tblsign.SetWidths(column_tblsign);
                ////&& HttpContext.Current.Session["LoginUserID"].ToString().ToLower() == "admin" 
                //if (lstQuot[0].ApprovalStatus.ToLower() == "approved")
                //{
                //    tblsign.AddCell(pdf.setCell("For , " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    tblsign.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    tblsign.AddCell(pdf.setCell(tblESignature1, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //}
                //else
                //{
                //    tblsign.AddCell(pdf.setCell("For , " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    tblsign.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //}


                // ---------------------------------------------------
                int[] column_tblFooter = { 50, 50 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                //tblFooter.SplitLate = false;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                // tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                 #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].ProjectSheetNo.Replace("/", "-").ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();

            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Organization Name 
            //tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //pdfDoc.Add(tableHeader);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Footer
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);

            // >>>>>> Adding Quotation Header
            //tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            //pdfDoc.Add(tblSignOff);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }

    }
}