using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ExternalLeads
    {
        public Int64 pkID { get; set; }
        public string LeadID { get; set; }
        public string LeadSource { get; set; }
        public string ACID { get; set; }
        public string SenderName { get; set; }
        public string SenderMail { get; set; }
        public DateTime QueryDatetime { get; set; }
        public string CompanyName { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public string ForProduct { get; set; }
        public string PrimaryMobileNo { get; set; }
        public string SecondaryMobileNo { get; set; }
        public string CountryFlagURL { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string LeadStatus { get; set; }
        public string CustomerName { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public Int64 StateCode { get; set; }
        public Int64 CityCode { get; set; }
        public string CountryISO { get; set; }
        public string CountryCode { get; set; }
        public string LoginUserID { get; set; }
        public string InquiryNo { get; set; }
        public Int64 InquiryNopkID { get; set; }

        public Int64 ExLeadClosure { get; set; }
        public string ExLeadCloserReason { get; set; }
        public string InquiryStatus { get; set; }

        public string FollowupNotes { get; set; }
        public DateTime FollowupDate { get; set; }
        public string PreferredTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DisqualifedRemarks { get; set; }
    }

    public class ExternalLeads_Report
    {
        public Int64 pkID { get; set; }
        public string LeadID { get; set; }
        public string LeadSource { get; set; }
        public string ActiveType { get; set; }
        public string ACID { get; set; }
        public string SenderName { get; set; }
        public string SenderMail { get; set; }
        public DateTime QueryDatetime { get; set; }
        public string CompanyName { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string State { get; set; }
        public string ForProduct { get; set; }
        public string PrimaryMobileNo { get; set; }
        public string SecondaryMobileNo { get; set; }
        public string CountryFlagURL { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string LeadStatus { get; set; }
        public string CustomerName { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public Int64 StateCode { get; set; }
        public Int64 CityCode { get; set; }
        public string LoginUserID { get; set; }
        public string InquiryNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime FollowupCreatedDate { get; set; }
        public string FollowupCreatedBy { get; set; }
        public string ExLeadClosureStatus { get; set; }
        public Int64 ExLeadClosure { get; set; }
        public Int64 FollowupTypeID { get; set; }
        public string FollowupType { get; set; }
    }
    public class ExternalLeadsRegion
    {
        public string LoginUserID { get; set; }
        public Int64 pkID { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public string CountryCode { get; set; }
        public string Country { get; set; }
        public Int64 StateCode { get; set; }
        public string State { get; set; }
        public Int64 CityCode { get; set; }
        public string City { get; set; }

        public bool IsActive { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string  UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Int64 noofcity { get; set; }
        public string CityList { get; set; }
    }
}
