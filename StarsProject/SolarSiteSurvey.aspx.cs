using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using iTextSharp.text.html;

namespace StarsProject
{
    public partial class SolarSiteSurvey : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        if (!String.IsNullOrEmpty(Request.QueryString["CustomerId"]))
                        {
                            hdnCustomerID.Value = (!String.IsNullOrEmpty(Request.QueryString["CustomerId"])) ? Request.QueryString["CustomerId"] : "";
                        }
                        BindSiteSurvayDocuments("0", "sitephoto");
                        BindSiteSurvayDocuments("0", "sitevideo");
                        BindSiteSurvayDocuments("0", "transnameplate");
                        BindSiteSurvayDocuments("0", "sketchwithobject");
                        BindSiteSurvayDocuments("0", "roofplan");
                        BindSiteSurvayDocuments("0", "electricitybill");
                        BindSiteSurvayDocuments("0", "earthresistivity");
                        BindSiteSurvayDocuments("0", "earthpil");
                        BindSiteSurvayDocuments("0", "purlindistance");
                        BindSiteSurvayDocuments("0", "structurestability");
                        BindSiteSurvayDocuments("0", "skylight");
                        BindSiteSurvayDocuments("0", "soiltest");
                        BindSiteSurvayDocuments("0", "contoursurvey");
                    }
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            hdnMode.Value = Request.QueryString["mode"].ToString();
                            if (hdnMode.Value.ToLower() == "view")
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

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"sitephoto" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "sitephoto" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    uploadDocument.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtSitePhotos = new DataTable();
                                    dtSitePhotos = (DataTable)Session["dtSitePhotos"];
                                    Int64 cntRow = dtSitePhotos.Rows.Count + 1;
                                    DataRow dr = dtSitePhotos.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "sitephoto";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtSitePhotos.Rows.Add(dr);
                                    Session.Add("dtSitePhotos", dtSitePhotos);
                                    // ---------------------------------------------------------------
                                    rptSitePhoto.DataSource = dtSitePhotos;
                                    rptSitePhoto.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (uploadVideo.PostedFile != null)
                {
                    if (uploadVideo.PostedFile.FileName.Length > 0)
                    {
                        if (uploadVideo.HasFile)
                        {
                            string filePath = uploadVideo.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext == ".mp4")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"sitevideo" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "sitevideo" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    uploadVideo.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtSiteVideo = new DataTable();
                                    dtSiteVideo = (DataTable)Session["dtSiteVideo"];
                                    Int64 cntRow = dtSiteVideo.Rows.Count + 1;
                                    DataRow dr = dtSiteVideo.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["Filetype"] = "sitevideo";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtSiteVideo.Rows.Add(dr);
                                    Session.Add("dtSiteVideo", dtSiteVideo);
                                    // ---------------------------------------------------------------
                                    rptSiteVideo.DataSource = dtSiteVideo;
                                    rptSiteVideo.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadTransNamePlate.PostedFile != null)
                {
                    if (UploadTransNamePlate.PostedFile.FileName.Length > 0)
                    {
                        if (UploadTransNamePlate.HasFile)
                        {
                            string filePath = UploadTransNamePlate.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"transnameplate" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "transnameplate" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadTransNamePlate.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtTransNamePlate = new DataTable();
                                    dtTransNamePlate = (DataTable)Session["dtTransNamePlate"];
                                    Int64 cntRow = dtTransNamePlate.Rows.Count + 1;
                                    DataRow dr = dtTransNamePlate.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "transnameplate";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtTransNamePlate.Rows.Add(dr);
                                    Session.Add("dtTransNamePlate", dtTransNamePlate);
                                    // ---------------------------------------------------------------
                                    rptTransNamePlate.DataSource = dtTransNamePlate;
                                    rptTransNamePlate.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadSiteSketchWithObject.PostedFile != null)
                {
                    if (UploadSiteSketchWithObject.PostedFile.FileName.Length > 0)
                    {
                        if (UploadSiteSketchWithObject.HasFile)
                        {
                            string filePath = UploadSiteSketchWithObject.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"sketchwithobject" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "sketchwithobjet" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadSiteSketchWithObject.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtSketchWithObject = new DataTable();
                                    dtSketchWithObject = (DataTable)Session["dtSketchWithObject"];
                                    Int64 cntRow = dtSketchWithObject.Rows.Count + 1;
                                    DataRow dr = dtSketchWithObject.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "sketchwithobject";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtSketchWithObject.Rows.Add(dr);
                                    Session.Add("dtSketchwithObject", dtSketchWithObject);
                                    // ---------------------------------------------------------------
                                    rptSiteSketchWithObject.DataSource = dtSketchWithObject;
                                    rptSiteSketchWithObject.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadRoofPlan.PostedFile != null)
                {
                    if (UploadRoofPlan.PostedFile.FileName.Length > 0)
                    {
                        if (UploadRoofPlan.HasFile)
                        {
                            string filePath = UploadRoofPlan.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"roofplan" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "roofplan" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadRoofPlan.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtRoofPlan = new DataTable();
                                    dtRoofPlan = (DataTable)Session["dtRoofPlan"];
                                    Int64 cntRow = dtRoofPlan.Rows.Count + 1;
                                    DataRow dr = dtRoofPlan.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "roofplan";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtRoofPlan.Rows.Add(dr);
                                    Session.Add("dtRoofPlan", dtRoofPlan);
                                    // ---------------------------------------------------------------
                                    rptRoofPlan.DataSource = dtRoofPlan;
                                    rptRoofPlan.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadElectricityBill.PostedFile != null)
                {
                    if (UploadElectricityBill.PostedFile.FileName.Length > 0)
                    {
                        if (UploadElectricityBill.HasFile)
                        {
                            string filePath = UploadElectricityBill.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"electricitybill" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "electricitybill" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadElectricityBill.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtElectricityBill = new DataTable();
                                    dtElectricityBill = (DataTable)Session["dtElectricityBill"];
                                    Int64 cntRow = dtElectricityBill.Rows.Count + 1;
                                    DataRow dr = dtElectricityBill.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "electricitybill";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtElectricityBill.Rows.Add(dr);
                                    Session.Add("dtElectricityBill", dtElectricityBill);
                                    // ---------------------------------------------------------------
                                    rptElectricityBill.DataSource = dtElectricityBill;
                                    rptElectricityBill.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadEarthResistivity.PostedFile != null)
                {
                    if (UploadEarthResistivity.PostedFile.FileName.Length > 0)
                    {
                        if (UploadEarthResistivity.HasFile)
                        {
                            string filePath = UploadEarthResistivity.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"earthresistivity" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "earthresistivity" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadEarthResistivity.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtEarthResistivity = new DataTable();
                                    dtEarthResistivity = (DataTable)Session["dtEarthResistivity"];
                                    Int64 cntRow = dtEarthResistivity.Rows.Count + 1;
                                    DataRow dr = dtEarthResistivity.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "earthresistivity";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtEarthResistivity.Rows.Add(dr);
                                    Session.Add("dtEarthResistivity", dtEarthResistivity);
                                    // ---------------------------------------------------------------
                                    rptEarthResistivity.DataSource = dtEarthResistivity;
                                    rptEarthResistivity.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadEarthPil.PostedFile != null)
                {
                    if (UploadEarthPil.PostedFile.FileName.Length > 0)
                    {
                        if (UploadEarthPil.HasFile)
                        {
                            string filePath = UploadEarthPil.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"earthpil" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "earthpill" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadEarthPil.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtEarthPil = new DataTable();
                                    dtEarthPil = (DataTable)Session["dtEarthPil"];
                                    Int64 cntRow = dtEarthPil.Rows.Count + 1;
                                    DataRow dr = dtEarthPil.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "earthpil";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtEarthPil.Rows.Add(dr);
                                    Session.Add("dtEarthPil", dtEarthPil);
                                    // ---------------------------------------------------------------
                                    rptEarthPil.DataSource = dtEarthPil;
                                    rptEarthPil.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadPurlinDistance.PostedFile != null)
                {
                    if (UploadPurlinDistance.PostedFile.FileName.Length > 0)
                    {
                        if (UploadPurlinDistance.HasFile)
                        {
                            string filePath = UploadPurlinDistance.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"purlindistance" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "purlindistance" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadPurlinDistance.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtPurlinDistance = new DataTable();
                                    dtPurlinDistance = (DataTable)Session["dtPurlinDistance"];
                                    Int64 cntRow = dtPurlinDistance.Rows.Count + 1;
                                    DataRow dr = dtPurlinDistance.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "purlindistance";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtPurlinDistance.Rows.Add(dr);
                                    Session.Add("dtPurlinDistance", dtPurlinDistance);
                                    // ---------------------------------------------------------------
                                    rptPurlinDistance.DataSource = dtPurlinDistance;
                                    rptPurlinDistance.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadStructureStability.PostedFile != null)
                {
                    if (UploadStructureStability.PostedFile.FileName.Length > 0)
                    {
                        if (UploadStructureStability.HasFile)
                        {
                            string filePath = UploadStructureStability.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"structurestability" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "structurestability" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadStructureStability.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtStructureStability = new DataTable();
                                    dtStructureStability = (DataTable)Session["dtStructureStability"];
                                    Int64 cntRow = dtStructureStability.Rows.Count + 1;
                                    DataRow dr = dtStructureStability.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "structurestability";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtStructureStability.Rows.Add(dr);
                                    Session.Add("dtStructureStability", dtStructureStability);
                                    // ---------------------------------------------------------------
                                    rptStructureStability.DataSource = dtStructureStability;
                                    rptStructureStability.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadSkylights.PostedFile != null)
                {
                    if (UploadSkylights.PostedFile.FileName.Length > 0)
                    {
                        if (UploadSkylights.HasFile)
                        {
                            string filePath = UploadSkylights.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"skylight" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "skylight" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadSkylights.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtSkyLight = new DataTable();
                                    dtSkyLight = (DataTable)Session["dtSkyLight"];
                                    Int64 cntRow = dtSkyLight.Rows.Count + 1;
                                    DataRow dr = dtSkyLight.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "skylight";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtSkyLight.Rows.Add(dr);
                                    Session.Add("dtSkyLight", dtSkyLight);
                                    // ---------------------------------------------------------------
                                    rptSkylights.DataSource = dtSkyLight;
                                    rptSkylights.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadSoilTest.PostedFile != null)
                {
                    if (UploadSoilTest.PostedFile.FileName.Length > 0)
                    {
                        if (UploadSoilTest.HasFile)
                        {
                            string filePath = UploadSoilTest.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"soiltest" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "soiltest" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadSoilTest.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtSoilTest = new DataTable();
                                    dtSoilTest = (DataTable)Session["dtSoilTest"];
                                    Int64 cntRow = dtSoilTest.Rows.Count + 1;
                                    DataRow dr = dtSoilTest.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "soiltest";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtSoilTest.Rows.Add(dr);
                                    Session.Add("dtSoilTest", dtSoilTest);
                                    // ---------------------------------------------------------------
                                    rptSoilTest.DataSource = dtSoilTest;
                                    rptSoilTest.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                if (UploadContourSurvey.PostedFile != null)
                {
                    if (UploadContourSurvey.PostedFile.FileName.Length > 0)
                    {
                        if (UploadContourSurvey.HasFile)
                        {
                            string filePath = UploadContourSurvey.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("SiteDocs");
                                    string filesToDelete = @"contoursurvey" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "contoursurvey" + "-" + filename1;
                                    String tmpFile = Server.MapPath("SiteDocs/") + flname;
                                    UploadContourSurvey.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtContourSurvey = new DataTable();
                                    dtContourSurvey = (DataTable)Session["dtContourSurvey"];
                                    Int64 cntRow = dtContourSurvey.Rows.Count + 1;
                                    DataRow dr = dtContourSurvey.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["DocNo"] = txtDocNo.Text;
                                    dr["FileName"] = flname;
                                    dr["FileType"] = "contoursurvey";
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dtContourSurvey.Rows.Add(dr);
                                    Session.Add("dtContourSurvey", dtContourSurvey);
                                    // ---------------------------------------------------------------
                                    rptContourSurvey.DataSource = dtContourSurvey;
                                    rptContourSurvey.DataBind();
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
        }
        public void BindSiteSurvayDocuments(String DocNo, String Type)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "sitephoto");
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptSitePhoto.DataSource = dtDetail1;
            rptSitePhoto.DataBind();
            Session.Add("dtSitePhotos", dtDetail1);

            DataTable dtDetail2 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst2 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "sitevideo");
            dtDetail2 = PageBase.ConvertListToDataTable(lst2);
            rptSiteVideo.DataSource = dtDetail2;
            rptSiteVideo.DataBind();
            Session.Add("dtSiteVideo", dtDetail2);

            DataTable dtDetail3 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst3 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "transnameplate");
            dtDetail3 = PageBase.ConvertListToDataTable(lst3);
            rptTransNamePlate.DataSource = dtDetail3;
            rptTransNamePlate.DataBind();
            Session.Add("dtTransNamePlate", dtDetail3);

            DataTable dtDetail4 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst4 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "sketchwithobject");
            dtDetail4 = PageBase.ConvertListToDataTable(lst4);
            rptSiteSketchWithObject.DataSource = dtDetail4;
            rptSiteSketchWithObject.DataBind();
            Session.Add("dtSketchWithObject", dtDetail4);

            DataTable dtDetail5 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst5 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "roofplan");
            dtDetail5 = PageBase.ConvertListToDataTable(lst5);
            rptRoofPlan.DataSource = dtDetail5;
            rptRoofPlan.DataBind();
            Session.Add("dtRoofPlan", dtDetail5);

            DataTable dtDetail6 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst6 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "electricitybill");
            dtDetail6 = PageBase.ConvertListToDataTable(lst6);
            rptElectricityBill.DataSource = dtDetail6;
            rptElectricityBill.DataBind();
            Session.Add("dtElectricityBill", dtDetail6);

            DataTable dtDetail7 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst7 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "earthresistivity");
            dtDetail7 = PageBase.ConvertListToDataTable(lst7);
            rptEarthResistivity.DataSource = dtDetail7;
            rptEarthResistivity.DataBind();
            Session.Add("dtEarthResistivity", dtDetail7);

            DataTable dtDetail8 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst8 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "earthpil");
            dtDetail8 = PageBase.ConvertListToDataTable(lst8);
            rptEarthPil.DataSource = dtDetail8;
            rptEarthPil.DataBind();
            Session.Add("dtEarthPil", dtDetail8);

            DataTable dtDetail9 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst9 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "purlindistance");
            dtDetail9 = PageBase.ConvertListToDataTable(lst9);
            rptPurlinDistance.DataSource = dtDetail9;
            rptPurlinDistance.DataBind();
            Session.Add("dtPurlinDistance", dtDetail9);

            DataTable dtDetail10 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst10 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "structurestability");
            dtDetail10 = PageBase.ConvertListToDataTable(lst10);
            rptStructureStability.DataSource = dtDetail10;
            rptStructureStability.DataBind();
            Session.Add("dtStructureStability", dtDetail10);

            DataTable dtDetail11 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst11 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "skylight");
            dtDetail11 = PageBase.ConvertListToDataTable(lst11);
            rptSkylights.DataSource = dtDetail11;
            rptSkylights.DataBind();
            Session.Add("dtSkyLight", dtDetail11);

            DataTable dtDetail12 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst12 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "soiltest");
            dtDetail12 = PageBase.ConvertListToDataTable(lst12);
            rptSoilTest.DataSource = dtDetail12;
            rptSoilTest.DataBind();
            Session.Add("dtSoilTest", dtDetail12);

            DataTable dtDetail13 = new DataTable();
            List<Entity.SiteSurvayDocuments> lst13 = BAL.SiteSurvayMgmt.GetSiteSurvayDocumentsList(0, DocNo, "contoursurvey");
            dtDetail13 = PageBase.ConvertListToDataTable(lst13);
            rptContourSurvey.DataSource = dtDetail13;
            rptContourSurvey.DataBind();
            Session.Add("dtContourSurvey", dtDetail13);
        }
        public void ClearAllField()
        {
            txtDocNo.Text = "";
            txtSheetNo.Text = "";
            txtSurveyDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtContPerson1.Text = "";
            txtContNo1.Text = "";
            txtContAddress1.Text = "";
            txtContEmail1.Text = "";
            txtContDesignation1.Text = "";
            txtContPerson2.Text = "";
            txtContNo2.Text = "";
            txtContAddress2.Text = "";
            txtContEmail2.Text = "";
            txtContDesignation2.Text = "";
            txtSiteAddress.Text = "";
            txtLatitude.Text = "";
            txtLongitude.Text = "";
            txtAltitude.Text = "";
            txtNearByRailwayStation.Text = "";
            txtNearByAirport.Text = "";
            txtWaterElectricity.Text = "";
            txtRoofTopRCCLocation.Text = "";
            txtRoofTopMetalSheetLocation.Text = "";
            txtGroundMountLocation.Text = "";
            txtStructureType.Text = "";
            txtRoofTopRCCTildAngle.Text = "";
            txtRoofTopMetalSheetTildAngle.Text = "";
            txtGroundMountTildAngle.Text = "";
            txtRoofTopRCCArea.Text = "";
            txtRoofTopMetalSheetArea.Text = "";
            txtGroundMountArea.Text = "";
            txtRoofTopRCCOrientation.Text = "";
            txtRoofTopMetalSheetOrientation.Text = "";
            txtGroundMountOrientation.Text = "";
            txtPenetrationAllowed.Text = "";
            txtOnGridDGRating.Text = "";
            txtOffGridDGRating.Text = "";
            txtHybridDGRating.Text = "";
            txtOnGridContractDemand.Text = "";
            txtOffGridContractDemand.Text = "";
            txtHybridContractDemand.Text = "";
            txtOnGridInstallationCapacity.Text = "";
            txtOffGridInstallationCapacity.Text = "";
            txtHybridInstallationCapacity.Text = "";
            txtInstallationType.Text = "";
            txtDGSynchronisation.Text = "";
            txtDGOperationMode.Text = "";
            txtDataMonitoring.Text = "";
            txtWeatherMonitoringSystem.Text = "";
            txtBreaker.Text = "";
            txtBusBarTypeSize.Text = "";
            txtKVARating.Text = "";
            txtPrimaryVoltage.Text = "";
            txtSecondaryVoltage.Text = "";
            txtImpedance.Text = "";
            txtVectorGrp.Text = "";

            rptSitePhoto.DataSource = null;
            rptSitePhoto.DataBind();

            rptSiteVideo.DataSource = null;
            rptSiteVideo.DataBind();

            rptTransNamePlate.DataSource = null;
            rptTransNamePlate.DataBind();

            rptSiteSketchWithObject.DataSource = null;
            rptSiteSketchWithObject.DataBind();

            txtOMRequirements.Text = "";

            txtModuleCleaningRequirements.Text = "";

            txtRoofPlan.Text = "";
            rptRoofPlan.DataSource = null;
            rptRoofPlan.DataBind();

            txtLoadDetails.Text = "";

            rptElectricityBill.DataSource = null;
            rptElectricityBill.DataBind();

            txtEarthResistive.Text = "";
            rptEarthResistivity.DataSource = null;
            rptEarthResistivity.DataBind();

            txtEarthPil.Text = "";
            rptEarthPil.DataSource = null;
            rptEarthPil.DataBind();

            txtDistanceFromElectricRoom.Text = "";

            txtSheetType.Text = "";

            txtPurlinDistance.Text = "";
            rptPurlinDistance.DataSource = null;
            rptPurlinDistance.DataBind();

            txtRoofSheet.Text = "";

            txtStructureStability.Text = "";
            rptStructureStability.DataSource = null;
            rptStructureStability.DataBind();

            txtSkylight.Text = "";
            rptSkylights.DataSource = null;
            rptSkylights.DataBind();

            txtLadderRoof.Text = "";

            txtSoilTest.Text = "";
            rptSoilTest.DataSource = null;
            rptSoilTest.DataBind();

            txtContourSurvey.Text = "";
            rptContourSurvey.DataSource = null;
            rptContourSurvey.DataBind();

            txtTilt.Text = "";

            txtInverter.Text = "";

            btnSave.Disabled = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.SiteSurvay> lstEntity = new List<Entity.SiteSurvay>();
                // ----------------------------------------------------
                lstEntity = BAL.SiteSurvayMgmt.GetSiteSurvay(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtDocNo.Text = lstEntity[0].DocNo.ToString();
                txtSheetNo.Text = lstEntity[0].SheetNo.ToString();
                txtSurveyDate.Text = (lstEntity[0].SurvayDate.Year <= 1900) ? "" : lstEntity[0].SurvayDate.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustID.ToString();
                txtCustomerName.Text = lstEntity[0].Customer.ToString();
                txtContPerson1.Text = lstEntity[0].ContPerson1.ToString();
                txtContNo1.Text = lstEntity[0].ContNo1.ToString();
                txtContAddress1.Text = lstEntity[0].ContAddress1.ToString();
                txtContEmail1.Text = lstEntity[0].ContEmail1.ToString();
                txtContDesignation1.Text = lstEntity[0].ContDesignation1.ToString();
                txtContPerson2.Text = lstEntity[0].ContPerson2.ToString();
                txtContNo2.Text = lstEntity[0].ContNo2.ToString();
                txtContAddress2.Text = lstEntity[0].ContAddress2.ToString();
                txtContEmail2.Text = lstEntity[0].ContEmail2.ToString();
                txtContDesignation2.Text = lstEntity[0].ContDesignation2.ToString();
                txtSiteAddress.Text = lstEntity[0].SiteAddress.ToString();
                txtLatitude.Text = String.IsNullOrEmpty(lstEntity[0].Latitude.ToString()) ? "0" : lstEntity[0].Latitude.ToString();
                txtLongitude.Text = String.IsNullOrEmpty(lstEntity[0].Longitude.ToString()) ? "0" : lstEntity[0].Longitude.ToString();
                txtAltitude.Text = String.IsNullOrEmpty(lstEntity[0].Altitude.ToString()) ? "0" : lstEntity[0].Altitude.ToString();
                txtNearByRailwayStation.Text = lstEntity[0].NearByRailwayStation.ToString();
                txtNearByAirport.Text = lstEntity[0].NearByAirport.ToString();
                txtWaterElectricity.Text = lstEntity[0].WaterAndElectricity.ToString();
                txtRoofTopRCCLocation.Text = lstEntity[0].RoofTopRCCLocation.ToString();
                txtRoofTopMetalSheetLocation.Text = lstEntity[0].RoofTopMetalSheetLocation.ToString();
                txtGroundMountLocation.Text = lstEntity[0].GroundMountLocation.ToString();
                txtStructureType.Text = lstEntity[0].StructureType.ToString();
                txtRoofTopRCCTildAngle.Text = String.IsNullOrEmpty(lstEntity[0].RoofTopRCCTiltAngle.ToString()) ? "0" : lstEntity[0].RoofTopRCCTiltAngle.ToString();
                txtRoofTopMetalSheetTildAngle.Text = String.IsNullOrEmpty(lstEntity[0].RoofTopMetalSheetTiltAngle.ToString()) ? "0" : lstEntity[0].RoofTopMetalSheetTiltAngle.ToString();
                txtGroundMountTildAngle.Text = String.IsNullOrEmpty(lstEntity[0].GroundMountTiltAngle.ToString()) ? "0" : lstEntity[0].GroundMountTiltAngle.ToString();
                txtRoofTopRCCArea.Text = String.IsNullOrEmpty(lstEntity[0].RoofTopRCCArea.ToString()) ? "0" : lstEntity[0].RoofTopRCCArea.ToString();
                txtRoofTopMetalSheetArea.Text = String.IsNullOrEmpty(lstEntity[0].RoofTopMetalSheetArea.ToString()) ? "0" : lstEntity[0].RoofTopMetalSheetArea.ToString();
                txtGroundMountArea.Text = String.IsNullOrEmpty(lstEntity[0].GroundMountArea.ToString()) ? "0" : lstEntity[0].GroundMountArea.ToString();
                txtRoofTopRCCOrientation.Text = String.IsNullOrEmpty(lstEntity[0].RoofTopRCCOrientation.ToString()) ? "" : lstEntity[0].RoofTopRCCOrientation.ToString();
                txtRoofTopMetalSheetOrientation.Text = String.IsNullOrEmpty(lstEntity[0].RoofTopMetalSheetOrientation.ToString()) ? "" : lstEntity[0].RoofTopMetalSheetOrientation.ToString();
                txtGroundMountOrientation.Text = String.IsNullOrEmpty(lstEntity[0].GroundMountOrientation.ToString()) ? "" : lstEntity[0].GroundMountOrientation.ToString();
                txtPenetrationAllowed.Text = lstEntity[0].PenetrationAllowed.ToString();
                txtOnGridDGRating.Text = lstEntity[0].OnGridDGRating.ToString();
                txtOffGridDGRating.Text = lstEntity[0].OffGridDGRating.ToString();
                txtHybridDGRating.Text = lstEntity[0].HybridDGRating.ToString();
                txtOnGridContractDemand.Text = lstEntity[0].OnGridContractDemand.ToString();
                txtOffGridContractDemand.Text = lstEntity[0].OffGridContractDemand.ToString();
                txtHybridContractDemand.Text = lstEntity[0].HybridContractDemand.ToString();
                txtOnGridInstallationCapacity.Text = String.IsNullOrEmpty(lstEntity[0].OnGridCapacity.ToString()) ? "0" : lstEntity[0].OnGridCapacity.ToString();
                txtOffGridInstallationCapacity.Text = String.IsNullOrEmpty(lstEntity[0].OffGridCapacity.ToString()) ? "0" : lstEntity[0].OffGridCapacity.ToString();
                txtHybridInstallationCapacity.Text = String.IsNullOrEmpty(lstEntity[0].HybridCapacity.ToString()) ? "0" : lstEntity[0].HybridCapacity.ToString();
                txtInstallationType.Text = lstEntity[0].InstalationType.ToString();
                txtDGSynchronisation.Text = lstEntity[0].DGSynchronisation.ToString();
                txtDGOperationMode.Text = lstEntity[0].DGOperationMode.ToString();
                txtDataMonitoring.Text = lstEntity[0].DataMonitoring.ToString();
                txtWeatherMonitoringSystem.Text = lstEntity[0].WeatherMonitoringSystem.ToString();
                txtBreaker.Text = lstEntity[0].AvailableBreaker.ToString();
                txtBusBarTypeSize.Text = lstEntity[0].BusBarTypeAndSize.ToString();
                txtKVARating.Text = String.IsNullOrEmpty(lstEntity[0].KVARating.ToString()) ? "0" : lstEntity[0].KVARating.ToString();
                txtPrimaryVoltage.Text = String.IsNullOrEmpty(lstEntity[0].PrimaryVolt.ToString()) ? "0" : lstEntity[0].PrimaryVolt.ToString();
                txtSecondaryVoltage.Text = String.IsNullOrEmpty(lstEntity[0].SecondaryVolt.ToString()) ? "0" : lstEntity[0].SecondaryVolt.ToString();
                txtImpedance.Text = String.IsNullOrEmpty(lstEntity[0].Impedance.ToString()) ? "0" : lstEntity[0].Impedance.ToString();
                txtVectorGrp.Text = lstEntity[0].VectorGrp.ToString();
                txtOMRequirements.Text = lstEntity[0].OMRequirements.ToString();
                txtModuleCleaningRequirements.Text = lstEntity[0].ModuleCleaningRequirements.ToString();
                txtRoofPlan.Text = lstEntity[0].RoofPlan.ToString();
                txtLoadDetails.Text = lstEntity[0].LoadDetails.ToString();
                txtEarthResistive.Text = lstEntity[0].EarthResistivity.ToString();
                txtEarthPil.Text = lstEntity[0].EarthPit.ToString();
                txtDistanceFromElectricRoom.Text = lstEntity[0].DistanceFromElectricalRoom.ToString();

                txtSheetType.Text = lstEntity[0].SheetType.ToString();
                txtPurlinDistance.Text = lstEntity[0].PurlinDistance.ToString();
                txtRoofSheet.Text = lstEntity[0].RoofSheet.ToString();
                txtStructureStability.Text = lstEntity[0].StructureStability.ToString();
                txtSkylight.Text = lstEntity[0].Skylight.ToString();
                txtLadderRoof.Text = lstEntity[0].LadderToRoof.ToString();

                txtSoilTest.Text = lstEntity[0].SoilTest.ToString();
                txtContourSurvey.Text = lstEntity[0].ContourSurvey.ToString();
                txtTilt.Text = lstEntity[0].Tilt.ToString();
                txtInverter.Text = lstEntity[0].Inverter.ToString();

                BindSiteSurvayDocuments(txtDocNo.Text, "sitephoto");
                BindSiteSurvayDocuments(txtDocNo.Text, "sitevideo");
                BindSiteSurvayDocuments(txtDocNo.Text, "transnameplate");
                BindSiteSurvayDocuments(txtDocNo.Text, "sketchwithobject");
                BindSiteSurvayDocuments(txtDocNo.Text, "roofplan");
                BindSiteSurvayDocuments(txtDocNo.Text, "electricitybill");
                BindSiteSurvayDocuments(txtDocNo.Text, "earthresistivity");
                BindSiteSurvayDocuments(txtDocNo.Text, "earthpil");
                BindSiteSurvayDocuments(txtDocNo.Text, "purlindistance");
                BindSiteSurvayDocuments(txtDocNo.Text, "structurestability");
                BindSiteSurvayDocuments(txtDocNo.Text, "skylight");
                BindSiteSurvayDocuments(txtDocNo.Text, "soiltest");
                BindSiteSurvayDocuments(txtDocNo.Text, "contoursurvey");

            }
        }
        public void OnlyViewControls()
        {
            txtDocNo.ReadOnly = true;
            txtSheetNo.ReadOnly = true;
            txtSurveyDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtContPerson1.ReadOnly = true;
            txtContNo1.ReadOnly = true;
            txtContAddress1.ReadOnly = true;
            txtContEmail1.ReadOnly = true;
            txtContDesignation1.ReadOnly = true;
            txtContPerson2.ReadOnly = true;
            txtContNo2.ReadOnly = true;
            txtContAddress2.ReadOnly = true;
            txtContEmail2.ReadOnly = true;
            txtContDesignation2.ReadOnly = true;
            txtSiteAddress.ReadOnly = true;
            txtLatitude.ReadOnly = true;
            txtLongitude.ReadOnly = true;
            txtAltitude.ReadOnly = true;
            txtNearByRailwayStation.ReadOnly = true;
            txtNearByAirport.ReadOnly = true;
            txtWaterElectricity.ReadOnly = true;
            txtRoofTopRCCLocation.ReadOnly = true;
            txtRoofTopMetalSheetLocation.ReadOnly = true;
            txtGroundMountLocation.ReadOnly = true;
            txtStructureType.ReadOnly = true;
            txtRoofTopRCCTildAngle.ReadOnly = true;
            txtRoofTopMetalSheetTildAngle.ReadOnly = true;
            txtGroundMountTildAngle.ReadOnly = true;
            txtRoofTopRCCArea.ReadOnly = true;
            txtRoofTopMetalSheetArea.ReadOnly = true;
            txtGroundMountArea.ReadOnly = true;
            txtRoofTopRCCOrientation.ReadOnly = true;
            txtRoofTopMetalSheetOrientation.ReadOnly = true;
            txtGroundMountOrientation.ReadOnly = true;
            txtPenetrationAllowed.ReadOnly = true;
            txtOnGridDGRating.ReadOnly = true;
            txtOffGridDGRating.ReadOnly = true;
            txtHybridDGRating.ReadOnly = true;
            txtOnGridContractDemand.ReadOnly = true;
            txtOffGridContractDemand.ReadOnly = true;
            txtHybridContractDemand.ReadOnly = true;
            txtOnGridInstallationCapacity.ReadOnly = true;
            txtOffGridInstallationCapacity.ReadOnly = true;
            txtHybridInstallationCapacity.ReadOnly = true;
            txtInstallationType.ReadOnly = true;
            txtDGSynchronisation.ReadOnly = true;
            txtDGOperationMode.ReadOnly = true;
            txtDataMonitoring.ReadOnly = true;
            txtWeatherMonitoringSystem.ReadOnly = true;
            txtBreaker.ReadOnly = true;
            txtBusBarTypeSize.ReadOnly = true;
            txtKVARating.ReadOnly = true;
            txtPrimaryVoltage.ReadOnly = true;
            txtSecondaryVoltage.ReadOnly = true;    
            txtImpedance.ReadOnly = true;
            txtVectorGrp.ReadOnly = true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveAndSendData(false, sender);
        }
        public void SaveAndSendData(Boolean paraSaveAndEmail, object sender)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            // --------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnSiteSurvayNo = "";
            // --------------------------------------------------------------
            _pageValid = true;
            string strError = "";

            if(String.IsNullOrEmpty(txtSheetNo.Text) || String.IsNullOrEmpty(txtSurveyDate.Text) || String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    strError += "<li>" + "Select Proper Customer From List." + "</li>";
                txtCustomerName.Focus();

                if(String.IsNullOrEmpty(txtSheetNo.Text))
                    strError += "<li>" + "Select Sheet No." + "</li>";

                if (String.IsNullOrEmpty(txtSurveyDate.Text))
                    strError += "<li>" + "Select Survey Date." + "</li>";

            }
            if (_pageValid)
            {
                Entity.SiteSurvay objEntity = new Entity.SiteSurvay();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.DocNo = txtDocNo.Text;
                objEntity.SheetNo = txtSheetNo.Text;
                objEntity.SurvayDate = Convert.ToDateTime(txtSurveyDate.Text);
                objEntity.CustID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.Customer = txtCustomerName.Text;
                objEntity.ContPerson1 = txtContPerson1.Text;
                objEntity.ContNo1 = txtContNo1.Text;
                objEntity.ContAddress1 = txtContAddress1.Text;
                objEntity.ContEmail1 = txtContEmail1.Text;
                objEntity.ContDesignation1 = txtContDesignation1.Text;
                objEntity.ContPerson2 = txtContPerson2.Text;
                objEntity.ContNo2 = txtContNo2.Text;
                objEntity.ContAddress2 = txtContAddress2.Text;
                objEntity.ContEmail2 = txtContEmail2.Text;
                objEntity.ContDesignation2 = txtContDesignation2.Text;
                objEntity.SiteAddress = txtSiteAddress.Text;
                objEntity.Latitude = String.IsNullOrEmpty(txtLatitude.Text) ? 0 : Convert.ToDecimal(txtLatitude.Text);
                objEntity.Longitude = String.IsNullOrEmpty(txtLongitude.Text) ? 0 : Convert.ToDecimal(txtLongitude.Text);
                objEntity.Altitude = String.IsNullOrEmpty(txtAltitude.Text) ? 0 : Convert.ToDecimal(txtAltitude.Text);
                objEntity.NearByRailwayStation = txtNearByRailwayStation.Text;
                objEntity.NearByAirport = txtNearByAirport.Text;
                objEntity.WaterAndElectricity = txtWaterElectricity.Text;
                objEntity.RoofTopRCCLocation = txtRoofTopRCCLocation.Text;
                objEntity.RoofTopMetalSheetLocation = txtRoofTopMetalSheetLocation.Text;
                objEntity.GroundMountLocation = txtGroundMountLocation.Text;
                objEntity.StructureType = txtStructureType.Text;
                objEntity.RoofTopRCCTiltAngle = String.IsNullOrEmpty(txtRoofTopRCCTildAngle.Text) ? 0 : Convert.ToDecimal(txtRoofTopRCCTildAngle.Text);
                objEntity.RoofTopMetalSheetTiltAngle = String.IsNullOrEmpty(txtRoofTopMetalSheetTildAngle.Text) ? 0 : Convert.ToDecimal(txtRoofTopMetalSheetTildAngle.Text);
                objEntity.GroundMountTiltAngle = String.IsNullOrEmpty(txtGroundMountTildAngle.Text) ? 0 : Convert.ToDecimal(txtGroundMountTildAngle.Text);
                objEntity.RoofTopRCCArea = String.IsNullOrEmpty(txtRoofTopRCCArea.Text) ? 0 : Convert.ToDecimal(txtRoofTopRCCArea.Text);
                objEntity.RoofTopMetalSheetArea = String.IsNullOrEmpty(txtRoofTopMetalSheetArea.Text) ? 0 : Convert.ToDecimal(txtRoofTopMetalSheetArea.Text);
                objEntity.GroundMountArea = String.IsNullOrEmpty(txtGroundMountArea.Text) ? 0 : Convert.ToDecimal(txtGroundMountArea.Text);
                objEntity.RoofTopRCCOrientation = txtRoofTopRCCOrientation.Text;
                objEntity.RoofTopMetalSheetOrientation = txtRoofTopMetalSheetOrientation.Text;
                objEntity.GroundMountOrientation = txtGroundMountOrientation.Text;
                objEntity.PenetrationAllowed = txtPenetrationAllowed.Text;
                objEntity.OnGridDGRating = txtOnGridDGRating.Text;
                objEntity.OffGridDGRating = txtOffGridDGRating.Text;
                objEntity.HybridDGRating = txtHybridDGRating.Text;
                objEntity.OnGridContractDemand = txtOnGridContractDemand.Text;
                objEntity.OffGridContractDemand = txtOffGridContractDemand.Text;
                objEntity.HybridContractDemand = txtHybridContractDemand.Text;
                objEntity.OnGridCapacity = String.IsNullOrEmpty(txtOnGridInstallationCapacity.Text) ? 0 : Convert.ToDecimal(txtOnGridInstallationCapacity.Text);
                objEntity.OffGridCapacity = String.IsNullOrEmpty(txtOffGridInstallationCapacity.Text) ? 0 : Convert.ToDecimal(txtOffGridInstallationCapacity.Text);
                objEntity.HybridCapacity = String.IsNullOrEmpty(txtHybridInstallationCapacity.Text) ? 0 : Convert.ToDecimal(txtHybridInstallationCapacity.Text);
                objEntity.InstalationType = txtInstallationType.Text;
                objEntity.DGSynchronisation = txtDGSynchronisation.Text;
                objEntity.DGOperationMode = txtDGOperationMode.Text;
                objEntity.DataMonitoring = txtDataMonitoring.Text;
                objEntity.WeatherMonitoringSystem = txtWeatherMonitoringSystem.Text;
                objEntity.AvailableBreaker = txtBreaker.Text;
                objEntity.BusBarTypeAndSize = txtBusBarTypeSize.Text;
                objEntity.KVARating = String.IsNullOrEmpty(txtKVARating.Text) ? 0 : Convert.ToDecimal(txtKVARating.Text);
                objEntity.PrimaryVolt = String.IsNullOrEmpty(txtPrimaryVoltage.Text) ? 0 : Convert.ToDecimal(txtPrimaryVoltage.Text);
                objEntity.SecondaryVolt = String.IsNullOrEmpty(txtSecondaryVoltage.Text) ? 0 : Convert.ToDecimal(txtSecondaryVoltage.Text);
                objEntity.Impedance = String.IsNullOrEmpty(txtImpedance.Text) ? 0 : Convert.ToDecimal(txtImpedance.Text);
                objEntity.VectorGrp = txtVectorGrp.Text;
                objEntity.OMRequirements = txtOMRequirements.Text;
                objEntity.ModuleCleaningRequirements = txtModuleCleaningRequirements.Text;
                objEntity.RoofPlan = txtRoofPlan.Text;
                objEntity.LoadDetails = txtLoadDetails.Text;
                objEntity.EarthResistivity = txtEarthResistive.Text;
                objEntity.EarthPit = txtEarthPil.Text;
                objEntity.DistanceFromElectricalRoom = txtDistanceFromElectricRoom.Text;
                objEntity.SheetType = txtSheetType.Text;
                objEntity.PurlinDistance = txtPurlinDistance.Text;
                objEntity.RoofSheet = txtRoofSheet.Text;
                objEntity.StructureStability = txtStructureStability.Text;
                objEntity.Skylight = txtSkylight.Text;
                objEntity.LadderToRoof = txtLadderRoof.Text;
                objEntity.SoilTest = txtSoilTest.Text;
                objEntity.ContourSurvey = txtContourSurvey.Text;
                objEntity.Tilt = txtTilt.Text;
                objEntity.Inverter = txtInverter.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.SiteSurvayMgmt.AddUpdateSiteSurvay(objEntity, out ReturnCode, out ReturnMsg, out ReturnSiteSurvayNo);
                strError += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    txtDocNo.Text = ReturnSiteSurvayNo;
                    btnSave.Disabled = true;
                    
                    string filename1,type;
                    
                    DataTable dtSitePhotos = new DataTable();
                    dtSitePhotos = (DataTable)Session["dtSitePhotos"];
                    // ----------------------------------------------
                    String DocNo = ReturnSiteSurvayNo;
                    if (dtSitePhotos != null)
                    {
                        
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "sitephoto", out ReturnCode1, out ReturnMsg1);
                        if (dtSitePhotos.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSitePhotos.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    filename1 = dr["FileName"].ToString();
                                    type = dr["Filetype"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                                }
                            }
                        }
                    }
                    Session.Remove("dtSitePhotos");

                    //----------------SiteVideo--------------------------------
                    DataTable dtSiteVideo = new DataTable();
                    dtSiteVideo = (DataTable)Session["dtSiteVideo"];
                    // ----------------------------------------------
                    //String DocNo2 = ReturnSiteSurvayNo;
                    if (dtSiteVideo != null)
                    {
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "sitevideo", out ReturnCode1, out ReturnMsg1);
                        if (dtSiteVideo.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSiteVideo.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    filename1 = dr["FileName"].ToString();
                                    type = dr["Filetype"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                                }
                            }
                       }
                    }
                    Session.Remove("dtSiteVideo");

                    //-----------------TransNamePlate------------------------------------------
                    DataTable dtTransNamePlate = new DataTable();
                    dtTransNamePlate = (DataTable)Session["dtTransNamePlate"];
                    // ----------------------------------------------
                    //String DocNo = ReturnSiteSurvayNo;
                    if (dtTransNamePlate != null)
                    {
                        
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "transnameplate", out ReturnCode1, out ReturnMsg1);
                        if (dtTransNamePlate.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtTransNamePlate.Rows)
                            {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtTransNamePlate");
                    //---------------SketchWithObject--------------------------------------
                    DataTable dtSketchWithObject = new DataTable();
                    dtSketchWithObject = (DataTable)Session["dtSketchWithObject"];
                    // ----------------------------------------------
                    //String DocNo = ReturnSiteSurvayNo;
                    if (dtSketchWithObject != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "sketchwithobject", out ReturnCode1, out ReturnMsg1);
                        if (dtSketchWithObject.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSketchWithObject.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtSketchWithObject");

                    //--------------------RoofPlan---------------------------------
                    DataTable dtRoofPlan = new DataTable();
                    dtRoofPlan = (DataTable)Session["dtRoofPlan"];
                    // ----------------------------------------------
                    //String DocNo = ReturnSiteSurvayNo;
                    if (dtRoofPlan != null)
                    {
                          
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "roofplan", out ReturnCode1, out ReturnMsg1);
                        if (dtRoofPlan.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtRoofPlan.Rows)
                            {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtRoofPlan"); 

                     //------------------ElectricityBill-----------------------------
                     DataTable dtElectricityBill = new DataTable();
                    dtElectricityBill = (DataTable)Session["dtElectricityBill"];
                    // ----------------------------------------------
                    //String DocNo = ReturnSiteSurvayNo;
                    if (dtElectricityBill != null)
                    {
                        
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "electricitybill", out ReturnCode1, out ReturnMsg1);
                        if (dtElectricityBill.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtElectricityBill.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtElectricityBill");

                    //-----------------------EarthResistivity--------------------------------------
                    DataTable dtEarthResistivity = new DataTable();
                    dtEarthResistivity = (DataTable)Session["dtEarthResistivity"];
                    // ----------------------------------------------
                    //String DocNo = ReturnSiteSurvayNo;
                    if (dtEarthResistivity != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "earthresistivity", out ReturnCode1, out ReturnMsg1);
                        if (dtEarthResistivity.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtEarthResistivity.Rows)
                            {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtEarthResistivity");

                    //-------------------EarthPil-------------------------------------
                    DataTable dtEarthPil = new DataTable();
                    dtEarthPil = (DataTable)Session["dtEarthPil"];
                    // ----------------------------------------------
                    //String DocNo = ReturnSiteSurvayNo;
                    if (dtEarthPil != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "earthpil", out ReturnCode1, out ReturnMsg1);
                        if (dtEarthPil.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtEarthPil.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtEarthPil");

                    //---------------------PurlinDistance-------------------------------------
                    DataTable dtPurlinDistance = new DataTable();
                    dtPurlinDistance = (DataTable)Session["dtPurlinDistance"];
                    // ----------------------------------------------
                    //String DocNo2 = ReturnSiteSurvayNo;
                    if (dtPurlinDistance != null)
                    {
                        
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "purlindistance", out ReturnCode1, out ReturnMsg1);
                        if (dtPurlinDistance.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtPurlinDistance.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtPurlinDistance");

                    //----------------------StructureStability-------------------------------------
                    DataTable dtStructureStability = new DataTable();
                    dtStructureStability = (DataTable)Session["dtStructureStability"];
                    // ----------------------------------------------
                    //String DocNo2 = ReturnSiteSurvayNo;
                    if (dtStructureStability != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "structurestability", out ReturnCode1, out ReturnMsg1);
                        if (dtStructureStability.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtStructureStability.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtStructureStability");

                    //---------------------Skylight--------------------------------------
                    DataTable dtSkyLight = new DataTable();
                    dtSkyLight = (DataTable)Session["dtSkyLight"];
                    // ----------------------------------------------
                    //String DocNo2 = ReturnSiteSurvayNo;
                    if (dtSkyLight != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "skylight", out ReturnCode1, out ReturnMsg1);
                        if (dtSkyLight.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSkyLight.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtSkyLight");

                    //---------------------SoilTest--------------------------------------
                    DataTable dtSoilTest = new DataTable();
                    dtSoilTest = (DataTable)Session["dtSoilTest"];
                    // ----------------------------------------------
                    //String DocNo2 = ReturnSiteSurvayNo;
                    if (dtSoilTest != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "soiltest", out ReturnCode1, out ReturnMsg1);
                        if (dtSoilTest.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtSoilTest.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtSoilTest");

                    //----------------------ContourSurvey-------------------------------------
                    DataTable dtContourSurvey = new DataTable();
                    dtContourSurvey = (DataTable)Session["dtContourSurvey"];
                    // ----------------------------------------------
                    //String DocNo2 = ReturnSiteSurvayNo;
                    if (dtContourSurvey != null)
                    {
                         
                        // -------------------------------------------------------------- Delete Record
                        BAL.SiteSurvayMgmt.DeleteSiteSurvayDocumentsByDocNo(ReturnSiteSurvayNo, "contoursurvey", out ReturnCode1, out ReturnMsg1);
                        if (dtContourSurvey.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtContourSurvey.Rows)
                        {
                            if (dr.RowState.ToString() != "Deleted")
                            {
                                filename1 = dr["FileName"].ToString();
                                type = dr["Filetype"].ToString();
                                // -------------------------------------------------------------- Insert/Update Record
                                BAL.SiteSurvayMgmt.AddUpdateSiteSurvayDocuments(DocNo, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                            }
                          }
                        }
                    }
                    Session.Remove("dtContourSurvey");
                    //-----------------------------------------------------------
                }
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strError))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-danger');", true);
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteSiteSurvay(string pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();
            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.SiteSurvayMgmt.DeleteSiteSurvay(Convert.ToInt64(pkID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);
            return serializer.Serialize(rows);
        }
        protected void rptSitePhoto_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtSitePhotos = (DataTable)Session["dtSitePhotos"];
                for (int i = dtSitePhotos.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtSitePhotos.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtSitePhotos.AcceptChanges();
                Session.Add("dtSitePhotos", dtSitePhotos);
                rptSitePhoto.DataSource = dtSitePhotos;
                rptSitePhoto.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }
        
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            DataTable dtContact;
            dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(Convert.ToInt64(hdnCustomerID.Value));
            if(dtContact.Rows.Count > 0)
            {
                txtContPerson1.Text = dtContact.Rows[0]["ContactPerson1"].ToString();
                txtContNo1.Text = dtContact.Rows[0]["ContactNumber1"].ToString();
                txtContEmail1.Text = dtContact.Rows[0]["ContactEmail1"].ToString();
                txtContDesignation1.Text = dtContact.Rows[0]["ContactDesigCode1"].ToString();

                txtContPerson2.Text = dtContact.Rows[1]["ContactPerson1"].ToString();
                txtContNo2.Text = dtContact.Rows[1]["ContactNumber1"].ToString();
                txtContEmail2.Text = dtContact.Rows[1]["ContactEmail1"].ToString();
                txtContDesignation2.Text = dtContact.Rows[1]["ContactDesigCode1"].ToString();
            }
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {

        }

        protected void rptSiteVideo_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtSiteVideo = (DataTable)Session["dtSiteVideo"];
                for (int i = dtSiteVideo.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtSiteVideo.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtSiteVideo.AcceptChanges();
                Session.Add("dtSiteVideo", dtSiteVideo);
                rptSiteVideo.DataSource = dtSiteVideo;
                rptSiteVideo.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptSiteSketchWithObject_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtSketchWithObject = (DataTable)Session["dtSketchWithObject"];
                for (int i = dtSketchWithObject.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtSketchWithObject.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtSketchWithObject.AcceptChanges();
                Session.Add("dtSketchWithObject", dtSketchWithObject);
                rptSiteSketchWithObject.DataSource = dtSketchWithObject;
                rptSiteSketchWithObject.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptTransNamePlate_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtTransNamePlate = (DataTable)Session["dtTransNamePlate"];
                for (int i = dtTransNamePlate.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtTransNamePlate.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtTransNamePlate.AcceptChanges();
                Session.Add("dtTransNamePlate", dtTransNamePlate);
                rptTransNamePlate.DataSource = dtTransNamePlate;
                rptTransNamePlate.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptRoofPlan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtRoofPlan = (DataTable)Session["dtRoofPlan"];
                for (int i = dtRoofPlan.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtRoofPlan.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtRoofPlan.AcceptChanges();
                Session.Add("dtRoofPlan", dtRoofPlan);
                rptRoofPlan.DataSource = dtRoofPlan;
                rptRoofPlan.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptElectricityBill_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtElectricityBill = (DataTable)Session["dtElectricityBill"];
                for (int i = dtElectricityBill.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtElectricityBill.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtElectricityBill.AcceptChanges();
                Session.Add("dtElectricityBill", dtElectricityBill);
                rptElectricityBill.DataSource = dtElectricityBill;
                rptElectricityBill.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptEarthResistivity_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtEarthResistivity = (DataTable)Session["dtEarthResistivity"];
                for (int i = dtEarthResistivity.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtEarthResistivity.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtEarthResistivity.AcceptChanges();
                Session.Add("dtEarthResistivity", dtEarthResistivity);
                rptEarthResistivity.DataSource = dtEarthResistivity;
                rptEarthResistivity.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptEarthPil_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtEarthPil = (DataTable)Session["dtEarthPil"];
                for (int i = dtEarthPil.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtEarthPil.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtEarthPil.AcceptChanges();
                Session.Add("dtEarthPil", dtEarthPil);
                rptEarthPil.DataSource = dtEarthPil;
                rptEarthPil.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptPurlinDistance_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtPurlinDistance = (DataTable)Session["dtPurlinDistance"];
                for (int i = dtPurlinDistance.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtPurlinDistance.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtPurlinDistance.AcceptChanges();
                Session.Add("dtPurlinDistance", dtPurlinDistance);
                rptPurlinDistance.DataSource = dtPurlinDistance;
                rptPurlinDistance.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptStructureStability_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtStructureStability = (DataTable)Session["dtStructureStability"];
                for (int i = dtStructureStability.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtStructureStability.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtStructureStability.AcceptChanges();
                Session.Add("dtStructureStability", dtStructureStability);
                rptStructureStability.DataSource = dtStructureStability;
                rptStructureStability.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptSkylights_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtSkyLight = (DataTable)Session["dtSkyLight"];
                for (int i = dtSkyLight.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtSkyLight.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtSkyLight.AcceptChanges();
                Session.Add("dtSkyLight", dtSkyLight);
                rptSkylights.DataSource = dtSkyLight;
                rptSkylights.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptSoilTest_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtSoilTest = (DataTable)Session["dtSoilTest"];
                for (int i = dtSoilTest.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtSoilTest.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtSoilTest.AcceptChanges();
                Session.Add("dtSoilTest", dtSoilTest);
                rptSoilTest.DataSource = dtSoilTest;
                rptSoilTest.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void rptContourSurvey_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtContourSurvey = (DataTable)Session["dtContourSurvey"];
                for (int i = dtContourSurvey.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtContourSurvey.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtContourSurvey.AcceptChanges();
                Session.Add("dtContourSurvey", dtContourSurvey);
                rptContourSurvey.DataSource = dtContourSurvey;
                rptContourSurvey.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }
    }
}