using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CompanyRegistration
    {
        public Int64 pkID { get; set; }
        public string CompanyName	{ get; set; }
        public Int64 NoOfUsers	{ get; set; }
        public string SerialKey	{ get; set; }
        public string DBIP	{ get; set; }
        public string DBName	{ get; set; }
        public string DBUsername	{ get; set; }
        public string DBPassword	{ get; set; }
        public string LoginUserID { get; set; }
        public string Regno { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RootPath { get; set; }
        public string SiteURL { get; set; }
        public string IndiaMartKey { get; set; }
        public string IndiaMartMobile { get; set; }
        public string IndiaMartAcAlias { get; set; }

        public string IndiaMartKey2 { get; set; }
        public string IndiaMartMobile2 { get; set; }
        public string IndiaMartAcAlias2 { get; set; }
    }
}
