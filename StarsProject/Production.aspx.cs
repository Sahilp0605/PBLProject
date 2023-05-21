using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Production : System.Web.UI.Page
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

        [System.Web.Services.WebMethod]
        public static string DeleteProduction(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ProductionMgmt.DeleteProduction(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Production> lstEntity = new List<Entity.Production>();
                // ----------------------------------------------------
                lstEntity = BAL.ProductionMgmt.GetProduction(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtProNo.Text = lstEntity[0].pkID.ToString();
                txtProDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].ProductionDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnProductIDOuter.Value = lstEntity[0].FinishedProductID.ToString();
                txtProductNameOuter.Text = lstEntity[0].FinishedProductName;
                txtQuantity.Text = lstEntity[0].Quantity.ToString();
                txtRemarks.Text = lstEntity[0].Remarks.ToString();
                // -------------------------------------------------------------------------
                BindProductionDetailList(lstEntity[0].pkID);
                txtProDate.Focus();
            }
        }

        public void OnlyViewControls()
        {
            txtProNo.ReadOnly = true;
            txtProDate.ReadOnly = true;
            txtProductNameOuter.ReadOnly = true;
            txtQuantity.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }


        public void ClearAllField()
        {
            Session.Remove("dtDetail");
            hdnpkID.Value = "";
            txtProNo.Text = "";
            txtProDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtProductNameOuter.Text = "";
            txtQuantity.Text = "";
            txtRemarks.Text = "";
            BindProductionDetailList(0);
            txtProNo.Text = "";
            btnSave.Disabled = false;
        }

        public void BindProductionDetailList(Int64 pkID)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.ProductionMgmt.GetProductionDetail(pkID);
            rptPro.DataSource = dtDetail1;
            rptPro.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void btnLoadAssembly_ServerClick(object sender, EventArgs e)
        {
            GetAssemnblyProducts();
        }

        public void GetAssemnblyProducts()
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            dtDetail.Clear();
            // ========================================================

            if (!String.IsNullOrEmpty(hdnProductIDOuter.Value))
            {
                DataTable Ass = new DataTable();
                Ass = BAL.InquiryInfoMgmt.GetAssemblyProductForProduction(Convert.ToInt64(hdnProductIDOuter.Value));

                foreach (System.Data.DataColumn col in Ass.Columns) col.ReadOnly = false;
                foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;
                foreach (System.Data.DataColumn col in dtDetail.Columns) col.AllowDBNull = true;
                if (Ass != null)
                {
                    foreach (DataRow dr in Ass.Rows)
                    {
                        dtDetail.ImportRow(dr);
                    }
                }
                Session.Add("dtDetail", dtDetail);
                rptPro.DataSource = dtDetail;
                rptPro.DataBind();
                Session.Add("dtDetail", dtDetail);
                //hdnProductIDOuter.Value = "";
                Ass.Clear();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Please Enter Proper Product!','toast-danger');", true);
            //txtQuotationDate.Focus();
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            Control edSender = (Control)sender;
            var item = (RepeaterItem)edSender.NamingContainer;
            HiddenField edpkID = (HiddenField)item.FindControl("edpkID");
            HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");
            TextBox edRemarks = (TextBox)item.FindControl("edRemarks");
            
            // --------------------------------------------------------------------------
            Decimal qty = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            String unit = (!String.IsNullOrEmpty(edUnit.Text)) ? edUnit.Text : "";
            String re = (!String.IsNullOrEmpty(edRemarks.Text)) ? edRemarks.Text : "";
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
                    if (row["pkID"].ToString() == edpkID.Value)
                    {
                        row.SetField("Quantity", qty);
                        row.SetField("Unit", unit);
                        row.SetField("Remarks", re);
                    }
                }
                rptPro.DataSource = dtDetail;
                rptPro.DataBind();
            }
            Session.Add("dtDetail", dtDetail);
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            DataTable dtDetail = (DataTable)Session["dtDetail"];
            foreach (DataRow dr in dtDetail.Rows)
            {
                //if(dr["Ref"].ToString() == "old")
                    dr["Quantity"] = Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(dr["Qty"]);
            }
            dtDetail.AcceptChanges();
            Session.Add("dtDetail", dtDetail);
            rptPro.DataSource = dtDetail;
            rptPro.DataBind();
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
                            dr["Qty"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dr["Remarks"] = (!String.IsNullOrEmpty(remarks)) ? remarks : "";
                            dr["Ref"] = "new";
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            //----------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnInwardNo = "";

            string strErr = "";
            _pageValid = true;


            if (String.IsNullOrEmpty(txtProductNameOuter.Text) || String.IsNullOrEmpty(txtProDate.Text) || String.IsNullOrEmpty(txtQuantity.Text))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtProDate.Text))
                    strErr += "<li>" + "Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtProductNameOuter.Text))
                    strErr += "<li>" + "Product Selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtQuantity.Text))
                    strErr += "<li>" + "Quantity is required." + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnProductIDOuter.Value) && hdnProductIDOuter.Value != "0")
                    {
                        Entity.Production objEntity = new Entity.Production();

                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.FinishedProductID = Convert.ToInt64(hdnProductIDOuter.Value);
                        objEntity.ProductionDate = Convert.ToDateTime(txtProDate.Text);
                        objEntity.Quantity = Convert.ToDecimal(txtQuantity.Text);
                        objEntity.Remarks = txtRemarks.Text;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.ProductionMgmt.AddUpdateProduction(objEntity, out ReturnCode, out ReturnMsg);
                        strErr += "<li>" + ReturnMsg + "</li>";

                        // ------------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        // ------------------------------------------------------------------
                        if (ReturnCode > 0)
                        {
                            BAL.ProductionMgmt.DeleteProductionDetailByParentID(ReturnCode, out ReturnCode1, out ReturnMsg1);

                            Entity.Production objQuotDet = new Entity.Production();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.ParentID = ReturnCode;
                                objQuotDet.FinishedProductID = Convert.ToInt64(hdnProductIDOuter.Value);
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.Unit = dr["Unit"].ToString();
                                objQuotDet.Remarks = dr["Remarks"].ToString();
                                objQuotDet.Ref = dr["Ref"].ToString();
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();

                                BAL.ProductionMgmt.AddUpdateProductionDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
                            }
                            if (ReturnCode1 > 0)
                            {
                                Session.Remove("dtDetail");
                            }
                            btnSave.Disabled = true;
                        }
                    }
                    else
                    {
                        strErr = "<li>" + "Atleast One Item is required to save Inward Information !" + "</li>";
                    }
                }
            }
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptPro.Controls[rptPro.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));

            if (!string.IsNullOrEmpty(hdnProductID.Value))
            {
                int tot = 0;
                List<Entity.Products> lstPro = new List<Entity.Products>();
                lstPro = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), Session["LoginUserID"].ToString(), 1, 1111, out tot);
                if (lstPro.Count > 0)
                    txtUnit.Text = lstPro[0].Unit;
            }
        }
    }
}