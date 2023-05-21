using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DailyActivitySQL:BaseSqlManager
    {
        public virtual List<Entity.DailyActivity> GetDailyActivityList(Int64 pkID, string ActivityDate, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.DailyActivity> lstLocation = new List<Entity.DailyActivity>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DailyActivityList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ActivityDate", Convert.ToDateTime(ActivityDate));
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.DailyActivity objEntity = new Entity.DailyActivity();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskDuration = GetDecimal(dr, "TaskDuration");
                objEntity.ActivityDate = GetDateTime(dr, "ActivityDate");
                objEntity.TaskCategoryName = GetTextVale(dr, "TaskCategoryName");
                objEntity.ToDOID = GetInt64(dr, "ToDOID");
                objEntity.ToDODescription = GetTextVale(dr, "ToDODescription");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.DailyActivity> GetDailyActivityList(Int64 pkID, Int64 EmployeeID, string ActivityDate, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.DailyActivity> lstLocation = new List<Entity.DailyActivity>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DailyActivityList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmdGet.Parameters.AddWithValue("@ActivityDate", Convert.ToDateTime(ActivityDate));
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.DailyActivity objEntity = new Entity.DailyActivity();
                objEntity.pkID = GetInt64(dr, "pkID");

                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskDuration = GetDecimal(dr, "TaskDuration");
                objEntity.ActivityDate = GetDateTime(dr, "ActivityDate");
                objEntity.TaskCategoryName = GetTextVale(dr, "TaskCategoryName");
                objEntity.ToDOID = GetInt64(dr, "ToDOID");
                objEntity.ToDODescription = GetTextVale(dr, "ToDODescription");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.DailyActivity> GetDailyActivityListByUser(string LoginUserID, string ActivityDate, Int64 pMonth, Int64 pYear, string FromDate = null, string ToDate=null)
        {
            List<Entity.DailyActivity> lstLocation = new List<Entity.DailyActivity>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DailyActivityListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ActivityDate", ActivityDate);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.DailyActivity objEntity = new Entity.DailyActivity();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskDuration = GetDecimal(dr, "TaskDuration");
                objEntity.ActivityDate = GetDateTime(dr, "ActivityDate");
                objEntity.TaskCategoryName = GetTextVale(dr, "TaskCategoryName");
                objEntity.ToDOID = GetInt64(dr, "ToDOID");
                objEntity.ToDODescription = GetTextVale(dr, "ToDODescription");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateDailyActivity(Entity.DailyActivity objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "DailyActivity_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ActivityDate", objEntity.ActivityDate);
            cmdAdd.Parameters.AddWithValue("@TaskDescription", objEntity.TaskDescription);
            cmdAdd.Parameters.AddWithValue("@TaskCategoryID", objEntity.TaskCategoryID);
            cmdAdd.Parameters.AddWithValue("@TaskDuration", objEntity.TaskDuration);
            cmdAdd.Parameters.AddWithValue("@ToDOID", objEntity.ToDOID);
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

        public virtual void DeleteDailyActivity(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DailyActivity_DEL";
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

        public virtual List<Entity.TaskCategory> GetTaskCategoryList(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.TaskCategory> lstLocation = new List<Entity.TaskCategory>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "TaskCategoryList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Category", "TODO");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.TaskCategory objEntity = new Entity.TaskCategory();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TaskCategoryName = GetTextVale(dr, "TaskCategoryName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.TaskCategory> GetTaskCategoryList(Int64 pkID, string pCategory, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.TaskCategory> lstLocation = new List<Entity.TaskCategory>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "TaskCategoryList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.TaskCategory objEntity = new Entity.TaskCategory();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TaskCategoryName = GetTextVale(dr, "TaskCategoryName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
