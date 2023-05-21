using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class AudioFiles
    {
        public Int64 pkID { get; set; }
        public String ModuleName { get; set; }
        public String KeyID { get; set; }
        public String FileName { get; set; }
        public String FileType { get; set; }
        public String ContentData { get; set; }
        public String LoginUserID { get; set; }
        public String EmployeeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

