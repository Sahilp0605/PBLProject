using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class HolidayMgmt
    {
        public static List<Entity.Holiday> GetHolidayList()
        {
            return (new DAL.HolidaySQL().GetHolidayList());
        }

        public static List<Entity.Holiday> GetHolidayList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.HolidaySQL().GetHolidayList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Holiday> GetHolidayList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.HolidaySQL().GetHolidayList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Holiday> GetHolidayList(string HolidayType)
        {
            return (new DAL.HolidaySQL().GetHolidayListByName(HolidayType));
        }

        public static List<Entity.Holiday> GetHolidayListByCount(Int64 pMonth, Int64 pYear)
        {
            return (new DAL.HolidaySQL().GetHolidayListByCount(pMonth, pYear));
        }


        public static void AddUpdateHoliday(Entity.Holiday entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.HolidaySQL().AddUpdateHoliday(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteHoliday(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.HolidaySQL().DeleteHoliday(pkID, out ReturnCode, out ReturnMsg);
        }

    }
}
