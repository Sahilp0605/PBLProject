using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class AccountLedgerMgmt
    {
        public static List<Entity.AccountLedger> GetAccountLedgerList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.AccountLedgerSQL().GetAccountLedgerList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.AccountLedger> GetAccountLedgerByUser(string LoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.AccountLedgerSQL().GetAccountLedgerByUser(LoginUserID, pMonth, pYear));
        }

        public static List<Entity.AccountLedger> GetDashboardAccountLedgerList(String FollowupStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.AccountLedgerSQL().GetDashboardAccountLedgerList(FollowupStatus, pMonth, pYear, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Insert & Update
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static void AddUpdateAccountLedger(Entity.AccountLedger entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AccountLedgerSQL().AddUpdateAccountLedger(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteAccountLedger(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.AccountLedgerSQL().DeleteAccountLedger(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
