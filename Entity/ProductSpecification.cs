using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ProductSpecification
    {
        public Int64 pkID { get; set; }
        public string GroupHead { get; set; }
        public string MaterialHead { get; set; }
        public string MaterialSpec { get; set; }
        public string ItemOrder { get; set; }
        public Int64 FinishProductID { get; set; }
        public string MaterialRemarks { get; set; }
        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }

    }
}
