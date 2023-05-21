using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ContractInfoMgmt
    {
        public static DataTable GetContractInfoProductDetail(string pInquiryNo)
        {
            return (new DAL.ContractInfoSQL().GetContractProductDetail(pInquiryNo));
        }

        public static List<Entity.ContractInfo> GetContractInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ContractInfoSQL().GetContractInfoList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ContractInfo> GetContractInfoList(Int64 pkID, string LoginUserID, string SearchKey, string pStatus, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ContractInfoSQL().GetContractInfoList(pkID, LoginUserID, SearchKey, pStatus, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ContractInfo> GetContractInfoListByCustomer(Int64 pCustomerID)
        {
            return (new DAL.ContractInfoSQL().GetContractInfoListByCustomer(pCustomerID));
        }

        public static void AddUpdateContractInfo(Entity.ContractInfo entity, out int ReturnCode, out string ReturnMsg, out string newInqNo)
        {
            new DAL.ContractInfoSQL().AddUpdateContractInfo(entity, out ReturnCode, out ReturnMsg, out newInqNo);
        }

        public static void DeleteContractInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ContractInfoSQL().DeleteContractInfo(pkID, out ReturnCode, out ReturnMsg);
        }

        public static List<Entity.ContractInfo> GetContractInfoProductGroupList(string pInquiryNo)
        {
            return (new DAL.ContractInfoSQL().GetContractInfoProductGroupList(pInquiryNo));
        }

        public static void AddUpdateContractInfoProduct(Entity.ContractInfo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ContractInfoSQL().AddUpdateContractInfoProduct(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteContractInfoProductByInquiryNo(string pInquiryNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ContractInfoSQL().DeleteContractInfoProductByInquiryNo(pInquiryNo, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteContractInfoProduct(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ContractInfoSQL().DeleteContractInfoProduct(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
