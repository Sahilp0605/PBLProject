using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class PurchaseBill
    {
        public Int64 pkID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Int64 FixedLedgerID { get; set; }
        public string FixedLedgerName { get; set; }
        public string DocRefNoList { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string GSTNO { get; set; }
        public Int64 BankID { get; set; }
        public Int64 TerminationOfDeliery { get; set; }
        public String ForCoustmerID { get; set; }
        public String TermsCondition { get; set; }
        public decimal BasicAmt { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal ROffAmt { get; set; }

        public String ChargeName1 { get; set; }
        public String ChargeName2 { get; set; }
        public String ChargeName3 { get; set; }
        public String ChargeName4 { get; set; }
        public String ChargeName5 { get; set; }

        public Int64 ChargeID1 { get; set; }
        public decimal ChargeAmt1 { get; set; }
        public decimal ChargeBasicAmt1 { get; set; }
        public decimal ChargeGSTAmt1 { get; set; }

        public Int64 ChargeID2 { get; set; }
        public decimal ChargeAmt2 { get; set; }
        public decimal ChargeBasicAmt2 { get; set; }
        public decimal ChargeGSTAmt2 { get; set; }

        public Int64 ChargeID3 { get; set; }
        public decimal ChargeAmt3 { get; set; }
        public decimal ChargeBasicAmt3 { get; set; }
        public decimal ChargeGSTAmt3 { get; set; }

        public Int64 ChargeID4 { get; set; }
        public decimal ChargeAmt4 { get; set; }
        public decimal ChargeBasicAmt4 { get; set; }
        public decimal ChargeGSTAmt4 { get; set; }

        public Int64 ChargeID5 { get; set; }
        public decimal ChargeAmt5 { get; set; }
        public decimal ChargeBasicAmt5 { get; set; }
        public decimal ChargeGSTAmt5 { get; set; }

        public decimal NetAmt { get; set; }

        public decimal TaxAmt { get; set; }

        public string ModeOfTransport { get; set; }
        public string TransporterName { get; set; }
        public string VehicleNo { get; set; }
        public string LRNo { get; set; }
        public DateTime LRDate { get; set; }
        public string TransportRemark { get; set; }

        public string CreatedEmployeeName { get; set; }


        //----------------------------- BAnk details--------------
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIFSC { get; set; }
        public string BankSwift { get; set; }
        //-----------------------------------------------------

        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Int64 CRDays { get; set; }
        public DateTime DueDate { get; set; }

        public string EmployeeName { get; set; }
        public Decimal BillAmount { get; set; }
        public Int64 CompanyID { get; set; }
        public String BillNo { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }
    }
}
