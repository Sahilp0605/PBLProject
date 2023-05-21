using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CustomerContacts
    {
        public Int64 pkID { get; set; }
        public Int64 CustomerID { get; set; }

        public string ContactPerson1 { get; set; }
        public string ContactDesigCode1 { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactEmail1 { get; set; }

        public string ContactPerson2 { get; set; }
        public string ContactDesigCode2 { get; set; }
        public string ContactNumber2 { get; set; }
        public string ContactEmail2 { get; set; }

        public string ContactPerson3{ get; set; }
        public string ContactDesigCode3 { get; set; }
        public string ContactNumber3 { get; set; }
        public string ContactEmail3 { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
