using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class JobCardOutwardSQL: BaseSqlManager
    {
        public virtual List<Entity.JobCardOutward> GetJobCardOutwardList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardOutward> lstObject = new List<Entity.JobCardOutward>();
            while (dr.Read())
            {
                Entity.JobCardOutward objEntity = new Entity.JobCardOutward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderStatus = GetTextVale(dr, "OrderStatus");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.OutwardAmount = GetDecimal(dr, "OutwardAmount");

                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DCNo = GetTextVale(dr, "DCNo");
                objEntity.DCDate = GetDateTime(dr, "DCDate");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.JobCardOutward> GetJobCardOutwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardOutward> lstLocation = new List<Entity.JobCardOutward>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardList";
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
                Entity.JobCardOutward objEntity = new Entity.JobCardOutward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderStatus = GetTextVale(dr, "OrderStatus");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.OutwardAmount = GetDecimal(dr, "OutwardAmount");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DCNo = GetTextVale(dr, "DCNo");
                objEntity.DCDate = GetDateTime(dr, "DCDate");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JobCardOutward> GetJobCardOutwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardOutward> lstLocation = new List<Entity.JobCardOutward>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardList";
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
                Entity.JobCardOutward objEntity = new Entity.JobCardOutward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.BasicAmount = GetDecimal(dr, "BasicAmount");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.OutwardAmount = GetDecimal(dr, "OutwardAmount");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderStatus = GetTextVale(dr, "OrderStatus");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DCNo = GetTextVale(dr, "DCNo");
                objEntity.DCDate = GetDateTime(dr, "DCDate");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JobCardOutward> GetJobCardOutwardList(Int64 CustomerID, Int64 ProductID, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardListByUser";
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ProductID", ProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardOutward> lstObject = new List<Entity.JobCardOutward>();
            while (dr.Read())
            {
                Entity.JobCardOutward objEntity = new Entity.JobCardOutward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderStatus = GetTextVale(dr, "OrderStatus");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");

                objEntity.BasicAmount = GetDecimal(dr, "BasicAmount");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.OutwardAmount = GetDecimal(dr, "OutwardAmount");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DCNo = GetTextVale(dr, "DCNo");
                objEntity.DCDate = GetDateTime(dr, "DCDate");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateJobCardOutward(Entity.JobCardOutward objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnOutwardNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCardOutward_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            cmdAdd.Parameters.AddWithValue("@OutwardDate", objEntity.OutwardDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@OrderStatus", objEntity.OrderStatus);
            cmdAdd.Parameters.AddWithValue("@ModeOfTransport", objEntity.ModeOfTransport);
            cmdAdd.Parameters.AddWithValue("@TransporterName", objEntity.TransporterName);
            cmdAdd.Parameters.AddWithValue("@VehicleNo", objEntity.VehicleNo);
            cmdAdd.Parameters.AddWithValue("@LRNo", objEntity.LRNo);
            cmdAdd.Parameters.AddWithValue("@LRDate", objEntity.LRDate);
            cmdAdd.Parameters.AddWithValue("@DCNo", objEntity.DCNo);
            cmdAdd.Parameters.AddWithValue("@DCDate", objEntity.DCDate);
            cmdAdd.Parameters.AddWithValue("@DeliveryNote", objEntity.DeliveryNote);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnOutwardNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnOutwardNo = cmdAdd.Parameters["@ReturnOutwardNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteJobCardOutward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardOutward_DEL";
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
        // -------------------------------------------------------------------------
        public DataTable GetJobCardOutwardDetail(string pOutwardNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT qtd.pkID, qtd.OutwardNo, qt.OutwardDate,qt.CustomerID, qtd.ProductID, item.ProductName, '[' + item.ProductAlias + '] - ' + item.ProductName As ProductNameLong, qtd.ProductSpecification,  qtd.Quantity, qtd.Unit, qtd.QuantityWeight, qtd.SerialNo, qtd.BoxNo, qtd.UnitRate, qtd.DiscountPercent, qtd.NetRate, qtd.Amount, qtd.TaxRate, qtd.TaxAmount, qtd.NetAmount From JobCardOutward qt Inner Join JobCardOutward_Detail qtd On qt.OutwardNo = qtd.OutwardNo Inner Join MST_Product item On qtd.ProductID = item.pkID Where qt.OutwardNo = '" + @pOutwardNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.JobCardOutwardDetail> GetJobCardOutwardDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardOutwardDetail> lstObject = new List<Entity.JobCardOutwardDetail>();
            while (dr.Read())
            {
                Entity.JobCardOutwardDetail objEntity = new Entity.JobCardOutwardDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.QuantityWeight = GetDecimal(dr, "QuantityWeight");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.BoxNo = GetTextVale(dr, "BoxNo");
                objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                objEntity.NetRate = GetDecimal(dr, "NetRate");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.NetAmount = GetDecimal(dr, "NetAmount");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.JobCardOutwardDetail> GetJobCardOutwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardOutwardDetail> lstLocation = new List<Entity.JobCardOutwardDetail>();
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
                Entity.JobCardOutwardDetail objEntity = new Entity.JobCardOutwardDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.QuantityWeight = GetDecimal(dr, "QuantityWeight");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.BoxNo = GetTextVale(dr, "BoxNo");
                objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                objEntity.NetRate = GetDecimal(dr, "NetRate");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.NetAmount = GetDecimal(dr, "NetAmount");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateJobCardOutwardDetail(Entity.JobCardOutwardDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCardOutwardDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@QuantityWeight", objEntity.QuantityWeight);
            cmdAdd.Parameters.AddWithValue("@SerialNo", objEntity.SerialNo);
            cmdAdd.Parameters.AddWithValue("@BoxNo", objEntity.BoxNo);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@UnitRate", objEntity.UnitRate);
            cmdAdd.Parameters.AddWithValue("@DiscountPercent", objEntity.DiscountPercent);
            cmdAdd.Parameters.AddWithValue("@NetRate", objEntity.NetRate);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@TaxRate", objEntity.TaxRate);
            cmdAdd.Parameters.AddWithValue("@TaxAmount", objEntity.TaxAmount);
            cmdAdd.Parameters.AddWithValue("@NetAmount", objEntity.NetAmount);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
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

        public virtual void DeleteJobCardOutwardDetailByOutwardNo(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardOutwardDetailByOutwardNo_DEL";
            cmdDel.Parameters.AddWithValue("@OutwardNo", pOutwardNo);
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
        // --------------------------------------------------------------
        public virtual List<Entity.JobCardOutwardDetailAssembly> GetJobCardOutwardDetailAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardOutwardDetailAssemblyList";
            cmdGet.Parameters.AddWithValue("@OutwardNo", pOutwardNo);
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            cmdGet.Parameters.AddWithValue("@AssemblyID", pAssemblyID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardOutwardDetailAssembly> lstObject = new List<Entity.JobCardOutwardDetailAssembly>();
            while (dr.Read())
            {
                Entity.JobCardOutwardDetailAssembly objEntity = new Entity.JobCardOutwardDetailAssembly();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.AssemblyID = GetInt64(dr, "AssemblyID");
                objEntity.AssemblyName = GetTextVale(dr, "AssemblyName");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.QuantityWeight = GetDecimal(dr, "QuantityWeight");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.SerialNo = GetTextVale(dr, "SerialNo");
                objEntity.BoxNo = GetTextVale(dr, "BoxNo");
                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateJobCardOutwardDetailAssembly(Entity.JobCardOutwardDetailAssembly objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCardOutwardDetailAssembly_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@AssemblyID", objEntity.AssemblyID);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@QuantityWeight", objEntity.QuantityWeight);
            cmdAdd.Parameters.AddWithValue("@SerialNo", objEntity.SerialNo);
            cmdAdd.Parameters.AddWithValue("@BoxNo", objEntity.BoxNo);
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

        public virtual void DeleteJobCardOutwardDetailAssemblyByOutwardNo(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardOutwardDetailAssemblyByOutwardNo_DEL";
            cmdDel.Parameters.AddWithValue("@OutwardNo", pOutwardNo);
            cmdDel.Parameters.AddWithValue("@ProductID", pProductID);
            cmdDel.Parameters.AddWithValue("@AssemblyID", pAssemblyID);
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
