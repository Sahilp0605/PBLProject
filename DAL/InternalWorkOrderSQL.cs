using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class InternalWorkOrderSQL : BaseSqlManager
    {
        public virtual List<Entity.InternalWorkOrder> GetInternalWorkOrderList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InternalWorkOrder> lstObject = new List<Entity.InternalWorkOrder>();
            while (dr.Read())
            {
                Entity.InternalWorkOrder objEntity = new Entity.InternalWorkOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate= GetDateTime(dr, "ReferenceDate");
                objEntity.SalesOrderNo = GetTextVale(dr, "SalesOrderNo");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.InternalWorkOrder> GetInternalWorkOrderList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InternalWorkOrder> lstLocation = new List<Entity.InternalWorkOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderList";
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
                Entity.InternalWorkOrder objEntity = new Entity.InternalWorkOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");
                objEntity.SalesOrderNo = GetTextVale(dr, "SalesOrderNo");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.InternalWorkOrder> GetInternalWorkOrderList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InternalWorkOrder> lstLocation = new List<Entity.InternalWorkOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderList";
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
                Entity.InternalWorkOrder objEntity = new Entity.InternalWorkOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");
                objEntity.SalesOrderNo = GetTextVale(dr, "SalesOrderNo");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateInternalWorkOrder(Entity.InternalWorkOrder objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnWorkOrderNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InternalWorkOrder_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@WorkOrderNo", objEntity.WorkOrderNo);
            cmdAdd.Parameters.AddWithValue("@WorkOrderDate", objEntity.WorkOrderDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ReferenceNo", objEntity.ReferenceNo);
            cmdAdd.Parameters.AddWithValue("@ReferenceDate", objEntity.ReferenceDate);
            cmdAdd.Parameters.AddWithValue("@SalesOrderNo", objEntity.SalesOrderNo);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@LogInUserID", objEntity.LogInUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnWorkOrderNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnWorkOrderNo = cmdAdd.Parameters["@ReturnWorkOrderNo"].Value.ToString();

            ForceCloseConncetion();
        }
        public virtual void DeleteInternalWorkOrder(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InternalWorkOrder_DEL";
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

        //-----------------------------------Internal Work Order Detail-------------------------------
        public virtual List<Entity.InternalWorkOrderDetail> GetIternalWorkOrderDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InternalWorkOrderDetail> lstObject = new List<Entity.InternalWorkOrderDetail>();
            while (dr.Read())
            {
                Entity.InternalWorkOrderDetail objEntity = new Entity.InternalWorkOrderDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");
                objEntity.ScopeOfWork = GetTextVale(dr, "ScopeOfWork");
                objEntity.ScopeOfWork_Client = GetTextVale(dr, "ScopeOfWork_Client");
                objEntity.Remarks= GetTextVale(dr, "Remarks");
                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.InternalWorkOrderDetail> GetIternalWorkOrderDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InternalWorkOrderDetail> lstLocation = new List<Entity.InternalWorkOrderDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderDetailList";
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
                Entity.InternalWorkOrderDetail objEntity = new Entity.InternalWorkOrderDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");
                objEntity.ScopeOfWork = GetTextVale(dr, "ScopeOfWork");
                objEntity.ScopeOfWork_Client = GetTextVale(dr, "ScopeOfWork_Client");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.InternalWorkOrderDetail> GetIternalWorkOrderDetailListByWorkOrderNo(string WorkOrderNo, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InternalWorkOrderDetail> lstLocation = new List<Entity.InternalWorkOrderDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderDetailListByWorkOrderNo";
            cmdGet.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InternalWorkOrderDetail objEntity = new Entity.InternalWorkOrderDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");
                objEntity.ScopeOfWork = GetTextVale(dr, "ScopeOfWork");
                objEntity.ScopeOfWork_Client = GetTextVale(dr, "ScopeOfWork_Client");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateInternalWorkOrderDetail(Entity.InternalWorkOrderDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InternalWorkOrderDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@WorkOrderNo", objEntity.WorkOrderNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            //cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
            cmdAdd.Parameters.AddWithValue("@DeliveryDate", objEntity.DeliveryDate);
            cmdAdd.Parameters.AddWithValue("@ScopeOfWork", objEntity.ScopeOfWork);
            cmdAdd.Parameters.AddWithValue("@ScopeOfWork_Client", objEntity.ScopeOfWork_Client);
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

        public virtual void DeleteInternalWorkOrderDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InternalWorkOrderDetail_DEL";
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
        public virtual void DeleteInternalWorkOrderDetailByWorkOrderNo(string WorkOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InternalWorkOrderDetailByWorkOrderNo_DEL";
            cmdDel.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo);
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
        public virtual List<Entity.InternalWorkOrderDetail> GetInternalWorkOrderProdSpecList(String WorkOrderNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.InternalWorkOrderDetail> lstLocation = new List<Entity.InternalWorkOrderDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InternalWorkOrderSpecsList";
            cmdGet.Parameters.AddWithValue("@WorkOrderNo", WorkOrderNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InternalWorkOrderDetail objEntity = new Entity.InternalWorkOrderDetail();
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ScopeOfWork = GetTextVale(dr, "ScopeOfWork");
                objEntity.ScopeOfWork_Client = GetTextVale(dr, "ScopeOfWork_Client");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.InternalWorkOrder> GetQuotationInquiry(String OrderNo,Int64 CustomerID )
        {
            List<Entity.InternalWorkOrder> lstLocation = new List<Entity.InternalWorkOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetQuotationInquiry";
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", "admin");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InternalWorkOrder objEntity = new Entity.InternalWorkOrder();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
