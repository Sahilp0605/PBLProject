using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using QRCoder;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using iTextSharp.text.html;

namespace StarsProject
{
    public partial class QuatationFollowUp : System.Web.UI.Page
    {
        //Boolean _pageValid = true;
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    //Session.Remove("dtQuatationFollowup");
        //    if (!IsPostBack)
        //    {
        //        if (!String.IsNullOrEmpty(Request.QueryString["QuotID"]))
        //        {
        //            hdnQuatationID.Value = Request.QueryString["QuotID"].ToString();
        //            if (hdnQuatationID.Value == "0" || hdnQuatationID.Value == "")
        //                ClearAllField();
        //            else
        //            {
        //                BindQuatFollowup();
        //            }
        //        }
        //        List<Entity.Quotation> lstEntityLog = new List<Entity.Quotation>();
        //        lstEntityLog = BAL.QuotationMgmt.GetQuatationLogList(hdnQuatationID.Value);
        //        rptQuatationFollowUp.DataSource = lstEntityLog;
        //        rptQuatationFollowUp.DataBind();

        //        rptQuatationFollowupLog.DataSource = lstEntityLog;
        //        rptQuatationFollowupLog.DataBind();
        //    }

        //    if (UploadFile.PostedFile != null)
        //    {
        //        if (UploadFile.PostedFile.FileName.Length > 0)
        //        {
        //            if (UploadFile.HasFile)
        //            {
        //                string filePath = UploadFile.PostedFile.FileName;
        //                string filename1 = Path.GetFileName(filePath);
        //                string ext = Path.GetExtension(filename1);
        //                string type = String.Empty;
        //                Int64 FollowupID = BAL.QuotationMgmt.GetMaxQuatationFollowupID(Convert.ToInt64(hdnQuatationID.Value));
        //                string QuatationNo = BAL.CommonMgmt.GetQuotationNo(Convert.ToInt64(hdnQuatationID.Value));

        //                txtFollowupNo.Text = FollowupID.ToString();

        //                if (ext.ToLower() == ".pdf" || ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg")
        //                {
        //                    try
        //                    {
        //                        Int64 intcnt = 0;
        //                        string rootFolderPath = Server.MapPath("QuatationFollowup");
        //                        string filesToDelete = QuatationNo + "-" + FollowupID + "-File-*.*";   // Only delete DOC files containing "DeleteMe" in their filenames
        //                        string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
        //                        foreach (string file in fileList)
        //                        {
        //                            //System.IO.File.Delete(file);
        //                            intcnt = intcnt + 1;
        //                        }

        //                        DataTable dtQuatationFollowup = new DataTable();
        //                        dtQuatationFollowup = (DataTable)Session["dtQuatationFollowup"];
        //                        Int64 cntRow = dtQuatationFollowup.Rows.Count + 1;
        //                        DataRow dr = dtQuatationFollowup.NewRow();
        //                        // -----------------------------------------------------
        //                        String flname = QuatationNo + "-" + FollowupID + "-File-" + cntRow + ext;
        //                        String tmpFile = Server.MapPath("QuatationFollowup/") + flname;
        //                        UploadFile.PostedFile.SaveAs(tmpFile);
        //                        // -----------------------------------------------------
        //                        dr["pkID"] = intcnt;
        //                        dr["FollowUpID"] = FollowupID;
        //                        dr["QuatationID"] = hdnQuatationID.Value;
        //                        dr["Remark"] = txtRemarks.Text;
        //                        dr["FileName"] = ("QuatationFollowup/") + flname;
        //                        dr["CreatedBy"] = Session["LoginUserID"].ToString();
        //                        dr["CreatedDate"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
        //                        dtQuatationFollowup.Rows.Add(dr);
        //                        Session.Add("dtQuatationFollowup", dtQuatationFollowup);
        //                        // ---------------------------------------------------------------
        //                        DataRow[] drr = dtQuatationFollowup.Select("FollowUpID <> " + FollowupID);
        //                        foreach (var row in drr)
        //                            row.Delete();
        //                        dtQuatationFollowup.AcceptChanges();
        //                        // ---------------------------------------------------------------
        //                        rptQuatationFollowUp.DataSource = dtQuatationFollowup;
        //                        rptQuatationFollowUp.DataBind();
        //                        btnSave_Click(sender, e);
        //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
        //                    }
        //                    catch (Exception ex) { }
        //                }
        //                else
        //                    ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
        //            }
        //        }
        //    }
        //}
        //public void BindQuatFollowup()
        //{
        //    DataTable dtDetail1 = new DataTable();
        //    List<Entity.Quotation> lst = BAL.QuotationMgmt.GetQuatationLogList(hdnQuatationID.Value);
        //    dtDetail1 = PageBase.ConvertListToDataTable(lst);
        //    rptQuatationFollowUp.DataSource = dtDetail1;
        //    rptQuatationFollowUp.DataBind();
        //    Session.Add("dtQuatationFollowup", dtDetail1);

