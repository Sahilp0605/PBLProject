using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class CategoryDescriptionSQL : BaseSqlManager
    {
        public virtual List<Entity.CategoryDescription> GetCategoryDescriptionList(string Category)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CatDescSSRList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CategoryDescription> lstObject = new List<Entity.CategoryDescription>();
            while (dr.Read())
            {
                Entity.CategoryDescription objEntity = new Entity.CategoryDescription();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.Description = GetTextVale(dr, "Decription");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.CategoryDescription> GetCategoryDescriptionList(Int64 pkID, string Category, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.CategoryDescription> lstLocation = new List<Entity.CategoryDescription>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CatDescSSRList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.CategoryDescription objEntity = new Entity.CategoryDescription();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.Description = GetTextVale(dr, "Decription");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CategoryDescription> GetCategoryDescriptionList(Int64 pkID, string Category, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.CategoryDescription> lstLocation = new List<Entity.CategoryDescription>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CatDescSSRList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Category", Category);
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
                Entity.CategoryDescription objEntity = new Entity.CategoryDescription();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.Description = GetTextVale(dr, "Description");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateCategoryDescription(Entity.CategoryDescription objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CatDescSSR_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@Category", objEntity.Category);
            cmdAdd.Parameters.AddWithValue("@Decription", objEntity.Description);
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

        public virtual void DeleteCategoryDescription(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CatDescSSR_DEL";
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
