using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ScriptGeneratorMgmt
    {
        public static string GetTableColumnsList(string pTableName)
        {
            return DAL.ScriptGeneratorSQL.GetTableColumnsList(pTableName);
        }
        public static string GetTableColumnsList(string pTableName, string pCategory)
        {
            return DAL.ScriptGeneratorSQL.GetTableColumnsList(pTableName, pCategory);
        }

        public static List<Entity.ScriptGenerator> GetTableStructure(String pTableName)
        {
            return (new DAL.ScriptGeneratorSQL().GetTableStructure(pTableName));
        }
    }
}
