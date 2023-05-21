using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace DAL
{
    public class ManageImpExpSQL : BaseSqlManager 
    {

        // ===================================================================================
        // Member Photo ID .. 
        // ===================================================================================
        public virtual void AddProductPhoto(Int64 pkID, byte[] imgData, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SetProductImage";
            cmdAdd.Parameters.AddWithValue("@pkID", pkID);
            cmdAdd.Parameters.AddWithValue("@img", SqlDbType.Image).Value = imgData;
            ExecuteNonQuery(cmdAdd);
            ReturnCode = 0;
            ReturnMsg = "Success";
            ForceCloseConncetion();
        }

        public virtual byte[] GetProductPhotoID(Int64 pkID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GetProductImage";
            cmdAdd.Parameters.AddWithValue("@pkID", pkID);
            SqlDataReader dr = ExecuteDataReader(cmdAdd);
            byte[] msImage = null;
            while (dr.Read())
            {
                msImage = (byte[])dr["ProductImage"];
            }
            dr.Close();
            ForceCloseConncetion();
            return msImage;
        }

        // ===================================================================================
        // Member Photo ID .. 
        // ===================================================================================
        public virtual void AddMemberPhoto(string pRegistrationNo, byte[] imgData, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SetMemberImage";
            cmdAdd.Parameters.AddWithValue("@RegistrationNo", pRegistrationNo);
            cmdAdd.Parameters.AddWithValue("@img", SqlDbType.Image).Value = imgData; 
            ExecuteNonQuery(cmdAdd);
            ReturnCode = 0;
            ReturnMsg = "Success";
            ForceCloseConncetion();
        }

        public virtual byte[] GetMemberPhotoID(string pRegistrationNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GetMemberImage";
            cmdAdd.Parameters.AddWithValue("@RegistrationNo", pRegistrationNo);
            SqlDataReader dr = ExecuteDataReader(cmdAdd);
            byte[] msImage = null;
            while (dr.Read())
            {
                msImage = (byte[])dr["MemberImage"];
            }
            dr.Close();
            ForceCloseConncetion();
            return msImage;
        }

        // ===================================================================================
        // Driver Photo ID .. 
        // ===================================================================================
        public virtual void AddDriverPhoto(Int64 pDriverID, byte[] imgData, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SetDriverImage";
            cmdAdd.Parameters.AddWithValue("@DriverID", pDriverID);
            cmdAdd.Parameters.AddWithValue("@img", SqlDbType.Image).Value = imgData;
            ExecuteNonQuery(cmdAdd);
            ReturnCode = 0;
            ReturnMsg = "Success";
            ForceCloseConncetion();
        }

        public virtual byte[] GetDriverPhotoID(Int64 pDriverID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GetDriverImage";
            cmdAdd.Parameters.AddWithValue("@DriverID", pDriverID);
            SqlDataReader dr = ExecuteDataReader(cmdAdd);
            byte[] msImage = null;
            while (dr.Read())
            {
                msImage = (byte[])dr["DriverImage"];
            }
            dr.Close();
            ForceCloseConncetion();
            return msImage;
        }
    }
}
