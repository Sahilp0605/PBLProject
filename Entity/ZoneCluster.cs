using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ZoneCluster
    {
        public Int64 pkID { get; set; }
        public Int64 ClusterID { get; set; }
        public string ClusterName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public Int64 StateCode { get; set; }
        public string StateName { get; set; }
        public Int64 CityCode { get; set; }
        public string CityName { get; set; }

        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
