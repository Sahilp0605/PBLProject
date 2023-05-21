using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CourierInfoMgmt
    {
        public static List<Entity.CourierInfo> GetCourierInfoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CourierInfoSQL().GetCourierInfoList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.CourierInfo> GetCourierInfoList(Int64 pkID, string LoginUserID, string SearchKey, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CourierInfoSQL().GetCourierInfoList(pkID, LoginUserID, SearchKey, pMonth, pYear, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCourierInfo(Entity.CourierInfo entity, out int ReturnCode, out string ReturnMsg, out string newSerialNo)
        {
            new DAL.CourierInfoSQL().AddUpdateCourierInfo(entity, out ReturnCode, out ReturnMsg, out newSerialNo);
        }

        public static void DeleteCourierInfo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CourierInfoSQL().DeleteCourierInfo(pkID, out ReturnCode, out ReturnMsg);
        }
        /*---------------------------------------------------------------------------*/
        public static List<Entity.CourierInfo> GetCourierImageList(Int64 pkID, String CourierNo)
        {
            return (new DAL.CourierInfoSQL().GetCourierImageList(pkID, CourierNo));
        }

        public static void AddUpdateCourierImages(Entity.CourierInfo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CourierInfoSQL().AddUpdateCourierImages(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteCourierImage(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CourierInfoSQL().DeleteCourierImage(pkID, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteCourierImageByCourierID(String CourierNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CourierInfoSQL().DeleteCourierImageByCourierID(CourierNo, out ReturnCode, out ReturnMsg);
        }
        /*---------------------------------------------------------------------------*/
    }
}
