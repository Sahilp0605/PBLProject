using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class BundleMgmt
    {
        public static List<Entity.Bundle> GetBundleList()
        {
            return (new DAL.BundleSQL().GetBundleList());
        }
        public static List<Entity.Bundle> GetBundle(Int64 pkId)
        {
            return (new DAL.BundleSQL().GetBundle(pkId));
        }

        public static void AddUpdateBundle(Entity.Bundle entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BundleSQL().AddUpdateBundle(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteBundle(Int64 BundleID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BundleSQL().DeleteBundle(BundleID, out ReturnCode, out ReturnMsg);
        }

    }
}
