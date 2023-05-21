using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;


namespace DAL
{
    public class MailNotificationSQL : BaseSqlManager
    {
        public virtual void sendEmailNotification(Entity.EmailStructure objEntity, out int ReturnCode, out string ReturnMsg)
        {
            Int64 pPortNumber = 0;
            bool pEnableSSL = false;
            string pEmailFrom = "", pEmailTo = "", pEmailCC = "", pEmailBCC = "", pSubject = "", pHost = "", pUserName = "", pPassword = "", pBody = "";
            // ------------------------------------------------
            ReturnCode = 1;
            ReturnMsg = "Email Successfully Sent !";
            // ------------------------------------------------
            pEmailTo = objEntity.EmailTo;
            pEmailCC = objEntity.EmailCC;
            pEmailBCC = objEntity.EmailBCC; 
            pBody = objEntity.Body;
            pSubject = objEntity.Subject;
            // ------------------------------------------------
            CommonSQL csObj = new CommonSQL();
            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = csObj.GetCompanyProfileList(1, "admin");
            
            pEmailFrom = lstCompany[0].UserName;
            pHost = lstCompany[0].Host;
            pUserName = lstCompany[0].UserName;
            pPassword = lstCompany[0].Password;
            pEnableSSL = lstCompany[0].EnableSSL;
            pPortNumber = lstCompany[0].PortNumber;
            // ------------------------------------------------
            if (!String.IsNullOrEmpty(pEmailFrom) && !String.IsNullOrEmpty(pEmailTo) && !String.IsNullOrEmpty(pHost) && !String.IsNullOrEmpty(pUserName) && !String.IsNullOrEmpty(pPassword))
            {
                try
                {
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(pEmailFrom);
                        mailMessage.Subject = objEntity.Subject;
                        mailMessage.Body = pBody;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.To.Add(new MailAddress("mrunalyoddha@gmail.com"));
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = pHost;
                        smtp.EnableSsl = pEnableSSL;
                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                        NetworkCred.UserName = pUserName;
                        NetworkCred.Password = pPassword;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = Convert.ToInt16(pPortNumber);
                        smtp.Send(mailMessage);
                    }
                }
                catch (Exception ex)
                {
                    ReturnCode = -1;
                    ReturnMsg = "Some Exception Occured !";
                }
            }           
        }
    }
}
