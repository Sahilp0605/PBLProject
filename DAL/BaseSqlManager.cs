using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BaseSqlManager
    {
        public static SqlConnection sCon;

        #region ConncetionString
        public static string ConncetionString()
        {
            if (Entity.Company.Mode == 2)
                return GenrateConnectionString(Entity.Company.CompanyId);
            else
                return System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;
        }
        #endregion

        #region ConncetionStringReport
        public static string ConncetionStringReport(string VendorCodes)
        {
            //return "Data Source=(local);Initial Catalog=manageamc_" + VendorCodes + ";Persist Security Info=True;User ID=sa;Password=9033409488";
            return "Data Source=db204.my-hosting-panel.com;Initial Catalog=manageamc_" + VendorCodes + ";Persist Security Info=True;User ID=manageamc_cmsclient;Password=client@321";
        }
        #endregion

        #region ConncetionStringRegistration
        public static string ConncetionStringRegistration()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["RegConfigConnection"].ConnectionString;
        }
        #endregion

        public static string GenrateConnectionString(int CompanyId)
        {
            string localString = "";
            SqlDataReader sdr;
            using (SqlConnection connection = new SqlConnection(ConncetionStringRegistration()))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("proc_ConnectionString " + CompanyId, connection))
                {
                    sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        localString = sdr.GetString(0);
                    }
                }
                connection.Close();
            }
            return localString;
        }


        #region OpenConncetion
        public static void OpenConncetion(SqlCommand command)
        {
            sCon = new SqlConnection();
            sCon.ConnectionString = ConncetionString();
            command.Connection = sCon;
            command.CommandTimeout = 30000;
            sCon.Open();
        }
        #endregion

        #region OpenConncetion
        public static void OpenConncetionReport(SqlCommand command, string VendorCodes)
        {
            sCon = new SqlConnection();
            sCon.ConnectionString = ConncetionStringReport(VendorCodes);
            command.Connection = sCon;
            command.CommandTimeout = 3000;
            sCon.Open();
        }
        #endregion

        #region OpenConncetionRegistration
        public static void OpenConncetionRegistration(SqlCommand command)
        {
            sCon = new SqlConnection();
            sCon.ConnectionString = ConncetionStringRegistration();
            command.Connection = sCon;
            command.CommandTimeout = 3000;
            sCon.Open();
        }
        #endregion

        #region ForceCloseConncetion
        public static void ForceCloseConncetion()
        {
            if (sCon.State == System.Data.ConnectionState.Open)
            {
                sCon.Close();
                SqlConnection.ClearAllPools();
            }
        }
        #endregion

        #region ExecuteDataReader
        public static SqlDataReader ExecuteDataReader(SqlCommand command)
        {
            OpenConncetion(command);
            return command.ExecuteReader();
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(SqlCommand command)
        {
            OpenConncetion(command);
            return command.ExecuteNonQuery();
        }
        #endregion

        public static SqlDataAdapter ExecuteSqlDataAdapter(SqlCommand command, string VendorCodes)
        {
            OpenConncetionReport(command, VendorCodes);
            SqlDataAdapter adp = new SqlDataAdapter(command);
            return adp;
        }

        #region ExecuteScalar
        public static object ExecuteScalar(SqlCommand command)
        {
            OpenConncetion(command);
            return command.ExecuteScalar();
        }
        #endregion

        #region ExecuteReader
        public SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            OpenConncetion(cmd);

            return cmd.ExecuteReader();
        } 
        #endregion

        public static decimal GetDecimal(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToDecimal(dr[fieldname]);
            else
                return 0;
        }

        public static double GetDouble(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToDouble(dr[fieldname]);
            else
                return 0;
        }

        public static string GetTextVale(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToString(dr[fieldname]);
            else
                return "";
        }

        public static int GetInt32(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToInt32(dr[fieldname]);
            else
                return 0;
        }

        public static long GetInt64(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToInt64(dr[fieldname]);
            else
                return 0;
        }

        public static DateTime GetDateTime(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToDateTime(dr[fieldname]);
            else
                return Convert.ToDateTime("01/01/1900");
        }

        public static bool GetBoolean(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToBoolean(dr[fieldname]);
            else
                return false;
        }

        public static string GetBase64(SqlDataReader dr, string fieldname)
        {
            if (dr[fieldname] != DBNull.Value)
                return Convert.ToBase64String((byte[])dr[fieldname]);
            else
                return "";
        }

    }
}
