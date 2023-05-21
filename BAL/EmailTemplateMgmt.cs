using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class EmailTemplateMgmt
    {
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Email Template 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.EmailTemplate> GetEmailTemplateList()
        {
            return (new DAL.EmailTemplateSQL().GetEmailTemplateList());
        }

        public static List<Entity.EmailTemplate> GetEmailTemplate(string EmailTemplateCode, string Category, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.EmailTemplateSQL().GetEmailTemplate(EmailTemplateCode, Category, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateEmailTemplate(Entity.EmailTemplate entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.EmailTemplateSQL().AddUpdateEmailTemplate(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteEmailTemplate(string EmailTemplateCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.EmailTemplateSQL().DeleteEmailTemplate(EmailTemplateCode, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Email/SMS .... Campaign Template 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.CampaignTemplate> GetCampaignList(Int64 CampaignID, String CampaignCategory, String LoginUserID)
        {
            return (new DAL.EmailTemplateSQL().GetCampaignList(CampaignID, CampaignCategory, LoginUserID));
        }

        public static void AddUpdateCampaignLog(Entity.CampaignLog entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.EmailTemplateSQL().AddUpdateCampaignLog(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.Customer> GetCampaignCustomerList(string Category,string CustCategory)
        {
            return (new DAL.EmailTemplateSQL().GetCampaignCustomerList(Category, CustCategory));
        }

        public static List<Entity.OrganizationEmployee> GetCampaignEmployeeList(string Category, string empDesignation)
        {
            return (new DAL.EmailTemplateSQL().GetCampaignEmployeeList(Category, empDesignation));
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // General Template 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.EmailTemplate> GetGeneralTemplate(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.EmailTemplateSQL().GetGeneralTemplate(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.EmailTemplate> GetGeneralTemplate(Int64 pkID,string SearchKey, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.EmailTemplateSQL().GetGeneralTemplate(pkID,SearchKey ,LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateGeneralTemplate(Entity.EmailTemplate entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.EmailTemplateSQL().AddUpdateGeneralTemplate(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteGeneralTemplate(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.EmailTemplateSQL().DeleteGeneralTemplate(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
