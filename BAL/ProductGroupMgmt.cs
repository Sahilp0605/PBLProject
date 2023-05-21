using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ProductGroupMgmt
    {
        public static List<Entity.ProductGroup> GetProductGroupList()
        {
            return (new DAL.ProductGroupSQL().GetProductGroupList());
        }

        public static List<Entity.ProductGroup> GetProductGroupList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductGroupSQL().GetProductGroupList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ProductGroup> GetProductGroupList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductGroupSQL().GetProductGroupList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateProductGroup(Entity.ProductGroup entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductGroupSQL().AddUpdateProductGroup(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteProductGroup(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductGroupSQL().DeleteProductGroup(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
