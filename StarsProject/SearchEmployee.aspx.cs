using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Net.Mail;

namespace StarsProject
{
    public partial class SearchEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindLeaveMonthYear();
                BindPayrollMonthYear();
                BindAttendanceMonthYear();
                BindExpnMonthYear();
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
                if (requestTarget.ToLower() == "txtemployeename")
                {
                    if (!String.IsNullOrEmpty(hdnEmpID.Value))
                        BindEmployee();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:setEditor();", true);
            }
        }
        public void BindEmployee()
        {
            int TotalCount = 0;
            // ----------------------------------------------------
            List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
            if (!String.IsNullOrEmpty(hdnEmpID.Value))
            {
                lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(hdnEmpID.Value), 1, 10, out TotalCount);
                // ----------------------------------------------------
                lblOrgName.Text = lstEntity[0].OrgName;
                lblDesignation.Text = lstEntity[0].Designation;
                lblReportTo.Text = lstEntity[0].ReportToEmployeeName;
                lblJoiningDate.Text = lstEntity[0].JoiningDate != SqlDateTime.MinValue.Value && (lstEntity[0].JoiningDate).Year > 1900 ? lstEntity[0].JoiningDate.ToShortDateString() : "";
                lblConfirmationDate.Text = lstEntity[0].ConfirmationDate != SqlDateTime.MinValue.Value && (lstEntity[0].ConfirmationDate).Year > 1900 ? lstEntity[0].ConfirmationDate.ToShortDateString() : "";
                lblMobile.Text = String.Concat(lstEntity[0].MobileNo, (!String.IsNullOrEmpty(lstEntity[0].Landline) ? ", " + lstEntity[0].Landline : ""));
                lblEmail.Text = lstEntity[0].EmailAddress;
                lblBirthDate.Text = lstEntity[0].BirthDate != SqlDateTime.MinValue.Value && (lstEntity[0].BirthDate).Year > 1900 ? lstEntity[0].BirthDate.ToShortDateString() : "";
                lblSignatory.Text = lstEntity[0].AuthorizedSign;
                if (lstEntity[0].EmployeeImage != "")
                {
                    lblImage.ImageUrl = lstEntity[0].EmployeeImage;
                }
                else
                {
                    lblImage.ImageUrl = "images/customer.png";
                }
                // ----------------------------------------------------------
                txtTo.Text = lstEntity[0].EmailAddress;
                string[] tmpAry = BAL.CommonMgmt.GetHREmailAddress().Split(',');
                if (tmpAry.Length>0)
                {
                    txtFrom.Text = tmpAry[0].ToString();
                    txtBCC.Text = tmpAry[0].ToString();
                    hdnPass.Value = tmpAry[1].ToString();
                }
                // ----------------------------------------------------------
                BindLeaveRequest();
                BindToDoTask();
                BindPayroll();
                BindAttendance();
                BindNotification();
            }
        }
        public void BindLeaveMonthYear()
        {
            // -----------------------------------------------------------------
            drpLeaveMonth.Items.Add(new ListItem("January", "1"));
            drpLeaveMonth.Items.Add(new ListItem("February", "2"));
            drpLeaveMonth.Items.Add(new ListItem("March", "3"));
            drpLeaveMonth.Items.Add(new ListItem("April", "4"));
            drpLeaveMonth.Items.Add(new ListItem("May", "5"));
            drpLeaveMonth.Items.Add(new ListItem("June", "6"));
            drpLeaveMonth.Items.Add(new ListItem("July", "7"));
            drpLeaveMonth.Items.Add(new ListItem("August", "8"));
            drpLeaveMonth.Items.Add(new ListItem("September", "9"));
            drpLeaveMonth.Items.Add(new ListItem("October", "10"));
            drpLeaveMonth.Items.Add(new ListItem("November", "11"));
            drpLeaveMonth.Items.Add(new ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpLeaveYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            // -----------------------------------------------------------------
            drpLeaveYear.SelectedValue = DateTime.Now.Year.ToString();
            drpLeaveMonth.SelectedValue = DateTime.Now.Month.ToString();
        }
        public void BindLeaveRequest()
        {
            Int64 pMon = 0, pYear = 0;

            if (!String.IsNullOrEmpty(drpLeaveMonth.SelectedValue))
                pMon = Convert.ToInt64(drpLeaveMonth.SelectedValue);

            if (!String.IsNullOrEmpty(drpLeaveYear.SelectedValue))
                pYear = Convert.ToInt64(drpLeaveYear.SelectedValue);

            if (!String.IsNullOrEmpty(hdnEmpID.Value))
            rptLeave.DataSource = BAL.LeaveRequestMgmt.GetLeaveRequestListByEmployeeID(Convert.ToInt64(hdnEmpID.Value), pMon, pYear);
            rptLeave.DataBind();
        }
        protected void drpLeaveMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindLeaveRequest();
        }
        public void BindToDoTask()
        {
            int TotalCount;
            List<Entity.ToDo> lstEntity1 = new List<Entity.ToDo>();
            lstEntity1 = BAL.ToDoMgmt.GetToDoList(0, Session["LoginUserID"].ToString(), hdnEmpID.Value, Convert.ToInt32(1), Convert.ToInt32(Session["PageSize"]), out TotalCount);
            rpttodotask.DataSource = lstEntity1;
            rpttodotask.DataBind();
        }
        public void BindPayrollMonthYear()
        {
            // -----------------------------------------------------------------
            drpPayrollMonth.Items.Add(new ListItem("January", "1"));
            drpPayrollMonth.Items.Add(new ListItem("February", "2"));
            drpPayrollMonth.Items.Add(new ListItem("March", "3"));
            drpPayrollMonth.Items.Add(new ListItem("April", "4"));
            drpPayrollMonth.Items.Add(new ListItem("May", "5"));
            drpPayrollMonth.Items.Add(new ListItem("June", "6"));
            drpPayrollMonth.Items.Add(new ListItem("July", "7"));
            drpPayrollMonth.Items.Add(new ListItem("August", "8"));
            drpPayrollMonth.Items.Add(new ListItem("September", "9"));
            drpPayrollMonth.Items.Add(new ListItem("October", "10"));
            drpPayrollMonth.Items.Add(new ListItem("November", "11"));
            drpPayrollMonth.Items.Add(new ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpPayrollYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            // -----------------------------------------------------------------
            drpPayrollYear.SelectedValue = DateTime.Now.Year.ToString();
            drpPayrollMonth.SelectedValue = DateTime.Now.Month.ToString();
        }
        public void BindPayroll()
        {
            int TotalRecord = 0;
            Int64 pMon = 0, pYear = 0;

            if (!String.IsNullOrEmpty(drpPayrollMonth.SelectedValue))
                pMon = Convert.ToInt64(drpPayrollMonth.SelectedValue);

            if (!String.IsNullOrEmpty(drpPayrollYear.SelectedValue))
                pYear = Convert.ToInt64(drpPayrollYear.SelectedValue);
            // -------------------------------------------------------------
            List<Entity.Payroll> lstPayroll = new List<Entity.Payroll>();
            lstPayroll = BAL.PayrollMgmt.GetPayrollList(0, pMon, pYear, 1, 999999, out TotalRecord);
            rptPayroll.DataSource = lstPayroll;
            rptPayroll.DataBind();
        }
        protected void drpBindPayrollMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPayroll();
        }
        public void BindAttendanceMonthYear()
        {
            // -----------------------------------------------------------------
            drpAttendanceMonth.Items.Add(new ListItem("January", "1"));
            drpAttendanceMonth.Items.Add(new ListItem("February", "2"));
            drpAttendanceMonth.Items.Add(new ListItem("March", "3"));
            drpAttendanceMonth.Items.Add(new ListItem("April", "4"));
            drpAttendanceMonth.Items.Add(new ListItem("May", "5"));
            drpAttendanceMonth.Items.Add(new ListItem("June", "6"));
            drpAttendanceMonth.Items.Add(new ListItem("July", "7"));
            drpAttendanceMonth.Items.Add(new ListItem("August", "8"));
            drpAttendanceMonth.Items.Add(new ListItem("September", "9"));
            drpAttendanceMonth.Items.Add(new ListItem("October", "10"));
            drpAttendanceMonth.Items.Add(new ListItem("November", "11"));
            drpAttendanceMonth.Items.Add(new ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpAttendanceYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            // -----------------------------------------------------------------
            drpAttendanceYear.SelectedValue = DateTime.Now.Year.ToString();
            drpAttendanceMonth.SelectedValue = DateTime.Now.Month.ToString();
        }
        protected void drpAttendanceMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAttendance();
        }
        public void BindAttendance()
        {
            Int64 pMon = 0, pYear = 0;

            if (!String.IsNullOrEmpty(drpAttendanceMonth.SelectedValue))
                pMon = Convert.ToInt64(drpAttendanceMonth.SelectedValue);

            if (!String.IsNullOrEmpty(drpAttendanceYear.SelectedValue))
                pYear = Convert.ToInt64(drpAttendanceYear.SelectedValue);

            if (!String.IsNullOrEmpty(hdnEmpID.Value))
            {
                List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
                lstEntity = BAL.AttendanceMgmt.GetAttendanceList(0, Convert.ToInt64(hdnEmpID.Value), pMon, pYear);
                rptAttendance.DataSource = lstEntity;
                rptAttendance.DataBind();
            }
            else
            {
                List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
                lstEntity = BAL.AttendanceMgmt.GetAttendanceList(-1, Convert.ToInt64(hdnEmpID.Value), pMon, pYear);
                rptAttendance.DataSource = lstEntity;
                rptAttendance.DataBind();
            }
        }
        protected void rptAttendance_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnWorkingTotalHrs = ((HiddenField)e.Item.FindControl("hdnWorkingTotalHrs"));
                HiddenField hdnWorkingHrsFlag = ((HiddenField)e.Item.FindControl("hdnWorkingHrsFlag"));
                HiddenField hdnDayStatus = ((HiddenField)e.Item.FindControl("hdnDayStatus"));

                HtmlTableRow trItem = ((HtmlTableRow)e.Item.FindControl("trItem"));
                HtmlTableCell tdWorkingHrsFlag = ((HtmlTableCell)e.Item.FindControl("tdWorkingHrsFlag"));
                HtmlTableCell tdDayStatus = ((HtmlTableCell)e.Item.FindControl("tdDayStatus"));

                // ------------------------------------------------
                Decimal tmpFlagValue = 0;
                tmpFlagValue = Convert.ToDecimal(hdnWorkingHrsFlag.Value);
                if (tmpFlagValue == Convert.ToDecimal(0.00))
                {
                    if (hdnDayStatus.Value.ToLower() == "wo")
                    {
                        tdWorkingHrsFlag.InnerHtml = "Week Off";
                        trItem.Style.Add("background-color", "red");
                        trItem.Style.Add("color", "white");
                    }
                    else if (hdnDayStatus.Value.ToLower() == "hl")
                    {
                        tdWorkingHrsFlag.InnerHtml = "Holiday";
                        trItem.Style.Add("background-color", "#cd5c5c");
                        trItem.Style.Add("color", "white");
                    }
                    else
                    {
                        tdWorkingHrsFlag.InnerHtml = "<i class='material-icons font-size-small red-text'>brightness_1</i>&nbsp;Absent";
                        trItem.Style.Add("background-color", "red");
                        trItem.Style.Add("color", "white");
                    }
                }
                else
                {
                    if (tmpFlagValue > Convert.ToDecimal(0.00) && tmpFlagValue <= Convert.ToDecimal(0.5))
                    {
                        tdWorkingHrsFlag.InnerHtml = "<i class='material-icons font-size-small navy-text'>brightness_6</i>&nbsp;Half Day";
                        tdWorkingHrsFlag.Style.Add("color", "navy");
                    }
                    else
                    {
                        tdWorkingHrsFlag.InnerHtml = "<i class='material-icons font-size-small green-text'>brightness_5</i>&nbsp;Full Day";
                        tdWorkingHrsFlag.Style.Add("color", "green");
                    }
                }
                // ------------------------------------------------
                if (hdnDayStatus.Value.ToLower() == "rd")
                {
                    tdDayStatus.InnerText = "Regular";
                    tdDayStatus.Style.Add("color", "navy");
                }
                else if (hdnDayStatus.Value.ToLower() == "wo")
                {
                    tdDayStatus.InnerText = "Week Off";
                    tdDayStatus.Style.Add("color", "darkgreen");
                }
                else if (hdnDayStatus.Value.ToLower() == "hl")
                {
                    tdDayStatus.InnerText = "Holiday";
                    tdDayStatus.Style.Add("color", "darkorange");
                }
            }
        }

        public void BindNotification()
        {
            int totrec;
            rptNotification.DataSource = BAL.EmailTemplateMgmt.GetEmailTemplate("","HR", 1, 10000, out totrec);
            rptNotification.DataBind();
        }
        protected void rptNotification_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "SendNotification")
                {
                    int totrec = 0;
                    List<Entity.EmailTemplate> lstTemplate = new List<Entity.EmailTemplate>();
                    lstTemplate = BAL.EmailTemplateMgmt.GetEmailTemplate(e.CommandArgument.ToString(), "HR", 1, 9999, out totrec);
                    if (lstTemplate.Count>0)
                    {
                        hdnTemplateID.Value = e.CommandArgument.ToString();
                        txtSubject.Text = lstTemplate[0].Subject;
                        txtEditor.Text = HttpUtility.HtmlDecode(lstTemplate[0].ContentData);
                    }
                }
            }
        }

        public void BindExpnMonthYear()
        {
            // -----------------------------------------------------------------
            drpExpnMonth.Items.Add(new ListItem("January", "1"));
            drpExpnMonth.Items.Add(new ListItem("February", "2"));
            drpExpnMonth.Items.Add(new ListItem("March", "3"));
            drpExpnMonth.Items.Add(new ListItem("April", "4"));
            drpExpnMonth.Items.Add(new ListItem("May", "5"));
            drpExpnMonth.Items.Add(new ListItem("June", "6"));
            drpExpnMonth.Items.Add(new ListItem("July", "7"));
            drpExpnMonth.Items.Add(new ListItem("August", "8"));
            drpExpnMonth.Items.Add(new ListItem("September", "9"));
            drpExpnMonth.Items.Add(new ListItem("October", "10"));
            drpExpnMonth.Items.Add(new ListItem("November", "11"));
            drpExpnMonth.Items.Add(new ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpExpnYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            // -----------------------------------------------------------------
            drpExpnMonth.SelectedValue = DateTime.Now.Year.ToString();
            drpExpnMonth.SelectedValue = DateTime.Now.Month.ToString();
        }

        public void BindExpenseLedger()
        {
            Int64 pMon = 0, pYear = 0;

            pMon = (!String.IsNullOrEmpty(drpExpnMonth.SelectedValue)) ? Convert.ToInt64(drpExpnMonth.SelectedValue) : 0;
            pYear = (!String.IsNullOrEmpty(drpExpnYear.SelectedValue)) ? Convert.ToInt64(drpExpnYear.SelectedValue) : 0;

            if (!String.IsNullOrEmpty(hdnEmpID.Value))
            {
                myEmployeeExpnLedger.pageMonth = pMon.ToString();
                myEmployeeExpnLedger.pageYear = pYear.ToString();
                myEmployeeExpnLedger.EmployeeID = hdnEmpID.Value;
                myEmployeeExpnLedger.BindEmployeeExpnLedger();
                // ------------------------------------------------
                List<Entity.OrganizationEmployee> lstSummary = new List<Entity.OrganizationEmployee>();
                lstSummary = myEmployeeExpnLedger.BindEmployeeExpnLedgerSummary();
                var lstExpnSummary = lstSummary.Where(x => x.TransCategory == "Expense").GroupBy(x => x.TransType).Select(x => new { TransType = x.Key, CreditAmount = x.Sum(ta => ta.CreditAmount) }).ToList();

                String tmpStr = "<h5 class='gradient-45deg-red-pink white-text float-left'>Expense Summary</h5>";
                tmpStr += "<table>";
                foreach (var cRow in lstExpnSummary)
                {
                    tmpStr += "<tr>";
                    tmpStr += "<td>" + cRow.TransType + "</td>";
                    tmpStr += "<td>" + cRow.CreditAmount + "</td>";
                    tmpStr += "</tr>";
                }
                tmpStr += "</table>";
                divExpnSummary.InnerHtml = tmpStr;
            }

        }
        protected void drpExpnMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindExpenseLedger();

        }
        // -------------------------------------------------------------------------
        // Section : Notification Tab
        // -------------------------------------------------------------------------
        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            string body = string.Empty, strErr = "";

            string currEmpID = "", currUserID = "";
            currEmpID = (!String.IsNullOrEmpty(hdnEmpID.Value)) ? hdnEmpID.Value : "0";
            currUserID = BAL.CommonMgmt.GetUserIDByEmployeeID(Convert.ToInt64(currEmpID));

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
            lstCompany = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);

            List<Entity.OrganizationEmployee> lstSuper = new List<Entity.OrganizationEmployee>();
            lstSuper = BAL.OrganizationEmployeeMgmt.GetEmployeeSupervisorList(currUserID);
            // -----------------------------------------------------------------------------
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = lstCompany[0].Host; //  ConfigurationManager.AppSettings["Host"];
                    if (!String.IsNullOrEmpty(lstCompany[0].EnableSSL.ToString().ToLower()))
                        smtp.EnableSsl = lstCompany[0].EnableSSL;
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = Convert.ToInt16(lstCompany[0].PortNumber);
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    smtp.Credentials = NetworkCred;
                    NetworkCred.UserName = txtFrom.Text;
                    NetworkCred.Password = hdnPass.Value;

                    mailMessage.Subject = txtSubject.Text;
                    mailMessage.From = new MailAddress(txtFrom.Text);
                    mailMessage.To.Add(new MailAddress(lblEmail.Text));
                    for (int i = 0; i <= lstSuper.Count - 1; i++)
                    {
                        if (txtFrom.Text.ToLower() != lstSuper[i].EmailAddress.ToLower())
                            mailMessage.CC.Add(new MailAddress(lstSuper[i].EmailAddress));
                    }
                    mailMessage.Bcc.Add(new MailAddress(txtFrom.Text));
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    smtp.Send(mailMessage);
                }
                strErr = "Success";
            }
            catch (Exception ex)
            {
                string tmpMessage = "";
                tmpMessage = ex.Message.ToString();
                strErr = tmpMessage;
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);

        }
    }
}