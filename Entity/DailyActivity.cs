using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class DailyActivity
    {
        public Int64 pkID { get; set; }
        public DateTime ActivityDate { get; set; }

        public string TaskDescription { get; set; }
        
        public Int64 TaskCategoryID { get; set; }
        public string TaskCategoryName { get; set; }

        public Decimal TaskDuration { get; set; }

        public Int64 ToDOID { get; set; }
        public string ToDODescription { get; set; }
        public string CustomerName { get; set; }
        public String CreatedEmployeeName { get; set; }
        
        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class TaskCategory
    {
        public Int64 pkID { get; set; }
        public string TaskCategoryName { get; set; }
    }
}
