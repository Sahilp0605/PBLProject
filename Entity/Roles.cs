using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Roles
    {
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public Boolean ActiveFlag { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }

        //-------For Role Rights---------//
        public string MenuId { get; set; }
    }
}
