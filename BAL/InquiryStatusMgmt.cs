using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class InquiryStatusMgmt
    {
        public static List<Entity.InquiryStatus> GetInquiryStatusList(string statuscategory)
        {
            return (new DAL.InquiryStatusSQL().GetInquiryStatusList(statuscategory));
        }

        public static List<Entity.InquiryStatus> GetInquiryStatusList(Int64 pkID, string statuscategory, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InquiryStatusSQL().GetInquiryStatusList(pkID, statuscategory, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.InquiryStatus> GetInquiryStatusList(Int64 pkID, string statuscategory, string LoginUserID,  string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InquiryStatusSQL().GetInquiryStatusList(pkID, statuscategory, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateInquiryStatus(Entity.InquiryStatus entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryStatusSQL().AddUpdateInquiryStatus(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInquiryStatus(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryStatusSQL().DeleteInquiryStatus(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
