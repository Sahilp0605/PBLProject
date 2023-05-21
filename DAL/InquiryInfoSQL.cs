using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace DAL
{
    public class InquiryInfoSQL: BaseSqlManager
    {
        public DataTable GetInquiryProductDetail(string pInquiryNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ip.pkID, ip.InquiryNo, ip.ProductID, CAST(it.ProductName AS NVARCHAR(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.Unit, ISNULL(ip.UnitPrice,0) AS 'UnitPrice', it.TaxRate, ip.Quantity ,ip.Unit, ip.Thickness, ip.Factor, ip.Area, ip.Remarks From Inquiry_Product ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.InquiryNo = '" + @pInquiryNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetInquiryProductForQuotation(string pInquiryNo, string forQuotationNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;

            if (!pInquiryNo.Contains(","))
                myCommand.CommandText = " select a.*, InquiryNo As DocRefNo, " +
                                 " case when (a.TaxType =1) then (Round((ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0)),2) + a.TaxAmount) else  cast(Round(ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0),2) as decimal(12,2)) end As NetAmount," +
                                  " case when (a.TaxType =1) then (Round((ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0)),2) + a.TaxAmount) else  cast(Round(ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0),2) as decimal(12,2)) end As NetAmt" +
                                 " From  " +
                                 " (SELECT '' As QuotationNo,'' as OrderNo,'' as InvoiceNo, '' As ReferenceNo, ip.pkID, ip.InquiryNo, ip.ProductID, it.ProductName As ProductName,it.ProductSpecification, it.TaxType, '[' + it.ProductAlias + '] - ' +  " +
                                 " it.ProductName As ProductNameLong, it.Unit, ISNULL(ip.UnitPrice,0) As UnitRate, ISNULL(ip.UnitPrice,0) As UnitPrice, ISNULL(ip.UnitPrice,0) As Rate, ISNULL(ip.UnitPrice,0) As NetRate,   " +
                                 " cast((Round(ISNULL(ip.UnitPrice,0),2)*Round(ISNULL(ip.Quantity,0),2)) as decimal(12,2)) As Amount, it.TaxRate,   " +
                                 " Case when (it.TaxType=0) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*ISNULL(it.TaxRate,0))/(100 + it.TaxRate))  " +
                                 " as decimal(12,2))  else  (Case when (it.TaxType=1) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*  " +
                                 " ISNULL(it.TaxRate,0))/(100)) as decimal(12,2)) else 0 end ) end  As TaxAmount,  cast('0' as decimal(12,2))  as 'DiscountPercent',cast('0' as decimal(12,2))  as 'DiscountPer', " +
                                 " cast('0' as decimal(12,2))  as 'DiscountAmt',cast('0' as decimal(12,2))  as CGSTPer,cast('0' as decimal(12,2))  as SGSTPer,  " +
                                 " it.TaxRate as IGSTPer,cast('0' as decimal(12,2))  as CGSTAmt,cast('0' as decimal(12,2))  as SGSTAmt, ip.InquiryNo As ForOrderNo, " +
                                 " Case when (it.TaxType=0) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*ISNULL(it.TaxRate,0))/(100 + it.TaxRate))  " +
                                 " as decimal(12,2))  else  (Case when (it.TaxType=1) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*ISNULL(it.TaxRate,0))/(100))  " +
                                 " as decimal(12,2)) else 0 end ) end   As IGSTAmt, 0 as UnitQty,  ip.Quantity,ip.Quantity as Qty, ISNULL(it.UnitQuantity,1) As UnitQuantity, it.UnitSize, it.UnitSurface, it.UnitGrade, ISNULL(it.Box_Weight,0) As Box_Weight, ISNULL(it.Box_SQFT,0) As Box_SQFT, ISNULL(it.Box_SQMT,0) As Box_SQMT, 0 as BundleId,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,CONVERT(NVARCHAR(10),'') as DeliveryDate, cast('' as nvarchar(20)) as Flag " +
                                 " From Inquiry_Product ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.InquiryNo = '" + pInquiryNo + "') as a";
            else
                myCommand.CommandText = " select a.*, InquiryNo As DocRefNo," +
                                 " case when (a.TaxType =1) then (Round((ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0)),2) + a.TaxAmount) else  cast(Round(ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0),2) as decimal(12,2)) end As NetAmount," +
                                  " case when (a.TaxType =1) then (Round((ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0)),2) + a.TaxAmount) else  cast(Round(ISNULL(a.UnitRate,0)*ISNULL(a.Quantity,0),2) as decimal(12,2)) end As NetAmt" +
                                 " From  " +
                                 " (SELECT '' As QuotationNo,'' as OrderNo,'' as InvoiceNo, '' As ReferenceNo, ip.pkID, ip.InquiryNo, ip.ProductID, it.ProductName As ProductName,it.ProductSpecification, it.TaxType, '[' + it.ProductAlias + '] - ' +  " +
                                 " it.ProductName As ProductNameLong, it.Unit, ISNULL(ip.UnitPrice,0) As UnitRate, ISNULL(ip.UnitPrice,0) As UnitPrice, ISNULL(ip.UnitPrice,0) As Rate, ISNULL(ip.UnitPrice,0) As NetRate,   " +
                                 " cast((Round(ISNULL(ip.UnitPrice,0),2)*Round(ISNULL(ip.Quantity,0),2)) as decimal(12,2)) As Amount, it.TaxRate,   " +
                                 " Case when (it.TaxType=0) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*ISNULL(it.TaxRate,0))/(100 + it.TaxRate))  " +
                                 " as decimal(12,2))  else  (Case when (it.TaxType=1) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*  " +
                                 " ISNULL(it.TaxRate,0))/(100)) as decimal(12,2)) else 0 end ) end  As TaxAmount,  cast('0' as decimal(12,2))  as 'DiscountPercent',cast('0' as decimal(12,2))  as 'DiscountPer', " +
                                 " cast('0' as decimal(12,2))  as 'DiscountAmt',cast('0' as decimal(12,2))  as CGSTPer,cast('0' as decimal(12,2))  as SGSTPer,  " +
                                 " it.TaxRate as IGSTPer,cast('0' as decimal(12,2))  as CGSTAmt,cast('0' as decimal(12,2))  as SGSTAmt, ip.InquiryNo As ForOrderNo, " +
                                 " Case when (it.TaxType=0) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*ISNULL(it.TaxRate,0))/(100 + it.TaxRate))  " +
                                 " as decimal(12,2))  else  (Case when (it.TaxType=1) then   cast((((ISNULL(ip.UnitPrice,0)*ISNULL(ip.Quantity,0))*ISNULL(it.TaxRate,0))/(100))  " +
                                 " as decimal(12,2)) else 0 end ) end   As IGSTAmt, 0 as UnitQty,  ip.Quantity,ip.Quantity as Qty, ISNULL(it.UnitQuantity,1) As UnitQuantity, it.UnitSize, it.UnitSurface, it.UnitGrade, ISNULL(it.Box_Weight,0) As Box_Weight, ISNULL(it.Box_SQFT,0) As Box_SQFT, ISNULL(it.Box_SQMT,0) As Box_SQMT, 0 as BundleId,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,CONVERT(NVARCHAR(10),'') as DeliveryDate, cast('' as nvarchar(20)) as Flag " +
                                 " From Inquiry_Product ip Inner Join MST_Product it On ip.ProductID = it.pkID Where '" + pInquiryNo + "' like Concat('%',ip.InquiryNo,'%')) as a";

            SqlDataReader dr = ExecuteDataReader(myCommand);

            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetAssemblyProductForQuotation(Int64 ProductID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = " select a.*, '' As DocRefNo,"+
                                    " case when(a.TaxType = 1) then(Round((ISNULL(a.UnitRate, 0) * ISNULL(a.Quantity, 0)), 2) + a.TaxAmount) else  cast(Round(ISNULL(a.UnitRate, 0) * ISNULL(a.Quantity, 0), 2) as decimal(12, 2)) end As NetAmount," +
                                    " case when(a.TaxType = 1) then(Round((ISNULL(a.UnitRate, 0) * ISNULL(a.Quantity, 0)), 2) + a.TaxAmount) else  cast(Round(ISNULL(a.UnitRate, 0) * ISNULL(a.Quantity, 0), 2) as decimal(12, 2)) end As NetAmt" +
                                    " From" +
                                    " (SELECT  FinishProductID,'' As QuotationNo, '' as OrderNo, '' as InvoiceNo, '' As ReferenceNo, ip.pkID, '' as InquiryNo, ip.ProductID, it.ProductName As ProductName, it.ProductSpecification, it.TaxType, '[' + it.ProductAlias + '] - ' +" +
                                    " it.ProductName As ProductNameLong, it.Unit, ISNULL(it.UnitPrice, 0) As UnitRate, ISNULL(it.UnitPrice, 0) As UnitPrice, ISNULL(it.UnitPrice, 0) As Rate, ISNULL(it.UnitPrice, 0) As NetRate," +
                                    " cast((Round(ISNULL(it.UnitPrice, 0), 2) * Round(ISNULL(ip.Quantity, 0), 2)) as decimal(12, 2)) As Amount, it.TaxRate,   " +
                                    " Case when (it.TaxType = 0) then   cast((((ISNULL(it.UnitPrice, 0) * ISNULL(ip.Quantity, 0)) * ISNULL(it.TaxRate, 0)) / (100 + it.TaxRate))" +
                                    " as decimal(12, 2))  else  (Case when(it.TaxType = 1) then cast((((ISNULL(it.UnitPrice, 0) * ISNULL(ip.Quantity, 0)) *" +
                                    " ISNULL(it.TaxRate, 0)) / (100)) as decimal(12, 2)) else 0 end ) end As TaxAmount,  cast('0' as decimal(12, 2))  as 'DiscountPercent',cast('0' as decimal(12, 2))  as 'DiscountPer'," +
                                    " cast('0' as decimal(12, 2))  as 'DiscountAmt',cast('0' as decimal(12, 2))  as CGSTPer,cast('0' as decimal(12, 2))  as SGSTPer, " +
                                    " it.TaxRate as IGSTPer,cast('0' as decimal(12, 2))  as CGSTAmt,cast('0' as decimal(12, 2))  as SGSTAmt, '' As ForOrderNo," +
                                    " Case when(it.TaxType = 0) then cast((((ISNULL(it.UnitPrice, 0) * ISNULL(ip.Quantity, 0)) * ISNULL(it.TaxRate, 0)) / (100 + it.TaxRate))" +
                                    " as decimal(12, 2))  else  (Case when(it.TaxType = 1) then cast((((ISNULL(it.UnitPrice, 0) * ISNULL(ip.Quantity, 0)) * ISNULL(it.TaxRate, 0)) / (100))" +
                                    " as decimal(12, 2)) else 0 end ) end As IGSTAmt, 0 as UnitQty,  ip.Quantity,ip.Quantity as Qty, ISNULL(it.UnitQuantity, 1) As UnitQuantity, it.UnitSize, it.UnitSurface, it.UnitGrade, ISNULL(it.Box_Weight, 0) As Box_Weight, ISNULL(it.Box_SQFT, 0) As Box_SQFT, ISNULL(it.Box_SQMT, 0) As Box_SQMT, 0 as BundleId,cast('0' as decimal(12, 2))  as HeaderDiscAmt,cast('0' as decimal(12, 2))  as AddTaxPer,cast('0' as decimal(12, 2))  as AddTaxAmt,CONVERT(NVARCHAR(10), '') as DeliveryDate, cast('' as nvarchar(20)) as Flag" +
                                    " From MST_Product_Detail ip Inner Join MST_Product it On ip.ProductID = it.pkID Where FinishProductID = "+ ProductID + ") as a Order by a.pkID";

            SqlDataReader dr = ExecuteDataReader(myCommand);

            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetAssemblyProductForProduction(Int64 ProductID)
        {
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = " Select pkID,'old' as Ref,FinishProductID,ProductID,dbo.fnGetProductName(ProductID) as ProductName,dbo.fnGetProductNameLong(ProductID) as ProductNameLong,Quantity,Quantity as Qty,MST_Product_Detail.Unit,Cast('' as nvarchar(500)) as Remarks from MST_Product_Detail Where FinishProductID = " + ProductID + " Order by pkID";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryInfoList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryListByUser";
            cmdGet.Parameters.AddWithValue("@InquiryStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InquiryInfo> lstObject = new List<Entity.InquiryInfo>();
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");                
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");
                
                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");

                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateName = GetTextVale(dr, "StateName");

                objEntity.Priority = GetTextVale(dr,"Priority");
                objEntity.ClosureReason = GetInt64(dr, "ClosureReason");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryStatusList(string pLoginUserID, string pStatus, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquiryStatusList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@InquiryStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InquiryInfo> lstObject = new List<Entity.InquiryInfo>();
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");                
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateName = GetTextVale(dr, "StateName");

                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.ClosureReason = GetInt64(dr, "ClosureReason");
                objEntity.ClosureReasonName = GetTextVale(dr, "ClosureReasonName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InquiryInfo> lstLocation = new List<Entity.InquiryInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryList";
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
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");                

                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");
                objEntity.NoFollowUp = GetBoolean(dr, "NoFollowUp");

                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.ClosureReason = GetInt64(dr, "ClosureReason");

                objEntity.AssignToEmployee = GetInt64(dr, "AssignToEmployee");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryInfoList(Int64 pkID, string LoginUserID, string SearchKey, string pStatus, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InquiryInfo> lstLocation = new List<Entity.InquiryInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@Status", pStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");

                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");
                objEntity.NoFollowUp = GetBoolean(dr, "NoFollowUp");

                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");

                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.ClosureReason = GetInt64(dr, "ClosureReason");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateName = GetTextVale(dr, "StateName");

                objEntity.AssignToEmployee = GetInt64(dr, "AssignToEmployee");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryInfoListByCustomer(Int64 pCustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryListByCustomer";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InquiryInfo> lstObject = new List<Entity.InquiryInfo>();
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryNoStatus = GetTextVale(dr, "InquiryNoStatus");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");                
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateInquiryInfo(Entity.InquiryInfo objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInquiryNo, out Int64 ReturnFollowupNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Inquiry_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@InquiryDate", objEntity.InquiryDate);
            cmdAdd.Parameters.AddWithValue("@ReferenceName", objEntity.ReferenceName);
            cmdAdd.Parameters.AddWithValue("@RefNo", objEntity.RefNo);
            cmdAdd.Parameters.AddWithValue("@InquirySource", objEntity.InquirySource);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@MeetingNotes", objEntity.MeetingNotes);
            cmdAdd.Parameters.AddWithValue("@FollowupNotes", objEntity.FollowupNotes);
            cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
            cmdAdd.Parameters.AddWithValue("@PreferredTime", objEntity.PreferredTime);
            cmdAdd.Parameters.AddWithValue("@InquiryStatusID", objEntity.InquiryStatusID);
            cmdAdd.Parameters.AddWithValue("@Priority",objEntity.Priority);
            //cmdAdd.Parameters.AddWithValue("@RefNo", objEntity.RefNo);
            cmdAdd.Parameters.AddWithValue("@ClosureReason", objEntity.ClosureReason);
            cmdAdd.Parameters.AddWithValue("@AssignToEmployee", objEntity.AssignToEmployee);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnInquiryNo", SqlDbType.NVarChar, 50);
            SqlParameter p3 = new SqlParameter("@ReturnFollowupNo", SqlDbType.BigInt);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            p3.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            cmdAdd.Parameters.Add(p3);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnInquiryNo = cmdAdd.Parameters["@ReturnInquiryNo"].Value.ToString();
            ReturnFollowupNo = Convert.ToInt64(cmdAdd.Parameters["@ReturnFollowupNo"].Value.ToString());

            ForceCloseConncetion();
        }

        public virtual void DeleteInquiryInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Inquiry_DEL";
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

        public virtual List<Entity.InquiryInfo> GetInquiryProductGroupList(string pInquiryNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryProductGroupList";
            cmdGet.Parameters.AddWithValue("@InquiryNo", pInquiryNo);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 100);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InquiryInfo> lstObject = new List<Entity.InquiryInfo>();
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateInquiryProduct(Entity.InquiryInfo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InquiryProduct_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@UnitPrice", objEntity.UnitPrice);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Thickness", objEntity.Thickness);
            cmdAdd.Parameters.AddWithValue("@Factor", objEntity.Factor);
            cmdAdd.Parameters.AddWithValue("@Area", objEntity.Area);
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

        public virtual void DeleteInquiryProductByInquiryNo(string InquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryProductByInquiryNo_DEL";
            cmdDel.Parameters.AddWithValue("@InquiryNo", InquiryNo);
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

        public virtual void DeleteInquiryProduct(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryProduct_DEL";
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

        //=======================Inquiry Owner==================================//
        public virtual void AddUpdateInquiryOwner(Entity.InquiryInfo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InquiryOwner_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
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

        public virtual List<Entity.InquiryInfo> GetInquiryOwnerListByInquiryNo(string InquiryNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryOwnerListByInquiryNo";
            cmdGet.Parameters.AddWithValue("@InquiryNo", InquiryNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InquiryInfo> lstObject = new List<Entity.InquiryInfo>();
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void DeleteInquiryOwnerByInquiryNo(string pInquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryOwnerByInquiryNo_DEL";
            cmdDel.Parameters.AddWithValue("@InquiryNo", pInquiryNo);
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
       
        //================================================================================//

        public virtual List<Entity.InquiryInfo> GetInquiryListByStatus(string pInquiryStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.InquiryInfo> lstLocation = new List<Entity.InquiryInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquiryListByStatus";
            cmdGet.Parameters.AddWithValue("@InquiryStatus", pInquiryStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.BillNo = GetTextVale(dr, "InvoiceNo");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.InquiryInfo> GetDashboardAllLeads(string pInquiryStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, Int64 pFrom, Int64 pTo)
        {
            List<Entity.InquiryInfo> lstLocation = new List<Entity.InquiryInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardAllLeads";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@InquiryStatus", pInquiryStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDays", pFrom);
            cmdGet.Parameters.AddWithValue("@ToDays", pTo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.ActivityDays = GetInt64(dr, "ActivityDays");
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.TypeOfData = GetTextVale(dr, "TypeOfData");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");
                objEntity.AssignToEmployee = GetInt64(dr, "AssignToEmployee");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                //objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                //objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                //objEntity.BillNo = GetTextVale(dr, "InvoiceNo");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CRMSummary> GetCrmAnalysisReport(string pType, Int64 pMonth, Int64 pYear)
        {
            List<Entity.CRMSummary> lstLocation = new List<Entity.CRMSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CrmAnalysisList";
            cmdGet.Parameters.AddWithValue("@Type", pType);
            cmdGet.Parameters.AddWithValue("@LoginUserID", HttpContext.Current.Session["LoginUserID"].ToString());
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.CRMSummary objEntity = new Entity.CRMSummary();
                objEntity.ByHead = GetTextVale(dr, "ByHead");
                objEntity.Deals = GetInt64(dr, "Deals");
                objEntity.WonDeal = GetInt64(dr, "WonDeal");
                objEntity.LostDeal = GetInt64(dr, "LostDeal");
                objEntity.OpenDeal = GetInt64(dr, "OpenDeal");

                objEntity.Conversion = GetDecimal(dr, "Conversion");
                objEntity.Contribution = GetDecimal(dr, "Contribution");

                objEntity.TotalRevenue = GetDecimal(dr, "TotalRevenue");
                objEntity.WonRevenue = GetDecimal(dr, "WonRevenue");
                objEntity.LostRevenue = GetDecimal(dr, "LostRevenue");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
    }
}
