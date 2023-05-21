using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SiteSurvay
    {
        public Int64 pkID { get; set; }
        public String DocNo { get; set; }
        public String SheetNo { get; set; }
        public DateTime SurvayDate { get; set; }
        public Int64 CustID { get; set; }
        public String Customer { get; set; }
        public String ContPerson1 { get; set; }
        public String ContNo1 { get; set; }
        public String ContAddress1 { get; set; }
        public String ContEmail1 { get; set; }
        public String ContDesignation1 { get; set; }
        public String ContPerson2 { get; set; }
        public String ContNo2 { get; set; }
        public String ContAddress2 { get; set; }
        public String ContEmail2 { get; set; }
        public String ContDesignation2 { get; set; }
        public String SiteAddress { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Altitude { get; set; }
        public String NearByRailwayStation { get; set; }
        public String NearByAirport { get; set; }
        public String WaterAndElectricity { get; set; }
        public String RoofTopRCCLocation { get; set; }
        public String RoofTopMetalSheetLocation { get; set; }
        public String GroundMountLocation { get; set; }
        public String StructureType { get; set; }
        public Decimal RoofTopRCCTiltAngle { get; set; }
        public Decimal RoofTopMetalSheetTiltAngle { get; set; }
        public Decimal GroundMountTiltAngle { get; set; }
        public Decimal RoofTopRCCArea { get; set; }
        public Decimal RoofTopMetalSheetArea { get; set; }
        public Decimal GroundMountArea { get; set; }
        public String RoofTopRCCOrientation { get; set; }
        public String RoofTopMetalSheetOrientation { get; set; }
        public String GroundMountOrientation { get; set; }
        public String PenetrationAllowed { get; set; }
        public String OnGridDGRating { get; set; }
        public String OffGridDGRating { get; set; }
        public String HybridDGRating { get; set; }
        public String OnGridContractDemand { get; set; }
        public String OffGridContractDemand { get; set; }
        public String HybridContractDemand { get; set; }
        public Decimal OnGridCapacity { get; set; }
        public Decimal OffGridCapacity { get; set; }
        public Decimal HybridCapacity { get; set; }
        public String InstalationType { get; set; }
        public String DGSynchronisation { get; set; }
        public String DGOperationMode { get; set; }
        public String DataMonitoring { get; set; }
        public String WeatherMonitoringSystem { get; set; }
        public String AvailableBreaker { get; set; }
        public String BusBarTypeAndSize { get; set; }
        public Decimal KVARating { get; set; }
        public Decimal PrimaryVolt { get; set; }
        public Decimal SecondaryVolt { get; set; }
        public Decimal Impedance { get; set; }
        public String VectorGrp { get; set; }
        public String OMRequirements { get; set; }
        public String ModuleCleaningRequirements { get; set; }
        public String RoofPlan { get; set; }
        public String LoadDetails { get; set; }
        public String EarthResistivity { get; set; }
        public String EarthPit { get; set; }
        public String DistanceFromElectricalRoom { get; set; }
        public String SheetType { get; set; }
        public String PurlinDistance { get; set; }
        public String RoofSheet { get; set; }
        public String StructureStability { get; set; }
        public String Skylight { get; set; }
        public String LadderToRoof { get; set; }
        public String SoilTest { get; set; }
        public String ContourSurvey { get; set; }
        public String Tilt { get; set; }
        public String Inverter { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String CreatedEmployeeName { get; set; }
        public String UpdatedEmployeeName { get; set; }
        public String LoginUserID { get; set; }

    }
    public class SiteSurvayDocuments
    {
        public Int64 pkID { get; set; }
        public String DocNo { get; set; }
        public String FileName { get; set; }
        public String FileType { get; set; }
        public String data { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
