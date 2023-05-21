using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class CompanyProfile : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 10;
                //drpCity.CssClass = "form-control";
                BindDropDown();
                // --------------------------------------------------------
                if (drpCompany.Items.Count > 0)
                {
                    hdnCompanyID.Value = drpCompany.SelectedValue;
                    setLayout("Edit");
                }
            }
            else
            {
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
                            string rootFolderPath = Server.MapPath("images");
                            string filesToDelete = @"eSignature.*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            foreach (string file in fileList)
                            {
                                System.IO.File.Delete(file);
                            }
                            // -----------------------------------------------------
                            String flname = "eSignature" + ext;
                            imgSignatureUpload.SaveAs(Server.MapPath("images/") + flname);
                            imgSignature.ImageUrl = "";
                            imgSignature.ImageUrl = "Images/" + flname;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('eSignature Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showFileExtError('image');", true);
                    }
                }
            }
            txtPassword.Attributes["value"] = txtPassword.Text;
        }

        public void OnlyViewControls()
        {
            txtCompanyName.ReadOnly = true;
            txtGSTNo.ReadOnly = true;
            txtPANNo.ReadOnly = true;
            txtCINo.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtArea.ReadOnly = true;
            txtPincode.ReadOnly = true;
            chkCustomer.Enabled = false;
            chkInquiry.Enabled = false;
            chkQuotation.Enabled = false;
            drpCity.Attributes.Add("disabled", "disabled");
            drpState.Attributes.Add("disabled", "disabled"); 
            drpParentCompany.Attributes.Add("disabled", "disabled");

            txtHost.ReadOnly = true;
            txtUserName.ReadOnly = true;
            txtPassword.ReadOnly = true;
            txtPortNumber.ReadOnly = true;

            txtBankName.ReadOnly = true;
            txtBranchName.ReadOnly = true;
            txtBankAccountNo.ReadOnly = true;
            txtBankIFSC.ReadOnly = true;
            txtBankSWIFT.ReadOnly = true;
            chkEnableSSL.Enabled = false;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            //// ---------------- State List -------------------------------------
            List<Entity.State> lstState = new List<Entity.State>();
            lstState = BAL.StateMgmt.GetStateList();
            drpState.DataSource = lstState;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new ListItem("-- All State --", ""));

            //// ---------------- City List -------------------------------------
            //List<Entity.City> lstEvents = new List<Entity.City>();
            //lstEvents = BAL.CityMgmt.GetCityList();
            //drpCity.DataSource = lstEvents;
            //drpCity.DataValueField = "CityCode";
            //drpCity.DataTextField = "CityName";
            //drpCity.DataBind();
            //drpCity.Items.Insert(0, new ListItem("-- All City --", ""));

            // ---------------- Designation List  -------------------------------------
            List<Entity.CompanyProfile> lstDesig = new List<Entity.CompanyProfile>();
            lstDesig = BAL.CommonMgmt.GetCompanyProfileList(0, Session["LoginUserID"].ToString());
            drpParentCompany.DataSource = lstDesig;
            drpParentCompany.DataValueField = "CompanyID";
            drpParentCompany.DataTextField = "CompanyName";
            drpParentCompany.DataBind();
            drpParentCompany.Items.Insert(0, new ListItem("-- Select Company --", "0"));

            // ---------------- Designation List  -------------------------------------
            List<Entity.CompanyProfile> lstComp = new List<Entity.CompanyProfile>();

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            if (objAuth.CompanyType.ToUpper() == "HO")
                lstComp = BAL.CommonMgmt.GetCompanyProfileList(0, Session["LoginUserID"].ToString());
            else
                lstComp = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, Session["LoginUserID"].ToString());
            drpCompany.DataSource = lstComp;
            drpCompany.DataValueField = "CompanyID";
            drpCompany.DataTextField = "CompanyName";
            drpCompany.DataBind();
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.CompanyProfile> lstEntity = new List<Entity.CompanyProfile>();
                // ----------------------------------------------------
                lstEntity = BAL.CommonMgmt.GetCompanyProfileList(Convert.ToInt64(hdnCompanyID.Value), Session["LoginUserID"].ToString());
                hdnBankID.Value = lstEntity[0].BankID.ToString();
                hdnCompanyID.Value = lstEntity[0].CompanyID.ToString();
                txtCompanyName.Text = lstEntity[0].CompanyName;
                txtGSTNo.Text = (lstEntity.Count > 0) ? lstEntity[0].GSTNo : "";
                txtPANNo.Text = (lstEntity.Count > 0) ? lstEntity[0].PANNo : "";
                txtCINo.Text = (lstEntity.Count > 0) ? lstEntity[0].CINNo : "";
                txtAddress.Text = lstEntity[0].Address;
                txtArea.Text = lstEntity[0].Area;
                txtPincode.Text = lstEntity[0].Pincode;
                chkCustomer.Checked = (String.IsNullOrEmpty(lstEntity[0].chkCustomer) || lstEntity[0].chkCustomer.ToLower() == "no") ? false : true;
                chkInquiry.Checked = (String.IsNullOrEmpty(lstEntity[0].chkInquiry) || lstEntity[0].chkInquiry.ToLower() == "no") ? false : true;
                chkQuotation.Checked = (String.IsNullOrEmpty(lstEntity[0].chkQuotation) || lstEntity[0].chkQuotation.ToLower() == "no") ? false : true;
                chkLeaveRequest.Checked = (String.IsNullOrEmpty(lstEntity[0].chkLeaveRequest) || lstEntity[0].chkLeaveRequest.ToLower() == "no") ? false : true;
                chkFeedback.Checked = (String.IsNullOrEmpty(lstEntity[0].chkFeedback) || lstEntity[0].chkFeedback.ToLower() == "no") ? false : true;
                drpState.SelectedValue = (String.IsNullOrEmpty(lstEntity[0].StateCode.ToString()) || lstEntity[0].StateCode.ToString() == "0") ? "0" : lstEntity[0].StateCode.ToString();
                if (!String.IsNullOrEmpty(lstEntity[0].StateCode.ToString()) && Convert.ToInt64(lstEntity[0].StateCode) > 0)
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].CityCode.ToString();
                }
                imgSignature.ImageUrl = lstEntity[0].eSignaturePath;
                //drpCity.SelectedValue = (String.IsNullOrEmpty(lstEntity[0].CityCode.ToString()) || lstEntity[0].CityCode.ToString() == "0") ? "0" : lstEntity[0].CityCode.ToString();
                drpParentCompany.SelectedValue = (String.IsNullOrEmpty(lstEntity[0].ParentCompanyID.ToString()) || lstEntity[0].ParentCompanyID.ToString() == "0") ? "0" : lstEntity[0].ParentCompanyID.ToString();

                txtHost.Text = lstEntity[0].Host; 
                txtUserName.Text = lstEntity[0].UserName;
                txtPassword.Text = lstEntity[0].Password;
                txtPortNumber.Text = lstEntity[0].PortNumber.ToString();
                chkEnableSSL.Checked = lstEntity[0].EnableSSL;
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                // Bank Info
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //List<Entity.OrganizationBankInfo> lstEntity1 = new List<Entity.OrganizationBankInfo>();
                //if (hdnBankID.Value == "0")
                //    lstEntity1 = BAL.CommonMgmt.GetBankInfoList(Convert.ToInt64(hdnCompanyID.Value));
                //else
                //    lstEntity1 = BAL.CommonMgmt.GetBankInfoListBypkID(Convert.ToInt64(hdnBankID.Value));

                //hdnBankID.Value = (lstEntity1.Count > 0) ? lstEntity1[0].pkID.ToString() : "0";
                //txtBankName.Text = (lstEntity1.Count>0) ? lstEntity1[0].BankName : "";
                //txtBranchName.Text = (lstEntity1.Count>0) ? lstEntity1[0].BranchName : "";
                //txtBankAccountNo.Text = (lstEntity1.Count>0) ? lstEntity1[0].BankAccountNo : "";
                //txtBankIFSC.Text = (lstEntity1.Count>0) ? lstEntity1[0].BankIFSC : "";
                //txtBankSWIFT.Text = (lstEntity1.Count > 0) ? lstEntity1[0].BankSWIFT : ""; 
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            _pageValid = true;
            string strErr = "";

            if (String.IsNullOrEmpty(txtCompanyName.Text))
            {
                _pageValid = false;

                

                if (String.IsNullOrEmpty(txtCompanyName.Text))
                                strErr += "<li>" + "Company Name is required." + "</li>";
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {

                // --------------------------------------------------------------
                Entity.CompanyProfile objEntity = new Entity.CompanyProfile();

                if (!String.IsNullOrEmpty(hdnCompanyID.Value))
                    objEntity.CompanyID = Convert.ToInt64(hdnCompanyID.Value);
                objEntity.BankID = Convert.ToInt64(hdnBankID.Value);
                objEntity.CompanyName = txtCompanyName.Text;
                objEntity.GSTNo = txtGSTNo.Text;
                objEntity.PANNo = txtPANNo.Text;
                objEntity.CINNo = txtCINo.Text;
                objEntity.Address = txtAddress.Text;
                objEntity.Area = txtArea.Text;
                objEntity.Pincode = txtPincode.Text;
                objEntity.eSignaturePath = imgSignature.ImageUrl;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                objEntity.CityCode = (!String.IsNullOrEmpty(drpCity.SelectedValue)) ? Convert.ToInt64(drpCity.SelectedValue) : 0;
                objEntity.StateCode = (!String.IsNullOrEmpty(drpState.SelectedValue)) ? Convert.ToInt64(drpState.SelectedValue) : 0;
                objEntity.ParentCompanyID = (!String.IsNullOrEmpty(drpParentCompany.SelectedValue)) ? Convert.ToInt64(drpParentCompany.SelectedValue) : 0;  
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                objEntity.chkCustomer = chkCustomer.Checked ? "Yes" : "No";
                objEntity.chkInquiry = chkInquiry.Checked ? "Yes" : "No";
                objEntity.chkQuotation = chkQuotation.Checked ? "Yes" : "No";
                objEntity.chkLeaveRequest = chkLeaveRequest.Checked ? "Yes" : "No";
                objEntity.chkFeedback = chkFeedback.Checked ? "Yes" : "No";

                objEntity.Host = txtHost.Text;
                objEntity.UserName = txtUserName.Text;
                objEntity.Password = txtPassword.Text;
                objEntity.PortNumber = Convert.ToInt64(txtPortNumber.Text);
                objEntity.EnableSSL = chkEnableSSL.Checked;
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CommonMgmt.AddUpdateCompanyProfile(objEntity, out ReturnCode, out ReturnMsg);
                
                if (ReturnCode > 0)
                {
                    Entity.OrganizationBankInfo objBank = new Entity.OrganizationBankInfo();
                    objBank.pkID = Convert.ToInt64(hdnBankID.Value);
                    objBank.CompanyID = Convert.ToInt64(hdnCompanyID.Value);
                    objBank.BankName = txtBankName.Text;
                    objBank.BranchName = txtBranchName.Text;
                    objBank.BankAccountNo = txtBankAccountNo.Text;
                    objBank.BankIFSC = txtBankIFSC.Text;
                    objBank.BankSWIFT = txtBankSWIFT.Text;
                    objBank.BankAccountName = "";
                    //objBank.GSTNo = txtGSTNo.Text;
                    //objBank.PANNo = txtPANNo.Text;
                    objBank.LoginUserID = Session["LoginUserID"].ToString();

                    BAL.CommonMgmt.AddUpdateBankInfo(objBank, out ReturnCode1, out ReturnMsg1);
                }
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";
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
            hdnBankID.Value = "";
            hdnCompanyID.Value = "";
            txtCompanyName.Text = "";
            txtGSTNo.Text = "";
            txtPANNo.Text = "";
            txtCINo.Text = "";
            txtAddress.Text = "";
            txtArea.Text = "";
            txtPincode.Text = "";
            drpCity.SelectedValue = "";
            drpState.SelectedValue = "";
            drpParentCompany.SelectedValue = "";

            chkCustomer.Checked = false;
            chkInquiry.Checked = false;
            chkQuotation.Checked = false;
            chkLeaveRequest.Checked = false;
            chkFeedback.Checked = false;

            txtHost.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtPortNumber.Text = "";

            txtBankName.Text = "";
            txtBranchName.Text = "";
            txtBankAccountNo.Text = "";
            txtBankIFSC.Text = "";
            txtBankSWIFT.Text = "";
            chkEnableSSL.Checked = false;

            txtCompanyName.Focus();
        }

        protected void drpCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnCompanyID.Value = drpCompany.SelectedValue;
            BindDropDown();
            setLayout("Edit");
            
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
                    drpCity.Items.Insert(0, new ListItem("-- All City --", ""));
                    drpCity.Enabled = true;
                }

            }
        }


        protected void imgBtnSave_Click(object sender, ImageClickEventArgs e)
        { }
    }
}