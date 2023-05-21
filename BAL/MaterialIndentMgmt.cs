using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class MaterialIndentMgmt
    {
        public static List<Entity.MaterialIndent> GetMaterialIndentList(String LoginUserID)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndentList(LoginUserID));
        }
        public static List<Entity.MaterialIndent> GetMaterialIndentList(String pApprovalStatus, String LoginUserID)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndentList(pApprovalStatus, LoginUserID));
        }

        public static List<Entity.MaterialIndent> GetMaterialIndent(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndent(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.MaterialIndent> GetMaterialIndent(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndent(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.MaterialIndent_detail> GetMaterialIndentDetailListByProduct(Int64 pProductID)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndentDetailListByProduct(pProductID));
        }
        //--------------------------------------Insert/Update------------------------------------------
        public static void AddUpdateMaterialIndent(Entity.MaterialIndent entity, out int ReturnCode, out string ReturnMsg, out string ReturnIndentNo)
        {
            new DAL.MaterialIndentSQL().AddUpdateMaterialIndent(entity, out ReturnCode, out ReturnMsg, out ReturnIndentNo);
        }

        public static void DeleteMaterialIndent(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MaterialIndentSQL().DeleteMaterialIndent(pkID, out ReturnCode, out ReturnMsg);
        }
        //----------------------------------------------Detail List--------------------------------------

        public static DataTable GetMaterialIndentDetail(string pIndentNo)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndentDetail(pIndentNo));
        }

        public static List<Entity.MaterialIndent_detail> GetMaterialIndentDetailList()
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndentDetailList());
        }

        public static List<Entity.MaterialIndent_detail> GetMaterialIndentDetailList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.MaterialIndentSQL().GetMaterialIndentDetailList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMaterialIndentDetail(Entity.MaterialIndent_detail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MaterialIndentSQL().AddUpdateMaterialIndentDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteMaterialIndentDetailByIndentNo(string pOutwardNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MaterialIndentSQL().DeleteMaterialIndentDetailByIndentNo(pOutwardNo, out ReturnCode, out ReturnMsg);
        }

        public static void UpdateIndentApproval(Entity.MaterialIndent entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.MaterialIndentSQL().UpdateIndentApproval(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
