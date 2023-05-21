using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class resetpassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int TotalCount = 0;
            List<Entity.Users> lstUser = new List<Entity.Users>();
            lstUser = BAL.UserMgmt.GetLoginUserList(username.Text.Trim(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
            if (lstUser.Count>0)
            {


                if (oldpassword.Text.Trim() != lstUser[0].UserPassword.Trim())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error','Wrong Password !');", true);
                }
                else
                {
                    Entity.Authenticate objAuth = new Entity.Authenticate();
                    objAuth = BAL.UserMgmt.AuthenticateUser(username.Text.Trim(), oldpassword.Text.Trim());

                    if (newpassword1.Text.Trim() == newpassword2.Text.Trim())
                    {
                        Entity.Users objUser = new Entity.Users();
                        objUser.UserID = username.Text.Trim();
                        objUser.UserPassword = newpassword1.Text.Trim();
                        objUser.LoginUserID = username.Text.Trim();
                        int ReturnCode = 0, ReturnCode1 = 0;
                        string ReturnMsg = "", ReturnMsg1 = "";
                        // ----------------------------------------------- Calling Procedure to Insert/Update Record
                        BAL.UserMgmt.UpdateUserPassword(username.Text.Trim(), newpassword1.Text.Trim(), out ReturnCode, out ReturnMsg);
                        if (ReturnCode > 0)
                        {
                            BAL.UserMgmt.AddUpdateUserManagementRegistration(objUser, objAuth.SerialKey.ToString(), out ReturnCode1, out ReturnMsg1);
                        }
                        if (ReturnCode>0 && ReturnCode1>0)
                        {
                            string pEmailTo, pBody = "";
                            int ReturnCode3;
                            string ReturnMsg3 = "";
                            // ------------------------------------------------
                            pEmailTo = BAL.CommonMgmt.GetEmployeeEmailAddress(lstUser[0].UserID);
                            // ------------------------------------------------
                            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                            lstCompany = BAL.CommonMgmt.GetCompanyProfileList(1, "admin");
                            // ------------------------------------------------
                            pBody += "<span style='font-family:Calibri;font-size: 11pt;'>";
                            pBody += "Dear User,<br /><br />This is to inform you that your <b>User Credentials</b> for <b>CRM Application</b> has been changed.<br /><br /><br />";
                            pBody += "<b>Login Credentials :</b><br />";
                            pBody += "User ID : <b>" + lstUser[0].UserID + "</b><br />";
                            pBody += "Regards<br />";
                            pBody += "<b>" + lstCompany[0].CompanyName + "</b><br />";
                            pBody += "</span>";

                            Entity.EmailStructure lstMail = new Entity.EmailStructure();
                            lstMail.EmailTo = pEmailTo;
                            lstMail.Subject = "Login Credentials Changed";
                            lstMail.Body = pBody;
                            // ------------------------------------------------
                            BAL.MailNotificationMgmt.sendEmailNotification(lstMail, out ReturnCode3, out ReturnMsg3);
                            if (ReturnCode3>0)
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Information','" + ReturnMsg + " !');", true);
                            else
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error','" + ReturnMsg + " !');", true);
                        }
                        // ----------------------------------------------- 
                        
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error','Password & Confirm Password doesnot matched !');", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error','User ID/Password doesnot matched !');", true);
            }
        }
    }
}