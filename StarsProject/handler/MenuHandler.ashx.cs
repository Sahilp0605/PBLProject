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
    public class MenuHandler : IHttpHandler, IRequiresSessionState 
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
                List<ApplicationMenu> listMenu = new List<ApplicationMenu>();
                SqlConnection con = new SqlConnection(cs);
                SqlCommand cmd = new SqlCommand("ApplicationMenuList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@LoginUserID", context.Session["LoginUserID"].ToString());
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ApplicationMenu menu = new ApplicationMenu();
                    menu.pkID = Convert.ToInt64(rdr["pkID"]);
                    menu.MenuName = rdr["MenuName"].ToString();
                    menu.MenuText = rdr["MenuText"].ToString();
                    menu.ParentId = (rdr["parentID"] != DBNull.Value) ? Convert.ToInt64(rdr["parentID"]) : (Int64?)null;
                    menu.Active = Convert.ToBoolean(rdr["ActiveStatus"]);
                    menu.MenuLevel = Convert.ToInt64(rdr["MenuLevel"]);
                    menu.MenuURL = rdr["MenuURL"].ToString();
                    menu.MenuImage = rdr["MenuImage"].ToString();
                    menu.MenuImageHeight = Convert.ToInt64(rdr["MenuImageHeight"].ToString());
                    menu.MenuImageWidth = Convert.ToInt64(rdr["MenuImageWidth"].ToString());
                    listMenu.Add(menu);
                }
                con.Close();
                con.Dispose();

                List<ApplicationMenu> menuTree = GetMenuTree(listMenu, null);

                JavaScriptSerializer js = new JavaScriptSerializer();
                context.Response.Write(js.Serialize(menuTree));
            }
            catch (Exception ex)
            {
                context.Response.Redirect("~/default.aspx");
            }
        }

        public List<ApplicationMenu> GetMenuTree(List<ApplicationMenu> list, Int64? parent)
        {
            return list.Where(x => x.ParentId == parent).Select(x => new ApplicationMenu
            {
                pkID = x.pkID,
                MenuText = x.MenuText,
                MenuName = x.MenuName,
                ParentId = x.ParentId,
                Active = x.Active,
                MenuLevel = x.MenuLevel,
                MenuURL = x.MenuURL,
                MenuImage = x.MenuImage,
                MenuImageHeight = x.MenuImageHeight,
                MenuImageWidth = x.MenuImageWidth,
                List = GetMenuTree(list, x.pkID)
            }).ToList();
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}