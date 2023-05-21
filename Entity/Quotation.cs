using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Quotation
    {
        public Int64 pkID { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string DocRefNoList { get; set; }
        public string InquiryNo { get; set; }
        public Int64 InquirypkID { get; set; }
        public DateTime InquiryDate { get; set; }
        public string ProjectName { get; set; }
        public string CreditDays { get; set; }
        public string QuotationType { get; set; }
        public Int64 BankId { get; set; }
        public String BankName { get; set; }
        public String BankAccountName { get; set; }
        public String BankAccountNo { get; set; }
        public String BranchName { get; set; }
        public String BankIFSC { get; set; }
        public String BankSWIFT{ get; set; }
        public string QuotationSubject { get; set; }
        public string QuotationKindAttn { get; set; }

        public string QuotationHeader { get; set; }
        public string QuotationFooter { get; set; }
        public string AssumptionRemark { get; set; }
        public string AdditionalRemark { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }

        public decimal QuotationAmount { get; set; }
        public string InquiryStatus { get; set; }
        public string CurrencyShortName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }

        public string LoginUserID { get; set; }
        public Int64 CreatedID { get; set; }

        public string CreatedEmployeeName { get; set; }
        public string AuthorizedSign { get; set; }
        public string CreatedEmployeeMobileNo { get; set; }
        public string UpdatedEmployeeName { get; set; }
        public string EmployeeEmailAddress { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }

        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitRate { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetRate { get; set; }
        public decimal NetAmount { get; set; }

        public decimal TotalAmount { get; set; }
        public string Unit { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal Amount { get; set; }

        //////////////////////////////////////////////////////////////
        public decimal BasicAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal ROffAmt { get; set; }

        public String ChargeName1 { get; set; }
        public String ChargeName2 { get; set; }
        public String ChargeName3 { get; set; }
        public String ChargeName4 { get; set; }
        public String ChargeName5 { get; set; }

        public Int64 ChargeID1 { get; set; }
        public decimal ChargeAmt1 { get; set; }
        public decimal ChargeBasicAmt1 { get; set; }
        public decimal ChargeGSTAmt1 { get; set; }

        public Int64 ChargeID2 { get; set; }
        public decimal ChargeAmt2 { get; set; }
        public decimal ChargeBasicAmt2 { get; set; }
        public decimal ChargeGSTAmt2 { get; set; }

        public Int64 ChargeID3 { get; set; }
        public decimal ChargeAmt3 { get; set; }
        public decimal ChargeBasicAmt3 { get; set; }
        public decimal ChargeGSTAmt3 { get; set; }

        public Int64 ChargeID4 { get; set; }
        public decimal ChargeAmt4 { get; set; }
        public decimal ChargeBasicAmt4 { get; set; }
        public decimal ChargeGSTAmt4 { get; set; }

        public Int64 ChargeID5 { get; set; }
        public decimal ChargeAmt5 { get; set; }
        public decimal ChargeBasicAmt5 { get; set; }
        public decimal ChargeGSTAmt5 { get; set; }

        public decimal NetAmt { get; set; }

        //-------------Quatation Log -------------
        public Int64 RowNum { get; set; }
        public Int64 FollowUpID { get; set; }
        public Int64 QuatationID { get; set; }
        public Int64 LogID { get; set; }
        public string Remark { get; set; }
        public string FileName { get; set; }
        public string FromEmployeeName { get; set; }

    }
}
