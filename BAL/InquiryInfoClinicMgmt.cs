using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace BAL
{
    public class InquiryInfoClinicMgmt
    {
        //public static List<Entity.InquiryInfo> GetInquiryInfoClinicList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        public static  List<Entity.InquiryInfo> GetInquiryInfoClinicList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            //return (new DAL.InquiryInfoClinicSQL().GetInquiryInfoClinicList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
            return (new DAL.InquiryInfoClinicSQL().GetInquiryInfoClinicList(pStatus, pLoginUserID, pMonth, pYear));
        }
        public static List<Entity.InquiryInfo> GetInquiryInfoClinicList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InquiryInfoClinicSQL().GetInquiryInfoClinicList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateInquiryInfoClinic(Entity.InquiryInfo entity, out int ReturnCode, out string ReturnMsg, out string newInqNo)
        {
            new DAL.InquiryInfoClinicSQL().AddUpdateInquiryInfoClinic(entity, out ReturnCode, out ReturnMsg, out newInqNo);
        }

        public static void DeleteInquiryInfoClinic(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoClinicSQL().DeleteInquiryInfoClinic(pkID, out ReturnCode, out ReturnMsg);
        }


        /*-------------------------------------------------------------*/

        public static DataTable GetInquiryClinicProductDetail(string pInquiryNo)
        {
            return (new DAL.InquiryInfoClinicSQL().GetInquiryClinicProductDetail(pInquiryNo));
        }

        public static void AddUpdateInquiryClinicProduct(Entity.InquiryInfo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoClinicSQL().AddUpdateInquiryClinicProduct(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInquiryClinicProductByInquiryNo(string pInquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoClinicSQL().DeleteInquiryClinicProductByInquiryNo(pInquiryNo, out ReturnCode, out ReturnMsg);
        }
        /* ------------------------------------------------------------- */
        /* Patient Payment Information */
        /* ------------------------------------------------------------- */
        public static List<Entity.PatientPayment> GetPatientPaymentList(Int64 pkID, Int64 CustomerID, string InquiryNo, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InquiryInfoClinicSQL().GetIPatientPaymentList(pkID, CustomerID, InquiryNo, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdatePatientPayment(Entity.PatientPayment entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoClinicSQL().AddUpdatePatientPayment(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeletePatientPayment(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoClinicSQL().DeletePatientPayment(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInquiryClinicPaymentByInquiryNo(string pInquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InquiryInfoClinicSQL().DeleteInquiryClinicPaymentByInquiryNo(pInquiryNo, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.PatientPayment> GetDashboardPatientSummary(string Category, Int64 CustomerID, string InquiryNo, string LoginUserID)
        {
            return (new DAL.InquiryInfoClinicSQL().GetDashboardPatientSummary(Category, CustomerID, InquiryNo, LoginUserID));
        }
    }
}
