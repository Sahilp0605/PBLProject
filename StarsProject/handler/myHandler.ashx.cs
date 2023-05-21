using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarsProject.handler
{
    /// <summary>
    /// Summary description for myHandler
    /// </summary>
    public class myHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string DBName = "";
            if (!String.IsNullOrEmpty(context.Request.QueryString["DBName"]))
            {
                DBName = context.Request.QueryString["DBName"].ToString();
            }
            
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            context.Response.Clear();
            context.Response.ContentType = "application/octect-stream";
            context.Response.AppendHeader("content-disposition", "filename=" + DBName);
            context.Response.TransmitFile(DBName);
            context.Response.End();
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
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