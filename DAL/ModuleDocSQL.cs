using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ModuleDocSQL :BaseSqlManager
    {
        public virtual List<Entity.ModuleDocuments> GetModuleDocumentList(Int64 pkID, string SearchKey, string ModuleName, string DocName, string KeyValue, string LoginUserID)
        {
            List<Entity.ModuleDocuments> lstObject = new List<Entity.ModuleDocuments>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ModuleDocumentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@ModuleName", ModuleName);
            cmdGet.Parameters.AddWithValue("@DocName", DocName);
            cmdGet.Parameters.AddWithValue("@KeyValue", KeyValue);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ModuleDocuments objEntity = new Entity.ModuleDocuments();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ModuleName = GetTextVale(dr, "ModuleName");
                objEntity.KeyValue = GetTextVale(dr, "KeyValue");
                objEntity.DocName = GetTextVale(dr, "DocName");
                objEntity.DocType = GetTextVale(dr, "DocType");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateDocument(Entity.ModuleDocuments objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ModuleDocuments_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ModuleName", objEntity.ModuleName);
            cmdAdd.Parameters.AddWithValue("@KeyValue", objEntity.KeyValue);
            cmdAdd.Parameters.AddWithValue("@DocName", objEntity.DocName);
            cmdAdd.Parameters.AddWithValue("@DocType", objEntity.DocType);
            cmdAdd.Parameters.AddWithValue("@DocData", objEntity.DocData);
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

        public virtual void DeleteDocument(Int64 pkID, string ModuleName, string KeyValue, string DocName, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ModuleDocuments_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            cmdDel.Parameters.AddWithValue("@ModuleName", ModuleName);
            cmdDel.Parameters.AddWithValue("@KeyValue", KeyValue);
            cmdDel.Parameters.AddWithValue("@DocName", DocName);
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
