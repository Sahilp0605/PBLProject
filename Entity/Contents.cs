using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Contents
    {
        public Int64 pkID { get; set; }
        public string Category { get; set; }
        public string TNC_Header { get; set; }
        public string TNC_Content { get; set; }
        public string LoginUserID { get; set; }
    }
}
