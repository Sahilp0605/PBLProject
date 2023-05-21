using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PurchaseOrderSQL : BaseSqlManager
    {
        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseOrder> lstObject = new List<Entity.PurchaseOrder>();
            while (dr.Read())
            {
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.BuyerRef = GetTextVale(dr, "BuyerRef");
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
                objEntity.EmployeeMobileNo = GetTextVale(dr, "EmployeeMobileNo");
                objEntity.EmployeeEmailAddress = GetTextVale(dr, "EmployeeEmailAddress");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.EmailHeader = GetTextVale(dr, "EmailHeader");
                objEntity.EmailContent = GetTextVale(dr, "EmailContent");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");
                //objEntity.ClientOrderNo = GetTextVale(dr, "ClientOrderNo");
                //objEntity.ClientOrderDate = GetDateTime(dr, "ClientOrderDate");
                //objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                //objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");
                objEntity.CreatedID = GetInt64(dr, "CreatedID");
                objEntity.ApprovedID = GetInt64(dr, "ApprovedID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

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

                objEntity.TankerNo = GetTextVale(dr, "TankerNo");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");
                objEntity.DriverDetails = GetTextVale(dr, "DriverDetails");
                objEntity.DriverName = GetTextVale(dr, "DriverName");
                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.DriverNumber = GetTextVale(dr, "DriverNumber");
                objEntity.ConductorName = GetTextVale(dr, "ConductorName");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.ConsigneeName = GetTextVale(dr, "ConsigneeName");
                objEntity.ConsigneeAddress = GetTextVale(dr, "ConsigneeAddress");
                objEntity.TripDistance = GetTextVale(dr, "TripDistance");

                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.RefType = GetTextVale(dr, "RefType");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PurchaseOrder> lstLocation = new List<Entity.PurchaseOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderList";
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
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.BuyerRef = GetTextVale(dr, "BuyerRef");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
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
                objEntity.CreatedID = GetInt64(dr, "CreatedID");
                objEntity.ApprovedID = GetInt64(dr, "ApprovedID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.EmployeeMobileNo = GetTextVale(dr, "EmployeeMobileNo");
                objEntity.EmployeeEmailAddress = GetTextVale(dr, "EmployeeEmailAddress");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.OrderAmount = GetDecimal(dr, "OrderAmount");
                objEntity.ApprovalStatus = GetTextVale(dr, "ApprovalStatus");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.CurrencySymbol = "";
                objEntity.CurrencyName = "";
                objEntity.ExchangeRate = 0;
                //objEntity.ClientOrderNo = GetTextVale(dr, "ClientOrderNo");
                //objEntity.ClientOrderDate = GetDateTime(dr, "ClientOrderDate");
                //objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                //objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.DeliveryNote = GetTextVale(dr, "DeliveryNote");
                objEntity.EmailHeader = GetTextVale(dr, "EmailHeader");
                objEntity.EmailContent = GetTextVale(dr, "EmailContent");

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");
                objEntity.ApprovedEmployeeName = GetTextVale(dr, "ApprovedEmployeeName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CreatedDesigCode = GetTextVale(dr, "CreatedDesigCode");
                objEntity.CreatedDesignation = GetTextVale(dr, "CreatedDesignation");
                objEntity.ApprovedDesignation = GetTextVale(dr, "ApprovedDesignation");

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

                objEntity.TankerNo = GetTextVale(dr, "TankerNo");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");
                objEntity.DriverDetails = GetTextVale(dr, "DriverDetails");
                objEntity.DriverName = GetTextVale(dr, "DriverName");
                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.DriverNumber = GetTextVale(dr, "DriverNumber");
                objEntity.ConductorName = GetTextVale(dr, "ConductorName");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.ConsigneeName = GetTextVale(dr, "ConsigneeName");
                objEntity.ConsigneeAddress = GetTextVale(dr, "ConsigneeAddress");
                objEntity.TripDistance = GetTextVale(dr, "TripDistance");

                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");

                objEntity.RefNo = GetTextVale(dr, "RefNo");
                objEntity.RefType = GetTextVale(dr, "RefType");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderListbyUser";
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseOrder> lstObject = new List<Entity.PurchaseOrder>();
            while (dr.Read())
            {
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.BuyerRef = GetTextVale(dr, "BuyerRef");
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
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PatientName = GetTextVale(dr, "PatientName");
                objEntity.PatientType = GetTextVale(dr, "PatientType");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.Percentage = GetDecimal(dr, "Percentage");
                objEntity.EstimatedAmt = GetDecimal(dr, "EstimatedAmt");

                objEntity.TankerNo = GetTextVale(dr, "TankerNo");
                objEntity.Gross_Weight = GetDecimal(dr, "Gross_Weight");
                objEntity.Tare_Weight = GetDecimal(dr, "Tare_Weight");
                objEntity.Net_Weight = GetDecimal(dr, "Net_Weight");
                objEntity.LicenseNo = GetTextVale(dr, "LicenseNo");
                objEntity.DriverDetails = GetTextVale(dr, "DriverDetails");
                objEntity.DriverName = GetTextVale(dr, "DriverName");
                objEntity.DrivingLicenseNo = GetTextVale(dr, "DrivingLicenseNo");
                objEntity.DriverNumber = GetTextVale(dr, "DriverNumber");
                objEntity.ConductorName = GetTextVale(dr, "ConductorName");
                objEntity.ModeOfPayment = GetTextVale(dr, "ModeOfPayment");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.ConsigneeName = GetTextVale(dr, "ConsigneeName");
                objEntity.ConsigneeAddress = GetTextVale(dr, "ConsigneeAddress");
                objEntity.TripDistance = GetTextVale(dr, "TripDistance");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                String chkVersion = DAL.CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
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

        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderListByStatus(String pApprovalStatus, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PurchaseOrder> lstLocation = new List<Entity.PurchaseOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderListByStatus";
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
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

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

                String chkVersion = DAL.CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
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

        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderListByBillStatus(String pBillingStatus, String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PurchaseOrder> lstLocation = new List<Entity.PurchaseOrder>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderListByBillStatus";
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
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

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

                String chkVersion = DAL.CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
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

        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderListByCustomer(String LoginUserID, Int64 pCustID, Int64 pProductID, string pStatus, Int64 pMonth, Int64 pYear, bool ForSalesBill = false)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderListbyCustomer";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseOrder> lstObject = new List<Entity.PurchaseOrder>();
            while (dr.Read())
            {
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.BuyerRef = GetTextVale(dr, "BuyerRef");
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
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
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


                String chkVersion = DAL.CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
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
        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderListByCust(Int64 pCustID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderListbyCust";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            //cmdGet.Parameters.AddWithValue("@ListMode", "");
            //cmdGet.Parameters.AddWithValue("@PageNo", 1);
            //cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            //SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseOrder> lstObject = new List<Entity.PurchaseOrder>();
            while (dr.Read())
            {
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.OrderDate = GetDateTime(dr, "OrderDate");
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
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
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


                String chkVersion = DAL.CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
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


        public virtual List<Entity.PurchaseOrder> GetPurchaseOrderListbyCustomerForSales(String LoginUserID, Int64 pCustID, string pStatus)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderListbyCustomerForSales";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdGet.Parameters.AddWithValue("@ApprovalStatus", pStatus);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseOrder> lstObject = new List<Entity.PurchaseOrder>();
            while (dr.Read())
            {
                Entity.PurchaseOrder objEntity = new Entity.PurchaseOrder();
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual void UpdatePurchaseOrderApproval(Entity.PurchaseOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseOrderApproval_UPD";
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
        public virtual void AddUpdatePurchaseOrder(Entity.PurchaseOrder objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseOrder_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@OrderDate", objEntity.OrderDate);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@BuyerRef", objEntity.BuyerRef);
            cmdAdd.Parameters.AddWithValue("@BillNo", objEntity.BillNo);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@TermsCondition", objEntity.TermsCondition);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ApprovalStatus", objEntity.ApprovalStatus);
            cmdAdd.Parameters.AddWithValue("@ProjectName", objEntity.ProjectName);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@DeliveryNote", objEntity.DeliveryNote);
            cmdAdd.Parameters.AddWithValue("@EmailHeader", objEntity.EmailHeader);
            cmdAdd.Parameters.AddWithValue("@EmailContent", objEntity.EmailContent);

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

            cmdAdd.Parameters.AddWithValue("@TankerNo", objEntity.TankerNo);
            cmdAdd.Parameters.AddWithValue("@Gross_Weight", objEntity.Gross_Weight);
            cmdAdd.Parameters.AddWithValue("@Tare_Weight", objEntity.Tare_Weight);
            cmdAdd.Parameters.AddWithValue("@Net_Weight", objEntity.Net_Weight);
            cmdAdd.Parameters.AddWithValue("@LicenseNo", objEntity.LicenseNo);
            cmdAdd.Parameters.AddWithValue("@DriverDetails", objEntity.DriverDetails);
            cmdAdd.Parameters.AddWithValue("@DriverName", objEntity.DriverName);
            cmdAdd.Parameters.AddWithValue("@DrivingLicenseNo", objEntity.DrivingLicenseNo);
            cmdAdd.Parameters.AddWithValue("@DriverNumber", objEntity.DriverNumber);
            cmdAdd.Parameters.AddWithValue("@ConductorName", objEntity.ConductorName);
            cmdAdd.Parameters.AddWithValue("@ModeOfPayment", objEntity.ModeOfPayment);
            cmdAdd.Parameters.AddWithValue("@TransporterName", objEntity.TransporterName);
            cmdAdd.Parameters.AddWithValue("@ConsigneeName", objEntity.ConsigneeName);
            cmdAdd.Parameters.AddWithValue("@ConsigneeAddress", objEntity.ConsigneeAddress);
            cmdAdd.Parameters.AddWithValue("@TripDistance", objEntity.TripDistance);

            cmdAdd.Parameters.AddWithValue("@CurrencyName", objEntity.CurrencyName);
            cmdAdd.Parameters.AddWithValue("@CurrencySymbol", objEntity.CurrencySymbol);
            cmdAdd.Parameters.AddWithValue("@ExchangeRate", objEntity.ExchangeRate);


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

        public virtual void DeletePurchaseOrder(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseOrder_DEL";
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



        public DataTable GetPurchaseOrderDetail(string pOrderNo)
        {
            DataTable dt = new DataTable();


            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (tmpVer == "1")
                myCommand.CommandText = "SELECT CAST(it.ProductName As NVARCHAR(200)) As ProductName, " +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,it.HSNCode, qd.ProductSpecification,Case When ISNULL(CAST(DeliveryDate AS Date),'') = '' Then CONVERT(NVARCHAR(10),'') ELSE  Convert(NVARCHAR(10), DeliveryDate, 126) END As DeliveryDate, qd.* From PurchaseOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "' ORDER BY qd.pkID ASC ";
            else
                myCommand.CommandText = "SELECT cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo, " +
                                        " qd.UnitRate as UnitPrice,qd.UnitRate as Rate,qd.Quantity as Qty,qd.DiscountPercent as DiscountPer,qd.NetAmount as NetAmt, " +
                                        " cast(qd.HeaderDiscAmt as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt," +
                                        " 0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong," +
                                        " it.HSNCode,ISNULL(pg.ProductGroupName,'') as ProductGroupName,qd.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount," +
                                        " Case When ISNULL(CAST(DeliveryDate AS Date),'') = '' Then CONVERT(NVARCHAR(10),'') ELSE  Convert(NVARCHAR(10), DeliveryDate, 126) END As DeliveryDate,qd.* " +
                                        " From PurchaseOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID left join MST_ProductGroup pg on it.ProductGroupId =pg.pkid Where qd.OrderNo = '" + pOrderNo + "' ORDER BY qd.pkID ASC ";
                                        

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetPurchaseOrderDetailForSale(string pOrderNo)
        {
            DataTable dt = new DataTable();


            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (tmpVer == "1")
                myCommand.CommandText = "SELECT CAST(it.ProductName As NVARCHAR(200)) As ProductName, " +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, it.ProductSpecification, qd.* From PurchaseOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";
            else
                myCommand.CommandText = "SELECT cast('' as nvarchar(20)) as QuotationNo,cast('' as nvarchar(20)) as InvoiceNo,cast('' as nvarchar(20)) as InquiryNo,qd.UnitRate as UnitPrice,0 as BundleID,CAST(it.ProductName As NVARCHAR(200)) As ProductName," +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                                        " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount, " +
                                        " qd.quantity as Qty,qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt,qd.* From PurchaseOrder_Detail qd " +
                                        "Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public DataTable GetPurchaseOrderDetailForInward(string pOrderNo)
        {
            DataTable dt = new DataTable();
            // ------------------------------------------------------------------
            // Checking Quotation Version 
            // ------------------------------------------------------------------
            String tmpVer = CommonSQL.GetConstant("PurchaseOrderVersion", 0, 1);
            // ------------------------------------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (tmpVer == "1")
                myCommand.CommandText = "SELECT CAST(it.ProductName As NVARCHAR(200)) As ProductName, " +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, it.ProductSpecification, qd.* From PurchaseOrder_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.OrderNo = '" + pOrderNo + "'";
            else
                //myCommand.CommandText = " Select *, OrderQty-InwardQty As Qty from (" +
                //                        " SELECT qd.UnitRate as UnitPrice,0 as BundleID,qd.ProductID,CONCAT(qd.ProductID,'-',qd.OrderNo) As OrdProductID, CONCAT(qd.OrderNo,' : ',CAST(it.ProductName As NVARCHAR(200)),' : ',qd.quantity,' ',it.Unit) As ProductName, " +
                //                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong, " +
                //                        " it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount, " +
                //                        " qd.quantity as OrderQty," +
                //                        " (Select sum(Quantity) From Inward_Detail id where id.OrderNo=qd.OrderNo And id.ProductID=qd.ProductID) As InwardQty," +
                //                        " qd.UnitRate as Rate,qd.DiscountPercent As DiscountPer,cast('0' as decimal(12,2))  as HeaderDiscAmt,cast('0' as decimal(12,2))  as AddTaxPer,cast('0' as decimal(12,2))  as AddTaxAmt,NetAmount As NetAmt From PurchaseOrder_Detail qd " +
                //                        " Inner Join MST_Product it On qd.ProductID = it.pkID " +
                //                        " Inner Join PurchaseOrder qt On qd.OrderNo = qt.OrderNo " +
                //                        " Where qd.OrderNo in (" + pOrderNo + ")) as Temp" +
                //                        " Having  OrderQty-InwardQty > 0"+
                //                        " Order By qt.OrderDate DESC, qt.OrderNo DESC,it.ProductName";

                myCommand.CommandText = " Select *, OrderQty-InwardQty as Quantity,CONCAT(OrderNo,' : ',CAST(ProductName As NVARCHAR(200)),' : ',OrderQty-InwardQty,' ',Unit) As DisplayProductName From( " +
                                        " Select ProductID,it.TaxType,it.ProductName," +
                                        " CONCAT(qd.ProductID,'-',qd.OrderNo) As OrdProductID, " +
                                        " Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,   " +
                                        " Quantity as OrderQty,ISNULL( (Select sum(Quantity) From Inward_Detail id where id.OrderNo=qd.OrderNo And id.ProductID=qd.ProductID),0) As InwardQty,  " +
                                        " UnitRate,qd.Unit,DiscountPercent,qd.DiscountAmt,NetRate,Amount,CGSTPer,SGSTPer,IGSTPer,(qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate, " +
                                        " qd.CGSTAmt,qd.SGSTAmt,qd.IGSTAmt,TaxAmount,NetAmount,qd.OrderNo,qt.OrderDate,qd.pkID " +
                                        " From PurchaseOrder_Detail qd  " +
                                        " Inner Join MST_Product it On qd.ProductID = it.pkID   " +
                                        " Inner Join PurchaseOrder qt On qd.OrderNo = qt.OrderNo " +
                                        " Where qd.OrderNo in(" + pOrderNo + ") " +
                                        " ) As Temp " +
                                        " Where (OrderQty-InwardQty) > 0" +
                                        " Order By OrderDate DESC, OrderNo DESC,ProductName ";


            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.PurchaseOrderDetail> GetPurchaseOrderDetailList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderDetailList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseOrderDetail> lstObject = new List<Entity.PurchaseOrderDetail>();
            while (dr.Read())
            {
                Entity.PurchaseOrderDetail objEntity = new Entity.PurchaseOrderDetail();
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
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.PurchaseOrderDetail> GetPurchaseOrderDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PurchaseOrderDetail> lstLocation = new List<Entity.PurchaseOrderDetail>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseOrderDetailList";
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
                Entity.PurchaseOrderDetail objEntity = new Entity.PurchaseOrderDetail();
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
                objEntity.IndentNo = GetTextVale(dr, "IndentNo");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdatePurchaseOrderDetail(Entity.PurchaseOrderDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseOrderDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@ProductSpecification", objEntity.ProductSpecification);
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
            cmdAdd.Parameters.AddWithValue("@IndentNo", objEntity.IndentNo);
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

        public virtual void DeletePurchaseOrderDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseOrderDetail_DEL";
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

        public virtual void DeletePurchaseOrderDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseOrderDetailByOrderNo_DEL";
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
            cmdGet.CommandText = "PurchaseOrderPayScheduleList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual void AddUpdatePurchaseOrderPaySchedule(Entity.PurchaseOrder objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseOrderPaySch_INS_UPD";
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
        public virtual void DeletePurchaseOrderPaySchedule(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseOrderPaySch_DEL";
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

        //-------------------------------Check Point ----------------------
        public DataTable GetCheckList(string OrderNo)
        {
            DataTable dt = new DataTable();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pkID,OrderNo,CheckPointDetail,Comment,Date from PurchaseOrder_CheckList Where OrderNo = '" + OrderNo + "'";
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual void AddUpdatePurchaseOrderCheckList(Entity.PurchaseOrderCheckList objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseOrder_CheckList_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);
            cmdAdd.Parameters.AddWithValue("@CheckPointDetail", objEntity.CheckPointDetail);
            cmdAdd.Parameters.AddWithValue("@Comment", objEntity.Comment);
            cmdAdd.Parameters.AddWithValue("@Date", objEntity.Date);
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
        public virtual void DeletePurchaseOrderCheckList(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CheckListDetailByPurchaseOrder_DEL";
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
