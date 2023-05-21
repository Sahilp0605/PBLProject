using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class OutwardMgmt
    {
        public static List<Entity.Outward> GetOutwardList(String LoginUserID)
        {
            return (new DAL.OutwardSQL().GetOutwardList(LoginUserID));
        }

        public static List<Entity.Outward> GetOutwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OutwardSQL().GetOutwardList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Outward> GetOutwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OutwardSQL().GetOutwardList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        //Created by Vikram Rajput
        public static List<Entity.Outward> GetOutwardList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.OutwardSQL().GetOutwardList(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.Outward> GetOutwardListPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.OutwardSQL().GetOutwardList(pLoginUserID, 0, 0, FromDate, ToDate));
        }

        public static List<Entity.Outward> GetOutwardListByCustomerProduct(Int64 pCustomerID, Int64 pProductID, string pLoginUserID)
        {
            return (new DAL.OutwardSQL().GetOutwardListByCustomerProduct(pCustomerID, pProductID, pLoginUserID));
        }

        public static void AddUpdateOutward(Entity.Outward entity, out int ReturnCode, out string ReturnMsg, out string ReturnOutwardNo)
        {
            new DAL.OutwardSQL().AddUpdateOutward(entity, out ReturnCode, out ReturnMsg, out ReturnOutwardNo);
        }

        public static void DeleteOutward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().DeleteOutward(pkID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetOutwardDetail(string pOutwardNo)
        {
            return (new DAL.OutwardSQL().GetOutwardDetail(pOutwardNo));
        }

        public static List<Entity.OutwardDetail> GetOutwardDetailList()
        {
            return (new DAL.OutwardSQL().GetOutwardDetailList());
        }

        public static List<Entity.OutwardDetail> GetOutwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OutwardSQL().GetOutwardDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateOutwardDetail(Entity.OutwardDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().AddUpdateOutwardDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteOutwardDetailByOutwardNo(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().DeleteOutwardDetailByOutwardNo(pOutwardNo, out ReturnCode, out ReturnMsg);
        }
        // -----------------------------------------------------
        public static List<Entity.OutwardDetailAssembly> GetOutwardDetailAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            return (new DAL.OutwardSQL().GetOutwardDetailAssemblyList(pOutwardNo, pProductID, pAssemblyID));
        }
        public static void AddUpdateOutwardDetailAssembly(Entity.OutwardDetailAssembly entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().AddUpdateOutwardDetailAssembly(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteOutwardDetailAssemblyByOutwardNo(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().DeleteOutwardDetailAssemblyByOutwardNo(pOutwardNo, pProductID, pAssemblyID, out ReturnCode, out ReturnMsg);
        }


        // -----------------------------------------------------------------------------
        // Export Detail
        // -----------------------------------------------------------------------------
        public static List<Entity.Outward> GetOutwardExportList(Int64 pkID, string OutwardNo, string LoginUserID)
        {
            return (new DAL.OutwardSQL().GetOutwardExportList(pkID, OutwardNo, LoginUserID));
        }
        public static void AddUpdateOutwardExport(Entity.Outward entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().AddUpdateOutwardExport(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteOutwardExport(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().DeleteOutwardExport(pOutwardNo, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.Outward_Attachment> GetOutwardAttachmentList(Int64 pkID, string OutwardNo)
        {
            return (new DAL.OutwardSQL().GetOutwardAttachmentList(pkID, OutwardNo));
        }

        public static void DeleteOutwardAttachment(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().DeleteOutwardAttachment(pOrderNo, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateOutwardAttachment(Entity.Outward_Attachment entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OutwardSQL().AddUpdateOutwardAttachment(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
