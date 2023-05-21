using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CitySQL: BaseSqlManager
    {
        public virtual List<Entity.City> GetCityList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CityList";
            cmdGet.Parameters.AddWithValue("@CityCode", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);   
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.City> lstObject = new List<Entity.City>();
            while (dr.Read())
            {
                Entity.City objEntity = new Entity.City();
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.City> GetCity(Int64 CityCode, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.City> lstLocation = new List<Entity.City>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CityList";
            cmdGet.Parameters.AddWithValue("@CityCode", CityCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);   
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.City objEntity = new Entity.City();
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.City> GetCity(Int64 CityCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.City> lstLocation = new List<Entity.City>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CityList";
            cmdGet.Parameters.AddWithValue("@CityCode", CityCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
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
                Entity.City objEntity = new Entity.City();
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.City> GetCityByState(Int64 StateCode)
        {
            List<Entity.City> lstLocation = new List<Entity.City>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CityListByState";
            cmdGet.Parameters.AddWithValue("@StateCode", StateCode);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.City objEntity = new Entity.City();
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateCity(Entity.City objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "City_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
            cmdAdd.Parameters.AddWithValue("@CityName", objEntity.CityName);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
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

        public virtual void DeleteCity(Int64 CityCode, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "City_DEL";
            cmdDel.Parameters.AddWithValue("@CityCode", CityCode);
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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.City> GetCityListForDropdown(String CityName, Int64 StateCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CityListForDropdown";
            cmdGet.Parameters.AddWithValue("@CityName", CityName);
            cmdGet.Parameters.AddWithValue("@StateCode", StateCode);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.City> lstObject = new List<Entity.City>();
            while (dr.Read())
            {
                Entity.City objEntity = new Entity.City();
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.State> GetStateListForDropdown(String StateName, Int64 CountryCode)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "StateListForDropdown";
            cmdGet.Parameters.AddWithValue("@StateName", StateName);
            cmdGet.Parameters.AddWithValue("@CountryCode", CountryCode);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.State> lstObject = new List<Entity.State>();
            while (dr.Read())
            {
                Entity.State objEntity = new Entity.State();
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
    }
}
