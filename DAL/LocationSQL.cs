using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace DAL
{
    public class LocationSQL : BaseSqlManager
    {
        public virtual List<Entity.Location> GetLocationList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LocationList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Location> lstObject = new List<Entity.Location>();
            while (dr.Read())
            {
                Entity.Location objEntity = new Entity.Location();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LocationName = GetTextVale(dr, "LocationName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Location> GetLocation(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Location> lstObject = new List<Entity.Location>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LocationList";
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
                Entity.Location objEntity = new Entity.Location();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LocationName = GetTextVale(dr, "LocationName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.Location> GetLocation(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Location> lstObject = new List<Entity.Location>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "LocationList";
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
                Entity.Location objEntity = new Entity.Location();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LocationName = GetTextVale(dr, "LocationName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void AddUpdateLocation(Entity.Location objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Location_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@LocationName", objEntity.LocationName);
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
        public virtual void DeleteLocation(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Location_DEL";
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
