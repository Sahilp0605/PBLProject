using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace StarsProject
{
    public partial class OutwardAssembly : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["OutwardNo"]))
                    hdnOutwardNo.Value = Request.QueryString["OutwardNo"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["ProductID"]))
                    hdnProductID.Value = Request.QueryString["ProductID"].ToString();

                // --------------------------------------------------------
                DataTable dtAssembly = new DataTable();
                dtAssembly = (DataTable)Session["dtAssembly"];
                rptOutwardDetailAssembly.DataSource = dtAssembly;
                rptOutwardDetailAssembly.DataBind();
            }
        }
        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            //int totalrecord;

            //Control rptFootCtrl = rptOutwardDetailAssembly.Controls[rptOutwardDetailAssembly.Controls.Count - 1].Controls[0];
            //HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            //TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
            //TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
            //TextBox txtUnit = ((TextBox)rptFootCtrl.FindControl("txtUnit"));
            //TextBox txtQuantityWeight = ((TextBox)rptFootCtrl.FindControl("txtQuantityWeight"));
            //TextBox txtSerialNo = ((TextBox)rptFootCtrl.FindControl("txtSerialNo"));
            //TextBox txtBoxNo = ((TextBox)rptFootCtrl.FindControl("txtBoxNo"));
            //List<Entity.Products> lstEntity = new List<Entity.Products>();

            //if (!String.IsNullOrEmpty(hdnProductID.Value))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //txtUnit.Text = (lstEntity.Count > 0) ? lstEntity[0].Unit : "";

            // ------------------------------------------------------------------
            //txtQuantity.Focus();

        }

        protected void rptOutwardDetailAssembly_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string pkid = ((HiddenField)e.Item.FindControl("edpkID")).Value;
                string icode1 = ((HiddenField)e.Item.FindControl("edProductID")).Value;
                string icode2 = ((HiddenField)e.Item.FindControl("edAssemblyID")).Value;

                if (e.CommandName.ToString() == "Delete")
                {
                    int ReturnCode = 0;
                    string ReturnMsg = "";

                    DataTable dtAssembly = new DataTable();
                    dtAssembly = (DataTable)Session["dtAssembly"];
                    // --------------------------------- Delete Record
                    foreach (DataRow dr in dtAssembly.Rows)
                    {
                        if (dr["pkID"].ToString() == pkid && dr["ProductID"].ToString() == icode1 && dr["AssemblyID"].ToString() == icode2)
                        {
                            dtAssembly.Rows.Remove(dr);
                            dtAssembly.AcceptChanges();
                            break;
                        }
                    }

                    rptOutwardDetailAssembly.DataSource = dtAssembly;
                    rptOutwardDetailAssembly.DataBind();

                    Session.Add("dtAssembly", dtAssembly);
                    // -----------------------------------------------
                    strErr += "<li>Assembly Product Deleted Successfully !</li>";
                    if (!String.IsNullOrEmpty(strErr))
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);

                }
            }
        }

        protected void btnSaveAssembly_Click(object sender, EventArgs e)
        {
            DataTable dtAssembly = new DataTable();
            dtAssembly = (DataTable)Session["dtAssembly"];
            foreach (System.Data.DataColumn col in dtAssembly.Columns) col.ReadOnly = false;
            //----------------------------------------------------------------
            foreach (RepeaterItem item in rptOutwardDetailAssembly.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField edpkID = (HiddenField)item.FindControl("edpkID");
                    HiddenField edProductID = (HiddenField)item.FindControl("edProductID");
                    HiddenField edAssemblyID = (HiddenField)item.FindControl("edAssemblyID");
                    TextBox edUnit = (TextBox)item.FindControl("edUnit");
                    TextBox edQuantity = (TextBox)item.FindControl("edQuantity");
                    TextBox edQuantityWeight = (TextBox)item.FindControl("edQuantityWeight");
                    TextBox edSerialNo = (TextBox)item.FindControl("edSerialNo");
                    TextBox edBoxNo = (TextBox)item.FindControl("edBoxNo");

                    Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
                    Decimal qw = (!String.IsNullOrEmpty(edQuantityWeight.Text)) ? Convert.ToDecimal(edQuantityWeight.Text) : 0;
                    // --------------------------------------------------------------------------


                    foreach (DataRow row in dtAssembly.Rows)
                    {
                        if (row["pkID"].ToString() == edpkID.Value && row["ProductID"].ToString() == edProductID.Value && row["AssemblyID"].ToString() == edAssemblyID.Value)
                        {
                            row.SetField("Quantity", q);
                            row.SetField("Unit", edUnit.Text);
                            row.SetField("QuantityWeight", qw);
                            row.SetField("SerialNo", edSerialNo.Text);
                            row.SetField("BoxNo", edBoxNo.Text);
                            row.AcceptChanges();
                        }
                    }
                }
            }
            rptOutwardDetailAssembly.DataSource = dtAssembly;
            rptOutwardDetailAssembly.DataBind();
            Session.Add("dtAssembly", dtAssembly);
            string strErr = "";
            strErr += "<li>Data Updated Successfully !</li>";
            if (!String.IsNullOrEmpty(strErr))
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);

        }
    }
}