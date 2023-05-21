using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SiteSurveyReportSQL : BaseSqlManager
    {
        public virtual List<Entity.SiteSurveyReport> GetSiteSurveyReport(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurveyReportList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            //cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SiteSurveyReport> lstObject = new List<Entity.SiteSurveyReport>();
            while (dr.Read())
            {
                Entity.SiteSurveyReport objEntity = new Entity.SiteSurveyReport();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SurveyID = GetTextVale(dr, "SurveyID");
                objEntity.VisitDate = GetDateTime(dr, "VisitDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.SolarPosition = GetTextVale(dr, "SolarPosition");
                objEntity.SolarPositionRemarks = GetTextVale(dr, "SolarPositionRemarks");
                objEntity.BuildType = GetTextVale(dr, "BuildType");
                objEntity.BuildTypeRemarks = GetTextVale(dr, "BuildTypeRemarks");
                objEntity.RoofType = GetTextVale(dr, "RoofType");
                objEntity.RoofTypeRemarks = GetTextVale(dr, "RoofTypeRemarks");
                objEntity.ClientReq = GetTextVale(dr, "ClientReq");
                objEntity.SanctionLoad = GetTextVale(dr, "SanctionLoad");
                objEntity.MonthlyConsumption = GetTextVale(dr, "MonthlyConsumption");
                objEntity.TotalArea = GetTextVale(dr, "TotalArea");
                objEntity.LeaseProperty = GetBoolean(dr, "LeaseProperty");
                objEntity.ExistingPhase = GetTextVale(dr, "ExistingPhase");
                objEntity.ExistingPhaseRemarks = GetTextVale(dr, "ExistingPhaseRemarks");
                objEntity.Synchronization = GetBoolean(dr, "Synchronization");
                objEntity.ReasonForSync = GetTextVale(dr, "ReasonForSync");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ClientQueries = GetTextVale(dr, "ClientQueries");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                //objEntity.LoginUserID = GetInt64(dr, "LoinUserID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.SiteSurveyReport> GetSiteSurveyReport(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurveyReportList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            //cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SiteSurveyReport> lstObject = new List<Entity.SiteSurveyReport>();
            while (dr.Read())
            {
                Entity.SiteSurveyReport objEntity = new Entity.SiteSurveyReport();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SurveyID = GetTextVale(dr, "SurveyID");
                objEntity.VisitDate = GetDateTime(dr, "VisitDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.SolarPosition = GetTextVale(dr, "SolarPosition");
                objEntity.SolarPositionRemarks = GetTextVale(dr, "SolarPositionRemarks");
                objEntity.BuildType = GetTextVale(dr, "BuildType");
                objEntity.BuildTypeRemarks = GetTextVale(dr, "BuildTypeRemarks");
                objEntity.RoofType = GetTextVale(dr, "RoofType");
                objEntity.RoofTypeRemarks = GetTextVale(dr, "RoofTypeRemarks");
                objEntity.ClientReq = GetTextVale(dr, "ClientReq");
                objEntity.SanctionLoad = GetTextVale(dr, "SanctionLoad");
                objEntity.MonthlyConsumption = GetTextVale(dr, "MonthlyConsumption");
                objEntity.TotalArea = GetTextVale(dr, "TotalArea");
                objEntity.LeaseProperty = GetBoolean(dr, "LeaseProperty");
                objEntity.ExistingPhase = GetTextVale(dr, "ExistingPhase");
                objEntity.ExistingPhaseRemarks = GetTextVale(dr, "ExistingPhaseRemarks");
                objEntity.Synchronization = GetBoolean(dr, "Synchronization");
                objEntity.ReasonForSync = GetTextVale(dr, "ReasonForSync");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ClientQueries = GetTextVale(dr, "ClientQueries");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                //objEntity.LoginUserID = GetInt64(dr, "LoinUserID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.SiteSurveyReport> GetSiteSurveyReport(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SiteSurveyReportList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SiteSurveyReport> lstObject = new List<Entity.SiteSurveyReport>();
            while (dr.Read())
            {
                Entity.SiteSurveyReport objEntity = new Entity.SiteSurveyReport();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SurveyID = GetTextVale(dr, "SurveyID");
                objEntity.VisitDate = GetDateTime(dr, "VisitDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.SolarPosition = GetTextVale(dr, "SolarPosition");
                objEntity.SolarPositionRemarks = GetTextVale(dr, "SolarPositionRemarks");
                objEntity.BuildType = GetTextVale(dr, "BuildType");
                objEntity.BuildTypeRemarks = GetTextVale(dr, "BuildTypeRemarks");
                objEntity.RoofType = GetTextVale(dr, "RoofType");
                objEntity.RoofTypeRemarks = GetTextVale(dr, "RoofTypeRemarks");
                objEntity.ClientReq = GetTextVale(dr, "ClientReq");
                objEntity.SanctionLoad = GetTextVale(dr, "SanctionLoad");
                objEntity.MonthlyConsumption = GetTextVale(dr, "MonthlyConsumption");
                objEntity.TotalArea = GetTextVale(dr, "TotalArea");
                objEntity.LeaseProperty = GetBoolean(dr, "LeaseProperty");
                objEntity.ExistingPhase = GetTextVale(dr, "ExistingPhase");
                objEntity.ExistingPhaseRemarks = GetTextVale(dr, "ExistingPhaseRemarks");
                objEntity.Synchronization = GetBoolean(dr, "Synchronization");
                objEntity.ReasonForSync = GetTextVale(dr, "ReasonForSync");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ClientQueries = GetTextVale(dr, "ClientQueries");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                //objEntity.LoginUserID = GetInt64(dr, "LoinUserID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void AddUpdateSiteSurveyReport(Entity.SiteSurveyReport objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnSurveyID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SiteSurveyReport_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@SurveyID", objEntity.SurveyID);
            cmdAdd.Parameters.AddWithValue("@VisitDate", objEntity.VisitDate.ToString("yyyy/MM/dd"));
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@SolarPosition", objEntity.SolarPosition);
            cmdAdd.Parameters.AddWithValue("@SolarPositionRemarks", objEntity.SolarPositionRemarks);
            cmdAdd.Parameters.AddWithValue("@BuildType", objEntity.BuildType);
            cmdAdd.Parameters.AddWithValue("@BuildTypeRemarks", objEntity.BuildTypeRemarks);
            cmdAdd.Parameters.AddWithValue("@RoofType", objEntity.RoofType);
            cmdAdd.Parameters.AddWithValue("@RoofTypeRemarks", objEntity.RoofTypeRemarks);
            cmdAdd.Parameters.AddWithValue("@ClientReq", objEntity.ClientReq);
            cmdAdd.Parameters.AddWithValue("@SanctionLoad", objEntity.SanctionLoad);
            cmdAdd.Parameters.AddWithValue("@MonthlyConsumption", objEntity.MonthlyConsumption);
            cmdAdd.Parameters.AddWithValue("@TotalArea", objEntity.TotalArea);
            cmdAdd.Parameters.AddWithValue("@LeaseProperty", objEntity.LeaseProperty);
            cmdAdd.Parameters.AddWithValue("@ExistingPhase", objEntity.ExistingPhase);
            cmdAdd.Parameters.AddWithValue("@ExistingPhaseRemarks", objEntity.ExistingPhaseRemarks);
            cmdAdd.Parameters.AddWithValue("@Synchronization", objEntity.Synchronization);
            cmdAdd.Parameters.AddWithValue("@ReasonForSync", objEntity.ReasonForSync);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@ClientQueries", objEntity.ClientQueries);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnSurveyID", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);

            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnSurveyID = cmdAdd.Parameters["@ReturnSurveyID"].Value.ToString();
            ForceCloseConncetion();
        }
        public virtual void DeleteSiteSurveyReport(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SiteSurveyReport_DEL";
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

        //----------------------------------------Site Survey Report Roof Details---------------------------------------------

        public virtual void AddUpdateSSRRoofingDetails(Entity.SSRRoofDetails objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SSRRoofDetails_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@SurveyID", objEntity.SurveyID);
            cmdAdd.Parameters.AddWithValue("@BuildingName", objEntity.BuildingName);
            cmdAdd.Parameters.AddWithValue("@RoofArea", objEntity.RoofArea);
            cmdAdd.Parameters.AddWithValue("@RoofType", objEntity.RoofType);
            cmdAdd.Parameters.AddWithValue("@CapacityOfBuilding", objEntity.CapacityOfBuilding);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSSRRoofDetails(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSRRoofDetails_DEL";
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

        public virtual void DeleteSSRRoofDetailsBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSRRoofDetails_DELBySurveyID";
            cmdDel.Parameters.AddWithValue("@SurveyID", SurveyID);
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
        public DataTable GetSSRRoofDetails(String SurveyID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID,SurveyID,BuildingName,RoofArea,RoofType,CapacityOfBuilding from SiteSurveyReport_RoofDetails Where SurveyID = '" + SurveyID  + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        //--------------------------------------Site Survey Report for Equipment and Loation Details---------------------------

        public virtual void AddUpdateSSREquipmentLocation(Entity.SSREquipmentLocation objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SSREquipmentLocation_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@SurveyID", objEntity.SurveyID);
            cmdAdd.Parameters.AddWithValue("@Equipment", objEntity.Equipment);
            cmdAdd.Parameters.AddWithValue("@Distance", objEntity.Distance);
            cmdAdd.Parameters.AddWithValue("@ConnPossibility", objEntity.ConnPossibility);
            cmdAdd.Parameters.AddWithValue("@ClientRating", objEntity.ClientRating);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSSREquipmentLocation(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSREquipmentLocation_DEL";
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

        public virtual void DeleteSSREquipmentLocationBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSREuipmentLocation_DELBySurveyID";
            cmdDel.Parameters.AddWithValue("@SurveyID", SurveyID);
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

        public DataTable GetSSREquipmentLocation(String SurveyID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (SurveyID != "")
                myCommand.CommandText = "Select pkID,SurveyID,Equipment,Distance,ConnPossibility,ClientRating from SiteSurveyReport_EquipLocations Where SurveyID = '" + SurveyID + "'";
            else
                myCommand.CommandText = "Select pkID,Description As Equipment,cast('' as nvarchar(50)) As Distance, cast('' as nvarchar(50)) As ConnPossibility, cast('' as nvarchar(50)) As ClientRating from MST_CatDescSSR Where Category = 'EquipmentLocation'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        //--------------------------------------Site Survey Report for Existing System Availability---------------------------

        public virtual void AddUpdateSSRSysAvailablity(Entity.SSRSysAvailablity objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SSRSysAvailablity_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@SurveyID", objEntity.SurveyID);
            cmdAdd.Parameters.AddWithValue("@LoadDesc", objEntity.LoadDesc);
            cmdAdd.Parameters.AddWithValue("@Capacity", objEntity.Capacity);
            cmdAdd.Parameters.AddWithValue("@Voltage", objEntity.Voltage);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSSRSysAvailablity(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSRSysAvailablity_DEL";
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

        public virtual void DeleteSSRSysAvailablityBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSRSysAvailablity_DELBySurveyID";
            cmdDel.Parameters.AddWithValue("@SurveyID", SurveyID);
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

        public DataTable GetSSRSysAvailablity(String SurveyID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (SurveyID != "")
                myCommand.CommandText = "Select pkID,SurveyID,LoadDesc,Capacity,Voltage,Quantity from SiteSurveyReport_SysAvailablity Where SurveyID = '" + SurveyID + "'";
            else
                myCommand.CommandText = "Select pkID,Description As LoadDesc,cast('' as nvarchar(50)) As Capacity, cast('' as nvarchar(50)) As Voltage, cast('' as nvarchar(50)) As Quantity from MST_CatDescSSR Where Category = 'SystemAvailablity'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        //--------------------------------------Site Survey Report for Required Engineering Details---------------------------

        public virtual void AddUpdateSSRRequiredDetails(Entity.SSRRequiredDetails objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SSRRequiredDetails_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@SurveyID", objEntity.SurveyID);
            cmdAdd.Parameters.AddWithValue("@Description", objEntity.Description);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteSSRRequiredDetails(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSRRequiredDetails_DEL";
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

        public virtual void DeleteSSRRequiredDetailsBySurveyID(String SurveyID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SSRRequiredDetails_DELBySurveyID";
            cmdDel.Parameters.AddWithValue("@SurveyID", SurveyID);
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

        public DataTable GetSSRRequiredDetails(String SurveyID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (SurveyID != "")
                myCommand.CommandText = "Select pkID,SurveyID,Description,Remarks from SiteSurveyReport_RequiredDetails Where SurveyID = '" + SurveyID + "'";
            else
                myCommand.CommandText = "Select pkID,Description As Description,cast('' as nvarchar(200)) As Remarks from MST_CatDescSSR Where Category = 'RequiredEngineering'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
    }
}
