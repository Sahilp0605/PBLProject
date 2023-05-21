using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BundleSQL : BaseSqlManager
    {
        public virtual List<Entity.Bundle> GetBundleList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BundleList";
            cmdGet.Parameters.AddWithValue("@BundleId", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Bundle> lstObject = new List<Entity.Bundle>();
            while (dr.Read())
            {
                Entity.Bundle objEntity = new Entity.Bundle();
                objEntity.BundleId  = GetInt64(dr, "BundleId");
                objEntity.BundleName  = GetTextVale(dr, "BundleName");
               
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.Bundle> GetBundle(Int64 pkId)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "BundleList";
            cmdGet.Parameters.AddWithValue("@BundleId", pkId);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Bundle> lstObject = new List<Entity.Bundle>();
            while (dr.Read())
            {             
                Entity.Bundle objEntity = new Entity.Bundle();
                objEntity.BundleId = GetInt64(dr, "BundleId");
                objEntity.BundleName = GetTextVale(dr, "BundleName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateBundle(Entity.Bundle objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Bundle_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@BundleId", objEntity.BundleId);
            cmdAdd.Parameters.AddWithValue("@BundleName", objEntity.BundleName);
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

        public virtual void DeleteBundle(Int64 BundleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Bundle_DEL";
            cmdDel.Parameters.AddWithValue("@BundleID", BundleID);
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
