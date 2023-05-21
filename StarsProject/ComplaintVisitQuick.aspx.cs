using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Web.Services;
using System.IO;

namespace StarsProject
{
    public partial class ComplaintVisitQuick : System.Web.UI.Page
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
                //BindComplaintByCustomer();
                // --------------------------------------------------------
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        ClearAllField();
                        BindComplaintByCustomer();
                        // --------------------------------------------------------------
                        if (!String.IsNullOrEmpty(Request.QueryString["complaintno"]))
                        {
                            hdnComplaintNo.Value = Request.QueryString["complaintno"].ToString();
                            List<Entity.Complaint> lstComplaint = new List<Entity.Complaint>();
                            lstComplaint = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(hdnComplaintNo.Value), 0, "", Session["LoginUserID"].ToString());
                            //if (lstComplaint.Count > 0)
                            //{
                            //    hdnCustomerID.Value = lstComplaint[0].CustomerID.ToString();
                            //    txtCustomerName.Text = lstComplaint[0].CustomerName.ToString();
                            //    // -------------------------------------------
                            //    drpComplaintNo.Items.Clear();
                            //    drpComplaintNo.DataSource = lstComplaint;
                            //    drpComplaintNo.DataValueField = "pkID";
                            //    drpComplaintNo.DataTextField = "pkID";
                            //    drpComplaintNo.DataBind();
                            //    drpComplaintNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "0"));

