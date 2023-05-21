﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Designations
    {
        public string DesigCode { get; set; }
        public string Designation { get; set; }
        public Boolean ActiveFlag { get; set; }
        public string ActiveFlagDesc { get; set; }

        public string UserID_CreatedBy { get; set; }
        public DateTime UserID_CreatedDate { get; set; }
        public string UserID_ModifiedBy { get; set; }
        public DateTime UserID_ModifiedDate { get; set; }

        public string LoginUserID { get; set; }
    }
}
