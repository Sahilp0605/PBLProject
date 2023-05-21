using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class FollowupSQL : BaseSqlManager
    {

        public virtual List<Entity.Followup> GetFollowupList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Followup> lstLocation = new List<Entity.Followup>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryFollowupList";
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
                Entity.Followup objEntity = new Entity.Followup();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.NextFollowupDate = GetDateTime(dr, "NextFollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                objEntity.Rating = GetInt64(dr, "Rating");
                objEntity.NoFollowup = GetBoolean(dr, "NoFollowup");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.InquiryNoStatus = GetTextVale(dr, "InquiryNoStatus");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.NoFollClosureID = GetInt64(dr, "NoFollClosureID");
                objEntity.NoFollClosureName = GetTextVale(dr, "NoFollClosureName");
                objEntity.FollowupPriority = GetInt64(dr, "FollowupPriority");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                //objEntity.CityName = GetTextVale(dr, "CityName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Followup> GetFollowupByUser(string LoginUserID, Int64 pMonth, Int64 pYear, string pFromDate=null, string pToDate=null)
        {
            List<Entity.Followup> lstLocation = new List<Entity.Followup>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryFollowupListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", pFromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", pToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Followup objEntity = new Entity.Followup();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.NextFollowupDate = GetDateTime(dr, "NextFollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                objEntity.Rating = GetInt64(dr, "Rating");
                objEntity.NoFollowup = GetBoolean(dr, "NoFollowup");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.NoFollClosureID = GetInt64(dr, "NoFollClosureID");
                objEntity.NoFollClosureName = GetTextVale(dr, "NoFollClosureName");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Followup> GetDashboardFollowupList(String FollowupStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Followup> lstLocation = new List<Entity.Followup>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardInquiryFollowupList";
            cmdGet.Parameters.AddWithValue("@FollowupStatus", FollowupStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Followup objEntity = new Entity.Followup();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExtpkID = GetInt64(dr, "ExtpkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.NextFollowupDate = GetDateTime(dr, "NextFollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                objEntity.Rating = GetInt64(dr, "Rating");
                objEntity.NoFollowup = GetBoolean(dr, "NoFollowup");
                objEntity.FollowUpSource = GetTextVale(dr, "FollowUpSource");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.NoFollClosureID = GetInt64(dr, "NoFollClosureID");
                objEntity.NoFollClosureName = GetTextVale(dr, "NoFollClosureName");
                objEntity.ContactNo1 = GetTextVale(dr, "ContactNo1");
                objEntity.ContactNo2 = GetTextVale(dr, "ContactNo2");
                objEntity.ContactPerson = GetTextVale(dr, "ContactPerson1");
                objEntity.ContactPersonPhone = GetTextVale(dr, "ContactNumber1");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Followup> GetDashboardFollowupTimeline(Int64 CustomerID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Followup> lstLocation = new List<Entity.Followup>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardFollowupTimeline";
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Followup objEntity = new Entity.Followup();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.NextFollowupDate = GetDateTime(dr, "NextFollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                objEntity.Rating = GetInt64(dr, "Rating");
                objEntity.NoFollowup = GetBoolean(dr, "NoFollowup");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.QuotationNo = GetTextVale(dr, "QuotationNo");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                objEntity.NoFollClosureID = GetInt64(dr, "NoFollClosureID");
                objEntity.NoFollClosureName = GetTextVale(dr, "NoFollClosureName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateFollowup(Entity.Followup objEntity, out int ReturnCode, out string ReturnMsg, out Int64 ReturnFollowupPKID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InquiryFollowup_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@MeetingNotes", objEntity.MeetingNotes);
            cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
            cmdAdd.Parameters.AddWithValue("@NextFollowupDate", objEntity.NextFollowupDate);
            cmdAdd.Parameters.AddWithValue("@PreferredTime", objEntity.PreferredTime);
            cmdAdd.Parameters.AddWithValue("@Rating", objEntity.Rating);
            cmdAdd.Parameters.AddWithValue("@InquiryStatusID", objEntity.InquiryStatusID);
            cmdAdd.Parameters.AddWithValue("@NoFollowup", objEntity.NoFollowup);
            cmdAdd.Parameters.AddWithValue("@NoFollClosureID", objEntity.NoFollClosureID);
            cmdAdd.Parameters.AddWithValue("@FollowupPriority", objEntity.FollowupPriority);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnFollowupPKID", SqlDbType.BigInt);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnFollowupPKID = Convert.ToInt64(cmdAdd.Parameters["@ReturnFollowupPKID"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteFollowup(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryFollowup_DEL";
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

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Inquiry Followup - External
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        public virtual List<Entity.Followup> GetFollowupExtList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Followup> lstLocation = new List<Entity.Followup>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryFollowupExtList";
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
                Entity.Followup objEntity = new Entity.Followup();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExtpkID = GetInt64(dr, "ExtpkID");
                objEntity.FollowUpSource = GetTextVale(dr, "FollowUpSource");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.InquirySource = GetTextVale(dr, "LeadSource");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.NextFollowupDate = GetDateTime(dr, "NextFollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.NoFollClosureID = GetInt64(dr, "NoFollClosureID");
                objEntity.NoFollClosureName = GetTextVale(dr, "NoFollClosureName");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.Followup> GetDashboardFollowupExtTimeline(Int64 ExtpkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Followup> lstLocation = new List<Entity.Followup>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardFollowupExtTimeline";
            cmdGet.Parameters.AddWithValue("@ExtpkID", ExtpkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Followup objEntity = new Entity.Followup();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ExtpkID = GetInt64(dr, "ExtpkID");
                objEntity.FollowUpSource = GetTextVale(dr, "FollowUpSource");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.InquirySource = GetTextVale(dr, "LeadSource");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.NextFollowupDate = GetDateTime(dr, "NextFollowupDate");
                objEntity.PreferredTime = GetTextVale(dr, "PreferredTime");
                objEntity.LeadStatus = GetTextVale(dr, "LeadStatus");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = 0;
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateFollowupExt(Entity.Followup objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InquiryFollowupExt_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ExtpkID", objEntity.ExtpkID);
            cmdAdd.Parameters.AddWithValue("@FollowUpSource", objEntity.FollowUpSource);
            cmdAdd.Parameters.AddWithValue("@InquiryStatusID", objEntity.InquiryStatusID);
            cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
            cmdAdd.Parameters.AddWithValue("@MeetingNotes", objEntity.MeetingNotes);
            cmdAdd.Parameters.AddWithValue("@NextFollowupDate", objEntity.NextFollowupDate);
            cmdAdd.Parameters.AddWithValue("@PreferredTime", objEntity.PreferredTime);
            cmdAdd.Parameters.AddWithValue("@LeadStatus", objEntity.LeadStatus);
            cmdAdd.Parameters.AddWithValue("@NoFollClosureID", objEntity.NoFollClosureID);
            cmdAdd.Parameters.AddWithValue("@AssignToEmployee", objEntity.EmployeeID);
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

        public virtual void DeleteFollowupExt(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryFollowupExt_DEL";
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
