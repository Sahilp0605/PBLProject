using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ToDo
    {
        public Int64 RowNum { get; set; }
        public Int64 pkID { get; set; }
        public String Priority { get; set; }
        public String ActionTaken { get; set; }
        public String TaskDescription { get; set; }
        public String TaskDescriptionShort { get; set; }
        public String Location { get; set; }

        public Int64 TaskCategoryID { get; set; }
        public String TaskCategory { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public String Duration { get; set; }
        public String TaskStatus { get; set; }

        public Int64 ProjectID { get; set; }
        public String ProjectName { get; set; }

        public Int64 OrgCode { get; set; }
        public String OrgName { get; set; }

        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }

        public Int64 FromEmployeeID { get; set; }
        public String FromEmployeeName { get; set; }

        public Int64 SubTaskCompleted { get; set; }
        public Int64 TotalSubTask { get; set; }

        public Boolean Reminder { get; set; }
        public Int64 ReminderMonth { get; set; }

        public String ClosingRemarks { get; set; }

        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }

        public String LoginUserID { get; set; }

        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    
}
