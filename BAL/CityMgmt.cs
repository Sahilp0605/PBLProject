using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CityMgmt
    {
        public static List<Entity.City> GetCityList()
        {
            return (new DAL.CitySQL().GetCityList());
        }

        public static List<Entity.City> GetCity(Int64 CityCode,string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CitySQL().GetCity(CityCode, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.City> GetCity(Int64 CityCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CitySQL().GetCity(CityCode, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.City> GetCityByState(Int64 StateCode)
        {
            return (new DAL.CitySQL().GetCityByState(StateCode));
        }


        public static void AddUpdateCity(Entity.City entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CitySQL().AddUpdateCity(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCity(Int64 CityCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CitySQL().DeleteCity(CityCode, out ReturnCode, out ReturnMsg);
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public static List<Entity.City> GetCityListForDropdown(String CityName, Int64 StateCode)
        {
            return (new DAL.CitySQL().GetCityListForDropdown(CityName, StateCode));
        }

        public static List<Entity.State> GetStateListForDropdown(String StateName, Int64 CountryCode)
        {
            return (new DAL.CitySQL().GetStateListForDropdown(StateName, CountryCode));
        }

    }
}
