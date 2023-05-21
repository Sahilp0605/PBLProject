using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CountryMgmt
    {
        public static List<Entity.Country> GetCountryList()
        {
            return (new DAL.CountrySQL().GetCountryList());
        }

        public static List<Entity.Country> GetCountry(string CountryCode, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CountrySQL().GetCountry(CountryCode, LoginUserID ,PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Country> GetCountry(string CountryCode, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CountrySQL().GetCountry(CountryCode, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCountry(Entity.Country entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CountrySQL().AddUpdateCountry(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCountry(string CountryCode, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CountrySQL().DeleteCountry(CountryCode, out ReturnCode, out ReturnMsg);
        }
    }
}
