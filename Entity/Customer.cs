using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Customer
    {
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }

        public bool BlockCustomer { get; set; }

        public string Address { get; set; }
        public string ShipToGSTNo { get; set; }
        public string ShipToCompanyName { get; set; }
        public string Area { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string GSTStateCode { get; set; }
        public string GSTStateCode1 { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Pincode { get; set; }

        public string Address1 { get; set; }
        public string Area1 { get; set; }
        public string CityCode1 { get; set; }
        public string CityName1 { get; set; }
        public string StateCode1 { get; set; }
        public string StateName1 { get; set; }
        public string CountryCode1 { get; set; }
        public string CountryName1 { get; set; }
        public string Pincode1 { get; set; }

        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime AnniversaryDate { get; set; }

        //Added Newly
        public string TinVatNo { get; set; }
        public string TinCstNo { get; set; }

        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }

        public string EmailAddress { get; set; }
        public string WebsiteAddress { get; set; }

        public Int64 OrgTypeCode { get; set; }
        public string OrgTypeName { get; set; }

        public Int64 ParentID { get; set; }
        public string ParentName { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public Decimal OrderAmount { get; set; }

        public Int64 KeyID { get; set; }
        public DateTime TransDate { get; set; }
        public string TransCategory { get; set; }
        public string TransType { get; set; }
        public string Description { get; set; }
        public string DBAccountName { get; set; }
        public string CRAccountName { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ClosingAmount { get; set; }
        public Int64 PriceListID { get; set; }
        public decimal ErpClosing { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedEmployee { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedEmployee { get; set; }

        public string LoginUserID { get; set; }

        public Int64 CustomerSourceID { get; set; }
        public string CustomerSourceName { get; set; }

        public Int64 SpecialityID { get; set; }
        public string TreatmentType { get; set; }
        public bool GenerateInquiry { get; set; }
        public Int64 CR_Days { get; set; }
        public decimal CR_Limit { get; set; }
        public string BlockCustomerStatus { get; set; }//for report 
        public string CampaignSentOn { get; set; }//for Campaign 
        public string Remarks { get; set; }
        public string CreatedEmployeeName { get; set; }
        public string NameOfCustomer { get; set; }

    }

    public class CustomerMasterList
    {
        public Int64 CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }

        public bool BlockCustomer { get; set; }

        public string Address { get; set; }
        public string ShipToGSTNo { get; set; }
        public string ShipToCompanyName { get; set; }
        public string Area { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string GSTStateCode { get; set; }
        public string GSTStateCode1 { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Pincode { get; set; }

        public string Address1 { get; set; }
        public string Area1 { get; set; }
        public string CityCode1 { get; set; }
        public string CityName1 { get; set; }
        public string StateCode1 { get; set; }
        public string StateName1 { get; set; }
        public string CountryCode1 { get; set; }
        public string CountryName1 { get; set; }
        public string Pincode1 { get; set; }

        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string MonthlySales { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime JoinAsDealer { get; set; }

        public string PrimaryContactNo1 { get; set; }
        public string PrimaryContactNo2 { get; set; }

        public string EmailAddress { get; set; }
        public string WebsiteAddress { get; set; }

        public Int64 OrgTypeCode { get; set; }
        public string OrgTypeName { get; set; }

        public Int64 ParentID { get; set; }
        public string ParentName { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public Decimal OrderAmount { get; set; }

        public Int64 KeyID { get; set; }
        public DateTime TransDate { get; set; }
        public string TransCategory { get; set; }
        public string TransType { get; set; }
        public string Description { get; set; }
        //public string Remarks { get; set; }
        public string DBAccountName { get; set; }
        public string CRAccountName { get; set; }
        public decimal OpeningAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal ClosingAmount { get; set; }
        public Int64 PriceListID { get; set; }
        public decimal ErpClosing { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedEmployee { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedEmployee { get; set; }

        public string LoginUserID { get; set; }

        public Int64 CustomerSourceID { get; set; }
        public string CustomerSourceName { get; set; }

        public Int64 SpecialityID { get; set; }
        public string TreatmentType { get; set; }
        public bool GenerateInquiry { get; set; }

        public Int64 CR_Days { get; set; }
        public decimal CR_Limit { get; set; }

        public string BlockCustomerStatus { get; set; }//for report 
        public string CampaignSentOn { get; set; }//for Campaign 


    }

}
