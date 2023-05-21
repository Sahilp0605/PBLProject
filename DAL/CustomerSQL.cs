using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CustomerSQL :BaseSqlManager
    {
        public virtual List<Entity.Customer> GetCustomerBySalesOrder()
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select cust.CustomerID, cust.CustomerName From MST_Customer cust Inner Join SalesOrder so On cust.CustomerID = so.CustomerID;";
            SqlDataReader dr = ExecuteReader(myCommand);
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objLocation = new Entity.Customer();
                objLocation.CustomerID = GetInt64(dr, "CustomerID");
                objLocation.CustomerName = GetTextVale(dr, "CustomerName");

                lstLocation.Add(objLocation);
            }
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Customer> GetCustomerListForDropdown(string pCustomerName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerListForDropdown";
            cmdGet.Parameters.AddWithValue("@CustomerName", pCustomerName);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.Customer> GetCustomerListForComplaintVisit(string pNameOfCustomer)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerListForComplaintVisit";
            cmdGet.Parameters.AddWithValue("@NameOfCustomer", pNameOfCustomer);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                //objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");
                //objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.Customer> GetCustomerListForDropdown(string pCustomerName, string pSearchModule)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerListForDropdown";
            cmdGet.Parameters.AddWithValue("@CustomerName", pCustomerName);
            cmdGet.Parameters.AddWithValue("@SearchModule", pSearchModule);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Customer> GetCustomerListByMobileNo(string ContactNo)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerListByMobileNo";
            cmdGet.Parameters.AddWithValue("@ContactNo", ContactNo);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual List<Entity.Customer> GetFixedLedgerForDropdown()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerFixedLedgerForDropdown";
            cmdGet.Parameters.AddWithValue("@Module", "");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Customer> GetFixedLedgerForDropdown(string pModule)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerFixedLedgerForDropdown";
            cmdGet.Parameters.AddWithValue("@Module", pModule);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Customer> GetCustomerList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerList";
            cmdGet.Parameters.AddWithValue("@CustomerID", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 0);
            cmdGet.Parameters.AddWithValue("@PageSize", 0);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);  
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.BlockCustomer = GetBoolean(dr, "BlockCustomer");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.GSTStateCode = GetTextVale(dr, "GSTStateCode");
                objEntity.GSTStateCode1 = GetTextVale(dr, "GSTStateCode1");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.ShipToCompanyName = GetTextVale(dr, "ShipToCompanyName");
                objEntity.ShipToGSTNo = GetTextVale(dr, "ShipToGSTNo");
                objEntity.Address1 = GetTextVale(dr, "Address1");
                objEntity.Area1 = GetTextVale(dr, "Area1");
                objEntity.Pincode1 = GetTextVale(dr, "Pincode1");
                objEntity.CityCode1 = GetTextVale(dr, "CityCode1");
                objEntity.CityName1 = GetTextVale(dr, "CityName1");
                objEntity.StateCode1 = GetTextVale(dr, "StateCode1");
                objEntity.StateName1 = GetTextVale(dr, "StateName1");
                objEntity.GSTNo = GetTextVale(dr, "GSTNo");
                objEntity.PANNo = GetTextVale(dr, "PANNo");
                objEntity.CINNo = GetTextVale(dr, "CINNo");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.WebsiteAddress = GetTextVale(dr, "WebsiteAddress");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.AnniversaryDate = GetDateTime(dr, "AnniversaryDate");

                //------------NewlyAdded--------------------------
                objEntity.TinVatNo = GetTextVale(dr, "TinVatNo");
                objEntity.TinCstNo = GetTextVale(dr, "TinCstNo");

                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.OrgTypeName = GetTextVale(dr, "OrgType");

                objEntity.ParentID = GetInt64(dr, "ParentID");
                objEntity.ParentName = GetTextVale(dr, "ParentName");

                objEntity.ErpClosing = GetDecimal(dr, "ErpClosing");

                objEntity.CustomerSourceID = GetInt64(dr, "CustomerSourceID");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                objEntity.GenerateInquiry = GetBoolean(dr, "GenerateInquiry");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.OpeningAmount = GetDecimal(dr, "Opening");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                objEntity.ClosingAmount = GetDecimal(dr, "Closing");
                objEntity.PriceListID = GetInt64(dr, "PriceListID");
                //objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                //objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.Customer> GetCustomerList(Int64 CustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerList";
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);   
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.BlockCustomer = GetBoolean(dr, "BlockCustomer");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.GSTStateCode = GetTextVale(dr, "GSTStateCode");
                objEntity.GSTStateCode1 = GetTextVale(dr, "GSTStateCode1");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.CountryCode1 = GetTextVale(dr, "CountryCode1");
                objEntity.CountryName1 = GetTextVale(dr, "CountryName1");
                objEntity.Address1 = GetTextVale(dr, "Address1");
                objEntity.Area1 = GetTextVale(dr, "Area1");
                objEntity.Pincode1 = GetTextVale(dr, "Pincode1");
                objEntity.CityCode1 = GetTextVale(dr, "CityCode1");
                objEntity.CityName1 = GetTextVale(dr, "CityName1");
                objEntity.StateCode1 = GetTextVale(dr, "StateCode1");
                objEntity.StateName1 = GetTextVale(dr, "StateName1");
                objEntity.ShipToCompanyName = GetTextVale(dr, "ShipToCompanyName");
                objEntity.ShipToGSTNo = GetTextVale(dr, "ShipToGSTNo");
                objEntity.GSTNo = GetTextVale(dr, "GSTNo");
                objEntity.PANNo = GetTextVale(dr, "PANNo");
                objEntity.CINNo = GetTextVale(dr, "CINNo");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.WebsiteAddress = GetTextVale(dr, "WebsiteAddress");
                objEntity.Remarks = GetTextVale(dr, "Remarks");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.AnniversaryDate = GetDateTime(dr, "AnniversaryDate");

                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.OrgTypeName = GetTextVale(dr, "OrgType");

                objEntity.ParentID = GetInt64(dr, "ParentID");
                objEntity.ParentName = GetTextVale(dr, "ParentName");
                objEntity.ErpClosing = GetDecimal(dr, "ErpClosing");

                objEntity.CustomerSourceID = GetInt64(dr, "CustomerSourceID");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                objEntity.GenerateInquiry = GetBoolean(dr, "GenerateInquiry");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");

                objEntity.OpeningAmount = GetDecimal(dr, "Opening");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                objEntity.ClosingAmount = GetDecimal(dr, "Closing");
                objEntity.PriceListID = GetInt64(dr, "PriceListID");
                objEntity.CR_Days = GetInt64(dr, "CR_Days");
                objEntity.CR_Limit = GetDecimal(dr, "CR_Limit");

                //------------NewlyAdded--------------------------
                objEntity.TinVatNo = GetTextVale(dr, "TinVatNo");
                objEntity.TinCstNo = GetTextVale(dr, "TinCstNo");

                //objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                //objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Customer> GetCustomerList(Int64 CustomerID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerList";
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
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
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.CustomerSourceID = GetInt64(dr, "CustomerSourceID");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                objEntity.BlockCustomer = GetBoolean(dr, "BlockCustomer");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.GSTStateCode = GetTextVale(dr, "GSTStateCode");
                objEntity.GSTStateCode1 = GetTextVale(dr, "GSTStateCode1");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.ShipToCompanyName = GetTextVale(dr, "ShipToCompanyName");
                objEntity.ShipToGSTNo = GetTextVale(dr, "ShipToGSTNo");
                objEntity.Address1 = GetTextVale(dr, "Address1");
                objEntity.Area1 = GetTextVale(dr, "Area1");
                objEntity.Pincode1 = GetTextVale(dr, "Pincode1");
                objEntity.CityCode1 = GetTextVale(dr, "CityCode1");
                objEntity.CityName1 = GetTextVale(dr, "CityName1");
                objEntity.StateCode1 = GetTextVale(dr, "StateCode1");
                objEntity.StateName1 = GetTextVale(dr, "StateName1");
                objEntity.GSTNo = GetTextVale(dr, "GSTNo");
                   objEntity.PANNo = GetTextVale(dr, "PANNo");
                objEntity.CINNo = GetTextVale(dr, "CINNo");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.WebsiteAddress = GetTextVale(dr, "WebsiteAddress");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedEmployeeName = GetTextVale(dr, "CreatedEmployeeName");
                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.AnniversaryDate = GetDateTime(dr, "AnniversaryDate");

                //------------NewlyAdded--------------------------
                objEntity.TinVatNo = GetTextVale(dr, "TinVatNo");
                objEntity.TinCstNo = GetTextVale(dr, "TinCstNo");

                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.OrgTypeName = GetTextVale(dr, "OrgType");

                objEntity.ParentID = GetInt64(dr, "ParentID");
                objEntity.ParentName = GetTextVale(dr, "ParentName");
                objEntity.ErpClosing = GetDecimal(dr, "ErpClosing");

                objEntity.GenerateInquiry = GetBoolean(dr, "GenerateInquiry");
                objEntity.OpeningAmount = GetDecimal(dr, "Opening");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                objEntity.ClosingAmount = GetDecimal(dr, "Closing");
                objEntity.PriceListID = GetInt64(dr, "PriceListID");
                objEntity.CR_Days = GetInt64(dr, "CR_Days");
                objEntity.CR_Limit = GetDecimal(dr, "CR_Limit");
                //objEntity.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                //objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Customer> GetCustomerList(string pCustomerName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerListByName";
            cmdGet.Parameters.AddWithValue("@CustomerName", pCustomerName);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 99999);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Customer> lstObject = new List<Entity.Customer>();
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.BlockCustomer = GetBoolean(dr, "BlockCustomer");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


        public virtual List<Entity.Customer> GetCustomerSearchInfo(string pCustName, string pType, string pSource, string pContact, string pEmail, string pState, string pCity)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerSearchInfo";
            cmdGet.Parameters.AddWithValue("@CustomerName", pCustName);
            cmdGet.Parameters.AddWithValue("@CustomerType", pType);
            cmdGet.Parameters.AddWithValue("@CustomerSource", pSource);
            cmdGet.Parameters.AddWithValue("@ContactNo", pContact);
            cmdGet.Parameters.AddWithValue("@EmailAddress", pEmail);
            cmdGet.Parameters.AddWithValue("@State", pState);
            cmdGet.Parameters.AddWithValue("@City", pCity);
            cmdGet.Parameters.AddWithValue("@LoginUserID", "admin");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.BlockCustomer = GetBoolean(dr, "BlockCustomer");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");

                objEntity.Address1 = GetTextVale(dr, "Address1");
                objEntity.Area1 = GetTextVale(dr, "Area1");
                objEntity.Pincode1 = GetTextVale(dr, "Pincode1");
                objEntity.CityCode1 = GetTextVale(dr, "CityCode1");
                objEntity.CityName1 = GetTextVale(dr, "CityName1");
                objEntity.StateCode1 = GetTextVale(dr, "StateCode1");
                objEntity.StateName1 = GetTextVale(dr, "StateName1");

                objEntity.GSTNo = GetTextVale(dr, "GSTNo");
                objEntity.PANNo = GetTextVale(dr, "PANNo");
                objEntity.CINNo = GetTextVale(dr, "CINNo");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.WebsiteAddress = GetTextVale(dr, "WebsiteAddress");

                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.AnniversaryDate = GetDateTime(dr, "AnniversaryDate");

                //------------NewlyAdded--------------------------
                objEntity.TinVatNo = GetTextVale(dr, "TinVatNo");
                objEntity.TinCstNo = GetTextVale(dr, "TinCstNo");

                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.OrgTypeName = GetTextVale(dr, "OrgType");

                objEntity.ParentID = GetInt64(dr, "ParentID");
                objEntity.ParentName = GetTextVale(dr, "ParentName");
                objEntity.CreatedEmployee = GetTextVale(dr, "CreatedEmployee");
                objEntity.ErpClosing = GetDecimal(dr, "ErpClosing");

                objEntity.CustomerSourceID = GetInt64(dr, "CustomerSourceID");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                objEntity.GenerateInquiry = GetBoolean(dr, "GenerateInquiry");

                objEntity.OpeningAmount = GetDecimal(dr, "Opening");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                objEntity.ClosingAmount = GetDecimal(dr, "Closing");
                objEntity.PriceListID = GetInt64(dr, "PriceListID");
                objEntity.CR_Days = GetInt64(dr, "CR_Days");
                objEntity.CR_Limit = GetDecimal(dr, "CR_Limit");
                lstLocation.Add(objEntity);
            }
            dr.Close();

            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Customer> GetCustomerList(string pLoginUserID, Int64 pMonth, Int64 pYear, string pFromDate = null, string pToDate = null)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", pFromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", pToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.BlockCustomer = GetBoolean(dr, "BlockCustomer");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.Area = GetTextVale(dr, "Area");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.CityCode = GetTextVale(dr, "CityCode");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.StateCode = GetTextVale(dr, "StateCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.CountryName = GetTextVale(dr, "CountryName");

                objEntity.Address1 = GetTextVale(dr, "Address1");
                objEntity.Area1 = GetTextVale(dr, "Area1");
                objEntity.Pincode1 = GetTextVale(dr, "Pincode1");
                objEntity.CityCode1 = GetTextVale(dr, "CityCode1");
                objEntity.CityName1 = GetTextVale(dr, "CityName1");
                objEntity.StateCode1 = GetTextVale(dr, "StateCode1");
                objEntity.StateName1 = GetTextVale(dr, "StateName1");

                objEntity.GSTNo = GetTextVale(dr, "GSTNo");
                objEntity.PANNo = GetTextVale(dr, "PANNo");
                objEntity.CINNo = GetTextVale(dr, "CINNo");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.WebsiteAddress = GetTextVale(dr, "WebsiteAddress");
                //objEntity.Remarks = GetTextVale(dr, "Remarks");


                objEntity.BirthDate = GetDateTime(dr, "BirthDate");
                objEntity.AnniversaryDate = GetDateTime(dr, "AnniversaryDate");

                //------------NewlyAdded--------------------------
                objEntity.TinVatNo = GetTextVale(dr, "TinVatNo");
                objEntity.TinCstNo = GetTextVale(dr, "TinCstNo");

                objEntity.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objEntity.OrgTypeName = GetTextVale(dr, "OrgType");

                objEntity.ParentID = GetInt64(dr, "ParentID");
                objEntity.ParentName = GetTextVale(dr, "ParentName");
                objEntity.CreatedEmployee = GetTextVale(dr, "CreatedEmployee");
                objEntity.ErpClosing = GetDecimal(dr, "ErpClosing");

                objEntity.CustomerSourceID = GetInt64(dr, "CustomerSourceID");
                objEntity.CustomerSourceName = GetTextVale(dr, "CustomerSourceName");
                objEntity.GenerateInquiry = GetBoolean(dr, "GenerateInquiry");

                objEntity.OpeningAmount = GetDecimal(dr, "Opening");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                objEntity.ClosingAmount = GetDecimal(dr, "Closing");
                objEntity.PriceListID = GetInt64(dr, "PriceListID");
                objEntity.CR_Days = GetInt64(dr, "CR_Days");
                objEntity.CR_Limit = GetDecimal(dr, "CR_Limit");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Customer> GetCustomerLedgerList(Int64 pCustomerID, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerLedgerList";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
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

        public virtual List<Entity.Customer> GetCustomerDetailLedgerList(Int64 pCustomerID, string pLoginUserID)
        {
            List<Entity.Customer> lstLocation = new List<Entity.Customer>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerDetailLedger";
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Customer objEntity = new Entity.Customer();
                objEntity.KeyID = GetInt64(dr, "pkID");
                objEntity.TransDate = GetDateTime(dr, "TransDate");
                objEntity.Description = GetTextVale(dr, "Description");
                objEntity.Remarks = GetTextVale(dr, "Remarks");
                objEntity.TransCategory = GetTextVale(dr, "TransCategory");
                objEntity.TransType = GetTextVale(dr, "TransType");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.DebitAmount = GetDecimal(dr, "Debit");
                objEntity.CreditAmount = GetDecimal(dr, "Credit");
                lstLocation.Add(objEntity);
            }
            dr.Close();

            ForceCloseConncetion();
            return lstLocation;
        }        
        // ============================= Insert & Update
        public virtual void AddUpdateCustomer(Entity.Customer objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Customer_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CustomerName", objEntity.CustomerName);
            cmdAdd.Parameters.AddWithValue("@CustomerType", objEntity.CustomerType);
            cmdAdd.Parameters.AddWithValue("@BlockCustomer", objEntity.BlockCustomer);
            
            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@Area", objEntity.Area);
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
            cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);

            cmdAdd.Parameters.AddWithValue("@ShipToCompanyName", objEntity.ShipToCompanyName);
            cmdAdd.Parameters.AddWithValue("@ShipToGSTNo", objEntity.ShipToGSTNo);
            cmdAdd.Parameters.AddWithValue("@Address1", objEntity.Address1);
            cmdAdd.Parameters.AddWithValue("@Area1", objEntity.Area1);
            cmdAdd.Parameters.AddWithValue("@CountryCode1", objEntity.CountryCode1);
            cmdAdd.Parameters.AddWithValue("@StateCode1", objEntity.StateCode1);
            cmdAdd.Parameters.AddWithValue("@CityCode1", objEntity.CityCode1);
            cmdAdd.Parameters.AddWithValue("@Pincode1", objEntity.Pincode1);

            cmdAdd.Parameters.AddWithValue("@GSTNo", objEntity.GSTNo);
            cmdAdd.Parameters.AddWithValue("@PANNo", objEntity.PANNo);
            cmdAdd.Parameters.AddWithValue("@CINNo", objEntity.CINNo);
            cmdAdd.Parameters.AddWithValue("@ContactNo1", objEntity.ContactNo1);
            cmdAdd.Parameters.AddWithValue("@ContactNo2", objEntity.ContactNo2);
            cmdAdd.Parameters.AddWithValue("@EmailAddress", objEntity.EmailAddress);
            cmdAdd.Parameters.AddWithValue("@WebsiteAddress", objEntity.WebsiteAddress);
            cmdAdd.Parameters.AddWithValue("@BirthDate", objEntity.BirthDate);
            cmdAdd.Parameters.AddWithValue("@AnniversaryDate", objEntity.AnniversaryDate);
            cmdAdd.Parameters.AddWithValue("@TinVatNo", objEntity.TinVatNo);//Newly Added
            cmdAdd.Parameters.AddWithValue("@TinCstNo", objEntity.TinCstNo);//Newly Added
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);

            cmdAdd.Parameters.AddWithValue("@Opening", objEntity.OpeningAmount);
            cmdAdd.Parameters.AddWithValue("@Debit", objEntity.DebitAmount);
            cmdAdd.Parameters.AddWithValue("@Credit", objEntity.CreditAmount);
            cmdAdd.Parameters.AddWithValue("@CR_Limit", objEntity.CR_Limit);
            cmdAdd.Parameters.AddWithValue("@CR_Days", objEntity.CR_Days);
            cmdAdd.Parameters.AddWithValue("@Closing", objEntity.ClosingAmount);
            cmdAdd.Parameters.AddWithValue("@PriceListID", objEntity.PriceListID);
            cmdAdd.Parameters.AddWithValue("@CustomerSourceID", objEntity.CustomerSourceID);
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
        public virtual void AddUpdateCustomerQuick(Entity.Customer objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Customer_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CustomerName", objEntity.CustomerName);
            cmdAdd.Parameters.AddWithValue("@CustomerType", objEntity.CustomerType);
            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@Area", objEntity.Area);
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
            cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);
            cmdAdd.Parameters.AddWithValue("@ContactNo1", objEntity.ContactNo1);
            cmdAdd.Parameters.AddWithValue("@ContactNo2", objEntity.ContactNo2);
            cmdAdd.Parameters.AddWithValue("@EmailAddress", objEntity.EmailAddress);
            cmdAdd.Parameters.AddWithValue("@CustomerSourceID", objEntity.CustomerSourceID);
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
        public virtual void AddUpdateCustomerUPDOWN(Entity.Customer objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Customer_INS_UPD_UPDOWN";
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CustomerName", objEntity.CustomerName);
            cmdAdd.Parameters.AddWithValue("@CustomerType", objEntity.CustomerType);
            cmdAdd.Parameters.AddWithValue("@PriceListID", objEntity.PriceListID);
            cmdAdd.Parameters.AddWithValue("@CityName", objEntity.CityName);
            cmdAdd.Parameters.AddWithValue("@StateName", objEntity.StateName);
            cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("@Area", objEntity.Area);
            cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);
            cmdAdd.Parameters.AddWithValue("@ContactNo1", objEntity.ContactNo1);
            cmdAdd.Parameters.AddWithValue("@ContactNo2", objEntity.ContactNo2);
            cmdAdd.Parameters.AddWithValue("@EmailAddress", objEntity.EmailAddress);
            cmdAdd.Parameters.AddWithValue("@GSTNo", objEntity.GSTNo);
            cmdAdd.Parameters.AddWithValue("@PANNo", objEntity.PANNo);
            cmdAdd.Parameters.AddWithValue("@CINNo", objEntity.CINNo);
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

        public virtual void DeleteCustomer(Int64 CustomerID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Customer_DEL";
            cmdDel.Parameters.AddWithValue("@CustomerID", CustomerID);
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

        // ============================= Insert & Update
        public virtual void AddUpdateCustomerInstant(Entity.Customer objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Customer_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@CustomerName", objEntity.CustomerName);
            cmdAdd.Parameters.AddWithValue("@CustomerType", objEntity.CustomerType);
            cmdAdd.Parameters.AddWithValue("@OrgTypeCode", objEntity.OrgTypeCode);
            cmdAdd.Parameters.AddWithValue("@ParentID", objEntity.ParentID);
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

        // **********************************************************************
        // Customer Documents
        // **********************************************************************
        public virtual List<Entity.Documents> GetCustomerDocumentsList(Int64 pkID, Int64 pCustomerID)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "CustomerDocumentsList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@CustomerID", pCustomerID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Documents> lstLocation = new List<Entity.Documents>();
            while (dr.Read())
            {
                Entity.Documents objLocation = new Entity.Documents();
                objLocation.pkID = GetInt64(dr, "pkID");
                objLocation.EmployeeID = GetInt64(dr, "CustomerID");
                objLocation.EmployeeName = GetTextVale(dr, "CustomerName");
                objLocation.FileName = GetTextVale(dr, "Name");
                objLocation.CreatedBy = GetTextVale(dr, "CreatedBy");
                lstLocation.Add(objLocation);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateCustomerDocuments(Int64 pCustomerID, string pFilename, string pLoginUserID, out int ReturnCode, out string ReturnMsg)
        {
            try
            {
                string query = "insert into MST_Customer_Documents (CustomerID, Name, CreatedBy, CreatedDate)" + " values (@CustomerID, @Name, @LoginUserID, GETDATE())";
                SqlCommand cmdAdd = new SqlCommand(query);
                cmdAdd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = pCustomerID;
                cmdAdd.Parameters.Add("@Name", SqlDbType.VarChar).Value = pFilename;
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

        public virtual void DeleteCustomerDocuments(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CustomerDocuments_DEL";
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

        public virtual void DeleteCustomerDocumentsByCustomerId(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "CustomerDocumentsByCustomerId_DEL";
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
    }
}
