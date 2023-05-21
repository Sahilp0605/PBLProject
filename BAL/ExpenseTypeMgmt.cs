using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ExpenseTypeMgmt
    {
        public static List<Entity.ExpenseType> GetExpenseTypeList(Int64 ExpenseTypeID)
        {
            return (new DAL.ExpenseTypeSQL().GetExpenseTypeList(ExpenseTypeID));
        }

        public static List<Entity.ExpenseType> GetExpenseTypeList(Int64 ExpenseTypeID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ExpenseTypeSQL().GetExpenseTypeList(ExpenseTypeID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord)); ;
        }

        public static void AddUpdateExpenseType(Entity.ExpenseType entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseTypeSQL().AddUpdateExpenseType(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteExpenseType(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ExpenseTypeSQL().DeleteExpenseType(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
