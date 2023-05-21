using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarsProject.handler
{
    /// <summary>
    /// Summary description for ValidateTag
    /// </summary>
    public class ValidateTag : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(ValidateTagID(context.Request.Form["TagID"], context.Request.Form["flag"], context.Request.Form["app"]));
        }

        public string ValidateTagID(string TagID, string flag, string appno)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            //BAL.ApplicantMgmt.ValidateRFIDTag(appno, TagID, flag, out ReturnCode, out ReturnMsg);
            if (ReturnCode <= 0)
                return ReturnMsg;
            else
                return "valid";
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