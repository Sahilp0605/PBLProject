using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ModuleDocMgmt
    {

        public static List<Entity.ModuleDocuments> GetModuleDocumentList(Int64 pkID, string SearchKey, string ModuleName, string DocName, string KeyValue, string LoginUserID)
        {
            return (new DAL.ModuleDocSQL().GetModuleDocumentList(pkID, SearchKey, ModuleName, DocName, KeyValue, LoginUserID));
        }

        public static void AddUpdateDocument(Entity.ModuleDocuments entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ModuleDocSQL().AddUpdateDocument(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteDocument(Int64 pkID, string ModuleName, string KeyValue, string DocName, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.ModuleDocSQL().DeleteDocument(pkID, ModuleName, KeyValue, DocName, out ReturnCode, out ReturnMsg);
        }
    }
}
