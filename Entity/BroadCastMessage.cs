using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class BroadCastMessage
    {
        
        public Int64 pkID { get; set; }
        public string Message{ get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LoginUserID { get; set; }
    }
}
