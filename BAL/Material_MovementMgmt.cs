using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class Material_MovementMgmt
    {
        //--------------------------Material Movement----------------------------------
        public static List<Entity.Material_Movement> GetMaterial_MovementList(String LoginUserID, string TransType)
        {
            return (new DAL.Material_MovementSQL().GetMaterial_MovementList(LoginUserID, TransType));
        }

        public static List<Entity.Material_Movement> GetMaterial_Movement(Int64 pkID, string LoginUserID, string TransType, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_MovementSQL().GetMaterial_Movement(pkID, LoginUserID, TransType, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Material_Movement> GetMaterial_Movement(Int64 pkID, string LoginUserID, string SearchKey, string TransType, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_MovementSQL().GetMaterial_Movement(pkID, LoginUserID, SearchKey, TransType, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMaterial_Movement(Entity.Material_Movement entity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID)
        {
            new DAL.Material_MovementSQL().AddUpdateMaterial_Movement(entity, out ReturnCode, out ReturnMsg, out ReturnpkID);
        }

        public static void DeleteMaterial_Movement(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_MovementSQL().DeleteMaterial_Movement(pkID, out ReturnCode, out ReturnMsg);
        }

        //--------------------------Material Movement Detail----------------------------------

        public static List<Entity.Material_MovementDetail> GetMaterial_MovementDetailList(String LoginUserID)
        {
            return (new DAL.Material_MovementSQL().GetMaterial_MovementDetailList(LoginUserID));
        }

        public static List<Entity.Material_MovementDetail> GetMaterial_MovementDetail(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_MovementSQL().GetMaterial_MovementDetail(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Material_MovementDetail> GetMaterial_MovementDetail(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_MovementSQL().GetMaterial_MovementDetail(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMaterial_MovementDetail(Entity.Material_MovementDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_MovementSQL().AddUpdateMaterial_MovementDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMaterial_MovementDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_MovementSQL().DeleteMaterial_MovementDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMaterial_MovementDetailByNo(Int64 InquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_MovementSQL().DeleteMaterial_MovementDetailByNo(InquiryNo, out ReturnCode, out ReturnMsg);
        }
       

        //--------------------------------Material Movement Product Detail--------------------

        public static DataTable GetMaterialProductDetail(Int64 pInquiryNo)
        {
            return (new DAL.Material_MovementSQL().GetMaterialProductDetail(pInquiryNo));
        }
    }
}
