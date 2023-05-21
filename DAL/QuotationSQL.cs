using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mime;
using System.Net.Mail;
using System.Configuration;

namespace DAL
{
    public class QuotationSQL:BaseSqlManager
    {
        public virtual List<Entity.Quotation> GetQuotationListByCustomer(Int64 pCustomerID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationListByCustomer";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Quotation> lstObject = new List<Entity.Quotation>();
            while (dr.Read())
            {
                Entity.Quotation objEntity = new Entity.Quotation();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquirypkID = GetInt64(dr, "InquirypkID");
                objEntity.QuotationSubject = GetTextVale(dr, "QuotationSubject");
                objEntity.QuotationKindAttn = GetTextVale(dr, "QuotationKindAttn");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");

                objEntity.QuotationHeader = GetTextVale(dr, "QuotationHeader");
                objEntity.QuotationFooter = GetTextVale(dr, "QuotationFooter");
                objEntity.AssumptionRemark = GetTextVale(dr, "AssumptionRemark");
                objEntity.AdditionalRemark = GetTextVale(dr, "AdditionalRemark");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.QuotationAmount = GetDecimal(dr, "QuotationAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.CreatedEmployeeMobileNo = GetTextVale(dr, "CreatedEmployeeMobileNo");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");
                
                String chkVersion = DAL.CommonSQL.GetConstant("QuotationVersion", 0, 1);
                
                if (chkVersion == "3")
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
                }
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Quotation> GetQuotationList(string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationList";
            cmdGet.Parameters.AddWithValue("@pkID", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Quotation> lstObject = new List<Entity.Quotation>();
            while (dr.Read())
            {
                Entity.Quotation objEntity = new Entity.Quotation();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquirypkID = GetInt64(dr, "InquirypkID");
                objEntity.QuotationSubject = GetTextVale(dr, "QuotationSubject");
                objEntity.QuotationKindAttn = GetTextVale(dr, "QuotationKindAttn");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.BankId = GetInt64(dr, "BankId");
                //objEntity.QuotationType = GetTextVale(dr, "QType");
                objEntity.CreditDays = GetTextVale(dr, "CreditDays");
                objEntity.QuotationHeader = GetTextVale(dr, "QuotationHeader");
                objEntity.QuotationFooter = GetTextVale(dr, "QuotationFooter");
                objEntity.AssumptionRemark = GetTextVale(dr, "AssumptionRemark");
                objEntity.AdditionalRemark = GetTextVale(dr, "AdditionalRemark");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.QuotationAmount = GetDecimal(dr, "QuotationAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.CreatedEmployeeMobileNo = GetTextVale(dr, "CreatedEmployeeMobileNo");
                //objEntity.AuthorizedSign = GetTextVale(dr, "AuthorizedSign");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");
                //----------------Bank Details----------------------
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                //-------------------------------------------
                String chkVersion = DAL.CommonSQL.GetConstant("QuotationVersion", 0, 1);
                if (chkVersion == "3")
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
                }

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Quotation> GetQuotationList(Int64 pkID, string pLoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Quotation> lstLocation = new List<Entity.Quotation>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Quotation objEntity = new Entity.Quotation();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquirypkID = GetInt64(dr, "InquirypkID");
                objEntity.InquiryDate= GetDateTime(dr, "InquiryDate");
                objEntity.QuotationSubject = GetTextVale(dr, "QuotationSubject");
                objEntity.QuotationKindAttn = GetTextVale(dr, "QuotationKindAttn");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.BankId = GetInt64(dr, "BankId");
                //objEntity.QuotationType = GetTextVale(dr, "QType");
                objEntity.CreditDays = GetTextVale(dr, "CreditDays");
                objEntity.QuotationHeader = GetTextVale(dr, "QuotationHeader");
                objEntity.QuotationFooter = GetTextVale(dr, "QuotationFooter");
                objEntity.AssumptionRemark = GetTextVale(dr, "AssumptionRemark");
                objEntity.AdditionalRemark = GetTextVale(dr, "AdditionalRemark");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.QuotationAmount = GetDecimal(dr, "QuotationAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CreatedID= GetInt64(dr, "CreatedID");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.CreatedEmployeeMobileNo = GetTextVale(dr, "CreatedEmployeeMobileNo");
                objEntity.EmployeeEmailAddress = GetTextVale(dr, "EmployeeEmailAddress");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");
                //----------------Bank Details----------------------
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                //-------------------------------------------
                String chkVersion = DAL.CommonSQL.GetConstant("QuotationVersion", 0, 1);
                if (chkVersion == "3")
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
                }

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Quotation> GetQuotationList(Int64 pkID, string pLoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Quotation> lstLocation = new List<Entity.Quotation>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Quotation objEntity = new Entity.Quotation();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquirypkID = GetInt64(dr, "InquirypkID");
                objEntity.QuotationSubject = GetTextVale(dr, "QuotationSubject");
                objEntity.QuotationKindAttn = GetTextVale(dr, "QuotationKindAttn");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.BankId = GetInt64(dr, "BankId");
                objEntity.CreditDays = GetTextVale(dr, "CreditDays");
                objEntity.QuotationHeader = GetTextVale(dr, "QuotationHeader");
                objEntity.QuotationFooter = GetTextVale(dr, "QuotationFooter");
                objEntity.AssumptionRemark = GetTextVale(dr, "AssumptionRemark");
                objEntity.AdditionalRemark = GetTextVale(dr, "AdditionalRemark");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                //objEntity.QuotationType = GetTextVale(dr, "QType");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.QuotationAmount = GetDecimal(dr, "QuotationAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CreatedID = GetInt64(dr, "CreatedID");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.CreatedEmployeeMobileNo = GetTextVale(dr, "CreatedEmployeeMobileNo");
                //objEntity.AuthorizedSign = GetTextVale(dr, "AuthorizedSign");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");
                //----------------Bank Details----------------------
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                //-------------------------------------------
                String chkVersion = DAL.CommonSQL.GetConstant("QuotationVersion", 0, 1);
                if (chkVersion == "3")
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
                }

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Quotation> GetQuotationByUser(string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuotationListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Quotation> lstObject = new List<Entity.Quotation>();
            while (dr.Read())
            {
                Entity.Quotation objEntity = new Entity.Quotation();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquirypkID = GetInt64(dr, "InquirypkID");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.QuotationDate = GetDateTime(dr, "QuotationDate");
                objEntity.QuotationSubject = GetTextVale(dr, "QuotationSubject");
                objEntity.QuotationKindAttn = GetTextVale(dr, "QuotationKindAttn");
                objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                objEntity.QuotationHeader = GetTextVale(dr, "QuotationHeader");
                objEntity.QuotationFooter = GetTextVale(dr, "QuotationFooter");
                objEntity.AssumptionRemark = GetTextVale(dr, "AssumptionRemark");
                objEntity.AdditionalRemark = GetTextVale(dr, "AdditionalRemark");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.PinCode = GetTextVale(dr, "Pincode");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.QuotationAmount = GetDecimal(dr, "QuotationAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CreatedID = GetInt64(dr, "CreatedID");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.CreatedEmployeeMobileNo = GetTextVale(dr, "CreatedEmployeeMobileNo");
                objEntity.UpdatedEmployeeName = GetTextVale(dr, "UpdatedEmployeeName");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.CurrencyName = GetTextVale(dr, "CurrencyName");
                objEntity.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");
                objEntity.ExchangeRate = GetDecimal(dr, "ExchangeRate");
                //objEntity.QuotationType = GetTextVale(dr, "QType");
                String chkVersion = DAL.CommonSQL.GetConstant("QuotationVersion", 0, 1);
                if (chkVersion == "3")
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
                }

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateQuotation(Entity.Quotation objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnQuotationNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Quotation_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@QuotationNo", objEntity.QuotationNo);
            cmdAdd.Parameters.AddWithValue("@QuotationDate", objEntity.QuotationDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CreditDays", objEntity.CreditDays);
            cmdAdd.Parameters.AddWithValue("@QuotationSubject", objEntity.QuotationSubject);
            cmdAdd.Parameters.AddWithValue("@QuotationKindAttn", objEntity.QuotationKindAttn);
            cmdAdd.Parameters.AddWithValue("@ProjectName", objEntity.ProjectName);
            cmdAdd.Parameters.AddWithValue("@BankId", objEntity.BankId);
            cmdAdd.Parameters.AddWithValue("@QuotationHeader", objEntity.QuotationHeader);
            cmdAdd.Parameters.AddWithValue("@QuotationFooter", objEntity.QuotationFooter);
            cmdAdd.Parameters.AddWithValue("@AssumptionRemark", objEntity.AssumptionRemark);
            cmdAdd.Parameters.AddWithValue("@AdditionalRemark", objEntity.AdditionalRemark);
            //cmdAdd.Parameters.AddWithValue("@QType", objEntity.QuotationType);

            cmdAdd.Parameters.AddWithValue("@BasicAmt", objEntity.BasicAmt);
            cmdAdd.Parameters.AddWithValue("@DiscountAmt",objEntity.DiscountAmt);
            cmdAdd.Parameters.AddWithValue("@SGSTAmt",objEntity.SGSTAmt );
            cmdAdd.Parameters.AddWithValue("@CGSTAmt",objEntity.CGSTAmt);
            cmdAdd.Parameters.AddWithValue("@IGSTAmt",objEntity.IGSTAmt);
            cmdAdd.Parameters.AddWithValue("@ROffAmt",objEntity.ROffAmt);

            cmdAdd.Parameters.AddWithValue("@ChargeID1",objEntity.ChargeID1);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt1",objEntity.ChargeAmt1);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt1", objEntity.ChargeBasicAmt1);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt1",objEntity.ChargeGSTAmt1);

            cmdAdd.Parameters.AddWithValue("@ChargeID2",objEntity.ChargeID2);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt2",objEntity.ChargeAmt2);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt2", objEntity.ChargeBasicAmt2);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt2", objEntity.ChargeGSTAmt2);

            cmdAdd.Parameters.AddWithValue("@ChargeID3",objEntity.ChargeID3);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt3",objEntity.ChargeAmt3);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt3", objEntity.ChargeBasicAmt3);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt3", objEntity.ChargeGSTAmt3);

            cmdAdd.Parameters.AddWithValue("@ChargeID4",objEntity.ChargeID4);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt4",objEntity.ChargeAmt4);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt4", objEntity.ChargeBasicAmt4);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt4", objEntity.ChargeGSTAmt4);

            cmdAdd.Parameters.AddWithValue("@ChargeID5",objEntity.ChargeID5);
            cmdAdd.Parameters.AddWithValue("@ChargeAmt5",objEntity.ChargeAmt5);
            cmdAdd.Parameters.AddWithValue("@ChargeBasicAmt5", objEntity.ChargeBasicAmt5);
            cmdAdd.Parameters.AddWithValue("@ChargeGSTAmt5", objEntity.ChargeGSTAmt5);
            cmdAdd.Parameters.AddWithValue("@NetAmt", objEntity.NetAmt);

            cmdAdd.Parameters.AddWithValue("@CurrencyName", objEntity.CurrencyName);
            cmdAdd.Parameters.AddWithValue("@CurrencySymbol", objEntity.CurrencySymbol);
            cmdAdd.Parameters.AddWithValue("@ExchangeRate", objEntity.ExchangeRate);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnQuotationNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnQuotationNo = cmdAdd.Parameters["@ReturnQuotationNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteQuotation(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Quotation_DEL";
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

        public virtual void AddUpdateQuotationRevision(long pkID, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "QuotationRevision_INS_UPD";
            command.Parameters.AddWithValue("@pkID", (object)pkID);
            command.Parameters.AddWithValue("@LoginUserID", (object)pLoginUserID);
            SqlParameter sqlParameter1 = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter sqlParameter2 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, (int)byte.MaxValue);
            sqlParameter1.Direction = ParameterDirection.Output;
            sqlParameter2.Direction = ParameterDirection.Output;
            command.Parameters.Add(sqlParameter1);
            command.Parameters.Add(sqlParameter2);
            BaseSqlManager.ExecuteNonQuery(command);
            ReturnCode = Convert.ToInt32(command.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = command.Parameters["@ReturnMsg"].Value.ToString();
            BaseSqlManager.ForceCloseConncetion();
        }

        //------------------- Quatation Log --------------------------

        public virtual List<Entity.Quotation> GetQuatationLogList(String HeaderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "QuatationLogList";
            cmdGet.Parameters.AddWithValue("@HeaderID", HeaderID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", "");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Quotation> lstObject = new List<Entity.Quotation>();
            while (dr.Read())
            {
                Entity.Quotation objEntity = new Entity.Quotation();
                objEntity.RowNum = GetInt64(dr, "RowNum");
                objEntity.QuatationID = GetInt64(dr, "QuatationID");
                objEntity.Remark = GetTextVale(dr, "Remark");
                objEntity.FileName = GetTextVale(dr, "FileName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void AddUpdateQuatationLog(Entity.Quotation objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "QuatationLog_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@QuatationID", objEntity.QuatationID);
            cmdAdd.Parameters.AddWithValue("@Remark", objEntity.Remark);
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
        public virtual void AddUpdateQuotationDocuments(Entity.Quotation objEntity, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "Insert INTO QuotationFollowUp_ATTACHMENT (QuatationID, LogID, FileName, CreatedBy, CreatedDate)" + " Values (@QuatationID, @LogID, @FileName, @LoginUserID, GETDATE())";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@QuatationID", SqlDbType.BigInt).Value = objEntity.QuatationID;
                cmdAdd.Parameters.Add("@LogID", SqlDbType.NVarChar).Value = objEntity.LogID;
                cmdAdd.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = objEntity.FileName;
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

        public virtual void DeleteQuotationDocumentsByQuotationNo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "Delete From QuotationFollowUp_ATTACHMENT Where QuatationID = @QuatationID";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@QuatationID", SqlDbType.BigInt).Value = pkID;
                ExecuteNonQuery(cmdAdd);

                ReturnCode = 1;
                ReturnMsg = "File Deleted Successfully !";
            }
            catch (Exception ex)
            {
                ReturnCode = 0;
                ReturnMsg = ex.Message.ToString();
            }
            ForceCloseConncetion();
        }

        public virtual string SendQuotationEmail(string pTemplateID, string pLoginUserID, Int64 pkID, string pEmailAddress)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "EmailTemplateList";
            cmdGet.Parameters.AddWithValue("@TemplateID", pTemplateID);
            cmdGet.Parameters.AddWithValue("@Category", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            // ------------------------------------------------------------------
            SqlCommand cmdGet1 = new SqlCommand();
            cmdGet1.CommandType = CommandType.StoredProcedure;
            cmdGet1.CommandText = "QuotationList";
            cmdGet1.Parameters.AddWithValue("@pkID", pkID);
            cmdGet1.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet1.Parameters.AddWithValue("@PageNo", 1);
            cmdGet1.Parameters.AddWithValue("@PageSize", 10);
            SqlParameter p1 = new SqlParameter("@TotalCount", SqlDbType.Int);
            p1.Direction = ParameterDirection.Output;
            cmdGet1.Parameters.Add(p1);
            SqlDataReader drTable = ExecuteDataReader(cmdGet1);
            // ------------------------------------------------------------------
            while (dr.Read())
            {
                string body = string.Empty;
                body = GetTextVale(dr, "ContentData");
                string pSubject = GetTextVale(dr, "Subject");
                // ===================================================================================================
                // Custom Email Format : Family & Friends 
                // ===================================================================================================
                drTable.Read();
                String pUserID, pEmployeeName, pDesignation, pCompanyName;
                pUserID = GetTextVale(drTable, "CreatedBy");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(GetTextVale(drTable, "EmailAddress")))
                    pEmailAddress = GetTextVale(drTable, "EmailAddress");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(pUserID))
                {
                    pEmployeeName = DAL.CommonSQL.GetEmployeeNameByUserID(pUserID);
                    pDesignation = DAL.CommonSQL.GetDesignationByUserID(pUserID);
                    // --------------------------------------------------------
                    if (!String.IsNullOrEmpty(pEmployeeName))
                        body = body + "<br /><b>" + pEmployeeName + "</b><br />";

                    if (!String.IsNullOrEmpty(pDesignation))
                        body = body + "<b>" + pDesignation + "</b><br />";
                }
                // ------------------------------------------------------------
                pCompanyName = DAL.CommonSQL.GetCompanyName().Trim();
                if (!String.IsNullOrEmpty(pCompanyName))
                    body = body + "<b>" + pCompanyName + "</b><br /><br />";
                // ------------------------------------------------------------
                string filepath1, filepath2;
                LinkedResource Img1 = null;
                LinkedResource Img2 = null;
                AlternateView AV = null;

                if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment1")))
                {
                    filepath1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\" + GetTextVale(dr, "ImageAttachment1"));
                    Img1 = new LinkedResource(filepath1, MediaTypeNames.Image.Jpeg);
                    Img1.ContentId = "MyBrochure1";
                    body = body + "<img src=cid:MyBrochure1  id='img' alt='' width='900px' height='600px'/><br /><br />";
                }

                if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment2")))
                {
                    filepath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\" + GetTextVale(dr, "ImageAttachment2"));
                    Img2 = new LinkedResource(filepath2, MediaTypeNames.Image.Jpeg);
                    Img2.ContentId = "MyBrochure2";
                    body = body + "<img src=cid:MyBrochure2  id='img' alt='' width='900px' height='600px'/><br /><br />";
                }

                AV = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment1")))
                    AV.LinkedResources.Add(Img1);

                if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment2")))
                    AV.LinkedResources.Add(Img2);
                // ---------------------------------------------------------------------------------
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                    mailMessage.Subject = (!String.IsNullOrEmpty(pCompanyName)) ? pCompanyName + " - Inquiry" : "Inquiry";
                    //mailMessage.Body = body;
                    mailMessage.AlternateViews.Add(AV);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(new MailAddress(pEmailAddress));
                    //mailMessage.To.Add(new MailAddress("mrunalyoddha@gmail.com"));
                    // -------------------------------------------------------------
                    String pdfFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\Quotation_" + pkID.ToString() + ".pdf");
                    Attachment data = new Attachment(pdfFile);
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.DateTime.Now;
                    disposition.ModificationDate = System.DateTime.Now;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mailMessage.Attachments.Add(data);   // Attaching the file  
                    // -------------------------------------------------------------
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["Host"];
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
                    NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                    smtp.Send(mailMessage);
                }
            }
            return "Welcome Email Sent Successfully !";
        }

        public virtual List<Entity.Quotation> GetAssemblyBrand(Int64 FinishProductID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pd.ProductID, ass.ProductName, b.BrandName From MST_Product_Detail pd Inner Join MST_Product ass ON ass.pkID = pd.ProductID Inner Join MST_Brand b ON ass.BrandID = b.pkID Where pd.FinishProductID = " + FinishProductID;
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.Quotation> lstLocation = new List<Entity.Quotation>();
            while (dr.Read())
            {
                Entity.Quotation objLocation = new Entity.Quotation();
                objLocation.ProductName = GetTextVale(dr, "ProductName");
                objLocation.BrandName = GetTextVale(dr, "BrandName");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }



    }
}
