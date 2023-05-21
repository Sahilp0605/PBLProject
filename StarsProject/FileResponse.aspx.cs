using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class FileResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = Session["FileType"].ToString();
            Response.AddHeader("content-disposition", "attachment;filename=" + Session["FileName"].ToString());
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(Convert.FromBase64String(Session["FileData"].ToString()));
            Session["FileType"]=null;
            Session["FileName"]=null;
            Session["FileData"] = null;

        }
    }
}