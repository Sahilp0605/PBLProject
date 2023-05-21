using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;

namespace StarsProject
{
    public partial class AudioLibrary : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 15;
                hdnSerialKey.Value = Session["SerialKey"].ToString();
            }
            else
            {
                //hdnCurrentTab.Value = Request.Form[hdnCurrentTab.UniqueID];
                // ----------------------------------------------------------------------
                // Product Audio Gallery Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (uploadAudioGallery.PostedFile != null)
                {
                    if (uploadAudioGallery.PostedFile.FileName.Length > 0)
                    {
                        if (uploadAudioGallery.HasFile)
                        {
                            HttpFileCollection _HttpFileCollection = Request.Files;
                            for (int i = 0; i < _HttpFileCollection.Count; i++)
                            {
                                HttpPostedFile _HttpPostedFile = _HttpFileCollection[i];
                                if (_HttpPostedFile.ContentLength > 0)
                                {
                                    string filePath = _HttpPostedFile.FileName;
                                    string filename1 = Path.GetFileName(filePath);
                                    string ext = Path.GetExtension(filename1);
                                    string type = String.Empty;

                                    if (ext == ".mp3" || ext == ".mp4" || ext == ".wav")
                                    {
                                        try
                                        {
                                            string rootFolderPath = Server.MapPath("AudioFiles");
                                            string filesToDelete = @"audGall-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                            foreach (string file in fileList)
                                            {
                                                System.IO.File.Delete(file);
                                            }
                                            // -----------------------------------------------------
                                            String flname = "audGall-" + filename1;
                                            String tmpFile = Server.MapPath("AudioFiles/") + flname;
                                            // ---------------------------------------------------------------
                                            DataTable dtGall = new DataTable();
                                            dtGall = (DataTable)Session["dtAudGallery"];
                                            Int64 cntRow = dtGall.Rows.Count + 1;
                                            DataRow dr = dtGall.NewRow();
                                            dr["pkID"] = cntRow;
                                            dr["ProductID"] = 0;
                                            dr["FileName"] = flname;
                                            dr["FileType"] = type;

                                            var plainTextBytes = Encoding.UTF8.GetBytes(_HttpPostedFile.InputStream.ToString());
                                            string utfString = Convert.ToBase64String(plainTextBytes);

                                            System.IO.Stream fs = _HttpPostedFile.InputStream;
                                            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                                            dr["ContentData"] = base64String;
                                            dtGall.Rows.Add(dr);
                                            Session.Add("dtAudGallery", dtGall);
                                            // ---------------------------------------------------------------
                                            rptAudioList.DataSource = dtGall;
                                            rptAudioList.DataBind();
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                        }
                                        catch (Exception ex) { }
                                    }
                                    else
                                        ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                                }

                            }

                        }
                    }
                }

            }
        }

        public void BindAudioGallery(Int64 HeaderID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.AudioFiles> lst = BAL.AudioMgmt.GetAudioFiles(0, "TeleCaller", "");
            dtDetail1 = PageBase.ConvertListToDataTable(lst);

            rptAudioList.DataSource = dtDetail1;
            rptAudioList.DataBind();

            Session.Add("dtAudGallery", dtDetail1);

        }
    }
}