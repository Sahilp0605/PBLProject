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
    public partial class InquiryInfoJJB : System.Web.UI.Page
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
                BindDropDown();
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
                            else
                            {
                                divFollowupNotes.Visible = false;
                                divNextFollowup.Visible = false;
                            }

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
            txtInquiryNo.ReadOnly = true;
            txtInquiryDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtReferenceName.ReadOnly = true;
            txtMeetingNotes.ReadOnly = true;
            txtFollowupNotes.ReadOnly = true;
            txtFollowupDate.ReadOnly = true;
            txtPreferredTime.ReadOnly = true;

            drpInquirySource.Attributes.Add("disabled", "disabled");
            drpInquiryStatus.Attributes.Add("disabled", "disabled");
            drpPriority.Attributes.Add("disabled", "disabled");
            drpClosureReason.Attributes.Add("disabled", "disabled");

            pnlDetail.Enabled = false;

            btnSave.Visible = false;
            btnSaveEmail.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            // ---------------- Report To List -------------------------------------
            List<Entity.Customer> lstCustomer = new List<Entity.Customer>();
            lstCustomer = BAL.CustomerMgmt.GetCustomerList();

            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("Inquiry");
            drpInquiryStatus.DataSource = lstDesig;
            drpInquiryStatus.DataValueField = "pkID";
            drpInquiryStatus.DataTextField = "InquiryStatusName";
            drpInquiryStatus.DataBind();
            //drpInquiryStatus.Items.Insert(0, new ListItem("-- Select Status --", ""));

            // ---------------- Designation List  -------------------------------------
            List<Entity.InquiryStatus> lstSource = new List<Entity.InquiryStatus>();
            lstSource = BAL.InquiryStatusMgmt.GetInquiryStatusList("InquirySource");
            drpInquirySource.DataSource = lstSource;
            drpInquirySource.DataValueField = "InquiryStatusName";
            drpInquirySource.DataTextField = "InquiryStatusName";
            drpInquirySource.DataBind();
            drpInquirySource.Items.Insert(0, new ListItem("-- Select --", ""));

            // ---------------- Closure Reason List  -------------------------------------
            List<Entity.InquiryStatus> lstClosure = new List<Entity.InquiryStatus>();
            lstClosure = BAL.InquiryStatusMgmt.GetInquiryStatusList("DisQualifiedReason");
            drpClosureReason.DataSource = lstClosure;
            drpClosureReason.DataValueField = "pkID";
            drpClosureReason.DataTextField = "InquiryStatusName";
            drpClosureReason.DataBind();
            drpClosureReason.Items.Insert(0, new ListItem("-- Select Reason --", ""));

            // ---------------- Lead Prioriy  -------------------------------------
            List<Entity.InquiryStatus> lstPriority = new List<Entity.InquiryStatus>();
            lstPriority = BAL.InquiryStatusMgmt.GetInquiryStatusList("LeadPriority");
            drpPriority.DataSource = lstPriority;
            drpPriority.DataValueField = "InquiryStatusName";
            drpPriority.DataTextField = "InquiryStatusName";
            drpPriority.DataBind();
            drpPriority.Items.Insert(0, new ListItem("-- Select Priority --", "0"));
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        protected void custSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "";

            Entity.Customer objEntity = new Entity.Customer();

            objEntity.CustomerID = Convert.ToInt64(0);
            // -------------------------------------------------------------- Insert/Update Record
            BAL.CustomerMgmt.AddUpdateCustomerInstant(objEntity, out ReturnCode, out ReturnMsg);
            // --------------------------------------------------------------
            BindDropDown();
            // -------------------------------------------------------------------------
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:openFormContainer();", true);
        }

        public void BindInquiryProduct(string pInquiryNo)
        {
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.InquiryInfoMgmt.GetInquiryProductDetail(pInquiryNo);
            rptInquiryProductGroup.DataSource = dtDetail1;
            rptInquiryProductGroup.DataBind();
            Session.Add("dtDetailInq", dtDetail1);
        }

        protected void rptInquiryProductGroup_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var requestTarget = this.Request["__EVENTTARGET"];
            string strErr = "";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (e.CommandName.ToString() == "Save")
                {
                    _pageValid = true;

                    HiddenField hdnProductID = (HiddenField)e.Item.FindControl("hdnProductID");
                    TextBox txtProductName = (TextBox)e.Item.FindControl("txtProductName");
                    TextBox txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                    TextBox txtUnitPrice = (TextBox)e.Item.FindControl("txtUnitPrice");
                    DropDownList drpUnit = (DropDownList)e.Item.FindControl("drpUnit");
                    DropDownList drpThickness = (DropDownList)e.Item.FindControl("drpThickness");
                    TextBox txtFactor = (TextBox)e.Item.FindControl("txtFactor");
                    TextBox txtArea = (TextBox)e.Item.FindControl("txtArea");
                    TextBox txtRemarks = (TextBox)e.Item.FindControl("txtRemarks");

                    if (String.IsNullOrEmpty(hdnProductID.Value) || 
                        String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0 ||
                        String.IsNullOrEmpty(txtFactor.Text) || Convert.ToDecimal(txtFactor.Text) < 0 || 
                        String.IsNullOrEmpty(txtArea.Text) || Convert.ToDecimal(txtArea.Text) < 0)
                    {
                        _pageValid = false;

                        if (String.IsNullOrEmpty(hdnProductID.Value))
                            strErr += "<li>" + "Product Selection is required." + "</li>";

                        if (String.IsNullOrEmpty(txtQuantity.Text) || Convert.ToDecimal(txtQuantity.Text) < 0)
                            strErr += "<li>" + "Quantity is required." + "</li>";

                        if (String.IsNullOrEmpty(txtFactor.Text) || Convert.ToDecimal(txtFactor.Text) <= 0 || String.IsNullOrEmpty(txtArea.Text) || Convert.ToDecimal(txtArea.Text) <= 0 )
                            strErr += "<li>" + "Factor and Area is compulsary to calculate Area And Must not be Zero !" + "</li>";

                        //if (String.IsNullOrEmpty(txtArea.Text) || Convert.ToDecimal(txtArea.Text) < 0)
                        //    strErr += "<li>" + "Factor is required." + "</li>";


                    }
                    // -------------------------------------------------------------
                    if (_pageValid)
                    {
                        DataTable dtDetail = new DataTable();
                        dtDetail = (DataTable)Session["dtDetailInq"];
                        //----Check For Duplicate Item----//
                        string find = "ProductID = " + hdnProductID.Value + "";
                        DataRow[] foundRows = dtDetail.Select(find);
                        if (foundRows.Length > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Duplicate Item Not Allowed..!!')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearProductField", "clearProductField();", true);
                            return;
                        }


                        Int64 cntRow = dtDetail.Rows.Count + 1;

                        DataRow dr = dtDetail.NewRow();

                        dr["pkID"] = cntRow;
                        string icode = hdnProductID.Value;
                        string iname = txtProductName.Text;
                        string unitprice = ((TextBox)e.Item.FindControl("txtUnitPrice")).Text;
                        string unit = ((DropDownList)e.Item.FindControl("drpUnit")).Text;
                        string thickness = ((DropDownList)e.Item.FindControl("drpThickness")).Text;
                        string factor = ((TextBox)e.Item.FindControl("txtFactor")).Text;
                        string area = ((TextBox)e.Item.FindControl("txtArea")).Text;
                        string qty = ((TextBox)e.Item.FindControl("txtQuantity")).Text;
                        string remarks = ((TextBox)e.Item.FindControl("txtRemarks")).Text;

                        dr["InquiryNo"] = txtInquiryNo.Text;
                        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
                        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
                        dr["UnitPrice"] = (!String.IsNullOrEmpty(unitprice)) ? Convert.ToDecimal(unitprice) : 0;
                        dr["Unit"] = (!String.IsNullOrEmpty(unit)) ? Convert.ToString(unit) : "";
                        dr["Thickness"] = (!String.IsNullOrEmpty(thickness)) ? Convert.ToString(thickness) : "";
                        dr["Factor"] = (!String.IsNullOrEmpty(factor)) ? Convert.ToDecimal(factor) : 0;
                        dr["Area"] = (!String.IsNullOrEmpty(area)) ? Convert.ToDecimal(area) : 0;
                        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
                        dr["Remarks"] = (!String.IsNullOrEmpty(remarks)) ? Convert.ToString(remarks) : "";

                        dtDetail.Rows.Add(dr);
                        Session.Add("dtDetailInq", dtDetail);
                        // ---------------------------------------------------------------
                        rptInquiryProductGroup.DataSource = dtDetail;
                        rptInquiryProductGroup.DataBind();
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
                    dtDetail = (DataTable)Session["dtDetailInq"];

                    DataRow[] rows;
                    rows = dtDetail.Select("pkID=" + e.CommandArgument.ToString());
                    foreach (DataRow r in rows)
                        r.Delete();

                    rptInquiryProductGroup.DataSource = dtDetail;
                    rptInquiryProductGroup.DataBind();

                    Session.Add("dtDetailInq", dtDetail);
                }
            }
        }

        protected void rptInquiryProductGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                decimal v1, v2;
                v1 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "Quantity"));
                v2 = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "UnitPrice"));

                totAmount += Convert.ToDecimal(v1 * v2);
            }
            if (e.Item.ItemType == ListItemType.Footer)
            {
                Label lblTotalAmount = (Label)e.Item.FindControl("lblTotalAmount");
                lblTotalAmount.Text = totAmount.ToString("0.00");
            }
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.InquiryInfo> lstEntity = new List<Entity.InquiryInfo>();
                // ----------------------------------------------------
                lstEntity = BAL.InquiryInfoMgmt.GetInquiryInfoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                hdnCustEmailAddress.Value = lstEntity[0].EmailAddress;
                hdnEmployeeName.Value = lstEntity[0].EmployeeName;
                hdnDesignation.Value = lstEntity[0].Designation;
                txtInquiryNo.Text = lstEntity[0].InquiryNo;
                txtInquiryDate.Text = lstEntity[0].InquiryDate.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                drpInquirySource.SelectedValue = lstEntity[0].InquirySource.ToString();
                txtReferenceName.Text = lstEntity[0].ReferenceName;
                txtRefNo.Text = lstEntity[0].RefNo;
                txtMeetingNotes.Text = lstEntity[0].MeetingNotes;
                txtFollowupNotes.Text = lstEntity[0].FollowupNotes;
                txtFollowupDate.Text = (lstEntity[0].FollowupDate != SqlDateTime.MinValue.Value) ? lstEntity[0].FollowupDate.ToString("yyyy-MM-dd") : "";
                txtPreferredTime.Text = lstEntity[0].PreferredTime.ToString();
                drpInquiryStatus.SelectedValue = lstEntity[0].InquiryStatusID.ToString();
                drpPriority.SelectedValue = lstEntity[0].Priority.ToString();
                if (drpInquiryStatus.SelectedItem.Text.ToLower() == "close - lost")
                {
                    drpClosureReason.SelectedValue = lstEntity[0].ClosureReason.ToString();
                    drpInquiryStatus_SelectedIndexChanged(null, null);
                }
                // -------------------------------------------------------------------------
                BindInquiryProduct(txtInquiryNo.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SendAndSaveData(false);
        }

        protected void btnSaveEmail_Click(object sender, EventArgs e)
        {
            SendAndSaveData(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
            // ---------------------------------------------------
            // Resetting Temporary Table for Products
            // ---------------------------------------------------
            Session.Remove("dtDetailInq");
            DataTable dtDetail1 = new DataTable();
            dtDetail1 = BAL.InquiryInfoMgmt.GetInquiryProductDetail("-1");
            Session.Add("dtDetailInq", dtDetail1);
        }

        public void SendAndSaveData(Boolean paraSaveAndEmail)
        {
            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetailInq"];

            int ReturnCode = 0;
            string ReturnMsg = "";
            string ReturnInquiryNo = "";
            string strErr = "";
            Int64 ReturnFollowupNo = 0;

            //--------------------------------------------------------------
            _pageValid = true;

            if ((String.IsNullOrEmpty(txtInquiryDate.Text)) || (String.IsNullOrEmpty(txtCustomerName.Text)) || (String.IsNullOrEmpty(drpInquirySource.SelectedValue)) ||
                (String.IsNullOrEmpty(txtMeetingNotes.Text)) || String.IsNullOrEmpty(hdnCustomerID.Value))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtInquiryDate.Text))
                    strErr += "<li>" + "Lead Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Customer Name is required." + "</li>";

                if (String.IsNullOrEmpty(drpInquirySource.SelectedValue))
                    strErr += "<li>" + "Please Select Lead Source." + "</li>";

                if (String.IsNullOrEmpty(txtMeetingNotes.Text))
                    strErr += "<li>" + "Meeting Notes is required." + "</li>";

                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Select Proper Customer From List." + "</li>";

            }
            if (Request.QueryString["mode"].ToString() == "add")
            {
                if (!String.IsNullOrEmpty(txtInquiryDate.Text) && !String.IsNullOrEmpty(txtFollowupDate.Text))
                {
                    if (Convert.ToDateTime(txtFollowupDate.Text) < Convert.ToDateTime(txtInquiryDate.Text))
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Followup Date should be greater than Lead Date." + "</li>";
                    }
                }
                if (String.IsNullOrEmpty(txtFollowupDate.Text) && !String.IsNullOrEmpty(txtFollowupNotes.Text))
                {
                    _pageValid = false;
                    strErr += "<li>" + "Followup Date should be required" + "</li>";
                }
            }

            // --------------------------------------------------------
            //if ((hdnpkID.Value == "" || hdnpkID.Value == "0") && !String.IsNullOrEmpty(txtFollowupNotes.Text))
            //{
            //    if (!String.IsNullOrEmpty(txtInquiryDate.Text) && !String.IsNullOrEmpty(txtFollowupDate.Text))
            //    {
            //        if (Convert.ToDateTime(txtFollowupDate.Text) < Convert.ToDateTime(txtInquiryDate.Text))
            //        {
            //            strErr += "<li>" + "Followup Date should be greater than Lead Date." + "</li>";
            //            _pageValid = false;
            //        }
            //    }

            //}
            // --------------------------------------------------------
            if (!String.IsNullOrEmpty(drpInquiryStatus.SelectedValue) && Convert.ToInt64(drpInquiryStatus.SelectedValue) > 0)
            {
                if (String.IsNullOrEmpty(drpClosureReason.SelectedValue))
                {
                    if (drpInquiryStatus.SelectedItem.Text.ToLower() == "close - lost")
                    {
                        _pageValid = false;
                        strErr += "<li>" + "Closure Reaason is required." + "</li>";
                    }
                }
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.InquiryInfo objEntity = new Entity.InquiryInfo();

                if (dtDetail != null)
                {
                    if (dtDetail.Rows.Count > 0 && !String.IsNullOrEmpty(hdnCustomerID.Value) && hdnCustomerID.Value != "0")
                    {
                        if (!String.IsNullOrEmpty(hdnpkID.Value))
                            objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                        objEntity.InquiryNo = txtInquiryNo.Text;
                        objEntity.InquiryDate = Convert.ToDateTime(txtInquiryDate.Text);
                        //objEntity.CustomerID = Convert.ToInt64(drpCustomer.SelectedValue);
                        objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                        objEntity.ReferenceName = txtReferenceName.Text;
                        objEntity.RefNo = txtRefNo.Text;
                        objEntity.InquirySource = drpInquirySource.SelectedValue;
                        objEntity.MeetingNotes = txtMeetingNotes.Text;
                        objEntity.FollowupNotes = txtFollowupNotes.Text;
                        objEntity.FollowupDate = (!String.IsNullOrEmpty(txtFollowupDate.Text)) ? Convert.ToDateTime(txtFollowupDate.Text) : SqlDateTime.MinValue.Value;
                        objEntity.PreferredTime = txtPreferredTime.Text;
                        objEntity.InquiryStatusID = Convert.ToInt64(drpInquiryStatus.SelectedValue);
                        objEntity.Priority = drpPriority.SelectedValue.ToString();
                        objEntity.ClosureReason = (!String.IsNullOrEmpty(drpClosureReason.SelectedValue)) ? Convert.ToInt64(drpClosureReason.SelectedValue) : 0;
                        objEntity.LoginUserID = Session["LoginUserID"].ToString();
                        // -------------------------------------------------------------- Insert/Update Record
                        BAL.InquiryInfoMgmt.AddUpdateInquiryInfo(objEntity, out ReturnCode, out ReturnMsg, out ReturnInquiryNo, out ReturnFollowupNo);
                        strErr += "<li>" + ReturnMsg + "</li>";
                        // --------------------------------------------------------------
                        if (String.IsNullOrEmpty(ReturnInquiryNo) && !String.IsNullOrEmpty(txtInquiryNo.Text))
                        {
                            ReturnInquiryNo = txtInquiryNo.Text;
                        }
                        // --------------------------------------------------------------
                        // >>>>>>>> First Delete all Selectd ProductGroup entry from table
                        int ReturnCode1;
                        String ReturnMsg1;

                        if (ReturnCode > 0 && !String.IsNullOrEmpty(ReturnInquiryNo))
                        {

                            btnSave.Disabled = true;
                            btnSaveEmail.Disabled = true;

                            try
                            {

                                //string[] InqNo = ReturnInquiryNo.Split(',');

                                string notificationMsg = "";
                                if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
                                    notificationMsg = "Inquiry " + ReturnInquiryNo + " Updated For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                else
                                    notificationMsg = "Inquiry " + ReturnInquiryNo + " Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());

                                BAL.CommonMgmt.SendNotification_Firebase("Inquiry", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                BAL.CommonMgmt.SendNotificationToDB("Inquiry", 0, notificationMsg, Session["LoginUserID"].ToString(), 0);

                                if ((((!String.IsNullOrEmpty(txtFollowupDate.Text)) ? Convert.ToDateTime(txtFollowupDate.Text) : SqlDateTime.MinValue.Value) != SqlDateTime.MinValue.Value) && (!String.IsNullOrEmpty(txtFollowupNotes.Text)) && ReturnFollowupNo > 0)
                                {
                                    notificationMsg = "";
                                    notificationMsg = "FollowUp Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString());
                                    BAL.CommonMgmt.SendNotification_Firebase("FollowUp", notificationMsg, Session["LoginUserID"].ToString(), 0);
                                    BAL.CommonMgmt.SendNotificationToDB("FollowUp", ReturnFollowupNo, notificationMsg, Session["LoginUserID"].ToString(), 0);
                                }

                            }
                            catch (Exception)
                            { }

                            // --------------------------------------------------------------
                            BAL.InquiryInfoMgmt.DeleteInquiryProductByInquiryNo(ReturnInquiryNo, out ReturnCode1, out ReturnMsg1);

                            // >>>>>>>> Second Insert all Selectd ProductGroup entry into table
                            Entity.InquiryInfo objEntity1 = new Entity.InquiryInfo();

                            foreach (DataRow dr in dtDetail.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    objEntity1.ProductID = Convert.ToInt64(dr["ProductID"]);
                                    objEntity1.InquiryNo = ReturnInquiryNo;
                                    objEntity1.Quantity = (!String.IsNullOrEmpty(dr["Quantity"].ToString())) ? Convert.ToDecimal(dr["Quantity"].ToString()) : 0;
                                    objEntity1.UnitPrice = (!String.IsNullOrEmpty(dr["UnitPrice"].ToString())) ? Convert.ToDecimal(dr["UnitPrice"].ToString()) : 0;
                                    objEntity1.Unit = (!String.IsNullOrEmpty(dr["Unit"].ToString())) ? Convert.ToString(dr["Unit"].ToString()) : "";
                                    objEntity1.Thickness = (!String.IsNullOrEmpty(dr["Thickness"].ToString())) ? Convert.ToString(dr["Thickness"].ToString()) : "";
                                    objEntity1.Factor = (!String.IsNullOrEmpty(dr["Factor"].ToString())) ? Convert.ToDecimal(dr["Factor"].ToString()) : 0;
                                    objEntity1.Area = (!String.IsNullOrEmpty(dr["Area"].ToString())) ? Convert.ToDecimal(dr["Area"].ToString()) : 0;
                                    objEntity1.Remarks = (!String.IsNullOrEmpty(dr["Remarks"].ToString())) ? Convert.ToString(dr["Remarks"].ToString()) : "";
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.InquiryInfoMgmt.AddUpdateInquiryProduct(objEntity1, out ReturnCode, out ReturnMsg);
                                }
                            }
                            if (ReturnCode > 0)
                            {
                                Session.Remove("dtDetailInq");

                            }
                        }
                        // --------------------------------------------------------------
                        if (paraSaveAndEmail)
                        {
                            Entity.Authenticate objAuth = new Entity.Authenticate();
                            objAuth = (Entity.Authenticate)HttpContext.Current.Session["logindetail"];

                            String sendEmailFlag = BAL.CommonMgmt.GetConstant("INQ-EMAIL", 0, objAuth.CompanyID).ToLower();
                            if (ReturnCode > 0 && (sendEmailFlag == "yes" || sendEmailFlag == "y" || sendEmailFlag == "true"))
                            {
                                try
                                {
                                    if (String.IsNullOrEmpty(hdnCustEmailAddress.Value) && objEntity.CustomerID > 0)
                                    {
                                        hdnCustEmailAddress.Value = BAL.CommonMgmt.GetCustomerEmailAddress(objEntity.CustomerID);
                                    }
                                    // -------------------------------------------------------
                                    if (!String.IsNullOrEmpty(hdnCustEmailAddress.Value) && hdnCustEmailAddress.Value.ToUpper() != "NULL")
                                    {
                                        String respVal = "";
                                        respVal = BAL.CommonMgmt.SendEmailNotifcation("INQUIRY-WELCOME", Session["LoginUserID"].ToString(), ((!String.IsNullOrEmpty(hdnpkID.Value)) ? Convert.ToInt64(hdnpkID.Value) : 0), hdnCustEmailAddress.Value);
                                    }
                                    strErr += "<li>" + "Email Notification Sent Successfully !" + "</li>";
                                }
                                catch (Exception ex)
                                {
                                    strErr += "<li>" + "Email Notification Failed !" + "</li>";
                                }
                            }
                        }
                    }
                    else
                    {
                        strErr += "<li>" + "Minimum One Product required !" + "</li>";
                    }
                }
                else
                {
                    strErr += "<li>" + "Minimum One Product required !" + "</li>";
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

        protected void CalQuantity(object sender, EventArgs e)
        {
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
            TextBox txtFactor = (TextBox)item.FindControl("txtFactor");
            TextBox txtArea = (TextBox)item.FindControl("txtArea");


            Decimal factor = (!String.IsNullOrEmpty(txtFactor.Text)) ? Convert.ToDecimal(txtFactor.Text) : 1;
            Decimal area = (!String.IsNullOrEmpty(txtArea.Text)) ? Convert.ToDecimal(txtArea.Text) : 0;

            Decimal quantity = factor * area;

            txtQuantity.Text = quantity.ToString();

        }
        protected void editItem_TextChanged(object sender, EventArgs e)
        {
            TextBox edSender = (TextBox)sender;
            var item = (RepeaterItem)edSender.NamingContainer;

            HiddenField edProductID = (HiddenField)item.FindControl("hdnProductID_Grid");
            TextBox edUnitRate = (TextBox)item.FindControl("tdUnitPrice");
            TextBox edQuantity = (TextBox)item.FindControl("tdQuantity");
            //TextBox edFactor = (TextBox)item.FindControl("tdFactor");
            //TextBox edArea = (TextBox)item.FindControl("tdArea");
            TextBox edRemarks = (TextBox)item.FindControl("tdRemarks");

            Decimal ur = (!String.IsNullOrEmpty(edUnitRate.Text)) ? Convert.ToDecimal(edUnitRate.Text) : 0;
            Decimal q = (!String.IsNullOrEmpty(edQuantity.Text)) ? Convert.ToDecimal(edQuantity.Text) : 0;
            //Decimal factor = (!String.IsNullOrEmpty(edFactor.Text)) ? Convert.ToDecimal(edFactor.Text) : 1;
            //Decimal area = (!String.IsNullOrEmpty(edArea.Text)) ? Convert.ToDecimal(edArea.Text) : 0;
            //Decimal quantity = 0;

            //quantity = factor * area;

            //edQuantity.Text = quantity.ToString();

            DataTable dtDetail = new DataTable();
            dtDetail = (DataTable)Session["dtDetailInq"];

            foreach (System.Data.DataColumn col in dtDetail.Columns) col.ReadOnly = false;

            foreach (DataRow row in dtDetail.Rows)
            {
                if (row["pkID"].ToString() == edProductID.Value)
                {
                    row.SetField("UnitPrice", edUnitRate.Text);
                    row.SetField("Quantity", edQuantity.Text);
                    //row.SetField("Factor", edFactor.Text);
                    //row.SetField("Area", edArea.Text);
                    row.SetField("Remarks", edRemarks.Text);
                    row.AcceptChanges();
                }
                dtDetail.AcceptChanges();
                rptInquiryProductGroup.DataSource = dtDetail;
                rptInquiryProductGroup.DataBind();
                Session.Add("dtDetailInq", dtDetail);
            }
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnCustomerID.Value = "";
            hdnOrgCodeEmp.Value = "";
            txtInquiryDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtInquiryNo.Text = ""; //  BAL.CommonMgmt.GetInquiryNo(txtInquiryDate.Text);
            txtCustomerName.Text = "";
            txtReferenceName.Text = "";
            txtMeetingNotes.Text = "";
            txtFollowupNotes.Text = "";
            if(HttpContext.Current.Session["SerialKey"].ToString() != "JAYJ-ALAR-AMBR-ICKS")
                txtFollowupDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            drpInquiryStatus.ClearSelection();
            drpInquiryStatus.Items.FindByText("Open").Selected = true;
            //drpCustomer.SelectedValue = "";
            drpInquirySource.SelectedValue = "";
            drpPriority.SelectedValue = "0";
            drpClosureReason.SelectedValue = "";
            // ---------------------------------------------
            BindInquiryProduct("");
            txtInquiryDate.Focus();
            txtPreferredTime.Text = "";

            btnSave.Disabled = false;
            btnSaveEmail.Disabled = false;
        }

        public void ClearProductFields()
        {
            //drpProduct.SelectedValue = "";
            //txtProductName.Text = "";
            //txtQuantity.Text = "";
            //txtUnitPrice.Text = "";
            //lblTaxRate.Text = "";

            //drpProduct.Focus();
            //txtProductName.Focus();
        }

        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            int totalrecord;

            //Control rptFootCtrl = rptInquiryProductGroup.Controls[rptInquiryProductGroup.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            //Label lblUnitPrice = ((Label)rptFootCtrl.FindControl("lblUnitPrice"));
            //Label lblTaxRate = ((Label)rptFootCtrl.FindControl("lblTaxRate"));
            //TextBox txtQty = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));

            List<Entity.Products> lstEntity = new List<Entity.Products>();

            //if (!String.IsNullOrEmpty(ctrl1))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //lblUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
            // -----------------------------------------
            //if (!String.IsNullOrEmpty(drpProduct.SelectedValue))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(drpProduct.SelectedValue), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

            //if (!String.IsNullOrEmpty(hdnProductID.Value))
            //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);
            //txtUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
            //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";            

            //txtQuantity.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteInquiry(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- 
            BAL.InquiryInfoMgmt.DeleteInquiryInfo(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string GetInquiryNoPrimaryID(string pInqNo)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CommonMgmt.GetInquiryNoPrimaryID(pInqNo);
            return serializer.Serialize(rows);
        }
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        // Filter Customer, Fixed Ledger & Products 
        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
        [System.Web.Services.WebMethod]
        public static string FilterFixedLedger()
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetFixedLedgerForDropdown();
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterFixedLedgerModule(string pModule)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetFixedLedgerForDropdown(pModule);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterCustomer(string pCustName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetCustomerListForDropdown(pCustName);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterCustomerByModule(string pCustName, string pSearchModule)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.CustomerMgmt.GetCustomerListForDropdown(pCustName, pSearchModule);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProduct(string pProductName)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(pProductName);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProduct(string pProductName, string pSearchModule)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(pProductName, pSearchModule);
            return serializer.Serialize(rows);
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            int totalrecord;

            Control rptFootCtrl = rptInquiryProductGroup.Controls[rptInquiryProductGroup.Controls.Count - 1].Controls[0];
            //string ctrl1 = ((DropDownList)rptFootCtrl.FindControl("drpProduct")).Text;
            HiddenField hdnProductID = ((HiddenField)rptFootCtrl.FindControl("hdnProductID"));
            if (!String.IsNullOrEmpty(hdnProductID.Value))
            {
                TextBox txtProductName = ((TextBox)rptFootCtrl.FindControl("txtProductName"));
                TextBox txtQuantity = ((TextBox)rptFootCtrl.FindControl("txtQuantity"));
                TextBox txtUnitPrice = ((TextBox)rptFootCtrl.FindControl("txtUnitPrice"));
                //Label lblTaxRate = ((Label)rptFootCtrl.FindControl("lblTaxRate"));

                List<Entity.Products> lstEntity = new List<Entity.Products>();

                //if (!String.IsNullOrEmpty(ctrl1))
                //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(ctrl1), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                //lblUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
                //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";
                // -----------------------------------------
                //if (!String.IsNullOrEmpty(drpProduct.SelectedValue))
                //    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(drpProduct.SelectedValue), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                if (!String.IsNullOrEmpty(hdnProductID.Value))
                    lstEntity = BAL.ProductMgmt.GetProductList(Convert.ToInt64(hdnProductID.Value), null, Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out totalrecord);

                txtUnitPrice.Text = (lstEntity.Count > 0) ? lstEntity[0].UnitPrice.ToString() : "0";
                //lblTaxRate.Text = (lstEntity.Count > 0) ? lstEntity[0].TaxRate.ToString() : "0";          

                txtUnitPrice.Focus();
            }
            else
            {
                strErr += "<li> Select Proper Item From List !</li>";
            }
            if (!String.IsNullOrEmpty(strErr))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('" + strErr + "','toast-danger');", true);
            }
        }

        protected void drpInquiryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            divClosureReason.Visible = (drpInquiryStatus.SelectedItem.Text.ToLower() == "close - lost") ? true : false;
            //drpAssignTo.Visible = (drpInquiryStatus.SelectedItem.Text.ToLower() == "assign to partner") ? true : false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:checkedEventCall();", true);
            divAssignTo.Visible = (drpInquiryStatus.SelectedItem.Text.ToLower() == "close - assign to partner") ? true : false;
        }


        //protected void imgBtnSave_Click(object sender, ImageClickEventArgs e)
        //{
        //    _pageValid = true;
        //    divErrorMessage.InnerHtml = "";

        //    if (String.IsNullOrEmpty(hdnProductID.Value) || String.IsNullOrEmpty(txtQuantity.Text) || (txtQuantity.Text == "0"))
        //    {
        //        _pageValid = false;

        //        divErrorMessage.Style.Remove("color");
        //        divErrorMessage.Style.Add("color", "red");

        //        if (String.IsNullOrEmpty(hdnProductID.Value))
        //            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Product Selection is required." + "</li>"));

        //        if (String.IsNullOrEmpty(txtQuantity.Text) || (txtQuantity.Text == "0"))
        //            divErrorMessage.Controls.Add(new LiteralControl("<li>" + "Quantity is required." + "</li>"));

        //    }
        //    // -------------------------------------------------------------
        //    if (_pageValid)
        //    {
        //        DataTable dtDetail = new DataTable();
        //        dtDetail = (DataTable)Session["dtDetail"];

        //        Int64 cntRow = dtDetail.Rows.Count + 1;

        //        DataRow dr = dtDetail.NewRow();

        //        dr["pkID"] = cntRow;
        //        //string icode = drpProduct.SelectedValue;
        //        //string iname = drpProduct.SelectedItem.Text;
        //        string icode = hdnProductID.Value;
        //        string iname = txtProductName.Text;
        //        string qty = txtQuantity.Text;
        //        string unitprice = txtUnitPrice.Text;
        //        string taxrate = lblTaxRate.Text;

        //        dr["InquiryNo"] = txtInquiryNo.Text;
        //        dr["ProductID"] = (!String.IsNullOrEmpty(icode)) ? Convert.ToInt64(icode) : 0;
        //        dr["ProductName"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
        //        dr["ProductNameLong"] = (!String.IsNullOrEmpty(iname)) ? iname : "";
        //        dr["UnitPrice"] = (!String.IsNullOrEmpty(unitprice)) ? Convert.ToDecimal(unitprice) : 0;
        //        dr["TaxRate"] = (!String.IsNullOrEmpty(taxrate)) ? Convert.ToDecimal(taxrate) : 0;
        //        dr["Quantity"] = (!String.IsNullOrEmpty(qty)) ? Convert.ToDecimal(qty) : 0;
        //        dtDetail.Rows.Add(dr);

        //        Session.Add("dtDetail", dtDetail);
        //        // ---------------------------------------------------------------
        //        rptInquiryProductGroup.DataSource = dtDetail;
        //        rptInquiryProductGroup.DataBind();
        //    }
        //    // ------------------------------------
        //    ClearProductFields();
        //}

    }
}