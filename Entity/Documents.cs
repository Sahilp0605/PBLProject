using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Documents
    {
        public Int64 pkID { get; set; }
        public String KeyValue { get; set; }
        public Int64 ProductID { get; set; }
        public String FileName { get; set; }
        public String FileType { get; set; }
        public String FileData { get; set; }
        public byte[] FileBytes { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class SalesorderDocuments
    {
        public Int64 pkID { get; set; }
        public Int64 LogID { get; set; }
        public String OrderNo { get; set; }
        public String AttachmentFile { get; set; }
        public String LoginUserID { get; set; }
        public String CreatedByEmployee { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ModuleDocuments
    {
        public Int64 pkID { get; set; }
        public String ModuleName { get; set; }
        public String KeyValue { get; set; }
        public String DocName { get; set; }
        public String DocType { get; set; }
        public string DocData { get; set; }
        public String LoginUserID { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
