using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class HolidaySQL: BaseSqlManager
    {
        public virtual List<Entity.Holiday> GetHolidayList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "HolidayList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Holiday> lstObject = new List<Entity.Holiday>();
            while (dr.Read())
            {
                Entity.Holiday objEntity = new Entity.Holiday();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Holiday_Year = GetInt64(dr, "Holiday_Year");                
                objEntity.Holiday_Type = GetTextVale(dr, "Holiday_Type");
                objEntity.Holiday_Name = GetTextVale(dr, "Holiday_Name");
                objEntity.Holiday_Date = GetDateTime(dr, "Holiday_Date");
                objEntity.Holiday_Description = GetTextVale(dr, "Holiday_Description");
                objEntity.imageurl = GetTextVale(dr, "ImageURL");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Holiday> GetHolidayList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Holiday> lstLocation = new List<Entity.Holiday>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "HolidayList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Holiday objEntity = new Entity.Holiday();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Holiday_Year = GetInt64(dr, "Holiday_Year");
                objEntity.Holiday_Type = GetTextVale(dr, "Holiday_Type");
                objEntity.Holiday_Name = GetTextVale(dr, "Holiday_Name");
                objEntity.Holiday_Date = GetDateTime(dr, "Holiday_Date");
                objEntity.Holiday_Description = GetTextVale(dr, "Holiday_Description");
                objEntity.imageurl = GetTextVale(dr, "ImageURL");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;           
        }

        public virtual List<Entity.Holiday> GetHolidayList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Holiday> lstLocation = new List<Entity.Holiday>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "HolidayList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
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
                Entity.Holiday objEntity = new Entity.Holiday();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Holiday_Year = GetInt64(dr, "Holiday_Year");
                objEntity.Holiday_Type = GetTextVale(dr, "Holiday_Type");
                objEntity.Holiday_Name = GetTextVale(dr, "Holiday_Name");
                objEntity.Holiday_Date = GetDateTime(dr, "Holiday_Date");
                objEntity.Holiday_Description = GetTextVale(dr, "Holiday_Description");
                objEntity.imageurl = GetTextVale(dr, "ImageURL");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Holiday> GetHolidayListByName(string Holiday_Type)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "HolidayListByName";
            cmdGet.Parameters.AddWithValue("@HolidayType", Holiday_Type);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Holiday> lstObject = new List<Entity.Holiday>();
            while (dr.Read())
            {
                Entity.Holiday objEntity = new Entity.Holiday();
                objEntity.Holiday_Type = GetTextVale(dr, "Holiday_Type");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Holiday> GetHolidayListByCount(Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "HolidayListByCount";
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Holiday> lstObject = new List<Entity.Holiday>();
            while (dr.Read())
            {
                Entity.Holiday objEntity = new Entity.Holiday();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Holiday_Name = GetTextVale(dr, "Holiday_Name");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.TotalHolidays = GetInt64(dr, "TotalHolidays");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        // ============================= Insert & Update
        public virtual void AddUpdateHoliday(Entity.Holiday objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Holiday_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@Holiday_Year", objEntity.Holiday_Year);
            cmdAdd.Parameters.AddWithValue("@Holiday_Type", objEntity.Holiday_Type);
            cmdAdd.Parameters.AddWithValue("@Holiday_Name", objEntity.Holiday_Name);
            cmdAdd.Parameters.AddWithValue("@Holiday_Date", objEntity.Holiday_Date);
            cmdAdd.Parameters.AddWithValue("@Holiday_Description", objEntity.Holiday_Description);
            cmdAdd.Parameters.AddWithValue("@ImageURL", objEntity.imageurl);
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

        public virtual void DeleteHoliday(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Holiday_DEL";
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
    }
}
