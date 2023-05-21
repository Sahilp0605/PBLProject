using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SalesOrderDealerSQL:BaseSqlManager
    {
        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderDealer> lstObject = new List<Entity.SalesOrderDealer>();
            while (dr.Read())
            {
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
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

                    objEntity.RefNo = GetTextVale(dr, "RefNo");
                    objEntity.RefType = GetTextVale(dr, "RefType");
                }

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrderDealer> lstLocation = new List<Entity.SalesOrderDealer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerList";
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
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
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

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

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
                }

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerListbyUser";
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderDealer> lstObject = new List<Entity.SalesOrderDealer>();
            while (dr.Read())
            {
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
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
                objEntity.DealerName = GetTextVale(dr, "DealerName");
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

        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerListByStatus(String pApprovalStatus, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrderDealer> lstLocation = new List<Entity.SalesOrderDealer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerListByStatus";
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
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
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

        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerListByBillStatus(String pBillingStatus, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrderDealer> lstLocation = new List<Entity.SalesOrderDealer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerListByBillStatus";
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
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
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

        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerListByCustomer(String LoginUserID, Int64 pCustID, string pStatus, Int64 pMonth, Int64 pYear, bool ForSalesBill = false)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerListbyUser";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderDealer> lstObject = new List<Entity.SalesOrderDealer>();
            while (dr.Read())
            {
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
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

        public virtual List<Entity.SalesOrderDealer> GetSalesOrderDealerListbyCustomerForSales(String LoginUserID, Int64 pCustID, string pStatus)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerListbyCustomerForSales";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderDealer> lstObject = new List<Entity.SalesOrderDealer>();
            while (dr.Read())
            {
                Entity.SalesOrderDealer objEntity = new Entity.SalesOrderDealer();
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateDealerSalesOrder(Entity.SalesOrderDealer objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderDealer_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@OrderDate", objEntity.OrderDate);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@BillNo", objEntity.BillNo);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@TermsCondition", objEntity.TermsCondition);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);

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

        public virtual void DeleteSalesOrderDealer(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDealer_DEL";
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

        //public virtual void UpdateSalesOrderApproval(Entity.SalesOrder objEntity, out int ReturnCode, out string ReturnMsg)
        //{
        //    SqlCommand cmdAdd = new SqlCommand();
        //    cmdAdd.CommandType = CommandType.StoredProcedure;
        //    cmdAdd.CommandText = "SalesOrderApproval_UPD";
        //    cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
        //    cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
        //    cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
        //    SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
        //    SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
        //    p.Direction = ParameterDirection.Output;
        //    p1.Direction = ParameterDirection.Output;
        //    cmdAdd.Parameters.Add(p);
        //    cmdAdd.Parameters.Add(p1);
        //    ExecuteNonQuery(cmdAdd);
        //    ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
        //    ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
        //    ForceCloseConncetion();
        //}

        public DataTable GetSalesOrderDealerDetail(string pOrderNo)
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
                myCommand.CommandText = "SELECT CAST(it.ProductName As NVARCHAR(200)) As ProductName,  ISNULL(Box_Weight,0) AS Box_Weight, ISNULL(Box_SQFT,0) AS Box_SQFT, ISNULL(it.Box_SQMT,0) as Box_SQMT, " +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, it.ProductSpecification, qd.* From SalesOrderDealer_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";
            else
                myCommand.CommandText = "SELECT cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as OrderNo,cast('' as nvarchar(20)) as InquiryNo,qd.UnitRate as UnitPrice,qd.UnitRate as Rate,qd.Quantity as Qty,qd.DiscountPercent as DiscountPer,qd.NetAmount as NetAmt,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount, ISNULL(Box_Weight,0) AS Box_Weight, ISNULL(Box_SQFT,0) AS Box_SQFT, ISNULL(Box_SQMT,0) AS Box_SQMT, qd.* From SalesOrderDealer_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetSalesOrderDealerDetailForSale(string pOrderNo)
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
                myCommand.CommandText = "SELECT CAST(it.ProductName As NVARCHAR(200)) As ProductName, " +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, it.ProductSpecification, qd.* From SalesOrderDealer_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";
            else
                myCommand.CommandText = "SELECT cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                        " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount, " +
                                        " qd.quantity as Qty,qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From SalesOrderDealer_Detail qd " +
                                        "Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.SalesOrderDealerDetail> GetSalesOrderDealerDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesOrderDealerDetail> lstObject = new List<Entity.SalesOrderDealerDetail>();
            while (dr.Read())
            {
                Entity.SalesOrderDealerDetail objEntity = new Entity.SalesOrderDealerDetail();
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

        public virtual List<Entity.SalesOrderDealerDetail> GetSalesOrderDealerDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.SalesOrderDealerDetail> lstLocation = new List<Entity.SalesOrderDealerDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "SalesOrderDealerDetailList";
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
                Entity.SalesOrderDealerDetail objEntity = new Entity.SalesOrderDealerDetail();
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

        public virtual void AddUpdateSalesOrderDealerDetail(Entity.SalesOrderDealerDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderDealerDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
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

        public virtual void DeleteSalesOrderDealerDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDealerDetail_DEL";
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

        public virtual void DeleteSalesOrderDealerDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDealerDetailByOrderNo_DEL";
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
            cmdGet.CommandText = "SalesOrderDealerPayScheduleList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual void AddUpdateSalesOrderDealerPaySchedule(Entity.SalesOrderDealer objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "SalesOrderDealerPaySch_INS_UPD";
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
        
        public virtual void DeleteSalesOrderDealerPaySchedule(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "SalesOrderDealerPaySch_DEL";
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
    }
}
