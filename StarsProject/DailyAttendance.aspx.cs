using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class DailyAttendance : System.Web.UI.Page
    {
        public string objAuthEmployeeName;
        public Boolean _pageValid = true;
        public string strErr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)Session["logindetail"];

                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 100000;
                BindMonthYear();

                if (!String.IsNullOrEmpty(Request.QueryString["MenuID"]))
                    hdnMenuID.Value = Request.QueryString["MenuID"].ToString().Trim();
                else
                    hdnMenuID.Value = "0";
                // -----------------------------------------------------------------
                //  Add / Edit / Delete Flag 
                // -----------------------------------------------------------------
                List<Entity.ApplicationMenu> lstMenu = new List<Entity.ApplicationMenu>();
                lstMenu = BAL.CommonMgmt.GetMenuAddEditDelList(Convert.ToInt64(hdnMenuID.Value), Session["LoginUserID"].ToString());
                hdnAddFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].AddFlag.ToString().ToLower() : "true";
                hdnEditFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].EditFlag.ToString().ToLower() : "true";
                hdnDelFlag.Value = (lstMenu.Count > 0) ? lstMenu[0].DelFlag.ToString().ToLower() : "true";
                string strWeekOff = BAL.CommonMgmt.GetConstant("WeekOff", 0, objAuth.CompanyID);
                hdnWeekOff.Value = (!String.IsNullOrEmpty(strWeekOff)) ? strWeekOff : "7";
                // ----------------------------------------------------------
                
                objAuthEmployeeName = objAuth.EmployeeName;
                hdnEmployeeID.Value = objAuth.EmployeeID.ToString();
                hdnLoginUserID.Value = objAuth.UserID;
                hdnRole.Value = objAuth.RoleCode;
                drpEmployee.SelectedValue = hdnEmployeeID.Value;
                // ----------------------------------------------------------
                BindDropDown();
                // ----------------------------------------------------------
                BindAttendanceSummary();
                // ----------------------------------------------------------
                divEmployee.Visible = (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin") ? false : true;

                if (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                {
                    BindCalendar(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
                }
                else
                {
                    BindCalendar(Convert.ToInt64(hdnEmployeeID.Value), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
                }
                // ---------------------------------------------
                hdnBiometricAttendance.Value = BAL.CommonMgmt.GetConstant("BiometricAttendance", 0, objAuth.CompanyID).ToLower();
                ImgbtnAttendence.Visible = (!String.IsNullOrEmpty(hdnBiometricAttendance.Value) && hdnBiometricAttendance.Value == "yes") ? true : false;
            }
            // ----------------------------------------
            divUpload1.Visible = (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin") ? false : true;
            //divUpload2.Visible = (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin") ? false : true;
        }


        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpSummaryMonth.Items.Add(new ListItem("January", "1"));
            drpSummaryMonth.Items.Add(new ListItem("February", "2"));
            drpSummaryMonth.Items.Add(new ListItem("March", "3"));
            drpSummaryMonth.Items.Add(new ListItem("April", "4"));
            drpSummaryMonth.Items.Add(new ListItem("May", "5"));
            drpSummaryMonth.Items.Add(new ListItem("June", "6"));
            drpSummaryMonth.Items.Add(new ListItem("July", "7"));
            drpSummaryMonth.Items.Add(new ListItem("August", "8"));
            drpSummaryMonth.Items.Add(new ListItem("September", "9"));
            drpSummaryMonth.Items.Add(new ListItem("October", "10"));
            drpSummaryMonth.Items.Add(new ListItem("November", "11"));
            drpSummaryMonth.Items.Add(new ListItem("December", "12"));
            drpSummaryMonth.SelectedValue = DateTime.Now.Month.ToString();
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpSummaryYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            drpSummaryYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        public void BindDropDown()
        {
            int totrec;
            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            if (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(hdnLoginUserID.Value);
            else
                lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), 1, 9999, out totrec);

            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
        }

        public void BindCalendar(Int64 pEmpID, Int64 pMonth, Int64 pYear)
        {
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            CleanUpCalendar();
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            String ctrlField = "";
            DateTime tmpDate;
            DateTime dt;
            int cntWeekOff = 0;
            // -----------------------------------------------------------
            if (!String.IsNullOrEmpty(drpSummaryMonth.SelectedValue) && drpSummaryMonth.SelectedValue != "0" && !String.IsNullOrEmpty(drpSummaryYear.SelectedValue))
                dt = Convert.ToDateTime(drpSummaryYear.SelectedValue + "-" + drpSummaryMonth.SelectedValue + "-01"); // Selected Month & Year
            else
                dt = DateTime.Now; // System Month & Year
            // -----------------------------------------------------------
            DateTime startDate = new DateTime(dt.Year, dt.Month, 1);    // First Date of the month
            DateTime todayDate = Convert.ToDateTime(DateTime.Now);    //      Today
            DateTime lastDate = startDate.AddMonths(1).AddDays(-1);     // Last Date of the month
            tmpDate = startDate;
            int startPos = (tmpDate.DayOfWeek == 0) ? 7 : Convert.ToInt16(tmpDate.DayOfWeek);
            int endPos = (tmpDate.DayOfWeek == 0) ? Convert.ToInt16((7 + lastDate.Day) - 1) : Convert.ToInt16((tmpDate.DayOfWeek + lastDate.Day) - 1);
            // ------------------------------------------------------------------------------
            List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
            lstEntity = BAL.AttendanceMgmt.GetAttendanceList(0, pEmpID, pMonth, pYear);
            // ------------------------------------------------------------------------------
            List<Entity.LeaveRequest> lstLeave = new List<Entity.LeaveRequest>();
            lstLeave = BAL.LeaveRequestMgmt.GetLeaveRequestListByEmployeeID(Convert.ToInt64(drpEmployee.SelectedValue), pMonth, pYear);
            // ------------------------------------------------------------------------------
            List<Entity.Holiday> lstHoliday = new List<Entity.Holiday>();
            lstHoliday = BAL.HolidayMgmt.GetHolidayList();
            // ------------------------------------------------------------------------------
            for (int i = 1; i <= 42; i++)
            {
                if (i < startPos || i >= (startPos + lastDate.Day))
                {
                    ((TextBox)tblAttendance.FindControl("txtIDate" + i.ToString())).Visible = false;
                    ((TextBox)tblAttendance.FindControl("txtODate" + i.ToString())).Visible = false;
                    //((TextBox)tblAttendance.FindControl("Label" + i.ToString())).Visible = false;
                    ((HtmlGenericControl)tblAttendance.FindControl("kanban" + i.ToString())).Visible = false;
                    ((HtmlGenericControl)tblAttendance.FindControl("divFlag" + i.ToString())).Visible = false;
                    //"<i class='material-icons font-size-small red-text'>brightness_1</i>"
                }
                else
                {
                    if (i >= startPos && i <= endPos)
                    {
                        ctrlField = "lblDate" + (i);
                        Label lblControl = ((Label)tblAttendance.FindControl(ctrlField));
                        if (lblControl != null)
                            lblControl.Text = tmpDate.ToString("dd");
                        TextBox txtIn = ((TextBox)tblAttendance.FindControl("txtIDate" + i.ToString()));
                        TextBox txtOut = ((TextBox)tblAttendance.FindControl("txtODate" + i.ToString()));
                        TextBox txtNotes = ((TextBox)tblAttendance.FindControl("Label" + i.ToString()));
                        HtmlGenericControl dvkan = ((HtmlGenericControl)tblAttendance.FindControl("Kanban" + i.ToString()));
                        dvkan.Visible = true;
                        HtmlGenericControl dvFlag = ((HtmlGenericControl)tblAttendance.FindControl("divFlag" + i.ToString()));
                        dvFlag.Visible = true;
                        // ---------------------------------
                        if (lstEntity.Count > 0)
                        {
                            Entity.Attendance obj = lstEntity.Where(x => x.PresenceDate.Day == tmpDate.Day).FirstOrDefault();
                            if (obj != null)
                            {
                                txtNotes.Text = obj.Notes;
                                if (!String.IsNullOrEmpty(obj.Notes))
                                {
                                    dvkan.InnerHtml = "<i class='material-icons font-size-small green-text'>chat_bubble</i>";
                                }
                                txtIn.Text = obj.TimeIn;
                                txtOut.Text = obj.TimeOut;
                                if (!String.IsNullOrEmpty(obj.TimeIn))
                                    txtIn.ForeColor = (Convert.ToDateTime(obj.TimeIn) > Convert.ToDateTime("10:15 AM")) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                                // ----------------------------------------
                                if (obj.WorkingHrsFlag == 0)
                                    dvFlag.InnerHtml = "<i class='material-icons font-size-small red-text'>brightness_1</i>";
                                else if (obj.WorkingHrsFlag == Convert.ToDecimal(0.50))
                                    dvFlag.InnerHtml = "<i class='material-icons font-size-small green-text'>brightness_6</i>";
                                else if (obj.WorkingHrsFlag == Convert.ToDecimal(1.00))
                                    dvFlag.InnerHtml = "<i class='material-icons font-size-small green-text'>brightness_5</i>";
                            }
                        }
                        if (Session["RoleCode"].ToString() != "admin" && Session["RoleCode"].ToString() != "bradmin" && Session["RoleCode"].ToString() != "hradmin")
                        {
                            txtIn.Enabled = (!String.IsNullOrEmpty(txtIn.Text)) ? false : true;
                            txtOut.Enabled = (!String.IsNullOrEmpty(txtOut.Text)) ? false : true;
                            if (i >= (startPos + todayDate.Day) || pMonth > todayDate.Month || pYear > todayDate.Year)
                            {
                                txtIn.Enabled = false;
                                txtOut.Enabled = false;
                            }
                        }

                        // ------------------------------------------------------
                        // Disable & Lock ... Old Dates
                        // ------------------------------------------------------
                        // && tmpDate.DayOfWeek != DayOfWeek.Sunday
                        if (tmpDate < DateTime.Now.Date && hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                        {
                            if (!txtIn.CssClass.Contains("diagonalStrips"))
                            {
                                txtIn.CssClass = "timeInputDisable";
                                txtOut.CssClass = "timeInputDisable";
                            }
                            txtIn.Enabled = false;
                            txtOut.Enabled = false;
                        }
                        // ------------------------------------------------------
                        // Finding for Leave Request 
                        // ------------------------------------------------------
                        if (lstLeave.Count > 0)
                        {
                            Entity.LeaveRequest obj = lstLeave.Where(x => (tmpDate >= x.FromDate && tmpDate <= x.ToDate)).FirstOrDefault();
                            if (obj != null)
                            {
                                txtIn.Text = "-- Leave --";
                                txtOut.Text = "-- Leave --";
                                txtIn.Font.Bold = true;
                                txtOut.Font.Bold = true;
                                txtIn.Font.Size = 14;
                                txtOut.Font.Size = 14;
                                txtIn.ForeColor = System.Drawing.Color.Maroon;
                                txtOut.ForeColor = System.Drawing.Color.Maroon;
                                txtIn.Enabled = false;
                                txtOut.Enabled = false;
                                //if (tmpDate.DayOfWeek != DayOfWeek.Sunday)
                                if (tmpDate.DayOfWeek.ToString() != hdnWeekOff.Value)
                                {
                                    if (!txtIn.CssClass.Contains("diagonalStrips"))
                                    {
                                        txtIn.CssClass = "timeLeaveRequest";
                                        txtOut.CssClass = "timeLeaveRequest";
                                    }
                                    txtIn.Enabled = false;
                                    txtOut.Enabled = false;
                                }
                            }
                        }
                        // ------------------------------------------------------
                        // Finding for WeekOff
                        // ------------------------------------------------------
                        //Entity.Authenticate objAuth = new Entity.Authenticate();
                        //objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                        //string strWeekOff = BAL.CommonMgmt.GetConstant("WeekOff", 0, objAuth.CompanyID);
                        //int intWeekOff = (!String.IsNullOrEmpty(strWeekOff)) ? Convert.ToInt16(strWeekOff) : 6;

                        //if (i == ((intWeekOff*1)-1) || i == ((intWeekOff * 2) - 1) || i == ((intWeekOff * 3) - 1) || 
                        //          i == ((intWeekOff * 4) - 1) || i == ((intWeekOff * 5) - 1) || i == ((intWeekOff * 6) - 1))
                        //{
                        //    cntWeekOff += 1;
                        //    if (strWeekOff.IndexOf(cntWeekOff.ToString()) >= 0)
                        //    {
                        //        txtIn.CssClass = "timepicker diagonalStrips";
                        //        txtOut.CssClass = "timepicker diagonalStrips";
                        //    }
                        //}
                        // ------------------------------------------------------
                        // Finding for Holidays
                        // ------------------------------------------------------
                        if (lstHoliday.Count > 0)
                        {
                            Entity.Holiday objHoliday = lstHoliday.Where(x => (tmpDate == x.Holiday_Date)).FirstOrDefault();
                            if (objHoliday != null)
                            {
                                Entity.Attendance obj = lstEntity.Where(x => x.PresenceDate.Day == tmpDate.Day).FirstOrDefault();
                                if (obj != null)
                                {
                                    txtIn.Text = obj.TimeIn;
                                    txtOut.Text = obj.TimeOut;
                                    //if (!String.IsNullOrEmpty(obj.TimeIn))
                                    //    txtIn.ForeColor = (Convert.ToDateTime(obj.TimeIn) > Convert.ToDateTime("10:15 AM")) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                                }
                                //else
                                //{
                                //    txtIn.Text = objHoliday.Holiday_Name;
                                //    txtOut.Text = objHoliday.Holiday_Name;
                                //}
                                //txtIn.Font.Bold = true;
                                //txtOut.Font.Bold = true;
                                //txtIn.Font.Size = 10;
                                //txtOut.Font.Size = 10;
                                //txtIn.BackColor = System.Drawing.Color.OrangeRed;
                                //txtOut.BackColor = System.Drawing.Color.OrangeRed;
                                //txtIn.ForeColor = System.Drawing.Color.White;
                                //txtOut.ForeColor = System.Drawing.Color.White;
                                //txtIn.Enabled = false;
                                //txtOut.Enabled = false;
                                //if (tmpDate.DayOfWeek != DayOfWeek.Sunday)
                                //{
                                if (txtIn.CssClass.Contains("diagonalStrips"))
                                {
                                    txtIn.CssClass = txtIn.CssClass.Replace("diagonalStrips", "");
                                    txtOut.CssClass = txtIn.CssClass.Replace("diagonalStrips", "");
                                }
                                txtIn.CssClass += " timeHoliday";
                                txtOut.CssClass += " timeHoliday";
                                //txtIn.Enabled = false;
                                //txtOut.Enabled = false;
                                //}
                            }
                        }

                        // ---------------------------------
                        tmpDate = tmpDate.AddDays(1);
                    }

                }
            }
        }

        public void CleanUpCalendar()
        {
            for (int i = 1; i <= 42; i++)
            {
                Label dt = ((Label)tblAttendance.FindControl("lblDate" + i.ToString()));
                TextBox t1 = ((TextBox)tblAttendance.FindControl("txtIDate" + i.ToString()));
                TextBox t2 = ((TextBox)tblAttendance.FindControl("txtODate" + i.ToString()));
                TextBox t3 = ((TextBox)tblAttendance.FindControl("Label" + i.ToString()));
                // ---------------------------------------------------------
                HtmlGenericControl dvkan = ((HtmlGenericControl)tblAttendance.FindControl("Kanban" + i.ToString()));
                dvkan.InnerHtml = "<i class='material-icons font-size-small green-text'>chat_bubble_outline</i>";
                // ---------------------------------------------------------
                HtmlGenericControl dvFlag = ((HtmlGenericControl)tblAttendance.FindControl("divFlag" + i.ToString()));
                dvFlag.InnerHtml = "<i class='material-icons font-size-small red-text'>brightness_1</i>";

                t1.Visible = true;
                t2.Visible = true;
                t3.Visible = true;
                dvkan.Visible = false;
                dvFlag.Visible = false;
                t1.Font.Bold = false;
                t2.Font.Bold = false;
                t1.Font.Size = 10;
                t2.Font.Size = 10;
                t1.ForeColor = System.Drawing.Color.Black;
                t2.ForeColor = System.Drawing.Color.Black;

                dt.Text = "";
                t1.Text = "";
                t2.Text = "";
                t3.Text = "";

                t1.CssClass = "timepicker";
                t2.CssClass = "timepicker";

                //t1.CssClass = (i == 7 || i == 14 || i == 21 || i == 28 || i == 35) ? "timepicker diagonalStrips" : "timepicker";
                //t2.CssClass = (i == 7 || i == 14 || i == 21 || i == 28 || i == 35) ? "timepicker diagonalStrips" : "timepicker";

                // ------------------------------------------------------
                // Finding for WeekOff
                // ------------------------------------------------------
                int intWeekOff = (!String.IsNullOrEmpty(hdnWeekOff.Value)) ? Convert.ToInt16(hdnWeekOff.Value) : 7;

                if (i == intWeekOff || i == (intWeekOff + 7) || i == (intWeekOff + 14) ||
                          i == (intWeekOff + 21) || i == (intWeekOff + 28) || i == (intWeekOff + 35))
                {
                    t1.CssClass = "timepicker diagonalStrips";
                    t2.CssClass = "timepicker diagonalStrips";
                }

                t1.Enabled = true;
                t2.Enabled = true;
                t3.Enabled = true;
            }
        }

        protected void drpSummaryMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                BindCalendar(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
            else
                BindCalendar(Convert.ToInt64(hdnEmployeeID.Value), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int ReturnCode;
            string ReturnMsg;
            _pageValid = true;
            // -------------------------------------------
            for (int i = 1; i <= 42; i++)
            {
                Label dt = ((Label)tblAttendance.FindControl("lblDate" + i.ToString()));
                TextBox t1 = ((TextBox)tblAttendance.FindControl("txtIDate" + i.ToString().ToUpper()));
                TextBox t2 = ((TextBox)tblAttendance.FindControl("txtODate" + i.ToString().ToUpper()));

                if (!String.IsNullOrEmpty(t1.Text) && !String.IsNullOrEmpty(t2.Text))
                {
                    try
                    {
                        // This is ******* (Waste) .... Not at all acceptable 
                        //t1.Text = Convert.ToDateTime(t1.Text).ToString("dd-MMM-yyyy");
                        //t2.Text = Convert.ToDateTime(t2.Text).ToString("dd-MMM-yyyy");

                        String tmpDD = drpSummaryYear.SelectedValue + "-" + drpSummaryMonth.SelectedValue + "-" + dt.Text;

                        if (Convert.ToDateTime(tmpDD + " " + t1.Text) >= Convert.ToDateTime(tmpDD + " " + t2.Text))
                        {
                            _pageValid = false;
                            strErr = "Invalid Time ! Time-OUT is must be greater then Time-IN";
                        }

                    }
                    catch (Exception)
                    {
                        if (t1.Text != t2.Text)
                        {
                            _pageValid = false;
                            strErr = "Invalid Time ! Time-OUT is must be greater then Time-IN";
                        }
                        //else
                        //{
                        //    t1.Text = "";
                        //    t2.Text = "";
                        //}
                    }
                }

                //if (!String.IsNullOrEmpty(hdnEmployeeID.Value) && hdnEmployeeID.Value != "0" && !String.IsNullOrEmpty(dt.Text) && t1.CssClass != "timeHoliday" && !String.IsNullOrEmpty(t1.Text) && !String.IsNullOrEmpty(t2.Text))
                //{
                //    try
                //    {
                //        t1.Text = Convert.ToDateTime(t1.Text).ToString("dd-MMM-yyyy");
                //    }
                //    catch (Exception eee)
                //    {
                //    }
                //    try
                //    {
                //        t2.Text = Convert.ToDateTime(t2.Text).ToString("dd-MMM-yyyy");
                //    }
                //    catch (Exception eee)
                //    {}
                //    if (Convert.ToDateTime(tmpDD + " " + t1.Text) >= Convert.ToDateTime(tmpDD + " " + t2.Text))
                //    {
                //        _pageValid = false;
                //        strErr = "Invalid Time ! Time-OUT is must be greater then Time-IN";
                //    }
                //}
            }
            // -------------------------------------------------------------------
            if (_pageValid)
            {
                for (int i = 1; i <= 42; i++)
                {
                    Label dt = ((Label)tblAttendance.FindControl("lblDate" + i.ToString()));
                    TextBox t1 = ((TextBox)tblAttendance.FindControl("txtIDate" + i.ToString()));
                    TextBox t2 = ((TextBox)tblAttendance.FindControl("txtODate" + i.ToString()));
                    TextBox notes = ((TextBox)tblAttendance.FindControl("Label" + i.ToString()));
                    // -------------------------------------------------------------- 

                    if (!String.IsNullOrEmpty(hdnEmployeeID.Value) && hdnEmployeeID.Value != "0" && !String.IsNullOrEmpty(dt.Text) && t1.CssClass != "timeHoliday")
                    {
                        String tmpDD = drpSummaryYear.SelectedValue + "-" + drpSummaryMonth.SelectedValue + "-" + dt.Text;
                        Entity.Attendance objEntity = new Entity.Attendance();
                        objEntity.pkID = 0;
                        objEntity.EmployeeID = (!String.IsNullOrEmpty(hdnEmployeeID.Value)) ? Convert.ToInt64(hdnEmployeeID.Value) : 0;
                        objEntity.PresenceDate = Convert.ToDateTime(tmpDD);
                        objEntity.TimeIn = (t1.Text == "-- Leave --" || (t1.Text.ToUpper().IndexOf("AM") < 0 && t1.Text.ToUpper().IndexOf("PM") < 0)) ? "" : t1.Text;
                        objEntity.TimeOut = (t2.Text == "-- Leave --" || (t2.Text.ToUpper().IndexOf("AM") < 0 && t2.Text.ToUpper().IndexOf("PM") < 0)) ? "" : t2.Text;
                        objEntity.Notes = notes.Text;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- 
                        BAL.AttendanceMgmt.AddUpdateAttendance(objEntity, out ReturnCode, out ReturnMsg);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }

            // -------------------------------------------------
            if (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                BindCalendar(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
            else
                BindCalendar(Convert.ToInt64(hdnEmployeeID.Value), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
        }

        protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnEmployeeID.Value = drpEmployee.SelectedValue;
            // ---------------------------------------------------------------
            BindAttendanceSummary();
            // ---------------------------------------------------------------
            if (hdnRole.Value.ToLower() != "admin" && hdnRole.Value.ToLower() != "bradmin" && hdnRole.Value.ToLower() != "hradmin")
                BindCalendar(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));
            else
                BindCalendar(Convert.ToInt64(hdnEmployeeID.Value), Convert.ToInt64(drpSummaryMonth.SelectedValue), Convert.ToInt64(drpSummaryYear.SelectedValue));

        }
        public void BindAttendanceSummary()
        {
            // -----------------------------------------------------------------------------------
            //List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            //lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(hdnEmployeeID.Value), 1, 9999, out TotalCount);
            //if (lstEmployee.Count > 0)
            //{
            //    lblEmployeeName.Text = lstEmployee[0].EmployeeName;
            //    lblShift.Text = lstEmployee[0].ShiftName;
            //    List<Entity.ShiftMaster> lstShift = new List<Entity.ShiftMaster>();
            //    lstShift = BAL.ShiftMasterMgmt.GetShiftMaster(Convert.ToInt64(lstEmployee[0].ShiftCode), Session["LoginUserID"].ToString(), 1, 9999, out TotalCount);
            //    if (lstShift.Count > 0)
            //    {
            //        lblShiftMinHrs.Text = lstShift[0].MinHrsHalfDay.ToString();
            //        lblShiftMaxHrs.Text = lstShift[0].MinHrsFullDay.ToString();
            //        lblGraceMins.Text = Math.Abs(lstShift[0].GraceMins).ToString();
            //    }
            //}
            // -----------------------------------------------------------------------------------
            Int64 pEmployeeID = 0, pMonth = 0, pYear = 0;
            pEmployeeID = (!String.IsNullOrEmpty(drpEmployee.SelectedValue)) ? Convert.ToInt64(drpEmployee.SelectedValue) : 0;
            pMonth = (!String.IsNullOrEmpty(drpSummaryMonth.SelectedValue)) ? Convert.ToInt64(drpSummaryMonth.SelectedValue) : 0;
            pYear = (!String.IsNullOrEmpty(drpSummaryYear.SelectedValue)) ? Convert.ToInt64(drpSummaryYear.SelectedValue) : 0;

            if (pEmployeeID > 0)
            {
                List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
                lstEntity = BAL.AttendanceMgmt.GetAttendanceList(0, pEmployeeID, pMonth, pYear);
                if (lstEntity.Count > 0)
                {
                    Decimal totFD = lstEntity.Where(r => r.WorkingHrsFlag == 1).Sum(r => r.WorkingHrsFlag);
                    Decimal totHD = lstEntity.Where(r => r.WorkingHrsFlag < 1).Sum(r => r.WorkingHrsFlag);
                    Decimal totPR = lstEntity.Sum(r => r.WorkingHrsFlag);

                    lblFull.Text = totFD.ToString();
                    lblHalf.Text = totHD.ToString();
                    lblPres.Text = totPR.ToString();
                    // -------------------------------------------------------------------------------------
                    lblTotalShiftHrs.Text = calculateHrs(lstEntity, "ShiftTotalHrs");
                    lblTotalWKHrs.Text = calculateHrs(lstEntity, "WorkingTotalHrs");
                    lblTotalOTHrs.Text = calculateHrs(lstEntity, "OTHrs");
                }
            }
            else
            {
                lblFull.Text = "";
                lblHalf.Text = "";
                lblPres.Text = "";
                lblTotalShiftHrs.Text = "";
                lblTotalWKHrs.Text = "";
                lblTotalOTHrs.Text = "";
            }
        }
        public string calculateHrs(List<Entity.Attendance> lstEntity, string colName)
        {
            string returnVal = "";
            Decimal hrs1 = 0, mins1 = 0, hrs = 0, mins = 0;
            // ----------------------------------------------
            foreach (Entity.Attendance tmpAttendance in lstEntity)
            {
                if (colName == "ShiftTotalHrs")
                {
                    if (!String.IsNullOrEmpty(tmpAttendance.ShiftTotalHrs.ToString()) &&
                        tmpAttendance.ShiftTotalHrs.ToString() != "0" && tmpAttendance.ShiftTotalHrs.ToString() != "0.00")
                    {
                        string[] tmpAry = tmpAttendance.ShiftTotalHrs.ToString().Split('.');
                        hrs1 += Convert.ToDecimal(tmpAry[0]);
                        mins1 += Convert.ToDecimal(tmpAry[1]);
                    }
                }
                else if (colName == "WorkingTotalHrs")
                {
                    if (!String.IsNullOrEmpty(tmpAttendance.WorkingTotalHrs.ToString()) &&
                       tmpAttendance.WorkingTotalHrs.ToString() != "0" && tmpAttendance.WorkingTotalHrs.ToString() != "0.00")
                    {
                        string[] tmpAry = tmpAttendance.WorkingTotalHrs.ToString().Split('.');
                        hrs1 += Convert.ToDecimal(tmpAry[0]);
                        mins1 += Convert.ToDecimal(tmpAry[1]);
                    }
                }
                else if (colName == "OTHrs")
                {
                    if (!String.IsNullOrEmpty(tmpAttendance.OTHrs.ToString()) &&
                       tmpAttendance.OTHrs.ToString() != "0" && tmpAttendance.OTHrs.ToString() != "0.00")
                    {
                        string[] tmpAry = tmpAttendance.OTHrs.ToString().Split('.');
                        hrs1 += Convert.ToDecimal(tmpAry[0]);
                        mins1 += Convert.ToDecimal(tmpAry[1]);
                    }
                }
            }
            // ----------------------------------------------
            hrs = hrs1 + Math.Floor(mins1 / 60);
            mins = (mins1 - (Math.Floor(mins1 / 60) * 60));
            // ----------------------------------------------
            returnVal = hrs.ToString("00") + "." + mins.ToString("00");
            return returnVal;
        }

        protected void ImgbtnAttendence_Click(object sender, EventArgs e)
        {
            DateTime LatestDt = BAL.CommonMgmt.GetLatestAttendenceDt();
            if (LatestDt == Convert.ToDateTime("0001-01-01"))
            {
                LatestDt = Convert.ToDateTime("1900-01-01");
            }
            string tmpConnection = ConfigurationManager.ConnectionStrings["BioConnection"].ConnectionString;
            SqlCommand command = new SqlCommand();
            SqlConnection sCon = new SqlConnection();

            sCon = new SqlConnection();
            sCon.ConnectionString = tmpConnection;
            command.Connection = sCon;
            command.CommandTimeout = 30000;
            sCon.Open();
            // ----------------------------------------------------
            int ReturnCode;
            String ReturnMsg = "";

            command.CommandType = CommandType.Text;
            command.CommandText = "Select CardNo, EmpName, Attn_Dt, Convert(NVARCHAR,MIN(IN_OUT_TIME),8) In_TIME , Convert(NVARCHAR,MAX(IN_OUT_TIME),8) Out_TIME From tmpDmpTerminalData Inner Join EmpMaster On tmpDmpTerminalData.CardNo = EmpMaster.EmpID GROUP By CardNo, EmpName, Attn_Dt Having Convert(DateTime,Attn_Dt) >= ('" + LatestDt.ToString("yyyy-MM-dd") + "')";
            SqlDataReader dr = command.ExecuteReader();
            List<Entity.Attendance> lstAttendence = new List<Entity.Attendance>();
            while (dr.Read())
            {
                Entity.Attendance objAttendence = new Entity.Attendance();
                objAttendence.CardNo = Convert.ToInt64(dr["CardNo"]);
                objAttendence.EmployeeName = dr["EmpName"].ToString();
                objAttendence.PresenceDate = Convert.ToDateTime(dr["Attn_Dt"]);
                objAttendence.TimeIn = dr["In_TIME"].ToString();
                objAttendence.TimeOut = dr["Out_TIME"].ToString();
                objAttendence.LoginUserID = Session["LoginUserID"].ToString();
                // ----------------------------------------------------------------
                BAL.AttendanceMgmt.AddUpdateBiometricAttendance(objAttendence, out ReturnCode, out ReturnMsg);
                //lstAttendence.Add(objAttendence);
            }

            // ----------------------------------------------------
            sCon.Close();
            SqlConnection.ClearAllPools();
        }
    }
}