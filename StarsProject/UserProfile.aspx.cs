using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];
                // -----------------------------------------------------------
                showWorkPerfomance(objAuth.UserID);
                hdnLoginUserID.Value = objAuth.UserID;
                imgUserProfile.Src = (!String.IsNullOrEmpty(objAuth.EmployeeImage)) ? objAuth.EmployeeImage : "images/customer.png";
            }
        }

        public void showWorkPerfomance(string LoginUserID)
        {
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeWorkPerfomance(LoginUserID);

            hdnEmployeeID.Value = lstEmployee[0].pkID.ToString();
            lblUserName.Text = lstEmployee[0].EmployeeName;
            lblDesignation.Text = lstEmployee[0].Designation;
            lnkFollower.InnerText = lstEmployee[0].Follower.ToString();
            lnkInquiry.InnerText = lstEmployee[0].Inquiry.ToString();
            lnkQuotation.InnerText = lstEmployee[0].Quotation.ToString();
            lnkFollowup.InnerText = lstEmployee[0].Followup.ToString();
            lnkSalesOrder.InnerText = lstEmployee[0].SalesOrder.ToString();
            lnkSalesBill.InnerText = lstEmployee[0].SalesBill.ToString();
            lnkPurcBill.InnerText = lstEmployee[0].PurcBill.ToString();
            lnkCustomers.InnerText = lstEmployee[0].Customers.ToString();
            ifrPersonal.Attributes["src"] = "OrgEmployee.aspx?mode=Edit&type=profile1&id=" + hdnEmployeeID.Value;
        }
    }
}