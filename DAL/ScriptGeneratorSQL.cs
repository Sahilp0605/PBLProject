using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace DAL
{
    public class ScriptGeneratorSQL:BaseSqlManager
    {
        public static string GetTableColumnsList(string pTableName)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select dbo.MY_ColumnsList('" + pTableName + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public static string GetTableColumnsList(string pTableName, string pCategory)
        {
            SqlCommand myCommand = new SqlCommand();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = "Select dbo.MY_ColumnsList('" + pTableName + "','" + pCategory + "')";
            string varResult = ExecuteScalar(myCommand).ToString();
            ForceCloseConncetion();
            return varResult;
        }
        public virtual List<Entity.ScriptGenerator> GetTableStructure(String pTableName)
        {
            SqlCommand cmdGet = new SqlCommand();
            cmdGet.CommandType = CommandType.StoredProcedure;
            cmdGet.CommandText = "MY_StructureList";
            cmdGet.Parameters.AddWithValue("@TableName", pTableName);
            SqlDataReader dr = ExecuteDataReader(cmdGet);
            List<Entity.ScriptGenerator> lstObject = new List<Entity.ScriptGenerator>();
            while (dr.Read())
            {
                Entity.ScriptGenerator objEntity = new Entity.ScriptGenerator();
                objEntity.pkID = GetInt64(dr, "pkID");
                objEntity.ColName = GetTextVale(dr, "ColName");
                objEntity.ColType = GetTextVale(dr, "ColType");
                objEntity.ColIsNull = GetTextVale(dr, "ColIsNull");
                objEntity.ColWidth = GetTextVale(dr, "ColWidth");
                objEntity.ColScale = GetTextVale(dr, "ColScale");
                lstObject.Add(objEntity);
            }
            dr.Close();
            ForceCloseConncetion();
            return lstObject;
        }
    }
}
