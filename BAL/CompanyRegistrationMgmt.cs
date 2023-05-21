using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class CompanyRegistrationMgmt
    {
        public static List<Entity.CompanyRegistration> GetCompanyRegistrationList(String LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CompanyRegistrationSQL().GetCompanyRegistrationList(LoginUserID,  PageNo,  PageSize, out TotalRecord));
        }

        public static List<Entity.CompanyRegistration> GetCompanyRegistration(Int64 pkID, string LoginUserID, int PageNo, int PageSize, out int TotalRecord)
        {
            return (new DAL.CompanyRegistrationSQL().GetCompanyRegistration(pkID, LoginUserID, PageNo, PageSize, out TotalRecord));
        }

        public static List<Entity.CompanyRegistration> GetCompanyRegistrationBySerialKey(String pSerialKey)
        {
            return (new DAL.CompanyRegistrationSQL().GetCompanyRegistrationBySerialKey(pSerialKey));
        }

        public static void AddUpdateCompanyRegistration(Entity.CompanyRegistration entity, out int ReturnCode, out string ReturnMsg)
        {
            new DAL.CompanyRegistrationSQL().AddUpdateCompanyRegistration(entity, out ReturnCode, out ReturnMsg);
        }
    }
}
