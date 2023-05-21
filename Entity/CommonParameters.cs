using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CommonParameters
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }

    public class Chat
    {
        public Int64 pkID { get; set; }
        public string FromUser { get; set; }
        public string FromUserImage { get; set; }
        public string ToUser { get; set; }
        public string ToUserImage { get; set; }
        public string Message { get; set; }
        public string Flag { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UserID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string RoleCode { get; set; }
        public DateTime LastTimestamp { get; set; }
        public Int64 UnreadMessageCount { get; set; }
    }

    public class DailyReport
    {
        public string Category { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public Int64 Count { get; set; }

        public string TaskDescription { get; set; }
        public string TaskCategory { get; set; }
        public Decimal TaskDeuration { get; set; }
    }

    public class DashboardInquirySummary
    {
        public Int64 Open { get; set; }
        public Int64 WorkInProgress { get; set; }
        public Int64 OnHold { get; set; }
        public Int64 CloseLost { get; set; }
        public Int64 CloseSuccess { get; set; }
        public Int64 Unknown { get; set; }
    }

    public class DashboardCountSummary
    {
        public string label { get; set; }
        public Decimal value { get; set; }
        public Decimal value1 { get; set; }
        public Decimal value2 { get; set; }
        public Decimal value3 { get; set; }
        public Decimal value4 { get; set; }
        public Decimal value5 { get; set; }
        public Decimal value6 { get; set; }
        public Int64 flag { get; set; }

        public Decimal Jan { get; set; }
        public Decimal Feb { get; set; }
        public Decimal Mar { get; set; }
        public Decimal Apr { get; set; }
        public Decimal May { get; set; }
        public Decimal Jun { get; set; }
        public Decimal Jul { get; set; }
        public Decimal Aug { get; set; }
        public Decimal Sep { get; set; }
        public Decimal Oct { get; set; }
        public Decimal Nov { get; set; }
        public Decimal Dec { get; set; }

        public Decimal d1 { get; set; }
        public Decimal d2 { get; set; }
        public Decimal d3 { get; set; }
        public Decimal d4 { get; set; }
        public Decimal d5 { get; set; }
        public Decimal d6 { get; set; }
        public Decimal d7 { get; set; }
        public Decimal d8 { get; set; }
        public Decimal d9 { get; set; }
        public Decimal d10 { get; set; }
        public Decimal d11 { get; set; }
        public Decimal d12 { get; set; }
        public Decimal d13 { get; set; }
        public Decimal d14 { get; set; }
        public Decimal d15 { get; set; }
        public Decimal d16 { get; set; }
        public Decimal d17 { get; set; }
        public Decimal d18 { get; set; }
        public Decimal d19 { get; set; }
        public Decimal d20 { get; set; }
        public Decimal d21 { get; set; }
        public Decimal d22 { get; set; }
        public Decimal d23 { get; set; }
        public Decimal d24 { get; set; }
        public Decimal d25 { get; set; }
        public Decimal d26 { get; set; }
        public Decimal d27 { get; set; }
        public Decimal d28 { get; set; }
        public Decimal d29 { get; set; }
        public Decimal d30 { get; set; }
        public Decimal d31 { get; set; }
    }

    public class CalenderEvent
    {
        public Int64 pkID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string className { get; set; }
        public string rendering
        {
            get { return "background"; }
        }
        public string imageurl { get; set; }
        public string ImageAttributes { get; set; }
    }

    public class DashboardNotification
    {
        public Int64 pkID { get; set; }
        public string LoginUserID { get; set; }
        public string ModuleName { get; set; }
        public Int64 ModulePkID { get; set;}
        public string Description { get; set; }
        public string CreatedDate { get; set; }
    }
    public class EmailNotifications
    {
        public Int64 pkID { get; set; }
        public string ModuleName { get; set; }
        public Int64 OwnerID { get; set; }
        public string OwnerType { get; set; }
        public Int64 TemplateID { get; set; }
        public string NotificationSent { get; set; }
        public DateTime LoginUserID { get; set; }
    }
    public class ConversationChatBox
    {
        public Int64 pkID { get; set; }
        public string ModuleName { get; set; }
        public string KeyValue { get; set; }
        public Int64 CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string FromUser { get; set; }
        public string FromEmployeeName { get; set; }
        public string ToUser { get; set; }
        public string ToEmployeeName { get; set; }
        public string Message { get; set; }
        public string LoginUserID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
