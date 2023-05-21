using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CampaignTemplateSQL : BaseSqlManager
    {

        public virtual List<Entity.CampaignTemplate> GetCampaignTemplate(Int64 CampaignID, String Category, String SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.CampaignTemplate> lstLocation = new List<Entity.CampaignTemplate>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CampaignTemplateList";
            cmdGet.Parameters.AddWithValue("@CampaignID", CampaignID);
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@LoginUserID", "");
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.CampaignTemplate objEntity = new Entity.CampaignTemplate();
                objEntity.CampaignID = GetInt64(dr, "CampaignID");
                objEntity.CampaignCategory = GetTextVale(dr, "CampaignCategory");
                objEntity.CampaignSubject = GetTextVale(dr, "CampaignSubject");
                objEntity.CampaignHeader = GetTextVale(dr, "CampaignHeader");
                objEntity.CampaignFooter = GetTextVale(dr, "CampaignFooter");
                objEntity.CampaignImageUrl = GetTextVale(dr, "CampaignImageUrl");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateCampaignTemplate(Entity.CampaignTemplate objEntity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnCampaignID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CampaignTemplate_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CampaignID", objEntity.CampaignID);
            cmdAdd.Parameters.AddWithValue("@CampaignCategory", objEntity.CampaignCategory);
            cmdAdd.Parameters.AddWithValue("@CampaignSubject", objEntity.CampaignSubject);
            cmdAdd.Parameters.AddWithValue("@CampaignHeader", objEntity.CampaignHeader);
            cmdAdd.Parameters.AddWithValue("@CampaignFooter", objEntity.CampaignFooter);
            cmdAdd.Parameters.AddWithValue("@CampaignImageUrl", objEntity.CampaignImageUrl);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnCampaignID", SqlDbType.BigInt);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnCampaignID = Convert.ToInt64(cmdAdd.Parameters["@ReturnCampaignID"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteCampaignTemplate(Int64 CampaignID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CampaignTemplate_DEL";
            cmdDel.Parameters.AddWithValue("@CampaignID", CampaignID);
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
