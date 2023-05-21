using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ExpenseTypeSQL : BaseSqlManager
    {
        public virtual List<Entity.ExpenseType> GetExpenseTypeList(Int64 ExpenseTypeID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseTypeList";
            cmdGet.Parameters.AddWithValue("@pkID", ExpenseTypeID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ExpenseType> lstObject = new List<Entity.ExpenseType>();
            while (dr.Read())
            {
                Entity.ExpenseType objEntity = new Entity.ExpenseType();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                objEntity.IsLocationRequired = GetBoolean(dr, "IsLocationRequired");                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.ExpenseType> GetExpenseTypeList(Int64 ExpenseTypeID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExpenseTypeList";
            cmdGet.Parameters.AddWithValue("@pkID", ExpenseTypeID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ExpenseType> lstObject = new List<Entity.ExpenseType>();
            while (dr.Read())
            {
                Entity.ExpenseType objEntity = new Entity.ExpenseType();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseTypeName = GetTextVale(dr, "ExpenseTypeName");
                objEntity.IsLocationRequired = GetBoolean(dr, "IsLocationRequired");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateExpenseType(Entity.ExpenseType objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ExpenseType_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ExpenseTypeName", objEntity.ExpenseTypeName);
            cmdAdd.Parameters.AddWithValue("@IsLocationRequired", objEntity.IsLocationRequired);
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

        public virtual void DeleteExpenseType(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ExpenseType_DEL";
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
