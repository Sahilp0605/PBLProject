using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ExpenseType
    {
         public Int64  pkID	{ get; set; }
         public String ExpenseTypeName	{ get; set; }
         public Boolean IsLocationRequired { get; set; }
         public string LoginUserID { get; set; }
    }
}
 