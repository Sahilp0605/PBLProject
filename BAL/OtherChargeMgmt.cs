using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class OtherChargeMgmt
    {
        public static List<Entity.OtherCharge> GetOtherChargeList()
        {
            return (new DAL.OtherChargeSQL().GetOtherChargeList());
        }
        public static List<Entity.OtherCharge> GetOtherChargeList(Int64 pkId)
        {
            return (new DAL.OtherChargeSQL().GetOtherChargeList(pkId));
        }

        public static List<Entity.OtherCharge> GetOtherChargeList(Int64 pkId, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.OtherChargeSQL().GetOtherChargeList(pkId, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateOtherCharge(Entity.OtherCharge entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OtherChargeSQL().AddUpdateOtherCharge(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteOtherCharge(Int64 ChargeID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.OtherChargeSQL().DeleteOtherCharge(ChargeID, out ReturnCode, out ReturnMsg);
        }
    }
}
