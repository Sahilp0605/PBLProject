using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StarsProject
{
    public partial class SalesOrderLogAttachment : System.Web.UI.Page
    {
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
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                    hdnLogID.Value = Request.QueryString["id"].ToString();
                if (!String.IsNullOrEmpty(Request.QueryString["orderno"]))
                    hdnOrderNo.Value = Request.QueryString["orderno"].ToString();
                if (!String.IsNullOrEmpty(Request.QueryString["ordernoid"]))
                {
                    hdnOrderpkID.Value = Request.QueryString["ordernoid"].ToString();
                    // --------------------------------------------------------------
                    List<Entity.SalesOrder> lstOrder = new List<Entity.SalesOrder>();
                    lstOrder = BAL.SalesOrderMgmt.GetSalesOrderList(Convert.ToInt64(hdnOrderpkID.Value), Session["LoginUserID"].ToString(), 1, 1000, out totrec);
                    if (lstOrder.Count>0)
                    {
                        drpApprovalStatus.Items.FindByText(string.IsNullOrEmpty(lstOrder[0].ApprovalStatus)? "-- Select --" : lstOrder[0].ApprovalStatus.ToString()).Selected = true;
                        drpProjectStage.Items.FindByText(string.IsNullOrEmpty(lstOrder[0].ProjectStage)? "-- Select --" : lstOrder[0].ProjectStage.ToString()).Selected = true;
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(txtRemarks.Text) || String.IsNullOrEmpty(drpApprovalStatus.SelectedValue) || String.IsNullOrEmpty(drpProjectStage.SelectedValue))
                {
                    _pageValid = false;

                    if (String.IsNullOrEmpty(txtRemarks.Text))
                        strErr += "<li>" + "Remark is required. " + "</li>";

                    if (String.IsNullOrEmpty(drpApprovalStatus.SelectedValue))
                        strErr += "<li>" + "Please Select Order Status " + "</li>";

                    if (String.IsNullOrEmpty(drpProjectStage.SelectedValue))
                        strErr += "<li>" + "Please Select Project Status " + "</li>";

                }
                // --------------------------------------------------------------
                if (_pageValid)
                {
                    // ----------------------------------------------------------------------
                    // Product Document Upload On .... Page Postback
                    // ----------------------------------------------------------------------
                    if (uploadDocument.PostedFile != null)
                    {
                        if (uploadDocument.PostedFile.FileName.Length > 0)
                        {
                            if (uploadDocument.HasFile)
                            {
                                string filePath = uploadDocument.PostedFile.FileName;
                                string filename1 = Path.GetFileName(filePath);
                                string ext = Path.GetExtension(filename1);
                                string type = String.Empty;
                                string[] flArray = filename1.Split('.');
                                string onlyFileName = "";
                                if (flArray.Length>0)
                                    onlyFileName = flArray[0].Replace(".", "").Replace(" ", "");

                                if (ext == ".pdf")
                                {
                                    try
                                    {
                                        // -----------------------------------------------------
                                        Entity.SalesOrder objEntity = new Entity.SalesOrder();

                                        if (String.IsNullOrEmpty(hdnLogID.Value) || hdnLogID.Value == "0")
                                        {
                                            objEntity.pkID = Convert.ToInt64(hdnOrderpkID.Value);
                                            objEntity.OrderNo = hdnOrderNo.Value;
                                            objEntity.ApprovalStatus = drpApprovalStatus.SelectedValue;
                                            objEntity.ProjectStage = drpProjectStage.SelectedValue;
                                            objEntity.StatusRemarks = txtRemarks.Text;
                                            objEntity.LoginUserID = Session["LoginUserID"].ToString();
                                            // -------------------------------------------------------------- Insert/Update Record
                                            BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                                            if (ReturnCode > 0)
                                                hdnLogID.Value = ReturnCode.ToString();
                                        }

                                        // -----------------------------------------------------
                                        if (ReturnCode > 0 || (!String.IsNullOrEmpty(hdnLogID.Value) && hdnLogID.Value != "0" && !String.IsNullOrEmpty(onlyFileName)))
                                        {

                                            string rootFolderPath = Server.MapPath("OrderDocs");
                                            string filesToDelete = @"*-" + hdnLogID.Value + "-*.*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                            // -----------------------------------------------------
                                            String flname = "";
                                            if (fileList.Length <= 0)
                                                flname = "SO-" + onlyFileName + "-" + hdnLogID.Value + "-1.pdf";
                                            else
                                                flname = "SO-" + onlyFileName + "-" + hdnLogID.Value + "-" + (fileList.Length + 1).ToString() + ".pdf";

                                            String tmpFile = Server.MapPath("OrderDocs/") + flname;
                                            uploadDocument.PostedFile.SaveAs(tmpFile);
                                            // ---------------------------------------------------------------
                                            Entity.SalesorderDocuments objAttachment = new Entity.SalesorderDocuments();
                                            objAttachment.pkID = 0;
                                            objAttachment.LogID = Convert.ToInt64(hdnLogID.Value);
                                            objAttachment.OrderNo = hdnOrderNo.Value;
                                            objAttachment.AttachmentFile = flname;
                                            objAttachment.LoginUserID = Session["LoginUserID"].ToString();
                                            BAL.SalesOrderMgmt.AddUpdateSalesOrderDocuments(objAttachment, out ReturnCode1, out ReturnMsg1);
                                            // ---------------------------------------------------------------
                                            BindSalesOrderDocuments(Convert.ToInt64(hdnLogID.Value));
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
        public void BindDropDown()
        {
            drpApprovalStatus.DataSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("SOApproval");
            drpApprovalStatus.DataValueField = "InquiryStatusName";
            drpApprovalStatus.DataTextField = "InquiryStatusName";
            drpApprovalStatus.DataBind();
            drpApprovalStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            // ---------------------------------------------------------
            drpProjectStage.DataSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("ProjectStatus");
            drpProjectStage.DataValueField = "InquiryStatusName";
            drpProjectStage.DataTextField = "InquiryStatusName";
            drpProjectStage.DataBind();
            drpProjectStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
        }
        public void BindSalesOrderDocuments(Int64 pLogID)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.SalesorderDocuments> lst = BAL.SalesOrderMgmt.GetSalesOrderDocumentsList(0, pLogID, "");
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptOrderDocs.DataSource = dtDetail1;
            rptOrderDocs.DataBind();
            Session.Add("dtOrderDocs", dtDetail1);
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


            if (String.IsNullOrEmpty(txtRemarks.Text) || String.IsNullOrEmpty(drpApprovalStatus.SelectedValue) || String.IsNullOrEmpty(drpProjectStage.SelectedValue))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtRemarks.Text))
                    strErr += "<li>" + "Remark is required. " + "</li>";

                if (String.IsNullOrEmpty(drpApprovalStatus.SelectedValue))
                    strErr += "<li>" + "Please Select Order Status " + "</li>";

                if (String.IsNullOrEmpty(drpProjectStage.SelectedValue))
                    strErr += "<li>" + "Please Select Project Status " + "</li>";
            }
            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.SalesOrder objEntity = new Entity.SalesOrder();

                if (String.IsNullOrEmpty(hdnLogID.Value) || hdnLogID.Value == "0")
                {
                    objEntity.pkID = Convert.ToInt64(hdnLogID.Value);
                    objEntity.OrderNo = hdnOrderNo.Value;
                    objEntity.ApprovalStatus = drpApprovalStatus.SelectedValue;
                    objEntity.ProjectStage = drpProjectStage.SelectedValue;
                    objEntity.StatusRemarks = txtRemarks.Text;
                    objEntity.LoginUserID = Session["LoginUserID"].ToString();
                    // -------------------------------------------------------------- Insert/Update Record
                    BAL.SalesOrderMgmt.UpdateSalesOrderApproval(objEntity, out ReturnCode, out ReturnMsg);
                }
                strErr += "<li>" + ReturnMsg + "</li>";
                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    // SAVE - Product Documents
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    //string filePath, filename1, ext, type;
                    //Byte[] bytes;
                    //long EmpID;
                    //DataTable dtEmpDocs = new DataTable();
                    //dtEmpDocs = (DataTable)Session["dtEmpDocs"];
                    //// ----------------------------------------------
                    //EmpID = 0;
                    //if (dtEmpDocs != null)
                    //{
                    //    foreach (DataRow dr in dtEmpDocs.Rows)
                    //    {
                    //        if (dr.RowState.ToString() != "Deleted")
                    //        {
                    //            filename1 = dr["FileName"].ToString();
                    //            type = dr["Filetype"].ToString();
                    //            // -------------------------------------------------------------- Insert/Update Record
                    //            BAL.OrganizationEmployeeMgmt.AddUpdateEmployeeDocuments(EmpID, filename1, type, Session["LoginUserID"].ToString(), out ReturnCode1, out ReturnMsg1);
                    //        }
                    //    }
                    //}
                    Session.Remove("dtEmpDocs");


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
        protected void rptOrderDocs_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtOrderDocs = (DataTable)Session["dtOrderDocs"];
                for (int i = dtOrderDocs.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtOrderDocs.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtOrderDocs.AcceptChanges();
                Session.Add("dtOrderDocs", dtOrderDocs);
                rptOrderDocs.DataSource = dtOrderDocs;
                rptOrderDocs.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Deleted Successfully !');", true);
            }
        }
    }
}