using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Products
    {
        public Int64 pkID { get; set; }
        public string ProductName { get; set; }
        public Int64 LocationID { get; set; }
        public string LocationName { get; set; }
        public string ProductNameLong { get; set; }
        public string ProductNameLongStk { get; set; }
        public string ProductAlias { get; set; }
        public string ProductType { get; set; }

        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal AddTaxPer { get; set; }

        public string ProductSpecification { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        
        public string ProductImage { get; set; }
        public string HSNCode { get; set; }
        
        public Int64 UnitQuantity { get; set; }
        public string UnitSize { get; set; }
        public string UnitSurface { get; set; }
        public Nullable<DateTime> LRDate { get; set; }
        public string UnitGrade { get; set; }
        public decimal Box_Weight { get; set; }
        public decimal Box_SQFT { get; set; }
        public decimal Box_SQMT { get; set; }

        
        public int  TaxType { get; set; }
        public decimal ManPower { get; set; }
        public decimal HorsePower { get; set; }

        public decimal Min_UnitPrice { get; set; }
        public decimal Max_UnitPrice { get; set; }
        public decimal ProfitRatio { get; set; }
        public decimal MinQuantity { get; set; }

        public decimal OpeningSTK { get; set; }
        public decimal InwardSTK { get; set; }
        public decimal OutwardSTK { get; set; }
        public decimal ClosingSTK { get; set; }
        public decimal OpeningValuation { get; set; }
        public decimal OpeningWeightRate { get; set; }


        public DateTime AsOnDate { get; set; }
        
        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public String Name { get; set; }
        public String Type { get; set; }
        public String data { get; set; }
    }
    public class ProductImages
    {
        public Int64 pkID { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public String ContentData { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class ProductDetailCard
    {
        public Int64 pkID { get; set; }
        public Int64 FinishProductID { get; set; }
        public string FinishProductName { get; set; }
        public string FinishProductNameLong { get; set; }
        public string FinishProductAlias { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }
        public string ProductAlias { get; set; }

        public Decimal AssQty { get; set; }
        public Decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public decimal AddTaxPer { get; set; }

        public string ProductSpecification { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string ProductImage { get; set; }

        public double ClosingSTK { get; set; }
        public int TaxType { get; set; }
        public string LoginUserID { get; set; }

        public string QuotationNo {get; set; }
        public string OrderNo { get; set; } 
        public string GroupHead { get; set; }
        public string MaterialHead { get; set; }
        public string MaterialSpec { get; set; }
        public string ItemOrder { get; set; }
        public string MaterialRemarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string Module { get; set; }
        public string RefNo { get; set; }
        public string InvoiceNo { get; set; }
        public string SerialNo { get; set; }
    }
    public class ProductSerialNoHistory
    {
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string SerialNo { get; set; }
        public Int64 PurcpkID { get; set; }
        public string PurchaseBillNo { get; set; }
        public DateTime PurchaseBillDate { get; set; }
        public string PurchaseCustomer { get; set; }
        public Nullable<DateTime> PurchaseExpiryDate { get; set; }
        public Int64 PurchaseExpiryDays { get; set; }
        public Int64 SalepkID { get; set; }
        public string SalesBillNo { get; set; }
        public DateTime SalesBillDate { get; set; }
        public string SalesCustomer { get; set; }
    }
    public class Brand
    {
        public Int64 pkID { get; set; }
        
        public string BrandName { get; set; }
        public string BrandAlias { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class Units
    {
        public Int64 pkID { get; set; }
        public string UnitName { get; set; }
    }

    public class ProductOpeningStk
    {
        public Int64 pkID { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductAlias { get; set; }

        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public string Unit { get; set; }
        public Decimal Quantity { get; set; }
        public DateTime AsOnDate { get; set; }
    }

    public class ProductStockReport
    {
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductAlias { get; set; }
        public string ProductGroupName { get; set; }
        public string BrandName { get; set; }
        public DateTime TranDate { get; set; }
        public decimal OpeningSTK { get; set; }
        public decimal InwardSTK { get; set; }
        public decimal OutwardSTK { get; set; }
        public decimal ClosingSTK { get; set; }

    }

    public class InwardOutwardLedger
    {
        public Int64 ProjectID { get; set; }
        public Int64 LocationID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string ProductAlias { get; set; }
        public string ProductGroupName { get; set; }
        public string BrandName { get; set; }
        public string TransType { get; set; }
        public string Module { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal OpeningSTK { get; set; }
        public decimal InwardQty { get; set; }
        public decimal OutwardQty { get; set; }
        public decimal InwardAmt { get; set; }
        public decimal OutwardAmt { get; set; }
        public decimal ClosingSTK { get; set; }

    }
    public class Location
    {
        public Int64 pkID { get; set; }
        public string LocationName { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }

        public decimal OpeningSTK { get; set; }
        public decimal InwardSTK { get; set; }
        public decimal OutwardSTK { get; set; }
        public decimal ClosingSTK { get; set; }
        public string LoginUserID { get; set; }

        public string Module { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string TransType { get; set; }
        public decimal Quantity { get; set; }

    }
    public class Size
    {
        public Int64 pkID { get; set; }
        public string SizeName { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Color
    {
        public Int64 pkID { get; set; }
        public string ColorName { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Punch
    {
        public Int64 pkID { get; set; }
        public string PunchName { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Grade
    {
        public Int64 pkID { get; set; }
        public string GradeName { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Shade
    {
        public Int64 pkID { get; set; }
        public string ShadeName { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Batch
    {
        public Int64 pkID { get; set; }
        public string BatchName { get; set; }
        public string LoginUserID { get; set; }
    }

    public class GSTR
    {
        public string Description { get; set; }
        public Int64 NOE { get; set; }
        public Decimal BasicAmount { get; set; }
        public Decimal IGSTAmt { get; set; }
        public Decimal CGSTAmt { get; set; }
        public Decimal SGSTAmt { get; set; }
        public Decimal TaxAmt { get; set; }
        public Decimal InvoiceAmt { get; set; }

        public string LoginUserID { get; set; }

        public string Module { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }

    }

    public class ProductAssemblyStock
    {
        public Int64 FinishProductID { get; set; }
        public string FinishProductName { get; set; }
        public Int64 AssemblyID { get; set; }
        public string AssemblyName { get; set; }

        public decimal AssemblyQty { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal ClosingSTK { get; set; }

        public decimal BalanceQty { get; set; }
        public string BalanceStatus { get; set; }
    }

    //use in quotation for khodiyar
    public class ProductPartDetail
    {
        public Int64 pkID { get; set; }
        public string QuotationNo { get; set; }
        public string PartDescription { get; set; }
        public string BrandName { get; set; }
        public string ItemOrder { get; set; }
        public Int64 FinishProductID { get; set; }
        public string FinishProductName { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

}
