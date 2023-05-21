using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CustomerProductsMgmt
    {

        public static DataTable GetCustomerProductsDetail(Int64 pCustomerID)
        {
            return (new DAL.CustomerProductsSQL().GetCustomerProductsDetail(pCustomerID));
        }

        public static DataTable GetProductsDetail(Int64 pCustomerID)
        {
            return (new DAL.CustomerProductsSQL().GetProductsDetail(pCustomerID));
        }
        public static List<Entity.CustomerProducts> GetCustomerProductsList()
        {
            return (new DAL.CustomerProductsSQL().GetCustomerProductsList());
        }

        public static List<Entity.CustomerProducts> GetCustomerProductsList(Int64 CustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CustomerProductsSQL().GetCustomerProductsList(CustomerID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCustomerProducts(Entity.CustomerProducts entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerProductsSQL().AddUpdateCustomerProducts(entity, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateCustomerPro(Entity.CustomerProducts entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerProductsSQL().AddUpdateCustomerPro(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerProducts(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerProductsSQL().DeleteCustomerProducts(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerProductsByCustomer(Int64 CustID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerProductsSQL().DeleteCustomerProductsByCustomer(CustID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductsByCustomer(Int64 CustID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerProductsSQL().DeleteProductsByCustomer(CustID, out ReturnCode, out ReturnMsg);
        }
    }
}
