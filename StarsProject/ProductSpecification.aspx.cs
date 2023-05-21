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
    public partial class ProductSpecification : System.Web.UI.Page
    {
        bool _pageValid = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                hdnSerialKey.Value = Session["SerialKey"].ToString();

                hdnSpecFormat.Value = BAL.CommonMgmt.GetConstant("QuotationSpecificationFormat", 0, 1);
                if (hdnSpecFormat.Value == "old")
                {
                    txtRemarks.Attributes.Remove("content");
                    txtRemarks.Attributes.Add("class", "form-control");
                }
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["DocNo"]))
                    hdnDocNo.Value = Request.QueryString["DocNo"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["Module"]))
                    hdnModule.Value = Request.QueryString["Module"].ToString();

                if (!String.IsNullOrEmpty(Request.QueryString["FinishProductID"]))
                {
                    hdnFinishProductID.Value = Request.QueryString["FinishProductID"].ToString();
                    if (String.IsNullOrEmpty(hdnFinishProductID.Value) || hdnFinishProductID.Value == "")
                        hdnFinishProductID.Value = "0";
                }
                // --------------------------------------------------------

                hdnSpecRemark.Value = BAL.CommonMgmt.GetConstant("QuatSpecRemark", 0, 1);


                BindProductSpecs();

                BindJobWorkList();

            }
            // -----------------------------------
            //liJobWork.Visible = (hdnSerialKey.Value == "SUN9-SI3G-YU56-78KJ") ? true : false;

        }
        public void BindProductSpecs()
        {
            DataTable dtSpecs = new DataTable();
            List<Entity.ProductDetailCard> lst = new List<Entity.ProductDetailCard>();
            if (!String.IsNullOrEmpty(hdnDocNo.Value) || hdnDocNo.Value != "")
            {
                lst = BAL.ProductMgmt.GetQuotationProductSpecList(hdnDocNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                if (lst.Count == 0)
                {
                    lst = BAL.ProductMgmt.GetQuotationProductSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                }
            }
            else
                lst = BAL.ProductMgmt.GetQuotationProductSpecList("", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
            // -----------------------------------------
            dtSpecs = PageBase.ConvertListToDataTable(lst);
            rptProductSpecs.DataSource = dtSpecs;
            rptProductSpecs.DataBind();

            if (Session["dtSpecs"] == null)
            {
                Session["dtSpecs"] = dtSpecs;
            }
            // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

            List<Entity.Products> ProdSpec = new List<Entity.Products>();
            if (!String.IsNullOrEmpty(hdnDocNo.Value) || hdnDocNo.Value != "")
            {
                ProdSpec = BAL.ProductMgmt.GetProdSpecList(hdnModule.Value, hdnDocNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                if (ProdSpec.Count == 0)
                {
                    ProdSpec = BAL.ProductMgmt.GetProdSpecList(hdnModule.Value, "", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                }
            }
            else
                ProdSpec = BAL.ProductMgmt.GetProdSpecList(hdnModule.Value, "", Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());

            if (ProdSpec.Count > 0)
            {
                txtRemarks.Text = ProdSpec[0].ProductSpecification.ToString();
            }
            // --------------------------------------------------------------
           
            if (Session["mySpecs"] == null)
            {
                Session["mySpecs"] = PageBase.ConvertListToDataTable(ProdSpec);
            }
        }
        public void BindJobWorkList()
        {
            DataTable dtJobWork = new DataTable();
            if (Session["dtJobWork"] == null)
            {
                List<Entity.SalesBillJobWork> lstJobWork = new List<Entity.SalesBillJobWork>();
                lstJobWork = BAL.SalesBillMgmt.GetSalesBillJobWorkList(0, hdnDocNo.Value, Convert.ToInt64(hdnFinishProductID.Value), Session["LoginUserID"].ToString());
                dtJobWork = PageBase.ConvertListToDataTable(lstJobWork);
                Session.Add("dtJobWork", dtJobWork);
            }
            else
            {
                dtJobWork = (DataTable)Session["dtJobWork"];
            }            
            rptJobWork.DataSource = dtJobWork;
            rptJobWork.DataBind();

        }
        protected void rptProductSpecs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TextBox txtGroupHead = ((TextBox)e.Item.FindControl("newGroupHead"));
                TextBox txtMaterialHead = ((TextBox)e.Item.FindControl("newMaterialHead"));
                TextBox txtMaterialSpec = ((TextBox)e.Item.FindControl("newMaterialSpec"));
                TextBox txtOrder = ((TextBox)e.Item.FindControl("newOrder"));
                TextBox txtMaterialRemarks = ((TextBox)e.Item.FindControl("newMaterialRemarks"));
                
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dtSpecs = new DataTable();
            if (Session["mySpecs"] != null)
            {
                Boolean itemAdded = false;
                DataTable mySpecs = new DataTable();
                mySpecs = (DataTable)Session["mySpecs"];
                foreach (DataRow row in mySpecs.Rows)
                {
                    if (row["pkID"].ToString() == hdnFinishProductID.Value)
                    {
                        row["ProductSpecification"] = txtRemarks.Text;
                        itemAdded = true;
                    }
                }
                if (!itemAdded)
                {
                    DataRow dr = mySpecs.NewRow();
                    dr["pkID"] = Convert.ToInt64(hdnFinishProductID.Value);
                    dr["ProductSpecification"] = txtRemarks.Text;
                    mySpecs.Rows.Add(dr);
                }
                mySpecs.AcceptChanges();
                Session.Add("mySpecs", mySpecs);
                divErrorMessage.InnerHtml = " <center>  Specification Added Successfuly </br> <b> Note : Don't forget to 'Save'  From Main Screen.</b> </center>";
            }

            // ------------------------------------------------------------------------
            //if (Session["dtSpecs"] != null)
            //{
            //    dtSpecs = (DataTable)Session["dtSpecs"];

            //    DataRow[] drr = dtSpecs.Select("QuotationNo = '" + hdnDocNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value);
            //    foreach (var row in drr)
            //        row.Delete();
            //    dtSpecs.AcceptChanges();
            //    divErrorMessage.InnerHtml = " <center>  Specification Added Successfuly </br> <b> Note : Don't forget to 'Save'  From Main Screen.</b> </center>";
            //}
            //// ------------------------------------------------------    
            //int tt = -1;
            //foreach (RepeaterItem item in rptProductSpecs.Items)
            //{

            //    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            //    {
            //        HiddenField hdnQuotNo = (HiddenField)item.FindControl("hdnQuotNo");
            //        HiddenField hdnProductID = (HiddenField)item.FindControl("hdnProductID");
            //        String ctrGroupHead = ((TextBox)item.FindControl("newGroupHead")).Text;
            //        String ctrMaterialHead = ((TextBox)item.FindControl("newMaterialHead")).Text;
            //        String ctrMaterialSpec = ((TextBox)item.FindControl("newMaterialSpec")).Text;
            //        String ctrMaterialRemarks = ((TextBox)item.FindControl("newMaterialRemarks")).Text;
            //        String ctrOrder = ((TextBox)item.FindControl("newOrder")).Text;

            //        // --------------------------------------------------------
            //        //tt = dtSpecs.Rows.IndexOf(dtSpecs.Select("QuotationNo = '" + hdnQuotNo.Value + "' AND FinishProductID = " + hdnFinishProductID.Value)[0]);                    
            //        DataRow dr = dtSpecs.NewRow();
            //        dr["QuotationNo"] = hdnDocNo.Value;
            //        dr["FinishProductID"] = Convert.ToInt64(hdnProductID.Value);
            //        dr["GroupHead"] = ctrGroupHead;
            //        dr["MaterialHead"] = ctrMaterialHead;
            //        dr["MaterialSpec"] = ctrMaterialSpec;
            //        dr["ItemOrder"] = ctrOrder;
            //        dr["MaterialRemarks"] = ctrMaterialRemarks;
            //        dtSpecs.Rows.Add(dr);
            //    }
            //}
            //Session.Add("dtSpecs", dtSpecs);
            ////}

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:myTabControl();", true);

        }

        protected void rptJobWork_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    TextBox txtJobProductName = (TextBox)e.Item.FindControl("txtJobProductName");
                    TextBox txtJobHSNCode = (TextBox)e.Item.FindControl("txtJobHSNCode");
                    TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");

                        if (String.IsNullOrEmpty(txtJobProductName.Text))
                            strErr += "<li>" + "Product Name is required." + "</li>";

                        if (String.IsNullOrEmpty(txtJobHSNCode.Text))
                            strErr += "<li>" + "HSN Code is required." + "</li>";

                        if (String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0)
                            strErr += "<li>" + "Quantity is required." + "</li>";

                    if (strErr != "")
                        _pageValid = false;

                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtJobWork = new DataTable();
                        dtJobWork = (DataTable)Session["dtJobWork"];
                        //----Check For Duplicate Item----//
                        //string find = "FinishProductID = " + hdnFinishProductID.Value;
                        //DataRow[] foundRows = dtJobWork.Select(find);
                        //if (foundRows.Length > 0)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                        //    return;
                        //}


                        Int64 cntRow = dtJobWork.Rows.Count + 1;

                        DataRow dr = dtJobWork.NewRow();

                        dr["pkID"] = cntRow;
                        dr["InvoiceNo"] = hdnDocNo.Value;
                        dr["FinishProductID"] = (!String.IsNullOrEmpty(hdnFinishProductID.Value)) ? Convert.ToInt64(hdnFinishProductID.Value) : 0;
                        dr["JobProductName"] = (!String.IsNullOrEmpty(txtJobProductName.Text)) ? txtJobProductName.Text : "";
                        dr["JobHSNCode"] = (!String.IsNullOrEmpty(txtJobHSNCode.Text)) ? txtJobHSNCode.Text : "";
                        dr["Quantity"] = (!String.IsNullOrEmpty(txtQuantity.Text)) ? Convert.ToDecimal(txtQuantity.Text) : 0;

                        dtJobWork.Rows.Add(dr);
                        dtJobWork.AcceptChanges();
                        Session.Add("dtJobWork", dtJobWork);
                        // ---------------------------------------------------------------
                        rptJobWork.DataSource = dtJobWork;
                        rptJobWork.DataBind();
                    }
                    btnSave.Focus();
                }
                if (!string.IsNullOrEmpty(strErr))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                }
            }
            // --------------------------------------------------------------------------
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    string newInvoiceNo = hdnDocNo.Value;
                    string newFinishProductID = ((HiddenField)e.Item.FindControl("newFinishProductID")).Value;
                    string newJobProductName = ((TextBox)e.Item.FindControl("newJobProductName")).Text;
                    string newJobHSNCode = ((TextBox)e.Item.FindControl("newJobHSNCode")).Text;
                    Boolean deleteFlag = true;
                    // -----------------------------------------------------
                    if (deleteFlag)
                    {
                        DataTable dtJobWork = new DataTable();
                        dtJobWork = (DataTable)Session["dtJobWork"];

                        foreach (DataRow dr in dtJobWork.Rows)
                        {
                            if (dr["InvoiceNo"].ToString() == newInvoiceNo && dr["FinishProductID"].ToString() == newFinishProductID && dr["JobProductName"].ToString() == newJobProductName && dr["JobHSNCode"].ToString() == newJobHSNCode)
                            {
                                dtJobWork.Rows.Remove(dr);
                                break;
                            }
                        }
                        dtJobWork.AcceptChanges();
                        Session.Add("dtJobWork", dtJobWork);

                        rptJobWork.DataSource = dtJobWork;
                        rptJobWork.DataBind();
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Sorry Cannot Delete .. Associated Entry Reference Exists !','toast-danger');", true);
                }
            }
        }
    }
}