using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OrganizationStructure
    {
        public Int64 pkID { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public Int64 OrgTypeCode { get; set; }
        public string OrgType { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }

        public string Landline1 { get; set; }
        public string Landline2 { get; set; }

        public string Fax1 { get; set; }
        public string Fax2 { get; set; }

        public string EmailAddress { get; set; }

        public string GSTIN { get; set; }
        public string PANNO { get; set; }
        public string CINNO { get; set; }

        public string ReportTo_OrgCode { get; set; }
        public string ReportTo_OrgName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateCode { get; set; }
        public string GSTStateCode { get; set; }
        public string StateName { get; set; }

        public string OrgDepartmentCode { get; set; }
        public string OrgDepartmentName { get; set; }
        public string OrgShortName { get; set; }
        public string GroupCode { get; set; }
        public string MarkerImage { get; set; }

        // -----------------------------------------------------
        // Below Section is for Employee Tracking
        // -----------------------------------------------------
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string InquiryNo { get; set; }
        public DateTime FollowUpDate { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Distance { get; set; }
        public string SpeedLive { get; set; }

        public string MacID { get; set; }
        public Int64 DeviceID { get; set; }
        // -----------------------------------------------------
        public Boolean ActiveFlag { get; set; }
        public Int64 OrgHead { get; set; }
        public String OrgHeadName { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }
        
        public Int64 HelpLogID { get; set; }

        public string LoginUserID { get; set; }
    }
}
