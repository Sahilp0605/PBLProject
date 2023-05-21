using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SalesTarget
    {
        public Int64 pkID { get; set; }
        public Int64 EmployeeID { get; set; }
        public DateTime FromDate{ get; set; }
        public DateTime ToDate { get; set; }
        public decimal TargetAmount { get; set; }
        public Int64 BrandID { get; set; }
        public Int64 ProductGroupID { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string LoginUserID { get; set; }
        public string EmployeeName { get; set; }
        public string TargetType { get; set; }
        public string BrandName { get; set; }
        public string ProductGroupName { get; set; }
        public decimal AchievedAmount { get; set; }
        public decimal Percentage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
