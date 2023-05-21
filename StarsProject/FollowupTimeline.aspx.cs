using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class FollowupTimeline : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["view"]))
                hdnView.Value = Request.QueryString["view"].ToString().Trim();

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                hdnCustomerID.Value = Request.QueryString["id"].ToString().Trim();
                myFollowupTimeline.timelineCustomerID = hdnCustomerID.Value;
            }


            if (!String.IsNullOrEmpty(Request.QueryString["name"]))
            {
                hdnCustomerName.Value = Request.QueryString["name"].ToString().Trim();
                lblCustomerName.Text = hdnCustomerName.Value;
            }
            // ---------------------------------------------------------------------------
            int TotalCount = 0;
            if (hdnView.Value == "followup")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                {
                    myFollowupTimeline.BindFollowupList();
                    // --------------------------------------------
                    List<Entity.Customer> lstEntity = new List<Entity.Customer>();
                    // ----------------------------------------------------
                    lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                    // ----------------------------------------------------

                    if (!String.IsNullOrEmpty(lstEntity[0].Address.Trim()) && lstEntity[0].Address.Trim() != "" && lstEntity[0].Address.Trim() != "NULL")
                    {
                        lblAddress11.Text = lstEntity[0].Address;
                        lblAddress12.Text = lstEntity[0].Area + (!String.IsNullOrEmpty(lstEntity[0].CityName) ? ", " + lstEntity[0].CityName : "") + (!String.IsNullOrEmpty(lstEntity[0].Pincode) ? ", " + lstEntity[0].Pincode : "");
                        lblAddress13.Text = (!String.IsNullOrEmpty(lstEntity[0].StateName) ? ", " + lstEntity[0].StateName : "INDIA");
                    }

                    lblContact.Text = String.Concat(lstEntity[0].ContactNo1, (!String.IsNullOrEmpty(lstEntity[0].ContactNo2) ? ", " + lstEntity[0].ContactNo2 : ""));
                    lblEmail.Text = lstEntity[0].EmailAddress;
                    lblWebsite.Text = lstEntity[0].WebsiteAddress;
                }
            }

            if (hdnView.Value == "followupext")
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                {
                    myFollowupTimeline.BindFollowupExtList();
                    // ----------------------------------------------------
                    List<Entity.ExternalLeads> lstEntity = new List<Entity.ExternalLeads>();
                    lstEntity = BAL.ExternalLeadsMgmt.GetExternalLeadList(Convert.ToInt64(hdnCustomerID.Value), "", "telecaller", 1, 1000, out TotalCount);
                    // ----------------------------------------------------
                    lblCustomerName.Text = String.Concat(lstEntity[0].CompanyName, (!String.IsNullOrEmpty(lstEntity[0].SenderName) ? ", " + lstEntity[0].SenderName: ""));
                    if (!String.IsNullOrEmpty(lstEntity[0].Address.Trim()) && lstEntity[0].Address.Trim() != "" && lstEntity[0].Address.Trim() != "NULL")
                    {
                        lblAddress11.Text = lstEntity[0].Address;
                        lblAddress12.Text = (!String.IsNullOrEmpty(lstEntity[0].City) ? ", " + lstEntity[0].City : "") + (!String.IsNullOrEmpty(lstEntity[0].Pincode) ? ", " + lstEntity[0].Pincode : "");
                        lblAddress13.Text = (!String.IsNullOrEmpty(lstEntity[0].State) ? ", " + lstEntity[0].State : "INDIA");
                    }

                    lblContact.Text = String.Concat(lstEntity[0].PrimaryMobileNo, (!String.IsNullOrEmpty(lstEntity[0].SecondaryMobileNo) ? ", " + lstEntity[0].SecondaryMobileNo : ""));
                    lblEmail.Text = lstEntity[0].SenderMail;
                    lblWebsite.Text = "";
                }
            }
        }
    }
}