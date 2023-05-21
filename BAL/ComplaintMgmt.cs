using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ComplaintMgmt
    {
        // ===========================================================================
        // Complaint Information
        // ===========================================================================

        public static List<Entity.Complaint> GetComplaintList()
        {
            return (new DAL.ComplaintSQL().GetComplaintList());
        }
        public static List<Entity.Complaint> GetComplaintList(Int64 pkID, string LoginUserID)
        {
            return (new DAL.ComplaintSQL().GetComplaintList(pkID, LoginUserID));
        }

        public static List<Entity.Complaint> GetComplaintList(Int64 pkID, Int64 CustomerID, string ComplaintStatus, string LoginUserID)
        {
            return (new DAL.ComplaintSQL().GetComplaintList(pkID, CustomerID, ComplaintStatus, LoginUserID));
        }

        public static List<Entity.Complaint> GetComplaintList(Int64 pkID, Int64 CustomerID, string ComplaintStatus, Int64 pMon, Int64 pYear, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ComplaintSQL().GetComplaintList(pkID, CustomerID, ComplaintStatus, pMon, pYear, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Complaint> GetComplaintList(Int64 CustomerID, string ComplaintStatus, string LoginUserID)
        {
            return (new DAL.ComplaintSQL().GetComplaintList(CustomerID, ComplaintStatus, LoginUserID));
        }
        public static List<Entity.Complaint> GetComplaintListByComplaintNo(Int64 ComplaintNo, string ComplaintStatus, string LoginUserID)
        {
            return (new DAL.ComplaintSQL().GetComplaintListByComplaintNo(ComplaintNo, ComplaintStatus, LoginUserID));
        }
        public static void AddUpdateComplaint(Entity.Complaint entity, out int ReturnCode, out string ReturnMsg, out string ReturnComplaintNo)
        {
            new DAL.ComplaintSQL().AddUpdateComplaint(entity, out ReturnCode, out ReturnMsg, out ReturnComplaintNo);
        }

        public static void AddUpdateComplaintQuick(Entity.Complaint entity, out int ReturnCode, out string ReturnMsg, out string ReturnComplaintNo)
        {
            new DAL.ComplaintSQL().AddUpdateComplaintQuick(entity, out ReturnCode, out ReturnMsg, out ReturnComplaintNo);
        }

        public static void DeleteComplaint(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ComplaintSQL().DeleteComplaint(pkID, out ReturnCode, out ReturnMsg);
        }

        // ===========================================================================
        // Complaint Visit (Response) 
        // ===========================================================================
        public static List<Entity.ComplaintVisit> GetComplaintVisitList(Int64 pkID, Int64 ComplaintNo, Int64 EmployeeID, Int64 CustomerID, string ComplaintStatus, string pSearchKey, string LoginUserID)
        {
            return (new DAL.ComplaintSQL().GetComplaintVisitList(pkID, ComplaintNo, EmployeeID, CustomerID, ComplaintStatus, pSearchKey, LoginUserID));
        }

        public static void AddUpdateComplaintVisit(Entity.ComplaintVisit entity, out int ReturnCode, out string ReturnMsg,out int ReturnpkID, out string ReturnComplaintNo)
        {
            new DAL.ComplaintSQL().AddUpdateComplaintVisit(entity, out ReturnCode, out ReturnMsg,out ReturnpkID, out ReturnComplaintNo);
        }
        public static void AddUpdateComplaintVisitAccupanel(Entity.ComplaintVisit entity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID, out string ReturnComplaintNo)
        {
            new DAL.ComplaintSQL().AddUpdateComplaintVisitAccupanel(entity, out ReturnCode, out ReturnMsg, out ReturnpkID, out ReturnComplaintNo);
        }
        public static void DeleteComplaintVisit(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ComplaintSQL().DeleteComplaintVisit(pkID, out ReturnCode, out ReturnMsg);
        }

        //// ===========================================================================
        //// Complaint Visit Acupanel Detail 
        //// ===========================================================================
        //public static DataTable GetComplaintVisitDetail(Int64 ComplaintNo)
        //{
        //    return (new DAL.ComplaintSQL().GetComplaintVisitDetail(ComplaintNo));
        //}

        //public static List<Entity.ComplaintVisit> GetComplaintVisitDetailList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        //{
        //    return (new DAL.ComplaintSQL().GetComplaintVisitDetailList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        //}

        //public static void AddUpdateComplaintVisitDetail(Entity.ComplaintVisit entity, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.ComplaintSQL().AddUpdateComplaintVisitDetail(entity, out ReturnCode, out ReturnMsg);
        //}

        //public static void DeleteComplaintVisitDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.ComplaintSQL().DeleteComplaintVisitDetail(pkID, out ReturnCode, out ReturnMsg);
        //}
    }
}
