using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ToDoSQL:BaseSqlManager
    {
        public virtual List<Entity.ToDo> GetToDoList(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ToDoList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int); 
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ToDo> lstObject = new List<Entity.ToDo>();
            while (dr.Read())
            {
                Entity.ToDo objEntity = new Entity.ToDo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskDescriptionShort = GetTextVale(dr, "TaskDescriptionShort");
                objEntity.Location = GetTextVale(dr, "Location");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.DueDate = GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = GetDateTime(dr, "CompletionDate");
                objEntity.Duration = GetTextVale(dr, "Duration");
                objEntity.TaskStatus = GetTextVale(dr, "TaskStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromEmployeeID = GetInt64(dr, "FromEmployeeID");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.Reminder = GetBoolean(dr, "Reminder");
                objEntity.ReminderMonth = GetInt64(dr, "ReminderMonth");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                //objEntity.ProjectID = GetInt64(dr, "ProjectID");
                //objEntity.ProjectID = GetInt64(dr, "ProjectID");
                //objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                //objEntity.OrgCode = GetInt64(dr, "OrgCode");
                //objEntity.OrgName = GetTextVale(dr, "OrgName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.ToDo> GetToDoList(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ToDoList";
            cmdGet.Parameters.AddWithValue("@pkID", pkID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ToDo> lstObject = new List<Entity.ToDo>();
            while (dr.Read())
            {
                Entity.ToDo objEntity = new Entity.ToDo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskDescriptionShort = GetTextVale(dr, "TaskDescriptionShort");
                objEntity.Location = GetTextVale(dr, "Location");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.DueDate = GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = GetDateTime(dr, "CompletionDate");
                objEntity.Duration = GetTextVale(dr, "Duration");
                objEntity.TaskStatus = GetTextVale(dr, "TaskStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromEmployeeID = GetInt64(dr, "FromEmployeeID");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskCategory = GetTextVale(dr, "TaskCategory");
                objEntity.Reminder = GetBoolean(dr, "Reminder");
                objEntity.ReminderMonth = GetInt64(dr, "ReminderMonth");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                //objEntity.ProjectID = GetInt64(dr, "ProjectID");
                //objEntity.ProjectName = GetTextVale(dr, "ProjectName");
                //objEntity.OrgCode = GetInt64(dr, "OrgCode");
                //objEntity.OrgName = GetTextVale(dr, "OrgName");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.ToDo> GetDashboardToDoList(String TaskStatus, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardToDoList";
            cmdGet.Parameters.AddWithValue("@TaskStatus", TaskStatus);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ToDo> lstObject = new List<Entity.ToDo>();
            while (dr.Read())
            {
                Entity.ToDo objEntity = new Entity.ToDo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskDescriptionShort = GetTextVale(dr, "TaskDescriptionShort");
                objEntity.Location = GetTextVale(dr, "Location");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskCategory = GetTextVale(dr, "TaskCategory");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.DueDate = GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = GetDateTime(dr, "CompletionDate");
                objEntity.Duration = GetTextVale(dr, "Duration");
                objEntity.TaskStatus = GetTextVale(dr, "TaskStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromEmployeeID = GetInt64(dr, "FromEmployeeID");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.SubTaskCompleted = GetInt64(dr, "SubTaskCompleted");
                objEntity.TotalSubTask = GetInt64(dr, "TotalSubTask");
                objEntity.Reminder = GetBoolean(dr, "Reminder");
                objEntity.ReminderMonth = GetInt64(dr, "ReminderMonth");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

        public virtual List<Entity.ToDo> GetToDoListByUser(String LoginUserID, Int64 pMonth, Int64 pYear, string pFromDate = null, string pToDate = null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ToDoListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", pFromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", pToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ToDo> lstObject = new List<Entity.ToDo>();
            while (dr.Read())
            {
                Entity.ToDo objEntity = new Entity.ToDo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskDescriptionShort = GetTextVale(dr, "TaskDescriptionShort");
                objEntity.Location = GetTextVale(dr, "Location");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskCategory = GetTextVale(dr, "TaskCategory");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.DueDate = GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = GetDateTime(dr, "CompletionDate");
                objEntity.Duration = GetTextVale(dr, "Duration");
                objEntity.TaskStatus = GetTextVale(dr, "TaskStatus");
                objEntity.CustomerID = GetInt64(dr, "CustomerID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromEmployeeID = GetInt64(dr, "FromEmployeeID");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.Reminder = GetBoolean(dr, "Reminder");
                objEntity.ReminderMonth = GetInt64(dr, "ReminderMonth");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        // ============================= Insert & Update
        public virtual void AddUpdateToDo(Entity.ToDo objEntity, out int ReturnCode, out string ReturnMsg, out int ReturnHeaderID)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ToDo_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@TaskDescription", objEntity.TaskDescription);
            cmdAdd.Parameters.AddWithValue("@Location", objEntity.Location);
            cmdAdd.Parameters.AddWithValue("@TaskCategoryID", objEntity.TaskCategoryID);
            cmdAdd.Parameters.AddWithValue("@Priority", objEntity.Priority);
            cmdAdd.Parameters.AddWithValue("@StartDate", objEntity.StartDate);
            cmdAdd.Parameters.AddWithValue("@DueDate", objEntity.DueDate);
            if (objEntity.CompletionDate.Year >= 2020)
                cmdAdd.Parameters.AddWithValue("@CompletionDate", objEntity.CompletionDate);
            else
                cmdAdd.Parameters.AddWithValue("@CompletionDate", DBNull.Value);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@Reminder", objEntity.Reminder);
            cmdAdd.Parameters.AddWithValue("@ReminderMonth", objEntity.ReminderMonth);
            cmdAdd.Parameters.AddWithValue("@ClosingRemarks", objEntity.ClosingRemarks);
            cmdAdd.Parameters.AddWithValue("@CustomerID", objEntity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            SqlParameter p2 = new SqlParameter("@ReturnHeaderID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            p2.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            cmdAdd.Parameters.Add(p2);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ReturnHeaderID = Convert.ToInt32(cmdAdd.Parameters["@ReturnHeaderID"].Value.ToString());
            ForceCloseConncetion();
        }

        public virtual void DeleteToDo(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "TODO_DEL";
            cmdDel.Parameters.AddWithValue("@pkID", pkID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdDel.Parameters.Add(p);
            cmdDel.Parameters.Add(p1);
            ExecuteNonQuery(cmdDel);
            ReturnCode = Convert.ToInt32(cmdDel.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdDel.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

       
        public virtual List<Entity.ToDo> GetToDoLogList(Int64 HeaderID)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "ToDoLogList";
            cmdGet.Parameters.AddWithValue("@HeaderID", HeaderID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", "");
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ToDo> lstObject = new List<Entity.ToDo>();
            while (dr.Read())
            {
                Entity.ToDo objEntity = new Entity.ToDo();
                objEntity.RowNum = GetInt64(dr, "RowNum");
                objEntity.pkID = GetInt64(dr, "HeaderID");
                objEntity.ActionTaken = GetTextVale(dr, "ActionTaken");
                objEntity.TaskDescription = GetTextVale(dr, "ActionDescription");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.ClosingRemarks = GetTextVale(dr, "Remarks");
                objEntity.CreatedBy = GetTextVale(dr, "CreatedBy");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.CreatedDate = GetDateTime(dr, "CreatedDate");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
        public virtual void AddUpdateToDoLog(Entity.ToDo objEntity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "ToDoLog_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@HeaderID", objEntity.pkID);
            cmdAdd.Parameters.AddWithValue("@ActionTaken", objEntity.ActionTaken);
            cmdAdd.Parameters.AddWithValue("@ActionDescription", objEntity.TaskDescription);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", objEntity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@Remarks", objEntity.ClosingRemarks);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", objEntity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            ExecuteNonQuery(cmdAdd);
            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual List<Entity.ToDo> GetDashboardCustomerToDoList(String TaskStatus, Int64 CustomerID, Int64 pMonth, Int64 pYear, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "GetDashboardCustomerToDoList";
            cmdGet.Parameters.AddWithValue("@TaskStatus", TaskStatus);
            cmdGet.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ToDo> lstObject = new List<Entity.ToDo>();
            while (dr.Read())
            {
                Entity.ToDo objEntity = new Entity.ToDo();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.CustomerName = GetTextVale(dr, "CustomerName");
                objEntity.Priority = GetTextVale(dr, "Priority");
                objEntity.TaskDescription = GetTextVale(dr, "TaskDescription");
                objEntity.TaskDescriptionShort = GetTextVale(dr, "TaskDescriptionShort");
                objEntity.Location = GetTextVale(dr, "Location");
                objEntity.TaskCategoryID = GetInt64(dr, "TaskCategoryID");
                objEntity.TaskCategory = GetTextVale(dr, "TaskCategory");
                objEntity.StartDate = GetDateTime(dr, "StartDate");
                objEntity.DueDate = GetDateTime(dr, "DueDate");
                objEntity.CompletionDate = GetDateTime(dr, "CompletionDate");
                objEntity.Duration = GetTextVale(dr, "Duration");
                objEntity.TaskStatus = GetTextVale(dr, "TaskStatus");
                objEntity.EmployeeID = GetInt64(dr, "EmployeeID");
                objEntity.EmployeeName = GetTextVale(dr, "EmployeeName");
                objEntity.FromEmployeeID = GetInt64(dr, "FromEmployeeID");
                objEntity.FromEmployeeName = GetTextVale(dr, "FromEmployeeName");
                objEntity.SubTaskCompleted = GetInt64(dr, "SubTaskCompleted");
                objEntity.TotalSubTask = GetInt64(dr, "TotalSubTask");
                objEntity.Reminder = GetBoolean(dr, "Reminder");
                objEntity.ReminderMonth = GetInt64(dr, "ReminderMonth");
                objEntity.ClosingRemarks = GetTextVale(dr, "ClosingRemarks");
                lstObject.Add(objEntity);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstObject;
        }

    }
}
