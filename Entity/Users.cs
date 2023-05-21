using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Users
    {
        public Int64 pkID { get; set; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string ScreenFullName { get; set; }
        public string Description { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public Int64 OrgTypeCode { get; set; }
        public string OrgType { get; set; }

        public Int64 EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }

        public Int64 CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
       
        //use below fields for userlog
        public string LoginDateTime { get; set; }
        public string LogoutDateTime { get; set; }
        public string MacID { get; set; }

    }
    public class Users_Report
    {
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string ScreenFullName { get; set; }
        public string Description { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public Int64 OrgTypeCode { get; set; }
        public string OrgType { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

}
