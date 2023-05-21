using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ZoneClusterSQL:BaseSqlManager
    {
        public virtual List<Entity.ZoneCluster> GetZoneClusterList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ZoneCluster> lstLocation = new List<Entity.ZoneCluster>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ClusterDetailList";
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
                Entity.ZoneCluster objEntity = new Entity.ZoneCluster();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ClusterID = GetInt64(dr, "ClusterID");
                objEntity.ClusterName = GetTextVale(dr, "ClusterName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ZoneCluster> GetZoneClusterList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ZoneCluster> lstLocation = new List<Entity.ZoneCluster>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ClusterDetailList";
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
                Entity.ZoneCluster objEntity = new Entity.ZoneCluster();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ClusterID = GetInt64(dr, "ClusterID");
                objEntity.ClusterName = GetTextVale(dr, "ClusterName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }


        public virtual void AddUpdateCluster(Entity.ZoneCluster objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnClusterID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Cluster_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ClusterName", objEntity.ClusterName);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnClusterID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnClusterID = Convert.ToInt32(cmdAdd.Parameters["@ReturnClusterID"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void AddUpdateClusterDetail(Entity.ZoneCluster objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ClusterDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ClusterID", objEntity.ClusterID);
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
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

        public virtual void DeleteCourierInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ClusterDetail_DEL";
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
