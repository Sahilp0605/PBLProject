using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class JobWork_MovementMgmt
    {
        //--------------------------JobWork Movement----------------------------------
        public static List<Entity.JobWork_Movement> GetJobWork_MovementList(String LoginUserID, string TransType)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWork_MovementList(LoginUserID, TransType));
        }

        public static List<Entity.JobWork_Movement> GetJobWork_Movement(Int64 pkID, string LoginUserID, string TransType, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWork_Movement(pkID, LoginUserID, TransType, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JobWork_Movement> GetJobWork_Movement(Int64 pkID, string LoginUserID, string SearchKey, string TransType, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWork_Movement(pkID, LoginUserID, SearchKey, TransType, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateJobWork_Movement(Entity.JobWork_Movement entity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID)
        {
            new DAL.JobWork_MovementSQL().AddUpdateJobWork_Movement(entity, out ReturnCode, out ReturnMsg, out ReturnpkID);
        }

        public static void DeleteJobWork_Movement(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobWork_MovementSQL().DeleteJobWork_Movement(pkID, out ReturnCode, out ReturnMsg);
        }

        //--------------------------JobWork Movement Detail----------------------------------

        public static List<Entity.JobWork_MovementDetail> GetJobWork_MovementDetailList(String LoginUserID)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWork_MovementDetailList(LoginUserID));
        }

        public static List<Entity.JobWork_MovementDetail> GetJobWork_MovementDetail(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWork_MovementDetail(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JobWork_MovementDetail> GetJobWork_MovementDetail(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWork_MovementDetail(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateJobWork_MovementDetail(Entity.JobWork_MovementDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobWork_MovementSQL().AddUpdateJobWork_MovementDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJobWork_MovementDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobWork_MovementSQL().DeleteJobWork_MovementDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJobWork_MovementDetailByNo(Int64 InquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.JobWork_MovementSQL().DeleteJobWork_MovementDetailByNo(InquiryNo, out ReturnCode, out ReturnMsg);
        }

        
        //--------------------------------Material Movement Product Detail--------------------

        public static DataTable GetJobWorkProductDetail(Int64 pInquiryNo)
        {
            return (new DAL.JobWork_MovementSQL().GetJobWorkProductDetail(pInquiryNo));
        }

        public static List<Entity.JobWork_MovementDetail> GetOrderProductList(string OrderNo, string LoginUserID)
        {
            return (new DAL.JobWork_MovementSQL().GetOrderProductList(OrderNo, LoginUserID));
        }
    }
}
