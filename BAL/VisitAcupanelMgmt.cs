using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class VisitAcupanelMgmt
    {
        // ===========================================================================
        // Complaint Visit Acupanel Detail 
        // ===========================================================================
        public static DataTable GetComplaintVisitDetail(Int64 ComplaintNo,Int64 ParentID)
        {
            return (new DAL.VisitAcupanelSQL().GetComplaintVisitDetail(ComplaintNo, ParentID));
        }

        public static List<Entity.VisitAcupanel> GetComplaintVisitDetailList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.VisitAcupanelSQL().GetComplaintVisitDetailList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateComplaintVisitDetail(Entity.VisitAcupanel objEntity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VisitAcupanelSQL().AddUpdateComplaintVisitDetail(objEntity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteComplaintVisitDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VisitAcupanelSQL().DeleteComplaintVisitDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteComplaintVisitDetailByComplaintNo(Int64 ComplaintNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.VisitAcupanelSQL().DeleteComplaintVisitDetailByComplaintNo(ComplaintNo, out ReturnCode, out ReturnMsg);
        }
    }
}
