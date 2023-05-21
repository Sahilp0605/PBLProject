using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class QuotationDetail
    {
        public Int64 pkID { get; set; }        
        public string QuotationNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string QuotationHeader { get; set; }
        public string QuotationFooter { get; set; }
        public string DocRefNo { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }

        public Int64 ProductID { get; set; }
        public Int64 FinishProductID { get; set; }
        public string ProductName { get; set; }
        public string FinishProductName { get; set; }
        public string ProductNameLong { get; set; }
        public string ProductSpecification { get; set; }

        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public Int64 BundleId { get; set; }
        public string BundleName { get; set; }
        
        public string HSNCode { get; set; }
        public decimal UnitQty { get; set; }
        public decimal AssQty { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetRate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal HeaderDiscAmt { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        /////////////////////////////////////////////////////////////

        public decimal DiscountAmt { get; set; }
        public decimal SGSTPer { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmt { get; set; }
        public int TaxType { get; set; }
        public Boolean SubsidyApplicable { get; set; }
        public string Flag { get; set; }

    }

    public class QuotationSubsidy
    {
        public Int64 pkID { get; set; }
        public string Type { get; set; }
        public string QuotationNo { get; set; }
        public Int64 ProductID { get; set; }
        public decimal SlabQty { get; set; }
        public decimal SlabPer { get; set; }
        public decimal Quantity { get; set; }
        public decimal SubsidyPer { get; set; }
        public decimal SubsidyAmt { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class QuotationSubsidySummary
    {        
        public Int64 ProductID { get; set; }
        public decimal Slab { get; set; }
        public decimal Quantity { get; set; }
        public decimal SubsidyPer { get; set; }
        public decimal SubsidyAmt { get; set; }
        public decimal QuantityRem { get; set; }
        public decimal SubsidyPer1 { get; set; }
        public decimal SubsidyAmt1 { get; set; }
        public decimal TotalSubsidyAmt { get; set; }
    }
}
