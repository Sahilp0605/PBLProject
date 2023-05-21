using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace StarsProject.handler
{
    /// <summary>
    /// Summary description for Accessfield
    /// </summary>
    public class Accessfield : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Session["Access"] = context.Request.Form["Access"];
            context.Session["NoAccess"] = context.Request.Form["NoAccess"].ToString();
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