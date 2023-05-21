using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class CustomerContactsMgmt
    {
        // ----------------------------------------------------------------
        // Customer Contacts
        // ----------------------------------------------------------------

        public static List<Entity.CustomerContacts> GetCustomerContactsList(String ContactPerosn, Int64 CustomerID)
        {
            return (new DAL.CustomerContactsSQL().GetCustomerContactsList(ContactPerosn, CustomerID));
        }
        public static DataTable GetCustomerContactsDetail(Int64 pCustomerID)
        {
            return (new DAL.CustomerContactsSQL().GetCustomerContactsDetail(pCustomerID));
        }

        public static DataTable GetCustomerContacts(Int64 pCustomerID,String PersonName)
        {
            return (new DAL.CustomerContactsSQL().GetCustomerContacts(pCustomerID,PersonName));
        }

        public static List<Entity.CustomerContacts> GetCustomerContactsList()
        {
            return (new DAL.CustomerContactsSQL().GetCustomerContactsList());
        }

        public static List<Entity.CustomerContacts> GetCustomerContactsList(Int64 CustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CustomerContactsSQL().GetCustomerContactsList(CustomerID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCustomerContacts(Entity.CustomerContacts entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerContactsSQL().AddUpdateCustomerContacts(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerContacts(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerContactsSQL().DeleteCustomerContacts(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCustomerContactsByCustomer(Int64 CustID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerContactsSQL().DeleteCustomerContactsByCustomer(CustID, out ReturnCode, out ReturnMsg);
        }
        // ----------------------------------------------------------------
        // Customer Price List
        // ----------------------------------------------------------------
        public static DataTable GetCustomerPrice(Int64 pCustomerID)
        {
            return (new DAL.CustomerContactsSQL().GetCustomerPrice(pCustomerID));
        }
        public static void AddUpdateCustomerPrice(Entity.CustomerPriceList entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerContactsSQL().AddUpdateCustomerPrice(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteCustomerPrice(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerContactsSQL().DeleteCustomerPrice(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteCustomerPriceByCustomer(Int64 CustID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CustomerContactsSQL().DeleteCustomerPriceByCustomer(CustID, out ReturnCode, out ReturnMsg);
        }
    }
}
