using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OrgTypeSQL: BaseSqlManager
    {
        public virtual List<Entity.OrgTypes> GetOrgTypeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgTypeList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);  
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
            while (dr.Read())
            {
                Entity.OrgTypes objEntity = new Entity.OrgTypes();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgType = GetTextVale(dr, "OrgType");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.OrgTypes> GetOrgType(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgTypeList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);   
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.OrgTypes objEntity = new Entity.OrgTypes();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgType = GetTextVale(dr, "OrgType");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.OrgTypes> GetOrgType(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrgTypeList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
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
                Entity.OrgTypes objEntity = new Entity.OrgTypes();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrgType = GetTextVale(dr, "OrgType");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstEntity;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateOrgType(Entity.OrgTypes objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OrgType_INS_UPD";

            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrgType", objEntity.OrgType);
            cmdAdd.Parameters.AddWithValue("@ActiveFlag", objEntity.ActiveFlag);
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

        public virtual void DeleteOrgType(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OrgType_DEL";
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
