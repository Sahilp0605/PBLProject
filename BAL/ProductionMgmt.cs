using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ProductionMgmt
    {
        public static List<Entity.Production> GetProductionList()
        {
            return (new DAL.ProductionSQL().GetProductionList());
        }
        public static List<Entity.Production> GetProduction(Int64 pkID, string LoginUserID, int PageNo, int PageProduction, out int TotalRecord)
        {
            return (new DAL.ProductionSQL().GetProductionList(pkID, LoginUserID, PageNo, PageProduction, out TotalRecord));
        }
        public static List<Entity.Production> GetProduction(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageProduction, out int TotalRecord)
        {
            return (new DAL.ProductionSQL().GetProductionList(pkID, LoginUserID, SearchKey, PageNo, PageProduction, out TotalRecord));
        }
        public static void AddUpdateProduction(Entity.Production entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().AddUpdateProduction(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProduction(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().DeleteProduction(pkID, out ReturnCode, out ReturnMsg);
        }

        //-------------Production Detail----------------------//
        public static void AddUpdateProductionDetail(Entity.Production entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().AddUpdateProductionDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductionDetailByParentID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().DeleteProductionDetailByParentID(pkID, out ReturnCode, out ReturnMsg);
        }
        public static DataTable GetProductionDetail(Int64 pkID)
        {
            return (new DAL.ProductionSQL().GetProductionDetail(pkID));
        }

        //-------------Production By SO----------------------//
        public static List<Entity.Production> GetProductionBySoList()
        {
            return (new DAL.ProductionSQL().GetProductionBySoList());
        }
        public static List<Entity.Production> GetProductionBySoList(Int64 pkID, string LoginUserID, int PageNo, int PageProduction, out int TotalRecord)
        {
            return (new DAL.ProductionSQL().GetProductionBySoList(pkID, LoginUserID, PageNo, PageProduction, out TotalRecord));
        }
        public static List<Entity.Production> GetProductionBySoList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageProduction, out int TotalRecord)
        {
            return (new DAL.ProductionSQL().GetProductionBySoList(pkID, LoginUserID, SearchKey, PageNo, PageProduction, out TotalRecord));
        }
        public static void AddUpdateProductionBySo(Entity.Production entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().AddUpdateProductionBySo(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductionBySo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().DeleteProductionBySo(pkID, out ReturnCode, out ReturnMsg);
        }
        
        //-------------Production By SO Detail----------------------//
        public static void AddUpdateProductionBySoDetail(Entity.Production entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().AddUpdateProductionBySoDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductionBySoDetailbyParentID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().DeleteProductionBySoDetailbyParentID(pkID, out ReturnCode, out ReturnMsg);
        }
        public static DataTable GetProductionDetailBySo(Int64 pkID)
        {
            return (new DAL.ProductionSQL().GetProductionDetailBySo(pkID));
        }

        //-------------Production By SO Raw Detail----------------------//
        public static void AddUpdateProductionBySoRawDetail(Entity.Production entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().AddUpdateProductionBySoRawDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductionRawDetailByParentID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductionSQL().DeleteProductionRawDetailByParentID(pkID, out ReturnCode, out ReturnMsg);
        }
        public static DataTable GetProductionRawDetail(Int64 pkID)
        {
            return (new DAL.ProductionSQL().GetProductionRawDetail(pkID));
        }

    }


}
