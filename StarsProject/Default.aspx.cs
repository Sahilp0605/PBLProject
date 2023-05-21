using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

namespace StarsProject
{
    public partial class Default : PageBase
    {
        public string tmpDMS = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // -----------------------------------------------------------------------
                // Adding Dealer Management System Flag into 'objAuth' object
                // -----------------------------------------------------------------------
                tmpDMS = BAL.CommonMgmt.GetConstant("DMSSystem", 0, 1);
                divLoginAs.Visible = (tmpDMS.ToLower() == "yes") ? true : false;

                // --------------------------------------------------------------
                // Section : Re-Directing Page for External Web Complaint Module
                //         : Specially Designed For ACCUPANEL  
                // URL     : http://localhost:1111/Default.aspx?mode=webcom
                // --------------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                {
                    string tmpVal = Request.QueryString["mode"].ToString();
                    if (tmpVal == "webcom")
                    {
                        Response.Redirect("webComplaintNew.aspx");
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                tmpDMS = BAL.CommonMgmt.GetConstant("DMSSystem", 0, 1);
                if (tmpDMS.ToLower() == "yes")
                {
                    if (drpLoginAs.SelectedValue.ToLower() == "dealer")
                        objAuth = BAL.UserMgmt.AuthenticateUserDealer(username.Text.Trim(), password.Text.Trim());
                    else 
                        objAuth = BAL.UserMgmt.AuthenticateUser(username.Text.Trim(), password.Text.Trim());

                }
                else
                {
                    objAuth = BAL.UserMgmt.AuthenticateUser(username.Text.Trim(), password.Text.Trim());
                }
                objAuth.DMSSystem = (!String.IsNullOrEmpty(tmpDMS)) ? tmpDMS : "no";
                // ----------------------------------------------
                if (objAuth.RoleCode != null)
                {
                    if(objAuth.ActiveFlag == false)
                    {
                        //Response.Write("<script>alert('User Is InActive);</script>");
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Alert", "<script type='text/javascript'>alert('User Is InActive');</script>");
                        return;
                    }
                    // -------------------------------------------------------------
                    objAuth.LoginAs = (objAuth.DMSSystem.ToLower() == "yes") ? drpLoginAs.SelectedValue : "Employee";
                    // -------------------------------------------------------------
                    PageBase.CreateTicket(1, objAuth.UserID);
                    Session["logindetail"] = objAuth;
                    Session["LoginUserID"] = objAuth.UserID;
                    Session["CompanyID"] = objAuth.CompanyID;
                    Session["CompanyName"] = objAuth.CompanyName;
                    Session["CompanyType"] = objAuth.CompanyType;
                    Session["SerialKey"] = objAuth.SerialKey;
                    Session["RoleCode"] = objAuth.RoleCode;
                    if (objAuth.StateCode > 0)
                        Session["CompanyStateCode"] = objAuth.StateCode;
                    else
                        Session["CompanyStateCode"] = 0;
                    // --------------------------------------------------------
                    // Below Section : Fetching Login Employee's Email Address
                    // --------------------------------------------------------
                    if (String.IsNullOrEmpty(objAuth.EmailAddress))
                    {
                        if (objAuth.LoginAs.ToLower() == "employee")
                        {
                            string tmpVal = BAL.CommonMgmt.GetEmployeeEmailAddress(objAuth.UserID);
                            if (!String.IsNullOrEmpty(tmpVal))
                            {
                                objAuth.EmailAddress = tmpVal;
                                Session["EmailAddress"] = tmpVal;
                            }
                        }
                        if (objAuth.LoginAs.ToLower() == "dealer")
                        {
                            string tmpVal = BAL.CommonMgmt.GetCustomerEmailAddress(objAuth.UserID);
                            if (!String.IsNullOrEmpty(tmpVal))
                            {
                                objAuth.EmailAddress = tmpVal;
                                Session["EmailAddress"] = tmpVal;
                            }
                        }
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
                    // ReDirect to SendWaMsg.aspx for testing purpose
                    // ----------------------------------------------------
                    //Response.Redirect("WebComplaintNew.aspx");
                    if (objAuth.LoginAs.ToLower() == "employee")
                        Response.Redirect("DashboardDaily.aspx");
                    else
                        Response.Redirect("DashboardDealer.aspx");
                }
                else
                {
                    String strErr = "<li>User Authentification Failed ! </li>";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
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