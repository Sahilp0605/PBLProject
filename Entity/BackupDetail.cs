using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class BackupDetail
    {
        public string Database { get; set; }
        public string DirPath { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Date { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
