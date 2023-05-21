using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Vehicle
    {
        public Int64 pkID { get; set; }
        public string RegistrationNo { get; set; }

        public string ChasisNo { get; set; }
        public string Mfg { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string VehicleType { get; set; }
        public string MfgYear { get; set; }
        public string EngineCC { get; set; }
        public string InsurancePolicyNo { get; set; }
        public string InsuranceCompany { get; set; }
        public DateTime InsuranceExpiry { get; set; }

        public string MacID { get; set; }
        public string SimID { get; set; }
        
        public Int64 OwnerCode { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerLandline { get; set; }
        public string OwnerMobile { get; set; }

        public decimal RatePerKM { get; set; }
        public decimal Gross_Weight { get; set; }
        public decimal Tare_Weight { get; set; }
        public decimal Net_Weight { get; set; }
        public String LicenseNo { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }


        public DateTime TripDate { get; set; }
        public string DriverName { get; set; }
        public string From_Station { get; set; }
        public string To_Station { get; set; }
        public Decimal Kilometers { get; set; }
    }

    public class VehicleTrip
    {
        public Int64 pkID { get; set; }
        public DateTime TripDate { get; set; }
        public Int64 VehicleID { get; set; }
        public string RegistrationNo { get; set; }
        public string DriverName { get; set; }
        public string From_Station { get; set; }
        public string To_Station { get; set; }
        public Int64 Reading1 { get; set; }
        public Int64 Reading2 { get; set; }
        public Int64 Kilometers { get; set; }
        public decimal DieselCharge { get; set; }
        public decimal Amount { get; set; }
        public decimal TripCost { get; set; }
        public decimal Toll { get; set; }
        public decimal Bhatthu { get; set; }
        public decimal DriverAllowance { get; set; }
        public string Remarks { get; set; }
        public decimal AvgKmPerLtr { get; set; }
        public decimal FuelCostPerKM { get; set; }
        public decimal RateOfKMAllowance { get; set; }
        public decimal DriwingAllowance { get; set; }
        public decimal Salary { get; set; }
        public decimal TotalExpense { get; set; }
        
        //Newly Added-----------------------------------
        public decimal InsuranceAmount { get; set; }
        public decimal InsurancePerTrip { get; set; }
        public decimal GovernmentTax { get; set; }
        public decimal ExplosiveTax { get; set; }
        public decimal VehicleAmount { get; set; }
        public decimal DepreciationPerDay { get; set; }
        public decimal WeightKgPerCylinderQty { get; set; }
        public string MaterialName { get; set; }
        //-----------------------------------------------------
        public Int64 EmployeeID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
