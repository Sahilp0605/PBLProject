using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace StarsProject
{
    public partial class VisitorInfo : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetailInq", dtDetail);
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;

                BindDropDown();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        List<Entity.Country> lstEvents = new List<Entity.Country>();
                        lstEvents = BAL.CountryMgmt.GetCountryList();
                        drpCountry.DataSource = lstEvents;
                        drpCountry.DataValueField = "CountryCode";
                        drpCountry.DataTextField = "CountryName";
                        drpCountry.DataBind();
                        drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

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

            else
            {
                // ----------------------------------------------------------------------
                // Visitor Image Upload On .... Page Postback
                // ----------------------------------------------------------------------

                if (uploadImage.PostedFile != null)
                {
                    if (uploadImage.HasFile)
                    {
                        string filePath = uploadImage.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            string rootFolderPathImage = Server.MapPath("visitorimages");
                            string filesToDeleteImage = @"visitor-image-" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileListImage = System.IO.Directory.GetFiles(rootFolderPathImage, filesToDeleteImage);
                            foreach (string fileimage in fileListImage)
                            {
                                System.IO.File.Delete(fileimage);
                            }

                            String flnameimage = "visitor-image-" + hdnpkID.Value.Trim() + ext;
                            uploadImage.SaveAs(Server.MapPath("visitorimages/") + flnameimage);
                            imgVisitor.ImageUrl = "";
                            imgVisitor.ImageUrl = "visitorimages/" + flnameimage;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Visitor Image Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                    }
                }

                // ----------------------------------------------------------------------
                // Visitor Document Upload On .... Page Postback
                // ----------------------------------------------------------------------

                if (uploadDocument.PostedFile != null)
                {
                    if (uploadDocument.HasFile)
                    {
                        string filePath = uploadDocument.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            string rootFolderPathDocument = Server.MapPath("visitordocuments");
                            string filesToDeleteDocument = @"visitor-document-" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileListDocument = System.IO.Directory.GetFiles(rootFolderPathDocument, filesToDeleteDocument);
                            foreach (string filedocument in fileListDocument)
                            {
                                System.IO.File.Delete(filedocument);
                            }
                            // -----------------------------------------------------
                            String flnamedocument = "visitor-document-" + hdnpkID.Value.Trim() + ext;
                            uploadDocument.SaveAs(Server.MapPath("visitordocuments/") + flnamedocument);
                            imgDocument.ImageUrl = "";
                            imgDocument.ImageUrl = "visitordocuments/" + flnamedocument;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Visitor Document Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                    }
                }

                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }

        public void OnlyViewControls()
        {
            txtInquiryNo.ReadOnly = true;
            txtVisitDate.ReadOnly = true;
            txtVisitTime.ReadOnly = true;
            txtVisitorName.ReadOnly = true;
            txtVisitorContact.ReadOnly = true;
            txtVisitorEmail.ReadOnly = true;
            txtPurposeOfVisit.ReadOnly = true;

            txtCompanyName.ReadOnly = true;
            txtCompanyContact.ReadOnly = true;
            txtCompanyAddress.ReadOnly = true;
            txtPinCode.ReadOnly = true;

            drpDepartment.Attributes.Add("disabled", "disabled");
            drpMeetingTo.Attributes.Add("disabled", "disabled");
            drpCountry.Attributes.Add("disabled", "disabled");
            drpState.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");

            uploadImage.Enabled = false;
            uploadDocument.Enabled = false;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            //// ---------------- Department To List -------------------------------------
            List<Entity.OrganizationStructure> lstOrgDept2 = new List<Entity.OrganizationStructure>();
            lstOrgDept2 = BAL.OrganizationStructureMgmt.GetOrganizationStructureDropDownList("S", Session["LoginUserID"].ToString());
            drpDepartment.DataSource = lstOrgDept2;
            drpDepartment.DataValueField = "OrgName";
            drpDepartment.DataTextField = "OrgName";
            drpDepartment.DataBind();
            drpDepartment.Items.Insert(0, new ListItem("-- Select Department --", ""));
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                List<Entity.Country> lstEvents = new List<Entity.Country>();
                lstEvents = BAL.CountryMgmt.GetCountryList();
                drpCountry.DataSource = lstEvents;
                drpCountry.DataValueField = "CountryCode";
                drpCountry.DataTextField = "CountryName";
                drpCountry.DataBind();
                drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.VisitorInfo> lstEntity = new List<Entity.VisitorInfo>();
                // ----------------------------------------------------
                lstEntity = BAL.VisitorInfoMgmt.GetVisitorInfoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtInquiryNo.Text = lstEntity[0].InquiryNo;
                txtVisitDate.Text = lstEntity[0].VisitDate.ToString("yyyy-MM-dd");
                txtVisitTime.Text = lstEntity[0].VisitTime.ToString();
                txtVisitorName.Text = lstEntity[0].VisitorName;
                txtVisitorContact.Text = lstEntity[0].VisitorContact;
                txtVisitorEmail.Text = lstEntity[0].VisitorEmail;
                txtPurposeOfVisit.Text = lstEntity[0].PurposeOfVisit;
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();

                drpDepartment.SelectedValue = lstEntity[0].Department;

                if (!String.IsNullOrEmpty(lstEntity[0].Department.ToString()))
                {
                    drpMeetingTo.Enabled = true;
                    drpDepartment_SelectedIndexChanged(null, null);
                    drpMeetingTo.SelectedValue = lstEntity[0].MeetingTo.ToString();
                }

                txtCompanyName.Text = lstEntity[0].CompanyName.ToString();
                txtCompanyContact.Text = lstEntity[0].CompanyContact.ToString();
                txtCompanyAddress.Text = lstEntity[0].Address.ToString();
                txtPinCode.Text = lstEntity[0].Pincode.ToString();

                drpCountry.SelectedValue = lstEntity[0].Country;

                if (!String.IsNullOrEmpty(lstEntity[0].Country))
                {
                    drpState.Enabled = true;
                    drpCountry_SelectedIndexChanged(null, null);
                    drpState.SelectedValue = lstEntity[0].State;
                }

                if (!String.IsNullOrEmpty(lstEntity[0].State))
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].City;
                }

                imgVisitor.ImageUrl = lstEntity[0].VisitorImage;
                imgDocument.ImageUrl = lstEntity[0].VisitorDocument;

                txtVisitDate.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int ReturnCode = 0;
            string ReturnMsg = "";
            string ReturnInquiryNo = "";
            long @ReturnVisitorId = 0;
            string strErr = "";

            if (String.IsNullOrEmpty(txtVisitDate.Text) || String.IsNullOrEmpty(txtVisitTime.Text) || String.IsNullOrEmpty(txtVisitorName.Text) || String.IsNullOrEmpty(txtVisitorContact.Text) ||
                String.IsNullOrEmpty(txtPurposeOfVisit.Text) || String.IsNullOrEmpty(drpDepartment.SelectedValue) || String.IsNullOrEmpty(drpMeetingTo.SelectedValue) ||
                String.IsNullOrEmpty(txtCompanyAddress.Text) || String.IsNullOrEmpty(drpCountry.SelectedValue) || String.IsNullOrEmpty(drpState.SelectedValue) || String.IsNullOrEmpty(drpCity.SelectedValue))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtVisitDate.Text))
                    strErr += "<li>" + "Visit Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitTime.Text))
                    strErr += "<li>" + "Visit Time is required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitorName.Text))
                    strErr += "<li>" + "Visitor Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitorContact.Text))
                    strErr += "<li>" + "Contact is required." + "</li>";

                if (String.IsNullOrEmpty(txtPurposeOfVisit.Text))
                    strErr += "<li>" + "Purpose is required." + "</li>";

                if (String.IsNullOrEmpty(drpDepartment.SelectedValue))
                    strErr += "<li>" + "DepartName selection is required." + "</li>";

                if (!String.IsNullOrEmpty(drpDepartment.SelectedValue) && String.IsNullOrEmpty(drpMeetingTo.SelectedValue))
                    strErr += "<li>" + "Meeting To selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtCompanyAddress.Text))
                    strErr += "<li>" + "Address selection is required." + "</li>";

                if (String.IsNullOrEmpty(drpCountry.SelectedValue))
                    strErr += "<li>" + "Country selection is required." + "</li>";

                if (!String.IsNullOrEmpty(drpCountry.SelectedValue) && String.IsNullOrEmpty(drpState.SelectedValue))
                    strErr += "<li>" + "State selection is required." + "</li>";

                if (!String.IsNullOrEmpty(drpState.SelectedValue) && String.IsNullOrEmpty(drpCity.SelectedValue))
                    strErr += "<li>" + "City selection is required." + "</li>";
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.VisitorInfo objEntity = new Entity.VisitorInfo();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.InquiryNo = txtInquiryNo.Text;
                objEntity.VisitDate = Convert.ToDateTime(txtVisitDate.Text);
                objEntity.VisitTime = txtVisitTime.Text;
                objEntity.VisitorName = txtVisitorName.Text;
                objEntity.VisitorContact = txtVisitorContact.Text;
                objEntity.VisitorEmail = txtVisitorEmail.Text;
                objEntity.PurposeOfVisit = txtPurposeOfVisit.Text;

                if (!String.IsNullOrEmpty(drpDepartment.SelectedValue))
                    objEntity.Department = drpDepartment.SelectedValue;

                if (!String.IsNullOrEmpty(drpMeetingTo.SelectedValue))
                    objEntity.MeetingTo = drpMeetingTo.SelectedValue;

                objEntity.CompanyName = txtCompanyName.Text;
                objEntity.CompanyContact = txtCompanyContact.Text;
                objEntity.Address = txtCompanyAddress.Text;
                objEntity.Pincode = txtPinCode.Text;

                if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
                    objEntity.Country = drpCountry.SelectedValue;

                if (!String.IsNullOrEmpty(drpState.SelectedValue))
                    objEntity.State = drpState.SelectedValue;

                if (!String.IsNullOrEmpty(drpCity.SelectedValue))
                    objEntity.City = drpCity.SelectedValue;

                objEntity.VisitorImage = imgVisitor.ImageUrl;
                objEntity.VisitorDocument = imgDocument.ImageUrl;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.VisitorInfoMgmt.AddUpdateVisitorInfo(objEntity, out ReturnCode, out ReturnMsg, out ReturnInquiryNo, out ReturnVisitorId);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;

                    if (ReturnVisitorId > 0)
                    {
                        if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        {
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            // SAVE - Visitor Images
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                            string rootFolderPathImages = Server.MapPath("visitorimages");
                            string filesToDeleteImage = @"visitor-image-" + hdnpkID.Value.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileListImage = System.IO.Directory.GetFiles(rootFolderPathImages, filesToDeleteImage);
                            hdnpkID.Value = @ReturnVisitorId.ToString();
                            foreach (string file in fileListImage)
                            {
                                System.IO.File.Copy(file, file.Replace("visitor-image-", "visitor-image-" + hdnpkID.Value.Trim()));
                                System.IO.File.Delete(file);
                            }

                            // SAVE - Visitor Documents
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            string deleteVisitorImage_hdnpkID = "";
                            string rootFolderPathDocument = Server.MapPath("visitordocuments");
                            string filesToDeleteDocument = @"visitor-document-" + hdnpkID.Value.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
                            deleteVisitorImage_hdnpkID = filesToDeleteDocument.Replace("visitor-document-" + hdnpkID.Value.Trim() + ".*", "visitor-document-" + ".*");
                            string[] fileListDocument = System.IO.Directory.GetFiles(rootFolderPathDocument, deleteVisitorImage_hdnpkID);
                            hdnpkID.Value = @ReturnVisitorId.ToString();
                            foreach (string file in fileListDocument)
                            {
                                System.IO.File.Copy(file, file.Replace("visitor-document-", "visitor-document-" + hdnpkID.Value.Trim()));
                                System.IO.File.Delete(file);
                            }
                        }
                    }
                }
            }
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
            txtInquiryNo.Text = "";
            txtVisitDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtVisitTime.Text = DateTime.Now.ToString("hh:mm tt");
            txtVisitorName.Text = "";
            txtVisitorContact.Text = "";
            txtVisitorEmail.Text = "";
            txtPurposeOfVisit.Text = "";
            drpDepartment.SelectedValue = "";
            drpMeetingTo.SelectedValue = "";

            txtCompanyName.Text = "";
            txtCompanyContact.Text = "";
            txtCompanyAddress.Text = "";
            drpCountry.SelectedValue = "";
            drpState.SelectedValue = "";
            drpCity.SelectedValue = "";
            txtPinCode.Text = "";

            txtVisitDate.Focus();
            btnSave.Disabled = false;
        }

        protected void drpDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpMeetingTo.Items.Clear();

            List<Entity.OrganizationStructure> lstOrgDept2 = new List<Entity.OrganizationStructure>();
            //// ---------------- Meeting To List  -------------------------------------
            List<Entity.OrganizationEmployee> lstReportTo = new List<Entity.OrganizationEmployee>();
            if (!String.IsNullOrEmpty(drpDepartment.SelectedValue))
            {
                lstReportTo = BAL.OrganizationEmployeeMgmt.GetOrgEmployeeByOrgName(drpDepartment.SelectedValue);
            }

            else
            {
                lstReportTo = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            }

            drpMeetingTo.DataSource = lstReportTo;
            drpMeetingTo.DataValueField = "EmployeeName";
            drpMeetingTo.DataTextField = "EmployeeName";
            drpMeetingTo.DataBind();
            drpMeetingTo.Items.Insert(0, new ListItem("-- Select Meeting To --", ""));
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpState.Items.Clear();
            List<Entity.State> lstEvents = new List<Entity.State>();
            if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
            {
                lstEvents = BAL.StateMgmt.GetStateList(Convert.ToString(drpCountry.SelectedValue));
            }
            else
            {
                lstEvents = BAL.StateMgmt.GetStateList();
            }

            drpState.DataSource = lstEvents;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new ListItem("-- All State --", ""));

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

        [System.Web.Services.WebMethod]
        public static string DeleteVisitorInfo(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.VisitorInfoMgmt.DeleteVisitorInfo(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}