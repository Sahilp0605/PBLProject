using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CompanyProfile
    {
        public Int64 CompanyID { get; set; }
        public Int64 BankID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }

        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string CINNo { get; set; }

        public Int64 ParentCompanyID { get; set; }
        public string ParentCompanyName { get; set; }

        public string Address { get; set; }
        public string Area { get; set; }
        public string Pincode { get; set; }
        public Int64 CityCode { get; set; }
        public string CityName { get; set; }
        public Int64 StateCode { get; set; }
        public string StateName { get; set; }

        public string chkCustomer { get; set; }
        public string chkInquiry { get; set; }
        public string chkQuotation { get; set; }
        public string chkSalesOrder { get; set; }
        public string chkLeaveRequest { get; set; }
        public string chkFeedback { get; set; }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string BranchName { get; set; }
        public string BankIFSC { get; set; }
        
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public Int64 pkID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ConstantHead { get; set; }
        public string ConstantValue { get; set; }
        public string Host { get; set; }
        public Boolean EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Int64 PortNumber { get; set; }
        public Int64 DisplayOrder { get; set; }
        public string eSignaturePath { get; set; }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }
    }

    public class Company
    {
        public static int CompanyId { get; set; }
        public static int Mode { get; set; }
    }

}
