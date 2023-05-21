using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OverTimeSQL: BaseSqlManager
    {
        public virtual List<Entity.OverTime> GetOverTimeList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OverTimeList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OverTime> lstObject = new List<Entity.OverTime>();
            while (dr.Read())
            {
                Entity.OverTime objEntity = new Entity.OverTime();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.ReasonForOT = GetTextVale(dr, "ReasonForOT");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                //objEntity.ApprovedEmployeeID = GetInt64(dr, "ApprovedEmployeeID");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OverTime> GetOverTimeList(Int64 pkID, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OverTime> lstLocation = new List<Entity.OverTime>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OverTimeList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OverTime objEntity = new Entity.OverTime();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.ReasonForOT = GetTextVale(dr, "ReasonForOT");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                //objEntity.ApprovedEmployeeID = GetInt64(dr, "ApprovedEmployeeID");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OverTime> GetOverTimeList(Int64 pkID, string pLoginUserID, string SearchKey, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OverTime> lstLocation = new List<Entity.OverTime>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OverTimeList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OverTime objEntity = new Entity.OverTime();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                //objEntity.ApprovedEmployeeID = GetInt64(dr, "ApprovedEmployeeID");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OverTime> GetOverTimeListByStatus(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OverTime> lstLocation = new List<Entity.OverTime>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OverTimeListByStatus";
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OverTime objEntity = new Entity.OverTime();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.ReasonForOT = GetTextVale(dr, "ReasonForOT");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                //objEntity.ApprovedEmployeeID = GetInt64(dr, "ApprovedEmployeeID");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");
                objEntity.ApprovedDate = GetDateTime(dr, "ApprovedOn");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        //public virtual List<Entity.LeaveRequest> GetLeaveRequestListByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "LeaveRequestListByUser";
        //    cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
        //    cmdGet.Parameters.AddWithValue("@Month", pMonth);
        //    cmdGet.Parameters.AddWithValue("@Year", pYear);
        //    SqlDataReader dr = ExecuteDataReader(cmdGet);
        //    List<Entity.LeaveRequest> lstObject = new List<Entity.LeaveRequest>();
        //    while (dr.Read())
        //    {
        //        Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
        //        objEntity.pkID = GetInt64(dr, "pkID");
        //        objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
        //        objEntity.LeaveType = GetTextVale(dr, "LeaveType");
        //        objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
        //        objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
        //        objEntity.FromDate = GetDateTime(dr, "FromDate");
        //        objEntity.ToDate = GetDateTime(dr, "ToDate");
        //        objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
        //        objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
        //        objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
        //        lstObject.Add(objEntity);
        //    }
        //    dr.Close();
        //    ForceCloseConncetion();
        //    return lstObject;
        //}

        public virtual List<Entity.OverTime> GetOverTimeListByEmployeeID(Int64 pEmpID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OverTimetListByEmployeeID";
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmpID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OverTime> lstObject = new List<Entity.OverTime>();
            while (dr.Read())
            {
                Entity.OverTime objEntity = new Entity.OverTime();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.TotalMinutes = GetInt64(dr, "TotalMinutes");
                objEntity.ReasonForOT = GetTextVale(dr, "ReasonForOT");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateOverTime(Entity.OverTime objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OverTime_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@FromDate", objEntity.FromDate);
            cmdAdd.Parameters.AddWithValue("@ToDate", objEntity.ToDate);
            cmdAdd.Parameters.AddWithValue("@ReasonForOT", objEntity.ReasonForOT);
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

        public virtual void DeleteOverTime(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OverTime_DEL";
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

        public virtual void UpdateOverTimeApproval(Entity.OverTime objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OverTimeApproval_UPD";
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

        //public virtual Decimal GetOverTimeHours(Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        //{
        //    SqlCommand myCommand = new SqlCommand();
        //    myCommand.CommandType = CommandType.Text;
        //    myCommand.CommandText = "select DATEDIFF(minute, FromDate, ToDate) from OverTime Where EmployeeID;";
        //    SqlDataReader dr = ExecuteReader(myCommand);
        //    List<Entity.LeaveRequest> lstLocation = new List<Entity.LeaveRequest>();
        //    while (dr.Read())
        //    {
        //        Entity.LeaveRequest objLocation = new Entity.LeaveRequest();
        //        objLocation.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
        //        objLocation.LeaveType = GetTextVale(dr, "LeaveType");

        //        lstLocation.Add(objLocation);
        //    }
        //    ForceCloseConncetion();
        //    return lstLocation;
        //}

        public static Int64 GetOverTimeHours(Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT DATEDIFF(minute, FromDate, ToDate) From OverTime where EmployeeID=" + pEmployeeID.ToString() + " And Month(FromDate)=" + pMonth.ToString() + " And Year(FromDate)=" + pYear.ToString();
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }
        public static Decimal GetOverTimeAllow(Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT dbo.fnGetOverTime(" + pEmployeeID.ToString() + "," + pMonth.ToString() + "," + pYear.ToString() + ")";
            Decimal varResult = Convert.ToDecimal(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }
    }
}
