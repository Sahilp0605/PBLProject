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
using System.Reflection;
using iTextSharp.text.pdf.draw;

namespace StarsProject
{
    public partial class Quotation : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount, totTaxAmount, totNetAmount;
        //private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                totAmount = 0;
                totTaxAmount = 0;
                totNetAmount = 0;

                DataTable dtDetail = new DataTable();
                Session.Add("dtDetail", dtDetail);

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["dtSpecs"] = null;

                // --------------------------------------------------------
                BindDropDown();

                hdnQuotationVersion.Value = BAL.CommonMgmt.GetConstant("QuotationVersion", 0, 1);
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
                            hdnMode.Value = Request.QueryString["mode"].ToString();
                            if (hdnMode.Value.ToLower() == "view")
                                OnlyViewControls();
                        }
                    }
                }
                //-------------------------------------------------------
                // Loading Product Specification 
                //-------------------------------------------------------
                if (hdnMode.Value.ToLower() == "edit")
                {
                    DataTable dtSpecs = new DataTable();
                    List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
                    if (!String.IsNullOrEmpty(txtQuotationNo.Text) || txtQuotationNo.Text != "")
                    {
                        lst = BAL.ProductMgmt.GetQuotationProductSpecList(txtQuotationNo.Text, 0, Session["LoginUserID"].ToString());
                        if (lst.Count > 0)
                        {
                            dtSpecs = PageBase.ConvertListToDataTable(lst);
                            Session["dtSpecs"] = dtSpecs;
                        }
                    }
                }
            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];

                if (requestTarget.ToLower() == "drpinquiry")
                {
                    if ((hdnpkID.Value == "0" || hdnpkID.Value == "") && !String.IsNullOrEmpty(drpInquiry.SelectedValue))
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];
                        dtDetail.Clear();
                        dtDetail = BAL.InquiryInfoMgmt.GetInquiryProductForQuotation(drpInquiry.SelectedValue, txtQuotationNo.Text);
                        rptQuotationDetail.DataSource = dtDetail;
                        rptQuotationDetail.DataBind();
                        Session.Add("dtDetail", dtDetail);
                    }
                }
            }
        }

        public void OnlyViewControls()
        {
            divFollowUp.Visible = false;

            txtQuotationNo.ReadOnly = true;
            txtQuotationDate.ReadOnly = true;
            txtQuotationHeader.ReadOnly = true;
            txtQuotationFooter.ReadOnly = true;
            txtQuotationSubject.ReadOnly = true;
            //drpCustomer.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            drpInquiry.Attributes.Add("disabled", "disabled");
            drpQuotationKindAttn.Attributes.Add("disabled", "disabled");
            drpProjects.Attributes.Add("disabled", "disabled");
            drpTNC.Attributes.Add("disabled", "disabled");

            txtNextFollowupDate.ReadOnly = true;
            txtMeetingNotes.ReadOnly = true;
            drpFollowupType.Attributes.Add("disabled", "disabled");

            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;

            pnlDetail.Enabled = false;
        }

        public void BindDropDown()
        {
            // ---------------- Report To List -------------------------------------
            //List<Entity.Customer> lstOrgDept2 = new List<Entity.Customer>();
            //lstOrgDept2 = BAL.CustomerMgmt.GetCustomerList();
            //drpCustomer.DataSource = lstOrgDept2;
            //drpCustomer.DataValueField = "CustomerID";
            //drpCustomer.DataTextField = "CustomerName";
            //drpCustomer.DataBind();
            //drpCustomer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Customer --", ""));

            // ---------------- Report To List -------------------------------------
            List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            drpTNC.DataSource = lstList2;
            drpTNC.DataValueField = "pkID";
            drpTNC.DataTextField = "TNC_Header";
            drpTNC.DataBind();
            drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

            // ---------------- Report To List -------------------------------------
            int totrec = 0;
            List<Entity.Projects> lstList3 = new List<Entity.Projects>();
            lstList3 = BAL.ProjectsMgmt.GetProjectsList(0, "", 1, 50000, out totrec);
            drpProjects.DataSource = lstList3;
            drpProjects.DataValueField = "ProjectName";
            drpProjects.DataTextField = "ProjectName";
            drpProjects.DataBind();
            drpProjects.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Project --", ""));

            // ---------------- Report To List -------------------------------------
            List<Entity.InquiryStatus> lstOrgDept22 = new List<Entity.InquiryStatus>();
            lstOrgDept22 = BAL.InquiryStatusMgmt.GetInquiryStatusList("FollowupType");
            drpFollowupType.DataSource = lstOrgDept22;
            drpFollowupType.DataValueField = "pkID";
            drpFollowupType.DataTextField = "InquiryStatusName";
            drpFollowupType.DataBind();
            drpFollowupType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            // ---------------- Inquiry List -------------------------------------
            BindInquiryList(0);
        }

        public void BindCustomerContacts()
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                // ---------------- Report To List -------------------------------------
                int TotalCount = 0;
                List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
                lstObject = BAL.CustomerContactsMgmt.GetCustomerContactsList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                drpQuotationKindAttn.DataSource = lstObject;
                drpQuotationKindAttn.DataValueField = "ContactPerson1";
                drpQuotationKindAttn.DataTextField = "ContactPerson1";
                drpQuotationKindAttn.DataBind();
                drpQuotationKindAttn.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Contact --", ""));
            }
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        public void BindInquiryList(Int64 pCustomerID)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            drpInquiry.Items.Clear();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoListByCustomer(pCustomerID);
                // --------------------------------------------------
                lstEntity = lstEntity.Where(e => !(e.InquiryStatus == "Close - Lost" && e.InquiryStatus == "Close - Success")).ToList();
                // --------------------------------------------------
                drpInquiry.DataValueField = "InquiryNo";
                drpInquiry.DataTextField = "InquiryNo";
                if (lstEntity.Count > 0)
                {
                    drpInquiry.DataSource = lstEntity;
                    drpInquiry.DataBind();
                }
                drpInquiry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpInquiry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }

        public void BindQuotationDetailList(string pQuotationNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.QuotationDetailMgmt.GetQuotationDetail(pQuotationNo);
            rptQuotationDetail.DataSource = dtDetail1;
            rptQuotationDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void rptQuotationDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];

            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;
                    divErrorMessage.InnerHtml = "";

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0"
                        || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                    {
                        _pageValid = false;

                        divErrorMessage.Style.Remove("color");
                        divErrorMessage.Style.Add("color", "red");

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Product Selection is required." + "</li>"));

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Quantity is required." + "</li>"));

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Unit Rate is required." + "</li>"));

                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + ((HiddenField)e.Item.FindControl("hdnProductID")).Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
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
                        string unitrate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                        string disper = ((TextBox)e.Item.FindControl("txtDiscountPercent")).Text;
                        string netrate = ((TextBox)e.Item.FindControl("txtNetRate")).Text;
                        string amt = ((TextBox)e.Item.FindControl("txtAmount")).Text;
                        string taxrate = ((TextBox)e.Item.FindControl("txtTaxRate")).Text;
                        string taxamt = ((TextBox)e.Item.FindControl("txtTaxAmount")).Text;
                        string netamt = ((TextBox)e.Item.FindControl("txtNetAmount")).Text;
                        string bundleid = ((DropDownList)e.Item.FindControl("drpBundle1")).SelectedValue;

                        dr["QuotationNo"] = txtQuotationNo.Text;
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                        dr["UnitRate"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["UnitPrice"] = (!String.IsNullOrEmpty(unitrate)) ? Convert.ToDecimal(unitrate) : 0;
                        dr["DiscountPercent"] = (!String.IsNullOrEmpty(disper)) ? Convert.ToDecimal(disper) : 0;
                        dr["NetRate"] = (!String.IsNullOrEmpty(netrate)) ? Convert.ToDecimal(netrate) : 0;
                        dr["Amount"] = (!String.IsNullOrEmpty(amt)) ? Convert.ToDecimal(amt) : 0;
                        dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
                        dr["TaxAmount"] = (!String.IsNullOrEmpty(taxamt)) ? Convert.ToDecimal(taxamt) : 0;
                        dr["NetAmount"] = (!String.IsNullOrEmpty(netamt)) ? Convert.ToDecimal(netamt) : 0;
                        dr["BundleId"] = (!String.IsNullOrEmpty(bundleid)) ? Convert.ToInt64(bundleid) : 0;

                        dtDetail.Rows.Add(dr);
                        // ---------------------------------------------------------------
                        rptQuotationDetail.DataSource = dtDetail;
                        rptQuotationDetail.DataBind();
                        // ---------------------------------------------------------------
                        Session.Add("dtDetail", dtDetail);

                    }
                    btnSave.Focus();

                }
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
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
                            break;
                        }
                    }
                    rptQuotationDetail.DataSource = dtDetail;
                    rptQuotationDetail.DataBind();

                    Session.Add("dtDetail", dtDetail);
                    // -----------------------------------------------------
                    // Removing Specification Items
                    // -----------------------------------------------------
                    //string tmpCode = "";
                    //tmpCode = ((HiddenField)e.Item.FindControl("hdnItemCode")).Value;

                    //DataTable dtSpecs = new DataTable();
                    //if (Session["dtSpecs"] != null && !String.IsNullOrEmpty(tmpCode))
                    //{
                    //    dtSpecs = (DataTable)Session["dtSpecs"];


                    //    DataRow[] drr = dtSpecs.Select("QuotationNo = '" + txtQuotationNo.Text + "' AND FinishProductID = " + tmpCode);
                    //    foreach (var row in drr)
                    //        row.Delete();
                    //    dtSpecs.AcceptChanges();
                    //    Session.Add("dtSpecs", dtSpecs);
                    //}
                }
            }
        }

        protected void rptQuotationDetail_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2, v3;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Amount"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "TaxAmount"));
                v3 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "NetAmount"));

                totAmount += v1;
                totTaxAmount += v2;
                totNetAmount += v3;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotalGrossAmount = (Label)e.Item.FindControl("lblTotalGrossAmount");
                Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
                Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");

                lblTotalGrossAmount.Text = totAmount.ToString("0.00");
                lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
                lblTotalNetAmount.Text = totNetAmount.ToString("0.00");
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpBundle0"));
                ddl.DataSource = BindBundleList();
                ddl.DataValueField = "BundleId";
                ddl.DataTextField = "BundleName";
                ddl.DataBind();
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnBundle"));
                if (!String.IsNullOrEmpty(tmpField.Value))
                    ddl.SelectedValue = tmpField.Value;
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpBundle1"));
                ddl.DataSource = BindBundleList();
                ddl.DataValueField = "BundleId";
                ddl.DataTextField = "BundleName";
                ddl.DataBind();
                ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }
        public List<Entity.Bundle> BindBundleList()
        {
            // ---------------- Assign Employee ------------------------
            List<Entity.Bundle> lstEmployee = new List<Entity.Bundle>();
            lstEmployee = BAL.BundleMgmt.GetBundleList();
            return lstEmployee;

        }

        // ----------------------------------------------------------------------------------
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                divFollowUp.Visible = false;

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Quotation> lstEntity = new List<Entity.Quotation>();
                // ----------------------------------------------------
                lstEntity = BAL.QuotationMgmt.GetQuotationList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtQuotationNo.Text = lstEntity[0].QuotationNo;
                txtQuotationDate.Text = lstEntity[0].QuotationDate.ToString("dd-MM-yyyy");
                drpProjects.SelectedValue = lstEntity[0].ProjectName.ToString();
                //drpCustomer.SelectedValue = lstEntity[0].CustomerID.ToString();
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                BindCustomerContacts();
                // ------------------------------------------------------------
                BindInquiryList(lstEntity[0].CustomerID);
                // ------------------------------------------------------------
                txtQuotationSubject.Text = lstEntity[0].QuotationSubject;
                drpQuotationKindAttn.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].QuotationKindAttn)) ? lstEntity[0].QuotationKindAttn : "";
                drpInquiry.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].InquiryNo)) ? lstEntity[0].InquiryNo : "";
                txtQuotationHeader.Text = lstEntity[0].QuotationHeader;
                txtQuotationFooter.Text = lstEntity[0].QuotationFooter;
                // -------------------------------------------------------------------------
                BindQuotationDetailList(lstEntity[0].QuotationNo);
                txtCustomerName.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            // --------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnQuotationNo = "";
            Int64 ReturnFollowupPKID = 0;
            // --------------------------------------------------------------
            _pageValid = true;

            if (String.IsNullOrEmpty(txtQuotationDate.Text) || dtDetail.Rows.Count <= 0 ||
                (String.IsNullOrEmpty(txtCustomerName.Text) && String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0"))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text) && String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Customer Name is required." + "</li>"));

                if (String.IsNullOrEmpty(txtQuotationDate.Text))
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Quotation Date is required." + "</li>"));

                if (dtDetail.Rows.Count <= 0)
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Atleast One Item is required to save Quotation." + "</li>"));
            }

            if (divFollowUp.Visible == true)
            {
                Boolean cc = false;

                String val1 = txtNextFollowupDate.Text.Trim();
                String val2 = txtMeetingNotes.Text.Trim();
                String val3 = drpFollowupType.SelectedValue;
                if (val1 == "" && val2 == "" && val3 == "")
                    cc = true;
                else if (val1 != "" && val2 != "" && val3 != "")
                    cc = true;
                else
                    cc = false;

                //(!String.IsNullOrEmpty(txtNextFollowupDate.Text) || !String.IsNullOrEmpty(txtMeetingNotes.Text) || !String.IsNullOrEmpty(drpFollowupType.SelectedValue))
                if (cc == false)
                {
                    _pageValid = false;
                    divErrorMessage.Controls.Add(new LiteralControl("<li>" + "All Information required to Auto Generate Followup" + "</li>"));
                }

            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Quotation objEntity = new Entity.Quotation();
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                        objEntity.InquiryNo = drpInquiry.SelectedValue;
                        objEntity.QuotationNo = txtQuotationNo.Text;
                        objEntity.QuotationDate = Convert.ToDateTime(txtQuotationDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.ProjectName = drpProjects.SelectedValue;
                        objEntity.QuotationSubject = txtQuotationSubject.Text;
                        objEntity.QuotationKindAttn = drpQuotationKindAttn.SelectedValue;
                        objEntity.QuotationHeader = txtQuotationHeader.Text;
                        objEntity.QuotationFooter = txtQuotationFooter.Text;

                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.QuotationMgmt.AddUpdateQuotation(objEntity, out ReturnCode, out ReturnMsg, out ReturnQuotationNo);
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnQuotationNo) && !String.IsNullOrEmpty(txtQuotationNo.Text))
                        {
                            ReturnQuotationNo = txtQuotationNo.Text;
                        }
                        divErrorMessage.InnerHtml = ((ReturnCode > 0) ? ReturnQuotationNo + " " + ReturnMsg : ReturnMsg);
                        // --------------------------------------------------------------
                        #region >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnQuotationNo))
                        {
                            txtQuotationNo.Text = ReturnQuotationNo;
                            btnSave.Enabled = false;
                            btnSaveEmail.Enabled = false;


                            // ------------------------------------------------------------------
                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            BAL.QuotationDetailMgmt.DeleteQuotationDetailByQuotationNo(ReturnQuotationNo, out ReturnCode1, out ReturnMsg1);

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                Entity.QuotationDetail objQuotDet = new Entity.QuotationDetail();

                                objQuotDet.pkID = 0;
                                objQuotDet.QuotationNo = ReturnQuotationNo;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.UnitRate = Convert.ToDecimal(dr["UnitRate"]);
                                objQuotDet.DiscountPercent = Convert.ToDecimal(dr["DiscountPercent"]);
                                objQuotDet.NetRate = Convert.ToDecimal(dr["NetRate"]);
                                objQuotDet.Amount = Convert.ToDecimal(dr["Amount"]);
                                objQuotDet.TaxRate = Convert.ToDecimal(dr["TaxRate"]);
                                objQuotDet.TaxAmount = Convert.ToDecimal(dr["TaxAmount"]);
                                objQuotDet.NetAmount = Convert.ToDecimal(dr["NetAmount"]);
                                if ((!String.IsNullOrEmpty(dr["BundleId"].ToString())) && Convert.ToInt64(dr["BundleId"]) > 0)
                                    objQuotDet.BundleId = Convert.ToInt64(dr["BundleId"]);
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.QuotationDetailMgmt.AddUpdateQuotationDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            #region Direct_Control_Values
                            // -------------------------------------------------------
                            //Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
                            //string ctrl1 = ((TextBox)rptFootCtrl.FindControl("txtQuantity")).Text;
                            //string ctrl2 = ((TextBox)rptFootCtrl.FindControl("txtNetAmount")).Text;
                            //string ctrl3 = ((TextBox)rptFootCtrl.FindControl("txtProductName")).Text;
                            //string ctrl4 = ((HiddenField)rptFootCtrl.FindControl("hdnProductID")).Value;

                            //if (!String.IsNullOrEmpty(ctrl1) && !String.IsNullOrEmpty(ctrl2) && !String.IsNullOrEmpty(ctrl3) && (!String.IsNullOrEmpty(ctrl4) && ctrl4 != "0"))
                            //{
                            //    objQuotDet.pkID = 0;
                            //    objQuotDet.QuotationNo = txtQuotationNo.Text;
                            //    objQuotDet.ProductID = Convert.ToInt64(((HiddenField)rptFootCtrl.FindControl("hdnProductID")).Value);

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtQuantity")).Text))
                            //        objQuotDet.Quantity = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtQuantity")).Text);
                            //    else
                            //        objQuotDet.Quantity = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtUnit")).Text))
                            //        objQuotDet.Unit = ((TextBox)rptFootCtrl.FindControl("txtUnit")).Text;
                            //    else
                            //        objQuotDet.Unit = "";

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtUnitRate")).Text))
                            //        objQuotDet.UnitRate = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtUnitRate")).Text);
                            //    else
                            //        objQuotDet.UnitRate = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtDiscountPercent")).Text))
                            //        objQuotDet.DiscountPercent = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtDiscountPercent")).Text);
                            //    else
                            //        objQuotDet.DiscountPercent = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtNetRate")).Text))
                            //        objQuotDet.NetRate = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtNetRate")).Text);
                            //    else
                            //        objQuotDet.NetRate = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtAmount")).Text))
                            //        objQuotDet.Amount = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtAmount")).Text);
                            //    else
                            //        objQuotDet.Amount = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtTaxRate")).Text))
                            //        objQuotDet.TaxRate = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtTaxRate")).Text);
                            //    else
                            //        objQuotDet.TaxRate = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtTaxAmount")).Text))
                            //        objQuotDet.TaxAmount = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtTaxAmount")).Text);
                            //    else
                            //        objQuotDet.TaxAmount = 0;

                            //    if (!String.IsNullOrEmpty(((TextBox)rptFootCtrl.FindControl("txtNetAmount")).Text))
                            //        objQuotDet.NetAmount = Convert.ToDecimal(((TextBox)rptFootCtrl.FindControl("txtNetAmount")).Text);
                            //    else
                            //        objQuotDet.NetAmount = 0;

                            //    objQuotDet.LoginUserID = Session["LoginUserID"].ToString();


                            //    BAL.QuotationDetailMgmt.AddUpdateQuotationDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            //}
                            #endregion
                            // --------------------------------------------------------------
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                            }
                        }

                        #endregion

                        // --------------------------------------------------------------
                        // Adding FollowUp from Quotation
                        // --------------------------------------------------------------
                        if (ReturnCode > 0 && objEntity.pkID == 0)
                        {
                            if (!String.IsNullOrEmpty(txtNextFollowupDate.Text) && !String.IsNullOrEmpty(txtMeetingNotes.Text) && !String.IsNullOrEmpty(drpFollowupType.SelectedValue))
                            {
                                Entity.Followup objFollow = new Entity.Followup();
                                objFollow.FollowupDate = System.DateTime.Now;
                                objFollow.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                                objFollow.InquiryNo = drpInquiry.SelectedValue;
                                objFollow.MeetingNotes = txtMeetingNotes.Text;
                                objFollow.NextFollowupDate = Convert.ToDateTime(txtNextFollowupDate.Text);
                                objFollow.InquiryStatusID = (!String.IsNullOrEmpty(drpFollowupType.SelectedValue)) ? Convert.ToInt64(drpFollowupType.SelectedValue) : Convert.ToInt64("0");
                                objFollow.Rating = 1;
                                objFollow.LoginUserID = Session["LoginUserID"].ToString();
                                BAL.FollowupMgmt.AddUpdateFollowup(objFollow, out ReturnCode1, out ReturnMsg1, out ReturnFollowupPKID);
                            }
                        }
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Detail Specification 
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        int ReturnCode91 = 0;
                        string ReturnMsg91 = "";
                        if (ReturnCode > 0)
                        {
                            DataTable dtSpecs = new DataTable();
                            dtSpecs = (DataTable)Session["dtSpecs"];
                            if (dtSpecs != null)
                            {
                                foreach (DataRow dr in dtSpecs.Rows)
                                {
                                    if (dr.RowState.ToString() != "Deleted")
                                    {
                                        Entity.ProductDetailCard lstObject = new Entity.ProductDetailCard();

                                        if (!String.IsNullOrEmpty(dr["pkID"].ToString()) && dr["pkID"].ToString() != "0")
                                            lstObject.pkID = Convert.ToInt64(dr["pkID"].ToString());
                                        else
                                            lstObject.pkID = 0;
                                        lstObject.QuotationNo = ReturnQuotationNo;
                                        lstObject.FinishProductID = Convert.ToInt64(dr["FinishProductID"].ToString());
                                        lstObject.GroupHead = dr["GroupHead"].ToString();
                                        lstObject.MaterialHead = dr["MaterialHead"].ToString();
                                        lstObject.MaterialSpec = dr["MaterialSpec"].ToString();
                                        lstObject.LoginUserID = Session["LoginUserID"].ToString();
                                        // -------------------------------------------------------------- Insert/Update Record
                                        BAL.ProductMgmt.AddUpdateQuotationProductSpec(lstObject, out ReturnCode91, out ReturnMsg91);
                                    }
                                }
                            }
                            // ---------------------------------------------------
                            // Removing Unwanted Specification
                            // ---------------------------------------------------
                            BAL.ProductMgmt.DeleteUnwantedSpec(ReturnQuotationNo);


                        }
                        // ==============================================================
                        Int64 ppkID = 0;

                        if (ReturnCode > 0)
                        {
                            // Fetching "QuotationNo" using pkID ...
                            ppkID = BAL.CommonMgmt.GetQuotationNoPrimaryID(ReturnQuotationNo);

                            // Generating Quotation PDF file ...
                            GenerateQuotation(ppkID);
                            //GenerateQuotationEagle(ppkID);
                        }

                        // --------------------------------------------------------------
                        if (paraSaveAndEmail)
                        {
                            Entity.Authenticate objAuth = new Entity.Authenticate();
                            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                            String sendEmailFlag = BAL.CommonMgmt.GetConstant("QT-EMAIL", 0, objAuth.CompanyID).ToLower();
                            if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                            {
                                btnSave.Enabled = false;
                                try
                                {
                                    // Sending Email Notification ...
                                    String respVal = "";
                                    if (ppkID > 0)
                                    {
                                        String tmpEmailAddress = "";
                                        tmpEmailAddress = (objEntity.CustomerID > 0) ? BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID) : "";
                                        if (!String.IsNullOrEmpty(tmpEmailAddress) && tmpEmailAddress.ToUpper() != "NULL")
                                            //respVal = BAL.QuotationMgmt.SendQuotationEmail("QUOTATION", Session["LoginUserID"].ToString(), ppkID, "");
                                            respVal = BAL.CommonMgmt.SendEmailNotifcation("QUOTATION", Session["LoginUserID"].ToString(), ppkID, tmpEmailAddress);
                                    }
                                    divErrorMessage.InnerHtml = ((ReturnCode > 0) ? ReturnQuotationNo + " " + ReturnMsg : ReturnMsg) + " and Email Sent Successfully !";
                                }
                                catch (Exception ex)
                                {
                                    divErrorMessage.InnerHtml = ((ReturnCode > 0) ? ReturnQuotationNo + " " + ReturnMsg : ReturnMsg) + " and Sending Email Failed !";
                                }
                            }
                        }
                    }
                }
                else
                {
                    divErrorMessage.InnerHtml = "Atleast One Item is required to save Quotation Information !";
                }
            }

        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtQuotationDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            txtQuotationNo.Text = ""; // BAL.CommonMgmt.GetQuotationNo(txtQuotationDate.Text);
            txtQuotationHeader.Text = "";
            txtQuotationFooter.Text = "";
            txtQuotationSubject.Text = "";
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpQuotationKindAttn.SelectedValue = "";
            drpProjects.SelectedValue = "";
            drpInquiry.Items.Clear();
            drpInquiry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            txtNextFollowupDate.Text = "";
            txtMeetingNotes.Text = "";
            drpFollowupType.SelectedValue = "";

            BindQuotationDetailList("");
            txtCustomerName.Focus();
        }

        //protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int totalrecord;

        //    Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
        //    string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
        //    TextBox txUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
        //    TextBox txUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
        //    TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
        //    TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));

        //    List<Entity.Products> lstEntity = new List<Entity.Products>();

        //    if (!String.IsNullOrEmpty(ctrl1))
        //        lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]),  out totalrecord);

        //    txUnit.Text = (lstEntity.Count>0) ? lstEntity[0].Unit : "";
        //    txUnitRate.Text = (lstEntity.Count>0) ? lstEntity[0].UnitPrice.ToString() : "0";
        //    txtDiscountPercent.Text = "0";
        //    txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
        //}

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptQuotationDetail.Controls[rptQuotationDetail.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscountPercent = ((TextBox)rptFootCtrl.FindControl("txtDiscountPercent"));
            TextBox txtTaxRate = ((TextBox)rptFootCtrl.FindControl("txtTaxRate"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            if (!String.IsNullOrEmpty(hdnProductID.Value) && hdnProductID.Value != "0")
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
            txtUnitRate.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            txtDiscountPercent.Text = "0";
            txtTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
            txtQuantity.Focus();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                BindInquiryList(Convert.ToInt64(hdnCustomerID.Value));
            else
                BindInquiryList(0);
            // ---------------------------------------------
            BindCustomerContacts();
            // ---------------------------------------------
            drpInquiry.Focus();
        }

        protected void drpTNC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpTNC.SelectedValue) && drpTNC.SelectedValue != "0")
            {
                string tmpval = drpTNC.SelectedValue;
                List<Entity.Contents> lstList2 = new List<Entity.Contents>();
                lstList2 = BAL.CommonMgmt.GetContentList(Convert.ToInt64(drpTNC.SelectedValue), "TNC");
                if (lstList2.Count > 0)
                    txtQuotationFooter.Text = lstList2[0].TNC_Content;

            }
        }


        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            ///////---------------
            if (sender.ToString() == "System.Web.UI.WebControls.DropDownList")
            {
                DropDownList edSender = (DropDownList)sender;

                var item = (RepeaterItem)edSender.NamingContainer;
                HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
                DropDownList drpBundle0 = (DropDownList)item.FindControl("drpBundle0");
                Int64 drpBundle = ((!String.IsNullOrEmpty(drpBundle0.SelectedValue.ToString())) && Convert.ToInt64(drpBundle0.SelectedValue) > 0) ? Convert.ToInt64(drpBundle0.SelectedValue) : 0;
                drpBundle0.SelectedValue = drpBundle.ToString();

                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];

                foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                foreach (DataRow row in dtDetail.Rows)
                {
                    if (row["ProductName"].ToString() == edProductName.Value)
                    {

                        row.SetField("BundleId", drpBundle0.SelectedValue);
                    }
                }
                rptQuotationDetail.DataSource = dtDetail;
                rptQuotationDetail.DataBind();

                Session.Add("dtDetail", dtDetail);


            }
            else if (sender.ToString() == "System.Web.UI.WebControls.TextBox")
            {
                TextBox edSender = (TextBox)sender;

                var item = (RepeaterItem)edSender.NamingContainer;

                HiddenField edProductName = (HiddenField)item.FindControl("edProductName");
                TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
                TextBox edUnitRate = (TextBox)item.FindControl("edUnitRate");
                TextBox edDiscountPercent = (TextBox)item.FindControl("edDiscountPercent");
                TextBox edNetRate = (TextBox)item.FindControl("edNetRate");
                TextBox edAmount = (TextBox)item.FindControl("edAmount");
                TextBox edTaxRate = (TextBox)item.FindControl("edTaxRate");
                TextBox edTaxAmount = (TextBox)item.FindControl("edTaxAmount");
                TextBox edNetAmount = (TextBox)item.FindControl("edNetAmount");

                // --------------------------------------------------------------------------
                Decimal q = Convert.ToDecimal(edQuantity.Text);
                Decimal ur = Convert.ToDecimal(edUnitRate.Text);
                Decimal dp = Convert.ToDecimal(edDiscountPercent.Text);
                Decimal nr = Convert.ToDecimal(edNetRate.Text);
                Decimal a = Convert.ToDecimal(edAmount.Text);
                Decimal tr = Convert.ToDecimal(edTaxRate.Text);
                Decimal ta = Convert.ToDecimal(edTaxAmount.Text);
                Decimal na = Convert.ToDecimal(edNetAmount.Text);

                // --------------------------------------------------------------------------
                if (q > 0 && ur > 0 && dp >= 0 && tr >= 0)
                {
                    nr = Math.Round((ur - ((ur * dp) / 100)), 2);
                    a = Math.Round((q * nr), 2);
                    ta = Math.Round(((a * tr) / 100), 2);
                    na = Math.Round((a + ta), 2);

                    edUnitRate.Text = ur.ToString();
                    edDiscountPercent.Text = dp.ToString();
                    edNetRate.Text = nr.ToString();
                    edAmount.Text = a.ToString();
                    edTaxRate.Text = tr.ToString();
                    edTaxAmount.Text = ta.ToString();
                    edNetAmount.Text = na.ToString();

                    // --------------------------------------------------------------------------
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];

                    foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

                    foreach (DataRow row in dtDetail.Rows)
                    {
                        if (row["ProductName"].ToString() == edProductName.Value)
                        {
                            row.SetField("Quantity", edQuantity.Text);
                            row.SetField("UnitRate", edUnitRate.Text);
                            row.SetField("DiscountPercent", edDiscountPercent.Text);
                            row.SetField("NetRate", edNetRate.Text);
                            row.SetField("Amount", edAmount.Text);
                            row.SetField("TaxRate", edTaxRate.Text);
                            row.SetField("TaxAmount", edTaxAmount.Text);
                            row.SetField("NetAmount", edNetAmount.Text);
                        }
                    }
                    rptQuotationDetail.DataSource = dtDetail;
                    rptQuotationDetail.DataBind();

                    Session.Add("dtDetail", dtDetail);
                }
            }
        }


        [System.Web.Services.WebMethod]
        public static string DeleteQuotation(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.QuotationMgmt.DeleteQuotation(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }


        [WebMethod]
        public static string GenerateQuotationRevision(long pkID)
        {
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> dictionaryList = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            int ReturnCode = 0;
            string ReturnMsg = "";
            BAL.QuotationMgmt.AddUpdateQuotationRevision(pkID, HttpContext.Current.Session["LoginUserID"].ToString(), out ReturnCode, out ReturnMsg);
            dictionary.Add("ReturnCode", (object)ReturnCode);
            dictionary.Add("ReturnMsg", (object)ReturnMsg);
            dictionaryList.Add(dictionary);
            return scriptSerializer.Serialize((object)dictionaryList);
        }

        // ====================================================================================
        // PDF Generation .....
        // ====================================================================================
        private static byte[] showQRCode(string pQuotationNo, string pCustomerName)
        {

            byte[] returnValue;
            PageBase pb = new PageBase();
            string code = pQuotationNo + "," + pCustomerName;
            //string encCode = pb.Encrypt(code, "r0b1nr0y");
            string encCode = code;

            qrGenerator = new QRCodeGenerator();

            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(encCode, QRCodeGenerator.ECCLevel.Q);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

            imgBarCode.Style.Add("top", "-15px !important");
            imgBarCode.Style.Add("height", "150px !important");
            imgBarCode.Style.Add("width", "150px !important");
            imgBarCode.Style.Add("margin-top", "10px !important");
            imgBarCode.Style.Add("float", "right");
            imgBarCode.Style.Add("position", "relative");
            imgBarCode.Style.Add("padding-right", "10px");

            using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    returnValue = byteImage;
                }
            }
            return returnValue;
        }

        private static QRCodeGenerator qrGenerator;

        public static iTextSharp.text.Image imgQRCode;

        [WebMethod]
        public static void GenerateQuotation(Int64 pQuotID)
        {

            String tmpVer = BAL.CommonMgmt.GetConstant("QuotationVersion", 0, 1);
            // ===========================================================================================
            PdfPCell cell;
            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(4);
            PdfPTable tblDetail = new PdfPTable(8);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(1);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            string htmlOpen = "", htmlClose = "";
            // ===========================================================================================
            float paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf10 = 10;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var GreenBaseColor = new BaseColor(60, 179, 113);
            var WhiteBaseColor = new BaseColor(255, 255, 255);
            var BlackBaseColor = new BaseColor(0, 0, 0);
            var GrayBaseColor = new BaseColor(238, 238, 224);
            var NavyBaseColor = new BaseColor(0, 0, 128);
            var LightBlueBaseColor = new BaseColor(236, 240, 241);
            var OrangeBaseColor = new BaseColor(243, 156, 18);

            iTextSharp.text.Font objHeaderFont12 = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objHeaderFont14 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objHeaderFont16 = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objHeaderFont18 = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            iTextSharp.text.Font objHeaderFontWhite14 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font objHeaderFontWhite16 = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font objHeaderFontWhite18 = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

            iTextSharp.text.Font objFooterFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);

            iTextSharp.text.Font objContentFontTitleBlack = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font objContentFontDataBlack = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font objContentFontTitleWhite = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font objContentFontDataWhite = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
            iTextSharp.text.Font objContentFontTitleNavy = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, NavyBaseColor);
            iTextSharp.text.Font objContentFontDataNavy = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, NavyBaseColor);
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(30, 30, 40, 0);
            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;
            List<Entity.Quotation> lstQuot = new List<Entity.Quotation>();
            lstQuot = BAL.QuotationMgmt.GetQuotationList(pQuotID, "", 1, 1000, out TotalCount);
            // -------------------------------------------------------------------------------------------------------------
            DataTable dtItem = new DataTable();
            if (lstQuot.Count > 0)
                dtItem = BAL.QuotationDetailMgmt.GetQuotationDetail(lstQuot[0].QuotationNo);
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Section : QRCode
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            //if (!String.IsNullOrEmpty(lstQuot[0].QuotationNo))
            //{

            //    byte[] tmpVal = showQRCode(lstQuot[0].QuotationNo, lstQuot[0].CustomerName);
            //    MemoryStream ms1 = new MemoryStream(tmpVal);
            //    System.Drawing.Image sdi = System.Drawing.Image.FromStream(ms1);
            //    imgQRCode = iTextSharp.text.Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Png);
            //    //imgQRCode = new iTextSharp.text.Image.GetInstance(tmpVal);
            //}
            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";
            if (lstQuot.Count > 0)
            {
                // https://www.coderanch.com/how-to/javadoc/itext-2.1.7/constant-values.html#com.lowagie.text.Rectangle.RIGHT
                // -------------------------------------------------------------------------------------
                //  Defining : Header Table 
                // -------------------------------------------------------------------------------------
                //tableHeader.DefaultCell.Border = PdfPCell.NO_BORDER;
                //tableHeader.DefaultCell.CellEvent = new RoundedBorder();
                //tableHeader.SpacingBefore = 10f;
                //tableHeader.LockedWidth = true;
                //tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

                //PdfPTable tblNested1 = new PdfPTable(1);
                //int[] column_tblNested1 = { 100 };
                //tblNested1.SetWidths(column_tblNested1);
                //// --------------------------------------------------------------------
                List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
                lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
                //String x1 = lstEntity[0].Address;

                //String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                //            ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                //            ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + ", India";

                //String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") +
                //            ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? " Email Us : " + lstEntity[0].EmailAddress : "");
                //// --------------------------------------------------------------------
                //cell = new PdfPCell(new Paragraph(lstEntity[0].OrgName, objHeaderFont18));
                //cell.VerticalAlignment = Element.ALIGN_TOP;
                //cell.PaddingLeft = 2;
                //cell.PaddingTop = 8;
                //cell.Border = 0;
                //tblNested1.AddCell(cell);
                //tblNested1.AddCell(setCell(x1, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //tblNested1.AddCell(setCell(x2, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                //tblNested1.AddCell(setCell(x3, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));

                //cell = new PdfPCell(tblNested1);
                //cell.VerticalAlignment = Element.ALIGN_TOP;
                //cell.Border = PdfPCell.BOTTOM_BORDER;
                //cell.PaddingBottom = 5;
                //tableHeader.AddCell(cell);
                //// ---------------------------------------------------------------------------
                //imgQRCode.SetAbsolutePosition(pdfDoc.GetRight(80), pdfDoc.GetTop(90));
                //imgQRCode.ScaleToFit(80f, 80f);
                //imgQRCode.Alignment = Element.ALIGN_RIGHT;
                //cell = new PdfPCell(imgQRCode);
                //cell.Border = PdfPCell.BOTTOM_BORDER;
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.PaddingBottom = 5;
                //tableHeader.AddCell(cell);
                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Master Data Information
                // -------------------------------------------------------------------------------------
                #region Section >>>> Quotation Master Information
                tblMember.SpacingBefore = 15f;
                tblMember.LockedWidth = true;
                tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
                tblMember.DefaultCell.Border = PdfPCell.NO_BORDER;

                tblMember.AddCell(setCell("Quotation # : " + lstQuot[0].QuotationNo.ToString(), GreenBaseColor, objHeaderFontWhite14, paddingOf8, 2, Element.ALIGN_LEFT));
                tblMember.AddCell(setCell("Date of Quotation : " + lstQuot[0].QuotationDate.ToString("dd-MMM-yyyy"), GreenBaseColor, objHeaderFontWhite14, paddingOf8, 2, Element.ALIGN_LEFT));

                PdfPTable tblNested20 = new PdfPTable(1);
                int[] column_tblNested20 = { 100 };
                tblNested20.SetWidths(column_tblNested20);

                cell = new PdfPCell(new Paragraph("To,", objContentFontTitleBlack));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingLeft = 8;
                cell.PaddingTop = 5;
                cell.Border = 12;
                tblNested20.AddCell(cell);

                if (!String.IsNullOrEmpty(lstQuot[0].QuotationKindAttn))
                {
                    cell = new PdfPCell(new Paragraph(lstQuot[0].QuotationKindAttn, objHeaderFont12));
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.PaddingLeft = 8;
                    cell.PaddingTop = 2;
                    cell.Border = 12;
                    tblNested20.AddCell(cell);
                }
                cell = new PdfPCell(new Paragraph(lstQuot[0].CustomerName));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingLeft = 8;
                cell.PaddingTop = 8;
                cell.Border = 12;
                tblNested20.AddCell(cell);
                cell = new PdfPCell(new Paragraph(lstQuot[0].Address + "," + lstQuot[0].Area + "," + lstQuot[0].City + "," + lstQuot[0].PinCode));
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingLeft = 8;
                cell.PaddingTop = 2;
                cell.PaddingBottom = 8;
                cell.Border = 12;
                tblNested20.AddCell(cell);


                //tblNested20.AddCell(setCell("To,", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblNested20.AddCell(setCell(lstQuot[0].CustomerName, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //tblNested20.AddCell(setCell(lstQuot[0].Address + "," + lstQuot[0].Area + "," + lstQuot[0].City + "," + lstQuot[0].PinCode, WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                PdfPTable tblNested2 = new PdfPTable(2);
                int[] column_tblNested2 = { 40, 60 };
                tblNested2.SetWidths(column_tblNested2);
                tblNested2.AddCell(setCell("Contact # ", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(setCell(lstQuot[0].ContactNo1, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(setCell("Email Address", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(setCell(lstQuot[0].EmailAddress, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(setCell("Project Name", WhiteBaseColor, objContentFontTitleBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblNested2.AddCell(setCell(lstQuot[0].ProjectName, WhiteBaseColor, objContentFontDataBlack, paddingOf8, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblMember.AddCell(setCell(tblNested20, WhiteBaseColor, objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE));
                tblMember.AddCell(setCell(tblNested2, WhiteBaseColor, objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Subject
                // -------------------------------------------------------------------------------------
                #region Section >>>> Subject 
                tblSubject.SpacingBefore = 15f;
                tblSubject.LockedWidth = true;
                tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                tblSubject.AddCell(setCell("Subject : " + lstQuot[0].QuotationSubject, WhiteBaseColor, objHeaderFont12, paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Header                
                // -------------------------------------------------------------------------------------
                #region Section >>>> Header Information
                tblHeader.SpacingBefore = 8f;
                tblHeader.LockedWidth = true;
                tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                tblHeader.AddCell(setCell(lstQuot[0].QuotationHeader, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Detail
                // -------------------------------------------------------------------------------------
                #region Section >>>> QuotationDetail

                int[] column_tblNested = { 5, 38, 12, 8, 12, 10, 12, 13 };
                tblDetail.SetWidths(column_tblNested);
                tblDetail.SpacingBefore = 5f;
                tblDetail.LockedWidth = true;
                tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

                tblDetail.AddCell(setCell("#", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                tblDetail.AddCell(setCell("Product Name", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                tblDetail.AddCell(setCell("Quantity", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Unit", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                tblDetail.AddCell(setCell("Unit Rate", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Net Rate", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                tblDetail.AddCell(setCell("Tax Rate", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                tblDetail.AddCell(setCell("Amount", LightBlueBaseColor, objContentFontTitleBlack, paddingOf3, 1, Element.ALIGN_RIGHT));

                decimal totAmount = 0, taxAmt = 0, netAmt = 0;
                String tmpGroup = "";
                int rowCount = 1;
                if (dtItem.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(dtItem.Rows[0]["BundleName"].ToString()))
                    {
                        dtItem.DefaultView.Sort = "BundleName asc";
                        dtItem = dtItem.DefaultView.ToTable(true);
                    }
                }
                decimal basicAmt = 0;

                for (int i = 0; i < dtItem.Rows.Count; i++)
                {
                    if (!String.IsNullOrEmpty(dtItem.Rows[i]["BundleName"].ToString()))
                    {
                        if (String.IsNullOrEmpty(tmpGroup) || tmpGroup != dtItem.Rows[i]["BundleName"].ToString())
                        {
                            if (basicAmt > 0)
                            {
                                tblDetail.AddCell(setCell("Package Total Amount :", WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                                tblDetail.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                                tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 8, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 3));
                                totAmount = 0;
                            }

                            // ---------------------------------------------------
                            tblDetail.AddCell(setCell("Offered Package : " + dtItem.Rows[i]["BundleName"].ToString(), WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 8, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE));
                            rowCount = 1;
                        }
                    }
                    // ------------------------------------------------------------------
                    basicAmt = (Convert.ToDecimal(dtItem.Rows[i]["Quantity"]) * Convert.ToDecimal(dtItem.Rows[i]["NetRate"]));
                    totAmount += Convert.ToDecimal(dtItem.Rows[i]["Amount"].ToString());
                    taxAmt += Convert.ToDecimal(dtItem.Rows[i]["TaxAmount"].ToString());
                    netAmt += Convert.ToDecimal(dtItem.Rows[i]["NetAmount"].ToString());
                    // ------------------------------------------------------------------
                    tblDetail.AddCell(setCell(rowCount.ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                    if (!String.IsNullOrEmpty(dtItem.Rows[i]["HSNCode"].ToString()))
                        tblDetail.AddCell(setCell(dtItem.Rows[i]["ProductName"].ToString() + " - [ HSN Code - " + dtItem.Rows[i]["HSNCode"].ToString() + " ]", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                    else
                        tblDetail.AddCell(setCell(dtItem.Rows[i]["ProductName"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["Quantity"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["Unit"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["UnitRate"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["NetRate"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    tblDetail.AddCell(setCell(dtItem.Rows[i]["TaxRate"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                    tblDetail.AddCell(setCell(basicAmt.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT));
                    if (!String.IsNullOrEmpty(dtItem.Rows[i]["ProductSpecification"].ToString()))
                    {
                        tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_CENTER));
                        tblDetail.AddCell(setCell(dtItem.Rows[i]["ProductSpecification"].ToString(), WhiteBaseColor, objContentFontDataBlack, paddingOf5, 6, Element.ALIGN_LEFT));
                        tblDetail.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_LEFT));
                    }

                    // ------------------------------------------------------------------
                    rowCount++;
                    tmpGroup = dtItem.Rows[i]["BundleName"].ToString();

                }
                PageBase pb = new PageBase();
                string amountinwords = pb.ConvertToWords(totAmount.ToString("0.##"));
                // ---------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(dtItem.Rows[0]["BundleName"].ToString()))
                {
                    tblDetail.AddCell(setCell("Package Total Amount :", WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                    tblDetail.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                }
                else
                {
                    tblDetail.AddCell(setCell("Gross Amount :", WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                    tblDetail.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                }
                // ---------------------------------------------------------------------------
                tblDetail.AddCell(setCell("Tax Amount :", WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(setCell(taxAmt.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));
                tblDetail.AddCell(setCell("Net Amount :", WhiteBaseColor, objContentFontTitleBlack, paddingOf3, 7, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 6));
                tblDetail.AddCell(setCell(netAmt.ToString("0.00"), WhiteBaseColor, objContentFontDataBlack, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 10));

                //tblDetail.AddCell(setCell("Rs. :" + amountinwords, WhiteBaseColor, objContentFontDataBlack, paddingOf3, 6, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                #endregion

                // -------------------------------------------------------------------------------------
                //  Defining : Quotation Footer
                // -------------------------------------------------------------------------------------
                #region Section >>>> Terms & Condition

                tblFooter.SpacingBefore = 8f;
                tblFooter.LockedWidth = true;
                tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                tblFooter.AddCell(setCell("Terms & Conditions:", WhiteBaseColor, objContentFontTitleBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                tblFooter.AddCell(setCell(lstQuot[0].QuotationFooter, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

                // -------------------------------------------------------------------------------------
                //  Defining : Sign Off
                // -------------------------------------------------------------------------------------
                tblSignOff.SpacingBefore = 10f;
                tblSignOff.LockedWidth = true;
                tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                // ----------------------------------------------------
                //String tmpAuthorizedSign = BAL.CommonMgmt.GetAuthorizedSignUserID(lstQuot[0].CreatedBy);
                // ----------------------------------------------------
                //tblSignOff.AddCell(setCell(tmpAuthorizedSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblSignOff.AddCell(setCell("Thanks & Regards", WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblSignOff.AddCell(setCell("For, " + lstEntity[0].OrgName, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblSignOff.AddCell(setCell(lstQuot[0].CreatedEmployeeName, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


                #endregion
            }

            htmlClose = "</body></html>";
            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*

            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            //string sFileName = "Quotation_" + lstQuot[0].pkID.ToString() + ".pdf";
            string sFileName = lstQuot[0].QuotationNo.Replace("/", "-").ToString() + ".pdf";


            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            //clHeaderFooter.Title = lstEntity[0].OrgName;
            clHeaderFooter.HeaderFont = objHeaderFont18;
            clHeaderFooter.FooterFont = objFooterFont;
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
            //objStyle.LoadTagStyle("div", "border", "1");
            //objStyle.LoadTagStyle("div", "color", "black");
            //objStyle.LoadTagStyle("div", "margin-top", "10");
            //objStyle.LoadTagStyle("div", "margin-bottom", "10");

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

            // >>>>>> Adding Quotation Master Information Table
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblDetail);

            // >>>>>> Adding Quotation Header
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblFooter);

            // >>>>>> Adding Quotation Header
            tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSignOff);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // -------------------------------------------------------------------------------
            //pdfw.DirectContent.MoveTo(0, pdfDoc.PageSize.GetTop(35));
            //pdfw.DirectContent.LineTo(pdfDoc.PageSize.Width, pdfDoc.PageSize.GetTop(35));
            //pdfw.DirectContent.SetLineWidth(2);
            //pdfw.DirectContent.Stroke();

            // >>>>>> Adding Wallpaper
            //addWallPaper(pdfDoc, "images/Sanpar_Logo.png", 0, 0, 100, 100);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + sFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;
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

        private static PdfPCell setCell(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(objTable);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            return tmpCell;
        }

        private static void addWallPaper(Document xDoc, string xImagePath, float xPos = 0, float yPos = 0, float xWidth = 10, float xHeight = 10)
        {
            if (!String.IsNullOrEmpty(xImagePath))
            {
                var objWall = iTextSharp.text.Image.GetInstance(xImagePath);
                //var objWall = iTextSharp.text.Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath(xImagePath));
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

    class RoundedBorder : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] canvas)
        {
            PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.RoundRectangle(
              rect.Left + 1.5f,
              rect.Bottom + 1.5f,
              rect.Width - 3,
              rect.Height - 3, 4
            );
            cb.Stroke();
        }
    }

    class DottedCells : IPdfPTableEvent
    {
        public void TableLayout(PdfPTable table, float[][] widths,float[] heights, int headerRows, int rowStart,PdfContentByte[] canvases)
        {
            PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];
            canvas.SetLineDash(3f, 3f);
            float llx = widths[0][0];
            float urx = widths[0][widths[0].Length - 1];
            for (int i = 0; i < heights.Length; i++)
            {
                canvas.MoveTo(llx, heights[i]);
                canvas.LineTo(urx, heights[i]);
            }
            for (int i = 0; i < widths.Length; i++)
            {
                for (int j = 0; j < widths[i].Length; j++)
                {
                    canvas.MoveTo(widths[i][j], heights[i]);
                    canvas.LineTo(widths[i][j], heights[i + 1]);
                }
            }
            canvas.Stroke();
        }
    }
class ITextEvents : PdfPageEventHelper
    {
        PdfContentByte cb;
        PdfTemplate pageNoTemplate;
        //BaseFont bf = null;
        iTextSharp.text.Image watermarkImage;

        #region Properties
        private string _Title;
        public string Title
        {
            get { return "Eagle Pressure System"; }
            set { _Title = value; }
        }

        private string _HeaderLeft;
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }

        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }

        private iTextSharp.text.Font _HeaderFont;
        public iTextSharp.text.Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        // ------------------------------------------------------
        private string _FooterText;
        public string FooterText
        {
            get { return _FooterText; }
            set { _FooterText = value; }
        }

        private iTextSharp.text.Font _FooterFont;
        public iTextSharp.text.Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }

        private string _WallPaper;
        public string WallPaper
        {
            get { return _WallPaper; }
            set { _WallPaper = value; }
        }

        #endregion

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                //watermarkImage = iTextSharp.text.Image.GetInstance(WallPaper);
                //try
                //{
                //    HttpContext.Current.Session["CurrentPageNo"] = 1;
                //}
                //catch (Exception ex)
                //{}
                cb = writer.DirectContent;
                pageNoTemplate = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de) { throw de; }
            catch (System.IO.IOException ioe) { throw ioe; }
        }

        public override void OnStartPage(PdfWriter writer, Document pdfDoc)
        {
            myPdfConstruct c1 = new myPdfConstruct(pdfDoc, "", "");

            string tmpSerialKey, flagPrintHeader, pImageFile, pImageFile1, pImageFile2;
            // ------------------------------------------------------------------
            //float fontSize = 80;
            //float xwidth = pdfDoc.PageSize.Width;
            //float xheight = pdfDoc.PageSize.Height;
            //float xPosition = iTextSharp.text.PageSize.A4.Width / 2;
            //float yPosition = (iTextSharp.text.PageSize.A4.Height - 140f) / 2;
            //float angle = 0;
            //try
            //{
            //    PdfContentByte under = writer.DirectContentUnder;
            //    String pImageFile101 = StarsProject.QuotationEagle.imagepath + "\\CompanyLogo\\watermark.jpg";
            //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(pImageFile101);
            //    image.SetAbsolutePosition(0, 0);
            //    image.ScaleToFit(xwidth, xheight);
            //    under.AddImage(image);

            //}
            //catch (Exception ex)
            //{
            //    Console.Error.WriteLine(ex.Message);
            //}
            // ------------------------------------------------------------------
            if (Entity.Company.Mode == 2)
            {
                tmpSerialKey = StarsProject.QuotationEagle.serialkey;
                flagPrintHeader = StarsProject.QuotationEagle.printheader;
                pImageFile = StarsProject.QuotationEagle.imagepath + "\\CompanyLogo\\CompanyLogoBanner";
                pImageFile1 = StarsProject.QuotationEagle.imagepath + "\\CompanyLogo\\CompanyLogo";
                pImageFile2 = StarsProject.QuotationEagle.imagepath + "\\CompanyLogo\\CompanyLogoBanner1";
            }
            else
            {
                tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
                pImageFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogoBanner";
                pImageFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogo";
                pImageFile2 = StarsProject.QuotationEagle.imagepath + "\\CompanyLogo\\CompanyLogoBanner1";
            }
            int[] column_tblMember = { 60, 40 };
            int[] column_tblMemberEurolight = { 50, 50 };
            Int32 PageNO = Convert.ToInt32(writer.PageNumber.ToString());
            //tmpSerialKey = "EL6R-LHUV-5PXM-XXAT";
            //tmpSerialKey = "ZE5W-HOME-AG41-SF61";
            //tmpSerialKey = "DYNA-2GF3-J7G8-FF12";
            if (!String.IsNullOrEmpty(flagPrintHeader))
            {
                if (flagPrintHeader.ToLower() == "yes" || flagPrintHeader.ToLower() == "y")
                {
                    if (tmpSerialKey == "R3JA-OMMK-TMYG-DGJJ")
                        pdfDoc.Add(c1.GenerateCompanyHeader());
                    else if (tmpSerialKey == "C6H4-TTDP-AJER-MFG4")     // FCW Technologies
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "J63H-F8LX-B4B2-GYVZ")     // Hi-Tech
                        //pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 15, c1.fnCalibriBold14, "SALES QUOTATION", c1.paddingOf4, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 2, c1.fnCalibri14, PageNO, "", 0, 1, 0, 5, 0));
                    else if (tmpSerialKey == "G5MJ-M2OQ-H927-8NI9")     // Omkar Industries
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "4JM1-E874-JBK0-5HAN")     // Shree Balaji - SBR
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "PKN3-1F10-TEMY-9LNH")     // Motto Tech  
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "TJ7S-06Q2-8R2U-KJWG")     // Gautam   
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    //else if (tmpSerialKey == "BLUE-CHEM-56JK-BC88")     // Blue Chem   
                    //    pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")     // PerfectRoto   
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "LVK4-MN01-K121-NGVL")     // MN Rubber  
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "D33J-H872-3545-71A1")     // SuperTech  
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImage(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "SA98-6HY9-HU67-LORF" || tmpSerialKey == "AST8-GH6K-U79H-LH81")     // ShaktiPet & Astire
                    {
                        int[] column_tblMember2 = { 60, 40 };
                        pdfDoc.Add(c1.GenerateCompanyHeader_ImageMultiAddress(tmpSerialKey, column_tblMember2, pImageFile1));
                    }
                    else if (tmpSerialKey == "SBR2-SI89-GH89-KI90")     // SteelOnn
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "SIV3-DIO4-09IK-98RE")     // ShivSai
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "JME9-EI90-IKP9-K89I")     // JM Electrical
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "7IPC-ZGNX-R9AN-CG20")     // GreenStone
                        pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddress(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "DA19-Y46N-7P4L-P1ST")     // Dyna
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImage(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "ASS9-SF90-FD85-ASE8")     // Asha Techno
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImage(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "CL45-A6R9-T34E-AS89")     // Clartech
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "BHAR-ATPA-TTER-NENG")     // Bharat Patters
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "BAPL-SI90-GH78-MN90")     // BenchMark
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "PI01-YU02-RUBB-03ER")     // Piyush
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "STX1-UP06-YU89-JK23")     // stainex 
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImageStainex(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "SHSI-JA98-NA3S-51SD")     // Shajanand Engineering
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "MR09-DF34-TP45-55PE")     // Mudra Computers
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "JAYJ-ALAR-AMBR-ICKS")     // Jay Jalaram
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "EL6R-LHUV-5PXM-XXAT")     // Eagle Pressure System
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImage(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "SOL4-PB94-KY45-TY15")     // Solnce
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImage(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "KIO8-PL89-R6I0-VB8I")     // KIO
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "KRI1-NAS2-CHAM-SI70")     // Krishna
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "VJ89-VEER-RJCA-SHEW")     // Veer Cashew
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "SAF3-AL90-321M-ENT1")     // Safal
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "ECO3-2G21-TECH-3MRT")     // EcoTech
                        pdfDoc.Add(c1.GenerateCompanyHeader_EcoTech(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "TWS3-RT90-E22O-K88P")     // TWS
                        pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddress(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "SIGM-TECH-SI98-IJ45")     // Sigma 
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    else if (tmpSerialKey == "7PT0-SAFD-DGB5-CFD2")     // PRIMA
                        pdfDoc.Add(c1.GenerateCompanyHeader_AddressImagePRIMA(tmpSerialKey, column_tblMember, pImageFile1));
                    else if (tmpSerialKey == "DYNA-2GF3-J7G8-FF12")     // Dynamic Contral System 
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "salesorder")
                        {
                            pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                            if (pdfDoc.PageNumber > 1)
                            {
                                PdfPTable tblSession = (PdfPTable)HttpContext.Current.Session["Session"];
                                tblSession.WidthPercentage = 100f;
                                tblSession.DefaultCell.Border = 3;
                                pdfDoc.Add(tblSession);
                            }
                        }
                        else if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {
                            if (pdfDoc.PageNumber > 1)
                            {
                                PdfPTable tblSession = (PdfPTable)HttpContext.Current.Session["tblM"];
                                tblSession.WidthPercentage = 100f;
                                tblSession.DefaultCell.Border = Rectangle.NO_BORDER;
                                pdfDoc.Add(tblSession);
                            }
                            else
                            {
                                pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                            }
                        }
                        else
                            pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                    }
                    else if (tmpSerialKey == "EUSI-LI85-4SL5-88GT")     // Eurolight
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {
                            if (pdfDoc.PageNumber > 1)
                            {
                                myPdfConstruct pdf = new myPdfConstruct();
                                //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
                                int totrec1 = 0;
                                List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
                                //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
                                PdfPTable tblTitle = new PdfPTable(2);
                                int[] column_tblTitle = { 95, 5 };
                                tblTitle.SetWidths(column_tblTitle);
                                tblTitle.AddCell(pdf.setCellTransparantBorderColor(lstOrg[0].Fax1, pdf.WhiteBaseColor, pdf.fnCalibriGray8, new iTextSharp.text.BaseColor(128, 128, 128), pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 8));
                                tblTitle.AddCell(pdf.setCellTransparantBorderColor(pdfDoc.PageNumber.ToString(), pdf.WhiteBaseColor, pdf.fnCalibriGray8, new iTextSharp.text.BaseColor(128, 128, 128), pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 4));
                                tblTitle.AddCell(pdf.setCellTransparantBorderColor("Techno-Commercial Proposal", pdf.WhiteBaseColor, pdf.fnCalibriGray8, new iTextSharp.text.BaseColor(128, 128, 128), pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 8));
                                tblTitle.AddCell(pdf.setCellTransparantBorderColor(" ", pdf.WhiteBaseColor, pdf.fnCalibriGray8, new iTextSharp.text.BaseColor(128, 128, 128), pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 4));
                                tblTitle.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tblTitle.TotalWidth = pdfDoc.PageSize.Width;
                                pdfDoc.Add(tblTitle);
                            }
                        }
                        else
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressEurolight(tmpSerialKey, column_tblMemberEurolight, pImageFile1));
                    }
                    else if (tmpSerialKey == "ZE5W-HOME-AG41-SF61")     // Zemote
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "salesorder")
                        {
                            if (pdfDoc.PageNumber > 1)
                            {
                                PdfPTable tblQuotTitle;
                                tblQuotTitle = (PdfPTable)HttpContext.Current.Session["tblHeader1"];
                                tblQuotTitle.WidthPercentage = 100f;
                                pdfDoc.Add(tblQuotTitle);
                                //pdfDoc.Add(new Phrase(" "));
                                //pdfDoc.Add(c1.addWallPaper(pdfDoc, pImageFile1));

                            }
                        }
                        else
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddress(tmpSerialKey, column_tblMember, pImageFile1));
                    }
                    else if (tmpSerialKey == "HE43-SF8S-SDFC-AU9T")     // HEMSAN INDUSTRIES
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressHemsanIndQT(column_tblMember, pImageFile1));

                            PdfPTable tblMember = (PdfPTable)HttpContext.Current.Session["tblMember"];
                            PdfPTable tblDt = (PdfPTable)HttpContext.Current.Session["tblDt"];
                            PdfPTable tblDetail = (PdfPTable)HttpContext.Current.Session["tblDetail"];
                            PdfPTable tblTnC = (PdfPTable)HttpContext.Current.Session["tblTnC"];
                            PdfPTable tblFooter = (PdfPTable)HttpContext.Current.Session["tblFooter"];

                            var tblM = tblMember != null ? tblMember.CalculateHeights() : 0;
                            var tblDHD = tblDt != null ? tblDt.CalculateHeights() : 0;
                            var tblD = tblDetail != null ? tblDetail.CalculateHeights() : 0;
                            var tblTnc = tblTnC != null ? tblTnC.CalculateHeights() : 0;
                            var tblF = tblFooter != null ? tblFooter.CalculateHeights() : 0;
                            var margin = pdfDoc.TopMargin + pdfDoc.BottomMargin;
                            var PageH = pdfDoc.PageSize.Height;
                            var DHeight = PageH - (tblM + tblDHD + tblTnc + tblF + margin);
                            if (tblD > DHeight && pdfDoc.PageNumber != 1)
                            {
                                tblDt.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                                tblDt.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfDoc.Add(tblDt);
                            }
                        }
                        else
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressHemsanInd(column_tblMember, pImageFile1));
                    }
                    else if (tmpSerialKey == "COL1-AKL9-TEC9-SJ99")     // ColdTech
                    {
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, HttpContext.Current.Session["printModule"].ToString() == "outward" ? pImageFile2 : pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                        if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {

                            PdfPTable tblQuotTitle;
                            tblQuotTitle = (PdfPTable)HttpContext.Current.Session["tblBanner"];
                            tblQuotTitle.WidthPercentage = 100f;
                            pdfDoc.Add(tblQuotTitle);
                        }
                    }
                    else if (tmpSerialKey == "PART-KIT9-SIHC-CHEN")     // Parth Kitchen
                    {
                        pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressParth(column_tblMember, pImageFile1));
                    }
                    else if (tmpSerialKey == "DARS-SAFE-TA12-Y808")     // Darshan Safety Zone
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {                            
                                PdfPTable tblSession = (PdfPTable)HttpContext.Current.Session["tblLogo"];                                
                                tblSession.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                                tblSession.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfDoc.Add(tblSession);

                                PdfPTable tblMember = (PdfPTable)HttpContext.Current.Session["tblMember"];
                                PdfPTable tblDetailHD = (PdfPTable)HttpContext.Current.Session["tblDetailHD"];
                                PdfPTable tblDetail = (PdfPTable)HttpContext.Current.Session["tblDetail"];
                                PdfPTable tblFooter = (PdfPTable)HttpContext.Current.Session["tblFooter"];

                                var tblM = tblMember != null ? tblMember.CalculateHeights() : 0;
                                var tblDHD = tblDetailHD != null ? tblDetailHD.CalculateHeights() : 0;
                                var tblD = tblDetail != null ? tblDetail.CalculateHeights() : 0;
                                var tblF = tblFooter != null ? tblFooter.CalculateHeights() : 0;
                                var margin = pdfDoc.TopMargin + pdfDoc.BottomMargin;
                                var PageH = pdfDoc.PageSize.Height;
                                var DHeight = PageH - (tblM + tblDHD + tblF + margin);
                                if (tblD > DHeight && pdfDoc.PageNumber != 1)
                                {
                                    tblDetailHD.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
                                    tblDetailHD.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfDoc.Add(tblDetailHD);
                                }                            
                            }
                        }
                    else if (tmpSerialKey == "ARAN-WR5K-U7D4-LN9F")     // Aranka 
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {
                            if (pdfDoc.PageNumber == 1)
                            {
                                pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressAranka(column_tblMember, pImageFile1));
                            }
                        }
                        else
                        {
                                pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressAranka(column_tblMember, pImageFile1));
                        }
                    }
                    else if (tmpSerialKey == "ACSI-C803-CUP0-SHEL")     // ACCU PANEL
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() != "salesbill")
                        {
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressAccuPanel(column_tblMember, pImageFile1));
                        }
                        else
                        {
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddressAccuPanel2(column_tblMember, pImageFile1));
                        }

                    }
                    else if (tmpSerialKey == "BLUE-CHEM-56JK-BC88")     // Blue Chem 
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "Quotation")
                        {
                            if (pdfDoc.PageNumber == 1)
                            {
                                pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                            }
                        }
                        else
                        {
                            pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                        }
                    }
                    else if (tmpSerialKey == "GR87-E67E-J0NN-LEAF")     // GreenLeaf
                    {
                        if (HttpContext.Current.Session["printModule"].ToString() == "purchaseorder")
                        {
                            PdfPTable tblQuotTitle;
                            tblQuotTitle = (PdfPTable)HttpContext.Current.Session["tblMixHead"];
                            tblQuotTitle.WidthPercentage = 100f;
                            pdfDoc.Add(tblQuotTitle);
                        }
                        else
                            pdfDoc.Add(c1.GenerateCompanyHeader_ImageAddress(tmpSerialKey, column_tblMember, pImageFile1));
                    }
                    else if (tmpSerialKey == "RSIO-RUDR-34LR-SIO9")     // Rudra Solar
                        pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 2, c1.fnCalibri14, PageNO, "", 0, 1, 0, 5, 0));

                    else
                        pdfDoc.Add(c1.GenerateCompanyHeader());
                }
                else
                {
                    if (tmpSerialKey == "DYNA-2GF3-J7G8-FF12")     // Dynamic Contral System 
                    {
                        //pdfDoc.Add(c1.GenerateCompanyHeader_Banner(tmpSerialKey, pImageFile, 0, 6, 0, c1.fnCalibri14, PageNO));
                        if (HttpContext.Current.Session["printModule"].ToString() == "salesorder")
                        {
                            if (pdfDoc.PageNumber > 1)
                            {
                                PdfPTable tblSession = (PdfPTable)HttpContext.Current.Session["Session"];
                                tblSession.WidthPercentage = 100f;
                                tblSession.DefaultCell.Border = 3;
                                pdfDoc.Add(tblSession);
                            }
                        }
                        if (HttpContext.Current.Session["printModule"].ToString() == "quotation")
                        {
                            if (pdfDoc.PageNumber > 1)
                            {
                                PdfPTable tblSession = (PdfPTable)HttpContext.Current.Session["tblM"];
                                tblSession.WidthPercentage = 100f;
                                tblSession.DefaultCell.Border = 3;
                                pdfDoc.Add(tblSession);
                            }
                        }
                    }
                }

            }

        }

        public class GradientTableBackground : IPdfPTableEvent
        {
            public GradientTableBackground(PdfWriter writer)
            {
                this.writer = writer;
            }

            public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
            {


                BaseColor gradientStart = new BaseColor(255, 0, 0);
                BaseColor gradientEnd = new BaseColor(255, 255, 255);

                float[] topWidths = widths[0];
                PdfContentByte cb = canvases[PdfPTable.BACKGROUNDCANVAS];

                Rectangle rectangle = new Rectangle(topWidths[0], heights[heights.Length - 1], topWidths[heights.Length - 1], heights[0]);

                PdfShading shading = PdfShading.SimpleAxial(writer, 0, 0, rectangle.Top, 0, gradientEnd, gradientStart);
                PdfShadingPattern pattern = new PdfShadingPattern(shading);
                cb.SetShadingFill(pattern);
                cb.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                cb.Fill();
            }
            PdfWriter writer;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            
            base.OnEndPage(writer, document);
            PdfPCell cell = new PdfPCell();
            myPdfConstruct c1 = new myPdfConstruct();
            iTextSharp.text.Rectangle pageSize = document.PageSize;

            // -------------------------------------------------------------------------
            string tmpSerialKey, imagepath, flagPrintHeader, flagPrintFooter, flagPrintBorder;
            string flagPrintModule = "";

            int pagenumber = document.PageNumber;
      

            if (Entity.Company.Mode == 2)       // Company.Mode = 2 For API 
            {
                tmpSerialKey = StarsProject.QuotationEagle.serialkey;
                imagepath = StarsProject.QuotationEagle.imagepath + "\\CompanyLogo";
                flagPrintHeader = StarsProject.QuotationEagle.printheader;
                flagPrintFooter = StarsProject.QuotationEagle.printfooter;
                flagPrintModule = StarsProject.QuotationEagle.printModule.ToLower();
            }
            else
            {
                tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
                //String imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
                imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo");
                flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
                flagPrintFooter = (string)HttpContext.Current.Session["PrintFooter"];
                if (HttpContext.Current.Session["printModule"] != null)
                    flagPrintModule = HttpContext.Current.Session["printModule"].ToString().ToLower();
            }
            //flagPrintBorder = HttpContext.Current.Session["printborder"].ToString().ToLower();
            //tmpSerialKey = "DYNA-2GF3-J7G8-FF12";
            //tmpSerialKey = "COL1-AKL9-TEC9-SJ99";
            // -------------------------------------------------------------------------
            string tmpFile11 = "";
            string FooterImage = "";
            if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")          // STEELMAN GASES PVT LTD
            {
                PdfPTable tbl1;
                if (flagPrintFooter == null || flagPrintFooter == "yes")
                {
                    tbl1 = c1.GenerateCompanyFooter(document, tmpSerialKey);
                    tbl1.LockedWidth = true;
                    tbl1.HorizontalAlignment = Element.ALIGN_CENTER;
                    tbl1.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(20), writer.DirectContent);
                }
            }
            if (tmpSerialKey == "SIGM-TECH-SI98-IJ45")          // Sigma Tech
            {
                PdfPTable tbl1;
                if (flagPrintHeader == "yes")
                {
                    PdfPTable table = new PdfPTable(1);
                    int[] column_table = { 100 };
                    tbl1 = (PdfPTable)HttpContext.Current.Session["tbl"];
                    table.AddCell(c1.setCell(tbl1, c1.WhiteBaseColor, c1.objContentFontDataBlack, c1.paddingOf1, 1, Element.ALIGN_CENTER, Element.ALIGN_BOTTOM, 0));
                    table.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    table.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            if (tmpSerialKey == "ACSI-C803-CUP0-SHEL")          // Accu Panel
            {
                if (flagPrintModule == "salesorder" || flagPrintModule == "salesperfoma")
                {
                    //-------------------Experiment to Put Border--------------------
                    var content = writer.DirectContent;
                    var pageBorderRect = new Rectangle(document.PageSize);

                    pageBorderRect.Left = 10;
                    pageBorderRect.Right = 585;
                    pageBorderRect.Top = 820;
                    pageBorderRect.Bottom = 28;

                    content.SetColorStroke(BaseColor.BLACK);
                    content.SetLineWidth(1);
                    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                    content.Stroke();
                }

               if (flagPrintModule == "quotation")
                    {
                        //-------------------Experiment to Put Border--------------------
                        var content = writer.DirectContent;
                        var pageBorderRect = new Rectangle(document.PageSize);

                        //pageBorderRect.Left += document.LeftMargin;
                        //pageBorderRect.Right -= document.RightMargin;
                        //pageBorderRect.Top -= document.TopMargin;
                        //pageBorderRect.Bottom += document.BottomMargin;

                        pageBorderRect.Left = 25;
                        pageBorderRect.Right = 570;
                        pageBorderRect.Top = 817;
                        pageBorderRect.Bottom = 25;

                        content.SetColorStroke(BaseColor.BLACK);
                        content.SetLineWidth(1);
                        content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                        content.Stroke();
                    }

                
                //if (flagPrintModule == "orgemployee")
                //{
                //    //-------------------Experiment to Put Border--------------------
                //    var content = writer.DirectContent;
                //    var pageBorderRect = new Rectangle(document.PageSize);

                //    pageBorderRect.Left = 10;
                //    pageBorderRect.Right = 585;
                //    pageBorderRect.Top = 820;
                //    pageBorderRect.Bottom = 82;

                //    content.SetColorStroke(BaseColor.BLACK);
                //    content.SetLineWidth(1);
                //    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                //    content.Stroke();
                //}
            }
            else if (tmpSerialKey == "COL1-AKL9-TEC9-SJ99") // Cold Tech
            {
                if (flagPrintHeader == "yes")
                {
                    if (flagPrintModule != "quotation" && flagPrintModule != "outward")
                    {
                        tmpFile11 = imagepath + "\\CompanyLogoFooter";
                        FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                        if (String.IsNullOrEmpty(FooterImage))
                            FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                        if (String.IsNullOrEmpty(FooterImage))
                            FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                        iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                        eSign.ScaleAbsolute(document.PageSize.Width, 90);

                        PdfPTable Tblfooter = new PdfPTable(1);
                        Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                        Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                        Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                        Tblfooter.AddCell(c1.setCell(document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                        Tblfooter.AddCell(eSign);
                        //Tblfooter.AddCell(cell);
                        Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);

                        if (flagPrintModule == "proforma")
                        {
                            //-------------------Experiment to Put Border--------------------
                            var content = writer.DirectContent;
                            var pageBorderRect = new Rectangle(document.PageSize);

                            //pageBorderRect.Left += document.LeftMargin;
                            //pageBorderRect.Right -= document.RightMargin;
                            //pageBorderRect.Top -= document.TopMargin;
                            //pageBorderRect.Bottom += document.BottomMargin;

                            pageBorderRect.Left = 25;
                            pageBorderRect.Right = 570;
                            pageBorderRect.Top = 817;
                            pageBorderRect.Bottom = 25;

                            content.SetColorStroke(BaseColor.BLACK);
                            content.SetLineWidth(2);
                            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                            content.Stroke();
                        }
                    }
                    else
                    {
                        if(flagPrintModule != "outward")
                        { 
                            int totrec = 0;
                            List<Entity.OrganizationStructure> lst = new List<Entity.OrganizationStructure>();
                            lst = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 111, out totrec);

                            String Address = (!String.IsNullOrEmpty(lst[0].Address) ? lst[0].Address : "")
                            + (!String.IsNullOrEmpty(lst[0].CityName) ? ", " + lst[0].CityName : "")
                            + (!String.IsNullOrEmpty(lst[0].Pincode) ? " Pin - " + lst[0].Pincode : "")
                            + (!String.IsNullOrEmpty(lst[0].StateName) ? ", " + lst[0].StateName : "")
                            + ", India";

                            PdfPTable Tblfooter1 = new PdfPTable(1);
                            int[] Column_Tblfooter1 = { 100 };
                            Tblfooter1.SetWidths(Column_Tblfooter1);
                            Tblfooter1.LockedWidth = true;
                            Tblfooter1.DefaultCell.Border = Rectangle.NO_BORDER;
                            String SecLine = (!String.IsNullOrEmpty(lst[0].Landline1) ? "M: " + lst[0].Landline1 + " | " : "") +
                                            (!String.IsNullOrEmpty(lst[0].EmailAddress) ? "E: " + lst[0].EmailAddress + " | " : "") +
                                            (!String.IsNullOrEmpty(lst[0].Fax1) ? "W: " + lst[0].Fax1 : "");

                            tmpFile11 = imagepath + "\\CompanyLogoFooter";
                            FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                            if (String.IsNullOrEmpty(FooterImage))
                                FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                            if (String.IsNullOrEmpty(FooterImage))
                                FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                            iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                            eSign.ScaleAbsolute(document.PageSize.Width, 90);

                            Tblfooter1.AddCell(c1.setCell(document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                            //Tblfooter1.AddCell(c1.setCell(Address, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 1));
                            //Tblfooter1.AddCell(c1.setCell(SecLine, c1.WhiteBaseColor, c1.fnCalibriBold9,  c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                            Tblfooter1.AddCell(c1.setCell(eSign, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                            Tblfooter1.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                            Tblfooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                            Tblfooter1.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                            //-------------------Experiment to Put Border--------------------
                            var content = writer.DirectContent;
                            var pageBorderRect = new Rectangle(document.PageSize);

                            //pageBorderRect.Left += document.LeftMargin;
                            //pageBorderRect.Right -= document.RightMargin;
                            //pageBorderRect.Top -= document.TopMargin;
                            //pageBorderRect.Bottom += document.BottomMargin;

                            pageBorderRect.Left = 25;
                            pageBorderRect.Right = 570;
                            pageBorderRect.Top = 817;
                            pageBorderRect.Bottom = 25;

                            content.SetColorStroke(BaseColor.BLACK);
                            content.SetLineWidth(1);
                            content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                            content.Stroke();
                         }
                    }
                }
            }
            
            else if (tmpSerialKey == "DYNA-2GF3-J7G8-FF12") // Dynamic Control System
            {
                PdfPTable Tblfooter1 = new PdfPTable(3);
                int[] Column_Tblfooter1 = { 30, 40, 30 };
                Tblfooter1.SetWidths(Column_Tblfooter1);
                Tblfooter1.LockedWidth = true;
                Tblfooter1.DefaultCell.Border = Rectangle.NO_BORDER;
                int totrec = 0;
                List<Entity.OrganizationStructure> lst = new List<Entity.OrganizationStructure>();
                lst = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 111, out totrec);

                String Address = (!String.IsNullOrEmpty(lst[0].Address) ? lst[0].Address : "") + (!String.IsNullOrEmpty(lst[0].Address) ? ", " : "") + "\n" 
                + (!String.IsNullOrEmpty(lst[0].CityName) ?  lst[0].CityName : "") 
                + (!String.IsNullOrEmpty(lst[0].Pincode) ? " - "  : "") + (!String.IsNullOrEmpty(lst[0].Pincode) ? lst[0].Pincode : "")
                + ", India";

                PdfPTable Tblf = new PdfPTable(2);
                int[] Column_Tblf = { 25,75 };
                Tblf.SetWidths(Column_Tblf);
                Tblf.AddCell(c1.setCell("   Tele/Fax", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell(": " + lst[0].Landline2, c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell("   Mobile", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell(": " + lst[0].Landline1, c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell("   Email", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell(": " + lst[0].EmailAddress, c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell("   Visit us", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                Tblf.AddCell(c1.setCell(": " + lst[0].Fax1, c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

                //if (flagPrintModule == "purchaseorder")
                //{
                //    Phrase p = (Phrase)HttpContext.Current.Session["phrase"];
                //    Tblfooter1.AddCell(c1.setCell(p, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 1));
                //    Tblfooter1.AddCell(c1.setCell("Page : " + document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 1));
                //    Tblfooter1.AddCell(c1.setCell(Address, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    Tblfooter1.AddCell(c1.setCell(Tblf, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    Tblfooter1.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                //    Tblfooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                //    Tblfooter1.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                //}
                //else if (flagPrintModule == "salesorder")
                //{

                //    Tblfooter1.AddCell(c1.setCell("", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 1));
                //    Tblfooter1.AddCell(c1.setCell("Page : " + document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 1));
                //    Tblfooter1.AddCell(c1.setCell(Address, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    Tblfooter1.AddCell(c1.setCell(Tblf, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                //    Tblfooter1.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                //    Tblfooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                //    Tblfooter1.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                //}
                //else
                //{
                    if (flagPrintModule == "purchaseorder")
                    {
                        Phrase p = (Phrase)HttpContext.Current.Session["phrase"];
                        Tblfooter1.AddCell(c1.setCell(p, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 1));
                        Tblfooter1.AddCell(c1.setCell("Page : " + document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 1));
                    }
                    else if (flagPrintModule == "salesorder")
                    {
                        Tblfooter1.AddCell(c1.setCell("", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 1));
                        Tblfooter1.AddCell(c1.setCell("Page : " + document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_BOTTOM, 1));
                    }
                    Tblfooter1.AddCell(c1.setCell(" ", c1.WhiteBaseColor, c1.fnCalibri6, 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter1.AddCell(c1.setCell(Address, c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter1.AddCell(c1.setCell(Tblf, c1.WhiteBaseColor, c1.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter1.AddCell(c1.setCell(" ", c1.WhiteBaseColor, c1.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter1.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter1.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                //}
            }
            else if (tmpSerialKey == "VJ89-VEER-RJCA-SHEW")          // Veer Cashew
            {
                if (flagPrintHeader == "yes")
                {
                    int totrec = 0;
                    PdfPTable Tblfooter = new PdfPTable(3);
                    int[] Column_Tblfooter = { 33, 33, 33 };
                    Tblfooter.SetWidths(Column_Tblfooter);

                    List<Entity.OrganizationStructure> lst = new List<Entity.OrganizationStructure>();
                    lst = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 111, out totrec);

                    List<Entity.OrganizationStructure> lstBranch = new List<Entity.OrganizationStructure>();
                    lstBranch = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("002", 1, 111, out totrec);

                    Paragraph para = new Paragraph();
                    Phrase phBranches = new Phrase();
                    Chunk chBranches = new Chunk(lst[0].EmailAddress, c1.fnCalibriBold9);
                    chBranches.SetUnderline(1, -2);
                    phBranches.Add(chBranches);
                    para.Add(phBranches);
                    Paragraph para1 = new Paragraph();
                    Phrase phBranches1 = new Phrase();
                    Chunk chBranches1 = new Chunk(lst[0].Landline1, c1.fnCalibri9);
                    chBranches1.SetUnderline(1, -2);
                    phBranches1.Add(chBranches1);
                    para1.Add(phBranches1);
                    Paragraph para2 = new Paragraph();
                    Phrase phBranches2 = new Phrase();
                    Chunk chBranches2 = new Chunk(lst[0].Fax1, c1.fnCalibriBold9);
                    chBranches2.SetUnderline(1, -2);
                    phBranches2.Add(chBranches2);
                    para2.Add(phBranches2);

                    String branch = "";
                    if (lstBranch.Count > 0)
                        branch = (!String.IsNullOrEmpty(lstBranch[0].Address) ? lstBranch[0].Address : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].CityName) ? ", " + lstBranch[0].CityName : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].Pincode) ? " Pin - " + lstBranch[0].Pincode : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].StateName) ? ", " + lstBranch[0].StateName : "")
                        + ", India";

                    Phrase pq = new Phrase();
                    Chunk cq = new Chunk("Plant:- ", c1.fnCalibriBold8);
                    Chunk c2q = new Chunk(",  " + branch, c1.fnCalibri8);
                    pq.Add(cq);
                    pq.Add(c2q);
                    Paragraph phq = new Paragraph();
                    phq.Add(pq);

                    BaseColor gradientStart = new BaseColor(173, 216, 228);
                    BaseColor gradientEnd = new BaseColor(218, 238, 243);

                    PdfPTable Tblfooter1 = new PdfPTable(1);
                    int[] Column_Tblfooter1 = { 100 };
                    Tblfooter1.SetWidths(Column_Tblfooter1);
                    Tblfooter1.TotalWidth = (document.PageSize.Width);
                    Tblfooter1.LockedWidth = true;
                    Tblfooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter1.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.AddCell(c1.setCellTransparant(phq, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 3, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCellTransparant(para, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCellTransparant(para1, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCellTransparant(para2, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter1.AddCell(c1.setCellTransparant(Tblfooter, c1.WhiteBaseColor, c1.fnCalibriBold9, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //Tblfooter1.TableEvent = new GradientTableBackground(writer);
                    PdfShading shading = PdfShading.SimpleAxial(writer, 0, Tblfooter1.CalculateHeights(), document.PageSize.Width, 0, gradientEnd, gradientStart);
                    PdfShadingPattern pattern = new PdfShadingPattern(shading);
                    cb.SetShadingFill(pattern);
                    cb.Rectangle(0, 0, document.PageSize.Width, Tblfooter1.CalculateHeights());
                    cb.Fill();
                    Tblfooter1.WriteSelectedRows(0, -1, 0, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "J63H-F8LX-B4B2-GYVZ")          // HighTech
            {
                if (flagPrintHeader == "yes")
                {
                    int pageN = writer.PageNumber;
                    //String text = "Page " + pageN.ToString() + " of ";

                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);

                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.TOP_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(eSign);
                    //Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "7PT0-SAFD-DGB5-CFD2")          // PRIMA
            {
                if (flagPrintHeader == "yes")
                {
                    int pageN = writer.PageNumber;
                    //String text = "Page " + pageN.ToString() + " of ";

                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(100, 38);

                    PdfPTable Tblfooter = new PdfPTable(2);
                    int[] column_Tblfooter = { 50, 50 };
                    Tblfooter.SetWidths(column_Tblfooter);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.TOP_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(c1.setCellFixImage(eSign, c1.WhiteBaseColor, c1.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 0));
                    Tblfooter.AddCell(c1.setCell("Page  " + document.PageNumber.ToString(), c1.WhiteBaseColor, c1.fnTimesRoman13FlashWhite, c1.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    Tblfooter.AddCell(c1.setCell(" ", c1.WhiteBaseColor, c1.fnTimesRoman14, c1.paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    //Tblfooter.AddCell(eSign);
                    //Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(30), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "DARS-SAFE-TA12-Y808")          // Darshan Safety Zone
            {
                if (flagPrintModule == "quotation")
                {

                    //-----------------------Company Logo---------------------------------------


                    // --------------------------------------------------------------------------------------------
                    int totrec1 = 0;
                    List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                    lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
                    // ------------------------------------------------------------------------------------------------
                    List<Entity.OrganizationStructure> lstOrg1 = new List<Entity.OrganizationStructure>();
                    lstOrg1 = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("002", 1, 1000, out totrec1);
                    // ------------------------------------------------------------------------------------------------
                    List<Entity.OrganizationStructure> lstOrg2 = new List<Entity.OrganizationStructure>();
                    lstOrg2 = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("003", 1, 1000, out totrec1);
                    // -------------------------------------------------------------------------------------------------------------


                    PdfPTable tblFooter = new PdfPTable(5);
                    int[] column_tblFooter = { 10, 20, 20, 15, 35 };
                    tblFooter.SetWidths(column_tblFooter);
                    tblFooter.SpacingBefore = 0f;
                    tblFooter.LockedWidth = true;
                    tblFooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    document.Add(tblFooter);
                    tblFooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPTable Fix = new PdfPTable(5);
                    Fix.SetWidths(column_tblFooter);
                    String Address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : " ") +
                                     (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : " ") +
                                     (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " + lstOrg[0].Pincode : " ");


                    String Address1 = (!String.IsNullOrEmpty(lstOrg1[0].Address) ? lstOrg1[0].Address : " ") +
                                     (!String.IsNullOrEmpty(lstOrg1[0].CityName) ? ", " + lstOrg1[0].CityName : " ") +
                                     (!String.IsNullOrEmpty(lstOrg1[0].Pincode) ? " - " + lstOrg1[0].Pincode : " ");

                    String Address2 = (!String.IsNullOrEmpty(lstOrg2[0].Address) ? lstOrg2[0].Address : " ") +
                                     (!String.IsNullOrEmpty(lstOrg2[0].CityName) ? ", " + lstOrg2[0].CityName : " ") +
                                     (!String.IsNullOrEmpty(lstOrg2[0].Pincode) ? " - " + lstOrg2[0].Pincode : " ");

                    String Contact = (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 + "\n" : "") +
                                    (!String.IsNullOrEmpty(lstOrg[0].Landline2) ? lstOrg[0].Landline2 + "\n" : "") +
                                    (!String.IsNullOrEmpty(lstOrg1[0].Landline2) ? lstOrg1[0].Landline1 + "\n" : "") + "(Office)";

                    String Email = (!String.IsNullOrEmpty(lstOrg[0].EmailAddress) ? lstOrg[0].EmailAddress : "") +
                                    (!String.IsNullOrEmpty(lstOrg1[0].EmailAddress) ? "\n" + lstOrg1[0].EmailAddress : "");

                    String Web = (!String.IsNullOrEmpty(lstOrg[0].Fax1) ? lstOrg[0].Fax1 + "\n" : "") +
                                (!String.IsNullOrEmpty(lstOrg[0].Fax2) ? lstOrg[0].Fax2 + "\n" : "") +
                                (!String.IsNullOrEmpty(lstOrg1[0].Fax1) ? lstOrg1[0].Fax1 + "\n" : "");

                    Fix.AddCell(c1.setCell("", c1.WhiteBaseColor, c1.fnCalibri8, c1.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    Fix.AddCell(c1.setCellBoldUnbold("Office Address : " + "\n", (!String.IsNullOrEmpty(lstOrg2[0].Address) ? lstOrg2[0].Address : " ") +
                                     (!String.IsNullOrEmpty(lstOrg2[0].CityName) ? ", " + lstOrg2[0].CityName : " ") +
                                     (!String.IsNullOrEmpty(lstOrg2[0].Pincode) ? " - " + lstOrg2[0].Pincode : " "), c1.WhiteBaseColor, c1.fnCalibriBold8, c1.fnCalibri8, c1.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                    Fix.AddCell(c1.setCellBoldUnbold("Factory : " + "\n", (!String.IsNullOrEmpty(lstOrg1[0].Address) ? lstOrg1[0].Address : " ") +
                                     (!String.IsNullOrEmpty(lstOrg1[0].CityName) ? ", " + lstOrg1[0].CityName : " ") +
                                     (!String.IsNullOrEmpty(lstOrg1[0].Pincode) ? " - " + lstOrg1[0].Pincode : " "), c1.WhiteBaseColor, c1.fnCalibriBold8, c1.fnCalibri8, c1.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                    Fix.AddCell(c1.setCellBoldUnbold("Contact : " + "\n", (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 + "\n" : "") +
                                    (!String.IsNullOrEmpty(lstOrg[0].Landline2) ? lstOrg[0].Landline2 + "\n" : "") + "(Office)", c1.WhiteBaseColor, c1.fnCalibriBold8, c1.fnCalibri8, c1.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                    Phrase pp = new Phrase();
                    pp.Add(new Chunk("Email : " + "\n", c1.fnCalibriBold8));
                    pp.Add(new Chunk(Email + "\n", c1.fnCalibri8));
                    pp.Add(new Chunk("Web : " + "\n" + "www.fireblanket.in", c1.fnCalibriBold8));
                    pp.Add(new Chunk(Web, c1.fnCalibri8));
                    Paragraph php = new Paragraph();
                    php.Add(pp);

                    Fix.AddCell(c1.setCell(php, c1.WhiteBaseColor, c1.fnCalibri8, c1.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                    tblFooter.AddCell(c1.setCell(Fix, c1.WhiteBaseColor, c1.objContentFontDataBlack, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));                  
                    //----------------------------------------------------------------------------------------            
                }
            }
            else if (tmpSerialKey == "TJ7S-06Q2-8R2U-KJWG")          // Gautam
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);

                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "G5MJ-M2OQ-H927-8NI9")     // Omkar Industries
            {

                tmpFile11 = imagepath + "\\CompanyLogoFooter";
                FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                if (String.IsNullOrEmpty(FooterImage))
                    FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                if (String.IsNullOrEmpty(FooterImage))
                    FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                eSign.ScaleAbsolute(document.PageSize.Width, 90);

                PdfPTable Tblfooter = new PdfPTable(1);
                Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                Tblfooter.AddCell(eSign);
                Tblfooter.AddCell(cell);
                Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(20), writer.DirectContent);
            }
            else if (tmpSerialKey == "4JM1-E874-JBK0-5HAN")     // Shree Balaji - SBR
            {
                if (flagPrintModule != "salesbill" && flagPrintHeader == "yes")
                {
                    tmpFile11 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);

                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = document.PageSize.Width;
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, 0, document.GetBottom(5), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "TWS3-RT90-E22O-K88P")     // TWS
            {
                if (flagPrintModule == "salesbill" && flagPrintHeader == "yes")
                {
                    //tmpFile11 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogoFooter";
                    //FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    //if (String.IsNullOrEmpty(FooterImage))
                    //    FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    //if (String.IsNullOrEmpty(FooterImage))
                    //    FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    //iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    //eSign.ScaleAbsolute(document.PageSize.Width, 90);



                    PdfPTable Tblfooter = new PdfPTable(4);
                    int[] Column_Tblfooter = { 25, 25, 25, 25 };
                    Tblfooter.SetWidths(Column_Tblfooter);
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.LockedWidth = true;
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;

                    Phrase phBranches = new Phrase();
                    Chunk chBranches = new Chunk("Our Branches", c1.fnCalibriBold10);
                    chBranches.SetUnderline(1, -2);
                    phBranches.Add(chBranches);

                    Tblfooter.AddCell(c1.setCell(phBranches, c1.WhiteBaseColor, c1.fnCalibriBoldUnderline10, c1.paddingOf3, 6, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell(" ", c1.WhiteBaseColor, c1.fnCalibri8, 0, 6, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell("Ahmedabad", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell("Surat", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //Tblfooter.AddCell(c1.setCell("Mumbai", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell("369/4 Opp. Mukesh Industry BRTS Bus Stop, Beside Popular Wheelers Next to Naarsang Dada Mandir, Isanpur, Ahmedabad.382443", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.RIGHT_BORDER));
                    Tblfooter.AddCell(c1.setCell("Plot No. 12/1,12/2,c37/38, IChhapore, Bhatpore GIDC Estate, Behind Shreeji Tata Motors, Nr.Mudit marble, Hajira - Magdalla Road, surat - 394510", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //Tblfooter.AddCell(c1.setCell("B - 104 Sweet CHS, Sector 14, Plot No.19 Vashi,Navi Mumbai - 400703 ", c1.WhiteBaseColor, c1.fnCalibri9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell("+918758984266, +919624982447", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.RIGHT_BORDER));
                    Tblfooter.AddCell(c1.setCell("+918200018130", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //Tblfooter.AddCell(c1.setCell("+919687079797", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell("yogi@thewheelspecialists.in", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.RIGHT_BORDER));
                    Tblfooter.AddCell(c1.setCell("yogi@thewheelspecialists.in", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //Tblfooter.AddCell(c1.setCell("tarak@wheelspecialists.in", c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //document.Add(Tblfooter);
                    //Tblfooter.TotalWidth = document.PageSize.Width;
                    //Tblfooter.AddCell(eSign);
                    //Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(100), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "GR87-E67E-J0NN-LEAF")     // GreenLeaf
            {
                if (flagPrintHeader == "yes")
                {
                    int totrec = 0;
                    PdfPTable Tblfooter = new PdfPTable(1);
                    int[] Column_Tblfooter = { 100 };
                    Tblfooter.SetWidths(Column_Tblfooter);
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.LockedWidth = true;
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    List<Entity.OrganizationStructure> lst = new List<Entity.OrganizationStructure>();
                    lst = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 111, out totrec);

                    List<Entity.OrganizationStructure> lstBranch = new List<Entity.OrganizationStructure>();
                    lstBranch = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("BRANCH", 1, 111, out totrec);

                    Paragraph para = new Paragraph();
                    Phrase phBranches = new Phrase();
                    Chunk chBranches = new Chunk(lst[0].EmailAddress, c1.fnCalibri8);
                    chBranches.SetUnderline(1, -2);
                    phBranches.Add(chBranches);
                    para.Add(phBranches);

                    String address = (!String.IsNullOrEmpty(lst[0].Address) ? lst[0].Address : "")
                        + (!String.IsNullOrEmpty(lst[0].CityName) ? ", " + lst[0].CityName : "")
                        + (!String.IsNullOrEmpty(lst[0].StateName) ? ", " + lst[0].StateName : "")
                        + (!String.IsNullOrEmpty(lst[0].Pincode) ? " - " + lst[0].Pincode : "")
                        + (!String.IsNullOrEmpty(lst[0].Landline1) ? "\n" + " Phone No:- " + lst[0].Landline1 : "")
                        + (!String.IsNullOrEmpty(lst[0].EmailAddress) ? ", E-mail: " : "");

                    String branch = "";
                    if (lstBranch.Count > 0)
                        branch = (!String.IsNullOrEmpty(lstBranch[0].Address) ? lstBranch[0].Address : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].CityName) ? ", " + lstBranch[0].CityName : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].StateName) ? ", " + lstBranch[0].StateName : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].Pincode) ? " - " + lstBranch[0].Pincode : "")
                        + (!String.IsNullOrEmpty(lstBranch[0].Landline1) ? "\n" + " Contact:- " + lstBranch[0].Landline1 : "");

                    Phrase p = new Phrase();
                    Chunk c = new Chunk(lst[0].OrgName, c1.fnCalibriBold8);
                    Chunk c2 = new Chunk(",  " + address, c1.fnCalibri8);
                    p.Add(c);
                    p.Add(c2);
                    p.Add(chBranches);
                    Paragraph ph = new Paragraph();
                    ph.Add(p);

                    Phrase pq = new Phrase();
                    Chunk cq = new Chunk("Branch Address: ", c1.fnCalibriBold8);
                    Chunk c2q = new Chunk(",  " + branch, c1.fnCalibri8);
                    pq.Add(cq);
                    pq.Add(c2q);
                    Paragraph phq = new Paragraph();
                    phq.Add(pq);
                    Tblfooter.AddCell(c1.setCell(ph, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 1));
                    Tblfooter.AddCell(c1.setCell(phq, c1.WhiteBaseColor, c1.fnCalibriBold9, c1.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }

            else if (tmpSerialKey == "SBR2-SI89-GH89-KI90")     // Shree Balaji - SBR
            {
                if (flagPrintModule != "salesbill" && flagPrintHeader == "yes")
                {
                    tmpFile11 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo") + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);

                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = document.PageSize.Width;
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, 0, document.GetBottom(5), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")     // Perfect Roto Mound
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = 0;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "JME9-EI90-IKP9-K89I")     // JM Electricals
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = 0;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(35), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "ASS9-SF90-FD85-ASE8")     // Asha Techno
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - 5);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, 5, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "BHAR-ATPA-TTER-NENG")     // Bharat Pattern
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(-1, -1, 0, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "C6H4-TTDP-AJER-MFG4")     // FCW 
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(-1, -1, 0, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "CL45-A6R9-T34E-AS89")     // Clartech
            {
                if (flagPrintHeader == "yes")
                {

                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = 0;
                    Tblfooter.TotalWidth = (document.PageSize.Width - 5);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    if (flagPrintModule == "quotation")
                        Tblfooter.WriteSelectedRows(0, -1, 5, document.GetBottom(0), writer.DirectContent);
                    else
                        Tblfooter.WriteSelectedRows(0, -1, 5, document.GetBottom(0), writer.DirectContent);
                }

            }
            else if (tmpSerialKey == "BAPL-SI90-GH78-MN90")     // BenchMark
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = 0;
                    Tblfooter.TotalWidth = (document.PageSize.Width);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, 0, document.GetBottom(35), writer.DirectContent);
                    //AddPageNumber(writer, document);
                }
            }
            else if (tmpSerialKey == "KRI1-NAS2-CHAM-SI70")     // Krishna
            {
                if (flagPrintFooter == "yes")
                {
                    //tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    //FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    //if (String.IsNullOrEmpty(FooterImage))
                    //    FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    //if (String.IsNullOrEmpty(FooterImage))
                    //    FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    //iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    //eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.AddCell(c1.setCell("Page Number : " + pagenumber.ToString(), c1.WhiteBaseColor, c1.fnCalibri7, c1.paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.NO_BORDER));
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    //Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                    //AddPageNumber(writer, document);
                }
            }
            else if (tmpSerialKey == "ECO3-2G21-TECH-3MRT")     // EcoTech
            {
                if (flagPrintHeader == "yes")
                {
                    int totrec1 = 0;
                    List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                    lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
                    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));

                    Tblfooter.AddCell(c1.setCell("Mobile : +91 " + lstOrg[0].Landline1.ToString(), c1.WhiteBaseColor, c1.fnCalibri7, c1.paddingOf1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 1));
                    Tblfooter.AddCell(c1.setCell("Email : " + lstOrg[0].EmailAddress.ToString(), c1.WhiteBaseColor, c1.fnCalibri7, c1.paddingOf1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    Tblfooter.AddCell(c1.setCell(lstOrg[0].Fax1.ToString(), c1.WhiteBaseColor, c1.fnCalibri7, c1.paddingOf1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    //Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "SHSI-JA98-NA3S-51SD")     // Sahajanand
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = 0;
                    Tblfooter.TotalWidth = (document.PageSize.Width);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, 0, document.GetBottom(0), writer.DirectContent);
                    //AddPageNumber(writer, document);
                }
            }
            else if (tmpSerialKey == "MR09-DF34-TP45-55PE")          // Mudra Computers
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);

                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "JAYJ-ALAR-AMBR-ICKS")          // Jay Jalaram
            {
                if (flagPrintHeader == "yes")
                {
                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);

                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                    Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                }
            }
            else if (tmpSerialKey == "EUSI-LI85-4SL5-88GT")          // EuroLight
            {
                if (flagPrintModule == "quotation")
                {
                    if (flagPrintFooter == "yes")
                    {
                        if (writer.PageNumber != 1)
                        {
                            int totrec1 = 0;
                            List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                            lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
                            //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
                            myPdfConstruct pdf = new myPdfConstruct();
                            PdfPTable Tblfooter = new PdfPTable(2);
                            int[] column_Tblfooter = { 50, 50 };
                            Tblfooter.SetWidths(column_Tblfooter);
                            Tblfooter.AddCell(pdf.setCellTransparant("P a g e", pdf.WhiteBaseColor, pdf.fnCalibriGray8, pdf.paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                            Tblfooter.AddCell(pdf.setCellTransparant(writer.PageNumber.ToString() + " |", pdf.WhiteBaseColor, pdf.fnCalibriGray8, pdf.paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                            Tblfooter.AddCell(pdf.setCellTransparantBorderColor(lstOrg[0].Fax1 + " | " + "Confidential", pdf.WhiteBaseColor, pdf.fnCalibriGray8, new iTextSharp.text.BaseColor(128, 128, 128), pdf.paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_TOP, 1));
                            Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                            Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                            Tblfooter.SpacingBefore = 0f;
                            Tblfooter.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
                            Tblfooter.WriteSelectedRows(0, -1, document.LeftMargin, document.GetBottom(0), writer.DirectContent);
                        }
                    }

                    //-------------------Experiment to Put Border--------------------
                    var content = writer.DirectContent;
                    var pageBorderRect = new Rectangle(document.PageSize);

                    //pageBorderRect.Left += document.LeftMargin;
                    //pageBorderRect.Right -= document.RightMargin;
                    //pageBorderRect.Top -= document.TopMargin;
                    //pageBorderRect.Bottom += document.BottomMargin;

                    pageBorderRect.Left = 25;
                    pageBorderRect.Right = 570;
                    pageBorderRect.Top = 817;
                    pageBorderRect.Bottom = 25;

                    content.SetColorStroke(BaseColor.BLACK);
                    content.SetLineWidth(3);
                    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                    content.Stroke();
                }
            }
            else if (tmpSerialKey == "VAR2-DH0A-MAN9-8SIO")          // Vardhman
            {
                //-------------------Experiment to Put Border--------------------
                var content = writer.DirectContent;
                var pageBorderRect = new Rectangle(document.PageSize);

                pageBorderRect.Left = 25;
                pageBorderRect.Right = 570;
                pageBorderRect.Top = 817;
                pageBorderRect.Bottom = 25;

                content.SetColorStroke(BaseColor.BLACK);
                content.SetLineWidth(3);
                content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);

                content.Stroke();
            }
            else if (tmpSerialKey == "KH4O-D0IY-ARPL-A2ST")     // Khodiyar 
            {
                flagPrintFooter = "yes";
                if (flagPrintFooter == "yes")
                {

                    tmpFile11 = imagepath + "\\CompanyLogoFooter";
                    FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                    if (String.IsNullOrEmpty(FooterImage))
                        FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                    eSign.ScaleAbsolute(document.PageSize.Width, 90);
                    PdfPTable Tblfooter = new PdfPTable(1);
                    Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;
                    Tblfooter.DefaultCell.Border = 0;
                    Tblfooter.TotalWidth = (document.PageSize.Width - 5);
                    Tblfooter.AddCell(eSign);
                    Tblfooter.AddCell(cell);
                    Tblfooter.WriteSelectedRows(0, -1, 5, document.GetBottom(0), writer.DirectContent);
                }

            }
            else
            {
                tmpFile11 = imagepath + "\\CompanyLogoFooter";
                //tmpFile11 = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\QuotationFooter";

                FooterImage = File.Exists(tmpFile11 + ".jpeg") ? tmpFile11 + ".jpeg" : "";
                if (String.IsNullOrEmpty(FooterImage))
                    FooterImage = File.Exists(tmpFile11 + ".jpg") ? tmpFile11 + ".jpg" : "";
                if (String.IsNullOrEmpty(FooterImage))
                    FooterImage = File.Exists(tmpFile11 + ".png") ? tmpFile11 + ".png" : "";

                //string flagFooter = (string)HttpContext.Current.Session["PrintHeader"];
                string flagFooter = (string)StarsProject.QuotationEagle.printheader;


                if (!String.IsNullOrEmpty(flagFooter))
                {
                    if (flagFooter == "yes" || flagFooter == "Yes")
                    {
                        Paragraph footer = new Paragraph(Convert.ToString(writer.PageNumber), FontFactory.GetFont(FontFactory.TIMES, 1, iTextSharp.text.Font.NORMAL));
                        footer.Alignment = Element.ALIGN_RIGHT;
                        PdfPTable Tblfooter = new PdfPTable(1);
                        Tblfooter.HorizontalAlignment = Element.ALIGN_CENTER;

                        if (File.Exists(FooterImage) && tmpSerialKey == "LHUV-E36R-5PXM-2XAT")
                        {
                            cell.Border = 0;
                            //cell.PaddingLeft = 10;
                            Tblfooter.DefaultCell.Border = Rectangle.NO_BORDER;
                            iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(FooterImage);
                            eSign.ScaleAbsolute(document.PageSize.Width, 90);

                            Tblfooter.TotalWidth = document.PageSize.Width;
                            Tblfooter.AddCell(eSign);
                            Tblfooter.AddCell(cell);
                            Tblfooter.WriteSelectedRows(0, -1, 0, document.Bottom, writer.DirectContent);
                        }
                    }
                }
                // -------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(flagFooter))
                {
                    if (flagFooter.ToLower() == "yes" || flagFooter.ToLower() == "y")
                    {
                        // >>>> Option - 1
                        String strPageNo = "Page " + writer.PageNumber + " of ";
                        String strPrintDate = "Printed On " + DateTime.Now.ToString();

                        float lenPageNo = FooterFont.BaseFont.GetWidthPoint(strPageNo, 8);
                        float lenPrintDate = FooterFont.BaseFont.GetWidthPoint(strPrintDate, 8);
                        float lenFooterText = FooterFont.BaseFont.GetWidthPoint("Thanks for your bussiness", 8);

                        //watermarkImage.SetAbsolutePosition(100, 100);
                        //cb.AddImage(watermarkImage);

                        //cb.SetRGBColorFill(255, 0, 0);    // Setting up font color
                        //cb.SetColorFill(FooterFont.Color);

                        //cb.BeginText();
                        //cb.SetFontAndSize(FooterFont.BaseFont, 8);
                        //cb.SetTextMatrix(document.LeftMargin, pageSize.GetBottom(20));
                        //cb.ShowText(strPageNo);
                        //cb.EndText();
                        //cb.AddTemplate(pageNoTemplate, document.LeftMargin + lenPageNo, pageSize.GetBottom(20));

                        //cb.BeginText();
                        //cb.SetFontAndSize(FooterFont.BaseFont, 8);
                        //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, strPrintDate, (document.PageSize.Width - document.RightMargin), pageSize.GetBottom(20), 0);
                        //cb.EndText();

                        //if (!String.IsNullOrEmpty(FooterText))
                        //{
                        //    float xPos = ((document.PageSize.Width - lenFooterText) / 2);
                        //    cb.BeginText();
                        //    cb.SetFontAndSize(FooterFont.BaseFont, FooterFont.Size);
                        //    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, FooterText, xPos, pageSize.GetBottom(15), 0);
                        //    cb.EndText();
                        //}

                        // Move the pointer and draw line to separate footer section from rest of page
                        //cb.MoveTo(0, document.PageSize.GetBottom(30));
                        //cb.LineTo(document.PageSize.Width, document.PageSize.GetBottom(30));
                        //cb.Stroke();
                    }
                }

            }
        }
        private void AddPageNumber(PdfWriter writer, Document document)
        {
            var text = "Page No. " + writer.PageNumber.ToString();

            var numberTable = new PdfPTable(1);
            var numberCell = new PdfPCell(new Phrase(text, _FooterFont)) { HorizontalAlignment = Element.ALIGN_RIGHT };
            numberTable.AddCell(numberCell);
            numberTable.DefaultCell.Border = 0;
            numberTable.DefaultCell.Border = Rectangle.NO_BORDER;
            numberTable.TotalWidth = 65;
            numberTable.WriteSelectedRows(0, -1, document.Right - 80, document.Bottom + 20, writer.DirectContent);
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            pageNoTemplate.BeginText();
            pageNoTemplate.SetFontAndSize(FooterFont.BaseFont, 8);
            pageNoTemplate.SetTextMatrix(0, 0);
            pageNoTemplate.ShowText("" + (writer.PageNumber - 1));
            pageNoTemplate.EndText();
        }
    }
}

public class myPdfConstruct
{
    public string _pdfType = "";
    public static Int64 pageCount = 1;
    // --------------------------------------------------------------
    public string pdfType
    {
        get { return _pdfType; }
        set { _pdfType = value; }
    }

    public myPdfConstruct() { }

    public myPdfConstruct(string xType)
    {
        pdfType = xType;
    }

    public myPdfConstruct(iTextSharp.text.Document pDoc, String pQuotNo, String pCustName)
    {
        mydoc = pDoc;
        QuotationNo = pQuotNo;
        CustomerName = pCustName;
    }

    class RoundedBorder : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle rect, PdfContentByte[] canvas)
        {
            PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.RoundRectangle(
              rect.Left + 2f,
              rect.Bottom + 2f,
              rect.Width - 3,
              rect.Height - 3, 5
            );
            cb.Stroke();
        }
    }
    // --------------------------------------------------------------

    public float paddingOf1 = 1, paddingOf2 = 2, paddingOf3 = 3, paddingOf4 = 4, paddingOf5 = 5, paddingOf6 = 6, paddingOf8 = 8, paddingOf15 = 15, paddingOf18 = 18, paddingOf10 = 10;
    public static BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    public iTextSharp.text.BaseColor TransparantColor = new iTextSharp.text.BaseColor(226, 226, 226);
    public iTextSharp.text.BaseColor GreenBaseColor = new iTextSharp.text.BaseColor(60, 179, 113);
    public iTextSharp.text.BaseColor WhiteBaseColor = new iTextSharp.text.BaseColor(255, 255, 255);
    public iTextSharp.text.BaseColor SalmonBaseColor = new iTextSharp.text.BaseColor(248, 206, 194);
    public iTextSharp.text.BaseColor WhiteTransBaseColor = new iTextSharp.text.BaseColor(255,255,255);
    public iTextSharp.text.BaseColor BlackBaseColor = new iTextSharp.text.BaseColor(0, 0, 0);
    public iTextSharp.text.BaseColor DarkGrayBaseColor = new iTextSharp.text.BaseColor(216, 216, 216);
    public iTextSharp.text.BaseColor GrayBaseColor = new iTextSharp.text.BaseColor(238, 238, 224);
    public iTextSharp.text.BaseColor NavyBaseColor = new iTextSharp.text.BaseColor(0, 0, 128);
    public iTextSharp.text.BaseColor LightBlueBaseColor = new iTextSharp.text.BaseColor(236, 240, 241);
    public iTextSharp.text.BaseColor MidBlueBaseColor = new iTextSharp.text.BaseColor(84, 141, 212);
    public iTextSharp.text.BaseColor OrangeBaseColor = new iTextSharp.text.BaseColor(243, 156, 18);
    public iTextSharp.text.BaseColor BlueBaseColor = new iTextSharp.text.BaseColor(31, 55, 101);
    public iTextSharp.text.BaseColor CyanBaseColor = new iTextSharp.text.BaseColor(11, 184, 220);
    public iTextSharp.text.BaseColor RedBaseColor = new iTextSharp.text.BaseColor(255, 0, 0);
    public iTextSharp.text.BaseColor LightShadedBLUEBase = new iTextSharp.text.BaseColor(222, 229, 236);
    public iTextSharp.text.BaseColor DarkBrownGrey = new iTextSharp.text.BaseColor(118, 118, 118);
    public iTextSharp.text.BaseColor LightGrayGainsboro = new iTextSharp.text.BaseColor(217, 217, 217);
    public iTextSharp.text.BaseColor Darkgrey = new iTextSharp.text.BaseColor(200, 196, 196);
    public iTextSharp.text.BaseColor CreamConcrete = new iTextSharp.text.BaseColor(243, 243, 243);
    public iTextSharp.text.BaseColor Lightgrey = new iTextSharp.text.BaseColor(220, 220, 220);
    public iTextSharp.text.BaseColor Deamgrey = new iTextSharp.text.BaseColor(232, 232, 232);
    public iTextSharp.text.BaseColor Peach_Orange = new iTextSharp.text.BaseColor(255, 204, 153);
    public iTextSharp.text.BaseColor Perano = new iTextSharp.text.BaseColor(164, 194, 244);


    public iTextSharp.text.Font objHeaderFont12 = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public iTextSharp.text.Font objHeaderFont14 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public iTextSharp.text.Font objHeaderFont16 = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public iTextSharp.text.Font objHeaderFont18 = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

    public iTextSharp.text.Font objHeaderFontWhite12 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
    public iTextSharp.text.Font objHeaderFontWhite14 = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
    public iTextSharp.text.Font objHeaderFontWhite16 = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
    public iTextSharp.text.Font objHeaderFontWhite18 = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.WHITE);

    public iTextSharp.text.Font objFooterFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);

    public iTextSharp.text.Font objContentFontTitleBlack = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public iTextSharp.text.Font objContentFontDataBlack = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
    public iTextSharp.text.Font objContentFontTitleWhite = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
    public iTextSharp.text.Font objContentFontDataWhite = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
    public iTextSharp.text.Font objContentFontTitleNavy = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 128));
    public iTextSharp.text.Font objContentFontDataNavy = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 128));

    public iTextSharp.text.Font objContentFontTitleBlack8 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
    public iTextSharp.text.Font objContentFontDataBlack8 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
    public iTextSharp.text.Font objContentFontTitleWhite8 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
    public iTextSharp.text.Font objContentFontDataWhite8 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);
    public iTextSharp.text.Font objContentFontTitleNavy8 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 128));
    public iTextSharp.text.Font objContentFontDataNavy8 = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 128));
    public Font fnCalibri4 = FontFactory.GetFont("Calibri", 4, BaseColor.BLACK);
    public Font fnCalibriBold4 = FontFactory.GetFont("Calibri", 4, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibri5 = FontFactory.GetFont("Calibri", 5, BaseColor.BLACK);
    public Font fnCalibriBold5 = FontFactory.GetFont("Calibri", 5, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibri6 = FontFactory.GetFont("Calibri", 6, BaseColor.BLACK);
    public Font fnCalibriBold6 = FontFactory.GetFont("Calibri", 6, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibri7 = FontFactory.GetFont("Calibri", 7, BaseColor.BLACK);
    public Font fnCalibriBold7 = FontFactory.GetFont("Calibri", 7, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibri8 = FontFactory.GetFont("Calibri", 8, BaseColor.BLACK);
    public Font fnCalibri8Red = FontFactory.GetFont("Calibri", 8, new iTextSharp.text.BaseColor(172, 4, 4));
    public Font fnCalibriGray8 = FontFactory.GetFont("Calibri", 8, Font.NORMAL, new iTextSharp.text.BaseColor(128, 128, 128));
    public Font fnCalibriSkyBlue8 = FontFactory.GetFont("Calibri", 8, Font.ITALIC, new iTextSharp.text.BaseColor(31, 118, 179));
    public Font fnCalibriSkyBlueBold8 = FontFactory.GetFont("Calibri", 8, Font.BOLD, new iTextSharp.text.BaseColor(31, 118, 179));
    public Font fnCalibriBold8 = FontFactory.GetFont("Calibri", 8, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBoldRed8 = FontFactory.GetFont("Calibri", 8, Font.BOLD, new iTextSharp.text.BaseColor(172, 4, 4));
    public Font fnCalibriBoldItalic8 = FontFactory.GetFont("Calibri", 8, Font.BOLDITALIC, BaseColor.BLACK);
    public Font fnCalibriItalic8 = FontFactory.GetFont("Calibri", 8, Font.ITALIC, BaseColor.BLACK);
    public Font fnCalibriBold15 = FontFactory.GetFont("Calibri", 15, Font.BOLD, BaseColor.BLACK);

    public Font fnCalibriBold8White = FontFactory.GetFont("Calibri", 8, Font.BOLD, BaseColor.WHITE);
    public Font fnCalibriBold9White = FontFactory.GetFont("Calibri", 9, Font.BOLD, BaseColor.WHITE);
    public Font fnCalibri10 = FontFactory.GetFont("Calibri", 10, BaseColor.BLACK);
    public Font fnCalibriBlue8 = FontFactory.GetFont("Calibri", 8, BaseColor.BLUE);
    public Font fnCalibriBlue10 = FontFactory.GetFont("Calibri", 10, BaseColor.BLUE);
    public Font fnCalibriGray10 = FontFactory.GetFont("Calibri", 10, new iTextSharp.text.BaseColor(128, 128, 128));
    public Font fnCalibriBold10 = FontFactory.GetFont("Calibri", 10, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBoldGray10 = FontFactory.GetFont("Calibri", 10, Font.BOLD, new iTextSharp.text.BaseColor(128, 128, 128));
    public Font fnCalibriItalic10 = FontFactory.GetFont("Calibri", 10, Font.ITALIC, BaseColor.BLACK);
    public Font fnCalibriBoldUnderline10 = FontFactory.GetFont("Calibri", 10, Font.UNDERLINE, BaseColor.BLACK);
    public Font fnCalibriBoldBlue10 = FontFactory.GetFont("Calibri", 10, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBoldNBlue10 = FontFactory.GetFont("Calibri", 10, Font.BOLD, new iTextSharp.text.BaseColor(0, 32, 96));

    public Font fnCalibri11 = FontFactory.GetFont("Calibri", 11, BaseColor.BLACK);
    public Font fnCalibriBlue11 = FontFactory.GetFont("Calibri", 11, BaseColor.BLUE);
    public Font fnCalibriGray11 = FontFactory.GetFont("Calibri", 11, new iTextSharp.text.BaseColor(128, 128, 128));
    public Font fnCalibriBold11 = FontFactory.GetFont("Calibri", 11, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBoldGray11 = FontFactory.GetFont("Calibri", 11, Font.BOLD, new iTextSharp.text.BaseColor(128, 128, 128));
    public Font fnCalibriItalic11 = FontFactory.GetFont("Calibri", 11, Font.ITALIC, BaseColor.BLACK);
    public Font fnCalibriBoldUnderline11 = FontFactory.GetFont("Calibri", 11, Font.UNDERLINE, BaseColor.BLACK);
    public Font fnCalibriBoldBlue11 = FontFactory.GetFont("Calibri", 11, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBoldNBlue11 = FontFactory.GetFont("Calibri", 11, Font.BOLD, new iTextSharp.text.BaseColor(0, 32, 96));

    public Font fnCalibriItalicBBoldBlue10 = FontFactory.GetFont("Calibri", 10, Font.BOLDITALIC, BaseColor.BLUE);

    public Font fnCalibriBold10Red = FontFactory.GetFont("Calibri", 10, Font.BOLD, BaseColor.RED);

    public Font fnCalibri12Green = FontFactory.GetFont("Calibri", 12, new iTextSharp.text.BaseColor(51,142,43));
    public Font fnCalibriBold12Green = FontFactory.GetFont("Calibri", 12, Font.BOLD,new iTextSharp.text.BaseColor(51, 142, 43));
    public Font fnCalibriItalicBBoldBlue12 = FontFactory.GetFont("Calibri", 12, Font.BOLDITALIC, BaseColor.BLUE);
    public Font fnCalibriBold10White = FontFactory.GetFont("Calibri", 10, Font.BOLD, BaseColor.WHITE);
    public Font fnCalibri12 = FontFactory.GetFont("Calibri", 12, BaseColor.BLACK);
    public Font fnCalibriBold12 = FontFactory.GetFont("Calibri", 12, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold12White = FontFactory.GetFont("Calibri", 12, Font.BOLD, BaseColor.WHITE);
    public Font fnCalibriBold12Red = FontFactory.GetFont("Calibri", 12, Font.BOLD, new iTextSharp.text.BaseColor(255, 0, 0));
    public Font fnCalibriBold12Blue = FontFactory.GetFont("Calibri", 12, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBold12MidnightBlue = FontFactory.GetFont("Calibri", 12, Font.BOLD, new iTextSharp.text.BaseColor(0, 31, 95));
    public Font fnCalibri14 = FontFactory.GetFont("Calibri", 14, BaseColor.BLACK);
    public Font fnCalibri14BoldRed = FontFactory.GetFont("Calibri", 14, Font.BOLD, new iTextSharp.text.BaseColor(255, 0, 0));
    public Font fnCalibriBold14Bule = FontFactory.GetFont("Calibri", 14, Font.BOLD, new iTextSharp.text.BaseColor(0, 32, 96));
    public Font fnCalibriBold13 = FontFactory.GetFont("Calibri", 13, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold14 = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold16 = FontFactory.GetFont("Calibri", 16, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold17 = FontFactory.GetFont("Calibri", 17, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold18 = FontFactory.GetFont("Calibri", 18, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold14White = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.WHITE);
    public Font fnCalibriBold14Blue = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBold16Blue = FontFactory.GetFont("Calibri", 16, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBold18Blue = FontFactory.GetFont("Calibri", 18, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBold18DarkBlue = FontFactory.GetFont("Calibri", 18, Font.BOLD, new iTextSharp.text.BaseColor(26, 26, 64));
    public Font fnCalibriBold18Red = FontFactory.GetFont("Calibri", 18, Font.BOLD, BaseColor.RED);
    public Font fnCalibri20 = FontFactory.GetFont("Calibri", 20, BaseColor.BLACK);
    public Font fnCalibriBold20 = FontFactory.GetFont("Calibri", 20, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold20White = FontFactory.GetFont("Calibri", 20, Font.BOLD, BaseColor.WHITE);
    public Font fnCalibriBold24Blue = FontFactory.GetFont("Calibri", 24, Font.BOLD, BaseColor.BLUE);
    public Font fnCalibriBold24MidnightBlue = FontFactory.GetFont("Calibri", 24, Font.BOLD, new iTextSharp.text.BaseColor(0, 31, 95));
    public Font fnCalibriBold18MidnightBlue = FontFactory.GetFont("Calibri", 18, Font.BOLD, new iTextSharp.text.BaseColor(0, 31, 95));

    public Font fnCalibri9 = FontFactory.GetFont("Calibri", 9, BaseColor.BLACK);
    public Font fnCalibri9Silver = FontFactory.GetFont("Calibri", 9, Font.NORMAL, new iTextSharp.text.BaseColor(192,192,192));
    public Font fnCalibriBold9 = FontFactory.GetFont("Calibri", 9, Font.BOLD, BaseColor.BLACK);
    public Font fnCalibriBold9Silver = FontFactory.GetFont("Calibri", 9, Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
    public Font fnCalibriItalic9 = FontFactory.GetFont("Calibri", 9, Font.ITALIC, BaseColor.BLACK);
    public Font fnCalibriItalic9Silver = FontFactory.GetFont("Calibri", 9, Font.ITALIC, new iTextSharp.text.BaseColor(192, 192, 192));
    public Font fnCalibriItalicBold9 = FontFactory.GetFont("Calibri", 9, Font.BOLDITALIC, BaseColor.BLACK);
    public Font fnCalibriItalicBold10 = FontFactory.GetFont("Calibri", 10, Font.BOLDITALIC, BaseColor.BLACK);
    public Font fnCalibriItalicBold8 = FontFactory.GetFont("Calibri", 8, Font.BOLDITALIC, BaseColor.BLACK);
    public Font fnCalibriItalicBold9Silver = FontFactory.GetFont("Calibri", 9, Font.BOLDITALIC, new iTextSharp.text.BaseColor(192, 192, 192));
    public Font fnCalibriBold12Silver = FontFactory.GetFont("Calibri", 12, Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
    public Font fnCambriaSkyBlueBold10 = FontFactory.GetFont("Cambria", 10, Font.BOLD, new iTextSharp.text.BaseColor(31, 118, 179));
    public Font fnCambria10 = FontFactory.GetFont("Cambria", 10, BaseColor.BLACK);
    public Font fnCambriaBold10 = FontFactory.GetFont("Cambria", 10, Font.BOLD, BaseColor.BLACK);
    public Font fnCambria11 = FontFactory.GetFont("Cambria", 11, BaseColor.BLACK);
    public Font fnCambriaBold11 = FontFactory.GetFont("Cambria", 11, Font.BOLD, BaseColor.BLACK);
    public Font fnCambria12 = FontFactory.GetFont("Cambria", 12, BaseColor.BLACK);
    public Font fnCambriaBold12 = FontFactory.GetFont("Cambria", 12, Font.BOLD, BaseColor.BLACK);
    public Font fnArial8Red = FontFactory.GetFont("Arial", 8, new iTextSharp.text.BaseColor(172, 4, 4));
    public Font fnArial8 = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
    public Font fnArialBold8 = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.BLACK);
    public Font fnArial9 = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
    public Font fnArialBold9 = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK);
    public Font fnArial10 = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
    public Font fnArialBold10 = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
    public Font fnArialBold10White = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
    public Font fnArial12 = FontFactory.GetFont("Arial", 12, BaseColor.BLACK);
    public Font fnArialBold12 = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
    public Font fnArial14 = FontFactory.GetFont("Arial", 14, BaseColor.BLACK);
    public Font fnArialBold14 = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);


 
    public Font fntrebuchet8 = FontFactory.GetFont("trebuchet ms", 8, BaseColor.BLACK);
    public Font fntrebuchetBold8 = FontFactory.GetFont("trebuchet ms", 8, Font.BOLD, BaseColor.BLACK);
    public Font fntrebuchetBold8White = FontFactory.GetFont("trebuchet ms", 8, Font.BOLD, BaseColor.WHITE);
    public Font fntrebuchet10 = FontFactory.GetFont("trebuchet ms", 10, BaseColor.BLACK);
    public Font fntrebuchetBold10 = FontFactory.GetFont("trebuchet ms", 10, Font.BOLD, BaseColor.BLACK);
    public Font fntrebuchetBold10White = FontFactory.GetFont("trebuchet ms", 10, Font.BOLD, BaseColor.WHITE);
    public Font fntrebuchet12 = FontFactory.GetFont("trebuchet ms", 12, BaseColor.BLACK);
    public Font fntrebuchetBold12 = FontFactory.GetFont("trebuchet ms", 12, Font.BOLD, BaseColor.BLACK);
    public Font fntrebuchet14 = FontFactory.GetFont("trebuchet ms", 14, BaseColor.BLACK);
    public Font fntrebuchetBold14 = FontFactory.GetFont("trebuchet ms", 14, Font.BOLD, BaseColor.BLACK);

    public Font fnTimes8 = FontFactory.GetFont("Times New Roman", 8, BaseColor.BLACK);
    public Font fnTimesBold8 = FontFactory.GetFont("Times New Roman", 8, Font.BOLD, BaseColor.BLACK);
    public Font fnTimes10 = FontFactory.GetFont("Times New Roman", 10, BaseColor.BLACK);
    public Font fnTimesBold10 = FontFactory.GetFont("Times New Roman", 10, Font.BOLD, BaseColor.BLACK);
    public Font fnTimes12 = FontFactory.GetFont("Times New Roman", 12, BaseColor.BLACK);
    public Font fnTimesBold12 = FontFactory.GetFont("Times New Roman", 12, Font.BOLD, BaseColor.BLACK);
    public Font fnTimes14 = FontFactory.GetFont("Times New Roman", 14, BaseColor.BLACK);
    public Font fnTimesBold14 = FontFactory.GetFont("Times New Roman", 14, Font.BOLD, BaseColor.BLACK);
    public Font fnTimes16 = FontFactory.GetFont("Times New Roman", 16, BaseColor.BLACK);
    public Font fnTimesBold16 = FontFactory.GetFont("Times New Roman", 16, Font.BOLD, BaseColor.BLACK);
    public Font fnTimes18 = FontFactory.GetFont("Times New Roman", 18, BaseColor.BLACK);
    public Font fnTimesBold18 = FontFactory.GetFont("Times New Roman", 18, Font.BOLD, BaseColor.BLACK);
    public Font fnTimes20 = FontFactory.GetFont("Times New Roman", 20, BaseColor.BLACK);
    public Font fnTimesBold20 = FontFactory.GetFont("Times New Roman", 20, Font.BOLD, BaseColor.BLACK);

    public Font fnTimesRoman8 = FontFactory.GetFont("Times-Roman", 8, BaseColor.BLACK);
    public Font fnTimesRomanBold8 = FontFactory.GetFont("Times-Roman", 8, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRoman11 = FontFactory.GetFont("Times-Roman", 11, BaseColor.BLACK);
    public Font fnTimesRomanBold11 = FontFactory.GetFont("Times-Roman", 11, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRoman9 = FontFactory.GetFont("Times-Roman", 9, BaseColor.BLACK);
    public Font fnTimesRomanBold9 = FontFactory.GetFont("Times-Roman", 9, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRoman10 = FontFactory.GetFont("Times-Roman", 10, BaseColor.BLACK);
    public Font fnTimesRomanBold10 = FontFactory.GetFont("Times-Roman", 10, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRoman12 = FontFactory.GetFont("Times-Roman", 12, BaseColor.BLACK);
    public Font fnTimesRoman12Orange = FontFactory.GetFont("Times-Roman", 12, new iTextSharp.text.BaseColor(255, 77, 0));
    public Font fnTimesRoman14Blue = FontFactory.GetFont("Times-Roman", 14, BaseColor.BLUE);
    public Font fnTimesRoman12Blue = FontFactory.GetFont("Times-Roman", 12, BaseColor.BLUE);
    public Font fnTimesRoman12Pink = FontFactory.GetFont("Times-Roman", 12, Font.BOLD, new iTextSharp.text.BaseColor(249, 72, 146));
    public Font fnTimesRomanBold12Blue = FontFactory.GetFont("Times-Roman", 12, Font.BOLD, BaseColor.BLUE); 
    public Font fnTimesRomanBold12 = FontFactory.GetFont("Times-Roman", 12, Font.BOLD, BaseColor.BLACK); 
    public Font fnTimesRoman14 = FontFactory.GetFont("Times-Roman", 14, BaseColor.BLACK);
    public Font fnTimesRoman13FlashWhite = FontFactory.GetFont("Times-Roman", 14, new iTextSharp.text.BaseColor(169, 169, 169));
    public Font fnTimesRomanBold14 = FontFactory.GetFont("Times-Roman", 14, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRomanBold14Blue = FontFactory.GetFont("Times-Roman", 14, Font.BOLD, BaseColor.BLUE);
    public Font fnTimesRomanBold14Pink = FontFactory.GetFont("Times-Roman", 14, Font.BOLD, new iTextSharp.text.BaseColor(249, 72, 146));
    public Font fnTimesRoman16 = FontFactory.GetFont("Times-Roman", 16, BaseColor.BLACK);
    public Font fnTimesRomanBold16 = FontFactory.GetFont("Times-Roman", 16, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRoman18 = FontFactory.GetFont("Times-Roman", 18, BaseColor.BLACK);
    public Font fnTimesRomanBold18 = FontFactory.GetFont("Times-Roman", 18, Font.BOLD, BaseColor.BLACK);
    public Font fnTimesRoman20 = FontFactory.GetFont("Times-Roman", 20, BaseColor.BLACK);
    public Font fnTimesRomanRomanBold20 = FontFactory.GetFont("Times-Roman", 20, Font.BOLD, BaseColor.BLACK);

    public Font fnCourier7 = FontFactory.GetFont("Courier", 7, BaseColor.BLACK);
    public Font fnCourierBold7 = FontFactory.GetFont("Courier", 7, Font.BOLD, BaseColor.BLACK);
    public Font fnCourier8 = FontFactory.GetFont("Courier", 8, BaseColor.BLACK);
    public Font fnCourierBold8 = FontFactory.GetFont("Courier", 8, Font.BOLD, BaseColor.BLACK);
    public Font fnCourier10 = FontFactory.GetFont("Courier", 10, BaseColor.BLACK);
    public Font fnCourierBold10 = FontFactory.GetFont("Courier", 10, Font.BOLD, BaseColor.BLACK);
    public Font fnCourier12 = FontFactory.GetFont("Courier", 12, BaseColor.BLACK);
    public Font fnCourierBold12 = FontFactory.GetFont("Courier", 12, Font.BOLD, BaseColor.BLACK);
    public Font fnCourier14 = FontFactory.GetFont("Courier", 14, BaseColor.BLACK);
    public Font fnCourierBold14 = FontFactory.GetFont("Courier", 14, Font.BOLD, BaseColor.BLACK);
    public Font fnCourier16 = FontFactory.GetFont("Courier", 16, BaseColor.BLACK);
    public Font fnCourierBold16 = FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK);

    #region Properties
    private string _QuotationNo;
    public string QuotationNo
    {
        get { return _QuotationNo; }
        set { _QuotationNo = value; }
    }
    private Int64 _CustomerID;
    public Int64 CustomerID
    {
        get { return _CustomerID; }
        set { _CustomerID = value; }
    }
    private string _CustomerName;
    public string CustomerName
    {
        get { return _CustomerName; }
        set { _CustomerName = value; }
    }

    private iTextSharp.text.Document _mydoc;
    public iTextSharp.text.Document mydoc
    {
        get { return _mydoc; }
        set { _mydoc = value; }
    }

    private string _printModule;
    public string printModule
    {
        get { return _printModule; }
        set { _printModule = value.ToLower(); }
    }
    #endregion

    public PdfPTable setPdfPTable(Document tmpPdf, int tblCols, int tblWidth, int befSpec, int aftSpec, int hAlign = Element.ALIGN_LEFT)
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

    public PdfPTable setPdfPTable(Document tmpPdf, int tblCols, int tblWidth, string befaftSpac, int hAlign = Element.ALIGN_LEFT)
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

    public PdfPCell setCellRoundBorder(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(objTable);
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //tmpCell.Border = borderVal;
        tmpCell.CellEvent = new RoundedBorder();
        return tmpCell;
    }



    public PdfPCell setCellRoundBorder(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell; 
    }

    public PdfPCell setCellFixedHeight(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {

        PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.FixedHeight = 15f;
        return tmpCell;
    }
    public PdfPCell setCellFixedHeight(Phrase phrase, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {

        PdfPCell tmpCell = new PdfPCell(phrase);
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.FixedHeight = 15f;
        return tmpCell;
    }

    public PdfPCell setCell(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {

        PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }
    public PdfPCell setCellBoldUnboldDarshanSafty(string boldStr, string normalstr, BaseColor bc, iTextSharp.text.Font Bfn, iTextSharp.text.Font fn, float pad = 10, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        Phrase p = new Phrase();
        p.Add(new Chunk(boldStr, Bfn));
        p.Add(new Chunk(normalstr, fn));
        Paragraph ph = new Paragraph();
        ph.Add(p);

        PdfPCell tmpCell = new PdfPCell(new Phrase(ph));
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }
    public PdfPCell setCellBoldUnbold(string boldStr, string normalstr, BaseColor bc, iTextSharp.text.Font Bfn, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        Phrase p = new Phrase();
        p.Add(new Chunk(boldStr, Bfn));
        p.Add(new Chunk(normalstr, fn));
        Paragraph ph = new Paragraph();
        ph.Add(p);

        PdfPCell tmpCell = new PdfPCell(new Phrase(ph));
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }
    public PdfPCell setCell(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float[] padTRBL, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCell(Phrase phrase0, BaseColor bc, iTextSharp.text.Font fn, float padTRBL, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(phrase0);
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = padTRBL;
        //tmpCell.PaddingTop = (padTRBL[0] != null) ? padTRBL[0] : 0;
        //tmpCell.PaddingRight = (padTRBL[1] != null) ? padTRBL[1] : 0;
        //tmpCell.PaddingBottom = (padTRBL[2] != null) ? padTRBL[2] : 0;
        //tmpCell.PaddingLeft = (padTRBL[3] != null) ? padTRBL[3] : 0;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;

        return tmpCell;
    }
    public PdfPCell setCell(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCellFixImage(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCell(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCell(PdfPCell objCell, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(objCell);
        tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = borderVal;
        return tmpCell;
    }

    public Paragraph setParagraph(string txtStr, BaseColor bc, iTextSharp.text.Font fn, int hAlign = Element.ALIGN_LEFT)
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
    // ------------------------------------------------------------
    public PdfPCell setCellTransparantBorderColor(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, BaseColor BorderColor, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(objTable);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.BorderColor = BorderColor;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }

    public PdfPCell setCellTransparantBorderColor(Phrase phrase0, BaseColor bc, iTextSharp.text.Font fn, BaseColor BorderColor, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(phrase0);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.BorderColor = BorderColor;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }


    public PdfPCell setCellTransparantBorderColorWidth(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, BaseColor BorderColor, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15,int BorderWidth = 1)
    {
        PdfPCell tmpCell = new PdfPCell(objTable);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.BorderColor = BorderColor;
        tmpCell.BorderWidth = BorderWidth;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }
    public PdfPCell setCellTransparantBorderColorWidth(string txtStr, BaseColor bc, iTextSharp.text.Font fn, BaseColor BorderColor, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15, int BorderWidth = 1)
    {
        PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.BorderColor = BorderColor;
        tmpCell.BorderWidth = BorderWidth;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }
    public PdfPCell setCellTransparantBorderColor(string txtStr, BaseColor bc, iTextSharp.text.Font fn, BaseColor BorderColor , float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.BorderColor = BorderColor;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }

    public PdfPCell setCellTransparantBorderColor(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, BaseColor BorderColor, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(pImage, false);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        tmpCell.BorderColor = BorderColor;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }

    // ------------------------------------------------------------
    public PdfPCell setCellTransparant(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;
        //if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")
        //    tmpCell.FixedHeight = 18f;
        return tmpCell;
    }
    public PdfPCell setCellTransparant(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float[] padTRBL, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCellTransparant(Phrase phrase0, BaseColor bc, iTextSharp.text.Font fn, float padTRBL, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(phrase0);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = padTRBL;
        //tmpCell.PaddingTop = (padTRBL[0] != null) ? padTRBL[0] : 0;
        //tmpCell.PaddingRight = (padTRBL[1] != null) ? padTRBL[1] : 0;
        //tmpCell.PaddingBottom = (padTRBL[2] != null) ? padTRBL[2] : 0;
        //tmpCell.PaddingLeft = (padTRBL[3] != null) ? padTRBL[3] : 0;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;

        return tmpCell;
    }
    public PdfPCell setCellTransparant(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCellFixImageTransparant(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(pImage, false);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        tmpCell.Border = borderVal;

        return tmpCell;
    }
    public PdfPCell setCellTransparant(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
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
    public PdfPCell setCellTransparant(PdfPCell objCell, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPCell tmpCell = new PdfPCell(objCell);
        //tmpCell.BackgroundColor = bc;
        tmpCell.Padding = pad;
        tmpCell.Colspan = colspn;
        tmpCell.HorizontalAlignment = hAlign;
        tmpCell.VerticalAlignment = vAlign;
        tmpCell.Border = borderVal;
        return tmpCell;
    }

    public Paragraph setParagraphTransparant(string txtStr, BaseColor bc, iTextSharp.text.Font fn, int hAlign = Element.ALIGN_LEFT)
    {
        Paragraph p1 = new Paragraph();
        Chunk c1 = new Chunk(txtStr);
        //c1.SetBackground(bc);
        c1.Font = fn;

        p1.Add(c1);
        p1.Alignment = hAlign;

        return p1;
    }

    // ------------------------------------------------------------
    public void addWallPaper(Document xDoc, string xImagePath, float xPos = 0, float yPos = 0, float xWidth = 10, float xHeight = 10)
    {
        if (!String.IsNullOrEmpty(xImagePath))
        {
            var objWall = iTextSharp.text.Image.GetInstance(findProductImage(xImagePath));
            //var objWall = iTextSharp.text.Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath(xImagePath));
            objWall.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            objWall.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            objWall.BorderColor = BaseColor.GRAY;
            objWall.BorderWidth = 1f;
            objWall.SpacingBefore = 150;
            objWall.ScaleAbsolute(xWidth - 100f, xHeight - 100f);

            xDoc.Add(objWall);
        }

    }

    

    //public PdfPCell setCell(string txtStr, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    //{
    //    PdfPCell tmpCell = new PdfPCell(new Phrase(txtStr, fn));
    //    tmpCell.BackgroundColor = bc;
    //    tmpCell.Padding = pad;
    //    tmpCell.Colspan = colspn;
    //    tmpCell.HorizontalAlignment = hAlign;
    //    tmpCell.VerticalAlignment = vAlign;
    //    tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    tmpCell.Border = borderVal;

    //    return tmpCell;
    //}

    //public PdfPCell setCell(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    //{
    //    PdfPCell tmpCell = new PdfPCell(pImage, true);
    //    tmpCell.BackgroundColor = bc;
    //    tmpCell.Padding = pad;
    //    tmpCell.Colspan = colspn;
    //    tmpCell.HorizontalAlignment = hAlign;
    //    tmpCell.VerticalAlignment = vAlign;
    //    tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //    tmpCell.Border = borderVal;

    //    return tmpCell;
    //}

    //public PdfPCell setCell(PdfPTable objTable, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    //{
    //    PdfPCell tmpCell = new PdfPCell(objTable);
    //    tmpCell.BackgroundColor = bc;
    //    tmpCell.Padding = pad;
    //    tmpCell.Colspan = colspn;
    //    tmpCell.HorizontalAlignment = hAlign;
    //    tmpCell.VerticalAlignment = vAlign;
    //    return tmpCell;
    //}

    public byte[] showQRCode()
    {

        byte[] returnValue = null;

        if (!String.IsNullOrEmpty(QuotationNo) && !String.IsNullOrEmpty(CustomerName))
        {
            string code = QuotationNo + "," + CustomerName;
            //string encCode = pb.Encrypt(code, "r0b1nr0y");
            string encCode = code;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(encCode, QRCodeGenerator.ECCLevel.Q);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

            imgBarCode.Style.Add("top", "-15px !important");
            imgBarCode.Style.Add("height", "150px !important");
            imgBarCode.Style.Add("width", "150px !important");
            imgBarCode.Style.Add("margin-top", "10px !important");
            imgBarCode.Style.Add("float", "right");
            imgBarCode.Style.Add("position", "relative");
            imgBarCode.Style.Add("padding-right", "10px");

            using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    returnValue = byteImage;
                }
            }
        }
        return returnValue;
    }

    public Document initiatePage(List<Entity.DocPrinterSettings> lstPrinter, string pModule = "")
    {
        Document tmpPdfDoc = new Document(iTextSharp.text.PageSize.A4);
        // -------------------------------------------------------------------------
        pageCount = 1;
        printModule = pModule;
        // -------------------------------------------------------------------------
        string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];

        Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 10, RightMargin = 10;
        Int64 ProdDetail_Lines = 0;

        ProdDetail_Lines = String.IsNullOrEmpty(lstPrinter[0].ProdDetail_Lines.ToString()) ? 0 : Convert.ToInt64(lstPrinter[0].ProdDetail_Lines);

        if (flagPrintHeader == "yes" || flagPrintHeader == "y")
        {
            if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
            {
                String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                TopMargin = Convert.ToInt64(tmpary[0].ToString());
                BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                if (tmpary.Length > 2)
                {
                    LeftMargin = Convert.ToInt64(tmpary[2].ToString());
                    RightMargin = Convert.ToInt64(tmpary[3].ToString());
                }
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
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        // Declaring PDF Document Object
        // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        tmpPdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);
        tmpPdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
        tmpPdfDoc.AddCreationDate();
        mydoc = tmpPdfDoc;
        // ---------------------------------------------------------------
        return tmpPdfDoc;
    }

    public PdfPTable GenerateCompanyHeader()
    {
        PdfPTable tableHeader = new PdfPTable(2);
        PdfPCell cell;
        PdfPCell cell1;
        PdfPCell cell2;
        // --------------------------------------------------------------
        string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
        //string tmpSerialKey = StarsProject.QuotationEagle.serialkey.ToString();
        //string imagepath = StarsProject.QuotationEagle.imagepath.ToString();
        string imagepath = "";

        // --------------------------------------------------------------
        if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")     // SteelMan Gas
        {
            int[] column_tblNested2001 = { 55, 45 };
            tableHeader.SetWidths(column_tblNested2001);
        }
        else
        {
            int[] column_tblNested2001 = { 50, 50 };
            tableHeader.SetWidths(column_tblNested2001);
        }
        string HeaderImage = "";
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        //tableHeader.SpacingBefore = 10f;
        //tableHeader.SpacingAfter = 10f;
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        // --------------------------------------------------------------
        string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\QuotationHeader";
        //string tmpFile = imagepath + "\\QuotationHeader";

        HeaderImage = File.Exists(tmpFile + ".jpeg") ? tmpFile + ".jpeg" : "";
        if (String.IsNullOrEmpty(HeaderImage))
            HeaderImage = File.Exists(tmpFile + ".jpg") ? tmpFile + ".jpg" : "";
        if (String.IsNullOrEmpty(HeaderImage))
            HeaderImage = File.Exists(tmpFile + ".png") ? tmpFile + ".png" : "";

        //string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
        string flagPrintHeader = (string)StarsProject.QuotationEagle.printheader;

        if (flagPrintHeader == "yes" || flagPrintHeader == "Yes" || String.IsNullOrEmpty(flagPrintHeader))
        {
            if (File.Exists(HeaderImage) && tmpSerialKey == "LHUV-E36R-5PXM-2XAT")
            {
                iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(HeaderImage);
                eSign.ScaleAbsolute((mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin)), 110);
                tableHeader.AddCell(setCell(eSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            }
            else
            {
                PdfPTable tblNested1 = new PdfPTable(1);
                int[] column_tblNested1 = { 100 };
                tblNested1.SetWidths(column_tblNested1);
                // ---------------------------------------------------------------------------
                // Header Left Side - CompanyLogo/QRCode
                // ---------------------------------------------------------------------------
                int TotalCount = 0;
                List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
                lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
                String x1 = lstEntity[0].Address;

                String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                            ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                            ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + ", India";

                String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "");
                String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email Us : " + lstEntity[0].EmailAddress : "");
                String x5 = ((!String.IsNullOrEmpty(lstEntity[0].Fax1)) ? "Website : " + lstEntity[0].Fax1 : "");
                String x6 = ((!String.IsNullOrEmpty(lstEntity[0].GSTIN)) ? "GST No.: " + lstEntity[0].GSTIN : "");

                //-------------Company Details Table ---------------//
                List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
                lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 10000, out TotalCount);


                // --------------------------------------------------------------------
                if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")      // SteelMan Gas 
                {
                    string flagPrintUnit = (string)HttpContext.Current.Session["PrintUnitAddress"];
                    if (flagPrintUnit == "Unit I & II")
                    {
                        tblNested1.AddCell(setCell("Reg. Office & Factory unit I & II", WhiteBaseColor, fnArialBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell("Plt No. 21/20  Survey No. 439/2, Near Sun Hygiene Foods,", WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell("Rajkot - Muli Road, Village - Shekhpar, Taluka - Muli,", WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell("Dist. Surendranagar (Gujarat) Pin - 363510. Mo - 9825188035", WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell(" ", WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    }
                    else
                    {
                        tblNested1.AddCell(setCell("Reg. Office & Factory Unit III", WhiteBaseColor, fnArialBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell((!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : ""), WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell("Dist -" + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? lstOrg[0].CityName : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? " - " : "") +  (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? lstOrg[0].Pincode : ""), WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell("Mo - " + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 : "") + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? " , " : "") + (!String.IsNullOrEmpty(lstOrg[0].Landline2) ? lstOrg[0].Landline2 : "")  , WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                        tblNested1.AddCell(setCell(" ", WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    }
                }
                else
                {
                    cell1 = new PdfPCell(new Paragraph(lstEntity[0].OrgName, objHeaderFont18));
                    cell1.VerticalAlignment = Element.ALIGN_TOP;
                    //cell1.PaddingLeft = 2;
                    //cell1.PaddingTop = 6;
                    cell1.Border = 0;
                    tblNested1.AddCell(cell1);
                    tblNested1.AddCell(setCell(x1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    tblNested1.AddCell(setCell(x2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    if (!String.IsNullOrEmpty(x3))
                        tblNested1.AddCell(setCell(x3, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
                    if (!String.IsNullOrEmpty(x4))
                        tblNested1.AddCell(setCell(x4, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

                }
                cell1 = new PdfPCell(tblNested1);
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell1.Border = Rectangle.BOTTOM_BORDER;
                //cell1.BorderColor = BaseColor.BLACK;
                //cell1.PaddingBottom = 10f;

                // ---------------------------------------------------------------------------
                // Header Right Side - CompanyLogo/QRCode
                // ---------------------------------------------------------------------------
                iTextSharp.text.Image imgQRCode = null;
                cell2 = new PdfPCell();
                String flagQRCode = BAL.CommonMgmt.GetConstant("QuotationQRCode", 0, 1);
                if (String.IsNullOrEmpty(flagQRCode) || flagQRCode == "No" || String.IsNullOrEmpty(QuotationNo) || String.IsNullOrEmpty(CustomerName))
                {
                    //String companyLogo = System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\CompanyLogo\\") + "\\companylogo.png";

                    String companyLogo = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\CompanyLogo\\" + "companylogo.png";
                    if (File.Exists(companyLogo))
                    {
                        string h1 = "", w1 = "";
                        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
                            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();

                        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
                            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();

                        float imgWid, imgHeg;
                        if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")  // Steelman Gas
                        {
                            imgWid = (float)285;
                            imgHeg = (float)120;
                        }
                        //else if (tmpSerialKey == "AIRI-G3Y5-2T9E-YN9W")
                        //{
                        //    imgWid = (float)380;
                        //    imgHeg = (float)160;
                        //}
                        else
                        {
                            imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
                            imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;
                        }
                        iTextSharp.text.Image myImage11 = iTextSharp.text.Image.GetInstance(companyLogo);
                        myImage11.ScaleAbsolute(imgWid, imgHeg);
                        cell2 = new PdfPCell(myImage11);
                    }
                }
                else
                {
                    byte[] tmpVal = showQRCode();
                    MemoryStream ms1 = new MemoryStream(tmpVal);
                    System.Drawing.Image sdi = System.Drawing.Image.FromStream(ms1);
                    imgQRCode = iTextSharp.text.Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Png);

                    if (tmpVal != null)
                    {
                        imgQRCode.SetAbsolutePosition(mydoc.GetRight(80), mydoc.GetTop(90));
                        if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")
                            imgQRCode.ScaleToFit(95f, 90f);
                        else
                            imgQRCode.ScaleToFit(80f, 80f);
                        imgQRCode.Alignment = Element.ALIGN_RIGHT;
                        cell2 = new PdfPCell(imgQRCode);
                    }
                }

                //cell2.Border = PdfPCell.BOTTOM_BORDER;
                //cell2.BorderColor = BaseColor.BLACK;
                cell2.VerticalAlignment = Element.ALIGN_TOP;
                if (tmpSerialKey == "STX1-UP06-YU89-JK23")
                    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;

                //cell2.PaddingBottom = 10f;
                // -----------------------------------------------------

                if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")
                {
                    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                    tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                    tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, Rectangle.BOTTOM_BORDER));
                }
                else
                {
                    tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
                    tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));

                }
            }
        }
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }

    public PdfPTable GenerateCompanyHeader_AddressImage(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        //tableHeader.SpacingBefore = 10f;
        //tableHeader.SpacingAfter = 10f;
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell cell, cell1, cell2;

        // ---------------------------------------------------------------------------
        // Header Left Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(1);
        int[] column_tblNested1 = { 100 };
        tblNested1.SetWidths(column_tblNested1);
 

        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        String x = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address;

        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + "India";
        String x3 = "";
        if (tmpSerialKey == "STX1-UP06-YU89-JK23")
            x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "") + ", 9825034298";
        else
            x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "");
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email Us : " + lstEntity[0].EmailAddress : "");
        String x5 = ((!String.IsNullOrEmpty(lstEntity[0].Fax1)) ? "Website : " + lstEntity[0].Fax1 : "");
        String x6 = ((!String.IsNullOrEmpty(lstEntity[0].GSTIN)) ? "GST No. : " + lstEntity[0].GSTIN : "") + ((!String.IsNullOrEmpty(lstEntity[0].PANNO)) ? "       PAN No. : " + lstEntity[0].PANNO : "");

        if (tmpSerialKey == "ACSI-C803-CUP0-SHEL")
            tblNested1.AddCell(setCell(x, WhiteBaseColor, fnCalibriBold18Red, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        else
            tblNested1.AddCell(setCell(x, WhiteBaseColor, fnCalibriBold18, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        tblNested1.AddCell(setCell(x1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x3))
            tblNested1.AddCell(setCell(x3, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x4))
            tblNested1.AddCell(setCell(x4, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (tmpSerialKey == "STX1-UP06-YU89-JK23")
        {
            if (!String.IsNullOrEmpty(x5))
                tblNested1.AddCell(setCell(x5, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(x6))
                tblNested1.AddCell(setCell(x6, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        }
  

            cell1 = new PdfPCell(tblNested1);
        
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell1.Border = Rectangle.BOTTOM_BORDER;
        cell1.BorderColor = BaseColor.BLACK;
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));

        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();

        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();

        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;

        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }

    public PdfPTable GenerateCompanyHeader_AddressImageStainex(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        //tableHeader.SpacingBefore = 10f;
        //tableHeader.SpacingAfter = 10f;
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell cell, cell1, cell2;

        // ---------------------------------------------------------------------------
        // Header Left Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(2);
        int[] column_tblNested1 = { 22,78 };
        tblNested1.SetWidths(column_tblNested1);


        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);

        List<Entity.OrganizationStructure> lstEntity1 = new List<Entity.OrganizationStructure>();
        lstEntity1 = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("002", 1, 100, out TotalCount);
        String x = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address +
                    ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + "India"; ;
        String x2 = "";
        if (lstEntity1.Count > 0)
        {
            x2 = lstEntity1[0].Address +
                        ((!String.IsNullOrEmpty(lstEntity1[0].CityName)) ? lstEntity1[0].CityName + " - " : "") +
                        ((!String.IsNullOrEmpty(lstEntity1[0].Pincode)) ? lstEntity1[0].Pincode + ", " : "") +
                        ((!String.IsNullOrEmpty(lstEntity1[0].StateName)) ? lstEntity1[0].StateName + ", " : "") + "India"; ;
        }
        String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "") + ", 9825034298";
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? lstEntity[0].EmailAddress : "");
        String x5 = ((!String.IsNullOrEmpty(lstEntity[0].Fax1)) ? lstEntity[0].Fax1 : "");
        String x6 = ((!String.IsNullOrEmpty(lstEntity[0].GSTIN)) ? "GST No. : " + lstEntity[0].GSTIN : "") + ((!String.IsNullOrEmpty(lstEntity[0].PANNO)) ? "       PAN No. : " + lstEntity[0].PANNO : "");

        tblNested1.AddCell(setCell(x, WhiteBaseColor, fnCalibriBold18, paddingOf1, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell("Unit 1 : ", WhiteBaseColor, fnCalibriBold9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x1, WhiteBaseColor, fnCalibri9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell("Unit 2 : ", WhiteBaseColor, fnCalibriBold9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x2, WhiteBaseColor, fnCalibri9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell("Contact Us : ", WhiteBaseColor, fnCalibriBold9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(!String.IsNullOrEmpty(x3) ? x3 : "", WhiteBaseColor, fnCalibri9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell("Email Us : ", WhiteBaseColor, fnCalibriBold9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(!String.IsNullOrEmpty(x4) ? x4 : "", WhiteBaseColor, fnCalibri9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell("Website : ", WhiteBaseColor, fnCalibriBold9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(!String.IsNullOrEmpty(x5) ? x5 : "", WhiteBaseColor, fnCalibri9, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x6))
            tblNested1.AddCell(setCell(!String.IsNullOrEmpty(x6) ? x6 : "", WhiteBaseColor, fnCalibri9, 2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        cell1 = new PdfPCell(tblNested1);
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell1.Border = Rectangle.BOTTOM_BORDER;
        cell1.BorderColor = BaseColor.BLACK;
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));

        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();

        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();

        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;

        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }

    public PdfPTable GenerateCompanyHeader_AddressImagePRIMA(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        //tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        //tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        //tableHeader.SpacingBefore = 10f;
        //tableHeader.SpacingAfter = 10f;
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell cell, cell1, cell2;


        //---- Table for PRIMA
        PdfPTable tblNested2 = new PdfPTable(1);
        int[] column_tblNested2 = { 100 };
        tblNested2.SetWidths(column_tblNested2);

        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        String x = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address;

        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + "India";
        String x3 = "";
        if (tmpSerialKey == "STX1-UP06-YU89-JK23")
            x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "") + ", 9825034298";
        else
            x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "");
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email Us : " + lstEntity[0].EmailAddress : "");
        String x5 = ((!String.IsNullOrEmpty(lstEntity[0].Fax1)) ? "Website : " + lstEntity[0].Fax1 : "");
        String x6 = ((!String.IsNullOrEmpty(lstEntity[0].GSTIN)) ? "GST No. : " + lstEntity[0].GSTIN : "") + ((!String.IsNullOrEmpty(lstEntity[0].PANNO)) ? "       PAN No. : " + lstEntity[0].PANNO : "");

     
        // ---------------------------------------------------------------------------
        // ---------------------------------------------------------------------------
        //-- For Prima

        String Address11 = (!String.IsNullOrEmpty(lstEntity[0].Address) ? lstEntity[0].Address : "") + (!String.IsNullOrEmpty(lstEntity[0].Address) ? " , " : "")
                            + (!String.IsNullOrEmpty(lstEntity[0].Pincode) ? " - " : "") + (!String.IsNullOrEmpty(lstEntity[0].Pincode) ? lstEntity[0].Pincode : "");
        String City11 = (!String.IsNullOrEmpty(lstEntity[0].CityName) ? lstEntity[0].CityName : "") + (!String.IsNullOrEmpty(lstEntity[0].CityName) ? " , " : "")
                             + (!String.IsNullOrEmpty(lstEntity[0].StateName) ? lstEntity[0].StateName : "") + (!String.IsNullOrEmpty(lstEntity[0].StateName) ? " , " : "")
                             + "INDIA";
        String Contact11 = (!String.IsNullOrEmpty(lstEntity[0].Landline1) ? "Contact Us : " : "") + (!String.IsNullOrEmpty(lstEntity[0].Landline1) ? lstEntity[0].Landline1 : "") + (!String.IsNullOrEmpty(lstEntity[0].Landline2) ? " / " : "") + (!String.IsNullOrEmpty(lstEntity[0].Landline2) ? lstEntity[0].Landline2 : "");

        String Email11 = (!String.IsNullOrEmpty(lstEntity[0].EmailAddress) ? "Email Us : " : "") + (!String.IsNullOrEmpty(lstEntity[0].EmailAddress) ? lstEntity[0].EmailAddress : "") + (!String.IsNullOrEmpty(lstEntity[0].Fax2) ? " , " : "") + (!String.IsNullOrEmpty(lstEntity[0].Fax2) ? lstEntity[0].Fax2 : "");

        tblNested2.AddCell(setCell(Address11, WhiteBaseColor, fnTimesRoman10, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested2.AddCell(setCell(City11, WhiteBaseColor, fnTimesRoman10, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested2.AddCell(setCell(Email11, WhiteBaseColor, fnTimesRoman10, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested2.AddCell(setCell(Contact11, WhiteBaseColor, fnTimesRoman10, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

            cell1 = new PdfPCell(tblNested2);

        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        //cell1.Border = Rectangle.BOTTOM_BORDER;
        //cell1.BorderColor = BaseColor.BLACK;
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));

        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();

        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();

        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;

        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - 60);
        return tableHeader;
    }
    public PdfPTable GenerateCompanyHeader_EcoTech(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
        PdfPCell cell, cell1, cell2;
        // ---------------------------------------------------------------------------
        // Header Left Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(1);
        int[] column_tblNested1 = { 100 };
        tblNested1.SetWidths(column_tblNested1);
        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        String x = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address;

        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + "India";
        String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us : " + lstEntity[0].Landline1 : "") + ((!String.IsNullOrEmpty(lstEntity[0].Landline2)) ? ", " + lstEntity[0].Landline2 : "");
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email Us : " + lstEntity[0].EmailAddress : "");
        String x5 = ((!String.IsNullOrEmpty(lstEntity[0].Fax1)) ? "Website : " + lstEntity[0].Fax1 : "");
        String x6 = ((!String.IsNullOrEmpty(lstEntity[0].GSTIN)) ? "GST No. : " + lstEntity[0].GSTIN : "") + ((!String.IsNullOrEmpty(lstEntity[0].PANNO)) ? "       PAN No. : " + lstEntity[0].PANNO : "");


        tblNested1.AddCell(setCell(x, WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x1, WhiteBaseColor, fnCalibri8, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x2, WhiteBaseColor, fnCalibri8, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        cell1 = new PdfPCell(tblNested1);
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));

        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();

        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();

        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)65;
        float imgHeg = (!String.IsNullOrEmpty(h1)) ? float.Parse(h1) : (float)25;

        myLogo.ScaleAbsolute(130, 30);
        cell2 = new PdfPCell(myLogo);
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tableHeader.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }

    public PdfPTable GenerateCompanyHeader_ImageAddressParth(int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPTable tblLogo = new PdfPTable(1);
        int[] column_tblLogo = { 100 };
        tblLogo.SetWidths(column_tblLogo);
        int fileCount1 = 0;
        string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";


        if (File.Exists(tmpFile1))
        {
            if (File.Exists(tmpFile1))
            {
                PdfPTable tblSymbol = new PdfPTable(1);
                iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                eLoc.ScaleAbsolute(90, 90);
                tblSymbol.AddCell(setCellFixImage(eLoc, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(tblSymbol, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                fileCount1 = fileCount1 + 1;
            }
        }

        int totrec1 = 0;
        List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);

        PdfPTable tblExtra = new PdfPTable(2);
        int[] column_tblExtra = { 15, 75 };
        tblExtra.SetWidths(column_tblExtra);
        tblExtra.AddCell(setCell("Phone", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        tblExtra.AddCell(setCell("Email", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].EmailAddress) ? lstOrg[0].EmailAddress : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblExtra.AddCell(setCell("Website", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblExtra.AddCell(setCell(": " + "www.parthequipments.in , www.parthequipments.com", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblName = new PdfPTable(1);
        int[] column_tblName = { 100 };
        tblName.SetWidths(column_tblName);

        string address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : "") +
            (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : "") +
            (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " + lstOrg[0].Pincode : "");
        tblName.AddCell(setCell("MANUFACTURER, SUPPLIER & EXPORTER OF", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell("S.S.Commercial Kitchen Equipments for: Hotels, Restaurant, Food court, ", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell("industrial Canteens, Bakery Outlets, Live Counter.", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCellBoldUnbold("REG OFFICE: ", address, WhiteBaseColor, fnCalibriBold10, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(tblExtra, WhiteBaseColor, fnCalibri10, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblAddress = new PdfPTable(2);
        int[] column_tblAddress = { 35, 65 };
        tblAddress.SetWidths(column_tblAddress);
        tblAddress.SpacingAfter = 0f;
        tblAddress.SpacingBefore = 0f;
        tblAddress.LockedWidth = true;

        tblAddress.AddCell(setCell(tblLogo, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
        tblAddress.AddCell(setCell(tblName, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
        //tblAddress.AddCell(setCell(tblAddress, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tblAddress.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tblAddress;
    }

    public PdfPTable GenerateCompanyHeader_ImageAddressHemsanIndQT(int[] noOfColsStruc, string pImageFile)
    {
        //PdfPTable tableHeader = new PdfPTable(2);
        //tableHeader.SetWidths(noOfColsStruc);
        //tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        //tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        //tableHeader.LockedWidth = true;
        //tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        int totrec1 = 0;
        List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);


        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));

        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CompanyLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["CompanyLogoHeight"].ToString();

        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CompanyLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["CompanyLogoWidth"].ToString();

        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)178;
        float imgHeg = (!String.IsNullOrEmpty(h1)) ? float.Parse(h1) : (float)88;

        PdfPTable tblLogo = new PdfPTable(1);
        int[] column_tblLogo = { 100 };
        tblLogo.SetWidths(column_tblLogo);
        int fileCount1 = 0;
        string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";

        //tblLogo.AddCell(setCell("GST NO. " + lstOrg[0].GSTIN, WhiteBaseColor, fnCalibriBold8, paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblLogo.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        if (File.Exists(tmpFile1))
        {
            if (File.Exists(tmpFile1))
            {
                PdfPTable tblSymbol = new PdfPTable(1);
                iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                eLoc.ScaleAbsolute(imgWid, imgHeg);
                tblSymbol.AddCell(setCellFixImage(eLoc, WhiteBaseColor, fnCalibri9, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(tblSymbol, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                fileCount1 = fileCount1 + 1;
            }
        }



        //PdfPTable tblExtra = new PdfPTable(2);
        //int[] column_tblExtra = { 15, 75 };
        //tblExtra.SetWidths(column_tblExtra);
        //tblExtra.AddCell(setCell("Phone", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell("Email", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].EmailAddress) ? lstOrg[0].EmailAddress : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell("Website", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + "www.parthequipments.in , www.parthequipments.com", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblName = new PdfPTable(1);
        int[] column_tblName = { 100 };
        tblName.SetWidths(column_tblName);

        string address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : "") + (!String.IsNullOrEmpty(lstOrg[0].Address) ? " , " : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? lstOrg[0].CityName : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? " , " : "") + (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " : "") + (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? lstOrg[0].Pincode : "");

        //tblName.AddCell(setCell(lstOrg[0].OrgName, WhiteBaseColor, fnCalibriBold18DarkBlue, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(address, WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCellBoldUnbold(lstOrg[0].EmailAddress,  ",    " + "Phone:" + lstOrg[0].Landline1, WhiteBaseColor, fnCalibriBoldBlue10, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblName.AddCell(setCell("Email ID: " + lstOrg[0].EmailAddress, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        

        PdfPTable tblAddress = new PdfPTable(2);
        int[] column_tblAddress = { 30, 70 };
        tblAddress.SetWidths(column_tblAddress);
        tblAddress.SpacingBefore = 0f;
        tblAddress.LockedWidth = true;

        tblAddress.AddCell(setCell(tblLogo, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 3));
        tblAddress.AddCell(setCell(tblName, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 3));
        //tblAddress.AddCell(setCell(tblAddress, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tblAddress.TotalWidth = mydoc.PageSize.Width;
        return tblAddress;

    }
    public PdfPTable GenerateCompanyHeader_ImageAddressHemsanInd(int[] noOfColsStruc, string pImageFile)
    {
        //PdfPTable tableHeader = new PdfPTable(2);
        //tableHeader.SetWidths(noOfColsStruc);
        //tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        //tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        //tableHeader.LockedWidth = true;
        //tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        int totrec1 = 0;
        List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);

     
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));

        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CompanyLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["CompanyLogoHeight"].ToString();

        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["CompanyLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["CompanyLogoWidth"].ToString();

        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)100;
        float imgHeg = (!String.IsNullOrEmpty(h1)) ? float.Parse(h1) : (float)72;

        PdfPTable tblLogo = new PdfPTable(2);
        int[] column_tblLogo = { 50, 50 };
        tblLogo.SetWidths(column_tblLogo);
        int fileCount1 = 0;
        string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";

        tblLogo.AddCell(setCell("GST NO. " + lstOrg[0].GSTIN, WhiteBaseColor, fnCalibriBold8, paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblLogo.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 2, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        if (File.Exists(tmpFile1))
        {
            if (File.Exists(tmpFile1))
            {
                PdfPTable tblSymbol = new PdfPTable(1);
                iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                eLoc.ScaleAbsolute(imgWid, imgHeg);
                tblSymbol.AddCell(setCellFixImage(eLoc, WhiteBaseColor, fnCalibri9, 0, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(tblSymbol, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                fileCount1 = fileCount1 + 1;
            }
        }

     

        //PdfPTable tblExtra = new PdfPTable(2);
        //int[] column_tblExtra = { 15, 75 };
        //tblExtra.SetWidths(column_tblExtra);
        //tblExtra.AddCell(setCell("Phone", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell("Email", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].EmailAddress) ? lstOrg[0].EmailAddress : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell("Website", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + "www.parthequipments.in , www.parthequipments.com", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblName = new PdfPTable(1);
        int[] column_tblName = { 100 };
        tblName.SetWidths(column_tblName);

        string address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : "") + (!String.IsNullOrEmpty(lstOrg[0].Address) ? " , " : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? lstOrg[0].CityName : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? " , " : "") + (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " : "") + (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? lstOrg[0].Pincode : "");

        tblName.AddCell(setCell("M: " + lstOrg[0].Landline1, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell("Email ID: " + lstOrg[0].EmailAddress, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(lstOrg[0].OrgName, WhiteBaseColor, fnCalibriBold18DarkBlue, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(address, WhiteBaseColor, fnCalibriBoldRed8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblAddress = new PdfPTable(2);
        int[] column_tblAddress = { 50, 50 };
        tblAddress.SetWidths(column_tblAddress);
        tblAddress.SpacingBefore = 0f;
        tblAddress.LockedWidth = true;

        tblAddress.AddCell(setCell(tblName, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
        tblAddress.AddCell(setCell(tblLogo, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 11));
        //tblAddress.AddCell(setCell(tblAddress, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tblAddress.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tblAddress;

    }
    public PdfPTable GenerateCompanyHeader_ImageAddressAranka(int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPTable tblLogo = new PdfPTable(1);
        int[] column_tblLogo = { 100 };
        tblLogo.SetWidths(column_tblLogo);
        int fileCount1 = 0;
        string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogoBanner.png";


        if (File.Exists(tmpFile1))
        {
            if (File.Exists(tmpFile1))
            {
                PdfPTable tblSymbol = new PdfPTable(1);
                iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                eLoc.ScaleAbsolute(480, 72);
                tblSymbol.AddCell(setCellFixImage(eLoc, WhiteBaseColor, fnCalibri9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(tblSymbol, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                fileCount1 = fileCount1 + 1;
            }
        }

        int totrec1 = 0;
        List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);

        //PdfPTable tblExtra = new PdfPTable(2);
        //int[] column_tblExtra = { 15, 75 };
        //tblExtra.SetWidths(column_tblExtra);
        //tblExtra.AddCell(setCell("Phone", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].Landline1) ? lstOrg[0].Landline1 : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell("Email", WhiteBaseColor, fnCalibriBold10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + (!String.IsNullOrEmpty(lstOrg[0].EmailAddress) ? lstOrg[0].EmailAddress : ""), WhiteBaseColor, fnCalibri10, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell("Website", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tblExtra.AddCell(setCell(": " + "www.parthequipments.in , www.parthequipments.com", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblName = new PdfPTable(1);
        int[] column_tblName = { 100 };
        tblName.SetWidths(column_tblName);

        string address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : "") + (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : "") + (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " + lstOrg[0].Pincode : "");

        tblName.AddCell(setCell("Fact. Address : " + address, WhiteBaseColor, fnTimes10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell("Cont :" + lstOrg[0].Landline1 + "," + lstOrg[0].Landline2 + "Email :" + lstOrg[0].EmailAddress + " " + "www.arankainstruments", WhiteBaseColor, fnTimes10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(" ", WhiteBaseColor, fnCalibri5, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblAddress = new PdfPTable(2);
        int[] column_tblAddress = { 35, 65 };
        tblAddress.SetWidths(column_tblAddress);
        tblAddress.SpacingAfter = 0f;
        tblAddress.SpacingBefore = 0f;
        tblAddress.LockedWidth = true;

        tblAddress.AddCell(setCell(tblLogo, WhiteBaseColor, fnCalibriBold12, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblAddress.AddCell(setCell(tblName, WhiteBaseColor, fnCalibriBold12, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 2));
        //tblAddress.AddCell(setCell(tblAddress, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tblAddress.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tblAddress;
    }
    public PdfPTable GenerateCompanyHeader_ImageAddressAccuPanel(int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPTable tblLogo = new PdfPTable(1);
        int[] column_tblLogo = { 100 };
        tblLogo.SetWidths(column_tblLogo);
        int fileCount1 = 0;
        string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";


        if (File.Exists(tmpFile1))
        {
            if (File.Exists(tmpFile1))
            {
                PdfPTable tblSymbol = new PdfPTable(1);
                iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                eLoc.ScaleAbsolute(180, 86);
                tblSymbol.AddCell(setCellFixImage(eLoc, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(tblSymbol, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                fileCount1 = fileCount1 + 1;
            }
        }

        int totrec1 = 0;
        List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);
        
        string address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : "") +
                         (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : "") +
                         (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " + lstOrg[0].Pincode : "");

        PdfPTable tbleextra1 = new PdfPTable(1);
        int[] column_tbleextra1 = { 100 };
        tbleextra1.SetWidths(column_tbleextra1);
        tbleextra1.AddCell(setCell("CIN NO." + lstOrg[0].CINNO, WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tbleextra1.AddCell(setCell(address, WhiteBaseColor, fnCalibriBold9, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        PdfPTable tbleextra2 = new PdfPTable(2);
        int[] column_tbleextra2 = { 50, 50 };
        tbleextra2.SetWidths(column_tbleextra2);
        tbleextra2.AddCell(setCell("Mo.No :" + lstOrg[0].Landline1 + "/" + lstOrg[0].Landline2, WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tbleextra2.AddCell(setCell("Web : www.accupanels.com ", WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        if (HttpContext.Current.Session["printModule"].ToString() == "salesorder" ||  HttpContext.Current.Session["printModule"].ToString() == "SalesPerfoma" || HttpContext.Current.Session["printModule"].ToString() == "challan")
            tbleextra2.AddCell(setCell("Mail Id : " + "accounts@accupanels.com", WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        else
            tbleextra2.AddCell(setCell("Mail Id : " + (!String.IsNullOrEmpty(lstOrg[0].EmailAddress) ? lstOrg[0].EmailAddress : ""), WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        PdfPTable tblName = new PdfPTable(2);
        int[] column_tblName = { 50, 50 };
        tblName.SetWidths(column_tblName);


        tblName.AddCell(setCell(lstOrg[0].OrgName, WhiteBaseColor, fnCalibriBold18Red, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell("All type of Electrical Power & Control Solar ACDB DCDB Panel Manufacturer.", WhiteBaseColor, fnCalibriBold10, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(tbleextra1, WhiteBaseColor, fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(tbleextra2, WhiteBaseColor, fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblAddress = new PdfPTable(2);
        int[] column_tblAddress = { 65, 35 };
        tblAddress.SetWidths(column_tblAddress);
        tblAddress.SpacingAfter = 0f;
        tblAddress.SpacingBefore = 0f;
        tblAddress.LockedWidth = true;

        tblAddress.AddCell(setCell(tblName, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
        tblAddress.AddCell(setCell(tblLogo, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 11));
        //tblAddress.AddCell(setCell(tblAddress, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tblAddress.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tblAddress;
    }


    public PdfPTable GenerateCompanyHeader_ImageAddressAccuPanel2(int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPTable tblLogo = new PdfPTable(1);
        int[] column_tblLogo = { 100 };
        tblLogo.SetWidths(column_tblLogo);
        int fileCount1 = 0;
        string tmpFile1 = System.Web.Hosting.HostingEnvironment.MapPath("~/images/CompanyLogo/") + "\\CompanyLogo.png";


        if (File.Exists(tmpFile1))
        {
            if (File.Exists(tmpFile1))
            {
                PdfPTable tblSymbol = new PdfPTable(1);
                iTextSharp.text.Image eLoc = iTextSharp.text.Image.GetInstance(tmpFile1);
                eLoc.ScaleAbsolute(180, 86);
                tblSymbol.AddCell(setCellFixImage(eLoc, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblLogo.AddCell(setCell(tblSymbol, WhiteBaseColor, objContentFontDataBlack, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                fileCount1 = fileCount1 + 1;
            }
        }

        int totrec1 = 0;
        List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec1);

        string address = (!String.IsNullOrEmpty(lstOrg[0].Address) ? lstOrg[0].Address : "") +
                         (!String.IsNullOrEmpty(lstOrg[0].CityName) ? ", " + lstOrg[0].CityName : "") +
                         (!String.IsNullOrEmpty(lstOrg[0].Pincode) ? " - " + lstOrg[0].Pincode : "");

        PdfPTable tbleextra1 = new PdfPTable(1);
        int[] column_tbleextra1 = { 100 };
        tbleextra1.SetWidths(column_tbleextra1);
        tbleextra1.AddCell(setCell("CIN NO." + lstOrg[0].CINNO, WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tbleextra1.AddCell(setCell(address, WhiteBaseColor, fnCalibriBold9, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        PdfPTable tbleextra2 = new PdfPTable(2);
        int[] column_tbleextra2 = { 50, 50 };
        tbleextra2.SetWidths(column_tbleextra2);
        tbleextra2.AddCell(setCell("Mo.No :" + lstOrg[0].Landline1 + "/" + lstOrg[0].Landline2, WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tbleextra2.AddCell(setCell("Web : www.accupanels.com ", WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tbleextra2.AddCell(setCell("Mail Id : " + "accounts@accupanels.com", WhiteBaseColor, fnCalibriBold9, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        PdfPTable tblName = new PdfPTable(2);
        int[] column_tblName = { 50, 50 };
        tblName.SetWidths(column_tblName);


        tblName.AddCell(setCell(lstOrg[0].OrgName, WhiteBaseColor, fnCalibriBold18Red, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell("All type of Electrical Power & Control Solar ACDB DCDB Panel Manufacturer.", WhiteBaseColor, fnCalibriBold10, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(tbleextra1, WhiteBaseColor, fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblName.AddCell(setCell(tbleextra2, WhiteBaseColor, fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


        PdfPTable tblAddress = new PdfPTable(2);
        int[] column_tblAddress = { 65, 35 };
        tblAddress.SetWidths(column_tblAddress);
        tblAddress.SpacingAfter = 0f;
        tblAddress.SpacingBefore = 0f;
        tblAddress.LockedWidth = true;

        tblAddress.AddCell(setCell(tblName, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 7));
        tblAddress.AddCell(setCell(tblLogo, WhiteBaseColor, fnCalibriBold12, 0, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 11));
        //tblAddress.AddCell(setCell(tblAddress, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        tblAddress.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tblAddress;
    }

    public PdfPTable GenerateCompanyHeader_ImageAddress(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell cell, cell1, cell2;
        // ---------------------------------------------------------------------------
        // Header Left Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));
        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();
        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;
        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(1);
        int[] column_tblNested1 = { 100 };
        tblNested1.SetWidths(column_tblNested1);
        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        if(tmpSerialKey == "TWS3-RT90-E22O-K88P")
            lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList(BAL.CommonMgmt.GetOrgCodeByUserID(HttpContext.Current.Session["LoginUserID"].ToString()), 1, 100, out TotalCount);
        else
            lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        String org = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address;
        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + ", India";
        String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us -: " + lstEntity[0].Landline1 : "");
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email Us : " + lstEntity[0].EmailAddress : "");
        tblNested1.AddCell(setCell(org, WhiteBaseColor, objHeaderFont16, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x3))
            tblNested1.AddCell(setCell(x3, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x4))
            tblNested1.AddCell(setCell(x4, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        cell1 = new PdfPCell(tblNested1);
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell1.Border = Rectangle.BOTTOM_BORDER;
        cell1.BorderColor = BaseColor.BLACK;
        // -----------------------------------------------------
        if(tmpSerialKey == "TWS3-RT90-E22O-K88P")
        { 
            tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            tableHeader.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        }
        else
        { 
            tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOTTOM_BORDER));
            tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        }
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }

    public PdfPTable GenerateCompanyHeader_ImageAddressEurolight(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell cell, cell1, cell2;
        // ---------------------------------------------------------------------------
        // Header Left Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));
        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();
        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;
        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(1);
        int[] column_tblNested1 = { 100 };
        tblNested1.SetWidths(column_tblNested1);
        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        String org = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address;
        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + ", India";
        String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact Us -: " + lstEntity[0].Landline1 : "");
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email Us : " + lstEntity[0].EmailAddress : "");
        tblNested1.AddCell(setCell(org, WhiteBaseColor, objHeaderFont12, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x1, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x2, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x3))
            tblNested1.AddCell(setCell(x3, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x4))
            tblNested1.AddCell(setCell(x4, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        cell1 = new PdfPCell(tblNested1);
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell1.Border = Rectangle.BOTTOM_BORDER;
        cell1.BorderColor = BaseColor.BLACK;
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOTTOM_BORDER));
        tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }
    public PdfPTable GenerateCompanyHeader_ImageAddress_zemote(string tmpSerialKey, int[] noOfColsStruc, string pImageFile,string PrintModule )
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell cell, cell1, cell2;
        // ---------------------------------------------------------------------------
        // Header Left Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));
        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();
        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;
        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(1);
        int[] column_tblNested1 = { 100 };
        tblNested1.SetWidths(column_tblNested1);
        // ---------------------------------------------------------------------------
        int TotalCount = 0;
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        String org = lstEntity[0].OrgName;
        String x1 = lstEntity[0].Address;
        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : "") +
                    ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : "") + ", India";
        String x3 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? "Contact : " + lstEntity[0].Landline1 : "");
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? "Email : " + lstEntity[0].EmailAddress : "") + ((!String.IsNullOrEmpty(lstEntity[0].Fax1)) ? "WebSite : " + lstEntity[0].Fax1 : "");
        tblNested1.AddCell(setCell(org, WhiteBaseColor, objHeaderFont16, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x3))
            tblNested1.AddCell(setCell(x3, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x4))
            tblNested1.AddCell(setCell(x4, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        cell1 = new PdfPCell(tblNested1);
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell1.Border = Rectangle.BOTTOM_BORDER;
        cell1.BorderColor = BaseColor.BLACK;
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOTTOM_BORDER));

        if(PrintModule == "salesorder" || PrintModule == "voucher" || PrintModule ==  "proforma")
            tableHeader.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        else
            tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }


    public PdfPTable GenerateCompanyHeader_ImageMultiAddress(string tmpSerialKey, int[] noOfColsStruc, string pImageFile)
    {
        PdfPTable tableHeader = new PdfPTable(2);
        tableHeader.SetWidths(noOfColsStruc);
        tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell cell, cell1, cell2;
        // ---------------------------------------------------------------------------
        // Header Left Side - Company Logo (Image)
        // ---------------------------------------------------------------------------
        cell = new PdfPCell();
        iTextSharp.text.Image myLogo = iTextSharp.text.Image.GetInstance(findProductImage(pImageFile));
        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoHeight"].ToString();
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["PrintHeaderLogoWidth"].ToString();
        float imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : (float)85;
        float imgHeg = (!String.IsNullOrEmpty(w1)) ? float.Parse(h1) : (float)65;
        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell = new PdfPCell(myLogo);
        // ---------------------------------------------------------------------------
        // Header Right Side - Company Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(5);
        int[] column_tblNested1 = { 14, 1, 38, 17, 30 };
        tblNested1.SetWidths(column_tblNested1);
        int TotalCount = 0;

        // ------------------------Office Address-------------------------------------
        List<Entity.OrganizationStructure> lstEntity = new List<Entity.OrganizationStructure>();
        lstEntity = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 100, out TotalCount);
        //-------------------------Factory Address------------------------------------
        List<Entity.OrganizationStructure> lstEntity1 = new List<Entity.OrganizationStructure>();
        lstEntity1 = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("002", 1, 100, out TotalCount);
        //----------------------------------------------------------------------------
        Entity.Authenticate objAuth = new Entity.Authenticate();
        objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
        //----------------------------------------------------------------------------
        String x1 = lstEntity[0].Address + " ";
        String x2 = ((!String.IsNullOrEmpty(lstEntity[0].CityName)) ? lstEntity[0].CityName + " - " : " ") +
                    ((!String.IsNullOrEmpty(lstEntity[0].Pincode)) ? lstEntity[0].Pincode + ", " : " ");
        String x3 = ((!String.IsNullOrEmpty(lstEntity[0].StateName)) ? lstEntity[0].StateName + ", " : " ") + "India";
        String x4 = ((!String.IsNullOrEmpty(lstEntity[0].Landline1)) ? lstEntity[0].Landline1 : " ");
        String x5 = ((!String.IsNullOrEmpty(lstEntity[0].EmailAddress)) ? lstEntity[0].EmailAddress : " ");

        String a = lstEntity1[0].Address;
        String b = ((!String.IsNullOrEmpty(lstEntity1[0].CityName)) ? lstEntity1[0].CityName + " - " : " ") +
                    ((!String.IsNullOrEmpty(lstEntity1[0].Pincode)) ? lstEntity1[0].Pincode + ", " : " ");
        String c = ((!String.IsNullOrEmpty(lstEntity1[0].StateName)) ? lstEntity1[0].StateName + ", " : " ") + "India";


        tblNested1.AddCell(setCell("Office", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(": ", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(x1 + x2 + x3, WhiteBaseColor, fnCalibri9, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        tblNested1.AddCell(setCell("Factory", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(": ", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(a + b + c, WhiteBaseColor, fnCalibri9, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        tblNested1.AddCell(setCell("Contact", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(": ", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x4))
            tblNested1.AddCell(setCell(x4, WhiteBaseColor, fnCalibri9, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        tblNested1.AddCell(setCell("Email id", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(": ", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (!String.IsNullOrEmpty(x5))
            tblNested1.AddCell(setCell(x5, WhiteBaseColor, fnCalibri9, 1, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        tblNested1.AddCell(setCell("Website", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(": ", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        if (tmpSerialKey == "SA98-6HY9-HU67-LORF")
            tblNested1.AddCell(setCell("www.shaktipet.com", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        else
            tblNested1.AddCell(setCell("www.estirehealthcare.com", WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell("GSTIN No.", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        tblNested1.AddCell(setCell(": " + lstEntity[0].GSTIN, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        cell1 = new PdfPCell(tblNested1);
        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
        cell1.Border = Rectangle.BOTTOM_BORDER;
        cell1.BorderColor = BaseColor.BLACK;
        // ---------------------------------------------------------------------------
        // Header - Factory Address
        // ---------------------------------------------------------------------------
        PdfPTable tblNested2 = new PdfPTable(1);
        int[] column_tblNested2 = { 100 };
        tblNested2.SetWidths(column_tblNested2);
        // ---------------------------------------------------------------------------

        //List<Entity.OrganizationStructure> lstEntity1 = new List<Entity.OrganizationStructure>();
        //lstEntity1 = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("002", 1, 100, out TotalCount);
        //String a = lstEntity1[0].Address;
        //String b = ((!String.IsNullOrEmpty(lstEntity1[0].CityName)) ? lstEntity1[0].CityName + " - " : "") +
        //            ((!String.IsNullOrEmpty(lstEntity1[0].Pincode)) ? lstEntity1[0].Pincode + ", " : "");
        //String c = ((!String.IsNullOrEmpty(lstEntity1[0].StateName)) ? lstEntity1[0].StateName + ", " : "") + "India";
        //String d = ((!String.IsNullOrEmpty(lstEntity1[0].Landline1)) ? "Contact : " + lstEntity1[0].Landline1 : "");
        //String e = ((!String.IsNullOrEmpty(lstEntity1[0].EmailAddress)) ? "Email : " + lstEntity1[0].EmailAddress : "");
        //tblNested2.AddCell(setCell("Factory Address", WhiteBaseColor, fnCalibriBold10, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //tblNested2.AddCell(setCell(a, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //tblNested2.AddCell(setCell(b, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //if (!String.IsNullOrEmpty(c))
        //    tblNested2.AddCell(setCell(c, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //if (!String.IsNullOrEmpty(d))
        //    tblNested2.AddCell(setCell(d, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //if (!String.IsNullOrEmpty(e))
        //    tblNested2.AddCell(setCell(e, WhiteBaseColor, fnCalibri9, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //cell2 = new PdfPCell(tblNested2);
        //cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
        //cell2.Border = Rectangle.BOTTOM_BORDER;
        //cell2.BorderColor = BaseColor.BLACK;
        // -----------------------------------------------------
        tableHeader.AddCell(setCell(cell1, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tableHeader.AddCell(setCell(cell, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, 1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.BOTTOM_BORDER));
        // -----------------------------------------------------------------------
        tableHeader.TotalWidth = (mydoc.PageSize.Width - (mydoc.LeftMargin + mydoc.RightMargin));
        return tableHeader;
    }


    public PdfPTable GenerateCompanyHeader_Banner(string tmpSerialKey, string pImageFile, int imgHAlign, int imgVAlign, int imgBorder, iTextSharp.text.Font fn, Int32 CurrentPageNo, string pTitle = "", float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
    {
        PdfPTable tableHeader = new PdfPTable(1);
        //tableHeader.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
        tableHeader.DefaultCell.CellEvent = new RoundedBorder();
        tableHeader.LockedWidth = true;

        PdfPCell cell, cell1, cell2;
        //// ---------------------------------------------------------------------------
        //// Header Banner
        //// ---------------------------------------------------------------------------
        PdfPTable tblNested1 = new PdfPTable(1);
        int[] column_tblNested1 = { 100 };
        tblNested1.SetWidths(column_tblNested1);
        // ---------------------------------------------------------------------------
        cell2 = new PdfPCell();
        iTextSharp.text.Image myLogo = findProductImage(pImageFile);
        string h1 = "", w1 = "";
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["BannerLogoHeight"]))
            h1 = System.Configuration.ConfigurationManager.AppSettings["BannerLogoHeight"].ToString();
        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["BannerLogoWidth"]))
            w1 = System.Configuration.ConfigurationManager.AppSettings["BannerLogoWidth"].ToString();

        float imgWid = 0, imgHeg = 0;
        imgWid = (!String.IsNullOrEmpty(w1)) ? float.Parse(w1) : 210f;
        imgHeg = (!String.IsNullOrEmpty(h1)) ? float.Parse(h1) : 90f;
        myLogo.ScaleAbsolute(imgWid, imgHeg);
        cell2 = new PdfPCell(myLogo);

        // -----------------------------------------------------
        if (!String.IsNullOrEmpty(pTitle))
        {
            tableHeader.AddCell(setCell(pTitle, WhiteBaseColor, objContentFontDataBlack, pad, 1, hAlign, vAlign, borderVal));
        }
        tableHeader.AddCell(setCell(cell2, WhiteBaseColor, objContentFontDataBlack, pad, 1, imgHAlign, imgVAlign, 0));
        //Int64 mypage = Convert.ToInt64(HttpContext.Current.Session["CurrentPageNo"]);

        if ((tmpSerialKey == "J63H-F8LX-B4B2-GYVZ") && CurrentPageNo != 1) // For Hi-Tech & BharatP || tmpSerialKey == "BHAR-ATPA-TTER-NENG"
            tableHeader.AddCell(setCell(" ", WhiteBaseColor, objContentFontDataBlack, pad, 1, imgHAlign, imgVAlign, 2));

        //if ((tmpSerialKey == "BHAR-ATPA-TTER-NENG") && mypage != 1) // For Hi-Tech & BharatP || tmpSerialKey == "BHAR-ATPA-TTER-NENG"
        //{
        //    Table tblHead = new Table();
        //    int[] column_tblHead = { 100 };
        //    tblHead.BorderColor = System.Drawing.Color.Red;
        //    tblHead.BorderStyle = BorderStyle.Solid;
        //    tblHead.BorderWidth = 2;

        //}
        //mypage++;
        //HttpContext.Current.Session["CurrentPageNo"] = mypage;
        // -----------------------------------------------------------------------
        if (tmpSerialKey == "BHAR-ATPA-TTER-NENG" || tmpSerialKey == "KH4O-D0IY-ARPL-A2ST" || tmpSerialKey == "ECO3-2G21-TECH-3MRT")
            tableHeader.TotalWidth = (mydoc.PageSize.Width);
        else
            tableHeader.TotalWidth = (mydoc.PageSize.Width - 40);
        tableHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        return tableHeader;
    }

    public PdfPTable GenerateCompanyFooter(Document document, string tmpSerialKey)
    {
        PdfPTable tableFoot;
        //tmpSerialKey = "H0PX-EMRW-23IJ-C1TD";
        // --------------------------------------------------------------
        if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")
        {
            tableFoot = new PdfPTable(1);
            int[] column_tblNested2001 = { 100 };
            tableFoot.SetWidths(column_tblNested2001);
        }
        else
        {
            tableFoot = new PdfPTable(1);
            int[] column_tblNested2001 = { 100 };
            tableFoot.SetWidths(column_tblNested2001);
        }
        // --------------------------------------------------------------
        tableFoot.DefaultCell.Border = Rectangle.NO_BORDER;
        tableFoot.DefaultCell.CellEvent = new RoundedBorder();
        tableFoot.LockedWidth = true;
        tableFoot.HorizontalAlignment = Element.ALIGN_CENTER;
        // --------------------------------------------------------------
        if (tmpSerialKey == "H0PX-EMRW-23IJ-C1TD")
        {
            string flagPrintUnit = (string)HttpContext.Current.Session["PrintUnitAddress"];
            if (flagPrintUnit == "Unit I & II")
            {
                tableFoot.AddCell(setCell("Post Add : Plot No. C1-613, Phase-III, Near C.U.Shah Medical Hall G.I.D.C, Wadhwan, Surendranagar - 363030 (GUJ)", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.TOP_BORDER));
                tableFoot.AddCell(setCell("Ph. : 02752 - 240552. Mo.: +91 99789 52152    Website : steelmangas.com   Email : steelmangas@gmail.com /  steelmangas@rediffmail.com", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            }
            else
            {
                tableFoot.AddCell(setCell("Marketing Office : office no. 21, second floor, vedant Trade centre, Valia Chowkdi, Ankleshwar - 393001 (GUJ)", WhiteBaseColor, fnCalibri10, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.TOP_BORDER));
                tableFoot.AddCell(setCell("Mobile : 9879617465, 9524495240    Website : steelmangas.com   Email : steelmangas@gmail.com /  steelmangas@rediffmail.com", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            }
        }
        // -----------------------------------------------------------------------
        tableFoot.TotalWidth = (document.PageSize.Width - (document.LeftMargin + document.RightMargin));
        return tableFoot;
    }


    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
    public void GetPrinterMargins(string moduleName, string flagPrintHeader, out Int64 TopMargin, out Int64 BottomMargin, out Int64 ProdDetail_Lines)
    {
        TopMargin = 0;
        BottomMargin = 0;
        ProdDetail_Lines = 0;
        List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
        lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), moduleName);

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
    }
    public PdfPTable Quotation_CustomerInfo(List<Entity.Customer> lstCustomer)
    {
        PdfPTable tblCustInfo = new PdfPTable(1);
        int[] column_tblCustInfo = { 100 };
        tblCustInfo.SetWidths(column_tblCustInfo);
        tblCustInfo.AddCell(setCell(lstCustomer[0].CustomerName, WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        if (!String.IsNullOrEmpty(lstCustomer[0].Address.TrimStart('.') + lstCustomer[0].Area.TrimStart('.')))
            tblCustInfo.AddCell(setCell(lstCustomer[0].Address + "," + lstCustomer[0].Area + ",", WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        if (!String.IsNullOrEmpty(lstCustomer[0].CityName.TrimStart('.') + lstCustomer[0].StateName.TrimStart('.') + lstCustomer[0].Pincode.TrimStart('.')))
            tblCustInfo.AddCell(setCell(lstCustomer[0].CityName.TrimStart('.') + " - " + lstCustomer[0].Pincode + ", " + lstCustomer[0].StateName.Trim(), WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        else
            tblCustInfo.AddCell(setCell(" ", WhiteBaseColor, fnCalibri9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        if (!String.IsNullOrEmpty(lstCustomer[0].ContactNo1))
            tblCustInfo.AddCell(setCell("Contact No: " + lstCustomer[0].ContactNo1, WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        if (!String.IsNullOrEmpty(lstCustomer[0].EmailAddress))
            tblCustInfo.AddCell(setCell("Email     : " + lstCustomer[0].EmailAddress, WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        tblCustInfo.AddCell(setCell("GST No    : " + ((lstCustomer.Count > 0) ? lstCustomer[0].GSTNo : ""), WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        // ------------------------------------------
        return tblCustInfo;
    }

    public PdfPTable RestOfTable(Int64 ProdDetail_Lines, Int64 CompletedLints, int[] noOfColsStruc, Int16 pBorderVal)
    {
        int noOfCols = noOfColsStruc.Length;
        PdfPTable tblRestOfLines = new PdfPTable(noOfCols);
        tblRestOfLines.SetWidths(noOfColsStruc);
        if (ProdDetail_Lines > CompletedLints)
        {
            for (int i = 1; i <= (ProdDetail_Lines - CompletedLints); i++)
            {
                for (int j = 1; j <= noOfCols; j++)
                {
                    if (i < (ProdDetail_Lines - CompletedLints))
                        tblRestOfLines.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, pBorderVal));
                    else
                        tblRestOfLines.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, pBorderVal + 2));
                }
            }
        }
        return tblRestOfLines;
    }

    public PdfPTable RestOfTableByColSpan(Int64 ProdDetail_Lines, Int64 CompletedLints, int[] noOfColsStruc, int[] noOfColsSpan, Int16 pBorderVal)
    {
        int noOfCols = noOfColsStruc.Length;
        PdfPTable tblRestOfLines = new PdfPTable(noOfCols);
        tblRestOfLines.SetWidths(noOfColsStruc);
        if (ProdDetail_Lines > CompletedLints)
        {
            for (int i = 1; i <= (ProdDetail_Lines - CompletedLints); i++)
            {
                for (int j = 0; j < noOfColsSpan.Length; j++)
                {
                    int colspan = noOfColsSpan[j];
                    if (i < (ProdDetail_Lines - CompletedLints))
                        tblRestOfLines.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, colspan, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, pBorderVal));
                    else
                        tblRestOfLines.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, colspan, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, pBorderVal + 2));
                }
            }
        }
        return tblRestOfLines;
    }

    public PdfPTable TermsCondition(string pTNC, Int16 pBorderVal, Boolean pTitleFlag)
    {
        int[] colStructWidth = { 100 };
        PdfPTable tblTNC = new PdfPTable(1);
        tblTNC.SetWidths(colStructWidth);
        tblTNC.HorizontalAlignment = Element.ALIGN_LEFT;
        // ---------------------------------------------------
        if (pTitleFlag)
        {
            tblTNC.AddCell(setCell("Terms & Conditions : ", WhiteBaseColor, fnArialBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
        }
        tblTNC.AddCell(setCell(pTNC, WhiteBaseColor, fnArial8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
        // ---------------------------------------------------
        return tblTNC;
    }

    public PdfPTable TermsCondition(string pTNC, Int16 pBorderVal, Boolean pTitleFlag, iTextSharp.text.Font myFont)
    {
        int[] colStructWidth = { 100 };
        PdfPTable tblTNC = new PdfPTable(1);
        tblTNC.SetWidths(colStructWidth);
        tblTNC.HorizontalAlignment = Element.ALIGN_LEFT;
        // ---------------------------------------------------
        if (pTitleFlag)
        {
            tblTNC.AddCell(setCell("Terms & Conditions : ", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
        }
        tblTNC.AddCell(setCell(pTNC, WhiteBaseColor, myFont, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
        // ---------------------------------------------------
        return tblTNC;
    }

    public PdfPTable TermsConditionWithHeadTitle(string pTNC, Int16 pBorderVal, Boolean pTitleFlag, iTextSharp.text.Font myHeadFont, iTextSharp.text.Font myTextFont, Int64 pTemplate = 0)
    {

        if (pTemplate == 1)
        {
            int[] colStructWidth = { 100 };
            PdfPTable tblTNC = new PdfPTable(1);
            tblTNC.SetWidths(colStructWidth);
            tblTNC.HorizontalAlignment = Element.ALIGN_LEFT;
            // ---------------------------------------------------
            if (pTitleFlag)
            {
                tblTNC.AddCell(setCell("Terms & Conditions : ", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
            }

            pTNC.Split('\n').ToList().ForEach(line =>
            {
                line = line.Trim();
                if (!String.IsNullOrEmpty(line))
                {
                    if (line.ToString().Substring(line.ToString().Length - 1) == ":")
                        tblTNC.AddCell(setCell("* " + line, WhiteBaseColor, myHeadFont, paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                    else
                        tblTNC.AddCell(setCell("* " + line, WhiteBaseColor, myTextFont, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                }
            });
            // ---------------------------------------------------
            return tblTNC;
        }
        else
        {
            int[] colStructWidth = { 100 };
            PdfPTable tblTNC = new PdfPTable(1);
            tblTNC.SetWidths(colStructWidth);
            tblTNC.HorizontalAlignment = Element.ALIGN_LEFT;
            // ---------------------------------------------------
            if (pTitleFlag)
            {
                tblTNC.AddCell(setCell("Terms & Conditions : ", WhiteBaseColor, fnCalibriBold10, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
            }

            pTNC.Split('\n').ToList().ForEach(line =>
            {
                line = line.Trim();
                if (!String.IsNullOrEmpty(line))
                {
                    if (line.ToString().Substring(line.ToString().Length - 1) == ":")
                        tblTNC.AddCell(setCell(line, WhiteBaseColor, myHeadFont, paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                    else
                        tblTNC.AddCell(setCell(line, WhiteBaseColor, myTextFont, paddingOf1, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                }
            });
            // ---------------------------------------------------
            return tblTNC;
        }
        // return null;
    }
    public PdfPTable BankDetails(List<Entity.OrganizationBank> lstObject, Int16 pBorderVal, Int64 pTemplate)
    {
        if (pTemplate == 1)
        {
            PdfPTable tblBankDetail = new PdfPTable(2);
            int[] column_tblBankDetail = { 10, 90 };
            tblBankDetail.SetWidths(column_tblBankDetail);
            tblBankDetail.HorizontalAlignment = Element.ALIGN_LEFT;

            tblBankDetail.AddCell(setCell("Bank Details:", WhiteBaseColor, fnCalibriItalicBBoldBlue10, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("Bank Name ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(": " + ((lstObject.Count > 0) ? lstObject[0].BankName : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("Branch ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(": " + ((lstObject.Count > 0) ? lstObject[0].BranchName : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("A/c No ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(": " + ((lstObject.Count > 0) ? lstObject[0].BankAccountNo : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("IFSC Code ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(": " + ((lstObject.Count > 0) ? lstObject[0].BankIFSC : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            return tblBankDetail;
        }
        else if (pTemplate == 2)
        {
            PdfPTable tblBankDetail = new PdfPTable(4);
            int[] column_tblBankDetail = { 25, 25, 25, 25 };
            tblBankDetail.SetWidths(column_tblBankDetail);
            tblBankDetail.HorizontalAlignment = Element.ALIGN_LEFT;

            tblBankDetail.AddCell(setCell("Bank Details:", WhiteBaseColor, fnCalibriBold10, paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblBankDetail.AddCell(setCell("Bank Name", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("Branch   ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("A/c No   ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("IFSC Code", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblBankDetail.AddCell(setCell(((lstObject.Count > 0) ? lstObject[0].BankName : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(((lstObject.Count > 0) ? lstObject[0].BranchName : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(((lstObject.Count > 0) ? lstObject[0].BankAccountNo : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(((lstObject.Count > 0) ? lstObject[0].BankIFSC : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            return tblBankDetail;
        }
        else if (pTemplate == 3)
        {
            PdfPTable tblBankDetail = new PdfPTable(2);
            int[] column_tblBankDetail = { 30, 70 };
            tblBankDetail.SetWidths(column_tblBankDetail);
            tblBankDetail.HorizontalAlignment = Element.ALIGN_LEFT;

            string line1 = "", line2 = "";
            line1 = "Bank Name : " + ((lstObject.Count > 0) ? lstObject[0].BankName : "") + "   " + "Branch    : " + ((lstObject.Count > 0) ? lstObject[0].BranchName : "");
            line2 = "A/c No    : " + ((lstObject.Count > 0) ? lstObject[0].BankAccountNo : "") + "   " + "IFSC Code : " + ((lstObject.Count > 0) ? lstObject[0].BankIFSC : "");

            tblBankDetail.AddCell(setCell("Bank Details:", WhiteBaseColor, fnCalibriBold10, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(line1, WhiteBaseColor, fnCalibri8, paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell(line2, WhiteBaseColor, fnCalibri8, paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            return tblBankDetail;
        }
        else
        {
            PdfPTable tblBankDetail = new PdfPTable(2);
            int[] column_tblBankDetail = { 30, 70 };
            tblBankDetail.SetWidths(column_tblBankDetail);
            tblBankDetail.HorizontalAlignment = Element.ALIGN_LEFT;

            tblBankDetail.AddCell(setCell("Bank Details:", WhiteBaseColor, fnCalibriBold10, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("Bank Name : " + ((lstObject.Count > 0) ? lstObject[0].BankName : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("Branch    : " + ((lstObject.Count > 0) ? lstObject[0].BranchName : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("A/c No    : " + ((lstObject.Count > 0) ? lstObject[0].BankAccountNo : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblBankDetail.AddCell(setCell("IFSC Code : " + ((lstObject.Count > 0) ? lstObject[0].BankIFSC : ""), WhiteBaseColor, fnCalibri8, paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            return tblBankDetail;
        }
    }

    public PdfPTable AuthorisedSignature(string pCompanyName, Int16 pTemplate, string pSignFile = null, string pContactNo = null, string pEmailAddress = null)
    {
        PdfPTable tblESignature = new PdfPTable(1);
        int[] column_tblESignature = { 100 };
        tblESignature.SetWidths(column_tblESignature);
        tblESignature.HorizontalAlignment = Element.ALIGN_LEFT;

        if (pTemplate == 1)
        {
            string tmpFile;
            if (String.IsNullOrEmpty(pSignFile))
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            else
                tmpFile = pSignFile;
            // -------------------------------------------------------
            if (File.Exists(tmpFile))
            {
                if (File.Exists(tmpFile))   //Signature print
                {
                    PdfPTable tblSign = new PdfPTable(1);
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                    eSign.ScaleAbsolute(90, 70);
                    tblSign.AddCell(setCellFixImage(eSign, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblESignature.AddCell(setCell(tblSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
            }
            else
            {
                tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }
        }
        else if (pTemplate == 2)
        {
            tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell("Authorized Signatory", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(((!String.IsNullOrEmpty(pContactNo)) ? pContactNo : " "), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(((!String.IsNullOrEmpty(pContactNo)) ? pEmailAddress : " "), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        }
        else if (pTemplate == 3)
        {
            tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell("Authorized Signatory", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        }
        else if (pTemplate == 4)
        {
            string tmpFile;
            if (String.IsNullOrEmpty(pSignFile))
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            else
                tmpFile = pSignFile;
            // -------------------------------------------------------
            if (File.Exists(tmpFile))
            {
                if (File.Exists(tmpFile))   //Signature print
                {
                    PdfPTable tblSign = new PdfPTable(1);
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                    eSign.ScaleAbsolute(90, 70);
                    tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCellFixImage(eSign, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("(Authorized Signatory)", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblESignature.AddCell(setCell(tblSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
            }
            else
            {
                tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }
        }

        else if (pTemplate == 5)
        {
            string tmpFile;
            if (String.IsNullOrEmpty(pSignFile))
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            else
                tmpFile = pSignFile;
            // -------------------------------------------------------
            if (File.Exists(tmpFile))
            {
                if (File.Exists(tmpFile))   //Signature print
                {
                    PdfPTable tblSign = new PdfPTable(1);
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                    eSign.ScaleAbsolute(90, 70);
                    tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCellFixImage(eSign, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblESignature.AddCell(setCell(tblSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
            }
            else
            {
                tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }
        }
        else if (pTemplate == 6)
        {
            string tmpFile;
            if (String.IsNullOrEmpty(pSignFile))
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            else
                tmpFile = pSignFile;
            // -------------------------------------------------------
            if (File.Exists(tmpFile))
            {
                if (File.Exists(tmpFile))   //Signature print
                {
                    PdfPTable tblSign = new PdfPTable(1);
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                    eSign.ScaleAbsolute(90, 70);
                    tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCellFixImage(eSign, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("Authorized Signature", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblESignature.AddCell(setCell(tblSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
            }
            else
            {
                tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" " , WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" " , WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" " , WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell("Authorized Signature" , WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }
        }
        else if (pTemplate == 7)
        {
            string tmpFile;
            if (String.IsNullOrEmpty(pSignFile))
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            else
                tmpFile = pSignFile;
            PdfPTable tblSign = new PdfPTable(1);
            // -------------------------------------------------------
            if (File.Exists(tmpFile))
            {
                if (File.Exists(tmpFile))   //Signature print
                {
                    
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                    eSign.ScaleAbsolute(90, 70);
                    tblSign.AddCell(setCell("Thanking You, " , WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("Your Faithfully, ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCellFixImage(eSign, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("(Authorized Signatory)", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
            }
            else
            {
                tblSign.AddCell(setCell("Thanking You, ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell("  ", WhiteBaseColor, fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell("Your Faithfully, ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell("  ", WhiteBaseColor, fnCalibri8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell(" ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell(" ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell(" ", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblSign.AddCell(setCell("(Authorized Signatory)", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            }
            tblESignature.AddCell(setCell(tblSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        }
        else
        {
            string tmpFile;
            if (String.IsNullOrEmpty(pSignFile))
                tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/images") + "\\eSignature.png";
            else
                tmpFile = pSignFile;

            if (File.Exists(tmpFile))
            {
                if (File.Exists(tmpFile))   //Signature print
                {
                    PdfPTable tblSign = new PdfPTable(1);
                    iTextSharp.text.Image eSign = iTextSharp.text.Image.GetInstance(tmpFile);
                    eSign.ScaleAbsolute(90, 70);
                    tblSign.AddCell(setCellFixImage(eSign, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblSign.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                    tblESignature.AddCell(setCell(tblSign, WhiteBaseColor, objContentFontDataBlack, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                }
            }
            else
            {
                tblESignature.AddCell(setCell("For, " + pCompanyName, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell("Authorized Signatory", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
                tblESignature.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            }
            tblESignature.AddCell(setCell(((!String.IsNullOrEmpty(pContactNo)) ? pContactNo : " "), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblESignature.AddCell(setCell(((!String.IsNullOrEmpty(pContactNo)) ? pEmailAddress : " "), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        }
        // ----------------------------
        return tblESignature;
    }

    public PdfPTable Table_ProductImageList(List<Entity.ProductImages> lstProduct)
    {

        PdfPTable tblProdInfo = new PdfPTable(2);
        int[] column_tblProdInfo = { 50,50};
        tblProdInfo.SetWidths(column_tblProdInfo);
        for (int i = 0; i < lstProduct.Count; i++)
        {
            string prodImage = System.Web.Hosting.HostingEnvironment.MapPath("~/productimages/") + lstProduct[i].Name.ToString();
            // ---------------------------------------------------------
            iTextSharp.text.Image myProdImage = findProductImage(prodImage);
            if (myProdImage != null)
            {
                if (lstProduct.Count > 1)
                {
                    myProdImage.ScaleAbsolute(200, 200);
                    tblProdInfo.AddCell(setCellFixImage(myProdImage, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                else
                {
                    myProdImage.ScaleAbsolute(400, 300);
                    tblProdInfo.AddCell(setCellFixImage(myProdImage, WhiteBaseColor, fnCalibriBold8, paddingOf2, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
            }
            else
                tblProdInfo.AddCell(setCell("Image Not Found", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        }
        if (lstProduct.Count % 2 == 1)
            tblProdInfo.AddCell(setCell("", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        // ------------------------------------------
        return tblProdInfo;
    }

    public PdfPTable Table_ProductImageList(DataTable dtProduct, Int64 pTempID)
    {
        if (pTempID == 1) // For GreenStone
        {
            {
                PdfPTable tblProdInfo = new PdfPTable(6);
                int[] column_tblProdInfo = { 5, 40, 10, 10, 10, 25 };
                tblProdInfo.SetWidths(column_tblProdInfo);
                // ----------------------------------------------------------
                tblProdInfo.AddCell(setCell("Sr.#", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                tblProdInfo.AddCell(setCell("Description", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                tblProdInfo.AddCell(setCell("Size", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                tblProdInfo.AddCell(setCell("Category", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                tblProdInfo.AddCell(setCell("Quantity", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 3));
                tblProdInfo.AddCell(setCell("Image", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
                // ----------------------------------------------------------
                for (int i = 0; i < dtProduct.Rows.Count; i++)
                {
                    string prodImage = System.Web.Hosting.HostingEnvironment.MapPath("~/productimages") + "\\product-" + dtProduct.Rows[i]["ProductID"].ToString();
                    // ---------------------------------------------------------
                    tblProdInfo.AddCell(setCell((i + 1).ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));
                    tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["ProductNameLong"].ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 2));
                    tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["UnitSize"].ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));
                    tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["ProductGroupName"].ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));
                    tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["Quantity"].ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 2));
                    // ---------------------------------------------------------
                    iTextSharp.text.Image myProdImage = findProductImage(prodImage);
                    if (myProdImage != null)
                    {
                        myProdImage.ScaleAbsolute(60, 60);
                        tblProdInfo.AddCell(setCellFixImage(myProdImage, WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));
                    }
                    else
                    {
                        tblProdInfo.AddCell(setCell("No Image Found", WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 2));
                    }
                }
                // ------------------------------------------
                tblProdInfo.WidthPercentage = 100f;
                tblProdInfo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                return tblProdInfo;
            }
        }
        else  // For Ambani
        {
            PdfPTable tblProdInfo = new PdfPTable(7);
            int[] column_tblProdInfo = { 5, 30, 10, 10, 10, 10, 25 };
            tblProdInfo.SetWidths(column_tblProdInfo);
            // ----------------------------------------------------------
            tblProdInfo.AddCell(setCell("Sr.#", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
            tblProdInfo.AddCell(setCell("Description", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
            tblProdInfo.AddCell(setCell("Size", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
            tblProdInfo.AddCell(setCell("Grade", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
            tblProdInfo.AddCell(setCell("Unit", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
            tblProdInfo.AddCell(setCell("Quantity", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 3));
            tblProdInfo.AddCell(setCell("Image", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 3));
            // ----------------------------------------------------------
            for (int i = 0; i < dtProduct.Rows.Count; i++)
            {
                string prodImage = System.Web.Hosting.HostingEnvironment.MapPath("~/productimages") + "\\product-" + dtProduct.Rows[i]["ProductID"].ToString();
                // ---------------------------------------------------------
                tblProdInfo.AddCell(setCell((i + 1).ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["ProductNameLong"].ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["UnitSize"].ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["UnitGrade"].ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["Unit"].ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                tblProdInfo.AddCell(setCell(dtProduct.Rows[i]["Quantity"].ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                // ---------------------------------------------------------
                iTextSharp.text.Image myProdImage = findProductImage(prodImage);
                if (myProdImage != null)
                {
                    myProdImage.ScaleAbsolute(60, 60);
                    tblProdInfo.AddCell(setCellFixImage(myProdImage, WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
                else
                {
                    tblProdInfo.AddCell(setCell("No Image Found", WhiteBaseColor, fnCalibriBold8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
                }
            }
            // ------------------------------------------
            tblProdInfo.WidthPercentage = 100f;
            tblProdInfo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            return tblProdInfo;
        }
    }

    public iTextSharp.text.Image findProductImage(string prodFileName, float pWidth = 40f, float pHeight = 40f)
     {
        //prodImage = System.Web.Hosting.HostingEnvironment.MapPath("~/productimages") + "\\product-" + dtItem.Rows[i]["ProductID"].ToString() + ".jpg";
        Boolean imgFound = false;
        string prodImage = "";
        // --------------------------------
        prodImage = prodFileName;
        if (File.Exists(prodImage))
        {
            if (!imgFound)
            {
                imgFound = true;
                iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                myImage.ScaleAbsolute(pWidth, pHeight);
                return myImage;
            }
        }
        else
        {
            prodImage = prodFileName + ".png";
            if (File.Exists(prodImage))
            {
                if (!imgFound)
                {
                    imgFound = true;
                    iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                    myImage.ScaleAbsolute(pWidth, pHeight);
                    return myImage;
                }
            }
            else
            {
                prodImage = prodFileName + ".jpeg";
                if (File.Exists(prodImage))
                {
                    if (!imgFound)
                    {
                        imgFound = true;
                        iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                        myImage.ScaleAbsolute(pWidth, pHeight);
                        return myImage;
                    }
                }
                else
                {
                    prodImage = prodFileName + ".bmp";
                    if (File.Exists(prodImage))
                    {
                        if (!imgFound)
                        {
                            imgFound = true;
                            iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                            myImage.ScaleAbsolute(pWidth, pHeight);
                            return myImage;
                        }
                    }
                }
            }
        }
        return null;
    }

    public iTextSharp.text.Image findFolerImage(string RootFolderPath, string prodFileName, float pWidth = 40f, float pHeight = 40f)
    {
        //prodImage = System.Web.Hosting.HostingEnvironment.MapPath("~/productimages") + "\\product-" + dtItem.Rows[i]["ProductID"].ToString() + ".jpg";
        Boolean imgFound = false;
        string prodImage = "";
        // --------------------------------
        prodImage = System.Web.Hosting.HostingEnvironment.MapPath("~/" + RootFolderPath + "/") + prodFileName;
        if (File.Exists(prodImage))
        {
            if (!imgFound)
            {
                imgFound = true;
                iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                if (pWidth>0 && pHeight>0)
                    myImage.ScaleAbsolute(pWidth, pHeight);
                return myImage;
            }
        }
        else
        {
            prodImage = prodFileName + ".png";
            if (File.Exists(prodImage))
            {
                if (!imgFound)
                {
                    imgFound = true;
                    iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                    if (pWidth > 0 && pHeight > 0)
                        myImage.ScaleAbsolute(pWidth, pHeight);
                    return myImage;
                }
            }
            else
            {
                prodImage = prodFileName + ".jpeg";
                if (File.Exists(prodImage))
                {
                    if (!imgFound)
                    {
                        imgFound = true;
                        iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                        if (pWidth > 0 && pHeight > 0)
                            myImage.ScaleAbsolute(pWidth, pHeight);
                        return myImage;
                    }
                }
                else
                {
                    prodImage = prodFileName + ".bmp";
                    if (File.Exists(prodImage))
                    {
                        if (!imgFound)
                        {
                            imgFound = true;
                            iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(prodImage);
                            if (pWidth > 0 && pHeight > 0)
                                myImage.ScaleAbsolute(pWidth, pHeight);
                            return myImage;
                        }
                    }
                }
            }
        }
        return null;
    }

    public PdfPTable BillTaxAndAmount(List<Entity.Quotation> lstQuotation, DataTable dtItem, List<Entity.OtherCharge> lstCharge, Int16 pBorderVal, Int16 pTemplateID = 1)
    {
        //List<Entity.QuotationDetail> lstItem = new List<Entity.QuotationDetail>();
        //lstItem = ConvertDataTableToList<Entity.QuotationDetail>(dtItem);

        PdfPTable tblAmount = new PdfPTable(2);
        int[] column_tblAmount = { 60, 40 };
        tblAmount.SetWidths(column_tblAmount);
        tblAmount.HorizontalAlignment = Element.ALIGN_LEFT;
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        Decimal totAmount = 0, totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

        totAmount = lstQuotation[0].BasicAmt + lstQuotation[0].DiscountAmt;
        totRNDOff = lstQuotation[0].ROffAmt;
        totGST = (lstQuotation[0].SGSTAmt + lstQuotation[0].CGSTAmt + lstQuotation[0].IGSTAmt);

        if (lstQuotation[0].DiscountAmt != 0)
            tblAmount.AddCell(setCell("Total Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        else
            tblAmount.AddCell(setCell("Basic Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
            tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        else
            tblAmount.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        if (lstQuotation[0].DiscountAmt != 0)
        {
            tblAmount.AddCell(setCell("Discount Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].DiscountAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].DiscountAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

            tblAmount.AddCell(setCell("Basic Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].DiscountAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].BasicAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            befAmt += lstQuotation[0].ChargeAmt1;
            befGST += lstQuotation[0].ChargeGSTAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + " :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeGSTAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeGSTAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        }
        if (lstQuotation[0].ChargeGSTAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            befAmt += lstQuotation[0].ChargeAmt2;
            befGST += lstQuotation[0].ChargeGSTAmt2;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + " :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeGSTAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeGSTAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        }
        if (lstQuotation[0].ChargeGSTAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            befAmt += lstQuotation[0].ChargeAmt3;
            befGST += lstQuotation[0].ChargeGSTAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + " :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeGSTAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeGSTAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        }
        if (lstQuotation[0].ChargeGSTAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            befAmt += lstQuotation[0].ChargeAmt4;
            befGST += lstQuotation[0].ChargeGSTAmt4;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + " :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeGSTAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeGSTAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        }
        if (lstQuotation[0].ChargeGSTAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            befAmt += lstQuotation[0].ChargeAmt5;
            befGST += lstQuotation[0].ChargeGSTAmt5;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + " :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeGSTAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeGSTAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));

        }
        /* ---------------------------------------------------------- */
        if ((lstQuotation[0].CGSTAmt + lstQuotation[0].SGSTAmt) != 0)
        {
            List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
            lstTaxList = BAL.CommonMgmt.GetTaxSummary("quotation", "cgst", lstQuotation[0].QuotationNo);
            for (int i = 0; i < lstTaxList.Count; i++)
            {
                if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
                {
                    tblAmount.AddCell(setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    var phrase1 = new Phrase();
                    phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("0.00"), fnCalibri8));
                    tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                    tblAmount.AddCell(setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    phrase1 = new Phrase();
                    phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("0.00"), fnCalibri8));
                    tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
            }
        }
        if (lstQuotation[0].IGSTAmt != 0)
        {
            List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
            lstTaxList1 = BAL.CommonMgmt.GetTaxSummary("quotation", "igst", lstQuotation[0].QuotationNo);
            for (int i = 0; i < lstTaxList1.Count; i++)
            {
                if (lstTaxList1[i].IGSTAmt > 0)
                {
                    tblAmount.AddCell(setCell("IGST @ " + lstTaxList1[i].IGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                    var phrase1 = new Phrase();
                    phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("0.00"), fnCalibri8));
                    tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
                }
            }
        }
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 == 0 && lstQuotation[0].ChargeAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            aftAmt += lstQuotation[0].ChargeAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + (chrgPer != Convert.ToDecimal(0.00) ? strChrgPer : " :"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 == 0 && lstQuotation[0].ChargeAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            aftAmt += lstQuotation[0].ChargeAmt2;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + (chrgPer != Convert.ToDecimal(0.00) ? strChrgPer : " :"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 == 0 && lstQuotation[0].ChargeAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            aftAmt += lstQuotation[0].ChargeAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + (chrgPer != Convert.ToDecimal(0.00) ? strChrgPer : " :"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 == 0 && lstQuotation[0].ChargeAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            aftAmt += lstQuotation[0].ChargeAmt4;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + (chrgPer != Convert.ToDecimal(0.00) ? strChrgPer : " :"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 == 0 && lstQuotation[0].ChargeAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : " :";
            aftAmt += lstQuotation[0].ChargeAmt5;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + (chrgPer != Convert.ToDecimal(0.00) ? strChrgPer : " :"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ROffAmt != 0)
        {
            tblAmount.AddCell(setCell("Round Off :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ROffAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
            else
                tblAmount.AddCell(setCell((lstQuotation[0].ROffAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 0));
        }
        tblAmount.AddCell(setCell("Grand Total :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
        if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
            tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].NetAmt.ToString("0.00"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));
        else
            tblAmount.AddCell(setCell((lstQuotation[0].NetAmt).ToString("0.00"), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 15));

        return tblAmount;
    }

    public PdfPTable BillTaxAndAmount(List<Entity.SalesOrder> lstQuotation, List<Entity.OtherCharge> lstCharge, Int16 pBorderVal, Int16 pTemplateID = 1)
    {
        //List<Entity.SalesBillDetail> lstItem = new List<Entity.SalesBillDetail>();
        //lstItem = ConvertDataTableToList<Entity.SalesBillDetail>(dtItem);

        PdfPTable tblAmount = new PdfPTable(2);
        int[] column_tblAmount = { 60, 40 };
        tblAmount.SetWidths(column_tblAmount);
        tblAmount.HorizontalAlignment = Element.ALIGN_LEFT;
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        Decimal totAmount = 0, totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

        totAmount = lstQuotation[0].BasicAmt;
        totRNDOff = lstQuotation[0].ROffAmt;
        totGST = (lstQuotation[0].SGSTAmt + lstQuotation[0].CGSTAmt + lstQuotation[0].IGSTAmt);

        tblAmount.AddCell(setCell("Basic Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
            tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        else
            tblAmount.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt1;
            befGST += lstQuotation[0].ChargeGSTAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt2;
            befGST += lstQuotation[0].ChargeGSTAmt2;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt3;
            befGST += lstQuotation[0].ChargeGSTAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt4;
            befGST += lstQuotation[0].ChargeGSTAmt4;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt5;
            befGST += lstQuotation[0].ChargeGSTAmt5;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        /* ---------------------------------------------------------- */
        if (String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()) && lstQuotation[0].ExchangeRate == 0)
        {
            if (lstQuotation[0].IGSTAmt > 0)
            {
                tblAmount.AddCell(setCell("IGST @ 18% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                else
                    tblAmount.AddCell(setCell(lstQuotation[0].IGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            }
            else if ((lstQuotation[0].CGSTAmt + lstQuotation[0].SGSTAmt) > 0)
            {
                //tblAmount.AddCell(setCell("CGST @ 9% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(lstQuotation[0].CGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //tblAmount.AddCell(setCell("SGST @ 9% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(lstQuotation[0].SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //var summaryTax = lstItem.GroupBy(x => x.SGSTPer).Select(x => new { SGSTPer = x.Key, SGSTAmt = x.Sum(ta => ta.SGSTAmt) }).ToList();
                //foreach (var cRow in summaryTax)
                //{
                //    tblAmount.AddCell(setCell("CGST @ " + cRow.SGSTPer.ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblAmount.AddCell(setCell(cRow.SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //    tblAmount.AddCell(setCell("SGST @ " + cRow.SGSTPer.ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblAmount.AddCell(setCell(cRow.SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}

                List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
                lstTaxList = BAL.CommonMgmt.GetTaxSummary("salesorder", "cgst", lstQuotation[0].QuotationNo);
                for (int i = 0; i < lstTaxList.Count; i++)
                {
                    if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
                    {
                        tblAmount.AddCell(setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        var phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                        tblAmount.AddCell(setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                }
                List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
                lstTaxList1 = BAL.CommonMgmt.GetTaxSummary("salesorder", "igst", lstQuotation[0].QuotationNo);
                for (int i = 0; i < lstTaxList1.Count; i++)
                {
                    if (lstTaxList1[i].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(setCell("IGST @ " + lstTaxList1[i].IGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        var phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                }
            }
        }

        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 == 0 && lstQuotation[0].ChargeAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 == 0 && lstQuotation[0].ChargeAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt2;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 == 0 && lstQuotation[0].ChargeAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 == 0 && lstQuotation[0].ChargeAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt4;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 == 0 && lstQuotation[0].ChargeAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt5;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ROffAmt > 0 || lstQuotation[0].ROffAmt < 0)
        {
            tblAmount.AddCell(setCell("Round Off    :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ROffAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell((lstQuotation[0].ROffAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }

        tblAmount.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

        return tblAmount;
    }

    public PdfPTable BillTaxAndAmount(List<Entity.SalesBill> lstQuotation, DataTable dtItem, List<Entity.OtherCharge> lstCharge, Int16 pBorderVal, Int16 pTemplateID = 1)
    {
        //List<Entity.SalesBillDetail> lstItem = new List<Entity.SalesBillDetail>();
        //lstItem = ConvertDataTableToList<Entity.SalesBillDetail>(dtItem);

        PdfPTable tblAmount = new PdfPTable(2);
        int[] column_tblAmount = { 60, 40 };
        tblAmount.SetWidths(column_tblAmount);
        tblAmount.HorizontalAlignment = Element.ALIGN_LEFT;
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        Decimal totAmount = 0, totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

        totAmount = lstQuotation[0].BasicAmt;
        totRNDOff = lstQuotation[0].ROffAmt;
        totGST = (lstQuotation[0].SGSTAmt + lstQuotation[0].CGSTAmt + lstQuotation[0].IGSTAmt);

        tblAmount.AddCell(setCell("Basic Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
            tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        else
            tblAmount.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt1;
            befGST += lstQuotation[0].ChargeGSTAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt2;
            befGST += lstQuotation[0].ChargeGSTAmt2;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt3;
            befGST += lstQuotation[0].ChargeGSTAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt4;
            befGST += lstQuotation[0].ChargeGSTAmt4;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt5;
            befGST += lstQuotation[0].ChargeGSTAmt5;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        /* ---------------------------------------------------------- */
        if (String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()) && lstQuotation[0].ExchangeRate == 0)
        {
            if (lstQuotation[0].IGSTAmt > 0)
            {
                tblAmount.AddCell(setCell("IGST @ 18% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                else
                    tblAmount.AddCell(setCell(lstQuotation[0].IGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            }
            else if ((lstQuotation[0].CGSTAmt + lstQuotation[0].SGSTAmt) > 0)
            {
                //tblAmount.AddCell(setCell("CGST @ 9% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(lstQuotation[0].CGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //tblAmount.AddCell(setCell("SGST @ 9% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(lstQuotation[0].SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //var summaryTax = dtItem.AsEnumerable().ToList().GroupBy(x => x. x.Field<decimal>("SGSTPer")).Select(x => new { SGSTPer = x.Key, SGSTAmt = x.Sum(ta => ta.Field<decimal>("SGSTAmt")) }).ToList();
                //var summaryTax = lstItem.GroupBy(x => x.SGSTPer).Select(x => new { SGSTPer = x.Key, SGSTAmt = x.Sum(ta => ta.SGSTAmt) }).ToList();
                //foreach (var cRow in summaryTax)
                //{
                //    tblAmount.AddCell(setCell("CGST @ " + cRow.SGSTPer.ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblAmount.AddCell(setCell(cRow.SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //    tblAmount.AddCell(setCell("SGST @ " + cRow.SGSTPer.ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblAmount.AddCell(setCell(cRow.SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}

                // Multiple TAX Amount
                //------------------------------------CGST & SGST Summary----------------------------------
                List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
                lstTaxList = BAL.CommonMgmt.GetTaxSummary("salesbill", "cgst", lstQuotation[0].QuotationNo);
                for (int i = 0; i < lstTaxList.Count; i++)
                {
                    if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
                    {
                        tblAmount.AddCell(setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        var phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                        tblAmount.AddCell(setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                }
                //----------------------------------IGST Summary-----------------------------------------
                List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
                lstTaxList1 = BAL.CommonMgmt.GetTaxSummary("salesbill", "igst", lstQuotation[0].QuotationNo);
                for (int i = 0; i < lstTaxList1.Count; i++)
                {
                    if (lstTaxList1[i].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(setCell("IGST @ " + lstTaxList1[i].IGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        var phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                }
            }
        }

        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 == 0 && lstQuotation[0].ChargeAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 == 0 && lstQuotation[0].ChargeAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt2;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 == 0 && lstQuotation[0].ChargeAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 == 0 && lstQuotation[0].ChargeAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt4;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 == 0 && lstQuotation[0].ChargeAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt5;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ROffAmt > 0 || lstQuotation[0].ROffAmt < 0)
        {
            tblAmount.AddCell(setCell("Round Off    :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ROffAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell((lstQuotation[0].ROffAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }

        tblAmount.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

        return tblAmount;
    }
    public PdfPTable BillTaxAndAmount(List<Entity.SalesChallan> lstQuotation, DataTable dtItem, List<Entity.OtherCharge> lstCharge, Int16 pBorderVal, Int16 pTemplateID = 1)
    {
        //List<Entity.SalesBillDetail> lstItem = new List<Entity.SalesBillDetail>();
        //lstItem = ConvertDataTableToList<Entity.SalesBillDetail>(dtItem);

        PdfPTable tblAmount = new PdfPTable(2);
        int[] column_tblAmount = { 60, 40 };
        tblAmount.SetWidths(column_tblAmount);
        tblAmount.HorizontalAlignment = Element.ALIGN_LEFT;
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        Decimal totAmount = 0, totGST = 0, befGST = 0, befAmt = 0, aftGST = 0, aftAmt = 0, totRNDOff = 0;

        totAmount = lstQuotation[0].BasicAmt;
        totRNDOff = lstQuotation[0].ROffAmt;
        totGST = (lstQuotation[0].SGSTAmt + lstQuotation[0].CGSTAmt + lstQuotation[0].IGSTAmt);

        tblAmount.AddCell(setCell("Basic Amount :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
            tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        else
            tblAmount.AddCell(setCell(totAmount.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt1;
            befGST += lstQuotation[0].ChargeGSTAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt2;
            befGST += lstQuotation[0].ChargeGSTAmt2;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt3;
            befGST += lstQuotation[0].ChargeGSTAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt4;
            befGST += lstQuotation[0].ChargeGSTAmt4;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            befAmt += lstQuotation[0].ChargeAmt5;
            befGST += lstQuotation[0].ChargeGSTAmt5;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        /* ---------------------------------------------------------- */
        if (String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()) && lstQuotation[0].ExchangeRate == 0)
        {
            if (lstQuotation[0].IGSTAmt > 0)
            {
                tblAmount.AddCell(setCell("IGST @ 18% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                else
                    tblAmount.AddCell(setCell(lstQuotation[0].IGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            }
            else if ((lstQuotation[0].CGSTAmt + lstQuotation[0].SGSTAmt) > 0)
            {
                //tblAmount.AddCell(setCell("CGST @ 9% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(lstQuotation[0].CGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //tblAmount.AddCell(setCell("SGST @ 9% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                //    tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + (totGST + befGST).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                //else
                //    tblAmount.AddCell(setCell(lstQuotation[0].SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                //var summaryTax = dtItem.AsEnumerable().ToList().GroupBy(x => x. x.Field<decimal>("SGSTPer")).Select(x => new { SGSTPer = x.Key, SGSTAmt = x.Sum(ta => ta.Field<decimal>("SGSTAmt")) }).ToList();
                //var summaryTax = lstItem.GroupBy(x => x.SGSTPer).Select(x => new { SGSTPer = x.Key, SGSTAmt = x.Sum(ta => ta.SGSTAmt) }).ToList();
                //foreach (var cRow in summaryTax)
                //{
                //    tblAmount.AddCell(setCell("CGST @ " + cRow.SGSTPer.ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblAmount.AddCell(setCell(cRow.SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

                //    tblAmount.AddCell(setCell("SGST @ " + cRow.SGSTPer.ToString(), WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                //    tblAmount.AddCell(setCell(cRow.SGSTAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
                //}

                // Multiple TAX Amount
                //------------------------------------CGST & SGST Summary----------------------------------
                List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
                lstTaxList = BAL.CommonMgmt.GetTaxSummary("salesbill", "cgst", lstQuotation[0].QuotationNo);
                for (int i = 0; i < lstTaxList.Count; i++)
                {
                    if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
                    {
                        tblAmount.AddCell(setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        var phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

                        tblAmount.AddCell(setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                }
                //----------------------------------IGST Summary-----------------------------------------
                List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
                lstTaxList1 = BAL.CommonMgmt.GetTaxSummary("salesbill", "igst", lstQuotation[0].QuotationNo);
                for (int i = 0; i < lstTaxList1.Count; i++)
                {
                    if (lstTaxList1[i].IGSTAmt > 0)
                    {
                        tblAmount.AddCell(setCell("IGST @ " + lstTaxList1[i].IGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                        var phrase1 = new Phrase();
                        phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("0.00"), fnCalibri8));
                        tblAmount.AddCell(setCell(phrase1, WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
                    }
                }
            }
        }

        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ChargeGSTAmt1 == 0 && lstQuotation[0].ChargeAmt1 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID1).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt1;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName1.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt1.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt2 == 0 && lstQuotation[0].ChargeAmt2 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID2).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt2;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName2.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt2.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt3 == 0 && lstQuotation[0].ChargeAmt3 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID3).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt3;
            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName3.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt3.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt4 == 0 && lstQuotation[0].ChargeAmt4 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID4).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt4;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName4.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt4.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        if (lstQuotation[0].ChargeGSTAmt5 == 0 && lstQuotation[0].ChargeAmt5 > 0)
        {
            decimal chrgPer = lstCharge.Where(x => x.pkID == lstQuotation[0].ChargeID5).Select(x => x.GST_Per).FirstOrDefault();
            string strChrgPer = (chrgPer > 0) ? " @" + chrgPer.ToString() + "% :" : ":";
            aftAmt += lstQuotation[0].ChargeAmt5;

            tblAmount.AddCell(setCell(lstQuotation[0].ChargeName5.ToString() + strChrgPer, WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell(lstQuotation[0].ChargeAmt5.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }
        /* ---------------------------------------------------------- */
        if (lstQuotation[0].ROffAmt > 0 || lstQuotation[0].ROffAmt < 0)
        {
            tblAmount.AddCell(setCell("Round Off    :", WhiteBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            if (!String.IsNullOrEmpty(lstQuotation[0].CurrencySymbol.Trim()))
                tblAmount.AddCell(setCell(lstQuotation[0].CurrencySymbol.Trim() + " " + lstQuotation[0].ROffAmt.ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
            else
                tblAmount.AddCell(setCell((lstQuotation[0].ROffAmt).ToString("0.00"), WhiteBaseColor, fnCalibri8, paddingOf3, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        }

        tblAmount.AddCell(setCell(" ", WhiteBaseColor, fnCalibriBold8, paddingOf3, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

        return tblAmount;
    }


    public PdfPTable Table_MultipleGST(String OrderNo, string pModule, int[] noOfColsStruc, iTextSharp.text.Font HeadFont, iTextSharp.text.Font DetailFont, Int16 pBorderVal = 0, float pPadding = 2)
    {
        PdfPTable tmpTable = new PdfPTable(2);
        tmpTable.SetWidths(noOfColsStruc);

        List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
        lstTaxList = BAL.CommonMgmt.GetTaxSummary(pModule, "cgst", OrderNo);
        for (int i = 0; i < lstTaxList.Count; i++)
        {
            if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
            {
                tmpTable.AddCell(setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString("##,##0.00") + "% :", WhiteBaseColor, HeadFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("##,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, WhiteBaseColor, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));

                tmpTable.AddCell(setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString("##,##0.00") + "% :", WhiteBaseColor, HeadFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
                phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("##,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, WhiteBaseColor, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
            }
        }
        //----------------------------------IGST Summary-----------------------------------------
        List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
        lstTaxList1 = BAL.CommonMgmt.GetTaxSummary(pModule, "igst", OrderNo);
        for (int i = 0; i < lstTaxList1.Count; i++)
        {
            if (lstTaxList1[i].IGSTAmt > 0)
            {
                tmpTable.AddCell(setCell("IGST @ " + lstTaxList1[i].IGSTPer.ToString("##,##0.00") + "% :", WhiteBaseColor, HeadFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("##,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, WhiteBaseColor, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
            }
        }
        return tmpTable;
    }

    public PdfPTable Table_MultipleGSTwithoutcollon(String OrderNo, string pModule, int[] noOfColsStruc, iTextSharp.text.Font HeadFont, iTextSharp.text.Font DetailFont, Int16 pBorderVal = 0, float pPadding = 2)
    {
        PdfPTable tmpTable = new PdfPTable(2);
        tmpTable.SetWidths(noOfColsStruc);

        List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
        lstTaxList = BAL.CommonMgmt.GetTaxSummary(pModule, "cgst", OrderNo);
        for (int i = 0; i < lstTaxList.Count; i++)
        {
            if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
            {
                tmpTable.AddCell(setCell("CGST  ", WhiteBaseColor, HeadFont, pPadding, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("##,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, WhiteBaseColor, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));

                tmpTable.AddCell(setCell("SGST ", WhiteBaseColor, HeadFont, pPadding, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("##,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, WhiteBaseColor, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
            }
        }
        //----------------------------------IGST Summary-----------------------------------------
        List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
        lstTaxList1 = BAL.CommonMgmt.GetTaxSummary(pModule, "igst", OrderNo);
        for (int i = 0; i < lstTaxList1.Count; i++)
        {
            if (lstTaxList1[i].IGSTAmt > 0)
            {
                tmpTable.AddCell(setCell("IGST ", WhiteBaseColor, HeadFont, pPadding, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("##,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, WhiteBaseColor, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, pBorderVal));
            }
        }
        return tmpTable;
    }
    public PdfPTable Table_MultipleGSTGreyBackground (String OrderNo, string pModule, int[] noOfColsStruc, iTextSharp.text.Font HeadFont, iTextSharp.text.Font DetailFont, Int16 pBorderVal = 0, float pPadding = 2)
    {
        PdfPTable tmpTable = new PdfPTable(2);
        tmpTable.SetWidths(noOfColsStruc);

        List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
        lstTaxList = BAL.CommonMgmt.GetTaxSummary(pModule, "cgst", OrderNo);
        for (int i = 0; i < lstTaxList.Count; i++)
        {
            if ((lstTaxList[i].CGSTAmt + lstTaxList[i].SGSTAmt) > 0)
            {
                tmpTable.AddCell(setCell("CGST @ " + lstTaxList[i].CGSTPer.ToString("#,##0.00") + "% :", LightGrayGainsboro, HeadFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 11));
                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("#,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, LightGrayGainsboro, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));

                tmpTable.AddCell(setCell("SGST @ " + lstTaxList[i].SGSTPer.ToString("#,##0.00") + "% :", LightGrayGainsboro, HeadFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 11));
                phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("#,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, LightGrayGainsboro, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));
            }
        }
        //----------------------------------IGST Summary-----------------------------------------
        List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
        lstTaxList1 = BAL.CommonMgmt.GetTaxSummary(pModule, "igst", OrderNo);
        for (int i = 0; i < lstTaxList1.Count; i++)
        {
            if (lstTaxList1[i].IGSTAmt > 0)
            {
                tmpTable.AddCell(setCell("IGST @ " + lstTaxList1[i].IGSTPer.ToString("#,##0.00") + "% :", LightGrayGainsboro, HeadFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 11));
                var phrase1 = new Phrase();
                phrase1.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("#,##0.00"), DetailFont));
                tmpTable.AddCell(setCell(phrase1, LightGrayGainsboro, DetailFont, pPadding, 1, Element.ALIGN_RIGHT, Element.ALIGN_TOP, 2));
            }
            
        }
        return tmpTable;
    }

    public PdfPTable Table_MultipleGSTWithHSN(String OrderNo, string pModule, iTextSharp.text.Font HeadFont, iTextSharp.text.Font DetailFont)
    {
        PdfPTable tmpTable;
        tmpTable = new PdfPTable(5);

        List<Entity.QuotationDetail> lstTaxList = new List<Entity.QuotationDetail>();
        lstTaxList = BAL.CommonMgmt.GetTaxHSNSummary(pModule, "cgst", OrderNo);

        if (lstTaxList[0].CGSTAmt > 0)
        {
            tmpTable = new PdfPTable(5);
            int[] column_tblCGST = { 20, 20, 20, 20, 20 };
            tmpTable.SetWidths(column_tblCGST);

            tmpTable.AddCell(setCell("HSNCode", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("CGST Per.", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("CGST Amount", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("SGST Per.", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("SGST Amount", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));

            decimal cgst = 0, sgst = 0;

            for (int i = 0; i < lstTaxList.Count; i++)
            {
                if (lstTaxList[i].CGSTAmt > 0)
                {
                    cgst += lstTaxList[i].CGSTAmt;
                    sgst += lstTaxList[i].SGSTAmt;

                    tmpTable.AddCell(setCell(lstTaxList[i].HSNCode, WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                    tmpTable.AddCell(setCell(lstTaxList[i].CGSTPer.ToString("0.00") + "%", WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                    var phrase = new Phrase();
                    phrase.Add(new Chunk(lstTaxList[i].CGSTAmt.ToString("0.00"), fnCalibri9));
                    tmpTable.AddCell(setCell(phrase, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));

                    tmpTable.AddCell(setCell(lstTaxList[i].SGSTPer.ToString("0.00") + "%", WhiteBaseColor, fnCalibri9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                    phrase = new Phrase();
                    phrase.Add(new Chunk(lstTaxList[i].SGSTAmt.ToString("0.00"), fnCalibri9));
                    tmpTable.AddCell(setCell(phrase, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                }
            }

            tmpTable.AddCell(setCell("Total", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("CGST  -", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell(cgst.ToString(), WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("SGST  -", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell(sgst.ToString(), WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
        }

        //----------------------------------IGST Summary-----------------------------------------
        List<Entity.QuotationDetail> lstTaxList1 = new List<Entity.QuotationDetail>();
        lstTaxList1 = BAL.CommonMgmt.GetTaxHSNSummary(pModule, "igst", OrderNo);
        if (lstTaxList1[0].IGSTAmt > 0)
        {
            tmpTable = new PdfPTable(3);
            int[] column_tblIGST = { 33, 33, 34 };
            tmpTable.SetWidths(column_tblIGST);

            tmpTable.AddCell(setCell("HSNCode", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("IGST Per.", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("IGST Amount", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));

            decimal igst = 0;

            for (int i = 0; i < lstTaxList1.Count; i++)
            {

                tmpTable.AddCell(setCell(lstTaxList1[i].HSNCode, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                if (lstTaxList1[i].IGSTAmt > 0)
                {
                    igst += lstTaxList1[i].IGSTAmt;
                    tmpTable.AddCell(setCell(lstTaxList1[i].IGSTPer.ToString("0.00") + "% :", WhiteBaseColor, fnCalibriBold9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                    var phrase2 = new Phrase();
                    phrase2.Add(new Chunk(lstTaxList1[i].IGSTAmt.ToString("0.00"), fnCalibri9));
                    tmpTable.AddCell(setCell(phrase2, WhiteBaseColor, fnCalibri9, paddingOf3, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 0));
                }
            }

            tmpTable.AddCell(setCell("Total", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell("IGST  -", WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));
            tmpTable.AddCell(setCell(igst.ToString(), WhiteBaseColor, fnCalibriBold9, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, 15));

        }
        return tmpTable;
    }

    public PdfPTable Table_ProductDetail(DataTable dtProduct, Int64 maxLines, Int16 pBorderVal)
    {
        string[,] tblStructure = new string[8, 5]
        {{"Sr.No", "counter", "5", "0", "0"}, {"Product Name", "ProductName", "30", "0", "0"}, {"Unit", "Unit", "6", "0", "0"},
         {"Qty", "Quantity", "6", "1", "0"},  {"Unit Rate", "UnitRate", "8", "2", "0"}, {"Dis.%", "DiscountPercent", "6", "1", "0"},
         {"Net Rate", "NetRate", "8", "2", "0"},{"Amount", "Amount", "10", "2", "0"}};
        int cols = tblStructure.GetLength(0);
        PdfPTable tmpTable = new PdfPTable(cols);

        //int[] column_tblNested = new int[cols];
        int[] column_tblNested = { 5, 50, 6, 6, 8, 6, 8, 10 };


        for (int i = 0; i < cols; i++)
        {
            tmpTable.AddCell(setCell(tblStructure[i, 0].ToString(), LightBlueBaseColor, fnCalibriBold8, paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.BOX));
            column_tblNested[i] = Convert.ToInt16(tblStructure[i, 2]);
        }
        // -----------------------------------
        tmpTable.SetWidths(column_tblNested);
        tmpTable.HorizontalAlignment = Element.ALIGN_CENTER;
        // -----------------------------------
        int totPrintLines = 0;
        for (int i = 0; i < dtProduct.Rows.Count; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                string fieldName = tblStructure[j, 1].ToString();
                if (fieldName == "counter")
                    tmpTable.AddCell(setCell((i + 1).ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, pBorderVal));
                else
                    tmpTable.AddCell(setCell(dtProduct.Rows[i][fieldName].ToString(), WhiteBaseColor, fnCalibri8, paddingOf2, 1, Element.ALIGN_CENTER, Element.ALIGN_TOP, pBorderVal));
            }
            totPrintLines = totPrintLines + 1;
        }
        tmpTable.AddCell(setCell(RestOfTable(maxLines, totPrintLines, column_tblNested, pBorderVal), WhiteBaseColor, fnArialBold8, 0, 8, Element.ALIGN_LEFT, Element.ALIGN_TOP, pBorderVal));
        // -----------------------------------
        tmpTable.WidthPercentage = 100f;
        tmpTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        return tmpTable;
    }

    public PdfPTable Table_ProductDetail(string pModule, DataTable dtProduct, string[,] tblStructure, int[] noOfColsStruc, Int64 maxLines, Int16 pBorderVal, iTextSharp.text.Font fnFont, Int16 pSpecTemplate = 0, string pSpecFieldName = "", Int16 pSpecColumnPos = 0)
    {
        string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
        // Definition Of tblStructure ......
        // Paramenters : para0, para1, para2,   para3,     para4,     para5
        // Paramenters : Title, Field, Padding, Align_Hor, Align_Ver, Box

        //tblStructure = new string[8, 5]
        //{{"Sr.No", "counter", "paddingOf2", "Align_Left", "Align_Top", }, {"Product Name", "ProductName", "30", "0", "0"}, {"Unit", "Unit", "6", "0", "0"},

        int cols = tblStructure.GetLength(0);
        PdfPTable tmpTable = new PdfPTable(cols);
        tmpTable.SetWidths(noOfColsStruc);
        tmpTable.HorizontalAlignment = Element.ALIGN_CENTER;
        // ---------------------------------------------------------------------------------------------------------
        // Product Table : Title
        // ---------------------------------------------------------------------------------------------------------
        if (tmpSerialKey == "8YWQ-DDRO-V98V-LDN2" && pModule.ToLower() == "salesbill")      // FieldMaster 
        {
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));

            tmpTable.AddCell(setCell("Discount", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("SGST", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("CGST", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("IGST", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));

            for (int i = 0; i < cols; i++)
            {
                string xTitle = tblStructure[i, 0].ToString();
                int xPadding = Convert.ToInt16(tblStructure[i, 2].ToString());
                int xHorizontal = Convert.ToInt16(tblStructure[i, 3].ToString());
                int xVertical = Convert.ToInt16(tblStructure[i, 4].ToString());
                int xBox = Convert.ToInt16(tblStructure[i, 5].ToString());

                tmpTable.AddCell(setCell(xTitle, LightBlueBaseColor, fnFont, xPadding, 1, xHorizontal, xVertical, 14));
            }
        }
        else
        {
            for (int i = 0; i < cols; i++)
            {
                string xTitle = tblStructure[i, 0].ToString();
                int xPadding = Convert.ToInt16(tblStructure[i, 2].ToString());
                int xHorizontal = Convert.ToInt16(tblStructure[i, 3].ToString());
                int xVertical = Convert.ToInt16(tblStructure[i, 4].ToString());
                int xBox = Convert.ToInt16(tblStructure[i, 5].ToString());
                int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[i, 6].ToString()) : 1;

                tmpTable.AddCell(setCell(xTitle, LightBlueBaseColor, fnFont, xPadding, xColSpan, Element.ALIGN_CENTER, xVertical, 15));
            }
        }
        // ---------------------------------------------------------------------------------------------------------
        // Product Table : Data Rows
        // ---------------------------------------------------------------------------------------------------------
        int totPrintLines = 0;
        for (int i = 0; i < dtProduct.Rows.Count; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                string fieldName = tblStructure[j, 1].ToString();
                int xPadding = Convert.ToInt16(tblStructure[j, 2].ToString());
                int xHorizontal = Convert.ToInt16(tblStructure[j, 3].ToString());
                int xVertical = Convert.ToInt16(tblStructure[j, 4].ToString());
                int xBox = Convert.ToInt16(tblStructure[j, 5].ToString());
                int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[j, 6].ToString()) : 1;
                // -------------------------------------------
                if (fieldName == "counter")
                    tmpTable.AddCell(setCell((i + 1).ToString(), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                else if (String.IsNullOrEmpty(fieldName))
                    tmpTable.AddCell(setCell("", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                else
                    tmpTable.AddCell(setCell(dtProduct.Rows[i][fieldName].ToString(), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
            }
            // -------------------------------------------------
            if (pSpecTemplate > 0)
            {
                int specPrintLines = 0;
                for (int k = 0; k < cols; k++)
                {

                    int xPadding = Convert.ToInt16(tblStructure[k, 2].ToString());
                    int xHorizontal = Convert.ToInt16(tblStructure[k, 3].ToString());
                    int xVertical = Convert.ToInt16(tblStructure[k, 4].ToString());
                    int xBox = Convert.ToInt16(tblStructure[k, 5].ToString());
                    int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[k, 6].ToString()) : 1;
                    // -------------------------------------------
                    if (k == pSpecColumnPos)
                    {
                        if (pModule.ToLower() == "quotation")
                        {
                            string xQuotNo = dtProduct.Rows[i]["QuotationNo"].ToString();
                            Int64 xProdID = Convert.ToInt64(dtProduct.Rows[i]["ProductID"].ToString());
                            tmpTable.AddCell(setCell(GetSpecification_Quotation(xQuotNo, xProdID, pSpecTemplate, fnCalibri8, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, out specPrintLines), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                            totPrintLines += specPrintLines;
                        }
                        else if (pModule.ToLower() == "salesorder")
                        {
                            string xQuotNo = dtProduct.Rows[i]["OrderNo1"].ToString();
                            Int64 xProdID = Convert.ToInt64(dtProduct.Rows[i]["ProductID"].ToString());
                            tmpTable.AddCell(setCell(GetSpecification_SalesOrder(xQuotNo, xProdID, pSpecTemplate, fnCalibri8, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, out specPrintLines), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                            totPrintLines += specPrintLines;
                        }
                        else
                        {
                            tmpTable.AddCell(setCell("", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                        }
                    }
                    else
                    {
                        tmpTable.AddCell(setCell("", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                    }
                }
            }
            // -------------------------------------------------
            totPrintLines = totPrintLines + 1;
        }
        // ---------------------------------------------------------------------------------------------------------
        // Product Table : Rest Of Blank Lines (As per set in DocPrinterSettings Table into Database)
        // ---------------------------------------------------------------------------------------------------------
        if (maxLines > totPrintLines)
        {
            Boolean staticLineFlag = true;
            for (int x = 1; x <= (maxLines - totPrintLines); x++)
            {
                for (int i = 0; i < cols; i++)
                {
                    string xTitle = tblStructure[i, 0].ToString();
                    int xPadding = Convert.ToInt16(tblStructure[i, 2].ToString());
                    int xHorizontal = Convert.ToInt16(tblStructure[i, 3].ToString());
                    int xVertical = Convert.ToInt16(tblStructure[i, 4].ToString());
                    int xBox = Convert.ToInt16(tblStructure[i, 5].ToString());
                    int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[i, 6].ToString()) : 1;

                    if (tmpSerialKey == "J63H-F8LX-B4B2-GYVZ" && xTitle.ToLower() == "description" && staticLineFlag == true && pModule.ToLower() == "salesorder")      // For HI-TECH
                    {
                        tmpTable.AddCell(setCell("Description - As Per Our Quotation", WhiteBaseColor, fnArialBold8, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                        staticLineFlag = false;
                    }
                    else
                        tmpTable.AddCell(setCell(" ", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                }
            }
        }
        // -----------------------------------
        tmpTable.WidthPercentage = 100f;
        tmpTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        return tmpTable;
    }

    //---------------------------------with Header---------------------------------

    public PdfPTable Table_ProductDetailHeader(string pModule, DataTable dtProduct, string[,] tblStructure, int[] noOfColsStruc, Int64 maxLines, Int16 pBorderVal, iTextSharp.text.Font fnHeadFont, iTextSharp.text.Font fnFont, Int16 pSpecTemplate = 0, string pSpecFieldName = "", Int16 pSpecColumnPos = 0)
    {
        string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
        // Definition Of tblStructure ......
        // Paramenters : para0, para1, para2,   para3,     para4,     para5
        // Paramenters : Title, Field, Padding, Align_Hor, Align_Ver, Box

        //tblStructure = new string[8, 5]
        //{{"Sr.No", "counter", "paddingOf2", "Align_Left", "Align_Top", }, {"Product Name", "ProductName", "30", "0", "0"}, {"Unit", "Unit", "6", "0", "0"},

        int cols = tblStructure.GetLength(0);
        PdfPTable tmpTable = new PdfPTable(cols);
        tmpTable.SetWidths(noOfColsStruc);
        tmpTable.HorizontalAlignment = Element.ALIGN_CENTER;
        //tmpTable.DefaultCell.FixedHeight = 30f;
        // ---------------------------------------------------------------------------------------------------------
        // Product Table : Title
        // ---------------------------------------------------------------------------------------------------------
        if (tmpSerialKey == "8YWQ-DDRO-V98V-LDN2" && pModule.ToLower() == "salesbill")      // FieldMaster 
        {
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));

            tmpTable.AddCell(setCell("Discount", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));
            tmpTable.AddCell(setCell("SGST", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("CGST", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("IGST", LightBlueBaseColor, fnFont, 2, 2, 1, 5, 15));
            tmpTable.AddCell(setCell("", LightBlueBaseColor, fnFont, 2, 1, 0, 5, 13));

            for (int i = 0; i < cols; i++)
            {
                string xTitle = tblStructure[i, 0].ToString();
                int xPadding = Convert.ToInt16(tblStructure[i, 2].ToString());
                int xHorizontal = Convert.ToInt16(tblStructure[i, 3].ToString());
                int xVertical = Convert.ToInt16(tblStructure[i, 4].ToString());
                int xBox = Convert.ToInt16(tblStructure[i, 5].ToString());

                tmpTable.AddCell(setCell(xTitle, LightBlueBaseColor, fnHeadFont, xPadding, 1, xHorizontal, xVertical, 14));
            }
        }
        else
        {
            for (int i = 0; i < cols; i++)
            {
                string xTitle = tblStructure[i, 0].ToString();
                int xPadding = Convert.ToInt16(tblStructure[i, 2].ToString());
                int xHorizontal = Convert.ToInt16(tblStructure[i, 3].ToString());
                int xVertical = Convert.ToInt16(tblStructure[i, 4].ToString());
                int xBox = Convert.ToInt16(tblStructure[i, 5].ToString());
                int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[i, 6].ToString()) : 1;

                tmpTable.AddCell(setCell(xTitle, BlueBaseColor, fnHeadFont, xPadding, xColSpan, Element.ALIGN_CENTER, xVertical, 15));
            }
        }
        // ---------------------------------------------------------------------------------------------------------
        // Product Table : Data Rows
        // ---------------------------------------------------------------------------------------------------------
        int totPrintLines = 0;
        for (int i = 0; i < dtProduct.Rows.Count; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                string fieldName = tblStructure[j, 1].ToString();
                int xPadding = Convert.ToInt16(tblStructure[j, 2].ToString());
                int xHorizontal = Convert.ToInt16(tblStructure[j, 3].ToString());
                int xVertical = Convert.ToInt16(tblStructure[j, 4].ToString());
                int xBox = Convert.ToInt16(tblStructure[j, 5].ToString());
                int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[j, 6].ToString()) : 1;
                // -------------------------------------------
                if (tmpSerialKey == "PRI9-DG8H-G6GF-TP5V")         // PerfectRoto
                {
                    PdfPCell tmpCell;
                    if (fieldName == "counter")
                        tmpCell = new PdfPCell(new Phrase((i + 1).ToString(), fnFont));  
                    else if(fieldName == "Amount" || fieldName == "UnitRate")
                        tmpCell = new PdfPCell(new Phrase(Convert.ToDecimal(dtProduct.Rows[i][fieldName]).ToString("##,#00.00"), fnFont));
                    else if (String.IsNullOrEmpty(fieldName))
                        tmpCell = new PdfPCell(new Phrase("", fnFont));
                    else
                        tmpCell = new PdfPCell(new Phrase(dtProduct.Rows[i][fieldName].ToString(), fnFont));
                    tmpCell.BackgroundColor = WhiteBaseColor;
                    tmpCell.Padding = xPadding;
                    tmpCell.Colspan = xColSpan;
                    tmpCell.HorizontalAlignment = xHorizontal;
                    tmpCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tmpCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    tmpCell.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                    tmpCell.Border = xBox;
                   // tmpCell.FixedHeight = 20f;
                    tmpTable.AddCell(tmpCell);
                }
                else
                {
                    if (fieldName == "counter")
                        tmpTable.AddCell(setCell((i + 1).ToString(), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                    else if (String.IsNullOrEmpty(fieldName))
                        tmpTable.AddCell(setCell("", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                    else
                        tmpTable.AddCell(setCell(dtProduct.Rows[i][fieldName].ToString(), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                }

            }
            // -------------------------------------------------
            if (pSpecTemplate > 0)
            {
                int specPrintLines = 0;
                for (int k = 0; k < cols; k++)
                {

                    int xPadding = Convert.ToInt16(tblStructure[k, 2].ToString());
                    int xHorizontal = Convert.ToInt16(tblStructure[k, 3].ToString());
                    int xVertical = Convert.ToInt16(tblStructure[k, 4].ToString());
                    int xBox = Convert.ToInt16(tblStructure[k, 5].ToString());
                    int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[k, 6].ToString()) : 1;
                    // -------------------------------------------
                    if (k == pSpecColumnPos)
                    {
                        if (pModule.ToLower() == "quotation")
                        {
                            string xQuotNo = dtProduct.Rows[i]["QuotationNo"].ToString();
                            Int64 xProdID = Convert.ToInt64(dtProduct.Rows[i]["ProductID"].ToString());
                            tmpTable.AddCell(setCell(GetSpecification_Quotation(xQuotNo, xProdID, pSpecTemplate, fnCalibri8, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, out specPrintLines), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                            totPrintLines += specPrintLines;
                        }
                        else if (pModule.ToLower() == "salesorder")
                        {
                            string xQuotNo = dtProduct.Rows[i]["OrderNo1"].ToString();
                            Int64 xProdID = Convert.ToInt64(dtProduct.Rows[i]["ProductID"].ToString());
                            tmpTable.AddCell(setCell(GetSpecification_SalesOrder(xQuotNo, xProdID, pSpecTemplate, fnCalibri8, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0, out specPrintLines), WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                            totPrintLines += specPrintLines;
                        }
                        else
                        {
                            tmpTable.AddCell(setCell("", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                        }
                    }
                    else
                    {
                        tmpTable.AddCell(setCell("", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                    }
                }
            }
            // -------------------------------------------------
            totPrintLines = totPrintLines + 1;
        }
        // ---------------------------------------------------------------------------------------------------------
        // Product Table : Rest Of Blank Lines (As per set in DocPrinterSettings Table into Database)
        // ---------------------------------------------------------------------------------------------------------
        if (maxLines > totPrintLines)
        {
            Boolean staticLineFlag = true;
            for (int x = 1; x <= (maxLines - totPrintLines); x++)
            {
                for (int i = 0; i < cols; i++)
                {
                    string xTitle = tblStructure[i, 0].ToString();
                    int xPadding = Convert.ToInt16(tblStructure[i, 2].ToString());
                    int xHorizontal = Convert.ToInt16(tblStructure[i, 3].ToString());
                    int xVertical = Convert.ToInt16(tblStructure[i, 4].ToString());
                    int xBox = Convert.ToInt16(tblStructure[i, 5].ToString());
                    int xColSpan = (tblStructure.GetLength(1) > 6) ? Convert.ToInt16(tblStructure[i, 6].ToString()) : 1;

                    if (tmpSerialKey == "J63H-F8LX-B4B2-GYVZ" && xTitle.ToLower() == "description" && staticLineFlag == true && pModule.ToLower() == "salesorder")      // For HI-TECH
                    {
                        tmpTable.AddCell(setCell("Description - As Per Our Quotation", WhiteBaseColor, fnArialBold8, xPadding, xColSpan, xHorizontal, xVertical, xBox));
                        staticLineFlag = false;
                    }
                    else
                        tmpTable.AddCell(setCell("x", WhiteBaseColor, fnFont, xPadding, xColSpan, xHorizontal, xVertical, pBorderVal));
                }
            }
        }
        // -----------------------------------
        tmpTable.WidthPercentage = 100f;
        tmpTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        return tmpTable;
    }

    public PdfPTable GetSpecification_Quotation(string pQuotationNo, Int64 pProductID, Int16 pTemplateID, iTextSharp.text.Font fnFont, Int16 pPadding, Int16 pHAlign, Int16 pVAlign, Int16 pBorderVal, out int specPrintLines)
    {
        int tmpPrintLines = 0;
        PdfPTable tmpTableSpec = new PdfPTable(1);
        int[] tmpColStruct = { 100 };
        tmpTableSpec.SetWidths(tmpColStruct);
        tmpTableSpec.HorizontalAlignment = Element.ALIGN_LEFT;
        // -----------------------------------------------------------------
        StyleSheet objStyle = new StyleSheet();
        objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
        objStyle.LoadTagStyle("body", "font-size", "1pt");
        objStyle.LoadTagStyle("body", "color", "black");
        objStyle.LoadTagStyle("body", "position", "relative");
        objStyle.LoadTagStyle("body", "margin", "0 auto");

        // -----------------------------------------------------------------
        string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

        List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
        lstSpec = BAL.ProductMgmt.GetQuotationProductSpecList(pQuotationNo, pProductID, HttpContext.Current.Session["LoginUserID"].ToString());

        List<Entity.Products> lstProdSpec = new List<Entity.Products>();
        lstProdSpec = BAL.ProductMgmt.GetQuotProdSpecList(pQuotationNo, pProductID, HttpContext.Current.Session["LoginUserID"].ToString());


        if (lstSpec.Count > 0 || lstProdSpec.Count > 0)
        {
            string tmpSpec = "";
            if (tmpSerialKey == "VAR2-DH0A-MAN9-8SIO" || tmpSerialKey == "SI08-SB94-MY45-RY15" || tmpSerialKey == "STSD-54I3-WMYE-8XB2" || tmpSerialKey == "J63H-F8LX-B4B2-GYVZ" || tmpSerialKey == "TJ7S-06Q2-8R2U-KJWG" || tmpSerialKey == "KRI1-NAS2-CHAM-SI70")  //For Sharvaya,Steelman,Hi-Tech,Gautam we take descriptive quotation specification
            {
                tmpSpec = lstProdSpec.Count > 0 ? lstProdSpec[0].ProductSpecification.ToString() : "";
            }
            else
            {
                tmpSpec = lstSpec.Count > 0 ? lstSpec[0].MaterialSpec.ToString() : "";
            }

            if (pTemplateID == 1)
            {
                if (!String.IsNullOrEmpty(tmpSpec))
                {
                    tmpTableSpec.AddCell(setCell(tmpSpec.ToString(), WhiteBaseColor, fnFont, pPadding, 1, pHAlign, pVAlign, pBorderVal));
                    tmpPrintLines += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                }
                else
                {
                    tmpTableSpec.AddCell(setCell(" ", WhiteBaseColor, fnFont, pPadding, 1, pHAlign, pVAlign, pBorderVal));
                    tmpPrintLines += 1;
                }
            }
            else if (pTemplateID == 2)
            {
                //tmpSpec = lstSpec[0].MaterialSpec.ToString();
                //tmpSpec = System.Text.RegularExpressions.Regex.Replace(tmpSpec, "(style=.+?\")", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                tmpSpec = "<div style='font-family:Calibri; font-size:8pt; width:100%;'>" + tmpSpec + "</div>";
                // ===========================================================================================
                if (!String.IsNullOrEmpty(tmpSpec))
                {
                    PdfPCell text2cell = new PdfPCell(new Phrase(""));
                    text2cell.BackgroundColor = BaseColor.WHITE;
                    text2cell.Colspan = 1;
                    text2cell.Padding = 2;
                    text2cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    text2cell.VerticalAlignment = Element.ALIGN_TOP;

                    foreach (iTextSharp.text.IElement elm in HTMLWorker.ParseToList(new StringReader(tmpSpec), objStyle))
                    {
                        text2cell.AddElement(elm);
                        tmpPrintLines += 1;
                    }
                    tmpTableSpec.AddCell(setCell(text2cell, WhiteBaseColor, fnFont, pPadding, 1, pHAlign, pVAlign, pBorderVal));
                }
            }
        }

        specPrintLines = tmpPrintLines;
        tmpTableSpec.TotalWidth = 100f;
        tmpTableSpec.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        return tmpTableSpec;
    }

    public PdfPTable GetSpecification_SalesOrder(string pQuotationNo, Int64 pProductID, Int16 pTemplateID, iTextSharp.text.Font fnFont, Int16 pPadding, Int16 pHAlign, Int16 pVAlign, Int16 pBorderVal, out int specPrintLines)
    {
        int tmpPrintLines = 0;
        PdfPTable tmpTableSpec = new PdfPTable(1);
        int[] tmpColStruct = { 100 };
        tmpTableSpec.SetWidths(tmpColStruct);
        tmpTableSpec.HorizontalAlignment = Element.ALIGN_LEFT;
        // -----------------------------------------------------------------
        StyleSheet objStyle = new StyleSheet();
        objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
        objStyle.LoadTagStyle("body", "font-size", "1pt");
        objStyle.LoadTagStyle("body", "color", "black");
        objStyle.LoadTagStyle("body", "position", "relative");
        objStyle.LoadTagStyle("body", "margin", "0 auto");

        // -----------------------------------------------------------------
        string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

        List<Entity.ProductDetailCard> lstSpec = new List<Entity.ProductDetailCard>();
        lstSpec = BAL.ProductMgmt.GetSalesOrderProductSpecList(pQuotationNo, pProductID, HttpContext.Current.Session["LoginUserID"].ToString());

        List<Entity.Products> lstProdSpec = new List<Entity.Products>();
        lstProdSpec = BAL.ProductMgmt.GetSOQuotProdSpecList(pQuotationNo, "", pProductID, HttpContext.Current.Session["LoginUserID"].ToString());


        if (lstSpec.Count > 0 || lstProdSpec.Count > 0)
        {
            string tmpSpec = "";
            if (tmpSerialKey == "SI08-SB94-MY45-RY15" || tmpSerialKey == "STSD-54I3-WMYE-8XB2" || tmpSerialKey == "J63H-F8LX-B4B2-GYVZ" || tmpSerialKey == "TJ7S-06Q2-8R2U-KJWG")  //For Sharvaya,Steelman,Hi-Tech,Gautam we take descriptive quotation specification
            {
                tmpSpec = lstProdSpec.Count > 0 ? lstProdSpec[0].ProductSpecification.ToString() : "";
            }
            else
            {
                tmpSpec = lstSpec.Count > 0 ? lstSpec[0].MaterialSpec.ToString() : "";
            }

            if (pTemplateID == 1)
            {
                //string tmpSpec = lstSpec[0].MaterialSpec.ToString();
                if (!String.IsNullOrEmpty(tmpSpec))
                {
                    tmpTableSpec.AddCell(setCell(tmpSpec.ToString(), WhiteBaseColor, fnFont, pPadding, 1, pHAlign, pVAlign, pBorderVal));
                    tmpPrintLines += System.Text.RegularExpressions.Regex.Split(tmpSpec, @"\r?\n|\r").Length;
                }
                else
                {
                    tmpTableSpec.AddCell(setCell(" ", WhiteBaseColor, fnFont, pPadding, 1, pHAlign, pVAlign, pBorderVal));
                    tmpPrintLines += 1;
                }
            }
            else if (pTemplateID == 2)
            {
                //if (lstSpec.Count > 0)
                //{
                //string tmpSpec = lstSpec[0].MaterialSpec.ToString();
                //tmpSpec = System.Text.RegularExpressions.Regex.Replace(tmpSpec, "(style=.+?\")", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
                tmpSpec = "<div style='font-family:Calibri; font-size:8pt; width:100%;'>" + tmpSpec + "</div>";
                // ===========================================================================================
                if (!String.IsNullOrEmpty(tmpSpec))
                {
                    PdfPCell text2cell = new PdfPCell(new Phrase(""));
                    text2cell.BackgroundColor = BaseColor.WHITE;
                    text2cell.Colspan = 1;
                    text2cell.Padding = 2;
                    text2cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    text2cell.VerticalAlignment = Element.ALIGN_TOP;

                    foreach (iTextSharp.text.IElement elm in HTMLWorker.ParseToList(new StringReader(tmpSpec), objStyle))
                    {
                        text2cell.AddElement(elm);
                        tmpPrintLines += 1;
                    }

                    tmpTableSpec.AddCell(setCell(text2cell, WhiteBaseColor, fnFont, pPadding, 1, pHAlign, pVAlign, pBorderVal));
                }
            }
        }
        specPrintLines = tmpPrintLines;
        tmpTableSpec.TotalWidth = 100f;
        tmpTableSpec.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        return tmpTableSpec;
    }

    
    private static List<T> ConvertDataTableToList<T>(DataTable dt)
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row);
            data.Add(item);
        }
        return data;
    }
    private static T GetItem<T>(DataRow dr)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in dr.Table.Columns)
        {
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                if (pro.Name == column.ColumnName)
                {
                    if (!String.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                        pro.SetValue(obj, dr[column.ColumnName], null);
                }
                else
                    continue;
            }
        }
        return obj;
    }


}