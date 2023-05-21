using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlTypes;
using System.Threading;
using System.Threading.Tasks;

namespace StarsProject
{
    public partial class FinancialTrans_old : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]) && !String.IsNullOrEmpty(Request.QueryString["category"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    hdnTransCategory.Value = Request.QueryString["category"].ToString();
                    lblPageHead.InnerText = (hdnTransCategory.Value.ToLower() == "payment") ? "Financial Transaction - Manage Payment" : "Financial Transaction - Manage Receivables";
                    
                    // -----------------------------------------------------------
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
        }

        public void OnlyViewControls()
        {
            drpTransType.Attributes.Add("disabled", "disabled");
            txtTransDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtTransNotes.ReadOnly = true;
            txtTransFrom.ReadOnly = true;
            txtTransAmount.ReadOnly = true;
            txtTransID.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            List<Entity.SalesOrder> lstCustomer = new List<Entity.SalesOrder>();

            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                lstCustomer = BAL.SalesOrderMgmt.GetSalesOrderListByCustomer(Session["LoginUserID"].ToString(), Convert.ToInt64(hdnCustomerID.Value), "", 0, 0);
                drpOrder.DataSource = lstCustomer;
                drpOrder.DataValueField = "OrderNo";
                drpOrder.DataTextField = "OrderNo";
                drpOrder.DataBind();
                drpOrder.Items.Insert(0, new ListItem("-- Select Order --", ""));
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.FinancialTrans> lstEntity = new List<Entity.FinancialTrans>();
                // -----------------------------------------------------------------------------------
                //lstEntity = BAL.FinancialTransMgmt.GetFinancialTransList(Convert.ToInt64(hdnpkID.Value), hdnTransCategory.Value, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                //hdnpkID.Value = lstEntity[0].pkID.ToString();
                //hdnTransCategory.Value = lstEntity[0].TransCategory.ToString();
                //hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                //txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                //txtTransDate.Text = lstEntity[0].TransDate.ToString("dd-MM-yyyy");
                //drpTransType.SelectedValue = lstEntity[0].TransType.ToString();
                //txtTransAmount.Text = lstEntity[0].TransAmount.ToString();
                //txtTransFrom.Text = lstEntity[0].TransFrom.ToString();
                //txtTransID.Text = lstEntity[0].TransID.ToString();
                //txtTransNotes.Text = (!String.IsNullOrEmpty(lstEntity[0].TransNotes)) ? lstEntity[0].TransNotes : "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            _pageValid = true;

            wowList.Style.Remove("color");
            wowList.Style.Add("color", "Navy");

            if ((String.IsNullOrEmpty(txtCustomerName.Text) && String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0") || String.IsNullOrEmpty(txtTransDate.Text) || String.IsNullOrEmpty(drpTransType.SelectedValue))
            {
                _pageValid = false;
                wowList.Style.Remove("color");
                wowList.Style.Add("color", "red");

                if (String.IsNullOrEmpty(txtCustomerName.Text) && String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    wowList.Controls.Add(new LiteralControl("<li>" + "Customer Selection is required." + "</li>"));

                if (String.IsNullOrEmpty(drpTransType.SelectedValue))
                    wowList.Controls.Add(new LiteralControl("<li>" + "Transaction Type Selection is required." + "</li>"));

                if (String.IsNullOrEmpty(txtTransDate.Text))
                    wowList.Controls.Add(new LiteralControl("<li>" + "Transaction Date is required." + "</li>"));

            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.FinancialTrans objEntity = new Entity.FinancialTrans();

                //if (!String.IsNullOrEmpty(hdnpkID.Value))
                //    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                //objEntity.TransCategory = hdnTransCategory.Value.ToUpper();
                //objEntity.TransType = drpTransType.SelectedValue;
                //objEntity.TransDate = Convert.ToDateTime(txtTransDate.Text);
                //objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                //objEntity.TransNotes = txtTransNotes.Text;
                //objEntity.TransAmount = Convert.ToDecimal(txtTransAmount.Text);
                //objEntity.TransFrom = txtTransFrom.Text;
                //objEntity.TransID = txtTransID.Text;
                //objEntity.TransMode = "";
                //objEntity.OrderNo = drpOrder.SelectedValue;
                //objEntity.LoginUserID = Session["LoginUserID"].ToString();
                //// -------------------------------------------------------------- Insert/Update Record
                //BAL.FinancialTransMgmt.AddUpdateFinancialTrans(objEntity, out ReturnCode, out ReturnMsg);
                //divErrorMessage.InnerHtml = @ReturnMsg;
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtTransDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtTransNotes.Text = "";
            txtTransFrom.Text = "";
            txtTransAmount.Text = "";
            txtTransID.Text = "";
            txtCustomerName.Focus();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            BindDropDown();
            drpOrder.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteFinancialTrans(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FinancialTransMgmt.DeleteFinancialTrans(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
           // int TotalCount;
           // List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
           // lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(Convert.ToInt64(drpTransType.SelectedValue), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
           // -----------------------------------------------------------------------------------
           //divStatus.Visible = (!String.IsNullOrEmpty(drpTransType.SelectedValue)) ? true : false;
           // if (lstEntity.Count > 0)
           // {
           //     drpInquiryStatus.SelectedValue = lstEntity[0].InquiryStatusID.ToString();
           // }
        }
    }
}