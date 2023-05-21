using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using iTextSharp.text.html;

namespace StarsProject
{
    public partial class PayrollSlip : System.Web.UI.Page
    {
        int totrec;
        int ReturnCode;
        String ReturnMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnSerialKey.Value = Session["SerialKey"].ToString();
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 100000;
                BindMonthYear();
                BindPayroll();
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }

        public void BindMonthYear()
        {
            // ----------------------------------------------------.-------------
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("January", "1"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("February", "2"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("March", "3"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("April", "4"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("May", "5"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("June", "6"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("July", "7"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("August", "8"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("September", "9"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("October", "10"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("November", "11"));
            drpSummaryMonth.Items.Add(new System.Web.UI.WebControls.ListItem("December", "12"));
            // -----------------------------------------------------------------
            for (int i = 2019; i <= 2030; i++)
            {
                drpSummaryYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            // -----------------------------------------------------------------
            drpSummaryYear.SelectedValue = DateTime.Now.Year.ToString();
            drpSummaryMonth.SelectedValue = DateTime.Now.Month.ToString();
        }

        public void BindPayroll()
        {
            txtSearchBoxHeader.Text = "";
            int TotalRecord = 0;
            Int64 pMon = 0, pYear = 0;
            
            if (!String.IsNullOrEmpty(drpSummaryMonth.SelectedValue))
                pMon = Convert.ToInt64(drpSummaryMonth.SelectedValue);
            
            if (!String.IsNullOrEmpty(drpSummaryYear.SelectedValue))
                pYear = Convert.ToInt64(drpSummaryYear.SelectedValue);

            List<Entity.Payroll> lstObj = new List<Entity.Payroll>();
            lstObj = BAL.PayrollMgmt.GetPayrollList(0, pMon, pYear, 1, 100000, out TotalRecord);
            rptPayroll.DataSource = lstObj;
            rptPayroll.DataBind();
        }

        protected void drpSummaryMonthYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindPayroll();            
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int TotalRecord = 0;
            Int64 pMon = 0, pYear = 0;

            if (!String.IsNullOrEmpty(drpSummaryMonth.SelectedValue))
                pMon = Convert.ToInt64(drpSummaryMonth.SelectedValue);

            if (!String.IsNullOrEmpty(drpSummaryYear.SelectedValue))
                pYear = Convert.ToInt64(drpSummaryYear.SelectedValue);

            rptPayroll.DataSource = BAL.PayrollMgmt.GeneratePayrollList(0, pMon, pYear, true, Session["LoginUserID"].ToString(), 1, 50000, out TotalRecord);
            rptPayroll.DataBind();
        }

        protected void rptPayroll_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                // -------------------------------------------------------------- Delete Record
                BAL.PayrollMgmt.DeletePayroll(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                // -------------------------------------------------------------------------
                BindPayroll();
            }
            else if (e.CommandName.ToString() == "Regenerate")
            {
                int TotalRecord = 0;
                Int64 pMon = 0, pYear = 0;

                if (!String.IsNullOrEmpty(drpSummaryMonth.SelectedValue))
                    pMon = Convert.ToInt64(drpSummaryMonth.SelectedValue);

                if (!String.IsNullOrEmpty(drpSummaryYear.SelectedValue))
                    pYear = Convert.ToInt64(drpSummaryYear.SelectedValue);

                List<Entity.Payroll> lstEntity = new List<Entity.Payroll>();
                lstEntity = BAL.PayrollMgmt.GeneratePayrollList(Convert.ToInt64(e.CommandArgument.ToString()), pMon, pYear, true, Session["LoginUserID"].ToString(), 1, 50000, out TotalRecord);
                // -------------------------------------------------------------------------
                BindPayroll();
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {

        }

        protected void btnFilterData_Click(object sender, EventArgs e)
        {
            int TotalRecord = 0;
            Int64 pMon = 0, pYear = 0;

            pMon = (!String.IsNullOrEmpty(drpSummaryMonth.SelectedValue)) ? Convert.ToInt64(drpSummaryMonth.SelectedValue) : 0;
            pYear = (!String.IsNullOrEmpty(drpSummaryYear.SelectedValue))? Convert.ToInt64(drpSummaryYear.SelectedValue) : 0;

            rptPayroll.DataSource = BAL.PayrollMgmt.GetPayrollList(0, txtSearchBoxHeader.Text, pMon, pYear, 1, 100000, out TotalRecord);
            rptPayroll.DataBind();
        }
        protected void btnSelDelAll_ServerClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptPayroll.Items)
            {
                CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                HiddenField hdnpkID = (HiddenField)item.FindControl("hdnpkID");
                HiddenField hdnEmployeeID = (HiddenField)item.FindControl("hdnEmployeeID");
                if (chkCtrl.Checked)
                {
                    BAL.PayrollMgmt.DeletePayroll(Convert.ToInt64(hdnpkID.Value), out ReturnCode, out ReturnMsg);
                }
            }
            // -------------------------------
            BindPayroll();
        }

        protected void btnSelEmailAll_ServerClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptPayroll.Items)
            {
                CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                HiddenField hdnpkID = (HiddenField)item.FindControl("hdnpkID");
                HiddenField hdnEmployeeID = (HiddenField)item.FindControl("hdnEmployeeID");
                if (chkCtrl.Checked)
                {
                    GeneratePayslip(Convert.ToInt64(hdnpkID.Value));


                    Entity.Authenticate objAuth = new Entity.Authenticate();
                    objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                    String sendEmailFlag = BAL.CommonMgmt.GetConstant("PAYSLIP-EMAIL", 0, objAuth.CompanyID).ToLower();
                    if (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true")
                    {
                        try
                        {
                            // Sending Email Notification ...
                            String respVal = "";
                            if (Convert.ToInt64(hdnpkID.Value) > 0)
                            {
                                String tmpEmailAddress = "";
                                tmpEmailAddress = (Convert.ToInt64(hdnEmployeeID.Value) > 0) ? BAL.CommonMgmt.GetEmployeeEmailByEmployeeID(Convert.ToInt64(hdnEmployeeID.Value)) : "";
                                if (!String.IsNullOrEmpty(tmpEmailAddress) && tmpEmailAddress.ToUpper() != "NULL")
                                    respVal = BAL.CommonMgmt.SendEmailNotifcation("PAYSLIP", Session["LoginUserID"].ToString(), Convert.ToInt64(hdnpkID.Value), tmpEmailAddress);

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Email Sent Successfully !','toast-success');", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Sending Email Failed !','toast-success');", true);
                        }
                    }
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public static string GeneratePayslip(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // ----------------------------------------------------------------------- 
            string returnValue = "";

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            string LoginUserID = HttpContext.Current.Session["LoginUserID"].ToString();
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
            Int32 CompanyId = 0;

            StarsProject.QuotationEagle.serialkey = tmpSerialKey;
            StarsProject.QuotationEagle.LoginUserID = LoginUserID;
            StarsProject.QuotationEagle.printheader = flagPrintHeader;
            StarsProject.QuotationEagle.path = Path;
            StarsProject.QuotationEagle.imagepath = imagepath;
            StarsProject.QuotationEagle.companyid = CompanyId;

            if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")     // STEELMAN GASES PVT LTD
            {
                returnValue = GeneratePayslip_Sharvaya(pkID);
            }
            else if (tmpSerialKey == "LHUV-E36R-5PXM-2XAT")     // Boss
            {
                returnValue = GeneratePayslip_Boss(pkID);
            }
            else if (tmpSerialKey == "6GZP-BW7W-78DF-HG88")     // Dishachi
            {
                returnValue = GeneratePayslip_Dishachi(pkID);
            }
            else if (tmpSerialKey == "SIV3-DIO4-09IK-98RE")     // ShivSai
            {
                returnValue = GeneratePayslip_ShivSai(pkID);
            }
            else if (tmpSerialKey == "LVK4-MN01-K121-NGVL")     // M.N.Rubber
            {
                returnValue = GeneratePayslip_MNRUBBER(pkID);
            }
            else
            {
                returnValue = GeneratePayslip_Sharvaya(pkID);
            }
            return returnValue;
        }
        public static string GeneratePayslip_ShivSai(Int64 pQuotID)
        {
            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();

            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(3);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Payroll> lstQuot = new List<Entity.Payroll>();
            lstQuot = BAL.PayrollMgmt.GetPayrollList(Convert.ToInt64(pQuotID), 0, 0, Convert.ToInt32(1), Convert.ToInt32(10000), out TotalCount);

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");


            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out TotalCount);
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            Int64 ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;

            Document pdfDoc = pdf.initiatePage(lstPrinter, "payslip");
            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================

            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;
                //-----------------------------------------------------------
                // Employee Details Tables
                //-----------------------------------------------------------

                PdfPTable tblLocation = new PdfPTable(1);
                int[] column_tblLocation = { 100 };
                tblLocation.SetWidths(column_tblLocation);
                int fileCount2 = 0;
                string tmpFile2 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/") + "\\CompanyLogo.png";


                if (File.Exists(tmpFile2))
                {
                    if (File.Exists(tmpFile2))   //Signature print
                    {
                        PdfPTable tblSymbol = new PdfPTable(1);
                        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile2);
                        eLoc.ScaleAbsolute(90, 35);


                        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                        tblLocation.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount2 = fileCount2 + 1;
                    }
                }

                PdfPTable tblCompanyD= new PdfPTable(1);
                int[] column_tblCompanyD = { 100 };
                tblCompanyD.SetWidths(column_tblCompanyD);

               // string date = lstQuot[0].CreatedDate.("MMMM");
                


                tblCompanyD.AddCell(pdf.setCell(lstOrg[0].OrgName.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCompanyD.AddCell(pdf.setCell(lstOrg[0].Address.ToUpper()+", "+lstOrg[0].CityName.ToUpper()+ ", "+lstOrg[0].StateName.ToUpper() + " - "+ lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCompanyD.AddCell(pdf.setCell("PaySlip For the Month of " + lstQuot[0].CreatedDate.AddMonths(-1)+ " "+ lstQuot[0].CreatedDate.Year, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


                PdfPTable tblMemberEmployee = new PdfPTable(2);
                int[] column_tblMemberEmployee = { 30, 70 };
                tblMemberEmployee.SetWidths(column_tblMemberEmployee);
                tblMemberEmployee.AddCell(pdf.setCell("Employee", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Designation", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].Designation, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Department", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Location", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Effectve Work Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(": " + lstQuot[0].WDays, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Extra Day Work", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("HoliDay Work", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("LOP", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // -------------------------------------------------------------
                PdfPTable tblMemberDays = new PdfPTable(2);
                int[] column_tblMemberDays = { 30, 70 };
                tblMemberDays.SetWidths(column_tblMemberDays);
                tblMemberDays.AddCell(pdf.setCell("Employee No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": "+lstQuot[0].PayDate.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Joining Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Bank Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Bank Account No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("PAN Number", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("PF No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("PF UAN", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                Decimal totKms = 0;
                if (lstQuot.Count > 0)
                {
                    totKms = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingKilometers(lstQuot[0].EmployeeID, lstQuot[0].PayDate.Month, lstQuot[0].PayDate.Year));
                }
                tblMemberDays.AddCell(pdf.setCell(totKms.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //-------------------------------------------------------------------
                //  Details Tables
                //-------------------------------------------------------------------
                int[] column_tblDetail = { 25, 25, 25, 25 };
                tblDetail.SetWidths(column_tblDetail);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                //----------------------------------------------------------
                PdfPTable tblEarning = new PdfPTable(3);
                int[] column_tblEarning = { 50,25,25 };
                tblEarning.SetWidths(column_tblEarning);
                tblEarning.AddCell(pdf.setCell("Earning (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblEarning.AddCell(pdf.setCell("Full", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                tblEarning.AddCell(pdf.setCell("Actual", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));

                tblEarning.AddCell(pdf.setCell("Basic", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Basic.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Basic.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblEarning.AddCell(pdf.setCell("HRA", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].HRA.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].HRA.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblEarning.AddCell(pdf.setCell("Performance Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Special.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Special.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblEarning.AddCell(pdf.setCell("Total Earning : INR", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 7));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Total_Income.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Total_Income.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 14));
                //---------------------------------------------------------------------
                PdfPTable tblDeduction = new PdfPTable(2);
                int[] column_tblDeduction = { 70, 30 };
                tblDeduction.SetWidths(column_tblDeduction);
                tblDeduction.AddCell(pdf.setCell("Deduction (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblDeduction.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 14));
                if (lstQuot[0].PF != 0)
                {
                    tblDeduction.AddCell(pdf.setCell("P. F.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblDeduction.AddCell(pdf.setCell(lstQuot[0].PF.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                if (lstQuot[0].ESI != 0)
                {
                    tblDeduction.AddCell(pdf.setCell("E.S.I.C.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblDeduction.AddCell(pdf.setCell(lstQuot[0].ESI.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                if (lstQuot[0].PT != 0)
                {
                    tblDeduction.AddCell(pdf.setCell("Profession Tax", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblDeduction.AddCell(pdf.setCell(lstQuot[0].PT.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                if (lstQuot[0].Loan != 0)
                {
                    tblDeduction.AddCell(pdf.setCell("Repay Loan And Advance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblDeduction.AddCell(pdf.setCell(lstQuot[0].Loan.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                tblDeduction.AddCell(pdf.setCell("Deduction Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 7));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Total_Deduct.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 14));
                //----------------------------------------------------------------------------------
                // Footer 
                //----------------------------------------------------------------------------------
                int[] column_tblFooter = { 30, 30, 40 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                //--------------------------------------------------------------------------------------

                //--------------------------------------------------------------------------------------
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                string pImageFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo";
                int[] noOfColsStruc = { 50, 50 };
                //--------------------------------------------------------------------------------------
                //tblMember.AddCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile));
                tblMember.AddCell(pdf.setCell(tblLocation, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblCompanyD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                tblMember.AddCell(pdf.setCell(tblMemberEmployee, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberDays, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                tblDetail.AddCell(pdf.setCell(tblEarning, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(tblDeduction, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("Net Pay For The Month : (Total Earning - Total Deduction) : " + lstQuot[0].NetSalary.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)lstQuot[0].NetSalary);
                tblDetail.AddCell(pdf.setCell("( Ruppes "+NetAmtInWords+" Only )", pdf.WhiteBaseColor, pdf.fnCalibriItalic10, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

                #endregion
            }
            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "Pay_" + lstQuot[0].EmployeeName.ToString().Replace(" ", "") + "_" + lstQuot[0].PayDate.Month.ToString() + "_" + lstQuot[0].PayDate.Year.ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            //clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
            //clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
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
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);
            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            pdfDoc.Dispose();
            string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            fs.Dispose();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
            RecompressPDF(sPath + smallFileName, pdfFileName);
            return "PDF/" + sFileName;
        }
        [WebMethod(EnableSession = true)]
        public static string GeneratePayslip_Sharvaya(Int64 pQuotID)
        {
            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();
            
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(3);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Payroll> lstQuot = new List<Entity.Payroll>();
            lstQuot = BAL.PayrollMgmt.GetPayrollList(Convert.ToInt64(pQuotID), 0, 0, Convert.ToInt32(1), Convert.ToInt32(10000), out TotalCount);

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            Int64 ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20 ;

            Document pdfDoc = pdf.initiatePage(lstPrinter,"payslip");
            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();
            
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================

            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;
                //-----------------------------------------------------------
                // Employee Details Tables
                //-----------------------------------------------------------
                PdfPTable tblMemberEmployee = new PdfPTable(2);
                int[] column_tblMemberEmployee = { 30, 70 };
                tblMemberEmployee.SetWidths(column_tblMemberEmployee);

                decimal perday = 0;
                if (lstQuot[0].BasicPer.ToLower() == "monthly")
                    perday = lstQuot[0].FixedSalary / lstQuot[0].WDays;
                else if (lstQuot[0].BasicPer.ToLower() == "daily")
                    perday = lstQuot[0].FixedSalary;
                else
                    perday = lstQuot[0].FixedSalary;

                tblMemberEmployee.AddCell(pdf.setCell("Employee", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Designation", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].Designation, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic Per", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].BasicPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(Convert.ToString(lstQuot[0].FixedSalary), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic Per Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(perday.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // -------------------------------------------------------------
                PdfPTable tblMemberDays = new PdfPTable(2);
                int[] column_tblMemberDays = { 40, 60 };
                tblMemberDays.SetWidths(column_tblMemberDays);

                int Days = DateTime.DaysInMonth(lstQuot[0].PayDate.Year, lstQuot[0].PayDate.Month);
                tblMemberDays.AddCell(pdf.setCell("Pay For The Month", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PayDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Month Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(Days.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Comapny Working days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].WDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Present days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Holidays", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell((lstQuot[0].HDays + lstQuot[0].LDays).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Driving Kilimeters", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                Decimal totKms = 0;
                if (lstQuot.Count > 0)
                {
                    totKms = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingKilometers(lstQuot[0].EmployeeID, lstQuot[0].PayDate.Month, lstQuot[0].PayDate.Year));
                }
                tblMemberDays.AddCell(pdf.setCell(totKms.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //-------------------------------------------------------------------
                //  Details Tables
                //-------------------------------------------------------------------
                int[] column_tblDetail = { 25, 25, 25, 25 };
                tblDetail.SetWidths(column_tblDetail);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                //----------------------------------------------------------
                PdfPTable tblEarning = new PdfPTable(2);
                int[] column_tblEarning = { 70, 30 };
                tblEarning.SetWidths(column_tblEarning);
                tblEarning.AddCell(pdf.setCell("Earning (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblEarning.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                tblEarning.AddCell(pdf.setCell("Salary", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Basic.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Holiday Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].HRA.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Petrol Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Conveyance.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Other Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Special.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Earning Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Total_Income.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //---------------------------------------------------------------------
                PdfPTable tblDeduction = new PdfPTable(2);
                int[] column_tblDeduction = { 70, 30 };
                tblDeduction.SetWidths(column_tblDeduction);
                tblDeduction.AddCell(pdf.setCell("Deduction (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDeduction.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                tblDeduction.AddCell(pdf.setCell("P. F.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PF.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("E.S.I.C.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].ESI.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Profession Tax", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PT.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Repay Loan And Advance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Loan.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Deduction Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Total_Deduct.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //----------------------------------------------------------------------------------
                // Footer 
                //----------------------------------------------------------------------------------
                int[] column_tblFooter = { 30, 30, 40 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                //--------------------------------------------------------------------------------------

                //--------------------------------------------------------------------------------------
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                string pImageFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo";
                int[] noOfColsStruc = { 50, 50};
                //--------------------------------------------------------------------------------------
                //tblMember.AddCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile));

                //tblMember.AddCell(pdf.setCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile), pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell("SALARY VOUCHER", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberEmployee, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberDays, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                tblDetail.AddCell(pdf.setCell(tblEarning, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(tblDeduction, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("Net Pay :" + lstQuot[0].NetSalary.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //--------------Footer
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 9));

                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                tblFooter.AddCell(pdf.setCell("Prepared By : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 6));
                tblFooter.AddCell(pdf.setCell("Authorized By :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblFooter.AddCell(pdf.setCell("Signature", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                #endregion
            }
            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "Pay_" + lstQuot[0].EmployeeName.ToString().Replace(" ","") + "_" + lstQuot[0].PayDate.Month.ToString() + "_" + lstQuot[0].PayDate.Year.ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            //clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
            //clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
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
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);
            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            pdfDoc.Dispose();
            string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            fs.Dispose();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
            RecompressPDF(sPath + smallFileName, pdfFileName);
            return "PDF/"+ sFileName;
        }
        public static string GeneratePayslip_Dishachi(Int64 pQuotID)
        {
            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();

            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(3);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Payroll> lstQuot = new List<Entity.Payroll>();
            lstQuot = BAL.PayrollMgmt.GetPayrollList(Convert.ToInt64(pQuotID), 0, 0, Convert.ToInt32(1), Convert.ToInt32(10000), out TotalCount);

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            Int64 ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;

            Document pdfDoc = pdf.initiatePage(lstPrinter, "payslip");
            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================

            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;
                //-----------------------------------------------------------
                // Employee Details Tables
                //-----------------------------------------------------------
                PdfPTable tblMemberEmployee = new PdfPTable(2);
                int[] column_tblMemberEmployee = { 30, 70 };
                tblMemberEmployee.SetWidths(column_tblMemberEmployee);
                tblMemberEmployee.AddCell(pdf.setCell("Employee", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Designation", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].Designation, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic Per", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].BasicPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(Convert.ToString(lstQuot[0].FixedSalary), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic Per Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(Convert.ToString(lstQuot[0].FixedSalary), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // -------------------------------------------------------------
                PdfPTable tblMemberDays = new PdfPTable(2);
                int[] column_tblMemberDays = { 40, 60 };
                tblMemberDays.SetWidths(column_tblMemberDays);
                tblMemberDays.AddCell(pdf.setCell("Pay For The Month", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PayDate.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Month Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].WDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Comapny Working days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].WDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Present days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Holidays", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].HDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Driving Kilimeters", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                Decimal totKms = 0;
                if (lstQuot.Count > 0)
                {
                    totKms = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingKilometers(lstQuot[0].EmployeeID, lstQuot[0].PayDate.Month, lstQuot[0].PayDate.Year));
                }
                tblMemberDays.AddCell(pdf.setCell(totKms.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //-------------------------------------------------------------------
                //  Details Tables
                //-------------------------------------------------------------------
                int[] column_tblDetail = { 25, 25, 25, 25 };
                tblDetail.SetWidths(column_tblDetail);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                //----------------------------------------------------------
                PdfPTable tblEarning = new PdfPTable(2);
                int[] column_tblEarning = { 70, 30 };
                tblEarning.SetWidths(column_tblEarning);
                tblEarning.AddCell(pdf.setCell("Earning (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblEarning.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                tblEarning.AddCell(pdf.setCell("Salary", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Basic.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Holiday Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].HRA.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Petrol Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Conveyance.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Other Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Special.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Earning Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Total_Income.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //---------------------------------------------------------------------
                PdfPTable tblDeduction = new PdfPTable(2);
                int[] column_tblDeduction = { 70, 30 };
                tblDeduction.SetWidths(column_tblDeduction);
                tblDeduction.AddCell(pdf.setCell("Deduction (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDeduction.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                tblDeduction.AddCell(pdf.setCell("P. F.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PF.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("E.S.I.C.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].ESI.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Profession Tax", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PT.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Repay Loan And Advance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Loan.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Deduction Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Total_Deduct.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //----------------------------------------------------------------------------------
                // Footer 
                //----------------------------------------------------------------------------------
                int[] column_tblFooter = { 30, 30, 40 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                //--------------------------------------------------------------------------------------

                //--------------------------------------------------------------------------------------
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                string pImageFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo";
                int[] noOfColsStruc = { 50, 50 };
                //--------------------------------------------------------------------------------------
                //tblMember.AddCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile));
                //--------------------------Header--------------------------------
                PdfPTable tblBanner = new PdfPTable(1);
                int[] column_tblBanner = { 100 };
                tblBanner.SetWidths(column_tblBanner);
                int fileCount = 0;
                //string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\QuotationHeader.png";
                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        //int[] column_tblSign = { 30 };
                        //tblSign.SetWidths(column_tblSign);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(520, 90);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblBanner.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                //else
                //{
                //    tblBanner.AddCell(pdf.setCell("For " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //}
                //----------------------------------------------------------------
                //string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                tblMember.AddCell(pdf.setCell(tblBanner, pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                //tblMember.AddCell(pdf.setCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile), pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell("SALARY VOUCHER", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberEmployee, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberDays, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                tblDetail.AddCell(pdf.setCell(tblEarning, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(tblDeduction, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("Net Pay :" + lstQuot[0].NetSalary.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //--------------Footer
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 9));

                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));
                
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 4), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                tblFooter.AddCell(pdf.setCell("Prepared By : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 6));
                tblFooter.AddCell(pdf.setCell("Authorized By :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblFooter.AddCell(pdf.setCell("Signature", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                #endregion
            }
            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "Pay_" + lstQuot[0].EmployeeName.ToString().Replace(" ", "") + "_" + lstQuot[0].PayDate.Month.ToString() + "_" + lstQuot[0].PayDate.Year.ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            //clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
            //clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
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
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);
            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            pdfDoc.Dispose();
            string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            fs.Dispose();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
            RecompressPDF(sPath + smallFileName, pdfFileName);
            return "PDF/" + sFileName;
        }
        public static string GeneratePayslip_Boss(Int64 pQuotID)
        {
            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();

            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Payroll> lstQuot = new List<Entity.Payroll>();
            lstQuot = BAL.PayrollMgmt.GetPayrollList(Convert.ToInt64(pQuotID), 0, 0, Convert.ToInt32(1), Convert.ToInt32(10000), out TotalCount);

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out TotalCount);
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            Int64 ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;

            Document pdfDoc = pdf.initiatePage(lstPrinter, "payslip");
            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================

            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                //---------------------Company Name--------------------------
                PdfPTable tblCompany = new PdfPTable(1);
                int[] column_tblCompany = { 100};
                tblCompany.SetWidths(column_tblCompany);
                tblCompany.AddCell(pdf.setCell(lstOrg[0].OrgName.ToUpper(),pdf.WhiteBaseColor,pdf.fnCalibriBold10,pdf.paddingOf3,1,Element.ALIGN_CENTER,Element.ALIGN_TOP,15 ));
                tblCompany.AddCell(pdf.setCell(lstOrg[0].Address.ToUpper() + " " + lstOrg[0].CityName.ToUpper() + " - " + lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblCompany.AddCell(pdf.setCell(lstOrg[0].StateName.ToUpper()  + " ( INDIA ) ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblCompany.AddCell(pdf.setCell("CONTACT : " + lstOrg[0].Landline1 + " " + " EMAIL : account@bosspackaging.in" , pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblCompany.AddCell(pdf.setCell("Salary Slip For The Month Of March - 2021 --Ask--", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));


                //-----------------------------------------------------------
                // Employee Details Tables
                //-----------------------------------------------------------
                PdfPTable tblMemberEmployee = new PdfPTable(2);
                int[] column_tblMemberEmployee = { 20, 80 };
                tblMemberEmployee.SetWidths(column_tblMemberEmployee);
                tblMemberEmployee.AddCell(pdf.setCell("Employee", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMemberEmployee.AddCell(pdf.setCell("Designation", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].Designation, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMemberEmployee.AddCell(pdf.setCell("Joining Date", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMemberEmployee.AddCell(pdf.setCell("--ASK--", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //tblMemberEmployee.AddCell(pdf.setCell("Basic", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                //tblMemberEmployee.AddCell(pdf.setCell(Convert.ToString(lstQuot[0].FixedSalary), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblMemberEmployee.AddCell(pdf.setCell("Basic Per Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                //tblMemberEmployee.AddCell(pdf.setCell(Convert.ToString(lstQuot[0].FixedSalary), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //// -------------------------------------------------------------


                PdfPTable tblMemberDays = new PdfPTable(2);
                int[] column_tblMemberDays = { 40, 60 };
                tblMemberDays.SetWidths(column_tblMemberDays);
                tblMemberDays.AddCell(pdf.setCell("Pay For The Month", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PayDate.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Month Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].WDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Comapny Working days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].WDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Present days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Holidays", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].HDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Driving Kilimeters", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                Decimal totKms = 0;
                if (lstQuot.Count > 0)
                {
                    totKms = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingKilometers(lstQuot[0].EmployeeID, lstQuot[0].PayDate.Month, lstQuot[0].PayDate.Year));
                }
                tblMemberDays.AddCell(pdf.setCell(totKms.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //-------------------------------------------------------------------
                //  Details Tables
                //-------------------------------------------------------------------
                int[] column_tblDetail = { 25, 25, 25, 25 };
                tblDetail.SetWidths(column_tblDetail);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                //----------------------------------------------------------
                PdfPTable tblEarning = new PdfPTable(5);
                int[] column_tblEarning = { 15,25,20,20,20 };
                tblEarning.SetWidths(column_tblEarning);
                tblEarning.AddCell(pdf.setCell("SR. NO", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("DESCRIPTION", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("RATE / PER"+ "\n" + "DAY", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("PRESENT /" +"\n"+ "HOURS", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("TOTAL PAYABLE", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("1", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("Salary", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("2", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("Over Time", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("3", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("Tours(Days)", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("4", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("Others", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("[ A ]", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell("TOTAL", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Total_Income.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));


                //---------------------------------------------------------------------
                PdfPTable tblDeduction = new PdfPTable(3);
                int[] column_tblDeduction = { 20,40,40 };
                tblDeduction.SetWidths(column_tblDeduction);
                tblDeduction.AddCell(pdf.setCell("SR. NO.", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("DESCRIPTION", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("TOTAL" +"\n"+"REFUNDABLE", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("1", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("Loan", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Loan.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("2", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("Kharchi", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].TDS.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("3", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell( "Salary Advance", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("Advance", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("4", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("Prof. Tax", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PT.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell("[ B ]", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Total_Deduct.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));


                //----------------------------------------------------------------------------------
                // Footer 
                //----------------------------------------------------------------------------------
                int[] column_tblFooter = { 60,40 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;

                PdfPTable tblTotal = new PdfPTable(3);
                int[] column_tblTotal = { 20, 15, 65 };
                tblTotal.SetWidths(column_tblTotal);
                
                
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)lstQuot[0].NetSalary);

                tblTotal.AddCell(pdf.setCell("Net" +"\n" + "Payment", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblTotal.AddCell(pdf.setCell(lstQuot[0].NetSalary.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblTotal.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell("Rs. In Words : ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell(NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell("Bank :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell("Date : ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell(lstQuot[0].PayDate.ToString("dd-MM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell("EMPLOYEE SIGNATURE", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblTotal.AddCell(pdf.setCell(" " , pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));



                PdfPTable tblVerify = new PdfPTable(1);
                int[] column_tblVerify = { 100 };
                tblVerify.SetWidths(column_tblVerify);
               

                tblVerify.AddCell(pdf.setCell("Verified By:", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblVerify.AddCell(pdf.setCell("Checked By:", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblVerify.AddCell(pdf.setCell("Director / Managing Director / Authorise", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


                //--------------------------------------------------------------------------------------

                //--------------------------------------------------------------------------------------
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                string pImageFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo";
                int[] noOfColsStruc = { 50, 50 };
                //--------------------------------------------------------------------------------------
                //tblMember.AddCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile));

                //tblMember.AddCell(pdf.setCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile), pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                //tblMember.AddCell(pdf.setCell("SALARY VOUCHER", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblCompany, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberEmployee, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //tblMember.AddCell(pdf.setCell(tblMemberDays, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                tblDetail.AddCell(pdf.setCell(tblEarning, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(tblDeduction, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //--------------Footer
                tblFooter.AddCell(pdf.setCell(tblTotal, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblFooter.AddCell(pdf.setCell(tblVerify, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 11));

                //tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                //tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                //tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                //tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                tblDetail.AddCell(pdf.setCell(tblTotal, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblDetail.AddCell(pdf.setCell(tblVerify, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 11));
                //tblFooter.AddCell(pdf.setCell("Signature", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                #endregion
            }
            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "Pay_" + lstQuot[0].EmployeeName.ToString().Replace(" ", "") + "_" + lstQuot[0].PayDate.Month.ToString() + "_" + lstQuot[0].PayDate.Year.ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            //clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
            //clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
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
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);
            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            pdfDoc.Dispose();
            string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            fs.Dispose();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
            RecompressPDF(sPath + smallFileName, pdfFileName);
            return "PDF/" + sFileName;
        }
        public static string GeneratePayslip_MNRUBBER(Int64 pQuotID)
        {
            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();

            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(4);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(3);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Payroll> lstQuot = new List<Entity.Payroll>();
            lstQuot = BAL.PayrollMgmt.GetPayrollList(Convert.ToInt64(pQuotID), 0, 0, Convert.ToInt32(1), Convert.ToInt32(10000), out TotalCount);

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            Int64 ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;

            Document pdfDoc = pdf.initiatePage(lstPrinter, "payslip");
            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================

            if (lstQuot.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;
                //-----------------------------------------------------------
                // Employee Details Tables
                //-----------------------------------------------------------
                PdfPTable tblMemberEmployee = new PdfPTable(2);
                int[] column_tblMemberEmployee = { 30, 70 };
                tblMemberEmployee.SetWidths(column_tblMemberEmployee);

                decimal perday = 0;
                if (lstQuot[0].BasicPer.ToLower() == "monthly")
                    perday = lstQuot[0].FixedSalary / lstQuot[0].WDays;
                else if (lstQuot[0].BasicPer.ToLower() == "daily")
                    perday = lstQuot[0].FixedSalary;
                else
                    perday = lstQuot[0].FixedSalary;

                tblMemberEmployee.AddCell(pdf.setCell("Employee", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Designation", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].Designation, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic Per", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(lstQuot[0].BasicPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(Convert.ToString(lstQuot[0].FixedSalary), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMemberEmployee.AddCell(pdf.setCell("Basic Per Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblMemberEmployee.AddCell(pdf.setCell(perday.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // -------------------------------------------------------------
                PdfPTable tblMemberDays = new PdfPTable(2);
                int[] column_tblMemberDays = { 40, 60 };
                tblMemberDays.SetWidths(column_tblMemberDays);

                int Days = DateTime.DaysInMonth(lstQuot[0].PayDate.Year, lstQuot[0].PayDate.Month);
                tblMemberDays.AddCell(pdf.setCell("Pay For The Month", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PayDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Month Day", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(Days.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Comapny Working days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].WDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Present days", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell(lstQuot[0].PDays.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Leave Days ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell((lstQuot[0].LDays).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell("Overtime Hrs.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMemberDays.AddCell(pdf.setCell((lstQuot[0].ODays).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                Decimal totKms = 0;
                if (lstQuot.Count > 0)
                {
                    totKms = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingKilometers(lstQuot[0].EmployeeID, lstQuot[0].PayDate.Month, lstQuot[0].PayDate.Year));
                }
                tblMemberDays.AddCell(pdf.setCell(totKms.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //-------------------------------------------------------------------
                //  Details Tables
                //-------------------------------------------------------------------
                int[] column_tblDetail = { 25, 25, 25, 25 };
                tblDetail.SetWidths(column_tblDetail);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                //----------------------------------------------------------
                PdfPTable tblEarning = new PdfPTable(2);
                int[] column_tblEarning = { 70, 30 };
                tblEarning.SetWidths(column_tblEarning);
                tblEarning.AddCell(pdf.setCell("Earning (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblEarning.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                tblEarning.AddCell(pdf.setCell("Salary", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Basic.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Holiday Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].HRA.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Petrol Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Conveyance.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Overtime", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].OverTime.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Other Allowance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Special.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell("Earning Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblEarning.AddCell(pdf.setCell(lstQuot[0].Total_Income.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //---------------------------------------------------------------------
                PdfPTable tblDeduction = new PdfPTable(2);
                int[] column_tblDeduction = { 70, 30 };
                tblDeduction.SetWidths(column_tblDeduction);
                tblDeduction.AddCell(pdf.setCell("Deduction (Particulars)", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDeduction.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                tblDeduction.AddCell(pdf.setCell("P. F.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PF.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("E.S.I.C.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].ESI.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Profession Tax", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].PT.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Repay Loan", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].LoanAmt.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Advance", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Upad.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell("Deduction Total : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblDeduction.AddCell(pdf.setCell(lstQuot[0].Total_Deduct.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //----------------------------------------------------------------------------------
                // Footer 
                //----------------------------------------------------------------------------------
                int[] column_tblFooter = { 30, 30, 40 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                //--------------------------------------------------------------------------------------

                //--------------------------------------------------------------------------------------
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                string pImageFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo";
                int[] noOfColsStruc = { 50, 50 };
                //--------------------------------------------------------------------------------------
                //tblMember.AddCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile));

                //tblMember.AddCell(pdf.setCell(pdf.GenerateCompanyHeader_ImageAddress(tmpSerialKey, noOfColsStruc, pImageFile), pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell("SALARY VOUCHER", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberEmployee, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMember.AddCell(pdf.setCell(tblMemberDays, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                tblDetail.AddCell(pdf.setCell(tblEarning, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(tblDeduction, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("Net Pay :" + lstQuot[0].NetSalary.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //--------------Footer
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 5));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 9));

                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 4));
                tblFooter.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblFooter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 8));

                tblFooter.AddCell(pdf.setCell("Prepared By : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 6));
                tblFooter.AddCell(pdf.setCell("Authorized By :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblFooter.AddCell(pdf.setCell("Signature", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                #endregion
            }
            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "Pay_" + lstQuot[0].EmployeeName.ToString().Replace(" ", "") + "_" + lstQuot[0].PayDate.Month.ToString() + "_" + lstQuot[0].PayDate.Year.ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            //clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
            //clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
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
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);
            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            pdfDoc.Dispose();
            string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            fs.Dispose();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
            RecompressPDF(sPath + smallFileName, pdfFileName);
            return "PDF/" + sFileName;
        }

        public static void RecompressPDF(string largePDF, string smallPDF)
        {
            //Bind a reader to our large PDF
            PdfReader reader = new PdfReader(largePDF);
            //Create our output PDF
            using (FileStream fs = new FileStream(smallPDF, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //Bind a stamper to the file and our reader
                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {
                    //NOTE: This code only deals with page 1, you'd want to loop more for your code
                    //Get page 1
                    PdfDictionary page = reader.GetPageN(1);
                    //Get the xobject structure
                    PdfDictionary resources = (PdfDictionary)PdfReader.GetPdfObject(page.Get(PdfName.RESOURCES));
                    PdfDictionary xobject = (PdfDictionary)PdfReader.GetPdfObject(resources.Get(PdfName.XOBJECT));
                    if (xobject != null)
                    {
                        PdfObject obj;
                        //Loop through each key
                        foreach (PdfName name in xobject.Keys)
                        {
                            obj = xobject.Get(name);
                            if (obj.IsIndirect())
                            {
                                //Get the current key as a PDF object
                                PdfDictionary imgObject = (PdfDictionary)PdfReader.GetPdfObject(obj);
                                //See if its an image
                                if (imgObject.Get(PdfName.SUBTYPE).Equals(PdfName.IMAGE))
                                {
                                    //NOTE: There's a bunch of different types of filters, I'm only handing the simplest one here which is basically raw JPG, you'll have to research others
                                    if (imgObject.Get(PdfName.FILTER).Equals(PdfName.DCTDECODE))
                                    {
                                        //Get the raw bytes of the current image
                                        byte[] oldBytes = PdfReader.GetStreamBytesRaw((PRStream)imgObject);
                                        //Will hold bytes of the compressed image later
                                        byte[] newBytes;
                                        //Wrap a stream around our original image
                                        using (MemoryStream sourceMS = new MemoryStream(oldBytes))
                                        {
                                            //Convert the bytes into a .Net image
                                            using (System.Drawing.Image oldImage = System.Drawing.Bitmap.FromStream(sourceMS))
                                            {
                                                //Shrink the image to 90% of the original
                                                using (System.Drawing.Image newImage = ShrinkImage(oldImage, 0.9f))
                                                {
                                                    //Convert the image to bytes using JPG at 85%
                                                    newBytes = ConvertImageToBytes(newImage, 85);
                                                }
                                            }
                                        }
                                        //Create a new iTextSharp image from our bytes
                                        iTextSharp.text.Image compressedImage = iTextSharp.text.Image.GetInstance(newBytes);
                                        //Kill off the old image
                                        PdfReader.KillIndirect(obj);
                                        //Add our image in its place
                                        stamper.Writer.AddDirectImageSimple(compressedImage, (PRIndirectReference)obj);
                                    }
                                }
                            }
                        }
                    }
                }

                fs.Close();
                fs.Dispose();
            }
            reader.Close();
        }
        //Standard image save code from MSDN, returns a byte array
        private static byte[] ConvertImageToBytes(System.Drawing.Image image, long compressionLevel)
        {
            if (compressionLevel < 0)
            {
                compressionLevel = 0;
            }
            else if (compressionLevel > 100)
            {
                compressionLevel = 100;
            }
            System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, compressionLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jgpEncoder, myEncoderParameters);
                return ms.ToArray();
            }

        }
        //standard code from MSDN
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        //Standard high quality thumbnail generation from http://weblogs.asp.net/gunnarpeipman/archive/2009/04/02/resizing-images-without-loss-of-quality.aspx
        private static System.Drawing.Image ShrinkImage(System.Drawing.Image sourceImage, float scaleFactor)
        {
            int newWidth = Convert.ToInt32(sourceImage.Width * scaleFactor);
            int newHeight = Convert.ToInt32(sourceImage.Height * scaleFactor);

            var thumbnailBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnailBitmap))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                g.DrawImage(sourceImage, imageRectangle);
            }
            return thumbnailBitmap;
        }
        protected void btnBindGrd_ServerClick(object sender, EventArgs e)
        {
            BindPayroll();
        }
        protected void btnAutoGenerate_Click(object sender, EventArgs e)
        {
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            
            foreach (Entity.OrganizationEmployee tmpEmp in lstEmployee)
            {
                int TotalCount = 0;
                bool PF_Calculation = false, PT_Calculation = false;
                Int64 pEmployeeID = 0, pMon, pYear;
                Decimal pWD = 0, pPD = 0, pHD = 0, pOD = 0, pLD = 0, pTotDays = 0;
                Decimal fixedSalary = 0, tmpNetSalary = 0;
                String lblBasicPer = "", pPayDate = "";

                
                pEmployeeID = tmpEmp.pkID;
                pMon = Convert.ToInt64(drpSummaryMonth.SelectedValue);
                pYear = Convert.ToInt64(drpSummaryYear.SelectedValue);
                pWD = DateTime.DaysInMonth(Convert.ToInt16(pYear), Convert.ToInt16(pMon));
                pPayDate = drpSummaryYear.SelectedValue.ToString() + "-" + drpSummaryMonth.SelectedValue.ToString() + "-" + pWD.ToString();
                lblBasicPer = tmpEmp.BasicPer;
                fixedSalary = Convert.ToDecimal(tmpEmp.FixedSalary);


                // ---------------------------------------------------------
                if (pEmployeeID > 0)
                {
                    List<Entity.OrganizationEmployee> lstEntity = new List<Entity.OrganizationEmployee>();
                    lstEntity = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(pEmployeeID, 1, 99999, out TotalCount);

                    if (lstEntity.Count > 0)
                    {
                        PF_Calculation = lstEntity[0].PF_Calculation;
                        PT_Calculation = lstEntity[0].PT_Calculation;
                    }
                    //-------------------------Present Days (From Attendance) ---------------------
                    List<Entity.Attendance> lstEntity1 = new List<Entity.Attendance>();
                    lstEntity1 = BAL.AttendanceMgmt.GetAttendanceList(0, pEmployeeID, pMon, pYear);
                    
                    pPD = lstEntity1.Sum(item => item.WorkingHrsFlag);
                    //------------------------- Holidays (Sundays / Holidays) ---------------------
                    if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL" || hdnSerialKey.Value == "PI01-YU02-RUBB-03ER")    // M.N.Rubber
                    {
                        pHD = 0;
                    }
                    else
                    {
                        List<Entity.Holiday> lstEntity2 = new List<Entity.Holiday>();
                        lstEntity2 = BAL.HolidayMgmt.GetHolidayListByCount(pMon, pYear);
                        pHD = lstEntity2.Sum(item => item.TotalHolidays) + GetSundays(Convert.ToInt16(pMon), Convert.ToInt16(pYear));
                    }

                    //------------------------- Leave Days (From Leve Request) ---------------------
                    List<Entity.LeaveRequest> lstEntity3 = new List<Entity.LeaveRequest>();
                    lstEntity3 = BAL.LeaveRequestMgmt.GetLeaveRequestListByEmployeeID(pEmployeeID, pMon, pYear);
                    if (hdnSerialKey.Value == "PI01-YU02-RUBB-03ER")
                        pLD = 0;
                    else 
                        pLD = lstEntity3.Sum(item => item.TotalLeaveDays);

                    //------------------------- Overtime Hrs/Allow (From Overtime) ---------------------
                    Decimal retVal = BAL.OverTimeMgmt.GetOverTimeHours(pEmployeeID, pMon, pYear);
                    pOD = Math.Round(retVal / Convert.ToDecimal(60), 2);


                    pTotDays = ((pPD + pHD) - pLD);
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    Decimal tmpFixed = 0, tmpBasic = 0, tmpHRA = 0, tmpDA = 0, tmpConv = 0, tmpMedical = 0, tmpSpecial = 0, tmpOverTime = 0, tmpTotalInc = 0;
                    Decimal tmpPF = 0, tmpESI = 0, tmpPT = 0, tmpTDS = 0, tmpLoan = 0, tmpTotalDed = 0;
                    
                    tmpOverTime = BAL.OverTimeMgmt.GetOverTimeAllow(pEmployeeID, pMon, pYear);
                    // ----------------------------------------------------------------------------
                    // Save : AddUpdate
                    // ----------------------------------------------------------------------------
                    if (hdnSerialKey.Value == "H0PX-EMRW-23IJ-C1TD" || hdnSerialKey.Value == "SIV3-DIO4-09IK-98RE")    // Steelman Payslip Calculations
                    {
                        tmpFixed = Math.Round((Convert.ToDecimal(pPD) * Convert.ToDecimal(fixedSalary)), 0);
                        tmpBasic = tmpFixed;
                        tmpHRA = Math.Round((Convert.ToDecimal(pHD) * Convert.ToDecimal(fixedSalary)), 0);
                        tmpConv = 0;
                        tmpMedical = 0;
                        if (pEmployeeID>0  && pMon>0 && pYear>0)
                            tmpSpecial = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingAllowance(Convert.ToInt64(pEmployeeID), Convert.ToInt64(pMon), Convert.ToInt64(pYear)));
                        else
                            tmpSpecial = 0;

                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial));

                        tmpPF = 0;
                        tmpESI = 0;
                        tmpPT = 0;
                        tmpTDS = 0;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", pEmployeeID, pMon, pYear);

                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                    }
                    else if (hdnSerialKey.Value == "PI01-YU02-RUBB-03ER")    // Piyush Rubber
                    {
                        Decimal wd, pd, hd, od, ld, tdays;
                        wd = (!String.IsNullOrEmpty(pWD.ToString())) ? Convert.ToDecimal(pWD) : 0;
                        pd = (!String.IsNullOrEmpty(pPD.ToString())) ? Convert.ToDecimal(pPD) : 0;
                        hd = (!String.IsNullOrEmpty(pHD.ToString())) ? Convert.ToDecimal(pHD) : 0;
                        od = (!String.IsNullOrEmpty(pOD.ToString())) ? Convert.ToDecimal(pOD) : 0;
                        ld = (!String.IsNullOrEmpty(pLD.ToString())) ? Convert.ToDecimal(pLD) : 0;
                        tdays = ((pd + hd) - ld);

                        if (lblBasicPer.ToLower() == "daily")
                            tmpFixed = Math.Round((fixedSalary * tdays), 0);

                        // --------------------------------------------------------------------------------
                        tmpBasic = tmpFixed;
                        tmpHRA = 0;
                        tmpConv = 0;
                        tmpMedical = 0;
                        tmpSpecial = 0;
                        tmpOverTime = 0;
                        if (od > 0)
                        {
                            Decimal OTHrsRate = 0;
                            OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(pEmployeeID, pMon, pYear);
                            tmpOverTime = Math.Round((od * OTHrsRate), 0);
                        }

                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));


                        if (PF_Calculation == true)
                        {
                            tmpPF = (tmpTotalInc * 12 / 100);
                        }

                        tmpESI = 0;

                        if (PT_Calculation == true)
                        {
                            if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                                tmpPT = 80;
                            else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                                tmpPT = 150;
                            else if (tmpTotalInc > 11999)
                                tmpPT = 200;
                        }

                        tmpTDS = 0;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("", pEmployeeID, pMon, pYear);
                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);

                    }
                    else if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL")    // M.N.Rubber
                    {
                        Decimal wd, pd, hd, od, ld, tdays;
                        wd = (!String.IsNullOrEmpty(pWD.ToString())) ? Convert.ToDecimal(pWD) : 0;
                        pd = (!String.IsNullOrEmpty(pPD.ToString())) ? Convert.ToDecimal(pPD) : 0;
                        hd = (!String.IsNullOrEmpty(pHD.ToString())) ? Convert.ToDecimal(pHD) : 0;
                        od = (!String.IsNullOrEmpty(pOD.ToString())) ? Convert.ToDecimal(pOD) : 0;
                        ld = (!String.IsNullOrEmpty(pLD.ToString())) ? Convert.ToDecimal(pLD) : 0;
                        tdays = ((pd + hd) - ld);

                        if (lblBasicPer.ToLower() == "daily")
                            tmpFixed = Math.Round((fixedSalary * tdays), 0);
                        else
                            tmpFixed = Math.Round(((fixedSalary / wd) * tdays), 0);
                        // --------------------------------------------------------------------------------
                        tmpBasic = tmpFixed;
                        tmpHRA = 0;
                        tmpConv = 0;
                        tmpMedical = 0;
                        tmpSpecial = 0;
                        tmpOverTime = 0;
                        if (od > 0)
                        {
                            Decimal OTHrsRate = 0;
                            OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(pEmployeeID, pMon, pYear);
                            tmpOverTime = Math.Round((od * OTHrsRate), 0);
                        }

                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));


                        if (PF_Calculation == true)
                        {
                            tmpPF = (tmpTotalInc * 12 / 100);
                        }

                        tmpESI = 0;

                        if (PT_Calculation == true)
                        {
                            if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                                tmpPT = 80;
                            else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                                tmpPT = 150;
                            else if (tmpTotalInc > 11999)
                                tmpPT = 200;
                        }

                        tmpTDS = 0;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("", pEmployeeID, pMon, pYear); 

                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);

                    }
                    else if (hdnSerialKey.Value == "SO5H-DH90-E34L-SIOF")    // Soleos
                    {
                        Decimal wd, pd, hd, od, ld, tdays, fixsal, fixbasic, fixhra, fixconv, fixspecial;

                        wd = (!String.IsNullOrEmpty(pWD.ToString())) ? Convert.ToDecimal(pWD) : 0;
                        pd = (!String.IsNullOrEmpty(pPD.ToString())) ? Convert.ToDecimal(pPD) : 0;
                        hd = (!String.IsNullOrEmpty(pHD.ToString())) ? Convert.ToDecimal(pHD) : 0;
                        od = (!String.IsNullOrEmpty(pOD.ToString())) ? Convert.ToDecimal(pOD) : 0;
                        ld = (!String.IsNullOrEmpty(pLD.ToString())) ? Convert.ToDecimal(pLD) : 0;
                        tdays = (pd + hd);
                        // ---------------------------------------------------------------
                        fixsal = Convert.ToDecimal(fixedSalary);
                        fixbasic = lstEntity[0].FixedBasic;
                        fixhra = lstEntity[0].FixedHRA;
                        fixconv = lstEntity[0].FixedConv;
                        fixspecial = lstEntity[0].FixedSpecial;
                        // ---------------------------------------------------------------
                        tmpBasic = Math.Round(((fixbasic / wd) * tdays), 0);
                        tmpHRA = Math.Round(((fixhra / wd) * tdays), 0);
                        tmpConv = Math.Round(((fixconv / wd) * tdays), 0);
                        tmpMedical = 0;
                        tmpSpecial = Math.Round(((fixspecial / wd) * tdays), 0);
                        //if (od > 0)
                        //{
                        //    Decimal OTHrsRate = 0;
                        //    OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(Convert.ToInt64(drpEmployee.SelectedValue), Convert.ToInt64(hdnMonth.Value), Convert.ToInt64(hdnYear.Value));
                        //    tmpOverTime = Math.Round((od * OTHrsRate), 0);
                        //}

                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));


                        // tmpPF = (PF_Calculation == true) ? (tmpTotalInc * 12 / 100) : 0;
                        tmpPF = 0;
                        tmpESI = 0;
                        if (PT_Calculation == true)
                        {
                            if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                                tmpPT = 80;
                            else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                                tmpPT = 150;
                            else if (tmpTotalInc > 11999)
                                tmpPT = 200;
                        }
                        tmpTDS = 0;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", pEmployeeID, pMon, pYear);

                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);

                    }
                    else if (hdnSerialKey.Value == "PRI9-DG8H-G6GF-TP5V")    // Perfect Rotto Motors
                    {
                        List<Entity.Attendance> lstAttend = new List<Entity.Attendance>();
                        lstAttend = BAL.AttendanceMgmt.GetAttendanceList(0, pEmployeeID, Convert.ToInt16(pMon), Convert.ToInt16(pYear));
                        // ------------------------------------------------------
                        Decimal fixsal = 0, fixbasic = 0, fixhra = 0, fixconv = 0, fixspecial = 0;


                        if (lblBasicPer.ToLower() == "monthly" || lblBasicPer.ToLower() == "daily")
                            pPD = lstAttend.Sum(item => item.WorkingHrsFlag);
                        else
                        {
                            //decimal myHrs = 0;
                            //Decimal totot = 0, myMins = 0, myMins1 = 0, myMins2 = 0;
                            //totot = lstAttend.Sum(item => Convert.ToDecimal(item.WorkingTotalHrs));
                            //myHrs = Math.Floor(lstAttend.Sum(item => item.WorkingHrs));
                            //myMins1 = Math.Floor(lstAttend.Sum(item => item.WorkingMins) / 60);   // 11
                            //myMins2 = Convert.ToInt64(lstAttend.Sum(item => item.WorkingMins) - (myMins1 * 60));

                            //myHrs = myHrs + Convert.ToInt64(myMins1);
                            //myMins = myMins2;
                            //pPD = Convert.ToDecimal(myHrs.ToString("00") + "." + myMins.ToString("0"));

                            decimal myHrs = 0, myHrs1 = 0;
                            Decimal totot = 0, myMins = 0, myMins1 = 0, myMins2 = 0;

                            myHrs = lstAttend.Sum(item => Math.Floor(item.WorkingTotalHrs));    // 180
                            myMins1 = lstAttend.Sum(item => ((item.WorkingTotalHrs - Math.Floor(item.WorkingTotalHrs)) * 100));     // 145

                            myHrs1 = Math.Floor(myMins1 / 60);  // 2
                            myMins2 = Convert.ToInt64(myMins1 - (myHrs1 * 60)); // 25

                            pPD = Convert.ToDecimal((myHrs + myHrs1).ToString("00") + "." + myMins2.ToString("0"));
                        }

                        pHD = 0;
                        pOD = 0;
                        pLD = 0;
                        pTotDays = ((pPD + pHD));
                        // ---------------------------------------------------------------
                        fixsal = (!String.IsNullOrEmpty(fixedSalary.ToString())) ? Convert.ToDecimal(fixedSalary.ToString()) : 0;
                        fixbasic = fixsal;
                        fixhra = 0;
                        fixconv = 0;
                        fixspecial = 0;
                        // ---------------------------------------------------------------
                        // Income Side
                        // ---------------------------------------------------------------
                        tmpBasic = Math.Round((fixbasic * pPD), 0);
                        tmpHRA = 0;
                        tmpConv = 0;
                        tmpMedical = 0;
                        tmpSpecial = 0;
                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));

                        // ---------------------------------------------------------------
                        // Deduction Side
                        // ---------------------------------------------------------------
                        tmpPF = 0;
                        tmpESI = 0;
                        if (PT_Calculation == true)
                        {
                            if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                                tmpPT = 80;
                            else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                                tmpPT = 150;
                            else if (tmpTotalInc > 11999)
                                tmpPT = 200;
                        }
                        tmpTDS = 0;
                        tmpLoan = 0;
                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                    }
                    else
                    {                                              // Others Payslip Calculations

                        tmpFixed = Math.Round((((Convert.ToDecimal(pPD) + Convert.ToDecimal(pHD)) * Convert.ToDecimal(fixedSalary)) / Convert.ToInt64(pWD)), 0);
                        tmpBasic = Math.Round((40 * tmpFixed) / 100, 0);
                        tmpHRA = Math.Round((50 * tmpBasic) / 100, 0);
                        tmpConv = (tmpBasic > 0 && tmpHRA > 0) ? 1600 : 0;
                        tmpMedical = tmpBasic > 0 && tmpHRA > 0 ? 1250 : 0;
                        tmpSpecial = tmpBasic > 0 && tmpHRA > 0 ? (Convert.ToDecimal(tmpFixed) - (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical))) : 0;
                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial));

                        if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                            tmpPT = 80;
                        else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                            tmpPT = 150;
                        else if (tmpTotalInc > 11999)
                            tmpPT = 200;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", pEmployeeID, pMon, pYear);
                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                    }
                    tmpNetSalary = (tmpTotalInc - tmpTotalDed);
                    // -------------------------------------------------------
                    Entity.Payroll objEntity = new Entity.Payroll();

                    objEntity.pkID = 0;
                    objEntity.EmployeeID = pEmployeeID;
                    objEntity.PayDate = Convert.ToDateTime(pPayDate);
                    objEntity.WDays = (!String.IsNullOrEmpty(pWD.ToString())) ? Convert.ToInt64(pWD) : 0;
                    objEntity.PDays = (!String.IsNullOrEmpty(pPD.ToString())) ? Convert.ToDecimal(pPD) : 0;
                    objEntity.HDays = (!String.IsNullOrEmpty(pHD.ToString())) ? Convert.ToDecimal(pHD) : 0;
                    objEntity.ODays = (!String.IsNullOrEmpty(pOD.ToString())) ? Convert.ToDecimal(pOD) : 0;
                    objEntity.LDays = (!String.IsNullOrEmpty(pLD.ToString())) ? Convert.ToDecimal(pLD) : 0;
                    objEntity.FixedSalary = (!String.IsNullOrEmpty(fixedSalary.ToString())) ? Convert.ToDecimal(fixedSalary.ToString()) : 0;

                    objEntity.Basic = (!String.IsNullOrEmpty(tmpBasic.ToString())) ? Convert.ToDecimal(tmpBasic.ToString()) : 0;
                    objEntity.HRA = (!String.IsNullOrEmpty(tmpHRA.ToString())) ? Convert.ToDecimal(tmpHRA.ToString()) : 0;
                    objEntity.DA = (!String.IsNullOrEmpty(tmpDA.ToString())) ? Convert.ToDecimal(tmpDA.ToString()) : 0;
                    objEntity.Conveyance = (!String.IsNullOrEmpty(tmpConv.ToString())) ? Convert.ToDecimal(tmpConv.ToString()) : 0;
                    objEntity.Medical = (!String.IsNullOrEmpty(tmpMedical.ToString())) ? Convert.ToDecimal(tmpMedical.ToString()) : 0;
                    objEntity.Special = (!String.IsNullOrEmpty(tmpSpecial.ToString())) ? Convert.ToDecimal(tmpSpecial.ToString()) : 0;
                    objEntity.OverTime = (!String.IsNullOrEmpty(tmpOverTime.ToString())) ? Convert.ToDecimal(tmpOverTime.ToString()) : 0;
                    objEntity.Total_Income = Convert.ToDecimal(tmpTotalInc.ToString());

                    objEntity.PF = (!String.IsNullOrEmpty(tmpPF.ToString())) ? Convert.ToDecimal(tmpPF.ToString()) : 0;
                    objEntity.ESI = (!String.IsNullOrEmpty(tmpESI.ToString())) ? Convert.ToDecimal(tmpESI.ToString()) : 0;
                    objEntity.PT = (!String.IsNullOrEmpty(tmpPT.ToString())) ? Convert.ToDecimal(tmpPT.ToString()) : 0;
                    objEntity.TDS = (!String.IsNullOrEmpty(tmpTDS.ToString())) ? Convert.ToDecimal(tmpTDS.ToString()) : 0;
                    objEntity.Loan = (!String.IsNullOrEmpty(tmpLoan.ToString())) ? Convert.ToDecimal(tmpLoan.ToString()) : 0;
                    objEntity.Total_Deduct = Convert.ToDecimal(tmpTotalDed.ToString());
                    objEntity.NetSalary = Convert.ToDecimal(tmpNetSalary.ToString());

                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.PayrollMgmt.AddUpdatePayroll(objEntity, out ReturnCode, out ReturnMsg);

                }
            }
            // ----------------------------------
            BindPayroll();
        }       // btnAutoGenerate_Click
        public int GetSundays(int pMon, int pYear)
        {
            int returnVal = 0, month = 0, year = 0;

            if (pMon>0 && pYear>0)
            {
                var firstDay = new DateTime(pYear, pMon, 1);

                var day29 = firstDay.AddDays(28);
                var day30 = firstDay.AddDays(29);
                var day31 = firstDay.AddDays(30);

                if ((day29.Month == pMon && day29.DayOfWeek == DayOfWeek.Sunday)
                || (day30.Month == pMon && day30.DayOfWeek == DayOfWeek.Sunday)
                || (day31.Month == pMon && day31.DayOfWeek == DayOfWeek.Sunday))
                {
                    returnVal = 5;
                }
                else
                {
                    returnVal = 4;
                }
            }
            return returnVal;
        }
        protected void btnAutoReCalculate_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:$('#loading').show();", true);
            Int64 pMon, pYear;
            pMon = Convert.ToInt64(drpSummaryMonth.SelectedValue);
            pYear = Convert.ToInt64(drpSummaryYear.SelectedValue);
            // ----------------------------------------------------------


            List<Entity.Payroll> lstPayroll = new List<Entity.Payroll>();
            lstPayroll = BAL.PayrollMgmt.GetPayrollList(0, pMon, pYear, 1, 99999, out totrec);

            foreach (Entity.Payroll tmpPay in lstPayroll)
            {
                int TotalCount = 0;
                bool PF_Calculation = false, PT_Calculation = false;
                Int64 pEmployeeID = 0;
                Decimal pWD = 0, pPD = 0, pHD = 0, pOD = 0, pLD = 0, pTotDays = 0;
                Decimal fixedSalary = 0, tmpNetSalary = 0;
                String lblBasicPer = "", pPayDate = "";
                // ---------------------------------------------------------
                pEmployeeID = tmpPay.EmployeeID;


                // ---------------------------------------------------------
                if (pEmployeeID > 0)
                {
                    List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
                    lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList(pEmployeeID, 1, 9999, out totrec);

                    pWD = tmpPay.WDays;             // DateTime.DaysInMonth(Convert.ToInt16(pYear), Convert.ToInt16(pMon));
                    pPayDate = tmpPay.PayDate.ToString();      // drpSummaryYear.SelectedValue.ToString() + "-" + drpSummaryMonth.SelectedValue.ToString() + "-" + pWD.ToString();
                    fixedSalary = Convert.ToDecimal(tmpPay.FixedSalary);
                    lblBasicPer = lstEmployee[0].BasicPer;

                    if (lstEmployee.Count > 0)
                    {
                        PF_Calculation = lstEmployee[0].PF_Calculation;
                        PT_Calculation = lstEmployee[0].PT_Calculation;
                    }
                    //-------------------------Present Days (From Attendance) ---------------------
                    pPD = tmpPay.PDays;
                    pHD = tmpPay.HDays;
                    pLD = tmpPay.LDays;
                    pOD = tmpPay.ODays;
                    pTotDays = ((pPD + pHD) - pLD);
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    Decimal tmpFixed = 0, tmpBasic = 0, tmpHRA = 0, tmpDA = 0, tmpConv = 0, tmpMedical = 0, tmpSpecial = 0, tmpOverTime = 0, tmpTotalInc = 0;
                    Decimal tmpPF = 0, tmpESI = 0, tmpPT = 0, tmpTDS = 0, tmpLoan = 0, tmpTotalDed = 0;

                    tmpOverTime = BAL.OverTimeMgmt.GetOverTimeAllow(pEmployeeID, pMon, pYear);
                    // ----------------------------------------------------------------------------
                    // Save : AddUpdate
                    // ----------------------------------------------------------------------------
                    if (hdnSerialKey.Value == "H0PX-EMRW-23IJ-C1TD" || hdnSerialKey.Value == "SIV3-DIO4-09IK-98RE")    // Steelman Payslip Calculations
                    {
                        tmpFixed = Math.Round((Convert.ToDecimal(pPD) * Convert.ToDecimal(fixedSalary)), 0);
                        tmpBasic = tmpFixed;
                        tmpHRA = Math.Round((Convert.ToDecimal(pHD) * Convert.ToDecimal(fixedSalary)), 0);
                        tmpConv = 0;
                        tmpMedical = 0;
                        if (pEmployeeID > 0 && pMon > 0 && pYear > 0)
                            tmpSpecial = Convert.ToDecimal(BAL.CommonMgmt.GetDrivingAllowance(Convert.ToInt64(pEmployeeID), Convert.ToInt64(pMon), Convert.ToInt64(pYear)));
                        else
                            tmpSpecial = 0;

                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial));

                        tmpPF = 0;
                        tmpESI = 0;
                        tmpPT = 0;
                        tmpTDS = 0;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", pEmployeeID, pMon, pYear);

                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                    }
                    else if (hdnSerialKey.Value == "LVK4-MN01-K121-NGVL")    // M.N.Rubber
                    {
                        Decimal wd, pd, hd, od, ld, tdays;
                        wd = (!String.IsNullOrEmpty(pWD.ToString())) ? Convert.ToDecimal(pWD) : 0;
                        pd = (!String.IsNullOrEmpty(pPD.ToString())) ? Convert.ToDecimal(pPD) : 0;
                        hd = (!String.IsNullOrEmpty(pHD.ToString())) ? Convert.ToDecimal(pHD) : 0;
                        od = (!String.IsNullOrEmpty(pOD.ToString())) ? Convert.ToDecimal(pOD) : 0;
                        ld = (!String.IsNullOrEmpty(pLD.ToString())) ? Convert.ToDecimal(pLD) : 0;
                        tdays = ((pd + hd) - ld);

                        if (lblBasicPer.ToLower() == "daily")
                            tmpFixed = Math.Round((fixedSalary * tdays), 0);
                        else
                            tmpFixed = Math.Round(((fixedSalary / wd) * tdays), 0);
                        // --------------------------------------------------------------------------------
                        tmpBasic = tmpFixed;
                        tmpHRA = tmpPay.HRA;
                        tmpConv = tmpPay.Conveyance;
                        tmpMedical = tmpPay.Medical;
                        tmpSpecial = tmpPay.Special;
                        tmpOverTime = tmpPay.OverTime;
                        if (od > 0)
                        {
                            Decimal OTHrsRate = 0;
                            OTHrsRate = BAL.OverTimeMgmt.GetOverTimeAllow(pEmployeeID, pMon, pYear);
                            tmpOverTime = Math.Round((od * OTHrsRate), 0);
                        }

                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial) + Convert.ToDecimal(tmpOverTime));


                        if (PF_Calculation == true)
                        {
                            tmpPF = (tmpTotalInc * 12 / 100);
                        }

                        tmpESI = 0;

                        if (PT_Calculation == true)
                        {
                            if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                                tmpPT = 80;
                            else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                                tmpPT = 150;
                            else if (tmpTotalInc > 11999)
                                tmpPT = 200;
                        }

                        tmpTDS = 0;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("", pEmployeeID, pMon, pYear);

                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);

                    }
                    else
                    {                                              // Others Payslip Calculations

                        tmpFixed = Math.Round((((Convert.ToDecimal(pPD) + Convert.ToDecimal(pHD)) * Convert.ToDecimal(fixedSalary)) / Convert.ToInt64(pWD)), 0);
                        tmpBasic = Math.Round((40 * tmpFixed) / 100, 0);
                        tmpHRA = Math.Round((50 * tmpBasic) / 100, 0);
                        tmpConv = (tmpBasic > 0 && tmpHRA > 0) ? 1600 : 0;
                        tmpMedical = tmpBasic > 0 && tmpHRA > 0 ? 1250 : 0;
                        tmpSpecial = tmpBasic > 0 && tmpHRA > 0 ? (Convert.ToDecimal(tmpFixed) - (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical))) : 0;
                        tmpTotalInc = (Convert.ToDecimal(tmpBasic) + Convert.ToDecimal(tmpHRA) + Convert.ToDecimal(tmpConv) + Convert.ToDecimal(tmpMedical) + Convert.ToDecimal(tmpSpecial));

                        if (tmpTotalInc > 5999 && tmpTotalInc <= 8999)
                            tmpPT = 80;
                        else if (tmpTotalInc > 8999 && tmpTotalInc <= 11999)
                            tmpPT = 150;
                        else if (tmpTotalInc > 11999)
                            tmpPT = 200;
                        tmpLoan = BAL.LoanMgmt.GetLoanInstallmentAmount("upad", pEmployeeID, pMon, pYear);
                        tmpTotalDed = Convert.ToDecimal(tmpPF) + Convert.ToDecimal(tmpESI) + Convert.ToDecimal(tmpPT) + Convert.ToDecimal(tmpTDS) + Convert.ToDecimal(tmpLoan);
                    }
                    tmpNetSalary = (tmpTotalInc - tmpTotalDed);
                    // -------------------------------------------------------
                    Entity.Payroll objEntity = new Entity.Payroll();

                    objEntity.pkID = tmpPay.pkID;
                    objEntity.EmployeeID = pEmployeeID;
                    objEntity.PayDate = Convert.ToDateTime(pPayDate);
                    objEntity.WDays = (!String.IsNullOrEmpty(pWD.ToString())) ? Convert.ToInt64(pWD) : 0;
                    objEntity.PDays = (!String.IsNullOrEmpty(pPD.ToString())) ? Convert.ToDecimal(pPD) : 0;
                    objEntity.HDays = (!String.IsNullOrEmpty(pHD.ToString())) ? Convert.ToDecimal(pHD) : 0;
                    objEntity.ODays = (!String.IsNullOrEmpty(pOD.ToString())) ? Convert.ToDecimal(pOD) : 0;
                    objEntity.LDays = (!String.IsNullOrEmpty(pLD.ToString())) ? Convert.ToDecimal(pLD) : 0;
                    objEntity.FixedSalary = (!String.IsNullOrEmpty(fixedSalary.ToString())) ? Convert.ToDecimal(fixedSalary.ToString()) : 0;

                    objEntity.Basic = (!String.IsNullOrEmpty(tmpBasic.ToString())) ? Convert.ToDecimal(tmpBasic.ToString()) : 0;
                    objEntity.HRA = (!String.IsNullOrEmpty(tmpHRA.ToString())) ? Convert.ToDecimal(tmpHRA.ToString()) : 0;
                    objEntity.DA = (!String.IsNullOrEmpty(tmpDA.ToString())) ? Convert.ToDecimal(tmpDA.ToString()) : 0;
                    objEntity.Conveyance = (!String.IsNullOrEmpty(tmpConv.ToString())) ? Convert.ToDecimal(tmpConv.ToString()) : 0;
                    objEntity.Medical = (!String.IsNullOrEmpty(tmpMedical.ToString())) ? Convert.ToDecimal(tmpMedical.ToString()) : 0;
                    objEntity.Special = (!String.IsNullOrEmpty(tmpSpecial.ToString())) ? Convert.ToDecimal(tmpSpecial.ToString()) : 0;
                    objEntity.OverTime = (!String.IsNullOrEmpty(tmpOverTime.ToString())) ? Convert.ToDecimal(tmpOverTime.ToString()) : 0;
                    objEntity.Total_Income = Convert.ToDecimal(tmpTotalInc.ToString());

                    objEntity.PF = (!String.IsNullOrEmpty(tmpPF.ToString())) ? Convert.ToDecimal(tmpPF.ToString()) : 0;
                    objEntity.ESI = (!String.IsNullOrEmpty(tmpESI.ToString())) ? Convert.ToDecimal(tmpESI.ToString()) : 0;
                    objEntity.PT = (!String.IsNullOrEmpty(tmpPT.ToString())) ? Convert.ToDecimal(tmpPT.ToString()) : 0;
                    objEntity.TDS = (!String.IsNullOrEmpty(tmpTDS.ToString())) ? Convert.ToDecimal(tmpTDS.ToString()) : 0;
                    objEntity.Loan = (!String.IsNullOrEmpty(tmpLoan.ToString())) ? Convert.ToDecimal(tmpLoan.ToString()) : 0;
                    objEntity.Total_Deduct = Convert.ToDecimal(tmpTotalDed.ToString());
                    objEntity.NetSalary = Convert.ToDecimal(tmpNetSalary.ToString());

                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.PayrollMgmt.AddUpdatePayroll(objEntity, out ReturnCode, out ReturnMsg);

                }
            }
            // ----------------------------------
            //https://www.codeproject.com/Questions/335354/Update-Progress-is-not-working
            BindPayroll();
            //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:$('#loading').hide();", true);
        }   // btnAutoReCalculate_Click

        protected void btnSelectAll_ServerClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptPayroll.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkCtrl = (CheckBox)item.FindControl("chkSelect");
                    chkCtrl.Checked = true;
                }
            }
        }
    }
}