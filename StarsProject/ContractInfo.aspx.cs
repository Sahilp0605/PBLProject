using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;

namespace StarsProject
{
    public partial class ContractInfo : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                totAmount = 0;
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetailInq", dtDetail);
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                BindDropDown();
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
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];

                if (requestTarget.ToLower() == "drpinquiry")
                {
                }
            }
            txtEndDate.Attributes.Add("readonly", "readonly");
        }

        public void OnlyViewControls()
        {
            txtContractCode.ReadOnly = true;
            drpContractType.Attributes.Add("disabled", "disabled");
            txtStartDate.ReadOnly = true;
            txtEndDate.Attributes.Add("readonly", "readonly");
            txtCustomerName.ReadOnly = true;
            drpContactPerson.Attributes.Add("disabled", "disabled");
            drpContact.Attributes.Add("disabled", "disabled");

            txtContractFooter.ReadOnly = true;
            drpTNC.Attributes.Add("disabled", "disabled");

            pnlDetail.Enabled = false;

            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            List<Entity.Contents> lstList2 = new List<Entity.Contents>();
            lstList2 = BAL.CommonMgmt.GetContentList(0, "TNC");
            drpTNC.DataSource = lstList2;
            drpTNC.DataValueField = "pkID";
            drpTNC.DataTextField = "TNC_Header";
            drpTNC.DataBind();
            drpTNC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select T&C --", ""));

            // ---------------- Report To List -------------------------------------
            List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
            lstCustomer = BAL.CustomerMgmt.GetCustomerList();
        }

        public void BindCustomerContacts()
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                try
                {
                    // ---------------- Report To List -------------------------------------
                    drpContactPerson.Items.Clear();
                    drpContact.Items.Clear();
                    int TotalCount = 0;
                    List<Entity.CustomerContacts> lstObject = new List<Entity.CustomerContacts>();
                    lstObject = BAL.CustomerContactsMgmt.GetCustomerContactsList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                    drpContactPerson.DataSource = lstObject;
                    drpContactPerson.DataValueField = "ContactPerson1";
                    drpContactPerson.DataTextField = "ContactPerson1";
                    drpContactPerson.DataBind();

                    drpContact.DataSource = lstObject;
                    drpContact.DataValueField = "ContactNumber1";
                    drpContact.DataTextField = "ContactNumber1";
                    drpContact.DataBind();
                }
                catch (System.Exception ex)
                {

                }
                drpContactPerson.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Contact Person --", ""));
                drpContact.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Contact Number --", ""));
            }
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        protected void custSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            Entity.Customer objEntity = new Entity.Customer();

            objEntity.CustomerID = Convert.ToInt64(0);
            // -------------------------------------------------------------- Insert/Update Record
            BAL.CustomerMgmt.AddUpdateCustomerInstant(objEntity, out ReturnCode, out ReturnMsg);
            // --------------------------------------------------------------
            BindDropDown();
            // -------------------------------------------------------------------------
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:openFormContainer();", true);
        }

        public void BindContractInfoProduct(string pInquiryNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.ContractInfoMgmt.GetContractInfoProductDetail(pInquiryNo);
            rptContractInfoProductGroup.DataSource = dtDetail1;
            rptContractInfoProductGroup.DataBind();
            Session.Add("dtDetailInq", dtDetail1);
        }

        protected void rptContractInfoProductGroup_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    HiddenField hdnProductID = (HiddenField)e.Item.FindControl("hdnProductID");
                    TextBox txtProductName = (TextBox)e.Item.FindControl("txtProductName");
                    TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                    TextBox txtUnitPrice = (TextBox)e.Item.FindControl("txtUnitPrice");

                    if (String.IsNullOrEmpty(hdnProductID.Value) || String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0)
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnProductID.Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0)
                            strErr += "<li>" + "Quantity is required." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetailInq"];

                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + hdnProductID.Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                            return;
                        }


                        Int64 cntRow = dtDetail.Rows.Count + 1;

                        DataRow dr = dtDetail.NewRow();

                        dr["pkID"] = cntRow;
                        string icode = hdnProductID.Value;
                        string iname = txtProductName.Text;
                        string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                        string unitprice = ((TextBox)e.Item.FindControl("txtUnitPrice")).Text;

                        dr["InquiryNo"] = txtContractCode.Text;
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["UnitPrice"] = (!String.IsNullOrEmpty(unitprice)) ? Convert.ToDecimal(unitprice) : 0;
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetailInq", dtDetail);
                        // ---------------------------------------------------------------
                        rptContractInfoProductGroup.DataSource = dtDetail;
                        rptContractInfoProductGroup.DataBind();
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
                    dtDetail = (DataTable)Session["dtDetailInq"];

                    DataRow[] rows;
                    rows = dtDetail.Select("pkID=" + e.CommandArgument.ToString());
                    foreach (DataRow r in rows)
                        r.Delete();

                    rptContractInfoProductGroup.DataSource = dtDetail;
                    rptContractInfoProductGroup.DataBind();

                    Session.Add("dtDetailInq", dtDetail);
                }
            }
        }

        protected void rptContractInfoProductGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Quantity"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "UnitPrice"));

                totAmount += Convert.ToDecimal(v1 * v2);
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotalAmount = (Label)e.Item.FindControl("lblTotalAmount");
                lblTotalAmount.Text = totAmount.ToString("0.00");
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.ContractInfo> lstEntity = new List<Entity.ContractInfo>();
                // ----------------------------------------------------
                lstEntity = BAL.ContractInfoMgmt.GetContractInfoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                hdnEmployeeName.Value = lstEntity[0].EmployeeName;
                txtContractCode.Text = lstEntity[0].InquiryNo;
                drpContractType.SelectedValue = lstEntity[0].ContractType;
                txtStartDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].StartDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                txtEndDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].EndDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                txtCustomerName_TextChanged(null, null);
                drpContactPerson.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].ContactPerson)) ? lstEntity[0].ContactPerson : "";
                drpContact.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].ContactNumber)) ? lstEntity[0].ContactNumber : "";

                txtContractFooter.Text = lstEntity[0].ContractFooter;
                drpTNC.SelectedValue = (!String.IsNullOrEmpty(lstEntity[0].ContractTNC)) ? lstEntity[0].ContractTNC : "";
                // -------------------------------------------------------------------------
                BindContractInfoProduct(txtContractCode.Text);


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
            Session.Remove("dtDetailInq");
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetailInq"];

            int ReturnCode = 0;
            string ReturnMsg = "";
            string ReturnContractCode = "";
            string strErr = "";
            //--------------------------------------------------------------
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtStartDate.Text)) || ( hdnCustomerID.Value == "0"))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Customer Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtStartDate.Text))
                    strErr += "<li>" + "Start Date is required." + "</li>";
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.ContractInfo objEntity = new Entity.ContractInfo();

                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.InquiryNo = txtContractCode.Text;
                        objEntity.ContractType = drpContractType.SelectedValue;
                        objEntity.StartDate = (!String.IsNullOrEmpty(txtStartDate.Text)) ? Convert.ToDateTime(txtStartDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.EndDate = (!String.IsNullOrEmpty(txtEndDate.Text)) ? Convert.ToDateTime(txtEndDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.ContactPerson = drpContactPerson.SelectedValue;
                        objEntity.ContactNumber = drpContact.SelectedValue;
                        objEntity.ContractFooter = txtContractFooter.Text;
                        objEntity.ContractTNC = drpTNC.Text;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.ContractInfoMgmt.AddUpdateContractInfo(objEntity, out ReturnCode, out ReturnMsg, out ReturnContractCode);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnContractCode) && !String.IsNullOrEmpty(txtContractCode.Text))
                        {
                            ReturnContractCode = txtContractCode.Text;
                        }
                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        int ReturnCode1;
                        String ReturnMsg1;

                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnContractCode))
                        {

                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                            // --------------------------------------------------------------
                            BAL.ContractInfoMgmt.DeleteContractInfoProductByInquiryNo(ReturnContractCode, out ReturnCode1, out ReturnMsg1);

                            // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                            Entity.ContractInfo objEntity1 = new Entity.ContractInfo();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    objEntity1.ProductID = Convert.ToInt64(dr["ProductID"]);
                                    objEntity1.InquiryNo = ReturnContractCode;
                                    objEntity1.Quantity = (!String.IsNullOrEmpty(dr["Quantity"].ToString())) ? Convert.ToDecimal(dr["Quantity"].ToString()) : 0;
                                    objEntity1.UnitPrice = (!String.IsNullOrEmpty(dr["UnitPrice"].ToString())) ? Convert.ToDecimal(dr["UnitPrice"].ToString()) : 0;
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.ContractInfoMgmt.AddUpdateContractInfoProduct(objEntity1, out ReturnCode, out ReturnMsg);
                                }
                            }
                            if (ReturnCode > 0)
                            {
                                Session.Remove("dtDetailInq");

                            }
                        }
                        // --------------------------------------------------------------
                        if (paraSaveAndEmail)
                        {
                            Entity.Authenticate objAuth = new Entity.Authenticate();
                            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                            String sendEmailFlag = BAL.CommonMgmt.GetConstant("INQ-EMAIL", 0, objAuth.CompanyID).ToLower();
                            if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                            {
                                try
                                {
                                    if (String.IsNullOrEmpty(hdnCustEmailAddress.Value) && objEntity.CustomerID > 0)
                                    {
                                        hdnCustEmailAddress.Value = BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID);
                                    }
                                    // -------------------------------------------------------
                                    if (!String.IsNullOrEmpty(hdnCustEmailAddress.Value) && hdnCustEmailAddress.Value.ToUpper() != "NULL")
                                    {
                                        String respVal = "";
                                        respVal = BAL.CommonMgmt.SendEmailNotifcation("INQUIRY-WELCOME", Session["LoginUserID"].ToString(), ((!String.IsNullOrEmpty(hdnpkID.Value)) ? Convert.ToInt64(hdnpkID.Value) : 0), hdnCustEmailAddress.Value);
                                    }
                                    strErr += "<li>" + "Email Notification Sent Successfully !" + "</li>";
                                }
                                catch (Exception ex)
                                {
                                    strErr += "<li>" + "Email Notification Failed !" + "</li>";
                                }
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

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnCustomerID.Value = "";
            hdnOrgCodeEmp.Value = "";
            txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtEndDate.Text = "";
            txtContractCode.Text = "";
            drpContractType.SelectedValue = "-- Select --";
            txtCustomerName.Text = "";
            drpContactPerson.SelectedValue = "";
            drpContact.SelectedValue = "";
            hdnCustStateID.Value = "";
            //drpCustomer.SelectedValue = "";
            // ---------------------------------------------
            BindContractInfoProduct("");
            txtCustomerName.Focus();
            txtContractFooter.Text = "";
            drpTNC.SelectedValue = "";

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
        }

        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalrecord;

            //Control rptFootCtrl = rptContractInfoProductGroup.Controls[rptContractInfoProductGroup.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            //Label lblUnitPrice = ((Label)rptFootCtrl.FindControl("lblUnitPrice"));
            //Label lblTaxRate = ((Label)rptFootCtrl.FindControl("lblTaxRate"));
            //TextBox txtQty = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            //if (!String.IsNullOrEmpty(ctrl1))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //lblUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
            // -----------------------------------------
            //if (!String.IsNullOrEmpty(drpProduct.SelectedValue))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(drpProduct.SelectedValue), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //if (!String.IsNullOrEmpty(hdnProductID.Value))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);
            //txtUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";            

            //txtQuantity.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteContractInfo(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- 
            BAL.ContractInfoMgmt.DeleteContractInfo(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterCustomer(string pCustName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row = new Dictionary<string, object>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetCustomerList(pCustName).Select(sel => new { sel.CustomerName, sel.CustomerID });
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string GetContractInfoNoPrimaryID(string pInqNo)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            //Dictionary<string, object> row = new Dictionary<string, object>();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CommonMgmt.GetContractInfoNoPrimaryID(pInqNo);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProduct(string pProductName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductList(pProductName).Select(sel => new { sel.ProductNameLong, sel.pkID });
            return serializer.Serialize(rows);
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptContractInfoProductGroup.Controls[rptContractInfoProductGroup.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            TextBox txtUnitPrice = ((TextBox)rptFootCtrl.FindControl("txtUnitPrice"));
            //Label lblTaxRate = ((Label)rptFootCtrl.FindControl("lblTaxRate"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            //if (!String.IsNullOrEmpty(ctrl1))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //lblUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
            // -----------------------------------------
            //if (!String.IsNullOrEmpty(drpProduct.SelectedValue))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(drpProduct.SelectedValue), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            if (!String.IsNullOrEmpty(hdnProductID.Value))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txtUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";          

            txtUnitPrice.Focus();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;
            List<Entity.Customer> lstEntity = new List<Entity.Customer>();

            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                lstEntity = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && String.IsNullOrEmpty(txtCustomerName.Text))
                txtCustomerName.Text = (lstEntity.Count > 0) ? lstEntity[0].CustomerName : "";

            hdnCustStateID.Value = (lstEntity.Count > 0) ? lstEntity[0].StateCode : "0";
            DataTable dtDetail = new DataTable();
            dtDetail = BAL.CommonMgmt.funOnChangeTermination((DataTable)Session["dtDetailInq"], hdnCustStateID.Value, Session["CompanyStateCode"].ToString());
            rptContractInfoProductGroup.DataSource = dtDetail;
            rptContractInfoProductGroup.DataBind();

            Session.Add("dtDetailInq", dtDetail);
            ////////////////////////////////////////////////////////
            // ---------------------------------------------
            BindCustomerContacts();
        }

        protected void drpTNC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpTNC.SelectedValue) && drpTNC.SelectedValue != "0")
            {
                string tmpval = drpTNC.SelectedValue;
                List<Entity.Contents> lstList2 = new List<Entity.Contents>();
                lstList2 = BAL.CommonMgmt.GetContentList(Convert.ToInt64(drpTNC.SelectedValue), "TNC");
                if (lstList2.Count > 0)
                    txtContractFooter.Text = lstList2[0].TNC_Content;
            }
            else if (String.IsNullOrEmpty(drpTNC.SelectedValue))
            {
                txtContractFooter.Text = "";
            }
        }
    }
}