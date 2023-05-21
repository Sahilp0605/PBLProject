using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.Services;

namespace StarsProject
{
    public partial class VisitAcupanel : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                // --------------------------------------------------------
                myAttachBefore.ResetSession("VisitAcu-Before");
                myAttachAfter.ResetSession("VisitAcu-After");
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                }
                // --------------------------------------------------------    
                if (!String.IsNullOrEmpty(Request.QueryString["complaintno"]))
                {
                    hdnComplaintNo.Value = Request.QueryString["complaintno"].ToString();
                    List<Entity.Complaint> lstComplaint = new List<Entity.Complaint>();
                    lstComplaint = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnComplaintNo.Value), 0, "", Session["LoginUserID"].ToString());
                    if (lstComplaint.Count > 0)
                    {
                        hdnCustomerID.Value = lstComplaint[0].CustomerID.ToString();
                        txtNameOfCustomer.Text = lstComplaint[0].CustomerName.ToString();
                        // --------------------------------------------------------
                        //BindComplaintByCustomer();
                    }
                }
                // --------------------------------------------------------
                if (hdnpkID.Value == "0" || hdnpkID.Value == "")
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
                }
            }
            else
            {

                myAttachBefore.ModuleName = "VisitAcu-Before";
                myAttachBefore.KeyValue = hdnpkID.Value;
                myAttachBefore.ManageLibraryDocs();

                myAttachAfter.ModuleName = "VisitAcu-After";
                myAttachAfter.KeyValue = hdnpkID.Value;
                myAttachAfter.ManageLibraryDocs();
            }
        }
        //----------------------------------------------------------------------
        // Visitor Document Upload On....Page Postback
        //----------------------------------------------------------------------

        //    if (uploadDocument.PostedFile != null && uploadDocument1.PostedFile != null)
        //    {
        //        if (uploadDocument.HasFile)
        //        {
        //            string filePath = uploadDocument.PostedFile.FileName;
        //            string filename1 = Path.GetFileName(filePath);
        //            string ext = Path.GetExtension(filename1).ToLower();
        //            string type = String.Empty;
        //            // ----------------------------------------------------------
        //            if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
        //            {
        //                string rootFolderPathDocument = Server.MapPath("~/panelphoto");
        //                string filesToDeleteDocument = @"Visitdoc-Before" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
        //                string[] fileListDocument = System.IO.Directory.GetFiles(rootFolderPathDocument, filesToDeleteDocument);
        //                foreach (string filedocument in fileListDocument)
        //                {
        //                    System.IO.File.Delete(filedocument);
        //                }
        //                // -----------------------------------------------------
        //                String flnamedocument = "Visitdoc-Before" + hdnpkID.Value.Trim() + ext;
        //                uploadDocument.SaveAs(Server.MapPath("~/panelphoto/") + flnamedocument);
        //                imgBefore.Value = flnamedocument;
        //                imgDocument.ImageUrl = "";
        //                imgDocument.ImageUrl = "~/panelphoto/" + flnamedocument;
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
        //            }
        //            else
        //                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
        //        }
        //        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //        if (uploadDocument1.HasFile)
        //        {
        //            string filePath = uploadDocument1.PostedFile.FileName;
        //            string filename1 = Path.GetFileName(filePath);
        //            string ext = Path.GetExtension(filename1).ToLower();
        //            string type = String.Empty;
        //            // ----------------------------------------------------------
        //            if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
        //            {
        //                string rootFolderPathDocument = Server.MapPath("~/photoafteraction");
        //                string filesToDeleteDocument = @"Visitdoc-After" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
        //                string[] fileListDocument = System.IO.Directory.GetFiles(rootFolderPathDocument, filesToDeleteDocument);
        //                foreach (string filedocument in fileListDocument)
        //                {
        //                    System.IO.File.Delete(filedocument);
        //                }
        //                // -----------------------------------------------------
        //                String flnamedocument = "Visitdoc-After" + hdnpkID.Value.Trim() + ext;
        //                uploadDocument1.SaveAs(Server.MapPath("~/photoafteraction/") + flnamedocument);
        //                imgAfter.Value = flnamedocument;
        //                imgDocument1.ImageUrl = "";
        //                imgDocument1.ImageUrl = "~/photoafteraction/" + flnamedocument;
        //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
        //            }
        //            else
        //                ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
        //        }
        //    }

        //    var requestTarget = this.Request["__EVENTTARGET"];

        //    if (requestTarget.ToLower() == "txtNameOfCustomer")
        //    {
        //        if (!String.IsNullOrEmpty(hdnCustomerID.Value))
        //            BindComplaintByCustomer();
        //    }

        //}


        public void BindDropDown()
        {
            int totrec1 = 0;
            // ---------------- NatureOfCall List  -------------------------------------
            List<Entity.NatureCall> lstObj = new List<Entity.NatureCall>();
            lstObj = BAL.NatureOfCallMgmt.GetNatureCallList(0, Session["LoginUserID"].ToString(), 1, 10000, out totrec1);
            drpNatureOfCall.DataSource = lstObj;
            drpNatureOfCall.DataValueField = "pkID";
            drpNatureOfCall.DataTextField = "NatureOfCall";
            drpNatureOfCall.DataBind();
            drpNatureOfCall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            // ---------------- Complaint No.  -------------------------------------
            List<Entity.Complaint> lstComplaint = new List<Entity.Complaint>();
            lstComplaint = BAL.ComplaintMgmt.GetComplaintList();
            drpComplaintNo.DataSource = lstComplaint;
            drpComplaintNo.DataValueField = "pkID";
            drpComplaintNo.DataTextField = "ComplaintNo";
            drpComplaintNo.DataBind();
            drpComplaintNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Complaint No. --", "0"));
        }
        //public void BindComplaintByCustomer()
        //{
        //    int totrec = 0;
        //    // ---------------- Designation List  -------------------------------------
        //    List<Entity.Complaint> lstEmployee = new List<Entity.Complaint>();
        //    lstEmployee = BAL.ComplaintMgmt.GetComplaintList(0, Convert.ToInt64(hdnCustomerID.Value), "", 0, 0, Session["LoginUserID"].ToString(), "", 1, 10000, out totrec);
        //    drpComplaintNo.DataSource = lstEmployee;
        //    drpComplaintNo.DataValueField = "pkID";
        //    drpComplaintNo.DataTextField = "ComplaintNo";
        //    drpComplaintNo.DataBind();
        //    drpComplaintNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Complaint # --", "0"));
        //}

        public void OnlyViewControls()
        {
            drpComplaintNo.Attributes.Add("disabled", "disabled");
            txtNameOfCustomer.ReadOnly = true;
            txtContactNo1.ReadOnly = true;
            drpStatus.Attributes.Add("disabled", "disabled");
            txtVisitDate.ReadOnly = true;
            txtTimeFrom.ReadOnly = true;
            txtTimeTo.ReadOnly = true;
            txtPanelSRNo.ReadOnly = true;
            txtProductSRNo.ReadOnly = true;
            drpVisitType.Attributes.Add("disabled", "disabled");
            txtSiteCondition.ReadOnly = true;
            txtFaultByService.ReadOnly = true;
            txtActionTaken.ReadOnly = true;
            txtFurtherAction.ReadOnly = true;
            drpNatureOfCall.Attributes.Add("disabled", "disabled");
            txtCloseDate.ReadOnly = true;
            drpVisitChargeType.Attributes.Add("disabled", "disabled");
            txtVisitCharge.ReadOnly = true;
            txtComplaintNotes.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                List<Entity.ComplaintVisit> lstEntity = new List<Entity.ComplaintVisit>();
                lstEntity = BAL.ComplaintMgmt.GetComplaintVisitList(Convert.ToInt64(hdnpkID.Value), Convert.ToInt64(hdnComplaintNo.Value), 0, 0, "", "", Session["LoginUserID"].ToString());

                hdnpkID.Value = lstEntity[0].pkID.ToString();
                hdnEmployeeID.Value = lstEntity[0].EmployeeID.ToString();
                drpComplaintNo.SelectedValue = lstEntity[0].ComplaintNo.ToString();
                hdnComplaintNo.Value = lstEntity[0].ComplaintNo.ToString();
                txtContactNo1.Text = lstEntity[0].ContactNo1.ToString();
                //hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtNameOfCustomer.Text = lstEntity[0].NameOfCustomer.ToString();
                //txtNameOfCustomer_TextChanged(null,null);               
                drpVisitType.SelectedValue = lstEntity[0].VisitType.ToString();
                if (!String.IsNullOrEmpty(lstEntity[0].VisitDate.ToString()) && lstEntity[0].VisitDate.Value.Year > 1900)
                    txtVisitDate.Text = lstEntity[0].VisitDate.Value.ToString("yyyy-MM-dd");
                else
                    txtVisitDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTimeFrom.Text = lstEntity[0].TimeFrom.ToString();
                txtTimeTo.Text = lstEntity[0].TimeTo.ToString();
                txtPanelSRNo.Text = lstEntity[0].PanelSRNo.ToString();
                txtProductSRNo.Text = lstEntity[0].ProductSRNo.ToString();
                txtSiteCondition.Text = lstEntity[0].SiteCondition.ToString();
                txtFaultByService.Text = lstEntity[0].FaultByService.ToString();
                txtActionTaken.Text = lstEntity[0].ActionTaken.ToString();
                txtFurtherAction.Text = lstEntity[0].FurtherAction.ToString();
                drpNatureOfCall.SelectedValue = lstEntity[0].NatureOfCall.ToString();
                if (!String.IsNullOrEmpty(lstEntity[0].CloseDate.ToString()))
                    txtCloseDate.Text = lstEntity[0].CloseDate.Value.ToString("yyyy-MM-dd");
                else
                    txtCloseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                drpVisitChargeType.SelectedValue = lstEntity[0].VisitChargeType.ToString();
                txtVisitCharge.Text = lstEntity[0].VisitCharge.ToString();
                drpStatus.SelectedValue = lstEntity[0].ComplaintStatus.ToString();
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();

                //if (!String.IsNullOrEmpty(lstEntity[0].PanelPhoto))
                //    imgDocument.ImageUrl = "~/panelPhoto/" + lstEntity[0].PanelPhoto;

                //if (!String.IsNullOrEmpty(lstEntity[0].PhotoAfterAction))
                //    imgDocument1.ImageUrl = "~/photoafteraction/" + lstEntity[0].PhotoAfterAction;

                BindProductParts(Convert.ToInt64(drpComplaintNo.SelectedValue));
            }
            //------------------------------------------------------------------------
            myAttachBefore.ModuleName = "VisitAcu-Before";
            myAttachBefore.KeyValue = hdnpkID.Value;
            myAttachBefore.BindModuleDocuments();
            //---------------------------------------------------------------------------
            myAttachAfter.ModuleName = "VisitAcu-After";
            myAttachAfter.KeyValue = hdnpkID.Value;
            myAttachAfter.BindModuleDocuments();
        }

        public void ClearAllField()
        {
            myAttachBefore.ResetSession("VisitAcu-Before");
            myAttachAfter.ResetSession("VisitAcu-After");

            hdnpkID.Value = "0";
            hdnComplaintNo.Value = "0";
            hdnParent.Value = "";
            //hdnCustomerID.Value = "";
            hdnEmployeeID.Value = "";
            txtNameOfCustomer.Text = "";
            txtContactNo1.Text = "";
            txtVisitDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTimeFrom.Text = DateTime.Now.ToString("hh:mm tt");
            txtTimeTo.Text = DateTime.Now.ToString("hh:mm tt");
            drpVisitType.SelectedValue = "Free";
            txtPanelSRNo.Text = "";
            txtProductSRNo.Text = "";
            txtSiteCondition.Text = "";
            txtFaultByService.Text = "";
            txtActionTaken.Text = "";
            txtFurtherAction.Text = "";
            drpNatureOfCall.SelectedValue = "";
            txtCloseDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            drpVisitChargeType.SelectedValue = "";
            txtVisitCharge.Text = "";
            drpComplaintNo.ClearSelection();
            txtComplaintNotes.Text = "";
            txtNameOfCustomer.Focus();
            btnSave.Disabled = false;

            // ------------------------------------------------------------
            myAttachBefore.ModuleName = "VisitAcu-Before";
            myAttachBefore.KeyValue = hdnpkID.Value;
            myAttachBefore.BindModuleDocuments();
            // ------------------------------------------------------------
            myAttachAfter.ModuleName = "VisitAcu-After";
            myAttachAfter.KeyValue = hdnpkID.Value;
            myAttachAfter.BindModuleDocuments();

            BindProductParts(0);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnCode1 = 0, ReturnpkID = 0;
            string ReturnMsg = "", ReturnMsg1 = "" , ReturnComplaintNo = "";
            string strErr = "";

            _pageValid = true;

            if (String.IsNullOrEmpty(txtNameOfCustomer.Text) || (String.IsNullOrEmpty(drpComplaintNo.SelectedValue) ||
               String.IsNullOrEmpty(txtContactNo1.Text) || String.IsNullOrEmpty(txtVisitDate.Text) || String.IsNullOrEmpty(txtTimeFrom.Text) || String.IsNullOrEmpty(txtTimeTo.Text)
               || String.IsNullOrEmpty(txtFaultByService.Text) || String.IsNullOrEmpty(txtActionTaken.Text)))

            {
                _pageValid = false;

                if (String.IsNullOrEmpty(drpComplaintNo.SelectedValue))
                    strErr += "<li>" + "Complaint No. is Required." + "</li>";

                if (String.IsNullOrEmpty(txtNameOfCustomer.Text))
                    strErr += "<li>" + "Customer Selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtContactNo1.Text))
                    strErr += "<li>" + "Contact No. is required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitDate.Text) || String.IsNullOrEmpty(txtTimeFrom.Text) || String.IsNullOrEmpty(txtTimeTo.Text))
                    strErr += "<li>" + "Visit Date and Period is required." + "</li>";

                if (String.IsNullOrEmpty(txtFaultByService.Text))
                    strErr += "<li>" + "Fault finding by service engineer is required." + "</li>";

                if (String.IsNullOrEmpty(txtActionTaken.Text))
                    strErr += "<li>" + "Action Taken is required." + "</li>";
            }

            if (!String.IsNullOrEmpty(txtVisitDate.Text))
            {
                DateTime dt2 = DateTime.Now;
                if (Convert.ToDateTime(txtVisitDate.Text) > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Future Visit Date Not Allowed." + "</li>";
                }
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.ComplaintVisit objEntity = new Entity.ComplaintVisit();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                if (!String.IsNullOrEmpty(drpComplaintNo.SelectedValue))
                    objEntity.ComplaintNo = Convert.ToInt64(drpComplaintNo.SelectedValue);
                //objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.ContactNo1 = Convert.ToInt64(txtContactNo1.Text);
                objEntity.VisitDate = Convert.ToDateTime(txtVisitDate.Text);
                objEntity.TimeFrom = txtTimeFrom.Text;
                objEntity.TimeTo = txtTimeTo.Text;
                objEntity.PanelSRNo = txtPanelSRNo.Text;
                objEntity.ProductSRNo = txtProductSRNo.Text;
                objEntity.SiteCondition = txtSiteCondition.Text;
                objEntity.FaultByService = txtFaultByService.Text;
                objEntity.ActionTaken = txtActionTaken.Text;
                objEntity.FurtherAction = txtFurtherAction.Text;

                if (!String.IsNullOrEmpty(drpNatureOfCall.SelectedValue))
                    objEntity.NatureOfCall = Convert.ToInt64(drpNatureOfCall.SelectedValue);

                objEntity.CloseDate = Convert.ToDateTime(txtCloseDate.Text);

                objEntity.VisitType = drpVisitType.Text;
                if (drpVisitType.SelectedValue == "Charged")
                {
                    objEntity.VisitChargeType = drpVisitChargeType.SelectedValue;
                    objEntity.VisitCharge = (!String.IsNullOrEmpty(txtVisitCharge.Text) && txtVisitCharge.Text != "0") ? Convert.ToDecimal(txtVisitCharge.Text) : 0;
                }
                objEntity.ComplaintStatus = drpStatus.SelectedValue;
                objEntity.ComplaintNotes = txtComplaintNotes.Text;
                objEntity.NameOfCustomer = txtNameOfCustomer.Text;

                objEntity.PanelPhoto = imgBefore.Value;
                objEntity.PhotoAfterAction = imgAfter.Value;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ComplaintMgmt.AddUpdateComplaintVisitAccupanel(objEntity, out ReturnCode, out ReturnMsg, out ReturnpkID, out ReturnComplaintNo);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    string[] RetComplaintDetail = ReturnComplaintNo.Split(',');
                    ReturnComplaintNo = RetComplaintDetail[0];
                    Int64 ReturntPKID = Convert.ToInt64(RetComplaintDetail[1]);

                    hdnpkID.Value = Convert.ToInt64(ReturntPKID).ToString();
                    btnSave.Disabled = true;
                    //------------------------------------------------------------
                    myAttachBefore.KeyValue = Convert.ToInt64(ReturntPKID).ToString();
                    myAttachBefore.SaveModuleDocs();
                    // ------------------------------------------------------------
                    myAttachAfter.KeyValue = Convert.ToInt64(ReturntPKID).ToString();
                    myAttachAfter.SaveModuleDocs();
                }
                // =========================================================================================
                // >>>>>>>> First Delete Acupanel detail entry from table
                // =========================================================================================
                if (ReturnCode > 0)
                {
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];
                    // --------------------------------------------------------------
                    BAL.VisitAcupanelMgmt.DeleteComplaintVisitDetailByComplaintNo(ReturnpkID, out ReturnCode1, out ReturnMsg1);

                    // >>>>>>>> Second Insert all Acupanel detail entry into table
                    Entity.VisitAcupanel objEntity1 = new Entity.VisitAcupanel();
                    if (dtDetail != null)
                    {
                        foreach (DataRow dr in dtDetail.Rows)
                        {
                            objEntity1.pkID = 0;
                            objEntity1.ComplaintNo = Convert.ToInt64(drpComplaintNo.SelectedValue);
                            objEntity1.ParentID = ReturnpkID;
                            objEntity1.NewRep = dr["NewRep"].ToString();
                            objEntity1.ProductName = dr["ProductName"].ToString();
                            objEntity1.SrNo = dr["SrNo"].ToString();
                            objEntity1.ReplaceProduct = dr["ReplaceProduct"].ToString();
                            objEntity1.NewSrNo = dr["NewSrNo"].ToString();
                            objEntity1.Remarks = dr["Remarks"].ToString();
                            objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                            // -------------------------------------------------------------- Insert/Update Record
                            BAL.VisitAcupanelMgmt.AddUpdateComplaintVisitDetail(objEntity1, out ReturnCode1, out ReturnMsg1);
                        }
                    }
                    if (ReturnCode > 0)
                    {
                        strErr += "<li>" + ReturnMsg1 + "</li>";
                        Session.Remove("dtDetail");
                    }
                }
            }
            // ----------------------------------------------------------------
            if (!String.IsNullOrEmpty(strErr))
            {
                if (ReturnCode > 0)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-success');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        //protected void txtNameOfCustomer_TextChanged(object sender, EventArgs e)
        //{
        //    string strErr = "";
        //    int totalrecord;
        //    if (!String.IsNullOrEmpty(hdnpkID.Value))
        //    {
        //        // -----------------------------------------------------
        //        List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();

        //        if (!String.IsNullOrEmpty(hdnpkID.Value))
        //            lstEntity = BAL.CommonMgmt.GetCustomerListForVisit(0);

        //        if (!String.IsNullOrEmpty(hdnpkID.Value) && String.IsNullOrEmpty(txtNameOfCustomer.Text))
        //            txtNameOfCustomer.Text = (lstEntity.Count > 0) ? lstEntity[0].NameOfCustomer : "";
        //        //txtContactNo1.Text = lstEntity[0].ContactNo1.ToString();

        //        //BindComplaintByCustomer();
        //    }
        //    else
        //    {
        //        txtNameOfCustomer.Focus();
        //    }
        //    if (!String.IsNullOrEmpty(strErr))
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
        //    }
        //}

        [System.Web.Services.WebMethod]
        public static string DeleteComplaintVisit(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            List<Entity.ComplaintVisit> lstEntity = new List<Entity.ComplaintVisit>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintVisitList(Convert.ToInt64(pkID), 0, 0, 0, "", "", HttpContext.Current.Session["LoginUserID"].ToString());
            if (lstEntity.Count > 0)
            {
                // ---------------------------------------------------
                myModuleAttachment mya = new myModuleAttachment();
                mya.DeleteModuleEntry("VisitAcu-Before", pkID.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
                mya.DeleteModuleEntry("VisitAcu-After", pkID.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
            }
            // --------------------------------- Delete Record
            BAL.ComplaintMgmt.DeleteComplaintVisit(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        public void BindProductParts(Int64 ComplaintNo)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = BAL.VisitAcupanelMgmt.GetComplaintVisitDetail(ComplaintNo,Convert.ToInt64(hdnpkID.Value));
            rptProductParts.DataSource = dtDetail;
            rptProductParts.DataBind();
            Session["dtDetail"] = dtDetail;
        }
        protected void rptProductParts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string strErr = "";
            int ReturnCode = 0;
            String ReturnMsg = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    HiddenField hdnComplaintNo = (HiddenField)e.Item.FindControl("hdnComplaintNo");
                    TextBox txtNewRep = (TextBox)e.Item.FindControl("txtNewRep");
                    TextBox txtProductName = (TextBox)e.Item.FindControl("txtProductName");
                    TextBox txtSrNo = (TextBox)e.Item.FindControl("txtSrNo");
                    TextBox txtReplaceProduct = (TextBox)e.Item.FindControl("txtReplaceProduct");
                    TextBox txtNewSrNo = (TextBox)e.Item.FindControl("txtNewSrNo");
                    TextBox txtRemarks = (TextBox)e.Item.FindControl("txtRemarks");

                    if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtNewRep")).Text) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text) ||
                        String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtReplaceProduct")).Text) || String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtNewSrNo")).Text))
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtProductName")).Text))
                            strErr += "<li>" + "Product Name is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtNewRep")).Text))
                            strErr += "<li>" + "New/Replace is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtReplaceProduct")).Text))
                            strErr += "<li>" + "Replace Product is required." + "</li>";

                        if (String.IsNullOrEmpty(((TextBox)e.Item.FindControl("txtNewSrNo")).Text))
                            strErr += "<li>" + "New Serial No. is required." + "</li>";
                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetail"];

                        ////----Check For Duplicate Item----//
                        //string find = "pkID = " + hdnpkID.Value;
                        //DataRow[] foundRows = dtDetail.Select(find);
                        //if (foundRows.Length > 0)
                        //{
                        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                        //    return;
                        //}

                        Int64 cntRow = dtDetail.Rows.Count + 1;

                        DataRow dr = dtDetail.NewRow();

                        dr["pkID"] = cntRow;

                        string compno = hdnComplaintNo.Value;
                        string newrep = ((TextBox)e.Item.FindControl("txtNewRep")).Text;
                        string product = ((TextBox)e.Item.FindControl("txtProductName")).Text;
                        string srno = ((TextBox)e.Item.FindControl("txtSrNo")).Text;
                        string replace = ((TextBox)e.Item.FindControl("txtReplaceProduct")).Text;
                        string newsrno = ((TextBox)e.Item.FindControl("txtNewSrNo")).Text;
                        string remarks = ((TextBox)e.Item.FindControl("txtRemarks")).Text;

                        dr["ComplaintNo"] = (!String.IsNullOrEmpty(compno)) ? Convert.ToInt64(compno) : 0;
                        dr["NewRep"] = (!String.IsNullOrEmpty(newrep)) ? newrep : "";
                        dr["ProductName"] = (!String.IsNullOrEmpty(product)) ? product : "";
                        dr["SrNo"] = (!String.IsNullOrEmpty(srno)) ? srno : "";
                        dr["ReplaceProduct"] = (!String.IsNullOrEmpty(replace)) ? replace : "";
                        dr["NewSrNo"] = (!String.IsNullOrEmpty(newsrno)) ? newsrno : "";
                        dr["Remarks"] = (!String.IsNullOrEmpty(remarks)) ? remarks : "";

                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetail", dtDetail);
                        
                        // ---------------------------------------------------------------
                        rptProductParts.DataSource = dtDetail;
                        rptProductParts.DataBind();
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
                    DataTable dtDetail = new DataTable();
                    dtDetail = (DataTable)Session["dtDetail"];
                    // --------------------------------- delete record
                    string iname = ((HiddenField)e.Item.FindControl("hdnpkID")).Value;

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (dr["pkID"].ToString() == iname)
                        {
                            dtDetail.Rows.Remove(dr);
                            break;
                        }
                    }
                    rptProductParts.DataSource = dtDetail;
                    rptProductParts.DataBind();
                    Session.Add("dtDetail", dtDetail);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void rptProductParts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edpkID = (HiddenField)item.FindControl("hdnpkID");
            TextBox edNewRep = (TextBox)item.FindControl("edNewRep");
            TextBox edProductName = (TextBox)item.FindControl("edProductName");
            TextBox edSrNo = (TextBox)item.FindControl("edSrNo");
            TextBox edReplaceProduct = (TextBox)item.FindControl("edReplaceProduct");
            TextBox edNewSrNo = (TextBox)item.FindControl("edNewSrNo");
            TextBox edRemarks = (TextBox)item.FindControl("edRemarks");


            String newrep = (!String.IsNullOrEmpty(edNewRep.Text)) ? edNewRep.Text : "";
            String product = (!String.IsNullOrEmpty(edProductName.Text)) ? edProductName.Text : "";
            String srno = (!String.IsNullOrEmpty(edSrNo.Text)) ? edSrNo.Text : "";
            String replace = (!String.IsNullOrEmpty(edReplaceProduct.Text)) ? edReplaceProduct.Text : "";
            String newsrno = (!String.IsNullOrEmpty(edNewSrNo.Text)) ? edNewSrNo.Text : "";
            String remarks = (!String.IsNullOrEmpty(edRemarks.Text)) ? edRemarks.Text : "";

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetail"];

            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["pkID"].ToString() == edpkID.Value)
                {
                    row.SetField("NewRep", edNewRep.Text);
                    row.SetField("ProductName", edProductName.Text);
                    row.SetField("SrNo", edSrNo.Text);
                    row.SetField("ReplaceProduct", edReplaceProduct.Text);
                    row.SetField("NewSrNo", edNewSrNo.Text);
                    row.SetField("Remarks", edRemarks.Text);

                    row.AcceptChanges();
                }
                dtDetail.AcceptChanges();
                rptProductParts.DataSource = dtDetail;
                rptProductParts.DataBind();
                Session.Add("dtDetail", dtDetail);
            }
        }

        [System.Web.Services.WebMethod]
        public static string FilterCustomer(string pNameOfCustomer)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetCustomerListForComplaintVisit(pNameOfCustomer);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        //public static string FilterCustomerByModule(string pNameOfCustomer, string pSearchModule)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    serializer.MaxJsonLength = Int32.MaxValue;
        //    // --------------------------------- 
        //    var rows = BAL.CustomerMgmt.GetCustomerListForComplaintVisit(pNameOfCustomer, pSearchModule);
        //    return serializer.Serialize(rows);
        //}

        public static void RecompressPDF(string largePDF, string smallPDF)
        {
            //Bind a reader to our large PDF
            PdfReader reader = new PdfReader(largePDF);
            //Create our output PDF
            using (FileStream fs = new FileStream(smallPDF, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //Bind a stamper to the file and our reader
                using (PdfStamper stamper = new PdfStamper(reader, fs))
                {
                    //NOTE: This code only deals with page 1, you'd want to loop more for your code
                    //Get page 1
                    PdfDictionary page = reader.GetPageN(1);
                    //Get the xobject structure
                    PdfDictionary resources = (PdfDictionary)PdfReader.GetPdfObject(page.Get(PdfName.RESOURCES));
                    PdfDictionary xobject = (PdfDictionary)PdfReader.GetPdfObject(resources.Get(PdfName.XOBJECT));
                    if (xobject != null)
                    {
                        PdfObject obj;
                        //Loop through each key
                        foreach (PdfName name in xobject.Keys)
                        {
                            obj = xobject.Get(name);
                            if (obj.IsIndirect())
                            {
                                //Get the current key as a PDF object
                                PdfDictionary imgObject = (PdfDictionary)PdfReader.GetPdfObject(obj);
                                //See if its an image
                                if (imgObject.Get(PdfName.SUBTYPE).Equals(PdfName.IMAGE))
                                {
                                    //NOTE: There's a bunch of different types of filters, I'm only handing the simplest one here which is basically raw JPG, you'll have to research others
                                    if (imgObject.Get(PdfName.FILTER).Equals(PdfName.DCTDECODE))
                                    {
                                        //Get the raw bytes of the current image
                                        byte[] oldBytes = PdfReader.GetStreamBytesRaw((PRStream)imgObject);
                                        //Will hold bytes of the compressed image later
                                        byte[] newBytes;
                                        //Wrap a stream around our original image
                                        using (MemoryStream sourceMS = new MemoryStream(oldBytes))
                                        {
                                            //Convert the bytes into a .Net image
                                            using (System.Drawing.Image oldImage = System.Drawing.Bitmap.FromStream(sourceMS))
                                            {
                                                //Shrink the image to 90% of the original
                                                using (System.Drawing.Image newImage = ShrinkImage(oldImage, 0.9f))
                                                {
                                                    //Convert the image to bytes using JPG at 85%
                                                    newBytes = ConvertImageToBytes(newImage, 85);
                                                }
                                            }
                                        }
                                        //Create a new iTextSharp image from our bytes
                                        iTextSharp.text.Image compressedImage = iTextSharp.text.Image.GetInstance(newBytes);
                                        //Kill off the old image
                                        PdfReader.KillIndirect(obj);
                                        //Add our image in its place
                                        stamper.Writer.AddDirectImageSimple(compressedImage, (PRIndirectReference)obj);
                                    }
                                }
                            }
                        }
                    }
                }

                fs.Close();
                fs.Dispose();
            }
            reader.Close();
        }
        //Standard image save code from MSDN, returns a byte array
        private static byte[] ConvertImageToBytes(System.Drawing.Image image, long compressionLevel)
        {
            if (compressionLevel < 0)
            {
                compressionLevel = 0;
            }
            else if (compressionLevel > 100)
            {
                compressionLevel = 100;
            }
            System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, compressionLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jgpEncoder, myEncoderParameters);
                return ms.ToArray();
            }

        }
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private static System.Drawing.Image ShrinkImage(System.Drawing.Image sourceImage, float scaleFactor)
        {
            int newWidth = Convert.ToInt32(sourceImage.Width * scaleFactor);
            int newHeight = Convert.ToInt32(sourceImage.Height * scaleFactor);

            var thumbnailBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnailBitmap))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                g.DrawImage(sourceImage, imageRectangle);
            }
            return thumbnailBitmap;
        }

        [System.Web.Services.WebMethod]
        public static string GetVisitAcupanelNoForPDF(Int64 pkID)
        {
            String tempVal = "";
            tempVal = BAL.CommonMgmt.GetVisitAcupanelNo(pkID);
            return tempVal;
        }
        
        [WebMethod(EnableSession = true)]

        public static void GenerateVisitAcupanel(Int64 pkID)
        {
            // -----------------------------------------------------------------------
            // Company Reg.Key 
            // ----------------------------------------------------------------------- 

            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

            string tmpSerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            string LoginUserID = HttpContext.Current.Session["LoginUserID"].ToString();
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/images");
            Int32 CompanyId = 0;

            StarsProject.QuotationEagle.serialkey = tmpSerialKey;
            StarsProject.QuotationEagle.LoginUserID = LoginUserID;
            StarsProject.QuotationEagle.printheader = flagPrintHeader;
            StarsProject.QuotationEagle.path = Path;
            StarsProject.QuotationEagle.imagepath = imagepath;
            StarsProject.QuotationEagle.companyid = CompanyId;
            StarsProject.QuotationEagle.printModule = "Quotation";
            // -------------------------------------------------------
            if (tmpSerialKey == "ACSI-C803-CUP0-SHEL")     // Accu Panel
            {
                GenerateVisitAcupanel_AcuPanel(pkID);
            }
            else
            {
                GenerateVisitAcupanel_AcuPanel(pkID);
            }
        }

        public static void GenerateVisitAcupanel_AcuPanel(Int64 pkID)
        {
            //HttpContext.Current.Session["PrintHeader"] = "no";
            HttpContext.Current.Session["printModule"] = "visitcomplaint";
            string htmlOpen = "", htmlClose = "";
            htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
            htmlOpen += "<body>";

            myPdfConstruct pdf = new myPdfConstruct();
            // ----------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            // ----------------------------------------------------------
            PdfPCell cell;
            PdfPTable tblExtra = new PdfPTable(4);
            PdfPTable tableHeader = new PdfPTable(2);
            PdfPTable tblMember = new PdfPTable(1);
            PdfPTable tblDetail = new PdfPTable(8);
            PdfPTable tblSubject = new PdfPTable(1);
            PdfPTable tblHeader = new PdfPTable(1);
            PdfPTable tblFooter = new PdfPTable(2);
            PdfPTable tblSignOff = new PdfPTable(1);
            // ===========================================================================================
            // Retrieving Quotation Master & Detail Data
            // --------------------------------------------------------------------------------------------
            int TotalCount = 0;

            // ----------------------------------------Complaint Visit Data---------------------------------------------------------------------
            List<Entity.ComplaintVisit> lstVisitComplaint = new List<Entity.ComplaintVisit>();
            lstVisitComplaint = BAL.ComplaintMgmt.GetComplaintVisitList(pkID, 0, 0, 0, "", "", HttpContext.Current.Session["LoginUserID"].ToString());
            // ----------------------------------------Complaint Visit Detail Data---------------------------------------------------------------------
            DataTable dtVisitDetail = new DataTable();
            if(lstVisitComplaint.Count > 0)
                dtVisitDetail = BAL.VisitAcupanelMgmt.GetComplaintVisitDetail(Convert.ToInt64(lstVisitComplaint[0].ComplaintNo), pkID);
            // ----------------------------------------Complaint Data---------------------------------------------------------------------
            List<Entity.Complaint> lstcomplaint = new List<Entity.Complaint>();
            if (lstVisitComplaint.Count > 0)
                lstcomplaint = BAL.ComplaintMgmt.GetComplaintList(lstVisitComplaint[0].ComplaintNo, 0, "", HttpContext.Current.Session["LoginUserID"].ToString());
            // ------------------------------------------------------------------------------
            List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
            lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "visitcomplaint");
            //----------------------------------------------------------------------------------
            // -------------------------------------------------------------------------------------------------------------
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            if (lstVisitComplaint.Count > 0)
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(lstVisitComplaint[0].CustomerID.ToString()), HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);

            DataTable dtContact = new DataTable();
            if (lstVisitComplaint.Count > 0)
                dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstVisitComplaint[0].CustomerID);
            //----------------------------------------------------------
            List<Entity.ModuleDocuments> lstImageModulCCDefect = new List<Entity.ModuleDocuments>();
            lstImageModulCCDefect = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", "ComplaintAcu-Defect", "", lstcomplaint[0].ComplaintNo, HttpContext.Current.Session["LoginUserID"].ToString());

            List<Entity.ModuleDocuments> lstImageModulCCPanel = new List<Entity.ModuleDocuments>();
            lstImageModulCCPanel = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", "ComplaintAcu-Panel", "", lstcomplaint[0].ComplaintNo, HttpContext.Current.Session["LoginUserID"].ToString());

            List<Entity.ModuleDocuments> lstImageModulVVDefect = new List<Entity.ModuleDocuments>();
            lstImageModulVVDefect = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", "VisitAcu-Before", "", lstVisitComplaint[0].pkID.ToString(), HttpContext.Current.Session["LoginUserID"].ToString());

            List<Entity.ModuleDocuments> lstImageModulVVPanel = new List<Entity.ModuleDocuments>();
            lstImageModulVVPanel = BAL.ModuleDocMgmt.GetModuleDocumentList(0, "", "VisitAcu-After", "", lstVisitComplaint[0].pkID.ToString(), HttpContext.Current.Session["LoginUserID"].ToString());

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring PDF Document Object
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            Int64 TopMargin = 30, BottomMargin = 30, LeftMargin = 30, RightMargin = 30;
            string flagPrintHeader = (string)HttpContext.Current.Session["PrintHeader"];
            if (flagPrintHeader == "yes" || flagPrintHeader == "y")
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_WithHeader) && lstPrinter[0].PrintingMargin_WithHeader.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_WithHeader.Trim().Split(',');
                    TopMargin = Convert.ToInt64(tmpary[0].ToString());
                    BottomMargin = Convert.ToInt64(tmpary[1].ToString());
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(lstPrinter[0].PrintingMargin_Plain) && lstPrinter[0].PrintingMargin_Plain.Trim() != "0,0")
                {
                    String[] tmpary = lstPrinter[0].PrintingMargin_Plain.Trim().Split(',');
                    TopMargin = (Int64)Convert.ToDouble(tmpary[0].ToString());
                    BottomMargin = (Int64)Convert.ToDouble(tmpary[1].ToString());
                }
            }

            Document pdfDoc = new Document(iTextSharp.text.PageSize.A4);
            pdfDoc.SetMargins(LeftMargin, RightMargin, TopMargin, BottomMargin);

            pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            pdfDoc.AddCreationDate();

            MemoryStream ms = new MemoryStream();
            PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);

            Int64 ProdDetail_Lines = 20;
            ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;


            pdfw.PdfVersion = PdfWriter.VERSION_1_6;
            pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
            pdfw.SetFullCompression();

            // ===========================================================================================
            // Printing Heading
            // ===========================================================================================

            #region Section >>>>  Master Information

            int[] column_tblMember = {100};
            tblMember.SetWidths(column_tblMember);
            tblMember.SpacingBefore = 0f;
            tblMember.SpacingAfter = 0f;
            tblMember.LockedWidth = true;

            #region Section >>>> Header Information
            //----------------------------------------------------------
            //----------- Complaint Table Detail
            //----------------------------------------------------------

            var empty = new Phrase();
            empty.Add(new Chunk(" ", pdf.fnCalibriBold9));

            //----------- Service Report

            PdfPTable tblserviceinfo = new PdfPTable(2);
            int[] column_tblserviceinfo = { 15, 85 };
            tblserviceinfo.SetWidths(column_tblserviceinfo);
            tblserviceinfo.AddCell(pdf.setCell("Service Report", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblserviceinfo.AddCell(pdf.setCell("SA Number", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblserviceinfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].ComplaintNo, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblserviceinfo.AddCell(pdf.setCell("Complaint Status", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            String Status = (lstVisitComplaint[0].ComplaintStatus).ToLower();
            if (Status == "open" || Status == "" || Status == "0")
            {
                tblserviceinfo.AddCell(pdf.setCell(" : " + "Open", pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }
            else
            {
                tblserviceinfo.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].ComplaintStatus, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            }
            tblMember.AddCell(pdf.setCell(tblserviceinfo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblMember.AddCell(pdf.setCell("Service Report", pdf.WhiteBaseColor, pdf.fnCalibriBold14, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblMember.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnCalibri12, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


            //----------- Account & Contact Information

            String address = ":  " + lstcomplaint[0].SiteAddress +  "\n" +
                            "   " + (!String.IsNullOrEmpty(lstcomplaint[0].CityName) ? " " + lstcomplaint[0].CityName : "") + (!String.IsNullOrEmpty(lstcomplaint[0].Pincode) ? " (" + lstcomplaint[0].Pincode + "), " : "") + "\n" +
                            "   " + (!String.IsNullOrEmpty(lstcomplaint[0].StateName) ? " " + lstcomplaint[0].StateName : "") + "\n" +
                            "   " + (!String.IsNullOrEmpty(lstcomplaint[0].CountryCode) ? " " + lstcomplaint[0].CountryCode : "");

            PdfPTable tblAddre = new PdfPTable(1);
            int[] column_tblAddre = { 100 };
            tblAddre.SetWidths(column_tblAddre);
            tblAddre.AddCell(pdf.setCell(address, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


            PdfPTable tblAccInfo = new PdfPTable(4);
            int[] column_tblAccInfo = { 25, 35, 10, 30 };
            tblAccInfo.SetWidths(column_tblAccInfo);
            tblAccInfo.AddCell(pdf.setCell("Account & Contact Information", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAccInfo.AddCell(pdf.setCell("Company Name", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAccInfo.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].NameOfCustomer, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAccInfo.AddCell(pdf.setCell("Address", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblAccInfo.AddCell(pdf.setCell(tblAddre, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAccInfo.AddCell(pdf.setCell("Phone", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAccInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].CustmoreMobileNo, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAccInfo.AddCell(pdf.setCell("Email", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAccInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].EmailId, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


           
            //tblAccInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            //tblAccInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            //tblAccInfo.AddCell(pdf.setCell("Issue Description", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            //tblAccInfo.AddCell(pdf.setCell( " : " + lstcomplaint[0].ComplaintNotes, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            //tblAccInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            //tblAccInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAccInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblMember.AddCell(pdf.setCell(tblAccInfo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));




            //----------- Purchase Information

            PdfPTable tblPurchaseInfo = new PdfPTable(4);
            int[] column_tblPurchaseInfo = { 25, 25, 20, 30 };
            tblPurchaseInfo.SetWidths(column_tblPurchaseInfo);
            tblPurchaseInfo.AddCell(pdf.setCell("Purchase Information", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            
            tblPurchaseInfo.AddCell(pdf.setCell("Work Order No.", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblPurchaseInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].WorkOderNo, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblPurchaseInfo.AddCell(pdf.setCell("Date of Purchase", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblPurchaseInfo.AddCell(pdf.setCell(" : " + (lstcomplaint[0].DateOfPurchase.ToString("dd-MM-yyyy") != "" ? lstcomplaint[0].DateOfPurchase.ToString("dd-MM-yyyy") : " "), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            
            tblPurchaseInfo.AddCell(pdf.setCell("Panel Serial No.", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblPurchaseInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].PanelSRNo, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblPurchaseInfo.AddCell(pdf.setCell("Product Serial No.", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblPurchaseInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].ProductSRNo.ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblPurchaseInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblMember.AddCell(pdf.setCell(tblPurchaseInfo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            //----------- Appointment Information

            PdfPTable tblAppointInfo = new PdfPTable(4);
            int[] column_tblAppointInfo = { 15, 35, 15, 35 };
            tblAppointInfo.SetWidths(column_tblAppointInfo);
            tblAppointInfo.AddCell(pdf.setCell("Appointment Information", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            
         
            tblAppointInfo.AddCell(pdf.setCell("Convenient Date", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].ConvinientDate.ToString("dd-MM-yyyy"), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell("Convenient Time", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell(" : " + lstcomplaint[0].ConvinientTimeSlot.ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAppointInfo.AddCell(pdf.setCell("Service Engineer", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].CreatedByEmployee, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell("Attended Date", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell(" : " + (Convert.ToDateTime(lstVisitComplaint[0].VisitDate)).ToString("dd-MM-yyyy"), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblAppointInfo.AddCell(pdf.setCell("Starting Time", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].TimeFrom, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell("Finish Time", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblAppointInfo.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].TimeTo.ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


            tblAppointInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblMember.AddCell(pdf.setCell(tblAppointInfo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


            //----------- Site Condition Information

            PdfPTable tblFaultInfo = new PdfPTable(3);
            int[] column_tblFaultInfo = { 25, 4, 71 };
            tblFaultInfo.SetWidths(column_tblFaultInfo);
            tblFaultInfo.AddCell(pdf.setCell("Site Condition Information", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblFaultInfo.AddCell(pdf.setCell("Site Co-ordinator  ", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(" : ", pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(lstcomplaint[0].SiteCoordinatorName, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblFaultInfo.AddCell(pdf.setCell("Site Condition ", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(" : ", pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(lstVisitComplaint[0].SiteCondition, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblFaultInfo.AddCell(pdf.setCell("Fault Observed By Engineer", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(" : ", pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(lstVisitComplaint[0].FaultByService, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblFaultInfo.AddCell(pdf.setCell("Action Taken", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(" : " , pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(lstVisitComplaint[0].ActionTaken.ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblFaultInfo.AddCell(pdf.setCell("Further Action to be Taken", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_TOP, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(" : " , pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(lstVisitComplaint[0].FurtherAction, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            tblFaultInfo.AddCell(pdf.setCell("Nature of Call", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(" : " , pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblFaultInfo.AddCell(pdf.setCell(lstVisitComplaint[0].NatureOfCallName.ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            
            tblFaultInfo.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblMember.AddCell(pdf.setCell(tblFaultInfo, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));


            //----------- Work Order Line Items Details
            //-----------  

            //----------- Product Repalce 

            PdfPTable tblProductInfoDt = new PdfPTable(4);
            int[] column_tblProductInfoDt = { 15, 35, 15, 35 };
            tblProductInfoDt.SetWidths(column_tblProductInfoDt);
            tblProductInfoDt.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold10, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblProductInfoDt.AddCell(pdf.setCell("Work Order Line Items", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblProductInfoDt.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold10, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblProductInfoDt.AddCell(pdf.setCell("If Require New Parts", pdf.DarkBrownGrey, pdf.fnArialBold10White, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8)); 
            tblProductInfoDt.AddCell(pdf.setCell("If Replaced Parts.", pdf.DarkBrownGrey, pdf.fnArialBold10White, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            for (int i = 0; i < dtVisitDetail.Rows.Count; i++)
            {
                tblProductInfoDt.AddCell(pdf.setCell(dtVisitDetail.Rows[i]["ProductName"].ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
                tblProductInfoDt.AddCell(pdf.setCell(dtVisitDetail.Rows[i]["ReplaceProduct"].ToString(), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 11));
            }

            // -----------------------------------------------------------------------------
            // Section : Table Creation [ Complaint ]
            // -----------------------------------------------------------------------------
            PdfPTable tblComplaintImage = new PdfPTable(4);
            int[] column_tblDefectProductndPanel = { 15, 35, 15, 35 };
            tblComplaintImage.SetWidths(column_tblDefectProductndPanel);
            tblComplaintImage.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold10, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblComplaintImage.AddCell(pdf.setCell("Photo of Defect Products", pdf.DarkBrownGrey, pdf.fnArialBold10White, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
            tblComplaintImage.AddCell(pdf.setCell("Photo of Panel", pdf.DarkBrownGrey, pdf.fnArialBold10White, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE,  Rectangle.NO_BORDER));

            for (int i = 0; i < lstImageModulCCDefect.Count; i++)
            {
                string CCDefect = lstImageModulCCDefect[i].DocName.ToString();
                iTextSharp.text.Image myCCDefect = pdf.findFolerImage("ModuleDocs", CCDefect, 60, 60); 
                if (myCCDefect != null)
                {
                    myCCDefect.ScaleAbsolute(60, 60);
                    tblComplaintImage.AddCell(setCellFixImage(myCCDefect, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblComplaintImage.AddCell(pdf.setCell("No Image Found", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
            }
            for (int i = 0; i < lstImageModulCCPanel.Count; i++)
            {
                string CCPanel = lstImageModulCCPanel[i].DocName.ToString();
                iTextSharp.text.Image myCCPanel = pdf.findFolerImage("ModuleDocs", CCPanel, 60, 60); 
                if (myCCPanel != null)
                {
                    tblComplaintImage.AddCell(setCellFixImage(myCCPanel, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblComplaintImage.AddCell(pdf.setCell("No Image Found", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
            }

            tblProductInfoDt.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold10, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            // -----------------------------------------------------------------------------
            // Section : Table Creation [ Visit ]
            // -----------------------------------------------------------------------------

            PdfPTable tblVisitImage = new PdfPTable(4);
            int[] column_tblActionTakenndPanelPic = { 15, 35, 15, 35 };
            tblVisitImage.SetWidths(column_tblActionTakenndPanelPic);
            tblVisitImage.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold10, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblVisitImage.AddCell(pdf.setCell("After Taken Action Photo", pdf.DarkBrownGrey, pdf.fnArialBold10White, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 8));
            tblVisitImage.AddCell(pdf.setCell("Faulty Part/Panel Photo", pdf.DarkBrownGrey, pdf.fnArialBold10White, pdf.paddingOf6, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            for (int i = 0; i < lstImageModulVVDefect.Count; i++)
            {
                string vvDefect = lstImageModulVVDefect[i].DocName.ToString();
                iTextSharp.text.Image myvvDefect = pdf.findFolerImage("ModuleDocs", vvDefect, 60, 60);
                if (myvvDefect != null)
                {
                    //myvvDefect.ScaleAbsolute(60, 60);
                    tblVisitImage.AddCell(setCellFixImage(myvvDefect, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblVisitImage.AddCell(pdf.setCell("No Image Found", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
            }
            for (int i = 0; i < lstImageModulVVPanel.Count; i++)
            {
                string vvPanel = lstImageModulVVPanel[i].DocName.ToString();
                iTextSharp.text.Image myvvPanel = pdf.findFolerImage("ModuleDocs", vvPanel, 60, 60);
                if (myvvPanel != null)
                {
                    //myvvPanel.ScaleAbsolute(60, 60);
                    tblVisitImage.AddCell(setCellFixImage(myvvPanel, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
                else
                {
                    tblVisitImage.AddCell(pdf.setCell("No Image Found", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf3, 2, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, 15));
                }
            }

            tblVisitImage.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold10, 0, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            // ---------------------------------------------------------------------------
            // Section : Image Table Of Complaint & Visit ... Insertion To Master Table
            // ---------------------------------------------------------------------------
            //tblProductInfoDt.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblMember.AddCell(pdf.setCell(tblProductInfoDt, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblMember.AddCell(pdf.setCell(tblComplaintImage, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
            tblMember.AddCell(pdf.setCell(tblVisitImage, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            //----------- Customer Signature 

            PdfPTable tblCustomerSign = new PdfPTable(2);
            int[] column_tblCustomerSign = { 25, 75};
            tblCustomerSign.SetWidths(column_tblCustomerSign);
            tblCustomerSign.AddCell(pdf.setCell("Customer Signature", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
       
            tblCustomerSign.AddCell(pdf.setCell("Customer Signature", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblCustomerSign.AddCell(pdf.setCell(" : " , pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblCustomerSign.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblCustomerSign.AddCell(pdf.setCell("Signed By ", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblCustomerSign.AddCell(pdf.setCell(" : ", pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblCustomerSign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblMember.AddCell(pdf.setCell(tblCustomerSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            //----------- Engineer Signature  

            string tmpFile = System.Web.Hosting.HostingEnvironment.MapPath("~/EmployeeImages") + "\\esign-" + lstVisitComplaint[0].CreatedBy;
            //string panelphoto = lstVisitComplaint[0].PanelPhoto.Replace('/', '\\');
            iTextSharp.text.Image myesign = pdf.findProductImage(tmpFile);


            PdfPTable tblEngineerSign = new PdfPTable(4);
            int[] column_tblEngineerSign = { 15, 35, 15, 35 };
            tblEngineerSign.SetWidths(column_tblEngineerSign);
            tblEngineerSign.AddCell(pdf.setCell("Branch Service Engineer Signature", pdf.LightShadedBLUEBase, pdf.fnArialBold10, pdf.paddingOf6, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            
            tblEngineerSign.AddCell(pdf.setCell("Signature", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));


            if (File.Exists(tmpFile))
            {
                iTextSharp.text.Image mypanelphoto = iTextSharp.text.Image.GetInstance(tmpFile);
                mypanelphoto.ScaleAbsolute(60, 60);
                tblEngineerSign.AddCell(setCellFixImage(mypanelphoto, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            }
            else
            {
                tblEngineerSign.AddCell(pdf.setCell(" : No Image Found", pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            }

            tblEngineerSign.AddCell(pdf.setCell("Signed By ", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblEngineerSign.AddCell(pdf.setCell(" : " + lstVisitComplaint[0].CreatedByEmployee, pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            
            tblEngineerSign.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
            tblEngineerSign.AddCell(pdf.setCell(" : " + (Convert.ToDateTime(lstVisitComplaint[0].CloseDate).ToString("dd-MM-yyyy") != "" ? (Convert.ToDateTime(lstVisitComplaint[0].CloseDate).ToString("dd-MM-yyyy")) : " "), pdf.WhiteBaseColor, pdf.fnArial9, pdf.paddingOf3, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblEngineerSign.AddCell(pdf.setCell(empty, pdf.WhiteBaseColor, pdf.fnArialBold9, pdf.paddingOf3, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

            tblMember.AddCell(pdf.setCell(tblEngineerSign, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

            #endregion

            #endregion
            htmlClose = "</body></html>";

            // =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
            string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
            string sFileName = "VISITCOMP-" + lstcomplaint[0].pkID.ToString() + ".pdf";
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Header & Footer ..... Settings
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            ITextEvents clHeaderFooter = new ITextEvents();
            pdfw.PageEvent = clHeaderFooter;
            clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
            clHeaderFooter.FooterFont = pdf.objFooterFont;
            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            // Declaring Stylesheet ......
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            StyleSheet objStyle = new StyleSheet();
            objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
            objStyle.LoadTagStyle("body", "font-size", "12pt");
            objStyle.LoadTagStyle("body", "color", "black");
            objStyle.LoadTagStyle("body", "position", "relative");
            objStyle.LoadTagStyle("body", "margin", "0 auto");

            htmlparser.SetStyleSheet(objStyle);

            // ------------------------------------------------------------------------------------------------
            // pdfDOC >>> Open
            // ------------------------------------------------------------------------------------------------
            pdfDoc.Open();


            // >>>>>> Opening : HTML & BODY
            htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

            // >>>>>> Adding Organization Name 
            //tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            //pdfDoc.Add(tableHeader);

            // >>>>>> Adding Quotation Header
            tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSubject);

            // >>>>>> Adding Quotation Master Information Table
            tblExtra.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblExtra.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblExtra);


            // >>>>>> Adding Quotation Header
            tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblHeader);

            // >>>>>> Adding Quotation Header
            tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblMember.HorizontalAlignment = Element.ALIGN_CENTER;           
            pdfDoc.Add(tblMember);

            // >>>>>> Adding Quotation Detail Table
            tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            //pdfDoc.Add(tblDetail);

            PdfPTable tblOuterDetail = new PdfPTable(1);
            int[] column_tblNestedOuter = { 100 };
            tblOuterDetail.SetWidths(column_tblNestedOuter);
            tblOuterDetail.SpacingBefore = 0f;
            tblOuterDetail.LockedWidth = true;
            tblOuterDetail.SplitLate = false;
            tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            tblOuterDetail.AddCell(pdf.setCell(tblDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
            tblOuterDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblOuterDetail.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblOuterDetail);

            // >>>>>> Adding Quotation Footer
            tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            //pdfDoc.Add(tblFooter);

            PdfPTable tblOuterDetail1 = new PdfPTable(1);
            tblOuterDetail1.SetWidths(column_tblNestedOuter);
            tblOuterDetail1.SpacingBefore = 0f;
            tblOuterDetail1.LockedWidth = true;
            tblOuterDetail1.SplitLate = false;
            tblOuterDetail1.HorizontalAlignment = Element.ALIGN_CENTER;
            tblOuterDetail1.AddCell(pdf.setCell(tblFooter, pdf.WhiteBaseColor, pdf.fnCalibriBold9, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
            tblOuterDetail1.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblOuterDetail1.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfDoc.Add(tblOuterDetail1);

            // >>>>>> Adding Quotation Header
            tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
            tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            pdfDoc.Add(tblSignOff);

            // >>>>>> Closing : HTML & BODY
            htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
            // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
            pdfDoc.Close();
            pdfDoc.Dispose();
            string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
            byte[] content = ms.ToArray();
            FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
            fs.Write(content, 0, (int)content.Length);
            fs.Close();
            fs.Dispose();
            string pdfFileName = "";
            pdfFileName = sPath + sFileName;

            RecompressPDF(sPath + smallFileName, pdfFileName);
        }

        private static PdfPCell setCellFixImage(iTextSharp.text.Image pImage, BaseColor bc, iTextSharp.text.Font fn, float pad = 0, int colspn = 1, int hAlign = Element.ALIGN_LEFT, int vAlign = Element.ALIGN_MIDDLE, int borderVal = 15)
        {
            PdfPCell tmpCell = new PdfPCell(pImage, false);
            tmpCell.BackgroundColor = bc;
            tmpCell.Padding = pad;
            tmpCell.Colspan = colspn;
            tmpCell.HorizontalAlignment = hAlign;
            tmpCell.VerticalAlignment = vAlign;
            tmpCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            tmpCell.Border = borderVal;

            return tmpCell;
        }


        //private static PdfPCell setCellFixImage(iTextSharp.text.Image myProdImage, BaseColor whiteBaseColor, Font fnCalibriBold8, float paddingOf3, int v1, int aLIGN_CENTER, int aLIGN_MIDDLE, int v2)
        //{
        //    throw new NotImplementedException();
        //}

        #region Section >>>> 
        //public static void GenerateQCTestRaw_Sharvaya(Int64 pQuotID)
        //{
        //string htmlOpen = "", htmlClose = "";
        //htmlOpen = @"<html xmlns='http://www.w3.org/1999/xhtml'>";
        //htmlOpen += "<body>";

        //myPdfConstruct pdf = new myPdfConstruct();
        //// ----------------------------------------------------------
        //Entity.Authenticate objAuth = new Entity.Authenticate();
        //objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
        //// ----------------------------------------------------------

        //PdfPCell cell;

        //PdfPTable tableHeader = new PdfPTable(2);
        //PdfPTable tblMember = new PdfPTable(4);
        //PdfPTable tblDetail = new PdfPTable(7);
        //PdfPTable tblSubject = new PdfPTable(1);
        //PdfPTable tblHeader = new PdfPTable(1);
        //PdfPTable tblFooter = new PdfPTable(2);
        //PdfPTable tblSignOff = new PdfPTable(1);
        //// ===========================================================================================
        //// Retrieving Quotation Master & Detail Data
        //// --------------------------------------------------------------------------------------------
        //int TotalCount = 0;
        //List<Entity.Quotation> lstQuot = new List<Entity.Quotation>();
        //lstQuot = BAL.QuotationMgmt.GetQuotationList(pQuotID, HttpContext.Current.Session["LoginUserID"].ToString(), 1, 1000, out TotalCount);
        //// -------------------------------------------------------------------------------------------------------------
        //DataTable dtItem = new DataTable();
        //if (lstQuot.Count > 0)
        //    dtItem = BAL.QuotationDetailMgmt.GetQuotationDetail(lstQuot[0].QuotationNo);

        //// -------------------------------------------------------------------------------------------------------------
        //int totrec = 0;
        //List<Entity.Customer> lstCust = new List<Entity.Customer>();
        //if (lstQuot.Count > 0)
        //    lstCust = BAL.CustomerMgmt.GetCustomerList(lstQuot[0].CustomerID, "admin", 1, 1000, out totrec);

        //DataTable dtContact = new DataTable();
        //if (lstQuot.Count > 0)
        //    dtContact = BAL.CustomerContactsMgmt.GetCustomerContactsDetail(lstQuot[0].CustomerID);
        ////-------------------------------------------------------------------------------------------------
        //List<Entity.OrganizationStructure> lstOrg = new List<Entity.OrganizationStructure>();
        //if (lstQuot.Count > 0)
        //    lstOrg = BAL.OrganizationStructureMgmt.GetOrganizationStructureList("001", 1, 1000, out totrec);
        //// -------------------------------------------------------------------------------------------------------------
        //List<Entity.OrganizationBank> lstBank = new List<Entity.OrganizationBank>();
        //if (lstQuot.Count > 0)
        //    lstBank = BAL.OrganizationStructureMgmt.GetOrganizationBankListByCompID(lstQuot[0].CompanyID, 1, 1000, out totrec);
        //// ------------------------------------------------------------------------------
        //List<Entity.OtherCharge> lstCharge = new List<Entity.OtherCharge>();
        //lstCharge = BAL.OtherChargeMgmt.GetOtherChargeList();
        //// ------------------------------------------------------------------------------
        //List<Entity.DocPrinterSettings> lstPrinter = new List<Entity.DocPrinterSettings>();
        //lstPrinter = BAL.CommonMgmt.GetDocPrinterSettings(HttpContext.Current.Session["LoginUserID"].ToString(), "Quotation");

        //Int64 ProdDetail_Lines = 20;
        //ProdDetail_Lines = (lstPrinter.Count > 0) ? lstPrinter[0].ProdDetail_Lines : 20;
        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //// Declaring PDF Document Object
        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

        //Document pdfDoc = pdf.initiatePage(lstPrinter);
        //MemoryStream ms = new MemoryStream();
        //PdfWriter pdfw = PdfWriter.GetInstance(pdfDoc, ms);
        //pdfw.PdfVersion = PdfWriter.VERSION_1_6;
        //pdfw.CompressionLevel = PdfStream.BEST_COMPRESSION;
        //pdfw.SetFullCompression();

        //// ===========================================================================================
        //// Printing Heading
        //// ===========================================================================================

        //if (lstQuot.Count > 0)
        //{
        //    // -------------------------------------------------------------------------------------
        //    //  Defining : Quotation Master Information
        //    // -------------------------------------------------------------------------------------
        //    #region Section >>>> Quotation Master Information
        //    int[] column_tblMember = { 25, 20, 33, 22 };
        //    tblMember.SetWidths(column_tblMember);
        //    tblMember.SpacingBefore = 8f;
        //    tblMember.LockedWidth = true;

        //    PdfPTable tblNested2 = new PdfPTable(4);
        //    int[] column_tblNested2 = { 24, 35, 15, 26 };
        //    tblNested2.SetWidths(column_tblNested2);

        //    tblNested2.AddCell(pdf.setCell("Offer No.", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].QuotationNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell("Date", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].QuotationDate.ToString("dd-MMM-yyyy"), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell("Contact Person", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].QuotationKindAttn, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell("Executive Name", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].CreatedEmployeeName, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell("Contact No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + lstQuot[0].CreatedEmployeeMobileNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 3, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell("GST No ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + objAuth.GSTNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell("PAN No", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));
        //    tblNested2.AddCell(pdf.setCell(": " + objAuth.PANNo, pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf4, 1, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, Rectangle.NO_BORDER));

        //    tblMember.AddCell(pdf.setCell("Quotation", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf4, 4, Element.ALIGN_CENTER, Element.ALIGN_TOP, 13));
        //    tblMember.AddCell(pdf.setCell(pdf.Quotation_CustomerInfo(lstCust), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
        //    tblMember.AddCell(pdf.setCell(tblNested2, pdf.WhiteBaseColor, pdf.objContentFontDataBlack, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 13));
        //    #endregion

        //    // -------------------------------------------------------------------------------------
        //    //  Defining : Quotation Product Detail
        //    // -------------------------------------------------------------------------------------
        //    #region Section >>>> Quotation Product Detail

        //    var sumDis = dtItem.AsEnumerable().Sum(x => x.Field<decimal>("DiscountAmt"));

        //    if (sumDis > 0)
        //    {
        //        string[,] myColStruc = {
        //        { "Sr.No", "counter", "2", "1", "4", "12" },
        //        { "Description", "ProductName", "2", "1", "4", "12" },
        //        { "Unit", "Unit", "2", "1", "4", "12" },
        //        { "Qty", "Quantity", "2", "1", "4", "12" },
        //        { "Rate", "UnitRate", "2", "1", "4", "12" },
        //        { "Dis.%", "DiscountPercent", "2", "1", "4", "12" },
        //        { "Amount", "Amount", "2", "1", "4", "12" }};

        //        int[] column_tblDetailNested71 = { 6, 43, 6, 6, 10, 8, 12 };
        //        tblDetail.SetWidths(column_tblDetailNested71);
        //        tblDetail.SpacingBefore = 0f;
        //        tblDetail.LockedWidth = true;
        //        tblDetail.SplitLate = false;
        //        tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

        //        tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("quotation", dtItem, myColStruc, column_tblDetailNested71, ProdDetail_Lines, 12, pdf.fnCalibri8, 1, "", 1), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 7, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        //        tblDetail.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].QuotationFooter, 0, true), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 5, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
        //        tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
        //    }
        //    else
        //    {
        //        tblDetail = new PdfPTable(6);

        //        string[,] myColStruc = {
        //        { "Sr.No", "counter", "2", "1", "4", "12" },
        //        { "Description", "ProductName", "2", "1", "4", "12" },
        //        { "Unit", "Unit", "2", "1", "4", "12" },
        //        { "Qty", "Quantity", "2", "1", "4", "12" },
        //        { "Rate", "UnitRate", "2", "1", "4", "12" },
        //        { "Amount", "Amount", "2", "1", "4", "12" }};

        //        int[] column_tblDetailNested72 = { 6, 60, 6, 6, 10, 12 };
        //        tblDetail.SetWidths(column_tblDetailNested72);
        //        tblDetail.SpacingBefore = 0f;
        //        tblDetail.LockedWidth = true;
        //        tblDetail.SplitLate = false;
        //        tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;

        //        tblDetail.AddCell(pdf.setCell(pdf.Table_ProductDetail("quotation", dtItem, myColStruc, column_tblDetailNested72, ProdDetail_Lines, 12, pdf.fnCalibri8, 1, "", 1), pdf.WhiteBaseColor, pdf.fnArialBold8, 0, 6, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));

        //        tblDetail.AddCell(pdf.setCell(pdf.TermsCondition(lstQuot[0].QuotationFooter, 0, true), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));
        //        tblDetail.AddCell(pdf.setCell(pdf.BillTaxAndAmount(lstQuot, dtItem, lstCharge, 0), pdf.WhiteBaseColor, pdf.objContentFontDataBlack, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 15));

        //    }
        //    // ****************************************************************
        //    //netAmount = ((totAmount + befAmt) + (totGST + befGST) + aftAmt + totRNDOff);
        //    Decimal netAmount = lstQuot[0].NetAmt;
        //    PdfPTable tblAmount1 = new PdfPTable(2);
        //    int[] column_tblAmount1 = { 60, 40 };
        //    tblAmount1.SetWidths(column_tblAmount1);
        //    string NetAmtInWords = BAL.CommonMgmt.ConvertNumbertoWords((int)netAmount);
        //    tblAmount1.AddCell(pdf.setCell("Total Amount :", pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));

        //    if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
        //        tblAmount1.AddCell(pdf.setCell(lstQuot[0].CurrencySymbol.Trim() + " " + netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        //    else
        //        tblAmount1.AddCell(pdf.setCell(netAmount.ToString("0.00"), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 3f, 1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 0));
        //    // -----------------------------------------------
        //    if (sumDis > 0)
        //    {
        //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
        //            tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
        //        else
        //            tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));

        //        tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));
        //    }
        //    else
        //    {
        //        if (!String.IsNullOrEmpty(lstQuot[0].CurrencySymbol.Trim()))
        //            tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + lstQuot[0].CurrencyName.Trim().ToUpper(), pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
        //        else
        //            tblDetail.AddCell(pdf.setCell("Amount In Words: " + NetAmtInWords + " RUPEES", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 4, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, 15));
        //        tblDetail.AddCell(pdf.setCell(tblAmount1, pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf4, 2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, 15));

        //    }
        //    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
        //    #endregion

        //    // -------------------------------------------------------------------------------------
        //    //  Defining : Terms & Condition
        //    // -------------------------------------------------------------------------------------
        //    #region Section >>>> Terms & Condition
        //    PdfPTable tblFootDetail = new PdfPTable(2);
        //    int[] column_tblFootDetail = { 80, 20 };
        //    tblFootDetail.SetWidths(column_tblFootDetail);

        //    tblFootDetail.AddCell(pdf.setCell("We hope you will find above rates in line with your requirement. We assure you of our best services with maximum", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, 0));
        //    tblFootDetail.AddCell(pdf.setCell("technical supports at all times.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    tblFootDetail.AddCell(pdf.setCell("Thanking you and awaiting for your valued order.", pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    tblFootDetail.AddCell(pdf.setCell(" ", pdf.WhiteBaseColor, pdf.fnCalibri8, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        //    tblFootDetail.AddCell(pdf.setCell("Bank Details:", pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    tblFootDetail.AddCell(pdf.setCell("Bank Name : " + ((lstQuot.Count > 0) ? lstQuot[0].BankName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    tblFootDetail.AddCell(pdf.setCell("Branch    : " + ((lstQuot.Count > 0) ? lstQuot[0].BranchName : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    tblFootDetail.AddCell(pdf.setCell("A/c No    : " + ((lstQuot.Count > 0) ? lstQuot[0].BankAccountNo : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    tblFootDetail.AddCell(pdf.setCell("IFSC Code : " + ((lstQuot.Count > 0) ? lstQuot[0].BankIFSC : ""), pdf.WhiteBaseColor, pdf.fnCalibri8, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));

        //    //tblFootDetail.AddCell(pdf.setCell(pdf.BankDetails(lstQuot, 0, 1), pdf.WhiteBaseColor, pdf.fnCalibriBold10, pdf.paddingOf2, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, Rectangle.NO_BORDER));
        //    // ---------------------------------------------------
        //    int[] column_tblFooter = { 80, 20 };
        //    tblFooter.SetWidths(column_tblFooter);
        //    tblFooter.SpacingBefore = 0f;
        //    tblFooter.LockedWidth = true;
        //    tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    tblFooter.AddCell(pdf.setCell(tblFootDetail, pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 7));
        //    tblFooter.AddCell(pdf.setCell(pdf.AuthorisedSignature(objAuth.CompanyName, 0), pdf.WhiteBaseColor, pdf.fnCalibriBold8, 0, 1, Element.ALIGN_LEFT, Element.ALIGN_BOTTOM, 11));
        //    // -------------------------------------------------------------------------------------
        //    //  Defining : Sign Off
        //    // -------------------------------------------------------------------------------------
        //    tblSignOff.SpacingBefore = 0f;
        //    tblSignOff.LockedWidth = true;
        //    tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    tblSignOff.AddCell(pdf.setCell("SUBJECT TO " + lstOrg[0].CityName.ToUpper() + "JURIDICTION ", pdf.WhiteBaseColor, pdf.fnCalibriBold8, pdf.paddingOf8, 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, Rectangle.BOX));
        //    #endregion
        //}
        //htmlClose = "</body></html>";

        //// =*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*
        ////string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
        ////string sFileName = "SalesOrder_" + lstQuot[0].pkID.ToString() + ".pdf";

        //string sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/PDF/");
        //string sFileName = lstQuot[0].QuotationNo.Replace("/", "-").ToString() + ".pdf";
        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //// Header & Footer ..... Settings
        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //ITextEvents clHeaderFooter = new ITextEvents();
        //pdfw.PageEvent = clHeaderFooter;
        ////clHeaderFooter.Title = lstEntity[0].OrgName;
        //clHeaderFooter.HeaderFont = pdf.objHeaderFont18;
        //clHeaderFooter.FooterFont = pdf.objFooterFont;
        ////clHeaderFooter.FooterText = lstEntity[0].OrgAddress;
        ////clHeaderFooter.WallPaper = Server.MapPath("images/Sanpar_Logo.png");
        //iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);

        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //// Declaring Stylesheet ......
        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //StyleSheet objStyle = new StyleSheet();
        //objStyle.LoadTagStyle("body", "font-family", "Arial, Helvetica, sans-serif");
        //objStyle.LoadTagStyle("body", "font-size", "12pt");
        //objStyle.LoadTagStyle("body", "color", "black");
        //objStyle.LoadTagStyle("body", "position", "relative");
        //objStyle.LoadTagStyle("body", "margin", "0 auto");

        //htmlparser.SetStyleSheet(objStyle);

        //// ------------------------------------------------------------------------------------------------
        //// pdfDOC >>> Open
        //// ------------------------------------------------------------------------------------------------
        //pdfDoc.Open();


        //// >>>>>> Opening : HTML & BODY
        //htmlparser.Parse(new StringReader((htmlOpen.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));

        //// >>>>>> Adding Organization Name 
        ////tableHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        ////pdfDoc.Add(tableHeader);

        //// >>>>>> Adding Quotation Header
        //tblSubject.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        //tblSubject.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //pdfDoc.Add(tblSubject);

        //// >>>>>> Adding Quotation Master Information Table
        //tblMember.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        //tblMember.HorizontalAlignment = Element.ALIGN_CENTER;
        //pdfDoc.Add(tblMember);


        //// >>>>>> Adding Quotation Header
        //tblHeader.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        //tblHeader.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //pdfDoc.Add(tblHeader);

        //// >>>>>> Adding Quotation Detail Table
        //tblDetail.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        //tblDetail.HorizontalAlignment = Element.ALIGN_CENTER;
        //pdfDoc.Add(tblDetail);

        //// >>>>>> Adding Quotation Footer
        //tblFooter.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        //tblFooter.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //pdfDoc.Add(tblFooter);

        //// >>>>>> Adding Quotation Header
        //tblSignOff.TotalWidth = (pdfDoc.PageSize.Width - (pdfDoc.LeftMargin + pdfDoc.RightMargin));
        //tblSignOff.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //pdfDoc.Add(tblSignOff);

        //// >>>>>> Closing : HTML & BODY
        //htmlparser.Parse(new StringReader((htmlClose.ToString()).Replace("\r", "").Replace("\n", "").Replace("  ", "")));
        //// =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        //pdfDoc.Close();
        //pdfDoc.Dispose();
        //string smallFileName = HttpContext.Current.Session["LoginUserID"].ToString() + "-Tempsmall.pdf";
        //byte[] content = ms.ToArray();
        //FileStream fs = new FileStream(sPath + smallFileName, FileMode.Create);
        //fs.Write(content, 0, (int)content.Length);
        //fs.Close();
        //fs.Dispose();
        //string pdfFileName = "";
        //pdfFileName = sPath + sFileName;
        //RecompressPDF(sPath + smallFileName, pdfFileName);
        #endregion

        //protected void txtComplaintNo_TextChanged(object sender, EventArgs e)
        //{
           
        //}

        protected void drpComplaintNo_TextChanged(object sender, EventArgs e)
        {
            hdnComplaintNo.Value = drpComplaintNo.SelectedValue;
            if (!String.IsNullOrEmpty(hdnComplaintNo.Value))
            {
                // ----------------------------------------------------------------------------------------------------------
                List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();

                if (!String.IsNullOrEmpty(hdnComplaintNo.Value))
                    lstEntity = BAL.CommonMgmt.GetSRNO(Convert.ToInt64(hdnComplaintNo.Value));

                if (!String.IsNullOrEmpty(hdnComplaintNo.Value) && String.IsNullOrEmpty(drpComplaintNo.SelectedValue))
                    drpComplaintNo.SelectedValue = (lstEntity.Count > 0) ? lstEntity[0].ComplaintNo : "";
                txtPanelSRNo.Text = lstEntity[0].PanelSRNo.ToString();
                txtProductSRNo.Text = lstEntity[0].ProductSRNo.ToString();
                //-------------------------------------------------------------------------------------------------------------
                if (!String.IsNullOrEmpty(hdnComplaintNo.Value))
                    lstEntity = BAL.CommonMgmt.GetComplaintList(Convert.ToInt64(hdnComplaintNo.Value));

                if (!String.IsNullOrEmpty(hdnComplaintNo.Value) && String.IsNullOrEmpty(drpComplaintNo.SelectedValue))
                    drpComplaintNo.SelectedValue = (lstEntity.Count > 0) ? lstEntity[0].ComplaintNo : "";
                txtNameOfCustomer.Text = lstEntity[0].NameOfCustomer;
                txtContactNo1.Text = lstEntity[0].CustmoreMobileNo;
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();
            }
            else
            {
                drpComplaintNo.Focus();
            }
        }
    }

}