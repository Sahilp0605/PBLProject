using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class LoanMgmt
    {
        public static List<Entity.Loan> GetLoanList(String LoanCategory, String LoginUserID)
        {
            return (new DAL.LoanSQL().GetLoanList(LoanCategory, LoginUserID));
        }

        public static List<Entity.Loan> GetLoan(String LoanCategory, Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageBatch, out int TotalRecord)
        {
            return (new DAL.LoanSQL().GetLoan(LoanCategory, pkID, LoginUserID, SearchKey, PageNo, PageBatch, out TotalRecord));
        }

        public static void AddUpdateLoan(Entity.Loan entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LoanSQL().AddUpdateLoan(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteLoan(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LoanSQL().DeleteLoan(pkID, out ReturnCode, out ReturnMsg);
        }
        public static List<Entity.Loan> GetLoanApprovalList(String LoanCategory, String ApprovalStatus, String LoginUserID)
        {
            return (new DAL.LoanSQL().GetLoanApprovalList(LoanCategory, ApprovalStatus, LoginUserID));
        }
        public static void UpdateLoanApproval(Entity.Loan entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LoanSQL().UpdateLoanApproval(entity, out ReturnCode, out ReturnMsg);
        }
        public static Decimal GetLoanInstallmentAmount(String pLoanCategory, Int64 pEmployeeID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.LoanSQL().GetLoanInstallmentAmount(pLoanCategory, pEmployeeID, pMonth, pYear));
        }
    }
}
