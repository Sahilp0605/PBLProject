using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ConstantMgmt
    {
        public static List<Entity.Constant> GetConstantList()
        {
            return (new DAL.ConstantSQL().GetConstantList());
        }
         
        public static List<Entity.Constant> GetConstantList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ConstantSQL().GetConstantList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateConstant(Entity.Constant entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ConstantSQL().AddUpdateConstant(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteConstant(Int64 pkId, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ConstantSQL().DeleteConstant(pkId, out ReturnCode, out ReturnMsg);
        }
    }
}
