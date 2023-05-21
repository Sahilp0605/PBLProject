using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Holiday
    {
        public Int64 pkID { get; set; }
        public Int64 Holiday_Year { get; set; }
        public string Holiday_Type { get; set; }
        public string Holiday_Name { get; set; }
        public DateTime Holiday_Date { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int64 TotalHolidays { get; set; }
        public string Holiday_Description { get; set; }
        public string imageurl { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
