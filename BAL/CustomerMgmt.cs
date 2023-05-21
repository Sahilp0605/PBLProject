using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CustomerMgmt
    {
        public static List<Entity.Customer> GetCustomerBySalesOrder()
        {
            return (new DAL.CustomerSQL().GetCustomerBySalesOrder());
        }

        public static List<Entity.Customer> GetCustomerListForDropdown(string pCustomerName)   
        {
            return (new DAL.CustomerSQL().GetCustomerListForDropdown(pCustomerName));
        }
        public static List<Entity.Customer> GetCustomerListForComplaintVisit(string pNameOfCustomer)
        {
            return (new DAL.CustomerSQL().GetCustomerListForComplaintVisit(pNameOfCustomer));
        }
        public static List<Entity.Customer> GetCustomerListForDropdown(string pCustomerName, string pSearchModule)
        {
            return (new DAL.CustomerSQL().GetCustomerListForDropdown(pCustomerName, pSearchModule));
        }
        public static List<Entity.Customer> GetCustomerListByMobileNo(string ContactNo)
        {
            return (new DAL.CustomerSQL().GetCustomerListByMobileNo(ContactNo));
        }
        public static List<Entity.Customer> GetFixedLedgerForDropdown()
        {
            return (new DAL.CustomerSQL().GetFixedLedgerForDropdown());
        }
        
        public static List<Entity.Customer> GetFixedLedgerForDropdown(string pModule)
        {
            return (new DAL.CustomerSQL().GetFixedLedgerForDropdown(pModule));
        }

        public static List<Entity.Customer> GetCustomerList()
        {
            return (new DAL.CustomerSQL().GetCustomerList());
        }

        public static List<Entity.Customer> GetCustomerList(Int64 CustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CustomerSQL().GetCustomerList(CustomerID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Customer> GetCustomerList(Int64 CustomerID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CustomerSQL().GetCustomerList(CustomerID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Customer> GetCustomerList(string pCustomerName)
        {
            return (new DAL.CustomerSQL().GetCustomerList(pCustomerName));
        }

        public static List<Entity.Customer> GetCustomerSearchInfo(string pCustName, string pType, string pSource, string pContact, string pEmail, string pState, string pCity)
        {
            return (new DAL.CustomerSQL().GetCustomerSearchInfo(pCustName, pType, pSource, pContact, pEmail, pState, pCity));
        }

        public static List<Entity.Customer> GetCustomerList( string pLoginUserID, Int64 pMonth, Int64 pYear, string pFromDate = null, string pToDate = null)
        {
            return (new DAL.CustomerSQL().GetCustomerList(pLoginUserID, pMonth, pYear, pFromDate, pToDate));
        }

        public static List<Entity.Customer> GetCustomerLedgerList(Int64 pCustID, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.CustomerSQL().GetCustomerLedgerList(pCustID, pLoginUserID, pMonth, pYear));
        }

        public static List<Entity.Customer> GetCustomerDetailLedgerList(Int64 pCustID, string pLoginUserID)
        {
            return (new DAL.CustomerSQL().GetCustomerDetailLedgerList(pCustID, pLoginUserID));
        }

        public static void AddUpdateCustomer(Entity.Customer entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().AddUpdateCustomer(entity, out ReturnCode, out ReturnMsg);
        }

        public static void AddUpdateCustomerQuick(Entity.Customer entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().AddUpdateCustomerQuick(entity, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateCustomerUPDOWN(Entity.Customer entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().AddUpdateCustomerUPDOWN(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomer(Int64 CustomerID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().DeleteCustomer(CustomerID, out ReturnCode, out ReturnMsg);
        }


        public static void AddUpdateCustomerInstant(Entity.Customer entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().AddUpdateCustomerInstant(entity, out ReturnCode, out ReturnMsg);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // >>>>>>>>>>> Customer Documents 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.Documents> GetCustomerDocumentsList(Int64 pkID, Int64 CustomerID)
        {
            return (new DAL.CustomerSQL().GetCustomerDocumentsList(pkID, CustomerID));
        }
        public static void AddUpdateCustomerDocuments(Int64 pCustomerID, string pFilename, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().AddUpdateCustomerDocuments(pCustomerID, pFilename, pLoginUserID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().DeleteCustomerDocuments(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerDocumentsByCustomerId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerSQL().DeleteCustomerDocumentsByCustomerId(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
