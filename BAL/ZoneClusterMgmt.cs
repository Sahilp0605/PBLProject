using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ZoneClusterMgmt
    {
        public static List<Entity.ZoneCluster> GetZoneClusterList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ZoneClusterSQL().GetZoneClusterList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ZoneCluster> GetZoneClusterList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ZoneClusterSQL().GetZoneClusterList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCluster(Entity.ZoneCluster entity, out int ReturnCode, out string ReturnMsg, out int ReturnClusterID)
        {
            new DAL.ZoneClusterSQL().AddUpdateCluster(entity, out ReturnCode, out ReturnMsg, out ReturnClusterID);
        }

        public static void AddUpdateClusterDetail(Entity.ZoneCluster entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ZoneClusterSQL().AddUpdateClusterDetail(entity, out ReturnCode, out ReturnMsg);
        }

        //public static void DeleteCluster(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        //{
        //    new DAL.ZoneClusterSQL().DeleteCluster(pkID, out ReturnCode, out ReturnMsg);
        //}

    }
}
