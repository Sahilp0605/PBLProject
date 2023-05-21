using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Web.UI;
using System.Globalization;
using iTextSharp.text.html;

namespace StarsProject
{
    public class Class1
    {
        // TODO: Replace the following with your gateway instance ID, Forever Green client ID and secret:
        private static string INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
        private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
        private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

        private static string API_URL = "http://api.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;

        static void Main(string[] args)
        {
            Class1 msgSender = new Class1();
            //msgSender.sendMessage("12025550108", "Isn't this excting?");  //  Specify the recipient's number here. NOT the gateway number
            //Console.WriteLine("Press Enter to exit.");
            //Console.ReadLine();
        }

        public bool sendMessage(string number, string message)
        {
            bool success = true;
        
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                    client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                    Payload payloadObj = new Payload() { number = number, message = message };
                    string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                    client.Encoding = Encoding.UTF8;
                    string response = client.UploadString(API_URL, postData);
                    Console.WriteLine(response);
                }
            }
            catch (WebException webEx)
            {
                Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
                Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                String body = reader.ReadToEnd();
                Console.WriteLine(body);
                success = false;
            }

            return success;
        }

        public class Payload
        {
            public string number { get; set; }
            public string message { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public void GenerateQuotation_Sharvaya_Orig(Int64 pQuotID)
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

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(8);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Quotation> lstQuot = new List<Entity.Quotation>();
            lstQuot = BAL.QuotationMgmt.GetQuotationList(pQuotID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.QuotationDetailMgmt.GetQuotationDetail(lstQuot[0].QuotationNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);
            //-------------------------------------------------------------------------------------------------
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            if (lstQuot.Count > 0)
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(lstQuot[0].CompanyID, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ------------------------------------------------------------------------------
            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");

            Int64 ProdDetail_Lines = 20;
            ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            Document pdfDoc = pdf.initiatePage(lstPrinter);
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
                int[] column_tblMember = { 25, 20, 33, 22 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblNested2 = new PdfPTable(4);
                int[] column_tblNested2 = { 24, 35, 15, 26 };
                tblNested2.SetWidths(column_tblNested2);

                tblNested2.AddCell(pdf.setCell("Offer No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].QuotationNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].QuotationDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].QuotationKindAttn, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Executive Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].CreatedEmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("Contact No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].CreatedEmployeeMobileNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("GST No ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell("PAN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(pdf.setCell(": " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblMember.AddCell(pdf.setCell("Quotation", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(pdf.Quotation_CustomerInfo(lstCust), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                if (sumDis > 0)
                {
                    int[] column_tblNested = { 6, 10, 33, 6, 6, 10, 8, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                else
                {
                    int[] column_tblNested = { 6, 10, 39, 6, 6, 10, 1, 12 };
                    tblDetail.SetWidths(column_tblNested);
                }
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Discount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }

                decimal totAmount = 0, taxAmount = 0, netAmount = 0;

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    //tmpAmount = (Convert.ToDecimal(dtItem.Rows[i]["Quantity"]) * Convert.ToDecimal(dtItem.Rows[i]["NetRate"]));
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    //netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " -";
                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    if (sumDis > 0)
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountAmt"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }


                    string spcs = dtItem.Rows[i]["ProductSpecification"].ToString();
                    if (!String.IsNullOrEmpty(spcs))
                    {
                        tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductSpecification"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        if (sumDis > 0)
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                    }
                }

                //for (int i = 1; i < (13 - dtItem.Rows.Count); i++)
                //{

                if (ProdDetail_Lines > dtItem.Rows.Count)
                {
                    for (int i = 1; i <= (ProdDetail_Lines - dtItem.Rows.Count); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        if (sumDis > 0)
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        }
                    }
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                PdfPTable tblTNC = new PdfPTable(1);
                int[] column_tblTNC = { 100 };
                tblTNC.SetWidths(column_tblTNC);
                tblTNC.AddCell(pdf.setCell("Terms & Conditions", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(lstQuot[0].QuotationFooter, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // ---------------------------------------------------------------------------------------------------------
                PdfPTable tblAmount = new PdfPTable(2);
                int[] column_tblAmount = { 60, 40 };
                tblAmount.SetWidths(column_tblAmount);
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                totAmount = lstQuot[0].BasicAmt;
                totRNDOff = lstQuot[0].ROffAmt;
                totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt1;
                    befGST += lstQuot[0].ChargeGSTAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt2;
                    befGST += lstQuot[0].ChargeGSTAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt3;
                    befGST += lstQuot[0].ChargeGSTAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt4;
                    befGST += lstQuot[0].ChargeGSTAmt4;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    befAmt += lstQuot[0].ChargeAmt5;
                    befGST += lstQuot[0].ChargeGSTAmt5;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                //if (befAmt > 0)
                //{
                //    tblAmount.AddCell(setCell("Total     :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(setCell((totAmount + befAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                {
                    if (lstQuot[0].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        else
                            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        //tblAmount.AddCell(setCell((totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                    else
                    {
                        if ((lstQuot[0].CGSTAmt + lstQuot[0].SGSTAmt) > 0)
                        {
                            tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            else
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                            tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                            else
                                tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        }
                    }
                }
                //tblAmount.AddCell(setCell("Total     :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                //tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST)).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));

                /* ---------------------------------------------------------- */
                if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt1;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt2;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt3;
                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt4;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                {
                    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                    aftAmt += lstQuot[0].ChargeAmt5;

                    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + " @" + chrgPer.ToString() + "% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }
                /* ---------------------------------------------------------- */
                if (lstQuot[0].ROffAmt > 0)
                {
                    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    else
                        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                }

                tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //tblAmount.AddCell(setCell("Grand Total  :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuot[0].CurrencySymbol.Trim() + " " + ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                }

                // ****************************************************************
                //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
                netAmount = lstQuot[0].NetAmt;
                PdfPTable tblAmount1 = new PdfPTable(2);
                int[] column_tblAmount1 = { 60, 40 };
                tblAmount1.SetWidths(column_tblAmount1);
                string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                tblAmount1.AddCell(pdf.setCell("Total Amount ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                if (sumDis > 0)
                {
                    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                #endregion
                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------


                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);

                tblFootDetail.AddCell(setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblFootDetail.AddCell(setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblFootDetail.AddCell(setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(setCell("Bank Name : " + ((lstQuot.Count > 0) ? lstQuot[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(setCell("Branch    : " + ((lstQuot.Count > 0) ? lstQuot[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(setCell("A/c No    : " + ((lstQuot.Count > 0) ? lstQuot[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(setCell("IFSC Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //tblFootDetail.AddCell(pdf.setCell(pdf.BankDetails(lstQuot, 0, 1), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // ---------------------------------------------------
                int[] column_tblFooter = { 80, 20 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
                tblFooter.AddCell(setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 0), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(setCell("SUBJECT TO " + lstOrg[0].CityName.ToUpper() + "JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].QuotationNo.Replace("/", "-").ToString() + ".pdf";
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

            // >>>>>> Adding Organization Name 
            //tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //pdfDoc.Add(tableHeader);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);


            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Footer
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);

            // >>>>>> Adding Quotation Header
            tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSignOff);

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

        // -------------------------------------------------------------

        private static PdfPTable setPdfPTable(Document tmpPdf, int tblCols, int tblWidth, int befSpec, int aftSpec, int hAlign = Element.ALIGN_LEFT)
        {
            PdfPTable tmpTable = new PdfPTable(tblCols);
            tmpTable.TotalWidth = (tblWidth == 0) ? (tmpPdf.PageSize.Width - (tmpPdf.LeftMargin + tmpPdf.RightMargin)) : tblWidth;
            tmpTable.SpacingBefore = befSpec;
            tmpTable.SpacingAfter = aftSpec;
            tmpTable.LockedWidth = true;
            tmpTable.HorizontalAlignment = hAlign;
            tmpTable.DefaultCell.Border = PdfPCell.NO_BORDER;
            return tmpTable;
        }
        private static PdfPTable setPdfPTable(Document tmpPdf, int tblCols, int tblWidth, string befaftSpac, int hAlign = Element.ALIGN_LEFT)
        {
            PdfPTable tmpTable = new PdfPTable(tblCols);
            tmpTable.TotalWidth = (tblWidth == 0) ? (tmpPdf.PageSize.Width - (tmpPdf.LeftMargin + tmpPdf.RightMargin)) : tblWidth;
            if (!String.IsNullOrEmpty(befaftSpac))
            {
                String[] tmpary = befaftSpac.Split(',');
                tmpTable.SpacingBefore = (tmpary.Length > 0) ? (float)Convert.ToDouble(tmpary[0].ToString()) : 0;
                tmpTable.SpacingAfter = (tmpary.Length > 0) ? (float)Convert.ToDouble(tmpary[1].ToString()) : 0;
            }
            else
            {
                tmpTable.SpacingBefore = 0;
                tmpTable.SpacingAfter = 0;
            }
            tmpTable.LockedWidth = true;
            tmpTable.HorizontalAlignment = hAlign;
            tmpTable.DefaultCell.Border = PdfPCell.NO_BORDER;
            return tmpTable;
        }
        private static PdfPCell setCell(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCell(Phrase txtPhrase, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(txtPhrase);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCell(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float[] padTRBL, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
            tmpCell.BackgroundColor = bc;
            tmpCell.PaddingTop = (padTRBL[0] != null) ? padTRBL[0] : 0;
            tmpCell.PaddingRight = (padTRBL[1] != null) ? padTRBL[1] : 0;
            tmpCell.PaddingBottom = (padTRBL[2] != null) ? padTRBL[2] : 0;
            tmpCell.PaddingLeft = (padTRBL[3] != null) ? padTRBL[3] : 0;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCell(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(pImage, true);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCellFixImage(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(pImage, false);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCell(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(objTable);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = borderVal;
            return tmpCell;
        }
        // ---------------------------------------------------------------
        // Transparant Background For SetCell
        // ---------------------------------------------------------------
        private static PdfPCell setCellTransparent(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
            //tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCellTransparent(Phrase txtPhrase, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(txtPhrase);
            //tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCellTransparent(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float[] padTRBL, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
            //tmpCell.BackgroundColor = bc;
            tmpCell.PaddingTop = (padTRBL[0] != null) ? padTRBL[0] : 0;
            tmpCell.PaddingRight = (padTRBL[1] != null) ? padTRBL[1] : 0;
            tmpCell.PaddingBottom = (padTRBL[2] != null) ? padTRBL[2] : 0;
            tmpCell.PaddingLeft = (padTRBL[3] != null) ? padTRBL[3] : 0;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCellTransparent(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(pImage, true);
            //tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }
        private static PdfPCell setCellTransparent(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(objTable);
            //tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = borderVal;
            return tmpCell;
        }
        // -------------------------------------------------------------------------------
        private static PdfPCell TrasSetCell(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(objTable);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = borderVal;
            return tmpCell;
        }
        private static Paragraph setParagraph(string txtStr, BaseColor bc, iTextSharp.text.Font fn, int hAlign = Element.ALIGN_LEFT)
        {
            //tmpCell.Padding = pad;
            //tmpCell.Colspan = colspn;
            //tmpCell.HorizontalAlignment = hAlign;
            //tmpCell.VerticalAlignment = vAlign;
            //tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //tmpCell.Border = borderVal;

            Paragraph p1 = new Paragraph();
            Chunk c1 = new Chunk(txtStr);
            c1.SetBackground(bc);
            c1.Font = fn;

            p1.Add(c1);
            p1.Alignment = hAlign;

            return p1;
        }
        private static void addWallPaper(Document xDoc, string xImagePath, float xPos = 0, float yPos = 0, float xWidth = 10, float xHeight = 10)
        {
            if (!String.IsNullOrEmpty(xImagePath))
            {
                var objWall = iTextSharp.text.Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath(xImagePath));
                objWall.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                objWall.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
                objWall.BorderColor = BaseColor.GRAY;
                objWall.BorderWidth = 1f;
                objWall.SpacingBefore = 150;
                objWall.ScaleAbsolute(xWidth - 100f, xHeight - 100f);

                xDoc.Add(objWall);
            }

        }

    }
}