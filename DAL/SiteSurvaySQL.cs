using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SiteSurvaySQL : BaseSqlManager
    {
        public virtual List<Entity.SiteSurvay> GetSiteSurvay(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurveyList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SiteSurvay> lstObject = new List<Entity.SiteSurvay>();
            while (dr.Read())
            {
                Entity.SiteSurvay objEntity = new Entity.SiteSurvay();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.DocNo = GetTextVale(dr, "DocNo");
                objEntity.SurvayDate = GetDateTime(dr, "SurvayDate");
                objEntity.SheetNo = GetTextVale(dr, "SheetNo");
                objEntity.CustID = GetInt64(dr, "CustID");
                objEntity.Customer = GetTextVale(dr, "Customer");
                objEntity.ContPerson1 = GetTextVale(dr, "ContPerson1");
                objEntity.ContNo1 = GetTextVale(dr, "ContNo1");
                objEntity.ContAddress1 = GetTextVale(dr, "ContAddress1");
                objEntity.ContEmail1 = GetTextVale(dr, "ContEmail1");
                objEntity.ContDesignation1 = GetTextVale(dr, "ContDesignation1");
                objEntity.ContPerson2 = GetTextVale(dr, "ContPerson2");
                objEntity.ContNo2 = GetTextVale(dr, "ContNo2");
                objEntity.ContAddress2 = GetTextVale(dr, "ContAddress2");
                objEntity.ContEmail2 = GetTextVale(dr, "ContEmail2");
                objEntity.ContDesignation2 = GetTextVale(dr, "ContDesignation2");
                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.Latitude = GetDecimal(dr, "Latitude");
                objEntity.Longitude = GetDecimal(dr, "Longitude");
                objEntity.Altitude = GetDecimal(dr, "Altitude");
                objEntity.NearByRailwayStation = GetTextVale(dr, "NearByRailwayStation");
                objEntity.NearByAirport = GetTextVale(dr, "NearByAirport");
                objEntity.WaterAndElectricity = GetTextVale(dr, "WaterAndElectricity");
                objEntity.RoofTopRCCLocation = GetTextVale(dr, "RoofTopRCCLocation");
                objEntity.RoofTopMetalSheetLocation = GetTextVale(dr, "RoofTopMetalSheetLocation");
                objEntity.GroundMountLocation = GetTextVale(dr, "GroundMountLocation");
                objEntity.StructureType = GetTextVale(dr, "StructureType");
                objEntity.RoofTopRCCTiltAngle = GetDecimal(dr, "RoofTopRCCTiltAngle");
                objEntity.RoofTopMetalSheetTiltAngle = GetDecimal(dr, "RoofTopMetalSheetTiltAngle");
                objEntity.GroundMountTiltAngle = GetDecimal(dr, "GroundMountTiltAngle");
                objEntity.RoofTopRCCArea = GetDecimal(dr, "RoofTopRCCArea");
                objEntity.RoofTopMetalSheetArea = GetDecimal(dr, "RoofTopMetalSheetArea");
                objEntity.GroundMountArea = GetDecimal(dr, "GroundMountArea");
                objEntity.RoofTopRCCOrientation = GetTextVale(dr, "RoofTopRCCOrientation");
                objEntity.RoofTopMetalSheetOrientation = GetTextVale(dr, "RoofTopMetalSheetOrientation");
                objEntity.GroundMountOrientation = GetTextVale(dr, "GroundMountOrientation");
                objEntity.PenetrationAllowed = GetTextVale(dr, "PenetrationAllowed");
                objEntity.OnGridDGRating = GetTextVale(dr, "OnGridDGRating");
                objEntity.OffGridDGRating = GetTextVale(dr, "OffGridDGRating");
                objEntity.HybridDGRating = GetTextVale(dr, "HybridDGRating");
                objEntity.OnGridContractDemand = GetTextVale(dr, "OnGridContractDemand");
                objEntity.OffGridContractDemand = GetTextVale(dr, "OffGridContractDemand");
                objEntity.HybridContractDemand = GetTextVale(dr, "HybridContractDemand");
                objEntity.OnGridCapacity = GetDecimal(dr, "OnGridCapacity");
                objEntity.OffGridCapacity = GetDecimal(dr, "OffGridCapacity");
                objEntity.HybridCapacity = GetDecimal(dr, "HybridCapacity");
                objEntity.InstalationType = GetTextVale(dr, "InstalationType");
                objEntity.DGSynchronisation = GetTextVale(dr, "DGSynchronisation");
                objEntity.DGOperationMode = GetTextVale(dr, "DGOperationMode");
                objEntity.DataMonitoring = GetTextVale(dr, "DataMonitoring");
                objEntity.WeatherMonitoringSystem = GetTextVale(dr, "WeatherMonitoringSystem");
                objEntity.AvailableBreaker = GetTextVale(dr, "AvailableBreaker");
                objEntity.BusBarTypeAndSize = GetTextVale(dr, "BusBarTypeAndSize");
                objEntity.KVARating = GetDecimal(dr, "KVARating");
                objEntity.PrimaryVolt = GetDecimal(dr, "PrimaryVolt");
                objEntity.SecondaryVolt = GetDecimal(dr, "SecondaryVolt");
                objEntity.Impedance = GetDecimal(dr, "Impedance");
                objEntity.VectorGrp = GetTextVale(dr, "VectorGrp");
                objEntity.OMRequirements = GetTextVale(dr, "OMRequirements");
                objEntity.ModuleCleaningRequirements = GetTextVale(dr, "ModuleCleaningRequirements");
                objEntity.RoofPlan = GetTextVale(dr, "RoofPlan");
                objEntity.LoadDetails = GetTextVale(dr, "LoadDetails");
                objEntity.EarthResistivity = GetTextVale(dr, "EarthResistivity");
                objEntity.EarthPit = GetTextVale(dr, "EarthPit");
                objEntity.DistanceFromElectricalRoom = GetTextVale(dr, "DistanceFromElectricalRoom");
                objEntity.SheetType = GetTextVale(dr, "SheetType");
                objEntity.PurlinDistance = GetTextVale(dr, "PurlinDistance");
                objEntity.RoofSheet = GetTextVale(dr, "RoofSheet");
                objEntity.StructureStability = GetTextVale(dr, "StructureStability");
                objEntity.Skylight = GetTextVale(dr, "Skylight");
                objEntity.LadderToRoof = GetTextVale(dr, "LadderToRoof");
                objEntity.SoilTest = GetTextVale(dr, "SoilTest");
                objEntity.ContourSurvey = GetTextVale(dr, "ContourSurvey");
                objEntity.Tilt = GetTextVale(dr, "Tilt");
                objEntity.Inverter = GetTextVale(dr, "Inverter");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SiteSurvay> GetSiteSurvay(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SiteSurvay> lstObject = new List<Entity.SiteSurvay>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurveyList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.SiteSurvay objEntity = new Entity.SiteSurvay();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.DocNo = GetTextVale(dr, "DocNo");
                objEntity.SurvayDate = GetDateTime(dr, "SurvayDate");
                objEntity.SheetNo = GetTextVale(dr, "SheetNo");
                objEntity.CustID = GetInt64(dr, "CustID");
                objEntity.Customer = GetTextVale(dr, "Customer");
                objEntity.ContPerson1 = GetTextVale(dr, "ContPerson1");
                objEntity.ContNo1 = GetTextVale(dr, "ContNo1");
                objEntity.ContAddress1 = GetTextVale(dr, "ContAddress1");
                objEntity.ContEmail1 = GetTextVale(dr, "ContEmail1");
                objEntity.ContDesignation1 = GetTextVale(dr, "ContDesignation1");
                objEntity.ContPerson2 = GetTextVale(dr, "ContPerson2");
                objEntity.ContNo2 = GetTextVale(dr, "ContNo2");
                objEntity.ContAddress2 = GetTextVale(dr, "ContAddress2");
                objEntity.ContEmail2 = GetTextVale(dr, "ContEmail2");
                objEntity.ContDesignation2 = GetTextVale(dr, "ContDesignation2");
                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.Latitude = GetDecimal(dr, "Latitude");
                objEntity.Longitude = GetDecimal(dr, "Longitude");
                objEntity.Altitude = GetDecimal(dr, "Altitude");
                objEntity.NearByRailwayStation = GetTextVale(dr, "NearByRailwayStation");
                objEntity.NearByAirport = GetTextVale(dr, "NearByAirport");
                objEntity.WaterAndElectricity = GetTextVale(dr, "WaterAndElectricity");
                objEntity.RoofTopRCCLocation = GetTextVale(dr, "RoofTopRCCLocation");
                objEntity.RoofTopMetalSheetLocation = GetTextVale(dr, "RoofTopMetalSheetLocation");
                objEntity.GroundMountLocation = GetTextVale(dr, "GroundMountLocation");
                objEntity.StructureType = GetTextVale(dr, "StructureType");
                objEntity.RoofTopRCCTiltAngle = GetDecimal(dr, "RoofTopRCCTiltAngle");
                objEntity.RoofTopMetalSheetTiltAngle = GetDecimal(dr, "RoofTopMetalSheetTiltAngle");
                objEntity.GroundMountTiltAngle = GetDecimal(dr, "GroundMountTiltAngle");
                objEntity.RoofTopRCCArea = GetDecimal(dr, "RoofTopRCCArea");
                objEntity.RoofTopMetalSheetArea = GetDecimal(dr, "RoofTopMetalSheetArea");
                objEntity.GroundMountArea = GetDecimal(dr, "GroundMountArea");
                objEntity.RoofTopRCCOrientation = GetTextVale(dr, "RoofTopRCCOrientation");
                objEntity.RoofTopMetalSheetOrientation = GetTextVale(dr, "RoofTopMetalSheetOrientation");
                objEntity.GroundMountOrientation = GetTextVale(dr, "GroundMountOrientation");
                objEntity.PenetrationAllowed = GetTextVale(dr, "PenetrationAllowed");
                objEntity.OnGridDGRating = GetTextVale(dr, "OnGridDGRating");
                objEntity.OffGridDGRating = GetTextVale(dr, "OffGridDGRating");
                objEntity.HybridDGRating = GetTextVale(dr, "HybridDGRating");
                objEntity.OnGridContractDemand = GetTextVale(dr, "OnGridContractDemand");
                objEntity.OffGridContractDemand = GetTextVale(dr, "OffGridContractDemand");
                objEntity.HybridContractDemand = GetTextVale(dr, "HybridContractDemand");
                objEntity.OnGridCapacity = GetDecimal(dr, "OnGridCapacity");
                objEntity.OffGridCapacity = GetDecimal(dr, "OffGridCapacity");
                objEntity.HybridCapacity = GetDecimal(dr, "HybridCapacity");
                objEntity.InstalationType = GetTextVale(dr, "InstalationType");
                objEntity.DGSynchronisation = GetTextVale(dr, "DGSynchronisation");
                objEntity.DGOperationMode = GetTextVale(dr, "DGOperationMode");
                objEntity.DataMonitoring = GetTextVale(dr, "DataMonitoring");
                objEntity.WeatherMonitoringSystem = GetTextVale(dr, "WeatherMonitoringSystem");
                objEntity.AvailableBreaker = GetTextVale(dr, "AvailableBreaker");
                objEntity.BusBarTypeAndSize = GetTextVale(dr, "BusBarTypeAndSize");
                objEntity.KVARating = GetDecimal(dr, "KVARating");
                objEntity.PrimaryVolt = GetDecimal(dr, "PrimaryVolt");
                objEntity.SecondaryVolt = GetDecimal(dr, "SecondaryVolt");
                objEntity.Impedance = GetDecimal(dr, "Impedance");
                objEntity.VectorGrp = GetTextVale(dr, "VectorGrp");
                objEntity.OMRequirements = GetTextVale(dr, "OMRequirements");
                objEntity.ModuleCleaningRequirements = GetTextVale(dr, "ModuleCleaningRequirements");
                objEntity.RoofPlan = GetTextVale(dr, "RoofPlan");
                objEntity.LoadDetails = GetTextVale(dr, "LoadDetails");
                objEntity.EarthResistivity = GetTextVale(dr, "EarthResistivity");
                objEntity.EarthPit = GetTextVale(dr, "EarthPit");
                objEntity.DistanceFromElectricalRoom = GetTextVale(dr, "DistanceFromElectricalRoom");
                objEntity.SheetType = GetTextVale(dr, "SheetType");
                objEntity.PurlinDistance = GetTextVale(dr, "PurlinDistance");
                objEntity.RoofSheet = GetTextVale(dr, "RoofSheet");
                objEntity.StructureStability = GetTextVale(dr, "StructureStability");
                objEntity.Skylight = GetTextVale(dr, "Skylight");
                objEntity.LadderToRoof = GetTextVale(dr, "LadderToRoof");
                objEntity.SoilTest = GetTextVale(dr, "SoilTest");
                objEntity.ContourSurvey = GetTextVale(dr, "ContourSurvey");
                objEntity.Tilt = GetTextVale(dr, "Tilt");
                objEntity.Inverter = GetTextVale(dr, "Inverter");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SiteSurvay> GetSiteSurvay(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SiteSurvay> lstObject = new List<Entity.SiteSurvay>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurveyList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.SiteSurvay objEntity = new Entity.SiteSurvay();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.DocNo = GetTextVale(dr, "DocNo");
                objEntity.SurvayDate = GetDateTime(dr, "SurvayDate");
                objEntity.SheetNo = GetTextVale(dr, "SheetNo");
                objEntity.CustID = GetInt64(dr, "CustID");
                objEntity.Customer = GetTextVale(dr, "Customer");
                objEntity.ContPerson1 = GetTextVale(dr, "ContPerson1");
                objEntity.ContNo1 = GetTextVale(dr, "ContNo1");
                objEntity.ContAddress1 = GetTextVale(dr, "ContAddress1");
                objEntity.ContEmail1 = GetTextVale(dr, "ContEmail1");
                objEntity.ContDesignation1 = GetTextVale(dr, "ContDesignation1");
                objEntity.ContPerson2 = GetTextVale(dr, "ContPerson2");
                objEntity.ContNo2 = GetTextVale(dr, "ContNo2");
                objEntity.ContAddress2 = GetTextVale(dr, "ContAddress2");
                objEntity.ContEmail2 = GetTextVale(dr, "ContEmail2");
                objEntity.ContDesignation2 = GetTextVale(dr, "ContDesignation2");
                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.Latitude = GetDecimal(dr, "Latitude");
                objEntity.Longitude = GetDecimal(dr, "Longitude");
                objEntity.Altitude = GetDecimal(dr, "Altitude");
                objEntity.NearByRailwayStation = GetTextVale(dr, "NearByRailwayStation");
                objEntity.NearByAirport = GetTextVale(dr, "NearByAirport");
                objEntity.WaterAndElectricity = GetTextVale(dr, "WaterAndElectricity");
                objEntity.RoofTopRCCLocation = GetTextVale(dr, "RoofTopRCCLocation");
                objEntity.RoofTopMetalSheetLocation = GetTextVale(dr, "RoofTopMetalSheetLocation");
                objEntity.GroundMountLocation = GetTextVale(dr, "GroundMountLocation");
                objEntity.StructureType = GetTextVale(dr, "StructureType");
                objEntity.RoofTopRCCTiltAngle = GetDecimal(dr, "RoofTopRCCTiltAngle");
                objEntity.RoofTopMetalSheetTiltAngle = GetDecimal(dr, "RoofTopMetalSheetTiltAngle");
                objEntity.GroundMountTiltAngle = GetDecimal(dr, "GroundMountTiltAngle");
                objEntity.RoofTopRCCArea = GetDecimal(dr, "RoofTopRCCArea");
                objEntity.RoofTopMetalSheetArea = GetDecimal(dr, "RoofTopMetalSheetArea");
                objEntity.GroundMountArea = GetDecimal(dr, "GroundMountArea");
                objEntity.RoofTopRCCOrientation = GetTextVale(dr, "RoofTopRCCOrientation");
                objEntity.RoofTopMetalSheetOrientation = GetTextVale(dr, "RoofTopMetalSheetOrientation");
                objEntity.GroundMountOrientation = GetTextVale(dr, "GroundMountOrientation");
                objEntity.PenetrationAllowed = GetTextVale(dr, "PenetrationAllowed");
                objEntity.OnGridDGRating = GetTextVale(dr, "OnGridDGRating");
                objEntity.OffGridDGRating = GetTextVale(dr, "OffGridDGRating");
                objEntity.HybridDGRating = GetTextVale(dr, "HybridDGRating");
                objEntity.OnGridContractDemand = GetTextVale(dr, "OnGridContractDemand");
                objEntity.OffGridContractDemand = GetTextVale(dr, "OffGridContractDemand");
                objEntity.HybridContractDemand = GetTextVale(dr, "HybridContractDemand");
                objEntity.OnGridCapacity = GetDecimal(dr, "OnGridCapacity");
                objEntity.OffGridCapacity = GetDecimal(dr, "OffGridCapacity");
                objEntity.HybridCapacity = GetDecimal(dr, "HybridCapacity");
                objEntity.InstalationType = GetTextVale(dr, "InstalationType");
                objEntity.DGSynchronisation = GetTextVale(dr, "DGSynchronisation");
                objEntity.DGOperationMode = GetTextVale(dr, "DGOperationMode");
                objEntity.DataMonitoring = GetTextVale(dr, "DataMonitoring");
                objEntity.WeatherMonitoringSystem = GetTextVale(dr, "WeatherMonitoringSystem");
                objEntity.AvailableBreaker = GetTextVale(dr, "AvailableBreaker");
                objEntity.BusBarTypeAndSize = GetTextVale(dr, "BusBarTypeAndSize");
                objEntity.KVARating = GetDecimal(dr, "KVARating");
                objEntity.PrimaryVolt = GetDecimal(dr, "PrimaryVolt");
                objEntity.SecondaryVolt = GetDecimal(dr, "SecondaryVolt");
                objEntity.Impedance = GetDecimal(dr, "Impedance");
                objEntity.VectorGrp = GetTextVale(dr, "VectorGrp");
                objEntity.OMRequirements = GetTextVale(dr, "OMRequirements");
                objEntity.ModuleCleaningRequirements = GetTextVale(dr, "ModuleCleaningRequirements");
                objEntity.RoofPlan = GetTextVale(dr, "RoofPlan");
                objEntity.LoadDetails = GetTextVale(dr, "LoadDetails");
                objEntity.EarthResistivity = GetTextVale(dr, "EarthResistivity");
                objEntity.EarthPit = GetTextVale(dr, "EarthPit");
                objEntity.DistanceFromElectricalRoom = GetTextVale(dr, "DistanceFromElectricalRoom");
                objEntity.SheetType = GetTextVale(dr, "SheetType");
                objEntity.PurlinDistance = GetTextVale(dr, "PurlinDistance");
                objEntity.RoofSheet = GetTextVale(dr, "RoofSheet");
                objEntity.StructureStability = GetTextVale(dr, "StructureStability");
                objEntity.Skylight = GetTextVale(dr, "Skylight");
                objEntity.LadderToRoof = GetTextVale(dr, "LadderToRoof");
                objEntity.SoilTest = GetTextVale(dr, "SoilTest");
                objEntity.ContourSurvey = GetTextVale(dr, "ContourSurvey");
                objEntity.Tilt = GetTextVale(dr, "Tilt");
                objEntity.Inverter = GetTextVale(dr, "Inverter");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateSiteSurvay(Entity.SiteSurvay objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnSiteSurvayNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SiteSurvey_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@DocNo", objEntity.DocNo);
            cmdAdd.Parameters.AddWithValue("@SheetNo", objEntity.SheetNo);
            cmdAdd.Parameters.AddWithValue("@SurvayDate", objEntity.SurvayDate.ToString("yyyy/MM/dd"));
            cmdAdd.Parameters.AddWithValue("@CustID", objEntity.CustID);
            cmdAdd.Parameters.AddWithValue("@Customer", objEntity.Customer);
            cmdAdd.Parameters.AddWithValue("@ContPerson1", objEntity.ContPerson1);
            cmdAdd.Parameters.AddWithValue("@ContNo1", objEntity.ContNo1);
            cmdAdd.Parameters.AddWithValue("@ContAddress1", objEntity.ContAddress1);
            cmdAdd.Parameters.AddWithValue("@ContEmail1", objEntity.ContEmail1);    
            cmdAdd.Parameters.AddWithValue("@ContDesignation1", objEntity.ContDesignation1);
            cmdAdd.Parameters.AddWithValue("@ContPerson2", objEntity.ContPerson2);
            cmdAdd.Parameters.AddWithValue("@ContNo2", objEntity.ContNo2);
            cmdAdd.Parameters.AddWithValue("@ContAddress2", objEntity.ContAddress2);
            cmdAdd.Parameters.AddWithValue("@ContEmail2", objEntity.ContEmail2);
            cmdAdd.Parameters.AddWithValue("@ContDesignation2", objEntity.ContDesignation2);
            cmdAdd.Parameters.AddWithValue("@SiteAddress", objEntity.SiteAddress);
            cmdAdd.Parameters.AddWithValue("@Latitude", objEntity.Latitude);
            cmdAdd.Parameters.AddWithValue("@Longitude", objEntity.Longitude);
            cmdAdd.Parameters.AddWithValue("@Altitude", objEntity.Altitude);
            cmdAdd.Parameters.AddWithValue("@NearByRailwayStation", objEntity.NearByRailwayStation);
            cmdAdd.Parameters.AddWithValue("@NearByAirport", objEntity.NearByAirport);
            cmdAdd.Parameters.AddWithValue("@WaterAndElectricity", objEntity.WaterAndElectricity);
            cmdAdd.Parameters.AddWithValue("@RoofTopRCCLocation", objEntity.RoofTopRCCLocation);
            cmdAdd.Parameters.AddWithValue("@RoofTopMetalSheetLocation", objEntity.RoofTopMetalSheetLocation);
            cmdAdd.Parameters.AddWithValue("@GroundMountLocation", objEntity.GroundMountLocation);
            cmdAdd.Parameters.AddWithValue("@StructureType", objEntity.StructureType);
            cmdAdd.Parameters.AddWithValue("@RoofTopRCCTiltAngle", objEntity.RoofTopRCCTiltAngle);
            cmdAdd.Parameters.AddWithValue("@RoofTopMetalSheetTiltAngle", objEntity.RoofTopMetalSheetTiltAngle);
            cmdAdd.Parameters.AddWithValue("@GroundMountTiltAngle", objEntity.GroundMountTiltAngle);
            cmdAdd.Parameters.AddWithValue("@RoofTopRCCArea", objEntity.RoofTopRCCArea);
            cmdAdd.Parameters.AddWithValue("@RoofTopMetalSheetArea", objEntity.RoofTopMetalSheetArea);
            cmdAdd.Parameters.AddWithValue("@GroundMountArea", objEntity.GroundMountArea);
            cmdAdd.Parameters.AddWithValue("@RoofTopRCCOrientation", objEntity.RoofTopRCCOrientation);
            cmdAdd.Parameters.AddWithValue("@RoofTopMetalSheetOrientation", objEntity.RoofTopMetalSheetOrientation);
            cmdAdd.Parameters.AddWithValue("@GroundMountOrientation", objEntity.GroundMountOrientation);
            cmdAdd.Parameters.AddWithValue("@PenetrationAllowed", objEntity.PenetrationAllowed);
            cmdAdd.Parameters.AddWithValue("@OnGridDGRating", objEntity.OnGridDGRating);
            cmdAdd.Parameters.AddWithValue("@OffGridDGRating", objEntity.OffGridDGRating);
            cmdAdd.Parameters.AddWithValue("@HybridDGRating", objEntity.HybridDGRating);
            cmdAdd.Parameters.AddWithValue("@OnGridContractDemand", objEntity.OnGridContractDemand);
            cmdAdd.Parameters.AddWithValue("@OffGridContractDemand", objEntity.OffGridContractDemand);
            cmdAdd.Parameters.AddWithValue("@HybridContractDemand", objEntity.HybridContractDemand);
            cmdAdd.Parameters.AddWithValue("@OnGridCapacity", objEntity.OnGridCapacity);
            cmdAdd.Parameters.AddWithValue("@OffGridCapacity", objEntity.OffGridCapacity);
            cmdAdd.Parameters.AddWithValue("@HybridCapacity", objEntity.HybridCapacity);
            cmdAdd.Parameters.AddWithValue("@InstalationType", objEntity.InstalationType);
            cmdAdd.Parameters.AddWithValue("@DGSynchronisation", objEntity.DGSynchronisation);
            cmdAdd.Parameters.AddWithValue("@DGOperationMode", objEntity.DGOperationMode);
            cmdAdd.Parameters.AddWithValue("@DataMonitoring", objEntity.DataMonitoring);
            cmdAdd.Parameters.AddWithValue("@WeatherMonitoringSystem", objEntity.WeatherMonitoringSystem);
            cmdAdd.Parameters.AddWithValue("@AvailableBreaker", objEntity.AvailableBreaker);
            cmdAdd.Parameters.AddWithValue("@BusBarTypeAndSize", objEntity.BusBarTypeAndSize);
            cmdAdd.Parameters.AddWithValue("@KVARating", objEntity.KVARating);
            cmdAdd.Parameters.AddWithValue("@PrimaryVolt", objEntity.PrimaryVolt);
            cmdAdd.Parameters.AddWithValue("@SecondaryVolt", objEntity.SecondaryVolt);
            cmdAdd.Parameters.AddWithValue("@Impedance", objEntity.Impedance);
            cmdAdd.Parameters.AddWithValue("@VectorGrp", objEntity.VectorGrp);
            cmdAdd.Parameters.AddWithValue("@OMRequirements", objEntity.OMRequirements);
            cmdAdd.Parameters.AddWithValue("@ModuleCleaningRequirements", objEntity.ModuleCleaningRequirements);
            cmdAdd.Parameters.AddWithValue("@RoofPlan", objEntity.RoofPlan);
            cmdAdd.Parameters.AddWithValue("@LoadDetails", objEntity.LoadDetails);
            cmdAdd.Parameters.AddWithValue("@EarthResistivity", objEntity.EarthResistivity);
            cmdAdd.Parameters.AddWithValue("@EarthPit", objEntity.EarthPit);
            cmdAdd.Parameters.AddWithValue("@DistanceFromElectricalRoom", objEntity.DistanceFromElectricalRoom);
            cmdAdd.Parameters.AddWithValue("@SheetType", objEntity.SheetType);
            cmdAdd.Parameters.AddWithValue("@PurlinDistance", objEntity.PurlinDistance);
            cmdAdd.Parameters.AddWithValue("@RoofSheet", objEntity.RoofSheet);
            cmdAdd.Parameters.AddWithValue("@StructureStability", objEntity.StructureStability);
            cmdAdd.Parameters.AddWithValue("@Skylight", objEntity.Skylight);
            cmdAdd.Parameters.AddWithValue("@LadderToRoof", objEntity.LadderToRoof);
            cmdAdd.Parameters.AddWithValue("@SoilTest", objEntity.SoilTest);
            cmdAdd.Parameters.AddWithValue("@ContourSurvey", objEntity.ContourSurvey);
            cmdAdd.Parameters.AddWithValue("@Tilt", objEntity.Tilt);
            cmdAdd.Parameters.AddWithValue("@Inverter", objEntity.Inverter);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnDocNo", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);

            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnSiteSurvayNo = cmdAdd.Parameters["@ReturnDocNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSiteSurvay(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SiteSurvey_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        //======================================================================//
        // Get Site Survay Documents
        //======================================================================//

        public virtual List<Entity.SiteSurvayDocuments> GetSiteSurvayDocumentsList(Int64 pkID, String DocNo, String Type)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurvayDocumentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@DocNo", DocNo);
            cmdGet.Parameters.AddWithValue("@Type", Type);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SiteSurvayDocuments> lstLocation = new List<Entity.SiteSurvayDocuments>();
            while (dr.Read())
            {
                Entity.SiteSurvayDocuments objLocation = new Entity.SiteSurvayDocuments();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.DocNo = GetTextVale(dr, "DocNo");
                objLocation.FileName = GetTextVale(dr, "FileName");
                objLocation.FileType = GetTextVale(dr, "FileType");
                objLocation.data = GetTextVale(dr, "data");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
       
        public virtual void AddUpdateSiteSurvayDocuments(String DocNo, string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "insert into MST_SiteSurvay_Documents (DocNo, Name,type,createdby)" + " values (@DocNo, @Name, @type, @LoginUserID)";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@DocNo", SqlDbType.VarChar).Value = DocNo;
                cmdAdd.Parameters.Add("@Name", SqlDbType.VarChar).Value = pFilename;
                cmdAdd.Parameters.Add("@type", SqlDbType.VarChar).Value = pType;
                cmdAdd.Parameters.Add("@LoginUserID", SqlDbType.VarChar).Value = pLoginUserID;
                ExecuteNonQuery(cmdAdd);

                ReturnCode = 1;
                ReturnMsg = "File Uploaded Successfully !";
            }
            catch (Exception ex)
            {
                ReturnCode = 0;
                ReturnMsg = ex.Message.ToString();
            }
            ForceCloseConncetion();
        }

        public virtual void DeleteSiteSurvayDocumentsByDocNo(String DocNo,String Type, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SiteSurvayDocumentsByDOcNo_DEL";
            cmdDel.Parameters.AddWithValue("@DocNo", DocNo);
            cmdDel.Parameters.AddWithValue("@Type", Type);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }
    }
}
