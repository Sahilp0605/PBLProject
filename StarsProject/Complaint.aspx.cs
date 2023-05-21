using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;

namespace StarsProject
{
    public partial class Complaint : System.Web.UI.Page
    {

        bool _pageValid = true;
        string _pageErrMsg;

        private static DataTable dtDetail;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session.Remove("dtModuleDoc");

                Session["PageNo"] = 1;
                Session["PageSize"] = 10;
                BindDropDown();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();
                    //lblComplaintNo.Text = hdnpkID.Value;
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
                myModuleAttachment.ModuleName = "complaint";
                myModuleAttachment.KeyValue = lblComplaintNo.Text;
                myModuleAttachment.ManageLibraryDocs();
            }
        }

        public void OnlyViewControls()
        {
            txtComplaintDate.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtReferenceNo.ReadOnly = true;
            drpStatus.Attributes.Add("disabled", "disabled");
            drpType.Attributes.Add("disabled", "disabled");
            txtComplaintNotes.ReadOnly = true;
            drpEmployee.Attributes.Add("disabled", "disabled");
            //txtTimeFrom.ReadOnly = true;
            //txtTimeTo.ReadOnly = true;
            txtPreferredDate.ReadOnly = true;

            txtTimeTo.Enabled  = false ;
            txtTimeFrom.Enabled = false;

            txtProductName.ReadOnly = true;
            txtSrNo.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindDropDown()
        {
            // ---------------- Designation List  -------------------------------------
            List<Entity.OrganizationEmployee> lstEmployee = new List<Entity.OrganizationEmployee>();
            lstEmployee = BAL.OrganizationEmployeeMgmt.GetOrganizationEmployeeList();
            drpEmployee.DataSource = lstEmployee;
            drpEmployee.DataValueField = "pkID";
            drpEmployee.DataTextField = "EmployeeName";
            drpEmployee.DataBind();
            drpEmployee.Items.Insert(0, new ListItem("-- Assigned To --", "0"));

            List<Entity.InquiryStatus> lstDesig = new List<Entity.InquiryStatus>();
            lstDesig = BAL.InquiryStatusMgmt.GetInquiryStatusList("ComplaintStatus");
            drpStatus.DataSource = lstDesig;
            drpStatus.DataValueField = "InquiryStatusName";
            drpStatus.DataTextField = "InquiryStatusName";
            drpStatus.DataBind();
            drpStatus.Items.Insert(0, new ListItem("--Select Status --", ""));
        }

        public List<Entity.Products> BindProductList()
        {
            // ---------------- Product List -------------------------------------
            List<Entity.Products> lstProduct = new List<Entity.Products>();
            lstProduct = BAL.ProductMgmt.GetProductList();
            return lstProduct;
        }

        public void setLayout(string pMode)
        {
            if (pMode == "Edit")
            {
                int TotalCount = 0;
                // -----------------------------------------------------------------------------------
                List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
                // -----------------------------------------------------------------------------------
                lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnpkID.Value), 0, "", Session["LoginUserID"].ToString());
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                lblComplaintNo.Text = lstEntity[0].ComplaintNo;
                txtComplaintDate.Text = lstEntity[0].ComplaintDate.ToString("yyyy-MM-dd");
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                txtReferenceNo.Text = lstEntity[0].ReferenceNo.ToString();
                txtComplaintNotes.Text = lstEntity[0].ComplaintNotes.ToString();
                drpStatus.SelectedValue = lstEntity[0].ComplaintStatus.ToString();
                drpType.SelectedValue = lstEntity[0].ComplaintType.ToString();
                drpEmployee.SelectedValue = lstEntity[0].EmployeeID.ToString();
                if (!String.IsNullOrEmpty(lstEntity[0].PreferredDate.ToString()) && lstEntity[0].PreferredDate.Value.Year > 1900)
                    txtPreferredDate.Text = lstEntity[0].PreferredDate.Value.ToString("yyyy-MM-dd");
                else
                    txtPreferredDate.Text = null;

                txtTimeFrom.Text = lstEntity[0].TimeFrom.ToString();
                txtTimeTo.Text = lstEntity[0].TimeTo.ToString();

                hdnProductID.Value = lstEntity[0].ProductID.ToString();
                txtProductName.Text = lstEntity[0].ProductName.ToString();
                txtSrNo.Text = lstEntity[0].SrNo.ToString();
                // ------------------------------------------------------------
                myModuleAttachment.ModuleName = "complaint";
                myModuleAttachment.KeyValue = lblComplaintNo.Text;
                myModuleAttachment.BindModuleDocuments();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0;
            string ReturnMsg = "", ReturnComplaintNo = "";
            string strErr = "";

            _pageValid = true;

            if ((String.IsNullOrEmpty(txtComplaintDate.Text) ||String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0") || 
                String.IsNullOrEmpty(txtComplaintDate.Text) || String.IsNullOrEmpty(txtPreferredDate.Text) || 
                String.IsNullOrEmpty(txtComplaintNotes.Text) || drpEmployee.SelectedValue == "0")
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtComplaintDate.Text))
                    strErr += "<li>" + "Complaint Date is required." + "</li>";

                if (String.IsNullOrEmpty(hdnCustomerID.Value))
                    strErr += "<li>" + "Select Proper Customer from List" + "</li>";

                if (String.IsNullOrEmpty(txtComplaintNotes.Text))
                    strErr += "<li>" + "Complaint Notes is required." + "</li>";

                if (String.IsNullOrEmpty(txtPreferredDate.Text))
                    strErr += "<li>" + "Schedule Date is required." + "</li>";

                if ((String.IsNullOrEmpty(txtTimeFrom.Text) || String.IsNullOrEmpty(txtTimeTo.Text)))
                    strErr += "<li>" + "Preferred Time is required." + "</li>";

                if (drpEmployee.SelectedValue == "0")
                    strErr += "<li>" + "Assigned To Name is required." + "</li>";
            }
            if (!String.IsNullOrEmpty(txtComplaintDate.Text) && !String.IsNullOrEmpty(txtPreferredDate.Text))
            {
                DateTime dt2 = DateTime.Now;
                if (Convert.ToDateTime(txtComplaintDate.Text) > dt2)
                {
                    strErr += "<li>" + "Future Complaint Date not allowed." + "</li>";
                    _pageValid = false;
                }


                if (Convert.ToDateTime(txtComplaintDate.Text) > Convert.ToDateTime(txtPreferredDate.Text))
                {
                    strErr += "<li>" + "Complaint Date should be less than Schedule Date." + "</li>";
                    _pageValid = false;
                }
                    
            }
            // ------------------------------------------------------------------------
            // Section : Future Date Validation
            // ------------------------------------------------------------------------
            if (!String.IsNullOrEmpty(txtComplaintDate.Text))
            {
                DateTime dt1 = Convert.ToDateTime(txtComplaintDate.Text);
                DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                if (dt1 > dt2)
                {
                    _pageValid = false;
                    strErr += "<li>" + "Complaint Date is Not Valid." + "</li>";
                }
            }

            // --------------------------------------------------------------
            if (_pageValid)
            {
                Entity.Complaint objEntity = new Entity.Complaint();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);
                objEntity.ComplaintNo = lblComplaintNo.Text;
                objEntity.ComplaintDate = Convert.ToDateTime(txtComplaintDate.Text);
                objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.ReferenceNo = txtReferenceNo.Text;
                objEntity.ComplaintNotes = txtComplaintNotes.Text;
                objEntity.ComplaintStatus = drpStatus.SelectedValue;
                objEntity.ComplaintType = drpType.SelectedValue;
                if (drpEmployee.SelectedValue != "0")
                    objEntity.EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);
                objEntity.PreferredDate = String.IsNullOrWhiteSpace(txtPreferredDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtPreferredDate.Text);
                objEntity.TimeFrom = txtTimeFrom.Text;
                objEntity.TimeTo = txtTimeTo.Text;
                if (hdnProductID.Value != "0" && hdnProductID.Value != "")
                    objEntity.ProductID = Convert.ToInt64(hdnProductID.Value);
                objEntity.SrNo = txtSrNo.Text;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ComplaintMgmt.AddUpdateComplaint(objEntity, out ReturnCode, out ReturnMsg, out ReturnComplaintNo);
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {

                    string[] RetComplaintDetail = ReturnComplaintNo.Split(',');
                    ReturnComplaintNo = RetComplaintDetail[0];
                    Int64 ReturntPKID = Convert.ToInt64(RetComplaintDetail[1]);

                    lblComplaintNo.Text = ReturnComplaintNo;
                    btnSave.Disabled = true;
                    // ------------------------------------------------------------
                    myModuleAttachment.KeyValue = lblComplaintNo.Text;
                    myModuleAttachment.SaveModuleDocs();
                    
                    try
                    {
                        Int64 EmployeeID = 0;
                        if (drpEmployee.SelectedValue != "0")
                            EmployeeID = Convert.ToInt64(drpEmployee.SelectedValue);

                        string notificationMsg = "";
                        if (!String.IsNullOrEmpty(hdnpkID.Value) && Convert.ToInt64(hdnpkID.Value) > 0)
                            notificationMsg = "Complaint Updated For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString()) + " And Assign To " + BAL.CommonMgmt.GetEmployeeNameByEmployeeID(EmployeeID);
                        else
                            notificationMsg = "Complaint Created For " + txtCustomerName.Text + " By " + BAL.CommonMgmt.GetEmployeeNameByUserID(Session["LoginUserID"].ToString()) + " And Assign To " + BAL.CommonMgmt.GetEmployeeNameByEmployeeID(EmployeeID);

                        BAL.CommonMgmt.SendNotification_Firebase("Complaint", notificationMsg, Session["LoginUserID"].ToString(), EmployeeID);
                        BAL.CommonMgmt.SendNotificationToDB("Complaint", ReturntPKID, notificationMsg, Session["LoginUserID"].ToString(), EmployeeID);
                    }
                    catch (Exception)
                    {}
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
            lblComplaintNo.Text = "";
            txtComplaintDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtReferenceNo.Text = "";
            txtComplaintNotes.Text = "";
            drpEmployee.SelectedValue = "0";
            drpStatus.SelectedValue = "0";
            drpType.SelectedValue = "0";
            txtPreferredDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtTimeFrom.Text = String.Empty;
            txtTimeTo.Text = String.Empty;

            hdnProductID.Value = "";
            txtProductName.Text = "";
            txtSrNo.Text = "";

            txtCustomerName.Focus();
            btnSave.Disabled = false;
            // ------------------------------------------------------------
            myModuleAttachment.ModuleName = "complaint";
            myModuleAttachment.KeyValue = lblComplaintNo.Text;
            myModuleAttachment.BindModuleDocuments();
        }

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            string strErr = "";
            if(hdnCustomerID.Value == "0")
                strErr += "<li>" + "Select Proper Customer From List" + "</li>";
            txtReferenceNo.Focus();
        }

        [System.Web.Services.WebMethod]
        public static string DeleteComplaint(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // -----------------------------------------------------------------------------------
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(pkID), 0, "", HttpContext.Current.Session["LoginUserID"].ToString());
            if (lstEntity.Count > 0)
            {
                myModuleAttachment mya = new myModuleAttachment();
                mya.DeleteModuleEntry("complaint", lstEntity[0].ComplaintNo.ToString(), HttpContext.Current.Server.MapPath("ModuleDocs"));
            }
                
            // --------------------------------- Delete Record
            BAL.ComplaintMgmt.DeleteComplaint(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void txtProductName_TextChanged(object sender, EventArgs e)
        {

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
            String SerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(pProductName, pSearchModule);
            return serializer.Serialize(rows);
        }

        [System.Web.Services.WebMethod]
        public static string FilterProductCust(string pProductName, string pSearchModule, Int64 CustomerID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            String SerialKey = HttpContext.Current.Session["SerialKey"].ToString();
            // --------------------------------- 
            var rows = BAL.ProductMgmt.GetProductListForDropdown(SerialKey, pProductName, pSearchModule, CustomerID);
            return serializer.Serialize(rows);
        }
    }
}