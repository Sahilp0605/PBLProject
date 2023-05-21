using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mime;
using System.Web.Script.Serialization;
using RestSharp;
using System.Data.Common;
//using Limilabs.Mail;
//using Limilabs.Client.IMAP;

namespace DAL
{
    public class CommonSQL : BaseSqlManager
    {
        public virtual String GetQuotationType(Int64 pkID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select Convert(nvarchar,ISNULL(QType,'')) AS QType from Quotation Where pkID = " + @pkID;
            string result = ExecuteScalar(cmdGet).ToString();
            ForceCloseConncetion();
            return result;
        }
        public virtual List<Entity.Complaint> GetCustomerListForVisit(Int64 pkID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pkID, NameOfCustomer from Complaint Where pkID = " + @pkID;
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.Complaint> GetSRNO(Int64 ComplaintNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pkID,PanelSRNo,ProductSRNo from Complaint Where pkID = " +  ComplaintNo ;
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.Complaint> GetComplaintList(Int64 ComplaintNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pkID, SrNo, ProductID, dbo.fnGetProductName(ProductID) as 'ProductName', ComplaintNotes, NameOfCustomer, CustmoreMobileNo from Complaint Where pkID = " + ComplaintNo;
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.SrNo = GetTextVale(dr, "SrNo");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");
                objEntity.CustmoreMobileNo = GetTextVale(dr, "CustmoreMobileNo");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Customer> ReGenerateTrialBalance()
        {
            
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ReGenerateLedger";
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.OpeningAmount = GetDecimal(dr, "Opening");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                objEntity.ClosingAmount = GetDecimal(dr, "Closing");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.Products> ReGenerateStock(Boolean WantList)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ReGenerateStock";
            cmdGet.Parameters.AddWithValue("@WantList", WantList);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Products> lstLocation = new List<Entity.Products>();
            while (dr.Read())
            {
                Entity.Products objEntity = new Entity.Products();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ProductGroupID = GetInt64(dr, "ProductGroupID");
                objEntity.ProductGroupName = GetTextVale(dr, "ProductGroupName");
                objEntity.BrandID = GetInt64(dr, "BrandID");
                objEntity.BrandName = GetTextVale(dr, "BrandName");
                objEntity.OpeningSTK = GetDecimal(dr, "OpeningSTK");
                objEntity.InwardSTK = GetDecimal(dr, "InwardSTK");
                objEntity.OutwardSTK = GetDecimal(dr, "OutwardSTK");
                objEntity.ClosingSTK = GetDecimal(dr, "ClosingSTK");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CRMSummary> GetDashboardCRMSummary(string Type, Int64 pMonth, Int64 pYear, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DashboardCRMSummary";
            cmdGet.Parameters.AddWithValue("@Type", Type);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CRMSummary> lstLocation = new List<Entity.CRMSummary>();
            while (dr.Read())
            {
                Entity.CRMSummary objLocation = new Entity.CRMSummary();
                if (Type == "")
                {
                    objLocation.TotalLeads = GetInt64(dr, "TotalLeads");
                    objLocation.TotalQuoatation = GetInt64(dr, "TotalQuoatation");
                    objLocation.TotalQuoatationAmt = GetInt64(dr, "TotalQuoatationAmt");
                    objLocation.TotalSalesOrder = GetInt64(dr, "TotalSalesOrder");
                    objLocation.TotalSalesOrderAmt = GetInt64(dr, "TotalSalesOrderAmt");
                    objLocation.TotalSalesBill = GetInt64(dr, "TotalSalesBill");
                    objLocation.TotalSalesBillAmt = GetInt64(dr, "TotalSalesBillAmt");
                }
                else
                {
                    objLocation.DocNo = GetTextVale(dr, "DocNo");
                    objLocation.DocDate = GetDateTime(dr, "DocDate");
                    objLocation.CustomerID = GetInt64(dr, "CustomerID");
                    objLocation.CustomerName = GetTextVale(dr, "CustomerName");
                    objLocation.BasicAmt = GetDecimal(dr, "BasicAmt");
                    objLocation.TaxAmount = GetDecimal(dr, "TaxAmount");
                    objLocation.NetAmount = GetDecimal(dr, "NetAmount");
                    objLocation.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");
                }
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CRMSummary> GetDashboardDailySummary(string Type, Int64 pMonth, Int64 pYear, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DashboardDailySummary";
            cmdGet.Parameters.AddWithValue("@Type", Type);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CRMSummary> lstLocation = new List<Entity.CRMSummary>();
            while (dr.Read())
            {
                Entity.CRMSummary objLocation = new Entity.CRMSummary();
                if (Type == "")
                {
                    objLocation.DueDispatch = GetInt64(dr, "DueDispatch");
                    objLocation.DuePurchase = GetInt64(dr, "DuePurchase");
                    objLocation.DuePayable = GetInt64(dr, "DuePayable");
                    objLocation.DueReceivable = GetInt64(dr, "DueReceivable");
                    objLocation.DuePurchasePaySch = GetInt64(dr, "DuePurchasePaySch");
                    objLocation.DueSalesPaySch = GetInt64(dr, "DueSalesPaySch");

                    objLocation.AppSalesOrder = GetInt64(dr, "AppSalesOrder");
                    objLocation.AppPurchaseOrder = GetInt64(dr, "AppPurchaseOrder");

                    objLocation.DocInquiry = GetInt64(dr, "DocInquiry");
                    objLocation.DocQuotation = GetInt64(dr, "DocQuotation");
                    objLocation.DocSalesOrder = GetInt64(dr, "DocSalesOrder");
                }
                else
                {
                    objLocation.DocNo = GetTextVale(dr, "DocNo");
                    objLocation.DocDate = GetDateTime(dr, "DocDate");
                    objLocation.CustomerID = GetInt64(dr, "CustomerID");
                    objLocation.CustomerName = GetTextVale(dr, "CustomerName");
                    objLocation.NetAmount = GetDecimal(dr, "NetAmount");
                    objLocation.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");
                }
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public static string GetDocRefNoList(string pModule, string pKeyValue)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].GetDocRefNoList('" + pModule + "','" + pKeyValue + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetHREmailAddress()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select Concat(lower(EmailAddress),',',lower(EmailPassword)) From viewCompanyUsers Where Lower(RoleCode)='hradmin'";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public virtual Boolean AllowDeleteModuleEntry(string Module, string KeyValue)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnAllowDeleteModuleEntry('" + Module + "','" + KeyValue + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return Convert.ToBoolean(varResult);
        }
        public virtual Boolean AllowDeleteModuleEntry(string Module, string KeyValue, Int64 ProductID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnAllowDeleteModuleEntry('" + Module + "','" + KeyValue + "'," + ProductID.ToString() + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return Convert.ToBoolean(varResult);
        }
        public static void DeleteFileFromFolder(string rootFolderName, string FileToDelete)
        {
            if (!String.IsNullOrEmpty(rootFolderName) && !String.IsNullOrEmpty(FileToDelete))
            {
                string rootFolderPath = HttpContext.Current.Server.MapPath(rootFolderName);
                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, FileToDelete);
                foreach (string file in fileList)
                {
                    System.IO.File.Delete(file);
                }
            }
        }
        public static string GetEmployeeEmailByEmployeeID(Int64 pEmpID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT EmailAddress From OrganizationEmployee Where pkID = " + pEmpID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public virtual List<Entity.GSTR> PendingGSTNO(string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PendingGSTNO";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.GSTR> lstLocation = new List<Entity.GSTR>();
            while (dr.Read())
            {
                Entity.GSTR objLocation = new Entity.GSTR();
                objLocation.Description = GetTextVale(dr, "Module");
                objLocation.CustomerName = GetTextVale(dr, "CustomerName");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.GSTR> GSTRSummary(string ReportType, Int64 pMonth, Int64 pYear, Int64 CustomerID, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GSTRSummary";
            cmdGet.Parameters.AddWithValue("@ReportType", ReportType);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.GSTR> lstLocation = new List<Entity.GSTR>();
            while (dr.Read())
            {
                Entity.GSTR objLocation = new Entity.GSTR();
                if (ReportType == "GSTR1" || ReportType == "GSTR2")
                {
                    objLocation.Module = GetTextVale(dr, "Module");
                    objLocation.Description = GetTextVale(dr, "Description");
                    objLocation.NOE = GetInt64(dr, "NOE");
                    objLocation.BasicAmount = GetDecimal(dr, "BasicAmount");
                    objLocation.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objLocation.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objLocation.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objLocation.TaxAmt = (GetDecimal(dr, "IGSTAmt") + GetDecimal(dr, "CGSTAmt") + GetDecimal(dr, "SGSTAmt"));
                    objLocation.InvoiceAmt = (GetDecimal(dr, "BasicAmount") + GetDecimal(dr, "IGSTAmt") + GetDecimal(dr, "CGSTAmt") + GetDecimal(dr, "SGSTAmt"));
                }
                else
                {
                    objLocation.Module = GetTextVale(dr, "Module");
                    objLocation.Description = GetTextVale(dr, "Description");
                    objLocation.InvoiceNo = GetTextVale(dr, "InvoiceNo");
                    objLocation.InvoiceDate = GetDateTime(dr, "InvoiceDate");
                    objLocation.BasicAmount = GetDecimal(dr, "BasicAmount");
                    objLocation.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objLocation.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objLocation.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objLocation.TaxAmt = (GetDecimal(dr, "IGSTAmt") + GetDecimal(dr, "CGSTAmt") + GetDecimal(dr, "SGSTAmt"));
                    objLocation.InvoiceAmt = (GetDecimal(dr, "BasicAmount") + GetDecimal(dr, "IGSTAmt") + GetDecimal(dr, "CGSTAmt") + GetDecimal(dr, "SGSTAmt"));
                }
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.PurchaseBill> GetLocationList()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select pkID AS LocationID,LocationName From MST_Location;";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            while (dr.Read())
            {
                Entity.PurchaseBill objLocation = new Entity.PurchaseBill();
                objLocation.LocationID = GetInt64(dr, "LocationID");
                objLocation.LocationName = GetTextVale(dr, "LocationName");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.PurchaseBill> GetLocationList_DistinctEmployeeCity()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select distinct ct.cityCode AS LocationID, ct.CityName AS LocationName From OrganizationEmployee OE INNER JOIN MST_City ct on OE.CityCode = ct.CityCode;";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            while (dr.Read())
            {
                Entity.PurchaseBill objLocation = new Entity.PurchaseBill();
                objLocation.LocationID = GetInt64(dr, "LocationID");
                objLocation.LocationName = GetTextVale(dr, "LocationName");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public static string GetFinishProductNameForSO(Int64 pProductID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetFinishProductNameForSO(" + pProductID + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetStateCode(Int64 StateCode)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select ISNULL(GSTStateCode,0) from MST_State Where StateCode = " + StateCode;
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetRefererenceNoFromBill(String InvoiceNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetReferenceNo('" + InvoiceNo + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }


        public virtual void UpdateCourierImage(string pKeyField, string pFileName)
        {
            try
            {
                string query = "Update CourierInfo Set CourierImage = '" + pFileName + "' Where SerialNo = '" + pKeyField + "'";
                SqlCommand cmdAdd = new SqlCommand(query);
                ExecuteNonQuery(cmdAdd);
            }
            catch (Exception ex) { }
            ForceCloseConncetion();
        }

        public DataTable GetRILPrice()
        {
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select CONVERT(varchar, CreatedDate, 6) As Date, RILPrice from RILPrice_Log";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            dr.Close();
            ForceCloseConncetion();
            return dt;
        }

        public virtual void UpdateRILPrice(Decimal RILPrice)
        {
            try
            {
                string query = "Update MST_Constant set ConstantValue = '" + RILPrice + "' Where ConstantHead = 'RILPrice'";
                string query1 = "UPDATE mcp SET mcp.RatePerBag = ((mcp.ConversionRate + '" + RILPrice + "') * mp.Box_Weight) / 1000 FROM MST_Customer_Products mcp INNER JOIN MST_Product mp ON mcp.ProductID=mp.pkID";
                SqlCommand cmdAdd = new SqlCommand(query);
                SqlCommand cmdAdd1 = new SqlCommand(query1);
                ExecuteNonQuery(cmdAdd);
                ExecuteNonQuery(cmdAdd1);
            }
            catch (Exception ex) { }
            ForceCloseConncetion();
        }

        public virtual void AddRILPrice(Decimal RILPrice)
        {
            try
            {
                string query = "insert into RILPrice_Log (RILPrice, CreatedBy,CreatedDate)" + " values (@RILPrice, @CreatedBy,@CreatedDate)";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@RILPrice", SqlDbType.BigInt).Value = RILPrice;
                cmdAdd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = HttpContext.Current.Session["LoginUserID"];
                cmdAdd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;

                ExecuteNonQuery(cmdAdd);

                //ReturnCode = 1;, out int ReturnCode, out string ReturnMsg
                //ReturnMsg = "RIL Price Uploaded Successfully !";
            }
            catch (Exception ex)
            {
                //ReturnCode = 0;
                //ReturnMsg = ex.Message.ToString();
            }
            ForceCloseConncetion();
        }

        public virtual void GetProductPriceListRate(Int64 pCustID, Int64 pProdID, out Decimal retUnitPrice, out Decimal retDiscount)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GetProductPriceListData";
            cmdAdd.Parameters.AddWithValue("@CustomerID", pCustID);
            cmdAdd.Parameters.AddWithValue("@ProductID", pProdID);
            SqlParameter p1 = new SqlParameter("@ReturnUnitPrice", SqlDbType.Decimal) { Precision = 12, Scale = 2 };
            SqlParameter p2 = new SqlParameter("@ReturnDiscountPer", SqlDbType.Decimal) { Precision = 12, Scale = 2 };
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            retUnitPrice = Convert.ToDecimal(cmdAdd.Parameters["@ReturnUnitPrice"].Value.ToString());
            retDiscount = Convert.ToDecimal(cmdAdd.Parameters["@ReturnDiscountPer"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual List<Entity.ApplicationMenu> GetMenuIconList(String LoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ApplicationIconList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteReader(cmdGet);
            List<Entity.ApplicationMenu> lstLocation = new List<Entity.ApplicationMenu>();
            while (dr.Read())
            {
                Entity.ApplicationMenu objLocation = new Entity.ApplicationMenu();
                objLocation.MenuName = GetTextVale(dr, "MenuName");
                objLocation.MenuText = GetTextVale(dr, "MenuText");
                objLocation.MenuOrder = GetInt64(dr, "MenuOrder");
                objLocation.MenuURL = GetTextVale(dr, "MenuURL");
                objLocation.MenuImage = GetTextVale(dr, "MenuImage");
                objLocation.MenuImageHeight = GetInt64(dr, "MenuImageHeight");
                objLocation.MenuImageWidth = GetInt64(dr, "MenuImageWidth");
                objLocation.MenuLevel = GetInt64(dr, "SectionLevel");
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ApplicationMenu> GetMenuAddEditDelList(Int64 MenuID, String LoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MenuAddEditDelList";
            cmdGet.Parameters.AddWithValue("@MenuID", MenuID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteReader(cmdGet);
            List<Entity.ApplicationMenu> lstLocation = new List<Entity.ApplicationMenu>();
            while (dr.Read())
            {
                Entity.ApplicationMenu objLocation = new Entity.ApplicationMenu();
                objLocation.pkID = GetInt64(dr, "MenuID");
                objLocation.MenuName = GetTextVale(dr, "MenuName");
                objLocation.MenuText = GetTextVale(dr, "MenuText");
                objLocation.MenuURL = GetTextVale(dr, "MenuURL");
                objLocation.AddFlag = GetBoolean(dr, "AddFlag");
                objLocation.EditFlag = GetBoolean(dr, "EditFlag");
                objLocation.DelFlag = GetBoolean(dr, "DelFlag");
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.ApplicationMenu> GetMenuGeneralAddEditDelList(string MenuID, String LoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MenuGeneralAddEditDelList";
            cmdGet.Parameters.AddWithValue("@MenuID", MenuID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteReader(cmdGet);
            List<Entity.ApplicationMenu> lstLocation = new List<Entity.ApplicationMenu>();
            while (dr.Read())
            {
                Entity.ApplicationMenu objLocation = new Entity.ApplicationMenu();
                objLocation.GenpkID = GetTextVale(dr, "MenuID");
                objLocation.AddFlag = GetBoolean(dr, "AddFlag");
                objLocation.EditFlag = GetBoolean(dr, "EditFlag");
                objLocation.DelFlag = GetBoolean(dr, "DelFlag");
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.ApplicationMenu> GetGeneralMenuList(String LoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ApplicationGeneralMenuList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteReader(cmdGet);
            List<Entity.ApplicationMenu> lstLocation = new List<Entity.ApplicationMenu>();
            while (dr.Read())
            {
                Entity.ApplicationMenu objLocation = new Entity.ApplicationMenu();
                objLocation.MenuName = GetTextVale(dr, "MenuName");
                objLocation.MenuText = GetTextVale(dr, "MenuText");
                objLocation.MenuOrder = GetInt64(dr, "MenuOrder");
                objLocation.MenuURL = GetTextVale(dr, "MenuURL");
                objLocation.MenuImage = GetTextVale(dr, "MenuImage");
                objLocation.MenuImageHeight = GetInt64(dr, "MenuImageHeight");
                objLocation.MenuImageWidth = GetInt64(dr, "MenuImageWidth");
                objLocation.MenuLevel = GetInt64(dr, "SectionLevel");
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateUserAction(Entity.ApplicationMenu objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "UserAction_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@MenuID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@UserID", objEntity.UserID);
            cmdAdd.Parameters.AddWithValue("@AddFlag", objEntity.AddFlag);
            cmdAdd.Parameters.AddWithValue("@EditFlag", objEntity.EditFlag);
            cmdAdd.Parameters.AddWithValue("@DelFlag", objEntity.DelFlag);
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

        public static string GetDrivingAllowance(Int64 pEmpID, Int64 pMon, Int64 pYear)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetTripAmtByEmp(" + pEmpID + "," + pMon + "," + pYear + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetDrivingKilometers(Int64 pEmpID, Int64 pMon, Int64 pYear)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetTripKMSByEmp(" + pEmpID + "," + pMon + "," + pYear + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static DateTime GetLatestAttendenceDt()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select TOP 1 CONVERT(DATETIME,CONVERT(NVARCHAR(20), CONVERT(DATE, PresenceDate)) + 'T00:00:00.000') From DailyAttendance Where TIMEIN IS NOT NULL AND TIMEOUT IS NOT NULL ORDER BY PresenceDate DESC";
            DateTime varResult = Convert.ToDateTime(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }

        public virtual List<Entity.ReportMenu> GetReportsList()
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            // --------------------------------------
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select mmr.pkID, mmr.ReportName, mmr.ReportText, mmr.parentID, mmr.ActiveStatus, mmr.ReportURL, mmr.ReportOrder, mmr.ReportImage, mmr.ReportImageHeight, mmr.ReportImageWidth From MST_MenuReports mmr Inner Join MST_RoleReport_Rights rtgs On rtgs.MenuId = mmr.pkID Where mmr.ParentID IS NOT NULL AND rtgs.RoleCode = '" + objAuth.RoleCode + "'";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.ReportMenu> lstLocation = new List<Entity.ReportMenu>();
            while (dr.Read())
            {
                Entity.ReportMenu objLocation = new Entity.ReportMenu();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.ReportName = GetTextVale(dr, "ReportName");
                objLocation.ReportText = GetTextVale(dr, "ReportText");
                objLocation.ParentID = GetInt64(dr, "ParentID");
                objLocation.ReportURL = GetTextVale(dr, "ReportURL");
                objLocation.ReportOrder = GetInt64(dr, "ReportOrder");
                objLocation.ReportImage = GetTextVale(dr, "ReportImage");
                objLocation.ReportImageHeight = GetInt64(dr, "ReportImageHeight");
                objLocation.ReportImageWidth = GetInt64(dr, "ReportImageWidth");
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void GetDailyReport(DateTime startdate, DateTime enddate, string LoginUserID, out DataTable table1, out DataTable table2, out DataTable table3, out DataTable table4, out DataTable dtFollow, out DataTable dtQuotation, out DataTable dtSalesOrder, out DataTable dtSalesBil)
        {
            DataTable t1 = new DataTable();
            t1.Columns.Add("CreatedBy", typeof(String));

            DataTable t2 = new DataTable();
            t2.Columns.Add("Category", typeof(String));
            t2.Columns.Add("Remark", typeof(String));
            t2.Columns.Add("CreatedBy", typeof(String));
            t2.Columns.Add("Count", typeof(int));

            DataTable t3 = new DataTable();
            t3.Columns.Add("ActivityDate", typeof(DateTime));
            t3.Columns.Add("CreatedBy", typeof(String));
            t3.Columns.Add("TaskDescription", typeof(String));
            t3.Columns.Add("TaskDuration", typeof(Decimal));
            t3.Columns.Add("TaskCategory", typeof(String));

            DataTable t4 = new DataTable();
            t4.Columns.Add("Category", typeof(String));
            t4.Columns.Add("CreatedDate", typeof(DateTime));
            t4.Columns.Add("Total", typeof(Decimal));

            DataTable t5 = new DataTable();     // FollowUp
            t5.Columns.Add("FollowUpDate", typeof(DateTime));
            t5.Columns.Add("CustomerName", typeof(String));
            t5.Columns.Add("NextFollowUpDate", typeof(DateTime));
            t5.Columns.Add("MeetingNotes", typeof(String));

            DataTable t6 = new DataTable();     // Quotation
            t6.Columns.Add("QuotationNo", typeof(string));
            t6.Columns.Add("QuotationDate", typeof(DateTime));
            t6.Columns.Add("CustomerName", typeof(String));
            t6.Columns.Add("BasicAmt", typeof(Decimal));
            t6.Columns.Add("NetAmt", typeof(Decimal));

            DataTable t7 = new DataTable();     // Sales Order
            t7.Columns.Add("OrderNo", typeof(string));
            t7.Columns.Add("OrderDate", typeof(DateTime));
            t7.Columns.Add("CustomerName", typeof(String));
            t7.Columns.Add("BasicAmt", typeof(Decimal));
            t7.Columns.Add("NetAmt", typeof(Decimal));

            DataTable t8 = new DataTable();     // Sales Bill
            t8.Columns.Add("InvoiceNo", typeof(string));
            t8.Columns.Add("InvoiceDate", typeof(DateTime));
            t8.Columns.Add("CustomerName", typeof(String));
            t8.Columns.Add("BasicAmt", typeof(Decimal));
            t8.Columns.Add("NetAmt", typeof(Decimal));

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DailyReport";
            cmdGet.Parameters.AddWithValue("@StartDate", startdate);
            cmdGet.Parameters.AddWithValue("@EndDate", enddate);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                DataRow rw = t1.NewRow();
                rw["CreatedBy"] = (dr["CreatedBy"] != null ? (string)dr["CreatedBy"] : " ");
                t1.Rows.Add(rw);
            }
            dr.NextResult();

            while (dr.Read())
            {
                DataRow rw = t2.NewRow();
                rw["Category"] = (dr["Category"] != null ? (string)dr["Category"] : " ");
                rw["Remark"] = (dr["Remark"] != null ? (string)dr["Remark"] : " ");
                rw["CreatedBy"] = (dr["CreatedBy"] != null ? (string)dr["CreatedBy"] : " ");
                rw["Count"] = (int)dr["Count"];

                t2.Rows.Add(rw);
            }
            dr.NextResult();

            while (dr.Read())
            {
                DataRow rw = t3.NewRow();
                rw["CreatedBy"] = (string)dr["CreatedBy"];
                rw["TaskDescription"] = (string)dr["TaskDescription"];
                rw["TaskCategory"] = (string)dr["TaskCategory"];
                rw["TaskDuration"] = (Decimal)dr["TaskDuration"];
                t3.Rows.Add(rw);
            }
            dr.NextResult();

            while (dr.Read())
            {
                DataRow rw = t4.NewRow();
                rw["Category"] = (string)dr["Category"];
                rw["CreatedDate"] = (DateTime)dr["CreatedDate"];
                rw["Total"] = (Decimal)dr["Total"];
                t4.Rows.Add(rw);
            }

            // -------------------------------------------------------
            // Section : FollowUp
            // -------------------------------------------------------
            dr.NextResult();
            while (dr.Read())
            {
                DataRow rw = t5.NewRow();
                rw["FollowUpDate"] = (DateTime)dr["FollowUpDate"];
                rw["CustomerName"] = (String)dr["CustomerName"];
                rw["NextFollowUpDate"] = (DateTime)dr["NextFollowUpDate"];
                rw["MeetingNotes"] = (string)dr["MeetingNotes"];
                t5.Rows.Add(rw);
            }

            // -------------------------------------------------------
            // Section : Quotation
            // -------------------------------------------------------
            dr.NextResult();
            while (dr.Read())
            {
                DataRow rw = t6.NewRow();
                rw["QuotationNo"] = (String)dr["QuotationNo"];
                rw["QuotationDate"] = (DateTime)dr["QuotationDate"];
                rw["CustomerName"] = (String)dr["CustomerName"];
                rw["BasicAmt"] = (Decimal)dr["BasicAmt"];
                rw["NetAmt"] = (Decimal)dr["NetAmt"];
                t6.Rows.Add(rw);
            }

            // -------------------------------------------------------
            // Section : Sales Order
            // -------------------------------------------------------
            dr.NextResult();
            while (dr.Read())
            {
                DataRow rw = t7.NewRow();
                rw["OrderNo"] = (String)dr["OrderNo"];
                rw["OrderDate"] = (DateTime)dr["OrderDate"];
                rw["CustomerName"] = (String)dr["CustomerName"];
                rw["BasicAmt"] = (Decimal)dr["BasicAmt"];
                rw["NetAmt"] = (Decimal)dr["NetAmt"];
                t7.Rows.Add(rw);
            }

            // -------------------------------------------------------
            // Section : Sales Order
            // -------------------------------------------------------
            dr.NextResult();
            while (dr.Read())
            {
                DataRow rw = t8.NewRow();
                rw["InvoiceNo"] = (String)dr["InvoiceNo"];
                rw["InvoiceDate"] = (DateTime)dr["InvoiceDate"];
                rw["CustomerName"] = (String)dr["CustomerName"];
                rw["BasicAmt"] = (Decimal)dr["BasicAmt"];
                rw["NetAmt"] = (Decimal)dr["NetAmt"];
                t8.Rows.Add(rw);
            }
            // --------------------------------------
            table1 = t1;
            table2 = t2;
            table3 = t3;
            table4 = t4;

            dtFollow = t5;
            dtQuotation = t6;
            dtSalesOrder = t7;
            dtSalesBil = t8;
            // --------------------------------------
            dr.Close();
            ForceCloseConncetion();
        }

        public virtual DataTable GetExportDataList(string keyVal)
        {
            DataTable dt = new DataTable();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MY_ExportDataList";
            cmdGet.Parameters.AddWithValue("@module", keyVal);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            dt.Load(dr);
            dr.Close();
            ForceCloseConncetion();
            return dt;
        }

        public virtual DataTable GetExportDataList(string module, string loginuserid, string keyval)
        {
            DataTable dt = new DataTable();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MY_ExportDataList";
            cmdGet.Parameters.AddWithValue("@module", module);
            cmdGet.Parameters.AddWithValue("@LoginUserID", loginuserid);
            cmdGet.Parameters.AddWithValue("@keyvalue", keyval);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            dt.Load(dr);
            dr.Close();
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.Chat> GetChatBoxList(string pFrom, string pTo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ChatBoxList";
            cmdGet.Parameters.AddWithValue("@FromUser", pFrom);
            cmdGet.Parameters.AddWithValue("@ToUser", pTo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Chat> lstLocation = new List<Entity.Chat>();
            while (dr.Read())
            {
                Entity.Chat objLocation = new Entity.Chat();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.FromUser = GetTextVale(dr, "FromUser");
                objLocation.FromUserImage = GetTextVale(dr, "FromUserImage");
                objLocation.ToUser = GetTextVale(dr, "ToUser");
                objLocation.ToUserImage = GetTextVale(dr, "ToUserImage");
                objLocation.Message = GetTextVale(dr, "Message");
                objLocation.Flag = GetTextVale(dr, "Flag");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Chat> GetChatBoxUserList(string pUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ChatBoxUserList";
            cmdGet.Parameters.AddWithValue("@FromUser", pUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Chat> lstLocation = new List<Entity.Chat>();
            while (dr.Read())
            {
                Entity.Chat objLocation = new Entity.Chat();
                objLocation.UserID = GetTextVale(dr, "UserID");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.EmployeeImage = (!String.IsNullOrEmpty(GetTextVale(dr, "EmployeeImage"))) ? GetTextVale(dr, "EmployeeImage") : "images/customer.png";
                objLocation.RoleCode = GetTextVale(dr, "RoleCode");
                objLocation.LastTimestamp = GetDateTime(dr, "LastTimestamp");
                objLocation.UnreadMessageCount = GetInt64(dr, "UnreadMessageCount");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void UpdateChatLastTimestamp(string pFromUser, string pToUser)
        {
            try
            {
                string query = "Update ChatBoxTimestamp Set LastTimestamp = getdate() Where Lower(FromUser) = '" + pFromUser + "' and Lower(ToUser)='" + pToUser + "'";
                SqlCommand cmdAdd = new SqlCommand(query);
                ExecuteNonQuery(cmdAdd);
            }
            catch (Exception ex) { }
            ForceCloseConncetion();
        }

        public static string lastNotificationTimestamp (string LoginUserID)
        {
            try
            {
                string query = "Select NotificationTimestamp  from MST_Users where UserID ='" + LoginUserID + "'";
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandType = CommandType.Text;
                myCommand.CommandText = query;
                string varResult = ExecuteScalar(myCommand).ToString();
                ForceCloseConncetion();
                return varResult;
            }
            catch (Exception ex) {
                return "";
            }
            ForceCloseConncetion();

        }

        public virtual void AddUpdateChatBox(Entity.Chat objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ChatBox_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@FromUser", objEntity.FromUser);
            cmdAdd.Parameters.AddWithValue("@ToUser", objEntity.ToUser);
            cmdAdd.Parameters.AddWithValue("@Message", objEntity.Message);
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

        public virtual List<Entity.QuotationDetail> GetTaxSummaryWidget(string pModule, string pKeyID, Boolean pHSNFlag)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "TaxSummaryWidget";
            cmdGet.Parameters.AddWithValue("@Module", pModule);
            cmdGet.Parameters.AddWithValue("@KeyID", pKeyID);
            cmdGet.Parameters.AddWithValue("@HSNFlag", pHSNFlag);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.QuotationDetail> lstLocation = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objLocation = new Entity.QuotationDetail();
                //objLocation.id = GetTextVale(dr, "OrgCode");
                //objLocation.name = GetTextVale(dr, "OrgName");
                //objLocation.parent = GetTextVale(dr, "ReportTo");
                //objLocation.activeflag = GetTextVale(dr, "ActiveFlag");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.SalesTaxDetail> GetTaxSummaryWidget(string pModule, string pKeyID, Boolean pHSNFlag, string FromDate, string ToDate)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "myTaxSummaryWidgetControl";
            cmdGet.Parameters.AddWithValue("@Module", pModule);
            cmdGet.Parameters.AddWithValue("@KeyID", pKeyID);
            cmdGet.Parameters.AddWithValue("@HSNFlag", pHSNFlag);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.SalesTaxDetail> lstLocation = new List<Entity.SalesTaxDetail>();
            while (dr.Read())
            {
                Entity.SalesTaxDetail objTax = new Entity.SalesTaxDetail();
                objTax.HSNCode = GetTextVale(dr, "HSNCode");
                objTax.TaxPer = GetDecimal(dr, "TaxPer");
                objTax.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                objTax.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                objTax.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                lstLocation.Add(objTax);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.QuotationDetail> GetTaxSummary(string dtTable, string taxCategory, string keyVal)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            // -----------------------------------------------------------
            if (dtTable.ToLower() == "quotation")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From Quotation_Detail Where (CGSTPer+SGSTPer+IGSTPer > 0) And QuotationNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select IGSTPer, sum(IGSTAmt) as IGSTAmt From Quotation_Detail Where (CGSTPer+SGSTPer+IGSTPer > 0) And QuotationNo = '" + keyVal + "' Group By IGSTPer";
            }

            if (dtTable.ToLower() == "salesorder")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From salesorder_Detail Where OrderNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select IGSTPer, sum(IGSTAmt) as IGSTAmt From salesorder_Detail Where OrderNo = '" + keyVal + "' Group By IGSTPer";
            }

            if (dtTable.ToLower() == "salesbill")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From SalesBill_Detail Where InvoiceNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select IGSTPer, sum(IGSTAmt) as IGSTAmt From SalesBill_Detail Where InvoiceNo = '" + keyVal + "' Group By IGSTPer";
            }
            // -----------------------------------------------------------
            if (dtTable.ToLower() == "workordercomm")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From WorkOrderComm_Detail Where OrderNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select IGSTPer, sum(IGSTAmt) as IGSTAmt From WorkOrderComm_Detail Where OrderNo = '" + keyVal + "' Group By IGSTPer";
            }
            // -----------------------------------------------------------
            if (dtTable.ToLower() == "purchaseorder")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From PurchaseOrder_Detail Where OrderNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select IGSTPer, sum(IGSTAmt) as IGSTAmt From PurchaseOrder_Detail Where OrderNo = '" + keyVal + "' Group By IGSTPer";
            }

            if (dtTable.ToLower() == "purchasebill")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From Purchase_Detail Where InvoiceNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select IGSTPer, sum(IGSTAmt) as IGSTAmt From Purchase_Detail Where InvoiceNo = '" + keyVal + "' Group By IGSTPer";
            }
            // -----------------------------------------------------------
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.QuotationDetail> lstLocation = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objLocation = new Entity.QuotationDetail();
                if (taxCategory.ToLower() == "igst")
                {
                    objLocation.CGSTPer = 0;
                    objLocation.CGSTAmt = 0;
                    objLocation.SGSTPer = 0;
                    objLocation.SGSTAmt = 0;
                    objLocation.IGSTPer = GetDecimal(dr, "IGSTPer");
                    objLocation.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                }
                else
                {
                    objLocation.CGSTPer = GetDecimal(dr, "CGSTPer");
                    objLocation.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objLocation.SGSTPer = GetDecimal(dr, "SGSTPer");
                    objLocation.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objLocation.IGSTPer = 0;
                    objLocation.IGSTAmt = 0;
                }
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.QuotationDetail> GetHSNTaxSummary(string dtTable, string taxCategory, string keyVal)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            // -----------------------------------------------------------
            if (dtTable.ToLower() == "quotation")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select mp.HSNCode,CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From Quotation_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where (CGSTPer+SGSTPer+IGSTPer > 0) And QuotationNo = '" + keyVal + "' Group By HSNCode,CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select mp.HSNCode,IGSTPer, sum(IGSTAmt) as IGSTAmt From Quotation_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where (CGSTPer+SGSTPer+IGSTPer > 0) And QuotationNo = '" + keyVal + "' Group By HSNCode,IGSTPer";
            }

            if (dtTable.ToLower() == "salesorder")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select  mp.HSNCode,CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From salesorder_Detail  pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where OrderNo = '" + keyVal + "' Group By HSNCode,CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select  mp.HSNCode,IGSTPer, sum(IGSTAmt) as IGSTAmt From salesorder_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where OrderNo = '" + keyVal + "' Group By HSNCode,IGSTPer";
            }

            if (dtTable.ToLower() == "salesbill")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select  mp.HSNCode,CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From SalesBill_Detail  pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where InvoiceNo = '" + keyVal + "' Group By  HSNCode,CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select  mp.HSNCode,IGSTPer, sum(IGSTAmt) as IGSTAmt From SalesBill_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID  Where InvoiceNo = '" + keyVal + "' Group By  HSNCode,IGSTPer";
            }
            // -----------------------------------------------------------
            if (dtTable.ToLower() == "workordercomm")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select mp.HSNCode,CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From WorkOrderComm_Detail  pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where OrderNo = '" + keyVal + "' Group By  HSNCode,CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select mp.HSNCode,IGSTPer, sum(IGSTAmt) as IGSTAmt From WorkOrderComm_Detail  pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where OrderNo = '" + keyVal + "' Group By  HSNCode,IGSTPer";
            }
            // -----------------------------------------------------------
            if (dtTable.ToLower() == "purchaseorder")
            {
                if (taxCategory.ToLower() != "igst")
                    //myCommand.CommandText = "Select CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From PurchaseOrder_Detail Where OrderNo = '" + keyVal + "' Group By CGSTPer, SGSTPer";
                    myCommand.CommandText = "Select mp.HSNCode,CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From PurchaseOrder_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where OrderNo = '" + keyVal + "' Group By HSNCode,CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select mp.HSNCode,IGSTPer, sum(IGSTAmt) as IGSTAmt From PurchaseOrder_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where OrderNo = '" + keyVal + "' Group By HSNCode,IGSTPer";
            }

            if (dtTable.ToLower() == "purchasebill")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select  mp.HSNCode,CGSTPer, SGSTPer, sum(CGSTAmt) as CGSTAmt, sum(SGSTAmt) as SGSTAmt From Purchase_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID Where InvoiceNo = '" + keyVal + "' Group By  HSNCode,CGSTPer, SGSTPer";
                else
                    myCommand.CommandText = "Select  mp.HSNCode,IGSTPer, sum(IGSTAmt) as IGSTAmt From Purchase_Detail pd Inner Join MST_Product mp on pd.ProductID = mp.pkID  Where InvoiceNo = '" + keyVal + "' Group By  HSNCode,IGSTPer";
            }
            // -----------------------------------------------------------
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.QuotationDetail> lstLocation = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objLocation = new Entity.QuotationDetail();
                if (taxCategory.ToLower() == "igst")
                {
                    objLocation.HSNCode = GetTextVale(dr, "HSNCode");
                    objLocation.CGSTPer = 0;
                    objLocation.CGSTAmt = 0;
                    objLocation.SGSTPer = 0;
                    objLocation.SGSTAmt = 0;
                    objLocation.IGSTPer = GetDecimal(dr, "IGSTPer");
                    objLocation.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                }
                else
                {
                    objLocation.HSNCode = GetTextVale(dr, "HSNCode");
                    objLocation.CGSTPer = GetDecimal(dr, "CGSTPer");
                    objLocation.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objLocation.SGSTPer = GetDecimal(dr, "SGSTPer");
                    objLocation.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objLocation.IGSTPer = 0;
                    objLocation.IGSTAmt = 0;
                }
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.QuotationDetail> GetHSNTaxWithNumberSummaryNew(string dtTable, string taxCategory, string keyVal)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            // -----------------------------------------------------------
          


            if (dtTable.ToLower() == "salesbill")
            {
                if (taxCategory.ToLower() != "igst")
                    myCommand.CommandText = "Select Sum(Qty) as Quantity,Sum(Amount) as Amount, sum(CGSTAmt) AS CGSTAmt, sum(SGSTAmt) AS SGSTAmt, sum(IGSTAmt) AS IGSTAmt,CGSTPer,SGSTPer,IGSTPer ,HSNCode from SalesBill_Detail sd Inner Join MST_Product mp on mp.pkiD = sd.ProductID Where InvoiceNo = '" + keyVal + "'Group By HSNCode,CGSTPer,SGSTPer,IGSTPer";
                else
                    myCommand.CommandText = "Select Sum(Qty) as Quantity,Sum(Amount) as Amount, sum(CGSTAmt) AS CGSTAmt, sum(SGSTAmt) AS SGSTAmt, sum(IGSTAmt) AS IGSTAmt,CGSTPer,SGSTPer,IGSTPer ,HSNCode from SalesBill_Detail sd Inner Join MST_Product mp on mp.pkiD = sd.ProductID Where InvoiceNo = '" + keyVal + "'Group By HSNCode,CGSTPer,SGSTPer,IGSTPer";
            }
            
            // -----------------------------------------------------------
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.QuotationDetail> lstLocation = new List<Entity.QuotationDetail>();
            while (dr.Read())
            {
                Entity.QuotationDetail objLocation = new Entity.QuotationDetail();
                if (taxCategory.ToLower() == "igst")
                {
                    objLocation.HSNCode = GetTextVale(dr, "HSNCode");
                    objLocation.CGSTPer = GetDecimal(dr, "CGSTPer");
                    objLocation.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objLocation.SGSTPer = GetDecimal(dr, "SGSTPer");
                    objLocation.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objLocation.IGSTPer = GetDecimal(dr, "IGSTPer");
                    objLocation.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objLocation.Quantity = GetDecimal(dr, "Quantity");
                    objLocation.Amount = GetDecimal(dr, "Amount");
                } 
                else
                {
                    objLocation.HSNCode = GetTextVale(dr, "HSNCode");
                    objLocation.CGSTPer = GetDecimal(dr, "CGSTPer");
                    objLocation.CGSTAmt = GetDecimal(dr, "CGSTAmt");
                    objLocation.SGSTPer = GetDecimal(dr, "SGSTPer");
                    objLocation.SGSTAmt = GetDecimal(dr, "SGSTAmt");
                    objLocation.IGSTPer = GetDecimal(dr, "IGSTPer");
                    objLocation.IGSTAmt = GetDecimal(dr, "IGSTAmt");
                    objLocation.Quantity = GetDecimal(dr, "Quantity");
                    objLocation.Amount = GetDecimal(dr, "Amount");
                }
                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }


        public virtual List<Entity.Currency> GetCurrencyList()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID, CurrencyName, (CurrencyShortName + ' ' + CurrencySymbol) As CurrencyShortName, CurrencySymbol, ActiveFlag from MST_Currency;";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.Currency> lstLocation = new List<Entity.Currency>();
            while (dr.Read())
            {
                Entity.Currency objLocation = new Entity.Currency();
                objLocation.CurrencyName = GetTextVale(dr, "CurrencyName");
                objLocation.CurrencyShortName = GetTextVale(dr, "CurrencyShortName");
                objLocation.CurrencySymbol = GetTextVale(dr, "CurrencySymbol");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.OrgChart> GetOrgChartList(string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetOrganizationChart";
            // Below Parameter should have ... "GROUP", "DEPT", "MEMBER", "DEPTEMP"
            //cmdGet.Parameters.AddWithValue("@OrgChartType", pOrgChartType);
            //cmdGet.Parameters.AddWithValue("@OrgDeptCode", pOrgDepartmentCode);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrgChart> lstLocation = new List<Entity.OrgChart>();
            while (dr.Read())
            {
                Entity.OrgChart objLocation = new Entity.OrgChart();
                objLocation.id = GetTextVale(dr, "OrgCode");
                objLocation.name = GetTextVale(dr, "OrgName");
                objLocation.parent = GetTextVale(dr, "ReportTo");
                objLocation.activeflag = GetTextVale(dr, "ActiveFlag");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CalenderEvent> GetCalenderList(Int64 pMonth, Int64 pYear, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CalenderList";
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CalenderEvent> lstLocation = new List<Entity.CalenderEvent>();
            while (dr.Read())
            {
                Entity.CalenderEvent objLocation = new Entity.CalenderEvent();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.Title = GetTextVale(dr, "Title");
                objLocation.StartDate = GetDateTime(dr, "StartDate");
                objLocation.EndDate = GetDateTime(dr, "EndDate");
                objLocation.Status = GetTextVale(dr, "Status");
                objLocation.className = GetTextVale(dr, "Category");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.imageurl = GetTextVale(dr, "ImageURL");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.CalenderEvent> GetCalenderListByEmployee(Int64 pMonth, Int64 pYear, Int64 pEmployeeID, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CalenderList";
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@EmployeeID", pEmployeeID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CalenderEvent> lstLocation = new List<Entity.CalenderEvent>();
            while (dr.Read())
            {
                Entity.CalenderEvent objLocation = new Entity.CalenderEvent();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.Title = GetTextVale(dr, "Title");
                objLocation.StartDate = GetDateTime(dr, "StartDate");
                objLocation.EndDate = GetDateTime(dr, "EndDate");
                objLocation.Status = GetTextVale(dr, "Status");
                objLocation.className = GetTextVale(dr, "Category");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.imageurl = GetTextVale(dr, "ImageURL");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.DashboardNotification> GetNotificationList(string LoginUserID, string ListBy)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "Mst_NotificationList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@ListBy", ListBy);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.DashboardNotification> lstLocation = new List<Entity.DashboardNotification>();
            while (dr.Read())
            {
                Entity.DashboardNotification objLocation = new Entity.DashboardNotification();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.ModuleName = GetTextVale(dr, "ModuleName");
                objLocation.ModulePkID = GetInt64(dr, "ModulePkID");
                objLocation.Description = GetTextVale(dr, "Description");
                objLocation.CreatedDate = GetTextVale(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void UpdateUserTimeStamp(string pLoginUserId, string pCompanyID)
        {
            try
            {
                string query = "Update MST_Users Set NotificationTimestamp = getdate() Where userid ='" + pLoginUserId + "'";
                SqlCommand cmdAdd = new SqlCommand(query);

                ExecuteNonQuery(cmdAdd);
            }
            catch (Exception ex)
            {

            }
            ForceCloseConncetion();
        }

        public static string GetBroadcastMessage(string pLoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetBroadcastMessage('" + pLoginUserID + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult.TrimStart(',');
        }
        public static Int64 GetNoOfUsers()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT count(UserID) as TotalUsers from [dbo].MST_Users where ActiveFlag='1'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetServerTimestamp()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT dbo.fnGetServerTimestamp()";
            String varResult = Convert.ToString(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetPageHiddenControls(string pPageName)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT dbo.fnGetPageHiddenControls('" + pPageName + "')";
            String varResult = Convert.ToString(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }

        public virtual void AddUpdateEmailNotification(Entity.EmailNotifications objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "EmailNotification_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@ModuleName", objEntity.ModuleName);
            cmdAdd.Parameters.AddWithValue("@OwnerID", objEntity.OwnerID);
            cmdAdd.Parameters.AddWithValue("@OwnerType", objEntity.OwnerType);
            cmdAdd.Parameters.AddWithValue("@TemplateID", objEntity.TemplateID);
            cmdAdd.Parameters.AddWithValue("@NotificationSent", objEntity.NotificationSent);
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
        public virtual void AddUpdateCompanyProfile(Entity.CompanyProfile objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "CompanyProfile_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CompanyID", objEntity.CompanyID);
            cmdAdd.Parameters.AddWithValue("@BankID", objEntity.BankID);
            cmdAdd.Parameters.AddWithValue("@CompanyName", objEntity.CompanyName);
            cmdAdd.Parameters.AddWithValue("@GSTNo", objEntity.GSTNo);
            cmdAdd.Parameters.AddWithValue("@PANNo", objEntity.PANNo);
            cmdAdd.Parameters.AddWithValue("@CINNo", objEntity.CINNo);
            cmdAdd.Parameters.AddWithValue("@ParentCompanyID", objEntity.ParentCompanyID);
            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@Area", objEntity.Area);
            cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@chkCustomer", objEntity.chkCustomer);
            cmdAdd.Parameters.AddWithValue("@chkInquiry", objEntity.chkInquiry);
            cmdAdd.Parameters.AddWithValue("@chkQuotation", objEntity.chkQuotation);
            cmdAdd.Parameters.AddWithValue("@chkSalesOrder", objEntity.chkSalesOrder);
            cmdAdd.Parameters.AddWithValue("@chkLeaveRequest", objEntity.chkLeaveRequest);
            cmdAdd.Parameters.AddWithValue("@chkFeedback", objEntity.chkFeedback);
            cmdAdd.Parameters.AddWithValue("@Host", objEntity.Host);
            cmdAdd.Parameters.AddWithValue("@EnableSSL", objEntity.EnableSSL);
            cmdAdd.Parameters.AddWithValue("@UserName", objEntity.UserName);
            cmdAdd.Parameters.AddWithValue("@Password", objEntity.Password);
            cmdAdd.Parameters.AddWithValue("@PortNumber", objEntity.PortNumber);
            cmdAdd.Parameters.AddWithValue("@eSignaturePath", objEntity.eSignaturePath);
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

        public virtual List<Entity.CompanyProfile> GetCompanyProfileList(Int64 CompanyID, string LoginUserID)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CompanyProfileList";
            cmdGet.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CompanyProfile> lstLocation = new List<Entity.CompanyProfile>();
            while (dr.Read())
            {
                Entity.CompanyProfile objLocation = new Entity.CompanyProfile();
                objLocation.CompanyID = GetInt64(dr, "CompanyID");
                objLocation.CompanyName = GetTextVale(dr, "CompanyName");
                objLocation.GSTNo = GetTextVale(dr, "GSTNo");
                objLocation.PANNo = GetTextVale(dr, "PANNo");
                objLocation.CINNo = GetTextVale(dr, "CINNo");
                objLocation.ParentCompanyID = GetInt64(dr, "ParentCompanyID");
                objLocation.Address = GetTextVale(dr, "Address");
                objLocation.Area = GetTextVale(dr, "Area");
                objLocation.CityCode = GetInt64(dr, "CityCode");
                objLocation.CityName = GetTextVale(dr, "CityName");
                objLocation.StateCode = GetInt64(dr, "StateCode");
                objLocation.StateName = GetTextVale(dr, "StateName");
                objLocation.Pincode = GetTextVale(dr, "Pincode");
                objLocation.chkCustomer = GetTextVale(dr, "chkCustomer");
                objLocation.chkInquiry = GetTextVale(dr, "chkInquiry");
                objLocation.chkQuotation = GetTextVale(dr, "chkQuotation");
                objLocation.chkSalesOrder = GetTextVale(dr, "chkSalesOrder");
                objLocation.chkLeaveRequest = GetTextVale(dr, "chkLeaveRequest");
                objLocation.chkFeedback = GetTextVale(dr, "chkFeedback");
                objLocation.Host = GetTextVale(dr, "Host");
                objLocation.UserName = GetTextVale(dr, "UserName");
                objLocation.Password = GetTextVale(dr, "Password");
                objLocation.PortNumber = GetInt64(dr, "PortNumber");
                objLocation.EnableSSL = GetBoolean(dr, "EnableSSL");
                objLocation.eSignaturePath = GetTextVale(dr, "eSignaturePath");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateBankInfo(Entity.OrganizationBankInfo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "OrganizationBankInfo_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CompanyID", objEntity.CompanyID);
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@BankName", objEntity.BankName);
            cmdAdd.Parameters.AddWithValue("@BankAccountName", objEntity.BankAccountName);
            cmdAdd.Parameters.AddWithValue("@BranchName", objEntity.BranchName);
            cmdAdd.Parameters.AddWithValue("@BankAccountNo", objEntity.BankAccountNo);
            cmdAdd.Parameters.AddWithValue("@BankIFSC", objEntity.BankIFSC);
            cmdAdd.Parameters.AddWithValue("@BankSWIFT", objEntity.BankSWIFT);
            //cmdAdd.Parameters.AddWithValue("@GSTNo", objEntity.GSTNo);
            //cmdAdd.Parameters.AddWithValue("@PANNo", objEntity.PANNo);
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
        public virtual List<Entity.OrganizationBankInfo> GetBankInfoList(Int64 CompanyID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationBankListByCompID";
            cmdGet.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationBankInfo> lstLocation = new List<Entity.OrganizationBankInfo>();
            while (dr.Read())
            {
                Entity.OrganizationBankInfo objLocation = new Entity.OrganizationBankInfo();
                objLocation.pkID = GetInt64(dr, "pkID");
                //objLocation.CompanyID = GetInt64(dr, "CompanyID");
                //objLocation.CompanyName = GetTextVale(dr, "CompanyName");
                objLocation.BankName = GetTextVale(dr, "BankName");
                objLocation.BranchName = GetTextVale(dr, "BranchName");
                objLocation.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objLocation.BankIFSC = GetTextVale(dr, "BankIFSC");
                objLocation.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.OrganizationBankInfo> GetBankInfoListBypkID(Int64 pkID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationBankListBypkID";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationBankInfo> lstLocation = new List<Entity.OrganizationBankInfo>();
            while (dr.Read())
            {
                Entity.OrganizationBankInfo objLocation = new Entity.OrganizationBankInfo();
                objLocation.pkID = GetInt64(dr, "pkID");
                //objLocation.CompanyID = GetInt64(dr, "CompanyID");
                //objLocation.CompanyName = GetTextVale(dr, "CompanyName");
                objLocation.BankName = GetTextVale(dr, "BankName");
                objLocation.BranchName = GetTextVale(dr, "BranchName");
                objLocation.BankAccountName = GetTextVale(dr, "BankAccountName");
                objLocation.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objLocation.BankIFSC = GetTextVale(dr, "BankIFSC");
                objLocation.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.OrganizationBankInfo> GetBankInfo(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationBankInfo> lstObject = new List<Entity.OrganizationBankInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationBankList";
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
                Entity.OrganizationBankInfo objEntity = new Entity.OrganizationBankInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                //objEntity.CompanyID = GetInt64(dr, "CompanyID");
                //objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.OrganizationBankInfo> GetBankInfo(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.OrganizationBankInfo> lstObject = new List<Entity.OrganizationBankInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "OrganizationBankList";
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
                Entity.OrganizationBankInfo objEntity = new Entity.OrganizationBankInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                //objEntity.CompanyID = GetInt64(dr, "CompanyID");
                //objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.BankName = GetTextVale(dr, "BankName");
                objEntity.BranchName = GetTextVale(dr, "BranchName");
                objEntity.BankAccountName = GetTextVale(dr, "BankAccountName");
                objEntity.BankAccountNo = GetTextVale(dr, "BankAccountNo");
                objEntity.BankIFSC = GetTextVale(dr, "BankIFSC");
                objEntity.BankSWIFT = GetTextVale(dr, "BankSWIFT");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void DeleteBankDetails(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "BankInfo_DEL";
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
        public virtual List<Entity.CompanyProfile> GetCostantList(string Category, string ConstantHead, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "Constantist";
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@ConstantHead", ConstantHead);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.CompanyProfile> lstLocation = new List<Entity.CompanyProfile>();
            while (dr.Read())
            {
                Entity.CompanyProfile objLocation = new Entity.CompanyProfile();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.Category = GetTextVale(dr, "Category");
                objLocation.Description = GetTextVale(dr, "Description");
                objLocation.ConstantHead = GetTextVale(dr, "ConstantHead");
                objLocation.ConstantValue = GetTextVale(dr, "ConstantHead");
                objLocation.Host = GetTextVale(dr, "Host");
                objLocation.EnableSSL = GetBoolean(dr, "EnableSSL");
                objLocation.UserName = GetTextVale(dr, "UserName");
                objLocation.Password = GetTextVale(dr, "Password");
                objLocation.PortNumber = GetInt64(dr, "PortNumber");

                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void UpdateEmailNotificationStatus(string pHeader, string pValue, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "Update MST_Constant Set ConstantValue = @pValue Where ConstantHead = @pHeader";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@pHeader", SqlDbType.VarChar).Value = pHeader;
                cmdAdd.Parameters.Add("@pValue", SqlDbType.VarChar).Value = pValue;
                ExecuteNonQuery(cmdAdd);

                ReturnCode = 1;
                ReturnMsg = "Data Updated Successfully !";
            }
            catch (Exception ex)
            {
                ReturnCode = 0;
                ReturnMsg = ex.Message.ToString();
            }
            ForceCloseConncetion();
        }

        public virtual void AddUpdateUserLog(Entity.UserLog objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "UserLog_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@UserID", objEntity.UserID);
            cmdAdd.Parameters.AddWithValue("@MacID", objEntity.MacID);
            cmdAdd.Parameters.AddWithValue("@INOUT", objEntity.INOUT);
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

        public virtual List<Entity.Documents> GetDocumentsList(Int64 pkID, Int64 pProductID)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ProductDocumentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ProductID", pProductID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Documents> lstLocation = new List<Entity.Documents>();
            while (dr.Read())
            {
                Entity.Documents objLocation = new Entity.Documents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.ProductID = GetInt64(dr, "ProductID");
                objLocation.FileName = GetTextVale(dr, "Name");
                objLocation.FileType = GetTextVale(dr, "Type");
                //objLocation.FileData = GetBase64(dr, "Data");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateProductDocuments(Int64 pProductID, string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "insert into MST_Product_Documents (ProductID, Name,type,createdby)" + " values (@ProductID, @Name, @type, @LoginUserID)";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@ProductID", SqlDbType.BigInt).Value = pProductID;
                cmdAdd.Parameters.Add("@Name", SqlDbType.VarChar).Value = pFilename;
                cmdAdd.Parameters.Add("@type", SqlDbType.VarChar).Value = pType;
                cmdAdd.Parameters.Add("@LoginUserID", SqlDbType.VarChar).Value = pLoginUserID;
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

        public virtual void DeleteProductDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductDocuments_DEL";
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
        public virtual void DeleteProductDocumentsByProductId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ProductDocumentsByProductId_DEL";
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

        // **********************************************************************
        public virtual List<Entity.Documents> GetDocumentGalleryList(Int64 pkID)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DocumentGalleryList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Documents> lstLocation = new List<Entity.Documents>();
            while (dr.Read())
            {
                Entity.Documents objLocation = new Entity.Documents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.FileName = GetTextVale(dr, "Name");
                objLocation.FileType = GetTextVale(dr, "Type");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual List<Entity.Documents> GetDocumentGalleryListByName(String pFileName)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "DocumentGalleryListByName";
            cmdGet.Parameters.AddWithValue("@FileName", pFileName);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Documents> lstLocation = new List<Entity.Documents>();
            while (dr.Read())
            {
                Entity.Documents objLocation = new Entity.Documents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.FileName = GetTextVale(dr, "Name");
                objLocation.FileType = GetTextVale(dr, "Type");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void AddUpdateDocumentGallery(string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "insert into MST_Document_Gallery (Name, type, createdby)" + " values (@Name, @type, @LoginUserID)";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@Name", SqlDbType.VarChar).Value = pFilename;
                cmdAdd.Parameters.Add("@type", SqlDbType.VarChar).Value = pType;
                cmdAdd.Parameters.Add("@LoginUserID", SqlDbType.VarChar).Value = pLoginUserID;
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

        public virtual void DeleteDocumentGallery(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DocumentGallery_DEL";
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

        public virtual void DeleteDocumentGalleryByFileName(String pFileName, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "DocumentGalleryByFileName_DEL";
            cmdDel.Parameters.AddWithValue("@FileName", pFileName);
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

        public virtual List<Entity.Contents> GetContentList(Int64 pkID, string pCategory)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ContentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 50000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Contents> lstLocation = new List<Entity.Contents>();
            while (dr.Read())
            {
                Entity.Contents objLocation = new Entity.Contents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.Category = GetTextVale(dr, "Category");
                objLocation.TNC_Header = GetTextVale(dr, "TNC_Header");
                objLocation.TNC_Content = GetTextVale(dr, "TNC_Content");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Contents> GetContentList(Int64 pkID, string pCategory, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ContentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Contents> lstLocation = new List<Entity.Contents>();
            while (dr.Read())
            {
                Entity.Contents objLocation = new Entity.Contents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.Category = GetTextVale(dr, "Category");
                objLocation.TNC_Header = GetTextVale(dr, "TNC_Header");
                objLocation.TNC_Content = GetTextVale(dr, "TNC_Content");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            return lstLocation;
        }

        public virtual void AddUpdateContents(Entity.Contents objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Contents_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@Category", objEntity.Category);
            cmdAdd.Parameters.AddWithValue("@TNC_Header", objEntity.TNC_Header);
            cmdAdd.Parameters.AddWithValue("@TNC_Content", objEntity.TNC_Content);
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

        public virtual void DeleteContents(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Contents_DEL";
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

        public virtual byte[] GetMemberPhotoID(string pRegistrationNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GetMemberImage";
            cmdAdd.Parameters.AddWithValue("@RegistrationNo", pRegistrationNo);
            SqlDataReader dr = ExecuteDataReader(cmdAdd);
            byte[] msImage = null;
            while (dr.Read())
            {
                msImage = (byte[])dr["MemberImage"];
            }
            dr.Close();
            ForceCloseConncetion();
            return msImage;
        }

        public virtual byte[] GetDriverPhotoID(Int64 pDriverID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "GetDriverImage";
            cmdAdd.Parameters.AddWithValue("@DriverID", pDriverID);
            SqlDataReader dr = ExecuteDataReader(cmdAdd);
            byte[] msImage = null;
            while (dr.Read())
            {
                msImage = (byte[])dr["DriverImage"];
            }
            dr.Close();
            ForceCloseConncetion();
            return msImage;
        }

        public virtual string RetrieveFormattedAddress(string lat, string lng)
        {

            string baseUri = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&libraries=places&radius=500&types=shopping_mall,food,restaurant";

            string location = string.Empty;

            string requestUri = string.Format(baseUri, lat, lng);

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants() where elm.Name == "status" select elm).FirstOrDefault();

                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants() where elm.Name == "formatted_address" select elm).FirstOrDefault();
                    requestUri = res.Value;
                }
            }
            return requestUri;
        }

        // ===========================================================================================================
        // Procedure : To Send Email Notification (Thank you, Welcome, Alert Notification Email and many more...)
        // ===========================================================================================================

        public virtual string SendAlertNotificationEmail(string pTemplateID, string pEmailTo, string pHelpLogOrgCode, string pRelationType, string pOrgCode, string pOrgName, string pOrgDeptCode, string pOrgDeptName, string pContactName, string pTicketNo, string pMemberName, string pLatitude, string pLongitude, string nearByPlace)
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
            while (dr.Read())
            {
                string body = string.Empty;
                //body = GetTextVale(dr, "ContentData");
                // ===================================================================================================
                // Custom Email Format : Family & Friends 
                // ===================================================================================================
                //List<Entity.DispatchNotification> lstEntity = new List<Entity.DispatchNotification>();
                //lstEntity = GetDispatchNotificationInfo(pHelpLogOrgCode, Convert.ToInt64(pTicketNo));
                // --------------------------------------------------------------------------------------------------
                //List<Entity.TripPlanning> lstTripPlanning = new List<Entity.TripPlanning>();
                //lstTripPlanning = GetTripPlanningListByTicketNo(Convert.ToInt64(pTicketNo));
                // ------------------------------------------------------------------------------
                //string memberImageURL="", driverImageURL="", tmpValue1="", tmpValue2="";

                //if (lstTripPlanning.Count > 0)
                //{
                //    tmpValue1 = Convert.ToBase64String(GetMemberPhotoID(lstTripPlanning[0].RegistrationNo));
                //    tmpValue2 = Convert.ToBase64String(GetDriverPhotoID(lstTripPlanning[0].DriverID));

                //    if (tmpValue1 != null)
                //        memberImageURL = string.Format("data:image/jpeg;base64,{0}", tmpValue1);

                //    if (tmpValue2 != null)
                //        driverImageURL = string.Format("data:image/jpeg;base64,{0}", tmpValue2);
                //}
                // --------------------------------------------------------------------------------------------------
                if (String.IsNullOrEmpty(pOrgCode))
                {
                    body = "Dear <b>{ContactName},</b><br /><br /><br />This is to inform you that {MemberName} initiated an emergency button for help.";
                    body = body + "Our Help dispatch center already informed Police authorities for the same.<br /><br />";
                    body = body + "<b>We also request you to contact your member at your earliest.</b><br /><br /><br />";
                }
                else
                {
                    body = "To,<br /><b>{ContactName}</b><br /><br />{Designation}<br />{OrgName}<br />{OrgDeptName}";
                    body = body + "<br /><br />This is to inform you that {MemberName} initiated an emergency button for help. Our Help dispatch center informed below mentioned Police Station and PCR Van.";
                    body = body + "<br />You are requested to coordinate with concerned police stations and reach incident place immediately.<br /><br />";
                }
                // --------------------------------------------------------------------------------------------------
                body = body + "<table style='border: 1px solid black;' width='100%'><tr style='color: Navy; font-size:14px; font-weight: bold; border: 1px solid black;'>";
                body = body + "<td style='border: 1px solid black;'>Member Name</td>";
                body = body + "<td style='border: 1px solid black;'>Email Address</td>";
                body = body + "<td style='border: 1px solid black;'>Mobile #</td>";
                body = body + "<td style='border: 1px solid black;'>Landline</td>";
                body = body + "<td style='border: 1px solid black;'>Relation/Designation</td></tr>";
                //foreach (Entity.DispatchNotification lstObj in lstEntity) // Loop through List with foreach.
                //{
                //    body = body + "<tr style='color: Black; font-size:12px; font-weight: bold; border: 1px solid black;'>";
                //    body = body + "<td style='border: 1px solid black;'>" + lstObj.MemberName + "</td>";
                //    body = body + "<td style='border: 1px solid black;'>" + lstObj.EmailAddress + "</td>";
                //    body = body + "<td style='border: 1px solid black;'>" + lstObj.MobileNo + "</td>";
                //    body = body + "<td style='border: 1px solid black;'>" + lstObj.Landline + "</td>";
                //    body = body + "<td style='border: 1px solid black;'>" + lstObj.RelationType + "</td>";
                //    body = body + "</tr>";
                //}
                body = body + "</table><br /><br />";
                // --------------------------------------------------------------------------------------------------
                body = body + "<table style='border: 1px solid black;' width='100%'>";
                body = body + "<tr style='color: Red; font-size:14px; font-weight: bold; border: 1px solid black;' >";
                body = body + "<td colspan='3' style='border: 1px solid black; text-align: center;'>Help Log Ticket Information</td></tr>";
                body = body + "<tr style='color: black; font-size:14px; font-weight: bold; border: 1px solid black;' >";
                body = body + "<td style='border: 1px solid black; text-align: center;'>Ticket #</td><td style='border: 1px solid black; text-align: center;'>Latitude</td><td style='border: 1px solid black; text-align: center;'>Longitude</td></tr>";
                body = body + "<tr style='color: black; font-size:14px; font-weight: bold; border: 1px solid black;' >";
                body = body + "<td style='border: 1px solid black; text-align: center;'>{TicketNo}</td><td style='border: 1px solid black; text-align: center;'>{Latitude}</td><td style='border: 1px solid black; text-align: center;'>{Longitude}</td></tr>";
                body = body + "<tr style='color: Navy; font-size:14px; font-weight: bold; border: 1px solid black;' >";
                body = body + "<td colspan='3' style='border: 1px solid black; text-align:center;'><b>Nearest Landmark: " + nearByPlace + "</b></td></tr></table>";
                // --------------------------------------------------------------------------------------------------
                //if (lstTripPlanning.Count > 0)
                //{
                body = body + "<br /><br /><table style='border: 2px solid Navy;' width='100%'>";
                body = body + "<tr><td width='50%' style='text-align:center;color: Navy; font-size:14px; font-weight: bold;'>Member Information</td><td width='50%' style='text-align:center;color: Red; font-size:14px; font-weight: bold;'>Driver Information</td></tr>";
                body = body + "<tr><td width='50%' style='text-align:center;'><img src='{memberImage}' /></td><td width='50%' style='text-align:center;'><img src='{driverImage}' /></td></tr>";
                body = body + "<tr><td width='50%' style='background-color:Navy; color:White; text-align:center;'>{MemberName}</td><td width='50%' style='background-color:Navy; color:Red; text-align:center;'>{DriverName}</td></tr>";
                body = body + "</table>";
                //}
                // --------------------------------------------------------------------------------------------------
                body = body + "<br /><br />" + "Please click below link to check incident location.<br />";
                body = body + "<a href='https://www.google.co.in/maps/place/{Latitude}+{Longitude}/@{Latitude},{Longitude},17z'>https://www.google.co.in/maps/place/{Latitude}+{Longitude}/@{Latitude},{Longitude},17z</a>";
                body = body + "<br /><br /><b>Best Wishes</b><br /><br />I-LAB informatics Pvt Ltd.";
                // --------------------------------------------------------------------------------------------------
                body = body.Replace("{ContactName}", pContactName);
                //body = body.Replace("{MemberName}", lstTripPlanning[0].MemberName);
                body = body.Replace("{Designation}", pRelationType);
                body = body.Replace("{OrgName}", pOrgName);
                body = body.Replace("{OrgDeptName}", pOrgDeptName);

                body = body.Replace("{TicketNo}", pTicketNo);
                body = body.Replace("{Latitude}", pLatitude);
                body = body.Replace("{Longitude}", pLongitude);

                //if (lstTripPlanning.Count > 0)
                //{
                //    body = body.Replace("{DriverName}", lstTripPlanning[0].DriverName);

                //    if (!String.IsNullOrEmpty(memberImageURL))
                //        body = body.Replace("{memberImage}", memberImageURL);

                //    if (!String.IsNullOrEmpty(driverImageURL))
                //        body = body.Replace("{driverImage}", driverImageURL);
                //}
                // ---------------------------------------------------------------------------------
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                    mailMessage.Subject = GetTextVale(dr, "Subject"); ;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    //mailMessage.Attachments = new Attachment(Convert.ToBase64String(img , "MemberPhotoID");
                    mailMessage.To.Add(new MailAddress(pEmailTo));
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
            return "Alert Notrification Sent Successfully !";
        }

        //public virtual string SendTripNotificationEmail(string pTemplateID, string pTicketNo)
        //{
        //    SqlCommand cmdGet = new SqlCommand();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "EmailTemplateList";
        //    cmdGet.Parameters.AddWithValue("@TemplateID", pTemplateID);
        //    cmdGet.Parameters.AddWithValue("@ListMode", "L");
        //    cmdGet.Parameters.AddWithValue("@PageNo", 1);
        //    cmdGet.Parameters.AddWithValue("@PageSize", 10);
        //    SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
        //    p.Direction = ParameterDirection.Output;
        //    cmdGet.Parameters.Add(p);
        //    SqlDataReader dr = ExecuteDataReader(cmdGet);
        //    // ------------------------------------------------------------------
        //    while (dr.Read())
        //    {
        //        string body = string.Empty;
        //        //body = GetTextVale(dr, "ContentData");
        //        // ===================================================================================================
        //        // Custom Email Format : Family & Friends 
        //        // ===================================================================================================
        //        List<Entity.TripPlanning> lstTripPlanning = new List<Entity.TripPlanning>();
        //        lstTripPlanning = GetTripPlanningList(Convert.ToInt64(pTicketNo));

        //        string tmpValue = Convert.ToBase64String(GetDriverPhotoID(lstTripPlanning[0].DriverID));
        //        string driverImageURL=""; 
        //        if (tmpValue != null)
        //            driverImageURL = string.Format("data:image/jpeg;base64,{0}", tmpValue);
        //        // --------------------------------------------------------------------------------------------------
        //        body = "Welcome <b>{CustomerName},</b><br /><br /";
        //        body = body + "<br /><br />You have chosen “OLA CAB SERVICE” to serve you. Your trip details are as follows:<br /><br />";
        //        // --------------------------------------------------------------------------------------------------
        //        body = body + "<table style='border: 1px solid black;' width='100%'>";
        //        body = body + "<tr style='border: 1px solid black;'>";
        //        body = body + "<td style='border: 1px solid black; color: Navy; font-size:14px; font-weight: bold; text-align:center;' width='50%'>Pickup Address</td>";
        //        body = body + "<td style='border: 1px solid black; color: Navy; font-size:14px; font-weight: bold; text-align:center;' width='50%'>Destination Address</td></tr>";
        //        body = body + "<tr style='border: 1px solid black;'>";
        //        body = body + "<td style='border: 1px solid black; color: Navy; font-size:14px; font-weight: bold; text-align:center;' width='50%'>{TravelFrom}</td>";
        //        body = body + "<td style='border: 1px solid black; color: Navy; font-size:14px; font-weight: bold; text-align:center;' width='50%'>{TravelTo}</td></tr>";
        //        body = body + "</table><br /><br />";
        //        // --------------------------------------------------------------------------------------------------
        //        body = body + "<table style='border: 1px solid black;' width='100%'>";
        //        body = body + "<tr style='color: Red; font-size:14px; font-weight: bold; border: 1px solid black;' >";
        //        body = body + "<td colspan='3' style='border: 1px solid black; text-align: center;'>Trip Booking Information</td></tr>";
        //        body = body + "<tr style='color: black; font-size:14px; font-weight: bold; border: 1px solid black;' >";
        //        body = body + "<td style='border: 1px solid black; text-align: center;'>Booking ID</td><td style='border: 1px solid black; text-align: center;'>Booking Date/Time</td><td style='border: 1px solid black; text-align: center;'>Vehicle Registration #</td></tr>";
        //        body = body + "<tr style='color: black; font-size:14px; font-weight: bold; border: 1px solid black;' >";
        //        body = body + "<td style='border: 1px solid black; text-align: center;'>{BookingID}</td><td style='border: 1px solid black; text-align: center;'>{BookingDate}</td><td style='border: 1px solid black; text-align: center;'>{VehicleRegNo}</td></tr>";
        //        body = body + "</table>";
        //        // --------------------------------------------------------------------------------------------------
        //        body = body + "<br /><br /><table style='border: 2px solid Navy;' width='100%'>";
        //        body = body + "<tr><td style='text-align:center;color: Red; font-size:16px; font-weight: bold;'>Driver Information</td></tr>";
        //        body = body + "<tr><td style='text-align:center;'><img src='{driverImage}' /></td></tr>";
        //        body = body + "<tr><td style='background-color:Navy; color:Red; text-align:center;'>Driver Name  :&nbsp;{DriverName}</td></tr>";
        //        body = body + "</table>";
        //        body = body + "<br /><br /><br /><br />Please not that your cab is equipped with latest GPS based Emergency alert ";
        //        body = body + "activation system – “KAVACH”. For your comfort the safety device is fitted right "; 
        //        body = body + "behind the co driver seat, please make sure that “KAVACH” is on POWER ON mode.";
        //        body = body + "<br /><br />Please follow these standard procedures when you aboard the cab:<br /><br />";

        //        body = body + "<ul>";
        //        body = body + "<li>Please press “Start Trip” button flashing on the KAVACH screen once you aboard the cab.</li>";
        //        body = body + "<li>Screen will flash your journey details with map view facility for navigation.</li>";
        //        body = body + "<li>On top right hand side corner of screen, KAVACH is equipped with SOS button which is the world’s most advance emergency alert activation and response mechanism system developed for your protection, please press the button in case of any emergency for the prompt help.</li>";
        //        body = body + "<li>Please press “End trip” button once you reach to your destination safely.</li>";
        //        body = body + "</ul>";
        //        body = body + "<br /><br /><br />";
        //        // --------------------------------------------------------------------------------------------------
        //        body = body + "<br /><br />On behalf of team <b>कवच</b> we are wishing you a safe and happy journey with us. We are looking forward to seeing you on board again !!<br /><br /><br /><b>Regards</b><br /><br />Team कवच<br/><br /><br /><b>॥ सर्वेडत्र सुखिनः सन्तु ॥</b><br /><br />";
        //        // --------------------------------------------------------------------------------------------------
        //        body = body.Replace("{CustomerName}", lstTripPlanning[0].MemberName);
        //        body = body.Replace("{TravelFrom}", lstTripPlanning[0].TravelFrom);
        //        body = body.Replace("{TravelTo}", lstTripPlanning[0].TravelTo);

        //        body = body.Replace("{BookingID}", lstTripPlanning[0].pkID.ToString());
        //        body = body.Replace("{BookingDate}", lstTripPlanning[0].BookDate.ToString("dd-MM-yyyyTHH:mm"));
        //        body = body.Replace("{VehicleRegNo}", lstTripPlanning[0].VehicleRegNo);

        //        body = body.Replace("{DriverName}", lstTripPlanning[0].DriverName);
        //        if (tmpValue != null)
        //            body = body.Replace("{driverImage}", driverImageURL);
        //        // ---------------------------------------------------------------------------------
        //        using (MailMessage mailMessage = new MailMessage())
        //        {
        //            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
        //            mailMessage.Subject ="Trip Booking Notification";
        //            mailMessage.Body = body;
        //            mailMessage.IsBodyHtml = true;
        //            mailMessage.To.Add(new MailAddress(lstTripPlanning[0].EmailAddress));
        //            SmtpClient smtp = new SmtpClient();
        //            smtp.Host = ConfigurationManager.AppSettings["Host"];
        //            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        //            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
        //            NetworkCred.UserName = ConfigurationManager.AppSettings["UserName"];
        //            NetworkCred.Password = ConfigurationManager.AppSettings["Password"];
        //            smtp.UseDefaultCredentials = true;
        //            smtp.Credentials = NetworkCred;
        //            smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
        //            smtp.Send(mailMessage);
        //        }
        //    }
        //    return "Trip Notification Sent Successfully !";
        //}
        // =======================================================================================================================

        public virtual List<Entity.DashboardCountSummary> GetEmployeeFollowupCount(String pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select dbo.fnGetEmployeeByUserID(createdby) as employee, Count(*) as total from Inquiry_Followup where customerid = 21403 group by dbo.fnGetEmployeeByUserID(createdby);";
            //string varResult = ExecuteScalar(myCommand).ToString();
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.DashboardCountSummary> lstLocation = new List<Entity.DashboardCountSummary>();
            while (dr.Read())
            {
                Entity.DashboardCountSummary objLocation = new Entity.DashboardCountSummary();
                objLocation.label = GetTextVale(dr, "employee");
                objLocation.value = GetInt64(dr, "total");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.SMS> GetSMSConfigSettings(String pCompanyID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "sp_getSMSConfig";
            cmdAdd.Parameters.AddWithValue("@CompanyID", pCompanyID);
            OpenConncetionRegistration(cmdAdd);
            cmdAdd.ExecuteNonQuery();
            SqlDataReader dr = cmdAdd.ExecuteReader();
            List<Entity.SMS> lstSMSConfig = new List<Entity.SMS>();
            while (dr.Read())
            {
                Entity.SMS objLocation = new Entity.SMS();
                objLocation.AuthKey = GetTextVale(dr, "AuthKey");
                objLocation.SenderID = GetTextVale(dr, "SenderID");
                objLocation.UserName = GetTextVale(dr, "UserName");
                objLocation.Password = GetTextVale(dr, "Password");
                objLocation.SMSType = GetTextVale(dr, "SMSType");
                objLocation.PortalName = GetTextVale(dr, "PortalName");
                lstSMSConfig.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstSMSConfig;
        }

        //public virtual string SendSMSCampaign(String pCompanyID, String MobileNo, String strMsg, List<Entity.SMS> lstSMSConfig)
        //{
        //    ////Your authentication key
        //    //string authKey = "263473AonJouMY5c6a5a1c";
        //    ////Multiple mobiles numbers separated by comma
        //    //string mobileNumber = "8866156626";
        //    ////Sender ID,While using route4 sender id should be 6 characters long.
        //    //string senderId = "DM-CRM";
        //    ////Your message to send, Add URL encoding here.
        //    //string message = HttpUtility.UrlEncode("Welcome To Sharvaya Infotech");

        //    //Prepare you post parameters
        //    //System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
        //    //sbPostData.AppendFormat("authkey={0}", authKey);
        //    //sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
        //    //sbPostData.AppendFormat("&message={0}", message);
        //    //sbPostData.AppendFormat("&sender={0}", senderId);
        //    //sbPostData.AppendFormat("&route={0}", "4");

        //    //Prepare you post parameters
        //    System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
        //    sbPostData.AppendFormat("authkey={0}", lstSMSConfig[0].AuthKey);
        //    sbPostData.AppendFormat("&mobiles={0}", MobileNo);
        //    sbPostData.AppendFormat("&message={0}", strMsg);
        //    sbPostData.AppendFormat("&sender={0}", lstSMSConfig[0].SenderID);
        //    sbPostData.AppendFormat("&route={0}", "default");

        //    try
        //    {
        //        //Call Send SMS API
        //        string sendSMSUri = "http://api.msg91.com/api/sendhttp.php";
        //        //Create HTTPWebrequest
        //        System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);
        //        //Prepare and Add URL Encoded data
        //        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //        byte[] data = encoding.GetBytes(sbPostData.ToString());
        //        //Specify post method
        //        httpWReq.Method = "POST";
        //        httpWReq.ContentType = "application/x-www-form-urlencoded";
        //        httpWReq.ContentLength = data.Length;
        //        using (System.IO.Stream stream = httpWReq.GetRequestStream())
        //        {
        //            stream.Write(data, 0, data.Length);
        //        }
        //        //Get the response
        //        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        string responseString = reader.ReadToEnd();

        //        //Close the response
        //        reader.Close();
        //        response.Close();
        //        return "SUCESS";
        //    }
        //    catch (SystemException ex)
        //    {
        //        return "SMS Sending Failed!";
        //    }
        //}


        public virtual string SendSMSCampaign(String pLoginUserID, String MobileNo, String strMsg, List<Entity.SMS> lstSMSConfig)
        {
            //http://sms.hspsms.com/sendSMS?username=femicure&message=Test&sendername=FEMICURE&smstype=TRANS&numbers=8140939704&apikey=5119b0de-8b01-4542-9b25-675e9a4d4e58

            string PortalName = lstSMSConfig[0].PortalName.Trim();
            string authKey = lstSMSConfig[0].AuthKey.Trim();    //Your authentication key
            string mobileNumber = MobileNo;                     //Multiple mobiles numbers separated by comma   //"9925474579"; //"9925011579";
            string username = lstSMSConfig[0].UserName.Trim();
            string Password = lstSMSConfig[0].Password.Trim();
            string SMSType = lstSMSConfig[0].SMSType.Trim();    //Route Or transactional/Promotional
            string senderId = lstSMSConfig[0].SenderID.Trim();  //Sender ID,While using route4 sender id should be 6 characters long.
            string message = HttpUtility.UrlEncode(strMsg);     //Your message to send, Add URL encoding here.

            try
            {
                string sendSMSUri = "";
                System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();    //Prepare you post parameters

                if (PortalName.ToLower() == "msg 91")
                {
                    sbPostData.AppendFormat("authkey={0}", authKey);
                    sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
                    sbPostData.AppendFormat("&message={0}", message);
                    sbPostData.AppendFormat("&sender={0}", senderId);
                    sbPostData.AppendFormat("&route={0}", SMSType);
                    sendSMSUri = "http://api.msg91.com/api/sendhttp.php";                //Call Send SMS API
                }
                else if (PortalName.ToLower() == "hsp media")
                {
                    sbPostData.AppendFormat("username={0}", username);
                    sbPostData.AppendFormat("&apikey={0}", authKey);
                    sbPostData.AppendFormat("&numbers={0}", mobileNumber);
                    sbPostData.AppendFormat("&message={0}", message);
                    sbPostData.AppendFormat("&sendername={0}", senderId);
                    sbPostData.AppendFormat("&smstype={0}", SMSType);
                    sendSMSUri = "http://sms.hspsms.com/sendSMS";
                }

                System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);       //Create HTTPWebrequest
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();         //Prepare and Add URL Encoded data
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                httpWReq.Method = "POST";                                                   //Specify post method
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (System.IO.Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();   //Get the response
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                reader.Close();       //Close the response
                response.Close();

                return responseString;
            }
            catch (SystemException ex)
            {
                return "SMS Sending Failed! :" + ex.Message;
                //MessageBox.Show(ex.Message.ToString());
            }
        }

        //public virtual string SendSMSCampaign(String pLoginUserID, String MobileNo, String strMsg, List<Entity.SMS> lstSMSConfig)
        //{
        //    //http://sms.hspsms.com/sendSMS?username=femicure&message=Test&sendername=FEMIQR&smstype=TRANS&numbers=8140939704&apikey=5119b0de-8b01-4542-9b25-675e9a4d4e58

        //    string PortalName = lstSMSConfig[0].PortalName.Trim();
        //    string authKey = lstSMSConfig[0].AuthKey.Trim();
        //    string mobileNumber = MobileNo;
        //    string senderId = lstSMSConfig[0].SenderID.Trim();
        //    string Password = lstSMSConfig[0].Password.Trim();   
        //    string message = HttpUtility.UrlEncode(strMsg);
        //    string username = lstSMSConfig[0].UserName.Trim();
        //    string SMSType = lstSMSConfig[0].SMSType.Trim();

        //    System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
        //    sbPostData.AppendFormat("username={0}", username);
        //    sbPostData.AppendFormat("&apikey={0}", authKey);
        //    sbPostData.AppendFormat("&numbers={0}", mobileNumber);
        //    sbPostData.AppendFormat("&message={0}", message);
        //    sbPostData.AppendFormat("&sendername={0}", senderId);
        //    sbPostData.AppendFormat("&smstype={0}", SMSType);

        //    try
        //    {
        //        string sendSMSUri = "http://sms.hspsms.com/sendSMS";
        //        System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);
        //        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //        byte[] data = encoding.GetBytes(sbPostData.ToString());
        //        httpWReq.Method = "POST";
        //        httpWReq.ContentType = "application/x-www-form-urlencoded";
        //        httpWReq.ContentLength = data.Length;
        //        using (System.IO.Stream stream = httpWReq.GetRequestStream())
        //        {
        //            stream.Write(data, 0, data.Length);
        //        }
        //        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        string responseString = reader.ReadToEnd();
        //        reader.Close();
        //        response.Close();

        //        return responseString;
        //    }
        //    catch (SystemException ex)
        //    {
        //        return "SMS Sending Failed!";
        //    }
        //}



        // ===================================================================================================
        // All User Defined Function's 
        // ===================================================================================================

        public static string GetEmployeeEmailAddress(String pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select emp.EmailAddress From OrganizationEmployee emp Inner Join MST_Users usr On emp.pkID = usr.EmployeeID Where usr.UserID = '" + pUserID + "'";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static String GetOrgHeadEmployee(String OrgCode)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select usr.UserID From OrganizationStructure emp Inner Join MST_Users usr On emp.OrgHead = usr.EmployeeID Where emp.OrgCode = '" + OrgCode + "'";
            String varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static String GetOrgCodeByUserID(String LoginUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select OrgCode From MST_Users Where UserID  = '" + LoginUserID + "'";
            String varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetCustomerEmailAddress(String pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select emp.EmailAddress From MST_Customer emp Inner Join MST_Users usr On emp.CustomerID = usr.CustomerID Where usr.UserID = '" + pUserID + "'";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetContractInfoNoPrimaryID(string pInqNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From ContractInfo Where InquiryNo = '" + pInqNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetEmployeeCode(Int64 pEmpCode)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select ISNULL(EmpCode,0) From OrganizationEmployee WHere pkID = " + pEmpCode ;
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand));
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetInquiryNo(String pDate)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetInquiryNo(" + Convert.ToDateTime(pDate).ToString("dd-MM-yyyy") + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetMaterialMovementOrderNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select OrderNo From Material_Movement Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetQuotationNo(String pDate)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetQuotationNo(" + pDate + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetQuotationNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT QuotationNo From Quotation Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetProjectSheetNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ProjectSheetNo From ProjectSheet Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        
        public static string GetJobCardNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT pkID From JobCard Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static Int64 GetJobCardpkID(String JobCardNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT pkID From JobCard Where JobCardNo = '" + JobCardNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetVoucherNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select VoucherNo from Financial_Trans where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetQuotationNoPrimaryID(string pQuotNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From Quotation Where QuotationNo = '" + pQuotNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetQuotationNoFromInquiryNo(string pInquiryNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select Top 1 QuotationNo From Quotation Where InquiryNo = '" + pInquiryNo + "' Order By QuotationDate DESC";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetInquiryNoPrimaryID(string pInqNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From Inquiry Where InquiryNo = '" + pInqNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetSalesOrderPrimaryID(string pOrderNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From SalesOrder Where OrderNo = '" + pOrderNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 GetSalesBillPrimaryID(string pInvoiceNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From SalesBill Where InvoiceNo = '" + pInvoiceNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }
        public static Int64 GetPurchaseBillPrimaryID(string pInvoiceNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From PurchaseBill Where InvoiceNo = '" + pInvoiceNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        //public static string GetInwardNo()
        //{
        //    SqlCommand myCommand = new SqlCommand();
        //    myCommand.CommandType = CommandType.Text;
        //    myCommand.CommandText = "SELECT [dbo].fnGetInwardNo()";
        //    string varResult = ExecuteScalar(myCommand).ToString();
        //    ForceCloseConncetion();
        //    return varResult;
        //}
        public static string fnGetInwardNo(String pDate)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].GetInwardNoByDate('" + pDate + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetOutwardNo()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetOutwardNo()";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string fnGetOutwardNoByDate(String pDate)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetOutwardNo('" + pDate + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetOutwardNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT OutwardNo From Outward Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetInwardNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InwardNo From Inward Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetVoucherNoForPDF(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT VoucherNo From Financial_trans Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetIndentNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT IndentNo From Indent Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetJobCardOutwardNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT OutwardNo From JobCardOutward Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static Int64 GetInwardNoPrimaryID(string pInwardNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From Inward Where InwardNo = '" + pInwardNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }
        public static Int64 GetOutwardNoPrimaryID(string pOutwardNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select pkID From Outward Where OutwardNo = '" + pOutwardNo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetSalesOrderNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT OrderNo From SalesOrder Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetWorkOrderCommNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT OrderNo From WorkOrderComm Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetSalesOrderDealerNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT OrderNo From SalesOrderDealer Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetPurchaseOrderNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT OrderNo From PurchaseOrder Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static Int64 GetPurchaseOrderPrimaryID(string pPONo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "select pkID from PurchaseOrder Where OrderNo = '" + pPONo + "'";
            Int64 varResult = Convert.ToInt64(ExecuteScalar(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetPurchaseBillNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InvoiceNo From PurchaseBill Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetSalesBillNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT InvoiceNo From SalesBill Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetOrgEmpAccuPanelNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT EmployeeName From OrganizationEmployee Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetVisitAcupanelNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ComplaintNo From Complaint_Detail Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetComplaintVisitNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ComplaintNo From Complaint_Detail Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetSalesChallanNo(Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT ChallanNo From SalesChallan Where pkID = " + pkID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetSalesOrderNo(String pDate)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetSalesOrderNo(" + pDate + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetConstant(string pHead, Int64 pkID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (pkID == 0)
                myCommand.CommandText = "SELECT [dbo].fnGetConstant('" + pHead + "', 0)";
            else
                myCommand.CommandText = "SELECT [dbo].fnGetConstantByID(" + pkID.ToString() + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetConstant(string pHead, Int64 pkID, Int64 pCompanyID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            if (pkID == 0)
                myCommand.CommandText = "SELECT [dbo].fnGetConstant('" + pHead + "'," + pCompanyID.ToString() + ")";
            else
                myCommand.CommandText = "SELECT [dbo].fnGetConstantByID(" + pkID.ToString() + "," + pCompanyID.ToString() + ")";

            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetCompanyName()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetOrganizationName('001')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetEmployeeNameByUserID(string pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetEmployeeByUserID('" + pUserID + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetEmployeeIDByUserID(string pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetEmployeeIDByUserID('" + pUserID + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetUserIDByEmployeeID(Int64 pEmployeeID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnUserByEmployeeID(" + pEmployeeID + ")";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetEmployeeNameByEmployeeID(Int64 pEmpID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT EmployeeName From OrganizationEmployee Where pkID = " + pEmpID.ToString();
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetAuthorizedSignUserID(string pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select AuthorizedSign From OrganizationEmployee emp Inner Join viewCompanyUsers vcu On emp.pkID = vcu.EmployeeID Where lower(vcu.UserID) = '" + pUserID.ToLower() + "'";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetDesignationByUserID(string pUserID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetDesignationByUserID('" + pUserID + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetMemberName(string pRegistrationNo)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetMemberName(@RegistrationNo)";
            myCommand.Parameters.AddWithValue("@RegistrationNo", pRegistrationNo);
            string varResult = ExecuteScalar(myCommand).ToString();

            ForceCloseConncetion();
            return varResult;
        }

        public static string GetDriverName(string pDriverID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetDriverName(@DriverID)";
            myCommand.Parameters.AddWithValue("@DriverID", pDriverID);
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetRegistrationNoFromMac(string pMacID)
        {
            // This function will .. 1st Search from MemberMaster (Nagrik) ... 2nd Search from Trip Planning (CAB)
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetRegistrationNoFromMac(@MacID)";
            myCommand.Parameters.AddWithValue("@MacID", pMacID);
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static string CheckAutoPilotMode(string pLoginUserID)
        {
            // This function will .. 1st Search from MemberMaster (Nagrik) ... 2nd Search from Trip Planning (CAB)
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnChkConfiguration(@ConfigCode, @LoginUserID)";
            myCommand.Parameters.AddWithValue("@ConfigCode", "AUTOPILOT");
            myCommand.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            string varResult = "N"; // ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        public static Int64 setInquiryStatusFromFollowup(string pInquiryNo, Int64 pStatusID, Int64 pClosureID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Update Inquiry Set InquiryStatusID = " + pStatusID + ", ClosureReason=" + pClosureID + " Where InquiryNo = '" + pInquiryNo + "'";

            Int64 varResult = Convert.ToInt64(ExecuteNonQuery(myCommand).ToString());
            ForceCloseConncetion();
            return varResult;
        }

        public static string GetCustomerEmailAddress(Int64 CustomerID)
        {
            // This function will .. 
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT [dbo].fnGetCustomerEmailAddress(@CustomerID)";
            myCommand.Parameters.AddWithValue("@CustomerID", CustomerID);
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }

        // ===================================================================================================
        // All User Defined Function's 
        // ===================================================================================================

        public virtual List<Entity.DashboardCountSummary> GetWidgetList(string pLoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetWidgetList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "WidgetID");
                objEntity.flag = GetInt64(dr, "Active");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardInquirySummary(string pLoginUserID, Int64 pMonth, Int64 pYear, String pCategory)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquirySummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.value = GetDecimal(dr, "value");
                if (pCategory == "YEARLY")
                {
                    objEntity.value1 = GetDecimal(dr, "value1");
                    objEntity.value2 = GetDecimal(dr, "value2");
                    objEntity.value3 = GetDecimal(dr, "value3");
                    objEntity.value4 = GetDecimal(dr, "value4");
                    objEntity.value5 = GetDecimal(dr, "value5");
                    objEntity.value6 = GetDecimal(dr, "value6");
                }
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardInquirySummary> GetDashboardInquiryStatusSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquiryStatusSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.DashboardInquirySummary> lstEntity = new List<Entity.DashboardInquirySummary>();
            while (dr.Read())
            {
                Entity.DashboardInquirySummary objEntity = new Entity.DashboardInquirySummary();
                objEntity.CloseLost = GetInt64(dr, "CloseLost");
                objEntity.CloseSuccess = GetInt64(dr, "CloseSuccess");
                objEntity.Unknown = GetInt64(dr, "Unknown");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardInquirySourceSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquirySourceSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.Jan = GetDecimal(dr, "Jan");
                objEntity.Feb = GetDecimal(dr, "Feb");
                objEntity.Mar = GetDecimal(dr, "Mar");
                objEntity.Apr = GetDecimal(dr, "Apr");
                objEntity.May = GetDecimal(dr, "May");
                objEntity.Jun = GetDecimal(dr, "Jun");
                objEntity.Jul = GetDecimal(dr, "Jul");
                objEntity.Aug = GetDecimal(dr, "Aug");
                objEntity.Sep = GetDecimal(dr, "Sep");
                objEntity.Oct = GetDecimal(dr, "Oct");
                objEntity.Nov = GetDecimal(dr, "Nov");
                objEntity.Dec = GetDecimal(dr, "Dec");
                objEntity.value = GetDecimal(dr, "Value");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardInquiryDisQualiSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquiryDisqualiSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "Label");
                objEntity.Jan = GetDecimal(dr, "Jan");
                objEntity.Feb = GetDecimal(dr, "Feb");
                objEntity.Mar = GetDecimal(dr, "Mar");
                objEntity.Apr = GetDecimal(dr, "Apr");
                objEntity.May = GetDecimal(dr, "May");
                objEntity.Jun = GetDecimal(dr, "Jun");
                objEntity.Jul = GetDecimal(dr, "Jul");
                objEntity.Aug = GetDecimal(dr, "Aug");
                objEntity.Sep = GetDecimal(dr, "Sep");
                objEntity.Oct = GetDecimal(dr, "Oct");
                objEntity.Nov = GetDecimal(dr, "Nov");
                objEntity.Dec = GetDecimal(dr, "Dec");
                objEntity.value = GetDecimal(dr, "Value");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardInquiryTeleCallStatusSummary(string pLoginUserID, Int64 pYear)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardTeleCallStatusSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.Jan = GetDecimal(dr, "Jan");
                objEntity.Feb = GetDecimal(dr, "Feb");
                objEntity.Mar = GetDecimal(dr, "Mar");
                objEntity.Apr = GetDecimal(dr, "Apr");
                objEntity.May = GetDecimal(dr, "May");
                objEntity.Jun = GetDecimal(dr, "Jun");
                objEntity.Jul = GetDecimal(dr, "Jul");
                objEntity.Aug = GetDecimal(dr, "Aug");
                objEntity.Sep = GetDecimal(dr, "Sep");
                objEntity.Oct = GetDecimal(dr, "Oct");
                objEntity.Nov = GetDecimal(dr, "Nov");
                objEntity.Dec = GetDecimal(dr, "Dec");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardInquiryTeleDisQualiSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardTeleDisQualiSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.Jan = GetDecimal(dr, "Jan");
                objEntity.Feb = GetDecimal(dr, "Feb");
                objEntity.Mar = GetDecimal(dr, "Mar");
                objEntity.Apr = GetDecimal(dr, "Apr");
                objEntity.May = GetDecimal(dr, "May");
                objEntity.Jun = GetDecimal(dr, "Jun");
                objEntity.Jul = GetDecimal(dr, "Jul");
                objEntity.Aug = GetDecimal(dr, "Aug");
                objEntity.Sep = GetDecimal(dr, "Sep");
                objEntity.Oct = GetDecimal(dr, "Oct");
                objEntity.Nov = GetDecimal(dr, "Nov");
                objEntity.Dec = GetDecimal(dr, "Dec");
                objEntity.value = GetDecimal(dr, "value");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardInquiryTeleConversionSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardTeleConversionSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.value = GetDecimal(dr, "value");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardTeleEntrySummary(string pLoginUserID, string pLeadSource, string pCategory, Int64 pMonth, Int64 pYear)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardTeleEntrySummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@LeadSource", pLeadSource);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                if (pCategory.ToLower() == "monthly")
                {
                    objEntity.label = GetTextVale(dr, "label");
                    objEntity.Jan = GetDecimal(dr, "Jan");
                    objEntity.Feb = GetDecimal(dr, "Feb");
                    objEntity.Mar = GetDecimal(dr, "Mar");
                    objEntity.Apr = GetDecimal(dr, "Apr");
                    objEntity.May = GetDecimal(dr, "May");
                    objEntity.Jun = GetDecimal(dr, "Jun");
                    objEntity.Jul = GetDecimal(dr, "Jul");
                    objEntity.Aug = GetDecimal(dr, "Aug");
                    objEntity.Sep = GetDecimal(dr, "Sep");
                    objEntity.Oct = GetDecimal(dr, "Oct");
                    objEntity.Nov = GetDecimal(dr, "Nov");
                    objEntity.Dec = GetDecimal(dr, "Dec");
                }
                else if (pCategory.ToLower() == "daily")
                {
                    objEntity.label = GetTextVale(dr, "label");
                    objEntity.d1 = GetDecimal(dr, "d1");
                    objEntity.d2 = GetDecimal(dr, "d2");
                    objEntity.d3 = GetDecimal(dr, "d3");
                    objEntity.d4 = GetDecimal(dr, "d4");
                    objEntity.d5 = GetDecimal(dr, "d5");
                    objEntity.d6 = GetDecimal(dr, "d6");
                    objEntity.d7 = GetDecimal(dr, "d7");
                    objEntity.d8 = GetDecimal(dr, "d8");
                    objEntity.d9 = GetDecimal(dr, "d9");
                    objEntity.d10 = GetDecimal(dr, "d10");
                    objEntity.d11 = GetDecimal(dr, "d11");
                    objEntity.d12 = GetDecimal(dr, "d12");
                    objEntity.d13 = GetDecimal(dr, "d13");
                    objEntity.d14 = GetDecimal(dr, "d14");
                    objEntity.d15 = GetDecimal(dr, "d15");
                    objEntity.d16 = GetDecimal(dr, "d16");
                    objEntity.d17 = GetDecimal(dr, "d17");
                    objEntity.d18 = GetDecimal(dr, "d18");
                    objEntity.d19 = GetDecimal(dr, "d19");
                    objEntity.d20 = GetDecimal(dr, "d20");
                    objEntity.d21 = GetDecimal(dr, "d21");
                    objEntity.d22 = GetDecimal(dr, "d22");
                    objEntity.d23 = GetDecimal(dr, "d23");
                    objEntity.d24 = GetDecimal(dr, "d24");
                    objEntity.d25 = GetDecimal(dr, "d25");
                    objEntity.d26 = GetDecimal(dr, "d26");
                    objEntity.d27 = GetDecimal(dr, "d27");
                    objEntity.d28 = GetDecimal(dr, "d28");
                    objEntity.d29 = GetDecimal(dr, "d29");
                    objEntity.d30 = GetDecimal(dr, "d30");
                    objEntity.d31 = GetDecimal(dr, "d31");
                }
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardSalesSummary(String pLoginUserID, Int64 pMonth, Int64 pYear, String pCategory)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardSalesSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.value = GetDecimal(dr, "value");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboardExternalSummary(String pLoginUserID, Int64 pMonth, Int64 pYear, String pCategory)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardExternalSummary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.value = GetDecimal(dr, "value");
                objEntity.value1 = GetDecimal(dr, "value1");
                objEntity.value2 = GetDecimal(dr, "value2");
                objEntity.value3 = GetDecimal(dr, "value3");
                objEntity.value4 = GetDecimal(dr, "value4");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual List<Entity.DashboardCountSummary> GetDashboard2_Summary(String pCategory, DateTime date1, DateTime date2, String pLoginUserID)
        {
            List<Entity.DashboardCountSummary> lstEntity = new List<Entity.DashboardCountSummary>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboard2_Summary";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@FromDate", date1);
            cmdGet.Parameters.AddWithValue("@ToDate", date2);
            cmdGet.Parameters.AddWithValue("@Category", pCategory);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DashboardCountSummary objEntity = new Entity.DashboardCountSummary();
                objEntity.label = GetTextVale(dr, "label");
                objEntity.value = GetDecimal(dr, "value");
                objEntity.value1 = GetDecimal(dr, "value1");
                objEntity.value2 = GetDecimal(dr, "value2");
                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public virtual string SendEmailNotify(string pTemplateID, string pLoginUserID, DataTable dtDetail)
        {
            String strErr = "";
            try
            {
                //string tmpQuotationNo = "", tmpQuotationNo1 = "", tmpInvoiceNo = "", tmpInvoiceNo1 = "", tmpOrderNo = "", tmpOrderNo1 = "", tmpPtName = "";
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
                // -----------------------------------------------------
                String body = "", pDesignation = "", pCompanyName = "";
                int intCnt = 1;
                pCompanyName = GetCompanyName().Trim();
                // -----------------------------------------------------
                if (!String.IsNullOrEmpty(pTemplateID) && !String.IsNullOrEmpty(pLoginUserID))
                {
                    int totrec = 0;

                    List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                    lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

                    OrganizationEmployeeSQL inst = new OrganizationEmployeeSQL();
                    List<Entity.OrganizationEmployee> lstEmp = new List<Entity.OrganizationEmployee>();
                    lstEmp = inst.GetOrganizationEmployeeList(objAuth.EmployeeID, 1, 10000, out totrec);

                    List<Entity.OrganizationEmployee> lstSuper = new List<Entity.OrganizationEmployee>();
                    lstSuper = inst.GetEmployeeSupervisorList(objAuth.UserID);

                    String strMultiEmail = GetConstant("MultiEmailID", 0, 1);
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        if (dtDetail != null)
                        {
                            string textBody = "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#4da6ff'><td><b>Inventory Item</b></td> <td> <b> Required Qunatity </b> </td></tr>";
                            textBody += "</table>";

                            if (dtDetail.Rows.Count > 0)
                            {
                                // -------------------------------------------------------------------------
                                // Section : Building Daily Activity
                                // -------------------------------------------------------------------------
                                body += "<h2>Daily Activity</h2>";
                                body += "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " style='width=100%;'>";
                                body += "<tr bgcolor='navy' color='white' style='font-size:14px;'>";
                                body += "<td style='width:10%; text-align:center;'><b>No</b></td>";
                                body += "<td style='width:70%'>TaskDescription</td>";
                                body += "<td style='width:20%; text-align:center;'>Duration Hrs</td>";
                                body += "</tr>";
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    if (dr.RowState.ToString() != "Deleted")
                                    {
                                        body += "<tr style='background-color=white; color=black; font-size:12px;'>";
                                        body += "<td style='width:10%; text-align:center;'><b>" + intCnt.ToString() + "</b></td>";
                                        body += "<td style='width:70%'>" + dr["TaskDescription"].ToString() + "</td>";
                                        body += "<td style='width:20%; text-align:center;'>" + dr["TaskDuration"].ToString() + " Hrs." + "</td>";
                                        body += "</tr>";
                                    }
                                    intCnt = intCnt + 1;
                                }
                                body += "</table>";

                                // -------------------------------------------------------------------------
                                // Section : Building Quotation
                                // -------------------------------------------------------------------------
                                Int64 xMonth = 0, xYear = 0;
                                xMonth = Convert.ToDateTime(dtDetail.Rows[0]["ActivityDate"].ToString()).Month;
                                xYear = Convert.ToDateTime(dtDetail.Rows[0]["ActivityDate"].ToString()).Year;

                                QuotationSQL objQuot = new QuotationSQL();
                                List<Entity.Quotation> lstQuot = new List<Entity.Quotation>();
                                lstQuot = objQuot.GetQuotationByUser(objAuth.UserID, 0, 0, Convert.ToDateTime(dtDetail.Rows[0]["ActivityDate"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dtDetail.Rows[0]["ActivityDate"]).ToString("yyyy-MM-dd"));
                                if (lstQuot.Count>0)
                                {
                                    body += "<h2>Quotation</h2>";
                                    body += "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " style='width=100%;'>";
                                    body += "<tr bgcolor='navy' color='white' style='font-size:14px;'>";
                                    body += "<td style='width:5%;text-align:center;'><b>Sr.No</b></td>";
                                    body += "<td style='width:25%;text-align:center;'>QuotationNo</td>";
                                    body += "<td style='width:50%'>CustomerName </td>";
                                    body += "<td style='width:20%;text-align:center;'>Quotation Amt</b></td>";
                                    body += "</tr>";
                                    for (int i = 0; i < lstQuot.Count; i++)
                                    {
                                        body += "<tr style='background-color=white; color=black; font-size:12px;'>";
                                        body += "<td style='width:5%;text-align:center;'><b>" + (i+1).ToString() + "</b></td>";
                                        body += "<td style='width:25%;text-align:center;'>" + lstQuot[i].QuotationNo + "</td>";
                                        body += "<td style='width:50%'>" + lstQuot[i].CustomerName + " Hours" + "</td>";
                                        body += "<td style='width:20%;text-align:center;'>" + lstQuot[i].NetAmt.ToString("#.##") + "</td>";
                                        body += "</tr>";
                                    }
                                    body += "</table>";
                                }

                                // -------------------------------------------------------------------------
                                // Section : Building ToDO Task
                                // -------------------------------------------------------------------------
                                ToDoSQL oblToDO = new ToDoSQL();
                                List<Entity.ToDo> lstToDO = new List<Entity.ToDo>();
                                lstToDO = oblToDO.GetToDoListByUser(objAuth.UserID, 0, xYear, "", "");
                                lstToDO = lstToDO.Where(it => (it.TaskStatus.ToLower() == "pending" || it.TaskStatus.ToLower() == "overdue")).ToList();
                                if (lstToDO.Count > 0)
                                {
                                    body += "<h2>Task Completed</h2>";
                                    body += "<table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " style='width=100%;'>";
                                    body += "<tr bgcolor='navy' color='white' style='font-size:14px;'>";
                                    body += "<td style='width:5%;text-align:center;'>Sr.No</td>";
                                    body += "<td style='width:15%'>Customer Name</td>";
                                    body += "<td style='width:35%'>Task Description</td>";
                                    body += "<td style='width:20%;text-align:center;'>Assignd From</td>";
                                    body += "<td style='width:10%;text-align:center;'>Start Date</td>";
                                    body += "<td style='width:10%;text-align:center;'>Due Date</td>";
                                    body += "</tr>";
                                    for (int i = 0; i < lstToDO.Count; i++)
                                    {
                                        body += "<tr style='background-color=white; color=black; font-size:12px;'>";
                                        body += "<td style='width:5%;text-align:center;'><b>" + (i + 1).ToString() + "</b></td>";
                                        body += "<td style='width:15%;'>" + lstToDO[i].CustomerName + "</td>";
                                        body += "<td style='width:35%;'>" + lstToDO[i].TaskDescription + "</td>";
                                        body += "<td style='width:20%;text-align:center;'>" + lstToDO[i].FromEmployeeName + "</td>";
                                        body += "<td style='width:10%;text-align:center;'>" + lstToDO[i].StartDate.ToString("dd-MMM-yyyy") + "</td>";
                                        body += "<td style='width:10%;text-align:center;'>" + lstToDO[i].DueDate.ToString("dd-MMM-yyyy") + "</td>";
                                        body += "</tr>";
                                    }
                                    body += "</table>";
                                }

                                // -------------------------------------------------------------------------
                                mailMessage.Subject = "Daily Activity Report - " + Convert.ToDateTime(dtDetail.Rows[0]["ActivityDate"]).ToString("dd-MMM-yyyy");
                                mailMessage.From = new MailAddress(lstEmp[0].EmailAddress);
                                mailMessage.Body = body;
                                mailMessage.IsBodyHtml = true;

                                for (int i = 0; i <= lstSuper.Count - 1; i++)
                                {
                                    if (lstEmp[0].EmailAddress.ToLower() != lstSuper[i].EmailAddress.ToLower())
                                    {
                                        if (i == 0)
                                            mailMessage.To.Add(new MailAddress(lstSuper[i].EmailAddress));
                                        else
                                            mailMessage.CC.Add(new MailAddress(lstSuper[i].EmailAddress));
                                    }
                                }
                                mailMessage.CC.Add(new MailAddress("mrunalyoddha@gmail.com"));
                                // -------------------------------------------------------------
                                try
                                {
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = lstCompany[0].Host; //  ConfigurationManager.AppSettings["Host"];
                                    if (!String.IsNullOrEmpty(lstCompany[0].EnableSSL.ToString().ToLower()))
                                        smtp.EnableSsl = lstCompany[0].EnableSSL;
                                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                    smtp.UseDefaultCredentials = false;
                                    NetworkCred.UserName = lstEmp[0].EmailAddress;      // ConfigurationManager.AppSettings["UserName"];
                                    NetworkCred.Password = lstEmp[0].EmailPassword;      // ConfigurationManager.AppSettings["Password"];
                                    smtp.Credentials = NetworkCred;
                                    smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                                    smtp.Send(mailMessage);
                                    strErr = "Success";
                                }
                                catch (Exception ex)
                                {
                                    string tmpMessage = "";
                                    tmpMessage = ex.Message.ToString();
                                    strErr = tmpMessage;
                                    throw;
                                }

                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                strErr = "Failed";
            }
            return strErr;
        }
        public virtual string SendEmailNotifcation(string pTemplateID, string pLoginUserID, Int64 pkID, string pEmailAddress)
        {
            try
            {
                string tmpQuotationNo = "", tmpQuotationNo1 = "", tmpInvoiceNo = "", tmpInvoiceNo1 = "", tmpOrderNo = "", tmpOrderNo1 = "", tmpPtName = "";

                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
                // -----------------------------------------------------
                String pEmployeeName = "", pDesignation = "", pCompanyName = "";
                pCompanyName = GetCompanyName().Trim();
                // -----------------------------------------------------
                if (!String.IsNullOrEmpty(pTemplateID) && !String.IsNullOrEmpty(pLoginUserID) && !String.IsNullOrEmpty(pEmailAddress))
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
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string body = string.Empty;
                            body = HttpUtility.HtmlDecode(GetTextVale(dr, "ContentData"));
                            // ------------------------------------------------------------
                            body = body.Replace("{CompanyName}", objAuth.CompanyName);
                            body = body.Replace("{EmployeeName}", objAuth.EmployeeName);
                            // ------------------------------------------------------------
                            string pSubject = GetTextVale(dr, "Subject");
                            string pTemplate = GetTextVale(dr, "TemplateID");
                            Boolean pActiveStatus = GetBoolean(dr, "ActiveFlag");
                            if (pActiveStatus == true)
                            {
                                // ------------------------------------------------------------------
                                if (!String.IsNullOrEmpty(pTemplate) && (pTemplate == "INQUIRY-WELCOME" || pTemplate == "QUOTATION" || pTemplate == "SALESBILL" || pTemplate == "PROFORMA" || pTemplate == "SALESORDER" || pTemplate == "PURCHASEORDER"))
                                {

                                    if (!String.IsNullOrEmpty(pTemplate) && pkID > 0)
                                    {
                                        SqlCommand cmdGet1 = new SqlCommand();
                                        cmdGet1.CommandType = CommandType.StoredProcedure;

                                        if (pTemplate == "INQUIRY-WELCOME")
                                            cmdGet1.CommandText = "InquiryList";
                                        else if (pTemplate == "QUOTATION")
                                            cmdGet1.CommandText = "QuotationList";
                                        else if (pTemplate == "SALESBILL")
                                            cmdGet1.CommandText = "SalesBillList";
                                        else if (pTemplate == "PROFORMA" || pTemplate == "SALESORDER")
                                            cmdGet1.CommandText = "SalesOrderList";
                                        else if (pTemplate == "PURCHASEORDER")
                                            cmdGet1.CommandText = "PurchaseOrderList";

                                        cmdGet1.Parameters.AddWithValue("@pkID", pkID);
                                        cmdGet1.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
                                        cmdGet1.Parameters.AddWithValue("@PageNo", 1);
                                        cmdGet1.Parameters.AddWithValue("@PageSize", 10);
                                        SqlParameter p1 = new SqlParameter("@TotalCount", SqlDbType.Int);
                                        p1.Direction = ParameterDirection.Output;
                                        cmdGet1.Parameters.Add(p1);
                                        SqlDataReader drTable = ExecuteDataReader(cmdGet1);
                                        drTable.Read();
                                        // ------------------------------------------------------------
                                        string tmpCreatedBy = GetTextVale(drTable, "CreatedBy");
                                        if (pTemplate == "QUOTATION")
                                        {
                                            tmpQuotationNo = GetTextVale(drTable, "QuotationNo");
                                            tmpQuotationNo1 = tmpQuotationNo;       //for image attachment
                                            tmpQuotationNo = tmpQuotationNo.Replace("/", "-");
                                        }
                                        else if (pTemplate == "SALESBILL")
                                        {
                                            tmpInvoiceNo = GetTextVale(drTable, "InvoiceNo");
                                            tmpInvoiceNo1 = tmpInvoiceNo;
                                            tmpInvoiceNo = tmpInvoiceNo.Replace("/", "-");
                                            tmpOrderNo = GetTextVale(drTable, "OrderNo");
                                            tmpPtName = GetTextVale(drTable, "PatientName");
                                        }
                                        else if (pTemplate == "PROFORMA" || pTemplate == "SALESORDER")
                                        {
                                            tmpOrderNo = GetTextVale(drTable, "OrderNo");
                                            tmpOrderNo1 = tmpOrderNo;
                                            tmpOrderNo = tmpOrderNo.Replace("/", "-");
                                            tmpPtName = GetTextVale(drTable, "PatientName");
                                        }
                                        else if (pTemplate == "INQUIRY-WELCOME")
                                        {
                                            tmpPtName = GetTextVale(drTable, "CustomerName");
                                        }
                                        else if (pTemplate == "PURCHASEORDER")
                                        {
                                            tmpOrderNo = GetTextVale(drTable, "OrderNo");
                                            tmpOrderNo1 = tmpOrderNo;
                                            tmpOrderNo = tmpOrderNo.Replace("/", "-");
                                            tmpPtName = GetTextVale(drTable, "PatientName");
                                        }

                                        body = body.Replace("{Patient Name}", tmpPtName);
                                        //body = body.Replace("{User Name}", lstCompany[0].UserName);
                                        body = body.Replace("{Invoice No}", tmpInvoiceNo);
                                        body = body.Replace("{Order No}", tmpOrderNo);
                                        body = body.Replace("{Proforma No}", tmpOrderNo);


                                        if (!String.IsNullOrEmpty(tmpCreatedBy))
                                        {
                                            String tmpAuthorizedSign = GetAuthorizedSignUserID(tmpCreatedBy);
                                            if (!String.IsNullOrEmpty(tmpAuthorizedSign))
                                            {
                                                body = body + "<br />";
                                                body = body + tmpAuthorizedSign;
                                            }
                                            else
                                            {
                                                // ===================================================================================================
                                                // Custom Email Format : Family & Friends 
                                                // ===================================================================================================
                                                if (!String.IsNullOrEmpty(pLoginUserID))
                                                {
                                                    pEmployeeName = GetEmployeeNameByUserID(pLoginUserID);
                                                    pDesignation = GetDesignationByUserID(pLoginUserID);
                                                    // --------------------------------------------------------
                                                    if (!String.IsNullOrEmpty(pEmployeeName))
                                                        body = body + "<b>" + pEmployeeName + "</b><br />";

                                                    if (!String.IsNullOrEmpty(pDesignation))
                                                        body = body + "<b>" + pDesignation + "</b><br />";
                                                }
                                                // ------------------------------------------------------------

                                                if (!String.IsNullOrEmpty(pCompanyName))
                                                    body = body + "<b>" + pCompanyName + "</b><br /><br />";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(pLoginUserID))
                                    {
                                        String tmpAuthorizedSign = GetAuthorizedSignUserID(pLoginUserID);
                                        if (!String.IsNullOrEmpty(tmpAuthorizedSign))
                                        {
                                            body = body + "<br />";
                                            body = body + tmpAuthorizedSign;
                                        }
                                        else
                                        {
                                            // ===================================================================================================
                                            // Custom Email Format : Family & Friends 
                                            // ===================================================================================================
                                            if (!String.IsNullOrEmpty(pLoginUserID))
                                            {
                                                pEmployeeName = GetEmployeeNameByUserID(pLoginUserID);
                                                pDesignation = GetDesignationByUserID(pLoginUserID);
                                                // --------------------------------------------------------
                                                body = body + "<br />";
                                                if (!String.IsNullOrEmpty(pEmployeeName))
                                                    body = body + "<b>" + pEmployeeName + "</b><br />";

                                                if (!String.IsNullOrEmpty(pDesignation))
                                                    body = body + "<b>" + pDesignation + "</b><br />";
                                            }
                                            // ------------------------------------------------------------

                                            if (!String.IsNullOrEmpty(pCompanyName))
                                                body = body + "<b>" + pCompanyName + "</b><br /><br />";
                                        }
                                    }
                                }
                                // ------------------------------------------------------------
                                string filepath1, filepath2;
                                LinkedResource Img1 = null;
                                LinkedResource Img2 = null;
                                AlternateView AV = null;

                                if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment1")))
                                {
                                    try
                                    {
                                        filepath1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\" + GetTextVale(dr, "ImageAttachment1"));
                                        Img1 = new LinkedResource(filepath1, MediaTypeNames.Image.Jpeg);
                                        Img1.ContentId = "MyBrochure1";
                                        body = body + "<img src=cid:MyBrochure1  id='img' alt='' width='900px' height='600px'/><br /><br />";
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment2")))
                                {
                                    try
                                    {
                                        filepath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\" + GetTextVale(dr, "ImageAttachment2"));
                                        Img2 = new LinkedResource(filepath2, MediaTypeNames.Image.Jpeg);
                                        Img2.ContentId = "MyBrochure2";
                                        body = body + "<img src=cid:MyBrochure2  id='img' alt='' width='900px' height='600px'/><br /><br />";
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }

                                AV = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                                try
                                {
                                    if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment1")))
                                        AV.LinkedResources.Add(Img1);

                                    if (!String.IsNullOrEmpty(GetTextVale(dr, "ImageAttachment2")))
                                        AV.LinkedResources.Add(Img2);
                                }
                                catch (Exception ex)
                                {
                                }
                                // ---------------------------------------------------------------------------------
                                int totrec = 0;

                                List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                                lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

                                List<Entity.OrganizationEmployee> lstEmp = new List<Entity.OrganizationEmployee>();
                                OrganizationEmployeeSQL inst = new OrganizationEmployeeSQL();
                                lstEmp = inst.GetOrganizationEmployeeList(objAuth.EmployeeID, 1, 10000, out totrec);

                                String strMultiEmail = GetConstant("MultiEmailID", 0, 1);

                                using (MailMessage mailMessage = new MailMessage())
                                {
                                    if (strMultiEmail.ToLower() == "yes" || strMultiEmail.ToLower() == "y")
                                        mailMessage.From = new MailAddress(lstEmp[0].EmailAddress);
                                    else
                                        mailMessage.From = new MailAddress(lstCompany[0].UserName);
                                    // ---------------------------------------------------------------
                                    if (pTemplate == "INQUIRY-WELCOME")
                                        mailMessage.Subject = pCompanyName + " - Inquiry " + (string.IsNullOrEmpty(tmpPtName) ? "" : " - " + tmpPtName);
                                    else if (pTemplate == "QUOTATION")
                                        mailMessage.Subject = pCompanyName + " - Quotation";
                                    else if (pTemplate == "SALESBILL")
                                        mailMessage.Subject = pCompanyName + " - Sales Invoice : " + tmpInvoiceNo + ((string.IsNullOrEmpty(tmpOrderNo) ? "" : " (Ref :" + tmpOrderNo + ") - ") + tmpPtName);
                                    else if (pTemplate == "PROFORMA")
                                        mailMessage.Subject = pCompanyName + " - Proforma Invoice : " + tmpOrderNo + " " + tmpPtName;
                                    else if (pTemplate == "SALESORDER")
                                        mailMessage.Subject = pCompanyName + " - Sales Order";
                                    else if (pTemplate == "PURCHASEORDER")
                                        mailMessage.Subject = pCompanyName + " - Purchase Order";
                                    else if (pTemplate == "PATIENT")
                                        mailMessage.Subject = "Welcome To " + pCompanyName;
                                    else
                                        mailMessage.Subject = (!String.IsNullOrEmpty(pSubject)) ? pSubject : "Greetings From " + pCompanyName;

                                    // ----------------------------------------------------------------


                                    mailMessage.Body = body;
                                    mailMessage.AlternateViews.Add(AV);
                                    mailMessage.IsBodyHtml = true;
                                    mailMessage.To.Add(new MailAddress(pEmailAddress));
                                    if (strMultiEmail.ToLower() == "yes" || strMultiEmail.ToLower() == "y")
                                        mailMessage.Bcc.Add(lstEmp[0].EmailAddress);
                                    else
                                        mailMessage.Bcc.Add(lstCompany[0].UserName);
                                    // -------------------------------------------------------------
                                    if (pTemplate == "QUOTATION")
                                    {
                                        if (String.IsNullOrEmpty(tmpQuotationNo))
                                        {
                                            tmpQuotationNo = "Quotation_" + pkID.ToString();
                                        }
                                        String pdfFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\" + tmpQuotationNo + ".pdf");
                                        Attachment data = new Attachment(pdfFile);
                                        ContentDisposition disposition = data.ContentDisposition;
                                        disposition.CreationDate = System.DateTime.Now;
                                        disposition.ModificationDate = System.DateTime.Now;
                                        disposition.DispositionType = DispositionTypeNames.Attachment;
                                        mailMessage.Attachments.Add(data);   // Attaching the file  
                                        // -------------------------------------------------------------                                
                                        // Attaching Product Broucher PDF 
                                        // -------------------------------------------------------------                                
                                        SqlCommand cmdGet1 = new SqlCommand();
                                        cmdGet1.CommandType = CommandType.StoredProcedure;
                                        cmdGet1.CommandText = "QuotationDetailListByQuotationNo";
                                        cmdGet1.Parameters.AddWithValue("@QuotationNo", tmpQuotationNo1);
                                        cmdGet1.Parameters.AddWithValue("@PageNo", 1);
                                        cmdGet1.Parameters.AddWithValue("@PageSize", 10);
                                        SqlParameter p1 = new SqlParameter("@TotalCount", SqlDbType.Int);
                                        p1.Direction = ParameterDirection.Output;
                                        cmdGet1.Parameters.Add(p1);
                                        SqlDataReader dr1 = ExecuteDataReader(cmdGet1);
                                        while (dr1.Read())
                                        {
                                            Int64 tmpProductID = GetInt64(dr1, "ProductID");
                                            // --------------------------------------------------------
                                            SqlCommand cmdGet2 = new SqlCommand();
                                            cmdGet2.CommandType = CommandType.StoredProcedure;
                                            cmdGet2.CommandText = "ProductDocumentsList";
                                            cmdGet2.Parameters.AddWithValue("@pkID", 0);
                                            cmdGet2.Parameters.AddWithValue("@ProductID", tmpProductID);
                                            SqlDataReader dr11 = ExecuteDataReader(cmdGet2);
                                            String tmpAttachment = "";
                                            if (dr11.HasRows)
                                            {
                                                while (dr11.Read())
                                                {
                                                    tmpAttachment = GetTextVale(dr11, "Name");
                                                    if (!String.IsNullOrEmpty(tmpAttachment))
                                                    {
                                                        try
                                                        {
                                                            String prodPDF = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "productimages\\" + tmpAttachment);
                                                            Attachment data1 = new Attachment(prodPDF);
                                                            ContentDisposition disposition1 = data1.ContentDisposition;
                                                            disposition1.CreationDate = System.DateTime.Now;
                                                            disposition1.ModificationDate = System.DateTime.Now;
                                                            disposition1.DispositionType = DispositionTypeNames.Attachment;
                                                            mailMessage.Attachments.Add(data1);   // Attaching the file  
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else if (pTemplate == "SALESBILL")
                                    {
                                        if (String.IsNullOrEmpty(tmpInvoiceNo))
                                        {
                                            tmpInvoiceNo = "Invoice_" + pkID.ToString();
                                        }
                                        String pdfFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\" + tmpInvoiceNo + ".pdf");
                                        Attachment data = new Attachment(pdfFile);
                                        ContentDisposition disposition = data.ContentDisposition;
                                        disposition.CreationDate = System.DateTime.Now;
                                        disposition.ModificationDate = System.DateTime.Now;
                                        disposition.DispositionType = DispositionTypeNames.Attachment;
                                        mailMessage.Attachments.Add(data);   // Attaching the file                                      
                                    }

                                    else if (pTemplate == "PROFORMA" || pTemplate == "SALESORDER")
                                    {
                                        if (String.IsNullOrEmpty(tmpOrderNo))
                                        {
                                            tmpOrderNo = "SO_" + pkID.ToString();
                                        }
                                        String pdfFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\" + tmpOrderNo + ".pdf");
                                        Attachment data = new Attachment(pdfFile);
                                        ContentDisposition disposition = data.ContentDisposition;
                                        disposition.CreationDate = System.DateTime.Now;
                                        disposition.ModificationDate = System.DateTime.Now;
                                        disposition.DispositionType = DispositionTypeNames.Attachment;
                                        mailMessage.Attachments.Add(data);   // Attaching the file                                      
                                    }
                                    else if (pTemplate == "PURCHASEORDER")
                                    {
                                        if (String.IsNullOrEmpty(tmpOrderNo))
                                        {
                                            tmpOrderNo = "PO_" + pkID.ToString();
                                        }
                                        String pdfFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\" + tmpOrderNo + ".pdf");
                                        Attachment data = new Attachment(pdfFile);
                                        ContentDisposition disposition = data.ContentDisposition;
                                        disposition.CreationDate = System.DateTime.Now;
                                        disposition.ModificationDate = System.DateTime.Now;
                                        disposition.DispositionType = DispositionTypeNames.Attachment;
                                        mailMessage.Attachments.Add(data);   // Attaching the file                                      
                                    }

                                    // -------------------------------------------------------------
                                    try
                                    {
                                        SmtpClient smtp = new SmtpClient();
                                        smtp.Host = lstCompany[0].Host; //  ConfigurationManager.AppSettings["Host"];
                                        if (!String.IsNullOrEmpty(lstCompany[0].EnableSSL.ToString().ToLower()))
                                            smtp.EnableSsl = lstCompany[0].EnableSSL;
                                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                                        smtp.UseDefaultCredentials = false;
                                        if (strMultiEmail.ToLower() == "yes" || strMultiEmail.ToLower() == "y")
                                        {
                                            NetworkCred.UserName = lstEmp[0].EmailAddress;      // ConfigurationManager.AppSettings["UserName"];
                                            NetworkCred.Password = lstEmp[0].EmailPassword;      // ConfigurationManager.AppSettings["Password"];
                                        }
                                        else
                                        {
                                            NetworkCred.UserName = lstCompany[0].UserName;      // ConfigurationManager.AppSettings["UserName"];
                                            NetworkCred.Password = lstCompany[0].Password;      // ConfigurationManager.AppSettings["Password"];
                                        }
                                        smtp.Credentials = NetworkCred;
                                        smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                                        smtp.Send(mailMessage);
                                        // ---------------------------------------------
                                        //SendEmailToIMAP(lstCompany[0].UserName, pEmailAddress, "Copy Of Email", body);
                                    }
                                    catch (Exception ex)
                                    {
                                        string tmpMessage = "";
                                        tmpMessage = ex.Message.ToString();
                                        Console.Write(tmpMessage);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return "Email Failed, Template Not Defined For Sales Bill !";
                    }
                }
                //return "Email Sent Successfully !";
                return "";
            }
            catch (WebException ex)
            {
                return "Sending Email Failed !";
            }
        }
        public string SendEmailToIMAP(string _FromMail, string _ToEmail, string _Subject, string _Body)
        {
            //IMail email = Limilabs.Mail.Fluent.Mail.Html(_Body).To(_ToEmail).From(_FromMail).Subject(_Subject).Create();
            // --------------------------------------------------------------------
            string respValue = "";
            //using (Imap imap = new Imap())
            //{
            //    imap.Connect("imap.yandex.com",993, true);  // or ConnectSSL for SSL/TLS
            //    imap.UseBestLogin("server@sharvayainfotech.com", "sharvaya@2020$");
                
            //    FolderInfo sent = new CommonFolders(imap.GetFolders()).Sent;
            //    imap.UploadMessage(sent, email);

            //    imap.Close();
            //}
            // ------------------------------------------
            return respValue;
        }
        public virtual string SendSMSNotifcation(string pTemplateID, string pLoginUserID, string pContactNo)
        {
            try
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
                // -----------------------------------------------------
                String pCompanyName = "";
                pCompanyName = GetCompanyName().Trim();
                // -----------------------------------------------------
                if (!String.IsNullOrEmpty(pTemplateID) && !String.IsNullOrEmpty(pLoginUserID) && !String.IsNullOrEmpty(pContactNo) && !String.IsNullOrEmpty(objAuth.SMS_Uri) && !String.IsNullOrEmpty(objAuth.SMS_AuthKey) && !String.IsNullOrEmpty(objAuth.SMS_SenderID))
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
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string body = string.Empty;
                            body = GetTextVale(dr, "ContentDataSMS");
                            // ------------------------------------------------------------
                            string retVal = SingleSmsSend_EWYDE(pContactNo, body);
                            return retVal;
                        }
                    }
                    else
                    {
                        return "SMS Template Not Found";
                    }
                }
                // -----------------------------------------------------------
                if (String.IsNullOrEmpty(objAuth.SMS_Uri) || String.IsNullOrEmpty(objAuth.SMS_AuthKey) || String.IsNullOrEmpty(objAuth.SMS_SenderID))
                {
                    return "SMS Configuration is Missing !";
                }
                else if (String.IsNullOrEmpty(pTemplateID))
                    return "Template Is Missing !";
                else 
                    return "";
            }
            catch (WebException ex)
            {
                return "Sending SMS Failed !";
            }
        }

        public virtual string SendMsg91Message(string pMobileNos, string pMessage)
        {
            // http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=YourAuthKey
            // &message=message&senderId=DEMOOS&routeId=1&mobileNos=9999999999,9999999999&smsContentType=english

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            // --------------------------------------------------------
            string xMessage = HttpUtility.UrlEncode(pMessage);
            // --------------------------------------------------------
            try
            {
                string sendSMSUri = "";
                sendSMSUri = objAuth.SMS_Uri;
                // http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=YourAuthKey&message=message&senderId=DEMOOS&routeId=1&mobileNos=9999999999&smsContentType=english

                sendSMSUri = "http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?";
                sendSMSUri += "AUTH_KEY=" + objAuth.SMS_AuthKey + "&";
                sendSMSUri += "message=" + xMessage + "&";
                sendSMSUri += "senderId=" + objAuth.SMS_SenderID + "&";
                sendSMSUri += "routeId=11" + "&";
                sendSMSUri += "mobileNos=" + pMobileNos + "&";
                sendSMSUri += "smsContentType=english";
                //System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
                //sbPostData.AppendFormat("AUTH_KEY={0}", objAuth.SMS_AuthKey);
                //sbPostData.AppendFormat("&message={0}", xMessage);
                //sbPostData.AppendFormat("&senderId={0}", objAuth.SMS_SenderID);
                //sbPostData.AppendFormat("&routeId={0}", "1");
                //sbPostData.AppendFormat("&mobileNos={0}", pMobileNos);
                //sbPostData.AppendFormat("&smsContentType={0}", "english");
                //// -------------------------------------------------------------------------
                //System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);       //Create HTTPWebrequest
                //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                //byte[] data = encoding.GetBytes(sbPostData.ToString());
                //httpWReq.Method = "GET";
                //httpWReq.ContentType = "application/x-www-form-urlencoded";
                //httpWReq.ContentLength = data.Length;
                //using (System.IO.Stream stream = httpWReq.GetRequestStream())
                //{
                //    stream.Write(data, 0, data.Length);
                //}
                //System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();   //Get the response
                //StreamReader reader = new StreamReader(response.GetResponseStream());
                //string responseString = reader.ReadToEnd();

                //reader.Close();
                //response.Close();

                //var client = new RestClient(sendSMSUri);
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("Cache-Control", "no-cache");
                //IRestResponse response = client.Execute(request);

                return "Success";
            }
            catch (SystemException ex)
            {
                return "Fail";
            }
        }

        private static string SingleSmsSend_EWYDE(string pMobileNos, string pMessage)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // --------------------------------------------------------
            string retVal = "", xMessage = HttpUtility.UrlEncode(pMessage);
            // --------------------------------------------------------
            // var client = new RestClient("http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=f955892331ff44ec298c0ad9746a8b7&message=HelloText&senderId=SIERPS&routeId=11&mobileNos=7878181860&smsContentType=english");

            try
            {
                string sendSMSUri = "";
                sendSMSUri = objAuth.SMS_Uri;
                // http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=YourAuthKey&message=message&senderId=DEMOOS&routeId=1&mobileNos=9999999999&smsContentType=english

                sendSMSUri = objAuth.SMS_Uri;
                sendSMSUri += "AUTH_KEY=" + objAuth.SMS_AuthKey + "&";
                sendSMSUri += "message=" + xMessage + "&";
                sendSMSUri += "senderId=" + objAuth.SMS_SenderID + "&";
                sendSMSUri += "routeId=11" + "&";
                sendSMSUri += "mobileNos=" + pMobileNos + "&";
                sendSMSUri += "smsContentType=english";
                // ------------------------------------------------------
                // var client = new RestClient("http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=f955892331ff44ec298c0ad9746a8b7&message=HelloText&senderId=SIERPS&routeId=2&mobileNos=9898621973&smsContentType=english");
                var client = new RestClient(sendSMSUri);
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cache-Control", "no-cache");
                IRestResponse response = client.Execute(request);

                retVal = "Success";
            }
            catch (Exception ex) {
                retVal = "Failed";
            }

            return retVal;
        }

        //private static void JsonSmsSend()
        //{
        //    var msgContent = new SmsRequestDto
        //    {
        //        //groupId = 0,
        //        routeId = 11, //Trans DND Other Route
        //        mobileNumbers = "7878181860",
        //        senderId = "SIERPS",
        //        smsContentType = "unicode",
        //        smsContent = "Hello this is test message"
        //    };
        //    var client = new RestClient("http://Loginsms.ewyde.com/rest/services/sendSMS/sendGroupSms?AUTH_KEY=f955892331ff44ec298c0ad9746a8b7");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("undefined", JsonConvert.SerializeObject(msgContent), ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //}

        public virtual string SendWhatsApp(string pTemplateID, string pLoginUserID, string pContactNo)
        {
            try
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
                // -----------------------------------------------------
                String pCompanyName = "";
                pCompanyName = GetCompanyName().Trim();
                // -----------------------------------------------------
                if (!String.IsNullOrEmpty(pTemplateID) && !String.IsNullOrEmpty(pLoginUserID) && !String.IsNullOrEmpty(pContactNo))
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
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string body = string.Empty;
                            body = GetTextVale(dr, "ContentDataSMS");
                            // ------------------------------------------------------------
                            body = body.Replace("{CompanyName}", objAuth.CompanyName);
                            // ------------------------------------------------------------
                            return SendWhatsAppMessage(pContactNo, body);
                        }
                    }
                    else
                    {
                        return "WhatsApp Template Not Found";
                    }
                }
                return "";
            }
            catch (WebException ex)
            {
                return "Sending SMS Failed !";
            }
        }

        public virtual string SendWhatsAppMessage(string pMobileNos, string pMessage)
        {
            string apiKey = "c324b7af0913472e8c1af433dfb1ab4c";
            string message = HttpUtility.UrlEncode(pMessage);
            // --------------------------------------------------------
            try
            {
                string sendSMSUri = "";
                sendSMSUri = "https://api.bulkwhatsapp.net/wapp/api/send?apikey=c324b7af0913472e8c1af433dfb1ab4c&mobile=[mobile]&msg=[msg]";
                sendSMSUri = sendSMSUri.Replace("[mobile]", pMobileNos);
                sendSMSUri = sendSMSUri.Replace("[msg]", pMessage);
                System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = Encoding.UTF8.GetBytes(pMessage);
                httpWReq.ContentLength = byteArray.Length;
                var dataStream = httpWReq.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                return "Success";
            }
            catch (SystemException ex)
            {
                return "Fail";
            }
        }
        public virtual string SendLeaveNotification(string pTemplateID, Entity.LeaveRequest objEntity)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // -----------------------------------------------------
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
            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);
            // ------------------------------------------------------------------
            while (dr.Read())
            {
                string body = string.Empty;
                body = GetTextVale(dr, "ContentData");
                // ===================================================================================================
                // Custom Email Format : Family & Friends 
                // ===================================================================================================
                String tmpEmployeeName = GetEmployeeNameByEmployeeID(objEntity.EmployeeID);
                body = body.Replace("{StartDate}", objEntity.FromDate.ToString("dd-MM-yyyy"));
                body = body.Replace("{EndDate}", objEntity.ToDate.ToString("dd-MM-yyyy"));
                body = body.Replace("{EmployeeName}", tmpEmployeeName);
                body = body.Replace("{ReasonForLeave}", objEntity.ReasonForLeave);
                // ---------------------------------------------------------------------------------
                string pEmailToHR = "", pHost = "", pUserName = "", pPassword = "";
                pEmailToHR = ConfigurationManager.AppSettings["HREmail"].ToString();
                pHost = lstCompany[0].Host;
                pUserName = lstCompany[0].UserName;
                pPassword = lstCompany[0].Password;

                if (lstCompany[0].chkLeaveRequest == "Yes" && !String.IsNullOrEmpty(pEmailToHR) && !String.IsNullOrEmpty(pHost) && !String.IsNullOrEmpty(pUserName) && !String.IsNullOrEmpty(pPassword))
                {
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(lstCompany[0].UserName);
                        mailMessage.Subject = "Leave Request - " + objEntity.EmployeeName;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.To.Add(new MailAddress(pEmailToHR));
                        // -------------------------------------------------------------
                        //string tmpVal = GetEmployeeEmailAddress(objAuth.UserID);
                        //if (!String.IsNullOrEmpty(tmpVal))
                        //    mailMessage.CC.Add(new MailAddress(tmpVal));

                        // -------------------------------------------------------------
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = pHost;
                        smtp.EnableSsl = Convert.ToBoolean(lstCompany[0].EnableSSL);
                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                        NetworkCred.UserName = lstCompany[0].UserName;
                        NetworkCred.Password = lstCompany[0].Password;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                        smtp.Send(mailMessage);
                    }
                }
            }
            return "Leave Request Sent Successfully !";
        }

        public virtual string SendDailyReportNotification(string pTemplateID, string ToEmailAddress, string pdfAttachment, string StartDate, string EndDate)
        {
            string pUserName = "", pPassword = "", pHost = "", body = "", returnMessage = "";
            Int16 pPortNumber = 0;
            Boolean pSSL = false;

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            String strMultiEmail = GetConstant("MultiEmailID", 0, 1);
            // -----------------------------------------------------
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


            // ---------------------------------------------------------------------------------
            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);
            pUserName = lstCompany[0].UserName;
            pPassword = lstCompany[0].Password;
            pHost = lstCompany[0].Host;
            pPortNumber = Convert.ToInt16(lstCompany[0].PortNumber);
            pSSL = lstCompany[0].EnableSSL;

            if (strMultiEmail.ToLower() == "yes" || strMultiEmail.ToLower() == "y")
            {
                int totrec;
                List<Entity.OrganizationEmployee> lstEmp = new List<Entity.OrganizationEmployee>();
                OrganizationEmployeeSQL inst = new OrganizationEmployeeSQL();
                lstEmp = inst.GetOrganizationEmployeeList(objAuth.EmployeeID, 1, 10000, out totrec);
                pUserName = lstEmp[0].EmailAddress;
                pPassword = lstEmp[0].EmailPassword;

            }
            // ---------------------------------------------------------------------------------
            if (dr.HasRows && !String.IsNullOrEmpty(pUserName) && !String.IsNullOrEmpty(pPassword) && !String.IsNullOrEmpty(ToEmailAddress))
            {
                while (dr.Read())
                {
                    body = HttpUtility.HtmlDecode(GetTextVale(dr, "ContentData"));
                    if (!String.IsNullOrEmpty(StartDate))
                        body = body.Replace("{StartDate}", Convert.ToDateTime(StartDate).ToString("dd-MM-yyyy"));
                    if (!String.IsNullOrEmpty(EndDate))
                        body = body.Replace("{EndDate}", Convert.ToDateTime(EndDate).ToString("dd-MM-yyyy"));
                    body = body.Replace("{EmployeeName}", objAuth.EmployeeName);

                    try
                    {
                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(pUserName);
                            mailMessage.Subject = "Daily Report : " + objAuth.EmployeeName;
                            mailMessage.Body = body;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.To.Add(new MailAddress(ToEmailAddress));
                            mailMessage.Bcc.Add(new MailAddress(pUserName));
                            // -------------------------------------------------------------
                            // Attachment 
                            // -------------------------------------------------------------
                            String pdfFile = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/") + pdfAttachment;
                            Attachment data = new Attachment(pdfFile);
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.DateTime.Now;
                            disposition.ModificationDate = System.DateTime.Now;
                            disposition.DispositionType = DispositionTypeNames.Attachment;
                            mailMessage.Attachments.Add(data);   // Attaching the file  
                            // -------------------------------------------------------------
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = pHost;
                            smtp.EnableSsl = Convert.ToBoolean(pSSL);
                            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                            NetworkCred.UserName = pUserName;
                            NetworkCred.Password = pPassword;
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = pPortNumber;
                            smtp.Send(mailMessage);
                        }
                        returnMessage = "Daily Report Email Sent Successfully !";
                    }
                    catch (Exception ex)
                    {
                        returnMessage = "Email Failed, Some Creditials Missing !";
                    }
                }
            }
            else
            {
                returnMessage = "Email Failed ! Daily Report Template/Email Setup Missing.";
            }
            return returnMessage;
        }

        public virtual string SendHRNotification(string pTemplateID, Int64 pEmployeeID)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            if (objAuth.RoleCode.ToLower() == "admin" || objAuth.RoleCode.ToLower() == "hr" || objAuth.RoleCode.ToLower() == "hradmin" || objAuth.RoleCode.ToLower() == "hrhead")
            {
                // -----------------------------------------------------
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
                List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);
                // ------------------------------------------------------------------
                int totrec;
                OrganizationEmployeeSQL inst = new OrganizationEmployeeSQL();
                List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
                lstEmployee = inst.GetOrganizationEmployeeList(pEmployeeID, 1, 10000, out totrec);
                // ------------------------------------------------------------------
                List<Entity.OrganizationEmployee> lstEmployeeHR = new List<Entity.OrganizationEmployee>();
                lstEmployeeHR = inst.GetOrganizationEmployeeList(objAuth.EmployeeID, 1, 10000, out totrec);
                // ------------------------------------------------------------------
                while (dr.Read())
                {
                    string body = string.Empty, subject = "";
                    body = GetTextVale(dr, "ContentData");
                    body = body.Replace("{CompanyName}", objAuth.CompanyName);
                    subject = GetTextVale(dr, "Subject");
                    // ===================================================================================================
                    // Custom Email Format : Family & Friends 
                    // ===================================================================================================
                    if (lstEmployee.Count > 0 && !String.IsNullOrEmpty(body))
                    {
                        body = body.Replace("{EmployeeName}", lstEmployee[0].EmployeeName);
                        body = body.Replace("{Designation}", lstEmployee[0].Designation);
                        body = body.Replace("{ConfirmationDate}", lstEmployee[0].ConfirmationDate.ToString("dd-MM-yyyy"));
                        body = body.Replace("{JoiningDate}", lstEmployee[0].JoiningDate.ToString("dd-MM-yyyy"));
                        body = body.Replace("{ReleaseDate}", lstEmployee[0].ReleaseDate.ToString("dd-MM-yyyy"));
                    }
                    // ---------------------------------------------------------------------------------
                    string pEmailToEmployee = "", pHost = "", pUserName = "", pPassword = "";
                    pEmailToEmployee = lstEmployee[0].EmailAddress;
                    pHost = lstCompany[0].Host;
                    pUserName = lstCompany[0].UserName;
                    pPassword = lstCompany[0].Password;

                    if (!String.IsNullOrEmpty(pEmailToEmployee) && !String.IsNullOrEmpty(pHost) && !String.IsNullOrEmpty(pUserName) && !String.IsNullOrEmpty(pPassword))
                    {
                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(lstCompany[0].UserName);
                            mailMessage.Subject = subject;
                            mailMessage.Body = body;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.To.Add(new MailAddress(pEmailToEmployee));
                            mailMessage.CC.Add(new MailAddress(lstCompany[0].UserName));
                            // -------------------------------------------------------------
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = pHost;
                            smtp.EnableSsl = Convert.ToBoolean(lstCompany[0].EnableSSL);
                            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                            NetworkCred.UserName = lstCompany[0].UserName;
                            NetworkCred.Password = lstCompany[0].Password;
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                            smtp.Send(mailMessage);
                        }
                    }
                }
                return "Email Notification Sent Successfully !";
            }
            else
                return "Sorry ! User with HR / HRADMIN / HRHEAD role can send notifications.";
        }

        public virtual string SendHospitalNotification(string pTemplateID, string pLoginUserID, string pHospitalName, string pPatientlName, string pSpeciality, string pEmailAddress, string pAppoinmentDate, string pInquiryNo)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // -----------------------------------------------------
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
            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);
            // ------------------------------------------------------------------

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string body = string.Empty;
                    body = GetTextVale(dr, "ContentData");
                    // ===================================================================================================
                    // Custom Email Format : Family & Friends 
                    // ===================================================================================================                
                    body = body.Replace("{Patient Name}", pPatientlName);
                    body = body.Replace("{Hospital Name}", pHospitalName);
                    body = body.Replace("{Speciality Name}", pSpeciality);
                    body = body.Replace("{User Name}", lstCompany[0].UserName);
                    body = body.Replace("{Appointment Date}", pAppoinmentDate);
                    body = body.Replace("{Inquiry No}", pInquiryNo);


                    // ---------------------------------------------------------------------------------
                    string pHost = "", pUserName = "", pPassword = "";
                    pHost = lstCompany[0].Host;
                    pUserName = lstCompany[0].UserName;
                    pPassword = lstCompany[0].Password;

                    if (!String.IsNullOrEmpty(pEmailAddress) && !String.IsNullOrEmpty(pHost) && !String.IsNullOrEmpty(pUserName) && !String.IsNullOrEmpty(pPassword))
                    {
                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = new MailAddress(lstCompany[0].UserName);
                            mailMessage.Subject = "Patient Information - " + pPatientlName;
                            mailMessage.Body = body;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.To.Add(new MailAddress(pEmailAddress));
                            // -------------------------------------------------------------
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = pHost;
                            smtp.EnableSsl = Convert.ToBoolean(lstCompany[0].EnableSSL);
                            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                            NetworkCred.UserName = lstCompany[0].UserName;
                            NetworkCred.Password = lstCompany[0].Password;
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                            smtp.Send(mailMessage);
                        }
                    }
                }
                return "Email Sent Successfully !";
            }
            else
            {
                return "Email Tamplate Not Found !";
            }
        }


        public virtual void SendNotification_Firebase(string pModuleName, string pnotificationMsg, string pLoginUserID, Int64 AssignedToEmployeeID)
        {

            //Send notification to firebase
            //String AuthenticationKey = "AAAAWgMbOBA:APA91bHcd7PYOlNcOWGVRYKCZeq2uSZc5alXe8wuG1Rh4j6GdVUv4kcCVAdnhfd5sTgMEmbfwqYfuArEEmrVWXZ3waezghLg5xwQZ__4VUgZop5F2eZP2DMHDRZPsoe01xl555-no_1-";
            //string senderId = "386599172112";
            //string TOKEN = "em1-fOeWTNOSB2oC39zc9Z:APA91bGAIFvB2yJUHk0Q3jB40kDYXsdNHh1obYZOZOBoSseAOuj0ZE7So7Jue6b2YyofFpU39EM-C4WDOvZFoJXIA9ln_zKjBjDKYfpTzDhdhpVDHPYoSD-sXU1pnkFLVJ2qvmgSkKhT";   ////Single Token

            //string[] Tokens = { "cAqd7lpLTgu5DJSK8QB2LB:APA91bGOdcraoyt5Z2O25hty5cRvvMaKYp20Qgh55cne9Beg8hAWn8lAmNuE8Eq0vp-tU6zRzNyxgPkTh73GO-PQ0UEQ8i4JjtKHXn9i_2DoFIszNqCx6AHtyvDp2UB0Hm6LiFYxRdQM",
            //                    "d67sAxmPRBaVPiBeJcqLe_:APA91bEcZYawqkU5wEXKZ1VquldElUYc-lLNsOLQ9R-rzXCRZPEp0oerpksG4Aj3tIpjjP3LefJrH80q9hHdLgsmCgg_qEUCAdSXWnFWa4MWYPJxjm4EgATwISEUjYhp7dmgCMOM2zza"};


            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            List<Entity.OrganizationEmployee> lstReceiver = new List<Entity.OrganizationEmployee>();
            lstReceiver = GetNotificationReceiverList(pModuleName, pLoginUserID, AssignedToEmployeeID);

            if (lstReceiver.Count > 0)
            {
                string[] Tokens = lstReceiver.Select(row => row.TokenNo).ToArray();
                String AuthenticationKey = ConfigurationManager.AppSettings["ServerKey"];
                string senderId = ConfigurationManager.AppSettings["SenderID"];

                try
                {
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data1 = new
                    {
                        //to = TOKEN,           //Single
                        registration_ids = Tokens,          //Multiple
                        data = new
                        {
                            priority = "high",
                            title = pModuleName,
                            body = pnotificationMsg,  
                            //show_in_foreground = true
                            // icon = "myicon"
                        }
                    };

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(data1);

                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", AuthenticationKey));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    string str = sResponseFromServer;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
            }
        }

        public virtual List<Entity.OrganizationEmployee> GetNotificationReceiverList(string pModuleName, string LoginUserID, Int64 AssignedToEmployeeID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MST_NotificationReceiverList";
            cmdGet.Parameters.AddWithValue("@ModuleName", pModuleName);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@AssignedToEmployeeID", AssignedToEmployeeID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.OrganizationEmployee> lstLocation = new List<Entity.OrganizationEmployee>();
            while (dr.Read())
            {
                Entity.OrganizationEmployee objLocation = new Entity.OrganizationEmployee();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.EmployeeName = GetTextVale(dr, "EmployeeName");
                objLocation.TokenNo = GetTextVale(dr, "TokenNo");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }
        public virtual void SendNotificationToDB(string ModuleName, Int64 ModulePkID, string notificationMsg, string LoginUserID, Int64 AssignedToEmployeeID)
        //public virtual void SendNotificationToDB(string ModuleName, string notificationMsg, string LoginUserID, Int64 AssignedToEmployeeID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Mst_Notification_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@ModuleName", ModuleName);
            cmdAdd.Parameters.AddWithValue("@ModulePkID", ModulePkID);
            cmdAdd.Parameters.AddWithValue("@Description", notificationMsg);
            cmdAdd.Parameters.AddWithValue("@AssignedToEmployeeID", AssignedToEmployeeID);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ForceCloseConncetion();
        }

        public virtual string SendFeedback(string pTemplateID, string pCustomerEmailID)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // -----------------------------------------------------
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
            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);
            // ------------------------------------------------------------------
            while (dr.Read())
            {
                string body = string.Empty;
                body = GetTextVale(dr, "ContentData");
                // ===================================================================================================
                // Custom Email Format : Family & Friends 
                // ===================================================================================================
                //body = body + "<br /><br />" + "Please click below link to check incident location.<br />";
                //body = body + "<a href='https://www.google.co.in/maps/place/{Latitude}+{Longitude}/@{Latitude},{Longitude},17z'>https://www.google.co.in/maps/place/{Latitude}+{Longitude}/@{Latitude},{Longitude},17z</a>";
                //body = body + "<br /><br /><b>Best Wishes</b><br /><br />I-LAB informatics Pvt Ltd.";
                // ---------------------------------------------------------------------------------
                string pEmailTo = "", pHost = "", pUserName = "", pPassword = "";
                pEmailTo = pCustomerEmailID;
                pHost = lstCompany[0].Host;
                pUserName = lstCompany[0].UserName;
                pPassword = lstCompany[0].Password;

                if (!String.IsNullOrEmpty(pEmailTo) && !String.IsNullOrEmpty(pHost) && !String.IsNullOrEmpty(pUserName) && !String.IsNullOrEmpty(pPassword))
                {
                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(lstCompany[0].UserName);
                        mailMessage.Subject = GetTextVale(dr, "Subject");
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;
                        //mailMessage.Attachments = new Attachment(Convert.ToBase64String(img , "MemberPhotoID");
                        mailMessage.To.Add(new MailAddress(pEmailTo));
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = pHost;
                        smtp.EnableSsl = lstCompany[0].EnableSSL;
                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                        NetworkCred.UserName = lstCompany[0].UserName;
                        NetworkCred.Password = lstCompany[0].Password;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                        smtp.Send(mailMessage);
                    }
                }
            }
            return "Leave Request Sent Successfully !";
        }

        public Attachment AttachPDF(string pFileName)
        {
            String prodPDF = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "productimages\\" + pFileName);
            Attachment retAttachment = new Attachment(prodPDF);
            ContentDisposition cntDisposition = retAttachment.ContentDisposition;
            cntDisposition.CreationDate = System.DateTime.Now;
            cntDisposition.ModificationDate = System.DateTime.Now;
            cntDisposition.DispositionType = DispositionTypeNames.Attachment;
            return retAttachment;
        }

        #region Transaction_Common_Functions
        //////////////////////////////////////////////////////////////////////////////////
        //******************** Transaction Common functions ***************************
        //////////////////////////////////////////////////////////////////////////////////      
        public virtual string checkGSTNO(string pCustomerID)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT GSTNO From MST_Customer Where CustomerID = " + pCustomerID;
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult.Trim();
        }
        public virtual Boolean isIGST(string CustomerStateId, string CompanyStateId)
        {
            if (string.Equals(CustomerStateId, CompanyStateId))
                return false;
            else
                return true;
        }

        public virtual void funCalculate(Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, decimal HdDiscAmt, string CustomerStateId, string CompanyStateId, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        {
            //Out
            TaxAmt = 0;
            CGSTPer = 0; CGSTAmt = 0;
            SGSTPer = 0; SGSTAmt = 0;
            IGSTPer = 0; IGSTAmt = 0;
            NetRate = 0; BasicAmt = 0; NetAmt = 0; ItmDiscPer1 = 0; ItmDiscAmt1 = 0; AddTaxAmt = 0;

            bool isIGSTItem = isIGST(CustomerStateId, CompanyStateId);

            decimal BasicVal = 0, GSTAmt = 0;
            //In
            //TaxType = 0; Qty = 2; Rate = 100; ItmDiscPer = 5; ItmDiscAmt = 0; TaxPer = 5; AddTaxPer = 5; isIGST = false;HeadDiscAmt=0;

            if (Rate > 0)
            {
                if (ItmDiscPer > 0)
                {
                    ItmDiscAmt = ((ItmDiscPer * Rate) / 100);
                }
                else
                {
                    ItmDiscAmt = 0;
                }
                //else if (ItmDiscPer == 0 && ItmDiscAmt > 0)
                //{
                //    ItmDiscPer = Math.Round((ItmDiscAmt * 100) / Rate, 2);
                //}

                NetRate = Math.Round(Rate - ItmDiscAmt, 2);
                ItmDiscPer1 = ItmDiscPer;
                ItmDiscAmt1 = ItmDiscAmt;
                BasicVal = Math.Round(Qty * NetRate, 2);

                if (TaxType == 0)
                {
                    NetAmt = BasicVal;
                    BasicVal = BasicVal - HdDiscAmt;
                    decimal taxamt1 = Math.Round((((TaxPer + AddTaxPer) * BasicVal) / (100 + TaxPer + AddTaxPer)), 2);
                    BasicAmt = Math.Round(BasicVal - taxamt1, 2);
                    GSTAmt = Math.Round(Math.Round((((BasicAmt) * TaxPer) / 100), 2) / 2, 2) * 2;    //To set round of difference while sgst+CGST
                    AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                    BasicAmt = Math.Round(BasicVal - (AddTaxAmt + GSTAmt), 2);
                }
                else if (TaxType == 1)
                {
                    BasicAmt = BasicVal - HdDiscAmt;
                    GSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);
                    AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                    NetAmt = Math.Round(BasicVal + GSTAmt + AddTaxAmt, 2);
                }
                else
                {
                    BasicAmt = BasicVal;
                    NetAmt = BasicVal;
                }

                if (isIGSTItem)
                {
                    IGSTPer = TaxPer;
                    IGSTAmt = GSTAmt;
                }
                else
                {
                    CGSTPer = Math.Round(TaxPer / 2, 2);
                    SGSTPer = Math.Round(TaxPer / 2, 2);
                    CGSTAmt = Math.Round(GSTAmt / 2, 2);
                    SGSTAmt = Math.Round(GSTAmt / 2, 2);
                }
            }
        }

        // =================================================================
        // Unit Quantity Concept For SteelMan
        // =================================================================
        public virtual void funCalculateSteelNew(decimal UnitQuantity, Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, decimal HdDiscAmt, string CustomerStateId, string CompanyStateId, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        {
            //Out
            TaxAmt = 0;
            CGSTPer = 0; CGSTAmt = 0;
            SGSTPer = 0; SGSTAmt = 0;
            IGSTPer = 0; IGSTAmt = 0;
            NetRate = 0; BasicAmt = 0; NetAmt = 0; ItmDiscPer1 = 0; ItmDiscAmt1 = 0; AddTaxAmt = 0;

            bool isIGSTItem = isIGST(CustomerStateId, CompanyStateId);

            decimal BasicVal = 0, GSTAmt = 0;
            //In
            //TaxType = 0; Qty = 2; Rate = 100; ItmDiscPer = 5; ItmDiscAmt = 0; TaxPer = 5; AddTaxPer = 5; isIGST = false;HeadDiscAmt=0;

            if (Rate > 0)
            {

                ItmDiscAmt = (ItmDiscPer > 0) ? ((ItmDiscPer * Rate) / 100) : 0;
                NetRate = Math.Round(Rate - ItmDiscAmt, 2);
                ItmDiscPer1 = ItmDiscPer;
                ItmDiscAmt1 = ItmDiscAmt;
                BasicVal = Math.Round((Qty * UnitQuantity) * Rate, 2);       // 1000 * 1 = 1000


                if (TaxType == 0)           // Tax Type : Inclusive 
                {
                    decimal firstNetRate = Math.Round((NetRate * 100) / (100 + TaxPer), 2);     // 847.46
                    
                    ItmDiscAmt = (ItmDiscPer > 0) ? RoundDown(((ItmDiscPer * firstNetRate) / 100),2) : 0;   // 84.74
                    ItmDiscPer1 = ItmDiscPer;
                    ItmDiscAmt1 = ItmDiscAmt;   // 84.74
                    decimal aftDisNetRate = Math.Round(firstNetRate - ItmDiscAmt, 2);         // (847.46-84.74) = 762.72
                    NetRate = aftDisNetRate - HdDiscAmt;             // IF 100 Then (762.72-100 = 662.72)

                    BasicVal = Math.Round((Qty * UnitQuantity) * NetRate, 2);
                    BasicAmt = BasicVal;
                    decimal taxamt1 = Math.Round((((TaxPer + AddTaxPer) * BasicAmt) / 100), 2);
                    //GSTAmt = Math.Round(Math.Round(taxamt1, 2) / 2, 2) * 2;    //To set round of difference while sgst+CGST
                    GSTAmt = RoundDown(taxamt1, 2);
                    AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                    //BasicAmt = Math.Round(BasicVal - (AddTaxAmt + GSTAmt), 2);

                    //BasicVal = BasicVal - HdDiscAmt;
                    //decimal taxamt1 = Math.Round((((TaxPer + AddTaxPer) * BasicVal) / (100 + TaxPer + AddTaxPer)), 2);
                    //BasicAmt = Math.Round(BasicVal - taxamt1, 2);

                    //GSTAmt = Math.Round(Math.Round((((BasicAmt) * TaxPer) / 100), 2) / 2, 2) * 2;    //To set round of difference while sgst+CGST
                    //AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                    //BasicAmt = Math.Round(BasicVal - (AddTaxAmt + GSTAmt), 2);

                    if (isIGSTItem)
                    {
                        IGSTPer = TaxPer;
                        IGSTAmt = GSTAmt;
                    }
                    else
                    { 
                        CGSTPer = Math.Round(TaxPer / 2, 2);
                        SGSTPer = Math.Round(TaxPer / 2, 2);
                        CGSTAmt = Math.Round(GSTAmt / 2, 2);
                        SGSTAmt = Math.Round(GSTAmt / 2, 2);
                    }
                }
                else if (TaxType == 1)      // Tax Type : Exclusive 
                {
                    BasicAmt = BasicVal - HdDiscAmt;
                    GSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);

                    if (isIGSTItem)
                    {
                        IGSTPer = TaxPer;
                        IGSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);
                    }
                    else
                    {
                        CGSTPer = Math.Round(TaxPer / 2, 2);
                        SGSTPer = Math.Round(TaxPer / 2, 2);

                        if (HttpContext.Current.Session["SerialKey"].ToString() == "SA98-6HY9-HU67-LORF")    // For ShaktiPet
                        {
                            CGSTAmt = Math.Round(((BasicAmt * CGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                            SGSTAmt = Math.Round(((BasicAmt * SGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                        }
                        else
                        {              // For All Others
                            CGSTAmt = Math.Round(GSTAmt / 2, 2);
                            SGSTAmt = Math.Round(GSTAmt / 2, 2);
                        }
                    }

                    GSTAmt = (SGSTAmt + CGSTAmt + IGSTAmt);
                    //TaxAmt = GSTAmt; 
                }
                else
                {
                    BasicAmt = BasicVal;
                    NetAmt = BasicVal;
                }
                // ------------------------------------------------
                //if (isIGSTItem)
                //{
                //    IGSTPer = TaxPer;
                //    IGSTAmt = GSTAmt;
                //}
                //else
                //{
                //    CGSTPer = Math.Round(TaxPer / 2, 2);
                //    SGSTPer = Math.Round(TaxPer / 2, 2);
                //    if (HttpContext.Current.Session["SerialKey"].ToString() == "SA98-6HY9-HU67-LORF")    // For ShaktiPet
                //    {
                //        CGSTAmt = Math.Round(((BasicAmt * CGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                //        SGSTAmt = Math.Round(((BasicAmt * SGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                //    }
                //    else {              // For All Others
                //            CGSTAmt = Math.Round(GSTAmt / 2, 2);
                //            SGSTAmt = Math.Round(GSTAmt / 2, 2);
                //    }
                //}
                TaxAmt = GSTAmt;
                AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                // NetAmt = Math.Round(BasicVal + GSTAmt + AddTaxAmt, 2);
                NetAmt = Math.Round(BasicAmt + GSTAmt, 2);

            }
        }

        public decimal RoundDown(decimal i, double decimalPlaces)
        {
            var power = Convert.ToDecimal(Math.Pow(10, decimalPlaces));
            return Math.Floor(i * power) / power;
        }
        public virtual void funCalculateSteel(decimal UnitQuantity, Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, decimal HdDiscAmt, string CustomerStateId, string CompanyStateId, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        {
            //Out
            TaxAmt = 0;
            CGSTPer = 0; CGSTAmt = 0;
            SGSTPer = 0; SGSTAmt = 0;
            IGSTPer = 0; IGSTAmt = 0;
            NetRate = 0; BasicAmt = 0; NetAmt = 0; ItmDiscPer1 = 0; ItmDiscAmt1 = 0; AddTaxAmt = 0;

            bool isIGSTItem = isIGST(CustomerStateId, CompanyStateId);

            decimal BasicVal = 0, GSTAmt = 0;
            //In
            //TaxType = 0; Qty = 2; Rate = 100; ItmDiscPer = 5; ItmDiscAmt = 0; TaxPer = 5; AddTaxPer = 5; isIGST = false;HeadDiscAmt=0;

            if (Rate > 0)
            {
                if (ItmDiscPer > 0)
                {
                    ItmDiscAmt = ((ItmDiscPer * Rate) / 100);
                }
                else
                {
                    ItmDiscAmt = 0;
                }
                //else if (ItmDiscPer == 0 && ItmDiscAmt > 0)
                //{
                //    ItmDiscPer = Math.Round((ItmDiscAmt * 100) / Rate, 2);
                //}

                NetRate = Math.Round(Rate - ItmDiscAmt, 2);
                ItmDiscPer1 = ItmDiscPer;
                ItmDiscAmt1 = ItmDiscAmt;
                BasicVal = Math.Round((Qty * UnitQuantity) * NetRate, 2);


                if (TaxType == 0)
                {
                    NetAmt = BasicVal;
                    BasicVal = BasicVal - HdDiscAmt;
                    decimal taxamt1 = Math.Round((((TaxPer + AddTaxPer) * BasicVal) / (100 + TaxPer + AddTaxPer)), 2);
                    BasicAmt = Math.Round(BasicVal - taxamt1, 2);
                    GSTAmt = Math.Round(Math.Round((((BasicAmt) * TaxPer) / 100), 2) / 2, 2) * 2;    //To set round of difference while sgst+CGST
                    AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                    BasicAmt = Math.Round(BasicVal - (AddTaxAmt + GSTAmt), 2);

                    if (isIGSTItem)
                    {
                        IGSTPer = TaxPer;
                        IGSTAmt = GSTAmt;
                    }
                    else
                    { 
                        CGSTPer = Math.Round(TaxPer / 2, 2);
                        SGSTPer = Math.Round(TaxPer / 2, 2);
                        CGSTAmt = Math.Round(GSTAmt / 2, 2);
                        SGSTAmt = Math.Round(GSTAmt / 2, 2);
                    }
                }
                else if (TaxType == 1)
                {
                    BasicAmt = BasicVal - HdDiscAmt;
                    GSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);

                    if (isIGSTItem)
                    {
                        IGSTPer = TaxPer;
                        IGSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);
                    }
                    else
                    {
                        CGSTPer = Math.Round(TaxPer / 2, 2);
                        SGSTPer = Math.Round(TaxPer / 2, 2);

                        if (HttpContext.Current.Session["SerialKey"].ToString() == "SA98-6HY9-HU67-LORF")    // For ShaktiPet
                        {
                            CGSTAmt = Math.Round(((BasicAmt * CGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                            SGSTAmt = Math.Round(((BasicAmt * SGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                        }
                        else
                        {              // For All Others
                            CGSTAmt = Math.Round(GSTAmt / 2, 2);
                            SGSTAmt = Math.Round(GSTAmt / 2, 2);
                        }
                    }

                    GSTAmt = (SGSTAmt + CGSTAmt + IGSTAmt);
                    //TaxAmt = GSTAmt; 
                }
                else
                {
                    BasicAmt = BasicVal;
                    NetAmt = BasicVal;
                }
                // ------------------------------------------------
                //if (isIGSTItem)
                //{
                //    IGSTPer = TaxPer;
                //    IGSTAmt = GSTAmt;
                //}
                //else
                //{
                //    CGSTPer = Math.Round(TaxPer / 2, 2);
                //    SGSTPer = Math.Round(TaxPer / 2, 2);
                //    if (HttpContext.Current.Session["SerialKey"].ToString() == "SA98-6HY9-HU67-LORF")    // For ShaktiPet
                //    {
                //        CGSTAmt = Math.Round(((BasicAmt * CGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                //        SGSTAmt = Math.Round(((BasicAmt * SGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
                //    }
                //    else {              // For All Others
                //            CGSTAmt = Math.Round(GSTAmt / 2, 2);
                //            SGSTAmt = Math.Round(GSTAmt / 2, 2);
                //    }
                //}
                TaxAmt = GSTAmt;
                AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
                NetAmt = Math.Round(BasicVal + GSTAmt + AddTaxAmt, 2);

            }
        }


        //public virtual void funCalculateSteel(decimal UnitQuantity, Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, decimal HdDiscAmt, string CustomerStateId, string CompanyStateId, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        //{
        //    //Out
        //    TaxAmt = 0;
        //    CGSTPer = 0; CGSTAmt = 0;
        //    SGSTPer = 0; SGSTAmt = 0;
        //    IGSTPer = 0; IGSTAmt = 0;
        //    NetRate = 0; BasicAmt = 0; NetAmt = 0; ItmDiscPer1 = 0; ItmDiscAmt1 = 0; AddTaxAmt = 0;

        //    bool isIGSTItem = isIGST(CustomerStateId, CompanyStateId);

        //    decimal BasicVal = 0, GSTAmt = 0;
        //    //In
        //    //TaxType = 0; Qty = 2; Rate = 100; ItmDiscPer = 5; ItmDiscAmt = 0; TaxPer = 5; AddTaxPer = 5; isIGST = false;HeadDiscAmt=0;

        //    if (Rate > 0)
        //    {
        //        if (ItmDiscPer > 0)
        //        {
        //            ItmDiscAmt = ((ItmDiscPer * Rate) / 100);
        //        }
        //        else
        //        {
        //            ItmDiscAmt = 0;
        //        }
        //        //else if (ItmDiscPer == 0 && ItmDiscAmt > 0)
        //        //{
        //        //    ItmDiscPer = Math.Round((ItmDiscAmt * 100) / Rate, 2);
        //        //}

        //        NetRate = Math.Round(Rate - ItmDiscAmt, 2);
        //        ItmDiscPer1 = ItmDiscPer;
        //        ItmDiscAmt1 = ItmDiscAmt;
        //        BasicVal = Math.Round((Qty * UnitQuantity) * NetRate, 2);


        //        if (TaxType == 0)
        //        {
        //            NetAmt = BasicVal;
        //            BasicVal = BasicVal - HdDiscAmt;
        //            decimal taxamt1 = Math.Round((((TaxPer + AddTaxPer) * BasicVal) / (100 + TaxPer + AddTaxPer)), 2);
        //            BasicAmt = Math.Round(BasicVal - taxamt1, 2);
        //            GSTAmt = Math.Round(Math.Round((((BasicAmt) * TaxPer) / 100), 2) / 2, 2) * 2;    //To set round of difference while sgst+CGST
        //            AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
        //            BasicAmt = Math.Round(BasicVal - (AddTaxAmt + GSTAmt), 2);
        //        }
        //        else if (TaxType == 1)
        //        {
        //            BasicAmt = BasicVal - HdDiscAmt;

        //            if (HttpContext.Current.Session["SerialKey"].ToString() == "SA98-6HY9-HU67-LORF")    // ShaktiPet
        //            {
        //                decimal part1, part2;
        //                part1 = TruncateDecimal(((BasicAmt * (TaxPer / 2)) / 100), 2);
        //                part2 = TruncateDecimal(((BasicAmt * (TaxPer / 2)) / 100), 2);
        //                GSTAmt = Math.Round(part1 + part2, 2);
        //                //GSTAmt = TruncateDecimal(((BasicAmt * TaxPer) / 100), 2);
        //            }
        //            else
        //            {      // For All Others
        //                GSTAmt = Math.Round(((BasicAmt * TaxPer) / 100), 2);
        //            }
        //            AddTaxAmt = Math.Round((((BasicAmt) * AddTaxPer) / 100), 2);
        //            NetAmt = Math.Round(BasicVal + GSTAmt + AddTaxAmt, 2);
        //        }
        //        else
        //        {
        //            BasicAmt = BasicVal;
        //            NetAmt = BasicVal;
        //        }
        //        // ------------------------------------------------
        //        if (isIGSTItem)
        //        {
        //            IGSTPer = TaxPer;
        //            IGSTAmt = GSTAmt;
        //        }
        //        else
        //        {
        //            CGSTPer = Math.Round(TaxPer / 2, 2);
        //            SGSTPer = Math.Round(TaxPer / 2, 2);
        //            if (HttpContext.Current.Session["SerialKey"].ToString() == "SA98-6HY9-HU67-LORF")    // For ShaktiPet
        //            {
        //                CGSTAmt = Math.Round(((BasicAmt * CGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
        //                SGSTAmt = Math.Round(((BasicAmt * SGSTPer) / 100), 2);   // Math.Round(GSTAmt / 2, 2);
        //            }
        //            else
        //            {              // For All Others
        //                CGSTAmt = Math.Round(GSTAmt / 2, 2);
        //                SGSTAmt = Math.Round(GSTAmt / 2, 2);
        //            }
        //        }
        //        TaxAmt = GSTAmt;
        //    }
        //}



        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            return tmp / step;
        }
        public virtual void funCalOthChrgGST(decimal txtOthChrgAmt, bool OthChrgBeforeGST, decimal OthChrgGST, int taxtype, out decimal OthChargGSTAmt, out decimal othchargBasicAmt)
        {
            OthChargGSTAmt = 0;
            othchargBasicAmt = 0;

            if (Convert.ToBoolean(OthChrgBeforeGST) == true)
            {
                if (taxtype == 0)
                {
                    OthChargGSTAmt = Math.Round((txtOthChrgAmt * OthChrgGST) / (100 + OthChrgGST), 2);
                    othchargBasicAmt = txtOthChrgAmt - OthChargGSTAmt;
                }
                else if (taxtype == 1)
                {
                    OthChargGSTAmt = Math.Round((txtOthChrgAmt * OthChrgGST) / 100, 2);
                    othchargBasicAmt = txtOthChrgAmt;
                }
                else
                {
                    othchargBasicAmt = txtOthChrgAmt;
                    OthChargGSTAmt = 0;
                }
            }
            else
            {
                othchargBasicAmt = txtOthChrgAmt;
                OthChargGSTAmt = 0;
            }
        }

        public virtual DataTable funOnChangeTermination(DataTable dtDetail, string CustomerStateId, string CompanyStateId)
        {
            //DataTable dtDetail = new DataTable();
            //dtDetail = (DataTable)Session["dtDetail"];

            if (dtDetail != null)
            {
                foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                foreach (DataRow row in dtDetail.Rows)
                {
                    if (isIGST(CustomerStateId, CompanyStateId))
                    {
                        row.SetField("IGSTPer", Convert.ToDecimal(row["CGSTPer"]) + Convert.ToDecimal(row["SGSTPer"]) + Convert.ToDecimal(row["IGSTPer"]));
                        row.SetField("SGSTPer", 0);
                        row.SetField("CGSTPer", 0);
                        //row.SetField("IGSTAmt", Convert.ToDecimal(row["CGSTAmt"]) + Convert.ToDecimal(row["SGSTAmt"]) + Convert.ToDecimal(row["IGSTAmt"]));
                        row.SetField("IGSTAmt", Math.Round((Convert.ToDecimal(row["IGSTPer"]) * Convert.ToDecimal(row["Amount"])) / 100, 2));

                        row.SetField("SGSTAmt", 0);
                        row.SetField("CGSTAmt", 0);

                        row.SetField("TaxAmount", Convert.ToDecimal(row["SGSTAmt"]) + Convert.ToDecimal(row["CGSTAmt"]) + Convert.ToDecimal(row["IGSTAmt"]));
                        row.SetField("NetAmount", Convert.ToDecimal(row["Amount"]) + Convert.ToDecimal(row["TaxAmount"]) + Convert.ToDecimal(row["HeaderDiscAmt"]));
                        row.SetField("NetAmt", Convert.ToDecimal(row["NetAmount"]));
                    }
                    else
                    {
                        row.SetField("CGSTPer", (Convert.ToDecimal(row["CGSTPer"]) + Convert.ToDecimal(row["SGSTPer"]) + Convert.ToDecimal(row["IGSTPer"])) / 2);
                        row.SetField("SGSTPer", Convert.ToDecimal(row["CGSTPer"]));
                        row.SetField("IGSTPer", 0);
                        //row.SetField("CGSTAmt", Math.Round((Convert.ToDecimal(row["CGSTAmt"]) + Convert.ToDecimal(row["SGSTAmt"]) + Convert.ToDecimal(row["IGSTAmt"])) / 2, 2));
                        row.SetField("CGSTAmt", Math.Round((Convert.ToDecimal(row["CGSTPer"]) * Convert.ToDecimal(row["Amount"])) / 100, 2));
                        row.SetField("SGSTAmt", Convert.ToDecimal(row["CGSTAmt"]));
                        row.SetField("IGSTAmt", 0);
                        row.SetField("TaxAmount", Convert.ToDecimal(row["SGSTAmt"]) + Convert.ToDecimal(row["CGSTAmt"]) + Convert.ToDecimal(row["IGSTAmt"]));
                        row.SetField("NetAmount", Convert.ToDecimal(row["Amount"]) + Convert.ToDecimal(row["TaxAmount"]) + Convert.ToDecimal(row["HeaderDiscAmt"]));
                        row.SetField("NetAmt", Convert.ToDecimal(row["NetAmount"]));
                    }
                }
            }
            return dtDetail;
            //rptQuotationDetail.DataSource = dtDetail;
            //rptQuotationDetail.DataBind();

            //Session.Add("dtDetail", dtDetail);
        }

        public virtual void funOthChrgTextChange(Int64 OthChrgId, decimal txtOthChrgAmt, out decimal OthChrgGSTAmt1, out decimal OthChrgBasicAmt1)
        {
            OthChrgGSTAmt1 = 0;
            OthChrgBasicAmt1 = 0;

            decimal OthChrgGST = 0, OthChrgGSTAmt = 0, OthChrgBasicAmt = 0;
            bool OthChrgBeforeGST;
            int Taxtype;

            if (OthChrgId > 0 && txtOthChrgAmt > 0)
            {
                OtherChargeSQL tmp = new OtherChargeSQL();
                List<Entity.OtherCharge> lstEntity = new List<Entity.OtherCharge>();
                lstEntity = tmp.GetOtherChargeList(OthChrgId);
                OthChrgGST = (lstEntity.Count > 0) ? Convert.ToDecimal(lstEntity[0].GST_Per) : 0;
                Taxtype = (lstEntity.Count > 0) ? Convert.ToInt32(lstEntity[0].TaxType) : 0;
                OthChrgBeforeGST = (lstEntity.Count > 0) ? Convert.ToBoolean(lstEntity[0].BeforeGST) : false;
                funCalOthChrgGST(txtOthChrgAmt, OthChrgBeforeGST, OthChrgGST, Taxtype, out OthChrgGSTAmt, out OthChrgBasicAmt);
                OthChrgGSTAmt1 = OthChrgGSTAmt;
                OthChrgBasicAmt1 = OthChrgBasicAmt;
            }
            else
            {
                OthChrgGSTAmt1 = 0;
                OthChrgBasicAmt1 = 0;
            }
        }
        #endregion Transaction_Common_Functions

        public virtual List<Entity.DocPrinterSettings> GetDocPrinterSettings(String pLoginUserID, string pFormatType)
        {
            List<Entity.DocPrinterSettings> lstEntity = new List<Entity.DocPrinterSettings>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDocPrinterSettings";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@FormatType", pFormatType);
            SqlDataReader dr = ExecuteDataReader(cmdGet);

            while (dr.Read())
            {
                Entity.DocPrinterSettings objEntity = new Entity.DocPrinterSettings();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");
                objEntity.FormatType = GetTextVale(dr, "FormatType");
                objEntity.Header_Spacing = GetTextVale(dr, "Header_Spacing");
                objEntity.Header_FontName = GetTextVale(dr, "Header_FontName");
                objEntity.Header_FontSize = GetInt64(dr, "Header_FontSize");
                objEntity.Header_QR = GetBoolean(dr, "Header_QR");
                objEntity.Header_QR_Position = GetInt64(dr, "Header_QR_Position");
                objEntity.Header_QR_Size = GetTextVale(dr, "Header_QR_Size");
                objEntity.Header_Company = GetBoolean(dr, "Header_Company");
                objEntity.Header_Company_Position = GetInt64(dr, "Header_Company_Position");
                objEntity.Introduction_Show = GetBoolean(dr, "Introduction_Show");
                objEntity.Introduction_BeforePageBreak = GetBoolean(dr, "Introduction_BeforePageBreak");
                objEntity.Introduction_AfterPageBreak = GetBoolean(dr, "Introduction_AfterPageBreak");
                objEntity.ProdDetail_Show = GetBoolean(dr, "ProdDetail_Show");
                objEntity.ProdDetail_Spacing = GetTextVale(dr, "ProdDetail_Spacing");
                objEntity.ProdDetail_BeforePageBreak = GetBoolean(dr, "ProdDetail_BeforePageBreak");
                objEntity.ProdDetail_AfterPageBreak = GetBoolean(dr, "ProdDetail_AfterPageBreak");
                objEntity.ProdDetail_WithSpecification = GetBoolean(dr, "ProdDetail_WithSpecification");
                objEntity.ProdDetail_WithAssembly = GetBoolean(dr, "ProdDetail_WithAssembly");
                objEntity.ProdDetail_WithImage = GetBoolean(dr, "ProdDetail_WithImage");
                objEntity.ProdDetail_Image_Size = GetTextVale(dr, "ProdDetail_Image_Size");
                objEntity.Footer_Spacing = GetTextVale(dr, "Footer_Spacing");
                objEntity.Footer_PageNo = GetBoolean(dr, "Footer_PageNo");
                objEntity.Footer_PrintDate = GetBoolean(dr, "Footer_PrintDate");
                objEntity.Footer_FontName = GetTextVale(dr, "Footer_FontName");
                objEntity.Footer_FontSize = GetInt64(dr, "Footer_FontSize");
                objEntity.PrintingMargin_Plain = GetTextVale(dr, "PrintingMargin_Plain");
                objEntity.PrintingMargin_WithHeader = GetTextVale(dr, "PrintingMargin_WithHeader");
                objEntity.ProdDetail_Lines = GetInt64(dr, "ProdDetail_Lines");

                lstEntity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstEntity;
        }

        public static DataTable getBackupTableList()
        {
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            dr.Close();
            ForceCloseConncetion();
            return dt;
        }

        


        public static string ConvertNumbertoWords(int number)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            // ------------------------------------------------------------------------
            if (number == 0)
                return "ZERO";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            // ------------------------------------------------------------------------
            string words = "";
            if ((number / 10000000) > 0)
            {
                words += ConvertNumbertoWords(number / 10000000) + " CRORE ";
                number %= 10000000;
            }
            //if ((number / 1000000) > 0)
            //{
            //    words += ConvertNumbertoWords(number / 1000000) + " LACS ";
            //    number %= 1000000;
            //}
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " LACS ";
                number %= 100000;
            }
            //if ((number / 10000) > 0)
            //{
            //    words += ConvertNumbertoWords(number / 10000) + " THOUSANDS ";
            //    number %= 10000;
            //}
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "AND ";
                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            string tmpVal = myTI.ToTitleCase(words.ToLower());
            return tmpVal;
        }
        private static string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
            return words;
        }

        public static string ConvertNumbertoWordsinDecimalNew(Decimal doubleNumber)
        {
            String[] number = Convert.ToString(doubleNumber.ToString()).Split('.');
            Int64 afterFloating = 0;
            var afterFloatinWord = "";
            //var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloating = (int)Math.Floor(Convert.ToDecimal(number[0]));
            if (number.Count() > 1)
            {
                afterFloating = (int)Math.Floor(Convert.ToDecimal(number[1]));
                afterFloatinWord = ConvertNumbertoWordsinDecimal(afterFloating) + " Paise";
            }
            //var beforeFloatingPointWord = ConvertNumbertoWordsinDecimal(beforeFloatingPoint) + " Rupees";
            var beforeFloatingWord = ConvertNumbertoWordsinDecimal(beforeFloating) + " Rupees "; 
            

            //var afterFloatingPointWord = SmallNumberToWord((int)Math.Round(((doubleNumber - beforeFloatingPoint) * 100), 2), beforeFloatingPointWord);
            //return "{beforeFloatingPointWord} and {afterFloatingPointWord}";
            var finalword = "";
            if (number.Count() > 1)
                finalword = beforeFloatingWord + afterFloatinWord;
            else
                finalword = beforeFloatingWord ;
            return finalword;
        }
        public static string ConvertNumbertoWordsinDecimal(double doubleNumber)
        {
            var beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            var beforeFloatingPointWord = ConvertNumbertoWordsinDecimal(beforeFloatingPoint);
            var afterFloatingPointWord =SmallNumberToWord((int)Math.Round(((doubleNumber - beforeFloatingPoint) * 100),2), beforeFloatingPointWord);
            //return "{beforeFloatingPointWord} and {afterFloatingPointWord}";
            return afterFloatingPointWord;
        }
        public static string ConvertNumbertoWordsinDecimal(int number)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + ConvertNumbertoWordsinDecimal(Math.Abs(number));

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += ConvertNumbertoWordsinDecimal(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += ConvertNumbertoWordsinDecimal(number / 1000000) + " million ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += ConvertNumbertoWordsinDecimal(number / 1000) + " thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += ConvertNumbertoWordsinDecimal(number / 100) + " hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);
            string tmpVal = myTI.ToTitleCase(words.ToLower());
            return tmpVal;
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Project Chat Box Log
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public virtual List<Entity.ConversationChatBox> GetConversationChatBoxList(string ModuleName, string KeyValue, string LoginUserID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ConversationChatBoxList";
            cmdGet.Parameters.AddWithValue("@ModuleName", ModuleName);
            cmdGet.Parameters.AddWithValue("@KeyValue", KeyValue);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ConversationChatBox> lstLocation = new List<Entity.ConversationChatBox>();
            while (dr.Read())
            {
                Entity.ConversationChatBox objLocation = new Entity.ConversationChatBox();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.ModuleName = GetTextVale(dr, "ModuleName");
                objLocation.KeyValue = GetTextVale(dr, "KeyValue");
                objLocation.CustomerID = GetInt64(dr, "CustomerID");
                objLocation.CustomerName = GetTextVale(dr, "CustomerName");
                objLocation.FromUser = GetTextVale(dr, "FromUser");
                objLocation.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objLocation.ToUser = GetTextVale(dr, "ToUser");
                objLocation.ToEmployeeName = GetTextVale(dr, "ToEmployeeName");
                objLocation.Message = GetTextVale(dr, "Message");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                objLocation.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateConversationChatBox(Entity.ConversationChatBox objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ConversationChatBox_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@ModuleName", objEntity.ModuleName);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@KeyValue", objEntity.KeyValue);
            cmdAdd.Parameters.AddWithValue("@FromUser", objEntity.FromUser);
            cmdAdd.Parameters.AddWithValue("@ToUser", objEntity.ToUser);
            cmdAdd.Parameters.AddWithValue("@Message", objEntity.Message);
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
    }
}
