using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CompanyRegistrationSQL : BaseSqlManager
    {


        public virtual List<Entity.CompanyRegistration> GetCompanyRegistrationList(String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CompanyRegistrationList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            //SqlDataReader dr = ExecuteDataReader(cmdGet);
            OpenConncetionRegistration(cmdGet);
            SqlDataReader dr = cmdGet.ExecuteReader();
            List<Entity.CompanyRegistration> lstObject = new List<Entity.CompanyRegistration>();
            while (dr.Read())
            {
                Entity.CompanyRegistration objEntity = new Entity.CompanyRegistration();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.NoOfUsers = GetInt64(dr, "NoOfUsers");
                objEntity.SerialKey = GetTextVale(dr, "SerialKey");
                objEntity.DBIP = GetTextVale(dr, "DBIP");
                objEntity.DBName = GetTextVale(dr, "DBName");
                objEntity.DBUsername = GetTextVale(dr, "DBUsername");
                objEntity.DBPassword = GetTextVale(dr, "DBPassword");
                objEntity.Regno = GetTextVale(dr, "Regno");
                objEntity.InstallationDate = GetDateTime(dr, "InstallationDate");
                objEntity.ExpiryDate = GetDateTime(dr, "ExpiryDate");

                objEntity.RootPath = GetTextVale(dr, "RootPath");
                objEntity.SiteURL = GetTextVale(dr, "SiteURL");
                objEntity.IndiaMartKey = GetTextVale(dr, "IndiaMartKey");
                objEntity.IndiaMartMobile = GetTextVale(dr, "IndiaMartMobile");
                objEntity.IndiaMartAcAlias = GetTextVale(dr, "IndiaMartAcAlias");

                objEntity.IndiaMartKey2 = GetTextVale(dr, "IndiaMartKey2");
                objEntity.IndiaMartMobile2 = GetTextVale(dr, "IndiaMartMobile2");
                objEntity.IndiaMartAcAlias2 = GetTextVale(dr, "IndiaMartAcAlias2");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.CompanyRegistration> GetCompanyRegistration(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.CompanyRegistration> lstObject = new List<Entity.CompanyRegistration>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CompanyRegistrationList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            OpenConncetionRegistration(cmdGet);
            SqlDataReader dr = cmdGet.ExecuteReader();
            while (dr.Read())
            {
                Entity.CompanyRegistration objEntity = new Entity.CompanyRegistration();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.NoOfUsers = GetInt64(dr, "NoOfUsers");
                objEntity.SerialKey = GetTextVale(dr, "SerialKey");
                objEntity.DBIP = GetTextVale(dr, "DBIP");
                objEntity.DBName = GetTextVale(dr, "DBName");
                objEntity.DBUsername = GetTextVale(dr, "DBUsername");
                objEntity.DBPassword = GetTextVale(dr, "DBPassword");
                objEntity.Regno = GetTextVale(dr, "Regno");
                objEntity.InstallationDate = GetDateTime(dr, "InstallationDate");
                objEntity.ExpiryDate = GetDateTime(dr, "ExpiryDate");
                objEntity.RootPath = GetTextVale(dr, "RootPath");
                objEntity.SiteURL = GetTextVale(dr, "SiteURL");
                objEntity.IndiaMartKey = GetTextVale(dr, "IndiaMartKey");
                objEntity.IndiaMartMobile = GetTextVale(dr, "IndiaMartMobile");
                objEntity.IndiaMartAcAlias = GetTextVale(dr, "IndiaMartAcAlias");

                objEntity.IndiaMartKey2 = GetTextVale(dr, "IndiaMartKey2");
                objEntity.IndiaMartMobile2 = GetTextVale(dr, "IndiaMartMobile2");
                objEntity.IndiaMartAcAlias2 = GetTextVale(dr, "IndiaMartAcAlias2");

                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.CompanyRegistration> GetCompanyRegistrationBySerialKey(String pSerialKey)
        {
            List<Entity.CompanyRegistration> lstObject = new List<Entity.CompanyRegistration>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CompanyRegistrationListBySerialKey";
            cmdGet.Parameters.AddWithValue("@SerialKey", pSerialKey);
            OpenConncetionRegistration(cmdGet);
            SqlDataReader dr = cmdGet.ExecuteReader();
            while (dr.Read())
            {
                Entity.CompanyRegistration objEntity = new Entity.CompanyRegistration();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.NoOfUsers = GetInt64(dr, "NoOfUsers");
                objEntity.SerialKey = GetTextVale(dr, "SerialKey");
                objEntity.DBIP = GetTextVale(dr, "DBIP");
                objEntity.DBName = GetTextVale(dr, "DBName");
                objEntity.DBUsername = GetTextVale(dr, "DBUsername");
                objEntity.DBPassword = GetTextVale(dr, "DBPassword");
                objEntity.Regno = GetTextVale(dr, "Regno");
                objEntity.InstallationDate = GetDateTime(dr, "InstallationDate");
                objEntity.ExpiryDate = GetDateTime(dr, "ExpiryDate");
                objEntity.RootPath = GetTextVale(dr, "RootPath");
                objEntity.SiteURL = GetTextVale(dr, "SiteURL");
                objEntity.IndiaMartKey = GetTextVale(dr, "IndiaMartKey");
                objEntity.IndiaMartMobile = GetTextVale(dr, "IndiaMartMobile");
                objEntity.IndiaMartAcAlias = GetTextVale(dr, "IndiaMartAcAlias");

                objEntity.IndiaMartKey2 = GetTextVale(dr, "IndiaMartKey2");
                objEntity.IndiaMartMobile2 = GetTextVale(dr, "IndiaMartMobile2");
                objEntity.IndiaMartAcAlias2 = GetTextVale(dr, "IndiaMartAcAlias2");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateCompanyRegistration(Entity.CompanyRegistration objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CompanyRegistration_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@CompanyName", objEntity.CompanyName);
            cmdAdd.Parameters.AddWithValue("@NoOfUsers", objEntity.NoOfUsers);

            cmdAdd.Parameters.AddWithValue("@SerialKey", objEntity.SerialKey);
            cmdAdd.Parameters.AddWithValue("@DBIP", objEntity.DBIP);
            cmdAdd.Parameters.AddWithValue("@DBName", objEntity.DBName);
            cmdAdd.Parameters.AddWithValue("@DBUsername", objEntity.DBUsername);
            cmdAdd.Parameters.AddWithValue("@DBPassword", objEntity.DBPassword);

            cmdAdd.Parameters.AddWithValue("@RootPath", objEntity.RootPath);
            cmdAdd.Parameters.AddWithValue("@SiteURL", objEntity.SiteURL);
            cmdAdd.Parameters.AddWithValue("@IndiaMartKey", objEntity.IndiaMartKey);
            cmdAdd.Parameters.AddWithValue("@IndiaMartMobile", objEntity.IndiaMartMobile);
            cmdAdd.Parameters.AddWithValue("@IndiaMartAcAlias", objEntity.IndiaMartAcAlias);

            cmdAdd.Parameters.AddWithValue("@IndiaMartKey2", objEntity.IndiaMartKey2);
            cmdAdd.Parameters.AddWithValue("@IndiaMartMobile2", objEntity.IndiaMartMobile2);
            cmdAdd.Parameters.AddWithValue("@IndiaMartAcAlias2", objEntity.IndiaMartAcAlias2);

            if (objEntity.InstallationDate.ToString("dd/MM/yyyy") != "01/01/0001")
                cmdAdd.Parameters.AddWithValue("@InstallationDate", objEntity.InstallationDate);

            if (objEntity.ExpiryDate.ToString("dd/MM/yyyy") != "01/01/0001")
                cmdAdd.Parameters.AddWithValue("@ExpiryDate", objEntity.ExpiryDate);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;

            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);

            //ExecuteNonQuery(cmdAdd);
            OpenConncetionRegistration(cmdAdd);
            cmdAdd.ExecuteNonQuery();

            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteCompanyRegistration(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CompanyRegistration_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);

            OpenConncetionRegistration(cmdDel);
            cmdDel.ExecuteNonQuery();

            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }
    }
}
