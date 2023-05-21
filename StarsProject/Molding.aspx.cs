using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class Molding : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

            hdnType.Value = drpWType.SelectedValue;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.Molding> lstEntity = new List<Entity.Molding>();
                // ----------------------------------------------------
                lstEntity = BAL.MoldingMgmt.GetMoldingList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtMoldingNo.Text = lstEntity[0].MoldingNo;
                txtCustomerName.Text = lstEntity[0].CustomerName;
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                drpWType.SelectedValue = lstEntity[0].WorkType;
                drpOrder.SelectedValue = lstEntity[0].OrderNo;
                txtMoldingDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].MoldingDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                BindMoldingDetailList(txtMoldingNo.Text);
                txtClientName_TextChanged(null, null);
                txtMoldingDate.Focus();
            }
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnCustomerID.Value = "";
            txtMoldingNo.Text = "";
            txtCustomerName.Text = "";
            drpWType.SelectedValue = "";
            drpOrder.SelectedValue = "";
            txtMoldingDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtMoldingDate.Focus();
            BindMoldingDetailList("");
        }

        public void OnlyViewControls()
        {
            txtMoldingNo.ReadOnly = true;
            txtMoldingDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            drpWType.Attributes.Add("disabled", "disabled");
            drpOrder.Attributes.Add("disabled", "disabled");
            btnSave.Visible = false;
            btnReset.Visible = false;
            BindMoldingDetailList("");
        }

        public void BindMoldingDetailList(string pMoldingNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.MoldingMgmt.GetMoldingDetail(pMoldingNo);
            rptMoldingDetail.DataSource = dtDetail1;
            rptMoldingDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void rptMoldingDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;


                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtWorker")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0" ||
                        ((TextBox)e.Item.FindControl("txtDie")).Text == "0" || ((TextBox)e.Item.FindControl("txtCavity")).Text == "0")
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtWorker")).Text))
                            strErr += "<li>" + "Worker is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtQuantity")).Text) || ((TextBox)e.Item.FindControl("txtQuantity")).Text == "0")
                            strErr += "<li>" + "Quantity is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtDie")).Text) || ((TextBox)e.Item.FindControl("txtDie")).Text == "0")
                            strErr += "<li>" + "Die should not be Blank." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtCavity")).Text) || ((TextBox)e.Item.FindControl("txtCavity")).Text == "0")
                            strErr += "<li>" + "Cavity should not be Blank." + "</li>";

                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        if (dtDetail != null)
                        {
                            // ----------------------------------------------------------
                            foreach (System.Data.DataColumn col in dtDetail.Columns) col.AllowDBNull = true;

                            // ----------------------------------------------------------
                            //----Check For Duplicate Item----//
                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = dtDetail.Rows.Count + 1;
                            string worktype = ((DropDownList)e.Item.FindControl("drpWorkType")).SelectedValue;
                            string worker = ((TextBox)e.Item.FindControl("txtWorker")).Text;
                            string product = ((TextBox)e.Item.FindControl("txtProduct")).Text;
                            string pcode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                            string cname = ((TextBox)e.Item.FindControl("txtClientName")).Text;
                            string orderno = ((DropDownList)e.Item.FindControl("drpOrder")).SelectedValue;
                            string die = ((TextBox)e.Item.FindControl("txtDie")).Text;
                            string cavity = ((TextBox)e.Item.FindControl("txtCavity")).Text;
                            string dieNo = ((TextBox)e.Item.FindControl("txtDieNo")).Text;
                            string material = ((TextBox)e.Item.FindControl("txtMaterial")).Text;
                            string hardness = ((TextBox)e.Item.FindControl("txtHardness")).Text;
                            string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;

                            dr["MoldingNo"] = txtMoldingNo.Text;
                            dr["WorkType"] = (!String.IsNullOrEmpty(worktype)) ? Convert.ToString(worktype) : "";
                            dr["WorkerName"] = (!String.IsNullOrEmpty(worker)) ? Convert.ToString(worker) : "";
                            dr["ProductID"] = (!String.IsNullOrEmpty(pcode)) ? Convert.ToInt64(pcode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(product)) ? product : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(product)) ? product : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Die"] = (!String.IsNullOrEmpty(die)) ? Convert.ToDecimal(die) : 0;
                            dr["Cavity"] = (!String.IsNullOrEmpty(cavity)) ? Convert.ToDecimal(cavity) : 0;
                            dr["DieNo"] = (!String.IsNullOrEmpty(dieNo)) ? Convert.ToString(dieNo) : "";
                            dr["Material"] = (!String.IsNullOrEmpty(material)) ? Convert.ToString(material) : "";
                            dr["Hardness"] = (!String.IsNullOrEmpty(hardness)) ? Convert.ToString(hardness) : "";
                            dr["OrderNo"] = (!String.IsNullOrEmpty(orderno)) ? Convert.ToString(orderno) : "";
                            dtDetail.Rows.Add(dr);
                            Session.Add("dtDetail", dtDetail);
                            // ---------------------------------------------------------------
                            rptMoldingDetail.DataSource = dtDetail;
                            rptMoldingDetail.DataBind();
                            //----------------------------------------------------------------
                            

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
                Int64 iname = Convert.ToInt64(((HiddenField)e.Item.FindControl("edProductID")).Value);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (Convert.ToInt64(dr["ProductID"]) == iname)
                    {
                        dtDetail.Rows.Remove(dr);
                        //dr.Delete();
                        break;
                    }
                }
                rptMoldingDetail.DataSource = dtDetail;
                rptMoldingDetail.DataBind();
                Session.Add("dtDetail", dtDetail);
            }
        }

        protected void rptMoldingDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Footer)
            //{
            //    Control rptFootCtrl = rptMoldingDetail.Controls[rptMoldingDetail.Controls.Count - 1].Controls[0];
            //    DropDownList drpProduct = ((DropDownList)rptFootCtrl.FindControl("drpProduct"));
            //    // ---------------- Product List -------------------------------------
            //    List<Entity.Products> lstProduct = new List<Entity.Products>();
            //    lstProduct = BAL.ProductMgmt.GetProductList();
            //    drpProduct.DataSource = lstProduct;
            //    drpProduct.DataValueField = "pkID";
            //    drpProduct.DataTextField = "ProductName";
            //    drpProduct.DataBind();
            //    drpProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            //}

        }

        protected void btnReset_ServerClick(object sender, EventArgs e)
        {
            ClearAllField();
            Session.Remove("dtDetail");
        }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];
            //----------------------------------------------------------------
            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "", ReturnMoldingNo = "";

            string strErr = "";
            _pageValid = true;


            if (String.IsNullOrEmpty(txtMoldingDate.Text) || String.IsNullOrEmpty(drpWType.SelectedValue) 
                || String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value)
                || String.IsNullOrEmpty(drpOrder.SelectedValue))
            {
                _pageValid = false;
                if (String.IsNullOrEmpty(txtMoldingDate.Text))
                    strErr += "<li>" + "Moulding Date is required." + "</li>";

                if (String.IsNullOrEmpty(drpWType.SelectedValue))
                    strErr += "<li>" + "Work Type Selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text) && String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Please Select Proper Customer." + "</li>";

                if (String.IsNullOrEmpty(drpOrder.SelectedValue))
                    strErr += "<li>" + "Order Selection is required." + "</li>";

            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0)
                    {
                        Entity.Molding objEntity = new Entity.Molding();

                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.MoldingNo = txtMoldingNo.Text;
                        objEntity.MoldingDate = Convert.ToDateTime(txtMoldingDate.Text);
                        objEntity.CustomerID = !String.IsNullOrEmpty(hdnCustomerID.Value) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                        objEntity.OrderNo = !String.IsNullOrEmpty(drpOrder.SelectedValue) ? drpOrder.SelectedValue : "";
                        objEntity.WorkType = !String.IsNullOrEmpty(drpWType.SelectedValue) ? drpWType.SelectedValue : "";
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.MoldingMgmt.AddUpdateMolding(objEntity, out ReturnCode, out ReturnMsg, out ReturnMoldingNo);
                        strErr += "<li>" + ReturnMsg + "</li>";

                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table

                        
                        if (ReturnCode > 0)
                        {
                            BAL.MoldingMgmt.DeleteMoldingDetailByMoldingNo(txtMoldingNo.Text, out ReturnCode, out ReturnMsg);

                            Entity.MoldingDetail objQuotDet = new Entity.MoldingDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                objQuotDet.pkID = 0;
                                objQuotDet.MoldingNo = ReturnMoldingNo.Trim();
                                objQuotDet.WorkType = !String.IsNullOrEmpty(drpWType.SelectedValue) ? drpWType.SelectedValue : "" ;
                                objQuotDet.WorkerName = Convert.ToString(dr["WorkerName"]);
                                objQuotDet.ClientID = !String.IsNullOrEmpty(hdnCustomerID.Value) ? Convert.ToInt64(hdnCustomerID.Value) : 0;
                                objQuotDet.OrderNo = !String.IsNullOrEmpty(drpOrder.SelectedValue) ? drpOrder.SelectedValue : ""; ;
                                objQuotDet.ProductID = Convert.ToInt64(dr["ProductID"]);
                                objQuotDet.Die = Convert.ToDecimal(dr["Die"]);
                                objQuotDet.Cavity = Convert.ToDecimal(dr["Cavity"]);
                                objQuotDet.DieNo = Convert.ToString(dr["DieNo"]);
                                objQuotDet.Material= Convert.ToString(dr["Material"]);
                                objQuotDet.Hardness = Convert.ToString(dr["Hardness"]);
                                objQuotDet.Quantity = Convert.ToDecimal(dr["Quantity"]);
                                objQuotDet.LoginUserID = Session["LoginUserID"].ToString();
                                BAL.MoldingMgmt.AddUpdateMoldingDetail(objQuotDet, out ReturnCode1, out ReturnMsg1);
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

        [System.Web.Services.WebMethod]
        public static string DeleteMolding(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.MoldingMgmt.DeleteMolding(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void txtClientName_TextChanged(object sender, EventArgs e)
        {
            //------------------------------------
            List<Entity.SalesOrder> lstEntity = new List<Entity.SalesOrder>();
            if (Convert.ToInt64(hdnCustomerID.Value) != 0)
            {
                lstEntity = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer("", Convert.ToInt64(hdnCustomerID.Value), "Approved", 0, 0);
                // --------------------------------------------------
                    drpOrder.DataValueField = "OrderNo";
                    drpOrder.DataTextField = "OrderNo";
                // ----------------------------------------------------
                if (lstEntity.Count > 0)
                {
                    drpOrder.DataSource = lstEntity;
                    drpOrder.DataBind();
                }
                drpOrder.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
            else
            {
                drpOrder.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            }
        }

        protected void drpOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Control rptFootCtrl = rptMoldingDetail.Controls[rptMoldingDetail.Controls.Count - 1].Controls[0];
            //DropDownList drpOrder = ((DropDownList)rptFootCtrl.FindControl("drpOrder"));
            //DropDownList drpProduct = ((DropDownList)rptFootCtrl.FindControl("drpProduct"));

            //List<Entity.MoldingDetail> lstEntity = new List<Entity.MoldingDetail>();
            //if (!String.IsNullOrEmpty(drpOrder.SelectedValue))
            //{
            //    lstEntity = BAL.MoldingMgmt.GetProductsByOrderNo(drpOrder.SelectedValue);
            //    drpProduct.DataValueField = "ProductID";
            //    drpProduct.DataTextField = "ProductNameLong";
            //    if (lstEntity.Count > 0)
            //    {
            //        drpProduct.DataSource = lstEntity;
            //        drpProduct.DataBind();
            //    }
            //    drpProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            //}
            //else
            //{
            //    drpProduct.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            //}
        }

        protected void txtCavity_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptMoldingDetail.Controls[rptMoldingDetail.Controls.Count - 1].Controls[0];
            TextBox txtDie = ((TextBox)rptFootCtrl.FindControl("txtDie"));
            TextBox txtCavity = ((TextBox)rptFootCtrl.FindControl("txtCavity"));
            TextBox txtDieNo = ((TextBox)rptFootCtrl.FindControl("txtDieNo"));
            TextBox txtMaterial = ((TextBox)rptFootCtrl.FindControl("txtMaterial"));
            TextBox txtHardness = ((TextBox)rptFootCtrl.FindControl("txtHardness"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            if (!String.IsNullOrEmpty(txtDie.Text) && !String.IsNullOrEmpty(txtCavity.Text))
                txtQuantity.Text = (Convert.ToDecimal(txtDie.Text) * Convert.ToDecimal(txtCavity.Text)).ToString();
            else
                txtQuantity.Text = "0";
        }


        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            String strErr = "";

            Control edSender = (Control)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField hdnProductID = (HiddenField)item.FindControl("edProductID");
            //HiddenField hdnpkID = (HiddenField)item.FindControl("edpkID");
            HiddenField hdnOrderQ = (HiddenField)item.FindControl("edOrderQ");
            TextBox edWorker = (TextBox)item.FindControl("edWorker");
            TextBox edDie = (TextBox)item.FindControl("edDie");
            TextBox edCavity = (TextBox)item.FindControl("edCavity");
            TextBox edDieNo = (TextBox)item.FindControl("edDieNo");
            TextBox edMaterial = (TextBox)item.FindControl("edMaterial");
            TextBox edHardness = (TextBox)item.FindControl("edHardness");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            Decimal die = !String.IsNullOrEmpty(edDie.Text) ? Convert.ToDecimal(edDie.Text) : 1;
            Decimal cavity = !String.IsNullOrEmpty(edCavity.Text) ? Convert.ToDecimal(edCavity.Text) : 1;
            Decimal qty = 0;
            //if (drpWType.SelectedValue == "Molding")
            //{
            //    qty = die * cavity;

            //    Decimal OrderQ = Convert.ToDecimal(hdnOrderQ.Value);
            //    if (qty > OrderQ)
            //    {
            //        _pageValid = false;
            //        strErr = "<li>" + "Moulding Quantity should not Greater then Ordered Quantity - " + hdnOrderQ.Value + " </li>";
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError(\'" + strErr + "\',\'toast-danger\');", true);
            //    }
            //}
            //else
            //    qty = !String.IsNullOrEmpty(edQuantity.Text) ? Convert.ToDecimal(edQuantity.Text) : 0;

            // -------------------------------------------------------------
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;
            if (_pageValid)
            {
                foreach (DataRow dr in dtDetail.Rows)
                {
                    if (dr.RowState.ToString() != "Deleted")
                    {
                        if (dr["ProductID"].ToString() == hdnProductID.Value)
                        {
                            dr["WorkerName"] = !String.IsNullOrEmpty(edWorker.Text) ? edWorker.Text : "";
                            dr["Die"] = die;
                            dr["Cavity"] = cavity;
                            dr["Quantity"] = qty;
                            dr["DieNo"] = edDieNo.Text;
                            dr["Material"] = edMaterial.Text;
                            dr["Hardness"] = edHardness.Text;
                        }
                    }
                }
                dtDetail.AcceptChanges();
                Session.Add("dtDetail", dtDetail);
                rptMoldingDetail.DataSource = dtDetail;
                rptMoldingDetail.DataBind();
            }
        }

        protected void drpOrder_SelectedIndexChanged1(object sender, EventArgs e)
        {
            DataTable dtDetail1 = new DataTable();
            if (drpWType.SelectedValue.ToLower() == "molding")
            {
                dtDetail1 = BAL.MoldingMgmt.GetSOProducts(drpOrder.SelectedValue);
            }
            else
            {
                dtDetail1 = BAL.MoldingMgmt.GetMoldingProducts(drpOrder.SelectedValue);
            }
            rptMoldingDetail.DataSource = dtDetail1;
            rptMoldingDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }

        protected void txtProduct_TextChanged(object sender, EventArgs e)
        {
            Control edSender = (Control)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            string find = "ProductID = " + ((HiddenField)item.FindControl("hdnProductID")).Value + "";
            DataRow[] foundRows = dtDetail.Select(find);
            if (foundRows.Length > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                return;
            }
        }
    }
}