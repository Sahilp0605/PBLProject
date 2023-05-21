using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Services;
using System.Web;

namespace StarsProject
{
    public partial class MasterReport : System.Web.UI.Page
    {
        //Public Variable Declaration
        Nullable<DateTime> d1, d2;
        DateTime d3, d4, d5;
        String pCustomerID;
        string msg = "No Record Found For Selected Criteria";
        string sysmsg = "Kindly Contact Your Application Provider";
        public string loginuserid, LoginEmployee;
        //

        protected void Page_Load(object sender, EventArgs e)
        {
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)Session["logindetail"];
            loginuserid = objAuth.LoginUserID;
            LoginEmployee = objAuth.EmployeeName;
            // ---------------------------------------------------------------
            if (!IsPostBack)
            {
                hdnSerialKey.Value = Session["SerialKey"].ToString();

                Session["report"] = null;
                CrystalReportViewer1.ReportSource = null;
                DateTime dt = DateTime.Now; // Your Date
                DateTime start = new DateTime(dt.Year, dt.Month, 1); //First Date of the month
                DateTime end = start.AddMonths(1).AddDays(-1); //Last Date of the month
                //Literal lblUserVal = (Literal)Page.Master.FindControl("ltrEmployeeName");
                //lblUserVal.Text = "admin";   // lblUserVal.Text;
                LoginEmployee = "admin";     // lblUserVal.Text;
                txtFromDate.Focus();
                //string startDay = start.DayOfWeek.ToString(); //First weekday of the month
                //string endDay = end.DayOfWeek.ToString(); //Last weekday of the month


                //txtFromDate.Text = start.ToString("dd-MM-yyyy");
                //txtToDate.Text = end.ToString("dd-MM-yyyy");

                //txtFromDate.Text = DateTime.Now.AddDays(-30).ToString("dd-MM-yyyy");
                //txtToDate.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                //txtAsOnDate.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");

                txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                txtAsOnDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                // -----------------------------------------------------------------
                if ((!String.IsNullOrEmpty(Request.QueryString["ReportName"].ToString())) && (!String.IsNullOrEmpty(Request.QueryString["DispName"].ToString())))
                {
                    hdnReportName.Value = Request.QueryString["ReportName"].ToString();
                    lblReportTitle.Text = Request.QueryString["DispName"].ToString();

                }
                // -----------------------------------------------------------------    
                if (
                    hdnReportName.Value.ToLower() == "salestargetlist" ||
                    hdnReportName.Value.ToLower() == "followupbycustomer" || hdnReportName.Value.ToLower() == "followupbyemployee"
                    )
                {
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                    if (hdnReportName.Value.ToLower() != "salestargetlist")
                        divInqTele.Visible = true;
                    divDurationSelection.Visible = true;
                    if (hdnReportName.Value.ToLower() == "followupbyemployee")
                        divEmployeeDate.Visible = true;
                }



                //else
                //{

                //    divEmployee.Visible = false;
                //}
                //-------------------------------------------------------------------
                if (hdnReportName.Value.ToLower() == "inquirylist" || hdnReportName.Value.ToLower() == "inquirydetail")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    BindInquiryDropdown();
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                    divInquiry.Visible = true;
                    if(hdnSerialKey.Value== "ECO3-2G21-TECH-3MRT")
                    {
                        BindCountry();
                        BindState();
                        BindCity();
                        divStateCity.Visible = true;
                    }
                        
                }


                if (hdnReportName.Value.ToLower() == "report_suppliermaterialstatus")
                {
                    divMonthYear.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "report_brandwiseproductgroupwiseproduct")
                {
                    BindProductDropDown();
                    divProduct.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "employeeactivity")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "followupbycustomer" || hdnReportName.Value.ToLower() == "followupbyemployee")
                {
                    divInquiry.Visible = true;
                    divInqTele.Visible = true;
                    BindInquiryDropdown();
                    BindEmployeeDropdown();
                    BindCustomerDropdown();
                    divCustomer.Visible = (hdnReportName.Value.ToLower() == "followupbycustomer") ? true : false;
                    //divFollowUpCust.Visible = (hdnReportName.Value.ToLower() == "followupbycustomer") ? true : false;
                    divEmployee.Visible = (hdnReportName.Value.ToLower() == "followupbyemployee") ? true : false;
                    divEmployeeDate.Visible = (hdnReportName.Value.ToLower() == "followupbyemployee") ? true : false;
                }

                if (hdnReportName.Value.ToLower() == "quotationfromprojectname" || hdnReportName.Value.ToLower() == "salesorderfromprojectname" 
                    || hdnReportName.Value.ToLower() == "purchaseorderfromprojectname")
                {
                    divDurationSelection.Visible = true;
                    //divProjectList.Visible = true;
                    divProjectName.Visible = true;
                    BindProjectDropdown();

                }


