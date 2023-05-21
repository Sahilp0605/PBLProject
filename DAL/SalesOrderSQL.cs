using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SalesOrderSQL:BaseSqlManager
    {
        public virtual List<Entity.SalesBill> GetOutstandingBills(String pCategory, string pStatus, string ByDateType, DateTime AsOnDate, string LM1, string LM2, string LM3, string LM4, string LM5, string LM6)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PendingBillList";
            cmdGet.Parameters.AddWithValue("@BillingCategory", pCategory);
            cmdGet.Parameters.AddWithValue("@BillingStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@ByDateType", ByDateType);
            cmdGet.Parameters.AddWithValue("@AsOnDate", AsOnDate);
            cmdGet.Parameters.AddWithValue("@LM1", LM1);
            cmdGet.Parameters.AddWithValue("@LM2", LM2);
            cmdGet.Parameters.AddWithValue("@LM3", LM3);
            cmdGet.Parameters.AddWithValue("@LM4", LM4);
            cmdGet.Parameters.AddWithValue("@LM5", LM5);
            cmdGet.Parameters.AddWithValue("@LM6", LM6);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesBill> lstObject = new List<Entity.SalesBill>();
            while (dr.Read())
            {
                Entity.SalesBill objEntity = new Entity.SalesBill();

                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.BillAmount = GetDecimal(dr, "BillAmount");
                objEntity.ReceivedAmount = GetDecimal(dr, "ReceivedAmount");
                objEntity.DBAmount = GetDecimal(dr, "DBAmount");
                objEntity.CRAmount = GetDecimal(dr, "CRAmount");
                objEntity.BalanceAmount = GetDecimal(dr, "BalanceAmount");
                objEntity.BillStatus = GetTextVale(dr, "BillStatus");

                objEntity.OverdueDays = GetInt64(dr, "OverdueDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.Slab1 = GetDecimal(dr, "Slab1");
                objEntity.Slab2 = GetDecimal(dr, "Slab2");
                objEntity.Slab3 = GetDecimal(dr, "Slab3");
                objEntity.Slab4 = GetDecimal(dr, "Slab4");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.SalesOrder> GetSalesOrderList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");
                objEntity.BankID = GetInt64(dr, "BankID");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.CreditDays = GetTextVale(dr, "CreditDays");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.DeliveryTerms = GetTextVale(dr, "DeliveryTerms");
                objEntity.PaymentTerms = GetTextVale(dr, "PaymentTerms");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ProjectStage = GetTextVale(dr, "ProjectStage");
                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;
                //objEntity.ClientOrderNo = GetTextVale(dr, "ClientOrderNo");
                //objEntity.ClientOrderDate = GetDateTime(dr, "ClientOrderDate");
                //objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                //objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.PIno = GetTextVale(dr, "PIno");
                objEntity.PIdate = GetDateTime(dr, "PIdate");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");

                objEntity.BillNo = GetTextVale(dr, "BillNo");                
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                // -------------------------------------------------
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CreatedID = GetInt64(dr, "CreatedID");

                objEntity.EmailHeader = GetTextVale(dr, "EmailHeader");
                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                    objEntity.RefNo = GetTextVale(dr, "RefNo");
                    objEntity.RefType = GetTextVale(dr, "RefType");
                }

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.SalesOrder> GetSalesOrderListForProduction(Int64 pCustID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select Distinct OrderNo From ( "+
                                " Select OrderNo, CustomerID from SalesOrder Where( " +
                                " (Select ISNULL(Sum(Quantity), 0) From SalesOrder_Detail Where SalesOrder_Detail.OrderNo = SalesOrder.OrderNo) <> " +
                                " (Select ISNULL(Sum(Quantity), 0) From ProductionBySo_Detail sod Where sod.SoNo = SalesOrder.OrderNo)) " +
                                " Union " +
                                " Select OrderNo,CustomerID from SalesOrder Where ( " +
                                " Select ISNULL(Sum(Quantity), 0) From SalesOrder_Assembly Where SalesOrder_Assembly.OrderNo = SalesOrder.OrderNo) <>" +
                                " (Select ISNULL(SUM(Quantity), 0) From ProductionBySo_RawDetail Where ProductionBySo_RawDetail.SoNo = SalesOrder.OrderNo))as temp Where CustomerID = " + pCustID;
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesOrder> GetSalesOrderList(Int64 pkID,String SerialKey, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrder> lstLocation = new List<Entity.SalesOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SerialKey", SerialKey);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");
                objEntity.BankID = GetInt64(dr, "BankID");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.MobileNo = GetTextVale(dr, "MobileNo");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.CreditDays = GetTextVale(dr, "CreditDays");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.DeliveryTerms = GetTextVale(dr, "DeliveryTerms");
                objEntity.PaymentTerms = GetTextVale(dr, "PaymentTerms");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ProjectStage = GetTextVale(dr, "ProjectStage");
                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;
                //objEntity.ClientOrderNo = GetTextVale(dr, "ClientOrderNo");
                //objEntity.ClientOrderDate = GetDateTime(dr, "ClientOrderDate");
                //objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                //objEntity.TransporterName = GetTextVale(dr, "TransporterName");

                objEntity.BillNo = GetTextVale(dr, "BillNo");                
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.PIno = GetTextVale(dr, "PIno");
                objEntity.PIdate = GetDateTime(dr, "PIdate");
                objEntity.WorkOrderNo = GetTextVale(dr, "WorkOrderNo");
                objEntity.WorkOrderDate = GetDateTime(dr, "WorkOrderDate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedID = GetInt64(dr, "CreatedID");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSCCode = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFTCode= GetTextVale(dr, "BankSWIFT");
                objEntity.BranchName= GetTextVale(dr, "BranchName");

                objEntity.EmailHeader = GetTextVale(dr, "EmailHeader");
                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.DeliveryDate = GetDateTime(dr, "DeliveryDate");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                    objEntity.RefNo = GetTextVale(dr, "RefNo");
                    objEntity.RefType = GetTextVale(dr, "RefType");

                    objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                    objEntity.ApprovedBy = GetTextVale(dr, "ApprovedBy");

                }

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.SalesOrder> GetSalesOrderList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListbyUser";
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");

                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;

                objEntity.BillNo = GetTextVale(dr, "BillNo");                
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.IssuedBy = GetTextVale(dr, "IssuedBy");
                objEntity.DealerName = GetTextVale(dr, "DealerName");
                //objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                }

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.SalesOrder> GetSalesOrderListBYProjectStatus( string pLoginUserID,string ProjectStage, string pStatus)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListByStatusAndProject";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@ProjectStage", ProjectStage);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");

                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ProjectStage = GetTextVale(dr, "ProjectStage");
                objEntity.StatusRemarks = GetTextVale(dr, "StatusRemarks");
                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.IssuedBy = GetTextVale(dr, "IssuedBy");
                objEntity.DealerName = GetTextVale(dr, "DealerName");

                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                }

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesOrder> GetSalesOrderListByStatus(String pApprovalStatus, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrder> lstLocation = new List<Entity.SalesOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListByStatus";
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pApprovalStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.DeliveryTerms = GetTextVale(dr, "DeliveryTerms");
                objEntity.PaymentTerms = GetTextVale(dr, "PaymentTerms");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");

                objEntity.BillNo = GetTextVale(dr, "BillNo");                
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                // -------------------------------------------------
                //objEntity.PreCarrBy = GetTextVale(dr, "PreCarrBy");
                //objEntity.PreCarrRecPlace = GetTextVale(dr, "PreCarrRecPlace");
                //objEntity.FlightNo = GetTextVale(dr, "FlightNo");
                //objEntity.PortOfLoading = GetTextVale(dr, "PortOfLoading");
                //objEntity.PortOfDispatch = GetTextVale(dr, "PortOfDispatch");
                //objEntity.PortOfDestination = GetTextVale(dr, "PortOfDestination");
                //objEntity.MarkNo = GetTextVale(dr, "MarkNo");
                //objEntity.Packages = GetTextVale(dr, "Packages"); 

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                }
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.SalesOrder> GetSalesOrderListByBillStatus(String pBillingStatus, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrder> lstLocation = new List<Entity.SalesOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListByBillStatus";
            cmdGet.Parameters.AddWithValue("@BillingStatus", pBillingStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.ReferenceDate = GetDateTime(dr, "ReferenceDate");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");

                objEntity.BillNo = GetTextVale(dr, "BillNo");                
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                }
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.SalesOrder> GetSalesOrderListByCustomer(Int64 pCustID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select SoNo as OrderNo From ( " +
                                " Select DISTINCT ProductionBySo_Detail.SoNo,sum(Quantity) as PQty, " +
                                " (Select ISNULL(sum(Quantity), 0) From SalesOrder_Detail Where SalesOrder_Detail.OrderNo = ProductionBySo_Detail.SoNo) as SQty," +
                                " (Select ISNULL(sum(Quantity), 0) from Outward_Detail Inner Join Outward on Outward.OutwardNo = Outward_Detail.OutwardNo Where ProductionBySo_Detail.SONo = Outward.OrderNo) as OQty" +
                                " from ProductionBySo_Detail Inner JOin ProductionBySo on ProductionBySo.pkID = ProductionBySo_Detail.ParentID Where ProductionBySo_Detail.SoNo <> '' and CustomerID = " + pCustID + " Group By ProductionBySo_Detail.SoNo ) As Temp Where PQty > OQty AND SQty >= PQty";
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                lstObject.Add(objEntity);
            }
            
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.SalesOrder> GetSalesOrderListByCustomer(String LoginUserID, Int64 pCustID, string pStatus, Int64 pMonth, Int64 pYear, bool ForSalesBill = false )
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListbyUser";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            if(ForSalesBill==false)
                cmdGet.Parameters.AddWithValue("@ForModule", "Outward");

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                objEntity.BillNo = GetTextVale(dr, "BillNo");                
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                // -------------------------------------------------
                //objEntity.PreCarrBy = GetTextVale(dr, "PreCarrBy");
                //objEntity.PreCarrRecPlace = GetTextVale(dr, "PreCarrRecPlace");
                //objEntity.FlightNo = GetTextVale(dr, "FlightNo");
                //objEntity.PortOfLoading = GetTextVale(dr, "PortOfLoading");
                //objEntity.PortOfDispatch = GetTextVale(dr, "PortOfDispatch");
                //objEntity.PortOfDestination = GetTextVale(dr, "PortOfDestination");
                //objEntity.MarkNo = GetTextVale(dr, "MarkNo");
                //objEntity.Packages = GetTextVale(dr, "Packages"); 

                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                }
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.SalesOrder> GetSalesOrderListByCustomerForSaleBill(String LoginUserID, Int64 pCustID, string pStatus, Int64 pMonth, Int64 pYear, string InvoiceNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListbyUser";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                // -------------------------------------------------
                //objEntity.PreCarrBy = GetTextVale(dr, "PreCarrBy");
                //objEntity.PreCarrRecPlace = GetTextVale(dr, "PreCarrRecPlace");
                //objEntity.FlightNo = GetTextVale(dr, "FlightNo");
                //objEntity.PortOfLoading = GetTextVale(dr, "PortOfLoading");
                //objEntity.PortOfDispatch = GetTextVale(dr, "PortOfDispatch");
                //objEntity.PortOfDestination = GetTextVale(dr, "PortOfDestination");
                //objEntity.MarkNo = GetTextVale(dr, "MarkNo");
                //objEntity.Packages = GetTextVale(dr, "Packages"); 

                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                String chkVersion = DAL.CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
                if (chkVersion == "2")
                {
                    objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                    objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

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
                    objEntity.AdvPer = GetDecimal(dr, "AdvancePer");
                    objEntity.AdvAmt = GetDecimal(dr, "AdvanceAmt");

                }
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesOrder> GetSalesOrderListByCustomerProduct(Int64 pCustID, Int64 pProductID, string pStatus, String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListbyCustomerProduct";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesOrder> GetSalesOrderListbyCustomerForSales(String LoginUserID, Int64 pCustID, string pStatus)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderListbyCustomerForSales";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;           
        }

        // ============================= Insert & Update
        public virtual void AddUpdateSalesOrder(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrder_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@OrderDate", objEntity.OrderDate);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@BillNo", objEntity.BillNo);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@TermsCondition", objEntity.TermsCondition);
            cmdAdd.Parameters.AddWithValue("@DeliveryTerms", objEntity.DeliveryTerms);
            cmdAdd.Parameters.AddWithValue("@PaymentTerms", objEntity.PaymentTerms);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ReferenceNo", objEntity.ReferenceNo);
            cmdAdd.Parameters.AddWithValue("@ReferenceDate", objEntity.ReferenceDate);
            cmdAdd.Parameters.AddWithValue("@BankID", objEntity.BankID);
            cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@CreditDays", objEntity.CreditDays);

            cmdAdd.Parameters.AddWithValue("@EmailHeader", objEntity.EmailHeader);
            cmdAdd.Parameters.AddWithValue("@EmailContent", objEntity.EmailContent);
            cmdAdd.Parameters.AddWithValue("@ProjectName", objEntity.ProjectName);
            cmdAdd.Parameters.AddWithValue("@DeliveryDate", objEntity.DeliveryDate);

            //cmdAdd.Parameters.AddWithValue("@ClientOrderNo", objEntity.ClientOrderNo);
            //cmdAdd.Parameters.AddWithValue("@ClientOrderDate", objEntity.ClientOrderDate);
            //cmdAdd.Parameters.AddWithValue("@ModeOfTransport", objEntity.ModeOfTransport);
            //cmdAdd.Parameters.AddWithValue("@TransporterName", objEntity.TransporterName);

            cmdAdd.Parameters.AddWithValue("@PatientName", objEntity.PatientName);
            cmdAdd.Parameters.AddWithValue("@PatientType", objEntity.PatientType);
            cmdAdd.Parameters.AddWithValue("@FinalAmount", objEntity.FinalAmount);
            cmdAdd.Parameters.AddWithValue("@Percentage", objEntity.Percentage);
            cmdAdd.Parameters.AddWithValue("@EstimatedAmt", objEntity.EstimatedAmt);

            cmdAdd.Parameters.AddWithValue("@BasicAmt", objEntity.BasicAmt);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt", objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@ROffAmt", objEntity.ROffAmt);

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
            cmdAdd.Parameters.AddWithValue("@AdvanceAmt", objEntity.AdvAmt);
            cmdAdd.Parameters.AddWithValue("@AdvancePer", objEntity.AdvPer);

            cmdAdd.Parameters.AddWithValue("@RefType", objEntity.RefType);

            cmdAdd.Parameters.AddWithValue("@CurrencyName", objEntity.CurrencyName);
            cmdAdd.Parameters.AddWithValue("@CurrencySymbol", objEntity.CurrencySymbol);
            cmdAdd.Parameters.AddWithValue("@ExchangeRate", objEntity.ExchangeRate);

            cmdAdd.Parameters.AddWithValue("@PIno", objEntity.PIno);
            cmdAdd.Parameters.AddWithValue("@PIdate", objEntity.PIdate);
            cmdAdd.Parameters.AddWithValue("@WorkOrderNo", objEntity.WorkOrderNo);
            cmdAdd.Parameters.AddWithValue("@WorkOrderDate", objEntity.WorkOrderDate);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnOrderNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnOrderNo = cmdAdd.Parameters["@ReturnOrderNo"].Value.ToString();

            ForceCloseConncetion();
        }

        public virtual void DeleteSalesOrder(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrder_DEL";
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

        public virtual void UpdateSalesOrderApproval(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderApproval_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
            cmdAdd.Parameters.AddWithValue("@ProjectStage", objEntity.ProjectStage);
            cmdAdd.Parameters.AddWithValue("@StatusRemarks", objEntity.StatusRemarks);
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

        public virtual void UpdateSalesOrderDealerApproval(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderDealerApproval_UPD";
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

        // ============================= Insert & Update
        public virtual List<Entity.SalesOrder> GetSalesOrderLogList(String HeaderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderLogList";
            cmdGet.Parameters.AddWithValue("@HeaderID", HeaderID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", "");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            { 
                Entity.SalesOrder objEntity = new Entity.SalesOrder();
                objEntity.RowNum = GetInt64(dr, "RowNum");
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.HeaderID = GetTextVale(dr, "HeaderID");
                objEntity.ActionTaken = GetTextVale(dr, "ActionTaken");
                objEntity.TaskDescription = GetTextVale(dr, "ActionDescription");
                //objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                //objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FilePath = GetTextVale(dr, "FilePath");
                objEntity.ClosingRemarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void AddUpdateSalesOrderLog(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderLog_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@HeaderID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ActionTaken", objEntity.ActionTaken);
            cmdAdd.Parameters.AddWithValue("@ActionDescription", objEntity.TaskDescription);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.ClosingRemarks);
            cmdAdd.Parameters.AddWithValue("@FilePath", objEntity.FilePath);
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
        public DataTable GetSalesOrderDetail(string pOrderNo)
        {
            DataTable dt = new DataTable();


            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (tmpVer == "1")
                myCommand.CommandText = "SELECT qd.DocRefNo, CAST(it.ProductName As NVARCHAR(200)) As ProductName,  ISNULL(Box_Weight,0) AS Box_Weight, ISNULL(Box_SQFT,0) AS Box_SQFT, ISNULL(Box_SQMT,0) AS Box_SQMT," +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, qd.ProductSpecification," +
                                        " it.UnitSize,it.UnitSurface,it.UnitGrade, it.HSNCode,Case When ISNULL(CAST(DeliveryDate AS Date),'') = '' Then '' ELSE  Convert(NVARCHAR, DeliveryDate, 105) END As DeliveryDate," +
                                        " qd.* From SalesOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "' ORDER BY qd.pkID DESC ";
            else
                myCommand.CommandText = " SELECT qd.DocRefNo, cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as OrderNo,cast('' as nvarchar(20)) as InquiryNo," +
                                        " qd.UnitRate as UnitPrice,qd.UnitRate as Rate,qd.UnitQty as UnitQty,qd.Quantity as Qty, ISNULL(it.UnitQuantity,1) AS UnitQuantity, qd.DiscountPercent as DiscountPer,qd.NetAmount as NetAmt,qd.HeaderDiscAmt As HeaderDiscAmt," +
                                        " cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName, " + 
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                        " qd.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount," +
                                        " ISNULL(Box_Weight,0) AS Box_Weight, ISNULL(Box_SQFT,0) AS Box_SQFT, ISNULL(Box_SQMT,0) AS Box_SQMT, '' As ForOrderNo," +
                                        " it.UnitSize,it.UnitSurface,it.UnitGrade, it.HSNCode,pg.ProductGroupName,Case When ISNULL(CAST(DeliveryDate AS Date),'') = '' Then CONVERT(NVARCHAR(10),'') ELSE  Convert(NVARCHAR(10), DeliveryDate, 126) END As DeliveryDate," +
                                        " qd.* From SalesOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID " +
                                        " left join MST_ProductGroup pg on it.ProductGroupId =pg.pkid" +
                                        " Where qd.OrderNo = '" + pOrderNo + "' ORDER BY qd.pkID DESC ";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetSalesOrderDetailForSale(string pOrderNo)
        {
            DataTable dt = new DataTable();

            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            //if (tmpVer == "1")
            //    myCommand.CommandText = "SELECT CAST(it.ProductName As NVARCHAR(200)) As ProductName, " +
            //                            " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, it.ProductSpecification, qd.* From SalesOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";
            //else
            if (System.Web.HttpContext.Current.Session["SerialKey"].ToString() == "STX1-UP06-YU89-JK23")
            {
                if (!pOrderNo.Contains(","))
                    myCommand.CommandText = "SELECT sd.pkID, qd.OrderNo As DocRefNo, cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo, so.ReferenceNo, qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                            " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                            " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,  qd.OrderNo As ForOrderNo, cast('0' as decimal(12,2)) As QuantityWeight, cast('' As NVARCHAR(30)) As SerialNo, cast('' As NVARCHAR(10)) As BoxNo, " +
                                            " ISNULL(qd.UnitQty,0) as UnitQty, qd.quantity as Qty, ISNULL(it.UnitQuantity,1) As UnitQuantity, qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From SalesOrder_Detail qd Inner Join SalesOrder sd on sd.OrderNo = qd.OrderNo" +
                                            " Inner Join SalesOrder so On qd.OrderNo = so.OrderNo Inner Join MST_Product it On qd.ProductID = it.pkID Where so.ReferenceNo = '" + pOrderNo + "' And so.ReferenceNo IS NOT NULL And so.ReferenceNo<>''";
                else
                    myCommand.CommandText = "SELECT sd.pkID, qd.OrderNo As DocRefNo, cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo, so.ReferenceNo, qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                            " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                            " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,  qd.OrderNo As ForOrderNo, cast('0' as decimal(12,2)) As QuantityWeight, cast('' As NVARCHAR(30)) As SerialNo, cast('' As NVARCHAR(10)) As BoxNo, " +
                                            " ISNULL(qd.UnitQty,0) as UnitQty, qd.quantity as Qty, ISNULL(it.UnitQuantity,1) As UnitQuantity, qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From SalesOrder_Detail qd Inner Join SalesOrder sd on sd.OrderNo = qd.OrderNo" +
                                            " Inner Join SalesOrder so On qd.OrderNo = so.OrderNo Inner Join MST_Product it On qd.ProductID = it.pkID Where '" + pOrderNo + "' like Concat('%',so.ReferenceNo,',%') And so.ReferenceNo IS NOT NULL And so.ReferenceNo<>''";
            }
            else
            {
                if (!pOrderNo.Contains(","))
                    myCommand.CommandText = "SELECT sd.pkID, qd.OrderNo As DocRefNo, cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,'' as ReferenceNo,qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                            " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                            " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,  qd.OrderNo As ForOrderNo, cast('0' as decimal(12,2)) As QuantityWeight, cast('' As NVARCHAR(30)) As SerialNo, cast('' As NVARCHAR(10)) As BoxNo, " +
                                            " ISNULL(qd.UnitQty,0) as UnitQty, qd.quantity as Qty, ISNULL(it.UnitQuantity,1) As UnitQuantity, qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From SalesOrder_Detail qd Inner Join SalesOrder sd on sd.OrderNo = qd.OrderNo" +
                                            " Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";
                else
                    myCommand.CommandText = "SELECT sd.pkID, qd.OrderNo As DocRefNo, cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,'' as ReferenceNo,qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                            " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                            " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,  qd.OrderNo As ForOrderNo, cast('0' as decimal(12,2)) As QuantityWeight, cast('' As NVARCHAR(30)) As SerialNo, cast('' As NVARCHAR(10)) As BoxNo, " +
                                            " ISNULL(qd.UnitQty,0) as UnitQty, qd.quantity as Qty, ISNULL(it.UnitQuantity,1) As UnitQuantity, qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From SalesOrder_Detail qd Inner Join SalesOrder sd on sd.OrderNo = qd.OrderNo" +
                                            " Inner Join MST_Product it On qd.ProductID = it.pkID Where '" + pOrderNo + "' like Concat('%',qd.OrderNo,',%')";
            }

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }


        public DataTable GetSalesOrderDetailForOut(string pOrderNo)
        {
            DataTable dt = new DataTable();

            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT sd.pkID, qd.OrderNo As DocRefNo, cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,'' as ReferenceNo,qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName, "+
                                    " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias <> '' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,"+
                                    " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,  qd.OrderNo As ForOrderNo, cast('0' as decimal(12, 2)) As QuantityWeight, cast('' As NVARCHAR(30)) As SerialNo, cast('' As NVARCHAR(10)) As BoxNo,"+
                                    " ISNULL(qd.UnitQty, 0) as UnitQty, qd.quantity as Qty, (Select ISNULL(Sum(Quantity), 0) from Outward_Detail Inner Join Outward on Outward.OutwardNo = Outward_Detail.OutwardNo Where Outward.OrderNo = qd.OrderNo and ProductID = qd.ProductID), ISNULL(it.UnitQuantity, 1) As UnitQuantity, qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12, 2))  as HeaderDiscAmt,cast('0' as decimal(12, 2))  as AddTaxPer,cast('0' as decimal(12, 2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From SalesOrder_Detail qd Inner Join SalesOrder sd on sd.OrderNo = qd.OrderNo"+
                                    " Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "' AND qd.quantity > (Select ISNULL(Sum(Quantity), 0) from Outward_Detail Inner Join Outward on Outward.OutwardNo = Outward_Detail.OutwardNo Where Outward.OrderNo = qd.OrderNo and ProductID = qd.ProductID)";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetSalesOrderDetailForProduction(string pOrderNo)
        {
            DataTable dt = new DataTable();

            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("SalesOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select sod.pkID,sod.OrderNo,sod.ProductID,dbo.fnGetProductName(sod.ProductID) as ProductName,dbo.fnGetProductNameLong(sod.ProductID) as ProductNameLong,sod.Quantity,sod.Unit,Cast('' as nvarchar(2000)) as Remarks from SalesOrder_Detail sod Where OrderNo = '" + pOrderNo + "' Order By sod.pkID ";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        public DataTable GetSalesOrderAssemblyForProduction(string pOrderNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;

            myCommand.CommandText = "Select temp.pkID,OrderNo,OrderNo as SoNo,temp.FinishProductID, temp.FinishProductID as FinishedProductID,temp.ProductID ,dbo.fnGetProductName(temp.ProductID) as ProductName,dbo.fnGetProductNameLong(temp.ProductID) as ProductNameLong,temp.Unit,ISNULL(pd.Quantity,0) as AssQty,ActQty,(ActQty - ProQty) as Quantity from(  " +
                                    " Select sa.pkID, sa.OrderNo, sa.FinishProductID, sa.ProductID, sa.Unit, sa.Quantity as ActQty," +
                                    " (Select ISNULL(Sum(Quantity), 0) From ProductionBySo_RawDetail oas Where oas.ProductID = sa.ProductID AND oas.FinishedProductID = sa.FinishProductID And oas.SoNo = sa.OrderNo) as ProQty" +
                                    " from SalesOrder_Assembly sa ) As Temp" +
                                    " LEFT Join MST_Product_Detail pd on pd.ProductID = Temp.ProductID AND pd.FinishProductID = Temp.FinishProductID Where OrderNo = '" + pOrderNo + "' Order By temp.pkID";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetSalesOrderAssembly(string pOrderNo)
        {
            DataTable dt = new DataTable();
            
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
           
            myCommand.CommandText = "SELECT cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,it.pkID as ProductID, cast('' as nvarchar(20)) as InquiryNo,it.UnitPrice as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                    " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias <> '' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                    " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,  qd.OrderNo As ForOrderNo, cast('0' as decimal(12, 2)) As QuantityWeight, cast('' As NVARCHAR(30)) As SerialNo, cast('' As NVARCHAR(10)) As BoxNo, " +
                                    " qd.quantity as Qty,qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12, 2))  as HeaderDiscAmt,cast('0' as decimal(12, 2))  as AddTaxPer,cast('0' as decimal(12, 2))  as AddTaxAmt,NetAmount As NetAmt," +
                                    " it.pkID as pkID, itd.Quantity,it.Unit,qd.OrderNo" +
                                    " From SalesOrder_Detail qd " +
                                    " Inner Join MST_Product_Detail itd On itd.FinishProductID = qd.ProductID " +
                                    " Inner Join MST_Product it On itd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.SalesOrderDetail> GetSalesOrderDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderDetail> lstObject = new List<Entity.SalesOrderDetail>();
            while (dr.Read())
            {
                Entity.SalesOrderDetail objEntity = new Entity.SalesOrderDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
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

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesOrderDetail> GetSalesOrderDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrderDetail> lstLocation = new List<Entity.SalesOrderDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDetailList";
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
                Entity.SalesOrderDetail objEntity = new Entity.SalesOrderDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
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

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateSalesOrderDetail(Entity.SalesOrderDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@DocRefNo", objEntity.DocRefNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
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
            cmdAdd.Parameters.AddWithValue("@DeliveryDate", objEntity.DeliveryDate);
            cmdAdd.Parameters.AddWithValue("@HeaderDiscAmt", objEntity.HeaderDiscAmt);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt", objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@SGSTPer", objEntity.SGSTPer);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt", objEntity.SGSTAmt);
            cmdAdd.Parameters.AddWithValue("@CGSTPer", objEntity.CGSTPer);
            cmdAdd.Parameters.AddWithValue("@CGSTAmt", objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTPer", objEntity.IGSTPer);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt", objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);

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

        public virtual void DeleteSalesOrderDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDetail_DEL";
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

        public virtual void DeleteSalesOrderDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDetailByOrderNo_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", pOrderNo);
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
        // ================================
        public DataTable GetPayScheduleList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            DataTable dt = new DataTable();

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderPayScheduleList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual void AddUpdateSalesOrderPaySchedule(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderPaySch_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@DueDate", objEntity.DueDate);
            cmdAdd.Parameters.AddWithValue("@PayAmount", objEntity.PayAmount);
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
        public virtual void DeleteSalesOrderPaySchedule(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderPaySch_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", pOrderNo);
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

        public virtual List<Entity.SalesOrder> GetSalesOrderExportList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderExportList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");

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

        public virtual void AddUpdateSalesOrderExport(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrder_Export_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
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

        public virtual void DeleteSalesOrderExport(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalerOrderExport_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", pOrderNo);
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


        // **********************************************************************
        // Shipping details
        // **********************************************************************
        public virtual List<Entity.SalesOrder> GetSalesOrder_ShippingDetailsList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrder_ShippingDetailsList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrder> lstObject = new List<Entity.SalesOrder>();
            while (dr.Read())
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.SComapnyName = GetTextVale(dr, "SCompanyName");
                objEntity.SGSTNo = GetTextVale(dr, "SGSTNo");
                objEntity.SContactNo = GetTextVale(dr, "SContactNo");
                objEntity.SContactPersonName = GetTextVale(dr, "SContactPersonName");
                objEntity.SAddress = GetTextVale(dr, "SAddress");
                objEntity.SArea = GetTextVale(dr, "SArea");
                objEntity.SCountryCode = GetTextVale(dr, "SCountryCode");
                objEntity.SCityCode = GetInt64(dr, "SCityCode");
                objEntity.SStateCode = GetInt64(dr, "SStateCode");
                objEntity.SPinCode = GetTextVale(dr, "SPincode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                //objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                //objEntity.CompanyID = GetInt64(dr, "CompanyID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateSalesOrder_ShippingDetails(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrder_ShippingDetails_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@SCompanyName", objEntity.SComapnyName);
            cmdAdd.Parameters.AddWithValue("@SGSTNo", objEntity.SGSTNo);
            cmdAdd.Parameters.AddWithValue("@SContactNo", objEntity.SContactNo);
            cmdAdd.Parameters.AddWithValue("@SContactPersonName", objEntity.SContactPersonName);
            cmdAdd.Parameters.AddWithValue("@SAddress", objEntity.SAddress);
            cmdAdd.Parameters.AddWithValue("@SArea", objEntity.SArea);
            cmdAdd.Parameters.AddWithValue("@SCountryCode", objEntity.SCountryCode);
            cmdAdd.Parameters.AddWithValue("@SCityCode", objEntity.SCityCode);
            cmdAdd.Parameters.AddWithValue("@SStateCode", objEntity.SStateCode);
            cmdAdd.Parameters.AddWithValue("@SPincode", objEntity.SPinCode);

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

        public virtual void DeleteSalesOrder_ShippingDetails(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalerOrderExport_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", pOrderNo);
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

        // **********************************************************************
        // Employee Documents
        // **********************************************************************
        public virtual List<Entity.SalesorderDocuments> GetSalesOrderDocumentsList(Int64 pkID, Int64 pLogID, String pOrderNo)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDocumentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LogID", pLogID);
            cmdGet.Parameters.AddWithValue("@OrderNo", pOrderNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesorderDocuments> lstLocation = new List<Entity.SalesorderDocuments>();
            while (dr.Read())
            {
                Entity.SalesorderDocuments objLocation = new Entity.SalesorderDocuments();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.LogID = GetInt64(dr, "LogID");
                objLocation.OrderNo = GetTextVale(dr, "OrderNo");
                objLocation.AttachmentFile = GetTextVale(dr, "AttachmentFile");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateSalesOrderDocuments(Entity.SalesorderDocuments objEntity, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "Insert INTO SalesOrder_Attachment (LogID, OrderNo, AttachmentFile, CreatedBy)" + " Values (@LogID, @OrderNo, @AttachmentFile, @LoginUserID)";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@LogID", SqlDbType.BigInt).Value = objEntity.LogID;
                cmdAdd.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = objEntity.OrderNo;
                cmdAdd.Parameters.Add("@AttachmentFile", SqlDbType.NVarChar).Value = objEntity.AttachmentFile;
                cmdAdd.Parameters.Add("@LoginUserID", SqlDbType.NVarChar).Value = objEntity.LoginUserID;
                ExecuteNonQuery(cmdAdd);

                ReturnCode = 1;
                ReturnMsg = "File Uploaded Successfully !";
            }
            catch (Exception ex)
            {
                ReturnCode = 0;
                ReturnMsg = ex.Message.ToString();
            }
            ForceCloseConncetion();
        }
       
        public virtual List<Entity.SalesOrderProduction> GetOrderProductionDetailList(Int64 pkID, string OrderNo, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrder_Production_DetailList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderProduction> lstObject = new List<Entity.SalesOrderProduction>();
            while (dr.Read())
            {
                Entity.SalesOrderProduction objEntity = new Entity.SalesOrderProduction();
                objEntity.PkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");               
                objEntity.HKMoterMake = GetTextVale(dr, "HKMoterMake");
                objEntity.HKMoterMakeSRNO = GetTextVale(dr, "HKMoterMakeSRNO");
                objEntity.CTMotorMake = GetTextVale(dr, "CTMotorMake");
                objEntity.CTMotorMakeSRNO = GetTextVale(dr, "CTMotorMakeSRNO");
                objEntity.HTBreak = GetTextVale(dr, "HTBreak");
                objEntity.HTBreakSRNO = GetTextVale(dr, "HTBreakSRNO");
                objEntity.CTBreak = GetTextVale(dr, "CTBreak");
                objEntity.CTBreakSRNO = GetTextVale(dr, "CTBreakSRNO");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void AddUpdateSalesOrderProductionDetails(Entity.SalesOrderProduction objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrder_Production_Detail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@HKMoterMake", objEntity.HKMoterMake);
            cmdAdd.Parameters.AddWithValue("@HKMoterMakeSRNO", objEntity.HKMoterMakeSRNO);
            cmdAdd.Parameters.AddWithValue("@CTMotorMake", objEntity.CTMotorMake);
            cmdAdd.Parameters.AddWithValue("@CTMotorMakeSRNO", objEntity.CTMotorMakeSRNO);
            cmdAdd.Parameters.AddWithValue("@HTBreak", objEntity.HTBreak);
            cmdAdd.Parameters.AddWithValue("@HTBreakSRNO", objEntity.HTBreakSRNO);
            cmdAdd.Parameters.AddWithValue("@CTBreak", objEntity.CTBreak);
            cmdAdd.Parameters.AddWithValue("@CTBreakSRNO", objEntity.CTBreakSRNO);
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
        public virtual void DeleteSalesOrderProductionDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrder_Production_Detail_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", pOrderNo);
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


        //-------------------------------- SalesOrder Assembly --------------------------------------//

        public virtual void DeleteSalesOrderAssemblyByOrderNo(string OrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderAssemblyByOrder_DEL";
            cmdDel.Parameters.AddWithValue("@OrderNo", OrderNo);
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

        public virtual void AddUpdateSalesOrderAssembly(Entity.QuotationDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrder_Assembly_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
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

        public virtual List<Entity.QuotationDetail> GetSalesOrderAssemblyList(string OrderNo, Int64 pProductID, Int64 pAssemblyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderAssemblyList";
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.QuotationDetail> lstObject = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objEntity = new Entity.QuotationDetail();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.FinishProductID = GetInt64(dr, "FinishProductID");
                objEntity.FinishProductName = GetTextVale(dr, "FinishProductName");
                objEntity.AssQty = GetDecimal(dr, "AssQty");
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

        public virtual string DeleteUnwantedSalesOrderAssembly(string OrderNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Delete From SalesOrder_Assembly Where OrderNo = '" + OrderNo + "' And FinishProductID NOT IN (Select ProductID From SalesOrder_Detail qd Where OrderNo = SalesOrder_Assembly.OrderNo)";
            ExecuteNonQuery(myCommand);
            ForceCloseConncetion();
            return "";
        }
    }
}
