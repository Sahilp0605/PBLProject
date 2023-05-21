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
    public partial class Expense : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDown();

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnExpenseID.Value = Request.QueryString["id"].ToString();
                    hdnSerialKey.Value = HttpContext.Current.Session["SerialKey"].ToString();
                    if (hdnExpenseID.Value == "0" || hdnExpenseID.Value == "")
                    {
                        ClearAllField();
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
                        drpExpenseType_SelectedIndexChanged(null, null);
                    }
                }
            }
            if (uploadDocument.PostedFile != null)
            {
                if (uploadDocument.PostedFile.FileName.Length > 0)
                {

                    // ----------------------------------------------------------
                    if (uploadDocument.HasFile)
                    {
                        string filePath = uploadDocument.PostedFile.FileName;
                        string filename1 = Path.GetFileName(filePath);
                        string ext = Path.GetExtension(filename1).ToLower();
                        string type = String.Empty;

                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
                        {
                            try
                            {
                                string rootFolderPath = Server.MapPath("otherimages");
                                string filesToDelete = @"expn-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                foreach (string file in fileList)
                                {
                                    System.IO.File.Delete(file);
                                }
                                // -----------------------------------------------------
                                String flname = "expn-" + filename1;
                                String tmpFile = Server.MapPath("otherimages/") + flname;
                                uploadDocument.PostedFile.SaveAs(tmpFile);
                                // ---------------------------------------------------------------
                                DataTable dtDocs = new DataTable();
                                dtDocs = (DataTable)Session["dtDocs"];
                                Int64 cntRow = dtDocs.Rows.Count + 1;
                                DataRow dr = dtDocs.NewRow();
                                dr["pkID"] = cntRow;
                                dr["Name"] = flname;
                                dr["Type"] = type;
                                dtDocs.Rows.Add(dr);
                                Session.Add("dtDocs", dtDocs);
                                // ---------------------------------------------------------------
                                rptFileListCtrl.DataSource = dtDocs;
                                rptFileListCtrl.DataBind();
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
            //else
            //{
            //    if (FileUpload1.PostedFile != null)
            //    {
            //        if (FileUpload1.HasFile)
            //        {
            //            string filePath = FileUpload1.PostedFile.FileName;
            //            string filename1 = Path.GetFileName(filePath);
            //            string ext = Path.GetExtension(filename1);
            //            string type = String.Empty;
            //            // ----------------------------------------------------------
            //            if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg")
            //            {
            //                //if (!String.IsNullOrEmpty(hdnpkID.Value.Trim()))
            //                //{
            //                string rootFolderPath = Server.MapPath("otherimages");
            //                string filesToDelete = @"expn-" + hdnExpenseID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
            //                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            //                foreach (string file in fileList)
            //                {
            //                    //System.Diagnostics.Debug.WriteLine(file + "  Will be deleted");
            //                    System.IO.File.Delete(file);
            //                }
            //                // -----------------------------------------------------
            //                String flname = "expn-" + hdnExpenseID.Value.Trim() + ext;
            //                FileUpload1.SaveAs(Server.MapPath("otherimages/") + flname);
            //                imgProduct.ImageUrl = "";
            //                imgProduct.ImageUrl = "otherimages/" + flname;
            //                //}
            //            }
            //            else
            //                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
            //        }
            //    }
            //}
        }
        public void BindExpenseImages(Int64 ExpenseID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.Expense> lst = BAL.ExpenseMgmt.GetExpenseImageList(0, ExpenseID);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptFileListCtrl.DataSource = dtDetail1;
            rptFileListCtrl.DataBind();
            Session.Add("dtDocs", dtDetail1);
        }
        public void BindDropDown()
        {
            // ---------------- OtherCharge List -------------------------------------
            List<Entity.ExpenseType> lstExpense = new List<Entity.ExpenseType>();
            lstExpense = BAL.ExpenseTypeMgmt.GetExpenseTypeList(0);
            drpExpenseType.DataSource = lstExpense;
            drpExpenseType.DataValueField = "pkId";
            drpExpenseType.DataTextField = "ExpenseTypeName";
            drpExpenseType.DataBind();
            drpExpenseType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Expense Type --", ""));
        }
        public void ClearAllField()
        {
            hdnExpenseID.Value = "";
            txtExpenseDate.Text = "";
            txtCustomer.Text = "";
            txtExpenseDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            drpExpenseType.SelectedValue = "";
            txtAmount.Text = "";
            txtExpenseNote.Text = "";
            txtFromLocation.Text = "";
            txtToLocation.Text = "";

            BindExpenseImages(0);

            btnSave.Disabled = false;

        }

        public void OnlyViewControls()
        {
            txtExpenseDate.ReadOnly = true;
            drpExpenseType.Attributes.Add("disabled", "disabled");
            txtAmount.ReadOnly = true;
            txtCustomer.ReadOnly = true;
            txtExpenseNote.ReadOnly = true;
            txtFromLocation.ReadOnly = true;
            txtToLocation.ReadOnly = true;

            //FileUpload1.Enabled = false;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }
        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                List<Entity.Expense> lstEntity = new List<Entity.Expense>();

                lstEntity = BAL.ExpenseMgmt.GetExpense(Convert.ToInt64(hdnExpenseID.Value), Session["LoginUserID"].ToString());
                hdnExpenseID.Value = (!String.IsNullOrEmpty(lstEntity[0].pkID.ToString())) ? lstEntity[0].pkID.ToString() : "";
                //txtExpenseDate.Text = (!String.IsNullOrEmpty(lstEntity[0].ExpenseDate.ToString())) ? lstEntity[0].ExpenseDate.ToString("dd-MM-yyyy") : "";
                txtExpenseDate.Text = lstEntity[0].ExpenseDate.ToString("yyyy-MM-dd");
                drpExpenseType.SelectedValue = (lstEntity[0].ExpenseTypeId > 0) ? lstEntity[0].ExpenseTypeId.ToString() : "";
                txtCustomer.Text = (!String.IsNullOrEmpty(lstEntity[0].CustomerName.ToString()) ? lstEntity[0].CustomerName.Trim() : "");
                txtAmount.Text = (!String.IsNullOrEmpty(lstEntity[0].Amount.ToString())) ? Convert.ToDecimal(lstEntity[0].Amount).ToString() : "0";
                txtExpenseNote.Text = (!String.IsNullOrEmpty(lstEntity[0].ExpenseNotes)) ? lstEntity[0].ExpenseNotes.Trim() : "";
                txtFromLocation.Text = (!String.IsNullOrEmpty(lstEntity[0].FromLocation)) ? lstEntity[0].FromLocation.Trim() : "";
                txtToLocation.Text = (!String.IsNullOrEmpty(lstEntity[0].ToLocation)) ? lstEntity[0].ToLocation.Trim() : "";
                txtDistanceCovered.Text = (!String.IsNullOrEmpty(lstEntity[0].DistanceCovered.ToString())) ? lstEntity[0].DistanceCovered.ToString() : "";
                //imgProduct.ImageUrl = lstEntity[0].ExpenseImage;
                txtExpenseDate.Focus();

                // -------------------------------------------------------------------------
                // Expense Images
                // -------------------------------------------------------------------------
                BindExpenseImages(Convert.ToInt64(hdnExpenseID.Value));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnExpenseId = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "";

            _pageValid = true;
            string strErr = "";
            if (String.IsNullOrEmpty(txtAmount.Text) || txtAmount.Text == "0" || String.IsNullOrEmpty(txtExpenseDate.Text) || String.IsNullOrEmpty(drpExpenseType.SelectedValue) || String.IsNullOrEmpty(txtExpenseNote.Text))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtExpenseDate.Text))
                    strErr += "<li>" + "Expense Date is required." + "</li>";

                if (String.IsNullOrEmpty(drpExpenseType.SelectedValue))
                    strErr += "<li>" + "Expense Type is required." + "</li>";

                if (String.IsNullOrEmpty(txtAmount.Text) || txtAmount.Text == "0")
                    strErr += "<li>" + "Amount is required." + "</li>";

                if (String.IsNullOrEmpty(txtExpenseNote.Text))
                    strErr += "<li>" + "Expense Note is required." + "</li>";

                if (drpExpenseType.SelectedValue == "1")
                {
                    if (String.IsNullOrEmpty(txtFromLocation.Text))
                        strErr += "<li>" + "From Location is required." + "</li>";

                    if (String.IsNullOrEmpty(txtToLocation.Text))
                        strErr += "<li>" + "To Location is required." + "</li>";
                }
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtExpenseDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtExpenseDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Expense Date is Not Valid." + "</li>";
                }
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Expense objEntity = new Entity.Expense();

                if (!String.IsNullOrEmpty(hdnExpenseID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnExpenseID.Value);

                objEntity.ExpenseDate = Convert.ToDateTime(txtExpenseDate.Text);
                objEntity.ExpenseTypeId = Convert.ToInt64(drpExpenseType.SelectedValue);
                objEntity.CustomerName = (!String.IsNullOrEmpty(txtCustomer.Text) ? txtCustomer.Text.Trim() : "");
                objEntity.Amount = (!String.IsNullOrEmpty(txtAmount.Text)) ? Convert.ToDecimal(txtAmount.Text) : 0;
                objEntity.ExpenseNotes = (!String.IsNullOrEmpty(txtExpenseNote.Text)) ? txtExpenseNote.Text.Trim() : "";
                objEntity.FromLocation = (!String.IsNullOrEmpty(txtFromLocation.Text)) ? txtFromLocation.Text.Trim() : "";
                objEntity.ToLocation = (!String.IsNullOrEmpty(txtToLocation.Text)) ? txtToLocation.Text.Trim() : "";
                objEntity.DistanceCovered = (!String.IsNullOrEmpty(txtDistanceCovered.Text)) ? Convert.ToDecimal(txtDistanceCovered.Text) : 0;
                //objEntity.ExpenseImage = imgProduct.ImageUrl;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();

                // -------------------------------------------------------------- Insert/Update Record
                BAL.ExpenseMgmt.AddUpdateExpense(objEntity, out ReturnCode, out ReturnMsg, out  ReturnExpenseId);
                strErr += "<li>" + ReturnMsg + "</li>";
                // --------------------------------------------------------------

                if (ReturnCode > 0)
                {
                    if (ReturnExpenseId > 0)
                    {
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Documents
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        BAL.ExpenseMgmt.DeleteExpenseImageByExpenseID(Convert.ToInt64(ReturnExpenseId), out ReturnCode1, out ReturnMsg1);

                        string filePath, filename1, ext, type;
                        Byte[] bytes;
                        long ExpenseId;
                        DataTable dtDocs = new DataTable();
                        dtDocs = (DataTable)Session["dtDocs"];

                        if (dtDocs != null)
                        {
                            foreach (DataRow dr in dtDocs.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    Entity.Expense objEntity1 = new Entity.Expense();
                                    objEntity1.ExpenseID = ReturnExpenseId;
                                    objEntity1.Name = dr["Name"].ToString();
                                    objEntity1.Type = dr["type"].ToString();
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.ExpenseMgmt.AddUpdateExpenseImages(objEntity1, out ReturnCode1, out ReturnMsg1);
                                    strErr += "<li>" + ReturnMsg + "</li>";
                                }
                            }
                        }

                    }
                    // ------------------------------------------------
                    if (ReturnCode > 0)
                    {
                        btnSave.Disabled = true;
                        Session.Remove("dtDocs");
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
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteExpense(string ExpenseID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ExpenseMgmt.DeleteExpense(Convert.ToInt64(ExpenseID), out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpExpenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!String.IsNullOrEmpty(drpExpenseType.SelectedValue)) && Convert.ToInt64(drpExpenseType.SelectedValue) > 0)
            {
                List<Entity.ExpenseType> lstExpense = new List<Entity.ExpenseType>();
                lstExpense = BAL.ExpenseTypeMgmt.GetExpenseTypeList(Convert.ToInt64(drpExpenseType.SelectedValue));

                bool location = (!String.IsNullOrEmpty(lstExpense[0].IsLocationRequired.ToString())) ? lstExpense[0].IsLocationRequired : false;

                if (location == true)
                    divLocation.Visible = true;

            }
        }
        protected void rptFileListCtrl_ItemCommand(object source, RepeaterCommandEventArgs e)
        {


            //List<Entity.Documents> lstFiles = new List<Entity.Documents>();
            //lstFiles = BAL.CommonMgmt.GetDocumentsList(Convert.ToInt64(e.CommandArgument.ToString()), 0);

            //if (lstFiles.Count > 0)
            //{
            //    if (e.CommandName.ToString() == "Delete")
            //    {
            //        int ReturnCode = 0;
            //        string ReturnMsg = "";
            //        // -------------------------------------------------------------- Delete Record
            //        BAL.ExpenseMgmt.DeleteExpenseImage(Convert.ToInt64(e.CommandArgument.ToString()), out ReturnCode, out ReturnMsg);
            //        ScriptManager.RegisterStartupScript(this, typeof(string), "msg", "javascript:showmessage('" + ReturnMsg + "');", true);
            //        if (!String.IsNullOrEmpty(hdnExpenseID.Value))
            //        {
            //            BindExpenseImages(Convert.ToInt64(hdnExpenseID.Value));
            //        }
            //        // -------------------------------------------------------------
            //        //foreach (Entity.Documents tmpObj in lstFiles)
            //        //{
            //        //if (tmpObj.pkID == Convert.ToInt64(e.CommandArgument.ToString()))
            //        //{
            //        string rootFolderPath = Server.MapPath("productimages");
            //        string filesToDelete = @lstFiles[0].FileName;
            //        string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            //        foreach (string file in fileList)
            //        {
            //            System.IO.File.Delete(file);
            //        }
            //        frameDoc.Attributes.Remove("scr");
            //        frameDoc.Attributes.Add("src", "images/buttons/Preview.png");
            //        //}
            //        //}
            //    }
            //    if (e.CommandName.ToString() == "Preview")
            //    {
            //        if (lstFiles.Count > 0)
            //        {
            //            string filePath = "productimages/" + lstFiles[0].FileName;
            //            frameDoc.Attributes.Add("src", filePath);
            //        }
            //    }
            //}
            //else
            //{
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtDocs = (DataTable)Session["dtDocs"];
                for (int i = dtDocs.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtDocs.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                    {
                        //string rootFolderPath = Server.MapPath("otherimages");
                        //string filesToDelete = System.IO.Path.GetFileNameWithoutExtension(dr["Name"].ToString()) + ".*";   
                        //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                        //foreach (string file in fileList)
                        //{
                        //    System.IO.File.Delete(file);
                        //}

                        dr.Delete();
                    }
                        
                }
                dtDocs.AcceptChanges();
                Session.Add("dtDocs", dtDocs);

                frameDoc.Attributes.Remove("scr");
                frameDoc.Attributes.Add("src", "images/buttons/Preview.png");

                rptFileListCtrl.DataSource = dtDocs;
                rptFileListCtrl.DataBind();
            }
            if (e.CommandName.ToString() == "Preview")
            {
                DataTable dtDocs = (DataTable)Session["dtDocs"];
                for (int i = dtDocs.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtDocs.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                    {
                        string filePath = "otherimages/" + dr["Name"];
                        frameDoc.Attributes.Add("src", filePath);
                    }
                }
            }
            //}

        }
    }
}