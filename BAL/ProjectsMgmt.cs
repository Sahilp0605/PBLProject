using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class ProjectsMgmt
    {
        public static List<Entity.Projects> GetProjectsList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProjectsSQL().GetProjectsList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.Projects> GetProjectsList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProjectsSQL().GetProjectsList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateProjects(Entity.Projects entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectsSQL().AddUpdateProjects(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteProjects(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectsSQL().DeleteProjects(pkID, out ReturnCode, out ReturnMsg);
        }
    }
    public class ProjectSheetMgmt
    {
        public static List<Entity.ProjectSheet> GetProjectSheetList(string LoginUserID, out int TotalRecord)
        {
            return (new DAL.ProjectSheetSQL().GetProjectSheetList(LoginUserID, out TotalRecord));
        }

        public static List<Entity.ProjectSheet> GetProjectSheetList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProjectSheetSQL().GetProjectSheetList(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.ProjectSheet> GetProjectSheetList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.ProjectSheetSQL().GetProjectSheetList(pkID, LoginUserID, SearchKey, PageNo, PageSize, out TotalRecord));
        }

        public static void AddUpdateProjectSheet(Entity.ProjectSheet entity, out int ReturnCode, out string ReturnMsg, out String ReturnProjectNo)
        {
            new DAL.ProjectSheetSQL().AddUpdateProjectSheet(entity, out ReturnCode, out ReturnMsg, out ReturnProjectNo);
        }

        public static void DeleteProjectSheet(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectSheetSQL().DeleteProjectSheet(pkID, out ReturnCode, out ReturnMsg);
        }
        //--------------------------------------------------------------------------------------------------
        public static void AddUpdateProject_Detail(Entity.Project_Detail entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectSheetSQL().AddUpdateProject_Detail(entity, out ReturnCode, out ReturnMsg);
        }

        public static DataTable GetProjectProductDetail(string pProjectNo)
        {
            return (new DAL.ProjectSheetSQL().GetProjectProductDetail(pProjectNo));
        }

        public static DataTable GetProjectAssemblyDetail(string pProjectNo,Int64 FinishedProID)
        {
            return (new DAL.ProjectSheetSQL().GetProjectAssemblyDetail(pProjectNo,FinishedProID));
        }
        public static void DeleteProjectDetailsBySheetNo(string pProjectSheetNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectSheetSQL().DeleteProjectDetailsBySheetNo(pProjectSheetNo, out ReturnCode, out ReturnMsg);
        }
        public static void DeleteAssemblyByProjectNo(string pProjectSheetNo, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectSheetSQL().DeleteAssemblyByProjectNo(pProjectSheetNo, out ReturnCode, out ReturnMsg);
        }
        public static void AddUpdateProject_Assembly(Entity.ProjectAssembly entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ProjectSheetSQL().AddUpdateProject_Assembly(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
