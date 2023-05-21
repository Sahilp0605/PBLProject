using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class ApplicationMenuMgmt
    {
        public static List<Entity.ApplicationMenu> GetMenuByParent(string pModule, Int64 ParentId)
        {
            return (new DAL.ApplicationMenuSQL().GetMenuByParent(pModule, ParentId));
        }
    }
}
