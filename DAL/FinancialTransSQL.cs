using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class FinancialTransSQL:BaseSqlManager
    {
        public virtual List<Entity.FinancialTrans> GetFinancialTransList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.FinancialTrans> lstLocation = new List<Entity.FinancialTrans>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "FinancialTransList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.FinancialTrans objEntity = new Entity.FinancialTrans();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherType = GetTextVale(dr, "VoucherType");
                objEntity.RecPay = GetTextVale(dr, "RecPay");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.AccountID = GetInt64(dr, "AccountID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.TDSAccountID = GetInt64(dr, "TDSAccountID");
                objEntity.TDSAmount = GetDecimal(dr, "TDSAmount");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TransType = GetTextVale(dr, "TransType");
                objEntity.TransModeID = GetInt64(dr, "TransModeID");
                objEntity.TransID = GetTextVale(dr, "TransID");
                objEntity.TransDate = GetDateTime(dr, "TransDate");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.Remark = GetTextVale(dr, "Remark");
                objEntity.TerminationOfDelivery = GetInt64(dr, "TerminationOfDelivery");
                objEntity.RDURD = GetTextVale(dr, "RDURD");
                objEntity.TaxPer = GetDecimal(dr, "TaxPer");
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.GSTAmt = GetDecimal(dr, "GSTAmt");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.AccountName = GetTextVale(dr, "AccountName");
                objEntity.TDSAccountName = GetTextVale(dr, "TDSAccountName");
                objEntity.TransModeName = GetTextVale(dr, "TransModeName");

                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTPer = GetDecimal(dr, "IGSTPer");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.FinancialTrans> GetFinancialTransList(Int64 pkID, string LoginUserID, string SearchKey,string TrType, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.FinancialTrans> lstLocation = new List<Entity.FinancialTrans>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "FinancialTransList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@TrType", TrType);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.FinancialTrans objEntity = new Entity.FinancialTrans();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherType = GetTextVale(dr, "VoucherType");
                objEntity.RecPay = GetTextVale(dr, "RecPay");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.AccountID = GetInt64(dr, "AccountID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.TDSAccountID = GetInt64(dr, "TDSAccountID");
                objEntity.TDSAmount = GetDecimal(dr, "TDSAmount");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TransType = GetTextVale(dr, "TransType");
                objEntity.TransModeID = GetInt64(dr, "TransModeID");
                objEntity.TransID = GetTextVale(dr, "TransID");
                objEntity.TransDate = GetDateTime(dr, "TransDate");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.Remark = GetTextVale(dr, "Remark");
                objEntity.TerminationOfDelivery = GetInt64(dr, "TerminationOfDelivery");
                objEntity.RDURD = GetTextVale(dr, "RDURD");
                objEntity.TaxPer = GetDecimal(dr, "TaxPer");
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.GSTAmt = GetDecimal(dr, "GSTAmt");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.AccountName = GetTextVale(dr, "AccountName");
                objEntity.TDSAccountName = GetTextVale(dr, "TDSAccountName");
                objEntity.TransModeName = GetTextVale(dr, "TransModeName");

                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTPer = GetDecimal(dr, "IGSTPer");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");

                bool hasColumn = false;
                var columns = Enumerable.Range(0, dr.FieldCount).Select(dr.GetName).ToList();
                hasColumn =  columns.Any(s => s == "InvoiceNo") ? true : false;
                if(hasColumn)
                    objEntity.InvoiceNoDetail = GetTextVale(dr, "InvoiceNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.FinancialTrans> GetBankListByName(string pBankName)
        {
            List<Entity.FinancialTrans> lstLocation = new List<Entity.FinancialTrans>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BankListByName";
            cmdGet.Parameters.AddWithValue("@BankName", pBankName);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.FinancialTrans objEntity = new Entity.FinancialTrans();                
                objEntity.BankName = GetTextVale(dr, "BankName");
                
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        //public virtual List<Entity.FinancialTrans> GetFinancialTransByUser(string LoginUserID, string TransCategory, Int64 pMonth, Int64 pYear)
        //{
        //    List<Entity.FinancialTrans> lstLocation = new List<Entity.FinancialTrans>();
        //    SqlCommand cmdGet = new SqlCommand();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "FinancialTransListByUser";
        //    cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
        //    cmdGet.Parameters.AddWithValue("@TransCategory", TransCategory);
        //    cmdGet.Parameters.AddWithValue("@Month", pMonth);
        //    cmdGet.Parameters.AddWithValue("@Year", pYear);
        //    SqlDataReader dr = ExecuteDataReader(cmdGet);
        //    while (dr.Read())
        //    {
        //        Entity.FinancialTrans objEntity = new Entity.FinancialTrans();
        //        objEntity.pkID = GetInt64(dr, "pkID");
        //        objEntity.TransType = GetTextVale(dr, "TransType");
        //        objEntity.TransDate = GetDateTime(dr, "TransDate");
        //        objEntity.CustomerID = GetInt64(dr, "CustomerID");
        //        objEntity.TransMode = GetTextVale(dr, "TransMode");
        //        objEntity.TransAmount = GetDecimal(dr, "TransAmount");
        //        objEntity.TransFrom = GetTextVale(dr, "TransFrom");
        //        objEntity.TransID = GetTextVale(dr, "TransID");
        //        objEntity.TransNotes = GetTextVale(dr, "TransNotes");
        //        lstLocation.Add(objEntity);
        //    }
        //    dr.Close();
        //    ForceCloseConncetion();
        //    return lstLocation;
        //}

        //public virtual List<Entity.FinancialTrans> GetDashboardFinancialTransList(String FollowupStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        //{
        //    List<Entity.FinancialTrans> lstLocation = new List<Entity.FinancialTrans>();
        //    SqlCommand cmdGet = new SqlCommand();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "GetDashboardFinancialTransList";
        //    cmdGet.Parameters.AddWithValue("@FollowupStatus", FollowupStatus);
        //    cmdGet.Parameters.AddWithValue("@Month", pMonth);
        //    cmdGet.Parameters.AddWithValue("@Year", pYear);
        //    cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
        //    cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
        //    cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
        //    SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
        //    p.Direction = ParameterDirection.Output;
        //    cmdGet.Parameters.Add(p);
        //    SqlDataReader dr = ExecuteDataReader(cmdGet);
        //    while (dr.Read())
        //    {
        //        Entity.FinancialTrans objEntity = new Entity.FinancialTrans();
        //        objEntity.pkID = GetInt64(dr, "pkID");
        //        objEntity.TransType = GetTextVale(dr, "TransType");
        //        objEntity.TransDate = GetDateTime(dr, "TransDate");
        //        objEntity.CustomerID = GetInt64(dr, "CustomerID");
        //        objEntity.TransMode = GetTextVale(dr, "TransMode");
        //        objEntity.TransAmount = GetDecimal(dr, "TransAmount");
        //        objEntity.TransFrom = GetTextVale(dr, "TransFrom");
        //        objEntity.TransID = GetTextVale(dr, "TransID");
        //        objEntity.TransNotes = GetTextVale(dr, "TransNotes");
        //        lstLocation.Add(objEntity);
        //    }
        //    dr.Close();
        //    TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
        //    ForceCloseConncetion();
        //    return lstLocation;
        //}
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // ============================= Insert & Update
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual void AddUpdateFinancialTrans(Entity.FinancialTrans objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "FinancialTrans_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherType", objEntity.VoucherType);
            cmdAdd.Parameters.AddWithValue("@RecPay", objEntity.RecPay);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@VoucherDate", objEntity.VoucherDate);
            cmdAdd.Parameters.AddWithValue("@AccountID", objEntity.AccountID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@TDSAccountID", objEntity.TDSAccountID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@TransType", objEntity.TransType);
            cmdAdd.Parameters.AddWithValue("@TransModeID", objEntity.TransModeID);
            cmdAdd.Parameters.AddWithValue("@TransID", objEntity.TransID);
            cmdAdd.Parameters.AddWithValue("@TransDate", objEntity.TransDate);
            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
            cmdAdd.Parameters.AddWithValue("@TDSAmount", objEntity.TDSAmount);
            cmdAdd.Parameters.AddWithValue("@BankName", objEntity.BankName);
            cmdAdd.Parameters.AddWithValue("@Remark", objEntity.Remark);
            cmdAdd.Parameters.AddWithValue("@TerminationOfDelivery", objEntity.TerminationOfDelivery);
            cmdAdd.Parameters.AddWithValue("@RDURD", objEntity.RDURD);
            //cmdAdd.Parameters.AddWithValue("@TaxPer", objEntity.TaxPer);
            cmdAdd.Parameters.AddWithValue("@BasicAmt", objEntity.BasicAmt);
            //cmdAdd.Parameters.AddWithValue("@GSTAmt", objEntity.GSTAmt);
            cmdAdd.Parameters.AddWithValue("@NetAmt", objEntity.NetAmt);

            cmdAdd.Parameters.AddWithValue("@SGSTPer", objEntity.SGSTPer);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTPer", objEntity.CGSTPer);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTPer", objEntity.IGSTPer);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);

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

        public virtual void DeleteFinancialTrans(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "FinancialTrans_DEL";
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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : FINANCIAL TRANSACTION DETAIL
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.FinancialTransDetail> GetFinancialTransDetailList(Int64 ParentID, string InvoiceNo, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.FinancialTransDetail> lstLocation = new List<Entity.FinancialTransDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "FinancialTransDetailList";
            cmdGet.Parameters.AddWithValue("@ParentID", ParentID);
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.FinancialTransDetail objEntity = new Entity.FinancialTransDetail();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherType = GetTextVale(dr, "VoucherType");
                objEntity.RecPay = GetTextVale(dr, "RecPay");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.AccountID = GetInt64(dr, "AccountID");
                objEntity.AccountName = GetTextVale(dr, "AccountName");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Amount = GetDecimal(dr, "Amount");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateFinancialTransDetail(Entity.FinancialTransDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "FinancialTransDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ParentID", objEntity.ParentID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
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

        public virtual void DeleteFinancialTransDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "FinancialTransDetail_DEL";
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

        public virtual void DeleteFinancialTransDetailByParentID(Int64 ParentID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "FinancialTransDetailByParentID_DEL";
            cmdDel.Parameters.AddWithValue("@ParentID", ParentID);
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
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : PETTY CASH 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.JournalVoucher> GetPettyCashList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PettyCashList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JournalVoucher> GetPettyCashList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PettyCashList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdatePettyCash(Entity.JournalVoucher objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PettyCash_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@VoucherDate", objEntity.VoucherDate);
            cmdAdd.Parameters.AddWithValue("@DBCustomerID", objEntity.DBCustomerID);
            cmdAdd.Parameters.AddWithValue("@CRCustomerID", objEntity.CRCustomerID);
            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@DBC", "PCAS");
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnVoucherNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnVoucherNo = cmdAdd.Parameters["@ReturnVoucherNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeletePettyCash(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PettyCash_DEL";
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
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : Debit/Credit Note
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.JournalVoucher> GetDBCRNoteList(Int64 pkID, string DBC, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DBCRNoteList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@DBC", DBC);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JournalVoucher> GetDBCRNoteList(Int64 pkID, string DBC, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DBCRNoteList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@DBC", DBC);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateDBCRNote(Entity.JournalVoucher objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "DBCRNote_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@VoucherDate", objEntity.VoucherDate);
            cmdAdd.Parameters.AddWithValue("@DBCustomerID", objEntity.DBCustomerID);
            cmdAdd.Parameters.AddWithValue("@CRCustomerID", objEntity.CRCustomerID);
            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@DBC", objEntity.DBC);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnVoucherNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnVoucherNo = cmdAdd.Parameters["@ReturnVoucherNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void AddUpdateDBCRNote(Entity.DBNote objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "DBCRNote_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@VoucherDate", objEntity.VoucherDate);
            cmdAdd.Parameters.AddWithValue("@DBCustomerID", objEntity.DBCustomerID);
            cmdAdd.Parameters.AddWithValue("@CRCustomerID", objEntity.CRCustomerID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@BasicAmt", objEntity.BasicAmt);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt", objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@ROffAmt", objEntity.ROffAmt);

            cmdAdd.Parameters.AddWithValue("@ChargeID1", objEntity.ChargeID1);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt1", objEntity.ChargeAmt1);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt1", objEntity.ChargeBasicAmt1);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt1", objEntity.ChargeGSTAmt1);

            cmdAdd.Parameters.AddWithValue("@ChargeID2", objEntity.ChargeID2);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt2", objEntity.ChargeAmt2);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt2", objEntity.ChargeBasicAmt2);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt2", objEntity.ChargeGSTAmt2);

            cmdAdd.Parameters.AddWithValue("@ChargeID3", objEntity.ChargeID3);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt3", objEntity.ChargeAmt3);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt3", objEntity.ChargeBasicAmt3);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt3", objEntity.ChargeGSTAmt3);

            cmdAdd.Parameters.AddWithValue("@ChargeID4", objEntity.ChargeID4);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt4", objEntity.ChargeAmt4);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt4", objEntity.ChargeBasicAmt4);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt4", objEntity.ChargeGSTAmt4);

            cmdAdd.Parameters.AddWithValue("@ChargeID5", objEntity.ChargeID5);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt5", objEntity.ChargeAmt5);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt5", objEntity.ChargeBasicAmt5);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt5", objEntity.ChargeGSTAmt5);

            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
            cmdAdd.Parameters.AddWithValue("@NetAmt", objEntity.NetAmt);

            cmdAdd.Parameters.AddWithValue("@DBC", objEntity.DBC);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnVoucherNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnVoucherNo = cmdAdd.Parameters["@ReturnVoucherNo"].Value.ToString();
            ForceCloseConncetion();
        }
        public virtual List<Entity.DBNote> GetDBCRList(Int64 pkID, string DBC, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.DBNote> lstLocation = new List<Entity.DBNote>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DBCRNoteList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@DBC", DBC);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.DBNote objEntity = new Entity.DBNote();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                //objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.ChargeID1 = GetInt64(dr, "ChargeID1");
                objEntity.ChargeAmt1 = GetDecimal(dr, "ChargeAmt1");
                objEntity.ChargeBasicAmt1 = GetDecimal(dr, "ChargeBasicAmt1");
                objEntity.ChargeGSTAmt1 = GetDecimal(dr, "ChargeGSTAmt1");

                objEntity.ChargeID2 = GetInt64(dr, "ChargeID2");
                objEntity.ChargeAmt2 = GetDecimal(dr, "ChargeAmt2");
                objEntity.ChargeBasicAmt2 = GetDecimal(dr, "ChargeBasicAmt2");
                objEntity.ChargeGSTAmt2 = GetDecimal(dr, "ChargeGSTAmt2");

                objEntity.ChargeID3 = GetInt64(dr, "ChargeID3");
                objEntity.ChargeAmt3 = GetDecimal(dr, "ChargeAmt3");
                objEntity.ChargeBasicAmt3 = GetDecimal(dr, "ChargeBasicAmt3");
                objEntity.ChargeGSTAmt3 = GetDecimal(dr, "ChargeGSTAmt3");

                objEntity.ChargeID4 = GetInt64(dr, "ChargeID4");
                objEntity.ChargeAmt4 = GetDecimal(dr, "ChargeAmt4");
                objEntity.ChargeBasicAmt4 = GetDecimal(dr, "ChargeBasicAmt4");
                objEntity.ChargeGSTAmt4 = GetDecimal(dr, "ChargeGSTAmt4");

                objEntity.ChargeID5 = GetInt64(dr, "ChargeID5");
                objEntity.ChargeAmt5 = GetDecimal(dr, "ChargeAmt5");
                objEntity.ChargeBasicAmt5 = GetDecimal(dr, "ChargeBasicAmt5");
                objEntity.ChargeGSTAmt5 = GetDecimal(dr, "ChargeGSTAmt5");

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                


                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateDBCRNoteDetail(Entity.DBNote_Detail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "DBCRNoteDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);
            cmdAdd.Parameters.AddWithValue("@DBC", objEntity.DBC);
            cmdAdd.Parameters.AddWithValue("@UnitQty", objEntity.UnitQty);
            cmdAdd.Parameters.AddWithValue("@Qty", objEntity.Qty);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Rate", objEntity.Rate);
            cmdAdd.Parameters.AddWithValue("@DiscountPer", objEntity.DiscountPer);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt", objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@NetRate", objEntity.NetRate);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@SGSTPer", objEntity.SGSTPer);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTPer", objEntity.CGSTPer);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTPer", objEntity.IGSTPer);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@AddTaxPer", objEntity.AddTaxPer);
            cmdAdd.Parameters.AddWithValue("@AddTaxAmt", objEntity.AddTaxAmt);
            cmdAdd.Parameters.AddWithValue("@NetAmt", objEntity.NetAmt);
            cmdAdd.Parameters.AddWithValue("@HeaderDiscAmt", objEntity.HeaderDiscAmt);
            cmdAdd.Parameters.AddWithValue("@ForOrderNo", objEntity.ForOrderNo);
           
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
        public virtual void DeleteDBCRNoteDetailByVoucherNo(string VoucherNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DBCRNoteDetailByVoucherNo_DEL";
            cmdDel.Parameters.AddWithValue("@VoucherNo", VoucherNo);
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
        public virtual void DeleteDBCRNote(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DBCRNote_DEL";
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
        public DataTable GetDBCRNoteDetail(string pVoucherNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT qd.ProductID, it.ProductName, it.HSNCode, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, qd.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.UnitQty as UnitQty,qd.Qty as Quantity, ISNULL(it.UnitQuantity,0) As UnitQuantity, qd.Rate as UnitRate,qd.Rate as UnitPrice,qd.DiscountPer as DiscountPercent,qd.NetAmt as NetAmount,qd.ForOrderNo, qd.* From DBCRNote_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.VoucherNo = '" + pVoucherNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : Journal Voucher Master
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.JournalVoucher> GetJournalVoucherList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JournalVoucherList";
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
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JournalVoucher> GetJournalVoucherList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JournalVoucherList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateJournalVoucher(Entity.JournalVoucher objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JournalVoucher_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@VoucherDate", objEntity.VoucherDate);
            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@DBC", "JOUR");
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnVoucherNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnVoucherNo = cmdAdd.Parameters["@ReturnVoucherNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteJournalVoucher(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JournalVoucher_DEL";
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
        // ------------------------------------------------------------
        public virtual List<Entity.JournalVoucherDetail> GetJournalVoucherDetailList(Int64 pkID, string VoucherNo, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucherDetail> lstLocation = new List<Entity.JournalVoucherDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JournalVoucherDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@VoucherNo", VoucherNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucherDetail objEntity = new Entity.JournalVoucherDetail();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.TransType = GetTextVale(dr, "TransType");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }


        public virtual void AddUpdateJournalVoucherDetail(Entity.JournalVoucherDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JournalVoucherDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@TransType", objEntity.TransType);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
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

        public virtual void DeleteJournalVoucherDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JournalVoucherDetail_DEL";
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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : Expense Voucher
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        public virtual List<Entity.JournalVoucher> GetExpenseVoucherList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseVoucherList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");
                objEntity.FileName = GetTextVale(dr, "FileName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JournalVoucher> GetExpenseVoucherList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JournalVoucher> lstLocation = new List<Entity.JournalVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseVoucherList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.VoucherNo = GetTextVale(dr, "VoucherNo");
                objEntity.VoucherDate = GetDateTime(dr, "VoucherDate");
                objEntity.DBCustomerID = GetInt64(dr, "DBCustomerID");
                objEntity.DBCustomerName = GetTextVale(dr, "DBCustomerName");
                objEntity.CRCustomerID = GetInt64(dr, "CRCustomerID");
                objEntity.CRCustomerName = GetTextVale(dr, "CRCustomerName");
                objEntity.VoucherAmount = GetDecimal(dr, "VoucherAmount");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.DBC = GetTextVale(dr, "DBC");
                objEntity.FileName = GetTextVale(dr, "FileName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateExpenseVoucher(Entity.JournalVoucher objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ExpenseVoucher_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@VoucherNo", objEntity.VoucherNo);
            cmdAdd.Parameters.AddWithValue("@VoucherDate", objEntity.VoucherDate);
            cmdAdd.Parameters.AddWithValue("@DBCustomerID", objEntity.DBCustomerID);
            cmdAdd.Parameters.AddWithValue("@CRCustomerID", objEntity.CRCustomerID);
            cmdAdd.Parameters.AddWithValue("@VoucherAmount", objEntity.VoucherAmount);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@DBC", "EV");
            cmdAdd.Parameters.AddWithValue("@FileName", objEntity.FileName);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnVoucherNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnVoucherNo = cmdAdd.Parameters["@ReturnVoucherNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteExpenseVoucher(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ExpenseVoucher_DEL";
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
