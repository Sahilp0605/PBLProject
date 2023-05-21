using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class InquiryStatus
    {
        public Int64 pkID { get; set; }
        public string InquiryStatusName { get; set; }
        public string StatusCategory { get; set; }
        public string LoginUserID { get; set; }
    }
}
