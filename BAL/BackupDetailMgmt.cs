using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class BackupDetailMgmt
    {
        public static void GenerateDatabaseBackup(Entity.BackupDetail entity)
        {
            new DAL.BackupDetailSQL().GenerateDatabaseBackup(entity);
        }
        public static List<Entity.BackupDetail> GetDatabaseBackupList(int PageNo, int PageSize, out int TotalRecord)
        {
            return new DAL.BackupDetailSQL().GetDatabaseBackupList(PageNo, PageSize, out TotalRecord);
        }
    }
}
