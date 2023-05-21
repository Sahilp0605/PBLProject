using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Inward
    {

        public Int64 pkID { get; set; }

        public string InwardNo { get; set; }
        public DateTime InwardDate { get; set; }
        public string DocRefNoList { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }

        public string CreatedEmployeeName { get; set; }

        //public string QuotationNo { get; set; }
        //public string ApprovalStatus { get; set; }

        //public string TermsCondition { get; set; }

        //public Int64 EmployeeID { get; set; }
        //public string EmployeeName { get; set; }
        public decimal BasicAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal ROffAmt { get; set; }

        public decimal NetAmt { get; set; }

        public decimal TaxAmt { get; set; }

        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }
        public DateTime LRDate { get; set; }
        public string TransportRemark { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InwardAmount { get; set; }

        //public Boolean ActiveFlag { get; set; }
        //public string ActiveFlagDesc { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }

        public string LoginUserID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ManuaLInwardNo { get; set; }

    }
}
