using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class FinancialTransMgmt
    {
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Bank & Cash Transaction 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.FinancialTrans> GetFinancialTransList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetFinancialTransList(pkID,LoginUserID,PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.FinancialTrans> GetFinancialTransList(Int64 pkID, string LoginUserID, string SearchKey,string TrType, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetFinancialTransList(pkID, LoginUserID, SearchKey, TrType, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.FinancialTrans> GetBankListByName(string pBankName)
        {
            return (new DAL.FinancialTransSQL().GetBankListByName(pBankName));
        }
        //public static List<Entity.FinancialTrans> GetFinancialTransByUser(string LoginUserID, string TransCategory, Int64 pMonth, Int64 pYear)
        //{
        //    return (new DAL.FinancialTransSQL().GetFinancialTransByUser(LoginUserID, TransCategory, pMonth, pYear));
        //}

        //public static List<Entity.FinancialTrans> GetDashboardFinancialTransList(String FollowupStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        //{
        //    return (new DAL.FinancialTransSQL().GetDashboardFinancialTransList(FollowupStatus, pMonth, pYear, LoginUserID, PageNo, PageSize, out TotalRecord));
        //}

        // ==========================================================================
        // Insert & Update
        // ==========================================================================
        public static void AddUpdateFinancialTrans(Entity.FinancialTrans entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().AddUpdateFinancialTrans(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteFinancialTrans(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteFinancialTrans(pkID, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : FINANCIAL TRANSACTION DETAIL
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.FinancialTransDetail> GetFinancialTransDetailList(Int64 ParentID, string InvoiceNo, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetFinancialTransDetailList(ParentID, InvoiceNo, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateFinancialTransDetail(Entity.FinancialTransDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().AddUpdateFinancialTransDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteFinancialTransDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteFinancialTransDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteFinancialTransDetailByParentID(Int64 ParentID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteFinancialTransDetailByParentID(ParentID, out ReturnCode, out ReturnMsg);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : PETTY CASH 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.JournalVoucher> GetPettyCashList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetPettyCashList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JournalVoucher> GetPettyCashList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetPettyCashList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdatePettyCash(Entity.JournalVoucher entity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            new DAL.FinancialTransSQL().AddUpdatePettyCash(entity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
        }

        public static void DeletePettyCash(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeletePettyCash(pkID, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : DEBIT NOTE
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.JournalVoucher> GetDBCRNoteList(Int64 pkID, string DBC, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetDBCRNoteList(pkID, DBC, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JournalVoucher> GetDBCRNoteList(Int64 pkID, string DBC, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetDBCRNoteList(pkID, DBC, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateDBCRNote(Entity.JournalVoucher entity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            new DAL.FinancialTransSQL().AddUpdateDBCRNote(entity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
        }

        public static List<Entity.DBNote> GetDBCRList(Int64 pkID, string DBC, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetDBCRList(pkID, DBC, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static void AddUpdateDBCRNote(Entity.DBNote entity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            new DAL.FinancialTransSQL().AddUpdateDBCRNote(entity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
        }
        public static void AddUpdateDBCRNoteDetail(Entity.DBNote_Detail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().AddUpdateDBCRNoteDetail(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteDBCRNoteDetailByVoucherNo(string VoucherNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteDBCRNoteDetailByVoucherNo(VoucherNo, out ReturnCode, out ReturnMsg);
        }
        public static DataTable GetDBCRNoteDetail(string pVoucherNo)
        {
            return (new DAL.FinancialTransSQL().GetDBCRNoteDetail(pVoucherNo));
        }
        public static void DeleteDBCRNote(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteDBCRNote(pkID, out ReturnCode, out ReturnMsg);
        }


        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : JOURNAL MASTER 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.JournalVoucher> GetJournalVoucherList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetJournalVoucherList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JournalVoucher> GetJournalVoucherList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetJournalVoucherList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateJournalVoucher(Entity.JournalVoucher entity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            new DAL.FinancialTransSQL().AddUpdateJournalVoucher(entity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
        }

        public static void DeleteJournalVoucher(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteJournalVoucher(pkID, out ReturnCode, out ReturnMsg);
        }
        // ----------------------------------------------------
        public static List<Entity.JournalVoucherDetail> GetJournalVoucherDetailList(Int64 pkID, string VoucherNo, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetJournalVoucherDetailList(pkID, VoucherNo, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateJournalVoucherDetail(Entity.JournalVoucherDetail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().AddUpdateJournalVoucherDetail(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteJournalVoucherDetail(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteJournalVoucherDetail(pkID, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // MODULE : EXPENSE VOUCHER 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.JournalVoucher> GetExpenseVoucherList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetExpenseVoucherList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.JournalVoucher> GetExpenseVoucherList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.FinancialTransSQL().GetExpenseVoucherList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateExpenseVoucher(Entity.JournalVoucher entity, out int ReturnCode, out string ReturnMsg, out string ReturnVoucherNo)
        {
            new DAL.FinancialTransSQL().AddUpdateExpenseVoucher(entity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
        }

        public static void DeleteExpenseVoucher(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.FinancialTransSQL().DeleteExpenseVoucher(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
