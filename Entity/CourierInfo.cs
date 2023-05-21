using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CourierInfo
    {
        public Int64 pkID { get; set; }
        public string SerialNo { get; set; }
        public string DocketNo { get; set; }
        public DateTime ActivityDate { get; set; }
        public string DocumentType { get; set; }
        public string AcceptanceType { get; set; }

        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }

        public Int64 CourierID { get; set; }
        public string CourierName { get; set; }
        public string CourierEmail { get; set; }
        public string CourierContact { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }

        public string ReceivedBy { get; set; }
        public string Remarks { get; set; }

        public string CourierImage { get; set; }
        public string LoginUserID { get; set; }

        public string EmployeeName { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public String CourierNo { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public String data { get; set; }
    }
}
