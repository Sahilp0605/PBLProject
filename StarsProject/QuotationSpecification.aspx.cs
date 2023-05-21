using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace StarsProject
{
    public partial class QuotationSpecification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                // ---https://www.dotnetcurry.com/ShowArticle.aspx?ID=206
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnSerialKey.Value = Session["SerialKey"].ToString();
                hdnQuotSpecFormat.Value = BAL.CommonMgmt.GetConstant("QuotationSpecificationFormat", 0, 1);
                hdnQuotationProductParts.Value = BAL.CommonMgmt.GetConstant("QuotationProductParts", 0, 1);
                // --------------------------------------------------------
                if (hdnQuotSpecFormat.Value.ToLower() == "old")
                {
                    txtRemarks.Attributes.Remove("content");
                    txtRemarks.Attributes.Add("class", "form-control");
                }
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["QuotationNo"]))
                   hdnQuotationNo.Value = Request.QueryString["QuotationNo"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["RefNo"]))
                    hdnRefNo.Value = Request.QueryString["RefNo"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                {
                    hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();
                    if (String.IsNullOrEmpty(hdnFinishProductID.Value) || hdnFinishProductID.Value == "")
                        hdnFinishProductID.Value = "0";
                }
                // --------------------------------------------------------

                hdnQuatSpecRemark.Value = BAL.CommonMgmt.GetConstant("QuatSpecRemark", 0, 1);

                BindProductSpecs();
                BindProductParts();
            }
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Product Specification 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        public void BindProductSpecs()
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
            if(lst.Count > 0)
                if (lst[0].ItemOrder != "")
                    lst = lst.OrderBy(e => Convert.ToInt32(e.ItemOrder)).ToList();
            dtSpecs = PageBase.ConvertListToDataTable(lst);
            rptProductSpecs.DataSource = dtSpecs;
            rptProductSpecs.DataBind();
            //}
            if (Session["dtSpecs"] == null)
            {
                Session["dtSpecs"] = dtSpecs;
            }
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

            List<Entity.Products> ProdSpec = new List<Entity.Products>();
            if(!String.IsNullOrEmpty(hdnQuotationNo.Value) || hdnQuotationNo.Value != "")
            {
                ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList(hdnQuotationNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Convert.ToInt64(hdnRefNo.Value), Session["LoginUserID"].ToString());
                if(ProdSpec.Count == 0)
                {
                    ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Convert.ToInt64(hdnRefNo.Value), Session["LoginUserID"].ToString());
                }
            }
            else
                ProdSpec = BAL.ProductMgmt.GetQuotProdSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());

            if (ProdSpec.Count > 0)
            {
                txtRemarks.Text = ProdSpec[0].ProductSpecification.ToString();
            }
            // --------------------------------------------------------------
            //DataTable mySpecs = new DataTable();
            //mySpecs = (DataTable)Session["mySpecs"];
            if (Session["mySpecs"] == null)
                Session["mySpecs"] = PageBase.ConvertListToDataTable(ProdSpec);          
        }
        
        public void BindProductParts()
        {
            
            List<Entity.ProductPartDetail> lst = new List<Entity.ProductPartDetail>();

            if (Session["dtProductParts"] != null ? (((DataTable)Session["dtProductParts"]).Rows.Count > 0 ? true : false) : false)
            {
                DataTable DtTemp = (DataTable)Session["dtProductParts"];
                DataRow[] Dr = DtTemp.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
                if (Dr.Any())
                {
                    rptProdPart.DataSource = Dr.CopyToDataTable();
                    rptProdPart.DataBind();
                }

            }
            else
            {
                DataTable dtDetail1 = new DataTable();
                if (!String.IsNullOrEmpty(hdnQuotationNo.Value))
                    lst = BAL.QuotationDetailMgmt.GetQuotationProductPartList(hdnQuotationNo.Value, 0, Session["LoginUserID"].ToString());
                else
                    lst = BAL.QuotationDetailMgmt.GetQuotationProductPartList("", 0, Session["LoginUserID"].ToString());

                dtDetail1 = PageBase.ConvertListToDataTable(lst);
                Session.Add("dtProductParts", dtDetail1);
                rptProdPart.DataSource = dtDetail1;
                rptProdPart.DataBind();
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
                TextBox txtMaterialRemarks = ((TextBox)e.Item.FindControl("newMaterialRemarks"));
                // -----------------------------------------------------

            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

            DataTable dtSpecs = new DataTable();

            //if(hdnQuatSpecRemark.Value.ToLower()=="yes")
            //{
            //    if (Session["dtSpecs"] != null)
            //    {
            //        dtSpecs = (DataTable)Session["dtSpecs"];

            //        DataRow[] drr = dtSpecs.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
            //        foreach (var row in drr)
            //            row.Delete();
            //        dtSpecs.AcceptChanges();
            //        divErrorMessage.InnerHtml = " Quotation Remarks Added Successfuly </br> <b> Note : Don't forget to 'Save' Quotation From Main Screen.</b> ";
            //    }
            //    // ------------------------------------------------------    
            //    int tt = -1;
            //    //foreach (RepeaterItem item in rptProductSpecs.Items)
            //    //{

            //    //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            //    //    {
            //    //        HiddenField hdnQuotNo = (HiddenField)item.FindControl("hdnQuotNo");
            //    //        HiddenField hdnProductID = (HiddenField)item.FindControl("hdnProductID");
            //    //        String ctrGroupHead = ((TextBox)item.FindControl("newGroupHead")).Text;
            //    //        String ctrMaterialHead = ((TextBox)item.FindControl("newMaterialHead")).Text;
            //    //        String ctrMaterialSpec = ((TextBox)item.FindControl("newMaterialSpec")).Text;
            //            // --------------------------------------------------------
            //            //tt = dtSpecs.Rows.IndexOf(dtSpecs.Select("QuotationNo = '" + hdnQuotNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value)[0]);                    
            //            DataRow dr = dtSpecs.NewRow();
            //            dr["QuotationNo"] = hdnQuotationNo.Value;
            //            dr["FinishProductID"] = Convert.ToInt64(hdnFinishProductID.Value);
            //            dr["GroupHead"] = "";
            //            dr["MaterialHead"] = "";
            //            dr["MaterialSpec"] = txtRemarks.Text;
            //            dr["MaterialRemarks"] = "";
            //            dtSpecs.Rows.Add(dr);
            //        //}
            //    //}
            //    Session.Add("dtSpecs", dtSpecs);
            //}
            //else
            //{
            //Session["ProductShortRemark"] = txtRemarks.Text;
            if (Session["mySpecs"] != null)
            {
                Boolean itemAdded = false;
                DataTable mySpecs = new DataTable();
                mySpecs = (DataTable)Session["mySpecs"];
                foreach (DataRow row in mySpecs.Rows)
                {
                    if (row["pkID"].ToString() == hdnRefNo.Value)
                    {
                        row["ProductSpecification"] = txtRemarks.Text;
                        itemAdded = true;
                    }
                }
                if (!itemAdded)
                {
                    DataRow dr = mySpecs.NewRow();
                    dr["pkID"] = Convert.ToInt64(hdnRefNo.Value);
                    dr["ProductSpecification"] = txtRemarks.Text;
                    mySpecs.Rows.Add(dr);
                }
                mySpecs.AcceptChanges();
                Session.Add("mySpecs", mySpecs);
            }

            // ------------------------------------------------------------------------
            if (Session["dtSpecs"] != null)
                {
                    dtSpecs = (DataTable)Session["dtSpecs"];

                    DataRow[] drr = dtSpecs.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
                    foreach (var row in drr)
                        row.Delete();
                    dtSpecs.AcceptChanges();
                    divErrorMessage.InnerHtml = " <center> Quotation Specification Added Successfuly </br> <b> Note : Don't forget to 'Save'  From Quotation Main Screen.</b> </center>";
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
                        String ctrMaterialRemarks = ((TextBox)item.FindControl("newMaterialRemarks")).Text;
                        String ctrOrder = ((TextBox)item.FindControl("newOrder")).Text;
                        
                        // --------------------------------------------------------
                        //tt = dtSpecs.Rows.IndexOf(dtSpecs.Select("QuotationNo = '" + hdnQuotNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value)[0]);                    
                        DataRow dr = dtSpecs.NewRow();
                        dr["QuotationNo"] = hdnQuotationNo.Value;
                        dr["FinishProductID"] = Convert.ToInt64(hdnProductID.Value);
                        dr["GroupHead"] = ctrGroupHead;
                        dr["MaterialHead"] = ctrMaterialHead;
                        dr["MaterialSpec"] = ctrMaterialSpec;
                        dr["ItemOrder"] = ctrOrder;
                        dr["MaterialRemarks"] = ctrMaterialRemarks;
                        dtSpecs.Rows.Add(dr);
                    }
                }
                Session.Add("dtSpecs", dtSpecs);
            //}


            DataTable dtProductParts = new DataTable();
            dtProductParts = (DataTable)Session["dtProductParts"];
            if (dtProductParts != null)
            {
                DataTable dtQTProductParts = new DataTable();
                dtQTProductParts = (DataTable)Session["dtQTProductParts"];

                if (dtQTProductParts != null)
                {
                    DataRow[] drr = dtQTProductParts.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
                    foreach (var row in drr)
                        row.Delete();
                    dtQTProductParts.AcceptChanges();
                }
                else
                    dtQTProductParts = dtProductParts.Clone();

                if (dtProductParts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProductParts.Rows)
                    {
                        dtQTProductParts.ImportRow(dr);
                    }
                }

                Session.Add("dtQTProductParts", dtQTProductParts);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:retainTabPosition();", true);
            
        }

        protected void rptProdPart_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptProdPart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "DeleteDetail")
            {
                DataTable dtProductParts = new DataTable();
                dtProductParts = (DataTable)Session["dtProductParts"];

                string iname = e.CommandArgument.ToString();

                foreach (DataRow dr in dtProductParts.Rows)
                {
                    if (dr["PartDescription"].ToString() == iname && dr["QuotationNo"].ToString() == hdnQuotationNo.Value && dr["FinishProductID"].ToString() == hdnFinishProductID.Value)
                    {
                        dtProductParts.Rows.Remove(dr);
                        break;
                    }
                }
                dtProductParts.AcceptChanges();
                // -----------------------------------------------------------
                DataRow[] Dr = dtProductParts.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
                if (Dr.Any())
                {
                    Session.Add("dtProductParts", dtProductParts);
                    rptProdPart.DataSource = Dr.CopyToDataTable();
                    rptProdPart.DataBind();
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:retainTabPosition();", true);
        }

        protected void imgBtnSaveDetail_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dtProductParts = new DataTable();
            dtProductParts = (DataTable)Session["dtProductParts"];
            Int64 cntRow = dtProductParts.Rows.Count + 1;
            DataRow dr = dtProductParts.NewRow();
            dr["QuotationNo"] = hdnQuotationNo.Value;
            dr["FinishProductID"] = (!String.IsNullOrEmpty(hdnFinishProductID.Value) && hdnFinishProductID.Value != "0") ? Convert.ToInt64(hdnFinishProductID.Value) : 0;
            dr["ItemOrder"] = newOrder1.Text;
            dr["PartDescription"] = txtPartDescription1.Text;
            dr["BrandName"] = txtBrandName1.Text;
            dtProductParts.Rows.Add(dr);
            dtProductParts.AcceptChanges();
            Session.Add("dtProductParts", dtProductParts);
            // -----------------------------------------------------
            // Filter Records
            // -----------------------------------------------------
            DataRow[] Dr = dtProductParts.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
            if (Dr.Any())
            {
                rptProdPart.DataSource = Dr.CopyToDataTable();
                rptProdPart.DataBind();
            }
            // -----------------------------------------------------
            newOrder1.Text = String.Empty;
            txtPartDescription1.Text = String.Empty;
            txtBrandName1.Text = String.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:retainTabPosition();", true);
        }

        protected void newOrder_TextChanged(object sender, EventArgs e)
        {
            TextBox edSender = (TextBox)sender;
            editItemDataChange(edSender);
        }

        protected void txtPartDescription_TextChanged(object sender, EventArgs e)
        {
            TextBox edSender = (TextBox)sender;
            editItemDataChange(edSender);
        }

        protected void txtBrandName_TextChanged(object sender, EventArgs e)
        {
            TextBox edSender = (TextBox)sender;
            editItemDataChange(edSender);
        }

        public void editItemDataChange(TextBox edSender)
        {
            DataTable dtProductParts = new DataTable();
            dtProductParts = (DataTable)Session["dtProductParts"];


            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField hdnOrder = (HiddenField)item.FindControl("hdnOrder");
            HiddenField hdnDescription = (HiddenField)item.FindControl("hdnDescription");
            HiddenField hdnBrand = (HiddenField)item.FindControl("hdnBrand");

            TextBox newOrder = (TextBox)item.FindControl("newOrder");
            TextBox txtPartDescription = (TextBox)item.FindControl("txtPartDescription");
            TextBox txtBrandName = (TextBox)item.FindControl("txtBrandName");
            // --------------------------------------------------------
            foreach (DataRow dr in dtProductParts.Rows)
            {
                if (dr["ItemOrder"].ToString() == hdnOrder.Value && dr["PartDescription"].ToString() == hdnDescription.Value && dr["BrandName"].ToString() == hdnBrand.Value &&
                    dr["QuotationNo"].ToString() == hdnQuotationNo.Value && dr["FinishProductID"].ToString() == hdnFinishProductID.Value)
                {
                    dr["QuotationNo"] = hdnQuotationNo.Value;
                    dr["FinishProductID"] = Convert.ToInt64(hdnFinishProductID.Value);
                    dr["ItemOrder"] = newOrder.Text;
                    dr["PartDescription"] = txtPartDescription.Text;
                    dr["BrandName"] = txtBrandName.Text;
                    dtProductParts.AcceptChanges();
                    break;
                }
            }
            Session.Add("dtProductParts", dtProductParts);
            // -----------------------------------------------------
            // Filter Records
            // -----------------------------------------------------
            DataRow[] Dr = dtProductParts.Select("QuotationNo = '" + hdnQuotationNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
            if (Dr.Any())
            {
                rptProdPart.DataSource = Dr.CopyToDataTable();
                rptProdPart.DataBind();
            }
            // --------------------------------------------------------------------
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:retainTabPosition();", true);
        }
    }
}