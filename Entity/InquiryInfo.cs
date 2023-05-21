using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class InquiryInfo
    {
        public Int64 pkID { get; set; }
        public string TypeOfData { get; set; }
        public string InquiryNo { get; set; }
        public string InquiryNoStatus { get; set; }
        public DateTime InquiryDate { get; set; }
        public string ReferenceName { get; set; }

        public string RefName { get; set; }
        public string RefNo { get; set; }

        public string InquirySource { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Int64 SpecialityID { get; set; }
        public string TreatmentType { get; set; }
        
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set; }
        public string MeetingNotes { get; set; }

        public string FollowupNotes { get; set; }
        public DateTime FollowupDate { get; set; }
        public string PreferredTime { get; set; }

        public DateTime LastFollowupDate { get; set; }
        public DateTime LastNextFollowupDate { get; set; }
        public Boolean NoFollowUp { get; set; }

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

        public Int64 ActivityDays { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByEmployee { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        //*-------------------------------------------------*//
        public string DoctorName { get; set; }
        public Int64 DoctorID { get; set; }
        public decimal Amount { get; set; }
        public Boolean Visited { get; set; }
        public DateTime? AppoinmentDt { get; set; }
        public string SpecialityName { get; set; }
        public Boolean Started { get; set; }    
        public Boolean Finished { get; set; }
        public DateTime? CompletionDt { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal ReceivedAmount { get; set; }
        public string BillNo { get; set; }
        public string OrderNo { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string QuotationNo { get; set; }        
        //----------------------- JJB ---------------------------
        public String Unit { get; set; }
        public String Thickness {get; set;}
        public Decimal Factor { get;  set; }
        public Decimal Area { get; set; }
        public String Remarks { get; set; }
        //*-----------------------------------------------------*//
        public string Priority { get; set; }
        public Int64 ClosureReason { get; set; }
        public String ClosureReasonName { get; set; }
        public Int64 AssignToEmployee { get; set; }
    }

    public class InquiryInfo_Report
    {
        public Int64 pkID { get; set; }
        public string InquiryNo { get; set; }
        public DateTime InquiryDate { get; set; }
        public string ReferenceName { get; set; }
        public string InquirySource { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactNo1 { get; set; }
        public string EmailAddress { get; set; }
        public string EmployeeName { get; set; }
        public string MeetingNotes { get; set; }
        public string Designation { get; set; }
        public string FollowupNotes { get; set; }
        public DateTime FollowupDate { get; set; }
        public Int64 InquiryStatusID { get; set; }
        public string InquiryStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string ProductGroupName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        
    }

    public class InquiryClinic_Report
    {
        public string InquiryNo { get; set; }
        public DateTime InquiryDate { get; set; }
        public string TreatmentType { get; set; }
        public string CustomerName { get; set; }
        public string InquiryStatus { get; set; }
        public string InquirySource { get; set; }
        public string ReferenceName { get; set; }
        public string SpecialityName { get; set; }
        public string MeetingNotes { get; set; }
        public string HospitalName { get; set; }
        public string ContactPerson1 { get; set; }
        public decimal ProposedAmount { get; set; }
        public Boolean Visited { get; set; }
        public Boolean Started { get; set; }
        public Boolean Finished { get; set; }
        public DateTime CompletionDt { get; set; }
        public DateTime AppoinmentDt { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string WalletName { get; set; }
    }

    public class CRMSummary
    {
        //Section : CRM Summary
        public Int64 TotalLeads { get; set; }
        public Int64 TotalQuoatation { get; set; }
        public Int64 TotalSalesOrder { get; set; }
        public Int64 TotalSalesBill { get; set; }
        public Int64 TotalQuoatationAmt { get; set; }
        public Int64 TotalSalesOrderAmt { get; set; }
        public Int64 TotalSalesBillAmt { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal BasicAmt { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string CreatedByEmployee { get; set; }

        //Section : Daily Summary
        public Int64 DueDispatch { get; set; }
        public Int64 DuePurchase { get; set; }
        public Int64 DuePayable { get; set; }
        public Int64 DueReceivable { get; set; }
        public Int64 AppSalesOrder { get; set; }
        public Int64 AppPurchaseOrder { get; set; }
        public Int64 PayDuePayable { get; set; }
        public Int64 PayDueReceivable { get; set; }
        public Int64 DuePurchasePaySch { get; set; }
        public Int64 DueSalesPaySch { get; set; }

        public Int64 DocInquiry { get; set; }
        public Int64 DocQuotation { get; set; }
        public Int64 DocSalesOrder { get; set; }

        // ----------------------------------------------------------
        public string ByHead { get; set; }
        public Int64 Deals { get; set; }
        public Int64 WonDeal { get; set; }
        public Int64 LostDeal { get; set; }
        public Int64 OpenDeal { get; set; }

        public Decimal Conversion { get; set; }
        public Decimal Contribution { get; set; }

        public Decimal TotalRevenue { get; set; }
        public Decimal WonRevenue { get; set; }
        public Decimal LostRevenue { get; set; }


    }
}
