using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{

    public class ExternalLeadsSQL : BaseSqlManager
    {
        public virtual List<Entity.ExternalLeads> GetExternalLeadList(Int64 pkId, string acid, string source, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ExternalLeads> lstLead = new List<Entity.ExternalLeads>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExternalLeadList";
            cmdGet.Parameters.AddWithValue("@pkId", pkId);
            cmdGet.Parameters.AddWithValue("@acid", acid);
            cmdGet.Parameters.AddWithValue("@LeadSource", source);
            cmdGet.Parameters.AddWithValue("@LoginUserID", System.Web.HttpContext.Current.Session["LoginUserID"].ToString());
            cmdGet.Parameters.AddWithValue("@SerialKey", System.Web.HttpContext.Current.Session["SerialKey"].ToString());
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeadID = GetTextVale(dr, "LeadID");
                objEntity.LeadSource = GetTextVale(dr, "LeadSource");
                objEntity.ACID = GetTextVale(dr, "ACID");
                objEntity.QueryDatetime = GetDateTime(dr, "QueryDatetime");
                objEntity.SenderName = GetTextVale(dr, "SenderName");
                objEntity.SenderMail = GetTextVale(dr, "SenderMail");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.CountryFlagURL = GetTextVale(dr, "CountryFlagURL");
                objEntity.Message = GetTextVale(dr, "Message");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.PrimaryMobileNo = GetTextVale(dr, "PrimaryMobileNo");
                objEntity.SecondaryMobileNo = GetTextVale(dr, "SecondaryMobileNo");
                objEntity.ForProduct = GetTextVale(dr, "ForProduct");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryNopkID = GetInt64(dr, "InquiryNopkID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.ExLeadClosure = GetInt64(dr, "ExLeadClosure");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.DisqualifedRemarks = GetTextVale(dr, "DisqualifedRemarks");
                //objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                //objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                //objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");

                lstLead.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLead;
        }

        public virtual List<Entity.ExternalLeads> GetExternalLeadListByStatus(Int64 pkId, string acid, string cat, string source, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ExternalLeads> lstLead = new List<Entity.ExternalLeads>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExternalLeadListByStatus";
            cmdGet.Parameters.AddWithValue("@pkId", pkId);
            cmdGet.Parameters.AddWithValue("@acid", acid);
            cmdGet.Parameters.AddWithValue("@category", cat);
            cmdGet.Parameters.AddWithValue("@LeadSource", source);
            cmdGet.Parameters.AddWithValue("@LoginUserID", System.Web.HttpContext.Current.Session["LoginUserID"].ToString());
            cmdGet.Parameters.AddWithValue("@SerialKey", System.Web.HttpContext.Current.Session["SerialKey"].ToString());
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeadID = GetTextVale(dr, "LeadID");
                objEntity.LeadSource = GetTextVale(dr, "LeadSource");
                objEntity.ACID = GetTextVale(dr, "ACID");
                objEntity.QueryDatetime = GetDateTime(dr, "QueryDatetime");
                objEntity.SenderName = GetTextVale(dr, "SenderName");
                objEntity.SenderMail = GetTextVale(dr, "SenderMail");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.CountryFlagURL = GetTextVale(dr, "CountryFlagURL");
                objEntity.Message = GetTextVale(dr, "Message");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.PrimaryMobileNo = GetTextVale(dr, "PrimaryMobileNo");
                objEntity.SecondaryMobileNo = GetTextVale(dr, "SecondaryMobileNo");
                objEntity.ForProduct = GetTextVale(dr, "ForProduct");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryNopkID = GetInt64(dr, "InquiryNopkID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.ExLeadClosure = GetInt64(dr, "ExLeadClosure");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.ExLeadCloserReason = GetTextVale(dr, "ExLeadCloserReason");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.DisqualifedRemarks = GetTextVale(dr, "DisqualifedRemarks");
                //objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                //objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                //objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");


                lstLead.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLead;
        }

        public virtual List<Entity.ExternalLeads> GetExternalLeadListByStatus(Int64 pkId, string acid, string cat, string source, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ExternalLeads> lstLead = new List<Entity.ExternalLeads>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExternalLeadListByStatus";
            cmdGet.Parameters.AddWithValue("@pkId", pkId);
            cmdGet.Parameters.AddWithValue("@acid", acid);
            cmdGet.Parameters.AddWithValue("@category", cat);
            cmdGet.Parameters.AddWithValue("@LeadSource", source);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@SerialKey", System.Web.HttpContext.Current.Session["SerialKey"].ToString());
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeadID = GetTextVale(dr, "LeadID");
                objEntity.LeadSource = GetTextVale(dr, "LeadSource");
                objEntity.ACID = GetTextVale(dr, "ACID");
                objEntity.QueryDatetime = GetDateTime(dr, "QueryDatetime");
                objEntity.SenderName = GetTextVale(dr, "SenderName");
                objEntity.SenderMail = GetTextVale(dr, "SenderMail");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.CountryFlagURL = GetTextVale(dr, "CountryFlagURL");
                objEntity.Message = GetTextVale(dr, "Message");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.PrimaryMobileNo = GetTextVale(dr, "PrimaryMobileNo");
                objEntity.SecondaryMobileNo = GetTextVale(dr, "SecondaryMobileNo");
                objEntity.ForProduct = GetTextVale(dr, "ForProduct");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryNopkID = GetInt64(dr, "InquiryNopkID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.ExLeadClosure = GetInt64(dr, "ExLeadClosure");
                objEntity.ExLeadCloserReason = GetTextVale(dr, "ExLeadCloserReason");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.DisqualifedRemarks = GetTextVale(dr, "DisqualifedRemarks");
                //objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                //objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                //objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                lstLead.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLead;
        }
        public virtual List<Entity.ExternalLeads> GetExternalLeadView(string status, string source, Int64 month, Int64 year, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.ExternalLeads> lstLead = new List<Entity.ExternalLeads>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExternalLeadCustomView";
            cmdGet.Parameters.AddWithValue("@LeadStatus", status);
            cmdGet.Parameters.AddWithValue("@LeadSource", source);
            cmdGet.Parameters.AddWithValue("@Month", month);
            cmdGet.Parameters.AddWithValue("@Year", year);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.LeadID = GetTextVale(dr, "LeadID");
                objEntity.LeadSource = GetTextVale(dr, "LeadSource");
                objEntity.ACID = GetTextVale(dr, "ACID");
                objEntity.QueryDatetime = GetDateTime(dr, "QueryDatetime");
                objEntity.SenderName = GetTextVale(dr, "SenderName");
                objEntity.SenderMail = GetTextVale(dr, "SenderMail");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.CountryFlagURL = GetTextVale(dr, "CountryFlagURL");
                objEntity.Message = GetTextVale(dr, "Message");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.PrimaryMobileNo = GetTextVale(dr, "PrimaryMobileNo");
                objEntity.SecondaryMobileNo = GetTextVale(dr, "SecondaryMobileNo");
                objEntity.ForProduct = GetTextVale(dr, "ForProduct");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.CountryISO = GetTextVale(dr, "CountryISO");
                objEntity.CountryName = GetTextVale(dr, "CountryName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryNopkID = GetInt64(dr, "InquiryNopkID");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.ExLeadClosure = GetInt64(dr, "ExLeadClosure");
                objEntity.ExLeadCloserReason = GetTextVale(dr, "ExLeadCloserReason");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");

                lstLead.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLead;
        }
        public virtual List<Entity.ExternalLeads> GetExternalLeadList_RPT(Int64 pkId, int PageNo, int PageSize, out int TotalRecord, DateTime Todate, DateTime Fromdate)
        {
            List<Entity.ExternalLeads> lstLead = new List<Entity.ExternalLeads>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExternalLeadList_Report";
            cmdGet.Parameters.AddWithValue("@pkId", pkId);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            cmdGet.Parameters.AddWithValue("@Todate", Todate);
            cmdGet.Parameters.AddWithValue("@FromDate", Fromdate);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ExternalLeads objEntity = new Entity.ExternalLeads();

                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.QueryDatetime = GetDateTime(dr, "QueryDatetime");
                objEntity.SenderName = GetTextVale(dr, "SenderName");
                objEntity.SenderMail = GetTextVale(dr, "SenderMail");
                objEntity.CompanyName = GetTextVale(dr, "CompanyName");
                objEntity.CountryFlagURL = GetTextVale(dr, "CountryFlagURL");
                objEntity.Message = GetTextVale(dr, "Message");
                objEntity.Address = GetTextVale(dr, "Address");
                objEntity.City = GetTextVale(dr, "City");
                objEntity.Pincode = GetTextVale(dr, "Pincode");
                objEntity.State = GetTextVale(dr, "State");
                objEntity.PrimaryMobileNo = GetTextVale(dr, "PrimaryMobileNo");
                objEntity.SecondaryMobileNo = GetTextVale(dr, "SecondaryMobileNo");
                objEntity.ForProduct = GetTextVale(dr, "ForProduct");
                objEntity.LeadSource = GetTextVale(dr, "LeadSource");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.ProductID = GetInt64(dr, "ProductID");
                objEntity.ProductName = GetTextVale(dr, "ProductName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.CityCode = GetInt64(dr, "CityCode");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");

                lstLead.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLead;
        }

        public virtual List<Entity.City> GetCityCodeByName(string CityName, string StateCode, string LoginUserID)
        {
            List<Entity.City> lstCity = new List<Entity.City>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetCityCodeByName";
            cmdGet.Parameters.AddWithValue("@CityName", CityName);
            cmdGet.Parameters.AddWithValue("@StateCode", StateCode);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.City objEntity = new Entity.City();
                objEntity.CityCode = GetInt64(dr, "CityCode");
                lstCity.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();

            return lstCity;
        }


        public virtual void AddUpdateExternalLeads(Entity.ExternalLeads objEntity, out int ReturnCode, out string ReturnMsg,out Int64 ReturnInquiryPkID, out Int64 ReturnFollowupPkID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ExternalLead_INS_UPD";

            cmdAdd.Parameters.AddWithValue("pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("LeadID", objEntity.LeadID);
            cmdAdd.Parameters.AddWithValue("LeadSource", objEntity.LeadSource);
            cmdAdd.Parameters.AddWithValue("ACID", objEntity.ACID);
            cmdAdd.Parameters.AddWithValue("QueryDatetime", objEntity.QueryDatetime);
            cmdAdd.Parameters.AddWithValue("SenderName", objEntity.SenderName);
            cmdAdd.Parameters.AddWithValue("SenderMail", objEntity.SenderMail);
            cmdAdd.Parameters.AddWithValue("CompanyName", objEntity.CompanyName);
            cmdAdd.Parameters.AddWithValue("CountryFlagURL", objEntity.CountryFlagURL);
            cmdAdd.Parameters.AddWithValue("Message", objEntity.Message);
            cmdAdd.Parameters.AddWithValue("Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("City", objEntity.City);
            //cmdAdd.Parameters.AddWithValue("Country", objEntity.CountryName);
            cmdAdd.Parameters.AddWithValue("State", objEntity.State);
            cmdAdd.Parameters.AddWithValue("CountryISO", objEntity.CountryISO);
            cmdAdd.Parameters.AddWithValue("PrimaryMobileNo", objEntity.PrimaryMobileNo);
            cmdAdd.Parameters.AddWithValue("SecondaryMobileNo", objEntity.SecondaryMobileNo);
            cmdAdd.Parameters.AddWithValue("LeadStatus", objEntity.LeadStatus);
            cmdAdd.Parameters.AddWithValue("ForProduct", objEntity.ForProduct);
            if (objEntity.LeadSource.ToLower() == "telecaller")
            {
                cmdAdd.Parameters.AddWithValue("ProductID", objEntity.ProductID);
                cmdAdd.Parameters.AddWithValue("Pincode", objEntity.Pincode);
                cmdAdd.Parameters.AddWithValue("StateCode", objEntity.StateCode);
                cmdAdd.Parameters.AddWithValue("CityCode", objEntity.CityCode);
                cmdAdd.Parameters.AddWithValue("CountryCode", objEntity.CountryCode);
                cmdAdd.Parameters.AddWithValue("CustomerID", objEntity.CustomerID);
                cmdAdd.Parameters.AddWithValue("EmployeeID", objEntity.EmployeeID);
                if (objEntity.LeadStatus.ToString() == "Qualified")
                {
                    
                    if (!String.IsNullOrEmpty(objEntity.FollowupNotes))
                    {
                        cmdAdd.Parameters.AddWithValue("@FollowupNotes", objEntity.FollowupNotes);
                        cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
                        cmdAdd.Parameters.AddWithValue("@PreferredTime", objEntity.PreferredTime);
                    }
                }
                else
                {
                    cmdAdd.Parameters.AddWithValue("ExLeadClosure", objEntity.ExLeadClosure);
                    cmdAdd.Parameters.AddWithValue("DisqualifedRemarks", objEntity.DisqualifedRemarks);
                }
            }
            else
            {
                if (objEntity.LeadStatus.ToString() == "Qualified")
                {
                    cmdAdd.Parameters.AddWithValue("EmployeeID", objEntity.EmployeeID);
                    cmdAdd.Parameters.AddWithValue("ProductID", objEntity.ProductID);
                    cmdAdd.Parameters.AddWithValue("Pincode", objEntity.Pincode);
                    cmdAdd.Parameters.AddWithValue("StateCode", objEntity.StateCode);
                    cmdAdd.Parameters.AddWithValue("CityCode", objEntity.CityCode);
                    cmdAdd.Parameters.AddWithValue("CountryCode", objEntity.CountryCode);
                    cmdAdd.Parameters.AddWithValue("CustomerID", objEntity.CustomerID);

                    cmdAdd.Parameters.AddWithValue("@FollowupNotes", objEntity.FollowupNotes);
                    cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
                    cmdAdd.Parameters.AddWithValue("@PreferredTime", objEntity.PreferredTime);
                }
                else if (objEntity.LeadStatus.ToString() == "InProcess")
                {
                    cmdAdd.Parameters.AddWithValue("EmployeeID", objEntity.EmployeeID);
                }
                else if (objEntity.LeadStatus.ToString() == "Disqualified")
                {
                    cmdAdd.Parameters.AddWithValue("ExLeadClosure", objEntity.ExLeadClosure);
                    cmdAdd.Parameters.AddWithValue("DisqualifedRemarks", objEntity.DisqualifedRemarks);
                }
            }
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            cmdAdd.Parameters.AddWithValue("@SerialKey", System.Web.HttpContext.Current.Session["SerialKey"].ToString());
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnInquiryPkID", SqlDbType.BigInt);
            SqlParameter p3 = new SqlParameter("@ReturnFollowupPkID", SqlDbType.BigInt);

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
            ReturnInquiryPkID = Convert.ToInt64(cmdAdd.Parameters["@ReturnInquiryPkID"].Value.ToString());
            ReturnFollowupPkID = Convert.ToInt64(cmdAdd.Parameters["@ReturnFollowupPkID"].Value.ToString());

            ForceCloseConncetion();
        }

        public virtual void AddUpdateExternalLeadsUPDOWN(Entity.ExternalLeads objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ExternalLead_INS_UPD_UPDOWN";

            cmdAdd.Parameters.AddWithValue("pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("LeadID", objEntity.LeadID);
            cmdAdd.Parameters.AddWithValue("LeadSource", objEntity.LeadSource);
            cmdAdd.Parameters.AddWithValue("ACID", objEntity.ACID);
            cmdAdd.Parameters.AddWithValue("QueryDatetime", objEntity.QueryDatetime);
            cmdAdd.Parameters.AddWithValue("SenderName", objEntity.SenderName);
            cmdAdd.Parameters.AddWithValue("SenderMail", objEntity.SenderMail);
            cmdAdd.Parameters.AddWithValue("CompanyName", objEntity.CompanyName);
            cmdAdd.Parameters.AddWithValue("CountryFlagURL", objEntity.CountryFlagURL);
            cmdAdd.Parameters.AddWithValue("Message", objEntity.Message);
            cmdAdd.Parameters.AddWithValue("Address", objEntity.Address);
            cmdAdd.Parameters.AddWithValue("City", objEntity.City);

            cmdAdd.Parameters.AddWithValue("State", objEntity.State);
            cmdAdd.Parameters.AddWithValue("CountryISO", objEntity.CountryISO);
            cmdAdd.Parameters.AddWithValue("PrimaryMobileNo", objEntity.PrimaryMobileNo);
            cmdAdd.Parameters.AddWithValue("SecondaryMobileNo", objEntity.SecondaryMobileNo);
            cmdAdd.Parameters.AddWithValue("LeadStatus", objEntity.LeadStatus);
            cmdAdd.Parameters.AddWithValue("ForProduct", objEntity.ForProduct);
            cmdAdd.Parameters.AddWithValue("EmployeeID", objEntity.EmployeeID);
            if (objEntity.LeadSource.ToLower() == "telecaller")
            {
                cmdAdd.Parameters.AddWithValue("ProductID", objEntity.ProductID);
                cmdAdd.Parameters.AddWithValue("Pincode", objEntity.Pincode);
                cmdAdd.Parameters.AddWithValue("StateCode", objEntity.StateCode);
                cmdAdd.Parameters.AddWithValue("CityCode", objEntity.CityCode);
                cmdAdd.Parameters.AddWithValue("CountryCode", objEntity.CountryCode);
                cmdAdd.Parameters.AddWithValue("CustomerID", objEntity.CustomerID);
                if (objEntity.LeadStatus.ToString().ToLower() == "disqualified")
                {
                    cmdAdd.Parameters.AddWithValue("ExLeadClosure", objEntity.ExLeadClosure);
                }
            }
            else
            {
                if (objEntity.LeadStatus.ToString() == "Qualified")
                {
                    cmdAdd.Parameters.AddWithValue("EmployeeID", objEntity.EmployeeID);
                    cmdAdd.Parameters.AddWithValue("ProductID", objEntity.ProductID);
                    cmdAdd.Parameters.AddWithValue("Pincode", objEntity.Pincode);
                    cmdAdd.Parameters.AddWithValue("StateCode", objEntity.StateCode);
                    cmdAdd.Parameters.AddWithValue("CityCode", objEntity.CityCode);
                    cmdAdd.Parameters.AddWithValue("CountryCode", objEntity.CountryCode);
                    cmdAdd.Parameters.AddWithValue("CustomerID", objEntity.CustomerID);
                }
                else if (objEntity.LeadStatus.ToString() == "Disqualified")
                {
                    cmdAdd.Parameters.AddWithValue("ExLeadClosure", objEntity.ExLeadClosure);
                }
            }
            // ----------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(objEntity.FollowupNotes))
            {
                cmdAdd.Parameters.AddWithValue("@FollowupNotes", objEntity.FollowupNotes);
                cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
                cmdAdd.Parameters.AddWithValue("@PreferredTime", objEntity.PreferredTime);
            }

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

        public virtual void DeleteExternalLeads(String pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ExternalLead_DEL";
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

        public virtual void AddUpdateExternalLeadsRegion(Entity.ExternalLeadsRegion objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ExternalLeadsRegion_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@CountryCode", objEntity.CountryCode);
            cmdAdd.Parameters.AddWithValue("@StateCode", objEntity.StateCode);
            cmdAdd.Parameters.AddWithValue("@CityCode", objEntity.CityCode);
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

        public virtual void DeleteExternalLedasRegion(Int64 EmployeeID, string CountryCode, Int64 StateCode, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "ExternalLeadsRegion_DEL";
            cmdDel.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmdDel.Parameters.AddWithValue("@CountryCode", CountryCode);
            cmdDel.Parameters.AddWithValue("@StateCode", StateCode);
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

        public virtual List<Entity.ExternalLeadsRegion> GetExternalLeadsRegionList(Int64 pkId, Int64 EmployeeID, Int64 CountryCode,  Int64 StateCode, Int64 CityCode, string LoginUserID, out int TotalCount)
        {
            List<Entity.ExternalLeadsRegion> lstLeadRegion = new List<Entity.ExternalLeadsRegion>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ExternalLeadsRegionList";
            cmdGet.Parameters.AddWithValue("@pkID", pkId);
            cmdGet.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            cmdGet.Parameters.AddWithValue("@CountryCode", CountryCode);
            cmdGet.Parameters.AddWithValue("@StateCode", StateCode);
            cmdGet.Parameters.AddWithValue("@CityCode", CityCode);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int, 255);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.ExternalLeadsRegion objEntity = new Entity.ExternalLeadsRegion();
                //objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.CountryCode = GetTextVale(dr, "CountryCode");
                objEntity.Country = GetTextVale(dr, "CountryName");
                objEntity.StateCode = GetInt64(dr, "StateCode");
                objEntity.State = GetTextVale(dr, "StateName");
                //objEntity.City = GetTextVale(dr, "CityName");
                //objEntity.CityCode = GetInt64(dr, "CityCode");
                //objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                //objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                //objEntity.UpdatedBy = GetTextVale(dr, "UpdatedBy");
                //objEntity.UpdatedDate = GetDateTime(dr, "UpdatedDate");
                objEntity.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                objEntity.noofcity = GetInt64(dr, "noofcity");
                objEntity.CityList = GetTextVale(dr, "CityList");
                lstLeadRegion.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            TotalCount = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            return lstLeadRegion;

        }
    }
}
