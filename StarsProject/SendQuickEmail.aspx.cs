using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SendQuickEmail : System.Web.UI.Page
    {
        int totrec = 0;
        int ReturnCode = 0;
        string ReturnMsg = "", strErr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                txtFrom.Text = objAuth.EmailAddress;
                txtBCC.Text = objAuth.EmailAddress;
                hdnPass.Value = objAuth.EmailPassword;
                // -------------------------------------------------------------------------------------
                BindDropDown();
                // -------------------------------------------------------------------------------------
                BindSenderInformation();
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:setEditor();", true);
        }
        public void BindDropDown()
        {
            List<Entity.EmailTemplate> lstTemplate = new List<Entity.EmailTemplate>();
            lstTemplate = BAL.EmailTemplateMgmt.GetEmailTemplate("", "", 1, 9999, out totrec);
            drpTemplate.DataSource = lstTemplate;
            drpTemplate.DataValueField = "TemplateID";
            drpTemplate.DataTextField = "Description";
            drpTemplate.DataBind();
            drpTemplate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }
        public void BindSenderInformation()
        {
            if (drpType.SelectedValue.ToLower() == "employee")
            {
                rptCustomer.Visible = false;
                rptEmployee.Visible = true;

                List<Entity.OrganizationEmployee> lstResult = new List<Entity.OrganizationEmployee>();
                lstResult = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
                lstResult = lstResult.Where(x => x.EmailAddress != "").ToList();
                rptEmployee.DataSource = lstResult;
                rptEmployee.DataBind();

            }
            else if (drpType.SelectedValue.ToLower() == "customer")
            {
                rptEmployee.Visible = false;
                rptCustomer.Visible = true;

                List<Entity.Customer> lstResult = new List<Entity.Customer>();
                lstResult = BAL.CustomerMgmt.GetCustomerList();
                lstResult = lstResult.Where(x => x.EmailAddress != "").ToList();
                rptCustomer.DataSource = lstResult;
                rptCustomer.DataBind();
            }

        }
        // -----------------------------------------------------------------------------
        public void LoadTemplate()
        {
            int totrec = 0;
            if (!String.IsNullOrEmpty(drpTemplate.SelectedValue))
            {
                List<Entity.EmailTemplate> lstTemplate = new List<Entity.EmailTemplate>();
                lstTemplate = BAL.EmailTemplateMgmt.GetEmailTemplate(drpTemplate.SelectedValue, "", 1, 9999, out totrec);
                if (lstTemplate.Count > 0)
                {
                    hdnTemplateID.Value = drpTemplate.SelectedValue;
                    txtSubject.Text = lstTemplate[0].Subject;
                    if (drpCommType.SelectedValue.ToLower() == "sms")
                    {
                        txtEditor.Text = HttpUtility.HtmlDecode(lstTemplate[0].ContentDataSMS);
                    }
                    else
                    {
                        txtEditor.Text = HttpUtility.HtmlDecode(lstTemplate[0].ContentData);
                    }
                }
            }
            else
            {
                hdnTemplateID.Value = "";
                txtSubject.Text = "";
                txtEditor.Text = "";
            }
            // -----------------------------------------------------------
            divFrom.Visible = (drpCommType.SelectedValue.ToLower() == "email") ? true : false;
            divCC.Visible = (drpCommType.SelectedValue.ToLower() == "email") ? true : false;
            divBCC.Visible = (drpCommType.SelectedValue.ToLower() == "email") ? true : false;
            divSubject.Visible = (drpCommType.SelectedValue.ToLower() == "email") ? true : false;
        }
        protected void drpTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTemplate();
        }

        protected void drpCommType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTemplate();
        }

        protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSenderInformation();
        }

        //protected void rptEmail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        HtmlTableCell tdAccountName = ((HtmlTableCell)e.Item.FindControl("tdAccountName"));
        //        HtmlTableCell tdEmailAddress = ((HtmlTableCell)e.Item.FindControl("tdEmailAddress"));
        //        if (drpType.SelectedValue.ToLower() == "employee")
        //        {
        //            tdAccountName.InnerText = 
        //        }
        //    }
        //}

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            Repeater rptEmail = new Repeater();
            if (drpType.SelectedValue.ToLower() == "employee")
            {
                rptEmail = rptEmployee;
            }
            if (drpType.SelectedValue.ToLower() == "customer")
            {
                rptEmail = rptCustomer;
            }
            // -------------------------------------------------------------------
            foreach (RepeaterItem item in rptEmail.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    if (chkCtrl.Checked)
                    {
                        string body = string.Empty, strErr = "";

                        string currEmpID = "", currUserID = "";
                        //currEmpID = (!String.IsNullOrEmpty(hdnEmpID.Value)) ? hdnEmpID.Value : "0";
                        //currUserID = BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64(currEmpID));

                        Entity.Authenticate objAuth = new Entity.Authenticate();
                        objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                        List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                        lstCompany = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

                        List<Entity.OrganizationEmployee> lstSuper = new List<Entity.OrganizationEmployee>();
                        lstSuper = BAL.OrganizationEmployeeMgmt.GetEmployeeSupervisorList(currUserID);
                        // -----------------------------------------------------------------------------
                        HtmlTableCell xName = (HtmlTableCell)item.FindControl("tdName");
                        HtmlTableCell xEmail = (HtmlTableCell)item.FindControl("tdEmail");

                        if (drpType.SelectedValue.ToLower() == "employee")
                        {
                            body = body.Replace("{Employee}", xName.InnerText).Replace("{EmployeeName}", xName.InnerText);
                            body = body.Replace("{CompanyName}", objAuth.CompanyName).Replace("{Organization}", objAuth.CompanyName);
                        }

                        if (drpType.SelectedValue.ToLower() == "customer")
                        {
                            body = body.Replace("{Customer}", xName.InnerText).Replace("{CustomerName}", xName.InnerText);
                            body = body.Replace("{CompanyName}", objAuth.CompanyName).Replace("{Organization}", objAuth.CompanyName);
                        }
                        // -----------------------------------------------------------------------------
                        try
                        {
                            using (MailMessage mailMessage = new MailMessage())
                            {
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = lstCompany[0].Host; //  ConfigurationManager.AppSettings["Host"];
                                if (!String.IsNullOrEmpty(lstCompany[0].EnableSSL.ToString().ToLower()))
                                    smtp.EnableSsl = lstCompany[0].EnableSSL;
                                smtp.UseDefaultCredentials = false;
                                smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                smtp.Credentials = NetworkCred;
                                NetworkCred.UserName = txtFrom.Text;
                                NetworkCred.Password = hdnPass.Value;

                                mailMessage.Subject = txtSubject.Text;
                                mailMessage.From = new MailAddress(txtFrom.Text);
                                mailMessage.To.Add(new MailAddress(txtFrom.Text));
                                for (int i = 0; i <= lstSuper.Count - 1; i++)
                                {
                                    if (txtFrom.Text.ToLower() != lstSuper[i].EmailAddress.ToLower())
                                        mailMessage.CC.Add(new MailAddress(lstSuper[i].EmailAddress));
                                }
                                mailMessage.Bcc.Add(new MailAddress(txtFrom.Text));
                                mailMessage.Body = body;
                                mailMessage.IsBodyHtml = true;
                                smtp.Send(mailMessage);
                            }
                            strErr = "Success";
                        }
                        catch (Exception ex)
                        {
                            string tmpMessage = "";
                            tmpMessage = ex.Message.ToString();
                            strErr = tmpMessage;
                        }
                    }

                }
            }
            // --------------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);

        }
    }
}