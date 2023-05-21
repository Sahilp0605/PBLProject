using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Web.Services;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;


namespace StarsProject
{
    public partial class Inspection : System.Web.UI.Page
    {
        public string loginuserid;
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                totAmount = 0;
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetail", dtDetail);
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                BindDropDown();
                BindInspectionDetail(0);
                 // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        if (!String.IsNullOrEmpty(Request.QueryString["CustomerId"]))
                        {
                            hdnCustomerID.Value = (!String.IsNullOrEmpty(Request.QueryString["CustomerId"])) ? Request.QueryString["CustomerId"] : "";
                            txtCustomerName_TextChanged(null, null);
                        }
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
            }
        }

        public void OnlyViewControls()
        {
            txtMovementCode.ReadOnly = true;
            txtInspectionDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpOrder.Attributes.Add("disabled", "disabled");
            drpEmployee.Attributes.Add("disabled", "disabled");

            pnlDetail.Enabled = false;

            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            // ---------------- Assign Employee ------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            loginuserid = Session["LoginUserID"].ToString();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetEmployeeFollowerList(Session["LoginUserID"].ToString());
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- All --", "0"));

            //------------------ Assign Customer -------------------------
            List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
            lstCustomer = BAL.CustomerMgmt.GetCustomerList();
        }
        public void BindInspectionDetail(Int64 pInquiryNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.InspectionMgmt.GetInspectionDetail(pInquiryNo);
            rptProductDetail.DataSource = dtDetail1;
            rptProductDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Inspection> lstEntity = new List<Entity.Inspection>();
                // ----------------------------------------------------
                lstEntity = BAL.InspectionMgmt.GetInspectionList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), hdnTransType.Value, 1, 1000, out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtMovementCode.Text = lstEntity[0].pkID.ToString();
                txtInspectionDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].InspectionDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                txtCustomerName_TextChanged(null, null);
                drpOrder.SelectedValue = lstEntity[0].OrderNo;
                hdnEmployeeName.Value = lstEntity[0].EmployeeID.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                drpCheckList.SelectedValue = lstEntity[0].InspectionType.ToString();
                // -------------------------------------------------------------------------
                BindInspectionDetail(Convert.ToInt64(txtMovementCode.Text));

            }
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnCustomerID.Value = "";
            txtMovementCode.Text = "0";
            txtInspectionDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";
            drpOrder.SelectedValue = "";
            drpEmployee.SelectedValue = "";
            // ---------------------------------------------
            BindInspectionDetail(0);
            txtInspectionDate.Focus();

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                lstOrder = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), "", 0, 0);
                if (lstOrder.Count > 0)
                {
                    drpOrder.DataSource = lstOrder;
                    drpOrder.DataValueField = "OrderNo";
                    drpOrder.DataTextField = "OrderNo";
                    drpOrder.DataBind();
                }
            }
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            int ReturnCode = 0, ReturnpkID = 0;
            string ReturnMsg = "";
            string strErr = "";
            //--------------------------------------------------------------
            _pageValid = true;

            if (String.IsNullOrEmpty(txtInspectionDate.Text) || String.IsNullOrEmpty(txtCustomerName.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtInspectionDate.Text))
                    strErr += "<li>" + "Inspection Date is required." + "</li>";
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.Inspection objEntity = new Entity.Inspection();

                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.InspectionDate = (!String.IsNullOrEmpty(txtInspectionDate.Text)) ? Convert.ToDateTime(txtInspectionDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.OrderNo = drpOrder.SelectedValue;
                        objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                        objEntity.InspectionType = drpCheckList.SelectedValue;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();

                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.InspectionMgmt.AddUpdateInspection(objEntity, out ReturnCode, out ReturnMsg, out ReturnpkID);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        int ReturnCode1;
                        String ReturnMsg1;

                        if (ReturnCode > 0 && ReturnpkID > 0)
                        {

                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                            // --------------------------------------------------------------
                            if (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0")
                                BAL.InspectionMgmt.DeleteInspectionDetailByRefID(Convert.ToInt64(ReturnpkID), out ReturnCode1, out ReturnMsg1);

                            // --------------------------------------------------------------
                            // >>>>>>>> Second Insert all Selectd ProductGroup entry into table

                            Entity.InspectionDetail objEntity1 = new Entity.InspectionDetail();
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    objEntity1.RefID = ReturnpkID;
                                    objEntity1.CheckDesc = dr["CheckDesc"].ToString();
                                    objEntity1.CheckFlag = dr["CheckFlag"].ToString();
                                    objEntity1.CheckRemark = dr["CheckRemark"].ToString();
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.InspectionMgmt.AddUpdateInspectionDetail(objEntity1, out ReturnCode, out ReturnMsg);
                                }
                            }
                            if (ReturnCode > 0)
                            {
                                Session.Remove("dtDetail");

                            }
                        }
                    }
                    else
                    {
                        strErr += "<li>" + "Minimum One Product required !" + "</li>";
                    }
                }
                else
                {
                    strErr += "<li>" + "Minimum One Product required !" + "</li>";
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
        [System.Web.Services.WebMethod]
        public static string DeleteTransaction(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- 
            BAL.InspectionMgmt.DeleteInspection(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.InspectionMgmt.GetCheckList(drpCheckList.SelectedValue);
            rptProductDetail.DataSource = dtDetail1;
            rptProductDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void rptProductDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList drp = ((DropDownList)e.Item.FindControl("edCheckFlag"));
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnCheckFlag"));
                if (drp.Items.FindByText(hdn1.Value) != null)
                    drp.Items.FindByText(hdn1.Value).Selected = true;
            }
        }


        protected void rptProductDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    TextBox txtCheckDesc = (TextBox)e.Item.FindControl("txtCheckDesc");
                    DropDownList txtCheckFlag = (DropDownList)e.Item.FindControl("txtCheckFlag");
                    TextBox txtCheckRemark = (TextBox)e.Item.FindControl("txtCheckRemark");

                    if (String.IsNullOrEmpty(txtCheckDesc.Text) || String.IsNullOrEmpty(txtCheckFlag.SelectedValue))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnpkID.Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(txtCheckDesc.Text))
                            strErr += "<li>" + "Description is required." + "</li>";
                        if (String.IsNullOrEmpty(txtCheckFlag.SelectedValue))
                            strErr += "<li>" + "Flag is required." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        Int64 cntRow = 900000 + 1;

                        DataRow dr = dtDetail.NewRow();


                        string RefID = hdnpkID.Value;
                        string t1 = txtCheckDesc.Text;
                        string t2 = txtCheckFlag.SelectedValue;
                        string t3 = txtCheckRemark.Text;

                        dr["pkID"] = cntRow;
                        dr["RefID"] = (!String.IsNullOrEmpty(RefID)) ? Convert.ToInt64(RefID) : 0;
                        dr["CheckDesc"] = (!String.IsNullOrEmpty(t1)) ? t1 : "";
                        dr["CheckFlag"] = (!String.IsNullOrEmpty(t2)) ? t2 : "";
                        dr["CheckRemark"] = (!String.IsNullOrEmpty(t3)) ? t3 : "";
                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetail", dtDetail);
                        // ---------------------------------------------------------------
                        rptProductDetail.DataSource = dtDetail;
                        rptProductDetail.DataBind();
                    }
                    btnSave.Focus();
                }
                if (!string.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
            }
            // --------------------------------------------------------------------------
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

                    DataRow[] rows;
                    rows = dtDetail.Select("pkID=" + e.CommandArgument.ToString());
                    foreach (DataRow r in rows)
                        r.Delete();

                    rptProductDetail.DataSource = dtDetail;
                    rptProductDetail.DataBind();

                    Session.Add("dtDetail", dtDetail);
                }
                if (e.CommandName.ToString() == "Update")
                {
                    TextBox edCheckDesc = (TextBox)e.Item.FindControl("edCheckDesc");
                    DropDownList edCheckFlag = (DropDownList)e.Item.FindControl("edCheckFlag");
                    TextBox edCheckRemark = (TextBox)e.Item.FindControl("edCheckRemark");
                    // -------------------------------------------------------------
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

                    foreach (DataColumn col in dtDetail.Columns)
                        col.ReadOnly = false;

                    foreach (DataRow row in dtDetail.Rows)
                    {
                        if (row["CheckDesc"].ToString() == edCheckDesc.Text)
                        {
                            row["CheckFlag"] = edCheckFlag.SelectedValue;
                            row["CheckRemark"] = edCheckRemark.Text;
                            
                        }
                    }
                    dtDetail.AcceptChanges();

                    Session.Add("dtDetail", dtDetail);
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetSalesOrderNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetMaterialMovementOrderNo(pkID);
            return "MM-" + tempVal;
        }

        [WebMethod(EnableSession = true)]
        public static void GenerateMarterialMovementReport_Abhishek(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "yes";

            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------

            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
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
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "SalesBill");

            if (lstPrinter.Count > 0)
            {
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
            }


            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Sales Order Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            String tmpOrderNo = "";
            tmpOrderNo = BAL.CommonMgmt.GetMaterialMovementOrderNo(pkID);
            List<Entity.Material_Report> lstMaterial = new List<Entity.Material_Report>();
            lstMaterial = BAL.InspectionMgmt.MovementDetailByInvoiceNo(tmpOrderNo, 1, 1000, out TotalCount);

            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstMaterial.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstMaterial[0].CustomerID, "admin", 1, 1000, out TotalCount);


            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstMaterial.Count > 0)
            {
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 30, 20, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                //----------------------Customer Details Table---------------------------//

                PdfPTable tblCustomer = new PdfPTable(2);
                int[] column_tblCustomer = { 35, 65 };
                tblCustomer.SetWidths(column_tblCustomer);

                tblCustomer.AddCell(pdf.setCell("Customer Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell(": " + lstMaterial[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomer.AddCell(pdf.setCell("Mo. NO.: ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell(": " + lstCust[0].ContactNo1 + lstCust[0].ContactNo2, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomer.AddCell(pdf.setCell("Capacity Applied (kw):" + "", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell("Solar Module: _______________watt," + lstMaterial[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell("Type : ________________(Mono/Poly),", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell("Make : ________________(Waaree/Goldi/____________)", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell("Fabricatore Name : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomer.AddCell(pdf.setCell(": " + lstMaterial[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));




                PdfPTable tblAddress = new PdfPTable(2);
                int[] column_tblAddress = { 30, 70 };
                tblAddress.SetWidths(column_tblAddress);

                tblAddress.AddCell(pdf.setCell("Address :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(lstCust[0].Address, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(": " + lstMaterial[0].TransDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblAddress.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell("Fabrication material Reort", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblCustomer, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblAddress, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Material Movement Detail
                // -------------------------------------------------------------------------------------

                int[] column_tblDetail = { 7, 43, 24, 13, 13 };
                tblDetail.SetWidths(column_tblDetail);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 5, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 1));
                tblDetail.AddCell(pdf.setCell("SR.NO", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7));
                tblDetail.AddCell(pdf.setCell("       " + "DESCRIPTION", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7));
                tblDetail.AddCell(pdf.setCell("MATERIAL" + "\n" + "DISPATCHED   FT", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7));
                tblDetail.AddCell(pdf.setCell("MATERIAL" + "\n" + "USED   FT", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 7));
                tblDetail.AddCell(pdf.setCell("SURPLUS" + "\n" + "MATERIAL", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));



                for (int i = 0; i < lstMaterial.Count; i++)
                {

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 6));
                    tblDetail.AddCell(pdf.setCell("       " + lstMaterial[i].ProductName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                    tblDetail.AddCell(pdf.setCell(lstMaterial[i].Dispatched.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 6));
                    tblDetail.AddCell(pdf.setCell(lstMaterial[i].Consumed.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 6));
                    tblDetail.AddCell(pdf.setCell(lstMaterial[i].Surplus.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                }
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell("Sign Of Customer & Date", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell("Sign Of Fabricator & Date", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 3, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));



            }

            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "MM-" + tmpOrderNo.Replace("/", "-").ToString() + ".pdf";
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
    }
}
