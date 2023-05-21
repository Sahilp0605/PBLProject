using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ProductSpecificationMgmt
    {
        public static List<Entity.ProductSpecification> GetProductSpecificationList(Int64 pProductID)
        {
            return (new DAL.ProductSpecificationSQL().GetProductSpecificationList(pProductID));
        }
        public static void AddUpdateProductSpecification(Entity.ProductSpecification entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSpecificationSQL().AddUpdateProductSpecification(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductSpecificationByProductID(Int64 pProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSpecificationSQL().DeleteProductSpecificationByProductID(pProductID, out ReturnCode, out ReturnMsg);
        }
        //======================================================================================================//
        public static List<Entity.ProductSpecification> GetProductSpecificationList()
        {
            return (new DAL.ProductSpecificationSQL().GetProductSpecificationList());
        }
       
        public static List<Entity.ProductSpecification> GetProductSpecificationList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductSpecificationSQL().GetProductSpecificationList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ProductSpecification> GetProductSpecGroupList(string GroupHead)
        {
            return (new DAL.ProductSpecificationSQL().GetProductSpecGroupList(GroupHead));
        }

       

        public static void DeleteProductSpecification(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSpecificationSQL().DeleteProductSpecification(pkID, out ReturnCode, out ReturnMsg);
        }


    }
}
