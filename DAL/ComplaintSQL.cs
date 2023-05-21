using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ComplaintSQL:BaseSqlManager
    {
        public virtual List<Entity.Complaint> GetComplaintList()
        {
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.Text;
            cmdGet.CommandText = "Select pkID,ComplaintNo From Complaint";
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Complaint> GetComplaintList(Int64 pkID, string LoginUserID)
        {
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ComplaintList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.ComplaintNoString = "Complaint # : " + GetInt64(dr, "pkID").ToString();
                objEntity.ComplaintDate = GetDateTime(dr, "ComplaintDate");
                objEntity.ComplaintDays = GetInt64(dr, "ComplaintDays");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PreferredDate = GetDateTime(dr, "PreferredDate");
                objEntity.TimeFrom = GetTextVale(dr, "TimeFrom");
                objEntity.TimeTo = GetTextVale(dr, "TimeTo");
                objEntity.ComplaintStatus = GetTextVale(dr, "ComplaintStatus");
                objEntity.ComplaintType = GetTextVale(dr, "ComplaintType");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                objEntity.ScheduleDate = GetDateTime(dr, "ScheduleDate");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.SrNo = GetTextVale(dr, "SrNo");

                //---------------------------------------------------
                objEntity.EmailId = GetTextVale(dr, "EmailId");
                objEntity.CustomerEmpName = GetTextVale(dr, "CustomerEmpName");
                objEntity.CustmoreMobileNo = GetTextVale(dr, "CustmoreMobileNo");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.WorkOderNo = GetTextVale(dr, "WorkOderNo");
                objEntity.DateOfPurchase = GetDateTime(dr, "DateOfPurchase");

                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");

                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.SiteCoordinatorName = GetTextVale(dr, "SiteCoordinatorName");
                objEntity.SiteMobileNo = GetTextVale(dr, "SitemobileNo");

                objEntity.ConvinientTimeSlot = GetTextVale(dr, "ConvinientTimeSlot");
                objEntity.ConvinientDate = GetDateTime(dr, "ConvinientDate");
                objEntity.PhotoOfDefectProduct = GetTextVale(dr, "PhotoOfDefectProduct");
                objEntity.PhotoOfPanel = GetTextVale(dr, "PhotoOfPanel");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");


                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Complaint> GetComplaintList(Int64 pkID, Int64 CustomerID, string ComplaintStatus, string LoginUserID)
        {
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ComplaintList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ComplaintStatus", ComplaintStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.ComplaintNoString = "Complaint # : " + GetInt64(dr, "pkID").ToString();
                objEntity.ComplaintDate = GetDateTime(dr, "ComplaintDate");
                objEntity.ComplaintDays = GetInt64(dr, "ComplaintDays");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PreferredDate = GetDateTime(dr, "PreferredDate");
                objEntity.TimeFrom = GetTextVale(dr, "TimeFrom");
                objEntity.TimeTo = GetTextVale(dr, "TimeTo");
                objEntity.ComplaintStatus = GetTextVale(dr, "ComplaintStatus");
                objEntity.ComplaintType = GetTextVale(dr, "ComplaintType");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                objEntity.ScheduleDate = GetDateTime(dr, "ScheduleDate");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.SrNo = GetTextVale(dr, "SrNo");

                //---------------------------------------------------
                objEntity.EmailId = GetTextVale(dr, "EmailId");
                objEntity.CustomerEmpName = GetTextVale(dr, "CustomerEmpName");
                objEntity.CustmoreMobileNo = GetTextVale(dr, "CustmoreMobileNo");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.WorkOderNo = GetTextVale(dr, "WorkOderNo");
                objEntity.DateOfPurchase = GetDateTime(dr, "DateOfPurchase");

                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");

                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.SiteCoordinatorName = GetTextVale(dr, "SiteCoordinatorName");
                objEntity.SiteMobileNo = GetTextVale(dr, "SitemobileNo");

                objEntity.ConvinientTimeSlot = GetTextVale(dr, "ConvinientTimeSlot");
                objEntity.ConvinientDate = GetDateTime(dr, "ConvinientDate");
                objEntity.PhotoOfDefectProduct = GetTextVale(dr, "PhotoOfDefectProduct");
                objEntity.PhotoOfPanel = GetTextVale(dr, "PhotoOfPanel");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");


                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Complaint> GetComplaintList(Int64 pkID, Int64 CustomerID, string ComplaintStatus, Int64 pMon, Int64 pYear, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ComplaintList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ComplaintStatus", ComplaintStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMon);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
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
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.ComplaintNoString = "Complaint # : " + GetInt64(dr, "pkID").ToString();
                objEntity.ComplaintDateString = GetTextVale(dr, "ComplaintNo") + " : " + GetDateTime(dr, "ComplaintDate").ToString("dd-MM-yyyy");
                objEntity.ComplaintDate = GetDateTime(dr, "ComplaintDate");
                objEntity.ComplaintDays = GetInt64(dr, "ComplaintDays");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PreferredDate = GetDateTime(dr, "PreferredDate");
                objEntity.TimeFrom = GetTextVale(dr, "TimeFrom");
                objEntity.TimeTo = GetTextVale(dr, "TimeTo");
                objEntity.ComplaintStatus = GetTextVale(dr, "ComplaintStatus");
                objEntity.ComplaintType = GetTextVale(dr, "ComplaintType");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                objEntity.ScheduleDate = GetDateTime(dr, "ScheduleDate");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.SrNo = GetTextVale(dr, "SrNo");

                //---------------------------------------------------
                objEntity.EmailId = GetTextVale(dr, "EmailId");
                objEntity.CustomerEmpName = GetTextVale(dr, "CustomerEmpName");
                objEntity.CustmoreMobileNo = GetTextVale(dr, "CustmoreMobileNo");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.WorkOderNo = GetTextVale(dr, "WorkOderNo");
                objEntity.DateOfPurchase = GetDateTime(dr, "DateOfPurchase");

                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");

                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.SiteCoordinatorName = GetTextVale(dr, "SiteCoordinatorName");
                objEntity.SiteMobileNo = GetTextVale(dr, "SitemobileNo");

                objEntity.ConvinientTimeSlot = GetTextVale(dr, "ConvinientTimeSlot");
                objEntity.ConvinientDate = GetDateTime(dr, "ConvinientDate");
                objEntity.PhotoOfDefectProduct = GetTextVale(dr, "PhotoOfDefectProduct");
                objEntity.PhotoOfPanel = GetTextVale(dr, "PhotoOfPanel");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            return lstLocation;
        }

        public virtual List<Entity.Complaint> GetComplaintList(Int64 CustomerID, string ComplaintStatus, string LoginUserID)
        {
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ComplaintList";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ComplaintStatus", ComplaintStatus);
            cmdGet.Parameters.AddWithValue("@Month", 0);
            cmdGet.Parameters.AddWithValue("@Year", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.ComplaintNoString = "Complaint # : " + GetInt64(dr, "pkID").ToString();
                objEntity.ComplaintDate = GetDateTime(dr, "ComplaintDate");
                objEntity.ComplaintDays = GetInt64(dr, "ComplaintDays");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PreferredDate = GetDateTime(dr, "PreferredDate");
                objEntity.TimeFrom = GetTextVale(dr, "TimeFrom");
                objEntity.TimeTo = GetTextVale(dr, "TimeTo");
                objEntity.ComplaintStatus = GetTextVale(dr, "ComplaintStatus");
                objEntity.ComplaintType = GetTextVale(dr, "ComplaintType");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                objEntity.ScheduleDate = GetDateTime(dr, "ScheduleDate");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.SrNo = GetTextVale(dr, "SrNo");

                //---------------------------------------------------
                objEntity.EmailId = GetTextVale(dr, "EmailId");
                objEntity.CustomerEmpName = GetTextVale(dr, "CustomerEmpName");
                objEntity.CustmoreMobileNo = GetTextVale(dr, "CustmoreMobileNo");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.WorkOderNo = GetTextVale(dr, "WorkOderNo");
                objEntity.DateOfPurchase = GetDateTime(dr, "DateOfPurchase");

                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");

                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.SiteCoordinatorName = GetTextVale(dr, "SiteCoordinatorName");
                objEntity.SiteMobileNo = GetTextVale(dr, "SitemobileNo");

                objEntity.ConvinientTimeSlot = GetTextVale(dr, "ConvinientTimeSlot");
                objEntity.ConvinientDate = GetDateTime(dr, "ConvinientDate");
                objEntity.PhotoOfDefectProduct = GetTextVale(dr, "PhotoOfDefectProduct");
                objEntity.PhotoOfPanel = GetTextVale(dr, "PhotoOfPanel");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Complaint> GetComplaintListByComplaintNo(Int64 ComplaintNo, string ComplaintStatus, string LoginUserID)
        {
            List<Entity.Complaint> lstLocation = new List<Entity.Complaint>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ComplaintListByComplaintNo";
            cmdGet.Parameters.AddWithValue("@pkID", 0);
            cmdGet.Parameters.AddWithValue("@ComplaintNo", ComplaintNo);
            cmdGet.Parameters.AddWithValue("@ComplaintStatus", ComplaintStatus);
            cmdGet.Parameters.AddWithValue("@Month", 0);
            cmdGet.Parameters.AddWithValue("@Year", 0);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", "");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 10000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Complaint objEntity = new Entity.Complaint();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetTextVale(dr, "ComplaintNo");
                objEntity.ComplaintNoString = "Complaint # : " + GetInt64(dr, "pkID").ToString();
                objEntity.ComplaintDate = GetDateTime(dr, "ComplaintDate");
                objEntity.ComplaintDays = GetInt64(dr, "ComplaintDays");
                objEntity.ReferenceNo = GetTextVale(dr, "ReferenceNo");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.PreferredDate = GetDateTime(dr, "PreferredDate");
                objEntity.TimeFrom = GetTextVale(dr, "TimeFrom");
                objEntity.TimeTo = GetTextVale(dr, "TimeTo");
                objEntity.ComplaintStatus = GetTextVale(dr, "ComplaintStatus");
                objEntity.ComplaintType = GetTextVale(dr, "ComplaintType");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                objEntity.ScheduleDate = GetDateTime(dr, "ScheduleDate");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.SrNo = GetTextVale(dr, "SrNo");

                //---------------------------------------------------
                objEntity.EmailId = GetTextVale(dr, "EmailId");
                objEntity.CustomerEmpName = GetTextVale(dr, "CustomerEmpName");
                objEntity.CustmoreMobileNo = GetTextVale(dr, "CustmoreMobileNo");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.WorkOderNo = GetTextVale(dr, "WorkOderNo");
                objEntity.DateOfPurchase = GetDateTime(dr, "DateOfPurchase");

                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");

                objEntity.SiteAddress = GetTextVale(dr, "SiteAddress");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.SiteCoordinatorName = GetTextVale(dr, "SiteCoordinatorName");
                objEntity.SiteMobileNo = GetTextVale(dr, "SitemobileNo");

                objEntity.ConvinientTimeSlot = GetTextVale(dr, "ConvinientTimeSlot");
                objEntity.ConvinientDate = GetDateTime(dr, "ConvinientDate");
                objEntity.PhotoOfDefectProduct = GetTextVale(dr, "PhotoOfDefectProduct");
                objEntity.PhotoOfPanel = GetTextVale(dr, "PhotoOfPanel");


                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateComplaint(Entity.Complaint objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnComplaintNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Complaint_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
            cmdAdd.Parameters.AddWithValue("@ComplaintDate", objEntity.ComplaintDate);
            cmdAdd.Parameters.AddWithValue("@ReferenceNo", objEntity.ReferenceNo);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNotes", objEntity.ComplaintNotes);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@ComplaintStatus", objEntity.ComplaintStatus);
            cmdAdd.Parameters.AddWithValue("@ComplaintType", objEntity.ComplaintType);
            cmdAdd.Parameters.AddWithValue("@PreferredDate", objEntity.PreferredDate);
            cmdAdd.Parameters.AddWithValue("@TimeFrom", objEntity.TimeFrom);
            cmdAdd.Parameters.AddWithValue("@TimeTo", objEntity.TimeTo);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);

            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@SrNo", objEntity.SrNo);

            //-------------------------------------
            cmdAdd.Parameters.AddWithValue("@EmailId", objEntity.EmailId);
            cmdAdd.Parameters.AddWithValue("@CustomerEmpName", objEntity.CustomerEmpName);
            cmdAdd.Parameters.AddWithValue("@Designation", objEntity.Designation);
            cmdAdd.Parameters.AddWithValue("@CustmoreMobileNo", objEntity.CustmoreMobileNo);

            cmdAdd.Parameters.AddWithValue("@WorkOderNo", objEntity.WorkOderNo);
            cmdAdd.Parameters.AddWithValue("@DateOfPurchase", objEntity.DateOfPurchase.Year < 1900 ? DateTime.Now : objEntity.DateOfPurchase );

            cmdAdd.Parameters.AddWithValue("@PanelSRNo", objEntity.PanelSRNo);
            cmdAdd.Parameters.AddWithValue("@ProductSRNo", objEntity.ProductSRNo);

            cmdAdd.Parameters.AddWithValue ("@SiteAddress",  objEntity.SiteAddress);
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
           cmdAdd.Parameters.AddWithValue ("@Pincode",  objEntity.Pincode);
            cmdAdd.Parameters.AddWithValue("@SiteCoordinatorName", objEntity.SiteCoordinatorName);
            cmdAdd.Parameters.AddWithValue("@SiteMobileNo", objEntity.SiteMobileNo);

            cmdAdd.Parameters.AddWithValue("@ConvinientTimeSlot", objEntity.ConvinientTimeSlot);
            cmdAdd.Parameters.AddWithValue("@ConvinientDate", objEntity.ConvinientDate.Year < 1900 ? DateTime.Now : objEntity.ConvinientDate);
            cmdAdd.Parameters.AddWithValue ("@PhotoOfDefectProduct",  objEntity.PhotoOfDefectProduct);
            cmdAdd.Parameters.AddWithValue ("@PhotoOfPanel",  objEntity.PhotoOfPanel);
            cmdAdd.Parameters.AddWithValue("@NameOfCustomer", objEntity.NameOfCustomer);

            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnComplaintNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnComplaintNo = cmdAdd.Parameters["@ReturnComplaintNo"].Value.ToString();
            ForceCloseConncetion();
        }

        // ============================= Insert & Update
        public virtual void AddUpdateComplaintQuick(Entity.Complaint objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnComplaintNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Complaint_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
            cmdAdd.Parameters.AddWithValue("@ComplaintDate", objEntity.ComplaintDate);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNotes", objEntity.ComplaintNotes);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@ComplaintStatus", objEntity.ComplaintStatus);
            cmdAdd.Parameters.AddWithValue("@ComplaintType", objEntity.ComplaintType);
            cmdAdd.Parameters.AddWithValue("@ClosingRemarks", objEntity.ClosingRemarks);
            cmdAdd.Parameters.AddWithValue("@ScheduleDate", objEntity.ScheduleDate);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnComplaintNo", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnComplaintNo = cmdAdd.Parameters["@ReturnComplaintNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteComplaint(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Complaint_DEL";
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
        // ===========================================================================
        // Complaint Visit (Response) 
        // ===========================================================================
        public virtual List<Entity.ComplaintVisit> GetComplaintVisitList(Int64 pkID, Int64 ComplaintNo, Int64 EmployeeID, Int64 CustomerID, string ComplaintStatus, string SearchKey, string LoginUserID)
        {
            List<Entity.ComplaintVisit> lstLocation = new List<Entity.ComplaintVisit>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ComplaintVisitList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@ComplaintNo", ComplaintNo);
            cmdGet.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@ComplaintStatus", ComplaintStatus);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ComplaintVisit objEntity = new Entity.ComplaintVisit();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ComplaintNo = GetInt64(dr, "ComplaintNo");
                objEntity.ComplaintNoString = GetTextVale(dr, "ComplaintNoString");
                objEntity.ComplaintDate = GetDateTime(dr, "ComplaintDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.VisitDate = GetDateTime(dr, "VisitDate");
                objEntity.TimeFrom = GetTextVale(dr, "TimeFrom");
                objEntity.TimeTo = GetTextVale(dr, "TimeTo");
                objEntity.VisitType = GetTextVale(dr, "VisitType");
                objEntity.VisitChargeType = GetTextVale(dr, "VisitChargeType");
                objEntity.VisitCharge = GetDecimal(dr, "VisitCharge");
                objEntity.VisitNotes = GetTextVale(dr, "VisitNotes");
                objEntity.ComplaintStatus = GetTextVale(dr, "ComplaintStatus");
                objEntity.PanelPhoto = GetTextVale(dr, "PanelPhoto");
                objEntity.PhotoAfterAction = GetTextVale(dr, "PhotoAfterAction");
                objEntity.SiteCondition = GetTextVale(dr, "SiteCondition");
                objEntity.FaultByService = GetTextVale(dr, "FaultByService");
                objEntity.ActionTaken = GetTextVale(dr, "ActionTaken");
                objEntity.FurtherAction = GetTextVale(dr, "FurtherAction");
                objEntity.PanelSRNo = GetTextVale(dr, "PanelSRNo");
                objEntity.ProductSRNo = GetTextVale(dr, "ProductSRNo");
                objEntity.ContactNo1 = GetInt64(dr, "ContactNo1");
                objEntity.NatureOfCall = GetInt64(dr, "NatureOfCall"); 
                objEntity.NatureOfCallName = GetTextVale(dr, "NatureOfCallName"); 
                objEntity.CloseDate = GetDateTime(dr, "CloseDate");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.CreatedByEmployee = GetTextVale(dr, "CreatedByEmployee");

                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.ComplaintNotes = GetTextVale(dr, "ComplaintNotes");
                objEntity.SrNo = GetTextVale(dr, "SrNo");
                objEntity.NameOfCustomer = GetTextVale(dr, "NameOfCustomer");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        // ============================= Insert & Update
        public virtual void AddUpdateComplaintVisit(Entity.ComplaintVisit objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID, out string ReturnComplaintNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ComplaintVisit_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@VisitDate", objEntity.VisitDate);
            cmdAdd.Parameters.AddWithValue("@TimeFrom", objEntity.TimeFrom);
            cmdAdd.Parameters.AddWithValue("@TimeTo", objEntity.TimeTo);
            cmdAdd.Parameters.AddWithValue("@VisitNotes", objEntity.VisitNotes);
            cmdAdd.Parameters.AddWithValue("@VisitType", objEntity.VisitType);
            cmdAdd.Parameters.AddWithValue("@VisitChargeType", objEntity.VisitChargeType);
            cmdAdd.Parameters.AddWithValue("@VisitCharge", objEntity.VisitCharge);
            cmdAdd.Parameters.AddWithValue("@ComplaintStatus", objEntity.ComplaintStatus);
            cmdAdd.Parameters.AddWithValue("@PanelPhoto", objEntity.PanelPhoto);
            cmdAdd.Parameters.AddWithValue("@PhotoAfterAction", objEntity.PhotoAfterAction);
            cmdAdd.Parameters.AddWithValue("@SiteCondition", objEntity.SiteCondition);
            cmdAdd.Parameters.AddWithValue("@FaultByService", objEntity.FaultByService);
            cmdAdd.Parameters.AddWithValue("@ActionTaken", objEntity.ActionTaken);
            cmdAdd.Parameters.AddWithValue("@FurtherAction", objEntity.FurtherAction);
            cmdAdd.Parameters.AddWithValue("@NatureOfCall", objEntity.NatureOfCall);
            cmdAdd.Parameters.AddWithValue("@PanelSRNo", objEntity.PanelSRNo);
            cmdAdd.Parameters.AddWithValue("@ProductSRNo", objEntity.ProductSRNo);
            cmdAdd.Parameters.AddWithValue("@ContactNo1", objEntity.ContactNo1);
            cmdAdd.Parameters.AddWithValue("@CloseDate", objEntity.CloseDate);

            //cmdAdd.Parameters.AddWithValue("@NameOfCustomer", objEntity.NameOfCustomer);
            cmdAdd.Parameters.AddWithValue("@SrNo", objEntity.SrNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnpkID", SqlDbType.Int);
            SqlParameter p3 = new SqlParameter("@ReturnComplaintNo", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            p3.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            cmdAdd.Parameters.Add(p3);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnpkID = Convert.ToInt32(cmdAdd.Parameters["@ReturnpkID"].Value.ToString());
            ReturnComplaintNo = cmdAdd.Parameters["@ReturnComplaintNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void AddUpdateComplaintVisitAccupanel(Entity.ComplaintVisit objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID, out string ReturnComplaintNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ComplaintVisit_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@VisitDate", objEntity.VisitDate);
            cmdAdd.Parameters.AddWithValue("@TimeFrom", objEntity.TimeFrom);
            cmdAdd.Parameters.AddWithValue("@TimeTo", objEntity.TimeTo);
            cmdAdd.Parameters.AddWithValue("@VisitNotes", objEntity.VisitNotes);
            cmdAdd.Parameters.AddWithValue("@VisitType", objEntity.VisitType);
            cmdAdd.Parameters.AddWithValue("@VisitChargeType", objEntity.VisitChargeType);
            cmdAdd.Parameters.AddWithValue("@VisitCharge", objEntity.VisitCharge);
            cmdAdd.Parameters.AddWithValue("@ComplaintStatus", objEntity.ComplaintStatus);
            cmdAdd.Parameters.AddWithValue("@PanelPhoto", objEntity.PanelPhoto);
            cmdAdd.Parameters.AddWithValue("@PhotoAfterAction", objEntity.PhotoAfterAction);
            cmdAdd.Parameters.AddWithValue("@SiteCondition", objEntity.SiteCondition);
            cmdAdd.Parameters.AddWithValue("@FaultByService", objEntity.FaultByService);
            cmdAdd.Parameters.AddWithValue("@ActionTaken", objEntity.ActionTaken);
            cmdAdd.Parameters.AddWithValue("@FurtherAction", objEntity.FurtherAction);
            cmdAdd.Parameters.AddWithValue("@NatureOfCall", objEntity.NatureOfCall);
            cmdAdd.Parameters.AddWithValue("@PanelSRNo", objEntity.PanelSRNo);
            cmdAdd.Parameters.AddWithValue("@ProductSRNo", objEntity.ProductSRNo);
            cmdAdd.Parameters.AddWithValue("@ContactNo1", objEntity.ContactNo1);
            cmdAdd.Parameters.AddWithValue("@CloseDate", objEntity.CloseDate);

            cmdAdd.Parameters.AddWithValue("@NameOfCustomer", objEntity.NameOfCustomer);
            cmdAdd.Parameters.AddWithValue("@SrNo", objEntity.SrNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);

            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnpkID", SqlDbType.Int);
            SqlParameter p3 = new SqlParameter("@ReturnComplaintNo", SqlDbType.NVarChar, 255);

            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            p3.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            cmdAdd.Parameters.Add(p3);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnpkID = Convert.ToInt32(cmdAdd.Parameters["@ReturnpkID"].Value.ToString());
            ReturnComplaintNo = cmdAdd.Parameters["@ReturnComplaintNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteComplaintVisit(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ComplaintVisit_DEL";
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

        ////====================================Complaint Visit Acupanel detail=========================================================
        //public DataTable GetComplaintVisitDetail(Int64 ComplaintNo)
        //{
        //    DataTable dt = new DataTable();

        //    SqlCommand myCommand = new SqlCommand();
        //    myCommand.CommandType = CommandType.Text;
        //    myCommand.CommandText = "Select vad.pkID, vad.ComplaintNo, vad.NewRep, vad.ProductName, vad.SrNo, vad.ReplaceProduct, vad.NewSrNo, vad.Remarks from VisitAcupanel_Detail vad inner join Complaint_Detail cd on vad.ComplaintNo = cd.ComplaintNo Where vad.ComplaintNo =" + @ComplaintNo.ToString();
        //    SqlDataReader dr = ExecuteDataReader(myCommand);
        //    dt.Load(dr);
        //    ForceCloseConncetion();
        //    return dt;
        //}
        //public virtual List<Entity.ComplaintVisit> GetComplaintVisitDetailList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        //{
        //    List<Entity.ComplaintVisit> lstLocation = new List<Entity.ComplaintVisit>();
        //    SqlCommand cmdGet = new SqlCommand();
        //    cmdGet.CommandType = CommandType.StoredProcedure;
        //    cmdGet.CommandText = "VisitAcupanelDetailList";
        //    cmdGet.Parameters.AddWithValue("@pkID", pkID);
        //    cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
        //    cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
        //    cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
        //    cmdGet.Parameters.AddWithValue("@PageSize", PageSize);

        //    SqlDataReader dr = ExecuteDataReader(cmdGet);
        //    while (dr.Read())
        //    {
        //        Entity.ComplaintVisit objEntity = new Entity.ComplaintVisit();
        //        objEntity.pkID = GetInt64(dr, "pkID");
        //        objEntity.ComplaintNo = GetInt64(dr, "ComplaintNo");
        //        objEntity.NewRep = GetTextVale(dr, "NewRep");
        //        objEntity.ProductName = GetTextVale(dr, "ProductName");
        //        objEntity.SrNo = GetTextVale(dr, "SrNo");
        //        objEntity.ReplaceProduct = GetTextVale(dr, "ReplaceProduct");
        //        objEntity.NewSrNo = GetTextVale(dr, "NewSrNo");
        //        objEntity.Remarks = GetTextVale(dr, "Remarks");
        //        objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
        //        objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
               
        //        lstLocation.Add(objEntity);
        //    }
        //    dr.Close();
        //    TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
        //    ForceCloseConncetion();
        //    return lstLocation;
        //}

        //// ============================= Insert & Update
        //public virtual void AddUpdateComplaintVisitDetail(Entity.ComplaintVisit objEntity, out int ReturnCode, out string ReturnMsg)
        //{
        //    SqlCommand cmdAdd = new SqlCommand();
        //    cmdAdd.CommandType = CommandType.StoredProcedure;
        //    cmdAdd.CommandText = "VisitAcupanelDetail_INS_UPD";
        //    cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
        //    cmdAdd.Parameters.AddWithValue("@ComplaintNo", objEntity.ComplaintNo);
        //    cmdAdd.Parameters.AddWithValue("@NewRep", objEntity.NewRep);
        //    cmdAdd.Parameters.AddWithValue("@ProductName", objEntity.ProductName);
        //    cmdAdd.Parameters.AddWithValue("@SrNo", objEntity.SrNo);
        //    cmdAdd.Parameters.AddWithValue("@ReplaceProduct", objEntity.ReplaceProduct);
        //    cmdAdd.Parameters.AddWithValue("@NewSrNo", objEntity.NewSrNo);
        //    cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.Remarks);

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

        //public virtual void DeleteComplaintVisitDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        //{
        //    SqlCommand cmdDel = new SqlCommand();
        //    cmdDel.CommandType = CommandType.StoredProcedure;
        //    cmdDel.CommandText = "VisitAcupanelDetail_DEL";
        //    cmdDel.Parameters.AddWithValue("@pkID", pkID);
        //    SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
        //    SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
        //    p.Direction = ParameterDirection.Output;
        //    p1.Direction = ParameterDirection.Output;
        //    cmdDel.Parameters.Add(p);
        //    cmdDel.Parameters.Add(p1);
        //    ExecuteNonQuery(cmdDel);
        //    ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
        //    ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
        //    ForceCloseConncetion();
        //}
    }
}
