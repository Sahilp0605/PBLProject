using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class PendingMedicalLead : System.Web.UI.UserControl
    {
        int ReturnCode = 0;
        string ReturnMsg = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // -----------------------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            hdnRole.Value = objAuth.RoleCode.ToLower();
            hdnLoginUserID.Value = objAuth.LoginUserID;
        }

        public string pageView
        {
            get { return hdnView.Value; }
            set { hdnView.Value = value; }
        }
        public string pageFilter
        {
            get { return hdnFilter.Value.ToLower(); }
            set { hdnFilter.Value = value.ToLower(); }
        }
        public void BindPendingMedicalLead()
        {
            if (!String.IsNullOrEmpty(pageView))
            {
                List<Entity.PatientPayment> lstEntity = new List<Entity.PatientPayment>();
                lstEntity = BAL.InquiryInfoClinicMgmt.GetDashboardPatientSummary(pageView, 0, "", Session["LoginUserID"].ToString());
                if (pageFilter == "pending")
                    //lstEntity = lstEntity.Where(e => ((e.OrderNo == "" || String.IsNullOrEmpty(e.OrderNo)) && (e.BillNo == "" || String.IsNullOrEmpty(e.BillNo)))).ToList();
                    lstEntity = lstEntity.Where(e =>  (e.BillNo == "" || String.IsNullOrEmpty(e.BillNo))).ToList();
                else if (pageFilter == "completed")
                    //lstEntity = lstEntity.Where(e => (!String.IsNullOrEmpty(e.OrderNo) || !String.IsNullOrEmpty(e.BillNo))).ToList();
                    lstEntity = lstEntity.Where(e =>  !String.IsNullOrEmpty(e.BillNo)).ToList();

                    // ------------------------------------------------------------------
                rptPatient.DataSource = lstEntity;
                rptPatient.DataBind();
            }
        }

        public void UpdateBillInfo()
        {
            //foreach (RepeaterItem i in rptPatient.Items)
            //{
            //    Entity.SalesOrder objEntity = new Entity.SalesOrder();
            //    HiddenField hdnID = (HiddenField)i.FindControl("hdnpkID");
            //    DropDownList ddl = ((DropDownList)i.FindControl("drpApprovalStatus"));
            //    // --------------------------------------------------------
            //    if (!String.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
            //    {
            //        objEntity.pkID = Convert.ToInt64(hdnID.Value);
            //        objEntity.ApprovalStatus = ddl.SelectedValue;
            //        objEntity.LoginUserID = Session["LoginUserID"].ToString();
            //        // -------------------------------------------------------------- Insert/Update Record
            //        BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
            //    }
            //}
        }

        protected void rptPatient_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //DropDownList ddl = ((DropDownList)e.Item.FindControl("drpApprovalStatus"));
                //HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnApprovalStatus"));
                //HiddenField hdnEmployeeName = ((HiddenField)e.Item.FindControl("hdnEmployeeName"));
                //HiddenField hdnCreatedBy = ((HiddenField)e.Item.FindControl("hdnCreatedBy"));

                //if (!String.IsNullOrEmpty(tmpField.Value))
                //    ddl.SelectedValue = tmpField.Value;
                //// -----------------------------------------------------------------------
                //Entity.Authenticate objAuth = new Entity.Authenticate();
                //objAuth = (Entity.Authenticate)Session["logindetail"];
                //// -----------------------------------------------------------------------
                ////if ((hdnView.Value == "dashboard" || objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()) && (objAuth.RoleCode.ToLower() != "admin" && objAuth.RoleCode.ToLower() != "bradmin" && objAuth.RoleCode.ToLower() != "supervisor"))
                ////    ddl.Attributes["disabled"] = "disabled";
                //if  (objAuth.RoleCode.ToLower() != "admin" && (objAuth.RoleCode.ToLower() == "bradmin" && objAuth.EmployeeName.ToLower() == hdnEmployeeName.Value.ToLower()))
                //    ddl.Attributes["disabled"] = "disabled";
            }
        }
    
    }
}