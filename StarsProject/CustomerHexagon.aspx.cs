using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class CustomerHexagon : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageMode = (!String.IsNullOrEmpty(Request.QueryString["mode"])) ? Request.QueryString["mode"] : "";
            hdnRILPrice.Value = BAL.CommonMgmt.GetConstant("RILPrice", 0, 1);
            //txtRILPrice.Text = hdnRILPrice.Value;
            hdnCustWisePro.Value = BAL.CommonMgmt.GetConstant("CustomerWiseProducts", 0, 1);
            hdnPageMode.Value = pageMode;
            setModeLayout(pageMode);
            // -----------------------------------------
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(BAL.CommonMgmt.GetConstant("PriceListWithCustomer", 0, Convert.ToInt64(Session["CompanyID"].ToString()))))
                    hdnIsPriceListCustomer.Value = BAL.CommonMgmt.GetConstant("PriceListWithCustomer", 0, Convert.ToInt64(Session["CompanyID"].ToString()));

                DataTable dtCustomer = new DataTable();
                Session.Add("dtCustomer", dtCustomer);
                DataTable dtProducts = new DataTable();
                Session.Add("dtProducts", dtProducts);
                Session["OldUserID"] = "";

                hdnSerialKey.Value = Session["SerialKey"].ToString();
                BindDropDown();
                // --------------------------------------------------------
                hdnHiddenControl.Value = BAL.CommonMgmt.GetPageHiddenControls("customer");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnCustomerID.Value = Request.QueryString["id"].ToString();

                    if (hdnCustomerID.Value == "0" || hdnCustomerID.Value == "")
                    {
                        ClearAllField();
                        hdnCustomerID.Value = "0";
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
            else
            {
                // ----------------------------------------------------------------------
                // Product Document Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (uploadDocument.PostedFile != null)
                {
                    if (uploadDocument.PostedFile.FileName.Length > 0)
                    {
                        if (uploadDocument.HasFile)
                        {
                            string filePath = uploadDocument.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext == ".pdf" || ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("CustomerDocs");
                                    string filesToDelete = @"Cust" + hdnCustomerID.Value + "-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "Cust" + hdnCustomerID.Value + "-" + filename1;
                                    String tmpFile = Server.MapPath("CustomerDocs/") + flname;
                                    uploadDocument.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtCustDocs = new DataTable();
                                    dtCustDocs = (DataTable)Session["dtCustDocs"];
                                    Int64 cntRow = 0;
                                    if (dtCustDocs != null)
                                    {
                                        cntRow = dtCustDocs.Rows.Count + 1;
                                        DataRow dr = dtCustDocs.NewRow();
                                        dr["pkID"] = cntRow;
                                        dr["FileName"] = flname;
                                        dr["Filetype"] = type;
                                        dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                        //dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                        dtCustDocs.Rows.Add(dr);
                                        Session.Add("dtCustDocs", dtCustDocs);
                                        // ---------------------------------------------------------------
                                        rptEmpDocs.DataSource = dtCustDocs;
                                        rptEmpDocs.DataBind();
                                    }
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
            }
            // -----------------------------------------------
            if (!PageBase.checkGeneralMenuAccess("customer"))
            {
                btnPanel.Style.Add("display", "none");
                divRestriction.Style.Add("display", "block");
                divRestriction.InnerText = "Sorry ! You Can't Save Customer. Customer Module Rights Restricted For Your Role.";
                //btnSave.Style.Add("display", "none");
                //btnSaveEmail.Style.Add("display", "none");
                //btnReset.Style.Add("display", "none");
            }
        }

        public void OnlyViewControls()
        {
            divFollowUp.Visible = false;
            txtCustomerName.ReadOnly = true;
            drpCustomerType.Attributes.Add("disabled", "disabled");
            //drpOrgType.Attributes.Add("disabled", "disabled");
            //drpReportTo.Attributes.Add("disabled", "disabled");
            drpPriceList.Attributes.Add("disabled", "disabled");

            chkBlock.Enabled = false;

            drpCity.Attributes.Add("disabled", "disabled");

            txtGSTNo.ReadOnly = true;
            txtPANNo.ReadOnly = true;
            txtCINNo.ReadOnly = true;

            txtAddress.ReadOnly = true;
            txtArea.ReadOnly = true;
            //drpCity.Attributes.Add("disabled", "disabled");
            //drpState.Attributes.Add("disabled", "disabled");
            //drpCountry.Attributes.Add("disabled", "disabled");
            txtPincode.ReadOnly = true;

            txtAddress1.ReadOnly = true;
            txtArea1.ReadOnly = true;
            //drpCity1.Attributes.Add("disabled", "disabled");
            //drpState1.Attributes.Add("disabled", "disabled");
            //drpCountry1.Attributes.Add("disabled", "disabled");
            txtPincode1.ReadOnly = true;

            txtContactNo1.ReadOnly = true;
            txtContactNo2.ReadOnly = true;
            txtEmailAddress.ReadOnly = true;
            txtWebsite.ReadOnly = true;

            //txtOpening.ReadOnly = true;
            txtDebit.ReadOnly = true;
            txtCredit.ReadOnly = true;
            txtClosing.ReadOnly = true;
            txtCrLimit.ReadOnly = true;
            txtCrDays.ReadOnly = true;

            txtNextFollowupDate.ReadOnly = true;
            txtMeetingNotes.ReadOnly = true;
            drpFollowupType.Attributes.Add("disabled", "disabled");
            txtRemarks.ReadOnly = true;

            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;

            txtBirthDate.ReadOnly = true;
            txtAnniversaryDate.ReadOnly = true;

            drpCustomerSource.Attributes.Add("disabled", "disabled");
        }

        public void setModeLayout(string pMode)
        {

            // -------------------------------------------
            //if (pMode.ToLower() == "vendor")
            //{
            //    drpCustomerType.SelectedValue = "Customer";
            //}
            //// -------------------------------------------
            //if (pMode.ToLower() == "vendor")
            //{
            //    //pageHeader.InnerHtml = "Manage Supplier/Vendor";
            //    lblCustomerName.InnerText = "Supplier/Vendor Name";
            //    drpCustomerType.SelectedValue = "Supplier";
            //}

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

            drpCountry1.ClearSelection();
            drpCountry1.DataSource = lstCountry;
            drpCountry1.DataValueField = "CountryCode";
            drpCountry1.DataTextField = "CountryName";
            drpCountry1.DataBind();
            drpCountry1.Items.Insert(0, new ListItem("-- All Country --", ""));
            //-------------------State List----------------------------------------

            //List<Entity.State> lstState = new List<Entity.State>();
            //lstState = BAL.StateMgmt.GetStateList();
            //drpState.DataSource = lstState;
            //drpState.DataValueField = "StateCode";
            //drpState.DataTextField = "StateName";
            //drpState.DataBind();
            //drpState.Items.Insert(0, new ListItem("-- All State --", ""));

            //List<Entity.State> lstState1 = new List<Entity.State>();
            //lstState1 = BAL.StateMgmt.GetStateList();
            //drpState1.DataSource = lstState1;
            //drpState1.DataValueField = "StateCode";
            //drpState1.DataTextField = "StateName";
            //drpState1.DataBind();
            //drpState1.Items.Insert(0, new ListItem("-- All State --", ""));

            // ---------------- Customer Category List -------------------------------------
            List<Entity.CustomerCategory> lstCustCat = new List<Entity.CustomerCategory>();
            lstCustCat = BAL.CustomerCategoryMgmt.GetCustomerCategoryList();
            drpCustomerType.DataSource = lstCustCat;
            drpCustomerType.DataValueField = "CategoryName";
            drpCustomerType.DataTextField = "CategoryName";
            drpCustomerType.DataBind();
            drpCustomerType.Items.Insert(0, new ListItem("-- All Category --", ""));

            // ---------------- Price List -------------------------------------
            List<Entity.PriceList> lstPriceList = new List<Entity.PriceList>();
            lstPriceList = BAL.PriceListMgmt.GetPriceList(0, Session["LoginUserID"].ToString());
            drpPriceList.DataSource = lstPriceList;
            drpPriceList.DataValueField = "pkID";
            drpPriceList.DataTextField = "PriceListName";
            drpPriceList.DataBind();
            drpPriceList.Items.Insert(0, new ListItem("-- Select --", ""));

            // ---------------- Designation List -------------------------------------
            drpContactDesigCode1.DataSource = BindDesignationList();
            drpContactDesigCode1.DataValueField = "DesigCode";
            drpContactDesigCode1.DataTextField = "Designation";
            drpContactDesigCode1.DataBind();
            drpContactDesigCode1.Items.Insert(0, new ListItem("-- Select --", "0"));

            // ---------------- Followup Type -------------------------------------
            List<Entity.InquiryStatus> lstOrgDept22 = new List<Entity.InquiryStatus>();
            lstOrgDept22 = BAL.InquiryStatusMgmt.GetInquiryStatusList("Followup");
            drpFollowupType.DataSource = lstOrgDept22;
            drpFollowupType.DataValueField = "pkID";
            drpFollowupType.DataTextField = "InquiryStatusName";
            drpFollowupType.DataBind();
            drpFollowupType.Items.Insert(0, new ListItem("-- Select --", ""));

            // ---------------- Customer Source  -------------------------------------
            List<Entity.InquiryStatus> lstSource = new List<Entity.InquiryStatus>();
            lstSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquirySource");
            drpCustomerSource.DataSource = lstSource;
            drpCustomerSource.DataValueField = "pkID";
            drpCustomerSource.DataTextField = "InquiryStatusName";
            drpCustomerSource.DataBind();
            drpCustomerSource.Items.Insert(0, new ListItem("-- Select --", ""));

        }

        public void BindContacts(Int64 pCustomerID)
        {
            DataTable dtCustomer1 = new DataTable();
            dtCustomer1 = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(pCustomerID);
            rptContacts.DataSource = dtCustomer1;
            rptContacts.DataBind();
            Session["dtCustomer"] = dtCustomer1;
        }

        public List<Entity.Designations> BindDesignationList()
        {
            // ---------------- Designation List  -------------------------------------
            List<Entity.Designations> lstDesig = new List<Entity.Designations>();
            lstDesig = BAL.DesignationMgmt.GetDesignationList();
            return lstDesig;
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        protected void rptContacts_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpContactDesigCode0"));
                ddl.DataSource = BindDesignationList();
                ddl.DataValueField = "DesigCode";
                ddl.DataTextField = "Designation";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("-- Select --", ""));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnDesigCode"));
                if (!String.IsNullOrEmpty(tmpField.Value))
                    ddl.SelectedValue = tmpField.Value;
            }
        }

        protected void rptContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Save")
            {
                DateTime cdt = DateTime.Now;
                _pageValid = true;

                String strErr = "";
                if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtContactPerson1")).Text) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtContactNumber1")).Text))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtContactPerson1")).Text))
                        strErr += "<li>" + "Contact Person Name is required." + "</li>";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtContactNumber1")).Text))
                        strErr += "<li>" + "Contact Number is required." + "</li>";
                }

                if (!String.IsNullOrEmpty(txtMeetingNotes.Text) && !String.IsNullOrEmpty(drpFollowupType.SelectedValue) && Convert.ToDateTime(txtNextFollowupDate.Text) < cdt)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Back Date Followup is restricted." + "</li>";
                }


                if (_pageValid)
                {

                    DataTable dtCustomer = new DataTable();
                    dtCustomer = (DataTable)Session["dtCustomer"];

                    DataRow dr = dtCustomer.NewRow();

                    string cdesig = ((DropDownList)e.Item.FindControl("drpContactDesigCode1")).SelectedValue;
                    string cname = ((TextBox)e.Item.FindControl("txtContactPerson1")).Text;
                    string cnumber = ((TextBox)e.Item.FindControl("txtContactNumber1")).Text;
                    string cemail = ((TextBox)e.Item.FindControl("txtContactEmail1")).Text;

                    Int64 cntRow = dtCustomer.Rows.Count + 1;
                    dr["pkID"] = cntRow;
                    dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                    dr["ContactDesigCode1"] = cdesig;
                    dr["ContactPerson1"] = cname;
                    dr["ContactNumber1"] = cnumber;
                    dr["ContactEmail1"] = cemail;

                    dtCustomer.Rows.Add(dr);

                    Session.Add("dtCustomer", dtCustomer);
                    // ---------------------------------------------------------------
                    rptContacts.DataSource = dtCustomer;
                    rptContacts.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Customer Contact Added Successfully  !');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
            if (e.CommandName.ToString() == "Delete")
            {
                string person = ((TextBox)e.Item.FindControl("txtContactPerson0")).Text;
                string contact = ((TextBox)e.Item.FindControl("txtContactNumber0")).Text;
                // ----------------------------------------------------------------------------------------
                DataTable dtCustomer = new DataTable();
                dtCustomer = (DataTable)Session["dtCustomer"];
                foreach (DataRow dr in dtCustomer.Rows)
                {
                    if (dr["ContactPerson1"].ToString() == person && dr["ContactNumber1"].ToString() == contact)
                    {
                        dtCustomer.Rows.Remove(dr);
                        break;
                    }
                }
                dtCustomer.AcceptChanges();
                Session.Add("dtCustomer", dtCustomer);

                rptContacts.DataSource = dtCustomer;
                rptContacts.DataBind();

                //int ReturnCode = 0;
                //string ReturnMsg = "";
                // --------------------------------- Delete Record
                //BAL.CustomerContactsMgmt.DeleteCustomerContacts(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                // -------------------------------------------------
                //if (ReturnCode > 0)
                //{
                //    BindContacts(Convert.ToInt64(hdnCustomerID.Value));
                //}
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + ReturnMsg + "');", true);
            }
            if (e.CommandName.ToString() == "Update")
            {
                int ReturnCode1 = 0;
                string ReturnMsg1 = "";

                DataTable dtCustomer = new DataTable();
                dtCustomer = (DataTable)Session["dtCustomer"];

                string cpkid = ((HiddenField)e.Item.FindControl("hdnpkIDContact")).Value;
                string cdesig = ((DropDownList)e.Item.FindControl("drpContactDesigCode0")).SelectedValue;
                string cname = ((TextBox)e.Item.FindControl("txtContactPerson0")).Text;
                string cnumber = ((TextBox)e.Item.FindControl("txtContactNumber0")).Text;
                string cemail = ((TextBox)e.Item.FindControl("txtContactEmail0")).Text;
                // --------------------------------- Delete Record
                foreach (DataRow dr in dtCustomer.Rows)
                {
                    if (Convert.ToInt64(dr["pkID"]) == Convert.ToInt64(cpkid))
                    {
                        dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                        dr["ContactDesigCode1"] = cdesig;
                        dr["ContactPerson1"] = cname;
                        dr["ContactNumber1"] = cnumber;
                        dr["ContactEmail1"] = cemail;
                    }
                }

                // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                Entity.CustomerContacts objEntity1 = new Entity.CustomerContacts();

                foreach (DataRow dr in dtCustomer.Rows)
                {
                    objEntity1.pkID = Convert.ToInt64(dr["pkID"]);
                    objEntity1.CustomerID = Convert.ToInt64(dr["CustomerID"]);
                    objEntity1.ContactPerson1 = dr["ContactPerson1"].ToString();
                    objEntity1.ContactNumber1 = dr["ContactNumber1"].ToString();
                    objEntity1.ContactEmail1 = dr["ContactEmail1"].ToString();
                    objEntity1.ContactDesigCode1 = dr["ContactDesigCode1"].ToString();
                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.CustomerContactsMgmt.AddUpdateCustomerContacts(objEntity1, out ReturnCode1, out ReturnMsg1);
                }

                Session.Add("dtCustomer", dtCustomer);
                // ---------------------------------------------------------------
                rptContacts.DataSource = dtCustomer;
                rptContacts.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Customer Contact Updated Successfully !</li>');", true);
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                divFollowUp.Visible = false;

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpCustomerType.SelectedValue = lstEntity[0].CustomerType;

                drpPriceList.SelectedValue = lstEntity[0].PriceListID.ToString();

                chkBlock.Checked = lstEntity[0].BlockCustomer;

                txtAddress.Text = lstEntity[0].Address;
                txtArea.Text = lstEntity[0].Area;
                txtPincode.Text = lstEntity[0].Pincode;
                txtAddress1.Text = lstEntity[0].Address1;
                txtArea1.Text = lstEntity[0].Area1;
                txtPincode1.Text = lstEntity[0].Pincode1;
                txtGSTNo.Text = lstEntity[0].GSTNo;
                txtPANNo.Text = lstEntity[0].PANNo;
                txtCINNo.Text = lstEntity[0].CINNo;
                txtContactNo1.Text = lstEntity[0].ContactNo1;
                txtContactNo2.Text = lstEntity[0].ContactNo2;
                txtEmailAddress.Text = lstEntity[0].EmailAddress;
                txtWebsite.Text = lstEntity[0].WebsiteAddress;
                txtShipToCompName.Text = lstEntity[0].ShipToCompanyName;
                txtShipToGSTNo.Text = lstEntity[0].ShipToGSTNo;
                txtOpening.Text = lstEntity[0].OpeningAmount.ToString();
                txtDebit.Text = lstEntity[0].DebitAmount.ToString();
                txtCredit.Text = lstEntity[0].CreditAmount.ToString();
                txtClosing.Text = lstEntity[0].ClosingAmount.ToString();
                txtCrDays.Text = lstEntity[0].CR_Days.ToString();
                txtCrLimit.Text = lstEntity[0].CR_Limit.ToString();

                txtBirthDate.Text = (lstEntity[0].BirthDate != SqlDateTime.MinValue.Value) ? lstEntity[0].BirthDate.ToString("yyyy-MM-dd") : "";
                txtAnniversaryDate.Text = (lstEntity[0].AnniversaryDate != SqlDateTime.MinValue.Value) ? lstEntity[0].AnniversaryDate.ToString("yyyy-MM-dd") : "";

                drpCountry.SelectedValue = lstEntity[0].CountryCode.ToString();
                drpCountry1.SelectedValue = lstEntity[0].CountryCode1.ToString();

                drpCustomerSource.SelectedValue = lstEntity[0].CustomerSourceID.ToString();

                if (!String.IsNullOrEmpty(lstEntity[0].CountryCode))
                {
                    drpState.Enabled = true;
                    drpCountry_SelectedIndexChanged(null, null);
                    drpState.SelectedValue = lstEntity[0].StateCode.ToString();
                }

                if (!String.IsNullOrEmpty(lstEntity[0].StateCode) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                }
                // ------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(lstEntity[0].CountryCode1))
                {
                    drpState.Enabled = true;
                    BindStateList(drpState1, lstEntity[0].CountryCode1);
                    drpState1.SelectedValue = lstEntity[0].StateCode1.ToString();
                }

                if (!String.IsNullOrEmpty(lstEntity[0].StateCode1) && Convert.ToInt64(lstEntity[0].StateCode1) > 0)
                {

                    drpCity1.Enabled = true;
                    BindCityList(drpCity1, lstEntity[0].StateCode1);
                    drpCity1.SelectedValue = lstEntity[0].CityCode1.ToString();
                }
                txtRILPrice.Text = hdnRILPrice.Value;
                txtRemarks.Text = lstEntity[0].Remarks;

                // ----------------------------------------------------------
                BindContacts(Convert.ToInt64(hdnCustomerID.Value));
                BindProducts(Convert.ToInt64(hdnCustomerID.Value));
                // -------------------------------------------------------------------------
                // Product Documents
                // -------------------------------------------------------------------------
                BindCustomerDocuments(Convert.ToInt64(hdnCustomerID.Value));
                BindPro(Convert.ToInt64(hdnCustomerID.Value));
                txtCustomerName.Focus();
            }
            else if (pMode == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------- Delete Record
                BAL.CustomerMgmt.DeleteCustomer(Convert.ToInt64(hdnCustomerID.Value), out ReturnCode, out ReturnMsg);
                if (ReturnCode == 0)
                {
                    string title = "Delete Action Failed";
                    string body = ReturnMsg;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "ErrorPopup", "ShowErrorPopup('" + title + "', '" + body + "');", true);
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
            Session.Remove("dtCustomer");
        }


        public void ClearAllField()
        {
            divFollowUp.Visible = true;
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpCustomerType.SelectedValue = "";
            drpPriceList.SelectedValue = "";
            chkBlock.Checked = true;

            //drpOrgType.SelectedValue = "0";
            //drpReportTo.SelectedValue = "0";
            txtGSTNo.Text = "";
            txtPANNo.Text = "";
            txtCINNo.Text = "";
            txtBirthDate.Text = "";
            txtAnniversaryDate.Text = "";
            txtAddress.Text = "";
            txtArea.Text = "";
            // ------------------------------------------------
            drpCity.Items.Clear();
            drpState.Items.Clear();
            drpCountry.ClearSelection();
            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);
            }

            //if (drpState.Items.FindByText("Gujarat") != null)
            //{
            //    drpState.Items.FindByText("Gujarat").Selected = true;
            //    drpState_SelectedIndexChanged(null, null);
            //}
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            List<Entity.CompanyProfile> lstCSC = new List<Entity.CompanyProfile>();
            lstCSC = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

            drpState.ClearSelection();
            if (drpState.Items.FindByText(lstCSC[0].StateName) != null)
            {
                drpState.Items.FindByText(lstCSC[0].StateName).Selected = true;
                drpState_SelectedIndexChanged(null, null);
            }
            drpCity.ClearSelection();
            if (drpCity.Items.FindByText(lstCSC[0].CityName) != null)
            {
                drpCity.Items.FindByText(lstCSC[0].CityName).Selected = true;
                //drpCity_SelectedIndexChanged(null, null);
            }

            drpCity1.Items.Clear();
            drpState1.Items.Clear();
            drpCountry1.ClearSelection();
            if (drpCountry1.Items.FindByText("India") != null)
            {
                drpCountry1.Items.FindByText("India").Selected = true;
                drpCountry1_SelectedIndexChanged(null, null);
            }
            //if (drpState1.Items.FindByText("Gujarat") != null)
            //{
            //    drpState1.Items.FindByText("Gujarat").Selected = true;
            //    drpState1_SelectedIndexChanged(null, null);
            //}
            Entity.Authenticate objAuth1 = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            List<Entity.CompanyProfile> lstCSC1 = new List<Entity.CompanyProfile>();
            lstCSC1 = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

            drpState1.ClearSelection();
            if (drpState1.Items.FindByText(lstCSC[0].StateName) != null)
            {
                drpState1.Items.FindByText(lstCSC[0].StateName).Selected = true;
                drpState1_SelectedIndexChanged(null, null);
            }
            drpCity1.ClearSelection();
            if (drpCity1.Items.FindByText(lstCSC[0].CityName) != null)
            {
                drpCity1.Items.FindByText(lstCSC[0].CityName).Selected = true;
                //drpCity_SelectedIndexChanged(null, null);
            }
            //if (drpState1.Items.FindByText("Gujarat") != null)
            //{
            //    drpState1.Items.FindByText("Gujarat").Selected = true;
            //    drpState1_SelectedIndexChanged(null, null);
            //}

            txtPincode.Text = "";
            txtAddress1.Text = "";
            txtArea1.Text = "";

            txtPincode1.Text = "";
            txtContactNo1.Text = "";
            txtContactNo2.Text = "";
            txtEmailAddress.Text = "";
            txtWebsite.Text = "";

            txtOpening.Text = "";
            txtDebit.Text = "";
            txtCredit.Text = "";
            txtClosing.Text = "";
            txtCrDays.Text = "";
            txtCrLimit.Text = "";

            txtRemarks.Text = "";
            // ------------------------------------------------------------
            BindContacts(0);
            BindProducts(0);
            BindPro(0);
            BindCustomerDocuments(0);
            // ------------------------------------------------------------
            txtNextFollowupDate.Text = "";
            txtMeetingNotes.Text = "";
            drpFollowupType.SelectedValue = "";

            drpCustomerSource.SelectedValue = "";

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;

            lnkcustdocs.Visible = false;

            txtCustomerName.Focus();
        }

        [WebMethod]
        public static string DeleteCustomer(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CustomerMgmt.DeleteCustomer(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            if (ReturnCode > 0)
            {
                string custFiles = "";
                custFiles = "Cust" + pkID.ToString() + "-*.*";
                string rootFolderPath = HttpContext.Current.Server.MapPath("CustomerDocs");
                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, custFiles);
                foreach (string file in fileList)
                {
                    System.IO.File.Delete(file);
                }
            }
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            String strErr = "";

            int ReturnCode = 0;
            string ReturnMsg = "";

            int ReturnCode1 = 0;
            string ReturnMsg1 = "";

            int ReturnCodeDel = 0;
            string ReturnMsgDel = "";

            Int64 ReturnFollowupPKID = 0;

            _pageValid = true;

            if (String.IsNullOrEmpty(txtCustomerName.Text) || (String.IsNullOrEmpty(txtContactNo1.Text) && drpCustomerType.SelectedValue != "FIXED LEDGER") || ((String.IsNullOrEmpty(drpCountry.SelectedValue) || drpCountry.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V") || ((String.IsNullOrEmpty(drpState.SelectedValue) || drpState.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V") || ((String.IsNullOrEmpty(drpCity.SelectedValue) || drpCity.SelectedValue == "0") && hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V"))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer/Company Name is required." + "</li>";

                if (String.IsNullOrEmpty(drpCustomerType.SelectedValue))
                {
                    strErr += "<li>" + "Customer Type is required." + "</li>";
                }

                if (String.IsNullOrEmpty(txtContactNo1.Text) && drpCustomerType.SelectedValue != "FIXED LEDGER")
                    strErr += "<li>" + "Contact #1 is required." + "</li>";

                if (String.IsNullOrEmpty(drpCountry.SelectedValue))
                    strErr += "<li>" + "Country Selection is required." + "</li>";

                if (hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
                {
                    if (String.IsNullOrEmpty(drpState.SelectedValue) || drpState.SelectedValue == "0")
                        strErr += "<li>" + "State Selection is required." + "</li>";
                }

                if (hdnSerialKey.Value != "PRI9-DG8H-G6GF-TP5V")
                {
                    if (String.IsNullOrEmpty(drpCity.SelectedValue) || drpCity.SelectedValue == "0")
                        strErr += "<li>" + "City Selection is required." + "</li>";
                }

            }

            if (divFollowUp.Visible == true)
            {
                Boolean cc = false;

                String val1 = txtNextFollowupDate.Text.Trim();
                String val2 = txtMeetingNotes.Text.Trim();
                String val3 = drpFollowupType.SelectedValue;
                if (val1 == "" && val2 == "" && val3 == "")
                    cc = true;
                else if (val1 != "" && val2 != "" && val3 != "")
                    cc = true;
                else
                    cc = false;

                //(!String.IsNullOrEmpty(txtNextFollowupDate.Text) || !String.IsNullOrEmpty(txtMeetingNotes.Text) || !String.IsNullOrEmpty(drpFollowupType.SelectedValue))
                if (cc == false)
                {
                    _pageValid = false;
                    strErr += "<li>" + "All Information required to Auto Generate Followup" + "</li>";
                }

            }

            // ----------------------------------------------------------------
            Entity.Customer objEntity = new Entity.Customer();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);

                objEntity.CustomerName = txtCustomerName.Text;
                objEntity.CustomerType = drpCustomerType.SelectedValue.Trim();
                objEntity.BlockCustomer = chkBlock.Checked;
                objEntity.PriceListID = (!String.IsNullOrEmpty(drpPriceList.SelectedValue)) ? Convert.ToInt64(drpPriceList.SelectedValue) : 0;
                objEntity.GSTNo = txtGSTNo.Text;
                objEntity.PANNo = txtPANNo.Text;
                objEntity.CINNo = txtCINNo.Text;

                objEntity.Address = txtAddress.Text;
                objEntity.Area = txtArea.Text;
                objEntity.CountryCode = (!String.IsNullOrEmpty(drpCountry.SelectedValue) ? drpCountry.SelectedValue : "");
                objEntity.StateCode = (!String.IsNullOrEmpty(drpState.SelectedValue) ? drpState.SelectedValue : "0");
                objEntity.CityCode = (!String.IsNullOrEmpty(drpCity.SelectedValue) ? drpCity.SelectedValue : "0");
                objEntity.Pincode = txtPincode.Text;

                objEntity.Address1 = txtAddress1.Text;
                objEntity.Area1 = txtArea1.Text;
                objEntity.CountryCode1 = drpCountry1.SelectedValue;
                objEntity.StateCode1 = drpState1.SelectedValue;
                objEntity.CityCode1 = drpCity1.SelectedValue;
                objEntity.Pincode1 = txtPincode1.Text;
                objEntity.ShipToGSTNo = txtShipToGSTNo.Text;
                objEntity.ShipToCompanyName = txtShipToCompName.Text;
                objEntity.ContactNo1 = txtContactNo1.Text;
                objEntity.ContactNo2 = txtContactNo2.Text;
                objEntity.EmailAddress = txtEmailAddress.Text;
                objEntity.WebsiteAddress = txtWebsite.Text;
                objEntity.Remarks = txtRemarks.Text;

                objEntity.OpeningAmount = (!String.IsNullOrEmpty(txtOpening.Text) ? Convert.ToDecimal(txtOpening.Text) : 0);
                objEntity.DebitAmount = (!String.IsNullOrEmpty(txtDebit.Text) ? Convert.ToDecimal(txtDebit.Text) : 0);
                objEntity.CreditAmount = (!String.IsNullOrEmpty(txtCredit.Text) ? Convert.ToDecimal(txtCredit.Text) : 0);
                objEntity.ClosingAmount = (!String.IsNullOrEmpty(txtClosing.Text) ? Convert.ToDecimal(txtClosing.Text) : 0);

                objEntity.CR_Days = (!String.IsNullOrEmpty(txtCrDays.Text) ? Convert.ToInt64(txtCrDays.Text) : 0);
                objEntity.CR_Limit = (!String.IsNullOrEmpty(txtCrLimit.Text) ? Convert.ToDecimal(txtCrLimit.Text) : 0);

                objEntity.BirthDate = (!String.IsNullOrEmpty(txtBirthDate.Text)) ? Convert.ToDateTime(txtBirthDate.Text) : SqlDateTime.MinValue.Value;
                objEntity.AnniversaryDate = (!String.IsNullOrEmpty(txtAnniversaryDate.Text)) ? Convert.ToDateTime(txtAnniversaryDate.Text) : SqlDateTime.MinValue.Value;
                objEntity.CustomerSourceID = (!String.IsNullOrEmpty(drpCustomerSource.SelectedValue)) ? Convert.ToInt64(drpCustomerSource.SelectedValue) : 0;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CustomerMgmt.AddUpdateCustomer(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    btnSaveEmail.Disabled = true;
                }
                // =========================================================================================
                // >>>>>>>> First Delete all Selectd Customer Contacts entry from table
                // =========================================================================================
                if (ReturnCode > 0)
                {
                    DataTable dtCustomer = new DataTable();
                    dtCustomer = (DataTable)Session["dtCustomer"];
                    // --------------------------------------------------------------
                    BAL.CustomerContactsMgmt.DeleteCustomerContactsByCustomer(ReturnCode, out ReturnCodeDel, out ReturnMsgDel);
                    // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                    Entity.CustomerContacts objEntity1 = new Entity.CustomerContacts();
                    if (dtCustomer != null)
                    {
                        foreach (DataRow dr in dtCustomer.Rows)
                        {
                            objEntity1.pkID = 0;
                            objEntity1.CustomerID = ReturnCode;
                            objEntity1.ContactPerson1 = dr["ContactPerson1"].ToString();
                            objEntity1.ContactNumber1 = dr["ContactNumber1"].ToString();
                            objEntity1.ContactEmail1 = dr["ContactEmail1"].ToString();
                            objEntity1.ContactDesigCode1 = dr["ContactDesigCode1"].ToString();
                            objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.CustomerContactsMgmt.AddUpdateCustomerContacts(objEntity1, out ReturnCode1, out ReturnMsg1);
                        }
                    }
                    if (ReturnCode > 0)
                        Session.Remove("dtCustomer");

                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    // SAVE - Customer Documents
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    BAL.CustomerMgmt.DeleteCustomerDocumentsByCustomerId(ReturnCode, out ReturnCode1, out ReturnMsg1);
                    // ----------------------------------------------
                    string filePath, filename1, ext, type;
                    Byte[] bytes;
                    long CustID;
                    DataTable dtCustDocs = new DataTable();
                    dtCustDocs = (DataTable)Session["dtCustDocs"];
                    // ----------------------------------------------
                    CustID = (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0") ? Convert.ToInt64(hdnCustomerID.Value) : ReturnCode;
                    if (dtCustDocs != null)
                    {
                        foreach (DataRow dr in dtCustDocs.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.CustomerMgmt.AddUpdateCustomerDocuments(CustID, filename1, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                        }
                    }
                    Session.Remove("dtCustDocs");

                    // =========================================================================================
                    // >>>>>>>> Delete all Selectd Customer Price List entry from table
                    // =========================================================================================

                    DataTable dtProducts = new DataTable();
                    dtProducts = (DataTable)Session["dtProducts"];
                    // --------------------------------------------------------------
                    BAL.CustomerProductsMgmt.DeleteCustomerProductsByCustomer(ReturnCode, out ReturnCodeDel, out ReturnMsgDel);
                    // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                    Entity.CustomerProducts objEntity2 = new Entity.CustomerProducts();

                    foreach (DataRow dr in dtProducts.Rows)
                    {
                        objEntity2.pkID = 0;
                        objEntity2.CustomerID = ReturnCode;
                        objEntity2.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                        objEntity2.ConversionRate = Convert.ToDecimal(dr["ConversionRate"].ToString());
                        objEntity2.RatePerBag = Convert.ToDecimal(dr["RatePerBag"].ToString());
                        objEntity2.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.CustomerProductsMgmt.AddUpdateCustomerProducts(objEntity2, out ReturnCode1, out ReturnMsg1);

                    }
                    if (ReturnCode > 0)
                        Session.Remove("dtProducts");

                    DataTable dtP = new DataTable();
                    dtP = (DataTable)Session["dtP"];
                    // --------------------------------------------------------------
                    BAL.CustomerProductsMgmt.DeleteProductsByCustomer(ReturnCode, out ReturnCodeDel, out ReturnMsgDel);
                    // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                    Entity.CustomerProducts objPro = new Entity.CustomerProducts();

                    foreach (DataRow dr in dtP.Rows)
                    {
                        objPro.pkID = 0;
                        objPro.CustomerID = ReturnCode;
                        objPro.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                        objPro.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.CustomerProductsMgmt.AddUpdateCustomerPro(objPro, out ReturnCode1, out ReturnMsg1);

                    }
                    if (ReturnCode > 0)
                        Session.Remove("dtP");
                    // ---------------------------------------------------------------------
                    if (ReturnCode > 0 && objEntity.CustomerID == 0 &&
                        (divFollowUp.Visible == true && (!String.IsNullOrEmpty(txtNextFollowupDate.Text) && !String.IsNullOrEmpty(txtMeetingNotes.Text) && !String.IsNullOrEmpty(drpFollowupType.SelectedValue))))
                    {
                        Entity.Followup objFollow = new Entity.Followup();
                        objFollow.FollowupDate = System.DateTime.Now;
                        objFollow.CustomerID = ReturnCode;
                        objFollow.InquiryNo = "";
                        objFollow.MeetingNotes = txtMeetingNotes.Text;
                        objFollow.NextFollowupDate = Convert.ToDateTime(txtNextFollowupDate.Text);
                        objFollow.InquiryStatusID = (!String.IsNullOrEmpty(drpFollowupType.SelectedValue)) ? Convert.ToInt64(drpFollowupType.SelectedValue) : Convert.ToInt64("0");
                        objFollow.Rating = 1;
                        objFollow.LoginUserID = Session["LoginUserID"].ToString();
                        BAL.FollowupMgmt.AddUpdateFollowup(objFollow, out ReturnCode1, out ReturnMsg1, out ReturnFollowupPKID);
                        strErr += "<li>" + ReturnMsg1 + "</li>";

                        if (ReturnCode1 > 0)
                        {
                            try
                            {
                                string notificationMsg = "";
                                notificationMsg = "FollowUp Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                BAL.CommonMgmt.SendNotification_Firebase("FollowUp", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                BAL.CommonMgmt.SendNotificationToDB("FollowUp", ReturnFollowupPKID, notificationMsg, Session["LoginUserID"].ToString(), 0);
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                }
                // --------------------------------------------------------------
                if (paraSaveAndEmail)
                {
                    Entity.Authenticate objAuth = new Entity.Authenticate();
                    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                    String sendEmailFlag = BAL.CommonMgmt.GetConstant("COMPANYPROFILE", 0, objAuth.CompanyID).ToLower();
                    if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(txtEmailAddress.Text) && txtEmailAddress.Text.ToUpper() != "NULL")
                            {
                                String respVal = "";
                                respVal = BAL.CommonMgmt.SendEmailNotifcation("COMPANYPROFILE", Session["LoginUserID"].ToString(), 0, txtEmailAddress.Text);
                            }
                            strErr += "<li>" + @ReturnMsg + " and Email Sent Successfully !" + "</li>";
                            //divErrorMessage.InnerHtml = @ReturnMsg + " and Email Sent Successfully !";
                        }
                        catch (Exception ex)
                        {
                            strErr += "<li>" + @ReturnMsg + " and Sending Email Failed !" + "</li>";
                            //divErrorMessage.InnerHtml = @ReturnMsg + " and Sending Email Failed !";
                        }

                    }
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
                    drpCity.Items.Insert(0, new ListItem("-- All City --", "0"));
                    //drpCity.Enabled = true;
                    drpCity.Focus();
                }

            }
            if (drpState.SelectedValue == "0" || drpState.SelectedValue == "")
            {
                drpCity.Items.Clear();
            }
        }
        //----------------------------------------------------------------------------------------
        protected void drpCountry1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpCountry1.SelectedValue))
            {
                drpState1.ClearSelection();
                List<Entity.State> lstEvents = new List<Entity.State>();
                lstEvents = BAL.StateMgmt.GetStateList((drpCountry1.SelectedValue).ToString());
                drpState1.DataSource = lstEvents;
                drpState1.DataValueField = "StateCode";
                drpState1.DataTextField = "StateName";
                drpState1.DataBind();
                drpState1.Items.Insert(0, new ListItem("-- All State --", "0"));
                //drpState1.Enabled = true;
                drpState1.Focus();
            }
        }

        protected void drpState1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpState1.SelectedValue))
            {
                if (Convert.ToInt64(drpState1.SelectedValue) > 0)
                {
                    List<Entity.City> lstEvents = new List<Entity.City>();
                    lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpState1.SelectedValue));
                    drpCity1.DataSource = lstEvents;
                    drpCity1.DataValueField = "CityCode";
                    drpCity1.DataTextField = "CityName";
                    drpCity1.DataBind();
                    drpCity1.Items.Insert(0, new ListItem("-- All City --", "0"));
                    //drpCity1.Enabled = true;
                    drpCity1.Focus();
                }

            }
        }
        //----------------------------------------------------------------------------------------------
        protected void imgBtnSave_Click(object sender, ImageClickEventArgs e)
        {
            _pageValid = true;
            String strErr = "";
            if (String.IsNullOrEmpty(txtContactPerson1.Text) || String.IsNullOrEmpty(txtContactNumber1.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtContactPerson1.Text))
                    strErr += "<li>" + "Contact Person Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtContactNumber1.Text))
                    strErr += "<li>" + "Contact Number is required." + "</li>";
            }
            // -------------------------------------------------------------------------
            if (_pageValid)
            {

                DataTable dtCustomer = new DataTable();
                dtCustomer = (DataTable)Session["dtCustomer"];

                DataRow dr = dtCustomer.NewRow();

                string cdesig = drpContactDesigCode1.SelectedValue;
                string cname = txtContactPerson1.Text;
                string cnumber = txtContactNumber1.Text;
                string cemail = txtContactEmail1.Text;

                Int64 cntRow = dtCustomer.Rows.Count + 1;
                dr["pkID"] = cntRow;
                dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                dr["ContactDesigCode1"] = cdesig;
                dr["ContactPerson1"] = cname;
                dr["ContactNumber1"] = cnumber;
                dr["ContactEmail1"] = cemail;

                dtCustomer.Rows.Add(dr);

                Session.Add("dtCustomer", dtCustomer);
                // ---------------------------------------------------------------
                rptContacts.DataSource = dtCustomer;
                rptContacts.DataBind();
                // ---------------------------------------------------------------
                txtContactPerson1.Text = "";
                txtContactNumber1.Text = "";
                txtContactEmail1.Text = "";
                // ---------------------------------------------------------------
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Contact Added .. But Dont Forget To SAVE Entry !');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
            }
        }

        protected void txtOpening_TextChanged(object sender, EventArgs e)
        {
            Decimal op = 0, db = 0, cr = 0, cl = 0;
            op = (!String.IsNullOrEmpty(txtOpening.Text) ? Convert.ToDecimal(txtOpening.Text) : 0);
            db = (!String.IsNullOrEmpty(txtDebit.Text) ? Convert.ToDecimal(txtDebit.Text) : 0);
            cr = (!String.IsNullOrEmpty(txtCredit.Text) ? Convert.ToDecimal(txtCredit.Text) : 0);
            cl = (!String.IsNullOrEmpty(txtClosing.Text) ? Convert.ToDecimal(txtClosing.Text) : 0);
            cl = ((op + db) - cr);
            txtClosing.Text = cl.ToString("0.00");
            txtNextFollowupDate.Focus();
        }

        public void BindStateList(DropDownList drpControl, string ParentKey)
        {
            if (!String.IsNullOrEmpty(ParentKey) && ParentKey != "0")
            {
                drpControl.ClearSelection();
                List<Entity.State> lstEvents = new List<Entity.State>();
                lstEvents = BAL.StateMgmt.GetStateList((drpCountry1.SelectedValue).ToString());
                drpControl.DataSource = lstEvents;
                drpControl.DataValueField = "StateCode";
                drpControl.DataTextField = "StateName";
                drpControl.DataBind();
                drpControl.Items.Insert(0, new ListItem("-- All State --", "0"));
            }
        }

        public void BindCityList(DropDownList drpControl, string ParentKey)
        {
            if (!String.IsNullOrEmpty(ParentKey) && ParentKey != "0")
            {
                drpControl.ClearSelection();
                List<Entity.City> lstEvents = new List<Entity.City>();
                lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpState1.SelectedValue));
                drpControl.DataSource = lstEvents;
                drpControl.DataValueField = "CityCode";
                drpControl.DataTextField = "CityName";
                drpControl.DataBind();
                drpControl.Items.Insert(0, new ListItem("-- All City --", "0"));
            }
        }

        public void BindCustomerDocuments(Int64 pCustomerID)
        {
            DataTable dtCustDocs = new DataTable();
            List<Entity.Documents> lst = BAL.CustomerMgmt.GetCustomerDocumentsList(0, pCustomerID);
            dtCustDocs = PageBase.ConvertListToDataTable(lst);
            rptEmpDocs.DataSource = dtCustDocs;
            rptEmpDocs.DataBind();
            Session.Add("dtCustDocs", dtCustDocs);
        }

        protected void rptEmpDocs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int ReturnCode1;
            string ReturnMsg1;
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtCustDocs = (DataTable)Session["dtCustDocs"];
                for (int i = dtCustDocs.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtCustDocs.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                    {
                        BAL.CustomerMgmt.DeleteCustomerDocuments(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode1, out ReturnMsg1);
                        // --------------------------------------------------------------
                        string filename1 = dr["FileName"].ToString();
                        BAL.CommonMgmt.DeleteFileFromFolder("CustomerDocs", filename1);
                        // --------------------------------------------------------------
                        dr.Delete();
                    }

                }
                dtCustDocs.AcceptChanges();
                Session.Add("dtCustDocs", dtCustDocs);
                rptEmpDocs.DataSource = dtCustDocs;
                rptEmpDocs.DataBind();
                // --------------------------------------------------------------
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {

        }

        public void BindProducts(Int64 pCustomerID)
        {
            DataTable dtProducts1 = new DataTable();
            dtProducts1 = BAL.CustomerProductsMgmt.GetCustomerProductsDetail(pCustomerID);
            rptProducts.DataSource = dtProducts1;
            rptProducts.DataBind();
            Session["dtProducts"] = dtProducts1;
        }

        public void BindPro(Int64 pCustomerID)
        {
            DataTable dtP = new DataTable();
            dtP = BAL.CustomerProductsMgmt.GetProductsDetail(pCustomerID);
            rptPro.DataSource = dtP;
            rptPro.DataBind();
            Session["dtP"] = dtP;
        }

        protected void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            String strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                DateTime cdt = DateTime.Now;
                _pageValid = true;

                //String strErr = "";
                if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductC")).Text) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtConversionC")).Text))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductC")).Text))
                        strErr += "<li>" + "Product Name is required." + "</li>";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtConversionC")).Text))
                        strErr += "<li>" + "Conversion Rate is required." + "</li>";
                }

                if (_pageValid)
                {

                    DataTable dtProducts = new DataTable();
                    dtProducts = (DataTable)Session["dtProducts"];

                    DataRow dr = dtProducts.NewRow();
                    string productid = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                    string productname = ((TextBox)e.Item.FindControl("txtProductC")).Text;
                    string conversion = ((TextBox)e.Item.FindControl("txtConversionC")).Text;
                    string size = ((TextBox)e.Item.FindControl("txtSizeC")).Text;
                    string weight = ((TextBox)e.Item.FindControl("txtWeightC")).Text;
                    string bagrate = ((TextBox)e.Item.FindControl("txtBagRateC")).Text;

                    Int64 cntRow = dtProducts.Rows.Count + 1;
                    dr["pkID"] = cntRow;
                    dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                    dr["ProductID"] = (!String.IsNullOrEmpty(productid)) ? Convert.ToInt64(productid) : 0;
                    dr["ProductName"] = (!String.IsNullOrEmpty(productname)) ? productname : "";
                    dr["ConversionRate"] = conversion;
                    dr["UnitSize"] = size;
                    dr["Box_Weight"] = weight;
                    dr["RatePerBag"] = bagrate;

                    dtProducts.Rows.Add(dr);


                    Session.Add("dtProducts", dtProducts);

                    //DataTable dtProducts = new DataTable();
                    dtProducts = (DataTable)Session["dtProducts"];
                    // --------------------------------------------------------------
                    //BAL.CustomerProductsMgmt.DeleteCustomerProductsByCustomer(ReturnCode, out ReturnCodeDel, out ReturnMsgDel);
                    Entity.CustomerProducts objEntity1 = new Entity.CustomerProducts();
                    foreach (DataRow dr1 in dtProducts.Rows)
                    {
                        objEntity1.pkID = Convert.ToInt64(dr1["pkID"]);
                        objEntity1.CustomerID = Convert.ToInt64(dr1["CustomerID"]);
                        objEntity1.ProductID = Convert.ToInt64(dr1["ProductID"]);
                        objEntity1.ConversionRate = Convert.ToDecimal(dr1["ConversionRate"]);
                        objEntity1.RatePerBag = Convert.ToDecimal(dr1["RatePerBag"]);
                        objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.CustomerProductsMgmt.AddUpdateCustomerProducts(objEntity1, out ReturnCode1, out ReturnMsg1);
                    }
                    // ---------------------------------------------------------------
                    rptProducts.DataSource = dtProducts;
                    rptProducts.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Product Added Successfully  !');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
            if (e.CommandName.ToString() == "Delete")
            {
                //DataTable dtProducts = new DataTable();
                //dtProducts = (DataTable)Session["dtProducts"];
                //// --------------------------------- Delete Record
                //string iname = ((HiddenField)e.Item.FindControl("hdnpkIDProduct")).Value;

                //foreach (DataRow dr in dtProducts.Rows)
                //{
                //    if (dr["pkID"].ToString() == iname)
                //    {
                //        dtProducts.Rows.Remove(dr);
                //        //dr.Delete();
                //        break;
                //    }
                //}
                // --------------------------------- Delete Record
                BAL.CustomerProductsMgmt.DeleteCustomerProducts(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                // -------------------------------------------------
                if (ReturnCode > 0)
                {
                    BindProducts(Convert.ToInt64(hdnCustomerID.Value));
                }
                //rptProducts.DataSource = dtProducts;
                //rptProducts.DataBind();

                //Session.Add("dtProducts", dtProducts);
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
            if (e.CommandName.ToString() == "Update")
            {


                DataTable dtProducts = new DataTable();
                dtProducts = (DataTable)Session["dtProducts"];

                string pkid = ((HiddenField)e.Item.FindControl("hdnpkIDProduct")).Value;
                string conversion = ((TextBox)e.Item.FindControl("txtConversion")).Text;
                string bagrate = ((TextBox)e.Item.FindControl("txtBagRate")).Text;
                // --------------------------------- Delete Record
                foreach (DataRow dr in dtProducts.Rows)
                {
                    if (Convert.ToInt64(dr["pkID"]) == Convert.ToInt64(pkid))
                    {
                        dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                        dr["ConversionRate"] = conversion;
                        dr["RatePerBag"] = bagrate;
                    }
                }

                // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                Entity.CustomerProducts objEntity1 = new Entity.CustomerProducts();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    objEntity1.pkID = Convert.ToInt64(dr["pkID"]);
                    objEntity1.CustomerID = Convert.ToInt64(dr["CustomerID"]);
                    objEntity1.ProductID = Convert.ToInt64(dr["ProductID"]);
                    objEntity1.ConversionRate = Convert.ToDecimal(dr["ConversionRate"]);
                    objEntity1.RatePerBag = Convert.ToDecimal(dr["RatePerBag"]);
                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.CustomerProductsMgmt.AddUpdateCustomerProducts(objEntity1, out ReturnCode1, out ReturnMsg1);
                }

                Session.Add("dtProducts", dtProducts);
                // ---------------------------------------------------------------
                rptProducts.DataSource = dtProducts;
                rptProducts.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Product Updated Successfully !</li>');", true);
            }
        }

        protected void txtConversionC_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptProducts.Controls[rptProducts.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            TextBox txtBagRateC = ((TextBox)rptFootCtrl.FindControl("txtBagRateC"));
            TextBox txtConversionC = ((TextBox)rptFootCtrl.FindControl("txtConversionC"));
            TextBox txtWeightC = ((TextBox)rptFootCtrl.FindControl("txtWeightC"));
            txtBagRateC.Text = (((Convert.ToDecimal(hdnRILPrice.Value) + Convert.ToDecimal(txtConversionC.Text)) * Convert.ToDecimal(txtWeightC.Text)) / 1000).ToString();
        }

        protected void btnUpdateRILPrice_ServerClick(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            Decimal price = Convert.ToDecimal(txtRILPrice.Text);
            BAL.CommonMgmt.UpdateRILPrice(Convert.ToDecimal(price));
            BAL.CommonMgmt.AddRILPrice(Convert.ToDecimal(price));
            BindProducts(Convert.ToInt64(hdnCustomerID.Value));
            //DataTable dtProducts = new DataTable();
            //dtProducts = (DataTable)Session["dtProducts"];
            //rptProducts.DataSource = dtProducts;
            //rptProducts.DataBind();
        }

        protected void btnRilLog_ServerClick(object sender, EventArgs e)
        {

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

        protected void txtConversion_TextChanged(object sender, EventArgs e)
        {
            //Control rptFootCtrl = rptProducts.Controls[rptProducts.Controls.Count - 1].Controls[0];
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField pkID = ((HiddenField)item.FindControl("hdnpkIDProduct"));
            TextBox txtConversion = ((TextBox)item.FindControl("txtConversion"));
            TextBox txtWeight = ((TextBox)item.FindControl("txtWeight"));
            TextBox txtBagRate = ((TextBox)item.FindControl("txtBagRate"));

            Decimal conversion = (!String.IsNullOrEmpty(txtConversion.Text)) ? Convert.ToDecimal(txtConversion.Text) : 0;

            txtBagRate.Text = (((Convert.ToDecimal(hdnRILPrice.Value) + Convert.ToDecimal(txtConversion.Text)) * Convert.ToDecimal(txtWeight.Text)) / 1000).ToString();
            Decimal bagrate = (!String.IsNullOrEmpty(txtBagRate.Text)) ? Convert.ToDecimal(txtBagRate.Text) : 0;

            DataTable dtProducts = new DataTable();
            dtProducts = (DataTable)Session["dtProducts"];

            foreach (System.Data.DataColumn col in dtProducts.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtProducts.Rows)
            {
                if (row["pkID"].ToString() == pkID.Value)
                {
                    row.SetField("ConversionRate", conversion);
                    row.SetField("RatePerBag", bagrate);
                }
            }
            dtProducts.AcceptChanges();
            rptProducts.DataSource = dtProducts;
            rptProducts.DataBind();

            Session.Add("dtProducts", dtProducts);
            txtConversion.Focus();
        }

        protected void txtProductC_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptProducts.Controls[rptProducts.Controls.Count - 1].Controls[0];

            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtSizeC = ((TextBox)rptFootCtrl.FindControl("txtSizeC"));
            TextBox txtWeightC = ((TextBox)rptFootCtrl.FindControl("txtWeightC"));
            int totrec;
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                List<Entity.Products> lstProduct = new List<Entity.Products>();
                lstProduct = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), Session["LoginUserID"].ToString(), 0, 0, out totrec);
                txtSizeC.Text = !String.IsNullOrEmpty(lstProduct[0].UnitSize) ? lstProduct[0].UnitSize : "0";
                txtWeightC.Text = !String.IsNullOrEmpty(lstProduct[0].Box_Weight.ToString()) ? lstProduct[0].Box_Weight.ToString() : "0";
            }
        }

        protected void rptPro_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptPro_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            _pageValid = true;
            String strErr = "";

            DataTable dtP = new DataTable();
            dtP = (DataTable)Session["dtP"];

            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    String pro = ((TextBox)e.Item.FindControl("txtPro")).Text;
                    String proid = ((HiddenField)e.Item.FindControl("hdnProID")).Value;

                    if (String.IsNullOrEmpty(proid))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Please Enter Proper Product" + "</li>";
                    }

                    foreach (System.Data.DataColumn col in dtP.Columns) col.AllowDBNull = true;

                    if (dtP != null)
                    {
                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + proid;
                        DataRow[] foundRows = dtP.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                            return;
                        }

                        if (_pageValid)
                        {
                            DataRow dr = dtP.NewRow();
                            dr["pkID"] = dtP.Rows.Count + 1;
                            dr["ProductID"] = proid;
                            dr["ProductName"] = pro;
                            dtP.Rows.Add(dr);
                            dtP.AcceptChanges();
                        }
                        Session.Add("dtP", dtP);
                        rptPro.DataSource = dtP;
                        rptPro.DataBind();
                    }
                }
            }
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    String proid = ((HiddenField)e.Item.FindControl("edProductID")).Value;
                    foreach(DataRow dr in dtP.Rows)
                    {
                        if (dr["ProductID"].ToString() == proid)
                        {
                            dtP.Rows.Remove(dr);
                            break;
                        }
                       
                    }
                    dtP.AcceptChanges();
                    Session.Add("dtP", dtP);
                    rptPro.DataSource = dtP;
                    rptPro.DataBind();
                }
            }
        }
    }
}