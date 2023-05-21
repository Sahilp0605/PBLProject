using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    public class PurchaseBillSQL : BaseSqlManager
    {
        public virtual List<Entity.PurchaseBill> GetPurchaseBillList(String LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseBillList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseBill> lstObject = new List<Entity.PurchaseBill>();
            while (dr.Read())
            {
                Entity.PurchaseBill objEntity = new Entity.PurchaseBill();
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
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");

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

                //---------------------------Bank Details-----------------

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift = GetTextVale(dr, "BankSwift");
                objEntity.ForCoustmerID = GetTextVale(dr, "ForCoustmerID");
                //------------------------------------------------

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.BillNo = GetTextVale(dr, "BillNo");
                //objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                //objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                //objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                //objEntity.LRNo = GetTextVale(dr, "LRNo");
                //objEntity.LRDate = GetDateTime(dr, "LRDate");
                //objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");
                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.PurchaseBill> GetPurchaseBillList(string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseBillListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.PurchaseBill> lstObject = new List<Entity.PurchaseBill>();
            while (dr.Read())
            {
                Entity.PurchaseBill objEntity = new Entity.PurchaseBill();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");
                objEntity.BillAmount = GetDecimal(dr, "BillAmount");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
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
                objEntity.BillNo = GetTextVale(dr, "BillNo");

                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.PurchaseBill> GetPurchaseBillList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseBillList";
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
                Entity.PurchaseBill objEntity = new Entity.PurchaseBill();
                objEntity.pkID = GetInt64(dr, "pkId");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.CustomerID = GetInt64(dr, "CustomerId");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.LocationID = GetInt64(dr, "LocationID");
                objEntity.BankID= GetInt64(dr, "BankID");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");

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
                //---------------------------Bank Details-----------------

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift = GetTextVale(dr, "BankSwift");
                objEntity.ForCoustmerID = GetTextVale(dr, "ForCoustmerID");
                //------------------------------------------------

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");

                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.PurchaseBill> GetPurchaseBillList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PurchaseBillList";
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
                Entity.PurchaseBill objEntity = new Entity.PurchaseBill();
                objEntity.pkID = GetInt64(dr, "pkId");
                objEntity.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                objEntity.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                objEntity.FixedLedgerID = GetInt64(dr, "FixedLedgerID");
                objEntity.FixedLedgerName = GetTextVale(dr, "FixedLedgerName");
                objEntity.DocRefNoList = GetTextVale(dr, "DocRefNoList");
                objEntity.CustomerID = GetInt64(dr, "CustomerId");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.GSTNO = GetTextVale(dr, "GSTNO");
                objEntity.BankID= GetInt64(dr, "BankID");
                objEntity.TerminationOfDeliery = GetInt64(dr, "TerminationOfDeliery");
                objEntity.TermsCondition = GetTextVale(dr, "TermsCondition");
                objEntity.BasicAmt = GetDecimal(dr, "BasicAmt");
                objEntity.DiscountAmt = GetDecimal(dr, "DiscountAmt");
                objEntity.TaxAmt = GetDecimal(dr, "TaxAmt");
                objEntity.ROffAmt = GetDecimal(dr, "ROffAmt");

                objEntity.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objEntity.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objEntity.IGSTAmt = GetDecimal(dr, "IGSTAmt");

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

                //---------------------------Bank Details-----------------

                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSwift = GetTextVale(dr, "BankSwift");
                objEntity.ForCoustmerID = GetTextVale(dr, "ForCoustmerID");
                //------------------------------------------------

                objEntity.NetAmt = GetDecimal(dr, "NetAmt");
                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.ModeOfTransport = GetTextVale(dr, "ModeOfTransport");
                objEntity.TransporterName = GetTextVale(dr, "TransporterName");
                objEntity.VehicleNo = GetTextVale(dr, "VehicleNo");
                objEntity.LRNo = GetTextVale(dr, "LRNo");
                objEntity.LRDate = GetDateTime(dr, "LRDate");
                objEntity.TransportRemark = GetTextVale(dr, "TransportRemark");

                objEntity.CRDays = GetInt64(dr, "CRDays");
                objEntity.DueDate = GetDateTime(dr, "DueDate");

                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdatePurchaseBill(Entity.PurchaseBill objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInvoiceNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseBill_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@InvoiceDate", objEntity.InvoiceDate);
            cmdAdd.Parameters.AddWithValue("@FixedLedgerID", objEntity.FixedLedgerID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@BankID", objEntity.BankID);
            cmdAdd.Parameters.AddWithValue("@TerminationOfDeliery", objEntity.TerminationOfDeliery);
            cmdAdd.Parameters.AddWithValue("@TermsCondition", objEntity.TermsCondition);
            cmdAdd.Parameters.AddWithValue("@BillNo", objEntity.BillNo);
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

            cmdAdd.Parameters.AddWithValue("@ModeOfTransport", objEntity.ModeOfTransport);
            cmdAdd.Parameters.AddWithValue("@TransporterName", objEntity.TransporterName);
            cmdAdd.Parameters.AddWithValue("@VehicleNo", objEntity.VehicleNo);
            cmdAdd.Parameters.AddWithValue("@LRNo", objEntity.LRNo);
            cmdAdd.Parameters.AddWithValue("@ForCoustmerID", objEntity.ForCoustmerID);
            if (objEntity.LRDate.Year >=2000)
                cmdAdd.Parameters.AddWithValue("@LRDate", objEntity.LRDate);
            cmdAdd.Parameters.AddWithValue("@TransportRemark", objEntity.TransportRemark);

            cmdAdd.Parameters.AddWithValue("@CRDays", objEntity.CRDays);
            if (objEntity.DueDate.Year > 2000)
                cmdAdd.Parameters.AddWithValue("@DueDate", objEntity.DueDate);

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

        public virtual void DeletePurchaseBill(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseBill_DEL";
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
        public DataTable GetPurchaseBillDetail(string pInvoiceNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            //myCommand.CommandText = "SELECT it.ProductName, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,qd.Qty as Quantity,qd.Unit,it.Rate as UnitRate, it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount, qd.* From Purchase_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.InvoiceNo = '" + pInvoiceNo + "'";
            myCommand.CommandText = "SELECT it.ProductName, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,qd.Qty as Quantity,qd.Unit ,it.HSNCode,ISNULL(pg.ProductGroupName, '' ) As ProductGroupName, qd.Rate as UnitRate, it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.Amount, qd.* From Purchase_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID LEFT JOin MST_ProductGroup pg on it.ProductGroupID = pg.pkID Where qd.InvoiceNo = '" + pInvoiceNo + "' ORDER BY qd.pkID ASC ";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        //--------------------------------------------------------------------------------
        public DataTable GetPurchaseBillDetailWithDisc(string pInvoiceNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            //myCommand.CommandText = "SELECT it.ProductName, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,qd.Qty as Quantity,qd.Unit,it.Rate as UnitRate, it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount, qd.* From Purchase_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.InvoiceNo = '" + pInvoiceNo + "'";
            myCommand.CommandText = "SELECT it.ProductName, Case When (it.ProductAlias IS NOT NULL And it.ProductAlias<>'' And LTRIM(RTRIM(it.ProductName)) <> LTRIM(RTRIM(it.ProductAlias))) Then '[' + it.ProductAlias + '] - ' + it.ProductName Else it.ProductName End As ProductNameLong,qd.Qty as Quantity,qd.Unit, ISNULL(it.UnitQuantity,1) As UnitQuantity, 1 AS UnitQTY, qd.DiscountPer,qd.DiscountAmt, it.HSNCode, qd.Rate as UnitRate, it.ProductSpecification, (qd.CGSTPer + qd.SGSTPer + qd.IGSTPer) as TaxRate,(qd.CGSTAmt + qd.SGSTAmt + qd.IGSTAmt) as TaxAmount,qd.Amount, qd.* From Purchase_Detail qd Inner Join MST_Product it On qd.ProductID = it.pkID Where qd.InvoiceNo = '" + pInvoiceNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }
        public DataTable GetPurchaseBillNoByCustomerID(Int64 CustomerID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InvoiceNo From PurchaseBill qd Where qd.CustomerID = " + CustomerID.ToString();
            //myCommand.CommandText = "Select * from ( SELECT qd.customerID, qd.InvoiceNo, (select sum(Amount) from Financial_Trans_Detail where ParentID = ft.pkID) AS PaidAmount, " +
            //                        " ft.VoucherAmount - (select sum(Amount) from Financial_Trans_Detail where ParentID = ft.pkID) AS RemainingAmount   " +
            //                        " From SalesBill qd LEFT JOIN Financial_Trans_Detail ftd on qd.InvoiceNo = ftd.InvoiceNo INNER JOIN Financial_Trans ft on ft.pkID = ftd.ParentID " +
            //                        " Where qd.CustomerID =  " + CustomerID.ToString() + " ) AS Temp WHere RemainingAmount > 0";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        
        public virtual void AddUpdatePurchaseBillDetail(Entity.PurchaseBillDetail objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PurchaseBillDetail_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InvoiceNo", objEntity.InvoiceNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@LocationID", objEntity.LocationID);
            cmdAdd.Parameters.AddWithValue("@TaxType", objEntity.TaxType);
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

        public virtual void DeletePurchaseBillDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseBillDetail_DEL";
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

        public virtual void DeletePurchaseBillDetailByInvoiceNo(string pInvoiceNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PurchaseBillDetailByInvoiceNo_DEL";
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

        //--------------------------------------------------------------------------------
        public DataTable GetPurchasePendingBillsByCustomerID(Int64 CustomerID)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InvoiceNo From PurchaseBill qd Where qd.CustomerID = " + CustomerID.ToString() + " And dbo.fnCheckPendingBillStatus('purchase',qd.InvoiceNo, qd.CustomerID)=1";
            //myCommand.CommandText = "Select * from ( SELECT qd.customerID, qd.InvoiceNo, (select sum(Amount) from Financial_Trans_Detail where ParentID = ft.pkID) AS PaidAmount, " +
            //                        " ft.VoucherAmount - (select sum(Amount) from Financial_Trans_Detail where ParentID = ft.pkID) AS RemainingAmount   " +
            //                        " From PurchaseBill qd LEFT JOIN Financial_Trans_Detail ftd on qd.InvoiceNo = ftd.InvoiceNo INNER JOIN Financial_Trans ft on ft.pkID = ftd.ParentID " +
            //                        " Where qd.CustomerID =  " + CustomerID.ToString() + " ) AS Temp WHere RemainingAmount > 0";

            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public Decimal GetPurchasePendingBillsAmount(String InvoiceNo)
        {
            Decimal varResult = 0;
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT dbo.fnCheckPendingBillAmount('purchase', '" + InvoiceNo.Trim() + "')";
            varResult = Convert.ToDecimal(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }
    }
}
