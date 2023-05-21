using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace StarsProject
{
    public partial class myModuleAttachment : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string ModuleName
        {
            get { return hdnModuleName.Value; }
            set { hdnModuleName.Value = value; }
        }

        public string KeyValue
        {
            get { return hdnKeyValue.Value; }
            set { hdnKeyValue.Value = value; }
        }

        protected void rptAttachment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DeleteModuleDocs(e.CommandArgument.ToString(), rptAttachment);
            }
        }

        public void BindModuleDocuments()
        {
            DataTable dtModuleDoc = new DataTable();
            dtModuleDoc = (DataTable)Session[ModuleName];
            if (dtModuleDoc == null || dtModuleDoc.Rows.Count < 0)
            {
                List<Entity.ModuleDocuments> lst = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", ModuleName, "", KeyValue, Session["LoginUserID"].ToString());
                dtModuleDoc = PageBase.ConvertListToDataTable(lst);
                Session.Add(ModuleName, dtModuleDoc);
            }
            // -----------------------------------------------------
            //DataTable selectedTable = dtModuleDoc.AsEnumerable().Where(r => r.Field<string>("ModuleName") == ModuleName).CopyToDataTable();
            rptAttachment.DataSource = dtModuleDoc;
            rptAttachment.DataBind();
            
        }

        public void ManageLibraryDocs()
        {
            if (uploadAttachment.PostedFile != null)
            {
                if (uploadAttachment.PostedFile.FileName.Length > 0)
                {
                    if (uploadAttachment.HasFile)
                    {
                        HttpFileCollection _HttpFileCollection = Request.Files;
                        for (int i = 0; i<_HttpFileCollection.Count; i++)
                        {
                            HttpPostedFile _HttpPostedFile = _HttpFileCollection[i];
                            if (_HttpPostedFile.ContentLength > 0)
                            {
                                string filePath = _HttpPostedFile.FileName;
                                string filename1 = Path.GetFileName(filePath);
                                string ext = Path.GetExtension(filename1);
                                string type = String.Empty;

                                if (ext == ".bmp" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                                {
                                    try
                                    {
                                        string rootFolderPath = Server.MapPath("ModuleDocs");
                                        string filesToDelete = filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                        // -----------------------------------------------------
                                        String flname = filename1;
                                        String tmpFile = Server.MapPath("ModuleDocs" + "/") + flname;
                                        uploadAttachment.PostedFile.SaveAs(tmpFile);
                                        // ---------------------------------------------------------------
                                        DataTable dtGall = new DataTable();
                                        dtGall = (DataTable)Session[ModuleName];
                                        if (dtGall == null || dtGall.Rows.Count < 0)
                                        {
                                            List<Entity.ModuleDocuments> lst = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", ModuleName, "", "-111", Session["LoginUserID"].ToString());
                                            dtGall = PageBase.ConvertListToDataTable(lst);
                                            Session.Add(ModuleName, dtGall);
                                        }
                                        Int64 cntRow = dtGall.Rows.Count + 1;
                                        DataRow dr = dtGall.NewRow();
                                        dr["pkID"] = cntRow;
                                        dr["ModuleName"] = ModuleName;
                                        dr["KeyValue"] = KeyValue;
                                        dr["DocName"] = flname;
                                        dr["DocType"] = "";
                                        // ---------------------------------------------------------------
                                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_HttpPostedFile.InputStream.ToString());
                                        string utfString = Convert.ToBase64String(plainTextBytes);

                                        System.IO.Stream fs = _HttpPostedFile.InputStream;
                                        System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                                        dr["DocData"] = base64String;
                                        
                                        // ---------------------------------------------------------------
                                        //dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                        //dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                        dtGall.Rows.Add(dr);

                                        dtGall.AcceptChanges();
                                        Session.Add(ModuleName, dtGall);
                                        // ---------------------------------------------------------------
                                        string delFolderPath = Server.MapPath("ModuleDocs");
                                        if (System.IO.File.Exists(Path.Combine(rootFolderPath, flname)))
                                        {
                                            System.IO.File.Delete(Path.Combine(rootFolderPath, flname));
                                        }
                                        // ---------------------------------------------------------------
                                        if (dtGall != null)
                                        {
                                            //DataTable selectedTable = dtGall.AsEnumerable().Where(r => r.Field<string>("ModuleName") == ModuleName).CopyToDataTable();
                                            rptAttachment.DataSource = dtGall;
                                            rptAttachment.DataBind();
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SaveModuleDocs()
        {
            int ReturnCode1 = 0;
            string ReturnMsg1 = "";

            DataTable dtModuleDoc = new DataTable();
            dtModuleDoc = (DataTable)Session[ModuleName];
            if (dtModuleDoc != null)
            {
                BAL.ModuleDocMgmt.DeleteDocument(0, ModuleName, KeyValue, "", out ReturnCode1, out ReturnMsg1);

                if (dtModuleDoc.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtModuleDoc.Rows)
                    {
                        if (dr.RowState.ToString() != "Deleted")
                        {
                            Entity.ModuleDocuments objEntityDocList = new Entity.ModuleDocuments();
                            objEntityDocList.pkID = 0;
                            objEntityDocList.ModuleName = dr["ModuleName"].ToString();
                            objEntityDocList.KeyValue = KeyValue;
                            objEntityDocList.DocType = "";
                            objEntityDocList.LoginUserID = Session["LoginUserID"].ToString();
                            // --------------------------------------------------------------
                            string tmpFileName = "";
                            if (dr["DocName"].ToString().Replace(" ", "").Contains(dr["ModuleName"].ToString() + "-" + KeyValue.ToString().Trim() + "-"))
                                tmpFileName = dr["DocName"].ToString().Replace(" ", "");
                            else
                                tmpFileName = dr["ModuleName"].ToString() + "-" + KeyValue.ToString().Trim() + "-" + dr["DocName"].ToString().Replace(" ", "");

                            objEntityDocList.DocName = tmpFileName;
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.ModuleDocMgmt.AddUpdateDocument(objEntityDocList, out ReturnCode1, out ReturnMsg1);
                            // -------------------------------------------------------------- 
                            String tmpFile = Server.MapPath("ModuleDocs/") + tmpFileName;
                            if (dr["DocData"] != null && !String.IsNullOrEmpty(dr["DocData"].ToString()))
                                System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(dr["DocData"].ToString()));
                        }
                    }
                }
            }
            Session.Remove(ModuleName);
        }

        public void DeleteModuleDocs(string DocName, Repeater rptControl)
        {
            DataTable dtModuleDoc = (DataTable)Session[ModuleName];
            for (int i = dtModuleDoc.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtModuleDoc.Rows[i];
                if (dr["ModuleName"].ToString() == ModuleName && dr["KeyValue"].ToString() == KeyValue && dr["DocName"].ToString() == DocName)
                {
                    string rootFolderPath = Server.MapPath("ModuleDocs");
                    if (System.IO.File.Exists(Path.Combine(rootFolderPath, dr["DocName"].ToString())))
                    {
                        System.IO.File.Delete(Path.Combine(rootFolderPath, dr["DocName"].ToString()));
                    }
                    // ---------------------------------------
                    dr.Delete();
                }
            }
            dtModuleDoc.AcceptChanges();
            Session.Add(ModuleName, dtModuleDoc);
            // ---------------------------------------
            //DataTable selectedTable = dtModuleDoc.AsEnumerable().Where(r => r.Field<string>("ModuleName") == ModuleName).CopyToDataTable();
            rptControl.DataSource = dtModuleDoc;
            rptControl.DataBind();
            // ---------------------------------------
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
        }
        
        public void DeleteModuleEntry(string pModuleName, string pKeyValue, string rootFolderPath)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            DataTable dtModuleDoc = new DataTable();
            List<Entity.ModuleDocuments> lst = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", pModuleName, "", pKeyValue, "admin");
            dtModuleDoc = PageBase.ConvertListToDataTable(lst);
            if (dtModuleDoc.Rows.Count > 0 && pModuleName != null)
            {
                for (int i = dtModuleDoc.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtModuleDoc.Rows[i];
                    if (dr["ModuleName"].ToString() == pModuleName && dr["KeyValue"].ToString() == pKeyValue)
                    {
                        string strDocName = dr["DocName"].ToString();
                        if (System.IO.File.Exists(Path.Combine(rootFolderPath, strDocName)))
                        {
                            System.IO.File.Delete(Path.Combine(rootFolderPath, strDocName));
                            BAL.ModuleDocMgmt.DeleteDocument(0, pModuleName, pKeyValue, strDocName, out ReturnCode, out ReturnMsg);
                        }
                        // ---------------------------------------
                        dr.Delete();
                    }
                }
                dtModuleDoc.AcceptChanges();
                if (dtModuleDoc != null)
                {
                    if (dtModuleDoc.Rows.Count>0)
                        Session.Add(pModuleName, dtModuleDoc);
                }
                
            }
        }

        public void ResetSession(string pModule)
        {
            Session.Remove(pModule);
        }
        protected void btnAttachment_Click(object sender, EventArgs e) { }

    }
}