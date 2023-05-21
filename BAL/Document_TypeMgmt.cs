using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class Document_TypeMgmt
    {
        public static List<Entity.Document_Type> GetDocument_TypeList(Int64 pkID, String LoginUserID)
        {
            return (new DAL.Document_TypeSQL().GetDocument_TypeList(pkID, LoginUserID));
        }

        public static List<Entity.Document_Type> GetDocument_Type(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Document_TypeSQL().GetDocument_Type(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Document_Type> GetDocument_Type(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.Document_TypeSQL().GetDocument_Type(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateDocument_Type(Entity.Document_Type entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Document_TypeSQL().AddUpdateDocument_Type(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteDocument_Type(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.Document_TypeSQL().DeleteDocument_Type(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}