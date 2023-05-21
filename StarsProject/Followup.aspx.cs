using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Web.Services;
using System.IO;
using System.Text;
//using RestSharp;

namespace StarsProject
{
    public partial class Followup : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;
    
        private static DataTable dtDetail;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            myNotificationPanel.FSaveEmailClick += btnFSaveEmailClick;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //if (Session["logindetail"] == null)
            //{
            //    ClientScript.RegisterStartupScript(GetType(), "closePage", "<script type=\"text/JavaScript\">window.close();");
            //    Response.Redirect("Default.aspx");
            //}
            // -----------------------------------------------------------------
            if (!IsPostBack)
            {
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                hdnSerialKey.Value = Session["SerialKey"].ToString();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["passFlag"]))
                {
                    string tmpval = Request.QueryString["passFlag"].ToString().Trim();
                    btnReset.Visible = (tmpval == "true") ? true : false;
                }
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    string tmpval = BAL.CommonMgmt.GetConstant("AllowBackDatedFollowup", 0, 1);
                    hdnAllowBackDatedFollowup.Value = (!String.IsNullOrEmpty(tmpval)) ? tmpval : "Yes";

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        if (!String.IsNullOrEmpty(Request.QueryString["CustomerID"]) && !String.IsNullOrEmpty(Request.QueryString["CustomerName"]))
                        {
                            hdnCustomerID.Value = Request.QueryString["CustomerID"].ToString();
                            txtCustomerName.Text = Request.QueryString["CustomerName"].ToString();
                            txtCustomerName.ReadOnly = true;
                            txtCustomerName_TextChanged(null, null);
                        }
                        // ----------------------------------------------------
                        // Below Section is to Generate FollowUp From Inquiry
                        // ----------------------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["InqID"]))
                        {
                            int TotalCount = 0;
                            Int64 inqID = Convert.ToInt64(Request.QueryString["InqID"]);
                            // -----------------------------------------------------------------------------------
                            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
                            lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(inqID, Session["LoginUserID"].ToString(), 1, 10000, out TotalCount);
                            if (lstEntity.Count > 0)
                            {
                                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                                txtCustomerName.Text = lstEntity[0].CustomerName;
                                // ---------------------------------------------------
                                BindFollowupList();
                                // ---------------------------------------------------
                                BindInquiryList(Convert.ToInt64(hdnCustomerID.Value));

                                int ic = 0;
                                foreach (ListItem li in drpInquiry.Items)
                                {
                                    if (li.Text.IndexOf(lstEntity[0].InquiryNo) != -1)
                                        break;
                                    // -----------------------------
                                    ic++;
                                }
                                drpInquiry.Items[ic].Selected = true;
                                hdnInquiryNo.Value = lstEntity[0].InquiryNo;
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["mode"]))
                            hdnEntryMode.Value = Request.QueryString["mode"].ToString();
                        // -------------------------------------
                        setLayout("Edit");
                        // -------------------------------------
                        if (!String.IsNullOrEmpty(hdnEntryMode.Value))
                        {
                            if (hdnEntryMode.Value == "view")
                                OnlyViewControls();
                        }
                    }
                }
                // ------------------------------------------------

            }
            else
            {
                if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                    BindFollowupList();
                // ----------------------------------------------------------------------
                // Audio File Upload On .... Page Postback
                // ----------------------------------------------------------------------
                myAudioControl.setAudioDataTable(uploadAudioGallery);

                //DataTable dtGall = new DataTable();
                //dtGall = (DataTable)Session["dtAudGallery"];
                //if (dtGall == null)
                //{
                //    List<Entity.AudioFiles> lstAudio = new List<Entity.AudioFiles>();
                //    lstAudio = BAL.AudioMgmt.GetAudioFiles(0, "", "-1");
                //    dtGall = PageBase.ConvertListToDataTable(lstAudio);
                //}
                //// ----------------------------------------------------------------------
                //if (uploadAudioGallery.PostedFile != null)
                //{
                //    if (uploadAudioGallery.PostedFile.FileName.Length > 0)
                //    {
                //        if (uploadAudioGallery.HasFile)
                //        {
                //            HttpFileCollection _HttpFileCollection = Request.Files;
                //            for (int i = 0; i < _HttpFileCollection.Count; i++)
                //            {
                //                HttpPostedFile _HttpPostedFile = _HttpFileCollection[i];
                //                if (_HttpPostedFile.ContentLength > 0)
                //                {
                //                    string ext = Path.GetExtension(Path.GetFileName(_HttpPostedFile.FileName));
                //                    if (ext == ".mp3" || ext == ".mp4" || ext == ".wav" || ext == ".aac")
                //                    {

                //                        try
                //                        {

                //                            DataRow dr = dtGall.NewRow();
                //                            dr["pkID"] = 0;
                //                            dr["KeyID"] = hdnpkID.Value;
                //                            dr["ModuleName"] = "FollowUp";
                //                            dr["FileName"] = filename1;
                //                            dr["FileType"] = type;

                //                            var plainTextBytes = Encoding.UTF8.GetBytes(_HttpPostedFile.InputStream.ToString());
                //                            string utfString = Convert.ToBase64String(plainTextBytes);

                //                            System.IO.Stream fs = _HttpPostedFile.InputStream;
                //                            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                //                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                //                            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                //                            dr["ContentData"] = base64String;
                //                            dtGall.Rows.Add(dr);
                //                            Session.Add("dtAudGallery", dtGall);
                //                        }
                //                        catch (Exception ex) { }
                //                    }
                //                    else
                //                        ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('Audio');", true);
                //                }

                //            }

                //        }
                //    }
                //}
            }
        }

        public void OnlyViewControls()
        {
            txtFollowupDate.ReadOnly = true;
            //drpCustomer.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            drpInquiry.Attributes.Add("disabled", "disabled");
            drpFollowupType.Attributes.Add("disabled", "disabled");
            drpPriority.Attributes.Add("disabled", "disabled");
            txtNextFollowupDate.ReadOnly = true;
            txtMeetingNotes.ReadOnly = true;
            chkNoFollowup.Enabled = false;
            btnSave.Visible = false;
            //btnFSaveEmail.Visible = false;
            btnReset.Visible = false;
            drpClosureReason.Attributes.Add("disabled", "disabled");
        }

        public void BindDropDown()
        {
            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("Inquiry");
            drpInquiryStatus.DataSource = lstDesig;
            drpInquiryStatus.DataValueField = "pkID";
            drpInquiryStatus.DataTextField = "InquiryStatusName";
            drpInquiryStatus.DataBind();
            //drpInquiryStatus.Items.Insert(0, new ListItem("-- Select Status --", ""));

            // ---------------- Report To List -------------------------------------
            List<Entity.InquiryStatus> lstOrgDept22 = new List<Entity.InquiryStatus>();
            lstOrgDept22 = BAL.InquiryStatusMgmt.GetInquiryStatusList("Followup");
            drpFollowupType.DataSource = lstOrgDept22;
            drpFollowupType.DataValueField = "pkID";
            drpFollowupType.DataTextField = "InquiryStatusName";
            drpFollowupType.DataBind();
            drpFollowupType.Items.Insert(0, new ListItem("-- Select --", ""));

            // ---------------- Inquiry Closure Reason-------------------------------------
            List<Entity.InquiryStatus> lstReason = new List<Entity.InquiryStatus>();
            lstReason = BAL.InquiryStatusMgmt.GetInquiryStatusList("DisQualifiedReason");
            drpClosureReason.DataSource = lstReason;
            drpClosureReason.DataValueField = "pkID";
            drpClosureReason.DataTextField = "InquiryStatusName";
            drpClosureReason.DataBind();
            drpClosureReason.Items.Insert(0, new ListItem("-- Select Reason --", "0"));
            // ---------------- Inquiry List -------------------------------------
            BindInquiryList(0);
        }

        public void BindInquiryList(Int64 pCustomerID)
        {
            List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
            if (pCustomerID != 0)
            {
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoListByCustomer(pCustomerID);
                // --------------------------------------------------
                //if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                //{
                //    lstEntity = lstEntity.Where(e => (e.InquiryStatus != "Close - Lost" && e.InquiryStatus != "Close - Success")).ToList();
                //}
                // --------------------------------------------------
                drpInquiry.Items.Clear();
                drpInquiry.DataValueField = "pkID";
                drpInquiry.DataTextField = "InquiryNoStatus";
                if (lstEntity.Count > 0)
                {
                    drpInquiry.DataSource = lstEntity;
                    drpInquiry.DataBind();
                }
                drpInquiry.Items.Insert(0, new ListItem("-- Select --", ""));
            }
            else
            {
                drpInquiry.Items.Clear();
                drpInquiry.Items.Insert(0, new ListItem("-- Select --", ""));
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.Followup> lstEntity = new List<Entity.Followup>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.FollowupMgmt.GetFollowupList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = (hdnEntryMode.Value != "continue") ? lstEntity[0].pkID.ToString() : "0";
                //hdnRating.Value = lstEntity[0].Rating.ToString();
                txtFollowupDate.Text = (hdnEntryMode.Value != "continue") ? lstEntity[0].FollowupDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                drpFollowupType.SelectedValue = lstEntity[0].InquiryStatusID.ToString();
                drpPriority.SelectedValue = lstEntity[0].FollowupPriority.ToString();
                chkNoFollowup.Checked = lstEntity[0].NoFollowup;
                if (!chkNoFollowup.Checked)
                {
                    drpClosureReason.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    drpClosureReason.Attributes.Remove("disabled");
                }
                drpClosureReason.SelectedValue = lstEntity[0].NoFollClosureID.ToString();
                // -----------------------------------------------------------------------------------
                BindInquiryList(lstEntity[0].CustomerID);
                // ----------------------------------------------------------------------------------- 
                if (!String.IsNullOrEmpty(lstEntity[0].InquiryNo))
                {
                    hdnInquiryNo.Value = lstEntity[0].InquiryNo;
                    if (drpInquiry.Items.FindByText(lstEntity[0].InquiryNoStatus) != null)
                        drpInquiry.Items.FindByText(lstEntity[0].InquiryNoStatus).Selected = true;
                }

                if (hdnEntryMode.Value != "continue")
                    txtMeetingNotes.Text = (!String.IsNullOrEmpty(lstEntity[0].MeetingNotes)) ? lstEntity[0].MeetingNotes : "";
                else
                    txtMeetingNotes.Text = "";

                //txtNextFollowupDate.Text = lstEntity[0].NextFollowupDate.ToString("dd-MM-yyyy");
                if (hdnEntryMode.Value != "continue")
                {
                    if (lstEntity[0].NextFollowupDate != SqlDateTime.MinValue.Value && (lstEntity[0].NextFollowupDate).Year > 1900)
                    {
                        txtNextFollowupDate.Text = lstEntity[0].NextFollowupDate.ToString("yyyy-MM-dd");
                    }
                    txtPreferredTime.Text = lstEntity[0].PreferredTime.ToString();
                }
                else
                {
                    txtNextFollowupDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtPreferredTime.Text = "";

                }
                // -----------------------------------------------------------------------------------       
                List<Entity.InquiryInfo> lstEntity1 = new List<Entity.InquiryInfo>();
                if (!String.IsNullOrEmpty(drpInquiry.SelectedValue))
                    lstEntity1 = BAL.InquiryInfoMgmt.GetInquiryInfoList(Convert.ToInt64(drpInquiry.SelectedValue), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                else
                    lstEntity1 = BAL.InquiryInfoMgmt.GetInquiryInfoList(0, Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                // -----------------------------------------------------------------------------------
                divStatus.Visible = (!String.IsNullOrEmpty(drpInquiry.SelectedValue)) ? true : false;
                if (lstEntity1.Count > 0)
                {
                    //drpInquiryStatus.Items.FindByValue(lstEntity[0].InquiryStatusID.ToString()).Selected = true; 
                    drpInquiryStatus.SelectedValue = lstEntity1[0].InquiryStatusID.ToString();
                }
                // -----------------------------------------------------------------------------------   
                BindFollowupList();
                // -------------------------------------
                myAudioControl.pageFilePrefix = "Audio-";
                myAudioControl.pageServerPath = "AudioFiles/";
                myAudioControl.pageModule = "FollowUp";
                myAudioControl.pageModuleType = "Audio";
                myAudioControl.pageKeyID = hdnpkID.Value;
                myAudioControl.BindAudioList();
                // ----------------------------------------------------
                if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                {
                    myNotificationPanel.CustomerID = hdnCustomerID.Value;
                    myNotificationPanel.BindNotificationData();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData();
        }

        private void btnFSaveEmailClick()
        {
            SendAndSaveData();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void SendAndSaveData()
        {
            int ReturnCode = 0;
            string ReturnMsg = "";
            string strErr = "";
            Int64 ReturnFollowupPKID = 0;
            _pageValid = true;


            DateTime dt2 = DateTime.Now;
            if (hdnSerialKey.Value == "JAYJ-ALAR-AMBR-ICKS" && chkNoFollowup.Checked == true)
            {

            }
            else
            {
                if ((String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0") ||
                    (String.IsNullOrEmpty(txtMeetingNotes.Text)) ||
                    String.IsNullOrEmpty(txtFollowupDate.Text) || String.IsNullOrEmpty(txtNextFollowupDate.Text) ||
                    String.IsNullOrEmpty(drpFollowupType.SelectedValue) || String.IsNullOrEmpty(drpPriority.SelectedValue))
                {
                    _pageValid = false;


                    if (String.IsNullOrEmpty(drpFollowupType.SelectedValue))
                        strErr += "<li>" + "Followup Type Selection is required." + "</li>";

                    if (String.IsNullOrEmpty(txtFollowupDate.Text))
                        strErr += "<li>" + "Followup Date is Required." + "</li>";

                    if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                        strErr += "<li>" + "Select Proper Customer From List." + "</li>";

                    if (String.IsNullOrEmpty(txtMeetingNotes.Text))
                        strErr += "<li>" + "Meeting Notes is Required." + "</li>";

                    if (String.IsNullOrEmpty(txtNextFollowupDate.Text))
                        strErr += "<li>" + "Next FollowUp Date is Required." + "</li>";

                    if (String.IsNullOrEmpty(drpPriority.SelectedValue))
                        strErr += "<li>" + "Priority Selection is required." + "</li>";
                }
            }
            // ----------------------------------------
            if (!String.IsNullOrEmpty(txtNextFollowupDate.Text))
            {
                if (!String.IsNullOrEmpty(txtFollowupDate.Text) && !String.IsNullOrEmpty(txtNextFollowupDate.Text))
                {
                    if (Convert.ToDateTime(txtNextFollowupDate.Text) < Convert.ToDateTime(txtFollowupDate.Text))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Next Followup Date should be greater than Followoup Date." + "</li>";
                    }
                }
                DateTime dt1;
                if (!String.IsNullOrEmpty(txtPreferredTime.Text))
                    dt1 = Convert.ToDateTime((txtNextFollowupDate.Text + " " + txtPreferredTime.Text).Trim());
                else
                    dt1 = Convert.ToDateTime((txtNextFollowupDate.Text + " 23:59").Trim());

                //DateTime dt1 = Convert.ToDateTime((txtNextFollowupDate.Text + " " + txtPreferredTime.Text).Trim());
                // if (hdnAllowBackDatedFollowup.Value.ToLower() == "no" && dt1<dt2 && (hdnpkID.Value == "" || hdnpkID.Value == "0"))
                if (hdnAllowBackDatedFollowup.Value.ToLower() == "no" && dt1 < dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Backdated Next Followup is not allowed." + "</li>";
                }
            }

            if ((drpInquiryStatus.SelectedItem.Text == "Close - Lost" || drpInquiryStatus.SelectedItem.Text == "Lost") && (String.IsNullOrEmpty(drpClosureReason.SelectedValue) || drpClosureReason.SelectedValue == "0"))
            {
                _pageValid = false;
                strErr += "<li>" + "Closure Reason is Required for Close Lost " + "</li>";
            }
            if ((drpInquiryStatus.SelectedItem.Text != "Close - Lost" && drpInquiryStatus.SelectedItem.Text != "Lost") && (drpClosureReason.SelectedValue != "0"))
            {
                _pageValid = false;
                strErr += "<li>" + "Closure Reason Selection Only on Close - Lost" + "</li>";
            }
            // --------------------------------------------------------------
            Entity.Followup objEntity = new Entity.Followup();
            if (_pageValid)
            {
                //hdnInquiryNo.Value = FetchInquiryNo(drpInquiry.SelectedValue);

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.FollowupDate = Convert.ToDateTime(txtFollowupDate.Text);
                objEntity.NextFollowupDate = (!String.IsNullOrEmpty(txtNextFollowupDate.Text)) ? Convert.ToDateTime(txtNextFollowupDate.Text) : SqlDateTime.MinValue.Value;
                objEntity.PreferredTime = txtPreferredTime.Text;
                objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.InquiryNo = (!String.IsNullOrEmpty(hdnInquiryNo.Value)) ? hdnInquiryNo.Value : "";
                objEntity.MeetingNotes = txtMeetingNotes.Text;
                objEntity.InquiryStatusID = (!String.IsNullOrEmpty(drpFollowupType.SelectedValue)) ? Convert.ToInt64(drpFollowupType.SelectedValue) : Convert.ToInt64("0");
                //objEntity.Rating = (!String.IsNullOrEmpty(hdnRating.Value)) ? Convert.ToInt64(hdnRating.Value) : 1;
                objEntity.NoFollowup = (drpInquiryStatus.SelectedItem.Text == "Close - Lost" || drpInquiryStatus.SelectedItem.Text == "Lost") ? true : chkNoFollowup.Checked;
                objEntity.NoFollClosureID = (!String.IsNullOrEmpty(drpClosureReason.SelectedValue)) ? Convert.ToInt64(drpClosureReason.SelectedValue) : Convert.ToInt64("0");
                objEntity.FollowupPriority = (!String.IsNullOrEmpty(drpPriority.SelectedValue)) ? Convert.ToInt64(drpPriority.SelectedValue) : Convert.ToInt64("0");
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.FollowupMgmt.AddUpdateFollowup(objEntity, out ReturnCode, out ReturnMsg, out ReturnFollowupPKID);
                strErr += "<li>" + ReturnMsg + "</li>";
                // --------------------------------------------------------------
                if (ReturnCode > 0)
                {
                    //When record saves/updates then save button will be disabled.
                    btnSave.Disabled = true;
                    try
                    {
                        string notificationMsg = "";
                        if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
                            notificationMsg = "FollowUp Updated For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                        else
                            notificationMsg = "FollowUp Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());

                        BAL.CommonMgmt.SendNotification_Firebase("FollowUp", notificationMsg, Session["LoginUserID"].ToString(), 0);
                        BAL.CommonMgmt.SendNotificationToDB("FollowUp", ReturnFollowupPKID, notificationMsg, Session["LoginUserID"].ToString(), 0);
                    }
                    catch (Exception ex)
                    { }

                    //btnFSaveEmail.Disabled = true;
                    Int64 para1, para2;
                    para1 = (!String.IsNullOrEmpty(drpInquiryStatus.SelectedValue)) ? Convert.ToInt64(drpInquiryStatus.SelectedValue) : Convert.ToInt64("0");
                    para2 = (!String.IsNullOrEmpty(drpClosureReason.SelectedValue)) ? Convert.ToInt64(drpClosureReason.SelectedValue) : Convert.ToInt64("0");
                    BAL.CommonMgmt.setInquiryStatusFromFollowup(hdnInquiryNo.Value, para1, para2);
                }

                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                // SAVE - Audio Gallery
                // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                myAudioControl.SaveAudioFile(ReturnFollowupPKID.ToString());
                if (ReturnCode > 0)
                    myAudioControl.destroyAudioGallery();

                //int ReturnCode1 = 0;
                //String ReturnMsg1 = "";
                //BAL.AudioMgmt.DeleteAudioByKeyID("FollowUp", ReturnFollowupPKID.ToString(), out ReturnCode1, out ReturnMsg1);
                //DataTable dtImgGall = new DataTable();
                //dtImgGall = (DataTable)Session["dtAudGallery"];

                //if (dtImgGall != null)
                //{
                //    foreach (DataRow dr in dtImgGall.Rows)
                //    {
                //        if (dr.RowState.ToString() != "Deleted")
                //        {
                //            Entity.AudioFiles lstObj = new Entity.AudioFiles();
                //            lstObj.pkID = 0;
                //            lstObj.ModuleName = "FollowUp";
                //            lstObj.KeyID = ReturnFollowupPKID.ToString();
                //            String tmpNewFileName = "";
                //            if (dr["FileName"].ToString().Contains("Audio-" + ReturnFollowupPKID.ToString()))
                //                tmpNewFileName = dr["FileName"].ToString();
                //            else
                //                tmpNewFileName = "Audio-" + ReturnFollowupPKID.ToString() + "-" + dr["FileName"].ToString();

                //            lstObj.FileName = tmpNewFileName;
                //            lstObj.FileType = dr["FileType"].ToString();
                //            // -------------------------------------------------------------- Insert/Update Record
                //            BAL.AudioMgmt.AddUpdateAudio(lstObj, out ReturnCode1, out ReturnMsg1);


                //            String tmpFile = Server.MapPath("AudioFiles/") + tmpNewFileName;
                //            System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(dr["ContentData"].ToString()));
                //        }
                //    }
                //}
            }
            // ------------------------------------------------------
            // Section : Send 'Thank You' Email
            // ------------------------------------------------------
            Entity.Authenticate objAuth = new Entity.Authenticate();
            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];
            String sendEmailFlag = BAL.CommonMgmt.GetConstant("FOLLOWUP", 0, objAuth.CompanyID).ToLower();
            // ------------------------------------------------------
            if (myNotificationPanel.checkEmail.ToLower() == "true")
            {
                if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                {
                    try
                    {
                        // Sending Email Notification ...
                        String respVal = "", tmpEmailAddress = "";
                        tmpEmailAddress = (objEntity.CustomerID > 0) ? BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID) : "";

                        if (!String.IsNullOrEmpty(tmpEmailAddress) && tmpEmailAddress.ToUpper() != "NULL")
                            respVal = BAL.CommonMgmt.SendEmailNotifcation("FOLLOWUP", Session["LoginUserID"].ToString(), 0, tmpEmailAddress);

                        strErr += "<li>" + ReturnMsg + " and Email Sent Successfully !" + "</li>";
                    }
                    catch (Exception ex)
                    {
                        strErr += "<li>" + ReturnMsg + " and Sending Email Failed !" + "</li>";
                    }
                }
            }
            // ------------------------------------------------------
            // Section : Send 'Thank You' SMS
            // ------------------------------------------------------
            if (ReturnCode > 0 && myNotificationPanel.checkSMS.ToLower() == "true")
            {
                try
                {
                    int totrec;
                    List<Entity.Customer> lstCust = new List<Entity.Customer>();
                    lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), "admin", 1, 1000, out totrec);
                    // Sending Notification ...
                    if (lstCust.Count > 0)
                    {
                        String respVal = "", tmpContactNo = "";
                        tmpContactNo = (!String.IsNullOrEmpty(myNotificationPanel.getSMSContacts)) ? myNotificationPanel.getSMSContacts : "";

                        if (!String.IsNullOrEmpty(tmpContactNo) && tmpContactNo.ToUpper() != "0")
                            respVal = BAL.CommonMgmt.SendSMSNotifcation("FOLLOWUP", Session["LoginUserID"].ToString(), tmpContactNo);

                        if (respVal.ToLower() == "success")
                            strErr += "<li>" + ReturnMsg + " and SMS Sent Successfully !" + "</li>";
                        else
                            strErr += "<li>" + ReturnMsg + " and Sending SMS Failed !" + "</li>";
                    }
                    else
                    {
                        strErr += "<li> Customer Not Found and Sending SMS Failed !" + "</li>";
                    }
                }
                catch (Exception ex)
                {
                    strErr += "<li>" + ReturnMsg + " and Sending SMS Failed !" + "</li>";
                }
            }
            // ------------------------------------------------------
            // Section : Send 'Thank You' WhatsApp
            // ------------------------------------------------------
            if (myNotificationPanel.checkWhatsapp.ToLower() == "true")
            {
                try
                {
                    string sendSMSUri = "";
                    //System.Text.StringBuilder sbPostData = new System.Text.StringBuilder();
                    //sbPostData.AppendFormat("authkey={0}", authKey);
                    //sbPostData.AppendFormat("&mobiles={0}", pMobileNos);
                    //sbPostData.AppendFormat("&message={0}", message);
                    //sbPostData.AppendFormat("&sender={0}", senderId);
                    //sbPostData.AppendFormat("&route={0}", SMSType);
                    sendSMSUri = "https://api.whatsapp.com/send?phone=+919898621973&text=HelloBoss";

                    System.Net.HttpWebRequest httpWReq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sendSMSUri);       //Create HTTPWebrequest
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    //byte[] data = encoding.GetBytes(sbPostData.ToString());
                    httpWReq.Method = "POST";
                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    //httpWReq.ContentLength = data.Length;
                    //using (System.IO.Stream stream = httpWReq.GetRequestStream())
                    //{
                    //    stream.Write(data, 0, data.Length);
                    //}
                    System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)httpWReq.GetResponse();   //Get the response
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseString = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                    // ---------------------------------------------------------------

                    // Sending Email Notification ...
                    //String respVal = "", tmpContactNo = "";
                    //int totrec;
                    //List<Entity.Customer> lstCust = new List<Entity.Customer>();
                    //lstCust = BAL.CustomerMgmt.GetCustomerList(objEntity.CustomerID, "admin", 1, 1000, out totrec);
                    //// Sending Notification ...
                    //if (lstCust.Count > 0)
                    //{
                    //    if (!String.IsNullOrEmpty(lstCust[0].ContactNo1) && !String.IsNullOrEmpty(lstCust[0].ContactNo2))
                    //        tmpContactNo = lstCust[0].ContactNo1 + "," + lstCust[0].ContactNo2;
                    //    else if (!String.IsNullOrEmpty(lstCust[0].ContactNo1) && String.IsNullOrEmpty(lstCust[0].ContactNo2))
                    //        tmpContactNo = lstCust[0].ContactNo1;
                    //    else if (String.IsNullOrEmpty(lstCust[0].ContactNo1) && !String.IsNullOrEmpty(lstCust[0].ContactNo2))
                    //        tmpContactNo = lstCust[0].ContactNo2;

                    //    if (!String.IsNullOrEmpty(tmpContactNo) && tmpContactNo.ToUpper() != "0")
                    //        respVal = BAL.CommonMgmt.SendWhatsApp("FOLLOWUP", Session["LoginUserID"].ToString(), tmpContactNo);

                    //    if (respVal.ToLower() == "success")
                    //        strErr += "<li>" + ReturnMsg + " and SMS Sent Successfully !" + "</li>";
                    //    else
                    //        strErr += "<li>" + ReturnMsg + " and Sending SMS Failed !" + "</li>";
                    //}
                    //else
                    //{
                    //    strErr += "<li>Customer Not Found !" + "</li>";
                    //}
                }
                catch (Exception ex)
                {
                    strErr += "<li>" + ReturnMsg + " and Sending SMS Failed !" + "</li>";
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

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            //drpFollowupType.Items.FindByText("Telephonic").Selected = true;
            drpFollowupType.SelectedValue = "";
            drpPriority.SelectedValue = "1";
            drpInquiry.Items.Clear();
            drpInquiry.Items.Insert(0, new ListItem("-- Select --", ""));
            divStatus.Visible = (!String.IsNullOrEmpty(drpInquiry.SelectedValue)) ? true : false;
            txtNextFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtMeetingNotes.Text = "";
            chkNoFollowup.Checked = false;
            txtPreferredTime.Text = "";
            if (divStatus.Visible)
                drpClosureReason.Attributes.Add("disabled", "disabled");
            txtFollowupDate.Focus();
            btnSave.Disabled = false;
            // -----------------------------------------
            myAudioControl.destroyAudioGallery();
            myAudioControl.pageFilePrefix = "Audio-";
            myAudioControl.pageServerPath = "AudioFiles/";
            myAudioControl.pageModule = "FollowUp";
            myAudioControl.pageModuleType = "Audio";
            myAudioControl.pageKeyID = "-1";
            myAudioControl.BindAudioList(0, "FollowUp", "-1");
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
            {
                BindInquiryList(Convert.ToInt64(hdnCustomerID.Value));
                myNotificationPanel.CustomerID = hdnCustomerID.Value;
                myNotificationPanel.BindNotificationData();
            }
            else
                BindInquiryList(0);
            // ------------------------------
            drpInquiry.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteFollowup(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.FollowupMgmt.DeleteFollowup(pkID, out ReturnCode, out ReturnMsg);
            BAL.AudioMgmt.DeleteAudioByKeyID("FollowUp", pkID.ToString(), out ReturnCode, out ReturnMsg);
            string rootFolderPath = System.Web.HttpContext.Current.Server.MapPath("AudioFiles");
            string filesToDelete = @"Audio-" + pkID.ToString() + "-" + "*.*";   // Only delete DOC files containing "DeleteMe" in their filenames
            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            foreach (string file in fileList)
            {
                System.IO.File.Delete(file);
            }
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void drpInquiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int TotalCount;
            hdnInquiryNo.Value = FetchInquiryNo(drpInquiry.SelectedValue);
            divStatus.Visible = (!String.IsNullOrEmpty(hdnInquiryNo.Value)) ? true : false;
            // ----------------------------------------
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:resetClosure();", true);
        }
        public string FetchInquiryNo(String pInqID)
        {
            int totrec;
            string retVal = "";
            if (!String.IsNullOrEmpty(pInqID) && pInqID != "0")
            {
                List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(Convert.ToInt64(pInqID), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totrec);
                if (lstEntity.Count > 0)
                {
                    drpInquiryStatus.SelectedValue = lstEntity[0].InquiryStatusID.ToString();
                    retVal = lstEntity[0].InquiryNo;
                }
            }
            return retVal;
        }
        public void BindFollowupList()
        {
            int TotRec;
            List<Entity.Followup> lstEntity = new List<Entity.Followup>();
            lstEntity = BAL.FollowupMgmt.GetDashboardFollowupTimeline(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 100000, out TotRec);
            rptFollowupTrail.DataSource = lstEntity;
            rptFollowupTrail.DataBind();
            lnkPriorFollowup.Visible = (lstEntity.Count > 0) ? true : false;
        }

        protected void rptFollowupTrail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnitemid = ((HiddenField)e.Item.FindControl("hdnItemID"));
                HiddenField hdn1 = ((HiddenField)e.Item.FindControl("hdnNextFollowup"));
                System.Web.UI.HtmlControls.HtmlGenericControl dv = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("ltrNextFollowup"));
                if (String.IsNullOrEmpty(hdn1.Value))
                {
                    dv.InnerText = "Not Applied";
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(hdn1.Value);
                    if (dt.Year < 2000)
                        dv.InnerText = "Not Applied";
                }
                // ------------------------------------------------
                if (hdnpkID.Value == hdnitemid.Value)
                {

                }
            }
        }

        protected void chkNoFollowup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNoFollowup.Checked)
                drpClosureReason.Attributes.Remove("disabled");
            else
                drpClosureReason.Attributes.Add("disabled", "disabled");
        }

        protected void drpInquiryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpInquiryStatus.SelectedItem.Text == "Close - Lost" || drpInquiryStatus.SelectedItem.Text == "Lost")
            {
                drpClosureReason.Attributes.Remove("disabled");
            }
            else
            {
                drpClosureReason.Attributes.Add("disabled", "disabled");
            }
        }
    }
}