using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Globalization;
using iTextSharp.text.html;

namespace StarsProject.Services
{
    /// <summary>
    /// Summary description for DashboardService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    

    [System.Web.Script.Services.ScriptService]
    public class DashboardService : System.Web.Services.WebService
    {

        int totrec = 0;
        int ReturnCode = 0;
        string ReturnMsg = "";

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendDailyReport(string ToEmailAddress)
        {
            string currUserID = "", returnPDFFile = "", xStartDate = "", xEndDate = "", returnMessage = "";
            currUserID = Session["LoginUserID"].ToString();
            xStartDate = DateTime.Now.ToString("yyyy-MM-dd");
            xEndDate = DateTime.Now.ToString("yyyy-MM-dd");
            // ----------------------------------------------------------
            returnPDFFile = webGenerateDailyReport(xStartDate, xEndDate, currUserID);
            if (File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/") + returnPDFFile))
            {
                Entity.Authenticate objAuth = new Entity.Authenticate();
                objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                List<Entity.CompanyProfile> lstCompany = new List<Entity.CompanyProfile>();
                lstCompany = BAL.CommonMgmt.GetCompanyProfileList(objAuth.CompanyID, objAuth.UserID);
                // ----------------------------------------------------------    
                returnMessage = BAL.CommonMgmt.SendDailyReportNotification("DAILYREPORT", ToEmailAddress, returnPDFFile, xStartDate, xEndDate);
            }
            else
            {
                returnMessage = "Email Failed, Due To Daily Report File Not Generated !";
            }
            // ----------------------------------------------------------
            return returnMessage;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string webGenerateDailyReport(String startdt, String enddt, String loginuserid)
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
            if (t1.Rows.Count > 0 || t2.Rows.Count > 0)
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

                tblNested2.AddCell(pdf.setCell("Period From :" + Convert.ToDateTime(startdt).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(enddt).ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));

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
                for (int y = 17; y <= 31; y++)
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
                    tblFollowUp.AddCell(pdf.setCell((k + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_CENTER, 15));
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
            string sFileName = "DailyReport-" + loginuserid + "-" + Convert.ToDateTime(startdt).ToString("ddMMMyyyy") + "-" + Convert.ToDateTime(enddt).ToString("ddMMMyyyy") + ".pdf";
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

            if (dtQuotation.Rows.Count > 0)
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
            RecompressPDF(sPath + smallFileName, pdfFileName);
            System.IO.File.Delete(sPath + smallFileName);
            return sFileName;
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

    }
}
