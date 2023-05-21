using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class MSTExpenseMgmt
    {
        public static List<Entity.MSTExpense> GetExpenseList()
        {
            return (new DAL.MSTExpenseSQL().GetExpenseList());
        }
    }
}
