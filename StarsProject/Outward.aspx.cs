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
using System.Globalization;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;

namespace StarsProject
{
    public partial class Outward : System.Web.UI.Page
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

                Session.Remove("dtDetail");
                Session.Remove("mySpecs");

                BindDropDown();
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
                hdnSerialKey.Value = Session["SerialKey"].ToString().Replace("\r\n", "");
                // --------------------------------------------------------
                string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

                if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V") // it will show all reference related textbox for only PRI
                {
                    divReference.Visible = true;
                }
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
            txtOutwardNo.ReadOnly = true;
            txtOutwardDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;

            txtExpRef.ReadOnly = true;
            txtBuyRef.ReadOnly = true;
            txtOrderDate.ReadOnly = true;
            txtOtherRef.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
            drpLocation.Attributes.Add("disabled", "disabled");

            txtPreCarrBy.ReadOnly = true;
            txtPreCarrRecPlace.ReadOnly = true;
            txtFlightNo.ReadOnly = true;
            txtPortOfLoading.ReadOnly = true;
            txtPortOfDispatch.ReadOnly = true;
            txtPortOfDestination.ReadOnly = true;
            txtContainerNo.ReadOnly = true;
            txtPackages.ReadOnly = true;
            txtPackageType.ReadOnly = true;
            txtNetWeight.ReadOnly = true;
            txtGrossWeight.ReadOnly = true;
            txtFOB.ReadOnly = true;
        }
        public void BindDropDown()
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
                            strErr += "<li>" + "Select Proper Product Name From List." + "</li>";

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

                            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

                            if (tmpSerialKey != "PRI9-DG8H-G6GF-TP5V") // it will not check for duplicate item for PerfectRoto
                            {
                                //----Check For Duplicate Item----//
                                string find = "ProductID = " + ((HiddenField)e.Item.FindControl("hdnProductID")).Value;

                                DropDownList fld2 = ((DropDownList)e.Item.FindControl("drpForOrderNo"));
                                if (!String.IsNullOrEmpty(fld2.SelectedValue))
                                    find += " And OrderNo = '" + ((DropDownList)e.Item.FindControl("drpForOrderNo")).SelectedValue + "'";

                                DataRow[] foundRows = dtDetail.Select(find);
                                if (foundRows.Length > 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                                    return;
                                }
                            }

                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = 0;
                            //string icode = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedValue;
                            //string iname = ((DropDownList)e.Item.FindControl("drpProduct")).SelectedItem.Text;
                            string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                            string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                            string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                            string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                            string qtyweight = ((TextBox)e.Item.FindControl("txtQuantityWeight")).Text;
                            string serialno = ((TextBox)e.Item.FindControl("txtSerialNo")).Text;
                            string boxno = ((TextBox)e.Item.FindControl("txtBoxNo")).Text;
                            string fororderno = ((DropDownList)e.Item.FindControl("drpForOrderNo")).SelectedValue;
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

                            //dr["OutwardNo"] = txtOutwardNo.Text;
                            //dr["BundleID"] = 0;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dr["QuantityWeight"] = (!String.IsNullOrEmpty(qtyweight)) ? Convert.ToDecimal(qtyweight) : 0;
                            dr["SerialNo"] = (!String.IsNullOrEmpty(serialno)) ? serialno : "";
                            dr["BoxNo"] = (!String.IsNullOrEmpty(boxno)) ? boxno : "";
                            dr["OrderNo"] = (!String.IsNullOrEmpty(fororderno)) ? fororderno : "";
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

        protected void rptOutwardDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

            if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V") // Label Change and hiding column for Perfect
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    HtmlTableCell HTSerialNo = (HtmlTableCell)e.Item.FindControl("HTSerialNo");
                    HtmlTableCell HTBoxNo = (HtmlTableCell)e.Item.FindControl("HTBoxNo");
                    HtmlTableCell HTWeight = (HtmlTableCell)e.Item.FindControl("HTWeight");

                    HTWeight.Attributes.Add("style", "display:none;");
                    HTSerialNo.InnerText = "Box Calculation";
                    HTBoxNo.InnerText = "Box Nos.";
                }
                else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlTableCell ITWeight = (HtmlTableCell)e.Item.FindControl("ITWeight");

                    ITWeight.Attributes.Add("style", "display:none;");
                }
                else if (e.Item.ItemType == ListItemType.Footer)
                {
                    HtmlTableCell FTWeight = (HtmlTableCell)e.Item.FindControl("FTWeight");

                    FTWeight.Attributes.Add("style", "display:none;");
                }
            }

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
                drpLocation.SelectedValue = lstEntity[0].LocationID.ToString();
                //------------------------------------
                txtExpRef.Text = lstEntity[0].ExporterRef;
                txtBuyRef.Text = lstEntity[0].SupOrderRef;
                txtOrderDate.Text = lstEntity[0].SupOrderDate.ToString("yyyy-MM-dd");
                txtOtherRef.Text = lstEntity[0].OtherRef;
                //------------------------------------
                drpModeOfTransport.SelectedValue = lstEntity[0].ModeOfTransport;
                txtVehicleNo.Text = lstEntity[0].VehicleNo;
                txtTransporterName.Text = lstEntity[0].TransporterName;
                txtLRNo.Text = lstEntity[0].LRNo;
                txtLRDate.Text = (lstEntity[0].LRDate != null) ? lstEntity[0].LRDate.Value.ToString("yyyy-MM-dd") : null;
                txtDCNo.Text = lstEntity[0].DCNo;
                txtDCDate.Text = (lstEntity[0].DCDate != null) ? lstEntity[0].DCDate.Value.ToString("yyyy-MM-dd") : null;
                txtDeliveryNote.Text = lstEntity[0].DeliveryNote;
                txtRemarks.Text = lstEntity[0].Remarks;

                txtManualOutwardNo.Text = lstEntity[0].ManualOutwardNo;

                //------------------------------------------------------------------------------
                List<Entity.Outward> lstExport = new List<Entity.Outward>();
                lstExport = BAL.OutwardMgmt.GetOutwardExportList(Convert.ToInt64(hdnpkID.Value), txtOutwardNo.Text, Session["LoginUserID"].ToString());
                if (lstExport.Count > 0)
                {
                    txtPreCarrBy.Text = !String.IsNullOrEmpty(lstExport[0].PreCarrBy) ? lstExport[0].PreCarrBy.ToString() : "";
                    txtPreCarrRecPlace.Text = !String.IsNullOrEmpty(lstExport[0].PreCarrRecPlace) ? lstExport[0].PreCarrRecPlace.ToString() : "";
                    txtFlightNo.Text = !String.IsNullOrEmpty(lstExport[0].FlightNo) ? lstExport[0].FlightNo.ToString() : "";
                    txtPortOfLoading.Text = !String.IsNullOrEmpty(lstExport[0].PortOfLoading) ? lstExport[0].PortOfLoading.ToString() : "";
                    txtPortOfDispatch.Text = !String.IsNullOrEmpty(lstExport[0].PortOfDispatch) ? lstExport[0].PortOfDispatch.ToString() : "";
                    txtPortOfDestination.Text = !String.IsNullOrEmpty(lstExport[0].PortOfDestination) ? lstExport[0].PortOfDestination.ToString() : "";
                    txtContainerNo.Text = !String.IsNullOrEmpty(lstExport[0].MarksNo) ? lstExport[0].MarksNo.ToString() : "";
                    txtPackages.Text = !String.IsNullOrEmpty(lstExport[0].Packages) ? lstExport[0].Packages.ToString() : "";
                    txtPackageType.Text = !String.IsNullOrEmpty(lstExport[0].PackageType) ? lstExport[0].PackageType.ToString() : "";
                    txtNetWeight.Text = !String.IsNullOrEmpty(lstExport[0].NetWeight) ? lstExport[0].NetWeight.ToString() : "";
                    txtGrossWeight.Text = !String.IsNullOrEmpty(lstExport[0].GrossWeight) ? lstExport[0].GrossWeight.ToString() : "";
                    txtFOB.Text = !String.IsNullOrEmpty(lstExport[0].FreeOnBoard) ? lstExport[0].FreeOnBoard.ToString() : "";
                }
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    BindSalesOrderList(Convert.ToInt64(hdnCustomerID.Value));
                    drpReferenceNo.Items.FindByValue(lstEntity[0].OrderNo).Selected = true;
                }
                BindOutwardDetailList(lstEntity[0].OutwardNo);

            }
            // ----------------------------------------------------------------------
            txtCustomerName.Enabled = (pMode.ToLower() == "add") ? true : false;
            txtOutwardDate.Focus();
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


            if (String.IsNullOrEmpty(txtCustomerName.Text) || Convert.ToInt64(hdnCustomerID.Value) == 0 || String.IsNullOrEmpty(txtOutwardDate.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtOutwardDate.Text))
                    strErr += "<li>" + "Outward Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text) || Convert.ToInt64(hdnCustomerID.Value) == 0)
                    strErr += "<li>" + "Customer Selection is required." + "</li>";
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtOutwardDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtOutwardDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Outward Date is Not Valid." + "</li>";
                }
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
                        Int64 intLocation = (!String.IsNullOrEmpty(drpLocation.SelectedValue)) ? Convert.ToInt64(drpLocation.SelectedValue) : 0;
                        objEntity.OutwardNo = txtOutwardNo.Text;
                        objEntity.OutwardDate = Convert.ToDateTime(txtOutwardDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.LocationID = intLocation;
                        objEntity.OrderNo = drpReferenceNo.SelectedValue != "" ? drpReferenceNo.SelectedValue : "";
                        objEntity.ExporterRef = txtExpRef.Text;
                        objEntity.SupOrderRef = txtBuyRef.Text;
                        if (!String.IsNullOrEmpty(txtOrderDate.Text))
                        {
                            if (Convert.ToDateTime(txtOrderDate.Text).Year > 1900)
                                objEntity.SupOrderDate = Convert.ToDateTime(txtOrderDate.Text);
                        }
                        //objEntity.SupOrderDate = Convert.ToDateTime(txtOrderDate.Text);
                        objEntity.OtherRef = txtOtherRef.Text;
                        objEntity.ModeOfTransport = drpModeOfTransport.SelectedValue;
                        objEntity.TransporterName = txtTransporterName.Text;
                        objEntity.VehicleNo = txtVehicleNo.Text;
                        objEntity.LRNo = txtLRNo.Text;
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
                        objEntity.DeliveryNote = txtDeliveryNote.Text;
                        objEntity.Remarks = txtRemarks.Text;

                        objEntity.ManualOutwardNo = txtManualOutwardNo.Text;

                        objEntity.LoginUserID = Session["LoginUserID"].ToString();

                        objEntity.OutwardNo = txtOutwardNo.Text;
                        objEntity.PreCarrBy = txtPreCarrBy.Text;
                        objEntity.PreCarrRecPlace = txtPreCarrRecPlace.Text;
                        objEntity.FlightNo = txtFlightNo.Text;
                        objEntity.PortOfLoading = txtPortOfLoading.Text;
                        objEntity.PortOfDispatch = txtPortOfDispatch.Text;
                        objEntity.PortOfDestination = txtPortOfDestination.Text;
                        objEntity.MarksNo = txtContainerNo.Text;
                        objEntity.Packages = txtPackages.Text;
                        objEntity.PackageType = txtPackageType.Text;
                        objEntity.NetWeight = txtNetWeight.Text;
                        objEntity.GrossWeight = txtGrossWeight.Text;
                        objEntity.FreeOnBoard = txtFOB.Text;

                        BAL.OutwardMgmt.AddUpdateOutwardExport(objEntity, out ReturnCode1, out ReturnMsg1);
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

                            Entity.OutwardDetail objQuotDet = new Entity.OutwardDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.OutwardNo = txtOutwardNo.Text;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.LocationID = intLocation;
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.QuantityWeight = Convert.ToDecimal(dr["QuantityWeight"]);
                                objQuotDet.SerialNo = dr["SerialNo"].ToString();
                                objQuotDet.BoxNo = dr["BoxNo"].ToString();
                                objQuotDet.OrderNo = dr["OrderNo"].ToString();
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
                            // -------------------------------------------------------
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                                Session.Remove("mySpecs");
                            }
                            // -------------------------------------------------------
                            // Fetching "QuotationNo" using pkID ...
                            Int64 pkID = BAL.CommonMgmt.GetOutwardNoPrimaryID(ReturnOutwardNo);

                            GenerateOutward(pkID);

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
            Session.Remove("dtDetail");
            Session.Remove("mySpecs");

            hdnpkID.Value = "";
            txtOutwardNo.Text = ""; //  BAL.CommonMgmt.fnGetOutwardNoByDate(DateTime.Today.ToString("yyyy-MM-dd"));
            txtOutwardDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";

            txtExpRef.Text = "";
            txtBuyRef.Text = "";
            txtOrderDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtOtherRef.Text = "";

            drpModeOfTransport.SelectedValue = "";
            txtVehicleNo.Text = "";
            txtTransporterName.Text = "";
            txtLRNo.Text = "";
            txtLRDate.Text = "";
            txtDCNo.Text = "";
            txtDCDate.Text = "";
            txtDeliveryNote.Text = "";
            txtRemarks.Text = "";
            BindOutwardDetailList("");
            btnSave.Disabled = false;

            txtManualOutwardNo.Text = "";

            txtPreCarrBy.Text = "";
            txtPreCarrRecPlace.Text = "";
            txtFlightNo.Text = "";
            txtPortOfLoading.Text = "";
            txtPortOfDispatch.Text = "";
            txtPortOfDestination.Text = "";

            txtOutwardDate.Focus();
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
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");
            TextBox edQuantityWeight = (TextBox)item.FindControl("edQuantityWeight");
            TextBox edSerialNo = (TextBox)item.FindControl("edSerialNo");
            TextBox edBoxNo = (TextBox)item.FindControl("edBoxNo");
            TextBox edForOrderNo = (TextBox)item.FindControl("edForOrderNo");
            
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
                if (row["ProductID"].ToString() == edProductID.Value && row["OrderNo"].ToString() == edForOrderNo.Text)
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

            // -----------------------------------------------------
            // Binding Customer's Purchase Order
            // -----------------------------------------------------
            DropDownList drpForOrderNo = (DropDownList)rptFootCtrl.FindControl("drpForOrderNo");
            drpForOrderNo.Items.Clear();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && !String.IsNullOrEmpty(hdnProductID.Value))
            {
                //drpForOrderNo.DataSource = BAL.InwardMgmt.GetInwardListByCustomer(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), Session["LoginUserID"].ToString(), 0, 0);
                drpForOrderNo.DataSource = BAL.SalesOrderMgmt.GetSalesOrderListByCustomerProduct(Convert.ToInt64(hdnCustomerID.Value), Convert.ToInt64(hdnProductID.Value), "", Session["LoginUserID"].ToString());
                drpForOrderNo.DataTextField = "OrderNo";
                drpForOrderNo.DataValueField = "OrderNo";
                drpForOrderNo.DataBind();
                drpForOrderNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- SO # --", ""));
            }
            // -----------------------------------------------------
            txtQuantity.Focus();

        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                BindSalesOrderList(Convert.ToInt64(hdnCustomerID.Value));
            else
                strErr += "<li>" + "Select Proper Customer From List." + "</li>";
            drpReferenceNo.Focus();
        }

        public void BindSalesOrderList(Int64 pCustomerID)
        {
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            //drpReferenceNo.Items.Clear();
            if (pCustomerID != 0)
            {
                if (hdnSerialKey.Value == "ECO3-2G21-TECH-3MRT")    // For Stainex
                    lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", pCustomerID, "", 0, 0);
                else if(hdnSerialKey.Value == "DYNA-2GF3-J7G8-FF12") // For Dynamic
                    lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(pCustomerID);
                else
                    lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", pCustomerID, "", 0, 0, false);

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

            if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
            {
                dtDetail.Clear();
                String Assembly = BAL.CommonMgmt.GetConstant("GetSOWiseAssembly", 0, 1);
                if (Assembly.ToLower() == "yes" || Assembly.ToLower() == "y")
                    dtDetail = BAL.SalesOrderMgmt.GetSalesOrderAssembly(drpReferenceNo.SelectedValue);
                else
                    dtDetail = BAL.SalesOrderMgmt.GetSalesOrderDetailForSale(drpReferenceNo.SelectedValue);
            }

            Session.Add("dtDetail", dtDetail);
            //dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetail"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
            //Session.Add("dtDetail", dtDetail);

            rptOutwardDetail.DataSource = dtDetail;
            rptOutwardDetail.DataBind();
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

            //tmpSerialKey = "COL1-AKL9-TEC9-SJ99";

            if (tmpSerialKey == "SI08-SB94-MY45-RY15")          // Sharvaya Infotech
            {
                GenerateOutward_Sharvaya(pkID);
            }
            else if (tmpSerialKey == "4JM1-E874-JBK0-5HAN")      //  Shree Balaji - SBR
            {
                GenerateOutward_Balaji(pkID);
            }
            else if (tmpSerialKey == "8YWQ-DDRO-V98V-LDN2")     // FIELDMASTER Innovation Limited
            {
                GenerateOutward_FieldMaster(pkID);
            }
            else if (tmpSerialKey == "J63H-F8LX-B4B2-GYVZ")      //  HI-TECH
            {
                GenerateOutward_Hitech(pkID);
            }
            else if (tmpSerialKey == "COL1-AKL9-TEC9-SJ99")  // Coldtech
            {
                GenerateOutward_ColdtechRound(pkID);
            }
            else if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")     // PerfectRoto 
                GenerateOutward_PRI(pkID);
            else if (tmpSerialKey == "jalaram")     // PerfectRoto 
                GenerateOutward_Jalaram(pkID);
            else

            {
                GenerateOutward_Sharvaya(pkID);
            }

        }

        public static void GenerateOutward_Coldtech(Int64 pkID)
        {
            //HttpContext.Current.Session["PrintHeader"] = "no";
            HttpContext.Current.Session["printModule"] = "outward";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(2);
            PdfPTable tblDetail = new PdfPTable(5);

            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            if (lstQuot.Count > 0)
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);

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
                int[] column_tblMember = { 70, 30 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;


                string CustomerAdd = lstCust[0].Address + " , " + lstCust[0].Area + " \n" + lstCust[0].CityName + " - " + lstCust[0].Pincode + " , " + lstCust[0].StateName + " , " + lstCust[0].CountryName;

                PdfPTable tblCustomerD = new PdfPTable(2);
                int[] column_tblCustomerD = { 6, 94 };
                tblCustomerD.SetWidths(column_tblCustomerD);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblCustomerD.AddCell(pdf.setCell(CustomerAdd, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                PdfPTable tblOutwardD = new PdfPTable(2);
                int[] column_tblOutwardD = { 20, 80 };
                tblOutwardD.SetWidths(column_tblOutwardD);

                tblOutwardD.AddCell(pdf.setCell("No", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardDate.ToString("dd-MM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));



                //---------------------------------------------------------

                //--------------------------------------------------------------
                tblMember.AddCell(pdf.setCellRoundBorder(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblMember.AddCell(pdf.setCellRoundBorder(tblOutwardD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                //tblMember.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 7, 49, 14, 25, 5 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                PdfPTable pdfMerge = new PdfPTable(3);
                int[] column_pdfMerge = { 10, 70, 20 };
                pdfMerge.SetWidths(column_pdfMerge);



                decimal totAmount = 0, taxAmount = 0, netAmount = 0, quantity = 0;

                int totalRowCount = 0, totalSpecLines = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    decimal q = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    quantity += Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());

                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    pdfMerge.AddCell(pdf.setCellRoundBorder((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    pdfMerge.AddCell(pdf.setCellRoundBorder(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                    pdfMerge.AddCell(pdf.setCellRoundBorder(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCellRoundBorder(dtItem.Rows[i]["ProductSpecification"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));

                }

                //int lines = (int)(ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount));
                //pdfMerge.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0)).Rowspan = dtItem.Rows.Count;
                //pdfMerge.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0)).Rowspan = dtItem.Rows.Count;
                //pdfMerge.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0)).Rowspan = dtItem.Rows.Count;


                PdfPTable pdfextra = new PdfPTable(2);
                int[] column_pdfextra = { 70, 30 };
                pdfextra.SetWidths(column_pdfextra);
                pdfextra.AddCell(setCellRoundBorder("ON LOAN", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("FOR DEMO AND TRIAL", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("FOR REPAIRING", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("SUPPLY AGAINST SHORT SUPPLY", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("REPAIRED AND TESTED OKAY", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("PARTY'S MATERIAL BEING RETURNED", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("REJECTED RETURNED", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;



                tblDetail.AddCell(pdf.setCellRoundBorder("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder("Particulars", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder("Quantity", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder("Remarks", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCell(pdfMerge, pdf.WhiteBaseColor, pdf.fnCalibri10, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = dtItem.Rows.Count;
                tblDetail.AddCell(pdf.setCell(pdfextra, pdf.WhiteBaseColor, pdf.fnCalibri10, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                //if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                //{
                //    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                //    {
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0)).Rowspan = (int)(ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount));
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //}

                //tblDetail.AddCell(pdf.setCell(pdfextra, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 4, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));





                // -------------------------------------------------------------------------------------
                //  Defining : Footer 
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail


                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        //int[] column_tblSign = { 30 };
                        //tblSign.SetWidths(column_tblSign);

                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(80, 50);
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("Receiver's Stamp & Sign", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    tblESignature.AddCell(pdf.setCell("Receiver's Stamp & Sign" + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                // ---------------------------------------------------
                int[] column_tblFooter = { 100 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCellRoundBorder(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));

                #endregion
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GenerateOutward_ColdtechRound(Int64 pkID)
        {
            //HttpContext.Current.Session["PrintHeader"] = "no";
            HttpContext.Current.Session["printModule"] = "outward";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(2);
            PdfPTable tblDetail = new PdfPTable(5);

            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            if (lstQuot.Count > 0)
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);

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
                int[] column_tblMember = { 70, 30 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                string CustomerAdd = (!String.IsNullOrEmpty(lstCust[0].Address) ? lstCust[0].Address + " , " : "") + (String.IsNullOrEmpty(lstCust[0].Area) ? lstCust[0].Area + " , " : "");
                string CustomerAddCity = (!String.IsNullOrEmpty(lstCust[0].CityName) ? lstCust[0].CityName + " - " : "") + (!String.IsNullOrEmpty(lstCust[0].Pincode) ? lstCust[0].Pincode + " , " : "") + (!String.IsNullOrEmpty(lstCust[0].StateName) ? lstCust[0].StateName + " , " : "") + (!String.IsNullOrEmpty(lstCust[0].CountryName) ? lstCust[0].CountryName : "");

                PdfPTable tblCustomerD = new PdfPTable(2);
                int[] column_tblCustomerD = { 6, 94 };
                tblCustomerD.SetWidths(column_tblCustomerD);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                if ((!String.IsNullOrEmpty(lstCust[0].Address)) || (!String.IsNullOrEmpty(lstCust[0].Area)))
                {
                    tblCustomerD.AddCell(pdf.setCell(CustomerAdd, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                }
                if ((!String.IsNullOrEmpty(lstCust[0].CityName)) || (!String.IsNullOrEmpty(lstCust[0].Pincode)) || (!String.IsNullOrEmpty(lstCust[0].StateName)) || (!String.IsNullOrEmpty(lstCust[0].CountryName)))
                {
                    tblCustomerD.AddCell(pdf.setCell(CustomerAddCity, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                }

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                {
                    tblCustomerD.AddCell(pdf.setCell( "Contact No : " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                }

                PdfPTable tblOutwardD = new PdfPTable(2);
                int[] column_tblOutwardD = { 20, 80 };
                tblOutwardD.SetWidths(column_tblOutwardD);

                tblOutwardD.AddCell(pdf.setCell("No", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardDate.ToString("dd-MM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));



                //---------------------------------------------------------

                //--------------------------------------------------------------
                tblMember.AddCell(pdf.setCellRoundBorder(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                tblMember.AddCell(pdf.setCellRoundBorder(tblOutwardD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                //tblMember.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 7, 49, 14, 25,5 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                PdfPTable pdfMerge = new PdfPTable(3);
                int[] column_pdfMerge = { 10,70,20 };
                pdfMerge.SetWidths(column_pdfMerge);



                decimal totAmount = 0, taxAmount = 0, netAmount = 0, quantity = 0;

                int totalRowCount = 0, totalSpecLines = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    decimal q = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    quantity += Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());

                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    pdfMerge.AddCell(pdf.setCellRoundBorder((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    pdfMerge.AddCell(pdf.setCellRoundBorder(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                    pdfMerge.AddCell(pdf.setCellRoundBorder(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCellRoundBorder(dtItem.Rows[i]["ProductSpecification"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));

                }


                PdfPTable SrNo = new PdfPTable(1);
                int[] column_SrNo = { 100 };
                SrNo.SetWidths(column_SrNo);
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    SrNo.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                }

                PdfPTable PName = new PdfPTable(1);
                int[] column_PName = { 100 };
                PName.SetWidths(column_PName);
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    PName.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                }

                PdfPTable Qty = new PdfPTable(1);
                int[] column_Qty = { 100 };
                Qty.SetWidths(column_Qty);
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    Qty.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                }

                PdfPTable pdfextra = new PdfPTable(2);
                int[] column_pdfextra = { 70, 30 };
                pdfextra.SetWidths(column_pdfextra);
                pdfextra.AddCell(setCellRoundBorder("ON LOAN", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("FOR DEMO AND TRIAL", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("FOR REPAIRING", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("SUPPLY AGAINST SHORT SUPPLY", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("REPAIRED AND TESTED OKAY", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("PARTY'S MATERIAL BEING RETURNED", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder("REJECTED RETURNED", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;
                pdfextra.AddCell(setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = 2;



                tblDetail.AddCell(pdf.setCellRoundBorder("Sr.\n No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder("Particulars", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder("Quantity", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder("Remarks", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder(SrNo, pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder(PName, pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                tblDetail.AddCell(pdf.setCellRoundBorder(Qty, pdf.WhiteBaseColor, pdf.fnCalibriBold10, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                tblDetail.AddCell(pdf.setCell(pdfextra, pdf.WhiteBaseColor, pdf.fnCalibri10, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));


                //tblDetail.AddCell(pdf.setCell(pdfMerge, pdf.WhiteBaseColor, pdf.fnCalibri10, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0)).Rowspan = dtItem.Rows.Count;

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                //if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                //{
                //    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                //    {
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0)).Rowspan = (int)(ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount));
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));
                //        tblDetail.AddCell(pdf.setCellRoundBorder(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //}

                //tblDetail.AddCell(pdf.setCell(pdfextra, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 4, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 0));





                // -------------------------------------------------------------------------------------
                //  Defining : Footer 
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail


                PdfPTable tblESignature = new PdfPTable(1);
                    int[] column_tblESignature = { 100 };
                    tblESignature.SetWidths(column_tblESignature);
                    int fileCount = 0;
                    string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
                    //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eStamp.png";

                    if (File.Exists(tmpFile))
                    {
                        if (File.Exists(tmpFile))   //Signature print
                        {
                            PdfPTable tblSign = new PdfPTable(1);
                            //int[] column_tblSign = { 30 };
                            //tblSign.SetWidths(column_tblSign);

                            iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                            eSign.ScaleAbsolute(80, 50);
                            tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                            tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            tblSign.AddCell(pdf.setCell("Receiver's Stamp & Sign", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                            fileCount = fileCount + 1;
                        }
                    }
                    else
                    {
                        tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell("Receiver's Stamp & Sign" + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                    }
                    // ---------------------------------------------------
                    int[] column_tblFooter = { 100 };
                    tblFooter.SetWidths(column_tblFooter);
                    tblFooter.SpacingBefore = 0f;
                    tblFooter.LockedWidth = true;
                    tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    tblFooter.AddCell(pdf.setCellRoundBorder(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));

                    #endregion
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                    htmlClose = "</body></html>";

                    // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                    string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                    string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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


        public static PdfPCell setCellRoundBorder(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {

            PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            //tmpCell.Border = borderVal;
            tmpCell.CellEvent = new RoundedBorder();
            tmpCell.FixedHeight = 55f;
            return tmpCell;
        }

        public static void GenerateOutward_FieldMaster(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(7);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            List<Entity.OutwardDetailAssembly> lstAssembly = new List<Entity.OutwardDetailAssembly>();
            if (lstQuot.Count > 0)
                lstAssembly = BAL.OutwardMgmt.GetOutwardDetailAssemblyList(lstQuot[0].OutwardNo, 0, 0);
            // ------------------------------------------------------------------------------
            Int64 pkIDOrder = -1;
            Decimal totNetAmt = 0;
            int totrec1 = 0;
            if (!String.IsNullOrEmpty(lstQuot[0].OrderNo))
                pkIDOrder = BAL.CommonMgmt.GetSalesOrderPrimaryID(lstQuot[0].OrderNo);

            List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();
            lstOrder = BAL.SalesOrderMgmt.GetSalesOrderList(pkIDOrder, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out totrec1);
            if (lstOrder.Count > 0)
                totNetAmt = lstOrder.Sum(item => item.NetAmt);

            Decimal ordQty = 0;
            DataTable dtOrderDetail = new DataTable();
            dtOrderDetail = BAL.SalesOrderMgmt.GetSalesOrderDetail(lstQuot[0].OrderNo);
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

                PdfPTable tblCustomerD = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // -----------------------------------------------------------------
                PdfPTable tblTransport = new PdfPTable(3);
                int[] column_tblTrans = { 25, 25, 50 };
                tblTransport.SetWidths(column_tblTrans);
                tblTransport.AddCell(pdf.setCell("Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                tblTransport.AddCell(pdf.setCell("Mod Of Transport", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                tblTransport.AddCell(pdf.setCell("Transporter Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));

                tblTransport.AddCell(pdf.setCell(lstQuot[0].DeliveryNote, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                tblTransport.AddCell(pdf.setCell(lstQuot[0].ModeOfTransport, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                tblTransport.AddCell(pdf.setCell(lstQuot[0].TransporterName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));

                tblCustomerD.AddCell(pdf.setCell(tblTransport, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                // -----------------------------------------------------------------
                PdfPTable tblOutwardD = new PdfPTable(4);
                int[] column_tblNested2 = { 24, 35, 15, 26 };
                tblOutwardD.SetWidths(column_tblNested2);

                tblOutwardD.AddCell(pdf.setCell("Doc. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Order No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + ((lstOrder.Count > 0) ? lstOrder[0].OrderNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + ((lstOrder.Count > 0) ? lstOrder[0].OrderDate.ToString("dd-MMM-yyyy") : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Sales Terms", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": Cash/Credit", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Payment made for this Order : ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Ord. Amt", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + totNetAmt.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Outstanding Amt", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf1, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblMember.AddCell(pdf.setCell("Packing List", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf8, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblOutwardD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 5, 30, 30, 8, 8, 10, 8 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Product Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Assembly Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Order Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Packing Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Serial No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Box No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                string FinishProduct = "";
                decimal quantity = 0; ;
                int totalRowCount = 0, intcnt = 1;

                int detRow = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    if (dtOrderDetail.Rows.Count > 0)
                    {
                        for (int z = 0; z < dtOrderDetail.Rows.Count; z++)
                        {
                            if (dtOrderDetail.Rows[z]["ProductID"].ToString() == dtItem.Rows[i]["ProductID"].ToString())
                            {
                                ordQty = Convert.ToDecimal(dtOrderDetail.Rows[z]["Quantity"].ToString());
                            }
                        }
                    }

                    tblDetail.AddCell(pdf.setCell(intcnt.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                    tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                    tblDetail.AddCell(pdf.setCell(ordQty.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["SerialNo"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["BoxNo"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                    detRow = detRow + 1;

                    for (int j = 0; j < lstAssembly.Count; j++)
                    {
                        if (dtItem.Rows[i]["ProductID"].ToString() == lstAssembly[j].ProductID.ToString())
                        {
                            Decimal subQty;
                            subQty = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                            tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                            tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                            tblDetail.AddCell(pdf.setCell(lstAssembly[j].AssemblyName.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                            tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                            tblDetail.AddCell(pdf.setCell(subQty.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                            tblDetail.AddCell(pdf.setCell(lstAssembly[j].SerialNo.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                            tblDetail.AddCell(pdf.setCell(lstAssembly[j].BoxNo.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.BOX));
                            detRow = detRow + 1;
                        }
                    }
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (detRow + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (lstAssembly.Count + dtItem.Rows.Count + totalRowCount)); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }
                //tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 13));
                //tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 13));
                //tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 13));
                //tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 13));
                #endregion

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                tblDetail.AddCell(pdf.setCell("FOR ACCOUNT USE:", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblDetail.AddCell(pdf.setCell("D.C NO : " + lstQuot[0].DCNo.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf5, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblDetail.AddCell(pdf.setCell("AMOUNT  : " + totNetAmt, pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf5, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 8));

                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

                tblDetail.AddCell(pdf.setCell("L.R Docket No : " + lstQuot[0].LRNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 4));
                tblDetail.AddCell(pdf.setCell("For FIELDMASTER INNOVATION LTD", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 4, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 8));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 12));
                tblDetail.AddCell(pdf.setCell("Verify Sign :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 6));
                tblDetail.AddCell(pdf.setCell("Auth. Signatory", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf5, 4, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 10));

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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
                tblFooter.HorizontalAlignment = Element.ALIGN_CENTER;
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
                // ------------------------------------------------
                RecompressPDF(sPath + smallFileName, pdfFileName);
            }
        }


        public static void GenerateOutward_Balaji(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(10);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
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
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // ------------------------------------------------------------------------------------- 
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblCustomerD = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                PdfPTable tblOutwardD = new PdfPTable(4);
                int[] column_tblNested2 = { 24, 35, 15, 26 };
                tblOutwardD.SetWidths(column_tblNested2);

                tblOutwardD.AddCell(pdf.setCell("Doc. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("S/O No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OrderNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblMember.AddCell(pdf.setCell("Packing List", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf8, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblOutwardD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 5, 35, 5, 7, 6, 9, 9, 6, 9, 9 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 7, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Qty (Weight)", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                decimal totAmount = 0, taxAmount = 0, netAmount = 0, quantity = 0; ;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    decimal q = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    quantity += Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());

                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["QuantityWeight"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductSpecification"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell("", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    // ----------------------------------------
                    //List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    //lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].OutwardNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    //if (lstSpec.Count > 0)
                    //{
                    //    string tmpSpec = dtItem.Rows[i]["ProductSpecification"].ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 6, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
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
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }
                tblDetail.AddCell(pdf.setCell("Total", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 8, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(quantity.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GenerateOutward_Sharvaya(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(10);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
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
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Information
                // ------------------------------------------------------------------------------------- 
                #region Section >>>> Quotation Master Information
                int[] column_tblMember = { 25, 25, 25, 25 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;

                PdfPTable tblCustomerD = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                PdfPTable tblOutwardD = new PdfPTable(4);
                int[] column_tblNested2 = { 24, 35, 15, 26 };
                tblOutwardD.SetWidths(column_tblNested2);

                tblOutwardD.AddCell(pdf.setCell("Doc. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("S/O No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OrderNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblMember.AddCell(pdf.setCell("Outward Details", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf8, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblOutwardD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 5, 35, 5, 7, 6, 9, 9, 6, 9, 9 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 7, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                decimal totAmount = 0, taxAmount = 0, netAmount = 0, quantity = 0; ;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    decimal q = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    quantity += Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());

                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));

                    // ----------------------------------------
                    List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].OutwardNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    if (lstSpec.Count > 0)
                    {
                        string tmpSpec = dtItem.Rows[i]["ProductSpecification"].ToString();
                        if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                            totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                        }
                    }
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }
                tblDetail.AddCell(pdf.setCell("Total", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 8, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(quantity.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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


        public static void GenerateOutward_Jalaram(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(2);
            PdfPTable tblDetail = new PdfPTable(5);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            if (lstQuot.Count > 0)
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);


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
                int[] column_tblMember = { 50, 50 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;



                //----------------------Customer Name and Address---------------------------------//
                PdfPTable tblKind = new PdfPTable(2);
                int[] column_tblKind = { 30, 70 };
                tblKind.SetWidths(column_tblKind);
                tblKind.AddCell(pdf.setCell("GSTIN", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblKind.AddCell(pdf.setCell(": " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblKind.AddCell(pdf.setCell("GSM", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblKind.AddCell(pdf.setCell(": " + ((lstOrg.Count > 0) ? lstOrg[0].Landline1 : " "), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblOrg = new PdfPTable(1);
                int[] column_tblOrg = { 100 };
                tblOrg.SetWidths(column_tblOrg);

                //Phrase ph1 = new Phrase();
                //Chunk c1 = new Chunk("Bill To (Recipient) :-", pdf.fnCalibriBold9);
                //c1.SetUnderline(1, -2);
                //ph1.Add(c1);

                tblOrg.AddCell(pdf.setCell(objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstOrg[0].Address.TrimStart('.')))
                    tblOrg.AddCell(pdf.setCell(lstOrg[0].Address + ", ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].CityName.TrimStart('.') + lstOrg[0].StateName.TrimStart('.') + lstOrg[0].Pincode.TrimStart('.')))
                    tblOrg.AddCell(pdf.setCell(lstOrg[0].CityName + " - " + lstOrg[0].Pincode + ", " + lstOrg[0].StateName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblOrg.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblOrg.AddCell(pdf.setCell(tblKind, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblCust = new PdfPTable(1);
                int[] column_tblCust = { 100 };
                tblCust.SetWidths(column_tblCust);

                //Phrase ph3 = new Phrase();
                //Chunk c3 = new Chunk("Ship To (Delivery) :-", pdf.fnCalibriBold9);
                //c3.SetUnderline(1, -2);
                //ph3.Add(c3);

                tblCust.AddCell(pdf.setCell("Buyers : ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCust.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                if (!String.IsNullOrEmpty(lstCust[0].Address.TrimStart('.') + lstCust[0].Area.TrimStart('.')))
                    tblCust.AddCell(pdf.setCell(lstCust[0].Address + ", " + lstCust[0].Area + ", ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.TrimStart('.') + lstCust[0].StateName.TrimStart('.') + lstCust[0].Pincode1.TrimStart('.')))
                    tblCust.AddCell(pdf.setCell(lstCust[0].CityName + " - " + lstCust[0].Pincode1 + ", " + lstCust[0].StateName + ", " + lstCust[0].CountryName, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblCust.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCust.AddCell(pdf.setCell("GST # " + lstCust[0].GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblMix = new PdfPTable(1);
                int[] column_tblMix = { 100 };
                tblMix.SetWidths(column_tblMix);

                tblMix.AddCell(pdf.setCell(tblOrg, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblMix.AddCell(pdf.setCell(tblCust, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));


                //----------------------Invoice Details---------------------------------//
                PdfPTable tblInvoiceDetails = new PdfPTable(2);
                int[] column_tblInvoiceDetails = { 42, 55 };
                tblInvoiceDetails.SetWidths(column_tblInvoiceDetails);

                tblInvoiceDetails.AddCell(pdf.setCell("Outward No :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Date :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(lstQuot[0].OutwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Tentative Delivery :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Packing Type :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Payment :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Transportation :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Unloading of Site :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Delivery at :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("Generated by :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell("PO Verified By:", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblInvoiceDetails.AddCell(pdf.setCell(lstQuot[0].EmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));



                //tblMember.AddCell(pdf.setCell("Offer / Proforma", pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblMember.AddCell(pdf.setCell(tblSec, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblMix, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblMember.AddCell(pdf.setCell(tblInvoiceDetails, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail


                //var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));
                int[] column_tblNested = { 30, 20, 16, 18, 16 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.KeepTogether = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;


                tblDetail.AddCell(pdf.setCell("Particulars", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Quantity", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Rate / Brick", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Amount", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Total (Rs.)", pdf.GrayBaseColor, pdf.fnCalibriBold10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));

                int totalSpecLines = 0;
                decimal totAmount = 0, taxAmount = 0, netAmount = 0, amount = 0;
                int totalRowCount = 0;
                Decimal TotalQty = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    amount = Convert.ToDecimal(dtItem.Rows[i]["UnitRate"]) * Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);

                    //-------------------------------------------------------------------
                    string tmpHSNCode = "";
                    List<Entity.Products> lstProd = new List<Entity.Products>();
                    //if (lstProd.Count > 0)
                    //{
                    Int64 tmpIcode = Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString());
                    lstProd = BAL.ProductMgmt.GetProductList(tmpIcode, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                    if (lstProd.Count > 0)
                        tmpHSNCode = lstProd[0].HSNCode.ToString();
                    //}
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    //tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString() + " " + dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell("Rs. " + dtItem.Rows[i]["UnitRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["UnitRate"].ToString() + " x " + dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                    tblDetail.AddCell(pdf.setCell(amount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));

                    TotalQty += Convert.ToInt64(dtItem.Rows[i]["Quantity"]);


                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                    {

                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));

                    }
                }


                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

                // ---------------------------------------------------------------------------------------------------------

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-                
                #endregion
                //---------------------------------Tax Details Table------------------------------------



                tblDetail.AddCell(pdf.setCell("Sub Total", pdf.GrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibri9, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                tblDetail.AddCell(pdf.setCell(lstQuot[0].BasicAmount.ToString(), pdf.GrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));



                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition



                PdfPTable tblTerms = new PdfPTable(1);
                int[] column_tblTerms = { 100 };
                tblTerms.SetWidths(column_tblTerms);
                tblTerms.KeepTogether = true;
                tblTerms.SplitLate = false;
                tblTerms.AddCell(pdf.setCell("Terms & Conditions : -", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTerms.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblTerms.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblBank = new PdfPTable(1);
                int[] column_tblBank = { 100 };
                tblBank.SetWidths(column_tblBank);
                tblBank.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(pdf.setCell("E. & O.E.", pdf.GrayBaseColor, pdf.fnCalibriItalic8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibriItalic8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblBank.AddCell(pdf.setCell(" ", pdf.GrayBaseColor, pdf.fnCalibriItalic8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));




                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature1 = { 100 };
                tblESignature.SetWidths(column_tblESignature1);
                int fileCount = 0;
                string tmpFile;
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";
                // -------------------------------------------------------
                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(90, 45);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    }
                }

                var phrase = new Phrase();
                phrase.Add(new Chunk("Jay Jalaram Brick Works,", pdf.fnCalibriBold9));
                Chunk c = new Chunk("Jay Jalaram Brick Works,", pdf.fnCalibriBold9);
                Chunk c1 = new Chunk("Tel : ", pdf.fnCalibriBold9);
                Chunk c2 = new Chunk("Cell : ", pdf.fnCalibriBold9);
                Chunk c3 = new Chunk("Email : ", pdf.fnCalibriBold9);

                PdfPTable tblThank = new PdfPTable(1);
                int[] column_tblThank = { 100 };
                tblThank.SetWidths(column_tblThank);
                tblThank.AddCell(pdf.setCell("Head Office :", pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblThank.AddCell(pdf.setCell("Jay Jalaram Brick Works, Near Main R.T.O. Office, Ghodhra - 389001. Gujarat, India", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblThank.AddCell(pdf.setCell("Tel: +91 2672 266088 / +91 9313765101 / +91 9313752907 , Cell: +91 9925670707 / +919978377077", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblThank.AddCell(pdf.setCell("Email : tarun@jjb.co.in", pdf.WhiteBaseColor, pdf.fnCalibri9, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblsign = new PdfPTable(2);
                int[] column_tblsign = { 70, 30 };
                tblsign.SetWidths(column_tblsign);

                tblsign.AddCell(pdf.setCell(tblThank, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                tblsign.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                int[] column_tblFooter = { 70, 30 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                tblFooter.AddCell(pdf.setCell(tblTerms, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFooter.AddCell(pdf.setCell(tblBank, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFooter.AddCell(pdf.setCell(tblThank, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblFooter.AddCell(pdf.setCell("SUBJECT TO GHODHRA JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFooter.AddCell(pdf.setCell(tblLast, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                //tblFooter.AddCell(pdf.setCell(tblESignature, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                //tblSignOff.SpacingBefore = 0f;
                //tblSignOff.LockedWidth = true;
                //tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                //tblSignOff.AddCell(pdf.setCell("SUBJECT TO GHODHRA JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                #endregion
                // >>>>>> Closing : HTML & BODY
                iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
                htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

                // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


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

                // >>>>>> Closing : HTML & BODY

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


        public static void GenerateOutward_Hitech(Int64 pkID)
        {
            HttpContext.Current.Session["PrintHeader"] = "no";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(10);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            if (lstQuot.Count > 0)
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);

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

                PdfPTable tblCustomerD = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell("GST No    : " + ((lstCust.Count > 0) ? lstCust[0].GSTNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                PdfPTable tblOutwardD = new PdfPTable(4);
                int[] column_tblNested2 = { 24, 35, 15, 26 };
                tblOutwardD.SetWidths(column_tblNested2);

                tblOutwardD.AddCell(pdf.setCell("Doc. No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OutwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell("S/O No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(": " + lstQuot[0].OrderNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblOutwardD.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                ////---------------Header------------------------
                //PdfPTable tblLocation = new PdfPTable(1);
                //int[] column_tblLocation = { 100 };
                //tblLocation.SetWidths(column_tblLocation);
                //int fileCount1 = 0;
                //string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\headerforall.jpg";


                //if (File.Exists(tmpFile1))
                //{
                //    if (File.Exists(tmpFile1))   //Signature print
                //    {
                //        PdfPTable tblSymbol = new PdfPTable(1);
                //        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                //        eLoc.ScaleAbsolute(530, 80);


                //        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                //        tblLocation.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //        fileCount1 = fileCount1 + 1;
                //    }
                //}
                //---------------------------------------------------------

                //-----------------------Hitech Logo---------------------------------------
                PdfPTable tblLogo = new PdfPTable(1);
                int[] column_tblLogo = { 100 };
                tblLogo.SetWidths(column_tblLogo);
                int fileCount1 = 0;
                string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\Hitech.png";


                if (File.Exists(tmpFile1))
                {
                    if (File.Exists(tmpFile1))   //Signature print
                    {
                        PdfPTable tblSymbol = new PdfPTable(1);
                        iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                        eLoc.ScaleAbsolute(210, 73);


                        tblSymbol.AddCell(pdf.setCellFixImage(eLoc, pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                        tblLogo.AddCell(pdf.setCell(tblSymbol, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount1 = fileCount1 + 1;
                    }
                }
                //------------------------------------Location Image-----------------------------
                PdfPTable tblLocation = new PdfPTable(1);
                int[] column_tblLocation = { 100 };
                tblLocation.SetWidths(column_tblLocation);
                int fileCount2 = 0;
                string tmpFile2 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/") + "\\location.png";


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

                PdfPTable tblName = new PdfPTable(1);
                int[] column_tblName = { 100 };
                tblName.SetWidths(column_tblName);

                tblName.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri10, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblName.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].Address.TrimStart('.')))
                    tblName.AddCell(pdf.setCell(lstOrg[0].Address + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].CityName.TrimStart('.') + lstOrg[0].StateName.TrimStart('.') + lstOrg[0].Pincode.TrimStart('.')))
                    tblName.AddCell(pdf.setCell(lstOrg[0].CityName.TrimStart('.') + " - " + lstOrg[0].Pincode + ", " + lstOrg[0].StateName.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblName.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblAddress = new PdfPTable(2);
                int[] column_tblAddress = { 32, 68 };
                tblAddress.SetWidths(column_tblAddress);

                tblAddress.AddCell(pdf.setCell(tblLocation, pdf.WhiteBaseColor, pdf.fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(tblName, pdf.WhiteBaseColor, pdf.fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell(" GST NO : 24AAMFH4110Q1Z7", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("MOBLIE NO :- +91 90990 67240, +91 98795 83066, T-(079) 25855240", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblAddress.AddCell(pdf.setCell("EMAIL :- info@hi-techscrewbarrel.com, dpshitech@gmail.com", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                PdfPTable tblMix = new PdfPTable(2);
                int[] column_tblMix = { 40, 60 };
                tblMix.SetWidths(column_tblMix);
                tblMix.AddCell(pdf.setCell(tblLogo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMix.AddCell(pdf.setCell(tblAddress, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));



                tblMember.AddCell(pdf.setCell("Outward Details", pdf.WhiteBaseColor, pdf.fnCalibriBold20, pdf.paddingOf8, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                //tblMember.AddCell(pdf.setCell(tblLocation, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblMix, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblOutwardD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail

                int[] column_tblNested = { 5, 35, 5, 7, 6, 9, 9, 6, 9, 9 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr.No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 7, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Qty", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Unit", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Disc %", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Rate", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Tax Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //tblDetail.AddCell(pdf.setCell("Net Amount", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                decimal totAmount = 0, taxAmount = 0, netAmount = 0, quantity = 0;

                int totalRowCount = 0, totalSpecLines = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {

                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    netAmount += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    decimal q = Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());
                    quantity += Convert.ToDecimal(dtItem.Rows[i]["Quantity"].ToString());

                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 8));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["DiscountPercent"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Amount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxRate"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["TaxAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));
                    //tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["NetAmount"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 12));

                    // ----------------------------------------

                    string tmpSpec = dtItem.Rows[i]["ProductSpecification"].ToString();

                    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    {
                        // ------------------------------------------------------------
                        PdfPCell text2cell = new PdfPCell(new Phrase("", new iTextSharp.text.Font(FontFactory.GetFont("Calibri", 8.0f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0)))));
                        text2cell.BackgroundColor = BaseColor.WHITE;

                        text2cell.Colspan = 1;
                        text2cell.Padding = 2;
                        text2cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        text2cell.VerticalAlignment = Element.ALIGN_TOP;
                        tmpSpec = "<div style='font-family:Calibri; font-size: 9pt;'>" + tmpSpec + "</div>";
                        foreach (iTextSharp.text.IElement elm in HTMLWorker.ParseToList(new StringReader(tmpSpec), new StyleSheet()))
                        {
                            text2cell.AddElement(elm);
                            totalSpecLines = totalSpecLines + 1;
                        }
                        // -------------------------------------------------------------
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(text2cell, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        //totalRowCount += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                    }

                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                if (ProdDetail_Lines > (dtItem.Rows.Count + totalRowCount))
                {
                    for (int i = 1; i <= (ProdDetail_Lines - (dtItem.Rows.Count + totalRowCount)); i++)
                    {
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 7, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    }
                }
                tblDetail.AddCell(pdf.setCell("Total", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 8, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(quantity.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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

        public static void GenerateOutward_PRI(Int64 pkID)
        {
            //HttpContext.Current.Session["PrintHeader"] = "no";
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(2);
            PdfPTable tblDetail = new PdfPTable(7);
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            if (lstQuot.Count > 0)
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);


            List<Entity.Outward> lstExp = new List<Entity.Outward>();
            if (lstQuot.Count > 0)
                lstExp = BAL.OutwardMgmt.GetOutwardExportList(pkID, lstQuot[0].OutwardNo, HttpContext.Current.Session["LoginUserID"].ToString());
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
                int[] column_tblMember = { 45, 55 };
                tblMember.SetWidths(column_tblMember);
                tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;


                //---------------------------------Exporter Details---------------------------
                PdfPTable tblExporter = new PdfPTable(1);
                int[] column_tblExporter = { 100 };
                tblExporter.SetWidths(column_tblExporter);

                tblExporter.AddCell(pdf.setCell("Exporter ;", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblExporter.AddCell(pdf.setCell(lstOrg[0].OrgName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].Address.TrimStart('.') + lstOrg[0].CityName.TrimStart('.')))
                    tblExporter.AddCell(pdf.setCell(lstOrg[0].Address + "," + lstOrg[0].CityName.TrimStart('.') + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstOrg[0].StateName.TrimStart('.') + lstOrg[0].Pincode.TrimStart('.')))
                    tblExporter.AddCell(pdf.setCell(lstOrg[0].StateName.Trim() + "-" + lstOrg[0].Pincode, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblExporter.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //-------------------------------Consignee Details----------------------------
                PdfPTable tblConsignee = new PdfPTable(1);
                int[] column_tblConsignee = { 100 };
                tblConsignee.SetWidths(column_tblConsignee);

                tblConsignee.AddCell(pdf.setCell("Consignee ;", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblConsignee.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].Address.TrimStart('.') + lstCust[0].Area.TrimStart('.')))
                    tblConsignee.AddCell(pdf.setCell(lstCust[0].Address + "," + lstCust[0].Area + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName.TrimStart('.') + lstCust[0].StateName.TrimStart('.') + lstCust[0].Pincode.TrimStart('.')))
                    tblConsignee.AddCell(pdf.setCell(lstCust[0].CityName.TrimStart('.') + " - " + lstCust[0].Pincode + ", " + lstCust[0].StateName.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblConsignee.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                    tblConsignee.AddCell(pdf.setCell("TEL: " + lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                //-----------------------------Invoice Details---------------------------------
                PdfPTable tblInvoice = new PdfPTable(2);
                int[] column_tblInvoice = { 60, 40 };
                tblInvoice.SetWidths(column_tblInvoice);

                tblInvoice.AddCell(pdf.setCell("Outward No. & Date :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 13));
                tblInvoice.AddCell(pdf.setCell("Exporter's Ref. :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 13));
                tblInvoice.AddCell(pdf.setCell(lstQuot[0].OutwardNo + " DT. " + lstQuot[0].OutwardDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblInvoice.AddCell(pdf.setCell(lstQuot[0].ExporterRef, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblInvoice.AddCell(pdf.setCell("Buyer's Order No. & Date:", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 13));
                tblInvoice.AddCell(pdf.setCell("P.O. NO.: " + lstQuot[0].SupOrderRef + "   DATE " + lstQuot[0].SupOrderDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 14));
                tblInvoice.AddCell(pdf.setCell("Other Reference(s).", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblInvoice.AddCell(pdf.setCell(lstQuot[0].OtherRef, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 11));

                //--------------------------------Notify Details-------------------------------
                PdfPTable tblNotify = new PdfPTable(1);
                int[] column_tblNotify = { 100 };
                tblNotify.SetWidths(column_tblNotify);
                tblNotify.AddCell(pdf.setCell(" NOTIFY PARTY :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 13));
                tblNotify.AddCell(pdf.setCell(lstQuot[0].CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].Address1.TrimStart('.') + lstCust[0].Area1.TrimStart('.')))
                    tblNotify.AddCell(pdf.setCell(lstCust[0].Address1 + "," + lstCust[0].Area1 + ",", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].CityName1.TrimStart('.') + lstCust[0].StateName1.TrimStart('.') + lstCust[0].Pincode1.TrimStart('.')))
                    tblNotify.AddCell(pdf.setCell(lstCust[0].CityName1.TrimStart('.') + " - " + lstCust[0].Pincode1 + ", " + lstCust[0].StateName1.Trim(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                else
                    tblNotify.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                if (!String.IsNullOrEmpty(lstCust[0].ContactNo1))
                    tblNotify.AddCell(pdf.setCell("TEL: " + lstCust[0].ContactNo2, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //----------------------------------Shipment Details--------------------------------
                PdfPTable tblShipment = new PdfPTable(2);
                int[] column_tblShipment = { 40, 60 };
                tblShipment.SetWidths(column_tblShipment);
                tblShipment.AddCell(pdf.setCell("Pre Carriage By", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell("Place of Receipt by Pre Carrier", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].PreCarrBy : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].PreCarrRecPlace : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell("VESSEL/FLIGHT No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell("Port Of Loading", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].FlightNo : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].PortOfLoading : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell("Port of Dispatch", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell("Final Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].PortOfDispatch : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].PortOfDestination : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell("Marks & Nos.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell("Nos. & Kinds of Packages.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].Packages : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblShipment.AddCell(pdf.setCell((lstExp.Count > 0 ? lstExp[0].PackageType : " "), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));


                //----------------------------Other Details-----------------------------------------
                PdfPTable tblOther = new PdfPTable(2);
                int[] column_tblOther = { 45, 55 };
                tblOther.SetWidths(column_tblOther);
                tblOther.AddCell(pdf.setCell("Country of Origin", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblOther.AddCell(pdf.setCell("Country of final Destination", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 13));
                tblOther.AddCell(pdf.setCell("India", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblOther.AddCell(pdf.setCell(lstCust[0].CountryName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 14));
                tblOther.AddCell(pdf.setCell("Shipment Terms :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOther.AddCell(pdf.setCell(lstQuot[0].DeliveryNote, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOther.AddCell(pdf.setCell("Payment Terms :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOther.AddCell(pdf.setCell(lstQuot[0].Remarks, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                tblOther.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 0));
                tblOther.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 0));


                //-------------------------------Appending to Member table------------------------
                tblMember.AddCell(pdf.setCell("Packing List", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblExporter, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblInvoice, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblConsignee, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblNotify, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblShipment, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                tblMember.AddCell(pdf.setCell(tblOther, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));


                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Product Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Product Detail


                int[] column_tblNested = { 12, 35, 8, 7, 7, 14, 17 };
                tblDetail.SetWidths(column_tblNested);

                tblDetail.SpacingBefore = 0f;
                tblDetail.LockedWidth = true;
                tblDetail.SplitLate = false;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(pdf.setCell("Sr." + "\n" + "No", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Description", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Box No.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Nos.", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("UOM", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell("Remarks", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                decimal totAmount = 0, taxAmount = 0, netAmount = 0, qty = 0;
                int totalRowCount = 0;
                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    qty += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"]);
                    taxAmount += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"]);
                    // ------------------------------------------------------------------
                    string tmpVal = "", tmpProdAlias = "";
                    tmpVal = dtItem.Rows[i]["ProductNameLong"].ToString();
                    if (tmpVal.IndexOf("]") >= 0)
                        tmpProdAlias = tmpVal.Substring(1, tmpVal.IndexOf("]") - 1) + " ";

                    if (i > 0)
                    {
                        if (dtItem.Rows[i]["ProductName"].Equals(dtItem.Rows[i - 1]["ProductName"]))
                        {
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 14));
                            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 14));
                        }
                        else
                        {
                            tblDetail.AddCell(pdf.setCell(i.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                            tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                        }
                    }
                    else
                    {
                        tblDetail.AddCell(pdf.setCell((i + 1).ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
                        tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
                    }
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["BoxNo"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Quantity"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["Unit"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["SerialNo"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
                    tblDetail.AddCell(pdf.setCell(dtItem.Rows[i]["ProductSpecification"].ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));




                    // ----------------------------------------
                    //List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
                    //lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(lstQuot[0].QuotationNo, Convert.ToInt64(dtItem.Rows[i]["ProductID"].ToString()), HttpContext.Current.Session["LoginUserID"].ToString());
                    //if (lstSpec.Count > 0)
                    //{
                    //    string tmpSpec = lstSpec[0].MaterialSpec.ToString();
                    //    if (!String.IsNullOrEmpty(tmpSpec.Trim()))
                    //    {
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(tmpSpec, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 12));
                    //        if (sumDis > 0)
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //        else
                    //        {
                    //            tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));
                    //        }
                    //         >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
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
                        tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 12));

                    }
                }
                tblDetail.AddCell(pdf.setCell("Net Wt. (Kgs.)", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 9));

                tblDetail.AddCell(pdf.setCell("Gros Wt. (Kgs.)", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                tblDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 10));

                //decimal othercharge = lstQuot[0].ChargeAmt1 + lstQuot[0].ChargeAmt2 + lstQuot[0].ChargeAmt3 + lstQuot[0].ChargeAmt4 + lstQuot[0].ChargeAmt5;

                //tblDetail.AddCell(pdf.setCell("FRIEGHT / INSURANCE", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(othercharge.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(othercharge.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}

                //decimal total = totAmount + othercharge;

                //tblDetail.AddCell(pdf.setCell("TOTAL AMOUNT : USD", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(total.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 11));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(total.ToString(), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 11));
                //}
                //string NetAmtInWord = BAL.CommonMgmt.ConvertNumbertoWords((int)total);
                //tblDetail.AddCell(pdf.setCell("Amount in Words:" + NetAmtInWord, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));


                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

                //PdfPTable tblTNC = new PdfPTable(1);
                //int[] column_tblTNC = { 100 };
                //tblTNC.SetWidths(column_tblTNC);
                //tblTNC.AddCell(pdf.setCell("Terms & Conditions : ", pdf.WhiteBaseColor, pdf.fnArialBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblTNC.AddCell(pdf.setCell(lstQuot[0].TermsCondition, pdf.WhiteBaseColor, pdf.fnArial8, pdf.paddingOf5, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


                #region >>> Section of GST and Other charges Calculation
                //// ---------------------------------------------------------------------------------------------------------
                //PdfPTable tblAmount = new PdfPTable(2);
                //int[] column_tblAmount = { 60, 40 };
                //tblAmount.SetWidths(column_tblAmount);
                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //Decimal totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

                //totAmount = lstQuot[0].BasicAmt;
                //totRNDOff = lstQuot[0].ROffAmt;
                //totGST = (lstQuot[0].SGSTAmt + lstQuot[0].CGSTAmt + lstQuot[0].IGSTAmt);

                //tblAmount.AddCell(pdf.setCell("Basic Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(pdf.setCell(totAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt1;
                //    befGST += lstQuot[0].ChargeGSTAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt2;
                //    befGST += lstQuot[0].ChargeGSTAmt2;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt3;
                //    befGST += lstQuot[0].ChargeGSTAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt4;
                //    befGST += lstQuot[0].ChargeGSTAmt4;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    befAmt += lstQuot[0].ChargeAmt5;
                //    befGST += lstQuot[0].ChargeGSTAmt5;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()) && lstQuot[0].ExchangeRate == 0)
                //{
                //    if (lstQuot[0].IGSTAmt > 0)
                //    {
                //        tblAmount.AddCell(pdf.setCell("IGST @ 18% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].IGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //    else
                //    {
                //        tblAmount.AddCell(pdf.setCell("CGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //        tblAmount.AddCell(pdf.setCell("SGST @ 9% :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //        else
                //            tblAmount.AddCell(pdf.setCell(lstQuot[0].SGSTAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    }
                //}

                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ChargeGSTAmt1 == 0 && lstQuot[0].ChargeAmt1 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt1;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName1.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt1.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt2 == 0 && lstQuot[0].ChargeAmt2 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt2;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName2.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt2.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt3 == 0 && lstQuot[0].ChargeAmt3 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt3;
                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName3.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt3.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt4 == 0 && lstQuot[0].ChargeAmt4 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt4;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName4.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt4.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                //if (lstQuot[0].ChargeGSTAmt5 == 0 && lstQuot[0].ChargeAmt5 > 0)
                //{
                //    decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuot[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
                //    string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
                //    aftAmt += lstQuot[0].ChargeAmt5;

                //    tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeName5.ToString() + strChrgPer, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].ChargeAmt5.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}
                ///* ---------------------------------------------------------- */
                //if (lstQuot[0].ROffAmt > 0 || lstQuot[0].ROffAmt < 0)
                //{
                //    tblAmount.AddCell(pdf.setCell("Round Off    :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblAmount.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + lstQuot[0].ROffAmt.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //    else
                //        tblAmount.AddCell(pdf.setCell((lstQuot[0].ROffAmt).ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //}

                //tblAmount.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                //if (sumDis > 0)
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}
                //else
                //{
                //    tblDetail.AddCell(pdf.setCell(tblTNC, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
                //}
                #endregion
                // ****************************************************************
                //netAmount = lstQuot[0].NetAmt;
                //PdfPTable tblAmount1 = new PdfPTable(2);
                //int[] column_tblAmount1 = { 60, 40 };
                //tblAmount1.SetWidths(column_tblAmount1);
                //string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
                //tblAmount1.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //    tblAmount1.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //// -----------------------------------------------
                //if (sumDis > 0)
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 3, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}
                //else
                //{
                //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    else
                //        tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //}
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-                
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Terms & Condition
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition
                PdfPTable tblFootDetail = new PdfPTable(2);
                int[] column_tblFootDetail = { 80, 20 };
                tblFootDetail.SetWidths(column_tblFootDetail);

                //tblFootDetail.AddCell(pdf.setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 1));
                //tblFootDetail.AddCell(pdf.setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                //tblFootDetail.AddCell(pdf.setCell("GST No  : " + objAuth.GSTNo + "        " + "PAN No  : " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                //tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstQuot.Count > 0) ? lstQuot[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstQuot.Count > 0) ? lstQuot[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstQuot.Count > 0) ? lstQuot[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                //tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("Disclaimer : We declare that this Invoice shows the actual price of the goods described", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblFootDetail.AddCell(pdf.setCell("and that all particulars are true and correct.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // ---------------------------------------------------
                PdfPTable tblESignature = new PdfPTable(1);
                int[] column_tblESignature = { 100 };
                tblESignature.SetWidths(column_tblESignature);
                int fileCount = 0;
                string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";

                if (File.Exists(tmpFile))
                {
                    if (File.Exists(tmpFile))   //Signature print
                    {
                        PdfPTable tblSign = new PdfPTable(1);
                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                        eSign.ScaleAbsolute(90, 70);
                        tblSign.AddCell(pdf.setCellFixImage(eSign, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblSign.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        tblESignature.AddCell(pdf.setCell(tblSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                        fileCount = fileCount + 1;
                    }
                }
                else
                {
                    tblESignature.AddCell(pdf.setCell("For, " + objAuth.CompanyName, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }

                // ---------------------------------------------------
                int[] column_tblFooter = { 70, 30 };
                tblFooter.SetWidths(column_tblFooter);
                tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 7));
                tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(lstOrg[0].OrgName, 3), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 15));
                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                //tblSignOff.SpacingBefore = 0f;
                //tblSignOff.LockedWidth = true;
                //tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                //tblSignOff.AddCell(pdf.setCell("SUBJECT TO AHMEDABAD JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
                #endregion

                // >>>>>> Closing : HTML & BODY
                //htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
                //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                //pdfDoc.Close();
                //pdfDoc.Dispose();
                //string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
                //byte[] content = ms.ToArray();
                //FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
                //fs.Write(content, 0, (int)content.Length);
                //fs.Close();
                //fs.Dispose();
                //string pdfFileName = "";
                //pdfFileName = sPath + sFileName;
                //// ------------------------------------------------
                //RecompressPDF(sPath + smallFileName, pdfFileName);
            }
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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

        [WebMethod(EnableSession = true)]
        public static void GenerateOutwardhnmed(Int64 pkID)
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

            if (tmpSerialKey == "HONP-MEDF-9RTS-FG10")          // HNMED
            {
                GenerateOutwardhnmed_Sharvaya(pkID);
            }
            else
            {
                GenerateOutwardhnmed_Sharvaya(pkID);
            }

        }

        public static void GenerateOutwardhnmed_Sharvaya(Int64 pkID)
        {
            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;

            PdfPTable tableHeader = new PdfPTable(1);
            PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblDetail = new PdfPTable(10);
            //PdfPTable tblCylDetail = new PdfPTable(4);

            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
            PdfPTable tblSignOff = new PdfPTable(2);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // ===========================================================================================


            float paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf10 = 10, lengthmm = 2;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

            float length = (float)((lengthmm) / 25.4 * 72);

            Int64 TopMargin = Convert.ToInt64(length), BottomMargin = Convert.ToInt64(length), LeftMargin = Convert.ToInt64(length), RightMargin = Convert.ToInt64(length);
            Int64 ProdDetail_Lines = 0;

            // =========================================================================
            // PDF Document Object Instance Creation
            // =========================================================================
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
            //pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            float Widthmm = 100, Heightmm = 71;
            //Double Width = (((Convert.ToDouble(Widthmm)) / 25.4) * 72);
            float Width = (float)((Widthmm) / 25.4 * 72);
            float Height = (float)((Heightmm) / 25.4 * 72);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(Width, Height));
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
            List<Entity.Outward> lstQuot = new List<Entity.Outward>();
            lstQuot = BAL.OutwardMgmt.GetOutwardList(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
            //if (lstQuot.Count > 0)
            //{
            //    HttpContext.Current.Session["PrintUnitAddress"] = lstQuot[0].ProjectName;
            //}
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.OutwardMgmt.GetOutwardDetail(lstQuot[0].OutwardNo);

            // -------------------------------------------------------------------------------------------------------------
            int totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstQuot.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);
            // ------------------------------------------------------------------------------
            List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
            lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();

            int totrec1 = 0;
            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
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
                int[] column_tblMember = { 100 };
                tblMember.SetWidths(column_tblMember);
                //tblMember.SpacingBefore = 8f;
                tblMember.LockedWidth = true;


                var empty = new Phrase();
                empty.Add(new Chunk(" ", pdf.fnCalibri5));

                string address = (!String.IsNullOrEmpty(lstCust[0].Address) ? lstCust[0].Address : "") +
                               (!String.IsNullOrEmpty(lstCust[0].Area) ? " " + lstCust[0].Area : "") +
                               (!String.IsNullOrEmpty(lstCust[0].CityName) ? ", " + lstCust[0].CityName : "") +
                               (!String.IsNullOrEmpty(lstCust[0].StateName) ? ", " + lstCust[0].StateName : "") +
                                (!String.IsNullOrEmpty(lstCust[0].Pincode) ? " - " + lstCust[0].Pincode : "");

                string Org_Address =  lstOrg[0].Address + " , " + lstOrg[0].CityName + " - " + lstOrg[0].Pincode + " , " + lstOrg[0].StateName ;

                // -------------------------------------------------------------------------------------
                //  Defining : Customer  Detail

                Phrase CustomerName = new Phrase();
                Chunk c1 = new Chunk(lstQuot[0].CustomerName, pdf.fnCalibriBold18);
                c1.SetUnderline(1, -1);
                CustomerName.Add(c1);

                Phrase Address = new Phrase();
                Chunk c2 = new Chunk("Add:", pdf.fnCalibriBold14);
                c2.SetUnderline(1, -1);
                Address.Add(c2);

                Phrase Mo = new Phrase();
                Chunk c3 = new Chunk("MO:", pdf.fnCalibriBold14);
                c3.SetUnderline(1, -1);
                Mo.Add(c3);

                PdfPTable tblCustomerD = new PdfPTable(2);
                int[] column_tblNested20 = { 14, 86 };
                tblCustomerD.SetWidths(column_tblNested20);

                tblCustomerD.AddCell(pdf.setCell("To,", pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(CustomerName, pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(Address, pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(address, pdf.WhiteBaseColor, pdf.fnCalibriBold12, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(Mo, pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblCustomerD.AddCell(pdf.setCell(lstCust[0].ContactNo1, pdf.WhiteBaseColor, pdf.fnCalibriBold18, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                tblCustomerD.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri5, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                // -------------------------------------------------------------------------------------
                //  Defining : Organization  Detail

                PdfPTable tblorgadd = new PdfPTable(2);
                int[] column_tblorgadd = { 50, 50 };
                tblorgadd.SetWidths(column_tblorgadd);
                tblorgadd.AddCell(pdf.setCell("From,", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                tblorgadd.AddCell(pdf.setCell("MO." + lstOrg[0].Landline1, pdf.WhiteBaseColor, pdf.fnCalibriBold9, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblorgadd.AddCell(pdf.setCell(lstOrg[0].OrgName.ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));

                tblorgadd.AddCell(pdf.setCell(Org_Address, pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));


                tblMember.AddCell(pdf.setCell(tblCustomerD, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblMember.AddCell(pdf.setCell(tblorgadd, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BASELINE, 15));




                // ---------------------------------------------------
                int[] column_tblFooter = {100 };
                tblFooter.SetWidths(column_tblFooter);
                //tblFooter.SpacingBefore = 0f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_BOTTOM;
                tblFooter.AddCell(pdf.setCell(tblorgadd, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 15));
                // -------------------------------------------------------------------------------------

                #endregion


                htmlClose = "</body></html>";

                // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
                string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
                string sFileName = lstQuot[0].OutwardNo.Replace("/", "-").ToString() + ".pdf";
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

          

                // >>>>>> Adding Quotation Master Information Table
                tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfDoc.Add(tblMember);


                // >>>>>> Adding Quotation Footer
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