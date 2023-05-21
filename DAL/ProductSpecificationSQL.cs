using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ProductSpecificationSQL: BaseSqlManager
    {
        public virtual List<Entity.ProductSpecification> GetProductSpecificationList(Int64 pProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductSpecificationByProductList";
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductSpecification> lstObject = new List<Entity.ProductSpecification>();
            while (dr.Read())
            {
                Entity.ProductSpecification objEntity = new Entity.ProductSpecification();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                objEntity.MaterialRemarks = GetTextVale(dr, "MaterialRemarks");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateProductSpecification(Entity.ProductSpecification objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ProductSpecification_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@GroupHead", objEntity.GroupHead);
            cmdAdd.Parameters.AddWithValue("@MaterialHead", objEntity.MaterialHead);
            cmdAdd.Parameters.AddWithValue("@MaterialSpec", objEntity.MaterialSpec);
            cmdAdd.Parameters.AddWithValue("@MaterialRemarks", objEntity.MaterialRemarks);
            cmdAdd.Parameters.AddWithValue("@ItemOrder", objEntity.ItemOrder);
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
        public virtual void DeleteProductSpecificationByProductID(Int64 pProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductSpecificationByProductID_DEL";
            cmdDel.Parameters.AddWithValue("@ProductID", pProductID);
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

        //============================================================================================//

        public virtual List<Entity.ProductSpecification> GetProductSpecificationList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductSpecificationList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductSpecification> lstObject = new List<Entity.ProductSpecification>();
            while (dr.Read())
            {
                Entity.ProductSpecification objEntity = new Entity.ProductSpecification();
                objEntity.pkID = GetInt64 (dr, "pkID");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.MaterialRemarks = GetTextVale(dr, "MaterialRemarks");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
       
        public virtual List<Entity.ProductSpecification> GetProductSpecificationList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ProductSpecification> lstLocation = new List<Entity.ProductSpecification>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductSpecificationList";
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
                Entity.ProductSpecification objEntity = new Entity.ProductSpecification();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                objEntity.MaterialHead = GetTextVale(dr, "MaterialHead");
                objEntity.MaterialSpec = GetTextVale(dr, "MaterialSpec");
                objEntity.MaterialRemarks = GetTextVale(dr, "MaterialRemarks");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;           
        }

        public virtual List<Entity.ProductSpecification> GetProductSpecGroupList(string GroupHead)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductSpecGroupListByName";
            cmdGet.Parameters.AddWithValue("@GroupHead", GroupHead);
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ProductSpecification> lstObject = new List<Entity.ProductSpecification>();
            while (dr.Read())
            {
                Entity.ProductSpecification objEntity = new Entity.ProductSpecification();
                objEntity.GroupHead = GetTextVale(dr, "GroupHead");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void DeleteProductSpecification(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductSpecification_DEL";
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
