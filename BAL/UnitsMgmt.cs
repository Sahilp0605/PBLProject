using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class UnitsMgmt
    {
        public static List<Entity.Units> GetUnitList()
        {
            return (new DAL.UnitsSQL().GetUnitList());
        }
    }
}
