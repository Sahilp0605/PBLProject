using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Rojmel
    {
        public Int64 Rownum { get; set; }
        public decimal Opening { get; set; }
        public decimal IncomeRs { get; set; }
        public Int64 IncomeCustCode { get; set; }
        public string Income { get; set; }
        public decimal IncomeClosing { get; set; }
        public string IncomeRemark { get; set; }
        public decimal ExpenseRs { get; set; }
        public Int64 ExpenseCustCode { get; set; }
        public string Expense { get; set; }
        public decimal ExpenseClosing { get; set; }
        public string ExpenseRemark { get; set; }
   } 
}
