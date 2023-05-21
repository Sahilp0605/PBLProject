using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class BankVoucherSQL : BaseSqlManager
    {
        public virtual List<Entity.BankVoucher> GetBankVoucherList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BrandList_Report";
            cmdGet.Parameters.AddWithValue("@InvoiceNo", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@FromDate", 0);
            cmdGet.Parameters.AddWithValue("@ToDate", 0);
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);

            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.BankVoucher> lstObject = new List<Entity.BankVoucher>();
            while (dr.Read())
            {
                Entity.BankVoucher objEntity = new Entity.BankVoucher();
                objEntity.InvoiceNo =   GetTextVale(dr, "InvoiceNo");
                objEntity.ListMode = GetTextVale(dr, "ListMode");
                objEntity.LoginUserId = GetTextVale(dr, "LoginUserId");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.LoginUserId = GetTextVale(dr, "LoginUserId");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.BankVoucher> GetBankVoucher( string InvoiceNo, string LoginUserID, string ListMode, DateTime FromDate, DateTime ToDate,int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.BankVoucher> lstLocation = new List<Entity.BankVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BankVoucherList_Report";
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.BankVoucher objEntity = new Entity.BankVoucher();
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.LoginUserId = GetTextVale(dr, "LoginUserId");
                objEntity.ListMode = GetTextVale(dr, "ListMode");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.ListMode = GetTextVale(dr, "PageNo");
                objEntity.ListMode = GetTextVale(dr, "PageSize");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.BankVoucher> GetBankVoucherList(string InvoiceNo, string LoginUserID, DateTime FromDate, DateTime ToDate,int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.BankVoucher> lstLocation = new List<Entity.BankVoucher>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BankVoucherList_Report";
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.BankVoucher objEntity = new Entity.BankVoucher();
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.LoginUserId = GetTextVale(dr, "LoginUserId");
                objEntity.ListMode = GetTextVale(dr, "ListMode");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
