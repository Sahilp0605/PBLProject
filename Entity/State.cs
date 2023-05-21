using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class State
    {
        public Int64 StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Int64 GSTStateCode { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
       
    }
}
