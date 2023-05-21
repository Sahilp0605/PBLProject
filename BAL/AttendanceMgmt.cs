using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class AttendanceMgmt
    {
        public static List<Entity.Attendance> GetAttendanceList(Int64 pkID, Int64 EmployeeID, Int64 pMon, Int64 pYear)
        {
            return (new DAL.AttendanceSQL().GetAttendanceList(pkID, EmployeeID, pMon, pYear));
        }
        
        public static List<Entity.Attendance> GetLatePunchList(String pLoginUserID, Int64 pMon, Int64 pYear)
        {
            return (new DAL.AttendanceSQL().GetLatePunchList(pLoginUserID, pMon, pYear));
        }

        public static List<Entity.Attendance> GetLatePunchListPeriod(String pLoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.AttendanceSQL().GetLatePunchList(pLoginUserID, 0, 0, FromDate, ToDate));
        }

        public static void AddUpdateAttendance(Entity.Attendance entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AttendanceSQL().AddUpdateAttendance(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteAttendance(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AttendanceSQL().DeleteAttendance(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateBiometricAttendance(Entity.Attendance entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AttendanceSQL().AddUpdateBiometricAttendance(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.MissedPunch> GetMissedPunchList(Int64 pkID, string pLoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.AttendanceSQL().GetMissedPunchList(pkID, pLoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMissedPunch(Entity.MissedPunch entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AttendanceSQL().AddUpdateMissedPunch(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMissedPunch(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AttendanceSQL().DeleteMissedPunch(pkID, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.MissedPunch> GetMissedPunchApprovalList(string pStatus, string pLoginUserID)
        {
            return (new DAL.AttendanceSQL().GetMissedPunchApprovalList(pStatus, pLoginUserID));
        }

        public static void UpdateMissedPunchApproval(Entity.MissedPunch entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AttendanceSQL().UpdateMissedPunchApproval(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
