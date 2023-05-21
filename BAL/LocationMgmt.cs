using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
  public  class LocationMgmt
    {
        public static List<Entity.Location> GetLocationList(String LoginUserID)
        {
            return (new DAL.LocationSQL().GetLocationList(LoginUserID));
        }

        public static List<Entity.Location> GetLocation(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.LocationSQL().GetLocation(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Location> GetLocation(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.LocationSQL().GetLocation(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateLocation(Entity.Location entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LocationSQL().AddUpdateLocation(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteLocation(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.LocationSQL().DeleteLocation(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
