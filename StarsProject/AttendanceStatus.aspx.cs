using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class AttendanceStatus : System.Web.UI.Page
    {
        int TotalCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["empid"]))
                {
                    hdnEmployeeID.Value = Request.QueryString["empid"].ToString();
                    // -------------------------------------------------------
                    if (!String.IsNullOrEmpty(Request.QueryString["month"]))
                    {
                        hdnMonth.Value = Request.QueryString["month"].ToString();
                        hdnMonth.Value = (hdnMonth.Value.Length == 1) ? "0" + hdnMonth.Value : hdnMonth.Value;
                    }

                    if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                    {
                        hdnYear.Value = Request.QueryString["year"].ToString();
                    }
                    // -----------------------------------------------------------------------------------
                    List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
                    lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(Convert.ToInt64(hdnEmployeeID.Value), 1, 9999, out TotalCount);
                    if (lstEmployee.Count>0)
                    {
                        lblEmployeeName.Text = lstEmployee[0].EmployeeName;
                        lblShift.Text = lstEmployee[0].ShiftName;
                        List<Entity.ShiftMaster> lstShift = new List<Entity.ShiftMaster>();
                        lstShift = BAL.ShiftMasterMgmt.GetShiftMaster(Convert.ToInt64(lstEmployee[0].ShiftCode), Session["LoginUserID"].ToString(), 1, 9999, out TotalCount);
                        if (lstShift.Count>0)
                        {
                            lblShiftMinHrs.Text = lstShift[0].MinHrsHalfDay.ToString();
                            lblShiftMaxHrs.Text = lstShift[0].MinHrsFullDay.ToString();
                            lblGraceMins.Text = Math.Abs(lstShift[0].GraceMins).ToString();
                        }
                    }
                    // -----------------------------------------------------------------------------------
                    List<Entity.Attendance> lstEntity = new List<Entity.Attendance>();
                    lstEntity = BAL.AttendanceMgmt.GetAttendanceList(0, Convert.ToInt64(hdnEmployeeID.Value), Convert.ToInt64(hdnMonth.Value), Convert.ToInt64(hdnYear.Value));
                    rptAttendaceStatus.DataSource = lstEntity;
                    rptAttendaceStatus.DataBind();
                    if (lstEntity.Count>0)
                    {
                        //Decimal totFD = lstEntity.Where(r => r.WorkingHrsFlag(r.Field<string>("Category").ToLower() == "followup" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));
                        // -------------------------------------------------------------------------------------
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
        protected void rptAttendaceStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        trItem.Style.Add("background-color", "#f5f5dc");
                        trItem.Style.Add("color", "blue");
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
                    //if (obj.WorkingHrsFlag == 0)
                    //    dvFlag.InnerHtml = "<i class='material-icons font-size-small red-text'>brightness_1</i>";
                    //else if (obj.WorkingHrsFlag == Convert.ToDecimal(0.50))
                    //    dvFlag.InnerHtml = "<i class='material-icons font-size-small green-text'>brightness_6</i>";
                    //else if (obj.WorkingHrsFlag == Convert.ToDecimal(1.00))
                    //    dvFlag.InnerHtml = "<i class='material-icons font-size-small green-text'>brightness_5</i>";

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

                //if (tdWorkingHrsFlag.InnerText == "Absent")
                //    tdDayStatus.Style.Add("color", "white");
                // ------------------------------------------------
            }
        }
    }
}