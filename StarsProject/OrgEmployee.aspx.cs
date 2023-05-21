using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace StarsProject
{
    public partial class OrgEmployee : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session.Remove("dtModuleDoc");

                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 10;

                hdnSerialKey.Value = Session["SerialKey"].ToString();
                BindDropDown();
                // --------------------------------------------------------
                myProductDocs.ResetSession("ProductDoc-");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnPkIDEmp.Value = Request.QueryString["id"].ToString();

                    if (hdnPkIDEmp.Value == "0" || hdnPkIDEmp.Value == "")
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
                    // -------------------------------------
                    if (!String.IsNullOrEmpty(Request.QueryString["type"]))
                    {
                        if (Request.QueryString["type"].ToString() == "profile1")
                        {
                            divPersonal.Visible = true;
                            divOther.Visible = false;
                            divPersonal.Attributes["class"] = "col m12 padding-1";
                        }
                        if (Request.QueryString["type"].ToString() == "profile2")
                        {
                            divPersonal.Visible = false;
                            divOther.Visible = true;
                            divOther.Attributes["class"] = "col m12 padding-1";
                        }
                    }
                }
            }
            else
            {
                // ----------------------------------------------------------------------
                // Product Iamge Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (imgEmpFileUpload.PostedFile != null)
                {
                    if (imgEmpFileUpload.HasFile)
                    {
                        string filePath = imgEmpFileUpload.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            string rootFolderPath = Server.MapPath("EmployeeImages");
                            string filesToDelete = @"emp-" + hdnPkIDEmp.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            foreach (string file in fileList)
                            {
                                System.IO.File.Delete(file);
                            }
                            // -----------------------------------------------------
                            String flname = "emp-" + hdnPkIDEmp.Value.Trim() + ext;
                            imgEmpFileUpload.SaveAs(Server.MapPath("EmployeeImages/") + flname);
                            imgEmployee.ImageUrl = "";
                            imgEmployee.ImageUrl = "EmployeeImages/" + flname;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Product Image Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showFileExtError('image');", true);
                    }
                }

                // ----------------------------------------------------------------------
                // eSignature Iamge Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (imgSignatureUpload.PostedFile != null)
                {
                    if (imgSignatureUpload.HasFile)
                    {
                        string filePath = imgSignatureUpload.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            string rootFolderPath = Server.MapPath("EmployeeImages");
                            string filesToDelete = @"esign-" + hdnPkIDEmp.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            foreach (string file in fileList)
                            {
                                System.IO.File.Delete(file);
                            }
                            // -----------------------------------------------------
                            String flname = "esign-" + hdnPkIDEmp.Value.Trim() + ext;
                            imgSignatureUpload.SaveAs(Server.MapPath("EmployeeImages/") + flname);
                            imgSignature.ImageUrl = "";
                            imgSignature.ImageUrl = "EmployeeImages/" + flname;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('eSignature Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showFileExtError('image');", true);
                    }
                }

                // ----------------------------------------------------------------------
                // Product Document Upload On .... Page Postback
                // ----------------------------------------------------------------------
                //if (uploadDocument.PostedFile != null)
                //{
                //    if (uploadDocument.PostedFile.FileName.Length > 0)
                //    {
                //        if (uploadDocument.HasFile)
                //        {
                //            string filePath = uploadDocument.PostedFile.FileName;
                //            string filename1 = Path.GetFileName(filePath);
                //            string ext = Path.GetExtension(filename1);
                //            string type = String.Empty;

                //            if (ext == ".pdf")
                //            {
                //                try
                //                {
                //                    string rootFolderPath = Server.MapPath("EmployeeDocs");
                //                    string filesToDelete = @"emp" + hdnPkIDEmp.Value + "-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                //                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                //                    foreach (string file in fileList)
                //                    {
                //                        System.IO.File.Delete(file);
                //                    }
                //                    // -----------------------------------------------------
                //                    String flname = "emp" + hdnPkIDEmp.Value + "-" + filename1;
                //                    String tmpFile = Server.MapPath("EmployeeDocs/") + flname;
                //                    uploadDocument.PostedFile.SaveAs(tmpFile);
                //                    // ---------------------------------------------------------------
                //                    DataTable dtEmpDocs = new DataTable();
                //                    dtEmpDocs = (DataTable)Session["dtEmpDocs"];
                //                    Int64 cntRow = dtEmpDocs.Rows.Count + 1;
                //                    DataRow dr = dtEmpDocs.NewRow();
                //                    dr["pkID"] = cntRow;
                //                    dr["FileName"] = flname;
                //                    dr["Filetype"] = type;
                //                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                //                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                //                    dtEmpDocs.Rows.Add(dr);
                //                    Session.Add("dtEmpDocs", dtEmpDocs);
                //                    // ---------------------------------------------------------------
                //                    rptEmpDocs.DataSource = dtEmpDocs;
                //                    rptEmpDocs.DataBind();
                //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                //                }
                //                catch (Exception ex) { }
                //            }
                //            else
                //                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                //        }
                //    }
                //}

                myProductDocs.ModuleName = "ProductDoc-";
                myProductDocs.KeyValue = hdnPkIDEmp.Value;
                myProductDocs.ManageLibraryDocs();
            }
            // -------------------------------------------------------------
            txtEmailPassword.Attributes["value"] = txtEmailPassword.Text;
        }

        //public void BindEmployeeDocuments(Int64 pEmployeeID)
        //{
        //    DataTable dtDetail1 = new DataTable();
        //    List<Entity.Documents> lst = BAL.OrganizationEmployeeMgmt.GetEmployeeDocumentsList(0, pEmployeeID);
        //    dtDetail1 = PageBase.ConvertListToDataTable(lst);
        //    rptEmpDocs.DataSource = dtDetail1;
        //    rptEmpDocs.DataBind();
        //    Session.Add("dtEmpDocs", dtDetail1);
        //}

        //protected void rptEmpDocs_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName.ToString() == "Delete")
        //    {
        //        DataTable dtEmpDocs = (DataTable)Session["dtEmpDocs"];
        //        for (int i = dtEmpDocs.Rows.Count - 1; i >= 0; i--)
        //        {
        //            DataRow dr = dtEmpDocs.Rows[i];
        //            if (dr["pkID"].ToString() == e.CommandArgument.ToString())
        //                dr.Delete();
        //        }
        //        dtEmpDocs.AcceptChanges();
        //        Session.Add("dtEmpDocs", dtEmpDocs);
        //        rptEmpDocs.DataSource = dtEmpDocs;
        //        rptEmpDocs.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
        //    }
        //}

        public void OnlyViewControls()
        {
            txtEmployeeName.ReadOnly = true;
            txtCardNo.ReadOnly = true;
            txtEmailAddress.ReadOnly = true;
            txtEmailPassword.ReadOnly = true;
            txtMobileNo.ReadOnly = true;
            txtLandline.ReadOnly = true;
            txtFixedSalary.ReadOnly = true;
            txtBirthDate.ReadOnly = true;
            txtConfirmationDate.ReadOnly = true;
            txtJoiningDate.ReadOnly = true;
            txtReleaseDate.ReadOnly = true;
            txtWorkingHours.ReadOnly = true;
            drpDesignationEmp.Attributes.Add("disabled", "disabled");
            drpOrganizationEmp.Attributes.Add("disabled", "disabled");
            drpBasicPer.Attributes.Add("disabled", "disabled");
            drpGender.Attributes.Add("disabled", "disabled");
            txtEditor.ReadOnly = true;
            txtBankName.ReadOnly = true;
            txtBankBranch.ReadOnly = true;
            txtBankAccountNo.ReadOnly = true;
            txtBankIFSC.ReadOnly = true;
            txtEmpCode.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;

            txtAddress.ReadOnly = true;
            txtArea.ReadOnly = true;
            drpCountry.Attributes.Add("disabled", "disabled");
            drpState.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");
            txtPincode.ReadOnly = true;
            drpMaritalStatus.Attributes.Add("disabled", "disabled");
            txtBloodGroup.ReadOnly = true;
            txtESICNo.ReadOnly = true;
            txtPFAccountNo.ReadOnly = true;

        }

        public void BindDropDown()
        {

            //// ---------------- Report To List -------------------------------------
            List<Entity.OrganizationStructure> lstOrgDept2 = new List<Entity.OrganizationStructure>();
            lstOrgDept2 = BAL.OrganizationStructureMgmt.GetOrganizationStructureDropDownList("S", Session["LoginUserID"].ToString());
            drpOrganizationEmp.DataSource = lstOrgDept2;
            drpOrganizationEmp.DataValueField = "OrgCode";
            drpOrganizationEmp.DataTextField = "OrgName";
            drpOrganizationEmp.DataBind();
            drpOrganizationEmp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Org --", ""));

            //// ---------------- Designation List  -------------------------------------
            List<Entity.Designations> lstDesig = new List<Entity.Designations>();
            lstDesig = BAL.DesignationMgmt.GetDesignationList();
            drpDesignationEmp.DataSource = lstDesig;
            drpDesignationEmp.DataValueField = "DesigCode";
            drpDesignationEmp.DataTextField = "Designation";
            drpDesignationEmp.DataBind();
            drpDesignationEmp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Designation --", ""));

            //// ---------------- ReportTo List  -------------------------------------
            List<Entity.OrganizationEmployee> lstReportTo = new List<Entity.OrganizationEmployee>();
            lstReportTo = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpReportTo.DataSource = lstReportTo;
            drpReportTo.DataValueField = "pkID";
            drpReportTo.DataTextField = "EmployeeName";
            drpReportTo.DataBind();
            drpReportTo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Report To --", ""));
            //// ---------------- Shift Master -------------------------------------
            List<Entity.ShiftMaster> lstShift = new List<Entity.ShiftMaster>();
            lstShift = BAL.ShiftMasterMgmt.GetShiftMaster(Session["LoginUserID"].ToString());
            drpShift.DataSource = lstShift;
            drpShift.DataValueField = "ShiftCode";
            drpShift.DataTextField = "ShiftName";
            drpShift.DataBind();
            drpShift.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Shift --", "0"));

            drpCountry.ClearSelection();
            List<Entity.Country> lstCountry = new List<Entity.Country>();
            lstCountry = BAL.CountryMgmt.GetCountryList();
            drpCountry.DataSource = lstCountry;
            drpCountry.DataValueField = "CountryCode";
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataBind();
            drpCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All Country --", ""));

        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
                // ----------------------------------------------------
                lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(hdnPkIDEmp.Value), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnPkIDEmp.Value = lstEntity[0].pkID.ToString();
                txtEmployeeName.Text = lstEntity[0].EmployeeName;
                txtCardNo.Text = lstEntity[0].CardNo.ToString();
                txtLandline.Text = lstEntity[0].Landline;
                txtMobileNo.Text = lstEntity[0].MobileNo;
                txtEmailAddress.Text = lstEntity[0].EmailAddress;
                txtEmailPassword.Text = lstEntity[0].EmailPassword;
                txtFixedSalary.Text = lstEntity[0].FixedSalary.ToString();
                txtFixedBasic.Text = lstEntity[0].FixedBasic.ToString();
                txtFixedHRA.Text = lstEntity[0].FixedHRA.ToString();
                txtFixedDA.Text = lstEntity[0].FixedDA.ToString();
                txtFixedConv.Text = lstEntity[0].FixedConv.ToString();
                txtFixedSpecial.Text = lstEntity[0].FixedSpecial.ToString();

                txtBirthDate.Text = (lstEntity[0].BirthDate.Year <= 1900) ? "" : lstEntity[0].BirthDate.ToString("yyyy-MM-dd");
                txtConfirmationDate.Text = (lstEntity[0].ConfirmationDate.Year <= 1900) ? "" : lstEntity[0].ConfirmationDate.ToString("yyyy-MM-dd");
                txtJoiningDate.Text = (lstEntity[0].JoiningDate.Year <= 1900) ? "" : lstEntity[0].JoiningDate.ToString("yyyy-MM-dd");
                txtReleaseDate.Text = (lstEntity[0].ReleaseDate.Year <= 1900) ? "" : lstEntity[0].ReleaseDate.ToString("yyyy-MM-dd");
                drpGender.SelectedValue = lstEntity[0].Gender.ToString(); 
                drpBasicPer.SelectedValue = lstEntity[0].BasicPer.ToString(); 
                drpShift.SelectedValue = lstEntity[0].ShiftCode.ToString(); 
                drpOrganizationEmp.SelectedValue = lstEntity[0].OrgCode;
                drpDesignationEmp.SelectedValue = lstEntity[0].DesigCode;
                drpReportTo.SelectedValue = lstEntity[0].ReportTo.ToString();
                txtWorkingHours.Text = lstEntity[0].WorkingHours.ToString();
                txtEditor.Text = lstEntity[0].AuthorizedSign;
                hdnEditor.Value = lstEntity[0].AuthorizedSign;
                imgEmployee.ImageUrl = lstEntity[0].EmployeeImage;
                imgSignature.ImageUrl = lstEntity[0].eSignaturePath;

                txtBankName.Text = lstEntity[0].BankName;
                txtBankBranch.Text = lstEntity[0].BankBranch;
                txtBankAccountNo.Text = lstEntity[0].BankAccountNo;
                txtBankIFSC.Text = lstEntity[0].BankIFSC;

                txtDrivingLicenseNo.Text = lstEntity[0].DrivingLicenseNo;
                txtPassportNo.Text = lstEntity[0].PassportNo;
                txtAadharCardNo.Text = lstEntity[0].AadharCardNo;
                txtPANCardNo.Text = lstEntity[0].PANCardNo;
                txtEmpCode.Text = lstEntity[0].EmpCode;

                drpPFCalc.SelectedValue = (lstEntity[0].PF_Calculation == true) ? "Yes" : "No";
                drpPTCalc.SelectedValue = (lstEntity[0].PT_Calculation == true) ? "Yes" : "No";

                txtAddress.Text = lstEntity[0].Address;
                txtArea.Text = lstEntity[0].Area;
                drpCountry.SelectedValue = lstEntity[0].CountryCode;

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

                txtPincode.Text = lstEntity[0].Pincode;
                drpMaritalStatus.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].MaritalStatus) ? lstEntity[0].MaritalStatus : "");
                txtBloodGroup.Text = lstEntity[0].BloodGroup;
                txtESICNo.Text = lstEntity[0].ESICNo;
                txtPFAccountNo.Text = lstEntity[0].PFAccountNo;
                // -------------------------------------------------------------------------
                // Product Documents
                // -------------------------------------------------------------------------
                //BindEmployeeDocuments(Convert.ToInt64(hdnPkIDEmp.Value));
                // -------------------------------------------------------------------------
                // Employee Credentials
                // -------------------------------------------------------------------------
                BindCredentials(Convert.ToInt64(hdnPkIDEmp.Value));
                txtEmployeeName.Focus();
            }
            //------------------------------------------------------------------------
            myProductDocs.ModuleName = "ProductDoc-";
            myProductDocs.KeyValue = hdnPkIDEmp.Value;
            myProductDocs.BindModuleDocuments();
            //---------------------------------------------------------------------------
        }

        protected void rptOrgEmployee_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0, ReturnpkID = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            _pageValid = true;
            String strErr = "";

            if (String.IsNullOrEmpty(txtEmployeeName.Text) ||
                String.IsNullOrEmpty(drpOrganizationEmp.SelectedValue) || String.IsNullOrEmpty(drpDesignationEmp.SelectedValue) ||
                String.IsNullOrEmpty(txtMobileNo.Text) || String.IsNullOrEmpty(txtBirthDate.Text) ||
                String.IsNullOrEmpty(txtJoiningDate.Text) || String.IsNullOrEmpty(txtFixedSalary.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtEmployeeName.Text))
                    strErr += "<li>" + "Employee Name is required." + "</li>";

                if (String.IsNullOrEmpty(drpOrganizationEmp.SelectedValue))
                    strErr += "<li>" + "Organization is required." + "</li>";

                if (String.IsNullOrEmpty(drpDesignationEmp.SelectedValue))
                    strErr += "<li>" + "Designation is required." + "</li>";

                //if (String.IsNullOrEmpty(txtEmailAddress.Text))
                //    strErr += "<li>" + "Email Address is required." + "</li>";

                //if (String.IsNullOrEmpty(txtEmailPassword.Text))
                //    strErr += "<li>" + "Email Password is required." + "</li>";

                if (String.IsNullOrEmpty(txtMobileNo.Text))
                    strErr += "<li>" + "Mobile is required." + "</li>";

                //if (String.IsNullOrEmpty(drpReportTo.SelectedValue))
                //    strErr += "<li>" + "Report To Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtBirthDate.Text))
                    strErr += "<li>" + "Birth Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtJoiningDate.Text))
                    strErr += "<li>" + "Joining Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtFixedSalary.Text))
                    strErr += "<li>" + "Salary is required." + "</li>";
            }

            if (String.IsNullOrEmpty(txtConfirmationDate.Text) && !String.IsNullOrEmpty(txtReleaseDate.Text))
            {
                _pageValid = false;
                strErr += "<li>" + "Confirmation Date is required." + "</li>";
            }

            if (!String.IsNullOrEmpty(txtJoiningDate.Text) && !String.IsNullOrEmpty(txtConfirmationDate.Text))
            {
                if (Convert.ToDateTime(txtJoiningDate.Text) > Convert.ToDateTime(txtConfirmationDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Joining Date should be less than Confirmation Date." + "</li>";
                }
            }

            if (!String.IsNullOrEmpty(txtJoiningDate.Text) && !String.IsNullOrEmpty(txtConfirmationDate.Text) && !String.IsNullOrEmpty(txtReleaseDate.Text))
            {
                if (Convert.ToDateTime(txtReleaseDate.Text) < Convert.ToDateTime(txtJoiningDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Joining Date should be less than Release Date." + "</li>";
                }

                else if (Convert.ToDateTime(txtReleaseDate.Text) < Convert.ToDateTime(txtConfirmationDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Confirmation Date should be less than Release Date." + "</li>";
                }
            }

            if (String.IsNullOrEmpty(drpCountry.SelectedValue))
            {
                
                strErr += "<li>" + "Country Selection is required." + "</li>";
            }
                

            if (String.IsNullOrEmpty(drpState.SelectedValue) || drpState.SelectedValue == "0")
            {
                _pageValid = false;
                strErr += "<li>" + "State Selection is required." + "</li>";
            }
                    
            
            if (String.IsNullOrEmpty(drpCity.SelectedValue) || drpCity.SelectedValue == "0")
            {
                _pageValid = false;
                strErr += "<li>" + "City Selection is required." + "</li>";
            }
                    
            // -------------------------------------------------------------
            if (_pageValid)
            {
                //string textEmailPassword = EncryptedPassword(txtEmailPassword.Text);
                // --------------------------------------------------------------
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();

                if (!String.IsNullOrEmpty(hdnPkIDEmp.Value))
                    objEntity.pkID = Convert.ToInt64(hdnPkIDEmp.Value);

                objEntity.EmployeeName = txtEmployeeName.Text;
                objEntity.CardNo = !String.IsNullOrEmpty(txtCardNo.Text) ? Convert.ToInt64(txtCardNo.Text) : 0;
                objEntity.Landline = txtLandline.Text;
                objEntity.MobileNo = txtMobileNo.Text;
                objEntity.EmailAddress = txtEmailAddress.Text;
                objEntity.EmailPassword = txtEmailPassword.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                objEntity.Gender = drpGender.SelectedValue;
                objEntity.WorkingHours= txtWorkingHours.Text;

                objEntity.ShiftCode = Convert.ToInt64(drpShift.SelectedValue);
                objEntity.BasicPer = drpBasicPer.SelectedValue;
                objEntity.OrgCode = drpOrganizationEmp.SelectedValue;
                objEntity.DesigCode = drpDesignationEmp.SelectedValue;
                if (!String.IsNullOrEmpty(drpReportTo.SelectedValue))
                    objEntity.ReportTo = Convert.ToInt64(drpReportTo.SelectedValue);

                objEntity.FixedSalary = (!String.IsNullOrEmpty(txtFixedSalary.Text)) ? Convert.ToDecimal(txtFixedSalary.Text) : 0;

                objEntity.FixedBasic = (!String.IsNullOrEmpty(txtFixedBasic.Text)) ? Convert.ToDecimal(txtFixedBasic.Text) : 0;
                objEntity.FixedHRA = (!String.IsNullOrEmpty(txtFixedHRA.Text)) ? Convert.ToDecimal(txtFixedHRA.Text) : 0;
                objEntity.FixedDA = (!String.IsNullOrEmpty(txtFixedDA.Text)) ? Convert.ToDecimal(txtFixedDA.Text) : 0;
                objEntity.FixedConv = (!String.IsNullOrEmpty(txtFixedConv.Text)) ? Convert.ToDecimal(txtFixedConv.Text) : 0;
                objEntity.FixedSpecial = (!String.IsNullOrEmpty(txtFixedSpecial.Text)) ? Convert.ToDecimal(txtFixedSpecial.Text) : 0;

                if (!String.IsNullOrEmpty(txtBirthDate.Text))
                {
                    if (Convert.ToDateTime(txtBirthDate.Text).Year > 1900)
                        objEntity.BirthDate = Convert.ToDateTime(txtBirthDate.Text);
                }

                if (!String.IsNullOrEmpty(txtConfirmationDate.Text))
                {
                    if (Convert.ToDateTime(txtConfirmationDate.Text).Year > 1900)
                        objEntity.ConfirmationDate = Convert.ToDateTime(txtConfirmationDate.Text);
                }



                if (!String.IsNullOrEmpty(txtJoiningDate.Text))
                {
                    if (Convert.ToDateTime(txtJoiningDate.Text).Year > 1900)
                        objEntity.JoiningDate = Convert.ToDateTime(txtJoiningDate.Text);
                }

                if (!String.IsNullOrEmpty(txtReleaseDate.Text))
                {
                    if (Convert.ToDateTime(txtReleaseDate.Text).Year > 1900)
                        objEntity.ReleaseDate = Convert.ToDateTime(txtReleaseDate.Text);
                }

                objEntity.AuthorizedSign = hdnEditor.Value;
                objEntity.AuthorizedSign = txtEditor.Text;
                objEntity.EmployeeImage = imgEmployee.ImageUrl;
                objEntity.eSignaturePath = imgSignature.ImageUrl;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                objEntity.BankName = txtBankName.Text;
                objEntity.BankBranch = txtBankBranch.Text;
                objEntity.BankAccountNo = txtBankAccountNo.Text;
                objEntity.BankIFSC = txtBankIFSC.Text;

                objEntity.DrivingLicenseNo = txtDrivingLicenseNo.Text;
                objEntity.PassportNo = txtPassportNo.Text;
                objEntity.AadharCardNo = txtAadharCardNo.Text;
                objEntity.PANCardNo = txtPANCardNo.Text;
                objEntity.EmpCode = txtEmpCode.Text;
                objEntity.PF_Calculation = (drpPFCalc.SelectedValue == "Yes") ? true : false;
                objEntity.PT_Calculation = (drpPTCalc.SelectedValue == "Yes") ? true : false;

                objEntity.Address = txtAddress.Text;
                objEntity.Area = txtArea.Text;
                objEntity.CountryCode = (!String.IsNullOrEmpty(drpCountry.SelectedValue) ? drpCountry.SelectedValue : "");
                objEntity.StateCode = (!String.IsNullOrEmpty(drpState.SelectedValue) ? drpState.SelectedValue : "0");
                objEntity.CityCode = (!String.IsNullOrEmpty(drpCity.SelectedValue) ? drpCity.SelectedValue : "0");
                objEntity.Pincode = txtPincode.Text;
                objEntity.MaritalStatus = (!String.IsNullOrEmpty(drpMaritalStatus.SelectedValue) ? drpMaritalStatus.SelectedValue : "");
                objEntity.BloodGroup = txtBloodGroup.Text;
                objEntity.ESICNo = txtESICNo.Text;
                objEntity.PFAccountNo = txtPFAccountNo.Text;

                // -------------------------------------------------------------- Insert/Update Record
                BAL.OrganizationEmployeeMgmt.AddUpdateOrganizationEmployee(objEntity, out ReturnCode, out ReturnMsg, out ReturnpkID);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    //string[] RetComplaintDetail = ReturnpkID.Split(',');
                    //ReturnpkID = RetComplaintDetail[0];
                    //Int64 ReturntPKID = Convert.ToInt64(RetComplaintDetail[1]);

                    hdnPkIDEmp.Value = Convert.ToInt64(ReturnpkID).ToString();
                    btnSave.Disabled = true;
                    // ------------------------------------------------------------
                    myProductDocs.KeyValue = hdnPkIDEmp.Value;
                    myProductDocs.SaveModuleDocs();

                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    // SAVE - Product Documents
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    //BAL.OrganizationEmployeeMgmt.DeleteEmployeeDocumentsByEmployeeId(ReturnCode, out ReturnCode1, out ReturnMsg1);
                    //// ----------------------------------------------
                    //string filePath, filename1, ext, type;
                    //Byte[] bytes;
                    //long EmpID;
                    //DataTable dtEmpDocs = new DataTable();
                    //dtEmpDocs = (DataTable)Session["dtEmpDocs"];
                    //// ----------------------------------------------
                    //EmpID = (!String.IsNullOrEmpty(hdnPkIDEmp.Value) && hdnPkIDEmp.Value != "0") ? Convert.ToInt64(hdnPkIDEmp.Value) : ReturnCode;
                    //if (dtEmpDocs != null)
                    //{
                    //    foreach (DataRow dr in dtEmpDocs.Rows)
                    //    {
                    //        if (dr.RowState.ToString() != "Deleted")
                    //        {
                    //            filename1 = dr["FileName"].ToString();
                    //            type = dr["Filetype"].ToString();
                    //            // -------------------------------------------------------------- Insert/Update Record
                    //            BAL.OrganizationEmployeeMgmt.AddUpdateEmployeeDocuments(EmpID, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                    //        }
                    //    }
                    //}
                    //Session.Remove("dtEmpDocs");
                    //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    //// SAVE - Product Documents
                    //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    //Session.Remove("dtCredentials");
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
            myProductDocs.ResetSession("ProductDoc-");

            hdnPkIDEmp.Value = "";
            txtEmployeeName.Text = "";
            txtEmailAddress.Text = "";
            txtEmailPassword.Text = "";
            txtMobileNo.Text = "";
            txtLandline.Text = "";
            txtWorkingHours.Text = "";
            drpShift.SelectedValue = " ";
            drpGender.SelectedValue = " ";
            drpDesignationEmp.SelectedValue = " ";
            drpOrganizationEmp.SelectedValue = "";
            drpBasicPer.SelectedValue = "Monthly";
            drpReportTo.SelectedValue = "";
            txtCardNo.Text = "";
            txtFixedSalary.Text = "";
            txtBirthDate.Text = "";
            txtConfirmationDate.Text = "";
            txtJoiningDate.Text = "";
            txtReleaseDate.Text = "";
            txtEditor.Text = "";
            hdnEditor.Value = "";
            txtEmpCode.Text = "";
            txtEmployeeName.Focus();

            txtAddress.Text = "";
            txtArea.Text = "";
            drpCity.Items.Clear();
            drpState.Items.Clear();
            drpCountry.ClearSelection();
            drpMaritalStatus.ClearSelection();
            txtBloodGroup.Text = "";
            txtESICNo.Text = "";
            txtPFAccountNo.Text = "";
            //BindEmployeeDocuments(0);
            btnSave.Disabled = false;

            // ------------------------------------------------------------
            myProductDocs.ModuleName = "ProductDoc-";
            myProductDocs.KeyValue = hdnPkIDEmp.Value;
            myProductDocs.BindModuleDocuments();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteOrgEmployee(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
            lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            if (lstEntity.Count > 0)
            {
                // ---------------------------------------------------
                myModuleAttachment mya = new myModuleAttachment();
                mya.DeleteModuleEntry("ProductDoc-", pkID.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
            }
            // --------------------------------- Delete Record
            BAL.OrganizationEmployeeMgmt.DeleteOrganizationEmployee(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterEmployee(string pEmpName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // ---------------------------------
            var rows = BAL.OrganizationEmployeeMgmt.GetEmployeeList(pEmpName).Select(sel => new { sel.EmployeeName, sel.pkID });
            return serializer.Serialize(rows);
        }


        //Email Password stored in Encrypted format by Vikram Rajput 21-07-2020 
        public string EncryptedPassword(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }


        //protected void btnUploadDoc_Click(object sender, EventArgs e)
        //{

        //}

        public void BindCredentials(Int64 pEmployeeID)
        {
            DataTable dtCred = new DataTable();
            List<Entity.OrganizationEmployee> lstCred = new List<Entity.OrganizationEmployee>(); 
            lstCred = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeCredentials(pEmployeeID);
            dtCred = PageBase.ConvertListToDataTable(lstCred);
            rptCredentials.DataSource = dtCred;
            rptCredentials.DataBind();
            Session["dtCredentials"] = dtCred;
        }

        protected void rptCredentials_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Save")
            {
                DateTime cdt = DateTime.Now;
                _pageValid = true;
                String strErr = "";
                //if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtDescriptionNew")).Text) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUserIDNew")).Text))
                //{
                //    _pageValid = false;

                //    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtContactPerson1")).Text))
                //        strErr += "<li>" + "Contact Person Name is required." + "</li>";

                //    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtContactNumber1")).Text))
                //        strErr += "<li>" + "Contact Number is required." + "</li>";
                //}

                //if (!String.IsNullOrEmpty(txtMeetingNotes.Text) && !String.IsNullOrEmpty(drpFollowupType.SelectedValue) && Convert.ToDateTime(txtNextFollowupDate.Text) < cdt)
                //{
                //    _pageValid = false;
                //    strErr += "<li>" + "Back Date Followup is restricted." + "</li>";
                //}


                if (_pageValid)
                {

                    DataTable dtCred = new DataTable();
                    dtCred = (DataTable)Session["dtCredentials"];

                    DataRow dr = dtCred.NewRow();

                    string desc = ((TextBox)e.Item.FindControl("txtDescriptionNew")).Text;
                    string uid = ((TextBox)e.Item.FindControl("txtUserIDNew")).Text;
                    string upwd = ((TextBox)e.Item.FindControl("txtUserPasswordNew")).Text;

                    dr["pkID"] = 0;
                    dr["EmployeeID"] = (!String.IsNullOrEmpty(hdnPkIDEmp.Value)) ? Convert.ToInt64(hdnPkIDEmp.Value) : 0;
                    dr["Description"] = desc;
                    dr["UserID"] = uid;
                    dr["UserPassword"] = upwd;

                    dtCred.Rows.Add(dr);

                    Session.Add("dtCredentials", dtCred);
                    // ---------------------------------------------------------------
                    Entity.OrganizationEmployee objEntity11 = new Entity.OrganizationEmployee();
                    objEntity11.pkID = Convert.ToInt64(0);
                    objEntity11.RefEmployeeID = Convert.ToInt64(hdnPkIDEmp.Value);
                    objEntity11.Description = desc;
                    objEntity11.UserID = uid;
                    objEntity11.UserPassword = upwd;
                    
                    // ---------------------------------------------------------------
                    rptCredentials.DataSource = dtCred;
                    rptCredentials.DataBind();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Customer Contact Added Successfully  !');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
            if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // --------------------------------- Delete Record
                BAL.OrganizationEmployeeMgmt.DeleteEmployeeCredentials(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                // -------------------------------------------------
                if (ReturnCode > 0)
                {
                    BindCredentials(Convert.ToInt64(hdnPkIDEmp.Value));
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + ReturnMsg + "');", true);
            }
            if (e.CommandName.ToString() == "Update")
            {
                int ReturnCode1 = 0;
                string ReturnMsg1 = "";

                DataTable dtCustomer = new DataTable();
                dtCustomer = (DataTable)Session["dtCredentials"];
                string cpkid = ((HiddenField)e.Item.FindControl("hdnpkIDPass")).Value;
                string desc = ((TextBox)e.Item.FindControl("txtDescription")).Text;
                string uid = ((TextBox)e.Item.FindControl("txtUserID")).Text;
                string upwd = ((TextBox)e.Item.FindControl("txtUserPassword")).Text;
                //// --------------------------------- Delete Record
                foreach (DataRow dr in dtCustomer.Rows)
                {
                    if (Convert.ToInt64(dr["pkID"]) == Convert.ToInt64(cpkid))
                    {
                        dr["pkID"] = (!String.IsNullOrEmpty(cpkid)) ? Convert.ToInt64(cpkid) : 0;
                        dr["RefEmployeeID"] = (!String.IsNullOrEmpty(hdnPkIDEmp.Value)) ? Convert.ToInt64(hdnPkIDEmp.Value) : 0;
                        dr["Description"] = desc;
                        dr["UserID"] = uid;
                        dr["UserPassword"] = upwd;
                    }
                }

                // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                Entity.OrganizationEmployee objEntity1 = new Entity.OrganizationEmployee();

                foreach (DataRow dr in dtCustomer.Rows)
                {
                    objEntity1.pkID = Convert.ToInt64(dr["pkID"]);
                    objEntity1.RefEmployeeID = Convert.ToInt64(hdnPkIDEmp.Value);
                    objEntity1.Description = dr["Description"].ToString();
                    objEntity1.UserID = dr["UserID"].ToString();
                    objEntity1.UserPassword = dr["UserPassword"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.OrganizationEmployeeMgmt.AddUpdateEmployeeCredentials(objEntity1, out ReturnCode1, out ReturnMsg1);
                }

                Session.Add("dtCredentials", dtCustomer);
                // ---------------------------------------------------------------
                rptCredentials.DataSource = dtCustomer;
                rptCredentials.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Customer Contact Updated Successfully !</li>');", true);
            }
        }

        protected void imgBtnSave_Click(object sender, ImageClickEventArgs e)
        {
            _pageValid = true;
            String strErr = "";
            // -------------------------------------------------------------------------
            if (_pageValid)
            {

                DataTable dtCustomer = new DataTable();
                dtCustomer = (DataTable)Session["dtCredentials"];

                DataRow dr = dtCustomer.NewRow();

                string desc = txtDescriptionNew.Text;
                string uid = txtUserIDNew.Text;
                string upwd = txtUserPasswordNew.Text;

                Int64 cntRow = dtCustomer.Rows.Count + 1;
                dr["pkID"] = cntRow;
                dr["RefEmployeeID"] = (!String.IsNullOrEmpty(hdnPkIDEmp.Value)) ? Convert.ToInt64(hdnPkIDEmp.Value) : 0;
                dr["Description"] = desc;
                dr["UserID"] = uid;
                dr["UserPassword"] = upwd;

                dtCustomer.Rows.Add(dr);

                Session.Add("dtCredentials", dtCustomer);
                rptCredentials.DataSource = dtCustomer;
                rptCredentials.DataBind();
                // -------------------------------------------------------------- Insert/Update Record
                int ReturnCode11 = 0;
                string ReturnMsg11 = "";
                Entity.OrganizationEmployee objEntity11 = new Entity.OrganizationEmployee();
                objEntity11.pkID = Convert.ToInt64(0);
                objEntity11.RefEmployeeID = Convert.ToInt64(hdnPkIDEmp.Value);
                objEntity11.Description = desc;
                objEntity11.UserID = uid;
                objEntity11.UserPassword = upwd;
                BAL.OrganizationEmployeeMgmt.AddUpdateEmployeeCredentials(objEntity11, out ReturnCode11, out ReturnMsg11);
                // ---------------------------------------------------------------
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Contact Added .. But Dont Forget To SAVE Entry !');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
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
                    drpState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All State --", "0"));
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
                    drpCity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All City --", "0"));
                    //drpCity.Enabled = true;
                    drpCity.Focus();
                }

            }
            if (drpState.SelectedValue == "0" || drpState.SelectedValue == "")
            {
                drpCity.Items.Clear();
            }
        }


        public static void RecompressPDF(string largePDF, string smallPDF)
        {
            //Bind a reader to our large PDF
            PdfReader reader = new PdfReader(largePDF);
            //Create our output PDF
            using (FileStream fs = new FileStream(smallPDF, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //Bind a stamper to the file and our reader
                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {
                    //NOTE: This code only deals with page 1, you'd want to loop more for your code
                    //Get page 1
                    PdfDictionary page = reader.GetPageN(1);
                    //Get the xobject structure
                    PdfDictionary resources = (PdfDictionary)PdfReader.GetPdfObject(page.Get(PdfName.RESOURCES));
                    PdfDictionary xobject = (PdfDictionary)PdfReader.GetPdfObject(resources.Get(PdfName.XOBJECT));
                    if (xobject != null)
                    {
                        PdfObject obj;
                        //Loop through each key
                        foreach (PdfName name in xobject.Keys)
                        {
                            obj = xobject.Get(name);
                            if (obj.IsIndirect())
                            {
                                //Get the current key as a PDF object
                                PdfDictionary imgObject = (PdfDictionary)PdfReader.GetPdfObject(obj);
                                //See if its an image
                                if (imgObject.Get(PdfName.SUBTYPE).Equals(PdfName.IMAGE))
                                {
                                    //NOTE: There's a bunch of different types of filters, I'm only handing the simplest one here which is basically raw JPG, you'll have to research others
                                    if (imgObject.Get(PdfName.FILTER).Equals(PdfName.DCTDECODE))
                                    {
                                        //Get the raw bytes of the current image
                                        byte[] oldBytes = PdfReader.GetStreamBytesRaw((PRStream)imgObject);
                                        //Will hold bytes of the compressed image later
                                        byte[] newBytes;
                                        //Wrap a stream around our original image
                                        using (MemoryStream sourceMS = new MemoryStream(oldBytes))
                                        {
                                            //Convert the bytes into a .Net image
                                            using (System.Drawing.Image oldImage = System.Drawing.Bitmap.FromStream(sourceMS))
                                            {
                                                //Shrink the image to 90% of the original
                                                using (System.Drawing.Image newImage = ShrinkImage(oldImage, 0.9f))
                                                {
                                                    //Convert the image to bytes using JPG at 85%
                                                    newBytes = ConvertImageToBytes(newImage, 85);
                                                }
                                            }
                                        }
                                        //Create a new iTextSharp image from our bytes
                                        iTextSharp.text.Image compressedImage = iTextSharp.text.Image.GetInstance(newBytes);
                                        //Kill off the old image
                                        PdfReader.KillIndirect(obj);
                                        //Add our image in its place
                                        stamper.Writer.AddDirectImageSimple(compressedImage, (PRIndirectReference)obj);
                                    }
                                }
                            }
                        }
                    }
                }

                fs.Close();
                fs.Dispose();
            }
            reader.Close();
        }
        //Standard image save code from MSDN, returns a byte array
        private static byte[] ConvertImageToBytes(System.Drawing.Image image, long compressionLevel)
        {
            if (compressionLevel < 0)
            {
                compressionLevel = 0;
            }
            else if (compressionLevel > 100)
            {
                compressionLevel = 100;
            }
            System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, compressionLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jgpEncoder, myEncoderParameters);
                return ms.ToArray();
            }

        }
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private static System.Drawing.Image ShrinkImage(System.Drawing.Image sourceImage, float scaleFactor)
        {
            int newWidth = Convert.ToInt32(sourceImage.Width * scaleFactor);
            int newHeight = Convert.ToInt32(sourceImage.Height * scaleFactor);

            var thumbnailBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnailBitmap))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                g.DrawImage(sourceImage, imageRectangle);
            }
            return thumbnailBitmap;
        }

        [System.Web.Services.WebMethod]
        public static string GetOrgEmpAccuPanelNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetOrgEmpAccuPanelNo(pkID);
            return tempVal;
        }

        [WebMethod(EnableSession = true)]

        public static void GenerateOrgEmpAccuPanel(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // ----------------------------------------------------------------------- 

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            string LoginUserID = HttpContext.Current.Session["LoginUserID"].ToString();
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
            Int32 CompanyId = 0;

            StarsProject.QuotationEagle.serialkey = tmpSerialKey;
            StarsProject.QuotationEagle.LoginUserID = LoginUserID;
            StarsProject.QuotationEagle.printheader = flagPrintHeader;
            StarsProject.QuotationEagle.path = Path;
            StarsProject.QuotationEagle.imagepath = imagepath;
            StarsProject.QuotationEagle.companyid = CompanyId;
            StarsProject.QuotationEagle.printModule = "OrgEmployee";
            // -------------------------------------------------------
            GenerateOrgEmpAccuPanel_AccuPanel(pkID);
        }

        public static void GenerateOrgEmpAccuPanel_AccuPanel(Int64 pkID)
        {
            HttpContext.Current.Session["printModule"] = "OrgEmployee";
            HttpContext.Current.Session["PrintHeader"] = "no";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            //PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblpaddingPDF = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 0, BottomMargin = 0, LeftMargin = 0, RightMargin = 0;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "OrgEmployee");

            ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                    LeftMargin = Convert.ToInt64(tmpary[2].ToString());
                    RightMargin = Convert.ToInt64(tmpary[3].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                    LeftMargin = Convert.ToInt64(tmpary[2].ToString());
                    RightMargin = Convert.ToInt64(tmpary[3].ToString());
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
            // --------------------------------------------------------------------------------------------
           
            List<Entity.OrganizationEmployee> lstOrgEmp = new List<Entity.OrganizationEmployee>();
            lstOrgEmp = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(pkID, 1, 1000, out totrec1);
            // -------------------------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Quotation> lstQuot = new List<Entity.Quotation>();
            lstQuot = BAL.QuotationMgmt.GetQuotationList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //--------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.QuotationDetailMgmt.GetQuotationDetail(lstQuot[0].QuotationNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);

            List<Entity.CustomerContacts> lstCont = new List<Entity.CustomerContacts>();
            if (lstQuot.Count > 0)
                lstCont = BAL.CustomerContactsMgmt.GetCustomerContactsList(lstQuot[0].QuotationKindAttn, lstQuot[0].CustomerID);
            //-------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ------------------------------------------------------------------------------
            List<Entity.EmailTemplate> lstemail = new List<Entity.EmailTemplate>();
            lstemail = BAL.EmailTemplateMgmt.GetEmailTemplateList();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstOrgEmp.Count > 0)
            {
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information


                //int[] column_tableHeader = { 100 };
                //tableHeader.SetWidths(column_tableHeader);
                //tableHeader.SpacingBefore = 8f;
                //tableHeader.LockedWidth = true;
                ////tableHeader.AddCell(pdf.setCell("Original", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                PdfPTable tblMember = new PdfPTable(1);
                int[] column_tblMember = { 100 };
                tblMember.SetWidths(column_tblMember);
                //tblMember.SpacingBefore = 8f;
                //tblMember.LockedWidth = true;

                var empty = new Phrase();
                empty.Add(new Chunk(" ", pdf.fnCalibriBold8));

                //-----------------------Company Logo---------------------------------------
                PdfPTable tblLogo = new PdfPTable(1);
                int[] column_tblLogo = { 100 };
                tblLogo.SetWidths(column_tblLogo);
                int fileCount1 = 0;
                string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";


                if (File.Exists(tmpFile1))
                {
                    if (File.Exists(tmpFile1))   //Signature print
                    {
                        PdfPTable tblSymbol = new PdfPTable(1);
                        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                        eLoc.ScaleAbsolute(180, 86);


                        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                        tblLogo.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount1 = fileCount1 + 1;
                    }
                }


                //-----------------------Employee Photo---------------------------------------
                PdfPTable tblEmPhoto = new PdfPTable(1);
                int[] column_tblEmPhoto = { 100 };
                tblEmPhoto.SetWidths(column_tblEmPhoto);
                int fileCount2 = 0;
                //string EmpImage = lstOrgEmp[0].EmployeeImage;
                //string tmpFile2 = System.Web.Hosting.HostingEnvironment.MapPath("~/EmployeeImages") + "\\emp-" + lstOrgEmp[0].pkID + ".jpg";

                string prodImage = "";
                string tmpFile2 = System.Web.Hosting.HostingEnvironment.MapPath("~/EmployeeImages") + "\\emp-" + lstOrgEmp[0].pkID;
                if (File.Exists(tmpFile2 + ".jpeg"))
                {
                    prodImage = tmpFile2 + ".jpeg";
                }
                else if (File.Exists(tmpFile2 + ".jpg"))
                {
                    prodImage = tmpFile2 + ".jpg";
                }
                else if (File.Exists(tmpFile2 + ".png"))
                {
                    prodImage = tmpFile2 + ".png";
                }

                if (File.Exists(prodImage))
                {
                    
                        PdfPTable tblSymbol1 = new PdfPTable(1);
                        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(prodImage);
                        eLoc.ScaleAbsolute(85, 100);


                        tblSymbol1.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                        tblEmPhoto.AddCell(pdf.setCell(tblSymbol1, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount2 = fileCount2 + 1;
                    }
                    else
                    {
                        tblEmPhoto.AddCell(pdf.setCell("Employee \n Photo ", pdf.WhiteBaseColor, pdf.fnCalibriBold15, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblEmPhoto.AddCell(pdf.setCell("(No Image Found)", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    }
                


                //---------------------------------------------
                //-------------------------------- City & Pincode & State & Country

                PdfPTable tblCPSC = new PdfPTable(4);
                int[] column_tblCPSC = { 12, 38, 15, 35 };
                tblCPSC.SetWidths(column_tblCPSC);

                tblCPSC.AddCell(pdf.setCell("City", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblCPSC.AddCell(pdf.setCell(lstOrgEmp[0].CityName,pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblCPSC.AddCell(pdf.setCell("Pincode", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
                tblCPSC.AddCell(pdf.setCell(lstOrgEmp[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblCPSC.AddCell(pdf.setCell("State", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblCPSC.AddCell(pdf.setCell(lstOrgEmp[0].StateName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblCPSC.AddCell(pdf.setCell("Country", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
                tblCPSC.AddCell(pdf.setCell(lstOrgEmp[0].CountryName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                //---------------------------------------------
                //-------------------------------- Logo With Header

                PdfPTable tblLogoHeader = new PdfPTable(2);
                int[] column_tblLogoHeader = { 28, 72 };
                tblLogoHeader.SetWidths(column_tblLogoHeader);

                tblLogoHeader.AddCell(pdf.setCell(tblLogo, pdf.WhiteBaseColor, pdf.fnCalibriBold12MidnightBlue, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogoHeader.AddCell(pdf.setCell("Employee Personal & Bank Details", pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                //---------------------------------------------
                //-------------------------------- Personal Details

                PdfPTable tblPersonalDT = new PdfPTable(2);
                int[] column_tblPersonalDT = { 37, 63 };
                tblPersonalDT.SetWidths(column_tblPersonalDT);

                tblPersonalDT.AddCell(pdf.setCell("Employee Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblPersonalDT.AddCell(pdf.setCell(lstOrgEmp[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblPersonalDT.AddCell(pdf.setCell("Mobile No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblPersonalDT.AddCell(pdf.setCell(lstOrgEmp[0].MobileNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblPersonalDT.AddCell(pdf.setCell("Emergency Contact No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblPersonalDT.AddCell(pdf.setCell(lstOrgEmp[0].Landline, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblPersonalDT.AddCell(pdf.setCell("Email ID", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblPersonalDT.AddCell(pdf.setCell(lstOrgEmp[0].EmailAddress, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                tblPersonalDT.AddCell(pdf.setCell("Address", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblPersonalDT.AddCell(pdf.setCell((!String.IsNullOrEmpty(lstOrgEmp[0].Address) ? lstOrgEmp[0].Address + " , " : "") + (!String.IsNullOrEmpty(lstOrgEmp[0].Area) ? lstOrgEmp[0].Area : ""), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                PdfPTable tblww = new PdfPTable(2);
                int[] column_tblww = { 81, 19 };
                tblww.SetWidths(column_tblww);
                tblww.AddCell(pdf.setCell(tblPersonalDT, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblww.AddCell(pdf.setCell(tblEmPhoto, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                //---------------------------------------------
                //-------------------------------- Personal with Photo Details

                PdfPTable tblpersonalwithPhoto = new PdfPTable(3);
                int[] column_tblpersonalwithPhoto = { 30, 50, 20};
                tblpersonalwithPhoto.SetWidths(column_tblpersonalwithPhoto);

                tblpersonalwithPhoto.AddCell(pdf.setCell("Personal Details", pdf.Deamgrey, pdf.fnCalibriBold17, pdf.paddingOf6, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                tblpersonalwithPhoto.AddCell(pdf.setCell(tblww, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblpersonalwithPhoto.AddCell(pdf.setCell(tblEmPhoto, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                
                tblpersonalwithPhoto.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                tblpersonalwithPhoto.AddCell(pdf.setCell(tblCPSC, pdf.WhiteBaseColor, pdf.fnCalibri10, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Date of birth", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell((!String.IsNullOrEmpty(lstOrgEmp[0].BirthDate.ToString()) ? Convert.ToDateTime(lstOrgEmp[0].BirthDate).Year > 1900 ? lstOrgEmp[0].BirthDate.ToString("dd-MM-yyyy") : "" : ""), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Marital Status", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].MaritalStatus, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Driving License No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].DrivingLicenseNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Blood Group", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].BloodGroup, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Insurance No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].PassportNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Adhar Card no.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].AadharCardNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Pan Card no.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].PANCardNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("Driving License No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].DrivingLicenseNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
                                
                tblpersonalwithPhoto.AddCell(pdf.setCell("PF Account No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].PFAccountNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblpersonalwithPhoto.AddCell(pdf.setCell("ESIC No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblpersonalwithPhoto.AddCell(pdf.setCell(lstOrgEmp[0].ESICNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                //---------------------------------------------
                //-------------------------------- Position Details

                PdfPTable tblPositiondt = new PdfPTable(4);
                int[] column_tblPositiondt = { 30, 35, 17, 18 };
                tblPositiondt.SetWidths(column_tblPositiondt);

                tblPositiondt.AddCell(pdf.setCell("Position Details", pdf.Deamgrey, pdf.fnCalibriBold17, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                tblPositiondt.AddCell(pdf.setCell("Department/Division", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblPositiondt.AddCell(pdf.setCell(lstOrgEmp[0].OrgName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblPositiondt.AddCell(pdf.setCell("Date of Joining", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
                tblPositiondt.AddCell(pdf.setCell((!String.IsNullOrEmpty(lstOrgEmp[0].JoiningDate.ToString()) ? Convert.ToDateTime(lstOrgEmp[0].JoiningDate).Year > 1900 ? lstOrgEmp[0].JoiningDate.ToString("dd-MM-yyyy") : "" : ""), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
       
                tblPositiondt.AddCell(pdf.setCell("Designation", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblPositiondt.AddCell(pdf.setCell(lstOrgEmp[0].Designation, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
            
                tblPositiondt.AddCell(pdf.setCell("Biometric Code", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));
                tblPositiondt.AddCell(pdf.setCell(lstOrgEmp[0].CardNo.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                //---------------------------------------------
                //-------------------------------- Position Details

                PdfPTable tblBankdt = new PdfPTable(2);
                int[] column_tblBankdt = { 30, 70 };
                tblBankdt.SetWidths(column_tblBankdt);

                tblBankdt.AddCell(pdf.setCell("Bank Account Details", pdf.Deamgrey, pdf.fnCalibriBold17, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                tblBankdt.AddCell(pdf.setCell("Bank Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblBankdt.AddCell(pdf.setCell(lstOrgEmp[0].BankName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblBankdt.AddCell(pdf.setCell("Branch Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblBankdt.AddCell(pdf.setCell(lstOrgEmp[0].BankBranch, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblBankdt.AddCell(pdf.setCell("Account No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblBankdt.AddCell(pdf.setCell(lstOrgEmp[0].BankAccountNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

                tblBankdt.AddCell(pdf.setCell("IFSC Code", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                tblBankdt.AddCell(pdf.setCell(lstOrgEmp[0].BankIFSC, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 10));

               
                //-------------------------------------------------------

                tblMember.AddCell(pdf.setCell(tblLogoHeader, pdf.WhiteBaseColor, pdf.fnCalibriBold24Blue, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblpersonalwithPhoto, pdf.WhiteBaseColor, pdf.fnCalibriBold24Blue, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblPositiondt, pdf.WhiteBaseColor, pdf.fnCalibriBold24Blue, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblBankdt, pdf.WhiteBaseColor, pdf.fnCalibriBold24Blue, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                int[] column_tblpaddingPDF = { 100 };
                tblpaddingPDF.SetWidths(column_tblpaddingPDF);
                tblpaddingPDF.SpacingBefore = 8f;
                tblpaddingPDF.LockedWidth = true;
                tblpaddingPDF.AddCell(pdf.setCell(tblMember, pdf.WhiteBaseColor, pdf.fnCalibriBold24Blue, pdf.paddingOf15, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));

                #endregion



            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstOrgEmp[0].EmployeeName.Replace("/", "-").ToString() + ".pdf";
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

            // >>>>>> Adding Quotation Master Information Table
            tblpaddingPDF.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblpaddingPDF.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblpaddingPDF);            

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



        private static PdfPCell setCellFixImage(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(pImage, false);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }

    }
}