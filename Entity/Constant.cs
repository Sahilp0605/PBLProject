using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Constant
    {
        public Int64 pkID { get; set; }
        public Int64 CompanyID { get; set; }
        public string ConstantStyle { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ConstantHead { get; set; }
        public string ConstantValue { get; set; }
        public Int64 DisplayOrder { get; set; }
               
        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
