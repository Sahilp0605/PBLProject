using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class LeaveRequestSQL: BaseSqlManager
    {
        public virtual List<Entity.LeaveRequest> GetLeaveTypes()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select pkID As LeaveTypeID, Description As LeaveType from MST_LeaveTypes;";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.LeaveRequest> lstLocation = new List<Entity.LeaveRequest>();
            while (dr.Read())
            {
                Entity.LeaveRequest objLocation = new Entity.LeaveRequest();
                objLocation.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objLocation.LeaveType = GetTextVale(dr, "LeaveType");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.LeaveRequest> GetLeaveRequestList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LeaveRequestList";
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
            List<Entity.LeaveRequest> lstObject = new List<Entity.LeaveRequest>();
            while (dr.Read())
            {
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LeaveDays = GetDecimal(dr, "LeaveDays");
                objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.PaidUnpaid = GetTextVale(dr, "PaidUnpaid");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.LeaveRequest> GetLeaveRequestList(Int64 pkID, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.LeaveRequest> lstLocation = new List<Entity.LeaveRequest>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LeaveRequestList";
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
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LeaveDays = GetDecimal(dr, "LeaveDays");
                objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.PaidUnpaid = GetTextVale(dr, "PaidUnpaid");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.LeaveRequest> GetLeaveRequestList(Int64 pkID, string pLoginUserID, string SearchKey, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.LeaveRequest> lstLocation = new List<Entity.LeaveRequest>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LeaveRequestList";
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
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LeaveDays = GetDecimal(dr, "LeaveDays");
                objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.PaidUnpaid = GetTextVale(dr, "PaidUnpaid");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.LeaveRequest> GetLeaveRequestListByStatus(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.LeaveRequest> lstLocation = new List<Entity.LeaveRequest>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LeaveRequestListByStatus";
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
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LeaveDays = GetDecimal(dr, "LeaveDays");
                objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.PaidUnpaid = GetTextVale(dr, "PaidUnpaid");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.LeaveRequest> GetLeaveRequestListByUser(string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate = null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LeaveRequestListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.LeaveRequest> lstObject = new List<Entity.LeaveRequest>();
            while (dr.Read())
            {
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LeaveDays = GetDecimal(dr, "LeaveDays");
                objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.PaidUnpaid = GetTextVale(dr, "PaidUnpaid");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.LeaveRequest> GetLeaveRequestListByEmployeeID(Int64 pEmpID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LeaveRequestListByEmployeeID";
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmpID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.LeaveRequest> lstObject = new List<Entity.LeaveRequest>();
            while (dr.Read())
            {
                Entity.LeaveRequest objEntity = new Entity.LeaveRequest();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LeaveDays = GetDecimal(dr, "LeaveDays");
                objEntity.TotalMinutes = GetInt64(dr, "TotalMinutes");
                objEntity.TotalLeaveDays = GetInt64(dr, "TotalLeaveDays");
                objEntity.ReasonForLeave = GetTextVale(dr, "ReasonForLeave");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.PaidUnpaid = GetTextVale(dr, "PaidUnpaid");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateLeaveRequest(Entity.LeaveRequest objEntity, out int ReturnCode, out string ReturnMsg,out Int64 ReturnLeavePKID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "LeaveRequest_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@LeaveTypeID", objEntity.LeaveTypeID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@FromDate", objEntity.FromDate);
            cmdAdd.Parameters.AddWithValue("@ToDate", objEntity.ToDate);
            cmdAdd.Parameters.AddWithValue("@ReasonForLeave", objEntity.ReasonForLeave);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnLeavePKID", SqlDbType.BigInt, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnLeavePKID = Convert.ToInt64(cmdAdd.Parameters["@ReturnLeavePKID"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteLeaveRequest(Int64 pkID, string LoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "LeaveRequest_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            cmdDel.Parameters.AddWithValue("@LoginUserID", LoginUserID);
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

        public virtual void UpdateLeaveRequestApproval(Entity.LeaveRequest objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "LeaveRequestApproval_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@PaidUnpaid", objEntity.PaidUnpaid);
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

        public virtual List<Entity.EmployeeLeaveBalance> GetEmployeeLeaveBalance(Int64 pEmployeeID, Int64 pLeaveTypeID, string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeLeaveBalanceList";
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            cmdGet.Parameters.AddWithValue("@LeaveTypeID", pLeaveTypeID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.EmployeeLeaveBalance> lstObject = new List<Entity.EmployeeLeaveBalance>();
            while (dr.Read())
            {
                Entity.EmployeeLeaveBalance objEntity = new Entity.EmployeeLeaveBalance();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.LeaveTypeID = GetInt64(dr, "LeaveTypeID");
                objEntity.LeaveType = GetTextVale(dr, "LeaveType");
                objEntity.LeaveCode = GetTextVale(dr, "LeaveCode");
                objEntity.OpeningBal = GetDecimal(dr, "OpeningBal");
                objEntity.Earned = GetDecimal(dr, "Earned");
                objEntity.Used = GetDecimal(dr, "Used");
                objEntity.ClosingBal = GetDecimal(dr, "ClosingBal");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
    }
}
