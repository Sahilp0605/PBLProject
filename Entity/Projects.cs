using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Projects
    {
        public Int64 pkID { get; set; }

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class ProjectSheet
    {
        public Int64 pkID { get; set; }
        public String ProjectSheetNo { get; set; }
        public DateTime ProjectSheetDate { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String SiteAddress { get; set; }
        public String SiteNo { get; set; }
        public String SiteArea { get; set; }
        public Int64 SiteCityID { get; set; }
        public String CityName { get; set; }
        public Int64 SiteStateID { get; set; }
        public String StateName { get; set; }
        public String SiteCountryID { get; set; }
        public String CountryName { get; set; }
        public String SitePincode { get; set; }
        public String Remarks { get; set; }
        public String LoginUserID { get; set; }
        public String CreatedEmployeeName { get; set; }
        public String UpdatedEmployeeName { get; set; }
    }
    public class Project_Detail
    {
        public Int64 pkID { get; set; }
        public String ProjectSheetNo { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public String ProductNameLong { get; set; }
        public String Unit { get; set; }
        public Decimal SysCapacity { get; set; }
        public String PanalWattage { get; set; }
        public String LoginUserID { get; set; }

    }

    public class ProjectAssembly
    {
        public Int64 pkID { get; set; }
        public string ProjectSheetNo { get; set; }
        public Int64 FinishedProductID { get; set; }
        public string FinishedProductName { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public string Remarks { get; set; }
        public Int64 ProductMake { get; set; }
        public String ProductMakeName { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

