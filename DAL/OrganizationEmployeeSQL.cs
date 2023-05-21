using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OrganizationEmployeeSQL : BaseSqlManager
    {
        public virtual List<Entity.OrganizationEmployee> GetEmployeeExpnLedgerList(Int64 pEmployeeID, Int64 pMonth, Int64 pYear, string pLoginUserID)
        {
            List<Entity.OrganizationEmployee> lstLocation = new List<Entity.OrganizationEmployee>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeExpenseLedger";
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TransDate = GetDateTime(dr, "ExpenseDate");
                objEntity.Description = GetTextVale(dr, "ExpenseNotes");
                objEntity.TransCategory = GetTextVale(dr, "Category");
                objEntity.TransType = GetTextVale(dr, "ExpenseTypeName");
                objEntity.DebitAmount = GetDecimal(dr, "DebitAmount");
                objEntity.CreditAmount = GetDecimal(dr, "CreditAmount");
                lstLocation.Add(objEntity);
            }
            dr.Close();

            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.OrganizationEmployee> GetOrgEmployeeByRegion(Int64 pStateCode, Int64 pCityCode, string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgEmployeeByRegion";
            cmdGet.Parameters.AddWithValue("@StateCode", pStateCode);
            cmdGet.Parameters.AddWithValue("@CityCode", pCityCode);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 1000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetOrganizationEmployeeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgEmployeeList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CardNo = GetInt64(dr, "CardNo");

                objEntity.Landline = GetTextVale(dr, "Landline");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmailPassword = GetTextVale(dr, "EmailPassword");

                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.BasicPer = GetTextVale(dr, "BasicPer");
                objEntity.Gender = GetTextVale(dr, "Gender");
                objEntity.WorkingHours = GetTextVale(dr, "WorkingHours");

                objEntity.WorkingHours = GetTextVale(dr, "WorkingHours");
                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");

                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.FixedBasic = GetDecimal(dr, "FixedBasic");
                objEntity.FixedHRA = GetDecimal(dr, "FixedHRA");
                objEntity.FixedDA = GetDecimal(dr, "FixedDA");
                objEntity.FixedConv = GetDecimal(dr, "FixedConv");
                objEntity.FixedSpecial = GetDecimal(dr, "FixedSpecial");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.ConfirmationDate = GetDateTime(dr, "ConfirmationDate");
                objEntity.JoiningDate = GetDateTime(dr, "JoinDate");
                objEntity.ReleaseDate = GetDateTime(dr, "ReleaseDate");
                objEntity.AuthorizedSign = GetTextVale(dr, "AuthorizedSign");
                objEntity.EmployeeImage = GetTextVale(dr, "EmployeeImage");

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankBranch = GetTextVale(dr, "BankBranch");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");

                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.PassportNo = GetTextVale(dr, "PassportNo");
                objEntity.AadharCardNo = GetTextVale(dr, "AadharCardNo");
                objEntity.PANCardNo = GetTextVale(dr, "PANCardNo");
                objEntity.PF_Calculation = GetBoolean(dr, "PF_Calculation");
                objEntity.PT_Calculation = GetBoolean(dr, "PT_Calculation");
                objEntity.eSignaturePath = GetTextVale(dr, "eSignaturePath");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetEmployeeList(string pEmployeeName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeListByName";
            cmdGet.Parameters.AddWithValue("@EmployeeName", pEmployeeName);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 1000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetOrgEmployeeByOrgName(string OrgName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgEmployeeByOrgName";
            cmdGet.Parameters.AddWithValue("@OrgName", OrgName);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> OrgEmployeeByMobileNumber(string EmployeeName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgEmployeeByMobileNumber";
            cmdGet.Parameters.AddWithValue("@EmployeeName", EmployeeName);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetOrganizationEmployeeList(long pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgEmployeeList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CardNo = GetInt64(dr, "CardNo");

                objEntity.Landline = GetTextVale(dr, "Landline");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmailPassword = GetTextVale(dr, "EmailPassword");

                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.BasicPer = GetTextVale(dr, "BasicPer");
                objEntity.Gender = GetTextVale(dr, "Gender");
                objEntity.WorkingHours = GetTextVale(dr, "WorkingHours");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");

                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.FixedBasic = GetDecimal(dr, "FixedBasic");
                objEntity.FixedHRA = GetDecimal(dr, "FixedHRA");
                objEntity.FixedDA = GetDecimal(dr, "FixedDA");
                objEntity.FixedConv = GetDecimal(dr, "FixedConv");
                objEntity.FixedSpecial = GetDecimal(dr, "FixedSpecial");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.ConfirmationDate = GetDateTime(dr, "ConfirmationDate");
                objEntity.JoiningDate = GetDateTime(dr, "JoinDate");
                objEntity.ReleaseDate = GetDateTime(dr, "ReleaseDate");
                objEntity.AuthorizedSign = GetTextVale(dr, "AuthorizedSign");
                objEntity.EmployeeImage = GetTextVale(dr, "EmployeeImage");

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankBranch = GetTextVale(dr, "BankBranch");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");

                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.PassportNo = GetTextVale(dr, "PassportNo");
                objEntity.AadharCardNo = GetTextVale(dr, "AadharCardNo");
                objEntity.PANCardNo = GetTextVale(dr, "PANCardNo");

                objEntity.PF_Calculation = GetBoolean(dr, "PF_Calculation");
                objEntity.PT_Calculation = GetBoolean(dr, "PT_Calculation");
                objEntity.eSignaturePath = GetTextVale(dr, "eSignaturePath");

                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.Pincode = GetTextVale(dr, "EmpPincode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");

                objEntity.MaritalStatus = GetTextVale(dr, "MaritalStatus");
                objEntity.BloodGroup = GetTextVale(dr, "BloodGroup");
                objEntity.ESICNo = GetTextVale(dr, "ESICNo");
                objEntity.PFAccountNo = GetTextVale(dr, "PFAccountNo");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetOrganizationEmployeeList(string OrgCode, string pLoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationEmployee> lstLocation = new List<Entity.OrganizationEmployee>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationEmployeeList";
            cmdGet.Parameters.AddWithValue("@OrgCode", OrgCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CardNo = GetInt64(dr, "CardNo");

                objEntity.Landline = GetTextVale(dr, "Landline");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmailPassword = GetTextVale(dr, "EmailPassword");

                objEntity.ShiftCode = GetInt64(dr, "ShiftCode");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.Gender = GetTextVale(dr, "Gender");
                objEntity.WorkingHours = GetTextVale(dr, "WorkingHours");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");
                
                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");

                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.FixedBasic = GetDecimal(dr, "FixedBasic");
                objEntity.FixedHRA = GetDecimal(dr, "FixedHRA");
                objEntity.FixedDA = GetDecimal(dr, "FixedDA");
                objEntity.FixedConv = GetDecimal(dr, "FixedConv");
                objEntity.FixedSpecial = GetDecimal(dr, "FixedSpecial");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.ConfirmationDate = GetDateTime(dr, "ConfirmationDate");
                objEntity.JoiningDate = GetDateTime(dr, "JoinDate");
                objEntity.ReleaseDate = GetDateTime(dr, "ReleaseDate");

                objEntity.AuthorizedSign = GetTextVale(dr, "AuthorizedSign");
                objEntity.EmployeeImage = GetTextVale(dr, "EmployeeImage");

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankBranch = GetTextVale(dr, "BankBranch");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");

                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.PassportNo = GetTextVale(dr, "PassportNo");
                objEntity.AadharCardNo = GetTextVale(dr, "AadharCardNo");
                objEntity.PANCardNo = GetTextVale(dr, "PANCardNo");
                objEntity.PF_Calculation = GetBoolean(dr, "PF_Calculation");
                objEntity.PT_Calculation = GetBoolean(dr, "PT_Calculation");
                objEntity.eSignaturePath = GetTextVale(dr, "eSignaturePath");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OrganizationEmployee> GetOrganizationEmployeeList(string OrgCode, string pLoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationEmployee> lstLocation = new List<Entity.OrganizationEmployee>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationEmployeeList";
            cmdGet.Parameters.AddWithValue("@OrgCode", OrgCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
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
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CardNo = GetInt64(dr, "CardNo");

                objEntity.Landline = GetTextVale(dr, "Landline");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmailPassword = GetTextVale(dr, "EmailPassword");
                objEntity.Gender = GetTextVale(dr, "Gender");
                objEntity.ShiftName = GetTextVale(dr, "ShiftName");
                objEntity.BasicPer = GetTextVale(dr, "BasicPer");
                objEntity.WorkingHours = GetTextVale(dr, "WorkingHours");

                objEntity.DesigCode = GetTextVale(dr, "DesigCode");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.OrgCode = GetTextVale(dr, "OrgCode");
                objEntity.OrgName = GetTextVale(dr, "OrgName");

                objEntity.ReportTo = GetInt64(dr, "ReportTo");
                objEntity.ReportToEmployeeName = GetTextVale(dr, "ReportToEmployeeName");

                objEntity.FixedSalary = GetDecimal(dr, "FixedSalary");
                objEntity.FixedBasic = GetDecimal(dr, "FixedBasic");
                objEntity.FixedHRA = GetDecimal(dr, "FixedHRA");
                objEntity.FixedDA = GetDecimal(dr, "FixedDA");
                objEntity.FixedConv = GetDecimal(dr, "FixedConv");
                objEntity.FixedSpecial = GetDecimal(dr, "FixedSpecial");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.ConfirmationDate = GetDateTime(dr, "ConfirmationDate");
                objEntity.JoiningDate = GetDateTime(dr, "JoinDate");
                objEntity.ReleaseDate = GetDateTime(dr, "ReleaseDate");

                objEntity.AuthorizedSign = GetTextVale(dr, "AuthorizedSign");
                objEntity.EmployeeImage = GetTextVale(dr, "EmployeeImage");

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankBranch = GetTextVale(dr, "BankBranch");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");

                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.PassportNo = GetTextVale(dr, "PassportNo");
                objEntity.AadharCardNo = GetTextVale(dr, "AadharCardNo");
                objEntity.PANCardNo = GetTextVale(dr, "PANCardNo");

                objEntity.PF_Calculation = GetBoolean(dr, "PF_Calculation");
                objEntity.PT_Calculation = GetBoolean(dr, "PT_Calculation");
                objEntity.eSignaturePath = GetTextVale(dr, "eSignaturePath");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        
        public virtual List<Entity.OrganizationEmployee> GetEmployeeFollowerList(string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeFollowerList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.LoginUserID = GetTextVale(dr, "UserID");
                objEntity.TokenNo = GetTextVale(dr, "TokenNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetEmployeeSupervisorList(string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeSupervisorList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.LoginUserID = GetTextVale(dr, "UserID");
                objEntity.TokenNo = GetTextVale(dr, "TokenNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetEmployeeListByRole(string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeListByRole";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OrganizationEmployee> GetEmployeeWorkPerfomance(string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeWorkPerfomance";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");
                objEntity.Inquiry = GetInt64(dr, "Inquiry");
                objEntity.Quotation = GetInt64(dr, "Quotation");
                objEntity.Followup = GetInt64(dr, "Followup");
                objEntity.SalesOrder = GetInt64(dr, "SalesOrder");
                objEntity.SalesBill = GetInt64(dr, "SalesBill");
                objEntity.PurcBill = GetInt64(dr, "PurcBill");
                objEntity.Customers = GetInt64(dr, "Customers");
                objEntity.Follower = GetInt64(dr, "Follower");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateOrganizationEmployee(Entity.OrganizationEmployee objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OrganizationEmployee_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeName", objEntity.EmployeeName);
            cmdAdd.Parameters.AddWithValue("@CardNo", objEntity.CardNo);
            cmdAdd.Parameters.AddWithValue("@Landline", objEntity.Landline);
            cmdAdd.Parameters.AddWithValue("@MobileNo", objEntity.MobileNo);
            cmdAdd.Parameters.AddWithValue("@EmailAddress", objEntity.EmailAddress);
            cmdAdd.Parameters.AddWithValue("@EmailPassword", objEntity.EmailPassword);
            cmdAdd.Parameters.AddWithValue("@EmployeeImage", objEntity.EmployeeImage);
            cmdAdd.Parameters.AddWithValue("@WorkingHours", objEntity.WorkingHours);
            cmdAdd.Parameters.AddWithValue("@Gender", objEntity.Gender);
            cmdAdd.Parameters.AddWithValue("@BasicPer", objEntity.BasicPer);
            cmdAdd.Parameters.AddWithValue("@ShiftCode", objEntity.ShiftCode);
            cmdAdd.Parameters.AddWithValue("@DesigCode", objEntity.DesigCode);
            cmdAdd.Parameters.AddWithValue("@OrgCode", objEntity.OrgCode);
            cmdAdd.Parameters.AddWithValue("@ReportTo", objEntity.ReportTo);
            cmdAdd.Parameters.AddWithValue("@FixedSalary", objEntity.FixedSalary);
            cmdAdd.Parameters.AddWithValue("@FixedBasic", objEntity.FixedBasic);
            cmdAdd.Parameters.AddWithValue("@FixedHRA", objEntity.FixedHRA);
            cmdAdd.Parameters.AddWithValue("@FixedDA", objEntity.FixedDA);
            cmdAdd.Parameters.AddWithValue("@FixedConv", objEntity.FixedConv);
            cmdAdd.Parameters.AddWithValue("@FixedSpecial", objEntity.FixedSpecial);
            if (objEntity.BirthDate.Year >= 1900)
                cmdAdd.Parameters.AddWithValue("@BirthDate", objEntity.BirthDate);
            if (objEntity.ConfirmationDate.Year >= 1900)
                cmdAdd.Parameters.AddWithValue("@ConfirmationDate", objEntity.ConfirmationDate);
            if (objEntity.JoiningDate.Year >= 1900)
                cmdAdd.Parameters.AddWithValue("@JoinDate", objEntity.JoiningDate);
            if (objEntity.ReleaseDate.Year>=1900)
                cmdAdd.Parameters.AddWithValue("@ReleaseDate", objEntity.ReleaseDate);

            cmdAdd.Parameters.AddWithValue("@BankName", objEntity.BankName);
            cmdAdd.Parameters.AddWithValue("@BankBranch", objEntity.BankBranch);
            cmdAdd.Parameters.AddWithValue("@BankAccountNo", objEntity.BankAccountNo);
            cmdAdd.Parameters.AddWithValue("@BankIFSC", objEntity.BankIFSC);

            cmdAdd.Parameters.AddWithValue("@DrivingLicenseNo", objEntity.DrivingLicenseNo);
            cmdAdd.Parameters.AddWithValue("@PassportNo", objEntity.PassportNo);
            cmdAdd.Parameters.AddWithValue("@AadharCardNo", objEntity.AadharCardNo);
            cmdAdd.Parameters.AddWithValue("@PANCardNo", objEntity.PANCardNo);

            cmdAdd.Parameters.AddWithValue("@PF_Calculation", objEntity.PF_Calculation);
            cmdAdd.Parameters.AddWithValue("@PT_Calculation", objEntity.PT_Calculation);
            cmdAdd.Parameters.AddWithValue("@EmpCode", objEntity.EmpCode
                );

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            cmdAdd.Parameters.AddWithValue("@AuthorizedSign", objEntity.AuthorizedSign);
            cmdAdd.Parameters.AddWithValue("@eSignaturePath", objEntity.eSignaturePath);

            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@Area", objEntity.Area);
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);

            cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);
            cmdAdd.Parameters.AddWithValue("@MaritalStatus", objEntity.MaritalStatus);
            cmdAdd.Parameters.AddWithValue("@BloodGroup", objEntity.BloodGroup);
            cmdAdd.Parameters.AddWithValue("@ESICNo", objEntity.ESICNo);
            cmdAdd.Parameters.AddWithValue("@PFAccountNo", objEntity.PFAccountNo);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnpkID", SqlDbType.Int);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnpkID = Convert.ToInt32(cmdAdd.Parameters["@ReturnpkID"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteOrganizationEmployee(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OrganizationEmployee_DEL";
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
        // **********************************************************************
        // Employee Documents
        // **********************************************************************
        public virtual List<Entity.Documents> GetEmployeeDocumentsList(Int64 pkID, Int64 pEmployeeID)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeDocumentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Documents> lstLocation = new List<Entity.Documents>();
            while (dr.Read())
            {
                Entity.Documents objLocation = new Entity.Documents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.EmployeeID = GetInt64(dr, "EmployeeID");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.FileName = GetTextVale(dr, "Name");
                objLocation.FileType = GetTextVale(dr, "Type");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateEmployeeDocuments(Int64 pEmployeeID, string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "insert into MST_Employee_Documents (EmployeeID, Name,type,createdby)" + " values (@EmployeeID, @Name, @type, @LoginUserID)";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@EmployeeID", SqlDbType.BigInt).Value = pEmployeeID;
                cmdAdd.Parameters.Add("@Name", SqlDbType.VarChar).Value = pFilename;
                cmdAdd.Parameters.Add("@type", SqlDbType.VarChar).Value = pType;
                cmdAdd.Parameters.Add("@LoginUserID", SqlDbType.VarChar).Value = pLoginUserID;
                ExecuteNonQuery(cmdAdd);

                ReturnCode = 1;
                ReturnMsg = "File Uploaded Successfully !";
            }
            catch (Exception ex)
            {
                ReturnCode = 0;
                ReturnMsg = ex.Message.ToString();
            }
            ForceCloseConncetion();
        }

        public virtual void DeleteEmployeeDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "EmployeeDocuments_DEL";
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

        public virtual void DeleteEmployeeDocumentsByEmployeeId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "EmployeeDocumentsByEmployeeId_DEL";
            cmdDel.Parameters.AddWithValue("@EmployeeID", pkID);
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

        // ================================= Employee Credentials Vault
        public virtual List<Entity.OrganizationEmployee> GetOrganizationEmployeeCredentials(Int64 pEmployeeID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgEmployeeCredentials";
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstObject = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objEntity = new Entity.OrganizationEmployee();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RefEmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.Description = GetTextVale(dr, "Description");
                objEntity.UserID = GetTextVale(dr, "UserID");
                objEntity.UserPassword = GetTextVale(dr, "UserPassword");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateEmployeeCredentials(Entity.OrganizationEmployee objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OrgEmployeeCredentials_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.RefEmployeeID);
            cmdAdd.Parameters.AddWithValue("@Description", objEntity.Description);
            cmdAdd.Parameters.AddWithValue("@UserID", objEntity.UserID);
            cmdAdd.Parameters.AddWithValue("@UserPassword", objEntity.UserPassword);
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

        public virtual void DeleteEmployeeCredentials(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OrgEmployeeCredentials_DEL";
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
