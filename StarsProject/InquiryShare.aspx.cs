using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;

namespace StarsProject
{
    public partial class InquiryShare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["userid"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    hdnUserID.Value = Request.QueryString["userid"].ToString();
                    BindDropDown();
                    setLayout();
                    BindInquiryOwner(txtInquiryNo.Text);
                    //OnlyViewControls();
                }
            }
        }
        public void OnlyViewControls()
        {
            txtInquiryNo.ReadOnly = true;
            txtInquiryDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtReferenceName.ReadOnly = true;
            //txtMeetingNotes.ReadOnly = true;
            //txtFollowupNotes.ReadOnly = true;
            //txtFollowupDate.ReadOnly = true;
            ////drpCustomer.Attributes.Add("disabled", "disabled");
            drpInquirySource.Attributes.Add("disabled", "disabled");
            drpInquiryStatus.Attributes.Add("disabled", "disabled");
            //btnSave.Visible = false;
            //btnSaveEmail.Visible = false;
            //btnReset.Visible = false;
        }
        public void BindDropDown()
        {
            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquiryStatus");
            drpInquiryStatus.DataSource = lstDesig;
            drpInquiryStatus.DataValueField = "pkID";
            drpInquiryStatus.DataTextField = "InquiryStatusName";
            drpInquiryStatus.DataBind();
            //drpInquiryStatus.Items.Insert(0, new ListItem("-- Select Status --", ""));

            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstSource = new List<Entity.InquiryStatus>();
            lstSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquirySource");
            drpInquirySource.DataSource = lstSource;
            drpInquirySource.DataValueField = "InquiryStatusName";
            drpInquirySource.DataTextField = "InquiryStatusName";
            drpInquirySource.DataBind();
            drpInquirySource.Items.Insert(0, new ListItem("-- Select --", ""));

            // ---------------- Employee List  -------------------------------------
            int TotalRecord = 0;
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            //lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeListByRole(hdnUserID.Value);
            //lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(hdnUserID.Value);
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            chklstEmployee.DataSource = lstEmployee;
            chklstEmployee.DataValueField = "pkID";
            chklstEmployee.DataTextField = "EmployeeName";
            chklstEmployee.DataBind();
        }
        public void setLayout()
        {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
                // ----------------------------------------------------
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                //hdnCustEmailAddress.Value = lstEntity[0].EmailAddress;
                //hdnEmployeeName.Value = lstEntity[0].EmployeeName;
                //hdnDesignation.Value = lstEntity[0].Designation;
                txtInquiryNo.Text = lstEntity[0].InquiryNo;
                txtInquiryDate.Text = lstEntity[0].InquiryDate.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                drpInquirySource.SelectedValue = lstEntity[0].InquirySource.ToString();
                txtReferenceName.Text = lstEntity[0].ReferenceName;
                //txtMeetingNotes.Text = lstEntity[0].MeetingNotes;
                //txtFollowupNotes.Text = lstEntity[0].FollowupNotes;
                ////txtFollowupDate.Text = lstEntity[0].FollowupDate.ToString("dd-MM-yyyy");
                //txtFollowupDate.Text = (lstEntity[0].FollowupDate != SqlDateTime.MinValue.Value) ? lstEntity[0].FollowupDate.ToString("dd-MM-yyyy") : "";
                drpInquiryStatus.SelectedValue = lstEntity[0].InquiryStatusID.ToString();
                // -------------------------------------------------------------------------
                hdnUserID.Value = lstEntity[0].CreatedBy;
        }
        protected void BindInquiryOwner(string inqno)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            // ----------------------------------------------------
            lstEntity = BAL.InquiryInfoMgmt.GetInquiryOwnerListByInquiryNo(txtInquiryNo.Text);

            for(int i=0;i<lstEntity.Count;i++)
            {
                for(int j=0;j<chklstEmployee.Items.Count; j++)
                {
                    if (chklstEmployee.Items[j].Value == lstEntity[i].EmployeeID.ToString())
                    {
                        chklstEmployee.Items[j].Selected = true;
                        break;
                    }             
                }
            }

            for (int j = 0; j < chklstEmployee.Items.Count; j++)
            {
                if (chklstEmployee.Items[j].Value == BAL.CommonMgmt.GetEmployeeIDByUserID(hdnUserID.Value))
                {
                    chklstEmployee.Items[j].Selected = true;
                    chklstEmployee.Items[j].Enabled = false;
                    break;
                }
            }

        }
       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Entity.InquiryInfo objEntity = new Entity.InquiryInfo();
            int ReturnCode = 0;
            string ReturnMsg = "";

            BAL.InquiryInfoMgmt.DeleteInquiryOwnerByInquiryNo(txtInquiryNo.Text, out ReturnCode, out ReturnMsg);
            if (ReturnCode>0)
            {
                for (int i = 0; i < chklstEmployee.Items.Count; i++)
                {
                    if (chklstEmployee.Items[i].Selected)
                    {
                        objEntity.InquiryNo = txtInquiryNo.Text;
                        objEntity.EmployeeID = Convert.ToInt64(chklstEmployee.Items[i].Value);
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        BAL.InquiryInfoMgmt.AddUpdateInquiryOwner(objEntity, out ReturnCode, out ReturnMsg);
                    }

                }  
            }
             
            if (ReturnCode > 0)
            {
                divErrorMessage.InnerHtml = ReturnMsg;
            }
        }
    }
}