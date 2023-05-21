using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ShiftMasterSQL : BaseSqlManager
    {

        public virtual List<Entity.ShiftMaster> GetShiftMaster(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ShiftMasterList";
            cmdGet.Parameters.AddWithValue("@ShiftCode", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ShiftMaster> lstObject = new List<Entity.ShiftMaster>();
            while (dr.Read())
            {
                Entity.ShiftMaster objEntity = new Entity.ShiftMaster();
                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.StartTime = GetTextVale(dr, "StartTime");
                objEntity.EndTime = GetTextVale(dr, "EndTime");
                objEntity.GraceMins = GetInt64(dr, "GraceMins");
                objEntity.MinHrsHalfDay = GetDecimal(dr, "MinHrsHalfDay");
                objEntity.MinHrsFullDay = GetDecimal(dr, "MinHrsFullDay");
                objEntity.LunchFrom = GetTextVale(dr, "LunchFrom");
                objEntity.LunchTo = GetTextVale(dr, "LunchTo");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.ShiftMaster> GetShiftMaster(Int64 ShiftCode, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ShiftMaster> lstObject = new List<Entity.ShiftMaster>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ShiftMasterList";
            cmdGet.Parameters.AddWithValue("@ShiftCode", ShiftCode);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ShiftMaster objEntity = new Entity.ShiftMaster();
                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.StartTime = GetTextVale(dr, "StartTime");
                objEntity.EndTime = GetTextVale(dr, "EndTime");
                objEntity.GraceMins = GetInt64(dr, "GraceMins");

                objEntity.MinHrsHalfDay = GetDecimal(dr, "MinHrsHalfDay");
                objEntity.MinHrsFullDay = GetDecimal(dr, "MinHrsFullDay");
                objEntity.LunchFrom = GetTextVale(dr, "LunchFrom");
                objEntity.LunchTo = GetTextVale(dr, "LunchTo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.ShiftMaster> GetShiftMaster(Int64 ShiftCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ShiftMaster> lstObject = new List<Entity.ShiftMaster>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ShiftMasterList";
            cmdGet.Parameters.AddWithValue("@ShiftCode", ShiftCode);
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
                Entity.ShiftMaster objEntity = new Entity.ShiftMaster();
                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.StartTime = GetTextVale(dr, "StartTime");
                objEntity.EndTime = GetTextVale(dr, "EndTime");
                objEntity.GraceMins = GetInt64(dr, "GraceMins");

                objEntity.MinHrsHalfDay = GetDecimal(dr, "MinHrsHalfDay");
                objEntity.MinHrsFullDay = GetDecimal(dr, "MinHrsFullDay");
                objEntity.LunchFrom = GetTextVale(dr, "LunchFrom");
                objEntity.LunchTo = GetTextVale(dr, "LunchTo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateShiftMaster(Entity.ShiftMaster objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ShiftMaster_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@ShiftCode", objEntity.ShiftCode);
            cmdAdd.Parameters.AddWithValue("@ShiftName", objEntity.ShiftName);
            cmdAdd.Parameters.AddWithValue("@StartTime", objEntity.StartTime);
            cmdAdd.Parameters.AddWithValue("@EndTime", objEntity.EndTime);
            cmdAdd.Parameters.AddWithValue("@GraceMins", objEntity.GraceMins);
            cmdAdd.Parameters.AddWithValue("@MinHrsHalfDay", objEntity.MinHrsHalfDay);
            cmdAdd.Parameters.AddWithValue("@MinHrsFullDay", objEntity.MinHrsFullDay);
            cmdAdd.Parameters.AddWithValue("@LunchFrom", objEntity.LunchFrom);
            cmdAdd.Parameters.AddWithValue("@LunchTo", objEntity.LunchTo);

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

        public virtual void DeleteShiftMaster(Int64 ShiftCode, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ShiftMaster_DEL";
            cmdDel.Parameters.AddWithValue("@ShiftCode", ShiftCode);
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
