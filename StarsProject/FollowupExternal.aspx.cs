using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Web.Services;

namespace StarsProject
{
    public partial class FollowupExternal : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["followupsource"]))
                {
                    hdnFollowUpSource.Value = Request.QueryString["followupsource"].ToString();
                }
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    string tmpval = BAL.CommonMgmt.GetConstant("AllowBackDatedFollowup", 0, 1);
                    hdnAllowBackDatedFollowup.Value = (!String.IsNullOrEmpty(tmpval)) ? tmpval : "Yes";
                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        // -----------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["ExtpkID"]))
                        {
                            hdnExtpkID.Value = Request.QueryString["ExtpkID"].ToString();
                            myFollowupTimeline.timelineCustomerID = hdnExtpkID.Value;
                            myFollowupTimeline.BindFollowupExtList();

                            int TotalCount;
                            List<Entity.ExternalLeads> lstEntity = new List<Entity.ExternalLeads>();
                            lstEntity = BAL.ExternalLeadsMgmt.GetExternalLeadList(Convert.ToInt64(hdnExtpkID.Value), "", "", Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                            if(lstEntity.Count > 0)
                            {
                                drpLeadStatus.SelectedValue = lstEntity[0].LeadStatus.ToString();
                                drpLeadStatus_SelectedIndexChanged(null, null);

                                if (lstEntity[0].EmployeeID.ToString() != "" && lstEntity[0].EmployeeID.ToString() != "0")
                                    drpAssignTo.SelectedValue = lstEntity[0].EmployeeID.ToString();

                                if (drpLeadStatus.SelectedValue.ToLower() == "qualified")
                                {
                                    drpLeadStatus.Attributes.Add("disabled", "disabled");
                                    drpAssignTo.Attributes.Add("disabled", "disabled");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                            hdnEntryMode.Value = Request.QueryString["mode"].ToString();
                        // -------------------------------------
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(hdnEntryMode.Value))
                        {
                            if (hdnEntryMode.Value == "view")
                                OnlyViewControls();
                        }
                    }

                }
            }
        }

        public void OnlyViewControls()
        {
            txtFollowupDate.ReadOnly = true;
            drpFollowupType.Attributes.Add("disabled", "disabled");
            txtNextFollowupDate.ReadOnly = true;
            txtPreferredTime.ReadOnly = true;
            txtMeetingNotes.ReadOnly = true;
            btnSave.Visible = false;
        }

        public void BindDropDown()
        {
            // ---------------- Followup Type -------------------------------------
            List<Entity.InquiryStatus> lstOrgDept22 = new List<Entity.InquiryStatus>();
            lstOrgDept22 = BAL.InquiryStatusMgmt.GetInquiryStatusList("Followup");
            drpFollowupType.DataSource = lstOrgDept22;
            drpFollowupType.DataValueField = "pkID";
            drpFollowupType.DataTextField = "InquiryStatusName";
            drpFollowupType.DataBind();
            drpFollowupType.Items.Insert(0, new ListItem("-- Select --", ""));

            // ---------------- Inquiry Closure Reason-------------------------------------
            List<Entity.InquiryStatus> lstReason = new List<Entity.InquiryStatus>();
            lstReason = BAL.InquiryStatusMgmt.GetInquiryStatusList("DisQualifiedReason");
            drpClosureReason.DataSource = lstReason;
            drpClosureReason.DataValueField = "pkID";
            drpClosureReason.DataTextField = "InquiryStatusName";
            drpClosureReason.DataBind();
            drpClosureReason.Items.Insert(0, new ListItem("-- Select Reason --", "0"));

            // ---------------- Inquiry Closure Reason-------------------------------------
            drpAssignTo.Items.Clear();
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpAssignTo.DataSource = lstEmployee;
            drpAssignTo.DataValueField = "pkID";
            drpAssignTo.DataTextField = "EmployeeName";
            drpAssignTo.DataBind();
            drpAssignTo.Items.Insert(0, new ListItem("-- Select --", ""));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.Followup> lstEntity = new List<Entity.Followup>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.FollowupMgmt.GetFollowupExtList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = (hdnEntryMode.Value != "continue") ? lstEntity[0].pkID.ToString() : "0";
                hdnExtpkID.Value = (hdnEntryMode.Value != "continue") ? lstEntity[0].ExtpkID.ToString() : "0";
                txtFollowupDate.Text = (hdnEntryMode.Value != "continue") ? lstEntity[0].FollowupDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                drpFollowupType.SelectedValue = lstEntity[0].InquiryStatusID.ToString();
                txtNextFollowupDate.Text = lstEntity[0].NextFollowupDate.ToString("yyyy-MM-dd");
                txtPreferredTime.Text = lstEntity[0].PreferredTime.ToString();
                hdnFollowUpSource.Value = lstEntity[0].FollowUpSource.ToString();
                // ----------------------------------------------------------------------------------- 
                if (hdnEntryMode.Value == "add" || String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0")
                {
                    txtNextFollowupDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtPreferredTime.Text = "";
                }
                if (hdnEntryMode.Value != "continue")
                    txtMeetingNotes.Text = (!String.IsNullOrEmpty(lstEntity[0].MeetingNotes)) ? lstEntity[0].MeetingNotes : "";
                else
                    txtMeetingNotes.Text = "";
                // -----------------------------------
                if (!String.IsNullOrEmpty(hdnExtpkID.Value))
                {
                    myFollowupTimeline.timelineCustomerID = hdnExtpkID.Value;
                    myFollowupTimeline.BindFollowupExtList();
                }
                // -----------------------------------------------------------------------------------       
                //drpLeadStatus.Items.FindByValue(lstEntity[0].InquiryStatusID.ToString()).Selected = true; 
                drpLeadStatus.SelectedValue = lstEntity[0].LeadStatus.ToString();
                if (lstEntity[0].LeadStatus.ToLower() == "disqualified")
                {
                    drpLeadStatus.Attributes.Add("disabled", "disabled");
                    drpClosureReason.Attributes.Add("disabled", "disabled");
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            string strErr = "";
            _pageValid = true;


            DateTime dt2 = DateTime.Now;

            if (String.IsNullOrEmpty(txtMeetingNotes.Text) ||
                String.IsNullOrEmpty(txtFollowupDate.Text) || String.IsNullOrEmpty(txtNextFollowupDate.Text) ||
                String.IsNullOrEmpty(drpFollowupType.SelectedValue))
            {
                _pageValid = false;


                if (String.IsNullOrEmpty(drpFollowupType.SelectedValue))
                    strErr += "<li>" + "Followup Type Selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtFollowupDate.Text))
                    strErr += "<li>" + "Followup Date is Required." + "</li>";

                if (String.IsNullOrEmpty(txtMeetingNotes.Text))
                    strErr += "<li>" + "Meeting Notes is Required." + "</li>";

                if (String.IsNullOrEmpty(txtNextFollowupDate.Text))
                    strErr += "<li>" + "Next FollowUp Date is Required." + "</li>";
            }
            // ----------------------------------------
            if (!String.IsNullOrEmpty(txtNextFollowupDate.Text))
            {
                if (!String.IsNullOrEmpty(txtFollowupDate.Text) && !String.IsNullOrEmpty(txtNextFollowupDate.Text))
                {
                    if (Convert.ToDateTime(txtNextFollowupDate.Text) < Convert.ToDateTime(txtFollowupDate.Text))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Next Followup Date should be greater than Followoup Date." + "</li>";
                    }
                }
                DateTime dt1 = Convert.ToDateTime((txtNextFollowupDate.Text + " " + txtPreferredTime.Text).Trim());
                if (hdnAllowBackDatedFollowup.Value.ToLower() == "no" && dt1 < dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Backdated Next Followup is not allowed." + "</li>";
                }

            }
            Entity.Followup objEntity = new Entity.Followup();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.ExtpkID = Convert.ToInt64(hdnExtpkID.Value);
                objEntity.FollowUpSource = hdnFollowUpSource.Value;
                objEntity.FollowupDate = Convert.ToDateTime(txtFollowupDate.Text);
                objEntity.InquiryStatusID = (!String.IsNullOrEmpty(drpFollowupType.SelectedValue)) ? Convert.ToInt64(drpFollowupType.SelectedValue) : Convert.ToInt64("0");
                objEntity.NextFollowupDate = (!String.IsNullOrEmpty(txtNextFollowupDate.Text)) ? Convert.ToDateTime(txtNextFollowupDate.Text) : SqlDateTime.MinValue.Value;
                objEntity.PreferredTime = txtPreferredTime.Text;
                objEntity.MeetingNotes = txtMeetingNotes.Text;
                objEntity.LeadStatus = drpLeadStatus.SelectedValue;
                objEntity.NoFollClosureID = (!String.IsNullOrEmpty(drpClosureReason.SelectedValue) && drpLeadStatus.SelectedValue.ToLower()=="disqualified") ? Convert.ToInt64(drpClosureReason.SelectedValue) : Convert.ToInt64("0");
                objEntity.EmployeeID = (!String.IsNullOrEmpty(drpAssignTo.SelectedValue) && (drpLeadStatus.SelectedValue.ToLower() == "qualified" || drpLeadStatus.SelectedValue.ToLower() == "inprocess")) ? Convert.ToInt64(drpAssignTo.SelectedValue) : Convert.ToInt64("0");
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.FollowupMgmt.AddUpdateFollowupExt(objEntity, out ReturnCode, out ReturnMsg);
                strErr += "<li>" + ReturnMsg + "</li>";
            }
            // ------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                }
            }

        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnExtpkID.Value = "";
            txtFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            drpFollowupType.SelectedValue = "";
            txtNextFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtMeetingNotes.Text = "";
            txtPreferredTime.Text = "";
            txtFollowupDate.Focus();
            btnSave.Disabled = false;

        }

        [System.Web.Services.WebMethod]
        public static string DeleteFollowupExt(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FollowupMgmt.DeleteFollowupExt(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpLeadStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpLeadStatus.SelectedValue.ToLower() == "inprocess")
            {
                divAssignTo.Visible = true;
                divClosureReason.Visible = false;
            }
            else if (drpLeadStatus.SelectedValue.ToLower() == "qualified")
            {
                divAssignTo.Visible = true;
                divClosureReason.Visible = false;
            }
            else if (drpLeadStatus.SelectedValue.ToLower() == "disqualified")
            {
                divAssignTo.Visible = false;
                divClosureReason.Visible = true;
            }
        }
    }
}