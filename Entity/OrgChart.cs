using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OrgChart
    {
        public string id { get; set; }
        public string name { get; set; }
        public string parent { get; set; }
        public string activeflag { get; set; }
    }
}
