using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PayrollSQL:BaseSqlManager
    {
        public virtual List<Entity.Payroll> GetPayrollList(long pkID, long Month, long Year, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PayrollList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@Month", Month);
            cmdGet.Parameters.AddWithValue("@Year", Year);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Payroll> lstObject = new List<Entity.Payroll>();
            while (dr.Read())
            {
                Entity.Payroll objEntity = new Entity.Payroll();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.BasicPer = GetTextVale(dr, "BasicPer");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.Gender = GetTextVale(dr, "Gender");
                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.MinHrsFullDay = GetDecimal(dr, "MinHrsFullDay");
                objEntity.MinHrsHalfDay = GetDecimal(dr, "MinHrsHalfDay");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");
                objEntity.PayDate = GetDateTime(dr, "PayDate");
                objEntity.WDays = GetInt64(dr, "WDays");
                objEntity.PDays = GetDecimal(dr, "PDays");
                objEntity.LDays = GetDecimal(dr, "LDays");
                objEntity.HDays = GetDecimal(dr, "HDays");
                objEntity.ODays = GetDecimal(dr, "ODays");
                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.Basic = GetDecimal(dr, "Basic");
                objEntity.HRA = GetDecimal(dr, "HRA");
                objEntity.DA = GetDecimal(dr, "DA");
                objEntity.Conveyance = GetDecimal(dr, "Conveyance");
                objEntity.Medical = GetDecimal(dr, "Medical");
                objEntity.Special = GetDecimal(dr, "Special");
                objEntity.OverTime = GetDecimal(dr, "OverTime");
                objEntity.Total_Income = GetDecimal(dr, "Total_Income");

                objEntity.PF = GetDecimal(dr, "PF");
                objEntity.ESI = GetDecimal(dr, "ESI");
                objEntity.PT = GetDecimal(dr, "PT");
                objEntity.TDS = GetDecimal(dr, "TDS");
                objEntity.Loan = GetDecimal(dr, "Loan");
                objEntity.LoanAmt = GetDecimal(dr, "LoanAmt");
                objEntity.Upad = GetDecimal(dr, "Upad");
                objEntity.Total_Deduct = GetDecimal(dr, "Total_Deduct");

                objEntity.NetSalary = GetDecimal(dr, "NetSalary");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Payroll> GetPayrollList(long pkID, string SearchKey, long Month, long Year, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PayrollList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@Month", Month);
            cmdGet.Parameters.AddWithValue("@Year", Year);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Payroll> lstObject = new List<Entity.Payroll>();
            while (dr.Read())
            {
                Entity.Payroll objEntity = new Entity.Payroll();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.BasicPer = GetTextVale(dr, "BasicPer");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.Gender = GetTextVale(dr, "Gender");
                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.MinHrsFullDay = GetDecimal(dr, "MinHrsFullDay");
                objEntity.MinHrsHalfDay = GetDecimal(dr, "MinHrsHalfDay");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");
                objEntity.PayDate = GetDateTime(dr, "PayDate");
                objEntity.WDays = GetInt64(dr, "WDays");
                objEntity.PDays = GetDecimal(dr, "PDays");
                objEntity.LDays = GetDecimal(dr, "LDays");
                objEntity.HDays = GetDecimal(dr, "HDays");
                objEntity.ODays = GetDecimal(dr, "ODays");
                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.Basic = GetDecimal(dr, "Basic");
                objEntity.HRA = GetDecimal(dr, "HRA");
                objEntity.DA = GetDecimal(dr, "DA");
                objEntity.Conveyance = GetDecimal(dr, "Conveyance");
                objEntity.Medical = GetDecimal(dr, "Medical");
                objEntity.Special = GetDecimal(dr, "Special");
                objEntity.OverTime = GetDecimal(dr, "OverTime");
                objEntity.Total_Income = GetDecimal(dr, "Total_Income");

                objEntity.PF = GetDecimal(dr, "PF");
                objEntity.ESI = GetDecimal(dr, "ESI");
                objEntity.PT = GetDecimal(dr, "PT");
                objEntity.TDS = GetDecimal(dr, "TDS");
                objEntity.Loan = GetDecimal(dr, "Loan");
                objEntity.LoanAmt = GetDecimal(dr, "LoanAmt");
                objEntity.Upad = GetDecimal(dr, "Upad");
                objEntity.Total_Deduct = GetDecimal(dr, "Total_Deduct");

                objEntity.NetSalary = GetDecimal(dr, "NetSalary");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        // ============================= Insert & Update
        public virtual void AddUpdatePayroll(Entity.Payroll objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Payroll_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@PayDate", objEntity.PayDate);
            cmdAdd.Parameters.AddWithValue("@WDays", objEntity.WDays);
            cmdAdd.Parameters.AddWithValue("@PDays", objEntity.PDays);
            cmdAdd.Parameters.AddWithValue("@LDays", objEntity.LDays);
            cmdAdd.Parameters.AddWithValue("@ODays", objEntity.ODays);
            cmdAdd.Parameters.AddWithValue("@HDays", objEntity.HDays);
            cmdAdd.Parameters.AddWithValue("@FixedSalary", objEntity.FixedSalary);
            cmdAdd.Parameters.AddWithValue("@Basic", objEntity.Basic);
            cmdAdd.Parameters.AddWithValue("@HRA", objEntity.HRA);
            cmdAdd.Parameters.AddWithValue("@DA", objEntity.DA);
            cmdAdd.Parameters.AddWithValue("@Conveyance", objEntity.Conveyance);
            cmdAdd.Parameters.AddWithValue("@Medical", objEntity.Medical);
            cmdAdd.Parameters.AddWithValue("@Special", objEntity.Special);
            cmdAdd.Parameters.AddWithValue("@OverTime", objEntity.OverTime);
            cmdAdd.Parameters.AddWithValue("@Total_Income", objEntity.Total_Income);
            cmdAdd.Parameters.AddWithValue("@PF", objEntity.PF);
            cmdAdd.Parameters.AddWithValue("@ESI", objEntity.ESI);
            cmdAdd.Parameters.AddWithValue("@PT", objEntity.PT);
            cmdAdd.Parameters.AddWithValue("@TDS", objEntity.TDS);
            cmdAdd.Parameters.AddWithValue("@Loan", objEntity.Loan);
            cmdAdd.Parameters.AddWithValue("@Total_Deduct", objEntity.Total_Deduct);
            cmdAdd.Parameters.AddWithValue("@NetSalary", objEntity.NetSalary);
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

        public virtual void DeletePayroll(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Payroll_DEL";
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

        public virtual List<Entity.Payroll> GeneratePayrollList(long pkID, long Month, long Year, Boolean ForceGenerate, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GeneratePayrollList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Month", Month);
            cmdGet.Parameters.AddWithValue("@Year", Year);
            cmdGet.Parameters.AddWithValue("@ForceGenerate", ForceGenerate);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Payroll> lstObject = new List<Entity.Payroll>();
            while (dr.Read())
            {
                Entity.Payroll objEntity = new Entity.Payroll();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.BasicPer = GetTextVale(dr, "BasicPer");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");
                
                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.WDays = GetInt64(dr, "WDays");
                objEntity.PDays = GetInt64(dr, "PDays");
                objEntity.LDays = GetInt64(dr, "LDays");
                objEntity.ODays = GetDecimal(dr, "ODays");
                objEntity.HDays = GetInt64(dr, "HDays");

                objEntity.Basic = GetDecimal(dr, "Basic");
                objEntity.HRA = GetDecimal(dr, "HRA");
                objEntity.Conveyance = GetDecimal(dr, "Conveyance");
                objEntity.Medical = GetDecimal(dr, "Medical");
                objEntity.Special = GetDecimal(dr, "Special");
                objEntity.OverTime = GetDecimal(dr, "OverTime");
                objEntity.Total_Income = GetDecimal(dr, "Total_Income");

                objEntity.PT = GetDecimal(dr, "PT");
                objEntity.TDS = GetDecimal(dr, "TDS");
                objEntity.Total_Deduct = GetDecimal(dr, "Total_Deduct");

                objEntity.NetSalary = GetDecimal(dr, "NetSalary");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Payroll> GeneratePayrollListForEmployee(long EmployeeID, long Month, long Year, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GeneratePayrollListForEmployee";
            cmdGet.Parameters.AddWithValue("@EmpID", EmployeeID);
            cmdGet.Parameters.AddWithValue("@Month", Month);
            cmdGet.Parameters.AddWithValue("@Year", Year);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Payroll> lstObject = new List<Entity.Payroll>();
            while (dr.Read())
            {
                Entity.Payroll objEntity = new Entity.Payroll();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");

                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.WDays = GetInt64(dr, "WDays");
                objEntity.PDays = GetInt64(dr, "PDays");
                objEntity.LDays = GetInt64(dr, "LDays");
                objEntity.ODays = GetDecimal(dr, "ODays");
                objEntity.HDays = GetInt64(dr, "HDays");

                objEntity.Basic = GetDecimal(dr, "Basic");
                objEntity.HRA = GetDecimal(dr, "HRA");
                objEntity.Conveyance = GetDecimal(dr, "Conveyance");
                objEntity.Medical = GetDecimal(dr, "Medical");
                objEntity.Special = GetDecimal(dr, "Special");
                objEntity.OverTime = GetDecimal(dr, "OverTime");
                objEntity.Total_Income = GetDecimal(dr, "Total_Income");

                objEntity.PT = GetDecimal(dr, "PT");
                objEntity.TDS = GetDecimal(dr, "TDS");
                objEntity.Total_Deduct = GetDecimal(dr, "Total_Deduct");

                objEntity.NetSalary = GetDecimal(dr, "NetSalary");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        // ========================================================

    }
}
