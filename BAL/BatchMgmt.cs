using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class BatchMgmt
    {
        public static List<Entity.Batch> GetBatchList(String LoginUserID)
        {
            return (new DAL.BatchSQL().GetBatchList(LoginUserID));
        }

        public static List<Entity.Batch> GetBatch(Int64 pkID, string LoginUserID, int PageNo, int PageBatch, out int TotalRecord)
        {
            return (new DAL.BatchSQL().GetBatch(pkID, LoginUserID, PageNo, PageBatch, out TotalRecord));
        }

        public static List<Entity.Batch> GetBatch(Int64 pkID, string LoginUserID, string SearchKey, int PageNo, int PageBatch, out int TotalRecord)
        {
            return (new DAL.BatchSQL().GetBatch(pkID, LoginUserID, SearchKey, PageNo, PageBatch, out TotalRecord));
        }

        public static void AddUpdateBatch(Entity.Batch entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BatchSQL().AddUpdateBatch(entity, out ReturnCode, out ReturnMsg);
        }

        public static void DeleteBatch(Int64 pkID, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.BatchSQL().DeleteBatch(pkID, out ReturnCode, out ReturnMsg);
        }
    }
}
