using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class UserLockScreen : System.Web.UI.Page
    {
        public string loginUserID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            loginUserID = Session["LoginUserID"].ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = BAL.UserMgmt.AuthenticateUser(loginUserID, password.Text.Trim());
                if (objAuth.RoleCode != null)
                {
                    if (objAuth.ActiveFlag == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showMyMessage('Error','Wrong Credetials !');", true);
                        return;
                    }

                    PageBase.CreateTicket(1, objAuth.UserID);
                    Session["logindetail"] = objAuth;
                    Session["LoginUserID"] = objAuth.UserID;
                    Session["CompanyID"] = objAuth.CompanyID;
                    Session["CompanyName"] = objAuth.CompanyName;
                    Session["CompanyType"] = objAuth.CompanyType;
                    Session["SerialKey"] = objAuth.SerialKey;
                    if (objAuth.StateCode > 0)
                        Session["CompanyStateCode"] = objAuth.StateCode;
                    else
                        Session["CompanyStateCode"] = 0;
                    // ----------------------------------------------------
                    // Below Section : Fetching Login Employee's Email Address
                    // ----------------------------------------------------
                    string tmpVal = BAL.CommonMgmt.GetEmployeeEmailAddress(objAuth.UserID);
                    if (!String.IsNullOrEmpty(tmpVal))
                    {
                        objAuth.EmailAddress = tmpVal;
                        Session["EmailAddress"] = tmpVal;
                    }
                    // ----------------------------------------------------
                    int ReturnCode;
                    string ReturnMsg;
                    string IP = Request.UserHostName;
                    //string compName = DetermineCompName(IP);
                    string compName = "";

                    Entity.UserLog objUserLog = new Entity.UserLog();
                    objUserLog.pkID = 0;
                    objUserLog.UserID = objAuth.UserID;
                    objUserLog.INOUT = "IN";
                    objUserLog.MacID = compName;
                    BAL.CommonMgmt.AddUpdateUserLog(objUserLog, out ReturnCode, out ReturnMsg);
                    // ----------------------------------------------------
                    Response.Redirect("DashboardDaily.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        public static string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }
    }
}