using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net.Mime;

namespace StarsProject
{
    public partial class BulkEmailSMS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1; 
                Session["PageSize"] = 5;
                // ----------------------------------------
                BindTemplateDropdown();
                // ----------------------------------------
                BindSenderDetail();
                BindDropdown();
            }
        }

        public void BindTemplateDropdown()
        {
            drpTemplate.Items.Clear();
            List<Entity.CampaignTemplate> lstEmailTemplate = new List<Entity.CampaignTemplate>();
            lstEmailTemplate = BAL.EmailTemplateMgmt.GetCampaignList(0, drpCampaign.SelectedValue, Session["LoginUserID"].ToString());
            drpTemplate.DataSource = lstEmailTemplate;
            drpTemplate.DataValueField = "CampaignID";
            drpTemplate.DataTextField = "CampaignSubject";
            drpTemplate.DataBind();
        }

        public void BindDropdown()
        {
            drpCustomerType.Items.Clear();
            if (drpCampaignFor.SelectedValue.ToLower() == "customer")
            {
                // ---------------- Customer Category List -------------------------------------
                List<Entity.CustomerCategory> lstCustCat = new List<Entity.CustomerCategory>();
                lstCustCat = BAL.CustomerCategoryMgmt.GetCustomerCategoryList();
                drpCustomerType.DataSource = lstCustCat;
                drpCustomerType.DataValueField = "CategoryName";
                drpCustomerType.DataTextField = "CategoryName";
                drpCustomerType.DataBind();
                drpCustomerType.Items.Insert(0, new ListItem("All Category", ""));
            }
            else if(drpCampaignFor.SelectedValue.ToLower() == "employee")
            {
                //// ---------------- Designation List  -------------------------------------
                List<Entity.Designations> lstDesig = new List<Entity.Designations>();
                lstDesig = BAL.DesignationMgmt.GetDesignationList();
                drpCustomerType.DataSource = lstDesig;
                drpCustomerType.DataValueField = "DesigCode";
                drpCustomerType.DataTextField = "Designation";
                drpCustomerType.DataBind();
                drpCustomerType.Items.Insert(0, new ListItem("All Designation", ""));
            }

        }
        protected void drpCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTemplateDropdown();
            //grid_ItemDataBound(sender, null);
            BindSenderDetail();
        }

        protected void drpCampaignFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropdown();
            BindSenderDetail();
        }

        protected void drpCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSenderDetail();
        }

        protected void drpTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        protected void btnSendCampaign_Click(object sender, EventArgs e)
        {
            GenerateCampaign();
            btnSendCampaign.Enabled = false;
        }
        // --------------------------------------------------------------------
        private void BindSenderDetail()
        {
            Int64 totCount = 0;

            grdCustomer.Visible = (drpCampaignFor.SelectedValue.ToLower() == "customer") ? true : false;
            grdEmployee.Visible = (drpCampaignFor.SelectedValue.ToLower() == "employee") ? true : false;
            grdExternal.Visible = (drpCampaignFor.SelectedValue.ToLower() == "external leads") ? true : false;
            // ------------------------------------------------------------------------
            if (drpCampaignFor.SelectedValue.ToLower() == "customer")
            {
                grdCustomer.DataSource = BAL.EmailTemplateMgmt.GetCampaignCustomerList(drpCampaign.SelectedValue, drpCustomerType.SelectedValue);
                grdCustomer.DataBind();
                totCount = grdCustomer.Items.Count;
            }
            else if (drpCampaignFor.SelectedValue.ToLower() == "employee")
            {
                grdEmployee.DataSource = BAL.EmailTemplateMgmt.GetCampaignEmployeeList(drpCampaign.SelectedValue, drpCustomerType.SelectedValue);
                grdEmployee.DataBind();
                totCount = grdEmployee.Items.Count;
            }
            else if (drpCampaignFor.SelectedValue.ToLower() == "external leads")
            {
                int TRecord;
                grdExternal.DataSource = BAL.ExternalLeadsMgmt.GetExternalLeadList(0, "", "", 1, 100000, out TRecord);
                grdExternal.DataBind();
                totCount = grdExternal.Items.Count;
            }
            spnTotal.InnerText = totCount.ToString();
            spnSelected.InnerHtml = "0";
        }

        private void GenerateCampaign()
        {
            // www.aspsnippets.com/Articles/Send-Bulk-Mass-Email-in-ASPNet-using-C-and-VBNet.aspx
            // Create a temporary DataTable
            DataTable dtCampaign = new DataTable();
            dtCampaign.Columns.AddRange(new DataColumn[4] { new DataColumn("SenderName", typeof(string)), new DataColumn("SenderMail", typeof(string)), new DataColumn("PrimaryMobileNo", typeof(string)), new DataColumn("SenderID", typeof(string)) });
            // -------------------------------------------------------------------
            // Copy the Checked Rows to DataTable
            // -------------------------------------------------------------------
            if (drpCampaignFor.SelectedValue.ToLower() == "customer")
            {
                foreach (RepeaterItem item in grdCustomer.Items)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    HiddenField hdnCtrl1 = (HiddenField)item.FindControl("hdnCustomerName");
                    HiddenField hdnCtrl2 = (HiddenField)item.FindControl("hdnCustomerMail");
                    HiddenField hdnCtrl3 = (HiddenField)item.FindControl("hdnCustomerContact");
                    HiddenField hdnCtrl4 = (HiddenField)item.FindControl("hdnCustomerID");
                    if (chkCtrl.Checked)
                        dtCampaign.Rows.Add(hdnCtrl1.Value, hdnCtrl2.Value, hdnCtrl3.Value, hdnCtrl4.Value);                    
                }
            }
            else if (drpCampaignFor.SelectedValue.ToLower() == "employee")
            {
                foreach (RepeaterItem item in grdEmployee.Items)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    HiddenField hdnCtrl1 = (HiddenField)item.FindControl("hdnEmployeeName");
                    HiddenField hdnCtrl2 = (HiddenField)item.FindControl("hdnEmployeeMail");
                    HiddenField hdnCtrl3 = (HiddenField)item.FindControl("hdnEmployeeContact");
                    HiddenField hdnCtrl4 = (HiddenField)item.FindControl("hdnEmployeeID");
                    if (chkCtrl.Checked)
                        dtCampaign.Rows.Add(hdnCtrl1.Value, hdnCtrl2.Value, hdnCtrl3.Value, hdnCtrl4.Value);
                }
            }
            else if (drpCampaignFor.SelectedValue.ToLower() == "external leads")
            {
                foreach (RepeaterItem item in grdExternal.Items)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    HiddenField hdnCtrl1 = (HiddenField)item.FindControl("hdnSenderName");
                    HiddenField hdnCtrl2 = (HiddenField)item.FindControl("hdnSenderMail");
                    HiddenField hdnCtrl3 = (HiddenField)item.FindControl("hdnSenderContact");
                    HiddenField hdnCtrl4 = (HiddenField)item.FindControl("hdnSenderID");
                    if (chkCtrl.Checked)
                        dtCampaign.Rows.Add(hdnCtrl1.Value, hdnCtrl2.Value, hdnCtrl3.Value, hdnCtrl4.Value);
                }
            }
            // -------------------------------------------------------------------
            // Getting Session Detail 
            // -------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            // -------------------------------------------------------------------
            // Sending Email to Selected List 
            // -------------------------------------------------------------------
            List<Entity.CampaignTemplate> lstTemp = new List<Entity.CampaignTemplate>();
            lstTemp = BAL.EmailTemplateMgmt.GetCampaignList(Convert.ToInt64(drpTemplate.SelectedValue), drpCampaign.SelectedValue, objAuth.UserID);

            String mailSubject = "";
            String mailBody = "";

            if (drpCampaign.SelectedValue == "Email")
            {
                mailSubject = (!String.IsNullOrEmpty(lstTemp[0].CampaignSubject)) ? lstTemp[0].CampaignSubject : "Greetings From Sharvay Infotech";
                mailBody = (!String.IsNullOrEmpty(lstTemp[0].CampaignHeader)) ? lstTemp[0].CampaignHeader : "";
                mailBody += (!String.IsNullOrEmpty(lstTemp[0].CampaignFooter)) ? lstTemp[0].CampaignFooter : "";
            }
            else if (drpCampaign.SelectedValue == "SMS")
            {
                mailBody = (!String.IsNullOrEmpty(lstTemp[0].CampaignHeader)) ? lstTemp[0].CampaignHeader : "";
            }

            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

            if (drpCampaign.SelectedValue == "Email")
             {
               // -------------------------------------------------------------------
               // Using Parallel Multi-Threading send multiple bulk email.
               // -------------------------------------------------------------------
                Parallel.ForEach(dtCampaign.AsEnumerable(), row =>
                {
                    SendCampaignEmail(objAuth.UserID, lstCompany, row["SenderMail"].ToString(), mailSubject, string.Format(mailBody, row["SenderName"]), Convert.ToInt64(row["SenderId"]),lstTemp[0].CampaignImageUrl);                    
                });
             }

            if (drpCampaign.SelectedValue == "SMS")
            {
                
                List<Entity.SMS> lstSMSConfig = new List<Entity.SMS>();
                lstSMSConfig = BAL.CommonMgmt.GetSMSConfigSettings(objAuth.CompanyID.ToString());

                for (int i = 0; i <= dtCampaign.Rows.Count - 1; i++ )
                {
                    SendCampaignSMS(objAuth.UserID, dtCampaign.Rows[i]["PrimaryMobileNo"].ToString(), mailSubject + string.Format(mailBody, dtCampaign.Rows[i]["SenderName"]), lstSMSConfig, Convert.ToInt64(dtCampaign.Rows[i]["SenderId"]));
                }
            }


            //else if (drpCampaign.SelectedValue == "SMS")
            // {

            //     List<Entity.SMS> lstSMSConfig = new List<Entity.SMS>();
            //     lstSMSConfig = BAL.CommonMgmt.GetSMSConfigSettings(objAuth.CompanyID.ToString());

            //     if (lstSMSConfig.Count >0)
            //     {
            //         // -------------------------------------------------------------------
            //         // Using Parallel Multi-Threading send multiple bulk SMS.
            //         // -------------------------------------------------------------------
            //         Parallel.ForEach(dtCampaign.AsEnumerable(), row =>
            //         {
            //             SendCampaignSMS(objAuth.UserID, row["PrimaryMobileNo"].ToString(), mailSubject + string.Format(mailBody, row["SenderName"]), lstSMSConfig);
            //         });
            //     }
            //     else
            //     {
            //         Response.Write("<script>alert('Please First Provide Valid Credentials First');</script>");
            //     }
                
            // }
        }

        private void SendCampaignSMS(string UserID, string recipient, string msg, List<Entity.SMS> lstSMSConfig, Int64 SenderID)
        {
            String respVal = "";
            respVal = BAL.CommonMgmt.SendSMSCampaign(UserID, recipient, msg, lstSMSConfig);

            int ReturnCode = 0;
            string ReturnMsg = "";

            Entity.CampaignLog objEntity = new Entity.CampaignLog();
            objEntity.pkID = 0;
            objEntity.CustomerID = SenderID; 
            objEntity.CampaignID = Convert.ToInt64(drpTemplate.SelectedValue);
            objEntity.CampaignCategory = drpCampaign.SelectedValue.ToLower();
            objEntity.CampaignFor = drpCampaignFor.SelectedValue.ToLower();
            objEntity.CampaignContact = recipient;
            objEntity.CampaignStatus = respVal;
            objEntity.LoginUserID = Session["LoginUserID"].ToString();
            BAL.EmailTemplateMgmt.AddUpdateCampaignLog(objEntity, out ReturnCode, out ReturnMsg);

        }

        private bool SendCampaignEmail(string UserID, List<Entity.CompanyProfile> lstCompany, string recipient, string subject, string body, Int64 SenderID, string imgFile)
        {
            if (lstCompany.Count > 0)
            {
                int ReturnCode = 0;
                string ReturnMsg = "";

                try
                {
                    LinkedResource Img1 = null;
                    if (!String.IsNullOrEmpty(imgFile))
                    {
                        string rootFolderPath = Server.MapPath("~/otherimages/") + imgFile;
                        //ServiceLog.WriteErrorLog("Campaign Image : " + imgFile);
                        Img1 = new LinkedResource(@rootFolderPath, MediaTypeNames.Image.Jpeg);
                        Img1.ContentId = "myImageID";
                        body = body + "<br/><br/><img src=cid:myImageID /><br />";
                    }

                    //body = body + "<br /><br />" + msgSign + "<br /><br />";

                    //create Alrternative HTML view
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

                    if (Img1 != null)
                    {
                        //Add the Image to the Alternate view
                        htmlView.LinkedResources.Add(Img1);
                    }
                    MailMessage mailMessage = new MailMessage(lstCompany[0].UserName, recipient);
                    mailMessage.Subject = subject;
                    //mailMessage.Body = body;
                    mailMessage.AlternateViews.Add(htmlView);
                    mailMessage.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = lstCompany[0].Host; //  ConfigurationManager.AppSettings["Host"];
                    if (!String.IsNullOrEmpty(lstCompany[0].EnableSSL.ToString().ToLower()))
                        smtp.EnableSsl = lstCompany[0].EnableSSL;
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    smtp.UseDefaultCredentials = false;
                    NetworkCred.UserName = lstCompany[0].UserName;      // ConfigurationManager.AppSettings["UserName"];
                    NetworkCred.Password = lstCompany[0].Password;      // ConfigurationManager.AppSettings["Password"];
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                    smtp.Send(mailMessage);

                    //System.Threading.Thread.Sleep(3000);
                    Entity.CampaignLog objEntity = new Entity.CampaignLog();
                    objEntity.pkID = 0;
                    objEntity.CustomerID = SenderID;
                    objEntity.CampaignID = Convert.ToInt64(drpTemplate.SelectedValue);
                    objEntity.CampaignCategory = drpCampaign.SelectedValue.ToLower();
                    objEntity.CampaignFor = drpCampaignFor.SelectedValue.ToLower();
                    objEntity.CampaignContact = recipient;
                    objEntity.CampaignStatus = "Sent Successfully.";
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    BAL.EmailTemplateMgmt.AddUpdateCampaignLog(objEntity, out ReturnCode, out ReturnMsg);
                }
                catch(SystemException ex)
                {
                    Entity.CampaignLog objEntity = new Entity.CampaignLog();
                    objEntity.pkID = 0;
                    objEntity.CustomerID = SenderID;
                    objEntity.CampaignID = Convert.ToInt64(drpTemplate.SelectedValue);
                    objEntity.CampaignCategory = drpCampaign.SelectedValue.ToLower();
                    objEntity.CampaignFor = drpCampaignFor.SelectedValue.ToLower();
                    objEntity.CampaignContact = recipient;
                    objEntity.CampaignStatus = "Sent unsuccessfully. : " + ex.Message;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    BAL.EmailTemplateMgmt.AddUpdateCampaignLog(objEntity, out ReturnCode, out ReturnMsg);
                }
            }
            return true;
        }

        protected void chkToCompare_CheckedChanged(object sender, EventArgs e)
        {
            Int64 Icount = 0;
            if (drpCampaignFor.SelectedValue.ToLower() == "customer")
            {
                foreach (RepeaterItem item in grdCustomer.Items)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    if (chkCtrl.Checked)
                        Icount = Icount + 1;
                }
            }
            else if (drpCampaignFor.SelectedValue.ToLower() == "employee")
            {
                foreach (RepeaterItem item in grdEmployee.Items)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    if (chkCtrl.Checked)
                        Icount = Icount + 1;
                }
            }
            else if (drpCampaignFor.SelectedValue.ToLower() == "external leads")
            {
                foreach (RepeaterItem item in grdExternal.Items)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    if (chkCtrl.Checked)
                        Icount = Icount + 1;
                }
            }

        }

        protected void grid_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (drpCampaignFor.SelectedValue.ToLower() == "customer")
                {
                    CheckBox chk = ((CheckBox)e.Item.FindControl("chkSelect"));
                    HiddenField ltr;
                    if (drpCampaign.SelectedValue.ToLower() == "email")
                        ltr = ((HiddenField)e.Item.FindControl("hdnCustomerMail"));
                    else
                         ltr = ((HiddenField)e.Item.FindControl("hdnCustomerContact"));
                    if (String.IsNullOrEmpty(ltr.Value))
                    {
                        chk.Enabled = false;
                    }
                }
                else if (drpCampaignFor.SelectedValue.ToLower() == "employee")
                {
                    CheckBox chk = ((CheckBox)e.Item.FindControl("chkSelect"));
                    HiddenField ltr;
                    if (drpCampaign.SelectedValue.ToLower() == "email")
                        ltr = ((HiddenField)e.Item.FindControl("hdnEmployeeMail"));
                    else
                        ltr = ((HiddenField)e.Item.FindControl("hdnEmployeeContact"));
                    if (String.IsNullOrEmpty(ltr.Value))
                    {
                        chk.Enabled = false;
                    }
                }
                else if (drpCampaignFor.SelectedValue.ToLower() == "external leads")
                {
                    CheckBox chk = ((CheckBox)e.Item.FindControl("chkSelect"));
                    HiddenField ltr;
                    if (drpCampaign.SelectedValue.ToLower() == "email")
                        ltr= ((HiddenField)e.Item.FindControl("hdnSenderMail"));
                    else
                        ltr = ((HiddenField)e.Item.FindControl("hdnSenderContact"));
                    if (String.IsNullOrEmpty(ltr.Value))
                    {
                        chk.Enabled = false;
                    }
                }
            }
        }
    }
}