using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Feedback
    {
        public Int64  pkID { get; set; }
	    public string CustomerId { get; set; }
	    public Int64 Product_Satisfaction { get; set; }
	    public Int64 SalesExecutive_Presentation { get; set; }
	    public Int64 Product_Features { get; set; }
	    public Int64 Product_Presentation { get; set; }
        public Int64 Recommendyn { get; set; }
	    public string Comment { get; set; }


        public string Questions { get; set; }
    }
}
