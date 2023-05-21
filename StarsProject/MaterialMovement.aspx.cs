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
    public partial class MaterialMovement : System.Web.UI.Page
    {
        public string loginuserid;
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove("dtDetail");

                totAmount = 0;
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetail", dtDetail);
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                BindDropDown();
                BindMaterialProductDetail(0);
                hdnLocationStock.Value = BAL.CommonMgmt.GetConstant("LocationWiseStock", 0, 1).ToLower();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["transtype"]))
                {
                    hdnTransType.Value = Request.QueryString["transtype"].ToString();
                }
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
                    hdnShowCustSO.Value = BAL.CommonMgmt.GetConstant("MaterialMovementWithCustomer", 0).ToLower();
                }
            }
        }

        public void OnlyViewControls()
        {
            txtMovementCode.ReadOnly = true;
            txtTranDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpOrder.Attributes.Add("disabled", "disabled");
            drpEmployee.Attributes.Add("disabled", "disabled");
            drpLocation.Attributes.Add("disabled", "disabled");

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

            //---------------------------Location Details-------------------------------
            List<Entity.PurchaseBill> lstLocation = new List<Entity.PurchaseBill>();
            lstLocation = BAL.CommonMgmt.GetLocationList();
            drpLocation.DataSource = lstLocation;
            drpLocation.DataValueField = "LocationID";
            drpLocation.DataTextField = "LocationName";
            drpLocation.DataBind();
            drpLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

        }
        public void BindMaterialProductDetail(Int64 pInquiryNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.Material_MovementMgmt.GetMaterialProductDetail(pInquiryNo);
            rptProductDetail.DataSource = dtDetail1;
            rptProductDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            Control edSender = (Control)sender;
            var item = (RepeaterItem)edSender.NamingContainer;
            HiddenField edProductID = (HiddenField)item.FindControl("hdnProductID_Grid");
            TextBox edQuantity = (TextBox)item.FindControl("tdQuantity");
            
            // --------------------------------------------------------------------------
            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            // --------------------------------------------------------------------------

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            if (dtDetail != null)
            {
                foreach (System.Data.DataColumn col in dtDetail.Columns)
                {
                    col.AllowDBNull = true;
                    col.ReadOnly = false;
                }

                foreach (DataRow row in dtDetail.Rows)
                {
                    if (row["pkID"].ToString() == edProductID.Value )
                    {
                        if (!String.IsNullOrEmpty(edQuantity.Text))
                            row.SetField("Quantity", edQuantity.Text);
                    }
                }
                rptProductDetail.DataSource = dtDetail;
                rptProductDetail.DataBind();
            }
            Session.Add("dtDetail", dtDetail);
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Material_Movement> lstEntity = new List<Entity.Material_Movement>();
                // ----------------------------------------------------
                lstEntity = BAL.Material_MovementMgmt.GetMaterial_Movement(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), hdnTransType.Value, 1, 1000, out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtMovementCode.Text = lstEntity[0].pkID.ToString();
                txtTranDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].TransDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpLocation.SelectedValue = lstEntity[0].LocationID.ToString();
                txtCustomerName_TextChanged(null, null);
                drpOrder.SelectedValue = lstEntity[0].OrderNo;
                hdnEmployeeName.Value = lstEntity[0].EmployeeID.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();

                // -------------------------------------------------------------------------
                BindMaterialProductDetail(Convert.ToInt64(txtMovementCode.Text));
            }
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnCustomerID.Value = "";
            txtMovementCode.Text = "0";
            txtTranDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";
            drpOrder.SelectedValue = "";
            drpEmployee.SelectedValue = "0";
            // ---------------------------------------------
            BindMaterialProductDetail(0);
            txtTranDate.Focus();
            
            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
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

                    HiddenField hdnProductID = (HiddenField)e.Item.FindControl("hdnProductID");
                    TextBox txtProductName = (TextBox)e.Item.FindControl("txtProductName");
                    TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                    TextBox txtClosingSTK = ((TextBox)e.Item.FindControl("txtClosingSTK"));
                    if (String.IsNullOrEmpty(hdnProductID.Value) || String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0)
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnProductID.Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0)
                            strErr += "<li>" + "Quantity is required." + "</li>";
                    }
                    //if ((!String.IsNullOrEmpty(txtQuantity.Text) && txtQuantity.Text != "0") && (!String.IsNullOrEmpty(txtClosingSTK.Text) || txtClosingSTK.Text != "0"))
                    //{
                        //if (Convert.ToDecimal(txtQuantity.Text) > Convert.ToDecimal(txtClosingSTK.Text))
                        //{
                           // _pageValid = false;
                           // strErr += "<li>" + "Issue Quantity is higher then Closing Stock !" + "</li>";
                       // }
                    //}
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


                        Int64 cntRow = (dtDetail != null && dtDetail.Rows.Count > 0) ? dtDetail.AsEnumerable().Max(r => r.Field<Int64>("pkID")) + 1 : 1;

                        DataRow dr = dtDetail.NewRow();

                        
                        string icode = hdnProductID.Value;
                        string iname = txtProductName.Text;
                        string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;

                        dr["pkID"] = cntRow;
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetail", dtDetail);
                        // ---------------------------------------------------------------
                        rptProductDetail.DataSource = dtDetail;
                        rptProductDetail.DataBind();
                    }
                    btnSave.Focus();
                }
                if (!string.IsNullOrEmpty(strErr))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
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
            }
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                drpOrder.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    lstOrder = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), "", 0, 0);
                    if (lstOrder.Count > 0)
                    {
                        drpOrder.DataSource = lstOrder;
                        drpOrder.DataValueField = "OrderNo";
                        drpOrder.DataTextField = "OrderNo";
                        drpOrder.DataBind();
                        drpOrder.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                    }
                }
            }
            else
            {
                string strErr = "";
                strErr += "<li>" + "Select Proper Customer Name From List." + "</li>";
            }
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;
            Control rptFootCtrl = rptProductDetail.Controls[rptProductDetail.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtClosingSTK = ((TextBox)rptFootCtrl.FindControl("txtClosingSTK"));
            if (String.IsNullOrEmpty(hdnProductID.Value) && hdnProductID.Value == "0")
            {
                string strErr = "";
                strErr += "<li>" + "Select Proper Product Name From List." + "</li>";
            }
            else
            {
                List<Entity.Products> lstEntity = new List<Entity.Products>();
                if (!String.IsNullOrEmpty(hdnProductID.Value))
                    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                //txtClosingSTK.Text = (lstEntity.Count > 0) ? lstEntity[0].ClosingSTK.ToString() : "";


            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            //Session.Remove("dtDetail");
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

            if (hdnShowCustSO.Value == "yes")
            {
                if ((String.IsNullOrEmpty(txtTranDate.Text)) || (String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0"))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(txtTranDate.Text))
                        strErr += "<li>" + "Transaction Date is required." + "</li>";
                    if (String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                        strErr += "<li>" + "Select Proper Customer From List." + "</li>";
                }
            }
            else
            {
                if (String.IsNullOrEmpty(txtTranDate.Text))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(txtTranDate.Text))
                        strErr += "<li>" + "Transaction Date is required." + "</li>";
                }
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtTranDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtTranDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Transaction Date is Not Valid." + "</li>";
                }
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.Material_Movement objEntity = new Entity.Material_Movement();

                if (dtDetail != null)
                {
                    if (rptProductDetail.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        Int64 intLocation = (!String.IsNullOrEmpty(drpLocation.SelectedValue)) ? Convert.ToInt64(drpLocation.SelectedValue) : 0;
                        objEntity.TransDate = (!String.IsNullOrEmpty(txtTranDate.Text)) ? Convert.ToDateTime(txtTranDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.TransType = hdnTransType.Value.ToUpper();
                        objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                        objEntity.LocationID = intLocation;
                        objEntity.OrderNo = (!String.IsNullOrEmpty(drpOrder.SelectedValue)) ? drpOrder.SelectedValue : "";
                        objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.Material_MovementMgmt.AddUpdateMaterial_Movement(objEntity, out ReturnCode, out ReturnMsg, out ReturnpkID);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        int ReturnCode1;
                        String ReturnMsg1;

                        if (ReturnCode > 0 && ReturnpkID>0)
                        {

                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                            // --------------------------------------------------------------
                            if (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0")
                                BAL.Material_MovementMgmt.DeleteMaterial_MovementDetailByNo(Convert.ToInt64(ReturnpkID), out ReturnCode1, out ReturnMsg1);

                            // --------------------------------------------------------------
                            // >>>>>>>> Second Insert all Selectd ProductGroup entry into table

                            Entity.Material_MovementDetail objEntity1 = new Entity.Material_MovementDetail();
                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    objEntity1.ParentID = ReturnpkID;
                                    objEntity1.ProductID = Convert.ToInt64(dr["ProductID"]);
                                    objEntity1.LocationID = intLocation;
                                    objEntity1.Quantity = (!String.IsNullOrEmpty(dr["Quantity"].ToString())) ? Convert.ToDecimal(dr["Quantity"].ToString()) : 0;
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.Material_MovementMgmt.AddUpdateMaterial_MovementDetail(objEntity1, out ReturnCode, out ReturnMsg);
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
            BAL.Material_MovementMgmt.DeleteMaterial_Movement(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }




    }
}