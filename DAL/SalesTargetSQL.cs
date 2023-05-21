using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SalesTargetSQL : BaseSqlManager
    {
        public virtual List<Entity.SalesTarget> GetSalesTargetList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesTargetList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesTarget> lstObject = new List<Entity.SalesTarget>();
            while (dr.Read())
            {
                Entity.SalesTarget objEntity = new Entity.SalesTarget();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.TargetAmount = GetDecimal(dr, "TargetAmount");
                objEntity.TargetType = GetTextVale(dr, "TargetType");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesTarget> GetSalesTarget(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesTarget> lstObject = new List<Entity.SalesTarget>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesTargetList";
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
                Entity.SalesTarget objEntity = new Entity.SalesTarget();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.TargetAmount = GetDecimal(dr, "TargetAmount");
                objEntity.TargetType = GetTextVale(dr, "TargetType");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesTarget> GetSalesTarget(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesTarget> lstObject = new List<Entity.SalesTarget>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesTargetList";
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
                Entity.SalesTarget objEntity = new Entity.SalesTarget();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.TargetAmount = GetDecimal(dr, "TargetAmount");
                objEntity.TargetType = GetTextVale(dr, "TargetType");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateSalesTarget(Entity.SalesTarget objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesTarget_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@FromDate", objEntity.FromDate);

            cmdAdd.Parameters.AddWithValue("@ToDate", objEntity.ToDate);
            cmdAdd.Parameters.AddWithValue("@TargetType", objEntity.TargetType);
            cmdAdd.Parameters.AddWithValue("@TargetAmount", objEntity.TargetAmount);
            cmdAdd.Parameters.AddWithValue("@BrandID", objEntity.BrandID);
            cmdAdd.Parameters.AddWithValue("@ProductGroupID", objEntity.ProductGroupID);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
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

        public virtual void DeleteSalesTarget(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesTarget_DEL";
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

        public virtual List<Entity.SalesTarget> GetSalesTargetListByTargetType(String LoginUserID,Int64 day,Int64 month,Int64 year,string targettype, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesTargetListByTargetType";

            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@Day", day);
            cmdGet.Parameters.AddWithValue("@Month", month);
            cmdGet.Parameters.AddWithValue("@Year", year);
            cmdGet.Parameters.AddWithValue("@TargetType", targettype);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesTarget> lstObject = new List<Entity.SalesTarget>();
            while (dr.Read())
            {
                Entity.SalesTarget objEntity = new Entity.SalesTarget();
                objEntity.pkID = GetInt64(dr, "pkID");
                //objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromDate = GetDateTime(dr, "FromDate");
                objEntity.ToDate = GetDateTime(dr, "ToDate");
                objEntity.TargetType = GetTextVale(dr, "TargetType");
                objEntity.TargetAmount = GetDecimal(dr, "TargetAmount");
                objEntity.AchievedAmount = GetDecimal(dr, "AchievedAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
    }
}
