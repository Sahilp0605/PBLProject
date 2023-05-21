using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class LeaveRequestMgmt
    {
        public static List<Entity.LeaveRequest> GetLeaveTypes()
        {
            return (new DAL.LeaveRequestSQL().GetLeaveTypes());
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestList(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestList(Int64 pkID, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestList(pkID, pLoginUserID, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestList(Int64 pkID, string pLoginUserID, string SearchKey, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestList(pkID, pLoginUserID, SearchKey, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestListByStatus(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestListByStatus(pStatus, pLoginUserID, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestListByUser(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestListByUser(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestListByUserPeriod(string pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestListByUser(pLoginUserID, 0, 0, FromDate, ToDate));
        }

        public static List<Entity.LeaveRequest> GetLeaveRequestListByEmployeeID(Int64 pEmpID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.LeaveRequestSQL().GetLeaveRequestListByEmployeeID(pEmpID, pMonth, pYear));
        }

        public static void AddUpdateLeaveRequest(Entity.LeaveRequest entity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnLeavePKID)
        {
            new DAL.LeaveRequestSQL().AddUpdateLeaveRequest(entity, out ReturnCode, out ReturnMsg, out ReturnLeavePKID);
        }

        public static void DeleteLeaveRequest(Int64 pkID, string LoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LeaveRequestSQL().DeleteLeaveRequest(pkID, LoginUserID, out ReturnCode, out ReturnMsg);
        }

        public static void UpdateLeaveRequestApproval(Entity.LeaveRequest entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LeaveRequestSQL().UpdateLeaveRequestApproval(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.EmployeeLeaveBalance> GetEmployeeLeaveBalance(Int64 pEmployeeID, Int64 pLeaveTypeID, string pLoginUserID)
        {
            return (new DAL.LeaveRequestSQL().GetEmployeeLeaveBalance(pEmployeeID, pLeaveTypeID, pLoginUserID));
        }
    }
}
