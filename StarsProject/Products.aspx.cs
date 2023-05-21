using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;

namespace StarsProject
{
    public partial class Products : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove("dtDocs");
                Session.Remove("dtProductCard");
                Session.Remove("dtAccessories");
                Session.Remove("dtSpecs");
                // ---------------------------------------------------------
                Session["PageNo"] = 1;
                Session["OldUserID"] = "";
                Session["PageSize"] = 15;
                hdnSerialKey.Value = Session["SerialKey"].ToString();

                BindDropDown();
                hdnClientERPIntegration.Value = BAL.CommonMgmt.GetConstant("ClientERPIntegration", 0, 1);
                hdnQuatSpecRemark.Value = BAL.CommonMgmt.GetConstant("QuatSpecRemark", 0, 1);
                hdnQuotSpecFormat.Value = BAL.CommonMgmt.GetConstant("QuotationSpecificationFormat", 0, 1);
                hdnApplicationIndustry.Value = BAL.CommonMgmt.GetConstant("ApplicationIndustry", 0, 1);
                if (hdnQuotSpecFormat.Value == "old")
                {
                    txtProductSpecification.Attributes.Remove("content");
                    txtProductSpecification.Attributes.Add("class", "form-control");
                }
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        hdnpkID.Value = "0";
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

