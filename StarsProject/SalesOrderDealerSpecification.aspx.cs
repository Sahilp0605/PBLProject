using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SalesOrderDealerSpecification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ---https://www.dotnetcurry.com/ShowArticle.aspx?ID=206
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["OrderNo"]))
                {
                    hdnSalesOrderNo.Value = Request.QueryString["OrderNo"].ToString();
                    if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                    {
                        hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();
                        if (String.IsNullOrEmpty(hdnFinishProductID.Value) || hdnFinishProductID.Value == "")
                            hdnFinishProductID.Value = "0";
                    }
                    // --------------------------------------------------------


                    BindProductSpecs();
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["QuotationNo"]))
                {
                    hdnQuotationNo.Value = Request.QueryString["QuotationNo"].ToString();
                    if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                    {
                        hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();
                        if (String.IsNullOrEmpty(hdnFinishProductID.Value) || hdnFinishProductID.Value == "")
                            hdnFinishProductID.Value = "0";
                    }
                    BindQuotationProductSpecs();
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                    {
                        hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();
                    }
                    else if (String.IsNullOrEmpty(hdnFinishProductID.Value) || hdnFinishProductID.Value == "")
                        hdnFinishProductID.Value = "0";
                    BindProductSpecs();
                }
                hdnQuatSpecRemark.Value = BAL.CommonMgmt.GetConstant("QuatSpecRemark", 0, 1);
            }
        }

        public void BindQuotationProductSpecs()
        {
            DataTable dtSpecs = new DataTable();
            List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
            if (!String.IsNullOrEmpty(hdnQuotationNo.Value) || hdnQuotationNo.Value != "")
            {
                lst = BAL.ProductMgmt.GetQuotationProductSpecList(hdnQuotationNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                if (lst.Count == 0)
                {
                    lst = BAL.ProductMgmt.GetQuotationProductSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                }
            }
            else
                lst = BAL.ProductMgmt.GetQuotationProductSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
            // -----------------------------------------
            dtSpecs = PageBase.ConvertListToDataTable(lst);
            if (hdnQuatSpecRemark.Value.ToLower() == "yes")
            {
                if (dtSpecs.Rows.Count > 0)
                    txtRemarks.Text = dtSpecs.Rows[0]["MaterialSpec"].ToString();
            }
            else
            {
                rptProductSpecs.DataSource = dtSpecs;
                rptProductSpecs.DataBind();
            }
            if (Session["dtSpecs"] == null)
            {
                Session["dtSpecs"] = dtSpecs;
            }
        }

        public void BindProductSpecs()
        {
            DataTable dtSpecs = new DataTable();
            List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
            if (!String.IsNullOrEmpty(hdnSalesOrderNo.Value) || hdnSalesOrderNo.Value != "")
            {
                lst = BAL.ProductMgmt.GetSalesOrderDealerProductSpecList(hdnSalesOrderNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                if (lst.Count == 0)
                {
                    lst = BAL.ProductMgmt.GetSalesOrderDealerProductSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                }
            }
            else
                lst = BAL.ProductMgmt.GetSalesOrderDealerProductSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
            // -----------------------------------------
            dtSpecs = PageBase.ConvertListToDataTable(lst);
            if (hdnQuatSpecRemark.Value.ToLower() == "yes")
            {
                if (dtSpecs.Rows.Count > 0)
                    txtRemarks.Text = dtSpecs.Rows[0]["MaterialSpec"].ToString();
            }
            else
            {
                rptProductSpecs.DataSource = dtSpecs;
                rptProductSpecs.DataBind();
            }
            if (Session["dtSpecs"] == null)
            {
                Session["dtSpecs"] = dtSpecs;
            }
        }

        protected void rptProductSpecs_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtGroupHead = ((TextBox)e.Item.FindControl("newGroupHead"));
                TextBox txtMaterialHead = ((TextBox)e.Item.FindControl("newMaterialHead"));
                TextBox txtMaterialSpec = ((TextBox)e.Item.FindControl("newMaterialSpec"));
                TextBox txtOrder = ((TextBox)e.Item.FindControl("newOrder"));
                // -----------------------------------------------------

            }
            //if (e.Item.ItemType == ListItemType.Footer)
            //{
            //    Label lblTotalGrossAmount = (Label)e.Item.FindControl("lblTotalGrossAmount");
            //    Label lblTotalTaxAmount = (Label)e.Item.FindControl("lblTotalTaxAmount");
            //    Label lblTotalNetAmount = (Label)e.Item.FindControl("lblTotalNetAmount");

            //    lblTotalGrossAmount.Text = totAmount.ToString("0.00");
            //    lblTotalTaxAmount.Text = totTaxAmount.ToString("0.00");
            //    lblTotalNetAmount.Text = totNetAmount.ToString("0.00");
            //}

            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    DropDownList ddl = ((DropDownList)e.Item.FindControl("drpBundle0"));
            //    ddl.DataSource = BindBundleList();
            //    ddl.DataValueField = "BundleId";
            //    ddl.DataTextField = "BundleName";
            //    ddl.DataBind();
            //    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            //    HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnBundle"));
            //    if (!String.IsNullOrEmpty(tmpField.Value))
            //        ddl.SelectedValue = tmpField.Value;
            //}
            //if (e.Item.ItemType == ListItemType.Footer)
            //{
            //    DropDownList ddl = ((DropDownList)e.Item.FindControl("drpBundle1"));
            //    ddl.DataSource = BindBundleList();
            //    ddl.DataValueField = "BundleId";
            //    ddl.DataTextField = "BundleName";
            //    ddl.DataBind();
            //    ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            //}
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dtSpecs = new DataTable();

            if (hdnQuatSpecRemark.Value.ToLower() == "yes")
            {
                if (Session["dtSpecs"] != null)
                {
                    dtSpecs = (DataTable)Session["dtSpecs"];

                    DataRow[] drr = dtSpecs.Select("OrderNo = '" + hdnSalesOrderNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
                    foreach (var row in drr)
                        row.Delete();
                    dtSpecs.AcceptChanges();
                    divErrorMessage.InnerHtml = " Dealer SalesOrder Remarks Added Successfuly </br> <b> Note : Don't forget to 'Save' Dealer SalesOrder From Main Screen.</b> ";
                }
                // ------------------------------------------------------    
                int tt = -1;
                //foreach (RepeaterItem item in rptProductSpecs.Items)
                //{

                //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                //    {
                //        HiddenField hdnQuotNo = (HiddenField)item.FindControl("hdnQuotNo");
                //        HiddenField hdnProductID = (HiddenField)item.FindControl("hdnProductID");
                //        String ctrGroupHead = ((TextBox)item.FindControl("newGroupHead")).Text;
                //        String ctrMaterialHead = ((TextBox)item.FindControl("newMaterialHead")).Text;
                //        String ctrMaterialSpec = ((TextBox)item.FindControl("newMaterialSpec")).Text;
                // --------------------------------------------------------
                //tt = dtSpecs.Rows.IndexOf(dtSpecs.Select("SalesOrderNo = '" + hdnQuotNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value)[0]);                    
                DataRow dr = dtSpecs.NewRow();
                dr["OrderNo"] = hdnSalesOrderNo.Value;
                dr["FinishProductID"] = Convert.ToInt64(hdnFinishProductID.Value);
                dr["GroupHead"] = "";
                dr["MaterialHead"] = "";
                dr["MaterialSpec"] = txtRemarks.Text;
                dtSpecs.Rows.Add(dr);
                //}
                //}
                Session.Add("dtSpecs", dtSpecs);
            }
            else
            {
                if (Session["dtSpecs"] != null)
                {
                    dtSpecs = (DataTable)Session["dtSpecs"];

                    DataRow[] drr = dtSpecs.Select("OrderNo = '" + hdnSalesOrderNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
                    foreach (var row in drr)
                        row.Delete();
                    dtSpecs.AcceptChanges();
                    divErrorMessage.InnerHtml = " <center> Dealer SalesOrder Specification Added Successfuly </br> <b> Note : Don't forget to 'Save' Dealer SalesOrder From Main Screen.</b> </center>";
                }
                // ------------------------------------------------------    
                int tt = -1;
                foreach (RepeaterItem item in rptProductSpecs.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField hdnQuotNo = (HiddenField)item.FindControl("hdnQuotNo");
                        HiddenField hdnProductID = (HiddenField)item.FindControl("hdnProductID");
                        String ctrGroupHead = ((TextBox)item.FindControl("newGroupHead")).Text;
                        String ctrMaterialHead = ((TextBox)item.FindControl("newMaterialHead")).Text;
                        String ctrMaterialSpec = ((TextBox)item.FindControl("newMaterialSpec")).Text;
                        String ctrOrder = ((TextBox)item.FindControl("newOrder")).Text;
                        // --------------------------------------------------------
                        //tt = dtSpecs.Rows.IndexOf(dtSpecs.Select("SalesOrderNo = '" + hdnQuotNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value)[0]);                    
                        DataRow dr = dtSpecs.NewRow();
                        dr["OrderNo"] = hdnSalesOrderNo.Value;
                        dr["FinishProductID"] = Convert.ToInt64(hdnProductID.Value);
                        dr["GroupHead"] = ctrGroupHead;
                        dr["MaterialHead"] = ctrMaterialHead;
                        dr["MaterialSpec"] = ctrMaterialSpec;
                        dr["ItemOrder"] = ctrOrder;
                        dtSpecs.Rows.Add(dr);
                    }
                }
                Session.Add("dtSpecs", dtSpecs);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:myTabControl();", true);

        }
    }
}