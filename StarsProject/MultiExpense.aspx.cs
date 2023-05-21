using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;

namespace StarsProject
{
    public partial class MultiExpense : System.Web.UI.Page
    {
        bool _pageValid = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnExpenseID.Value = Request.QueryString["id"].ToString();

                    if (hdnExpenseID.Value == "0" || hdnExpenseID.Value == "")
                    {
                        ClearAllField();
                        BindExpenseDetail(0);
                    }
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
            Control rptFootCtrl = rptExpenseDetail.Controls[rptExpenseDetail.Controls.Count - 1].Controls[0];
            string Expense = ((DropDownList)rptFootCtrl.FindControl("drpExpense")).SelectedValue;
            string ExpenseName = ((DropDownList)rptFootCtrl.FindControl("drpExpense")).SelectedItem.Text;
            string Amount = ((TextBox)rptFootCtrl.FindControl("txtAmount")).Text;
            string Remark = ((TextBox)rptFootCtrl.FindControl("txtRemark")).Text;
            FileUpload fileUpload = ((FileUpload)rptFootCtrl.FindControl("FileUpload1"));
            Image imgVoucher = ((Image)rptFootCtrl.FindControl("imgVoucher"));
            if (fileUpload.PostedFile != null)
            {
                if (fileUpload.PostedFile.FileName.Length > 0)
                {

                    // ----------------------------------------------------------
                    if (fileUpload.HasFile)
                    {
                        string filePath = fileUpload.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;

                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                        {
                            try
                            {
                                // ---------------------------------------------------------------
                                String newFileName = "expn-" + DateTime.Now.ToString().Replace("-", "").Replace("/", "").Replace(":", "").Replace(" ", "") + "-" + filename1;
                                // ---------------------------------------------------------------
                                Random _random = new Random();
                                string orgFileName = "Random_" + _random.Next(99999).ToString() + ext;
                                // ---------------------------------------------------------------
                                DataTable dtDetail = new DataTable();
                                dtDetail = (DataTable)Session["dtDetail"];
                                Int64 cntRow = dtDetail.Rows.Count + 1;
                                DataRow dr = dtDetail.NewRow();
                                dr["ExpenseTypeID"] = Expense;
                                dr["ExpenseTypeName"] = ExpenseName;
                                dr["Amount"] = Convert.ToDecimal(Amount);
                                dr["Remarks"] = Remark;
                                dr["FromLocation"] = orgFileName;
                                dr["Voucher"] = newFileName;
                                dtDetail.Rows.Add(dr);
                                Session.Add("dtDetail", dtDetail);
                                fileUpload.SaveAs(Server.MapPath("otherimages/") + orgFileName);
                                // ---------------------------------------------------------------
                                rptExpenseDetail.DataSource = dtDetail;
                                rptExpenseDetail.DataBind();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                    }
                }
            }
        }

        public void BindExpenseDetail(Int64 ExpenseID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.OfficeExpense> lst = BAL.ExpenseMgmt.GetMultiExpenseDetailList(ExpenseID, Session["LoginUserID"].ToString());
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptExpenseDetail.DataSource = dtDetail1;
            rptExpenseDetail.DataBind();
            Session.Add("dtDetail", dtDetail1);
        }
        public void ClearAllField()
        {
            hdnExpenseID.Value = "";
            txtExpenseDate.Text = "";
            txtExpenseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtExpenseNote.Text = "";
            BindExpenseDetail(0);
            btnSave.Disabled = false;
            btnReset.Disabled = false;
        }
        public void OnlyViewControls()
        {
            txtExpenseDate.ReadOnly = true;
            txtExpenseNote.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                List<Entity.OfficeExpense> lstEntity = new List<Entity.OfficeExpense>();

                lstEntity = BAL.ExpenseMgmt.GetMultiExpenseList(Convert.ToInt64(hdnExpenseID.Value), Session["LoginUserID"].ToString());
                hdnExpenseID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "0";
                //txtExpenseDate.Text = (!String.IsNullOrEmpty(lstEntity[0].ExpenseDate.ToString())) ? lstEntity[0].ExpenseDate.ToString("dd-MM-yyyy") : "";
                txtExpenseDate.Text = lstEntity[0].ExpenseDate.ToString("yyyy-MM-dd");
                txtExpenseNote.Text = (!String.IsNullOrEmpty(lstEntity[0].ExpenseNotes)) ? lstEntity[0].ExpenseNotes.Trim() : "";

                txtExpenseDate.Focus();

                // -------------------------------------------------------------------------
                // Expense Detail
                // -------------------------------------------------------------------------
                BindExpenseDetail(Convert.ToInt64(hdnExpenseID.Value));
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            int ReturnCode = 0, ReturnExpenseId = 0, ReturnCode1 = 0;
            Int64 RetExpDetailId = 0;
            string ReturnMsg = "", ReturnMsg1 = "";

            _pageValid = true;
            string strErr = "";

            if (String.IsNullOrEmpty(txtExpenseDate.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtExpenseDate.Text))
                    strErr += "<li>" + "Expense Date is required." + "</li>";
            }

            if (_pageValid)
            {
                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0)
                    {
                        Entity.OfficeExpense objEntity = new Entity.OfficeExpense();

                        if (!String.IsNullOrEmpty(hdnExpenseID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnExpenseID.Value);

                        objEntity.ExpenseDate = Convert.ToDateTime(txtExpenseDate.Text);
                        objEntity.ExpenseNotes = (!String.IsNullOrEmpty(txtExpenseNote.Text)) ? txtExpenseNote.Text.Trim() : "";
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();

                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.ExpenseMgmt.AddUpdateMultiExpense(objEntity, out ReturnCode, out ReturnMsg, out ReturnExpenseId);
                        strErr += "<li>" + (ReturnMsg) + "</li>";
                        // --------------------------------------------------------------

                        if (ReturnCode > 0)
                        {
                            if (ReturnExpenseId > 0)
                            {
                                btnSave.Disabled = true;
                                BAL.ExpenseMgmt.DeleteMultiExpenseDetailByExpenseNo(ReturnExpenseId, out ReturnCode, out ReturnMsg);
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    Entity.OfficeExpense objExpDet = new Entity.OfficeExpense();

                                    objExpDet.pkID = 0;
                                    objExpDet.RefpkID = ReturnExpenseId;
                                    objExpDet.ExpenseTypeId = Convert.ToInt64(dr["ExpenseTypeId"]);
                                    objExpDet.Amount = Convert.ToDecimal(dr["Amount"]);
                                    objExpDet.Remarks = dr["Remarks"].ToString();
                                    objExpDet.Voucher = dr["Voucher"].ToString();
                                    objExpDet.LoginUserID = Session["LoginUserId"].ToString();
                                    objExpDet.ExpenseDateDetail = Convert.ToDateTime(dr["ExpenseDateDetail"].ToString());

                                    BAL.ExpenseMgmt.AddUpdateMultiExpenseDetail(objExpDet, out ReturnCode1, out ReturnMsg1, out RetExpDetailId);
                                    // ---------------------------------------------
                                    string filePath = dr["Voucher"].ToString();
                                    string filename1 = Path.GetFileName(filePath);
                                    string ext = Path.GetExtension(filename1).ToLower();
                                    string type = String.Empty;

                                    if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                                    {
                                        try
                                        {
                                            string file1 = Server.MapPath("otherimages/") + dr["FromLocation"].ToString();
                                            string file2 = Server.MapPath("otherimages/") + dr["Voucher"].ToString();
                                            File.Copy(file1, file2);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    else
                                        ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                                }
                                // -----------------------------------------------------
                                string rootFolderPath = Server.MapPath("otherimages");
                                string filesToDelete = @"Random_*.*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                foreach (string file in fileList)
                                {
                                    System.IO.File.Delete(file);
                                }
                                // ------------------------------------------------------
                                rptExpenseDetail.DataSource = dtDetail;
                                rptExpenseDetail.DataBind();
                                Session.Remove("dtDetail");
                            }
                        }
                    }
                }
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteExpense(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Voucher Inages
            string rootFolderPath = System.Web.HttpContext.Current.Server.MapPath("otherimages");
            string filesToDelete = @"Random_*.*";   // Only delete DOC files containing "DeleteMe" in their filenames
            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            foreach (string file in fileList)
            {
                System.IO.File.Delete(file);
            }

            DataTable dtDetail = new DataTable();
            List<Entity.OfficeExpense> lst = BAL.ExpenseMgmt.GetMultiExpenseDetailList(pkID, System.Web.HttpContext.Current.Session["LoginUserID"].ToString());
            dtDetail = PageBase.ConvertListToDataTable(lst);
            if (dtDetail != null)
            {
                if (dtDetail.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        String flName = System.Web.HttpContext.Current.Server.MapPath("otherimages/") + dr["Voucher"].ToString();
                        System.IO.File.Delete(flName);
                    }
                }
            }
            // --------------------------------- Delete Record
            BAL.ExpenseMgmt.DeleteMultiExpense(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
        protected void rptExpenseDetail_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    string strErr = "";
                    _pageValid = true;

                    if (String.IsNullOrEmpty(((DropDownList)e.Item.FindControl("drpExpense")).Text) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtAmount")).Text))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtAmount")).Text))
                        {
                            strErr += "<li>" + "Amount is required." + "</li>";
                        }
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];
                        if (dtDetail != null)
                        {
                            foreach (System.Data.DataColumn col in dtDetail.Columns) col.AllowDBNull = true;

                            DataRow dr = dtDetail.NewRow();

                            dr["pkID"] = 0;
                            long ExpTypeID = Convert.ToInt64(((DropDownList)e.Item.FindControl("drpExpense")).SelectedValue);
                            string ExpType = ((DropDownList)e.Item.FindControl("drpExpense")).SelectedItem.Text;
                            decimal Amount = Convert.ToDecimal(((TextBox)e.Item.FindControl("txtAmount")).Text);
                            string remark = ((TextBox)e.Item.FindControl("txtRemark")).Text;

                            DateTime txtExpenseDateDetail = Convert.ToDateTime(((TextBox)e.Item.FindControl("txtExpenseDateDetail")).Text);

                            //string Voucher = ((Image)e.Item.FindControl("imgVoucher")).DescriptionUrl;

                            dr["ExpenseTypeId"] = (!String.IsNullOrEmpty((ExpTypeID).ToString())) ? Convert.ToInt64(ExpTypeID) : 0;
                            dr["ExpenseTypeName"] = (!String.IsNullOrEmpty(ExpType)) ? ExpType : "";
                            dr["Amount"] = (!String.IsNullOrEmpty((Amount).ToString())) ? Convert.ToDecimal(Amount) : 0;
                            dr["Remarks"] = (!String.IsNullOrEmpty(remark)) ? remark : "";
                            dr["Voucher"] = ""; // (!String.IsNullOrEmpty(Voucher)) ? Voucher : "";
                            dr["ExpenseDateDetail"] = txtExpenseDateDetail.ToString("yyyy-MM-dd");

                            dtDetail.Rows.Add(dr);
                            dtDetail.AcceptChanges();
                            Session.Add("dtDetail", dtDetail);
                            // ---------------------------------------------------------------
                            rptExpenseDetail.DataSource = dtDetail;
                            rptExpenseDetail.DataBind();
                            // ---------------------------------------------------------------0.00


                        }
                    }
                }

            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.CommandName.ToString() == "Delete")
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];
                    // --------------------------------- Delete Record
                    string expname = ((TextBox)e.Item.FindControl("edExpense")).Text;
                    string amt = ((TextBox)e.Item.FindControl("edAmount")).Text;
                    string remark = ((TextBox)e.Item.FindControl("edRemarks")).Text;
                    string edExpenseDateDetail = ((TextBox)e.Item.FindControl("edExpenseDateDetail")).Text;

                    if (dtDetail != null)
                    {
                        foreach (DataRow dr in dtDetail.Rows)
                        {
                            if (dr["ExpenseTypeName"].ToString() == expname && dr["Amount"].ToString() == amt && dr["Remarks"].ToString() == remark)
                            {
                                String flName1 = Server.MapPath("otherimages/") + dr["FromLocation"].ToString();
                                if (File.Exists(flName1))
                                    System.IO.File.Delete(flName1);

                                String flName2 = Server.MapPath("otherimages/") + dr["Voucher"].ToString();
                                if (File.Exists(flName2))
                                    System.IO.File.Delete(flName2);
                                // -------------------------------------------------
                                dtDetail.Rows.Remove(dr);
                                // -------------------------------------------------
                                break;
                            }
                        }
                    }
                    rptExpenseDetail.DataSource = dtDetail;
                    rptExpenseDetail.DataBind();
                }
            }
        }

        protected void rptExpenseDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddl = ((DropDownList)e.Item.FindControl("drpExpense"));
                ddl.DataSource = BAL.ExpenseTypeMgmt.GetExpenseTypeList(0);
                ddl.DataValueField = "pkId";
                ddl.DataTextField = "ExpenseTypeName";
                ddl.DataBind();

            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                String rootFolderPath = System.Web.HttpContext.Current.Server.MapPath("otherimages");

                HiddenField hdnFromLocation = ((HiddenField)e.Item.FindControl("hdnFromLocation"));
                HiddenField hdnVoucher = ((HiddenField)e.Item.FindControl("hdnVoucher"));
                HtmlImage imgVoucher = ((HtmlImage)e.Item.FindControl("imgVoucher"));
                TextBox edExpenseDateDetail = ((TextBox)e.Item.FindControl("edExpenseDateDetail"));

                if (!String.IsNullOrEmpty(hdnFromLocation.Value))
                {
                    //.ToString("yyyy-MM-dd");
                }


                if (!String.IsNullOrEmpty(hdnFromLocation.Value))
                {
                    String flName1 = Server.MapPath("otherimages/") + hdnFromLocation.Value;
                    if (File.Exists(flName1))
                        imgVoucher.Src = "otherimages/" + hdnFromLocation.Value;
                }

                if (!String.IsNullOrEmpty(hdnVoucher.Value))
                {
                    String flName2 = Server.MapPath("otherimages/") + hdnVoucher.Value;
                    if (File.Exists(flName2))
                        imgVoucher.Src = "otherimages/" + hdnVoucher.Value;
                }
            }
        }

    }
}