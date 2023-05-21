using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class PayrollMgmt
    {
        public static List<Entity.Payroll> GetPayrollList(long pkID, long Month, long Year, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PayrollSQL().GetPayrollList(pkID, Month, Year, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Payroll> GetPayrollList(long pkID, string SearchKey, long Month, long Year, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PayrollSQL().GetPayrollList(pkID, SearchKey, Month, Year, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdatePayroll(Entity.Payroll entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PayrollSQL().AddUpdatePayroll(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeletePayroll(long pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.PayrollSQL().DeletePayroll(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.Payroll> GeneratePayrollList(long pkID, long Month, long Year, Boolean ForceGenerate, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PayrollSQL().GeneratePayrollList(pkID, Month, Year, ForceGenerate, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Payroll> GeneratePayrollListForEmployee(long EmployeeID, long Month, long Year, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.PayrollSQL().GeneratePayrollListForEmployee(EmployeeID, Month, Year, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
    }
}
