using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BackupDetailSQL : BaseSqlManager
    {
        public virtual void GenerateDatabaseBackup(Entity.BackupDetail entity)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GenerateDatabaseBackup";
            cmdAdd.Parameters.AddWithValue("@Database", entity.Database);
            cmdAdd.Parameters.AddWithValue("@DirPath", entity.DirPath);
            cmdAdd.Parameters.AddWithValue("@Filename", entity.FileName);
            cmdAdd.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy);
            ExecuteNonQuery(cmdAdd);
            ForceCloseConncetion();
        }

        public virtual List<Entity.BackupDetail> GetDatabaseBackupList(int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDatabaseBackupList";
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.BackupDetail> lstBackup = new List<Entity.BackupDetail>();
            while (dr.Read())
            {
                Entity.BackupDetail objBackup = new Entity.BackupDetail();
                objBackup.FileName = GetTextVale(dr, "FileName");
                objBackup.Date = GetDateTime(dr, "Date");
                objBackup.CreatedBy = GetTextVale(dr, "CreatedBy");
                lstBackup.Add(objBackup);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstBackup;
        }
    }
}
