using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace StarsProject
{
    public partial class MaterialIndent : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session.Remove("mySpecs");

                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

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
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }

        public void OnlyViewControls()
        {
            txtIndentNo.ReadOnly = true;
            txtIndentDate.ReadOnly = true;
            txtRemarksExt.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtIndentNo.Text = ""; //  BAL.CommonMgmt.fnGetOutwardNoByDate(DateTime.Today.ToString("yyyy-MM-dd"));
            txtIndentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            BindIndentDetailList("");
            btnSave.Disabled = false;
            txtIndentDate.Focus();
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.MaterialIndent> lstEntity = new List<Entity.MaterialIndent>();
                // ----------------------------------------------------
                lstEntity = BAL.MaterialIndentMgmt.GetMaterialIndent(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtIndentNo.Text = lstEntity[0].IndentNo;
                txtIndentDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].IndentDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                txtRemarksExt.Text = lstEntity[0].Remarks;
                //txtIndentDate.Text = 

                BindIndentDetailList(lstEntity[0].IndentNo);
                txtIndentDate.Focus();
            }
        }

        public void BindIndentDetailList(string pIndentNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.MaterialIndentMgmt.GetMaterialIndentDetail(pIndentNo);
            rptOutwardDetail.DataSource = dtDetail1;
            rptOutwardDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }
        protected void rptOutwardDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;


                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) ||
                        ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0" || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtExpectedDate")).Text))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                            strErr += "<li>" + "Select Proper Product From List." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                            strErr += "<li>" + "Quantity is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtExpectedDate")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == " ")
                            strErr += "<li>" + "Expected Date is required." + "</li>";

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
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                                return;
                            }

                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = 0;
                            //string icode = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedValue;
                            //string iname = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedItem.Text;
                            string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                            string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                            string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                            string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                            string expdate = ((TextBox)e.Item.FindControl("txtExpectedDate")).Text;
                            string remark = ((TextBox)e.Item.FindControl("txtRemarksInt")).Text;

                            //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            //DataTable mySpecs = new DataTable();
                            //List<Entity.Products> ProdSpec = new List<Entity.Products>();

                            //ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList("", Convert.ToInt64(icode), Session["LoginUserID"].ToString());
                            //if (Session["mySpecs"] != null)
                            //{
                            //    mySpecs = (DataTable)Session["mySpecs"];

                            //    DataRow drTemp = mySpecs.NewRow();
                            //    drTemp["pkID"] = Convert.ToInt64(icode);
                            //    drTemp["ProductSpecification"] = (ProdSpec.Count > 0) ? ProdSpec[0].ProductSpecification : "";
                            //    mySpecs.Rows.Add(drTemp);
                            //}
                            //else
                            //    mySpecs = PageBase.ConvertListToDataTable(ProdSpec);


                            //mySpecs.AcceptChanges();
                            //Session.Add("mySpecs", mySpecs);
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

                            //dr["OutwardNo"] = txtOutwardNo.Text;
                            //dr["BundleID"] = 0;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dr["ExpectedDate"] = (!String.IsNullOrEmpty(expdate)) ? expdate : "";
                            dr["Remarks"] = (!String.IsNullOrEmpty(remark)) ? remark : "";

                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptOutwardDetail.DataSource = dtDetail;
                            rptOutwardDetail.DataBind();
                            //----------------------------------------------------------------
                            Session.Add("dtDetail", dtDetail);

                        }
                    }
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
            if (e.CommandName.ToString() == "Delete")
            {
                int ReturnCode = 0;
                string ReturnMsg = "";
                Int64 tmpRow;

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

                rptOutwardDetail.DataSource = dtDetail;
                rptOutwardDetail.DataBind();

                Session.Add("dtDetail", dtDetail);
            }
        }

        protected void rptOutwardDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            //----------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnIndentNo = "";
            string strErr = "";
            _pageValid = true;


            if ( String.IsNullOrEmpty(txtIndentDate.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtIndentDate.Text))
                    strErr += "<li>" + "Indent Date is required." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 )
                    {
                        Entity.MaterialIndent objEntity = new Entity.MaterialIndent();

                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                        objEntity.IndentNo = txtIndentNo.Text;
                        objEntity.IndentDate = Convert.ToDateTime(txtIndentDate.Text);
                        objEntity.Remarks = Convert.ToString(txtRemarksExt.Text);
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.MaterialIndentMgmt.AddUpdateMaterialIndent(objEntity, out ReturnCode, out ReturnMsg,out ReturnIndentNo);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        if (!String.IsNullOrEmpty(ReturnIndentNo))
                        {
                            txtIndentNo.Text = ReturnIndentNo;
                        }
                        // --------------------------------------------------------------

                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table

                        BAL.MaterialIndentMgmt.DeleteMaterialIndentDetailByIndentNo(ReturnIndentNo, out ReturnCode, out ReturnMsg);
                        if (ReturnCode > 0)
                        {
                            Entity.MaterialIndent_detail objQuotDet = new Entity.MaterialIndent_detail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.IndentNo = ReturnIndentNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Qty = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"]);
                                objQuotDet.Remarks = dr["Remarks"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.MaterialIndentMgmt.AddUpdateMaterialIndentDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            // -------------------------------------------------------
                            if (ReturnCode > 0)
                            {
                                Session.Remove("dtDetail");
                                Session.Remove("mySpecs");
                            }

                            btnSave.Disabled = true;
                        }
                    }
                    else
                    {
                        strErr = "<li>" + "Atleast One Item is required to save Outward Information !" + "</li>";
                    }
                }
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
        }

        [System.Web.Services.WebMethod]
        public static string DeleteMaterialIndent(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.MaterialIndentMgmt.DeleteMaterialIndent(pkID, out ReturnCode, out ReturnMsg);
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
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");
            TextBox edExpectedDate = (TextBox)item.FindControl("edExpectedDate");
            TextBox edRemarks = (TextBox)item.FindControl("edRemarks");
            // --------------------------------------------------------------------------
            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;
            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductName"].ToString() == edProductName.Value)
                {
                    row.SetField("Quantity", q);
                    row.SetField("Unit", edUnit.Text);
                    row.SetField("ExpectedDate", edExpectedDate.Text);
                    row.SetField("Remarks", edRemarks.Text);
                    //row.SetField("BoxNo", edBoxNo.Text);
                    row.AcceptChanges();
                }
            }
            rptOutwardDetail.DataSource = dtDetail;
            rptOutwardDetail.DataBind();

            Session.Add("dtDetail", dtDetail);
        }

        protected void editItem_TextChanged1()
        {
            Control rptFootCtrl = rptOutwardDetail.Controls[rptOutwardDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            TextBox txtQuantity = (TextBox)rptFootCtrl.FindControl("txtQuantity");
            TextBox txtUnit = (TextBox)rptFootCtrl.FindControl("txtUnit");
            TextBox txtExpectedDate = (TextBox)rptFootCtrl.FindControl("txtExpectedDate");
            TextBox txtRemarksInt = (TextBox)rptFootCtrl.FindControl("txtRemarksInt");
            //TextBox txtAmount = (TextBox)rptFootCtrl.FindControl("txtAmount");
            //TextBox txtTaxRate = (TextBox)rptFootCtrl.FindControl("txtTaxRate");
            //TextBox txtTaxAmount = (TextBox)rptFootCtrl.FindControl("txtTaxAmount");
            //TextBox txtNetAmount = (TextBox)rptFootCtrl.FindControl("txtNetAmount");
            //// --------------------------------------------------------------------------
            //Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            //Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);
            //Decimal dp = String.IsNullOrEmpty(txtExpectedDate.Text) ? 0 : Convert.ToDecimal(txtExpectedDate.Text);
            //Decimal nr = String.IsNullOrEmpty(txtNetRate.Text) ? 0 : Convert.ToDecimal(txtNetRate.Text);
            //Decimal a = String.IsNullOrEmpty(txtAmount.Text) ? 0 : Convert.ToDecimal(txtAmount.Text);
            //Decimal tr = String.IsNullOrEmpty(txtTaxRate.Text) ? 0 : Convert.ToDecimal(txtTaxRate.Text);
            //Decimal ta = String.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
            //Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            //// --------------------------------------------------------------------------
            //nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            //a = Math.Round((q * nr), 2);
            //ta = Math.Round(((a * tr) / 100), 2);
            //na = Math.Round((a + ta), 2);

            //txtNetRate.Text = nr.ToString();
            //txtAmount.Text = a.ToString();
            //txtTaxAmount.Text = ta.ToString();
            //txtNetAmount.Text = na.ToString();
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptOutwardDetail.Controls[rptOutwardDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));

            HiddenField hdnUnitQuantity = ((HiddenField)rptFootCtrl.FindControl("hdnUnitQuantity"));

            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtExpectedDate = ((TextBox)rptFootCtrl.FindControl("txtExpectedDate"));
            TextBox txtRemarksInt = ((TextBox)rptFootCtrl.FindControl("txtRemarksInt"));
            List<Entity.Products> lstEntity = new List<Entity.Products>();

            if (!String.IsNullOrEmpty(hdnProductID.Value))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            txtQuantity.Focus();
        }


        //----------------------pdf Generating----------------------------//
        [WebMethod]
        public static string GetIndentNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetIndentNo(pkID);
            return tempVal;
        }
        [WebMethod(EnableSession = true)]
        public static void GenerateIndent(Int64 pkID)
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

            if (tmpSerialKey == "SI08-SB94-MY45-RY15")          // Sharvaya Infotech
            {
                GenerateIndent_Sharvaya(pkID);
            }
            else
            {
                GenerateIndent_Sharvaya(pkID);
            }

        }
        public static void GenerateIndent_Sharvaya(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(6);
            //PdfPTable tblCylDetail = new PdfPTable(4);

            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(2);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // ===========================================================================================
            float paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf10 = 10;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            Int64 ProdDetail_Lines = 0;

            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");

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
            // =========================================================================
            // PDF Document Object Instance Creation
            // =========================================================================
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
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
            List<Entity.MaterialIndent> lstQuot = new List<Entity.MaterialIndent>();
            lstQuot = BAL.MaterialIndentMgmt.GetMaterialIndent(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.MaterialIndentMgmt.GetMaterialIndentDetail(lstQuot[0].IndentNo);

            // -------------------------------------------------------------------------------------------------------------
            //int totrec = 0;
            //List<Entity.Customer> lstCust = new List<Entity.Customer>();
            //if (lstQuot.Count > 0)
            //    lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            //// ------------------------------------------------------------------------------
            //List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            //lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
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

                //PdfPTable tblCustomerD = new PdfPTable(1);
                //int[] column_tblNested20 = { 100 };
                //tblCustomerD.SetWidths(column_tblNested20);

                //tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //tblCustomerD.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                PdfPTable tblIndentD = new PdfPTable(2);
                int[] column_tblIndentD = { 25,75 };
                tblIndentD.SetWidths(column_tblIndentD);

                tblIndentD.AddCell(pdf.setCell("Doc. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblIndentD.AddCell(pdf.setCell(": " + lstQuot[0].IndentNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblIndentD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblIndentD.AddCell(pdf.setCell(": " + lstQuot[0].IndentDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblIndentD.AddCell(pdf.setCell("Remarks (External)", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblIndentD.AddCell(pdf.setCell(": " + lstQuot[0].Remarks, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                
                tblMember.AddCell(pdf.setCell("Indent Details", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf8, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblIndentD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 10, 36, 10, 10,17,17 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Product Name", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Expected Date", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Remarks", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                decimal totAmount = 0, taxAmount = 0, netAmount = 0, quantity = 0; ;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    //totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    //taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    //netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    decimal q = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    quantity += Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());

                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ExpectedDate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Remarks"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));

                    // ----------------------------------------
                    //List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    //lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].IndentNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    //if (lstSpec.Count > 0)
                    //{
                    //    string tmpSpec = dtItem.Rows[i]["ProductSpecification"].ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    //        totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                    //    }
                    //}
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));

                    }
                }
                tblDetail.AddCell(pdf.setCell("Total", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(quantity.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].IndentNo.Replace("/", "-").ToString() + ".pdf";
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
                tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                pdfDoc.Add(tblHeader);

                // >>>>>> Adding Quotation Header
                tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                pdfDoc.Add(tblSubject);

                // >>>>>> Adding Quotation Master Information Table
                tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfDoc.Add(tblMember);

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

                #endregion

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
                // ------------------------------------------------
                RecompressPDF(sPath + smallFileName, pdfFileName);
            }
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