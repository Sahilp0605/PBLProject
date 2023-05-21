using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class MoldingMgmt
    {
        public static List<Entity.MoldingDetail> GetProductsByOrderNo(String OrderNo)
        {
            return (new DAL.MoldingSQL().GetProductsByOrderNo(OrderNo));
        }

        public static List<Entity.Molding> GetMoldingList(String LoginUserID)
        {
            return (new DAL.MoldingSQL().GetMoldingList(LoginUserID));
        }

        public static List<Entity.Molding> GetMoldingList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.MoldingSQL().GetMoldingList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Molding> GetMoldingList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.MoldingSQL().GetMoldingList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateMolding(Entity.Molding entity, out int ReturnCode, out string ReturnMsg, out string ReturnInwardNo)
        {
            new DAL.MoldingSQL().AddUpdateMolding(entity, out ReturnCode, out ReturnMsg, out ReturnInwardNo);
        }

        public static void DeleteMolding(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MoldingSQL().DeleteMolding(pkID, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetMoldingDetail(string MoldingNo)
        {
            return (new DAL.MoldingSQL().GetMoldingDetail(MoldingNo));
        }

        public static DataTable GetMoldingProducts(string OrderNo)
        {
            return (new DAL.MoldingSQL().GetMoldingProducts(OrderNo));
        }

        public static DataTable GetSOProducts(string OrderNo)
        {
            return (new DAL.MoldingSQL().GetSOProducts(OrderNo));
        }

        public static void AddUpdateMoldingDetail(Entity.MoldingDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MoldingSQL().AddUpdateMoldingDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMoldingDetailByMoldingNo(string pMoldingNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MoldingSQL().DeleteMoldingDetailByMoldingNo(pMoldingNo, out ReturnCode, out ReturnMsg);
        }
    }
}
