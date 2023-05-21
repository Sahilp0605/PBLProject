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
using System.Data.SqlTypes;

namespace StarsProject
{
    public partial class InternalWorkOrder : System.Web.UI.Page
    {
        public string loginuserid;
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //DataTable dtDetail = new DataTable();
                //Session.Add("dtDetail", dtDetail);
                //Session["PageNo"] = 1;
                //Session["PageSize"] = 10;
                // --------------------------------------------------------
                BindDropDown();
                BindInternalWorkOrderDetailList("");
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
        public void BindDropDown()
        {

        }

        public void BindInternalWorkOrderDetailList(string WorkOrderNo)
        {
            int TotRec;

            DataTable dtDetail = new DataTable();
            List<Entity.InternalWorkOrderDetail> lstWork = new List<Entity.InternalWorkOrderDetail>();
            lstWork = BAL.InternalWorkOrderMgmt.GetIternalWorkOrderDetailListByWorkOrderNo(txtWorkOrderNo.Text, 1, 10000, out TotRec);
            rptProductDetail.DataSource = lstWork;
            rptProductDetail.DataBind();
            dtDetail = PageBase.ConvertListToDataTable(lstWork);
            Session.Add("dtDetail", dtDetail);
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                drpSalesOrder.Items.Clear();
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    lstOrder = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), "", 0, 0);
                    if (lstOrder.Count > 0)
                    {
                        drpSalesOrder.DataSource = lstOrder;
                        drpSalesOrder.DataValueField = "OrderNo";
                        drpSalesOrder.DataTextField = "OrderNo";
                        drpSalesOrder.DataBind();
                        drpSalesOrder.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));
                    }
                }
            }
            else
            {
                string strErr = "";
                strErr += "<li>" + "Select Proper Customer Name From List." + "</li>";
            }
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnCustomerID.Value = "";
            txtWorkOrderNo.Text = "";
            txtWorkOrderDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomerName.Text = "";
            txtRefNo.Text = "";
            txtRefDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            drpSalesOrder.SelectedValue = "0";
            txtQuotationNo.Text= "";
            txtInquiryNo.Text = "";
            // ---------------------------------------------
            BindInternalWorkOrderDetailList("");
            txtWorkOrderDate.Focus();

            btnSave.Disabled = false;
            //btnSaveEmail.Disabled = false;
        }
        public void OnlyViewControls()
        {
            txtWorkOrderNo.ReadOnly = true;
            txtWorkOrderDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpSalesOrder.Attributes.Add("disabled", "disabled");
            txtInquiryNo.ReadOnly = true;
            txtQuotationNo.ReadOnly = true;
            //drpLocation.Attributes.Add("disabled", "disabled");
            txtRefNo.ReadOnly = true;
            txtRefDate.ReadOnly = true;
            //pnlDetail.Enabled = false;
            btnSave.Visible = false;
            //btnSaveEmail.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.InternalWorkOrder> lstEntity = new List<Entity.InternalWorkOrder>();
                // ----------------------------------------------------
                lstEntity = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtWorkOrderNo.Text = lstEntity[0].WorkOrderNo.ToString();
                txtWorkOrderDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].WorkOrderDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpSalesOrder.SelectedValue = lstEntity[0].SalesOrderNo.ToString();
                txtQuotationNo.Text = lstEntity[0].QuotationNo.ToString();
                txtInquiryNo.Text = lstEntity[0].InquiryNo.ToString();
                txtRefNo.Text = lstEntity[0].ReferenceNo;
                txtRefDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].ReferenceDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");

                //txtCustomerName_TextChanged(null, null);

                // -------------------------------------------------------------------------
                BindInternalWorkOrderDetailList(txtWorkOrderNo.Text);
                //drpOrder_SelectedIndexChanged(null, null);

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            int ReturnCode = 0, ReturnpkID = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnWorkOrderNo = "", ReturnMsg1 = "";
            string strErr = "";
            //--------------------------------------------------------------
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtWorkOrderDate.Text)) || (String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0"))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtWorkOrderDate.Text))
                    strErr += "<li>" + "Work Order Date is required." + "</li>";
                if (String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    strErr += "<li>" + "Select Proper Customer From List." + "</li>";
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.InternalWorkOrder objEntity = new Entity.InternalWorkOrder();

                if (dtDetail != null)
                {
                    if (rptProductDetail.Items.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.WorkOrderNo = txtWorkOrderNo.Text;
                        objEntity.WorkOrderDate = (!String.IsNullOrEmpty(txtWorkOrderDate.Text)) ? Convert.ToDateTime(txtWorkOrderDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.CustomerID = (!String.IsNullOrEmpty(hdnCustomerID.Value)) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                        objEntity.SalesOrderNo = (!String.IsNullOrEmpty(drpSalesOrder.SelectedValue)) ? drpSalesOrder.SelectedValue : "";
                        objEntity.QuotationNo = (!String.IsNullOrEmpty(txtQuotationNo.Text)) ? txtQuotationNo.Text : "";
                        objEntity.InquiryNo = (!String.IsNullOrEmpty(txtInquiryNo.Text)) ? txtInquiryNo.Text : "";
                        objEntity.ReferenceNo = (!String.IsNullOrEmpty(txtRefNo.Text)) ? txtRefNo.Text : "";
                        objEntity.ReferenceDate = (!String.IsNullOrEmpty(txtRefDate.Text)) ? Convert.ToDateTime(txtRefDate.Text) : SqlDateTime.MinValue.Value;

                        objEntity.LogInUserID = Session["LoginUserID"].ToString();

                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.InternalWorkOrderMgmt.AddUpdateInternalWorkOrder(objEntity, out ReturnCode, out ReturnMsg, out ReturnWorkOrderNo);
                        if (String.IsNullOrEmpty(ReturnWorkOrderNo) && !String.IsNullOrEmpty(txtWorkOrderNo.Text))
                        {
                            ReturnWorkOrderNo = txtWorkOrderNo.Text;
                        }
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnWorkOrderNo + " " + ReturnMsg : ReturnMsg) + "</li>";

                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table

                        BAL.InternalWorkOrderMgmt.DeleteInternalWorkOrderDetailByWorkOrderNo(ReturnWorkOrderNo, out ReturnCode, out ReturnMsg);
                        if (ReturnCode > 0)
                        {
                            Entity.InternalWorkOrderDetail objQuotDet = new Entity.InternalWorkOrderDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.WorkOrderNo = ReturnWorkOrderNo.Trim();
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.DeliveryDate = Convert.ToDateTime(dr["DeliveryDate"]);
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                if (Session["mySpecs"] != null)
                                {
                                    Boolean itemAdded = false;
                                    DataTable mySpecs = new DataTable();
                                    mySpecs = (DataTable)Session["mySpecs"];
                                    foreach (DataRow row in mySpecs.Rows)
                                    {
                                        if (row["pkID"].ToString() == dr["ProductID"].ToString())
                                        {
                                            objQuotDet.ScopeOfWork = row["ScopeOfWork"].ToString();
                                            objQuotDet.ScopeOfWork_Client = row["ScopeOfWork_Client"].ToString();
                                            objQuotDet.Remarks = row["Remarks"].ToString();

                                            itemAdded = true;
                                        }
                                    }

                                    if (!itemAdded)
                                    {
                                        objQuotDet.ScopeOfWork = dr["ScopeOfWork"].ToString();
                                        objQuotDet.ScopeOfWork_Client = dr["ScopeOfWork_Client"].ToString();
                                        objQuotDet.Remarks = dr["Remarks"].ToString();
                                    }
                                }
                                else
                                {
                                    objQuotDet.ScopeOfWork = dr["ScopeOfWork"].ToString();
                                    objQuotDet.ScopeOfWork_Client = dr["ScopeOfWork_Client"].ToString();
                                    objQuotDet.Remarks = dr["Remarks"].ToString();
                                }

                                BAL.InternalWorkOrderMgmt.AddUpdateInternalWorkOrderDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                                Session.Remove("mySpecs");
                            }
                            btnSave.Disabled = true;
                        }
                    }
                    else
                    {
                        strErr = "<li>" + "Atleast One Item is required to save Inward Information !" + "</li>";
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
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            //Session.Remove("dtDetail");
        }

        [System.Web.Services.WebMethod]
        public static string DeleteInternalWorkOrder(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.InternalWorkOrderMgmt.DeleteInternalWorkOrder(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void rptProductDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

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


                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) || String.IsNullOrEmpty(((HiddenField)e.Item.FindControl("hdnProductID")).Value))
                            strErr += "<li>" + "Select Proper Product From List." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        if (dtDetail != null)
                        {
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
                            string deldate = ((TextBox)e.Item.FindControl("txtDelDate")).Text;

                            DataTable mySpecs = new DataTable();
                            List<Entity.InternalWorkOrderDetail> ProdSpec = new List<Entity.InternalWorkOrderDetail>();

                            ProdSpec = BAL.InternalWorkOrderMgmt.GetInternalWorkOrderProdSpecList("", Convert.ToInt64(icode), Session["LoginUserID"].ToString());
                            if (Session["mySpecs"] != null)
                            {
                                mySpecs = (DataTable)Session["mySpecs"];

                                DataRow drTemp = mySpecs.NewRow();
                                drTemp["pkID"] = Convert.ToInt64(icode);
                                drTemp["ScopeOfWork"] = (ProdSpec.Count > 0) ? ProdSpec[0].ScopeOfWork : "";
                                drTemp["ScopeOfWork_Client"] = (ProdSpec.Count > 0) ? ProdSpec[0].ScopeOfWork_Client : "";
                                drTemp["Remarks"] = (ProdSpec.Count > 0) ? ProdSpec[0].Remarks : "";
                                mySpecs.Rows.Add(drTemp);
                            }
                            else
                                mySpecs = PageBase.ConvertListToDataTable(ProdSpec);


                            mySpecs.AcceptChanges();
                            Session.Add("mySpecs", mySpecs);

                            dr["WorkOrderNo"] = txtWorkOrderNo.Text;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["DeliveryDate"] = (!String.IsNullOrEmpty(deldate)) ? Convert.ToDateTime(deldate) : SqlDateTime.MinValue.Value;

                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptProductDetail.DataSource = dtDetail;
                            rptProductDetail.DataBind();
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

                rptProductDetail.DataSource = dtDetail;
                rptProductDetail.DataBind();

                Session.Add("dtDetail", dtDetail);
            }
        }

        protected void imgBtnSave_Click(object sender, ImageClickEventArgs e)
        {
            //var requestTarget = this.Request["__EVENTTARGET"];
            //string strErr = "";


            //        if (String.IsNullOrEmpty(txtProductName.Text))
            //        {
            //            _pageValid = false;

            //            if (String.IsNullOrEmpty(txtProductName.Text) || (String.IsNullOrEmpty(hdnProductID.Value)))
            //                strErr += "<li>" + "Select Proper Product From List." + "</li>";
            //        }
            //        // -------------------------------------------------------------
            //        if (_pageValid)
            //        {
            //            DataTable dtDetail = new DataTable();
            //            dtDetail = (DataTable)Session["dtDetail"];

            //            if (dtDetail != null)
            //            {
            //                //----Check For Duplicate Item----//
            //                string find = "ProductID = " + hdnProductID.Value + "";
            //                DataRow[] foundRows = dtDetail.Select(find);
            //                if (foundRows.Length > 0)
            //                {
            //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
            //                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
            //                    return;
            //                }

            //                DataRow dr = dtDetail.NewRow();

            //                dr["pkID"] = 0;
            //                dr["WorkOrderNo"] = txtWorkOrderNo.Text;
            //                dr["ProductID"] = hdnProductID.Value;
            //                dr["ProductName"] = txtProductName.Text;
            //                dr["DeliveryDate"] = (!String.IsNullOrEmpty(txtDelDate.Text)) ? Convert.ToDateTime(txtDelDate.Text) : SqlDateTime.MinValue.Value;

            //                dtDetail.Rows.Add(dr);
            //                // ---------------------------------------------------------------
            //                rptProductDetail.DataSource = dtDetail;
            //                rptProductDetail.DataBind();
            //                //----------------------------------------------------------------
            //                Session.Add("dtDetail", dtDetail);

            //            }
            //        }
            //        if (!String.IsNullOrEmpty(strErr))
            //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
            //    }
        }

        protected void drpSalesOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Entity.InternalWorkOrder> lst = new List<Entity.InternalWorkOrder>();
            lst = BAL.InternalWorkOrderMgmt.GetQuotationInquiry(drpSalesOrder.SelectedValue, Convert.ToInt64(hdnCustomerID.Value));
            if (lst.Count > 0)
            {
                txtQuotationNo.Text = lst[0].QuotationNo;
                txtInquiryNo.Text = lst[0].InquiryNo;
            }

        }
    }
}
    
