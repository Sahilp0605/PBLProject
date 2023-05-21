using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class ProductionBySo : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------
                Session.Remove("dtDetail");
                // --------------------------------------------
                hdnSerialKey.Value = Session["SerialKey"].ToString().Replace("\r\n", "");
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

                //if (requestTarget.ToLower() == "drpquotation")
                //{
                //    if ((hdnpkID.Value == "0" || hdnpkID.Value == ""))
                //    {
                //        dtDetail.Clear();
                //        //dtDetail = BAL.QuotationDetailMgmt.GetQuotationProductForSalesOrder(drpQuotation.SelectedValue, txtOrderNo.Text);
                //        rptInwardDetail.DataSource = dtDetail;
                //        rptInwardDetail.DataBind();
                //    }
                //}
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Production> lstEntity = new List<Entity.Production>();
                // ----------------------------------------------------
                lstEntity = BAL.ProductionMgmt.GetProductionBySoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProNo.Text = lstEntity[0].pkID.ToString();
                txtProDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].ProductionDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomer.Text = lstEntity[0].CustomerName;
                dvLoadItems.Visible = false;
                // -------------------------------------------------------------------------
                BindProductionDetailList(lstEntity[0].pkID);
                txtProDate.Focus();
            }
        }

        public void OnlyViewControls()
        {
            txtProNo.ReadOnly = true;
            txtProDate.ReadOnly = true;
            txtCustomer.ReadOnly = true;
            drpReferenceNo.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void ClearAllField()
        {
            Session.Remove("dtDetail");
            hdnpkID.Value = "";
            hdnCustomerID.Value = ""; 
            txtProNo.Text = "";
            txtProDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtCustomer.Text = "";
            drpReferenceNo.Items.Clear();
            BindProductionDetailList(0);
            //BindProductionRawDetailList(0);
            btnSave.Disabled = false;
        }

        public void BindProductionDetailList(Int64 pkID)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.ProductionMgmt.GetProductionDetailBySo(pkID);
            rptPro.DataSource = dtDetail1;
            rptPro.DataBind();
            Session.Add("dtDetail", dtDetail1);

            DataTable dtAssembly = new DataTable();
            dtAssembly = BAL.ProductionMgmt.GetProductionRawDetail(pkID);
            Session.Add("dtAssembly", dtAssembly);
        }
        protected void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                List<Entity.SalesOrder> lstSo = new List<Entity.SalesOrder>();
                lstSo = BAL.SalesOrderMgmt.GetSalesOrderListForProduction(Convert.ToInt64(hdnCustomerID.Value));
                if (lstSo.Count > 0)
                {
                    drpReferenceNo.DataSource = lstSo;
                    drpReferenceNo.DataTextField = "OrderNo";
                    drpReferenceNo.DataValueField = "OrderNo";
                    drpReferenceNo.DataBind();
                }
                drpReferenceNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }

        protected void drpSono_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnSelectedReference.Value))
            {
                DataTable dtDetail = new DataTable();
                dtDetail = (DataTable)Session["dtDetail"];
                dtDetail.Rows.Clear();
                DataTable dtAssembly = (DataTable)Session["dtAssembly"];
                dtAssembly.Clear();
                // ========================================================
                string[] values = hdnSelectedReference.Value.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    string tmpVal = values[i];
                    if (!String.IsNullOrEmpty(tmpVal))
                    {
                        DataTable dtTable = new DataTable();
                        dtTable = BAL.SalesOrderMgmt.GetSalesOrderDetailForProduction(tmpVal);

                        foreach (DataRow myrow in dtTable.Rows)
                        {
                            DataRow dr = dtDetail.NewRow();
                            dr["pkID"] = 0;
                            dr["ProductID"] = myrow["ProductID"].ToString();
                            dr["ProductName"] = myrow["ProductName"].ToString();
                            dr["ProductNameLong"] = myrow["ProductNameLong"].ToString();
                            dr["Quantity"] = Convert.ToDecimal(myrow["Quantity"].ToString());
                            dr["Unit"] = myrow["Unit"].ToString();
                            dr["Remarks"] = myrow["Remarks"].ToString();
                            dr["SoNo"] = myrow["OrderNo"].ToString();
                            dtDetail.Rows.Add(dr);
                        }
                    }
                    dtDetail.AcceptChanges();
                    Session.Add("dtDetail", dtDetail);

                    rptPro.DataSource = dtDetail;
                    rptPro.DataBind();

                    DataTable dt = new DataTable();

                    dt = BAL.SalesOrderMgmt.GetSalesOrderAssemblyForProduction(tmpVal);
                    if (dt != null)
                    {
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            dtAssembly.ImportRow(dr1);
                        }
                        dtAssembly.AcceptChanges();
                        Session.Add("dtAssembly", dtAssembly);
                    }
                }
            }
        }
        

        protected void rptPro_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        //if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || ((TextBox)e.Item.FindControl("txtUnitRate")).Text == "0")
                        //    strErr += "<li>" + "Unit Rate is required." + "</li>";

                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        if (dtDetail != null)
                        {
                            //----Check For Duplicate Item----//
                            string find = "";
                            HiddenField fld1 = ((HiddenField)e.Item.FindControl("hdnProductID"));
                            DropDownList fld2 = ((DropDownList)e.Item.FindControl("drpForOrderNo"));

                            if (!String.IsNullOrEmpty(fld1.Value))
                                find = "ProductID = " + fld1.Value;
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
                            string remarks = ((TextBox)e.Item.FindControl("txtRemarks")).Text;

                            dr["pkID"] = dtDetail != null ? dtDetail.AsEnumerable().Max(s => s.Field<Int64>("pkID")) + 1 : 1;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dr["Remarks"] = (!String.IsNullOrEmpty(remarks)) ? remarks : "";
                            dtDetail.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            rptPro.DataSource = dtDetail;
                            rptPro.DataBind();
                            //----------------------------------------------------------------
                            Session.Add("dtDetail", dtDetail);

                        }
                    }
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
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
                string iname = ((HiddenField)e.Item.FindControl("edpkID")).Value;

                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (dr["pkID"].ToString() == iname)
                    {
                        dtDetail.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }
                rptPro.DataSource = dtDetail;
                rptPro.DataBind();

                Session.Add("dtDetail", dtDetail);
            }
        }
        protected void rptPro_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            //For Footer Section

            //For repeater other section
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edpkID = (HiddenField)item.FindControl("edpkID");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");
            TextBox edRemarks = (TextBox)item.FindControl("edRemarks");
            // --------------------------------------------------------------------------

            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            String u = (!String.IsNullOrEmpty(edUnit.Text)) ? edUnit.Text : "";
            String Re = (!String.IsNullOrEmpty(edRemarks.Text)) ? edRemarks.Text : "";

            // --------------------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["pkID"].ToString() == edpkID.Value)
                {
                    row.SetField("Quantity", edQuantity.Text);
                    row.SetField("Unit", edUnit.Text);
                    row.SetField("Remarks", edRemarks.Text);
                }
            }
            dtDetail.AcceptChanges();

            rptPro.DataSource = dtDetail;
            rptPro.DataBind();
            Session.Add("dtDetail", dtDetail);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnInvoiceNo = "", ReturnMsg1 = "";
            string strErr = "";

            if (String.IsNullOrEmpty(txtCustomer.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || /*String.IsNullOrEmpty(drpReferenceNo.SelectedValue) ||*/
                (String.IsNullOrEmpty(txtProDate.Text)))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomer.Text) || String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Customer Selection is required. " + "</li>";

                //if (String.IsNullOrEmpty(drpReferenceNo.SelectedValue))
                //    strErr += "<li>" + "SSales Order Selection is required. " + "</li>";

                if (String.IsNullOrEmpty(txtProDate.Text))
                    strErr += "<li>" + "Production Date is required. " + "</li>";
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation 
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtProDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtProDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Future Date is Not Valid." + "</li>";
                }
            }

            //--------------------------------------------------------------- 
            // --------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];

            Entity.Production objEntity = new Entity.Production();
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.ProductionDate = Convert.ToDateTime(txtProDate.Text);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.SoNo = drpReferenceNo.SelectedValue;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.ProductionMgmt.AddUpdateProductionBySo(objEntity, out ReturnCode, out ReturnMsg);
                        strErr += "<li>" + ((ReturnCode > 0) ? ReturnInvoiceNo + " " + ReturnMsg : ReturnMsg) + "</li>";
                        // --------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            BAL.ProductionMgmt.DeleteProductionBySoDetailbyParentID(ReturnCode, out ReturnCode1, out ReturnMsg1);
                            // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                            Entity.Production objQuotDet = new Entity.Production();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.ParentID = ReturnCode;
                                objQuotDet.SoNo = dr["SoNo"].ToString();
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.Remarks = dr["Remarks"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.ProductionMgmt.AddUpdateProductionBySoDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                            }

                            BAL.ProductionMgmt.DeleteProductionRawDetailByParentID(ReturnCode, out ReturnCode1, out ReturnMsg1);
                            Entity.Production objQuotDet1 = new Entity.Production();

                            foreach (DataRow dr1 in dtAssembly.Rows)
                            {
                                objQuotDet.ParentID = ReturnCode;
                                objQuotDet.FinishedProductID = Convert.ToInt64(dr1["FinishedProductID"]); ;
                                objQuotDet.ProductID = Convert.ToInt64(dr1["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr1["Quantity"]);
                                objQuotDet.Unit = dr1["Unit"].ToString();
                                objQuotDet.SoNo = dr1["SoNo"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.ProductionMgmt.AddUpdateProductionBySoRawDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtAssembly");
                            }
                        }
                        // --------------------------------------------------------------
                        btnSave.Disabled = true;
                    }
                }
                else
                    strErr += "<li>Atleast One Item is required to save Purchase Bill Information !</li>";
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
        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;
            Control rptFootCtrl = rptPro.Controls[rptPro.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
                TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
                TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
                TextBox txtRemarks = ((TextBox)rptFootCtrl.FindControl("txtRemarks"));

                List<Entity.Products> lstEntity = new List<Entity.Products>();

                if (!String.IsNullOrEmpty(hdnProductID.Value))
                    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                DataTable dtAssembly = new DataTable();
                dtAssembly = (DataTable)Session["dtAssembly"];
                List<Entity.ProductDetailCard> lstAss = new List<Entity.ProductDetailCard>();

                lstAss = BAL.ProductMgmt.GetProductDetailListForProduction(0, Convert.ToInt64(hdnProductID.Value), Session["LoginUSerID"].ToString());
                if (lstAss.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = PageBase.ConvertListToDataTable(lstAss);
                    if (dt != null)
                    {
                        if (dtAssembly == null)
                            dtAssembly = dt.Clone();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dtAssembly != null)
                            {
                                string checkDuplicate = "";
                                checkDuplicate = "";
                                if (!string.IsNullOrEmpty(dr["ProductID"].ToString()))
                                    checkDuplicate = "ProductID = " + dr["ProductID"].ToString();

                                if (!string.IsNullOrEmpty(dr["FinishProductID"].ToString()))
                                    checkDuplicate += " And FinishProductID = '" + dr["FinishProductID"].ToString() + "'";

                                DataRow[] FoundRows = dtAssembly.Select(checkDuplicate);

                                if (FoundRows.Length > 0)
                                    continue;
                            }
                            dtAssembly.ImportRow(dr);
                            dtAssembly.AcceptChanges();
                        }
                    }
                }

                txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";
                // ----------------------------------------------------------------------------
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "NextFocus(txtQuantity);", true);
            }
            else
            {
                strErr += "<li> Select Proper Item From List !</li>";
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteProductionBySo(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProductionMgmt.DeleteProductionBySo(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}