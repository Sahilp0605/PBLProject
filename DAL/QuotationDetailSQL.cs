using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class QuotationDetailSQL:BaseSqlManager 
    {
        //public DataTable GetQuotationProductDetail(string pQuotationNo)
        //{
        //    DataTable dt = new DataTable();

        //    SqlCommand myCommand = new SqlCommand();
        //    myCommand.CommandType = CommandType.Text;
        //    myCommand.CommandText = "SELECT ip.pkID, ip.QuotationNo, ip.ProductID, it.ProductName As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.Unit, it.UnitPrice, it.TaxRate, ip.Quantity From Quotation_Detail ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.QuotationNo = '" + @pQuotationNo + "'";
        //    SqlDataReader dr = ExecuteDataReader(myCommand);
        //    dt.Load(dr);
        //    ForceCloseConncetion();
        //    return dt;
        //}

        public DataTable GetQuotationProductForSalesOrder(string pQuotationNo, string forOrderNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            //myCommand.CommandText = "SELECT ip.pkID, ip.QuotationNo, ip.ProductID, it.ProductName As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.Unit, it.UnitPrice As UnitRate, it.UnitPrice, ip.DiscountPercent, ip.NetRate, cast((Round(it.UnitPrice,2)*Round(ip.Quantity,2)) as decimal(12,2)) As Amount, it.TaxRate, cast((((it.UnitPrice*ip.Quantity)*it.TaxRate)/100) as decimal(12,2)) As TaxAmount, cast((Round((it.UnitPrice*ip.Quantity),2) + Round((((it.UnitPrice*ip.Quantity)*it.TaxRate)/100),2)) as decimal(12,2)) As NetAmount, ip.Quantity From Quotation_Detail ip Inner Join MST_Product it On ip.ProductID = it.pkID Where ip.QuotationNo = '" + @pQuotationNo + "'";
            if (!pQuotationNo.Contains(","))
                myCommand.CommandText = "SELECT qt.QuotationNo As DocRefNo, cast('" + forOrderNo + "' as nvarchar(20)) As OrderNo,cast('' as nvarchar(20)) as InvoiceNo, cast('' as nvarchar(20)) as InquiryNo, cast('' as nvarchar(20)) as ReferenceNo, qd.UnitRate as UnitPrice,qd.UnitRate as Rate,0 as BundleID, ISNULL(Box_Weight,0) AS Box_Weight, ISNULL(Box_SQFT,0) AS Box_SQFT, ISNULL(Box_SQMT,0) AS Box_SQMT, qd.pkID, qd.QuotationNo, qd.ProductID, CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong,qd.ProductSpecification,ISNULL(qd.UnitQty,0) as UnitQty, qd.Quantity,qd.Quantity as Qty,  ISNULL(it.UnitQuantity,1) AS UnitQuantity, qd.Unit, qd.UnitRate, qd.DiscountPercent,qd.DiscountPercent as DiscountPer, qd.NetRate, qd.Amount, qd.TaxRate, isnull(qd.TaxType,0) as TaxType, qd.TaxAmount, qd.DiscountAmt, qd.CGSTPer, qd.CGSTAmt, qd.SGSTPer, qd.SGSTAmt, qd.IGSTPer, qd.IGSTAmt, qd.NetAmount, qd.NetAmount as NetAmt,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,Qt.pkid As QtPKID,  '' As ForOrderNo, CONVERT(NVARCHAR(10),'') As DeliveryDate From Quotation_Detail qd Inner Join Quotation Qt On Qt.QuotationNo = qd.QuotationNo Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.QuotationNo = '" + @pQuotationNo + "'";
            else 
                myCommand.CommandText = "SELECT qt.QuotationNo As DocRefNo, cast('" + forOrderNo + "' as nvarchar(20)) As OrderNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,  cast('' as nvarchar(20)) as ReferenceNo, qd.UnitRate as UnitPrice,qd.UnitRate as Rate,0 as BundleID, ISNULL(Box_Weight,0) AS Box_Weight, ISNULL(Box_SQFT,0) AS Box_SQFT, ISNULL(Box_SQMT,0) AS Box_SQMT, qd.pkID, qd.QuotationNo, qd.ProductID, CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong,qd.ProductSpecification,ISNULL(qd.UnitQty,0) as UnitQty, qd.Quantity,qd.Quantity as Qty,  ISNULL(it.UnitQuantity,1) AS UnitQuantity, qd.Unit, qd.UnitRate, qd.DiscountPercent,qd.DiscountPercent as DiscountPer, qd.NetRate, qd.Amount, qd.TaxRate, isnull(qd.TaxType,0) as TaxType, qd.TaxAmount, qd.DiscountAmt, qd.CGSTPer, qd.CGSTAmt, qd.SGSTPer, qd.SGSTAmt, qd.IGSTPer, qd.IGSTAmt, qd.NetAmount, qd.NetAmount as NetAmt,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,Qt.pkid As QtPKID, qt.QuotationNo As ForOrderNo, CONVERT(NVARCHAR(10),'') As DeliveryDate From Quotation_Detail qd Inner Join Quotation Qt On Qt.QuotationNo = qd.QuotationNo Inner Join MST_Product it On qd.ProductID = it.pkID Where '" + @pQuotationNo + "' like Concat('%', qd.QuotationNo, ',%')";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetQuotationDetail(string pQuotationNo)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("ProductID", typeof(Int64));
            //dt.Columns.Add("ProductName", typeof(string));
            //dt.Columns.Add("ProductNameLong", typeof(string));
            //dt.Columns.Add("ProductSpecification", typeof(string));
            //dt.Columns.Add("pkID", typeof(Int64));
            //dt.Columns.Add("QuotationNo", typeof(string));
            //dt.Columns.Add("Quantity", typeof(decimal));
            //dt.Columns.Add("Unit", typeof(string));
            //dt.Columns.Add("UnitRate", typeof(decimal));
            //dt.Columns.Add("DiscountPercent", typeof(decimal));
            //dt.Columns.Add("NetRate", typeof(decimal));
            //dt.Columns.Add("Amount", typeof(decimal));
            //dt.Columns.Add("TaxRate", typeof(decimal));
            //dt.Columns.Add("TaxAmount", typeof(decimal));
            //dt.Columns.Add("NetAmount", typeof(decimal));
            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("QuotationVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (tmpVer=="1" || tmpVer=="2")
                myCommand.CommandText = "SELECT qd.pkID,CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, HSNCode, qd.ProductSpecification, pg.ProductGroupName, mb.BrandName, qd.pkID, qd.QuotationNo, qd.DocRefNo, qd.ProductID,qd.UnitQty as UnitQty, qd.Quantity, qd.Unit, qd.UnitRate as UnitRate,qd.UnitRate as UnitPrice, qd.DiscountPercent, qd.DiscountAmt, qd.TaxType,qd.CGSTPer , qd.SGSTPer , qd.IGSTPer, qd.CGSTAmt , qd.SGSTAmt , qd.IGSTAmt,qd.NetRate, qd.Amount, qd.TaxRate as TaxRate, qd.TaxAmount as TaxAmount, qd.NetAmount, qd.BundleId, dbo.fnGetBundleName(qd.BundleId) as BundleName, qd.CreatedBy, qd.CreatedDate, qd.UpdatedBy, qd.UpdatedDate, qd.Flag From Quotation_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID left outer join MST_ProductGroup pg on it.ProductGroupID = pg.pkID left outer join MST_Brand mb on mb.pkID = it.BrandID  Where qd.QuotationNo = '" + @pQuotationNo + "' ORDER BY qd.pkID ASC ";
            else
                myCommand.CommandText = "SELECT qd.pkID,cast('' as nvarchar(20)) as OrderNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo," +
                                        "CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.ProductAlias, pg.ProductGroupName, mb.BrandName," +
                                        " HSNCode, '' As ForOrderNo, Case When LTRIM(RTRIM(qd.ProductSpecification)) = '' THEN it.ProductSpecification Else qd.ProductSpecification END As ProductSpecification,qd.ProductSpecification as ProductSpecification1,  qd.pkID, qd.QuotationNo, qd.DocRefNo, qd.ProductID, qd.UnitQty as UnitQty, qd.Quantity,qd.Quantity as Qty, it.UnitQuantity, qd.Unit,CAST(ISNULL(SubsidyApplicable,0) As Bit) As SubsidyApplicable," +
                                        " qd.UnitRate as UnitRate,qd.UnitRate as UnitPrice,qd.UnitRate as Rate, qd.DiscountPercent,qd.DiscountPercent as DiscountPer,pg.ProductGroupName, " + 
                                        " qd.DiscountAmt, qd.TaxType, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.CGSTPer , " + 
                                        " qd.SGSTPer , qd.IGSTPer, qd.CGSTAmt , qd.SGSTAmt , qd.IGSTAmt,qd.NetRate, qd.Amount, qd.TaxRate as TaxRate1, qd.TaxAmount as TaxAmount1, " +
                                        " qd.NetAmount,qd.NetAmount as NetAmt,qd.HeaderDiscAmt As HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer, " +
                                        " it.UnitSurface, it.UnitSize, it.UnitGrade, ISNULL(it.Box_Weight,0) as Box_Weight, ISNULL(it.Box_SQFT,0) as Box_SQFT, ISNULL(it.Box_SQMT,0) as Box_SQMT," + 
                                        " cast('0' as decimal(12,2))  as AddTaxAmt, qd.BundleId, dbo.fnGetBundleName(qd.BundleId) as BundleName, qd.CreatedBy, qd.CreatedDate, qd.UpdatedBy, " +
                                        " qd.UpdatedDate, qd.Flag From Quotation_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID left outer Join MST_ProductGroup pg on it.ProductGroupID = pg.pkID left outer join MST_Brand mb on mb.pkID = it.BrandID  Where qd.QuotationNo = '" + @pQuotationNo + "' ORDER BY qd.pkID ASC";
                                    //"SELECT '" + forOrderNo + "' As OrderNo,'' as InvoiceNo,'' as InquiryNo, qd.UnitRate as UnitPrice,qd.UnitRate as Rate,0 as BundleID,qd.pkID, qd.QuotationNo, qd.ProductID, CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, qd.Quantity,qd.Quantity as Qty, qd.Unit, qd.UnitRate, qd.DiscountPercent,qd.DiscountPercent as DiscountPer, qd.NetRate, qd.Amount, qd.TaxRate, isnull(qd.TaxType,0) as TaxType, qd.TaxAmount, qd.DiscountAmt, qd.CGSTPer, qd.CGSTAmt, qd.SGSTPer, qd.SGSTAmt, qd.IGSTPer, qd.IGSTAmt, qd.NetAmount, qd.NetAmount as NetAmt,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt From Quotation_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.QuotationNo = '" + @pQuotationNo + "'";
                                    // Still You are fool
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetQuotationDetailCT(string pQuotationNo) // For ColdTech
        {
            DataTable dt = new DataTable();
            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("QuotationVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (tmpVer == "1" || tmpVer == "2")
                myCommand.CommandText = "SELECT isNull(qd.FinishProductID,0) as FinishProductID,qd.pkID,CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, HSNCode, qd.ProductSpecification, pg.ProductGroupName, mb.BrandName, qd.pkID, qd.QuotationNo, qd.DocRefNo, qd.ProductID,qd.UnitQty as UnitQty, qd.Quantity, qd.Unit, qd.UnitRate as UnitRate,qd.UnitRate as UnitPrice, qd.DiscountPercent, qd.DiscountAmt, qd.TaxType,qd.CGSTPer , qd.SGSTPer , qd.IGSTPer, qd.CGSTAmt , qd.SGSTAmt , qd.IGSTAmt,qd.NetRate, qd.Amount, qd.TaxRate as TaxRate, qd.TaxAmount as TaxAmount, qd.NetAmount, qd.BundleId, dbo.fnGetBundleName(qd.BundleId) as BundleName, qd.CreatedBy, qd.CreatedDate, qd.UpdatedBy, qd.UpdatedDate, qd.Flag From Quotation_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID left outer join MST_ProductGroup pg on it.ProductGroupID = pg.pkID left outer join MST_Brand mb on mb.pkID = it.BrandID  Where qd.QuotationNo = '" + @pQuotationNo + "' ORDER BY qd.pkID ASC ";
            else
                myCommand.CommandText = "SELECT isNull(qd.FinishProductID,0) as FinishProductID,qd.pkID,cast('' as nvarchar(20)) as OrderNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo," +
                                        "CAST(it.ProductName As nvarchar(200)) As ProductName, '[' + it.ProductAlias + '] - ' + it.ProductName As ProductNameLong, it.ProductAlias, pg.ProductGroupName, mb.BrandName," +
                                        " HSNCode, '' As ForOrderNo, Case When LTRIM(RTRIM(qd.ProductSpecification)) = '' THEN it.ProductSpecification Else qd.ProductSpecification END As ProductSpecification,qd.ProductSpecification as ProductSpecification1,  qd.pkID, qd.QuotationNo, qd.DocRefNo, qd.ProductID, qd.UnitQty as UnitQty, qd.Quantity,qd.Quantity as Qty, it.UnitQuantity, qd.Unit,CAST(ISNULL(SubsidyApplicable,0) As Bit) As SubsidyApplicable," +
                                        " qd.UnitRate as UnitRate,qd.UnitRate as UnitPrice,qd.UnitRate as Rate, qd.DiscountPercent,qd.DiscountPercent as DiscountPer,pg.ProductGroupName, " +
                                        " qd.DiscountAmt, qd.TaxType, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.CGSTPer , " +
                                        " qd.SGSTPer , qd.IGSTPer, qd.CGSTAmt , qd.SGSTAmt , qd.IGSTAmt,qd.NetRate, qd.Amount, qd.TaxRate as TaxRate1, qd.TaxAmount as TaxAmount1, " +
                                        " qd.NetAmount,qd.NetAmount as NetAmt,qd.HeaderDiscAmt As HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer, " +
                                        " it.UnitSurface, it.UnitSize, it.UnitGrade, ISNULL(it.Box_Weight,0) as Box_Weight, ISNULL(it.Box_SQFT,0) as Box_SQFT, ISNULL(it.Box_SQMT,0) as Box_SQMT," +
                                        " cast('0' as decimal(12,2))  as AddTaxAmt, qd.BundleId, dbo.fnGetBundleName(qd.BundleId) as BundleName, qd.CreatedBy, qd.CreatedDate, qd.UpdatedBy, " +
                                        " qd.UpdatedDate, qd.Flag From Quotation_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID left outer Join MST_ProductGroup pg on it.ProductGroupID = pg.pkID left outer join MST_Brand mb on mb.pkID = it.BrandID  Where qd.QuotationNo = '" + @pQuotationNo + "' ORDER BY qd.pkID ASC";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.QuotationDetail> GetQuotationDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.QuotationDetail> lstObject = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objEntity = new Entity.QuotationDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                objEntity.NetRate = GetDecimal(dr, "NetRate");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.NetAmount = GetDecimal(dr, "NetAmount");

                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTPer = GetDecimal(dr, "IGSTPer");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.BundleId = GetInt64(dr, "BundleId");
                objEntity.BundleName = GetTextVale(dr, "BundleName");
                objEntity.Flag = GetTextVale(dr, "Flag");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.QuotationDetail> GetQuotationDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.QuotationDetail> lstLocation = new List<Entity.QuotationDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationDetailList";
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
                Entity.QuotationDetail objEntity = new Entity.QuotationDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                objEntity.NetRate = GetDecimal(dr, "NetRate");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.NetAmount = GetDecimal(dr, "NetAmount");

                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTPer = GetDecimal(dr, "IGSTPer");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.BundleId = GetInt64(dr, "BundleId");
                objEntity.BundleName = GetTextVale(dr, "BundleName");
                objEntity.Flag = GetTextVale(dr, "Flag");
                // # You are Fool
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.QuotationDetail> GetQuotationDetailListByQuotationNo(string pQuotationNo, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.QuotationDetail> lstLocation = new List<Entity.QuotationDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationDetailListByQuotationNo";
            cmdGet.Parameters.AddWithValue("@QuotationNo", pQuotationNo);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.QuotationDetail objEntity = new Entity.QuotationDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.QuotationHeader = GetTextVale(dr, "QuotationHeader");
                objEntity.QuotationFooter = GetTextVale(dr, "QuotationFooter");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductNameLong = GetTextVale(dr, "ProductNameLong");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.UnitRate = GetDecimal(dr, "UnitRate");
                objEntity.DiscountPercent = GetDecimal(dr, "DiscountPercent");
                objEntity.NetRate = GetDecimal(dr, "NetRate");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.TaxRate = GetDecimal(dr, "TaxRate");
                objEntity.TaxAmount = GetDecimal(dr, "TaxAmount");
                objEntity.NetAmount = GetDecimal(dr, "NetAmount");

                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTPer = GetDecimal(dr, "IGSTPer");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxType = GetInt32(dr, "TaxType");
                objEntity.BundleId = GetInt64(dr, "BundleId");
                objEntity.BundleName = GetTextVale(dr, "BundleName");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateQuotationDetail(Entity.QuotationDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "QuotationDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@DocRefNo", objEntity.DocRefNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            //cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
            cmdAdd.Parameters.AddWithValue("@UnitQty", objEntity.UnitQty);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@UnitRate", objEntity.UnitRate);
            cmdAdd.Parameters.AddWithValue("@DiscountPercent", objEntity.DiscountPercent);
            cmdAdd.Parameters.AddWithValue("@NetRate", objEntity.NetRate);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@TaxRate", objEntity.TaxRate);
            cmdAdd.Parameters.AddWithValue("@TaxAmount", objEntity.TaxAmount);
            cmdAdd.Parameters.AddWithValue("@NetAmount", objEntity.NetAmount);
            cmdAdd.Parameters.AddWithValue("@HeaderDiscAmt", objEntity.HeaderDiscAmt);
            if (objEntity.BundleId > 0)
            {
                cmdAdd.Parameters.AddWithValue("@BundleId", objEntity.BundleId);
            }
            cmdAdd.Parameters.AddWithValue("@DiscountAmt",objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@SGSTPer",objEntity.SGSTPer );
            cmdAdd.Parameters.AddWithValue("@SGSTAmt",objEntity.SGSTAmt );
            cmdAdd.Parameters.AddWithValue("@CGSTPer",objEntity.CGSTPer);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt",objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTPer",objEntity.IGSTPer);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@TaxType",objEntity.TaxType);
            cmdAdd.Parameters.AddWithValue("@SubsidyApplicable", objEntity.SubsidyApplicable);
            cmdAdd.Parameters.AddWithValue("@Flag", objEntity.Flag);
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

        public virtual void DeleteQuotationDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationDetail_DEL";
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

        public virtual void DeleteQuotationDetailByQuotationNo(string pQuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationDetailByQuotationNo_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", pQuotationNo);
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

        public virtual void DeleteQuotationSpecByProduct(string pQuotationNo, Int64 pFinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationSpecByProduct_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", pQuotationNo);
            cmdDel.Parameters.AddWithValue("@FinishProductID", pFinishProductID);
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

        public virtual void AddUpdateQuotationSubsidy(Entity.QuotationSubsidy objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "QuotationSubsidy_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@Type", objEntity.Type);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
            cmdAdd.Parameters.AddWithValue("@SlabQty", objEntity.SlabQty);
            cmdAdd.Parameters.AddWithValue("@SlabPer", objEntity.SlabPer);
            cmdAdd.Parameters.AddWithValue("@SubsidyPer", objEntity.SubsidyPer);
            cmdAdd.Parameters.AddWithValue("@SubsidyAmt", objEntity.SubsidyAmt);
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
        public virtual List<Entity.QuotationSubsidy> GetQuotationSubsidyListByQuotationNo(string pQuotationNo, out int TotalRecord)
        {
            List<Entity.QuotationSubsidy> lstLocation = new List<Entity.QuotationSubsidy>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationSubsidyListByQuotationNo";
            cmdGet.Parameters.AddWithValue("@QuotationNo", pQuotationNo);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.QuotationSubsidy objEntity = new Entity.QuotationSubsidy();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.Type = GetTextVale(dr, "Type");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.SubsidyPer = GetDecimal(dr, "SubsidyPer");
                objEntity.SubsidyAmt = GetDecimal(dr, "SubsidyAmt");
                objEntity.SlabQty = GetDecimal(dr, "SlabQty");
                objEntity.SlabPer = GetDecimal(dr, "SlabPer");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void DeleteQuotationSubsidy(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationSubsidy_DEL";
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
        public virtual void DeleteQuotationSubsidyByQuotationNo(string pQuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationSubsidyByQuotationNo_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", pQuotationNo);
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

        public DataTable GetQuotationSubsidySummmary(string pQuotationNo)
        {
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select Type,Quantity,SubsidyPer, SUm(SubsidyAmt) As SubsidyAmt " + 
                                    "from quotation_Subsidy Where QuotationNo='"+ pQuotationNo + 
                                    "' Group by Type,Quantity,SubsidyPer";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }


        public virtual List<Entity.ProductPartDetail> GetQuotationProductPartList(String QuotationNo, Int64 FinishProductID, string LoginUserID)
        {
            List<Entity.ProductPartDetail> lstLocation = new List<Entity.ProductPartDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationProductPartList";
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ProductPartDetail objEntity = new Entity.ProductPartDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.PartDescription = GetTextVale(dr, "PartDescription");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.ItemOrder = GetTextVale(dr, "ItemOrder");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateQuotationProductParts(Entity.ProductPartDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "QuotationProductPart_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@PartDescription", objEntity.PartDescription);
            cmdAdd.Parameters.AddWithValue("@BrandName", objEntity.BrandName);
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

        public virtual void DeleteQuotationProductParts(String QuotationNo, Int64 FinishProductID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationProductParts_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            cmdDel.Parameters.AddWithValue("@FinishProductID", FinishProductID);
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
        public virtual void DeleteQuotationProductPartByQuotationNo(String QuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationProductPartByQuotationNo_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", QuotationNo);
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
        public virtual string DeleteUnwantedQuotationProductParts(string pQuotationNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From Quotation_ProductParts Where QuotationNo = '" + pQuotationNo + "' And FinishProductID NOT IN (Select ProductID From Quotation_Detail qd Where QuotationNo = Quotation_ProductParts.QuotationNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }

        public virtual void DeleteQuotationAssemblyByQuotationNo(string QuotationNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "QuotationAssemblyByQuotation_DEL";
            cmdDel.Parameters.AddWithValue("@QuotationNo", QuotationNo);
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

        public virtual void AddUpdateQuotationAssembly(Entity.QuotationDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Quotation_Assembly_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Quantity", objEntity.Quantity);
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

        public virtual List<Entity.QuotationDetail> GetQuotationAssemblyList(string QuotationNo, Int64 pProductID, Int64 pAssemblyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationAssemblyList";
            cmdGet.Parameters.AddWithValue("@QuotationNo", QuotationNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.QuotationDetail> lstObject = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objEntity = new Entity.QuotationDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.Quantity = GetDecimal(dr, "Quantity");
                objEntity.Unit = GetTextVale(dr, "Unit");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.ProductSpecification = GetTextVale(dr, "ProductSpecification");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual string DeleteUnwantedQuotationAssembly(string pQuotationNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From Quotation_Assembly Where QuotationNo = '" + pQuotationNo + "' And FinishProductID NOT IN (Select ProductID From Quotation_Detail qd Where QuotationNo = Quotation_Assembly.QuotationNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }
    }
}
