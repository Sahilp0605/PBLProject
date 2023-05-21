using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Complaint
    {
        public Int64 pkID { get; set; }
        public string ComplaintNo { get; set; }
        public DateTime ComplaintDate { get; set; }
        public Int64 ComplaintDays { get; set; }
        public string ComplaintNoString { get; set; }
        public string ReferenceNo { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SrNo { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }

        public string ComplaintNotes { get; set; }
        public string ComplaintStatus { get; set; }
        public string ComplaintType { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public Nullable<DateTime> PreferredDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string ComplaintDateString { get; set; }

        public string CustmoreMobileNo { get; set; }
        public string SiteMobileNo { get; set; }
        public string Designation { get; set; }
        public string EmailId { get; set; }
        public string WorkOderNo { get; set; }
        public  DateTime DateOfPurchase { get; set; }
        public string PanelSRNo { get; set; }
        public string ProductSRNo { get; set; }
        public string SiteAddress { get; set; }
        public Int64 CityCode { get; set; }
        public Int64 StateCode { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string Pincode { get; set; }
        public string SiteCoordinatorName { get; set; }
        public string ConvinientTimeSlot { get; set; }
        public DateTime ConvinientDate { get; set; }
        public string PhotoOfDefectProduct { get; set; }
        public string PhotoOfPanel { get; set; }
        public string CustomerEmpName { get; set; }

        public string CreatedByEmployee { get; set; }
        public string LoginUserID { get; set; }
        public Int64 ContactNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ClosingRemarks { get; set; }
        public Nullable<DateTime> ScheduleDate { get; set; }
        public string NameOfCustomer { get; set; }

    }

    public class ComplaintVisit
    {
        public Int64 pkID { get; set; }
        public Int64 ComplaintNo { get; set; }
        public DateTime ComplaintDate { get; set; }
        public String ComplaintNoString { get; set; }
        public Nullable<DateTime> PreferredDate { get; set; }
        public string PreferredTimeFrom { get; set; }
        public string PreferredTimeTo { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Nullable<DateTime> VisitDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string VisitType { get; set; }
        public string VisitNotes { get; set; }
        public decimal VisitCharge { get; set; }
        public string VisitChargeType { get; set; }
        public string ComplaintStatus { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public String VisitDocument { get; set; }

        public String VisitDocument1 { get; set; }
        public Int64 ContactNo1 { get; set; }
        public string PanelSRNo { get; set; }
        public string ProductSRNo { get; set; }
        public string SiteCondition { get; set; }
        public string FaultByService { get; set; }
        public string ActionTaken { get; set; }
        public string FurtherAction { get; set; }
        public Int64 NatureOfCall { get; set; }
        public string NatureOfCallName { get; set; }
        public Nullable<DateTime> CloseDate { get; set; }
        public String PanelPhoto { get; set; }
        public String PhotoAfterAction { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedByEmployee { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ComplaintNotes { get; set; }
        public string SrNo { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string NameOfCustomer { get; set; }

        //public string NewRep { get; set; }
        //public string ProductName { get; set; }
        //public string SrNo { get; set; }
        //public string ReplaceProduct { get; set; }
        //public string NewSrNo { get; set; }
        //public string Remarks { get; set; }

    }
    public class Complaint_Report
    {
        public Int64 pkID { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string ComplaintNoString { get; set; }
        public string ReferenceNo { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }

        public string ComplaintNotes { get; set; }
        public string ComplaintStatus { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public DateTime PreferredDate { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public DateTime VisitDate { get; set; }
        public string VisitFromtime { get; set; }
        public string VisitTotime { get; set; }
        public string VisitType { get; set; }
        public string VisitNotes { get; set; }
        public decimal VisitCharge { get; set; }
        public string VisitChargeType { get; set; }

    }
}
