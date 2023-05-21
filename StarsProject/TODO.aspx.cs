using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

//---
namespace StarsProject
{
    public partial class TODO : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;

                BindDropDown();
                if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                {
                    hdnToDOMode.Value = Request.QueryString["mode"].ToString().ToLower();
                    hdnSerialKey.Value = Session["SerialKey"].ToString();
                }

                // --------------------------------------------------------
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))   
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        ClearAllField();
                    else
                    {
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                        {
                            if (Request.QueryString["mode"].ToString() == "view")
                                OnlyViewControls();
                        }
                    }
                }
            }
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            //ScriptManager.RegisterStartupScript(this, typeof(string), "setrating1", "javascript:setRating();bindHoverEvent();", true);
        }

        public void BindDropDown()
        {
            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpAssignTo.DataSource = lstEmployee;
            drpAssignTo.DataValueField = "pkID";
            drpAssignTo.DataTextField = "EmployeeName";
            drpAssignTo.DataBind();
            drpAssignTo.Items.Insert(0, new ListItem("-- Select Employee --", ""));

            // ---------------- Assign Employee ------------------------
            drpTransferTo.DataSource = lstEmployee;
            drpTransferTo.DataValueField = "pkID";
            drpTransferTo.DataTextField = "EmployeeName";
            drpTransferTo.DataBind();
            drpTransferTo.Items.Insert(0, new ListItem("-- Select Employee --", ""));

            List<Entity.ToDoCategory> lstTaskCat = new List<Entity.ToDoCategory>();
            lstTaskCat = BAL.ToDoCategoryMgmt.GetTaskCategoryList("TODO");
            drpTaskCategory.DataSource = lstTaskCat;
            drpTaskCategory.DataValueField = "pkID";
            drpTaskCategory.DataTextField = "TaskCategoryName";
            drpTaskCategory.DataBind();
            drpTaskCategory.Items.Insert(0, new ListItem("-- Nature Of Task --", ""));
        }

        public void OnlyViewControls()
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            // -----------------------------------------------------------------------
            btnSave.Visible = (hdnToDOMode.Value != "view") ? true : false;
            btnReset.Visible = (hdnToDOMode.Value != "view") ? true : false;
            // -------------------------------------------------------------------
            //if (hdnCreatedBy.Value != Session["LoginUserID"].ToString() && objAuth.RoleCode.ToLower() != "admin")
            //{
            //    txtTaskDescription.Enabled = false;
            //    drpPriority.Attributes.Add("disabled", "disabled");
            //    txtStartDate.ReadOnly = true;
            //    txtStartTime.ReadOnly = true;
            //    txtDueDate.ReadOnly = true;
            //    txtDueTime.ReadOnly = true;
            //    drpAssignTo.Enabled = false;
            //    drpTaskCategory.Enabled = false;
            //    txtLocation.Enabled = true;
            //    chkReminder.Enabled = false;
            //    chkReminder.Attributes.Add("disabled", "disabled");
            //    drpReminderMonth.Attributes.Add("disabled", "disabled");
            //}


            txtCompletionDate.ReadOnly = ((hdnToDOMode.Value != "view" && (hdnCreatedBy.Value == Session["LoginUserID"].ToString() || hdnAssignToEmployeeName.Value.ToLower() == objAuth.EmployeeName.ToLower())) || objAuth.RoleCode.ToLower() == "admin") ? false : true;
            txtClosingRemarks.ReadOnly = ((hdnToDOMode.Value != "view" && (hdnCreatedBy.Value == Session["LoginUserID"].ToString() || hdnAssignToEmployeeName.Value.ToLower() == objAuth.EmployeeName.ToLower())) || objAuth.RoleCode.ToLower() == "admin") ? false : true;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.ToDoMgmt.GetToDoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtTaskDescription.Text = lstEntity[0].TaskDescription.ToString();
                txtLocation.Text = lstEntity[0].Location.ToString();
                drpTaskCategory.SelectedValue = lstEntity[0].TaskCategoryID.ToString();
                drpPriority.SelectedValue = lstEntity[0].Priority.ToString();
                txtStartDate.Text = (lstEntity[0].StartDate.Year <= 1900) ? "" : lstEntity[0].StartDate.ToString("yyyy-MM-dd");
                txtDueDate.Text = (lstEntity[0].DueDate.Year <= 1900) ? "" : lstEntity[0].DueDate.ToString("yyyy-MM-dd");
                txtCompletionDate.Text = (lstEntity[0].CompletionDate.Year <= 1900) ? "" : lstEntity[0].CompletionDate.ToString("yyyy-MM-dd");
                txtClosingRemarks.Text = String.IsNullOrEmpty(lstEntity[0].ClosingRemarks.ToString()) ? "" : lstEntity[0].ClosingRemarks.ToString();
                txtStartTime.Text = lstEntity[0].StartDate.ToString("HH:mm tt");
                txtDueTime.Text = lstEntity[0].DueDate.ToString("HH:mm tt");
                txtStartTime.Text = (txtStartTime.Text == "00:00 AM" || txtStartTime.Text == "00:00 PM") ? "00:00 AM" : txtStartTime.Text;
                txtDueTime.Text = (txtDueTime.Text == "00:00 AM" || txtDueTime.Text == "00:00 PM") ? "23:59 PM" : txtDueTime.Text;
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                chkReminder.Checked = lstEntity[0].Reminder;
                drpReminderMonth.SelectedValue = lstEntity[0].ReminderMonth.ToString();
                drpAssignTo.SelectedValue = lstEntity[0].EmployeeID.ToString();
                hdnCreatedBy.Value = lstEntity[0].CreatedBy;
                hdnAssignTo.Value = lstEntity[0].EmployeeID.ToString();
                hdnAssignToEmployeeName.Value = lstEntity[0].EmployeeName;
                hdnCompletionDate.Value = (lstEntity[0].CompletionDate.Year <= 1900) ? "" : lstEntity[0].CompletionDate.ToString("yyyy-MM-dd");
                //----------------Experiment---------------------------
                if (Session["LoginUserID"].ToString() != hdnCreatedBy.Value)
                {
                    txtTaskDescription.ReadOnly = true;
                    //drpTaskCategory.Attributes.Add("disabled", "disabled");
                    drpTaskCategory.Enabled = false;
                    //drpPriority.Attributes.Add("disabled", "disabled");
                    drpPriority.Enabled = false;
                    txtLocation.ReadOnly = true;
                    //drpAssignTo.Attributes.Add("disabled", "disabled");
                    drpAssignTo.Enabled = false;
                    txtStartDate.ReadOnly = true;
                    txtStartTime.ReadOnly = true;
                    txtDueDate.ReadOnly = true;
                    txtDueTime.ReadOnly = true;
                    chkReminder.Attributes.Add("disabled", "disabled");
                    //drpReminderMonth.Attributes.Add("disabled", "disabled");
                    drpReminderMonth.Enabled = false;
                }
                // ----------------------------------------------------
                List<Entity.ToDo> lstEntityLog = new List<Entity.ToDo>();
                lstEntityLog = BAL.ToDoMgmt.GetToDoLogList(lstEntity[0].pkID);
                rptToDOLog.DataSource = lstEntityLog;
                rptToDOLog.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnHeaderID = 0;
            string ReturnMsg = "";
            int ReturnCode1 = 0;
            string ReturnMsg1 = "";
            String strErr = "";
            _pageValid = true;

            if (String.IsNullOrEmpty(txtTaskDescription.Text) || String.IsNullOrEmpty(txtStartDate.Text) || String.IsNullOrEmpty(txtDueDate.Text)
               || String.IsNullOrEmpty(drpTaskCategory.SelectedValue) || String.IsNullOrEmpty(drpAssignTo.SelectedValue) || txtTaskDescription.Text.Length > 350 || txtClosingRemarks.Text.Length > 350)
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtTaskDescription.Text))
                    strErr += "<li>" + "Task Description is required." + "</li>";

                if (String.IsNullOrEmpty(drpTaskCategory.SelectedValue))
                    strErr += "<li>" + "Task Category is required." + "</li>";

                if (String.IsNullOrEmpty(drpAssignTo.SelectedValue))
                {
                    strErr += "<li>" + "Assign To Name is required." + "</li>";
                }

                if (String.IsNullOrEmpty(txtStartDate.Text))
                    strErr += "<li>" + "Start Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtDueDate.Text))
                    strErr += "<li>" + "Due Date is required." + "</li>";

                if (txtTaskDescription.Text.Length > 350)
                    strErr += "<li>" + "Tast Description should not greater than 350 characters." + "</li>";

                if (txtClosingRemarks.Text.Length > 350)
                    strErr += "<li>" + "Closing Remark should not greater than 350 characters." + "</li>";
            }
            if (!String.IsNullOrEmpty(txtStartDate.Text) || !String.IsNullOrEmpty(txtDueDate.Text))
            {

                if (Convert.ToDateTime(txtDueDate.Text) < Convert.ToDateTime(txtStartDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Due Date should be greater than Start Date." + "</li>";
                }
            }

            if (Convert.ToDateTime(txtStartDate.Text) < Convert.ToDateTime(DateTime.Now.ToShortDateString()) && (String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0"))
            {
                _pageValid = false;
                strErr += "<li>" + "No Backdate task allowed." + "</li>";
            }
            if (!String.IsNullOrEmpty(txtCompletionDate.Text))
            {
                if (Convert.ToDateTime(txtCompletionDate.Text) < Convert.ToDateTime(txtStartDate.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Completion Date should be greater than Start Date." + "</li>";
                }
                //else if (Convert.ToDateTime(txtCompletionDate.Text) < Convert.ToDateTime(txtDueDate.Text))
                //{
                //    _pageValid = false;
                //    strErr += "<li>" + "Completion Date should be greater than Due Date." + "</li>";
                //}
            }

            // --------------------------------------------------------------
            if (drpOption.SelectedValue.ToLower() == "reassign")
            {
                if (!String.IsNullOrEmpty(txtCompletionDate.Text))
                {
                    if (Convert.ToDateTime(txtCompletionDate.Text) < Convert.ToDateTime(txtStartDate.Text))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Completion Date should be greater than Start Date." + "</li>";
                    }
                }

                if (!String.IsNullOrEmpty(drpTransferTo.SelectedValue) && String.IsNullOrEmpty(txtClosingRemarks.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Closing Remark is Mandatory While Re-Assiging Task to Someone Else." + "</li>";
                }
            }

            if (drpOption.SelectedValue.ToLower() == "activity")
            {
                if (String.IsNullOrEmpty(txtClosingRemarks.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Closing Remark is Mandatory While Re-Assiging Task to Someone Else." + "</li>";
                }
            }

            if (drpOption.SelectedValue.ToLower() == "complete")
            {
                if (!String.IsNullOrEmpty(txtCompletionDate.Text))
                {
                    if (String.IsNullOrEmpty(txtClosingRemarks.Text))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Closing Remark is mandatory while Completing Task." + "</li>";
                    }

                    if (Convert.ToDateTime(txtCompletionDate.Text) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Future Complition Date is not allowed." + "</li>";
                    }
                }
            }
            // --------------------------------------------------------------
            if (_pageValid == true)
            {
                Entity.ToDo objEntity = new Entity.ToDo();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.TaskDescription = txtTaskDescription.Text;
                objEntity.Location = txtLocation.Text;

                if (!String.IsNullOrEmpty(drpTaskCategory.SelectedValue))
                    objEntity.TaskCategoryID = Convert.ToInt64(drpTaskCategory.SelectedValue);

                objEntity.Priority = drpPriority.SelectedValue;

                if (!String.IsNullOrEmpty(txtStartDate.Text) && !String.IsNullOrEmpty(txtStartTime.Text))
                    objEntity.StartDate = Convert.ToDateTime(Convert.ToDateTime(txtStartDate.Text + " " + txtStartTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                //if (!String.IsNullOrEmpty(txtStartDate.Text))
                //{
                //    if (Convert.ToDateTime(txtStartDate.Text).Year > 1900)
                //        objEntity.StartDate = Convert.ToDateTime(txtStartDate.Text);
                //}

                if (!String.IsNullOrEmpty(txtDueDate.Text) && !String.IsNullOrEmpty(txtDueTime.Text))
                    objEntity.DueDate = Convert.ToDateTime(Convert.ToDateTime(txtDueDate.Text + " " + txtDueTime.Text).ToString("yyyy-MM-dd HH:mm tt"));

                //if (!String.IsNullOrEmpty(txtDueDate.Text))
                //{
                //    if (Convert.ToDateTime(txtDueDate.Text).Year > 1900)
                //        objEntity.DueDate = Convert.ToDateTime(txtDueDate.Text);
                //}


                if (!String.IsNullOrEmpty(txtCompletionDate.Text))
                {
                    if (Convert.ToDateTime(txtCompletionDate.Text).Year > 1900)
                        objEntity.CompletionDate = Convert.ToDateTime(txtCompletionDate.Text);
                }
                objEntity.ClosingRemarks = txtClosingRemarks.Text;
                objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value) ? Convert.ToInt64(hdnCustomerID.Value) : 0);
                if (drpOption.SelectedValue == "reassign")
                {
                    if (drpAssignTo.SelectedValue == drpTransferTo.SelectedValue)
                        objEntity.EmployeeID = (!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0;
                    else
                        objEntity.EmployeeID = (!String.IsNullOrEmpty(drpTransferTo.SelectedValue)) ? Convert.ToInt64(drpTransferTo.SelectedValue) : 0;
                }
                else
                {
                    objEntity.EmployeeID = (!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0;
                }
                objEntity.Reminder = (chkReminder.Checked) ? true : false;
                objEntity.ReminderMonth = Convert.ToInt64(drpReminderMonth.SelectedValue);
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ToDoMgmt.AddUpdateToDo(objEntity, out ReturnCode, out ReturnMsg, out ReturnHeaderID);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    // ----------------------------------------------
                    Entity.ToDo objEntityLog = new Entity.ToDo();
                    String strDescription = "Action Performed";
                    String strAction = "Modified";
                    string notificationMsg = "";

                    if ((!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0") || ReturnHeaderID > 0)
                    {
                        if ((String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0") && ReturnHeaderID > 0)
                        {
                            strAction = "Task Initiated";
                            strDescription = "Task Assigned To  " + drpAssignTo.SelectedItem.Text;

                            try
                            {
                                notificationMsg = "To-Do Task " + txtTaskDescription.Text + " Is Created And Assign To " + BAL.CommonMgmt.GetEmployeeNameByEmployeeID((!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0) + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                BAL.CommonMgmt.SendNotification_Firebase("To-Do", notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64((!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0));
                                BAL.CommonMgmt.SendNotificationToDB("To-Do", ReturnHeaderID, notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64((!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0));
                            }
                            catch (Exception ex)
                            {}
                        }
                        else
                        {
                            if (drpOption.SelectedValue == "reassign")
                            {
                                if (drpAssignTo.SelectedValue != drpTransferTo.SelectedValue)
                                {
                                    strAction = "Task Transferred";
                                    strDescription = "Task Transfered From " + drpAssignTo.SelectedItem.Text + " To " + drpTransferTo.SelectedItem.Text;

                                    try
                                    {
                                        notificationMsg = "To-Do Task " + txtTaskDescription.Text + " Is Transfered From " + drpAssignTo.SelectedItem.Text + " And Assign To " + drpTransferTo.SelectedItem.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                        BAL.CommonMgmt.SendNotification_Firebase("To-Do", notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64((!String.IsNullOrEmpty(drpTransferTo.SelectedValue)) ? Convert.ToInt64(drpTransferTo.SelectedValue) : 0));
                                        BAL.CommonMgmt.SendNotificationToDB("To-Do", ReturnHeaderID, notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64((!String.IsNullOrEmpty(drpTransferTo.SelectedValue)) ? Convert.ToInt64(drpTransferTo.SelectedValue) : 0));
                                    }
                                    catch (Exception ex)
                                    {}
                                }
                            }
                            if (drpOption.SelectedValue == "complete")
                            {
                                if (!String.IsNullOrEmpty(txtCompletionDate.Text))
                                {
                                    if (Convert.ToDateTime(txtCompletionDate.Text).Year >= DateTime.Now.Year && !String.IsNullOrEmpty(txtClosingRemarks.Text))
                                    {
                                        strAction = "Task Complete";
                                        strDescription = "Task Completed On " + txtCompletionDate.Text;

                                        try
                                        {
                                            notificationMsg = "To-Do Task " + txtTaskDescription.Text + " Is Completed On  " + txtCompletionDate.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                            BAL.CommonMgmt.SendNotification_Firebase("To-Do", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                            BAL.CommonMgmt.SendNotificationToDB("To-Do", ReturnHeaderID, notificationMsg, Session["LoginUserID"].ToString(), 0);
                                        }
                                        catch (Exception ex)
                                        {}
                                    }
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(hdnCompletionDate.Value))
                                    {
                                        strAction = "Task Re-Open";
                                        strDescription = "Task Re-Open By User ID : " + Session["LoginUserID"].ToString();

                                        try
                                        {
                                            notificationMsg = "To-Do Task " + txtTaskDescription.Text + " Is Re-Open By  " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                            BAL.CommonMgmt.SendNotification_Firebase("To-Do", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                            BAL.CommonMgmt.SendNotificationToDB("To-Do", ReturnHeaderID, notificationMsg, Session["LoginUserID"].ToString(), 0);
                                        }
                                        catch (Exception ex)
                                        {}
                                    }
                                }
                            }
                            if (drpOption.SelectedValue == "activity")
                            {
                                strAction = "Task Activity";
                                strDescription = "Task Acitivity Added";

                                try
                                {
                                    notificationMsg = "To-Do Task Acitivity Added for " + txtTaskDescription.Text + " By  " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                    BAL.CommonMgmt.SendNotification_Firebase("To-Do", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                    BAL.CommonMgmt.SendNotificationToDB("To-Do", ReturnHeaderID, notificationMsg, Session["LoginUserID"].ToString(), 0);
                                }
                                catch (Exception ex)
                                {}
                            }

                            if(notificationMsg == "" && (Convert.ToInt64(hdnpkID.Value) > 0 && ReturnHeaderID > 0))
                            {
                                try
                                {
                                    notificationMsg = "To-Do Task " + txtTaskDescription.Text + " Is Updated By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                    BAL.CommonMgmt.SendNotification_Firebase("To-Do", notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64((!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0));
                                    BAL.CommonMgmt.SendNotificationToDB("To-Do", ReturnHeaderID, notificationMsg, Session["LoginUserID"].ToString(), Convert.ToInt64((!String.IsNullOrEmpty(drpAssignTo.SelectedValue)) ? Convert.ToInt64(drpAssignTo.SelectedValue) : 0));
                                }
                                catch (Exception ex)
                                { }
                            }
                        }

                        objEntityLog.pkID = ((String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0") && ReturnHeaderID > 0) ? ReturnHeaderID : objEntity.pkID;
                        objEntityLog.ActionTaken = strAction;
                        objEntityLog.TaskDescription = strDescription;
                        objEntityLog.EmployeeID = (!String.IsNullOrEmpty(drpTransferTo.SelectedValue)) ? Convert.ToInt64(drpTransferTo.SelectedValue) : 0;
                        objEntityLog.ClosingRemarks = txtClosingRemarks.Text; ;
                        objEntityLog.LoginUserID = Session["LoginUserID"].ToString();
                        BAL.ToDoMgmt.AddUpdateToDoLog(objEntityLog, out ReturnCode1, out ReturnMsg1);
                    }
                }
                // --------------------------------------------------------------
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtTaskDescription.Text = "";
            drpTaskCategory.SelectedValue = "";
            txtLocation.Text = "";
            drpAssignTo.SelectedValue = "";
            drpPriority.SelectedValue = "Low";
            txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtStartTime.Text = "00:00 AM";
            txtDueDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDueTime.Text = "23:59 PM";
            txtCompletionDate.Text = "";
            chkReminder.Checked = false;
            drpReminderMonth.SelectedValue = "0";
            txtClosingRemarks.Text = "";
            btnSave.Disabled = false;
        }
        [System.Web.Services.WebMethod]
        public static string DeleteTODO(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ToDoMgmt.DeleteToDo(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpOption.SelectedValue == "reassign")
            {
                divTransfer.Style.Add("display", "block");
                divCompDate.Style.Add("display", "none");
            }
            if (drpOption.SelectedValue == "complete")
            {
                divTransfer.Style.Add("display", "none");
                divCompDate.Style.Add("display", "block");
            }
            if (drpOption.SelectedValue == "activity")
            {
                divTransfer.Style.Add("display", "none");
                divCompDate.Style.Add("display", "none");
            }
        }


        protected void drpTransferTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCompletionDate.Text = "";
        }

        protected void rptToDOLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlTableRow trRemarks = ((HtmlTableRow)e.Item.FindControl("trRemarks"));
                HiddenField hdnClosingRemarks = ((HiddenField)e.Item.FindControl("hdnClosingRemarks"));
                if (String.IsNullOrEmpty(hdnClosingRemarks.Value))
                {
                    trRemarks.Visible = false;
                }
            }
        }
    }
}