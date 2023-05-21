using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Expense
    {
        public Int64  pkID	{ get; set; }
        public DateTime ExpenseDate	{ get; set; }
        public Int64 ExpenseTypeId	{ get; set; }
        public string ExpenseTypeName { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerName { get; set; }
        public Decimal Amount	{ get; set; }
        public String ExpenseNotes	{ get; set; }
        public String ExpenseImage	{ get; set; }
        public String FromLocation	{ get; set; }
        public String ToLocation	{ get; set; }
        public decimal DistanceCovered { get; set; }
        public string LoginUserID { get; set; }

        /*-----------------------------------*/
        public Int64 ExpenseID { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public String FileData { get; set; }
        /*-----------------------------------*/
    }

    public class Expense_Report
    {
        public Int64 pkID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Int64 ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public string EmployeeName { get; set; }
        public Decimal Amount { get; set; }
        public String ExpenseNotes { get; set; }
        public String ExpenseImage { get; set; }
        public String FromLocation { get; set; }
        public String ToLocation { get; set; }
        public Int64 Kilometers { get; set; }
        public string LoginUserID { get; set; }

        /*-----------------------------------*/
        public Int64 ExpenseID { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public String FileData { get; set; }
        /*-----------------------------------*/

        public String CreatedBy { get; set; }
        public String CreatedEmployee { get; set; }
    }

    public class OfficeExpense
    {
        public Int64 pkID { get; set; }
        public Int64 RefpkID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public String ExpenseNotes { get; set; }
        public Int64 ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public Decimal Amount { get; set; }
        public String Remarks { get; set; }
        public String Voucher { get; set; }
        public String FromLocation { get; set; }
        public String ToLocation { get; set; }
        public Int64 Kilometers { get; set; }
        public string LoginUserID { get; set; }

        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String CreatedEmployee { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String UpdatedEmployee { get; set; }

        public DateTime ExpenseDateDetail { get; set; }
    }
}
