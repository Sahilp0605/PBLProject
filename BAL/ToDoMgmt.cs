using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class ToDoMgmt
    {
        public static List<Entity.ToDo> GetToDoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ToDoSQL().GetToDoList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ToDo> GetToDoList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ToDoSQL().GetToDoList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ToDo> GetDashboardToDoList(String TaskStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ToDoSQL().GetDashboardToDoList(TaskStatus, pMonth, pYear, LoginUserID, PageNo, PageSize, out TotalRecord));
        }
        public static List<Entity.ToDo> GetDashboardCustomerToDoList(String TaskStatus, Int64 pCustomerID, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ToDoSQL().GetDashboardCustomerToDoList(TaskStatus, pCustomerID, pMonth, pYear, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ToDo> GetToDoListByUser(String LoginUserID, Int64 pMonth, Int64 pYear)
        {
            return (new DAL.ToDoSQL().GetToDoListByUser(LoginUserID, pMonth, pYear));
        }

        public static List<Entity.ToDo> GetToDoListByUserPeriod(String LoginUserID, string FromDate, string ToDate)
        {
            return (new DAL.ToDoSQL().GetToDoListByUser(LoginUserID, 0, 0, FromDate, ToDate));
        }

        public static void AddUpdateToDo(Entity.ToDo entity, out int ReturnCode, out string ReturnMsg, out int ReturnHeaderID)
        {
            new DAL.ToDoSQL().AddUpdateToDo(entity, out ReturnCode, out ReturnMsg, out ReturnHeaderID);
        }

        public static void DeleteToDo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ToDoSQL().DeleteToDo(pkID, out ReturnCode, out ReturnMsg);
        }
       

        /* ---------------------------------------------------------------------------------------------------- */
        /* TODO - LOG LIST     */
        /* ---------------------------------------------------------------------------------------------------- */
        public static List<Entity.ToDo> GetToDoLogList(Int64 HeaderID)
        {
            return (new DAL.ToDoSQL().GetToDoLogList(HeaderID));
        }
        public static void AddUpdateToDoLog(Entity.ToDo entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ToDoSQL().AddUpdateToDoLog(entity, out ReturnCode, out ReturnMsg);
        }
    }

}
