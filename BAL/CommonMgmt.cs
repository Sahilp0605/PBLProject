using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace BAL
{ 
    public class CommonMgmt  
    {
        public static String GetQuotationType(Int64 pkID)
        {
            return (new DAL.CommonSQL().GetQuotationType(pkID));
        }
        public static List<Entity.Complaint> GetCustomerListForVisit(Int64 pkID)
        {
            return (new DAL.CommonSQL().GetCustomerListForVisit(pkID));
        }
        public static List<Entity.Complaint> GetSRNO(Int64 ComplaintNo)
        {
            return (new DAL.CommonSQL().GetSRNO(ComplaintNo));
        }
        public static List<Entity.Complaint> GetComplaintList(Int64 ComplaintNo)
        {
            return (new DAL.CommonSQL().GetComplaintList(ComplaintNo));
        }
        public static List<Entity.Customer> ReGenerateTrialBalance()
        {
            return (new DAL.CommonSQL().ReGenerateTrialBalance());
        }
        public static List<Entity.Products> ReGenerateStock(Boolean WantList)
        {
            return (new DAL.CommonSQL().ReGenerateStock(WantList));
        }

        public static List<Entity.CRMSummary> GetDashboardCRMSummary(string Type, Int64 pMonth, Int64 pYear, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetDashboardCRMSummary(Type, pMonth, pYear, LoginUserID));
        }
        public static List<Entity.CRMSummary> GetDashboardDailySummary(string Type, Int64 pMonth, Int64 pYear, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetDashboardDailySummary(Type, pMonth, pYear, LoginUserID));
        }
        public static string GetDocRefNoList(string pModule, string KeyValue)
        {
            return DAL.CommonSQL.GetDocRefNoList(pModule, KeyValue);
        }
        public static string GetHREmailAddress()
        {
            return DAL.CommonSQL.GetHREmailAddress();
        }
        public static Boolean AllowDeleteModuleEntry(string Module, string KeyValue)
        {
            return (new DAL.CommonSQL().AllowDeleteModuleEntry(Module, KeyValue));
        }
        public static Boolean AllowDeleteModuleEntry(string Module, string KeyValue, Int64 ProductID)
        {
            return (new DAL.CommonSQL().AllowDeleteModuleEntry(Module, KeyValue, ProductID));
        }

        public static void DeleteFileFromFolder(string rootFolderName, string FileToDelete)
        {
            DAL.CommonSQL.DeleteFileFromFolder(rootFolderName, FileToDelete);
        }
        public static string GetEmployeeEmailByEmployeeID(Int64 pEmpID)
        {
            return DAL.CommonSQL.GetEmployeeEmailByEmployeeID(pEmpID);
        }
        public static List<Entity.GSTR> PendingGSTNO(string LoginUserID)
        {
            return (new DAL.CommonSQL().PendingGSTNO(LoginUserID));
        }
        public static List<Entity.GSTR> GSTRSummary(string ReportType, Int64 pMonth, Int64 pYear, Int64 CustomerID, string LoginUserID)
        {
            return (new DAL.CommonSQL().GSTRSummary(ReportType, pMonth, pYear, CustomerID, LoginUserID));
        }

        public static List<Entity.PurchaseBill> GetLocationList()
        {
            return (new DAL.CommonSQL().GetLocationList());
        }

        public static List<Entity.PurchaseBill> GetLocationList_DistinctEmployeeCity()
        {
            return (new DAL.CommonSQL().GetLocationList_DistinctEmployeeCity());
        }

        public static string GetFinishProductNameForSO(Int64 pProductID)
        {
            return DAL.CommonSQL.GetFinishProductNameForSO(pProductID);    
        }

        public static Int64 GetStateCode(Int64 StateCode)
        {
            return DAL.CommonSQL.GetStateCode(StateCode);
        }

        public static string GetRefererenceNoFromBill(String InvoiceNo)
        {
            return DAL.CommonSQL.GetRefererenceNoFromBill(InvoiceNo);
        }

        public static DataTable GetRILPrice()
        {
            return (new DAL.CommonSQL().GetRILPrice());
        }

        public static void UpdateCourierImage(string pKeyField, string pFileName)
        {
            new DAL.CommonSQL().UpdateCourierImage(pKeyField, pFileName);
        }

        public static void UpdateRILPrice(Decimal RILPrice)
        {
            new DAL.CommonSQL().UpdateRILPrice(RILPrice);
        }

        public static void AddRILPrice(Decimal RILPrice)
        {
            new DAL.CommonSQL().AddRILPrice(RILPrice);
        }
        public static void GetProductPriceListRate(Int64 pCustID, Int64 pProdID, out Decimal retUnitPrice, out Decimal retDiscount)
        {
            new DAL.CommonSQL().GetProductPriceListRate(pCustID, pProdID, out retUnitPrice, out retDiscount);
        }

        public static List<Entity.ApplicationMenu> GetMenuIconList(String LoginUserID)
        {
            return (new DAL.CommonSQL().GetMenuIconList(LoginUserID));
        }

        public static List<Entity.ApplicationMenu> GetMenuAddEditDelList(Int64 MenuID, String LoginUserID)
        {
            return (new DAL.CommonSQL().GetMenuAddEditDelList(MenuID, LoginUserID));
        }

        public static List<Entity.ApplicationMenu> GetGeneralMenuList(String LoginUserID)
        {
            return (new DAL.CommonSQL().GetGeneralMenuList(LoginUserID));
        }
        public static List<Entity.ApplicationMenu> GetMenuGeneralAddEditDelList(string MenuID, String LoginUserID)
        {
            return (new DAL.CommonSQL().GetMenuGeneralAddEditDelList(MenuID, LoginUserID));
        }
        public static void AddUpdateUserAction(Entity.ApplicationMenu objEntity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateUserAction(objEntity, out ReturnCode, out ReturnMsg);
        }
        //private static List<T> ConvertDataTable<T>(DataTable dt)
        //{
        //    List<T> data = new List<T>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        T item = GetItem<T>(row);
        //        data.Add(item);
        //    }
        //    return data;
        //}


        public static string GetDrivingAllowance(Int64 pEmpID, Int64 pMon, Int64 pYear)
        {
            return DAL.CommonSQL.GetDrivingAllowance(pEmpID, pMon, pYear);
        }
        public static string GetDrivingKilometers(Int64 pEmpID, Int64 pMon, Int64 pYear)
        {
            return DAL.CommonSQL.GetDrivingKilometers(pEmpID, pMon, pYear);
        }
        public static DateTime GetLatestAttendenceDt()
        {
            return DAL.CommonSQL.GetLatestAttendenceDt();
        }

        public static List<Entity.ReportMenu> GetReportsList()
        {
            return (new DAL.CommonSQL().GetReportsList());
        }

        public static void GetDailyReport(DateTime startdate, DateTime enddate, string LoginUserID, out DataTable table1, out DataTable table2, out DataTable table3, out DataTable table4, out DataTable dtFollow, out DataTable dtQuotation, out DataTable dtSalesOrder, out DataTable dtSalesBil)
        {
            new DAL.CommonSQL().GetDailyReport(startdate, enddate, LoginUserID, out table1, out table2, out table3, out table4, out dtFollow, out dtQuotation, out dtSalesOrder, out dtSalesBil);
        }

        public static DataTable GetExportDataList(string keyVal)
        {
            return (new DAL.CommonSQL().GetExportDataList(keyVal));
        }
        public static DataTable GetExportDataList(string module, string loginuserid, string keyval)
        {
            return (new DAL.CommonSQL().GetExportDataList(module, loginuserid, keyval));
        }
        // **************************************************************************
        public static List<Entity.Chat> GetChatBoxList(string pFrom, string pTo)
        {
            return (new DAL.CommonSQL().GetChatBoxList(pFrom, pTo));
        }
        public static List<Entity.Chat> GetChatBoxUserList(string pUserID)
        {
            return (new DAL.CommonSQL().GetChatBoxUserList(pUserID));
        }
        public static void UpdateChatLastTimestamp(string pFromUser, string pToUser)
        {
            new DAL.CommonSQL().UpdateChatLastTimestamp(pFromUser, pToUser);
        }
        public static void AddUpdateChatBox(Entity.Chat entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateChatBox(entity, out ReturnCode, out ReturnMsg);
        }
        // **************************************************************************
        public static List<Entity.QuotationDetail> GetTaxSummaryWidget(string pModule, string pKeyID, Boolean pHSNFlag)
        {
            return (new DAL.CommonSQL().GetTaxSummaryWidget(pModule, pKeyID, pHSNFlag));
        }
        public static List<Entity.SalesTaxDetail> GetTaxSummaryWidget(string Module, string KeyID, Boolean HSNFlag, string FromDate, string ToDate)
        {
            return (new DAL.CommonSQL().GetTaxSummaryWidget(Module, KeyID, HSNFlag, FromDate, ToDate));
        }
        public static List<Entity.QuotationDetail> GetTaxSummary(string dtTable, string taxCategory, string keyVal)
        {
            return (new DAL.CommonSQL().GetTaxSummary(dtTable, taxCategory, keyVal));
        }
        public static List<Entity.QuotationDetail> GetTaxHSNSummary(string dtTable, string taxCategory, string keyVal)
        {
            return (new DAL.CommonSQL().GetHSNTaxSummary(dtTable, taxCategory, keyVal));
        }
        public static List<Entity.QuotationDetail> GetHSNTaxWithNumberSummaryNew(string dtTable, string taxCategory, string keyVal)
        {
            return (new DAL.CommonSQL().GetHSNTaxWithNumberSummaryNew(dtTable, taxCategory, keyVal));
        }

        public static List<Entity.Currency> GetCurrencyList()
        {
            return (new DAL.CommonSQL().GetCurrencyList());
        }

        public static List<Entity.OrgChart> GetOrgChartList(string LoginUserID)
        {
            return (new DAL.CommonSQL().GetOrgChartList(LoginUserID));
        }

        public static List<Entity.CalenderEvent> GetCalenderList(Int64 pMonth, Int64 pYear, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetCalenderList(pMonth, pYear, LoginUserID));
        }

        public static List<Entity.CalenderEvent> GetCalenderListByEmployee(Int64 pMonth, Int64 pYear, Int64 pEmployeeID, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetCalenderListByEmployee(pMonth, pYear, pEmployeeID, LoginUserID));
        }

        public static List<Entity.DashboardNotification> GetNotificationList(string LoginUserID,string ListBy)
        {
            return (new DAL.CommonSQL().GetNotificationList(LoginUserID,ListBy));
        }
        public static void UpdateUserTimeStamp(string pLoginUserId, string pCompanyID)
        {
            new DAL.CommonSQL().UpdateUserTimeStamp(pLoginUserId, pCompanyID);
        }

        public static string GetBroadcastMessage(string pLoginUserID)
        {
            return DAL.CommonSQL.GetBroadcastMessage(pLoginUserID);
        }
        public static Int64 GetNoOfUsers()
        {
            return DAL.CommonSQL.GetNoOfUsers();
        }

        public static string GetServerTimestamp()
        {
            return DAL.CommonSQL.GetServerTimestamp();
        }
        public static string GetPageHiddenControls(string pPageName)
        {
            return DAL.CommonSQL.GetPageHiddenControls(pPageName);
        }

        public static void AddUpdateEmailNotification(Entity.EmailNotifications entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateEmailNotification(entity, out ReturnCode, out ReturnMsg);
        }
        // ---------------------------------------------------------------
        // >>>>>>>>>>>>>>>>> Company Profile 
        // ---------------------------------------------------------------
        public static void AddUpdateCompanyProfile(Entity.CompanyProfile entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateCompanyProfile(entity, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.CompanyProfile> GetCompanyProfileList(Int64 CompanyID, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetCompanyProfileList(CompanyID, LoginUserID));
        }
        // ---------------------------------------------------------------
        // >>>>>>>>>>>>>>>>> Organization Bank Information 
        // ---------------------------------------------------------------
        public static void AddUpdateBankInfo(Entity.OrganizationBankInfo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateBankInfo(entity, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.OrganizationBankInfo> GetBankInfoList(Int64 CompanyID)
        {
            return (new DAL.CommonSQL().GetBankInfoList(CompanyID));
        }
        public static List<Entity.OrganizationBankInfo> GetBankInfoListBypkID(Int64 pkID)
        {
            return (new DAL.CommonSQL().GetBankInfoListBypkID(pkID));
        }
        public static List<Entity.OrganizationBankInfo> GetBankInfo(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CommonSQL().GetBankInfo(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.OrganizationBankInfo> GetBankInfo(Int64 pkID, string LoginUserID, string Search, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CommonSQL().GetBankInfo(pkID, LoginUserID, Search, PageNo, PageSize, out TotalRecord));
        }
        public static void DeleteBankDetails(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().DeleteBankDetails(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.CompanyProfile> GetCostantList(string Category, string ConstantHead, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetCostantList(Category, ConstantHead, LoginUserID));
        }

        public static void UpdateEmailNotificationStatus(string pHeader, string pValue, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().UpdateEmailNotificationStatus(pHeader, pValue, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateUserLog(Entity.UserLog entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateUserLog(entity, out ReturnCode, out ReturnMsg);
        }
        // >>>>>>>>>>> Product Documents 
        public static List<Entity.Documents> GetDocumentsList(Int64 pkID, Int64 ProductID)
        {
            return (new DAL.CommonSQL().GetDocumentsList(pkID, ProductID));
        }

        public static void AddUpdateProductDocuments(Int64 pProductID, string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateProductDocuments(pProductID, pFilename, pType, pLoginUserID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteProductDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().DeleteProductDocuments(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteProductDocumentsByProductId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().DeleteProductDocumentsByProductId(pkID, out ReturnCode, out ReturnMsg);
        }
        // -----------------------------------------------------------
        // >>>>>>>>>>> Document Gallery
        // -----------------------------------------------------------
        public static List<Entity.Documents> GetDocumentGalleryList(Int64 pkID)
        {
            return (new DAL.CommonSQL().GetDocumentGalleryList(pkID));
        }
        public static List<Entity.Documents> GetDocumentGalleryListByName(String filename)
        {
            return (new DAL.CommonSQL().GetDocumentGalleryListByName(filename));
        }
        public static void AddUpdateDocumentGallery(string pFilename, string pType, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateDocumentGallery(pFilename, pType, pLoginUserID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteDocumentGallery(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().DeleteDocumentGallery(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteDocumentGalleryByFileName(String pFileName, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().DeleteDocumentGalleryByFileName(pFileName, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.Contents> GetContentList(Int64 pkID, string Category)
        {
            return (new DAL.CommonSQL().GetContentList(pkID, Category));
        }

        public static List<Entity.Contents> GetContentList(Int64 pkID, string Category, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CommonSQL().GetContentList(pkID, Category, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateContents(Entity.Contents entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateContents(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteContents(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().DeleteContents(pkID, out ReturnCode, out ReturnMsg);
        }

        public static string SendAlertNotificationEmail(string pTemplateID, string pEmailTo, string pHelpLogOrgCode, string pRelationType, string pOrgCode, string pOrgName, string pOrgDeptCode, string pOrgDeptName, string pContactName, string pTicketNo, string pMemberName, string pLatitude, string pLongitude, string nearByPlace)
        {
            return (new DAL.CommonSQL().SendAlertNotificationEmail(pTemplateID, pEmailTo, pHelpLogOrgCode, pRelationType, pOrgCode, pOrgName, pOrgDeptCode, pOrgDeptName, pContactName, pTicketNo, pMemberName, pLatitude, pLongitude, nearByPlace));
        }

        //public static string SendTripNotificationEmail(string pTemplateID, string pTicketNo)
        //{
        //    return (new DAL.CommonSQL().SendTripNotificationEmail(pTemplateID, pTicketNo));
        //}
        
        public static void SendNotification_Firebase(string pModuleName, string pnotificationMsg, string pLoginUserID,Int64 AssignedToEmployeeID)
        {
            new DAL.CommonSQL().SendNotification_Firebase(pModuleName, pnotificationMsg, pLoginUserID, AssignedToEmployeeID);
        }

        public static void SendNotificationToDB(string pModuleName,Int64 pModulePkID, string pnotificationMsg, string pLoginUserID, Int64 pAssignedToEmployeeID)
        {
            new DAL.CommonSQL().SendNotificationToDB(pModuleName, pModulePkID ,pnotificationMsg, pLoginUserID, pAssignedToEmployeeID);
        }

        public static string RetrieveFormattedAddress(string lat, string lng)
        {
            return (new DAL.CommonSQL().RetrieveFormattedAddress(lat, lng));
        }

        public static List<Entity.DashboardCountSummary> GetEmployeeFollowupCount(string pUserID)
        {
            return (new DAL.CommonSQL().GetEmployeeFollowupCount(pUserID));
        }
        // ===================================================================================================
        // All User Defined Function's 
        // ===================================================================================================
        public static string GetEmployeeEmailAddress(String pUserID)
        {
            return DAL.CommonSQL.GetEmployeeEmailAddress(pUserID);
        }

        public static string GetOrgHeadEmployee(String OrgCode)
        {
            return DAL.CommonSQL.GetOrgHeadEmployee(OrgCode);
        }

        public static string GetOrgCodeByUserID(String LoginUserID)
        {
            return DAL.CommonSQL.GetOrgCodeByUserID(LoginUserID);
        }

        public static string GetCustomerEmailAddress(String pUserID)
        {
            return DAL.CommonSQL.GetCustomerEmailAddress(pUserID);
        }

        public static string GetInquiryNo(string pDate)
        {
            return DAL.CommonSQL.GetInquiryNo(pDate);
        }

        public static string GetMaterialMovementOrderNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetMaterialMovementOrderNo(pkID);
        }


        public static string GetQuotationNo(string pDate)
        {
            return DAL.CommonSQL.GetQuotationNo(pDate);
        }

        public static string GetJobCardNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetJobCardNo(pkID);
        }
        public static Int64 GetJobCardpkID(String JobCardNo)
        {
            return DAL.CommonSQL.GetJobCardpkID(JobCardNo);
        }
        public static string GetQuotationNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetQuotationNo(pkID);
        }
        public static string GetProjectSheetNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetProjectSheetNo(pkID);
        }

        public static string GetVoucherNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetVoucherNo(pkID);
        }

        public static string GetOutwardNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetOutwardNo(pkID);
        }
        public static string GetInwardNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetInwardNo(pkID);
        }
        public static string GetVoucherNoForPDF(Int64 pkID)
        {
            return DAL.CommonSQL.GetVoucherNoForPDF(pkID);
        }
        public static string GetIndentNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetIndentNo(pkID);
        }

        public static string GetJobCardOutwardNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetJobCardOutwardNo(pkID);
        }

        public static Int64 GetInwardNoPrimaryID(string pInwardNo)
        {
            return DAL.CommonSQL.GetInwardNoPrimaryID(pInwardNo);
        }
        public static Int64 GetOutwardNoPrimaryID(string pOutwardNo)
        {
            return DAL.CommonSQL.GetOutwardNoPrimaryID(pOutwardNo);
        }

        public static Int64 GetQuotationNoPrimaryID(string pQuotNo)
        {
            return DAL.CommonSQL.GetQuotationNoPrimaryID(pQuotNo);
        }
        
        public static string GetQuotationNoFromInquiryNo(string pInquiryNo)
        {
            return DAL.CommonSQL.GetQuotationNoFromInquiryNo(pInquiryNo);
        }

        public static Int64 GetInquiryNoPrimaryID(string pInqNo)
        {
            return DAL.CommonSQL.GetInquiryNoPrimaryID(pInqNo);
        }

        public static Int64 GetSalesOrderPrimaryID(string pOrderNo)
        {
            return DAL.CommonSQL.GetSalesOrderPrimaryID(pOrderNo);
        }

        public static Int64 GetSalesBillPrimaryID(string pInvoiceNo)
        {
            return DAL.CommonSQL.GetSalesBillPrimaryID(pInvoiceNo);
        }
        public static Int64 GetPurchaseBillPrimaryID(string pInvoiceNo)
        {
            return DAL.CommonSQL.GetPurchaseBillPrimaryID(pInvoiceNo);
        }
        //public static string GetInwardNo()
        //{
        //    return DAL.CommonSQL.GetInwardNo();
        //}
        public static string fnGetInwardNo(string pDate)
        {
            return DAL.CommonSQL.fnGetInwardNo(pDate);
        }
        //public static string GetOutwardNo()
        //{
        //    return DAL.CommonSQL.GetOutwardNo();
        //}



        public static string fnGetOutwardNoByDate(string pDate)
        {
            return DAL.CommonSQL.fnGetOutwardNoByDate(pDate);
        }
        public static string GetSalesOrderNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetSalesOrderNo(pkID);
        }

        public static string GetWorkOrderCommNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetWorkOrderCommNo(pkID);
        }


        public static string GetSalesOrderDealerNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetSalesOrderNo(pkID);
        }

        public static string GetPurchaseOrderNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetPurchaseOrderNo(pkID);
        }
        public static Int64 GetPurchaseOrderPrimaryID(string pPONo)
        {
            return DAL.CommonSQL.GetPurchaseOrderPrimaryID(pPONo);
        }

        public static string GetPurchaseBillNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetPurchaseBillNo(pkID);
        }
        public static string GetSalesBillNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetSalesBillNo(pkID);
        }
        public static string GetOrgEmpAccuPanelNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetOrgEmpAccuPanelNo(pkID);
        }
        public static string GetVisitAcupanelNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetVisitAcupanelNo(pkID);
        }
        public static string GetComplaintVisitNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetComplaintVisitNo(pkID);
        }
        public static string GetSalesChallanNo(Int64 pkID)
        {
            return DAL.CommonSQL.GetSalesChallanNo(pkID);
        }

        public static string GetSalesOrderNo(string pDate)
        {
            return DAL.CommonSQL.GetSalesOrderNo(pDate);
        }


        public static string GetConstant(string pHead, Int64 pkID)
        {
            return DAL.CommonSQL.GetConstant(pHead, pkID);
        }

        public static string GetConstant(string pHead, Int64 pkID, Int64 pCompanyID)
        {
            return DAL.CommonSQL.GetConstant(pHead, pkID, pCompanyID);
        }

        public static string GetCompanyName()
        {
            return DAL.CommonSQL.GetCompanyName();
        }

        public static string GetAuthorizedSignUserID(string pUserID)
        {
            return DAL.CommonSQL.GetAuthorizedSignUserID(pUserID);
        }
        
        public static string GetEmployeeNameByUserID(string pUserID)
        {
            return DAL.CommonSQL.GetEmployeeNameByUserID(pUserID);
        }

        public static string GetEmployeeNameByEmployeeID(Int64 pEmployeeID)
        {
            return DAL.CommonSQL.GetEmployeeNameByEmployeeID(pEmployeeID);
        }

        public static string GetEmployeeIDByUserID(string pUserID)
        {
            return DAL.CommonSQL.GetEmployeeIDByUserID(pUserID);
        }
        public static string GetUserIDByEmployeeID(Int64 pEmployeeID)
        {
            return DAL.CommonSQL.GetUserIDByEmployeeID(pEmployeeID);
        }
        public static string GetDesignationByUserID(string pUserID)
        {
            return DAL.CommonSQL.GetDesignationByUserID(pUserID);
        }

        public static string GetMemberName(string pRegistrationNo)
        {
            return DAL.CommonSQL.GetMemberName(pRegistrationNo);
        }

        public static string GetDriverName(string pDriverID)
        {
            return DAL.CommonSQL.GetDriverName(pDriverID);
        }

        public static string GetRegistrationNoFromMac(string pMacID)
        {
            return DAL.CommonSQL.GetRegistrationNoFromMac(pMacID);
        }

        public static string CheckAutoPilotMode(string pLoginUserID)
        {
            return DAL.CommonSQL.CheckAutoPilotMode(pLoginUserID);
        }

        public static Int64 setInquiryStatusFromFollowup(string pInquiryNo, Int64 pStatusID, Int64 pClosureID)
        {
            return DAL.CommonSQL.setInquiryStatusFromFollowup(pInquiryNo, pStatusID, pClosureID);
        }

        public static string GetCustomerEmailAddress(Int64 pCustomerID)
        {
            return DAL.CommonSQL.GetCustomerEmailAddress(pCustomerID);
        }
        // ===================================================================================================
        // Dashboard Function for summary 
        // ===================================================================================================
        public static List<Entity.DashboardCountSummary> GetWidgetList(string pLoginUserID)
        {
            return (new DAL.CommonSQL().GetWidgetList(pLoginUserID));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardInquirySummary(string pLoginUserID, Int64 pMonth, Int64 pYear, String pCategory)
        {
            return (new DAL.CommonSQL().GetDashboardInquirySummary(pLoginUserID, pMonth, pYear, pCategory));
        }

        public static List<Entity.DashboardInquirySummary> GetDashboardInquiryStatusSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardInquiryStatusSummary(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardInquirySourceSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardInquirySourceSummary(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardInquiryDisQualiSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardInquiryDisQualiSummary(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardInquiryTeleCallStatusSummary(string pLoginUserID, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardInquiryTeleCallStatusSummary(pLoginUserID, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardInquiryTeleDisQualiSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardInquiryTeleDisQualiSummary(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardInquiryTeleConversionSummary(string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardInquiryTeleConversionSummary(pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardTeleEntrySummary(string pLoginUserID, string pLeadSource, string pCategory, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CommonSQL().GetDashboardTeleEntrySummary(pLoginUserID, pLeadSource, pCategory, pMonth, pYear));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardSalesSummary(string pLoginUserID, Int64 pMonth, Int64 pYear, String pCategory)
        {
            return (new DAL.CommonSQL().GetDashboardSalesSummary(pLoginUserID, pMonth, pYear, pCategory));
        }

        public static List<Entity.DashboardCountSummary> GetDashboardExternalSummary(string pLoginUserID, Int64 pMonth, Int64 pYear, String pCategory)
        {
            return (new DAL.CommonSQL().GetDashboardExternalSummary(pLoginUserID, pMonth, pYear, pCategory));
        }

        public static List<Entity.DashboardCountSummary> GetDashboard2_Summary(String pCategory, DateTime date1, DateTime date2, string pLoginUserID)
        {
            return (new DAL.CommonSQL().GetDashboard2_Summary(pCategory, date1, date2, pLoginUserID));
        }
        public static string SendEmailNotify(string pTemplateID, string pLoginUserID, DataTable dtDetail)
        {
            return (new DAL.CommonSQL().SendEmailNotify(pTemplateID, pLoginUserID, dtDetail));
        }
        public static string SendEmailNotifcation(string pTemplateID, string pLoginUserID, Int64 pkID, string pEmailAddress)
        {
            return (new DAL.CommonSQL().SendEmailNotifcation(pTemplateID, pLoginUserID, pkID, pEmailAddress));
        }

        public static string SendSMSNotifcation(string pTemplateID, string pLoginUserID, string pContactNo)
        {
            return (new DAL.CommonSQL().SendSMSNotifcation(pTemplateID, pLoginUserID, pContactNo));
        }

        public static string SendMsg91Message(string pMobileNos, string pMessage)
        {
            return (new DAL.CommonSQL().SendMsg91Message(pMobileNos, pMessage));
        }

        // --------------------------------------------------------------------------
        // Section : WhatsApp Notifications
        // --------------------------------------------------------------------------
        public static string SendWhatsApp(string pTemplateID, string pLoginUserID, string pContactNo)
        {
            return (new DAL.CommonSQL().SendWhatsApp(pTemplateID, pLoginUserID, pContactNo));
        }

        public static string SendWhatsAppMessage(string pMobileNos, string pMessage)
        {
            return (new DAL.CommonSQL().SendWhatsAppMessage(pMobileNos, pMessage));
        }
        public static string SendHospitalNotification(string pTemplateID, string pLoginUserID, string pHospitalName, string pPatientlName, string pSpeciality, string pEmailAddress, string pAppoinmentDate, string pInquiryNo)
        {
            return (new DAL.CommonSQL().SendHospitalNotification(pTemplateID, pLoginUserID, pHospitalName, pPatientlName, pSpeciality, pEmailAddress, pAppoinmentDate, pInquiryNo));
        }


        public static string SendLeaveNotification(string pTemplateID, Entity.LeaveRequest objEntity)
        {
            return (new DAL.CommonSQL().SendLeaveNotification(pTemplateID, objEntity));
        }
        public static string SendDailyReportNotification(string pTemplateID, string ToEmailAddress, string pdfAttachment, string StartDate, string EndDate)
        {
            return (new DAL.CommonSQL().SendDailyReportNotification(pTemplateID, ToEmailAddress, pdfAttachment, StartDate, EndDate));
        }

        public static string SendHRNotification(string pTemplateID, Int64 pEmpID)
        {
            return (new DAL.CommonSQL().SendHRNotification(pTemplateID, pEmpID));
        }

        public static string SendFeedback(string pTemplateID, string pCustomerEmailID)
        {
            return (new DAL.CommonSQL().SendFeedback(pTemplateID, pCustomerEmailID));
        }

        #region Transaction_Common_Functions
        //////////////////////////////////////////////////////////////////////////////////
        //******************** Transaction Common functions ***************************
        //////////////////////////////////////////////////////////////////////////////////       
        public static string checkGSTNO(string CustomerID)
        {
            return (new DAL.CommonSQL().checkGSTNO(CustomerID));
        }

        public static Boolean isIGST(string CustomerStateId, string CompanyStateId)
        {
            return (new DAL.CommonSQL().isIGST(CustomerStateId, CompanyStateId));
        }

        public static void funCalculate(Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, decimal HdDiscAmt, string CustomerStateId, string CompanyStateId, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        {
            new DAL.CommonSQL().funCalculate(TaxType, Qty, Rate, ItmDiscPer, ItmDiscAmt, TaxPer, AddTaxPer, HdDiscAmt, CustomerStateId, CompanyStateId, out  TaxAmt, out CGSTPer, out  CGSTAmt, out  SGSTPer, out  SGSTAmt, out IGSTPer, out  IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out  ItmDiscAmt1, out AddTaxAmt);
        }

        // --------------------------------------------------
        // ------------- For SteelMan -----------------------
        // --------------------------------------------------
        public static void funCalculateSteel(decimal UnitQuantity, Int16 TaxType, decimal Qty, decimal Rate, decimal ItmDiscPer, decimal ItmDiscAmt, decimal TaxPer, decimal AddTaxPer, decimal HdDiscAmt, string CustomerStateId, string CompanyStateId, out decimal TaxAmt, out decimal CGSTPer, out decimal CGSTAmt, out decimal SGSTPer, out decimal SGSTAmt, out decimal IGSTPer, out decimal IGSTAmt, out decimal NetRate, out decimal BasicAmt, out decimal NetAmt, out decimal ItmDiscPer1, out decimal ItmDiscAmt1, out decimal AddTaxAmt)
        {
            new DAL.CommonSQL().funCalculateSteel(UnitQuantity, TaxType, Qty, Rate, ItmDiscPer, ItmDiscAmt, TaxPer, AddTaxPer, HdDiscAmt, CustomerStateId, CompanyStateId, out  TaxAmt, out CGSTPer, out  CGSTAmt, out  SGSTPer, out  SGSTAmt, out IGSTPer, out  IGSTAmt, out NetRate, out BasicAmt, out NetAmt, out ItmDiscPer1, out  ItmDiscAmt1, out AddTaxAmt);
        }

        public static void funCalOthChrgGST(decimal txtOthChrgAmt, bool OthChrgBeforeGST, decimal OthChrgGST, int taxtype, out decimal OthChargGSTAmt, out decimal othchargBasicAmt)
        {
             new DAL.CommonSQL().funCalOthChrgGST( txtOthChrgAmt,  OthChrgBeforeGST,  OthChrgGST,  taxtype, out  OthChargGSTAmt, out  othchargBasicAmt);
        }


        public static DataTable funOnChangeTermination(DataTable dtDetail, string CustomerStateId, string CompanyStateId)
        {
            return (new DAL.CommonSQL(). funOnChangeTermination( dtDetail, CustomerStateId,  CompanyStateId));
        }

        public static void funOthChrgTextChange(Int64 OthChrgId, decimal txtOthChrgAmt, out decimal OthChrgGSTAmt1, out decimal OthChrgBasicAmt1)
        {
            new DAL.CommonSQL().funOthChrgTextChange(OthChrgId, txtOthChrgAmt, out  OthChrgGSTAmt1, out OthChrgBasicAmt1);
        }

        #endregion Transaction_Common_Functions

        public static List<Entity.SMS> GetSMSConfigSettings(string CompanyID)
        {
            return (new DAL.CommonSQL().GetSMSConfigSettings(CompanyID));
        }

        public static string SendSMSCampaign(string pCompanyID, string MobileNo, string Msg, List<Entity.SMS> lstSMSConfig)
        {
            return (new DAL.CommonSQL().SendSMSCampaign(pCompanyID, MobileNo, Msg, lstSMSConfig));
        }

        public static List<Entity.DocPrinterSettings> GetDocPrinterSettings(string pLoginUserID, string pFormatType)
        {
            return (new DAL.CommonSQL().GetDocPrinterSettings(pLoginUserID,pFormatType));
        }

        public static DataTable getBackupTableList()
        {
            return  DAL.CommonSQL.getBackupTableList();
        }

        public static string ConvertNumbertoWords(int number)
        {
            return DAL.CommonSQL.ConvertNumbertoWords(number);
        }
        public static string ConvertNumbertoWordsinDecimal(double number)
        {
            return DAL.CommonSQL.ConvertNumbertoWordsinDecimal(number);
        }

        public static string ConvertNumbertoWordsinDecimalNew(Decimal number)
        {
            return DAL.CommonSQL.ConvertNumbertoWordsinDecimalNew(number);
        }

        public static Int64 GetContractInfoNoPrimaryID(string pInqNo)
        {
            return DAL.CommonSQL.GetContractInfoNoPrimaryID(pInqNo);
        }

        public static Int64 GetEmployeeCode(Int64 pEmpCode)
        {
            return DAL.CommonSQL.GetEmployeeCode(pEmpCode);
        }

        public static string lastNotificationTimestamp(string LoginUserID)
        {
            return DAL.CommonSQL.lastNotificationTimestamp(LoginUserID);
        }
        // **************************************************************************
        // Conversation .. Chat Box 
        // **************************************************************************
        public static List<Entity.ConversationChatBox> GetConversationChatBoxList(string ModuleName, string KeyValue, string LoginUserID)
        {
            return (new DAL.CommonSQL().GetConversationChatBoxList(ModuleName, KeyValue, LoginUserID));
        }
        public static void AddUpdateConversationChatBox(Entity.ConversationChatBox entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CommonSQL().AddUpdateConversationChatBox(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
