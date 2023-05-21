using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Currency
    {
        public Int64 pkID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyShortName { get; set; }
        public string CurrencySymbol { get; set; }

        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }
    }
}
