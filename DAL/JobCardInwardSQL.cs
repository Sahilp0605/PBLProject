using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
   public class JobCardInwardSQL:BaseSqlManager
    {
        public virtual List<Entity.JobCardInward> GetJobCardInwardList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardInwardList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardInward> lstObject = new List<Entity.JobCardInward>();
            while (dr.Read())
            {
                Entity.JobCardInward objEntity = new Entity.JobCardInward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InwardNo = GetTextVale(dr, "InwardNo");
                objEntity.InwardDate = GetDateTime(dr, "InwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.InwardAmount = GetDecimal(dr, "InwardAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.JobCardInward> GetJobCardInwardList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardInward> lstLocation = new List<Entity.JobCardInward>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardInwardList";
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
                Entity.JobCardInward objEntity = new Entity.JobCardInward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InwardNo = GetTextVale(dr, "InwardNo");
                objEntity.InwardDate = GetDateTime(dr, "InwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.InwardAmount = GetDecimal(dr, "InwardAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.JobCardInward> GetJobCardInwardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardInward> lstLocation = new List<Entity.JobCardInward>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardInwardList";
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
                Entity.JobCardInward objEntity = new Entity.JobCardInward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InwardNo = GetTextVale(dr, "InwardNo");
                objEntity.InwardDate = GetDateTime(dr, "InwardDate");
                objEntity.BasicAmount = GetDecimal(dr, "BasicAmount");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.InwardAmount = GetDecimal(dr, "InwardAmount");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        //Created by Vikram Rajput
        public virtual List<Entity.JobCardInward> GetJobCardInwardList(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardInwardListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardInward> lstObject = new List<Entity.JobCardInward>();
            while (dr.Read())
            {
                Entity.JobCardInward objEntity = new Entity.JobCardInward();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InwardNo = GetTextVale(dr, "InwardNo");
                objEntity.InwardDate = GetDateTime(dr, "InwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");

                objEntity.BasicAmount = GetDecimal(dr, "BasicAmount");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.InwardAmount = GetDecimal(dr, "InwardAmount");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateJobCardInward(Entity.JobCardInward objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInwardNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCardInward_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InwardNo", objEntity.InwardNo);
            cmdAdd.Parameters.AddWithValue("@InwardDate", objEntity.InwardDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnInwardNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnInwardNo = cmdAdd.Parameters["@ReturnInwardNo"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteJobCardInward(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardInward_DEL";
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

        public DataTable GetJobCardInwardDetail(string pInwardNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT qtd.pkID, qtd.InwardNo, qt.InwardDate,qt.CustomerID, qtd.ProductID, item.ProductName, '[' + item.ProductAlias + '] - ' + item.ProductName As ProductNameLong, item.ProductSpecification,  qtd.Quantity, qtd.Unit, qtd.UnitRate, qtd.DiscountPercent, qtd.NetRate, qtd.Amount, qtd.TaxRate, qtd.TaxAmount, qtd.NetAmount,qtd.OutwardNo From JobCardInward qt Inner Join JobCardInward_Detail qtd On qt.InwardNo = qtd.InwardNo Inner Join MST_Product item On qtd.ProductID = item.pkID Where qt.InwardNo = '" + @pInwardNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.JobCardInwardDetail> GetJobCardInwardDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardInwardDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCardInwardDetail> lstObject = new List<Entity.JobCardInwardDetail>();
            while (dr.Read())
            {
                Entity.JobCardInwardDetail objEntity = new Entity.JobCardInwardDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InwardNo = GetTextVale(dr, "InwardNo");
                objEntity.InwardDate = GetDateTime(dr, "InwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                //objEntity.Unit = GetTextVale(dr, "Unit");
                //objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                //objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                //objEntity.NetRate = GetDecimal(dr, "NetRate");
                //objEntity.Amount = GetDecimal(dr, "Amount");
                //objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                //objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                //objEntity.NetAmount = GetDecimal(dr, "NetAmount");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.JobCardInwardDetail> GetJobCardInwardDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardInwardDetail> lstLocation = new List<Entity.JobCardInwardDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardInwardDetailList";
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
                Entity.JobCardInwardDetail objEntity = new Entity.JobCardInwardDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InwardNo = GetTextVale(dr, "InwardNo");
                objEntity.InwardDate = GetDateTime(dr, "InwardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.OutwardNo = GetTextVale(dr, "OutwardNo");
                //objEntity.Unit = GetTextVale(dr, "Unit");
                //objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                //objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                //objEntity.NetRate = GetDecimal(dr, "NetRate");
                //objEntity.Amount = GetDecimal(dr, "Amount");
                //objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                //objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                //objEntity.NetAmount = GetDecimal(dr, "NetAmount");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateJobCardInwardDetail(Entity.JobCardInwardDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCardInwardDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InwardNo", objEntity.InwardNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@OutwardNo", objEntity.OutwardNo);
            //cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            //cmdAdd.Parameters.AddWithValue("@UnitRate", objEntity.UnitRate);
            //cmdAdd.Parameters.AddWithValue("@DiscountPercent", objEntity.DiscountPercent);
            //cmdAdd.Parameters.AddWithValue("@NetRate", objEntity.NetRate);
            //cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            //cmdAdd.Parameters.AddWithValue("@TaxRate", objEntity.TaxRate);
            //cmdAdd.Parameters.AddWithValue("@TaxAmount", objEntity.TaxAmount);
            //cmdAdd.Parameters.AddWithValue("@NetAmount", objEntity.NetAmount);
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

        public virtual void DeleteJobCardInwardDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardInwardDetail_DEL";
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

        public virtual void DeleteJobCardInwardDetailByInwardNo(string pInwardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardInwardDetailByInwardNo_DEL";
            cmdDel.Parameters.AddWithValue("@InwardNo", pInwardNo);
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
