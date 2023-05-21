﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ConstantSQL: BaseSqlManager
    {
        public virtual List<Entity.Constant> GetConstantList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ConstantGeneralList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Constant> lstObject = new List<Entity.Constant>();
            while (dr.Read())
            {
                Entity.Constant objEntity = new Entity.Constant();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.ConstantStyle = GetTextVale(dr, "ConstantStyle");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.ConstantHead = GetTextVale(dr, "ConstantHead");
                objEntity.ConstantValue = GetTextVale(dr, "ConstantValue");
                objEntity.DisplayOrder = GetInt64(dr, "DisplayOrder");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Constant> GetConstantList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {

            List<Entity.Constant> lstLocation = new List<Entity.Constant>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ConstantGeneralList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Constant objEntity = new Entity.Constant();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.Category = GetTextVale(dr, "Category");
                objEntity.ConstantStyle = GetTextVale(dr, "ConstantStyle");
                objEntity.ConstantHead = GetTextVale(dr, "ConstantHead");
                objEntity.ConstantValue = GetTextVale(dr, "ConstantValue");
                objEntity.DisplayOrder = GetInt64(dr, "DisplayOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateConstant(Entity.Constant objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Constant_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@Category", objEntity.Category);
            cmdAdd.Parameters.AddWithValue("@ConstantStyle", objEntity.ConstantStyle);
            cmdAdd.Parameters.AddWithValue("@ConstantHead", objEntity.ConstantHead);
            cmdAdd.Parameters.AddWithValue("@ConstantValue", objEntity.ConstantValue);
            cmdAdd.Parameters.AddWithValue("@DisplayOrder",objEntity.DisplayOrder);
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

        public virtual void DeleteConstant(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Constant_DEL";
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
