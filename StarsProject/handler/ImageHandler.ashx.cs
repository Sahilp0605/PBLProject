using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace StarsProject.handler
{

    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // -------------------------------------------------------------
            byte[] img = null;

            if (context.Request.QueryString["imgFrom"].ToString().ToUpper() == "PRODUCT")
            {
                img = BAL.ManageImpExpMgmt.GetProductPhotoID(Convert.ToInt64(context.Request.QueryString["imgid"]));
            }

            if (context.Request.QueryString["imgFrom"].ToString().ToUpper() == "MEMBER")
            {
                img = BAL.ManageImpExpMgmt.GetMemberPhotoID(context.Request.QueryString["imgid"]);
            }

            if (context.Request.QueryString["imgFrom"].ToString().ToUpper() == "DRIVER")
            {
                img = BAL.ManageImpExpMgmt.GetDriverPhotoID(Convert.ToInt64(context.Request.QueryString["imgid"]));
            }
            // -------------------------------------------------------------
            if (img != null)
            {
                context.Response.ContentType = "image/jpg";
                context.Response.BinaryWrite(img);
            }
            else
            {
                context.Response.ContentType = "image/png";
                string imgAppPath;
                imgAppPath = "../images/no-figure.png";
                //if (context.Request.QueryString["imgtype"].ToString() == "F")
                //    imgAppPath = "../images/noimagefemale.jpg";
                //else 
                //    imgAppPath = "../images/noimage.png";

                FileStream fStream = new FileStream(context.Server.MapPath(imgAppPath), FileMode.Open, FileAccess.Read);
                System.Drawing.Image image = System.Drawing.Image.FromStream(fStream);
                img = ConvertImageToByteArray(image, System.Drawing.Imaging.ImageFormat.Png);
                context.Response.BinaryWrite(img);
            }
        }

        private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert,System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}