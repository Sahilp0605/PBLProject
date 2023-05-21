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
    public partial class CourierInfo : System.Web.UI.Page
    {
        bool _pageValid = true;
        string _pageErrMsg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtDetail = new DataTable();
                Session.Add("dtDetailInq", dtDetail);
                Session["PageNo"] = 1;
                Session["PageSize"] = 10;

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    hdnpkID.Value = Request.QueryString["id"].ToString();

                    if (hdnpkID.Value == "0" || hdnpkID.Value == "")
                    {
                        List<Entity.Country> lstEvents = new List<Entity.Country>();
                        lstEvents = BAL.CountryMgmt.GetCountryList();
                        drpCountry.DataSource = lstEvents;
                        drpCountry.DataValueField = "CountryCode";
                        drpCountry.DataTextField = "CountryName";
                        drpCountry.DataBind();
                        drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

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

            }
            else
            {
                var requestTarget = this.Request["__EVENTTARGET"];
                // ----------------------------------------------------------------------
                // Product Iamge Upload On .... Page Postback
                // ----------------------------------------------------------------------
                if (FileUpload1.PostedFile != null)
                {
                    if (FileUpload1.HasFile)
                    {
                        HttpFileCollection _HttpFileCollection = Request.Files;
                        for (int i = 0; i < _HttpFileCollection.Count; i++)
                        {
                            HttpPostedFile _HttpPostedFile = _HttpFileCollection[i];
                            if (_HttpPostedFile.ContentLength > 0)
                            {
                                //string filePath = FileUpload1.PostedFile.FileName;
                                string filePath = _HttpPostedFile.FileName;
                                string filename1 = Path.GetFileName(filePath);
                                //filename1 = filename1 + "-" + DateTime.Now.ToString();

                                string ext = Path.GetExtension(filename1).ToLower();
                                string type = String.Empty;

                                // ----------------------------------------------------------
                                if (ext == ".bmp" || ext == ".gif" || ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".pdf")
                                {
                                    try
                                    {
                                        string rootFolderPath = Server.MapPath("otherimages");
                                        string filesToDelete = @"courier-" + filename1 + ".*";   // Only delete DOC files containing "DeleteMe" in their filenames
                                        string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                        foreach (string file in fileList)
                                        {
                                            System.IO.File.Delete(file);
                                        }
                                        // -----------------------------------------------------
                                        String flname = "courier-" + DateTime.Now.ToString().Replace(" ","").Replace(":","")+"-" + filename1 ;
                                        String tmpFile = Server.MapPath("otherimages/") + flname;
                                        FileUpload1.PostedFile.SaveAs(tmpFile);
                                        // ---------------------------------------------------------------
                                        DataTable dtDocs = new DataTable();
                                        dtDocs = (DataTable)Session["dtDocs"];
                                        if(dtDocs == null)
                                            BindCourierImages("0");

                                        Int64 cntRow = dtDocs.Rows.Count + 1;
                                        DataRow dr = dtDocs.NewRow();
                                        dr["pkID"] = cntRow;
                                        dr["Name"] = flname;
                                        dr["Type"] = type;

                                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_HttpPostedFile.InputStream.ToString());
                                        string utfString = Convert.ToBase64String(plainTextBytes);

                                        System.IO.Stream fs = _HttpPostedFile.InputStream;
                                        System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

                                        dr["Data"] = base64String;

                                        dtDocs.Rows.Add(dr);
                                        dtDocs.AcceptChanges();
                                        Session.Add("dtDocs", dtDocs);
                                        // ---------------------------------------------------------------
                                        rptCourierImage.DataSource = dtDocs;
                                        rptCourierImage.DataBind();
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), Guid.NewGuid().ToString(), "javascript:showcaseError('Document Uploaded Successfully, Please Save Record  !');", true);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void BindCourierImages(String CourierNo)
        {
            DataTable dtDetail1 = new DataTable();
            List<Entity.CourierInfo> lst = BAL.CourierInfoMgmt.GetCourierImageList(0, CourierNo);
            dtDetail1 = PageBase.ConvertListToDataTable(lst);
            rptCourierImage.DataSource = dtDetail1;
            rptCourierImage.DataBind();
            Session.Add("dtDocs", dtDetail1);
        }

        public void OnlyViewControls()
        {
            txtSerialNo.ReadOnly = true;
            txtDocketNo.ReadOnly = true;
            txtActivityDate.ReadOnly = true;
            txtDocumentType.ReadOnly = true;
            drpModeOfSelection.Attributes.Add("disabled", "disabled");
            txtCustomerName.ReadOnly = true;
            txtCourierContact.ReadOnly = true;
            txtCourierEmail.ReadOnly = true;

            txtCourierName.ReadOnly = true;
            txtAddress.ReadOnly = true;
            txtPinCode.ReadOnly = true;

            txtRemarks.ReadOnly = true;

            drpCountry.Attributes.Add("disabled", "disabled");
            drpState.Attributes.Add("disabled", "disabled");
            drpCity.Attributes.Add("disabled", "disabled");

            btnSave.Visible = false;
            btnReset.Visible = false;
        }

        public void setLayout(string pMode)
        {
            if (pMode.ToLower() == "edit")
            {
                List<Entity.Country> lstEvents = new List<Entity.Country>();
                lstEvents = BAL.CountryMgmt.GetCountryList();
                drpCountry.DataSource = lstEvents;
                drpCountry.DataValueField = "CountryCode";
                drpCountry.DataTextField = "CountryName";
                drpCountry.DataBind();
                drpCountry.Items.Insert(0, new ListItem("-- All Country --", ""));

                int TotalCount = 0;
                // ----------------------------------------------------
                List<Entity.CourierInfo> lstEntity = new List<Entity.CourierInfo>();
                // ----------------------------------------------------
                lstEntity = BAL.CourierInfoMgmt.GetCourierInfoList(Convert.ToInt64(hdnpkID.Value), Session["LoginUserID"].ToString(), Convert.ToInt32(Session["PageNo"]), Convert.ToInt32(Session["PageSize"]), out TotalCount);
                hdnpkID.Value = lstEntity[0].pkID.ToString();
                txtSerialNo.Text = lstEntity[0].SerialNo;
                txtDocketNo.Text = lstEntity[0].DocketNo;
                txtActivityDate.Text = lstEntity[0].ActivityDate.ToString("yyyy-MM-dd");
                txtDocumentType.Text = lstEntity[0].DocumentType;
                drpModeOfSelection.SelectedValue = lstEntity[0].AcceptanceType;
                hdnCustomerID.Value = lstEntity[0].CustomerID.ToString();
                txtCustomerName.Text = lstEntity[0].CustomerName;
                txtCourierContact.Text = lstEntity[0].CourierContact;
                txtCourierEmail.Text = lstEntity[0].CourierEmail;
                txtCourierName.Text = lstEntity[0].CourierName;
                txtAddress.Text = lstEntity[0].Address.ToString();
                txtPinCode.Text = lstEntity[0].PinCode.ToString();
                txtRemarks.Text = lstEntity[0].Remarks;

                drpCountry.SelectedValue = lstEntity[0].Country;

                if (!String.IsNullOrEmpty(lstEntity[0].Country))
                {
                    drpState.Enabled = true;
                    drpCountry_SelectedIndexChanged(null, null);
                    drpState.SelectedValue = lstEntity[0].State;
                }

                if (!String.IsNullOrEmpty(lstEntity[0].State))
                {
                    drpCity.Enabled = true;
                    drpState_SelectedIndexChanged(null, null);
                    drpCity.SelectedValue = lstEntity[0].City;
                }
                //hdnPostedFile.Value = lstEntity[0].CourierImage;
                //imgProduct.ImageUrl = "otherimages/" + lstEntity[0].CourierImage;
                BindCourierImages(lstEntity[0].SerialNo);
                txtActivityDate.Focus();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int ReturnCode = 0, ReturnCode1 = 0;
            string ReturnMsg = "", ReturnMsg1 = "";
            string ReturnSerialNo = "";
            string strErr = "";

            if (String.IsNullOrEmpty(txtDocketNo.Text) || String.IsNullOrEmpty(txtActivityDate.Text) || String.IsNullOrEmpty(txtDocumentType.Text) || drpModeOfSelection.SelectedValue.ToLower() == "-- select --" ||
                String.IsNullOrEmpty(hdnCustomerID.Value) || String.IsNullOrEmpty(txtCustomerName.Text) ||
                String.IsNullOrEmpty(txtRemarks.Text) ||
                String.IsNullOrEmpty(txtAddress.Text) || String.IsNullOrEmpty(drpCountry.SelectedValue) || String.IsNullOrEmpty(drpState.SelectedValue) || String.IsNullOrEmpty(drpCity.SelectedValue))
            {
                _pageValid = false;

                if (drpModeOfSelection.SelectedValue.ToLower() == "-- select --")
                    strErr += "<li>" + "Mode Of Selection is required." + "</li>";

                if (String.IsNullOrEmpty(hdnCustomerID.Value) || String.IsNullOrEmpty(txtCustomerName.Text))
                    strErr += "<li>" + "Sender/Recipient Name is required." + "</li>";

                if (String.IsNullOrEmpty(txtActivityDate.Text))
                    strErr += "<li>" + "Activity Date is required." + "</li>";

                if (String.IsNullOrEmpty(txtDocumentType.Text))
                    strErr += "<li>" + "Parcel Type is required." + "</li>";

                if (String.IsNullOrEmpty(txtDocketNo.Text))
                    strErr += "<li>" + "Docket No is required." + "</li>";

                if (String.IsNullOrEmpty(txtAddress.Text))
                    strErr += "<li>" + "Address is required." + "</li>";

                if (String.IsNullOrEmpty(drpCountry.SelectedValue))
                    strErr += "<li>" + "Country selection is required." + "</li>";

                if (!String.IsNullOrEmpty(drpCountry.SelectedValue) && String.IsNullOrEmpty(drpState.SelectedValue))
                    strErr += "<li>" + "State selection is required." + "</li>";

                if (!String.IsNullOrEmpty(drpState.SelectedValue) && String.IsNullOrEmpty(drpCity.SelectedValue))
                    strErr += "<li>" + "City selection is required." + "</li>";

                if (String.IsNullOrEmpty(txtRemarks.Text))
                    strErr += "<li>" + "Remarks is required." + "</li>";
            }

            // -------------------------------------------------------------
            if (_pageValid)
            {
                // --------------------------------------------------------------
                Entity.CourierInfo objEntity = new Entity.CourierInfo();

                if (!String.IsNullOrEmpty(hdnpkID.Value))
                    objEntity.pkID = Convert.ToInt64(hdnpkID.Value);

                objEntity.SerialNo = txtSerialNo.Text;
                objEntity.DocketNo = txtDocketNo.Text;
                objEntity.ActivityDate = Convert.ToDateTime(txtActivityDate.Text);
                objEntity.DocumentType = txtDocumentType.Text;
                objEntity.AcceptanceType = drpModeOfSelection.SelectedValue;
                objEntity.CustomerID = Convert.ToInt64(hdnCustomerID.Value);
                objEntity.CourierContact = txtCourierContact.Text;
                objEntity.CourierEmail = txtCourierEmail.Text;
                objEntity.CourierName = txtCourierName.Text;
                objEntity.Address = txtAddress.Text;
                objEntity.PinCode = txtPinCode.Text;
                objEntity.Remarks = txtRemarks.Text;

                if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
                    objEntity.Country = drpCountry.SelectedValue;

                if (!String.IsNullOrEmpty(drpState.SelectedValue))
                    objEntity.State = drpState.SelectedValue;

                if (!String.IsNullOrEmpty(drpCity.SelectedValue))
                    objEntity.City = drpCity.SelectedValue;

                objEntity.LoginUserID = Session["LoginUserID"].ToString();
                // -------------------------------------------------------------- Insert/Update Record
                BAL.CourierInfoMgmt.AddUpdateCourierInfo(objEntity, out ReturnCode, out ReturnMsg, out ReturnSerialNo);
                // --------------------------------------------------------------
                strErr += "<li>" + ReturnMsg + "</li>";

                if (ReturnCode > 0)
                {

                    //if (!String.IsNullOrEmpty(hdnpkID.Value) && hdnpkID.Value != "0")
                    //{
                    //    if (!String.IsNullOrEmpty(hdnPostedFile.Value))
                    //    {
                    //        String flname = hdnPostedFile.Value;
                    //        string ext = Path.GetExtension(hdnPostedFile.Value).ToLower();
                    //        //FileUpload1.SaveAs(Server.MapPath("otherimages/") + flname);
                    //        imgProduct.ImageUrl = "";
                    //        imgProduct.ImageUrl = "otherimages/" + flname;
                    //        System.IO.File.Delete(Server.MapPath("otherimages/") + "courier-new" + ext);
                    //        BAL.CommonMgmt.UpdateCourierImage(txtSerialNo.Text, hdnPostedFile.Value);
                    //    }
                    //}
                    //else
                    //{
                    //    string ext = Path.GetExtension(hdnPostedFile.Value).ToLower();
                    //    String flname = "courier-" + ReturnSerialNo.Trim() + ext;
                    //    System.IO.File.Copy(Server.MapPath("otherimages/") + "courier-new" + ext, Server.MapPath("otherimages/") + flname, true);
                    //    System.IO.File.Delete(Server.MapPath("otherimages/") + "courier-new" + ext);
                    //    imgProduct.ImageUrl = "";
                    //    imgProduct.ImageUrl = "otherimages/" + flname;
                    //    BAL.CommonMgmt.UpdateCourierImage(ReturnSerialNo, flname);

                    //}

                    if (!String.IsNullOrEmpty(ReturnSerialNo))
                    {
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        // SAVE - Product Documents
                        // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                        BAL.CourierInfoMgmt.DeleteCourierImageByCourierID(ReturnSerialNo, out ReturnCode1, out ReturnMsg1);

                        string filePath, filename1, ext, type;
                        Byte[] bytes;
                        long COurierId;
                        DataTable dtDocs = new DataTable();
                        dtDocs = (DataTable)Session["dtDocs"];

                        if (dtDocs != null)
                        {
                            foreach (DataRow dr in dtDocs.Rows)
                            {
                                if (dr.RowState.ToString() != "Deleted")
                                {
                                    Entity.CourierInfo objEntity1 = new Entity.CourierInfo();
                                    objEntity1.CourierNo = ReturnSerialNo;
                                    objEntity1.Name = dr["Name"].ToString();
                                    objEntity1.Type = dr["type"].ToString();
                                    objEntity1.LoginUserID = Session["LoginUserID"].ToString();
                                    // -------------------------------------------------------------- Insert/Update Record
                                    BAL.CourierInfoMgmt.AddUpdateCourierImages(objEntity1, out ReturnCode1, out ReturnMsg1);
                                    strErr += "<li>" + ReturnMsg + "</li>";

                                    String flname = dr["Name"].ToString();
                                    String tmpFile = Server.MapPath("otherimages/") + flname;
                                    if (dr["Data"] != null && !String.IsNullOrEmpty(dr["Data"].ToString()))
                                        System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(dr["Data"].ToString()));
                                    else
                                        System.IO.File.WriteAllBytes(tmpFile, Convert.FromBase64String(ConvertImageToBase64(tmpFile)));
                                }
                                else
                                {
                                    string rootFolderPath = Server.MapPath("otherimages");
                                    string filesToDelete = dr["Name"].ToString();  
                                    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                    foreach (string file in fileList)
                                    {
                                        System.IO.File.Delete(file);
                                    }
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
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    // SAVE - Courier Image
                    // *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                    //if (txtSerialNo.Text == "0" || txtSerialNo.Text == "")
                    //{
                    //    string rootFolderPath = Server.MapPath("otherimages");
                    //    string filesToDelete = @"courier-" + txtSerialNo.Text.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
                    //    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                    //    foreach (string file in fileList)
                    //    {
                    //        string oldFileName = file;
                    //        string NewFileName = file.Replace(file.Substring(file.LastIndexOf(@"\") + 1, (file.LastIndexOf(".") - file.LastIndexOf(@"\") - 1)), "courier-" + hdnpkID.Value.Trim());
                    //        System.IO.File.Copy(oldFileName, NewFileName);
                    //        System.IO.File.Delete(file);
                    //    }
                    // -----------------------------------------------------


                    //}

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

        public string ConvertImageToBase64(string xFilePath)
        {
            string base64ImageRepresentation = "";
            byte[] imageArray = System.IO.File.ReadAllBytes(@xFilePath);
            base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearAllField();
        }

        public void ClearAllField()
        {
            hdnpkID.Value = "";
            txtSerialNo.Text = "";
            txtDocketNo.Text = "";
            txtActivityDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDocumentType.Text = "";
            drpModeOfSelection.SelectedValue = "-- Select --";
            hdnCustomerID.Value = "";
            txtCustomerName.Text = "";
            txtCourierContact.Text = "";
            txtCourierEmail.Text = "";
            txtRemarks.Text = "";

            txtCourierName.Text = "";
            txtAddress.Text = "";
            txtPinCode.Text = "";
            BindCourierImages("0");
            // ------------------------------------------------
            drpCity.Items.Clear();
            drpState.Items.Clear();

            if (drpCountry.Items.FindByText("India") != null)
            {
                drpCountry.Items.FindByText("India").Selected = true;
                drpCountry_SelectedIndexChanged(null, null);
            }

            if (drpState.Items.FindByText("Gujarat") != null)
            {
                drpState.Items.FindByText("Gujarat").Selected = true;
                drpState_SelectedIndexChanged(null, null);
            }

            btnSave.Disabled = false;

            drpModeOfSelection.Focus();
        }

        protected void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpState.Items.Clear();
            List<Entity.State> lstEvents = new List<Entity.State>();
            if (!String.IsNullOrEmpty(drpCountry.SelectedValue))
            {
                lstEvents = BAL.StateMgmt.GetStateList(Convert.ToString(drpCountry.SelectedValue));
            }
            else
            {
                lstEvents = BAL.StateMgmt.GetStateList();
            }

            drpState.DataSource = lstEvents;
            drpState.DataValueField = "StateCode";
            drpState.DataTextField = "StateName";
            drpState.DataBind();
            drpState.Items.Insert(0, new ListItem("-- All State --", ""));

        }

        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(drpState.SelectedValue))
            {
                if (Convert.ToInt64(drpState.SelectedValue) > 0)
                {
                    List<Entity.City> lstEvents = new List<Entity.City>();
                    lstEvents = BAL.CityMgmt.GetCityByState(Convert.ToInt64(drpState.SelectedValue));
                    drpCity.DataSource = lstEvents;
                    drpCity.DataValueField = "CityCode";
                    drpCity.DataTextField = "CityName";
                    drpCity.DataBind();
                    drpCity.Items.Insert(0, new ListItem("-- All City --", ""));
                    drpCity.Enabled = true;
                }

            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteCourierInfo(Int64 pkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row = new Dictionary<string, object>();

            int ReturnCode = 0;
            string ReturnMsg = "";
            // --------------------------------- Delete Record
            int TotalCount = 0;
            List<Entity.CourierInfo> lstEntity = new List<Entity.CourierInfo>();
            lstEntity = BAL.CourierInfoMgmt.GetCourierInfoList(pkID, "admin", 1, 1000, out TotalCount);
            if (lstEntity.Count > 0)
            {
                string rootFolderPath = System.Web.HttpContext.Current.Server.MapPath("otherimages");
                string filesToDelete = @"courier-" + lstEntity[0].SerialNo.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                foreach (string file in fileList)
                {
                    System.IO.File.Delete(file);
                }

                BAL.CourierInfoMgmt.DeleteCourierInfo(pkID, out ReturnCode, out ReturnMsg);
            }
            // --------------------------------- 
            row.Add("ReturnCode", ReturnCode);
            row.Add("ReturnMsg", ReturnMsg);
            rows.Add(row);

            return serializer.Serialize(rows);
        }

        protected void btnUpload1_Click(object sender, EventArgs e) { }

        //protected void btnDeleteImg_Click(object sender, EventArgs e)
        //{

        //    string rootFolderPath = Server.MapPath("otherimages");
        //    string filesToDelete = @"courier-" + hdnpkID.Value.Trim() + ".*";      // Only delete DOC files containing "DeleteMe" in their filenames
        //    string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
        //    foreach (string file in fileList)
        //    {
        //        System.IO.File.Delete(file);
        //    }
        //    imgProduct.ImageUrl = "~/images/no-figure.png";
        //    FileUpload1.Dispose();
        //}

        protected void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            //    int totrec = 0;

            //    List <Entity.Customer> lst = BAL.CustomerMgmt.GetCustomerList(Convert.ToInt64(hdnCustomerID.Value), Session["LoginUserID"].ToString(), 1, 9999, out totrec);
            //    txtAddress.Text = lst[0].Address.ToString();

            //    drpCountry.SelectedValue = (!String.IsNullOrEmpty(lst[0].CountryCode)) ? lst[0].CountryCode : "";
            //    if (!String.IsNullOrEmpty(lst[0].CountryCode))
            //    { 
            //        drpCountry_SelectedIndexChanged(null, null);
            //        drpState.SelectedValue = (!String.IsNullOrEmpty(lst[0].StateCode)) ? lst[0].StateCode : "0";
            //    }
            //    if (!String.IsNullOrEmpty(lst[0].StateCode))
            //    {
            //        drpState_SelectedIndexChanged(null, null);
            //        drpCity.SelectedValue = (!String.IsNullOrEmpty(lst[0].CityCode)) ? lst[0].CityCode : "0";
            //    }
            //    //drpCity.SelectedValue = String.IsNullOrEmpty(lst[0].CityCode) ? lst[0].CityCode : "0";
            //    txtPinCode.Text = (!String.IsNullOrEmpty(lst[0].Pincode)) ? lst[0].Pincode : "";
            //    txtContact.Text = (!String.IsNullOrEmpty(lst[0].ContactNo1)) ? lst[0].ContactNo1 : "";
            //    txtEmail.Text = (!String.IsNullOrEmpty(lst[0].EmailAddress)) ? lst[0].EmailAddress : "";
        }

        protected void rptCourierImage_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Delete")
            {
                DataTable dtDocs = (DataTable)Session["dtDocs"];
                for (int i = dtDocs.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtDocs.Rows[i];
                    if (dr["pkID"].ToString() == e.CommandArgument.ToString())
                        dr.Delete();
                }
                dtDocs.AcceptChanges();
                Session.Add("dtDocs", dtDocs);

                //frameDoc.Attributes.Remove("scr");
                //frameDoc.Attributes.Add("src", "images/buttons/Preview.png");

                rptCourierImage.DataSource = dtDocs;
                rptCourierImage.DataBind();
            }
            //if (e.CommandName.ToString() == "Preview")
            //{
            //    DataTable dtDocs = (DataTable)Session["dtDocs"];
            //    for (int i = dtDocs.Rows.Count - 1; i >= 0; i--)
            //    {
            //        DataRow dr = dtDocs.Rows[i];
            //        if (dr["pkID"].ToString() == e.CommandArgument.ToString())
            //        {
            //            string filePath = "otherimages/" + dr["Name"];
            //            //frameDoc.Attributes.Add("src", filePath);
            //        }
            //    }
            //}
        }
    }
}