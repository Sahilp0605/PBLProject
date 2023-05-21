using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ApplicationMenu
    {
        public Int64 pkID { get; set; }
        public string GenpkID { get; set; }
        public string MenuName { get; set; }
        public string MenuText { get; set; }
        public Int64? ParentId { get; set; }
        public bool Active { get; set; }
        public Int64 MenuLevel { get; set; }
        public Int64 MenuOrder { get; set; }
        public string MenuURL { get; set; }
        public string MenuImage { get; set; }
        public Int64 MenuImageHeight { get; set; }
        public Int64 MenuImageWidth { get; set; }
        public bool AddFlag { get; set; }
        public bool EditFlag { get; set; }
        public bool DelFlag { get; set; }
        public string UserID { get; set; }
        public List<ApplicationMenu> List { get; set; }
    }

    public class ReportMenu
    {
        public Int64 pkID { get; set; }
        public string ReportName { get; set; }
        public string ReportText { get; set; }
        public Int64? ParentID { get; set; }
        public bool Active { get; set; }
        public Int64 ReportOrder { get; set; }
        public Int64 ReportLevel { get; set; }
        public string ReportURL { get; set; }
        public string ReportImage { get; set; }
        public Int64 ReportImageHeight { get; set; }
        public Int64 ReportImageWidth { get; set; }
        public List<ReportMenu> List { get; set; }
    }
}
