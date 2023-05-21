using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class WalletMgmt
    {
        public static List<Entity.Wallet> GetWalletList()
        {
            return (new DAL.WalletSQL().GetWalletList());
        }
        
        public static List<Entity.Wallet> GetWallet(Int64 pkId)
        {
            return (new DAL.WalletSQL().GetWallet(pkId));
        }

        public static List<Entity.Wallet> GetWallet(Int64 pkId, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.WalletSQL().GetWallet(pkId, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateWallet(Entity.Wallet entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WalletSQL().AddUpdateWallet(entity, out ReturnCode, out ReturnMsg);
        }
         
        public static void DeleteWallet(Int64 WalletID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.WalletSQL().DeleteWallet(WalletID, out ReturnCode, out ReturnMsg);
        }
    }
}
