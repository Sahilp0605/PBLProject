using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class InternalWorkOrder
    {
        public Int64 pkID { get; set; }
        public String WorkOrderNo { get; set; }
        public DateTime WorkOrderDate { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String Address { get; set; }
        public String Area { get; set; }
        public String City { get; set; }
        public String PinCode { get; set; }
        public String EmailAddress { get; set; }
        public String ContactNo1 { get; set; }
        public String ContactNo2 { get; set; }
        public String ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public String SalesOrderNo { get; set; }
        public String QuotationNo { get; set; }
        public String InquiryNo { get; set; }
        public String LogInUserID { get; set; }
    }

    public class InternalWorkOrderDetail
    {
        public Int64 pkID { get; set; }

        public string WorkOrderNo { get; set; }
        public DateTime WorkOrderDate { get; set; }

        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLong { get; set; }
        public string ProductSpecification { get; set; }
        public DateTime DeliveryDate { get; set; }
        public String ScopeOfWork { get; set; }
        public String ScopeOfWork_Client { get; set; }
        public String Remarks { get; set; }
        public string LoginUserID { get; set; }

    }
}
