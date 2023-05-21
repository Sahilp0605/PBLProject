using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class JobCardOutward
    {
        public Int64 pkID { get; set; }

        public string OutwardNo { get; set; }
        public DateTime OutwardDate { get; set; }
        public string OrderNo { get; set; }
        public Int64 OrderStatusID { get; set; }
        public string OrderStatus { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }

        public string CreatedEmployeeName { get; set; }

        public decimal BasicAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal OutwardAmount { get; set; }
        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }
        public Nullable<DateTime> LRDate { get; set; }
        public string DCNo { get; set; }
        public Nullable<DateTime> DCDate { get; set; }
        public string DeliveryNote { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class JobCardOutwardDetail
    {
        public Int64 pkID { get; set; }
        public string OutwardNo { get; set; }
        public DateTime OutwardDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }
        public string ProductSpecification { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityWeight { get; set; }
        public string SerialNo { get; set; }
        public string BoxNo { get; set; }

        public string Unit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetRate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class JobCardOutwardDetailAssembly
    {
        public Int64 pkID { get; set; }
        public string OutwardNo { get; set; }
        public DateTime OutwardDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public Int64 AssemblyID { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblySpecification { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityWeight { get; set; }
        public string SerialNo { get; set; }
        public string BoxNo { get; set; }

        public string Unit { get; set; }

        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
