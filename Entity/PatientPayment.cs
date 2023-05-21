using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class PatientPayment
    {
        public Int64 pkID { get; set; }
        public Int64 InquirypkID { get; set; }
        public string InquiryNo { get; set; }
        public DateTime InquiryDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }

        public Int64 HospitalID { get; set; }
        public string HospitalName { get; set; }

        public Int64 DoctorID { get; set; }
        public string DoctorName { get; set; }

        public Boolean Visited { get; set; }
        public DateTime AppoinmentDt { get; set; }
        public Decimal Amount { get; set; }
        public Boolean Started { get; set; }
        public DateTime CompletionDt { get; set; }
        public Decimal FinalAmount { get; set; }
        public Decimal PatientPaid { get; set; }
        public Decimal BilledAmount { get; set; }
        public Decimal ReceivedAmount { get; set; }

        public Int64 PaymentTypeID { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public Decimal PaymentAmount { get; set; }
        public string TreatmentType { get; set; }
        public string BillNo { get; set; }
        public string OrderNo { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
