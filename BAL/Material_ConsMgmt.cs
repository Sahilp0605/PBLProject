using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class Material_ConsMgmt
    {
        public static List<Entity.Material_Cons> GetMaterial_ConsList(String LoginUserID)
        {
            return (new DAL.Material_ConsSQL().GetMaterial_ConsList(LoginUserID));
        }

        public static List<Entity.Material_Cons> GetMaterial_Cons(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_ConsSQL().GetMaterial_Cons(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Material_Cons> GetMaterial_Cons(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_ConsSQL().GetMaterial_Cons(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMaterial_Cons(Entity.Material_Cons entity, out int ReturnCode, out string ReturnMsg, out int ReturnpkID)
        {
            new DAL.Material_ConsSQL().AddUpdateMaterial_Cons(entity, out ReturnCode, out ReturnMsg, out ReturnpkID);
        }

        public static void DeleteMaterial_Cons(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_ConsSQL().DeleteMaterial_Cons(pkID, out ReturnCode, out ReturnMsg);
        }
        // --------------------------------------------------------------------------
        // Detail Section 
        // --------------------------------------------------------------------------
        public static List<Entity.Material_ConsDetail> GetMaterial_ConsDetailList(String LoginUserID)
        {
            return (new DAL.Material_ConsSQL().GetMaterial_ConsDetailList(LoginUserID));
        }

        public static List<Entity.Material_ConsDetail> GetMaterial_ConsDetail(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_ConsSQL().GetMaterial_ConsDetail(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Material_ConsDetail> GetMaterial_ConsDetail(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Material_ConsSQL().GetMaterial_ConsDetail(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMaterial_ConsDetail(Entity.Material_ConsDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_ConsSQL().AddUpdateMaterial_ConsDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMaterial_ConsDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_ConsSQL().DeleteMaterial_ConsDetail(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteMaterial_ConsDetailByNo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Material_ConsSQL().DeleteMaterial_ConsDetailByNo(pkID, out ReturnCode, out ReturnMsg);
        }

        //--------------------------------Material Movement Product Detail--------------------

        public static DataTable GetMaterialProductDetail(Int64 pInquiryNo)
        {
            return (new DAL.Material_ConsSQL().GetMaterialProductDetail(pInquiryNo));
        }

    }
}
