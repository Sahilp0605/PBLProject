using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Net.Mime;
using System.Web.SessionState;
using System.Collections.Specialized;

namespace StarsProject
{
    public partial class forgotpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int totrec;
                int ReturnCode;
                string ReturnMsg, pEmailTo, pBody = "";
                // ------------------------------------------------
                List<Entity.Users> lstUser = new List<Entity.Users>();
                lstUser = BAL.UserMgmt.GetLoginUserList(username.Text.Trim(), 1, 1000, out totrec);
                pEmailTo = BAL.CommonMgmt.GetEmployeeEmailAddress(lstUser[0].UserID);
                // ------------------------------------------------
                List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                lstCompany = BAL.CommonMgmt.GetCompanyProfileList(1, "admin");
                // ------------------------------------------------
                pBody += "<span style='font-family:Calibri;font-size: 11pt;'>";
                pBody += "Dear User,<br /><br />As per your request, Please find your below mentioned Login Credentials for using CRM.<br /><br /><br />";
                pBody += "<b>Login Credentials :</b><br />";
                pBody += "User ID : <b>" + lstUser[0].UserID + "</b><br />";
                pBody += "Password: <b>" + lstUser[0].UserPassword + "</b><br /><br />";
                pBody += "Regards<br />";
                pBody += "<b>" + lstCompany[0].CompanyName + "</b><br />";
                pBody = "</span>";

                Entity.EmailStructure lstMail = new Entity.EmailStructure();
                lstMail.EmailTo = pEmailTo;
                lstMail.Subject = "Login Credentials";
                lstMail.Body = pBody;
                // -----------------------------------------------
                BAL.MailNotificationMgmt.sendEmailNotification(lstMail, out ReturnCode, out ReturnMsg);
                if (ReturnCode > 0)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Success','" + ReturnMsg + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error','" + ReturnMsg + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error', 'Email Notification Failed !');", true);
            }
        }

    }
}