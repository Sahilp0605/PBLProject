using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class myAudioControl : System.Web.UI.UserControl
    {
        int totrec = 0, ReturnCode = 0;
        String ReturnMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnSerialKey.Value = Session["SerialKey"].ToString();
            }
        }
        public string pageModule
        {
            get { return hdnModule.Value; }
            set { hdnModule.Value = value; }
        }
        public string pageModuleType
        {
            get { return hdnModuleType.Value; }
            set { hdnModuleType.Value = value; }
        }
        public string pageKeyID
        {
            get { return hdnKeyID.Value; }
            set { hdnKeyID.Value = value; }
        }
        public string pageFilePrefix
        {
            get { return hdnFilePrefix.Value; }
            set { hdnFilePrefix.Value = value; }
        }
        public string pageServerPath
        {
            get { return hdnServerPath.Value; }
            set { hdnServerPath.Value = value; }
        }
        public void destroyAudioGallery()
        {
            Session.Remove("dtAudGallery");
        }
        public DataTable getAudioDataTable()
        {
            DataTable dtGall = new DataTable();
            dtGall = (DataTable)Session["dtAudGallery"];
            return dtGall;
        }
        public void setAudioDataTable(FileUpload _uploadAudioGallery)
        {
            if (_uploadAudioGallery.PostedFile != null)
            {
                if (_uploadAudioGallery.PostedFile.FileName.Length > 0)
                {
                    if (_uploadAudioGallery.HasFile)
                    {
                        HttpFileCollection _HttpFileCollection = Request.Files;
                        for (int i = 0; i < _HttpFileCollection.Count; i++)
                        {
                            HttpPostedFile _HttpPostedFile = _HttpFileCollection[i];
                            if (_HttpPostedFile.ContentLength > 0)
                            {
                                string ext = Path.GetExtension(Path.GetFileName(_HttpPostedFile.FileName));
                                if (ext == ".mp3" || ext == ".mp4" || ext == ".wav" || ext == ".aac")
                                {
                                    string filePath = _HttpPostedFile.FileName;
                                    string filename1 = Path.GetFileName(filePath);
                                    //string ext = Path.GetExtension(filename1);
                                    string type = Path.GetExtension(filename1);

                                    DataTable dtGall = new DataTable();
                                    dtGall = (DataTable)Session["dtAudGallery"];
                                    if (dtGall == null)
                                    {
                                        List<Entity.AudioFiles> lstAudio = new List<Entity.AudioFiles>();
                                        lstAudio = BAL.AudioMgmt.GetAudioFiles(0, "FollowUp", "-1");
                                        dtGall = PageBase.ConvertListToDataTable(lstAudio);
                                    }
                                    DataRow dr = dtGall.NewRow();
                                    dr["pkID"] = 0;
                                    dr["KeyID"] = hdnKeyID.Value;
                                    dr["ModuleName"] = hdnModule.Value;
                                    dr["FileName"] = filename1;
                                    dr["FileType"] = hdnModuleType.Value;

                                    var plainTextBytes = Encoding.UTF8.GetBytes(_HttpPostedFile.InputStream.ToString());
                                    string utfString = Convert.ToBase64String(plainTextBytes);

                                    System.IO.Stream fs = _HttpPostedFile.InputStream;
                                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                                    dr["ContentData"] = base64String;
                                    dtGall.Rows.Add(dr);
                                    Session.Add("dtAudGallery", dtGall);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void rptAudioList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";

            if (e.CommandName.ToString() == "Delete")
            {
                List<Entity.AudioFiles> lstAudio = new List<Entity.AudioFiles>();
                lstAudio = BAL.AudioMgmt.GetAudioFiles(Convert.ToInt64(e.CommandArgument.ToString()), hdnModule.Value, hdnKeyID.Value);

                string rootFolderPath = System.Web.HttpContext.Current.Server.MapPath(hdnServerPath.Value);
                string filesToDelete = lstAudio[0].FileName.ToString();
                //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                //foreach (string file in fileList)
                //{
                //    System.IO.File.Delete(file);
                //}
                if (File.Exists(Path.Combine(rootFolderPath, filesToDelete)))
                {
                    File.Delete(Path.Combine(rootFolderPath, filesToDelete));
                }
                // -------------------------------------------
                BAL.AudioMgmt.DeleteAudio(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
                // -------------------------------------------------------------------------
                BindAudioList();
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);

        }

        public void BindAudioList()
        {
            DataTable dtGall = new DataTable();
            List<Entity.AudioFiles> lstAudio = new List<Entity.AudioFiles>();
            lstAudio = BAL.AudioMgmt.GetAudioFiles(0, hdnModule.Value, hdnKeyID.Value);
            dtGall = PageBase.ConvertListToDataTable(lstAudio);
            rptAudioList.DataSource = lstAudio;
            rptAudioList.DataBind();
            Session.Add("dtAudGallery", dtGall);
        }
        public void BindAudioList(Int64 pkID, String pModule, String pKeyID)
        {
            DataTable dtGall = new DataTable();
            List<Entity.AudioFiles> lstAudio = new List<Entity.AudioFiles>();
            lstAudio = BAL.AudioMgmt.GetAudioFiles(pkID, pModule, pKeyID);
            dtGall = PageBase.ConvertListToDataTable(lstAudio);
            rptAudioList.DataSource = lstAudio;
            rptAudioList.DataBind();
            Session.Add("dtAudGallery", dtGall);
        }
        public void SaveAudioFile(String pKeyID)
        {
            hdnKeyID.Value = pKeyID;
            // --------------------------------------------
            BAL.AudioMgmt.DeleteAudioByKeyID(hdnModule.Value, hdnKeyID.Value.ToString(), out ReturnCode, out ReturnMsg);
            // --------------------------------------------
            DataTable dtImgGall = new DataTable();
            dtImgGall = (DataTable)Session["dtAudGallery"];

            if (dtImgGall != null)
            {
                foreach (DataRow dr in dtImgGall.Rows)
                {
                    if (dr.RowState.ToString() != "Deleted")
                    {
                        Entity.AudioFiles lstObj = new Entity.AudioFiles();
                        lstObj.pkID = 0;
                        lstObj.ModuleName = hdnModule.Value;
                        lstObj.KeyID = hdnKeyID.Value;
                        String tmpNewFileName = "";
                        if (dr["FileName"].ToString().Contains(hdnFilePrefix.Value + hdnKeyID.Value))
                            tmpNewFileName = dr["FileName"].ToString();
                        else
                            tmpNewFileName = hdnFilePrefix.Value + hdnKeyID.Value + "-" + dr["FileName"].ToString();

                        lstObj.FileName = tmpNewFileName;
                        lstObj.FileType = dr["FileType"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.AudioMgmt.AddUpdateAudio(lstObj, out ReturnCode, out ReturnMsg);
                        String tmpFile = Server.MapPath(hdnServerPath.Value) + tmpNewFileName;
                        System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(dr["ContentData"].ToString()));
                    }
                }
            }
        }
    }
}