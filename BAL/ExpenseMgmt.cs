using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ExpenseMgmt
    {
        public static List<Entity.Expense> GetExpenseList(string LoginUserID)
        {
            return (new DAL.ExpenseSQL().GetExpenseList(LoginUserID));
        }
        
        public static List<Entity.Expense> GetExpense(Int64 pkID, string LoginUserID)
        {
            return (new DAL.ExpenseSQL().GetExpense(pkID,LoginUserID));
        }

        public static List<Entity.Expense> GetExpenseList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExpenseSQL().GetExpenseList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateExpense(Entity.Expense entity, out int ReturnCode, out string ReturnMsg, out int ReturnExpenseId)
        {
            new DAL.ExpenseSQL().AddUpdateExpense(entity, out ReturnCode, out ReturnMsg, out ReturnExpenseId);
        }

        public static void DeleteExpense(Int64 ExpenseId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseSQL().DeleteExpense(ExpenseId, out ReturnCode, out ReturnMsg);
        }
 
        /*---------------------------------------------------------------------------*/
        public static List<Entity.Expense> GetExpenseImageList(Int64 pkID, Int64 ExpenseID)
        {
            return (new DAL.ExpenseSQL().GetExpenseImageList(pkID, ExpenseID));
        }

        public static void AddUpdateExpenseImages(Entity.Expense entity,  out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseSQL().AddUpdateExpenseImages(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteExpenseImage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseSQL().DeleteExpenseImage(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteExpenseImageByExpenseID(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseSQL().DeleteExpenseImageByExpenseID(pkID, out ReturnCode, out ReturnMsg);
        }
        /*---------------------------------------------------------------------------*/
        public static List<Entity.OfficeExpense> GetMultiExpenseList(string LoginUserID)
        {
            return (new DAL.ExpenseSQL().GetMultiExpenseList(LoginUserID));
        }

        public static List<Entity.OfficeExpense> GetMultiExpenseList(Int64 pkID, string LoginUserID)
        {
            return (new DAL.ExpenseSQL().GetMultiExpenseList(pkID, LoginUserID));
        }

        public static List<Entity.OfficeExpense> GetMultiExpenseList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExpenseSQL().GetMultiExpenseList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMultiExpense(Entity.OfficeExpense entity, out int ReturnCode, out string ReturnMsg, out int ReturnExpenseId)
        {
            new DAL.ExpenseSQL().AddUpdateMultiExpense(entity, out ReturnCode, out ReturnMsg, out ReturnExpenseId);
        }

        public static void DeleteMultiExpense(Int64 ExpenseId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseSQL().DeleteMultiExpense(ExpenseId, out ReturnCode, out ReturnMsg);
        }
        //**************************************************************//
        public static List<Entity.OfficeExpense> GetMultiExpenseDetailList(string LoginUserID)
        {
            return (new DAL.ExpenseSQL().GetMultiExpenseDetailList(LoginUserID));
        }

        public static List<Entity.OfficeExpense> GetMultiExpenseDetailList(Int64 pkID, string LoginUserID)
        {
            return (new DAL.ExpenseSQL().GetMultiExpenseDetailList(pkID, LoginUserID));
        }

        public static List<Entity.OfficeExpense> GetMultiExpenseDetailList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExpenseSQL().GetMultiExpenseDetailList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateMultiExpenseDetail(Entity.OfficeExpense entity, out int ReturnCode, out string ReturnMsg, out Int64 RetExpDetailId)
        {
            new DAL.ExpenseSQL().AddUpdateMultiExpenseDetail(entity, out ReturnCode, out ReturnMsg,out RetExpDetailId);
        }
        public static void DeleteMultiExpenseDetailByExpenseNo(Int64 ExpenseId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseSQL().DeleteMultiExpenseDetailByExpenseNo(ExpenseId, out ReturnCode, out ReturnMsg);
        }
    }
}
