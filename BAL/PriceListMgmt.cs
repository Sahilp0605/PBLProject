using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BAL
{
    public class PriceListMgmt
    {
        //-----------------------------------------------------------------
        // Price List
        //-----------------------------------------------------------------
        public static List<Entity.PriceList> GetPriceList(Int64 pkID, String LoginUserID)
        {
            return (new DAL.PriceListSQL().GetPriceList(pkID, LoginUserID));
        }
        public static List<Entity.PriceList> GetPriceList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PriceListSQL().GetPriceList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.PriceList> GetPriceList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PriceListSQL().GetPriceList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdatePriceList(Entity.PriceList entity, out int ReturnCode, out string ReturnMsg,out Int64 ReturnpkID)
        {
            new DAL.PriceListSQL().AddUpdatePriceList(entity, out ReturnCode, out ReturnMsg, out ReturnpkID);
        }
        public static void DeletePriceList(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PriceListSQL().DeletePriceList(pkID, out ReturnCode, out ReturnMsg);
        }
        //-----------------------------------------------------------------
        // Price List Detail
        //-----------------------------------------------------------------
        public static DataTable GetPriceListDetail(Int64 ParentID)
        {
            return (new DAL.PriceListSQL().GetPriceListDetail(ParentID));
        }
        public static List<Entity.PriceListDetail> GetPriceListDetail(Int64 ParentID, String LoginUserID)
        {
            return (new DAL.PriceListSQL().GetPriceListDetail(ParentID, LoginUserID));
        }
        public static List<Entity.PriceListDetail> GetPriceListDetail(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PriceListSQL().GetPriceListDetail(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.PriceListDetail> GetPriceListDetail(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PriceListSQL().GetPriceListDetail(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdatePriceListDetail(Entity.PriceListDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PriceListSQL().AddUpdatePriceListDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeletePriceListDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PriceListSQL().DeletePriceListDetail(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeletePriceListDetailByNo(Int64 parentID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PriceListSQL().DeletePriceListDetailByNo(parentID, out ReturnCode, out ReturnMsg);
        }
        //-----------------------------------------------------------------
        // Price List Detail Upload Download
        //-----------------------------------------------------------------
        public static void AddUpdatePriceListUPDOWN(Entity.PriceListDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PriceListSQL().AddUpdatePriceListUPDOWN(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
