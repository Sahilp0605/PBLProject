using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class JobCard
    {
        public Int64 pkID { get; set; }
        public String JobCardNo { get; set; }
        public DateTime JobCardDate { get; set; }
        public DateTime Date { get; set; }
        public string CollectedFrom { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo1 { get; set;}
        public string ContactNo2 { get; set; }
        public decimal JobCardAmount { get; set; }

        public DateTime DateIn { get; set; }
        public DateTime DateReturn { get; set; }
        public string WheelNo { get; set; }
        public string InvoiceNo { get; set; }
        public string DeliveryNoteNo { get; set; }
        public string Tyre { get; set; }
        public string Cap { get; set; }
        public string Sensor { get; set; }
        public string SensorValue { get; set; }
        public string Remarks { get; set; }
        public string PartNumber { get; set; }
        public string WheelMake { get; set; }
        public string StraightenedMeasurement { get; set; }
        public string ClaimNo { get; set; }
        public string RegNo { get; set; }
        public string ChassisNo { get; set; }
        public string PaintCode { get; set; }
        public string Comment { get; set; }
        public string DiamondCut { get; set; }
        public DateTime StartDate { get; set; }
        public string QualityCheck { get; set; }
        public string EstimatePrice { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string BuyerRef { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class JobCardDetail
    {
        public Int64 pkID { get; set; }
        public String JobCardNo { get; set; }
        public DateTime JobCardDate { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitRate { get; set; }
        public decimal Amount { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
