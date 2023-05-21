using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class UserSQL : BaseSqlManager
    {
        #region Authenticate User
        public virtual Entity.Authenticate AuthenticateUser(string UserID, string UserPwd)
        {

            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "AuthenticateUser";
            cmdGet.Parameters.AddWithValue("@UserID", UserID);
            cmdGet.Parameters.AddWithValue("@Password", UserPwd);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            Entity.Authenticate objAuth = new Entity.Authenticate();
            while (dr.Read())
            {
                objAuth.UserID = GetTextVale(dr, "UserID");
                objAuth.RoleCode = GetTextVale(dr, "RoleCode");
                objAuth.RoleName = GetTextVale(dr, "RoleName");
                objAuth.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objAuth.OrgCode = GetTextVale(dr, "OrgCode");
                objAuth.OrgName = GetTextVale(dr, "OrgName");
                objAuth.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objAuth.OrgType = GetTextVale(dr, "OrgType");
                objAuth.EmployeeID = GetInt64(dr, "EmployeeID");
                objAuth.EmployeeName = GetTextVale(dr, "EmployeeName");
                objAuth.EmployeeImage = GetTextVale(dr, "EmployeeImage");
                objAuth.EmailAddress = GetTextVale(dr, "EmailAddress");
                objAuth.EmailPassword = GetTextVale(dr, "EmailPassword");
                objAuth.ContactNo = GetTextVale(dr, "ContactNo");
                objAuth.CompanyID = GetInt64(dr, "CompanyID");
                objAuth.StateCode = GetInt64(dr, "StateCode");
                objAuth.CompanyName = GetTextVale(dr, "CompanyName");
                objAuth.CompanyType = GetTextVale(dr, "CompanyType");
                objAuth.Landline1 = GetTextVale(dr, "Landline1");
                objAuth.SerialKey = GetTextVale(dr, "SerialKey");
                objAuth.GSTNo = GetTextVale(dr, "GSTNo");
                objAuth.PANNo = GetTextVale(dr, "PANNo");
                objAuth.CINNo = GetTextVale(dr, "CINNo");

                objAuth.SMS_Uri = GetTextVale(dr, "SMS_Uri");
                objAuth.SMS_AuthKey = GetTextVale(dr, "SMS_AuthKey");
                objAuth.SMS_SenderID = GetTextVale(dr, "SMS_SenderID");

                objAuth.CustomerAccess = GetBoolean(dr, "CustomerAccess");
                objAuth.ProductAccess = GetBoolean(dr, "ProductAccess");

                objAuth.NotificationTimestamp = GetDateTime(dr, "NotificationTimestamp");
                objAuth.MailTimestamp = GetDateTime(dr, "MailTimestamp");
                objAuth.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objAuth.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
            }
            dr.Close();
            ForceCloseConncetion();
            return objAuth;
        }

        public virtual Entity.Authenticate AuthenticateUserDealer(string UserID, string UserPwd)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "AuthenticateUserDealer";
            cmdGet.Parameters.AddWithValue("@UserID", UserID);
            cmdGet.Parameters.AddWithValue("@Password", UserPwd);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            Entity.Authenticate objAuth = new Entity.Authenticate();
            while (dr.Read())
            {
                objAuth.UserID = GetTextVale(dr, "UserID");
                objAuth.RoleCode = GetTextVale(dr, "RoleCode");
                objAuth.RoleName = GetTextVale(dr, "RoleName");
                objAuth.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objAuth.OrgCode = GetTextVale(dr, "OrgCode");
                objAuth.OrgName = GetTextVale(dr, "OrgName");
                objAuth.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objAuth.OrgType = GetTextVale(dr, "OrgType");
                objAuth.EmployeeID = GetInt64(dr, "EmployeeID");
                objAuth.EmployeeName = GetTextVale(dr, "EmployeeName");
                //objAuth.EmployeeImage = GetTextVale(dr, "EmployeeImage");
                objAuth.EmailAddress = GetTextVale(dr, "EmailAddress");
                //objAuth.EmailPassword = GetTextVale(dr, "EmailPassword");
                objAuth.CompanyID = GetInt64(dr, "CompanyID");
                objAuth.StateCode = GetInt64(dr, "StateCode");
                objAuth.CompanyName = GetTextVale(dr, "CompanyName");
                objAuth.CompanyType = GetTextVale(dr, "CompanyType");
                objAuth.SerialKey = GetTextVale(dr, "SerialKey");
                objAuth.GSTNo = GetTextVale(dr, "GSTNo");
                objAuth.PANNo = GetTextVale(dr, "PANNo");
                objAuth.CINNo = GetTextVale(dr, "CINNo");

                objAuth.SMS_Uri = GetTextVale(dr, "SMS_Uri");
                objAuth.SMS_AuthKey = GetTextVale(dr, "SMS_AuthKey");
                objAuth.SMS_SenderID = GetTextVale(dr, "SMS_SenderID");

                objAuth.NotificationTimestamp = GetDateTime(dr, "NotificationTimestamp");
                objAuth.MailTimestamp = GetDateTime(dr, "MailTimestamp");
                objAuth.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objAuth.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
            }
            dr.Close();
            ForceCloseConncetion();
            return objAuth;
        }
        #endregion

        public virtual List<Entity.Users> GetLoginUserList(string UserID, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UserList";
            cmdGet.Parameters.AddWithValue("@UserID", UserID);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p); 
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Users> lstUser = new List<Entity.Users>();
            while (dr.Read())
            {
                Entity.Users objUser = new Entity.Users();

                objUser.pkID = GetInt64(dr, "pkID");
                objUser.UserID = GetTextVale(dr, "UserID");
                objUser.UserPassword = GetTextVale(dr, "UserPassword");
                objUser.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objUser.RoleCode = GetTextVale(dr, "RoleCode");
                objUser.RoleName = GetTextVale(dr, "RoleName");
                objUser.OrgCode = GetTextVale(dr, "OrgCode");
                objUser.OrgName = GetTextVale(dr, "OrgName");
                objUser.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objUser.OrgType = GetTextVale(dr, "OrgType");
                objUser.EmployeeID = GetInt64(dr, "EmployeeID");
                objUser.EmployeeName = GetTextVale(dr, "EmployeeName");
                objUser.CompanyID = GetInt64(dr, "CompanyID");
                objUser.CompanyName = GetTextVale(dr, "CompanyName");
                objUser.CompanyType = GetTextVale(dr, "CompanyType");
                objUser.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objUser.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstUser.Add(objUser);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstUser;
        }

        public DataTable GetUserLoginList(string LoginUserID)
        {
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select UserID From MST_Users Where UserID = " + @LoginUserID;
            SqlDataReader dr = ExecuteDataReader(myCommand);
            dt.Load(dr);
            ForceCloseConncetion();
            return dt;
        }

        public virtual List<Entity.Users> GetLoginUserList(string UserID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UserList";
            cmdGet.Parameters.AddWithValue("@UserID", UserID);
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Users> lstUser = new List<Entity.Users>();
            while (dr.Read())
            {
                Entity.Users objUser = new Entity.Users();

                objUser.pkID = GetInt64(dr, "pkID");
                objUser.UserID = GetTextVale(dr, "UserID");
                objUser.UserPassword = GetTextVale(dr, "UserPassword");
                objUser.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objUser.RoleCode = GetTextVale(dr, "RoleCode");
                objUser.RoleName = GetTextVale(dr, "RoleName");
                objUser.OrgCode = GetTextVale(dr, "OrgCode");
                objUser.OrgName = GetTextVale(dr, "OrgName");
                objUser.OrgTypeCode = GetInt64(dr, "OrgTypeCode");
                objUser.OrgType = GetTextVale(dr, "OrgType");
                objUser.EmployeeID = GetInt64(dr, "EmployeeID");
                objUser.EmployeeName = GetTextVale(dr, "EmployeeName");
                objUser.CompanyID = GetInt64(dr, "CompanyID");
                objUser.CompanyName = GetTextVale(dr, "CompanyName");
                objUser.CompanyType = GetTextVale(dr, "CompanyType");
                objUser.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                objUser.ActiveFlagDesc = GetTextVale(dr, "ActiveFlagDesc");
                lstUser.Add(objUser);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstUser;
        }

        public virtual List<Entity.Users> GetUserLogList(string pLoginUserID, Int64 pMonth, Int64 pYear, string FromDate=null, string ToDate=null)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UserLogListByUser";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Users> lstUser = new List<Entity.Users>();
            while (dr.Read())
            {
                Entity.Users objUser = new Entity.Users();
                objUser.pkID = GetInt64(dr, "pkID");
                objUser.UserID = GetTextVale(dr, "UserID");
                objUser.LoginDateTime = GetTextVale(dr, "LoginDateTime");
                objUser.LogoutDateTime = GetTextVale(dr, "LogoutDateTime");
                objUser.MacID = GetTextVale(dr, "MacID");
                lstUser.Add(objUser);
            }
            dr.Close();
            
            ForceCloseConncetion();
            return lstUser;
        }

        #region Add Update Login User
        public virtual void AddUpdateUserManagement(Entity.Users entity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Users_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", entity.pkID);
            cmdAdd.Parameters.AddWithValue("@ScreenFullName", entity.ScreenFullName);
            cmdAdd.Parameters.AddWithValue("@UserID", entity.UserID);
            cmdAdd.Parameters.AddWithValue("@UserPassword", entity.UserPassword);
            cmdAdd.Parameters.AddWithValue("@ActiveFlag", entity.ActiveFlag);
            cmdAdd.Parameters.AddWithValue("@Description", entity.Description);
            cmdAdd.Parameters.AddWithValue("@RoleCode", entity.RoleCode);
            cmdAdd.Parameters.AddWithValue("@CompanyID", entity.CompanyID);
            cmdAdd.Parameters.AddWithValue("@OrgCode", entity.OrgCode);
            cmdAdd.Parameters.AddWithValue("@EmployeeID", entity.EmployeeID);
            cmdAdd.Parameters.AddWithValue("@CustomerID", entity.CustomerID);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", entity.LoginUserID);
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
        #endregion

        #region Delete Login User
        public virtual void DeleteLoginUser(string UserID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Users_DEL";
            cmdDel.Parameters.AddWithValue("@UserID", UserID);
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
        #endregion

        public virtual List<Entity.UserLog> GetUserActivityList(string pLoginUserID, Int64 pDay, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UserActivityList";
            cmdGet.Parameters.AddWithValue("@LoginUserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Day", pDay);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.UserLog> lstUser = new List<Entity.UserLog>();
            while (dr.Read())
            {
                Entity.UserLog objUser = new Entity.UserLog();

                objUser.UserID = GetTextVale(dr, "UserID");
                objUser.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objUser.EmployeeID = GetInt64(dr, "EmployeeID");
                objUser.EmployeeName = GetTextVale(dr, "EmployeeName");
                objUser.Designation = GetTextVale(dr, "Designation");
                objUser.CompanyID = GetInt64(dr, "CompanyID");
                objUser.CompanyName = GetTextVale(dr, "CompanyName");
                objUser.CompanyType = GetTextVale(dr, "CompanyType");
                objUser.LoginDateTime = GetDateTime(dr, "LoginTime");
                objUser.LogoutDateTime = GetDateTime(dr, "LogoutTime");
                objUser.Contacts = GetInt64(dr, "Contacts");
                objUser.ToDO = GetInt64(dr, "ToDO");
                objUser.Leave = GetInt64(dr, "Leave");
                objUser.login_logout = GetInt64(dr, "login_logout");
                objUser.Inquiry = GetInt64(dr, "Inquiry");
                objUser.Quotation = GetInt64(dr, "Quotation");
                objUser.Followup = GetInt64(dr, "Followup");
                objUser.SalesOrder = GetInt64(dr, "SalesOrder");
                try
                {
                    objUser.LatePunch = GetInt64(dr, "LatePunch");
                    objUser.DailyActivity = GetInt64(dr, "DailyActivity");
                }
                catch (Exception ex)
                {
                    //ex.StackTrace
                }
                lstUser.Add(objUser);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstUser;
        }

        public virtual List<Entity.UserLog> GetUserActivityListByUser(string pLoginUserID, Int64 pDay, Int64 pMonth, Int64 pYear, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UserActivityListByUser";
            cmdGet.Parameters.AddWithValue("@UserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@Day", pDay);
            cmdGet.Parameters.AddWithValue("@Month", pMonth);
            cmdGet.Parameters.AddWithValue("@Year", pYear);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.UserLog> lstUser = new List<Entity.UserLog>();
            while (dr.Read())
            {
                Entity.UserLog objUser = new Entity.UserLog();

                objUser.UserID = GetTextVale(dr, "UserID");
                objUser.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objUser.EmployeeID = GetInt64(dr, "EmployeeID");
                objUser.EmployeeName = GetTextVale(dr, "EmployeeName");
                objUser.Designation = GetTextVale(dr, "Designation");
                objUser.CompanyID = GetInt64(dr, "CompanyID");
                objUser.CompanyName = GetTextVale(dr, "CompanyName");
                objUser.CompanyType = GetTextVale(dr, "CompanyType");
                objUser.LoginDateTime = GetDateTime(dr, "LoginTime");
                objUser.LogoutDateTime = GetDateTime(dr, "LogoutTime");
                objUser.Contacts = GetInt64(dr, "Contacts");
                objUser.ToDO = GetInt64(dr, "ToDO");
                objUser.Leave = GetInt64(dr, "Leave");
                objUser.login_logout = GetInt64(dr, "login_logout");
                objUser.Inquiry = GetInt64(dr, "Inquiry");
                objUser.Quotation = GetInt64(dr, "Quotation");
                objUser.Followup = GetInt64(dr, "Followup");
                objUser.SalesOrder = GetInt64(dr, "SalesOrder");
                objUser.SalesInvoice = GetInt64(dr, "SalesInvoice");
                objUser.PurchaseInvoice = GetInt64(dr, "PurchaseInvoice");
                objUser.Inward = GetInt64(dr, "Inward");
                objUser.Outward = GetInt64(dr, "Outward");

                try
                {
                    objUser.LatePunch = GetInt64(dr, "LatePunch");
                    objUser.DailyActivity = GetInt64(dr, "DailyActivity");
                }
                catch (Exception ex)
                {
                    //ex.StackTrace
                }
                lstUser.Add(objUser);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstUser;
        }

        public virtual List<Entity.UserLog> GetUserActivityListByUser(string pLoginUserID, DateTime FromDate, DateTime ToDate, int PageNo, int PageSize, out int TotalRecord)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "UserActivityListByUser";
            cmdGet.Parameters.AddWithValue("@UserID", pLoginUserID);
            cmdGet.Parameters.AddWithValue("@FromDate", FromDate);
            cmdGet.Parameters.AddWithValue("@ToDate", ToDate);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.UserLog> lstUser = new List<Entity.UserLog>();
            while (dr.Read())
            {
                Entity.UserLog objUser = new Entity.UserLog();

                objUser.UserID = GetTextVale(dr, "UserID");
                objUser.ScreenFullName = GetTextVale(dr, "ScreenFullName");
                objUser.EmployeeID = GetInt64(dr, "EmployeeID");
                objUser.EmployeeName = GetTextVale(dr, "EmployeeName");
                objUser.Designation = GetTextVale(dr, "Designation");
                objUser.CompanyID = GetInt64(dr, "CompanyID");
                objUser.CompanyName = GetTextVale(dr, "CompanyName");
                objUser.CompanyType = GetTextVale(dr, "CompanyType");
                objUser.LoginDateTime = GetDateTime(dr, "LoginTime");
                objUser.LogoutDateTime = GetDateTime(dr, "LogoutTime");
                objUser.Contacts = GetInt64(dr, "Contacts");
                objUser.ToDO = GetInt64(dr, "ToDO");
                objUser.Leave = GetInt64(dr, "Leave");
                objUser.login_logout = GetInt64(dr, "login_logout");
                objUser.Inquiry = GetInt64(dr, "Inquiry");
                objUser.Quotation = GetInt64(dr, "Quotation");
                objUser.Followup = GetInt64(dr, "Followup");
                objUser.SalesOrder = GetInt64(dr, "SalesOrder");
                objUser.SalesInvoice = GetInt64(dr, "SalesInvoice");
                objUser.PurchaseInvoice = GetInt64(dr, "PurchaseInvoice");
                objUser.Inward = GetInt64(dr, "Inward");
                objUser.Outward = GetInt64(dr, "Outward");

                try
                {
                    objUser.LatePunch = GetInt64(dr, "LatePunch");
                    objUser.DailyActivity = GetInt64(dr, "DailyActivity");
                }
                catch (Exception ex)
                {
                    //ex.StackTrace
                }
                lstUser.Add(objUser);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstUser;
        }
        public virtual void AddUpdateUserManagementRegistration(Entity.Users entity,string serialkey,out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Users_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@pkID", entity.pkID);
            cmdAdd.Parameters.AddWithValue("@UserID", entity.UserID);
            cmdAdd.Parameters.AddWithValue("@UserPassword", entity.UserPassword);           
            //cmdAdd.Parameters.AddWithValue("@CompanyID", entity.CompanyID);
            cmdAdd.Parameters.AddWithValue("@SerialKey", serialkey);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", entity.LoginUserID);
            SqlParameter p = new SqlParameter("@ReturnCode", SqlDbType.Int);
            SqlParameter p1 = new SqlParameter("@ReturnMsg", SqlDbType.NVarChar, 255);
            p.Direction = ParameterDirection.Output;
            p1.Direction = ParameterDirection.Output;
            cmdAdd.Parameters.Add(p);
            cmdAdd.Parameters.Add(p1);
            //ExecuteNonQuery(cmdAdd);

            OpenConncetionRegistration(cmdAdd);
            cmdAdd.ExecuteNonQuery();

            ReturnCode = Convert.ToInt32(cmdAdd.Parameters["@ReturnCode"].Value.ToString());
            ReturnMsg = cmdAdd.Parameters["@ReturnMsg"].Value.ToString();
            ForceCloseConncetion();
        }

        public virtual void UpdateUserPassword(string pLoginUserID, string pPassword, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "UserPassword_UPD";
            cmdAdd.Parameters.AddWithValue("@UserID", pLoginUserID);
            cmdAdd.Parameters.AddWithValue("@UserPassword", pPassword);
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
    }
}