                if (hdnReportName.Value.ToLower() == "customerledger")
                {
                    divDurationSelection.Visible = true;
                    //divFollowUpCust.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "externalinquirydetail" || hdnReportName.Value.ToLower() == "telecallerinquiry" || hdnReportName.Value.ToLower() == "monthlyworking")
                {

                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    //divLeadStatus.Visible = true;
                    if (hdnReportName.Value.ToLower() == "externalinquirydetail")
                    {
                        divLeadStatus1.Visible = true;
                        divDateDropdown.Visible = true;
                    }
                    else if (hdnReportName.Value.ToLower() == "telecallerinquiry" || hdnReportName.Value.ToLower() == "monthlyworking")
                    {
                        //divInqTele.Visible = true;
                        divLeadStatus1.Visible = true;
                        if (hdnReportName.Value.ToLower() == "monthlyworking")
                            divInqTele.Visible = true;
                    }
                    //BindLeadStatus();
                    BindEmployeeDropdown();
                    BindReason();

                }

                if (hdnReportName.Value.ToLower() == "dailyvisitreport" || hdnReportName.Value.ToLower() == "telecallerinquiry" || hdnReportName.Value.ToLower() == "monthlyworking")
                {

                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    //divLeadStatus.Visible = true;
                    if (hdnReportName.Value.ToLower() == "dailyvisitreport")
                    {
                        divLeadStatus1.Visible = true;
                        divDateDropdown.Visible = true;
                    }
                    else if (hdnReportName.Value.ToLower() == "telecallerinquiry" || hdnReportName.Value.ToLower() == "monthlyworking")
                    {
                        //divInqTele.Visible = true;
                        divLeadStatus1.Visible = true;
                        if (hdnReportName.Value.ToLower() == "monthlyworking")
                            divInqTele.Visible = true;
                    }
                    //BindLeadStatus();
                    BindEmployeeDropdown();
                    BindReason();

                }

               
                if (hdnReportName.Value.ToLower() == "attendancelist")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();

                }
                if (hdnReportName.Value.ToLower() == "dailyactivitylist")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    divTaskCategory.Visible = true;
                    BindTaskCategory();
                    BindEmployeeDropdown();

                }
                if (hdnReportName.Value.ToLower() == "tododetaillist")
                {
                    divAssignTo.Visible = true;
                    divEmployee.Visible = true;
                    divDurationSelection.Visible = true;
                    divTodoStatus.Visible = true;
                    divPriority.Visible = true;

                    BindEmployeeDropdown();
                    BindAssignToEmployeeDropdown();
                    BindTodoStatus();

                }
                if (hdnReportName.Value.ToLower() == "complaintlistreort")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divEmployee.Visible = true;
                    divCompliantStatus.Visible = true;
                    divAssignTo.Visible = true;
                    divTodoStatus.Visible = true;
                    divdrptodoStatus.Visible = false;

                    BindCustomerDropdown();
                    BindEmployeeDropdown();
                    BindAssignToEmployeeDropdown();
                    BindTodoStatus();
                    BindComplainStatus();
                }
                if (hdnReportName.Value.ToLower() == "leaverequestlistreport")
                {
                    divDurationSelection.Visible = true;
                    divLeaveStatus.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                    BindLeaveStatus();

                }

                if (hdnReportName.Value.ToLower() == "expenselistreport")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    divExpenseType.Visible = true;
                    BindExpenseType();
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "report_users")
                {
                    divDurationSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "customerlist")
                {
                    //divEmployee.Visible = true;
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divStateCity.Visible = true;
                    divCustomerList.Visible = true;
                    BindCustomerType();
                    BindCustomerDropdown();
                    BindState();
                    BindCity();

                }

                if (hdnReportName.Value.ToLower() == "customermasterlist")
                {
                    //divEmployee.Visible = true;
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divStateCity.Visible = true;
                    divCustomerList.Visible = true;
                    BindCustomerType();
                    BindCustomerDropdown();
                    BindState();
                    BindCity();
                    
                }

                if (hdnReportName.Value.ToLower() == "salesbilllist")
                {
                    divDurationSelection.Visible = true;
                    //divFollowUpCust.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    if (hdnSerialKey.Value == "HONP-MEDF-9RTS-FG10" || hdnSerialKey.Value == "TWS3-RT90-E22O-K88P")
                    {
                        divLocation.Visible = true;
                        BindLocationDropDown();
                    }
                }
                if (hdnReportName.Value.ToLower() == "detailsalesbilllist")
                {
                    divDurationSelection.Visible = true;
                    //divFollowUpCust.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "purchasebilllist")
                {
                    divDurationSelection.Visible = true;
                    //divFollowUpCust.Visible = true;
                    divCustomer.Visible = true;
                    divEmployee.Visible = true;
                    BindCustomerDropdown();
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "detailpurchasesbilllist")
                {
                    divDurationSelection.Visible = true;
                    //divFollowUpCust.Visible = true;
                    divCustomer.Visible = true;
                    divEmployee.Visible = true;
                    BindCustomerDropdown();
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "financialtransection")
                {
                    divDurationSelection.Visible = true;
                    divCustomerLedger.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "salesorderlist" || hdnReportName.Value.ToLower() == "salesorderdetail")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                    //divFollowUpCust.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    divApprovalStatus.Visible = true;
                    BindApprovalStatus();
                }
                if (hdnReportName.Value.ToLower() == "purchaseorderlist" || hdnReportName.Value.ToLower() == "purchaseorderdetail")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    divCustomer.Visible = true;
                    divProjectName.Visible = true;
                    BindCustomerDropdown();
                    divApprovalStatus.Visible = true;
                    BindApprovalStatus();
                    BindProjectDropdown();
                    BindEmployeeDropdown();
                    
                }
                if (hdnReportName.Value.ToLower() == "quotationlist" || hdnReportName.Value.ToLower() == "quotationdetail" || hdnReportName.Value.ToLower() == "quotationbasedoncountry")
                {
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    divDurationSelection.Visible = true;
                    if (hdnReportName.Value.ToLower() == "quotationbasedoncountry")
                        divBasedCountry.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();

                }
                if (hdnReportName.Value.ToLower() == "inquirycliniclist")
                {
                    divDurationSelection.Visible = true;
                    divFollowUpCust.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "tripbyvehicle" || hdnReportName.Value.ToLower() == "tripbydriver")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                    divVehiclelist.Visible = true;
                    BindVehicalDropdown();
                }
                if (hdnReportName.Value.ToLower() == "rojmel")
                {
                    divSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "report_stock")
                {
                    divSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "report_stock_new")
                {
                    divDurationSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "todo")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    divTodoStatus.Visible = true;
                    divCompliantStatus.Visible = false;

                    BindEmployeeDropdown();
                    BindTodoStatus();
                }
                if (hdnReportName.Value.ToLower() == "pettycashreport")
                {
                    divDurationSelection.Visible = true;
                    divFixedLedger.Visible = true;
                    BindFixedLedgerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "crnotereport")
                {
                    divDurationSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "dbnotereport")
                {
                    divDurationSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "cashbook")
                {
                    divDurationSelection.Visible = true;
                    divFixedLedger.Visible = true;
                    BindFixedLedgerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "bankbook")
                {
                    divDurationSelection.Visible = true;
                    divFixedLedger.Visible = true;
                    BindFixedLedgerDropdown();
                }

                if (hdnReportName.Value.ToLower() == "bankvoucherreport")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divCreditors.Visible = true;
                    BindCustomerDropdown();
                    //if (hdnSerialKey.Value == "TWS3-RT90-E22O-K88P")  //TWS
                    //{
                    //    ListItem removeItem = drpCreditOption.Items.FindByText("Remaining Credit");
                    //    drpCreditOption.Items.Remove(removeItem);
                    //}
                }

                if (hdnReportName.Value.ToLower() == "jv")
                {
                    divDurationSelection.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "purchaseregister" || hdnReportName.Value.ToLower() == "productwisepurchase")
                {
                    divDurationSelection.Visible = true;
                    divProduct.Visible = true;
                    BindProductDropDown();
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "statewisepurchase")
                {
                    divDurationSelection.Visible = true;
                    divStateCity.Visible = true;
                    divCustomer.Visible = true;
                    BindState();
                    BindCity();
                    BindCustomerDropdown();

                }
                if (hdnReportName.Value.ToLower() == "customerwisesale" || hdnReportName.Value.ToLower() == "brandwisesale")
                {
                    divDurationSelection.Visible = true;
                    divProduct.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    BindProductDropDown();
                }

                if (hdnReportName.Value.ToLower() == "statecustomerwisesale")
                {
                    divDurationSelection.Visible = true;
                    divStateCity.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    BindState();
                    BindCity();
                }
                if (hdnReportName.Value.ToLower() == "monthlysalessummary")
                {
                    divDurationSelection.Visible = true;
                    divQtyAmt.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "monthlypurchase")
                {
                    divDurationSelection.Visible = true;
                    divQtyAmt.Visible = true;
                }
                if (hdnReportName.Value.ToLower() == "minstockreport")
                {
                    divProduct.Visible = true;
                    BindProductDropDown();
                }
                if (hdnReportName.Value.ToLower() == "empwisemovement")
                {
                    divEmployee.Visible = true;
                    divDurationSelection.Visible = true;
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "userlogreport")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "orderagainstpurchaseorder")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "orderagainstsalesorder")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }

                if (hdnReportName.Value.ToLower() == "productionbysalesorder")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }


                if (hdnReportName.Value.ToLower() == "complaintreport")
                {
                    divDurationSelection.Visible = true;
                    BindEmployeeDropdown();
                    divEmployee.Visible = true;
                    divCompstatus.Visible = true;
                    BindComplainStatus();
                }
                if (hdnReportName.Value.ToLower() == "salaryregister")
                {
                    divMonthYear.Visible = true;
                    divAttendenceType.Visible = true;
                    //divYear.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();

                }
                if (hdnReportName.Value.ToLower() == "shiftreport")
                {
                    divShift.Visible = true;
                    divBasicPer.Visible = true;
                    BindShift();
                }
                if (hdnReportName.Value.ToLower() == "complaintwithsign")
                {
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "jobcardoutward")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divProducts.Visible = true;
                    BindCustomerDropdown();
                    BindProductDropDown();
                }
                if (hdnReportName.Value.ToLower() == "jobcardinward")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divProducts.Visible = true;
                    BindCustomerDropdown();
                    BindProductDropDown();
                }
                if (hdnReportName.Value.ToLower() == "jobcardlist")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divProducts.Visible = true;
                    divLocation.Visible = true;
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");

                    BindCustomerDropdown();
                    BindProductDropDown();
                    BindLocationDropDown();
                }
                if (hdnReportName.Value.ToLower() == "debitcreditnote")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    BindCustomerDropdown();
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");

                }
                if (hdnReportName.Value.ToLower() == "visitormanagement")
                {
                    divDurationSelection.Visible = true;
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    //BindLocationDropDown();
                }

                if (hdnReportName.Value.ToLower() == "ltremployeename".ToLower())
                {
                    divDurationSelection.Visible = true;
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    //BindLocationDropDown();
                }

                if (hdnReportName.Value.ToLower() == "materialmovement")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divTransType.Visible = true;
                    divProducts.Visible = true;
                    BindCustomerDropdown();
                    BindProductDropDown();
                }
                if (hdnReportName.Value.ToLower() == "installfabric")
                {
                    divCustomer.Visible = true;
                    divInsType.Visible = true;
                    BindCustomerDropdown();
                }
                if (hdnReportName.Value.ToLower() == "packinglist")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divProducts.Visible = true;
                    BindCustomerDropdown();
                    BindProductDropDown();
                }
                if (hdnReportName.Value.ToLower() == "grn")
                {
                    divDurationSelection.Visible = true;
                    divCustomer.Visible = true;
                    divProducts.Visible = true;
                    BindCustomerDropdown();
                    BindProductDropDown();
                }
                if (hdnReportName.Value.ToLower() == "expensevoucher")
                {
                    divDurationSelection.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();
                }
                if (hdnReportName.Value.ToLower() == "overtime")
                {
                    divDurationSelection.Visible = true;
                    //divLeaveStatus.Visible = true;
                    divEmployee.Visible = true;
                    BindEmployeeDropdown();


                }
                if (hdnReportName.Value.ToLower() == "outwardreport")
                {
                    divDurationSelection.Visible = true;
                    //divLeaveStatus.Visible = true;
                    divCustomer.Visible = true;
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    BindCustomerDropdown();
                }

                if (hdnReportName.Value.ToLower() == "inwardreport")
                {
                    divDurationSelection.Visible = true;
                    //divLeaveStatus.Visible = true;
                    divCustomer.Visible = true;
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    BindCustomerDropdown();
                }

                if (hdnReportName.Value.ToLower() == "inwardoutwardreport")
                {
                    divDurationSelection.Visible = true;
                    //divLeaveStatus.Visible = true;
                    divCustomer.Visible = true;
                    txtFromDate.Visible = true;
                    txtToDate.Visible = true;

                    txtFromDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    BindCustomerDropdown();
                }

                if (hdnReportName.Value.ToLower() == "pendinglistreport")
                {
                    //divDurationSelection.Visible = true;
                    //divLeaveStatus.Visible = true;
                    //divCustomer.Visible = true;
                    //divPendingTextBox.Visible = true;
                    //BindCustomerDropdown();
                }
            }
            else
            {
                //Literal lblUserVal = (Literal)Page.Master.FindControl("ltrEmployeeName");
                //LoginEmployee = objAuth.LoginUserID;
            }

        }

        #region Bind ALL ComboBox
        public void BindShift()
        {
            List<Entity.ShiftMaster> lstShift = new List<Entity.ShiftMaster>();
            lstShift = BAL.ShiftMasterMgmt.GetShiftMaster(Session["LoginUserID"].ToString());
            drpShift.DataSource = lstShift;
            drpShift.DataValueField = "ShiftCode";
            drpShift.DataTextField = "ShiftName";
            drpShift.DataBind();
            drpShift.Items.Insert(0, new ListItem("-- Select Shift --", ""));
        }
        public void BindEmployeeDropdown()
        {
            // ---------------- Assign Employee ------------------------
            int totrec;
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            loginuserid = Session["LoginUserID"].ToString();
            //lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList("", Session["LoginUserID"].ToString(), 1, 99999, out totrec);
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- All --", "0"));
        }

        public void BindVehicalDropdown()
        {
            int TotalRecord = 0;
            // ---------------- Assign Employee ------------------------
            List<Entity.Vehicle> lstVehicle = new List<Entity.Vehicle>();
            loginuserid = Session["LoginUserID"].ToString();
            lstVehicle = BAL.VehicleMgmt.GetVehicleList(0, Session["LoginUserID"].ToString(), 1, 1111, out TotalRecord);
            drpVehicleList.DataSource = lstVehicle;
            drpVehicleList.DataValueField = "pKID";
            drpVehicleList.DataTextField = "RegistrationNo";
            drpVehicleList.DataBind();
            drpVehicleList.Items.Insert(0, new ListItem("-- All --", "0"));
        }

        public void BindCustomerDropdown()
        {
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            loginuserid = Session["LoginUserID"].ToString();
            lstCust = BAL.CustomerMgmt.GetCustomerList();
            drpCustomer.DataSource = lstCust;
            drpCustomer.DataValueField = "CustomerID";
            drpCustomer.DataTextField = "CustomerName";
            drpCustomer.DataBind();
            drpCustomer.Items.Insert(0, new ListItem("-- All --", "0"));
        }

        public void BindProjectDropdown()
        {
            int tot = 0;
            List<Entity.Projects> lstProject = new List<Entity.Projects>();
            loginuserid = Session["LoginUserID"].ToString();
            lstProject = BAL.ProjectsMgmt.GetProjectsList(0, Session["LoginUserID"].ToString(), 1, 1111, out tot);//0,Session["LoginUserID"].ToString(),1,1111,tot out
            drpProject.DataSource = lstProject;
            drpProject.DataValueField = "pkID";
            drpProject.DataTextField = "ProjectName";
            drpProject.DataBind();
            drpProject.Items.Insert(0, new ListItem("-- All --", ""));
        }

        public void BindInquiryDropdown()
        {
            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            if (hdnReportName.Value.ToLower() == "inquirylist" || hdnReportName.Value.ToLower() == "inquirydetail")
            {
                lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("Inquiry");
            }
            if (hdnReportName.Value.ToLower() == "followupbycustomer" || hdnReportName.Value.ToLower() == "followupbyemployee")
            {
                lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("Followup");
            }

            drpInquiryStatus.DataSource = lstDesig;
            drpInquiryStatus.DataValueField = "pkID";
            drpInquiryStatus.DataTextField = "InquiryStatusName";
            drpInquiryStatus.DataBind();
            drpInquiryStatus.Items.Insert(0, new ListItem("-- All --", ""));

            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstSource = new List<Entity.InquiryStatus>();
            lstSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquirySource");
            drpInquirySource.DataSource = lstSource;
            drpInquirySource.DataValueField = "InquiryStatusName";
            drpInquirySource.DataTextField = "InquiryStatusName";
            drpInquirySource.DataBind();
            drpInquirySource.Items.Insert(0, new ListItem("-- All --", "0"));
        }
        public void BindProductDropDown()
        {
            // ---------------- Product Group List -------------------------------------
            List<Entity.ProductGroup> lstEvents = new List<Entity.ProductGroup>();
            lstEvents = BAL.ProductGroupMgmt.GetProductGroupList();
            drpProductGroup.DataSource = lstEvents;
            drpProductGroup.DataValueField = "pkID";
            drpProductGroup.DataTextField = "ProductGroupName";
            drpProductGroup.DataBind();
            drpProductGroup.Items.Insert(0, new ListItem("-- All --", "0"));

            // ---------------- Brand List -------------------------------------
            List<Entity.Brand> lstEvents1 = new List<Entity.Brand>();
            lstEvents1 = BAL.BrandMgmt.GetBrandList();
            drpBrand.DataSource = lstEvents1;
            drpBrand.DataValueField = "pkID";
            drpBrand.DataTextField = "BrandName";
            drpBrand.DataBind();
            drpBrand.Items.Insert(0, new ListItem("-- All --", "0"));


            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstEvents2 = new List<Entity.Products>();
            lstEvents2 = BAL.ProductMgmt.GetProductList();
            drpProducts.DataSource = lstEvents2;
            drpProducts.DataValueField = "pkID";
            drpProducts.DataTextField = "ProductName";
            drpProducts.DataBind();
            drpProducts.Items.Insert(0, new ListItem("-- All --", "0"));
        }
        public void BindAssignToEmployeeDropdown()
        {

            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            loginuserid = Session["LoginUserID"].ToString();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(loginuserid);
            //List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            //loginuserid = BAL.ReportMgmt.GetUserIDByEmployeeID(Convert.ToInt64(drpEmployee.SelectedValue));
            //lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(loginuserid);
            drpAssignToEmployee.DataSource = lstEmployee;
            drpAssignToEmployee.DataValueField = "pkID";
            drpAssignToEmployee.DataTextField = "EmployeeName";
            drpAssignToEmployee.DataBind();
            drpAssignToEmployee.Items.Insert(0, new ListItem("-- All --", "0"));

        }
        public void BindTaskCategory()
        {
            try
            {
                int TotalRec;
                List<Entity.TaskCategory> lstTaskCategory = new List<Entity.TaskCategory>();
                lstTaskCategory = BAL.DailyActivityMgmt.GetTaskCategoryList(0, 1, 5000, out TotalRec);
                drpTaskCategory.DataSource = lstTaskCategory;
                drpTaskCategory.DataValueField = "pkID";
                drpTaskCategory.DataTextField = "TaskCategoryName";
                drpTaskCategory.DataBind();
                drpTaskCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Category--", "0"));
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        void BindExpenseType()
        {
            List<Entity.ExpenseType> lstExpense = new List<Entity.ExpenseType>();
            lstExpense = BAL.ExpenseTypeMgmt.GetExpenseTypeList(0);
            drpExpenseType.DataSource = lstExpense;
            drpExpenseType.DataValueField = "pkId";
            drpExpenseType.DataTextField = "ExpenseTypeName";
            drpExpenseType.DataBind();
            drpExpenseType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Expense Type --", ""));
        }

        void BindApprovalStatus()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            if (hdnReportName.Value.ToLower() == "salesorderlist" || hdnReportName.Value.ToLower() == "salesorderdetail")
                lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("SOApproval");
            else if (hdnReportName.Value.ToLower() == "purchaseorderlist" || hdnReportName.Value.ToLower() == "purchaseorderdetail")
                lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("POApproval");
            drpApprovalStatus.DataSource = lstDesig;
            drpApprovalStatus.DataValueField = "InquiryStatusName";
            drpApprovalStatus.DataTextField = "InquiryStatusName";
            drpApprovalStatus.DataBind();
            drpApprovalStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Approval Status--", ""));
        }

        void BindCustomerType()
        {
            List<Entity.CustomerCategory> lstDesig = new List<Entity.CustomerCategory>();
            lstDesig = BAL.CustomerCategoryMgmt.GetCustomerCategoryList();
            drpCustomerType.DataSource = lstDesig;
            drpCustomerType.DataValueField = "CategoryName";
            drpCustomerType.DataTextField = "CategoryName";
            drpCustomerType.DataBind();
            drpCustomerType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer Type--", ""));
        }

        void BindLeadStatus()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("Inquiry");
            drpLeadStatus.DataSource = lstDesig;
            drpLeadStatus.DataValueField = "InquiryStatusName";
            drpLeadStatus.DataTextField = "InquiryStatusName";
            drpLeadStatus.DataBind();
            drpLeadStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Lead Status--", ""));
        }

        void BindLeaveStatus()
        {
            List<Entity.LeaveRequest> lstDesig = new List<Entity.LeaveRequest>();
            lstDesig = BAL.LeaveRequestMgmt.GetLeaveTypes();
            drpLeaveStatus.DataSource = lstDesig;
            drpLeaveStatus.DataValueField = "LeaveTypeID";
            drpLeaveStatus.DataTextField = "LeaveType";
            drpLeaveStatus.DataBind();
            drpLeaveStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Leave Status--", ""));
        }

        void BindTodoStatus()
        {
            List<Entity.ToDoCategory> lstDesig = new List<Entity.ToDoCategory>();
            lstDesig = BAL.ToDoCategoryMgmt.GetTaskCategoryList("TODO");
            drpTodoStatus.DataSource = lstDesig;
            drpTodoStatus.DataValueField = "pkID";
            drpTodoStatus.DataTextField = "TaskCategoryName";
            drpTodoStatus.DataBind();
            drpTodoStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select TODO Status--", ""));
        }

        void BindComplainStatus()
        {
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("ComplaintStatus");
            drpComplaintStatus.DataSource = lstDesig;
            drpComplaintStatus.DataValueField = "InquiryStatusName";
            drpComplaintStatus.DataTextField = "InquiryStatusName";
            drpComplaintStatus.DataBind();
            drpComplaintStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Complain Status--", ""));
        }

        void BindCountry()
        {
            drpCountry.ClearSelection();
            List<Entity.Country> lstCountry = new List<Entity.Country>();
            lstCountry = BAL.CountryMgmt.GetCountryList();
            drpCountry.DataSource = lstCountry;
            drpCountry.DataValueField = "CountryCode";
            drpCountry.DataTextField = "CountryName";
            drpCountry.DataBind();
            drpCountry.Items.Insert(0, new ListItem("-- All --", "0"));
        }

        void BindState()
        {
            List<Entity.State> lstState = new List<Entity.State>();
            if (hdnSerialKey.Value == "ECO3-2G21-TECH-3MRT")
                lstState = BAL.StateMgmt.GetStateList(drpCountry.SelectedValue.ToString());
            else
                lstState = BAL.StateMgmt.GetStateList("IND");

            drpState.DataSource = lstState;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new ListItem("--All--", "0"));
        }
        void BindCity()
        {
            List<Entity.City> lstCity = new List<Entity.City>();
            if(hdnSerialKey.Value== "ECO3-2G21-TECH-3MRT")
                lstCity = BAL.CityMgmt.GetCityByState(Convert.ToInt32(drpState.SelectedValue.ToString()));
            else
                lstCity = BAL.CityMgmt.GetCityByState(12);

            drpCity.DataSource = lstCity;
            drpCity.DataValueField = "CityCode";
            drpCity.DataTextField = "CityName";
            drpCity.DataBind();
            drpCity.Items.Insert(0, new ListItem("--All--", "0"));
        }
        void BindReason()
        {

            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("DisQualifiedReason");
            drpDisqulifiedReason.DataSource = lstDesig;
            drpDisqulifiedReason.DataValueField = "pkID";
            drpDisqulifiedReason.DataTextField = "InquiryStatusName";
            drpDisqulifiedReason.DataBind();
            drpDisqulifiedReason.Items.Insert(0, new ListItem("--All--", "0"));
        }
        public void BindFixedLedgerDropdown()
        {
            List<Entity.Customer> lstLedger = new List<Entity.Customer>();
            lstLedger = BAL.CustomerMgmt.GetFixedLedgerForDropdown();
            drpFixedLedger.DataSource = lstLedger;
            drpFixedLedger.DataValueField = "CustomerID";
            drpFixedLedger.DataTextField = "CustomerName";
            drpFixedLedger.DataBind();
            drpFixedLedger.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select A/c --", ""));
        }

        //public void BindProjectDropdown()
        //{
        //    List<Entity.Customer> lstLedger = new List<Entity.Customer>();
        //    lstLedger = BAL.CustomerMgmt.GetFixedLedgerForDropdown();
        //    drpFixedLedger.DataSource = lstLedger;
        //    drpFixedLedger.DataValueField = "CustomerID";
        //    drpFixedLedger.DataTextField = "CustomerName";
        //    drpFixedLedger.DataBind();
        //    drpFixedLedger.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select A/c --", ""));
        //}

        public void BindLocationDropDown()
        {
            // ---------------- Product Group List -------------------------------------
            List<Entity.Location> lstEvents = new List<Entity.Location>();
            lstEvents = BAL.LocationMgmt.GetLocationList("admin");
            drpLocation.DataSource = lstEvents;
            drpLocation.DataValueField = "pkID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new ListItem("-- All --", "0"));
        }

        // Bind Drop Down For The UserID
        //public void BindUserDropDown()
        //{
        //    int TotalRecord;
        //    //----------------------------- Bind User Drop Down---------------------------------
        //    List<Entity.Users> lstEvents = new List<Entity.Users>();
        //    lstEvents = BAL.UserMgmt.GetLoginUserList(Session["UserID"].ToString(), 1, 5000, out TotalRecord);
        //    drpLocation.DataSource = lstEvents;
        //    drpLocation.DataValueField = "UserID";
        //    drpLocation.DataTextField = "UserID";
        //    drpLocation.DataBind();
        //    drpLocation.Items.Insert(0, new ListItem("-- All --", "0"));


        //}
        #endregion

        #region page event

        protected void btnShow_Click(object sender, EventArgs e)
        {
            loadReportData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            txtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            //txtPendingFrom.Text = txtPendingTo.Text;
            //txtPendingTo.Text = txtPendingTo.Text;

            txtAsOnDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            drpBrand.SelectedValue = "0";
            drpComplaintStatus.SelectedValue = "";
            drpEmployee.SelectedValue = "0";
            drpProductGroup.SelectedValue = "0";
            drpInquirySource.SelectedValue = "";
            drpInquiryStatus.SelectedValue = "";
            //drpSummaryMonth.SelectedValue = DateTime.Now.Month.ToString();
            //drpSummaryYear.SelectedValue = DateTime.Now.Year.ToString();
            txtCustomerName.Text = "";
            drpLeadStatus.SelectedValue = "";
            drpLeaveStatus.SelectedValue = "";
            drpExpenseType.SelectedValue = "";
        }

        protected void btnCloseError_Click(object sender, EventArgs e)
        {
            try
            {
                BindAssignToEmployeeDropdown();
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        #endregion

        #region Report Load Method Declartion

        protected void loadReportData()
        {

            //-----start Parameter Declaration and Value Assign for reports
            if (txtFromDate.Text != "")
            {
                d1 = Convert.ToDateTime(txtFromDate.Text);
            }
            if (txtToDate.Text != "")
            {
                d2 = Convert.ToDateTime(txtToDate.Text);
            }
            if (txtAsOnDate.Text != "")
            {
                d5 = Convert.ToDateTime(txtAsOnDate.Text);
            }
            if (txtCustomerName.Text == "")
            {
                pCustomerID = "0";
            }
            else
            {
                pCustomerID = hdnCustomerID.Value;
            }
            //Int64 pMonth = drpSummaryMonth.SelectedValue == "" ? 0 : Convert.ToInt64(drpSummaryMonth.SelectedValue);
            //Int64 pYear = drpSummaryYear.SelectedValue == "" ? 0 : Convert.ToInt64(drpSummaryYear.SelectedValue);

            int EmployeeID = drpEmployee.SelectedValue == "" ? 0 : Convert.ToInt32(drpEmployee.SelectedValue);
            int UserID = drpEmployee.SelectedValue == "" ? 0 : Convert.ToInt32(drpEmployee.SelectedValue);
            int VehicleID = drpVehicleList.SelectedValue == "" ? 0 : Convert.ToInt32(drpVehicleList.SelectedValue); 
            int AssignToEmployeeID = drpAssignToEmployee.SelectedValue == "" ? 0 : Convert.ToInt32(drpAssignToEmployee.SelectedValue);
            string TransCategory = drpTransactionType.SelectedValue;
            string LoginUserID = Session["LoginUserID"].ToString();
            string SearchKey = "";
            string pReportType = hdnReportName.Value.ToLower();
            string ReportType = hdnReportName.Value.ToLower();
            String pSource = drpInquirySource.SelectedValue;
            String pStatus = drpInquiryStatus.SelectedValue;
            String pProductGroup = drpProductGroup.SelectedValue;
            String pBrand = drpBrand.SelectedValue;
            String pEmployeeID = drpEmployee.SelectedValue;
            string LeadStatus = drpLeadStatus.SelectedValue;
            string CompailntStaus = drpComplaintStatus.SelectedValue;
            string LeaveStatus = drpLeaveStatus.SelectedValue;
            Int64 ExpenseTypeID = drpExpenseType.SelectedValue == "" ? 0 : Convert.ToInt64(drpExpenseType.SelectedValue);
            d3 = txtFromDate.Text == "" ? Convert.ToDateTime("1900/01/01") : Convert.ToDateTime(txtFromDate.Text);
            d4 = txtToDate.Text == "" ? Convert.ToDateTime("1900/01/01") : Convert.ToDateTime(txtToDate.Text);
            d5 = txtAsOnDate.Text == "" ? Convert.ToDateTime("1900/01/01") : Convert.ToDateTime(txtAsOnDate.Text);

            //d6 = txtPendingFrom.Text == "" ? Convert.ToDateTime("1900/01/01") : Convert.ToDateTime(txtPendingFrom.Text);
            //d7 = txtPendingTo.Text == "" ? Convert.ToDateTime("1900/01/01") : Convert.ToDateTime(txtPendingTo.Text);

            Int64 TaskCategory = drpTaskCategory.SelectedValue == "" ? 0 : Convert.ToInt64(drpTaskCategory.SelectedValue);
            string TaskStatus = drpTodoStatus.SelectedValue;
            string Status = drpStatus.SelectedValue;
            string CustomerType = drpCustomerType.SelectedValue;
            string CompanyType = drpCustomerType.SelectedValue;

            
            //Int64 PendingFrom = txtPendingFrom;
            //Int64 ProjectName = drpProject.SelectedValue == "" ? 0 : Convert.ToInt64(drpProject.SelectedValue);

            Int64 PendingFrom = txtPendingFrom.Text == "" ? 0 : Convert.ToInt64(txtPendingFrom.Text);

            Int64 PendingTo = txtPendingTo.Text == "" ? 0 : Convert.ToInt64(txtPendingTo.Text);


            Int64 ProjectName = drpProject.SelectedValue == "" ? 0 : Convert.ToInt64(drpProject.SelectedValue);

            //Reason = drpDisqulifiedReason.SelectedValue == "" ? 0 : Convert.ToInt32(drpDisqulifiedReason.SelectedValue);

            Int64 State = drpState.SelectedValue == "" ? 0 : Convert.ToInt32(drpState.SelectedValue);
            Int64 City = drpCity.SelectedValue == "" ? 0 : Convert.ToInt32(drpCity.SelectedValue);
            Int64 Reason = drpDisqulifiedReason.SelectedValue == "" ? 0 : Convert.ToInt32(drpDisqulifiedReason.SelectedValue);
            Int64 DurationType = drpDatedurationOn.SelectedValue == "" ? 0 : Convert.ToInt32(drpDatedurationOn.SelectedValue);
            string ApprovalStatus = drpApprovalStatus.SelectedValue == "" ? "0" : drpApprovalStatus.SelectedValue.ToString();
            String Month = drpMonth.SelectedValue == "0" ? "" : drpMonth.SelectedValue;
            String Year = drpYear.SelectedValue == "0" ? "" : drpYear.SelectedValue;
            Int64 CustomerId = drpCustomer.SelectedValue == "" ? 0 : Convert.ToInt32(drpCustomer.SelectedValue);
            String CustomerName = drpCustomer.SelectedValue == "" ? "0" : drpCustomer.SelectedItem.Text;
            Int64 ShiftCode = drpShift.SelectedValue == "" ? 0 : Convert.ToInt64(drpShift.SelectedValue);
            string BasicPer = drpBasic.SelectedValue == "" ? "" : drpBasic.SelectedValue.ToString();
            Int64 FixedLedger = drpFixedLedger.SelectedValue == "" ? 0 : Convert.ToInt32(drpFixedLedger.SelectedValue);
            string FixedLedgerName = drpFixedLedger.SelectedValue == "" ? "" : drpFixedLedger.SelectedItem.Text;
            Int64 ProductId = drpProducts.SelectedValue == "" ? 0 : Convert.ToInt32(drpProducts.SelectedValue);
            string TransType = drpTransType.SelectedValue == "" ? "" : drpTransType.SelectedValue.ToString();
            string InsType = drpInsType.SelectedValue == "" ? "" : drpInsType.SelectedValue.ToString();
            string InqTele = drpInqTele.SelectedValue == "" ? "telecaller" : drpInqTele.SelectedValue.ToString();
            string LeadStatus1 = drpLeadStatus1.SelectedValue == "" ? "" : drpLeadStatus1.SelectedValue;
            string Priority = drpPriority.SelectedValue == "" ? "" : drpPriority.SelectedValue;

            Int64 Project = drpProject.SelectedValue == "" ? 0 : Convert.ToInt64(drpProject.SelectedValue);

            String BasedCountry = drpBasedCountry.SelectedValue == "" ? "" : drpBasedCountry.SelectedValue;
            Int64 credit = !String.IsNullOrEmpty(drpCreditOption.SelectedValue) ? Convert.ToInt64(drpCreditOption.SelectedValue) : 0;
            Int64 pLocation = (drpLocation.SelectedValue == "0" || drpLocation.SelectedValue == "" || drpLocation.SelectedValue == " ") ? 0 : Convert.ToInt64(drpLocation.SelectedValue);
            //---end


            if (d2 < d1 || d1 > d2)
            {
                alertMessage("To Date Must be greater then From Date", true);
                return;
            }

            // ------------------------------------------------------------

            if (hdnReportName.Value.ToLower() == "trialbalancereport")
            {
                string DBCR = "";
                Report_TrailBalanceReport(DBCR, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "productstockreport")
            {
                Report_ProductStockReport(LoginUserID, pProductGroup, pBrand);
            }
            else if (hdnReportName.Value.ToLower() == "rojmel")
            {
                Report_Rojmel(d5, LoginUserID);
            }
            else if (hdnReportName.Value.ToLower() == "report_stock_new")
            {
                Report_Stock_New(LoginUserID, d3, d4);
            }
            else if (hdnReportName.Value.ToLower() == "report_stock")
            {
                Report_Stock(d5, LoginUserID);
            }
            else if (hdnReportName.Value.ToLower() == "pettycashreport")
            {
                Report_PettyCashReport(LoginUserID, d3, d4, FixedLedger, FixedLedgerName);
            }
            if (hdnReportName.Value.ToLower() == "cashbook")
            {
                //DateTime strtDt = Convert.ToDateTime("2020/09/01");
                //DateTime endDt = Convert.ToDateTime("2020/12/31");
                //Int64 Ac = 40194;
                string nt = "cash";
                Report_CashBook(LoginUserID, d3, d4, FixedLedger, nt);
            }
            else if (hdnReportName.Value.ToLower() == "bankbook")
            {
                //DateTime strtDt = Convert.ToDateTime("2020/09/01");
                //DateTime endDt = Convert.ToDateTime("2020/12/31");
                //Int64 Ac = 40193;
                string nt = "bank";
                Report_BankBook(LoginUserID, d3, d4, FixedLedger, nt);
            }
            else if (hdnReportName.Value.ToLower() == "bankvoucherreport")
            {
                Report_BankVoucher(LoginUserID, CustomerId, credit, d3, d4);
            }
            else if (hdnReportName.Value.ToLower() == "jv")
            {
                //DateTime strtDt = Convert.ToDateTime("2020/01/01");
                //DateTime endDt = Convert.ToDateTime("2020/12/31");
                Report_JV(LoginUserID, d3, d4);
            }

            else if (hdnReportName.Value.ToLower() == "tripbyvehicle")
            {
                //DateTime strtDt = Convert.ToDateTime("2020/01/01");
                //DateTime endDt = Convert.ToDateTime("2020/12/10");
                Report_Trip(pReportType, d3, d4, LoginUserID, EmployeeID, VehicleID);
            }
            else if (hdnReportName.Value.ToLower() == "tripbydriver")
            {
                //DateTime strtDt = Convert.ToDateTime("2020/01/01");
                //DateTime endDt = Convert.ToDateTime("2020/12/10");
                Report_TripByDriver(pReportType, d3, d4, LoginUserID, EmployeeID, VehicleID);
            }
            
            //Report_TripByDriver
            //else if (hdnReportName.Value.ToLower() == "tripbyvehicle" || hdnReportName.Value.ToLower() == "tripbydriver")
            //{
            //    //DateTime strtDt = Convert.ToDateTime("2020/01/01");
            //    //DateTime endDt = Convert.ToDateTime("2020/12/10");
            //    Report_Trip(pReportType, d3, d4, LoginUserID);
            //}


            else if (hdnReportName.Value.ToLower() == "employeeactivity")
            {
                Report_EmployeeActivity(d1, d2, EmployeeID);
            }
            else if (hdnReportName.Value.ToLower() == "inquirylist" || hdnReportName.Value.ToLower() == "inquirydetail")
            {
                if(hdnSerialKey.Value== "ECO3-2G21-TECH-3MRT")
                    Report_InquiryList(pReportType, d1, d2, LoginUserID, pSource, pStatus, EmployeeID, CustomerId, drpState.SelectedValue.ToString(), drpCity.SelectedValue.ToString());
                else
                    Report_InquiryList(pReportType, d1, d2, LoginUserID, pSource, pStatus, EmployeeID, CustomerId,"0","0"); 
            }
            else if (hdnReportName.Value.ToLower() == "quotationlist" || hdnReportName.Value.ToLower() == "quotationdetail" || hdnReportName.Value.ToLower() == "quotationbasedoncountry")
            {
                Report_QuotationList(hdnReportName.Value.ToLower(), d1, d2, LoginUserID, EmployeeID, CustomerId, BasedCountry);
            }
            else if (hdnReportName.Value.ToLower() == "salesorderlist" || hdnReportName.Value.ToLower() == "salesorderdetail")
            {
                Report_SalesOrderList(hdnReportName.Value.ToLower(), d1, d2, LoginUserID, EmployeeID, CustomerId, ApprovalStatus);
            }
            else if (hdnReportName.Value.ToLower() == "purchaseorderlist" || hdnReportName.Value.ToLower() == "purchaseorderdetail")
            {
                Report_PurchaseOrderList(hdnReportName.Value.ToLower(), d1, d2, LoginUserID, EmployeeID, CustomerId, ApprovalStatus, ProjectName);
            }
            else if (hdnReportName.Value.ToLower() == "report_users")
            {
                Report_UsersMaster(d1, d2, LoginUserID);
            }
            else if (hdnReportName.Value.ToLower() == "customerlist")
            {
                Report_Customer(LoginUserID, CustomerId, d1, d2, Status, CustomerType, State, City);
            }
            else if (hdnReportName.Value.ToLower() == "customermasterlist")
            {
                Report_Customer1(LoginUserID, CustomerId, d1, d2, Status, CustomerType, State, City);
            }
            else if (hdnReportName.Value.ToLower() == "financialtransection")
            {
                Report_GetFinancialTransection(TransCategory, LoginUserID, d1, d2);
            }
            else if (hdnReportName.Value.ToLower() == "report_designation")
            {
                Report_DesignationsMaster();
            }
            else if (hdnReportName.Value.ToLower() == "tododetaillist")
            {
                Report_ToDo(d3, d4, EmployeeID, TaskStatus, LoginUserID, 0, 0, Priority);
            }
            else if (hdnReportName.Value.ToLower() == "report_orgstructure")
            {
                Report_OrganizationStructureMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_orgemployee")
            {
                Report_OrganizationEmployeeMaster(LoginUserID);
            }
            else if (hdnReportName.Value.ToLower() == "report_orgtypes")
            {
                Report_OrgTypesMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_state")
            {
                Report_StateMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_city")
            {
                Report_CityMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_brand")
            {
                Report_BrandMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_productgroup")
            {
                Report_ProductGroupMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_product")
            {
                Report_ProductMaster();
            }
            else if (hdnReportName.Value.ToLower() == "report_suppliermaterialstatus")
            {
                Report_SupplierMaterialStatusList("detail", Month, Year);
            }
            else if (hdnReportName.Value.ToLower() == "report_brandwiseproductgroupwiseproduct")
            {
                Report_BrandWiseProductGroupWiseProductMaster(pBrand, pProductGroup);
            }
            else if (hdnReportName.Value.ToLower() == "salestargetlist")
            {
                Report_SalesTarget(d1, d2, LoginUserID, EmployeeID);
            }
            else if (hdnReportName.Value.ToLower() == "report_roles")
            {
                Report_RolesMasterList();
            }
            else if (hdnReportName.Value.ToLower() == "externalinquirydetail")
            {
                Report_ExternalInquiryList(d2, d1, EmployeeID, LeadStatus, Reason, DurationType);
            }
            else if (hdnReportName.Value.ToLower() == "dailyactivitylist")
            {
                Report_DailyActivity(d2, d1, EmployeeID, LeadStatus, Reason, DurationType);
            }
            else if (hdnReportName.Value.ToLower() == "telecallerinquiry" || hdnReportName.Value.ToLower() == "monthlyworking")
            {
                Report_TeleInquiryList(d2, d1, EmployeeID, LeadStatus1, Reason, InqTele);
            }
            else if (hdnReportName.Value.ToLower() == "attendancelist")
            {
                Report_AttandanceList(0, LoginUserID, EmployeeID, d1, d2);
            }
            else if (hdnReportName.Value.ToLower() == "dailyactivity") 
            {
                Report_GetDailyActivityListByUser(LoginUserID, EmployeeID, d1, d2, TaskCategory);
            }
            else if (hdnReportName.Value.ToLower() == "complaintlistreort")
            {
                Report_GetComplaintList(0, CustomerId, CompailntStaus, LoginUserID, d1, d2, EmployeeID, AssignToEmployeeID);
            }
            else if (hdnReportName.Value.ToLower() == "leaverequestlistreport")
            {
                Report_GetLeaveRequestListByUser(LoginUserID, d1, d2, EmployeeID, LeaveStatus);
            }
            else if (hdnReportName.Value.ToLower() == "expenselistreport")
            {
                Report_GetExpenseList(LoginUserID, EmployeeID, d1, d2, ExpenseTypeID);
            }
            if (hdnReportName.Value.ToLower() == "followupbycustomer" || hdnReportName.Value.ToLower() == "followupbyemployee")
            {
                Report_followup(hdnReportName.Value.ToLower(), d3, d4, LoginUserID, CustomerId.ToString(), pEmployeeID, pSource, pStatus, InqTele);
            }
            if (hdnReportName.Value.ToLower() == "quotationfromprojectname" || hdnReportName.Value.ToLower() == "salesorderfromprojectname" || hdnReportName.Value.ToLower() == "purchaseorderfromprojectname")
            {
                Report_ProjectBy(Project, d3, d4);
            }

            if (hdnReportName.Value.ToLower() == "salesbilllist")
            {
                if(hdnSerialKey.Value == "HONP-MEDF-9RTS-FG10" || hdnSerialKey.Value == "TWS3-RT90-E22O-K88P") 
                    Report_GetSalesBillList(0, LoginUserID, d1, d2, CustomerId.ToString(), drpLocation.SelectedValue);
                else
                    Report_GetSalesBillList(0, LoginUserID, d1, d2, CustomerId.ToString(),"0");
            }
            if (hdnReportName.Value.ToLower() == "detailsalesbilllist")
            {
                Report_GetSalesBillDetailList(LoginUserID, d1, d2, CustomerId.ToString());
            }
            if (hdnReportName.Value.ToLower() == "purchasebilllist")
            {
                Report_GetPurchaseBillList(LoginUserID, d1, d2, Convert.ToInt64(CustomerId), EmployeeID);
            }
            if (hdnReportName.Value.ToLower() == "detailpurchasesbilllist")
            {
                Report_GetPurchaseBillDetailList(LoginUserID, d1, d2, CustomerId, EmployeeID);
            }
            if (hdnReportName.Value.ToLower() == "customerledger")
            {
                Report_GetCustomerDetailLedgerList(CustomerId, LoginUserID, d1, d2);
            }
            if (hdnReportName.Value.ToLower() == "crnotereport")
            {
                Report_crnotereportList(LoginUserID, d3, d4,0,0);
            }

            if (hdnReportName.Value.ToLower() == "dbnotereport")
            {
                Report_dbnotereportList(LoginUserID, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "shiftreport")
            {
                Report_shiftreportList(ShiftCode, BasicPer, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "vehiclereport")
            {
                Report_vehiclereportList(LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "purchaseregister" || hdnReportName.Value.ToLower() == "productwisepurchase")
            {
                Report_purchaseregisterList(LoginUserID, pBrand, pProductGroup, CustomerId, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "statewisepurchase")
            {
                Report_StateWisePurReg(LoginUserID, City, State, CustomerId, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "customerwisesale" || hdnReportName.Value.ToLower() == "brandwisesale")
            {
                Report_SalesReg(LoginUserID, pBrand, pProductGroup, CustomerId, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "statecustomerwisesale")
            {
                Report_StateWiseSalesReg(LoginUserID, City, State, CustomerId, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "monthlysalessummary")
            {
                string Type;
                if (Convert.ToInt16(drpQtyAmt.SelectedValue) == 1)
                    Type = "Qty";
                else if (Convert.ToInt16(drpQtyAmt.SelectedValue) == 2)
                    Type = "GAmt";
                else
                    Type = "NAmt";
                Report_MonthlySalesSummary(Type, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "monthlypurchase")
            {
                string Type;
                if (Convert.ToInt16(drpQtyAmt.SelectedValue) == 1)
                    Type = "Qty";
                else if (Convert.ToInt16(drpQtyAmt.SelectedValue) == 2)
                    Type = "GAmt";
                else
                    Type = "NAmt";
                Report_MonthlyPurchase(Type, d3, d4, LoginUserID);
            }

            if (hdnReportName.Value.ToLower() == "salaryregister")
            {
                string type = drpAttendenceType.SelectedValue;
                Report_salaryRegister(EmployeeID, Month, Year, LoginUserID, type);
            }
            if (hdnReportName.Value.ToLower() == "minstockreport")
            {
                Report_minstockreportList(pBrand, pProductGroup, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "empwisemovement")
            {
                Report_Empwisemovement(LoginUserID, d3, d4, EmployeeID);
            }

            if (hdnReportName.Value.ToLower() == "userlogreport")
            {
                Report_UserLog(d3, d4, UserID);
            }
            
            if (hdnReportName.Value.ToLower() == "orderagainstpurchaseorder")
            {
                Report_OrderAgainstPurchase("", 0, 0, LoginUserID, CustomerId, d3, d4);
            }

            if (hdnReportName.Value.ToLower() == "orderagainstsalesorder")
            {
                Report_OrderAgainstSales("", 0, 0, LoginUserID, CustomerId, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "productionbysalesorder")
            {
                Report_ProductionBySO(CustomerId, d3, d4);
            }

            if (hdnReportName.Value.ToLower() == "complaintreport")
            {
                string ComplaintStatus = drpCompstatus.SelectedValue;
                Report_Complaint(ComplaintStatus, EmployeeID, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "complaintwithsign")
            {
                Report_ReportWithSign(CustomerId, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "outwardreport")
            {
                Report_Outward(0,CustomerId, LoginUserID, d3, d4,0,0);
            }
            if (hdnReportName.Value.ToLower() == "jobcardoutward")
            {
                Report_JobCardOutward(ProductId, CustomerId, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "jobcardinward")
            {
                Report_JobCardInward(ProductId, CustomerId, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "jobcardlist")
            {
                Report_Jobcard(ProductId, CustomerId, pLocation, d3, d4, LoginUserID);
            }
            
            //{
            //    Report_Visitor(pkID, LoginUserID, SearchKey, PageNo, PageSize);
            //}
            if (hdnReportName.Value.ToLower() == "materialmovement")
            {
                Report_MaterialMovementReport(ProductId, CustomerId, TransType, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "installfabric")
            {
                Report_InstallFabricReport(CustomerId, InsType, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "packinglist")
            {
                Report_PackingList(ProductId, CustomerId, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "grn")
            {
                Report_GRN(ProductId, CustomerId, d3, d4, LoginUserID);
            }
            if (hdnReportName.Value.ToLower() == "expensevoucher")
            {
                Report_GetExpenseVouvherList(LoginUserID, EmployeeID, d3, d4);
            }
            if (hdnReportName.Value.ToLower() == "overtime")
            {
                Report_GetOverTimeListByUser(LoginUserID, d3, d4, EmployeeID);
            }
            if (hdnReportName.Value.ToLower() == "visitormanagement")
            {
                Report_Visitor(0, LoginUserID, SearchKey, 0, 0, d3, d4);
            }

            if (hdnReportName.Value.ToLower() == "inwardreport")
            {
                Report_Inward(0, LoginUserID, CustomerId, d3, d4, 0, 0);
            }

            if (hdnReportName.Value.ToLower() == "inwardoutwardreport")
            {
                Report_InwardOutward(0, LoginUserID, CustomerId, d3, d4, 0, 0);
            }

            if (hdnReportName.Value.ToLower() == "pendinglistreport")
            {
                Report_PendingBill("", "Pending", "bill", "", 0, 0, 0, 0, 0, 0);
            }

            if (hdnReportName.Value.ToLower() == "debitcreditnote")
            {
                Report_DebitCreditNote(LoginUserID, CustomerId,d3, d4, 0, 0);
            }
        }

        #endregion

        #region Report Load Method Defination


        // =======================================================================
        // Report : Stock Report
        // =======================================================================
        public void Report_Stock_New(String pLoginUserID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.ProductStockReport> lstEntity = new List<Entity.ProductStockReport>();
                lstEntity = BAL.ReportMgmt.Report_Stock_New(pLoginUserID, FromDate, ToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_Stock_New.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@StartDate", "@LoginUserID", "@FromDate", "@ToDate", "@EndDate" };
                    string[] parmvalue = { FromDate.ToString("dd-MM-yyyy"), pLoginUserID, FromDate.ToString("dd-MM-yyyy"), ToDate.ToString("dd-MM-yyyy"), ToDate.ToString("dd-MM-yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : Leave Request List
        // =======================================================================
        public void Report_EmployeeActivity(Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, Int64 EmployeeID)
        {
            try
            {

                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.EmployeeActivity> lstEntity = new List<Entity.EmployeeActivity>();
                lstEntity = BAL.ReportMgmt.Report_EmployeeActivity(pFromDate, pToDate, EmployeeID);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_EmployeeActivity.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@PageNo", "@PageSize", "@TotalCount", "@Todate", "@FromDate", "@EmpID", "StartDate", "EndDate" };
                    string[] parmvalue = { "1", "9999", "1", nulldate(pToDate), nulldate(pFromDate), EmployeeID.ToString(), nulldate(pFromDate).ToString(), nulldate(pToDate) };
                    //crystalReport.SetParameterValue("@LoginUserID", pLoginUserID);
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Leave Request List
        // =======================================================================
        public void Report_GetOverTimeListByUser(string pLoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, Int64 EmployeeID)
        {
            try
            {

                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.OverTime> lstEntity = new List<Entity.OverTime>();
                lstEntity = BAL.ReportMgmt.Report_GetOverTimeListByUser(pLoginUserID, pFromDate, pToDate, EmployeeID);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_LeaveRequestList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@LoginUserID", "@EmpID", "@FromDate", "@ToDate", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { pLoginUserID, EmployeeID.ToString(), nulldate(pFromDate), nulldate(pToDate), nulldate(pFromDate), nulldate(pToDate), LoginEmployee };
                    crystalReport.SetParameterValue("@LoginUserID", pLoginUserID);
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }



        public void Report_GetExpenseVouvherList(string pLoginUserID, Int64 EmployeeID, DateTime Fromdate, DateTime Todate)
        {
            try
            {

                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.ExpenseVoucher_Report> lstEntity = new List<Entity.ExpenseVoucher_Report>();
                lstEntity = BAL.ReportMgmt.Report_GetExpenseVoucherList(pLoginUserID, EmployeeID, Fromdate, Todate);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/ExpenseVoucherReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "@LoginUserID", "@EmployeeID", "@FromDate", "@ToDate" };
                    string[] parmvalue = { "0", "L", "0", "0", pLoginUserID, EmployeeID.ToString(), Fromdate.ToString(), Todate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;

                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_PackingList(Int64 ProductID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, string LoginUserID)
        {
            try
            {
                List<Entity.PackingListReport> lstEntity = new List<Entity.PackingListReport>();
                lstEntity = BAL.ReportMgmt.PackingListReport(ProductID, CustomerID, FromDate, ToDate, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/PackingListReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductID", "@CustomerID", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { ProductID.ToString(), CustomerID.ToString(), FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_GRN(Int64 ProductID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, string LoginUserID)
        {
            try
            {
                List<Entity.GRNReport> lstEntity = new List<Entity.GRNReport>();
                lstEntity = BAL.ReportMgmt.GRNReport(ProductID, CustomerID, FromDate, ToDate, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/GRNReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductID", "@CustomerID", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { ProductID.ToString(), CustomerID.ToString(), FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_InstallFabricReport(Int64 CustomerID, string InspectType, string LoginUserID)
        {
            try
            {
                List<Entity.InstallFabricReport> lstEntity = new List<Entity.InstallFabricReport>();
                lstEntity = BAL.ReportMgmt.InstallFabricReport(CustomerID, InspectType, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/InstallFabricReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@CustomerID", "@InspectType", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { CustomerID.ToString(), InspectType, LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        public void Report_MaterialMovementReport(Int64 ProductID, Int64 CustomerID, string TransType, DateTime FromDate, DateTime ToDate, string LoginUserID)
        {
            try
            {
                List<Entity.MaterialMovementReport> lstEntity = new List<Entity.MaterialMovementReport>();
                lstEntity = BAL.ReportMgmt.MaterialMovementReport(ProductID, CustomerID, TransType, FromDate, ToDate, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/MaterialMovementReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductID", "@CustomerID", "@TransType", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { ProductID.ToString(), CustomerID.ToString(), TransType, FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        public void Report_JobCardInward(Int64 ProductID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, string LoginUserID)
        {
            try
            {
                List<Entity.JobCardInwardReport> lstEntity = new List<Entity.JobCardInwardReport>();
                lstEntity = BAL.ReportMgmt.JobCardInwardReport(ProductID, CustomerID, FromDate, ToDate, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/JobCardInwardReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductID", "@CustomerID", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { ProductID.ToString(), CustomerID.ToString(), FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        public void Report_JobCardOutward(Int64 ProductID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, string LoginUserID)
        {
            try
            {
                List<Entity.JobCardOutwardReport> lstEntity = new List<Entity.JobCardOutwardReport>();
                lstEntity = BAL.ReportMgmt.JobCardOutwardReport(ProductID, CustomerID, FromDate, ToDate, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/JobCardOutwardReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductID", "@CustomerID", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { ProductID.ToString(), CustomerID.ToString(), FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_Outward(Int64 pkID, Int64 CustomerID, string LoginUserID, DateTime FromDate, DateTime ToDate, Int64 PageNo, Int64 PageSize)
        {
            try
            {
                List<Entity.OutwardReport> lstEntity = new List<Entity.OutwardReport>();
                lstEntity = BAL.ReportMgmt.OutwardReport(pkID,CustomerID, LoginUserID, FromDate, ToDate, PageNo, PageSize);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Rreport_Outward.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = {"@pkID","@CustomerID", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize" };
                    string[] parmvalue = {"0",CustomerID.ToString(), FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0"};
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_ReportWithSign(Int64 CustomerID, string LoginUserID)
        {
            try
            {
                List<Entity.ReportWithSign> lstEntity = new List<Entity.ReportWithSign>();
                lstEntity = BAL.ReportMgmt.ComplainWithSign(CustomerID, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/CustomerComplaintWithSign.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@CustomerID", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { CustomerID.ToString(), LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        // =======================================================================
        // Report : Employee wise Movement Report
        // =======================================================================
        public void Report_Empwisemovement(string pLoginUserID, DateTime FromDate, DateTime ToDate, Int64 EmployeeID)
        {
            try
            {
                List<Entity.EmpWiseMatMovement> lstEntity = new List<Entity.EmpWiseMatMovement>();
                lstEntity = BAL.ReportMgmt.EmpMovementRegister(pLoginUserID, FromDate, ToDate, EmployeeID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/EmpWiseMovement.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@FromDate", "@ToDate", "@PageNo", "@PageSize", "@TotalCount", "@StartDate", "@EndDate" };
                    string[] parmvalue = { pLoginUserID, FromDate.ToString(), ToDate.ToString(), "0", "0", "0", FromDate.ToString("dd-MM-yyy"), ToDate.ToString("dd-MM-yyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //********************************************************************************************************************** 
        //  REPORT METHOD SIGNATURE FOR : USER LOG REPORT UserLog_Report
        //********************************************************************************************************************** 

        public void Report_UserLog(DateTime FromDate, DateTime ToDate, Int64 UserID)
        {
            try
            {
                List<Entity.USerLog_Report> lstEntity = new List<Entity.USerLog_Report>();
                lstEntity = BAL.ReportMgmt.UserLog_Report(FromDate, ToDate,UserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/UserLog_Report.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@UserID" };
                    string[] parmvalue = { FromDate.ToString(), ToDate.ToString(), UserID.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //========================================================================================================================
        //  REPORT METHOD SIGNATURE FOR : Order Against Purchase Order Detail
        //========================================================================================================================

        public void Report_OrderAgainstPurchase(string ViewType, Int64 Month, Int64 Year, string LoginUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.OrderToPurchaseOrderDetail> lstEntity = new List<Entity.OrderToPurchaseOrderDetail>();
                lstEntity = BAL.ReportMgmt.OrderAgainstPurchase_Report(ViewType, Month, Year, LoginUserID, CustomerID, FromDate, ToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_PurchaseOrderAgainstOutward.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ViewType", "@Month", "@Year","@LoginUserID", "@CustomerID","@FromDate", "@ToDate" };
                    string[] parmvalue = { ViewType, Month.ToString(),Year.ToString(), LoginUserID, CustomerID.ToString(),FromDate.ToString(), ToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //========================================================================================================================
        //  REPORT METHOD SIGNATURE FOR : Order Against Sales Order Detail
        //========================================================================================================================

        public void Report_OrderAgainstSales(string ViewType, Int64 Month, Int64 Year, string LoginUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.OrderToSalesOrderDetail> lstEntity = new List<Entity.OrderToSalesOrderDetail>();
                lstEntity = BAL.ReportMgmt.OrderAgainstSales_Report(ViewType, Month, Year, LoginUserID, CustomerID, FromDate, ToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_SalesOrderAgainstOutward.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ViewType", "@Month", "@Year", "@LoginUserID", "@CustomerID", "@FromDate", "@ToDate" };
                    string[] parmvalue = { ViewType, Month.ToString(), Year.ToString(), LoginUserID, CustomerID.ToString(), FromDate.ToString(), ToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //========================================================================================================================
        //  REPORT METHOD SIGNATURE FOR : Production By SO Report
        //========================================================================================================================

        public void Report_ProductionBySO(Int64 CustomerID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.ProductionBySO_Report> lstEntity = new List<Entity.ProductionBySO_Report>();
                lstEntity = BAL.ReportMgmt.ProductionBySO_Report(CustomerID, FromDate, ToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_ProductionBySO.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@CustomerID", "@FromDate", "@ToDate" };
                    string[] parmvalue = { CustomerID.ToString(), FromDate.ToString(), ToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================================================================
        //  Report Method Signature : Quotation List By Project List Report (Quotation,SalesOrder,PurchaseOrder) From ProjectName
        // =======================================================================================================================
        public void Report_ProjectBy( Int64 Project, DateTime Fromdate, DateTime Todate)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();
                int totalrecord = 10;
                List<Entity.PurchaseOrderByProjectList> lstEntity = new List<Entity.PurchaseOrderByProjectList>();
                if (hdnReportName.Value.ToLower() == "salesorderfromprojectname") //telecallerinquiry
                    lstEntity = BAL.ReportMgmt.SalesOrderByProjectList( Project,Fromdate, Todate);
                else if (hdnReportName.Value.ToLower() == "purchaseorderfromprojectname")
                    lstEntity = BAL.ReportMgmt.PurchaseOrderByProjectList(Project, Fromdate, Todate);
                else if (hdnReportName.Value.ToLower() == "quotationfromprojectname")
                    lstEntity = BAL.ReportMgmt.QuotationByProjectlist(Project, Fromdate, Todate);
                if (lstEntity.Count > 0)
                {
                    if (hdnReportName.Value.ToLower() == "salesorderfromprojectname")
                        crystalReport.Load(Server.MapPath("~/Reports/ProjectWiseSalesOeder_Report.rpt")); //No
                    else if (hdnReportName.Value.ToLower() == "purchaseorderfromprojectname")
                        crystalReport.Load(Server.MapPath("~/Reports/ProjectWisePurchase_Report.rpt")); //Done
                    else if (hdnReportName.Value.ToLower() == "quotationfromprojectname")
                        crystalReport.Load(Server.MapPath("~/Reports/ProjectWiseQuotation_Report.rpt")); // Done
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@ProjectName", "@FromDate", "@Todate" };
                    string[] parmvalue = { Project.ToString(), Fromdate.ToString(), Todate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }
        // =======================================================================
        // Report : Minimum Stock Report
        // =======================================================================
        public void Report_minstockreportList(string BrandID, string ProductGroupID, string pLoginUserID)
        {
            try
            {
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                lstEntity = BAL.ReportMgmt.ProductMinStockList(BrandID, ProductGroupID, pLoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/MinStockLevel.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@BrandID", "@ProductGroupID", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { BrandID.ToString(), ProductGroupID.ToString(), pLoginUserID, "0", "0", "0", };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Complaint Report
        // =======================================================================
        public void Report_Complaint(string ComplaintStatus, Int64 EmployeeID, DateTime FromDate, DateTime ToDate, string pLoginUserID)
        {
            try
            {
                List<Entity.CompliantReport> lstEntity = new List<Entity.CompliantReport>();
                lstEntity = BAL.ReportMgmt.ComplaintReport(ComplaintStatus, EmployeeID, FromDate, ToDate, pLoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/ComplaintReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ComplaintSatatus", "@EmployeeID", "@LoginUserID", "@FromDate", "@ToDate", "@PageNo", "@PageSize", "@TotalCount", "StartDate", "EndDate" };
                    string[] parmvalue = { ComplaintStatus, EmployeeID.ToString(), pLoginUserID, FromDate.ToString(), ToDate.ToString(), "0", "0", "0", FromDate.ToString("dd/MM/yyyy"), ToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : Salary Register Master -- Maitri
        // =======================================================================
        public void Report_salaryRegister(Int64 EmployeeID, string Month, string Year, string pLoginUserID, string Type)
        {
            try
            {
                List<Entity.SalaryRegister> lstEntity = new List<Entity.SalaryRegister>();
                lstEntity = BAL.ReportMgmt.SalaryRegister(EmployeeID, Month, Year, pLoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    if (Type == "1")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_SalaryRegister.rpt"));
                    else
                        crystalReport.Load(Server.MapPath("~/Reports/Report_SalaryRegisterType2.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@EmpID", "@Month", "@YEAR", "@LoginUserID" };
                    string[] parmvalue = { EmployeeID.ToString(), Month.ToString(), Year.ToString(), pLoginUserID };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }



        // =======================================================================
        // Report : Purchase Monthly Master
        // =======================================================================

        public void Report_MonthlyPurchase(string Type, DateTime FromDt, DateTime ToDt, String pLoginUserID)
        {
            try
            {
                List<Entity.MonthlyPurchase> lstEntity = new List<Entity.MonthlyPurchase>();
                lstEntity = BAL.ReportMgmt.MonthlyPurchaseList(Type, FromDt, ToDt, pLoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_MonthlyPurchaseSummary.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@Type", "@FromDt", "@ToDt", "@LoginUserID", "StartDate", "EndDate" };
                    string[] parmvalue = { Type, FromDt.ToString(), ToDt.ToString(), pLoginUserID, FromDt.ToString("dd/MM/yyyy"), ToDt.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Monthly Sales Summary
        // =======================================================================
        public void Report_MonthlySalesSummary(String Type, DateTime d3, DateTime d4, String pLoginUserID)
        {
            try
            {
                List<Entity.MonthlySalesSummary> lstEntity = new List<Entity.MonthlySalesSummary>();
                lstEntity = BAL.ReportMgmt.Report_MonthlySalesSummary(Type, d3, d4, pLoginUserID);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_MonthlySalesSummary.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@Type", "@FromDt", "@ToDt", "@LoginUserID", "StartDate", "EndDate" };
                    string[] parmvalue = { Type, d3.ToString(), d4.ToString(), pLoginUserID, d3.ToString("dd/MM/yyyy"), d4.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Sales Register
        // =======================================================================
        public void Report_SalesReg(String pLoginUserID, String pBrand, String pProductGroup, Int64 CustomerId, DateTime pFromDate, DateTime pToDate)
        {
            try
            {
                List<Entity.SalesReport> lstEntity = new List<Entity.SalesReport>();
                lstEntity = BAL.ReportMgmt.Report_SalesReg(pLoginUserID, pBrand, pProductGroup, CustomerId, pFromDate, pToDate);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    if (hdnReportName.Value.ToLower() == "customerwisesale")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_CustomerWiseSales.rpt"));
                    else if (hdnReportName.Value.ToLower() == "brandwisesale")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_BrandWiseSales.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@BrandId", "@ProductGroupId", "@CustomerId", "@FromDate", "@ToDate", "StartDate", "EndDate" };
                    string[] parmvalue = { pLoginUserID, pBrand, pProductGroup, CustomerId.ToString(), pFromDate.ToString(), pToDate.ToString(), pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : StateWise Sales Register
        // =======================================================================
        public void Report_StateWiseSalesReg(String pLoginUserID, Int64 City, Int64 State, Int64 CustomerId, DateTime pFromDate, DateTime pToDate)
        {
            try
            {
                List<Entity.SalesReport> lstEntity = new List<Entity.SalesReport>();
                lstEntity = BAL.ReportMgmt.Report_StateWiseSalesReg(pLoginUserID, City, State, CustomerId, pFromDate, pToDate);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_StateWiseSales.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@BrandId", "@ProductGroupId", "@CustomerId", "@CityCode", "@StateCode", "@FromDate", "@ToDate", "StartDate", "EndDate" };
                    string[] parmvalue = { pLoginUserID, "0", "0", CustomerId.ToString(), City.ToString(), State.ToString(), pFromDate.ToString(), pToDate.ToString(), pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : State Register Master
        // =======================================================================

        public void Report_StateWisePurReg(String pLoginUserID, Int64 City, Int64 State, Int64 CustomerId, DateTime pFromDate, DateTime pToDate)
        {
            try
            {
                List<Entity.PurchaseReport> lstEntity = new List<Entity.PurchaseReport>();
                lstEntity = BAL.ReportMgmt.Report_StateWisePurReg(pLoginUserID, City, State, CustomerId, pFromDate, pToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_StateWisePurchase.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@BrandId", "@ProductGroupId", "@CustomerId", "@CityCode", "@StateCode", "@FromDate", "@ToDate", "StartDate", "EndDate" };
                    string[] parmvalue = { pLoginUserID, "0", "0", CustomerId.ToString(), City.ToString(), State.ToString(), pFromDate.ToString(), pToDate.ToString(), pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Purchase Register Master
        // =======================================================================
        public void Report_purchaseregisterList(string pLoginUserID, string BrandID, string ProductGroupId, Int64 CustomerId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.PurchaseReport> lstEntity = new List<Entity.PurchaseReport>();
                lstEntity = BAL.ReportMgmt.PurchaseReportList(pLoginUserID, BrandID, ProductGroupId, CustomerId, FromDate, ToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    if (hdnReportName.Value.ToLower() == "purchaseregister")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_CustomerWisePurchase.rpt"));
                    else if (hdnReportName.Value.ToLower() == "productwisepurchase")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_ProductWisePurchase.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@BrandID", "@ProductGroupId", "@CustomerId", "@FromDate", "@ToDate", "StartDate", "EndDate" };
                    string[] parmvalue = { pLoginUserID, BrandID, ProductGroupId, CustomerId.ToString(), d3.ToString(), d4.ToString(), d3.ToString(), d4.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : Vehicle Master
        // =======================================================================
        public void Report_vehiclereportList(string LoginUserID)
        {
            try
            {
                List<Entity.VehicleList> lstEntity = new List<Entity.VehicleList>();
                lstEntity = BAL.ReportMgmt.VehicleList(LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/VehicleReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { LoginUserID, "0", "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Shift Report
        // =======================================================================
        public void Report_shiftreportList(Int64 ShiftCode, string BasicPer, string pLoginUserID)
        {
            try
            {
                List<Entity.ShiftReport> lstEntity = new List<Entity.ShiftReport>();
                lstEntity = BAL.ReportMgmt.ShiftReportList(ShiftCode, BasicPer, pLoginUserID);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/ShiftReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ShiftCode", "@BasicPer", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount" };
                    string[] parmvalue = { ShiftCode.ToString(), BasicPer, pLoginUserID, "0", "0", "0", };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : Cash Book Report
        // =======================================================================
        public void Report_CashBook(String pLoginUserID, DateTime pFromDate, DateTime pToDate, Int64 Ac, String nt)
        {
            try
            {
                List<Entity.CashBook> lstEntity = new List<Entity.CashBook>();
                lstEntity = BAL.ReportMgmt.Report_CashBook(pLoginUserID, pFromDate, pToDate, Ac, nt);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_CashBookRpt.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@FromDate", "@ToDate", "@CustID", "@NT", "StartDate", "EndDate", "NatTran" };
                    string[] parmvalue = { pLoginUserID, pFromDate.ToString(), pToDate.ToString(), Ac.ToString(), nt, pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy"), nt };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Bank Book Report 
        // =======================================================================
        public void Report_BankBook(String pLoginUserID, DateTime pFromDate, DateTime pToDate, Int64 Ac, String nt)
        {
            try
            {
                List<Entity.BankBook> lstEntity = new List<Entity.BankBook>();
                lstEntity = BAL.ReportMgmt.Report_BankBook(pLoginUserID, pFromDate, pToDate, Ac, nt);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_CashBookRpt.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@FromDate", "@ToDate", "@CustID", "@NT", "StartDate", "EndDate", "NatTran" };
                    string[] parmvalue = { pLoginUserID, pFromDate.ToString(), pToDate.ToString(), Ac.ToString(), nt, pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy"), nt };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //=======================================================================
        //Report : Bank Voucher Report
        //=======================================================================

        public void Report_BankVoucher(string pLoginUserID, Int64 pCustomerID, Int64 Credit, DateTime pFromDate, DateTime pToDate)
        {
            try
            {
                List<Entity.BankVoucher> lstEntity = new List<Entity.BankVoucher>();
                lstEntity = BAL.ReportMgmt.Report_BankVoucher(pLoginUserID, pCustomerID, Credit, pFromDate, pToDate);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_BankVoucher_New.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserId", "@CustomerID", "@Credit", "@FromDate", "@ToDate" };
                    string[] parmvalue = { pLoginUserID, pCustomerID.ToString(), Credit.ToString(), pFromDate.ToString(), pToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Journal Voucher
        // =======================================================================
        public void Report_JV(String pLoginUserID, DateTime pFromDate, DateTime pToDate)
        {
            try
            {
                List<Entity.JournalVoucherReport> lstEntity = new List<Entity.JournalVoucherReport>();
                lstEntity = BAL.ReportMgmt.Report_JV(pLoginUserID, pFromDate, pToDate);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_JournalVoucher.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@FromDate", "@ToDate", "StartDate", "EndDate" };
                    string[] parmvalue = { pLoginUserID, pFromDate.ToString(), pToDate.ToString(), pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Debit Note Master 
        // =======================================================================
        public void Report_dbnotereportList(string LoginUserID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.DBNote> lstEntity = new List<Entity.DBNote>();
                lstEntity = BAL.ReportMgmt.DBNote(LoginUserID, FromDate, ToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/DBNote.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@StartTime", "@EndTime" };
                    string[] parmvalue = { FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0", FromDate.ToString("dd/MM/yyyy"), ToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : Credit Note Report
        // =======================================================================

        public void Report_crnotereportList(string pLoginUserID, DateTime FromDate, DateTime ToDate, Int64 PageNo, Int64 PageSize)
        {
            try
            {
                List<Entity.CRNote> lstEntity = new List<Entity.CRNote>();
                lstEntity = BAL.ReportMgmt.CRNote(pLoginUserID, FromDate, ToDate, PageNo, PageSize);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/CRNote.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@StartDate", "@EndDate" };
                    string[] parmvalue = { FromDate.ToString(), ToDate.ToString(), pLoginUserID, "0", "0", FromDate.ToString(), ToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        // =======================================================================
        // Report : Petty Cashg Report
        // =======================================================================

        public void Report_PettyCashReport(String pLoginUserID, DateTime pFromDate, DateTime pToDate, Int64 FixedLedger, string FixedLedgerName)
        {
            try
            {
                List<Entity.PettyCashReport> lstEntity = new List<Entity.PettyCashReport>();
                lstEntity = BAL.ReportMgmt.PettyCashReportList(pLoginUserID, pFromDate, pToDate, FixedLedger);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/PettyCashReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@CustomerID", "@LoginUserID", "@PageNo", "@PageSize", "StartDate", "EndDate", "CustomerName" };
                    string[] parmvalue = { pFromDate.ToString(), pToDate.ToString(), FixedLedger.ToString(), pLoginUserID, "0", "0", pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy"), FixedLedgerName };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        // =======================================================================
        // Report : Trail Balance Report Report
        // =======================================================================

        public void Report_TrailBalanceReport(string DBCR, string LoginUserID)
        {
            try
            {
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

                List<Entity.TrialBalanceReport> lstEntity = new List<Entity.TrialBalanceReport>();
                lstEntity = BAL.ReportMgmt.TrialBalanceReportList(DBCR, loginuserid);
                string Heading = "";
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    if (tmpSerialKey == "ECO3-2G21-TECH-3MRT")     //ECOTECH 
                        crystalReport.Load(Server.MapPath("~/Reports/TrialBalanceReport_Landscape.rpt"));
                    else
                        crystalReport.Load(Server.MapPath("~/Reports/TrialBalanceReport.rpt"));


                    crystalReport.SetDataSource(lstEntity);

                    CrystalReportViewer1.ReportSource = crystalReport;
                    if (DBCR == "CR")
                    {
                        Heading = "Creditor Report";
                    }
                    else if (DBCR == "DB")
                    {
                        Heading = "Debtor Report";
                    }
                    else
                    {
                        Heading = "Trial Balance Report";
                    }
                    string[] parm = { "@DBCR", "@LoginUserID", "@PageNo", "@PageSize", "Heading" };
                    string[] parmvalue = { DBCR, LoginUserID, "0", "0", Heading };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Product Wise Stock Report
        // =======================================================================
        public void Report_ProductStockReport(string LoginUserID, string ProductGroupName, string BrandName)
        {
            try
            {
                List<Entity.ProductStockReport> lstEntity = new List<Entity.ProductStockReport>();
                lstEntity = BAL.ReportMgmt.ProductStockReportList(loginuserid, ProductGroupName, BrandName);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/ProductStockReport.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@ProductGroupName", "@BrandName", "@PageNo", "@PageSize" };
                    string[] parmvalue = { LoginUserID, ProductGroupName, BrandName, "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Rojmel Report
        // =======================================================================
        public void Report_Rojmel(DateTime pToDate, String pLoginUserID)
        {
            try
            {
                List<Entity.Rojmel> lstEntity = new List<Entity.Rojmel>();
                lstEntity = BAL.ReportMgmt.Report_Rojmel(pToDate, pLoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_Rojmel.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ToDate", "@LoginUserID", "@StartDate" };
                    string[] parmvalue = { pToDate.ToString(), pLoginUserID, nulldate(pToDate) };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        // =======================================================================
        // Report : Stock Report
        // =======================================================================
        public void Report_Stock(DateTime pToDate, String pLoginUserID)
        {
            try
            {
                List<Entity.ProductStockReport> lstEntity = new List<Entity.ProductStockReport>();
                lstEntity = BAL.ReportMgmt.Report_Stock(pLoginUserID, pToDate);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_Stock.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@LoginUserID", "@dtDate", "@StartDate" };
                    string[] parmvalue = { pLoginUserID, pToDate.ToString(), pToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
        //REPORT METHOD SIGNATURE : TRIP BY DRIVE
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

        public void Report_Trip(String pReportType, DateTime pFromDate, DateTime pToDate, String pLoginUserID, Int64 EmployeeID, Int64 VehicleID)
        {
            try
            {
                List<Entity.VehicleTrip> lstEntity = new List<Entity.VehicleTrip>();
                lstEntity = BAL.ReportMgmt.Report_Trip(pFromDate, pToDate, pLoginUserID, EmployeeID, VehicleID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_VehicleKMReading.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "StartDate", "EndDate", "@EmployeeID", "@VehicleID" };
                    string[] parmvalue = { pFromDate.ToString(), pToDate.ToString(), pLoginUserID, pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy"), EmployeeID.ToString(), VehicleID.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
        //REPORT METHOD SIGNATURE : DRIVER WISE KM
        //*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

        public void Report_TripByDriver(String pReportType, DateTime pFromDate, DateTime pToDate, String pLoginUserID, Int64 EmployeeID, Int64 VehicleID)
        {
            try
            {
                List<Entity.VehicleTrip> lstEntity = new List<Entity.VehicleTrip>();
                lstEntity = BAL.ReportMgmt.Report_Trip(pFromDate, pToDate, pLoginUserID, EmployeeID, VehicleID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_DriverWiseKM.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "StartDate", "EndDate", "@EmployeeID", "@VehicleID" };
                    string[] parmvalue = { pFromDate.ToString(), pToDate.ToString(), pLoginUserID, pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy"), EmployeeID.ToString(), VehicleID.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //// =======================================================================
        //// Report : Fleet Management
        //// =======================================================================
        //public void Report_Trip(String pReportType, DateTime pFromDate, DateTime pToDate, String pLoginUserID)
        //{
        //    try
        //    {
        //        List<Entity.VehicleTrip> lstEntity = new List<Entity.VehicleTrip>();
        //        lstEntity = BAL.ReportMgmt.Report_Trip(pFromDate, pToDate, pLoginUserID);
        //        if (lstEntity.Count > 0)
        //        {
        //            ReportDocument crystalReport = new ReportDocument();
        //            if (pReportType == "tripbyvehicle")
        //                crystalReport.Load(Server.MapPath("~/Reports/Report_VehicleKMReading.rpt"));
        //            else if (pReportType == "tripbydriver")
        //                crystalReport.Load(Server.MapPath("~/Reports/Report_DriverWiseKM.rpt"));
        //            crystalReport.SetDataSource(lstEntity);
        //            CrystalReportViewer1.ReportSource = crystalReport;
        //            string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "StartDate", "EndDate" };
        //            string[] parmvalue = { pFromDate.ToString(), pToDate.ToString(), pLoginUserID, pFromDate.ToString("dd/MM/yyyy"), pToDate.ToString("dd/MM/yyyy") };
        //            ReportBinder(parm, parmvalue, crystalReport);
        //            Session["report"] = crystalReport;
        //        }
        //        else
        //        {
        //            alertMessage(msg, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        alertMessage(ex.ToString(), false);
        //    }
        //}
        // =======================================================================
        // Report : Roles MAster
        // =======================================================================
        public void Report_RolesMasterList()
        {
            try
            {
                List<Entity.Roles> lstEntity = new List<Entity.Roles>();
                lstEntity = BAL.ReportMgmt.Report_RolesMasterList();

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_RolesMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@RoleCode", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Inquiry Register (Periodical)
        // =======================================================================
        public void Report_followup(String pReportType, DateTime pFromDate, DateTime pToDate, String pLoginUserID, String pCustomerID, String pEmployeeID, string pInquirySource, string pInquiryStatus, string InqTele)
        {
            try
            {
                List<Entity.Followup> lstEntity = new List<Entity.Followup>();
                lstEntity = BAL.ReportMgmt.Report_Getfollowup(pReportType, pFromDate, pToDate, pLoginUserID, pCustomerID, pEmployeeID, pInquirySource, pInquiryStatus, InqTele);


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    if (pReportType == "followupbycustomer")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_FollowUpByCustomer.rpt"));
                    else if (pReportType == "followupbyemployee")
                        if (drpEmployeeDate.SelectedValue == "Employee")
                            crystalReport.Load(Server.MapPath("~/Reports/Report_FollowUpbyEmployee.rpt"));
                        else
                            crystalReport.Load(Server.MapPath("~/Reports/Report_FollowUpbyEmployeeByDate.rpt"));

                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@CustomerID", "@EmployeeID", "@LoginUserID", "@ReportType", "StartDate", "EndDate", "EmployeeName", "@InquirySource", "@InquiryStatus", };
                    string[] parmvalue = { pFromDate.ToString(), pToDate.ToString(), pCustomerID.ToString(), pEmployeeID.ToString(), pLoginUserID, pReportType, nulldate(pFromDate), nulldate(pToDate), LoginEmployee, pInquirySource, pInquiryStatus };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }


        //ProjectWise_Report

        //public void Report_Projectise(String ReportType, Int64 ProjectName, DateTime FromDate, DateTime ToDate)
        //{
        //    try
        //    {
        //        List<Entity.PurchaseOrderByProjectList> lstEntity = new List<Entity.PurchaseOrderByProjectList>();
        //        lstEntity = BAL.ReportMgmt.ProjectWise_Report(ReportType, ProjectName, FromDate, ToDate);


        //        if (lstEntity.Count > 0)
        //        {
        //            ReportDocument crystalReport = new ReportDocument();
        //            if (ReportType == "purchaseorderfromprojectname")
        //                crystalReport.Load(Server.MapPath("~/Reports/ProjectWisePurchase_Report.rpt"));
        //            else if (ReportType == "quotationfromprojectname")
        //                crystalReport.Load(Server.MapPath("~/Reports/ProjectWiseQuotation_Report.rpt"));
        //                //else
        //                //    crystalReport.Load(Server.MapPath("~/Reports/Report_FollowUpbyEmployeeByDate.rpt"));

        //            crystalReport.SetDataSource(lstEntity);
        //            CrystalReportViewer1.ReportSource = crystalReport;
        //            string[] parm = { "@ReportType", "@ProjectName", "@FromDate", "@ToDate" };
        //            string[] parmvalue = { ReportType, ProjectName.ToString(),FromDate.ToString(), ToDate.ToString()};
        //            ReportBinder(parm, parmvalue, crystalReport);
        //            Session["report"] = crystalReport;

        //        }
        //        else
        //        {

        //            alertMessage(msg, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        alertMessage(ex.ToString(), false);
        //    }
        //}

        // =======================================================================
        // Report : Sales Target Register
        // =======================================================================
        public void Report_SalesTarget(Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, String pLoginUserID, int pEmployeeID)
        {
            try
            {
                List<Entity.SalesTarget> lstEntity = new List<Entity.SalesTarget>();
                lstEntity = BAL.ReportMgmt.Report_SalesTarget_Report(pFromDate, pToDate, pLoginUserID, pEmployeeID);



                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_SalesTarget.rpt"));

                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@EmployeeID", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pLoginUserID.ToString(), pEmployeeID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Inquiry Register (Periodical)
        // =======================================================================
        public void Report_InquiryList(String pReportType, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, String pLoginUserID, String pSource, String pStatus, int EmpID, Int64 CustomerID, string pStateCode, string pCityCode)
        {
            try
            {
                List<Entity.InquiryInfo_Report> lstEntity = new List<Entity.InquiryInfo_Report>();
                lstEntity = BAL.ReportMgmt.Report_InquiryList_Report(pReportType, pFromDate, pToDate, pLoginUserID, pSource, pStatus, EmpID, CustomerID, pStateCode, pCityCode);


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalreport = new ReportDocument();
                    if (pReportType == "inquirylist")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_InquiryList.rpt"));
                    else if (pReportType == "inquirydetail")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_InquiryDetailList.rpt"));

                    crystalreport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalreport;
                    string[] parm = { "@FromDate", "@ToDate", "@InquirySource", "@InquiryStatus", "@LoginUserID", "@ReportType", "@EmployeeID", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pSource, pStatus, pLoginUserID, pReportType, EmpID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalreport);
                    Session["report"] = crystalreport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Quotation Register (Periodical)
        // =======================================================================
        public void Report_QuotationList(String pReportType, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, String pLoginUserID, int EmpID, Int64 pCustomerID, String BasedCountry)
        {
            try
            {
                List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
                if (pReportType == "quotationbasedoncountry")
                    lstEntity = BAL.ReportMgmt.Report_QuotationList_Report("quotationlist", pFromDate, pToDate, pLoginUserID, EmpID, pCustomerID, BasedCountry);
                else
                    lstEntity = BAL.ReportMgmt.Report_QuotationList_Report(pReportType, pFromDate, pToDate, pLoginUserID, EmpID, pCustomerID, BasedCountry);



                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalreport = new ReportDocument();
                    if (pReportType == "quotationlist")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_QuotationList.rpt"));
                    else if (pReportType == "quotationdetail")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_QuotationDetailList.rpt"));
                    else if (pReportType == "quotationbasedoncountry")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_QuotationListBasedOnCountry.rpt"));

                    crystalreport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalreport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@ReportType", "@EmployeeID", "StartDate", "EndDate", "EmployeeName", "@CustomerID" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pLoginUserID, pReportType, EmpID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee, pCustomerID.ToString() };
                    ReportBinder(parm, parmvalue, crystalreport);
                    Session["report"] = crystalreport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : SalesOrder Register (Periodical)
        // =======================================================================
        public void Report_SalesOrderList(String pReportType, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, String pLoginUserID, int EmpID, Int64 pCustomerID, string ApprovalStatus)
        {
            try
            {
                List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
                lstEntity = BAL.ReportMgmt.Report_SalesOrderList_Report(pReportType, pFromDate, pToDate, pLoginUserID, EmpID, pCustomerID, ApprovalStatus);


                //Crystal report
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalreport = new ReportDocument();
                    if (pReportType == "salesorderlist")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_SalesOrderList.rpt"));
                    else if (pReportType == "salesorderdetail")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_SalesOrderDetailList.rpt"));

                    crystalreport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalreport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@ReportType", "@EmpID", "StartDate", "EndDate", "EmployeeName", "@CustomerID", "@ApprovalStatus" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pLoginUserID, pReportType, EmpID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee, pCustomerID.ToString(), ApprovalStatus };
                    ReportBinder(parm, parmvalue, crystalreport);
                    Session["report"] = crystalreport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }

        // =======================================================================
        // Report : PurchaseOrder Register (Periodical)
        // =======================================================================
        public void Report_PurchaseOrderList(String pReportType, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, String pLoginUserID, int EmpID, Int64 pCustomerID, string ApprovalStatus, Int64 ProjectName)
        {
            try
            {
                List<Entity.PurchaseOrder> lstEntity = new List<Entity.PurchaseOrder>();
                lstEntity = BAL.ReportMgmt.Report_PurchaseOrderList_Report(pReportType, pFromDate, pToDate, pLoginUserID, EmpID, pCustomerID, ApprovalStatus);//, ProjectName);


                //Crystal report
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalreport = new ReportDocument();
                    if (pReportType == "purchaseorderlist")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_PurchaseOrderList.rpt"));
                    else if (pReportType == "purchaseorderdetail")
                        crystalreport.Load(Server.MapPath("~/Reports/Report_PurchaseOrderDetailList.rpt"));

                    crystalreport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalreport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@ReportType", "@EmpID", "StartDate", "EndDate", "EmployeeName", "@CustomerID", "@ApprovalStatus" };//, "@ProjectID" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pLoginUserID, pReportType, EmpID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee, pCustomerID.ToString(), ApprovalStatus };//, ProjectName.ToString() };
                    ReportBinder(parm, parmvalue, crystalreport);
                    Session["report"] = crystalreport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }
        // =======================================================================
        // Report : Member Master Report 
        // =======================================================================
        public void Report_Customer(string pLoginUserID, Int64 CustomerID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, string CustomerStatus, string CustomerType, Int64 StateCode, Int64 CityCode)
        {
            try
            {
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();
                lstEntity = BAL.ReportMgmt.Report_CustomerMasterList(pLoginUserID, CustomerID, pFromDate, pToDate, CustomerStatus, CustomerType, StateCode, CityCode);



                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_CustomerMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "EmployeeName", "@LoginUserID", "@FromDate", "@ToDate", "@Status", "@CustomerType", "@CustomerID", "StartDate", "EndDate", "@StateCode", "@CityCode" };//{ "@CustomerID", "@ListMode", "@PageNo", "@PageSize" };
                    string[] parmvalue = { pLoginUserID, pLoginUserID, nulldate(pFromDate), nulldate(pToDate), CustomerStatus, CustomerType, CustomerID.ToString(), nulldate(pFromDate), nulldate(pToDate), StateCode.ToString(), CityCode.ToString() };//{ "", "L", "", "" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }
        
        //--------------------------------------------------------------------------------------------
        //Report : Member MasterReport_Customer By Sahil For Jacquel
        //--------------------------------------------------------------------------------------------

        public void Report_Customer1(string pLoginUserID, Int64 CustomerID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, string CustomerStatus, string CustomerType, Int64 StateCode, Int64 CityCode)
        {
            //try
            //{
            //    List<Entity.CustomerMasterList> lstEntity = new List<Entity.CustomerMasterList>();
            //    lstEntity = BAL.ReportMgmt.Report_Customer1(pLoginUserID, CustomerID, pFromDate, pToDate, CustomerStatus, CustomerType, StateCode, CityCode);



            //    if (lstEntity.Count > 0)
            //    {
            //        ReportDocument crystalReport = new ReportDocument();
            //        crystalReport.Load(Server.MapPath("~/Reports/Report_Customer.rpt"));
            //        crystalReport.SetDataSource(lstEntity);
            //        CrystalReportViewer1.ReportSource = crystalReport;
            //        string[] parm = { "EmployeeName", "@LoginUserID", "@FromDate", "@ToDate", "@Status", "@CustomerType", "@CustomerID", "StartDate", "EndDate", "@StateCode", "@CityCode" };
            //        string[] parmvalue = { pLoginUserID, pLoginUserID, nulldate(pFromDate), nulldate(pToDate), CustomerStatus, CustomerType, CustomerID.ToString(), nulldate(pFromDate), nulldate(pToDate), StateCode.ToString(), CityCode.ToString() };//{ "", "L", "", "" };
            //        ReportBinder(parm, parmvalue, crystalReport);
            //        Session["report"] = crystalReport;
            //    }
            //    else
            //    {

            //        alertMessage(msg, true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    alertMessage(ex.ToString(), false);
            //}

        }

        // =======================================================================
        // Report : Customer Ledger Master Report 
        // =======================================================================
        public void Report_GetFinancialTransection(string TransCategory, string LoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate)
        {
            try
            {
                List<Entity.FinancialTrans> lstEntity = new List<Entity.FinancialTrans>();
                //lstEntity = BAL.ReportMgmt.Report_CustomerLedger(TransCategory, LoginUserID);
                lstEntity = BAL.ReportMgmt.Report_GetFinancialTransection(TransCategory, LoginUserID, pFromDate, pToDate);

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_FinancialTransectionList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "@LoginUserID", "StartDate", "EndDate", "EmployeeName", "@TransCategory", "@FromDate", "@ToDate" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginUserID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee, TransCategory, nulldate(pFromDate), nulldate(pToDate) };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }

        // =======================================================================
        // Report : City Master Report 
        // =======================================================================
        public void Report_CityMaster()
        {
            try
            {
                List<Entity.City> lstEntity = new List<Entity.City>();
                lstEntity = BAL.ReportMgmt.Report_CityMasterList();
                ReportDataSource rds = new ReportDataSource("DataSet1", lstEntity);
                myViewer.LocalReport.ReportPath = "Report_CityMasterList.rdlc";
                myViewer.ProcessingMode = ProcessingMode.Local;
                myViewer.LocalReport.DataSources.Clear();
                myViewer.LocalReport.DataSources.Add(rds);
                myViewer.LocalReport.Refresh();
                if (lstEntity.Count > 0)
                {
                    rptviewer.Visible = true;
                }
                if (rptviewer.Visible == false)
                    alertMessage(msg, true);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Designations Master Report 
        // =======================================================================
        public void Report_DesignationsMaster()
        {
            try
            {
                List<Entity.Designations> lstEntity = new List<Entity.Designations>();
                lstEntity = BAL.ReportMgmt.Report_DesignationsMasterList();


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_DesignationsMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@DesigCode", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : EmailTemplate Master Report 
        // =======================================================================
        public void Report_EmailTemplateMaster()
        {
            try
            {
                List<Entity.EmailTemplate> lstEntity = new List<Entity.EmailTemplate>();
                //lstEntity = BAL.ReportMgmt.Report_EmailTemplateMasterList();
                ReportDataSource rds = new ReportDataSource("DataSet1", lstEntity);
                myViewer.LocalReport.ReportPath = "Report_EmailTemplateMasterList.rdlc";
                myViewer.ProcessingMode = ProcessingMode.Local;
                myViewer.LocalReport.DataSources.Clear();
                myViewer.LocalReport.DataSources.Add(rds);
                myViewer.LocalReport.Refresh();
                if (lstEntity.Count > 0)
                {
                    rptviewer.Visible = true;
                }
                if (rptviewer.Visible == false)
                    alertMessage(msg, true);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : OrganizationStructure Master Report 
        // =======================================================================
        public void Report_OrganizationStructureMaster()
        {
            try
            {
                List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
                lstEntity = BAL.ReportMgmt.Report_OrganizationStructureMasterList();

                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_OrganizationStructureMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@OrgCode", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }

        // =======================================================================
        // Report : ToDo Task Report 
        // =======================================================================
        public void Report_ToDo(DateTime FromDate, DateTime ToDate, Int64 EmployeeID, string TaskStatus, string pLoginUserID, Int64 PageNo, Int64 PageSize, string Priority)
        {
            try
            {
                List<Entity.ToDo> lstEntity = new List<Entity.ToDo>();
                lstEntity = BAL.ReportMgmt.ToDoReport(FromDate, ToDate, EmployeeID, TaskStatus, pLoginUserID, PageNo, PageSize, Priority);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/ToDo.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize", "@TotalCount", "StartDate", "EndDate" };
                    string[] parmvalue = { FromDate.ToString(), ToDate.ToString(), pLoginUserID, "0", "0", "0", FromDate.ToString("dd/MM/yyyy"), ToDate.ToString("dd/MM/yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : OrganizationEmployee Master Report 
        // =======================================================================
        public void Report_OrganizationEmployeeMaster(string LoginUserID)
        {
            try
            {
                List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
                lstEntity = BAL.ReportMgmt.Report_OrganizationEmployeeMasterList(LoginUserID);


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_OrganizationEmployeeMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : OrgTypes Master Report 
        // =======================================================================
        public void Report_OrgTypesMaster()
        {
            try
            {
                List<Entity.OrgTypes> lstEntity = new List<Entity.OrgTypes>();
                lstEntity = BAL.ReportMgmt.Report_OrgTypesMasterList();


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_OrgTypesMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }

        // =======================================================================
        // Report : State Master Report 
        // =======================================================================
        public void Report_StateMaster()
        {
            try
            {
                List<Entity.State> lstEntity = new List<Entity.State>();
                lstEntity = BAL.ReportMgmt.Report_StateMasterList();
                ReportDataSource rds = new ReportDataSource("DataSet1", lstEntity);
                myViewer.LocalReport.ReportPath = "Report_StateMasterList.rdlc";
                myViewer.ProcessingMode = ProcessingMode.Local;
                myViewer.LocalReport.DataSources.Clear();
                myViewer.LocalReport.DataSources.Add(rds);
                myViewer.LocalReport.Refresh();
                if (lstEntity.Count > 0)
                {
                    rptviewer.Visible = true;
                }
                if (rptviewer.Visible == false)
                    alertMessage(msg, true);
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Users Master Report 
        // =======================================================================
        public void Report_UsersMaster(Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, string pLoginUserID)
        {
            try
            {
                List<Entity.Users_Report> lstEntity = new List<Entity.Users_Report>();
                lstEntity = BAL.ReportMgmt.Report_UsersMasterList(pFromDate, pToDate, pLoginUserID);


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_UsersMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@UserID", "@ListMode", "@PageNo", "@PageSize", "EmployeeName", "@FromDate", "@ToDate", "StartDate", "EndDate" };
                    string[] parmvalue = { pLoginUserID, "L", "0", "0", LoginEmployee, nulldate(pFromDate), nulldate(pToDate), nulldate(pFromDate), nulldate(pToDate) };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Brand Master Report 
        // =======================================================================
        public void Report_BrandMaster()
        {
            try
            {
                List<Entity.Brand> lstEntity = new List<Entity.Brand>();
                lstEntity = BAL.ReportMgmt.Report_GetBrandList();


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_BrandMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Product Group Master Report 
        // =======================================================================
        public void Report_ProductGroupMaster()
        {
            try
            {
                List<Entity.ProductGroup> lstEntity = new List<Entity.ProductGroup>();
                lstEntity = BAL.ReportMgmt.Report_GetProductGroupList();



                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_ProductGroupMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "0", "0", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Product Master Report 
        // =======================================================================
        public void Report_ProductMaster()
        {
            try
            {
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                lstEntity = BAL.ReportMgmt.Report_GetProductList();


                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_ProductMasterList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "1", "1000", LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;

                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }
        public void Report_SalesTarget(string LoginUserID, string Day, string Month, string Year, string TargetType)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                int totrec;
                List<Entity.SalesTarget> lstEntity = new List<Entity.SalesTarget>();
                lstEntity = BAL.SalesTargetMgmt.GetSalesTargetListByTargetType(LoginUserID, 0, Convert.ToInt64(Month), Convert.ToInt64(Year), "A", 1, 99000, out totrec);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_SupplierMaterialStatus.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ViewType", "@Month", "@Year", "@LoginUserID" };
                    string[] parmvalue = { "detail", Month, Year, LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        public void Report_SupplierMaterialStatusList(string type, string Month, string Year)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();

                List<Entity.DispatchStatus> lstEntity = new List<Entity.DispatchStatus>();
                lstEntity = BAL.ReportMgmt.SupplierMaterialStatusList("detail", Convert.ToInt64(Month), Convert.ToInt64(Year), Session["LoginUserID"].ToString());


                if (lstEntity.Count > 0)
                {

                    crystalReport.Load(Server.MapPath("~/Reports/Report_SupplierMaterialStatus.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ViewType", "@Month", "@Year", "@LoginUserID" };
                    string[] parmvalue = { "detail", Month, Year, LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;


                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        // =======================================================================
        // Report : Brand Wise ProductGroup Wise Product List 
        // =======================================================================
        public void Report_BrandWiseProductGroupWiseProductMaster(string pBrand, string pProductGroup)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                lstEntity = BAL.ReportMgmt.Report_GetBrandWiseProductGroupWiseProductList(pProductGroup, pBrand);
                if (lstEntity.Count > 0)
                {

                    crystalReport.Load(Server.MapPath("~/Reports/Report_BrandWiseProductGroupWiseProductMaster.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductGroupId", "@BrandId", "EmployeeName" };
                    string[] parmvalue = { pProductGroup, pBrand, LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    //CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;


                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        /*
          public void Report_DebitCreditNote(string LoginUserID, Int64 CustomerID ,DateTime FromDate, DateTime ToDate, Int64 PageNo, Int64 PageSize)
        {
            try
            {
                List<Entity.DebitCreditNoteReport> lstEntity = new List<Entity.DebitCreditNoteReport>();
                lstEntity = BAL.ReportMgmt.DebitCreditNoteReport(LoginUserID, CustomerID,FromDate, ToDate, PageNo, PageSize);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_DebitCredit.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@CustomerID","@ToDate", "@LoginUserID", "@PageNo", "@PageSize"};
                    string[] parmvalue = {FromDate.ToString(), ToDate.ToString(), CustomerID.ToString(),LoginUserID, "0", "0"};
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
             */

        // =======================================================================
        // Report : External Inquiry List
        // =======================================================================
        public void Report_ExternalInquiryList(Nullable<DateTime> Todate, Nullable<DateTime> Fromdate, Int64 EmployeeID, string LeadStatus, Int64 Reason, Int64 DurationType)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();
                int totalrecord = 10;
                List<Entity.ExternalLeads_Report> lstEntity = new List<Entity.ExternalLeads_Report>();
                lstEntity = BAL.ReportMgmt.Report_GetExternalLeadList(1, 9999, out totalrecord, Todate, Fromdate, EmployeeID, LeadStatus, Reason, DurationType);
                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_ExternalInquiryList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@PageNo", "@PageSize", "@Todate", "@FromDate", "@EmpID", "@LeadStatus", "StartDate", "EndDate", "EmployeeName", "@Reason", "@DurationType" };
                    string[] parmvalue = { "1", "9999", nulldate(Todate), nulldate(Fromdate), EmployeeID.ToString(), LeadStatus, nulldate(Fromdate), nulldate(Todate), LoginEmployee, Reason.ToString(), DurationType.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }

        // =======================================================================
        // Report : External Inquiry List
        // =======================================================================
        public void Report_DailyActivity(Nullable<DateTime> Todate, Nullable<DateTime> Fromdate, Int64 EmpID, string LeadStatus, Int64 Reason, Int64 DurationType)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();
                int totalrecord = 10;
                List<Entity.ExternalLeadReport> lstEntity = new List<Entity.ExternalLeadReport>();
                lstEntity = BAL.ReportMgmt.ExternalLeadReport(1, 9999, out totalrecord, d3, d4, EmpID, LeadStatus, Reason, DurationType);
                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_DailyVisit.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@PageNo", "@PageSize", "@Todate", "@FromDate", "@EmpID", "@LeadStatus",  "@Reason", "@DurationType" };
                    string[] parmvalue = { "1", "9999", nulldate(Todate), nulldate(Fromdate), EmpID.ToString(), LeadStatus, Reason.ToString(), DurationType.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }

        // =======================================================================
        // Report : Telecaller Inquiry List
        // =======================================================================
        public void Report_TeleInquiryList(Nullable<DateTime> Todate, Nullable<DateTime> Fromdate, Int64 EmployeeID, string LeadStatus, Int64 Reason, String InqTele)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();
                int totalrecord = 10;
                List<Entity.ExternalLeads_Report> lstEntity = new List<Entity.ExternalLeads_Report>();
                if (hdnReportName.Value.ToLower() == "telecallerinquiry")
                    lstEntity = BAL.ReportMgmt.Report_GetTelecallerInquiryList(1, 9999, out totalrecord, Todate, Fromdate, EmployeeID, LeadStatus, Reason, InqTele);
                else if (hdnReportName.Value.ToLower() == "monthlyworking")
                    lstEntity = BAL.ReportMgmt.Report_MonthlyWorking(1, 9999, out totalrecord, Todate, Fromdate, EmployeeID, LeadStatus, Reason, InqTele);
                if (lstEntity.Count > 0)
                {
                    if (hdnReportName.Value.ToLower() == "telecallerinquiry")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_TelecallerInquiry.rpt"));
                    else if (hdnReportName.Value.ToLower() == "monthlyworking")
                        crystalReport.Load(Server.MapPath("~/Reports/Report_MonthlyWorking.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@PageNo", "@PageSize", "@Todate", "@FromDate", "@EmpID", "@LeadStatus", "@Reason", "@DurationType", "StartDate", "EndDate", "EmployeeName", "@InqTele" };
                    string[] parmvalue = { "1", "9999", nulldate(Todate), nulldate(Fromdate), EmployeeID.ToString(), LeadStatus, Reason.ToString(), "0", nulldate(Fromdate), nulldate(Todate), LoginEmployee, InqTele };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }
        // =======================================================================
        // Report : Attandance List
        // =======================================================================
        public void Report_AttandanceList(Int64 pkID, string LoginUserID, Int64 EmployeeID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.Attendance_Report> lstEntity = new List<Entity.Attendance_Report>();
                lstEntity = BAL.ReportMgmt.GetAttendanceList_Report(pkID, LoginUserID, EmployeeID, pFromDate, pToDate);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_AttendanceList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@pkId", "@LogInUserID", "@EmployeeID", "@FromDate", "@ToDate", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { pkID.ToString(), LoginUserID, EmployeeID.ToString(), nulldate(pFromDate), nulldate(pToDate), nulldate(pFromDate), nulldate(pToDate), LoginEmployee};
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Daily Activity List  
        // =======================================================================
        public void Report_GetDailyActivityListByUser(string LoginUserID, Int64 EmployeeID, Nullable<DateTime> pFromdate, Nullable<DateTime> pTodate, Int64 TaskCategory)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.DailyActivity> lstEntity = new List<Entity.DailyActivity>();
                lstEntity = BAL.ReportMgmt.Report_GetDailyActivityListByUser(LoginUserID, EmployeeID, pFromdate, pTodate, TaskCategory);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_DailyActivityList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@LoginUserID", "@EmployeeID", "@FromDate", "@ToDate", "StartDate", "EndDate", "EmployeeName", "@TaskCategoryID" };
                    string[] parmvalue = { LoginUserID, EmployeeID.ToString(), nulldate(pFromdate), nulldate(pTodate), nulldate(pFromdate), nulldate(pTodate), LoginEmployee, TaskCategory.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Complaint List
        // =======================================================================
        public void Report_GetComplaintList(Int64 pkID, Int64 CustomerID, string ComplaintStatus, string LoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, Int64 EmployeeID, Int64 AssignToEmployeeID)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.Complaint_Report> lstEntity = new List<Entity.Complaint_Report>();
                lstEntity = BAL.ReportMgmt.Report_GetComplaintList(pkID, CustomerID, ComplaintStatus, LoginUserID, pFromDate, pToDate, EmployeeID, AssignToEmployeeID);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_ComplainList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@pkID", "@CustomerID", "@ComplaintStatus", "@LoginUserID", "@FromDate", "@ToDate", "@EmployeeID", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { pkID.ToString(), CustomerID.ToString(), ComplaintStatus, LoginUserID, nulldate(pFromDate), nulldate(pToDate), EmployeeID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Leave Request List
        // =======================================================================
        public void Report_GetLeaveRequestListByUser(string pLoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, Int64 EmployeeID, string LeaveStatus)
        {
            try
            {

                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.LeaveRequest> lstEntity = new List<Entity.LeaveRequest>();
                lstEntity = BAL.ReportMgmt.Report_GetLeaveRequestListByUser(pLoginUserID, pFromDate, pToDate, EmployeeID, LeaveStatus);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_LeaveRequestList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@LoginUserID", "@EmpID", "@FromDate", "@ToDate", "@Status", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { pLoginUserID, EmployeeID.ToString(), nulldate(pFromDate), nulldate(pToDate), LeaveStatus, nulldate(pFromDate), nulldate(pToDate), LoginEmployee };
                    crystalReport.SetParameterValue("@LoginUserID", pLoginUserID);
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        // =======================================================================
        // Report : Expence List
        // =======================================================================
        public void Report_GetExpenseList(string pLoginUserID, Int64 EmployeeID, Nullable<DateTime> Fromdate, Nullable<DateTime> Todate, Int64 ExpenseTypeID)
        {
            try
            {

                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.Expense_Report> lstEntity = new List<Entity.Expense_Report>();
                lstEntity = BAL.ReportMgmt.Report_GetExpenseList(pLoginUserID, EmployeeID, Fromdate, Todate, ExpenseTypeID);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_ExpenseList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@pkID", "@ListMode", "@PageNo", "@PageSize", "@LoginUserID", "@EmployeeID", "@FromDate", "@ToDate", "@ExpenseTypeID", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { "0", "L", "1", "50000", pLoginUserID, EmployeeID.ToString(), nulldate(Fromdate), nulldate(Todate), ExpenseTypeID.ToString(), nulldate(Fromdate), nulldate(Todate), LoginEmployee };
                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;

                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_GetSalesBillList(Int64 pkID, string pLoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, string CustomerID, string pLocation)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                int totalrecord = 0;
                List<Entity.SalesBillReport> lstEntity = new List<Entity.SalesBillReport>();
                lstEntity = BAL.ReportMgmt.Report_GetSalesBillList(pkID, pLoginUserID, pFromDate, pToDate, CustomerID, pLocation, 1, 1000, out totalrecord);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_SalesBillList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@pkID", "@LoginUserID", "@FromDate", "@ToDate", "@CustomerID", "@PageNo", "@PageSize", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { "0", pLoginUserID, nulldate(pFromDate), nulldate(pToDate), pCustomerID, "1", "1000", nulldate(pFromDate), nulldate(pToDate), LoginEmployee };

                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }

            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_GetSalesBillDetailList(string pLoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, string pCustomerID)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                int totalrecord = 0;
                List<Entity.SalesBillDetail_Report> lstEntity = new List<Entity.SalesBillDetail_Report>();
                lstEntity = BAL.ReportMgmt.Report_GetSalesBillDetailList(pLoginUserID, pFromDate, pToDate, pCustomerID);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_SalesBillDetailList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@CustomerID", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pLoginUserID, pCustomerID, nulldate(pFromDate), nulldate(pToDate), LoginEmployee };

                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {

                alertMessage(ex.ToString(), false);
            }
        }

       public void Report_GetPurchaseBillList(string pLoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, Int64 CustomerID, Int64 EmployeeID)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                int totalrecord = 0;
                List<Entity.PurchaseBill> lstEntity = new List<Entity.PurchaseBill>();
                lstEntity = BAL.ReportMgmt.Report_GetPurchaseBillList(pLoginUserID, pFromDate, pToDate, CustomerID, EmployeeID.ToString());

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_PurchaseBillList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@pkID", "@LoginUserID", "@FromDate", "@ToDate", "@CustomerID", "@PageNo", "@PageSize", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { "0", pLoginUserID, nulldate(pFromDate), nulldate(pToDate), CustomerID.ToString(), "1", "1000", nulldate(pFromDate), nulldate(pToDate), LoginEmployee };

                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }

            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_GetPurchaseBillDetailList(string pLoginUserID, Nullable<DateTime> pFromDate, Nullable<DateTime> pToDate, Int64 CustomerID, Int64 EmployeeID)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();


                List<Entity.PurchaseBillDetail_Report> lstEntity = new List<Entity.PurchaseBillDetail_Report>();
                lstEntity = BAL.ReportMgmt.Report_GetPurchaseBillDetailList(pLoginUserID, pFromDate, pToDate, CustomerID, EmployeeID);

                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_PurchaseBillDetailList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@FromDate", "@ToDate", "@LoginUserID", "@CustomerID", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { nulldate(pFromDate), nulldate(pToDate), pLoginUserID, pCustomerID.ToString(), nulldate(pFromDate), nulldate(pToDate), LoginEmployee };

                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {

                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_GetCustomerDetailLedgerList(Int64 CustomerID, string pLoginUserID, Nullable<DateTime> Fromdate, Nullable<DateTime> Todate)
        {
            try
            {
                ReportDocument crystalReport = new ReportDocument();
                crystalReport = new ReportDocument();

                List<Entity.Customer> lstEntity = new List<Entity.Customer>();
                lstEntity = BAL.ReportMgmt.Report_GetCustomerDetailLedgerList(CustomerID, pLoginUserID, Fromdate, Todate);
                if (lstEntity.Count > 0)
                {
                    crystalReport.Load(Server.MapPath("~/Reports/Report_CustomerLedgerNew.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    string[] parm = { "@CustomerID", "@LoginUserID", "@FromDate", "@ToDate", "StartDate", "EndDate", "EmployeeName" };
                    string[] parmvalue = { pCustomerID.ToString(), pLoginUserID, nulldate(Fromdate), nulldate(Todate), nulldate(Fromdate), nulldate(Todate), LoginEmployee };

                    ReportBinder(parm, parmvalue, crystalReport);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    Session["report"] = crystalReport;
                }
                else
                {

                    alertMessage(msg, true);
                }

            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_Jobcard(Int64 ProductID, Int64 CustomerID, Int64 Location, DateTime FromDate, DateTime ToDate, string LoginUserID)
        {
            try
            {
                List<Entity.JobCardReport> lstEntity = new List<Entity.JobCardReport>();
                lstEntity = BAL.ReportMgmt.JobcardList(ProductID, CustomerID, FromDate, Location, ToDate, LoginUserID);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/JobCardTWS.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@ProductID", "@CustomerID", "@Location", "@FromDate", "@ToDate", "@LoginUserID", "@PageNo", "@PageSize" };
                    string[] parmvalue = { ProductID.ToString(), CustomerID.ToString(), Location.ToString(), FromDate.ToString(), ToDate.ToString(), LoginUserID, "0", "0" };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_DebitCreditNote(string LoginUserID, Int64 CustomerID ,DateTime FromDate, DateTime ToDate, Int64 PageNo, Int64 PageSize)
        {
            try
            {
                List<Entity.DebitCreditNoteReport> lstEntity = new List<Entity.DebitCreditNoteReport>();
                lstEntity = BAL.ReportMgmt.DebitCreditNoteReport(LoginUserID, CustomerID,FromDate, ToDate, PageNo, PageSize);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Report_DebitCredit.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@FromDate", "@CustomerID","@ToDate", "@LoginUserID", "@PageNo", "@PageSize"};
                    string[] parmvalue = {FromDate.ToString(), ToDate.ToString(), CustomerID.ToString(),LoginUserID, "0", "0"};
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_Visitor(Int64 pkID, string LoginUserID, string SearchKey, Int64 PageNo, Int64 PageSize, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<Entity.VisitorReport> lstEntity = new List<Entity.VisitorReport>();
                lstEntity = BAL.ReportMgmt.VisitorReport(pkID, LoginUserID, SearchKey, PageNo, PageSize, d3, d4);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/VisitorManagement.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@LoginUserID", "@SearchKey", "@PageNo", "@PageSize", "@FromDate", "@Todate" };
                    string[] parmvalue = { pkID.ToString(), LoginUserID.ToString(), SearchKey.ToString(), PageNo.ToString(), PageSize.ToString(), FromDate.ToString(), ToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
        public void Report_Inward(Int64 pkID, string LoginUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 PageNo, Int64 PageSize)
        {
            try
            {
                List<Entity.InwardReport> lstEntity = new List<Entity.InwardReport>();
                lstEntity = BAL.ReportMgmt.InwardReport(pkID, LoginUserID, CustomerID, d3, d4, PageNo, PageSize);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/Inward_Report.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@pkID", "@LoginUserID", "@CustomerID", "@PageNo", "@PageSize", "@FromDate", "@Todate" };
                    string[] parmvalue = { pkID.ToString(), LoginUserID.ToString(), CustomerID.ToString(), PageNo.ToString(), PageSize.ToString(), FromDate.ToString(), ToDate.ToString() };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //======================================================================================================================================
        // Method Signature : Pending Bill List Report.
        //======================================================================================================================================

        public void Report_PendingBill(string BillingCategory, string BillingStatus, string ByDateType, string AsOnDate, Int64 LM1, Int64 LM2, Int64 LM3, Int64 LM4, Int64 LM5, Int64 LM6)
        {
            try
            {
                List<Entity.PendingBillReport> lstEntity = new List<Entity.PendingBillReport>();
                lstEntity = BAL.ReportMgmt.PendingBillListReport(BillingCategory, BillingStatus, ByDateType, AsOnDate, LM1, LM2, LM3, LM4, LM5,LM6);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/PendingBillList.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@BillingCategory", "@BillingStatus", "@ByDateType", "@AsOnDate", "@LM1", "@LM2", "@LM3" , "@LM4" , "@LM5" , "@LM6" };
                    string[] parmvalue = { BillingCategory.ToString(), BillingStatus.ToString(), ByDateType.ToString(), AsOnDate.ToString(), LM1.ToString(), LM2.ToString(), LM3.ToString(), LM4.ToString(), LM5.ToString(), LM6.ToString()};
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        public void Report_InwardOutward(Int64 pkID, string LoginUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 PageNo, Int64 PageSize)
        {
            try
            {
                List<Entity.InwardOutwardReport> lstEntity = new List<Entity.InwardOutwardReport>();
                lstEntity = BAL.ReportMgmt.InwardOutwardReport(pkID, LoginUserID, CustomerID, d3, d4, PageNo, PageSize);
                if (lstEntity.Count > 0)
                {
                    ReportDocument crystalReport = new ReportDocument();
                    crystalReport.Load(Server.MapPath("~/Reports/InwardOutward_Report.rpt"));
                    crystalReport.SetDataSource(lstEntity);
                    CrystalReportViewer1.ReportSource = crystalReport;
                    string[] parm = { "@StartDate","@pkID", "@LoginUserID", "@CustomerID", "@PageNo", "@PageSize", "@FromDate", "@Todate", "@EndDate" };
                    string[] parmvalue = { FromDate.ToString("dd-MM-yyyy"), pkID.ToString(), LoginUserID.ToString(), CustomerID.ToString(), PageNo.ToString(), PageSize.ToString(), FromDate.ToString(), ToDate.ToString(), ToDate.ToString("dd-MM-yyyy") };
                    ReportBinder(parm, parmvalue, crystalReport);
                    Session["report"] = crystalReport;
                }
                else
                {
                    alertMessage(msg, true);
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }

        }
        #endregion

        #region Private Function


        //FUNCTION FOR DBLOGIN FOR CRYSTAL REPORT
        void setDBLOGONforREPORT(ConnectionInfo myconnectioninfo, ReportDocument reportDocument)
        {
            try
            {
                Tables tables = reportDocument.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
                {
                    TableLogOnInfos mytableloginfos = new TableLogOnInfos();
                    mytableloginfos = CrystalReportViewer1.LogOnInfo;
                    foreach (TableLogOnInfo myTableLogOnInfo in mytableloginfos)
                    {
                        myTableLogOnInfo.ConnectionInfo = myconnectioninfo;
                    }
                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        //BIND PARAMETRE TO CRYSTALREPORTVIEWER
        //void ReportBinder(string[] parm, string[] parmvalues, ReportDocument reportDocument)
        //{
        //    try
        //    {
        //        ParameterFields parameterFields = new ParameterFields();
        //        ParameterField parameterField = null;
        //        ParameterDiscreteValue parameterValue = null;

        //        for (int i = 0; i < parm.Length; i++)
        //        {
        //            //CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinitions crParameterdef;
        //            //crParameterdef = reportDocument.DataDefinition.ParameterFields;
        //            //foreach (CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition def in crParameterdef)
        //            //{
        //            //    if (def.Name.Equals(parm[i])) // check if parameter exists in report
        //            //    {
        //            //        reportDocument.SetParameterValue(parm[i], parmvalues[i]); // set the parameter value in the report
        //            //    }

        //            //}


        //            parameterField = new ParameterField();
        //            parameterValue = new ParameterDiscreteValue();
        //            parameterField.Name = parm[i];
        //            parameterValue.Value = parmvalues[i];
        //            parameterField.CurrentValues.Add(parameterValue);
        //            parameterFields.Add(parameterField);
        //        }
        //        CrystalReportViewer1.ParameterFieldInfo = parameterFields;


        //        string con = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;

        //        SqlConnectionStringBuilder scon = new SqlConnectionStringBuilder(con);
        //        ConnectionInfo myConnectionInfo = new ConnectionInfo();

        //        myConnectionInfo.ServerName = scon.DataSource;
        //        myConnectionInfo.DatabaseName = scon.InitialCatalog;
        //        myConnectionInfo.UserID = scon.UserID;
        //        myConnectionInfo.Password = scon.Password;
        //        setDBLOGONforREPORT(myConnectionInfo, reportDocument);



        //    }
        //    catch (Exception ex)
        //    {
        //        alertMessage(ex.ToString(), false);
        //    }


        //}

        void ReportBinder(string[] parm, string[] parmvalues, ReportDocument reportDocument)
        {
            ParameterFields parameterFields = new ParameterFields();
            ParameterField parameterField = null;
            ParameterDiscreteValue parameterValue = null;

            for (int i = 0; i < parm.Length; i++)
            {
                parameterField = new ParameterField();
                parameterValue = new ParameterDiscreteValue();
                parameterField.Name = parm[i];
                parameterValue.Value = parmvalues[i];
                parameterField.CurrentValues.Add(parameterValue);
                parameterFields.Add(parameterField);
                reportDocument.SetParameterValue(parm[i], parmvalues[i]);
            }
            CrystalReportViewer1.ParameterFieldInfo = parameterFields;


            string con = System.Configuration.ConfigurationManager.ConnectionStrings["StarsConnection"].ConnectionString;

            SqlConnectionStringBuilder scon = new SqlConnectionStringBuilder(con);
            ConnectionInfo myConnectionInfo = new ConnectionInfo();

            myConnectionInfo.ServerName = scon.DataSource;
            myConnectionInfo.DatabaseName = scon.InitialCatalog;
            myConnectionInfo.UserID = scon.UserID;
            myConnectionInfo.Password = scon.Password;
            setDBLOGONforREPORT(myConnectionInfo, reportDocument);




        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindState();
        }

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCity();
        }

        //ERROR OR WARNING MSG ALERT POP UP

        void alertMessage(string msg, bool warningheading)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopup();", true);
            if (warningheading == true)
            {
                this.lblModelHeading.Text = "Warning";
                CrystalReportViewer1.ReportSource = null;
                CrystalReportViewer1.RefreshReport();
            }
            else
            {
                this.lblModelHeading.Text = "Application Error <br />" + sysmsg;
            }
            this.lblMessage.Text = msg;

        }

        string nulldate(Nullable<DateTime> d1)
        {
            string fromdate;
            if (d1 == null)
            {
                fromdate = "1900/01/01";
            }
            else
            {
                fromdate = d1.Value.ToString("yyyy/MM/dd");
            }


            return fromdate;
        }

        //[WebMethod]
        //public static List<ListItem> GetAssignToEmployee(string empid)
        //{
        //    string loginuserid;
        //    List<ListItem> AssignToEmployee = new List<ListItem>();

        //    List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
        //    loginuserid = BAL.ReportMgmt.GetUserIDByEmployeeID(Convert.ToInt64(empid));
        //    lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(loginuserid);
        //    if (lstEmployee.Count > 0)
        //    {
        //        for (int i = 0; i < lstEmployee.Count; i++)
        //        {
        //            AssignToEmployee.Add(new ListItem
        //            {
        //                Value = "pkID",
        //                Text = "EmployeeName"
        //            });
        //        }
        //    }
        //    else
        //    {

        //    }
        //    return AssignToEmployee;
        //}

        #endregion

        protected void Page_init(object sender, EventArgs e)
        {
            try
            {

                if (Session["report"] != null)
                {
                    ReportDocument rpt = new ReportDocument();
                    rpt = Session["report"] as ReportDocument;
                    CrystalReportViewer1.ReportSource = rpt;

                }
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try //h //ttp://localhost:3216/MasterReport.aspx.cs
            {
                CrystalReportViewer1.Dispose();
            }
            catch (Exception ex)
            {
                alertMessage(ex.ToString(), false);
            }
        }
    }
}