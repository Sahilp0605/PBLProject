using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class JobCardSQL : BaseSqlManager
    {
        public virtual List<Entity.JobCard> GetJobCardList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCard> lstLocation = new List<Entity.JobCard>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardList";
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
                Entity.JobCard objEntity = new Entity.JobCard();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.JobCardNo = GetTextVale(dr, "JobCardNo");
                objEntity.Date = GetDateTime(dr, "JobCardDate");
                objEntity.CollectedFrom = GetTextVale(dr, "CollectedFrom");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.DateIn = GetDateTime(dr, "DateIn");
                objEntity.DateReturn = GetDateTime(dr, "DateReturn");
                objEntity.WheelNo = GetTextVale(dr, "WheelNo");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.DeliveryNoteNo = GetTextVale(dr, "DeliveryNoteNo");
                objEntity.Tyre = GetTextVale(dr, "Tyre");
                objEntity.Cap = GetTextVale(dr, "Cap");
                objEntity.Sensor = GetTextVale(dr, "Sensor");
                objEntity.SensorValue = GetTextVale(dr, "SensorValue");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.PartNumber = GetTextVale(dr, "PartNumber");
                objEntity.WheelMake = GetTextVale(dr, "WheelMake");
                objEntity.StraightenedMeasurement = GetTextVale(dr, "StraightenedMeasurement");
                objEntity.ClaimNo = GetTextVale(dr, "ClaimNo");
                objEntity.RegNo = GetTextVale(dr, "RegNo");
                objEntity.ChassisNo = GetTextVale(dr, "ChassisNo");
                objEntity.PaintCode = GetTextVale(dr, "PaintCode");
                objEntity.Comment = GetTextVale(dr, "Comment");
                objEntity.DiamondCut = GetTextVale(dr, "DiamondCut");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.QualityCheck = GetTextVale(dr, "QualityCheck");
                objEntity.EstimatePrice = GetTextVale(dr, "EstimatePrice");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");
                objEntity.BuyerRef = GetTextVale(dr, "BuyerRef");
                objEntity.Location = GetTextVale(dr, "Location");
                objEntity.LocationName = GetTextVale(dr, "LocationName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateJobCard(Entity.JobCard objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnJobCardNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCard_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@JobCardNo", objEntity.JobCardNo);
            cmdAdd.Parameters.AddWithValue("@JobCardDate", objEntity.Date);
            cmdAdd.Parameters.AddWithValue("@CollectedFrom", objEntity.CollectedFrom);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@DateIn", objEntity.DateIn);
            cmdAdd.Parameters.AddWithValue("@DateReturn", objEntity.DateReturn);
            cmdAdd.Parameters.AddWithValue("@WheelNo", objEntity.WheelNo);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@DeliveryNoteNo", objEntity.DeliveryNoteNo);
            cmdAdd.Parameters.AddWithValue("@Tyre", objEntity.Tyre);
            cmdAdd.Parameters.AddWithValue("@Cap", objEntity.Cap);
            cmdAdd.Parameters.AddWithValue("@Sensor", objEntity.Sensor);
            cmdAdd.Parameters.AddWithValue("@SensorValue", objEntity.SensorValue);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);
            cmdAdd.Parameters.AddWithValue("@PartNumber", objEntity.PartNumber);
            cmdAdd.Parameters.AddWithValue("@WheelMake", objEntity.WheelMake);
            cmdAdd.Parameters.AddWithValue("@StraightenedMeasurement", objEntity.StraightenedMeasurement);
            cmdAdd.Parameters.AddWithValue("@ClaimNo", objEntity.ClaimNo);
            cmdAdd.Parameters.AddWithValue("@RegNo", objEntity.RegNo);
            cmdAdd.Parameters.AddWithValue("@ChassisNo", objEntity.ChassisNo);
            cmdAdd.Parameters.AddWithValue("@PaintCode", objEntity.PaintCode);
            cmdAdd.Parameters.AddWithValue("@Comment", objEntity.Comment);
            cmdAdd.Parameters.AddWithValue("@DiamondCut", objEntity.DiamondCut);
            cmdAdd.Parameters.AddWithValue("@StartDate", objEntity.StartDate);
            cmdAdd.Parameters.AddWithValue("@QualityCheck", objEntity.QualityCheck);
            cmdAdd.Parameters.AddWithValue("@EstimatePrice", objEntity.EstimatePrice);
            cmdAdd.Parameters.AddWithValue("@DeliveryDate", objEntity.DeliveryDate);
            cmdAdd.Parameters.AddWithValue("@BuyerRef", objEntity.BuyerRef);
            cmdAdd.Parameters.AddWithValue("@Location", objEntity.Location);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnJobCardNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnJobCardNo = cmdAdd.Parameters["@ReturnJobCardNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteJobCard(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCard_DEL";
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

        //================================================

        public virtual List<Entity.JobCardDetail> GetJobCardDetailList(Int64 pkID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.JobCardDetail> lstLocation = new List<Entity.JobCardDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardDetailList";
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
                Entity.JobCardDetail objEntity = new Entity.JobCardDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.JobCardNo = GetTextVale(dr, "JobCardNo");
                objEntity.JobCardDate = GetDateTime(dr, "JobCardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateJobCardDetail(Entity.JobCardDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "JobCardDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@JobCardNo", objEntity.JobCardNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@UnitRate", objEntity.UnitRate);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
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

        public virtual void DeleteJobCardDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardDetail_DEL";
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
        public virtual void DeleteJobCardDetailByJobCardNo(string JobCardNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "JobCardDetailByJobCardNo_DEL";
            cmdDel.Parameters.AddWithValue("@JobCardNo", JobCardNo);
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

        public DataTable GetJobCardDetail(string JobCardNo)
        {
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select jd.pkID, jd.ProductID, p.ProductName, jd.Unit, jd.Quantity, jd.UnitRate, jd.Amount From JobCard_Detail jd Inner Join MST_Product p on p.pkid = jd.ProductID where jd.JobCardNo = '" + JobCardNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;

        }

        public virtual List<Entity.JobCard> GetJobCardByCustomer(Int64 pCustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "JobCardListByCustomer";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.JobCard> lstObject = new List<Entity.JobCard>();
            while (dr.Read())
            {
                Entity.JobCard objEntity = new Entity.JobCard();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.JobCardNo = GetTextVale(dr, "JobCardNo");
                objEntity.JobCardDate = GetDateTime(dr, "JobCardDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.JobCardAmount = GetDecimal(dr, "JobCardAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public DataTable GetJobCardProductForSalesBill(string pJobCardNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            //myCommand.CommandText = "SELECT ip.pkID, ip.QuotationNo, ip.ProductID, it.ProductName As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.Unit, it.UnitPrice As UnitRate, it.UnitPrice, ip.DiscountPercent, ip.NetRate, cast((Round(it.UnitPrice,2)*Round(ip.Quantity,2)) as decimal(12,2)) As Amount, it.TaxRate, cast((((it.UnitPrice*ip.Quantity)*it.TaxRate)/100) as decimal(12,2)) As TaxAmount, cast((Round((it.UnitPrice*ip.Quantity),2) + Round((((it.UnitPrice*ip.Quantity)*it.TaxRate)/100),2)) as decimal(12,2)) As NetAmount, ip.Quantity From Quotation_Detail ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.QuotationNo = '" + @pQuotationNo + "'";
            if (!pJobCardNo.Contains(","))
                myCommand.CommandText = "SELECT cast('' as nvarchar(20)) As OrderNo,cast('' as nvarchar(20)) as InvoiceNo, cast('' as nvarchar(20)) as InquiryNo, cast('' as nvarchar(20)) as ReferenceNo, qd.UnitRate as UnitPrice,qd.UnitRate as Rate,0 as BundleID, 0 AS Box_Weight, 0 AS Box_SQFT, 0 AS Box_SQMT, qd.pkID, qd.JobCardNo As QuotationNo, qd.ProductID, CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong,'' As ProductSpecification,0.0 as UnitQty, qd.Quantity,qd.Quantity as Qty,it.TaxType,  ISNULL(it.UnitQuantity,1) AS UnitQuantity, qd.Unit, qd.UnitRate, 0.0 as DiscountPercent,0.0 As DiscountPer, 0.0 As NetRate, qd.Amount, 0.0 As TaxRate, 0 as TaxType, 0.0 AS TaxAmount, 0.0 As DiscountAmt, 0.0 as CGSTPer, 0.0 as CGSTAmt, 0.0 As SGSTPer, 0.0 As qd.SGSTAmt, 0.0 As IGSTPer, 0.0 as IGSTAmt, 0.0 AS NetAmount, 0.0 AS NetAmt,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,Qt.pkid As QtPKID,  '' As ForOrderNo, CONVERT(NVARCHAR(10),'') As DeliveryDate From JobCard_Detail qd Inner Join JobCard Qt On Qt.JobCardNo = qd.JobCardNo Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.JobCardNo = '" + pJobCardNo + "'";
            else
                myCommand.CommandText = "SELECT cast('' as nvarchar(20)) As OrderNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,  cast('' as nvarchar(20)) as ReferenceNo, qd.UnitRate as UnitPrice,qd.UnitRate as Rate,0 as BundleID, 0 AS Box_Weight, 0 AS Box_SQFT, 0 AS Box_SQMT, qd.pkID, qd.JobCardNo As QuotationNo, qd.ProductID, CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong,'' As ProductSpecification,0.0 as UnitQty, qd.Quantity,qd.Quantity as Qty, it.TaxType, ISNULL(it.UnitQuantity,1) AS UnitQuantity, qd.Unit, qd.UnitRate, 0.0 as DiscountPercent,0.0 As DiscountPer, 0.0 As NetRate, qd.Amount, 0.0 As TaxRate, 0 as TaxType, 0.0 As TaxAmount, 0.0 As DiscountAmt, 0.0 as CGSTPer, 0.0 as CGSTAmt, 0.0 As SGSTPer, 0.0 AS SGSTAmt, 0.0 As IGSTPer, 0.0 As IGSTAmt, 0.0 AS NetAmount, 0.0 AS NetAmt,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,Qt.pkid As QtPKID, '' As ForOrderNo, CONVERT(NVARCHAR(10),'') As DeliveryDate From JobCard_Detail qd Inner Join JobCard Qt On Qt.JobCardNo = qd.JobCardNo Inner Join MST_Product it On qd.ProductID = it.pkID Where '" + pJobCardNo + "' like Concat('%', qd.JobCardNo, ',%')";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
    }
}