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
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace StarsProject
{
    public partial class OutwardWithAssembly : System.Web.UI.Page
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
                Session.Remove("mySpecs");
                hdnSerialKey.Value = Session["SerialKey"].ToString().Replace("\r\n", "");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    BindOrderStatus();
                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        // ----------------------------------
                        List<Entity.OutwardDetailAssembly> lstObject = new List<Entity.OutwardDetailAssembly>();
                        lstObject = BAL.OutwardMgmt.GetOutwardDetailAssemblyList("", -1, 0);
                        DataTable dtAssembly = new DataTable();
                        dtAssembly = PageBase.ConvertListToDataTable(lstObject);
                        Session.Add("dtAssembly", dtAssembly);

                        BindOutwardAttachment("");
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
            else
            {
                if (uploadImgGallery.PostedFile != null)
                {
                    if (uploadImgGallery.PostedFile.FileName.Length > 0)
                    {

                        // ----------------------------------------------------------
                        if (uploadImgGallery.HasFile)
                        {
                            HttpFileCollection _HttpFileCollection = Request.Files;
                            for (int i = 0; i < _HttpFileCollection.Count; i++)
                            {
                                HttpPostedFile _HttpPostedFile = _HttpFileCollection[i];
                                if (_HttpPostedFile.ContentLength > 0)
                                {
                                    string filePath = _HttpPostedFile.FileName;
                                    string filename1 = Path.GetFileName(filePath);
                                    string ext = Path.GetExtension(filename1);
                                    string type = String.Empty;

                                    if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                                    {
                                        try
                                        {
                                            string rootFolderPath = Server.MapPath("OutwardAttachments");
                                            string filesToDelete = @"OWUpGall-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                            foreach (string file in fileList)
                                            {
                                                System.IO.File.Delete(file);
                                            }
                                            // -----------------------------------------------------
                                            String flname = "OWUpGall-" + filename1;
                                            String tmpFile = Server.MapPath("OutwardAttachments/") + flname;
                                            //_HttpPostedFile.SaveAs(tmpFile);
                                            // ---------------------------------------------------------------
                                            DataTable dtGall = new DataTable();
                                            dtGall = (DataTable)Session["dtImgGallery"];
                                            Int64 cntRow = 0;
                                            if (dtGall != null)
                                                cntRow = dtGall.Rows.Count + 1;
                                            else
                                                cntRow = 1;

                                            DataRow dr = dtGall.NewRow();
                                            dr["pkID"] = cntRow;
                                            dr["OutwardNo"] = String.IsNullOrEmpty(hdnpkID.Value)?"0": hdnpkID.Value;
                                            dr["AttachmentFile"] = flname;
                                            dr["LogID"] = cntRow;

                                            var plainTextBytes = Encoding.UTF8.GetBytes(_HttpPostedFile.InputStream.ToString());
                                            string utfString = Convert.ToBase64String(plainTextBytes);

                                            System.IO.Stream fs = _HttpPostedFile.InputStream;
                                            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                                            dr["ContentData"] = base64String;
                                            dtGall.Rows.Add(dr);
                                            Session.Add("dtImgGallery", dtGall);
                                            // ---------------------------------------------------------------
                                            rptProductImages.DataSource = dtGall;
                                            rptProductImages.DataBind();
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                        }
                                        catch (Exception ex) { }
                                    }
                                    else
                                        ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                                }

                            }

                        }
                    }
                }

                var requestTarget = this.Request["__EVENTTARGET"];
            }
        }

        public void OnlyViewControls()
        {
            txtOutwardNo.ReadOnly = true;
            txtOutwardDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        public void BindOutwardDetailList(string pOutwardNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.OutwardMgmt.GetOutwardDetail(pOutwardNo);
            rptOutwardDetail.DataSource = dtDetail1;
            rptOutwardDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        public List<Entity.OutwardDetailAssembly> BindOutwardDetailAssemblyList(string pOutwardNo, Int64 pProductID, Int64 pAssemblyID)
        {
            List<Entity.OutwardDetailAssembly> lstObject = new List<Entity.OutwardDetailAssembly>();
            lstObject = BAL.OutwardMgmt.GetOutwardDetailAssemblyList(pOutwardNo, pProductID, pAssemblyID);
            //DataTable dtAssembly = new DataTable();
            //dtAssembly = PageBase.ConvertListToDataTable(lstObject);
            //Session.Add("dtAssembly", dtAssembly);
            return lstObject;
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


                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                            strErr += "<li>" + "Quantity is required." + "</li>";

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
                            string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                            string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                            string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                            string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                            string qtyweight = ((TextBox)e.Item.FindControl("txtQuantityWeight")).Text;
                            string serialno = ((TextBox)e.Item.FindControl("txtSerialNo")).Text;
                            string boxno = ((TextBox)e.Item.FindControl("txtBoxNo")).Text;
                            
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            // Adding Product Assembly
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            DataTable dtAssembly = new DataTable();
                            dtAssembly = (DataTable)Session["dtAssembly"];

                           
                            List<Entity.ProductDetailCard> lstAssem = new List<Entity.ProductDetailCard>();
                            lstAssem = BAL.ProductMgmt.GetProductDetailList(0, Convert.ToInt64(icode), Session["LoginUserID"].ToString());
                            int seqVal = 1;
                            for (decimal i = 0; i < Convert.ToDecimal(qty); i++)
                            {
                                foreach (Entity.ProductDetailCard lstObject in lstAssem)
                                {
                                    DataRow drAss = dtAssembly.NewRow();
                                    drAss["pkID"] = seqVal;
                                    drAss["OutwardNo"] = txtOutwardNo.Text;
                                    drAss["ProductID"] = lstObject.FinishProductID.ToString();
                                    drAss["ProductName"] = lstObject.FinishProductName.ToString();
                                    drAss["AssemblyID"] = lstObject.ProductID.ToString();
                                    drAss["AssemblyName"] = lstObject.ProductName.ToString();
                                    drAss["Unit"] = unit;
                                    drAss["Quantity"] = lstObject.Quantity;
                                    drAss["QuantityWeight"] = 0;
                                    drAss["SerialNo"] = "";
                                    drAss["BoxNo"] = "";
                                    dtAssembly.Rows.Add(drAss);
                                    seqVal++;
                                }
                            }
                            dtAssembly.AcceptChanges();
                            Session.Add("dtAssembly", dtAssembly);

                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            DataTable mySpecs = new DataTable();
                            List<Entity.Products> ProdSpec = new List<Entity.Products>();

                            ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList("", Convert.ToInt64(icode), Session["LoginUserID"].ToString());
                            if (Session["mySpecs"] != null)
                            {
                                mySpecs = (DataTable)Session["mySpecs"];

                                DataRow drTemp = mySpecs.NewRow();
                                drTemp["pkID"] = Convert.ToInt64(icode);
                                drTemp["ProductSpecification"] = (ProdSpec.Count > 0) ? ProdSpec[0].ProductSpecification : "";
                                mySpecs.Rows.Add(drTemp);
                            }
                            else
                                mySpecs = PageBase.ConvertListToDataTable(ProdSpec);

                            mySpecs.AcceptChanges();
                            Session.Add("mySpecs", mySpecs);
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dr["QuantityWeight"] = (!String.IsNullOrEmpty(qtyweight)) ? Convert.ToDecimal(qtyweight) : 0;
                            dr["SerialNo"] = (!String.IsNullOrEmpty(serialno)) ? serialno : "";
                            dr["BoxNo"] = (!String.IsNullOrEmpty(boxno)) ? boxno : "";

                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptOutwardDetail.DataSource = dtDetail;
                            rptOutwardDetail.DataBind();
                            //----------------------------------------------------------------
                            Session.Add("dtDetail", dtDetail);
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*


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

        protected void rptOutwardDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //}
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Outward> lstEntity = new List<Entity.Outward>();
                // ----------------------------------------------------
                lstEntity = BAL.OutwardMgmt.GetOutwardList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtOutwardNo.Text = lstEntity[0].OutwardNo;
                txtOutwardDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].OutwardDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;

                drpModeOfTransport.SelectedValue = lstEntity[0].ModeOfTransport;
                txtVehicleNo.Text = lstEntity[0].VehicleNo;
                txtTransporterName.Text = lstEntity[0].TransporterName;
                txtLRNo.Text = lstEntity[0].LRNo;
                txtLRDate.Text = (lstEntity[0].LRDate != null) ? lstEntity[0].LRDate.Value.ToString("yyyy-MM-dd") : null;
                txtDCNo.Text = lstEntity[0].DCNo;
                txtDCDate.Text = (lstEntity[0].DCDate != null) ? lstEntity[0].DCDate.Value.ToString("yyyy-MM-dd") : null;
                txtDeliveryNote.Text = lstEntity[0].DeliveryNote;

                txtManualOutwardNo.Text = lstEntity[0].ManualOutwardNo;

                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    
                    BindSalesOrderList(Convert.ToInt64(hdnCustomerID.Value));
                    if (!String.IsNullOrEmpty(lstEntity[0].OrderNo) )
                    {
                        if (drpReferenceNo.Items.FindByText(lstEntity[0].OrderNo) != null)
                            drpReferenceNo.Items.FindByText(lstEntity[0].OrderNo).Selected = true;
                    }
                    if (!String.IsNullOrEmpty(lstEntity[0].OrderStatus))
                    {
                        if (drpOrderStatus.Items.FindByText(lstEntity[0].OrderStatus) != null)
                            drpOrderStatus.Items.FindByText(lstEntity[0].OrderStatus).Selected = true;
                    }
                }
                // -------------------------------------------------
                BindOutwardDetailList(txtOutwardNo.Text);
                // -------------------------------------------------
                List<Entity.OutwardDetailAssembly> lstObject = new List<Entity.OutwardDetailAssembly>();
                lstObject = BAL.OutwardMgmt.GetOutwardDetailAssemblyList(txtOutwardNo.Text, 0, 0);
                DataTable dtAssembly = new DataTable();
                dtAssembly = PageBase.ConvertListToDataTable(lstObject);
                Session.Add("dtAssembly", dtAssembly);
                // -------------------------------------------------

                //--------------------------------------------------
                //Bind Attachment
                //--------------------------------------------------
                BindOutwardAttachment(lstEntity[0].OutwardNo);

                txtOutwardDate.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            //----------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnOutwardNo = "";
            string strErr = "";
            _pageValid = true;


            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(txtOutwardDate.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtOutwardDate.Text))
                    strErr += "<li>" + "Outward Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Selection is required." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        Entity.Outward objEntity = new Entity.Outward();

                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                        objEntity.OutwardNo = txtOutwardNo.Text;
                        objEntity.OutwardDate = Convert.ToDateTime(txtOutwardDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);

                        if (!String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                            objEntity.OrderNo = drpReferenceNo.SelectedItem.Value;

                        if (!String.IsNullOrEmpty(drpOrderStatus.SelectedValue))
                            objEntity.OrderStatus = drpOrderStatus.SelectedItem.Text;

                        objEntity.ModeOfTransport = drpModeOfTransport.SelectedValue;
                        objEntity.VehicleNo = txtVehicleNo.Text;
                        objEntity.TransporterName = txtTransporterName.Text;
                        objEntity.LRNo = txtLRNo.Text;

                        objEntity.ManualOutwardNo = txtManualOutwardNo.Text;

                        if (!String.IsNullOrEmpty(txtLRDate.Text))
                        {
                            if (Convert.ToDateTime(txtLRDate.Text).Year > 1900)
                                objEntity.LRDate = Convert.ToDateTime(txtLRDate.Text);
                        }
                        objEntity.DCNo = txtDCNo.Text;
                        if (!String.IsNullOrEmpty(txtDCDate.Text))
                        {
                            if (Convert.ToDateTime(txtDCDate.Text).Year > 1900)
                                objEntity.DCDate = Convert.ToDateTime(txtDCDate.Text);
                        }
                        //objEntity.DCDate = (!String.IsNullOrEmpty(txtDCDate.Text)) ? Convert.ToDateTime(txtDCDate.Text) : (DateTime?)null;
                        objEntity.DeliveryNote = txtDeliveryNote.Text;

                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.OutwardMgmt.AddUpdateOutward(objEntity, out ReturnCode, out ReturnMsg, out ReturnOutwardNo);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        if (!String.IsNullOrEmpty(ReturnOutwardNo))
                        {
                            txtOutwardNo.Text = ReturnOutwardNo;
                        }
                        // --------------------------------------------------------------

                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        if (ReturnCode > 0)
                        {

                            BAL.OutwardMgmt.DeleteOutwardDetailByOutwardNo(txtOutwardNo.Text, out ReturnCode, out ReturnMsg);
                            // ----------------------------------------------------------
                            Entity.OutwardDetail objQuotDet = new Entity.OutwardDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.OutwardNo = txtOutwardNo.Text;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.QuantityWeight = Convert.ToDecimal(dr["QuantityWeight"]);
                                objQuotDet.SerialNo = dr["SerialNo"].ToString();
                                objQuotDet.BoxNo = dr["BoxNo"].ToString();
                                if (Session["mySpecs"] != null)
                                {
                                    Boolean itemAdded = false;
                                    DataTable mySpecs = new DataTable();
                                    mySpecs = (DataTable)Session["mySpecs"];
                                    foreach (DataRow row in mySpecs.Rows)
                                    {
                                        if (row["pkID"].ToString() == dr["ProductID"].ToString())
                                        {
                                            objQuotDet.ProductSpecification = row["ProductSpecification"].ToString();
                                            itemAdded = true;
                                        }
                                    }

                                    if (!itemAdded)
                                    {
                                        objQuotDet.ProductSpecification = dr["ProductSpecification"].ToString();
                                    }
                                }
                                else
                                {
                                    objQuotDet.ProductSpecification = dr["ProductSpecification"].ToString();
                                }
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.OutwardMgmt.AddUpdateOutwardDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            // ----------------------------------------------------------
                            // Outward Detail Assembly
                            // ----------------------------------------------------------
                            BAL.OutwardMgmt.DeleteOutwardDetailAssemblyByOutwardNo(txtOutwardNo.Text, 0, 0, out ReturnCode, out ReturnMsg);

                            DataTable dtAssembly = new DataTable();
                            dtAssembly = (DataTable)Session["dtAssembly"];

                            Entity.OutwardDetailAssembly objAssembly = new Entity.OutwardDetailAssembly();
                            foreach (DataRow dr in dtAssembly.Rows)
                            {
                                objAssembly.pkID = 0;
                                objAssembly.OutwardNo = txtOutwardNo.Text;
                                objAssembly.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objAssembly.AssemblyID = Convert.ToInt64(dr["AssemblyID"]);
                                objAssembly.Unit = dr["Unit"].ToString();
                                objAssembly.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objAssembly.QuantityWeight = Convert.ToDecimal(dr["QuantityWeight"]);
                                objAssembly.SerialNo = dr["SerialNo"].ToString();
                                objAssembly.BoxNo = dr["BoxNo"].ToString();
                                BAL.OutwardMgmt.AddUpdateOutwardDetailAssembly(objAssembly, out ReturnCode1, out ReturnMsg1);
                            }
                            // --------------------------------------------------------

                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            // SAVE - Product Image Gallery
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            BAL.OutwardMgmt.DeleteOutwardAttachment(ReturnOutwardNo, out ReturnCode1, out ReturnMsg1);
                            DataTable dtImgGall = new DataTable();
                            dtImgGall = (DataTable)Session["dtImgGallery"];

                            if (dtImgGall != null)
                            {
                                foreach (DataRow dr in dtImgGall.Rows)
                                {
                                    if (dr.RowState.ToString() != "Deleted")
                                    {
                                        Entity.Outward_Attachment lstObj = new Entity.Outward_Attachment();
                                        lstObj.pkID = 0;
                                        lstObj.OutwardNo = ReturnOutwardNo;
                                        lstObj.AttachmentFile = dr["AttachmentFile"].ToString();
                                        lstObj.LogID = 0;
                                        // -------------------------------------------------------------- Insert/Update Record
                                        BAL.OutwardMgmt.AddUpdateOutwardAttachment(lstObj, out ReturnCode1, out ReturnMsg1);

                                        String flname = dr["AttachmentFile"].ToString();
                                        String tmpFile = Server.MapPath("OutwardAttachments/") + flname;

                                        if (dr["ContentData"] != null && !String.IsNullOrEmpty(dr["ContentData"].ToString()))
                                            System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(dr["ContentData"].ToString()));
                                        else
                                            System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(PageBase.ConvertImageToBase64(tmpFile)));

                                    }
                                    if (dr.RowState.ToString() == "Deleted")
                                    {
                                        String flname = dr["AttachmentFile"].ToString();
                                        String tmpFile = Server.MapPath("OutwardAttachments/") + flname;
                                        System.IO.File.Delete(tmpFile);
                                    }

                                }
                            }

                            Session.Remove("dtAssembly");
                            Session.Remove("dtDetail");
                            Session.Remove("mySpecs");
                            Session.Remove("dtImgGallery");

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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtOutwardNo.Text = ""; //  BAL.CommonMgmt.fnGetOutwardNoByDate(DateTime.Today.ToString("yyyy-MM-dd"));
            txtOutwardDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";
            drpModeOfTransport.SelectedValue = "";
            txtVehicleNo.Text = "";
            txtTransporterName.Text = "";
            txtLRNo.Text = "";
            txtLRDate.Text = "";
            txtDCNo.Text = "";
            txtDCDate.Text = "";
            txtDeliveryNote.Text = "";
            txtManualOutwardNo.Text = "";
            BindOutwardDetailList("");
            // --------------------------------------------
            txtOutwardDate.Focus();
            btnSave.Disabled = false;
        }

        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptOutwardDetail.Controls[rptOutwardDetail.Controls.Count - 1].Controls[0];
            string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            TextBox txUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            if (!String.IsNullOrEmpty(ctrl1))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            txUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            txtDiscountPercent.Text = "0";
            txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
        }
        public void BindOrderStatus()
        {
            drpOrderStatus.DataSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("SOApproval");
            drpOrderStatus.DataValueField = "InquiryStatusName";
            drpOrderStatus.DataTextField = "InquiryStatusName";
            drpOrderStatus.DataBind();
        }
        [System.Web.Services.WebMethod]
        public static string DeleteOutward(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.OutwardMgmt.DeleteOutward(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            //editItem_TextChanged1();
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");
            TextBox edQuantityWeight = (TextBox)item.FindControl("edQuantityWeight");
            TextBox edSerialNo = (TextBox)item.FindControl("edSerialNo");
            TextBox edBoxNo = (TextBox)item.FindControl("edBoxNo");
            // --------------------------------------------------------------------------
            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            Decimal qw = (!String.IsNullOrEmpty(edQuantityWeight.Text)) ? Convert.ToDecimal(edQuantityWeight.Text) : 0;
            //Decimal sn = (!String.IsNullOrEmpty(edSerialNo.Text)) ? Convert.ToDecimal(edSerialNo.Text) : 0;
            //Decimal bn = (!String.IsNullOrEmpty(edBoxNo.Text)) ? Convert.ToDecimal(edBoxNo.Text) : 0;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;
            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["ProductName"].ToString() == edProductName.Value)
                {
                    row.SetField("Quantity", q);
                    row.SetField("Unit", edUnit.Text);
                    row.SetField("QuantityWeight", qw);
                    row.SetField("SerialNo", edSerialNo.Text);
                    row.SetField("BoxNo", edBoxNo.Text);
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
            TextBox txtUnitRate = (TextBox)rptFootCtrl.FindControl("txtUnitRate");
            TextBox txtDiscountPercent = (TextBox)rptFootCtrl.FindControl("txtDiscountPercent");
            TextBox txtNetRate = (TextBox)rptFootCtrl.FindControl("txtNetRate");
            TextBox txtAmount = (TextBox)rptFootCtrl.FindControl("txtAmount");
            TextBox txtTaxRate = (TextBox)rptFootCtrl.FindControl("txtTaxRate");
            TextBox txtTaxAmount = (TextBox)rptFootCtrl.FindControl("txtTaxAmount");
            TextBox txtNetAmount = (TextBox)rptFootCtrl.FindControl("txtNetAmount");
            // --------------------------------------------------------------------------
            Decimal q = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDecimal(txtQuantity.Text);
            Decimal ur = String.IsNullOrEmpty(txtUnitRate.Text) ? 0 : Convert.ToDecimal(txtUnitRate.Text);
            Decimal dp = String.IsNullOrEmpty(txtDiscountPercent.Text) ? 0 : Convert.ToDecimal(txtDiscountPercent.Text);
            Decimal nr = String.IsNullOrEmpty(txtNetRate.Text) ? 0 : Convert.ToDecimal(txtNetRate.Text);
            Decimal a = String.IsNullOrEmpty(txtAmount.Text) ? 0 : Convert.ToDecimal(txtAmount.Text);
            Decimal tr = String.IsNullOrEmpty(txtTaxRate.Text) ? 0 : Convert.ToDecimal(txtTaxRate.Text);
            Decimal ta = String.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
            Decimal na = String.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
            // --------------------------------------------------------------------------
            nr = Math.Round((ur - ((ur * dp) / 100)), 2);
            a = Math.Round((q * nr), 2);
            ta = Math.Round(((a * tr) / 100), 2);
            na = Math.Round((a + ta), 2);

            txtNetRate.Text = nr.ToString();
            txtAmount.Text = a.ToString();
            txtTaxAmount.Text = ta.ToString();
            txtNetAmount.Text = na.ToString();
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
            TextBox txtQuantityWeight = ((TextBox)rptFootCtrl.FindControl("txtQuantityWeight"));
            TextBox txtSerialNo = ((TextBox)rptFootCtrl.FindControl("txtSerialNo"));
            TextBox txtBoxNo = ((TextBox)rptFootCtrl.FindControl("txtBoxNo"));
            List<Entity.Products> lstEntity = new List<Entity.Products>();

            if (!String.IsNullOrEmpty(hdnProductID.Value))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";

            // ------------------------------------------------------------------
            txtQuantity.Focus();

        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                BindSalesOrderList(Convert.ToInt64(hdnCustomerID.Value));

            drpReferenceNo.Focus();
        }

        public void BindSalesOrderList(Int64 pCustomerID)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            drpReferenceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                if (hdnSerialKey.Value == "DYNA-2GF3-J7G8-FF12") // For Dynamic
                    lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(pCustomerID);
                else
                    lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", pCustomerID, "", 0, 0);
                // --------------------------------------------------
                if (hdnSerialKey.Value == "STX1-UP06-YU89-JK23")    // For Stainex
                {
                    drpReferenceNo.DataValueField = "OrderNo";
                    drpReferenceNo.DataTextField = "ReferenceNo";
                }
                else
                {
                    drpReferenceNo.DataValueField = "OrderNo";
                    drpReferenceNo.DataTextField = "OrderNo";
                }
                if (lstEntity.Count > 0)
                {
                    drpReferenceNo.DataSource = lstEntity;
                    drpReferenceNo.DataBind();
                }
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }

        protected void drpReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedItem.Value))
            {
                dtDetail.Clear();
                if(hdnSerialKey.Value == "DYNA-2GF3-J7G8-FF12")
                    dtDetail = BAL.SalesOrderMgmt.GetSalesOrderDetailForOut(drpReferenceNo.SelectedItem.Value);
                else
                    dtDetail = BAL.SalesOrderMgmt.GetSalesOrderDetailForSale(drpReferenceNo.SelectedItem.Value);
            }

            Session.Add("dtDetail", dtDetail);
            //dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
            //Session.Add("dtDetail", dtDetail);

            rptOutwardDetail.DataSource = dtDetail;
            rptOutwardDetail.DataBind();
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            // Adding Product Assembly
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];

            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                string icode = dtDetail.Rows[i]["ProductID"].ToString();
                string qty = dtDetail.Rows[i]["Quantity"].ToString();

                if (!String.IsNullOrEmpty(icode))
                {
                    List<Entity.ProductDetailCard> lstAssem = new List<Entity.ProductDetailCard>();
                    lstAssem = BAL.ProductMgmt.GetProductDetailList(0, Convert.ToInt64(icode), Session["LoginUserID"].ToString());
                    int seqVal = 1;
                    for (decimal j = 0; j < Convert.ToDecimal(qty); j++)
                    {
                        foreach (Entity.ProductDetailCard lstObject in lstAssem)
                        {
                            DataRow drAss = dtAssembly.NewRow();
                            drAss["pkID"] = seqVal;
                            drAss["OutwardNo"] = txtOutwardNo.Text;
                            drAss["ProductID"] = lstObject.FinishProductID.ToString();
                            drAss["ProductName"] = lstObject.FinishProductName.ToString();
                            drAss["AssemblyID"] = lstObject.ProductID.ToString();
                            drAss["AssemblyName"] = lstObject.ProductName.ToString();
                            drAss["Quantity"] = lstObject.Quantity;
                            drAss["QuantityWeight"] = 0;
                            drAss["SerialNo"] = "";
                            drAss["BoxNo"] = "";
                            dtAssembly.Rows.Add(drAss);
                            seqVal++;
                        }
                    }
                }
            }
            dtAssembly.AcceptChanges();
            Session.Add("dtAssembly", dtAssembly);
        }

        [WebMethod]
        public static string GetOutwardNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetOutwardNo(pkID);
            return tempVal;
        }
        [WebMethod(EnableSession = true)]
        public static void GenerateOutward(Int64 pkID)
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

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {

        }

        protected void rptOrderDocs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void rptProductImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtImgGallery = (DataTable)Session["dtImgGallery"];
                for (int i = dtImgGallery.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtImgGallery.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                    {

                        string rootFolderPath = Server.MapPath("OutwardAttachments");
                        string filesToDelete = System.IO.Path.GetFileNameWithoutExtension(dr["AttachmentFile"].ToString()) + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                        string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                        foreach (string file in fileList)
                        {
                            System.IO.File.Delete(file);
                        }
                        // -----------------------------------------------
                        dr.Delete();
                    }
                }
                dtImgGallery.AcceptChanges();
                Session.Add("dtImgGallery", dtImgGallery);

                rptProductImages.DataSource = dtImgGallery;
                rptProductImages.DataBind();
                // -----------------------------------------------
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }

        protected void btnUpload3_Click(object sender, EventArgs e)
        {

        }

        public void BindOutwardAttachment(string OutwardNo)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.Outward_Attachment> lst = BAL.OutwardMgmt.GetOutwardAttachmentList(0, OutwardNo);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);

            rptProductImages.DataSource = dtDetail1;
            rptProductImages.DataBind();

            Session.Add("dtImgGallery", dtDetail1);
        }
    }
}