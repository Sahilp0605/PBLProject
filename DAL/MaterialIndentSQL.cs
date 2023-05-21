using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class MaterialIndentSQL : BaseSqlManager
    {
        public virtual List<Entity.MaterialIndent> GetMaterialIndentList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "IndentList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.MaterialIndent> lstObject = new List<Entity.MaterialIndent>();
            while (dr.Read())
            {
                Entity.MaterialIndent objEntity = new Entity.MaterialIndent();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.IndentDate= GetDateTime(dr, "IndentDate");
                objEntity.Remarks= GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.MaterialIndent> GetMaterialIndentList(String ApprovalStatus, String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "IndentList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", ApprovalStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.MaterialIndent> lstObject = new List<Entity.MaterialIndent>();
            while (dr.Read())
            {
                Entity.MaterialIndent objEntity = new Entity.MaterialIndent();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.IndentDate = GetDateTime(dr, "IndentDate");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.MaterialIndent> GetMaterialIndent(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.MaterialIndent> lstObject = new List<Entity.MaterialIndent>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "IndentList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.MaterialIndent objEntity = new Entity.MaterialIndent();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.IndentDate = GetDateTime(dr, "IndentDate");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.MaterialIndent> GetMaterialIndent(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.MaterialIndent> lstObject = new List<Entity.MaterialIndent>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "IndentList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.MaterialIndent objEntity = new Entity.MaterialIndent();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.IndentDate = GetDateTime(dr, "IndentDate");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        //----------------------------------------Insert - Update & Delete---------------------------
        public virtual void AddUpdateMaterialIndent(Entity.MaterialIndent objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnIndentNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Indent_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@IndentNo", objEntity.IndentNo);
            cmdAdd.Parameters.AddWithValue("@IndentDate", objEntity.IndentDate);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnIndentNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnIndentNo = cmdAdd.Parameters["@ReturnIndentNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteMaterialIndent(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Indent_DEL";
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


        //----------------------------Detail List-------------------------------------------
        public DataTable GetMaterialIndentDetail(string pIndentNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT qtd.pkID, qtd.IndentNo, qt.IndentDate,qtd.ProductID, item.ProductName, '[' + item.ProductAlias + '] - ' + item.ProductName As ProductNameLong,pg.ProductGroupName,  qtd.Quantity, qtd.Unit, qtd.ExpectedDate, qtd.Remarks From Indent qt Inner Join Indent_Detail qtd On qt.IndentNo = qtd.IndentNo Inner Join MST_Product item On qtd.ProductID = item.pkID Left Join MST_ProductGroup pg on pg.pkID = item.ProductGroupID Where qt.IndentNo = '" + pIndentNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.MaterialIndent_detail> GetMaterialIndentDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "IndentDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            //cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.MaterialIndent_detail> lstObject = new List<Entity.MaterialIndent_detail>();
            while (dr.Read())
            {
                Entity.MaterialIndent_detail objEntity = new Entity.MaterialIndent_detail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.Qty = GetDecimal(dr, "Qty");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.ExpectedDate = GetDateTime(dr, "ExpectedDate");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.MaterialIndent_detail> GetMaterialIndentDetailListByProduct(Int64 pProductID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "IndentDetailListByProduct";
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            
            //SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            //p.Direction = ParameterDirection.Output;
            //cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.MaterialIndent_detail> lstObject = new List<Entity.MaterialIndent_detail>();
            while (dr.Read())
            {
                Entity.MaterialIndent_detail objEntity = new Entity.MaterialIndent_detail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.Qty = GetDecimal(dr, "Quantity");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.ExpectedDate = GetDateTime(dr, "ExpectedDate");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.MaterialIndent_detail> GetMaterialIndentDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.MaterialIndent_detail> lstLocation = new List<Entity.MaterialIndent_detail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardDetailList";
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
                Entity.MaterialIndent_detail objEntity = new Entity.MaterialIndent_detail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.Qty = GetDecimal(dr, "Qty");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.ExpectedDate = GetDateTime(dr, "ExpectedDate");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        //----------------------------Insert/Update Detail------------------------------

        public virtual void AddUpdateMaterialIndentDetail(Entity.MaterialIndent_detail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "IndentDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@IndentNo", objEntity.IndentNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Qty);
            cmdAdd.Parameters.AddWithValue("@ExpectedDate", objEntity.ExpectedDate);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
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

        public virtual void DeleteMaterialIndentDetailByIndentNo(string pIndentNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "IndentDetailByIndentNo_DEL";
            cmdDel.Parameters.AddWithValue("@IndentNo", pIndentNo);
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

        public virtual void UpdateIndentApproval(Entity.MaterialIndent objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "MaterialIndentApproval_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
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
    }

}
