using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MailBoxSQL : BaseSqlManager
    {
        public virtual List<Entity.MailBox> GetMailBoxList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.MailBox> lstLocation = new List<Entity.MailBox>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MailBoxList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@MessageID", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.MailBox objEntity = new Entity.MailBox();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.MessageID = GetTextVale(dr, "MessageID");
                objEntity.MailDate = GetDateTime(dr, "MailDate");
                objEntity.MailDateSent = GetDateTime(dr, "MailDateSent");
                objEntity.MailFrom = GetTextVale(dr, "MailFrom");
                objEntity.MailTo = GetTextVale(dr, "MailTo");
                objEntity.MailCc = GetTextVale(dr, "MailCc");
                objEntity.MailSubject = GetTextVale(dr, "MailSubject");
                objEntity.MailBody = GetTextVale(dr, "MailBody");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateMailBoxEntry(Entity.MailBox objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "MailBoxEntry_INS_UPD";

            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@MessageID", objEntity.MessageID);
            cmdAdd.Parameters.AddWithValue("@MailDate", objEntity.MailDate);
            cmdAdd.Parameters.AddWithValue("@MailDateSent", objEntity.MailDate);
            cmdAdd.Parameters.AddWithValue("@MailFrom", objEntity.MailFrom);
            cmdAdd.Parameters.AddWithValue("@MailTo", objEntity.MailFrom);
            cmdAdd.Parameters.AddWithValue("@MailCc", objEntity.MailCc);
            cmdAdd.Parameters.AddWithValue("@MailSubject", objEntity.MailSubject);
            cmdAdd.Parameters.AddWithValue("@MailBody", objEntity.MailBody);
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

        public virtual void DeleteMailBoxEntry(string MessageID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "MailBoxEntry_DEL";
            cmdDel.Parameters.AddWithValue("@MessageID", MessageID);
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

        public virtual string GetLastMailTimestamp(string pLoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT MailTimestamp From MST_Users Where lower(UserID)='" + pLoginUserID + "'";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public virtual string setLastMailTimestamp(string pLoginUserID)
        {
            try
            {
                string query = "Update MST_Users Set MailTimestamp = getdate() Where lower(UserID)='" + pLoginUserID + "'";
                SqlCommand cmdAdd = new SqlCommand(query);
                ExecuteNonQuery(cmdAdd);
            }
            catch (Exception ex) { }
            ForceCloseConncetion();
            return "";
        }
    }
}
