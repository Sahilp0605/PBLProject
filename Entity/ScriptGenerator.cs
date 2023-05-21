using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ScriptGenerator
    {
        public Int64 pkID { get; set; }
        public String ColName { get; set; }
        public String ColType { get; set; }
        public String ColWidth { get; set; }
        public String ColScale { get; set; }
        public String ColIsNull { get; set; }

    }
}
