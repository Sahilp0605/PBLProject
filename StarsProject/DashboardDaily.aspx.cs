using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Globalization;
using iTextSharp.text.html;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Net;


namespace StarsProject
{
    public partial class DashboardDaily : System.Web.UI.Page
    { 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnLoginUserID.Value = Session["LoginUserID"].ToString();
                hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();

                BindMonthYear();
                // -------------------------------------------------------
                BindEmployee();
                // -------------------------------------------------------
                BindSummaryCount();     // CRM Summary Count
                BindTaskList();         // ToDO
                BindFollowupList();     // FollowUp
                BindUserControl();      // User CRM Activity
                BindAllAssignedLeads();      // All Assigned Leads
                // -------------------------------------------------------                             // ------------------------------------------------
                //var ClickButton = myAllLeads.FindControl("btnTemp") as Button;
                //var trigger = new PostBackTrigger();
                //trigger.ControlID = ClickButton.UniqueID.ToString();
                //updDailyLead.Triggers.Add(trigger);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:setUserActivityInterface();", true);
                BindTaskList();         // ToDO
                BindFollowupList();     // FollowUp
            }

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            spnFollowCount.InnerText = "Count : " + myFollowup.FollowupCount.ToString() + " ";
            spnToDOCount.InnerText = "Count : " + myToDo.ToDoCount.ToString() + " ";
        }

        public void BindMonthYear()
        {
            // -----------------------------------------------------------------
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
            drpDailyMonth.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));

            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpDailyYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            drpDailyYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            drpDailyYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        public void BindEmployee()
        {
            // ---------------- Employee List  -------------------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "LoginUserID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.SelectedValue = Session["LoginUserID"].ToString();

            drpEmployeeToDO.DataSource = lstEmployee;
            drpEmployeeToDO.DataValueField = "LoginUserID";
            drpEmployeeToDO.DataTextField = "EmployeeName";
            drpEmployeeToDO.DataBind();
            drpEmployeeToDO.SelectedValue = Session["LoginUserID"].ToString();

            drpLeadEmployee.DataSource = lstEmployee;
            drpLeadEmployee.DataValueField = "LoginUserID";
            drpLeadEmployee.DataTextField = "EmployeeName";
            drpLeadEmployee.DataBind();
            drpLeadEmployee.SelectedValue = Session["LoginUserID"].ToString();
            // -----------------------------------------------------------------------
            drpUser.DataSource = lstEmployee;
            drpUser.DataValueField = "LoginUserID";
            drpUser.DataTextField = "EmployeeName";
            drpUser.DataBind();
            if (Session["RoleCode"].ToString().ToLower() == "admin")
                drpUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", ""));
            drpUser.SelectedValue = Session["LoginUserID"].ToString();

            if (hdnSerialKey.Value == "ZE5W-HOME-AG41-SF61")
            {
                drpUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));
            }
        }
        // --------------------------------------------------------------
        // Binding Summary Count
        // --------------------------------------------------------------
        public void BindSummaryCount()
        {
            myCRMSummary.pageView = "dashboarddaily";
            myCRMSummary.pageMonth = drpDailyMonth.SelectedValue;
            myCRMSummary.pageYear = drpDailyYear.SelectedValue;
            myCRMSummary.BindCRMSummaryCount();

        }
        // --------------------------------------------------------------
        // Binding TODO
        // --------------------------------------------------------------
        public void BindTaskList()
        {
            myToDo.pageView = "dashboarddaily";
            myToDo.pageMonth = drpDailyMonth.SelectedValue;
            myToDo.pageYear = drpDailyYear.SelectedValue;
            myToDo.BindTaskList(drpEmployeeToDO.SelectedValue, drpToDO.SelectedValue);
            spnToDOCount.InnerText = "Count : " + myToDo.ToDoCount.ToString() + " ";
        }

        protected void drpToDO_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTaskList();
        }

        protected void drpEmployeeToDO_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTaskList();
        }

        // --------------------------------------------------------------
        // Binding FollowUp
        // --------------------------------------------------------------
        public void BindFollowupList()
        {
            myFollowup.pageView = "dashboarddaily";
            myFollowup.pageMonth = drpDailyMonth.SelectedValue;
            myFollowup.pageYear = drpDailyYear.SelectedValue;
            myFollowup.BindFollowupList(drpEmployee.SelectedValue.ToString(), drpFollowup.SelectedValue);
            spnFollowCount.InnerText = "Count : " + myFollowup.FollowupCount.ToString() + " ";
        }

        protected void drpFollowup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFollowupList();
        }
        protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFollowupList();
        }

        // --------------------------------------------------------------
        // Binding User Activity
        // --------------------------------------------------------------
        public void BindUserControl()
        {
            myUserActivity.pageView = "dashboarddaily";
            myUserActivity.pageMonth = drpDailyMonth.SelectedValue;
            myUserActivity.pageYear = drpDailyYear.SelectedValue;
            myUserActivity.UserID = drpUser.SelectedValue;
            myUserActivity.BindUserActivity(drpUser.SelectedValue);
            //lblUserCount.Text = " Total Count : " + myUserActivity.UserCount.ToString() + " ";
        }

        protected void drpUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUserControl();
        }

        // --------------------------------------------------------------
        // Binding All Assigned Leads
        // --------------------------------------------------------------
        public void BindAllAssignedLeads()
        {
            myAllLeads.pageView = "dashboarddaily";
            myAllLeads.pageMonth = drpDailyMonth.SelectedValue;
            myAllLeads.pageYear = drpDailyYear.SelectedValue;
            myAllLeads.pageUserID = drpLeadEmployee.SelectedValue;
            myAllLeads.BindAssignLeadList();
            spnAllLeadCount.InnerText = "Count : " + myAllLeads.pageeadCount.ToString() + " ";
        }

        protected void drpLeadEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindAllAssignedLeads();
            ScriptManager.RegisterStartupScript(this, typeof(string), "spkwrds", "javascript:initLeadBar();", true);
        }


        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // General Dropdown 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        protected void drpDailyMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSummaryCount();     // CRM Summary Count
            BindTaskList();         // ToDO
            BindFollowupList();     // FollowUp
            BindUserControl();      // User CRM Activity
            BindAllAssignedLeads();      // All Assigned Leads
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void UpdateNotificationTimeStamp()
        {
            BAL.CommonMgmt.UpdateUserTimeStamp(HttpContext.Current.Session["LoginUserID"].ToString(), HttpContext.Current.Session["CompanyID"].ToString());
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GenerateDailyReport(String startdt, String enddt, String loginuserid)
        {
            Boolean firstRow = true;
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblDates = new PdfPTable(16);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(30, 30, 40, 0);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            DataTable t1 = new DataTable();
            DataTable t2 = new DataTable();
            DataTable t3 = new DataTable();
            DataTable t4 = new DataTable();
            DataTable dtFollow = new DataTable();
            DataTable dtQuotation = new DataTable();
            DataTable dtSalesOrder = new DataTable();
            DataTable dtSalesBil = new DataTable();

            BAL.CommonMgmt.GetDailyReport(Convert.ToDateTime(startdt), Convert.ToDateTime(enddt), loginuserid, out t1, out t2, out t3, out t4, out dtFollow, out dtQuotation, out dtSalesOrder, out dtSalesBil);

            String tmpGroup = "", lastEmployee = "";
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (t1.Rows.Count > 0 || t2.Rows.Count>0)
            {
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 20, 33, 22 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                tblNested20.AddCell(pdf.setCell("Generated By : " + loginuserid.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 24, 35, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Period From :" + startdt.ToString() + " To " + enddt.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));

                tblMember.AddCell(pdf.setCell("Daily Report", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblNested20, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 25, 25, 25, 25 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
                #endregion
                // ------------------------------------------------------------------

                String tmpEmployee = "";
                int fuCount = 0, tcCount = 0, inCount = 0, qtCount = 0, csCount = 0, ctCount = 0;
                for (int j = 0; j < t1.Rows.Count; j++)
                {
                    tmpEmployee = t1.Rows[j]["CreatedBy"].ToString();
                    fuCount = 0;
                    tcCount = 0;
                    inCount = 0;
                    qtCount = 0;
                    csCount = 0;
                    ctCount = 0;
                    fuCount = t2.AsEnumerable().Where(r => (r.Field<string>("Category").ToLower() == "followup" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));
                    inCount = t2.AsEnumerable().Where(r => (r.Field<string>("Category").ToLower() == "inquiry" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));
                    tcCount = t2.AsEnumerable().Where(r => (r.Field<string>("Category").ToLower() == "telecaller" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));
                    qtCount = t2.AsEnumerable().Where(r => (r.Field<string>("Category").ToLower() == "quotation" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));
                    csCount = t2.AsEnumerable().Where(r => (r.Field<string>("Category").ToLower() == "customer" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));
                    ctCount = t2.AsEnumerable().Where(r => (r.Field<string>("Category").ToLower() == "complaint" && r.Field<string>("CreatedBy") == tmpEmployee)).Sum(r => r.Field<int>("Count"));

                    if ((tcCount + fuCount + inCount + qtCount + csCount + ctCount) > 0 ||
                        (t3.AsEnumerable().Where(r => r.Field<string>("CreatedBy") == tmpEmployee).Count() > 0))
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_CENTER, 0));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.objContentFontTitleBlack, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_CENTER, 0));
                        tblDetail.AddCell(pdf.setCell("Employee Name : " + tmpEmployee, pdf.GrayBaseColor, pdf.fnCalibriBold12, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_CENTER, 15));

                        if ((tcCount + fuCount + inCount + qtCount + csCount + ctCount) > 0)
                        {
                            tblDetail.AddCell(pdf.setCell("Tele-Caller", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("Inquiry", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("FollowUp", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("Quotation", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));

                            tblDetail.AddCell(pdf.setCell(tcCount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell(inCount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell(fuCount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell(qtCount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));

                            tblDetail.AddCell(pdf.setCell("Complaint", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("Customer", pdf.LightBlueBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));

                            tblDetail.AddCell(pdf.setCell(ctCount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                            tblDetail.AddCell(pdf.setCell(csCount.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER));
                        }
                        // --------------------------------------------------------------------------
                        firstRow = true;
                        if (t3.AsEnumerable().Where(r => r.Field<string>("CreatedBy") == tmpEmployee).Count() > 0)
                        {
                            DataTable selectedTable = t3.AsEnumerable().Where(r => r.Field<string>("CreatedBy") == tmpEmployee).CopyToDataTable();
                            for (int k = 0; k < selectedTable.Rows.Count; k++)
                            {
                                if (firstRow)
                                {
                                    tblDetail.AddCell(pdf.setCell("Daily Activity", pdf.GrayBaseColor, pdf.fnCalibri12, pdf.paddingOf3, 4, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                                    tblDetail.AddCell(pdf.setCell("Category", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                                    tblDetail.AddCell(pdf.setCell("Description", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                                    tblDetail.AddCell(pdf.setCell("Duration (hh.mm)", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                                    firstRow = false;
                                }
                                tblDetail.AddCell(pdf.setCell(selectedTable.Rows[k]["TaskCategory"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                                tblDetail.AddCell(pdf.setCell(selectedTable.Rows[k]["TaskDescription"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                                tblDetail.AddCell(pdf.setCell(selectedTable.Rows[k]["TaskDuration"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                            }
                        }
                    }
                }
            }
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            // Section : Monthly Summary 
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            tblDates.SpacingBefore = 20f;
            tblDates.LockedWidth = true;
            tblDates.HorizontalAlignment = Element.ALIGN_CENTER;

            List<string> CategoryList = new List<string>();
            t4.AsEnumerable().Select(s => s.Field<string>("Category").ToString()).ToList().ForEach(c => c.ToString().Split('|').ToList().ForEach(l => CategoryList.Add(l)));
            foreach (string str in CategoryList.Distinct())
            {
                DataTable tbl1 = t4.AsEnumerable().Where(r => (r.Field<DateTime>("CreatedDate").Day <= 16 && r.Field<string>("Category") == str)).CopyToDataTable();
                tblDates.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf3, 31, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 0));
                tblDates.AddCell(pdf.setCell(str, pdf.GrayBaseColor, pdf.fnCalibri12, pdf.paddingOf3, 31, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                for (int y = 1; y <= 16; y++)
                {
                    tblDates.AddCell(pdf.setCell(y.ToString(), pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                }
                for (int k = 0; k < tbl1.Rows.Count; k++)
                {
                    tblDates.AddCell(pdf.setCell(tbl1.Rows[k]["Total"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                }
                // -------------------------------------------------------------
                DataTable tbl2 = t4.AsEnumerable().Where(r => (r.Field<DateTime>("CreatedDate").Day > 16 && r.Field<string>("Category") == str)).CopyToDataTable();
                for (int y=17; y<=31; y++)
                {
                    tblDates.AddCell(pdf.setCell(y.ToString(), pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                }
                tblDates.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                for (int z = 0; z < tbl2.Rows.Count; z++)
                {
                    tblDates.AddCell(pdf.setCell(tbl2.Rows[z]["Total"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                }
                tblDates.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
            }

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            // Section : FollowUp
            // Sr.No  -  Date  -  CustomerName  -  NextFollowUp  -  Notes
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            PdfPTable tblFollowUp = new PdfPTable(5);
            int[] column_tblFollowUp = { 8, 12, 20, 12, 48 };
            tblFollowUp.SetWidths(column_tblFollowUp);
            tblFollowUp.SpacingBefore = 0f;
            tblFollowUp.LockedWidth = true;
            tblFollowUp.HorizontalAlignment = Element.ALIGN_CENTER;

            
            firstRow = true;
            
            if (dtFollow.AsEnumerable().Count() > 0)
            {
                for (int k = 0; k < dtFollow.Rows.Count; k++)
                {
                    if (firstRow)
                    {
                        tblFollowUp.AddCell(pdf.setCell("Follow Up List", pdf.GrayBaseColor, pdf.fnCalibri14, pdf.paddingOf5, 5, Element.ALIGN_LEFT, Element.ALIGN_CENTER, 15));

                        tblFollowUp.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                        tblFollowUp.AddCell(pdf.setCell("FollowUp Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                        tblFollowUp.AddCell(pdf.setCell("Customer Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                        tblFollowUp.AddCell(pdf.setCell("Next FollowUp", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                        tblFollowUp.AddCell(pdf.setCell("FollowUp Notes", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                        firstRow = false;
                    }
                    tblFollowUp.AddCell(pdf.setCell((k+1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                    tblFollowUp.AddCell(pdf.setCell(Convert.ToDateTime(dtFollow.Rows[k]["FollowUpDate"]).ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblFollowUp.AddCell(pdf.setCell(dtFollow.Rows[k]["CustomerName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblFollowUp.AddCell(pdf.setCell(Convert.ToDateTime(dtFollow.Rows[k]["NextFollowUpDate"]).ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblFollowUp.AddCell(pdf.setCell(dtFollow.Rows[k]["MeetingNotes"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                }
            }

            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            // Section : Quotation
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            PdfPTable tblQuot = new PdfPTable(6);
            int[] column_tblQuot = { 6, 12, 12, 30, 16, 16 };
            tblQuot.SetWidths(column_tblQuot);
            tblQuot.SpacingBefore = 0f;
            tblQuot.LockedWidth = true;
            tblQuot.HorizontalAlignment = Element.ALIGN_CENTER;

            firstRow = true;
            
            if (dtQuotation.AsEnumerable().Count() > 0)
            {
                for (int k = 0; k < dtQuotation.Rows.Count; k++)
                {
                    if (firstRow)
                    {
                        tblQuot.AddCell(pdf.setCell("List Of Quotation", pdf.GrayBaseColor, pdf.fnCalibri14, pdf.paddingOf5, 6, Element.ALIGN_LEFT, Element.ALIGN_CENTER, 15));

                        tblQuot.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                        tblQuot.AddCell(pdf.setCell("Quotation #", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                        tblQuot.AddCell(pdf.setCell("Quotation Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                        tblQuot.AddCell(pdf.setCell("Customer Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                        tblQuot.AddCell(pdf.setCell("Basic Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                        tblQuot.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                        firstRow = false;
                    }

                    tblQuot.AddCell(pdf.setCell((k + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
                    tblQuot.AddCell(pdf.setCell(dtQuotation.Rows[k]["QuotationNo"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblQuot.AddCell(pdf.setCell(Convert.ToDateTime(dtQuotation.Rows[k]["QuotationDate"]).ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblQuot.AddCell(pdf.setCell(dtQuotation.Rows[k]["CustomerName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblQuot.AddCell(pdf.setCell(Convert.ToDecimal(dtQuotation.Rows[k]["BasicAmt"]).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                    tblQuot.AddCell(pdf.setCell(Convert.ToDecimal(dtQuotation.Rows[k]["NetAmt"]).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
                }
            }
            // --------------------------------------------------------------------------
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "DailyReport_admin" + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            //tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            //pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            tblDates.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDates.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDates);

            if (dtFollow.Rows.Count > 0)
            {
                tblFollowUp.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblFollowUp.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfDoc.NewPage();
                pdfDoc.Add(tblFollowUp);
            }

            if (dtQuotation.Rows.Count>0)
            {
                tblQuot.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblQuot.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfDoc.NewPage();
                pdfDoc.Add(tblQuot);
            }

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            //string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            string smallFileName = loginuserid + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;

            return pdfFileName;
            //RecompressPDF(sPath + smallFileName, pdfFileName);
        }


    }
}