using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ApplicationMenuSQL : BaseSqlManager
    {
        public virtual List<Entity.ApplicationMenu> GetMenuByParent(string pModule, Int64 ParentId)
        {
            List<Entity.ApplicationMenu> lstLocation = new List<Entity.ApplicationMenu>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ApplicationMenuListByParentId";
            cmdGet.Parameters.AddWithValue("@Module", pModule);
            cmdGet.Parameters.AddWithValue("@ParentId", ParentId);
            
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ApplicationMenu objEntity = new Entity.ApplicationMenu();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.MenuText = GetTextVale(dr, "MenuText");
                objEntity.ParentId = GetInt64(dr, "ParentId");
                objEntity.Active = GetBoolean(dr, "ActiveStatus");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
