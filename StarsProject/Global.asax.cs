using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
namespace StarsProject
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started  
            if (Session["LoginUserID"] != null)
            {
                //Redirect to Welcome Page if Session is not null  
                HttpContext.Current.Response.Redirect("Dashboard.aspx");

            }
            else
            {
                //Redirect to Login Page if Session is null & Expires   
                HttpContext.Current.Response.Redirect("Default.aspx");

            }  
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
         {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["logindetail"] == null)
            //{
            //    Page page = HttpContext.Current.CurrentHandler as Page;
            //    page.ClientScript.RegisterStartupScript(GetType(), "closePage", "<script type=\"text/JavaScript\">window.close();");
            //    HttpContext.Current.Response.Redirect("Default.aspx");
            //}
        }

        protected void Session_End(object sender, EventArgs e)
        {
            ////Redirect to Login Page if Session is null & Expires   
            //HttpContext.Current.Response.Redirect("Default.aspx");
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}