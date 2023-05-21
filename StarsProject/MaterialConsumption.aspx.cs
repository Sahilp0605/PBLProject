using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class MaterialConsuption : System.Web.UI.Page
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
        
    }

        public void OnlyViewControls()
        {
            txtCDate.ReadOnly = true;
            txtConsCode.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpOrderNo.Attributes.Add("disabled", "disabled");
            drpEmployee.Attributes.Add("disabled", "disabled");
            pnlDetail.Enabled = false;
            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Material_Cons> lstEntity = new List<Entity.Material_Cons>();
                // ----------------------------------------------------
                lstEntity = BAL.Material_ConsMgmt.GetMaterial_Cons(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtConsCode.Text = lstEntity[0].pkID.ToString();
                hdnEmployeeName.Value = lstEntity[0].EmployeeID.ToString();
                txtCDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].ConsDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                txtCustomerName_TextChanged(null, null);
                drpOrderNo.SelectedValue = lstEntity[0].OrderNo;
                hdnEmployeeName.Value = lstEntity[0].EmployeeID.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();


                BindMaterialProductDetail(Convert.ToInt64(txtConsCode.Text));


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetailInq"];

            int ReturnCode = 0, ReturnpkID = 0;
            string ReturnMsg = "";
            string strErr = "";
            //--------------------------------------------------------------
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtCustomerName.Text)) )
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Name is required." + "</li>";

            }
           
            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.Material_Cons objEntity = new Entity.Material_Cons();

                if (dtDetail != null)
                {
                    if (rptContractInfoProductGroup.Items.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        
                        objEntity.ConsDate = (!String.IsNullOrEmpty(txtCDate.Text)) ? Convert.ToDateTime(txtCDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.OrderNo = drpOrderNo.SelectedValue;
                        objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                          
                      
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.Material_ConsMgmt.AddUpdateMaterial_Cons(objEntity, out ReturnCode, out ReturnMsg, out ReturnpkID);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        // --------------------------------------------------------------
                        
                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        int ReturnCode1;
                        String ReturnMsg1;
                    

                        if (ReturnCode > 0 && ReturnpkID > 0)
                        {

                            Int64 pkID = 0;

                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;
                           
                            // --------------------------------------------------------------
                            BAL.Material_ConsMgmt.DeleteMaterial_ConsDetailByNo(Convert.ToInt64(ReturnpkID), out ReturnCode, out ReturnMsg);

                            // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                            Entity.Material_ConsDetail objEntity1 = new Entity.Material_ConsDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    objEntity1.ParentID = ReturnpkID;
                                    objEntity1.ProductID = Convert.ToInt64(dr["ProductID"]);
                                    objEntity1.Quantity = (!String.IsNullOrEmpty(dr["Quantity"].ToString())) ? Convert.ToDecimal(dr["Quantity"].ToString()) : 0;
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.Material_ConsMgmt.AddUpdateMaterial_ConsDetail(objEntity1, out ReturnCode, out ReturnMsg);
                                }
                            }
                            if (ReturnCode > 0)
                            {
                                Session.Remove("dtDetailInq");

                            }
                        }
                        // --------------------------------------------------------------
                        
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


        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetailInq");
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtConsCode.Text = "0";
            txtCDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            drpEmployee.SelectedValue = "";
            drpOrderNo.SelectedValue = "";
            // ------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            if (drpEmployee.Items.FindByText(objAuth.EmployeeName) != null)
            {
                drpEmployee.Items.FindByText(objAuth.EmployeeName).Selected = true;
            }

            //drpCustomer.SelectedValue = "";
            // ---------------------------------------------
            BindMaterialProductDetail(0);
            

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
        }

        //public void BindContractInfoProduct(string pInquiryNo)
        //{
        //    DataTable dtDetail1 = new DataTable();
        //    dtDetail1 = BAL.ContractInfoMgmt.GetContractInfoProductDetail(pInquiryNo);
        //    rptContractInfoProductGroup.DataSource = dtDetail1;
        //    rptContractInfoProductGroup.DataBind();
        //    Session.Add("dtDetailInq", dtDetail1);
        //}
        public void BindMaterialProductDetail(Int64 pInquiryNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.Material_ConsMgmt.GetMaterialProductDetail(pInquiryNo);
            rptContractInfoProductGroup.DataSource = dtDetail1;
            rptContractInfoProductGroup.DataBind();
            Session.Add("dtDetailInq", dtDetail1);
        }

        public void BindDropDown()
        {
           // ---------------- Report To List -------------------------------------
            List<Entity.OrganizationEmployee> lstEmp = new List<Entity.OrganizationEmployee>();
            lstEmp = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstEmp;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Select Order --", "0"));
        }
        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }
        
        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptContractInfoProductGroup.Controls[rptContractInfoProductGroup.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));


           
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            //int totalrecord;
            List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();

            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                drpOrderNo.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    lstOrder = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), "", 0, 0);
                    if (lstOrder.Count > 0)
                    {
                        drpOrderNo.DataSource = lstOrder;
                        drpOrderNo.DataValueField = "OrderNo";
                        drpOrderNo.DataTextField = "OrderNo";
                        drpOrderNo.DataBind();
                        drpOrderNo.Items.Insert(0, new ListItem("-- Select Order --", ""));
                    }
                }
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
            BAL.Material_ConsMgmt.DeleteMaterial_Cons(pkID, out ReturnCode, out ReturnMsg);
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
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetCustomerList(pCustName).Select(sel => new { sel.CustomerName, sel.CustomerID });
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

                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
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

           


        }

       protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        
    }
}