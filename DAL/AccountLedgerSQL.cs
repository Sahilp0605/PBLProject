using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AccountLedgerSQL:BaseSqlManager
    {
        public virtual List<Entity.AccountLedger> GetAccountLedgerList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.AccountLedger> lstLocation = new List<Entity.AccountLedger>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "AccountLedgerList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.AccountLedger objEntity = new Entity.AccountLedger();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LedgerCode = GetTextVale(dr, "LedgerCode");
                objEntity.LedgerName = GetTextVale(dr, "LedgerName");
                objEntity.OpenBal = GetDecimal(dr, "OpenBal");
                objEntity.DebitBal = GetDecimal(dr, "DebitBal");
                objEntity.CreditBal = GetDecimal(dr, "CreditBal");
                objEntity.CloseBal = GetDecimal(dr, "CloseBal");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.AccountLedger> GetAccountLedgerByUser(string LoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.AccountLedger> lstLocation = new List<Entity.AccountLedger>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "AccountLedgerListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.AccountLedger objEntity = new Entity.AccountLedger();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LedgerCode = GetTextVale(dr, "LedgerCode");
                objEntity.LedgerName = GetTextVale(dr, "LedgerName");
                objEntity.OpenBal = GetDecimal(dr, "OpenBal");
                objEntity.DebitBal = GetDecimal(dr, "DebitBal");
                objEntity.CreditBal = GetDecimal(dr, "CreditBal");
                objEntity.CloseBal = GetDecimal(dr, "CloseBal");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.AccountLedger> GetDashboardAccountLedgerList(String FollowupStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.AccountLedger> lstLocation = new List<Entity.AccountLedger>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardAccountLedgerList";
            cmdGet.Parameters.AddWithValue("@FollowupStatus", FollowupStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.AccountLedger objEntity = new Entity.AccountLedger();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LedgerCode = GetTextVale(dr, "LedgerCode");
                objEntity.LedgerName = GetTextVale(dr, "LedgerName");
                objEntity.OpenBal = GetDecimal(dr, "OpenBal");
                objEntity.DebitBal = GetDecimal(dr, "DebitBal");
                objEntity.CreditBal = GetDecimal(dr, "CreditBal");
                objEntity.CloseBal = GetDecimal(dr, "CloseBal");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Insert & Update
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual void AddUpdateAccountLedger(Entity.AccountLedger objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "AccountLedger_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@LedgerCode", objEntity.LedgerCode);
            cmdAdd.Parameters.AddWithValue("@LedgerName", objEntity.LedgerName);
            cmdAdd.Parameters.AddWithValue("@OpenBal", objEntity.OpenBal);
            cmdAdd.Parameters.AddWithValue("@DebitBal", objEntity.DebitBal);
            cmdAdd.Parameters.AddWithValue("@CreditBal", objEntity.CreditBal);
            cmdAdd.Parameters.AddWithValue("@CloseBal", objEntity.CloseBal);
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

        public virtual void DeleteAccountLedger(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "AccountLedger_DEL";
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
