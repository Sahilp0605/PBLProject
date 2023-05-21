using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OrganizationBank
    {
        public Int64 pkID { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }

        public string CompanyID { get; set; }
        public string CompanyName { get; set; }

        public string BankName { get; set; }
        public string BankAccountNo { get; set; }
        public string BranchName { get; set; }
        public string BankIFSC { get; set; }
        public string BankSWIFT { get; set; }
        public string GSTNo { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
