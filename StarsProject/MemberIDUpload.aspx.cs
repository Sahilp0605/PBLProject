using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace StarsProject
{
    public partial class MemberIDUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["imgFrom"].ToString()))
            {
                hdnImageFrom.Value = Request.QueryString["imgFrom"].ToString();
                // ----------------------------------------------
                if (hdnImageFrom.Value.ToLower() == "member")
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["RegistrationNo"].ToString()))
                        hdnRegistrationNo.Value = Request.QueryString["RegistrationNo"].ToString();
                }
                if (hdnImageFrom.Value.ToLower() == "driver")
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["DriverID"].ToString()))
                        hdnDriverID.Value = Request.QueryString["DriverID"].ToString();
                }
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    if (FileUploadControl.PostedFile.ContentType == "image/jpeg")
                    {
                        if (FileUploadControl.PostedFile.ContentLength < 102400)
                        {
                            int ReturnCode = 0;
                            string ReturnMsg = "";
                            // ------------------------------------------------------------------------------------------------------
                            string ImageName = string.Empty;
                            byte[] imgPic = null;
                            if (FileUploadControl.PostedFile != null && FileUploadControl.PostedFile.FileName != "")
                            {
                                ImageName = Path.GetFileName(FileUploadControl.FileName);
                                imgPic = new byte[FileUploadControl.PostedFile.ContentLength];
                                HttpPostedFile UploadedImage = FileUploadControl.PostedFile;
                                UploadedImage.InputStream.Read(imgPic, 0, (int)FileUploadControl.PostedFile.ContentLength);
                                
                            }
                            // ------------------------------------------------------------------------------------------------------
                            if (hdnImageFrom.Value.ToLower() == "member")
                            {
                                BAL.ManageImpExpMgmt.AddMemberPhoto(hdnRegistrationNo.Value, imgPic, out ReturnCode, out ReturnMsg);
                                StatusLabel.Text = "Status: Member Photo ID uploaded successfully !";
                            }
                            if (hdnImageFrom.Value.ToLower() == "driver")
                            {
                                BAL.ManageImpExpMgmt.AddDriverPhoto(Convert.ToInt64(hdnDriverID.Value), imgPic, out ReturnCode, out ReturnMsg);
                                StatusLabel.Text = "Status: Driver Photo ID uploaded successfully !";
                            }
                        }
                        else
                            StatusLabel.Text = "Upload status: The file has to be less than 100 kb!";
                    }
                    else
                        StatusLabel.Text = "Upload status: Only JPEG files are accepted!";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }
    }
}