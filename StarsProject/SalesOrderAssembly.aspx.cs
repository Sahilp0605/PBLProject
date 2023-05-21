using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SalesOrderAssembly : System.Web.UI.Page
    {
        bool _pageValid = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["OrderNo"]))
                    hdnOrderNo.Value = Request.QueryString["OrderNo"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                    hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();

                // --------------------------------------------------------
                DataTable dtAssembly = new DataTable();
                dtAssembly = (DataTable)Session["dtAssembly"];
                rptQuotationAssembly.DataSource = BindRepeater(dtAssembly);
                rptQuotationAssembly.DataBind();
            }
        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {

            Control edSender = (Control)sender;
            var item = (RepeaterItem)edSender.NamingContainer;
            HiddenField edpkID = (HiddenField)item.FindControl("edpkID");
            TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
            TextBox edUnit = (TextBox)item.FindControl("edUnit");

            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];

            foreach (System.Data.DataColumn col in dtAssembly.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtAssembly.Rows)
            {
                if (row.RowState.ToString() != "Deleted")
                {
                    if (row["pkID"].ToString() == edpkID.Value)
                    {
                        row.SetField("Quantity", edQuantity.Text);
                        row.SetField("Unit", edUnit.Text);
                    }
                }
            }
            dtAssembly.AcceptChanges();
            rptQuotationAssembly.DataSource = BindRepeater(dtAssembly);
            rptQuotationAssembly.DataBind();

            Session.Add("dtAssembly", dtAssembly);
        }

        protected void rptQuotationAssembly_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                        DataTable dtAssembly = new DataTable();
                        dtAssembly = (DataTable)Session["dtAssembly"];

                        if (dtAssembly != null)
                        {
                            foreach (System.Data.DataColumn col in dtAssembly.Columns) col.AllowDBNull = true;

                            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();

                            //----Check For Duplicate Item----//
                            string find = "ProductID = " + ((HiddenField)e.Item.FindControl("hdnProductID")).Value + " AND FinishedProductID = " + hdnFinishProductID.Value;

                            DataRow[] foundRows = dtAssembly.Select(find);
                            if (foundRows.Length > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "alert('Duplicate Item Not Allowed..!!')", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "clearProductField();", true);
                                return;
                            }


                            string icode = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                            string iname = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                            string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                            string unit = ((TextBox)e.Item.FindControl("txtUnit")).Text;
                            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                            DataRow dr = dtAssembly.NewRow();
                            Int64 Rows = dtAssembly.Rows.Count + 1;
                            dr["pkID"] = Rows;
                            dr["FinishedProductID"] = hdnFinishProductID.Value;
                            dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                            dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                            dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                            dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? unit : "";
                            dtAssembly.Rows.Add(dr);
                            // ---------------------------------------------------------------
                            Session.Add("dtAssembly", dtAssembly);
                            rptQuotationAssembly.DataSource = BindRepeater(dtAssembly);
                            rptQuotationAssembly.DataBind();
                            //----------------------------------------------------------------


                        }
                    }
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int ReturnCode = 0;
                    string ReturnMsg = "";
                    string pkid = ((HiddenField)e.Item.FindControl("edpkID")).Value;
                    DataTable dtAssembly = new DataTable();
                    dtAssembly = (DataTable)Session["dtAssembly"];
                    // --------------------------------- Delete Record
                    foreach (DataRow dr in dtAssembly.Rows)
                    {
                        if (dr["pkID"].ToString() == pkid)
                        {
                            dtAssembly.Rows.Remove(dr);
                            break;
                        }
                        dtAssembly.AcceptChanges();
                    }
                    Session.Add("dtAssembly", dtAssembly);
                    rptQuotationAssembly.DataSource = BindRepeater(dtAssembly);
                    rptQuotationAssembly.DataBind();
                    // -----------------------------------------------
                    strErr += "<li>Assembly Product Deleted Successfully !</li>";
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            int totalrecord;

            Control rptFootCtrl = rptQuotationAssembly.Controls[rptQuotationAssembly.Controls.Count - 1].Controls[0];
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();
            if (!String.IsNullOrEmpty(hdnProductID.Value))
                lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";

            // ------------------------------------------------------------------
            txtQuantity.Focus();
        }

        protected void btnSaveAssembly_Click(object sender, EventArgs e)
        {
            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];
            foreach (System.Data.DataColumn col in dtAssembly.Columns) col.ReadOnly = false;
            //----------------------------------------------------------------
            foreach (RepeaterItem item in rptQuotationAssembly.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField edpkID = (HiddenField)item.FindControl("edpkID");
                    HiddenField edFinishProductID = (HiddenField)item.FindControl("edFinishProductID");
                    HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
                    TextBox edUnit = (TextBox)item.FindControl("edUnit");
                    TextBox edQuantity = (TextBox)item.FindControl("edQuantity");

                    Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
                    // --------------------------------------------------------------------------
                    foreach (DataRow row in dtAssembly.Rows)
                    {
                        if (row["pkID"].ToString() == edpkID.Value)
                        {
                            row.SetField("Quantity", q);
                            row.SetField("Unit", edUnit.Text);
                            row.AcceptChanges();
                        }
                    }
                }
            }
            Session.Add("dtAssembly", dtAssembly);
            rptQuotationAssembly.DataSource = BindRepeater(dtAssembly);
            rptQuotationAssembly.DataBind();
            string strErr = "";
            strErr += "<li>Data Updated Successfully !</li>";
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);

        }

        public DataTable BindRepeater(DataTable dtAss)
        {
            DataTable dt = new DataTable();
            if (dtAss != null)
            {
                var raw = dtAss.AsEnumerable().Where(r => r.Field<Int64>("FinishedProductID") == Convert.ToInt64(hdnFinishProductID.Value)).Any();
                if (raw)
                {
                    dt = dtAss.AsEnumerable().Where(r => r.Field<Int64>("FinishedProductID") == Convert.ToInt64(hdnFinishProductID.Value)).CopyToDataTable();
                }
            }
            return dt;
        }
    }
}