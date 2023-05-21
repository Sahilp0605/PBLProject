using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ToDoCategory
    {
        public Int64 pkID { get; set; }
        public String Category { get; set; }
        public String TaskCategoryName { get; set; }
        public String LoginUserID { get; set; }
    }
}
