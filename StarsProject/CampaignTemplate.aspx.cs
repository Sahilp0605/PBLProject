using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace StarsProject
{
    public partial class CampaignTemplate : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ------------------------------------------------------
                // Category : 'Email' or 'SMS'
                // ------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["category"]))
                {
                    hdnCategory.Value = Request.QueryString["Category"].ToString();
                }
                // ------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnCampaignID.Value = Request.QueryString["id"].ToString();

                    if (hdnCampaignID.Value == "0" || hdnCampaignID.Value == "")
                    {
                        hdnCampaignID.Value = "0";
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
                if (FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.HasFile)
                    {
                        string filePath = FileUpload1.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            string rootFolderPath = Server.MapPath("otherimages");
                            string filesToDelete = @"campaign-" + hdnCampaignID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            foreach (string file in fileList)
                            {
                                System.IO.File.Delete(file);
                            }
                            // -----------------------------------------------------
                            String flname = "campaign-" + hdnCampaignID.Value.Trim() + ext;
                            FileUpload1.SaveAs(Server.MapPath("otherimages/") + flname);
                            imgProduct.ImageUrl = "";
                            imgProduct.ImageUrl = "otherimages/" + flname;
                            hdnFileName.Value = flname;
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                    }
                }
            }
        }
        public void ClearAllField()
        {
            hdnFileName.Value = "";
            txtCampaignSubject.Text = "";
            txtCampaignHeader.Text = "";
            txtCampaignFooter.Text = "";
            //drpCampaignCategory.SelectedValue = "";
        }
        public void OnlyViewControls()
        {
            txtCampaignSubject.ReadOnly = true;
            txtCampaignHeader.ReadOnly = true;
            txtCampaignFooter.ReadOnly = true;
            //drpCampaignCategory.Attributes.Add("disabled", "disabled");
            FileUpload1.Enabled = false;
            btnSave.Visible = false;
            btnReset.Visible = false;

        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.CampaignTemplate> lstEntity = new List<Entity.CampaignTemplate>();

                lstEntity = BAL.CampaignTemplateMgmt.GetCampaignTemplate(Convert.ToInt64(hdnCampaignID.Value), 1, 1, out TotalCount);
                hdnCampaignID.Value = (!String.IsNullOrEmpty(lstEntity[0].CampaignID.ToString())) ? lstEntity[0].CampaignID.ToString() : "";
                //drpCampaignCategory.SelectedValue = lstEntity[0].CampaignCategory;
                txtCampaignSubject.Text = (!String.IsNullOrEmpty(lstEntity[0].CampaignSubject)) ? lstEntity[0].CampaignSubject.Trim() : "";
                txtCampaignHeader.Text = (!String.IsNullOrEmpty(lstEntity[0].CampaignHeader)) ? lstEntity[0].CampaignHeader.Trim() : "";
                txtCampaignFooter.Text = (!String.IsNullOrEmpty(lstEntity[0].CampaignFooter)) ? lstEntity[0].CampaignFooter.Trim() : "";
                imgProduct.ImageUrl = "~/otherimages/" + lstEntity[0].CampaignImageUrl;
                // ----------------------------------------------------------
                divSignOff.Visible = (hdnCategory.Value == "Email") ? true : false;
                divImage.Visible = (hdnCategory.Value == "Email") ? true : false;
                lblSubject.InnerText = (hdnCategory.Value == "Email") ? "Mail Subject" : "SMS Subject";
                lblContent.InnerText = (hdnCategory.Value == "Email") ? "Mail Content" : "SMS Content";
                // ----------------------------------------------------------
                txtCampaignSubject.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            _pageValid = true;
            _pageErrMsg = "";

            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                Int64 ReturnCampaignID = 0;
                // --------------------------------------------------------------
                Entity.CampaignTemplate objEntity = new Entity.CampaignTemplate();

                if (!String.IsNullOrEmpty(hdnCampaignID.Value))
                    objEntity.CampaignID = Convert.ToInt64(hdnCampaignID.Value);

                objEntity.CampaignCategory = hdnCategory.Value; //  drpCampaignCategory.SelectedValue;
                objEntity.CampaignSubject = (!String.IsNullOrEmpty(txtCampaignSubject.Text)) ? txtCampaignSubject.Text.Trim()  : "";
                objEntity.CampaignHeader = (!String.IsNullOrEmpty(txtCampaignHeader.Text)) ? txtCampaignHeader.Text.Trim() : "";
                if  (hdnCategory.Value == "Email")
                {
                    objEntity.CampaignFooter = (!String.IsNullOrEmpty(txtCampaignFooter.Text)) ? txtCampaignFooter.Text.Trim() : "";
                    //objEntity.CampaignImageUrl = imgProduct.ImageUrl;
                    objEntity.CampaignImageUrl = hdnFileName.Value;
                }
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CampaignTemplateMgmt.AddUpdateCampaignTemplate(objEntity, out ReturnCode, out ReturnMsg, out ReturnCampaignID);
                // --------------------------------------------------------------
                _pageErrMsg += (hdnCategory.Value == "Email") ?  "<li> Email " + ReturnMsg + "</li>" :  "<li>SMS " + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    if (ReturnCampaignID > 0)
                    {
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Images
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        if ((hdnCampaignID.Value == "0" || hdnCampaignID.Value == "") && hdnCategory.Value == "Email")
                        {
                            string rootFolderPath = Server.MapPath("otherimages");
                            string filesToDelete = @"campaign-" + hdnCampaignID.Value.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            hdnCampaignID.Value = ReturnCampaignID.ToString();
                            foreach (string file in fileList)
                            {
                                System.IO.File.Copy(file, file.Replace("campaign-0", "campaign-" + hdnCampaignID.Value.Trim()));
                                System.IO.File.Delete(file);
                            }
                        }

                    }
                }
                // --------------------------------------------------------------
                
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + _pageErrMsg + "');", true);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteCampaignTemplate(string CampaignID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.CampaignTemplateMgmt.DeleteCampaignTemplate(Convert.ToInt64(CampaignID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpCampaignCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (drpCampaignCategory.SelectedValue.ToString() == "Email")
            //{
            //    txtCampaignFooter.ReadOnly = false;
            //    FileUpload1.Enabled = true;
            //}
            //else if (drpCampaignCategory.SelectedValue.ToString() == "SMS")
            //{
            //    txtCampaignFooter.ReadOnly = true;
            //    FileUpload1.Enabled = false;
            //}
        }
    }
}