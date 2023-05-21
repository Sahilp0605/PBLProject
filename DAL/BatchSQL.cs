using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class BatchSQL : BaseSqlManager
    {
        public virtual List<Entity.Batch> GetBatchList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BatchList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Batch> lstObject = new List<Entity.Batch>();
            while (dr.Read())
            {
                Entity.Batch objEntity = new Entity.Batch();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.BatchName = GetTextVale(dr, "BatchName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Batch> GetBatch(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Batch> lstObject = new List<Entity.Batch>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BatchList";
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
                Entity.Batch objEntity = new Entity.Batch();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.BatchName = GetTextVale(dr, "BatchName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.Batch> GetBatch(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Batch> lstObject = new List<Entity.Batch>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BatchList";
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
                Entity.Batch objEntity = new Entity.Batch();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.BatchName = GetTextVale(dr, "BatchName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void AddUpdateBatch(Entity.Batch objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Batch_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@BatchName", objEntity.BatchName);
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
        public virtual void DeleteBatch(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Batch_DEL";
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