                BindProductSpecs();
            }
            else
            {
                hdnCurrentTab.Value = Request.Form[hdnCurrentTab.UniqueID];
                // ----------------------------------------------------------------------
                // Product Iamge Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.HasFile)
                    {
                        string filePath = FileUpload1.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            //if (!String.IsNullOrEmpty(hdnpkID.Value.Trim()))
                            //{
                                string rootFolderPath = Server.MapPath("productimages");
                                string filesToDelete = @"product-" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                foreach (string file in fileList)
                                {
                                    //System.Diagnostics.Debug.WriteLine(file + "  Will be deleted");
                                    System.IO.File.Delete(file);
                                }
                                // -----------------------------------------------------
                                String flname = "product-" + hdnpkID.Value.Trim() + ext;
                                FileUpload1.SaveAs(Server.MapPath("productimages/") + flname);
                                imgProduct.ImageUrl = "";
                                imgProduct.ImageUrl = "productimages/" + flname;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Product Image Uploaded Successfully, Please Save Record  !');", true);
                            //}
                        } 
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                    }
                }

                // ----------------------------------------------------------------------
                // Product Document Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (uploadDocument.PostedFile != null)
                {
                    if (uploadDocument.PostedFile.FileName.Length > 0)
                    {
                       
                        // ----------------------------------------------------------
                        if (uploadDocument.HasFile)
                        {
                            string filePath = uploadDocument.PostedFile.FileName;
                            string filename1 = Path.GetFileName(filePath);
                            string ext = Path.GetExtension(filename1).ToLower();
                            string type = String.Empty;

                             if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                            {
                                try
                                {
                                    string rootFolderPath = Server.MapPath("productimages");
                                    string filesToDelete = @"prdoc-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                    // -----------------------------------------------------
                                    String flname = "prdoc-" + filename1;
                                    String tmpFile = Server.MapPath("productimages/") + flname;
                                    uploadDocument.PostedFile.SaveAs(tmpFile);
                                    // ---------------------------------------------------------------
                                    DataTable dtDocs = new DataTable();
                                    dtDocs = (DataTable)Session["dtDocs"];
                                    Int64 cntRow = dtDocs.Rows.Count + 1;
                                    DataRow dr = dtDocs.NewRow();
                                    dr["pkID"] = cntRow;
                                    dr["FileName"] = flname;
                                    dr["Filetype"] = type;
                                    dr["CreatedBy"] = Session["LoginUserID"].ToString();
                                    //dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                                    dr["CreatedDate"] = DateTime.Now.ToString();
                                    dtDocs.Rows.Add(dr);
                                    Session.Add("dtDocs", dtDocs);
                                    // ---------------------------------------------------------------
                                    rptProdDocs.DataSource = dtDocs;
                                    rptProdDocs.DataBind();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                }
                                catch (Exception ex) { }
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                        }
                    }
                }
                // ----------------------------------------------------------------------
                // Product Image Gallery Upload On .... Page Postback
                // ----------------------------------------------------------------------
                //var file = fuAttachment.PostedFile;
                //if (file != null)
                //{
                //    var content = new byte[file.ContentLength];
                //    file.InputStream.Read(content, 0, content.Length);
                //    Session["FileContent"] = content;
                //    Session["FileContentType"] = file.ContentType;
                //}
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
                                //_HttpPostedFile.SaveAs(Server.MapPath("~/a4d/ComposeEmail/" + Path.GetFileName(_HttpPostedFile.FileName)));
                                if (_HttpPostedFile.ContentLength > 0)
                                {
                                    string filePath = _HttpPostedFile.FileName;
                                    string filename1 = Path.GetFileName(filePath);
                                    string ext = Path.GetExtension(filename1).ToLower();
                                    string type = String.Empty;

                                    if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                                    {
                                        try
                                        {
                                            string rootFolderPath = Server.MapPath("productimages");
                                            string filesToDelete = @"prGall-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                            foreach (string file in fileList)
                                            {
                                                System.IO.File.Delete(file);
                                            }
                                            // -----------------------------------------------------
                                            String flname = "prGall-" + filename1;
                                            String tmpFile = Server.MapPath("productimages/") + flname;
                                            //_HttpPostedFile.SaveAs(tmpFile);
                                            // ---------------------------------------------------------------
                                            DataTable dtGall = new DataTable();
                                            dtGall = (DataTable)Session["dtImgGallery"];
                                            Int64 cntRow = dtGall.Rows.Count + 1;
                                            DataRow dr = dtGall.NewRow();
                                            dr["pkID"] = cntRow;
                                            dr["ProductID"] = Convert.ToInt64(hdnpkID.Value);
                                            dr["Name"] = flname;
                                            dr["Type"] = type;
                                            //var content = new byte[_HttpPostedFile.ContentLength];
                                            //_HttpPostedFile.InputStream.Read(content, 0, content.Length);
                                            //dr["ContentData"] = content;
                                            //BinaryReader br = new BinaryReader(_HttpPostedFile.InputStream);
                                            //byte[] bytes = br.ReadBytes((Int32)_HttpPostedFile.InputStream.Length);
                                            //string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

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

            }
            if (String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0")
            {
                liBrochure.Visible = false;
                liProdImages.Visible = false;
                proddocs.Style.Add("display", "none");
                prodimg.Style.Add("display", "none");
            }

            // -----------------------------------------------
            if (!PageBase.checkGeneralMenuAccess("product"))
            {
                btnPanel.Style.Add("display", "none");
                divRestriction.Style.Add("display", "block");
                divRestriction.InnerText = "Sorry ! You Can't Save Product. Product Module Rights Restricted For Your Role.";
            }

        }

        public void BindProductDocuments(Int64 HeaderID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.Documents> lst = BAL.CommonMgmt.GetDocumentsList(0, HeaderID);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            //rptFileListCtrl.DataSource = dtDetail1;
            //rptFileListCtrl.DataBind();

            rptProdDocs.DataSource = dtDetail1;
            rptProdDocs.DataBind();

            Session.Add("dtDocs", dtDetail1);

        }

        public void OnlyViewControls()
        {
            txtProductName.ReadOnly = true;
            txtProductAlias.ReadOnly = true;
            txtUnit.ReadOnly = true;
            txtUnitPrice.ReadOnly = true;
            txtTaxRate.ReadOnly = true;
            //txtAddTaxPer.ReadOnly = true;
            txtProductSpecification.ReadOnly = true;
            drpProductGroup.Attributes.Add("disabled", "disabled");
            drpBrand.Attributes.Add("disabled", "disabled");
            drpProductType.Attributes.Add("disabled", "disabled");
            chkActive.Enabled = false;
            drpTaxType.Attributes.Add("disabled", "disabled");
            uploadDocument.Enabled = false;
            FileUpload1.Enabled = false;
            txtUnitQuantity.ReadOnly = true;
            txtUnitSize.ReadOnly = true;
            txtUnitSurface.ReadOnly = true;
            txtLRDate.ReadOnly = true;
            drpUnitGrade.Attributes.Add("disabled", "disabled");
            txtBox_Weight.ReadOnly = true;
            txtBox_SQFT.ReadOnly = true;
            txtBox_SQMT.ReadOnly = true;

            txtHSNCode.ReadOnly = true;
            txtMinUnitPrice.ReadOnly = true;
            txtMaxUnitPrice.ReadOnly = true;
            txtProfitRatio.ReadOnly = true;
            txtMinQuantity.ReadOnly = true;
            txtOpeningSTK.ReadOnly = true;
            txtOpeningValuation.ReadOnly = true;
            txtOpeningWeightRate.ReadOnly = true;
            //txtManPower.ReadOnly = true;
            //txtHorsePower.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }
       
        public void BindDropDown()
        {

            // ---------------- Product Group List -------------------------------------
            List<Entity.ProductGroup> lstEvents = new List<Entity.ProductGroup>();
            lstEvents = BAL.ProductGroupMgmt.GetProductGroupList();
            lstEvents = lstEvents.Where(e => (e.ActiveFlag == true)).ToList();
            drpProductGroup.DataSource = lstEvents;
            drpProductGroup.DataValueField = "pkID";
            drpProductGroup.DataTextField = "ProductGroupName";
            drpProductGroup.DataBind();
            drpProductGroup.Items.Insert(0, new ListItem("-- Select --", "0"));

            // ---------------- Brand List -------------------------------------
            List<Entity.Brand> lstEvents1 = new List<Entity.Brand>();
            lstEvents1 = BAL.BrandMgmt.GetBrandList();
            lstEvents1 = lstEvents1.Where(e => (e.ActiveFlag == true)).ToList();
            drpBrand.DataSource = lstEvents1;
            drpBrand.DataValueField = "pkID";
            drpBrand.DataTextField = "BrandName";
            drpBrand.DataBind();
            drpBrand.Items.Insert(0, new ListItem("-- Select --", "0"));

        }

        public void setLayout(string pMode)
        {

            if (String.IsNullOrEmpty(hdnApplicationIndustry.Value.ToLower()) || hdnApplicationIndustry.Value.ToLower() == "ceramic")
            {
                dvCeraBox.Visible = false;
                dvCeraSize.Visible = false;
                dvCeraSurface.Visible = false;
            }
            // ------------------------------------------------------
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                // ----------------------------------------------------
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProductName.Text = lstEntity[0].ProductName;
                txtProductAlias.Text = lstEntity[0].ProductAlias;
                txtProductSpecification.Text = lstEntity[0].ProductSpecification;
                drpProductType.SelectedValue = lstEntity[0].ProductType;
                txtUnit.Text = lstEntity[0].Unit;
                txtUnitPrice.Text = lstEntity[0].UnitPrice.ToString();
                txtTaxRate.Text = lstEntity[0].TaxRate.ToString();
                //txtAddTaxPer.Text = lstEntity[0].AddTaxPer.ToString();
                txtHSNCode.Text = lstEntity[0].HSNCode.ToString();
                txtLRDate.Text = Convert.ToDateTime(lstEntity[0].LRDate).ToString("yyyy-MM-dd");
                //txtManPower.Text = lstEntity[0].ManPower.ToString();
                //txtHorsePower.Text = lstEntity[0].HorsePower.ToString();
                txtMinQuantity.Text = lstEntity[0].MinQuantity.ToString();
                txtUnitQuantity.Text = lstEntity[0].UnitQuantity.ToString();
                txtUnitSize.Text = lstEntity[0].UnitSize.ToString();
                txtUnitSurface.Text = lstEntity[0].UnitSurface.ToString();
                txtMinUnitPrice.Text = lstEntity[0].Min_UnitPrice.ToString();
                txtMaxUnitPrice.Text = lstEntity[0].Max_UnitPrice.ToString();
                txtProfitRatio.Text = lstEntity[0].ProfitRatio.ToString();

                drpUnitGrade.SelectedValue = lstEntity[0].UnitGrade.ToString();
                txtBox_Weight.Text = lstEntity[0].Box_Weight.ToString();
                txtBox_SQFT.Text = lstEntity[0].Box_SQFT.ToString();
                txtBox_SQMT.Text = lstEntity[0].Box_SQMT.ToString();
                txtOpeningSTK.Text = lstEntity[0].OpeningSTK.ToString();
                txtInwardSTK.Text = lstEntity[0].InwardSTK.ToString();
                txtOutwardSTK.Text = lstEntity[0].OutwardSTK.ToString();
                txtClosingSTK.Text = lstEntity[0].ClosingSTK.ToString();
                txtOpeningValuation.Text = lstEntity[0].OpeningValuation.ToString();
                txtOpeningWeightRate.Text = lstEntity[0].OpeningWeightRate.ToString();
                // ------------------------------------------------------------------------------
                // Below if ... for showing ERP Closing Stock ( Handled from MST_Constant table)
                // ------------------------------------------------------------------------------
                if (hdnClientERPIntegration.Value.ToLower() == "yes") 
                    lblClosingStock.Text = lstEntity[0].ClosingSTK.ToString();

                if (!String.IsNullOrEmpty(lstEntity[0].ProductGroupID.ToString()) || lstEntity[0].ProductGroupID.ToString()!="0")
                    drpProductGroup.SelectedValue = lstEntity[0].ProductGroupID.ToString();

                if (!String.IsNullOrEmpty(lstEntity[0].BrandID.ToString()) || lstEntity[0].BrandID.ToString() != "0")
                    drpBrand.SelectedValue = lstEntity[0].BrandID.ToString();

                chkActive.Checked = lstEntity[0].ActiveFlag;
                imgProduct.ImageUrl = lstEntity[0].ProductImage;
             
                if (!String.IsNullOrEmpty(lstEntity[0].TaxType.ToString()))
                {
                    if(lstEntity[0].TaxType.ToString()=="0")
                        drpTaxType.SelectedValue ="0";
                    else if(lstEntity[0].TaxType.ToString()=="1")
                        drpTaxType.SelectedValue ="1";
                }

                txtProductName.Focus();
                // -------------------------------------------------------------------------
                // Product Documents
                // -------------------------------------------------------------------------
                BindProductDocuments(Convert.ToInt64(hdnpkID.Value));
                // -------------------------------------------------------------------------
                // Product Image Gallery
                // -------------------------------------------------------------------------
                BindProductGallery(Convert.ToInt64(hdnpkID.Value));
                // -------------------------------------------------------------------------
                // Product Detail Assembly
                // -------------------------------------------------------------------------
                BindProductDetail();
                BindProductAccessories();
                BindProductSpecs();
            }                                                                                                                                                    
        }

        protected void btnUpload1_Click(object sender, EventArgs e) { }
        public void BindProductGallery(Int64 HeaderID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.ProductImages> lst = BAL.ProductMgmt.GetProductImageList(0, HeaderID);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);

            rptProductImages.DataSource = dtDetail1;
            rptProductImages.DataBind();

            Session.Add("dtImgGallery", dtDetail1);

        }
        protected void btnUpload3_Click(object sender, EventArgs e) { }
        protected void rptProductImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // https://www.aspsnippets.com/Articles/Upload-and-Download-files-from-Folder-Directory-in-ASPNet-using-C-and-VBNet.aspx
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtImgGallery = (DataTable)Session["dtImgGallery"];
                for (int i = dtImgGallery.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtImgGallery.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                    {
                        
                        string rootFolderPath = Server.MapPath("productimages");
                        string filesToDelete = System.IO.Path.GetFileNameWithoutExtension(dr["Name"].ToString()) + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
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

        public string ConvertImageToBase64(string xFilePath)
        {
            string base64ImageRepresentation = "";
            byte[] imageArray = System.IO.File.ReadAllBytes(@xFilePath);
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1=0;
            long ReturnProductId = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            String strErr = "";
            _pageValid = true;
            divErrorMessage.InnerHtml = "";

            if (String.IsNullOrEmpty(txtProductName.Text) || String.IsNullOrEmpty(txtProductAlias.Text) ||
                (String.IsNullOrEmpty(txtUnitPrice.Text) || txtUnitPrice.Text == "0") || String.IsNullOrEmpty(drpBrand.SelectedValue) || String.IsNullOrEmpty(drpProductGroup.SelectedValue) ||
                (String.IsNullOrEmpty(txtUnitQuantity.Text) || txtUnitQuantity.Text == "0")) 
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtProductName.Text))
                    strErr += "<li>" + "Product Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtProductAlias.Text))
                    strErr += "<li>" + "Product Alias is required." + "</li>";

                if (String.IsNullOrEmpty(txtUnitPrice.Text) || txtUnitPrice.Text == "0")
                    strErr += "<li>" + "Unit Price is required." + "</li>";

                if (String.IsNullOrEmpty(txtUnitQuantity.Text) || txtUnitQuantity.Text == "0")
                    strErr += "<li>" + "Unit Quantity is required (Atleast One)." + "</li>";

                if (drpBrand.SelectedValue == "0")
                    strErr += "<li>" + "Product Brand is required." + "</li>";

                if (drpProductGroup.SelectedValue == "0")
                    strErr += "<li>" + "Product Group is required." + "</li>";
            }
            // -------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Products objEntity = new Entity.Products();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.ProductName = txtProductName.Text;
                objEntity.ProductAlias = txtProductAlias.Text;
                objEntity.ProductSpecification = txtProductSpecification.Text;
                objEntity.ProductType = drpProductType.SelectedValue;
                objEntity.Unit = txtUnit.Text;
                objEntity.UnitPrice = (!String.IsNullOrEmpty(txtUnitPrice.Text)) ? Convert.ToDecimal(txtUnitPrice.Text) : 0;
                objEntity.TaxRate = (!String.IsNullOrEmpty(txtTaxRate.Text)) ? Convert.ToDecimal(txtTaxRate.Text) : 0;
                //objEntity.AddTaxPer = (!String.IsNullOrEmpty(txtAddTaxPer.Text)) ? Convert.ToDecimal(txtAddTaxPer.Text) : 0;
                objEntity.ProductGroupID = Convert.ToInt64(drpProductGroup.SelectedValue);
                objEntity.BrandID = Convert.ToInt64(drpBrand.SelectedValue);
                objEntity.UnitQuantity = Convert.ToInt64(txtUnitQuantity.Text);
                objEntity.UnitSize = txtUnitSize.Text;
                objEntity.UnitSurface = txtUnitSurface.Text;
                objEntity.LRDate = (!String.IsNullOrEmpty(txtLRDate.Text)) ? Convert.ToDateTime(txtLRDate.Text) : DateTime.Now;
                //objEntity.LRDate = (!String.IsNullOrEmpty(txtLRDate.Text)) ? Convert.ToDateTime(txtLRDate.Text) : Convert.ToDateTime("");
                objEntity.UnitGrade = drpUnitGrade.SelectedValue;
                objEntity.Box_Weight = (!String.IsNullOrEmpty(txtBox_Weight.Text)) ? Convert.ToDecimal(txtBox_Weight.Text) : 0;
                objEntity.Box_SQFT = (!String.IsNullOrEmpty(txtBox_SQFT.Text)) ? Convert.ToDecimal(txtBox_SQFT.Text) : 0;
                objEntity.Box_SQMT = (!String.IsNullOrEmpty(txtBox_SQMT.Text)) ? Convert.ToDecimal(txtBox_SQMT.Text) : 0;
                objEntity.ActiveFlag = chkActive.Checked;
                objEntity.TaxType = (!String.IsNullOrEmpty(drpTaxType.SelectedValue)) ? Convert.ToInt16(drpTaxType.SelectedValue) : 0;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                objEntity.ProductImage = imgProduct.ImageUrl;
                objEntity.HSNCode = txtHSNCode.Text;
                objEntity.Min_UnitPrice = (!String.IsNullOrEmpty(txtMinUnitPrice.Text)) ? Convert.ToDecimal(txtMinUnitPrice.Text) : 0;
                objEntity.Max_UnitPrice = (!String.IsNullOrEmpty(txtMaxUnitPrice.Text)) ? Convert.ToDecimal(txtMaxUnitPrice.Text) : 0;
                objEntity.ProfitRatio = (!String.IsNullOrEmpty(txtProfitRatio.Text)) ? Convert.ToDecimal(txtProfitRatio.Text) : 0;
                objEntity.MinQuantity = (!String.IsNullOrEmpty(txtMinQuantity.Text)) ? Convert.ToDecimal(txtMinQuantity.Text) : 0;
                objEntity.OpeningSTK = (!String.IsNullOrEmpty(txtOpeningSTK.Text)) ? Convert.ToDecimal(txtOpeningSTK.Text) : 0;
                objEntity.OpeningValuation = (!String.IsNullOrEmpty(txtOpeningValuation.Text)) ? Convert.ToDecimal(txtOpeningValuation.Text) : 0;
                objEntity.OpeningWeightRate = (!String.IsNullOrEmpty(txtOpeningWeightRate.Text)) ? Convert.ToDecimal(txtOpeningWeightRate.Text) : 0;
                //// -------------------------------------------------------------- Insert/Update Record
                BAL.ProductMgmt.AddUpdateProduct(objEntity, out ReturnCode, out ReturnMsg, out  ReturnProductId);
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    if(ReturnProductId>0)
                    {
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Images
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                        {
                            string rootFolderPath = Server.MapPath("productimages");
                            string filesToDelete = @"product-" + hdnpkID.Value.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            hdnpkID.Value = ReturnProductId.ToString();
                            //string filenm = @imgProduct.ImageUrl.Substring(imgProduct.ImageUrl.IndexOf('/') + 1);
                            //filenm = filenm.Replace("product-", "product-" + hdnpkID.Value.Trim());
                            foreach (string file in fileList)
                            {
                                string oldFileName = file;
                                string NewFileName = file.Replace(file.Substring(file.LastIndexOf(@"\") + 1, (file.LastIndexOf(".") - file.LastIndexOf(@"\") - 1)), "product-" + hdnpkID.Value.Trim());
                                System.IO.File.Copy(oldFileName, NewFileName);
                                //System.IO.File.Copy(file, file.Replace("product-", "product-" + hdnpkID.Value.Trim()));
                                System.IO.File.Delete(file);
                            }
                        }

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Documents
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        BAL.CommonMgmt.DeleteProductDocumentsByProductId(ReturnProductId, out ReturnCode1, out ReturnMsg1);

                        string filePath, filename1, ext, type;
                        Byte[] bytes;
                        long ProductId;
                        DataTable dtDocs = new DataTable();
                        dtDocs = (DataTable)Session["dtDocs"];

                        if (dtDocs != null)
                        {
                            foreach (DataRow dr in dtDocs.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    ProductId = ReturnProductId;
                                    filename1 = dr["FileName"].ToString();
                                    type = dr["Filetype"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.CommonMgmt.AddUpdateProductDocuments(ProductId, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                                    
                                }
                            }
                        }
                        if (ReturnCode1 > 0)
                        {
                            strErr += "<li>" + ReturnMsg1 + "</li>";
                            Session.Remove("dtDocs");
                        }

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Image Gallery
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        BAL.ProductMgmt.DeleteProductImageByProductID(ReturnProductId, out ReturnCode1, out ReturnMsg1);
                        DataTable dtImgGall = new DataTable();
                        dtImgGall = (DataTable)Session["dtImgGallery"];

                        if (dtImgGall != null)
                        {
                            foreach (DataRow dr in dtImgGall.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    Entity.ProductImages lstObj = new Entity.ProductImages();
                                    lstObj.pkID = 0;
                                    lstObj.ProductID = ReturnProductId;
                                    lstObj.Name = dr["Name"].ToString();
                                    lstObj.Type = dr["Type"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.ProductMgmt.AddUpdateProductImages(lstObj, out ReturnCode1, out ReturnMsg1);
                                    
                                    //Byte[] bytes1 = (Byte[])dr["ContentData"];
                                    
                                    String flname = dr["Name"].ToString();
                                    String tmpFile = Server.MapPath("productimages/") + flname;
                                    if (dr["ContentData"] != null && !String.IsNullOrEmpty(dr["ContentData"].ToString()))
                                        System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(dr["ContentData"].ToString()));
                                    else
                                        System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(ConvertImageToBase64(tmpFile)));

                                }
                            }
                        }
                        if (ReturnCode1 > 0)
                        {
                            strErr += "<li>" + ReturnMsg1 + "</li>";
                            Session.Remove("dtImgGallery");
                        }

                        // ***********************************************************
                        // Product Assembly
                        // ***********************************************************
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        BAL.ProductMgmt.DeleteProductDetailByFinishProductID(ReturnProductId, out ReturnCode1, out ReturnMsg1);

                        DataTable dtProductCard = new DataTable();
                        dtProductCard = (DataTable)Session["dtProductCard"];

                        if (dtProductCard != null)
                        {
                            foreach (DataRow dr in dtProductCard.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    Entity.ProductDetailCard objCard = new Entity.ProductDetailCard();
                                    objCard.FinishProductID = Convert.ToInt64(ReturnProductId);
                                    objCard.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                                    objCard.Unit = dr["Unit"].ToString();
                                    objCard.Quantity = Convert.ToDecimal(dr["Quantity"].ToString());
                                    objCard.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.ProductMgmt.AddUpdateProductDetail(objCard, out ReturnCode1, out ReturnMsg1);

                                }
                            }
                        }
                        if (ReturnCode1 > 0)
                        {
                            strErr += "<li>" + ReturnMsg1 + "</li>";
                            Session.Remove("dtProductCard");
                        }

                        // ***********************************************************
                        // Product Other Accessories
                        // ***********************************************************

                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        BAL.ProductMgmt.DeleteProductAccessoriesByFinishProductID(ReturnProductId, out ReturnCode1, out ReturnMsg1);

                        DataTable dtAccessories = new DataTable();
                        dtAccessories = (DataTable)Session["dtAccessories"];

                        if (dtAccessories != null)
                        {
                            foreach (DataRow dr in dtAccessories.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    Entity.ProductDetailCard objCard = new Entity.ProductDetailCard();
                                    objCard.FinishProductID = Convert.ToInt64(ReturnProductId);
                                    objCard.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                                    objCard.Unit = dr["Unit"].ToString();
                                    objCard.Quantity = Convert.ToDecimal(dr["Quantity"].ToString());
                                    objCard.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.ProductMgmt.AddUpdateProductAccessories(objCard, out ReturnCode1, out ReturnMsg1);

                                }
                            }
                        }
                        if (ReturnCode1 > 0)
                        {
                            strErr += "<li>" + ReturnMsg1 + "</li>";
                            Session.Remove("dtAccessories");
                        }

                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        //if (hdnQuatSpecRemark.Value.ToLower() == "yes")
                        //{
                        //}
                        //else
                        //{
                        btnSaveSpec_Click(null, null);
                            // >>>>>>>> First Delete all Selectd ProductSpecification entry from table
                            BAL.ProductSpecificationMgmt.DeleteProductSpecificationByProductID(ReturnProductId, out ReturnCode1, out ReturnMsg1);
                            if (ReturnCode1 > 0)
                            {
                                DataTable dtSpecs = new DataTable();
                                dtSpecs = (DataTable)Session["dtSpecs"];
                                if (dtSpecs != null)
                                {
                                    foreach (DataRow dr in dtSpecs.Rows)
                                    {
                                        if (dr.RowState.ToString() != "Deleted")
                                        {
                                            Entity.ProductSpecification lstObject = new Entity.ProductSpecification();
                                            lstObject.pkID = 0;
                                            lstObject.FinishProductID = ReturnProductId;
                                            lstObject.GroupHead = dr["GroupHead"].ToString();
                                            lstObject.MaterialHead = dr["MaterialHead"].ToString();
                                            lstObject.MaterialSpec = dr["MaterialSpec"].ToString();
                                            lstObject.MaterialRemarks = dr["MaterialRemarks"].ToString();
                                            lstObject.ItemOrder = dr["ItemOrder"].ToString();
                                            lstObject.LoginUserID = Session["LoginUserID"].ToString();
                                            // -------------------------------------------------------------- Insert/Update Record
                                            BAL.ProductSpecificationMgmt.AddUpdateProductSpecification(lstObject, out ReturnCode1, out ReturnMsg1);
                                            
                                        }
                                    }
                                }
                                if (ReturnCode1 > 0)
                                {
                                    strErr += "<li>" + ReturnMsg1 + "</li>";
                                    Session.Remove("dtSpecs");
                                }
                            }
                        //}
                    }
                }
            }
            // ------------------------------------------------------
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
            // --------------------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:reinitializePage();", true);
        }

        protected void btnDeleteImg_Click(object sender, EventArgs e)
        {

            string rootFolderPath = Server.MapPath("productimages");
            string filesToDelete = @"product-" + hdnpkID.Value.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            foreach (string file in fileList)
            {
                System.IO.File.Delete(file);
            }
            imgProduct.ImageUrl = "~/images/no-figure.png";
            FileUpload1.Dispose();
        }

        public void ClearAllField()
        {

            // ------------------------------------------------
            hdnpkID.Value = "";
            txtProductName.Text = "";
            txtProductAlias.Text = "";
            txtUnit.Text = "";
            txtUnitPrice.Text = "";
            txtTaxRate.Text = "0";
            //txtAddTaxPer.Text = "0";
            txtProductSpecification.Text = "";
            drpProductGroup.SelectedValue = "0";
            drpBrand.SelectedValue = "0";
            chkActive.Checked = true;
            drpTaxType.SelectedValue = "1";
            //uploadDocument.Enabled = false;
            //FileUpload1.Enabled = false;
            txtUnitQuantity.Text = "1";
            txtMinQuantity.Text = "1";
            txtUnitSize.Text = "";
            txtUnitSurface.Text = "";
            txtBox_Weight.Text = "";
            txtBox_SQFT.Text = "";
            txtBox_SQMT.Text = "";
            txtHSNCode.Text = "";
            txtMinUnitPrice.Text = "";
            txtMaxUnitPrice.Text = "";
            txtProfitRatio.Text = "";
            txtOpeningSTK.Text = "";
            txtInwardSTK.Text = "";
            txtOutwardSTK.Text = "";
            txtClosingSTK.Text = "";
            txtOpeningValuation.Text = "";
            txtOpeningWeightRate.Text = "";
            txtLRDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value == "0")
            {
                liBrochure.Visible = false;
                liProdImages.Visible = false;
                proddocs.Style.Add("display", "none");
                prodimg.Style.Add("display", "none");
            }
            // --------------------------------------------------------------
            BindProductDocuments(0);
            // --------------------------------------------------------------
            BindProductGallery(0);
            // --------------------------------------------------------------
            BindProductDetail();
            // --------------------------------------------------------------
            BindProductAccessories();
            // --------------------------------------------------------------
            if (String.IsNullOrEmpty(hdnApplicationIndustry.Value.ToLower()) || hdnApplicationIndustry.Value.ToLower() == "no")
            {
                dvCeraBox.Visible = false;
                dvCeraSize.Visible = false;
                dvCeraSurface.Visible = false;
            }
            btnSave.Disabled = false;
            //Session.Remove("dtDocs");
            //Session.Remove("dtProductCard");
            //Session.Remove("dtAccessories");
            //Session.Remove("dtSpecs");
            BindProductSpecs();
            txtProductName.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProduct(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProductMgmt.DeleteProduct(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);
            return serializer.Serialize(rows);
        }

        protected void rptProdDocs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // https://www.aspsnippets.com/Articles/Upload-and-Download-files-from-Folder-Directory-in-ASPNet-using-C-and-VBNet.aspx

            //List<Entity.Documents> lstFiles = new List<Entity.Documents>();
            //lstFiles = BAL.CommonMgmt.GetDocumentsList(Convert.ToInt64(e.CommandArgument.ToString()), 0);
            // -------------------------------------------------------
            //if (e.CommandName.ToString() == "Download")
            //{
            //    if (lstFiles.Count > 0)
            //    {
            //        for (int i = 0; i <= (lstFiles.Count - 1); i++)
            //        {
            //            if (!String.IsNullOrEmpty(e.CommandArgument.ToString()))
            //            {
            //                string filePath = "";
            //                filePath = "productimages/" + lstFiles[i].FileName.ToString();
            //                ScriptManager.RegisterStartupScript(this, typeof(string), "opPdf", "javascript:ShowPDFfile('" + filePath + "');", true);
            //            }
            //        }
            //    }
            //}
            //if(lstFiles.Count>0)
            //{
            //    if (e.CommandName.ToString() == "Delete")
            //    {
            //        int ReturnCode = 0;
            //        string ReturnMsg = "";
            //        // -------------------------------------------------------------- Delete Record
            //        BAL.CommonMgmt.DeleteProductDocuments(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
            //        ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
            //        if (!String.IsNullOrEmpty(hdnpkID.Value))
            //        {
            //            BindProductDocuments(Convert.ToInt64(hdnpkID.Value));
            //        }
            //        // -------------------------------------------------------------
            //        //foreach (Entity.Documents tmpObj in lstFiles)
            //        //{
            //        //if (tmpObj.pkID == Convert.ToInt64(e.CommandArgument.ToString()))
            //        //{
            //        string rootFolderPath = Server.MapPath("productimages");
            //        string filesToDelete = @lstFiles[0].FileName;
            //        string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            //        foreach (string file in fileList)
            //        {
            //            System.IO.File.Delete(file);
            //        }
            //        frameDoc.Attributes.Remove("scr");
            //        frameDoc.Attributes.Add("src", "images/buttons/Preview.png");
            //        //}
            //        //}
            //    }
            //    if (e.CommandName.ToString() == "Preview")
            //    {
            //        if (lstFiles.Count > 0)
            //        {
            //            string filePath = "productimages/" + lstFiles[0].FileName;
            //            frameDoc.Attributes.Add("src", filePath);
            //        }
            //    }
            //}
            //else
            //{
                if (e.CommandName.ToString() == "Delete")
                {
                    DataTable dtDocs = (DataTable)Session["dtDocs"];
                    for (int i = dtDocs.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dtDocs.Rows[i];
                        if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                            dr.Delete();
                    }
                    dtDocs.AcceptChanges();
                    Session.Add("dtDocs", dtDocs);

                    //frameDoc.Attributes.Remove("scr");
                    //frameDoc.Attributes.Add("src", "images/buttons/Preview.png");

                    rptProdDocs.DataSource = dtDocs;
                    rptProdDocs.DataBind();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
                }
                //if (e.CommandName.ToString() == "Preview")
                //{
                //    DataTable dtDocs = (DataTable)Session["dtDocs"];
                //    for (int i = dtDocs.Rows.Count - 1; i >= 0; i--)
                //    {
                //        DataRow dr = dtDocs.Rows[i];
                //        if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                //        {
                //            string filePath = "productimages/" + dr["FileName"];
                //            frameDoc.Attributes.Add("src", filePath);
                //        }
                //    }
                //}
                //}
            
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Product Assembly Detail Card
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public void BindProductDetail() 
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
            if (!String.IsNullOrEmpty(hdnpkID.Value))
            {
                lst = BAL.ProductMgmt.GetProductDetailList(0, Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString());
            }
            else
            {
                lst = BAL.ProductMgmt.GetProductDetailList(0, 0, Session["LoginUserID"].ToString());
            }
            // -----------------------------------------
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptProductDetail.DataSource = dtDetail1;
            rptProductDetail.DataBind();
            Session.Add("dtProductCard", dtDetail1);   
        }

        protected void rptProductDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "DeleteDetail")
            {
                DataTable dtProductCard = new DataTable();
                dtProductCard = (DataTable)Session["dtProductCard"];
                // --------------------------------- Delete Record
                string iname = e.CommandArgument.ToString();

                foreach (DataRow dr in dtProductCard.Rows)
                {
                    if (dr["ProductID"].ToString() == iname)
                    {
                        dtProductCard.Rows.Remove(dr);
                        break;
                    }
                }
                dtProductCard.AcceptChanges();
                rptProductDetail.DataSource = dtProductCard;
                rptProductDetail.DataBind();

                Session.Add("dtProductCard", dtProductCard);
            }
        }
        
        // Product Assembly
        protected void imgBtnSaveDetail_Click(object sender, ImageClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnSubProductID.Value) && hdnSubProductID.Value != "0")
            {
                DataTable dtProductCard = new DataTable();
                dtProductCard = (DataTable)Session["dtProductCard"];

                string find = "ProductID = " + hdnSubProductID.Value;
                DataRow[] foundRows = dtProductCard.Select(find);
                if (foundRows.Length > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                    return;
                }

                Int64 cntRow = dtProductCard.Rows.Count + 1;
                DataRow dr = dtProductCard.NewRow();
                dr["FinishProductID"] = (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0") ? Convert.ToInt64(hdnpkID.Value) : 0;
                dr["ProductID"] = (!String.IsNullOrEmpty(hdnSubProductID.Value) && hdnSubProductID.Value != "0") ? Convert.ToInt64(hdnSubProductID.Value) : 0; 
                dr["ProductName"] = newProductName.Text;
                dr["Unit"] = newUnit.Text;
                dr["Quantity"] = Convert.ToDecimal(newQuantity.Text);
                dtProductCard.Rows.Add(dr);
                dtProductCard.AcceptChanges();
                Session.Add("dtProductCard", dtProductCard);
                // -----------------------------------------------------
                rptProductDetail.DataSource = dtProductCard;
                rptProductDetail.DataBind();
                // -----------------------------------------------------
                hdnSubProductID.Value = String.Empty;
                newProductName.Text = String.Empty;
                newUnit.Text = String.Empty;
                newQuantity.Text = String.Empty;

                newProductName.Focus();
            }
        }

        protected void chkActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Product Specification : New Row
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        protected void btnSaveSpecAdd_Click(object sender, EventArgs e)
        {
            DataTable dtSpecs = new DataTable();
            if (Session["dtSpecs"] != null)
            {
                dtSpecs = (DataTable)Session["dtSpecs"];
                DataRow dr = dtSpecs.NewRow();
                dr["FinishProductID"] = ((!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0") ? hdnpkID.Value : "0");
                dr["itemOrder"] = newOrder1.Text;
                dr["GroupHead"] = newGroupHead1.Text;
                dr["MaterialHead"] = newMaterialHead1.Text;
                dr["MaterialSpec"] = newMaterialSpec1.Text;
                dr["MaterialRemarks"] = newMaterialRemarks1.Text;
                dtSpecs.Rows.Add(dr);
                dtSpecs.AcceptChanges();
                Session.Add("dtSpecs", dtSpecs);
                rptProductSpecs.DataSource = dtSpecs;
                rptProductSpecs.DataBind();
                //divErrorMsgSpec.InnerHtml = " <center> Specification Added Successfuly </br> <b> Note : Don't forget to 'Save' From Main Screen.</b> </center>";
                // -----------------------------------------------------------
                newOrder1.Text = "";
                newGroupHead1.Text = "";
                newMaterialHead1.Text = "";
                newMaterialSpec1.Text = "";
                newMaterialRemarks1.Text = "";
            }

        }

        protected void btnSaveSpec_Click(object sender, EventArgs e)
        {
            DataTable dtSpecs = new DataTable();

            if (Session["dtSpecs"] != null)
            {
                dtSpecs = (DataTable)Session["dtSpecs"];
                dtSpecs.Rows.Clear();
                dtSpecs.AcceptChanges();
                divspecmsg.InnerHtml = " <center> Specification Added Successfuly </br> <b> Note : Don't forget to 'Save' From Main Screen.</b> </center>";
            }
            // ------------------------------------------------------    
            int tt = -1;
            foreach (RepeaterItem item in rptProductSpecs.Items)
            {

                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    String ctrOrder = ((TextBox)item.FindControl("newOrder")).Text;
                    String ctrGroupHead = ((TextBox)item.FindControl("newGroupHead")).Text;
                    String ctrMaterialHead = ((TextBox)item.FindControl("newMaterialHead")).Text;
                    String ctrMaterialSpec = ((TextBox)item.FindControl("newMaterialSpec")).Text;
                    String ctrMaterialRemarks = ((TextBox)item.FindControl("newMaterialRemarks")).Text;
                    // --------------------------------------------------------
                    DataRow dr = dtSpecs.NewRow();
                    dr["itemOrder"] = ctrOrder;
                    dr["FinishProductID"] = ((!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0") ?hdnpkID.Value:"0");
                    dr["GroupHead"] = ctrGroupHead;
                    dr["MaterialHead"] = ctrMaterialHead;
                    dr["MaterialSpec"] = ctrMaterialSpec;
                    dr["MaterialRemarks"] = ctrMaterialRemarks;
                    dtSpecs.Rows.Add(dr);
                }
            }
            Session.Add("dtSpecs", dtSpecs);
        }
        
        public void BindProductSpecs()
        {
            DataTable dtSpecs = new DataTable();
            List<Entity.ProductSpecification> lst = new List<Entity.ProductSpecification>();
            if (!String.IsNullOrEmpty(hdnpkID.Value) || hdnpkID.Value != "")
            {
                lst = BAL.ProductSpecificationMgmt.GetProductSpecificationList(Convert.ToInt64(hdnpkID.Value));
                if (lst.Count == 0)
                {
                    lst = BAL.ProductSpecificationMgmt.GetProductSpecificationList(0);
                }
            }
            else
                lst = BAL.ProductSpecificationMgmt.GetProductSpecificationList(0);
            // -----------------------------------------
            dtSpecs = PageBase.ConvertListToDataTable(lst);
          
            rptProductSpecs.DataSource = dtSpecs;
            rptProductSpecs.DataBind();
          
            //if (Session["dtSpecs"] == null)
            //{
                Session["dtSpecs"] = dtSpecs;
            //}
        }

        protected void rptProductSpecs_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtGroupHead = ((TextBox)e.Item.FindControl("newGroupHead"));
                TextBox txtMaterialHead = ((TextBox)e.Item.FindControl("newMaterialHead"));
                TextBox txtMaterialSpec = ((TextBox)e.Item.FindControl("newMaterialSpec"));
                TextBox txtOrder = ((TextBox)e.Item.FindControl("newOrder"));
            }
        }

        protected void txtOpeningSTK_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtOpeningSTK.Text))
            {
                decimal opstk = 0, instk = 0, outstk = 0;
                opstk = !String.IsNullOrEmpty(txtOpeningSTK.Text) ? Convert.ToDecimal(txtOpeningSTK.Text) : 0;
                instk = !String.IsNullOrEmpty(txtInwardSTK.Text) ? Convert.ToDecimal(txtInwardSTK.Text) : 0;
                outstk = !String.IsNullOrEmpty(txtOutwardSTK.Text) ? Convert.ToDecimal(txtOutwardSTK.Text) : 0;
                // --------------------------------------------------------------
                txtClosingSTK.Text = ((opstk + instk) - outstk).ToString("0.00");
            }
        }

        protected void rptProductSpecs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    DataTable dtSpecs = new DataTable();
                    dtSpecs = (DataTable)Session["dtSpecs"];

                    DataRow[] rows;
                    rows = dtSpecs.Select("pkID=" + e.CommandArgument.ToString());
                    foreach (DataRow r in rows)
                        r.Delete();

                    rptProductSpecs.DataSource = dtSpecs;
                    rptProductSpecs.DataBind();

                    Session.Add("dtSpecs", dtSpecs);
                }
            }
        }

        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Product Accessories
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public void BindProductAccessories()
        {
            DataTable dtAccessories = new DataTable();

            List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
            if (!String.IsNullOrEmpty(hdnpkID.Value))
                lst = BAL.ProductMgmt.GetProductAccessoriesList(0, Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString());
            else
                lst = BAL.ProductMgmt.GetProductAccessoriesList(0, 0, Session["LoginUserID"].ToString());

            dtAccessories = PageBase.ConvertListToDataTable(lst);
            Session.Add("dtAccessories", dtAccessories);
            // -----------------------------------------
            rptAccessories.DataSource = dtAccessories;
            rptAccessories.DataBind();
            
        }
        protected void rptAccessories_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "DeleteDetail")
            {
                DataTable dtAccessories = new DataTable();
                dtAccessories = (DataTable)Session["dtAccessories"];
                // --------------------------------- Delete Record
                string iname = e.CommandArgument.ToString();

                foreach (DataRow dr in dtAccessories.Rows)
                {
                    if (dr["ProductID"].ToString() == iname)
                    {
                        dtAccessories.Rows.Remove(dr);
                        break;
                    }
                }
                dtAccessories.AcceptChanges();
                rptAccessories.DataSource = dtAccessories;
                rptAccessories.DataBind();

                Session.Add("dtAccessories", dtAccessories);
            }
        }

        protected void imgBtnSaveDetailAcc_Click(object sender, ImageClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnAccProductID.Value) && hdnAccProductID.Value != "0")
            {
                DataTable dtAccessories = new DataTable();
                dtAccessories = (DataTable)Session["dtAccessories"];
                Int64 cntRow = dtAccessories.Rows.Count + 1;
                DataRow dr = dtAccessories.NewRow();
                dr["FinishProductID"] = (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0") ? Convert.ToInt64(hdnpkID.Value) : 0;
                dr["ProductID"] = (!String.IsNullOrEmpty(hdnAccProductID.Value) && hdnAccProductID.Value != "0") ? Convert.ToInt64(hdnAccProductID.Value) : 0;
                dr["ProductName"] = accProductName.Text;
                //dr["Unit"] = accUnit.Text;
                dr["Quantity"] = Convert.ToDecimal(accQuantity.Text);
                dtAccessories.Rows.Add(dr);
                dtAccessories.AcceptChanges();
                Session.Add("dtAccessories", dtAccessories);
                // -----------------------------------------------------
                rptAccessories.DataSource = dtAccessories;
                rptAccessories.DataBind();
                // -----------------------------------------------------
                hdnAccProductID.Value = String.Empty;
                accProductName.Text = String.Empty;
                accQuantity.Text = String.Empty;

                accProductName.Focus();
            }
        }

        protected void newProductName_TextChanged(object sender, EventArgs e)
        {
            int tot = 0;
            List<Entity.Products> lstPro = new List<Entity.Products>();
            lstPro = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnSubProductID.Value), Session["LoginUserID"].ToString(), 1, 1111, out tot);
            if (lstPro.Count > 0)
                newUnit.Text = lstPro[0].Unit;
        }
    }
}