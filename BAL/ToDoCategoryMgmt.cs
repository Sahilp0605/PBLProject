using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ToDoCategoryMgmt
    {
        public static List<Entity.ToDoCategory> GetTaskCategoryList(String pCategory)
        {
            return (new DAL.ToDoCategorySQL().GetTaskCategoryList(pCategory));
        }
        public static List<Entity.ToDoCategory> GetTODOCategoryList(Int64 pkID, String LoginUserID,String SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ToDoCategorySQL().GetTODOCategoryList(pkID,LoginUserID,SearchKey,PageNo,PageSize, out TotalRecord));
        }
        public static void AddUpdateTODOCategory(Entity.ToDoCategory entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ToDoCategorySQL().AddUpdateTODOCategory(entity, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteTODOCategory(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ToDoCategorySQL().DeleteTODOCategory(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
