using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OutwardSQL: BaseSqlManager
    {
        public virtual List<Entity.Outward> GetOutwardList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Outward> lstObject = new List<Entity.Outward>();
            while (dr.Read())
            {
                Entity.Outward objEntity = new Entity.Outward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                //------------------
                objEntity.ExporterRef = GetTextVale(dr, "ExporterRef");
                objEntity.SupOrderRef = GetTextVale(dr, "SupOrderRef");
                objEntity.SupOrderDate = GetDateTime(dr, "SupOrderDate");
                objEntity.OtherRef = GetTextVale(dr, "OtherRef");
                //-----------------
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
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Outward> GetOutwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Outward> lstLocation = new List<Entity.Outward>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardList";
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
                Entity.Outward objEntity = new Entity.Outward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                //------------------
                objEntity.ExporterRef= GetTextVale(dr, "ExporterRef");
                objEntity.SupOrderRef= GetTextVale(dr, "SupOrderRef");
                objEntity.SupOrderDate= GetDateTime(dr, "SupOrderDate");
                objEntity.OtherRef= GetTextVale(dr, "OtherRef");
                //-----------------
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
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                objEntity.ManualOutwardNo = GetTextVale(dr, "ManualOutwardNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Outward> GetOutwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Outward> lstLocation = new List<Entity.Outward>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Outward objEntity = new Entity.Outward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.BasicAmount = GetDecimal(dr, "BasicAmount");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.OutwardAmount = GetDecimal(dr, "OutwardAmount");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                //------------------
                objEntity.ExporterRef = GetTextVale(dr, "ExporterRef");
                objEntity.SupOrderRef = GetTextVale(dr, "SupOrderRef");
                objEntity.SupOrderDate = GetDateTime(dr, "SupOrderDate");
                objEntity.OtherRef = GetTextVale(dr, "OtherRef");
                //-----------------
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
                objEntity.Remarks= GetTextVale(dr, "Remarks");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                objEntity.ManualOutwardNo = GetTextVale(dr, "ManualOutwardNo");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        //Created by Vikram Rajput
        public virtual List<Entity.Outward> GetOutwardList(string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Outward> lstObject = new List<Entity.Outward>();
            while (dr.Read())
            {
                Entity.Outward objEntity = new Entity.Outward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderStatus = GetTextVale(dr, "OrderStatus");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");     // Supp.Ref.No of From SalesOrder Reference No
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

        public virtual List<Entity.Outward> GetOutwardListByCustomerProduct(Int64 pCustomerID, Int64 pProductID, string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardListByCustomerProduct";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Outward> lstObject = new List<Entity.Outward>();
            while (dr.Read())
            {
                Entity.Outward objEntity = new Entity.Outward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                objEntity.OutwardDate = GetDateTime(dr, "OutwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderStatus = GetTextVale(dr, "OrderStatus");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");     // Supp.Ref.No of From SalesOrder Reference No
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
        public virtual void AddUpdateOutward(Entity.Outward objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnOutwardNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Outward_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            cmdAdd.Parameters.AddWithValue("@OutwardDate", objEntity.OutwardDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@ExporterRef", objEntity.ExporterRef);
            cmdAdd.Parameters.AddWithValue("@SupOrderRef", objEntity.SupOrderRef);
            if (objEntity.SupOrderDate != null)
            {
                if (objEntity.SupOrderDate.Year>=1900)
                    cmdAdd.Parameters.AddWithValue("@SupOrderDate", objEntity.SupOrderDate);
            }
            cmdAdd.Parameters.AddWithValue("@OtherRef", objEntity.OtherRef);
            cmdAdd.Parameters.AddWithValue("@OrderStatus", objEntity.OrderStatus);
            cmdAdd.Parameters.AddWithValue("@ModeOfTransport", objEntity.ModeOfTransport);
            cmdAdd.Parameters.AddWithValue("@TransporterName", objEntity.TransporterName);
            cmdAdd.Parameters.AddWithValue("@VehicleNo", objEntity.VehicleNo);
            cmdAdd.Parameters.AddWithValue("@LRNo", objEntity.LRNo);

            cmdAdd.Parameters.AddWithValue("@ManualOutwardNo", objEntity.ManualOutwardNo);

            if (objEntity.LRDate != null)
            {
                if (Convert.ToDateTime(objEntity.LRDate).Year >= 1900)
                    cmdAdd.Parameters.AddWithValue("@LRDate", objEntity.LRDate);
            }
            //cmdAdd.Parameters.AddWithValue("@LRDate", objEntity.LRDate);
            cmdAdd.Parameters.AddWithValue("@DCNo", objEntity.DCNo);
            if (objEntity.DCDate != null)
            {
                if (Convert.ToDateTime(objEntity.DCDate).Year >= 1900)
                    cmdAdd.Parameters.AddWithValue("@DCDate", objEntity.DCDate);
            }
            //cmdAdd.Parameters.AddWithValue("@DCDate", objEntity.DCDate);
            cmdAdd.Parameters.AddWithValue("@DeliveryNote", objEntity.DeliveryNote);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);

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

        public virtual void DeleteOutward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Outward_DEL";
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
        public DataTable GetOutwardDetail(string pOutwardNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT qtd.pkID, qtd.OutwardNo, qt.OutwardDate,qt.CustomerID, qtd.ProductID, item.ProductName, '[' + item.ProductAlias + '] - ' + item.ProductName As ProductNameLong, qtd.ProductSpecification,  qtd.Quantity, qtd.Unit, qtd.QuantityWeight, qtd.SerialNo, qtd.BoxNo, qtd.UnitRate, qtd.DiscountPercent, qtd.NetRate, qtd.Amount, qtd.TaxRate, qtd.TaxAmount, qtd.NetAmount,qtd.* From Outward qt Inner Join Outward_Detail qtd On qt.OutwardNo = qtd.OutwardNo Inner Join MST_Product item On qtd.ProductID = item.pkID Where qt.OutwardNo = '" + @pOutwardNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.OutwardDetail> GetOutwardDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OutwardDetail> lstObject = new List<Entity.OutwardDetail>();
            while (dr.Read())
            {
                Entity.OutwardDetail objEntity = new Entity.OutwardDetail();
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

        public virtual List<Entity.OutwardDetail> GetOutwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OutwardDetail> lstLocation = new List<Entity.OutwardDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardDetailList";
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
                Entity.OutwardDetail objEntity = new Entity.OutwardDetail();
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

        public virtual void AddUpdateOutwardDetail(Entity.OutwardDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OutwardDetail_INS_UPD";
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
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
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

        public virtual void DeleteOutwardDetailByOutwardNo(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OutwardDetailByOutwardNo_DEL";
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
        public virtual List<Entity.OutwardDetailAssembly> GetOutwardDetailAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardDetailAssemblyList";
            cmdGet.Parameters.AddWithValue("@OutwardNo", pOutwardNo);
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            cmdGet.Parameters.AddWithValue("@AssemblyID", pAssemblyID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OutwardDetailAssembly> lstObject = new List<Entity.OutwardDetailAssembly>();
            while (dr.Read())
            {
                Entity.OutwardDetailAssembly objEntity = new Entity.OutwardDetailAssembly();
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

        public virtual void AddUpdateOutwardDetailAssembly(Entity.OutwardDetailAssembly objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OutwardDetailAssembly_INS_UPD";
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

        public virtual void DeleteOutwardDetailAssemblyByOutwardNo(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OutwardDetailAssemblyByOutwardNo_DEL";
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


        //---------------------------Export Details-----------------------------
        public virtual List<Entity.Outward> GetOutwardExportList(Int64 pkID, string OutwardNo, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OutwardExportList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@OutwardNo", OutwardNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Outward> lstObject = new List<Entity.Outward>();
            while (dr.Read())
            {
                Entity.Outward objEntity = new Entity.Outward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");

                objEntity.PreCarrBy = GetTextVale(dr, "PreCarrBy");
                objEntity.PreCarrRecPlace = GetTextVale(dr, "PreCarrRecPlace");
                objEntity.FlightNo = GetTextVale(dr, "FlightNo");
                objEntity.PortOfLoading = GetTextVale(dr, "PortOfLoading");
                objEntity.PortOfDispatch = GetTextVale(dr, "PortOfDispatch");
                objEntity.PortOfDestination = GetTextVale(dr, "PortOfDestination");
                objEntity.MarksNo = GetTextVale(dr, "MarksNo");
                objEntity.Packages = GetTextVale(dr, "Packages");
                objEntity.PackageType = GetTextVale(dr, "PackageType");
                objEntity.NetWeight = GetTextVale(dr, "NetWeight");
                objEntity.GrossWeight = GetTextVale(dr, "GrossWeight");
                objEntity.FreeOnBoard = GetTextVale(dr, "FreeOnBoard");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                //objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                //objEntity.CompanyID = GetInt64(dr, "CompanyID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateOutwardExport(Entity.Outward objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Outward_Export_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            cmdAdd.Parameters.AddWithValue("@PreCarrBy", objEntity.PreCarrBy);
            cmdAdd.Parameters.AddWithValue("@PreCarrRecPlace", objEntity.PreCarrRecPlace);
            cmdAdd.Parameters.AddWithValue("@FlightNo", objEntity.FlightNo);
            cmdAdd.Parameters.AddWithValue("@PortOfLoading", objEntity.PortOfLoading);
            cmdAdd.Parameters.AddWithValue("@PortOfDispatch", objEntity.PortOfDispatch);
            cmdAdd.Parameters.AddWithValue("@PortOfDestination", objEntity.PortOfDestination);
            cmdAdd.Parameters.AddWithValue("@MarksNo", objEntity.MarksNo);
            cmdAdd.Parameters.AddWithValue("@Packages", objEntity.Packages);
            cmdAdd.Parameters.AddWithValue("@PackageType", objEntity.PackageType);
            cmdAdd.Parameters.AddWithValue("@NetWeight", objEntity.NetWeight);
            cmdAdd.Parameters.AddWithValue("@GrossWeight", objEntity.GrossWeight);
            cmdAdd.Parameters.AddWithValue("@FreeOnBoard", objEntity.FreeOnBoard);

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

        public virtual void DeleteOutwardExport(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "OutwardExport_DEL";
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

        public virtual void DeleteOutwardAttachment(string OutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Outward_Attachment_DEL";
            cmdDel.Parameters.AddWithValue("@OutwardNo", OutwardNo);
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

        public virtual void AddUpdateOutwardAttachment(Entity.Outward_Attachment objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Outward_Attachment_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            cmdAdd.Parameters.AddWithValue("@AttachmentFile", objEntity.AttachmentFile);
            cmdAdd.Parameters.AddWithValue("@LogID", objEntity.LogID);
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

        public virtual List<Entity.Outward_Attachment> GetOutwardAttachmentList(Int64 pkID, string OutwardNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "Outward_Attachment_List";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@OutwardNo", OutwardNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Outward_Attachment> lstLocation = new List<Entity.Outward_Attachment>();
            while (dr.Read())
            {
                Entity.Outward_Attachment objLocation = new Entity.Outward_Attachment();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.OutwardNo= GetTextVale(dr, "OutwardNo");
                objLocation.AttachmentFile = GetTextVale(dr, "AttachmentFile");
                objLocation.LogID = GetInt64(dr, "LogID");
                objLocation.ContentData = null;
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
