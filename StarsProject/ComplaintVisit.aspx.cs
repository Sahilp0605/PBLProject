using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace StarsProject
{
    public partial class ComplaintVisit : System.Web.UI.Page
    {

        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                }
                // --------------------------------------------------------    
                if (!String.IsNullOrEmpty(Request.QueryString["complaintno"]))
                {
                    hdnComplaintNo.Value = Request.QueryString["complaintno"].ToString();
                    List<Entity.Complaint> lstComplaint = new List<Entity.Complaint>();
                    lstComplaint = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnComplaintNo.Value), 0, "", Session["LoginUserID"].ToString());
                    if (lstComplaint.Count > 0)
                    {
                        hdnCustomerID.Value = lstComplaint[0].CustomerID.ToString();
                        txtCustomerName.Text = lstComplaint[0].CustomerName.ToString();
                        // --------------------------------------------------------
                        BindComplaintByCustomer();
                    }
                }
                // --------------------------------------------------------
                if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                {
                    ClearAllField();
                }
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
            else
            {
                // ----------------------------------------------------------------------
                // Visitor Document Upload On .... Page Postback
                // ----------------------------------------------------------------------

                if (uploadDocument.PostedFile != null)
                {
                    if (uploadDocument.HasFile)
                    {
                        string filePath = uploadDocument.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                        {
                            string rootFolderPathDocument = Server.MapPath("visitdocuments");
                            string filesToDeleteDocument = @"visit-document-" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileListDocument = System.IO.Directory.GetFiles(rootFolderPathDocument, filesToDeleteDocument);
                            foreach (string filedocument in fileListDocument)
                            {
                                System.IO.File.Delete(filedocument);
                            }
                            // -----------------------------------------------------
                            String flnamedocument = "visit-document-" + hdnpkID.Value.Trim() + ext;
                            uploadDocument.SaveAs(Server.MapPath("visitdocuments/") + flnamedocument);
                            imgDocument.ImageUrl = "";
                            imgDocument.ImageUrl = "visitdocuments/" + flnamedocument;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                    }
                }

                var requestTarget = this.Request["__EVENTTARGET"];

                if (requestTarget.ToLower() == "txtcustomername")
                {
                    if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                        BindComplaintByCustomer();
                }

            }
        }

        public void OnlyViewControls()
        {
            txtCustomerName.ReadOnly = true;
            drpComplaintNo.Attributes.Add("disabled", "disabled");
            drpStatus.Attributes.Add("disabled", "disabled");
            txtVisitDate.ReadOnly = true;
            txtTimeFrom.ReadOnly = true;
            txtTimeTo.ReadOnly = true;
            drpVisitType.Attributes.Add("disabled", "disabled");
            txtVisitNotes.ReadOnly = true;
            drpVisitChargeType.Attributes.Add("disabled", "disabled");
            txtVisitCharge.ReadOnly = true;
            txtProductName.ReadOnly = true;
            txtSrNo.ReadOnly = true;
            txtComplaintNotes.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindComplaintByCustomer()
        {
            int totrec = 0;
            // ---------------- Designation List  -------------------------------------
            List<Entity.Complaint> lstEmployee = new List<Entity.Complaint>();
            lstEmployee = BAL.ComplaintMgmt.GetComplaintList(0, Convert.ToInt64(hdnCustomerID.Value), "", 0, 0, Session["LoginUserID"].ToString(), "", 1, 10000, out totrec);
            drpComplaintNo.DataSource = lstEmployee;
            drpComplaintNo.DataValueField = "pkID";
            drpComplaintNo.DataTextField = "ComplaintNo";
            drpComplaintNo.DataBind();
            drpComplaintNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Complaint # --", "0"));
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                List<Entity.ComplaintVisit> lstEntity = new List<Entity.ComplaintVisit>();
                lstEntity = BAL.ComplaintMgmt.GetComplaintVisitList(Convert.ToInt64(hdnpkID.Value), Convert.ToInt64(hdnComplaintNo.Value), 0, 0, "", "", Session["LoginUserID"].ToString());

                hdnpkID.Value = lstEntity[0].pkID.ToString();
                hdnEmployeeID.Value = lstEntity[0].EmployeeID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                txtCustomerName_TextChanged(null, null);
                drpComplaintNo.SelectedValue = lstEntity[0].ComplaintNo.ToString();
                hdnComplaintNo.Value = lstEntity[0].ComplaintNo.ToString();
                drpVisitType.SelectedValue = lstEntity[0].VisitType.ToString();
                txtVisitNotes.Text = lstEntity[0].VisitNotes.ToString();
                if (!String.IsNullOrEmpty(lstEntity[0].VisitDate.ToString()) && lstEntity[0].VisitDate.Value.Year > 1900)
                    txtVisitDate.Text = lstEntity[0].VisitDate.Value.ToString("yyyy-MM-dd");
                else
                    txtVisitDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTimeFrom.Text = lstEntity[0].TimeFrom.ToString();
                txtTimeTo.Text = lstEntity[0].TimeTo.ToString();
                drpVisitChargeType.SelectedValue = lstEntity[0].VisitChargeType.ToString();
                txtVisitCharge.Text = lstEntity[0].VisitCharge.ToString();
                drpStatus.SelectedValue = lstEntity[0].ComplaintStatus.ToString();
                hdnProductID.Value = lstEntity[0].ProductID.ToString();
                txtProductName.Text = lstEntity[0].ProductName.ToString();
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();
                txtSrNo.Text = lstEntity[0].SrNo.ToString();

                imgDocument.ImageUrl = lstEntity[0].VisitDocument;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0,ReturnpkID = 0;
            string ReturnMsg = "", ReturnComplaintNo = "";
            string strErr = "";

            _pageValid = true;

            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0" ||
                drpComplaintNo.SelectedValue == "0" || String.IsNullOrEmpty(txtVisitNotes.Text) ||
                String.IsNullOrEmpty(txtVisitDate.Text) || String.IsNullOrEmpty(txtTimeFrom.Text) || String.IsNullOrEmpty(txtTimeTo.Text) ||
                (drpVisitType.SelectedValue == "Charged" && (String.IsNullOrEmpty(drpVisitChargeType.SelectedValue) || String.IsNullOrEmpty(txtVisitCharge.Text) || txtVisitCharge.Text == "0")))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    strErr += "<li>" + "Customer Selection is required." + "</li>";

                if (drpComplaintNo.SelectedValue == "0")
                    strErr += "<li>" + "Complaint # is Required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitNotes.Text))
                    strErr += "<li>" + "Complaint Visit Notes is required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitDate.Text) || String.IsNullOrEmpty(txtTimeFrom.Text) || String.IsNullOrEmpty(txtTimeTo.Text))
                    strErr += "<li>" + "Visit Date and Period is required." + "</li>";

                if (drpVisitType.SelectedValue == "Charged" && (String.IsNullOrEmpty(drpVisitChargeType.SelectedValue) || String.IsNullOrEmpty(txtVisitCharge.Text) || txtVisitCharge.Text == "0"))
                    strErr += "<li>" + "Charged Type and Amount is required for Charged Visit Type." + "</li>";
            }

            if (!String.IsNullOrEmpty(txtVisitDate.Text))
            {
                DateTime dt2 = DateTime.Now;
                if (Convert.ToDateTime(txtVisitDate.Text) > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Future Visit Date Not Allowed." + "</li>";
                }
            }

            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtVisitDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtVisitDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Visit Date is Not Valid." + "</li>";
                }
            }


            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.ComplaintVisit objEntity = new Entity.ComplaintVisit();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                if (!String.IsNullOrEmpty(drpComplaintNo.SelectedValue))
                    objEntity.ComplaintNo = Convert.ToInt64(drpComplaintNo.SelectedValue);
                objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.VisitDate = Convert.ToDateTime(txtVisitDate.Text);
                objEntity.TimeFrom = txtTimeFrom.Text;
                objEntity.TimeTo = txtTimeTo.Text;
                objEntity.VisitNotes = txtVisitNotes.Text;
                objEntity.VisitType = drpVisitType.Text;
                if (drpVisitType.SelectedValue == "Charged")
                {
                    objEntity.VisitChargeType = drpVisitChargeType.SelectedValue;
                    objEntity.VisitCharge = (!String.IsNullOrEmpty(txtVisitCharge.Text) && txtVisitCharge.Text != "0") ? Convert.ToDecimal(txtVisitCharge.Text) : 0;
                }
                objEntity.ComplaintStatus = drpStatus.SelectedValue;
                objEntity.VisitDocument = imgDocument.ImageUrl;
                objEntity.ComplaintNotes = txtComplaintNotes.Text;
                objEntity.SrNo = txtSrNo.Text;
                objEntity.ProductID = Convert.ToInt64(hdnProductID.Value);

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ComplaintMgmt.AddUpdateComplaintVisit(objEntity, out ReturnCode, out ReturnMsg,out ReturnpkID, out ReturnComplaintNo);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                }
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
            hdnComplaintNo.Value = "";
            hdnParent.Value = "";
            hdnCustomerID.Value = "";
            hdnEmployeeID.Value = "";
            txtCustomerName.Text = "";
            txtVisitDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTimeFrom.Text = DateTime.Now.ToString("hh:mm tt");
            txtTimeTo.Text = DateTime.Now.ToString("hh:mm tt");
            drpVisitType.SelectedValue = "Free";
            txtVisitNotes.Text = "";
            drpVisitChargeType.SelectedValue = "";
            txtVisitCharge.Text = "";
            drpComplaintNo.Items.Clear();
            txtCustomerName.Focus();

            txtComplaintNotes.Text = "";
            txtSrNo.Text = "";
            hdnProductID.Value = "";
            txtProductName.Text = "";
            btnSave.Disabled = false;
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                // -----------------------------------------------------
                List<Entity.Customer> lstEntity = new List<Entity.Customer>();

                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && String.IsNullOrEmpty(txtCustomerName.Text))
                    txtCustomerName.Text = (lstEntity.Count > 0) ? lstEntity[0].CustomerName : "";
                  
                BindComplaintByCustomer();
            }
            else
            {
                txtCustomerName.Focus();
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }


        [System.Web.Services.WebMethod]
        public static string DeleteComplaintVisit(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ComplaintMgmt.DeleteComplaintVisit(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpComplaintNo_TextChanged(object sender, EventArgs e)
        {
            hdnComplaintNo.Value = drpComplaintNo.SelectedValue;
            if (!String.IsNullOrEmpty(hdnComplaintNo.Value))
            {
                // -----------------------------------------------------
                List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();

                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    lstEntity = BAL.CommonMgmt.GetComplaintList(Convert.ToInt64(hdnComplaintNo.Value));

                if (!String.IsNullOrEmpty(hdnComplaintNo.Value) && String.IsNullOrEmpty(drpComplaintNo.SelectedValue))
                    drpComplaintNo.SelectedValue = (lstEntity.Count > 0) ? lstEntity[0].ComplaintNo : "";
                txtSrNo.Text = lstEntity[0].SrNo.ToString();
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();
                hdnProductID.Value = lstEntity[0].ProductID.ToString();
                txtProductName.Text = lstEntity[0].ProductName.ToString();
            }
            else
            {
                drpComplaintNo.Focus();
            }
        }

        //protected void drpVisitType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    dvVisitCharge.Visible = (drpVisitType.SelectedValue == "Free") ? false : true;
        //}

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
            
        [System.Web.Services.WebMethod]
        public static string GetComplaintVisitNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetComplaintVisitNo(pkID);
            return tempVal;
        }

        [WebMethod(EnableSession = true)]

        public static void GenerateComplaintVisit(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // ----------------------------------------------------------------------- 

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
            StarsProject.QuotationEagle.printModule = "Quotation";
            // -------------------------------------------------------
            GenerateComplaintVisit_ParthKitchen(pkID);
        }

        public static void GenerateComplaintVisit_ParthKitchen(Int64 pkID)
        {
            //HttpContext.Current.Session["PrintHeader"] = "no";

            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblExtra = new PdfPTable(4);
            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblDetail = new PdfPTable(8);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;

            // ----------------------------------------Complaint Visit Data---------------------------------------------------------------------
            List<Entity.ComplaintVisit> lstVisitComplaint = new List<Entity.ComplaintVisit>();
            lstVisitComplaint = BAL.ComplaintMgmt.GetComplaintVisitList(pkID, 0, 0, 0, "", "", HttpContext.Current.Session["LoginUserID"].ToString());
            // ----------------------------------------Complaint Visit Detail Data---------------------------------------------------------------------
            DataTable dtVisitDetail = new DataTable();
            if (lstVisitComplaint.Count > 0)
                dtVisitDetail = BAL.VisitAcupanelMgmt.GetComplaintVisitDetail(Convert.ToInt64(lstVisitComplaint[0].ComplaintNo), pkID);
            // ----------------------------------------Complaint Data---------------------------------------------------------------------
            List<Entity.Complaint> lstcomplaint = new List<Entity.Complaint>();
            if (lstVisitComplaint.Count > 0)
                lstcomplaint = BAL.ComplaintMgmt.GetComplaintList(lstVisitComplaint[0].ComplaintNo, 0, "", HttpContext.Current.Session["LoginUserID"].ToString());
            // ------------------------------------------------------------------------------
            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");

            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstVisitComplaint[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);


            List<Entity.ComplaintVisit> lstEntity = new List<Entity.ComplaintVisit>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintVisitList(pkID, lstVisitComplaint[0].ComplaintNo, 0, 0, "", "", HttpContext.Current.Session["LoginUserID"].ToString());
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

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
            // Printing Heading
            // ===========================================================================================

            #region Section >>>>  Master Information

            int[] column_tblMember = { 100 };
            tblMember.SetWidths(column_tblMember);
            tblMember.SpacingBefore = 0f;
            tblMember.SpacingAfter = 0f;
            tblMember.LockedWidth = true;
            // -------------------------------------------------------------------------------------
            //  Defining : Quotation Master Information
            // -------------------------------------------------------------------------------------
            //#region Section >>>> Quotation Master Information



            #region Section >>>> Header Information

            //----------------------------------------------------------
            //----------- Complaint Table Detail

            string address = (!String.IsNullOrEmpty(lstCust[0].Address) ? lstCust[0].Address : "") +
                                (!String.IsNullOrEmpty(lstCust[0].Area) ? " " + lstCust[0].Area : "") + 
                                (!String.IsNullOrEmpty(lstCust[0].CityName) ? " " + lstCust[0].CityName : "") +
                                (!String.IsNullOrEmpty(lstCust[0].Pincode) ? " - " + lstCust[0].Pincode : "") +
                                (!String.IsNullOrEmpty(lstCust[0].StateName) ? ", " + lstCust[0].StateName : "") +
                                (!String.IsNullOrEmpty(lstCust[0].CountryName) ? ", " + lstCust[0].CountryName : "");

            PdfPTable tblDetailVisit = new PdfPTable(4);
            int[] column_tblComplaint = { 20, 40, 20, 20 };
            tblDetailVisit.SetWidths(column_tblComplaint);

            tblDetailVisit.AddCell(pdf.setCell("Request No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].ComplaintNo, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Service Date", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + Convert.ToDateTime(lstcomplaint[0].ComplaintDate).ToString("dd-MM-yyyy").Replace("-","/"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


            tblDetailVisit.AddCell(pdf.setCell("Customer Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Solution Date ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + Convert.ToDateTime(lstVisitComplaint[0].VisitDate).ToString("dd-MM-yyyy").Replace("-", "/"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Address ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + address, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
           
            tblDetailVisit.AddCell(pdf.setCell("Phone No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Serial No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].SrNo, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


            tblDetailVisit.AddCell(pdf.setCell("Product Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].ProductName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

             tblDetailVisit.AddCell(pdf.setCell("Problem", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].ComplaintNotes, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Spares Charges", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].VisitCharge, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Service Eng.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].CreatedByEmployee, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Type of Service", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].VisitType, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblDetailVisit.AddCell(pdf.setCell("Customer Remarks.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblDetailVisit.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].VisitNotes, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            var empty = new Phrase();
            empty.Add(new Chunk(" ", pdf.fnCalibriBold10));


            PdfPTable tblESignature = new PdfPTable(1);
            int[] column_tblESignature = { 100 };
            tblESignature.SetWidths(column_tblESignature);
            //string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/EmployeeImages") + "\\esign-" + lstVisitComplaint[0].EmployeeID;
            iTextSharp.text.Image myesign = pdf.findProductImage(tmpFile);

            // -------------------------------------------------------
            if (myesign != null)
            {
                PdfPTable tblSign = new PdfPTable(1);
                iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(myesign);
                eSign.ScaleAbsolute(90, 70);
                tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            }
            else
            {
                 tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }

            PdfPTable tblsign = new PdfPTable(4);
            int[] column_tblsign = { 38, 30, 2, 30 };
            tblsign.SetWidths(column_tblsign);

            tblsign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblsign.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
            tblsign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblsign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));

            tblsign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
            tblsign.AddCell(pdf.setCell("Engineer Sign" , pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));
            tblsign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
            tblsign.AddCell(pdf.setCell("Customer Sign", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));

            tblMember.AddCell(pdf.setCell(tblDetailVisit, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
            tblMember.AddCell(pdf.setCell(tblsign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
            
            #endregion


            #endregion
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "Complaint Visit -" + lstcomplaint[0].pkID.ToString() + ".pdf";
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
            tblExtra.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblExtra.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblExtra);


            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Header
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfDoc.Add(tblDetail);

            PdfPTable tblOuterDetail = new PdfPTable(1);
            int[] column_tblNestedOuter = { 100 };
            tblOuterDetail.SetWidths(column_tblNestedOuter);
            tblOuterDetail.SpacingBefore = 0f;
            tblOuterDetail.LockedWidth = true;
            tblOuterDetail.SplitLate = false;
            tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            tblOuterDetail.AddCell(pdf.setCell(tblDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
            tblOuterDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblOuterDetail);

            // >>>>>> Adding Quotation Footer
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            //pdfDoc.Add(tblFooter);

            PdfPTable tblOuterDetail1 = new PdfPTable(1);
            tblOuterDetail1.SetWidths(column_tblNestedOuter);
            tblOuterDetail1.SpacingBefore = 0f;
            tblOuterDetail1.LockedWidth = true;
            tblOuterDetail1.SplitLate = false;
            tblOuterDetail1.HorizontalAlignment = Element.ALIGN_CENTER;
            tblOuterDetail1.AddCell(pdf.setCell(tblFooter, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
            tblOuterDetail1.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblOuterDetail1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblOuterDetail1);

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


    }
}