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
    public partial class JournalVoucher : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
        public decimal totAmount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                totAmount = 0;
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetailInq", dtDetail);
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
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

                if (requestTarget.ToLower() == "drpinquiry")
                {
                }
            }
        }

        public void OnlyViewControls()
        {
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.ReadOnly = true;
            txtVoucherAmount.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            pnlDetail.Enabled = false;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        protected void rptJournal_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    HiddenField hdnCustomerIDNew = (HiddenField)e.Item.FindControl("hdnCustomerIDNew");
                    TextBox txtCustomerNameNew = (TextBox)e.Item.FindControl("txtCustomerNameNew");
                    HiddenField hdnTransType = (HiddenField)e.Item.FindControl("hdnTransType");
                    DropDownList dtTransType = (DropDownList)e.Item.FindControl("drpTransType");
                    TextBox dtVoucherAmount = (TextBox)e.Item.FindControl("txtVoucherAmount");

                    if (String.IsNullOrEmpty(hdnCustomerIDNew.Value) || String.IsNullOrEmpty(dtTransType.SelectedValue) || 
                        Convert.ToDecimal(dtVoucherAmount.Text) < 0)
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnCustomerIDNew.Value))
                            strErr += "<li>" + "A/c Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(dtTransType.Text) || Convert.ToDecimal(dtVoucherAmount.Text) < 0)
                            strErr += "<li>" + "Amount is required." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetailInq"];
                        Int64 cntRow = dtDetail.Rows.Count + 1;

                        DataRow dr = dtDetail.NewRow();

                        dr["pkID"] = 0;
                        dr["VoucherNo"] = txtVoucherNo.Text;
                        dr["CustomerID"] = (!String.IsNullOrEmpty(hdnCustomerIDNew.Value)) ? Convert.ToInt64(hdnCustomerIDNew.Value) : 0;
                        dr["CustomerName"] = txtCustomerNameNew.Text;
                        dr["TransType"] = (!String.IsNullOrEmpty(dtTransType.SelectedValue)) ? dtTransType.SelectedValue : "";
                        dr["VoucherAmount"] = (!String.IsNullOrEmpty(dtVoucherAmount.Text)) ? Convert.ToDecimal(dtVoucherAmount.Text) : 0;
                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetailInq", dtDetail);
                        // ---------------------------------------------------------------
                        rptJournal.DataSource = dtDetail;
                        rptJournal.DataBind();
                    }
                    btnSave.Focus();
                }
                if (!string.IsNullOrEmpty(strErr))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "');", true);
                }
            }
            // --------------------------------------------------------------------------
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    int ReturnCode = 0;
                    string ReturnMsg = "";
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetailInq"];

                    DataRow[] rows;
                    rows = dtDetail.Select("pkID=" + e.CommandArgument.ToString());
                    foreach (DataRow r in rows)
                        r.Delete();

                    rptJournal.DataSource = dtDetail;
                    rptJournal.DataBind();
                    BAL.FinancialTransMgmt.DeleteJournalVoucherDetail(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
                    Session.Add("dtDetailInq", dtDetail);
                }
            }
        }

        protected void rptJournal_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpTransType"));
                ddl.Items.Insert(0, new ListItem("CR", "CR"));
                ddl.Items.Insert(0, new ListItem("DB", "DB"));
                HiddenField tmpField = ((HiddenField)e.Item.FindControl("hdnTransType"));
                if (!String.IsNullOrEmpty(tmpField.Value))
                    ddl.SelectedValue = tmpField.Value;
            }

        }

        public void BindJournalVoucherDetail(string pVoucherNo)
        {
            int TotalCount1 = 0;
            DataTable dtDetail1 = new DataTable();
            List<Entity.JournalVoucherDetail> lstEntity = new List<Entity.JournalVoucherDetail>();
            lstEntity = BAL.FinancialTransMgmt.GetJournalVoucherDetailList(0, pVoucherNo, Session["LoginUserID"].ToString(), 1, 1000, out TotalCount1);
            dtDetail1 = PageBase.ConvertListToDataTable(lstEntity);
            rptJournal.DataSource = dtDetail1;
            rptJournal.DataBind();
            Session.Add("dtDetailInq", dtDetail1);
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
                // ----------------------------------------------------
                lstEntity = BAL.FinancialTransMgmt.GetJournalVoucherList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtVoucherNo.Text = lstEntity[0].VoucherNo;
                txtVoucherDate.Text = lstEntity[0].VoucherDate.ToString("yyyy-MM-dd");
                txtVoucherAmount.Text = lstEntity[0].VoucherAmount.ToString();
                txtRemarks.Text = lstEntity[0].Remarks;
                // -------------------------------------------------------------------------
                BindJournalVoucherDetail(txtVoucherNo.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            // ---------------------------------------------------
            // Resetting Temporary Table for Products
            // ---------------------------------------------------
            Session.Remove("dtDetailInq");
            DataTable dtDetail1 = new DataTable();
            //dtDetail1 = BAL.FinancialTransMgmt.GetJournalVoucherList("-1");
            Session.Add("dtDetailInq", dtDetail1);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetailInq"];

            int ReturnCode = 0;
            string ReturnMsg = "";
            string ReturnVoucherNo = "";
            string strErr = "";
            //--------------------------------------------------------------
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtVoucherDate.Text)) || String.IsNullOrEmpty(txtRemarks.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtVoucherDate.Text))
                    strErr += "<li>" + "Voucher Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtRemarks.Text))
                    strErr += "<li>" + "Remarks is required." + "</li>";

            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtVoucherDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtVoucherDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Voucher Date is Not Valid." + "</li>";
                }
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.VoucherNo = txtVoucherNo.Text;
                        objEntity.VoucherDate = Convert.ToDateTime(txtVoucherDate.Text);
                        objEntity.VoucherAmount = Convert.ToDecimal(txtVoucherAmount.Text);
                        objEntity.Remarks = txtRemarks.Text;
                        objEntity.DBC = "JOUR";
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.FinancialTransMgmt.AddUpdateJournalVoucher(objEntity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnVoucherNo) && !String.IsNullOrEmpty(txtVoucherNo.Text))
                        {
                            ReturnVoucherNo = txtVoucherNo.Text;
                        }
                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        int ReturnCode1;
                        String ReturnMsg1;

                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnVoucherNo))
                        {

                            btnSave.Disabled = true;
                            // --------------------------------------------------------------
                            //BAL.InquiryInfoMgmt.DeleteJournalVoucherByVoucherNo(ReturnVoucherNo, out ReturnCode1, out ReturnMsg1);

                            // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                            Entity.JournalVoucherDetail objEntity1 = new Entity.JournalVoucherDetail();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    objEntity1.pkID = Convert.ToInt64(dr["pkID"].ToString());
                                    objEntity1.VoucherNo = ReturnVoucherNo; 
                                    objEntity1.TransType = dr["TransType"].ToString();
                                    objEntity1.CustomerID = Convert.ToInt64(dr["CustomerID"].ToString());
                                    objEntity1.VoucherAmount = (!String.IsNullOrEmpty(dr["VoucherAmount"].ToString())) ? Convert.ToDecimal(dr["VoucherAmount"].ToString()) : 0;
                                    objEntity1.Remarks = dr["Remarks"].ToString();
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.FinancialTransMgmt.AddUpdateJournalVoucherDetail(objEntity1, out ReturnCode, out ReturnMsg);
                                }
                            }
                            if (ReturnCode > 0)
                            {
                                Session.Remove("dtDetailInq");

                            }
                        }
                    }
                }
                //else
                //{
                //    strErr += "<li>" + "Minimum One Two required !" + "</li>";
                //}
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

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtVoucherDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtVoucherNo.Text = ""; //  BAL.CommonMgmt.GetInquiryNo(txtInquiryDate.Text);
            txtRemarks.Text = "";
            txtVoucherAmount.Text = "";
            // ---------------------------------------------
            BindJournalVoucherDetail("");
            txtVoucherDate.Focus();

            btnSave.Disabled = false;
        }

        [System.Web.Services.WebMethod]
        public static string DeleteJournalVoucher(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- 
            BAL.FinancialTransMgmt.DeleteJournalVoucher(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
    }
}