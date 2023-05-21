using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OtherChargeSQL : BaseSqlManager
    {
        public virtual List<Entity.OtherCharge> GetOtherChargeList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OtherChargeList";
            cmdGet.Parameters.AddWithValue("@pkId", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OtherCharge> lstObject = new List<Entity.OtherCharge>();
            while (dr.Read())
            {
                Entity.OtherCharge objEntity = new Entity.OtherCharge();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ChargeName = GetTextVale(dr, "ChargeName");
                objEntity.GST_Per = GetDecimal(dr, "GST_Per");
                objEntity.HSNCODE = GetTextVale(dr, "HSNCODE");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.TaxTypeName = GetTextVale(dr, "TaxTypeName");
                objEntity.BeforeGST = GetBoolean(dr, "BeforeGST");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OtherCharge> GetOtherChargeList(Int64 pkId)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OtherChargeList";
            cmdGet.Parameters.AddWithValue("@pkId", pkId);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OtherCharge> lstObject = new List<Entity.OtherCharge>();
            while (dr.Read())
            {
                Entity.OtherCharge objEntity = new Entity.OtherCharge();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ChargeName = GetTextVale(dr, "ChargeName");
                objEntity.GST_Per = GetDecimal(dr, "GST_Per");
                objEntity.HSNCODE = GetTextVale(dr, "HSNCODE");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.BeforeGST = GetBoolean(dr, "BeforeGST");
                objEntity.TaxTypeName = GetTextVale(dr, "TaxTypeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.OtherCharge> GetOtherChargeList(Int64 pkId, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OtherChargeList";
            cmdGet.Parameters.AddWithValue("@pkId", pkId);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OtherCharge> lstObject = new List<Entity.OtherCharge>();
            while (dr.Read())
            {
                Entity.OtherCharge objEntity = new Entity.OtherCharge();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ChargeName = GetTextVale(dr, "ChargeName");
                objEntity.GST_Per = GetDecimal(dr, "GST_Per");
                objEntity.HSNCODE = GetTextVale(dr, "HSNCODE");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.BeforeGST = GetBoolean(dr, "BeforeGST");
                objEntity.TaxTypeName = GetTextVale(dr, "TaxTypeName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            return lstObject;
        }
        public virtual void AddUpdateOtherCharge(Entity.OtherCharge objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OtherCharge_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ChargeName", objEntity.ChargeName);
            cmdAdd.Parameters.AddWithValue("@GST_Per", objEntity.GST_Per);
            cmdAdd.Parameters.AddWithValue("@HSNCODE", objEntity.HSNCODE);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);
            cmdAdd.Parameters.AddWithValue("@BeforeGST", objEntity.BeforeGST);
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

        public virtual void DeleteOtherCharge(Int64 ChargeId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OtherCharge_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", ChargeId);
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
