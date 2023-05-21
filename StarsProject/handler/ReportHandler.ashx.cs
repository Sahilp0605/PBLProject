using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Entity;
using System.Web.SessionState;

namespace StarsProject.handler
{
    // ------------------------------------------------------------

    public class ReportHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string cs = ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
            List<ReportMenu> listReport = new List<ReportMenu>();
            listReport = BAL.CommonMgmt.GetReportsList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(listReport));
        }

        public List<ReportMenu> GetReportTree(List<ReportMenu> list, Int64? parent)
        {
            return list.Select(x => new ReportMenu
            {
                pkID = x.pkID,
                ReportText = x.ReportText,
                ReportName = x.ReportName,
                ParentID = x.ParentID,
                ReportLevel = x.ReportLevel,
                ReportURL = x.ReportURL,
                ReportImage = x.ReportImage,
                ReportImageHeight = x.ReportImageHeight,
                ReportImageWidth = x.ReportImageWidth,
                List = GetReportTree(list, x.pkID)
            }).ToList();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }

}