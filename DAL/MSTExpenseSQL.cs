using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MSTExpenseSQL : BaseSqlManager
    {
        public virtual List<Entity.MSTExpense> GetExpenseList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MSTExpenseList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.MSTExpense> lstObject = new List<Entity.MSTExpense>();
            while (dr.Read())
            {
                Entity.MSTExpense objEntity = new Entity.MSTExpense();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExpenseName = GetTextVale(dr, "ExpenseName");
                objEntity.IsLocationRequired = GetBoolean(dr, "IsLocationRequired");                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
    }
}
