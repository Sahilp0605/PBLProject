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
    public partial class DocumentGallery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 15;
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetail", dtDetail);
                // ---------------------------------------------
                BindDocumentGallery();
            }
            else
            {
                // ----------------------------------------------------------------------
                // Product Document Upload On .... Page Postback
                // ----------------------------------------------------------------------
                String strErr = "";
                int ReturnCode1;
                String ReturnMsg1;
                if (uploadDocument.PostedFile != null)
                {
                    if (uploadDocument.PostedFile.FileName.Length > 0)
                    {

                        // ----------------------------------------------------------
                        if (uploadDocument.HasFile)
                        {
                            string filePath = uploadDocument.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1);
                            string type = String.Empty;

                            if (ext == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("DocumentGallery");
                                    string actionFile = filename1;   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, actionFile);

                                    List<Entity.Documents> lstFile = BAL.CommonMgmt.GetDocumentGalleryListByName(actionFile);
                                    if (lstFile.Count > 0)
                                    {
                                        System.IO.File.Delete(actionFile);
                                        BAL.CommonMgmt.DeleteDocumentGalleryByFileName(actionFile, out ReturnCode1, out ReturnMsg1);
                                    }
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.CommonMgmt.AddUpdateDocumentGallery(actionFile, "pdf", Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                                    if (ReturnCode1 > 0)
                                    {
                                        String tmpFile = Server.MapPath("DocumentGallery/") + actionFile;
                                        uploadDocument.PostedFile.SaveAs(tmpFile);
                                        strErr += "<li>" + ReturnMsg1 + "</li>";
                                    }
                                    // ---------------------------------------------------------------
                                    BindDocumentGallery();

                                }
                                catch (Exception ex) { }
                            }
                            // ---------------------------------------------------
                            if (!String.IsNullOrEmpty(strErr))
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                        }
                    }
                }
            }

        }
        public void BindDocumentGallery()
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.Documents> lst = BAL.CommonMgmt.GetDocumentGalleryList(0);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);

            rptDocGallery.DataSource = dtDetail1;
            rptDocGallery.DataBind();
        }
        protected void rptDocGallery_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int ReturnCode1;
            String ReturnMsg1;
            String strErr = "";
            // https://www.aspsnippets.com/Articles/Upload-and-Download-files-from-Folder-Directory-in-ASPNet-using-C-and-VBNet.aspx
            if (e.CommandName.ToString() == "Delete")
            {
                List<Entity.Documents> lst = BAL.CommonMgmt.GetDocumentGalleryList(Convert.ToInt64(e.CommandArgument.ToString()));
                BAL.CommonMgmt.DeleteDocumentGallery(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode1, out ReturnMsg1);
                if (ReturnCode1>0)
                {
                    if (lst.Count>0)
                    {
                        String tmpFile = Server.MapPath("DocumentGallery/") + lst[0].FileName;
                        if (!String.IsNullOrEmpty(tmpFile))
                            System.IO.File.Delete(tmpFile);
                    }
                    
                }
                // ----------------------------------------------
                BindDocumentGallery();

            }
        }
        protected void btnUpload1_Click(object sender, EventArgs e) { }
    }
}