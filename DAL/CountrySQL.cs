using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CountrySQL: BaseSqlManager
    {
        public virtual List<Entity.Country> GetCountryList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CountryList";
            cmdGet.Parameters.AddWithValue("@CountryCode", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);  
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Country> lstObject = new List<Entity.Country>();
            while (dr.Read())
            {
                Entity.Country objEntity = new Entity.Country();
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Country> GetCountry(string CountryCode, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Country> lstLocation = new List<Entity.Country>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CountryList";
            cmdGet.Parameters.AddWithValue("@CountryCode", CountryCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);  
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Country objEntity = new Entity.Country();
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Country> GetCountry(string CountryCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Country> lstLocation = new List<Entity.Country>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CountryList";
            cmdGet.Parameters.AddWithValue("@CountryCode", CountryCode);
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
                Entity.Country objEntity = new Entity.Country();
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateCountry(Entity.Country objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Country_INS_UPD";
            if (objEntity.CountryName.Length > 3)
                objEntity.CountryCode = (objEntity.CountryName).Substring(0, 3).ToUpper();
            else
                objEntity.CountryCode = (objEntity.CountryName).ToUpper();
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@CountryName", objEntity.CountryName);
            cmdAdd.Parameters.AddWithValue("@CurrencyName", objEntity.CurrencyName);
            cmdAdd.Parameters.AddWithValue("@CurrencySymbol", objEntity.CurrencySymbol);
            cmdAdd.Parameters.AddWithValue("@CountryISO", objEntity.CountryISO);
            cmdAdd.Parameters.AddWithValue("@ActiveFlag", 1);
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

        public virtual void DeleteCountry(string CountryCode, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Country_DEL";
            cmdDel.Parameters.AddWithValue("@CountryCode", CountryCode);
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
