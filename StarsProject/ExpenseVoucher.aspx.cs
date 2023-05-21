using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace StarsProject
{
    public partial class ExpenseVoucher : System.Web.UI.Page
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
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

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
            else
            {
                // ----------------------------------------------------------------------
                // Documents Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (uploadDocument.PostedFile != null)
                {
                    if (uploadDocument.HasFile)
                    {
                        string filePath = uploadDocument.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;
                        // ----------------------------------------------------------
                        if (ext == ".pdf")
                        {
                            string rootFolderPath = Server.MapPath("EmployeeDocs");
                            string filesToDelete = @"Exp-"  + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                            foreach (string file in fileList)
                            {
                                System.IO.File.Delete(file);
                            }
                            // -----------------------------------------------------
                            String flname = "Exp-"  + filename1;
                            uploadDocument.SaveAs(Server.MapPath("EmployeeDocs/") + flname);
                            imgEmployee.ImageUrl = "";
                            imgEmployee.ImageUrl = "EmployeeDocs/" + flname;
                            hdnFileName.Value = "EmployeeDocs/" + flname;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Product Image Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showFileExtError('image');", true);
                    }
                }
            }
            }

        public void OnlyViewControls()
        {
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.ReadOnly = true;
            txtDBCustomerName.ReadOnly = true;
            txtCRCustomerName.ReadOnly = true;
            txtVoucherAmount.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.JournalVoucher> lstEntity = new List<Entity.JournalVoucher>();
                // -----------------------------------------------------------------------------------

                lstEntity = BAL.FinancialTransMgmt.GetExpenseVoucherList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();

                txtVoucherNo.Text = lstEntity[0].VoucherNo.ToString();
                txtVoucherDate.Text = (Request.QueryString["mode"].ToString() != "continue") ? lstEntity[0].VoucherDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnDBCustomerID.Value = lstEntity[0].DBCustomerID.ToString();
                txtDBCustomerName.Text = lstEntity[0].DBCustomerName.ToString();
                hdnCRCustomerID.Value = lstEntity[0].CRCustomerID.ToString();
                txtCRCustomerName.Text = lstEntity[0].CRCustomerName.ToString();
                txtVoucherAmount.Text = lstEntity[0].VoucherAmount.ToString();
                txtRemarks.Text = lstEntity[0].Remarks.ToString();
                imgEmployee.ImageUrl = Server.MapPath("EmployeeDocs/") + (lstEntity[0].FileName.ToString()).Substring(13, (lstEntity[0].FileName.ToString()).Length - 13);
                hdnFileName.Value = (lstEntity[0].FileName.ToString());
                //imgEmployee.ImageUrl = lstEntity[0].FileName.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "", ReturnVoucherNo = "";
            string strErr = "";
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtVoucherDate.Text) || String.IsNullOrEmpty(txtVoucherAmount.Text) ? 0 : Convert.ToDecimal(txtVoucherAmount.Text)) == 0 ||
                String.IsNullOrEmpty(hdnDBCustomerID.Value) || String.IsNullOrEmpty(hdnCRCustomerID.Value))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtVoucherDate.Text))
                    strErr += "<li>" + "Voucher Date is required. " + "</li>";

                if (String.IsNullOrEmpty(hdnDBCustomerID.Value))
                    strErr += "<li>" + "Please Select Proper Debit Account " + "</li>";

                if (String.IsNullOrEmpty(hdnCRCustomerID.Value))
                    strErr += "<li>" + "Please Select Proper Credit Account " + "</li>";

                if ((String.IsNullOrEmpty(txtVoucherAmount.Text) ? 0 : Convert.ToDecimal(txtVoucherAmount.Text)) == 0)
                    strErr += "<li>" + "Voucher amount must be greater than zero" + "</li>";

                if (String.IsNullOrEmpty(txtRemarks.Text))
                    strErr += "<li>" + "Transaction Notes is required. " + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.JournalVoucher objEntity = new Entity.JournalVoucher();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.VoucherNo = (!String.IsNullOrEmpty(txtVoucherNo.Text)) ? txtVoucherNo.Text : "";
                objEntity.VoucherDate = Convert.ToDateTime(txtVoucherDate.Text);
                objEntity.DBCustomerID = (!String.IsNullOrEmpty(hdnDBCustomerID.Value)) ? Convert.ToInt64(hdnDBCustomerID.Value) : 0;
                objEntity.CRCustomerID = (!String.IsNullOrEmpty(hdnCRCustomerID.Value)) ? Convert.ToInt64(hdnCRCustomerID.Value) : 0;
                objEntity.VoucherAmount = (!String.IsNullOrEmpty(txtVoucherAmount.Text)) ? Convert.ToDecimal(txtVoucherAmount.Text) : 0;
                objEntity.Remarks = (!String.IsNullOrEmpty(txtRemarks.Text)) ? txtRemarks.Text : "";
                objEntity.FileName = imgEmployee.ImageUrl;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                //uploadDocument.SaveAs(Server.MapPath("EmployeeDocs/") + (imgEmployee.ImageUrl).Substring(13, (imgEmployee.ImageUrl).Length - 13));
                
                // -------------------------------------------------------------- Insert/Update Record
                BAL.FinancialTransMgmt.AddUpdateExpenseVoucher(objEntity, out ReturnCode, out ReturnMsg, out ReturnVoucherNo);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnDBCustomerID.Value = "";
            hdnCRCustomerID.Value = "";
            txtVoucherNo.Text = "";
            txtVoucherDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDBCustomerName.Text = "";
            txtCRCustomerName.Text = "";
            txtVoucherAmount.Text = "";
            txtRemarks.Text = "";
            btnSave.Disabled = false;
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        [System.Web.Services.WebMethod]
        public static string DeleteExpenseVoucher(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FinancialTransMgmt.DeleteExpenseVoucher(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {

        }
    }
}