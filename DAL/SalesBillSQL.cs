using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SalesBillSQL : BaseSqlManager
    {
        public DataTable GetSalesBillNoByCustomerID(Int64 CustomerID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InvoiceNo From SalesBill qd Where qd.CustomerID = " + CustomerID.ToString();
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        

        public virtual List<Entity.SalesTaxDetail> GetTaxDetailHSNWiseForSalesBillOC(String pModule, String pInquiryNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillOtherChargeTaxSummary";
            cmdGet.Parameters.AddWithValue("@Module", pModule);
            cmdGet.Parameters.AddWithValue("@InvoiceNo", pInquiryNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesTaxDetail> lstObject = new List<Entity.SalesTaxDetail>();
            while (dr.Read())
            {
                Entity.SalesTaxDetail objEntity = new Entity.SalesTaxDetail();
                objEntity.HSNCode = GetTextVale(dr, "HSNCODE");
                objEntity.ChargeBasicAmt = GetDecimal(dr, "ChargeBasicAmt");
                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.TotalTax = GetDecimal(dr, "TaxAmount");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.SalesTaxDetail> GetTaxDetailHSNWiseForSalesBill(String pModule, String pInquiryNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesTaxDetail";
            cmdGet.Parameters.AddWithValue("@Module", pModule);
            cmdGet.Parameters.AddWithValue("@InvNo", pInquiryNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesTaxDetail> lstObject = new List<Entity.SalesTaxDetail>();
            while (dr.Read())
            {
                Entity.SalesTaxDetail objEntity = new Entity.SalesTaxDetail();
                objEntity.HSNCode = GetTextVale(dr, "HSNCode");
                objEntity.TaxableValue = GetDecimal(dr, "TaxableValue");
                objEntity.SGSTPer = GetDecimal(dr, "SGSTPer");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTPer = GetDecimal(dr, "CGSTPer");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTPer = GetDecimal(dr, "IGSTPer");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TotalTax = GetDecimal(dr, "TotalTax");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesBill> GetSalesBillList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesBill> lstObject = new List<Entity.SalesBill>();
            while (dr.Read())
            {
                Entity.SalesBill objEntity = new Entity.SalesBill();
                objEntity.pkID = GetInt64(dr, "pkId");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.CustomerID = GetInt64(dr, "CustomerId");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.BankID= GetInt64(dr, "BankID");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TerminationOfDelieryName = GetTextVale(dr, "TerminationOfDelieryName");
                objEntity.TerminationOfDelieryCity = GetInt64(dr, "TerminationOfDelieryCity");
                objEntity.TerminationOfDelieryCityName = GetTextVale(dr, "TerminationOfDelieryCityName");
                objEntity.SupplierRef = GetTextVale(dr, "SupplierRef");
                objEntity.SupplierRefDate = GetDateTime(dr, "SupplierRefDate");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderNo = GetTextVale(dr, "QuotationNo");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.RefType = GetTextVale(dr, "RefType"); 
                objEntity.OtherRef= GetTextVale(dr, "OtherRef");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");

                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.EmailSubject = GetTextVale(dr, "EmailSubject"); 

                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");

                objEntity.CRDays = GetInt64(dr,"CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.ChargeID1 = GetInt64(dr, "ChargeID1");
                objEntity.ChargeAmt1 = GetDecimal(dr, "ChargeAmt1");
                objEntity.ChargeBasicAmt1 = GetDecimal(dr, "ChargeBasicAmt1");
                objEntity.ChargeGSTAmt1 = GetDecimal(dr, "ChargeGSTAmt1");

                objEntity.ChargeID2 = GetInt64(dr, "ChargeID2");
                objEntity.ChargeAmt2 = GetDecimal(dr, "ChargeAmt2");
                objEntity.ChargeBasicAmt2 = GetDecimal(dr, "ChargeBasicAmt2");
                objEntity.ChargeGSTAmt2 = GetDecimal(dr, "ChargeGSTAmt2");

                objEntity.ChargeID3 = GetInt64(dr, "ChargeID3");
                objEntity.ChargeAmt3 = GetDecimal(dr, "ChargeAmt3");
                objEntity.ChargeBasicAmt3 = GetDecimal(dr, "ChargeBasicAmt3");
                objEntity.ChargeGSTAmt3 = GetDecimal(dr, "ChargeGSTAmt3");

                objEntity.ChargeID4 = GetInt64(dr, "ChargeID4");
                objEntity.ChargeAmt4 = GetDecimal(dr, "ChargeAmt4");
                objEntity.ChargeBasicAmt4 = GetDecimal(dr, "ChargeBasicAmt4");
                objEntity.ChargeGSTAmt4 = GetDecimal(dr, "ChargeGSTAmt4");

                objEntity.ChargeID5 = GetInt64(dr, "ChargeID5");
                objEntity.ChargeAmt5 = GetDecimal(dr, "ChargeAmt5");
                objEntity.ChargeBasicAmt5 = GetDecimal(dr, "ChargeBasicAmt5");
                objEntity.ChargeGSTAmt5 = GetDecimal(dr, "ChargeGSTAmt5");
                
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");

                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DispatchDocNo = GetTextVale(dr, "DispatchDocNo");

                objEntity.EwayBillNo = GetTextVale(dr, "EwayBillNo");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");

                //---------------------------Bank Details-----------------

                objEntity.BankName= GetTextVale(dr, "BankName");
                objEntity.BranchName= GetTextVale(dr, "BranchName");
                objEntity.BankAccountName= GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo= GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC= GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift= GetTextVale(dr, "BankSwift");

                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesBill> GetSalesBillList(string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesBill> lstObject = new List<Entity.SalesBill>();
            while (dr.Read())
            {
                Entity.SalesBill objEntity = new Entity.SalesBill();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SOpkID = GetInt64(dr, "SOpkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TerminationOfDelieryName = GetTextVale(dr, "TerminationOfDelieryName");
                objEntity.TerminationOfDelieryCity = GetInt64(dr, "TerminationOfDelieryCity");
                objEntity.TerminationOfDelieryCityName = GetTextVale(dr, "TerminationOfDelieryCityName");
                objEntity.SupplierRef = GetTextVale(dr, "SupplierRef");
                objEntity.SupplierRefDate = GetDateTime(dr, "SupplierRefDate");

                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DispatchDocNo = GetTextVale(dr, "DispatchDocNo");
                objEntity.EwayBillNo = GetTextVale(dr, "EwayBillNo");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");
                
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.BillAmount = GetDecimal(dr, "BillAmount");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.OtherRef = GetTextVale(dr, "OtherRef");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.RefType = GetTextVale(dr, "RefType");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
           
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.ChargeName1 = GetTextVale(dr, "ChargeName1");
                objEntity.ChargeName2 = GetTextVale(dr, "ChargeName2");
                objEntity.ChargeName3 = GetTextVale(dr, "ChargeName3");
                objEntity.ChargeName4 = GetTextVale(dr, "ChargeName4");
                objEntity.ChargeName5 = GetTextVale(dr, "ChargeName5");
                objEntity.ChargeID1 = GetInt64(dr, "ChargeID1");
                objEntity.ChargeAmt1 = GetDecimal(dr, "ChargeAmt1");
                objEntity.ChargeBasicAmt1 = GetDecimal(dr, "ChargeBasicAmt1");
                objEntity.ChargeGSTAmt1 = GetDecimal(dr, "ChargeGSTAmt1");
                objEntity.ChargeID2 = GetInt64(dr, "ChargeID2");
                objEntity.ChargeAmt2 = GetDecimal(dr, "ChargeAmt2");
                objEntity.ChargeBasicAmt2 = GetDecimal(dr, "ChargeBasicAmt2");
                objEntity.ChargeGSTAmt2 = GetDecimal(dr, "ChargeGSTAmt2");
                objEntity.ChargeID3 = GetInt64(dr, "ChargeID3");
                objEntity.ChargeAmt3 = GetDecimal(dr, "ChargeAmt3");
                objEntity.ChargeBasicAmt3 = GetDecimal(dr, "ChargeBasicAmt3");
                objEntity.ChargeGSTAmt3 = GetDecimal(dr, "ChargeGSTAmt3");
                objEntity.ChargeID4 = GetInt64(dr, "ChargeID4");
                objEntity.ChargeAmt4 = GetDecimal(dr, "ChargeAmt4");
                objEntity.ChargeBasicAmt4 = GetDecimal(dr, "ChargeBasicAmt4");
                objEntity.ChargeGSTAmt4 = GetDecimal(dr, "ChargeGSTAmt4");
                objEntity.ChargeID5 = GetInt64(dr, "ChargeID5");
                objEntity.ChargeAmt5 = GetDecimal(dr, "ChargeAmt5");
                objEntity.ChargeBasicAmt5 = GetDecimal(dr, "ChargeBasicAmt5");
                objEntity.ChargeGSTAmt5 = GetDecimal(dr, "ChargeGSTAmt5");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesBill> GetSalesBillList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesBill> lstLocation = new List<Entity.SalesBill>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillList";
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
                Entity.SalesBill objEntity = new Entity.SalesBill();
                objEntity.pkID = GetInt64(dr, "pkId");
                objEntity.SOpkID = GetInt64(dr, "SOpkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.CustomerID = GetInt64(dr, "CustomerId");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.EmployeeMobileNo = GetTextVale(dr, "CreatedEmployeeMobile");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.LocationName = GetTextVale(dr, "LocationName");
                objEntity.BankID= GetInt64(dr, "BankID");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TerminationOfDelieryName = GetTextVale(dr, "TerminationOfDelieryName");
                objEntity.TerminationOfDelieryCity = GetInt64(dr, "TerminationOfDelieryCity");
                objEntity.TerminationOfDelieryCityName = GetTextVale(dr, "TerminationOfDelieryCityName");
                objEntity.SupplierRef = GetTextVale(dr, "SupplierRef");
                objEntity.SupplierRefDate = GetDateTime(dr, "SupplierRefDate");
                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.EmailSubject = GetTextVale(dr, "EmailSubject");

                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.RefType = GetTextVale(dr, "RefType");

                objEntity.OtherRef = GetTextVale(dr, "OtherRef");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.ChargeName1 = GetTextVale(dr, "ChargeName1");
                objEntity.ChargeName2 = GetTextVale(dr, "ChargeName2");
                objEntity.ChargeName3 = GetTextVale(dr, "ChargeName3");
                objEntity.ChargeName4 = GetTextVale(dr, "ChargeName4");
                objEntity.ChargeName5 = GetTextVale(dr, "ChargeName5");

                objEntity.ChargeID1 = GetInt64(dr, "ChargeID1");
                objEntity.ChargeAmt1 = GetDecimal(dr, "ChargeAmt1");
                objEntity.ChargeBasicAmt1 = GetDecimal(dr, "ChargeBasicAmt1");
                objEntity.ChargeGSTAmt1 = GetDecimal(dr, "ChargeGSTAmt1");

                objEntity.ChargeID2 = GetInt64(dr, "ChargeID2");
                objEntity.ChargeAmt2 = GetDecimal(dr, "ChargeAmt2");
                objEntity.ChargeBasicAmt2 = GetDecimal(dr, "ChargeBasicAmt2");
                objEntity.ChargeGSTAmt2 = GetDecimal(dr, "ChargeGSTAmt2");

                objEntity.ChargeID3 = GetInt64(dr, "ChargeID3");
                objEntity.ChargeAmt3 = GetDecimal(dr, "ChargeAmt3");
                objEntity.ChargeBasicAmt3 = GetDecimal(dr, "ChargeBasicAmt3");
                objEntity.ChargeGSTAmt3 = GetDecimal(dr, "ChargeGSTAmt3");

                objEntity.ChargeID4 = GetInt64(dr, "ChargeID4");
                objEntity.ChargeAmt4 = GetDecimal(dr, "ChargeAmt4");
                objEntity.ChargeBasicAmt4 = GetDecimal(dr, "ChargeBasicAmt4");
                objEntity.ChargeGSTAmt4 = GetDecimal(dr, "ChargeGSTAmt4");

                objEntity.ChargeID5 = GetInt64(dr, "ChargeID5");
                objEntity.ChargeAmt5 = GetDecimal(dr, "ChargeAmt5");
                objEntity.ChargeBasicAmt5 = GetDecimal(dr, "ChargeBasicAmt5");
                objEntity.ChargeGSTAmt5 = GetDecimal(dr, "ChargeGSTAmt5");

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.DeliverTo = GetTextVale(dr, "DeliverTo");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");

                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DispatchDocNo = GetTextVale(dr, "DispatchDocNo");
                objEntity.EwayBillNo = GetTextVale(dr, "EwayBillNo");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");

                //---------------------------Bank Details-----------------

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift = GetTextVale(dr, "BankSwift");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CreatedID = GetInt64(dr, "CreatedID");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                //------------------------------------------------

                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.SalesBill> GetSalesBillList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesBill> lstLocation = new List<Entity.SalesBill>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillList";
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
                Entity.SalesBill objEntity = new Entity.SalesBill();
                objEntity.pkID = GetInt64(dr, "pkId");
                objEntity.SOpkID = GetInt64(dr, "SOpkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
                objEntity.CustomerID = GetInt64(dr, "CustomerId");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.BankID= GetInt64(dr, "BankID");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.LocationName = GetTextVale(dr, "LocationName");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TerminationOfDelieryName = GetTextVale(dr, "TerminationOfDelieryName");
                objEntity.TerminationOfDelieryCity = GetInt64(dr, "TerminationOfDelieryCity");
                objEntity.TerminationOfDelieryCityName = GetTextVale(dr, "TerminationOfDelieryCityName");
                objEntity.SupplierRef = GetTextVale(dr, "SupplierRef");
                objEntity.SupplierRefDate = GetDateTime(dr, "SupplierRefDate");
                //objEntity.InquiryNo =  GetTextVale(dr, "InquiryNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.EmailSubject = GetTextVale(dr, "EmailSubject");
                objEntity.OtherRef = GetTextVale(dr, "OtherRef");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.RefType = GetTextVale(dr, "RefType");

                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.ChargeName1 = GetTextVale(dr, "ChargeName1");
                objEntity.ChargeName2 = GetTextVale(dr, "ChargeName2");
                objEntity.ChargeName3 = GetTextVale(dr, "ChargeName3");
                objEntity.ChargeName4 = GetTextVale(dr, "ChargeName4");
                objEntity.ChargeName5 = GetTextVale(dr, "ChargeName5");

                objEntity.ChargeID1 = GetInt64(dr, "ChargeID1");
                objEntity.ChargeAmt1 = GetDecimal(dr, "ChargeAmt1");
                objEntity.ChargeBasicAmt1 = GetDecimal(dr, "ChargeBasicAmt1");
                objEntity.ChargeGSTAmt1 = GetDecimal(dr, "ChargeGSTAmt1");

                objEntity.ChargeID2 = GetInt64(dr, "ChargeID2");
                objEntity.ChargeAmt2 = GetDecimal(dr, "ChargeAmt2");
                objEntity.ChargeBasicAmt2 = GetDecimal(dr, "ChargeBasicAmt2");
                objEntity.ChargeGSTAmt2 = GetDecimal(dr, "ChargeGSTAmt2");

                objEntity.ChargeID3 = GetInt64(dr, "ChargeID3");
                objEntity.ChargeAmt3 = GetDecimal(dr, "ChargeAmt3");
                objEntity.ChargeBasicAmt3 = GetDecimal(dr, "ChargeBasicAmt3");
                objEntity.ChargeGSTAmt3 = GetDecimal(dr, "ChargeGSTAmt3");

                objEntity.ChargeID4 = GetInt64(dr, "ChargeID4");
                objEntity.ChargeAmt4 = GetDecimal(dr, "ChargeAmt4");
                objEntity.ChargeBasicAmt4 = GetDecimal(dr, "ChargeBasicAmt4");
                objEntity.ChargeGSTAmt4 = GetDecimal(dr, "ChargeGSTAmt4");

                objEntity.ChargeID5 = GetInt64(dr, "ChargeID5");
                objEntity.ChargeAmt5 = GetDecimal(dr, "ChargeAmt5");
                objEntity.ChargeBasicAmt5 = GetDecimal(dr, "ChargeBasicAmt5");
                objEntity.ChargeGSTAmt5 = GetDecimal(dr, "ChargeGSTAmt5");

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");

                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.EwayBillNo = GetTextVale(dr, "EwayBillNo");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                //---------------------------Bank Details-----------------

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift = GetTextVale(dr, "BankSwift");

                //------------------------------------------------
                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateSalesBill(Entity.SalesBill objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInvoiceNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesBill_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@InvoiceDate", objEntity.InvoiceDate);
            cmdAdd.Parameters.AddWithValue("@FixedLedgerID", objEntity.FixedLedgerID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@BankID", objEntity.BankID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@TerminationOfDeliery", objEntity.TerminationOfDeliery);
            cmdAdd.Parameters.AddWithValue("@TerminationOfDelieryCity", objEntity.TerminationOfDelieryCity);
            cmdAdd.Parameters.AddWithValue("@TermsCondition", objEntity.TermsCondition);
            cmdAdd.Parameters.AddWithValue("@EmailSubject", objEntity.EmailSubject);
            cmdAdd.Parameters.AddWithValue("@EmailContent", objEntity.EmailContent);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
            cmdAdd.Parameters.AddWithValue("@SupplierRef", objEntity.SupplierRef);
            if (objEntity.SupplierRefDate.Year>2000)
                cmdAdd.Parameters.AddWithValue("@SupplierRefDate", objEntity.SupplierRefDate);
            cmdAdd.Parameters.AddWithValue("@OtherRef", objEntity.OtherRef);
            cmdAdd.Parameters.AddWithValue("@PatientName", objEntity.PatientName);
            cmdAdd.Parameters.AddWithValue("@PatientType", objEntity.PatientType);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@Percentage", objEntity.Percentage);
            cmdAdd.Parameters.AddWithValue("@EstimatedAmt", objEntity.EstimatedAmt);

            cmdAdd.Parameters.AddWithValue("@BasicAmt", objEntity.BasicAmt);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt", objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@ROffAmt", objEntity.ROffAmt);

            cmdAdd.Parameters.AddWithValue("@CRDays", objEntity.CRDays);
            if (objEntity.DueDate.Year > 2000)
                cmdAdd.Parameters.AddWithValue("@DueDate", objEntity.DueDate);

            cmdAdd.Parameters.AddWithValue("@ChargeID1", objEntity.ChargeID1);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt1", objEntity.ChargeAmt1);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt1", objEntity.ChargeBasicAmt1);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt1", objEntity.ChargeGSTAmt1);

            cmdAdd.Parameters.AddWithValue("@ChargeID2", objEntity.ChargeID2);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt2", objEntity.ChargeAmt2);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt2", objEntity.ChargeBasicAmt2);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt2", objEntity.ChargeGSTAmt2);

            cmdAdd.Parameters.AddWithValue("@ChargeID3", objEntity.ChargeID3);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt3", objEntity.ChargeAmt3);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt3", objEntity.ChargeBasicAmt3);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt3", objEntity.ChargeGSTAmt3);

            cmdAdd.Parameters.AddWithValue("@ChargeID4", objEntity.ChargeID4);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt4", objEntity.ChargeAmt4);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt4", objEntity.ChargeBasicAmt4);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt4", objEntity.ChargeGSTAmt4);

            cmdAdd.Parameters.AddWithValue("@ChargeID5", objEntity.ChargeID5);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt5", objEntity.ChargeAmt5);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt5", objEntity.ChargeBasicAmt5);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt5", objEntity.ChargeGSTAmt5);

            cmdAdd.Parameters.AddWithValue("@NetAmt", objEntity.NetAmt);

            cmdAdd.Parameters.AddWithValue("@ModeOfTransport",objEntity.ModeOfTransport);
            cmdAdd.Parameters.AddWithValue("@TransporterName",objEntity.TransporterName);
            cmdAdd.Parameters.AddWithValue("@DeliverTo", objEntity.DeliverTo);
            cmdAdd.Parameters.AddWithValue("@VehicleNo",objEntity.VehicleNo );
            cmdAdd.Parameters.AddWithValue("@DeliveryNote", objEntity.DeliveryNote);
            cmdAdd.Parameters.AddWithValue("@DeliveryDate", objEntity.DeliveryDate);

            cmdAdd.Parameters.AddWithValue("@LRNo",objEntity.LRNo);
            cmdAdd.Parameters.AddWithValue("@LRDate", objEntity.LRDate);
            cmdAdd.Parameters.AddWithValue("@DispatchDocNo", objEntity.DispatchDocNo);

            cmdAdd.Parameters.AddWithValue("@EwayBillNo", objEntity.EwayBillNo);
            cmdAdd.Parameters.AddWithValue("@ModeOfPayment", objEntity.ModeOfPayment);
            cmdAdd.Parameters.AddWithValue("@TransportRemark",objEntity.TransportRemark);

            cmdAdd.Parameters.AddWithValue("@CurrencyName", objEntity.CurrencyName);
            cmdAdd.Parameters.AddWithValue("@CurrencySymbol", objEntity.CurrencySymbol);
            cmdAdd.Parameters.AddWithValue("@ExchangeRate", objEntity.ExchangeRate);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnInvoiceNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnInvoiceNo = cmdAdd.Parameters["@ReturnInvoiceNo"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteSalesBill(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesBill_DEL";
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
        //--------------------------------------------------------------------------------
        public DataTable GetSalesBillDetail(string pInvoiceNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT qd.DocRefNo, cast('' as nvarchar(20)) as OrderNo,HeaderDiscAmt,cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InquiryNo, dbo.fnGetOrderReferenceNo(qd.ForOrderNo) As ReferenceNo, 0 as BundleID, qd.ProductID, Cast(it.ProductName as nvarchar(150)) as ProductName, it.HSNCode,pg.ProductGroupName, Cast(Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End as nvarchar(150)) As ProductNameLong, qd.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.UnitQty as UnitQty,qd.Qty as Quantity, ISNULL(it.UnitQuantity,0) As UnitQuantity, qd.Rate as UnitRate,qd.Rate as UnitPrice,qd.DiscountPer as DiscountPercent,qd.NetAmt as NetAmount,qd.ForOrderNo, qd.* From SalesBill_Detail qd left join MST_Product it on it.pkID = qd.ProductID left join MST_ProductGroup pg on it.ProductGroupId = pg.pkid Where qd.InvoiceNo = '" + pInvoiceNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        //--------------------------------------------------------------------------------
        public DataTable GetSalesBillDetailWithDisc(string pInvoiceNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT cast('' as nvarchar(20)) as OrderNo,cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InquiryNo,0 as BundleID, qd.ProductID, it.ProductName, it.HSNCode, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, qd.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.UnitQty as UnitQty,qd.Qty as Quantity,qd.DiscountPer,qd.DiscountAmt, ISNULL(it.UnitQuantity,0) As UnitQuantity, qd.Rate as UnitRate,qd.Rate as UnitPrice,qd.DiscountPer as DiscountPercent,qd.NetAmt as NetAmount,qd.ForOrderNo, qd.* From SalesBill_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.InvoiceNo = '" + pInvoiceNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        public virtual void AddUpdateSalesBillDetail(Entity.SalesBillDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesBillDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@DocRefNo", objEntity.DocRefNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);
            cmdAdd.Parameters.AddWithValue("@UnitQty", objEntity.UnitQty);
            cmdAdd.Parameters.AddWithValue("@Qty", objEntity.Qty);
            cmdAdd.Parameters.AddWithValue("@Unit", objEntity.Unit);
            cmdAdd.Parameters.AddWithValue("@Rate", objEntity.Rate);
            cmdAdd.Parameters.AddWithValue("@DiscountPer", objEntity.DiscountPer);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt", objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@NetRate", objEntity.NetRate);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@SGSTPer", objEntity.SGSTPer);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTPer", objEntity.CGSTPer);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTPer", objEntity.IGSTPer);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);

            cmdAdd.Parameters.AddWithValue("@AddTaxPer", objEntity.AddTaxPer);
            cmdAdd.Parameters.AddWithValue("@AddTaxAmt", objEntity.AddTaxAmt);

            cmdAdd.Parameters.AddWithValue("@NetAmt", objEntity.NetAmt);
            cmdAdd.Parameters.AddWithValue("@HeaderDiscAmt", objEntity.HeaderDiscAmt);
            cmdAdd.Parameters.AddWithValue("@ForOrderNo", objEntity.ForOrderNo);
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

        public virtual void DeleteSalesBillDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesBillDetail_DEL";
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

        public virtual void DeleteSalesBillDetailByInvoiceNo(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesBillDetailByInvoiceNo_DEL";
            cmdDel.Parameters.AddWithValue("@InvoiceNo", pInvoiceNo);
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

        public virtual List<Entity.InquiryInfo> GetInquiryInfoListByHospital(Int64 pHospitalID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryListByHospital";
            cmdGet.Parameters.AddWithValue("@HospitalID", pHospitalID);
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
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.InquiryInfo> GetInquiryDetailForSalesBill(String pInquiryNo, Int64 pHospitalID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryDetailForSalesBill";
            cmdGet.Parameters.AddWithValue("@InquiryNo", pInquiryNo);
            cmdGet.Parameters.AddWithValue("@HospitalID", pHospitalID);
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
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.TreatmentType = GetTextVale(dr, "TreatmentType");               
                objEntity.Amount = GetDecimal(dr, "Amount");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // ---------------------------------------------------------------------------------------------------------------------
        // Section : Export
        // ---------------------------------------------------------------------------------------------------------------------
        public virtual List<Entity.SalesBill> GetSalesBillExportList(Int64 pkID, string InvoiceNo, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillExportList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesBill> lstObject = new List<Entity.SalesBill>();
            while (dr.Read())
            {
                Entity.SalesBill objEntity = new Entity.SalesBill();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");

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
                //objEntity.CreatedID = GetInt64(dr, "CreatedID");
                //objEntity.CompanyID = GetInt64(dr, "CompanyID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateSalesBillExport(Entity.SalesBill objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesBill_Export_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
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

        public virtual void DeleteSalesBillExport(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalerBillExport_DEL";
            cmdDel.Parameters.AddWithValue("@InvoiceNo", pInvoiceNo);
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
        public virtual List<Entity.SalesBill> GetInvoiceListByCustomer(Int64 CustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InvoiceListByCustomer";
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesBill> lstObject = new List<Entity.SalesBill>();
            while (dr.Read())
            {
                Entity.SalesBill objEntity = new Entity.SalesBill();
                objEntity.pkID = GetInt64(dr, "pkId");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.CustomerID = GetInt64(dr, "CustomerId");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.BankID = GetInt64(dr, "BankID");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TerminationOfDelieryName = GetTextVale(dr, "TerminationOfDelieryName");
                objEntity.TerminationOfDelieryCity = GetInt64(dr, "TerminationOfDelieryCity");
                objEntity.TerminationOfDelieryCityName = GetTextVale(dr, "TerminationOfDelieryCityName");
                objEntity.SupplierRef = GetTextVale(dr, "SupplierRef");
                objEntity.SupplierRefDate = GetDateTime(dr, "SupplierRefDate");

                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderNo = GetTextVale(dr, "QuotationNo");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.RefType = GetTextVale(dr, "RefType");
                objEntity.OtherRef = GetTextVale(dr, "OtherRef");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");

                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.EmailSubject = GetTextVale(dr, "EmailSubject");

                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.Amount = GetDecimal(dr, "Amount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.ChargeID1 = GetInt64(dr, "ChargeID1");
                objEntity.ChargeAmt1 = GetDecimal(dr, "ChargeAmt1");
                objEntity.ChargeBasicAmt1 = GetDecimal(dr, "ChargeBasicAmt1");
                objEntity.ChargeGSTAmt1 = GetDecimal(dr, "ChargeGSTAmt1");

                objEntity.ChargeID2 = GetInt64(dr, "ChargeID2");
                objEntity.ChargeAmt2 = GetDecimal(dr, "ChargeAmt2");
                objEntity.ChargeBasicAmt2 = GetDecimal(dr, "ChargeBasicAmt2");
                objEntity.ChargeGSTAmt2 = GetDecimal(dr, "ChargeGSTAmt2");

                objEntity.ChargeID3 = GetInt64(dr, "ChargeID3");
                objEntity.ChargeAmt3 = GetDecimal(dr, "ChargeAmt3");
                objEntity.ChargeBasicAmt3 = GetDecimal(dr, "ChargeBasicAmt3");
                objEntity.ChargeGSTAmt3 = GetDecimal(dr, "ChargeGSTAmt3");

                objEntity.ChargeID4 = GetInt64(dr, "ChargeID4");
                objEntity.ChargeAmt4 = GetDecimal(dr, "ChargeAmt4");
                objEntity.ChargeBasicAmt4 = GetDecimal(dr, "ChargeBasicAmt4");
                objEntity.ChargeGSTAmt4 = GetDecimal(dr, "ChargeGSTAmt4");

                objEntity.ChargeID5 = GetInt64(dr, "ChargeID5");
                objEntity.ChargeAmt5 = GetDecimal(dr, "ChargeAmt5");
                objEntity.ChargeBasicAmt5 = GetDecimal(dr, "ChargeBasicAmt5");
                objEntity.ChargeGSTAmt5 = GetDecimal(dr, "ChargeGSTAmt5");

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");

                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.DispatchDocNo = GetTextVale(dr, "DispatchDocNo");

                objEntity.EwayBillNo = GetTextVale(dr, "EwayBillNo");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");

                //---------------------------Bank Details-----------------

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift = GetTextVale(dr, "BankSwift");

                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol"); 
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName"); 
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate"); 

                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // ---------------------------------------------------------------------------------------------------------------------
        // Section : Job Work 
        // ---------------------------------------------------------------------------------------------------------------------
        public virtual List<Entity.SalesBillJobWork> GetSalesBillJobWorkList(Int64 pkID, string InvoiceNo, Int64 FinishProductID, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesBillJobWorkList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@FinishProductID", FinishProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesBillJobWork> lstObject = new List<Entity.SalesBillJobWork>();
            while (dr.Read())
            {
                Entity.SalesBillJobWork objEntity = new Entity.SalesBillJobWork();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.JobProductName = GetTextVale(dr, "JobProductName");
                objEntity.JobHSNCode = GetTextVale(dr, "JobHSNCode");
                objEntity.Quantity = GetDecimal(dr, "Quantity");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateSalesBillJobWork(Entity.SalesBillJobWork objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesBillJobWork_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@FinishProductID", objEntity.FinishProductID);
            cmdAdd.Parameters.AddWithValue("@JobProductName", objEntity.JobProductName);
            cmdAdd.Parameters.AddWithValue("@JobHSNCode", objEntity.JobHSNCode);
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

        public virtual void DeleteSalesBillJobWorkByInvoiceNo(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesBillJobWorkByInvoiceNo_DEL";
            cmdDel.Parameters.AddWithValue("@InvoiceNo", pInvoiceNo);
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
        public DataTable GetSalesPendingBillsByCustomerID(Int64 CustomerID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InvoiceNo From SalesBill qd Where qd.CustomerID = " + CustomerID.ToString() + " And dbo.fnCheckPendingBillStatus('sales',qd.InvoiceNo, qd.CustomerID)=1";
            //myCommand.CommandText = "Select * from ( SELECT qd.customerID, qd.InvoiceNo, (select sum(Amount) from Financial_Trans_Detail where ParentID = ft.pkID) AS PaidAmount, " +
            //                        " ft.VoucherAmount - (select sum(Amount) from Financial_Trans_Detail where ParentID = ft.pkID) AS RemainingAmount   " +
            //                        " From SalesBill qd LEFT JOIN Financial_Trans_Detail ftd on qd.InvoiceNo = ftd.InvoiceNo INNER JOIN Financial_Trans ft on ft.pkID = ftd.ParentID " +
            //                        " Where qd.CustomerID =  " + CustomerID.ToString() + " ) AS Temp WHere RemainingAmount > 0";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        public Decimal GetSalesPendingBillsAmount(String InvoiceNo)
        {
            Decimal varResult = 0;
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT dbo.fnCheckPendingBillAmount('sales','" + InvoiceNo.Trim() + "')";
            varResult = Convert.ToDecimal(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }

    }
}

