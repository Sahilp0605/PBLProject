using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OrgDepartment
    {
        public string OrgDepartmentCode { get; set; }
        public string OrgDepartmentName { get; set; }
        public string OrgShortName { get; set; }

        public string OrgAddress { get; set; }
        public string OrgCity { get; set; }
        public string OrgPincode { get; set; }
        public string OrgState { get; set; }
        public string OrgLandline { get; set; }
        public string OrgFax { get; set; }
        public string OrgEmailAddress { get; set; }
        
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
