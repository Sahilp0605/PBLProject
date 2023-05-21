using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OrganizationEmployee
    {
        public Int64 pkID { get; set; }
        public String Prefix { get; set; }
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String EmployeeName { get; set; }
        public String Landline { get; set; }
        public String MobileNo { get; set; }
        public String EmailAddress { get; set; }
        public String EmailPassword { get; set; }
        
        public Int64 ShiftCode { get; set; }
        public String ShiftName { get; set; }
        public string BasicPer { get; set; }
        public decimal FixedBasic { get; set; }
        public decimal FixedHRA { get; set; }
        public decimal FixedConv { get; set; }
        public decimal FixedDA { get; set; }
        public decimal FixedSpecial { get; set; }

        public string Gender { get; set; }
        public string WorkingHours { get; set; }
        public String DesigCode { get; set; }
        public String Designation { get; set; }
        public String OrgCode { get; set; }
        public String OrgName { get; set; }
        public Int64 ReportTo { get; set; }
        public String ReportToEmployeeName { get; set; }
        public Int64 CardNo { get; set; }

        public Decimal FixedSalary { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ReleaseDate { get; set; }

        public String AuthorizedSign { get; set; }
        public string EmployeeImage { get; set; }

        public string BankName      { get; set; }
        public string BankBranch    { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIFSC      { get; set; }

        public Int64 CompanyID { get; set; }
        public String CompanyName { get; set; }

        public Boolean ActiveFlag { get; set; }
        public String ActiveFlagDesc { get; set; }

        public string CampaignSentOn { get; set; }  //for Campaign 

        public String DrivingLicenseNo { get; set; }
        public String PassportNo { get; set; }
        public String AadharCardNo { get; set; }
        public String PANCardNo { get; set; }
        public String EmpCode { get; set; }
        public Boolean PF_Calculation { get; set; }
        public Boolean PT_Calculation { get; set; }
        public string eSignaturePath { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public String LoginUserID { get; set; }
        
        public Int64 RefEmployeeID { get; set; }
        public String Description { get; set; }
        public String UserID { get; set; }
        public String UserPassword { get; set; }

        public Int64 Inquiry { get; set; }
        public Int64 Quotation { get; set; }
        public Int64 Followup { get; set; }
        public Int64 SalesOrder { get; set; }
        public Int64 SalesBill { get; set; }
        public Int64 PurcBill { get; set; }
        public Int64 Customers { get; set; }
        public Int64 Follower { get; set; }

        public string TokenNo { get; set; }

        public DateTime TransDate { get; set; }
        public string TransCategory { get; set; }
        public string TransType { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ClosingAmount { get; set; }

        public string Address { get; set; }
        public string Area { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Pincode { get; set; }
        public string MaritalStatus { get; set; }
        public string BloodGroup { get; set; }
        public string ESICNo { get; set; }
        public string PFAccountNo { get; set; }

    }

    public class ShiftMaster
    {
        public Int64 ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Int64 GraceMins { get; set; }

        public decimal MinHrsHalfDay { get; set; }

        public decimal MinHrsFullDay { get; set; }
        public string LunchFrom { get; set; }
        public string LunchTo { get; set; }

        public string LoginUserID { get; set; }
    }
}
