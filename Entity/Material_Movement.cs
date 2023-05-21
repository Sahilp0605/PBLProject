using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class Material_Movement
    {
        public Int64 pkID { get; set; }
        public DateTime TransDate { get; set; }

        public string    TransType { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String OrderNo { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public String LoginUserID { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }
    }
    public class Material_MovementDetail
    {
        public Int64 pkID { get; set; }
        public Int64 ParentID { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public string ProductAlias { get; set; }
        public string ProductType { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }

        public Int64 BrandID { get; set; }
        public string BrandName { get; set; }
        public Decimal Quantity { get; set; }
        public String LoginUserID { get; set; }
        public Int64 LocationID { get; set; }
        public String LocationName { get; set; }
    }

    public class Material_Cons
    {
        public Int64 pkID { get; set; }
        public DateTime ConsDate { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public string OrderNo { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Material_ConsDetail
    {
        public Int64 pkID { get; set; }
        public Int64 ParentID { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public string ProductAlias { get; set; }
        public string ProductType { get; set; }
        public Int64 ProductGroupID { get; set; }
        public string ProductGroupName { get; set; }
        public Decimal Quantity { get; set; }
        public string LoginUserID { get; set; }
    }
    public class Material_Report
    {
        public Int64 pkID { get; set; }
        public DateTime TransDate { get; set; }

        public string TransType { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String OrderNo { get; set; }
        public Int64 EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public String LoginUserID { get; set; }
        public Int64 ProductID { get; set; }
        public String ProductName { get; set; }
        public decimal Dispatched { get; set; }
        public decimal Consumed { get; set; }
        public decimal Surplus{ get; set; }


    }
}