                            //}
                        }
                    }
                    else
                    {
                        BindComplaintByCustomer();
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
                // Visitor Document Upload On .... Page Postback
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
                        if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                        {
                            string rootFolderPathDocument = Server.MapPath("visitdocuments");
                            string filesToDeleteDocument = @"visit-document-" + hdnpkID.Value.Trim() + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                            string[] fileListDocument = System.IO.Directory.GetFiles(rootFolderPathDocument, filesToDeleteDocument);
                            foreach (string filedocument in fileListDocument)
                            {
                                System.IO.File.Delete(filedocument);
                            }
                            // -----------------------------------------------------
                            String flnamedocument = "visit-document-" + hdnpkID.Value.Trim() + ext;
                            uploadDocument.SaveAs(Server.MapPath("visitdocuments/") + flnamedocument);
                            imgDocument.ImageUrl = "";
                            imgDocument.ImageUrl = "visitdocuments/" + flnamedocument;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, typeof(string), "popFileExt", "javascript:showFileExtError('image');", true);
                    }
                }

                var requestTarget = this.Request["__EVENTTARGET"];

                if (requestTarget.ToLower() == "txtcustomername")
                {
                    if (!String.IsNullOrEmpty(hdnCustomerID.Value))
                        BindComplaintByCustomer();
                }

            }
        }
        public void OnlyViewControls()
        {
            txtCustomerName.ReadOnly = true;
            drpComplaintNo.Attributes.Add("disabled", "disabled");
            drpStatus.Attributes.Add("disabled", "disabled");
            txtVisitDate.ReadOnly = true;
            txtTimeFrom.ReadOnly = true;
            txtTimeTo.ReadOnly = true;
            drpVisitType.Attributes.Add("disabled", "disabled");
            txtVisitNotes.ReadOnly = true;
            drpVisitChargeType.Attributes.Add("disabled", "disabled");
            txtVisitCharge.ReadOnly = true;

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void BindComplaintByCustomer()
        {
            int totrec = 0;
            // ---------------- Designation List  -------------------------------------
            List<Entity.Complaint> lstEmployee = new List<Entity.Complaint>();
            //lstEmployee = BAL.ComplaintMgmt.GetComplaintList(0, Convert.ToInt64(hdnCustomerID.Value), "", 0, 0, Session["LoginUserID"].ToString(), "", 1, 10000, out totrec);
            lstEmployee = BAL.ComplaintMgmt.GetComplaintList(0, 0, "", 0, 0, Session["LoginUserID"].ToString(), "", 1, 10000, out totrec);
            drpComplaintNo.DataSource = lstEmployee;
            drpComplaintNo.DataValueField = "pkID";
            drpComplaintNo.DataTextField = "ComplaintDateString";
            drpComplaintNo.DataBind();
            drpComplaintNo.Items.Insert(0, new ListItem("-- Select Complaint # --", "0"));
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
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                Int32 totrec = 0;
                List<Entity.Customer> lstCust = new List<Entity.Customer>();
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 10000, out totrec);
                if (lstCust.Count > 0)
                {
                    txtAddress.Text = lstCust[0].Address + ", " + lstCust[0].CityName + ", " + lstCust[0].StateName + ", " + lstCust[0].CountryName;
                }
                txtCustomerName.Text = lstEntity[0].CustomerName.ToString();
                drpVisitType.SelectedValue = lstEntity[0].VisitType.ToString();
                txtVisitNotes.Text = lstEntity[0].VisitNotes.ToString();
                if (!String.IsNullOrEmpty(lstEntity[0].VisitDate.ToString()) && lstEntity[0].VisitDate.Value.Year > 1900)
                    txtVisitDate.Text = lstEntity[0].VisitDate.Value.ToString("yyyy-MM-dd");
                else
                    txtVisitDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtTimeFrom.Text = lstEntity[0].TimeFrom.ToString();
                txtTimeTo.Text = lstEntity[0].TimeTo.ToString();
                drpVisitChargeType.SelectedValue = lstEntity[0].VisitChargeType.ToString();
                txtVisitCharge.Text = lstEntity[0].VisitCharge.ToString();
                drpStatus.SelectedValue = lstEntity[0].ComplaintStatus.ToString();

                imgDocument.ImageUrl = lstEntity[0].VisitDocument;
            }
        }
        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            Int32 totrec = 0;
            List<Entity.Customer> lstCust = new List<Entity.Customer>();
            lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 10000, out totrec);
            if (lstCust.Count > 0)
            {
                txtAddress.Text = lstCust[0].Address +", "+ lstCust[0].CityName +", "+ lstCust[0].StateName +", "+ lstCust[0].CountryName;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }
        public void ClearAllField()
        {
            hdnpkID.Value = "";
            hdnComplaintNo.Value = "";
            hdnParent.Value = "";
            hdnCustomerID.Value = "";
            hdnEmployeeID.Value = "";
            txtCustomerName.Text = "";
            txtVisitDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtTimeFrom.Text = DateTime.Now.ToString("hh:mm tt");
            txtTimeTo.Text = DateTime.Now.ToString("hh:mm tt");
            drpVisitType.SelectedValue = "Free";
            txtVisitNotes.Text = "";
            drpVisitChargeType.SelectedValue = "";
            txtVisitCharge.Text = "";
            drpComplaintNo.Items.Clear();
            txtCustomerName.Focus();
            btnSave.Disabled = false;
        }
        [System.Web.Services.WebMethod]
        public static string DeleteComplaintVisit(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            BAL.ComplaintMgmt.DeleteComplaintVisit(pkID, out ReturnCode, out ReturnMsg);
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ReturnCode = 0, ReturnpkID = 0;
            string ReturnMsg = "", ReturnComplaintNo = "";
            string strErr = "";

            _pageValid = true;

            if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0" ||
                drpComplaintNo.SelectedValue == "0" || String.IsNullOrEmpty(txtVisitNotes.Text) ||
                String.IsNullOrEmpty(txtVisitDate.Text) || 
                (drpVisitType.SelectedValue == "Charged" && (String.IsNullOrEmpty(drpVisitChargeType.SelectedValue) || String.IsNullOrEmpty(txtVisitCharge.Text) || txtVisitCharge.Text == "0")))
            {
                _pageValid = false;

                if (String.IsNullOrEmpty(txtCustomerName.Text) || String.IsNullOrEmpty(hdnCustomerID.Value) || hdnCustomerID.Value == "0")
                    strErr += "<li>" + "Customer Selection is required." + "</li>";

                if (drpComplaintNo.SelectedValue == "0")
                    strErr += "<li>" + "Complaint # is Required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitNotes.Text))
                    strErr += "<li>" + "Complaint Visit Notes is required." + "</li>";

                if (String.IsNullOrEmpty(txtVisitDate.Text))
                    strErr += "<li>" + "Visit Date is required." + "</li>";

                if (drpVisitType.SelectedValue == "Charged" && (String.IsNullOrEmpty(drpVisitChargeType.SelectedValue) || String.IsNullOrEmpty(txtVisitCharge.Text) || txtVisitCharge.Text == "0"))
                    strErr += "<li>" + "Charged Type and Amount is required for Charged Visit Type." + "</li>";
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
                objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.VisitDate = Convert.ToDateTime(txtVisitDate.Text);
                objEntity.TimeFrom = txtTimeFrom.Text;
                objEntity.TimeTo = txtTimeTo.Text;
                objEntity.VisitNotes = txtVisitNotes.Text;
                objEntity.VisitType = drpVisitType.Text;
                if (drpVisitType.SelectedValue == "Charged")
                {
                    objEntity.VisitChargeType = drpVisitChargeType.SelectedValue;
                    objEntity.VisitCharge = (!String.IsNullOrEmpty(txtVisitCharge.Text) && txtVisitCharge.Text != "0") ? Convert.ToDecimal(txtVisitCharge.Text) : 0;
                }
                objEntity.ComplaintStatus = drpStatus.SelectedValue;
                objEntity.VisitDocument = imgDocument.ImageUrl;
                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.ComplaintMgmt.AddUpdateComplaintVisit(objEntity, out ReturnCode, out ReturnMsg, out ReturnpkID, out ReturnComplaintNo);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {
                    btnSave.Disabled = true;
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

        protected void drpComplaintNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Entity.Complaint> lstEntity = new List<Entity.Complaint>();
            lstEntity = BAL.ComplaintMgmt.GetComplaintList(Convert.ToInt64(drpComplaintNo.SelectedValue), 0, "", Session["LoginUserID"].ToString());

            hdnpkID.Value = lstEntity[0].pkID.ToString();
            hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
            txtCustomerName.Text = lstEntity[0].CustomerName.ToString();

            if (hdnCustomerID.Value != "")
            { 
                Int32 totrec1 = 0;
                List<Entity.Customer> lstCust = new List<Entity.Customer>();
                lstCust = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 10000, out totrec1);
                if (lstCust.Count > 0)
                {
                    txtAddress.Text = lstCust[0].Address + ", " + lstCust[0].CityName + ", " + lstCust[0].StateName + ", " + lstCust[0].CountryName;
                }
            }
        }
    }
}