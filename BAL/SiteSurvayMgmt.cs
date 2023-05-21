using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class SiteSurvayMgmt
    {
        public static List<Entity.SiteSurvay> GetSiteSurvay(String LoginUserID)
        {
            return (new DAL.SiteSurvaySQL().GetSiteSurvay(LoginUserID));
        }

        public static List<Entity.SiteSurvay> GetSiteSurvay(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SiteSurvaySQL().GetSiteSurvay(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.SiteSurvay> GetSiteSurvay(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.SiteSurvaySQL().GetSiteSurvay(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateSiteSurvay(Entity.SiteSurvay entity, out int ReturnCode, out string ReturnMsg, out String ReturnSiteSurvayNo)
        {
            new DAL.SiteSurvaySQL().AddUpdateSiteSurvay(entity, out ReturnCode, out ReturnMsg,out ReturnSiteSurvayNo);
        }

        public static void DeleteSiteSurvay(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurvaySQL().DeleteSiteSurvay(pkID, out ReturnCode, out ReturnMsg);
        }

        //==========================================================//
        // Site Survay Documents
        //==========================================================//
        public static List<Entity.SiteSurvayDocuments> GetSiteSurvayDocumentsList(Int64 pkID, String DocNo, String Type)
        {
            return (new DAL.SiteSurvaySQL().GetSiteSurvayDocumentsList(pkID, DocNo, Type));
        }

        public static void DeleteSiteSurvayDocumentsByDocNo(String DocNo, String Type, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurvaySQL().DeleteSiteSurvayDocumentsByDocNo(DocNo,Type, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateSiteSurvayDocuments(String DocNo, string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.SiteSurvaySQL().AddUpdateSiteSurvayDocuments(DocNo, pFilename, pType, pLoginUserID, out ReturnCode, out ReturnMsg);
        }
    }
}
