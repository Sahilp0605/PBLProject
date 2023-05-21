using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ActivityMgmt
    {
        public static List<Entity.Activity> GetActivityList()
        {
            return (new DAL.ActivitySQL().GetActivityList());
        }

        public static List<Entity.Activity> GetActivity(string ActivityCode)
        {
            return (new DAL.ActivitySQL().GetActivity(ActivityCode));
        }

        public static void AddUpdateActivity(Entity.Activity entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ActivitySQL().AddUpdateActivity(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteActivity(string ActivityCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ActivitySQL().DeleteActivity(ActivityCode, out ReturnCode, out ReturnMsg);
        }
    }
}
