using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class LoanSQL : BaseSqlManager
    {
        public virtual List<Entity.Loan> GetLoanList(String LoanCategory, String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LoanInstallmentsList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoanCategory", LoanCategory);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Loan> lstObject = new List<Entity.Loan>();
            while (dr.Read())
            {
                Entity.Loan objEntity = new Entity.Loan();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RowNum = GetInt64(dr, "RowNum");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.LoanAmount = GetDecimal(dr, "LoanAmount");
                objEntity.NoOfInstallments = GetInt32(dr, "NoOfInstallments");
                objEntity.InstallmentAmount = GetDecimal(dr, "InstallmentAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");
                objEntity.ApprovedOn = GetDateTime(dr, "ApprovedOn");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.Loan> GetLoan(String LoanCategory, Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageBatch, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LoanInstallmentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoanCategory", LoanCategory);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageBatch);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Loan> lstObject = new List<Entity.Loan>();
            while (dr.Read())
            {
                Entity.Loan objEntity = new Entity.Loan();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RowNum = GetInt64(dr, "RowNum");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.LoanAmount = GetDecimal(dr, "LoanAmount");
                objEntity.NoOfInstallments = GetInt32(dr, "NoOfInstallments");
                objEntity.InstallmentAmount = GetDecimal(dr, "InstallmentAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");
                objEntity.ApprovedOn = GetDateTime(dr, "ApprovedOn");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateLoan(Entity.Loan objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "LoanInstallments_INS_UPD";

            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@LoanCategory", objEntity.LoanCategory);
            cmdAdd.Parameters.AddWithValue("@LoanType", objEntity.LoanType);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@LoanAmount", objEntity.LoanAmount);
            cmdAdd.Parameters.AddWithValue("@StartDate", objEntity.StartDate);
            if (objEntity.LoanCategory.ToLower() == "loan")
            {
                cmdAdd.Parameters.AddWithValue("@EndDate", objEntity.EndDate);
                cmdAdd.Parameters.AddWithValue("@NoOfInstallments", objEntity.NoOfInstallments);
                cmdAdd.Parameters.AddWithValue("@InstallmentAmount", objEntity.InstallmentAmount);
            }
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
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

        public virtual void DeleteLoan(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "LoanInstallments_DEL";
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

        public virtual List<Entity.Loan> GetLoanApprovalList(String LoanCategory, String ApprovalStatus, String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LoanInstallmentsList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoanCategory", LoanCategory);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Loan> lstObject = new List<Entity.Loan>();
            while (dr.Read())
            {
                Entity.Loan objEntity = new Entity.Loan();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.RowNum = GetInt64(dr, "RowNum");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.EndDate = GetDateTime(dr, "EndDate");
                objEntity.LoanAmount = GetDecimal(dr, "LoanAmount");
                objEntity.NoOfInstallments = GetInt32(dr, "NoOfInstallments");
                objEntity.InstallmentAmount = GetDecimal(dr, "InstallmentAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");
                objEntity.ApprovedOn = GetDateTime(dr, "ApprovedOn");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void UpdateLoanApproval(Entity.Loan objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "LoanApproval_UPD";
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

        public virtual Decimal GetLoanInstallmentAmount(String pLoanCategory, Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        {
            Decimal retValue = 0;
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmployeeLoanInstallment";
            cmdGet.Parameters.AddWithValue("@LoanCategory", pLoanCategory);
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Loan> lstObject = new List<Entity.Loan>();
            while (dr.Read())
            {
                retValue = GetDecimal(dr, "InstallmentAmount");
            }
            dr.Close();
            ForceCloseConncetion();
            return retValue;
        }
    }
}
