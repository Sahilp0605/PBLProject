using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class InquiryInfoClinicSQL : BaseSqlManager
    {
        
        public virtual List<Entity.InquiryInfo> GetInquiryInfoClinicList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            List<Entity.InquiryInfo> lstLocation = new List<Entity.InquiryInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryListByUser";
            //cmdGet.Parameters.AddWithValue("@pkID", pkID);
            //cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            //cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            //cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            cmdGet.Parameters.AddWithValue("@InquiryStatus", pStatus);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            //SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            //p.Direction = ParameterDirection.Output;
            //cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.SpecialityID = GetInt64(dr, "SpecialityID");
                objEntity.TreatmentType = GetTextVale(dr, "TreatmentType");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                objEntity.SpecialityName = GetTextVale(dr, "SpecialityName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            //TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryInfoClinicList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.InquiryInfo> lstLocation = new List<Entity.InquiryInfo>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            //cmdGet.Parameters.AddWithValue("@InquiryStatus", pStatus);
            //cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            //cmdGet.Parameters.AddWithValue("@Month", pMonth);
            //cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.SpecialityID = GetInt64(dr, "SpecialityID");
                objEntity.TreatmentType = GetTextVale(dr, "TreatmentType");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");

                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");                
                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                objEntity.SpecialityName = GetTextVale(dr, "SpecialityName");
                objEntity.StateName = GetTextVale(dr, "StateName");
                objEntity.CityName = GetTextVale(dr, "CityName");       

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdateInquiryInfoClinic(Entity.InquiryInfo objEntity, out int ReturnCode, out string ReturnMsg, out string ReturnInquiryNo)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Inquiry_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@InquiryDate", objEntity.InquiryDate);
            cmdAdd.Parameters.AddWithValue("@ReferenceName", objEntity.ReferenceName);
            cmdAdd.Parameters.AddWithValue("@InquirySource", objEntity.InquirySource);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@SpecialityID", objEntity.SpecialityID);
            cmdAdd.Parameters.AddWithValue("@TreatmentType", objEntity.TreatmentType);            
            cmdAdd.Parameters.AddWithValue("@MeetingNotes", objEntity.MeetingNotes);
            cmdAdd.Parameters.AddWithValue("@FollowupNotes", objEntity.FollowupNotes);
            cmdAdd.Parameters.AddWithValue("@FollowupDate", objEntity.FollowupDate);
            cmdAdd.Parameters.AddWithValue("@BillNo", objEntity.BillNo);
            cmdAdd.Parameters.AddWithValue("@OrderNo", objEntity.OrderNo);            
            cmdAdd.Parameters.AddWithValue("@InquiryStatusID", objEntity.InquiryStatusID);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnInquiryNo", SqlDbType.NVarChar, 30);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnInquiryNo = cmdAdd.Parameters["@ReturnInquiryNo"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void DeleteInquiryInfoClinic(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Inquiry_DEL";
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

        
        /*-------------------------------------------------------------*/
        public DataTable GetInquiryClinicProductDetail(string pInquiryNo)
        {
            DataTable dt = new DataTable();

            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = " SELECT ip.pkID, ip.InquiryNo, ip.CustomerID as ProductID, CAST(it.CustomerName AS NVARCHAR(200)) As ProductName, " +
                                    " it.CustomerName  As ProductNameLong, ip.DoctorID, ip.Doctorid As DoctorName, ISNULL(ip.Amount,0) AS 'Amount', ip.Visited,  " +
                                    " FORMAT(ip.AppoinmentDt, 'yyyy-MM-dd') As AppoinmentDt, ip.Started, ip.FinalAmount as FinalAmt, FORMAT(ip.CompletionDt, 'yyyy-MM-dd') As CompletionDt, ip.Finished" +
                                    " From Inquiry_Hospital ip  " +
                                    " Inner Join MST_Customer it On ip.CustomerID = it.CustomerID Where ip.InquiryNo = '" + @pInquiryNo + "'";
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual void AddUpdateInquiryClinicProduct(Entity.InquiryInfo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "InquiryClinicProduct_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@ProductID", objEntity.ProductID);
            cmdAdd.Parameters.AddWithValue("@DoctorID", objEntity.DoctorID);
            cmdAdd.Parameters.AddWithValue("@Amount", objEntity.Amount);
            cmdAdd.Parameters.AddWithValue("@AppoinmentDt", objEntity.AppoinmentDt);            
            cmdAdd.Parameters.AddWithValue("@Visited", objEntity.Visited);
            cmdAdd.Parameters.AddWithValue("@Started", objEntity.Started);
            cmdAdd.Parameters.AddWithValue("@CompletionDt", objEntity.CompletionDt);
            cmdAdd.Parameters.AddWithValue("@FinalAmount", objEntity.FinalAmount);
            cmdAdd.Parameters.AddWithValue("@Finished", objEntity.Finished);
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

        public virtual void DeleteInquiryClinicProductByInquiryNo(string InquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryClinicProductByInquiryNo_DEL";
            cmdDel.Parameters.AddWithValue("@InquiryNo", InquiryNo);
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

        /* ------------------------------------------------------------- */
        /* Patient Payment Information                                   */
        /* ------------------------------------------------------------- */
        public virtual List<Entity.PatientPayment> GetIPatientPaymentList(Int64 pkID, Int64 CustomerID, string InquiryNo, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.PatientPayment> lstLocation = new List<Entity.PatientPayment>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "PatientPaymentList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@InquiryNo", InquiryNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.PatientPayment objEntity = new Entity.PatientPayment();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.CustomerType = GetTextVale(dr, "CustomerType");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.PaymentType = GetTextVale(dr, "PaymentType");
                objEntity.PaymentTypeID = GetInt64(dr, "PaymentTypeID");
                objEntity.PaymentDate = GetDateTime(dr, "PaymentDate");
                objEntity.PaymentAmount = GetDecimal(dr, "PaymentAmount");

                lstLocation.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual void AddUpdatePatientPayment(Entity.PatientPayment objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "PatientPayment_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@InquiryNo", objEntity.InquiryNo);
            cmdAdd.Parameters.AddWithValue("@PaymentTypeID", objEntity.PaymentTypeID);
            cmdAdd.Parameters.AddWithValue("@PaymentDate", objEntity.PaymentDate);
            cmdAdd.Parameters.AddWithValue("@PaymentAmount", objEntity.PaymentAmount);
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

        public virtual void DeletePatientPayment(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "PatientPayment_DEL";
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

        public virtual void DeleteInquiryClinicPaymentByInquiryNo(string InquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "InquiryClinicPaymentByInquiryNo_DEL";
            cmdDel.Parameters.AddWithValue("@InquiryNo", InquiryNo);
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

        /* ------------------------------------------------------------- */
        /* Patient Payment Summary ... Dashboard/Graph                   */
        /* ------------------------------------------------------------- */
        public virtual List<Entity.PatientPayment> GetDashboardPatientSummary(string Category, Int64 CustomerID, string InquiryNo, string LoginUserID)
        {
            List<Entity.PatientPayment> lstLocation = new List<Entity.PatientPayment>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardPatientSummary";
            cmdGet.Parameters.AddWithValue("@Category", Category);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@InquiryNo", InquiryNo);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.PatientPayment objEntity = new Entity.PatientPayment();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquirypkID = GetInt64(dr, "InquirypkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.HospitalID = GetInt64(dr, "HospitalID");
                objEntity.HospitalName = GetTextVale(dr, "HospitalName");
                objEntity.DoctorID = GetInt64(dr, "DoctorID");
                objEntity.DoctorName = GetTextVale(dr, "DoctorName");
                objEntity.CompletionDt = GetDateTime(dr, "CompletionDt");
                objEntity.FinalAmount = GetDecimal(dr, "FinalAmount");
                objEntity.PatientPaid = GetDecimal(dr, "PatientPaid");
                objEntity.BilledAmount = GetDecimal(dr, "BilledAmount");
                objEntity.ReceivedAmount = GetDecimal(dr, "ReceivedAmount");
                objEntity.BillNo = GetTextVale(dr, "BillNo");
                objEntity.OrderNo = GetTextVale(dr, "OrderNo");
                objEntity.TreatmentType = GetTextVale(dr, "TreatmentType");
                lstLocation.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstLocation;
        }

        public virtual List<Entity.InquiryInfo> GetInquiryInfoForSalesBill(Int64 pkID, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "InquiryListForSalesBill";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.InquiryInfo> lstObject = new List<Entity.InquiryInfo>();
            while (dr.Read())
            {
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.InquiryNo = GetTextVale(dr, "InquiryNo");
                objEntity.InquiryDate = GetDateTime(dr, "InquiryDate");
                objEntity.ReferenceName = GetTextVale(dr, "ReferenceName");
                objEntity.InquirySource = GetTextVale(dr, "InquirySource");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.SpecialityID = GetInt64(dr, "SpecialityID");
                objEntity.TreatmentType = GetTextVale(dr, "TreatmentType");

                objEntity.EmailAddress = GetTextVale(dr, "EmailAddress");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.Designation = GetTextVale(dr, "Designation");

                objEntity.MeetingNotes = GetTextVale(dr, "MeetingNotes");
                objEntity.FollowupNotes = GetTextVale(dr, "FollowupNotes");
                objEntity.FollowupDate = GetDateTime(dr, "FollowupDate");
                objEntity.InquiryStatusID = GetInt64(dr, "InquiryStatusID");
                objEntity.InquiryStatus = GetTextVale(dr, "InquiryStatus");
                objEntity.TotalAmount = GetDecimal(dr, "TotalAmount");

                objEntity.LastFollowupDate = GetDateTime(dr, "LastFollowupDate");
                objEntity.LastNextFollowupDate = GetDateTime(dr, "LastNextFollowupDate");

                objEntity.CompanyID = GetInt64(dr, "CompanyID");

                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }


    }
}
