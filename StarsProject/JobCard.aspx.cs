using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class JobCard : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 15;

                Session.Remove("dtDetail");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    BindDropdown();
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
        }

        public void BindDropdown()
        {
            //---------------------------Location Details-------------------------------
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            lstLocation = BAL.CommonMgmt.GetLocationList();
            drpLocation.DataSource = lstLocation;
            drpLocation.DataValueField = "LocationID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

        }
        public void BindInvoiceList(Int64 pCustomerID)
        {
            List<Entity.SalesBill> lstEntity = new List<Entity.SalesBill>();
            drpInvoiceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.SalesBillMgmt.GetInvoiceListByCustomer(pCustomerID);
                if (lstEntity.Count > 0)
                {
                    drpInvoiceNo.DataSource = lstEntity;
                    drpInvoiceNo.DataValueField = "InvoiceNo";
                    drpInvoiceNo.DataTextField = "InvoiceNo";
                    drpInvoiceNo.DataBind();
                }
                drpInvoiceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpInvoiceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                BindInvoiceList(Convert.ToInt64(hdnCustomerID.Value));
        }

        public void OnlyViewControls()
        {
            txtJobCardNo.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtCollectedFrom.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtDateIn.ReadOnly = true;
            txtDateReturn.ReadOnly = true;
            txtWheelNo.ReadOnly = true;
            drpInvoiceNo.Attributes.Add("disabled", "disabled");
            txtDeliveryNoteNo.ReadOnly = true;
            drpTyre.Attributes.Add("disabled", "disabled");
            drpCap.Attributes.Add("disabled", "disabled");
            drpSensor.Attributes.Add("disabled", "disabled");
            drpSensorValue.Attributes.Add("disabled", "disabled");
            txtRemarks.ReadOnly = true;
            txtPartNumber.ReadOnly = true;
            txtWheelMake.ReadOnly = true;
            txtStraightenedMeasurement.ReadOnly = true;
            txtClaimNo.ReadOnly = true;
            txtRegNo.ReadOnly = true;
            txtChassisNo.ReadOnly = true;
            txtPaintCode.ReadOnly = true;
            txtComment.ReadOnly = true;
            drpDiamondCut.Attributes.Add("disabled", "disabled");
            txtStartDate.ReadOnly = true;
            txtQualityCheck.ReadOnly = true;
            txtEstimatePrice.ReadOnly = true;
            txtDeliveryDate.ReadOnly = true;
            drpLocation.Attributes.Add("disabled", "disabled");
            txtBuyerRef.ReadOnly = true;
            btnSave.Disabled = false;
            btnReset.Disabled = false;

        }
     
        [System.Web.Services.WebMethod]
        public static string GetJobCardNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetJobCardNo(pkID);
            return tempVal;
        }
        [System.Web.Services.WebMethod]
        public static void GenerateJobCard_TWS(Int64 pkID)
        {
            //HttpContext.Current.Session["printheader"] = "no";
            HttpContext.Current.Session["printModule"] = "jobcard";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(2);
            PdfPTable tblDetail = new PdfPTable(5);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Proforma");

            ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                }
            }

            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);

            //Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            //pdfDoc.SetMargins(30, 30, 40, 0);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
            // -------------------------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.JobCard> lstQuot = new List<Entity.JobCard>();
            lstQuot = BAL.JobCardMgmt.GetJobCardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(),"", 1, 1000, out TotalCount);
            //--------------------------------------------------------------------------------------------------------------
            //List<Entity.SalesBill> lstExp = new List<Entity.SalesBill>();
            //lstExp = BAL.SalesBillMgmt.GetSalesBillExportList(pkID, lstQuot[0].InvoiceNo, HttpContext.Current.Session["LoginUserID"].ToString());
            //// -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.JobCardMgmt.GetJobCardDetail(lstQuot[0].JobCardNo);
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstQuot[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstQuot.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);
            //-------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
            if (lstQuot.Count > 0)
                lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(1, 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information


                //int[] column_tableHeader = { 100 };
                //tableHeader.SetWidths(column_tableHeader);
                //tableHeader.SpacingBefore = 8f;
                //tableHeader.LockedWidth = true;
                ////tableHeader.AddCell(pdf.setCell("Original", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

                int[] column_tblMember = { 70, 30 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;


                //------------------Steelman image-----------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\proforma.jpg";
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(65, 65);

                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }

                PdfPTable tblCompany = new PdfPTable(1);
                int[] column_tblCompany = { 100 };
                tblCompany.SetWidths(column_tblCompany);

                tblCompany.AddCell(pdf.setCell(lstOrg[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCompany.AddCell(pdf.setCell(lstOrg[0].CityName + " - " + lstOrg[0].Pincode + ", " + lstOrg[0].StateName.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCompany.AddCell(pdf.setCell("Mobile : " + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCompany.AddCell(pdf.setCell("Email :" + lstOrg[0].EmailAddress.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCompany.AddCell(pdf.setCell("Website: " + lstOrg[0].Fax1, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblSteel = new PdfPTable(2);
                int[] column_tblSteel = { 50, 50 };
                tblSteel.SetWidths(column_tblSteel);

                tblSteel.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSteel.AddCell(pdf.setCell(tblCompany, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                
                PdfPTable tblInvoiceD = new PdfPTable(2);
                int[] column_tblInvoiceD = { 40, 60 };
                tblInvoiceD.SetWidths(column_tblInvoiceD);

                tblInvoiceD.AddCell(pdf.setCell("WHEEL NUMBER :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].WheelNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceD.AddCell(pdf.setCell("LOCATION:", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].LocationName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceD.AddCell(pdf.setCell("DELIVERY NOTE NO : ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceD.AddCell(pdf.setCell(lstQuot[0].DeliveryNoteNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                PdfPTable tblInvoiceTo = new PdfPTable(4);
                int[] column_tblConsigneeD = { 30, 30, 20, 20 };
                tblInvoiceTo.SetWidths(column_tblConsigneeD);

                tblInvoiceTo.AddCell(pdf.setCell("COLLECTED FROM :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf10, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell(lstQuot[0].CollectedFrom, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf10, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell("CUSTOMER :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf10, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf10, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell("DATE IN :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf10, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell(lstQuot[0].DateIn.ToString("dd/MM/yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
                tblInvoiceTo.AddCell(pdf.setCell("DATE RETURN : " , pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                tblInvoiceTo.AddCell(pdf.setCell(lstQuot[0].DateReturn.ToString("dd/MM/yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));

                PdfPTable tblInvoiceDET = new PdfPTable(8);
                int[] column_tblConsigneeDet = { 10, 15, 10, 15, 10, 10, 10, 20 };
                tblInvoiceDET.SetWidths(column_tblConsigneeDet);

                tblInvoiceDET.AddCell(pdf.setCell("TYRE " , pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].Tyre, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell("SENSOR ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].Sensor, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell("REMARK ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].Remarks, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell("CAP ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].Cap, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell("SENSOR VALUE ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].SensorValue, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblInvoiceDET.AddCell(pdf.setCell("PART NUMBER : ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].PartNumber, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                tblInvoiceDET.AddCell(pdf.setCell("WHEEL MAKE :  ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                tblInvoiceDET.AddCell(pdf.setCell(lstQuot[0].WheelMake, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));

                //if (flagPrintHeader == "yes" || flagPrintHeader == "y")
                //    tblMember.AddCell(pdf.setCell(tblSteel, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell("JOB CARD", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 5));
                tblMember.AddCell(pdf.setCell("No." + lstQuot[0].JobCardNo, pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 9));
                tblMember.AddCell(pdf.setCell(tblInvoiceTo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 5));
                tblMember.AddCell(pdf.setCell(tblInvoiceD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblInvoiceDET, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));


                //tblMember.AddCell(pdf.setCell(tblSteel, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                //tblMember.AddCell(pdf.setCell("JOB CARD", pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 5));
                //tblMember.AddCell(pdf.setCell("No.", pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 9));
                //tblMember.AddCell(pdf.setCell(tblInvoiceTo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                //tblMember.AddCell(pdf.setCell(tblInvoiceD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                //var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));

                int[] column_tblNested = { 20, 20, 20, 20, 20 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("STRAIGHTENED" + "\n" + "MEASUREMENT", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("A", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("B", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("C", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell("D", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));

                PdfPTable tblDetail1 = new PdfPTable(4);
                int[] column_tblDetail1 = { 15, 35, 15, 35 };
                tblDetail1.SetWidths(column_tblDetail1);

                //tblDetail1.AddCell(pdf.setCell("SR." + "\n" + "No", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail1.AddCell(pdf.setCell("Particulars", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail1.AddCell(pdf.setCell("Quantity", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));

                //for (int i = 0; i < dtItem.Rows.Count; i++)
                //{
                //    tblDetail1.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                //    tblDetail1.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                //    tblDetail1.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                //}

                tblDetail1.AddCell(pdf.setCell("CLAIM NO :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].ClaimNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                tblDetail1.AddCell(pdf.setCell("REG NO :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].RegNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell("CHASSIS NO :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].ChassisNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                tblDetail1.AddCell(pdf.setCell("PAINT CODE :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].PaintCode, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell("COMMENT : ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].Comment, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                tblDetail1.AddCell(pdf.setCell("DIAMOND CUT/ PAINTED :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].DiamondCut, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell("Buyer's Ref :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].BuyerRef, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                tblDetail1.AddCell(pdf.setCell("START DATE :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].StartDate.ToString("dd/MM/yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                tblDetail1.AddCell(pdf.setCell("QUALITY CHECK :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].QualityCheck, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                //tblDetail1.AddCell(pdf.setCell("ESTIMATE PRICE :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                //tblDetail1.AddCell(pdf.setCell(lstQuot[0].EstimatePrice, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
                //tblDetail1.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
                tblDetail1.AddCell(pdf.setCell("DELIVERY DATE :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
                tblDetail1.AddCell(pdf.setCell(lstQuot[0].DeliveryDate.ToString("dd/MM/yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));

                tblDetail1.AddCell(pdf.setCell("SR." + "\n" + "No", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail1.AddCell(pdf.setCell("Particulars", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail1.AddCell(pdf.setCell("Quantity", pdf.LightBlueBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    tblDetail1.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    tblDetail1.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
                    tblDetail1.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                }

                tblDetail.AddCell(pdf.setCell(tblDetail1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition

                PdfPTable tblTNC = new PdfPTable(2);
                int[] column_tblTNC = { 50, 50 };
                tblTNC.SetWidths(column_tblTNC);
                //   tblTNC.AddCell(pdf.setCell("Terms and Conditions :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell("I have read,understood and agree to all the terms and conditions.", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell("Date:______________________________", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell("Customer's Signature____________________", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTNC.AddCell(pdf.setCell("Workshop Incharge________________________", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf6, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                int[] column_tblFooter = { 70, 30 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                // tblFooter.AddCell(pdf.setCell(tblBD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf6, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblFooter.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                // tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(lstOrg[0].OrgName, 3), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 0f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblSignOff.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf6, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 1));
                //tblSignOff.AddCell(pdf.setCell("SUBJECT TO VADODARA JURISDICTION", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                #endregion
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].pkID.ToString() + ".pdf";
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
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.JobCard> lstEntity = new List<Entity.JobCard>();
                // ----------------------------------------------------
                lstEntity = BAL.JobCardMgmt.GetJobCardList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), "", Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtJobCardNo.Text = lstEntity[0].JobCardNo;
                txtDate.Text = !String.IsNullOrEmpty(lstEntity[0].Date.ToString()) ? Convert.ToDateTime(lstEntity[0].Date).ToString("yyyy-MM-dd") : SqlDateTime.MinValue.Value.ToString();
                txtCollectedFrom.Text = lstEntity[0].CollectedFrom;
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                txtDateIn.Text = lstEntity[0].DateIn.ToString("yyyy-MM-dd");
                txtDateReturn.Text = lstEntity[0].DateReturn.ToString("yyyy-MM-dd");
                txtWheelNo.Text = lstEntity[0].WheelNo;
                drpInvoiceNo.SelectedValue = lstEntity[0].InvoiceNo;
                txtDeliveryNoteNo.Text = lstEntity[0].DeliveryNoteNo;
                drpTyre.SelectedValue = lstEntity[0].Tyre;
                drpCap.SelectedValue = lstEntity[0].Cap;
                drpSensor.SelectedValue = lstEntity[0].Sensor;
                drpSensorValue.SelectedValue = lstEntity[0].SensorValue;
                txtRemarks.Text = lstEntity[0].Remarks;
                txtPartNumber.Text = lstEntity[0].PartNumber;
                txtWheelMake.Text = lstEntity[0].WheelMake;
                txtStraightenedMeasurement.Text = lstEntity[0].StraightenedMeasurement;
                txtClaimNo.Text = lstEntity[0].ClaimNo;
                txtRegNo.Text = lstEntity[0].RegNo;
                txtChassisNo.Text = lstEntity[0].ChassisNo;
                txtPaintCode.Text = lstEntity[0].PaintCode;
                txtComment.Text = lstEntity[0].Comment;
                drpDiamondCut.SelectedValue = lstEntity[0].DiamondCut;
                txtQualityCheck.Text = lstEntity[0].QualityCheck;
                txtEstimatePrice.Text = lstEntity[0].EstimatePrice;
                txtStartDate.Text = lstEntity[0].StartDate.ToString("yyyy-MM-dd");
                txtDeliveryDate.Text = lstEntity[0].DeliveryDate.ToString("yyyy-MM-dd");
                txtBuyerRef.Text = lstEntity[0].BuyerRef;
                drpLocation.SelectedValue = lstEntity[0].Location
;                // -------------------------------------------------------------------------
                BindJobCardDetailList(lstEntity[0].JobCardNo);
                txtCustomerName.Enabled = (pMode.ToLower() == "add") ? true : false;
                txtCustomerName.Focus();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            divErrorMessage.InnerHtml = "";
            string strErr = "";

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            int n;
            if (String.IsNullOrEmpty(txtClaimNo.Text) || (!String.IsNullOrEmpty(txtWheelNo.Text) && int.TryParse(txtWheelNo.Text, out n) == false) )
            {
                _pageValid = false;

                divErrorMessage.Style.Remove("color");
                divErrorMessage.Style.Add("color", "red");

                if (String.IsNullOrEmpty(txtClaimNo.Text))
                    strErr += "<li>" + "ClaimNo is required. " + "</li>";

                if(int.TryParse(txtWheelNo.Text, out n) == false)
                    strErr += "<li>" + "Enter only numeric value in Wheel No. " + "</li>";
                //divErrorMessage.Controls.Add(new LiteralControl("<li>" + "ClaimNo is required." + "</li>"));
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "JobCard Date is Not Valid." + "</li>";
                }
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                int ReturnCode = 0, ReturnCode1 = 0, ReturnCode2 = 0;
                string ReturnMsg = "", ReturnMsg1 = "", ReturnMsg2 = "", ReturnJobCardNo = "";

                Entity.JobCard objEntity = new Entity.JobCard();
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.JobCardNo = txtJobCardNo.Text;
                        objEntity.Date = Convert.ToDateTime(txtDate.Text);
                        objEntity.CollectedFrom = txtCollectedFrom.Text;
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.DateIn = Convert.ToDateTime(txtDateIn.Text);
                        objEntity.DateReturn = Convert.ToDateTime(txtDateReturn.Text);
                        objEntity.WheelNo = txtWheelNo.Text;
                        objEntity.InvoiceNo = drpInvoiceNo.SelectedValue;
                        objEntity.DeliveryNoteNo = txtDeliveryNoteNo.Text;
                        objEntity.Tyre = drpTyre.SelectedValue;
                        objEntity.Cap = drpCap.SelectedValue;
                        objEntity.Sensor = drpSensor.SelectedValue;
                        objEntity.SensorValue = drpSensorValue.SelectedValue;
                        objEntity.Remarks = txtRemarks.Text;
                        objEntity.PartNumber = txtPartNumber.Text;
                        objEntity.WheelMake = txtWheelMake.Text;
                        objEntity.StraightenedMeasurement = txtStraightenedMeasurement.Text;

                        objEntity.ClaimNo = txtClaimNo.Text;

                        objEntity.RegNo = txtRegNo.Text;

                        objEntity.ChassisNo = txtChassisNo.Text;
                        objEntity.PaintCode = txtPaintCode.Text;
                        objEntity.Comment = txtComment.Text;
                        objEntity.DiamondCut = drpDiamondCut.SelectedValue;
                        objEntity.StartDate = Convert.ToDateTime(txtStartDate.Text);

                        objEntity.QualityCheck = txtQualityCheck.Text;
                        objEntity.EstimatePrice = txtEstimatePrice.Text;
                        objEntity.DeliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                        objEntity.BuyerRef = txtBuyerRef.Text;
                        objEntity.Location = drpLocation.SelectedValue;

                        objEntity.LoginUserID = Session["LoginUserID"].ToString();

                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.JobCardMgmt.AddUpdateJobCard(objEntity, out ReturnCode, out ReturnMsg, out ReturnJobCardNo);
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnMsg : ReturnMsg) + "</li>";

                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnJobCardNo))
                        {
                            txtJobCardNo.Text = ReturnJobCardNo;
                            Entity.JobCardDetail objQuotDet = new Entity.JobCardDetail();

                            strErr += "<li>" + ((ReturnCode > 0) ? ReturnJobCardNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                            BAL.JobCardMgmt.DeleteJobCardDetailByJobCardNo(ReturnJobCardNo, out ReturnCode1, out ReturnMsg1);

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.JobCardNo = ReturnJobCardNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.UnitRate = Convert.ToDecimal(dr["UnitRate"]);
                                objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.JobCardMgmt.AddUpdateJobCardDetail(objQuotDet, out ReturnCode2, out ReturnMsg2);
                                strErr += "<li>" + ((ReturnCode > 0) ? ReturnMsg : ReturnMsg) + "</li>";
                                if (ReturnCode > 0)
                                {
                                    Session.Remove("dtDetail");
                                    
                                    btnSave.Disabled = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        strErr = "<li>" + "Atleast One Item is required to save JobCard Information !" + "</li>";
                    }
                }
                else
                {
                    strErr = "<li>" + "Atleast One Item is required to save JobCard Information !" + "</li>";
                }

                // --------------------------------------------------------------
                //divErrorMessage.InnerHtml = ReturnMsg;
                if (!String.IsNullOrEmpty(strErr))
                {
                    if (ReturnCode > 0)
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                }
            }
            else
            { 
                if (!String.IsNullOrEmpty(strErr))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        } 
        public void ClearAllField()
        {
            Session.Remove("dtDetail");

            hdnpkID.Value = "";
            txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtJobCardNo.Text = "";
            txtCollectedFrom.Text = "";
            txtCustomerName.Text = "";
            txtDateIn.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDateReturn.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtWheelNo.Text = "";
            drpInvoiceNo.SelectedValue = "";
            txtDeliveryNoteNo.Text = "";
            drpTyre.SelectedValue = "";
            drpCap.SelectedValue = "";
            drpSensor.SelectedValue = "";
            drpSensorValue.SelectedValue = "";
            txtRemarks.Text = "";
            txtPartNumber.Text = "";
            txtWheelMake.Text = "";
            txtStraightenedMeasurement.Text = "";
            txtClaimNo.Text = "";
            txtRegNo.Text = "";
            txtChassisNo.Text = "";
            txtPaintCode.Text = "";
            txtComment.Text = "";
            drpDiamondCut.SelectedValue = "";
            txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtQualityCheck.Text = "";
            txtEstimatePrice.Text = "";
            txtDeliveryDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtStartDate.Focus();
            txtBuyerRef.Text = "";
            drpLocation.SelectedValue = "";
            btnReset.Disabled = false;
            btnSave.Disabled = false;
            BindJobCardDetailList("");

            txtCustomerName.Enabled = true;
        }

        [System.Web.Services.WebMethod]
        public static string DeleteJobCard(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.JobCardMgmt.DeleteJobCard(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
            TextBox edAmount = (TextBox)item.FindControl("edAmount");
            // --------------------------------------------------------------------------

            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            Decimal ur = (!String.IsNullOrEmpty(edUnitRate.Text)) ? Convert.ToDecimal(edUnitRate.Text) : 0;
            Decimal a = (!String.IsNullOrEmpty(edAmount.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;

            edAmount.Text = (Convert.ToDecimal(edQuantity.Text) * Convert.ToDecimal(edUnitRate.Text)).ToString();
            // --------------------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            
            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductID"].ToString() == edProductID.Value)
                {
                   
                    row.SetField("Quantity", edQuantity.Text);
                    row.SetField("UnitRate", edUnitRate.Text);
                    row.SetField("Amount", edAmount.Text);
                }
                
            }
            dtDetail.AcceptChanges();
            rptJobCardDetail.DataSource = dtDetail;
            rptJobCardDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
        }

        public void BindJobCardDetailList(string JobCardNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.JobCardMgmt.GetJobCardDetail(JobCardNo);
            rptJobCardDetail.DataSource = dtDetail1;
            rptJobCardDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void rptJobCardDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.CommandName.ToString() == "Save")
            {
                _pageValid = true;
                if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0"
                    || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                        strErr += "<li>" + "Product Selection is required." + "</li>";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                        strErr += "<li>" + "Quantity is required." + "</li>";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                        strErr += "<li>" + "Unit Rate is required." + "</li>";

                }
                // -------------------------------------------------------------
                if (_pageValid)
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

                    if (dtDetail != null)
                    {

                        foreach (System.Data.DataColumn col in dtDetail.Columns) col.AllowDBNull = true;

                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + ((HiddenField)e.Item.FindControl("hdnProductID")).Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "<li>'Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                            return;
                        }

                        DataRow dr = dtDetail.NewRow();

                        dr["pkID"] = 0;
                        
                        string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                        string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                        string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                        string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                        string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                        string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;

                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                        dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["Amount"] = (!String.IsNullOrEmpty(amt)) ? Convert.ToDecimal(amt) : 0;
                        
                        dtDetail.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptJobCardDetail.DataSource = dtDetail;
                        rptJobCardDetail.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtDetail", dtDetail);
                    }
                }
                // -------------------------------------------------

                //txtHeadDiscount_TextChanged(null, null);

                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                // --------------------------------- Delete Record
                string iname = ((HiddenField)e.Item.FindControl("edProductName")).Value;

                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (dr["ProductName"].ToString() == iname)
                    {
                        dtDetail.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }

                rptJobCardDetail.DataSource = dtDetail;
                rptJobCardDetail.DataBind();

                Session.Add("dtDetail", dtDetail);
                // -------------------------------------------------
                if (!String.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + "<li>Item Deleted Successfully !</li>" + "');", true);
            }
        }

        protected void rptJobCardDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;

            Control rptFootCtrl = rptJobCardDetail.Controls[rptJobCardDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
                TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
                TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
                TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));

                List<Entity.Products> lstEntity = new List<Entity.Products>();

                if (!String.IsNullOrEmpty(hdnProductID.Value))
                    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (lstEntity.Count > 0)
                {
                    if (lstEntity.Count > 0)
                        txtUnit.Text = lstEntity[0].Unit;

                    decimal plUnitPrice = 0, plDiscount = 0;
                    hdnCustomerID.Value = (String.IsNullOrEmpty(hdnCustomerID.Value)) ? "0" : hdnCustomerID.Value;
                    hdnProductID.Value = (String.IsNullOrEmpty(hdnProductID.Value)) ? "0" : hdnProductID.Value;
                    BAL.CommonMgmt.GetProductPriceListRate(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), out plUnitPrice, out plDiscount);

                    txtUnitRate.Text = plUnitPrice.ToString();
                    
                }
            }
            else
            {
                strErr += "<li> Select Proper Item From List !</li>";
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
            TextBox txtUnit1 = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            txtUnit1.Focus();

        }

        protected void txtUnit_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptJobCardDetail.Controls[rptJobCardDetail.Controls.Count - 1].Controls[0];

            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtAmount = ((TextBox)rptFootCtrl.FindControl("txtAmount"));

            txtAmount.Text = (Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtUnitRate.Text)).ToString();
        }

        protected void txtUnitRate_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptJobCardDetail.Controls[rptJobCardDetail.Controls.Count - 1].Controls[0];

            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtAmount = ((TextBox)rptFootCtrl.FindControl("txtAmount"));

            txtAmount.Text = (Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtUnitRate.Text)).ToString();
        }
    }
}