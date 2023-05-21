using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CountryISO { get; set; }

        public Boolean ActiveFlag { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
