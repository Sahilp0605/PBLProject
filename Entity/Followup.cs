using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Followup
    {
        public Int64 pkID { get; set; }
        public DateTime FollowupDate { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPhone { get; set; }

        public string InquiryNo { get; set; }
        public DateTime InquiryDate { get; set; }
        public string ReferenceName { get; set; }
        public string InquirySource { get; set; }

        public Int64 InquiryStatusID { get; set; }
        public string InquiryStatus { get; set; }

        public string QuotationNo { get; set; }
        public string FollowUpSource { get; set; }
        public string MeetingNotes { get; set; }
        public DateTime NextFollowupDate { get; set; }
        public string PreferredTime { get; set; }
        public Int64 Rating { get; set; }

        public Boolean NoFollowup { get; set; }

        public Int64 NoFollClosureID { get; set; }
        public string NoFollClosureName { get; set; }

        public string LeadStatus { get; set; }

        public string LoginUserID { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string InquiryNoStatus { get; set; }

        public Int64 ExtpkID { get; set; }
        public Int64 FollowupPriority { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public string CityName { get; set; }
    }
}
