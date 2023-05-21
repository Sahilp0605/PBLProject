using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CategoryDescriptionMgmt
    {
        public static List<Entity.CategoryDescription> GetCategoryDescriptionList(string statuscategory)
        {
            return (new DAL.CategoryDescriptionSQL().GetCategoryDescriptionList(statuscategory));
        }

        public static List<Entity.CategoryDescription> GetCategoryDescriptionList(Int64 pkID, string statuscategory, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CategoryDescriptionSQL().GetCategoryDescriptionList(pkID, statuscategory, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.CategoryDescription> GetCategoryDescriptionList(Int64 pkID, string statuscategory, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CategoryDescriptionSQL().GetCategoryDescriptionList(pkID, statuscategory, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateCategoryDescription(Entity.CategoryDescription entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CategoryDescriptionSQL().AddUpdateCategoryDescription(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteCategoryDescription(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CategoryDescriptionSQL().DeleteCategoryDescription(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
