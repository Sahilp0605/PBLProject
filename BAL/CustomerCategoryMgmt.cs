using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CustomerCategoryMgmt
    {
        public static List<Entity.CustomerCategory> GetCustomerCategoryList()
        {
            return (new DAL.CustomerCategorySQL().GetCustomerCategoryList());
        }

        public static List<Entity.CustomerCategory> GetCustomerCategoryList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CustomerCategorySQL().GetCustomerCategoryList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.CustomerCategory> GetCustomerCategoryList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CustomerCategorySQL().GetCustomerCategoryList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCustomerCategory(Entity.CustomerCategory entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerCategorySQL().AddUpdateCustomerCategory(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerCategory(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerCategorySQL().DeleteCustomerCategory(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
