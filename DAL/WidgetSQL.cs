using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace DAL
{
    public class WidgetSQL : BaseSqlManager
    {
        public void DeleteWidgetRole(string RoleID, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.CommandText = "WidgetRoleByRoleCode_DEL";
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
        public void AddUpdateWidgetRole(string RoleID, string WidgetID, string pLoginUserId, out int ReturnCode, out string ReturnMsg)
        {
            SqlCommand cmdAdd = new SqlCommand();
            cmdAdd.CommandType = CommandType.StoredProcedure;
            cmdAdd.CommandText = "WidgetRole_INS_UPD";
            cmdAdd.Parameters.AddWithValue("@RoleCode", RoleID);
            cmdAdd.Parameters.AddWithValue("@WidgetID", WidgetID);
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
        public virtual string GetWidgetRole(string RoleID)
        {
            string rolerightslist = "";
            List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "WidgetRoleList";
            cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);

            SqlDataReader dr = ExecuteDataReader(cmdGet);
            while (dr.Read())
            {
                if (rolerightslist == "")
                    rolerightslist = GetTextVale(dr, "WidgetID");
                else
                    rolerightslist = rolerightslist + "," + GetTextVale(dr, "WidgetID");

            }
            dr.Close();

            ForceCloseConncetion();
            return rolerightslist;
        }
        public virtual List<Entity.Widget> GetWidgetList()
        {
            //List<Entity.Roles> lstRole = new List<Entity.Roles>();
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "WidgetList";
            //cmdGet.Parameters.AddWithValue("@RoleCode", RoleID);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.Widget> lstObject = new List<Entity.Widget>();
            while (dr.Read())
            {
                Entity.Widget objEntity = new Entity.Widget();
                objEntity.WidgetID = GetTextVale(dr, "WidgetID");
                objEntity.WidgetName = GetTextVale(dr, "WidgetName");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        } 
    }
}
