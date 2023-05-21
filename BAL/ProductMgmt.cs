using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ProductMgmt
    {
        public static List<Entity.ProductAssemblyStock> GetAssemblyStockSummary(string pStatus, string pOrderNo)
        { 
            return (new DAL.ProductSQL().GetAssemblyStockSummary(pStatus, pOrderNo));
        }

        public static List<Entity.ProductAssemblyStock> GetAssemblyStockSummaryProductWise(string pStatus, Int64 ProductID, double Quantity)
        {
            return (new DAL.ProductSQL().GetAssemblyStockSummaryProductWise(pStatus, ProductID, Quantity));
        }

        public static List<Entity.Products> GetProductListForDropdown(string pProductName) 
        {
            return (new DAL.ProductSQL().GetProductListForDropdown(pProductName));
        }
        public static List<Entity.Products> GetProductListForDropdown(string pProductName, string pSearchModule)
        {
            return (new DAL.ProductSQL().GetProductListForDropdown(pProductName, pSearchModule));
        }

        public static List<Entity.Products> GetProductListForDropdownForMaterialIndent(string pProductName, string pSearchModule)
        {
            return (new DAL.ProductSQL().GetProductListForDropdownForMaterialIndent(pProductName, pSearchModule));
        }

        public static List<Entity.Products> GetProductListForDropdown(string SerialKey,string pProductName, string pSearchModule, Int64 CustomerID)
        {
            return (new DAL.ProductSQL().GetProductListForDropdown(SerialKey,pProductName, pSearchModule, CustomerID));
        }


        public static List<Entity.Products> GetProductList()
        {
            return (new DAL.ProductSQL().GetProductList());
        }

        public static List<Entity.Products> GetProductList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductSQL().GetProductList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Products> GetProductList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductSQL().GetProductList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Products> GetProductList(string pProductName)
        {
            return (new DAL.ProductSQL().GetProductList(pProductName));
        }

        public static List<Entity.Products> GetProductCategoryList(Int64 pkID)
        {
            return (new DAL.ProductSQL().GetProductCategoryList(pkID));
        }

        public static List<Entity.ProductOpeningStk> GetProductOpeningStockList(Int64 pkID, string LoginUserID, string FinancialYear,string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductSQL().GetProductOpeningStockList(pkID, LoginUserID, FinancialYear,SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.InwardOutwardLedger> GetInwardOutwardLedger(Int64 ProjectID, Int64 LocationID, Int64 pkID, string pLedgerType)
        {
            return (new DAL.ProductSQL().GetInwardOutwardLedger(ProjectID, LocationID, pkID, pLedgerType));
        }
        // ------------------------------------------------------------------------------
        public static void AddUpdateProduct(Entity.Products entity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnProductId)
        {
            new DAL.ProductSQL().AddUpdateProduct(entity, out ReturnCode, out ReturnMsg, out ReturnProductId);
        }

        public static void AddUpdateProductUPDOWN(Entity.Products entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateProductUPDOWN(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteProduct(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProduct(pkID, out ReturnCode, out ReturnMsg);
        }
        // ------------------------------------------------------------------------------
        // Section : Product Master Detail 
        // ------------------------------------------------------------------------------
        public static List<Entity.ProductDetailCard> GetProductDetailList(Int64 pkID, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetProductDetailList(pkID, FinishProductID, LoginUserID));
        }

        public static List<Entity.ProductDetailCard> GetProductDetailListForProduction(Int64 pkID, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetProductDetailListForProduction(pkID, FinishProductID, LoginUserID));
        }
        public static List<Entity.ProductDetailCard> GetAssembly(Int64 FinishProductID)
        {
            return (new DAL.ProductSQL().GetAssembly(FinishProductID));
        }
        public static void AddUpdateProductDetail(Entity.ProductDetailCard entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateProductDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductDetail(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductDetailByFinishProductID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductDetailByFinishProductID(pkID, out ReturnCode, out ReturnMsg);
        }

        // ------------------------------------------------------------------------------
        // Section : Product Accessories
        // ------------------------------------------------------------------------------
        public static List<Entity.ProductDetailCard> GetProductAccessoriesList(Int64 pkID, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetProductAccessoriesList(pkID, FinishProductID, LoginUserID));
        }
        public static void AddUpdateProductAccessories(Entity.ProductDetailCard entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateProductAccessories(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductAccessories(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductAccessories(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductAccessoriesByFinishProductID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductAccessoriesByFinishProductID(pkID, out ReturnCode, out ReturnMsg);
        }

        // ------------------------------------------------------------------------------
        // Section : Product Master Specification For Quotation
        // ------------------------------------------------------------------------------
        public static List<Entity.ProductDetailCard> GetQuotationProductSpecList(String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetQuotationProductSpecList(QuotationNo, FinishProductID, LoginUserID));
        }

        public static void AddUpdateQuotationProductSpec(Entity.ProductDetailCard entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateQuotationProductSpec(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteQuotationProductSpec(String QuotationNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteQuotationProductSpec(QuotationNo, FinishProductID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteUnwantedSpec(String QuotationNo)
        {
            new DAL.ProductSQL().DeleteUnwantedSpec(QuotationNo);
        }

        public static void DeleteUnwantedSubsidy(String QuotationNo)
        {
            new DAL.ProductSQL().DeleteUnwantedSubsidy(QuotationNo);
        }


        public static List<Entity.Products> GetQuotProdSpecList(String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetQuotProdSpecList(QuotationNo, FinishProductID, LoginUserID));
        }
        public static List<Entity.Products> GetQuotProdSpecList(String QuotationNo, Int64 FinishProductID,Int64 pkID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetQuotProdSpecList(QuotationNo, FinishProductID, pkID, LoginUserID));
        }

        public static List<Entity.Products> GetSOQuotProdSpecList(String OrderNo,String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetSOQuotProdSpecList(OrderNo, QuotationNo, FinishProductID, LoginUserID));
        }
        public static List<Entity.Products> GetProdSpecList(String Module,String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetProdSpecList(Module,QuotationNo, FinishProductID, LoginUserID));
        }
        // ------------------------------------------------------------------------------
        // Section : Product Master Specification For SalesOrder
        // ------------------------------------------------------------------------------
        public static List<Entity.ProductDetailCard> GetSalesOrderProductSpecList(String OrderNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetSalesOrderProductSpecList(OrderNo, FinishProductID, LoginUserID));
        }

        public static void AddUpdateSalesOrderProductSpec(Entity.ProductDetailCard entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateSalesOrderProductSpec(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderProductSpec(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteSalesOrderProductSpec(OrderNo, FinishProductID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteUnwantedSalesOrderSpec(String OrderNo)
        {
            new DAL.ProductSQL().DeleteUnwantedSalesOrderSpec(OrderNo);
        }

        // ------------------------------------------------------------------------------
        // Section : Product Master Specification For Dealer Sales Order
        // ------------------------------------------------------------------------------
        public static List<Entity.ProductDetailCard> GetSalesOrderDealerProductSpecList(String OrderNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetSalesOrderDealerProductSpecList(OrderNo, FinishProductID, LoginUserID));
        }

        public static void AddUpdateSalesOrderDealerProductSpec(Entity.ProductDetailCard entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateSalesOrderDealerProductSpec(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteSalesOrderDealerProductSpec(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteSalesOrderDealerProductSpec(OrderNo, FinishProductID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteUnwantedSalesOrderDealerSpec(String OrderNo)
        {
            new DAL.ProductSQL().DeleteUnwantedSalesOrderDealerSpec(OrderNo);
        }

        // ------------------------------------------------------------------------------
        // Section : Product Master Specification For PurchaseOrder
        // ------------------------------------------------------------------------------
        public static List<Entity.ProductDetailCard> GetPurchaseOrderProductSpecList(String OrderNo, Int64 FinishProductID, string LoginUserID)
        {
            return (new DAL.ProductSQL().GetPurchaseOrderProductSpecList(OrderNo, FinishProductID, LoginUserID));
        }

        public static void AddUpdatePurchaseOrderProductSpec(Entity.ProductDetailCard entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdatePurchaseOrderProductSpec(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeletePurchaseOrderProductSpec(String OrderNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeletePurchaseOrderProductSpec(OrderNo, FinishProductID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteUnwantedPurchaseOrderSpec(String OrderNo)
        {
            new DAL.ProductSQL().DeleteUnwantedPurchaseOrderSpec(OrderNo);
        }

        /*---------------------------------------------------------------------------*/
        /* Upload : Product Images                                                   */
        /*---------------------------------------------------------------------------*/
        public static List<Entity.ProductImages> GetProductImageList(Int64 pkID, Int64 ProductID)
        {
            return (new DAL.ProductSQL().GetProductImageList(pkID, ProductID));
        }
        public static void AddUpdateProductImages(Entity.ProductImages entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().AddUpdateProductImages(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductImage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductImage(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteProductImageByProductID(Int64 ProductID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductImageByProductID(ProductID, out ReturnCode, out ReturnMsg);
        }

        //-----------------------------Serial No Filteration ----------------------------
        public static List<Entity.ProductDetailCard> GetSerialNoForDropdown(Int64 ProductID)
        {
            return (new DAL.ProductSQL().GetSerialNoForDropdown(ProductID));
        }
        public static List<Entity.ProductDetailCard> GetSerialNoForDropdown(Int64 ProductID, String Module)
        {
            return (new DAL.ProductSQL().GetSerialNoForDropdown(ProductID, Module));
        }
        //public static List<Entity.ProductSerialNoHistory> GetSerialNoHistory(Int64 ProductID, String SerialNo, String FilterType)
        //{
        //    return (new DAL.ProductSQL().GetSerialNoHistory(ProductID, SerialNo, FilterType));
        //}
        public static List<Entity.ProductDetailCard> GetProductSerialNoListByInvoiceNo(String Module, String InvoiceNo, Int64 ProductID)
        {
            return (new DAL.ProductSQL().GetProductSerialNoListByInvoiceNo(Module, InvoiceNo, ProductID));
        }

        //------------------------------------------------------Product Hexagon----------------------------------------------------------
        public static List<Entity.Products> GetProductHexagonList()
        {
            return (new DAL.ProductSQL().GetProductHexagonList());
        }

        public static List<Entity.Products> GetProductHexagonList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProductSQL().GetProductHexagonList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateProductHexagon(Entity.Products entity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnProductId)
        {
            new DAL.ProductSQL().AddUpdateProductHexagon(entity, out ReturnCode, out ReturnMsg, out ReturnProductId);
        }

        public static void DeleteProductHexagon(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProductSQL().DeleteProductHexagon(pkID, out ReturnCode, out ReturnMsg);
        }

    }

    public class BrandMgmt
    {
        public static List<Entity.Brand> GetBrandList()
        {
            return (new DAL.BrandSQL().GetBrandList());
        }

        public static List<Entity.Brand> GetBrandList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.BrandSQL().GetBrandList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Brand> GetBrandList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.BrandSQL().GetBrandList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateBrand(Entity.Brand entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BrandSQL().AddUpdateBrand(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteBrand(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BrandSQL().DeleteBrand(pkID, out ReturnCode, out ReturnMsg);
        }
       
    }
}
