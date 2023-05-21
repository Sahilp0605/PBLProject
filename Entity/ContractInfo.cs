using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ContractInfo
    {
        public Int64 pkID { get; set; }
        public string InquiryNo { get; set; }
        public string InquiryNoStatus { get; set; }
        public string ContractType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }

        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }

        public string ContractFooter { get; set; }
        public string ContractTNC { get; set; }

        public DateTime LastFollowupDate { get; set; }
        public DateTime LastNextFollowupDate { get; set; }

        public Int64 InquiryStatusID { get; set; }
        public string InquiryStatus { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }

        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }

        public decimal TotalAmount { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }

        public string LoginUserID { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public string Designation { get; set; }

        public string externalInvoiceNo { get; set; }
        public Decimal externalInvoiceAmount { get; set; }
        public Int64 RenewDays { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
