using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AttendanceSQL : BaseSqlManager
    {

        public virtual List<Entity.Attendance> GetAttendanceList(Int64 pkID, Int64 EmployeeID, Int64 pMon, Int64 pYear)
        {
            List<Entity.Attendance> lstLocation = new List<Entity.Attendance>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DailyAttendanceList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmdGet.Parameters.AddWithValue("@Month", pMon);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Attendance objEntity = new Entity.Attendance();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PresenceDate = GetDateTime(dr, "PresenceDate");
                objEntity.TimeIn = GetTextVale(dr, "TimeIn");
                objEntity.TimeOut = GetTextVale(dr, "TimeOut");
                objEntity.Notes = GetTextVale(dr, "Notes");
                objEntity.WorkingHrs = GetDecimal(dr, "WorkingHrs");
                objEntity.WorkingMins = GetDecimal(dr, "WorkingMins");
                objEntity.WorkingTotalHrs = GetDecimal(dr, "WorkingTotalHrs");
                objEntity.WorkingHrsFlag = GetDecimal(dr, "WorkingHrsFlag");
                objEntity.OTHrs = GetDecimal(dr, "OTHrs");
                objEntity.GraceMins = GetDecimal(dr, "GraceMins");
                objEntity.ShiftTotalHrs = GetDecimal(dr, "ShiftTotalHrs");
                objEntity.LunchFrom = GetTextVale(dr, "LunchFrom");
                objEntity.LunchTo = GetTextVale(dr, "LunchTo");

                objEntity.DayStatus = GetTextVale(dr, "DayStatus");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Attendance> GetLatePunchList(string pLoginUserID, Int64 pMon, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            List<Entity.Attendance> lstLocation = new List<Entity.Attendance>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LatePunchList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMon);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Attendance objEntity = new Entity.Attendance();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PresenceDate = GetDateTime(dr, "PresenceDate");
                objEntity.TimeIn = GetTextVale(dr, "TimeIn");
                objEntity.TimeOut = GetTextVale(dr, "TimeOut");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateAttendance(Entity.Attendance objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "DailyAttendance_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@PresenceDate", objEntity.PresenceDate);
            cmdAdd.Parameters.AddWithValue("@TimeIn", objEntity.TimeIn);
            cmdAdd.Parameters.AddWithValue("@TimeOut", objEntity.TimeOut);
            cmdAdd.Parameters.AddWithValue("@Notes", objEntity.Notes);
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
        // ============================= Delete
        public virtual void DeleteAttendance(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DailyAttendance_DEL";
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

        public virtual void AddUpdateBiometricAttendance(Entity.Attendance objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "DailyAttendanceBiometric_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@CardNo", objEntity.CardNo);
            cmdAdd.Parameters.AddWithValue("@PresenceDate", objEntity.PresenceDate);
            cmdAdd.Parameters.AddWithValue("@TimeIn", objEntity.TimeIn);
            cmdAdd.Parameters.AddWithValue("@TimeOut", objEntity.TimeOut);
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

        // -----------------------------------------------------------------------
        public virtual List<Entity.MissedPunch> GetMissedPunchList(Int64 pkID, string pLoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.MissedPunch> lstLocation = new List<Entity.MissedPunch>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MissedPunchList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.MissedPunch objEntity = new Entity.MissedPunch();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PresenceDate = GetDateTime(dr, "PresenceDate");
                objEntity.TimeIn = GetTextVale(dr, "TimeIn");
                objEntity.TimeOut = GetTextVale(dr, "TimeOut");
                objEntity.Notes = GetTextVale(dr, "Notes");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");
                objEntity.ApprovedOn = GetTextVale(dr, "ApprovedOn");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateMissedPunch(Entity.MissedPunch objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "MissedPunch_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@PresenceDate", objEntity.PresenceDate);
            cmdAdd.Parameters.AddWithValue("@TimeIn", objEntity.TimeIn);
            cmdAdd.Parameters.AddWithValue("@TimeOut", objEntity.TimeOut);
            cmdAdd.Parameters.AddWithValue("@Notes", objEntity.Notes);
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

        public virtual void DeleteMissedPunch(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "MissedPunch_DEL";
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
        public virtual List<Entity.MissedPunch> GetMissedPunchApprovalList(string pStatus, string pLoginUserID)
        {
            List<Entity.MissedPunch> lstLocation = new List<Entity.MissedPunch>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MissedPunchList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 99999);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.MissedPunch objEntity = new Entity.MissedPunch();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PresenceDate = GetDateTime(dr, "PresenceDate");
                objEntity.TimeIn = GetTextVale(dr, "TimeIn");
                objEntity.TimeOut = GetTextVale(dr, "TimeOut");
                objEntity.Notes = GetTextVale(dr, "Notes");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");
                objEntity.ApprovedOn = GetTextVale(dr, "ApprovedOn");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void UpdateMissedPunchApproval(Entity.MissedPunch objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "MissedPunchApproval_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
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
    }
}
