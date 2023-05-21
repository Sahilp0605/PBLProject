using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class JobCardInward
    {

        public Int64 pkID { get; set; }

        public string InwardNo { get; set; }
        public DateTime InwardDate { get; set; }

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

        //public string QuotationNo { get; set; }
        //public string ApprovalStatus { get; set; }

        //public string TermsCondition { get; set; }

        //public Int64 EmployeeID { get; set; }
        //public string EmployeeName { get; set; }

        public decimal BasicAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InwardAmount { get; set; }

        //public Boolean ActiveFlag { get; set; }
        //public string ActiveFlagDesc { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
    public class JobCardInwardDetail
    {
        public Int64 pkID { get; set; }
        public string InwardNo { get; set; }
        public DateTime InwardDate { get; set; }

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
        public string Unit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal NetRate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string OutwardNo { get; set; }
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
}
