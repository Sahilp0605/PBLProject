using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CategoryDescription
    {
        public Int64 pkID { get; set; }
        public String Category { get; set; }
        public String Description { get; set; }
        public String LoginUserID { get; set; }
    }
    public class SiteSurveyReport
    {
        public Int64 pkID { get; set; }
        public String SurveyID { get; set; }
        public DateTime VisitDate { get; set; }
        public Int64 CustomerID { get; set; }
        public String CustomerName{ get; set; }
        public String Address { get; set; }
        public String Area { get; set; }
        public String Pincode { get; set; }
        public String City { get; set; }
        public String EmailAddress { get; set; }
        public String ContactNo1 { get; set; }
        public String ContactNo2 { get; set; }
        public String SolarPosition { get; set; }
        public String SolarPositionRemarks { get; set; }
        public String BuildType { get; set; }
        public String BuildTypeRemarks { get; set; }
        public String RoofType { get; set; }
        public String RoofTypeRemarks { get; set; }
        public String ClientReq { get; set; }
        public String SanctionLoad { get; set; }
        public String MonthlyConsumption { get; set; }
        public String TotalArea { get; set; }
        public Boolean LeaseProperty { get; set; }
        public String ExistingPhase { get; set; }
        public String ExistingPhaseRemarks { get; set; }
        public Boolean Synchronization { get; set; }
        public String ReasonForSync { get; set; }
        public String Remarks { get; set; }
        public String ClientQueries { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedEmployeeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public String UpdatedEmployeeName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String LoginUserID { get; set; }
    }
    public class SSRRoofDetails
    {
        public Int64 pkID { get; set; }
        public String SurveyID {get; set;}
        public String RoofType {get; set; }
        public String RoofArea { get; set; }
        public String BuildingName { get; set; }
        public String CapacityOfBuilding { get; set; }
        public String LoginUserID { get; set; }
    }
    public class SSREquipmentLocation
    {
        public Int64 pkID { get; set; }
        public String SurveyID { get; set; }
        public String Equipment { get; set; }
        public String Distance { get; set; }
        public String ConnPossibility { get; set; }
        public String ClientRating { get; set; }
        public String LoginUserID { get; set; }
    }
    public class SSRSysAvailablity
    {
       public Int64 pkID { get; set; }
        public String SurveyID { get; set; }
        public String LoadDesc { get; set; }
        public String Capacity { get; set; }
        public String Voltage { get; set; }
        public String Quantity { get; set; }
        public String LoginUserID { get; set; }
    }
    public class SSRRequiredDetails
    {
        public Int64 pkID { get; set; }
        public String SurveyID { get; set; }
        public String Description { get; set; }
        public String Remarks { get; set; }
        public String LoginUserID { get; set; }
    }
}
