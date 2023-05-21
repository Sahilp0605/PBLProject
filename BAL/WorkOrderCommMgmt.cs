using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class WorkOrderCommMgmt
    {
        public static List<Entity.WorkOrderComm> GetWorkOrderCommList(String LoginUserID)
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommList(LoginUserID));
        }

        public static List<Entity.WorkOrderComm> GetWorkOrderCommList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommList(pkID, LoginUserID, "", PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.WorkOrderComm> GetWorkOrderCommList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.WorkOrderComm> GetWorkOrderCommList(string pStatus, string pLoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommList(pStatus, pLoginUserID, pMonth, pYear));
        }
        public static void AddUpdateWorkOrderComm(Entity.WorkOrderComm entity, out int ReturnCode, out string ReturnMsg, out string ReturnOrderNo)
        {
            new DAL.WorkOrderCommSQL().AddUpdateWorkOrderComm(entity, out ReturnCode, out ReturnMsg, out ReturnOrderNo);
        }

        public static void DeleteWorkOrderComm(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WorkOrderCommSQL().DeleteWorkOrderComm(pkID, out ReturnCode, out ReturnMsg);
        }

        // -----------------------------------------------------------------------------
        public static DataTable GetWorkOrderCommDetail(string pOrderNo)
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommDetail(pOrderNo));
        }
        public static List<Entity.WorkOrderCommDetail> GetWorkOrderCommDetailList()
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommDetailList());
        }

        public static List<Entity.WorkOrderCommDetail> GetWorkOrderCommDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.WorkOrderCommSQL().GetWorkOrderCommDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateWorkOrderCommDetail(Entity.WorkOrderCommDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WorkOrderCommSQL().AddUpdateWorkOrderCommDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteWorkOrderCommDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WorkOrderCommSQL().DeleteWorkOrderCommDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteWorkOrderCommDetailByOrderNo(string pOrderNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WorkOrderCommSQL().DeleteWorkOrderCommDetailByOrderNo(pOrderNo, out ReturnCode, out ReturnMsg);
        }
        public static void UpdateWorkOrderApproval(Entity.WorkOrderComm entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WorkOrderCommSQL().UpdateWorkOrderApproval(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
