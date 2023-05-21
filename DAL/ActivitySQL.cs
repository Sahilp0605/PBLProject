using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ActivitySQL : BaseSqlManager
    {
        public virtual List<Entity.Activity> GetActivityList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ActivityList";
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Activity> lstLocation = new List<Entity.Activity>();
            while (dr.Read())
            {
                Entity.Activity objEntity = new Entity.Activity();
                objEntity.ActivityCode = GetTextVale(dr, "ActivityCode");
                objEntity.ActivityName = GetTextVale(dr, "ActivityName");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");               
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Activity> GetActivity(string ActivityCode)
        {
            List<Entity.Activity> lstLocation = new List<Entity.Activity>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ActivityList";
            cmdGet.Parameters.AddWithValue("@ActivityCode", ActivityCode);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Activity objEntity = new Entity.Activity();
                objEntity.ActivityCode = GetTextVale(dr, "ActivityCode");
                objEntity.ActivityName = GetTextVale(dr, "ActivityName");
                objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc"); 
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateActivity(Entity.Activity objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Activity_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@ActivityCode", objEntity.ActivityCode);
            cmdAdd.Parameters.AddWithValue("@ActivityName", objEntity.ActivityName);
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

        public virtual void DeleteActivity(string ActivityCode, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Activity_DEL";
            cmdDel.Parameters.AddWithValue("@ActivityCode", ActivityCode);
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
