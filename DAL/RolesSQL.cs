using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class RolesSQL : BaseSqlManager
    {
        #region Get Role List For Dropdown
        public virtual List<Entity.Roles> GetRoleList()
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RolesList";
            cmdGet.Parameters.AddWithValue("@RoleCode", "");
            cmdGet.Parameters.AddWithValue("@ListMode", "L");
            cmdGet.Parameters.AddWithValue("@PageNo", 1);
            cmdGet.Parameters.AddWithValue("@PageSize", 1000);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            while (dr.Read())
            {
                Entity.Roles objRole = new Entity.Roles();
                objRole.RoleCode = GetTextVale(dr, "RoleCode");
                objRole.Description = GetTextVale(dr, "Description");
                objRole.Comments = GetTextVale(dr, "Comments");
                objRole.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                lstRole.Add(objRole);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstRole;
        } 
        #endregion

        #region Add Update Role Detail
        public virtual void AddUpdateRoleDetail(Entity.Roles entity, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "Roles_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@RoleCode", entity.RoleCode);
            cmdAdd.Parameters.AddWithValue("@Description", entity.Description);
            cmdAdd.Parameters.AddWithValue("@Comments", entity.Comments);
           cmdAdd.Parameters.AddWithValue("@ActiveFlag", entity.ActiveFlag);
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

        #region Get Role List
        public virtual List<Entity.Roles> GetRole(string RoleID, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RolesList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Roles objRole = new Entity.Roles();
                objRole.RoleCode = GetTextVale(dr, "RoleCode");
                objRole.Description = GetTextVale(dr, "Description");
                objRole.Comments = GetTextVale(dr, "Comments");
                objRole.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                lstRole.Add(objRole);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstRole;
        } 
        #endregion

        #region Get Role List
        public virtual List<Entity.Roles> GetRole(string RoleID, string LoginUserID, string SearchKey, int PageNo, int PageSize, out int TotalRecord)
        {
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RolesList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdGet.Parameters.AddWithValue("@ListMode", "");
            cmdGet.Parameters.AddWithValue("@LoginUserID", LoginUserID);
            cmdGet.Parameters.AddWithValue("@SearchKey", SearchKey);
            cmdGet.Parameters.AddWithValue("@PageNo", PageNo);
            cmdGet.Parameters.AddWithValue("@PageSize", PageSize);
            SqlParameter p = new SqlParameter("@TotalCount", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            cmdGet.Parameters.Add(p);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                Entity.Roles objRole = new Entity.Roles();
                objRole.RoleCode = GetTextVale(dr, "RoleCode");
                objRole.Description = GetTextVale(dr, "Description");
                objRole.Comments = GetTextVale(dr, "Comments");
                objRole.ActiveFlag = GetBoolean(dr, "ActiveFlag");
                lstRole.Add(objRole);
            }
            dr.Close();
            TotalRecord = Convert.ToInt32(cmdGet.Parameters["@TotalCount"].Value.ToString());
            ForceCloseConncetion();
            return lstRole;
        }
        #endregion

        #region Delete Role Detail
        public virtual void DeleteRoleDetail(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "Role_DEL";
            cmdDel.Parameters.AddWithValue("@RoleCode", RoleID);
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

        //--------- Menu : Role Rights-------------
        public void DeleteRoleRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "RoleRightsByRoleCode_DEL";
            cmdDel.Parameters.AddWithValue("@RoleCode", RoleID);
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
        public void AddUpdateRoleRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "RoleRights_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdAdd.Parameters.AddWithValue("@MenuId", MenuId);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", pLoginUserId);
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
        public virtual string GetRoleRights(string RoleID)
        {
            string rolerightslist = "";
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RoleRightsList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);
           
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                if(rolerightslist=="")
                    rolerightslist = GetTextVale(dr, "MenuId");
                else
                    rolerightslist = rolerightslist + "," + GetTextVale(dr, "MenuId");
                
            }
            dr.Close();
            
            ForceCloseConncetion();
            return rolerightslist;
        }

        //--------- Reports : Role Reports Rights-------------
        public void DeleteRoleReportRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "RoleReportRightsByRoleCode_DEL";
            cmdDel.Parameters.AddWithValue("@RoleCode", RoleID);
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
        public void AddUpdateRoleReportRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "RoleReportRights_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdAdd.Parameters.AddWithValue("@MenuId", MenuId);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", pLoginUserId);
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
        public virtual string GetRoleReportRights(string RoleID)
        {
            string rolerightslist = "";
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RoleReportRightsList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                if (rolerightslist == "")
                    rolerightslist = GetTextVale(dr, "MenuId");
                else
                    rolerightslist = rolerightslist + "," + GetTextVale(dr, "MenuId");

            }
            dr.Close();

            ForceCloseConncetion();
            return rolerightslist;
        }

        //--------- Reports : Role ICON Rights-------------
        public void DeleteRoleIconRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "RoleIconRightsByRoleCode_DEL";
            cmdDel.Parameters.AddWithValue("@RoleCode", RoleID);
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
        public void AddUpdateRoleIconRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "RoleIconRights_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdAdd.Parameters.AddWithValue("@MenuId", MenuId);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", pLoginUserId);
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
        public virtual string GetRoleIconRights(string RoleID)
        {
            string rolerightslist = "";
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RoleIconRightsList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                if (rolerightslist == "")
                    rolerightslist = GetTextVale(dr, "MenuId");
                else
                    rolerightslist = rolerightslist + "," + GetTextVale(dr, "MenuId");

            }
            dr.Close();

            ForceCloseConncetion();
            return rolerightslist;
        }
        public void DeleteRoleGeneralMasterRights(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "RoleGeneralMasterRightsByRoleCode_DEL";
            cmdDel.Parameters.AddWithValue("@RoleCode", RoleID);
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
        public void AddUpdateGetRoleGeneralMasterRights(string RoleID, string MenuId, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "RoleGeneralMasterRights_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdAdd.Parameters.AddWithValue("@MenuId", MenuId);
            cmdAdd.Parameters.AddWithValue("@LoginUserID", pLoginUserId);
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
        public virtual string GetRoleGeneralMasterRights(string RoleID)
        {
            string rolerightslist = "";
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "RoleGeneralMasterRightsList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                if (rolerightslist == "")
                    rolerightslist = GetTextVale(dr, "MenuId");
                else
                    rolerightslist = rolerightslist + "," + GetTextVale(dr, "MenuId");

            }
            dr.Close();

            ForceCloseConncetion();
            return rolerightslist;
        }
    }
}
