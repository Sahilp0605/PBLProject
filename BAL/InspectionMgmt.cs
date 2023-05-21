using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class InspectionMgmt
    {
        public static List<Entity.Inspection> GetInspectionList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.InspectionSQL().GetInspectionList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateInspection(Entity.Inspection entity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID)
        {
            new DAL.InspectionSQL().AddUpdateInspection(entity, out ReturnCode, out ReturnMsg, out ReturnpkID);
        }

        public static void DeleteInspection(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InspectionSQL().DeleteInspection(pkID, out ReturnCode, out ReturnMsg);
        }

        // --------------------------------------------------
        public static DataTable GetCheckList(string pCheckHead)
        {
            return (new DAL.InspectionSQL().GetCheckList(pCheckHead));
        }

        public static DataTable GetInspectionDetail(Int64 pRefID)
        {
            return (new DAL.InspectionSQL().GetInspectionDetail(pRefID));
        }
        public static void AddUpdateInspectionDetail(Entity.InspectionDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InspectionSQL().AddUpdateInspectionDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteInspectionDetailByRefID(Int64 RefID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.InspectionSQL().DeleteInspectionDetailByRefID(RefID, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.Material_Report> MovementDetailByInvoiceNo(string InvoiceNo, Int64 PageNo, Int64 PageSize, out int TotalCount)
        {
            return (new DAL.Material_MovementSQL().MovementDetailByInvoiceNo(InvoiceNo, PageNo, PageSize, out TotalCount));
        }
    }
}