        //    rptQuatationFollowupLog.DataSource = dtDetail1;
        //    rptQuatationFollowUp.DataBind();
        //}

        //public void ClearAllField()
        //{
        //    txtFollowupNo.Text = "";
        //    txtRemarks.Text = "";
        //    rptQuatationFollowUp.DataSource = null;
        //    rptQuatationFollowUp.DataBind();

        //}
        //protected void rptQuatationFollowUp_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName.ToString() == "Delete")
        //    {
        //        DataTable dtQuatationFollowup = (DataTable)Session["dtQuatationFollowup"];
        //        for (int i = dtQuatationFollowup.Rows.Count - 1; i >= 0; i--)
        //        {
        //            DataRow dr = dtQuatationFollowup.Rows[i];
        //            if (dr["RowNum"].ToString() == e.CommandArgument.ToString())
        //                dr.Delete();
        //        }
        //        dtQuatationFollowup.AcceptChanges();
        //        Session.Add("dtQuatationFollowup", dtQuatationFollowup);
        //        rptQuatationFollowUp.DataSource = dtQuatationFollowup;
        //        rptQuatationFollowUp.DataBind();
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
        //    }
        //}

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{

        //}

        //protected void btnReset_Click(object sender, EventArgs e)
        //{
        //    ClearAllFields();
        //}

        //public void ClearAllFields()
        //{
        //    txtFollowupNo.Text = "";
        //    txtRemarks.Text = "";
        //    Session["dtQuatationFollowup"] = null;
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    _pageValid = true;
        //    string strError = "", ReturnMsg1 = "";
        //    int ReturnCode1 = 0;

        //    if (String.IsNullOrEmpty(txtFollowupNo.Text))
        //    {
        //        _pageValid = false;

        //        strError += "<li>" + "Select Followup No" + "</li>";
        //        txtFollowupNo.Focus();
        //    }
        //    if (_pageValid)
        //    {
        //        DataTable dtQuatationFollowup = new DataTable();
        //        dtQuatationFollowup = (DataTable)Session["dtQuatationFollowup"];

        //        if(dtQuatationFollowup != null)
        //        {
        //            if (dtQuatationFollowup.Rows.Count > 0)
        //            {
        //                Entity.Quotation objEntity = new Entity.Quotation();
        //                foreach (DataRow dr in dtQuatationFollowup.Rows)
        //                {
        //                    objEntity.FollowUpID = Convert.ToInt64(dr["FollowUpID"]);
        //                    objEntity.QuatationID = Convert.ToInt64(dr["QuatationID"]);
        //                    objEntity.Remark = dr["Remark"].ToString();
        //                    objEntity.FileName = dr["FileName"].ToString();
        //                    objEntity.CreatedBy = Session["LoginUserID"].ToString();

        //                    BAL.QuotationMgmt.AddUpdateQuatationLog(objEntity, out ReturnCode1, out ReturnMsg1);
        //                    strError += "<li>" + ReturnMsg1 + "</li>";
        //                    //BindQuatFollowup();
        //                }

        //                btnSave.Disabled = true;
        //                //btnReset.Disabled = true;

        //                if (!String.IsNullOrEmpty(strError))
        //                {
        //                    if (ReturnCode1 > 0)
        //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-success');", true);
        //                    else
        //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-danger');", true);
        //                }
        //            }
        //        }
        //    }
        //}

        int ReturnCode = 0, ReturnCode1 = 0, totrec = 0;
        string ReturnMsg = "", ReturnMsg1 = "";
        string strErr = "";
        Boolean _pageValid = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                // --------------------------------------------------------
                //BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    hdnLogID.Value = Request.QueryString["id"].ToString();
                //if (!String.IsNullOrEmpty(Request.QueryString["orderno"]))
                //    hdnQuatationNo.Value = Request.QueryString["orderno"].ToString();
                if (!String.IsNullOrEmpty(Request.QueryString["QuotID"]))
                {
                    hdnQuatationpkID.Value = Request.QueryString["QuotID"].ToString();
                }
                List<Entity.Quotation> lstEntityLog = new List<Entity.Quotation>();
                lstEntityLog = BAL.QuotationMgmt.GetQuatationLogList(hdnQuatationpkID.Value);
                rptQuatationFollowUp.DataSource = lstEntityLog;
                rptQuatationFollowUp.DataBind();

                rptQuatationFollowupLog.DataSource = lstEntityLog;
                rptQuatationFollowupLog.DataBind();
            }
            else
            {
                //if (String.IsNullOrEmpty(txtRemarks.Text))
                //{
                //    _pageValid = false;

                //    if (String.IsNullOrEmpty(txtRemarks.Text))
                //        strErr += "<li>" + "Remark is required. " + "</li>";
                //}
                // --------------------------------------------------------------
                if (_pageValid)
                {
                    // ----------------------------------------------------------------------
                    // Product Document Upload On .... Page Postback
                    // ----------------------------------------------------------------------
                    if (UploadFile.PostedFile != null)
                    {
                        if (UploadFile.PostedFile.FileName.Length > 0)
                        {
                            if (UploadFile.HasFile)
                            {
                                string filePath = UploadFile.PostedFile.FileName;
                                string filename1 = Path.GetFileName(filePath);
                                string ext = Path.GetExtension(filename1);
                                string type = String.Empty;
                                string[] flArray = filename1.Split('.');
                                string onlyFileName = "";
                                if (flArray.Length > 0)
                                    onlyFileName = flArray[0].Replace(".", "").Replace(" ", "");

                                if (ext.ToLower() == ".pdf" || ext.ToLower() == ".bmp" || ext.ToLower() == ".gif" || ext.ToLower() == ".png" || ext.ToLower() == ".jpg" || ext.ToLower() == ".jpeg")
                                {
                                    try
                                    {
                                        // -----------------------------------------------------
                                        Entity.Quotation objEntity = new Entity.Quotation();

                                        if (String.IsNullOrEmpty(hdnLogID.Value) || hdnLogID.Value == "0")
                                        {
                                            objEntity.QuatationID = Convert.ToInt64(hdnQuatationpkID.Value);
                                            objEntity.Remark = txtRemarks.Text;
                                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                                            // -------------------------------------------------------------- Insert/Update Record
                                            BAL.QuotationMgmt.AddUpdateQuatationLog(objEntity, out ReturnCode, out ReturnMsg);
                                            if (ReturnCode > 0)
                                            {
                                                hdnLogID.Value = ReturnCode.ToString();
                                                txtFollowupNo.Text = ReturnCode.ToString();
                                            }
                                        }
                                        if (ReturnCode > 0 || (!String.IsNullOrEmpty(hdnLogID.Value) && hdnLogID.Value != "0" && !String.IsNullOrEmpty(onlyFileName)))
                                        {
                                            //string QuatationNo = BAL.CommonMgmt.GetQuotationNo(Convert.ToInt64(hdnQuatationpkID.Value));
                                            string QuatationNo = BAL.CommonMgmt.GetSalesOrderNo(Convert.ToInt64(hdnQuatationpkID.Value));
                                            string rootFolderPath = Server.MapPath("QuatationFollowup");
                                            string filesToDelete = QuatationNo + "-" + hdnLogID.Value + "-*.*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                            
                                            // -----------------------------------------------------
                                            String flname = "";
                                            if (fileList.Length <= 0)
                                                flname = QuatationNo + "-" + hdnLogID.Value + "-1" + ext;
                                            else
                                                flname = QuatationNo + "-" + hdnLogID.Value + "-" + (fileList.Length + 1).ToString() + ext;

                                            String tmpFile = Server.MapPath("QuatationFollowup/") + flname;
                                            UploadFile.PostedFile.SaveAs(tmpFile);
                                            // ---------------------------------------------------------------
                                            Entity.Quotation objAttachment = new Entity.Quotation();
                                            objAttachment.pkID = 0;
                                            objAttachment.LogID = Convert.ToInt64(hdnLogID.Value);
                                            objAttachment.QuatationID = Convert.ToInt64(hdnQuatationpkID.Value);
                                            objAttachment.FileName = "QuatationFollowup/" + flname;
                                            objAttachment.LoginUserID = Session["LoginUserID"].ToString();
                                            BAL.QuotationMgmt.AddUpdateQuotationDocuments(objAttachment, out ReturnCode1, out ReturnMsg1);
                                            // ---------------------------------------------------------------
                                            //BindQuotationDocuments(hdnLogID.Value);
                                            BindQuotationDocuments(hdnQuatationpkID.Value);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, !');", true);
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                                else
                                    ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('pdf');", true);
                            }
                        }
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(strErr))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
                    }
                }
            }
        }

        public void BindQuotationDocuments(String pLogID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.Quotation> lst = BAL.QuotationMgmt.GetQuatationLogList(pLogID);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptQuatationFollowUp.DataSource = dtDetail1;
            rptQuatationFollowUp.DataBind();
            Session.Add("dtQuatationFollowup", dtDetail1);

            rptQuatationFollowupLog.DataSource = dtDetail1;
            rptQuatationFollowupLog.DataBind();
        }
        protected void rptQuatationFollowUp_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtQuatationFollowup = (DataTable)Session["dtQuatationFollowup"];
                if (dtQuatationFollowup == null)
                {
                    BindQuotationDocuments(hdnQuatationpkID.Value.ToString());
                }
                for (int i = dtQuatationFollowup.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtQuatationFollowup.Rows[i];
                    if (dr["RowNum"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtQuatationFollowup.AcceptChanges();
                Session.Add("dtQuatationFollowup", dtQuatationFollowup);
                rptQuatationFollowUp.DataSource = dtQuatationFollowup;
                rptQuatationFollowUp.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);

                Entity.Quotation objEnt = new Entity.Quotation();

                if (String.IsNullOrEmpty(hdnLogID.Value) || hdnLogID.Value == "0")
                {
                    objEnt.QuatationID = Convert.ToInt64(hdnQuatationpkID.Value);
                    objEnt.Remark = txtRemarks.Text;
                    objEnt.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.QuotationMgmt.AddUpdateQuatationLog(objEnt, out ReturnCode, out ReturnMsg);
                    if (ReturnCode > 0)
                    {
                        hdnLogID.Value = ReturnCode.ToString();
                        txtFollowupNo.Text = ReturnCode.ToString();
                    }
                }
                if (ReturnCode > 0 || (!String.IsNullOrEmpty(hdnLogID.Value) && hdnLogID.Value != "0" ))
                {

                    BAL.QuotationMgmt.DeleteQuotationDocumentsByQuotationNo(Convert.ToInt64(hdnQuatationpkID.Value), out ReturnCode1, out ReturnMsg1);

                    if (dtQuatationFollowup != null)
                    {
                        if (dtQuatationFollowup.Rows.Count > 0)
                        {
                            Entity.Quotation objEntity = new Entity.Quotation();
                            foreach (DataRow dr in dtQuatationFollowup.Rows)
                            {
                                objEntity.pkID = 0;
                                objEntity.LogID = Convert.ToInt64(hdnLogID.Value);
                                objEntity.QuatationID = Convert.ToInt64(hdnQuatationpkID.Value);
                                objEntity.FileName = dr["FileName"].ToString();
                                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                                BAL.QuotationMgmt.AddUpdateQuotationDocuments(objEntity, out ReturnCode1, out ReturnMsg1);
                            }
                        }
                    }
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        public void ClearAllFields()
        {
            txtFollowupNo.Text = "";
            txtRemarks.Text = "";
            Session["dtQuatationFollowup"] = null;
            rptQuatationFollowUp.DataSource = null;
            rptQuatationFollowUp.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _pageValid = true;
            string strError = "", ReturnMsg1 = "";
            int ReturnCode1 = 0;

            if (String.IsNullOrEmpty(txtFollowupNo.Text))
            {
                _pageValid = false;

                strError += "<li>" + "Select Followup No" + "</li>";
                txtFollowupNo.Focus();
            }
            if (_pageValid)
            {
                DataTable dtQuatationFollowup = new DataTable();
                dtQuatationFollowup = (DataTable)Session["dtQuatationFollowup"];

                BAL.QuotationMgmt.DeleteQuotationDocumentsByQuotationNo(Convert.ToInt64(hdnQuatationpkID.Value), out ReturnCode1, out ReturnMsg1);

                if (dtQuatationFollowup != null)
                {
                    if (dtQuatationFollowup.Rows.Count > 0)
                    {
                        Entity.Quotation objEntity = new Entity.Quotation();
                        foreach (DataRow dr in dtQuatationFollowup.Rows)
                        {
                            objEntity.FollowUpID = Convert.ToInt64(dr["FollowUpID"]);
                            objEntity.QuatationID = Convert.ToInt64(dr["QuatationID"]);
                            objEntity.Remark = dr["Remark"].ToString();
                            objEntity.FileName = dr["FileName"].ToString();
                            objEntity.CreatedBy = Session["LoginUserID"].ToString();

                            BAL.QuotationMgmt.AddUpdateQuatationLog(objEntity, out ReturnCode1, out ReturnMsg1);
                            strError += "<li>" + ReturnMsg1 + "</li>";
                            //BindQuatFollowup();
                        }

                        btnSave.Disabled = true;
                        //btnReset.Disabled = true;

                        if (!String.IsNullOrEmpty(strError))
                        {
                            if (ReturnCode1 > 0)
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-success');", true);
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strError + "','toast-danger');", true);
                        }
                    }
                }
            }
        }
    }
}
