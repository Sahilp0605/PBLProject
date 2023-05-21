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
    public partial class PriceList : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                Session["oldUserID"] = "";
                // ----------------------------------------
                ClearAllField();
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
                    FetchPriceListDetail(Convert.ToInt64(hdnpkID.Value));
                }
            }
        }
        public void ClearAllField()
        {
            Session["oldUserID"] = "";
            txtPriceListName.Text = "";
            FetchPriceListDetail(0);
            rptCustPriceList.DataBind();
            txtPriceListName.Focus();
        }
        public void OnlyViewControls()
        {
            txtPriceListName.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                List<Entity.PriceList> lstPriceList = new List<Entity.PriceList>();
                
                // -------------------------------------------------------------------------
                lstPriceList = BAL.PriceListMgmt.GetPriceList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

                hdnpkID.Value = lstPriceList[0].pkID.ToString();
                txtPriceListName.Text = lstPriceList[0].PriceListName;
                txtPriceListName.Focus();
                FetchPriceListDetail(Convert.ToInt64(hdnpkID.Value));
            }
        }
        public void FetchPriceListDetail(Int64 ParentId)
        {
            DataTable dtPrice1 = new DataTable();
            dtPrice1 = BAL.PriceListMgmt.GetPriceListDetail(ParentId);
            rptCustPriceList.DataSource = dtPrice1;
            rptCustPriceList.DataBind();
            Session["dtPrice"] = dtPrice1;

            //int TotalCount = 0;
            //List<Entity.PriceListDetail> lstPriceListDetail = new List<Entity.PriceListDetail>();
            //lstPriceListDetail = BAL.PriceListMgmt.GetPriceListDetail(Convert.ToInt64(ParentId), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);

            //rptCustPriceList.DataSource = lstPriceListDetail;
            //rptCustPriceList.DataBind();
            //Session["dtPrice"] = lstPriceListDetail;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }
        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            String strErr = "";

            int ReturnCode = 0;
            string ReturnMsg = "";
            Int64 ReturnpkID = 0;

            int ReturnCode1 = 0;
            string ReturnMsg1 = "";

            int ReturnCodeDel = 0;
            string ReturnMsgDel = "";

            _pageValid = true;

            if (String.IsNullOrEmpty(txtPriceListName.Text) || rptCustPriceList.Items.Count == 0)
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtPriceListName.Text))
                    strErr += "<li>" + "Price List Name is required." + "</li>";
                if(rptCustPriceList.Items.Count == 0)
                    strErr += "<li>" + "Atleast One Product Require." + "</li>";
            }

            // ----------------------------------------------------------------
            Entity.PriceList objEntity = new Entity.PriceList();
            if (_pageValid)
            {
                if (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0")
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.PriceListName = txtPriceListName.Text;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.PriceListMgmt.AddUpdatePriceList(objEntity, out ReturnCode, out ReturnMsg,out ReturnpkID);
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0 && ReturnpkID > 0)
                {
                    btnSave.Disabled = true;
                    //btnReset.Disabled = true;
                }
               
                if (ReturnCode > 0 )
                {
                    // =========================================================================================
                    // >>>>>>>> Delete all Selectd Price List entry from table
                    // =========================================================================================
                    DataTable dtPrice = new DataTable();
                    dtPrice = (DataTable)Session["dtPrice"];
                    // --------------------------------------------------------------
                    BAL.PriceListMgmt.DeletePriceListDetailByNo(ReturnpkID, out ReturnCodeDel, out ReturnMsgDel);
                    // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                    Entity.PriceListDetail objEntity1 = new Entity.PriceListDetail();

                    foreach (DataRow dr in dtPrice.Rows)
                    {
                        objEntity1.pkID = 0;
                        objEntity1.ParentID = ReturnpkID;
                        objEntity1.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                        objEntity1.UnitPrice = Convert.ToDecimal(dr["UnitPrice"].ToString());
                        objEntity1.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                        objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.PriceListMgmt.AddUpdatePriceListDetail(objEntity1, out ReturnCode1, out ReturnMsg1);

                    }
                    if (ReturnCode1 > 0)
                    {
                        strErr += "<li>" + ReturnMsg1 + "</li>";
                        Session.Remove("dtPrice");
                    }
                }
                
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

        //public void BindPriceList(Int64 pParentID)
        //{
        //    DataTable dtPrice1 = new DataTable();
        //    dtPrice1 = BAL.PriceListMgmt.GetPriceListDetail(pCustomerID);
        //    rptCustPriceList.DataSource = (pCustomerID == 0) ? null : dtPrice1;
        //    rptCustPriceList.DataBind();
        //    Session["dtPrice"] = dtPrice1;
        //}

        protected void rptCustPriceList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
                if (e.CommandName.ToString() == "addprice")
                {
                    TextBox txt_ProductName = (TextBox)e.Item.FindControl("txtProductName");
                    TextBox txt_UnitRate = (TextBox)e.Item.FindControl("txtUnitRate");
                    TextBox txt_Discount = (TextBox)e.Item.FindControl("txtDiscount");
                    // ===================================================================

                    DateTime cdt = DateTime.Now;
                    _pageValid = true;

                    String strErr = "";
                    if ((String.IsNullOrEmpty(txt_ProductName.Text)) && (!String.IsNullOrEmpty(txt_UnitRate.Text) || (!String.IsNullOrEmpty(txt_Discount.Text)))
                           || (!String.IsNullOrEmpty(txt_ProductName.Text)) && (String.IsNullOrEmpty(txt_UnitRate.Text) && (String.IsNullOrEmpty(txt_Discount.Text)))
                           || (String.IsNullOrEmpty(txt_ProductName.Text) && (String.IsNullOrEmpty(txt_UnitRate.Text) && (String.IsNullOrEmpty(txt_Discount.Text)))))
                    {
                        _pageValid = false;

                        if ((String.IsNullOrEmpty(txt_ProductName.Text)) && (!String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) || (!String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtDiscount")).Text))))
                            strErr += "<li>" + "Product Name is required." + "</li>";

                        if ((!String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text)) && (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtUnitRate")).Text) && (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtDiscount")).Text))))
                            strErr += "<li>" + "Unit Rate Or Discount % is required." + "</li>";

                        if (String.IsNullOrEmpty(txt_ProductName.Text) && (String.IsNullOrEmpty(txt_UnitRate.Text) && (String.IsNullOrEmpty(txt_Discount.Text))))
                            strErr += "<li>" + "Product Name And Unit Rate Or Discount % is required." + "</li>";
                    }
                    if (_pageValid)
                    {

                        DataTable dtPrice = new DataTable();
                        dtPrice = (DataTable)Session["dtPrice"];

                        DataRow dr = dtPrice.NewRow();

                        string cProductId = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;

                        Int64 cntRow = 900000 + (dtPrice.Rows.Count + 1);
                        dr["pkID"] = cntRow;

                        dr["ProductID"] = cProductId;
                        dr["ProductName"] = txt_ProductName.Text;
                        dr["UnitPrice"] = (!String.IsNullOrEmpty(txt_UnitRate.Text)) ? Convert.ToDecimal(txt_UnitRate.Text) : 0;
                        dr["Discount"] = (!String.IsNullOrEmpty(txt_Discount.Text)) ? Convert.ToDecimal(txt_Discount.Text) : 0;

                        dtPrice.Rows.Add(dr);

                        Session.Add("dtPrice", dtPrice);
                        // ---------------------------------------------------------------
                        rptCustPriceList.DataSource = dtPrice;
                        rptCustPriceList.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Customer Price List Added Successfully  !');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                    }
                }
            if (e.CommandName.ToString() == "updprice")
            {
                int ReturnCode1 = 0;
                string ReturnMsg1 = "";

                DataTable dtPrice = new DataTable();
                dtPrice = (DataTable)Session["dtPrice"];
                Int64 cpkid = Convert.ToInt64(e.CommandArgument.ToString());
                string cProductId = ((HiddenField)e.Item.FindControl("hdnProductID")).Value;
                string cUnitRate = ((TextBox)e.Item.FindControl("txtUnitRate")).Text;
                string cDiscount = ((TextBox)e.Item.FindControl("txtDiscount")).Text;

                // --------------------------------- Delete Record
                foreach (DataRow dr in dtPrice.Rows)
                {
                    if (Convert.ToInt64(dr["pkID"]) == Convert.ToInt64(cpkid))
                    {
                        dr["ProductID"] = cProductId;
                        dr["UnitPrice"] = Convert.ToDecimal(cUnitRate);
                        dr["Discount"] = Convert.ToDecimal(cDiscount);
                    }
                }

                // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                Entity.PriceListDetail objEntity1 = new Entity.PriceListDetail();

                foreach (DataRow dr in dtPrice.Rows)
                {
                    objEntity1.pkID = Convert.ToInt64(dr["pkID"]);
                    objEntity1.ParentID = Convert.ToInt64(dr["ParentID"]);
                    objEntity1.ProductID = Convert.ToInt64(dr["ProductID"].ToString());
                    objEntity1.UnitPrice = Convert.ToDecimal(dr["UnitPrice"].ToString());
                    objEntity1.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.PriceListMgmt.AddUpdatePriceListDetail(objEntity1, out ReturnCode1, out ReturnMsg1);
                }

                Session.Add("dtPrice", dtPrice);
                // ---------------------------------------------------------------
                rptCustPriceList.DataSource = dtPrice;
                rptCustPriceList.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('<li>Customer Contact Updated Successfully !</li>');", true);
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "delprice")
                {
                    //int ReturnCode = 0;
                    //string ReturnMsg = "";
                    //// --------------------------------- Delete Record
                    //BAL.PriceListMgmt.DeletePriceListDetail(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                    //// -------------------------------------------------
                    //if (ReturnCode > 0)
                    //{
                    //    //BindPriceList(Convert.ToInt64(hdnpkID.Value));
                    //    FetchPriceListDetail(Convert.ToInt64(hdnpkID.Value));
                    //}
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + ReturnMsg + "');", true);

                    DataTable dtPrice = new DataTable();
                    dtPrice = (DataTable)Session["dtPrice"];

                    DataRow[] rows;
                    rows = dtPrice.Select("pkID=" + e.CommandArgument.ToString());
                    foreach (DataRow r in rows)
                        r.Delete();

                    rptCustPriceList.DataSource = dtPrice;
                    rptCustPriceList.DataBind();

                    Session.Add("dtPrice", dtPrice);
                }
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeletePriceList(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            
            // --------------------------------- Delete Record
            BAL.PriceListMgmt.DeletePriceList(pkID, out ReturnCode, out ReturnMsg);
            BAL.PriceListMgmt.DeletePriceListDetail(pkID, out ReturnCode1, out ReturnMsg1);
            // --------------------------------- 
            if (ReturnCode > 0 && ReturnCode1 > 0 )
            { 
                row.Add("ReturnCode", ReturnCode);
                row.Add("ReturnMsg", ReturnMsg);
            }
            rows.Add(row);
            return serializer.Serialize(rows);
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            Control rptFootCtrl = rptCustPriceList.Controls[rptCustPriceList.Controls.Count - 1].Controls[0];
            TextBox txtUnitRate = ((TextBox)rptFootCtrl.FindControl("txtUnitRate"));
            TextBox txtDiscount = ((TextBox)rptFootCtrl.FindControl("txtDiscount"));
            
            txtUnitRate.Focus();
        }
    }
}