using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
   public class VisitorInfoSQL : BaseSqlManager
    {
       public virtual List<Entity.VisitorInfo> GetVisitorInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
       {
           List<Entity.VisitorInfo> lstLocation = new List<Entity.VisitorInfo>();
           SqlCommand cmdGet = new SqlCommand();
           cmdGet.CommandType = CommandType.StoredProcedure;
           cmdGet.CommandText = "VisitorInfoList";
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
               Entity.VisitorInfo objEntity = new Entity.VisitorInfo();
               objEntity.pkID = GetInt64(dr, "pkID");
               objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
               objEntity.VisitDate = GetDateTime(dr, "VisitDate");
               objEntity.VisitTime = GetTextVale(dr, "VisitTime");
               objEntity.VisitorName = GetTextVale(dr, "VisitorName");
               objEntity.VisitorContact = GetTextVale(dr, "VisitorContact");
               objEntity.VisitorEmail = GetTextVale(dr, "VisitorEmail");
               objEntity.PurposeOfVisit = GetTextVale(dr, "PurposeOfVisit");

               objEntity.Department = GetTextVale(dr, "Department");
               objEntity.MeetingTo = GetTextVale(dr, "MeetingTo");

               objEntity.CompanyName = GetTextVale(dr, "CompanyName");
               objEntity.CompanyContact = GetTextVale(dr, "CompanyContact");
               objEntity.Address = GetTextVale(dr, "Address");
               objEntity.City = GetTextVale(dr, "City");
               objEntity.Pincode = GetTextVale(dr, "Pincode");
               objEntity.State = GetTextVale(dr, "State");
               objEntity.Country = GetTextVale(dr, "Country");

               objEntity.VisitorImage = (!String.IsNullOrEmpty(GetTextVale(dr, "VisitorImage"))) ? GetTextVale(dr, "VisitorImage") : "~/images/no-figure.png";
               objEntity.VisitorDocument = (!String.IsNullOrEmpty(GetTextVale(dr, "VisitorDocument"))) ? GetTextVale(dr, "VisitorDocument") : "~/images/no-figure.png";

               objEntity.CustomerID = GetInt64(dr, "CustomerID");
               //objEntity.CustomerName = GetTextVale(dr, "CustomerName");

               objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");

               objEntity.CompanyID = GetInt64(dr, "CompanyID");
               objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");

               lstLocation.Add(objEntity);
           }
           dr.Close();
           TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
           ForceCloseConncetion();
           return lstLocation;
       }

       public virtual List<Entity.VisitorInfo> GetVisitorInfoList(Int64 pkID, string LoginUserID, string SearchKey, string pStatus, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
       {
           List<Entity.VisitorInfo> lstLocation = new List<Entity.VisitorInfo>();
           SqlCommand cmdGet = new SqlCommand();
           cmdGet.CommandType = CommandType.StoredProcedure;
           cmdGet.CommandText = "VisitorInfoList";
           cmdGet.Parameters.AddWithValue("@pkID", pkID);
           cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
           cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
           cmdGet.Parameters.AddWithValue("@Status", pStatus);
           cmdGet.Parameters.AddWithValue("@Month", pMonth);
           cmdGet.Parameters.AddWithValue("@Year", pYear);
           cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
           cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
           SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
           p.Direction = ParameterDirection.Output;
           cmdGet.Parameters.Add(p);
           SqlDataReader dr = ExecuteDataReader(cmdGet);
           while (dr.Read())
           {
               Entity.VisitorInfo objEntity = new Entity.VisitorInfo();
               objEntity.pkID = GetInt64(dr, "pkID");
               objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
               objEntity.VisitDate = GetDateTime(dr, "VisitDate");
               objEntity.VisitTime = GetTextVale(dr, "VisitTime");
               objEntity.VisitorName = GetTextVale(dr, "VisitorName");
               objEntity.VisitorContact = GetTextVale(dr, "VisitorContact");
               objEntity.VisitorEmail = GetTextVale(dr, "VisitorEmail");
               objEntity.PurposeOfVisit = GetTextVale(dr, "PurposeOfVisit");

               objEntity.Department = GetTextVale(dr, "Department");
               objEntity.MeetingTo = GetTextVale(dr, "MeetingTo");

               objEntity.CompanyName = GetTextVale(dr, "CompanyName");
               objEntity.CompanyContact = GetTextVale(dr, "CompanyContact");
               objEntity.Address = GetTextVale(dr, "Address");
               objEntity.City = GetTextVale(dr, "City");
               objEntity.Pincode = GetTextVale(dr, "Pincode");
               objEntity.State = GetTextVale(dr, "State");
               objEntity.Country = GetTextVale(dr, "Country");

               objEntity.VisitorImage = (!String.IsNullOrEmpty(GetTextVale(dr, "VisitorImage"))) ? GetTextVale(dr, "VisitorImage") : "~/images/no-figure.png";
               objEntity.VisitorDocument = (!String.IsNullOrEmpty(GetTextVale(dr, "VisitorDocument"))) ? GetTextVale(dr, "VisitorDocument") : "~/images/no-figure.png";

               objEntity.CustomerID = GetInt64(dr, "CustomerID");
               //objEntity.CustomerName = GetTextVale(dr, "CustomerName");

               objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");

               objEntity.CompanyID = GetInt64(dr, "CompanyID");
               objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");

               lstLocation.Add(objEntity);
           }
           dr.Close();
           TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
           ForceCloseConncetion();
           return lstLocation;
       }

       public virtual void AddUpdateVisitorInfo(Entity.VisitorInfo objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInquiryNo, out Int64 ReturnVisitorId)
       {
           SqlCommand cmdAdd = new SqlCommand();
           cmdAdd.CommandType = CommandType.StoredProcedure;
           cmdAdd.CommandText = "VisitorInfo_INS_UPD";
           cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
           cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
           cmdAdd.Parameters.AddWithValue("@VisitDate", objEntity.VisitDate);
           cmdAdd.Parameters.AddWithValue("@VisitTime", objEntity.VisitTime);
           cmdAdd.Parameters.AddWithValue("@VisitorName", objEntity.VisitorName);
           cmdAdd.Parameters.AddWithValue("@VisitorContact", objEntity.VisitorContact);
           cmdAdd.Parameters.AddWithValue("@VisitorEmail", objEntity.VisitorEmail);
           cmdAdd.Parameters.AddWithValue("@PurposeOfVisit", objEntity.PurposeOfVisit);

           cmdAdd.Parameters.AddWithValue("@Department", objEntity.Department);
           cmdAdd.Parameters.AddWithValue("@MeetingTo", objEntity.MeetingTo);

           cmdAdd.Parameters.AddWithValue("@CompanyName", objEntity.CompanyName);
           cmdAdd.Parameters.AddWithValue("@CompanyContact", objEntity.CompanyContact);
           cmdAdd.Parameters.AddWithValue("@Address", objEntity.Address);
           cmdAdd.Parameters.AddWithValue("@City", objEntity.City);
           cmdAdd.Parameters.AddWithValue("@Pincode", objEntity.Pincode);
           cmdAdd.Parameters.AddWithValue("@State", objEntity.State);
           cmdAdd.Parameters.AddWithValue("@Country", objEntity.Country);

           if (!String.IsNullOrEmpty(objEntity.VisitorImage))
               cmdAdd.Parameters.AddWithValue("@VisitorImage", objEntity.VisitorImage);

           if (!String.IsNullOrEmpty(objEntity.VisitorDocument))
               cmdAdd.Parameters.AddWithValue("@VisitorDocument", objEntity.VisitorDocument);

           cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);

           cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);

           cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
           SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
           SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
           SqlParameter p2 = new SqlParameter("@ReturnInquiryNo", SqlDbType.NVarChar, 30);
           SqlParameter p3 = new SqlParameter("@ReturnVisitorId", SqlDbType.BigInt);
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
           ReturnInquiryNo = cmdAdd.Parameters["@ReturnInquiryNo"].Value.ToString();
           ReturnVisitorId = Convert.ToInt32(cmdAdd.Parameters["@ReturnVisitorId"].Value.ToString());
           ForceCloseConncetion();
       }

       public virtual void DeleteVisitorInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
       {
           SqlCommand cmdDel = new SqlCommand();
           cmdDel.CommandType = CommandType.StoredProcedure;
           cmdDel.CommandText = "VisitorInfo_DEL";
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
