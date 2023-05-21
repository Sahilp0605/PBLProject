using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CampaignTemplateMgmt
    {
        public static List<Entity.CampaignTemplate> GetCampaignTemplate(Int64 CampaignID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CampaignTemplateSQL().GetCampaignTemplate(CampaignID, "", "", PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.CampaignTemplate> GetCampaignTemplate(Int64 CampaignID, String Category, String SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CampaignTemplateSQL().GetCampaignTemplate(CampaignID, Category, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCampaignTemplate(Entity.CampaignTemplate entity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnCampaignID)
        {
            new DAL.CampaignTemplateSQL().AddUpdateCampaignTemplate(entity, out ReturnCode, out ReturnMsg, out ReturnCampaignID);
        }
        public static void DeleteCampaignTemplate(Int64 CampaignID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CampaignTemplateSQL().DeleteCampaignTemplate(CampaignID, out ReturnCode, out ReturnMsg);
        }
    }
}
